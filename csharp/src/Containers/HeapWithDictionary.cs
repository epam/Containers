using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{



	/// <summary>
	///  Implementation of Priority Queue as Binary Heap(support actions with user's keys).
	/// </summary>
	/// <typeparam name="TValue">Type of Heap's elements.</typeparam>
	/// <typeparam name="TKey">Type of Heap's elements user keys</typeparam>
	public class HeapWithIndices<TValue, TKey> : ICollection, ICloneable, IEnumerable<HeapWithIndices<TValue, TKey>.HeapEntry>
	{
		internal HeapEntry[] elements;
		int[] keysPosition;
		int head = 0;
		IComparer<TValue> comparer;
		Dictionary<TKey, int> keys = new Dictionary<TKey, int>();
		internal int version = 0;
		internal bool isHeap = false;


		/// <summary>
		/// Heap's enumerator.
		/// </summary>
		public struct HeapWithIndicesEnumerator : IEnumerator<HeapWithIndices<TValue, TKey>.HeapEntry>
		{
			long version;
			HeapWithIndices<TValue, TKey> heap;
			int index;


			internal HeapWithIndicesEnumerator(HeapWithIndices<TValue, TKey> heap)
			{
				this.heap = heap;
				index = -1;
				version = heap.version;
			}

			/// <summary>
			/// Return current element of enumerator.
			/// </summary>
			public HeapWithIndices<TValue, TKey>.HeapEntry Current
			{
				get
				{
					if (heap.version != version) throw new InvalidOperationException("Collection was changed.");
					return heap.elements[index];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					if (heap.version != version) throw new InvalidOperationException("Collection was changed.");
					return heap.elements[index];
				}
			}

			/// <summary>
			/// Dispose this enumerator.
			/// </summary>
			public void Dispose()
			{
			}
			/// <summary>
			/// Return true if current element is not last in collection.
			/// </summary>
			/// <returns>True if current element is not last in collection.</returns>
			public bool MoveNext()
			{
				index++;
				if (index < heap.Count) return true;
				return false;
			}

			/// <summary>
			/// Reset this enumerator.
			/// </summary>
			public void Reset()
			{
				index = -1;
			}
		}


		/// <summary>
		/// If number of elements in heap is more than this value, than heap will be real heap.
		/// </summary>
		public int ListTreshold
		{
			get; internal set;
		}

		/// <summary>
		/// Heap entry.
		/// </summary>
		public struct HeapEntry
		{
			internal int key;
			/// <summary>
			/// Value of entry.
			/// </summary>
			public TValue Value
			{
				get; internal set;
			}
			/// <summary>
			/// Key of entry.
			/// </summary>
			public TKey Key
			{
				get; internal set;
			}
			internal HeapEntry(int key, TKey userKey, TValue value)
			{
				this.key = key;
				this.Value = value;
				this.Key = userKey;
			}

			public override string ToString()
			{
				return "Value: " + Value + " Key: " + Key;
			}
		}


		private int count = 0;
		/// <summary>
		/// Returns mumber of elements.
		/// </summary>
		public int Count
		{
			get { return count; }
		}


		/// <summary>
		/// Create new instance of heap.
		/// </summary>
		/// <param name="capacity">Start capacity.</param>
		/// <param name="comparer">Elements comparer.</param>
		/// <param name="listTreshhold">If number of elements in heap is more than this value, than heap will be real heap.</param>
		public HeapWithIndices(int capacity, IComparer<TValue> comparer, int listTreshhold) : this(capacity, comparer)
		{
			ListTreshold = listTreshhold;
		}



		/// <summary>
		/// Create new instance of heap.
		/// </summary>
		/// <param name="comparer">Elements comparer. If null then will used CompareTo.</param>
		public HeapWithIndices(IComparer<TValue> comparer = null)
		{
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			Array.Resize(ref elements, 100);
			Array.Resize(ref keysPosition, 100);
			for (int i = 0; i < 100; ++i) keysPosition[i] = -i - 2;
			ListTreshold = 10;
		}

		/// <summary>
		/// Create new instance of heap with start capacity.
		/// </summary>
		/// <param name="capacity">Start capacity.</param>
		/// <param name="comparer">Elements comparer. If null then will used CompareTo.</param>
		public HeapWithIndices(int capacity, IComparer<TValue> comparer = null)
		{
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			Array.Resize(ref elements, capacity);
			Array.Resize(ref keysPosition, capacity);
			for (int i = 0; i < capacity; ++i) keysPosition[i] = -i - 2;
			ListTreshold = 10;
		}


		/// <summary>
		/// Method returns "true" if key was found, "false" otherwise.
		/// </summary>
		/// <param name="key">Key of the value to find. Cannot be null.</param>
		/// <param name="returnValue">If the key is found, contains the value associated with the key upon method return. If the key is not found, contains default(TValue).</param>
		/// <returns>"true" if key was found, "false" otherwise.</returns>
		public bool TryGetValue(TKey key, out TValue returnValue)
		{
			int intKey = -11;
			if (keys.TryGetValue(key, out intKey))
			{
				returnValue = elements[keysPosition[intKey]].Value;
				return true;
			}
			returnValue = default(TValue);
			return false;
		}

		/// <summary>
		/// Add new element with user key.
		/// </summary>
		/// <param name="value">Value of new element.</param>
		/// <param name="key">User key of new element.</param>
		public bool Add(TValue value, TKey key)
		{
			version++;
			if (keys.ContainsKey(key)) return false;
			if (elements.Length == count)
			{
				Array.Resize(ref elements, elements.Length << 1);
			}
			if (isHeap) AddHeap(value, key); else AddList(value, key);
			return true;
		}

		void AddHeap(TValue value, TKey key)
		{
			int index = count;
			int next = (index - 1) >> 1;
			while (index > 0 && comparer.Compare(value, elements[next].Value) < 0)
			{
				keysPosition[elements[next].key] = index;
				elements[index] = elements[next];
				index = next;
				next = (index - 1) >> 1;
			}
			elements[index] = new HeapEntry(GetNextFreeKey(), key, value);
			keysPosition[elements[index].key] = index;
			keys.Add(key, elements[index].key);
			count++;
		}

		private int BinarySearch(TValue value)
		{
			if (count == 0) return 0;
			int l = 0;
			int r = count - 1;
			while (l < r)
			{
				int c = (l + r) / 2;
				if (comparer.Compare(value, elements[c].Value) < 0) l = c + 1; else r = c;
			}
			if (comparer.Compare(value, elements[l].Value) < 0) l++;
			return l;
		}
		private void RebuidToHeap()
		{
			Array.Reverse(elements, 0, count);
			for (int i = 0; i < count; ++i) keysPosition[elements[i].key] = i;
		}

		void AddList(TValue value, TKey userKey)
		{
			int insertIndex = BinarySearch(value);
			for (int k = count; k > insertIndex; k--)
			{
				elements[k] = elements[k - 1];
				keysPosition[elements[k].key] = k;
			}
			int key = GetNextFreeKey();
			elements[insertIndex] = new HeapEntry(key, userKey, value);
			keysPosition[key] = insertIndex;
			count++;
			keys.Add(userKey, key);
			if (count >= ListTreshold)
			{
				RebuidToHeap();
				isHeap = true;
			}
		}

		private int GetNextFreeKey()
		{
			int key = Math.Abs(head);
			if (key >= keysPosition.Length)
			{
				int oldLength = keysPosition.Length;
				Array.Resize(ref keysPosition, keysPosition.Length << 1);
				for (int i = oldLength; i < keysPosition.Length; ++i) keysPosition[i] = -i - 2;
			}
			head = -(keysPosition[head] + 1);
			return key;
		}

		/// <summary>
		/// Remove minimum from Heap.
		/// <returns> Minimum from heap.</returns>
		/// </summary>
		public HeapEntry Pop()
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap) return PopHeap(); else return PopList();
		}

		private HeapEntry PopHeap()
		{
			HeapEntry returnValue = elements[0];
			keys.Remove(elements[0].Key);
			int last = head;
			head = elements[0].key;
			keysPosition[head] = -last - 1;
			elements[0] = elements[count - 1];
			HeapEntry value = elements[count - 1];
			count--;
			int current = 0;
			if (count == 0) return returnValue;
			while (current < count)
			{
				int left = (current << 1) + 1;
				int right = (current << 1) + 2;
				int swapIndex = current;
				if (left > count) swapIndex = current;
				else
				if (right > count)
				{
					if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
				}
				else
				{
					if (comparer.Compare(elements[left].Value, elements[right].Value) < 0)
					{
						if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
					}
					else if (comparer.Compare(elements[right].Value, value.Value) < 0) swapIndex = right;
				}
				if (swapIndex == current) break;
				elements[current] = elements[swapIndex];
				keysPosition[elements[current].key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].key] = current;
			return returnValue;
		}

		private HeapEntry PopList()
		{
			count--;
			int last = head;
			head = elements[count].key;
			keysPosition[head] = -last - 1;
			keys.Remove(elements[count].Key);
			return elements[count];
		}


		/// <summary>
		/// Modify element of heap with userKey.
		/// </summary>
		/// <param name="userKey">UserKey of element.</param>
		/// <param name="newValue">New value of element.</param>
		/// <returns>True if heap contains userKey.</returns>
		public bool Modify(TKey userKey, TValue newValue)
		{
			version++;
			if (isHeap) return ModifyHeap(userKey, newValue);
			else return ModifyList(userKey, newValue);
		}

		private bool ModifyHeap(TKey userKey, TValue newValue)
		{
			int key = -1;
			if (!keys.TryGetValue(userKey, out key)) return false;
			int index = keysPosition[key];
			HeapEntry value = new HeapEntry(elements[keysPosition[key]].key, elements[keysPosition[key]].Key, newValue);
			int current = index;
			while (current < count)
			{
				int left = (current << 1) + 1;
				int right = (current << 1) + 2;
				int swapIndex = current;
				if (left >= count) swapIndex = current;
				else
				if (right >= count)
				{
					if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
				}
				else
				{
					if (comparer.Compare(elements[left].Value, elements[right].Value) < 0)
					{
						if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
					}
					else if (comparer.Compare(elements[right].Value, value.Value) < 0) swapIndex = right;
				}
				if (swapIndex == current) break;
				elements[current] = elements[swapIndex];
				keysPosition[elements[current].key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].key] = current;
			current = index;
			value = elements[current];
			while (current != 0)
			{
				int swapIndex = (current - 1) >> 1;
				if (comparer.Compare(value.Value, elements[swapIndex].Value) < 0)
				{
					elements[current] = elements[swapIndex];
					keysPosition[elements[current].key] = current;
					current = swapIndex;
				}
				else break;
			}
			if (current != index)
			{
				elements[current] = value;
				keysPosition[elements[current].key] = current;
			}
			return true;
		}

		private bool ModifyList(TKey userKey, TValue newValue)
		{
			int key = -1;
			if (!keys.TryGetValue(userKey, out key)) return false;
			int index = keysPosition[key];
			if (index < 0) return false;
			for (int i = index; i < count; ++i)
			{
				elements[i] = elements[i + 1];
				keysPosition[elements[i].key] = i;
			}
			count--;
			int insertIndex = BinarySearch(newValue);
			for (int i = count; i >= insertIndex + 1; --i)
			{
				elements[i] = elements[i - 1];
				keysPosition[elements[i].key] = keysPosition[elements[i - 1].key];
			}
			elements[insertIndex] = new HeapEntry(key, userKey, newValue);
			keysPosition[key] = insertIndex;
			count++;
			return true;
		}



		/// <summary>
		/// Modify top element.
		/// </summary>
		/// <param name="newValue">New value of top element.</param>
		/// <returns>Old top element.</returns>
		public HeapEntry ModifyTop(TValue newValue)
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap) return ModifyTopHeap(newValue);
			else return ModifyTopList(newValue);
		}

		private HeapEntry ModifyTopHeap(TValue newValue)
		{
			HeapEntry returnValue = elements[0];
			HeapEntry value = new HeapEntry(elements[0].key, elements[0].Key, newValue);
			int current = 0;
			while (current < count)
			{
				int left = (current << 1) + 1;
				int right = (current << 1) + 2;
				int swapIndex = current;
				if (left >= count) swapIndex = current;
				else
				if (right >= count)
				{
					if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
				}
				else
				{
					if (comparer.Compare(elements[left].Value, elements[right].Value) < 0)
					{
						if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
					}
					else if (comparer.Compare(elements[right].Value, value.Value) < 0) swapIndex = right;
				}
				if (swapIndex == current) break;
				elements[current] = elements[swapIndex];
				keysPosition[elements[current].key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].key] = current;
			return returnValue;
		}

		private HeapEntry ModifyTopList(TValue newValue)
		{
			HeapEntry returnValue = elements[count - 1];
			int insertIndex = BinarySearch(newValue);
			if (insertIndex >= count) insertIndex = count - 1;
			for (int i = count - 1; i > insertIndex; i--)
			{
				elements[i] = elements[i - 1];
				keysPosition[elements[i].key] = i;
			}
			elements[insertIndex].Key = returnValue.Key;
			elements[insertIndex].Value = newValue;
			elements[insertIndex].key = returnValue.key;
			keysPosition[returnValue.key] = insertIndex;
			return returnValue;

		}

		/// <summary>
		/// Remove element by user key.
		/// </summary>
		/// <param name="userKey">user key.</param>
		public bool Remove(TKey userKey)
		{
			version++;
			if (isHeap) return RemoveHeap(userKey);
			else return RemoveList(userKey);
		}


		private bool RemoveHeap(TKey userKey)
		{
			int key = -1;
			if (!keys.TryGetValue(userKey, out key)) return false;
			int index = keysPosition[key];
			keys.Remove(userKey);
			int last = head;
			head = elements[index].key;
			keysPosition[head] = -last - 1;
			HeapEntry value = elements[count - 1];
			count--;
			int current = index;
			if (count == 0) return true;
			if (index == count) return true;
			while (current < count)
			{
				int left = (current << 1) + 1;
				int right = (current << 1) + 2;
				int swapIndex = current;
				if (left > count) swapIndex = current;
				else
				if (right > count)
				{
					if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
				}
				else
				{
					if (comparer.Compare(elements[left].Value, elements[right].Value) < 0)
					{
						if (comparer.Compare(elements[left].Value, value.Value) < 0) swapIndex = left;
					}
					else if (comparer.Compare(elements[right].Value, value.Value) < 0) swapIndex = right;
				}
				if (swapIndex == current) break;
				elements[current] = elements[swapIndex];
				keysPosition[elements[current].key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].key] = current;
			current = index;
			value = elements[current];
			while (current != 0)
			{
				int swapIndex = (current - 1) >> 1;
				if (comparer.Compare(value.Value, elements[swapIndex].Value) < 0)
				{
					elements[current] = elements[swapIndex];
					keysPosition[elements[current].key] = current;
					current = swapIndex;
				}
				else break;
			}
			if (current != index)
			{
				elements[current] = value;
				keysPosition[elements[current].key] = current;
			}
			return true;
		}

		private bool RemoveList(TKey userKey)
		{
			int key = -1;
			if (!keys.TryGetValue(userKey, out key)) return false;
			keys.Remove(userKey);
			int index = keysPosition[key];
			int last = head;
			head = elements[index].key;
			keysPosition[head] = -last - 1;
			count--;
			for (int i = index; i < count; ++i)
			{
				elements[i] = elements[i + 1];
				keysPosition[elements[i].key] = i;
			}
			return true;
		}

		/// <summary>
		/// Return minimum element.
		/// </summary>
		public HeapEntry Peek
		{
			get
			{
				if (count > 0)
				{
					if (isHeap) return elements[0]; else return elements[count - 1];
				}

				else throw new Exception("Heap is empty");
			}
		}

		/// <summary>
		/// Return value of minimum element.
		/// </summary>
		public TValue PeekValue
		{
			get
			{
				if (count > 0)
				{
					if (isHeap) return elements[0].Value; else return elements[count - 1].Value;
				}

				else throw new Exception("Heap is empty");
			}
		}

		/// <summary>
		/// Return key of minimum element.
		/// </summary>
		public TKey PeekKey
		{
			get
			{
				if (count > 0)
				{
					if (isHeap) return elements[0].Key; else return elements[count - 1].Key;
				}

				else throw new Exception("Heap is empty");
			}

		}




		/// <summary>
		/// Return true if heap contains value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>True if heap contains value.</returns>
		public bool Contains(TValue value)
		{
			for (int i = 0; i < count; ++i) if (comparer.Compare(value, elements[i].Value) == 0) return true;
			return false;
		}

		/// <summary>
		/// Return true if heap contains key.
		/// </summary>
		/// <param name="key">User key.</param>
		/// <returns>True if heap contains key.</returns>
		public bool ContainsKey(TKey key)
		{
			return keys.ContainsKey(key);
		}



		/// <summary>
		/// Gets an object that can be used to synchronize access to the System.Collections.ICollection.
		/// An object that can be used to synchronize access to the System.Collections.ICollection.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return elements.SyncRoot;
			}
		}
		/// <summary>
		///     Gets a value indicating whether access to the System.Collections.ICollection
		///     is synchronized (thread safe).
		///     True if access to the System.Collections.ICollection is synchronized (thread
		///     safe); otherwise, false.
		/// </summary>

		public bool IsSynchronized
		{
			get
			{
				return elements.IsSynchronized;
			}
		}
		/// <summary>
		/// Copy to array.
		/// </summary>
		/// <param name="array">Destination's array.</param>
		/// <param name="index">start index.</param>
		public void CopyTo(Array array, int index)
		{
			elements.CopyTo(array, index);
		}

		/// <summary>
		/// Copy elements of heap to new array.
		/// </summary>
		/// <returns>Array with elements of heap.</returns>
		public HeapEntry[] ToArray()
		{
			HeapEntry[] destinationArray = new HeapEntry[count];
			for (int i = 0; i < count; ++i) destinationArray[i] = elements[i];
			return destinationArray;
		}




		public HeapWithIndicesEnumerator GetEnumerator()
		{
			return new HeapWithIndicesEnumerator(this);
		}



		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An System.Collections.IEnumerator object that can be used to iterate through the collection</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new HeapWithIndicesEnumerator(this);
		}

		/// <summary>
		/// Return clone of element.
		/// </summary>
		/// <returns>Clone of elements.</returns>
		public object Clone()
		{
			HeapWithIndices<TValue, TKey> clone = new HeapWithIndices<TValue, TKey>();
			clone.count = count;
			clone.elements = new HeapEntry[elements.Length];
			clone.head = head;
			clone.keysPosition = new int[keysPosition.Length];
			clone.isHeap = isHeap;
			clone.ListTreshold = ListTreshold;
			Array.Copy(keysPosition, clone.keysPosition, keysPosition.Length);
			Array.Copy(elements, clone.elements, count);
			clone.keys = new Dictionary<TKey, int>();
			foreach (var item in keys) clone.keys.Add(item.Key, item.Value);
			clone.comparer = this.comparer;
			return clone;
		}

		IEnumerator<HeapWithIndices<TValue, TKey>.HeapEntry> IEnumerable<HeapWithIndices<TValue, TKey>.HeapEntry>.GetEnumerator()
		{
			return new HeapWithIndicesEnumerator(this);

		}
	}


}

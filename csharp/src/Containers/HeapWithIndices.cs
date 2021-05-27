/*
  Copyright 2021 EPAM Systems, Inc

  See the NOTICE file distributed with this work for additional information
  regarding copyright ownership. Licensed under the Apache License,
  Version 2.0 (the "License"); you may not use this file except in compliance
  with the License.  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
  License for the specific language governing permissions and limitations under
  the License.
 */
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{



	/// <summary>
	/// Implementation of Priority Queue as Binary Heap(support actions with our keys).
	/// </summary>
	/// <typeparam name="TValue">Type of Heap's elements.</typeparam>
	public class HeapWithIndices<TValue> : ICollection, ICloneable, IEnumerable<HeapWithIndices<TValue>.HeapEntry>
	{


		internal HeapEntry[] elements;
		int[] keysPosition;
		int head = 0;
		IComparer<TValue> comparer;
		internal bool isHeap = false;
		internal int version = 0;

		/// <summary>
		/// Heap's enumerator.
		/// </summary>
		public struct HeapWithIndicesEnumerator : IEnumerator<HeapWithIndices<TValue>.HeapEntry>
		{
			long version;
			HeapWithIndices<TValue> heap;
			int index;


			internal HeapWithIndicesEnumerator(HeapWithIndices<TValue> heap)
			{
				this.heap = heap;
				index = -1;
				version = heap.version;
			}


			/// <summary>
			/// Return current element of enumerator.
			/// </summary>
			public HeapWithIndices<TValue>.HeapEntry Current
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
			/// Dispose enumerator.
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
			/// Reset enumerator.
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
		/// Heap's entry.
		/// </summary>
		public struct HeapEntry
		{
			/// <summary>
			/// Our key of entry.
			/// </summary>
			public int Key { get; internal set; }
			/// <summary>
			/// Entry's value.
			/// </summary>
			public TValue Value
			{
				get; internal set;
			}
			internal HeapEntry(int key, TValue value)
			{
				this.Key = key;
				this.Value = value;
			}
			public override string ToString()
			{
				return "Value: " + Value + " Key: " + Key;
			}
		}

		/// <summary>
		/// Method returns "true" if key was found, "false" otherwise.
		/// </summary>
		/// <param name="key">Key of the value to find. Cannot be null.</param>
		/// <param name="returnValue">If the key is found, contains the value associated with the key upon method return. If the key is not found, contains default(TValue).</param>
		/// <returns>"true" if key was found, "false" otherwise.</returns>
		public bool TryGetValue(int key, out TValue returnValue)
		{
			if (key >= 0 && key < keysPosition.Length && keysPosition[key] >= 0)
			{
				returnValue = elements[keysPosition[key]].Value;
				return true;
			}
			returnValue = default(TValue);
			return false;
		}

		private int count = 0;
		/// <summary>
		/// Returns number of elements.
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
		/// <param name="listTreshold">If number of elements in heap is more than this value, than heap will be real heap.</param>
		public HeapWithIndices(int capacity, IComparer<TValue> comparer, int listTreshold) : this(capacity, comparer)
		{
			ListTreshold = listTreshold;
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
			ListTreshold = 10;
			for (int i = 0; i < 100; ++i) keysPosition[i] = -i - 2;
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
			ListTreshold = 10;
			for (int i = 0; i < capacity; ++i) keysPosition[i] = -i - 2;
		}

		/// <summary>
		/// Add element to the heap. 
		/// </summary>
		/// <param name="value">Element to add.</param>
		/// <returns>Key for this element.</returns>
		public int Add(TValue value)
		{
			version++;
			if (elements.Length == count)
			{
				Array.Resize(ref elements, elements.Length << 1);
			}
			if (isHeap) return AddHeap(value);
			else return AddList(value);
		}

		private int AddHeap(TValue value)
		{
			int index = count;
			int next = (index - 1) >> 1;
			while (index > 0 && comparer.Compare(value, elements[next].Value) < 0)
			{
				keysPosition[elements[next].Key] = index;
				elements[index] = elements[next];
				index = next;
				next = (index - 1) >> 1;
			}
			elements[index] = new HeapEntry(GetNextFreeKey(), value);
			keysPosition[elements[index].Key] = index;
			count++;
			return elements[index].Key;
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

		private int AddList(TValue value)
		{
			int insertIndex = BinarySearch(value);
			for (int k = Count; k > insertIndex; k--)
			{
				elements[k] = elements[k - 1];
				keysPosition[elements[k].Key] = k;
			}
			int key = GetNextFreeKey();
			elements[insertIndex] = new HeapEntry(key, value);
			keysPosition[key] = insertIndex;
			count++;
			if (count == ListTreshold)
			{
				RebuidToHeap();
				isHeap = true;
			}
			return key;
		}

		private void RebuidToHeap()
		{
			Array.Reverse(elements, 0, count);
			for (int i = 0; i < count; ++i) keysPosition[elements[i].Key] = i;
			isHeap = true;
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
		/// Remove minimum from the heap and return it;
		/// <returns>Minimum from heap.</returns>
		/// </summary>
		public HeapEntry Pop()
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap) return PopHeap();
			else return PopList();
		}


		private HeapEntry PopList()
		{
			count--;
			int last = head;
			head = elements[count].Key;
			keysPosition[head] = -last - 1;
			return elements[count];
		}
		private HeapEntry PopHeap()
		{
			HeapEntry returnValue = elements[0];
			int last = head;
			head = elements[0].Key;
			keysPosition[head] = -last - 1;
			elements[0] = elements[count - 1];
			HeapEntry value = elements[count - 1];
			count--;
			if (count == 0) return returnValue;
			int current = 0;
			while (current < count)
			{
				int left = (current << 1) + 1;
				int right = (current << 1) + 2;
				int swapIndex = current;
				if (left > count)
				{
					swapIndex = current;
				}
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
				keysPosition[elements[current].Key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].Key] = current;
			return returnValue;
		}


		/// <summary>
		/// Modify element of heap with userKey.
		/// </summary>
		/// <param name="key">UserKey of element.</param>
		/// <param name="newValue">New value of element.</param>
		/// <returns>True if heap contains userKey.</returns>
		public bool Modify(int key, TValue newValue)
		{
			version++;
			if (isHeap) return ModifyHeap(key, newValue); else return ModifyList(key, newValue);
		}

		private bool ModifyList(int key, TValue newValue)
		{
			if (key >= keysPosition.Length || key < 0 || keysPosition[key] < 0) return false;
			int index = keysPosition[key];
			if (index < 0) return false;
			for (int i = index; i < count - 1; ++i)
			{
				elements[i] = elements[i + 1];
				keysPosition[elements[i].Key] = i;
			}
			count--;
			int insertIndex = BinarySearch(newValue);
			for (int i = count; i >= insertIndex + 1; i--)
			{
				elements[i] = elements[i - 1];
				keysPosition[elements[i].Key] = i;
			}
			elements[insertIndex] = new HeapEntry(key, newValue);
			keysPosition[key] = insertIndex;
			count++;
			return true;
		}

		private bool ModifyHeap(int key, TValue newValue)
		{
			if (key >= keysPosition.Length || key < 0 || keysPosition[key] < 0) return false;
			int index = keysPosition[key];
			HeapEntry value = new HeapEntry(elements[keysPosition[key]].Key, newValue);
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
				keysPosition[elements[current].Key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].Key] = current;
			current = index;
			value = elements[current];
			while (current != 0)
			{
				int swapIndex = (current - 1) >> 1;
				if (comparer.Compare(value.Value, elements[swapIndex].Value) < 0)
				{
					elements[current] = elements[swapIndex];
					keysPosition[elements[current].Key] = current;
					current = swapIndex;
				}
				else break;
			}
			if (current != index)
			{
				elements[current] = value;
				keysPosition[elements[current].Key] = current;
			}
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
			HeapEntry value = new HeapEntry(elements[0].Key, newValue);
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
				keysPosition[elements[current].Key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].Key] = current;
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
				keysPosition[elements[i].Key] = i;
			}
			elements[insertIndex].Key = returnValue.Key;
			elements[insertIndex].Value = newValue;
			keysPosition[returnValue.Key] = insertIndex;
			return returnValue;
		}


		/// <summary>
		/// Remove element by key.
		/// </summary>
		/// <param name="key">Key of element which we want to delete.</param>
		public bool Remove(int key)
		{
			version++;
			if (key < 0 || key >= keysPosition.Length || keysPosition[key] < 0) return false;
			if (isHeap) RemoveHeap(key);
			else RemoveList(key);
			return true;
		}


		private void RemoveList(int key)
		{
			int index = keysPosition[key];
			int last = head;
			head = elements[index].Key;
			keysPosition[head] = -last - 1;
			count--;
			for (int i = index; i < count; ++i)
			{
				elements[i] = elements[i + 1];
				keysPosition[elements[i].Key] = i;
			}
		}

		private void RemoveHeap(int key)
		{
			int index = keysPosition[key];
			int last = head;
			head = elements[index].Key;
			keysPosition[head] = -last - 1;
			HeapEntry value = elements[count - 1];
			count--;
			if (index == count) return;
			if (count == 0) return;
			int current = index;
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
				keysPosition[elements[current].Key] = current;
				current = swapIndex;
			}
			elements[current] = value;
			keysPosition[elements[current].Key] = current;
			current = index;
			value = elements[current];
			while (current != 0)
			{
				int swapIndex = (current - 1) >> 1;
				if (comparer.Compare(value.Value, elements[swapIndex].Value) < 0)
				{
					elements[current] = elements[swapIndex];
					keysPosition[elements[current].Key] = current;
					current = swapIndex;
				}
				else break;
			}
			if (current != index)
			{
				elements[current] = value;
				keysPosition[elements[current].Key] = current;
			}
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
					if (isHeap) return elements[0];
					else return elements[count - 1];
				}
				else throw new Exception("Heap is empty");
			}
		}

		/// <summary>
		/// Return key of minimum element.
		/// </summary>
		public int PeekKey
		{
			get
			{
				if (count > 0)
				{
					if (isHeap) return elements[0].Key;
					else return elements[count - 1].Key;
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
					if (isHeap) return elements[0].Value;
					else return elements[count - 1].Value;
				}
				else throw new Exception("Heap is empty");
			}
		}




		/// <summary>
		/// Return true if heap contains key.
		/// </summary>
		/// <param name="key">Our key.</param>
		/// <returns>True if heap contains key.</returns>
		public bool ContainsKey(int key)
		{
			if (key < 0 || key >= keysPosition.Length) return false;
			return keysPosition[key] >= 0;
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
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An System.Collections.IEnumerator object that can be used to iterate through the collection</returns>
		HeapWithIndicesEnumerator GetEnumerator()
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
			HeapWithIndices<TValue> clone = new HeapWithIndices<TValue>();
			clone.count = count;
			clone.elements = new HeapEntry[elements.Length];
			clone.head = head;
			clone.isHeap = isHeap;
			clone.ListTreshold = ListTreshold;
			clone.keysPosition = new int[keysPosition.Length];
			Array.Copy(keysPosition, clone.keysPosition, keysPosition.Length);
			Array.Copy(elements, clone.elements, count);
			clone.comparer = this.comparer;
			return clone;
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

		IEnumerator<HeapWithIndices<TValue>.HeapEntry> IEnumerable<HeapWithIndices<TValue>.HeapEntry>.GetEnumerator()
		{
			return new HeapWithIndicesEnumerator(this);
		}
	}


}
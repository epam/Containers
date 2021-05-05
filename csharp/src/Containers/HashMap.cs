using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{


	struct HashMapEnumerator<K, V> : IEnumerator<KeyValuePair<K, V>>
	{
		Boolean first;
		long iterator;
		HashMap<K, V> hashMap;

		public HashMapEnumerator(HashMap<K, V> map)
		{
			hashMap = map;
			iterator = -1;
			first = true;
		}

		public KeyValuePair<K, V> Current
		{
			get
			{
				return new KeyValuePair<K, V>(hashMap.GetKeyAt(iterator), hashMap.GetValueAt(iterator));
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return new KeyValuePair<K, V>(hashMap.GetKeyAt(iterator), hashMap.GetValueAt(iterator));
			}
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (first)
			{
				iterator = hashMap.FirstIterator;
				first = false;
			}
			else
			{
				iterator = hashMap.GetNext(iterator);
			}
			if (iterator != -1) return true;
			return false;
		}

		public void Reset()
		{
			first = true;
			iterator = -1;
		}
	}



	/**
	 * Public class for HashMap. Key is Object, Value is Object.
	 */

	public class HashMap<K, V> : IEnumerable<KeyValuePair<K, V>>
	{

		/// <summary>
		/// Pointer to empty element.
		/// </summary>
		public static int NoElement = -1;
		private static int NonEmptyFlag = Int32.MinValue;


		V defaultValue;

		List<K> keys;
		List<V> values;
		int[] first;
		int[] next;

		bool allocedPlaceWasFilled = true;
		int head;
		int capacity;
		int count = 0;
		long reservedSpace = NoElement;



		/// <summary>
		/// Node of list.
		/// </summary>
		public struct Node
		{
			internal long iterator;
			internal HashMap<K, V> hashMap;
			K key;
			V value;

			internal Node(HashMap<K, V> hashMap, long iterator)
			{
				this.hashMap = hashMap;
				this.iterator = iterator;
				this.key = hashMap.GetKeyAt(iterator);
				this.value = hashMap.GetValueAt(iterator);
			}

			/// <summary>
			/// Return data in this Node.
			/// </summary>
			public KeyValuePair<K, V> Data
			{
				get
				{
					return new KeyValuePair<K, V>(hashMap.GetKeyAt(iterator), hashMap.GetValueAt(iterator));
				}
			}

			/// <summary>
			/// Key of this element.
			/// </summary>
			public K Key
			{
				get
				{
					return key;
				}

				set
				{
					hashMap.SetKeyAt(iterator, value);
				}
			}

			/// <summary>
			/// Value of this element.
			/// </summary>
			public V Value
			{
				get
				{
					return value;
				}

				set
				{
					hashMap.SetValueAt(iterator, value);
				}
			}

			/// <summary>
			/// Remove this element from hashmap.
			/// </summary>
			/// <returns>The element follows this.</returns>
			public Node? Remove()
			{
				long nextIterator = hashMap.RemoveAt(iterator);
				if (nextIterator == NoElement) return null;
				else return new Node(hashMap, nextIterator);
			}


			/// <summary>
			/// Return node follows this.
			/// </summary>
			public Node? Next
			{
				get
				{
					long nextIterator = hashMap.GetNext(iterator);
					if (nextIterator == NoElement) return null;
					else return new Node(hashMap, nextIterator);
				}
			}
		}


		/// <summary>
		/// Node for unsafe enumeration.
		/// </summary>
		public struct UnsafeNode
		{
			internal int iterator;
			internal HashMap<K, V> hashMap;
			K key;
			V value;

			internal UnsafeNode(HashMap<K, V> hashMap, int iterator)
			{
				this.hashMap = hashMap;
				this.iterator = iterator;
				this.key = hashMap.GetKeyByUnsafeIterator(iterator);
				this.value = hashMap.GetValueByUnsafeIterator(iterator);
			}

			/// <summary>
			/// Return data in this Node.
			/// </summary>
			public KeyValuePair<K, V> Data
			{
				get
				{
					return new KeyValuePair<K, V>(hashMap.GetKeyByUnsafeIterator(iterator), hashMap.GetValueByUnsafeIterator(iterator));
				}
			}

			/// <summary>
			/// Key of this element.
			/// </summary>
			public K Key
			{
				get
				{
					return key;
				}

			}

			/// <summary>
			/// Value of this element.
			/// </summary>
			public V Value
			{
				get
				{
					return value;
				}

			}



			/// <summary>
			/// Return node follows this.
			/// </summary>
			public UnsafeNode? Next
			{
				get
				{
					int nextIterator = hashMap.GetUnsafeNext(iterator);
					if (nextIterator == NoElement) return null;
					else return new UnsafeNode(hashMap, nextIterator);
				}
			}
		}


		/// <summary>
		/// Reserved by locateOrReserve empty space.
		/// </summary>
		public long ReservedSpace
		{
			get
			{
				return reservedSpace;
			}
		}

		int HashFunction(K value)
		{
			return Math.Abs(value.GetHashCode() % capacity);
		}

		/// <summary>
		/// Return number of elements in hashmap.
		/// </summary>		
		public int Count
		{
			get
			{
				return count;
			}
		}

		/// <summary>
		/// Capacity of hashmap.
		/// </summary>
		public int Capacity
		{
			get
			{
				return capacity;
			}
			set
			{
				if (capacity >= value) return;
				Rebuild(value);
			}
		}

		/// <summary>
		/// Create instance of hashmap. 
		/// </summary>
		/// <param name="startCapacity">Start capacity of hashmap.</param>
		/// <param name="defaultValue">Default value. Used as return-value for some methods.</param>
		public HashMap(int startCapacity, V defaultValue)
		{
			this.defaultValue = defaultValue;
			keys = new List<K>(startCapacity);
			for (int i = 0; i < startCapacity; ++i)
			{
				keys.Add(default(K));
			}
			values = new List<V>(startCapacity);
			for (int i = 0; i < startCapacity; ++i)
			{
				values.Add(defaultValue);
			}
			first = new int[startCapacity];
			next = new int[startCapacity];
			for (int i = 0; i < startCapacity; ++i)
			{
				next[i] = -i - 2;
			}
			for (int i = 0; i < startCapacity; ++i)
			{
				first[i] = -1;
			}
			capacity = startCapacity;
		}

		/// <summary>
		/// Create instance of hashmap. 
		/// </summary>
		/// <param name="defaultValue">Default value. Used as return-value for some methods.</param>
		public HashMap(V defaultValue) : this(8, defaultValue)
		{
		}

		void Rebuild(int newCapacity)
		{
			int oldCapacity = capacity;
			capacity = newCapacity;
			while (values.Count < capacity)
			{
				values.Add(defaultValue);
			}
			while (keys.Count < capacity)
			{
				keys.Add(default(K));
			}
			Array.Resize(ref first, capacity);
			Array.Resize(ref next, capacity);
			for (int i = 0; i < capacity; ++i)
			{
				next[i] = -i - 2;
			}
			for (int i = 0; i < capacity; ++i)
			{
				first[i] = -1;
			}
			head = 0;
			count = 0;
			for (int i = 0; i < oldCapacity; ++i)
			{
				this[keys[i]] = values[i];
			}
		}

		/// <summary>
		/// This operator give access to element with such key to you.
		/// Setter adds new element with key and value if there is no element with such key.
		/// Overwrite only value if there is element with such key in map.
		/// </summary>
		/// <param name="key">Key of element.</param>
		/// <returns>Value of element with key or default if there is no element with such key.</returns>
		public V this[K key]
		{
			get
			{
				int hash = HashFunction(key);
				int current = first[hash];
				while (current >= 0)
				{
					if (keys[current].Equals(key))
					{
						return values[current];
					}
					current = next[current];
				}
				return defaultValue;
			}
			set
			{
				if (count == capacity)
				{
					Rebuild(capacity << 1);
				}
				int hash = HashFunction(key);
				int current = first[hash];
				while (current >= 0)
				{
					if (keys[current].Equals(key))
					{
						values[current] = value;
						return;
					}
					current = next[current];
				}
				int last = head;
				keys[last] = key;
				values[last] = value;
				head = -(next[last] + 1);
				next[last] = first[hash];
				if (next[last] < 0)
				{
					next[last] = NonEmptyFlag;
				}
				first[hash] = last;
				count++;
			}
		}


		/// <summary>
		/// Try to add element with key and value. 
		/// </summary>
		/// <param name="key">Key of element.</param>
		/// <param name="value">Value of element.</param>
		/// <returns>True if we can add this element(there is no element with such key). False otherwise.</returns>
		public bool TrySet(K key, V value)
		{
			if (count == capacity)
			{
				Rebuild(capacity << 1);
			}
			int hash = HashFunction(key);
			int current = first[hash];
			while (current >= 0)
			{
				if (keys[current].Equals(key))
				{
					return false;
				}
				current = next[current];
			}
			int last = head;
			keys[last] = key;
			values[last] = value;
			head = -(next[last] + 1);
			next[last] = first[hash];
			if (next[last] < 0)
			{
				next[last] = NonEmptyFlag;
			}
			first[hash] = last;
			count++;
			return true;
		}

		/// <summary>
		/// Add element with key and value to HashMap. If there is element with such key than this method overwrite only value. 
		/// </summary>
		/// <param name="key">Key of element.</param>
		/// <param name="value">Value of element.</param>
		/// <returns>Old value of element or default value(if key not exists).</returns>		
		public V SetAndGet(K key, V value)
		{
			if (count == capacity)
			{
				Rebuild(capacity << 1);
			}
			int hash = HashFunction(key);
			int current = first[hash];
			while (current >= 0)
			{
				if (keys[current].Equals(key))
				{
					V oldValue = values[current];
					values[current] = value;
					return oldValue;
				}
				current = next[current];
			}
			int last = head;
			keys[last] = key;
			values[last] = value;
			head = -(next[last] + 1);
			next[last] = first[hash];
			if (next[last] < 0)
			{
				next[last] = NonEmptyFlag;
			}
			first[hash] = last;
			count++;
			return defaultValue;
		}


		/// <summary>
		/// Remove element with such key from hashmap. 
		/// </summary>
		/// <param name="key">Key of element to delete.</param>
		/// <returns>True if element exists. False otherwise.</returns>
		public bool TryRemove(K key)
		{
			int hash = HashFunction(key);
			int current = first[hash];
			int previous = -1;
			while (current >= 0)
			{
				if (keys[current].Equals(key))
				{
					int last = head;
					head = current;
					if (current == first[hash])
					{
						first[hash] = next[current];
					}
					else
					{
						next[previous] = next[current];
					}
					next[head] = -last - 1;
					count--;
					return true;
				}
				previous = current;
				current = next[current];
			}
			return false;
		}

		/// <summary>
		/// Remove element from hashmap.
		/// </summary>
		/// <param name="key">Key of element to delete.</param>
		/// <returns>Value of removed element or default(if there is no key to remove).</returns>
		public V Remove(K key)
		{
			int hash = HashFunction(key);
			int current = first[hash];
			int previous = -1;
			while (current >= 0)
			{
				if (keys[current].Equals(key))
				{
					int last = head;
					head = current;
					if (current == first[hash])
					{
						first[hash] = next[current];
					}
					else
					{
						next[previous] = next[current];
					}
					next[head] = -last - 1;
					count--;
					return values[current];
				}
				previous = current;
				current = next[current];
			}
			return defaultValue;
		}

		/// <summary>
		/// Remove all elements from hashmap.
		/// </summary>
		public void Clear()
		{
			count = 0;
			allocedPlaceWasFilled = true;
			head = 0;
			for (int i = 0; i < next.Length; ++i)
			{
				next[i] = -i - 2;
			}
			for (int i = 0; i < first.Length; ++i)
			{
				first[i] = -1;
			}
		}


		private long GetIterator(int hash, int place)
		{
			return (long)hash | ((long)place << 32);
		}

		private int GetPlace(long iterator)
		{
			return (int)(iterator >> 32);
		}

		private int GetHash(long iterator)
		{
			return (int)(iterator & (0x00000000ffffffffL));
		}


		/// <summary>
		/// Return true if hashmap contains key. 
		/// </summary>
		/// <param name="key">Key to find.</param>
		/// <returns>True if hashmap contains key.</returns>
		public bool ContainsKey(K key)
		{
			return Locate(key) != NoElement;
		}


		/// <summary>
		/// Find iterator of element with key in HashMap. 
		/// </summary>
		/// <param name="key">Key to find.</param>
		/// <returns>Iterator of element with key in HashMap (NO_ELEMENT if key not existing).</returns>
		public long Locate(K key)
		{
			int hash = HashFunction(key);
			int current = first[hash];
			while (current >= 0)
			{
				if (keys[current].Equals(key))
				{
					return GetIterator(hash, current);
				}
				current = next[current];
			}
			return NoElement;
		}

		/// <summary>
		/// Find iterator of element with key in HashMap of allocate empty space for element with such key.
		/// You should fill allocated space before next usage of this method.
		/// </summary>
		/// <param name="key">Key to find.</param>
		/// <returns>Index of key (or allocated space) for element.</returns>
		public long LocateOrReserve(K key)
		{
			if (!allocedPlaceWasFilled)
			{
				throw new InvalidOperationException("You try to allocate new empty space before filling old empty space");
			}
			if (count == capacity)
			{
				Rebuild(capacity << 1);
			}

			int hash = HashFunction(key);
			int current = first[hash];
			while (current >= 0)
			{
				if (keys[current].Equals(key))
				{
					reservedSpace = NoElement;
					return GetIterator(hash, current);
				}
				current = next[current];
			}

			int last = head;
			head = -(next[last] + 1);
			next[last] = first[hash];
			if (next[last] < 0)
			{
				next[last] = NonEmptyFlag;
			}
			first[hash] = last;
			count++;
			allocedPlaceWasFilled = false;
			reservedSpace = GetIterator(hash, last);
			return reservedSpace;
		}

		internal long FirstIterator
		{
			get
			{
				for (int i = 0; i < capacity; ++i)
				{
					if (first[i] >= 0)
					{
						return GetIterator(i, first[i]);
					}
				}
				return NoElement;
			}
		}

		/// <summary>
		/// Return first element of hash map.
		/// </summary>
		public Node? First
		{
			get
			{
				for (int i = 0; i < capacity; ++i)
				{
					if (first[i] >= 0)
					{
						return new Node(this, GetIterator(i, first[i]));
					}
				}
				return null;
			}
		}


		/// <summary>
		/// Return first element (unsafe enumeration) of hash map.
		/// </summary>
		public UnsafeNode? UnsafeFirst
		{
			get
			{
				int iterator = GetUnsafeFirstIterator();
				if (iterator == NoElement) return null;
				return new UnsafeNode(this, iterator);
			}
		}

		internal int GetUnsafeFirstIterator()
		{
			for (int i = 0; i < keys.Count; ++i)
			{
				if (next[i] >= 0 || next[i] == NonEmptyFlag)
				{
					return i;
				}
			}
			return NoElement;
		}

		internal int GetUnsafeNext(int unsafeIterator)
		{
			for (int i = unsafeIterator + 1; i < keys.Count; ++i)
			{
				if (next[i] >= 0 || next[i] == NonEmptyFlag)
				{
					return i;
				}
			}
			return NoElement;
		}

		internal K GetKeyByUnsafeIterator(int iterator)
		{
			return keys[iterator];
		}

		internal V GetValueByUnsafeIterator(int iterator)
		{
			return values[iterator];
		}

		/// <summary>
		/// Return iterator of element follows by given.
		/// </summary>
		/// <param name="iterator">Iterator to element.</param>
		/// <returns>Iterator of element follows by given.</returns>
		internal long GetNext(long iterator)
		{
			int place = GetPlace(iterator);
			int hash = GetHash(iterator);
			if (next[place] < 0)
			{
				hash++;
				while (hash < capacity && first[hash] < 0)
				{
					hash++;
				}
				if (hash == capacity)
				{
					return NoElement;
				}
				else
				{
					return GetIterator(hash, first[hash]);
				}
			}
			else
			{
				return GetIterator(hash, next[place]);
			}
		}

		/// <summary>
		/// Remove element by iterator. 
		/// </summary>
		/// <param name="iterator">Iterator of element.</param>
		/// <returns>Iterator of element follows by given</returns>
		internal long RemoveAt(long iterator)
		{
			if (iterator == NoElement)
			{
				throw new KeyNotFoundException("You try to delete element by incorrect iterator");
			}

			int previous = -1;
			long nxt = GetNext(iterator);
			int hash = GetHash(iterator);
			int current = first[hash];
			int itPlace = GetPlace(iterator);
			while (next[current] >= 0 && current != itPlace)
			{
				previous = current;
				current = next[current];
			}
			int last = head;
			head = current;
			if (itPlace == first[hash])
			{
				first[hash] = next[current];
			}
			else
			{
				next[previous] = next[current];
			}
			next[current] = -last - 1;
			count--;
			return nxt;
		}

		/// <summary>
		/// Return key of element by iterator.
		/// </summary>
		/// <param name="iterator">iterator Iterator of element.</param>
		/// <returns>Key of element by iterator.</returns>
		internal K GetKeyAt(long iterator)
		{
			int place = GetPlace(iterator);
			return keys[place];
		}

		/// <summary>
		/// Return value of element by iterator. 
		/// </summary>
		/// <param name="iterator">Iterator of element.</param>
		/// <returns>Value of element by iterator.</returns>
		internal V GetValueAt(long iterator)
		{
			int place = GetPlace(iterator);
			return values[place];
		}

		/// <summary>
		/// Set value of element by iterator. 
		/// </summary>
		/// <param name="iterator">Iterator of element.</param>
		/// <param name="value">New Value of element.</param>
		internal void SetValueAt(long iterator, V value)
		{
			int place = GetPlace(iterator);
			values[place] = value;
		}

		/// <summary>
		/// Set key of element by iterator.
		/// </summary>
		/// <param name="iterator">Iterator of element.</param>
		/// <param name="value">New key to set.</param>
		internal void SetKeyAt(long iterator, K value)
		{
			if (iterator == reservedSpace)
			{
				allocedPlaceWasFilled = true;
			}
			int place = GetPlace(iterator);
			keys[place] = value;
		}

		/// <summary>
		/// Get enumerator to this collection
		/// </summary>
		/// <returns>Enumerator to this collection</returns>
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return new HashMapEnumerator<K, V>(this);
		}


		/// <summary>
		/// Get enumerator to this collection
		/// </summary>
		/// <returns>Enumerator to this collection</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new HashMapEnumerator<K, V>(this);
		}

	}
}


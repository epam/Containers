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
using System.Text;

namespace EPAM.Deltix.Containers{
	struct UnorderedHashSetEnumerator<K> : IEnumerator<K>
	{
		Boolean first;
		long iterator;
		UnorderedHashSet<K> hashSet;

		public UnorderedHashSetEnumerator(UnorderedHashSet<K> set)
		{
			hashSet = set;
			iterator = -1;
			first = true;
		}

		public K Current
		{
			get
			{
				return hashSet.GetKeyAt(iterator);
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return hashSet.GetKeyAt(iterator);
			}
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (first)
			{
				iterator = hashSet.FirstIterator;
				first = false;
			}
			else
			{
				iterator = hashSet.GetNext(iterator);
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
	 * Public class for HashSet. Key is Object.
	 */

	public class UnorderedHashSet<K> : IEnumerable<K>
	{

		/// <summary>
		/// Pointer to empty element.
		/// </summary>
		public static int NoElement = -1;
		
		List<K> keys;
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
			internal UnorderedHashSet<K> hashSet;
			K key;

			internal Node(UnorderedHashSet<K> hashSet, long iterator)
			{
				this.hashSet = hashSet;
				this.iterator = iterator;
				this.key = hashSet.GetKeyAt(iterator);
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
					hashSet.SetKeyAt(iterator, value);
				}
			}

			
			/// <summary>
			/// Remove this element from hashSet.
			/// </summary>
			/// <returns>The element follows this.</returns>
			public Node? Remove()
			{
				long nextIterator = hashSet.RemoveAt(iterator);
				if (nextIterator == NoElement) return null;
				else return new Node(hashSet, nextIterator);
			}


			/// <summary>
			/// Return node follows this.
			/// </summary>
			public Node? Next
			{
				get
				{
					long nextIterator = hashSet.GetNext(iterator);
					if (nextIterator == NoElement) return null;
					else return new Node(hashSet, nextIterator);
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
		/// Return number of elements in HashSet.
		/// </summary>		
		public int Count
		{
			get
			{
				return count;
			}
		}

		/// <summary>
		/// Capacity of HashSet.
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
		/// Create instance of HashSet. 
		/// </summary>
		/// <param name="startCapacity">Start capacity of HashSet</param>
		public UnorderedHashSet(int startCapacity)
		{
			keys = new List<K>(startCapacity);
			for (int i = 0; i < startCapacity; ++i)
			{
				keys.Add(default(K));
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
		/// Create instance of HashSet. 
		/// </summary>
		public UnorderedHashSet() : this(8)
		{
		}

		void Rebuild(int newCapacity)
		{
			int oldCapacity = capacity;
			capacity = newCapacity;
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
				Put(keys[i]);
			}
		}


		/// <summary>
		/// Add element to HashSet. Overwrite key if it exists.
		/// </summary>
		/// <param name="key">Key of element.</param>
		/// <returns>True if we can add this element(there is no element with such key). False otherwise.</returns>
		public void Put(K key)
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
					keys[current] = key;
					return;
				}
				current = next[current];
			}
			int last = head;
			keys[last] = key;
			head = -(next[last] + 1);
			next[last] = first[hash];
			first[hash] = last;
			count++;
		}

		/// <summary>
		/// Try to add element to HashSet. 
		/// </summary>
		/// <param name="key">Key of element.</param>
		/// <returns>True if we can add this element(there is no element with such key). False otherwise.</returns>
		public bool TryPut(K key)
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
			head = -(next[last] + 1);
			next[last] = first[hash];
			first[hash] = last;
			count++;
			return true;
		}




		/// <summary>
		/// Remove element with such key from HashSet. 
		/// </summary>
		/// <param name="key">Key of element to delete.</param>
		/// <returns>True if element exists. False otherwise.</returns>
		public bool Remove(K key)
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
		/// Remove all elements from HashSet.
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
		/// Return true if HashSet contains key. 
		/// </summary>
		/// <param name="key">Key to find.</param>
		/// <returns>True if HashSet contains key.</returns>
		public bool Contains(K key)
		{
			return Locate(key) != NoElement;
		}


		/// <summary>
		/// Find iterator of element with key in HashSet. 
		/// </summary>
		/// <param name="key">Key to find.</param>
		/// <returns>Iterator of element with key in HashSet (NO_ELEMENT if key not existing).</returns>
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
		/// Find iterator of element with key in HashSet of allocate empty space for element with such key.
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
		/// Return first element of hash set.
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
		public IEnumerator<K> GetEnumerator()
		{
			return new UnorderedHashSetEnumerator<K>(this);
		}

		/// <summary>
		/// Get enumerator to this collection
		/// </summary>
		/// <returns>Enumerator to this collection</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new UnorderedHashSetEnumerator<K>(this);
		}

	}
}
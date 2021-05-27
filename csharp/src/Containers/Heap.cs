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




	internal class DefaultComparer<TValue> : Comparer<TValue>
	{
		public override int Compare(TValue x, TValue y)
		{
			if (x is IComparable<TValue>)
			{
				IComparable<TValue> x1 = (IComparable<TValue>)x;
				return x1.CompareTo(y);
			}
			throw new Exception("There is no CompareTo for this type");
		}
	}
	/// <summary>
	/// Implementation of Priority Queue as Binary Heap.
	/// </summary>
	/// <typeparam name="TValue">Type of Heap's elements.</typeparam>
	public class Heap<TValue> : ICollection, ICloneable, IEnumerable<TValue>
	{
		internal TValue[] elements;
		IComparer<TValue> comparer;
		internal bool isHeap = false;
		private int count = 0;
		internal long version = 0;
		/// <summary>
		/// Returns mumber of elements.
		/// </summary>
		public int Count
		{
			get { return count; }
		}

		/// <summary>
		/// If number of elements in heap is more than this value, than heap will be real heap.
		/// </summary>
		public int ListTreshold { get; internal set; }

		/// <summary>
		/// Clear the heap.
		/// </summary>
		public void Clear()
		{
			version++;
			count = 0;
		}

		/// <summary>
		/// Enumerator for heap.
		/// </summary>
		public struct HeapEnumerator : IEnumerator<TValue>
		{
			long version;
			Heap<TValue> heap;
			int index;


			internal HeapEnumerator(Heap<TValue> heap)
			{
				this.heap = heap;
				index = -1;
				version = heap.version;
			}

			/// <summary>
			/// Current value of enumerator.
			/// </summary>
			public TValue Current
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
			/// Dispose method.
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
		/// Create new instance of heap.
		/// </summary>
		/// <param name="capacity">Start capacity.</param>
		/// <param name="comparer">Elements comparer.</param>
		/// <param name="listTreshhold">If number of elements in heap is more than this value, than heap will be real heap.</param>
		public Heap(int capacity, IComparer<TValue> comparer, int listTreshhold) : this(capacity, comparer)
		{
			ListTreshold = listTreshhold;
		}



		/// <summary>
		/// Create new instance of heap.
		/// </summary>
		/// <param name="comparer">Elements comparer. If null then will used CompareTo.</param>
		public Heap(IComparer<TValue> comparer = null)
		{
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			ListTreshold = 10;
			isHeap = false;
			Array.Resize(ref elements, 100);
		}

		/// <summary>
		/// Create new instance of heap with start capacity.
		/// </summary>
		/// <param name="capacity">Start capacity.</param>
		/// <param name="comparer">Elements comparer. If null the will used CompareTo</param>
		public Heap(int capacity, IComparer<TValue> comparer = null)
		{
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			ListTreshold = 10;
			isHeap = false;
			Array.Resize(ref elements, capacity);
		}

		/// <summary>
		/// Add new element to heap.
		/// </summary>
		/// <param name="value">Value of element</param>
		public void Add(TValue value)
		{
			version++;
			if (isHeap) AddHeap(value);
			else AddList(value);
		}

		private int BinarySearch(TValue value)
		{
			if (count == 0) return 0;
			int l = 0;
			int r = Count - 1;
			while (l < r)
			{
				int c = (l + r) / 2;
				if (comparer.Compare(value, elements[c]) < 0) l = c + 1; else r = c;
			}
			if (comparer.Compare(value, elements[l]) < 0) l++;
			return l;

		}

		private void AddList(TValue value)
		{
			if (Count == elements.Length) Array.Resize(ref elements, elements.Length * 2);
			int insertIndex = BinarySearch(value);
			for (int k = Count; k > insertIndex; k--) elements[k] = elements[k - 1];
			elements[insertIndex] = value;
			count++;
			if (count > ListTreshold)
			{
				RebuildToHeap();
			}
		}

		private void RebuildToHeap()
		{
			Array.Reverse(elements, 0, count);
			isHeap = true;
		}

		private void AddHeap(TValue value)
		{
			if (elements.Length == count)
			{
				Array.Resize(ref elements, elements.Length << 1);
			}
			int index = count;
			int next = (index - 1) >> 1;
			while (index > 0 && comparer.Compare(value, elements[next]) < 0)
			{
				elements[index] = elements[next];
				index = next;
				next = (index - 1) >> 1;
			}
			elements[index] = value;
			count++;
		}

		/// <summary>
		/// Remove minimum from heap.
		/// <returns> Minimum</returns>
		/// </summary>
		public TValue Pop()
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap)
			{
				TValue returnValue = elements[0];
				TValue value = elements[count - 1];
				count--;
				int current = 0;
				while (current < count)
				{
					int left = (current << 1) + 1;
					int right = (current << 1) + 2;
					int swapIndex = current;
					if (left > count) swapIndex = current;
					else
					if (right > count)
					{
						if (comparer.Compare(elements[left], value) < 0) swapIndex = left;
					}
					else
					{
						if (comparer.Compare(elements[left], elements[right]) < 0)
						{
							if (comparer.Compare(elements[left], value) < 0) swapIndex = left;
						}
						else if (comparer.Compare(elements[right], value) < 0) swapIndex = right;
					}
					if (swapIndex == current) break;
					elements[current] = elements[swapIndex];
					current = swapIndex;
				}
				elements[current] = value;
				return returnValue;
			}
			else
			{
				count--;
				return elements[count];
			}
		}

		/// <summary>
		/// Modify top element.
		/// </summary>
		/// <param name="value">New value of top element.</param>
		/// <returns>Old top element.</returns>
		public TValue ModifyTop(TValue value)
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap)
			{
				TValue returnValue = elements[0];
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
						if (comparer.Compare(elements[left], value) < 0) swapIndex = left;
					}
					else
					{
						if (comparer.Compare(elements[left], elements[right]) < 0)
						{
							if (comparer.Compare(elements[left], value) < 0) swapIndex = left;
						}
						else if (comparer.Compare(elements[right], value) < 0) swapIndex = right;
					}
					if (swapIndex == current) break;
					elements[current] = elements[swapIndex];
					current = swapIndex;
				}
				elements[current] = value;
				return returnValue;
			}
			else
			{
				TValue returnValue = Pop();
				AddList(value);
				return returnValue;
			}
		}


		/// <summary>
		/// Copy elements of heap to new array.
		/// </summary>
		/// <returns>Array with elements of heap.</returns>
		public TValue[] ToArray()
		{
			TValue[] destinationArray = new TValue[count];
			for (int i = 0; i < count; ++i) destinationArray[i] = elements[i];
			return destinationArray;
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
		public HeapEnumerator GetEnumerator()
		{
			return new HeapEnumerator(this);
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An System.Collections.IEnumerator object that can be used to iterate through the collection</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new HeapEnumerator(this);
		}

		/// <summary>
		/// Return clone of element.
		/// </summary>
		/// <returns>Clone of elements.</returns>
		public object Clone()
		{
			Heap<TValue> clone = new Heap<TValue>();
			clone.count = count;
			clone.isHeap = isHeap;
			clone.ListTreshold = ListTreshold;
			clone.elements = new TValue[elements.Length];
			Array.Copy(elements, clone.elements, count);
			clone.comparer = this.comparer;
			return clone;
		}

		/// <summary>
		/// Return minimum element from heap.
		/// </summary>
		public TValue Peek
		{
			get
			{
				if (count == 0) throw new Exception("Heap is empty");
				if (isHeap) return elements[0]; else return elements[count - 1];
			}
		}

		/// <summary>
		/// Return true if heap contains value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>True if heap contains value.</returns>
		public bool Contains(TValue value)
		{
			for (int i = 0; i < count; ++i) if (comparer.Compare(value, elements[i]) == 0) return true;
			return false;
		}

		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
		{
			return new HeapEnumerator(this);
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
	}

}
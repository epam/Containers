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
	/// Implementation of Priority Queue as Binary Heap.
	/// </summary>
	/// <typeparam name="TValue">Type of Heap's elements.</typeparam>
	/// <typeparam name="TAttachments">Type of attachemnts to heap's elements</typeparam>
	public class HeapWithAttachments<TValue, TAttachments> : ICollection, ICloneable, IEnumerable<HeapWithAttachments<TValue, TAttachments>.HeapEntry>
	{
		internal HeapEntry[] elements;
		IComparer<TValue> comparer;
		internal long version = 0;
		private int count = 0;


		internal bool isHeap = false;

		/// <summary>
		/// If number of elements in heap is more than this value, than heap will be real heap.
		/// </summary>
		public int ListTreshold
		{
			get; internal set;
		}


		/// <summary>
		/// Returns mumber of elements.
		/// </summary>
		public int Count
		{
			get { return count; }
		}

		/// <summary>
		/// Clear the heap.
		/// </summary>
		public void Clear()
		{
			count = 0;
		}

		/// <summary>
		/// Return heap enumerator.
		/// </summary>
		public struct HeapEnumerator : IEnumerator<HeapWithAttachments<TValue, TAttachments>.HeapEntry>
		{
			long version;
			HeapWithAttachments<TValue, TAttachments> heap;
			int index;


			internal HeapEnumerator(HeapWithAttachments<TValue, TAttachments> heap)
			{
				this.heap = heap;
				index = -1;
				version = heap.version;
			}

			/// <summary>
			/// Return current element of enumerator.
			/// </summary>
			public HeapWithAttachments<TValue, TAttachments>.HeapEntry Current
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
		/// Entry for heap
		/// </summary>
		public struct HeapEntry
		{
			/// <summary>
			/// Value of HeapEntry.
			/// </summary>
			public TValue Value
			{
				get; internal set;
			}
			/// <summary>
			/// Attachment of HeapEntry.
			/// </summary>
			public TAttachments Attachment
			{
				get; internal set;
			}
			/// <summary>
			/// Create instance of HeapEntry.
			/// </summary>
			/// <param name="value">Value of HeapEntry</param>
			/// <param name="attachment">attachment of HeapEntry</param>
			public HeapEntry(TValue value, TAttachments attachment)
			{
				this.Value = value;
				this.Attachment = attachment;
			}

			public override string ToString()
			{
				return "Value: " + Value + " Attachments: " + Attachment;
			}
		}

		/// <summary>
		/// Create new instance of heap.
		/// </summary>
		/// <param name="capacity">Start capacity.</param>
		/// <param name="comparer">Elements comparer.</param>
		/// <param name="listTreshhold">If number of elements in heap is more than this value, than heap will be real heap.</param>
		public HeapWithAttachments(int capacity, IComparer<TValue> comparer, int listTreshhold) : this(capacity, comparer)
		{
			ListTreshold = listTreshhold;
		}



		/// <summary>
		/// Create new instance of heap.
		/// </summary>
		/// <param name="comparer">Elements comparer. If null then will used CompareTo.</param>
		public HeapWithAttachments(IComparer<TValue> comparer = null)
		{
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			ListTreshold = 10;
			Array.Resize(ref elements, 100);
		}

		/// <summary>
		/// Create new instance of heap with start capacity.
		/// </summary>
		/// <param name="capacity">Start capacity.</param>
		/// <param name="comparer">Elements comparer. If null the will used CompareTo</param>
		public HeapWithAttachments(int capacity, IComparer<TValue> comparer = null)
		{
			if (comparer == null) this.comparer = new DefaultComparer<TValue>();
			else this.comparer = comparer;
			ListTreshold = 10;
			Array.Resize(ref elements, capacity);
		}

		/// <summary>
		/// Add new element to heap.
		/// </summary>
		/// <param name="value">Value of element</param>
		/// <param name="attachments">Attachment of element</param>
		public void Add(TValue value, TAttachments attachments)
		{
			version++;
			if (elements.Length == count)
			{
				Array.Resize(ref elements, elements.Length << 1);
			}
			if (isHeap) AddHeap(value, attachments);
			else AddList(value, attachments);
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

		private void AddList(TValue value, TAttachments attachments)
		{
			int insertIndex = BinarySearch(value);
			for (int k = Count; k > insertIndex; k--) elements[k] = elements[k - 1];
			elements[insertIndex] = new HeapEntry(value, attachments);
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

		private void AddHeap(TValue value, TAttachments attachments)
		{
			int index = count;
			int next = (index - 1) >> 1;
			while (index > 0 && comparer.Compare(value, elements[next].Value) < 0)
			{
				elements[index] = elements[next];
				index = next;
				next = (index - 1) >> 1;
			}
			elements[index].Value = value;
			elements[index].Attachment = attachments;
			count++;
		}

		/// <summary>
		/// Remove minimum from heap.
		/// <returns> Minimum</returns>
		/// </summary>
		public HeapEntry Pop()
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap)
			{
				HeapEntry returnValue = elements[0];
				HeapEntry value = elements[count - 1];
				count--;
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
		public HeapEntry ModifyTop(TValue value)
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap)
			{
				HeapEntry returnValue = elements[0];
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
						if (comparer.Compare(elements[left].Value, value) < 0) swapIndex = left;
					}
					else
					{
						if (comparer.Compare(elements[left].Value, elements[right].Value) < 0)
						{
							if (comparer.Compare(elements[left].Value, value) < 0) swapIndex = left;
						}
						else if (comparer.Compare(elements[right].Value, value) < 0) swapIndex = right;
					}
					if (swapIndex == current) break;
					elements[current] = elements[swapIndex];
					current = swapIndex;
				}
				elements[current].Value = value;
				elements[current].Attachment = returnValue.Attachment;
				return returnValue;
			}
			else
			{
				HeapEntry returnValue = elements[count - 1];
				Pop();
				Add(value, returnValue.Attachment);
				return returnValue;
			}
		}

		/// <summary>
		/// Modify top element.
		/// </summary>
		/// <param name="value">New value of top element.</param>
		/// <param name="attachments">New attachments of top element.</param>
		/// <returns>Old top element.</returns>
		public HeapEntry ModifyTop(TValue value, TAttachments attachments)
		{
			version++;
			if (count == 0) throw new Exception("Heap is empty");
			if (isHeap)
			{
				HeapEntry returnValue = elements[0];
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
						if (comparer.Compare(elements[left].Value, value) < 0) swapIndex = left;
					}
					else
					{
						if (comparer.Compare(elements[left].Value, elements[right].Value) < 0)
						{
							if (comparer.Compare(elements[left].Value, value) < 0) swapIndex = left;
						}
						else if (comparer.Compare(elements[right].Value, value) < 0) swapIndex = right;
					}
					if (swapIndex == current) break;
					elements[current] = elements[swapIndex];
					current = swapIndex;
				}
				elements[current].Value = value;
				elements[current].Attachment = attachments;
				return returnValue;
			}
			else
			{
				HeapEntry returnValue = elements[count - 1];
				Pop();
				Add(value, attachments);
				return returnValue;
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
			HeapWithAttachments<TValue, TAttachments> clone = new HeapWithAttachments<TValue, TAttachments>();
			clone.count = count;
			clone.elements = new HeapEntry[elements.Length];
			clone.isHeap = isHeap;
			clone.ListTreshold = ListTreshold;
			Array.Copy(elements, clone.elements, count);
			clone.comparer = this.comparer;
			return clone;
		}

		/// <summary>
		/// Return minimum element from heap.
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
		/// Return minimum element value from heap.
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
		/// Return minimum element attachments from heap.
		/// </summary>
		public TAttachments PeekAttachments
		{
			get
			{
				if (count > 0)
				{
					if (isHeap) return elements[0].Attachment; else return elements[count - 1].Attachment;
				}
				else throw new Exception("Heap is empty");
			}
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

		IEnumerator<HeapEntry> IEnumerable<HeapEntry>.GetEnumerator()
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
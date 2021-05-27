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
	/// <summary>
	/// Class for RingedQueue.
	/// </summary>
	/// <typeparam name="T">Type of elements</typeparam>
	public class RingedQueue<T> : IEnumerable<T>
	{

		private T[] array;
		private int first, size;

		/// <summary>
		/// Create new instance of RingedQueue with default capacity
		/// </summary>
		public RingedQueue() : this(0x10)
		{
		}

		/// <summary>
		/// Create new instance of RingedQueue with custom capacity
		/// </summary>
		/// <param name="capacity">Capacity of ringed queue</param>
		public RingedQueue(int capacity)
		{
			array = new T[capacity];
			first = size = 0;
		}

		/// <summary>
		/// Get element by index 
		/// </summary>
		/// <param name="index">index of element</param>
		/// <returns></returns>
		public T this[int index]
		{
			get
			{
				if (index > size)
					throw new IndexOutOfRangeException();

				index += first;
				index = index >= array.Length ? index - array.Length : index;
				return (T)array[index];
			}

			set
			{
				if (index > size)
					throw new IndexOutOfRangeException();

				index += first;
				index = index >= array.Length ? index - array.Length : index;
				array[index] = value;
			}
		}

		/// <summary>
		/// Add element to queue
		/// </summary>
		/// <param name="x">element to add</param>
		public void Add(T x)
		{
			if (size == array.Length)
			{
				Extend();
			}
			int index = first + size++;
			index = index >= array.Length ? index - array.Length : index;
			array[index] = x;
		}

		/// <summary>
		/// Return first element of queue
		/// </summary>
		/// <returns>First element of queue</returns>
		public T First
		{
			get
			{
				return (T)array[first];
			}
			set
			{
				array[first] = value;
			}
		}

		/// <summary>
		/// Return last element of queue.
		/// </summary>
		/// <returns>Last element of queue</returns>
		public T Last
		{
			get
			{
				return this[size - 1];
			}
			set
			{
				this[size - 1] = value;
			}
		}

		/// <summary>
		/// Return first element and remove it
		/// </summary>
		/// <returns>First element and remove it</returns>
		public T Pop()
		{
			T res = array[first++];
			first = first >= array.Length ? first - array.Length : first;
			--size;

			return res;
		}

		private void Extend()
		{
			T[] newArray = new T[array.Length << 1];

			Array.Copy(array, first, newArray, 0, array.Length - first);
			if (first > 0)
				Array.Copy(array, 0, newArray, array.Length - first, first);
			first = 0;
			array = newArray;
		}

		/// <summary>
		/// Clear queue
		/// </summary>
		public void Clear()
		{
			first = size = 0;
		}

		/// <summary>
		/// Return count of elements.
		/// </summary>
		/// <returns>Count of elements.</returns>
		public int Count
		{
			get
			{
				return size;
			}
		}

		/// <summary>
		/// Record all elements of queue to array.
		/// </summary>
		/// <returns>Array with all elements of queue</returns>
		public T[] ToArray()
		{
			T[] result = new T[size];
			ToArray(result, 0);
			return result;
		}

		/// <summary>
		/// Record all elements of queue to array with offset
		/// </summary>
		/// <param name="data">Array with all elements to queue</param>
		/// <param name="offset">Array offset.</param>
		public void ToArray(T[] data, int offset)
		{
			if (size > data.Length - offset)
				throw new ArgumentException();

			for (int i = 0, j = first; i < size; ++i)
			{
				data[i] = array[j++];
				j = j == array.Length ? 0 : j;
			}
		}






		private struct QueueEnumerator : IEnumerator<T>
		{
			RingedQueue<T> queue;
			int offset;
			internal QueueEnumerator(RingedQueue<T> queue)
			{
				offset = 0;
				this.queue = queue;
			}




			public T Current
			{
				get
				{
					return queue.array[(queue.first + offset - 1) % queue.array.Length];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return queue.array[(queue.first + offset - 1) % queue.array.Length];
				}
			}

			public void Dispose()
			{
				
			}

			public bool MoveNext()
			{
				offset++;
				return offset <= queue.size;
				
			}

			public void Reset()
			{
				offset = 0;
			}
		}

		/// <summary>
		/// Return enumerator for this ringed queue.
		/// </summary>
		/// <returns>enumerator for this ringed queue</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return new QueueEnumerator(this);
		}

		/// <summary>
		/// Return enumerator for this ringed queue
		/// </summary>
		/// <returns>enumerator for this ringed queue</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new QueueEnumerator(this);
		}
	}
}
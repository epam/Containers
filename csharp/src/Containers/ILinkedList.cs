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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
	interface ILinkedList<T> : ILinkedListReadOnly<T> 
	{
		/// <summary>
		/// Add new first element in linked list.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		int AddFirst(T obj);
		/// <summary>
		/// Add new last element in linked list.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		int AddLast(T obj);
		/// <summary>
		/// Remove first element from list.
		/// </summary>
		void RemoveFirst();
		/// <summary>
		/// Remove last element from list.
		/// </summary>
		void RemoveLast();
		/// <summary>
		/// Remove element with specified unique index (key).
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="key">Unique index of element to remove.</param>
		void Remove(int key);
		/// <summary>
		/// Add new element after element with specified index.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="key">Index of previous for new element.</param>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		int AddAfter(int key, T obj);
		/// <summary>
		/// Add new element before element with specified index.
		/// Note: unique index are reusable.
		/// </summary>
		/// <param name="key">Index of next for new element.</param>
		/// <param name="obj">New data element to store in list.</param>
		/// <returns>Unique index (for random access) of stored element.</returns>
		int AddBefore(int key, T obj);
		/// <summary>
		/// Get first data element stored in list.
		/// </summary>
		new T First { get; set; }
		/// <summary>
		/// Get last data element stored in list.
		/// </summary>
		new T Last { get; set; }
		/// <summary>
		/// Get data element by associated unque index.
		/// Without index validatioin.
		/// </summary>
		/// <param name="key">Unique index of data element.</param>
		/// <returns>Desired data element.</returns>
		new T this[int key]
		{
			get;
			set;
		}
	}
}
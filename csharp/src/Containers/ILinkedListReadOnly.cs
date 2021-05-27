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
	interface ILinkedListReadOnly<T>
	{
		/// <summary>
		/// Get element previous to element which unique index specified.
		/// With index validation.
		/// </summary>
		/// <param name="key">Index of element which previous are desired.</param>
		/// <returns>Index of previous element.</returns>
		int PrevProtected(int key);
		/// <summary>
		/// Get element next to element which unique index specified.
		/// With index validation.
		/// </summary>
		/// <param name="key">Index of element which next are desired.</param>
		/// <returns>Index of previous element.</returns>
		int NextProtected(int key);
		/// <summary>
		/// Returns true if and only if there is element with specified key.
		/// </summary>
		/// <param name="key">Key to verify.</param>
		/// <returns>True if and only if there is element with specified key.</returns>
		Boolean ContainsKey(Int32 key);
			/// <summary>
		/// Get element previous to element which unique index specified.
		/// Without index validation.
		/// </summary>
		/// <param name="key">Index of element which previous are desired.</param>
		/// <returns>Index of previous element.</returns>
		int Prev(int key);
		/// <summary>
		/// Get element next to element which unique index specified.
		/// Without index validation.
		/// </summary>
		/// <param name="key">Index of element which next are desired.</param>
		/// <returns>Index of previous element.</returns>
		int Next(int key);
		/// <summary>
		/// Unique index of first element in list.
		/// </summary>
		int FirstKey { get; }
		/// <summary>
		/// Unique index of last element in list.
		/// </summary>
		int LastKey { get; }
		/// <summary>
		/// Get first data element stored in list.
		/// </summary>
		T First { get; }
		/// <summary>
		/// Get last data element stored in list.
		/// </summary>
		T Last { get; }
		/// <summary>
		/// Get data element by associated unque index.
		/// Without index validatioin.
		/// </summary>
		/// <param name="key">Unique index of data element.</param>
		/// <returns>Desired data element.</returns>
		T this[int key]
		{
			get;
		}
		/// <summary>
		/// Get data element by associated unque index.
		/// With index validatioin.
		/// </summary>
		/// <param name="key">Unique index of data element.</param>
		/// <returns>Desired data element.</returns>
		T GetProtected(int key);

		/// <summary>
		/// Number of elements stored in list.
		/// </summary>
		int Count { get; }
	}
}
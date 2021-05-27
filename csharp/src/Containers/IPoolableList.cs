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
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Interface for poolable indexed list.
	/// </summary>
	/// <typeparam name="T">List elements type</typeparam>
	public interface IPoolableIndexedList<T> : IReadOnlyList<T>
	{
		/// <summary>
		/// Capacity of list.
		/// </summary>
		int Capacity { get; }

		/// <summary>
		/// Clear this list.
		/// </summary>
		void Clear();

		/// <summary>
		/// Change capacity of list.
		/// </summary>
		/// <param name="capacity">New capacity of list</param>
		/// <param name="type">Type of new elements (should be inheritor of T)</param>
		void HintCapacity<Q>(int capacity) where Q : T;

		/// <summary>
		/// Change size of list.
		/// </summary>
		/// <param name="count">New count of list</param>
		/// <param name="type">Type of new elements (should be inheritor of T)</param>
		void SetSize<Q>(int count) where Q : T;

		/// <summary>
		/// Append new element to list.
		/// </summary>
		/// <param name="type">Type of new element (should be inheritor of T)</param>
		/// <returns>Appended element</returns>
		Q Append<Q>() where Q : T;
	}

}
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
﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Allow to enumerate and indexing
	/// </summary>
	/// <typeparam name="T">Type</typeparam>
	public interface IIndexedEnumerable<T> : IEnumerable<T>, ICopyable<T>, ICloneable
	{
		/// <summary>
		/// Indexing
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>object with index</returns>
		T this[int index] { get; }
		/// <summary>
		/// Number of element
		/// </summary>
		int Count { get; }
	}
}
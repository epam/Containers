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
	internal class BufferedLinkedListEnumerator<T> : IEnumerator<T>
	{
		int key;
		BufferedLinkedList<T> list;
		bool isStartedEnumeration;
		internal BufferedLinkedListEnumerator<T> Init(BufferedLinkedList<T> list) {
			this.list = list;
			isStartedEnumeration = false;
			return this;
		} 

		public T Current
		{
			get { return list[key]; }
		}

		public void Dispose()
		{
			list.enumerators.Delete(this);
		}

		object System.Collections.IEnumerator.Current
		{
			get { return list[key]; }
		}

		public bool MoveNext()
		{
			if (!isStartedEnumeration)
			{
				key = list.FirstKey;
				isStartedEnumeration = true;
				return key >= 0;
			}
			if (list.Next(key) >= 0)
			{
				key = list.Next(key);
				return true;
			}
			else return false;
		}

		public void Reset()
		{
			key = list.FirstKey;
		}
	}
}
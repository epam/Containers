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
using NUnit.Framework;
namespace EPAM.Deltix.Containers.Tests{

	public enum ListType {
		EmptyList,
		NonEmptyList
	};

	[TestFixture]
	class BufferedLinkedListTest
	{
		[TestCase(ListType.EmptyList)]
		[TestCase(ListType.NonEmptyList)]
		public void EnumerateListTest(ListType type)
		{
			BufferedLinkedList<Int32> list = new BufferedLinkedList<int>();
			if (type == ListType.EmptyList)
			{
				int sum = 0;
				foreach (var item in list) sum += item;
				Assert.AreEqual(0, sum);
				list.AddLast(1);
				list.AddLast(2);
				sum = 0;
				foreach (var item in list) sum += item;
				Assert.AreEqual(3, sum);
			}
			else
			{
				for (int i = 0; i < 10; ++i) list.AddLast(i);
				int index = 0;
				foreach (var item in list)
				{
					Assert.AreEqual(index, item);
					index++;
				}
				Assert.AreEqual(10, index);
				list.Clear();
				index = -1;
				foreach (var item in list) index = item;
				Assert.AreEqual(-1, index);
			}
		}

	}
}
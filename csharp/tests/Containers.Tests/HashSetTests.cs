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
﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture]
    public class HashSetTests
    {
		[TestCase()]
		public void HashSetStressTest()
		{
			long iterations = 1000000;
			Random r = new Random();
			SortedSet<Int64> etalon = new SortedSet<Int64>();
			UnorderedHashSet<Int64> tested = new UnorderedHashSet<Int64>();
			List<Int64> values = new List<Int64>();
			for (int i = 0; i < iterations; ++i)
			{
				if (r.Next() % 2 == 0)
				{
					long x = r.Next();
					values.Add(x);
					etalon.Add(x);
					tested.Put(x);
					Assert.AreEqual(etalon.Contains(x), tested.Contains(x));
				}
				else if (etalon.Count > 0)
				{
					long x = etalon.GetEnumerator().Current;
					etalon.Remove(x);
					tested.Remove(x);
					Assert.AreEqual(etalon.Contains(x), tested.Contains(x));
				}
			}

			Assert.AreEqual(etalon.Count, tested.Count);

			List<Int64> etalonList = new List<Int64>();
			List<Int64> testedList = new List<Int64>();

			foreach (Int64 x in etalon)
			{
				etalonList.Add(x);
			}

			foreach (Int64 x in tested)
			{
				testedList.Add(x);
			}

			etalonList.Sort();
			testedList.Sort();

			Assert.AreEqual(etalonList.Count, testedList.Count);

			for (int i = 0; i < etalonList.Count; ++i)
			{
				Assert.AreEqual(etalonList[i], testedList[i]);
			}

			for (int i = 0; i < values.Count; ++i)
			{
				Assert.AreEqual(etalon.Contains(values[i]), tested.Contains(values[i]));
			}
		}

	}
}
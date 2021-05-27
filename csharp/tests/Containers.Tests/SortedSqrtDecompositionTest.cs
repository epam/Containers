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
	class SqrtDecompositionTest
	{

		class IntComparer : IComparer<Int32>
		{
			public int Compare(int x, int y)
			{
				return x.CompareTo(y);
			}
		}

		[TestCase()]
		public void SqrtDecompositionStressTest()
		{
			int iterations = 100000;
			Random rand = new Random(55);
			SortedSqrtDecomposition<Int32> sortedSqrtDecomposition = new SortedSqrtDecomposition<Int32>(new IntComparer());
			SortedSet<Int32> set = new SortedSet<int>();

			for (int i = 0; i < iterations; ++i)
			{
				int type = rand.Next();
				int oldSize = sortedSqrtDecomposition.Count;
				if (type % 2 == 0)
				{
					int x = rand.Next();
					sortedSqrtDecomposition.AddIfNotExist(x);
					set.Add(x);
				}
				else
				{
					if (sortedSqrtDecomposition.Count == 0) continue;
					int position = Math.Abs(rand.Next()) % sortedSqrtDecomposition.Count;
					set.Remove(sortedSqrtDecomposition.GetAt(sortedSqrtDecomposition.GetIteratorByIndex(position)));
					sortedSqrtDecomposition.Remove(sortedSqrtDecomposition.GetAt(sortedSqrtDecomposition.GetIteratorByIndex(position)));
				}
				Assert.AreEqual(set.Count, sortedSqrtDecomposition.Count);
				if (set.Count == 0) continue;
				int index = Math.Abs(rand.Next()) % sortedSqrtDecomposition.Count;
				int iteratorToIndex = sortedSqrtDecomposition.GetIteratorByIndex(index);
				int elementAt = sortedSqrtDecomposition.GetIterator(sortedSqrtDecomposition.GetAt(iteratorToIndex));
				Assert.AreEqual(elementAt, iteratorToIndex);
				int sqrtIterator = sortedSqrtDecomposition.First;
				foreach (Int32 x in set)
				{
					if (index == 0)
					{
						Assert.AreEqual(iteratorToIndex, sqrtIterator);
					}
					index--;
					Assert.AreEqual(x, sortedSqrtDecomposition.GetAt(sqrtIterator));
					sqrtIterator = sortedSqrtDecomposition.GetNext(sqrtIterator);
				}

			}
		}

		[TestCase()]
		public void SqrtDecompositionStressTestWithRemoveByIterator()
		{
			int iterations = 100000;
			Random rand = new Random(55);
			SortedSqrtDecomposition<Int32> sortedSqrtDecomposition = new SortedSqrtDecomposition<Int32>(new IntComparer());
			SortedSet<Int32> set = new SortedSet<int>();

			for (int i = 0; i < iterations; ++i)
			{
				int type = rand.Next();
				int oldSize = sortedSqrtDecomposition.Count;
				if (type % 2 == 0)
				{
					int x = rand.Next();
					sortedSqrtDecomposition.AddIfNotExist(x);
					set.Add(x);
				}
				else
				{
					if (sortedSqrtDecomposition.Count == 0) continue;
					int position = Math.Abs(rand.Next()) % sortedSqrtDecomposition.Count;
					set.Remove(sortedSqrtDecomposition.GetAt(sortedSqrtDecomposition.GetIteratorByIndex(position)));
					sortedSqrtDecomposition.RemoveAt(sortedSqrtDecomposition.GetIteratorByIndex(position));
				}
				Assert.AreEqual(set.Count, sortedSqrtDecomposition.Count);
				if (set.Count == 0) continue;
				int index = Math.Abs(rand.Next()) % sortedSqrtDecomposition.Count;
				int iteratorToIndex = sortedSqrtDecomposition.GetIteratorByIndex(index);
				int elementAt = sortedSqrtDecomposition.GetIterator(sortedSqrtDecomposition.GetAt(iteratorToIndex));
				Assert.AreEqual(elementAt, iteratorToIndex);
				int sqrtIterator = sortedSqrtDecomposition.First;
				foreach (Int32 x in set)
				{
					if (index == 0)
					{
						Assert.AreEqual(iteratorToIndex, sqrtIterator);
					}
					index--;
					Assert.AreEqual(x, sortedSqrtDecomposition.GetAt(sqrtIterator));
					sqrtIterator = sortedSqrtDecomposition.GetNext(sqrtIterator);
				}

			}
		}
	}
}
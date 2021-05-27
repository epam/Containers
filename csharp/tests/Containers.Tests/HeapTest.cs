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
	[TestFixture()]
	class HeapTest
	{

		[TestCase()]
		public void HeapWithDictionaryStressTest()
		{
			int numberOfIterations = 1000000;
			SortedDictionary<long, long> priorityQueue = new SortedDictionary<long, long>();
			Dictionary<long, long> dictionary = new Dictionary<long, long>();
			HeapWithIndices<long, long> myHeap = new HeapWithIndices<long, long>(10);
			long lastKey = 0;
			Random rand = new Random(55);
			for (int i = 0; i < numberOfIterations; ++i)
			{
				if (rand.Next() % 3 == 0)
				{
					if (priorityQueue.Count > 0)
					{
						Assert.AreEqual(priorityQueue.Count, myHeap.Count);
						long value = priorityQueue.First().Key;
						long key = priorityQueue.First().Value;
						Assert.AreEqual((long)value, (long)myHeap.PeekValue);
						Assert.AreEqual((long)key, (long)myHeap.PeekKey);
						dictionary.Remove(key);
						myHeap.Pop();
						priorityQueue.Remove(value);
					}
				}
				else
				{
					long x = rand.Next();
					while (priorityQueue.ContainsKey(x)) x = rand.Next();
					myHeap.Add(x, (long)i);
					priorityQueue.Add(x, (long)i);
					dictionary.Add((long)i, x);
					lastKey = i;
				}
				if (rand.Next() % 3 == 0)
				{
					if (rand.Next() % 2 == 0)
					{
						if (priorityQueue.Count > 0)
						{
							if (dictionary.ContainsKey(lastKey))
							{
								priorityQueue.Remove(dictionary[lastKey]);
								myHeap.Remove((long)lastKey);
								dictionary.Remove(lastKey);
							}
						}
					}
					else
					{
						if (priorityQueue.Count > 0)
						{
							if (rand.Next() % 2 == 0)
							{
								long newTop = rand.Next();
								while (priorityQueue.ContainsKey(newTop)) newTop = rand.Next();

								long key = myHeap.PeekKey;
								priorityQueue.Remove(dictionary[key]);
								dictionary.Remove(key);
								priorityQueue.Add(newTop, key);
								dictionary.Add(key, newTop);
								myHeap.ModifyTop(newTop);
							}
							else if (myHeap.ContainsKey((long)lastKey))
							{
								long newTop = rand.Next();
								while (priorityQueue.ContainsKey(newTop)) newTop = rand.Next();

								long key = lastKey;
								priorityQueue.Remove(dictionary[key]);
								dictionary.Remove(key);
								priorityQueue.Add(newTop, key);
								dictionary.Add(key, newTop);
								myHeap.Modify(key, newTop);
							}
						}
					}
				}
			}
		}

		[TestCase()]
		public void HeapWithIndicesStressTest()
		{
			int numberOfIterations = 1000000;
			SortedDictionary<long, int> priorityQueue = new SortedDictionary<long, int>();
			Dictionary<int, long> dictionary = new Dictionary<int, long>();
			HeapWithIndices<long> myHeap = new HeapWithIndices<long>(10);
			int lastKey = 0;
			Random rand = new Random(55);
			for (int i = 0; i < numberOfIterations; ++i)
			{
				int type = rand.Next() % 3;
				if (type == 0)
				{
					if (priorityQueue.Count > 0)
					{
						Assert.AreEqual(priorityQueue.Count, myHeap.Count);
						long value = priorityQueue.First().Key;
						int key = priorityQueue.First().Value;
						Assert.AreEqual((long)value, (long)myHeap.PeekValue);
						Assert.AreEqual((long)key, (long)myHeap.PeekKey);
						dictionary.Remove(key);
						myHeap.Pop();
						priorityQueue.Remove(value);
					}
				}
				if (rand.NextDouble() > 0.1)
				{
					long x = rand.Next();
					while (priorityQueue.ContainsKey(x)) x = rand.Next();
					int y = myHeap.Add(x);
					priorityQueue.Add(x, y);
					dictionary.Add(y, x);
					lastKey = y;
				}
				if (type == 1)
				{
					if (priorityQueue.Count > 0)
					{
						if (dictionary.ContainsKey(lastKey))
						{
							priorityQueue.Remove(dictionary[lastKey]);
							myHeap.Remove(lastKey);
							dictionary.Remove(lastKey);
						}
					}
				}
				if (type == 2)
				{
					if (priorityQueue.Count > 0)
					{
						if (rand.NextDouble() < 0.5)
						{
							long newTop = rand.Next();
							while (priorityQueue.ContainsKey(newTop)) newTop = rand.Next();

							int key = myHeap.PeekKey;
							priorityQueue.Remove(dictionary[key]);
							dictionary.Remove(key);
							priorityQueue.Add(newTop, key);
							dictionary.Add(key, newTop);
							myHeap.ModifyTop(newTop);
						}
						else
						{
							if (dictionary.ContainsKey(lastKey))
							{
								long newTop = rand.Next();
								while (priorityQueue.ContainsKey(newTop)) newTop = rand.Next();
								int key = lastKey;
								priorityQueue.Remove(dictionary[key]);
								dictionary.Remove(key);
								priorityQueue.Add(newTop, key);
								dictionary.Add(key, newTop);
								myHeap.Modify(key, newTop);
							}
						}
					}
				}
			}
		}

		public struct LongLongPair : IComparable<LongLongPair>
		{
			internal long first;
			internal long second;
			internal LongLongPair(long x, long y)
			{
				first = x;
				second = y;
			}

			public int CompareTo(LongLongPair other)
			{
				int x = first.CompareTo(other.first);
				if (x != 0) return x;
				return second.CompareTo(other.second);
			}
		}

		[TestCase()]
		public void HeapWithAttachmentsStressTest()
		{
			int numberOfIterations = 1000000;

			SortedSet<LongLongPair> priorityQueue = new SortedSet<LongLongPair>();
			UnorderedHashSet<long> elements = new UnorderedHashSet<long>();

			HeapWithAttachments<long, long> myHeap = new HeapWithAttachments<long, long>(10);

			Random rand = new Random(55);
			for (int i = 0; i < numberOfIterations; ++i)
			{
				if (rand.Next() % 3 == 0)
				{
					if (priorityQueue.Count > 0)
					{
						Assert.AreEqual(priorityQueue.Count, myHeap.Count);
						LongLongPair top = priorityQueue.First();
						Assert.AreEqual((long)top.first, (long)myHeap.PeekValue);
						Assert.AreEqual((long)top.second, (long)myHeap.PeekAttachments);
						priorityQueue.Remove(top);
						elements.Remove(top.first);
						myHeap.Pop();
					}
				}
				else
				{
					long x = rand.Next();
					while (elements.Contains(x)) x = rand.Next();
					long y = i;
					myHeap.Add(x, y);
					elements.Put(x);
					priorityQueue.Add(new LongLongPair(x, y));
				}
				if (myHeap.Count > 0 && rand.Next() % 10 == 0)
				{
					long x = rand.Next();
					while (elements.Contains(x)) x = rand.Next();
					long y = myHeap.PeekAttachments;
					myHeap.ModifyTop(x);
					priorityQueue.Remove(priorityQueue.First());
					priorityQueue.Add(new LongLongPair(x, y));
				}
			}

		}

		[TestCase()]
		public void HeapStressTest()
		{
			int numberOfIterations = 10000000;
			SortedSet<long> priorityQueue = new SortedSet<long>();


			Heap<long> myHeap = new Heap<long>(10);

			Random rand = new Random(55);
			for (int i = 0; i < numberOfIterations; ++i)
			{
				if (rand.Next() % 3 == 0)
				{
					if (priorityQueue.Count > 0)
					{
						Assert.AreEqual(priorityQueue.Count, myHeap.Count);
						Assert.AreEqual((long)priorityQueue.First(), (long)myHeap.Pop());
						priorityQueue.Remove(priorityQueue.First());
					}
				}
				else
				{
					long x = rand.Next();
					if (priorityQueue.Contains(x)) continue;
					myHeap.Add(x);
					priorityQueue.Add(x);
				}
				if (rand.Next() % 10 == 0 && myHeap.Count > 0)
				{
					long x = rand.Next();
					if (priorityQueue.Contains(x)) continue;
					myHeap.ModifyTop(x);
					priorityQueue.Remove(priorityQueue.First());
					priorityQueue.Add(x);
				}
			}
		}



		Heap<Int32> simpleHeap;
		HeapWithIndices<Int32> heap;




		[TestCase(10000000)]
		public void HeapWithIndicesStressTest(int k)
		{
			HeapWithIndices<long> heap = new HeapWithIndices<long>();
			HeapWithIndices<long, long> headWithDictionary = new HeapWithIndices<long, long>();
			SortedSet<int> keys = new SortedSet<int>();
			Random rand = new Random(55);
			for (int i = 0; i < k; ++i)
			{
				if (rand.Next() % 3 == 1)
				{
					if (rand.Next() % 2 == 0)
					{
						if (heap.Count > 0)
						{
							keys.Remove(heap.Pop().Key);
						}
					}
					else
					{
						if (keys.Count == 0) continue;
						heap.Remove(keys.First());
						keys.Remove(keys.First());
					}
				}
				else
				{
					keys.Add(heap.Add(rand.Next()));
				}
			}

		}


		[TestCase(10000000)]
		public void HeapWithIndicesStressTestModifyTop(int k)
		{
			HeapWithIndices<long> heap = new HeapWithIndices<long>();
			HeapWithIndices<long, long> headWithDictionary = new HeapWithIndices<long, long>();
			SortedSet<int> keys = new SortedSet<int>();
			Random rand = new Random(55);
			for (int i = 0; i < k; ++i)
			{
				if (rand.Next() % 3 == 1)
				{
					heap.ModifyTop(rand.Next());
				}
				else
				{
					keys.Add(heap.Add(rand.Next()));
				}
			}

		}


		[TestCase(10000000)]
		public void HeapWithDictionaryRemoveBadKey(int k)
		{
			HeapWithIndices<long, long> heap = new HeapWithIndices<long, long>(10);
			SortedSet<long> keys = new SortedSet<long>();
			Random rand = new Random(55);
			for (int i = 0; i < k; ++i)
			{
				if (rand.Next() % 3 == 1)
				{
					if (rand.Next() % 2 == 0)
					{
						if (heap.Count > 0)
						{
							keys.Remove(heap.Pop().Key);
						}
					}
					else
					{
						if (keys.Count == 0) continue;
						heap.Remove(keys.First() - 1);
						heap.Remove(keys.First());
						keys.Remove(keys.First());
					}
				}
				else
				{
					keys.Add(i);
					heap.Add(rand.Next(), i);
				}
			}

		}

		[TestCase(10000000)]
		public void HeapWithDictionaryStressTest(int k)
		{
			HeapWithIndices<long, long> heap = new HeapWithIndices<long, long>(10);
			SortedSet<long> keys = new SortedSet<long>();
			Random rand = new Random(55);
			for (int i = 0; i < k; ++i)
			{
				if (rand.Next() % 3 == 1)
				{
					if (rand.Next() % 2 == 0)
					{
						if (heap.Count > 0)
						{
							keys.Remove(heap.Pop().Key);
						}
					}
					else
					{
						if (keys.Count == 0) continue;
						heap.Remove(keys.First());
						keys.Remove(keys.First());
					}
				}
				else
				{
					keys.Add(i);
					heap.Add(rand.Next(), i);
				}
			}

		}

		[Test()]
		public void AddTest()
		{
			simpleHeap = new Heap<int>();
			heap = new HeapWithIndices<int>();
			for (int i = 1; i <= 10; ++i) simpleHeap.Add(i);
			for (int i = 1; i <= 10; ++i) heap.Add(i);
			Assert.AreEqual(1, simpleHeap.Peek);
			Assert.AreEqual(1, heap.Peek.Value);
			simpleHeap.Add(-1);
			heap.Add(-1);
			Assert.AreEqual(-1, simpleHeap.Peek);
			Assert.AreEqual(-1, heap.Peek.Value);
		}
		[Test()]
		public void PopTest()
		{
			simpleHeap = new Heap<int>();
			heap = new HeapWithIndices<int>();
			for (int i = 1; i <= 10; ++i) simpleHeap.Add(i);
			for (int i = 1; i <= 10; ++i) heap.Add(i);
			Assert.AreEqual(1, simpleHeap.Peek);
			Assert.AreEqual(1, heap.Peek.Value);
			simpleHeap.Add(-1);
			heap.Add(-1);
			Assert.AreEqual(-1, simpleHeap.Peek);
			Assert.AreEqual(-1, heap.Peek.Value);
			for (int i = 0; i < 5; ++i) simpleHeap.Pop();
			for (int i = 0; i < 5; ++i) heap.Pop();
			Assert.AreEqual(5, simpleHeap.Peek);
			Assert.AreEqual(5, heap.Peek.Value);
		}

		[Test()]
		public void HeapWithAttachmentsModifyTopTest()
		{
			HeapWithAttachments<int, String> heapWithAttachments = new HeapWithAttachments<int, string>();
			for (int i = 0; i < 11; ++i) heapWithAttachments.Add(i, i.ToString());
			heapWithAttachments.ModifyTop(-1);
			Assert.AreEqual("0", heapWithAttachments.Peek.Attachment);
			Assert.AreEqual(-1, heapWithAttachments.Peek.Value);
		}


		[Test()]
		public void RemoveTest()
		{
			heap = new HeapWithIndices<int>();
			for (int i = 1; i <= 10; ++i) heap.Add(i);
			Assert.AreEqual(1, heap.Peek.Value);
			heap.Add(-1);
			Assert.AreEqual(-1, heap.Peek.Value);
			for (int i = 10; i > 5; --i) heap.Remove(i);
			heap.Remove(0);
			Assert.AreEqual(2, heap.Peek.Value);
		}

		[Test()]
		public void UserKeyTest()
		{
			HeapWithIndices<int, String> heap = new HeapWithIndices<int, String>();
			for (int i = 1; i <= 10; ++i) heap.Add(i, "abacaba" + i.ToString());
			Assert.AreEqual(1, heap.Peek.Value);
			heap.Add(-1, "abacaba-1");
			Assert.AreEqual(-1, heap.Peek.Value);
			for (int i = 10; i > 5; --i) heap.Remove("abacaba" + i.ToString());
			heap.Remove("abacaba-1");
			heap.Remove("abacaba1");
			Assert.AreEqual(2, heap.Peek.Value);
		}


		[Test()]
		public void TryGetValueTest()
		{
			HeapWithIndices<int> simpleHeap = new HeapWithIndices<int>();
			HeapWithIndices<int, String> heap = new HeapWithIndices<int, string>();
			for (int i = -10; i <= 10; ++i) simpleHeap.Add(i);
			for (int i = -10; i <= 10; ++i) heap.Add(i, i.ToString());
			int returnValue = 0;
			simpleHeap.TryGetValue(1, out returnValue);
			Assert.AreEqual(true, simpleHeap.TryGetValue(1, out returnValue));
			Assert.AreEqual(-9, returnValue);
			Assert.AreEqual(false, simpleHeap.TryGetValue(50, out returnValue));
			heap.TryGetValue("-5", out returnValue);
			Assert.AreEqual(true, heap.TryGetValue("-5", out returnValue));
			Assert.AreEqual(-5, returnValue);
			Assert.AreEqual(false, heap.TryGetValue("50", out returnValue));

		}

		[Test()]
		public void ModifyTest()
		{
			Heap<int> simpleHeap = new Heap<int>();
			HeapWithIndices<int, String> heap = new HeapWithIndices<int, string>();
			for (int i = 0; i < 10; ++i) simpleHeap.Add(i);
			for (int i = 0; i <= 10; ++i) heap.Add(i, i.ToString());
			simpleHeap.ModifyTop(12);
			Assert.AreEqual(1, simpleHeap.Peek);
			simpleHeap.ModifyTop(-11);
			Assert.AreEqual(-11, simpleHeap.Peek);
			for (int i = 0; i < 9; ++i) simpleHeap.Pop();
			Assert.AreEqual(12, simpleHeap.Peek);
			heap.Modify("10", -1111);
			Assert.AreEqual(-1111, heap.Peek.Value);
			Assert.AreEqual("10", heap.Peek.Key);
			heap.Pop();
			Assert.AreEqual("0", heap.Peek.Key);
			Assert.AreEqual(0, heap.Peek.Value);
			for (int i = 0; i < 9; ++i) heap.Pop();
			Assert.AreEqual("9", heap.Peek.Key);
			Assert.AreEqual(9, heap.Peek.Value);
			heap.ModifyTop(1111);
			Assert.AreEqual("9", heap.Peek.Key);
			Assert.AreEqual(1111, heap.Peek.Value);
		}


		[Test()]
		public void AttachmentsHeapTest()
		{
			HeapWithAttachments<int, String> heap = new HeapWithAttachments<int, string>();
			for (int i = 0; i < 10; ++i) heap.Add(i, i.ToString());
			Assert.AreEqual(0, heap.PeekValue);
			Assert.AreEqual("0", heap.PeekAttachments);
			heap.ModifyTop(11);
			Assert.AreEqual(1, heap.PeekValue);
			Assert.AreEqual("1", heap.PeekAttachments);
			heap.ModifyTop(-1111, "-111");
			Assert.AreEqual(-1111, heap.PeekValue);
			Assert.AreEqual("-111", heap.PeekAttachments);
			heap.ModifyTop(-1);
			Assert.AreEqual(-1, heap.PeekValue);
			Assert.AreEqual("-111", heap.PeekAttachments);
		}

		[Test()]
		public void EnumeratorHeapTest()
		{
			Heap<int> heap = new Heap<int>();
			HeapWithAttachments<int, int> heapWithAttachments = new HeapWithAttachments<int, int>();
			HeapWithIndices<int, int> heapWithDictionary = new HeapWithIndices<int, int>();
			HeapWithIndices<int> heapWithIndices = new HeapWithIndices<int>();
			for (int i = 1; i <= 10; ++i)
			{
				heap.Add(i);
				heapWithAttachments.Add(i, i);
				heapWithDictionary.Add(i, i);
				heapWithIndices.Add(i);
			}

			int index = 0;
			foreach (int item in heap)
			{
				index += item;
			}
			Assert.AreEqual(55, index);
			foreach (HeapWithAttachments<int, int>.HeapEntry item in heapWithAttachments)
			{
				index -= (item.Attachment + item.Value);
			}
			Assert.AreEqual(-55, index);
			foreach (HeapWithIndices<int, int>.HeapEntry item in heapWithDictionary)
			{
				index += (item.Key + item.Value);
			}
			Assert.AreEqual(55, index);
			foreach (HeapWithIndices<int>.HeapEntry item in heapWithIndices)
			{
				index += (item.Value);
			}
			Assert.AreEqual(110, index);
			bool flag = false;
			try
			{
				foreach (int item in heap)
				{
					heap.Add(1);
				}
			}
			catch (Exception)
			{
				flag = true;
			}
			Assert.AreEqual(true, flag);
			flag = false;
			try
			{
				foreach (HeapWithAttachments<int, int>.HeapEntry item in heapWithAttachments)
				{
					heapWithAttachments.Add(1, 1);
				}
			}
			catch (Exception)
			{
				flag = true;
			}
			Assert.AreEqual(true, flag);
			flag = false;
			try
			{
				foreach (HeapWithIndices<int, int>.HeapEntry item in heapWithDictionary)
				{
					heapWithDictionary.Add(1, 1);
				}
			}
			catch (Exception)
			{
				flag = true;
			}
			Assert.AreEqual(true, flag);
			flag = false;
			try
			{
				foreach (HeapWithIndices<int>.HeapEntry item in heapWithIndices)
				{
					heapWithIndices.Add(1);
				}
			}
			catch (Exception)
			{
				flag = true;
			}
			Assert.AreEqual(true, flag);

		}



	}
}
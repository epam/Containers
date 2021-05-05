using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture()]
	class ListFactoryTest
	{
		[TestCase()]
		public void ListFactoryStressTest()
		{

			int numberOfIterations = 2000000;
			int numberOfLists = 100;
			ListFactory<int> factory = new ListFactory<int>();
			List<ListFactory<int>.List> ids = new List<ListFactory<int>.List>();
			List<BufferedLinkedList<int>> etalonLists = new List<BufferedLinkedList<int>>();
			for (int i = 0; i < numberOfLists; ++i)
			{
				ids.Add(factory.CreateList());
				etalonLists.Add(new BufferedLinkedList<int>());
			}
			Random random = new Random(55);
			for (int i = 0; i < numberOfIterations; ++i)
			{

				int type = Math.Abs(random.Next()) % 3;
				if (type < 2)
				{
					int index = Math.Abs(random.Next()) % numberOfLists;

					int element = random.Next();
					int insertionType = (Math.Abs(random.Next()) % 4);
					if (insertionType == 0)
					{
						ids[index].AddLast(element);
						etalonLists[index].AddLast(element);
					}
					else if (insertionType == 1)
					{
						ids[index].AddFirst(element);
						etalonLists[index].AddFirst(element);
					}
					else if (insertionType == 2)
					{
						if (etalonLists[index].Count < 2) continue;
						ids[index].First.Next.Value.AddAfter(element);
						etalonLists[index].AddAfter(etalonLists[index].Next(etalonLists[index].FirstKey), element);
					}
					else if (insertionType == 3)
					{
						if (etalonLists[index].Count < 2) continue;
						ids[index].First.Next.Value.AddBefore(element);
						etalonLists[index].AddBefore(etalonLists[index].Next(etalonLists[index].FirstKey), element);
					}
				}
				else
				{
					int removeType = Math.Abs(random.Next()) % 3;
					int index = Math.Abs(random.Next()) % numberOfLists;
					
					if (removeType == 0)
					{
						if (etalonLists[index].Count == 0) continue;
						etalonLists[index].Remove(etalonLists[index].FirstKey);
						ids[index].First.Remove();
					}
					else if (removeType == 1)
					{
						if (etalonLists[index].Count == 0) continue;
						etalonLists[index].Remove(etalonLists[index].LastKey);
						ids[index].Last.Remove();
					}
					else if (removeType == 2)
					{
						if (etalonLists[index].Count < 2) continue;
						etalonLists[index].Remove(etalonLists[index].Next(etalonLists[index].FirstKey));
						ids[index].First.Next.Value.Remove();
					}
				}
				if (i % 10000 == 0)
				{
					
					for (int j = 0; j < ids.Count; ++j)
					{
						Assert.AreEqual(etalonLists[j].Count, ids[j].Count);
						int ptr1 = etalonLists[j].FirstKey;
						var ptr2 = ids[j].First;
						int n = ids[j].Count;
						for (int k = 0; k < n; ++k)
						{
							Assert.AreEqual(etalonLists[j][ptr1], ptr2.Data);
							ptr1 = etalonLists[j].Next(ptr1);
							if (k != n - 1) ptr2 = ptr2.Next.Value;
						}
					}
				}
			}
			for (int j = 0; j < ids.Count; ++j)
			{
				Assert.AreEqual(etalonLists[j].Count, ids[j].Count);
				int ptr1 = etalonLists[j].FirstKey;
				var ptr2 = ids[j].First;
				int n = ids[j].Count;
				for (int k = 0; k < n; ++k)
				{
					Assert.AreEqual(etalonLists[j][ptr1], ptr2.Data);
					ptr1 = etalonLists[j].Next(ptr1);
					if (k != n - 1) ptr2 = ptr2.Next.Value;
				}
			}
		}
	}
}

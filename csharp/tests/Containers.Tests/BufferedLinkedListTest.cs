using System;
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

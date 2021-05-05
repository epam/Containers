using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture]
	public class MultiThreadingTest
	{
		public enum TestedClass
		{
			MutableString,
			BinaryArray,
		}

		static volatile int expectedHash = 0;
		static volatile Int32 index = 0, previousIndex = 0;

		[TestCase(TestedClass.MutableString)]
		[TestCase(TestedClass.BinaryArray)]
		public static void TestHashCodeMultithread(TestedClass testWhat)
		{
			Int32 threadCount = 2;
			Int32 iterationsCount = 1000000;
   

			BinaryArray a = new BinaryArray();
			MutableString s = new MutableString();
			Barrier beforeWrite = new Barrier(threadCount);
			Barrier beforeRead = new Barrier(threadCount);
		
			Action<int> action = (x) =>
			{
				while (index < iterationsCount) 
				{
					beforeWrite.SignalAndWait();
					if ((previousIndex % threadCount) == x)
					{
						index++;
						expectedHash = index;
						switch (testWhat)
						{
							case TestedClass.BinaryArray: a.Assign(index); break;
							case TestedClass.MutableString: s.Assign(index); expectedHash = s.Clone().GetHashCode(); break;
						}
						a.Assign(index);
						if ((index % 100000) == 0) Console.WriteLine("Index = " + index);
					}
					beforeRead.SignalAndWait();
					int currentHash = 0;
					switch (testWhat)
					{
						case TestedClass.BinaryArray: currentHash = a.GetHashCode(); break;
						case TestedClass.MutableString: currentHash = s.GetHashCode(); break;
					}
					Assert.AreEqual(currentHash, expectedHash);
					previousIndex = index;
				}
			};

			Task[] t = new Task[threadCount];
			for (int i = 0; i < threadCount; ++i)
			{
				int id = i;
				t[i] = new Task(() => action(id)); 
				t[i].Start();
			}

			Task.WaitAll(t);
		}
	}
}

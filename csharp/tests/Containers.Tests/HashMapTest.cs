using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	public class HashMapTest
	{


		[TestCase()]
		public void ObjToObjUnsafeIterationStressTest()
		{

			int numberOfIteration = 2000000;
			HashMap<BinaryArray, Int32> myHashMap = new HashMap<BinaryArray, Int32>(-1);
			Dictionary<BinaryArray, Int32> etalon = new Dictionary<BinaryArray, int>();
			Random rand = new Random(55);
			for (int i = 0; i < numberOfIteration; ++i)
			{
				if (rand.Next(3) != 0)
				{
					BinaryArray ar = new BinaryArray().Assign(i);
					etalon.Add(ar, i);
					myHashMap[ar] = i;
					Assert.AreEqual(etalon[ar], myHashMap[ar]);
				}
				else
				{
					int x = rand.Next(i);
					BinaryArray ar = new BinaryArray().Assign(x);
					Assert.AreEqual(etalon.ContainsKey(ar), myHashMap.ContainsKey(ar));
					if (myHashMap.ContainsKey(ar))
					{
						myHashMap.Remove(ar);
						etalon.Remove(ar);
					}
				}

				if (i % 10000 == 0)
				{
					List<Int32> etalon1 = new List<int>();
					List<Int32> unsafeEnumeration = new List<int>();
					for (var item = myHashMap.First; item != null; item = item.Value.Next)
					{
						etalon1.Add(item.Value.Value);
					}	
					for (var item = myHashMap.UnsafeFirst; item != null; item = item.Value.Next)
					{
						unsafeEnumeration.Add(item.Value.Value);
					}
					etalon1.Sort();
					unsafeEnumeration.Sort();
					Assert.AreEqual(etalon1.Count, unsafeEnumeration.Count);
					for (int j = 0; j < etalon1.Count; ++j)
					{
						Assert.AreEqual(etalon1[j], unsafeEnumeration[j]);
					}
				}
			}
		}


		[TestCase()]
		public void ObjToObjExtendedHashMapTests()
		{
			HashMap<BinaryArray, Int32> extendedObjToObjHashMap = new HashMap<BinaryArray, Int32>(-1);
			Assert.AreEqual(-1, extendedObjToObjHashMap.SetAndGet(new BinaryArray().Assign(1), 1));
			Assert.AreEqual(1, extendedObjToObjHashMap.SetAndGet(new BinaryArray().Assign(1), 1));

			Assert.AreEqual(true, extendedObjToObjHashMap.TrySet(new BinaryArray().Assign(2), 1));
			Assert.AreEqual(false, extendedObjToObjHashMap.TrySet(new BinaryArray().Assign(2), 1));

			extendedObjToObjHashMap[new BinaryArray().Assign(3)] =  1;
		
			Assert.AreEqual(-1, extendedObjToObjHashMap[new BinaryArray().Assign(4)]);
			Assert.AreEqual(1, extendedObjToObjHashMap[new BinaryArray().Assign(3)]);

			extendedObjToObjHashMap.Remove(new BinaryArray().Assign(3));
			
			Assert.AreEqual(1, extendedObjToObjHashMap.Remove(new BinaryArray().Assign(2)));
			Assert.AreEqual(-1, extendedObjToObjHashMap.Remove(new BinaryArray().Assign(2)));

		}




		[TestCase()]
		public void ObjToObjIteratingTest()
		{
			HashMap<BinaryArray, Int32> myHashMap = new HashMap<BinaryArray, Int32>(-1);
			List<Int32> integerArrayList = new List<Int32>();
			for (int i = 0; i < 10; ++i)
			{
				myHashMap[new BinaryArray().Assign(i)] = i;
			}
			for (var iterator = myHashMap.First; iterator != null; iterator = iterator.Value.Next)
			{
				Assert.AreEqual(true, iterator.Value.Key.Equals(new BinaryArray().Assign(iterator.Value.Value)));
				integerArrayList.Add(iterator.Value.Value);
			}
			integerArrayList.Sort();
			Assert.AreEqual(10, integerArrayList.Count);
			for (int i = 0; i < 10; ++i) Assert.AreEqual(i, integerArrayList[i]);
		}


		[TestCase()]
		public void ObjToObjIteratingTest2()
		{
			HashMap<BinaryArray, Int32> myHashMap = new HashMap<BinaryArray, Int32>(-1);
			List<Int32> integerArrayList = new List<Int32>();
			for (int i = 0; i < 10; ++i)
			{
				myHashMap[new BinaryArray().Assign(i)] = i;
			}
			foreach (var item in myHashMap)
			{
				Assert.AreEqual(true, item.Key.Equals(new BinaryArray().Assign(item.Value)));
				integerArrayList.Add(item.Value);

			}
			integerArrayList.Sort();
			Assert.AreEqual(10, integerArrayList.Count);
			for (int i = 0; i < 10; ++i) Assert.AreEqual(i, integerArrayList[i]);
		}




		[TestCase()]
		public void ObjToObjTest()
		{
			HashMap<BinaryArray, Int32> myHashMap = new HashMap<BinaryArray, Int32>(-1);
			myHashMap[new BinaryArray().Assign(1)] = 10;
			myHashMap[new BinaryArray().Assign(1)] = 20;
			Assert.AreEqual(20, myHashMap[new BinaryArray().Assign(1)]);
			bool flag = false;
			int x = myHashMap[new BinaryArray().Assign(2)];
			Assert.AreEqual(-1, x);
			

		}



		[TestCase()]
		public void ObjToObjLocateStressTest()
		{

			int numberOfIteration = 2000000;
			HashMap<BinaryArray, Int32> myHashMap = new HashMap<BinaryArray, Int32>(-1);
			Dictionary<BinaryArray, Int32> etalon = new Dictionary<BinaryArray, Int32>();
			Random rand = new Random(55);
			for (int i = 0; i < numberOfIteration; ++i)
			{
				if (rand.Next(3) != 0)
				{
					BinaryArray ar = new BinaryArray().Assign(i);
					etalon[ar] = i;
					long iterator = myHashMap.LocateOrReserve(ar);
					myHashMap.SetKeyAt(iterator, ar);
					myHashMap.SetValueAt(iterator, i);
					Assert.AreEqual(myHashMap[ar], etalon[ar]);
				}
				else
				{
					int x = rand.Next(i);
					BinaryArray ar = new BinaryArray().Assign(x);
					Assert.AreEqual(etalon.ContainsKey(ar), myHashMap.ContainsKey(ar));
					if (myHashMap.ContainsKey(ar))
					{
						long key = myHashMap.Locate(ar);
						Assert.AreEqual(true, myHashMap.GetKeyAt(myHashMap.Locate(ar)).Equals(ar));
						Assert.AreEqual(x, myHashMap.GetValueAt(myHashMap.Locate(ar)));
						myHashMap.RemoveAt(key);
						etalon.Remove(ar);
					}
					Assert.AreEqual(etalon.ContainsKey(ar), myHashMap.ContainsKey(ar));
				}

				if (i % 1000000 == 0)
				{
					myHashMap.Clear();
					etalon.Clear();
				}
			}
		}



		////////////////////////////////////////////////////////////////

		[TestCase()]
		public void ObjToObjStressTest()
		{

			int numberOfIteration = 2000000;
			HashMap<BinaryArray, Int32> myHashMap = new HashMap<BinaryArray, Int32>(-1);
			Dictionary<BinaryArray, Int32> etalon = new Dictionary<BinaryArray, int>();
			Random rand = new Random(55);
			for (int i = 0; i < numberOfIteration; ++i)
			{
				if (rand.Next(3) != 0)
				{
					BinaryArray ar = new BinaryArray().Assign(i);
					etalon.Add(ar, i);
					myHashMap[ar] = i;
					Assert.AreEqual(etalon[ar], myHashMap[ar]);
				}
				else
				{
					int x = rand.Next(i);
					BinaryArray ar = new BinaryArray().Assign(x);
					Assert.AreEqual(etalon.ContainsKey(ar), myHashMap.ContainsKey(ar));
					if (myHashMap.ContainsKey(ar))
					{
						myHashMap.Remove(ar);
						etalon.Remove(ar);
					}
				}

				if (i % 1000000 == 0)
				{
					myHashMap.Clear();
					etalon.Clear();
				}
			}
		}



	}
}

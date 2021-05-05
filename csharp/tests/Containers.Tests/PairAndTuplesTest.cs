using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture]
    class PairAndTuplesTest
    {
		[TestCase()]
		public void ValuePairTest()
		{
			ValuePair<Int32, Int32> pair = new ValuePair<int, int>(55, 555);
			Assert.AreEqual("First: 55; Second: 555", pair.ToString());
			ValuePair<Int32, Int32> pair2 = new ValuePair<int, int>(55, 55);

			ValuePair<Int32, Int32> pair3 = new ValuePair<int, int>(55, 555);
			Assert.AreEqual(pair3.GetHashCode(), pair.GetHashCode());
			Assert.AreNotEqual(pair3.GetHashCode(), pair2.GetHashCode());

			Assert.AreEqual(true, pair3.Equals(pair));
			Assert.AreEqual(false, pair3.Equals(pair2));
		}

		[TestCase()]
		public void PairTest()
		{
			Pair<Int32, Int32> pair = new Pair<int, int>(55, 555);
			Assert.AreEqual("First: 55; Second: 555", pair.ToString());
			Pair<Int32, Int32> pair2 = new Pair<int, int>(55, 55);

			Pair<Int32, Int32> pair3 = new Pair<int, int>(55, 555);
			Assert.AreEqual(pair3.GetHashCode(), pair.GetHashCode());
			Assert.AreNotEqual(pair3.GetHashCode(), pair2.GetHashCode());

			Assert.AreEqual(true, pair3.Equals(pair));
			Assert.AreEqual(false, pair3.Equals(pair2));
		}

		[TestCase()]
		public void TupleTest()
		{
			TupleReference<Int32, Int32> pair = new TupleReference<int, int>(55, 555);
			Assert.AreEqual("Item1 = 55; Item2 = 555; ", pair.ToString());
			TupleReference<Int32, Int32> pair2 = new TupleReference<int, int>(55, 55);

			TupleReference<Int32, Int32> pair3 = new TupleReference<int, int>(55, 555);
			Assert.AreEqual(pair3.GetHashCode(), pair.GetHashCode());
			Assert.AreNotEqual(pair3.GetHashCode(), pair2.GetHashCode());

			Assert.AreEqual(true, pair3.Equals(pair));
			Assert.AreEqual(false, pair3.Equals(pair2));
		}

		[TestCase()]
		public void TupleValueTest()
		{
			TupleValue<Int32, Int32> pair = new TupleValue<int, int>(55, 555);
			Assert.AreEqual("Item1 = 55; Item2 = 555; ", pair.ToString());
			TupleValue<Int32, Int32> pair2 = new TupleValue<int, int>(55, 55);

			TupleValue<Int32, Int32> pair3 = new TupleValue<int, int>(55, 555);
			Assert.AreEqual(pair3.GetHashCode(), pair.GetHashCode());
			Assert.AreNotEqual(pair3.GetHashCode(), pair2.GetHashCode());

			Assert.AreEqual(true, pair3.Equals(pair));
			Assert.AreEqual(false, pair3.Equals(pair2));
		}

	}
}

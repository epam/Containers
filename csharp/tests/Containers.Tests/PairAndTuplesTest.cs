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
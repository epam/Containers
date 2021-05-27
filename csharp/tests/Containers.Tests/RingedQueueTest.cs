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
	[TestFixture()]
    public class RingedQueueTest
    {
		[TestCase()]
		public void RingedQueueStressTest()
		{
			RingedQueue<BinaryArray> queue = new RingedQueue<BinaryArray>();
			BufferedLinkedList<BinaryArray> etalon = new BufferedLinkedList<BinaryArray>();

			Random rand = new Random(55);
			int iterations = 1000000;

			for (int i = 0; i < iterations; ++i)
			{
				int type = rand.Next() % 3;
				if (Math.Abs(type) == 0 && queue.Count > 0)
				{
					Assert.AreEqual(true, queue.Pop().Equals(etalon.First));
					etalon.RemoveFirst();
				}
				else
				{
					BinaryArray array = new BinaryArray();
					array.Append(i);
					queue.Add(array);
					etalon.AddLast(array);
				}

				if (i % 100000 == 0)
				{
					int index = 0;
					int iterator = etalon.FirstKey;
					Assert.AreEqual(queue.Count, etalon.Count);
					foreach (BinaryArray element in queue)
					{
						Assert.AreEqual(true, element.Equals(etalon[iterator]));
						Assert.AreEqual(true, element == queue[index]);
						index++;
						iterator = etalon.Next(iterator);
					}
				}
			}
		}
	}
}
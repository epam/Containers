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
	class A
	{
		internal int x;
		internal A(int x)
		{
			this.x = x;	
		} 
	}
	[TestFixture]
    class MemoryManagerTest
    {
		Random random = new Random();
		APCustomMemoryManager<A> apCustomManager = new APCustomMemoryManager<A>(() => new A(555));
		GPCustomMemoryManager<A> gpCustomManager = new GPCustomMemoryManager<A>(() => new A(666));
		[TestCase()]
		public void CustomMemoryManagerTest()
		{
			Assert.AreEqual(555, apCustomManager.New().x);
			Assert.AreEqual(666, gpCustomManager.New().x);

			for (int i = 0; i < 1000; ++i)
			{
				Assert.AreEqual(555, apCustomManager.New().x);
				Assert.AreEqual(666, gpCustomManager.New().x);
			}
		}
		
    }
}
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
﻿using EPAM.Deltix.Time;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture]
	class TimeProviderTest
	{
		StringBuilder sb;

		[TestCase()]
		public void PriorityTest()
		{
			sb = new StringBuilder();
			ManualTimeProvider timeProvider = new ManualTimeProvider();
			timeProvider.AddBreakPoint(new HdDateTime(10), 10, AddToBuilder, 2);

			timeProvider.AddBreakPoint(new HdDateTime(10), 11, AddToBuilder, 1);

			timeProvider.GoToTime(new HdDateTime(20));
			Assert.AreEqual("1110", sb.ToString());
		}

		private void AddToBuilder(HdDateTime arg1, object arg2)
		{
			sb.Append(arg2.ToString());
		}
	}
}
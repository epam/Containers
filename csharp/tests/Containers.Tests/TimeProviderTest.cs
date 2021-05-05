using EPAM.Deltix.Time;
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

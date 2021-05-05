using NUnit.Framework;
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

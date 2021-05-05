using EPAM.Deltix.DFP;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	public class DataQueueTest
	{
		[TestCase()]
		public void ManualQueueTest()
		{
			DataQueue queue = new DataQueue(true, false);
			queue.PutLast(Decimal64.One);
			Assert.AreEqual(1, queue.Count);
			queue.PutLast(Decimal64.Two);
			Assert.AreEqual(Decimal64.One, queue.GetPointsAgo(1));
			Assert.AreEqual(Decimal64.Two, queue.GetPointsAgo(0));
			for (int i = 0; i < 100; i++)
			{
				queue.PutLast(Decimal64.FromDouble(i));
			}
			Assert.AreEqual(102, queue.Count);
			Assert.AreEqual(Decimal64.FromDouble(4), queue.GetPointsAgo(95));
			Assert.AreEqual(Decimal64.One, queue.First);
			queue.RemoveFirst();
			Assert.AreEqual(Decimal64.FromDouble(2), queue.GetPointsAgo(100));
			Assert.AreEqual(101, queue.Count);
			queue.PutFirst(Decimal64.One);
			Assert.AreEqual(Decimal64.FromDouble(1), queue.GetPointsAgo(101));

			Assert.AreEqual(Decimal64.FromDouble(4953), queue.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(328355), queue.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(4953), queue.Sum);
			Assert.AreEqual(48.559, queue.ExpectedValue.ToDouble(), 0.001);

			Assert.AreEqual(Decimal64.FromDouble(99), queue.Last);
			DataQueue queueClone = queue.Clone();
			queueClone.RemoveLast();
			Assert.AreEqual(Decimal64.FromDouble(98), queueClone.Last);
			Assert.AreEqual(Decimal64.FromDouble(99), queue.Last);

			Assert.AreEqual(Decimal64.FromDouble(4854), queueClone.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(318554), queueClone.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(4854), queueClone.Sum);

			queue.SetPointsAgo(1, Decimal64.Zero);
			Assert.AreEqual(Decimal64.FromDouble(0), queue.GetPointsAgo(1));
			Assert.AreEqual(queue.Last, queue.GetPointsAgo(0));
			Assert.AreEqual(Decimal64.FromDouble(4855), queue.Sum);

			var arr = queue.ToArray();
			int idx = 0;
			foreach(var x in queue)
			{
				Assert.AreEqual(x, arr[idx++]);
			}
		}

		[TestCase()]
		public void ManualQueueWithDatesTest()
		{
			DataQueue queue = new DataQueue(true, true);
			queue.PutLast(Decimal64.One, new DateTime(1));
			Assert.AreEqual(1, queue.Count);
			queue.PutLast(Decimal64.Two, new DateTime(2));
			Assert.AreEqual(Decimal64.One, queue.GetPointsAgo(1));
			Assert.AreEqual(new DateTime(1), queue.GetTimeByIndexAgo(1));
			Assert.AreEqual(Decimal64.Two, queue.GetPointsAgo(0));
			Assert.AreEqual(new DateTime(2), queue.GetTimeByIndexAgo(0));
			for (int i = 3; i < 100; i++)
			{
				queue.PutLast(Decimal64.FromDouble(i), new DateTime(i));
			}
			Assert.AreEqual(99, queue.Count);
			Assert.AreEqual(Decimal64.FromDouble(4), queue.GetPointsAgo(95));
			Assert.AreEqual(new DateTime(4), queue.GetTimeByIndexAgo(95));
			Assert.AreEqual(Decimal64.One, queue.First);
			Assert.AreEqual(new DateTime(1), queue.FirstDateTime);
			queue.RemoveFirst();
			Assert.AreEqual(Decimal64.FromDouble(2), queue.GetPointsAgo(97));
			Assert.AreEqual(new DateTime(2), queue.GetTimeByIndexAgo(97));
			Assert.AreEqual(98, queue.Count);
			queue.PutFirst(Decimal64.One, new DateTime(1));
			Assert.AreEqual(Decimal64.FromDouble(1), queue.GetPointsAgo(98));
			Assert.AreEqual(new DateTime(1), queue.GetTimeByIndexAgo(98));

			Assert.AreEqual(Decimal64.FromDouble(4950), queue.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(328350), queue.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(4950), queue.Sum);
			Assert.AreEqual(50, queue.ExpectedValue.ToDouble(), 0.001);

			DataQueue queueClone = queue.Clone();
			queueClone.RemoveLast();
			Assert.AreEqual(Decimal64.FromDouble(98), queueClone.Last);
			Assert.AreEqual(new DateTime(98), queueClone.LastDateTime);
			Assert.AreEqual(Decimal64.FromDouble(99), queue.Last);
			Assert.AreEqual(new DateTime(99), queue.LastDateTime);

			Assert.AreEqual(Decimal64.FromDouble(4851), queueClone.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(318549), queueClone.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(4851), queueClone.Sum);

			queue.SetPointsAgo(1, Decimal64.Zero);
			Assert.AreEqual(Decimal64.FromDouble(0), queue.GetPointsAgo(1));
			Assert.AreEqual(new DateTime(98), queue.GetTimeByIndexAgo(1));
			Assert.AreEqual(queue.Last, queue.GetPointsAgo(0));
			Assert.AreEqual(Decimal64.FromDouble(4852), queue.Sum);

			Assert.AreEqual(74, queue.GetIndexByTime(new DateTime(25)));
			Assert.AreEqual(89, queue.GetIndexByTime(new DateTime(10)));
			Assert.AreEqual(Decimal64.FromDouble(25), queue.GetByTime(new DateTime(25)));
			queue.SetByTime(new DateTime(25), Decimal64.FromDouble(100));
			Assert.AreEqual(Decimal64.FromDouble(4927), queue.Sum);
		}

		[TestCase()]
		public void AutoDynamicQueueTest()
		{
			TimeSpan period = new TimeSpan(20);
			DataQueue queue = new DataQueue(20, period, true);
			queue.Put(Decimal64.One, new DateTime(1));
			Assert.AreEqual(1, queue.Count);
			queue.Put(Decimal64.Two, new DateTime(2));
			Assert.AreEqual(Decimal64.One, queue.GetPointsAgo(1));
			Assert.AreEqual(new DateTime(1), queue.GetTimeByIndexAgo(1));
			Assert.AreEqual(Decimal64.Two, queue.GetPointsAgo(0));
			Assert.AreEqual(new DateTime(2), queue.GetTimeByIndexAgo(0));
			for (int i = 3; i < 100; i++)
			{
				queue.Put(Decimal64.FromDouble(i), new DateTime(i));
			}
			Assert.AreEqual(period, queue.LastDateTime - queue.FirstDateTime);
			Assert.AreEqual(21, queue.Count);
			Assert.AreEqual(Decimal64.FromDouble(79), queue.GetPointsAgo(20));
			Assert.AreEqual(new DateTime(79), queue.GetTimeByIndexAgo(20));
			Assert.AreEqual(Decimal64.FromDouble(79), queue.First);
			Assert.AreEqual(new DateTime(79), queue.FirstDateTime);
			Assert.AreEqual(Decimal64.FromDouble(98), queue.Previous);


			Assert.AreEqual(Decimal64.FromDouble(1869), queue.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(167111), queue.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(1869), queue.Sum);
			Assert.AreEqual(89, queue.ExpectedValue.ToDouble(), 0.001);

			DataQueue queueClone = queue.Clone();
			queueClone.Put(Decimal64.FromDouble(100), new DateTime(100));
			Assert.AreEqual(Decimal64.FromDouble(100), queueClone.GetPointsAgo(0));
			Assert.AreEqual(new DateTime(100), queueClone.GetTimeByIndexAgo(0));
			Assert.AreEqual(Decimal64.FromDouble(99), queue.GetPointsAgo(0));
			Assert.AreEqual(new DateTime(99), queue.GetTimeByIndexAgo(0));

			Assert.AreEqual(Decimal64.FromDouble(1890), queueClone.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(170870), queueClone.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(1890), queueClone.Sum);

			queue.SetPointsAgo(1, Decimal64.Zero);
			Assert.AreEqual(Decimal64.FromDouble(0), queue.GetPointsAgo(1));
			Assert.AreEqual(new DateTime(98), queue.GetTimeByIndexAgo(1));
			Assert.AreEqual(queue.Last, queue.GetPointsAgo(0));
			Assert.AreEqual(Decimal64.FromDouble(1771), queue.Sum);

			Assert.AreEqual(-1, queue.GetIndexByTime(new DateTime(25)));
			Assert.AreEqual(19, queue.GetIndexByTime(new DateTime(80)));
			Assert.AreEqual(DataQueue.DefaultValue, queue.GetByTime(new DateTime(25)));
			queue.SetByTime(new DateTime(80), Decimal64.FromDouble(100));
			Assert.AreEqual(Decimal64.FromDouble(1791), queue.Sum);

			Assert.AreEqual(period, queue.Period);
			Assert.AreEqual(new DateTime(99), queue.LastDateTime);
			Assert.AreEqual(Decimal64.FromDouble(79), queue.ToArray()[0]);
			Assert.AreEqual(Decimal64.FromDouble(79), queue.First);
		}

		[TestCase()]
		public void AutoStaticQueueTest()
		{
			DataQueue queue = new DataQueue(20, true, true);
			queue.Put(Decimal64.One, new DateTime(1));
			Assert.AreEqual(1, queue.Count);
			queue.Put(Decimal64.Two, new DateTime(2));
			Assert.AreEqual(Decimal64.One, queue.GetPointsAgo(1));
			Assert.AreEqual(new DateTime(1), queue.GetTimeByIndexAgo(1));
			Assert.AreEqual(Decimal64.Two, queue.GetPointsAgo(0));
			Assert.AreEqual(new DateTime(2), queue.GetTimeByIndexAgo(0));
			for (int i = 3; i < 100; i++)
			{
				if (i % 2 == 0)
					queue.Put(Decimal64.FromDouble(i), new DateTime(i));
				else queue.Put(Decimal64.FromDouble(-i), new DateTime(i));
			}
			Assert.AreEqual(20, queue.Count);
			Assert.AreEqual(Decimal64.FromDouble(80), queue.GetPointsAgo(19));
			Assert.AreEqual(new DateTime(80), queue.GetTimeByIndexAgo(19));
			Assert.AreEqual(Decimal64.FromDouble(80), queue.First);
			Assert.AreEqual(new DateTime(80), queue.FirstDateTime);
			Assert.AreEqual(Decimal64.FromDouble(98), queue.Previous);


			Assert.AreEqual(Decimal64.FromDouble(1790), queue.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(160870), queue.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(-10), queue.Sum);
			Assert.AreEqual(-0.5, queue.ArithmeticMean.ToDouble(), 0.01);
			Assert.AreEqual(8043.25, queue.Variance.ToDouble(), 0.01);
			Assert.AreEqual(-179.36, queue.CoefficientOfVariation.ToDouble(), 0.01);
			Assert.AreEqual(-0.5, queue.ExpectedValue.ToDouble(), 0.001);

			DataQueue queueClone = queue.Clone();
			queueClone.Put(Decimal64.FromDouble(100), new DateTime(100));
			Assert.AreEqual(Decimal64.FromDouble(100), queueClone.GetPointsAgo(0));
			Assert.AreEqual(new DateTime(100), queueClone.GetTimeByIndexAgo(0));
			Assert.AreEqual(Decimal64.FromDouble(-99), queue.GetPointsAgo(0));
			Assert.AreEqual(new DateTime(99), queue.GetTimeByIndexAgo(0));

			Assert.AreEqual(Decimal64.FromDouble(1810), queueClone.SumOfAbsoluteValues);
			Assert.AreEqual(Decimal64.FromDouble(164470), queueClone.SumOfSquares);
			Assert.AreEqual(Decimal64.FromDouble(10), queueClone.Sum);
			Assert.AreEqual(20, queueClone.Count);

			queue.SetPointsAgo(1, Decimal64.Zero);
			Assert.AreEqual(Decimal64.FromDouble(0), queue.GetPointsAgo(1));
			Assert.AreEqual(new DateTime(98), queue.GetTimeByIndexAgo(1));
			Assert.AreEqual(queue.Last, queue.GetPointsAgo(0));
			Assert.AreEqual(Decimal64.FromDouble(-108), queue.Sum);
			Assert.AreEqual(20, queueClone.Count);

			Assert.AreEqual(-1, queue.GetIndexByTime(new DateTime(25)));
			Assert.AreEqual(19, queue.GetIndexByTime(new DateTime(80)));
			Assert.AreEqual(DataQueue.DefaultValue, queue.GetByTime(new DateTime(25)));
			queue.SetByTime(new DateTime(80), Decimal64.FromDouble(100));
			Assert.AreEqual(Decimal64.FromDouble(-88), queue.Sum);

			Assert.AreEqual(new DateTime(99), queue.LastDateTime);
			Assert.AreEqual(Decimal64.FromDouble(100), queue.ToArray()[0]);
			Assert.AreEqual(Decimal64.FromDouble(100), queue.First);
			queue.Clear();
			Assert.AreEqual(0, queue.Count);
		}

		[TestCase()]
		public void SingleElementQueueTest()
		{
			DataQueue queue = new DataQueue(1, true, true);
			queue.Put(Decimal64.One, new DateTime(1));
			Assert.AreEqual(Decimal64.FromDouble(1), queue.Sum);
			Assert.AreEqual(1, queue.Count);
			queue.Put(Decimal64.Two, new DateTime(2));
			Assert.AreEqual(Decimal64.FromDouble(2), queue.Sum);
			Assert.AreEqual(Decimal64.One, queue.GetPointsAgo(1));
			Assert.AreEqual(Decimal64.One, queue.Previous);
			Assert.AreEqual(Decimal64.Two, queue.First);
			Assert.AreEqual(Decimal64.Two, queue.Last);
			Assert.AreEqual(new DateTime(2), queue.FirstDateTime);
			Assert.AreEqual(new DateTime(2), queue.LastDateTime);
			Assert.AreEqual(new DateTime(1), queue.GetTimeByIndexAgo(1));
			Assert.AreEqual(1, queue.Count);
			queue.SetPointsAgo(0, Decimal64.Ten);
			Assert.AreEqual(Decimal64.FromDouble(10), queue.Sum);

			DataQueue queueClone = queue.Clone();
			Assert.AreEqual(Decimal64.FromDouble(10), queueClone.Sum);
			queueClone.Put(Decimal64.One, new DateTime(55));
			Assert.AreEqual(Decimal64.Ten, queue.First);
			Assert.AreEqual(Decimal64.FromDouble(1), queueClone.First);
		}

		private int callbacksCallsCount = 0;
		private DateTime lastTime;
		private Decimal64 lastDate;

		[TestCase()]
		public void QueueListenersTest()
		{
			DataQueue queue = new DataQueue(20, true, true);

			queue.OnPush += delegate (Decimal64 data, DateTime time) {
				callbacksCallsCount++;
				lastTime = time;
			};
			for (int i = 1; i <= 100; i++) {
				if (i % 2 == 0)
					queue.Put(Decimal64.FromDouble(i), new DateTime(i));
				else queue.Put(Decimal64.FromDouble(-i), new DateTime(i));
			}

			Assert.AreEqual(100, callbacksCallsCount);
			Assert.AreEqual(new DateTime(100), lastTime);
			queue.OnPop += delegate (Decimal64 data, DateTime time) {
				callbacksCallsCount++;
				lastDate = data;
			};
			callbacksCallsCount = 0;
			for (int i = 1; i <= 10; i++)
				queue.Put(Decimal64.Zero, new DateTime(100 + i));
			Assert.AreEqual(20, callbacksCallsCount);
			Assert.AreEqual(Decimal64.FromDouble(90), lastDate);
		}
	}
}

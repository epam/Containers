using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture()]
	class AVLTreeTest
	{
		[Test]
		public void AVLTreeStressTest(
		[Values(100000)] int count,
		[Values(4)] int testsCount,
		[Random(1)] int randomSeed)
		{
			int requestsCount = 5;
			int emptyRequest = 7;

			Random r = new Random(randomSeed);

			for (int testNumber = 0; testNumber < testsCount; testNumber++)
			{
				int current = 0;

				var map = new Deltix.Containers.AvlTree<Double, Double>();

				double[] dataFirst = new double[count];
				double[] dataSecond = new double[count];

				Deltix.Containers.AvlTree<Double, Double>.AvlTreeIterator it;

				double[] request = new double[count];
				int[] requestType = new int[count];
				double[] answer = new double[count];

				for (int requestNumber = 0; requestNumber < count; requestNumber++)
				{
					requestType[requestNumber] = r.Next() % requestsCount;
					switch (requestType[requestNumber])
					{
						case 0:
						case 1:
						case 2:
							{
								request[requestNumber] = r.NextDouble();
								answer[requestNumber] = r.NextDouble();
								dataFirst[current] = request[requestNumber];
								dataSecond[current] = answer[requestNumber];

								int j = current++;

								while (j > 0 && dataFirst[j - 1] > dataFirst[j])
								{
									double tFirst = dataFirst[j - 1];
									double tSecond = dataSecond[j - 1];

									dataFirst[j - 1] = dataFirst[j];
									dataFirst[j] = tFirst;

									dataSecond[j - 1] = dataSecond[j];
									dataSecond[j] = tSecond;

									j--;
								}
							}
							break;

						case 3:
							{
								if (current > 0)
								{
									request[requestNumber] = r.Next(current);
									answer[requestNumber] = dataFirst[(int)request[requestNumber]];
								}
								else
									requestType[requestNumber] = emptyRequest;
							}
							break;

						case 4:
							{
								if (current > 0)
								{
									int p = r.Next(current);

									request[requestNumber] = dataFirst[p];
									for (; p < current - 1; p++)
									{
										double tFirst = dataFirst[p];
										double tSecond = dataSecond[p];

										dataFirst[p] = dataFirst[p + 1];
										dataSecond[p] = dataSecond[p + 1];

										dataFirst[p + 1] = tFirst;
										dataSecond[p + 1] = tSecond;
									}
									current--;
								}
								else
									requestType[requestNumber] = emptyRequest;
							}
							break;
					}
				}

				for (int requestNumber = 0; requestNumber < count; requestNumber++)
				{
					switch (requestType[requestNumber])
					{
						case 0:
						case 1:
						case 2:
							{
								map.Add(request[requestNumber], answer[requestNumber]);
							}
							break;

						case 3:
							{
								it = map[(int)request[requestNumber]];
								if (!it.Valid || it.Key != answer[requestNumber])
								{
									throw new ArithmeticException("AVL tree test failed. Expected: " + answer[requestNumber] + " Received: " + it.Key);
								}
							}
							break;

						case 4:
							{
								if (current > 0)
								{
									map.Remove(request[requestNumber]);
								}
							}
							break;
					}
				}

				it = map.First;
				for (int i = 0; i < current; i++, it.Next())
				{
					if (!it.Valid || it.Key != dataFirst[i])
					{
						throw new ArithmeticException("AVL tree test failed. Expected: " + dataFirst[i] + " Received: " + it.Key);
					}
				}

				it = map.Last;

				for (int i = current - 1; i >= 0; i--, it.Previous())
				{
					if (!it.Valid || it.Key != dataFirst[i])
					{
						throw new ArithmeticException("AVL tree test failed. Expected: " + dataFirst[i] + " Received: " + it.Key);
					}
				}
			}
		}
	}
}

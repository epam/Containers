using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using RTMath.Containers;

namespace LargestSmallestBenchmark
{
	//[ClrJob, CoreJob]
	//[RPlotExporter, RankColumn]
	public class LargestSmallestBenchmark
	{
		private double[] dataIn, dataOut;
		private int[] dataInd;
		private IList<double> dataInIList, dataOutIList;
		private IList<int> dataIndIList;

		[Params(1000000)] public int N;

		[Params(20)] public int M;

		[Params(42)] public int Seed;

		[GlobalSetup]
		public void Setup()
		{
			dataIn = new double[N];
			var random = new Random(Seed);
			for (int i = 0; i < dataIn.Length; ++i)
				dataIn[i] = random.NextDouble();

			dataOut = new double[M];
			dataInd = new int[M];

			dataInIList = dataIn;
			dataOutIList = dataOut;
			dataIndIList = dataInd;
		}

		[Benchmark]
		public void Smallest()
		{
			dataIn.Smallest(dataOut, false);
		}

		[Benchmark]
		public void SmallestIndices()
		{
			dataIn.SmallestIndices(dataOut, dataInd, false);
		}

		[Benchmark]
		public void Largest()
		{
			dataIn.Largest(dataOut, false);
		}

		[Benchmark]
		public void SmallestIList()
		{
			dataInIList.Smallest(dataOutIList, false);
		}

		[Benchmark]
		public void SmallestIndicesIList()
		{
			dataInIList.SmallestIndices(dataOutIList, dataIndIList, false);
		}

		[Benchmark]
		public void LargestIList()
		{
			dataInIList.Largest(dataOutIList, false);
		}
	}
}

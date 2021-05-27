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
﻿using System;
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
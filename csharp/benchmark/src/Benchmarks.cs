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
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
	public class Benchmarks
	{
		public static void Main(string[] args)
		{
#if NET46
			var thisClasses = typeof(Benchmarks).Assembly.GetTypes();
#else
			var thisClasses = typeof(Benchmarks).GetTypeInfo().Assembly.GetTypes();
#endif

			if (args.Length == 0)
			{
				foreach (var thisClass in thisClasses)
				{
					if (thisClass != typeof(Benchmarks))
						BenchmarkRunner.Run(thisClass);
				}
			}
			else
			{
				foreach (var className in args)
				{
					bool isClassFound = false;
					foreach (var thisClass in thisClasses)
					{
						if (thisClass.Name == className || thisClass.FullName == className)
						{
							BenchmarkRunner.Run(thisClass);
							isClassFound = true;
							break;
						}
					}

					if (!isClassFound)
						Console.WriteLine($"Can't find {className} class in the assembly.");
				}
			}
		}
	}
}
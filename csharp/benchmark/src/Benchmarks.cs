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

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RTMath.Containers;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace RTMath.Containers.Tests
{
	class Program
	{
		private static unsafe IntPtr GetPointer<Struct>(Struct[] input)
		{
			Type[] methodArgs = { typeof(Struct[]) };
			DynamicMethod getPointer = new DynamicMethod("GetPointer1", typeof(IntPtr), methodArgs, Assembly.GetExecutingAssembly().ManifestModule);
			ILGenerator generator = getPointer.GetILGenerator();
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldc_I4_0);
			generator.Emit(OpCodes.Ldelema, typeof(Struct));
			generator.Emit(OpCodes.Conv_I);
			generator.Emit(OpCodes.Ret);
			object[] args = { input };
			Func<Struct[], IntPtr> myDelegate = (Func<Struct[], IntPtr>)getPointer.CreateDelegate(typeof(Func<Struct[], IntPtr>));
			return myDelegate(input);
		}

		static void Main(string[] args)
		{
			HeapBenchmark heapBenchmark = new HeapBenchmark();
			heapBenchmark.DoubleHeapModifyTopTest(1000000, 250);
			heapBenchmark.DoubleHeapTest(1000000, 250);
			IBinaryArrayReadOnly ar1 = new BinaryArray();
			BinaryArray ar = new BinaryArray(ar1);
		}
	}
}
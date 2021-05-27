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
using System.Diagnostics;
using NUnit.Framework;
using EPAM.Deltix.Containers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
namespace EPAM.Deltix.Containers.Tests{
	public enum BinaryArrayTestType
	{
		Byte,
		BinaryArray,
		ArrayOfByte,
		Boolean,
		Char,
		DateTime,
		Decimal,
		Double,
		Int16,
		Int32,
		Int64,
		UInt16,
		UInt32,
		UInt64,
		SByte,
		Single,
		Guid,
		MutableString,
		String,
		ByteArray,
		ByteArrayWithOffset,
		BytePtr,
		ByteEnumerable,
		SByteArray,
		SByteArrayWithOffset,
		SBytePtr,
		SByteEnumerable,
		ByteArrayWithNew,
		UTF8

	}

	public enum IListTestType
	{
		Contains,
		IndexOf,
		CopyTo,
		RemoveAt,
		Insert,
		Remove,
		Equals,
		Clone,
		CompareTo
	}
	[TestFixture]
	class BinaryArrayUnitTest
	{
		private BinaryArray a = new BinaryArray();
		private byte[] ar;
		private Byte[] ar1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		private Byte[] resultArray, resultArray1;
		private sbyte[] sAr1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
		private sbyte[] sar;
		private Byte[] ar3 = {48, 49, 50, 51, 52, 53, 54, 55};

		/*[TestCase()]
		public void ReadOnlyStringTest()
		{
			BinaryArray ar = new BinaryArray();
			BinaryArray ar1 = new BinaryArray();
			BinaryArray ar2 = new BinaryArray();
			ar.Assign("saddsad");
			ar1.Assign(ar);
			ar2.Assign((IReadOnlyString)ar);
			Assert.AreEqual(true, ar1.Equals(ar2));
		}*/

		[TestCase(BinaryArrayTestType.BinaryArray)]
		[TestCase(BinaryArrayTestType.Boolean)]
		[TestCase(BinaryArrayTestType.Char)]
		[TestCase(BinaryArrayTestType.DateTime)]
		[TestCase(BinaryArrayTestType.Decimal)]
		[TestCase(BinaryArrayTestType.Double)]
		[TestCase(BinaryArrayTestType.Guid)]
		[TestCase(BinaryArrayTestType.Int16)]
		[TestCase(BinaryArrayTestType.Int32)]
		[TestCase(BinaryArrayTestType.Int64)]
		[TestCase(BinaryArrayTestType.MutableString)]
		[TestCase(BinaryArrayTestType.SByte)]
		[TestCase(BinaryArrayTestType.Single)]
		[TestCase(BinaryArrayTestType.UInt16)]
		[TestCase(BinaryArrayTestType.UInt32)]
		[TestCase(BinaryArrayTestType.UInt64)]
		[TestCase(BinaryArrayTestType.ArrayOfByte)]
		[TestCase(BinaryArrayTestType.String)]
		[TestCase(BinaryArrayTestType.ByteEnumerable)]
		[TestCase(BinaryArrayTestType.BytePtr)]
		[TestCase(BinaryArrayTestType.ByteArray)]
		[TestCase(BinaryArrayTestType.ByteArrayWithOffset)]
		[TestCase(BinaryArrayTestType.SByteEnumerable)]
		[TestCase(BinaryArrayTestType.SBytePtr)]
		[TestCase(BinaryArrayTestType.SByteArray)]
		[TestCase(BinaryArrayTestType.SByteArrayWithOffset)]
		[TestCase(BinaryArrayTestType.Byte)]
		[TestCase(BinaryArrayTestType.ByteArrayWithNew)]
		[TestCase(BinaryArrayTestType.UTF8)]

		
		public void TestAssignAndToType(BinaryArrayTestType type)
		{
			Array.Resize(ref resultArray, 10);
			if (type == BinaryArrayTestType.Boolean)
			{
				a.Assign(true);
				Assert.AreEqual(a.ToBoolean(), true);
				a.Assign(false);
				Assert.AreEqual(a.ToBoolean(), false);
			}
			if (type == BinaryArrayTestType.Char)
			{
				a.Assign('a');
				Assert.AreEqual(a.ToChar(), 'a');
				a.Assign('.');
				Assert.AreEqual(a.ToChar(), '.');
			}
			if (type == BinaryArrayTestType.DateTime)
			{
				DateTime time = DateTime.Now;
				a.Assign(time);
				Assert.AreEqual(a.ToDateTime(), time);
				a.Assign(DateTime.MaxValue);
				Assert.AreEqual(a.ToDateTime(), DateTime.MaxValue);
			}
			if (type == BinaryArrayTestType.Decimal)
			{
				Decimal x = 468;
				a.Assign(x);
				Assert.AreEqual(a.ToDecimal(), x);
				x = 98.4867M;
				a.Assign(x);
				Assert.AreEqual(a.ToDecimal(), x);
			}
			if (type == BinaryArrayTestType.Double)
			{
				Double x = 3.14;
				a.Assign(x);
				Assert.AreEqual(a.ToDouble(), x);
				x = 2.71;
				a.Assign(x);
				Assert.AreEqual(a.ToDouble(), x);
			}
			if (type == BinaryArrayTestType.Guid)
			{
				Guid x = new Guid(100, 10, 5, 100, 100, 78, 35, 12, 68, 17, 34);
				a.Assign(x);
				Assert.AreEqual(a.ToGuid(), x);
				x = new Guid(7, 78, 13, 54, 6, 47, 41, 68, 88, 6, 67);
				a.Assign(x);
				Assert.AreEqual(a.ToGuid(), x);
			}
			if (type == BinaryArrayTestType.Int16)
			{
				Int16 x = 8974;
				a.Assign(x);
				Assert.AreEqual(a.ToInt16(), x);
				x = -30000;
				a.Assign(x);
				Assert.AreEqual(a.ToInt16(), x);
			}
			if (type == BinaryArrayTestType.Int32)
			{
				Int32 x = 3245436;
				a.Assign(x);
				Assert.AreEqual(a.ToInt32(), x);
				x = -654678;
				a.Assign(x);
				Assert.AreEqual(a.ToInt32(), x);
			}
			if (type == BinaryArrayTestType.Int64)
			{
				Int64 x = 3546546245436;
				a.Assign(x);
				Assert.AreEqual(a.ToInt64(), x);
				x = -654654654278;
				a.Assign(x);
				Assert.AreEqual(a.ToInt64(), x);
			}
			if (type == BinaryArrayTestType.MutableString)
			{
				MutableString x = new MutableString("sadasd");
				MutableString result = new MutableString();
				a.Assign(x);
				a.ToMutableString(result);
				Assert.AreEqual(x, result);
				x = new MutableString("fdks;fdskf");
				a.Assign(x);
				a.ToMutableString(result);
				Assert.AreEqual(x, result);
			}
			if (type == BinaryArrayTestType.SByte)
			{
				SByte x = -45;
				a.Assign(x);
				Assert.AreEqual(a.ToSByte(), x);
				x = 89;
				a.Assign(x);
				Assert.AreEqual(a.ToSByte(), x);
			}
			if (type == BinaryArrayTestType.Single)
			{
				Single x = 87.54F;
				a.Assign(x);
				Assert.AreEqual(a.ToSingle(), x);
				x = 1.59F;
				a.Assign(x);
				Assert.AreEqual(a.ToSingle(), x);
			}
			if (type == BinaryArrayTestType.UInt16)
			{
				UInt16 x = 8974;
				a.Assign(x);
				Assert.AreEqual(a.ToUInt16(), x);
				x = 50000;
				a.Assign(x);
				Assert.AreEqual(a.ToUInt16(), x);
			}
			if (type == BinaryArrayTestType.UInt32)
			{
				UInt32 x = 3245436;
				a.Assign(x);
				Assert.AreEqual(a.ToUInt32(), x);
				x = 654678;
				a.Assign(x);
				Assert.AreEqual(a.ToUInt32(), x);
			}
			if (type == BinaryArrayTestType.UInt64)
			{
				UInt64 x = 3546546245436;
				a.Assign(x);
				Assert.AreEqual(a.ToUInt64(), x);
				x = 654654654278;
				a.Assign(x);
				Assert.AreEqual(a.ToUInt64(), x);
			}
			if (type == BinaryArrayTestType.BinaryArray)
			{
				BinaryArray b = new BinaryArray();
				b.Assign('q');
				a.Assign(b as IBinaryConvertibleReadWrite);
				Assert.AreEqual(a.ToChar(), 'q');
				Assert.AreEqual(a, b);
			}
			if (type == BinaryArrayTestType.ArrayOfByte)
			{
				byte[] ar = { 1, 2, 3, 4, 148 };
				byte[] ar1 = { };
				Array.Resize(ref ar1, 6);
				unsafe
				{

					fixed (byte* ptr = ar)
					{
						a.Assign(ptr, 5);
						a.ToByteArray(0, ar1, 0, 5);
						Assert.AreEqual(ar1[0], 1);
						Assert.AreEqual(ar1[1], 2);
						Assert.AreEqual(ar1[2], 3);
						Assert.AreEqual(ar1[3], 4);
						Assert.AreEqual(ar1[4], 148);


					}
				}
			}
			if (type == BinaryArrayTestType.String)
			{
				a.Assign("abacaba");
				Assert.AreEqual(a.ToString(), "abacaba");
				a.Assign("dskfdsf");
				Assert.AreEqual(a.ToString(), "dskfdsf");
				a.Assign("abc");
				Assert.AreEqual(a.ToString(), "abc");
				a.Assign(Encoding.ASCII.GetString(ar3), true);
				Assert.AreEqual(a.ToString(true), "01234567");
			}
			if (type == BinaryArrayTestType.ByteArray)
			{
				a.Assign(ar1);
				a.ToByteArray(0, resultArray, 0, a.Count);
				for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, resultArray[i - 1]);
			}
			if (type == BinaryArrayTestType.ByteArrayWithOffset)
			{
				a.Assign(ar1, 2, 5);
				a.ToByteArray(0, resultArray, 0, a.Count);
				for (int i = 0; i < 5; ++i) Assert.AreEqual(i + 3, resultArray[i]);
			}
			if (type == BinaryArrayTestType.BytePtr)
			{
				unsafe
				{
					fixed (byte* ptr = ar1)
					{
						a.Assign(ptr, 10);
						a.ToByteArray(0, resultArray, 0, a.Count);
						for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, resultArray[i - 1]);
					}
				}
			}
			if (type == BinaryArrayTestType.ByteEnumerable)
			{
				List<Byte> list = new List<byte>();
				list.Add(4);
				list.Add(7);
				list.Add(9);
				a.Assign(list);
				Assert.AreEqual(a[0], 4);
				Assert.AreEqual(a[1], 7);
				Assert.AreEqual(a[2], 9);
			}

			if (type == BinaryArrayTestType.SByteArray)
			{
				a.Assign(sAr1);
				Array.Resize(ref sar, 10);
				for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, a.ToSByte((i - 1) * sizeof(sbyte)));
				a.ToSByteArray(0, sar, 0, 10);
				for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, sar[i - 1]);
				a.ToSByteArray(0, sar, 5, 5);
				for (int i = 6; i <= 10; ++i) Assert.AreEqual(i - 5, sar[i - 1]);

			}
			if (type == BinaryArrayTestType.SByteArrayWithOffset)
			{
				a.Assign(sAr1, 2, 5);

				for (int i = 0; i < 5; ++i) Assert.AreEqual(i + 3, a.ToSByte(i * sizeof(sbyte)));
			}
			if (type == BinaryArrayTestType.SBytePtr)
			{
				unsafe
				{
					fixed (sbyte* ptr = sAr1)
					{
						a.Assign(ptr, 10);
						for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, a.ToSByte((i - 1) * sizeof(sbyte)));
					}
				}
			}
			if (type == BinaryArrayTestType.SByteEnumerable)
			{
				List<SByte> list = new List<sbyte>();
				list.Add(4);
				list.Add(7);
				list.Add(9);
				a.Assign(list);
				Assert.AreEqual(a.ToSByte(0), 4);
				Assert.AreEqual(a.ToSByte(sizeof(sbyte)), 7);
				Assert.AreEqual(a.ToSByte(sizeof(sbyte) * 2), 9);
			}
			if (type == BinaryArrayTestType.Byte)
			{
				a.Assign((byte)58);
				Assert.AreEqual(58, a.ToByte());
				a.Clear();
				a.Append((byte)58);
				Assert.AreEqual(58, a.ToByte());
			}
			if (type == BinaryArrayTestType.ByteArrayWithNew)
			{
				a.Clear();
				a.Append((byte)123);
				a.Append((byte)11);
				a.Append((byte)78);
				Byte[] array = a.ToByteArray();
				Assert.AreEqual(array.Length, 3);
				Assert.AreEqual(array[0], 123);
				Assert.AreEqual(array[1], 11);
				Assert.AreEqual(array[2], 78);
			}
			if (type == BinaryArrayTestType.UTF8)
			{
				a.Clear();
				a.Append("abacaba");
				Array.Resize(ref resultArray, 100);
				a.ToUTF8(resultArray);
				String s = "abacaba";
				resultArray1 = Encoding.UTF8.GetBytes(s);
				for (int i = 0; i < 7; ++i) Assert.AreEqual(resultArray[i * 2], resultArray1[i]);
			}
		}

		[TestCase(BinaryArrayTestType.Boolean)]
		[TestCase(BinaryArrayTestType.Char)]
		[TestCase(BinaryArrayTestType.DateTime)]
		[TestCase(BinaryArrayTestType.Decimal)]
		[TestCase(BinaryArrayTestType.Double)]
		[TestCase(BinaryArrayTestType.Guid)]
		[TestCase(BinaryArrayTestType.Int16)]
		[TestCase(BinaryArrayTestType.Int32)]
		[TestCase(BinaryArrayTestType.Int64)]
		[TestCase(BinaryArrayTestType.MutableString)]
		[TestCase(BinaryArrayTestType.SByte)]
		[TestCase(BinaryArrayTestType.Single)]
		[TestCase(BinaryArrayTestType.UInt16)]
		[TestCase(BinaryArrayTestType.UInt32)]
		[TestCase(BinaryArrayTestType.UInt64)]
		[TestCase(BinaryArrayTestType.String)]
		[TestCase(BinaryArrayTestType.Byte)]
		[TestCase(BinaryArrayTestType.UTF8)]
		public void ToTypeWithOffsetTest(BinaryArrayTestType type)
		{
			if (type == BinaryArrayTestType.Boolean)
			{
				a.Assign(159);
				a.Append(true);
				Assert.AreEqual(a.ToBoolean(4), true);
				a.Assign((byte)2);
				a.Append(false);
				Assert.AreEqual(a.ToBoolean(1), false);
			}
			if (type == BinaryArrayTestType.Char)
			{
				a.Assign((Int64)10);
				a.Append('a');
				Assert.AreEqual(a.ToChar(8), 'a');
				a.Assign((Guid)Guid.Empty);
				a.Append('s');
				Assert.AreEqual(a.ToChar(16), 's');
			}
			if (type == BinaryArrayTestType.DateTime)
			{
				a.Assign((Int64)10);
				a.Append(DateTime.MaxValue);
				Assert.AreEqual(a.ToDateTime(8), DateTime.MaxValue);
				a.Assign((Guid)Guid.Empty);
				a.Append(DateTime.MinValue);
				Assert.AreEqual(a.ToDateTime(16), DateTime.MinValue);
			}
			if (type == BinaryArrayTestType.Decimal)
			{
				a.Assign((Int64)10);
				a.Append((decimal)3.14);
				Assert.AreEqual(a.ToDecimal(8), (decimal)3.14);
				a.Assign((Guid)Guid.Empty);
				a.Append((decimal)2.71);
				Assert.AreEqual(a.ToDecimal(16), (decimal)2.71);
			}
			if (type == BinaryArrayTestType.Double)
			{
				a.Assign((Int64)10);
				a.Append((double)3.14);
				Assert.AreEqual(a.ToDouble(8), (double)3.14);
				a.Assign((Guid)Guid.Empty);
				a.Append((double)2.71);
				Assert.AreEqual(a.ToDouble(16), (double)2.71);
			}
			if (type == BinaryArrayTestType.Guid)
			{
				a.Assign((Int64)10);
				a.Append(Guid.Empty);
				Assert.AreEqual(a.ToGuid(8), Guid.Empty);
				a.Assign((Guid)Guid.Empty);
				a.Append(new Guid(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
				Assert.AreEqual(a.ToGuid(16), new Guid(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));
			}
			if (type == BinaryArrayTestType.Int16)
			{
				a.Assign((Int64)10);
				a.Append((Int16)314);
				Assert.AreEqual(a.ToInt16(8), (Int16)314);
				a.Assign((Guid)Guid.Empty);
				a.Append((Int16)271);
				Assert.AreEqual(a.ToInt16(16), (Int16)271);
			}
			if (type == BinaryArrayTestType.Int32)
			{
				a.Assign((Int64)10);
				a.Append((Int32)314000);
				Assert.AreEqual(a.ToInt32(8), (Int32)314000);
				a.Assign((Guid)Guid.Empty);
				a.Append((Int32)270001);
				Assert.AreEqual(a.ToInt32(16), (Int32)270001);
			}
			if (type == BinaryArrayTestType.Int64)
			{
				a.Assign((Int64)10);
				a.Append((Int64)1e+18);
				Assert.AreEqual(a.ToInt64(8), (Int64)1e+18);
				a.Assign((Guid)Guid.Empty);
				a.Append((Int64)Int64.MinValue);
				Assert.AreEqual(a.ToInt64(16), Int64.MinValue);
			}
			if (type == BinaryArrayTestType.UInt16)
			{
				a.Assign((Int64)10);
				a.Append((UInt16)314);
				Assert.AreEqual(a.ToUInt16(8), (UInt16)314);
				a.Assign((Guid)Guid.Empty);
				a.Append((UInt16)271);
				Assert.AreEqual(a.ToUInt16(16), (UInt16)271);
			}
			if (type == BinaryArrayTestType.UInt32)
			{
				a.Assign((Int64)10);
				a.Append((UInt32)314000);
				Assert.AreEqual(a.ToUInt32(8), (UInt32)314000);
				a.Assign((Guid)Guid.Empty);
				a.Append((UInt32)270001);
				Assert.AreEqual(a.ToUInt32(16), (UInt32)270001);
			}
			if (type == BinaryArrayTestType.UInt64)
			{
				a.Assign((UInt64)10);
				a.Append((UInt64)1e+18);
				Assert.AreEqual(a.ToUInt64(8), (UInt64)1e+18);
				a.Assign((Guid)Guid.Empty);
				a.Append((UInt64)UInt64.MinValue);
				Assert.AreEqual(a.ToUInt64(16), UInt64.MinValue);
			}
			if (type == BinaryArrayTestType.SByte)
			{
				a.Assign((UInt64)10);
				a.Append((sbyte)63);
				Assert.AreEqual(a.ToSByte(8), (sbyte)63);
				a.Assign((Guid)Guid.Empty);
				a.Append(SByte.MinValue);
				Assert.AreEqual(a.ToSByte(16), sbyte.MinValue);
			}
			if (type == BinaryArrayTestType.Single)
			{
				a.Assign((UInt64)10);
				a.Append(Single.PositiveInfinity);
				Assert.AreEqual(a.ToSingle(8), Single.PositiveInfinity);
				a.Assign((Guid)Guid.Empty);
				a.Append((Single)1.25);
				Assert.AreEqual(a.ToSingle(16), 1.25);

			}
			if (type == BinaryArrayTestType.MutableString)
			{
				MutableString str = new MutableString();
				MutableString str1 = new MutableString();
				a.Assign((UInt64)10);
				str.Assign("sdsd");
				a.Append(str);
				a.ToMutableString(str1, 8);
				Assert.AreEqual(str, str1);
			}
			if (type == BinaryArrayTestType.Byte)
			{
				a.Assign((byte)56);
				a.Append((byte)8);
				a.Append((byte)7);
				a.Add((byte)20);
				Assert.AreEqual(56, a.ToByte(0));
				Assert.AreEqual(8, a.ToByte(1));
				Assert.AreEqual(7, a.ToByte(2));
				Assert.AreEqual(20, a.ToByte(3));
			}
			if (type == BinaryArrayTestType.UTF8)
			{
				a.Clear();
				a.Append("abacaba");
				Array.Resize(ref resultArray, 100);
				a.ToUTF8(resultArray, 2);
				String s = "abacaba";
				resultArray1 = Encoding.UTF8.GetBytes(s);
				for (int i = 0; i < 6; ++i) Assert.AreEqual(resultArray[i * 2], resultArray1[i + 1]);
			}
		}


		
		[TestCase(IListTestType.Contains)]
		[TestCase(IListTestType.CopyTo)]
		[TestCase(IListTestType.IndexOf)]
		[TestCase(IListTestType.Insert)]
		[TestCase(IListTestType.Remove)]
		[TestCase(IListTestType.RemoveAt)]
		[TestCase(IListTestType.Equals)]
		[TestCase(IListTestType.Clone)]
		[TestCase(IListTestType.CompareTo)]

		public void IListTest(IListTestType type)
		{
			BinaryArray a = new BinaryArray();
			BinaryArray b = new BinaryArray();

			if (type == IListTestType.Contains)
			{
				a.Clear();
				a.Append((byte)130);
				a.Append((byte)139);
				a.Append((byte)12);
				a.Append((byte)4);
				a.Append((byte)58);
				Assert.AreEqual(a.Contains(130), true);
				Assert.AreEqual(a.Contains(12), true);
				Assert.AreEqual(a.Contains(58), true);
				Assert.AreEqual(a.Contains(13), false);
				Assert.AreEqual(a.Contains(121), false);
				Assert.AreEqual(a.Contains(5), false);
			}
			if (type == IListTestType.IndexOf)
			{
				a.Clear();
				a.Append((byte)130);
				a.Append((byte)139);
				a.Append((byte)12);
				a.Append((byte)4);
				a.Append((byte)58);
				Assert.AreEqual(a.IndexOf(130), 0);
				Assert.AreEqual(a.IndexOf(12), 2);
				Assert.AreEqual(a.IndexOf(58), 4);
				Assert.AreEqual(a.IndexOf(13), -1);
				Assert.AreEqual(a.IndexOf(121), -1);
				Assert.AreEqual(a.IndexOf(4), 3);

			}
			if (type == IListTestType.CopyTo)
			{
				a.Clear();
				Array.Resize(ref ar, 10);
				a.Append((byte)0);
				a.Append((byte)1);
				a.Append((byte)2);
				a.Append((byte)3);
				a.Append((byte)4);
				a.CopyTo(ar, 0);
				for (int i = 0; i < 5; ++i) Assert.AreEqual(ar[i], i);
				a.CopyTo(ar, 4);
				for (int i = 4; i < 9; ++i) Assert.AreEqual(ar[i], i - 4);
				Array.Resize(ref resultArray, 10);
				unsafe
				{
					fixed (byte* ptr = resultArray)
					{
						a.CopyTo(ptr);
						for (int i = 0; i < 5; ++i) Assert.AreEqual(*(ptr + i), i);
					}
				}
			}
			if (type == IListTestType.Insert)
			{
				a.Clear();
				a.Append((byte)0);
				a.Append((byte)1);
				a.Append((byte)2);
				a.Append((byte)3);
				a.Append((byte)4);
				a.Insert(1, (byte)10);
				Assert.AreEqual(a[0], 0);
				Assert.AreEqual(a[1], 10);
				Assert.AreEqual(a[2], 1);
				Assert.AreEqual(a[3], 2);
				Assert.AreEqual(a[4], 3);
				Assert.AreEqual(a[5], 4);
				a.Clear();
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Insert(0, (byte)10);
				for (int i = 1; i < a.Count; ++i) Assert.AreEqual(a[i], 255);
				Assert.AreEqual(a[0], 10);
				Assert.AreEqual(a.Count, 33);
				a.Clear();
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Insert(7, (byte)10);
				for (int i = 0; i < a.Count; ++i) if (i != 7) Assert.AreEqual(a[i], 255);
				Assert.AreEqual(a[7], 10);
				Assert.AreEqual(a.Count, 33);
				a.Clear();
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Insert(32, (byte)10);
				a.Clear();
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Insert(16, (byte)10);
				for (int i = 0; i < a.Count; ++i) if (i != 16) Assert.AreEqual(a[i], 255);
				Assert.AreEqual(a[16], 10);
				Assert.AreEqual(a.Count, 33);
				a.Clear();
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Append((ulong)ulong.MaxValue);
				a.Insert(18, (byte)10);
				for (int i = 0; i < a.Count; ++i) if (i != 18) Assert.AreEqual(a[i], 255);
				Assert.AreEqual(a[18], 10);
				Assert.AreEqual(a.Count, 33);
			}
			if (type == IListTestType.Remove)
			{
				a.Clear();
				a.Append((byte)0);
				a.Append((byte)1);
				a.Append((byte)2);
				a.Append((byte)3);
				a.Append((byte)4);
				a.Remove(2);
				Assert.AreEqual(a[0], 0);
				Assert.AreEqual(a[1], 1);
				Assert.AreEqual(a[2], 3);
				Assert.AreEqual(a[3], 4);
			}
			if (type == IListTestType.RemoveAt)
			{
				a.Clear();
				a.Append((byte)0);
				a.Append((byte)1);
				a.Append((byte)2);
				a.Append((byte)3);
				a.Append((byte)4);
				a.RemoveAt(3);
				Assert.AreEqual(a[0], 0);
				Assert.AreEqual(a[1], 1);
				Assert.AreEqual(a[2], 2);
				Assert.AreEqual(a[3], 4);
			}
			if (type == IListTestType.Equals)
			{
				a.Assign("Hello Test");
				Assert.AreEqual(a.Equals(a), true);
				Assert.AreEqual(BinaryArray.Equals(a, a), true);
				b.Assign("Hello Test");
				Assert.AreEqual(a.Equals(b), true);
				b.Assign("sdsd");
				Assert.AreEqual(a.Equals(b), false);
				Assert.AreEqual(a.Equals(null), false);
				Assert.AreEqual(BinaryArray.Equals(null, null), true);
				Assert.AreEqual(BinaryArray.Equals(a, null), false);
				Assert.AreEqual(BinaryArray.Equals(null, a), false);
				Assert.AreEqual(BinaryArray.Equals(new BinaryArray(), new BinaryArray()), true);

				a.Assign("Hello Test");
				MutableString ms = new MutableString();
				ms.Assign("Hello Test");
				//Assert.AreEqual(true, ((IReadOnlyString)a).Equals(ms));
				Assert.AreEqual(true, ms.Equals(a.ToString()));

			}
			if (type == IListTestType.Clone)
			{
				a.Assign(1234);
				BinaryArray temp = a.Clone();
				Assert.AreEqual(a.Equals(temp), true);
				Assert.AreEqual(a.Capacity, temp.Capacity);
				Assert.AreEqual(a.GetHashCode(), temp.GetHashCode());

			}
			if (type == IListTestType.CompareTo)
			{
				a.Assign(1324);
				b.Assign(10);
				Assert.AreEqual(a.CompareTo(b) > 0, true);

				Assert.AreEqual(b.CompareTo(a) < 0, true);
				b.Assign(1324);
				Assert.AreEqual(a.CompareTo(b), 0);
				Assert.AreEqual(b.CompareTo(a), 0);

			}
		}
		[TestCase(BinaryArrayTestType.Byte)]
		[TestCase(BinaryArrayTestType.BinaryArray)]
		[TestCase(BinaryArrayTestType.ArrayOfByte)]
		[TestCase(BinaryArrayTestType.ByteEnumerable)]
		[TestCase(BinaryArrayTestType.BytePtr)]
		[TestCase(BinaryArrayTestType.ByteArray)]
		[TestCase(BinaryArrayTestType.ByteArrayWithOffset)]
		[TestCase(BinaryArrayTestType.SByteEnumerable)]
		[TestCase(BinaryArrayTestType.SBytePtr)]
		[TestCase(BinaryArrayTestType.SByteArray)]
		[TestCase(BinaryArrayTestType.SByteArrayWithOffset)]
		[TestCase(BinaryArrayTestType.String)]
		[TestCase(BinaryArrayTestType.MutableString)]
		public void TestConstructors(BinaryArrayTestType type)
		{
			Array.Resize(ref resultArray, 10);

			if (type == BinaryArrayTestType.BinaryArray)
			{
				BinaryArray b = new BinaryArray();
				b.Assign('q');
				a = new BinaryArray(b);
				Assert.AreEqual(a.ToChar(), 'q');
				Assert.AreEqual(a, b);

				/*a = new BinaryArray((IReadOnlyString) new BinaryArray("Qwe"));
				b = new BinaryArray("Qwe");
				Assert.AreEqual(true, b.Equals(a));
				*/

			}
			if (type == BinaryArrayTestType.ArrayOfByte)
			{
				byte[] ar = { 1, 2, 3, 4, 148 };
				byte[] ar1 = { };
				Array.Resize(ref ar1, 6);
				unsafe
				{

					fixed (byte* ptr = ar)
					{
						a = new BinaryArray(ptr, 5);
						a.ToByteArray(0, ar1, 0, 5);
						Assert.AreEqual(ar1[0], 1);
						Assert.AreEqual(ar1[1], 2);
						Assert.AreEqual(ar1[2], 3);
						Assert.AreEqual(ar1[3], 4);
						Assert.AreEqual(ar1[4], 148);


					}
				}
				BinaryArray _buffer = new BinaryArray(0);
				Byte[] byteArray = new Byte[_buffer.Count];
				_buffer.ToByteArray(0, byteArray, 0, _buffer.Count);
			}

			if (type == BinaryArrayTestType.ByteArray)
			{
				a = new BinaryArray(ar1);
				a.ToByteArray(0, resultArray, 0, a.Count);
				for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, resultArray[i - 1]);
			}
			if (type == BinaryArrayTestType.ByteArrayWithOffset)
			{
				a = new BinaryArray(ar1, 2, 5);
				a.ToByteArray(0, resultArray, 0, a.Count);
				for (int i = 0; i < 5; ++i) Assert.AreEqual(i + 3, resultArray[i]);
			}
			if (type == BinaryArrayTestType.BytePtr)
			{
				unsafe
				{
					fixed (byte* ptr = ar1)
					{
						a = new BinaryArray(ptr, 10);
						a.ToByteArray(0, resultArray, 0, a.Count);
						for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, resultArray[i - 1]);
					}
				}
			}
			if (type == BinaryArrayTestType.ByteEnumerable)
			{
				List<Byte> list = new List<byte>();
				list.Add(4);
				list.Add(7);
				list.Add(9);
				a = new BinaryArray(list);
				Assert.AreEqual(a[0], 4);
				Assert.AreEqual(a[1], 7);
				Assert.AreEqual(a[2], 9);
			}

			if (type == BinaryArrayTestType.SByteArray)
			{
				a = new BinaryArray(sAr1);

				for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, a.ToSByte((i - 1) * sizeof(sbyte)));
			}
			if (type == BinaryArrayTestType.SByteArrayWithOffset)
			{
				a = new BinaryArray(sAr1, 2, 5);

				for (int i = 0; i < 5; ++i) Assert.AreEqual(i + 3, a.ToSByte(i * sizeof(sbyte)));
			}
			if (type == BinaryArrayTestType.SBytePtr)
			{
				unsafe
				{
					fixed (sbyte* ptr = sAr1)
					{
						a = new BinaryArray(ptr, 10);
						for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, a.ToSByte((i - 1) * sizeof(sbyte)));
					}
				}
			}
			if (type == BinaryArrayTestType.SByteEnumerable)
			{
				List<SByte> list = new List<sbyte>();
				list.Add(4);
				list.Add(7);
				list.Add(9);
				a = new BinaryArray(list);
				Assert.AreEqual(a.ToSByte(0), 4);
				Assert.AreEqual(a.ToSByte(sizeof(sbyte)), 7);
				Assert.AreEqual(a.ToSByte(sizeof(sbyte) * 2), 9);
			}
			if (type == BinaryArrayTestType.Byte)
			{
				a = new BinaryArray((byte)58);
				Assert.AreEqual(a.ToByte(), 58);
			}
			if (type == BinaryArrayTestType.MutableString)
			{
				MutableString str = new MutableString("sder");
				MutableString str1 = new MutableString();
				BinaryArray a = new BinaryArray(str);
				a.ToMutableString(str1);
				Assert.AreEqual(str1, str);
			}
			if (type == BinaryArrayTestType.String)
			{
				BinaryArray a = new BinaryArray(""); 
				Assert.AreEqual(a.ToString(), "");
				Assert.AreEqual("", ((Object)a).ToString());
				//Assert.AreEqual("", ((IReadOnlyString)a).ToString());
			}
		}
		[TestCase(BinaryArrayTestType.DateTime)]
		[TestCase(BinaryArrayTestType.Boolean)]
		[TestCase(BinaryArrayTestType.Byte)]
		[TestCase(BinaryArrayTestType.BytePtr)]
		[TestCase(BinaryArrayTestType.Char)]
		[TestCase(BinaryArrayTestType.Decimal)]
		[TestCase(BinaryArrayTestType.Double)]
		[TestCase(BinaryArrayTestType.Single)]
		[TestCase(BinaryArrayTestType.Int32)]
		[TestCase(BinaryArrayTestType.Int64)]
		[TestCase(BinaryArrayTestType.SByte)]
		[TestCase(BinaryArrayTestType.Int16)]
		[TestCase(BinaryArrayTestType.UInt32)]
		[TestCase(BinaryArrayTestType.UInt64)]
		[TestCase(BinaryArrayTestType.UInt16)]

		public void TestConstructorsWithCapacity(BinaryArrayTestType type)
		{
			if (type == BinaryArrayTestType.DateTime)
			{
				a = new BinaryArray(80, DateTime.MaxValue);
				Assert.AreEqual(a.ToDateTime(), DateTime.MaxValue);
				Assert.AreEqual(a.Capacity, 80);
			}
			if (type == BinaryArrayTestType.Boolean)
			{
				a = new BinaryArray(80, true);
				Assert.AreEqual(a.ToBoolean(), true);
				Assert.AreEqual(a.Capacity, 80);
			}
			if (type == BinaryArrayTestType.Byte)
			{
				a = new BinaryArray(80, (byte)45);
				Assert.AreEqual(a.ToByte(), 45);
				Assert.AreEqual(a.Capacity, 80);
			}
			if (type == BinaryArrayTestType.BytePtr)
			{
				unsafe
				{
					fixed (byte* ptr = ar1)
					{
						a = new BinaryArray(800, ptr, 10);
						a.ToByteArray(0, resultArray, 0, a.Count);
						for (int i = 1; i <= 10; ++i) Assert.AreEqual(i, resultArray[i - 1]);
						Assert.AreEqual(a.Capacity, 800);
					}
				}
			}
			if (type == BinaryArrayTestType.Char)
			{
				a = new BinaryArray(80, 'b');
				Assert.AreEqual(a.ToChar(), 'b');
				Assert.AreEqual(a.Capacity, 80);
			}
			if (type == BinaryArrayTestType.Decimal)
			{
				a = new BinaryArray(800, Decimal.MinusOne);
				Assert.AreEqual(a.ToDecimal(), Decimal.MinusOne);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.Double)
			{
				a = new BinaryArray(800, 3.14);
				Assert.AreEqual(a.ToDouble(), 3.14);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.Single)
			{
				a = new BinaryArray(800, (float)3);
				Assert.AreEqual(a.ToSingle(), 3);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.Int32)
			{
				a = new BinaryArray(800, 10);
				Assert.AreEqual(a.ToInt32(), 10);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.Int64)
			{
				a = new BinaryArray(800, (long)505555);
				Assert.AreEqual(a.ToInt64(), 505555);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.SByte)
			{
				a = new BinaryArray(800, (sbyte)50);
				Assert.AreEqual(a.ToSByte(), 50);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.Int16)
			{
				a = new BinaryArray(800, (Int16)67);
				Assert.AreEqual(a.ToInt16(), 67);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.UInt16)
			{
				a = new BinaryArray(800, (UInt16)67);
				Assert.AreEqual(a.ToUInt16(), 67);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.UInt32)
			{
				a = new BinaryArray(800, (UInt32)10);
				Assert.AreEqual(a.ToUInt32(), 10);
				Assert.AreEqual(a.Capacity, 800);
			}
			if (type == BinaryArrayTestType.UInt64)
			{
				a = new BinaryArray(800, (UInt64)505555);
				Assert.AreEqual(a.ToUInt64(), 505555);
				Assert.AreEqual(a.Capacity, 800);
			}
		}

		[TestCase]
		public void BinaryArrayInsertByteTest()
		{
			BinaryArray ba = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			BinaryArray[] array = new BinaryArray[17];
			array[0x00] = new BinaryArray() { 0xF0, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x01] = new BinaryArray() { 0x00, 0xF0, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x02] = new BinaryArray() { 0x00, 0x11, 0xF0, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x03] = new BinaryArray() { 0x00, 0x11, 0x22, 0xF0, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x04] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0xF0, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x05] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0xF0, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x06] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0xF0, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x07] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0xF0, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x08] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0xF0, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x09] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xF0, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x0A] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xF0, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x0B] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xF0, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x0C] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xF0, 0xCC, 0xDD, 0xEE, 0xFF };
			array[0x0D] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xF0, 0xDD, 0xEE, 0xFF };
			array[0x0E] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xF0, 0xEE, 0xFF };
			array[0x0F] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xF0, 0xFF };
			array[0x10] = new BinaryArray() { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0xF0 };

			for (int i = 0; i < array.Length; ++i)
			{
				BinaryArray temp = ba.Clone();
				temp.Insert(i, (byte)0xF0);
				Assert.AreEqual(temp, array[i]);
			}
		}
	}

	public class BinaryArrayTimeTest
	{
		BinaryArray a = new BinaryArray();
		MutableString s = new MutableString();
		MutableString[] s1;
		Byte[] utf8;
		long[] hash;
		Random rand = new Random(478);
		public void TestConversionFromBinaryArrayToMutableString(int numberOfIterations, int sizeOfString)
		{
			Array.Resize(ref s1, numberOfIterations);
			Array.Resize(ref hash, numberOfIterations);
			Array.Resize(ref utf8, sizeOfString * 4);
			for (int i = 0; i < numberOfIterations; ++i)
			{
				string temp = "";
				for (int j = 0; j < sizeOfString; ++j)
				{
					temp = temp + (char)('A' + rand.Next(26));
				}
				s1[i] = new MutableString(temp);

			}
			Stopwatch timer = Stopwatch.StartNew();

			for (int i = 0; i < numberOfIterations; ++i)
			{
				a.Assign(s1[i]);
				a.ToMutableString(s);
				//a.ToUTF8(utf8, 2);
				hash[i] = s.GetHashCode();
			}
			timer.Stop();
			System.Console.WriteLine(timer.Elapsed);
			long sum = 0;
			for (int i = 0; i < numberOfIterations; ++i) sum = sum * i + hash[i];
			System.Console.WriteLine(sum);
		}
	}

	public class BinaryArraySerializationTest
	{
		long[] ar = { 1, 2, 3, 4, 5 };
		[TestCase()]
		public void SimpleSerializationBinaryArrayTest()
		{
			BinaryArray a = new BinaryArray();
			BinaryArray b = new BinaryArray();
			MemoryStream stream = new MemoryStream();
			for (int i = 0; i < 5; ++i) a.Append(ar[i]);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, a);
			stream.Position = 0;
			b = (BinaryArray)formatter.Deserialize(stream);
			Assert.AreEqual(a, b);
			Assert.AreEqual(b.Length, 5);
			for (int i = 0; i < 5; ++i) Assert.AreEqual(b[i], a[i]);
		}
	}

	public class BinaryArrayInsertTest
	{
		long[] ar = { 1, 2, 3, 4, 5 };
		byte[] ar1 = { 11, 12, 13, 14, 15 };

		Guid GetGuidByByte(byte i)
		{
			return new Guid(i, i, i, i, i, i, i, i, i, i, i);		
		}
		[TestCase(BinaryArrayTestType.Int64)]
		[TestCase(BinaryArrayTestType.Int32)]
		[TestCase(BinaryArrayTestType.Int16)]
		[TestCase(BinaryArrayTestType.UInt64)]
		[TestCase(BinaryArrayTestType.UInt32)]
		[TestCase(BinaryArrayTestType.UInt16)]
		[TestCase(BinaryArrayTestType.Guid)]
		[TestCase(BinaryArrayTestType.Boolean)]
		[TestCase(BinaryArrayTestType.ArrayOfByte)]
		[TestCase(BinaryArrayTestType.ByteArrayWithOffset)]
		[TestCase(BinaryArrayTestType.BinaryArray)]
		[TestCase(BinaryArrayTestType.Double)]
		[TestCase(BinaryArrayTestType.Single)]
		[TestCase(BinaryArrayTestType.Decimal)]
		public void TestBinaryArrayInsert(BinaryArrayTestType type) 
		{
			BinaryArray a = new BinaryArray();
			if (type == BinaryArrayTestType.Int64) {
				for (int i = 0; i < 5; ++i) a.Append(ar[i]);
				a.Insert(8, (long)10);
				Assert.AreEqual(a.ToInt64(0), 1);
				Assert.AreEqual(a.ToInt64(8), 10);
				Assert.AreEqual(a.ToInt64(16), 2);
				Assert.AreEqual(a.ToInt64(24), 3);
				Assert.AreEqual(a.ToInt64(32), 4);
				Assert.AreEqual(a.ToInt64(40), 5);
			}
			if (type == BinaryArrayTestType.Int32)
			{
				for (int i = 0; i < 5; ++i) a.Append((int)ar[i]);
				a.Insert(4, (int)10);
				Assert.AreEqual(a.ToInt32(0), 1);
				Assert.AreEqual(a.ToInt32(4), 10);
				Assert.AreEqual(a.ToInt32(8), 2);
				Assert.AreEqual(a.ToInt32(12), 3);
				Assert.AreEqual(a.ToInt32(16), 4);
				Assert.AreEqual(a.ToInt32(20), 5);
			}
			if (type == BinaryArrayTestType.Int16)
			{
				for (int i = 0; i < 5; ++i) a.Append((Int16)ar[i]);
				a.Insert(2, (Int16)10);
				Assert.AreEqual(a.ToInt16(0), 1);
				Assert.AreEqual(a.ToInt16(2), 10);
				Assert.AreEqual(a.ToInt16(4), 2);
				Assert.AreEqual(a.ToInt16(6), 3);
				Assert.AreEqual(a.ToInt16(8), 4);
				Assert.AreEqual(a.ToInt16(10), 5);
			}

			if (type == BinaryArrayTestType.UInt64)
			{
				for (int i = 0; i < 5; ++i) a.Append((ulong)ar[i]);
				a.Insert(8, (ulong)10);
				Assert.AreEqual(a.ToUInt64(0), 1);
				Assert.AreEqual(a.ToUInt64(8), 10);
				Assert.AreEqual(a.ToUInt64(16), 2);
				Assert.AreEqual(a.ToUInt64(24), 3);
				Assert.AreEqual(a.ToUInt64(32), 4);
				Assert.AreEqual(a.ToUInt64(40), 5);
			}
			if (type == BinaryArrayTestType.UInt32)
			{
				for (int i = 0; i < 5; ++i) a.Append((uint)ar[i]);
				a.Insert(4, (uint)10);
				Assert.AreEqual(a.ToUInt32(0), 1);
				Assert.AreEqual(a.ToUInt32(4), 10);
				Assert.AreEqual(a.ToUInt32(8), 2);
				Assert.AreEqual(a.ToUInt32(12), 3);
				Assert.AreEqual(a.ToUInt32(16), 4);
				Assert.AreEqual(a.ToUInt32(20), 5);
			}
			if (type == BinaryArrayTestType.UInt16)
			{
				for (int i = 0; i < 5; ++i) a.Append((UInt16)ar[i]);
				a.Insert(2, (UInt16)10);
				Assert.AreEqual(a.ToUInt16(0), 1);
				Assert.AreEqual(a.ToUInt16(2), 10);
				Assert.AreEqual(a.ToUInt16(4), 2);
				Assert.AreEqual(a.ToUInt16(6), 3);
				Assert.AreEqual(a.ToUInt16(8), 4);
				Assert.AreEqual(a.ToUInt16(10), 5);
			}
			if (type == BinaryArrayTestType.Guid)
			{
				for (int i = 1; i <= 5; ++i) a.Append(GetGuidByByte((byte)i));
				a.Insert(16, GetGuidByByte(10));
				Assert.AreEqual(a.ToGuid(0), GetGuidByByte(1));
				Assert.AreEqual(a.ToGuid(16), GetGuidByByte(10));
				Assert.AreEqual(a.ToGuid(32), GetGuidByByte(2));
				Assert.AreEqual(a.ToGuid(48), GetGuidByByte(3));
				Assert.AreEqual(a.ToGuid(64), GetGuidByByte(4));
				Assert.AreEqual(a.ToGuid(80), GetGuidByByte(5));
			}
			if (type == BinaryArrayTestType.Boolean)
			{
				for (int i = 1; i <= 5; ++i) a.Append(true);
				a.Insert(1, false);
				Assert.AreEqual(a.ToBoolean(0), true);
				Assert.AreEqual(a.ToBoolean(1), false);
				Assert.AreEqual(a.ToBoolean(2), true);
				Assert.AreEqual(a.ToBoolean(3), true);
				Assert.AreEqual(a.ToBoolean(4), true);
				Assert.AreEqual(a.ToBoolean(5), true);
			}
			if (type == BinaryArrayTestType.ArrayOfByte)
			{
				for (int i = 0; i <= 10; ++i) a.Append((byte)i);
				a.Insert(11, ar1);
				for (int i = 0; i <= 15; ++i) Assert.AreEqual(a[i], i);
			}
			if (type == BinaryArrayTestType.ByteArrayWithOffset)
			{
				for (int i = 0; i <= 10; ++i) a.Append((byte)i);
				a.Insert(5, ar1, 2, 2);
				for (int i = 0; i < 5; ++i) Assert.AreEqual(a[i], i);
				Assert.AreEqual(a[5], 13);
				Assert.AreEqual(a[6], 14);
				for (int i = 7; i <= 12; ++i) Assert.AreEqual(a[i], i - 2);
			}
			if (type == BinaryArrayTestType.BinaryArray)
			{
				BinaryArray b = new BinaryArray();
				for (int i = 0; i <= 10; ++i) a.Append((byte)i);
				for (int i = 0; i <= 5; ++i) b.Append((byte)i);
				a.Insert(5, b as IBinaryConvertibleReadWrite);
				for (int i = 0; i < 5; ++i) Assert.AreEqual(a[i], i);
				for (int i = 5; i <= 10; ++i) Assert.AreEqual(a[i], i - 5);
				for (int i = 11; i <= 16; ++i) Assert.AreEqual(a[i], i - 6);
			}
			if (type == BinaryArrayTestType.Double)
			{
				a.Append(3.14);
				a.Append(3.14);
				a.Append(3.14);
				a.Insert(8, 6.28);
				Assert.AreEqual(a.ToDouble(0), 3.14);
				Assert.AreEqual(a.ToDouble(8), 6.28);
				Assert.AreEqual(a.ToDouble(16), 3.14);
				Assert.AreEqual(a.ToDouble(24), 3.14);
			}
			if (type == BinaryArrayTestType.Single)
			{
				a.Append((float)3.14);
				a.Append((float)3.14);
				a.Append((float)3.14);
				a.Insert(4, (float)6.28);
				Assert.AreEqual(a.ToSingle(0), (float)3.14);
				Assert.AreEqual(a.ToSingle(4), (float)6.28);
				Assert.AreEqual(a.ToSingle(8), (float)3.14);
				Assert.AreEqual(a.ToSingle(12), (float)3.14);
			}
			if (type == BinaryArrayTestType.Decimal)
			{
				a.Append((decimal)3.14);
				a.Append((decimal)3.14);
				a.Append((decimal)3.14);
				a.Insert(16, (decimal)6.28);
				Assert.AreEqual(a.ToDecimal(0), (decimal)3.14);
				Assert.AreEqual(a.ToDecimal(16), (decimal)6.28);
				Assert.AreEqual(a.ToDecimal(32), (decimal)3.14);
				Assert.AreEqual(a.ToDecimal(48), (decimal)3.14);
			}
		}
		[TestCase()]
		public void HashCodeTest()
		{
			BinaryArray ar = new BinaryArray();
			ar.Clear();
			ar.Append("aba");
			ar.Append("caba");
			ar.Append("abacaba");
			BinaryArray ar1 = new BinaryArray();
			ar1.Clear();
			ar1.Append("abacaba");
			ar1.Append("abacaba");
			BinaryArray ar2 = new BinaryArray();
			ar2.Assign("abacabaabacaba");
			Assert.AreEqual(ar1.GetHashCode(), ar2.GetHashCode());
			Assert.AreEqual(ar1.GetHashCode(), ar.GetHashCode());
		}



	/*	[TestCase()]
		public void UTF8ReadOnlyStringTest()
		{
			byte[] ar = new byte[100];
			ar[12] = 0;
			BinaryArray a = new BinaryArray();
			IReadOnlyString str = (BinaryArray)a;
			str.ToUTF8(ar);
			str.ToUTF8(ar, 2);
			str.ToUTF8(7, 2, ar, 7);
			Assert.AreEqual(0, str.UTF8ByteLength);
			a.Append("ABCDEFG01234");
			Assert.AreEqual(12, str.UTF8ByteLength);
			str.ToUTF8(ar);
			for (int i = 0; i < 7; ++i) Assert.AreEqual(65 + i, ar[i]);
			for (int i = 7; i < 12; ++i) Assert.AreEqual(48 + i - 7, ar[i]);
			Assert.AreEqual(0, ar[12]);
			for (int i = 0; i < 100; ++i) ar[i] = 0;
			unsafe
			{
				fixed (byte* ptr = ar)
				{
					str.ToUTF8(ptr);
				}
			}
			for (int i = 0; i < 7; ++i) Assert.AreEqual(65 + i, ar[i]);
			for (int i = 7; i < 12; ++i) Assert.AreEqual(48 + i - 7, ar[i]);
			Assert.AreEqual(0, ar[12]);
			for (int i = 0; i < 100; ++i) ar[i] = 0;

			str.ToUTF8(ar, 2);
			Assert.AreEqual(0, ar[0]);
			Assert.AreEqual(0, ar[1]);
			for (int i = 2; i < 9; ++i) Assert.AreEqual(65 + i - 2, ar[i]);
			for (int i = 9; i < 13; ++i) Assert.AreEqual(48 + i - 9, ar[i]);
			for (int i = 14; i < 100; ++i) Assert.AreEqual(0, ar[i]);
			for (int i = 0; i < 100; ++i) ar[i] = 0;

			str.ToUTF8(ar, 10);
			for (int i = 0; i < 7; ++i) Assert.AreEqual(65 + i, ar[i + 10]);
			for (int i = 7; i < 12; ++i) Assert.AreEqual(48 + i - 7, ar[i + 10]);
			for (int i = 0; i < 10; ++i) Assert.AreEqual(0, ar[i]);
			for (int i = 22; i < 100; ++i) Assert.AreEqual(0, ar[i]);
			for (int i = 0; i < 100; ++i) ar[i] = 0;

			str.ToUTF8(7, 2, ar, 7);
			for (int i = 0; i < 7; ++i) Assert.AreEqual(0, ar[i]);
			for (int i = 7; i < 9; ++i) Assert.AreEqual(48 + i - 7, ar[i]);
			for (int i = 9; i < 100; ++i) Assert.AreEqual(0, ar[i]);

		}*/
	}
}
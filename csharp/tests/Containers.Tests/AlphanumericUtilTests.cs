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
using NUnit.Framework;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture]
	public class AlphanumericUtilTest
	{
		public struct TestPair
		{
			public readonly Int64 IntValue;
			public readonly String StringValue;

			public TestPair(UInt64 i, String s)
			{
				IntValue = (Int64)i;
				StringValue = s;
			}

			public TestPair(Int64 i, String s)
			{
				IntValue = i;
				StringValue = s;
			}
		}

		// For these pairs Int->String == String, but String->Int != Int
		private static readonly TestPair[] CorrectI2S =
		{
			 new TestPair(0x123456789ABCDEF	, ""),
			 new TestPair(0x1123456789ABCDEF, "$"),
			 new TestPair(0xB000000000000000, null),
			 new TestPair(0xB123456789ABCDEF, null),
			 new TestPair(0x80000000000000F1, "        "),

			 new TestPair(0x1FFFFFFFFFFFFFFF, "_"),
			 new TestPair(0x2FFFFFFFFFFFFFFF, "__"),
			 new TestPair(0x3FFFFFFFFFFFFFFF, "___"),
			 new TestPair(0x4FFFFFFFFFFFFFFF, "____"),
			 new TestPair(0x5FFFFFFFFFFFFFFF, "_____"),
			 new TestPair(0x6FFFFFFFFFFFFFFF, "______"),
			 new TestPair(0x7FFFFFFFFFFFFFFF, "_______"),
			 new TestPair(0x8FFFFFFFFFFFFFFF, "________"),
			 new TestPair(0x9FFFFFFFFFFFFFFF, "_________"),

			 new TestPair(Int64.MaxValue	, "_______"),
		};

		// For these pairs String->Int == Int, but Int->String != String
		private static readonly TestPair[] CorrectS2I =
		{
			 new TestPair(0x8000000000000000, "        "),
			 new TestPair(Int64.MinValue	, "        "),
		};

		// For these pairs String->Int == Int and Int->String == String
		private static readonly TestPair[] Correct =
		{
			 new TestPair(Int64.MinValue	, null),
			 new TestPair(0x8000000000000000, null),
			 new TestPair(0, ""),
			 new TestPair(0x1100000000000000, "$"),

			 new TestPair(0x1000000000000000, " "),
			 new TestPair(0x2000000000000000, "  "),
			 new TestPair(0x3000000000000000, "   "),
			 new TestPair(0x4000000000000000, "    "),
			 new TestPair(0x5000000000000000, "     "),
			 new TestPair(0x6000000000000000, "      "),
			 new TestPair(0x7000000000000000, "       "),
			 // no 0x8000000000000000
			 new TestPair(0x9000000000000000, "         "),
			 new TestPair(0xA000000000000000, "          "),

			 new TestPair(0x1FC0000000000000, "_"),
			 new TestPair(0x2FFF000000000000, "__"),
			 new TestPair(0x3FFFFC0000000000, "___"),
			 new TestPair(0x4FFFFFF000000000, "____"),
			 new TestPair(0x5FFFFFFFC0000000, "_____"),
			 new TestPair(0x6FFFFFFFFF000000, "______"),
			 new TestPair(0x7FFFFFFFFFFC0000, "_______"),
			 new TestPair(0x8FFFFFFFFFFFF000, "________"),
			 new TestPair(0x9FFFFFFFFFFFFFC0, "_________"),
			 new TestPair(0xAFFFFFFFFFFFFFFF, "__________"),

			 new TestPair(0x1440000000000000, "1"),
			 new TestPair(0x2493000000000000, "23"),
			 new TestPair(0x3515580000000000, "456"),
			 new TestPair(0x45D8661000000000, "789A"),
			 new TestPair(0x58A3925980000000, "BCDEF"),
			 new TestPair(0x6A29AABB2D000000, "HIJKLM"),
			 new TestPair(0x7BAFC31CB3D00000, "NOPQRST"),
			 new TestPair(0x8D76DF8E7AEFC000, "UVWXYZ[\\"),
			 new TestPair(0x9F7EFC00420C4140, "]^_ !\"#$%"),
			 new TestPair(0xA18720928B30D38F, "&'()*+,-./"),

			 new TestPair(0x5FA0FE0F80000000, "^@_@^"),
			 new TestPair(0x669B71D79F000000, ":;<=>?"),
		};

		private static readonly UInt64[] IncorrectInt =
		{
			 UInt64.MaxValue,
			 0xFFFFFFFFFFFFFFFF,
			 0xF000000000000000,
			 0xE000000000000000,
			 0xD000000000000000,
			 0xC000000000000000,
			 0xF123456789ABCDEF,
			 0xE123456789ABCDEF,
			 0xD123456789ABCDEF,
			 0xC123456789ABCDEF,
		};

		private static readonly String[] IncorrectString =
		{
			 "Абракадабра",
			 "\0123",
			 "\0178",
			 "\x1F",
			 "\x7F",
			 "\xFF",
			 "\x60",
			 "\n",
			 "\r",
			 "`",
			 "a",
			 "all your base are belong to us",
			 "04259837l2"
		};

		[Test, TestCaseSource(nameof(CorrectI2S)), TestCaseSource(nameof(Correct))]
		public void Int64ToString(TestPair p)
		{
			Assert.AreEqual(p.StringValue, AlphanumericUtils.ToString(p.IntValue));
			Assert.AreEqual(p.StringValue, AlphanumericUtils.ToString((UInt64)p.IntValue));
		}

		[Test, TestCaseSource(nameof(CorrectI2S)), TestCaseSource(nameof(Correct))]
		public void Int64AssignToMString(TestPair p)
		{
			MutableString ms1 = new MutableString(), ms2 = new MutableString("ewrfvewrtfcr\tmewfxlqwoexhfsm3egzfsqewir\r \nqfaGFEWFW");
			
			AlphanumericUtils.AssignAlphanumeric(ms1, p.IntValue);
			AlphanumericUtils.AssignAlphanumeric(ms2, p.IntValue);

			if (p.StringValue == null)
			{
				Assert.AreEqual("", ms1.ToString());
				Assert.AreEqual("", ms2.ToString());
			}
			else
			{
				Assert.AreEqual(p.StringValue, ms1.ToString());
				Assert.AreEqual(p.StringValue, ms2.ToString());
			}

			Assert.AreEqual(ms1, ms2);
		}

		[Test, TestCaseSource(nameof(Correct))]
		public void Int64ToStringAndBack(TestPair p)
		{
			Assert.AreEqual(p.IntValue, AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToString(p.IntValue)));
			Assert.AreEqual(p.IntValue, AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToString((UInt64)p.IntValue)));
			Assert.AreEqual(p.IntValue, (Int64)AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToString(p.IntValue)));
			Assert.AreEqual(p.IntValue, (Int64)AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToString((UInt64)p.IntValue)));

			Assert.AreEqual((UInt64)p.IntValue, AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToString(p.IntValue)));
			Assert.AreEqual((UInt64)p.IntValue, AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToString((UInt64)p.IntValue)));
			Assert.AreEqual((UInt64)p.IntValue, (UInt64)AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToString(p.IntValue)));
			Assert.AreEqual((UInt64)p.IntValue, (UInt64)AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToString((UInt64)p.IntValue)));
		}

		[Test, TestCaseSource(nameof(Correct))]
		public void Int64ToMutableStringAndBack(TestPair p)
		{
			Assert.AreEqual(p.IntValue, AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToMutableString(p.IntValue)));
			Assert.AreEqual(p.IntValue, AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToMutableString((UInt64)p.IntValue)));
			Assert.AreEqual(p.IntValue, (Int64)AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToMutableString(p.IntValue)));
			Assert.AreEqual(p.IntValue, (Int64)AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToMutableString((UInt64)p.IntValue)));

			Assert.AreEqual((UInt64)p.IntValue, AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToMutableString(p.IntValue)));
			Assert.AreEqual((UInt64)p.IntValue, AlphanumericUtils.ToAlphanumericUInt64(AlphanumericUtils.ToMutableString((UInt64)p.IntValue)));
			Assert.AreEqual((UInt64)p.IntValue, (UInt64)AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToMutableString(p.IntValue)));
			Assert.AreEqual((UInt64)p.IntValue, (UInt64)AlphanumericUtils.ToAlphanumericInt64(AlphanumericUtils.ToMutableString((UInt64)p.IntValue)));
		}

		[Test, TestCaseSource(nameof(Correct))]
		public void StringToInt64AndBack(TestPair p)
		{
			Assert.AreEqual(p.StringValue, AlphanumericUtils.ToString(AlphanumericUtils.ToAlphanumericInt64(p.StringValue)));
			Assert.AreEqual(p.StringValue, AlphanumericUtils.ToString(AlphanumericUtils.ToAlphanumericUInt64(p.StringValue)));
		}

		[Test, TestCaseSource(nameof(Correct))]
		public void MutableStringToInt64AndBack(TestPair p)
		{
			MutableString ms = null != p.StringValue ? new MutableString(p.StringValue) : null;
			Assert.AreEqual(ms, AlphanumericUtils.ToMutableString(AlphanumericUtils.ToAlphanumericInt64(ms)));
			Assert.AreEqual(ms, AlphanumericUtils.ToMutableString(AlphanumericUtils.ToAlphanumericUInt64(ms)));
		}

		//[TestCase(TestedStringClass.MutableString)]
		//[TestCase(TestedStringClass.String)]

		[Test, TestCaseSource(nameof(Correct)), TestCaseSource(nameof(CorrectI2S))]
		public void IsValidInt64(TestPair p)
		{
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumeric(p.IntValue));
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumeric((UInt64)p.IntValue));
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumeric((IReadOnlyString)(new MutableString().Assign(p.IntValue))));

		}

		[Test, TestCaseSource(nameof(Correct)), TestCaseSource(nameof(CorrectI2S))]
		public void IsValidStringToInt64(TestPair p)
		{
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumericUInt64(p.StringValue));
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumericUInt64(p.StringValue));
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumericUInt64((IReadOnlyString)(new MutableString().Assign(p.StringValue))));

		}

		[Test, TestCaseSource(nameof(Correct)), TestCaseSource(nameof(CorrectS2I))]
		public void IsValidString(TestPair p)
		{
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumeric(p.StringValue));
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumeric((IReadOnlyString)(new MutableString().Assign(p.StringValue))));

		}

		[Test, TestCaseSource(nameof(Correct)), TestCaseSource(nameof(CorrectS2I))]
		public void IsValidMutableString(TestPair p)
		{
			MutableString ms = null != p.StringValue ? new MutableString(p.StringValue) : null;
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumeric(ms));
			Assert.IsTrue(AlphanumericUtils.IsValidAlphanumeric((IReadOnlyString)(new MutableString().Assign(ms))));

		}

		[Test, TestCaseSource(nameof(IncorrectInt))]
		public void IsInvalidInt64(UInt64 x)
		{
			Assert.IsFalse(AlphanumericUtils.IsValidAlphanumeric((Int64)x));
		}

		[Test, TestCaseSource(nameof(IncorrectString))]
		public void IsInvalidString(String s)
		{
			Assert.IsFalse(AlphanumericUtils.IsValidAlphanumeric(s));
			Assert.IsFalse(AlphanumericUtils.IsValidAlphanumeric(new MutableString(s)));
			Assert.IsFalse(AlphanumericUtils.IsValidAlphanumeric((IReadOnlyString)(new MutableString(s))));

		}

		[Test, TestCaseSource(nameof(IncorrectInt))]
		public void Int64ToStringFails(UInt64 x)
		{
			Assert.That(() => AlphanumericUtils.ToString((Int64)x), Throws.TypeOf<ArgumentException>());
		}

		[Test, TestCaseSource(nameof(IncorrectInt))]
		public void Int64ToMutableStringFails(UInt64 x)
		{
			Assert.That(() => AlphanumericUtils.ToMutableString((Int64)x), Throws.TypeOf<ArgumentException>());
		}

		[Test, TestCaseSource(nameof(IncorrectString))]
		public void StringToIntFails(String x)
		{
			Assert.That(() => AlphanumericUtils.ToAlphanumericInt64(x), Throws.TypeOf<ArgumentException>());
		}

		[Test, TestCaseSource(nameof(IncorrectString))]
		public void MutableStringToIntFails(String x)
		{
			Assert.AreEqual(x, new MutableString(x).ToString());
			Assert.That(() => AlphanumericUtils.ToAlphanumericInt64(x), Throws.TypeOf<ArgumentException>());
		}

		[Test, TestCaseSource(nameof(IncorrectInt))]
		public void AssignAlphanumericFails(UInt64 x)
		{
			MutableString ms = new MutableString();
			Assert.That(() => AlphanumericUtils.AssignAlphanumeric(ms, x), Throws.TypeOf<ArgumentException>());
		}

#if false
		[Test]
		[TestCaseSource("CorrectI2S")]
		[TestCaseSource("Correct")]
		public void PrintStr(TestPair p)
		{
			Console.WriteLine("0x{0:X} -> '{1}'", p.intValue, AlphanumericUtils.ToString(p.intValue));
		}

		[Test]
		[TestCaseSource("Correct")]
		[TestCaseSource("CorrectS2I")]
		public void PrintHex(TestPair p)
		{
			Console.WriteLine("'{0}' -> 0x{1:X}", p.stringValue, AlphanumericUtils.ToAlphanumericInt64(p.stringValue));
		}
#endif
		}
}
using EPAM.Deltix.Time;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	public class BinaryStringTest
	{
		BinaryAsciiString s = new BinaryAsciiString();
		BinaryAsciiString s1 = new BinaryAsciiString();
		Char[] ar = { 's', 's', 'w', 't', 'u', 'j', '1', 'r' };
		Char[] ar1;
		Byte[] utf8 = { 65, 65, 65, 65, 66, 97 };
		Byte[] utf8test;
		Byte[] utf16 = { };



		[TestCase()]
		public void TestBigCount()
		{
			BinaryAsciiString s = new BinaryAsciiString();
			for (int i = 0; i < 100; ++i) s.AppendFastHex(10000);
			s.Clear();
		}


		[Test]
		public void TestUpperAndLowerCaseMethod()
		{
			BinaryAsciiString s1 = new BinaryAsciiString("aza23523aza");
			BinaryAsciiString s2 = new BinaryAsciiString("AzA23523Aza");
			BinaryAsciiString s3 = new BinaryAsciiString("AZA23523AZA");
			BinaryAsciiString s4 = new BinaryAsciiString();
			s1.CopyTo(s4);
			s4.Append("a");
			Assert.IsTrue(s1.ToUpperCase().Equals(s2.ToUpperCase()));
			Assert.IsTrue(s3.Equals(s2));
			s1.Append("a");
			s3.Append("A");
			Assert.IsFalse(s1.Equals(s3));
			Assert.IsTrue(s1.ToLowerCase().Equals(s3.ToLowerCase().ToUpperCase().ToLowerCase()));
			Assert.IsTrue(s1.Equals(s4));
		}

		UUID uuid = new UUID("0123456789ABCDEF1011121314151617");
		[TestCase(MutableStringAppendTestType.Char)]
		[TestCase(MutableStringAppendTestType.Integer)]
		[TestCase(MutableStringAppendTestType.String)]
		[TestCase(MutableStringAppendTestType.MutableString)]
		[TestCase(MutableStringAppendTestType.ArrayOfChar)]
		[TestCase(MutableStringAppendTestType.ArrayOfCharWithOffset)]
		[TestCase(MutableStringAppendTestType.CharPtr)]
		[TestCase(MutableStringAppendTestType.StringBuilder)]
		[TestCase(MutableStringAppendTestType.Short)]
		[TestCase(MutableStringAppendTestType.Long)]
		[TestCase(MutableStringAppendTestType.UTF8)]
		[TestCase(MutableStringAppendTestType.UTF16)]
		[TestCase(MutableStringAppendTestType.UUID)]
		[TestCase(MutableStringAppendTestType.DateTime)]
		[TestCase(MutableStringAppendTestType.HdDateTime)]
		[TestCase(MutableStringAppendTestType.TimeSpan)]
		[TestCase(MutableStringAppendTestType.HdTimeSpan)]
		[TestCase(MutableStringAppendTestType.Double)]
		[TestCase(MutableStringAppendTestType.Float)]
		[TestCase(MutableStringAppendTestType.Boolean)]
		[TestCase(MutableStringAppendTestType.Decimal)]
		[TestCase(MutableStringAppendTestType.Object)]

		public void TestAppend(MutableStringAppendTestType type)
		{
			if (type == MutableStringAppendTestType.Char)
			{
				s.Assign("sasd");
				s.Append('e');
				Assert.AreEqual(s.ToString(), "sasde");
				s.Assign('e');
				Assert.AreEqual(s.ToString(), "e");
				s.Append('t');
				Assert.AreEqual(s.ToString(), "et");
			}
			if (type == MutableStringAppendTestType.String)
			{
				s.Assign("qwert");
				s.Append("yuiop");
				Assert.AreEqual(s.ToString(), "qwertyuiop");
				s.Assign("qwerty");
				Assert.AreEqual(s.ToString(), "qwerty");
			}
			if (type == MutableStringAppendTestType.Integer)
			{
				s.Assign("");
				s.Append(-123);
				Assert.AreEqual(s.ToString(), "-123");
				s.Assign("");
				s.Append(124);
				Assert.AreEqual(s.ToString(), "124");
				s.Assign(-123);
				Assert.AreEqual(s.ToString(), "-123");
				s.Assign(124);
				Assert.AreEqual(s.ToString(), "124");

			}
			if (type == MutableStringAppendTestType.MutableString)
			{
				s.Assign("aba");
				s1.Assign("caba");
				s.Append(s1);
				Assert.AreEqual(s.ToString(), "abacaba");
				s1.Append(s);
				Assert.AreEqual(s1.ToString(), "cabaabacaba");

				s.Assign(s1);
				Assert.AreEqual(s.ToString(), "cabaabacaba");
			}
			if (type == MutableStringAppendTestType.ArrayOfChar)
			{
				s.Clear();
				s.Append(ar);
				Assert.AreEqual(s.ToString(), "sswtuj1r");

				s.Assign(ar);
				Assert.AreEqual(s.ToString(), "sswtuj1r");
			}
			if (type == MutableStringAppendTestType.ArrayOfCharWithOffset)
			{
				s.Clear();
				s.Append(ar, 2, 4);
				Assert.AreEqual(s.ToString(), "wtuj");
				s.Append(ar, 0, 1);
				Assert.AreEqual(s.ToString(), "wtujs");

				s.Assign(ar, 2, 4);
				Assert.AreEqual(s.ToString(), "wtuj");

			}
			if (type == MutableStringAppendTestType.CharPtr)
			{
				unsafe
				{
					fixed (char* ptr = ar)
					{
						s.Clear();
						s.Append(ptr, 5);
						Assert.AreEqual(s.ToString(), "sswtu");

						s.Assign(ptr, 5);
						Assert.AreEqual(s.ToString(), "sswtu");
					}
				}

				s.Clear();
				s.Append(ar, 0, 5);
				Assert.AreEqual(s.ToString(), "sswtu");
				s.Assign("abacaba");

				s.Assign(ar, 0, 5);
				Assert.AreEqual(s.ToString(), "sswtu");

				s.Assign(ar, 1, 3);
				Assert.AreEqual(s.ToString(), "swt");

			}
			if (type == MutableStringAppendTestType.StringBuilder)
			{
				StringBuilder builder = new StringBuilder();
				builder.Append("qazxcvb");
				s.Clear();
				s.Append(builder);
				Assert.AreEqual(s.ToString(), "qazxcvb");

				s.Assign(builder);
				Assert.AreEqual(s.ToString(), "qazxcvb");
			}
			if (type == MutableStringAppendTestType.Long)
			{
				s.Assign("");
				s.Append((long)-123);
				Assert.AreEqual(s.ToString(), "-123");
				s.Assign("");
				s.Append((long)124);
				Assert.AreEqual(s.ToString(), "124");
				s.Append((long)124);
				Assert.AreEqual(s.ToString(), "124124");



				s.Assign((long)-123);
				s.Append((long)-123);
				Assert.AreEqual(s.ToString(), "-123-123");
				s.Assign((long)124);
				Assert.AreEqual(s.ToString(), "124");
			}
			if (type == MutableStringAppendTestType.Short)
			{
				s.Assign("");
				s.Append((short)-123);
				Assert.AreEqual(s.ToString(), "-123");
				s.Assign("");
				s.Append((short)124);
				Assert.AreEqual(s.ToString(), "124");

				s.Assign((short)-123);
				Assert.AreEqual(s.ToString(), "-123");
				s.Assign((short)124);
				Assert.AreEqual(s.ToString(), "124");
			}
			if (type == MutableStringAppendTestType.UTF8)
			{
				s.Clear();
				s.AppendUTF8(utf8);
				Assert.AreEqual(s.ToString(), "AAAABa");
				s.Clear();
				s.AppendUTF8(utf8, 4, 2);
				Assert.AreEqual(s.ToString(), "Ba");
				s.Clear();
				unsafe
				{
					fixed (byte* ptr = utf8)
					{
						s.AppendUTF8(ptr, 6);
						Assert.AreEqual(s.ToString(), "AAAABa");
					}
				}
				s.AssignUTF8(utf8);
				Assert.AreEqual(s.ToString(), "AAAABa");
				s.AssignUTF8(utf8, 4, 2);
				Assert.AreEqual(s.ToString(), "Ba");
				unsafe
				{
					fixed (byte* ptr = utf8)
					{
						s.AssignUTF8(ptr, 6);
						Assert.AreEqual(s.ToString(), "AAAABa");
					}
				}
			}
			if (type == MutableStringAppendTestType.UTF16)
			{
				String tempString = "abacaba";
				utf16 = UnicodeEncoding.Unicode.GetBytes(tempString);
				s.Clear();
				s.AppendUTF16(utf16);
				Assert.AreEqual(s.ToString(), "abacaba");
				s.Clear();
				unsafe
				{
					fixed (byte* ptr = utf16)
					{
						s.AppendUTF16(ptr, 4);
						Assert.AreEqual(s.ToString(), "ab");
					}
				}
				s.Clear();
				s.AppendUTF16(utf16, 0, 4);
				Assert.AreEqual(s.ToString(), "ab");

				s.AssignUTF16(utf16);
				Assert.AreEqual(s.ToString(), "abacaba");
				unsafe
				{
					fixed (byte* ptr = utf16)
					{
						s.AssignUTF16(ptr, 4);
						Assert.AreEqual(s.ToString(), "ab");
					}
				}
				s.AssignUTF16(utf16, 0, 4);
				Assert.AreEqual(s.ToString(), "ab");
			}
			if (type == MutableStringAppendTestType.UUID)
			{
				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.LowerCase);
				s.Append(uuid, UUIDPrintFormat.LowerCase);
				Assert.AreEqual("01234567-89ab-cdef-1011-12131415161701234567-89ab-cdef-1011-121314151617", s.ToString());

				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.UpperCase);
				s.Append(uuid, UUIDPrintFormat.UpperCase);
				Assert.AreEqual("01234567-89AB-CDEF-1011-12131415161701234567-89AB-CDEF-1011-121314151617", s.ToString());

				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.LowerCaseWithoutDashes);
				s.Append(uuid, UUIDPrintFormat.LowerCaseWithoutDashes);
				Assert.AreEqual("0123456789abcdef10111213141516170123456789abcdef1011121314151617", s.ToString());

				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.UpperCaseWithoutDashes);
				s.Append(uuid, UUIDPrintFormat.UpperCaseWithoutDashes);
				Assert.AreEqual("0123456789ABCDEF10111213141516170123456789ABCDEF1011121314151617", s.ToString());
			}

			if (type == MutableStringAppendTestType.DateTime)
			{
				s.Clear();
				DateTime dateTime = new DateTime(2016, 1, 1, 9, 7, 55, 555);
				s.Append(dateTime);
				Assert.AreEqual(s.ToString(), "01/01/2016 09:07:55.555");
				dateTime = new DateTime(2016, 10, 11, 19, 17, 55, 555);
				s.Assign(dateTime);
				Assert.AreEqual(s.ToString(), "10/11/2016 19:17:55.555");
			}
			if (type == MutableStringAppendTestType.HdDateTime)
			{
				s.Clear();
				HdDateTime dateTime = new HdDateTime(new DateTime(2016, 1, 1, 9, 7, 55, 555), 10);
				s.Append(dateTime);
				Assert.AreEqual(s.ToString(), "01/01/2016 09:07:55.555.10");
				dateTime = new HdDateTime(new DateTime(2016, 10, 11, 19, 17, 55, 555), 9);
				s.Assign(dateTime);
				Assert.AreEqual(s.ToString(), "10/11/2016 19:17:55.555.09");
			}
			if (type == MutableStringAppendTestType.TimeSpan)
			{
				s.Clear();
				TimeSpan timeSpan = new TimeSpan(10, 9, 7, 55, 555);
				s.Append(timeSpan);
				Assert.AreEqual(s.ToString(), "10.09:07:55.555");
				timeSpan = new TimeSpan(9, 19, 17, 55, 55);
				s.Assign(timeSpan);
				Assert.AreEqual(s.ToString(), "9.19:17:55.055");
			}
			if (type == MutableStringAppendTestType.HdTimeSpan)
			{
				s.Clear();
				HdTimeSpan timeSpan = new HdTimeSpan(new TimeSpan(10, 9, 7, 55, 555));
				s.Append(timeSpan);
				Assert.AreEqual(s.ToString(), "10.09:07:55.5550000");
				timeSpan = new HdTimeSpan(new TimeSpan(9, 19, 17, 55, 55));
				s.Assign(timeSpan);
				Assert.AreEqual(s.ToString(), "9.19:17:55.0550000");
			}
			if (type == MutableStringAppendTestType.Double)
			{
				s.Clear();
				s.Append((double)3.14);
				Assert.AreEqual((s.ToString() == "3.14" || s.ToString() == "3,14"), true);
				s.Assign((double)3.1459);
				Assert.AreEqual((s.ToString() == "3.1459" || s.ToString() == "3,1459"), true);
			}
			if (type == MutableStringAppendTestType.Float)
			{
				s.Clear();
				s.Append((float)3.14);
				Assert.AreEqual((s.ToString() == "3.14" || s.ToString() == "3,14"), true);
				s.Assign((float)3.1459);
				Assert.AreEqual((s.ToString() == "3.1459" || s.ToString() == "3,1459"), true);

			}
			if (type == MutableStringAppendTestType.Boolean)
			{
				s.Clear();
				s.Append(true);
				Assert.AreEqual(s.ToString(), "True");
				s.Assign(false);
				Assert.AreEqual(s.ToString(), "False");
			}
			if (type == MutableStringAppendTestType.Decimal)
			{
				s.Clear();
				s.Append((decimal)3.14);
				Assert.AreEqual((s.ToString() == "3.14" || s.ToString() == "3,14"), true);
				s.Assign((decimal)3.1459);
				Assert.AreEqual((s.ToString() == "3.1459" || s.ToString() == "3,1459"), true);
			}
			if (type == MutableStringAppendTestType.Object)
			{
				s.Clear();
				Tuple<int, int> tuple = new Tuple<int, int>(1, 1);
				s.Append(tuple);
				Assert.AreEqual(tuple.ToString(), s.ToString());
				s.Assign(tuple);
				Assert.AreEqual(tuple.ToString(), s.ToString());

			}
		}



		public enum TypeOfSubstring
		{
			BeginEnd,
			MiddleEnd,
			MiddleMiddle,
			Empty
		}

		[TestCase(TypeOfSubstring.BeginEnd)]
		[TestCase(TypeOfSubstring.Empty)]
		[TestCase(TypeOfSubstring.MiddleEnd)]
		[TestCase(TypeOfSubstring.MiddleMiddle)]
		public void TestSubstring(TypeOfSubstring type)
		{
			MutableString s = new MutableString();
			s.Assign("qwertyuiop");
			if (type == TypeOfSubstring.BeginEnd)
			{
				Assert.AreEqual(s.SubString(0, s.Length - 1).ToString(), "qwertyuiop");
			}
			if (type == TypeOfSubstring.Empty)
			{
				Assert.AreEqual(s.SubString(1, 0).ToString(), "");
			}
			if (type == TypeOfSubstring.MiddleEnd)
			{
				Assert.AreEqual(s.SubString(5, s.Length - 1).ToString(), "yuiop");
			}
			if (type == TypeOfSubstring.MiddleMiddle)
			{
				Assert.AreEqual(s.SubString(2, 5).ToString(), "erty");
			}
		}






		[TestCase(MutableStringAppendTestType.MutableString)]
		[TestCase(MutableStringAppendTestType.StringBuilder)]
		[TestCase(MutableStringAppendTestType.Char)]
		[TestCase(MutableStringAppendTestType.CharPtr)]
		[TestCase(MutableStringAppendTestType.ArrayOfChar)]
		[TestCase(MutableStringAppendTestType.ArrayOfCharWithOffset)]
		[TestCase(MutableStringAppendTestType.Integer)]
		[TestCase(MutableStringAppendTestType.Long)]
		[TestCase(MutableStringAppendTestType.Short)]
		[TestCase(MutableStringAppendTestType.String)]
		public void BinaryStringTestConstructors(MutableStringAppendTestType type)
		{
			if (type == MutableStringAppendTestType.MutableString)
			{
				BinaryAsciiString s = new BinaryAsciiString("ab");
				BinaryAsciiString s1 = new BinaryAsciiString(s);
				Assert.AreEqual(s.ToString(), s1.ToString());
				Assert.AreEqual(s1.ToString(), "ab");
				BinaryAsciiString s2 = new BinaryAsciiString(100, s);
				Assert.AreEqual(s2.ToString(), s.ToString());
			}
			if (type == MutableStringAppendTestType.String)
			{
				BinaryAsciiString s = new BinaryAsciiString("as");
				Assert.AreEqual(s.ToString(), "as");
				s = new BinaryAsciiString(105, "as");
				Assert.AreEqual(s.ToString(), "as");
			}
			if (type == MutableStringAppendTestType.StringBuilder)
			{
				StringBuilder builder = new StringBuilder("abab");
				BinaryAsciiString s = new BinaryAsciiString(builder);
				Assert.AreEqual(s.ToString(), "abab");
				BinaryAsciiString s1 = new BinaryAsciiString(100, builder);
				Assert.AreEqual(s1.ToString(), "abab");
			}
			if (type == MutableStringAppendTestType.Char)
			{
				BinaryAsciiString s = new BinaryAsciiString('a');
				Assert.AreEqual(s.ToString(), "a");
				BinaryAsciiString s1 = new BinaryAsciiString(256, 'a');
				Assert.AreEqual(s1.ToString(), "a");
			}
			if (type == MutableStringAppendTestType.CharPtr)
			{
				unsafe
				{
					fixed (char* ptr = ar)
					{
						BinaryAsciiString s = new BinaryAsciiString(ptr, 8);
						Assert.AreEqual(s.ToString(), "sswtuj1r");
						BinaryAsciiString s1 = new BinaryAsciiString(546, ptr, 8);
						Assert.AreEqual(s1.ToString(), "sswtuj1r");
					}
				}
			}
			if (type == MutableStringAppendTestType.ArrayOfChar)
			{
				BinaryAsciiString s = new BinaryAsciiString(ar);
				Assert.AreEqual(s.ToString(), "sswtuj1r");
				BinaryAsciiString s1 = new BinaryAsciiString(100, ar);
				Assert.AreEqual(s1.ToString(), "sswtuj1r");
			}
			if (type == MutableStringAppendTestType.ArrayOfCharWithOffset)
			{
				BinaryAsciiString s = new BinaryAsciiString(ar, 1, 2);
				Assert.AreEqual(s.ToString(), "sw");
				BinaryAsciiString s1 = new BinaryAsciiString(105, ar, 1, 2);
				Assert.AreEqual(s1.ToString(), "sw");
			}
			if (type == MutableStringAppendTestType.Integer)
			{
				s = new BinaryAsciiString(100, -100);
				Assert.AreEqual(s.ToString(), "-100");
				s = new BinaryAsciiString(100, 100);
				Assert.AreEqual(s.ToString(), "100");

			}
			if (type == MutableStringAppendTestType.Short)
			{

				s = new BinaryAsciiString(100, (short)-100);
				Assert.AreEqual(s.ToString(), "-100");
				s = new BinaryAsciiString(100, (short)100);
				Assert.AreEqual(s.ToString(), "100");

			}
			if (type == MutableStringAppendTestType.Long)
			{
				s = new BinaryAsciiString(100, (long)-100);
				Assert.AreEqual(s.ToString(), "-100");
				s = new BinaryAsciiString(100, (long)100);
				Assert.AreEqual(s.ToString(), "100");

			}
		}

		[TestCase(MutableStringAppendTestType.Long)]
		[TestCase(MutableStringAppendTestType.UTF8)]
		[TestCase(MutableStringAppendTestType.String)]
		public void ToTypeTest(MutableStringAppendTestType type)
		{
			if (type == MutableStringAppendTestType.Long)
			{
				BinaryAsciiString s = new BinaryAsciiString();
				s.Assign((long)100000000000000000);
				Assert.AreEqual(s.ToInt64(), (long)100000000000000000);
			}
			if (type == MutableStringAppendTestType.UTF8)
			{
				Array.Resize(ref utf8test, 100);
				s.Assign("AAAaaa");

				s.ToUTF8(utf8test);
				for (int i = 0; i < 3; ++i) Assert.AreEqual(utf8test[i], 65);
				for (int i = 3; i < 6; ++i) Assert.AreEqual(utf8test[i], 97);

				utf8test[1] = 45;
				utf8test[2] = 45;
				s.ToUTF8(0, 2, utf8test, 0);
				for (int i = 0; i < 2; ++i) Assert.AreEqual(utf8test[i], 65);
				Assert.AreEqual(utf8test[2], 45);
				utf8test[1] = 45;
				utf8test[2] = 45;
				s.ToUTF8(utf8test, 2);
				for (int i = 2; i < 5; ++i) Assert.AreEqual(utf8test[i], 65);
				for (int i = 5; i < 8; ++i) Assert.AreEqual(utf8test[i], 97);


			}
			if (type == MutableStringAppendTestType.String)
			{
				BinaryAsciiString s = new BinaryAsciiString();
				s.Assign("abacaba");
				Assert.AreEqual(s.ToString(), "abacaba");
				Assert.AreEqual(s.Length, 7);
			}
		}



		[TestCase()]
		public void HashCodeTest()
		{
			BinaryAsciiString ar = new BinaryAsciiString();
			ar.Clear();
			ar.Append("aba");
			ar.Append("caba");
			ar.Append("abacaba");
			BinaryAsciiString ar1 = new BinaryAsciiString();
			ar1.Clear();
			ar1.Append("abacaba");
			ar1.Append("abacaba");
			BinaryAsciiString ar2 = new BinaryAsciiString();
			ar2.Assign("abacabaabacaba");
			Assert.AreEqual(ar1.GetHashCode(), ar2.GetHashCode());
			Assert.AreEqual(ar1.GetHashCode(), ar.GetHashCode());
		}

		[TestCase]
		public void EmptyTest()
		{
			Assert.IsTrue(new BinaryAsciiString().Empty);
			Assert.IsTrue(new BinaryAsciiString("").Empty);
			Assert.IsTrue(new BinaryAsciiString("123").Clear().Empty);

			Assert.IsFalse(new BinaryAsciiString("1").Empty);
			Assert.IsFalse(new BinaryAsciiString().Append("asd").Empty);
			Assert.IsFalse(new BinaryAsciiString().Assign("qwe").Empty);
		}


		[TestCase(-4994165663973249513L, ExpectedResult = "BAB128C9F16BEE17")]
		[TestCase(8520181221404153971L, ExpectedResult = "763DC3ADD3BF9873")]
		[TestCase(8726572470186071490L, ExpectedResult = "791B036D2024D5C2")]
		[TestCase(7632483291392660792L, ExpectedResult = "69EC071D69905538")]
		[TestCase(-6796755928382580220, ExpectedResult = "A1AD127E6FB9DA04")]
		[TestCase(2182163843979789332L, ExpectedResult = "1E489A745A1DC414")]
		[TestCase(6954165158794076907L, ExpectedResult = "6082285DFF51A2EB")]
		[TestCase(2403514907034969788L, ExpectedResult = "215B0012D62CE6BC")]
		[TestCase(3162104787147719725L, ExpectedResult = "2BE20D8CE480842D")]
		[TestCase(-2544258361427576470L, ExpectedResult = "DCB0FA80164AB96A")]
		public String TestAppendHexLong(long parameter)
		{
			return new BinaryAsciiString().AppendFastHex(parameter).ToString();
		}
	}

}

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NUnit.Framework;
using EPAM.Deltix.Time;

namespace EPAM.Deltix.Containers.Tests{
	public enum MutableStringOperatorsType
	{
		Equal,
		NotEqual
	}

	public enum MutableStringAppendTestType
	{
		Integer,
		Long,
		Short,
		Char,
		String,
		MutableString,
		ArrayOfChar,
		ArrayOfCharWithOffset,
		CharPtr,
		StringBuilder,
		UTF8,
		UTF16,
		UUID,
		DateTime,
		HdDateTime,
		TimeSpan,
		HdTimeSpan,
		Double,
		Float,
		Boolean,
		Decimal,
		Object
	}
	[TestFixture]
	class MutableStringUnitTest
	{
		MutableString s = new MutableString();
		MutableString s1 = new MutableString();
		Char[] ar = { 's', 's', 'w', 't', 'u', 'j', '1', 'r' };
		Char[] ar1;
		Byte[] utf8 = { 65, 65, 65, 65, 66, 97 };
		Byte[] utf8test;
		Byte[] utf16 = { };
		UUID uuid = new UUID("0123456789ABCDEF1011121314151617");

		[Test()]
		public void TestSecureClear()
		{
			MutableString s1 = new MutableString();
			s1.Assign("asdasdasdasdasdasd");
			s1.SecureClear();
			Assert.AreEqual(0, s1.Length);
			for (int i = 0; i < s1._data.Length; ++i) Assert.AreEqual(0, s1._data[i]);
		}


		[Test]
		public void TestUpperAndLowerCaseMethod()
		{
			MutableString s1 = new MutableString("aza23523aza");
			MutableString s2 = new MutableString("AzA23523Aza");
			MutableString s3 = new MutableString("AZA23523AZA");
			MutableString s4 = new MutableString();
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
				Assert.AreEqual(s, "sasde");
				s.Assign('e');
				Assert.AreEqual(s, "e");
				s.Add('t');
				Assert.AreEqual(s, "et");
			}
			if (type == MutableStringAppendTestType.String)
			{
				s.Assign("qwert");
				s.Append("yuiop");
				Assert.AreEqual(s, "qwertyuiop");
				s.Assign("qwerty");
				Assert.AreEqual(s, "qwerty");
			}
			if (type == MutableStringAppendTestType.Integer)
			{
				s.Assign("");
				s.Append(-123);
				Assert.AreEqual(s, "-123");
				s.Assign("");
				s.Append(124);
				Assert.AreEqual(s, "124");
				s.Assign(-123);
				Assert.AreEqual(s, "-123");
				s.Assign(124);
				Assert.AreEqual(s, "124");

			}
			if (type == MutableStringAppendTestType.MutableString)
			{
				s.Assign("aba");
				s1.Assign("caba");
				s.Append(s1);
				Assert.AreEqual(s, "abacaba");
				s1.Append(s);
				Assert.AreEqual(s1, "cabaabacaba");

				s.Assign(s1);
				Assert.AreEqual(s, "cabaabacaba");
			}
			if (type == MutableStringAppendTestType.ArrayOfChar)
			{
				s.Clear();
				s.Append(ar);
				Assert.AreEqual(s, "sswtuj1r");

				s.Assign(ar);
				Assert.AreEqual(s, "sswtuj1r");
			}
			if (type == MutableStringAppendTestType.ArrayOfCharWithOffset)
			{
				s.Clear();
				s.Append(ar, 2, 4);
				Assert.AreEqual(s, "wtuj");
				s.Append(ar, 0, 1);
				Assert.AreEqual(s, "wtujs");

				s.Assign(ar, 2, 4);
				Assert.AreEqual(s, "wtuj");

			}
			if (type == MutableStringAppendTestType.CharPtr)
			{
				unsafe
				{
					fixed (char* ptr = ar)
					{
						s.Clear();
						s.Append(ptr, 5);
						Assert.AreEqual(s, "sswtu");

						s.Assign(ptr, 5);
						Assert.AreEqual(s, "sswtu");
					}
				}
			}
			if (type == MutableStringAppendTestType.StringBuilder)
			{
				StringBuilder builder = new StringBuilder();
				builder.Append("qazxcvb");
				s.Clear();
				s.Append(builder);
				Assert.AreEqual(s, "qazxcvb");

				s.Assign(builder);
				Assert.AreEqual(s, "qazxcvb");
			}
			if (type == MutableStringAppendTestType.Long)
			{
				s.Assign("");
				s.Append((long)-123);
				Assert.AreEqual(s, "-123");
				s.Assign("");
				s.Append((long)124);
				Assert.AreEqual(s, "124");

				s.Assign((long)-123);
				Assert.AreEqual(s, "-123");
				s.Assign((long)124);
				Assert.AreEqual(s, "124");
			}
			if (type == MutableStringAppendTestType.Short)
			{
				s.Assign("");
				s.Append((short)-123);
				Assert.AreEqual(s, "-123");
				s.Assign("");
				s.Append((short)124);
				Assert.AreEqual(s, "124");

				s.Assign((short)-123);
				Assert.AreEqual(s, "-123");
				s.Assign((short)124);
				Assert.AreEqual(s, "124");
			}
			if (type == MutableStringAppendTestType.UTF8)
			{
				s.Clear();
				s.AppendUTF8(utf8);
				Assert.AreEqual(s, "AAAABa");
				s.Clear();
				s.AppendUTF8(utf8, 4, 2);
				Assert.AreEqual(s, "Ba");
				s.Clear();
				unsafe
				{
					fixed (byte* ptr = utf8)
					{
						s.AppendUTF8(ptr, 6);
						Assert.AreEqual(s, "AAAABa");
					}
				}
				s.AssignUTF8(utf8);
				Assert.AreEqual(s, "AAAABa");
				s.AssignUTF8(utf8, 4, 2);
				Assert.AreEqual(s, "Ba");
				unsafe
				{
					fixed (byte* ptr = utf8)
					{
						s.AssignUTF8(ptr, 6);
						Assert.AreEqual(s, "AAAABa");
					}
				}
			}
			if (type == MutableStringAppendTestType.UTF16)
			{
				String tempString = "abacaba";
				utf16 = UnicodeEncoding.Unicode.GetBytes(tempString);
				s.Clear();
				s.AppendUTF16(utf16);
				Assert.AreEqual(s, "abacaba");
				s.Clear();
				unsafe
				{
					fixed (byte* ptr = utf16)
					{
						s.AppendUTF16(ptr, 4);
						Assert.AreEqual(s, "ab");
					}
				}
				s.Clear();
				s.AppendUTF16(utf16, 0, 4);
				Assert.AreEqual(s, "ab");

				s.AssignUTF16(utf16);
				Assert.AreEqual(s, "abacaba");
				unsafe
				{
					fixed (byte* ptr = utf16)
					{
						s.AssignUTF16(ptr, 4);
						Assert.AreEqual(s, "ab");
					}
				}
				s.AssignUTF16(utf16, 0, 4);
				Assert.AreEqual(s, "ab");
			}
			if (type == MutableStringAppendTestType.UUID)
			{
				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.LowerCase);
				s.Append(uuid, UUIDPrintFormat.LowerCase);
				Assert.AreEqual("01234567-89ab-cdef-1011-12131415161701234567-89ab-cdef-1011-121314151617", s);

				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.UpperCase);
				s.Append(uuid, UUIDPrintFormat.UpperCase);
				Assert.AreEqual("01234567-89AB-CDEF-1011-12131415161701234567-89AB-CDEF-1011-121314151617", s);

				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.LowerCaseWithoutDashes);
				s.Append(uuid, UUIDPrintFormat.LowerCaseWithoutDashes);
				Assert.AreEqual("0123456789abcdef10111213141516170123456789abcdef1011121314151617", s);

				s.Clear();
				s.Assign(uuid, UUIDPrintFormat.UpperCaseWithoutDashes);
				s.Append(uuid, UUIDPrintFormat.UpperCaseWithoutDashes);
				Assert.AreEqual("0123456789ABCDEF10111213141516170123456789ABCDEF1011121314151617", s);
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
				Assert.AreEqual(s.ToString(), "10.09:07:55.555.00");
				timeSpan = new HdTimeSpan(new TimeSpan(9, 19, 17, 55, 55));
				s.Assign(timeSpan);
				Assert.AreEqual(s.ToString(), "9.19:17:55.055.00");
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
			MutableString s = new MutableString();
			MutableString s1 = new MutableString();
			if (type == IListTestType.Contains)
			{
				s.Assign("qwertyui");
				Assert.AreEqual(s.Contains('r'), true);
				Assert.AreEqual(s.Contains('q'), true);
				Assert.AreEqual(s.Contains('i'), true);
				Assert.AreEqual(s.Contains('a'), false);
				Assert.AreEqual(s.Contains('s'), false);
				Assert.AreEqual(s.Contains('d'), false);
			}
			if (type == IListTestType.CopyTo)
			{
				s.Assign("sadfsf");
				s1.Assign("r");
				s1.CopyTo(s);
				Assert.AreEqual(s1, "r");
				Assert.AreEqual(s, "r");
				s.Assign("sadfsf");
				s1.Assign("r");
				s.CopyTo(s1);
				Assert.AreEqual(s1, "sadfsf");
				Assert.AreEqual(s, "sadfsf");
				s.Assign("qwer");
				Array.Resize(ref ar1, 10);
				s.CopyTo(ar1, 0);
				Assert.AreEqual(ar1[0], 'q');
				Assert.AreEqual(ar1[1], 'w');
				Assert.AreEqual(ar1[2], 'e');
				Assert.AreEqual(ar1[3], 'r');
			}
			if (type == IListTestType.IndexOf)
			{
				s.Assign("qwertyui");
				Assert.AreEqual(s.IndexOf('r'), 3);
				Assert.AreEqual(s.IndexOf('q'), 0);
				Assert.AreEqual(s.IndexOf('i'), 7);
				Assert.AreEqual(s.IndexOf('a'), -1);
				Assert.AreEqual(s.IndexOf('s'), -1);
				Assert.AreEqual(s.IndexOf('d'), -1);
			}
			if (type == IListTestType.Insert)
			{
				s.Assign("qazxcv");
				s.Insert(0, '1');
				Assert.AreEqual(s, "1qazxcv");
				s.Insert(2, '2');
				Assert.AreEqual(s, "1q2azxcv");
				s.Insert(8, '8');
				Assert.AreEqual(s, "1q2azxcv8");
			}
			if (type == IListTestType.Remove)
			{
				s.Assign("qwaedszf");
				s.Remove('e');
				Assert.AreEqual(s, "qwadszf");
				s.Remove('q');
				Assert.AreEqual(s, "wadszf");
				s.Remove('f');
				Assert.AreEqual(s, "wadsz");
			}
			if (type == IListTestType.RemoveAt)
			{
				s.Assign("qwaedszf");
				s.RemoveAt(3);
				Assert.AreEqual(s, "qwadszf");
				s.RemoveAt(0);
				Assert.AreEqual(s, "wadszf");
				s.RemoveAt(5);
				Assert.AreEqual(s, "wadsz");
			}
			if (type == IListTestType.Equals)
			{
				s.Assign("sd");
				Assert.AreEqual(s.Equals(s), true);
				Assert.AreEqual(MutableString.Equals(s, s), true);
				s1.Assign("sd");
				Assert.AreEqual(s.Equals(s1), true);
				Assert.AreEqual(true, s.Equals((IReadOnlyString)s1));
			
				s1.Assign("sdsd");
				Assert.AreEqual(s.Equals(s1), false);

				Assert.AreEqual(false, s.Equals((IReadOnlyString)s1));
				Assert.AreEqual(s1.Equals("sdsd"), true);
				Assert.AreEqual(s1.Equals("sdsd1"), false);
				Assert.AreEqual(s1.Equals("sdsq"), false);
				Assert.AreEqual(s1.Equals((MutableString)null), false);
				Assert.AreEqual(MutableString.Equals(null, null), true);
				Assert.AreEqual(MutableString.Equals(s, null), false);
				Assert.AreEqual(MutableString.Equals(null, s), false);

				Assert.AreEqual(MutableString.Equals(new MutableString(), new MutableString()), true);
			}
			if (type == IListTestType.Clone)
			{
				s.Assign("qwer");
				MutableString s2 = s.Clone();
				Assert.AreEqual(s2, "qwer");
				Assert.AreEqual(s2, s);
			}
			if (type == IListTestType.CompareTo)
			{
				s.Assign("qwer");
				s1.Assign("qwzz");
				Assert.AreEqual(s.CompareTo(s1) < 0, true);
				Assert.AreEqual(MutableString.Compare(s, s1) < 0, true);
				Assert.AreEqual(s1.CompareTo(s) > 0, true);
				Assert.AreEqual(MutableString.Compare(s1, s) > 0, true);
				s1.Assign("qwer");
				Assert.AreEqual(s.CompareTo(s1), 0);
				Assert.AreEqual(s1.CompareTo(s), 0);
				Assert.AreEqual(MutableString.Compare(s, s1), 0);
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
				Assert.AreEqual(s.SubString(0, s.Length - 1), "qwertyuiop");
			}
			if (type == TypeOfSubstring.Empty)
			{
				Assert.AreEqual(s.SubString(1, 0), "");
			}
			if (type == TypeOfSubstring.MiddleEnd)
			{
				Assert.AreEqual(s.SubString(5, s.Length - 1), "yuiop");
			}
			if (type == TypeOfSubstring.MiddleMiddle)
			{
				Assert.AreEqual(s.SubString(2, 5), "erty");
			}
		}


		[TestCase(MutableStringOperatorsType.Equal)]
		[TestCase(MutableStringOperatorsType.NotEqual)]
		public void TestOperators(MutableStringOperatorsType type)
		{
			MutableString mutableString = new MutableString();
			MutableString mutableString2 = new MutableString();
			if (type == MutableStringOperatorsType.Equal)
			{
				mutableString.Assign("qwe123");
				mutableString2.Assign("qwe123");
				Assert.AreEqual(mutableString == mutableString2, true);
				Assert.AreEqual(mutableString != mutableString2, false);
				Assert.AreEqual(mutableString == null, false);
				Assert.AreEqual(null == mutableString, false);
			}
			if (type == MutableStringOperatorsType.NotEqual)
			{
				mutableString.Assign("qwe123");
				mutableString2.Assign("qwe153");
				Assert.AreEqual(mutableString == mutableString2, false);
				Assert.AreEqual(mutableString != mutableString2, true);
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
		public void MutableStringTestConstructors(MutableStringAppendTestType type)
		{
			if (type == MutableStringAppendTestType.MutableString)
			{
				MutableString s = new MutableString("ab");
				MutableString s1 = new MutableString(s);
				Assert.AreEqual(s, s1);
				Assert.AreEqual(s1, "ab");
				MutableString s2 = new MutableString(100, s);
				Assert.AreEqual(s2, s);
				Assert.AreEqual(s2.Capacity, 100);
			}
			if (type == MutableStringAppendTestType.String)
			{
				MutableString s = new MutableString("as");
				Assert.AreEqual(s, "as");
				s = new MutableString(105, "as");
				Assert.AreEqual(s, "as");
				Assert.AreEqual(s.Capacity, 105);
			}
			if (type == MutableStringAppendTestType.StringBuilder)
			{
				StringBuilder builder = new StringBuilder("abab");
				MutableString s = new MutableString(builder);
				Assert.AreEqual(s, "abab");
				MutableString s1 = new MutableString(100, builder);
				Assert.AreEqual(s1, "abab");
				Assert.AreEqual(s1.Capacity, 100);
			}
			if (type == MutableStringAppendTestType.Char)
			{
				MutableString s = new MutableString('a');
				Assert.AreEqual(s, "a");
				MutableString s1 = new MutableString(256, 'a');
				Assert.AreEqual(s1, "a");
				Assert.AreEqual(256, s1.Capacity);
			}
			if (type == MutableStringAppendTestType.CharPtr)
			{
				unsafe
				{
					fixed (char* ptr = ar)
					{
						MutableString s = new MutableString(ptr, 8);
						Assert.AreEqual(s, "sswtuj1r");
						MutableString s1 = new MutableString(546, ptr, 8);
						Assert.AreEqual(s1, "sswtuj1r");
						Assert.AreEqual(s1.Capacity, 546);
					}
				}
			}
			if (type == MutableStringAppendTestType.ArrayOfChar)
			{
				MutableString s = new MutableString(ar);
				Assert.AreEqual(s, "sswtuj1r");
				MutableString s1 = new MutableString(100, ar);
				Assert.AreEqual(s1, "sswtuj1r");
				Assert.AreEqual(s1.Capacity, 100);
			}
			if (type == MutableStringAppendTestType.ArrayOfCharWithOffset)
			{
				MutableString s = new MutableString(ar, 1, 2);
				Assert.AreEqual(s, "sw");
				MutableString s1 = new MutableString(105, ar, 1, 2);
				Assert.AreEqual(s1, "sw");
				Assert.AreEqual(s1.Capacity, 105);
			}
			if (type == MutableStringAppendTestType.Integer)
			{
				s = new MutableString(100, -100);
				Assert.AreEqual(s, "-100");
				s = new MutableString(100, 100);
				Assert.AreEqual(s, "100");
				Assert.AreEqual(s.Capacity, 100);

			}
			if (type == MutableStringAppendTestType.Short)
			{

				s = new MutableString(100, (short)-100);
				Assert.AreEqual(s, "-100");
				s = new MutableString(100, (short)100);
				Assert.AreEqual(s, "100");
				Assert.AreEqual(s.Capacity, (short)100);

			}
			if (type == MutableStringAppendTestType.Long)
			{
				s = new MutableString(100, (long)-100);
				Assert.AreEqual(s, "-100");
				s = new MutableString(100, (long)100);
				Assert.AreEqual(s, "100");
				Assert.AreEqual(s.Capacity, (long)100);

			}
		}

		[TestCase(MutableStringAppendTestType.Long)]
		[TestCase(MutableStringAppendTestType.UTF8)]
		[TestCase(MutableStringAppendTestType.String)]
		public void ToTypeTest(MutableStringAppendTestType type)
		{
			if (type == MutableStringAppendTestType.Long)
			{
				MutableString s = new MutableString();
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
				MutableString s = new MutableString();
				s.Assign("abacaba");
				Assert.AreEqual(s.ToString(), "abacaba");
				Assert.AreEqual(s.Count, 7);
			}
		}

		[TestCase(MutableStringAppendTestType.ArrayOfChar)]
		[TestCase(MutableStringAppendTestType.ArrayOfCharWithOffset)]
		[TestCase(MutableStringAppendTestType.MutableString)]
		[TestCase(MutableStringAppendTestType.String)]
		[TestCase(MutableStringAppendTestType.Char)]
		[TestCase(MutableStringAppendTestType.CharPtr)]
		[TestCase(MutableStringAppendTestType.StringBuilder)]
		[TestCase(MutableStringAppendTestType.Long)]
		public void TestInsert(MutableStringAppendTestType type)
		{
			MutableString s = new MutableString();
			s.Append("abacaba");
			if (type == MutableStringAppendTestType.ArrayOfChar)
			{
				s.Insert(3, ar);
				Assert.AreEqual(s.ToString(), "abasswtuj1rcaba");
			}
			if (type == MutableStringAppendTestType.ArrayOfCharWithOffset)
			{
				s.Insert(3, ar, 2, 2);
				Assert.AreEqual(s.ToString(), "abawtcaba");
			}
			if (type == MutableStringAppendTestType.MutableString)
			{
				MutableString b = new MutableString(ar);
				s.Insert(3, b);
				Assert.AreEqual(s.ToString(), "abasswtuj1rcaba");
			}
			if (type == MutableStringAppendTestType.String)
			{
				s.Insert(3, "111");
				Assert.AreEqual(s.ToString(), "aba111caba");
				Assert.AreEqual(new MutableString("2016-04-27 09:30:03.5927721").Insert(27, String.Empty), "2016-04-27 09:30:03.5927721");
				Assert.AreEqual(new MutableString("2016-04-27 09:30:03.5927721").Insert(13, String.Empty), "2016-04-27 09:30:03.5927721");
				Assert.AreEqual(new MutableString("aba").Insert(3, "aba"), "abaaba");

			}
			if (type == MutableStringAppendTestType.StringBuilder)
			{
				StringBuilder builder = new StringBuilder("111");
				s.Insert(3, builder);
				Assert.AreEqual(s.ToString(), "aba111caba");
			}
			if (type == MutableStringAppendTestType.Char)
			{
				s.Insert(3, 'c');
				Assert.AreEqual(s.ToString(), "abaccaba");
			}
			if (type == MutableStringAppendTestType.CharPtr)
			{
				unsafe
				{
					fixed (char* ptr = ar)
					{
						s.Insert(3, ptr, 2);
						Assert.AreEqual(s, "abasscaba");
					}
				}
			}
			if (type == MutableStringAppendTestType.Long)
			{
				Int64 number = 12345;
				s.Insert(3, number);
				Assert.AreEqual(s, "aba12345caba");
				s = new MutableString("abacaba");
				number = -12345;
				s.Insert(3, number);
				Assert.AreEqual(s, "aba-12345caba");
				s = new MutableString("abacaba");
				number = 0;
				s.Insert(3, number);
				Assert.AreEqual(s, "aba0caba");
			}
		}

		[TestCase()]
		public void HashCodeTest()
		{
			MutableString ar = new MutableString();
			ar.Clear();
			ar.Append("aba");
			ar.Append("caba");
			ar.Append("abacaba");
			MutableString ar1 = new MutableString();
			ar1.Clear();
			ar1.Append("abacaba");
			ar1.Append("abacaba");
			MutableString ar2 = new MutableString();
			ar2.Assign("abacabaabacaba");
			Assert.AreEqual(ar1.GetHashCode(), ar2.GetHashCode());
			Assert.AreEqual(ar1.GetHashCode(), ar.GetHashCode());
		}
	}

	[TestFixture]
	public class StringComparisonTest
	{
		MutableString s = new MutableString("abacaba");
		MutableString s1 = new MutableString("AbAcAbA");
		[TestCase(StringComparison.Ordinal)]
		[TestCase(StringComparison.OrdinalIgnoreCase)]
		public void IndexOfTest(StringComparison type)
		{
			if (type == StringComparison.Ordinal)
			{
				Assert.AreEqual(s.IndexOf(s1, type), -1);
				Assert.AreEqual(s1.IndexOf(new MutableString("AcA"), type), 2);
				Assert.AreEqual(s1.IndexOf(new MutableString("aca"), type), -1);

			}
			else if (type == StringComparison.OrdinalIgnoreCase)
			{
				Assert.AreEqual(s.IndexOf(s1, type), 0);
				Assert.AreEqual(s1.IndexOf(new MutableString("aca"), type), 2);
				Assert.AreEqual(s1.IndexOf(new MutableString("AcA"), type), 2);
			}
		}

		[TestCase(StringComparison.Ordinal)]
		[TestCase(StringComparison.OrdinalIgnoreCase)]
		public void StartWithTest(StringComparison type)
		{
			if (type == StringComparison.Ordinal)
			{
				Assert.AreEqual(s1.StartsWith(new MutableString("aba"), StringComparison.Ordinal), false);
				Assert.AreEqual(s1.StartsWith(new MutableString("AbA"), StringComparison.Ordinal), true);

			}
			else if (type == StringComparison.OrdinalIgnoreCase)
			{
				Assert.AreEqual(s1.StartsWith(new MutableString("aba"), StringComparison.OrdinalIgnoreCase), true);
				Assert.AreEqual(s1.StartsWith(new MutableString("AbA"), StringComparison.OrdinalIgnoreCase), true);
			}
		}

		[TestCase(StringComparison.Ordinal)]
		[TestCase(StringComparison.OrdinalIgnoreCase)]
		public void EndWithTest(StringComparison type)
		{
			if (type == StringComparison.Ordinal)
			{
				Assert.AreEqual(s1.EndsWith(new MutableString("aba"), StringComparison.Ordinal), false);
				Assert.AreEqual(s1.EndsWith(new MutableString("AbA"), StringComparison.Ordinal), true);

			}
			else if (type == StringComparison.OrdinalIgnoreCase)
			{
				Assert.AreEqual(s1.EndsWith(new MutableString("aba"), StringComparison.OrdinalIgnoreCase), true);
				Assert.AreEqual(s1.EndsWith(new MutableString("AbA"), StringComparison.OrdinalIgnoreCase), true);
			}
		}

		[TestCase(StringComparison.Ordinal)]
		[TestCase(StringComparison.OrdinalIgnoreCase)]
		public void Equals(StringComparison type)
		{
			if (type == StringComparison.Ordinal)
			{
				Assert.AreEqual(s.Equals(s1, StringComparison.Ordinal), false);
				Assert.AreEqual(s.Equals(s, StringComparison.Ordinal), true);
			}
			else if (type == StringComparison.OrdinalIgnoreCase)
			{
				Assert.AreEqual(s.Equals(s1, StringComparison.OrdinalIgnoreCase), true);
				Assert.AreEqual(s.Equals(s, StringComparison.OrdinalIgnoreCase), true);
			}
		}

		[TestCase(StringComparison.Ordinal)]
		[TestCase(StringComparison.OrdinalIgnoreCase)]
		public void Contains(StringComparison type)
		{
			if (type == StringComparison.Ordinal)
			{
				Assert.AreEqual(s.Contains(s1, type), false);
				Assert.AreEqual(s1.Contains(new MutableString("AcA"), type), true);
				Assert.AreEqual(s1.Contains(new MutableString("aca"), type), false);

			}
			else if (type == StringComparison.OrdinalIgnoreCase)
			{
				Assert.AreEqual(s.Contains(s1, type), true);
				Assert.AreEqual(s1.Contains(new MutableString("aca"), type), true);
				Assert.AreEqual(s1.Contains(new MutableString("AcA"), type), true);
			}
		}

		[TestCase]
		public void Trim()
		{
			Assert.AreEqual("", new MutableString().Trim().ToString());
			Assert.AreEqual("", new MutableString("  \t\n  ").Trim().ToString());
			Assert.AreEqual("no \nwhitespace", new MutableString("no \nwhitespace").Trim().ToString());
			Assert.AreEqual("lots\t of\nwhitespace", new MutableString("  lots\t of\nwhitespace\r\n ").Trim().ToString());

			Assert.AreEqual("", new MutableString().TrimLeft().ToString());
			Assert.AreEqual("", new MutableString("  \t\n  ").TrimLeft().ToString());
			Assert.AreEqual("no \nwhitespace", new MutableString("no \nwhitespace").TrimLeft().ToString());
			Assert.AreEqual("lots\t of\nwhitespace\r\n ", new MutableString("  lots\t of\nwhitespace\r\n ").TrimLeft().ToString());

			Assert.AreEqual("", new MutableString().TrimRight().ToString());
			Assert.AreEqual("", new MutableString("  \t\n  ").TrimRight().ToString());
			Assert.AreEqual("no \nwhitespace", new MutableString("no \nwhitespace").TrimRight().ToString());
			Assert.AreEqual("  lots\t of\nwhitespace", new MutableString("  lots\t of\nwhitespace\r\n ").TrimRight().ToString());
		}

		[TestCase]
		public void Empty()
		{
			Assert.IsTrue(new MutableString().Empty);
			Assert.IsTrue(new MutableString("").Empty);
			Assert.IsTrue(new MutableString("123").Clear().Empty);

			Assert.IsFalse(new MutableString("1").Empty);
			Assert.IsFalse(new MutableString().Append("asd").Empty);
			Assert.IsFalse(new MutableString().Assign("qwe").Empty);
		}
	}

	public class MutableStringSerializationTest
	{
		
		[TestCase()]
		public void SimpleSerializationMutableStringTest()
		{
			MutableString a = new MutableString("abacaba");
			MutableString b = new MutableString();
			MemoryStream stream = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, a);
			stream.Position = 0;
			b = (MutableString)formatter.Deserialize(stream);
			Assert.AreEqual(a, b);
			Assert.AreEqual(b.Length, 7);
			for (int i = 0; i < 7; ++i) Assert.AreEqual(b[i], a[i]);
		}
	}

}

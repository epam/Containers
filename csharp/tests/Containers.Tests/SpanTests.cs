using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers.Tests{
	[TestFixture()]
	public class SpanTests
	{
		[TestCase()]
		public void BinaryArraySpanTest()
		{
			byte[] byteArray = { 1, 2, 3, 4, 5 };
			sbyte[] sbyteArray = { 1, 2, 3, 4, 5 };
			char[] charArray = { 'a', 'b', 'c', 'd', 'e' };
			Span<byte> testByteSpan = new Span<byte>(byteArray);
			Span<sbyte> testSByteSpan = new Span<sbyte>(sbyteArray);
			Span<char> testCharSpan = new Span<char>(charArray);
			
			BinaryArray ar = new BinaryArray();
			ar.Assign((ReadOnlySpan<byte>)testByteSpan);
			
			Assert.AreEqual(5, ar.Count);
			for (int i = 0; i < ar.Count; ++i) Assert.AreEqual(i + 1, ar[i]);
			ar.Append(testByteSpan);
			Assert.AreEqual(10, ar.Count);
			for (int i = 0; i < ar.Count; ++i) Assert.AreEqual(i % 5 + 1, ar[i]);

			ar.Insert(5, testByteSpan);
			Assert.AreEqual(15, ar.Count);
			for (int i = 0; i < ar.Count; ++i) Assert.AreEqual(i % 5 + 1, ar[i]);

			ar.Assign(testSByteSpan);
			Assert.AreEqual(5, ar.Count);
			for (int i = 0; i < ar.Count; ++i) Assert.AreEqual(i + 1, ar[i]);
			ar.Append(testSByteSpan);
			Assert.AreEqual(10, ar.Count);
			for (int i = 0; i < ar.Count; ++i) Assert.AreEqual(i % 5 + 1, ar[i]);

			ar.Insert(5, testSByteSpan);
			Assert.AreEqual(15, ar.Count);
			for (int i = 0; i < ar.Count; ++i) Assert.AreEqual(i % 5 + 1, ar[i]);

			ar.Assign(testCharSpan);
			Assert.AreEqual(10, ar.Count);
			Assert.AreEqual("abcde", ar.ToString());
			ar.Append(testCharSpan);
			Assert.AreEqual(20, ar.Count);
			Assert.AreEqual("abcdeabcde", ar.ToString());
			ar.Insert(10, testCharSpan);
			Assert.AreEqual(30, ar.Count);
			Assert.AreEqual("abcdeabcdeabcde", ar.ToString());

			Span<byte> span = new Span<byte>(new byte[30]);
			ar.ToByteArray(0, span);
			BinaryArray ar1 = new BinaryArray(span);
			Assert.AreEqual("abcdeabcdeabcde", ar1.ToString());

			Span<byte> span1 = new Span<byte>(new byte[20]);
			ar1.ToByteArray(10, span1);
			BinaryArray ar2 = new BinaryArray(span1);
			Assert.AreEqual("abcdeabcde", ar2.ToString());

		}

		[TestCase()]
		public void MutableStringSpanTest()
		{
			char[] charArray = { 'a', 'b', 'c', 'd', 'e' };
			Span<char> testCharSpan = new Span<char>(charArray);
			MutableString mutableString = new MutableString();
			mutableString.Assign(testCharSpan);
			Assert.AreEqual(5, mutableString.Count);
			Assert.AreEqual("abcde", mutableString.ToString());
			mutableString.Append(testCharSpan);
			Assert.AreEqual(10, mutableString.Count);
			Assert.AreEqual("abcdeabcde", mutableString.ToString());
			mutableString.Insert(5, testCharSpan);
			Assert.AreEqual("abcdeabcdeabcde", mutableString.ToString());
			Span<char> span = new Span<char>(new char[15]);
			mutableString.ToCharArray(span);
			MutableString ar1 = new MutableString(span);
			Assert.AreEqual("abcdeabcdeabcde", ar1.ToString());

		}

		[TestCase()]
		public void BinaryAsciiStringTest()
		{
			char[] charArray = { 'a', 'b', 'c', 'd', 'e' };
			Span<char> testCharSpan = new Span<char>(charArray);
			BinaryAsciiString mutableString = new BinaryAsciiString();
			mutableString.Assign(testCharSpan);
			Assert.AreEqual(5, mutableString.Length);
			Assert.AreEqual("abcde", mutableString.ToString());
			mutableString.Append(testCharSpan);
			Assert.AreEqual(10, mutableString.Length);
			Assert.AreEqual("abcdeabcde", mutableString.ToString());
			Span<char> span = new Span<char>(new char[10]);
			mutableString.ToCharArray(span);
			BinaryAsciiString ar1 = new BinaryAsciiString(span);
			Assert.AreEqual("abcdeabcde", ar1.ToString());
		}

	}
}

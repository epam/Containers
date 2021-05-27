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
﻿using EPAM.Deltix.Time;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Defines special version of text type, pass textual information between API and client without allocating new String instances
	/// </summary>
	[Serializable]
	public class MutableString : IList<Char>, IComparable, ICloneable, IConvertible, IComparable<MutableString>, IEnumerable<Char>, IEnumerable, IEquatable<MutableString>, ISerializable, IMutableString
	{
		private static readonly char[] HexDigitsUpper = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
		private static readonly char[] HexDigitsLower = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

		internal Char[] _data;
		internal Int32 _length;
		internal String cachedToString;
		internal Int32 _cachedHash = 0;
		private static Int32 GetMaxCharsForUTF8Bytes(Int32 byteCount) { return byteCount + 1; }
		private static Int32 GetMaxCharsForUTF16Bytes(Int32 byteCount) { return (byteCount >> 1) + 1; }

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public MutableString(Int32 capacity)
		{
			_length = 0;
			_data = new Char[Math.Max(capacity, 0x10)];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		public MutableString() : this(0x10) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString(String str) : this(str.Length) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString(StringBuilder str) : this(str.Length) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString(MutableString str) : this(str.Length) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The char value.</param>
		public MutableString(Char c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The DateTime value.</param>
		public MutableString(DateTime c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The HdDateTime value.</param>
		public MutableString(HdDateTime c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The TimeSpan value.</param>
		public MutableString(TimeSpan c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The HdTimeSpan value.</param>
		public MutableString(HdTimeSpan c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The double value.</param>
		public MutableString(double c) : this(0x10) { Assign(c); }
		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The float value.</param>
		public MutableString(float c) : this(0x10) { Assign(c); }
		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The float value.</param>
		public MutableString(bool c) : this(0x10) { Assign(c); }


		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The decimal value.</param>
		public MutableString(decimal c) : this(0x10) { Assign(c); }


		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="c">The object value.</param>
		public MutableString(object c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="str">The string in form of char array</param>
		public MutableString(Char[] str) : this(0x10) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="str">The string in form of char array.</param>
		/// <param name="offset">Offset in source char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public MutableString(Char[] str, Int32 offset, Int32 count) : this(0x10) { Assign(str, offset, count); }


		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class
		/// from specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Initial data</param>
		public MutableString(IBinaryConvertibleReadOnly binaryArray)
			: this(0x10)
		{
			Assign(binaryArray);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class
		/// </summary>
		/// <param name="span">span to init</param>
		public MutableString(Span<char> span) : this(0x10)
		{
			Assign(span);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class
		/// from specified <see cref="UUID"/>, using specified <see cref="UUIDPrintFormat"/>.
		/// </summary>
		/// <param name="uuid"></param>
		/// <param name="format"></param>
		public MutableString(UUID uuid, UUIDPrintFormat format = UUIDPrintFormat.UpperCase)
			: this(0x10)
		{
			Assign(uuid, format);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="str">The string in form of pointer to char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public unsafe MutableString(Char* str, Int32 count) : this(0x10) { Assign(str, count); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string.</param>
		public MutableString(Int32 capacity, String str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string.</param>
		public MutableString(Int32 capacity, StringBuilder str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string.</param>
		public MutableString(Int32 capacity, MutableString str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString" /> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="c">The char value.</param>
		public MutableString(Int32 capacity, Char c) : this(capacity) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="i">The integer value.</param>
		public MutableString(Int32 capacity, Int16 i) : this(capacity) { Assign(i); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="i">The integer value.</param>
		public MutableString(Int32 capacity, Int32 i) : this(capacity) { Assign(i); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="i">The integer value.</param>
		public MutableString(Int32 capacity, Int64 i) : this(capacity) { Assign(i); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string in form of char array</param>
		public MutableString(Int32 capacity, Char[] str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string in form of char array.</param>
		/// <param name="offset">Offset in source char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public MutableString(Int32 capacity, Char[] str, Int32 offset, Int32 count)
			: this(capacity)
		{
			Assign(str, offset, count);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MutableString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string in form of pointer to char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public unsafe MutableString(Int32 capacity, Char* str, Int32 count) : this(capacity) { Assign(str, count); }

		#endregion Constructors

		#region Append

		///// <summary>
		///// Appends the specified string.
		///// </summary>
		// <param name="str">The string.</param>
		//public void Append(java.lang.CharSequence str)
		//{
		//	if (str == null)
		//		return;

		//	Int32 tempLength = str.length();
		//	Expand(_length + tempLength);

		//	for (Int32 i = 0; i < _length; ++i)
		//		_data[tempLength + i] = str.charAt(i);

		//	_length = _length + tempLength;
		//}

		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString Append(StringBuilder str)
		{
			if (str == null)
				return this;
			Int32 tempLength = str.Length;
			Expand(_length + tempLength);
			str.CopyTo(0, _data, _length, tempLength);
			_length = _length + tempLength;
			ResetHashCode();

			return this;
		}

		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString IMutableString.Append(StringBuilder str)
		{
			return Append(str);
		}

		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString Append(MutableString str)
		{
			if ((object)str == null)
				return this;
			Int32 tempLength = str.Length;
			Expand(_length + tempLength);
			str.ToCharArray(0, _data, _length, tempLength);
			_length = _length + tempLength;
			ResetHashCode();

			return this;
		}


		/// <summary>
		/// Append the specified object.
		/// </summary>
		/// <param name="item">Specified object.</param>
		/// <returns>The string.</returns>
		public MutableString Append(Object item)
		{
			ResetHashCode();
			return Append(item.ToString());
		}
		/// <summary>
		/// Append the specified decimal.
		/// </summary>
		/// <param name="item">Specified decimal.</param>
		/// <returns>The string.</returns>
		public MutableString Append(decimal item)
		{
			ResetHashCode();
			return Append(item.ToString());
		}

		/// <summary>
		/// Append the specified object.
		/// </summary>
		/// <param name="item">Specified object.</param>
		/// <returns>The string.</returns>
		IMutableString IMutableString.Append(Object item)
		{
			return Append(item);
		}
		/// <summary>
		/// Append the specified decimal.
		/// </summary>
		/// <param name="item">Specified decimal.</param>
		/// <returns>The string.</returns>
		IMutableString IMutableString.Append(decimal item)
		{
			return Append(item);
		}

		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString IMutableString.Append(IReadOnlyString str)
		{
			for (int i = 0; i < str.Length; ++i)
				Append(str[i]);
			return this;
		}


		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		MutableString Append(IReadOnlyString str)
		{
			for (int i = 0; i < str.Length; ++i)
				Append(str[i]);
			return this;
		}

		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString Append(String str)
		{
			if (str == null)
				return this;
			Int32 tempLength = str.Length;
			Expand(_length + tempLength);
			str.CopyTo(0, _data, _length, tempLength);
			_length = _length + tempLength;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString IMutableString.Append(String str)
		{
			return Append(str);
		}
		/// <summary>
		/// Appends the specified char value.
		/// </summary>
		/// <param name="c">The char value.</param>
		public MutableString Append(Char c)
		{
			Expand(_length + 1);
			_data[_length] = c;
			++_length;
			ResetHashCode();

			return this;
		}

		/// <summary>
		/// Appends the specified char value.
		/// </summary>
		/// <param name="c">The char value.</param>
		IMutableString IMutableString.Append(Char c)
		{
			return Append(c);
		}

		/// <summary>
		/// Appends the integer.
		/// </summary>
		/// <param name="i">The integer value.</param>
		private MutableString AppendInteger(Int64 i)
		{
			Char t;
			Expand(_length + 21);
			if (i < 0)
			{
				Append('-');
				i = -i;
			}
			Int32 index = _length, index2;
			do
			{
				_data[index++] = (Char)((i % 10) + '0');
				i /= 10;
			} while (i != 0);
			index--;
			index2 = _length + ((index - _length) / 2);
			for (Int32 j = _length, l = index; j <= index2; ++j, --l)
			{
				t = _data[l];
				_data[l] = _data[j];
				_data[j] = t;
			}
			_length = index + 1;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public MutableString Append(Int16 i)
		{
			return AppendInteger(i);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString IMutableString.Append(Int16 i)
		{
			return Append(i);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public MutableString Append(Int32 i)
		{
			return AppendInteger(i);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString IMutableString.Append(Int32 i)
		{
			return Append(i);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public MutableString Append(Int64 i)
		{
			return AppendInteger(i);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString IMutableString.Append(Int64 i)
		{
			return Append(i);
		}

		/// <summary>
		/// Appends the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <returns>Self instance.</returns>
		public MutableString Append(Char[] str)
		{
			return Append(str, 0, str.Length);
		}

		/// <summary>
		/// Appends the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Append(Char[] str)
		{
			return Append(str);
		}

		/// <summary>
		/// Appends the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <param name="offset">Offset in char array.</param>
		/// <param name="count">Number of chars to copy</param>
		/// <returns>Self instance.</returns>
		public MutableString Append(Char[] str, Int32 offset, Int32 count)
		{
			Expand(_length + count);
			Buffer.BlockCopy(str, offset * sizeof(char), _data, _length * sizeof(char), count * sizeof(char));
			_length += count;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <param name="offset">Offset in char array.</param>
		/// <param name="count">Number of chars to copy</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Append(Char[] str, Int32 offset, Int32 count)
		{
			return Append(str, offset, count);
		}

		/// <summary>
		/// Appends the specified string in form of pointer to char array.
		/// </summary>
		/// <param name="bytes">String in form of char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		/// <returns>Self instance.</returns>
		public unsafe MutableString Append(Char* bytes, Int32 count)
		{
			Expand(_length + count);
			Marshal.Copy((IntPtr)(void*)bytes, _data, _length, count);
			_length += count;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified string in form of pointer to char array.
		/// </summary>
		/// <param name="bytes">String in form of char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString IMutableString.Append(Char* bytes, Int32 count)
		{
			return Append(bytes, count);
		}

		/// <summary>
		/// Appends the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		public MutableString AppendUTF8(Byte[] bytes)
		{
			return AppendUTF8(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Appends the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AppendUTF8(Byte[] bytes)
		{
			return AppendUTF8(bytes);
		}

		/// <summary>
		/// Appends the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public MutableString AppendUTF8(Byte[] bytes, Int32 offset, Int32 count)
		{
			Expand(_length + GetMaxCharsForUTF8Bytes(count));
			_length += Encoding.UTF8.GetChars(bytes, offset, count, _data, _length);
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AppendUTF8(Byte[] bytes, Int32 offset, Int32 count)
		{
			return AppendUTF8(bytes, offset, count);
		}

		/// <summary>
		/// Appends the specified UTF8 string in form of pointer to byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public unsafe MutableString AppendUTF8(Byte* bytes, Int32 count)
		{
			Expand(_length + GetMaxCharsForUTF8Bytes(count));
			fixed (Char* _dataPtr = _data)
			{
				_length += Encoding.UTF8.GetChars(bytes, count, _dataPtr + _length, _data.Length - _length);
			}
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified UTF8 string in form of pointer to byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString IMutableString.AppendUTF8(Byte* bytes, Int32 count)
		{
			return AppendUTF8(bytes, count);
		}

		/// <summary>
		/// Appends the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		public MutableString AppendUTF16(Byte[] bytes)
		{
			return AppendUTF16(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Appends the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AppendUTF16(Byte[] bytes)
		{
			return AppendUTF16(bytes);
		}

		/// <summary>
		/// Appends the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public MutableString AppendUTF16(Byte[] bytes, Int32 offset, Int32 count)
		{
			Expand(_length + GetMaxCharsForUTF16Bytes(count));
			_length += Encoding.Unicode.GetChars(bytes, offset, count, _data, _length);
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AppendUTF16(Byte[] bytes, Int32 offset, Int32 count)
		{
			return AppendUTF16(bytes, offset, count);
		}

		/// <summary>
		/// Appends the specified UTF16 string in form of pointer to byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public unsafe MutableString AppendUTF16(Byte* bytes, Int32 count)
		{
			Expand(_length + GetMaxCharsForUTF16Bytes(count));
			fixed (Char* _dataPtr = _data)
			{
				_length += Encoding.Unicode.GetChars(bytes, count, _dataPtr + _length, count / 2);
			}
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Appends the specified UTF16 string in form of pointer to byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString IMutableString.AppendUTF16(Byte* bytes, Int32 count)
		{
			return AppendUTF16(bytes, count);
		}

		/// <summary>
		/// Appends hexidecimal representation on specified <see cref="BinaryArray"/> to this string.
		/// </summary>
		/// <param name="binaryArray">Binary array to append.</param>
		/// <returns>Self instance.</returns>
		public MutableString Append(IBinaryConvertibleReadOnly binaryArray)
		{
			ResetHashCode();
			for (int i = 0; i < binaryArray.Count; i += 2)
			{
				Append(binaryArray.ToChar(i));
			}
			return this;
		}

		/// <summary>
		/// Appends hexidecimal representation on specified <see cref="BinaryArray"/> to this string.
		/// </summary>
		/// <param name="binaryArray">Binary array to append.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Append(IBinaryConvertibleReadOnly binaryArray)
		{
			return Append(binaryArray);
		}

		/// <summary>
		/// Converts specified <see cref="UUID"/>, using specified <see cref="UUIDPrintFormat"/>
		/// into string and appends it.
		/// </summary>
		/// <param name="uuid">Universally unique identifier.</param>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>Self instance.</returns>
		public MutableString Append(UUID uuid, UUIDPrintFormat format = UUIDPrintFormat.UpperCase)
		{
			ResetHashCode();

			char[] hexDigits = (format == UUIDPrintFormat.LowerCase || format == UUIDPrintFormat.LowerCaseWithoutDashes)
					? HexDigitsLower : HexDigitsUpper;

			switch (format)
			{
				case UUIDPrintFormat.LowerCase:
				case UUIDPrintFormat.UpperCase:
					ulong m = uuid.MSB;
					ulong l = uuid.LSB;
					for (int i = 15; i > 7; i -= 1)
						Append(hexDigits[(int)(m >> (i * 4)) & 0xF]);
					Append('-');
					for (int i = 7; i > 3; i -= 1)
						Append(hexDigits[(int)(m >> (i * 4)) & 0xF]);
					Append('-');
					for (int i = 3; i >= 0; i -= 1)
						Append(hexDigits[(int)(m >> (i * 4)) & 0xF]);
					Append('-');

					for (int i = 15; i > 11; i -= 1)
						Append(hexDigits[(int)(l >> (i * 4)) & 0xF]);
					Append('-');
					for (int i = 11; i >= 0; i -= 1)
						Append(hexDigits[(int)(l >> (i * 4)) & 0xF]);
					break;
				case UUIDPrintFormat.LowerCaseWithoutDashes:
				case UUIDPrintFormat.UpperCaseWithoutDashes:
					byte[] bytes = uuid.ToBytes();
					foreach (byte b in bytes)
					{
						int v = b & 0xFF;
						Append(hexDigits[v >> 4]);
						Append(hexDigits[v & 0x0F]);
					}
					break;
			}

			return this;
		}

		/// <summary>
		/// Converts specified <see cref="UUID"/>, using specified <see cref="UUIDPrintFormat"/>
		/// into string and appends it.
		/// </summary>
		/// <param name="uuid">Universally unique identifier.</param>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Append(UUID uuid, UUIDPrintFormat format)
		{
			return Append(uuid, format);
		}


		/// <summary>
		/// Appends the specified <see cref="DateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="DateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString IMutableString.Append(DateTime item)
		{
			return Append(item);
		}
		/// <summary>
		/// Appends the specified <see cref="TimeSpan"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="TimeSpan"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString IMutableString.Append(TimeSpan item)
		{
			return Append(item);
		}
		/// <summary>
		/// Appends the specified <see cref="HdDateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="HdDateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString IMutableString.Append(HdDateTime item)
		{
			return Append(item);
		}
		/// <summary>
		/// Appends the specified <see cref="HdTimeSpan"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="HdTimeSpan"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString IMutableString.Append(HdTimeSpan item)
		{
			return Append(item);
		}
		/// <summary>
		/// Appends the specified bool to this string.
		/// </summary>
		/// <param name="item">bool to append.</param>
		/// <returns>Self instance</returns>
		IMutableString IMutableString.Append(bool item)
		{
			return Append(item);
		}
		/// <summary>
		/// Appends the specified double to this string.
		/// </summary>
		/// <param name="item">double to append.</param>
		/// <returns>Self instance</returns>
		IMutableString IMutableString.Append(double item)
		{
			return Append(item);
		}
		/// <summary>
		/// Appends the specified float to this string.
		/// </summary>
		/// <param name="item">float to append.</param>
		/// <returns>Self instance</returns>
		IMutableString IMutableString.Append(float item)
		{
			return Append(item);
		}

		void AppendDateTimeInts(long item, bool isMilliSeconds = false)
		{
			if (isMilliSeconds && item < 100) Append('0');
			if (item < 10) Append('0');
			Append(item);
		}
		/// <summary>
		/// Appends the specified <see cref="DateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="DateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		public MutableString Append(DateTime item)
		{
			ResetHashCode();
			AppendDateTimeInts(item.Month);
			Append('/');
			AppendDateTimeInts(item.Day);
			Append('/');
			Append(item.Year);
			Append(' ');
			AppendDateTimeInts(item.Hour);
			Append(':');
			AppendDateTimeInts(item.Minute);
			Append(':');
			AppendDateTimeInts(item.Second);
			Append('.');
			AppendDateTimeInts(item.Millisecond, true);
			return this;
		}
		/// <summary>
		/// Appends the specified <see cref="TimeSpan"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="TimeSpan"/> to append.</param>
		/// <returns>Self instance</returns>
		public MutableString Append(TimeSpan item)
		{
			ResetHashCode();
			Append(item.Days);
			Append('.');
			AppendDateTimeInts(item.Hours);
			Append(':');
			AppendDateTimeInts(item.Minutes);
			Append(':');
			AppendDateTimeInts(item.Seconds);
			Append('.');
			AppendDateTimeInts(item.Milliseconds, true);
			return this;
		}
		/// <summary>
		/// Appends the specified <see cref="HdDateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="HdDateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		public MutableString Append(HdDateTime item)
		{
			ResetHashCode();
			Append(item.Timestamp);
			Append('.');
			AppendDateTimeInts(item.TimestampModulo);
			return this;
		}
		/// <summary>
		/// Appends the specified <see cref="HdTimeSpan"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="HdTimeSpan"/> to append.</param>
		/// <returns>Self instance</returns>
		public MutableString Append(HdTimeSpan item)
		{
			ResetHashCode();
			Append(item.TimeSpan);
			Append('.');
			AppendDateTimeInts(item.TimeSpanModulo);
			return this;
		}
		/// <summary>
		/// Appends the specified bool to this string.
		/// </summary>
		/// <param name="item">bool to append.</param>
		/// <returns>Self instance</returns>
		public MutableString Append(bool item)
		{
			ResetHashCode();
			if (item == true) Append("True");
			else Append("False");
			return this;
		}
		/// <summary>
		/// Appends the specified double to this string.
		/// </summary>
		/// <param name="item">double to append.</param>
		/// <returns>Self instance</returns>
		public MutableString Append(double item)
		{
			ResetHashCode();
			return Append(item.ToString());
		}
		/// <summary>
		/// Appends the specified float to this string.
		/// </summary>
		/// <param name="item">float to append.</param>
		/// <returns>Self instance</returns>
		public MutableString Append(float item)
		{
			ResetHashCode();
			return Append(item.ToString());
		}




		#endregion Append

		#region Assign

		///// <summary>
		///// Assigns the specified string.
		///// </summary>
		// <param name="str">The string.</param>
		//public void Assign(java.lang.StringBuffer str)
		//{
		//	if (str == null)
		//	{
		//		_length = 0;
		//		return;
		//	}
		//	_length = str.length();
		//	Expand(_length);
		//	for (Int32 i = 0; i < _length; ++i)
		//		_data[i] = str.charAt(i);
		//}


		/// <summary>
		/// Assign the specified object.
		/// </summary>
		/// <param name="item">Specified object.</param>
		/// <returns>The string.</returns>
		IMutableString IMutableString.Assign(Object item)
		{
			return Assign(item);
		}
		/// <summary>
		/// Assign the specified decimal.
		/// </summary>
		/// <param name="item">Specified decimal.</param>
		/// <returns>The string.</returns>
		IMutableString IMutableString.Assign(decimal item)
		{
			return Assign(item);
		}


		/// <summary>
		/// Assign the specified object.
		/// </summary>
		/// <param name="item">Specified object.</param>
		/// <returns>The string.</returns>
		public MutableString Assign(Object item)
		{
			Clear();
			return Append(item);
		}
		/// <summary>
		/// Assign the specified decimal.
		/// </summary>
		/// <param name="item">Specified decimal.</param>
		/// <returns>The string.</returns>
		public MutableString Assign(decimal item)
		{
			Clear();
			return Append(item);
		}


		/// <summary>
		/// Assigns string representation on specified <see cref="HdDateTime"/>.
		/// </summary>
		/// <param name="item">HdDateTime to assign.</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(HdDateTime item)
		{
			Clear();
			return Append(item);
		}
		/// <summary>
		/// Assigns string representation on specified <see cref="HdTimeSpan"/>.
		/// </summary>
		/// <param name="item">HdTimeSpan to assign.</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(HdTimeSpan item)
		{
			Clear();
			return Append(item);
		}
		/// <summary>
		/// Assigns string representation on specified <see cref="DateTime"/>.
		/// </summary>
		/// <param name="item">DateTime to assign.</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(DateTime item)
		{
			Clear();
			return Append(item);
		}
		/// <summary>
		/// Assigns string representation on specified <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="item">TimeSpan to assign.</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(TimeSpan item)
		{
			Clear();
			return Append(item);
		}
		/// <summary>
		/// Assigns the specified bool value.
		/// </summary>
		/// <param name="item">The bool value.</param>
		public MutableString Assign(bool item)
		{
			Clear();
			return Append(item);
		}
		/// <summary>
		/// Assigns the specified double value.
		/// </summary>
		/// <param name="item">The double value.</param>
		public MutableString Assign(double item)
		{
			Clear();
			return Append(item);
		}
		/// <summary>
		/// Assigns the specified float value.
		/// </summary>
		/// <param name="item">The float value.</param>
		public MutableString Assign(float item)
		{
			Clear();
			return Append(item);
		}


		/// <summary>
		/// Assigns string representation on specified <see cref="HdDateTime"/>.
		/// </summary>
		/// <param name="item">HdDateTime to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(HdDateTime item)
		{
			return Assign(item);
		}
		/// <summary>
		/// Assigns string representation on specified <see cref="HdTimeSpan"/>.
		/// </summary>
		/// <param name="item">HdTimeSpan to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(HdTimeSpan item)
		{
			return Assign(item);
		}
		/// <summary>
		/// Assigns string representation on specified <see cref="DateTime"/>.
		/// </summary>
		/// <param name="item">DateTime to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(DateTime item)
		{
			return Assign(item);
		}
		/// <summary>
		/// Assigns string representation on specified <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="item">TimeSpan to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(TimeSpan item)
		{
			return Assign(item);
		}
		/// <summary>
		/// Assigns the specified bool value.
		/// </summary>
		/// <param name="item">The bool value.</param>
		IMutableString IMutableString.Assign(bool item)
		{
			return Assign(item);
		}
		/// <summary>
		/// Assigns the specified double value.
		/// </summary>
		/// <param name="item">The double value.</param>
		IMutableString IMutableString.Assign(double item)
		{
			return Assign(item);
		}
		/// <summary>
		/// Assigns the specified float value.
		/// </summary>
		/// <param name="item">The float value.</param>
		IMutableString IMutableString.Assign(float item)
		{
			return Assign(item);
		}

		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString Assign(StringBuilder str)
		{
			if (str == null)
			{
				Clear();
				return this;
			}
			_length = str.Length;

			Expand(_length);
			str.CopyTo(0, _data, 0, _length);
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString IMutableString.Assign(StringBuilder str)
		{
			return Assign(str);
		}

		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString Assign(MutableString str)
		{
			if (str == null)
			{
				Clear();
				return this;
			}
			_length = str.Length;

			_cachedHash = str._cachedHash;
			Expand(_length);
			str.ToCharArray(0, _data, 0, _length);
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		MutableString Assign(IReadOnlyString str)
		{
			Clear();
			return (MutableString)(this as IMutableString).Append(str);
		}


		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString IMutableString.Assign(IReadOnlyString str)
		{
			Clear();
			return (this as IMutableString).Append(str);
		}

		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		public MutableString Assign(String str)
		{
			if (str == null)
			{
				Clear();
				return this;
			}
			_length = str.Length;

			Expand(_length);
			str.CopyTo(0, _data, 0, _length);
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString IMutableString.Assign(String str)
		{
			return Assign(str);
		}

		/// <summary>
		/// Assigns the specified char.
		/// </summary>
		/// <param name="c">The char value.</param>
		public MutableString Assign(Char c)
		{
			_length = 1;
			_data[0] = c;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Assigns the specified char.
		/// </summary>
		/// <param name="c">The char value.</param>
		IMutableString IMutableString.Assign(Char c)
		{
			return Assign(c);
		}

		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public MutableString Assign(Int16 i)
		{
			_length = 0;
			return AppendInteger(i);
		}

		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString IMutableString.Assign(Int16 i)
		{
			return Assign(i);
		}

		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public MutableString Assign(Int32 i)
		{
			_length = 0;
			return AppendInteger(i);
		}

		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString IMutableString.Assign(Int32 i)
		{
			return Assign(i);
		}

		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public MutableString Assign(Int64 i)
		{
			_length = 0;
			return AppendInteger(i);
		}

		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString IMutableString.Assign(Int64 i)
		{
			return Assign(i);
		}

		/// <summary>
		/// Assigns the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(Char[] str)
		{
			_length = 0;
			return Append(str);
		}

		/// <summary>
		/// Assigns the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(Char[] str)
		{
			return Assign(str);
		}

		/// <summary>
		/// Assigns the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <param name="offset">Offset in char array.</param>
		/// <param name="count">Number of chars to copy</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(Char[] str, Int32 offset, Int32 count)
		{
			_length = 0;
			return Append(str, offset, count);
		}

		/// <summary>
		/// Assigns the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <param name="offset">Offset in char array.</param>
		/// <param name="count">Number of chars to copy</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(Char[] str, Int32 offset, Int32 count)
		{
			return Assign(str, offset, count);
		}




		/// <summary>
		/// Assigns the specified string in form of pointer to char array.
		/// </summary>
		/// <param name="str">String in form of pointer to char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		/// <returns>Self instance.</returns>
		public unsafe MutableString Assign(Char* str, Int32 count)
		{
			_length = 0;
			return Append(str, count);
		}

		/// <summary>
		/// Assigns the specified string in form of pointer to char array.
		/// </summary>
		/// <param name="str">String in form of pointer to char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString IMutableString.Assign(Char* str, Int32 count)
		{
			return Assign(str, count);
		}

		/// <summary>
		/// Assigns the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		public MutableString AssignUTF8(Byte[] utf8)
		{
			_length = 0;
			return AppendUTF8(utf8);
		}

		/// <summary>
		/// Assigns the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AssignUTF8(Byte[] utf8)
		{
			return AssignUTF8(utf8);
		}

		/// <summary>
		/// Assigns the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public MutableString AssignUTF8(Byte[] utf8, Int32 offset, Int32 count)
		{
			_length = 0;
			return AppendUTF8(utf8, offset, count);
		}

		/// <summary>
		/// Assigns the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AssignUTF8(Byte[] utf8, Int32 offset, Int32 count)
		{
			return AssignUTF8(utf8, offset, count);
		}

		/// <summary>
		/// Assigns the specified UTF8 string in form of pointer to byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public unsafe MutableString AssignUTF8(Byte* utf8, Int32 count)
		{
			_length = 0;
			return AppendUTF8(utf8, count);
		}

		/// <summary>
		/// Assigns the specified UTF8 string in form of pointer to byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString IMutableString.AssignUTF8(Byte* utf8, Int32 count)
		{
			return AssignUTF8(utf8, count);
		}

		/// <summary>
		/// Assigns the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		public MutableString AssignUTF16(Byte[] utf16)
		{
			_length = 0;
			return AppendUTF16(utf16);
		}

		/// <summary>
		/// Assigns the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AssignUTF16(Byte[] utf16)
		{
			return AssignUTF16(utf16);
		}

		/// <summary>
		/// Assigns the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public MutableString AssignUTF16(Byte[] utf16, Int32 offset, Int32 count)
		{
			_length = 0;
			return AppendUTF16(utf16, offset, count);
		}

		/// <summary>
		/// Assigns the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.AssignUTF16(Byte[] utf16, Int32 offset, Int32 count)
		{
			return AssignUTF16(utf16, offset, count);
		}

		/// <summary>
		/// Assigns the specified UTF16 string in form of pointer to byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		public unsafe MutableString AssignUTF16(Byte* utf16, Int32 count)
		{
			_length = 0;
			return AppendUTF16(utf16, count);
		}

		/// <summary>
		/// Assigns the specified UTF16 string in form of pointer to byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString IMutableString.AssignUTF16(Byte* utf16, Int32 count)
		{
			return AssignUTF16(utf16, count);
		}

		/// <summary>
		/// Assigns string representation on specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Binary array to assign.</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(IBinaryConvertibleReadOnly binaryArray)
		{
			_length = 0;
			return Append(binaryArray);
		}

		/// <summary>
		/// Assigns string representation on specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Binary array to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(IBinaryConvertibleReadOnly binaryArray)
		{
			return Assign(binaryArray);
		}

		/// <summary>
		/// Assigns string representation on specified <see cref="UUID"/>,
		/// using specified <see cref="UUIDPrintFormat"/>.
		/// </summary>
		/// <param name="uuid">Universally unique identifier.</param>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>Self instance.</returns>
		public MutableString Assign(UUID uuid, UUIDPrintFormat format)
		{
			_length = 0;
			return Append(uuid, format);
		}

		/// <summary>
		/// Assigns string representation on specified <see cref="UUID"/>,
		/// using specified <see cref="UUIDPrintFormat"/>.
		/// </summary>
		/// <param name="uuid">Universally unique identifier.</param>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>Self instance.</returns>
		IMutableString IMutableString.Assign(UUID uuid, UUIDPrintFormat format)
		{
			return Assign(uuid, format);
		}

		#endregion Assign


		IMutableString IMutableString.SecureClear()
		{
			return this.SecureClear();
		}

		public MutableString SecureClear()
		{
			Array.Clear(_data, 0, _data.Length);
			return Clear();
		}


		/// <summary>
		/// Clears this instance.
		/// </summary>
		public MutableString Clear()
		{
			_length = 0;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Clears this instance
		/// </summary>
		/// <returns></returns>
		IMutableString IMutableString.Clear()
		{
			return Clear();
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public MutableString Clone()
		{
			MutableString result = new MutableString()
			{
				_cachedHash = this._cachedHash,
				_length = this._length,
				_data = (Char[])this._data.Clone()
			};
			return result;
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		IMutableString IReadOnlyString.Clone()
		{
			return Clone();
		}

		///// <summary>
		///// Copies to.
		///// </summary>
		// <param name="sourceIndex">Index of the source.</param>
		// <param name="destination">The destination.</param>
		// <param name="destinationIndex">Index of the destination.</param>
		// <param name="count">The count.</param>
		/// <param name="str"/>
		/// <exception cref="System.IndexOutOfRangeException"></exception>
		public void CopyTo(IMutableString str)
		{
			str.Clear();
			str.Assign(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceIndex"></param>
		/// <param name="destination"></param>
		/// <param name="destinationIndex"></param>
		/// <param name="count"></param>
		public void ToCharArray(Int32 sourceIndex, Char[] destination, Int32 destinationIndex, Int32 count)
		{
			if (sourceIndex < 0 || sourceIndex + count > _length)
				throw new IndexOutOfRangeException();
			Buffer.BlockCopy(_data, sourceIndex * sizeof(Char), destination, destinationIndex * sizeof(Char), count * sizeof(Char));
		}

		/// <summary>
		/// Expands the specified capacity.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public void Expand(Int32 capacity)
		{
			if (_data.Length >= capacity)
				return;
			Array.Resize(ref _data, capacity);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override String ToString()
		{
			if (cachedToString == null)
			{
				cachedToString = new String(_data, 0, _length);
			}
			return cachedToString;
		}

		internal void ResetHashCode()
		{
			_cachedHash = _cachedHash | 0x40000000;
			cachedToString = null;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override Int32 GetHashCode()
		{
			int hashCode = 0;
			if ((_cachedHash & 0x40000000) > 0)
			{
				hashCode = 0;
				for (Int32 i = 0; i < _length; ++i)
				{
					hashCode = hashCode * 28561;
					hashCode += _data[i];
				}
				hashCode = hashCode & (0x3FFFFFFF);
				_cachedHash = hashCode;
			}
			return _cachedHash;
		}

		/// <summary>
		/// Gets or sets the <see cref="Char"/> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="Char"/>.
		/// </value>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// </exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public Char this[Int32 index]
		{
			get
			{
				if (index < 0 || index >= _length)
					throw new IndexOutOfRangeException();

				return _data[index];
			}

			set
			{
				if (index < 0 || index >= _length)
					throw new IndexOutOfRangeException();

				_data[index] = value;
				ResetHashCode();
			}
		}

		/// <summary>
		/// Convert value to the int64.
		/// </summary>
		/// <returns>Converted int64 value.</returns>
		public Int64 ToInt64()
		{
			Int64 result = 0;
			for (Int32 i = 0; i < _length; ++i)
			{
				result *= 10;
				result += _data[i] - '0';
			}

			return result;
		}

		/// <summary>
		/// Convert string to UTF8 and store it into byte array.
		/// </summary>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <returns>Number of bytes written.</returns>
		public Int32 ToUTF8(Byte[] utf8)
		{
			return Encoding.UTF8.GetBytes(_data, 0, _length, utf8, 0);
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte array.
		/// </summary>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <returns>Number of bytes written.</returns>
		public Int32 ToUTF8(Byte[] utf8, Int32 offset)
		{
			return Encoding.UTF8.GetBytes(_data, 0, _length, utf8, offset);
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte buffer.
		/// </summary>
		/// <param name="utf8">Byte buffer to store utf8 string.</param>
		/// <returns>Number of bytes written.</returns>
		public unsafe Int32 ToUTF8(Byte* utf8)
		{
			fixed (Char* str = _data)
			{
				return Encoding.UTF8.GetBytes(str, _length, utf8, 0);
			}
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte buffer.
		/// </summary>
		/// <param name="utf8">Byte buffer to store utf8 string.</param>
		/// <param name="byteCount">Number of bytes in byte buffer.</param>
		/// <returns>Number of bytes written.</returns>
		public unsafe Int32 ToUTF8(Byte* utf8, Int32 byteCount)
		{
			fixed (Char* str = _data)
			{
				return Encoding.UTF8.GetBytes(str, _length, utf8, byteCount);
			}
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte array.
		/// </summary>
		/// <param name="charIndex">Start index.</param>
		/// <param name="charCount">Count of char to convert.</param>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <param name="utf8Index">Offset in byte array.</param>
		/// <returns>Number of bytes written.</returns>
		public Int32 ToUTF8(Int32 charIndex, Int32 charCount, Byte[] utf8, Int32 utf8Index)
		{
			return Encoding.UTF8.GetBytes(_data, charIndex, charCount, utf8, utf8Index);
		}

		/// <summary>
		/// Assigns binary representation of this string to specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Output <see cref="BinaryArray"/>, containing binary representation of this string.</param>
		public void ToBinaryArray(IBinaryConvertibleReadWrite binaryArray)
		{
			binaryArray.Assign(this, false);
		}

		/// <summary>
		/// Returns binary representation of this string.
		/// </summary>
		/// <returns>New <see cref="BinaryArray"/> representation of this string.</returns>
		public BinaryArray ToBinaryArray()
		{
			return new BinaryArray(this);
		}

		/// <summary>
		/// Returns binary representation of this string.
		/// </summary>
		/// <returns>New <see cref="BinaryArray"/> representation of this string.</returns>
		IBinaryConvertibleReadOnly IReadOnlyString.ToBinaryArray()
		{
			return ToBinaryArray();
		}

		/// <summary>
		/// Create <see cref="UUID"/> from this string, using specified <see cref="UUIDParseFormat"/>.
		/// </summary>
		/// <param name="format">This string allowed uuid format.</param>
		/// <returns> New <see cref="UUID"/>.</returns>
		public UUID ToUUID(UUIDParseFormat format = UUIDParseFormat.Any)
		{
			return new UUID(this, format);
		}

		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		public Int32 Length { get { return _length; } set { Expand(value); _length = value; } }

		/// <summary>
		/// Defines if string has no characters (i.e. it's length equals zero).
		/// </summary>
		public bool Empty
		{
			get
			{
				return _length == 0;
			}
		}

		/// <summary>
		/// Gets or sets the capacity.
		/// </summary>
		/// <value>
		/// The capacity.
		/// </value>
		public Int32 Capacity { get { return _data.Length; } set { Expand(value); } }

		/// <summary>
		/// Get number of bytes in UTF8 representation of this string.
		/// </summary>
		public Int32 UTF8ByteLength { get { return Encoding.UTF8.GetByteCount(_data, 0, _length); } }

		#region IComparable Members

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="ms1">An object to compare with ba2 instance.</param>>
		/// <param name="ms2">An object to compare with ba1 instance.</param>>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero <paramref name="ms1"/> is less than the <paramref name="ms2"/> parameter.Zero <paramref name="ms1"/>is equal to <paramref name="ms2"/>. Greater than zero <paramref name="ms1"/> is greater than <paramref name="ms2"/>. 
		/// </returns>
		public static Int32 Compare(MutableString ms1, MutableString ms2)
		{
			if ((object)ms1 == null)
				throw new ArgumentNullException("ms1");

			return ms1.CompareTo(ms2);
		}

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>. Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
		public Int32 CompareTo(Object obj)
		{
			MutableString another = obj as MutableString;
			Int32 tempLength = another._length < _length ? another._length : _length;
			for (Int32 i = 0; i < tempLength; ++i)
				if (_data[i] != another._data[i])
					return _data[i] < another._data[i] ? -1 : 1;

			if (another._length == _length)
				return 0;

			return another._length < _length ? -1 : 1;
		}

		#endregion IComparable Members

		#region ICloneable Members

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		Object ICloneable.Clone()
		{
			return Clone() as Object;
		}

		#endregion ICloneable Members

		#region IConvertible Members

		/// <summary>
		/// Returns the <see cref="T:System.TypeCode"/> for this instance.
		/// </summary>
		/// <returns>
		/// The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// A Boolean value equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Boolean ToBoolean(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 8-bit unsigned integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Byte ToByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// A Unicode character equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Char ToChar(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public DateTime ToDateTime(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Decimal ToDecimal(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// A double-precision floating-point number equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Double ToDouble(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 16-bit signed integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Int16 ToInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 32-bit signed integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Int32 ToInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 64-bit signed integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Int64 ToInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 8-bit signed integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public SByte ToSByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// A single-precision floating-point number equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Single ToSingle(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent <see cref="T:System.String"/> using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> instance equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public String ToString(IFormatProvider provider)
		{
			return ToString();
		}

		/// <summary>
		/// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
		/// </returns>
		/// <param name="conversionType">The <see cref="T:System.Type"/> to which the value of this instance is converted. </param><param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public Object ToType(Type conversionType, IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 16-bit unsigned integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public UInt16 ToUInt16(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 32-bit unsigned integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public UInt32 ToUInt32(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
		/// </summary>
		/// <returns>
		/// An 64-bit unsigned integer equivalent to the value of this instance.
		/// </returns>
		/// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param><filterpriority>2</filterpriority>
		public UInt64 ToUInt64(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		#endregion IConvertible Members

		#region IComparable<MutableString> Members

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="another"/> parameter.Zero This object is equal to <paramref name="another"/>. Greater than zero This object is greater than <paramref name="another"/>. 
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public Int32 CompareTo(MutableString another)
		{
			Int32 tempLength = another._length < _length ? another._length : _length;
			for (Int32 i = 0; i < tempLength; ++i)
				if (_data[i] != another._data[i])
					return _data[i] < another._data[i] ? -1 : 1;

			if (another._length == _length)
				return 0;

			return another._length < _length ? -1 : 1;
		}

		#endregion IComparable<MutableString> Members

		#region IEnumerable<char> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<Char> GetEnumerator()
		{
			for (Int32 i = 0; i < _length; ++i)
				yield return _data[i];
		}

		#endregion IEnumerable<char> Members

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		IEnumerator IEnumerable.GetEnumerator()
		{
			for (Int32 i = 0; i < _length; ++i)
				yield return _data[i];
		}

		#endregion IEnumerable Members

		#region IEquatable<MutableString> Members

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="ms1">An object to compare with <paramref name="ms2"/>.</param>
		/// <param name="ms2">An object to compare with <paramref name="ms1"/>.</param>
		/// <returns>
		/// true if the <paramref name="ms1"/> is equal to the <paramref name="ms2"/> parameter; otherwise, false.
		/// </returns>
		public static Boolean Equals(MutableString ms1, MutableString ms2)
		{
			if (ms1 == null && ms2 == null) return true;
			if (ms1 == null || ms2 == null) return false;
			return ms1.Equals(ms2);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public Boolean Equals(MutableString another)
		{
			if (another == null)
				return false;
			if (another.Length != _length)
				return false;

			for (Int32 i = 0; i < _length; ++i)
				if (_data[i] != another[i])
					return false;
			return true;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public Boolean Equals(IReadOnlyString another)
		{
			if (another == null)
				return false;
			if (another.Length != Length)
				return false;
			for (int i = 0; i < Length; ++i)
				if (another[i] != this[i])
					return false;
			return true;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="another">An object to compare with this object.</param>
		/// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
		public Boolean Equals(String another)
		{
			if (another == null)
				return false;
			if (another.Length != _length)
				return false;

			for (Int32 i = 0; i < _length; ++i)
				if (_data[i] != another[i])
					return false;


			return true;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this object.</param>
		/// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
		public override Boolean Equals(Object obj)
		{
			if (obj is String) return Equals((String)obj);
			if (obj is IReadOnlyString) return Equals((IReadOnlyString)obj);
			if (obj is MutableString) return Equals((MutableString)obj);
			return false;
		}

		#endregion IEquatable<MutableString> Members

		#region Conversions

		/// <summary>
		/// Explicit cast operator from a String.
		/// </summary>
		/// <param name="s">String to convert.</param>
		/// <returns>MutableString result.</returns>
		public static explicit operator MutableString(String s)
		{
			if (s == null)
				return null;
			return new MutableString(s.Length, s);
		}

		/// <summary>
		/// Explicit cast operator to a String.
		/// </summary>
		/// <param name="s">MutableString to convert.</param>
		/// <returns>String result.</returns>
		public static explicit operator String(MutableString s)
		{
			if ((object)s == null)
				return null;
			return s.ToString();
		}

		#endregion Conversions

		/// <summary>
		/// Extract sub string.
		/// </summary>
		/// <param name="start">Start char index.</param>
		/// <param name="end">End char idnex.</param>
		/// <returns></returns>
		public MutableString SubString(Int32 start, Int32 end)
		{
			MutableString s = new MutableString();
			for (int i = start; i <= end; ++i)
				s.Append(_data[i]);
			return s;
		}

		/// <summary>
		/// Extract sub string.
		/// </summary>
		/// <param name="start">Start char index.</param>
		/// <param name="end">End char idnex.</param>
		/// <returns></returns>
		IReadOnlyString IReadOnlyString.SubString(Int32 start, Int32 end)
		{
			MutableString s = new MutableString();
			for (int i = start; i <= end; ++i)
				s.Append(_data[i]);
			return s;
		}

		/// <summary>
		/// Return index of first occurance of item or -1 if no occurance
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>index of first occurance of item</returns>
		public int IndexOf(char item)
		{
			for (int i = 0; i < _length; ++i)
				if (_data[i] == item)
					return i;
			return -1;
		}

		/// <summary>
		/// Return index of first occurance of item or -1 if no occurance
		/// </summary>
		/// <param name="item">item</param>
		/// <param name="comparisonType">comparison type</param>
		/// <returns></returns>
		public int IndexOf(MutableString item, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				for (int i = 0; i <= _length - item.Length; i++)
				{
					bool equals = true;
					for (int j = 0; j < item.Length; j++)
						if (item[j] != _data[i + j])
						{
							equals = false;
							break;
						}
					if (equals)
						return i;
				}
				return -1;
			}
			else if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				for (int i = 0; i <= _length - item.Length; i++)
				{
					bool equals = true;
					for (int j = 0; j < item.Length; j++)
						if (Char.ToLower(item[j]) != Char.ToLower(_data[i + j]))
						{
							equals = false;
							break;
						}
					if (equals)
						return i;
				}
				return -1;

			}
			else
			{
				return ToString().IndexOf(item.ToString(), comparisonType);
			}
		}

		/// <summary>
		/// Detect if string end equal item.
		/// </summary>
		/// <param name="item">item</param>
		/// <param name="comparisonType">comparison type</param>
		/// <returns>true if string end equal item</returns>
		public bool EndsWith(MutableString item, StringComparison comparisonType = StringComparison.Ordinal)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				if (item.Length > Length)
					return false;
				for (int j = 0; j < item.Length; j++)
					if (item[j] != _data[Length - item.Length + j])
						return false;
				return true;
			}
			else if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				if (item.Length > Length)
					return false;
				for (int j = 0; j < item.Length; j++)
					if (Char.ToLower(item[j]) != Char.ToLower(_data[Length - item.Length + j]))
						return false;
				return true;
			}
			else
			{
				return ToString().EndsWith(item.ToString(), comparisonType);
			}
		}

		/// <summary>
		/// Detect if string beginning equals item
		/// </summary>
		/// <param name="item">item</param>
		/// <param name="comparisonType">comparison type</param>
		/// <returns>true if string beggining equals item</returns>
		public bool StartsWith(MutableString item, StringComparison comparisonType = StringComparison.Ordinal)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				if (item.Length > Length)
					return false;
				for (int i = 0; i < item.Length; ++i)
					if (_data[i] != item[i])
						return false;
				return true;
			}
			else if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				if (item.Length > Length)
					return false;
				for (int i = 0; i < item.Length; ++i)
					if (Char.ToLower(_data[i]) != Char.ToLower(item[i]))
						return false;
				return true;
			}
			else
			{
				return ToString().StartsWith(item.ToString(), comparisonType);
			}
		}

		/// <summary>
		/// Return true if item equals this 
		/// </summary>
		/// <param name="item">item</param>
		/// <param name="comparisonType">comparison type</param>
		/// <returns>return true if item equals this</returns>
		public bool Equals(MutableString item, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				if (item.Length != Length)
					return false;
				for (int i = 0; i < Length; ++i)
					if (item[i] != _data[i])
						return false;
				return true;
			}
			else if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				if (item.Length != Length)
					return false;
				for (int i = 0; i < Length; ++i)
					if (Char.ToLower(item[i]) != Char.ToLower(_data[i]))
						return false;
				return true;
			}
			else
			{
				return ToString().Equals(item.ToString(), comparisonType);
			}
		}

		/// <summary>
		/// Return true if this contains item
		/// </summary>
		/// <param name="item">item</param>
		/// <param name="comparisonType">comparison type</param>
		/// <returns>true if this contains item</returns>
		public bool Contains(MutableString item, StringComparison comparisonType)
		{
			return IndexOf(item, comparisonType) != -1;
		}

		private void InsertGap(int index, int size)
		{
			Expand(_length + size + 1);
			for (int i = _length + size; i >= size + index; i--)
				_data[i] = _data[i - size];
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		public MutableString Insert(int index, char item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, 1);
			_data[index] = item;
			_length++;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString IMutableString.Insert(int index, char item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		public MutableString Insert(int index, String item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, item.Length);
			for (int i = 0; i < item.Length; ++i)
				_data[index + i] = item[i];
			item.CopyTo(0, _data, index, item.Length);
			_length += item.Length;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString IMutableString.Insert(int index, String item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		public MutableString Insert(int index, IReadOnlyString item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, item.Length);
			item.ToCharArray(0, _data, index, item.Length);
			_length += item.Length;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// CopyFrom source
		/// </summary>
		/// <param name="source">source</param>
		IMutableString IMutableString.CopyFrom(IReadOnlyString source)
		{
			return CopyFrom(source);
		}

		/// <summary>
		/// CopyFrom source
		/// </summary>
		/// <param name="source">source</param>
		public MutableString CopyFrom(IReadOnlyString source)
		{
			return Assign(source);
		}

		/// <summary>
		/// CopyFrom source
		/// </summary>
		/// <param name="source">source</param>
		IMutableString IMutableString.CopyFrom(String source)
		{
			return CopyFrom(source);
		}

		/// <summary>
		/// CopyFrom source
		/// </summary>
		/// <param name="source">source</param>
		public MutableString CopyFrom(String source)
		{
			return Assign(source);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString IMutableString.Insert(int index, IReadOnlyString item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		public MutableString Insert(int index, StringBuilder item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, item.Length);
			item.CopyTo(0, _data, index, item.Length);
			_length += item.Length;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString IMutableString.Insert(int index, StringBuilder item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		public MutableString Insert(int index, Char[] item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			return Insert(index, item, 0, item.Length);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString IMutableString.Insert(int index, Char[] item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		public MutableString Insert(int index, Char[] item, int offset, int count)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, count);
			Buffer.BlockCopy(item, offset * sizeof(char), _data, index * sizeof(char), count * sizeof(char));
			_length += count;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IMutableString IMutableString.Insert(int index, Char[] item, int offset, int count)
		{
			return Insert(index, item, offset, count);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe public MutableString Insert(int index, Char* item, int count)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, count);
			Marshal.Copy((IntPtr)(void*)item, _data, index, count);
			_length += count;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe IMutableString IMutableString.Insert(int index, Char* item, int count)
		{
			return Insert(index, item, count);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public MutableString Insert(int index, Int64 item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			//return Insert(index, item.ToString());
			int kolOfDigit = 0;
			Int64 x = item;
			if (item <= 0)
			{
				kolOfDigit++;
				item = -item;
			}
			while (item > 0)
			{
				kolOfDigit++;
				item = item / 10;
			}
			int oldLength = _length + kolOfDigit;
			InsertGap(index, kolOfDigit);
			_length = index;
			AppendInteger(x);
			_length = oldLength;
			return this;
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IMutableString IMutableString.Insert(int index, Int64 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public MutableString Insert(int index, Int32 item)
		{
			return Insert(index, (Int64)item);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IMutableString IMutableString.Insert(int index, Int32 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public MutableString Insert(int index, Int16 item)
		{
			return Insert(index, (Int64)item);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IMutableString IMutableString.Insert(int index, Int16 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		void IList<Char>.Insert(int index, char item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, 1);
			_data[index] = item;
			_length++;
			ResetHashCode();
		}

		/// <summary>
		/// Append chars to MutableString
		/// </summary>
		/// <param name="c">chars</param>
		/// <returns>this</returns>
		public MutableString Append(ReadOnlySpan<char> c)
		{
			Expand(_length + c.Length);
			unsafe
			{
				fixed (char* dataPtr = _data)
				{
					c.CopyTo(new Span<char>(_data, _length, c.Length));
				}
			}
			_length += c.Length;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Assign MutableString by chars
		/// </summary>
		/// <param name="c">Chars</param>
		/// <returns>this</returns>
		public MutableString Assign(ReadOnlySpan<char> c)
		{
			Clear();
			return Append(c);
		}

		/// <summary>
		/// Copy data to charSpan
		/// </summary>
		/// <param name="charSpan">charSpan</param>
		public void ToCharArray(Span<char> charSpan)
		{
			new Span<char>(_data, 0, _length).CopyTo(charSpan);
		}


		/// <summary>
		/// Insert chars to MutableString
		/// </summary>
		/// <param name="index">Index to insert</param>
		/// <param name="item">Chars to insert</param>
		/// <returns>this</returns>
		public MutableString Insert(int index, ReadOnlySpan<char> item)
		{
			if (index > _length)
				throw new IndexOutOfRangeException("insert in incorrect place");

			InsertGap(index, item.Length);
			unsafe
			{
				fixed (char* dataPtr = _data)
				{
					item.CopyTo(new Span<char>(_data, index, item.Length));
				}
			}
			_length += item.Length;
			ResetHashCode();
			return this;

		}

		/// <summary>
		/// Append chars to MutableString
		/// </summary>
		/// <param name="c">chars</param>
		/// <returns>this</returns>
		IMutableString IMutableString.Append(ReadOnlySpan<char> c)
		{
			return Append(c);
		}

		/// <summary>
		/// Assign MutableString by chars
		/// </summary>
		/// <param name="c">Chars</param>
		/// <returns>this</returns>
		IMutableString IMutableString.Assign(ReadOnlySpan<char> c)
		{
			return Assign(c);
		}


		/// <summary>
		/// Insert chars to MutableString
		/// </summary>
		/// <param name="index">Index to insert</param>
		/// <param name="item">Chars to insert</param>
		/// <returns>this</returns>
		IMutableString IMutableString.Insert(int index, ReadOnlySpan<char> item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Removes whitespaces from the start and the end of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		public MutableString Trim()
		{
			TrimLeft();
			return TrimRight();
		}

		/// <summary>
		/// Removes whitespaces from the start and the end of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		IMutableString IMutableString.Trim()
		{
			return Trim();
		}

		/// <summary>
		/// Removes whitespaces from the start of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		public MutableString TrimLeft()
		{
			int startIndex = 0;
			while (startIndex < _length && Char.IsWhiteSpace(this[startIndex]))
			{
				++startIndex;
			}
			_length = _length > startIndex ? _length - startIndex : 0;
			Array.Copy(_data, startIndex, _data, 0, _length);
			return this;
		}

		/// <summary>
		/// Removes whitespaces from the start of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		IMutableString IMutableString.TrimLeft()
		{
			return TrimLeft();
		}

		/// <summary>
		/// Removes whitespaces from the end of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		public MutableString TrimRight()
		{
			while (_length > 0 && Char.IsWhiteSpace(this[_length - 1]))
			{
				--_length;
			}
			return this;
		}

		/// <summary>
		/// Removes whitespaces from the end of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		IMutableString IMutableString.TrimRight()
		{
			return TrimRight();
		}

		/// <summary>
		/// Remove item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>link to this</returns>
		public MutableString RemoveAt(int index)
		{
			if (index >= _length)
				throw new IndexOutOfRangeException("incorrect index");

			for (int i = index; i < _length - 1; i++)
				_data[i] = _data[i + 1];
			_length--;
			ResetHashCode();
			return this;
		}

		/// <summary>
		/// Remove item at index
		/// </summary>
		/// <param name="index">index</param>
		void IList<Char>.RemoveAt(int index)
		{
			if (index >= _length)
				throw new IndexOutOfRangeException("incorrect index");

			for (int i = index; i < _length - 1; i++) _data[i] = _data[i + 1];
			_length--;
			ResetHashCode();
		}

		/// <summary>
		/// Pushback item
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>link to this</returns> 
		public MutableString Add(char item)
		{
			Append(item);
			return this;
		}

		/// <summary>
		/// Pushback item
		/// </summary>
		/// <param name="item">item</param>
		void ICollection<Char>.Add(char item)
		{
			Append(item);
		}

		/// <summary>
		/// return true if string contains item
		/// </summary>
		/// <param name="item">true</param>
		/// <returns>true if string contains item</returns>
		public bool Contains(char item)
		{
			return IndexOf(item) != -1;
		}

		/// <summary>
		/// Copy string to char array with offset
		/// </summary>
		/// <param name="array">array</param>
		/// <param name="arrayIndex">offset</param>
		public void CopyTo(char[] array, int arrayIndex)
		{
			for (int i = 0; i < _length; i++)
				array[arrayIndex + i] = _data[i];
		}

		/// <summary>
		/// Number of elementes
		/// </summary>
		public int Count
		{
			get { return _length; }
		}

		/// <summary>
		/// IsReadOnly
		/// </summary>
		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Remove first occurance of item
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>false if no occurance</returns>
		bool ICollection<Char>.Remove(char item)
		{

			int index = IndexOf(item);
			if (index == -1)
				return false;
			RemoveAt(index);
			return true;
		}

		/// <summary>
		/// Remove first occurance of item
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public MutableString Remove(char item)
		{
			int index = IndexOf(item);
			if (index == -1)
				return this;
			RemoveAt(index);
			return this;
		}

		void ICollection<Char>.Clear()
		{
			_length = 0;
			ResetHashCode();
		}

		/// <summary>
		/// Operator ==
		/// </summary>
		/// <param name="s1">first string</param>
		/// <param name="s2">second string</param>
		/// <returns>return true if first string equals second string</returns>
		public static bool operator ==(MutableString s2, MutableString s1)
		{
			if (Object.ReferenceEquals(s1, s2))
				return true;
			if (Object.ReferenceEquals(s1, null) || Object.ReferenceEquals(s2, null))
				return false;
			if (s1.Length != s2.Length)
				return false;
			for (int i = 0; i < s1.Length; ++i)
				if (s1[i] != s2[i])
					return false;
			return true;
		}

		/// <summary>
		/// Operator !=
		/// </summary>
		/// <param name="s1">first string</param>
		/// <param name="s2">second string</param>
		/// <returns>return true if first string not equals second string</returns>
		public static bool operator !=(MutableString s2, MutableString s1)
		{
			if (Object.ReferenceEquals(s1, s2))
				return false;
			if (Object.ReferenceEquals(s1, null) || Object.ReferenceEquals(s2, null))
				return true;
			if (s1.Length != s2.Length)
				return true;
			for (int i = 0; i < s1.Length; ++i)
				if (s1[i] != s2[i])
					return true;
			return false;
		}

		// Commented because of ambiguous conversions when comparing with null like: MutableString s; if (s == null) ...
		///// <summary>
		///// Operator !=
		///// </summary>
		///// <param name="s1">first string</param>
		///// <param name="s2">second string</param>
		///// <returns>return true if first string not equals second string</returns>
		//public static bool operator !=(string s2, MutableString s1)
		//{
		//	if (s1.Length != s2.Length) return true;
		//	for (int i = 0; i < s1.Length; ++i) if (s1[i] != s2[i]) return true;
		//	return false;
		//}
		///// <summary>
		///// Operator ==
		///// </summary>
		///// <param name="s1">first string</param>
		///// <param name="s2">second string</param>
		///// <returns>return true if first string equals second string</returns>
		//public static bool operator ==(string s2, MutableString s1)
		//{
		//	if (s1.Length != s2.Length) return false;
		//	for (int i = 0; i < s1.Length; ++i) if (s1[i] != s2[i]) return false;
		//	return true;
		//}
		///// <summary>
		///// Operator !=
		///// </summary>
		///// <param name="s1">first string</param>
		///// <param name="s2">second string</param>
		///// <returns>return true if first string not equals second string</returns>
		//public static bool operator != (MutableString s1, string s2)
		//{

		//	if (s1.Length != s2.Length) return true;
		//	for (int i = 0; i < s1.Length; ++i) if (s1[i] != s2[i]) return true;
		//	return false;
		//}
		///// <summary>
		///// Operator ==
		///// </summary>
		///// <param name="s1">first string</param>
		///// <param name="s2">second string</param>
		///// <returns>return true if first string equals second string</returns>
		//public static bool operator ==(MutableString s1, string s2)
		//{
		//	if (s1.Length != s2.Length) return false;
		//	for (int i = 0; i < s1.Length; ++i) if (s1[i] != s2[i]) return false;
		//	return true;
		//}

		/// <summary>
		/// Get data for serialization
		/// </summary>
		/// <param name="info">info</param>
		/// <param name="context">context</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			MemoryStream ms = new MemoryStream();
			unsafe
			{
				fixed (void* ptr = _data)
				{
					byte* buffer = (byte*)ptr;
					for (int i = 0; i < _length * 2; ++i)
					{
						ms.WriteByte(buffer[i]);
					}
				}
			}
			info.AddValue("d", ms.ToArray(), typeof(Byte[]));
		}



		/// <summary>
		/// Create instance of this class by serialization
		/// </summary>
		/// <param name="info">info</param>
		/// <param name="context">context</param>
		public MutableString(SerializationInfo info, StreamingContext context)
		{
			Byte[] buffer = (Byte[])info.GetValue("d", typeof(Byte[]));
			_length = buffer.Length >> 1;
			_data = new Char[_length];
			Expand(_length);
			MemoryUtils.Copy(buffer, 0, _data, 0, buffer.Length);
			ResetHashCode();
		}

		/// <summary>
		/// Transforms this string to lower case
		/// </summary>
		/// <returns>this string in lower case</returns>
		IMutableString IMutableString.ToLowerCase()
		{
			return ToLowerCase();
		}

		/// <summary>
		/// Transforms this string to lower case
		/// </summary>
		/// <returns>this string in lower case</returns>
		public MutableString ToLowerCase()
		{
			for (int i = 0; i < Length; ++i)
			{
				this[i] = char.ToLower(this[i]);
			}
			return this;
		}

		/// <summary>
		/// Transforms this string to upper case
		/// </summary>
		/// <returns>this string in upper case</returns>
		IMutableString IMutableString.ToUpperCase()
		{
			return ToUpperCase();
		}

		/// <summary>
		/// Transforms this string to upper case
		/// </summary>
		/// <returns>this string in upper case</returns>
		public MutableString ToUpperCase()
		{
			for (int i = 0; i < Length; ++i)
			{
				this[i] = char.ToUpper(this[i]);
			}
			return this;
		}
	}
}
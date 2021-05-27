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
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Binary-representation of ASCII-string. Throws exceptions if you try to load to it non-ASCII characters.
	/// </summary>
	public class BinaryAsciiString : IBinaryAsciiString
	{

		private static readonly char[] HexDigitsUpper = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
		private static readonly char[] HexDigitsLower = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };


		private long[] data;
		private Int32 hashCode;
		private Int32 lastByte;
		private static IBinaryArrayOptimization[] _optimizations = new IBinaryArrayOptimization[]
		{
			new BinaryArrayOptimization0(),
			new BinaryArrayOptimization1(),
			new BinaryArrayOptimization2(),
			new BinaryArrayOptimization3(),
			new BinaryArrayOptimization4(),
			new BinaryArrayOptimization5(),
			new BinaryArrayOptimization6(),
			new BinaryArrayOptimization7(),
			new BinaryArrayOptimization8(),
			new BinaryArrayOptimizationTotal()
		};



		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public BinaryAsciiString(Int32 capacity)
		{
			lastByte = 0;
			data = new long[Math.Max((capacity + 7) >> 3, 0x10)];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		public BinaryAsciiString() : this(0x10) { }


		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(IReadOnlyString str) : this(str.Length) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(String str) : this(str.Length) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(StringBuilder str) : this(str.Length) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(MutableString str) : this(str.Length) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The char value.</param>
		public BinaryAsciiString(Char c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The DateTime value.</param>
		public BinaryAsciiString(DateTime c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The HdDateTime value.</param>
		public BinaryAsciiString(HdDateTime c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The TimeSpan value.</param>
		public BinaryAsciiString(TimeSpan c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The HdTimeSpan value.</param>
		public BinaryAsciiString(HdTimeSpan c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The double value.</param>
		public BinaryAsciiString(double c) : this(0x10) { Assign(c); }
		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The float value.</param>
		public BinaryAsciiString(float c) : this(0x10) { Assign(c); }
		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The float value.</param>
		public BinaryAsciiString(bool c) : this(0x10) { Assign(c); }


		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The decimal value.</param>
		public BinaryAsciiString(decimal c) : this(0x10) { Assign(c); }


		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="c">The object value.</param>
		public BinaryAsciiString(object c) : this(0x10) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string in form of char array</param>
		public BinaryAsciiString(Char[] str) : this(0x10) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string in form of char array</param>
		public BinaryAsciiString(Span<char> str) : this(0x10) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string in form of char array.</param>
		/// <param name="offset">Offset in source char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public BinaryAsciiString(Char[] str, Int32 offset, Int32 count) : this(0x10) { Assign(str, offset, count); }


		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class
		/// from specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Initial data</param>
		public BinaryAsciiString(IBinaryConvertibleReadOnly binaryArray)
					: this(0x10)
		{
			Assign(binaryArray);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class
		/// from specified <see cref="UUID"/>, using specified <see cref="UUIDPrintFormat"/>.
		/// </summary>
		/// <param name="uuid"></param>
		/// <param name="format"></param>
		public BinaryAsciiString(UUID uuid, UUIDPrintFormat format = UUIDPrintFormat.UpperCase)
					: this(0x10)
		{
			Assign(uuid, format);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="str">The string in form of pointer to char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public unsafe BinaryAsciiString(Char* str, Int32 count) : this(0x10) { Assign(str, count); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(Int32 capacity, String str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(Int32 capacity, IReadOnlyString str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(Int32 capacity, StringBuilder str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string.</param>
		public BinaryAsciiString(Int32 capacity, MutableString str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString" /> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="c">The char value.</param>
		public BinaryAsciiString(Int32 capacity, Char c) : this(capacity) { Assign(c); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="i">The integer value.</param>
		public BinaryAsciiString(Int32 capacity, Int16 i) : this(capacity) { Assign(i); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="i">The integer value.</param>
		public BinaryAsciiString(Int32 capacity, Int32 i) : this(capacity) { Assign(i); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="i">The integer value.</param>
		public BinaryAsciiString(Int32 capacity, Int64 i) : this(capacity) { Assign(i); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string in form of char array</param>
		public BinaryAsciiString(Int32 capacity, Char[] str) : this(capacity) { Assign(str); }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string in form of char array.</param>
		/// <param name="offset">Offset in source char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public BinaryAsciiString(Int32 capacity, Char[] str, Int32 offset, Int32 count)
					: this(capacity)
		{
			Assign(str, offset, count);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryAsciiString"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="str">The string in form of pointer to char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		public unsafe BinaryAsciiString(Int32 capacity, Char* str, Int32 count) : this(capacity) { Assign(str, count); }




		private bool IsAscii(char c)
		{
			return (c < 128);
		}

		/// <summary>
		/// Assign Binary String by object. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="obj">Object to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Object obj)
		{
			Clear();
			return Append(obj);
		}

		/// <summary>
		/// Append object to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="obj">Object to append</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Append(Object obj)
		{
			String s = obj.ToString();
			return Append(s);
		}


		/// <summary>
		/// Assign Binary String by string. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="obj">String to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(String str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Append string to Binary String Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="obj">String to append</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Append(String str)
		{
			int errorCode = 0;
			for (int i = 0; i < str.Length; ++i) errorCode |= str[i];
			if (errorCode > 127 || errorCode < 0) throw new NotSupportedException("We support only ASCII-character appending");
			Expand(((lastByte + str.Length) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < str.Length; ++i) *(bytePtr + i) = (byte)str[i];
					lastByte += str.Length;
				}
			}
			return this;
		}



		/// <summary>
		/// Assign Binary String by UTF8 array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF8 array to assign</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AssignUTF8(byte* str, int count)
		{
			Clear();
			return AppendUTF8(str, count);
		}

		/// <summary>
		/// Append UTF8 array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF8 array to append</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AppendUTF8(byte* str, int count)
		{

			if (Encoding.UTF8.GetCharCount(str, count) != count) throw new NotSupportedException("We support only ASCII-character appending");
			return Append(str, count);
		}

		/// <summary>
		/// Append array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		private unsafe BinaryAsciiString Append(byte* buffer, int size)
		{
			if (size == 0)
				return this;
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + 7 + size) >> 3));
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < size; ++i) *(bytePtr + i) = *(buffer + i);
					lastByte += size;
				}
			}
			return this;
		}

		/// <summary>
		/// Append byte string to BinaryAsciiString.
		/// </summary>
		/// <param name="bytes">bytes to append</param>
		/// <param name="offset">destination offset</param>
		/// <param name="count">bytes count to append</param>
		/// <returns></returns>
		public BinaryAsciiString Assign(Byte[] bytes, int offset, int count)
		{
			Clear();
			return Append(bytes, offset, count);
		}

		/// <summary>
		/// Append byte string to BinaryAsciiString.
		/// </summary>
		/// <param name="bytes">bytes to append</param>
		/// <param name="offset">destination offset</param>
		/// <param name="count">bytes count to append</param>
		/// <returns></returns>
		public BinaryAsciiString Append(Byte[] bytes, int offset, int count)
		{
			Expand(((lastByte + count) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count; ++i) *(bytePtr + i) = bytes[offset + i];
					lastByte += count;
				}
			}
			return this;
		}


		/// <summary>
		/// Assign Binary String by UTF16 array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16 array to assign</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AssignUTF16(byte* str, int count)
		{
			Clear();
			return AppendUTF16(str, count);
		}


		/// <summary>
		/// Append UTF16 array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16 array to append</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AppendUTF16(byte* str, int count)
		{

			if (Encoding.Unicode.GetCharCount(str, count) * 2 != count) throw new NotSupportedException("We support only ASCII-character appending");
			Expand(((lastByte + count) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count / 2; i++) *(bytePtr + i) = *(str + i + i);
					lastByte += count / 2;
				}
			}
			return this;
		}


		/// <summary>
		/// Assign Binary String by UTF8 array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16 array to assign</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AssignUTF8(sbyte* str, int count)
		{
			Clear();
			return AppendUTF8(str, count);
		}


		/// <summary>
		/// Append UTF8 array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF8 array to append</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AppendUTF8(sbyte* str, int count)
		{
			if (Encoding.UTF8.GetCharCount((byte*)str, count) != count) throw new NotSupportedException("We support only ASCII-character appending");
			return Append((byte*)str, count);
		}


		/// <summary>
		/// Assign Binary String by UTF16 array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16 array to assign</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AssignUTF16(sbyte* str, int count)
		{
			Clear();
			return AppendUTF16(str, count);
		}

		/// <summary>
		/// Append UTF16 array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16 array to append</param>
		/// <param name="count">Bytes count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AppendUTF16(sbyte* str, int count)
		{

			if (Encoding.Unicode.GetCharCount((byte*)str, count) * 2 != count) throw new NotSupportedException("We support only ASCII-character appending");
			Expand(((lastByte + count) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count / 2; i++) *(bytePtr + i) = (byte)*(str + i + i);
					lastByte += count / 2;
				}
			}
			return this;
		}

		/// <summary>
		/// Assign Binary String by char array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Char array to assign</param>
		/// <param name="count">Char count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString Assign(char* str, int count)
		{
			Clear();
			return Append(str, count);
		}

		/// <summary>
		/// Append char array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Char array to append</param>
		/// <param name="count">Char count</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString Append(char* str, int count)
		{
			int errorCode = 0;
			for (int i = 0; i < count; ++i) errorCode |= *(str + i);
			if (errorCode > 127 || errorCode < 0) throw new NotSupportedException("We support only ASCII-character appending");
			Expand(((lastByte + count) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count; i++) *(bytePtr + i) = (byte)*(str + i);
					lastByte += count;
				}
			}
			return this;
		}

		/// <summary>
		/// Assign Binary String by char. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Char to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(char str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Append char to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Char to append</param>
		/// <returns>This string after this operation</returns>

		public unsafe BinaryAsciiString Append(char str)
		{
			if (!IsAscii(str)) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			if ((lastByte & 7) == 0)
			{
				Expand(((lastByte + 7) >> 3) + 1);
			}
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)ptr;
					*(bytePtr + lastByte) = (byte)str;
				}
			}
			lastByte++;
			return this;
		}


		void AppendDateTimeInts(long item, bool isMilliSeconds = false)
		{
			if (isMilliSeconds && item < 100) InternalAppend('0');
			if (item < 10) InternalAppend('0');
			Append(item);
		}

		/// <summary>
		/// Assign Binary String by DateTime. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">DateTime to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(DateTime str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Appends the specified <see cref="DateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="DateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		public BinaryAsciiString Append(DateTime item)
		{
			Expand(((lastByte + 33) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			AppendDateTimeInts(item.Month);
			InternalAppend('/');
			AppendDateTimeInts(item.Day);
			InternalAppend('/');
			Append(item.Year);
			InternalAppend(' ');
			AppendDateTimeInts(item.Hour);
			InternalAppend(':');
			AppendDateTimeInts(item.Minute);
			InternalAppend(':');
			AppendDateTimeInts(item.Second);
			InternalAppend('.');
			AppendDateTimeInts(item.Millisecond, true);
			return this;
		}

		/// <summary>
		/// Assign Binary String by Timespan. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Timespan to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(TimeSpan str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Appends the specified <see cref="TimeSpan"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="TimeSpan"/> to append.</param>
		/// <returns>Self instance</returns>
		public BinaryAsciiString Append(TimeSpan item)
		{
			Expand(((lastByte + 33) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			Append(item.Days);
			InternalAppend('.');
			AppendDateTimeInts(item.Hours);
			InternalAppend(':');
			AppendDateTimeInts(item.Minutes);
			InternalAppend(':');
			AppendDateTimeInts(item.Seconds);
			InternalAppend('.');
			AppendDateTimeInts(item.Milliseconds, true);
			return this;
		}

		/// <summary>
		/// Assign Binary String by HdDateTime. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">HdDateTime to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(HdDateTime str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Appends the specified <see cref="DateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="DateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		public BinaryAsciiString Append(HdDateTime item)
		{
			Expand(((lastByte + 33) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			Append(item.Timestamp);
			InternalAppend('.');
			AppendDateTimeInts(item.TimestampModulo);
			return this;
		}

		

		/// <summary>
		/// Assign Binary String by Decimal. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Decimal to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Decimal str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Append Decimal to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Decimal to append</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Append(Decimal str)
		{
			return Append(str.ToString());
		}



		/// <summary>
		/// Assign Binary String by Double. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Double to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(float str)
		{
			Clear();
			return Append(str);
		}


		/// <summary>
		/// Append double to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Double to append</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Append(float str)
		{
			return Append(str.ToString());
		}




		/// <summary>
		/// Assign Binary String by Double. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Double to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Double str)
		{
			Clear();
			return Append(str);
		}


		/// <summary>
		/// Append double to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Double to append</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Append(Double str)
		{
			return Append(str.ToString());
		}




		/// <summary>
		/// Appends the integer.
		/// </summary>
		/// <param name="i">The integer value.</param>
		private BinaryAsciiString AppendInteger(Int64 i)
		{
			hashCode = hashCode | 0x40000000;
			byte t;
			Expand(((lastByte + 21) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					if (i < 0)
					{
						*(bytePtr) = (byte)('-');
						bytePtr++;
						i = -i;
						lastByte++;
					}
					int oldLen = lastByte;
					Int32 index = 0, index2;
					lastByte += 21;
					do
					{
						*(bytePtr + index) = (byte)((i % 10) + '0');
						index++;
						i /= 10;
					} while (i != 0);
					index--;
					index2 = index / 2;
					for (Int32 j = 0, l = index; j <= index2; ++j, --l)
					{
						t = *(bytePtr + l);
						*(bytePtr + l) = *(bytePtr + j);
						*(bytePtr + j) = t;
					}
					lastByte = oldLen + index + 1;
				}
			}
			return this;
		}


		/// <summary>
		/// Assign Binary String by Int16. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Int16 to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Int16 str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public BinaryAsciiString Append(Int16 str)
		{
			return AppendInteger(str);
		}

		/// <summary>
		/// Assign Binary String by Int32. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Int16 to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Int32 str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public BinaryAsciiString Append(Int32 str)
		{
			return AppendInteger(str);
		}

		/// <summary>
		/// Assign Binary String by Int64. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Int16 to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Int64 str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		public BinaryAsciiString Append(Int64 str)
		{
			return AppendInteger(str);
		}

		/// <summary>
		/// Assign Binary String by UUID. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UUID to assign</param>
		/// <param name="format">Format of string-representation of UUID</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(UUID str, UUIDPrintFormat format = UUIDPrintFormat.UpperCase)
		{
			Clear();
			return Append(str, format);
		}






		/// <summary>
		/// Converts specified <see cref="UUID"/>, using specified <see cref="UUIDPrintFormat"/>
		/// into string and appends it. 
		/// </summary>
		/// <param name="uuid">Universally unique identifier.</param>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>Self instance.</returns>
		public BinaryAsciiString Append(UUID uuid, UUIDPrintFormat format = UUIDPrintFormat.UpperCase)
		{
			char[] hexDigits = (format == UUIDPrintFormat.LowerCase || format == UUIDPrintFormat.LowerCaseWithoutDashes)
				? HexDigitsLower : HexDigitsUpper;
			Expand(((lastByte + 36) >> 3) + 1);
			hashCode = hashCode | 0x40000000;
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)ptr + lastByte;
					switch (format)
					{
						case UUIDPrintFormat.LowerCase:
						case UUIDPrintFormat.UpperCase:
							ulong m = uuid.MSB;
							ulong l = uuid.LSB;
							for (int i = 15; i > 7; i -= 1, bytePtr++)
								*(bytePtr) = (byte)hexDigits[(int)(m >> (i * 4)) & 0xF];
							*(bytePtr) = (byte)'-';
							bytePtr++;
							for (int i = 7; i > 3; i -= 1, bytePtr++)
								*(bytePtr) = (byte)hexDigits[(int)(m >> (i * 4)) & 0xF];
							*(bytePtr) = (byte)'-';
							bytePtr++;
							for (int i = 3; i >= 0; i -= 1, bytePtr++)
								*(bytePtr) = (byte)hexDigits[(int)(m >> (i * 4)) & 0xF];
							*(bytePtr) = (byte)'-';
							bytePtr++;

							for (int i = 15; i > 11; i -= 1, bytePtr++)
								*(bytePtr) = (byte)hexDigits[(int)(l >> (i * 4)) & 0xF];
							*(bytePtr) = (byte)'-';
							bytePtr++;
							for (int i = 11; i >= 0; i -= 1, bytePtr++)
								*(bytePtr) = (byte)hexDigits[(int)(l >> (i * 4)) & 0xF];
							lastByte += 36;
							break;
						case UUIDPrintFormat.LowerCaseWithoutDashes:
						case UUIDPrintFormat.UpperCaseWithoutDashes:

							ulong m1 = uuid.MSB;
							ulong l1 = uuid.LSB;
							for (int i = 15; i >= 0; i -= 1, bytePtr++)
								*(bytePtr) = (byte)hexDigits[(int)(m1 >> (i * 4)) & 0xF];

							for (int i = 15; i >= 0; i -= 1, bytePtr++)
								*(bytePtr) = (byte)hexDigits[(int)(l1 >> (i * 4)) & 0xF];

							lastByte += 32;
							break;
					}
				}
			}

			return this;
		}

		/// <summary>
		/// Assign Binary String by IReadOnlyString. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">IReadOnlyString to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(IReadOnlyString str)
		{
			Clear();
			if (str is BinaryAsciiString)
			{
				BinaryAsciiString str1 = (BinaryAsciiString)str;
				Array.Resize(ref data, str1.data.Length);
				hashCode = str1.hashCode;
				lastByte = str1.lastByte;
				int a = (lastByte + 7) >> 3;
				_optimizations[9 + ((a - 9) & (a - 9) >> 31)].CopyTo(str1.data, data, a);
			}
			else Append(str);
			return this;
		}

		/// <summary>
		/// Appends the specified string. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">The string.</param>
		public BinaryAsciiString Append(IReadOnlyString str)
		{
			int errorCode = 0;
			for (int i = 0; i < str.Length; ++i) errorCode |= str[i];
			if (errorCode > 127 || errorCode < 0) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + str.Length) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < str.Length; ++i) *(bytePtr + i) = (byte)str[i];
					lastByte += str.Length;
				}
			}
			return this;
		}

		/// <summary>
		/// Assign Binary String by StringBuilder. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">StringBuilder to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(StringBuilder str)
		{
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Appends the specified string. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">The string.</param>
		public BinaryAsciiString Append(StringBuilder str)
		{
			int errorCode = 0;
			for (int i = 0; i < str.Length; ++i) errorCode |= str[i];
			if (errorCode > 127 || errorCode < 0) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + str.Length) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < str.Length; ++i) *(bytePtr + i) = (byte)str[i];
					lastByte += str.Length;
				}
			}
			return this;
		}


		/// <summary>
		/// Assign Binary String by UTF8-array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF8-array to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AssignUTF8(Byte[] str)
		{
			Clear();
			return AppendUTF8(str);
		}


		/// <summary>
		/// Append UTF8-array to Binary String . Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF8-array to append</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AppendUTF8(Byte[] str)
		{

			if (Encoding.UTF8.GetCharCount(str) != str.Length) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + str.Length) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < str.Length; ++i) *(bytePtr + i) = str[i];
					lastByte += str.Length;
				}
			}
			return this;
		}

		/// <summary>
		/// Fast append long to BinaryAsciiString
		/// </summary>
		/// <param name="i">Long to fast append.</param>
		/// <returns>Long to append.</returns>
		public BinaryAsciiString AppendFastHex(long i)
		{
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + 17) >> 3) + 1);
			AppendFastHexInternal(i);
			return this;
		}

		private void AppendFastHexInternal(long i)
		{
			long firstLong = 0;
			long secondLong = 0;
			firstLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)((i & 255L))] << 48;
			firstLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)((i >> 8) & 255L)] << 32;
			firstLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)((i >> 16) & 255L)] << 16;
			firstLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)((i >> 24) & 255L)];

			secondLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)(((i >> 32) & 255L))] << 48;
			secondLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)((i >> 40) & 255L)] << 32;
			secondLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)((i >> 48) & 255L)] << 16;
			secondLong |= (long)BinaryAsciiStringHelper.DigitPairs[(int)((i >> 56) & 255L)];


			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					*((long*)bytePtr) = secondLong;
					*((long*)(bytePtr + 8)) = firstLong;
				}
			}
		
			lastByte += 16;
		}

		/// <summary>
		/// Fast Append UUID to BinaryAsciiString.
		/// </summary>
		/// <param name="uuidReadOnly">UUID to append.</param>
		/// <returns>This string after append.</returns>
		public BinaryAsciiString AppendFastHex(UUID uuidReadOnly)
		{
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + 33) >> 3) + 1);
			AppendFastHexInternal((long)uuidReadOnly.LSB);
			AppendFastHexInternal((long)uuidReadOnly.MSB);
			return this;
		}


		/// <summary>
		/// Assign Binary String by UTF16-array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16-array to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AssignUTF16(Byte[] str)
		{
			Clear();
			return AppendUTF16(str);
		}

		/// <summary>
		/// Append UTF16-array to Binary String . Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16-array to append</param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AppendUTF16(Byte[] str)
		{

			if (Encoding.Unicode.GetCharCount(str) * 2 != str.Length) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + str.Length) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					int len = str.Length;
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < len / 2; i++) *(bytePtr + i) = str[i + i];
					lastByte += len / 2;
				}
			}
			return this;
		}


		/// <summary>
		/// Assign Binary String by UTF8-array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF18-array to assign</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AssignUTF8(Byte[] str, int offset, int count)
		{
			Clear();
			return AppendUTF8(str, offset, count);
		}


		/// <summary>
		/// Append UTF8-array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF8-array to append</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AppendUTF8(Byte[] str, int offset, int count)
		{

			if (Encoding.UTF8.GetCharCount(str, offset, count) != count) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + count) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count; ++i) *(bytePtr + i) = str[offset + i];
					lastByte += count;
				}
			}
			return this;
		}

		/// <summary>
		/// Assign Binary String by UTF16-array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16-array to assign</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AssignUTF16(Byte[] str, int offset, int count)
		{
			Clear();
			return AppendUTF16(str, offset, count);
		}

		/// <summary>
		/// Append UTF16-array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16-array to append</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AppendUTF16(Byte[] str, int offset, int count)
		{

			if (Encoding.Unicode.GetCharCount(str, offset, count) * 2 != count) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + count) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count / 2; i++) *(bytePtr + i) = str[offset + i + i];
					lastByte += count / 2;
				}
			}
			return this;
		}

		/// <summary>
		/// Assign Binary String by UTF8-array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16-array to assign</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AssignUTF8(SByte[] str, int offset, int count)
		{
			Clear();
			return AppendUTF8(str, offset, count);
		}

		/// <summary>
		/// Assign Binary String by UTF16-array. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16-array to assign</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AssignUTF16(SByte[] str, int offset, int count)
		{
			Clear();
			return AppendUTF16(str, offset, count);
		}


		/// <summary>
		/// Append UTF8-array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF8-array to append</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString AppendUTF8(SByte[] str, int offset, int count)
		{
			unsafe
			{
				fixed (sbyte* ptr = str)
				{
					if (Encoding.UTF8.GetCharCount((byte*)(ptr + offset), count) != count) throw new NotSupportedException("We support only ASCII-character appending");
				}
			}
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + count) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count; ++i) *(bytePtr + i) = (byte)str[offset + i];
					lastByte += count;
				}
			}
			return this;
		}

		/// <summary>
		/// Append UTF16-array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">UTF16-array to append</param>
		/// <param name="offset">Source offset in bytes</param>
		/// <param name="count"> Count of bytes </param>
		/// <returns>This string after this operation</returns>
		public unsafe BinaryAsciiString AppendUTF16(SByte[] str, int offset, int count)
		{
			unsafe
			{
				fixed (sbyte* ptr = str)
				{
					if (Encoding.Unicode.GetCharCount((byte*)(ptr + offset), count) * 2 != count) throw new NotSupportedException("We support only ASCII-character appending");
				}
			}
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + count) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count / 2; i++) *(bytePtr + i) = (byte)str[offset + i];
					lastByte += count / 2;
				}
			}
			return this;
		}



		/// <summary>
		/// Assign Binary String by char. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">char array to assign</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Char[] str)
		{
			Clear();
			return Append(str, 0, str.Length);
		}

		/// <summary>
		/// Append char array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Char array to append</param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Append(Char[] str)
		{
			return Append(str, 0, str.Length);
		}



		/// <summary>
		/// Assign Binary String by char. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">char array to assign</param>
		/// <param name="offset">Source offset in chars</param>
		/// <param name="count"> Count of chars </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Assign(Char[] str, int offset, int count)
		{
			Clear();
			return Append(str, offset, count);
		}

		/// <summary>
		/// Append char array to Binary String. Throws exception if you try to load to it non-ASCII characters.
		/// </summary>
		/// <param name="str">Char array to append</param>
		/// <param name="offset">Source offset in chars</param>
		/// <param name="count"> Count of chars </param>
		/// <returns>This string after this operation</returns>
		public BinaryAsciiString Append(Char[] str, int offset, int count)
		{
			int errorCode = 0;
			for (int i = 0; i < count; ++i) errorCode |= str[offset + i];
			if (errorCode > 127 || errorCode < 0) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + count) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < count; ++i) *(bytePtr + i) = (byte)str[offset + i];
					lastByte += count;
				}
			}
			return this;
		}







		/// <summary>
		/// Length of this string.
		/// </summary>
		public int Length
		{
			get
			{
				return lastByte;
			}
		}

		/// <summary>
		/// Return true if this string is empty
		/// </summary>
		public bool Empty
		{
			get
			{
				return Length == 0;
			}
		}

		/// <summary>
		/// Return Byte length of this string in UTF-8
		/// </summary>
		public int UTF8ByteLength
		{
			get
			{
				return lastByte;
			}
		}



		/// <summary>
		/// Get char by index.
		/// </summary>
		/// <param name="index">Index of char</param>
		/// <returns>Char by index</returns>
		public char this[int index]
		{
			get
			{
				return (char)ToByte(index);
			}
			set
			{
				if (!IsAscii(value)) throw new NotSupportedException("We support only ASCII - character appending");
				hashCode = hashCode | 0x40000000;
				SetLongByByte(ref data[index >> 3], (index & 7) << 3, (byte)value);
			}
		}

		unsafe BinaryAsciiString InternalAppend(byte* bytePtr, char buffer)
		{
			*(bytePtr + lastByte) = (byte)buffer;
			lastByte++;
			return this;
		}

		BinaryAsciiString InternalAppend(char buffer)
		{
			return InternalAppend((byte)buffer);
		}





		BinaryAsciiString InternalAppend(byte buffer)
		{
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)ptr;
					*(bytePtr + lastByte) = buffer;
				}
			}
			lastByte++;
			return this;
		}


		/// <summary>
		/// Append a byte
		/// </summary>
		/// <param name="buffer">byte</param>
		/// <returns>new binary array</returns>
		public BinaryAsciiString Append(byte buffer)
		{
			hashCode = hashCode | 0x40000000;
			if ((lastByte & 7) == 0)
			{
				Expand(((lastByte + 7) >> 3) + 1);
			}
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)ptr;
					*(bytePtr + lastByte) = buffer;
				}
			}
			lastByte++;
			return this;
		}


		/// <summary>
		/// Convert BinaryArray to Byte
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Byte</returns>
		private Byte ToByte(Int32 offset)
		{
			if (offset + sizeof(Byte) > lastByte)
				throw new ArgumentOutOfRangeException();
			return GetByte(data[(offset) >> 3], (offset & 7) << 3);
		}

		private void SetLongByByte(ref long destination, int index, byte src)
		{
			destination -= (((destination >> index) & 255) << index);
			destination += ((long)src << index);
		}

		private byte GetByte(long x, int index)
		{
			return (byte)((x >> index) & 255);
		}


		/// <summary>
		/// Expands the specified capacity.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public unsafe void Expand(Int32 capacity)
		{
			if (((lastByte + 7) >> 3) >= capacity)
				return;
			if (data == null || data.Length < capacity)
				Array.Resize(ref data, capacity);
		}

		/// <summary>
		/// Return substring of this string.
		/// </summary>
		/// <param name="start">Start index of substring</param>
		/// <param name="end">Incremented index of last element.</param>
		/// <returns>Substring of this string.</returns>
		public IReadOnlyString SubString(int start, int end)
		{
			BinaryAsciiString array = new BinaryAsciiString();
			for (int i = start; i < end; ++i) array.Append(ToByte(i));
			return array;
		}





		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public Boolean Equals(BinaryAsciiString another)
		{
			if (another == null)
				return false;
			if (lastByte != another.lastByte)
				return false;
			int a = (lastByte + 7) >> 3;
			return _optimizations[9 + ((a - 9) & (a - 9) >> 31)].Equals(data, another.data, a);
		}


		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public Boolean Equals(BinaryArray another)
		{
			if (another == null)
				return false;
			if (lastByte != another.lastByte)
				return false;
			int a = (lastByte + 7) >> 3;
			return _optimizations[9 + ((a - 9) & (a - 9) >> 31)].Equals(data, another.data, a);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public Boolean Equals(IBinaryConvertibleReadOnly another)
		{
			if (another == null)
				return false;
			if (lastByte != another.Count)
				return false;
			for (int i = 0; i < lastByte; i++)
				if (ToByte(i) != another[i])
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
		public bool Equals(IReadOnlyString another)
		{
			if (another == null) return false;
			if (another is BinaryArray) return this.Equals((BinaryArray)another);
			if (another is BinaryAsciiString) return this.Equals((BinaryAsciiString)another);
			long len = ((IReadOnlyString)this).Length;
			if (len != another.Length) return false;
			for (int i = 0; i < len; ++i) if (this[i] != another[i]) return false;
			return true;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public bool Equals(string another)
		{
			if (another == null) return false;
			long len = ((IReadOnlyString)this).Length;
			if (len != another.Length) return false;
			for (int i = 0; i < len; ++i) if (this[i] != another[i]) return false;
			return true;
		}


		/// <summary>
		/// Equals to object
		/// </summary>
		/// <param name="obj">object</param>
		/// <returns>true if equals</returns>
		public override Boolean Equals(Object obj)
		{
			if (obj is BinaryAsciiString) return Equals((BinaryAsciiString)obj);
			if (obj is BinaryArray) return Equals((BinaryArray)obj);
			if (obj is IReadOnlyString) return Equals((IReadOnlyString)obj);
			if (obj is String) Equals((String)obj);
			return false;
		}



		public override string ToString()
		{
			MutableString stringBuilder = new MutableString();
			for (int i = 0; i < Length; ++i) stringBuilder.Append(this[i]);
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Convert this string to Int64
		/// </summary>
		/// <returns>Converted string</returns>
		public long ToInt64()
		{
			Int64 result = 0;
			for (Int32 i = 0; i < Length; ++i)
			{
				result *= 10;
				result += this[i] - '0';
			}

			return result;
		}

		/// <summary>
		/// Convert string to char span.
		/// </summary>
		/// <param name="span">span to convert</param>
		public void ToCharArray(Span<char> span)
		{
			unsafe {
				fixed (long* longPtr = data) {
					byte* bytePtr = (byte*)longPtr;
					for (int i = 0; i < lastByte; ++i) span[i] = (char)*(bytePtr + i);
				}
			}
		}

		/// <summary>
		/// Convert string to UTF8 and store it into byte array.
		/// </summary>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <returns>Number of bytes written.</returns>
		public int ToUTF8(byte[] utf8)
		{
			if (lastByte == 0) return 0;
			for (int i = 0; i < lastByte; ++i) utf8[i] = (byte)this[i];
			return lastByte;
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte array.
		/// </summary>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <returns>Number of bytes written.</returns>
		public int ToUTF8(byte[] utf8, int offset)
		{
			if (lastByte == 0) return 0;
			for (int i = 0; i < lastByte; ++i) utf8[i + offset] = (byte)this[i];
			return lastByte;
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte buffer.
		/// </summary>
		/// <param name="utf8">Byte buffer to store utf8 string.</param>
		/// <returns>Number of bytes written.</returns>
		public unsafe int ToUTF8(byte* utf8)
		{
			if (lastByte == 0) return 0;
			for (int i = 0; i < lastByte; ++i) *(utf8 + i) = (byte)this[i];
			return lastByte;
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte buffer.
		/// </summary>
		/// <param name="utf8">Byte buffer to store utf8 string.</param>
		/// <param name="byteCount">Number of bytes in byte buffer.</param>
		/// <returns>Number of bytes written.</returns>
		public unsafe int ToUTF8(byte* utf8, int byteCount)
		{
			if (lastByte == 0) return 0;
			int len = Math.Min(lastByte, byteCount);
			for (int i = 0; i < len; ++i) *(utf8 + i) = (byte)this[i];
			return len;
		}

		/// <summary>
		/// Convert string to UTF8 and store into byte array.
		/// </summary>
		/// <param name="charIndex">Start index.</param>
		/// <param name="charCount">Count of char to convert.</param>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <param name="utf8Index">Offset in byte array.</param>
		/// <returns>Number of bytes written.</returns>
		public int ToUTF8(int charIndex, int charCount, byte[] utf8, int utf8Index)
		{
			if (lastByte == 0) return 0;
			for (int i = charIndex; i < charIndex + charCount; ++i) utf8[utf8Index + i - charIndex] = (byte)this[i];
			return charCount;
		}

		/// <summary>
		/// Assigns binary representation of this string to specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Output <see cref="BinaryArray"/>, containing binary representation of this string.</param>
		public void ToBinaryArray(IBinaryConvertibleReadWrite binaryArray)
		{
			binaryArray.Assign(this, true);
		}

		/// <summary>
		/// Returns binary representation of this string.
		/// </summary>
		/// <returns>New <see cref="BinaryArray"/> representation of this string.</returns>
		public IBinaryConvertibleReadOnly ToBinaryArray()
		{
			return new BinaryArray().Assign(this, true);
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
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public BinaryAsciiString Clone()
		{
			BinaryAsciiString result = new BinaryAsciiString();
			CopyTo(result);
			return result;
		}

		/// <summary>
		/// Copies to
		/// </summary>
		/// <param name="str">destination</param>
		public void CopyTo(BinaryAsciiString str)
		{
			Array.Resize(ref str.data, data.Length);
			str.hashCode = hashCode;
			str.lastByte = lastByte;
			int a = (lastByte + 7) >> 3;
			_optimizations[9 + ((a - 9) & (a - 9) >> 31)].CopyTo(data, str.data, a);
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		IMutableString IReadOnlyString.Clone()
		{
			return new MutableString().Assign(this);
		}


		/// <summary>
		/// Copy read-only string to mutable string.
		/// </summary>
		/// <param name="sourceIndex">Index of the source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destinationIndex">Index of the destination.</param>
		/// <param name="count">The count.</param>
		/// <param name="str"/>
		/// <exception cref="System.IndexOutOfRangeException"></exception>
		public void CopyTo(IMutableString str)
		{
			str.Clear();
			for (int i = 0; i < Length; ++i) str.Append(this[i]);
		}


		private void CalculateHashCode()
		{
			int a = (lastByte + 7) >> 3;
			hashCode = _optimizations[9 + ((a - 9) & (a - 9) >> 31)].GetHashCode(data, a);
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
			if ((hashCode & 0x40000000) > 0)
				CalculateHashCode();
			return hashCode;
		}


		/// <summary>
		/// Clears this instance.
		/// </summary>
		public BinaryAsciiString Clear()
		{
			if (data != null)
				Array.Clear(data, 0, data.Length);
			lastByte = 0;
			CalculateHashCode();
			return this;
		}

		/// <summary>
		/// Copy read-only string to char array 
		/// </summary>
		/// <param name="sourceIndex">Start index in source.</param>
		/// <param name="destination">Destination char array.</param>
		/// <param name="destinationIndex">Start index in destination.</param>
		/// <param name="count">Count of char to copy.</param>
		public void ToCharArray(int sourceIndex, char[] destination, int destinationIndex, int count)
		{
			for (int i = sourceIndex; i < sourceIndex + count; ++i) destination[destinationIndex + i - sourceIndex] = this[i];
		}

		/// <summary>
		/// Copies from
		/// </summary>
		/// <param name="str">destination</param>
		public BinaryAsciiString CopyFrom(IReadOnlyString str)
		{
			return Assign(str);
		}



		/// <summary>
		/// Append chars to BinaryAsciiString
		/// </summary>
		/// <param name="c">chars</param>
		/// <returns>this</returns>
		IBinaryAsciiString IBinaryAsciiString.Append(ReadOnlySpan<char> c)
		{
			return Append(c);
		}

		/// <summary>
		/// Assign BinaryAsciiString by chars
		/// </summary>
		/// <param name="c">Chars</param>
		/// <returns>this</returns>
		IBinaryAsciiString IBinaryAsciiString.Assign(ReadOnlySpan<char> c)
		{
			return Assign(c);
		}

		/// <summary>
		/// Append chars to BinaryAsciiString
		/// </summary>
		/// <param name="c">chars</param>
		/// <returns>this</returns>
		public BinaryAsciiString Append(ReadOnlySpan<char> c)
		{
			int errorCode = 0;
			for (int i = 0; i < c.Length; ++i) errorCode |= c[i];
			if (errorCode > 127 || errorCode < 0) throw new NotSupportedException("We support only ASCII-character appending");
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + c.Length) >> 3) + 1);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* bytePtr = (byte*)(ptr) + lastByte;
					for (int i = 0; i < c.Length; ++i) *(bytePtr + i) = (byte)c[i];
					lastByte += c.Length;
				}
			}
			return this;
		}

		/// <summary>
		/// Assign BinaryAsciiString by chars
		/// </summary>
		/// <param name="c">Chars</param>
		/// <returns>this</returns>
		public BinaryAsciiString Assign(ReadOnlySpan<char> c)
		{
			Clear();
			return Append(c);
		}


		IBinaryAsciiString IBinaryAsciiString.AppendFastHex(long i)
		{
			return AppendFastHex(i);
		}

		IBinaryAsciiString IBinaryAsciiString.AppendFastHex(UUID i)
		{
			return AppendFastHex(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(object item)
		{
			return Assign(item);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(object item)
		{
			return Append(item);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(IBinaryArrayReadOnly item)
		{
			return Assign(item);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(IBinaryArrayReadOnly item)
		{
			return Append(item);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(bool item)
		{
			return Append(item);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(IReadOnlyString cs)
		{
			return Append(cs);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(char c)
		{
			return Append(c);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(short i)
		{
			return Append(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(int i)
		{
			return Append(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(long i)
		{
			return Append(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(char[] str)
		{
			return Append(str);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(char[] str, int offset, int count)
		{
			return Append(str, offset, count);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(double i)
		{
			return Append(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(UUID uuid)
		{
			return Append(uuid);
		}

		IBinaryAsciiString IBinaryAsciiString.Append(UUID uuid, UUIDPrintFormat format)
		{
			return Append(uuid, format);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(bool item)
		{
			return Assign(item);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(IReadOnlyString cs)
		{
			return Assign(cs);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(char c)
		{
			return Assign(c);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(short i)
		{
			return Assign(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(double i)
		{
			return Assign(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(int i)
		{
			return Assign(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(long i)
		{
			return Assign(i);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(char[] str)
		{
			return Assign(str);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(char[] str, int offset, int count)
		{
			return Assign(str, offset, count);
		}


		IBinaryAsciiString IBinaryAsciiString.Assign(UUID uuid)
		{
			return Assign(uuid);
		}

		IBinaryAsciiString IBinaryAsciiString.Assign(UUID uuid, UUIDPrintFormat format)
		{
			return Assign(uuid, format);
		}

		IBinaryAsciiString IBinaryAsciiString.Clear()
		{
			return Clear();
		}

		IReadOnlyString IReadOnlyString.SubString(int start, int end)
		{
			return SubString(start, end);
		}

		/// <summary>
		/// Copies from
		/// </summary>
		/// <param name="str">destination</param>
		IBinaryAsciiString IBinaryAsciiString.CopyFrom(IReadOnlyString str)
		{
			return Assign(str);
		}

		/// <summary>
		/// Transforms this string to lower case
		/// </summary>
		/// <returns>this string in lower case</returns>
		IBinaryAsciiString IBinaryAsciiString.ToLowerCase()
		{
			return ToLowerCase();
		}

		/// <summary>
		/// Transforms this string to lower case
		/// </summary>
		/// <returns>this string in lower case</returns>
		public BinaryAsciiString ToLowerCase()
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
		IBinaryAsciiString IBinaryAsciiString.ToUpperCase()
		{
			return ToUpperCase();
		}

		/// <summary>
		/// Transforms this string to upper case
		/// </summary>
		/// <returns>this string in upper case</returns>
		public BinaryAsciiString ToUpperCase()
		{
			for (int i = 0; i < Length; ++i)
			{
				this[i] = char.ToUpper(this[i]);
			}
			return this;
		}
	}
}
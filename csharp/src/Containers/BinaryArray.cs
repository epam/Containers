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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Linq;
namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Defines special version of binary array.
	/// </summary>

	[Serializable]
	public class BinaryArray : IList<Byte>, IList<SByte>, IComparable, ICloneable, IComparable<BinaryArray>, IEnumerable<Byte>, IEnumerable, IEquatable<BinaryArray>, ICopyable<BinaryArray>, ISerializable, IBinaryConvertibleReadWrite
	{
		internal long[] data;
		internal Int32 hashCode;
		internal Int32 lastByte;
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

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryArray"/> class.
		/// </summary>
		public BinaryArray()
			: this(8)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryArray"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public BinaryArray(Int32 capacity)
		{
			lastByte = 0;
			data = new long[(capacity + 7) >> 3];
		}

		/// <summary>
		/// Initializes a new instance
		/// </summary>
		/// <param name="b">Binary Array</param>
		public BinaryArray(BinaryArray b)
		{
			Assign(b);
		}

		/// <summary>
		/// Initializes a new instance
		/// </summary>
		/// <param name="b">Binary Array</param>
		public BinaryArray(IBinaryArrayReadOnly b)
		{
			Assign(b);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryArray"/> class.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name = "size">The size of buffer </param>
		public unsafe BinaryArray(byte* buffer, int size)
			: this(size)
		{
			Assign(buffer, size);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryArray"/> class.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		public BinaryArray(byte buffer)
			: this(0x10)
		{
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryArray"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name = "size">The size of buffer </param>
		public unsafe BinaryArray(Int32 capacity, byte* buffer, int size)
			: this(capacity)
		{
			Assign(buffer, size);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, byte buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Boolean buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Char buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, DateTime buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Decimal buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Double buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Int16 buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Int32 buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Int64 buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, UInt16 buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="bytes">The buffer</param>
		public BinaryArray(ReadOnlySpan<byte> bytes)
		{
			Assign(bytes);
		}

		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray(ReadOnlySpan<sbyte> bytes)
		{
			Assign(bytes);
		}

		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray(ReadOnlySpan<char> bytes)
		{
			Assign(bytes);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, UInt32 buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, UInt64 buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, SByte buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref ="BinaryArray"/> class 
		/// </summary>
		/// <param name="capacity">The capacity</param>
		/// <param name="buffer">The buffer</param>
		public BinaryArray(Int32 capacity, Single buffer)
		{
			Expand((capacity + 7) >> 3);
			Assign(buffer);
		}

		/// <summary>
		/// Init a new instance of the BinaryArray
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		public BinaryArray(Byte[] bytes)
		{
			Assign(bytes);
		}

		/// <summary>
		/// Init a new instance of the Binary Array
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		public BinaryArray(Byte[] bytes, int offset, int count)
		{
			Assign(bytes, offset, count);
		}

		/// <summary>
		/// Init a new instance of the Binary Array
		/// </summary>
		/// <param name="bytes">array of Bytes</param>
		public BinaryArray(SByte[] bytes)
		{
			Assign(bytes);
		}

		/// <summary>
		/// Init a new instance of the Binary Array
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		public BinaryArray(SByte[] bytes, int offset, int count)
		{
			Assign(bytes, offset, count);
		}

		/// <summary>
		/// Init a new instance of Binary Array
		/// </summary>
		/// <param name="ptr">pointer to Byte</param>
		/// <param name="count">count</param>
		public unsafe BinaryArray(SByte* ptr, int count)
		{
			Assign(ptr, count);
		}

		/// <summary>
		/// Init a new instance of Binary Array
		/// </summary>
		/// <param name="enumerable">enumerable</param>
		public BinaryArray(IEnumerable<byte> enumerable)
		{
			Assign(enumerable);
		}

		/// <summary>
		/// Init a new instance of Binary Array
		/// </summary>
		/// <param name="enumerable">enumerable</param>
		public BinaryArray(IEnumerable<sbyte> enumerable)
		{
			Assign(enumerable);
		}

		/// <summary>
		/// Init a new instance of Binary Array
		/// </summary>
		/// <param name="str">mutable string</param>
		public BinaryArray(IReadOnlyString str, bool isASCII = false)
		{
			Assign(str, isASCII);
		}

		/// <summary>
		/// Init a new instance of Binary Array
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="isASCII">is it string ASCII</param>
		public BinaryArray(String str, bool isASCII = false)
		{
			Assign(str, isASCII);
		}

		/// <summary>
		/// Init a new instance of Binary Array
		/// </summary>
		/// <param name="uuid">UUID</param>
		public BinaryArray(UUID uuid)
		{
			Assign(uuid);
		}

		#endregion Constructors

		#region Append

		/// <summary>
		/// Append MutableString to this
		/// </summary>
		/// <param name="str">MutableString</param>
		/// <param name="isASCII">is str ASCII string or no</param>
		/// <returns>binary array</returns>
		public BinaryArray Append(IReadOnlyString str, bool isASCII = false)
		{
			hashCode = hashCode | 0x40000000;

			if (isASCII)
			{
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
			}
			else
			{
				Expand(((lastByte + (str.Length << 1)) >> 3) + 1);
				for (int i = 0; i < str.Length; i++)
					AppendCharInternal(str[i]);
			}
			return this;
		}

		/// <summary>
		/// Append MutableString to this
		/// </summary>
		/// <param name="str">MutableString</param>
		/// <returns>binary array</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(IReadOnlyString str, bool isASCII)
		{
			return Append(str, true);
		}

		internal void InternalAppend(byte x)
		{
			if ((lastByte & 7) == 0)
			{
				SetLongByByte(ref data[((lastByte + 7) >> 3)], 0, x);
				lastByte++;
			}
			else
			{
				SetLongByByte(ref data[((lastByte + 7) >> 3) - 1], ((lastByte & 7) << 3), x);
				lastByte++;
			}
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		public BinaryArray Append(Byte[] bytes)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + bytes.Length + 7) >> 3);
			for (int i = 0; i < bytes.Length; ++i)
				InternalAppend(bytes[i]);
			return this;
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Byte[] bytes)
		{
			return Append(bytes);
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		public BinaryArray Append(Byte[] bytes, int offset, int count)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + count + 7) >> 3);
			for (int i = offset; i < offset + count; ++i)
				InternalAppend(bytes[i]);
			return this;
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Byte[] bytes, int offset, int count)
		{
			return Append(bytes, offset, count);
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		public BinaryArray Append(SByte[] bytes)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + bytes.Length + 7) >> 3);
			for (int i = 0; i < bytes.Length; ++i)
				InternalAppend((byte)bytes[i]);
			return this;
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(SByte[] bytes)
		{
			return Append(bytes);
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		public BinaryArray Append(SByte[] bytes, int offset, int count)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + count + 7) >> 3);
			for (int i = offset; i < offset + count; ++i)
				InternalAppend((byte)bytes[i]);
			return this;
		}

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(SByte[] bytes, int offset, int count)
		{
			return Append(bytes, offset, count);
		}

		/// <summary>
		/// Append binary array to this
		/// </summary>
		/// <param name="str">Binary array</param>
		/// <returns>new binary array</returns>
		public BinaryArray Append(IBinaryConvertibleReadOnly str)
		{
			if (str == null)
				return this;
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + str.Count + 7) >> 3);

			for (int i = 0; i < str.Count; ++i)
				InternalAppend(str[i]);
			return this;
		}

		/// <summary>
		/// Append binary array to this
		/// </summary>
		/// <param name="str">Binary array</param>
		/// <returns>new binary array</returns>
		public BinaryArray Append(BinaryArray str)
		{
			if (str == null)
				return this;
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + str.Count + 7) >> 3);

			for (int i = 0; i < str.Count; ++i)
				InternalAppend(str[i]);
			return this;
		}

		/// <summary>
		/// Append binary array to this
		/// </summary>
		/// <param name="str">Binary array</param>
		/// <returns>new binary array</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(IBinaryConvertibleReadOnly str)
		{
			return Append(str);
		}

		/// <summary>
		/// Append array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		public unsafe BinaryArray Append(byte* buffer, int size)
		{
			if (size == 0)
				return this;
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + size + 7) >> 3));
			for (int i = 0; i < size; ++i)
			{
				InternalAppend(buffer[i]);
			}
			return this;
		}

		/// <summary>
		/// Append array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		unsafe IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(byte* buffer, int size)
		{
			return Append(buffer, size);
		}

		/// <summary>
		/// Append array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		public unsafe BinaryArray Append(sbyte* buffer, int size)
		{
			if (size == 0)
				return this;
			hashCode = hashCode | 0x40000000;
			Expand(((lastByte + 7 + size) >> 3));
			for (int i = 0; i < size; ++i)
			{
				InternalAppend((byte)buffer[i]);
			}

			return this;
		}

		/// <summary>
		/// Append array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		unsafe IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(sbyte* buffer, int size)
		{
			return Append(buffer, size);
		}

		/// <summary>
		/// Append a byte
		/// </summary>
		/// <param name="buffer">byte</param>
		/// <returns>new binary array</returns>
		public BinaryArray Append(byte buffer)
		{
			hashCode = hashCode | 0x40000000;
			if ((lastByte & 7) == 0)
			{
				Expand(((lastByte + 7) >> 3) + 1);

				SetLongByByte(ref data[((lastByte + 7) >> 3)], 0, buffer);
				lastByte++;
			}
			else
			{
				SetLongByByte(ref data[((lastByte + 7) >> 3) - 1], ((lastByte & 7) << 3), buffer);
				lastByte++;
			}

			return this;
		}

		/// <summary>
		/// Append a byte
		/// </summary>
		/// <param name="buffer">byte</param>
		/// <returns>new binary array</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(byte buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Boolean buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 8) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Boolean* d = (Boolean*)(p1 + lastByte);
					*(d) = buffer;
					lastByte++;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Boolean buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(char buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 9) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					char* d = (char*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 2;
				}
			}

			return this;
		}

		private BinaryArray AppendCharInternal(char buffer)
		{
			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					char* d = (char*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 2;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(char buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(DateTime buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 15) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					DateTime* d = (DateTime*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 8;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(DateTime buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Decimal buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 23) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Decimal* d = (Decimal*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 16;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Decimal buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Double buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 15) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Double* d = (Double*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 8;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Double buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Single buffer)
		{
			Expand((lastByte + 11) >> 3);
			hashCode = hashCode | 0x40000000;

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Single* d = (Single*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 4;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Single buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append UUID to this
		/// </summary>
		/// <param name="item">UUID</param>
		/// <returns>binary array</returns>
		public BinaryArray Append(UUID item)
		{
			Append(item.MSB);
			return Append(item.LSB);
		}

		/// <summary>
		/// Append UUID to this
		/// </summary>
		/// <param name="item">UUID</param>
		/// <returns>binary array</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(UUID item)
		{
			return Append(item);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Guid buffer)
		{
			Expand((lastByte + 23) >> 3);
			hashCode = hashCode | 0x40000000;

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Guid* d = (Guid*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 16;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Guid buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Int16 buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 9) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Int16* d = (Int16*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 2;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Int16 buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Int32 buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 11) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Int32* d = (Int32*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 4;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Int32 buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(Int64 buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 15) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Int64* d = (Int64*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 8;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(Int64 buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(UInt16 buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 9) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					UInt16* d = (UInt16*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 2;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(UInt16 buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(UInt32 buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 11) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					UInt32* d = (UInt32*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 4;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(UInt32 buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(UInt64 buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 15) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					UInt64* d = (UInt64*)(p1 + lastByte);
					*(d) = buffer;
					lastByte += 8;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(UInt64 buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		public BinaryArray Append(SByte buffer)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 8) >> 3);

			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					SByte* d = (SByte*)(p1 + lastByte);
					*(d) = buffer;
					lastByte++;
				}
			}

			return this;
		}

		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(SByte buffer)
		{
			return Append(buffer);
		}

		/// <summary>
		/// Append string to this BinaryArray
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>this BinaryArray</returns>
		/// <param name="isASCII">is it string ASCII</param>
		public BinaryArray Append(string str, bool isASCII = false)
		{
			hashCode = hashCode | 0x40000000;
			if (isASCII == false)
			{
				Expand(((lastByte + (str.Length << 1)) >> 3) + 1);
				for (int i = 0; i < str.Length; i++)
					AppendCharInternal(str[i]);
			}
			else
			{
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
			}
			return this;
		}

		/// <summary>
		/// Append string to this BinaryArray
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>this BinaryArray</returns>
		/// <param name="isASCII">is it string ASCII</param>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(string str, bool isASCII)
		{
			return Append(str, isASCII);
		}

		/// <summary>
		/// Append Enumerable of byte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of byte</param>
		/// <returns>this</returns>
		public BinaryArray Append(IEnumerable<byte> enumerable)
		{
			hashCode = hashCode | 0x40000000;
			foreach (byte item in enumerable)
				Append(item);
			return this;
		}

		/// <summary>
		/// Append Enumerable of byte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of byte</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(IEnumerable<byte> enumerable)
		{
			return Append(enumerable);
		}

		/// <summary>
		/// Append Enumerable of sbyte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of sbyte</param>
		/// <returns>this</returns>
		public BinaryArray Append(IEnumerable<sbyte> enumerable)
		{
			hashCode = hashCode | 0x40000000;
			foreach (sbyte item in enumerable)
				Append(item);
			return this;
		}

		/// <summary>
		/// Append Enumerable of sbyte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of sbyte</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(IEnumerable<sbyte> enumerable)
		{
			return Append(enumerable);
		}

		#endregion Append

		#region Assign

		/// <summary>
		/// Assign the string
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="isASCII">is it string ASCII</param>
		/// <returns>this binary array</returns>
		public BinaryArray Assign(string str, bool isASCII = false)
		{
			Clear();
			return Append(str, isASCII);
		}

		/// <summary>
		/// Assign the string
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="isASCII">is it string ASCII</param>
		/// <returns>this binary array</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(string str, bool isASCII)
		{
			return Assign(str, isASCII);
		}

		/// <summary>
		/// Assign this object this value 
		/// </summary>
		/// <param name="str">value</param>
		/// <returns>assigned object</returns>
		public BinaryArray Assign(IBinaryConvertibleReadOnly str)
		{
			if (str == this) return this;
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Assign this object this value 
		/// </summary>
		/// <param name="str">value</param>
		/// <returns>assigned object</returns>
		public BinaryArray Assign(BinaryArray str)
		{
			if (str == this) return this;
			Clear();
			return Append(str);
		}

		/// <summary>
		/// Assign this object this value 
		/// </summary>
		/// <param name="str">value</param>
		/// <returns>assigned object</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(IBinaryConvertibleReadOnly str)
		{
			return Assign(str);
		}

		/// <summary>
		/// Assigns the specified byte buffer.
		/// </summary>
		/// <param name="buffer">The byte buffer.</param>
		/// <param name = "size"> The buffer size </param>
		public unsafe BinaryArray Assign(byte* buffer, int size)
		{
			Clear();
			return Append(buffer, size);
		}

		/// <summary>
		/// Assigns the specified byte buffer.
		/// </summary>
		/// <param name="buffer">The byte buffer.</param>
		/// <param name = "size"> The buffer size </param>
		unsafe IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(byte* buffer, int size)
		{
			return Assign(buffer, size);
		}

		/// <summary>
		/// Assigns the specified byte buffer.
		/// </summary>
		/// <param name="buffer">The byte buffer.</param>
		/// <param name = "size"> The buffer size </param>
		public unsafe BinaryArray Assign(sbyte* buffer, int size)
		{
			Clear();
			return Append(buffer, size);
		}

		/// <summary>
		/// Assigns the specified byte buffer.
		/// </summary>
		/// <param name="buffer">The byte buffer.</param>
		/// <param name = "size"> The buffer size </param>
		unsafe IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(sbyte* buffer, int size)
		{
			return Assign(buffer, size);
		}

		/// <summary>
		/// Assigns the specified byte.
		/// </summary>
		/// <param name="buffer">The byte value.</param>
		public BinaryArray Assign(byte buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assigns the specified byte.
		/// </summary>
		/// <param name="buffer">The byte value.</param>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(byte buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Boolean
		/// </summary>
		/// <param name="buffer">The Boolean value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Boolean buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Boolean
		/// </summary>
		/// <param name="buffer">The Boolean value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Boolean buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Char
		/// </summary>
		/// <param name="buffer">The Char value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Char buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Char
		/// </summary>
		/// <param name="buffer">The Char value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Char buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified DateTime
		/// </summary>
		/// <param name="buffer">The DateTime value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(DateTime buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified DateTime
		/// </summary>
		/// <param name="buffer">The DateTime value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(DateTime buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Decimal
		/// </summary>
		/// <param name="buffer">The Decimal value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Decimal buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Decimal
		/// </summary>
		/// <param name="buffer">The Decimal value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Decimal buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Double
		/// </summary>
		/// <param name="buffer">The Double value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Double buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Double
		/// </summary>
		/// <param name="buffer">The Double value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Double buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Int16
		/// </summary>
		/// <param name="buffer">The Int16 value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Int16 buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Int16
		/// </summary>
		/// <param name="buffer">The Int16 value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Int16 buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Int32
		/// </summary>
		/// <param name="buffer">The Int32 value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Int32 buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Int32
		/// </summary>
		/// <param name="buffer">The Int32 value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Int32 buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Int64
		/// </summary>
		/// <param name="buffer">The Int64 value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Int64 buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Int64
		/// </summary>
		/// <param name="buffer">The Int64 value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Int64 buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified UInt16
		/// </summary>
		/// <param name="buffer">The UInt16 value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(UInt16 buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified UInt16
		/// </summary>
		/// <param name="buffer">The UInt16 value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(UInt16 buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified UInt32
		/// </summary>
		/// <param name="buffer">The UInt32 value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(UInt32 buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified UInt32
		/// </summary>
		/// <param name="buffer">The UInt32 value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(UInt32 buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified UInt64
		/// </summary>
		/// <param name="buffer">The UInt64 value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(UInt64 buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified UInt64
		/// </summary>
		/// <param name="buffer">The UInt64 value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(UInt64 buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified SByte
		/// </summary>
		/// <param name="buffer">The SByte value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(SByte buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified SByte
		/// </summary>
		/// <param name="buffer">The SByte value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(SByte buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the specified Single
		/// </summary>
		/// <param name="buffer">The Single value</param>
		/// <returns>BinaryArray</returns>
		public BinaryArray Assign(Single buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Single
		/// </summary>
		/// <param name="buffer">The Single value</param>
		/// <returns>BinaryArray</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Single buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign UUID to this
		/// </summary>
		/// <param name="item">UUID</param>
		/// <returns>binary array</returns>
		public BinaryArray Assign(UUID item)
		{
			Clear();
			return Append(item);
		}

		/// <summary>
		/// Assign UUID to this
		/// </summary>
		/// <param name="item">UUID</param>
		/// <returns>binary array</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(UUID item)
		{
			return Assign(item);
		}

		/// <summary>
		/// Assign the specified Guid
		/// </summary>
		/// <param name="buffer">The Guid Value</param>
		/// <returns></returns>
		public BinaryArray Assign(Guid buffer)
		{
			Clear();
			return Append(buffer);
		}

		/// <summary>
		/// Assign the specified Guid
		/// </summary>
		/// <param name="buffer">The Guid Value</param>
		/// <returns></returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Guid buffer)
		{
			return Assign(buffer);
		}

		/// <summary>
		/// Assign the mutable string
		/// </summary>
		/// <param name="str">mutable string</param>
		/// <param name="isASCII">is this string ASCII or no</param>
		/// <returns></returns>
		public BinaryArray Assign(IReadOnlyString str, bool isASCII = false)
		{
			Clear();
			return Append(str, isASCII);
		}

		/// <summary>
		/// Assign the mutable string
		/// </summary>
		/// <param name="str">mutable string</param>
		/// <returns></returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(IReadOnlyString str, bool isASCII)
		{
			return Assign(str, isASCII);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <returns>this</returns>
		public BinaryArray Assign(Byte[] bytes)
		{
			Clear();
			return Append(bytes);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Byte[] bytes)
		{
			return Assign(bytes);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		public BinaryArray Assign(Byte[] bytes, int offset, int count)
		{
			Clear();
			return Append(bytes, offset, count);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(Byte[] bytes, int offset, int count)
		{
			return Assign(bytes, offset, count);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <returns>this</returns>
		public BinaryArray Assign(SByte[] bytes)
		{
			Clear();
			return Append(bytes);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(SByte[] bytes)
		{
			return Assign(bytes);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		public BinaryArray Assign(SByte[] bytes, int offset, int count)
		{
			Clear();
			return Append(bytes, offset, count);
		}

		/// <summary>
		/// Assign the array of bytes
		/// </summary>
		/// <param name="bytes">array of bytes</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(SByte[] bytes, int offset, int count)
		{
			return Assign(bytes, offset, count);
		}

		/// <summary>
		/// Assign the enumerable of bytes
		/// </summary>
		/// <param name="enumerable">bytes</param>
		/// <returns>this</returns>
		public BinaryArray Assign(IEnumerable<byte> enumerable)
		{
			Clear();
			return Append(enumerable);
		}

		/// <summary>
		/// Assign the enumerable of bytes
		/// </summary>
		/// <param name="enumerable">bytes</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(IEnumerable<byte> enumerable)
		{
			return Assign(enumerable);
		}

		/// <summary>
		/// Assign the enumerable of sbytes
		/// </summary>
		/// <param name="enumerable">sbytes</param>
		/// <returns>this</returns>
		public BinaryArray Assign(IEnumerable<sbyte> enumerable)
		{
			Clear();
			return Append(enumerable);
		}

		/// <summary>
		/// Assign the enumerable of sbytes
		/// </summary>
		/// <param name="enumerable">sbytes</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(IEnumerable<sbyte> enumerable)
		{
			return Assign(enumerable);
		}

		#endregion Assign

		#region Converters

		/// <summary>
		/// Convert BinaryArray to bool
		/// </summary>
		/// <returns></returns>
		public bool ToBoolean()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					Boolean* p1 = (Boolean*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to byte
		/// </summary>
		/// <returns>byte</returns>
		public byte ToByte()
		{
			return (byte)data[0];
		}

		/// <summary>
		/// Convert BinaryArray to Char
		/// </summary>
		/// <returns>char</returns>
		public char ToChar()
		{
			return (char)data[0];
		}

		/// <summary>
		/// Convert BinaryArray to DateTime
		/// </summary>
		/// <returns>DateTime</returns>
		public DateTime ToDateTime()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					DateTime* p1 = (DateTime*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to decimal
		/// </summary>
		/// <returns>decimal</returns>
		public Decimal ToDecimal()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					Decimal* p1 = (Decimal*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to double
		/// </summary>
		/// <returns>double</returns>
		public double ToDouble()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					Double* p1 = (Double*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert to Int16
		/// </summary>
		/// <returns>Int16</returns>
		public Int16 ToInt16()
		{
			return (Int16)data[0];
		}

		/// <summary>
		/// Convert BinaryArray to Int32
		/// </summary>
		/// <returns>Int32</returns>
		public Int32 ToInt32()
		{
			return (Int32)data[0];
		}

		/// <summary>
		/// Convert BinaryArray to Int64
		/// </summary>
		/// <returns>Int64</returns>
		public Int64 ToInt64()
		{
			return data[0];
		}

		/// <summary>
		/// Convert to UInt16
		/// </summary>
		/// <returns>UInt16</returns>
		public UInt16 ToUInt16()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					UInt16* p1 = (UInt16*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Int32
		/// </summary>
		/// <returns>Int32</returns>
		public UInt32 ToUInt32()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					UInt32* p1 = (UInt32*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Int64
		/// </summary>
		/// <returns>Int64</returns>
		public UInt64 ToUInt64()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					UInt64* p1 = (UInt64*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert to sbyte
		/// </summary>
		/// <returns>sbyte</returns>
		public sbyte ToSByte()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					SByte* p1 = (SByte*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert to float
		/// </summary>
		/// <returns>float</returns>
		public float ToSingle()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					Single* p1 = (Single*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Guid
		/// </summary>
		/// <returns>Guid</returns>
		public Guid ToGuid()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					Guid* p1 = (Guid*)p;
					return *p1;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to UUID
		/// </summary>
		/// <returns>Guid</returns>
		public UUID ToUUID()
		{
			unsafe
			{
				fixed (long* p = data)
				{
					return new UUID((ulong)(*p), (ulong)(*(p + 1)));
				}
			}
		}





		/// <summary>
		/// Convert to UTF8
		/// </summary>
		/// <param name="utf8">reference to result array</param>
		public void ToUTF8(Byte[] utf8)
		{
			if (lastByte == 0)
				return;
			unsafe
			{
				fixed (long* p = data)
				{
					Marshal.Copy((IntPtr)p, utf8, 0, lastByte);
				}
			}
		}

		/// <summary>
		/// Convert to MutableString
		/// </summary>
		/// <param name="str">reference to result</param>
		/// <param name="index">start index</param>
		void IBinaryConvertibleReadOnly.ToMutableString(IMutableString str, int index)
		{
			str.Clear();
			for (int i = index; i < Count; i += 2)
				str.Append(ToChar(i));
		}

		/// <summary>
		/// Convert BinaryArray to Boolean
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Boolean</returns>
		public Boolean ToBoolean(Int32 offset)
		{
			if (offset + sizeof(Boolean) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					Boolean* buffer = (Boolean*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Byte
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Byte</returns>
		public Byte ToByte(Int32 offset)
		{
			if (offset + sizeof(Byte) > lastByte)
				throw new ArgumentOutOfRangeException();
			return GetByte(data[(offset) >> 3], (offset & 7) << 3);
		}

		/// <summary>
		/// Convert BinaryArray to Char
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Char</returns>
		public Char ToChar(Int32 offset)
		{
			if (offset + sizeof(Char) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					Char* buffer = (Char*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to DateTime
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>DateTime</returns>
		public DateTime ToDateTime(Int32 offset)
		{
			if (offset + 8 > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					DateTime* buffer = (DateTime*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Decimal
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Decimal</returns>
		public Decimal ToDecimal(Int32 offset)
		{
			if (offset + sizeof(Decimal) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					Decimal* buffer = (Decimal*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Double
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Double</returns>
		public Double ToDouble(Int32 offset)
		{
			if (offset + sizeof(Double) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					Double* buffer = (Double*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Int16
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int16</returns>
		public Int16 ToInt16(Int32 offset)
		{
			if (offset + sizeof(Int16) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					Int16* buffer = (Int16*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Int32
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int32</returns>
		public Int32 ToInt32(Int32 offset)
		{
			if (offset + sizeof(Int32) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					int* buffer = (int*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Int64
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int64</returns>
		public Int64 ToInt64(Int32 offset)
		{
			if (offset + sizeof(Int64) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					long* buffer = (long*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to UInt16
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>UInt16</returns>
		public UInt16 ToUInt16(Int32 offset)
		{
			if (offset + sizeof(UInt16) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					UInt16* buffer = (UInt16*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to UInt32
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>UInt32</returns>
		public UInt32 ToUInt32(UInt32 offset)
		{
			if (offset + sizeof(UInt32) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					UInt32* buffer = (UInt32*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to UInt64
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int64</returns>
		public UInt64 ToUInt64(Int32 offset)
		{
			if (offset + sizeof(UInt64) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					UInt64* buffer = (UInt64*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to SByte
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>SByte</returns>
		public SByte ToSByte(Int32 offset)
		{
			if (offset + sizeof(SByte) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					SByte* buffer = (SByte*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Single
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Single</returns>
		public Single ToSingle(Int32 offset)
		{
			if (offset + sizeof(Single) > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					Single* buffer = (Single*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to Guid
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Guid</returns>
		public Guid ToGuid(Int32 offset)
		{
			if (offset + 16 > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					Guid* buffer = (Guid*)(((byte*)lbuffer) + offset);
					return *buffer;
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to UUID
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>UUID</returns>
		public UUID ToUUID(Int32 offset)
		{
			if (offset + 16 > lastByte)
				throw new ArgumentOutOfRangeException();
			unsafe
			{
				fixed (long* p = data)
				{
					return new UUID(*((ulong*)(((byte*)p) + offset)), *((ulong*)(((byte*)p) + offset + 8)));
				}
			}
		}

		/// <summary>
		/// Convert to UTF8
		/// </summary>
		/// <param name="utf8">reference to result array</param>
		/// <param name="offset">offset</param>
		public void ToUTF8(Byte[] utf8, Int32 offset)
		{
			if (lastByte == 0)
				return;
			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Marshal.Copy((IntPtr)(p1 + offset), utf8, 0, lastByte - offset);
				}
			}
		}

		/// <summary>
		/// Convert to MutableString
		/// </summary>
		/// <param name="str">reference to result</param>
		/// <param name="offset">offset</param>
		public void ToMutableString(MutableString str, Int32 offset = 0)
		{
			if (lastByte == 0)
				return;
			str.Clear();
			str.Expand((lastByte - offset) >> 1);
			str.Length = (lastByte - offset) >> 1;
			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)(p);
					char* p2 = (char*)(p1 + offset);
					Marshal.Copy((IntPtr)p2, str._data, 0, (lastByte - offset) >> 1);
				}
			}
			str.ResetHashCode();
		}

		#endregion Converters

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public BinaryArray Clear()
		{
			if (data != null)
				Array.Clear(data, 0, data.Length);
			lastByte = 0;
			CalculateHashCode();
			return this;
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Clear()
		{
			return Clear();
		}

		/// <summary>
		/// Copy From
		/// </summary>
		/// <param name="source">source</param>
		/// <returns>this</returns>
		public BinaryArray CopyFrom(IBinaryArrayReadOnly source)
		{
			return Assign(source);
		}

		/// <summary>
		/// Copy From
		/// </summary>
		/// <param name="source">source</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.CopyFrom(IBinaryArrayReadOnly source)
		{
			return CopyFrom(source);
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public BinaryArray Clone()
		{
			BinaryArray result = new BinaryArray();
			CopyTo(result);
			return result;
		}

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadOnly.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Copies to
		/// </summary>
		/// <param name="ptr">destination</param>
		public unsafe void CopyTo(byte* ptr)
		{
			if (lastByte == 0)
				return;
			Marshal.Copy(data, 0, (IntPtr)ptr, ((lastByte + 7) >> 3));
		}

		/// <summary>
		/// Copies to
		/// </summary>
		/// <param name="str">destination</param>
		public void CopyTo(BinaryArray str)
		{
			Array.Resize(ref str.data, data.Length);
			str.hashCode = hashCode;
			str.lastByte = lastByte;
			int a = (lastByte + 7) >> 3;
			_optimizations[9 + ((a - 9) & (a - 9) >> 31)].CopyTo(data, str.data, a);
		}

		/// <summary>
		/// Copies to
		/// </summary>
		/// <param name="str">destination</param>
		public void CopyTo(IBinaryConvertibleReadWrite str)
		{
			str.Clear();
			str.Append((IBinaryConvertibleReadWrite)this);
		}

		/// <summary>
		/// Convert to Byte array
		/// </summary>
		/// <param name="sourceIndex">source index</param>
		/// <param name="destination">destination</param>
		/// <param name="destinationIndex">destination index</param>
		/// <param name="count">number of element</param>
		public void ToByteArray(Int32 sourceIndex, Byte[] destination, Int32 destinationIndex, Int32 count)
		{
			if (sourceIndex < 0 || sourceIndex + count > lastByte)
				throw new IndexOutOfRangeException();
			if (count == 0)
				return;
			unsafe
			{
				fixed (long* p = data)
				{
					byte* p1 = (byte*)p;
					Marshal.Copy((IntPtr)(p1 + sourceIndex), destination, destinationIndex, count);
				}
			}
		}

		/// <summary>
		/// Convert to SByte array
		/// </summary>
		/// <param name="sourceIndex">source index</param>
		/// <param name="destination">destination</param>
		/// <param name="destinationIndex">destination index</param>
		/// <param name="count">number of element</param>
		public void ToSByteArray(Int32 sourceIndex, SByte[] destination, Int32 destinationIndex, Int32 count)
		{
			if (sourceIndex < 0 || sourceIndex + count > lastByte)
				throw new IndexOutOfRangeException();
			if (count == 0)
				return;
			unsafe
			{
				fixed (long* p = data)
				{
					sbyte* p1 = (sbyte*)p;
					Marshal.Copy((IntPtr)(p1 + sourceIndex), (byte[])(Array)destination, destinationIndex, count);
				}
			}
		}

		/// <summary>
		/// Convert to byte array
		/// </summary>
		/// <returns>byte array</returns>
		public Byte[] ToByteArray()
		{
			byte[] array = new byte[lastByte];
			if (lastByte == 0)
				return array;
			unsafe
			{
				fixed (long* p = data)
				{
					Marshal.Copy((IntPtr)p, array, 0, lastByte);
				}
			}
			return array;
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
		/// Returns a string that represents the current object.
		/// </summary>
		/// <param name="isASCII">is it string ASCII</param>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public String ToString(bool isASCII)
		{
			unsafe
			{
				fixed (long* p = data)
				{
					if (!isASCII)
					{
						char* p1 = (char*)p;
						if (p1 == null) return "";
						return new String(p1, 0, lastByte >> 1);
					}
					else
					{
						sbyte* p1 = (sbyte*)p;
						if (p1 == null) return "";
						return new String(p1, 0, lastByte);
					}
				}
			}
		}
		/// <summary>
		///  Returns a string that represents the current object.
		/// </summary>
		/// <returns> A string that represents the current object.</returns>
		public override String ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <param name="offset">Start offset in binary array.</param>
		/// <returns>New string, representing this binary array.</returns>
		IReadOnlyString IBinaryConvertibleReadOnly.ToMutableString(int offset)
		{
			return ToMutableString(offset);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <param name="offset">Start offset in binary array.</param>
		/// <returns>New string, representing this binary array.</returns>
		public MutableString ToMutableString(int offset = 0)
		{
			MutableString ret = new MutableString();
			ToMutableString(ret, offset);
			return ret;
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
		/// Gets or sets the <see cref="Byte"/> at the specified index.
		/// </summary>
		/// <value>
		/// The <see cref="Byte"/>.
		/// </value>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		/// <exception cref="System.IndexOutOfRangeException">
		/// </exception>
		/// <exception cref="IndexOutOfRangeException"></exception>
		public unsafe Byte this[Int32 index]
		{
			get
			{
				if (index < 0 || index >= lastByte)
					throw new IndexOutOfRangeException();

				return GetByte(data[index >> 3], (index & 7) << 3);
			}

			set
			{
				if (index < 0 || index >= lastByte)
					throw new IndexOutOfRangeException();
				SetLongByByte(ref data[index >> 3], (index & 7) << 3, value);
				CalculateHashCode();
			}
		}

		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		public Int32 Length { get { return ((lastByte + 7) >> 3); } set { lastByte = value << 3; } }

		/// <summary>
		/// Gets or sets the capacity.
		/// </summary>
		/// <value>
		/// The capacity.
		/// </value>
		public Int32 Capacity { get { return data.Length << 3; } set { Expand(value); } }

		#region IComparable Members

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>. Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
		public Int32 CompareTo(Object obj)
		{
			BinaryArray another = obj as BinaryArray;
			Int32 tempLength = another.Length < ((lastByte + 7) >> 3) ? another.Length : ((lastByte + 7) >> 3);
			for (Int32 i = 0; i < tempLength; ++i)
				if (data[i] != another.data[i])
					return data[i] < another.data[i] ? -1 : 1;

			if (another.Length == ((lastByte + 7) >> 3))
				return 0;

			return another.Length < ((lastByte + 7) >> 3) ? -1 : 1;
		}

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="ba1">An object to compare with ba2 instance.</param>>
		/// <param name="ba2">An object to compare with ba1 instance.</param>>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero <paramref name="ba1"/> is less than the <paramref name="ba2"/> parameter.Zero <paramref name="ba1"/>is equal to <paramref name="ba2"/>. Greater than zero <paramref name="ba1"/> is greater than <paramref name="ba2"/>. 
		/// </returns>
		public static Int32 Compare(BinaryArray ba1, BinaryArray ba2)
		{
			if (ba1 == null)
				throw new ArgumentNullException("ba1");

			return ba1.CompareTo(ba2);
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

		#region IComparable<> Members

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="another"/> parameter.Zero This object is equal to <paramref name="another"/>. Greater than zero This object is greater than <paramref name="another"/>. 
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		public Int32 CompareTo(BinaryArray another)
		{
			if (another == null)
				throw new ArgumentNullException("another");

			Int32 tempLength = another.Length < ((lastByte + 7) >> 3) ? another.Length : ((lastByte + 7) >> 3);
			for (Int32 i = 0; i < tempLength; ++i)
				if (data[i] != another.data[i])
					return data[i] < another.data[i] ? -1 : 1;

			if (another.Length == ((lastByte + 7) >> 3))
				return 0;

			return another.Length < ((lastByte + 7) >> 3) ? -1 : 1;
		}

		#endregion IComparable<> Members

		#region IEnumerable<char> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		/// <filterpriority>1</filterpriority>
		public IEnumerator<Byte> GetEnumerator()
		{
			for (Int32 i = 0; i < lastByte; ++i)
				yield return GetByte(data[i >> 3], (i & 7) << 3);
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
			for (Int32 i = 0; i < ((lastByte + 7) >> 3); ++i)
				yield return data[i];
		}

		#endregion IEnumerable Members

		#region IEquatable<BinaryArray> Members

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
				if (this[i] != another[i])
					return false;
			return true;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="ba1">An object to compare with <paramref name="ba2"/>.</param>
		/// <param name="ba2">An object to compare with <paramref name="ba1"/>.</param>
		/// <returns>
		/// true if the <paramref name="ba1"/> is equal to the <paramref name="ba2"/> parameter; otherwise, false.
		/// </returns>
		public static Boolean Equals(BinaryArray ba1, BinaryArray ba2)
		{
			if (ba1 == null && ba2 == null)
				return true;
			if (ba1 == null || ba2 == null)
				return false;

			return ba1.Equals(ba2);
		}

		/// <summary>
		/// Equals to object
		/// </summary>
		/// <param name="obj">object</param>
		/// <returns>true if equals</returns>
		public override Boolean Equals(Object obj)
		{
			if (obj is BinaryArray) return (this as IEquatable<BinaryArray>).Equals((BinaryArray)obj);
			if (obj is IReadOnlyString) return ((IReadOnlyString)this).Equals((IReadOnlyString)obj);
			if (obj is String) ((IReadOnlyString)this).Equals((String)obj);
			if (obj is IBinaryConvertibleReadOnly) return Equals((IBinaryConvertibleReadOnly)obj);
			return false;
		}

		#endregion  IEquatable<BinaryArray> Members

		private void SetLongByByte(ref long destination, int index, byte src)
		{
			destination -= (((destination >> index) & 255) << index);
			destination += ((long)src << index);
		}

		private byte GetByte(long x, int index)
		{
			return (byte)((x >> index) & 255);
		}

		private byte ShiftLongRight(ref long destination, byte leftByte)
		{
			byte x = GetByte(destination, 56);
			destination = destination - x * ((long)1 << 56);
			destination = (destination << 8) + leftByte;
			return x;
		}

		/// <summary>
		/// Return index of first occurance of item or -1 if no occurance
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>index of first occurance of item</returns>
		public int IndexOf(byte item)
		{
			for (int i = 0; i < lastByte; ++i)
				if (GetByte(data[i >> 3], (i & 7) << 3) == item)
					return i;
			return -1;
		}

		private void InsertGap(int index, int size)
		{
			Expand((lastByte + size + 8) >> 3);
			unsafe
			{
				fixed (long* ptr = data)
				{
					byte* ptr1 = (byte*)ptr;
					MemoryUtils.MoveRight((ptr1 + index), (ptr1 + size + index), lastByte - index);
				}
			}
			lastByte += size;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		void IList<Byte>.Insert(int index, byte item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			hashCode = hashCode | 0x40000000;
			Expand((lastByte + 8) >> 3);
			int block = index >> 3;
			byte rightByte = (byte)((data[block] & (~((1 << 56) - 1))) >> 56);
			long x = (data[block] & (((long)1 << ((index & 7) << 3)) - 1));
			data[block] = x + (((long)item) << ((index & 7) << 3)) + (((data[block] - x) & (((long)1 << 56) - 1)) << 8);
			int finalBlock = (lastByte - 1) >> 3;
			for (int i = block + 1; i <= finalBlock; ++i)
			{
				rightByte = ShiftLongRight(ref data[i], rightByte);
			}
			if ((lastByte & 7) == 0 && index != lastByte)
			{
				Add(rightByte);
			}
			else
			{
				lastByte++;
			}
			CalculateHashCode();
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, byte item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			Expand((lastByte + 8) >> 3);
			int block = index >> 3;
			byte rightByte = (byte)((data[block] & (~((1 << 56) - 1))) >> 56);
			long x = (data[block] & (((long)1 << ((index & 7) << 3)) - 1));
			data[block] = x + (((long)item) << ((index & 7) << 3)) + (((data[block] - x) & (((long)1 << 56) - 1)) << 8);
			int finalBlock = (lastByte - 1) >> 3;
			for (int i = block + 1; i <= finalBlock; ++i)
			{
				rightByte = ShiftLongRight(ref data[i], rightByte);
			}
			if ((lastByte & 7) == 0 && index != lastByte)
			{
				Add(rightByte);
			}
			else
			{
				lastByte++;
			}
			CalculateHashCode();
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, byte item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Int16 item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 2);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Int16 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, UInt16 item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 2);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, UInt16 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Char item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 2);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Char item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Int32 item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 4);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Int32 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, UInt32 item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 4);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, UInt32 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Int64 item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 8);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Int64 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, UInt64 item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 8);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, UInt64 item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Guid item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 16);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Guid item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Double item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 8);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Double item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, DateTime item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 8);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, DateTime item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Single item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 4);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Single item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, IReadOnlyString item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 2 * item.Length);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, IReadOnlyString item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Byte[] item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Length);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Byte[] item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Byte[] item, int offset, int count)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, count);
			int temp = lastByte;
			lastByte = index;
			Append(item, offset, count);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Byte[] item, int offset, int count)
		{
			return Insert(index, item, offset, count);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, SByte[] item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Length);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, SByte[] item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, SByte[] item, int offset, int count)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, count);
			int temp = lastByte;
			lastByte = index;
			Append(item, offset, count);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, SByte[] item, int offset, int count)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Decimal item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 16);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Decimal item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, Boolean item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, 1);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, Boolean item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, IBinaryConvertibleReadOnly item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Count);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, IBinaryConvertibleReadOnly item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe public BinaryArray Insert(int index, byte* item, int count)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, count);
			int temp = lastByte;
			lastByte = index;
			Append(item, count);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, byte* item, int count)
		{
			return Insert(index, item, count);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe public BinaryArray Insert(int index, sbyte* item, int count)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, count);
			int temp = lastByte;
			lastByte = index;
			Append(item, count);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, sbyte* item, int count)
		{
			return Insert(index, item, count);
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="isASCII">isASCII</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, string item, bool isASCII = false)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			if (isASCII)
				InsertGap(index, item.Length);
			else
				InsertGap(index, item.Length * 2);
			int temp = lastByte;
			lastByte = index;
			Append(item, isASCII);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="isASCII">isASCII</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, string item, bool isASCII)
		{
			return Insert(index, item, isASCII);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, IEnumerable<Byte> item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Count());
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, IEnumerable<Byte> item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Insert(int index, IEnumerable<SByte> item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Count());
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, IEnumerable<SByte> item)
		{
			return Insert(index, item);
		}

		/// <summary>
		/// Remove item at index
		/// </summary>
		/// <param name="index">index</param>
		void IList<Byte>.RemoveAt(int index)
		{
			if (index >= lastByte)
				throw new IndexOutOfRangeException("incorrect index");

			for (int i = index; i < lastByte - 1; i++)
				SetLongByByte(ref data[(i) >> 3], 8 * (i & 7), GetByte(data[(i + 1) >> 3], 8 * ((i + 1) & 7)));
			lastByte--;
			CalculateHashCode();
		}

		/// <summary>
		/// Remove item at index
		/// </summary>
		/// <param name="index">index</param>
		void IList<SByte>.RemoveAt(int index)
		{
			if (index >= lastByte)
				throw new IndexOutOfRangeException("incorrect index");

			hashCode = hashCode | 0x40000000;
			for (int i = index; i < lastByte - 1; i++)
				SetLongByByte(ref data[(i) >> 3], (i & 7) << 3, GetByte(data[(i + 1) >> 3], (((i + 1) & 7)) << 3));
			lastByte--;
		}

		/// <summary>
		/// Remove item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>this</returns>
		public BinaryArray RemoveAt(int index)
		{
			if (index >= lastByte)
				throw new IndexOutOfRangeException("incorrect index");

			hashCode = hashCode | 0x40000000;
			for (int i = index; i < lastByte - 1; i++)
				SetLongByByte(ref data[(i) >> 3], (i & 7) << 3, GetByte(data[(i + 1) >> 3], (((i + 1) & 7)) << 3));
			lastByte--;
			return this;
		}

		/// <summary>
		/// Remove item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.RemoveAt(int index)
		{
			return RemoveAt(index);
		}

		/// <summary>
		/// Pushback item
		/// </summary>
		/// <param name="item">item</param>
		void ICollection<Byte>.Add(byte item)
		{
			Append(item);
		}

		/// <summary>
		/// Pushback item
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Add(byte item)
		{
			Append(item);
			return this;
		}

		void ICollection<byte>.Clear()
		{
			lastByte = 0;
			hashCode = hashCode | 0x40000000;
			CalculateHashCode();
		}

		/// <summary>
		/// return true if string contains item
		/// </summary>
		/// <param name="item">true</param>
		/// <returns>true if string contains item</returns>
		public bool Contains(byte item)
		{
			return IndexOf(item) != -1;
		}

		/// <summary>
		/// Copy string to char array with offset
		/// </summary>
		/// <param name="array">array</param>
		/// <param name="arrayIndex">offset</param>
		public void CopyTo(byte[] array, int arrayIndex)
		{
			ToByteArray(0, array, arrayIndex, lastByte);
		}

		/// <summary>
		/// Number of elementes
		/// </summary>
		public int Count
		{
			get { return lastByte; }
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
		bool ICollection<Byte>.Remove(byte item)
		{
			int x = IndexOf(item);
			if (x == -1)
				return false;
			RemoveAt(x);
			return true;
		}

		/// <summary>
		/// Remove first occurance of item
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Remove(byte item)
		{
			int x = IndexOf(item);
			if (x == -1)
				return this;
			RemoveAt(x);
			return this;
		}

		int IList<SByte>.IndexOf(sbyte item)
		{
			return IndexOf((byte)item);
		}

		/// <summary>
		/// Insert item to position index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		/// <returns> this </returns>
		public BinaryArray Insert(int index, sbyte item)
		{
			Insert(index, (byte)item);
			return this;
		}

		/// <summary>
		/// Insert item to position index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		void IList<SByte>.Insert(int index, sbyte item)
		{
			Insert(index, (byte)item);
		}

		sbyte IList<sbyte>.this[int index]
		{
			get
			{
				return (sbyte)(this as IList<Byte>)[index];
			}
			set
			{
				(this as IList<Byte>)[index] = (byte)value;
			}
		}

		void ICollection<sbyte>.Add(sbyte item)
		{
			Add((byte)item);
		}

		void ICollection<sbyte>.Clear()
		{
			lastByte = 0;
		}

		bool ICollection<sbyte>.Contains(sbyte item)
		{
			return Contains((byte)item);
		}

		/// <summary>
		/// Copy to sbyte array
		/// </summary>
		/// <param name="array">array</param>
		/// <param name="arrayIndex">arrayIndex</param>
		public void CopyTo(sbyte[] array, int arrayIndex)
		{
			CopyTo((byte[])(Array)array, arrayIndex);
		}

		/// <summary>
		/// Remove item from array
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>true if removed</returns>
		bool ICollection<SByte>.Remove(sbyte item)
		{
			return (this as IList<Byte>).Remove((byte)item);
		}

		/// <summary>
		/// Remove item from array
		/// </summary>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		public BinaryArray Remove(sbyte item)
		{
			return Remove((byte)item);
		}

		IEnumerator<sbyte> IEnumerable<sbyte>.GetEnumerator()
		{
			for (Int32 i = 0; i < lastByte; ++i)
				yield return (sbyte)GetByte(data[i >> 3], (i & 7) << 3);
		}

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
				fixed (void* ptr = data)
				{
					byte* buffer = (byte*)ptr;
					for (int i = 0; i < lastByte; ++i)
						ms.WriteByte(buffer[i]);
				}
			}
			info.AddValue("d", ms.ToArray(), typeof(Byte[]));
		}



		/// <summary>
		/// Create instance of this class by serialization
		/// </summary>
		/// <param name="info">info</param>
		/// <param name="context">context</param>
		public BinaryArray(SerializationInfo info, StreamingContext context)
		{
			Byte[] buffer = (Byte[])info.GetValue("d", typeof(Byte[]));
			Assign(buffer);
		}




		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Assign(String str, bool isASCII)
		{
			return Assign(str, isASCII);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Assign(long x)
		{
			return Assign(x);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Assign(byte[] x)
		{
			return Assign(x);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Assign(byte[] x, int offset, int count)
		{
			return Assign(x, offset, count);
		}

		/// <summary>
		/// Assign BinaryArray by another BinaryArray.
		/// </summary>
		/// <param name="ar">Another BinaryArray.</param>
		/// <returns>BinaryArray after assign.</returns>
		public BinaryArray Assign(IBinaryArrayReadOnly ar)
		{
			if (ar == this) return this;
			Clear();
			Append(ar);
			return this;
		}
		/// <summary>
		/// Append BinaryArray to another BinaryArray.
		/// </summary>
		/// <param name="ar">Source BinaryArray.</param>
		/// <returns>Destination BinaryArray after append.</returns>
		public BinaryArray Append(IBinaryArrayReadOnly ar)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + ar.Count + 7) >> 3);
			for (int i = 0; i < ar.Count; ++i) InternalAppend(ar[i]);
			return this;
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Assign(IBinaryArrayReadOnly ar)
		{
			return Assign(ar);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Append(String str, bool isASCII)
		{
			return Append(str, isASCII);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Append(long x)
		{
			return Append(x);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Append(byte[] x)
		{
			return Append(x);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Append(byte[] x, int offset, int count)
		{
			return Append(x, offset, count);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Append(IBinaryArrayReadOnly ar)
		{
			return Append(ar);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Append(byte x)
		{
			return Append(x);
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Clone()
		{
			return Clone();
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Set(int index, byte value)
		{
			this[index] = value;
			return this;
		}

		IBinaryIdentifierReadWrite IBinaryIdentifierReadWrite.Clear()
		{
			Clear();
			return this;
		}

		long IBinaryIdentifierReadOnly.ToLong()
		{
			return ToInt64();
		}

		long IBinaryIdentifierReadOnly.ToLong(int offset)
		{
			return ToInt64(offset);
		}

		private static char[] HexDigitsUpper = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

		/// <summary>
		/// Convert BinaryArray to MutableString with Encoding.
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <param name="str">Destination MutableString.</param>
		public void GetChars(CharEncoding charset, MutableString str)
		{
			GetChars(charset, str, 0);
		}

		/// <summary>
		/// Convert BinaryArray to MutableString with Encoding.
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <param name="str">Destination MutableString.</param>
		/// <param name="offset">Source offset.</param>
		public void GetChars(CharEncoding charset, MutableString str, int offset)
		{
			if (charset == CharEncoding.Utf8) ToMutableString(str, offset);
			else if (charset == CharEncoding.Utf16)
			{
				throw new NotImplementedException();
			}
			else if (charset == CharEncoding.ASCII)
			{
				str.Clear();
				for (int i = offset; i < Count; ++i) str.Append((char)ToByte(i));
			}
			else if (charset == CharEncoding.Hex)
			{
				str.Clear();
				for (int i = offset; i < Count; ++i)
				{
					byte v = ToByte(i);
					str.Append(HexDigitsUpper[v >> 4]);
					str.Append(HexDigitsUpper[v & 0x0F]);
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to String with Encoding.
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <param name="offset">Offset in BinaryArray.</param>
		/// <returns>String representation of BinaryArray.</returns>
		public String ToString(CharEncoding charset, int offset)
		{
			MutableString str = new MutableString();
			if (charset == CharEncoding.Utf8) ToMutableString(str, offset);
			else if (charset == CharEncoding.Utf16)
			{
				throw new NotImplementedException();
			}
			else if (charset == CharEncoding.ASCII)
			{
				for (int i = offset; i < Count; ++i) str.Append((char)ToByte(i));
			}
			else if (charset == CharEncoding.Hex)
			{
				for (int i = offset; i < Count; ++i)
				{
					byte v = ToByte(i);
					str.Append(HexDigitsUpper[v >> 4]);
					str.Append(HexDigitsUpper[v & 0x0F]);
				}
			}
			return str.ToString();
		}

		/// <summary>
		/// Convert BinaryArray to String with Encoding. 
		/// </summary>
		/// <param name="charset">CharEncoding.</param>
		/// <returns>String representation of BinaryArray.</returns>
		public String ToString(CharEncoding charset)
		{
			return ToString(charset, 0);
		}


		bool IBinaryIdentifierReadOnly.Equals(IBinaryIdentifierReadOnly another)
		{
			if (Count != another.Count) return false;
			for (int i = 0; i < Count; ++i) if (this[i] != another[i]) return false;
			return true;
		}

		int IBinaryIdentifierReadOnly.HashCode()
		{
			return GetHashCode();
		}

		IBinaryIdentifierReadOnly IBinaryIdentifierReadOnly.Clone()
		{
			return Clone();
		}

		IBinaryArrayReadWrite IBinaryArrayReadWrite.Assign(byte[] ar)
		{
			return Assign(ar);
		}

		IBinaryArrayReadWrite IBinaryArrayReadWrite.Assign(byte[] ar, int offset, int count)
		{
			return Assign(ar, offset, count);
		}

		IBinaryArrayReadWrite IBinaryArrayReadWrite.Assign(IBinaryArrayReadOnly ar)
		{
			return Assign(ar);
		}

		IBinaryArrayReadWrite IBinaryArrayReadWrite.Append(byte[] ar)
		{
			return Append(ar);
		}
		IBinaryArrayReadWrite IBinaryArrayReadWrite.Append(byte[] ar, int offset, int count)
		{
			return Append(ar, offset, count);
		}

		IBinaryArrayReadWrite IBinaryArrayReadWrite.Append(IBinaryArrayReadOnly ar)
		{
			return Append(ar);
		}

		IBinaryArrayReadWrite IBinaryArrayReadWrite.Append(byte x)
		{
			return Append(x);
		}

		IBinaryArrayReadWrite IBinaryArrayReadWrite.Clone()
		{
			return Clone();
		}



		IBinaryArrayReadWrite IBinaryArrayReadWrite.Clear()
		{
			Clear();
			return this;
		}

		/// <summary>
		/// Convert BinaryArray to byte array.
		/// </summary>
		/// <param name="buffer">Byte Array.</param>
		public void GetBytes(byte[] buffer)
		{
			ToByteArray(0, buffer, 0, Count);
		}
		/// <summary>
		/// Convert BinaryArray to byte array with offset.
		/// </summary>
		/// <param name="buffer">Destination byte array.</param>
		/// <param name="srcOffset">BinaryArray offset.</param>
		/// <param name="size">Count of bytes to convert.</param>
		public void GetBytes(byte[] buffer, int srcOffset, int size)
		{
			ToByteArray(srcOffset, buffer, 0, size);
		}
		/// <summary>
		/// Convert BinaryArray to byte array with offset.
		/// </summary>
		/// <param name="buffer">Destination byte array.</param>
		/// <param name="srcOffset">Source offset.</param>
		/// <param name="dstOffset">Destination offset.</param>
		/// <param name="size">Count of bytes to convert.</param>
		public void GetBytes(byte[] buffer, int srcOffset, int dstOffset, int size)
		{
			ToByteArray(srcOffset, buffer, dstOffset, size);
		}



		IBinaryArrayReadOnly IBinaryArrayReadOnly.Clone()
		{
			return Clone();
		}

		/// <summary>
		/// Return byte at index.
		/// </summary>
		/// <param name="index">Index of byte.</param>
		/// <returns>Byte at index.</returns>
		public byte Get(int index)
		{
			return this[index];
		}

		/// <summary>
		/// Append ReadOnlySpan.
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray Append(ReadOnlySpan<byte> bytes)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + bytes.Length + 7) >> 3);
			unsafe
			{
				fixed (long* longPtr = data)
				{
					byte* bytePtr = (byte*)longPtr;
					bytes.CopyTo(new Span<byte>((bytePtr + lastByte), bytes.Length));
				}
			}
			for (int i = 0; i < bytes.Length; ++i) InternalAppend(bytes[i]);
			return this;

		}

		/// <summary>
		/// Append ReadOnlySpan.
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray Append(ReadOnlySpan<sbyte> bytes)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + bytes.Length + 7) >> 3);
			unsafe
			{
				fixed (long* longPtr = data)
				{
					byte* bytePtr = (byte*)longPtr;
					bytes.CopyTo(new Span<sbyte>((bytePtr + lastByte), bytes.Length));
				}
			}
			lastByte += bytes.Length;
			return this;
		}

		/// <summary>
		/// Append ReadOnlySpan.
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray Append(ReadOnlySpan<char> bytes)
		{
			hashCode = hashCode | 0x40000000;
			Expand((lastByte + (bytes.Length << 1) + 7) >> 3);

			unsafe
			{
				fixed (long* longPtr = data)
				{
					byte* bytePtr = (byte*)longPtr;
					bytes.CopyTo(new Span<char>((bytePtr + lastByte), bytes.Length << 1));
				}
			}

			lastByte += (bytes.Length << 1);
			return this;
		}


		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray Assign(ReadOnlySpan<byte> bytes)
		{
			Clear();
			return Append(bytes);
		}

		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray Assign(ReadOnlySpan<sbyte> bytes)
		{
			Clear();
			return Append(bytes);
		}

		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		public BinaryArray Assign(ReadOnlySpan<char> bytes)
		{
			Clear();
			return Append(bytes);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(ReadOnlySpan<byte> bytes)
		{
			return Append(bytes);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(ReadOnlySpan<sbyte> bytes)
		{
			return Append(bytes);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Append(ReadOnlySpan<char> bytes)
		{
			return Append(bytes);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(ReadOnlySpan<byte> bytes)
		{
			return Assign(bytes);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(ReadOnlySpan<sbyte> bytes)
		{
			return Assign(bytes);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Assign(ReadOnlySpan<char> bytes)
		{
			return Assign(bytes);
		}

		public BinaryArray Insert(int index, ReadOnlySpan<byte> item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Length);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		public BinaryArray Insert(int index, ReadOnlySpan<sbyte> item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Length);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		public BinaryArray Insert(int index, ReadOnlySpan<char> item)
		{
			if (index > lastByte)
				throw new IndexOutOfRangeException("Insert in incorrect place.");

			InsertGap(index, item.Length << 1);
			int temp = lastByte;
			lastByte = index;
			Append(item);
			lastByte = temp;
			return this;
		}

		/// <summary>
		/// Convert BinaryArray to byte span.
		/// </summary>
		/// <param name="destination">Destination byte span</param>
		public void ToUTF8(Span<byte> destination)
		{
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					byte* buffer = (byte*)lbuffer;
					new Span<byte>(buffer, lastByte).CopyTo(destination);
				}
			}
		}
		/// <summary>
		/// Convert BinaryArray to byte span.
		/// </summary>
		/// <param name="sourceIndex">Start offset in binary array</param>
		/// <param name="destination">Destination byte span</param>
		public void ToByteArray(Int32 sourceIndex, Span<byte> destination)
		{
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					byte* buffer = (byte*)lbuffer;
					new Span<byte>(buffer + sourceIndex, lastByte - sourceIndex).CopyTo(destination);
				}
			}
		}

		/// <summary>
		/// Convert BinaryArray to byte span.
		/// </summary>
		/// <param name="sourceIndex">Start offset in binary array</param>
		/// <param name="destination">Destination sbyte span</param>

		public void ToSByteArray(Int32 sourceIndex, Span<sbyte> destination)
		{
			unsafe
			{
				fixed (long* lbuffer = data)
				{
					sbyte* buffer = (sbyte*)lbuffer;
					new Span<sbyte>(buffer + sourceIndex, lastByte - sourceIndex).CopyTo(destination);
				}
			}
		}




		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, ReadOnlySpan<byte> item)
		{
			return Insert(index, item);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, ReadOnlySpan<sbyte> item)
		{
			return Insert(index, item);
		}

		IBinaryConvertibleReadWrite IBinaryConvertibleReadWrite.Insert(int index, ReadOnlySpan<char> item)
		{
			return Insert(index, item);
		}



		/// <summary>
		/// Return size of BinaryArray;
		/// </summary>
		public int Size
		{
			get
			{
				return Count;
			}
		}


	}
}
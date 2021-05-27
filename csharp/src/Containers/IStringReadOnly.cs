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

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Readonly interface for mutable string
	/// </summary>
	public interface IReadOnlyString
	{
		/// <summary>
		/// Extract sub string.
		/// </summary>
		/// <param name="start">Start char index.</param>
		/// <param name="end">End char idnex.</param>
		/// <returns></returns>
		IReadOnlyString SubString(Int32 start, Int32 end);

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		Boolean Equals(IReadOnlyString another);

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="another">An object to compare with this object.</param>
		/// <returns>True if the current object is equal to the other parameter; otherwise, false.</returns>
		Boolean Equals(String another);

		/// <summary>
		/// Number of elementes
		/// </summary>
		int Length
		{
			get;
		}

		/// <summary>
		/// Defines if string has no characters (i.e. it's length equals zero).
		/// </summary>
		bool Empty
		{
			get;
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
		Char this[Int32 index]
		{
			get;
		}

		/// <summary>
		/// Convert value to the int64.
		/// </summary>
		/// <returns>Converted int64 value.</returns>
		Int64 ToInt64();

		/// <summary>
		/// Convert string to UTF8 and store it into byte array.
		/// </summary>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <returns>Number of bytes written.</returns>
		Int32 ToUTF8(Byte[] utf8);

		/// <summary>
		/// Convert string to UTF8 and store into byte array.
		/// </summary>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <returns>Number of bytes written.</returns>
		Int32 ToUTF8(Byte[] utf8, Int32 offset);

		/// <summary>
		/// Convert string to UTF8 and store into byte buffer.
		/// </summary>
		/// <param name="utf8">Byte buffer to store utf8 string.</param>
		/// <returns>Number of bytes written.</returns>
		unsafe Int32 ToUTF8(Byte* utf8);

		/// <summary>
		/// Convert string to UTF8 and store into byte buffer.
		/// </summary>
		/// <param name="utf8">Byte buffer to store utf8 string.</param>
		/// <param name="byteCount">Number of bytes in byte buffer.</param>
		/// <returns>Number of bytes written.</returns>
		unsafe Int32 ToUTF8(Byte* utf8, Int32 byteCount);

		/// <summary>
		/// Convert string to UTF8 and store into byte array.
		/// </summary>
		/// <param name="charIndex">Start index.</param>
		/// <param name="charCount">Count of char to convert.</param>
		/// <param name="utf8">Byte array to store utf8 string.</param>
		/// <param name="utf8Index">Offset in byte array.</param>
		/// <returns>Number of bytes written.</returns>
		Int32 ToUTF8(Int32 charIndex, Int32 charCount, Byte[] utf8, Int32 utf8Index);

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		String ToString();

		/// <summary>
		/// Assigns binary representation of this string to specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Output <see cref="BinaryArray"/>, containing binary representation of this string.</param>
		void ToBinaryArray(IBinaryConvertibleReadWrite binaryArray);

		/// <summary>
		/// Returns binary representation of this string.
		/// </summary>
		/// <returns>New <see cref="BinaryArray"/> representation of this string.</returns>
		IBinaryConvertibleReadOnly ToBinaryArray();

		/// <summary>
		/// Create <see cref="UUID"/> from this string, using specified <see cref="UUIDParseFormat"/>.
		/// </summary>
		/// <param name="format">This string allowed uuid format.</param>
		/// <returns> New <see cref="UUID"/>.</returns>
		UUID ToUUID(UUIDParseFormat format = UUIDParseFormat.Any);

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		IMutableString Clone();

		/// <summary>
		/// Copy read-only string to mutable string.
		/// </summary>
		/// <param name="sourceIndex">Index of the source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destinationIndex">Index of the destination.</param>
		/// <param name="count">The count.</param>
		/// <param name="str"/>
		/// <exception cref="System.IndexOutOfRangeException"></exception>
		void CopyTo(IMutableString str);

		/// <summary>
		/// Copy read-only string to char array 
		/// </summary>
		/// <param name="sourceIndex">Start index in source.</param>
		/// <param name="destination">Destination char array.</param>
		/// <param name="destinationIndex">Start index in destination.</param>
		/// <param name="count">Count of char to copy.</param>
		void ToCharArray(Int32 sourceIndex, Char[] destination, Int32 destinationIndex, Int32 count);

		/// <summary>
		/// Convert string to char span.
		/// </summary>
		/// <param name="span">span to convert</param>
		void ToCharArray(Span<char> span);

		/// <summary>
		/// Get number of bytes in UTF8 representation of this string.
		/// </summary>
		Int32 UTF8ByteLength { get; }


	}
}
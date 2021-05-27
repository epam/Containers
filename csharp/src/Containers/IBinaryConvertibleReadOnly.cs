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

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Read only interface for BinaryArray
	/// </summary>
	public interface IBinaryConvertibleReadOnly : IBinaryIdentifierReadOnly
	{
		/// <summary>
		/// Convert BinaryArray to UUID
		/// </summary>
		/// <returns>UUID</returns>
		UUID ToUUID();
		/// <summary>
		/// Convert BinaryArray to bool
		/// </summary>
		/// <returns></returns>
		bool ToBoolean();
		/// <summary>
		/// Convert BinaryArray to byte
		/// </summary>
		/// <returns>byte</returns>
		byte ToByte();
		/// <summary>
		/// Convert BinaryArray to Char
		/// </summary>
		/// <returns>char</returns>
		char ToChar();
		/// <summary>
		/// Convert BinaryArray to DateTime
		/// </summary>
		/// <returns>DateTime</returns>
		DateTime ToDateTime();
		/// <summary>
		/// Convert BinaryArray to decimal
		/// </summary>
		/// <returns>decimal</returns>
		Decimal ToDecimal();
		/// <summary>
		/// Convert BinaryArray to double
		/// </summary>
		/// <returns>double</returns>
		double ToDouble();
		/// <summary>
		/// Convert to Int16
		/// </summary>
		/// <returns>Int16</returns>
		Int16 ToInt16();
		/// <summary>
		/// Convert BinaryArray to Int32
		/// </summary>
		/// <returns>Int32</returns>
		Int32 ToInt32();
		/// <summary>
		/// Convert BinaryArray to Int64
		/// </summary>
		/// <returns>Int64</returns>
		Int64 ToInt64();
		/// <summary>
		/// Convert to UInt16
		/// </summary>
		/// <returns>UInt16</returns>
		UInt16 ToUInt16();
		/// <summary>
		/// Convert BinaryArray to Int32
		/// </summary>
		/// <returns>Int32</returns>
		UInt32 ToUInt32();
		/// <summary>
		/// Convert BinaryArray to Int64
		/// </summary>
		/// <returns>Int64</returns>
		UInt64 ToUInt64();
		/// <summary>
		/// Convert to sbyte
		/// </summary>
		/// <returns>sbyte</returns>
		sbyte ToSByte();
		/// <summary>
		/// Convert to float
		/// </summary>
		/// <returns>float</returns>
		float ToSingle();
		/// <summary>
		/// Convert BinaryArray to Guid
		/// </summary>
		/// <returns>Guid</returns>
		Guid ToGuid();
		/// <summary>
		/// Convert to UTF8
		/// </summary>
		/// <param name="utf8">reference to result array</param>
		void ToUTF8(Byte[] utf8);
		/// <summary>
		/// Convert BinaryArray to UUID
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>UUID</returns>
		UUID ToUUID(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Boolean
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Boolean</returns>
		Boolean ToBoolean(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Byte
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Byte</returns>
		Byte ToByte(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Char
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Char</returns>
		Char ToChar(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to DateTime
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>DateTime</returns>
		DateTime ToDateTime(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Decimal
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Decimal</returns>
		Decimal ToDecimal(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Double
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Double</returns>
		Double ToDouble(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Int16
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int16</returns>
		Int16 ToInt16(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Int32
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int32</returns>
		Int32 ToInt32(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Int64
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int64</returns>
		Int64 ToInt64(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to UInt16
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>UInt16</returns>
		UInt16 ToUInt16(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to UInt32
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>UInt32</returns>
		UInt32 ToUInt32(UInt32 offset);

		/// <summary>
		/// Convert BinaryArray to UInt64
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Int64</returns>
		UInt64 ToUInt64(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to SByte
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>SByte</returns>
		SByte ToSByte(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Single
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Single</returns>
		Single ToSingle(Int32 offset);
		/// <summary>
		/// Convert BinaryArray to Guid
		/// </summary>
		/// <param name="offset">Offset.</param>
		/// <returns>Guid</returns>
		Guid ToGuid(Int32 offset);
		/// <summary>
		/// Convert to UTF8
		/// </summary>
		/// <param name="utf8">reference to result array</param>
		/// <param name="offset">offset</param>
		void ToUTF8(Byte[] utf8, Int32 offset);
		/// <summary>
		/// Convert to MutableString
		/// </summary>
		/// <param name="str">reference to result</param>
		/// <param name="offset">offset</param>
		void ToMutableString(IMutableString str, Int32 offset = 0);
		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		IBinaryConvertibleReadWrite Clone();
		/// <summary>
		/// Copies to
		/// </summary>
		/// <param name="ptr">destination</param>
		unsafe void CopyTo(byte* ptr);
		/// <summary>
		/// Copies to
		/// </summary>
		/// <param name="str">destination</param>
		void CopyTo(IBinaryConvertibleReadWrite str);
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="another"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="another">An object to compare with this object.</param>
		Boolean Equals(IBinaryConvertibleReadOnly another);

		/// <summary>
		/// Convert to byte array
		/// </summary>
		/// <returns>byte array</returns>
		Byte[] ToByteArray();
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <param name="isASCII">is it string ASCII</param>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		String ToString(bool isASCII = false);
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <param name="offset">Start offset in binary array.</param>
		/// <returns>New string, representing this binary array.</returns>
		IReadOnlyString ToMutableString(int offset = 0);

		/// <summary>
		/// Convert BinaryArray to byte span.
		/// </summary>
		/// <param name="destination">Destination byte span</param>
		void ToUTF8(Span<byte> destination);
		/// <summary>
		/// Convert BinaryArray to byte span.
		/// </summary>
		/// <param name="sourceIndex">Start offset in binary array</param>
		/// <param name="destination">Destination byte span</param>
		void ToByteArray(Int32 sourceIndex, Span<byte> destination);

		/// <summary>
		/// Convert BinaryArray to byte span.
		/// </summary>
		/// <param name="sourceIndex">Start offset in binary array</param>
		/// <param name="destination">Destination sbyte span</param>

		void ToSByteArray(Int32 sourceIndex, Span<sbyte> destination);

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
		unsafe Byte this[Int32 index]
		{
			get;
		}

	}
}
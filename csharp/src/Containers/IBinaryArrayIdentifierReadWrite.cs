using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Interface for BinaryIdentifier
	/// </summary>
	public interface IBinaryIdentifierReadWrite : IBinaryIdentifierReadOnly, IBinaryArrayReadWrite
	{
		/// <summary>
		/// Assign BinaryArray by string.
		/// </summary>
		/// <param name="str">Source string.</param>
		/// <param name="isASCII">True if string is ASCII.</param>
		/// <returns>BinaryArray after assign.</returns>
		IBinaryIdentifierReadWrite Assign(String str, bool isASCII);
		/// <summary>
		/// Append BinaryArray by string.
		/// </summary>
		/// <param name="str">Source string.</param>
		/// <param name="isASCII">True if string is ASCII.</param>
		/// <returns>BinaryArray after append.</returns>
		IBinaryIdentifierReadWrite Append(String str, bool isASCII);
		/// <summary>
		/// Assign BinaryArray by long.
		/// </summary>
		/// <param name="buffer">Source long.</param>
		/// <returns>BinaryArray after assign.</returns>
		IBinaryIdentifierReadWrite Assign(long buffer);
		/// <summary>
		/// Append BinaryArray by long.
		/// </summary>
		/// <param name="buffer">Source long.</param>
		/// <returns>BinaryArray after append.</returns>
		IBinaryIdentifierReadWrite Append(long buffer);
		/// <summary>
		/// Assign BinaryArray by byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <returns>BinaryArray after assign.</returns>
		IBinaryIdentifierReadWrite Assign(byte[] bytes);
		/// <summary>
		/// Append BinaryArray by byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <returns>BinaryArray after append.</returns>
		IBinaryIdentifierReadWrite Append(byte[] bytes);

		/// <summary>
		/// Assign BinaryArray by byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <param name="offset">Offset in source array.</param>
		/// <param name="count">Count of bytes to assign.</param>
		/// <returns>BinaryArray after assign.</returns>

		IBinaryIdentifierReadWrite Assign(byte[] bytes, int offset, int count);
		/// <summary>
		/// Append BinaryArray by byte array.
		/// </summary>
		/// <param name="bytes">Source byte array.</param>
		/// <param name="offset">Offset in source array.</param>
		/// <param name="count">Count of bytes to assign.</param>
		/// <returns>BinaryArray after append.</returns>

		IBinaryIdentifierReadWrite Append(byte[] bytes, int offset, int count);
		/// <summary>
		/// Assign BinaryArray by another BinaryArray.
		/// </summary>
		/// <param name="str">BinaryArray to assign.</param>
		/// <returns>BinaryArray after assign.</returns>
		IBinaryIdentifierReadWrite Assign(IBinaryArrayReadOnly str);
		/// <summary>
		/// Append BinaryArray by another BinaryArray.
		/// </summary>
		/// <param name="str">BinaryArray to append.</param>
		/// <returns>BinaryArray after append.</returns>

		IBinaryIdentifierReadWrite Append(IBinaryArrayReadOnly str);

		/*TODO: Strictly recommended to override*/
		/// <summary>
		/// Return clone of BinaryArray.
		/// </summary>
		/// <returns>Clone of BinaryArray.</returns>
		new IBinaryIdentifierReadWrite Clone();
		/// <summary>
		/// Set byte at index.
		/// </summary>
		/// <param name="index">Index of byte.</param>
		/// <param name="x">New value of byte.</param>
		/// <returns>BinaryArray after change.</returns>
		IBinaryIdentifierReadWrite Set(int index, byte x);
		/// <summary>
		/// Append byte to BinaryArray.
		/// </summary>
		/// <param name="b">Byte to append.</param>
		/// <returns>BinaryArray after appending.</returns>
		IBinaryIdentifierReadWrite Append(byte b);
		/// <summary>
		/// Clear BinaryArray.
		/// </summary>
		/// <returns>BinaryArray after clearing.</returns>
		IBinaryIdentifierReadWrite Clear();

	}
}

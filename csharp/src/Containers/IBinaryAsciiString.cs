using System;
using System.Collections.Generic;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Mutable interface for BinaryAsciiString.
	/// </summary>
	public interface IBinaryAsciiString : IReadOnlyString
	{


		/// <summary>
		/// Append hex-representation of long to string 
		/// </summary>
		/// <param name="i">Item to append.</param>
		/// <returns>This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString AppendFastHex(long i);

		/// <summary>
		/// Append hex-representation of UUID to string.
		/// </summary>
		/// <param name="i">Item to append.</param>
		/// <returns>This string after appending.</returns>
		IBinaryAsciiString AppendFastHex(UUID i);


		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(Object item);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(Object item);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(IBinaryArrayReadOnly item);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(IBinaryArrayReadOnly item);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(bool item);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(IReadOnlyString cs);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(char c);



		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(short i);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(int i);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(long i);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(char[] str);

		/// <summary>
		/// Append item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <param name="offset">Offset of array.</param>
		/// <param name="count">Count of elements.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(char[] str, int offset, int count);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(double i);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(UUID uuid);

		/// <summary>
		/// Append this item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to append.</param>
		/// <param name="format">Format of item to assign</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Append(UUID uuid, UUIDPrintFormat format);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(bool item);


		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(IReadOnlyString cs);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(char c);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(short i);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(double i);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(int i);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(long i);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(char[] str);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <param name="offset">Offset of array.</param>
		/// <param name="count">Count of elements.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(char[] str, int offset, int count);



		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(UUID uuid);

		/// <summary>
		/// Assign item to string. Throw exception if item contains non-ascii characters. 
		/// </summary>
		/// <param name="item">Item to assign.</param>
		/// <param name="format">Format of uuid to assign</param>
		/// <returns> This IBinaryAsciiString after appending.</returns>
		IBinaryAsciiString Assign(UUID uuid, UUIDPrintFormat format);


		/// <summary>
		/// Clear this string.
		/// </summary>
		/// <returns>This string after clean.</returns>
		IBinaryAsciiString Clear();

		/// <summary>
		/// Copies to
		/// </summary>
		/// <param name="str">destination</param>
		void CopyTo(BinaryAsciiString str);

		/// <summary>
		/// Copies from
		/// </summary>
		/// <param name="str">destination</param>
		IBinaryAsciiString CopyFrom(IReadOnlyString str);

		/// <summary>
		/// Get element by index
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>element by index.</returns>
		new unsafe char this[Int32 index]
		{
			get;
			set;
		}

		/// <summary>
		/// Append chars to MutableString
		/// </summary>
		/// <param name="c">chars</param>
		/// <returns>this</returns>
		IBinaryAsciiString Append(ReadOnlySpan<char> c);

		/// <summary>
		/// Assign MutableString by chars
		/// </summary>
		/// <param name="c">Chars</param>
		/// <returns>this</returns>
		IBinaryAsciiString Assign(ReadOnlySpan<char> c);

		/// <summary>
		/// Transforms this string to lower case
		/// </summary>
		/// <returns>this string in lower case</returns>
		IBinaryAsciiString ToLowerCase();

		/// <summary>
		/// Transforms this string to upper case
		/// </summary>
		/// <returns>this string in upper case</returns>
		IBinaryAsciiString ToUpperCase();

	}
}

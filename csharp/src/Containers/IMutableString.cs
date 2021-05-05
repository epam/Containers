using EPAM.Deltix.Time;
using System;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Interface for mutable string
	/// </summary>
	public interface IMutableString : IReadOnlyString
	{
		/// <summary>
		/// Append the specified object.
		/// </summary>
		/// <param name="item">Specified object.</param>
		/// <returns>The string.</returns>
		IMutableString Append(Object item);
		/// <summary>
		/// Append the specified decimal.
		/// </summary>
		/// <param name="item">Specified decimal.</param>
		/// <returns>The string.</returns>
		IMutableString Append(decimal item);
		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString Append(StringBuilder str);
		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString Append(IReadOnlyString str);
		/// <summary>
		/// Appends the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString Append(String str);
		/// <summary>
		/// Appends the specified char value.
		/// </summary>
		/// <param name="c">The char value.</param>
		IMutableString Append(Char c);
		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString Append(Int16 i);
		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString Append(Int32 i);
		/// <summary>
		/// Appends the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString Append(Int64 i);
		/// <summary>
		/// Appends the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <returns>Self instance.</returns>
		IMutableString Append(Char[] str);
		/// <summary>
		/// Appends the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <param name="offset">Offset in char array.</param>
		/// <param name="count">Number of chars to copy</param>
		/// <returns>Self instance.</returns>
		IMutableString Append(Char[] str, Int32 offset, Int32 count);
		/// <summary>
		/// Appends the specified string in form of pointer to char array.
		/// </summary>
		/// <param name="bytes">String in form of char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString Append(Char* bytes, Int32 count);
		/// <summary>
		/// Appends the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString AppendUTF8(Byte[] bytes);
		/// <summary>
		/// Appends the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString AppendUTF8(Byte[] bytes, Int32 offset, Int32 count);
		/// <summary>
		/// Appends the specified UTF8 string in form of pointer to byte array.
		/// </summary>
		/// <param name="bytes">UTF8 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString AppendUTF8(Byte* bytes, Int32 count);
		/// <summary>
		/// Appends the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString AppendUTF16(Byte[] bytes);
		/// <summary>
		/// Appends the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString AppendUTF16(Byte[] bytes, Int32 offset, Int32 count);
		/// <summary>
		/// Appends the specified UTF16 string in form of pointer to byte array.
		/// </summary>
		/// <param name="bytes">UTF16 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString AppendUTF16(Byte* bytes, Int32 count);
		/// <summary>
		/// Appends hexidecimal representation on specified <see cref="BinaryArray"/> to this string.
		/// </summary>
		/// <param name="binaryArray">Binary array to append.</param>
		/// <returns>Self instance.</returns>
		IMutableString Append(IBinaryConvertibleReadOnly binaryArray);
		/// <summary>
		/// Appends specified <see cref="UUID"/> in specified <see cref="UUIDPrintFormat"/> to this string.
		/// </summary>
		/// <param name="uuid">Universally unique identifier.</param>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>Self instance.</returns>
		IMutableString Append(UUID uuid, UUIDPrintFormat format = UUIDPrintFormat.UpperCase);
		/// <summary>
		/// Appends the specified <see cref="DateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="DateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString Append(DateTime item);
		/// <summary>
		/// Appends the specified <see cref="TimeSpan"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="TimeSpan"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString Append(TimeSpan item);
		/// <summary>
		/// Appends the specified bool to this string.
		/// </summary>
		/// <param name="item">bool to append.</param>
		/// <returns>Self instance</returns>
		IMutableString Append(bool item);
		/// <summary>
		/// Appends the specified double to this string.
		/// </summary>
		/// <param name="item">double to append.</param>
		/// <returns>Self instance</returns>
		IMutableString Append(double item);
		/// <summary>
		/// Appends the specified float to this string.
		/// </summary>
		/// <param name="item">float to append.</param>
		/// <returns>Self instance</returns>
		IMutableString Append(float item);




		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString Assign(StringBuilder str);
		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString Assign(IReadOnlyString str);
		/// <summary>
		/// Assigns the specified string.
		/// </summary>
		/// <param name="str">The string.</param>
		IMutableString Assign(String str);
		/// <summary>
		/// Assigns the specified char.
		/// </summary>
		/// <param name="c">The char value.</param>
		IMutableString Assign(Char c);
		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString Assign(Int16 i);
		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString Assign(Int32 i);
		/// <summary>
		/// Assigns the specified integer value.
		/// </summary>
		/// <param name="i">The integer value.</param>
		IMutableString Assign(Int64 i);
		/// <summary>
		/// Assigns the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(Char[] str);
		/// <summary>
		/// Assigns the specified string in form of char array.
		/// </summary>
		/// <param name="str">String in form of char array.</param>
		/// <param name="offset">Offset in char array.</param>
		/// <param name="count">Number of chars to copy</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(Char[] str, Int32 offset, Int32 count);
		/// <summary>
		/// Assigns the specified string in form of pointer to char array.
		/// </summary>
		/// <param name="str">String in form of pointer to char array.</param>
		/// <param name="count">Number of chars to copy.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString Assign(Char* str, Int32 count);

		/// <summary>
		/// Assigns the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString AssignUTF8(Byte[] utf8);
		/// <summary>
		/// Assigns the specified UTF8  string in form of byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString AssignUTF8(Byte[] utf8, Int32 offset, Int32 count);

		/// <summary>
		/// Assigns the specified UTF8 string in form of pointer to byte array.
		/// </summary>
		/// <param name="utf8">UTF8 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString AssignUTF8(Byte* utf8, Int32 count);

		/// <summary>
		/// Assigns the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of byte array.</param>
		/// <returns>Self instance.</returns>
		IMutableString AssignUTF16(Byte[] utf16);

		/// <summary>
		/// Assigns the specified UTF16  string in form of byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of byte array.</param>
		/// <param name="offset">Offset in byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		IMutableString AssignUTF16(Byte[] utf16, Int32 offset, Int32 count);

		/// <summary>
		/// Assigns the specified UTF16 string in form of pointer to byte array.
		/// </summary>
		/// <param name="utf16">UTF16 String in form of pouinter to byte array.</param>
		/// <param name="count">Number of bytes to decode.</param>
		/// <returns>Self instance.</returns>
		unsafe IMutableString AssignUTF16(Byte* utf16, Int32 count);

		/// <summary>
		/// Assigns string representation on specified <see cref="BinaryArray"/>.
		/// </summary>
		/// <param name="binaryArray">Binary array to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(IBinaryConvertibleReadOnly binaryArray);
		/// <summary>
		/// Assigns string representation on specified <see cref="UUID"/>,
		/// using specified <see cref="UUIDPrintFormat"/>.
		/// </summary>
		/// <param name="uuid">Universally unique identifier.</param>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(UUID uuid, UUIDPrintFormat format = UUIDPrintFormat.UpperCase);
		/// <summary>
		/// Assigns string representation on specified <see cref="DateTime"/>.
		/// </summary>
		/// <param name="item">DateTime to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(DateTime item);
		/// <summary>
		/// Assigns string representation on specified <see cref="TimeSpan"/>.
		/// </summary>
		/// <param name="item">TimeSpan to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(TimeSpan item);
		/// <summary>
		/// Assigns the specified bool value.
		/// </summary>
		/// <param name="item">The bool value.</param>
		IMutableString Assign(bool item);
		/// <summary>
		/// Assigns the specified double value.
		/// </summary>
		/// <param name="item">The double value.</param>
		IMutableString Assign(double item);
		/// <summary>
		/// Assigns the specified float value.
		/// </summary>
		/// <param name="item">The float value.</param>
		IMutableString Assign(float item);

		/// <summary>
		/// Assign the specified object.
		/// </summary>
		/// <param name="item">Specified object.</param>
		/// <returns>The string.</returns>
		IMutableString Assign(Object item);
		/// <summary>
		/// Assign the specified decimal.
		/// </summary>
		/// <param name="item">Specified decimal.</param>
		/// <returns>The string.</returns>
		IMutableString Assign(decimal item);



		/// <summary>
		/// Number of elementes
		/// </summary>
		new int Length
		{
			get;
			set;
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
		new Char this[Int32 index]
		{
			get;
			set;
		}

		/// <summary>
		/// Clears this instance and doesn't clear internal buffer.
		/// </summary>
		IMutableString Clear();

		/// <summary>
		/// Clears this instance and clear internal buffer.
		/// </summary>
		IMutableString SecureClear();

		/// <summary>
		/// Copy From
		/// </summary>
		/// <param name="source">source</param>
		/// <returns>this</returns>
		IMutableString CopyFrom(IReadOnlyString source);
		/// <summary>
		/// Copy From
		/// </summary>
		/// <param name="source">source</param>
		/// <returns>this</returns>
		IMutableString CopyFrom(String source);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString Insert(int index, char item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString Insert(int index, String item);

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString Insert(int index, IReadOnlyString item);

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString Insert(int index, StringBuilder item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>link to this</returns>
		IMutableString Insert(int index, Char[] item);

		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IMutableString Insert(int index, Char[] item, int offset, int count);
		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe IMutableString Insert(int index, Char* item, int count);
		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IMutableString Insert(int index, Int64 item);
		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IMutableString Insert(int index, Int32 item);
		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IMutableString Insert(int index, Int16 item);

		/// <summary>
		/// Removes whitespaces from the start and the end of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		IMutableString Trim();
		/// <summary>
		/// Removes whitespaces from the start of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		IMutableString TrimLeft();
		/// <summary>
		/// Removes whitespaces from the end of current string and returns itself.
		/// </summary>
		/// <returns>this</returns>
		IMutableString TrimRight();

		/// <summary>
		/// Append chars to MutableString
		/// </summary>
		/// <param name="c">chars</param>
		/// <returns>this</returns>
		IMutableString Append(ReadOnlySpan<char> c);

		/// <summary>
		/// Assign MutableString by chars
		/// </summary>
		/// <param name="c">Chars</param>
		/// <returns>this</returns>
		IMutableString Assign(ReadOnlySpan<char> c);


		/// <summary>
		/// Insert chars to MutableString
		/// </summary>
		/// <param name="index">Index to insert</param>
		/// <param name="item">Chars to insert</param>
		/// <returns>this</returns>
		IMutableString Insert(int index, ReadOnlySpan<char> item);


		/// <summary>
		/// Appends the specified <see cref="HdDateTime"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="HdDateTime"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString Append(HdDateTime item);
		/// <summary>
		/// Appends the specified <see cref="HdTimeSpan"/> to this string.
		/// </summary>
		/// <param name="item"><see cref="HdTimeSpan"/> to append.</param>
		/// <returns>Self instance</returns>
		IMutableString Append(HdTimeSpan item);
		/// <summary>
		/// Assigns string representation on specified <see cref="HdTimeSpan"/>.
		/// </summary>
		/// <param name="item">HdTimeSpan to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(HdTimeSpan item);
		/// <summary>
		/// Assigns string representation on specified <see cref="HdDateTime"/>.
		/// </summary>
		/// <param name="item">HdDateTime to assign.</param>
		/// <returns>Self instance.</returns>
		IMutableString Assign(HdDateTime item);
		/// <summary>
		/// Transforms this string to lower case
		/// </summary>
		/// <returns>this string in lower case</returns>
		IMutableString ToLowerCase();

		/// <summary>
		/// Transforms this string to upper case
		/// </summary>
		/// <returns>this string in upper case</returns>
		IMutableString ToUpperCase();
	}
}

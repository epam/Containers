using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Interface for BinaryArray
	/// </summary>
	public interface IBinaryConvertibleReadWrite : IBinaryConvertibleReadOnly, IBinaryIdentifierReadWrite
	{


		/// <summary>
		/// Append ReadOnlySpan.
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(ReadOnlySpan<byte> bytes);

		/// <summary>
		/// Append ReadOnlySpan.
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(ReadOnlySpan<sbyte> bytes);

		/// <summary>
		/// Append ReadOnlySpan.
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(ReadOnlySpan<char> bytes);


		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(ReadOnlySpan<byte> bytes);

		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(ReadOnlySpan<sbyte> bytes);

		/// <summary>
		/// Assign by ReadOnlySpan
		/// </summary>
		/// <param name="bytes">ReadOnlySpan to append</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(ReadOnlySpan<char> bytes);


		/// <summary>
		/// Insert ReadOnlySpan to BinaryArray
		/// </summary>
		/// <param name="index">index to insert</param>
		/// <param name="item">Item to insert</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, ReadOnlySpan<byte> item);

		/// <summary>
		/// Insert ReadOnlySpan to BinaryArray
		/// </summary>
		/// <param name="index">index to insert</param>
		/// <param name="item">Item to insert</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, ReadOnlySpan<sbyte> item);

		/// <summary>
		/// Insert ReadOnlySpan to BinaryArray
		/// </summary>
		/// <param name="index">index to insert</param>
		/// <param name="item">Item to insert</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, ReadOnlySpan<char> item);



		/// <summary>
		/// Copy From
		/// </summary>
		/// <param name="source">source</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite CopyFrom(IBinaryArrayReadOnly source);
		/// <summary>
		/// Append UUID to this
		/// </summary>
		/// <param name="item">UUID</param>
		/// <returns>binary array</returns>
		IBinaryConvertibleReadWrite Append(UUID item);
		/// <summary>
		/// Append MutableString to this
		/// </summary>
		/// <param name="str">MutableString</param>
		/// <returns>binary array</returns>
		IBinaryConvertibleReadWrite Append(IReadOnlyString str, bool isASCII);
		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(Byte[] bytes);

		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(Byte[] bytes, int offset, int count);
		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(SByte[] bytes);
		/// <summary>
		/// Append Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(SByte[] bytes, int offset, int count);
		/// <summary>
		/// Append binary array to this
		/// </summary>
		/// <param name="str">Binary array</param>
		/// <returns>new binary array</returns>
		IBinaryConvertibleReadWrite Append(IBinaryConvertibleReadOnly str);
		/// <summary>
		/// Append array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		unsafe IBinaryConvertibleReadWrite Append(byte* buffer, int size);
		/// <summary>
		/// Append array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		unsafe IBinaryConvertibleReadWrite Append(sbyte* buffer, int size);
		/// <summary>
		/// Append a byte
		/// </summary>
		/// <param name="buffer">byte</param>
		/// <returns>new binary array</returns>
		IBinaryConvertibleReadWrite Append(byte buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Boolean buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(char buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(DateTime buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Decimal buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Double buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Single buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Guid buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Int16 buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Int32 buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(Int64 buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(UInt16 buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(UInt32 buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(UInt64 buffer);
		/// <summary>
		/// Append buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Append(SByte buffer);
		/// <summary>
		/// Append string to this BinaryArray
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>this BinaryArray</returns>
		/// <param name="isASCII">is it string ASCII</param>
		IBinaryConvertibleReadWrite Append(string str, bool isASCII = false);
		/// <summary>
		/// Append Enumerable of byte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of byte</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(IEnumerable<byte> enumerable);
		/// <summary>
		/// Append Enumerable of sbyte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of sbyte</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Append(IEnumerable<sbyte> enumerable);
		///////////////////////
		/// <summary>
		/// Assign UUID to this
		/// </summary>
		/// <param name="item">UUID</param>
		/// <returns>binary array</returns>
		IBinaryConvertibleReadWrite Assign(UUID item);
		/// <summary>
		/// Assign MutableString to this
		/// </summary>
		/// <param name="str">MutableString</param>
		/// <param name="isASCII">is IReadOnlyString ASCII or no</param>
		/// <returns>binary array</returns>
		IBinaryConvertibleReadWrite Assign(IReadOnlyString str, bool isASCII);
		/// <summary>
		/// Assign Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(Byte[] bytes);

		/// <summary>
		/// Assign Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(Byte[] bytes, int offset, int count);
		/// <summary>
		/// Assign Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(SByte[] bytes);
		/// <summary>
		/// Assign Byte Array to BinaryArray
		/// </summary>
		/// <param name="bytes">Byte Array</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(SByte[] bytes, int offset, int count);
		/// <summary>
		/// Assign binary array to this
		/// </summary>
		/// <param name="str">Binary array</param>
		/// <returns>new binary array</returns>
		IBinaryConvertibleReadWrite Assign(IBinaryConvertibleReadOnly str);
		/// <summary>
		/// Assign array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		unsafe IBinaryConvertibleReadWrite Assign(byte* buffer, int size);
		/// <summary>
		/// Assign array of byte 
		/// </summary>
		/// <param name="buffer">pointer to begin</param>
		/// <param name="size">size</param>
		/// <returns>new binary array</returns>
		unsafe IBinaryConvertibleReadWrite Assign(sbyte* buffer, int size);
		/// <summary>
		/// Assign a byte
		/// </summary>
		/// <param name="buffer">byte</param>
		/// <returns>new binary array</returns>
		IBinaryConvertibleReadWrite Assign(byte buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Boolean buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(char buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(DateTime buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Decimal buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Double buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Single buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Guid buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Int16 buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Int32 buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(Int64 buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(UInt16 buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(UInt32 buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(UInt64 buffer);
		/// <summary>
		/// Assign buffer to this BinaryArray
		/// </summary>
		/// <param name="buffer">buffer</param>
		/// <returns>this BinaryArray</returns>
		IBinaryConvertibleReadWrite Assign(SByte buffer);
		/// <summary>
		/// Assign string to this BinaryArray
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>this BinaryArray</returns>
		/// <param name="isASCII">is it string ASCII</param>
		IBinaryConvertibleReadWrite Assign(string str, bool isASCII = false);
		/// <summary>
		/// Assign Enumerable of byte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of byte</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(IEnumerable<byte> enumerable);
		/// <summary>
		/// Assign Enumerable of sbyte to BinaryArray
		/// </summary>
		/// <param name="enumerable">enumerable of sbyte</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Assign(IEnumerable<sbyte> enumerable);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, byte item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Int16 item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, UInt16 item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Char item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Int32 item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, UInt32 item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Int64 item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, UInt64 item);

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Guid item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Double item);

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, DateTime item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Single item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, IReadOnlyString item);

		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Byte[] item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Byte[] item, int offset, int count);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, SByte[] item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="offset">offset</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, SByte[] item, int offset, int count);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Decimal item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, Boolean item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, IBinaryConvertibleReadOnly item);
		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe IBinaryConvertibleReadWrite Insert(int index, byte* item, int count);
		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="count">count</param>
		/// <returns>this</returns>
		unsafe IBinaryConvertibleReadWrite Insert(int index, sbyte* item, int count);
		/// <summary>
		/// Insert item to index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <param name="isASCII">isASCII</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, string item, bool isASCII = false);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, IEnumerable<Byte> item);
		/// <summary>
		/// Insert item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <param name="item">item</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite Insert(int index, IEnumerable<SByte> item);
		/// <summary>
		/// Remove item at index
		/// </summary>
		/// <param name="index">index</param>
		/// <returns>this</returns>
		IBinaryConvertibleReadWrite RemoveAt(int index);
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
		new unsafe Byte this[Int32 index]
		{
			get;
			set;
		}
		/// <summary>
		/// Clears this instance.
		/// </summary>
		IBinaryConvertibleReadWrite Clear();

		/// <summary>
		/// Gets or sets the capacity.
		/// </summary>
		/// <value>
		/// The capacity.
		/// </value>
		new Int32 Capacity { get; set; }
	}
}

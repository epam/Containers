package com.epam.deltix.containers.interfaces;

import java.io.InputStream;

/**
 * Created by VavilauA on 4/7/2017.
 */
public interface BinaryConvertibleReadWrite extends BinaryIdentifierReadWrite, BinaryConvertibleReadOnly{
    /**
     * Append byte-representation of item to BinaryArray.
     * @param str Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(ReadOnlyString str);

    /**
     * Append byte-representation of item to BinaryArray.
     * @param bytes Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(byte[] bytes);

    /**
     * Append byte-representation of count bytes of item(byte array) started from offset to BinaryArray.
     * @param bytes Item to append.
     * @param offset Offset.
     * @param count Count.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(byte[] bytes, int offset, int count);

    /**
     * Append byte-representation of item to BinaryArray.
     * @param str Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(BinaryArrayReadOnly str);

    /**
     * Append byte-representation of item to BinaryArray.
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(byte buffer);

    /**
     * Append byte-representation of item to BinaryArray.
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(boolean buffer);

    /**
     * Append byte-representation of item to BinaryArray.
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(char buffer);

    /**
     * Append byte-representation of item to BinaryArray.
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(double buffer);

    /**
     * Append byte-representation of item to BinaryArray.
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(float buffer);

    /**
     * Append byte-representation of item to BinaryArray
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(InputStream buffer);

    /**
     * Append byte-representation of item to BinaryArray
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(short buffer);

    /**
     * Append byte-representation of item to BinaryArray
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(int buffer);

    /**
     * Append byte-representation of item to BinaryArray
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(long buffer);

    /**
     * Append byte-representation of item to BinaryArray
     * @param str Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(String str);

    /**
     * Append byte-representation of item(ASCII/non-ASCII string) to BinaryArray
     * @param str Item to append.
     * @param isASCII ASCII/non-ASCII
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(String str, boolean isASCII);

    /**
     * Append byte-representation of item to BinaryArray
     * @param buffer Item to append.
     * @return This BinaryArray after appending.
     */
    BinaryConvertibleReadWrite append(UUIDReadOnly buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param str Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(ReadOnlyString str);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param bytes Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(byte[] bytes);

    /**
     * Assign this BinaryArray by byte-representation of count bytes of item(byte array) started from offset
     * @param bytes Item to assign.
     * @param offset Offset in bytes of source array.
     * @param count Count of bytes which need to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(byte[] bytes, int offset, int count);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param str Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite copyFrom(BinaryArrayReadOnly str);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param str Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(BinaryArrayReadOnly str);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(byte buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(boolean buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(char buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(double buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(float buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(short buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(UUIDReadOnly buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(int buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(long buffer);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param str Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(String str);

    /**
     * Assign this BinaryArray by byte-representation of item
     * @param buffer Item to assign.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(InputStream buffer);

    /**
     * Assign this BinaryArray by byte-representation of item(ASCII/non-ASCII string)
     * @param str Item to assign.
     * @param isASCII Flag ASCII/non-ASCII for string to append.
     * @return This BinaryArray after assigning.
     */
    BinaryConvertibleReadWrite assign(String str, boolean isASCII);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, byte item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, short item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, char item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, int item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, long item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, double item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, float item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, ReadOnlyString item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, byte[] item);

    /**
     * Insert count bytes from item(byte array) started from offset to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @param offset Offset of item to insert.
     * @param count Count of bytes in item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, byte[] item, int offset, int count);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, boolean item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, BinaryArrayReadOnly item);

    /**
     * Insert byte-representation of item to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, String item);

    /**
     * Insert byte-representation of ASCII/non-ASCII string to index of BinaryArray.
     * @param index Index of place for insert.
     * @param item Item to insert.
     * @param isASCII ASCII/non-ASCII flag.
     * @return This BinaryArray after inserting.
     */
    BinaryConvertibleReadWrite insert(int index, String item, boolean isASCII);

    /**
     * Remove byte with index from BinaryArray.
     * @param index Index of removing byte.
     * @return This BinaryArray after removing.
     */
    BinaryConvertibleReadWrite removeAt(int index);

    /**
     * Set value to byte with index.
     * @param index Index of byte.
     * @param x Value to setting.
     */
    void setByteAt(int index, byte x);

    /**
     * Remove all bytes from with BinaryArray.
     * @return This BinaryArray after clearing.
     */
    BinaryConvertibleReadWrite clear();

    @Override
    BinaryConvertibleReadWrite clone();
}

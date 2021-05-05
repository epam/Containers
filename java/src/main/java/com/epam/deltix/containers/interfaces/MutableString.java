package com.epam.deltix.containers.interfaces;

public interface MutableString extends ReadOnlyString, Appendable {
    /**
     * Append item to string
     *
     * @param item Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(boolean item);

    /**
     * Append item to string
     *
     * @param item Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(float item);

    /**
     * Append item to string
     *
     * @param str Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(StringBuilder str);

    /**
     * Append item to string
     *
     * @param str Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(ReadOnlyString str);

    /**
     * Append item to string
     *
     * @param str Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(String str);

    /**
     * Append item to string
     *
     * @param cs Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(CharSequence cs);

    /**
     * Append item to string
     *
     * @param c Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(char c);

    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(short i);

    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(int i);

    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(long i);

    /**
     * Append item to string
     *
     * @param str Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(char[] str);

    /**
     * Append count chars from item(char-array) started from offset
     *
     * @param str    Item to append.
     * @param offset offset
     * @param count  count
     * @return This MutableString after appending.
     */
    MutableString append(char[] str, int offset, int count);

    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(double i);

    /**
     * Append item to string, using {@code UUIDPrintFormat.UPPERCASE}.
     *
     * @param uuid Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(UUIDReadOnly uuid);

    /**
     * Append item to string, using specified {@code UUIDPrintFormat}.
     *
     * @param uuid   Item to append.
     * @param format Format of result string.
     * @return This MutableString after appending.
     */
    MutableString append(UUIDReadOnly uuid, UUIDPrintFormat format);

    /**
     * Append item to string.
     *
     * @param binaryArray Item to append.
     * @return This MutableString after appending.
     */
    MutableString append(BinaryConvertibleReadOnly binaryArray);

    /**
     * Append item to string
     *
     * @param bytes Item to append.
     * @return This MutableString after appending.
     */
    MutableString appendUTF8(byte[] bytes);

    /**
     * Append item(utf8-array) started from offset
     *
     * @param bytes  Item to append.
     * @param offset offset
     * @return This MutableString after appending.
     */
    MutableString appendUTF8(byte[] bytes, int offset);

    /**
     * Append item(utf8-array) started from offset and contains count bytes
     *
     * @param bytes  Item to append.
     * @param offset offset
     * @param count  count
     * @return This MutableString after appending.
     */
    MutableString appendUTF8(byte[] bytes, int offset, int count);

    /**
     * Assign string by item
     *
     * @param item Item to assign.
     * @return this string
     */
    MutableString assign(boolean item);

    /**
     * Assign string by item
     *
     * @param item Item to assign.
     * @return this string
     */
    MutableString assign(float item);

    /**
     * Assign string by item
     *
     * @param str Item to assign.
     * @return this string
     */
    MutableString assign(StringBuilder str);

    /**
     * Assign string by item
     *
     * @param str Item to assign.
     * @return this string
     */
    MutableString assign(ReadOnlyString str);

    /**
     * Assign string by item
     *
     * @param str Item to assign.
     * @return this string
     */
    MutableString assign(String str);

    /**
     * Assign string by item
     *
     * @param cs Item to assign.
     * @return this string
     */
    MutableString assign(CharSequence cs);

    /**
     * Assign string by item
     *
     * @param c Item to assign.
     * @return this string
     */
    MutableString assign(char c);

    /**
     * Assign string by item
     *
     * @param i Item to assign.
     * @return this string
     */
    MutableString assign(short i);

    /**
     * Assign string by item
     *
     * @param i Item to assign.
     * @return this string
     */
    MutableString assign(int i);

    /**
     * Assign string by item
     *
     * @param i Item to assign.
     * @return this string
     */
    MutableString assign(long i);

    /**
     * Assign string by item
     *
     * @param str Item to assign.
     * @return this string
     */
    MutableString assign(char[] str);

    /**
     * Assign this string by count chars from item(char-array) started from offset
     *
     * @param str    Item to assign.
     * @param offset offset
     * @param count  count
     * @return this string
     */
    MutableString assign(char[] str, int offset, int count);

    /**
     * Assign this string by item
     *
     * @param utf8 Item to assign.
     * @return this string
     */
    MutableString assignUTF8(byte[] utf8);

    /**
     * Assign this string by item(utf8-array) started from offset and contains count bytes
     *
     * @param utf8   Item to assign.
     * @param offset offset
     * @param count  count
     * @return this string
     */
    MutableString assignUTF8(byte[] utf8, int offset, int count);

    /**
     * Assign this string by item
     *
     * @param d Item to assign.
     * @return this string
     */
    MutableString assign(double d);

    /**
     * Assign this string by item, using specified {@code UUIDPrintFormat.UPPERCASE} format.
     *
     * @param uuid Item to assign.
     * @return this string
     */
    MutableString assign(UUIDReadOnly uuid);

    /**
     * Assign this string by item, using specified {@code UUIDPrintFormat}.
     *
     * @param uuid   Item to assign.
     * @param format Format of result string.
     * @return this string
     */
    MutableString assign(UUIDReadOnly uuid, UUIDPrintFormat format);

    /**
     * Assign this string by item.
     *
     * @param binaryArray Item to assign.
     * @return this string
     */
    MutableString assign(BinaryConvertibleReadOnly binaryArray);

    /**
     * Set char with index in this string to ch
     *
     * @param ch    ch
     * @param index index
     */
    void setCharAt(char ch, int index);

    /**
     * Clear this string
     *
     * @return this item
     */
    MutableString clear();

    /**
     * Clear this string. Set length to zero and clear buffer.
     *
     * @return Cleared string.
     */
    MutableString secureClear();

    /**
     * Assign this string by another
     *
     * @param source another string
     * @return this string
     */
    MutableString copyFrom(CharSequence source);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  item
     * @return this string
     */
    MutableString insert(int index, char item);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  Item to insert.
     * @return this string
     */
    MutableString insert(int index, String item);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  Item to insert.
     * @return this string
     */
    MutableString insert(int index, ReadOnlyString item);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  Item to insert.
     * @return this string
     */
    MutableString insert(int index, StringBuilder item);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  Item to insert.
     * @return this string
     */
    MutableString insert(int index, char[] item);

    /**
     * Insert count bytes started from offset from item(char array) to index
     *
     * @param index  index
     * @param item   Item to insert.
     * @param offset offset
     * @param count  count
     * @return this string
     */
    MutableString insert(int index, char[] item, int offset, int count);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  Item to insert.
     * @return this string
     */
    MutableString insert(int index, long item);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  Item to insert.
     * @return this string
     */
    MutableString insert(int index, int item);

    /**
     * Insert item to index
     *
     * @param index index
     * @param item  Item to insert.
     * @return this string
     */
    MutableString insert(int index, short item);

    /**
     * Removes whitespaces from the start and the end of current string and returns itself.
     *
     * @return this string
     */
    MutableString trim();

    /**
     * Removes whitespaces from the start of current string and returns itself.
     *
     * @return this string
     */
    MutableString trimLeft();

    /**
     * Removes whitespaces from the end of current string and returns itself.
     *
     * @return this string
     */
    MutableString trimRight();

    /**
     * Transforms this string to lower case
     *
     * @return this string in lower case
     */
    MutableString toLowerCase();

    /**
     * Transforms this string to upper case
     *
     * @return this string in upper case
     */
    MutableString toUpperCase();
}

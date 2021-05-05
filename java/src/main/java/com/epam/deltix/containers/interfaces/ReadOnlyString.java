package com.epam.deltix.containers.interfaces;

public interface ReadOnlyString extends CharSequence {
    /**
     * Return substring started in char with index start and ended in char with index end
     *
     * @param start start
     * @param end   end
     * @return substring started in char with index start and ended in char with index end
     */
    ReadOnlyString subString(int start, int end);

    /**
     * Return true if one MutableString equals to another
     *
     * @param another another
     * @return true if one MutableString equals to another
     */
    Boolean equals(ReadOnlyString another);

    /**
     * Return true if one MutableString equals to another String
     *
     * @param another another
     * @return true if one MutableString equals to another String
     */
    Boolean equals(String another);

    /**
     * Returns number of UTF-8 characters, needed to represent this string.
     *
     * @return Number of characters in UTF-8 representation of this string.
     */
    int getUTF8ByteLength();

    /**
     * Return char with index index.
     *
     * @param index index
     * @return char with index index.
     */
    char getCharAt(int index);

    /**
     * Return int64-representation of this string(it's correct if this string is a number)
     *
     * @return int64-representation of this string(it's correct if this string is a number)
     */
    long toInt64();

    /**
     * Write utf8-representation of this string to utf8-array
     *
     * @param utf8   utf8-array
     * @param offset offset
     * @return number of bytes
     */
    int toUTF8(byte[] utf8, int offset);


    /**
     * Write utf8-representation of this string to utf8-array
     * @param charIndex Start index in source mutable string.
     * @param charCount Count of chars in source mutable string.
     * @param utf8 Destination array of bytes.
     * @param utf8Index Start index in destination byte array.
     * @return Number of written bytes.
     */
    int toUTF8(int charIndex, int charCount, byte[] utf8, int utf8Index);

    /**
     * Return java-string-representation of this string
     *
     * @return java-string-representation of this string
     */
    @Override
    String toString();

    /**
     * Returns binary representation of this string.
     *
     * @return new {@code BinaryArray} representation of this string.
     */
    BinaryConvertibleReadOnly toBinaryArray();

    /**
     * Create {@code UUID} from this string, using {@code UUIDParseFormat.ANY} format.
     *
     * @return New {@code UUID}.
     */
    UUIDReadOnly toUUID();

    /**
     * Create {@code UUID} from this string, using specified {@code UUIDParseFormat}.
     *
     * @param format this string allowed uuid format.
     * @return New {@code UUID}.
     */
    UUIDReadOnly toUUID(UUIDParseFormat format);

    /**
     * Assigns binary representation of this string to specified {@code BinaryArray}.
     *
     * @param binaryArray output {@code BinaryArray}, containing binary representation of this sting.
     */
    void toBinaryArray(BinaryConvertibleReadWrite binaryArray);

    /**
     * Write uuid-value of this string to uuid, using {@code UUIDParseFormat.ANY} format.
     *
     * @param uuid output uuid.
     */
    void toUUID(UUID uuid);

    /**
     * Write uuid-value of all bytes, started from offset to uuid, using {@code UUIDParseFormat.ANY} format.
     *
     * @param uuid   output uuid.
     * @param offset offset in this string, chars.
     */
    void toUUID(UUID uuid, int offset);

    /**
     * Write uuid-value of this string to uuid, using specified {@code UUIDParseFormat}.
     *
     * @param uuid   output uuid.
     * @param format this string allowed uuid format.
     */
    void toUUID(UUID uuid, UUIDParseFormat format);

    /**
     * Write uuid-value of all bytes, started from offset to uuid, using specified {@code UUIDParseFormat}.
     *
     * @param uuid   output uuid.
     * @param offset offset in this string, chars.
     * @param format this string allowed uuid format.
     */
    void toUUID(UUID uuid, int offset, UUIDParseFormat format);

    /**
     * Return clone of this string
     *
     * @return clone of this string
     */
    MutableString clone();

    /**
     * Copy this string to another string.
     *
     * @param str another string
     */
    void copyTo(MutableString str);

    /**
     * Copy count chars from this string started from offset to char array with offset
     *
     * @param sourceIndex      source offset
     * @param destination      destination array
     * @param destinationIndex destination offset
     * @param count            number of chars
     */
    void toCharArray(int sourceIndex, char[] destination, int destinationIndex, int count);

    /**
     * Checks if string is empty (i.e. it's length equals zero) and returns result of this check.
     *
     * @return {@code true}, if string is empty, {@code false} otherwise.
     */
    boolean isEmpty();
}


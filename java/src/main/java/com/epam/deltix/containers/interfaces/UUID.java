package com.epam.deltix.containers.interfaces;

public interface UUID extends UUIDReadOnly {


    /**
     * Try to assign this UUID by string and return false if string is invalid UUID.
     * @param string String to assign.
     * @return Return false if string is invalid UUID. True if it's valid.
     */
    boolean tryAssign(CharSequence string);

    /**
     * Try to assign this UUID by string and return false if string is invalid UUID.
     * @param string String to assign.
     * @param offset Offset in string.
     * @return Return false if string is invalid UUID. True if it's valid.
     */
    boolean tryAssign(CharSequence string, int offset);

    /**
     * Try to assign this UUID by string and return false if string is invalid UUID.
     * @param string String to assign.
     * @param format Format of UUID.
     * @return Return false if string is invalid UUID. True if it's valid.
     */
    boolean tryAssign(CharSequence string, UUIDParseFormat format);

    /**
     * Try to assign this UUID by string and return false if string is invalid UUID.
     * @param string String to assign.
     * @param offset Offset in string.
     * @param format Format of UUID.
     * @return Return false if string is invalid UUID. True if it's valid.
     */
    boolean tryAssign(CharSequence string, int offset, UUIDParseFormat format);

    /**
     * Set most significant bits.
     *
     * @param x value to set.
     */
    void setMSB(long x);

    /**
     * Set less significant bits.
     *
     * @param x value to set.
     */
    void setLSB(long x);

    /**
     * Assign this item by java-UUID.
     *
     * @param uuid Java-UUID to assign.
     * @return This UUID after assigning.
     */
    UUID assign(java.util.UUID uuid);

    /**
     * Assign this item by two longs
     *
     * @param msb msb-long
     * @param lsb lsb-long
     * @return this
     */
    UUID assign(long msb, long lsb);

    /**
     * Assign this item by another UUID
     *
     * @param other another UUID
     * @return this
     */
    UUID copyFrom(UUIDReadOnly other);

    /**
     * Assign this item by CharSequence, using {@code UUIDParseFormat.ANY} format.
     *
     * @param source CharSequence to assign.
     * @return This UUID after assigning.
     */
    UUID fromString(CharSequence source);

    /**
     * Assign this item by CharSequence, using specified parsing format.
     *
     * @param source CharSequence to assign.
     * @param format format of input string.
     * @return This UUID after assigning.
     */
    UUID fromString(CharSequence source, UUIDParseFormat format);

    /**
     * Assign this item by array of bytes.
     *
     * @param source Array of bytes to assign.
     * @return This UUID after assigning.
     */
    UUID fromBytes(byte[] source);

    /**
     * Assign this item by binary array.
     *
     * @param source binary array to assign.
     * @return This UUID after assigning.
     */
    UUID fromBinaryArray(BinaryArrayReadOnly source);

    /**
     * Assign this item by another UUID
     *
     * @param other another UUID
     * @return this
     */
    UUID assign(UUIDReadOnly other);

    /**
     * Assign this item from byte array
     *
     * @param ar byte array
     * @return this
     */
    UUID assign(byte[] ar);

    /**
     * Assign this item from byte array started from offset
     *
     * @param ar     byte array
     * @param offset offset
     * @return this
     */
    UUID assign(byte[] ar, int offset);

    /**
     * Assign this item from binary array
     *
     * @param source binary array
     * @return this
     */
    UUID assign(BinaryArrayReadOnly source);

    /**
     * Assign this item from binary array starting from offset
     *
     * @param source binary array
     * @param offset offset in binary array
     * @return this
     */
    UUID assign(BinaryArrayReadOnly source, int offset);

    /**
     * Assign this item from CharSequence, using {@code UUIDParseFormat.ANY} format.
     *
     * @param string CharSequence
     * @return this
     */
    UUID assign(CharSequence string);

    /**
     * Assign this item from CharSequence started from offset, using {@code UUIDParseFormat.ANY} format.
     *
     * @param string CharSequence
     * @param offset offset
     * @return this
     */
    UUID assign(CharSequence string, int offset);

    /**
     * Assign this item from CharSequence, using specified parsing format.
     *
     * @param string CharSequence
     * @param format format of input string.
     * @return this
     */
    UUID assign(CharSequence string, UUIDParseFormat format);

    /**
     * Assign this item from CharSequence started from offset, using specified parsing format.
     *
     * @param string CharSequence
     * @param offset offset
     * @param format format of input string.
     * @return this
     */
    UUID assign(CharSequence string, int offset, UUIDParseFormat format);
}

/*
 * Copyright 2021 EPAM Systems, Inc
 *
 * See the NOTICE file distributed with this work for additional information
 * regarding copyright ownership. Licensed under the Apache License,
 * Version 2.0 (the "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
package com.epam.deltix.containers.interfaces;

/**
 * Created by VavilauA on 4/7/2017.
 */
public interface BinaryConvertibleReadOnly extends BinaryIdentifierReadOnly {
    /**
     * Return boolean-value(if we assigned this by boolean) of BinaryArray.
     * @return boolean-value(if we assigned this by boolean) of BinaryArray.
     */
    boolean toBoolean();

    /**
     * Return byte-value(if we assigned this by byte) of BinaryArray
     * @return byte-value(if we assigned this by byte) of BinaryArray
     */
    byte toByte();

    /**
     * Return char-value(if we assigned this by char) of BinaryArray
     * @return char-value(if we assigned this by char) of BinaryArray
     */
    char toChar();

    /**
     * Return double-value(if we assigned this by double) of BinaryArray
     * @return double-value(if we assigned this by double) of BinaryArray
     */
    double toDouble();

    /**
     * Return int16-value(if we assigned this by int16) of BinaryArray
     * @return int16-value(if we assigned this by int16) of BinaryArray
     */
    short toInt16();

    /**
     * Return int32-value(if we assigned this by int32) of BinaryArray
     * @return int32-value(if we assigned this by int32) of BinaryArray
     */
    int toInt32();

    /**
     * Return int64-value(if we assigned this by int64) of BinaryArray
     * @return int64-value(if we assigned this by int64) of BinaryArray
     */
    long toInt64();

    /**
     * Return single-value(if we assigned this by single) of BinaryArray
     * @return single-value(if we assigned this by single) of BinaryArray
     */
    float toSingle();

    /**
     * Write utf8-value of this BinaryArray to array
     * @param utf8 array
     */
    void toUTF8(byte[] utf8);

    /**
     * Write uuid-value of this BinaryArray to uuid
     * @param uuid uuid
     */
    void toUUID(UUID uuid);

    /**
     * Write string-value of this BinaryArray to MutableString
     * @param str destination
     */
    void toMutableString(MutableString str);

    /**
     * Return boolean-value of byte with index offset
     * @param offset offset
     * @return boolean-value of byte with index offset
     */
    boolean toBoolean(int offset);

    /**
     * Return byte with index offset
     * @param offset offset
     * @return byte with index offset
     */
    byte toByte(int offset);

    /**
     * Return char-value of two bytes with start index offset
     * @param offset offset
     * @return char-value of two bytes with start index offset
     */
    char toChar(int offset);

    /**
     * Return double-value of eight bytes with start index offset
     * @param offset Offset in BinaryArray.
     * @return double-value of eight bytes with start index offset
     */
    double toDouble(int offset);

    /**
     * Return int16-value of two bytes with start index offset
     * @param offset offset
     * @return int16-value of two bytes with start index offset
     */
    short toInt16(int offset);

    /**
     * Return int32-value of four bytes with start index offset
     * @param offset offset
     * @return int32-value of four bytes with start index offset
     */
    int toInt32(int offset);

    /**
     * Return int64-value of eight bytes with start index offset
     * @param offset offset
     * @return int64-value of eight bytes with start index offset
     */
    long toInt64(int offset);

    /**
     * Return float-value of four bytes with start index offset
     * @param offset offset
     * @return float-value of four bytes with start index offset
     */
    float toSingle(int offset);

    /**
     * Convert to utf8 all bytes, started from offset
     * @param utf8 utf8
     * @param offset offset
     */
    void toUTF8(byte[] utf8, int offset);

    /**
     * Write uuid-value of all bytes, started from offset to uuid
     * @param uuid uuid
     * @param  offset Offset in BinaryArray.
     */
    void toUUID(UUID uuid, int offset);

    /**
     * Convert to MutableString all bytes, started from offset
     * @param str MutableString
     * @param offset offset
     */
    void toMutableString(MutableString str, int offset);

    /**
     * Convert to MutableString all bytes, started from offset, of specified length.
     * If length is odd, length + 1 bytes will be used.
     * @param str MutableString
     * @param offset offset in this BinaryArray, bytes
     * @param length length in this BinaryArray, bytes
     */
    void toMutableString(MutableString str, int offset, int length);

    /**
     * Copy count bytes from BinaryArray started from source index to byte array destination started from destination index.
     * @param sourceIndex A 32-bit integer that represents the index(in bytes) in the BinaryArray at which copying begins.
     * @param destination destination(array of bytes)
     * @param destinationIndex A 32-bit integer that represents the index in the destinationArray at which storing begins.
     * @param count A 32-bit integer that represents the number of bytes to copy.
     */
    void toByteArray(int sourceIndex, byte[] destination, int destinationIndex, int count);

    /**
     * Copy to another binaryarray
     * @param str another binaryarray
     */
    void copyTo(BinaryArrayReadWrite str);

    /**
     * Return true if one BinaryArray equals to another
     * @param another another
     * @return true if one BinaryArray equals to another
     */
    boolean equals(BinaryConvertibleReadOnly another);

    /**
     * Convert binary array to created byte[]
     * @return created byte[]
     */
    byte[] toByteArray();

    /**
     * Returns the full copy of this object.
     * @return Copy of this object.
     */
    BinaryConvertibleReadOnly clone();

    /**
     * Convert BinaryArray to String (ASCII/non-ASCII)
     * @param isASCII (ASCII/non-ASCII)
     * @return String
     */
    String toString(boolean isASCII);

    /**
     * Convert BinaryArray to String
     * @return String
     */
    String toString();

    /**
     * Create {@code UUID} from this binary array.
     * @return New {@code UUID}.
     */
    UUIDReadOnly toUUID();

    /**
     * Create {@code MutableString} from this binary array.
     * @return New {@code MutableString}.
     */
    ReadOnlyString toMutableString();

    /**
     * return byte with index
     * @param index index
     * @return byte with index
     */
    byte getByteAt(int index);

    /**
     * return count of bytes in BinaryArray
     * @return count of bytes in BinaryArray
     */
    int getCount();

    /**
     * Return capacity of byte-storage in BinaryArray
     * @return capacity of byte-storage in BinaryArray
     */
    int getCapacity();
}
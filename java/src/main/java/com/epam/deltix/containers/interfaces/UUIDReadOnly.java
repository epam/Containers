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

public interface UUIDReadOnly {
    /**
     * Return MSB-long of UUID
     *
     * @return MSB-long of UUID
     */
    long getMSB();

    /**
     * Return LSB-long of UUID
     *
     * @return LSB-long of UUID
     */
    long getLSB();

    /**
     * Clone this UUID
     *
     * @return clone of this UUID
     */
    UUID clone();

    /**
     * Copy this UUID to another UUID
     *
     * @param destination another UUID
     */
    void copyTo(UUID destination);

    /**
     * Return true if this UUID equals to another UUID, false otherwise.
     *
     * @param other another UUID
     * @return true if this UUID equals to another UUID, false otherwise.
     */
    boolean equals(UUIDReadOnly other);

    /**
     * Return true if this UUID equals to another {@code java.util.UUID}, false otherwise.
     *
     * @param other Another {@code java.util.UUID}
     * @return true if this UUID equals to another {@code java.util.UUID}, false otherwise.
     */
    boolean equals(java.util.UUID other);

    /**
     * Convert this UUID to Java-UUID.
     *
     * @return Java-UUID.
     */
    java.util.UUID toJavaUUID();

    /**
     * Convert this UUID to array of bytes
     *
     * @return array of bytes
     */
    byte[] toBytes();

    /**
     * Write byte-representation of this UUID to array of bytes
     *
     * @param ar array of bytes
     */
    void toBytes(byte[] ar);

    /**
     * Return string-representation of this UUID, using {@code UUIDPrintFormat.UPPERCASE} format.
     *
     * @return string-representation of this UUID.
     */
    String toString();

    /**
     * Convert to string of specified format.
     *
     * @param format format of output string.
     * @return string representation of this UUID.
     */
    String toString(UUIDPrintFormat format);

    /**
     * Return hexstring-representation of this UUID, using {@code UUIDPrintFormat.UPPERCASE_WITHOUT_DASHES} format.
     *
     * @return hexstring-representation of this UUID.
     */
    String toHexString();

    /**
     * Returns string representation of this UUID, using {@code UUIDPrintFormat.UPPERCASE} format.
     *
     * @return new {@code MutableString} representation of this UUID.
     */
    ReadOnlyString toMutableString();

    /**
     * Returns string representation of this UUID, using specified {@code UUIDPrintFormat}.
     *
     * @param format format of output string.
     * @return new {@code MutableString} representation of this UUID.
     */
    ReadOnlyString toMutableString(UUIDPrintFormat format);

    /**
     * Assigns string representation of this UUID to specified {@code MutableString},
     * using {@code UUIDPrintFormat.UPPERCASE} format.
     *
     * @param string output {@code MutableString}, containing binary representation of this UUID.
     */
    void toMutableString(MutableString string);

    /**
     * Assigns string representation of this UUID to specified {@code MutableString},
     * using specified {@code UUIDPrintFormat}.
     *
     * @param string output {@code MutableString}, containing binary representation of this UUID.
     * @param format format of output string.
     */
    void toMutableString(MutableString string, UUIDPrintFormat format);

    /**
     * Returns binary representation of this UUID.
     *
     * @return new {@code BinaryArray} representation of this UUID.
     */
    BinaryConvertibleReadOnly toBinaryArray();

    /**
     * Assigns binary representation of this UUID to specified {@code BinaryArray}.
     *
     * @param binaryArray output {@code BinaryArray}, containing binary representation of this UUID.
     */
    void toBinaryArray(BinaryConvertibleReadWrite binaryArray);
}
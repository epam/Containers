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
 * Interface for Mutable Ascii String
 */
public interface BinaryAsciiString extends ReadOnlyString {

    /**
     * Append hex-representation of long to string
     *
     * @param i Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString appendFastHex(long i);

    /**
     * Append hex-representation of uuid to string
     *
     * @param i Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString appendFastHex(UUIDReadOnly i);

    /**
     * Assign item to string. Throw exception if item contains non-ascii characters.
     *
     * @param item Item to assign.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString assign(Object item);

    /**
     * Append item to string. Throw exception if item contains non-ascii characters.
     *
     * @param item Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(Object item);

    /**
     * Assign item to string. Throw exception if item contains non-ascii characters.
     *
     * @param item Item to assign.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString assign(BinaryArrayReadOnly item);

    /**
     * Append item to string. Throw exception if item contains non-ascii characters.
     *
     * @param item Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(BinaryArrayReadOnly item);

    /**
     * Append item to string
     *
     * @param item Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(boolean item);

    /**
     * Append item to string. Throw exception if item contains non-ascii characters.
     *
     * @param cs Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(CharSequence cs);

    /**
     * Append item to string. Throw exception if item is non-ascii characters.
     *
     * @param c Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(char c);



    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(short i);

    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(int i);

    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(long i);

    /**
     * Append item to string. Throw exception if item contains non-ascii characters.
     *
     * @param str Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(char[] str);

    /**
     * Append count chars from item(char-array) started from offset. Throw exception if item contains non-ascii characters.
     *
     * @param str    Item to append.
     * @param offset offset
     * @param count  count
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(char[] str, int offset, int count);

    /**
     * Append item to string. Throw exception if item contains non-ascii characters.
     *
     * @param str Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(byte[] str);

    /**
     * Append count chars from item(char-array) started from offset. Throw exception if item contains non-ascii characters.
     *
     * @param str    Item to append.
     * @param offset offset
     * @param count  count
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(byte[] str, int offset, int count);

    /**
     * Append item to string
     *
     * @param i Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(double i);

    /**
     * Append item to string, using {@code UUIDPrintFormat.UPPERCASE}.
     *
     * @param uuid Item to append.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(UUIDReadOnly uuid);

    /**
     * Append item to string, using specified {@code UUIDPrintFormat}.
     *
     * @param uuid   Item to append.
     * @param format Format of result string.
     * @return This BinaryAsciiString after appending.
     */
    BinaryAsciiString append(UUIDReadOnly uuid, UUIDPrintFormat format);

    /**
     * Assign string by item
     *
     * @param item Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(boolean item);


    /**
     * Assign string by item. Throw exception if item contains non-ascii characters.
     *
     * @param cs Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(CharSequence cs);

    /**
     * Assign string by item. Throw exception if item is non-ascii character.
     *
     * @param c Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(char c);

    /**
     * Assign string by item
     *
     * @param i Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(short i);

    /**
     * Assign string by item
     *
     * @param i Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(double i);

    /**
     * Assign string by item
     *
     * @param i Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(int i);

    /**
     * Assign string by item
     *
     * @param i Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(long i);

    /**
     * Assign string by item. Throw exception if item contains non-ascii characters.
     *
     * @param str Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(char[] str);

    /**
     * Assign this string by count chars from item(char-array) started from offset. Throw exception if item contains non-ascii characters.
     *
     * @param str    Item to assign.
     * @param offset offset
     * @param count  count
     * @return this string
     */
    BinaryAsciiString assign(char[] str, int offset, int count);



    /**
     * Assign string by item. Throw exception if item contains non-ascii characters.
     *
     * @param str Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(byte[] str);

    /**
     * Assign this string by count chars from item(char-array) started from offset. Throw exception if item contains non-ascii characters.
     *
     * @param str    Item to assign.
     * @param offset offset
     * @param count  count
     * @return this string
     */
    BinaryAsciiString assign(byte[] str, int offset, int count);



    /**
     * Assign this string by item, using specified {@code UUIDPrintFormat.UPPERCASE} format.
     *
     * @param uuid Item to assign.
     * @return this string
     */
    BinaryAsciiString assign(UUIDReadOnly uuid);

    /**
     * Assign this string by item, using specified {@code UUIDPrintFormat}.
     *
     * @param uuid   Item to assign.
     * @param format Format of result string.
     * @return this string
     */
    BinaryAsciiString assign(UUIDReadOnly uuid, UUIDPrintFormat format);

    /**
     * Set char with index in this string to ch. Throw exception if item is non-ascii characters.
     *
     * @param ch    ch
     * @param index index
     */
    void setCharAt(int index, char ch);

    /**
     * Clear this string
     *
     * @return this item
     */
    BinaryAsciiString clear();

    /**
     * Copy this string to another string.
     *
     * @param str another string
     */
    void copyTo(com.epam.deltix.containers.BinaryAsciiString str);

    /**
     * Assign this string by another
     *
     * @param source another string
     * @return this string
     */
    BinaryAsciiString copyFrom(CharSequence source);

    /**
     * Transforms this string to lower case
     *
     * @return this string in lower case
     */
    BinaryAsciiString toLowerCase();

    /**
     * Transforms this string to upper case
     *
     * @return this string in upper case
     */
    BinaryAsciiString toUpperCase();
}
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
package com.epam.deltix.containers;


/**
 * Implements encoding and decoding of strings composed from characters of the limited set.
 * <p>
 * The character set includes all Latin capital letters [A-Z], digits [0-9] as well as
 * the following special characters !"#$%&amp;'()*+,-./:;&lt;=&gt;?@[\]^_. Its range is [0x20, 0x5F].
 * </p>
 */
public class AlphanumericUtils {

    private static final long ALPHANUM10_MAX_LENGTH = 10;
    private static final long ALPHANUM_10_NUM_LENGTH_BITS = 4;
    private static final long INT64_NULL_VALUE = Long.MIN_VALUE;

    /**
     * Checks that string could be effectively packed with ALPHANUMERIC(N) encoder.
     * @param str String to check.
     * @return True if string could be packed with ALPHANUMERIC(N) encoder.
     */
    public static boolean isValidAlphanumeric(CharSequence str)
    {

        long length;
        if (str == null)
            return true;
        if ((length = str.length()) == 0)
            return true;

        if (length > 0xFFFE /* Max alphanumeric length that will supposedly go through all current codecs */)
            return false;

        int space = (int)' ';

        int acc = 0;
        for (int i = 0; i < length; ++i)
        {
            acc |= (int)str.charAt(i) - space;
        }

        return (acc >>> 6) == 0;	// None of the "encoded" characters should use more than 6 bits
    }

    /**
     * Checks that string could be effectively packed with ALPHANUMERIC(10) encoder.
     * @param str String to check.
     * @return True if string could be packed with ALPHANUMERIC(10) encoder.
     */
    public static boolean isValidAlphanumericUInt64(CharSequence str)
    {
        long length;

        if (str == null)
            return true;

        if ((length = str.length()) == 0)
            return true;

        if (length > 10) {
            return false;
        }
        int ch, check = 0;

        for (int i = (int)length; i >= 1; i--) {
            ch = (int)(short)(str.charAt(i - 1) - ' ');
            check |= ch;
        }
        return (check >>> 6) == 0;
    }

    /**
     * Checks that long integer number contains valid string packed by ALPHANUMERIC(10) codec.
     * @param alphanumericLong Integer to check
     * @return True if long integer number contains valid string packed by ALPHANUMERIC(10) codec, including null value.
     */
    public static Boolean isValidAlphanumeric(long alphanumericLong)
    {
        if (alphanumericLong == INT64_NULL_VALUE)
            return true;

        // number of bits to store |[0..size]| = size + 1 values
        // bsr(MaxSize + 1) = bsr(10 + 1) = 4

        long len = alphanumericLong >>> (8 * 8 - ALPHANUM_10_NUM_LENGTH_BITS);
        return len <= ALPHANUM10_MAX_LENGTH + 1; // valid lengths are: 0..n, n + 1
    }

    /**
     * Decodes ALPHANUMERIC(10) and assigns it to MutableString (can't be null).
     * Throws ArgumentException if alphanumericLong parameter can't be decoded
     * @param str MutableString to assign the result.
     * @param alphanumericLong Long Integer to decode.
     * @return Returns the same instance of MutableString that was passed as parameter.
     */
    public static MutableString assignAlphanumeric(MutableString str, long alphanumericLong)
    {
       str.clear();
       return appendAlphanumeric(str, alphanumericLong);
    }

    /**
     * Decodes ALPHANUMERIC(10) and appends it to MutableString (can't be null).
     * Throws ArgumentException if alphanumericLong parameter can't be decoded
     * @param str MutableString to append the result.
     * @param alphanumericLong Long Integer to decode.
     * @return Returns the same instance of MutableString that was passed as parameter.
     */
    public static MutableString appendAlphanumeric(MutableString str, long alphanumericLong)
    {
        if (alphanumericLong == INT64_NULL_VALUE)
            return str;

        // number of bits to store |[0..size]| = size + 1 values
        // bsr(MaxSize + 1) = bsr(10 + 1) = 4
        // Cast length to signed int without sign-extension (with good optimizer should generate no instructions)
        long len = (int)(alphanumericLong >>> (8 * 8 - ALPHANUM_10_NUM_LENGTH_BITS));

        if (len <= (int)ALPHANUM10_MAX_LENGTH)
        {
            for (int i = 1; i <= len; i++) {
                str.append((char)(' ' + ((alphanumericLong >>> (60 - 6 * i)) & 0x3F)));
            }
        }
        else
        if (len == ALPHANUM10_MAX_LENGTH + 1) // Alternate null value
        {
            return str;
        }
        else
        {
            throw new IllegalArgumentException(String.format("Invalid ALPHANUMERIC(10) value: %d", alphanumericLong));
        }
        return str;
    }

    /**
     * Decodes ALPHANUMERIC(10) and assigns it to MutableString (can't be null).
     * Throws ArgumentException if alphanumericLong parameter can't be decoded
     * @param str MutableString to assign the result.
     * @param alphanumericLong Long Integer to decode.
     * @return Returns the same instance of MutableString that was passed as parameter.
     */
    public static StringBuilder assignAlphanumeric(StringBuilder str, long alphanumericLong)
    {
        if (alphanumericLong == INT64_NULL_VALUE) {
            str.setLength(0);
            return str;
        }

        // number of bits to store |[0..size]| = size + 1 values
        // bsr(MaxSize + 1) = bsr(10 + 1) = 4
        // Cast length to signed int without sign-extension (with good optimizer should generate no instructions)
        long len = (int)(alphanumericLong >>> (8 * 8 - ALPHANUM_10_NUM_LENGTH_BITS));

        if (len <= (int)ALPHANUM10_MAX_LENGTH)
        {
            str.setLength(0);
            for (int i = 1; i <= 10; i++) {
                str.append((char)(' ' + ((alphanumericLong >>> (60 - 6 * i)) & 0x3F)));
            }
        }
        else
        if (len == ALPHANUM10_MAX_LENGTH + 1) // Alternate null value
        {
            str.setLength(0);
            return str;
        }
        else
        {
            throw new IllegalArgumentException(String.format("Invalid ALPHANUMERIC(10) value: %d", alphanumericLong));
        }
        return str;
    }


    /**
     * Decodes ALPHANUMERIC(10) from Int64, returning String.
     * @param alphanumericLong Long Integer to decode.
     * @return New String instance.
     */
    public static String toString(long alphanumericLong)
    {
        if (alphanumericLong == INT64_NULL_VALUE) return null;
        long len = alphanumericLong >>> (8 * 8 - ALPHANUM_10_NUM_LENGTH_BITS);
        if (len == ALPHANUM10_MAX_LENGTH + 1) {
            return null;
        }
        return assignAlphanumeric(new MutableString(), alphanumericLong).toString();
    }

    /**
     * Decodes ALPHANUMERIC(10) from Int64, returning String.
     * @param alphanumericLong Long Integer to decode.
     * @return New String instance.
     */
    public static MutableString toMutableString(long alphanumericLong)
    {
        if (alphanumericLong == INT64_NULL_VALUE) return null;
        long len = alphanumericLong >>> (8 * 8 - ALPHANUM_10_NUM_LENGTH_BITS);
        if (len == ALPHANUM10_MAX_LENGTH + 1) {
            return null;
        }
        return assignAlphanumeric(new MutableString(), alphanumericLong);
    }


    /**
     * Encodes String with ALPHANUMERIC(10) encoder, returning UInt64.
     * throws ArgumentException if the string is longer than 10 characters or contains symbols outside ['\x20'..'\x5F']
     * @param str String to encode.
     * @return Resulting long integer.
     */
    public static long toAlphanumericUInt64(CharSequence str)
    {
        long length;
        // number of bits to store |[0..size]| = size + 1 values
        // bsr(MaxSize + 1) = bsr(10 + 1) = 4

        if (null == str)
            return INT64_NULL_VALUE;

        length = str.length();

        long acc = 0;
        int ch, check = 0;
        if (length > 10) {
            throw new IllegalArgumentException(String.format("String %s is too long", str));
        }

        for (int i = (int)length; i >= 1; i--) {
            ch = (int)(short)(str.charAt(i - 1) - ' ');
            check |= ch;
            acc |= (long)(ch & 0x3F) << (10 - i) * 6;
        }


        if ((check >>> 6) != 0 )
            throw new IllegalArgumentException(String.format("String %s contains invalid character", str));
        return (length << (8 * 8 - ALPHANUM_10_NUM_LENGTH_BITS)) + acc;
    }


}
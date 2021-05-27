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
 * Format of input string.
 */
public enum UUIDParseFormat {
    /**
     * Lower case with dashes. Example: '01234567-89ab-cdef-1011-121314151617'.
     */
    LOWERCASE(1),

    /**
     * Upper case with dashes. This is the default output format. Example: '01234567-89AB-CDEF-1011-121314151617'.
     */
    UPPERCASE(2),

    /**
     * Any case with dashes. Symbol case can be mixed. Example: '01234567-89Ab-cDef-1011-121314151617'.
     */
    ANYCASE(3),

    /**
     * Lower case without dashes. Example: '0123456789abcdef1011121314151617'.
     */
    LOWERCASE_WITHOUT_DASHES(4),

    /**
     * Upper case without dashes. This is the default hex output format. Example: '0123456789ABCDEF1011121314151617'.
     */
    UPPERCASE_WITHOUT_DASHES(5),

    /**
     * Any case without dashes. Symbol case can be mixed. Example: '0123456789AbcDef1011121314151617'.
     */
    ANYCASE_WITHOUT_DASHES(6),

    /**
     * Any correctly formatted {@code UUID} string.
     * Can be either {@code ANYCASE} or  {@code ANYCASE_WITHOUT_DASHES}.
     * Dashes cannot be mixed.
     */
    ANY(7);

    private final int value;

    UUIDParseFormat(int value) {
        this.value = value;
    }

    /**
     * Returns the numeric code corresponding to the enumeration value.
     *
     * @return The numeric code corresponding to the enumeration value.
     */
    public int getNumber() {
        return this.value;
    }

    /**
     * Returns the value of the enumeration corresponding to the numeric code.
     *
     * @param value Numeric code.
     * @return Enumeration value.
     * @throws IllegalArgumentException When enumeration does not have a value corresponding to the given numeric code.
     */
    public static UUIDParseFormat strictValueOf(int value) {
        final UUIDParseFormat typedValue = valueOf(value);
        if (typedValue == null) {
            throw new IllegalArgumentException("Enumeration 'UUIDParseFormat' does not have value corresponding to '" + value + "'.");
        }
        return typedValue;
    }

    /**
     * Returns the value of the enumeration corresponding to the numeric code.
     *
     * @param value Numeric code.
     * @return Enumeration value; {@code null} when there is no corresponding value.
     */
    public static UUIDParseFormat valueOf(int value) {
        switch (value) {
            case 1:
                return LOWERCASE;

            case 2:
                return UPPERCASE;

            case 3:
                return ANYCASE;

            case 4:
                return LOWERCASE_WITHOUT_DASHES;

            case 5:
                return UPPERCASE_WITHOUT_DASHES;

            case 6:
                return ANYCASE_WITHOUT_DASHES;

            case 7:
                return ANY;

            default:
                return null;
        }
    }
}
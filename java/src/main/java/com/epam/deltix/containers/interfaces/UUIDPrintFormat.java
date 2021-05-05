package com.epam.deltix.containers.interfaces;

/**
 * Format of output string.
 */
public enum UUIDPrintFormat {
    /**
     * Lower case with dashes. Example: '01234567-89ab-cdef-1011-121314151617'.
     */
    LOWERCASE(1),

    /**
     * Upper case with dashes. This is the default output format. Example: '01234567-89AB-CDEF-1011-121314151617'.
     */
    UPPERCASE(2),

    /**
     * Lower case without dashes. Example: '0123456789abcdef1011121314151617'.
     */
    LOWERCASE_WITHOUT_DASHES(3),

    /**
     * Upper case without dashes. This is the default hex output format. Example: '0123456789ABCDEF1011121314151617'.
     */
    UPPERCASE_WITHOUT_DASHES(4);

    private final int value;

    UUIDPrintFormat(int value) {
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
    public static UUIDPrintFormat strictValueOf(int value) {
        final UUIDPrintFormat typedValue = valueOf(value);
        if (typedValue == null) {
            throw new IllegalArgumentException("Enumeration 'UUIDPrintFormat' does not have value corresponding to '" + value + "'.");
        }
        return typedValue;
    }

    /**
     * Returns the value of the enumeration corresponding to the numeric code.
     *
     * @param value Numeric code.
     * @return Enumeration value; {@code null} when there is no corresponding value.
     */
    public static UUIDPrintFormat valueOf(int value) {
        switch (value) {
            case 1:
                return LOWERCASE;

            case 2:
                return UPPERCASE;

            case 3:
                return LOWERCASE_WITHOUT_DASHES;

            case 4:
                return UPPERCASE_WITHOUT_DASHES;

            default:
                return null;
        }
    }
}

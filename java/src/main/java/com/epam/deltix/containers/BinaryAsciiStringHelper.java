package com.epam.deltix.containers;

class BinaryAsciiStringHelper {
    private static final char[] HEX_DIGITS_UPPER = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E',   'F'};
    static final short[] DIGIT_PAIRS = new short[256];
    static  {
        for (int i = 0; i < 16; ++i) {
            for (int j = 0; j < 16; ++j) {
                DIGIT_PAIRS[i * 16 + j] = (short)((((((int)HEX_DIGITS_UPPER[j] << 8) | (int)HEX_DIGITS_UPPER[i]))) & (0xffff));
            }
        }
    }
}

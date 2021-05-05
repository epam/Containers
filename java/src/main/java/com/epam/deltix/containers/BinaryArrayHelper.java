package com.epam.deltix.containers;

/**
 * Class with some methods for work with BinaryArray
 */
public class BinaryArrayHelper {
    /**
     * Method to return internal buffer from BinaryArray.
     *
     * @param binaryArray Binary array.
     * @return Internal buffer from binary array.
     */
    public long[] getInternalBuffer(BinaryArray binaryArray) {
        return binaryArray.data;
    }

    /**
     * Method to return internal buffer from BinaryAsciiString.
     *
     * @param binaryArray BinaryAsciiString.
     * @return Internal buffer from BinaryAsciiString.
     */
    public long[] getInternalBuffer(BinaryAsciiString binaryArray) {
        return binaryArray.data;
    }


    /**
     * Method to return internal buffer from MutableString.
     *
     * @param string mutable string.
     * @return Internal buffer from MutableString.
     */

    public char[] getInternalBuffer(MutableString string) {
        return string.data;
    }

}

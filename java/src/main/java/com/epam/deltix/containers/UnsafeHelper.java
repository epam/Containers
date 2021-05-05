package com.epam.deltix.containers;

import sun.misc.Unsafe;

import java.lang.reflect.Field;


/**
 * Helper for work with Unsafe class.
 */
@SuppressWarnings("WeakerAccess")
public class UnsafeHelper {

    private static Unsafe u;

    static {
        try {
            Field f = Unsafe.class.getDeclaredField("theUnsafe");
            f.setAccessible(true);

            u = (Unsafe) f.get(null);
        } catch (Exception ex) {
            throw new RuntimeException(ex);
        }
    }


    /**
     * Set long to array of long with offset.
     * @param array Destination array.
     * @param arrayOffset Destination array offset in bytes.
     * @param value New value of array + offset.
     */
    public static void setLongToLongArray(Object array, long arrayOffset, long value) {
        u.putLong(array, arrayOffset + Unsafe.ARRAY_LONG_BASE_OFFSET, value);
    }

    /**
     * Set int to array of long with offset.
     * @param array Destination array.
     * @param arrayOffset Destination array offset in bytes.
     * @param value New value of array + offset.
     */
    public static void setIntToLongArray(Object array, long arrayOffset, int value) {
        u.putInt(array, arrayOffset + Unsafe.ARRAY_LONG_BASE_OFFSET, value);
    }

    /**
     * Get byte from long array.
     * @param array Long array.
     * @param index Offset in bytes.
     * @return Byte by array and offset.
     */
    public static byte getByteFromLongArray(Object array, long index) {
        return u.getByte(array, index + Unsafe.ARRAY_LONG_BASE_OFFSET);
    }

    /**
     * Set byte to long array.
     * @param array Long array.
     * @param index Offset in bytes.
     * @param value New value of array.
     */
    public static void setByteAtLongArray(Object array, long index, byte value) {
        u.putByte(array, index + Unsafe.ARRAY_LONG_BASE_OFFSET, value);
    }

    /**
     * Mem move from long array to long array.
     * @param source Source array
     * @param sourceOffset Source offset in bytes.
     * @param destination Destination array.
     * @param destinationOffset Destination offset in bytes.
     * @param count Count in bytes.
     */
    public static void memMoveFromLongToLong(Object source, long sourceOffset, Object destination, long destinationOffset, long count) {
        u.copyMemory(source, sourceOffset + Unsafe.ARRAY_LONG_BASE_OFFSET, destination, destinationOffset + Unsafe.ARRAY_LONG_BASE_OFFSET, count);
    }


    /**
     * Mem move from long array to byte array.
     * @param source Source array
     * @param sourceOffset Source offset in bytes.
     * @param destination Destination array.
     * @param destinationOffset Destination offset in bytes.
     * @param count Count in bytes.
     */
    public static void memMoveFromLongToByte(Object source, long sourceOffset, Object destination, long destinationOffset, long count) {
        u.copyMemory(source, sourceOffset + Unsafe.ARRAY_LONG_BASE_OFFSET, destination, destinationOffset + Unsafe.ARRAY_BYTE_BASE_OFFSET, count);
    }


    /**
     * Mem move from byte array to long array.
     * @param source Source array
     * @param sourceOffset Source offset in bytes.
     * @param destination Destination array.
     * @param destinationOffset Destination offset in bytes.
     * @param count Count in bytes.
     */
    public static void memMoveFromByteToLong(Object source, long sourceOffset, Object destination, long destinationOffset, long count) {
        u.copyMemory(source, sourceOffset + Unsafe.ARRAY_BYTE_BASE_OFFSET, destination, destinationOffset + Unsafe.ARRAY_LONG_BASE_OFFSET, count);
    }

}

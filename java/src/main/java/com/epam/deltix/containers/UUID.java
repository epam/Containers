package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.BinaryConvertibleReadWrite;
import com.epam.deltix.containers.interfaces.BinaryArrayReadOnly;
import com.epam.deltix.containers.interfaces.UUIDParseFormat;
import com.epam.deltix.containers.interfaces.UUIDPrintFormat;
import com.epam.deltix.containers.interfaces.UUIDReadOnly;

import java.util.Random;

public class UUID implements Comparable<UUID>, com.epam.deltix.containers.interfaces.UUID {
    private static final Random rng = new Random(System.currentTimeMillis());

    /**
     * Constant for empty uuid
     */
    public static final UUID EMPTY = new UUID(0, 0);
    /**
     * Constant for minimal uuid.
     */
    public static final UUID MIN_VALUE = new UUID(0, 0);
    /**
     * Constant for maximal uuid.
     */
    public static final UUID MAX_VALUE = new UUID(-1, -1);

    private long msb;
    private long lsb;

    public UUID() {
        msb = lsb = 0;
    }

    public UUID(long msb, long lsb) {
        this.msb = msb;
        this.lsb = lsb;
    }

    public UUID(java.util.UUID other) {
        assign(other);
    }

    public UUID(UUIDReadOnly other) {
        if (other == null)
            throw new IllegalArgumentException("Parameter 'other' cannot be null.");
        this.msb = other.getMSB();
        this.lsb = other.getLSB();
    }

    public UUID(CharSequence source) {
        assign(source, 0);
    }

    public UUID(CharSequence source, int offset) {
        assign(source, offset);
    }

    public UUID(CharSequence source, UUIDParseFormat format) {
        assign(source, 0, format);
    }

    public UUID(CharSequence source, int offset, UUIDParseFormat format) {
        assign(source, offset, format);
    }

    public UUID(BinaryArrayReadOnly source) {
        assign(source, 0);
    }

    public UUID(BinaryArrayReadOnly source, int offset) {
        assign(source, offset);
    }

    public long getMSB() {
        return msb;
    }

    public long getLSB() {
        return lsb;
    }

    public void clear() {
        msb = lsb = 0;
    }

    public void setMSB(long x) {
        msb = x;
    }

    public void setLSB(long x) {
        lsb = x;
    }

    @Override
    public UUID clone() {
        return new UUID(this);
    }

    public void copyTo(com.epam.deltix.containers.interfaces.UUID destination) {
        if (destination == null)
            throw new IllegalArgumentException("Parameter 'destination' cannot be null.");

        destination.setMSB(msb);
        destination.setLSB(lsb);
    }

    /**
     * Assign this item by java-UUID.
     *
     * @param uuid Java-UUID to assign.
     * @return This UUID after assigning.
     */
    @Override
    public UUID assign(java.util.UUID uuid) {
        return assign(uuid.getMostSignificantBits(), uuid.getLeastSignificantBits());
    }

    @Override
    public UUID copyFrom(UUIDReadOnly source) {
        return assign(source);
    }

    @Override
    public UUID fromString(CharSequence source) {
        return assign(source);
    }

    @Override
    public UUID fromString(CharSequence source, UUIDParseFormat format) {
        return assign(source, format);
    }

    @Override
    public UUID fromBytes(byte[] source) {
        return assign(source);
    }

    @Override
    public UUID fromBinaryArray(BinaryArrayReadOnly source) {
        return assign(source);
    }

    @Override
    public UUID assign(UUIDReadOnly source) {
        if (source == null)
            throw new IllegalArgumentException("Parameter 'source' cannot be null.");

        msb = source.getMSB();
        lsb = source.getLSB();
        return this;
    }

    @Override
    public UUID assign(long msb, long lsb) {
        setLSB(lsb);
        setMSB(msb);
        return this;
    }

    public void randomize() {

        msb = rng.nextLong();
        lsb = rng.nextLong();

        /* Clear version. */
        msb &= 0xFFFF_FFFF_FFFF_0FFFL;
        /* Set to version 4. */
        msb |= 0x0000_0000_0000_4000L;
        /* Clear variant. */
        lsb &= 0x3FFF_FFFF_FFFF_FFFFL;
        /* Set to IETF variant. */
        lsb |= 0x8000_0000_0000_0000L;
    }

    public boolean equals(UUIDReadOnly other) {
        return other != null && other.getMSB() == msb && other.getLSB() == lsb;
    }

    @Override
    public boolean equals(java.util.UUID other) {
        return other != null && other.getMostSignificantBits() == msb && other.getLeastSignificantBits() == lsb;
    }

    @Override
    public boolean equals(Object other) {
        return (other instanceof UUID && equals((UUID) other))
                || (other instanceof java.util.UUID && equals((java.util.UUID) other));
    }

    @Override
    public int hashCode() {
        return (int) (msb ^ (msb >>> 32)) ^ (int) (lsb ^ (lsb >>> 32));
    }

    @Override
    public int compareTo(UUID other) {
        if (other == null)
            throw new IllegalArgumentException("Parameter 'other' cannot be null.");
        final int res = compareUnsigned(msb, other.msb);
        if (res != 0)
            return res;
        return compareUnsigned(lsb, other.lsb);
    }

    @Override
    public String toString() {
        return toMutableString(UUIDPrintFormat.UPPERCASE).toString();
    }

    @Override
    public String toString(UUIDPrintFormat format) {
        return toMutableString(format).toString();
    }

    @Override
    public String toHexString() {
        return toMutableString(UUIDPrintFormat.UPPERCASE_WITHOUT_DASHES).toString();
    }

    @Override
    public MutableString toMutableString() {
        return new MutableString().assign(this, UUIDPrintFormat.UPPERCASE);
    }

    @Override
    public MutableString toMutableString(UUIDPrintFormat format) {
        return new MutableString().assign(this, format);
    }

    @Override
    public void toMutableString(com.epam.deltix.containers.interfaces.MutableString string) {
        string.assign(this, UUIDPrintFormat.UPPERCASE);
    }

    @Override
    public void toMutableString(com.epam.deltix.containers.interfaces.MutableString string, UUIDPrintFormat format) {
        string.assign(this, format);
    }

    @Override
    public BinaryArray toBinaryArray() {
        return new BinaryArray(this);
    }

    @Override
    public void toBinaryArray(BinaryConvertibleReadWrite binaryArray) {
        binaryArray.assign(this);
    }

    public static boolean isValid(CharSequence string) {
        if (isTooLong(string, UUIDParseFormat.ANY)) {
            return false;
        }
        return isValid(string, 0);
    }

    public static boolean isValid(CharSequence string, int offset) {
        return isValid(string, offset, UUIDParseFormat.ANY);
    }

    public static boolean isValid(CharSequence string, UUIDParseFormat format) {
        if (isTooLong(string, format)) {
            return false;
        }
        return isValid(string, 0, format);
    }

    public static boolean isValid(CharSequence string, int offset, UUIDParseFormat format) {
        if (string == null)
            return false;
        if (string.length() < offset + 32)
            return false;
        if (format == UUIDParseFormat.ANY)
            format = string.charAt(offset + 8) == '-' ? UUIDParseFormat.ANYCASE : UUIDParseFormat.ANYCASE_WITHOUT_DASHES;

        switch (format) {
            case LOWERCASE:
            case UPPERCASE:
            case ANYCASE: {
                if (!canExtract(string, offset, 8, 0L, format)) return false;
                long m = extract(string, offset, 8, 0L, format);
                if (string.charAt(offset + 8) != '-')
                    return false;
                if (!canExtract(string, offset + 9, 4, m, format)) return false;
                m = extract(string, offset + 9, 4, m, format);
                if (string.charAt(offset + 13) != '-')
                    return false;
                if (!canExtract(string, offset + 14, 4, m, format)) return false;
                m = extract(string, offset + 14, 4, m, format);
                if (string.charAt(offset + 18) != '-')
                    return false;
                long l = 0;
                if (!canExtract(string, offset + 19, 4, 0L, format)) return false;
                l = extract(string, offset + 19, 4, 0L, format);
                if (string.charAt(offset + 23) != '-')
                    return false;
                if (!canExtract(string, offset + 24, 12, l, format)) return false;

                break;
            }
            case LOWERCASE_WITHOUT_DASHES:
            case UPPERCASE_WITHOUT_DASHES:
            case ANYCASE_WITHOUT_DASHES:
                if ((!canExtract(string, offset, 16, 0L, format)) || (!canExtract(string, offset + 16, 16, 0L, format))) return false;
                break;
        }

        return true;
    }


    @Override
    public boolean tryAssign(CharSequence string) {
        if (isTooLong(string, UUIDParseFormat.ANY)) {
            return false;
        }
        return tryAssign(string, 0);
    }

    @Override
    public boolean tryAssign(CharSequence string, int offset) {
        return tryAssign(string, offset, UUIDParseFormat.ANY);
    }

    @Override
    public boolean tryAssign(CharSequence string, UUIDParseFormat format) {
        if (isTooLong(string, format)) {
            return false;
        }
        return tryAssign(string, 0, format);
    }

    @Override
    public boolean tryAssign(CharSequence string, int offset, UUIDParseFormat format) {
        if (string == null)
            return false;
        if (string.length() < offset + 32)
            return false;
        if (format == UUIDParseFormat.ANY)
            format = string.charAt(offset + 8) == '-' ? UUIDParseFormat.ANYCASE : UUIDParseFormat.ANYCASE_WITHOUT_DASHES;

        switch (format) {
            case LOWERCASE:
            case UPPERCASE:
            case ANYCASE: {
                if (!canExtract(string, offset, 8, 0L, format)) return false;
                long m = extract(string, offset, 8, 0L, format);
                if (string.charAt(offset + 8) != '-')
                    return false;
                if (!canExtract(string, offset + 9, 4, m, format)) return false;
                m = extract(string, offset + 9, 4, m, format);
                if (string.charAt(offset + 13) != '-')
                    return false;
                if (!canExtract(string, offset + 14, 4, m, format)) return false;
                m = extract(string, offset + 14, 4, m, format);
                if (string.charAt(offset + 18) != '-')
                    return false;
                if (!canExtract(string, offset + 19, 4, 0L, format)) return false;
                long l = extract(string, offset + 19, 4, 0L, format);
                if (string.charAt(offset + 23) != '-')
                    return false;
                if (!canExtract(string, offset + 24, 12, l, format)) return false;
                l = extract(string, offset + 24, 12, l, format);

                msb = m;
                lsb = l;
                break;
            }
            case LOWERCASE_WITHOUT_DASHES:
            case UPPERCASE_WITHOUT_DASHES:
            case ANYCASE_WITHOUT_DASHES:
                if ((!canExtract(string, offset, 16, 0L, format)) || (!canExtract(string, offset + 16, 16, 0L, format))) return false;
                msb = extract(string, offset, 16, 0L, format);
                lsb = extract(string, offset + 16, 16, 0L, format);
                break;
        }

        return true;
    }

    ////

    public UUID assign(CharSequence string) {
        if (isTooLong(string, UUIDParseFormat.ANY)) {
            throw new IllegalArgumentException("String 'string' is too long.");
        }
        return assign(string, 0);
    }

    public UUID assign(CharSequence string, int offset) {
        return assign(string, offset, UUIDParseFormat.ANY);
    }

    @Override
    public UUID assign(CharSequence string, UUIDParseFormat format) {
        if (isTooLong(string, format)) {
            throw new IllegalArgumentException("String 'string' is too long.");
        }
        return assign(string, 0, format);
    }

    @Override
    public UUID assign(CharSequence string, int offset, UUIDParseFormat format) {
        if (string == null)
            throw new IllegalArgumentException("Argument 'string' cannot be null.");
        if (string.length() < offset + 32)
            throw new IllegalArgumentException("String 'string' is too short.");

        if (format == UUIDParseFormat.ANY)
            format = string.charAt(offset + 8) == '-' ? UUIDParseFormat.ANYCASE : UUIDParseFormat.ANYCASE_WITHOUT_DASHES;

        switch (format) {
            case LOWERCASE:
            case UPPERCASE:
            case ANYCASE: {
                long m = extract(string, offset, 8, 0L, format);
                if (string.charAt(offset + 8) != '-')
                    throw new IllegalArgumentException("UUID is improperly formatted.");
                m = extract(string, offset + 9, 4, m, format);
                if (string.charAt(offset + 13) != '-')
                    throw new IllegalArgumentException("UUID is improperly formatted.");
                m = extract(string, offset + 14, 4, m, format);
                if (string.charAt(offset + 18) != '-')
                    throw new IllegalArgumentException("UUID is improperly formatted.");

                long l = extract(string, offset + 19, 4, 0L, format);
                if (string.charAt(offset + 23) != '-')
                    throw new IllegalArgumentException("UUID is improperly formatted.");
                l = extract(string, offset + 24, 12, l, format);

                msb = m;
                lsb = l;
                break;
            }
            case LOWERCASE_WITHOUT_DASHES:
            case UPPERCASE_WITHOUT_DASHES:
            case ANYCASE_WITHOUT_DASHES:
                msb = extract(string, offset, 16, 0L, format);
                lsb = extract(string, offset + 16, 16, 0L, format);
                break;
        }

        return this;
    }

    @Override
    public java.util.UUID toJavaUUID() {
        return new java.util.UUID(getMSB(), getLSB());
    }

    @Override
    public byte[] toBytes() {
        byte[] data = new byte[16];
        long m = msb;
        for (int i = 0; i < 8; ++i) {
            data[7 - i] = (byte) (m & 0xFF);
            m >>= 8;
        }
        long l = lsb;
        for (int i = 0; i < 8; ++i) {
            data[15 - i] = (byte) (l & 0xFF);
            l >>= 8;
        }
        return data;
    }

    @Override
    public void toBytes(byte[] data) {
        long m = msb;
        for (int i = 0; i < 8; ++i) {
            data[7 - i] = (byte) (m & 0xFF);
            m >>= 8;
        }
        long l = lsb;
        for (int i = 0; i < 8; ++i) {
            data[15 - i] = (byte) (l & 0xFF);
            l >>= 8;
        }
    }

    @Override
    public UUID assign(byte[] bytes) {
        return assign(bytes, 0);
    }

    @Override
    public UUID assign(byte[] bytes, int offset) {
        if (bytes == null)
            throw new IllegalArgumentException("Argument 'bytes' cannot be null.");
        if (bytes.length < 16 + offset)
            throw new IllegalArgumentException("Array 'bytes' is too short.");

        long m = 0;
        for (int i = 0; i < 8; i += 1)
            m = (m << 8) | (bytes[offset + i] & 0xFF);

        long l = 0;
        for (int i = 8; i < 16; i += 1)
            l = (l << 8) | (bytes[offset + i] & 0xFF);

        msb = m;
        lsb = l;
        return this;
    }

    @Override
    public UUID assign(BinaryArrayReadOnly source) {
        return assign(source, 0);
    }

    @Override
    public UUID assign(BinaryArrayReadOnly source, int offset) {
        if (source == null)
            throw new IllegalArgumentException("Argument 'source' cannot be null.");
        if (source.size() < 16 + offset)
            throw new IllegalArgumentException("BinaryArray 'source' is too short.");

        long m = 0;
        for (int i = 0; i < 8; i += 1)
            m = (m << 8) | (source.get(offset + i) & 0xFF);

        long l = 0;
        for (int i = 8; i < 16; i += 1)
            l = (l << 8) | (source.get(offset + i) & 0xFF);

        msb = m;
        lsb = l;
        return this;
    }

    public static UUID random() {
        final UUID uuid = new UUID();
        uuid.randomize();
        return uuid;
    }

    private static boolean isTooLong(CharSequence string, UUIDParseFormat format) {
        if (format == UUIDParseFormat.ANY)
            format = string.charAt(8) == '-' ? UUIDParseFormat.ANYCASE : UUIDParseFormat.ANYCASE_WITHOUT_DASHES;
        switch (format) {
            case LOWERCASE:
            case UPPERCASE:
            case ANYCASE: {
                if (string.length() > 36)
                    return true;
                break;
            }
            case LOWERCASE_WITHOUT_DASHES:
            case UPPERCASE_WITHOUT_DASHES:
            case ANYCASE_WITHOUT_DASHES:
                if (string.length() > 32)
                    return true;
                break;
        }
        return false;
    }


    private static long extract(CharSequence source, int offset, int count, long accumulator, UUIDParseFormat format) {
        for (int i = offset; i < offset + count; i += 1) {
            int d = hexDigit(source.charAt(i), format);
            if (d == -1)
                throw new IllegalArgumentException("UUID is improperly formatted.");
            accumulator = (accumulator << 4) | d;
        }
        return accumulator;
    }

    private static boolean canExtract(CharSequence source, int offset, int count, long accumulator, UUIDParseFormat format) {
        if (offset + count - 1 >= source.length()) return false;
        for (int i = offset; i < offset + count; i += 1) {
            int d = hexDigit(source.charAt(i), format);
            if (d == -1) return false;
        }
        return true;
    }

    private static int hexDigit(char c, UUIDParseFormat format) {
        if (c >= '0' && c <= '9')
            return c - '0';
        switch (format) {
            case LOWERCASE:
            case LOWERCASE_WITHOUT_DASHES:
                if (c >= 'a' && c <= 'f')
                    return 10 + c - 'a';
                return -1;
            case UPPERCASE:
            case UPPERCASE_WITHOUT_DASHES:
                if (c >= 'A' && c <= 'F')
                    return 10 + c - 'A';
                return -1;
            case ANYCASE:
            case ANYCASE_WITHOUT_DASHES:
            case ANY:
                if (c >= 'a' && c <= 'f')
                    return 10 + c - 'a';
                if (c >= 'A' && c <= 'F')
                    return 10 + c - 'A';
                return -1;
        }
        return -1;
    }

    private static int compareUnsigned(long x, long y) {
        return Long.compare(x + Long.MIN_VALUE, y + Long.MIN_VALUE);
    }
}


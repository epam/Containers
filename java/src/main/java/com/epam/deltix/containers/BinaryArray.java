package com.epam.deltix.containers;

import com.epam.deltix.containers.binaryarrayoptimizations.BinaryArrayOptimization;
import com.epam.deltix.containers.binaryarrayoptimizations.BinaryArrayOptimizationTotal;
import com.epam.deltix.containers.generated.*;
import com.epam.deltix.containers.interfaces.*;

import java.io.IOException;
import java.io.InputStream;
import java.util.Arrays;

public class BinaryArray implements BinaryConvertibleReadWrite {
    private static final char[] HEX_DIGITS_UPPER = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
    private static BinaryArrayOptimization[] _optimizations = new BinaryArrayOptimization[]
            {
                    new BinaryArrayOptimization0(),
                    new BinaryArrayOptimization1(),
                    new BinaryArrayOptimization2(),
                    new BinaryArrayOptimization3(),
                    new BinaryArrayOptimization4(),
                    new BinaryArrayOptimization5(),
                    new BinaryArrayOptimization6(),
                    new BinaryArrayOptimization7(),
                    new BinaryArrayOptimization8(),
                    new BinaryArrayOptimizationTotal()
            };
    private int count = 0;
    private int hashCode;
    protected long[] data;

    public BinaryArray() {
        this(8);
    }

    public BinaryArray(int capacity) {
        data = new long[(capacity + 7) >>> 3];
    }

    public BinaryArray(byte x) {
        this(0x10);
        assign(x);
    }

    public BinaryArray(int capacity, byte x) {
        this(capacity);
        assign(x);
    }

    public BinaryArray(int capacity, short x) {
        this(capacity);
        assign(x);
    }

    public BinaryArray(int capacity, int x) {
        this(capacity);
        assign(x);
    }

    public BinaryArray(int capacity, long x) {
        this(capacity);
        assign(x);
    }

    public BinaryArray(int capacity, float x) {
        this(capacity);
        assign(x);
    }

    public BinaryArray(int capacity, double x) {
        this(capacity);
        assign(x);
    }

    public BinaryArray(byte[] x) {
        this(x.length);
        assign(x);
    }

    public BinaryArray(String x) {
        this(x.length());
        assign(x);
    }

    public BinaryArray(ReadOnlyString x) {
        this(x.length() * 2);
        assign(x);
    }

    public BinaryArray(InputStream x) {
        this(10);
        assign(x);
    }

    public BinaryArray(UUIDReadOnly x) {
        this(16);
        assign(x);
    }

    public BinaryArray(BinaryArrayReadOnly x) {
        this(x.capacity());
        assign(x);
    }

    private void resize(int newSize) {
        if (((newSize + 8) >>> 3) <= data.length)
            return;
        data = Arrays.copyOf(data, (newSize + 8) >>> 3);
    }


    @Override
    public void copyTo(BinaryArrayReadWrite destination) {
        destination.assign(this);
    }

    private void internalAppend(long buffer)
    {
        UnsafeHelper.setLongToLongArray(data, count, buffer);
        count += 8;
    }

    private void internalAppend(int buffer)
    {
        UnsafeHelper.setIntToLongArray(data, count, buffer);
        count += 4;
    }

    private void internalAppend(short buffer)
    {
        internalAppend((byte) (buffer & 255));
        internalAppend((byte) (buffer >>> 8));
    }

    private void internalAppend(byte buffer) {
        UnsafeHelper.setByteAtLongArray(data, count, buffer);
        count++;
    }

    @Override
    public BinaryArray append(ReadOnlyString str) {
        hashCode = hashCode | 0x40000000;
        resize(str.length() + count + 8);
        for (int i = 0; i < str.length(); ++i)
            internalAppend((short) str.getCharAt(i));
        return this;
    }


    @Override
    public BinaryArray append(byte[] bytes, int offset, int count) {
        hashCode = hashCode | 0x40000000;
        resize(this.count + count + 2);
        UnsafeHelper.memMoveFromByteToLong(bytes, offset, data, this.count, count);
        this.count += count;
        return this;
    }

    @Override
    public BinaryArray append(BinaryArrayReadOnly str) {
        hashCode = hashCode | 0x40000000;
        resize(str.size() + count + 8);
        for (int i = 0; i < str.size(); ++i)
            internalAppend(str.get(i));
        return this;
    }

    @Override
    public BinaryArray append(byte buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 1);
        internalAppend(buffer);
        return this;
    }

    @Override
    public BinaryArray append(boolean buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 1);
        if (buffer)
            internalAppend((byte) 1);
        else internalAppend((byte) 0);
        return this;
    }

    @Override
    public BinaryArray append(char buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 2);
        internalAppend((short) buffer);
        return this;
    }

    @Override
    public BinaryArray append(UUIDReadOnly buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 16);
        internalAppend(buffer.getMSB());
        internalAppend(buffer.getLSB());
        return this;
    }

    @Override
    public BinaryArray append(double buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 8);
        internalAppend(Double.doubleToLongBits(buffer));
        return this;
    }

    @Override
    public BinaryArray append(float buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 4);
        internalAppend(Float.floatToIntBits(buffer));
        return this;
    }

    @Override
    public BinaryArray append(short buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 2);
        internalAppend(buffer);
        return this;
    }

    @Override
    public BinaryArray append(int buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 4);
        internalAppend(buffer);
        return this;
    }

    @Override
    public BinaryArray append(long buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 8);
        internalAppend(buffer);
        return this;
    }


    public BinaryArray append(CharSequence str) {
        if (str instanceof BinaryArray) return append((BinaryArrayReadOnly) str);
        hashCode = hashCode | 0x40000000;
        resize((str.length() << 1) + count + 8);
        for (int i = 0; i < str.length(); ++i) {
            char ch = str.charAt(i);
            internalAppend((short) ch);
        }
        return this;
    }

    @Override
    public BinaryArray append(String str) {
        hashCode = hashCode | 0x40000000;
        resize((str.length() << 1) + count + 8);
        for (int i = 0; i < str.length(); ++i) {
            char ch = str.charAt(i);
            internalAppend((short) ch);
        }
        return this;
    }


    @Override
    public BinaryArray append(String str, boolean isASCII) {
        if (isASCII) {
            hashCode = hashCode | 0x40000000;
            resize(str.length() + count + 8);
            for (int i = 0; i < str.length(); ++i) {
                char ch = str.charAt(i);
                internalAppend((byte) ch);
            }
        } else {
            append(str);
        }
        return this;
    }

    @Override
    public BinaryArray append(CharSequence str, boolean isASCII) {
        if (isASCII) {
            hashCode = hashCode | 0x40000000;
            resize(str.length() + count + 8);
            for (int i = 0; i < str.length(); ++i) {
                char ch = str.charAt(i);
                internalAppend((byte) ch);
            }
        } else {
            append(str);
        }
        return this;
    }

    public BinaryArray assign(InputStream stream) {
        clear();
        append(stream);
        return this;
    }

    public BinaryArray append(InputStream stream) {
        hashCode = hashCode | 0x40000000;
        boolean notEndStream = true;
        while (notEndStream) {
            try {
                int x = stream.read();
                if (x == -1) {
                    notEndStream = false;
                } else {
                    append((byte) x);
                }
            } catch (IOException e) {
                notEndStream = false;
            }
        }
        return this;
    }

    @Override
    public BinaryArray assign(UUIDReadOnly buffer) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(ReadOnlyString str) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(str);
    }

    @Override
    public BinaryArray assign(byte[] bytes) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(bytes);
    }

    @Override
    public BinaryArray assign(byte[] bytes, int offset, int count) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(bytes, offset, count);
    }

    @Override
    public BinaryArray copyFrom(BinaryArrayReadOnly buffer) {
        return assign(buffer);
    }

    @Override
    public BinaryArray assign(BinaryArrayReadOnly buffer) {
        hashCode = hashCode | 0x40000000;
        clear();
        if (buffer instanceof BinaryArray) {
            BinaryArray array = (BinaryArray) buffer;
            resize(array.count + 8);
            count = array.count;
            hashCode = array.hashCode;
            UnsafeHelper.memMoveFromLongToLong(array.data, 0, data, 0, count);
        } else append(buffer);
        return this;
    }

    @Override
    public BinaryArray assign(byte buffer) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(boolean buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(char buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(double buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(float buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(short buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(int buffer) {
        clear();
        return append(buffer);
    }


    @Override
    public BinaryArray assign(long buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public BinaryArray assign(String str) {
        clear();
        return append(str);
    }

    @Override
    public BinaryArray assign(String str, boolean isASCII) {
        clear();
        return append(str, isASCII);
    }

    @Override
    public BinaryArray assign(CharSequence str, boolean isASCII) {
        if (str instanceof BinaryArray) return assign((BinaryArray) str);
        clear();
        return append(str, isASCII);
    }

    @Override
    public BinaryArray insert(int index, ReadOnlyString str) {
        resize(2 * (count + str.length() * 2));
        for (int i = count + str.length() * 2 - 1; i > index + str.length() * 2 - 1; i--)
            setByteAt(i, getByteAt(i - str.length() * 2));
        int lastCount = count + str.length() * 2;
        count = index;
        append(str);
        count = lastCount;
        return this;
    }

    @Override
    public BinaryArray insert(int index, byte item) {
        resize(2 * (count + 1));
        for (int i = count; i > index; i--)
            setByteAt(i, getByteAt(i - 1));
        int lastCount = count + 1;
        count = index;
        append(item);
        count = lastCount;
        return this;
    }

    @Override
    public BinaryArray insert(int index, short item) {
        resize(2 * (count + 2));
        for (int i = count + 1; i > index + 1; i--)
            setByteAt(i, getByteAt(i - 2));
        int lastCount = count + 2;
        count = index;
        append(item);
        count = lastCount;
        return this;
    }

    @Override
    public BinaryArray insert(int index, char item) {
        insert(index, (short) item);
        return this;
    }

    @Override
    public BinaryArray insert(int index, int item) {
        resize(2 * (count + 4));
        for (int i = count + 3; i > index + 3; i--)
            setByteAt(i, getByteAt(i - 4));
        int lastCount = count + 4;
        count = index;
        append(item);
        count = lastCount;
        return this;
    }

    @Override
    public BinaryArray insert(int index, long item) {
        resize(2 * (count + 8));
        for (int i = count + 7; i > index + 7; i--)
            setByteAt(i, getByteAt(i - 8));
        int lastCount = count + 8;
        count = index;
        append(item);
        count = lastCount;
        return this;
    }

    @Override
    public BinaryArray insert(int index, double item) {
        resize(2 * (count + 8));
        for (int i = count + 7; i > index + 7; i--)
            setByteAt(i, getByteAt(i - 8));
        int lastCount = count + 8;
        count = index;
        append(item);
        count = lastCount;
        return this;

    }

    @Override
    public BinaryArray insert(int index, float item) {
        resize(2 * (count + 4));
        for (int i = count + 3; i > index + 3; i--)
            setByteAt(i, getByteAt(i - 4));
        int lastCount = count + 4;
        count = index;
        append(item);
        count = lastCount;
        return this;

    }

    @Override
    public BinaryArray insert(int index, byte[] item) {
        resize(2 * (count + item.length));
        for (int i = count + item.length - 1; i > index + item.length - 1; i--)
            setByteAt(i, getByteAt(i - item.length));
        int lastCount = count + item.length;
        count = index;
        append(item);
        count = lastCount;
        return this;

    }

    @Override
    public BinaryArray insert(int index, byte[] item, int offset, int byteCount) {
        resize(2 * (count + byteCount));
        for (int i = count + byteCount - 1; i > index + byteCount - 1; i--)
            setByteAt(i, getByteAt(i - byteCount));
        int lastCount = count + byteCount;
        count = index;
        append(item);
        count = lastCount;
        return this;

    }

    @Override
    public BinaryArray insert(int index, boolean item) {
        resize(2 * (count + 1));
        for (int i = count; i > index; i--)
            setByteAt(i, getByteAt(i - 1));
        int lastCount = count + 1;
        count = index;
        append(item);
        count = lastCount;
        return this;

    }

    @Override
    public BinaryArray insert(int index, BinaryArrayReadOnly item) {
        resize(2 * (count + item.size()));
        for (int i = count + item.size() - 1; i > index + item.size() - 1; i--)
            setByteAt(i, getByteAt(i - item.size()));
        int lastCount = count + item.size();
        count = index;
        append(item);
        count = lastCount;
        return this;
    }

    @Override
    public BinaryArray insert(int index, String item) {
        resize(2 * (count + 2 * item.length()));
        for (int i = count + 2 * item.length() - 1; i > index + 2 * item.length() - 1; i--)
            setByteAt(i, getByteAt(i - 2 * item.length()));
        int lastCount = count + 2 * item.length();
        count = index;
        append(item);
        count = lastCount;
        return this;

    }

    @Override
    public BinaryArray insert(int index, String item, boolean isASCII) {
        if (!isASCII)
            insert(index, item);
        resize(2 * (count + item.length()));
        for (int i = count + item.length() - 1; i > index + item.length() - 1; i--)
            setByteAt(i, getByteAt(i - item.length()));
        int lastCount = count + item.length();
        count = index;
        append(item);
        count = lastCount;
        return this;
    }

    @Override
    public BinaryArray removeAt(int index) {
        hashCode = hashCode | 0x40000000;
        for (int i = index + 1; i < count; ++i)
            setByteAt(i - 1, getByteAt(i));
        setByteAt(count - 1, (byte) 0);
        count--;
        return this;
    }

    @Override
    public BinaryArray append(byte[] bytes) {
        hashCode = hashCode | 0x40000000;
        resize(count + bytes.length + 2);
        UnsafeHelper.memMoveFromByteToLong(bytes, 0, data, count, bytes.length);
        count += bytes.length;
        return this;
    }


    @Override
    public boolean toBoolean() {
        return toBoolean(0);
    }

    @Override
    public byte toByte() {
        return getByteAt(0);
    }

    @Override
    public char toChar() {
        return toChar(0);
    }

    @Override
    public double toDouble() {
        return toDouble(0);
    }

    @Override
    public short toInt16() {
        return toInt16(0);
    }

    @Override
    public int toInt32() {
        return toInt32(0);
    }

    @Override
    public long toInt64() {
        return toInt64(0);
    }

    @Override
    public float toSingle() {
        return toSingle(0);
    }

    @Override
    public void toUUID(com.epam.deltix.containers.interfaces.UUID buffer) {
        buffer.setMSB(toInt64(0));
        buffer.setLSB(toInt64(8));
    }

    @Override
    public void toUUID(com.epam.deltix.containers.interfaces.UUID buffer, int offset) {
        buffer.setMSB(toInt64(offset));
        buffer.setLSB(toInt64(offset + 8));
    }

    @Override
    public void toMutableString(com.epam.deltix.containers.interfaces.MutableString str) {
        toMutableString(str, 0);
    }

    @Override
    public void toMutableString(com.epam.deltix.containers.interfaces.MutableString str, int offset) {
        str.clear();
        for (int i = offset; i < getCount(); i += 2)
            str.append(toChar(i));
    }

    @Override
    public void toMutableString(com.epam.deltix.containers.interfaces.MutableString str, int offset, int length) {
        str.clear();
        for (int i = offset; i < offset + length; i += 2)
            str.append(toChar(i));
    }

    @Override
    public void toUTF8(byte[] utf8) {
        for (int i = 0; i < count; ++i)
            utf8[i] = getByteAt(i);
    }

    @Override
    public boolean toBoolean(int offset) {
        return getByteAt(offset) == 1;
    }

    @Override
    public byte toByte(int offset) {
        return getByteAt(offset);
    }

    @Override
    public char toChar(int offset) {
        return (char) toInt16(offset);
    }

    @Override
    public double toDouble(int offset) {
        return Double.longBitsToDouble(toInt64(offset));
    }

    @Override
    public short toInt16(int offset) {
        return (short) (((((short) ((short) ((short) getByteAt(offset + 1) & 255) << 8)) & (short) (0xff00)) |
                ((short) ((short) getByteAt(offset) & 255) & (short) (0xff))));
    }

    @Override
    public int toInt32(int offset) {
        return (((int) getByteAt(offset + 3) & 255) << 24) & 0xff000000 |
                (((int) getByteAt(offset + 2) & 255) << 16) & 0x00ff0000 |
                (((int) getByteAt(offset + 1) & 255) << 8) & 0x0000ff00 |
                ((int) getByteAt(offset) & 255) & 0x000000ff;

    }

    @Override
    public long toInt64(int offset) {
        return (((long) getByteAt(offset + 7) & 255) << 56) & 0xff00000000000000L |
                (((long) getByteAt(offset + 6) & 255) << 48) & 0x00ff000000000000L |
                (((long) getByteAt(offset + 5) & 255) << 40) & 0x0000ff0000000000L |
                (((long) getByteAt(offset + 4) & 255) << 32) & 0x000000ff00000000L |
                (((long) getByteAt(offset + 3) & 255) << 24) & 0x00000000ff000000L |
                (((long) getByteAt(offset + 2) & 255) << 16) & 0x0000000000ff0000L |
                (((long) getByteAt(offset + 1) & 255) << 8) & 0x000000000000ff00L |
                (long) getByteAt(offset) & 0x000000000000ffL;
    }

    @Override
    public float toSingle(int offset) {
        return Float.intBitsToFloat(toInt32(offset));
    }

    @Override
    public void toUTF8(byte[] utf8, int offset) {
        for (int i = offset; i < count; ++i)
            utf8[i - offset] = getByteAt(i);
    }


    public boolean equals(BinaryConvertibleReadOnly another) {
        if (count != another.getCount())
            return false;
        for (int i = 0; i < count; ++i)
            if (get(i) != another.get(i))
                return false;
        return true;
    }

    public boolean equals(BinaryArray another) {
        if (count != another.getCount())
            return false;
        int a = ((count + 7) >> 3);
        return _optimizations[9 + ((a - 9) & (a - 9) >> 31)].equals(data, another.data, a);

    }

    @Override
    public boolean equals(Object that) {
        if (that == null) {
            return false;
        } else if (that instanceof BinaryArray) {
            return equals((BinaryArray) that);
        } else if (that instanceof BinaryConvertibleReadOnly) {
            return equals((BinaryConvertibleReadOnly) that);
        } else if (that instanceof CharSequence) {
            return equals((CharSequence) that);
        } else
            return false;
    }

    /**
     * Copy count bytes from BinaryArray started from source index to byte array destination started from destination index.
     * @param sourceIndex source index
     * @param destination destination
     * @param destinationIndex destination index
     * @param count count
     */
    @Override
    public void toByteArray(int sourceIndex, byte[] destination, int destinationIndex, int count) {
        UnsafeHelper.memMoveFromLongToByte(data, sourceIndex, destination, destinationIndex, count);
    }

    @Override
    public int hashCode() {
        if ((hashCode & 0x40000000) != 0) {
            int a = (count + 7) >> 3;
            int tempHashCode = (int) _optimizations[9 + ((a - 9) & (a - 9) >> 31)].xxHash64(data, count);
            hashCode = tempHashCode & (0x3FFFFFFF);
        }
        return hashCode;
    }

    @Override
    public byte[] toByteArray() {
        byte[] temp = new byte[count];
        UnsafeHelper.memMoveFromLongToByte(data, 0, temp, 0, count);
        return temp;
    }

    @Override
    public void getBytes(byte[] buffer) {
        toByteArray(0, buffer, 0, count);
    }

    @Override
    public void getBytes(byte[] buffer, int srcOffset, int size) {
        toByteArray(srcOffset, buffer, 0, size);
    }

    @Override
    public void getBytes(byte[] buffer, int srcOffset, int dstOffset, int size) {
        toByteArray(srcOffset, buffer, dstOffset, size);
    }

    @Override
    public BinaryArray clone() {
        return new BinaryArray(this);
    }

    @Override
    public BinaryArray set(int index, byte x) {
        setByteAt(index, x);
        return this;
    }

    @Override
    public byte get(int index) {
        return getByteAt(index);
    }

    @Override
    public int size() {
        return getCount();
    }

    @Override
    public int capacity() {
        return getCapacity();
    }

    @Override
    public String toString(boolean isASCII) {
        StringBuilder builder = new StringBuilder();
        if (isASCII) {
            for (int i = 0; i < count; ++i)
                builder.append((char) getByteAt(i));
        } else {
            for (int i = 0; i < count; i += 2)
                builder.append(toChar(i));
        }
        return builder.toString();
    }

    @Override
    public long toLong() {
        return toLong(0);
    }

    @Override
    public long toLong(int offset) {
        return toInt64(offset);
    }

    @Override
    public void getChars(CharEncoding charset, Appendable str) {
        getChars(charset, str, 0);
    }

    @Override
    public void getChars(CharEncoding charset, Appendable str, int offset) {
        try {

            if (charset == CharEncoding.ASCII) {
                for (int i = offset; i < count; ++i) str.append((char) getByteAt(i));
            } else if (charset == CharEncoding.HEX) {
                for (int i = offset; i < count; ++i) {
                    int v = getByteAt(i) & 0xFF;
                    str.append(HEX_DIGITS_UPPER[v >>> 4]);
                    str.append(HEX_DIGITS_UPPER[v & 0x0F]);
                }
            } else {
                for (int i = offset; i < count; i += 2) {
                    str.append(toChar(i));
                }
            }
        } catch (IOException ignored) {
        }
    }

    public String toString(int offset) {
        StringBuilder builder = new StringBuilder();
        for (int i = offset; i < count; i += 2)
            builder.append(toChar(i));
        return builder.toString();
    }

    @Override
    public String toString(CharEncoding charset, int offset) {
        return toString(offset);
    }

    @Override
    public String toString(CharEncoding charset) {
        if (charset == CharEncoding.ASCII) {
            return toString(true);
        } else if (charset == CharEncoding.HEX) {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < count; ++i) {
                int v = getByteAt(i) & 0xFF;
                str.append(HEX_DIGITS_UPPER[v >>> 4]);
                str.append(HEX_DIGITS_UPPER[v & 0x0F]);
            }
            return str.toString();
        } else {
            return toString(false);
        }
    }

    @Override
    public boolean equals(BinaryIdentifierReadOnly another) {
        if (count != another.size())
            return false;
        for (int i = 0; i < count; ++i)
            if (get(i) != another.get(i))
                return false;
        return true;

    }


    @Override
    public String toString() {
        return toString(false);
    }

    @Override
    public UUID toUUID() {
        UUID uuid = new UUID();
        toUUID(uuid);
        return uuid;
    }

    @Override
    public MutableString toMutableString() {
        MutableString string = new MutableString();
        toMutableString(string);
        return string;
    }

    @Override
    public byte getByteAt(int index) {
        return UnsafeHelper.getByteFromLongArray(data, index);
    }

    @Override
    public int getCount() {
        return count;
    }

    @Override
    public int getCapacity() {
        return data.length << 3;
    }

    @Override
    public void setByteAt(int index, byte x) {
        UnsafeHelper.setByteAtLongArray(data, index, x);
        hashCode = hashCode | 0x40000000;
    }

    @Override
    public BinaryArray clear() {
        int len = ((count + 7) >>> 3);
        for (int i = 0; i < len; ++i)
            data[i] = 0L;
        count = 0;
        hashCode = hashCode | 0x40000000;
        return this;
    }

    public BinaryArrayInputStream toInputStream() {
        return new BinaryArrayInputStream(this);
    }

    public BinaryArrayInputStream toInputStream(BinaryArrayInputStream toReuse) {
        toReuse.updateDataSource(this);
        return toReuse;
    }

    public boolean isEmpty() {
        return count == 0;
    }
}

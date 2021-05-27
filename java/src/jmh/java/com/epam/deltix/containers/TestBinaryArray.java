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

import com.epam.deltix.containers.binaryarrayoptimizations.BinaryArrayOptimization;
import com.epam.deltix.containers.binaryarrayoptimizations.BinaryArrayOptimizationTotal;
import com.epam.deltix.containers.interfaces.*;
import com.epam.deltix.containers.generated.*;
import com.epam.deltix.containers.interfaces.*;

import java.io.IOException;
import java.io.InputStream;
import java.util.Arrays;

public class TestBinaryArray implements BinaryConvertibleReadWrite {
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


    private long[] data;

    public TestBinaryArray() {
        this(8);
    }

    public TestBinaryArray(int capacity) {
        data = new long[(capacity + 7) >>> 3];
    }

    public TestBinaryArray(byte x) {
        this(0x10);
        assign(x);
    }

    public TestBinaryArray(int capacity, byte x) {
        this(capacity);
        assign(x);
    }

    public TestBinaryArray(int capacity, short x) {
        this(capacity);
        assign(x);
    }

    public TestBinaryArray(int capacity, int x) {
        this(capacity);
        assign(x);
    }

    public TestBinaryArray(int capacity, long x) {
        this(capacity);
        assign(x);
    }

    public TestBinaryArray(int capacity, float x) {
        this(capacity);
        assign(x);
    }

    public TestBinaryArray(int capacity, double x) {
        this(capacity);
        assign(x);
    }

    public TestBinaryArray(byte[] x) {
        this(x.length);
        assign(x);
    }

    public TestBinaryArray(String x) {
        this(x.length());
        assign(x);
    }

    public TestBinaryArray(ReadOnlyString x) {
        this(x.length() * 2);
        assign(x);
    }

    public TestBinaryArray(InputStream x) {
        this(10);
        assign(x);
    }

    public TestBinaryArray(UUIDReadOnly x) {
        this(16);
        assign(x);
    }

    public TestBinaryArray(BinaryArrayReadOnly x) {
        this(x.capacity());
        assign(x);
    }

    private void resize(int newSize) {
        if (((newSize + 8) >>> 3) <= data.length)
            return;
        data = Arrays.copyOf(data, (newSize + 8) >>> 2);
    }


    @Override
    public void copyTo(BinaryArrayReadWrite destination) {
        destination.assign(this);
    }

    @Override
    public TestBinaryArray append(ReadOnlyString str) {
        hashCode = hashCode | 0x40000000;
        for (int i = 0; i < str.length(); ++i)
            append(str.getCharAt(i));
        return this;
    }

    @Override
    public TestBinaryArray append(byte[] bytes) {
        hashCode = hashCode | 0x40000000;
        resize(count + bytes.length + 2);
        for (int i = 0; i < bytes.length; ++i)
            internalAppend(bytes[i]);
        return this;
    }

    @Override
    public TestBinaryArray append(byte[] bytes, int offset, int count) {
        hashCode = hashCode | 0x40000000;
        for (int i = offset; i < offset + count; ++i)
            append(bytes[i]);
        return this;
    }

    @Override
    public TestBinaryArray append(BinaryArrayReadOnly str) {
        hashCode = hashCode | 0x40000000;
        resize(str.size() + count + 8);
        for (int i = 0; i < str.size(); ++i)
            internalAppend(str.get(i));

        return this;
    }


    private void internalAppend(byte buffer) {
        setByteAtZeroPosition(count, buffer);
        count++;
    }

    @Override
    public TestBinaryArray append(byte buffer) {
        hashCode = hashCode | 0x40000000;
        resize(count + 1);
        setByteAt(count, buffer);
        count++;
        return this;
    }

    @Override
    public TestBinaryArray append(boolean buffer) {
        hashCode = hashCode | 0x40000000;
        if (buffer)
            append((byte) 1);
        else append((byte) 0);
        return this;
    }

    @Override
    public TestBinaryArray append(char buffer) {
        hashCode = hashCode | 0x40000000;
        append((short) buffer);
        return this;
    }

    @Override
    public TestBinaryArray append(UUIDReadOnly buffer) {
        hashCode = hashCode | 0x40000000;
        append(buffer.getMSB());
        append(buffer.getLSB());
        return this;
    }

    @Override
    public TestBinaryArray append(double buffer) {
        hashCode = hashCode | 0x40000000;
        append(Double.doubleToLongBits(buffer));
        return this;
    }

    @Override
    public TestBinaryArray append(float buffer) {
        hashCode = hashCode | 0x40000000;
        append(Float.floatToIntBits(buffer));
        return this;
    }

    @Override
    public TestBinaryArray append(short buffer) {
        hashCode = hashCode | 0x40000000;
        append((byte) (buffer & 255));
        append((byte) (buffer >>> 8));
        return this;
    }

    @Override
    public TestBinaryArray append(int buffer) {
        hashCode = hashCode | 0x40000000;
        append((byte) (buffer & 255));
        append((byte) ((buffer >>> 8) & 255));
        append((byte) ((buffer >>> 16) & 255));
        append((byte) (buffer >>> 24));
        return this;
    }

    @Override
    public TestBinaryArray append(long buffer) {
        hashCode = hashCode | 0x40000000;
        append((byte) (buffer & 255));
        append((byte) ((buffer >>> 8) & 255));
        append((byte) ((buffer >>> 16) & 255));
        append((byte) ((buffer >>> 24) & 255));
        append((byte) ((buffer >>> 32) & 255));
        append((byte) ((buffer >>> 40) & 255));
        append((byte) ((buffer >>> 48) & 255));
        append((byte) (buffer >>> 56));
        return this;
    }

    @Override
    public TestBinaryArray append(String str) {
        hashCode = hashCode | 0x40000000;
        for (int i = 0; i < str.length(); ++i) {
            char ch = str.charAt(i);
            append((short) ch);
        }
        return this;
    }

    @Override
    public TestBinaryArray append(String str, boolean isASCII) {
        hashCode = hashCode | 0x40000000;
        if (isASCII) {
            for (int i = 0; i < str.length(); ++i) {
                char ch = str.charAt(i);
                append((byte) ch);
            }
        } else {
            append(str);
        }
        return this;
    }

    public TestBinaryArray assign(InputStream stream) {
        clear();
        append(stream);
        return this;
    }

    public TestBinaryArray append(InputStream stream) {
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
    public TestBinaryArray assign(UUIDReadOnly buffer) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(ReadOnlyString str) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(str);
    }

    @Override
    public TestBinaryArray assign(byte[] bytes) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(bytes);
    }

    @Override
    public TestBinaryArray assign(byte[] bytes, int offset, int count) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(bytes, offset, count);
    }

    @Override
    public TestBinaryArray copyFrom(BinaryArrayReadOnly buffer) {
        return assign(buffer);
    }

    @Override
    public TestBinaryArray assign(BinaryArrayReadOnly buffer) {
        hashCode = hashCode | 0x40000000;
        clear();
        if (buffer instanceof TestBinaryArray) {
            TestBinaryArray array = (TestBinaryArray) buffer;
            resize(array.count + 8);
            count = array.count;
            hashCode = array.hashCode;
            System.arraycopy(array.data, 0, data, 0, (count + 7) >> 3);
        } else append(buffer);
        return this;
    }

    @Override
    public TestBinaryArray assign(byte buffer) {
        hashCode = hashCode | 0x40000000;
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(boolean buffer) {

        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(char buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(double buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(float buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(short buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(int buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(long buffer) {
        clear();
        return append(buffer);
    }

    @Override
    public TestBinaryArray assign(String str) {
        clear();
        return append(str);
    }

    @Override
    public TestBinaryArray assign(String str, boolean isASCII) {
        clear();
        return append(str, isASCII);
    }

    @Override
    public TestBinaryArray insert(int index, ReadOnlyString str) {
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
    public TestBinaryArray insert(int index, byte item) {
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
    public TestBinaryArray insert(int index, short item) {
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
    public TestBinaryArray insert(int index, char item) {
        insert(index, (short) item);
        return this;
    }

    @Override
    public TestBinaryArray insert(int index, int item) {
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
    public TestBinaryArray insert(int index, long item) {
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
    public TestBinaryArray insert(int index, double item) {
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
    public TestBinaryArray insert(int index, float item) {
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
    public TestBinaryArray insert(int index, byte[] item) {
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
    public TestBinaryArray insert(int index, byte[] item, int offset, int byteCount) {
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
    public TestBinaryArray insert(int index, boolean item) {
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
    public TestBinaryArray insert(int index, BinaryArrayReadOnly item) {
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
    public TestBinaryArray insert(int index, String item) {
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
    public TestBinaryArray insert(int index, String item, boolean isASCII) {
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
    public TestBinaryArray removeAt(int index) {
        hashCode = hashCode | 0x40000000;
        for (int i = index + 1; i < count; ++i)
            setByteAt(i - 1, getByteAt(i));
        setByteAt(count - 1, (byte) 0);
        count--;
        return this;
    }

    public TestBinaryArray unsafeAppendByteArray(byte[] bytes) {
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

    @Override
    public boolean equals(BinaryConvertibleReadOnly another) {
        if (count != another.size())
            return false;
        for (int i = 0; i < ((count + 7) >>> 3); ++i)
            if (data[i] != another.toLong(i << 3))
                return false;
        return true;
    }

    public boolean equals(TestBinaryArray another) {
        if (count != another.getCount())
            return false;
        int a = ((count + 7) >> 3);
        return _optimizations[9 + ((a - 9) & (a - 9) >> 31)].equals(data, another.data, a);
    }

    @Override
    public boolean equals(Object that) {
        if (that == null) {
            return false;
        } else if (that instanceof TestBinaryArray) {
            return equals((TestBinaryArray) that);
        } else {
            return false;
        }
    }


    @Override
    /**
     * Copy count bytes from TestBinaryArray started from source index to byte array destination started from destination index.
     * @param sourceIndex source index
     * @param destination destination
     * @param destinationIndex destination index
     * @param count count
     */
    public void toByteArray(int sourceIndex, byte[] destination, int destinationIndex, int count) {
        for (int i = sourceIndex; i < sourceIndex + count; ++i)
            destination[destinationIndex + i - sourceIndex] = getByteAt(i);
    }

    @Override
    public int hashCode() {
        int tempHashCode;
        if ((hashCode & 0x40000000) != 0) {
            tempHashCode = 0;
            for (int i = 0; i < ((count + 7) >>> 3); ++i)
                tempHashCode ^= (int) (data[i] & 0xFFFFFFFFL) ^ (int) (data[i] >>> 32);

            hashCode = tempHashCode & (0x3FFFFFFF);
        }
        return hashCode;
    }

    public int nonCachedHashCode() {
        /*int tempHashCode = 0;
        for (int i = 0; i < ((count + 7) >>> 3); ++i)
            tempHashCode ^= (int) (data[i] & 0xFFFFFFFFL) ^ (int)(data[i] >>>  32);*/
        int a = (count + 7) >> 3;
        return (int) _optimizations[9 + ((a - 9) & (a - 9) >> 31)].xxHash64(data, count);
    }


    @Override
    public byte[] toByteArray() {
        byte[] temp = new byte[count];
        for (int i = 0; i < count; ++i)
            temp[i] = getByteAt(i);
        return temp;
    }

    @Override
    public TestBinaryArray clone() {
        return new TestBinaryArray(this);
    }

    @Override
    public BinaryIdentifierReadWrite set(int index, byte x) {
        return null;
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
                builder.append(getByteAt(i));
        } else {
            for (int i = 0; i < count; i += 2)
                builder.append(toChar(i));
        }
        return builder.toString();
    }

    @Override
    public boolean equals(BinaryIdentifierReadOnly another) {
        if (count != another.size())
            return false;
        for (int i = 0; i < ((count + 7) >>> 3); ++i)
            if (data[i] != another.toLong(i << 3))
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
        return (byte) (((data[index >>> 3]) >>> ((index & 7) << 3)) & 255);
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
        data[index >>> 3] -= (data[index >>> 3] >>> ((index & 7) << 3) & 255L) << ((index & 7) << 3);
        data[index >>> 3] += (((long) x & 255) << ((index & 7) << 3));
        hashCode = hashCode | 0x40000000;
    }

    public void setByteAtZeroPosition(int index, byte x) {
        data[index >>> 3] += (((long) x & 255) << ((index & 7) << 3));
        hashCode = hashCode | 0x40000000;
    }

    @Override
    public TestBinaryArray clear() {
        for (int i = 0; i < ((count + 8) >>> 3); ++i)
            data[i] = 0L;
        count = 0;
        hashCode = hashCode | 0x40000000;
        return this;
    }

}
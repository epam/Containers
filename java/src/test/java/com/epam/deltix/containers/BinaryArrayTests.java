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

import com.epam.deltix.containers.interfaces.CharEncoding;
import org.junit.Assert;
import org.junit.Test;
import org.junit.experimental.categories.Category;

import static org.junit.Assert.*;

@Category(Test.class)
public class BinaryArrayTests {
    private BinaryArray a = new BinaryArray();
    private BinaryArray b = new BinaryArray();
    private byte[] ar1 = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    private byte[] resultArray;
    private byte[] ar3 = {48, 49, 50, 51, 52, 53, 54, 55};


    @Test
    public void testAppendBigString()
    {
        int n = 100000;
        StringBuilder sb = new StringBuilder(n);
        for (int i = 0; i < n; ++i)
        {
            sb.append('a');
        }
        String result = sb.toString();
        a.clear();
        Assert.assertTrue(a.isEmpty());
        a.append(result);
        Assert.assertFalse(a.isEmpty());
        Assert.assertEquals(a.size(), 200000);

        b.append((CharSequence)result);
        Assert.assertEquals(b.size(), 200000);
    }

    @Test
    public void testConstructors() {
        {
            BinaryArray ba = new BinaryArray();
            Assert.assertTrue(ba.getCapacity() > 0);
        }
        {
            BinaryArray ba = new BinaryArray(17);
            Assert.assertTrue(ba.getCapacity() >= 17);
        }
        {
            BinaryArray ba = new BinaryArray((byte) 123);
            Assert.assertTrue(ba.getCapacity() > 0);
            assertEquals(123, ba.getByteAt(0));
        }
        {
            BinaryArray ba = new BinaryArray(18, (byte) 123);
            Assert.assertTrue(ba.getCapacity() >= 18);
            assertEquals(123, ba.toByte());
        }
        {
            BinaryArray ba = new BinaryArray(19, (short) 123);
            Assert.assertTrue(ba.getCapacity() >= 19);
            assertEquals(123, ba.toInt16());
        }
        {
            BinaryArray ba = new BinaryArray(20, 123);
            Assert.assertTrue(ba.getCapacity() >= 20);
            assertEquals(123, ba.toInt32());
        }
        {
            BinaryArray ba = new BinaryArray(21, 123L);
            Assert.assertTrue(ba.getCapacity() >= 21);
            assertEquals(123, ba.toInt64());
        }
        {
            BinaryArray ba = new BinaryArray(22, 12.3f);
            Assert.assertTrue(ba.getCapacity() >= 22);
            assertEquals(12.3f, ba.toSingle(), 0.000000000000001);
        }
        {
            BinaryArray ba = new BinaryArray(23, 12.3d);
            Assert.assertTrue(ba.getCapacity() >= 23);
            assertEquals(12.3d, ba.toDouble(), 0.000000000000001);
        }
        {
            byte[] input = new byte[]{1, 2, 3};
            byte[] output = new byte[3];
            BinaryArray ba = new BinaryArray(input);
            Assert.assertTrue(ba.getCapacity() > 0);
            ba.toByteArray(0, output, 0, 3);
            Assert.assertArrayEquals(input, output);
        }
        {
            String input = "234";
            BinaryArray ba = new BinaryArray(input);
            Assert.assertTrue(ba.getCapacity() > 0);
            assertEquals(input, ba.toString());
            Assert.assertEquals(input, ((Object) ba).toString());
        }
        {
            String input = "345";
            BinaryArray ba = new BinaryArray(new MutableString(input));
            Assert.assertTrue(ba.getCapacity() > 0);
            assertEquals(input, ba.toString());
        }
        {
            UUID input = UUID.random();
            BinaryArray ba = new BinaryArray(input);
            Assert.assertTrue(ba.getCapacity() > 0);
            assertEquals(input, ba.toUUID());
        }
        {
            BinaryArray input = new BinaryArray(24, 987654321L);
            BinaryArray ba = new BinaryArray(input);
            assertEquals(input, ba);
        }
        {
            BinaryArray input = new BinaryArray(new byte[0]);
            Assert.assertEquals(0, input.size());
        }
    }

    @Test
    public void testAssignAndToByte() {
        a.assign((byte) 10);
        assertEquals(a.toByte(), 10);
    }

    @Test
    public void testAssignAndToBinaryArray() {
        BinaryArray b = new BinaryArray(100);
        for (int i = 0; i < 100; ++i) b.append(i);
        a.assign(b);
        assertEquals(a.getCount(), b.getCount());
        for (int i = 0; i < a.getCount(); ++i)
            assertEquals(a.getByteAt(i), b.getByteAt(i));
    }

    @Test
    public void testAssignAndToArrayOfByte() {
        a.assign(ar1);
        assertEquals(ar1.length, a.getCount());
        for (int i = 0; i < 10; ++i)
            assertEquals(ar1[i], a.getByteAt(i));
    }

    @Test
    public void testAssignAndToBoolean() {
        a.assign(false);
        assertFalse(a.toBoolean());
        a.assign(true);
        assertTrue(a.toBoolean());
    }

    @Test
    public void testAssignAndToChar() {
        a.assign('z');
        assertEquals(a.toChar(), 'z');
    }

    @Test
    public void testAssignAndToDouble() {
        a.assign(3.14);
        assertEquals(a.toDouble(), 3.14, 0.00001);
    }

    @Test
    public void testAssignAndToInt16() {
        a.assign((short) 654);
        assertEquals(a.toInt16(), 654);
    }

    @Test
    public void testAssignAndToInt32() {
        a.assign((int) 64799);
        assertEquals(a.toInt32(), 64799);
    }

    @Test
    public void testAssignAndToInt64() {
        a.assign(54654654654L);
        assertEquals(a.toInt64(), 54654654654L);
    }

    @Test
    public void testAssignAndToSingle() {
        a.assign((float) 3.174);
        assertEquals(a.toSingle(), 3.174, 0.000001);
    }

    @Test
    public void testAssignAndToString() {
        a.assign("sdryapko");
        assertEquals(a.toString(), "sdryapko");
    }

    @Test
    public void testAssignAndToMutableString() {
        MutableString s = new MutableString();
        MutableString s1 = new MutableString();
        s.assign("sdryapko");
        a.assign(s);
        assertEquals(a.toString(), "sdryapko");
        a.toMutableString(s1);
        assertEquals(s, s1);
    }

    @Test
    public void testAssignAndToIUUID() {
        UUID uuid = new UUID();
        UUID newUuid = new UUID();
        uuid.setLSB(123);
        uuid.setMSB(5666);
        a.assign(uuid);
        a.toUUID(newUuid);
        assertEquals(newUuid.getLSB(), 123);
        assertEquals(newUuid.getMSB(), 5666);
    }

    @Test
    public void testAssignAndToByteArrayWithOffset() {
        a.assign(ar1, 2, 5);
        assertEquals(a.getCount(), 5);
        for (int i = 0; i < 5; ++i)
            assertEquals(a.getByteAt(i), i + 3);
        BinaryArray b = new BinaryArray();
        for (int i = 2; i < 7; ++i)
            b.append(ar1[i]);
        assertEquals(a, b);

    }

    @Test
    public void testAssignAndToByteArrayWithNew() {
        a.assign(ar1);
        resultArray = a.toByteArray();
        assertEquals(resultArray.length, 10);
        for (int i = 0; i < 10; ++i)
            assertEquals(resultArray[i], ar1[i]);
    }

    @Test
    public void testAssignAndToByteWithOffset() {
        a.assign(ar1);
        a.append((byte) 10);
        assertEquals(a.toByte(10), 10);
    }

    @Test
    public void testAppendByteArrayWithOffset() {
        BinaryArray b = new BinaryArray();

        a.clear();
        a.append(ar1, 0, ar1.length);
        for (int i = 0; i < ar1.length; ++i)
            b.append(ar1[i]);
        assertEquals(ar1.length, a.size());
        for (int i = 0; i < ar1.length; ++i)
            assertEquals(ar1[i], a.get(i));
        assertEquals(b, a);

        int from = 1;
        int count = 5;
        for (int testCase = 0; testCase < 100; ++testCase) {
            int offset = a.size();
            a.append(ar3, from, count);
            for (int i = from; i < from + count; ++i)
                b.append(ar3[i]);
            assertEquals("case = " + testCase, ar1.length + (testCase + 1) * count, a.size());
            for (int i = from; i < from + count; ++i)
                assertEquals("case = " + testCase, ar3[i], a.get(offset + i - from));
            assertEquals("case = " + testCase, b, a);
        }
    }

    @Test
    public void testAssignAndToArrayOfByteWithOffset() {
        a.assign(ar1);
        a.append(ar1);
        assertEquals(a.getCount(), 20);
        for (int i = 0; i < 10; ++i)
            assertEquals(a.getByteAt(i + 10), i + 1);
    }


    @Test
    public void testAssignAndToBooleanWithOffset() {
        a.assign(10);
        a.append(false);
        assertFalse(a.toBoolean(4));
        a.assign(10);
        a.append(true);
        assertTrue(a.toBoolean(4));
    }

    @Test
    public void testAssignAndToCharWithOffset() {
        a.assign('c');
        a.append('z');
        assertEquals(a.toChar(2), 'z');
    }

    @Test
    public void testAssignAndToDoubleWithOffset() {
        a.assign(ar1);
        a.append(3.14);
        assertEquals(a.toDouble(10), 3.14, 0.00001);
    }

    @Test
    public void testAssignAndToInt16WithOffset() {
        a.assign(ar1);
        a.append((short) 654);
        assertEquals(a.toInt16(10), 654);
    }

    @Test
    public void testAssignAndToInt32WithOffset() {
        a.assign(ar1);
        a.append((int) 64799);
        assertEquals(a.toInt32(10), 64799);
    }

    @Test
    public void testAssignAndToInt64WithOffset() {
        a.assign(ar1);
        a.append(54654654654L);
        assertEquals(a.toInt64(10), 54654654654L);
    }

    @Test
    public void testAssignAndToSingleWithOffset() {
        a.assign(ar1);
        a.append((float) 3.174);
        assertEquals(a.toSingle(10), 3.174, 0.000001);
    }

    @Test
    public void testAssignAndToIUUIDWithOffset() {
        a.assign(ar1);
        UUID uuid = new UUID();
        UUID newUuid = new UUID();
        uuid.setLSB(123);
        uuid.setMSB(5666);
        a.append(uuid);
        a.toUUID(newUuid, 10);
        assertEquals(newUuid.getLSB(), 123);
        assertEquals(newUuid.getMSB(), 5666);
    }

    @Test
    public void testAssignAndToMutableStringWithOffset() {
        a.assign(ar1);
        MutableString s = new MutableString();
        MutableString s1 = new MutableString();
        s.assign("sdryapko");
        a.append(s);
        a.toMutableString(s1, 10);
        assertEquals(s, s1);
    }


    @Test
    public void testEquals() {
        BinaryArray a = new BinaryArray();
        BinaryArray b = new BinaryArray();
        a.append(1);
        a.append(2);
        a.append(3);
        b.append(1);
        b.append(2);
        assertNotEquals(a, b);
        b.append(3);
        assertEquals(a, b);
    }

    @Test
    public void testEqualsToObj() {
        BinaryArray a = new BinaryArray();
        BinaryArray b = new BinaryArray();
        a.append(1);
        a.append(2);
        a.append(3);
        b.append(1);
        b.append(2);
        b.append(3);
        assertEquals(a, b);
    }


    @Test
    public void testHashCode() {
        BinaryArray a = new BinaryArray();
        BinaryArray b = new BinaryArray();
        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        b.append((byte) 1);
        b.append((byte) 2);
        assertEquals(a.hashCode() == b.hashCode(), false);
        b.append((byte) 3);
        assertEquals(a.hashCode() == b.hashCode(), true);
    }

    private static final char[] HEX_DIGITS_UPPER = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

    @Test
    public void testToHexString() {
        BinaryArray a = new BinaryArray();
        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        assertEquals("010203", a.toString(CharEncoding.HEX));
        StringBuilder builder = new StringBuilder();
        a.getChars(CharEncoding.HEX, builder, 0);
        assertEquals("010203", builder.toString());
        for (int i = 0; i < 256; ++i) {
            a.assign((byte) i);
            assertEquals("" + HEX_DIGITS_UPPER[i >>> 4] + HEX_DIGITS_UPPER[i & 0x0F], a.toString(CharEncoding.HEX));
        }

    }

    @Test
    public void testToASCIIString() {
        BinaryArray a = new BinaryArray();
        a.append((byte) 48);
        a.append((byte) 49);
        a.append((byte) 50);
        assertEquals("012", a.toString(CharEncoding.ASCII));
        StringBuilder builder = new StringBuilder();
        a.getChars(CharEncoding.ASCII, builder, 0);
        assertEquals("012", builder.toString());
    }

    @Test
    public void testUTF8String() {
        BinaryArray a = new BinaryArray();
        a.append('0');
        a.append('1');
        a.append('2');
        assertEquals("012", a.toString(CharEncoding.UTF8));
        StringBuilder builder = new StringBuilder();
        a.getChars(CharEncoding.UTF8, builder, 0);
        assertEquals("012", builder.toString());
    }

    @Test
    public void testClear() {
        BinaryArray a = new BinaryArray();

        a.append((byte) 1);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        a.append((byte) 4);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        a.append((byte) 4);
        a.append((byte) 5);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        a.append((byte) 4);
        a.append((byte) 5);
        a.append((byte) 6);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        a.append((byte) 4);
        a.append((byte) 5);
        a.append((byte) 6);
        a.append((byte) 7);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        a.append((byte) 4);
        a.append((byte) 5);
        a.append((byte) 6);
        a.append((byte) 7);
        a.append((byte) 8);
        a.clear();
        assertEquals(0, a.size());

        a.append((byte) 1);
        a.append((byte) 2);
        a.append((byte) 3);
        a.append((byte) 4);
        a.append((byte) 5);
        a.append((byte) 6);
        a.append((byte) 7);
        a.append((byte) 8);
        a.append((byte) 9);
        a.clear();
        assertEquals(0, a.size());
    }
}
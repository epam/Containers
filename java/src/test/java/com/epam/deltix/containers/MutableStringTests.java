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

import com.epam.deltix.containers.interfaces.UUIDPrintFormat;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.experimental.categories.Category;

import static org.junit.Assert.assertEquals;

@Category(Test.class)
public class MutableStringTests {
    private MutableString s = new MutableString();
    private char[] ar = {'a', 'q', 'w'};
    private UUID uuid = new UUID();

    @Before
    public void setUp() {
        uuid.fromString("0123456789ABCDEF1011121314151617");
    }


    @Test
    public void secureClearTest() {
        MutableString string = new MutableString();
        string.assign("asfkdsgjkdflsgjdflghjdflg");
        string.secureClear();
        Assert.assertEquals(0, string.length());
        BinaryArrayHelper helper = new BinaryArrayHelper();
        char data[] = helper.getInternalBuffer(string);
        for (int i = 0; i < data.length; ++i) Assert.assertEquals(0, data[i]);
    }



    @Test
    public void testToUtf8() {
        byte[] utf8test = new byte[100];
        MutableString s = new MutableString();
        s.assign("AAAaaa");

        s.toUTF8(utf8test, 0);
        for (int i = 0; i < 3; ++i) Assert.assertEquals(utf8test[i], 65);
        for (int i = 3; i < 6; ++i) Assert.assertEquals(utf8test[i], 97);

        utf8test[1] = 45;
        utf8test[2] = 45;
        s.toUTF8(0, 2, utf8test, 0);
        for (int i = 0; i < 2; ++i) Assert.assertEquals(utf8test[i], 65);
        Assert.assertEquals(utf8test[2], 45);
        utf8test[1] = 45;
        utf8test[2] = 45;
    }

    @Test
    public void testTrim() {
        assertEquals("", new MutableString().trim().toString());
        assertEquals("", new MutableString("  \t\n  ").trim().toString());
        assertEquals("no \nwhitespace", new MutableString("no \nwhitespace").trim().toString());
        assertEquals("lots\t of\nwhitespace", new MutableString("  lots\t of\nwhitespace\r\n ").trim().toString());

        assertEquals("", new MutableString().trimLeft().toString());
        assertEquals("", new MutableString("  \t\n  ").trimLeft().toString());
        assertEquals("no \nwhitespace", new MutableString("no \nwhitespace").trimLeft().toString());
        assertEquals("lots\t of\nwhitespace\r\n ", new MutableString("  lots\t of\nwhitespace\r\n ").trimLeft().toString());

        assertEquals("", new MutableString().trimRight().toString());
        assertEquals("", new MutableString("  \t\n  ").trimRight().toString());
        assertEquals("no \nwhitespace", new MutableString("no \nwhitespace").trimRight().toString());
        assertEquals("  lots\t of\nwhitespace", new MutableString("  lots\t of\nwhitespace\r\n ").trimRight().toString());
    }

    @Test
    public void subStringTest() {
        MutableString s = new MutableString("A Z235 23A AZA");
        Assert.assertEquals(s.subString(0, 0), new MutableString());
        Assert.assertEquals(s.subString(0, s.length()), s);
        Assert.assertEquals(s.subString(0, 1), new MutableString("A"));
        Assert.assertEquals(s.subString(1, 5), new MutableString(" Z23"));
        Assert.assertEquals(s.subString(2, 10), new MutableString("Z235 23A"));

        MutableString a = new MutableString();
        s.subString(a, 1, 4);
        Assert.assertEquals(a, new MutableString(" Z2"));
        Assert.assertEquals(s, new MutableString("A Z235 23A AZA"));
    }

    @Test
    public void testIsEmpty() {
        Assert.assertTrue(new MutableString().isEmpty());
        Assert.assertTrue(new MutableString("").isEmpty());
        Assert.assertTrue(new MutableString("123").clear().isEmpty());

        Assert.assertFalse(new MutableString("1").isEmpty());
        Assert.assertFalse(new MutableString().append("asd").isEmpty());
        Assert.assertFalse(new MutableString().assign("qwe").isEmpty());
    }

    @Test
    public void testAppendFloat() {
        s.assign(3.14);
        s.append(0.01);
        assertEquals(s.toString(), "3.140.01");
    }


    @Test
    public void testAppendBoolean() {
        s.assign(true);
        s.append(false);
        assertEquals(s.toString(), "TrueFalse");
    }

    @Test
    public void testAppendInteger() {
        s.assign(10);
        s.append(-10);
        assertEquals(s.toString(), "10-10");
    }

    @Test
    public void testAppendLong() {
        s.assign(1000000L);
        s.append(-1000000L);
        assertEquals(s.toString(), "1000000-1000000");
    }

    @Test
    public void testAppendShort() {
        s.assign((short) 1000);
        s.append((short) -1000);
        assertEquals(s.toString(), "1000-1000");
    }

    @Test
    public void testAppendChar() {
        s.assign('a');
        s.append('b');
        assertEquals(s.toString(), "ab");
    }

    @Test
    public void testAppendString() {
        s.assign("aba");
        s.append("caba");
        assertEquals(s.toString(), "abacaba");
    }

    @Test
    public void testAppendMutableString() {
        s.assign(new MutableString("aba"));
        s.append(new MutableString("aba"));
        assertEquals(s.toString(), "abaaba");
    }

    @Test
    public void testAppendArrayOfChar() {
        s.assign(ar);
        s.append(ar);
        assertEquals(s.toString(), "aqwaqw");
    }

    @Test
    public void testAppendArrayOfCharWithOffset() {
        s.assign(ar, 1, 2);
        s.append(ar, 2, 1);
        assertEquals(s.toString(), "qww");
    }

    @Test
    public void testAppendStringBuilder() {
        StringBuilder builder = new StringBuilder("sdsd");
        s.assign(builder);
        s.append(builder);
        assertEquals(s.toString(), "sdsdsdsd");
    }

    @Test
    public void testAppendUUID_LOWERCASE() {
        s.assign(uuid, UUIDPrintFormat.LOWERCASE);
        s.append(uuid, UUIDPrintFormat.LOWERCASE);
        assertEquals(s.toString(), "01234567-89ab-cdef-1011-12131415161701234567-89ab-cdef-1011-121314151617");
    }

    @Test
    public void testAppendUUID_UPPERCASE() {
        s.assign(uuid, UUIDPrintFormat.UPPERCASE);
        s.append(uuid, UUIDPrintFormat.UPPERCASE);
        assertEquals(s.toString(), "01234567-89AB-CDEF-1011-12131415161701234567-89AB-CDEF-1011-121314151617");
    }

    @Test
    public void testAppendUUID_LOWERCASE_WITHOUT_DASHES() {
        s.assign(uuid, UUIDPrintFormat.LOWERCASE_WITHOUT_DASHES);
        s.append(uuid, UUIDPrintFormat.LOWERCASE_WITHOUT_DASHES);
        assertEquals(s.toString(), "0123456789abcdef10111213141516170123456789abcdef1011121314151617");
    }

    @Test
    public void testAppendUUID_UPPERCASE_WITHOUT_DASHES() {
        s.assign(uuid, UUIDPrintFormat.UPPERCASE_WITHOUT_DASHES);
        s.append(uuid, UUIDPrintFormat.UPPERCASE_WITHOUT_DASHES);
        assertEquals(s.toString(), "0123456789ABCDEF10111213141516170123456789ABCDEF1011121314151617");
    }

    @Test
    public void testEquals() {
        MutableString s1 = new MutableString("abaaba");
        MutableString s2 = new MutableString("abaaba");
        MutableString s3 = new MutableString("abaabb");
        assertEquals(s1.equals(s2), true);
        assertEquals(s2.equals(s3), false);
        s2.append('c');
        assertEquals(s1.equals(s2), false);
    }

    @Test
    public void testHashCode() {
        MutableString ar = new MutableString();
        ar.clear();
        ar.append("aba");
        ar.append("caba");
        ar.append("abacaba");
        MutableString ar1 = new MutableString();
        ar1.clear();
        ar1.append("abacaba");
        ar1.append("abacaba");
        MutableString ar2 = new MutableString();
        ar2.assign("abacabaabacaba");
        assertEquals(ar1.hashCode(), ar2.hashCode());
        assertEquals(ar1.hashCode(), ar.hashCode());
    }

    @Test
    public void testHashCodeEqualsStringHashCode() {
        String s = "abacaba";
        String s1 = "asdad";
        String s2 = "qwe";
        MutableString ar = new MutableString();
        ar.assign(s);
        assertEquals(ar.hashCode(), s.hashCode());
        ar.assign(s1);
        assertEquals(ar.hashCode(), s1.hashCode());
        ar.assign(s2);
        assertEquals(ar.hashCode(), s2.hashCode());
    }

    @Test
    public void testInsert() {
        MutableString s = new MutableString("aba");

        s.insert(1, 'c');
        assertEquals("acba", s.toString());
        assertEquals("acba".hashCode(), s.hashCode());

        s.insert(2, "abc");
        assertEquals("acabcba", s.toString());
        assertEquals("acabcba".hashCode(), s.hashCode());

        s.insert(2, new char[]{'f', 'g'});
        assertEquals("acfgabcba", s.toString());
        assertEquals("acfgabcba".hashCode(), s.hashCode());

        s.insert(5, new char[]{'h', 'i', 'j', 'k'}, 1, 2);
        assertEquals("acfgaijbcba", s.toString());
        assertEquals("acfgaijbcba".hashCode(), s.hashCode());

        s.insert(2, 5L);
        assertEquals("ac5fgaijbcba", s.toString());
        assertEquals("ac5fgaijbcba".hashCode(), s.hashCode());
    }

    @Test
    public void testUpperAndLowerCaseMethod() {
        MutableString s1 = new MutableString("aza23523aza");
        MutableString s2 = new MutableString("AzA23523Aza");
        MutableString s3 = new MutableString("AZA23523AZA");
        MutableString s4 = new MutableString();
        s1.copyTo(s4);
        s4.append("a");
        Assert.assertTrue(s1.toUpperCase().equals(s2.toUpperCase()));
        Assert.assertTrue(s3.equals(s2));
        s1.append("a");
        s3.append("A");
        Assert.assertFalse(s1.equals(s3));
        Assert.assertTrue(s1.toLowerCase().equals(s3.toLowerCase().toUpperCase().toLowerCase()));
        Assert.assertTrue(s1.equals(s4));
    }
}
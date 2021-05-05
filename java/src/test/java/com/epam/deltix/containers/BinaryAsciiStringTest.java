package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.UUIDPrintFormat;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.util.ArrayList;
import java.util.Random;

import static org.junit.Assert.assertEquals;

public class BinaryAsciiStringTest {

    BinaryAsciiString s = new BinaryAsciiString();
    private char[] ar = {'a', 'q', 'w'};
    private UUID uuid = new UUID();

    @Before
    public void setUp() {
        uuid.fromString("0123456789ABCDEF1011121314151617");
    }


    @Test
    public void testBigCount() {
        BinaryAsciiString s = new BinaryAsciiString();
        for (int i = 0; i < 100; ++i) s.appendFastHex(10000);
        s.clear();
    }

    @Test
    public void subStringTest() {
        BinaryAsciiString s = new BinaryAsciiString("A Z235 23A AZA");
        Assert.assertEquals(s.subString(0, 0), new BinaryAsciiString());
        Assert.assertEquals(s.subString(0, s.length()), s);
        Assert.assertEquals(s.subString(0, 1), new BinaryAsciiString("A"));
        Assert.assertEquals(s.subString(1, 5), new BinaryAsciiString(" Z23"));
        Assert.assertEquals(s.subString(2, 10), new BinaryAsciiString("Z235 23A"));

        BinaryAsciiString a = new BinaryAsciiString();
        s.subString(a, 1, 4);
        Assert.assertEquals(a, new BinaryAsciiString(" Z2"));
        Assert.assertEquals(s, new BinaryAsciiString("A Z235 23A AZA"));
    }

    @Test
    public void testToUtf8() {
        byte[] utf8test = new byte[100];
        BinaryAsciiString s = new BinaryAsciiString();
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
    public void testIsEmpty() {
        Assert.assertTrue(new BinaryAsciiString().isEmpty());
        Assert.assertTrue(new BinaryAsciiString("").isEmpty());
        Assert.assertTrue(new BinaryAsciiString("123").clear().isEmpty());

        Assert.assertFalse(new BinaryAsciiString("1").isEmpty());
        Assert.assertFalse(new BinaryAsciiString().append("asd").isEmpty());
        Assert.assertFalse(new BinaryAsciiString().assign("qwe").isEmpty());
    }


    @Test
    public void testAppendBinaryAsciiString() {
        BinaryAsciiString s = new BinaryAsciiString();
        BinaryAsciiString s1 = new BinaryAsciiString();
        BinaryAsciiString s2 = new BinaryAsciiString();
        s.assign("123");
        s1.assign(s);
        assertEquals("123", s1.toString());
        s1.append("456789");
        s2.assign(s1);
        assertEquals("123456789", s2.toString());
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
    public void testConstructor() {
        BinaryAsciiString s = new BinaryAsciiString(0).assign("abc");
        Assert.assertEquals(3, s.length());
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
        BinaryAsciiString s1 = new BinaryAsciiString("abaaba");
        BinaryAsciiString s2 = new BinaryAsciiString("abaaba");
        BinaryAsciiString s3 = new BinaryAsciiString("abaabb");
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
    public void testUpperAndLowerCaseMethod() {
        BinaryAsciiString s1 = new BinaryAsciiString("az23523aaza");
        BinaryAsciiString s2 = new BinaryAsciiString("Az23523AAza");
        BinaryAsciiString s3 = new BinaryAsciiString("AZ23523AAZA");
        BinaryAsciiString s4 = new BinaryAsciiString();
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

    @Test
    public void testAppendHexLong() {
        long iterations = 1000000;
        BinaryAsciiString asciiString = new BinaryAsciiString();
        MutableString str = new MutableString();
        Random rand = new Random(55);
        ArrayList<Character> ar1 = new ArrayList<>();
        ArrayList<Character> ar2 = new ArrayList<>();
        for (int i = 0; i < iterations; ++i) {
            long randInt = rand.nextLong();
            asciiString.clear();
            asciiString.appendFastHex(randInt);
            str.assign(Long.toHexString(randInt));
            while (str.length() < 16) str.insert(0, '0');
            ar1.clear();
            ar2.clear();
            for (int j = 0; j < str.length(); ++j) ar1.add(Character.toLowerCase(str.getCharAt(j)));
            for (int j = 0; j < asciiString.length(); ++j) ar2.add(Character.toLowerCase(asciiString.getCharAt(j)));
            Assert.assertEquals(ar1.toString(), ar2.toString());

        }
    }

}

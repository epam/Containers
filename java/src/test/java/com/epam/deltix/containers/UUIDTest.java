package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.BinaryArrayReadOnly;
import com.epam.deltix.containers.interfaces.UUIDParseFormat;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;
import org.junit.experimental.categories.Category;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;

@Category(Test.class)
public class UUIDTest {
    private String referenceString;
    private UUID referenceUuid;

    @Before
    public void setUp() {
        referenceUuid = new UUID();
    }



    @Test
    public void testTryAssignAndIsCorrect() {
        UUID uuid = new UUID();
        Assert.assertEquals(false, uuid.tryAssign("123-45678-90AB-CDEF-1011-121314151617"));
        Assert.assertEquals(false, UUID.isValid("123-45678-90AB-CDEF-1011-121314151617"));

        Assert.assertEquals(false, uuid.tryAssign("12345678-90ab-cdef-1011-12131415161"));
        Assert.assertEquals(false, UUID.isValid("12345678-90ab-cdef-1011-12131415161"));

        Assert.assertEquals(true, uuid.tryAssign("12345678-90ab-cdef-1011-121314151617"));
        Assert.assertEquals(true, UUID.isValid("12345678-90ab-cdef-1011-121314151617"));

        Assert.assertEquals(true, uuid.tryAssign("12345678-90AB-CDEF-1011-121314151617"));
        Assert.assertEquals(true, UUID.isValid("12345678-90AB-CDEF-1011-121314151617"));

        Assert.assertEquals(true, uuid.tryAssign("12345678-90Ab-CdEf-1011-121314151617"));
        Assert.assertEquals(true, UUID.isValid("12345678-90Ab-CdEf-1011-121314151617"));

        Assert.assertEquals(true, uuid.tryAssign("1234567890abcdef1011121314151617"));
        Assert.assertEquals(true, UUID.isValid("1234567890abcdef1011121314151617"));

        Assert.assertEquals(true, uuid.tryAssign("1234567890ABCDEF1011121314151617"));
        Assert.assertEquals(true, UUID.isValid("1234567890ABCDEF1011121314151617"));

        Assert.assertEquals(true, uuid.tryAssign("1234567890AbCdEf1011121314151617"));
        Assert.assertEquals(true, UUID.isValid("1234567890AbCdEf1011121314151617"));

        Assert.assertEquals(false, uuid.tryAssign("00000000-0000-0000-0000-1583521870898"));
        Assert.assertEquals(false, UUID.isValid("00000000-0000-0000-0000-1583521870898"));

        Assert.assertEquals(true, uuid.tryAssign("W00000000-0000-0000-0000-1583521870898", 1));
        Assert.assertEquals(true, UUID.isValid("W00000000-0000-0000-0000-1583521870898", 1));
    }

    @Test
    public void testToBytesIsCorrect() {
        testToBytesIsCorrect(0x3E361A1D3DCA46CEL, 0x8E6FDA80805E82CBL, new byte[]{(byte) 0x3E, (byte) 0x36, (byte) 0x1A, (byte) 0x1D, (byte) 0x3D, (byte) 0xCA, (byte) 0x46, (byte) 0xCE, (byte) 0x8E, (byte) 0x6F, (byte) 0xDA, (byte) 0x80, (byte) 0x80, (byte) 0x5E, (byte) 0x82, (byte) 0xCB});
        testToBytesIsCorrect(0x7B6F95FF07634F7EL, 0x9697B88A137DBC9BL, new byte[]{(byte) 0x7B, (byte) 0x6F, (byte) 0x95, (byte) 0xFF, (byte) 0x07, (byte) 0x63, (byte) 0x4F, (byte) 0x7E, (byte) 0x96, (byte) 0x97, (byte) 0xB8, (byte) 0x8A, (byte) 0x13, (byte) 0x7D, (byte) 0xBC, (byte) 0x9B});
        testToBytesIsCorrect(0x79995BD3EF9040B0L, 0xAA67D9006E325E3DL, new byte[]{(byte) 0x79, (byte) 0x99, (byte) 0x5B, (byte) 0xD3, (byte) 0xEF, (byte) 0x90, (byte) 0x40, (byte) 0xB0, (byte) 0xAA, (byte) 0x67, (byte) 0xD9, (byte) 0x00, (byte) 0x6E, (byte) 0x32, (byte) 0x5E, (byte) 0x3D});
        testToBytesIsCorrect(0x4B25B8052A614516L, 0xA0A0F3D94C5996DCL, new byte[]{(byte) 0x4B, (byte) 0x25, (byte) 0xB8, (byte) 0x05, (byte) 0x2A, (byte) 0x61, (byte) 0x45, (byte) 0x16, (byte) 0xA0, (byte) 0xA0, (byte) 0xF3, (byte) 0xD9, (byte) 0x4C, (byte) 0x59, (byte) 0x96, (byte) 0xDC});
        testToBytesIsCorrect(0x140529FCFBBF49CAL, 0x89FD697837BB0A98L, new byte[]{(byte) 0x14, (byte) 0x05, (byte) 0x29, (byte) 0xFC, (byte) 0xFB, (byte) 0xBF, (byte) 0x49, (byte) 0xCA, (byte) 0x89, (byte) 0xFD, (byte) 0x69, (byte) 0x78, (byte) 0x37, (byte) 0xBB, (byte) 0x0A, (byte) 0x98});
        testToBytesIsCorrect(0x6C584811218D4D73L, 0x9035BC9FDD25856DL, new byte[]{(byte) 0x6C, (byte) 0x58, (byte) 0x48, (byte) 0x11, (byte) 0x21, (byte) 0x8D, (byte) 0x4D, (byte) 0x73, (byte) 0x90, (byte) 0x35, (byte) 0xBC, (byte) 0x9F, (byte) 0xDD, (byte) 0x25, (byte) 0x85, (byte) 0x6D});
        testToBytesIsCorrect(0x928970F45D724802L, 0x915C40A48E527440L, new byte[]{(byte) 0x92, (byte) 0x89, (byte) 0x70, (byte) 0xF4, (byte) 0x5D, (byte) 0x72, (byte) 0x48, (byte) 0x02, (byte) 0x91, (byte) 0x5C, (byte) 0x40, (byte) 0xA4, (byte) 0x8E, (byte) 0x52, (byte) 0x74, (byte) 0x40});
        testToBytesIsCorrect(0x04E8A53C22F64A80L, 0xA846E1C50022E221L, new byte[]{(byte) 0x04, (byte) 0xE8, (byte) 0xA5, (byte) 0x3C, (byte) 0x22, (byte) 0xF6, (byte) 0x4A, (byte) 0x80, (byte) 0xA8, (byte) 0x46, (byte) 0xE1, (byte) 0xC5, (byte) 0x00, (byte) 0x22, (byte) 0xE2, (byte) 0x21});
        testToBytesIsCorrect(0x5728ED8BB22A4352L, 0xB516F18E30ECD645L, new byte[]{(byte) 0x57, (byte) 0x28, (byte) 0xED, (byte) 0x8B, (byte) 0xB2, (byte) 0x2A, (byte) 0x43, (byte) 0x52, (byte) 0xB5, (byte) 0x16, (byte) 0xF1, (byte) 0x8E, (byte) 0x30, (byte) 0xEC, (byte) 0xD6, (byte) 0x45});
        testToBytesIsCorrect(0x3CBA5EB22FCD4DE5L, 0x8A1BF496E7D5EEB6L, new byte[]{(byte) 0x3C, (byte) 0xBA, (byte) 0x5E, (byte) 0xB2, (byte) 0x2F, (byte) 0xCD, (byte) 0x4D, (byte) 0xE5, (byte) 0x8A, (byte) 0x1B, (byte) 0xF4, (byte) 0x96, (byte) 0xE7, (byte) 0xD5, (byte) 0xEE, (byte) 0xB6});
    }

    private void testToBytesIsCorrect(long msb, long lsb, byte[] bytes) {
        final UUID uuid = new UUID(msb, lsb);
        assertEquals(msb, uuid.getMSB());
        assertEquals(lsb, uuid.getLSB());
        assertArrayEquals(bytes, uuid.toBytes());
        assertEquals(msb, uuid.getMSB());
        assertEquals(lsb, uuid.getLSB());
    }

    @Test
    public void testFromString() {
        referenceString = "12345678-90AB-CDEF-1011-121314151617";

        testFromString("123-45678-90AB-CDEF-1011-121314151617", false);
        testFromString("12345678-90ab-cdef-1011-12131415161", false);

        testFromString("12345678-90ab-cdef-1011-121314151617", true);
        testFromString("12345678-90AB-CDEF-1011-121314151617", true);
        testFromString("12345678-90Ab-CdEf-1011-121314151617", true);
        testFromString("1234567890abcdef1011121314151617", true);
        testFromString("1234567890ABCDEF1011121314151617", true);
        testFromString("1234567890AbCdEf1011121314151617", true);
        testFromString("000000000000000000001583521870898", false);
    }

    private void testFromString(String str, boolean isCorrect) {
        final boolean hasDashes = str.contains("-");
        final boolean isLowerCase = str.toLowerCase().equals(str);
        final boolean isUpperCase = str.toUpperCase().equals(str);

        testFromStringFormat(str, UUIDParseFormat.LOWERCASE, isCorrect && hasDashes && isLowerCase);
        testFromStringFormat(str, UUIDParseFormat.UPPERCASE, isCorrect && hasDashes && isUpperCase);
        testFromStringFormat(str, UUIDParseFormat.ANYCASE, isCorrect && hasDashes);

        testFromStringFormat(str, UUIDParseFormat.LOWERCASE_WITHOUT_DASHES, isCorrect && !hasDashes && isLowerCase);
        testFromStringFormat(str, UUIDParseFormat.UPPERCASE_WITHOUT_DASHES, isCorrect && !hasDashes && isUpperCase);
        testFromStringFormat(str, UUIDParseFormat.ANYCASE_WITHOUT_DASHES, isCorrect && !hasDashes);

        testFromStringFormat(str, UUIDParseFormat.ANY, isCorrect);
    }

    private void testFromStringFormat(String str, UUIDParseFormat format, boolean isCorrect) {
        UUID uuid = new UUID();

        if (isCorrect) {
            uuid.fromString(str, format);
            Assert.assertEquals(referenceString, uuid.toString());
        } else {
            try {
                uuid.fromString(str, format);
                Assert.fail("UUID: " + str + ". Format: " + format);
            } catch (Exception ignore) {
            }
        }
    }

    @Test
    public void testFromBinaryArray() {
        referenceUuid.fromString("1234567890ABCDEF1011121314151617");

        final byte[] src = new byte[]{0x00, 0x12, 0x34, 0x56, 0x78, (byte) 0x90, (byte) 0xAB, (byte) 0xCD, (byte) 0xEF, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17};

        testFromBinaryArray(new BinaryArray().assign(src, 1, 15), 0, false);
        testFromBinaryArray(new BinaryArray().assign(src), 2, false);

        testFromBinaryArray(new BinaryArray().assign(src, 1, 16), 0, true);
        testFromBinaryArray(new BinaryArray().assign(src), 1, true);
    }

    private void testFromBinaryArray(BinaryArrayReadOnly ba, int offset, boolean isCorrect) {
        if (isCorrect) {
            UUID uuid = new UUID(ba, offset);
            Assert.assertEquals(referenceUuid, uuid);
        } else {
            try {
                new UUID(ba, offset);
                Assert.fail("BinaryArray: " + ba + ". Offset: " + offset);
            } catch (Exception ignored) {
            }
        }
    }

    @Test
    @SuppressWarnings("assertEquals")
    public void testEquals() {
        Assert.assertTrue(new UUID(1, 2).equals(new UUID(1, 2)));
        Assert.assertFalse(new UUID(1, 2).equals(new UUID(1, 3)));
        Assert.assertFalse(new UUID(1, 2).equals(new UUID(2, 2)));

        Assert.assertTrue(new UUID(1, 2).equals(new java.util.UUID(1, 2)));
        Assert.assertFalse(new UUID(1, 2).equals(new java.util.UUID(1, 3)));
        Assert.assertFalse(new UUID(1, 2).equals(new java.util.UUID(2, 2)));

        Assert.assertEquals(new UUID(1, 2), new UUID(1, 2));
        Assert.assertNotEquals(new UUID(1, 2), new UUID(1, 3));
        Assert.assertNotEquals(new UUID(1, 2), new UUID(2, 2));

        Assert.assertEquals(new UUID(1, 2), new java.util.UUID(1, 2));
        Assert.assertNotEquals(new UUID(1, 2), new java.util.UUID(1, 3));
        Assert.assertNotEquals(new UUID(1, 2), new java.util.UUID(2, 2));
    }
}

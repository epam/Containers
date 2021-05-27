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

import org.junit.Assert;
import org.junit.Test;

public class AlphanumericUtilsTest {
    public class TestPair
    {
        public long intValue;
        public String stringValue;

        public  TestPair(long i, String s)
        {
            intValue = i;
            stringValue = s;
        }
    }

    // For these pairs Int->String == String, but String->Int != Int
    private final TestPair[] correctI2S = new TestPair[]
    {
        new TestPair(0x123456789ABCDEFL	, ""),
                new TestPair(0x1123456789ABCDEFL, "$"),
                new TestPair(0xB000000000000000L, null),
                new TestPair(0xB123456789ABCDEFL, null),
                new TestPair(0x80000000000000F1L, "        "),

                new TestPair(0x1FFFFFFFFFFFFFFFL, "_"),
                new TestPair(0x2FFFFFFFFFFFFFFFL, "__"),
                new TestPair(0x3FFFFFFFFFFFFFFFL, "___"),
                new TestPair(0x4FFFFFFFFFFFFFFFL, "____"),
                new TestPair(0x5FFFFFFFFFFFFFFFL, "_____"),
                new TestPair(0x6FFFFFFFFFFFFFFFL, "______"),
                new TestPair(0x7FFFFFFFFFFFFFFFL, "_______"),
                new TestPair(0x8FFFFFFFFFFFFFFFL, "________"),
                new TestPair(0x9FFFFFFFFFFFFFFFL, "_________"),

                new TestPair(Long.MAX_VALUE	, "_______"),
    };

    // For these pairs String->Int == Int, but Int->String != String
    private final TestPair[] correctS2I =
    {
        new TestPair(0x8000000000000000L, "        "),
                new TestPair(Long.MIN_VALUE	, "        "),
    };

    // For these pairs String->Int == Int and Int->String == String
    private final TestPair[] correct =
    {
        new TestPair(Long.MIN_VALUE	, null),
                new TestPair(0x8000000000000000L, null),
                new TestPair(0, ""),
                new TestPair(0x1100000000000000L, "$"),

                new TestPair(0x1000000000000000L, " "),
                new TestPair(0x2000000000000000L, "  "),
                new TestPair(0x3000000000000000L, "   "),
                new TestPair(0x4000000000000000L, "    "),
                new TestPair(0x5000000000000000L, "     "),
                new TestPair(0x6000000000000000L, "      "),
                new TestPair(0x7000000000000000L, "       "),
                // no 0x8000000000000000
                new TestPair(0x9000000000000000L, "         "),
                new TestPair(0xA000000000000000L, "          "),

                new TestPair(0x1FC0000000000000L, "_"),
                new TestPair(0x2FFF000000000000L, "__"),
                new TestPair(0x3FFFFC0000000000L, "___"),
                new TestPair(0x4FFFFFF000000000L, "____"),
                new TestPair(0x5FFFFFFFC0000000L, "_____"),
                new TestPair(0x6FFFFFFFFF000000L, "______"),
                new TestPair(0x7FFFFFFFFFFC0000L, "_______"),
                new TestPair(0x8FFFFFFFFFFFF000L, "________"),
                new TestPair(0x9FFFFFFFFFFFFFC0L, "_________"),
                new TestPair(0xAFFFFFFFFFFFFFFFL, "__________"),

                new TestPair(0x1440000000000000L, "1"),
                new TestPair(0x2493000000000000L, "23"),
                new TestPair(0x3515580000000000L, "456"),
                new TestPair(0x45D8661000000000L, "789A"),
                new TestPair(0x58A3925980000000L, "BCDEF"),
                new TestPair(0x6A29AABB2D000000L, "HIJKLM"),
                new TestPair(0x7BAFC31CB3D00000L, "NOPQRST"),
                new TestPair(0x8D76DF8E7AEFC000L, "UVWXYZ[\\"),
                new TestPair(0x9F7EFC00420C4140L, "]^_ !\"#$%"),
                new TestPair(0xA18720928B30D38FL, "&'()*+,-./"),

                new TestPair(0x5FA0FE0F80000000L, "^@_@^"),
                new TestPair(0x669B71D79F000000L, ":;<=>?"),
    };

    private final long[] incorrectInt =
    {
                0xFFFFFFFFFFFFFFFFL,
                0xF000000000000000L,
                0xE000000000000000L,
                0xD000000000000000L,
                0xC000000000000000L,
                0xF123456789ABCDEFL,
                0xE123456789ABCDEFL,
                0xD123456789ABCDEFL,
                0xC123456789ABCDEFL,
    };

    private static final String[] incorrectString =
    {
                "\0123",
                "\0178",
                "\n",
                "\r",
                "`",
                "all your base are belong to us",
                "04259837l2"
    };

    @Test
    public void int64ToString()
    {
        for (int i = 0; i < correctI2S.length; ++i) {
            Assert.assertEquals(correctI2S[i].stringValue, AlphanumericUtils.toString(correctI2S[i].intValue));
        }

        for (int i = 0; i < correct.length; ++i) {
            Assert.assertEquals(correct[i].stringValue, AlphanumericUtils.toString(correct[i].intValue));
        }
    }

    @Test
    public void int64AssignToMString()
    {
        for (TestPair p: correct) {
            MutableString ms1 = new MutableString(), ms2 = new MutableString("ewrfvewrtfcr\tmewfxlqwoexhfsm3egzfsqewir\r \nqfaGFEWFW");

            AlphanumericUtils.assignAlphanumeric(ms1, p.intValue);
            AlphanumericUtils.assignAlphanumeric(ms2, p.intValue);

            if (p.stringValue == null)
            {
                Assert.assertEquals("", ms1.toString());
                Assert.assertEquals("", ms2.toString());
            }
            else
            {
                Assert.assertEquals(p.stringValue, ms1.toString());
                Assert.assertEquals(p.stringValue, ms2.toString());
            }

            Assert.assertEquals(true, ms1.equals(ms2));
        }

        for (TestPair p: correctI2S) {
            MutableString ms1 = new MutableString(), ms2 = new MutableString("ewrfvewrtfcr\tmewfxlqwoexhfsm3egzfsqewir\r \nqfaGFEWFW");

            AlphanumericUtils.assignAlphanumeric(ms1, p.intValue);
            AlphanumericUtils.assignAlphanumeric(ms2, p.intValue);

            if (p.stringValue == null)
            {
                Assert.assertEquals("", ms1.toString());
                Assert.assertEquals("", ms2.toString());
            }
            else
            {
                Assert.assertEquals(p.stringValue, ms1.toString());
                Assert.assertEquals(p.stringValue, ms2.toString());
            }
            Assert.assertEquals(true, ms1.equals(ms2));
        }

    }

    @Test
    public void int64AppendToMString()
    {
        for (TestPair p: correct) {
            MutableString ms1 = new MutableString(), ms2 = new MutableString("ewrfvewrtfcr\tmewfxlqwoexhfsm3egzfsqewir\r \nqfaGFEWFW");
            MutableString ms3 = ms2.clone();

            AlphanumericUtils.appendAlphanumeric(ms1, p.intValue);
            AlphanumericUtils.appendAlphanumeric(ms2, p.intValue);

            if (p.stringValue == null)
            {
                Assert.assertEquals("", ms1.toString());
                Assert.assertEquals("ewrfvewrtfcr\tmewfxlqwoexhfsm3egzfsqewir\r \nqfaGFEWFW", ms2.toString());
            }
            else
            {
                Assert.assertEquals(p.stringValue, ms1.toString());
                Assert.assertEquals(ms3.toString().concat(p.stringValue), ms2.toString());
            }
        }

        for (TestPair p: correctI2S) {
            MutableString ms1 = new MutableString(), ms2 = new MutableString("ewrfvewrtfcr\tmewfxlqwoexhfsm3egzfsqewir\r \nqfaGFEWFW");
            MutableString ms3 = ms2.clone();

            AlphanumericUtils.appendAlphanumeric(ms1, p.intValue);
            AlphanumericUtils.appendAlphanumeric(ms2, p.intValue);

            if (p.stringValue == null)
            {
                Assert.assertEquals("", ms1.toString());
                Assert.assertEquals("ewrfvewrtfcr\tmewfxlqwoexhfsm3egzfsqewir\r \nqfaGFEWFW", ms2.toString());
            }
            else
            {
                Assert.assertEquals(p.stringValue, ms1.toString());
                Assert.assertEquals(ms3.toString().concat(p.stringValue), ms2.toString());
            }
        }
    }

    @Test
    public void int64ToStringAndBack()
    {
        for (TestPair p: correct) {
            Assert.assertEquals(p.intValue, AlphanumericUtils.toAlphanumericUInt64(AlphanumericUtils.toString(p.intValue)));
        }
    }

    @Test
    public void int64ToMutableStringAndBack()
    {
        for (TestPair p: correct) {
            Assert.assertEquals(p.intValue, AlphanumericUtils.toAlphanumericUInt64(AlphanumericUtils.toMutableString(p.intValue)));
        }
    }

	@Test
    public void stringToInt64AndBack() {
        for (TestPair p : correct) {
            Assert.assertEquals(p.stringValue, AlphanumericUtils.toString(AlphanumericUtils.toAlphanumericUInt64(p.stringValue)));
        }
    }

    @Test
    public void mutableStringToInt64AndBack()
    {
        for (TestPair p : correct) {
            MutableString ms = null != p.stringValue ? new MutableString(p.stringValue) : null;
            if (ms == null && AlphanumericUtils.toMutableString(AlphanumericUtils.toAlphanumericUInt64(ms)) == null) continue;
            Assert.assertEquals(ms.toString(), AlphanumericUtils.toMutableString(AlphanumericUtils.toAlphanumericUInt64(ms)).toString());
        }
    }

    @Test
    public void isValidInt64()
    {
        for (TestPair p : correct) {
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(p.intValue));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(new MutableString().assign(p.intValue)));
        }
        for (TestPair p : correctI2S) {
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(p.intValue));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(new MutableString().assign(p.intValue)));
        }
    }

    @Test
    public void isValidStringToInt64() {
        for (TestPair p : correct) {
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumericUInt64(p.stringValue));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumericUInt64(new MutableString().assign(p.stringValue)));
        }
        for (TestPair p : correctI2S) {
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumericUInt64(p.stringValue));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumericUInt64(new MutableString().assign(p.stringValue)));
        }
    }

    @Test
	public void isValidString()
    {
        for (TestPair p : correct) {
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(p.stringValue));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric((new MutableString().assign(p.stringValue))));
        }
        for (TestPair p : correctS2I) {
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(p.stringValue));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric((new MutableString().assign(p.stringValue))));
        }

    }

	@Test
    public void isValidMutableString()
    {
        for (TestPair p : correct) {
            MutableString ms = null != p.stringValue ? new MutableString(p.stringValue) : null;
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(ms));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric((new MutableString().assign(ms))));
        }
        for (TestPair p : correctS2I) {
            MutableString ms = null != p.stringValue ? new MutableString(p.stringValue) : null;
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric(ms));
            Assert.assertTrue(AlphanumericUtils.isValidAlphanumeric((new MutableString().assign(ms))));
        }


    }

	@Test
    public void isInvalidInt64()
    {
        for (long x: incorrectInt) {
            Assert.assertFalse(AlphanumericUtils.isValidAlphanumeric(x));
        }
    }

	@Test
    public void isInvalidString()
    {
        for (String s: incorrectString) {
            Assert.assertFalse(AlphanumericUtils.isValidAlphanumeric(s));
            Assert.assertFalse(AlphanumericUtils.isValidAlphanumeric(new MutableString(s)));
        }
    }

}
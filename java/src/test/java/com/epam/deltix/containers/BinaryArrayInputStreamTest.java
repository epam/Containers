package com.epam.deltix.containers;

import org.junit.Assert;
import org.junit.Test;

import java.io.IOException;

public class BinaryArrayInputStreamTest {
    @Test
    public void testStream() throws IOException {
        byte[] src = new byte[]{1, 2, 3, 127, (byte) 128, (byte) 129, (byte) 255};
        byte[] dst = new byte[src.length];

        BinaryArray ba = new BinaryArray(src);
        BinaryArrayInputStream stream = new BinaryArrayInputStream(ba);

        Assert.assertEquals(src.length, stream.read(dst));
        Assert.assertArrayEquals(src, dst);
    }

    @Test
    public void testUpdateDataSource() throws IOException {
        byte[] src1 = new byte[]{1, 2, 3};
        byte[] dst1 = new byte[src1.length];

        byte[] src2 = new byte[]{7, 8, 9, 10};
        byte[] dst2 = new byte[src2.length];

        // Create

        BinaryArray ba1 = new BinaryArray(src1);
        BinaryArrayInputStream stream = new BinaryArrayInputStream(ba1);

        Assert.assertEquals(src1.length, stream.read(dst1));
        Assert.assertArrayEquals(src1, dst1);

        // Update

        BinaryArray ba2 = new BinaryArray(src2);
        stream.updateDataSource(ba2);

        Assert.assertEquals(src2.length, stream.read(dst2));
        Assert.assertArrayEquals(src2, dst2);
    }
}

package com.epam.deltix.containers;

import com.epam.deltix.containers.generated.IntRingedQueue;
import org.junit.Assert;
import org.junit.Test;

import java.util.Random;

public class RingedQueueTest {

    @Test
    public void objStressTest() {
        ObjRingedQueue<BinaryArray> queue = new ObjRingedQueue<>();
        BufferedLinkedList<BinaryArray> etalon = new BufferedLinkedList<>();
        Assert.assertTrue(queue.isEmpty());

        Random rand = new Random(55);
        int iterations = 1000000;

        for (int i = 0; i < iterations; ++i) {
            int type = Math.abs(rand.nextInt()) % 3;
            if (Math.abs(type) == 0 && queue.size() > 0) {
                Assert.assertEquals(true, queue.pop().equals(etalon.getFirst()));
                etalon.removeFirst();
            } else {
                BinaryArray array = new BinaryArray();
                array.append(i);
                queue.add(array);
                etalon.addLast(array);
            }

            if (i % 100000 == 0) {
                int index = 0;
                int iterator = etalon.getFirstKey();
                Assert.assertEquals(queue.size(), etalon.getCount());
                for (BinaryArray element : queue) {
                    Assert.assertEquals(true, element.equals(etalon.getElementByKey(iterator)));
                    Assert.assertEquals(true, element == queue.get(index));
                    index++;
                    iterator = etalon.next(iterator);
                }
            }
        }
        Assert.assertFalse(queue.isEmpty());
    }



    @Test
    public void primitiveStressTest() {
        IntRingedQueue queue = new IntRingedQueue();
        BufferedLinkedList<Integer> etalon = new BufferedLinkedList<>();

        Random rand = new Random(55);
        int iterations = 1000000;

        for (int i = 0; i < iterations; ++i) {
            int type = Math.abs(rand.nextInt()) % 3;
            if (Math.abs(type) == 0 && queue.size() > 0) {
                Assert.assertEquals(queue.pop(),  etalon.getFirst().intValue());
                etalon.removeFirst();
            } else {
                queue.add(i);
                etalon.addLast(i);
            }

            if (i % 100000 == 0) {
                int index = 0;
                int iterator = etalon.getFirstKey();
                Assert.assertEquals(queue.size(), etalon.getCount());
                for (Integer element : queue) {
                    Assert.assertEquals(true, element.equals(etalon.getElementByKey(iterator)));
                    Assert.assertEquals(true, element == queue.get(index));
                    index++;
                    iterator = etalon.next(iterator);
                }
            }
        }

    }
}

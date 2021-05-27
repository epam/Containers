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

import com.epam.deltix.containers.interfaces.BiConsumer;
import com.epam.deltix.containers.generated.DecimalDataQueue;
import com.epam.deltix.dfp.Decimal64;
import org.junit.Assert;
import org.junit.Test;

import javax.naming.OperationNotSupportedException;
import java.util.Iterator;

public class DecimalDataQueueTest {

    @Test
    public void manualQueueTest() throws OperationNotSupportedException {
        DecimalDataQueue queue = new DecimalDataQueue(true, false);
        Assert.assertTrue(queue.isEmpty());
        queue.putLast(Decimal64.ONE);
        Assert.assertFalse(queue.isEmpty());
        Assert.assertEquals(1, queue.size());
        queue.putLast(Decimal64.TWO);
        Assert.assertEquals(Decimal64.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(Decimal64.TWO, queue.getPointsAgo(0));
        for(int i = 0; i < 100; i++) {
            queue.putLast(Decimal64.fromDouble(i));
        }
        Assert.assertEquals(102, queue.size());
        Assert.assertEquals(Decimal64.fromDouble(4), queue.getPointsAgo(95));
        Assert.assertEquals(Decimal64.ONE, queue.getFirst());
        queue.removeFirst();
        Assert.assertEquals(Decimal64.fromDouble(2), queue.getPointsAgo(100));
        Assert.assertEquals(101, queue.size());
        queue.putFirst(Decimal64.ONE);
        Assert.assertEquals(Decimal64.fromDouble(1), queue.getPointsAgo(101));

        Assert.assertEquals(Decimal64.fromDouble(4953), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(328355), queue.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(4953), queue.sum());
        Assert.assertEquals(48.559, queue.expectedValue().toDouble(), 0.001);

        DecimalDataQueue queueClone = queue.clone();
        queueClone.removeLast();
        Assert.assertEquals(Decimal64.fromDouble(98), queueClone.getLast());
        Assert.assertEquals(Decimal64.fromDouble(99), queue.getLast());

        Assert.assertEquals(Decimal64.fromDouble(4854), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(318554), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(4854), queueClone.sum());

        queue.setPointsAgo(1, Decimal64.ZERO);
        Assert.assertEquals(Decimal64.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64.fromDouble(4855), queue.sum());

        Iterator<Decimal64> it = queue.iterator();
        Assert.assertEquals(Decimal64.fromDouble(1), it.next());
    }

    @Test
    public void manualQueueWithDatesTest() throws OperationNotSupportedException {
        DecimalDataQueue queue = new DecimalDataQueue(true, true);
        queue.putLast(Decimal64.ONE, 1);
        Assert.assertEquals(1, queue.size());
        queue.putLast(Decimal64.TWO, 2);
        Assert.assertEquals(Decimal64.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(Decimal64.TWO, queue.getPointsAgo(0));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(0));
        for(int i = 3; i < 100; i++) {
            queue.putLast(Decimal64.fromDouble(i), i);
        }
        Assert.assertEquals(99, queue.size());
        Assert.assertEquals(Decimal64.fromDouble(4), queue.getPointsAgo(95));
        Assert.assertEquals(4, queue.getTimeByIndexAgo(95));
        Assert.assertEquals(Decimal64.ONE, queue.getFirst());
        Assert.assertEquals(1, queue.getFirstElementTime());
        queue.removeFirst();
        Assert.assertEquals(Decimal64.fromDouble(2), queue.getPointsAgo(97));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(97));
        Assert.assertEquals(98, queue.size());
        queue.putFirst(Decimal64.ONE, 1);
        Assert.assertEquals(Decimal64.fromDouble(1), queue.getPointsAgo(98));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(98));

        Assert.assertEquals(Decimal64.fromDouble(4950), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(328350), queue.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(4950), queue.sum());
        Assert.assertEquals(50, queue.expectedValue().toDouble(), 0.001);

        DecimalDataQueue queueClone = queue.clone();
        queueClone.removeLast();
        Assert.assertEquals(Decimal64.fromDouble(98), queueClone.getLast());
        Assert.assertEquals(98, queueClone.getLastElementTime());
        Assert.assertEquals(Decimal64.fromDouble(99), queue.getLast());
        Assert.assertEquals(99, queue.getLastElementTime());

        Assert.assertEquals(Decimal64.fromDouble(4851), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(318549), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(4851), queueClone.sum());

        queue.setPointsAgo(1, Decimal64.ZERO);
        Assert.assertEquals(Decimal64.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(98, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64.fromDouble(4852), queue.sum());

        Assert.assertEquals(74, queue.getIndexByTime(25));
        Assert.assertEquals(89, queue.getIndexByTime(10));
        Assert.assertEquals(Decimal64.fromDouble(25), queue.getByTime(25));
        queue.setByTime(25, Decimal64.fromDouble(100));
        Assert.assertEquals(Decimal64.fromDouble(4927), queue.sum());
    }

    @Test
    public void autoDynamicQueueTest() throws OperationNotSupportedException {
        int period = 20;
        DecimalDataQueue queue = new DecimalDataQueue(20, period, true);
        queue.put(Decimal64.ONE, 1);
        Assert.assertEquals(1, queue.size());
        queue.put(Decimal64.TWO, 2);
        Assert.assertEquals(Decimal64.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(Decimal64.TWO, queue.getPointsAgo(0));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(0));
        for(int i = 3; i < 100; i++) {
            queue.put(Decimal64.fromDouble(i), i);
        }
        Assert.assertEquals(20, queue.getLastElementTime() - queue.getFirstElementTime());
        Assert.assertEquals(21, queue.size());
        Assert.assertEquals(Decimal64.fromDouble(79), queue.getPointsAgo(20));
        Assert.assertEquals(79, queue.getTimeByIndexAgo(20));
        Assert.assertEquals(Decimal64.fromDouble(79), queue.getFirst());
        Assert.assertEquals(79, queue.getFirstElementTime());
        Assert.assertEquals(Decimal64.fromDouble(98), queue.getPrevious());


        Assert.assertEquals(Decimal64.fromDouble(1869), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(167111), queue.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(1869), queue.sum());
        Assert.assertEquals(89, queue.expectedValue().toDouble(), 0.001);

        DecimalDataQueue queueClone = queue.clone();
        queueClone.put(Decimal64.fromDouble(100), 100);
        Assert.assertEquals(Decimal64.fromDouble(100), queueClone.getPointsAgo(0));
        Assert.assertEquals(100, queueClone.getTimeByIndexAgo(0));
        Assert.assertEquals(Decimal64.fromDouble(99), queue.getPointsAgo(0));
        Assert.assertEquals(99, queue.getTimeByIndexAgo(0));

        Assert.assertEquals(Decimal64.fromDouble(1890), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(170870), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(1890), queueClone.sum());

        queue.setPointsAgo(1, Decimal64.ZERO);
        Assert.assertEquals(Decimal64.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(98, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64.fromDouble(1771), queue.sum());

        Assert.assertEquals(-1, queue.getIndexByTime(25));
        Assert.assertEquals(19, queue.getIndexByTime(80));
        Assert.assertEquals(DecimalDataQueue.DEFAULT_VALUE, queue.getByTime(25));
        queue.setByTime(80, Decimal64.fromDouble(100));
        Assert.assertEquals(Decimal64.fromDouble(1791), queue.sum());

        Assert.assertEquals(period, queue.getPeriod());
        Assert.assertEquals(99, queue.getLastElementTime());
        Assert.assertEquals(Decimal64.fromDouble(79), queue.toArray()[0]);
        Assert.assertEquals(Decimal64.fromDouble(79), queue.getFirst());
    }

    @Test
    public void autoStaticQueueTest() throws OperationNotSupportedException {
        DecimalDataQueue queue = new DecimalDataQueue(20, true, true);
        queue.put(Decimal64.ONE, 1);
        Assert.assertEquals(1, queue.size());
        queue.put(Decimal64.TWO, 2);
        Assert.assertEquals(Decimal64.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(Decimal64.TWO, queue.getPointsAgo(0));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(0));
        for(int i = 3; i < 100; i++) {
            if (i % 2 == 0)
                queue.put(Decimal64.fromDouble(i), i);
            else queue.put(Decimal64.fromDouble(-i), i);
        }
        Assert.assertEquals(20, queue.size());
        Assert.assertEquals(Decimal64.fromDouble(80), queue.getPointsAgo(19));
        Assert.assertEquals(80, queue.getTimeByIndexAgo(19));
        Assert.assertEquals(Decimal64.fromDouble(80), queue.getFirst());
        Assert.assertEquals(80, queue.getFirstElementTime());
        Assert.assertEquals(Decimal64.fromDouble(98), queue.getPrevious());


        Assert.assertEquals(Decimal64.fromDouble(1790), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(160870), queue.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(-10), queue.sum());
        Assert.assertEquals(-0.5, queue.arithmeticMean().toDouble(), 0.01);
        Assert.assertEquals(8043.25, queue.variance().toDouble(), 0.01);
        Assert.assertEquals(-179.36, queue.coefficientOfVariation().toDouble(), 0.01);
        Assert.assertEquals(-0.5, queue.expectedValue().toDouble(), 0.001);

        DecimalDataQueue queueClone = queue.clone();
        queueClone.put(Decimal64.fromDouble(100), 100);
        Assert.assertEquals(Decimal64.fromDouble(100), queueClone.getPointsAgo(0));
        Assert.assertEquals(100, queueClone.getTimeByIndexAgo(0));
        Assert.assertEquals(Decimal64.fromDouble(-99), queue.getPointsAgo(0));
        Assert.assertEquals(99, queue.getTimeByIndexAgo(0));

        Assert.assertEquals(Decimal64.fromDouble(1810), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64.fromDouble(164470), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64.fromDouble(10), queueClone.sum());
        Assert.assertEquals(20, queueClone.size());

        queue.setPointsAgo(1, Decimal64.ZERO);
        Assert.assertEquals(Decimal64.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(98, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64.fromDouble(-108), queue.sum());
        Assert.assertEquals(20, queueClone.size());

        Assert.assertEquals(-1, queue.getIndexByTime(25));
        Assert.assertEquals(19, queue.getIndexByTime(80));
        Assert.assertEquals(DecimalDataQueue.DEFAULT_VALUE, queue.getByTime(25));
        queue.setByTime(80, Decimal64.fromDouble(100));
        Assert.assertEquals(Decimal64.fromDouble(-88), queue.sum());

        Assert.assertEquals(99, queue.getLastElementTime());
        Assert.assertEquals(Decimal64.fromDouble(100), queue.toArray()[0]);
        Assert.assertEquals(Decimal64.fromDouble(100), queue.getFirst());
        queue.clear();
        Assert.assertEquals(0, queue.size());
    }

    @Test
    public void singleElementQueueTest() throws OperationNotSupportedException {
        DecimalDataQueue queue = new DecimalDataQueue(1, true, true);
        queue.put(Decimal64.ONE, 1);
        Assert.assertEquals(Decimal64.fromDouble(1), queue.sum());
        Assert.assertEquals(1, queue.size());
        queue.put(Decimal64.TWO, 2);
        Assert.assertEquals(Decimal64.fromDouble(2), queue.sum());
        Assert.assertEquals(Decimal64.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(Decimal64.ONE, queue.getPrevious());
        Assert.assertEquals(Decimal64.TWO, queue.getFirst());
        Assert.assertEquals(Decimal64.TWO, queue.getLast());
        Assert.assertEquals(2, queue.getFirstElementTime());
        Assert.assertEquals(2, queue.getLastElementTime());
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(1, queue.size());
        queue.setPointsAgo(0, Decimal64.TEN);
        Assert.assertEquals(Decimal64.fromDouble(10), queue.sum());

        DecimalDataQueue queueClone = queue.clone();
        Assert.assertEquals(Decimal64.fromDouble(10), queueClone.sum());
        queueClone.put(Decimal64.ONE, 55);
        Assert.assertEquals(Decimal64.TEN, queue.getFirst());
        Assert.assertEquals(Decimal64.fromDouble(1), queueClone.getFirst());
    }

    private int callbacksCallsCount = 0;
    private long lastTime;
    private Decimal64 lastDate;

    @Test
    public void queueListenersTest() throws OperationNotSupportedException {
        DecimalDataQueue queue = new DecimalDataQueue(20, true, true);
        queue.addOnPushListener(new BiConsumer<Decimal64, Long>() {
            @Override
            public void accept(Decimal64 data, Long time) {
                callbacksCallsCount++;
                lastTime = time;
            }
        });
        for (int i = 0; i < 100; i++) {
            if (i % 2 == 0)
                queue.put(Decimal64.fromDouble(i), i);
            else queue.put(Decimal64.fromDouble(-i), i);
        }
        Assert.assertEquals(100, callbacksCallsCount);
        Assert.assertEquals(99, lastTime);
        queue.addOnPopListener(new BiConsumer<Decimal64, Long>() {
            @Override
            public void accept(Decimal64 data, Long time) {
                callbacksCallsCount++;
                lastDate = data;
            }
        });
        callbacksCallsCount = 0;
        for (int i = 0; i < 10; i++)
            queue.put(Decimal64.ZERO, 100 + i);
        Assert.assertEquals(20, callbacksCallsCount);
        Assert.assertEquals(Decimal64.fromDouble(-89), lastDate);
    }
}
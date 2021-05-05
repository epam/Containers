package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.BiConsumer;
import com.epam.deltix.containers.generated.DecimalLongDataQueue;
import com.epam.deltix.dfp.Decimal64Utils;
import org.junit.Assert;
import org.junit.Test;

import javax.naming.OperationNotSupportedException;
import java.util.Iterator;

public class DecimalLongQueueTest {

    @Test
    public void manualQueueTest() throws OperationNotSupportedException {
        DecimalLongDataQueue queue = new DecimalLongDataQueue(true, false);
        queue.putLast(Decimal64Utils.ONE);
        Assert.assertEquals(1, queue.size());
        queue.putLast(Decimal64Utils.TWO);
        Assert.assertEquals(Decimal64Utils.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(Decimal64Utils.TWO, queue.getPointsAgo(0));
        for(int i = 0; i < 100; i++) {
            queue.putLast(Decimal64Utils.fromDouble(i));
        }
        Assert.assertEquals(102, queue.size());
        Assert.assertEquals(Decimal64Utils.fromDouble(4), queue.getPointsAgo(95));
        Assert.assertEquals(Decimal64Utils.ONE, queue.getFirst());
        queue.removeFirst();
        Assert.assertEquals(Decimal64Utils.fromDouble(2), queue.getPointsAgo(100));
        Assert.assertEquals(101, queue.size());
        queue.putFirst(Decimal64Utils.ONE);
        Assert.assertEquals(Decimal64Utils.fromDouble(1), queue.getPointsAgo(101));

        Assert.assertEquals(Decimal64Utils.fromDouble(4953), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(328355), queue.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(4953), queue.sum());
        Assert.assertEquals(48.559, Decimal64Utils.toDouble(queue.expectedValue()), 0.001);

        DecimalLongDataQueue queueClone = queue.clone();
        queueClone.removeLast();
        Assert.assertEquals(Decimal64Utils.fromDouble(98), queueClone.getLast());
        Assert.assertEquals(Decimal64Utils.fromDouble(99), queue.getLast());

        Assert.assertEquals(Decimal64Utils.fromDouble(4854), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(318554), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(4854), queueClone.sum());

        queue.setPointsAgo(1, Decimal64Utils.ZERO);
        Assert.assertEquals(Decimal64Utils.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64Utils.fromDouble(4855), queue.sum());

        Iterator<Long> it = queue.iterator();
        Assert.assertEquals((Long)Decimal64Utils.fromDouble(1), it.next());
    }

    @Test
    public void manualQueueWithDatesTest() throws OperationNotSupportedException {
        DecimalLongDataQueue queue = new DecimalLongDataQueue(true, true);
        queue.putLast(Decimal64Utils.ONE, 1);
        Assert.assertEquals(1, queue.size());
        queue.putLast(Decimal64Utils.TWO, 2);
        Assert.assertEquals(Decimal64Utils.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(Decimal64Utils.TWO, queue.getPointsAgo(0));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(0));
        for(int i = 3; i < 100; i++) {
            queue.putLast(Decimal64Utils.fromDouble(i), i);
        }
        Assert.assertEquals(99, queue.size());
        Assert.assertEquals(Decimal64Utils.fromDouble(4), queue.getPointsAgo(95));
        Assert.assertEquals(4, queue.getTimeByIndexAgo(95));
        Assert.assertEquals(Decimal64Utils.ONE, queue.getFirst());
        Assert.assertEquals(1, queue.getFirstElementTime());
        queue.removeFirst();
        Assert.assertEquals(Decimal64Utils.fromDouble(2), queue.getPointsAgo(97));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(97));
        Assert.assertEquals(98, queue.size());
        queue.putFirst(Decimal64Utils.ONE, 1);
        Assert.assertEquals(Decimal64Utils.fromDouble(1), queue.getPointsAgo(98));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(98));

        Assert.assertEquals(Decimal64Utils.fromDouble(4950), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(328350), queue.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(4950), queue.sum());
        Assert.assertEquals(50, Decimal64Utils.toDouble(queue.expectedValue()), 0.001);

        DecimalLongDataQueue queueClone = queue.clone();
        queueClone.removeLast();
        Assert.assertEquals(Decimal64Utils.fromDouble(98), queueClone.getLast());
        Assert.assertEquals(98, queueClone.getLastElementTime());
        Assert.assertEquals(Decimal64Utils.fromDouble(99), queue.getLast());
        Assert.assertEquals(99, queue.getLastElementTime());

        Assert.assertEquals(Decimal64Utils.fromDouble(4851), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(318549), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(4851), queueClone.sum());

        queue.setPointsAgo(1, Decimal64Utils.ZERO);
        Assert.assertEquals(Decimal64Utils.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(98, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64Utils.fromDouble(4852), queue.sum());

        Assert.assertEquals(74, queue.getIndexByTime(25));
        Assert.assertEquals(89, queue.getIndexByTime(10));
        Assert.assertEquals(Decimal64Utils.fromDouble(25), queue.getByTime(25));
        queue.setByTime(25, Decimal64Utils.fromDouble(100));
        Assert.assertEquals(Decimal64Utils.fromDouble(4927), queue.sum());
    }

    @Test
    public void autoDynamicQueueTest() throws OperationNotSupportedException {
        int period = 20;
        DecimalLongDataQueue queue = new DecimalLongDataQueue(20, period, true);
        queue.put(Decimal64Utils.ONE, 1);
        Assert.assertEquals(1, queue.size());
        queue.put(Decimal64Utils.TWO, 2);
        Assert.assertEquals(Decimal64Utils.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(Decimal64Utils.TWO, queue.getPointsAgo(0));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(0));
        for(int i = 3; i < 100; i++) {
            queue.put(Decimal64Utils.fromDouble(i), i);
        }
        Assert.assertEquals(20, queue.getLastElementTime() - queue.getFirstElementTime());
        Assert.assertEquals(21, queue.size());
        Assert.assertEquals(Decimal64Utils.fromDouble(79), queue.getPointsAgo(20));
        Assert.assertEquals(79, queue.getTimeByIndexAgo(20));
        Assert.assertEquals(Decimal64Utils.fromDouble(79), queue.getFirst());
        Assert.assertEquals(79, queue.getFirstElementTime());
        Assert.assertEquals(Decimal64Utils.fromDouble(98), queue.getPrevious());


        Assert.assertEquals(Decimal64Utils.fromDouble(1869), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(167111), queue.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(1869), queue.sum());
        Assert.assertEquals(89, Decimal64Utils.toDouble(queue.expectedValue()), 0.001);

        DecimalLongDataQueue queueClone = queue.clone();
        queueClone.put(Decimal64Utils.fromDouble(100), 100);
        Assert.assertEquals(Decimal64Utils.fromDouble(100), queueClone.getPointsAgo(0));
        Assert.assertEquals(100, queueClone.getTimeByIndexAgo(0));
        Assert.assertEquals(Decimal64Utils.fromDouble(99), queue.getPointsAgo(0));
        Assert.assertEquals(99, queue.getTimeByIndexAgo(0));

        Assert.assertEquals(Decimal64Utils.fromDouble(1890), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(170870), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(1890), queueClone.sum());

        queue.setPointsAgo(1, Decimal64Utils.ZERO);
        Assert.assertEquals(Decimal64Utils.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(98, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64Utils.fromDouble(1771), queue.sum());

        Assert.assertEquals(-1, queue.getIndexByTime(25));
        Assert.assertEquals(19, queue.getIndexByTime(80));
        Assert.assertEquals(DecimalLongDataQueue.DEFAULT_VALUE, queue.getByTime(25));
        queue.setByTime(80, Decimal64Utils.fromDouble(100));
        Assert.assertEquals(Decimal64Utils.fromDouble(1791), queue.sum());

        Assert.assertEquals(period, queue.getPeriod());
        Assert.assertEquals(99, queue.getLastElementTime());
        Assert.assertEquals(Decimal64Utils.fromDouble(79), queue.toArray()[0]);
        Assert.assertEquals(Decimal64Utils.fromDouble(79), queue.getFirst());
    }

    @Test
    public void autoStaticQueueTest() throws OperationNotSupportedException {
        DecimalLongDataQueue queue = new DecimalLongDataQueue(20, true, true);
        queue.put(Decimal64Utils.ONE, 1);
        Assert.assertEquals(1, queue.size());
        queue.put(Decimal64Utils.TWO, 2);
        Assert.assertEquals(Decimal64Utils.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(Decimal64Utils.TWO, queue.getPointsAgo(0));
        Assert.assertEquals(2, queue.getTimeByIndexAgo(0));
        for(int i = 3; i < 100; i++) {
            if (i % 2 == 0)
                queue.put(Decimal64Utils.fromDouble(i), i);
            else queue.put(Decimal64Utils.fromDouble(-i), i);
        }
        Assert.assertEquals(20, queue.size());
        Assert.assertEquals(Decimal64Utils.fromDouble(80), queue.getPointsAgo(19));
        Assert.assertEquals(80, queue.getTimeByIndexAgo(19));
        Assert.assertEquals(Decimal64Utils.fromDouble(80), queue.getFirst());
        Assert.assertEquals(80, queue.getFirstElementTime());
        Assert.assertEquals(Decimal64Utils.fromDouble(98), queue.getPrevious());


        Assert.assertEquals(Decimal64Utils.fromDouble(1790), queue.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(160870), queue.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(-10), queue.sum());
        Assert.assertEquals(-0.5, Decimal64Utils.toDouble(queue.arithmeticMean()), 0.01);
        Assert.assertEquals(8043.25, Decimal64Utils.toDouble(queue.variance()), 0.01);
        Assert.assertEquals(-179.36, Decimal64Utils.toDouble(queue.coefficientOfVariation()), 0.01);
        Assert.assertEquals(-0.5, Decimal64Utils.toDouble(queue.expectedValue()), 0.001);

        DecimalLongDataQueue queueClone = queue.clone();
        queueClone.put(Decimal64Utils.fromDouble(100), 100);
        Assert.assertEquals(Decimal64Utils.fromDouble(100), queueClone.getPointsAgo(0));
        Assert.assertEquals(100, queueClone.getTimeByIndexAgo(0));
        Assert.assertEquals(Decimal64Utils.fromDouble(-99), queue.getPointsAgo(0));
        Assert.assertEquals(99, queue.getTimeByIndexAgo(0));

        Assert.assertEquals(Decimal64Utils.fromDouble(1810), queueClone.sumOfAbsoluteValues());
        Assert.assertEquals(Decimal64Utils.fromDouble(164470), queueClone.sumOfSquares());
        Assert.assertEquals(Decimal64Utils.fromDouble(10), queueClone.sum());
        Assert.assertEquals(20, queueClone.size());

        queue.setPointsAgo(1, Decimal64Utils.ZERO);
        Assert.assertEquals(Decimal64Utils.fromDouble(0), queue.getPointsAgo(1));
        Assert.assertEquals(98, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(queue.getLast(), queue.getPointsAgo(0));
        Assert.assertEquals(Decimal64Utils.fromDouble(-108), queue.sum());
        Assert.assertEquals(20, queueClone.size());

        Assert.assertEquals(-1, queue.getIndexByTime(25));
        Assert.assertEquals(19, queue.getIndexByTime(80));
        Assert.assertEquals(DecimalLongDataQueue.DEFAULT_VALUE, queue.getByTime(25));
        queue.setByTime(80, Decimal64Utils.fromDouble(100));
        Assert.assertEquals(Decimal64Utils.fromDouble(-88), queue.sum());

        Assert.assertEquals(99, queue.getLastElementTime());
        Assert.assertEquals(Decimal64Utils.fromDouble(100), queue.toArray()[0]);
        Assert.assertEquals(Decimal64Utils.fromDouble(100), queue.getFirst());
        queue.clear();
        Assert.assertEquals(0, queue.size());
    }

    @Test
    public void singleElementQueueTest() throws OperationNotSupportedException {
        DecimalLongDataQueue queue = new DecimalLongDataQueue(1, true, true);
        queue.put(Decimal64Utils.ONE, 1);
        Assert.assertEquals(Decimal64Utils.fromDouble(1), queue.sum());
        Assert.assertEquals(1, queue.size());
        queue.put(Decimal64Utils.TWO, 2);
        Assert.assertEquals(Decimal64Utils.fromDouble(2), queue.sum());
        Assert.assertEquals(Decimal64Utils.ONE, queue.getPointsAgo(1));
        Assert.assertEquals(Decimal64Utils.ONE, queue.getPrevious());
        Assert.assertEquals(Decimal64Utils.TWO, queue.getFirst());
        Assert.assertEquals(Decimal64Utils.TWO, queue.getLast());
        Assert.assertEquals(2, queue.getFirstElementTime());
        Assert.assertEquals(2, queue.getLastElementTime());
        Assert.assertEquals(1, queue.getTimeByIndexAgo(1));
        Assert.assertEquals(1, queue.size());
        queue.setPointsAgo(0, Decimal64Utils.TEN);
        Assert.assertEquals(Decimal64Utils.fromDouble(10), queue.sum());

        DecimalLongDataQueue queueClone = queue.clone();
        Assert.assertEquals(Decimal64Utils.fromDouble(10), queueClone.sum());
        queueClone.put(Decimal64Utils.ONE, 55);
        Assert.assertEquals(Decimal64Utils.TEN, queue.getFirst());
        Assert.assertEquals(Decimal64Utils.fromDouble(1), queueClone.getFirst());
    }

    private int callbacksCallsCount = 0;
    private long lastTime;
    private Long lastDate;

    @Test
    public void queueListenersTest() throws OperationNotSupportedException {
        DecimalLongDataQueue queue = new DecimalLongDataQueue(20, true, true);
        queue.addOnPushListener(new BiConsumer<Long, Long>() {
            @Override
            public void accept(Long data, Long time) {
                callbacksCallsCount++;
                lastTime = time;
            }
        });
        for (int i = 0; i < 100; i++) {
            if (i % 2 == 0)
                queue.put(Decimal64Utils.fromDouble(i), i);
            else queue.put(Decimal64Utils.fromDouble(-i), i);
        }
        Assert.assertEquals(100, callbacksCallsCount);
        Assert.assertEquals(99, lastTime);
        queue.addOnPopListener(new BiConsumer<Long, Long>() {
            @Override
            public void accept(Long data, Long time) {
                callbacksCallsCount++;
                lastDate = data;
            }
        });
        callbacksCallsCount = 0;
        for (int i = 0; i < 10; i++)
            queue.put(Decimal64Utils.ZERO, 100 + i);
        Assert.assertEquals(20, callbacksCallsCount);
        Assert.assertEquals((Long)Decimal64Utils.fromDouble(-89), lastDate);
    }
}

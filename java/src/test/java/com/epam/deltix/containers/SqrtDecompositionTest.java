package com.epam.deltix.containers;

import org.junit.Assert;
import org.junit.Test;

import java.util.Comparator;
import java.util.Random;
import java.util.TreeSet;

public class SqrtDecompositionTest {
    @Test
    public void stressTest() {
        int iterations = 1000000;
        Random rand = new Random(55);
        SortedSqrtDecomposition<Integer> sortedSqrtDecomposition = new SortedSqrtDecomposition<Integer>(new Comparator<Integer>() {
            @Override
            public int compare(Integer o1, Integer o2) {
                return o1.compareTo(o2);
            }
        });
        Assert.assertTrue(sortedSqrtDecomposition.isEmpty());

        TreeSet<Integer> set = new TreeSet<>();

        for (int i = 0; i < iterations; ++i) {
            int type = rand.nextInt();
            int oldSize = sortedSqrtDecomposition.size();
            if (type % 2 == 0) {
                int x = rand.nextInt();
                sortedSqrtDecomposition.addIfNotExist(x);
                set.add(x);
            } else {
                if (sortedSqrtDecomposition.size() == 0) continue;
                int position = Math.abs(rand.nextInt()) % sortedSqrtDecomposition.size();
                set.remove(sortedSqrtDecomposition.getAt(sortedSqrtDecomposition.getIteratorByIndex(position)));
                sortedSqrtDecomposition.remove(sortedSqrtDecomposition.getAt(sortedSqrtDecomposition.getIteratorByIndex(position)));
            }
            Assert.assertEquals(set.size(), sortedSqrtDecomposition.size());
            if (set.size() == 0) continue;
            int index = Math.abs(rand.nextInt()) % sortedSqrtDecomposition.size();
            int iteratorToIndex = sortedSqrtDecomposition.getIteratorByIndex(index);
            int elementAt = sortedSqrtDecomposition.getIterator(sortedSqrtDecomposition.getAt(iteratorToIndex));
            Assert.assertEquals(elementAt, iteratorToIndex);
            int sqrtIterator = sortedSqrtDecomposition.getFirst();
            for (Integer x: set) {
                if (index == 0) {
                    Assert.assertEquals(iteratorToIndex, sqrtIterator);
                }
                index--;
                Assert.assertEquals(x, sortedSqrtDecomposition.getAt(sqrtIterator));
                sqrtIterator = sortedSqrtDecomposition.getNext(sqrtIterator);
            }

        }
        Assert.assertFalse(sortedSqrtDecomposition.isEmpty());
    }

    @Test
    public void stressTestWithRemoveByIterator() {
        int iterations = 1000000;
        Random rand = new Random(55);
        SortedSqrtDecomposition<Integer> sortedSqrtDecomposition = new SortedSqrtDecomposition<Integer>(new Comparator<Integer>() {
            @Override
            public int compare(Integer o1, Integer o2) {
                return o1.compareTo(o2);
            }
        });

        TreeSet<Integer> set = new TreeSet<>();

        for (int i = 0; i < iterations; ++i) {
            int type = rand.nextInt();
            int oldSize = sortedSqrtDecomposition.size();
            if (type % 2 == 0) {
                int x = rand.nextInt();
                sortedSqrtDecomposition.addIfNotExist(x);
                set.add(x);
            } else {
                if (sortedSqrtDecomposition.size() == 0) continue;
                int position = Math.abs(rand.nextInt()) % sortedSqrtDecomposition.size();
                set.remove(sortedSqrtDecomposition.getAt(sortedSqrtDecomposition.getIteratorByIndex(position)));
                sortedSqrtDecomposition.removeAt((sortedSqrtDecomposition.getIteratorByIndex(position)));
            }
            Assert.assertEquals(set.size(), sortedSqrtDecomposition.size());
            if (set.size() == 0) continue;
            int index = Math.abs(rand.nextInt()) % sortedSqrtDecomposition.size();
            int iteratorToIndex = sortedSqrtDecomposition.getIteratorByIndex(index);
            int elementAt = sortedSqrtDecomposition.getIterator(sortedSqrtDecomposition.getAt(iteratorToIndex));
            Assert.assertEquals(elementAt, iteratorToIndex);
            int sqrtIterator = sortedSqrtDecomposition.getFirst();
            for (Integer x: set) {
                if (index == 0) {
                    Assert.assertEquals(iteratorToIndex, sqrtIterator);
                }
                index--;
                Assert.assertEquals(x, sortedSqrtDecomposition.getAt(sqrtIterator));
                sqrtIterator = sortedSqrtDecomposition.getNext(sqrtIterator);
            }

        }
    }

}

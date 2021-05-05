package com.epam.deltix.containers;

import com.epam.deltix.containers.generated.LongHashSet;
import org.junit.Assert;
import org.junit.Test;

import java.util.*;

public class HashSetTests {

    @Test
    public void IllegalCapacityTest() {
        int flg = 0;
        try {
            ObjHashSet<Long> tested = new ObjHashSet<>(-3);
        } catch (IllegalArgumentException e) {
            flg = 1;
        }
        finally {
            Assert.assertEquals(flg, 1);
        }
    }

    @Test
    public void hashSetStressTest() {
        long iterations = 1000000;
        Random r = new Random();
        HashSet<Long> etalon = new HashSet<>();
        ObjHashSet<Long> tested = new ObjHashSet<>();
        Assert.assertTrue(tested.isEmpty());
        ArrayList<Long> values = new ArrayList<>();
        for (int i = 0; i < iterations; ++i) {
            if (r.nextInt() % 2 == 0) {
                long x = r.nextInt();
                values.add(x);
                etalon.add(x);
                tested.put(x);
                Assert.assertEquals(etalon.contains(x), tested.containsKey(x));
            } else if (etalon.size() > 0) {
                long x = etalon.iterator().next();
                etalon.remove(x);
                tested.remove(x);
                Assert.assertEquals(etalon.contains(x), tested.containsKey(x));
            }
        }

        Assert.assertEquals(etalon.size(), tested.size());

        ArrayList<Long> etalonList = new ArrayList<>();
        ArrayList<Long> testedList = new ArrayList<>();

        for (Long x : etalon) {
            etalonList.add(x);
        }

        for (Long x : tested) {
            testedList.add(x);
        }

        Collections.sort(etalonList);
        Collections.sort(testedList);

        Assert.assertEquals(etalonList.size(), testedList.size());

        for (int i = 0; i < etalonList.size(); ++i) {
            Assert.assertEquals(etalonList.get(i), testedList.get(i));
        }

        for (int i = 0; i < values.size(); ++i) {
            Assert.assertEquals(etalon.contains((long)values.get(i)), tested.containsKey(values.get(i)));
        }
        Assert.assertFalse(tested.isEmpty());
    }

    @Test
    public void primitiveHashSetStressTest() {
        long iterations = 1000000;
        Random r = new Random();
        HashSet<Long> etalon = new HashSet<>();
        LongHashSet tested = new LongHashSet();
        ArrayList<Long> values = new ArrayList<>();
        for (int i = 0; i < iterations; ++i) {
            if (r.nextInt() % 2 == 0) {
                long x = r.nextInt();
                values.add(x);
                etalon.add(x);
                tested.put(x);
                Assert.assertEquals(etalon.contains(x), tested.containsKey(x));
            } else if (etalon.size() > 0) {
                long x = etalon.iterator().next();
                etalon.remove(x);
                tested.remove(x);
                Assert.assertEquals(etalon.contains(x), tested.containsKey(x));
            }
        }

        Assert.assertEquals(etalon.size(), tested.size());

        ArrayList<Long> etalonList = new ArrayList<>();
        ArrayList<Long> testedList = new ArrayList<>();

        for (Long x : etalon) {
            etalonList.add(x);
        }

        for (Long x : tested) {
            testedList.add(x);
        }

        Collections.sort(etalonList);
        Collections.sort(testedList);

        Assert.assertEquals(etalonList.size(), testedList.size());

        for (int i = 0; i < etalonList.size(); ++i) {
            Assert.assertEquals(etalonList.get(i), testedList.get(i));
        }


        for (int i = 0; i < values.size(); ++i) {
            Assert.assertEquals(etalon.contains((long)values.get(i)), tested.containsKey(values.get(i)));
        }
    }

}

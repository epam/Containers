package com.epam.deltix.containers;

import com.epam.deltix.containers.generated.*;
import org.junit.Assert;
import org.junit.Test;
import org.junit.experimental.categories.Category;

import java.util.*;

/**
 * Created by DriapkoA on 29/08/2017.
 */
@Category(Test.class)
public class HashMapTest {


    @Test
    public void unsafeIterationTest() {
        int numberOfIteration = 2000000;
        ObjToObjHashMap<BinaryArray, Integer> myHashMap = new ObjToObjHashMap<>(-1);
        Assert.assertTrue(myHashMap.isEmpty());
        HashMap<BinaryArray, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                BinaryArray ar = new BinaryArray().assign(i);
                etalon.put(ar, i);
                long iterator = myHashMap.locateOrReserve(ar);
                myHashMap.setKeyAt(iterator, ar);
                myHashMap.setValueAt(iterator, i);
                Assert.assertEquals(myHashMap.get(ar).longValue(), etalon.get(ar).longValue());
            } else {
                int x = rand.nextInt(i);
                BinaryArray ar = new BinaryArray().assign(x);
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
                if (myHashMap.containsKey(ar)) {
                    long key = myHashMap.locate(ar);
                    Assert.assertEquals(true, myHashMap.getKeyAt(myHashMap.locate(ar)).equals(ar));
                    Assert.assertEquals(x, myHashMap.getValueAt(myHashMap.locate(ar)).longValue());
                    myHashMap.removeAt(key);
                    etalon.remove(ar);
                }
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
            }

            if (i % 10000 == 0) {
                ArrayList<Integer> valuesIteration = new ArrayList<>();
                ArrayList<Integer> valuesUnsafeIteration = new ArrayList<>();

                for (long iterator = myHashMap.getFirst(); iterator >= 0; iterator = myHashMap.getNext(iterator)) {
                    valuesIteration.add(myHashMap.getValueAt(iterator));
                };

                for (int unsafeIterator = myHashMap.getUnsafeFirst(); unsafeIterator >= 0; unsafeIterator = myHashMap.getUnsafeNext(unsafeIterator)) {
                    valuesUnsafeIteration.add(myHashMap.getValueByUnsafeIterator(unsafeIterator));
                }

                Collections.sort(valuesIteration);
                Collections.sort(valuesUnsafeIteration);

                Assert.assertEquals(valuesIteration.size(), valuesUnsafeIteration.size());
                for (int j = 0; j < valuesIteration.size(); ++j) {
                    Assert.assertEquals(valuesIteration.get(j), valuesUnsafeIteration.get(j));
                }
            }
        }
        Assert.assertFalse(myHashMap.isEmpty());
    }

    @Test
    public void intToIntExtendedHashMapTests() {
        IntToIntHashMap extendedObjToObjHashMap = new IntToIntHashMap(-1);
        Assert.assertEquals(-1, extendedObjToObjHashMap.setAndGet(1, 1));
        Assert.assertEquals(1, extendedObjToObjHashMap.setAndGet(1, 1));

        Assert.assertEquals(true, extendedObjToObjHashMap.trySet(2, 1));
        Assert.assertEquals(false, extendedObjToObjHashMap.trySet(2, 1));

        Assert.assertEquals(true, extendedObjToObjHashMap.trySet(3, 1));

        Assert.assertEquals(-1, extendedObjToObjHashMap.get(4));
        Assert.assertEquals(1, extendedObjToObjHashMap.get(3));

        Assert.assertEquals(1, extendedObjToObjHashMap.remove(2));
        Assert.assertEquals(-1, extendedObjToObjHashMap.remove(2));

    }



    @Test
    public void intToObjExtendedHashMapTests() {
        IntToObjHashMap<Integer> extendedObjToObjHashMap = new IntToObjHashMap<Integer>(-1);
        Assert.assertEquals(-1, extendedObjToObjHashMap.setAndGet(1, 1).longValue());
        Assert.assertEquals(1, extendedObjToObjHashMap.setAndGet(1, 1).longValue());

        Assert.assertEquals(true, extendedObjToObjHashMap.trySet(2, 1));
        Assert.assertEquals(false, extendedObjToObjHashMap.trySet(2, 1));

        Assert.assertEquals(-1, extendedObjToObjHashMap.setAndGet(3, 1).longValue());
        Assert.assertEquals(-1, extendedObjToObjHashMap.get(4).longValue());
        Assert.assertEquals(1, extendedObjToObjHashMap.get(3).longValue());


        Assert.assertEquals(1, extendedObjToObjHashMap.remove(2).longValue());
        Assert.assertEquals(-1, extendedObjToObjHashMap.remove(2).longValue());

    }


    @Test
    public void objToIntExtendedHashMapTests() {
        ObjToIntHashMap<BinaryArray> extendedObjToObjHashMap = new ObjToIntHashMap<>(-1);
        Assert.assertEquals(-1, extendedObjToObjHashMap.setAndGet(new BinaryArray().assign(1), 1));
        Assert.assertEquals(1, extendedObjToObjHashMap.setAndGet(new BinaryArray().assign(1), 1));

        Assert.assertEquals(true, extendedObjToObjHashMap.trySet(new BinaryArray().assign(2), 1));
        Assert.assertEquals(false, extendedObjToObjHashMap.trySet(new BinaryArray().assign(2), 1));

        Assert.assertEquals(true, extendedObjToObjHashMap.trySet(new BinaryArray().assign(3), 1));
        Assert.assertEquals(-1, extendedObjToObjHashMap.get(new BinaryArray().assign(4)));
        Assert.assertEquals(1, extendedObjToObjHashMap.get(new BinaryArray().assign(3)));

        Assert.assertEquals(1, extendedObjToObjHashMap.remove(new BinaryArray().assign(2)));
        Assert.assertEquals(-1, extendedObjToObjHashMap.remove(new BinaryArray().assign(2)));

    }


    @Test
    public void objToObjExtendedHashMapTests() {
        ObjToObjHashMap<BinaryArray, Integer> extendedObjToObjHashMap = new ObjToObjHashMap<>(-1);
        Assert.assertEquals(-1, extendedObjToObjHashMap.setAndGet(new BinaryArray().assign(1), 1).longValue());
        Assert.assertEquals(1, extendedObjToObjHashMap.setAndGet(new BinaryArray().assign(1), 1).longValue());

        Assert.assertEquals(true, extendedObjToObjHashMap.trySet(new BinaryArray().assign(2), 1));
        Assert.assertEquals(false, extendedObjToObjHashMap.trySet(new BinaryArray().assign(2), 1));

        Assert.assertEquals(true, extendedObjToObjHashMap.trySet(new BinaryArray().assign(3), 1));
        Assert.assertEquals(-1, extendedObjToObjHashMap.get(new BinaryArray().assign(4)).longValue());
        Assert.assertEquals(1, extendedObjToObjHashMap.get(new BinaryArray().assign(3)).longValue());

        Assert.assertEquals(1, extendedObjToObjHashMap.remove(new BinaryArray().assign(2)).longValue());
        Assert.assertEquals(-1, extendedObjToObjHashMap.remove(new BinaryArray().assign(2)).longValue());

    }


    @Test
    public void intToIntIteratingTest() {
        IntToIntHashMap myHashMap = new IntToIntHashMap(-1);
        ArrayList<Integer> integerArrayList = new ArrayList<>();
        for (int i = 0; i < 10; ++i) {
            myHashMap.set(i, i);
        }
        for (long iterator = myHashMap.getFirst(); iterator != ObjToIntHashMap.NO_ELEMENT; iterator = myHashMap.getNext(iterator)) {
            Assert.assertEquals(myHashMap.getKeyAt(iterator), myHashMap.getValueAt(iterator));
            integerArrayList.add(myHashMap.getValueAt(iterator));
        }
        Collections.sort(integerArrayList);
        Assert.assertEquals(10, integerArrayList.size());
        for (int i = 0; i < 10; ++i) Assert.assertEquals(i, integerArrayList.get(i).longValue());
    }

    @Test
    public void intToObjIteratingTest() {
        IntToObjHashMap<Integer> myHashMap = new IntToObjHashMap<>(-1);
        ArrayList<Integer> integerArrayList = new ArrayList<>();
        for (int i = 0; i < 10; ++i) {
            myHashMap.set(i, i);
        }
        for (long iterator = myHashMap.getFirst(); iterator != ObjToIntHashMap.NO_ELEMENT; iterator = myHashMap.getNext(iterator)) {
            Assert.assertEquals(myHashMap.getKeyAt(iterator), myHashMap.getValueAt(iterator).longValue());
            integerArrayList.add(myHashMap.getValueAt(iterator));
        }
        Collections.sort(integerArrayList);
        Assert.assertEquals(10, integerArrayList.size());
        for (int i = 0; i < 10; ++i) Assert.assertEquals(i, integerArrayList.get(i).longValue());

    }

    @Test
    public void objToIntIteratingTest() {
        ObjToIntHashMap<BinaryArray> myHashMap = new ObjToIntHashMap<>(-1);
        ArrayList<Integer> integerArrayList = new ArrayList<>();
        for (int i = 0; i < 10; ++i) {
            myHashMap.set(new BinaryArray().assign(i), i);
        }
        for (long iterator = myHashMap.getFirst(); iterator != ObjToIntHashMap.NO_ELEMENT; iterator = myHashMap.getNext(iterator)) {
            Assert.assertEquals(true, myHashMap.getKeyAt(iterator).equals(new BinaryArray().assign(myHashMap.getValueAt(iterator))));
            integerArrayList.add(myHashMap.getValueAt(iterator));
        }
        Collections.sort(integerArrayList);
        Assert.assertEquals(10, integerArrayList.size());
        for (int i = 0; i < 10; ++i) Assert.assertEquals(i, integerArrayList.get(i).longValue());

    }


    @Test
    public void objToObjIteratingTest() {
        ObjToObjHashMap<BinaryArray, Integer> myHashMap = new ObjToObjHashMap<>(-1);
        ArrayList<Integer> integerArrayList = new ArrayList<>();
        for (int i = 0; i < 10; ++i) {
            myHashMap.set(new BinaryArray().assign(i), i);
        }
        for (long iterator = myHashMap.getFirst(); iterator != ObjToObjHashMap.NO_ELEMENT; iterator = myHashMap.getNext(iterator)) {
            Assert.assertEquals(true, myHashMap.getKeyAt(iterator).equals(new BinaryArray().assign(myHashMap.getValueAt(iterator))));
            integerArrayList.add(myHashMap.getValueAt(iterator));
        }
        Collections.sort(integerArrayList);
        Assert.assertEquals(10, integerArrayList.size());
        for (int i = 0; i < 10; ++i) Assert.assertEquals(i, integerArrayList.get(i).longValue());
    }

    @Test
    public void charSequenceToIntIteratingTest() {
        CharSequenceToIntHashMap myHashMap = new CharSequenceToIntHashMap(-1);
        ArrayList<Integer> integerArrayList = new ArrayList<>();
        for (int i = 0; i < 10; ++i) {
            myHashMap.set("abc" + i, i);
        }
        for (long iterator = myHashMap.getFirst(); iterator != CharSequenceToIntHashMap.NO_ELEMENT; iterator = myHashMap.getNext(iterator)) {
            Assert.assertTrue(CharSequenceUtils.equals(myHashMap.getKeyAt(iterator), "abc" + myHashMap.getValueAt(iterator)));
            integerArrayList.add(myHashMap.getValueAt(iterator));
        }
        Collections.sort(integerArrayList);
        Assert.assertEquals(10, integerArrayList.size());
        for (int i = 0; i < 10; ++i) {
            Assert.assertEquals(i, integerArrayList.get(i).longValue());
            Assert.assertTrue(myHashMap.containsKey("abc" + i));
        }
    }

    @Test
    public void charSequenceToObjIteratingTest() {
        CharSequenceToObjHashMap myHashMap = new CharSequenceToObjHashMap(-1);
        for (int i = 0; i < 10; ++i) {
            myHashMap.set("abc" + i, "geabc" + i);
        }
        for (long iterator = myHashMap.getFirst(); iterator != CharSequenceToIntHashMap.NO_ELEMENT; iterator = myHashMap.getNext(iterator)) {
            Assert.assertTrue(CharSequenceUtils.equals("ge" + myHashMap.getKeyAt(iterator), myHashMap.getValueAt(iterator).toString()));
        }
        for (int i = 0; i < 10; ++i) {
            Assert.assertTrue(myHashMap.containsKey("abc" + i));
        }
    }




    @Test
    public void intToIntTest() {
        IntToIntHashMap myHashMap = new IntToIntHashMap(-1);
        myHashMap.set(1, 10);
        myHashMap.set(1, 20);
        Assert.assertEquals(20, myHashMap.get(1));
        Assert.assertEquals(-1, myHashMap.get(2));
        myHashMap.locateOrReserve(12345);
        myHashMap.remove(12345);
        myHashMap.locateOrReserve(12345);

    }

    @Test
    public void intToObjTest() {
        IntToObjHashMap<Integer> myHashMap = new IntToObjHashMap<Integer>(-1);
        myHashMap.set(1, 10);
        myHashMap.set(1, 20);
        Assert.assertEquals(20, myHashMap.get(1).longValue());
        boolean flag = false;
        Assert.assertEquals(-1, myHashMap.get(2).longValue());
        myHashMap.locateOrReserve(12345);
        myHashMap.remove(12345);
        myHashMap.locateOrReserve(12345);

    }

    @Test
    public void objToIntTest() {
        ObjToIntHashMap<BinaryArray> myHashMap = new ObjToIntHashMap<BinaryArray>(-1);
        myHashMap.set(new BinaryArray().assign(1), 10);
        myHashMap.set(new BinaryArray().assign(1), 20);
        Assert.assertEquals(20, myHashMap.get(new BinaryArray().assign(1)));
        Assert.assertEquals(-1, myHashMap.get(new BinaryArray().assign(2)));
        myHashMap.locateOrReserve(new BinaryArray().assign("12345"));
        myHashMap.remove(new BinaryArray("12345"));
        myHashMap.locateOrReserve(new BinaryArray().assign("12345"));

    }


    @Test
    public void objToObjTest() {
        ObjToObjHashMap<BinaryArray, Integer> myHashMap = new ObjToObjHashMap<>(-1);
        myHashMap.set(new BinaryArray().assign(1), 10);
        myHashMap.set(new BinaryArray().assign(1), 20);
        Assert.assertEquals(20, myHashMap.get(new BinaryArray().assign(1)).longValue());
        Assert.assertEquals(-1, myHashMap.get(new BinaryArray().assign(2)).longValue());

        myHashMap.locateOrReserve(new BinaryArray().assign("12345"));
        myHashMap.remove(new BinaryArray("12345"));
        myHashMap.locateOrReserve(new BinaryArray().assign("12345"));
    }



    @Test
    public void objToObjLocateStressTest() throws Exception {
        int numberOfIteration = 2000000;
        ObjToObjHashMap<BinaryArray, Integer> myHashMap = new ObjToObjHashMap<>(-1);
        HashMap<BinaryArray, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                BinaryArray ar = new BinaryArray().assign(i);
                etalon.put(ar, i);
                long iterator = myHashMap.locateOrReserve(ar);
                myHashMap.setKeyAt(iterator, ar);
                myHashMap.setValueAt(iterator, i);
                Assert.assertEquals(myHashMap.get(ar).longValue(), etalon.get(ar).longValue());
            } else {
                int x = rand.nextInt(i);
                BinaryArray ar = new BinaryArray().assign(x);
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
                if (myHashMap.containsKey(ar)) {
                    long key = myHashMap.locate(ar);
                    Assert.assertEquals(true, myHashMap.getKeyAt(myHashMap.locate(ar)).equals(ar));
                    Assert.assertEquals(x, myHashMap.getValueAt(myHashMap.locate(ar)).longValue());
                    myHashMap.removeAt(key);
                    etalon.remove(ar);
                }
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
            }

            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }

    @Test
    public void intToObjLocateStressTest() throws Exception {
        int numberOfIteration = 2000000;
        IntToObjHashMap<Integer> myHashMap = new IntToObjHashMap<>(10,-1);

        HashMap<Integer, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                BinaryArray ar = new BinaryArray().assign(i);
                etalon.put(i, i);
                long iterator = myHashMap.locateOrReserve(i);
                myHashMap.setKeyAt(iterator, i);
                myHashMap.setValueAt(iterator, i);
                Assert.assertEquals(myHashMap.get(i).longValue(), etalon.get(i).longValue());
            } else {
                int x = rand.nextInt(i);
                BinaryArray ar = new BinaryArray().assign(x);
                Assert.assertEquals(myHashMap.containsKey(i), etalon.containsKey(i));
                if (myHashMap.containsKey(i)) {
                    long key = myHashMap.locate(i);
                    Assert.assertEquals(true, myHashMap.getKeyAt(myHashMap.locate(i)) == i);
                    Assert.assertEquals(x, myHashMap.getValueAt(myHashMap.locate(i)).longValue());
                    myHashMap.removeAt(key);
                    etalon.remove(i);
                }
                Assert.assertEquals(myHashMap.containsKey(i), etalon.containsKey(i));
            }

            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }




    @Test
    public void objToIntLocateStressTest() throws Exception {
        int numberOfIteration = 2000000;
        ObjToIntHashMap<BinaryArray> myHashMap = new ObjToIntHashMap<BinaryArray>(-1);
        HashMap<BinaryArray, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                BinaryArray ar = new BinaryArray().assign(i);
                etalon.put(ar, i);
                long iterator = myHashMap.locateOrReserve(ar);
                myHashMap.setKeyAt(iterator, ar);
                myHashMap.setValueAt(iterator, i);
                Assert.assertEquals(myHashMap.get(ar), etalon.get(ar).longValue());
            } else {
                int x = rand.nextInt(i);
                BinaryArray ar = new BinaryArray().assign(x);
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
                if (myHashMap.containsKey(ar)) {
                    long key = myHashMap.locate(ar);
                    Assert.assertEquals(true, myHashMap.getKeyAt(myHashMap.locate(ar)).equals(ar));
                    Assert.assertEquals(x, myHashMap.getValueAt(myHashMap.locate(ar)));

                    myHashMap.removeAt(key);
                    etalon.remove(ar);
                }
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
            }

            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }

    @Test
    public void intToIntLocateStressTest() throws Exception {
        int numberOfIteration = 2000000;
        IntToIntHashMap myHashMap = new IntToIntHashMap(10);

        HashMap<Integer, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                BinaryArray ar = new BinaryArray().assign(i);
                etalon.put(i, i);
                long iterator = myHashMap.locateOrReserve(i);
                myHashMap.setKeyAt(iterator, i);
                myHashMap.setValueAt(iterator, i);
                Assert.assertEquals(myHashMap.get(i), etalon.get(i).longValue());
            } else {
                int x = rand.nextInt(i);
                BinaryArray ar = new BinaryArray().assign(x);
                Assert.assertEquals(myHashMap.containsKey(i), etalon.containsKey(i));
                if (myHashMap.containsKey(i)) {
                    long key = myHashMap.locate(i);
                    Assert.assertEquals(true, myHashMap.getKeyAt(myHashMap.locate(i)) == i);
                    Assert.assertEquals(x, myHashMap.getValueAt(myHashMap.locate(i)));
                    myHashMap.removeAt(key);
                    etalon.remove(ar);
                }
                Assert.assertEquals(myHashMap.containsKey(i), etalon.containsKey(i));
            }

            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }


    ////////////////////////////////////////////////////////////////

    @Test
    public void objToObjStressTest() throws Exception {
        int numberOfIteration = 2000000;
        ObjToObjHashMap<BinaryArray, Integer> myHashMap = new ObjToObjHashMap<>(-1);
        HashMap<BinaryArray, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                BinaryArray ar = new BinaryArray().assign(i);
                etalon.put(ar, i);
                myHashMap.set(ar, i);
                Assert.assertEquals(myHashMap.get(ar).longValue(), etalon.get(ar).longValue());
            } else {
                int x = rand.nextInt(i);
                BinaryArray ar = new BinaryArray().assign(x);
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
                if (myHashMap.containsKey(ar)) {
                    myHashMap.remove(ar);
                    etalon.remove(ar);
                }
            }

            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }

    @Test
    public void intToObjStressTest() throws Exception {
        int numberOfIteration = 2000000;
        IntToObjHashMap<Integer> myHashMap = new IntToObjHashMap<>(10);

        HashMap<Integer, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                etalon.put(i, i);
                myHashMap.set(i, i);
                Assert.assertEquals(etalon.get(i).longValue(), myHashMap.get(i).longValue());
                Assert.assertEquals(etalon.containsKey(243), myHashMap.containsKey(243));
            } else {
                int x = rand.nextInt(i);
                if (myHashMap.containsKey(x)) {
                    myHashMap.remove(x);
                    etalon.remove(x);
                }
                Assert.assertEquals(etalon.containsKey(x), myHashMap.containsKey(x));
                Assert.assertEquals(etalon.containsKey(243), myHashMap.containsKey(243));
            }

            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }




    @Test
    public void objToIntStressTest() throws Exception {
        int numberOfIteration = 2000000;
        ObjToIntHashMap myHashMap = new ObjToIntHashMap(-1);

        HashMap<BinaryArray, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                BinaryArray ar = new BinaryArray().assign(i);
                etalon.put(ar, i);
                myHashMap.set(ar, i);
                Assert.assertEquals(myHashMap.get(ar), etalon.get(ar).longValue());
            } else {
                int x = rand.nextInt(i);
                BinaryArray ar = new BinaryArray().assign(x);
                Assert.assertEquals(myHashMap.containsKey(ar), etalon.containsKey(ar));
                if (myHashMap.containsKey(ar)) {
                    myHashMap.remove(ar);
                    etalon.remove(ar);
                }
            }
            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }

    @Test
    public void intToIntStressTest() throws Exception {
        int numberOfIteration = 2000000;
        IntToIntHashMap myHashMap = new IntToIntHashMap(10);
        HashMap<Integer, Integer> etalon = new HashMap<>();
        Random rand = new Random(55);
        BinaryArray lastBinaryArray = null;
        for (int i = 0; i < numberOfIteration; ++i) {
            if (rand.nextInt(3) != 0) {
                etalon.put(i, i);
                myHashMap.set(i, i);
                Assert.assertEquals(etalon.get(i).longValue(), myHashMap.get(i));
                Assert.assertEquals(etalon.containsKey(243), myHashMap.containsKey(243));
            } else {
                int x = rand.nextInt(i);
                if (myHashMap.containsKey(x)) {
                    myHashMap.remove(x);
                    etalon.remove(x);
                }
                Assert.assertEquals(etalon.containsKey(x), myHashMap.containsKey(x));
                Assert.assertEquals(etalon.containsKey(243), myHashMap.containsKey(243));
            }
            if (i % 1000000 == 0) {
                myHashMap.clear();
                etalon.clear();
                lastBinaryArray = null;
            }
        }
    }

}

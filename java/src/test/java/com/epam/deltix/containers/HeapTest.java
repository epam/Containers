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

import com.epam.deltix.containers.generated.LongLongPair;
import org.junit.Assert;
import org.junit.Test;
import org.junit.experimental.categories.Category;

import java.util.*;

@Category(Test.class)
public class HeapTest {

    @Test
    public void heapWithDictionaryRemoveBadElement() {
        HeapWithDictionary<Long, Long> myHeap = new HeapWithDictionary<Long, Long>(10, new Comparator<Long>() {
            @Override
            public int compare(Long o1, Long o2) {
                return o1.compareTo(o2);
            }
        });
        Assert.assertTrue(myHeap.isEmpty());
        for (int i = 0; i < 100; ++i) {
            myHeap.add((long)i, (long)i);
            myHeap.add((long)i, (long)i);
            myHeap.add((long)i, (long)i);
        }
        Assert.assertFalse(myHeap.isEmpty());
        myHeap.remove(101l);
        myHeap.modify(101l, 11l);
        Assert.assertEquals(-10l, myHeap.getValue(101l, -10l).longValue());
    }

    @Test
    public void heapWithDictionaryStressTest() {
        int numberOfIterations = 10000000;
        TreeMap<Long, Long> priorityQueue = new TreeMap<Long, Long>();
        HashMap<Long, Long> dictionary = new HashMap<Long, Long>();
        HeapWithDictionary<Long, Long> myHeap = new HeapWithDictionary<Long, Long>(10, new Comparator<Long>() {
            @Override
            public int compare(Long o1, Long o2) {
                return o1.compareTo(o2);
            }
        });
        long lastKey = 0;
        Random rand = new Random(55);
        for (int i = 0; i < numberOfIterations; ++i) {
            if (rand.nextInt() % 3 == 0) {
                if (priorityQueue.size() > 0) {
                    Assert.assertEquals(priorityQueue.size(), myHeap.getCount());
                    Long value = priorityQueue.firstKey();
                    long key = priorityQueue.firstEntry().getValue();
                    Assert.assertEquals((long) value, (long) myHeap.peekValue());
                    Assert.assertEquals((long) key, (long) myHeap.peekKey());
                    dictionary.remove(key);
                    myHeap.pop();
                    priorityQueue.remove(value);
                }
            } else {
                Long x = rand.nextLong();
                myHeap.add(x, (long) i);
                priorityQueue.put(x, (long) i);
                dictionary.put((long) i, x);
                lastKey = i;
            }
            if (rand.nextInt() % 3 == 0) {
                if (rand.nextInt() % 2 == 0) {
                    if (priorityQueue.size() > 0) {
                        if (dictionary.containsKey(lastKey)) {
                            priorityQueue.remove(dictionary.get(lastKey));
                            myHeap.remove((long) lastKey);
                            dictionary.remove(lastKey);
                        }
                    }
                } else {
                    if (priorityQueue.size() > 0) {


                        if (rand.nextInt() % 2 == 0) {
                            long newTop = rand.nextLong();
                            Long key = myHeap.peekKey();
                            priorityQueue.remove(dictionary.get(key));
                            dictionary.remove(key);
                            priorityQueue.put(newTop, key);
                            dictionary.put(key, newTop);
                            myHeap.modifyTop(newTop);

                        } else if (myHeap.containsKey((long) lastKey)) {
                            long newTop = rand.nextLong();
                            long key = lastKey;
                            priorityQueue.remove(dictionary.get(key));
                            dictionary.remove(key);
                            priorityQueue.put(newTop, key);
                            dictionary.put(key, newTop);
                            myHeap.modify(key, newTop);
                        }
                    }
                }
            }
        }
    }

    @Test
    public void heapWithIndicesStressTestModifyTop() {
        int k = 1000000;
        HeapWithIndices<Long> heap = new HeapWithIndices<Long>(new Comparator<Long>() {
            @Override
            public int compare(Long o1, Long o2) {
                return o1.compareTo(o2);
            }
        });
        java.util.TreeSet<Integer> keys = new TreeSet<>();
        Random rand = new Random(55);
        for (int i = 0; i < k; ++i) {
            if (Math.abs(rand.nextInt()) % 3 == 1) {
                heap.modifyTop(rand.nextLong());
            } else {
                keys.add(heap.add(rand.nextLong()));
            }
        }

    }


    @Test
    public void heapWithIndicesStressTest() {
        int numberOfIterations = 10000000;
        TreeMap<Long, Integer> priorityQueue = new TreeMap<Long, Integer>();
        HashMap<Integer, Long> dictionary = new HashMap<Integer, Long>();
        HeapWithIndices<Long> myHeap = new HeapWithIndices<Long>(10, new Comparator<Long>() {
            @Override
            public int compare(Long o1, Long o2) {
                return o1.compareTo(o2);
            }
        });
        int lastKey = 0;
        Random rand = new Random(55);
        for (int i = 0; i < numberOfIterations; ++i) {
            int type = rand.nextInt() % 3;
            if (type == 0) {
                if (priorityQueue.size() > 0) {
                    Assert.assertEquals(priorityQueue.size(), myHeap.getCount());
                    Long value = priorityQueue.firstKey();
                    int key = priorityQueue.firstEntry().getValue();
                    Assert.assertEquals((long) value, (long) myHeap.peekValue());
                    Assert.assertEquals((long) key, (long) myHeap.peekKey());
                    dictionary.remove(key);
                    myHeap.pop();
                    priorityQueue.remove(value);
                }
            }
            if (rand.nextDouble() > 0.1) {
                Long x = rand.nextLong();
                int y = myHeap.add(x);
                priorityQueue.put(x, y);
                dictionary.put(y, x);
                lastKey = y;
            }
            if (type == 1) {
                if (priorityQueue.size() > 0) {
                    if (dictionary.containsKey(lastKey)) {
                        priorityQueue.remove(dictionary.get(lastKey));
                        myHeap.remove(lastKey);
                        dictionary.remove(lastKey);
                    }
                }
            }
            if (type == 2) {
                if (priorityQueue.size() > 0) {
                    if (rand.nextDouble() < 0.5) {
                        long newTop = rand.nextLong();
                        int key = myHeap.peekKey();
                        priorityQueue.remove(dictionary.get(key));
                        dictionary.remove(key);
                        priorityQueue.put(newTop, key);
                        dictionary.put(key, newTop);
                        myHeap.modifyTop(newTop);
                    } else {
                        if (dictionary.containsKey(lastKey)) {
                            long newTop = rand.nextLong();
                            int key = lastKey;
                            priorityQueue.remove(dictionary.get(key));
                            dictionary.remove(key);
                            priorityQueue.put(newTop, key);
                            dictionary.put(key, newTop);
                            myHeap.modify(key, newTop);
                        }
                    }
                }
            }
        }
    }

    @Test
    public void heapWithAttachmentsStressTest() {
        int numberOfIterations = 10000000;

        PriorityQueue<LongLongPair> priorityQueue = new PriorityQueue<LongLongPair>(10, new Comparator<LongLongPair>() {
            @Override
            public int compare(LongLongPair o1, LongLongPair o2) {
                return Long.compare(o1.getFirst(), o2.getFirst());
            }
        });

        HeapWithAttachments<Long, Long> myHeap = new HeapWithAttachments<Long, Long>(10, new Comparator<Long>() {
            @Override
            public int compare(Long o1, Long o2) {
                return o1.compareTo(o2);
            }
        });

        Random rand = new Random(55);
        for (int i = 0; i < numberOfIterations; ++i) {
            if (rand.nextInt() % 3 == 0) {
                if (priorityQueue.size() > 0) {
                    Assert.assertEquals(priorityQueue.size(), myHeap.getCount());
                    LongLongPair top = priorityQueue.peek();
                    Assert.assertEquals((long) top.getFirst(), (long) myHeap.peekValue());
                    Assert.assertEquals((long) top.getSecond(), (long) myHeap.peekAttachments());
                    priorityQueue.poll();
                    myHeap.pop();
                }
            } else {
                Long x = rand.nextLong();
                Long y = rand.nextLong();
                myHeap.add(x, y);
                priorityQueue.add(new LongLongPair(x, y));
            }
            if (myHeap.getCount() > 0 && rand.nextInt() % 10 == 0) {
                long x = rand.nextLong();
                long y = myHeap.peekAttachments();
                myHeap.modifyTop(x);
                priorityQueue.poll();
                priorityQueue.add(new LongLongPair(x, y));
            }
        }

    }

    @Test
    public void heapStressTest() {
        int numberOfIterations = 10000000;
        PriorityQueue<Long> priorityQueue = new PriorityQueue<Long>(10, new Comparator<Long>() {
            @Override
            public int compare(Long o1, Long o2) {
                return o1.compareTo(o2);
            }
        });

        Heap<Long> myHeap = new Heap<Long>(10, new Comparator<Long>() {
            @Override
            public int compare(Long o1, Long o2) {
                return o1.compareTo(o2);
            }
        });

        Random rand = new Random(55);
        for (int i = 0; i < numberOfIterations; ++i) {
            if (rand.nextInt() % 3 == 0) {
                if (priorityQueue.size() > 0) {
                    Assert.assertEquals(priorityQueue.size(), myHeap.getCount());
                    Assert.assertEquals((long) priorityQueue.poll(), (long) myHeap.pop());
                }
            } else {
                Long x = rand.nextLong();
                myHeap.add(x);
                priorityQueue.add(x);
            }
            if (rand.nextInt() % 10 == 0 && myHeap.getCount() > 0) {
                long x = rand.nextLong();
                myHeap.modifyTop(x);
                priorityQueue.poll();
                priorityQueue.add(x);
            }
        }
    }
}
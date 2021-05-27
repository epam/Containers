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

import java.util.ArrayList;
import java.util.Collections;
import java.util.Random;

/**
 * Created by DriapkoA on 31/01/2018.
 */
public class AvlTreeTest {

    @Test
    public void heightTest()
    {
        AvlTree<Integer, Integer> tree = new AvlTree<Integer, Integer>();
        int number_of_nodes = 100000;
        Assert.assertTrue(tree.isEmpty());
        for(int i = 1; i <= number_of_nodes; i++)
        {
            tree.add(i, i);
        }
        int a = tree.getValueByIterator(tree.getIterator(100000));
        int b = tree.getValueByIterator(tree.getIterator(1));
        Assert.assertEquals(a, 100000);
        Assert.assertEquals(b, 1);
        int maxHeight = 0;
        for(int i = 1; i <= number_of_nodes; i++)
        {
            int h = tree.getHeight(i);
            if(maxHeight < h)
                maxHeight = h;
        }
        Assert.assertFalse(tree.isEmpty());
        Assert.assertEquals(maxHeight, 17);
    }

    @Test
    public void iteratorsTest()
    {
        AvlTree<Integer, Integer> tree = new AvlTree<Integer, Integer>();
        tree.add(5, 5);
        tree.add(3, 3);
        tree.add(7, 7);
        tree.add(9, 9);
        tree.add(11, 11);


        int it = tree.getFirst();
        Assert.assertEquals((int)tree.getValueByIterator(it), 3);
        it = tree.getNext(it);
        Assert.assertEquals((int)tree.getValueByIterator(it), 5);
        it = tree.getNext(it);
        Assert.assertEquals((int)tree.getValueByIterator(it), 7);
        it = tree.getNext(it);
        Assert.assertEquals((int)tree.getValueByIterator(it), 9);
        it = tree.getNext(it);
        Assert.assertEquals((int)tree.getValueByIterator(it), 11);
        it = tree.getNext(it);
        Assert.assertEquals(it, -1);


        tree = new AvlTree<Integer, Integer>();
        int number_of_nodes = 10000;
        ArrayList<Integer> keys = new ArrayList<>();
        Random random = new Random();
        for(int i = 0; i < number_of_nodes; i++)
        {
            int x = random.nextInt(100000);
            keys.add(x);
            tree.add(x, x);
        }
        it = tree.getFirst();
        Collections.sort(keys);

        for(int i = 0; i < number_of_nodes; i++)
        {
            Assert.assertEquals((int)tree.getValueByIterator(it), (int)keys.get(i));
            it = tree.getNext(it);
        }
        Assert.assertEquals(it, -1);

        Collections.shuffle(keys);
        for(int i = keys.size() - 1; i >= 0; i -= 2)
        {
            tree.remove(keys.get(i));
            keys.remove(i);
        }

        Collections.sort(keys);
        Collections.reverse(keys);
        it = tree.getLast();
        for(int i = 0; i < number_of_nodes / 2; i++)
        {
            Assert.assertEquals((int)tree.getValueByIterator(it), (int)keys.get(i));
            Assert.assertEquals((int)tree.getKeyByIterator(it), (int)keys.get(i));
            it = tree.getPrev(it);
        }
        Assert.assertEquals(it, -1);


    }

    @Test
    public void stressTest() {
        stressTest(100000, 4, 55);
    }


    public void stressTest(int count, int testsCount, int randomSeed) {
        int requestsCount = 5;
        int emptyRequest = 7;
        Random r = new Random(randomSeed);
        for (int testNumber = 0; testNumber < testsCount; testNumber++)
        {
            int current = 0;

            AvlTree<Double, Double> map = new AvlTree<Double, Double>();

            double[] dataFirst = new double[count];
            double[] dataSecond = new double[count];

            int it;

            double[] request = new double[count];
            int[] requestType = new int[count];
            double[] answer = new double[count];

            for (int requestNumber = 0; requestNumber < count; requestNumber++)
            {
                requestType[requestNumber] = Math.abs(r.nextInt()) % requestsCount;
                switch (requestType[requestNumber])
                {
                    case 0:
                    case 1:
                    case 2:
                    {
                        request[requestNumber] = r.nextDouble();
                        answer[requestNumber] = r.nextDouble();
                        dataFirst[current] = request[requestNumber];
                        dataSecond[current] = answer[requestNumber];

                        int j = current++;

                        while (j > 0 && dataFirst[j - 1] > dataFirst[j])
                        {
                            double tFirst = dataFirst[j - 1];
                            double tSecond = dataSecond[j - 1];

                            dataFirst[j - 1] = dataFirst[j];
                            dataFirst[j] = tFirst;

                            dataSecond[j - 1] = dataSecond[j];
                            dataSecond[j] = tSecond;

                            j--;
                        }
                    }
                    break;

                    case 3:
                    {
                        if (current > 0)
                        {
                            request[requestNumber] = r.nextInt(current);
                            answer[requestNumber] = dataFirst[(int)request[requestNumber]];
                        }
                        else
                            requestType[requestNumber] = emptyRequest;
                    }
                    break;

                    case 4:
                    {
                        if (current > 0)
                        {
                            int p = r.nextInt(current);

                            request[requestNumber] = dataFirst[p];
                            for (; p < current - 1; p++)
                            {
                                double tFirst = dataFirst[p];
                                double tSecond = dataSecond[p];

                                dataFirst[p] = dataFirst[p + 1];
                                dataSecond[p] = dataSecond[p + 1];

                                dataFirst[p + 1] = tFirst;
                                dataSecond[p + 1] = tSecond;
                            }
                            current--;
                        }
                        else
                            requestType[requestNumber] = emptyRequest;
                    }
                    break;
                }
            }

            for (int requestNumber = 0; requestNumber < count; requestNumber++)
            {
                switch (requestType[requestNumber])
                {
                    case 0:
                    case 1:
                    case 2:
                    {
                        map.add(request[requestNumber], answer[requestNumber]);
                    }
                    break;

                    case 3:
                    {
                        it = map.getOrderStatisticIterator((int)request[requestNumber]);
                        if (it == -1 || !map.getKeyByIterator(it).equals(answer[requestNumber]))
                        {
                            throw new ArithmeticException("AVL tree test failed. Expected: " + answer[requestNumber] + " Received: " + map.getKeyByIterator(it));
                        }
                    }
                    break;

                    case 4:
                    {
                        if (current > 0)
                        {
                            map.remove(request[requestNumber]);
                        }
                    }
                    break;
                }
            }

            it = map.getFirst();
            for (int i = 0; i < current; i++, it = map.getNext(it))
            {
                if (it == -1 || map.getKeyByIterator(it) != dataFirst[i])
                {
                    throw new ArithmeticException("AVL tree test failed. Expected: " + dataFirst[i] + " Received: " + map.getKeyByIterator(it));
                }
            }

            it = map.getLast();

            for (int i = current - 1; i >= 0; i--, it = map.getPrev(it))
            {
                if (it == -1 || map.getKeyByIterator(it) != dataFirst[i])
                {
                    throw new ArithmeticException("AVL tree test failed. Expected: " + dataFirst[i] + " Received: " + map.getKeyByIterator(it));
                }
            }
        }
    }

}
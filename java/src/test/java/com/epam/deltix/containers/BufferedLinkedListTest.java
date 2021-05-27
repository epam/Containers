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

import org.junit.Test;
import org.junit.experimental.categories.Category;

import java.util.ArrayList;

import static org.junit.Assert.assertEquals;

@Category(Test.class)
public class BufferedLinkedListTest {
    private byte[] ar = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

    @Test
    public void testAddLast() {
        BufferedLinkedList<Byte> list = new BufferedLinkedList<Byte>(3);
        for (int i = 0; i < ar.length; ++i) list.addLast(ar[i]);
        int count = 0;
        int key = list.getFirstKey();
        while (key >= 0) {
            count++;
            assertEquals((int) list.getElementByKey(key), count);
            key = list.next(key);
        }
        assertEquals(count, 10);
    }

    @Test
    public void testAddFirst() {
        BufferedLinkedList<Byte> list = new BufferedLinkedList<Byte>(3);
        for (int i = 0; i < ar.length; ++i) list.addFirst(ar[i]);
        int count = 0;
        int key = list.getLastKey();
        while (key >= 0) {
            count++;
            assertEquals((int) list.getElementByKey(key), count);
            key = list.prev(key);
        }
        assertEquals(count, 10);
    }

    @Test
    public void testAddAfter() throws Exception {
        BufferedLinkedList<Byte> list = new BufferedLinkedList<>(3);
        for (int i = 0; i < ar.length; ++i) list.addLast(ar[i]);
        list.addAfter(list.getFirstKey(), Byte.MAX_VALUE);
        int key = list.getFirstKey();
        assertEquals((int) list.getElementByKey(key), 1);
        key = list.next(key);
        assertEquals((int) list.getElementByKey(key), Byte.MAX_VALUE);
        key = list.next(key);
        int count = 1;
        while (key >= 0) {
            count++;
            assertEquals((int) list.getElementByKey(key), count);
            key = list.next(key);
        }
    }

    @Test
    public void testAddBefore() throws Exception {
        BufferedLinkedList<Byte> list = new BufferedLinkedList<>(3);
        for (int i = 0; i < ar.length; ++i) list.addFirst(ar[i]);
        list.addBefore(list.getLastKey(), Byte.MAX_VALUE);
        int key = list.getLastKey();
        assertEquals((int) list.getElementByKey(key), 1);
        key = list.prev(key);
        assertEquals((int) list.getElementByKey(key), Byte.MAX_VALUE);
        key = list.prev(key);
        int count = 1;
        while (key >= 0) {
            count++;
            assertEquals((int) list.getElementByKey(key), count);
            key = list.prev(key);
        }
    }

    @Test
    public void testRemove() throws Exception {
        BufferedLinkedList<Byte> list = new BufferedLinkedList<Byte>(3);
        ArrayList<Integer> keys = new ArrayList<Integer>();

        for (int i = 0; i < ar.length; ++i) {
            if (ar[i] % 2 == 0) {
                keys.add(list.addLast(ar[i]));
            } else list.addLast(ar[i]);
        }
        for (int i = 0; i < keys.size(); ++i) list.remove((int) keys.get(i));
        int key = list.getFirstKey();
        int count = 1;
        while (key >= 0) {
            assertEquals((int) list.getElementByKey(key), count);
            key = list.next(key);
            count += 2;
        }
        assertEquals(count, 11);
    }

    @Test
    public void testClear() {
        BufferedLinkedList<Byte> list = new BufferedLinkedList<Byte>(3);
        for (int i = 0; i < ar.length; ++i) list.addLast(ar[i]);
        list.clear();
        for (int i = 0; i < ar.length; ++i) list.addLast(ar[i]);
        int count = 0;
        int key = list.getFirstKey();
        while (key >= 0) {
            count++;
            assertEquals((int) list.getElementByKey(key), count);
            key = list.next(key);
        }
        assertEquals(count, 10);
    }


    @Test
    public void testIterator() {
        BufferedLinkedList<Integer> list = new BufferedLinkedList<>();
        for (int i = 0; i < 10; ++i) list.addLast(i);
        int count = 0;
        for (Integer x : list) {
            assertEquals(count, x.intValue());
            count++;
        }
        assertEquals(10, count);
    }
}
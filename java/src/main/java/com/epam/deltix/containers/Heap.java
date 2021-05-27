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

import java.util.*;

/**
 * Implementation of Priority Queue as Binary Heap. Top element of heap - minimum.
 *
 * @param <TValue> Type of Priority Queue elements.
 */
public class Heap<TValue> {

    Object[] elements;
    private Comparator<TValue> comparator;
    boolean isHeap = false;
    private int count = 0;
    private int listThreshold = 5;


    /**
     * Return number of elements in heap is more than this value, than heap will be real heap.
     *
     * @return Number of elements in heap is more than this value, than heap will be real heap.
     */
    @SuppressWarnings("unused")
    public int getListThreshold() {
        return listThreshold;
    }

    /**
     * Return number of elements in heap.
     *
     * @return Number of elements in heap.
     */
    public int getCount() {
        return count;
    }

    /**
     * Clear the heap.
     */
    public void clear() {
        count = 0;
    }

    /**
     * Create instance of heap.
     *
     * @param capacity     Heap's start capacity.
     * @param comparator   Comparator for heap(top is minimum).
     * @param listThreshold If number of elements in heap is more than this value, than heap will be real heap.
     */
    @SuppressWarnings("WeakerAccess")
    public Heap(int capacity, Comparator<TValue> comparator, int listThreshold) {
        this.listThreshold = listThreshold;
        this.comparator = comparator;
        elements = new Object[capacity];
    }

    /**
     * Create instance of heap.
     *
     * @param capacity   Heap's start capacity.
     * @param comparator Comparator for heap(top is minimum).
     */
    @SuppressWarnings("unused")
    public Heap(int capacity, Comparator<TValue> comparator) {
        this(capacity, comparator, 10);
    }

    /**
     * Create instance of heap.
     *
     * @param comparator Comparator for heap(top is minimum).
     */
    @SuppressWarnings("unused")
    public Heap(Comparator<TValue> comparator) {
        this(10, comparator, 10);
    }

    /**
     * Add new element to heap.
     *
     * @param value Value of element.
     */
    public void add(TValue value) {
        if (isHeap) addHeap(value);
        else addList(value);
    }

    private void addHeap(TValue value) {
        if (elements.length == count) {
            elements = Arrays.copyOf(elements, elements.length << 1);
        }
        int index = count;
        int next = (index - 1) >> 1;
        //noinspection unchecked
        while (index > 0 && comparator.compare(value, (TValue) elements[next]) < 0) {
            elements[index] = elements[next];
            index = next;
            next = (index - 1) >> 1;
        }
        elements[index] = value;
        count++;
    }

    private int binarySearch(TValue value) {
        if (count == 0) return 0;
        int l = 0;
        int r = count - 1;
        while (l < r) {
            int c = (l + r) / 2;
            //noinspection unchecked
            if (comparator.compare(value, (TValue) elements[c]) < 0) l = c + 1;
            else r = c;
        }
        //noinspection unchecked
        if (comparator.compare(value, (TValue) elements[l]) < 0) l++;
        return l;
    }

    private void rebuildToHeap() {
        for (int i = 0; i < count / 2; ++i) {
            Object q = elements[i];
            elements[i] = elements[count - 1 - i];
            elements[count - 1 - i] = q;
        }
        isHeap = true;
    }

    private void addList(TValue value) {
        if (count == elements.length) {
            elements = Arrays.copyOf(elements, elements.length << 1);
        }
        int insertIndex = binarySearch(value);
        System.arraycopy(elements, insertIndex, elements, insertIndex + 1, count - insertIndex);
        elements[insertIndex] = value;
        count++;
        if (count > listThreshold) {
            rebuildToHeap();
        }
    }


    /**
     * Remove minimum from heap.
     *
     * @return Removed minimum.
     */
    @SuppressWarnings({"unchecked", "WeakerAccess"})
    public TValue pop() throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) {
            TValue returnValue = (TValue) elements[0];
            TValue value = (TValue) elements[count - 1];
            count--;
            int current = 0;
            while (current < count) {
                int left = (current << 1) + 1;
                int right = (current << 1) + 2;
                int swapIndex = current;
                if (left >= count) swapIndex = current;
                else if (right >= count) {
                    if (comparator.compare((TValue) elements[left], value) < 0) swapIndex = left;
                } else {
                    if (comparator.compare((TValue) elements[left], (TValue) elements[right]) < 0) {
                        if (comparator.compare((TValue) elements[left], value) < 0) swapIndex = left;
                    } else if (comparator.compare((TValue) elements[right], value) < 0) swapIndex = right;
                }
                if (swapIndex == current) break;
                elements[current] = elements[swapIndex];
                current = swapIndex;
            }
            elements[current] = value;
            return returnValue;
        } else {
            count--;
            return (TValue) elements[count];
        }
    }

    /**
     * Modify top element.
     *
     * @param value New value of top element.
     * @return Old top element.
     */
    @SuppressWarnings({"unchecked", "unused"})
    public TValue modifyTop(TValue value) throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) {
            TValue returnValue = (TValue) elements[0];
            int current = 0;
            while (current < count) {
                int left = (current << 1) + 1;
                int right = (current << 1) + 2;
                int swapIndex = current;
                if (left >= count) swapIndex = current;
                else if (right >= count) {
                    if (comparator.compare((TValue) elements[left], value) < 0) swapIndex = left;
                } else {
                    if (comparator.compare((TValue) elements[left], (TValue) elements[right]) < 0) {
                        if (comparator.compare((TValue) elements[left], value) < 0) swapIndex = left;
                    } else if (comparator.compare((TValue) elements[right], value) < 0) swapIndex = right;
                }
                if (swapIndex == current) break;
                elements[current] = elements[swapIndex];
                current = swapIndex;
            }
            elements[current] = value;
            return returnValue;
        } else {
            TValue returnValue = pop();
            addList(value);
            return returnValue;
        }
    }

    /**
     * Return top element of heap.
     *
     * @return Top element of heap.
     * @throws IndexOutOfBoundsException Throw this exception if heap is empty.
     */
    @SuppressWarnings({"unchecked", "unused"})
    public TValue peek() throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) return (TValue) elements[0];
        else return (TValue) elements[count - 1];
    }


    /**
     * Return list with all values in heap.
     * @return List with all values in heap.
     */
    @SuppressWarnings("unchecked")
    public List<TValue> values() {
        ArrayList<TValue> returnList = new ArrayList<>();
        for (int i = 0; i < count; ++i) returnList.add((TValue)elements[i]);
        return returnList;
    }

    /**
      * Return true if heap is empty.
      * @return True if heap is empty.
      */
    public boolean isEmpty() {
        return count == 0;
    }
}
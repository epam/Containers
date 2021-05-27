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
package com.epam.deltix.containers.interfaces;

/**
 * Read-write interface for double-linked list.
 */
@SuppressWarnings("unused")
public interface LinkedList<T> extends LinkedListReadOnly<T> {

    /**
     * Add element to front of BufferedLinkedList. Costs O(1) time.
     * @param obj Element to add.
     * @return Key of new element.
     */
    int addFirst(T obj);

    /**
     * Add element to back of BufferedLinkedList. Costs O(1) time.
     * @param obj Element to add.
     * @return Key of new element.
     */
    int addLast(T obj);

    /**
     * Remove element from front of list. Costs O(1) time.
     */
    void removeFirst();

    /**
     * Remove last element from back of list. Costs O(1) time.
     */
    void removeLast();

    /**
     * Remove element with given key. Costs O(1) time.
     * @param key Key of element.
     */
    void remove(int key);

    /**
     * Add element to position after element with given key. Costs O(1) time.
     * @param key Given key.
     * @param obj Element to add.
     * @return Key of new element.
     */
    int addAfter(int key, T obj);

    /**
     * Add element to position before element with given key. Costs O(1) time.
     * @param key Given key.
     * @param obj Element to add.
     * @return Key of new element.
     */
    int addBefore(int key, T obj);

    /**
     * Set element with given key to new value. Costs O(1) time.
     * @param key Given key.
     * @param value Given value.
     */
    void setElementByKey(int key, T value);

    /**
     * Clear BufferedLinkedList. Remove all elements from it. Costs O(n) where is n - number of elements in list.
     */
    void clear();

}
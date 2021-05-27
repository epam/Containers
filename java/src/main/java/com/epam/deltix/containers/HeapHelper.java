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

/**
 * Helper for work with internal fields of heaps.
 */
@SuppressWarnings("unused")
public class HeapHelper {
    /**
     * Get isHeap field from heap(true if it is real heap, not sorted list).
     * @param heap Heap.
     * @return true if it is real heap, not sorted list
     */
    public static boolean isHeap(Heap heap) {
        return heap.isHeap;
    }

    /**
     * Get isHeap field from heap(true if it is real heap, not sorted list).
     * @param heap Heap.
     * @return true if it is real heap, not sorted list
     */
    public static boolean isHeap(HeapWithIndices heap) {
        return heap.isHeap;
    }

    /**
     * Get isHeap field from heap(true if it is real heap, not sorted list).
     * @param heap Heap.
     * @return true if it is real heap, not sorted list
     */
    public static boolean isHeap(HeapWithDictionary heap) {
        return heap.isHeap;
    }

    /**
     * Get isHeap field from heap(true if it is real heap, not sorted list).
     * @param heap Heap.
     * @return true if it is real heap, not sorted list
     */
    public static boolean isHeap(HeapWithAttachments heap) {
        return heap.isHeap;
    }

    /**
     * Get keys array from heap.
     * @param heap Heap.
     * @return Keys array from heap.
     */
    public static int[] getKeys(HeapWithIndices heap) {
        return heap.elementsKey;
    }

    /**
     * Get values array from heap.
     * @param heap Heap.
     * @return Values array from heap.
     */
    public static Object[] getValues(HeapWithIndices heap) {
        return heap.elementsValue;
    }

    /**
     * Get keys array from heap.
     * @param heap Heap.
     * @return Keys array from heap.
     */
    public static Object[] getKeys(HeapWithDictionary heap) {
        return heap.elementsUserKey;
    }


    /**
     * Get values array from heap.
     * @param heap Heap.
     * @return Values array from heap.
     */
    public static Object[] getValues(HeapWithDictionary heap) {
        return heap.elementsValue;
    }

    /**
     * Get attachments array from heap.
     * @param heap Heap.
     * @return Attachments array from heap.
     */
    public static Object[] getAttachments(HeapWithAttachments heap) {
        return heap.elementsAttachment;
    }


    /**
     * Get values array from heap.
     * @param heap Heap.
     * @return Values array from heap.
     */
    public static Object[] getValues(HeapWithAttachments heap) {
        return heap.elementsValue;
    }

    /**
     * Get values array from heap.
     * @param heap Heap.
     * @return Values array from heap.
     */
    public static Object[] getValues(Heap heap) {
        return heap.elements;
    }

}
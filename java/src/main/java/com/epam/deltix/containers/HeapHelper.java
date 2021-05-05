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

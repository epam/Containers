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
     */
    void setElementByKey(int key, T value);

    /**
     * Clear BufferedLinkedList. Remove all elements from it. Costs O(n) where is n - number of elements in list.
     */
    void clear();

}

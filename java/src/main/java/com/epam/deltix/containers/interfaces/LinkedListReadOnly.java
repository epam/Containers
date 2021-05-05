package com.epam.deltix.containers.interfaces;

/**
 * Read-only interface for double-linked list.
 */
@SuppressWarnings("unused")
public interface LinkedListReadOnly<T> extends Iterable<T> {
    /**
     * Return true if list contains element with given key. Costs O(1) time.
     * @param key Given key.
     * @return True if list contains element with given key.
     */
    boolean containsKey(int key);

    /**
     * Get key of element in position after element with given key. Costs O(1) time. Return negative number if key doesn't exist. Costs O(1).
     * @param key Given key.
     * @return Key of element in position after element with given key.
     */
    int next(int key);

    /**
     * Get key of element in position before element with given key. Costs O(1) time. Return negative number if key doesn't exist.
     * @param key Given key.
     * @return Key of element in position before element with given key.
     */
    int prev(int key);

    /**
     * Get key of element in position after element with given key. Costs O(1) time. Throws exception if key doesn't exists.
     * @param key Given key.
     * @return Key of element in position after element with given key.
     */
    int nextProtected(int key);

    /**
     * Get key of element in position before element with given key. Costs O(1) time. Throws exception if key doesn't exists.
     * @param key Given key.
     * @return Key of element in position before element with given key.
     */
    int prevProtected(int key);

    /**
     * Get first element of list. Costs O(1) time.
     * @return First element of list.
     */
    T getFirst();

    /**
     * Get last element of list. Costs O(1) time.
     * @return Last element of list.
     */
    T getLast();

    /**
     * Get element with given key or trash if key doesn't exist. Costs O(1) time.
     * @param key Given key.
     * @return Element with given key or trash if key doesn't exist.
     */
    T getElementByKey(int key);

    /**
     * Get key of first element in list or negative number if list is empty. Costs O(1) time.
     * @return Key of first element in list or negative number if list is empty.
     */
    int getFirstKey();

    /**
     * Get key of last element in list or negative number if list is empty. Costs O(1) time.
     * @return Key of last element in list or negative number if list is empty.
     */
    int getLastKey();

    /**
     * Return number of elements in list. Costs O(1) time.
     * @return Number of elements in list. Costs O(1) time.
     */
    int getCount();
}

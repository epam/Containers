package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.LinkedList;

import java.util.Arrays;
import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * Implementation of double-linked list. Addition element to it costs O(1) time. Removing element from it costs O(1) time.
 * Getting next/prev element costs O(1) time.
 */
public class BufferedLinkedList<T> implements LinkedList<T> {
    private Object[] buffer;
    private int[] prev;
    private int[] next;
    private int[] free;
    private int first;
    private int last;
    private int count;

    private static final int REFERENCE_TO_NOTHING = -2;

    private static final int EMPTY_LIST_POINTERS = -1;

    /**
     * Create new instance of BufferedLinkedList with default capacity.
     */
    @SuppressWarnings("unused")
    public BufferedLinkedList() {
        this(10);
    }

    /**
     * Create new instance of BufferedLinkedList.
     * @param capacity Capacity of created BufferedLinkedList.
     */
    @SuppressWarnings("WeakerAccess")
    public BufferedLinkedList(int capacity) {
        buffer = new Object[capacity];
        prev = new int[capacity];
        next = new int[capacity];
        free = new int[capacity];
        for (int i = 0; i < capacity; ++i) {
            free[i] = i;
            prev[i] = REFERENCE_TO_NOTHING;
        }
        first = EMPTY_LIST_POINTERS;
        last = EMPTY_LIST_POINTERS;
        count = 0;
    }

    private void increaseSize() {
        int prevSize = buffer.length;
        int newSize = prevSize << 1;
        buffer = Arrays.copyOf(buffer, newSize);
        prev = Arrays.copyOf(prev, newSize);
        next = Arrays.copyOf(next, newSize);
        free = Arrays.copyOf(free, newSize);
        for (int i = prevSize; i < newSize; ++i) {
            free[i] = i;
            prev[i] = REFERENCE_TO_NOTHING;
        }
    }

    /**
     * Add element to front of BufferedLinkedList. Costs O(1) time.
     * @param obj Element to add.
     * @return Key of new element.
     */
    @Override
    public int addFirst(T obj) {
        if (count == buffer.length)
            increaseSize();

        int index = free[count++];
        buffer[index] = obj;
        next[index] = first;
        prev[index] = EMPTY_LIST_POINTERS;
        if (first != EMPTY_LIST_POINTERS)
            prev[first] = index;
        first = index;
        if (last == EMPTY_LIST_POINTERS)
            last = index;
        return index;
    }

    /**
     * Add element to back of BufferedLinkedList. Costs O(1) time.
     * @param obj Element to add.
     * @return Key of new element.
     */
    @Override
    public int addLast(T obj) {
        if (count == buffer.length)
            increaseSize();

        int index = free[count++];
        buffer[index] = obj;
        next[index] = EMPTY_LIST_POINTERS;
        prev[index] = last;
        if (last != EMPTY_LIST_POINTERS)
            next[last] = index;
        last = index;
        if (first == EMPTY_LIST_POINTERS)
            first = index;

        return index;
    }

    /**
     * Remove element from front of list. Costs O(1) time.
     */
    @Override
    public void removeFirst() {
        if (first != EMPTY_LIST_POINTERS) {
            int newFirst = next[first];
            prev[first] = REFERENCE_TO_NOTHING;
            free[--count] = first;

            if (newFirst != EMPTY_LIST_POINTERS)
                prev[newFirst] = EMPTY_LIST_POINTERS;
            first = newFirst;

            if (count == 0)
                last = EMPTY_LIST_POINTERS;
        }
    }

    /**
     * Remove last element from back of list. Costs O(1) time.
     */
    @Override
    public void removeLast() {
        if (last != EMPTY_LIST_POINTERS) {
            int newLast = prev[last];
            prev[last] = REFERENCE_TO_NOTHING;
            free[--count] = last;
            if (newLast != EMPTY_LIST_POINTERS)
                next[newLast] = EMPTY_LIST_POINTERS;
            last = newLast;
            if (count == 0)
                first = EMPTY_LIST_POINTERS;
        }
    }

    /**
     * Remove element with given key. Costs O(1) time.
     * @param key Key of element.
     */
    @Override
    public void remove(int key) {
        if (prev[key] == REFERENCE_TO_NOTHING)
            throw new NoSuchElementException("There is no such element with given key!");

        free[--count] = key;
        int prevElem = prev[key];
        int nextElem = next[key];

        if (prevElem != EMPTY_LIST_POINTERS)
            next[prevElem] = nextElem;
        else
            first = nextElem;

        if (nextElem != EMPTY_LIST_POINTERS)
            prev[nextElem] = prevElem;
        else
            last = prevElem;

        prev[key] = REFERENCE_TO_NOTHING;

    }

    /**
     * Add element to position after element with given key. Costs O(1) time.
     * @param key Given key.
     * @param obj Element to add.
     * @return Key of new element.
     */
    @Override
    public int addAfter(int key, T obj) {
        if (prev[key] == REFERENCE_TO_NOTHING)
            throw new NoSuchElementException("There is no such element with given key!");
        if (count == buffer.length)
            increaseSize();
        int index = free[count++];
        buffer[index] = obj;
        int nextElem = next[index] = next[key];
        prev[index] = key;
        next[key] = index;
        if (nextElem != EMPTY_LIST_POINTERS)
            prev[nextElem] = index;
        else
            last = index;
        return index;
    }

    /**
     * Add element to position before element with given key. Costs O(1) time.
     * @param key Given key.
     * @param obj Element to add.
     * @return Key of new element.
     */
    @Override
    public int addBefore(int key, T obj) {
        if (prev[key] == REFERENCE_TO_NOTHING)
            throw new NoSuchElementException("There is no such element with given key!");

        if (count == buffer.length)
            increaseSize();

        int index = free[count++];
        buffer[index] = obj;

        int prevElem = prev[index] = prev[key];
        next[index] = key;
        prev[key] = index;

        if (prevElem != EMPTY_LIST_POINTERS)
            next[prevElem] = index;
        else
            first = index;

        return index;
    }

    /**
     * Get key of element in position before element with given key. Costs O(1) time. Throws exception if key doesn't exists.
     * @param key Given key.
     * @return Key of element in position before element with given key.
     */
    @Override
    public int prevProtected(int key)  {
        if (key == EMPTY_LIST_POINTERS)
            return last;

        if (prev[key] == REFERENCE_TO_NOTHING)
            throw new NoSuchElementException("There is no such element with given key!");

        return prev[key];
    }

    /**
     * Get key of element in position after element with given key. Costs O(1) time. Throws exception if key doesn't exists.
     * @param key Given key.
     * @return Key of element in position after element with given key.
     */
    @Override
    public int nextProtected(int key) {
        if (key == EMPTY_LIST_POINTERS)
            return first;

        if (prev[key] == REFERENCE_TO_NOTHING)
            throw new NoSuchElementException("There is no such element with given key!");

        return next[key];
    }

    /**
     * Return true if list contains element with given key. Costs O(1) time.
     * @param key Given key.
     * @return True if list contains element with given key.
     */
    @Override
    public boolean containsKey(int key) {
        return key >= 0 && key < buffer.length && prev[key] != REFERENCE_TO_NOTHING;
    }

    /**
     * Get key of element in position before element with given key. Costs O(1) time. Return negative number if key doesn't exist.
     * @param key Given key.
     * @return Key of element in position before element with given key.
     */
    @Override
    public int prev(int key) {
        return prev[key];
    }

    /**
     * Get key of element in position after element with given key. Costs O(1) time. Return negative number if key doesn't exist. Costs O(1).
     * @param key Given key.
     * @return Key of element in position after element with given key.
     */
    @Override
    public int next(int key) {
        return next[key];
    }

    /**
     * Get key of first element in list or negative number if list is empty. Costs O(1) time.
     * @return Key of first element in list or negative number if list is empty.
     */
    @Override
    public int getFirstKey() {
        return first;
    }


    /**
     * Get key of last element in list or negative number if list is empty. Costs O(1) time.
     * @return Key of last element in list or negative number if list is empty.
     */
    @Override
    public int getLastKey() {
        return last;
    }

    /**
     * Get first element of list. Costs O(1) time.
     * @return First element of list.
     */
    @SuppressWarnings("unchecked")
    @Override
    public T getFirst() {
        return (T) buffer[first];
    }

    /**
     * Get last element of list. Costs O(1) time.
     * @return Last element of list.
     */
    @SuppressWarnings("unckecked")
    @Override
    public T getLast() {
        return (T) buffer[last];
    }

    /**
     * Get element with given key or trash if key doesn't exist. Costs O(1) time.
     * @param key Given key.
     * @return Element with given key or trash if key doesn't exist.
     */
    @SuppressWarnings("unchecked")
    @Override
    public T getElementByKey(int key) {
        return (T) buffer[key];
    }


    /**
     * Set element with given key to new value. Costs O(1) time.
     * @param key Given key.
     */
    @Override
    public void setElementByKey(int key, T value) {
        buffer[key] = value;
    }

    /**
     * Return number of elements in list. Costs O(1) time.
     * @return Number of elements in list. Costs O(1) time.
     */
    @Override
    public int getCount() {
        return count;
    }

    /**
     * Clear BufferedLinkedList. Remove all elements from it. Costs O(n) where is n - number of elements in list.
     */
    @Override
    public void clear() {
        count = 0;
        first = last = EMPTY_LIST_POINTERS;

        for (int i = 0; i < buffer.length; i++) {
            free[i] = i;
            prev[i] = REFERENCE_TO_NOTHING;
        }
    }

    /**
     * Returns an iterator over elements of type {@code T}.
     *
     * @return an Iterator.
     */
    @Override
    public Iterator<T> iterator() {
        return new BufferedLinkedListIterator<>(this);
    }

    /**
     * Return true if list is empty.
     * @return True if list is empty.
     */
    public boolean isEmpty() {
        return count == 0;
    }
}

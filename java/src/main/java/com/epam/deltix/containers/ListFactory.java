package com.epam.deltix.containers;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.SortedSet;

/**
 * Factory of generic lists. It does'nt give you direct access to lists. This collection just gives you API for work with lists.
 * The main feature of this collection - common memory pool for all lists and elements.
 * <p>
 * <p>
 * This collection provides functionallity to register (and unregister) new linked lists(methods {@link #createList} and {@link #deleteList} ).
 * {@link #createList} returns listId (it's necessary to work with list) of new list to you. This method takes O(1) time.
 * {@link #deleteList} remove all elements of list and unregister list. This method takes O(n) time, where n - number of elements in list.
 * To work with a concrete list you need to remember listId of it. Any element of lists has unique elementId.
 * ListId allows you to get elementId of first/last element ({@link #getFirst}/{@link #getLast}) of list
 * and count of elements in it ({@link #getCount}). It also allows you to insert new elements to head and to tale of list ({@link #addFirst}/{@link #addLast}).
 * All of this operations take O(1) time.
 * All operations with a concrete list identical to operations with <tt>BufferedLinkedList</tt>.
 * You can get elementId of next/previous element   ({@link #getNext}/{@link #getPrevious}), value by elementId ({@link #get}),
 * insert new element before/after it ({@link #addBefore}/{@link #addAfter}), remove it ({@link #deleteElement}. All of it takes O(1) time.
 *
 * @param <T> Type of elements of lists.
 */
public class ListFactory<T> {
    /**
     * No element constant.
     */
    public static final int NO_ELEMENT = -1;
    private final Object LIST_OBJECT = new Object();

    private int[] previousOrFirst;
    private int[] nextOrLast;
    private int[] listOrCount;
    private Object[] data;
    private int free = NO_ELEMENT;


    /**
     * Create new factory with capacity.
     *
     * @param capacity Capacity of lists.
     */
    public ListFactory(int capacity) {
        resize(capacity);
    }

    /**
     * Create new factory with default capacity.
     */
    public ListFactory() {
        resize(4);
    }

    private void resize() {
        resize(data.length << 1);
    }

    private void resize(int capacity) {
        int currentCapacity;
        int newCapacity;
        if (data == null) {
            currentCapacity = 0;
            newCapacity = 4;
        } else {
            currentCapacity = newCapacity = data.length;
        }
        while (newCapacity < capacity)
            newCapacity <<= 1;

        if (data == null) {
            previousOrFirst = new int[newCapacity];
            listOrCount = new int[newCapacity];
            nextOrLast = new int[newCapacity];
            data = new Object[newCapacity];

        } else {
            previousOrFirst = Arrays.copyOf(previousOrFirst, newCapacity);
            listOrCount = Arrays.copyOf(listOrCount, newCapacity);
            nextOrLast = Arrays.copyOf(nextOrLast, newCapacity);
            data = Arrays.copyOf(data, newCapacity);

        }

        for (int i = newCapacity - 1; i >= currentCapacity; --i)
            removeToPool(i);
    }

    private void removeToPool(int index) {
        data[index] = null;
        nextOrLast[index] = free;
        listOrCount[index] = NO_ELEMENT;

        free = index;
    }

    private void checkElementContract(int elementId) {
        if (listOrCount[elementId] == NO_ELEMENT) {
            throw new IllegalStateException("Id " + elementId + " is empty.");
        }

        if (data[elementId] == LIST_OBJECT) {
            throw new IllegalStateException("Id " + elementId + " is a list");
        }
    }

    private void checkListContract(int listId) {
        if (listOrCount[listId] == NO_ELEMENT) {
            throw new IllegalStateException("Id " + listId + " is empty.");
        }

        if (data[listId] != LIST_OBJECT) {
            throw new IllegalStateException("Id " + listId + " is not a list");
        }
    }

    /**
     * Create new list.
     *
     * @return If od new list.
     */
    public int createList() {

        if (free == -1)
            resize();
        int index = free;
        free = nextOrLast[free];
        data[index] = LIST_OBJECT;
        nextOrLast[index] = NO_ELEMENT;
        previousOrFirst[index] = NO_ELEMENT;
        listOrCount[index] = 0;
        return index;
    }

    /**
     * Insert element to position before element with id.
     *
     * @param beforeId Id of element after inserted.
     * @param value    Inserted value.
     * @return Id of inserted element.
     */
    public int addBefore(int beforeId, T value) {
        checkElementContract(beforeId);
        if (free == -1)
            resize();

        int listId = listOrCount[beforeId];
        int index = free;
        free = nextOrLast[free];

        data[index] = value;
        listOrCount[index] = listId;

        nextOrLast[index] = beforeId;
        previousOrFirst[index] = previousOrFirst[beforeId];

        if (previousOrFirst[beforeId] != NO_ELEMENT)
            nextOrLast[previousOrFirst[beforeId]] = index;
        previousOrFirst[beforeId] = index;

        if (previousOrFirst[listId] == beforeId)
            previousOrFirst[listId] = index;
        ++listOrCount[listId];

        return index;
    }

    /**
     * Insert element to position after element with id.
     *
     * @param afterId Id of element before inserted.
     * @param value   Inserted value.
     * @return Id of inserted element.
     */
    public int addAfter(int afterId, T value) {
        checkElementContract(afterId);
        if (free == -1)
            resize();

        int listId = listOrCount[afterId];
        int index = free;
        free = nextOrLast[free];

        data[index] = value;
        listOrCount[index] = listId;

        previousOrFirst[index] = afterId;
        nextOrLast[index] = nextOrLast[afterId];

        if (nextOrLast[afterId] != NO_ELEMENT)
            previousOrFirst[nextOrLast[afterId]] = index;
        nextOrLast[afterId] = index;

        if (nextOrLast[listId] == afterId)
            nextOrLast[listId] = index;
        ++listOrCount[listId];

        return index;
    }

    /**
     * Add first element to list with id.
     *
     * @param listId Id of list.
     * @param value  Value of inserted element.
     * @return Id of inserted element.
     */
    public int addFirst(int listId, T value) {
        checkListContract(listId);
        if (free == -1)
            resize();

        int index = free;
        free = nextOrLast[free];

        data[index] = value;
        nextOrLast[index] = previousOrFirst[listId];
        previousOrFirst[index] = NO_ELEMENT;
        listOrCount[index] = listId;
        if (nextOrLast[listId] == NO_ELEMENT)
            nextOrLast[listId] = index;
        else
            previousOrFirst[previousOrFirst[listId]] = index;
        previousOrFirst[listId] = index;
        ++listOrCount[listId];

        return index;
    }

    /**
     * Add last element to list with id.
     *
     * @param listId Id of list.
     * @param value  Value of inserted element.
     * @return Id of inserted element.
     */
    public int addLast(int listId, T value) {
        checkListContract(listId);
        if (free == -1)
            resize();

        int index = free;
        free = nextOrLast[free];

        data[index] = value;
        nextOrLast[index] = NO_ELEMENT;
        previousOrFirst[index] = nextOrLast[listId];
        listOrCount[index] = listId;
        if (previousOrFirst[listId] == NO_ELEMENT)
            previousOrFirst[listId] = index;
        else
            nextOrLast[nextOrLast[listId]] = index;
        nextOrLast[listId] = index;
        ++listOrCount[listId];

        return index;
    }

    /**
     * Remove element with id from list factory.
     *
     * @param elementId Id of element.
     * @return Removed element.
     */
    public T deleteElement(int elementId) {
        checkElementContract(elementId);

        Object d = data[elementId];
        int listId = listOrCount[elementId];
        --listOrCount[listId];
        if (previousOrFirst[listId] == elementId)
            previousOrFirst[listId] = nextOrLast[elementId];
        else {
            nextOrLast[previousOrFirst[elementId]] = nextOrLast[elementId];
        }

        if (nextOrLast[listId] == elementId)
            nextOrLast[listId] = previousOrFirst[elementId];
        else {
            previousOrFirst[nextOrLast[elementId]] = previousOrFirst[elementId];
        }

        removeToPool(elementId);
        return (T) d;
    }

    /**
     * Convert list with id to array.
     *
     * @param listId Id of list.
     * @return Array with data from list.
     */
    public T[] toArray(int listId) {
        checkListContract(listId);
        Object[] array = new Object[listOrCount[listId]];
        for (int i = previousOrFirst[listId], j = 0; i != NO_ELEMENT; i = nextOrLast[i], ++j) {
            array[j] = data[i];
        }

        return (T[]) array;
    }

    /**
     * Get id of first element of list.
     *
     * @param listId Id of list.
     * @return Id of first element of list.
     */
    public int getFirst(int listId) {
        checkListContract(listId);
        return previousOrFirst[listId];
    }

    /**
     * Get id of last element of list.
     *
     * @param listId Id of list.
     * @return Id of first element of list.
     */
    public int getLast(int listId) {
        checkListContract(listId);
        return nextOrLast[listId];
    }

    /**
     * Get count of list.
     *
     * @param listId Id of list.
     * @return Count of list.
     */
    public int getCount(int listId) {
        checkListContract(listId);
        return listOrCount[listId];
    }

    /**
     * Get element by id.
     *
     * @param elementId Id of element.
     * @return Element with id.
     */
    public T get(int elementId) {
        checkElementContract(elementId);
        return (T) data[elementId];
    }

    /**
     * Set element by id.
     *
     * @param elementId Id of element.
     * @param value     New value of element.
     * @return Old value of element.
     */
    public T set(int elementId, T value) {
        checkElementContract(elementId);
        Object d = data[elementId];
        data[elementId] = value;
        return (T) d;
    }

    /**
     * Get id of next element follows element with id.
     *
     * @param elementId Id of element.
     * @return Id of element follows element with id.
     */
    public int getNext(int elementId) {
        checkElementContract(elementId);
        return nextOrLast[elementId];
    }

    /**
     * Get id of previous element.
     *
     * @param elementId Id of element.
     * @return Id of previous element.
     */
    public int getPrevious(int elementId) {
        checkElementContract(elementId);
        return previousOrFirst[elementId];
    }

    /**
     * Delete list from listFactory;
     *
     * @param listId
     */
    public void deleteList(int listId) {
        checkListContract(listId);
        int next;
        for (int i = previousOrFirst[listId], j = 0; i != NO_ELEMENT; i = next, ++j) {
            next = nextOrLast[i];
            removeToPool(i);
        }
        removeToPool(listId);
    }
}

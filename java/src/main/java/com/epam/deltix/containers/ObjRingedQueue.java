package com.epam.deltix.containers;

import java.util.Iterator;

/**
 * Class for ringed queue
 * @param <T> type of elements.
 */
public class ObjRingedQueue<T> implements Iterable<T> {
    private Object[] array;
    private int first, size;

    /**
     * Create new instance of ringed queue with default capacity
     */
    public ObjRingedQueue() {
        this(0x10);
    }

    /**
     * Create new instance of ringed queue with custom capacity.
     * @param capacity Capacity of ringed queue.
     */
    public ObjRingedQueue(int capacity) {
        array = new Object[capacity];
        first = size = 0;
    }


    /**
     * Get element of ringed queue with given index.
     * @param index Index of element
     * @return element of ringed queue with given index.
     */
    public T get(int index) {

        if (index > size)
            throw new IndexOutOfBoundsException();

        index += first;
        index = index >= array.length ? index - array.length : index;
        return (T)array[index];
    }



    /**
     * Set element of ringed queue with given index.
     * @param index Index of element
     * @param element Element to set
     */
    public void set(int index, T element) {

        if (index > size)
            throw new IndexOutOfBoundsException();

        index += first;
        index = index >= array.length ? index - array.length : index;
        array[index] = element;
    }

    /**
     * Add new element to queue
     * @param x element to add
     */
    public void add(T x) {
        if (size == array.length) {
            extend();
        }
        int index = first + size++;
        index = index >= array.length ? index - array.length : index;
        array[index] = x;
    }

    /**
     * Return first element of queue
     * @return first element of queue
     */
    public T first() {
        return (T)array[first];
    }

    /**
     * Return last element of queue
     * @return last element of queue
     */
    public T last() {
        return get(size - 1);
    }

    /**
     * Return first element and remove it.
     * @return first element of queue
     */
    public T pop() {
        T res = (T)array[first++];
        first = first >= array.length ? first - array.length : first;
        --size;

        return res;
    }

    private void extend() {
        Object[] newArray = new Object[array.length << 1];

        System.arraycopy(array, first, newArray, 0, array.length - first);
        if (first > 0)
            System.arraycopy(array, 0, newArray, array.length - first, first);
        first = 0;
        array = newArray;
    }

    /**
     * Clear queue
     */
    public void clear() {
        first = size= 0;
    }

    /**
     * Return size of queue
     * @return
     */
    public int size() {
        return size;
    }

    /**
     * Record all elements of queue to array.
     * @return Array with all elements of queue
     */
    public Object[] toArray() {
        Object[] result = new Object[size];
        toArray(result, 0);
        return result;
    }

    /**
     * Record all elements of queue to array with offset
     * @param data Array with all elements to queue
     * @param offset Array offset.
     */
    public void toArray(Object[] data, int offset) {
        if (size > data.length - offset)
            throw new IllegalArgumentException();

        for (int i = 0, j = first; i < size; ++i) {
            data[i] = array[j++];
            j = j == array.length ? 0 : j;
        }
    }

    /**
     * Returns an iterator over elements of type {@code T}.
     *
     * @return an Iterator.
     */
    @Override
    public Iterator<T> iterator() {

        return new Iterator<T>() {
            int offset = 0;
            @Override
            public boolean hasNext() {
                return offset < size();
            }

            @Override
            public T next() {
                offset++;
                return (T)array[(first + offset - 1) % array.length];

            }

            /**
             * Removes from the underlying collection the last element returned
             * by this iterator (optional operation).  This method can be called
             * only once per call to {@link #next}.  The behavior of an iterator
             * is unspecified if the underlying collection is modified while the
             * iteration is in progress in any way other than by calling this
             * method.
             *
             * @throws UnsupportedOperationException if the {@code remove}
             *                                       operation is not supported by this iterator
             * @throws IllegalStateException         if the {@code next} method has not
             *                                       yet been called, or the {@code remove} method has already
             *                                       been called after the last call to the {@code next}
             *                                       method
             * @implSpec The default implementation throws an instance of
             * {@link UnsupportedOperationException} and performs no other action.
             */
            @Override
            public void remove() {
                throw new UnsupportedOperationException();
            }
        };
    }

    /**
     * Return true if queue is empty.
     * @return True if queue is empty.
     */
    public boolean isEmpty() {
        return size == 0;
    }
}


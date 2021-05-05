package com.epam.deltix.containers;

import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * Created by DriapkoA on 02.02.2017.
 */
class BufferedLinkedListIterator<T> implements Iterator<T> {

    BufferedLinkedList<T> list;
    int key;

    BufferedLinkedListIterator(BufferedLinkedList<T> list) {
        this.list = list;
        key = -1;
    }

    /**
     * Returns {@code true} if the iteration has more elements.
     * (In other words, returns {@code true} if {@link #next} would
     * return an element rather than throwing an exception.)
     *
     * @return {@code true} if the iteration has more elements
     */
    @Override
    public boolean hasNext() {
        if (key == -1) return list.getCount() > 0;
        else return list.next(key) >= 0;
    }

    /**
     * Returns the next element in the iteration.
     *
     * @return the next element in the iteration
     * @throws NoSuchElementException if the iteration has no more elements
     */
    @Override
    public T next() {
        if (key == -1) {
            if (list.getCount() > 0) {
                key = list.getFirstKey();
                return list.getElementByKey(key);
            } else throw new NoSuchElementException("No more elements in BufferedLinkedList");
        } else {
            if (list.next(key) < 0) throw new NoSuchElementException("No more elements in BufferedLinkedList");
            key = list.next(key);
            return list.getElementByKey(key);
        }
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
        throw new UnsupportedOperationException("We don't support operation remove in BufferedLinkedListIterator");
    }
}

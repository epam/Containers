package com.epam.deltix.containers.interfaces;

import java.util.ArrayList;

/**
 * ReadOnly interface for ArrayList.
 */
public interface ListReadOnly<T> {
    /**
     * Returns the number of elements in this list.
     *
     * @return The number of elements in this list.
     */

    public int size();


    /**
     * Returns the element at the specified position in this list.
     *
     * @param index Index of the element to return
     * @return The element at the specified position in this list.
     * @throws IndexOutOfBoundsException If the index is out of range
     *                                   (<tt>index &lt; 0 || index &gt;= size()</tt>)
     */
    public T get(int index);
}

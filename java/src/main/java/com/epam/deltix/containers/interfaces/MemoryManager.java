package com.epam.deltix.containers.interfaces;

/**
 * Memory pool of objects with T type interface.
 * @param <T> Type of objects.
 */
public interface MemoryManager<T> {
    /**
     * Interface for creating new objects of T type
     * @param <T> Type of objects
     */
    interface Creator<T> {
        /**
         * Create new object and return it.
         * @return Created object.
         */
        T create();

        /**
         * Create array of objects of T type
         * @param size Size of array.
         * @return Created array.
         */
        T[] create(int size);
    }

    /**
     * Return number of free objects in pool
     * @return Number of free objects in pool
     */
    int getFreeObjectsCount();

    /**
     * Return total number of objects from pool
     * @return Total number of objects from pool
     */
    int getTotalObjectsCount();

    /**
     * Return new free object from pool
     * @return New free object from pool
     */
    T getNew();

    /**
     * Return object to pool (make it free)
     * @param obj Object to pool.
     */
    void delete(T obj);
}

package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.MemoryManager;

/**
 * Memory pool of objects with T type interface. It grows in arithmetic progression
 * @param <T> Type of objects
 */
public class APMemoryManager<T> implements MemoryManager<T> {
    /**
     * The value of initial capacity that will be used if not provided.
     */
    @SuppressWarnings("WeakerAccess")
    public static final int DEFAULT_INITIAL_CAPACITY = 16;

    /**
     * The value of growth factor that will be used if not provided.
     */
    @SuppressWarnings("WeakerAccess")
    public static final int DEFAULT_INCREMENT = 16;

    private final Creator<T> creator;
    private final int initialCapacity;
    private final int increment;

    private T[] freeObjects = null;
    private int numberOfFreeObjects = 0;
    private int totalObjectsCount = 0;

    /**
     * Create new instance of MemoryManager.
     * @param creator Objects creator
     * @param initialCapacity Initial capacity of pool
     * @param increment Growth arithmetic progression increment.
     */
    @SuppressWarnings("WeakerAccess")
    public APMemoryManager(MemoryManager.Creator<T> creator, int initialCapacity, int increment) {
        if (creator == null)
            throw new IllegalArgumentException("Argument 'creator' cannot be null.");
        if (increment <= 0)
            throw new IllegalArgumentException("Argument 'increment' should be greater than 0.");
        if (initialCapacity < 0)
            throw new IllegalArgumentException("Argument 'initialCapacity' cannot be negative.");

        this.creator = creator;
        this.initialCapacity = initialCapacity;
        this.increment = increment;
    }

    /**
     * Create new instance of MemoryManager.
     * @param creator Objects creator.
     */
    @SuppressWarnings("WeakerAccess")
    public APMemoryManager(MemoryManager.Creator<T> creator) {
        this(creator, DEFAULT_INITIAL_CAPACITY, DEFAULT_INCREMENT);
    }


    /**
     * Create new instance of MemoryManager.
     * @param clazz Objects class.
     * @param initialCapacity Initial capacity.
     * @param increment Growth arithmetic progression increment.
     */
    @SuppressWarnings("WeakerAccess")
    public APMemoryManager(Class<T> clazz, int initialCapacity, int increment) {
        this(new ReflectionObjectCreator<>(clazz), initialCapacity, increment);
    }

    /**
     * Create new instance of MemoryManager.
     * @param clazz Objects class.
     */
    @SuppressWarnings({"WeakerAccess", "unused"})
    public APMemoryManager(Class<T> clazz) {
        this(clazz, DEFAULT_INITIAL_CAPACITY, DEFAULT_INCREMENT);
    }

    /**
     * Return number of free objects in pool
     * @return Number of free objects in pool
     */
    @Override
    public int getFreeObjectsCount() {
        return numberOfFreeObjects;
    }

    /**
     * Return total number of objects from pool
     * @return Total number of objects from pool
     */
    @Override
    public int getTotalObjectsCount() {
        return totalObjectsCount;
    }

    /**
     * Return new free object from pool
     * @return New free object from pool
     */
    @Override
    public T getNew() {
        if (numberOfFreeObjects == 0) {
            numberOfFreeObjects = initialCapacity > increment ? initialCapacity : increment;
            if (freeObjects == null)
                freeObjects = creator.create(numberOfFreeObjects);
            for (int i = 0; i < numberOfFreeObjects; i += 1)
                freeObjects[i] = creator.create();
            totalObjectsCount += numberOfFreeObjects;
        }
        numberOfFreeObjects -= 1;
        return freeObjects[numberOfFreeObjects];
    }

    /**
     * Return object to pool (make it free)
     * @param obj Object to pool.
     */
    @Override
    public void delete(T obj) {
        if (obj == null) throw new NullPointerException("Try to return null to MemoryManager");
        if (freeObjects == null)
            freeObjects = creator.create(Math.max(increment, initialCapacity));
        if (numberOfFreeObjects == freeObjects.length) {
            final T[] newFreeObjects = creator.create(freeObjects.length * 2);
            System.arraycopy(freeObjects, 0, newFreeObjects, 0, freeObjects.length);
            freeObjects = newFreeObjects;
        }
        freeObjects[numberOfFreeObjects] = obj;
        numberOfFreeObjects += 1;
    }
}

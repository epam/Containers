package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.MemoryManager;

/**
 * Memory pool of objects with T type interface. It grows in geometric progression.
 * @param <T> Type of objects
 */
@SuppressWarnings("unused")
public class GPMemoryManager<T> implements MemoryManager<T> {
    /**
     * The value of initial capacity that will be used if not provided.
     */
    @SuppressWarnings("WeakerAccess")
    public static final int DEFAULT_INITIAL_CAPACITY = 4;

    /**
     * Default maximal value of number object increment
     */
    public static final int DEFAULT_MAXIMAL_INCREMENT = 1000000;


    private int maxIncrement = 1000000;

    /**
     * The value of growth factor that will be used if not provided.
     */
    @SuppressWarnings("WeakerAccess")
    public static final double DEFAULT_GROWTH_FACTOR = 2.0;

    private final Creator<T> creator;
    private final int initialCapacity;
    private final double growthFactor;

    private T[] freeObjects = null;
    private int numberOfFreeObjects = 0;

    /**
     * Create new instance of MemoryManager.
     * @param creator Objects creator
     * @param initialCapacity Initial capacity of pool
     * @param growthFactor Growth geometric progression factor.
     */
    @SuppressWarnings("WeakerAccess")
    public GPMemoryManager(MemoryManager.Creator<T> creator, int initialCapacity, double growthFactor) {
        if (creator == null)
            throw new IllegalArgumentException("Argument 'creator' cannot be null.");
        if (growthFactor <= 1.0)
            throw new IllegalArgumentException("Argument 'growthFactor' should be greater than 1.0.");
        if (initialCapacity < 0)
            throw new IllegalArgumentException("Argument 'initialCapacity' cannot be negative.");

        this.creator = creator;
        this.initialCapacity = initialCapacity;
        this.growthFactor = growthFactor;
    }

    /**
     * Create new instance of MemoryManager.
     * @param creator Objects creator.
     */
    @SuppressWarnings("unused")
    public GPMemoryManager(MemoryManager.Creator<T> creator) {
        this(creator, DEFAULT_INITIAL_CAPACITY, DEFAULT_GROWTH_FACTOR);
    }

    /**
     * Set maximal increment to new value.
     * @param maxIncrement Maximal increment.
     */
    public void setMaxIncrement(int maxIncrement) {
        this.maxIncrement = maxIncrement;
    }

    /**
     * Create new instance of MemoryManager.
     * @param clazz Objects class.
     * @param initialCapacity Initial capacity.
     * @param growthFactor Growth geometric progression factor.
     */
    @SuppressWarnings("WeakerAccess")
    public GPMemoryManager(Class<T> clazz, int initialCapacity, double growthFactor) {
        this(new ReflectionObjectCreator<>(clazz), initialCapacity, growthFactor);
    }

    /**
     * Create new instance of MemoryManager.
     * @param clazz Objects class.
     */
    @SuppressWarnings("WeakerAccess")
    public GPMemoryManager(Class<T> clazz) {
        this(clazz, DEFAULT_INITIAL_CAPACITY, DEFAULT_GROWTH_FACTOR);
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
        return freeObjects.length;
    }

    /**
     * Return new free object from pool
     * @return New free object from pool
     */
    @Override
    public T getNew() {
        if (numberOfFreeObjects == 0) {
            final int capacity = freeObjects != null ? freeObjects.length : 0;
            final int newCapacity = Math.min(maxIncrement, capacity > 0 ? (int) (capacity * growthFactor) : initialCapacity);
            freeObjects = creator.create(newCapacity);
            numberOfFreeObjects = newCapacity;
            for (int i = 0; i < numberOfFreeObjects; i += 1)
                freeObjects[i] = creator.create();
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
        if (obj == null) throw new NullPointerException("Try to return null to MemoryManager.");
        if (freeObjects == null)
            freeObjects = creator.create(initialCapacity);
        if (numberOfFreeObjects == freeObjects.length) {
            final int capacity = (int) (freeObjects.length * growthFactor);
            final T[] newFreeObjects = creator.create(capacity);
            System.arraycopy(freeObjects, 0, newFreeObjects, 0, freeObjects.length);
            freeObjects = newFreeObjects;
        }
        freeObjects[numberOfFreeObjects] = obj;
        numberOfFreeObjects += 1;
    }
}

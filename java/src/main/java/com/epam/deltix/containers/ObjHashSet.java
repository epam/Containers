package com.epam.deltix.containers;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * Public class for HashSet. Key is Object.
 */
public class ObjHashSet<K> implements Iterable<K> {
    /**
     * Pointer to empty element.
     */
    public static final int NO_ELEMENT = -1;

    ArrayList<K> keys;
    int[] first;
    int[] next;

    boolean allocedPlaceWasFilled = true;
    int head;
    int capacity;
    int count = 0;
    long reservedSpace = NO_ELEMENT;

    /**
     * Get reserved by locateOrReserve empty space.
     *
     * @return Reserved by locateOrReserve empty space.
     */
    public long getReservedSpace() {
        return reservedSpace;
    }


    int hashFunction(K value) {
        return Math.abs(value.hashCode() % capacity);
    }


    /**
     * Return number of elements in HashSet.
     *
     * @return Number of elements in HashSet.
     */
    public int size() {
        return count;
    }

    /**
     * Return capacity of HashSet.
     *
     * @return Capacity of HashSet.
     */
    public int getCapacity() {
        return capacity;
    }


    /**
     * Increase capacity of this HashSet to new value. This method ignores attempts to decrease capacity.
     * @param newCapacity New capacity of HashSet.
     */
    public void setCapacity(int newCapacity) {
        if (capacity >= newCapacity) return;
        rebuild(newCapacity);
    }

    /**
     * Create instance of HashSet.
     *
     * @param startCapacity Start capacity of HashSet.
     * @throws IllegalArgumentException If capacity is not positive
     */
    public ObjHashSet(int startCapacity) {
        if (startCapacity <= 0)
            throw new IllegalArgumentException("Capacity must be positive");
        keys = new ArrayList<K>(startCapacity);
        for (int i = 0; i < startCapacity; ++i) {
            keys.add(null);
        }
        first = new int[startCapacity];
        next = new int[startCapacity];
        for (int i = 0; i < startCapacity; ++i) {
            next[i] = -i - 2;
        }
        for (int i = 0; i < startCapacity; ++i) {
            first[i] = -1;
        }
        capacity = startCapacity;
    }

    /**
     * Create instance of HashSet.
     */
    public ObjHashSet() {
        this(8);
    }

    void rebuild(int newCapacity) {
        if (newCapacity <= 0)
            throw new IllegalArgumentException("Capacity must be positive");
        int oldCapacity = capacity;
        capacity = newCapacity;
        while (keys.size() < capacity) {
            keys.add(null);
        }
        first = Arrays.copyOf(first, capacity);
        next = Arrays.copyOf(next, capacity);
        for (int i = 0; i < capacity; ++i) {
            next[i] = -i - 2;
        }
        for (int i = 0; i < capacity; ++i) {
            first[i] = -1;
        }
        head = 0;
        count = 0;
        for (int i = 0; i < oldCapacity; ++i) {
            put(keys.get(i));
        }
    }


    /**
     * Put element with key to HashSet.
     * Overwrite key if there is element with such key in set.
     *
     * @param key   Key of element.
     */
    public void put(K key) {
        if (count == capacity) {
            rebuild(capacity << 1);
        }
        int hash = hashFunction(key);
        int current = first[hash];
        while (current >= 0) {
            if (keys.get(current).equals(key)) {
                if (current == getPlace(reservedSpace)) {
                    allocedPlaceWasFilled = true;
                }
                keys.set(current, key);
                return;
            }
            current = next[current];
        }
        int last = head;
        keys.set(last, key);
        head = -(next[last] + 1);
        next[last] = first[hash];
        first[hash] = last;
        count++;
    }


    /**
     * Try to add element with key to HashSet.
     *
     * @param key   Key of element.
     * @return True if we can add this element(there is no element with such key). False otherwise.
     */
    public boolean tryPut(K key) {
        if (count == capacity) {
            rebuild(capacity << 1);
        }
        int hash = hashFunction(key);
        int current = first[hash];
        while (current >= 0) {
            if (keys.get(current).equals(key)) {
                return false;
            }
            current = next[current];
        }
        int last = head;
        keys.set(last, key);
        head = -(next[last] + 1);
        next[last] = first[hash];
        first[hash] = last;
        count++;
        return true;
    }


    /**
     * Remove element with such key from HashSet.
     *
     * @param key Key of element to delete.
     * @return True if element exists. False otherwise.
     */
    public boolean remove(K key) {
        int hash = hashFunction(key);
        int current = first[hash];
        int previous = -1;
        while (current >= 0) {
            if (keys.get(current).equals(key)) {
                if (current == getPlace(reservedSpace)) {
                    allocedPlaceWasFilled = true;
                }
                int last = head;
                head = current;
                if (current == first[hash]) {
                    first[hash] = next[current];
                } else {
                    next[previous] = next[current];
                }
                next[head] = -last - 1;

                count--;
                return true;
            }
            previous = current;
            current = next[current];
        }
        return false;
    }




    /**
     * Remove all elements from HashSet.
     */
    public void clear() {
        count = 0;
        allocedPlaceWasFilled = true;
        head = 0;
        for (int i = 0; i < next.length; ++i) {
            next[i] = -i - 2;
        }
        for (int i = 0; i < first.length; ++i) {
            first[i] = -1;
        }
    }


    private long getIterator(int hash, int place) {
        return hash | ((long) place << 32);
    }

    private int getPlace(long iterator) {
        return (int) (iterator >> 32);
    }

    private int getHash(long iterator) {
        return (int) (iterator & (0x00000000ffffffffL));
    }


    /**
     * Return true if HashSet contains key.
     *
     * @param key Key to find.
     * @return True if HashSet contains key.
     */
    public boolean containsKey(K key) {
        return locate(key) != NO_ELEMENT;
    }


    /**
     * Find iterator of element with key in HashSet.
     *
     * @param key Key to find.
     * @return Iterator of element with key in HashSet (NO_ELEMENT if key not existing).
     */
    public long locate(K key) {
        int hash = hashFunction(key);
        int current = first[hash];
        while (current >= 0) {
            if (keys.get(current).equals(key)) {
                return getIterator(hash, current);
            }
            current = next[current];
        }
        return NO_ELEMENT;
    }


    /**
     * Find iterator of element with key in HashSet of allocate empty space for element with such key.
     * You should fill allocated space before next usage of this method.
     *
     * @param key Key to find.
     * @return Index of key (or allocated space) for element.
     */
    public long locateOrReserve(K key) {
        if (!allocedPlaceWasFilled) {
            throw new RuntimeException("You try to allocate new empty space before filling old empty space");
        }
        if (count == capacity) {
            rebuild(capacity << 1);
        }

        int hash = hashFunction(key);
        int current = first[hash];
        while (current >= 0) {
            if (keys.get(current).equals(key)) {
                reservedSpace = NO_ELEMENT;
                return getIterator(hash, current);
            }
            current = next[current];
        }

        int last = head;
        head = -(next[last] + 1);
        next[last] = first[hash];
        first[hash] = last;
        keys.set(last, key);
        count++;
        allocedPlaceWasFilled = false;
        reservedSpace = getIterator(hash, last);
        return reservedSpace;
    }


    /**
     * Return iterator to first element of hash set.
     *
     * @return iterator to first element of hash set.
     */
    public long getFirst() {
        for (int i = 0; i < capacity; ++i) {
            if (first[i] >= 0) {
                return getIterator(i, first[i]);
            }
        }
        return NO_ELEMENT;
    }

    /**
     * Return iterator of element follows by given.
     *
     * @param iterator Iterator to element.
     * @return Iterator of element follows by given.
     */
    public long getNext(long iterator) {
        int place = getPlace(iterator);
        int hash = getHash(iterator);
        if (next[place] < 0) {
            hash++;
            while (hash < capacity && first[hash] < 0) {
                hash++;
            }
            if (hash == capacity) {
                return NO_ELEMENT;
            } else {
                return getIterator(hash, first[hash]);
            }
        } else {
            return getIterator(hash, next[place]);
        }
    }

    /**
     * Remove element by iterator.
     *
     * @param iterator Iterator of element.
     * @return Iterator of element follows by given.
     * @throws NoSuchElementException This method throws this exception if you try to delete element by incorrect iterator.
     */
    public long removeAt(long iterator) throws NoSuchElementException {
        if (iterator == NO_ELEMENT) {
            throw new NoSuchElementException("You try to delete element by incorrect iterator");
        }

        if (iterator == reservedSpace) {
            allocedPlaceWasFilled = true;
        }

        int previous = -1;
        long nxt = getNext(iterator);
        int hash = getHash(iterator);
        int current = first[hash];
        int itPlace = getPlace(iterator);
        while (next[current] >= 0 && current != itPlace) {
            previous = current;
            current = next[current];
        }
        int last = head;
        head = current;
        if (itPlace == first[hash]) {
            first[hash] = next[current];
        } else {
            next[previous] = next[current];
        }
        next[current] = -last - 1;
        count--;
        return nxt;
    }

    /**
     * Return key of element by iterator.
     *
     * @param iterator Iterator of element.
     * @return Key of element by iterator.
     */
    public K getKeyAt(long iterator) {
        int place = getPlace(iterator);
        return keys.get(place);
    }


    /**
     * Set key of element by iterator.
     *
     * @param iterator Iterator of element.
     * @param value    New key to set.
     */
    public void setKeyAt(long iterator, K value) {
        if (iterator == reservedSpace) {
            allocedPlaceWasFilled = true;
        }
        int place = getPlace(iterator);
        keys.set(place, value);
    }


    /**
     * Returns an iterator over elements of type {@code T}.
     *
     * @return an Iterator.
     */
    @Override
    public Iterator<K> iterator() {
        return new Iterator<K>() {
            long key = -1;

            @Override
            public boolean hasNext() {
                if ((key == -1 && getFirst() >= 0) || (key >= 0 && getNext(key) >= 0)) {
                    return true;
                }
                return false;
            }

            @Override
            public K next() {
                if (key == -1) {
                    key = getFirst();
                } else {
                    key = getNext(key);
                }
                int place = getPlace(key);
                return keys.get(place);
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
                removeAt(key);
            }
        };
    }

    /**
     * Return true if HashSet is empty.
     * @return True if HashSet is empty.
     */
    public boolean isEmpty() {
        return count == 0;
    }

}

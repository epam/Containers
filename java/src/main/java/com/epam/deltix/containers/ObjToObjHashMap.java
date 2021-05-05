package com.epam.deltix.containers;


import java.util.*;


/**
 * Public class for HashMap. Key is Object, Value is Object.
 */

public class ObjToObjHashMap<K, V> implements Iterable<ObjObjPair<K, V>> {

    /**
     * Pointer to empty element.
     */
    public static final int NO_ELEMENT = -1;
    private static final int NON_EMPTY_FLAG = Integer.MIN_VALUE;

    V defaultValue;

    ArrayList<K> keys;
    ArrayList<V> values;
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
     * Return number of elements in hashmap.
     *
     * @return Number of elements in hashmap.
     */
    public int size() {
        return count;
    }

    /**
     * Return capacity of hashmap.
     *
     * @return Capacity of hashmap.
     */
    public int getCapacity() {
        return capacity;
    }


    /**
     * Increase capacity of this hashmap to new value. This method ignores attempts to decrease capacity.
     * @param newCapacity New capacity of hashmap.
     */
    public void setCapacity(int newCapacity) {
        if (capacity >= newCapacity) return;
        rebuild(newCapacity);
    }

    /**
     * Create instance of hashmap.
     *
     * @param startCapacity Start capacity of hashmap.
     * @param defaultValue  Default value. Used as return-value for some methods.
     */
    public ObjToObjHashMap(int startCapacity, V defaultValue) {
        this.defaultValue = defaultValue;
        keys = new ArrayList<K>(startCapacity);
        for (int i = 0; i < startCapacity; ++i) {
            keys.add(null);
        }
        values = new ArrayList<V>(startCapacity);
        for (int i = 0; i < startCapacity; ++i) {
            values.add(null);
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
     * Create instance of hashmap.
     *
     * @param defaultValue Default value. Used as return-value for some methods.
     */
    public ObjToObjHashMap(V defaultValue) {
        this(8, defaultValue);
    }

    void rebuild(int newCapacity) {
        int oldCapacity = capacity;
        capacity = newCapacity;
        while (values.size() < capacity) {
            values.add(null);
        }
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
            set(keys.get(i), values.get(i));
        }
    }


    /**
     * Set element with key to value. Add new element with key and value if there is no element with such key.
     * Overwrite only value if there is element with such key in map.
     *
     * @param key   Key of element.
     * @param value Value of element.
     */
    public void set(K key, V value) {
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
                values.set(current, value);
                return;
            }
            current = next[current];
        }
        int last = head;
        keys.set(last, key);
        values.set(last, value);
        head = -(next[last] + 1);
        next[last] = first[hash];
        if (next[last] < 0) {
            next[last] = NON_EMPTY_FLAG;
        }
        first[hash] = last;
        count++;
    }


    /**
     * Try to add element with key and value.
     *
     * @param key   Key of element.
     * @param value Value of element.
     * @return True if we can add this element(there is no element with such key). False otherwise.
     */
    public boolean trySet(K key, V value) {
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
        values.set(last, value);
        head = -(next[last] + 1);
        next[last] = first[hash];
        if (next[last] < 0) {
            next[last] = NON_EMPTY_FLAG;
        }
        first[hash] = last;
        count++;
        return true;
    }

    /**
     * Add element with key and value to HashMap. If there is element with such key than this method overwrite only value.
     *
     * @param key   Key of element.
     * @param value Value of element.
     * @return Old value of element or default value(if key not exists).
     */
    public V setAndGet(K key, V value) {
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
                V oldValue = values.get(current);
                values.set(current, value);
                return oldValue;
            }
            current = next[current];
        }
        int last = head;
        keys.set(last, key);
        values.set(last, value);
        head = -(next[last] + 1);
        next[last] = first[hash];
        if (next[last] < 0) {
            next[last] = NON_EMPTY_FLAG;
        }
        first[hash] = last;
        count++;
        return defaultValue;
    }


    /**
     * Get value of element by key.
     *
     * @param key Key of element.
     * @return Value of element with key or default if there is no element with such key.
     */
    public V get(K key) {
        int hash = hashFunction(key);
        int current = first[hash];
        while (current >= 0) {
            if (keys.get(current).equals(key)) {
                return values.get(current);
            }
            current = next[current];
        }
        return defaultValue;
    }

    /**
     * Remove element with such key from hashmap.
     *
     * @param key Key of element to delete.
     * @return True if element exists. False otherwise.
     */
    public boolean tryRemove(K key) {
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
     * Remove element from hashmap.
     *
     * @param key Key of element to delete.
     * @return Value of removed element or default(if there is no key to remove).
     */
    public V remove(K key) {
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
                return values.get(current);
            }
            previous = current;
            current = next[current];
        }
        return defaultValue;
    }


    /**
     * Remove all elements from HashMap.
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
     * Return true if hashmap contains key.
     *
     * @param key Key to find.
     * @return True if hashmap contains key.
     */
    public boolean containsKey(K key) {
        return locate(key) != NO_ELEMENT;
    }


    /**
     * Find iterator of element with key in HashMap.
     *
     * @param key Key to find.
     * @return Iterator of element with key in HashMap (NO_ELEMENT if key not existing).
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
     * Find iterator of element with key in HashMap of allocate empty space for element with such key.
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
        if (next[last] < 0) {
            next[last] = NON_EMPTY_FLAG;
        }
        first[hash] = last;
        keys.set(last, key);
        count++;
        allocedPlaceWasFilled = false;
        reservedSpace = getIterator(hash, last);
        return reservedSpace;
    }


    /**
     * Return iterator to first element of hash map.
     *
     * @return iterator to first element of hash map.
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
     * Return value of element by iterator.
     *
     * @param iterator Iterator of element.
     * @return Value of element by iterator.
     */
    public V getValueAt(long iterator) {
        int place = getPlace(iterator);
        return values.get(place);
    }


    /**
     * Set value of element by iterator.
     *
     * @param iterator Iterator of element.
     * @param value    new Value of element.
     */
    public void setValueAt(long iterator, V value) {
        int place = getPlace(iterator);
        values.set(place, value);
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
     * Get unsafe iterator for first element. Return NO_ELEMENT if there is no first element.
     *
     * Note: You can't use methods for safe iteration with unsafe iterator.
     *
     * @return Unsafe iterator for first element.
     */
    public int getUnsafeFirst() {
        for (int i = 0; i < keys.size(); ++i) {
            if (next[i] >= 0 || next[i] == NON_EMPTY_FLAG) {
                return i;
            }
        }
        return NO_ELEMENT;
    }

    /**
     * Get unsafe iterator for next element. Return NO_ELEMENT if there is no next element.
     *
     * Note: You can't use methods for safe iteration with unsafe iterator.
     *
     *
     * @param unsafeIterator Unsafe iterator for current element.
     * @return Unsafe iterator for next element.
     */
    public int getUnsafeNext(int unsafeIterator) {
        for (int i = unsafeIterator + 1; i < keys.size(); ++i) {
            if (next[i] >= 0 || next[i] == NON_EMPTY_FLAG) {
                return i;
            }
        }
        return NO_ELEMENT;
    }

    /**
     * Get key by unsafe iterator.
     *
     * @param unsafeIterator Unsafe iterator for element.
     * @return Key of element.
     */
    public K getKeyByUnsafeIterator(int unsafeIterator) {
        return keys.get(unsafeIterator);
    }

    /**
     * Get value by unsafe iterator.
     * @param unsafeItetator Unsafe iterator for element.
     * @return Value of element.
     */
    public V getValueByUnsafeIterator(int unsafeItetator) {
        return values.get(unsafeItetator);
    }

    /**
     * Returns an iterator over elements of type {@code T}.
     *
     * @return an Iterator.
     */
    @Override
    public Iterator<ObjObjPair<K, V>> iterator() {
        return new Iterator<ObjObjPair<K, V>>() {
            long key = -1;

            @Override
            public boolean hasNext() {
                if ((key == -1 && getFirst() >= 0) || (key >= 0 && getNext(key) >= 0)) {
                    return true;
                }
                return false;
            }

            @Override
            public ObjObjPair<K, V> next() {
                if (key == -1) {
                    key = getFirst();
                } else {
                    key = getNext(key);
                }
                int place = getPlace(key);
                return new ObjObjPair<>(keys.get(place), values.get(place));
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
     * Return true if HashMap is empty.
     * @return True if HashMap is empty.
     */
    public boolean isEmpty() {
        return count == 0;
    }
}

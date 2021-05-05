package com.epam.deltix.containers;

import com.epam.deltix.containers.generated.ObjToIntHashMap;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.List;

/**
 * Class for heap with user key. Top element of heap - element with minimal value.
 *
 * @param <TValue> Type of heap's value.
 * @param <TKey>   Type of heap's key.
 */
public class HeapWithDictionary<TValue, TKey> {
    Object[] elementsValue;
    private int[] elementsKey;
    Object[] elementsUserKey;
    private Comparator<TValue> comparator;
    private int[] keysPosition;
    private int head = 0;
    private ObjToIntHashMap<TKey> keys;

    private int count = 0;

    /**
     * Return number of elements in heap.
     *
     * @return Number of elements in heap.
     */
    public int getCount() {
        return count;
    }

    boolean isHeap = false;
    private int listThreshold = 5;

    /**
     * Return number of elements in heap is more than this value, than heap will be real heap.
     *
     * @return Number of elements in heap is more than this value, than heap will be real heap.
     */

    @SuppressWarnings("unused")
    public int getListThreshold() {
        return listThreshold;
    }

    /**
     * Clear the heap.
     */
    public void clear() {
        count = 0;
    }

    /**
     * Create instance of heap.
     *
     * @param capacity     Heap's start capacity.
     * @param comparator   Comparator for heap(top is minimum).
     * @param listThreshold If number of elements in heap is more than this value, than heap will be real heap.
     */
    @SuppressWarnings("WeakerAccess")
    public HeapWithDictionary(int capacity, Comparator<TValue> comparator, int listThreshold) {
        this.listThreshold = listThreshold;
        this.comparator = comparator;
        elementsValue = new Object[capacity];
        elementsKey = new int[capacity];
        elementsUserKey = new Object[capacity];
        keysPosition = new int[capacity];
        keys = new ObjToIntHashMap<>(capacity, -1);
        for (int i = 0; i < capacity; ++i) keysPosition[i] = -i - 2;
    }

    /**
     * Create instance of heap.
     *
     * @param capacity   Heap's start capacity.
     * @param comparator Comparator for heap(top is minimum).
     */
    @SuppressWarnings("unused")
    public HeapWithDictionary(int capacity, Comparator<TValue> comparator) {
        this(capacity, comparator, 10);
    }

    /**
     * Create instance of heap.
     *
     * @param comparator Comparator for heap(top is minimum).
     */
    @SuppressWarnings("unused")
    public HeapWithDictionary(Comparator<TValue> comparator) {
        this(10, comparator, 10);
    }

    /**
     * Add element to heap.
     *
     * @param value Element's value.
     * @param key   Element's key.
     * @return True if operation was success.
     */
    public boolean add(TValue value, TKey key) {
        if (keys.containsKey(key)) return false;
        if (elementsValue.length == count) {
            elementsValue = Arrays.copyOf(elementsValue, elementsValue.length << 1);
            elementsKey = Arrays.copyOf(elementsKey, elementsKey.length << 1);
            elementsUserKey = Arrays.copyOf(elementsUserKey, elementsUserKey.length << 1);
        }
        if (isHeap) return keys.trySet(key, addHeap(value, key));
        else return keys.trySet(key, addList(value, key));
    }

    @SuppressWarnings("unchecked")
    private int addHeap(TValue value, TKey userKey) {
        int index = count;
        int next = (index - 1) >> 1;
        while (index > 0 && comparator.compare(value, (TValue) elementsValue[next]) < 0) {
            keysPosition[elementsKey[next]] = index;
            elementsKey[index] = elementsKey[next];
            elementsValue[index] = elementsValue[next];
            elementsUserKey[index] = elementsUserKey[next];
            index = next;
            next = (index - 1) >> 1;
        }
        elementsKey[index] = getNextFreeKey();
        elementsValue[index] = value;
        elementsUserKey[index] = userKey;
        keysPosition[elementsKey[index]] = index;
        count++;
        return elementsKey[index];
    }

    @SuppressWarnings("unchecked")
    private int binarySearch(TValue value) {
        if (count == 0) return 0;
        int l = 0;
        int r = count - 1;
        while (l < r) {
            int c = (l + r) / 2;
            if (comparator.compare(value, (TValue) elementsValue[c]) < 0) l = c + 1;
            else r = c;
        }
        if (comparator.compare(value, (TValue) elementsValue[l]) < 0) l++;
        return l;
    }

    private int addList(TValue value, TKey userKey) {
        int insertIndex = binarySearch(value);
        for (int k = count; k > insertIndex; k--) {
            elementsKey[k] = elementsKey[k - 1];
            elementsValue[k] = elementsValue[k - 1];
            elementsUserKey[k] = elementsUserKey[k - 1];
            keysPosition[elementsKey[k]] = k;
        }
        int key = getNextFreeKey();
        elementsKey[insertIndex] = key;
        elementsValue[insertIndex] = value;
        elementsUserKey[insertIndex] = userKey;
        keysPosition[key] = insertIndex;
        count++;
        if (count == listThreshold) {
            rebuildToHeap();
            isHeap = true;
        }
        return key;
    }

    private void rebuildToHeap() {
        for (int i = 0; i < count / 2; ++i) {
            Object q = elementsValue[i];
            elementsValue[i] = elementsValue[count - 1 - i];
            elementsValue[count - 1 - i] = q;
            int temp = elementsKey[i];
            elementsKey[i] = elementsKey[count - 1 - i];
            elementsKey[count - 1 - i] = temp;
            q = elementsUserKey[i];
            elementsUserKey[i] = elementsUserKey[count - 1 - i];
            elementsUserKey[count - 1 - i] = q;
        }
        for (int i = 0; i < count; ++i) keysPosition[elementsKey[i]] = i;
        isHeap = true;
    }

    private int getNextFreeKey() {
        int key = Math.abs(head);
        if (key >= keysPosition.length) {
            int oldLength = keysPosition.length;
            keysPosition = Arrays.copyOf(keysPosition, keysPosition.length << 1);
            for (int i = oldLength; i < keysPosition.length; ++i) keysPosition[i] = -i - 2;
        }
        head = -(keysPosition[head] + 1);
        return key;
    }

    /**
     * Remove top element from heap.
     *
     * @throws IndexOutOfBoundsException Throw this exception if heap is empty.
     */
    @SuppressWarnings("WeakerAccess")
    public void pop() throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) popHeap();
        else popList();
    }

    private void popList() {
        count--;
        keys.remove((TKey)elementsUserKey[count]);
        int last = head;
        head = elementsKey[count];
        keysPosition[head] = -last - 1;
    }

    @SuppressWarnings("unchecked")
    private void popHeap() {
        int last = head;
        head = elementsKey[0];
        keys.remove((TKey)elementsUserKey[0]);
        keysPosition[head] = -last - 1;
        elementsKey[0] = elementsKey[count - 1];
        elementsValue[0] = elementsValue[count - 1];
        TValue tempValue = (TValue) elementsValue[count - 1];
        TKey tempUserKey = (TKey) elementsUserKey[count - 1];
        int tempKey = elementsKey[count - 1];
        count--;
        if (count == 0) return;
        int current = 0;
        while (current < count) {
            int left = (current << 1) + 1;
            int right = (current << 1) + 2;
            int swapIndex = current;
            if (left >= count) {
                swapIndex = current;
            } else if (right >= count) {
                if (comparator.compare((TValue) elementsValue[left], tempValue) < 0) swapIndex = left;
            } else {
                if (comparator.compare((TValue) elementsValue[left], (TValue) elementsValue[right]) < 0) {
                    if (comparator.compare((TValue) elementsValue[left], tempValue) < 0) swapIndex = left;
                } else if (comparator.compare((TValue) elementsValue[right], tempValue) < 0) swapIndex = right;
            }
            if (swapIndex == current) break;
            elementsValue[current] = elementsValue[swapIndex];
            elementsKey[current] = elementsKey[swapIndex];
            elementsUserKey[current] = elementsUserKey[swapIndex];
            keysPosition[elementsKey[current]] = current;
            current = swapIndex;
        }
        elementsValue[current] = tempValue;
        elementsKey[current] = tempKey;
        elementsUserKey[current] = tempUserKey;
        keysPosition[elementsKey[current]] = current;
    }

    /**
     * Modify heap's element with key.
     *
     * @param key      Key of element.
     * @param newValue New value of element.
     * @return true if heap contains element with key.
     */
    @SuppressWarnings("unused")
    public boolean modify(TKey key, TValue newValue) {

        int intKey;
        intKey = keys.get(key);
        if (intKey < 0) {
            return false;
        }
        if (isHeap) return modifyHeap(intKey, newValue, key);
        else return modifyList(intKey, newValue, key);
    }

    private boolean modifyList(int key, TValue newValue, TKey userKey) {
        if (key < 0 || key >= keysPosition.length || keysPosition[key] < 0) return false;
        int index = keysPosition[key];
        if (index < 0) return false;
        for (int i = index; i < count - 1; ++i) {
            elementsKey[i] = elementsKey[i + 1];
            elementsValue[i] = elementsValue[i + 1];
            elementsUserKey[i] = elementsUserKey[i + 1];
            keysPosition[elementsKey[i]] = i;
        }
        count--;
        int insertIndex = binarySearch(newValue);
        for (int i = count; i > insertIndex; i--) {
            elementsKey[i] = elementsKey[i - 1];
            elementsValue[i] = elementsValue[i - 1];
            elementsUserKey[i] = elementsUserKey[i - 1];
            keysPosition[elementsKey[i]] = i;
        }
        elementsKey[insertIndex] = key;
        elementsValue[insertIndex] = newValue;
        elementsUserKey[insertIndex] = userKey;
        keysPosition[key] = insertIndex;
        count++;
        return true;
    }

    @SuppressWarnings("unchecked")
    private boolean modifyHeap(int key, TValue newValue, TKey userKey) {
        if (key < 0 || key >= keysPosition.length || keysPosition[key] < 0) return false;
        int index = keysPosition[key];
        int current = index;
        while (current < count) {
            int left = (current << 1) + 1;
            int right = (current << 1) + 2;
            int swapIndex = current;
            if (left >= count) swapIndex = current;
            else if (right >= count) {
                if (comparator.compare((TValue) elementsValue[left], newValue) < 0) swapIndex = left;
            } else {
                if (comparator.compare((TValue) elementsValue[left], (TValue) elementsValue[right]) < 0) {
                    if (comparator.compare((TValue) elementsValue[left], newValue) < 0) swapIndex = left;
                } else if (comparator.compare((TValue) elementsValue[right], newValue) < 0) swapIndex = right;
            }
            if (swapIndex == current) break;
            elementsValue[current] = elementsValue[swapIndex];
            elementsKey[current] = elementsKey[swapIndex];
            elementsUserKey[current] = elementsUserKey[swapIndex];
            keysPosition[elementsKey[current]] = current;
            current = swapIndex;
        }
        elementsValue[current] = newValue;
        elementsUserKey[current] = userKey;
        elementsKey[current] = key;
        keysPosition[elementsKey[current]] = current;
        current = index;
        TValue tempValue = (TValue) elementsValue[current];
        int tempKey = elementsKey[current];
        TKey tempUserKey = (TKey) elementsUserKey[current];
        while (current != 0) {
            int swapIndex = (current - 1) >> 1;
            if (comparator.compare(tempValue, (TValue) elementsValue[swapIndex]) < 0) {
                elementsValue[current] = elementsValue[swapIndex];
                elementsKey[current] = elementsKey[swapIndex];
                elementsUserKey[current] = elementsUserKey[swapIndex];
                keysPosition[elementsKey[current]] = current;
                current = swapIndex;
            } else break;
        }
        if (current != index) {
            elementsValue[current] = tempValue;
            elementsKey[current] = tempKey;
            elementsUserKey[current] = tempUserKey;
            keysPosition[elementsKey[current]] = current;
        }
        return true;
    }

    /**
     * Modify top element of heap.
     *
     * @param newValue New value of top element.
     * @throws IndexOutOfBoundsException Throw this exception if heap is empty.
     */
    @SuppressWarnings("unused")
    public void modifyTop(TValue newValue) throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) modifyTopHeap(newValue);
        else modifyTopList(newValue);
    }

    /**
     * Modify top element of heap.
     *
     * @param newKey   New key of top element.
     * @param newValue New value of top element.
     * @throws IndexOutOfBoundsException Throw this exception if heap is empty.
     */
    @SuppressWarnings("unused")
    public void modifyTop(TKey newKey, TValue newValue) throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        pop();
        add(newValue, newKey);
    }

    @SuppressWarnings("unchecked")
    private void modifyTopHeap(TValue newValue) {
        int tempKey = elementsKey[0];
        TKey tempUserKey = (TKey) elementsUserKey[0];
        int current = 0;
        while (current < count) {
            int left = (current << 1) + 1;
            int right = (current << 1) + 2;
            int swapIndex = current;
            if (left >= count) swapIndex = current;
            else if (right >= count) {
                if (comparator.compare((TValue) elementsValue[left], newValue) < 0) swapIndex = left;
            } else {
                if (comparator.compare((TValue) elementsValue[left], (TValue) elementsValue[right]) < 0) {
                    if (comparator.compare((TValue) elementsValue[left], newValue) < 0) swapIndex = left;
                } else if (comparator.compare((TValue) elementsValue[right], newValue) < 0) swapIndex = right;
            }
            if (swapIndex == current) break;
            elementsKey[current] = elementsKey[swapIndex];
            elementsValue[current] = elementsValue[swapIndex];
            elementsUserKey[current] = elementsUserKey[swapIndex];
            keysPosition[elementsKey[current]] = current;
            current = swapIndex;
        }
        elementsValue[current] = newValue;
        elementsUserKey[current] = tempUserKey;
        elementsKey[current] = tempKey;
        keysPosition[elementsKey[current]] = current;
    }

    @SuppressWarnings("unchecked")
    private void modifyTopList(TValue newValue) {
        int tempKey = elementsKey[count - 1];
        TKey tempUserKey = (TKey) elementsUserKey[count - 1];
        int insertIndex = binarySearch(newValue);
        if (insertIndex >= count) insertIndex = count - 1;
        for (int i = count - 1; i > insertIndex; i--) {
            elementsValue[i] = elementsValue[i - 1];
            elementsKey[i] = elementsKey[i - 1];
            elementsUserKey[i] = elementsUserKey[i - 1];
            keysPosition[elementsKey[i]] = i;
        }
        elementsKey[insertIndex] = tempKey;
        elementsUserKey[insertIndex] = tempUserKey;
        elementsValue[insertIndex] = newValue;
        keysPosition[tempKey] = insertIndex;
    }

    /**
     * Remove element with key from heap.
     *
     * @param key Key of element.
     * @return True if heap contains element with key.
     */
    public boolean remove(TKey key) {
        int intKey;
        intKey = keys.get(key);
        if (intKey < 0) {
            return false;
        }
        if (isHeap) removeHeap(intKey);
        else removeList(intKey);
        keys.remove(key);
        return true;
    }

    private void removeList(int key) {
        int index = keysPosition[key];
        int last = head;
        head = elementsKey[index];
        keysPosition[head] = -last - 1;
        count--;
        for (int i = index; i < count; ++i) {
            elementsValue[i] = elementsValue[i + 1];
            elementsKey[i] = elementsKey[i + 1];
            elementsUserKey[i] = elementsUserKey[i + 1];
            keysPosition[elementsKey[i]] = i;
        }
    }

    @SuppressWarnings("unchecked")
    private void removeHeap(int key) {
        int index = keysPosition[key];
        int last = head;
        head = elementsKey[index];
        keysPosition[head] = -last - 1;
        TValue tempValue = (TValue) elementsValue[count - 1];
        int tempKey = elementsKey[count - 1];
        TKey tempUserKey = (TKey) elementsUserKey[count - 1];
        count--;
        if (index == count) return;
        if (count == 0) return;
        int current = index;
        while (current < count) {
            int left = (current << 1) + 1;
            int right = (current << 1) + 2;
            int swapIndex = current;
            if (left >= count) swapIndex = current;
            else if (right >= count) {
                if (comparator.compare((TValue) elementsValue[left], tempValue) < 0) swapIndex = left;
            } else {
                if (comparator.compare((TValue) elementsValue[left], (TValue) elementsValue[right]) < 0) {
                    if (comparator.compare((TValue) elementsValue[left], tempValue) < 0) swapIndex = left;
                } else if (comparator.compare((TValue) elementsValue[right], tempValue) < 0) swapIndex = right;
            }
            if (swapIndex == current) break;
            elementsValue[current] = elementsValue[swapIndex];
            elementsKey[current] = elementsKey[swapIndex];
            elementsUserKey[current] = elementsUserKey[swapIndex];
            keysPosition[elementsKey[current]] = current;
            current = swapIndex;
        }
        elementsValue[current] = tempValue;
        elementsKey[current] = tempKey;
        elementsUserKey[current] = tempUserKey;
        keysPosition[elementsKey[current]] = current;
        current = index;
        tempValue = (TValue) elementsValue[current];
        tempKey = elementsKey[current];
        tempUserKey = (TKey) elementsUserKey[current];
        while (current != 0) {
            int swapIndex = (current - 1) >> 1;
            if (comparator.compare(tempValue, (TValue) elementsValue[swapIndex]) < 0) {
                elementsValue[current] = elementsValue[swapIndex];
                elementsKey[current] = elementsKey[swapIndex];
                elementsUserKey[current] = elementsUserKey[swapIndex];
                keysPosition[elementsKey[current]] = current;
                current = swapIndex;
            } else break;
        }
        if (current != index) {
            elementsValue[current] = tempValue;
            elementsKey[current] = tempKey;
            elementsUserKey[current] = tempUserKey;
            keysPosition[elementsKey[current]] = current;
        }
    }

    /**
     * Get value of top element.
     *
     * @return Value of top element.
     */
    @SuppressWarnings({"unchecked", "unused"})
    public TValue peekValue() {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty.");
        if (isHeap) return (TValue) elementsValue[0];
        else return (TValue) elementsValue[count - 1];
    }

    /**
     * Get key of top element.
     *
     * @return Key of top element.
     */
    @SuppressWarnings({"unchecked", "unused"})
    public TKey peekKey() {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty.");
        if (isHeap) return (TKey) elementsUserKey[0];
        else return (TKey) elementsUserKey[count - 1];
    }

    /**
     * Return true if heap contains key.
     *
     * @param key Key to find.
     * @return True if heap contains key.
     */
    public boolean containsKey(TKey key) {
        return keys.containsKey(key);
    }



    /**
     * Return key by value with null default
     * @param key Key.
     * @return Value of element or null if no key.
     */
    public TValue getValue(TKey key) {
        return getValue(key, null);
    }


    /**
     * Return value by key.
     *
     * @param key Key of element.
     * @param def Returned value if element not founded.
     * @return Value of element or default if element not founded.
     */
    @SuppressWarnings("unchecked")
    public TValue getValue(TKey key, TValue def) {
        int _intKey;
        _intKey = keys.get(key);
        if (_intKey < 0) {
            return def;
        }
        return (TValue) elementsValue[keysPosition[_intKey]];
    }

    /**
     * Return list with all values in heap.
     * @return List with all values in heap.
     */
    @SuppressWarnings("unchecked")
    public List<TValue> values() {
        ArrayList<TValue> returnList = new ArrayList<>();
        for (int i = 0; i < count; ++i) returnList.add((TValue)elementsValue[i]);
        return returnList;
    }

    /**
     * Return list with all keys in heap.
     * @return List with all keys in heap.
     */
    @SuppressWarnings("unchecked")
    public List<TKey> keys() {
        ArrayList<TKey> returnList = new ArrayList<>();
        for (int i = 0; i < count; ++i) returnList.add((TKey)elementsUserKey[i]);
        return returnList;
    }

    /**
     * Return true if heap is empty.
     * @return True if heap is empty.
     */
    public boolean isEmpty() {
        return count == 0;
    }
}

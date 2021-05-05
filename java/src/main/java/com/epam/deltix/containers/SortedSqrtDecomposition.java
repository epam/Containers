package com.epam.deltix.containers;


import java.util.Arrays;
import java.util.Comparator;

/**
 * Sorted collection. Insert/Delete works with complexity O(sqrt(N)). Iteration to next element works with complexity O(1).
 * Iterators can become incorrect after any modify operation.
 * @param <T> Type of elements.
 */
public class SortedSqrtDecomposition<T> {
    private Object[] data;
    private Object[] dataInBegin;
    private Object[] tempArray;
    private int numberOfBlocks;
    private int[] endOfBlock;
    private int size, len;
    Comparator<T> comparator;
    boolean needRebuild = false;

    /**
     * No element pointer.
     */
    public static final int NO_ELEMENT = -1;


    /**
     * Create instance of sorted sqrt decomposition.
     * @param capacity Capacity of decomposition.
     * @param comparator Comparator.
     */
    public SortedSqrtDecomposition(int capacity, Comparator<T> comparator) {
        size = 0;
        numberOfBlocks = 1;
        data = new Object[10];
        dataInBegin = new Object[10];
        tempArray = new Object[10];
        len = 5;
        endOfBlock = new int[10];
        this.comparator = comparator;
    }

    /**
     * Create instance of sorted sqrt decomposition with default capacity.
     * @param comparator Comparator.
     */
    public SortedSqrtDecomposition(Comparator<T> comparator) {
        this(10, comparator);
    }

    private void resizeBlocks(int size) {
        endOfBlock = Arrays.copyOf(endOfBlock, size);
        dataInBegin = Arrays.copyOf(data, size);
    }


    private void resizeData(int size) {
        data = Arrays.copyOf(data, size);
        tempArray = Arrays.copyOf(tempArray, size);
    }

    private void rebuildSqrt() {
        int index = 0;
        needRebuild = false;
        for (int i = 0; i < numberOfBlocks; ++i) {
            for (int j = (len * i << 1); j < endOfBlock[i]; ++j) {
                tempArray[index] = data[j];
                index++;
            }
        }
        len = (int) Math.ceil(Math.sqrt(index));
        if (len < 5) {
            len = 5;
        }
        numberOfBlocks = (int) Math.ceil(1.0 * index / len);
        if (numberOfBlocks == 0) {
            numberOfBlocks = 1;
            return;
        }
        if (endOfBlock.length <= numberOfBlocks) {
            resizeBlocks(numberOfBlocks << 1);
        }
        if (data.length <= (len * (numberOfBlocks + 3) << 1)) {
            resizeData((len * (2 + numberOfBlocks)) << 2);
        }

        for (int i = 0; i < index; ++i) {
            int tempIndex = i / len;
            data[(len * tempIndex << 1) + i % len] = tempArray[i];
            endOfBlock[tempIndex] = (len * tempIndex << 1) + i % len + 1;
        }
        for (int i = 0; i < numberOfBlocks; ++i) dataInBegin[i] = data[len * i << 1];
    }

    /**
     * Return size of sqrt decomposition.
     * @return Size of sqrt decomposition;
     */
    public int size() {
        return size;
    }


    /**
     * Add element if there is no such element in decomposition. Return iterator to added element of iterator to existing element.
     * @param x Element to add.
     * @return Iterator to given element.
     */
    public int addIfNotExist(T x) {
        boolean insert = false;
        if (needRebuild) {
            rebuildSqrt();
        }
        for (int j = 0; j < numberOfBlocks; ++j) {
            if (size != 0 && dataInBegin[j] != null && comparator.compare((T) dataInBegin[j], x) > 0) {
                if (j == 0) {
                    for (int k = endOfBlock[0]; k > 0; k--) {
                        data[k] = data[k - 1];
                    }
                    data[0] = x;
                    dataInBegin[0] = x;
                    endOfBlock[0]++;
                    insert = true;
                    size++;
                    if (endOfBlock[0] == (len << 1)) {
                        needRebuild = true;
                    }
                    return 0;
                }
                for (int k = (len * (j - 1) << 1); k < endOfBlock[j - 1]; k++)
                    if (comparator.compare((T) data[k], x) > 0) {
                        for (int l = endOfBlock[j - 1]; l > k; l--) {
                            data[l] = data[l - 1];
                        }
                        data[k] = x;
                        endOfBlock[j - 1]++;
                        insert = true;
                        size++;
                        if (endOfBlock[j - 1] == (len * j << 1)) needRebuild = true;
                        return k;
                    } else if (comparator.compare((T) data[k], x) == 0) {
                        return -(k + 1);
                    }
                if (!insert) {
                    data[endOfBlock[j - 1]] = x;
                    insert = true;
                    endOfBlock[j - 1]++;
                    size++;
                    if (endOfBlock[j - 1] == (len * j << 1)) {
                        needRebuild = true;
                    }
                    return endOfBlock[j - 1] - 1;
                }
            }
        }
        if (!insert) {
            for (int j = (len * (numberOfBlocks - 1) << 1); j < endOfBlock[numberOfBlocks - 1]; ++j)
                if (data[j] != null && comparator.compare((T)data[j], x) > 0) {
                    for (int l = endOfBlock[numberOfBlocks - 1]; l > j; l--) {
                        data[l] = data[l - 1];
                    }
                    data[j] = x;
                    if (j == ((len * (numberOfBlocks - 1)) << 1)) dataInBegin[numberOfBlocks - 1] = x;
                    insert = true;
                    endOfBlock[numberOfBlocks - 1]++;
                    size++;
                    if (endOfBlock[numberOfBlocks - 1] == (len * numberOfBlocks << 1)) needRebuild = true;
                    return j;
                } else if (comparator.compare((T)data[j], x) == 0) {
                    return -(j + 1);
                }
            if (!insert) {
                size++;
                if (endOfBlock[numberOfBlocks - 1] == (len * (numberOfBlocks - 1) << 1))
                    dataInBegin[numberOfBlocks - 1] = x;
                data[endOfBlock[numberOfBlocks - 1]] = x;
                endOfBlock[numberOfBlocks - 1]++;
                if (endOfBlock[numberOfBlocks - 1] == (len * numberOfBlocks << 1)) needRebuild = true;
                return endOfBlock[numberOfBlocks - 1] - 1;
            }
        }
        return NO_ELEMENT;
    }

    /**
     * Remove element from sorted sqrt decomposition
     * @param x Element to remove.
     * @return True if this element was in sqrt decomposition. False otherwise.
     */
    public boolean remove(T x) {
        boolean delete = false;
        if (needRebuild) {
            rebuildSqrt();
        }
        for (int j = 1; j < numberOfBlocks; ++j) {
            if (comparator.compare((T) dataInBegin[j], x) > 0) {
                for (int k = (len * (j - 1) << 1); k < endOfBlock[j - 1]; ++k) {
                    if (comparator.compare((T)data[k], x) == 0) {
                        if (k == (len * (j - 1) << 1)) dataInBegin[j - 1] = data[k + 1];
                        for (int l = k; l < endOfBlock[j - 1] - 1; l++) {
                            data[l] = data[l + 1];
                        }
                        endOfBlock[j - 1]--;
                        size--;
                        if (endOfBlock[j - 1] <= (len * (j - 1) << 1)) {
                            rebuildSqrt();
                        };
                        return true;
                    }
                }
                return false;
            }
        }
        if (!delete) {
            for (int k = (len * (numberOfBlocks - 1) << 1); k < endOfBlock[numberOfBlocks - 1]; k++) {
                if (comparator.compare((T) data[k], x) == 0) {
                    if (k == (len * (numberOfBlocks - 1)) << 1) dataInBegin[numberOfBlocks - 1] = data[k + 1];
                    for (int l = k; l < endOfBlock[numberOfBlocks - 1] - 1; l++) {
                        data[l] = data[l + 1];
                    }
                    endOfBlock[numberOfBlocks - 1]--;
                    size--;
                    if (endOfBlock[numberOfBlocks - 1] <= (len * (numberOfBlocks - 1) << 1)) {
                        rebuildSqrt();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * Remove element by iterator.
     * @param iterator Iterator to element.
     */
    public void removeAt(int iterator) {
        int block = iterator / (len << 1);
        if (block >= numberOfBlocks || iterator >= endOfBlock[block]) throw new ArrayIndexOutOfBoundsException("Try to remove by incorrect iterator");
        if (iterator == (len * (block) << 1)) dataInBegin[block] = data[iterator + 1];
        for (int l = iterator; l < endOfBlock[block] - 1; l++) {
            data[l] = data[l + 1];
        }
        endOfBlock[block]--;
        size--;
        if (endOfBlock[block] <= (len * block << 1)) {
            rebuildSqrt();
        }
    }

    /**
     * Get iterator to first element of decomposition.
     * @return Iterator to first element of decomposition.
     */
    public int getFirst() {
        if (size > 0)
            return 0;
        else
            return NO_ELEMENT;
    }

    /**
     * Get iterator to element.
     * @param element Element to find.
     * @return Iterator to element.
     */
    public int getIterator(T element) {
        for (int j = 1; j < numberOfBlocks; ++j) {
            if (dataInBegin[j] != null && comparator.compare((T)dataInBegin[j], element) > 0) {
                for (int k = (len * (j - 1) << 1); k < endOfBlock[j - 1]; ++k) {
                    if (comparator.compare((T) data[k], element) == 0) {
                        return k;
                    }
                }
            }
        }

        for (int k = (len * (numberOfBlocks - 1) << 1); k < endOfBlock[numberOfBlocks - 1]; k++)
            if (comparator.compare((T)data[k], element) == 0) {
                return k;
            }
        return NO_ELEMENT;
    }

    /**
     * Get iterator to element at index.
     * @param index Iterator to element at index.
     * @return Iterator to element at index;
     */
    public int getIteratorByIndex(int index) {
        int current = 0;
        for (int j = 0; j < numberOfBlocks; ++j) {
            if (current + endOfBlock[j] - (len * j << 1) > index) {
                int indexInData = (len * j << 1) + (index - current);
                return indexInData;
            } else current += endOfBlock[j] - (len * j << 1);
        }
        return NO_ELEMENT;
    }

    /**
     * Get element by iterator.
     * @param iterator iterator.
     * @return Element by iterator.
     */
    public T getAt(int iterator) {
        return (T)data[iterator];
    }

    /**
     * Set element by iterator to new value (sqrt decomposition should stay sorted).
     * @param iterator iterator to element.
     * @param element new value of element.
     */
    public void setAt(int iterator, T element) {
        data[iterator] = element;
    }


    /**
     * Get iterator to element followed current.
     * @param iterator Iterator to current element.
     * @return Iterator to element followed current.
     */
    public int getNext(int iterator) {
        int block = iterator / (len << 1);
        iterator++;
        if (iterator >= endOfBlock[block]) {
            block++;
            if (block >= numberOfBlocks) return NO_ELEMENT;
            iterator = 2 * len * block;
        }
        return iterator;
    }

    /**
     * Get iterator to element before current.
     * @param iterator Iterator to current element.
     * @return Iterator to previous element.
     */
    public int getPrev(int iterator) {
        int block = iterator / (len << 1);
        iterator--;
        if (iterator < 2 * len * block) {
            block--;
            if (block < 0) return NO_ELEMENT;
            iterator = endOfBlock[block] - 1;
        }
        return iterator;
    }

    /**
     * Get iterator to last element.
     * @return iterator to last element.
     */
    public int getLast() {
        if (size == 0)
            return NO_ELEMENT;
        else
            return endOfBlock[numberOfBlocks - 1] - 1;
    }

    /**
      * Return true if sqrtDecomposition is empty.
      * @return True if sqrtDecomposition is empty.
      */
    public boolean isEmpty() {
        return size == 0;
    }
}

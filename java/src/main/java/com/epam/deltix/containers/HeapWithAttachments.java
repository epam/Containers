package com.epam.deltix.containers;


import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.List;

/**
 * Implementation of Priority Queue as Binary Heap. Top element of heap - minimum.
 *
 * @param <TValue>       Type of Heap's elements.
 * @param <TAttachments> Type of attachments to heap's elements.
 */
@SuppressWarnings("unused")
public class HeapWithAttachments<TValue, TAttachments> {
    Object[] elementsValue;
    Object[] elementsAttachment;
    private Comparator<TValue> comparator;

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
    public HeapWithAttachments(int capacity, Comparator<TValue> comparator, int listThreshold) {
        this.listThreshold = listThreshold;
        this.comparator = comparator;
        elementsValue = new Object[capacity];
        elementsAttachment = new Object[capacity];
    }

    /**
     * Create instance of heap.
     *
     * @param capacity   Heap's start capacity.
     * @param comparator Comparator for heap(top is minimum).
     */
    public HeapWithAttachments(int capacity, Comparator<TValue> comparator) {
        this(capacity, comparator, 10);
    }

    /**
     * Create instance of heap.
     *
     * @param comparator Comparator for heap(top is minimum).
     */
    public HeapWithAttachments(Comparator<TValue> comparator) {
        this(10, comparator, 10);
    }

    /**
     * Add new element to heap.
     *
     * @param value       Value of element.
     * @param attachments Attachments of element.
     */
    public void add(TValue value, TAttachments attachments) {
        if (elementsValue.length == count) {
            elementsValue = Arrays.copyOf(elementsValue, elementsValue.length << 1);
            elementsAttachment = Arrays.copyOf(elementsAttachment, elementsAttachment.length << 1);
        }
        if (isHeap) addHeap(value, attachments);
        else addList(value, attachments);
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

    private void addList(TValue value, TAttachments attachments) {
        int insertIndex = binarySearch(value);
        for (int k = count; k > insertIndex; k--) {
            elementsValue[k] = elementsValue[k - 1];
            elementsAttachment[k] = elementsAttachment[k - 1];
        }
        elementsValue[insertIndex] = value;
        elementsAttachment[insertIndex] = attachments;
        count++;
        if (count > listThreshold) {
            rebuildToHeap();
        }
    }

    private void rebuildToHeap() {
        for (int i = 0; i < count / 2; ++i) {
            Object q = elementsValue[i];
            elementsValue[i] = elementsValue[count - 1 - i];
            elementsValue[count - 1 - i] = q;
            q = elementsAttachment[i];
            elementsAttachment[i] = elementsAttachment[count - 1 - i];
            elementsAttachment[count - 1 - i] = q;
        }
        isHeap = true;
    }

    @SuppressWarnings("unchecked")
    private void addHeap(TValue value, TAttachments attachments) {
        int index = count;
        int next = (index - 1) >> 1;
        while (index > 0 && comparator.compare(value, (TValue) elementsValue[next]) < 0) {
            elementsValue[index] = elementsValue[next];
            elementsAttachment[index] = elementsAttachment[next];
            index = next;
            next = (index - 1) >> 1;
        }
        elementsValue[index] = value;
        elementsAttachment[index] = attachments;
        count++;
    }

    /**
     * Remove minimum from heap.
     */
    @SuppressWarnings({"unchecked", "WeakerAccess"})
    public void pop() throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) {
            TValue tempValue = (TValue) elementsValue[count - 1];
            TAttachments tempAttachment = (TAttachments) elementsAttachment[count - 1];
            count--;
            int current = 0;
            while (current < count) {
                int left = (current << 1) + 1;
                int right = (current << 1) + 2;
                int swapIndex = current;
                if (left > count) swapIndex = current;
                else if (right > count) {
                    if (comparator.compare((TValue) elementsValue[left], tempValue) < 0) swapIndex = left;
                } else {
                    if (comparator.compare((TValue) elementsValue[left], (TValue) elementsValue[right]) < 0) {
                        if (comparator.compare((TValue) elementsValue[left], tempValue) < 0) swapIndex = left;
                    } else if (comparator.compare((TValue) elementsValue[right], tempValue) < 0) swapIndex = right;
                }
                if (swapIndex == current) break;
                elementsValue[current] = elementsValue[swapIndex];
                elementsAttachment[current] = elementsAttachment[swapIndex];
                current = swapIndex;
            }
            elementsValue[current] = tempValue;
            elementsAttachment[current] = tempAttachment;
        } else {
            count--;
        }
    }

    /**
     * Modify top element.
     *
     * @param value New value of top element.
     */
    @SuppressWarnings("unchecked")
    public void modifyTop(TValue value) throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) {
            int current = 0;
            Object attachments = elementsAttachment[0];
            while (current < count) {
                int left = (current << 1) + 1;
                int right = (current << 1) + 2;
                int swapIndex = current;
                if (left >= count) swapIndex = current;
                else if (right >= count) {
                    if (comparator.compare((TValue) elementsValue[left], value) < 0) swapIndex = left;
                } else {
                    if (comparator.compare((TValue) elementsValue[left], (TValue) elementsValue[right]) < 0) {
                        if (comparator.compare((TValue) elementsValue[left], value) < 0) swapIndex = left;
                    } else if (comparator.compare((TValue) elementsValue[right], value) < 0) swapIndex = right;
                }
                if (swapIndex == current) break;
                elementsValue[current] = elementsValue[swapIndex];
                elementsAttachment[current] = elementsAttachment[swapIndex];
                current = swapIndex;
            }
            elementsValue[current] = value;
            elementsAttachment[current] = attachments;
        } else {
            TAttachments tempAttachment = (TAttachments) elementsAttachment[count - 1];
            pop();
            add(value, tempAttachment);
        }
    }

    /**
     * Modify top element.
     *
     * @param value       New value of top element.
     * @param attachments New attachments of top element.
     * @throws IndexOutOfBoundsException Throw this exception if heap is empty.
     */
    @SuppressWarnings("unchecked")
    public void modifyTop(TValue value, TAttachments attachments) throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty");
        if (isHeap) {
            int current = 0;
            while (current < count) {
                int left = (current << 1) + 1;
                int right = (current << 1) + 2;
                int swapIndex = current;
                if (left >= count) swapIndex = current;
                else if (right >= count) {
                    if (comparator.compare((TValue) elementsValue[left], value) < 0) swapIndex = left;
                } else {
                    if (comparator.compare((TValue) elementsValue[left], (TValue) elementsValue[right]) < 0) {
                        if (comparator.compare((TValue) elementsValue[left], value) < 0) swapIndex = left;
                    } else if (comparator.compare((TValue) elementsValue[right], value) < 0) swapIndex = right;
                }
                if (swapIndex == current) break;
                elementsValue[current] = elementsValue[swapIndex];
                elementsAttachment[current] = elementsAttachment[swapIndex];
                current = swapIndex;
            }
            elementsValue[current] = value;
            elementsAttachment[current] = attachments;
        } else {
            pop();
            add(value, attachments);
        }
    }

    /**
     * Return value of heap's top element.
     *
     * @return Value of heap's top element.
     * @throws IndexOutOfBoundsException Throw this exception if heap is empty.
     */
    @SuppressWarnings("unchecked")
    public TValue peekValue() throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty.");
        if (isHeap) {
            return (TValue) elementsValue[0];
        } else {
            return (TValue) elementsValue[count - 1];
        }
    }

    /**
     * Return attachments of heap's top element.
     *
     * @return Attachments of heap's top element.
     * @throws IndexOutOfBoundsException Throw this exception if heap is empty.
     */
    @SuppressWarnings("unchecked")
    public TAttachments peekAttachments() throws IndexOutOfBoundsException {
        if (count == 0) throw new IndexOutOfBoundsException("Heap is empty.");
        if (isHeap) {
            return (TAttachments) elementsAttachment[0];
        } else {
            return (TAttachments) elementsAttachment[count - 1];
        }
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
     * Return list with all attachments in heap.
     * @return List with all attachments in heap.
     */
    @SuppressWarnings("unchecked")
    public List<TAttachments> attachments() {
        ArrayList<TAttachments> returnList = new ArrayList<>();
        for (int i = 0; i < count; ++i) returnList.add((TAttachments)elementsAttachment[i]);
        return returnList;
    }

    /**
     * Return true if heap is empty.
     * @return True if heap is empty.
     */
    public boolean isEmpty() {
        return count == 0;
    }}

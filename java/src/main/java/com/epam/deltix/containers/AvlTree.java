/*
 * Copyright 2021 EPAM Systems, Inc
 *
 * See the NOTICE file distributed with this work for additional information
 * regarding copyright ownership. Licensed under the Apache License,
 * Version 2.0 (the "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.MemoryManager;

import java.util.ArrayList;
import java.util.Arrays;

/**
 * Represents an AVL Tree http://en.wikipedia.org/wiki/AVL_tree.
 */
public class AvlTree<TKey extends Comparable<TKey>, TValue>  {

    private AvlTreeNode<TKey, TValue> root;
    private int count;
    private int [] freeIndexes;
    private int freeIndexesHead = -1;

    private int [] prev, next;
    private int capacity = 0;
    private ArrayList<TKey> keys = new ArrayList<>();
    private ArrayList<TValue> values = new ArrayList<>();

    @SuppressWarnings("unchecked")
    APMemoryManager<AvlTreeNode<TKey, TValue>> avlTreeNodeAPMemoryManager = new APMemoryManager<AvlTreeNode<TKey, TValue>>(new MemoryManager.Creator<AvlTreeNode<TKey, TValue>>() {
        @Override
        public AvlTreeNode<TKey, TValue> create() {
            return new AvlTreeNode<TKey, TValue>();
        }

        @Override
        public AvlTreeNode<TKey, TValue>[] create(int size) {
            return new AvlTreeNode[size];
        }

    });

    private void rotateRight(AvlTreeNode<TKey, TValue> l, AvlTreeNode<TKey, TValue> r, AvlTreeNode<TKey, TValue> p)
    {
        if (p == null)
            root = l;
        else if (p.left == r)
            p.left = l;
        else
            p.right = l;

        r.left = l.right;
        l.right = r;

        int leftHeight = r.left == null ? 0 : r.left.height;
        int rightHeight = r.right == null ? 0 : r.right.height;
        r.height = 1 + Math.max(leftHeight, rightHeight);
        r.count = 1 + (r.left == null ? 0 : r.left.count) + (r.right == null ? 0 : r.right.count);

        leftHeight = l.left == null ? 0 : l.left.height;
        rightHeight = r.height;
        l.height = 1 + Math.max(leftHeight, rightHeight);
        l.count = 1 + (l.left == null ? 0 : l.left.count) + r.count;
    }

    private void rotateLeft(AvlTreeNode<TKey, TValue> l, AvlTreeNode<TKey, TValue> r, AvlTreeNode<TKey, TValue> p)
    {
        if (p == null)
            root = r;
        else if (p.left == l)
            p.left = r;
        else
            p.right = r;

        l.right = r.left;
        r.left = l;

        int leftHeight = l.left == null ? 0 : l.left.height;
        int rightHeight = l.right == null ? 0 : l.right.height;
        l.height = 1 + Math.max(leftHeight, rightHeight);
        l.count = 1 + (l.left == null ? 0 : l.left.count) + (l.right == null ? 0 : l.right.count);

        leftHeight = l.height;
        rightHeight = r.right == null ? 0 : r.right.height;
        r.height = 1 + Math.max(leftHeight, rightHeight);
        r.count = 1 + l.count + (r.right == null ? 0 : r.right.count);
    }

    private void leftFix(AvlTreeNode<TKey, TValue> node, AvlTreeNode<TKey, TValue> parent)
    {
        int leftHeight = node.left == null ? 0 : node.left.height;
        int rightHeight = node.right == null ? 0 : node.right.height;
        if (leftHeight > rightHeight + 1)
        {
            AvlTreeNode<TKey, TValue> l = node.left;
            leftHeight = l.left == null ? 0 : l.left.height;
            rightHeight = l.right == null ? 0 : l.right.height;
            if (rightHeight > leftHeight)
                rotateLeft(l, l.right, node);

            rotateRight(node.left, node, parent);
        }
        leftHeight = node.left == null ? 0 : node.left.height;
        rightHeight = node.right == null ? 0 : node.right.height;
        node.height = 1 + Math.max(leftHeight, rightHeight);
        node.count = 1 + (node.left == null ? 0 : node.left.count) + (node.right == null ? 0 : node.right.count);
    }

    private void rightFix(AvlTreeNode<TKey, TValue> node, AvlTreeNode<TKey, TValue> parent)
    {
        int leftHeight = node.left == null ? 0 : node.left.height;
        int rightHeight = node.right == null ? 0 : node.right.height;
        if (leftHeight + 1 < rightHeight)
        {
            AvlTreeNode<TKey, TValue> r = node.right;
            leftHeight = r.left == null ? 0 : r.left.height;
            rightHeight = r.right == null ? 0 : r.right.height;
            if (rightHeight < leftHeight)
                rotateRight(r.left, r, node);

            rotateLeft(node, node.right, parent);
        }
        leftHeight = node.left == null ? 0 : node.left.height;
        rightHeight = node.right == null ? 0 : node.right.height;
        node.height = 1 + Math.max(leftHeight, rightHeight);
        node.count = 1 + (node.left == null ? 0 : node.left.count) + (node.right == null ? 0 : node.right.count);
    }

    public int getHeight(TKey keyToSearch)
    {
        int cnt = 0;
        AvlTreeNode<TKey, TValue> node = root;
        while(node != null)
        {
            ++cnt;
            if (keyToSearch.compareTo(node.key) < 0)
            {
                node = node.left;
            }
            else if (keyToSearch.compareTo(node.key) > 0)
            {
                node = node.right;
            }
            else
                return cnt;
        }
        return -1;
    }

    private void resize() {
        if (capacity == 0)
        {
            capacity = 10;
            prev = new int[capacity];
            next = new int[capacity];
            freeIndexes = new int[capacity];
            while (values.size() < capacity) {
                values.add(null);
            }
            while (keys.size() < capacity) {
                keys.add(null);
            }
            for(int i = 0; i < capacity; i++)
            {
                freeIndexes[i] = i;
                prev[i] = next[i] = -1;
            }
            freeIndexesHead = capacity - 1;
        }
        else
        {
            capacity <<= 1;
            while (values.size() < capacity) {
                values.add(null);
            }
            while (keys.size() < capacity) {
                keys.add(null);
            }
            prev = Arrays.copyOf(prev, capacity);
            next = Arrays.copyOf(next, capacity);
            freeIndexes = Arrays.copyOf(freeIndexes, capacity);
            for(int i = (capacity >> 1); i < capacity; i++)
            {
                freeIndexes[++freeIndexesHead] = i;
                prev[i] = next[i] = -1;
            }
        }
    }

    private void add(AvlTreeNode<TKey, TValue> node, TKey keyToAdd, TValue valueToAdd, AvlTreeNode<TKey, TValue> parent)
    {
        if (keyToAdd.compareTo(node.key) < 0)
        {
            if (node.left == null)
            {
                if(freeIndexesHead == -1) resize();
                node.left = avlTreeNodeAPMemoryManager.getNew();
                node.left.initNode(keyToAdd, valueToAdd, freeIndexes[freeIndexesHead]);

                values.set(freeIndexes[freeIndexesHead], node.left.value);
                keys.set(freeIndexes[freeIndexesHead], node.left.key);

                freeIndexesHead--;
                if(prev[node.index] != -1)
                    next[prev[node.index]] = node.left.index;
                prev[node.left.index] = prev[node.index];
                prev[node.index] = node.left.index;
                next[node.left.index] = node.index;
            }
            else
                add(node.left, keyToAdd, valueToAdd, node);

            leftFix(node, parent);
        }
        else// if (keyToAdd.CompareTo(node.key) > 0)
        {
            if (node.right == null)
            {
                if(freeIndexesHead == -1) resize();
                node.right = avlTreeNodeAPMemoryManager.getNew();
                node.right.initNode(keyToAdd, valueToAdd, freeIndexes[freeIndexesHead]);

                values.set(freeIndexes[freeIndexesHead], node.right.value);
                keys.set(freeIndexes[freeIndexesHead], node.right.key);

                freeIndexesHead--;
                if(next[node.index] != -1) prev[next[node.index]] = node.right.index;
                next[node.right.index] = next[node.index];
                next[node.index] = node.right.index;
                prev[node.right.index] = node.index;

            }
            else
                add(node.right, keyToAdd, valueToAdd, node);

            rightFix(node, parent);
        }
    }

    private void remove(AvlTreeNode<TKey, TValue> node, TKey keyToRemove, AvlTreeNode<TKey, TValue> parent)
    {
        if (keyToRemove.compareTo(node.key) == 0)
        {
            if (node.left == null)
            {
                if (parent == null)
                {

                    if(node.right != null) prev[node.right.index] = -1;

                    root = node.right;
                }
                else if (parent.left == node)
                {
                    if(node.right != null && prev[node.right.index] == node.index)
                    {
                        prev[node.right.index] = prev[node.index];
                        if(prev[node.index] != -1)
                            next[prev[node.index]] = node.right.index;
                    }

                    if (prev[parent.index] == node.index)
                    {
                        prev[parent.index] = prev[node.index];
                        if(prev[node.index] != -1)
                            next[prev[node.index]] = parent.index;
                    }

                    parent.left = node.right;

                }
                else
                {
                    if(node.right != null && prev[node.right.index] == node.index)
                    {
                        prev[node.right.index] = prev[node.index];
                        if(prev[node.index] != -1)
                            next[prev[node.index]] = node.right.index;
                    }

                    if (next[parent.index] == node.index)
                    {
                        next[parent.index] = next[node.index];
                        if(next[node.index] != -1)
                            prev[next[node.index]] = parent.index;
                    }

                    parent.right = node.right;

                }
                next[node.index] = -1;
                prev[node.index] = -1;
                values.set(node.index, null);
                keys.set(node.index, null);
                freeIndexes[++freeIndexesHead] = node.index;
                avlTreeNodeAPMemoryManager.delete(node);
            }
            else if (node.right == null)
            {
                if (parent == null)
                {
                    next[node.left.index] = next[root.index];

                    root = node.left;
                }
                else if (parent.left == node)
                {
                    if(next[node.left.index] == node.index)
                    {
                        next[node.left.index] = next[node.index];
                        if(next[node.left.index] != -1)
                            prev[next[node.left.index]] = node.left.index;
                    }

                    if (prev[parent.index] == node.index)
                    {
                        prev[parent.index] = prev[node.index];
                        if(prev[node.index] != -1)
                            next[prev[node.index]] = parent.index;
                    }

                    parent.left = node.left;
                }
                else
                {
                    if(next[node.left.index] == node.index)
                    {
                        next[node.left.index] = next[node.index];
                        if(next[node.left.index] != -1)
                            prev[next[node.left.index]] = node.left.index;
                    }

                    if (next[parent.index] == node.index)
                    {
                        next[parent.index] = next[node.index];
                        if(next[node.index] != -1)
                            prev[next[node.index]] = parent.index;
                    }

                    parent.right = node.left;
                }
                next[node.index] = -1;
                prev[node.index] = -1;
                values.set(node.index, null);
                keys.set(node.index, null);
                freeIndexes[++freeIndexesHead] = node.index;
                avlTreeNodeAPMemoryManager.delete(node);
            }
            else
            {
                AvlTreeNode<TKey, TValue> tempNode = node.left;
                while (tempNode.right != null)
                    tempNode = tempNode.right;

                node.value = tempNode.value;
                node.key = tempNode.key;
                keys.set(node.index, node.key);
                values.set(node.index, node.value);

                tempNode.key = keyToRemove;
                remove(node.left, keyToRemove, node);

                rightFix(node, parent);
            }
        }
        else if (keyToRemove.compareTo(node.key) < 0)
        {
            if (node.left != null)
            {
                remove(node.left, keyToRemove, node);
                rightFix(node, parent);
            }
        }
        else
        {
            if (node.right != null)
            {
                remove(node.right, keyToRemove, node);
                leftFix(node, parent);
            }
        }
    }

    /**
     * Get iterator on first element according to sorting order.
     * @return Iterator on first element according to sorting order.
     */
    public int getFirst()
    {
        if (root == null)
            return -1;
        AvlTreeNode<TKey, TValue> node = root;
        while (node.left != null)
        {
            node = node.left;
        }
        return node.index;
    }

    /**
     * Get iterator on last element according to sorting order.
     * @return Iterator on last element according to sorting order.
     */
    public int getLast()
    {
        if (root == null)
            return -1;
        AvlTreeNode<TKey, TValue> node = root;
        while (node.right != null)
        {
            node = node.right;
        }
        return node.index;
    }

    public int getPrev(int iterator)
    {
        iterator = prev[iterator];
        return iterator;
    }

    public int getNext(int iterator)
    {
        iterator = next[iterator];
        return iterator;
    }

    public TValue getValueByIterator(int iterator)
    {
        return values.get(iterator);
    }

    public TKey getKeyByIterator(int iterator)
    {
        return keys.get(iterator);
    }

    /**
     * Get iterator on element with particular position in sorted array.
     * @param position Position in sorted array
     * @return Iterator on element with specified position or null if there is no such element.
     */
    public int getOrderStatisticIterator(int position)
    {
        if (root == null || position >= count)
            return -1;
        AvlTreeNode<TKey, TValue> node = root;
        while (true)
        {
            if (node.left != null)
            {
                if (node.left.count < position)
                {
                    position -= node.left.count + 1;
                    node = node.right;
                }
                else if (node.left.count > position)
                {
                    node = node.left;
                }
                else
                    return node.index;
            }
            else
            {
                if (position > 0)
                {
                    position--;
                    node = node.right;
                }
                else
                    return node.index;
            }
        }
    }

    /**
     * Get iterator on node which greater than specified part of elements in tree.
     * @param quantile Part or elements to excel.
     * @return Iterator on desired element or null if there is no such element.
     */
    public int getQuantileIterator(double quantile)
    {
        if (!(quantile >= 0 && quantile <= 1)) // Double.NaN checking style
            throw new IllegalArgumentException("The quantile argument value must be in interval [0, 1], but it actual value was " + quantile + ".");
        int y = Math.min((int)Math.floor(quantile * (double)count + 0.5), count - 1);
        return getOrderStatisticIterator(y);
    }

    /**
     * Add new element in tree
     * @param key Key of element.
     * @param value Value of element.
     */
    public void add(TKey key, TValue value)
    {
        if (root == null)
        {
            if(freeIndexesHead == -1) resize();
            root = avlTreeNodeAPMemoryManager.getNew();
            root.initNode(key, value, freeIndexes[freeIndexesHead]);

            values.set(freeIndexes[freeIndexesHead], root.value);
            keys.set(freeIndexes[freeIndexesHead], root.key);
            freeIndexesHead--;
        }
        else add(root, key, value, null);
        count = root.count;
    }


    /**
     * Remove element with specified element from tree.
     * @param key Key to remove.
     */
    public void remove(TKey key)
    {
        if (root != null)
            remove(root, key, null);

        if (root != null) count = root.count;
    }


    /**
     * Return number of elements in data structure.
     * @return Number of elements in data structure.
     */
    public int getCount()
    {
        return count;
    }

    private int search(AvlTreeNode<TKey, TValue> node, TKey keyToSearch)
    {
        if (keyToSearch.compareTo(node.key) < 0)
        {
            if (node.left != null)
            {
                node = node.left;
                return search(node, keyToSearch);
            }
            return -1;
        }
        else if (keyToSearch.compareTo(node.key) > 0)
        {
            if (node.right != null)
            {
                node = node.right;
                return search(node, keyToSearch);
            }
            return -1;
        }
        else
            return node.index;
    }

    private int searchLessOrEqual(AvlTreeNode<TKey, TValue> node, TKey keyToSearch)
    {
        if (keyToSearch.compareTo(node.key) < 0)
        {
            if (node.left != null)
            {
                node = node.left;
                return searchLessOrEqual(node, keyToSearch);
            }
            return prev[node.index];
        }
        else if (keyToSearch.compareTo(node.key) > 0)
        {
            if (node.right != null)
            {
                node = node.right;
                return searchLessOrEqual(node, keyToSearch);
            }
            return node.index;
        }
        else
            return node.index;
    }

    private int searchGreaterOrEqual(AvlTreeNode<TKey, TValue> node, TKey keyToSearch)
    {
        {
            if (keyToSearch.compareTo(node.key) < 0)
            {
                if (node.left != null)
                {
                    node = node.left;
                    return searchGreaterOrEqual(node, keyToSearch);
                }
                return node.index;
            }
            else if (keyToSearch.compareTo(node.key) > 0)
            {
                if (node.right != null)
                {
                    node = node.right;
                    return searchGreaterOrEqual(node, keyToSearch);
                }
                return next[node.index];
            }
            else
                return node.index;
        }
    }



    /**
     * Get iterator pointed on element with specified search key.
     * @param keyToSearch Key to search.
     * @return Iterator pointed on element with specified key, or null if no such element.
     */
    public int getIterator(TKey keyToSearch)
    {
        if (root == null)
            return -1;
        return search(root, keyToSearch);
    }

    /**
     * Get iterator pointed on element with greatest key less or equal than specified.
     * @param keyToSearch Key to search.
     * @return If we found element which satisfy condition, we return iterator, otherwise we return null.
     */
    public int getLessOrEqualIterator(TKey keyToSearch)
    {
        if (root == null)
            return -1;
        return searchLessOrEqual(root, keyToSearch);
    }


    /**
     * Get iterator pointed on element with least key greater or equal than specified.
     * @param keyToSearch Key to search.
     * @return If we found element which satisfy condition, we return iterator, otherwise we return null.
     */
    public int getGreaterOrEqualIterator(TKey keyToSearch)
    {
        if (root == null)
            return -1;
        return searchGreaterOrEqual(root, keyToSearch);
    }

    /**
     * Returns true if and only if tree contains element with specified key.
     * @param keyToSearch Key to search.
     * @return True if and only if tree contains element with specified key.
     */
    public boolean containsKey(TKey keyToSearch)
    {
        int it = getIterator(keyToSearch);
        if (it == -1)
            return false;
        else
            return true;
    }


    /**
     * Get value associated with specified key.
     * @param keyToSearch Key to search.
     * @return Value if we find key, null otherwise.
     */
    public TValue getValue(TKey keyToSearch)
    {
        int it = getIterator(keyToSearch);
        if (it == -1)
        {
            return null;
        }
        else
        {
            return getValueByIterator(it);
        }
    }

    /**
     * Return true if tree is empty.
     * @return True if tree is empty.
     */
    public boolean isEmpty() {
        return root == null;
    }
}
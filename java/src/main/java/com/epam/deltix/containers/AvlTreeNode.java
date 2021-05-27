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

/**
 * Represents an AVL Tree Node.
 * @param <TKey> Type of Search Key in AVL Tree.
 * @param <TValue> Type of Values in AVL Tree.
 */
public class AvlTreeNode<TKey extends Comparable<TKey>, TValue> {
    /**
     * Return reference for left child of this node.
     * @return Reference for left child of this node.
     */
    public AvlTreeNode<TKey, TValue> getLeft() {
        return left;
    }

    /**
     * Return reference for right child of this node.
     * @return Reference for right child of this node.
     */
    public AvlTreeNode<TKey, TValue> getRight() {
        return right;
    }

    /**
     * Return key of this node.
     * @return Key of this node.
     */
    public TKey getKey() {
        return key;
    }

    /**
     * Return number of element in subtree with root in this node.
     * @return Number of element in subtree with root in this node.
     */
    public int getCount() {
        return count;
    }

    /**
     * Return height of subtree with root this node.
     * @return height of subtree with root this node.
     */
    public int getHeight() {
        return height;
    }

    /**
     * Return value associated with this node.
     * @return Value associated with this node.
     */
    public TValue getValue() {
        return value;
    }

    /**
     * Return index associated with this node.
     * @return Index associated with this node.
     */
    public int getIndex()
    {
        return index;
    }

    /**
     * Initializes AVLTreeNode with key, value and index
     * @param key Key
     * @param value Value
     * @param index Index
     */
    public void initNode(TKey key, TValue value, int index) {
        this.value = value;
        this.key = key;
        this.height = 1;
        this.count = 1;
        this.left = null;
        this.right = null;
        this.index = index;
    }

    AvlTreeNode<TKey, TValue> left;
    AvlTreeNode<TKey, TValue> right;
    int count;
    int height;
    int index;
    TKey key;
    TValue value;

    AvlTreeNode() {}

    AvlTreeNode(TKey key, TValue value, int index) {
        initNode(key, value, index);
    }
}
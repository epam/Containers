package com.epam.deltix.containers;

import java.util.ArrayList;

/**
 * Avl Tree iterator.
 * @param <TKey> Type of Search Key in AVL Tree.
 * @param <TValue> Type of Values in AVL Tree.
 */
public class AvlTreeIterator<TKey extends Comparable<TKey>, TValue> {
    private ArrayList<AvlTreeNode<TKey, TValue>> stack;
    private int pos;

    /**
     * Return key associated with current node.
     * @return Key associated with current node.
     */
    public TKey getKey() {
        if (stack.size() > 0) return stack.get(stack.size() - 1).getKey();
        return null;
    }

    /**
     * Return value associated with current node.
     * @return Value associated with current node.
     */
    public TValue getValue() {
        if (stack.size() > 0) return stack.get(stack.size() - 1).getValue();
        return null;
    }

    /**
     * Return current node.
     * @return Current node.
     */
    public AvlTreeNode<TKey, TValue> getCurrentNode() {
        if (stack.size() > 0) return stack.get(stack.size() - 1);
        return null;
    }



    AvlTreeIterator(AvlTreeNode<TKey, TValue> node)
    {
        stack = new ArrayList<>();
        stack.add(node);
        pos = node.left != null ? node.left.count : 0;
    }

    /**
     * Return position of current node in array sorted by the searching key.
     * @return Position of current node in array sorted by the searching key.
     */
    public int getPosition()
    {
        return pos;
    }

    /**
     * Indicates whether iterator in valid state or node.
     * @return It may become invalid if you going higher than you were on the beginning.
     */
    public boolean isValid()
    {
        return stack.size() > 0;
    }


    /**
     * Move iterator to next node according to sorting order.
     */
    public void next()
    {
        if (stack.size() == 0)
            return;
        AvlTreeNode<TKey, TValue> node = stack.get(stack.size() - 1);
        pos++;
        if (node.right != null)
        {
            node = node.right;
            while (node.left != null)
            {
                stack.add(node);
                node = node.left;
            }
            stack.add(node);
        }
        else
        {
            stack.remove(stack.size() - 1);
            while (stack.size() > 0 && stack.get(stack.size() - 1).right == node)
            {
                node = stack.get(stack.size() - 1);
                stack.remove(stack.size() - 1);
            }
        }
    }

    /**
     * Move iterator to previous node according to sorting order.
     */
    public void prev()
    {
        if (stack.size() == 0)
            return;
        AvlTreeNode<TKey, TValue> node = stack.get(stack.size() - 1);
        pos--;
        if (node.left != null)
        {
            node = node.left;
            while (node.right != null)
            {
                stack.add(node);
                node = node.right;
            }
            stack.add(node);
        }
        else
        {
            stack.remove(stack.size() - 1);
            while (stack.size() > 0 && stack.get(stack.size() - 1).left == node)
            {
                node = stack.get(stack.size() - 1);
                stack.remove(stack.size() - 1);
            }
        }
    }

    /**
     * Move iterator to left child of current node.
     */
    public void goLeft()
    {
        if (stack.size() == 0)
            return;
        if (stack.get(stack.size() - 1).left != null)
        {
            pos--;
            stack.add(stack.get(stack.size() - 1).left);
            pos -= stack.get(stack.size() - 1).right == null ? 0 : stack.get(stack.size() - 1).right.count;
        }
    }

    /**
     * Move iterator to right child of current node.
     */
    public void goRight()
    {
        if (stack.size() == 0)
            return;
        if (stack.get(stack.size() - 1).right != null)
        {
            pos++;
            stack.add(stack.get(stack.size() - 1).right);
            pos += stack.get(stack.size() - 1).left == null ? 0 : stack.get(stack.size() - 1).left.count;
        }
    }

    /**
     * Return iterator to parent of current node.
     */
    public void goUp()
    {
        if (stack.size() == 0)
            return;
        if (stack.size() > 1)
        {
            AvlTreeNode<TKey, TValue> node = stack.get(stack.size() - 1);
            stack.remove(stack.size() - 1);
            if (stack.get(stack.size() - 1).left == node)
                pos += 1 + (node.right == null ? 0 : node.right.count);
            if (stack.get(stack.size() - 1).right == node)
                pos -= node.left == null ? 0 : node.left.count;
        }
    }


}

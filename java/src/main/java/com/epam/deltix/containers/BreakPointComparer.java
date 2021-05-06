package com.epam.deltix.containers;

import java.util.Comparator;

/**
 * Created by DriapkoA on 30/08/2017.
 */
public class BreakPointComparer implements Comparator<BreakPoint> {
    /**
     * Compares its two arguments for order.  Returns a negative integer,
     * zero, or a positive integer as the first argument is less than, equal
     * to, or greater than the second.<p>
     * In the foregoing description, the notation
     * <code>sgn(</code><i>expression</i><code>)</code> designates the mathematical
     * <i>signum</i> function, which is defined to return one of <code>-1</code>,
     * <code>0</code>, or <code>1</code> according to whether the value of
     * <i>expression</i> is negative, zero or positive.<p>
     * The implementor must ensure that <code>sgn(compare(x, y)) ==
     * -sgn(compare(y, x))</code> for all <code>x</code> and <code>y</code>.  (This
     * implies that <code>compare(x, y)</code> must throw an exception if and only
     * if <code>compare(y, x)</code> throws an exception.)<p>
     * The implementor must also ensure that the relation is transitive:
     * <code>((compare(x, y)&gt;0) &amp;&amp; (compare(y, z)&gt;0))</code> implies
     * <code>compare(x, z)&gt;0</code>.<p>
     * Finally, the implementor must ensure that <code>compare(x, y)==0</code>
     * implies that <code>sgn(compare(x, z))==sgn(compare(y, z))</code> for all
     * <code>z</code>.<p>
     * It is generally the case, but <i>not</i> strictly required that
     * <code>(compare(x, y)==0) == (x.equals(y))</code>.  Generally speaking,
     * any comparator that violates this condition should clearly indicate
     * this fact.  The recommended language is "Note: this comparator
     * imposes orderings that are inconsistent with equals."
     *
     * @param a the first object to be compared.
     * @param b the second object to be compared.
     * @return a negative integer, zero, or a positive integer as the
     * first argument is less than, equal to, or greater than the
     * second.
     * @throws NullPointerException if an argument is null and this
     *                              comparator does not permit null arguments
     * @throws ClassCastException   if the arguments' types prevent them from
     *                              being compared by this comparator.
     */
    @Override
    public int compare(BreakPoint a, BreakPoint b) {
        int x = Long.compare(a.getTime(), b.getTime());
        int y = Long.compare(a.getPriority(), b.getPriority());
        int z = Long.compare(a.getNumberOfMessage(), b.getNumberOfMessage());
        if (x != 0) return x;
        else if (y != 0) return y;
        else return z;
    }
}

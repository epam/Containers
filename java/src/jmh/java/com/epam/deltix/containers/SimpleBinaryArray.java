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

import java.util.Arrays;
import java.util.HashMap;

/**
 * Created by DriapkoA on 03.04.2017.
 */
public class SimpleBinaryArray {
    byte[] data;
    int hashCode;
    int count;

    private void resize(int newSize) {
        if (newSize >= data.length) {
            data = Arrays.copyOf(data, newSize << 1);
        }
    }

    public SimpleBinaryArray() {
        data = new byte[8];
        count = 0;
    }

    /**
     * Returns a hash code value for the object. This method is
     * supported for the benefit of hash tables such as those provided by
     * {@link HashMap}.
     * <p>
     * The general contract of {@code hashCode} is:
     * <ul>
     * <li>Whenever it is invoked on the same object more than once during
     * an execution of a Java application, the {@code hashCode} method
     * must consistently return the same integer, provided no information
     * used in {@code equals} comparisons on the object is modified.
     * This integer need not remain consistent from one execution of an
     * application to another execution of the same application.
     * <li>If two objects are equal according to the {@code equals(Object)}
     * method, then calling the {@code hashCode} method on each of
     * the two objects must produce the same integer result.
     * <li>It is <em>not</em> required that if two objects are unequal
     * according to the {@link Object#equals(Object)}
     * method, then calling the {@code hashCode} method on each of the
     * two objects must produce distinct integer results.  However, the
     * programmer should be aware that producing distinct integer results
     * for unequal objects may improve the performance of hash tables.
     * </ul>
     * <p>
     * As much as is reasonably practical, the hashCode method defined by
     * class {@code Object} does return distinct integers for distinct
     * objects. (This is typically implemented by converting the internal
     * address of the object into an integer, but this implementation
     * technique is not required by the
     * Java&trade; programming language.)
     *
     * @return a hash code value for this object.
     * @see Object#equals(Object)
     * @see System#identityHashCode
     */
    @Override
    public int hashCode() {
        int tempHashCode;
        if ((hashCode & 0x40000000) != 0) {
            tempHashCode = 0;
            for (int i = 0; i < count; ++i)
                tempHashCode = (tempHashCode * 31 + data[i]) & (0x3FFFFFFF);
            hashCode = tempHashCode;
        }
        return hashCode;
    }


    public int nonCachedHashCode() {
        int tempHashCode = 0;
        for (int i = 0; i < count; ++i)
            tempHashCode = (tempHashCode * 31 + data[i]) & (0x3FFFFFFF);
        return tempHashCode;
    }


    /**
     * Indicates whether some other object is "equal to" this one.
     * <p>
     * The {@code equals} method implements an equivalence relation
     * on non-null object references:
     * <ul>
     * <li>It is <i>reflexive</i>: for any non-null reference value
     * {@code x}, {@code x.equals(x)} should return
     * {@code true}.
     * <li>It is <i>symmetric</i>: for any non-null reference values
     * {@code x} and {@code y}, {@code x.equals(y)}
     * should return {@code true} if and only if
     * {@code y.equals(x)} returns {@code true}.
     * <li>It is <i>transitive</i>: for any non-null reference values
     * {@code x}, {@code y}, and {@code z}, if
     * {@code x.equals(y)} returns {@code true} and
     * {@code y.equals(z)} returns {@code true}, then
     * {@code x.equals(z)} should return {@code true}.
     * <li>It is <i>consistent</i>: for any non-null reference values
     * {@code x} and {@code y}, multiple invocations of
     * {@code x.equals(y)} consistently return {@code true}
     * or consistently return {@code false}, provided no
     * information used in {@code equals} comparisons on the
     * objects is modified.
     * <li>For any non-null reference value {@code x},
     * {@code x.equals(null)} should return {@code false}.
     * </ul>
     * <p>
     * The {@code equals} method for class {@code Object} implements
     * the most discriminating possible equivalence relation on objects;
     * that is, for any non-null reference values {@code x} and
     * {@code y}, this method returns {@code true} if and only
     * if {@code x} and {@code y} refer to the same object
     * ({@code x == y} has the value {@code true}).
     * <p>
     * Note that it is generally necessary to override the {@code hashCode}
     * method whenever this method is overridden, so as to maintain the
     * general contract for the {@code hashCode} method, which states
     * that equal objects must have equal hash codes.
     *
     * @param obj the reference object with which to compare.
     * @return {@code true} if this object is the same as the obj
     * argument; {@code false} otherwise.
     * @see #hashCode()
     * @see HashMap
     */
    @Override
    public boolean equals(Object obj) {
        if (obj instanceof SimpleBinaryArray) {
            SimpleBinaryArray other = (SimpleBinaryArray) obj;
            if (other.count != count) return false;
            for (int i = 0; i < other.count; ++i) if (other.data[i] != data[i]) return false;
            return true;
        }
        return false;
    }

    public boolean equalsByFour(SimpleBinaryArray other) {
        if (other.count != count) return false;
        int i;
        int count1 = count - 3;
        for (i = 0; i < count1; i += 4) {
            if (data[i] != other.data[i] || data[i + 1] != other.data[i + 1] || data[i + 2] != other.data[i + 2] || data[i + 3] != other.data[i + 3])
                return false;
        }
        int lastChecked = i + 4;
        for (i = lastChecked; i < count; ++i) {
            if (data[i] != other.data[i]) return false;
        }
        return true;
    }


    public SimpleBinaryArray append(byte x) {
        hashCode = hashCode | 0x40000000;
        resize(1 + count);
        data[count] = x;
        count++;
        return this;
    }

    public SimpleBinaryArray append(byte[] x) {
        hashCode = hashCode | 0x40000000;
        resize(x.length + count);
        for (int i = 0; i < x.length; ++i) {
            data[count] = x[i];
            count++;
        }
        return this;
    }

    public SimpleBinaryArray appendByCopy(byte[] x) {
        hashCode = hashCode | 0x40000000;
        resize(x.length + count);
        System.arraycopy(x, 0, data, count, x.length);
        return this;
    }

    public SimpleBinaryArray assign(SimpleBinaryArray array) {
        clear();
        resize(array.count + 1);
        count = array.count;
        hashCode = array.hashCode;
        System.arraycopy(array.data, 0, data, 0, array.count);
        return this;
    }

    public SimpleBinaryArray assignByCycle(SimpleBinaryArray array) {
        clear();
        resize(array.count + 1);
        count = array.count;
        hashCode = array.hashCode;
        for (int i = 0; i < array.count; ++i) data[i] = array.data[i];
        return this;
    }


    public SimpleBinaryArray clear() {
        for (int i = 0; i < data.length; ++i) data[i] = 0;
        count = 0;
        return this;
    }


}
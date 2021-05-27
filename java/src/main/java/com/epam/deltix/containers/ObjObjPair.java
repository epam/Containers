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

import com.epam.deltix.containers.interfaces.ObjObjPairReadOnly;

import java.util.Objects;

/**
 * Created by DriapkoA on 06/11/2017.
 */
public class ObjObjPair<T1, T2> implements ObjObjPairReadOnly<T1, T2> {
    /**
     * Get second item of this pair.
     *
     * @return Second item of this pair.
     */
    @Override
    public T2 getSecond() {
        return second;
    }

    /**
     * Set second item of this pair to new value.
     *
     * @param second New value of second item.
     */
    public void setSecond(T2 second) {
        this.second = second;
    }

    /**
     * Get first item of this pair.
     *
     * @return First item of this pair.
     */
    @Override
    public T1 getFirst() {
        return first;
    }

    /**
     * Set first item of this pair to new value.
     *
     * @param first New value of first item.
     */
    public void setFirst(T1 first) {
        this.first = first;
    }

    T1 first;
    T2 second;


    /**
     * Init this pair by data.
     *
     * @param first  New value of first item.
     * @param second New value of second item.
     */
    public void init(T1 first, T2 second) {
        this.first = first;
        this.second = second;
    }

    /**
     * Create new instance of pair.
     *
     * @param first  New value of first item.
     * @param second New value of second item.
     */
    public ObjObjPair(T1 first, T2 second) {
        this.first = first;
        this.second = second;
    }

    /**
     * Create new instance of pair without initialization.
     */
    public ObjObjPair() {

    }

    @Override
    public void copyTo(ObjObjPair<T1, T2> destination) {
        destination.init(first, second);
    }

    @Override
    public ObjObjPair<T1, T2> clone() {
        ObjObjPair<T1, T2> destination = new ObjObjPair<>();
        copyTo(destination);
        return destination;
    }


    @Override
    public int hashCode() {
        int hash = 0;
        if (second != null) hash = second.hashCode() * 31;
        if (first != null) hash ^= first.hashCode();
        return hash;
    }

    @Override
    public String toString() {
        MutableString builder = new MutableString();
        builder.append('(');
        if (first == null) builder.append("null, ");
        else builder.append(first.toString()).append(", ");
        if (second == null) builder.append("null");
        else builder.append(second.toString());
        return builder.append(')').toString();
    }

    @Override
    public boolean equals(Object other) {
        if (other instanceof ObjObjPairReadOnly) {
            ObjObjPairReadOnly pair = (ObjObjPairReadOnly) other;
            return Objects.equals(first, pair.getFirst()) && Objects.equals(second, pair.getSecond());
        }
        return false;
    }

}
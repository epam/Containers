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
package com.epam.deltix.containers.interfaces;

import java.util.ArrayList;

/**
 * ReadOnly interface for ArrayList.
 */
public interface ListReadOnly<T> {
    /**
     * Returns the number of elements in this list.
     *
     * @return The number of elements in this list.
     */

    public int size();


    /**
     * Returns the element at the specified position in this list.
     *
     * @param index Index of the element to return
     * @return The element at the specified position in this list.
     * @throws IndexOutOfBoundsException If the index is out of range
     *                                   (<code>index &lt; 0 || index &gt;= size()</code>)
     */
    public T get(int index);
}
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

import java.lang.reflect.Array;

class ReflectionObjectCreator<T> implements MemoryManager.Creator<T> {
    private final Class<T> clazz;

    ReflectionObjectCreator(Class<T> clazz) {
        this.clazz = clazz;
    }

    @Override
    public T create() {
        try {
            return clazz.newInstance();
        } catch (Exception e) {
            throw new RuntimeException("Error in creating object of class " + clazz.getSimpleName(), e);
        }
    }

    @SuppressWarnings("unchecked")
    @Override
    public T[] create(int size) {
        return (T[]) Array.newInstance(clazz, size);
    }
}
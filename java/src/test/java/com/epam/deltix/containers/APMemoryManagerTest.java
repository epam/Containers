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

import org.junit.Test;

import static org.junit.Assert.assertNotNull;

public class APMemoryManagerTest {
    private final static int NUMBER_OF_TRIALS = 1000;

    @Test
    public void basic() throws InstantiationException, IllegalAccessException {
        final APMemoryManager<BinaryArray> memoryManager = new APMemoryManager<>(BinaryArray.class);

        // Check out.
        final BinaryArray[] objects = new BinaryArray[NUMBER_OF_TRIALS];
        for (int i = 0; i < objects.length; i += 1) {
            objects[i] = memoryManager.getNew();
            assertNotNull(objects[i]);
        }

        // Check in.
        for (int i = 0; i < objects.length; i += 1)
            memoryManager.delete(objects[i]);
    }

    @Test
    public void removeToEmptyPool() {
        final APMemoryManager<BinaryArray> apMemoryManager = new APMemoryManager<>(BinaryArray.class);
        final GPMemoryManager<BinaryArray> gpMemoryManager = new GPMemoryManager<>(BinaryArray.class);

        apMemoryManager.delete(new BinaryArray());
        gpMemoryManager.delete(new BinaryArray());
        assertNotNull(apMemoryManager.getNew());
        assertNotNull(gpMemoryManager.getNew());
        assertNotNull(apMemoryManager.getNew());
        assertNotNull(gpMemoryManager.getNew());
    }
}
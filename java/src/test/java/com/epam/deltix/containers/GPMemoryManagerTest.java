package com.epam.deltix.containers;

import org.junit.Test;

import static org.junit.Assert.assertNotNull;

public class GPMemoryManagerTest {
    private final static int NUMBER_OF_TRIALS = 1000;

    @Test
    public void basic() throws InstantiationException, IllegalAccessException {
        final GPMemoryManager<BinaryArray> memoryManager = new GPMemoryManager<>(BinaryArray.class);

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
}

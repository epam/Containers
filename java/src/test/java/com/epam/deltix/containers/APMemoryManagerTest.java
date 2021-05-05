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

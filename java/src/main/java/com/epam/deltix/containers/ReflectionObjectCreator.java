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

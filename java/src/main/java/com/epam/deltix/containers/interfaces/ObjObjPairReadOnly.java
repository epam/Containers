package com.epam.deltix.containers.interfaces;

import com.epam.deltix.containers.ObjObjPair;

/**
 * Created by DriapkoA on 06/11/2017.
 */
public interface ObjObjPairReadOnly<T1, T2> {
    /**
     * Get second item of this pair.
     *
     * @return Second item of this pair.
     */
    public T2 getSecond();

    /**
     * Get first item of this pair.
     *
     * @return First item of this pair.
     */
    public T1 getFirst();

    public void copyTo(ObjObjPair<T1, T2> destination);

}

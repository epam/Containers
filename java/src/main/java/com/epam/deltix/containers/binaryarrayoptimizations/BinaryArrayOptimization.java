package com.epam.deltix.containers.binaryarrayoptimizations;

/**
 * Created by DriapkoA on 18/04/2017.
 */
public interface BinaryArrayOptimization {
    long xxHash64(long[] input, long length);

    boolean equals(long[] data1, long[] data2, int count);
}

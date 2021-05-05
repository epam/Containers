package com.epam.deltix.containers.interfaces;

public interface BinaryArrayReadOnly {
    default void getBytes(byte[] buffer) { getBytes(buffer, 0, 0, size());}
    default void getBytes(byte[] buffer, int srcOffset, int size) {getBytes(buffer, 0, 0, size);}
    default void getBytes(byte[] buffer, int srcOffset, int dstOffset, int size) { /*TODO*/ }

    /*TODO: Strictly recommended to override*/
    BinaryArrayReadOnly clone();

    /*TODO: implement only these methods*/
    byte get(int index);
    int size();
    int capacity();
}

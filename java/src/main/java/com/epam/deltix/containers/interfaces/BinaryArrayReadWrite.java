package com.epam.deltix.containers.interfaces;

public interface BinaryArrayReadWrite extends BinaryArrayReadOnly {

    default BinaryArrayReadWrite assign(byte[] bytes) { return clear().append(bytes); }
    default BinaryArrayReadWrite append(byte[] bytes)  { /*TODO*/ return this; }

    default BinaryArrayReadWrite assign(byte[] bytes, int offset, int count) { return clear().append(bytes, 0, count); }
    default BinaryArrayReadWrite append(byte[] bytes, int offset, int count) { /*TODO*/ return this; }

    default BinaryArrayReadWrite assign(BinaryArrayReadOnly str) { return clear().append(str); }
    default BinaryArrayReadWrite append(BinaryArrayReadOnly str) { /*TODO*/ return this; }

    /*TODO: Strictly recommended to override*/
    @Override
    BinaryArrayReadWrite clone();

    /*TODO: implement only these methods*/
    BinaryArrayReadWrite set(int index, byte x);
    BinaryArrayReadWrite append(byte b);
    BinaryArrayReadWrite clear();

}

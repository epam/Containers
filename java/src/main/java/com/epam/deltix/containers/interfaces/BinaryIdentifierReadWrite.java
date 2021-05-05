package com.epam.deltix.containers.interfaces;

/**
 * Created by VavilauA on 4/7/2017.
 */
public interface BinaryIdentifierReadWrite extends BinaryArrayReadWrite, BinaryIdentifierReadOnly {
    default BinaryIdentifierReadWrite assign(CharSequence str, boolean isASCII) { return clear().append(str, false); }
    default BinaryIdentifierReadWrite append(CharSequence str, boolean isASCII)  { /*TODO*/ return this; }

    default BinaryIdentifierReadWrite assign(long buffer) { return clear().append(buffer); }
    default BinaryIdentifierReadWrite append(long buffer) {
        append((byte)(buffer & 255));
        append((byte)((buffer >>> 8) & 255));
        append((byte)((buffer >>> 16) & 255));
        append((byte)((buffer >>> 24) & 255));
        append((byte)((buffer >>> 32) & 255));
        append((byte)((buffer >>> 40) & 255));
        append((byte)((buffer >>> 48) & 255));
        append((byte)(buffer >>> 56));
        return this;
    }

    default BinaryIdentifierReadWrite assign(byte[] bytes) { return clear().append(bytes); }
    default BinaryIdentifierReadWrite append(byte[] bytes)  { /*TODO*/ return this; }

    default BinaryIdentifierReadWrite assign(byte[] bytes, int offset, int count) { return clear().append(bytes, 0, count); }
    default BinaryIdentifierReadWrite append(byte[] bytes, int offset, int count) { /*TODO*/ return this; }

    default BinaryIdentifierReadWrite assign(BinaryArrayReadOnly str) { return clear().append(str); }
    default BinaryIdentifierReadWrite append(BinaryArrayReadOnly str) { /*TODO*/ return this; }

    /*TODO: Strictly recommended to override*/
    @Override
    BinaryIdentifierReadWrite clone();

    /*TODO: implement only these methods*/
    BinaryIdentifierReadWrite set(int index, byte x);
    BinaryIdentifierReadWrite append(byte b);
    BinaryIdentifierReadWrite clear();

}

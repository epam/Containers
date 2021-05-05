package com.epam.deltix.containers.interfaces;

/**
 * Created by VavilauA on 4/7/2017.
 */
public interface BinaryIdentifierReadOnly extends BinaryArrayReadOnly{
    default long toLong() { return toLong(0);}
    default long toLong(int offset) { /*TODO*/ return 0;}

    // We will use Appendable instead of MutableString
    default void getChars(CharEncoding charset, Appendable str) { getChars(charset, str, 0); }
    default void getChars(CharEncoding charset, Appendable str, int offset) { /*TODO*/ }

    default String toString(CharEncoding charset, int offset) { /*TODO*/ return null; }
    default String toString(CharEncoding charset) { return toString(charset, 0);}

    /*TODO: Strictly recommended to override*/
    boolean equals(BinaryIdentifierReadOnly another);
    int hashCode();
    String toString();
    @Override
    BinaryIdentifierReadOnly clone();
}

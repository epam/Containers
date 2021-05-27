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
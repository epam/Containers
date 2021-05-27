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
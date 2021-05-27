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
package com.epam.deltix.containers;

import org.openjdk.jmh.annotations.*;
import org.openjdk.jmh.runner.Runner;
import org.openjdk.jmh.runner.RunnerException;
import org.openjdk.jmh.runner.options.Options;
import org.openjdk.jmh.runner.options.OptionsBuilder;

import java.util.HashMap;
import java.util.Random;
import java.util.concurrent.TimeUnit;

/**
 * Created by DriapkoA on 03.04.2017.
 */
@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.NANOSECONDS)
@Warmup(time = 5, timeUnit = TimeUnit.SECONDS, iterations = 1)
@Measurement(time = 5, timeUnit = TimeUnit.SECONDS, iterations = 1)
@State(Scope.Thread)
public class BinaryArrayBenchmark {
    SimpleBinaryArray simpleBinaryArray = new SimpleBinaryArray();
    SimpleBinaryArray simpleBinaryArray1 = new SimpleBinaryArray();
    SimpleBinaryArray simpleBinaryArray2 = new SimpleBinaryArray();
    TestBinaryArray longsBinaryArray = new TestBinaryArray();
    TestBinaryArray longsBinaryArray1 = new TestBinaryArray();
    TestBinaryArray longsBinaryArray2 = new TestBinaryArray();
    BinaryArray binaryArray = new BinaryArray();
    BinaryArray binaryArray1 = new BinaryArray();
    BinaryArray binaryArray2 = new BinaryArray();
    BinaryArray[] binaryArraysLongIds;
    BinaryArray[] binaryArraysIntIds;
    BinaryArray[] binaryArraysStringIds;
    BinaryArray[] binaryArraysASCIIStringIds;

    long[] longIds;
    int[] intIds;
    String[] stringIds;


    byte[] array;

    int COUNT_IDS = 1000000;

    @Param({"8", "16", "30", "200", "2000"})
    int SIZE = 8;


    @Setup(Level.Trial)
    public void setUp() {
        final Random random = new Random(55);
        array = new byte[SIZE];
        random.nextBytes(array);
        simpleBinaryArray1.append(array);
        longsBinaryArray1.append(array);
        longsBinaryArray2.assign(longsBinaryArray1);
        simpleBinaryArray2.assign(simpleBinaryArray1);

        binaryArray1.append(array);
        binaryArray2.assign(binaryArray1);

        longIds = new long[COUNT_IDS];
        binaryArraysLongIds = new BinaryArray[COUNT_IDS];
        for (int i = 0; i < COUNT_IDS; ++i) {
            int x = random.nextInt(900000000) + 100000000;
            longIds[i] = x;
            binaryArraysLongIds[i] = new BinaryArray();
            binaryArraysLongIds[i].assign(longIds[i]);
        }

        intIds = new int[COUNT_IDS];
        binaryArraysIntIds = new BinaryArray[COUNT_IDS];
        for (int i = 0; i < COUNT_IDS; ++i) {
            int x = random.nextInt(900000000) + 100000000;
            intIds[i] = x;
            binaryArraysIntIds[i] = new BinaryArray();
            binaryArraysIntIds[i].assign(intIds[i]);
        }

        stringIds = new String[COUNT_IDS];
        binaryArraysStringIds = new BinaryArray[COUNT_IDS];
        binaryArraysASCIIStringIds = new BinaryArray[COUNT_IDS];
        for (int i = 0; i < COUNT_IDS; ++i) {
            int x = random.nextInt(900000000) + 100000000;
            stringIds[i] = Integer.toString(x);
            binaryArraysStringIds[i] = new BinaryArray();
            binaryArraysStringIds[i].assign(stringIds[i], false);
            binaryArraysASCIIStringIds[i] = new BinaryArray();
            binaryArraysASCIIStringIds[i].assign(stringIds[i], true);


        }
    }


    @Benchmark
    public long binaryArrayIdIntsOnlySearch() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        for (int i = 0; i < COUNT_IDS; ++i) {
            dictionary.put(binaryArraysIntIds[i], true);
        }
        return dictionary.size();
    }


    @Benchmark
    public long binaryArrayIdLongsOnlySearch() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        for (int i = 0; i < COUNT_IDS; ++i) {
            dictionary.put(binaryArraysLongIds[i], true);
        }
        return dictionary.size();
    }

    @Benchmark
    public long binaryArrayIdASCIIStringOnlySearch() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        for (int i = 0; i < COUNT_IDS; ++i) {
            dictionary.put(binaryArraysASCIIStringIds[i], true);
        }
        return dictionary.size();
    }

    @Benchmark
    public long binaryArrayIdStringOnlySearch() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        for (int i = 0; i < COUNT_IDS; ++i) {
            dictionary.put(binaryArraysStringIds[i], true);
        }
        return dictionary.size();
    }


    @Benchmark
    public long binaryArrayIdInts() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        BinaryArray ar = new BinaryArray();
        for (int i = 0; i < COUNT_IDS; ++i) {
            ar.assign(intIds[i]);
            dictionary.put(ar, true);
        }
        return dictionary.size();
    }


    @Benchmark
    public long binaryArrayIdLongs() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        BinaryArray ar = new BinaryArray();
        for (int i = 0; i < COUNT_IDS; ++i) {
            ar.assign(longIds[i]);
            dictionary.put(ar, true);
        }
        return dictionary.size();
    }

    @Benchmark
    public long binaryArrayIdASCIIString() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        BinaryArray ar = new BinaryArray();
        for (int i = 0; i < COUNT_IDS; ++i) {
            ar.assign(stringIds[i], true);
            dictionary.put(ar, true);
        }
        return dictionary.size();
    }

    @Benchmark
    public long binaryArrayIdString() {
        HashMap<BinaryArray, Boolean> dictionary = new HashMap<BinaryArray, Boolean>();
        BinaryArray ar = new BinaryArray();
        for (int i = 0; i < COUNT_IDS; ++i) {
            ar.assign(stringIds[i], false);
            dictionary.put(ar, true);
        }
        return dictionary.size();
    }


    @Benchmark
    public BinaryArray binaryArrayAppend() {
        return binaryArray.append(array).clear();
    }

    @Benchmark
    public boolean binaryArrayEquals() {
        return binaryArray1.equals(binaryArray2);
    }

    @Benchmark
    public BinaryArray binaryArrayAssign() {
        return binaryArray2.assign(binaryArray1);
    }

    @Benchmark
    public BinaryArray binaryArraySetByte() {
        for (int i = 0; i < binaryArray1.getCount(); ++i) binaryArray1.setByteAt(i, binaryArray1.getByteAt(i));
        return binaryArray1;
    }
    ///


    @Benchmark
    public TestBinaryArray unsafeLongsBinaryArrayAppend() {
        return longsBinaryArray.unsafeAppendByteArray(array).clear();
    }


    /*  //@Benchmark
      public TestBinaryArray nativeBinaryArrayAppend() {
          return longsBinaryArray.nativeAppendByteArray(array).clear();
      }
  */
    @Benchmark
    public TestBinaryArray longsBinaryArrayAppend() {
        return longsBinaryArray.append(array).clear();
    }

    @Benchmark
    public boolean longsBinaryArrayEquals() {
        return longsBinaryArray1.equals(longsBinaryArray2);
    }

    @Benchmark
    public TestBinaryArray longsBinaryArrayAssign() {
        return longsBinaryArray2.assign(longsBinaryArray1);
    }

    @Benchmark
    public int XXHashCode() {
        return longsBinaryArray1.nonCachedHashCode();
    }

    ///

    @Benchmark
    public int simpleBinaryArrayHashCode() {
        return simpleBinaryArray1.nonCachedHashCode();
    }

    @Benchmark
    public SimpleBinaryArray simpleBinaryArrayAppend() {
        return simpleBinaryArray.append(array).clear();
    }

    @Benchmark
    public SimpleBinaryArray simpleBinaryArrayAppendByCopy() {
        return simpleBinaryArray.appendByCopy(array).clear();
    }


    @Benchmark
    public boolean simpleBinaryArrayEquals() {
        return simpleBinaryArray1.equals(simpleBinaryArray2);
    }

    @Benchmark
    public boolean simpleBinaryArrayEqualsByFour() {
        return simpleBinaryArray1.equalsByFour(simpleBinaryArray2);
    }


    @Benchmark
    public SimpleBinaryArray simpleBinaryArrayAssign() {
        return simpleBinaryArray2.assign(simpleBinaryArray1);
    }

    @Benchmark
    public SimpleBinaryArray simpleBinaryArrayAssignByCycle() {
        return simpleBinaryArray2.assignByCycle(simpleBinaryArray1);
    }


    public static void main(String[] args) throws RunnerException {
        BinaryArrayBenchmark b = new BinaryArrayBenchmark();
        b.setUp();
        b.binaryArrayIdASCIIStringOnlySearch();
        Options opt = new OptionsBuilder()
                .include(".*" + BinaryArrayBenchmark.class.getSimpleName() + ".*")
                .forks(1)
                .build();
        new Runner(opt).run();
    }


}
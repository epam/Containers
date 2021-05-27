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

import com.epam.deltix.containers.interfaces.Consumer;
import com.epam.deltix.containers.generated.LongObjPair;
import org.junit.Assert;
import org.junit.Test;

import java.util.ArrayList;
import java.util.Random;

/**
 * Created by DriapkoA on 10/01/2018.
 */
public class TimeProviderTest {

    @Test
    public void priorityTest() {
        final StringBuilder sb = new StringBuilder();
        ManualTimeProvider timeProvider = new ManualTimeProvider();
        timeProvider.addBreakPoint(10, 10, new Consumer<LongObjPair>() {

            /**
             * Performs this operation on the given argument.
             *
             * @param longObjPair the input argument
             */
            @Override
            public void accept(LongObjPair longObjPair) {
                sb.append(longObjPair.getSecond());
            }
        }, 2);

        timeProvider.addBreakPoint(10, 11, new Consumer<LongObjPair>() {

            /**
             * Performs this operation on the given argument.
             *
             * @param longObjPair the input argument
             */
            @Override
            public void accept(LongObjPair longObjPair) {
                sb.append(longObjPair.getSecond());
            }
        }, 1);

        timeProvider.goToTime(20);

        Assert.assertEquals("1110", sb.toString());
    }

    @Test
    public void stressTest() {
        final ManualTimeProvider timeProvider = new ManualTimeProvider();
        ArrayList<Long> keys = new ArrayList<>();
        final Random rand = new Random(55);
        int lastTime = 0;
        for (int i = 0; i < 100000; ++i) {
            for (int j = 0; j < 5; j++)
                keys.add(timeProvider.addBreakPoint(i, i, new Consumer<LongObjPair>() {
                    @Override
                    public void accept(LongObjPair longObjPair) {
                        if ((Math.abs(rand.nextInt()) % 20) == 0) {
                            timeProvider.addBreakPoint(longObjPair.getFirst(), longObjPair.getSecond(), new Consumer<LongObjPair>() {
                                @Override
                                public void accept(LongObjPair longObjPair) {

                                }
                            }, 1);
                        }
                    }
                }, 1));

            if (Math.abs(rand.nextInt()) % 2000 == 0) {
                timeProvider.goToTime(i);
                lastTime = i;
            } else if (Math.abs(rand.nextInt()) % 250 == 0) {
                timeProvider.deleteBreakPoint(keys.get(Math.abs(rand.nextInt(i - lastTime))));
            }

        }
    }
}
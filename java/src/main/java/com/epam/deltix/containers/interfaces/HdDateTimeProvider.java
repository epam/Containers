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

import com.epam.deltix.containers.generated.LongObjPair;
import com.epam.deltix.hdtime.HdDateTime;

/**
 * Interface for TimeProvider(TimeProvider work with time. Allow us to get current time, to set breakpoint and etc).
 * We suppose that implementation of this TimeProvider does'nt calls callbacks in addBreakpoint method.
 * Callbacks with lesser time called earlier. If time the same than callbacks with lesser priority called earlier.
 */
public interface HdDateTimeProvider {
    /**
     * Get current time from this provider.
     *
     * @return Current time from this provider.
     */
    HdDateTime getCurrentTime();

    /**
     * Add new breakpoint to this provider.
     *
     * @param time     Time of new breakpoint.
     * @param context  Context of new breakpoint.
     * @param action   Action which will be called on new breakpoint.
     * @param priority Priority of new breakpoint (breakpoints with lesser priority with same time called earlier).
     * @return Key of this breakpoint or -1 if we can't add breakpoint(time less than current time).
     */
    long addBreakPoint(HdDateTime time, Object context, Consumer<LongObjPair> action, int priority);

    /**
     * Delete breakpoint with key from this provider.
     *
     * @param key Key of breakpoint.
     * @return True if there is such key in TimeProvider.
     */
    boolean deleteBreakPoint(long key);

}
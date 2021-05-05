package com.epam.deltix.containers.interfaces;

import com.epam.deltix.containers.generated.LongObjPair;


/**
 * Interface for TimeProvider(TimeProvider work with time. Allow us to get current time, to set breakpoint and etc).
 * We suppose that implementation of this TimeProvider does'nt calls callbacks in addBreakpoint method.
 * Callbacks with lesser time called earlier. If time the same than callbacks with lesser priority called earlier.
 */
public interface TimeProvider {

    /**
     * Get current time from this provider.
     *
     * @return Current time from this provider.
     */
    long getCurrentTime();

    /**
     * Add new breakpoint to this provider.
     *
     * @param time     Time of new breakpoint.
     * @param context  Context of new breakpoint.
     * @param action   Action which will be called on new breakpoint.
     * @param priority Priority of new breakpoint (breakpoints with lesser priority with same time called earlier).
     * @return Key of this breakpoint or -1 if we can't add breakpoint(time less than current time).
     */
    long addBreakPoint(long time, Object context, Consumer<LongObjPair> action, int priority);

    /**
     * Delete breakpoint with key from this provider.
     *
     * @param key Key of breakpoint.
     * @return True if there is such key in TimeProvider.
     */
    boolean deleteBreakPoint(long key);


}

package com.epam.deltix.containers;

import com.epam.deltix.containers.generated.LongObjPair;
import com.epam.deltix.containers.interfaces.Consumer;
import com.epam.deltix.containers.interfaces.MemoryManager;
import com.epam.deltix.containers.interfaces.TimeProvider;

/**
 * Simple implementation of TimeProvider interface.
 */
public class ManualTimeProvider implements TimeProvider {
    static BreakPointComparer comparer = new BreakPointComparer();
    HeapWithIndices<BreakPoint> breakPoints = new HeapWithIndices<BreakPoint>(comparer);
    APMemoryManager<BreakPoint> memoryManager = new APMemoryManager<BreakPoint>(new MemoryManager.Creator<BreakPoint>() {
        @Override
        public BreakPoint create() {
            return new BreakPoint();
        }

        @Override
        public BreakPoint[] create(int size) {
            return new BreakPoint[size];
        }
    });
    private long key = 0;
    private long currentTime = 0;
    boolean needPop = false;
    LongObjPair pairToConsumer = new LongObjPair();

    /**
     * Get current time from this provider.
     *
     * @return Current time from this provider.
     */
    @Override
    public long getCurrentTime() {
        return currentTime;
    }


    /**
     * Move current time to new time.
     *
     * @param time New time of time provider.
     */
    public void goToTime(long time) {
        while (breakPoints.getCount() > 0 && breakPoints.peekValue().getTime() <= time) {
            goToNextBreakPoint(time);
        }

        currentTime = time;
    }


    /**
     * Move current time to time of next breakpoint.
     */
    public void goToNextBreakPoint() {
        goToNextBreakPoint(Long.MAX_VALUE);
    }


    void goToNextBreakPoint(long destinationTime) {
        if (needPop) {
            memoryManager.delete(breakPoints.peekValue());
            breakPoints.pop();
        }
        if (breakPoints.getCount() == 0) {
            needPop = false;
            return;
        }
        BreakPoint breakPoint = breakPoints.peekValue();
        if (breakPoint.getTime() > destinationTime) {
            needPop = false;
            return;
        }
        needPop = true;
        long numberOfMessage = breakPoint.getNumberOfMessage();
        currentTime = breakPoint.getTime();
        int key = (int) breakPoint.getKey();
        pairToConsumer.setFirst(currentTime);
        pairToConsumer.setSecond(breakPoint.getCustomData());
        breakPoint.getAction().accept(pairToConsumer);
    }


    /**
     * Add new breakpoint to this provider.
     *
     * @param time     Time of new breakpoint.
     * @param context  Context of new breakpoint.
     * @param action   Action which will be called on new breakpoint.
     * @param priority Priority of new breakpoint (breakpoints with lesser priority with same time called earlier).
     * @return Key of this breakpoint or -1 if breakpoint is incorrect.
     */
    @Override
    public long addBreakPoint(long time, Object context, Consumer<LongObjPair> action, int priority) {
        if (time < currentTime) return -1;
        key++;
        BreakPoint breakPoint;
        try {
            breakPoint = memoryManager.getNew();
        } catch (Exception e) {
            breakPoint = new BreakPoint();
        }
        breakPoint.setTime(time);
        breakPoint.setCustomData(context);
        breakPoint.setPriority(priority);
        breakPoint.setAction(action);
        breakPoint.setNumberOfMessage(key);
        int _key;
        if (needPop) {
            memoryManager.delete(breakPoints.peekValue());
            _key = breakPoints.peekKey();
            breakPoints.modifyTop(breakPoint);
            needPop = false;
        } else {
            _key = breakPoints.add(breakPoint);
        }
        breakPoint.setKey(_key);
        return _key;
    }

    /**
     * Delete breakpoint with key from this provider.
     *
     * @param key Key of breakpoint.
     * @return True if there is such key in TimeProvider.
     */
    @Override
    public boolean deleteBreakPoint(long key) {
        if (needPop) {
            memoryManager.delete(breakPoints.peekValue());
            breakPoints.pop();
            needPop = false;
        }
        BreakPoint breakPoint = breakPoints.getValue((int) key, null);
        if (breakPoint == null) return false;
        breakPoints.remove((int) key);
        memoryManager.delete(breakPoint);
        return true;
    }
}
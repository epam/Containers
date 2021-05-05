package com.epam.deltix.containers;

import com.epam.deltix.containers.generated.LongObjPair;

import com.epam.deltix.containers.interfaces.Consumer;

class BreakPoint {
    public Object getCustomData() {
        return customData;
    }

    public void setCustomData(Object customData) {
        this.customData = customData;
    }

    private Object customData;

    public long getTime() {
        return time;
    }

    public void setTime(long time) {
        this.time = time;
    }

    private long time;

    public long getKey() {
        return key;
    }

    public void setKey(long key) {
        this.key = key;
    }

    private long key;

    public long getPriority() {
        return priority;
    }

    public void setPriority(long priority) {
        this.priority = priority;
    }

    private long priority;

    public long getNumberOfMessage() {
        return numberOfMessage;
    }

    public void setNumberOfMessage(long numberOfMessage) {
        this.numberOfMessage = numberOfMessage;
    }

    private long numberOfMessage;

    public Consumer<LongObjPair> getAction() {
        return action;
    }

    public void setAction(Consumer<LongObjPair> action) {
        this.action = action;
    }

    private Consumer<LongObjPair> action;

}

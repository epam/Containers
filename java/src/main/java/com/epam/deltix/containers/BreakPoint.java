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
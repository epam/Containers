package com.epam.deltix.containers.interfaces;

/**
 * Interface for sending log information.
 */
public interface LogProcessor {
    /**
     * Event which called on event which needs to log.
     *
     * @param sender        Object, which send this event.
     * @param severity      Severity of this event.
     * @param exception     Exception of this event.
     * @param stringMessage String version of information about event.
     */
    void onLogEvent(Object sender, Severity severity, Throwable exception, CharSequence stringMessage);
}


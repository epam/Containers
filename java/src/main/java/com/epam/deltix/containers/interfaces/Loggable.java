package com.epam.deltix.containers.interfaces;

/**
 * Interface for logging.
 */
public interface Loggable {
    /**
     * Set logger. Default minimal severity for sent messages is Warning.
     *
     * @param logger Logger to set.
     */
    void setLogger(LogProcessor logger);

    /**
     * Set minimal value of severity for sent messages.
     * @param minSeverity Minimal value of severity for sent messages. (Info less than Warning, Warning less than Error).
     */
    void setMinimalSeverityToLog(Severity minSeverity);

    /**
     * Get minimal value of severity for sent messages.
     * @return Minimal value of severity for sent messages.
     */
    Severity getMinimalSeverityToLog();
}


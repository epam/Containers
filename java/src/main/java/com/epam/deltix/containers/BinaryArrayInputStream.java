package com.epam.deltix.containers;

import java.io.InputStream;

/**
 * Implementation of {@code InputStream} with {@code BinaryArray} as underlying data source.
 * Mark is not supported.
 */
public class BinaryArrayInputStream extends InputStream {
    private BinaryArray dataSource;
    private int position;

    /**
     * Constructs {@code BinaryArrayInputStream}.
     *
     * @param dataSource source of data provided by stream.
     */
    public BinaryArrayInputStream(BinaryArray dataSource) {
        this.dataSource = dataSource;
        this.position = 0;
    }

    /**
     * Changes underlying data source and resets position to zero.
     *
     * @param dataSource source of data provided by stream.
     * @return previous data source.
     */
    public BinaryArray updateDataSource(BinaryArray dataSource) {
        BinaryArray old = this.dataSource;
        this.dataSource = dataSource;
        this.position = 0;
        return old;
    }

    /**
     * Reads the next byte of data from the input stream. The value byte is
     * returned as an {@code int} in the range {@code 0} to
     * {@code 255}. If no byte is available because the end of the stream
     * has been reached, the value {@code -1} is returned.
     *
     * @return the next byte of data, or {@code -1} if the end of the stream is reached.
     */
    @Override
    public int read() {
        if (position >= dataSource.getCount())
            return -1;
        ++position;
        return dataSource.getByteAt(position - 1) & (0xff);
    }

    /**
     * Returns number of bytes that can be read.
     *
     * @return number of bytes that can be read.
     */
    @Override
    public int available() {
        return dataSource.getCount() - position;
    }

    /**
     * Resets current position in data source to zero.
     */
    @Override
    public synchronized void reset() {
        this.position = 0;
    }
}

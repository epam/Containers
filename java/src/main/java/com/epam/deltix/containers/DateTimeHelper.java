package com.epam.deltix.containers;

/**
 * Helper for work with DateTime. It's correct for all dates from 1970 to 2100 year.
 */
public class DateTimeHelper {
    final static long MILLI_FOR_DAY = 1000 * 60 * 60 * 24;
    final static long DAY_IN_FOUR_YEAR = 365 * 3 + 366;
    final static int[] DAY_IN_MONTH = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

    /**
     * Assign epoch time(readable format) to MutableString.
     *
     * @param mutableString Resulting MutableString.
     * @param time          Epoch time to convert.
     * @return Resulting string.
     */
    public static MutableString assign(MutableString mutableString, long time) {
        mutableString.clear();
        return append(mutableString, time);
    }

    /**
     * Append epoch time(readable format) to MutableString.
     *
     * @param mutableString Resulting MutableString.
     * @param time          Epoch time to convert.
     * @return Resulting string.
     */
    public static MutableString append(MutableString mutableString, long time) {
        long year = 1970;
        year += 4 * (time / (MILLI_FOR_DAY * (DAY_IN_FOUR_YEAR)));
        time %= (MILLI_FOR_DAY * (DAY_IN_FOUR_YEAR));
        for (int i = 0; i < 4; ++i) {
            int leapYear = 0;
            if (i == 2) leapYear = 1;
            if (time >= MILLI_FOR_DAY * (365 + leapYear)) {
                time -= MILLI_FOR_DAY * (365 + leapYear);
                year++;
            }
        }

        long month = 1;

        for (int i = 0; i < 12; ++i) {
            int dayInMonth = DAY_IN_MONTH[i];
            if (i == 1 && year % 4 == 0) dayInMonth++;
            if (time >= dayInMonth * MILLI_FOR_DAY) {
                month++;
                time -= dayInMonth * MILLI_FOR_DAY;
            } else break;
        }

        long day = time / MILLI_FOR_DAY + 1;
        time %= MILLI_FOR_DAY;

        long hour = time / (60 * 60 * 1000);
        time %= (60 * 60 * 1000);

        long minute = time / (60 * 1000);
        time %= 60000;

        long seconds = time / 1000;

        long milliseconds = time % 1000;
        mutableString.append(year);
        mutableString.append('-');
        if (month < 10) mutableString.append('0');
        mutableString.append(month);
        mutableString.append('-');
        if (day < 10) mutableString.append('0');
        mutableString.append(day);
        mutableString.append(' ');
        if (hour < 10) mutableString.append('0');
        mutableString.append(hour);
        mutableString.append(':');
        if (minute < 10) mutableString.append('0');
        mutableString.append(minute);
        mutableString.append(':');
        if (seconds < 10) mutableString.append('0');
        mutableString.append(seconds);
        mutableString.append('.');
        if (milliseconds < 100) mutableString.append('0');
        if (milliseconds < 10) mutableString.append('0');
        mutableString.append(milliseconds);
        return mutableString;
    }
}

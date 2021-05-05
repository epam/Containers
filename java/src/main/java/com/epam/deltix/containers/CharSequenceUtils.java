package com.epam.deltix.containers;

import java.util.Collection;

/**
 * Utils for work with CharSequence.
 */
public class CharSequenceUtils {
    public static int hashCode(final CharSequence s) {
        int hash = 0;

        if (s != null) {
            for (int i = 0, size = s.length(); i < size; i++) {
                hash = 31 * hash + s.charAt(i);
            }
        }

        return hash;
    }

    /**
     * Compare two CharSequences for equality. A null equals null.
     */
    public static boolean equals(final CharSequence s1, final CharSequence s2) {
        if (s1 == s2) {
            return true;
        }

        if (s1 == null | s2 == null) {
            return false;
        }

        final int length = s1.length();
        if (length != s2.length()) {
            return false;
        }

        for (int i = length - 1; i >= 0; i--) { // backward heuristic
            if (s1.charAt(i) != s2.charAt(i)) {
                return false;
            }
        }

        return true;
    }

    /**
     * Compare two CharSequences for equality. A null equals null.
     */
    public static boolean equals(CharSequence s1, CharSequence s2, int maxLength) {
        return (compare(s1, s2, maxLength, true) == 0);
    }


    /**
     * Compare two CharSequences for equality. Nulls are not supported.
     *
     * @param offset1 offset in the first char sequence
     * @param len1    useful length of the first char sequence
     */
    public static boolean equals(CharSequence s1, final int offset1, final int len1, CharSequence s2) {
        assert s1 != null && s2 != null;
        if (len1 != s2.length())
            return false;

        if (s1 == s2)
            return offset1 == 0;

        for (int ii = 0; ii < len1; ii++) {
            if (s1.charAt(ii + offset1) != s2.charAt(ii))
                return false;
        }

        return true;
    }

    /**
     * Compare two CharSequences. A null argument is always less than a non-null argument
     * and is equal to another null argument.
     *
     * @param fast When true, use a fast algorithm, which makes a
     *             char sequence greater than another if it is longer.
     *             When false, performs lexicographic comparison.
     */
    public static int compare(CharSequence s1, CharSequence s2, boolean fast) {
        return (compare(s1, s2, 0, fast));
    }

    /**
     * Compare two CharSequences. A null argument is always less than a non-null argument
     * and is equal to another null argument.
     *
     * @param maxLength Only compare the first <tt>maxLength</tt> characters.
     *                  Send 0 to unlimit.
     * @param fast      When true, use a fast algorithm, which makes a
     *                  char sequence greater than another if it is longer.
     *                  When false, performs lexicographic comparison.
     */
    public static int compare(
            CharSequence s1,
            CharSequence s2,
            int maxLength,
            boolean fast
    ) {
        if (s1 == null)
            if (s2 == null)
                return (0);
            else
                return (-1);
        else if (s2 == null)
            return (1);
        else if (s1 == s2)
            return (0);
        else {
            int len1 = s1.length();
            int len2 = s2.length();

            if (maxLength > 0) {
                if (maxLength < len1)
                    len1 = maxLength;

                if (maxLength < len2)
                    len2 = maxLength;
            }

            int diff = len1 - len2;

            if (fast && diff != 0)
                return (diff);

            int minLength = diff > 0 ? len2 : len1;

            for (int ii = 0; ii < minLength; ii++) {
                int cdiff = s1.charAt(ii) - s2.charAt(ii);

                if (cdiff != 0)
                    return (cdiff);
            }

            return (diff);
        }
    }

    public static int fastCompare(CharSequence s1, CharSequence s2) {

        int len1 = s1.length();
        int len2 = s2.length();

        int diff = len1 - len2;

        if (diff != 0)
            return (diff);

        for (int ii = 0; ii < len1; ii++) {
            int cdiff = s1.charAt(ii) - s2.charAt(ii);

            if (cdiff != 0)
                return (cdiff);
        }

        return (diff);
    }

    /**
     * Searches target CharSequence in the source.
     * The source is the character sequence being searched, and the target
     * is the character sequence being searched for.
     *
     * @param source       the characters being searched.
     * @param sourceOffset offset of the source string.
     * @param sourceCount  count of the source string.
     * @param target       the characters being searched for.
     * @param targetOffset offset of the target string.
     * @param targetCount  count of the target string.
     * @param fromIndex    the index to begin searching from.
     */
    public static int indexOf(CharSequence source, int sourceOffset, int sourceCount,
                              CharSequence target, int targetOffset, int targetCount,
                              int fromIndex) {
        if (fromIndex >= sourceCount) {
            return (targetCount == 0 ? sourceCount : -1);
        }
        if (fromIndex < 0) {
            fromIndex = 0;
        }
        if (targetCount == 0) {
            return fromIndex;
        }

        char first = target.charAt(targetOffset);
        int max = sourceOffset + (sourceCount - targetCount);

        for (int i = sourceOffset + fromIndex; i <= max; i++) {
            /* Look for first character. */
            if (source.charAt(i) != first) {
                while (++i <= max && source.charAt(i) != first) ;
            }

            /* Found first character, now look at the rest of v2 */
            if (i <= max) {
                int j = i + 1;
                int end = j + targetCount - 1;
                for (int k = targetOffset + 1; j < end && source.charAt(j) == target.charAt(k); j++, k++) ;

                if (j == end) {
                    /* Found whole string. */
                    return i - sourceOffset;
                }
            }
        }
        return -1;
    }


    public static boolean contains(Collection<? extends CharSequence> items, CharSequence value) {
        for (CharSequence item : items)
            if (equals(item, value))
                return true;
        return false;
    }

    /** @return true Source contains target */
    public static boolean contains(CharSequence source, CharSequence target) {
        return indexOf(source, 0, source.length(), target, 0, target.length(), 0) > -1;
    }


    public static boolean isEmptyOrNull(CharSequence value) {
        return value == null || value.length() == 0;
    }

    public static int indexOf(CharSequence s, char c) {
        return indexOf(s, 0, s.length(), c);
    }

    public static int indexOf(CharSequence s, int fromIndex, char c) {
        for (int i = fromIndex, end = s.length(); i < end; i++) {
            if (c == s.charAt(i)) {
                return i;
            }
        }

        return -1;
    }

    public static int indexOf(CharSequence s, int begin, int length, char c) {
        assert length >= 0;
        final int end = begin + length;
        for (int i = begin; i < end; i++)
            if (s.charAt(i) == c)
                return i;
        return -1;
    }

    public static boolean startsWith(CharSequence s, String prefix) {
        int prefixLen = prefix.length();
        if (s.length() < prefixLen)
            return false;

        prefixLen--;

        for (int i = 0; i < prefixLen; i++)
            if (s.charAt(i) != prefix.charAt(i))
                return false;
        return s.charAt(prefixLen) == prefix.charAt(prefixLen);
    }

    public static boolean isAscii(CharSequence s) {
        int errorCode = 0;
        for (int i = 0; i < s.length(); i++)
            errorCode |= (s.charAt(i));
        return errorCode <= 127;
    }

    public static String toString(CharSequence sequence) {
        return (sequence == null) ? null : sequence.toString();
    }
}

package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.*;

import java.io.IOException;
import java.util.Arrays;

public class MutableString implements com.epam.deltix.containers.interfaces.MutableString {
    private static final char[] HEX_DIGITS_UPPER = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
    private static final char[] HEX_DIGITS_LOWER = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
    private int cachedHash;
    private String cachedToString;
    char[] data = new char[10];
    private int count;

    /**
     * Create empty instance of MutableString.
     */
    public MutableString() {
        count = 0;
    }

    public MutableString(int capacity) {
        data = new char[capacity];
        count = 0;
    }

    /**
     * Create instance of MutableString by CharSequance.
     *
     * @param string CharSequance.
     */
    public MutableString(CharSequence string) {
        assign(string);
    }

    public MutableString(BinaryConvertibleReadOnly binaryArray) {
        assign(binaryArray);
    }

    public MutableString(UUIDReadOnly uuid) {
        assign(uuid);
    }

    public MutableString(UUIDReadOnly uuid, UUIDPrintFormat format) {
        assign(uuid, format);
    }


    public MutableString(float item) {
        assign(item);
    }

    public MutableString(boolean item) {
        assign(item);
    }

    @Override
    public MutableString clone() {
        return new MutableString(this);
    }

    @Override
    public void copyTo(com.epam.deltix.containers.interfaces.MutableString str) {
        str.assign(this);
    }

    @Override
    public void toCharArray(int sourceIndex, char[] destination, int destinationIndex, int count) {
        System.arraycopy(data, sourceIndex, destination, destinationIndex, count);
    }

    @Override
    public MutableString subString(int start, int end) {
        if (start < 0 || end > count || start > end)
            throw new IndexOutOfBoundsException("Wrong range");
        MutableString str = new MutableString();
        for (int i = start; i < end; ++i) str.append(data[i]);
        return str;

    }

    /**
     * Writes substring [start, end) to destination string without creating temp object
     * @param destination Destination
     * @param start Start index
     * @param end End index
     * @return Overwritten destination string
     */
    public MutableString subString(MutableString destination, int start, int end) {
        if (start < 0 || end > count || start > end)
            throw new IndexOutOfBoundsException("Wrong range");
        destination.clear();
        for (int i = start; i < end; ++i) destination.append(data[i]);
        return destination;
    }

    /**
     * Returns the <code>char</code> value at the specified index.  An index ranges from zero
     * to <code>length() - 1</code>.  The first <code>char</code> value of the sequence is at
     * index zero, the next at index one, and so on, as for array
     * indexing.
     * <p>If the <code>char</code> value specified by the index is a
     * <a href="{@docRoot}/java/lang/Character.html#unicode">surrogate</a>, the surrogate
     * value is returned.
     *
     * @param index the index of the <code>char</code> value to be returned
     * @return the specified <code>char</code> value
     * @throws IndexOutOfBoundsException if the <code>index</code> argument is negative or not less than
     *                                   <code>length()</code>
     */
    @Override
    public char charAt(int index) {
        return data[index];
    }

    /**
     * Returns a <code>CharSequence</code> that is a subsequence of this sequence.
     * The subsequence starts with the <code>char</code> value at the specified index and
     * ends with the <code>char</code> value at index <code>end - 1</code>.  The length
     * (in <code>char</code>s) of the
     * returned sequence is <code>end - start</code>, so if <code>start == end</code>
     * then an empty sequence is returned.
     *
     * @param start the start index, inclusive
     * @param end   the end index, exclusive
     * @return the specified subsequence
     * @throws IndexOutOfBoundsException if <code>start</code> or <code>end</code> are negative,
     *                                   if <code>end</code> is greater than <code>length()</code>,
     *                                   or if <code>start</code> is greater than <code>end</code>
     */
    @Override
    public MutableString subSequence(int start, int end) {
        MutableString temp = new MutableString();
        for (int i = start; i < end; ++i)
            temp.append(charAt(i));
        return temp;
    }

    @Override
    public Boolean equals(ReadOnlyString another) {
        if (length() != another.length())
            return false;
        for (int i = 0; i < length(); ++i)
            if (data[i] != another.getCharAt(i))
                return false;
        return true;
    }

    @Override
    public Boolean equals(String another) {
        if (length() != another.length())
            return false;
        for (int i = 0; i < length(); ++i)
            if (data[i] != another.charAt(i))
                return false;
        return true;
    }

    @Override
    public int length() {
        if (count >= 0)
            return count;
        else
            return -count;
    }

    @Override
    public int getUTF8ByteLength() {
        int size = 0;
        for (int i = 0, len = length(); i < len; i += 1) {
            char c = charAt(i);
            if (c <= 0x7F) {
                size += 1;
            } else if (c <= 0x7FF) {
                size += 2;
            } else if (Character.isHighSurrogate(c)) {
                size += 4;
                i += 1;
            } else {
                size += 3;
            }
        }
        return size;
    }

    @Override
    public char getCharAt(int index) {
        return data[index];
    }

    @Override
    public long toInt64() {
        long result = 0;
        for (int i = 0; i < length(); ++i) {
            result *= 10;
            result += data[i] - '0';
        }
        return result;
    }

    @Override
    public String toString() {
        if (cachedToString == null) {
            cachedToString = new String(data, 0, length());
        }
        return cachedToString;
    }

    @Override
    public BinaryArray toBinaryArray() {
        return new BinaryArray(this);
    }

    @Override
    public UUID toUUID() {
        return new UUID(this);
    }

    @Override
    public UUID toUUID(UUIDParseFormat format) {
        return new UUID(this, format);
    }

    @Override
    public void toBinaryArray(BinaryConvertibleReadWrite binaryArray) {
        binaryArray.assign(this);
    }

    @Override
    public void toUUID(com.epam.deltix.containers.interfaces.UUID uuid) {
        uuid.assign(this);
    }

    @Override
    public void toUUID(com.epam.deltix.containers.interfaces.UUID uuid, int offset) {
        uuid.assign(this, offset);
    }

    @Override
    public void toUUID(com.epam.deltix.containers.interfaces.UUID uuid, UUIDParseFormat format) {
        uuid.assign(this, format);
    }

    @Override
    public void toUUID(com.epam.deltix.containers.interfaces.UUID uuid, int offset, UUIDParseFormat format) {
        uuid.assign(this, offset, format);
    }

    @Override
    public int hashCode() {
        if (count > 0) {
            int hashCode = 0;
            for (int i = 0; i < count; ++i) {
                hashCode = hashCode * 31;
                hashCode += data[i];
            }
            cachedHash = hashCode;
            count = -count;
        }
        return cachedHash;
    }

    @Override
    public boolean equals(Object that) {
        if (that == null) {
            return false;
        } else if (that instanceof MutableString) {
            return equals((MutableString) that);
        } else if (that instanceof CharSequence) {
            return equals((CharSequence) that);
        } else {
            return false;
        }
    }

    public boolean equals(MutableString string) {
        int n = length();
        if (string.length() != n)
            return false;
        for (int i = 0; i < n; i += 1)
            if (data[i] != string.charAt(i))
                return false;
        return true;
    }

    public boolean equals(CharSequence string) {
        int n = length();
        if (string.length() != n)
            return false;
        for (int i = 0; i < n; i += 1)
            if (data[i] != string.charAt(i))
                return false;
        return true;
    }

    @Override
    public MutableString clear() {
        resetHashCode();
        count = 0;
        return this;
    }

    /**
     * Clear this string. Set length to zero and clear buffer.
     *
     * @return Cleared string.
     */
    @Override
    public com.epam.deltix.containers.interfaces.MutableString secureClear() {
        for (int i = 0; i < data.length; ++i) data[i] = 0;
        clear();
        return this;
    }


    @Override
    public MutableString copyFrom(CharSequence source) {
        return assign(source);
    }

    @Override
    public MutableString insert(int index, char item) {
        resetHashCode();
        int lastCount = count;
        resize(count + 1);
        System.arraycopy(data, index, data, index + 1, count - index);
        count = index;
        append(item);
        count = lastCount + 1;
        return this;
    }

    @Override
    public MutableString insert(int index, String item) {
        resetHashCode();
        int lastCount = count;
        resize(count + item.length());
        System.arraycopy(data, index, data, index + item.length(), count - index);
        count = index;
        append(item);
        count = lastCount + item.length();
        return this;
    }

    @Override
    public MutableString insert(int index, ReadOnlyString item) {
        resetHashCode();
        int lastCount = count;
        resize(count + item.length());
        for (int i = count + item.length() - 1; i > index + item.length() - 1; i--)
            data[i] = data[i - item.length()];
        count = index;
        append(item);
        count = lastCount + item.length();
        return this;
    }

    @Override
    public MutableString insert(int index, StringBuilder item) {
        resetHashCode();
        int lastCount = count;
        resize(count + item.length());
        for (int i = count + item.length() - 1; i > index + item.length() - 1; i--)
            data[i] = data[i - item.length()];
        count = index;
        append(item);
        count = lastCount + item.length();
        return this;
    }

    @Override
    public MutableString insert(int index, char[] item) {
        resetHashCode();
        int lastCount = count;
        resize(count + item.length);
        System.arraycopy(data, index, data, index + item.length, count + item.length - 1 - (index + item.length - 1));
        count = index;
        append(item);
        count = lastCount + item.length;
        return this;
    }

    @Override
    public MutableString insert(int index, char[] item, int offset, int count) {
        resetHashCode();
        int lastCount = this.count;
        resize(this.count + count);
        System.arraycopy(data, index, data, index + count, this.count - index);
        this.count = index;
        append(item, offset, count);
        this.count = lastCount + count;
        return this;
    }

    @Override
    public MutableString insert(int index, long item) {
        resetHashCode();
        int numberOfDigit = 0;
        long tmp = item;
        if (tmp <= 0) {
            numberOfDigit++;
            tmp = -tmp;
        }
        while (tmp > 0) {
            numberOfDigit++;
            tmp = tmp / 10;
        }
        int lastCount = count;
        System.arraycopy(data, index, data, index + numberOfDigit, count - index);
        count = index;
        append(item);
        count = lastCount + numberOfDigit;
        return this;
    }

    @Override
    public MutableString insert(int index, int item) {
        resetHashCode();
        return insert(index, (long) item);
    }

    @Override
    public MutableString insert(int index, short item) {
        resetHashCode();
        return insert(index, (long) item);
    }

    @Override
    public MutableString trim() {
        trimLeft();
        return trimRight();
    }

    @Override
    public MutableString trimLeft() {
        resetHashCode();
        int startIndex = 0;
        while (startIndex < count && Character.isWhitespace(charAt(startIndex))) {
            ++startIndex;
        }
        count = count > startIndex ? count - startIndex : 0;
        System.arraycopy(data, startIndex, data, 0, count);
        return this;
    }

    @Override
    public MutableString trimRight() {
        resetHashCode();
        while (count > 0 && Character.isWhitespace(charAt(count - 1))) {
            --count;
        }
        return this;
    }

    @Override
    public boolean isEmpty() {
        return length() == 0;
    }

    public void copyTo(MutableString string) {
        string.assign(this);
    }

    @Override
    public MutableString assign(char c) {
        clear();
        return append(c);
    }

    @Override
    public MutableString assign(short i) {
        clear();
        return append(i);
    }

    @Override
    public MutableString assign(int i) {
        clear();
        return append(i);
    }

    public MutableString assign(MutableString string) {
        if (string == null) {
            clear();
            return this;
        }
        clear();
        return append(string);
    }

    @Override
    public MutableString assign(String string) {
        if (string == null) {
            clear();
            return this;
        }
        clear();
        return append(string);
    }

    @Override
    public MutableString assign(CharSequence cs) {
        if (cs == null)
        {
            clear();
            return this;
        }
        clear();
        return append(cs);
    }

    @Override
    public MutableString assign(long i) {
        clear();
        return append(i);
    }

    @Override
    public MutableString assign(char[] str) {
        clear();
        return append(str);
    }

    @Override
    public MutableString assign(char[] str, int offset, int count) {
        clear();
        return append(str, offset, count);
    }

    @Override
    public MutableString assignUTF8(byte[] utf8) {
        clear();
        return appendUTF8(utf8);
    }

    @Override
    public MutableString assignUTF8(byte[] utf8, int offset, int count) {
        clear();
        return appendUTF8(utf8, offset, count);
    }

    @Override
    public MutableString assign(double d) {
        clear();
        return append(d);
    }

    @Override
    public MutableString assign(UUIDReadOnly uuid) {
        clear();
        return append(uuid);
    }

    @Override
    public MutableString assign(UUIDReadOnly uuid, UUIDPrintFormat format) {
        clear();
        return append(uuid, format);
    }

    @Override
    public MutableString assign(BinaryConvertibleReadOnly binaryArray) {
        clear();
        return append(binaryArray);
    }

    @Override
    public void setCharAt(char ch, int index) {
        resetHashCode();
        data[index] = ch;
    }

    public MutableString append(MutableString str) {
        resetHashCode();
        resize(count + str.length());
        System.arraycopy(str.data, 0, data, count, str.length());
        count += str.length();
        return this;
    }

    /**
     * Append item to string
     *
     * @param item Item to append.
     * @return This MutableString after appending.
     */
    @Override
    public MutableString append(boolean item) {
        resetHashCode();
        if (item == true) return append("True");
        else return append("False");
    }

    /**
     * Append item to string
     *
     * @param item Item to append.
     * @return This MutableString after appending.
     */
    @Override
    public MutableString append(float item) {
        resetHashCode();
        return append(Float.toString(item));
    }

    @Override
    public MutableString append(StringBuilder str) {
        resetHashCode();
        resize(count + str.length());
        str.getChars(0, str.length(), data, count);
        count += str.length();
        return this;
    }

    @Override
    public MutableString append(ReadOnlyString str) {
        resetHashCode();
        resize(count + str.length());
        for (int i = count; i < count + str.length(); ++i)
            data[i] = str.charAt(i - count);
        count += str.length();
        return this;
    }

    @Override
    public MutableString append(String str) {
        resetHashCode();
        resize(count + str.length());
        str.getChars(0, str.length(), data, count);
        count += str.length();
        return this;
    }

    @Override
    public MutableString append(char c) {
        resetHashCode();
        resize(count + 1);
        data[count] = c;
        count++;
        return this;
    }

    @Override
    public MutableString append(short i) {
        resetHashCode();
        return append((long) i);
    }

    @Override
    public MutableString append(int i) {
        resetHashCode();
        return append((long) i);
    }

    @Override
    public MutableString append(CharSequence str) {
        resetHashCode();
        resize(count + str.length());
        for (int i = count; i < count + str.length(); ++i)
            data[i] = str.charAt(i - count);
        count += str.length();
        return this;
    }

    @Override
    public Appendable append(CharSequence csq, int start, int end) throws IOException {
        resetHashCode();
        resize(count + end - start);
        for (int i = count; i < count + end - start; ++i)
            data[i] = csq.charAt(i - count + start);
        count += end - start;
        return this;
    }

    @Override
    public MutableString append(long i) {
        resetHashCode();
        char t;
        resize(count + 21);
        if (i < 0) {
            append('-');
            i = -i;
        }
        int index = count, index2;
        do {
            data[index++] = (char) ((i % 10) + '0');
            i /= 10;
        } while (i != 0);
        index--;
        index2 = count + ((index - count) / 2);
        for (int j = count, l = index; j <= index2; ++j, --l) {
            t = data[l];
            data[l] = data[j];
            data[j] = t;
        }
        count = index + 1;
        return this;
    }

    @Override
    public MutableString append(char[] str) {
        resetHashCode();
        resize(count + str.length);
        System.arraycopy(str, 0, data, count, str.length);
        count += str.length;
        return this;
    }

    @Override
    public MutableString append(char[] str, int offset, int count) {
        resetHashCode();
        resize(this.count + count);
        System.arraycopy(str, offset, data, this.count, count);
        this.count += count;
        return this;
    }

    @Override
    public MutableString assign(StringBuilder str) {
        resetHashCode();
        clear();
        return append(str);
    }

    @Override
    public MutableString assign(ReadOnlyString str) {
        resetHashCode();
        clear();
        return append(str);
    }

    @Override
    public MutableString append(double i) {
        resetHashCode();
        return append(Double.toString(i));
    }

    @Override
    public MutableString append(UUIDReadOnly uuid) {
        return append(uuid, UUIDPrintFormat.UPPERCASE);
    }

    @Override
    public MutableString append(UUIDReadOnly uuid, UUIDPrintFormat format) {
        resetHashCode();

        final char[] hexDigits = (format == UUIDPrintFormat.LOWERCASE || format == UUIDPrintFormat.LOWERCASE_WITHOUT_DASHES)
                ? HEX_DIGITS_LOWER : HEX_DIGITS_UPPER;

        switch (format) {
            case LOWERCASE:
            case UPPERCASE:
                final long m = uuid.getMSB();
                final long l = uuid.getLSB();
                for (int i = 15; i > 7; i -= 1)
                    append(hexDigits[(int) (m >> (i * 4)) & 0xF]);
                append('-');
                for (int i = 7; i > 3; i -= 1)
                    append(hexDigits[(int) (m >> (i * 4)) & 0xF]);
                append('-');
                for (int i = 3; i >= 0; i -= 1)
                    append(hexDigits[(int) (m >> (i * 4)) & 0xF]);
                append('-');

                for (int i = 15; i > 11; i -= 1)
                    append(hexDigits[(int) (l >> (i * 4)) & 0xF]);
                append('-');
                for (int i = 11; i >= 0; i -= 1)
                    append(hexDigits[(int) (l >> (i * 4)) & 0xF]);
                break;
            case LOWERCASE_WITHOUT_DASHES:
            case UPPERCASE_WITHOUT_DASHES:
                byte[] bytes = uuid.toBytes();
                for (byte b : bytes) {
                    int v = b & 0xFF;
                    append(hexDigits[v >>> 4]);
                    append(hexDigits[v & 0x0F]);
                }
                break;
        }

        return this;
    }

    @Override
    public MutableString append(BinaryConvertibleReadOnly binaryArray) {
        resetHashCode();
        for (int i = 0; i < binaryArray.size(); i += 2) {
            append(binaryArray.toChar(i));
        }
        return this;
    }


    /**
     * Write utf8-representation of this string to utf8-array
     * @param charIndex Start index in source mutable string.
     * @param charCount Count of chars in source mutable string.
     * @param utf8 Destination array of bytes.
     * @param utf8Index Start index in destination byte array.
     * @return Number of written bytes.
     */
    @Override
    public int toUTF8(int charIndex, int charCount, byte[] utf8, int utf8Index) {
        int index = utf8Index;
        for (int i = charIndex, len = Math.min(length(), charIndex + charCount); i < len; i += 1) {
            char c = data[i];
            if (c <= 0x7F) {
                utf8[index] = (byte) c;
                index += 1;
            } else if (c <= 0x7FF) {
                utf8[index] = (byte) ((c >> 6) | 0xC0);
                utf8[index + 1] = (byte) ((c & 0x3F) | 0x80);
                index += 2;
            } else if (Character.isHighSurrogate(c)) {
                int cp = Character.toCodePoint(c, charAt(i + 1));
                i += 1;
                utf8[index] = (byte) ((cp >> 18) | 0xF0);
                utf8[index + 1] = (byte) (((cp >> 12) & 0x3F) | 0x80);
                utf8[index + 2] = (byte) (((cp >> 6) & 0x3F) | 0x80);
                utf8[index + 3] = (byte) ((cp & 0x3F) | 0x80);
                index += 4;
            } else {
                utf8[index] = (byte) ((c >> 12) | 0xE0);
                utf8[index + 1] = (byte) (((c >> 6) & 0x3F) | 0x80);
                utf8[index + 2] = (byte) ((c & 0x3F) | 0x80);
                index += 3;
            }
        }
        return index - utf8Index;
    }

    @Override
    public int toUTF8(byte[] bytes, int offset) {
        int index = offset;
        for (int i = 0, len = length(); i < len; i += 1) {
            char c = data[i];
            if (c <= 0x7F) {
                bytes[index] = (byte) c;

                index += 1;
            } else if (c <= 0x7FF) {
                bytes[index] = (byte) ((c >> 6) | 0xC0);
                bytes[index + 1] = (byte) ((c & 0x3F) | 0x80);

                index += 2;
            } else if (Character.isHighSurrogate(c)) {
                int cp = Character.toCodePoint(c, charAt(i + 1));
                i += 1;

                bytes[index] = (byte) ((cp >> 18) | 0xF0);
                bytes[index + 1] = (byte) (((cp >> 12) & 0x3F) | 0x80);
                bytes[index + 2] = (byte) (((cp >> 6) & 0x3F) | 0x80);
                bytes[index + 3] = (byte) ((cp & 0x3F) | 0x80);

                index += 4;
            } else {
                bytes[index] = (byte) ((c >> 12) | 0xE0);
                bytes[index + 1] = (byte) (((c >> 6) & 0x3F) | 0x80);
                bytes[index + 2] = (byte) ((c & 0x3F) | 0x80);

                index += 3;
            }
        }
        return index - offset;
    }

    @Override
    public MutableString appendUTF8(byte[] bytes) {
        appendUTF8(bytes, 0, bytes.length);
        return this;
    }

    @Override
    public MutableString appendUTF8(byte[] bytes, int offset) {
        appendUTF8(bytes, offset, bytes.length - offset);
        return this;
    }

    @Override
    public MutableString appendUTF8(byte[] bytes, int offset, int count) {
        // TODO: This code does not validates that UTF-8 byte sequence is properly formed.
        for (int i = offset, maxLength = offset + count; i < maxLength; ) {
            int cp = 0;

            if (bytes[i] >= 0) {
                cp += bytes[i];
                i += 1;
            } else if (bytes[i] >> 5 == -2) {
                if (i + 1 >= maxLength)
                    throw new IllegalStateException("Malformed UTF-8 byte sequence.");

                // 2 Bytes:     110xxxxx 10xxxxxx

                // Code Point:  1110xxxx x0000000
                //              11111111 0xxxxxxx
                //              00011111 00000000 = 0x0F80
                //              -----------------
                //              0000xxxx xxxxxxxx

                cp = (bytes[i] << 6) ^ (bytes[i + 1]) ^ 0x0F80;
                i += 2;
            } else if (bytes[i] >> 4 == -2) {
                if (i + 2 >= maxLength)
                    throw new IllegalStateException("Malformed UTF-8 byte sequence.");

                // 3 Bytes:     1110xxxx 10xxxxxx 10xxxxxx

                // Code Point:  1110 xxxx0000 00000000
                //              1111 1110xxxx xx000000
                //              1111 11111111 10xxxxxx
                //              1110 00011111 10000000 = 0xFFFE1F80
                //              ----------------------
                //              0000 xxxxxxxx xxxxxxxx

                cp = (bytes[i] << 12) ^ (bytes[i + 1] << 6) ^ bytes[i + 2] ^ 0xFFFE1F80;
                i += 3;
            } else if (bytes[i] >> 3 == -2) {
                if (i + 3 >= maxLength)
                    throw new IllegalStateException("Malformed UTF-8 byte sequence.");

                // 4 Bytes:    11110xxx 10xxxxxx 10xxxxxx 10xxxxxx

                // Code Point: 11 110xxx00 00000000 00000000
                //             11 111110xx xxxx0000 00000000
                //             11 11111111 1110xxxx xx000000
                //             11 11111111 11111111 10xxxxxx
                //             00 00111000 00011111 10000000 = 0x00381F80
                //             -----------------------------
                //             00 000xxxxx xxxxxxxx xxxxxxxx

                cp = (bytes[i] << 18) ^ (bytes[i + 1] << 12) ^ (bytes[i + 2] << 6) ^ bytes[i + 3] ^ 0x381F80;
                i += 4;
            } else {
                throw new IllegalStateException("Malformed UTF-8 byte sequence.");
            }

            append(Character.toChars(cp));
        }
        return this;
    }

    /**
     * Assign string by item
     *
     * @param item Item to assign.
     * @return this string
     */
    @Override
    public com.epam.deltix.containers.interfaces.MutableString assign(boolean item) {
        resetHashCode();
        clear();
        return append(item);
    }

    /**
     * Assign string by item
     *
     * @param item Item to assign.
     * @return this string
     */
    @Override
    public com.epam.deltix.containers.interfaces.MutableString assign(float item) {
        resetHashCode();
        clear();
        return append(item);
    }

    public int compareTo(MutableString another) {
        int tempLength = another.length() < length() ? another.length() : length();
        for (int i = 0; i < tempLength; ++i)
            if (charAt(i) != another.charAt(i))
                return charAt(i) < another.charAt(i) ? -1 : 1;

        if (another.length() == length())
            return 0;
        return another.length() < length() ? -1 : 1;
    }

    private void resetHashCode() {
        if (count < 0)
            count = -count;
        cachedToString = null;
    }

    private void resize(int newSize) {
        if (newSize >= data.length) {
            data = Arrays.copyOf(data, newSize << 1);
        }
    }

    @Override
    public MutableString toUpperCase() {
        for (int i = 0; i < count; ++i) {
            setCharAt(Character.toUpperCase(data[i]), i);
        }
        return this;
    }

    @Override
    public MutableString toLowerCase() {
        for (int i = 0; i < count; ++i) {
            setCharAt(Character.toLowerCase(data[i]), i);
        }
        return this;
    }
}

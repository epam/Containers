package com.epam.deltix.containers;

import com.epam.deltix.containers.interfaces.*;
import com.epam.deltix.containers.interfaces.MutableString;
import com.epam.deltix.containers.interfaces.UUID;
import com.epam.deltix.containers.binaryarrayoptimizations.BinaryArrayOptimization;
import com.epam.deltix.containers.binaryarrayoptimizations.BinaryArrayOptimizationTotal;
import com.epam.deltix.containers.generated.*;

import java.util.Arrays;

/**
 *  Binary-representation of ASCII-string. Throws exceptions if you try to load to it non-ASCII characters.
 */
@SuppressWarnings({"WeakerAccess", "unused", "ForLoopReplaceableByForEach"})
public class BinaryAsciiString implements CharSequence, com.epam.deltix.containers.interfaces.BinaryAsciiString {
    private static final char[] HEX_DIGITS_UPPER = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E',   'F'};
    private static final char[] HEX_DIGITS_LOWER = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};



    private static BinaryArrayOptimization[] _optimizations = new BinaryArrayOptimization[]
            {
                    new BinaryArrayOptimization0(),
                    new BinaryArrayOptimization1(),
                    new BinaryArrayOptimization2(),
                    new BinaryArrayOptimization3(),
                    new BinaryArrayOptimization4(),
                    new BinaryArrayOptimization5(),
                    new BinaryArrayOptimization6(),
                    new BinaryArrayOptimization7(),
                    new BinaryArrayOptimization8(),
                    new BinaryArrayOptimizationTotal()
            };
    private int count = 0;
    private int hashCode;
    protected long[] data;

    /**
     * Create new instance of BinaryAsciiString with capacity.
     * @param capacity Capacity.
     */
    public BinaryAsciiString(int capacity) {
        data = new long[capacity];
        count = 0;
    }


    /**
     * Create new instance of BinaryAsciiString.
     */
    public BinaryAsciiString() { this(10); }

    /**
     * Create instance of MutableString assigned by CharSequence.
     *
     * @param string CharSequence.
     */
    public BinaryAsciiString(CharSequence string) {
        this(10);
        assign(string);
    }

    /**
     * Create instance of MutableString assigned by BinaryArray.
     *
     * @param binaryArray CharSequence.
     */
    public BinaryAsciiString(BinaryConvertibleReadOnly binaryArray) {
        this(10);
        assign(binaryArray);
    }

    /**
     * Create instance of MutableString assigned by UUID.
     *
     * @param uuid UUID.
     */
    public BinaryAsciiString(UUIDReadOnly uuid) {
        this(10);
        assign(uuid);
    }

    /**
     * Create instance of MutableString assigned by UUID with format.
     *
     * @param uuid UUID.
     * @param format Format of uuid.
     */
    public BinaryAsciiString(UUIDReadOnly uuid, UUIDPrintFormat format) {
        this(10);
        assign(uuid, format);
    }


    /**
     * Create instance of MutableString assigned by float.
     *
     * @param item Float.
     */
    public BinaryAsciiString(float item) {
        this(10);
        assign(item);
    }


    /**
     * Create instance of MutableString assigned by boolean.
     *
     * @param item Float.
     */
    public BinaryAsciiString(boolean item) {
        this(10);
        assign(item);
    }



    private void resize(int newSize) {
        if (newSize <= data.length)
            return;
        data = Arrays.copyOf(data, newSize);
    }


    /**
     * Assign BinaryAsciiString by object.
     * @param obj Object to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(Object obj) {
        clear();
        return append(obj);
    }

    /**
     * Append object to this string.
     * @param obj Object to append.
     * @return This instance.
     */
    public BinaryAsciiString append(Object obj) {
        return append(obj.toString());
    }

    /**
     * Assign BinaryAsciiString by BinaryArray.
     * @param str BinaryArray to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(BinaryArrayReadOnly str) {
        clear();
        return append(str);
    }

    /**
     * Append BinaryArray to this string.
     * @param str BinaryArray to append.
     * @return This instance.
     */
    public BinaryAsciiString append(BinaryArrayReadOnly str) {
        hashCode = hashCode | 0x40000000;
        resize(((count + str.size()) >>> 3) + 1);
        for (int i = 0; i < str.size(); ++i) {
            UnsafeHelper.setByteAtLongArray(data, i + count, str.get(i));
        }
        count += str.size();
        return this;
    }

    /**
     * Assign BinaryAsciiString by boolean.
     * @param str Boolean to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(boolean str) {
        clear();
        return append(str);
    }

    /**
     * Append boolean to this string.
     * @param str Boolean to append.
     * @return This instance.
     */
    public BinaryAsciiString append(boolean str) {
        if (!str)
            append("False");
        else
            append("True");
        return this;
    }


    /**
     * Assign BinaryAsciiString by double.
     * @param str Double to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(double str) {
        clear();
        return append(str);
    }


    /**
     * Append double to this string.
     * @param str Double to append.
     * @return This instance.
     */
    public BinaryAsciiString append(double str) {
        append(Double.toString(str));
        return this;
    }


    /**
     * Assign BinaryAsciiString by short.
     * @param str Short to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(short str) {
        clear();
        return append(str);
    }


    /**
     * Append short to this string.
     * @param str Short to append.
     * @return This instance.
     */
    public BinaryAsciiString append(short str) {
        append((long)str);
        return this;
    }

    /**
     * Assign BinaryAsciiString by int.
     * @param str Int to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(int str) {
        clear();
        return append(str);
    }


    /**
     * Append int to this string.
     * @param str Int to append.
     * @return This instance.
     */
    public BinaryAsciiString append(int str) {
        append((long)str);
        return this;
    }


    /**
     * Assign BinaryAsciiString by UUID.
     * @param uuid UUID to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(UUIDReadOnly uuid) {
        clear();
        return append(uuid);
    }

    /**
     * Assign BinaryAsciiString by short.
     * @param uuid Short to assign.
     * @param format Format of UUID string-representation.
     * @return This instance.
     */
    public BinaryAsciiString assign(UUIDReadOnly uuid, UUIDPrintFormat format) {
        clear();
        return append(uuid, format);
    }


    /**
     * Append UUID to this string.
     * @param uuid UUID to append.
     * @return This instance.
     */
    public BinaryAsciiString append(UUIDReadOnly uuid) {
        return append(uuid, UUIDPrintFormat.UPPERCASE);
    }

    /**
     * Assign BinaryAsciiString by CharSequence.
     * @param sequence CharSequence to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(CharSequence sequence) {
        clear();
        if (sequence instanceof BinaryAsciiString) {
            BinaryAsciiString array = (BinaryAsciiString)sequence;
            resize((array.count >> 3) + 1);
            count = array.count;
            hashCode = array.hashCode;
            UnsafeHelper.memMoveFromLongToLong(array.data, 0, data, 0, count);
        } else append(sequence);
        return this;
    }


    /**
     * Append CharSequence to this string.
     * @param sequence CharSequence to append.
     * @return This instance.
     */
    public BinaryAsciiString append(CharSequence sequence) {
        int errorCode = 0;
        for (int i = 0; i < sequence.length(); i++) errorCode |= (sequence.charAt(i));
        if (errorCode > 127 || errorCode < 0) throw new UnsupportedOperationException("We support only ASCII-characters");
        hashCode = hashCode | 0x40000000;
        resize(((count + sequence.length()) >>> 3) + 1);
        for (int i = 0; i < sequence.length(); ++i)
            UnsafeHelper.setByteAtLongArray(data, i + count, (byte)sequence.charAt(i));
        count += sequence.length();
        return this;
    }

    /**
     * Assign BinaryAsciiString by byte array.
     * @param bytes Byte array to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(byte[] bytes) {
        clear();
        return append(bytes);
    }


    /**
     * Append byte array to this string.
     * @param bytes Byte array to append.
     * @return This instance.
     */
    public BinaryAsciiString append(byte[] bytes) {
        return append(bytes, 0, bytes.length);
    }

    /**
     * Assign BinaryAsciiString by byte array.
     * @param bytes Byte array to assign.
     * @param offset Source array offset
     * @param count Bytes count.
     * @return This instance.
     */
    public BinaryAsciiString assign(byte[] bytes, int offset, int count) {
        clear();
        return append(bytes, offset, count);
    }



    /**
     * Append byte array to this string.
     * @param bytes byte array to append.
     * @param offset Offset in source array.
     * @param count Count of bytes to append.
     * @return This instance.
     */
    public BinaryAsciiString append(byte[] bytes, int offset, int count) {
        hashCode = hashCode | 0x40000000;
        resize(((this.count + count) >>> 3) + 2);
        UnsafeHelper.memMoveFromByteToLong(bytes, offset, data, this.count, count);
        this.count += count;
        return this;
    }



    /**
     * Append UUID to this string.
     * @param uuid Boolean to append.
     * @param format Format of uuid.
     * @return This instance.
     */
    public BinaryAsciiString append(UUIDReadOnly uuid, UUIDPrintFormat format) {
        hashCode = hashCode | 0x40000000;
        resize(((count + 33) >>> 3) + 1);

        final char[] hexDigits = (format == UUIDPrintFormat.LOWERCASE || format == UUIDPrintFormat.LOWERCASE_WITHOUT_DASHES)
                ? HEX_DIGITS_LOWER : HEX_DIGITS_UPPER;

        switch (format) {
            case LOWERCASE:
            case UPPERCASE:
                final long m = uuid.getMSB();
                final long l = uuid.getLSB();
                for (int i = 15; i > 7; i -= 1, count++)
                    UnsafeHelper.setByteAtLongArray(data, count, (byte)hexDigits[(int) (m >> (i * 4)) & 0xF]);
                UnsafeHelper.setByteAtLongArray(data, count, (byte)'-');
                count++;
                for (int i = 7; i > 3; i -= 1, count++)
                    UnsafeHelper.setByteAtLongArray(data, count, (byte)hexDigits[(int) (m >> (i * 4)) & 0xF]);
                UnsafeHelper.setByteAtLongArray(data, count, (byte)'-');
                count++;
                for (int i = 3; i >= 0; i -= 1, count++)
                    UnsafeHelper.setByteAtLongArray(data, count, (byte)hexDigits[(int) (m >> (i * 4)) & 0xF]);
                UnsafeHelper.setByteAtLongArray(data, count, (byte)'-');
                count++;

                for (int i = 15; i > 11; i -= 1, count++)
                    UnsafeHelper.setByteAtLongArray(data, count, (byte)hexDigits[(int) (l >> (i * 4)) & 0xF]);
                UnsafeHelper.setByteAtLongArray(data, count, (byte)'-');
                count++;
                for (int i = 11; i >= 0; i -= 1, count++)
                    UnsafeHelper.setByteAtLongArray(data, count, (byte)hexDigits[(int) (l >> (i * 4)) & 0xF]);
                break;
            case LOWERCASE_WITHOUT_DASHES:
            case UPPERCASE_WITHOUT_DASHES:
                final long m1 = uuid.getMSB();
                final long l1 = uuid.getLSB();
                for (int i = 15; i >= 0; i -= 1, count++)
                    UnsafeHelper.setByteAtLongArray(data, count, (byte)hexDigits[(int) (m1 >> (i * 4)) & 0xF]);
                for (int i = 15; i >= 0; i -= 1, count++)
                    UnsafeHelper.setByteAtLongArray(data, count, (byte)hexDigits[(int) (l1 >> (i * 4)) & 0xF]);
                break;
        }

        return this;
    }

    /**
     * Assign BinaryAsciiString by char array.
     * @param str Char array to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(char[] str) {
        clear();
        return append(str);
    }

    /**
     * Assign BinaryAsciiString by char array.
     * @param str Char array to assign.
     * @param offset Source array offset
     * @param count Bytes count.
     * @return This instance.
     */
    public BinaryAsciiString assign(char[] str, int offset, int count) {
        clear();
        return append(str, offset, count);
    }


    /**
     * Append char array to this string.
     * @param str Char array to append.
     * @return This instance.
     */
    public BinaryAsciiString append(char[] str) {
        int errorCode = 0;
        for (int i = 0; i < str.length; i++) errorCode |= (str[i]);
        if (errorCode > 127 || errorCode < 0) throw new UnsupportedOperationException("We support only ASCII-characters");
        hashCode = hashCode | 0x40000000;
        resize(((count + str.length) >>> 3) + 1);
        for (int i = 0; i < str.length; i++) UnsafeHelper.setByteAtLongArray(data, i + count, (byte)str[i]);
        count += str.length;
        return this;
    }


    /**
     * Append char array to this string.
     * @param str Char array to append.
     * @param offset Offset in source array
     * @param count Count of bytes to append.
     * @return This instance.
     */
    public BinaryAsciiString append(char[] str, int offset, int count) {
        int errorCode = 0;
        for (int i = 0; i < count; i++) errorCode |= (str[i + offset]);
        if (errorCode > 127 || errorCode < 0) throw new UnsupportedOperationException("We support only ASCII-characters");
        hashCode = hashCode | 0x40000000;
        resize(((this.count + count) >>> 3) + 1);
        for (int i = 0; i < count ; i++) UnsafeHelper.setByteAtLongArray(data, i + this.count, (byte)str[i + offset]);
        this.count += count;
        return this;
    }

    /**
     * Assign BinaryAsciiString by long.
     * @param str Long to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(long str) {
        clear();
        return append(str);
    }


    /**
     * Append long as hex
     * @param i Long to append.
     * @return This string after append.
     */
    public BinaryAsciiString appendFastHex(long i) {
        hashCode = hashCode | 0x40000000;
        resize(((count + 17) >>> 3) + 1);
        appendFastHexInternal(i);
        return this;
    }

    private void appendFastHexInternal(long i) {
        long firstLong = 0;
        long secondLong = 0;
        firstLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)((i & 255L))] << 48;
        firstLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)((i >>> 8) & 255L)] << 32;
        firstLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)((i >>> 16) & 255L)] << 16;
        firstLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)((i >>> 24) & 255L)];

        secondLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)(((i >>> 32) & 255L))] << 48;
        secondLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)((i >>> 40) & 255L)] << 32;
        secondLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)((i >>> 48) & 255L)] << 16;
        secondLong |= (long) BinaryAsciiStringHelper.DIGIT_PAIRS[(int)((i >>> 56) & 255L)];

        UnsafeHelper.setLongToLongArray(data, count, secondLong);
        UnsafeHelper.setLongToLongArray(data, count + 8, firstLong);
        count += 16;
    }

    /**
     * Append UUID to hex.
     * @param uuidReadOnly UUID to append.
     * @return This string after append.
     */
    public BinaryAsciiString appendFastHex(UUIDReadOnly uuidReadOnly) {
        hashCode = hashCode | 0x40000000;
        resize(((count + 33) >>> 3) + 1);
        appendFastHexInternal(uuidReadOnly.getLSB());
        appendFastHexInternal(uuidReadOnly.getMSB());
        return this;
    }


    /**
     * Append long to this string.
     * @param i long to append.
     * @return This instance.
     */
    public BinaryAsciiString append(long i) {
        hashCode = hashCode | 0x40000000;
        resize(((count + 21) >>> 3) + 1);
        byte t;
        if (i < 0) {
            UnsafeHelper.setByteAtLongArray(data, count, (byte)'-');
            count++;
            i = -i;
        }
        int index = count, index2;
        do {
            UnsafeHelper.setByteAtLongArray(data, index, (byte)((i % 10) + '0'));
            index++;
            i /= 10;
        } while (i != 0);
        index--;
        index2 = count + ((index - count) / 2);
        for (int j = count, l = index; j <= index2; ++j, --l) {
            t = UnsafeHelper.getByteFromLongArray(data, l);
            UnsafeHelper.setByteAtLongArray(data, l,  UnsafeHelper.getByteFromLongArray(data, j));
            UnsafeHelper.setByteAtLongArray(data, j,  t);
        }
        count = index + 1;
        return this;
    }


    /**
     * Assign BinaryAsciiString by char.
     * @param str Char to assign.
     * @return This instance.
     */
    public BinaryAsciiString assign(char str) {
        clear();
        return append(str);
    }


    /**
     * Append char to this string.
     * @param str Char to append.
     * @return This instance.
     */
    public BinaryAsciiString append(char str) {
        //if (str > 127) throw new UnsupportedOperationException("We support only Ascii-characters");
        hashCode = hashCode | 0x40000000;
        resize(((count + 1) >>> 3) + 1);
        UnsafeHelper.setByteAtLongArray(data, count, (byte)str);
        count++;
        return this;
    }

    @SuppressWarnings("UnusedReturnValue")
    private BinaryAsciiString internalAppend(char str) {
        UnsafeHelper.setByteAtLongArray(data, count, (byte)str);
        count++;
        return this;
    }




    /**
     * Return substring started in char with index start and ended in char with index end
     *
     * @param start start
     * @param end   end
     * @return substring started in char with index start and ended in char with index end
     */
    @Override
    public ReadOnlyString subString(int start, int end) {
        if (start < 0 || end > count || start > end)
            throw new IndexOutOfBoundsException("Wrong range");
        BinaryAsciiString str = new BinaryAsciiString();
        for (int i = start; i < end; ++i) str.append(getCharAt(i));
        return str;
    }

    /**
     * Writes substring [start, end) to destination string without creating temp object
     * @param destination Destination
     * @param start Start index
     * @param end End index
     * @return Overwritten destination string
     */
    public ReadOnlyString subString(BinaryAsciiString destination, int start, int end) {
        if (start < 0 || end > count || start > end)
            throw new IndexOutOfBoundsException("Wrong range");
        destination.clear();
        for (int i = start; i < end; ++i) destination.append(getCharAt(i));
        return destination;
    }

    /**
     * Return true if one MutableString equals to another
     *
     * @param another another
     * @return true if one MutableString equals to another
     */
    @Override
    public Boolean equals(ReadOnlyString another) {
        if (length() != another.length())
            return false;
        for (int i = 0; i < length(); ++i)
            if (getCharAt(i) != another.getCharAt(i))
                return false;
        return true;
    }

    /**
     * Return true if one MutableString equals to another String
     *
     * @param another another
     * @return true if one MutableString equals to another String
     */
    @Override
    public Boolean equals(String another) {
        if (length() != another.length())
            return false;
        for (int i = 0; i < length(); ++i)
            if (getCharAt(i) != another.charAt(i))
                return false;
        return true;
    }

    /**
     * Returns number of UTF-8 characters, needed to represent this string.
     *
     * @return Number of characters in UTF-8 representation of this string.
     */
    @Override
    public int getUTF8ByteLength() {
        return count;
    }

    /**
     * Return char with index index.
     *
     * @param index index
     * @return char with index index.
     */
    @Override
    public char getCharAt(int index) {
        return (char)UnsafeHelper.getByteFromLongArray(data, index);
    }

    /**
     * Return int64-representation of this string(it's correct if this string is a number)
     *
     * @return int64-representation of this string(it's correct if this string is a number)
     */
    @Override
    public long toInt64() {
        long result = 0;
        for (int i = 0; i < length(); ++i) {
            result *= 10;
            result += getCharAt(i) - '0';
        }
        return result;
    }

    /**
     * Write utf8-representation of this string to utf8-array
     *
     * @param utf8   utf8-array
     * @param offset offset
     * @return number of bytes
     */
    @Override
    public int toUTF8(byte[] utf8, int offset) {
        int index = offset;
        for (int i = 0, len = length(); i < len; i += 1) {
            utf8[index] = (byte) getCharAt(i);
            index += 1;
        }
        return index - offset;
    }

    /**
     * Write utf8-representation of this string to utf8-array
     *
     * @param charIndex Start index in source mutable string.
     * @param charCount Count of chars in source mutable string.
     * @param utf8      Destination array of bytes.
     * @param utf8Index Start index in destination byte array.
     * @return Number of written bytes.
     */
    @Override
    public int toUTF8(int charIndex, int charCount, byte[] utf8, int utf8Index) {
        int index = utf8Index;
        for (int i = charIndex, len = Math.min(charCount + charIndex, length()); i < len; i += 1) {
            utf8[index] = (byte) getCharAt(i);
            index += 1;
        }
        return index - utf8Index;
    }

    /**
     * Returns binary representation of this string.
     *
     * @return new {@code BinaryArray} representation of this string.
     */
    @Override
    public BinaryConvertibleReadOnly toBinaryArray() {
        return new BinaryArray(this);
    }

    /**
     * Create {@code UUID} from this string, using {@code UUIDParseFormat.ANY} format.
     *
     * @return New {@code UUID}.
     */
    @Override
    public UUIDReadOnly toUUID() {
        return new com.epam.deltix.containers.UUID(this);
    }

    /**
     * Create {@code UUID} from this string, using specified {@code UUIDParseFormat}.
     *
     * @param format this string allowed uuid format.
     * @return New {@code UUID}.
     */
    @Override
    public UUIDReadOnly toUUID(UUIDParseFormat format) {
        return new com.epam.deltix.containers.UUID(this, format);
    }

    /**
     * Assigns binary representation of this string to specified {@code BinaryArray}.
     *
     * @param binaryArray output {@code BinaryArray}, containing binary representation of this sting.
     */
    @Override
    public void toBinaryArray(BinaryConvertibleReadWrite binaryArray) {
        binaryArray.assign(this);
    }

    /**
     * Write uuid-value of this string to uuid, using {@code UUIDParseFormat.ANY} format.
     *
     * @param uuid output uuid.
     */
    @Override
    public void toUUID(UUID uuid) {
        uuid.assign(this);
    }

    /**
     * Write uuid-value of all bytes, started from offset to uuid, using {@code UUIDParseFormat.ANY} format.
     *
     * @param uuid   output uuid.
     * @param offset offset in this string, chars.
     */
    @Override
    public void toUUID(UUID uuid, int offset) {
        uuid.assign(this, offset);
    }

    /**
     * Write uuid-value of this string to uuid, using specified {@code UUIDParseFormat}.
     *
     * @param uuid   output uuid.
     * @param format this string allowed uuid format.
     */
    @Override
    public void toUUID(UUID uuid, UUIDParseFormat format) {
        uuid.assign(this, format);
    }

    /**
     * Write uuid-value of all bytes, started from offset to uuid, using specified {@code UUIDParseFormat}.
     *
     * @param uuid   output uuid.
     * @param offset offset in this string, chars.
     * @param format this string allowed uuid format.
     */
    @Override
    public void toUUID(UUID uuid, int offset, UUIDParseFormat format) {
        uuid.assign(this, offset, format);
    }

    /**
     * Return clone of this string
     *
     * @return clone of this string
     */
    @SuppressWarnings("MethodDoesntCallSuperMethod")
    @Override
    public MutableString clone() {
        return new com.epam.deltix.containers.MutableString().assign(this);
    }

    /**
     * Copy this string to another string.
     *
     * @param str another string
     */
    @Override
    public void copyTo(MutableString str) {
        str.assign(this);
    }

    /**
     * Copy this string to another string.
     *
     * @param str another string
     */
    @Override
    public void copyTo(BinaryAsciiString str) {
        str.assign(this);
    }

    /**
     * Assign this string by another
     *
     * @param source another string
     * @return this string
     */
    @Override
    public BinaryAsciiString copyFrom(CharSequence source) {
        return assign(source);
    }

    /**
     * Copy count chars from this string started from offset to char array with offset
     *
     * @param sourceIndex      source offset
     * @param destination      destination array
     * @param destinationIndex destination offset
     * @param count            number of chars
     */
    @Override
    public void toCharArray(int sourceIndex, char[] destination, int destinationIndex, int count) {
        for (int i = sourceIndex; i < sourceIndex + count; ++i) destination[destinationIndex + i - sourceIndex] = getCharAt(i);
    }

    /**
     * Checks if string is empty (i.e. it's length equals zero) and returns result of this check.
     *
     * @return {@code true}, if string is empty, {@code false} otherwise.
     */
    @Override
    public boolean isEmpty() {
        return count == 0;
    }

    /**
     * Returns the length of this character sequence.  The length is the number
     * of 16-bit <code>char</code>s in the sequence.
     *
     * @return the number of <code>char</code>s in this sequence
     */
    @Override
    public int length() {
        return count;
    }

    /**
     * Returns the <code>char</code> value at the specified index.  An index ranges from zero
     * to <tt>length() - 1</tt>.  The first <code>char</code> value of the sequence is at
     * index zero, the next at index one, and so on, as for array
     * indexing.
     * <p>
     * <p>If the <code>char</code> value specified by the index is a
     * <a href="{@docRoot}/java/lang/Character.html#unicode">surrogate</a>, the surrogate
     * value is returned.
     *
     * @param index the index of the <code>char</code> value to be returned
     * @return the specified <code>char</code> value
     * @throws IndexOutOfBoundsException if the <tt>index</tt> argument is negative or not less than
     *                                   <tt>length()</tt>
     */
    @Override
    public char charAt(int index) {
        return getCharAt(index);
    }

    /**
     * Returns a <code>CharSequence</code> that is a subsequence of this sequence.
     * The subsequence starts with the <code>char</code> value at the specified index and
     * ends with the <code>char</code> value at index <tt>end - 1</tt>.  The length
     * (in <code>char</code>s) of the
     * returned sequence is <tt>end - start</tt>, so if <tt>start == end</tt>
     * then an empty sequence is returned.
     *
     * @param start the start index, inclusive
     * @param end   the end index, exclusive
     * @return the specified subsequence
     * @throws IndexOutOfBoundsException if <tt>start</tt> or <tt>end</tt> are negative,
     *                                   if <tt>end</tt> is greater than <tt>length()</tt>,
     *                                   or if <tt>start</tt> is greater than <tt>end</tt>
     */
    @SuppressWarnings("SpellCheckingInspection")
    @Override
    public CharSequence subSequence(int start, int end) {
        return subString(start, end);
    }

    public BinaryAsciiString clear() {
        int len = ((count + 7) >>> 3);
        for (int i = 0; i < len; ++i)
            data[i] = 0L;
        count = 0;
        hashCode = hashCode | 0x40000000;
        return this;
    }

    /**
     * Set char at index.
     * @param index Index of char.
     * @param x Char to set.
     */
    public void setCharAt(int index, char x) {
        //if (x > 127) throw new UnsupportedOperationException("We support only Ascii-characters");
        UnsafeHelper.setByteAtLongArray(data, index, (byte)x);
        hashCode = hashCode | 0x40000000;
    }


    void setByteAt(int index, byte x) {
        UnsafeHelper.setByteAtLongArray(data, index, x);
    }


    @Override
    public int hashCode() {
        if ((hashCode & 0x40000000) != 0) {
            int a = (count + 7) >> 3;
            int tempHashCode = (int) _optimizations[9 + ((a - 9) & (a - 9) >> 31)].xxHash64(data, count);
            hashCode = tempHashCode & (0x3FFFFFFF);
        }
        return hashCode;
    }


    public boolean equals(BinaryAsciiString another) {
        if (count != another.count)
            return false;
        int a = ((count + 7) >> 3);
        return _optimizations[9 + ((a - 9) & (a - 9) >> 31)].equals(data, another.data, a);

    }


    public boolean equals(CharSequence string) {
        int n = length();
        if (string.length() != n)
            return false;
        for (int i = 0; i < n; i += 1)
            if (getCharAt(i) != string.charAt(i))
                return false;
        return true;
    }


    @Override
    public boolean equals(Object that) {
        if (that == null) {
            return false;
        } else if (that instanceof BinaryAsciiString) {
            return equals((BinaryAsciiString) that);
        } else
            return that instanceof CharSequence && equals((CharSequence) that);
    }

    @Override
    public String toString() {
        MutableString builder = new com.epam.deltix.containers.MutableString();
        for (int i = 0; i < count; ++i)
            builder.append(getCharAt(i));
        return builder.toString();
    }

    @Override
    public BinaryAsciiString toUpperCase() {
        for (int i = 0; i < count; ++i) {
            setCharAt(i, Character.toUpperCase(getCharAt(i)));
        }
        return this;
    }

    @Override
    public BinaryAsciiString toLowerCase() {
        for (int i = 0; i < count; ++i) {
            setCharAt(i, Character.toLowerCase(getCharAt(i)));
        }
        return this;
    }
}

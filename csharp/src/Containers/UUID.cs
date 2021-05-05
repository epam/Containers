using System;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Format of output string.
	/// </summary>
	public enum UUIDPrintFormat
	{
		/// <summary>
		/// Lower case with dashes. Example: '01234567-89ab-cdef-1011-121314151617'.
		/// </summary>
		LowerCase = 1,
		/// <summary>
		/// Upper case with dashes. This is the default output format. Example: '01234567-89AB-CDEF-1011-121314151617'.
		/// </summary>
		UpperCase = 2,
		/// <summary>
		/// Lower case without dashes. Example: '0123456789abcdef1011121314151617'.
		/// </summary>
		LowerCaseWithoutDashes = 3,
		/// <summary>
		/// Upper case without dashes. This is the default hex output format. Example: '0123456789ABCDEF1011121314151617'.
		/// </summary>
		UpperCaseWithoutDashes = 4
	}

	/// <summary>
	/// Format of input string.
	/// </summary>
	public enum UUIDParseFormat
	{
		/// <summary>
		/// Lower case with dashes. Example: '01234567-89ab-cdef-1011-121314151617'.
		/// </summary>
		LowerCase = 1,
		/// <summary>
		/// Upper case with dashes. Example: '01234567-89AB-CDEF-1011-121314151617'.
		/// </summary>
		UpperCase = 2,
		/// <summary>
		/// Any case with dashes. Symbol case can be mixed. Example: '01234567-89Ab-cDef-1011-121314151617'.
		/// </summary>
		AnyCase = 3,
		/// <summary>
		/// Lower case without dashes. Example: '0123456789abcdef1011121314151617'.
		/// </summary>
		LowerCaseWithoutDashes = 4,
		/// <summary>
		/// Upper case without dashes. Example: '0123456789ABCDEF1011121314151617'.
		/// </summary>
		UpperCaseWithoutDashes = 5,
		/// <summary>
		/// Any case without dashes. Symbol case can be mixed. Example: '0123456789AbcDef1011121314151617'.
		/// </summary>
		AnyCaseWithoutDashes = 6,
		/// <summary>
		/// Any correctly formatted <see cref="UUID"/> string.
		/// Can be either <see cref="AnyCase"/> or  <see cref="AnyCaseWithoutDashes"/>.
		/// Dashes cannot be mixed.
		/// </summary>
		Any = 7
	}

	/// <summary>
	/// UUID structure.
	/// </summary>
	public struct UUID : IEquatable<UUID>, IComparable<UUID>, IEquatable<Guid>
	{
		/// <summary>
		/// A read-only instance of the <see cref="UUID"/> structure whose value is all zeros.
		/// </summary>
		public static readonly UUID Empty = new UUID(0, 0);

		/// <summary>
		/// Minimal UUID
		/// </summary>
		public static readonly UUID MinValue = new UUID(0, 0);

		/// <summary>
		/// Maximal UUID
		/// </summary>
		public static readonly UUID MaxValue = new UUID(UInt64.MaxValue, UInt64.MaxValue);

		/// <summary>
		/// First long of UUID
		/// </summary>
		public ulong LSB
		{
			get;
			private set;
		}

		/// <summary>
		/// Second long of UUID
		/// </summary>
		public ulong MSB
		{
			get;
			private set;
		}

		/// <summary>
		/// Create instance of UUID
		/// </summary>
		/// <param name="msb">MSB</param>
		/// <param name="lsb">LSB</param>
		public UUID(ulong msb, ulong lsb)
		{
			LSB = lsb;
			MSB = msb;
		}

		/// <summary>
		/// Assign this UUID by another UUID
		/// </summary>
		/// <param name="other">another UUID</param>
		/// <returns>this</returns>
		public UUID(UUID other)
		{
			LSB = other.LSB;
			MSB = other.MSB;
		}


		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>true if string is valid uuid.</returns>
		public static bool IsValid(IReadOnlyString str)
		{
			return IsValid(str, 0);
		}

		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="offset">offset of string</param>
		/// <returns>true if string is valid uuid.</returns>
		public static bool IsValid(IReadOnlyString str, int offset)
		{
			return IsValid(str, offset, UUIDParseFormat.Any);
		}

		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="format">format of uuid.</param>
		/// <returns>True if string is valid uuid.</returns>
		public static bool IsValid(IReadOnlyString str, UUIDParseFormat format)
		{
			return IsValid(str, 0, format);
		}

		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="offset">offset of uuid.</param>
		/// <param name="format">format of uuid</param>
		/// <returns>True if string is valid uuid.</returns>
		public static bool IsValid(IReadOnlyString str, int offset, UUIDParseFormat format)
		{
			if (str == null)
				return false;
			if (str.Length < offset + 32)
				return false;
			if (format == UUIDParseFormat.Any)
				format = str[offset + 8] == '-' ? UUIDParseFormat.AnyCase : UUIDParseFormat.AnyCaseWithoutDashes;

			switch (format)
			{
				case UUIDParseFormat.LowerCase:
				case UUIDParseFormat.UpperCase:
				case UUIDParseFormat.AnyCase:
					{
						if (!CanExtract(str, offset, 8, 0, format)) return false;
						ulong m = Extract(str, offset, 8, 0, format);
						if (str[offset + 8] != '-')
							return false;
						if (!CanExtract(str, offset + 9, 4, m, format)) return false;
						m = Extract(str, offset + 9, 4, m, format);
						if (str[offset + 13] != '-')
							return false;
						if (!CanExtract(str, offset + 14, 4, m, format)) return false;
						m = Extract(str, offset + 14, 4, m, format);
						if (str[offset + 18] != '-')
							return false;
						ulong l = 0;
						if (!CanExtract(str, offset + 19, 4, 0L, format)) return false;
						l = Extract(str, offset + 19, 4, 0L, format);
						if (str[offset + 23] != '-')
							return false;
						if (!CanExtract(str, offset + 24, 12, l, format)) return false;

						break;
					}
				case UUIDParseFormat.LowerCaseWithoutDashes:
				case UUIDParseFormat.UpperCaseWithoutDashes:
				case UUIDParseFormat.AnyCaseWithoutDashes:
					if ((!CanExtract(str, offset, 16, 0L, format)) || (!CanExtract(str, offset + 16, 16, 0L, format))) return false;
					break;
			}

			return true;
		}

		private static bool CanExtract(IReadOnlyString source, int offset, int count, ulong accumulator, UUIDParseFormat format)
		{
			if (offset + count - 1 >= source.Length) return false;
			for (int i = offset; i < offset + count; i += 1)
			{
				int d = HexDigit(source[i], format);
				if (d == -1) return false;
			}
			return true;
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(IReadOnlyString str, out UUID output)
		{
			return TryParse(str, 0, out output);
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="offset">Offset of string</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(IReadOnlyString str, int offset, out UUID output)
		{
			return TryParse(str, offset, UUIDParseFormat.Any, out output);
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="format">Format of string</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(IReadOnlyString str, UUIDParseFormat format, out UUID output)
		{
			return TryParse(str, 0, format, out output);
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="offset">Offset of string</param>
		/// <param name="format">Format of string</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(IReadOnlyString str, int offset, UUIDParseFormat format, out UUID output)
		{
			output = Empty;
			if (str == null)
				return false;
			if (str.Length < offset + 32)
				return false;
			if (format == UUIDParseFormat.Any)
				format = str[offset + 8] == '-' ? UUIDParseFormat.AnyCase : UUIDParseFormat.AnyCaseWithoutDashes;

			switch (format)
			{
				case UUIDParseFormat.LowerCase:
				case UUIDParseFormat.UpperCase:
				case UUIDParseFormat.AnyCase:
					{
						if (!CanExtract(str, offset, 8, 0L, format)) return false;
						ulong m = Extract(str, offset, 8, 0L, format);
						if (str[offset + 8] != '-')
							return false;
						if (!CanExtract(str, offset + 9, 4, m, format)) return false;
						m = Extract(str, offset + 9, 4, m, format);
						if (str[offset + 13] != '-')
							return false;
						if (!CanExtract(str, offset + 14, 4, m, format)) return false;
						m = Extract(str, offset + 14, 4, m, format);
						if (str[offset + 18] != '-')
							return false;
						if (!CanExtract(str, offset + 19, 4, 0L, format)) return false;
						ulong l = Extract(str, offset + 19, 4, 0L, format);
						if (str[offset + 23] != '-')
							return false;
						if (!CanExtract(str, offset + 24, 12, l, format)) return false;
						l = Extract(str, offset + 24, 12, l, format);

						output.MSB = m;
						output.LSB = l;
						break;
					}
				case UUIDParseFormat.LowerCaseWithoutDashes:
				case UUIDParseFormat.UpperCaseWithoutDashes:
				case UUIDParseFormat.AnyCaseWithoutDashes:
					if ((!CanExtract(str, offset, 16, 0L, format)) || (!CanExtract(str, offset + 16, 16, 0L, format)))
					{
						return false;
					}
					output.MSB = Extract(str, offset, 16, 0L, format);
					output.LSB = Extract(str, offset + 16, 16, 0L, format);
					break;
			}

			return true;
		}




		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <returns>true if string is valid uuid.</returns>
		public static bool IsValid(string str)
		{
			return IsValid(str, 0);
		}

		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="offset">offset of string</param>
		/// <returns>true if string is valid uuid.</returns>
		public static bool IsValid(string str, int offset)
		{
			return IsValid(str, offset, UUIDParseFormat.Any);
		}

		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="format">format of uuid.</param>
		/// <returns>True if string is valid uuid.</returns>
		public static bool IsValid(string str, UUIDParseFormat format)
		{
			return IsValid(str, 0, format);
		}

		/// <summary>
		/// Return true if string is valid uuid.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="offset">offset of uuid.</param>
		/// <param name="format">format of uuid</param>
		/// <returns>True if string is valid uuid.</returns>
		public static bool IsValid(string str, int offset, UUIDParseFormat format)
		{
			if (str == null)
				return false;
			if (str.Length < offset + 32)
				return false;
			if (format == UUIDParseFormat.Any)
				format = str[offset + 8] == '-' ? UUIDParseFormat.AnyCase : UUIDParseFormat.AnyCaseWithoutDashes;

			switch (format)
			{
				case UUIDParseFormat.LowerCase:
				case UUIDParseFormat.UpperCase:
				case UUIDParseFormat.AnyCase:
					{
						if (!CanExtract(str, offset, 8, 0, format)) return false;
						ulong m = Extract(str, offset, 8, 0, format);
						if (str[offset + 8] != '-')
							return false;
						if (!CanExtract(str, offset + 9, 4, m, format)) return false;
						m = Extract(str, offset + 9, 4, m, format);
						if (str[offset + 13] != '-')
							return false;
						if (!CanExtract(str, offset + 14, 4, m, format)) return false;
						m = Extract(str, offset + 14, 4, m, format);
						if (str[offset + 18] != '-')
							return false;
						ulong l = 0;
						if (!CanExtract(str, offset + 19, 4, 0L, format)) return false;
						l = Extract(str, offset + 19, 4, 0L, format);
						if (str[offset + 23] != '-')
							return false;
						if (!CanExtract(str, offset + 24, 12, l, format)) return false;

						break;
					}
				case UUIDParseFormat.LowerCaseWithoutDashes:
				case UUIDParseFormat.UpperCaseWithoutDashes:
				case UUIDParseFormat.AnyCaseWithoutDashes:
					if ((!CanExtract(str, offset, 16, 0L, format)) || (!CanExtract(str, offset + 16, 16, 0L, format))) return false;
					break;
			}

			return true;
		}

		private static bool CanExtract(string source, int offset, int count, ulong accumulator, UUIDParseFormat format)
		{
			if (offset + count - 1 >= source.Length) return false;
			for (int i = offset; i < offset + count; i += 1)
			{
				int d = HexDigit(source[i], format);
				if (d == -1) return false;
			}
			return true;
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(string str, out UUID output)
		{
			return TryParse(str, 0, out output);
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="offset">Offset of string</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(string str, int offset, out UUID output)
		{
			return TryParse(str, offset, UUIDParseFormat.Any, out output);
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="format">Format of string</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(string str, UUIDParseFormat format, out UUID output)
		{ 
			return TryParse(str, 0, format, out output);
		}

		/// <summary>
		/// Return true if you can parse string as valid uuid.
		/// </summary>
		/// <param name="str">String</param>
		/// <param name="offset">Offset of string</param>
		/// <param name="format">Format of string</param>
		/// <param name="output">UUID.Empty if str is not valid uuid or parsed uuid if valid</param>
		/// <returns>True if you can parse string as valid uuid.</returns>
		public static bool TryParse(string str, int offset, UUIDParseFormat format, out UUID output)
		{
			output = Empty;
			if (str == null)
				return false;
			if (str.Length < offset + 32)
				return false;
			if (format == UUIDParseFormat.Any)
				format = str[offset + 8] == '-' ? UUIDParseFormat.AnyCase : UUIDParseFormat.AnyCaseWithoutDashes;

			switch (format)
			{
				case UUIDParseFormat.LowerCase:
				case UUIDParseFormat.UpperCase:
				case UUIDParseFormat.AnyCase:
					{
						if (!CanExtract(str, offset, 8, 0L, format)) return false;
						ulong m = Extract(str, offset, 8, 0L, format);
						if (str[offset + 8] != '-')
							return false;
						if (!CanExtract(str, offset + 9, 4, m, format)) return false;
						m = Extract(str, offset + 9, 4, m, format);
						if (str[offset + 13] != '-')
							return false;
						if (!CanExtract(str, offset + 14, 4, m, format)) return false;
						m = Extract(str, offset + 14, 4, m, format);
						if (str[offset + 18] != '-')
							return false;
						if (!CanExtract(str, offset + 19, 4, 0L, format)) return false;
						ulong l = Extract(str, offset + 19, 4, 0L, format);
						if (str[offset + 23] != '-')
							return false;
						if (!CanExtract(str, offset + 24, 12, l, format)) return false;
						l = Extract(str, offset + 24, 12, l, format);

						output.MSB = m;
						output.LSB = l;
						break;
					}
				case UUIDParseFormat.LowerCaseWithoutDashes:
				case UUIDParseFormat.UpperCaseWithoutDashes:
				case UUIDParseFormat.AnyCaseWithoutDashes:
					if ((!CanExtract(str, offset, 16, 0L, format)) || (!CanExtract(str, offset + 16, 16, 0L, format)))
					{
						return false;
					}
					output.MSB = Extract(str, offset, 16, 0L, format);
					output.LSB = Extract(str, offset + 16, 16, 0L, format);
					break;
			}

			return true;
		}





		/// <summary>
		/// Assign this UUID by string using specified <see cref="UUIDParseFormat"/>.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="format">Input string format.</param>
		public UUID(string str, UUIDParseFormat format)
			: this(str, 0, format)
		{
		}

		/// <summary>
		/// Assign this UUID by string with offset using specified <see cref="UUIDParseFormat"/>.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="offset">offset</param>
		/// <param name="format">Input string format.</param>
		public UUID(string str, int offset = 0, UUIDParseFormat format = UUIDParseFormat.Any)
		{
			if (str == null)
				throw new ArgumentNullException(nameof(str));
			if (str.Length < offset + 32)
				throw new ArgumentException("UUID is too short.", nameof(str));

			if (format == UUIDParseFormat.Any)
				format = str[offset + 8] == '-' ? UUIDParseFormat.AnyCase : UUIDParseFormat.AnyCaseWithoutDashes;

			switch (format)
			{
				case UUIDParseFormat.LowerCase:
				case UUIDParseFormat.UpperCase:
				case UUIDParseFormat.AnyCase:
					{
						ulong m = Extract(str, offset, 8, 0UL, format);
						if (str[offset + 8] != '-')
							throw new ArgumentException("UUID is improperly formatted", str);
						m = Extract(str, offset + 9, 4, m, format);
						if (str[offset + 13] != '-')
							throw new ArgumentException("UUID is improperly formatted", str);
						m = Extract(str, offset + 14, 4, m, format);
						if (str[offset + 18] != '-')
							throw new ArgumentException("UUID is improperly formatted", str);

						ulong l = Extract(str, offset + 19, 4, 0L, format);
						if (str[offset + 23] != '-')
							throw new ArgumentException("UUID is improperly formatted", str);
						l = Extract(str, offset + 24, 12, l, format);

						MSB = m;
						LSB = l;
						break;
					}
				case UUIDParseFormat.LowerCaseWithoutDashes:
				case UUIDParseFormat.UpperCaseWithoutDashes:
				case UUIDParseFormat.AnyCaseWithoutDashes:
					MSB = Extract(str, offset, 16, 0L, format);
					LSB = Extract(str, offset + 16, 16, 0L, format);
					break;
				default:
					throw new ArgumentException("Unsupported format", format.ToString());
			}
		}

		/// <summary>
		/// Assign this UUID by string using specified <see cref="UUIDParseFormat"/>.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="format">Input string format.</param>
		public UUID(IReadOnlyString str, UUIDParseFormat format)
			: this(str, 0, format)
		{
		}

		/// <summary>
		/// Assign this UUID by string with offset using specified <see cref="UUIDParseFormat"/>.
		/// </summary>
		/// <param name="str">string</param>
		/// <param name="offset">offset</param>
		/// <param name="format">Input string format.</param>
		public UUID(IReadOnlyString str, int offset = 0, UUIDParseFormat format = UUIDParseFormat.Any)
		{
			if (str == null)
				throw new ArgumentNullException(nameof(str));
			if (str.Length < offset + 32)
				throw new ArgumentException("UUID is too short.", nameof(str));

			if (format == UUIDParseFormat.Any)
				format = str[offset + 8] == '-' ? UUIDParseFormat.AnyCase : UUIDParseFormat.AnyCaseWithoutDashes;

			switch (format)
			{
				case UUIDParseFormat.LowerCase:
				case UUIDParseFormat.UpperCase:
				case UUIDParseFormat.AnyCase:
					{
						ulong m = Extract(str, offset, 8, 0UL, format);
						if (str[offset + 8] != '-')
							throw new ArgumentException("UUID is improperly formatted", str.ToString());
						m = Extract(str, offset + 9, 4, m, format);
						if (str[offset + 13] != '-')
							throw new ArgumentException("UUID is improperly formatted", str.ToString());
						m = Extract(str, offset + 14, 4, m, format);
						if (str[offset + 18] != '-')
							throw new ArgumentException("UUID is improperly formatted", str.ToString());

						ulong l = Extract(str, offset + 19, 4, 0L, format);
						if (str[offset + 23] != '-')
							throw new ArgumentException("UUID is improperly formatted", str.ToString());
						l = Extract(str, offset + 24, 12, l, format);

						MSB = m;
						LSB = l;
						break;
					}
				case UUIDParseFormat.LowerCaseWithoutDashes:
				case UUIDParseFormat.UpperCaseWithoutDashes:
				case UUIDParseFormat.AnyCaseWithoutDashes:
					MSB = Extract(str, offset, 16, 0L, format);
					LSB = Extract(str, offset + 16, 16, 0L, format);
					break;
				default:
					throw new ArgumentException("Unsupported format", format.ToString());
			}
		}

		/// <summary>
		/// Assign this UUID by array of byte with offset.
		/// </summary>
		/// <param name="ar">array of byte</param>
		/// <param name="offset">offset</param>
		/// <returns>this UUID</returns>
		public UUID(byte[] ar, int offset = 0)
		{
			if (ar == null)
				throw new ArgumentNullException(nameof(ar));
			if (ar.Length < 16 + offset)
				throw new ArgumentException("Array is too short.", nameof(ar));

			ulong m = 0;
			for (int i = 0; i < 8; i += 1)
				m = (m << 8) | (uint)(ar[offset + i] & 0xFF);

			ulong l = 0;
			for (int i = 8; i < 16; i += 1)
				l = (l << 8) | (uint)(ar[offset + i] & 0xFF);

			MSB = m;
			LSB = l;
		}

		/// <summary>
		/// Assign this UUID by array of byte with offset.
		/// </summary>
		/// <param name="ba">binary array</param>
		/// <param name="offset">offset</param>
		/// <returns>this UUID</returns>
		public UUID(IBinaryConvertibleReadOnly ba, int offset = 0)
		{
			if (ba == null)
				throw new ArgumentNullException(nameof(ba));
			if (ba.Count < 16 + offset)
				throw new ArgumentException("Array is too short.", nameof(ba));

			ulong m = 0;
			for (int i = 0; i < 8; i += 1)
				m = (m << 8) | (uint)(ba[offset + i] & 0xFF);

			ulong l = 0;
			for (int i = 8; i < 16; i += 1)
				l = (l << 8) | (uint)(ba[offset + i] & 0xFF);

			MSB = m;
			LSB = l;
		}

		/// <summary>
		/// Return true if this UUID is equals to another UUID.
		/// </summary>
		/// <param name="other">Another UUID.</param>
		/// <returns><c>true</c> if this UUID is equals to another UUID; <c>false</c> - otherwise.</returns>
		public bool Equals(UUID other)
		{
			return other.MSB == MSB && other.LSB == LSB;
		}

		/// <summary>
		/// Return true if this <see cref="UUID"/> is equals to the given <see cref="Guid"/>.
		/// </summary>
		/// <param name="other">Some <see cref="Guid"/></param>
		/// <returns><c>true</c> if this <see cref="UUID"/> is equals to the <see cref="Guid"/>; <c>false</c> - otherwise.</returns>
		public Boolean Equals(Guid other)
		{
			return Equals((UUID)other);
		}

		/// <summary>
		/// Return true if this <see cref="UUID"/> is equals to the given <see cref="Object"/>.
		/// </summary>
		/// <param name="other">Some <see cref="Object"/></param>
		/// <returns><c>true</c> if this <see cref="UUID"/> is equals to the <see cref="Object"/>; <c>false</c> - otherwise.</returns>
		public override Boolean Equals(Object other)
		{
			if (other is UUID)
				return Equals((UUID)other);
			if (other is Guid)
				return Equals((Guid)other);
			IEquatable<UUID> equatable = other as IEquatable<UUID>;
			return equatable != null && equatable.Equals(this);
		}

		/// <summary>
		/// Convert this UUID to array of bytes.
		/// </summary>
		/// <returns>array of bytes</returns>
		public byte[] ToBytes()
		{
			byte[] data = new byte[16];
			ulong m = MSB;
			for (int i = 0; i < 8; ++i)
			{
				data[7 - i] = (byte)(m & 0xFF);
				m >>= 8;
			}
			ulong l = LSB;
			for (int i = 0; i < 8; ++i)
			{
				data[15 - i] = (byte)(l & 0xFF);
				l >>= 8;
			}
			return data;
		}

		/// <summary>
		/// Write byte-representation of this UUID to array of bytes.
		/// </summary>
		/// <param name="ar">array of bytes</param>
		public void ToBytes(byte[] ar)
		{
			ulong m = MSB;
			for (int i = 0; i < 8; ++i)
			{
				ar[7 - i] = (byte)(m & 0xFF);
				m >>= 8;
			}
			ulong l = LSB;
			for (int i = 0; i < 8; ++i)
			{
				ar[15 - i] = (byte)(l & 0xFF);
				l >>= 8;
			}
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override Int32 GetHashCode()
		{
			return (int)(MSB ^ (MSB >> 32)) ^ (int)(LSB ^ (LSB >> 32));
		}

		/// <summary>
		/// Return string-representation of this UUID.
		/// </summary>
		/// <returns>string-representation of this UUID</returns>
		public override string ToString()
		{
			return new MutableString().Append(this, UUIDPrintFormat.UpperCase).ToString();
		}

		/// <summary>
		/// Return string-representation of this UUID, using specified <see cref="UUIDPrintFormat"/>.
		/// </summary>
		/// <param name="format">Output string format of uuid.</param>
		/// <returns>string-representation of this UUID</returns>
		public string ToString(UUIDPrintFormat format)
		{
			return new MutableString().Append(this, format).ToString();
		}

		/// <summary>
		/// Return hexadecimal string that is a representation of this UUID.
		/// </summary>
		/// <returns>Hexadecimal string that is a representation of this UUID</returns>
		public string ToHexString()
		{
			return new MutableString().Append(this, UUIDPrintFormat.UpperCaseWithoutDashes).ToString();
		}

		/// <summary>
		/// Create random UUID.
		/// </summary>
		/// <returns>New instance of generated random UUID</returns>
		public static UUID Random()
		{
			return Guid.NewGuid();
		}

		/// <summary>
		/// Compares that current instance with another object of the same type.
		/// </summary>
		/// <param name="that">An object to compare with this instance.</param>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared.
		/// The return value has these meanings:
		/// <list type="table">
		/// 	<listheader>
		/// 			<term>Value</term>
		/// 		<description>Meaning</description>
		/// 	</listheader>
		/// 	<item>
		/// 			<term>Less than zero</term>
		/// 			<description>This instance precedes other in the sort order.</description>
		/// 	</item>
		/// 	<item>
		/// 			<term>Zero</term>
		/// 			<description>This instance occurs in the same position in the sort order as other.</description>
		/// 	</item>
		/// 	<item>
		/// 			<term>Greater than zero</term>
		/// 			<description>This instance follows other in the sort order.</description>
		/// </item>
		/// </list>
		/// </returns>
		public int CompareTo(UUID that)
		{
			if (MSB < that.MSB)
				return -1;
			return MSB > that.MSB ? 1 : LSB.CompareTo(that.LSB);
		}

		/// <summary>
		/// Implicit conversion of <code>UUID</code> to <code>Guid</code>.
		/// </summary>
		/// <param name="other">The <code>UUID</code> value to be converted</param>
		public static implicit operator Guid(UUID other)
		{
			UInt64 msb = other.MSB;
			UInt64 lsb = other.LSB;

			UInt32 a = (UInt32)(msb >> 32);
			UInt16 b = (UInt16)((msb >> 16) & 0xFFFF);
			UInt16 c = (UInt16)(msb & 0xFFFF);

			Byte k = (Byte)(lsb & 0xFF);
			lsb = lsb >> 8;
			Byte j = (Byte)(lsb & 0xFF);
			lsb = lsb >> 8;
			Byte i = (Byte)(lsb & 0xFF);
			lsb = lsb >> 8;
			Byte h = (Byte)(lsb & 0xFF);
			lsb = lsb >> 8;
			Byte g = (Byte)(lsb & 0xFF);
			lsb = lsb >> 8;
			Byte f = (Byte)(lsb & 0xFF);
			lsb = lsb >> 8;
			Byte e = (Byte)(lsb & 0xFF);
			lsb = lsb >> 8;
			Byte d = (Byte)(lsb & 0xFF);

			return new Guid(a, b, c, d, e, f, g, h, i, j, k);
		}

		/// <summary>
		/// Implicit conversion of <code>Guid</code> to <code>UUID</code>.
		/// </summary>
		/// <param name="other">The <code>Guid</code> value to be converted</param>
		public static implicit operator UUID(Guid other)
		{
			UInt64 lsb = 0;
			UInt64 msb = 0;

			unsafe
			{
				byte* guidPtr = (byte*)&other;
				// int
				msb = guidPtr[3];
				msb = (msb << 8) | guidPtr[2];
				msb = (msb << 8) | guidPtr[1];
				msb = (msb << 8) | guidPtr[0];
				// short
				msb = (msb << 8) | guidPtr[5];
				msb = (msb << 8) | guidPtr[4];
				// short
				msb = (msb << 8) | guidPtr[7];
				msb = (msb << 8) | guidPtr[6];
				// byte[8]
				for (int i = 8; i < 16; ++i)
				{
					lsb = (lsb << 8) | guidPtr[i];
				}
			}

			return new UUID(msb, lsb);
		}

		/// <summary>
		/// Compares two UUIDs for equality.
		/// </summary>
		/// <param name="a">The first UUID to compare.</param>
		/// <param name="b">The second UUID to compare.</param>
		/// <returns><c>true</c>if a and b are equal; otherwise, <c>false</c>.</returns>
		public static Boolean operator ==(UUID a, UUID b)
		{
			return a.MSB == b.MSB && a.LSB == b.LSB;
		}

		/// <summary>
		/// Compares two UUIDs for inequality.
		/// </summary>
		/// <param name="a">The first UUID to compare.</param>
		/// <param name="b">The second UUID to compare.</param>
		/// <returns><c>true</c>if a and b are not equal; otherwise, <c>false</c>.</returns>
		public static Boolean operator !=(UUID a, UUID b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Compares two UUIDs.
		/// </summary>
		/// <param name="a">The first UUID to compare.</param>
		/// <param name="b">The second UUID to compare.</param>
		/// <returns><c>true</c> if a less than b; otherwise, <c>false</c>.</returns>
		public static bool operator <(UUID a, UUID b)
		{
			return a.CompareTo(b) < 0;
		}

		/// <summary>
		/// Compares two UUIDs.
		/// </summary>
		/// <param name="a">The first UUID to compare.</param>
		/// <param name="b">The second UUID to compare.</param>
		/// <returns><c>true</c> if a less than or equal to b; otherwise, <c>false</c>.</returns>
		public static bool operator <=(UUID a, UUID b)
		{
			return a.CompareTo(b) <= 0;
		}

		/// <summary>
		/// Compares two UUIDs.
		/// </summary>
		/// <param name="a">The first UUID to compare.</param>
		/// <param name="b">The second UUID to compare.</param>
		/// <returns><c>true</c> if a greater than b; otherwise, <c>false</c>.</returns>
		public static bool operator >(UUID a, UUID b)
		{
			return a.CompareTo(b) > 0;
		}

		/// <summary>
		/// Compares two UUIDs.
		/// </summary>
		/// <param name="a">The first UUID to compare.</param>
		/// <param name="b">The second UUID to compare.</param>
		/// <returns><c>true</c> if a greater than or equal to b; otherwise, <c>false</c>.</returns>
		public static bool operator >=(UUID a, UUID b)
		{
			return a.CompareTo(b) >= 0;
		}

		private static ulong Extract(String source, int offset, int count, ulong accumulator, UUIDParseFormat format)
		{
			for (int i = offset; i < offset + count; i += 1)
			{
				int d = HexDigit(source[i], format);
				if (d == -1)
					throw new ArgumentException("UUID is improperly formatted.");
				accumulator = (accumulator << 4) | (uint)d;
			}
			return accumulator;
		}

		private static ulong Extract(IReadOnlyString source, int offset, int count, ulong accumulator, UUIDParseFormat format)
		{
			for (int i = offset; i < offset + count; i += 1)
			{
				int d = HexDigit(source[i], format);
				if (d == -1)
					throw new ArgumentException("UUID is improperly formatted.");
				accumulator = (accumulator << 4) | (uint)d;
			}
			return accumulator;
		}

		private static int HexDigit(char c, UUIDParseFormat format)
		{
			if (c >= '0' && c <= '9')
				return c - '0';
			switch (format)
			{
				case UUIDParseFormat.LowerCase:
				case UUIDParseFormat.LowerCaseWithoutDashes:
					if (c >= 'a' && c <= 'f')
						return 10 + c - 'a';
					return -1;
				case UUIDParseFormat.UpperCase:
				case UUIDParseFormat.UpperCaseWithoutDashes:
					if (c >= 'A' && c <= 'F')
						return 10 + c - 'A';
					return -1;
				case UUIDParseFormat.AnyCase:
				case UUIDParseFormat.AnyCaseWithoutDashes:
				case UUIDParseFormat.Any:
					if (c >= 'a' && c <= 'f')
						return 10 + c - 'a';
					if (c >= 'A' && c <= 'F')
						return 10 + c - 'A';
					return -1;
			}
			return -1;
		}
	}
}

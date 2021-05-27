/*
  Copyright 2021 EPAM Systems, Inc

  See the NOTICE file distributed with this work for additional information
  regarding copyright ownership. Licensed under the Apache License,
  Version 2.0 (the "License"); you may not use this file except in compliance
  with the License.  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
  License for the specific language governing permissions and limitations under
  the License.
 */
﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Money is a class that is designed to store a fixed-precision fractional number.
	/// The value is stored in Int64 value; the size of the structure is 8 bytes. 
	/// The value is stored in multiplied form, the degree of multiplication is 10^6. This way the structure is capable of holding fractional numbers with 6 fractional digits.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	public struct Money : IComparable
	{
		/// <summary>
		/// Describes how an Int64 value should be treated: like an integer or like a number of fractions.
		/// </summary>
		internal enum Int64Meaning
		{
			Integer,
			Fractions
		}


		private readonly long _value;

		/// <summary>
		/// The multiplier that is used to convert fractional number to Int64 backing field.
		/// </summary>
		public const long Multiplier = 1000000;

		/// <summary>
		/// The reverse multiplier.
		/// </summary>
		private const decimal ReverseMultiplier = 0.000001m;


		/// <summary>
		/// Constant representing the Money value 0.
		/// </summary>
		public static readonly Money Zero = new Money(0L, Int64Meaning.Fractions);

		/// <summary>
		/// Constant representing the largest possible Money value.
		/// </summary>
		public static readonly Money MaxValue = new Money(Int64.MaxValue, Int64Meaning.Fractions);

		/// <summary>
		/// Constant representing the smallest possible Money value.
		/// </summary>
		public static readonly Money MinValue = new Money(Int64.MinValue, Int64Meaning.Fractions);


		internal Money(long value, Int64Meaning int64Meaning)
		{
			if (int64Meaning == Int64Meaning.Fractions)
				_value = value;
			else if (int64Meaning == Int64Meaning.Integer)
				_value = value * Multiplier;
			else
				throw new ArgumentException("int64Meaning");
		}

		/// <summary>
		/// Constructs Money from Int64 number.
		/// </summary>
		/// <param name="value">Int64 number.</param>
		public Money(Int64 value)
		{
			checked
			{
				_value = value * Multiplier;
			}
		}

		/// <summary>
		/// Constructs Money from Int32 number.
		/// </summary>
		/// <param name="value">Int32 number.</param>
		public Money(Int32 value)
		{
			checked
			{
				_value = value * Multiplier;
			}
		}

		/// <summary>
		/// Constructs Money from UInt64 number.
		/// </summary>
		/// <param name="value">UInt64 number.</param>
		public Money(UInt64 value)
		{
			checked
			{
				_value = (Int64)value * Multiplier;
			}
		}

		/// <summary>
		/// Constructs Money from UInt32 number.
		/// </summary>
		/// <param name="value">UInt32 number.</param>
		public Money(UInt32 value)
		{
			checked
			{
				_value = value * Multiplier;
			}
		}

		/// <summary>
		/// Constructs Money from Decimal number.
		/// </summary>
		/// <param name="value">Decimal number.</param>
		public Money(Decimal value)
		{
			checked
			{
				_value = Decimal.ToInt64(value * Multiplier);
			}
		}

		/// <summary>
		/// Constructs Money from Double number.
		/// </summary>
		/// <param name="value">Double number.</param>
		public Money(Double value)
		{
			checked
			{
				_value = Convert.ToInt64(value * Multiplier);
			}
		}

		/// <summary>
		/// Constructs Money from Single number.
		/// </summary>
		/// <param name="value">Single number.</param>
		public Money(Single value)
		{
			checked
			{
				_value = Convert.ToInt64(value * Multiplier);
			}
		}

		/// <summary>
		/// Explicitly converts Decimal to Money.
		/// </summary>
		/// <param name="value">Value to be converted</param>
		public static explicit operator Money(Decimal value)
		{
			return new Money(value);
		}

		/// <summary>
		/// Implicitly converts Money to Decimal.
		/// </summary>
		/// <param name="value">Value to be converted</param>
		public static implicit operator Decimal(Money value)
		{
			return value._value * ReverseMultiplier;
		}

		/// <summary>
		/// Explicitly converts Double to Money.
		/// </summary>
		/// <param name="value">Value to be converted</param>
		public static explicit operator Money(Double value)
		{
			return new Money(value);
		}

		/// <summary>
		/// Explicitly converts Money to Double.
		/// </summary>
		/// <param name="value">Value to be converted</param>
		public static explicit operator Double(Money value)
		{
			return Convert.ToDouble(value._value * ReverseMultiplier);
		}

		/// <summary>
		/// Explicitly converts Single to Money.
		/// </summary>
		/// <param name="value">Value to be converted</param>
		public static explicit operator Money(Single value)
		{
			return new Money(value);
		}

		/// <summary>
		/// Explicitly converts Money to Single.
		/// </summary>
		/// <param name="value">Value to be converted</param>
		public static explicit operator Single(Money value)
		{
			return Convert.ToSingle(value._value * ReverseMultiplier);
		}

		/// <summary>
		/// Equality operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static bool operator ==(Money m1, Money m2)
		{
			return m1._value == m2._value;
		}

		/// <summary>
		/// Inequality operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static bool operator !=(Money m1, Money m2)
		{
			return m1._value != m2._value;
		}

		/// <summary>
		/// Addition operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static Money operator +(Money m1, Money m2)
		{
			return new Money(m1._value + m2._value, Int64Meaning.Fractions);
		}

		/// <summary>
		/// Subtraction operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static Money operator -(Money m1, Money m2)
		{
			return new Money(m1._value - m2._value, Int64Meaning.Fractions);
		}

		/// <summary>
		/// Multiplication operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static Money operator *(Money m1, Money m2)
		{
			return new Money(m1._value * m2._value / Multiplier, Int64Meaning.Fractions);
		}

		/// <summary>
		/// Division operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static Money operator /(Money m1, Money m2)
		{
			return new Money(Multiplier * m1._value / m2._value, Int64Meaning.Fractions);
		}

		//public static Money operator %(Money m1, Money m2)
		//{
		//	return Remainder(m1, m2);
		//}

		/// <summary>
		/// Less operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static bool operator <(Money m1, Money m2)
		{
			return m1._value < m2._value;
		}

		/// <summary>
		/// Less or equal operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static bool operator <=(Money m1, Money m2)
		{
			return m1._value <= m2._value;
		}

		/// <summary>
		/// Greater operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static bool operator >(Money m1, Money m2)
		{
			return m1._value > m2._value;
		}

		/// <summary>
		/// Greater or equals operator.
		/// </summary>
		/// <param name="m1">Operand 1.</param>
		/// <param name="m2">Operand 2.</param>
		/// <returns></returns>
		public static bool operator >=(Money m1, Money m2)
		{
			return m1._value >= m2._value;
		}

		/// <summary>
		/// Chacks two Money objects for equality.
		/// </summary>
		/// <param name="o"></param>
		/// <returns>true if objects are equal, otherwise false.</returns>
		public override bool Equals(object o)
		{
			if (!(o is Money))
				return false;

			return this == (Money)o;
		}

		/// <summary>
		/// Calculates hash code for Money object.
		/// </summary>
		/// <returns>Hash code.</returns>
		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		/// <summary>
		/// Compares two Money objects.
		/// </summary>
		/// <returns>Result of comparison.</returns>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			if (!(obj is Money))
				throw new ArgumentException("Cannot compare money.");

			Money other = (Money)obj;
			return _value.CompareTo(other._value);
		}

		/// <summary>
		/// Returns string representation of Money object.
		/// </summary>
		/// <returns>String representation of Money object.</returns>
		public override string ToString()
		{
			return ((decimal) this).ToString(CultureInfo.InvariantCulture);
		}
	}
}
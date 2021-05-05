using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EPAM.Deltix.Containers;

namespace EPAM.Deltix.Containers.Tests{
	public class MoneyTests
	{
		[Test]
		public void CreateMoneyFromFractionsReturnsDividedValue()
		{
			Money m = new Money(1, Money.Int64Meaning.Fractions);
			Assert.AreEqual(0.000001m, (decimal)m);

			m = new Money(1000000, Money.Int64Meaning.Fractions);
			Assert.AreEqual(1m, (decimal)m);

			m = new Money(-1, Money.Int64Meaning.Fractions);
			Assert.AreEqual(-0.000001m, (decimal)m);

			m = new Money(-1000000, Money.Int64Meaning.Fractions);
			Assert.AreEqual(-1m, (decimal)m);
		}

		[Test]
		public void CreateMoneyFromIntegerReturnsSameValue()
		{
			Money m = new Money(1, Money.Int64Meaning.Integer);
			Assert.AreEqual(1m, (decimal)m);

			m = new Money(1000000, Money.Int64Meaning.Integer);
			Assert.AreEqual(1000000m, (decimal)m);

			m = new Money(-1, Money.Int64Meaning.Integer);
			Assert.AreEqual(-1m, (decimal)m);

			m = new Money(-1000000, Money.Int64Meaning.Integer);
			Assert.AreEqual(-1000000m, (decimal)m);
		}

		[Test]
		public void CreateMoneyFromInt64ReturnsSameValue()
		{
			Money m = new Money(1L);
			Assert.AreEqual(1m, (decimal)m);

			m = new Money(1000000L);
			Assert.AreEqual(1000000m, (decimal)m);

			m = new Money(-1L);
			Assert.AreEqual(-1m, (decimal)m);

			m = new Money(-1000000L);
			Assert.AreEqual(-1000000m, (decimal)m);
		}

		[Test]
		public void CreateMoneyFromInt32ReturnsSameValue()
		{
			Money m = new Money((Int32)1);
			Assert.AreEqual(1m, (decimal)m);

			m = new Money((Int32)1000000);
			Assert.AreEqual(1000000m, (decimal)m);

			m = new Money((Int32)(-1));
			Assert.AreEqual(-1m, (decimal)m);

			m = new Money((Int32)(-1000000L));
			Assert.AreEqual(-1000000m, (decimal)m);
		}

		[Test]
		public void CreateMoneyFromUInt64ReturnsSameValue()
		{
			Money m = new Money(1UL);
			Assert.AreEqual(1m, (decimal)m);

			m = new Money(1000000UL);
			Assert.AreEqual(1000000m, (decimal)m);
		}

		[Test]
		public void CreateMoneyFromUInt32ReturnsSameValue()
		{
			Money m = new Money((UInt32)1);
			Assert.AreEqual(1m, (decimal)m);

			m = new Money((UInt32)1000000);
			Assert.AreEqual(1000000m, (decimal)m);
		}

		[Test]
		public void CreateMoneyFromDecimalReturnsSameValue()
		{
			Money m = new Money(1m);
			Assert.AreEqual(1m, (decimal)m);

			m = new Money(1000000m);
			Assert.AreEqual(1000000m, (decimal)m);

			m = new Money(0.000001m);
			Assert.AreEqual(0.000001m, (decimal)m);

			m = new Money(-1m);
			Assert.AreEqual(-1m, (decimal)m);

			m = new Money(-1000000m);
			Assert.AreEqual(-1000000m, (decimal)m);

			m = new Money(-0.000001m);
			Assert.AreEqual(-0.000001m, (decimal)m);
		}

		[Test]
		public void CreateMoneyFromDoubleReturnsSameValue()
		{
			Money m = new Money(1D);
			Assert.AreEqual(1D, (double)m);

			m = new Money(1000000D);
			Assert.AreEqual(1000000D, (double)m);

			m = new Money(0.000001D);
			Assert.AreEqual(0.000001D, (double)m);

			m = new Money(-1D);
			Assert.AreEqual(-1D, (double)m);

			m = new Money(-1000000D);
			Assert.AreEqual(-1000000D, (double)m);

			m = new Money(-0.000001D);
			Assert.AreEqual(-0.000001D, (double)m);
		}

		[Test]
		public void CreateMoneyFromSingleReturnsSameValue()
		{
			Money m = new Money(1F);
			Assert.AreEqual(1F, (float)m);

			m = new Money(1000000F);
			Assert.AreEqual(1000000F, (float)m);

			m = new Money(0.000001F);
			Assert.AreEqual(0.000001F, (float)m);

			m = new Money(-1F);
			Assert.AreEqual(-1F, (float)m);

			m = new Money(-1000000F);
			Assert.AreEqual(-1000000F, (float)m);

			m = new Money(-0.000001F);
			Assert.AreEqual(-0.000001F, (float)m);
		}

		[Test]
		public void ConversionFromDecimalIsTheSameAsConstructor()
		{
			Money m1 = new Money(1.000001m);
			Money m2 = (Money)1.000001m;
			Assert.AreEqual(m1, m2);
		}

		[Test]
		public void ConversionToDecimalReturnsSameValue()
		{
			decimal d = 1.000001m;
			Money m = new Money(d);
			Assert.AreEqual(d, (decimal)m);
		}

		[Test]
		public void ConversionFromDoubleIsTheSameAsConstructor()
		{
			Money m1 = new Money(1.000001D);
			Money m2 = (Money)1.000001D;
			Assert.AreEqual(m1, m2);
		}

		[Test]
		public void ConversionToDoubleReturnsSameValue()
		{
			double d = 1.000001D;
			Money m = new Money(d);
			Assert.AreEqual(d, (double)m);
		}

		[Test]
		public void ConversionFromSingleIsTheSameAsConstructor()
		{
			Money m1 = new Money(1.000001F);
			Money m2 = (Money)1.000001F;
			Assert.AreEqual(m1, m2);
		}

		[Test]
		public void ConversionToSingleReturnsSameValue()
		{
			float d = 1.000001F;
			Money m = new Money(d);
			Assert.AreEqual(d, (float)m);
		}

		[Test]
		public void EqualityOperatorIsTrueForEqual()
		{
			Money m1 = new Money(1.000001m);
			Money m2 = new Money(1.000001m);
			Assert.IsTrue(m1 == m2);
		}

		[Test]
		public void EqualityOperatorIsFalseForNotEqual()
		{
			Money m1 = new Money(1.000001m);
			Money m2 = new Money(1.000002m);
			Assert.IsFalse(m1 == m2);
		}

		[Test]
		public void InequalityOperatorIsFalseForEqual()
		{
			Money m1 = new Money(1.000001m);
			Money m2 = new Money(1.000001m);
			Assert.IsFalse(m1 != m2);
		}

		[Test]
		public void InequalityOperatorIsTrueForNotEqual()
		{
			Money m1 = new Money(1.000001m);
			Money m2 = new Money(1.000002m);
			Assert.IsTrue(m1 != m2);
		}

		[Test]
		public void AdditionOperatorIsWorking()
		{
			decimal d1 = 1.000001m;
			decimal d2 = 2.000002m;

			Assert.AreEqual(new Money(d1 + d2), new Money(d1) + new Money(d2));
			Assert.AreEqual(new Money(d2 + d1), new Money(d2) + new Money(d1));
		}

		[Test]
		public void SubtractionOperatorIsWorking()
		{
			decimal d1 = 1.000001m;
			decimal d2 = 2.000002m;

			Assert.AreEqual(new Money(d1 - d2), new Money(d1) - new Money(d2));
			Assert.AreEqual(new Money(d2 - d1), new Money(d2) - new Money(d1));
		}

		[Test]
		public void MultiplicationOperatorIsWorking()
		{
			decimal d1 = 1.000001m;
			decimal d2 = 2.000002m;

			Assert.AreEqual(new Money(d1 * d2), new Money(d1) * new Money(d2));
			Assert.AreEqual(new Money(d2 * d1), new Money(d2) * new Money(d1));
		}

		[Test]
		public void DivisionOperatorIsWorking()
		{
			decimal d1 = 1.000001m;
			decimal d2 = 2.000002m;

			Assert.AreEqual(new Money(d1 / d2), new Money(d1) / new Money(d2));
			Assert.AreEqual(new Money(d2 / d1), new Money(d2) / new Money(d1));
		}

	}
}

using System;
using NUnit.Framework;

namespace EPAM.Deltix.Containers.Tests{
	// ReSharper disable InconsistentNaming
	public class UUIDTests
	{

		[TestCase]
		public void TestTryAssignAndIsCorrect()
		{
			UUID outputUUID;
			Assert.AreEqual(false, UUID.TryParse("123-45678-90AB-CDEF-1011-121314151617", out outputUUID));
			Assert.AreEqual(false, UUID.IsValid("123-45678-90AB-CDEF-1011-121314151617"));

			Assert.AreEqual(false, UUID.TryParse("12345678-90ab-cdef-1011-12131415161", out outputUUID));
			Assert.AreEqual(false, UUID.IsValid("12345678-90ab-cdef-1011-12131415161"));

			Assert.AreEqual(true, UUID.TryParse("12345678-90ab-cdef-1011-121314151617", out outputUUID));
			Assert.AreEqual(true, UUID.IsValid("12345678-90ab-cdef-1011-121314151617"));

			Assert.AreEqual(true, UUID.TryParse("12345678-90AB-CDEF-1011-121314151617", out outputUUID));
			Assert.AreEqual(true, UUID.IsValid("12345678-90AB-CDEF-1011-121314151617"));

			Assert.AreEqual(true, UUID.TryParse("12345678-90Ab-CdEf-1011-121314151617", out outputUUID));
			Assert.AreEqual(true, UUID.IsValid("12345678-90Ab-CdEf-1011-121314151617"));

			Assert.AreEqual(true, UUID.TryParse("1234567890abcdef1011121314151617", out outputUUID));
			Assert.AreEqual(true, UUID.IsValid("1234567890abcdef1011121314151617"));

			Assert.AreEqual(true, UUID.TryParse("1234567890ABCDEF1011121314151617", out outputUUID));
			Assert.AreEqual(true, UUID.IsValid("1234567890ABCDEF1011121314151617"));

			Assert.AreEqual(true, UUID.TryParse("1234567890AbCdEf1011121314151617", out outputUUID));
			Assert.AreEqual(true, UUID.IsValid("1234567890AbCdEf1011121314151617"));
		}


		[TestCase]
		public void ToStringIsCorrect()
		{
			Assert.That(new UUID(0x4e5e7aecbdf644c5, 0xa3aa19859bb67964).ToString().Equals(@"4e5e7aec-bdf6-44c5-a3aa-19859bb67964", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0x40ccb4b0e3ff4367, 0xa0ab426c2338444d).ToString().Equals(@"40ccb4b0-e3ff-4367-a0ab-426c2338444d", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0xe00fa594c8af47cd, 0xae2b16195616b5d5).ToString().Equals(@"e00fa594-c8af-47cd-ae2b-16195616b5d5", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0xdc91516808b24e28, 0x8aa71add130a7f30).ToString().Equals(@"dc915168-08b2-4e28-8aa7-1add130a7f30", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0x25adc2b0d8a042bb, 0xaec39cf08e14bb45).ToString().Equals(@"25adc2b0-d8a0-42bb-aec3-9cf08e14bb45", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0x070ee84c30a040e3, 0xbb647e3acf89b0a6).ToString().Equals(@"070ee84c-30a0-40e3-bb64-7e3acf89b0a6", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0xc85f7bd1d29145b5, 0xb745d98fafcf18e2).ToString().Equals(@"c85f7bd1-d291-45b5-b745-d98fafcf18e2", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0x02c7dd25f3de4d30, 0x93eed1990a518f20).ToString().Equals(@"02c7dd25-f3de-4d30-93ee-d1990a518f20", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0x9edd9ec7f4c34648, 0xa3f1ec8e1a0ea597).ToString().Equals(@"9edd9ec7-f4c3-4648-a3f1-ec8e1a0ea597", StringComparison.OrdinalIgnoreCase));
			Assert.That(new UUID(0x12317dfc281046d0, 0x8b6252a98e1b4bdd).ToString().Equals(@"12317dfc-2810-46d0-8b62-52a98e1b4bdd", StringComparison.OrdinalIgnoreCase));
		}

		[TestCase]
		public void CanConvertUUIDToGuid()
		{
			UUID expected = new UUID(0x4e5e7aecbdf644c5, 0xa3aa19859bb67964);
			Guid converted = expected;
			Assert.AreEqual(expected.ToString().ToLower(), converted.ToString().ToLower());
		}

		[TestCase]
		public void CanConvertGuidToUUID()
		{
			Guid expected = Guid.Parse("4e5e7aec-bdf6-44c5-a3aa-19859bb67964");
			UUID converted = expected;
			Assert.That(expected.ToString().ToLower(), Is.EqualTo(converted.ToString().ToLower()));
		}

		[TestCase("123-45678-90AB-CDEF-1011-121314151617", false)]
		[TestCase("12345678-90ab-cdef-1011-12131415161", false)]
		[TestCase("12345678-90ab-cdef-1011-121314151617", true)]
		[TestCase("12345678-90AB-CDEF-1011-121314151617", true)]
		[TestCase("12345678-90Ab-CdEf-1011-121314151617", true)]
		[TestCase("1234567890abcdef1011121314151617", true)]
		[TestCase("1234567890ABCDEF1011121314151617", true)]
		[TestCase("1234567890AbCdEf1011121314151617", true)]
		public void CanCreateFromString(String str, bool isCorrect)
		{
			bool hasDashes = str.Contains("-");
			bool isLowerCase = str.ToLower().Equals(str);
			bool isUpperCase = str.ToUpper().Equals(str);

			TestFromStringFormat(str, UUIDParseFormat.LowerCase, isCorrect && hasDashes && isLowerCase);
			TestFromStringFormat(str, UUIDParseFormat.UpperCase, isCorrect && hasDashes && isUpperCase);
			TestFromStringFormat(str, UUIDParseFormat.AnyCase, isCorrect && hasDashes);

			TestFromStringFormat(str, UUIDParseFormat.LowerCaseWithoutDashes, isCorrect && !hasDashes && isLowerCase);
			TestFromStringFormat(str, UUIDParseFormat.UpperCaseWithoutDashes, isCorrect && !hasDashes && isUpperCase);
			TestFromStringFormat(str, UUIDParseFormat.AnyCaseWithoutDashes, isCorrect && !hasDashes);

			TestFromStringFormat(str, UUIDParseFormat.Any, isCorrect);
		}

		private void TestFromStringFormat(String str, UUIDParseFormat format, bool isCorrect)
		{
			if (isCorrect)
			{
				UUID uuid = new UUID(str, format);
				Assert.AreEqual((UUID)Guid.Parse(str), uuid);
			}
			else
			{
				try
				{
					UUID uuid = new UUID(str, format);
					Assert.Fail("UUID: " + str + ". format: " + format);
				}
				catch (Exception)
				{
				}
			}
		}

		[TestCase]
		public void CanConvertToHexString()
		{
			Assert.AreEqual("00000000000000000000000000000000", UUID.Empty.ToHexString());
			Assert.AreEqual("00000000000000000000000000000000", new UUID().ToHexString());
			Assert.AreEqual("0123456789ABCDEF1011121314151617", new UUID("0123456789ABCDEF1011121314151617").ToHexString());
			Assert.AreEqual("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", new UUID(ulong.MaxValue, ulong.MaxValue).ToHexString());

			for (int i = 0; i < 100; ++i)
			{
				UUID uuid = UUID.Random();
				Assert.AreEqual(uuid, new UUID(uuid.ToHexString()));
			}
		}
	}
}

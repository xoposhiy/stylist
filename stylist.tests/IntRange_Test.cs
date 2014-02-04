using System;
using Newtonsoft.Json;
using NUnit.Framework;
using stylist.Checkers;

namespace stylist.tests
{
	[TestFixture]
	public class IntRange_Test
	{
		[Test]
		public void Test()
		{
			string serialized = JsonConvert.SerializeObject(new IntRange(10, 20));
			var deserialized = JsonConvert.DeserializeObject<IntRange>(serialized);
			Assert.AreEqual(10, deserialized.Min);
			Assert.AreEqual(20, deserialized.Max);
		}

		[Test]
		public void Test1()
		{
			var ch = new NameLengthChecker();
			JsonConvert.PopulateObject("{TypeNameLength: {Min: 1, Max: 1}}", ch);
			Console.WriteLine(ch.TypeNameLength);
		}
	}
}
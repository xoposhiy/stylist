using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using stylist.Checkers;

namespace stylist.tests
{
	[TestFixture]
	public class StyleChecker_Test
	{
		[TestFixtureSetUp]
		public void SetUpSpeller()
		{
			Speller.Initialize("spell");
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			Speller.DisposeInstance();
		}

		[Test]
		public void Creation_with_options()
		{
			var options = JsonConvert.DeserializeObject<CheckerOption[]>(
@"[
	{Checker: 'MethodLength', Options: {MaxStatementsPerMethod: 10} },
	{Checker: 'NamingCase' },
	{Checker: 'NamingLength', Options: {TypeNameLength: {Min:1, Max:1}} }
]");
			BaseChecker[] checkers = new StyleChecker(Speller.Instance, options).Checkers;
			Assert.AreEqual(10, checkers.OfType<MethodLengthChecker>().First().MaxStatementsPerMethod);
			Assert.AreEqual(new IntRange(1, 1), checkers.OfType<NamingLengthChecker>().First().TypeNameLength);
			CollectionAssert.IsEmpty(checkers.OfType<NamingCaseChecker>());
		}

		[Test]
		public void NamingCase()
		{
			RunTest("NamingCase", new NamingCaseChecker());
		}

		[Test]
		public void NamingLength()
		{
			RunTest("NamingLength", new NamingLengthChecker());
		}

		[Test]
		public void MethodLength()
		{
			RunTest("MethodLength", new MethodLengthChecker());
		}
		
		[Test]
		public void Naming()
		{
			RunTest("Naming", new NamingChecker());
		}

		private static void RunTest(string testName, params BaseChecker[] checkers)
		{
			TestCase testCase = TestCases.Get(testName);
			var styleChecker = new StyleChecker(checkers);
			List<string> actualIssues = styleChecker.Check(testCase.Source).Select(issue => issue.ToString()).ToList();
			foreach (string codeIssue in actualIssues)
				Console.WriteLine(codeIssue);
			CollectionAssert.AreEquivalent(testCase.CodeIssues, actualIssues);
		}
	}

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
			var ch = new NamingLengthChecker();
			JsonConvert.PopulateObject("{TypeNameLength: {Min: 1, Max: 1}}", ch);
			Console.WriteLine(ch.TypeNameLength);
		}
	}
}
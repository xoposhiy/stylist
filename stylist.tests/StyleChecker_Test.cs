using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace stylist.tests
{
	[TestFixture]
	public class StyleChecker_Test
	{
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
}
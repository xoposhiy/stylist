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
	{Checker: 'NameLength', Options: {TypeNameLength: {Min:1, Max:1}} }
]");
			BaseAstChecker[] checkers = new StyleChecker(Speller.Instance, options).Checkers.OfType<BaseAstChecker>().ToArray();
			Assert.AreEqual(10, checkers.OfType<MethodLengthChecker>().First().MaxStatementsPerMethod);
			Assert.AreEqual(new IntRange(1, 1), checkers.OfType<NameLengthChecker>().First().TypeNameLength);
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
			RunTest("NamingLength", new NameLengthChecker());
		}

		[Test]
		public void MethodLength()
		{
			RunTest("MethodLength", new MethodLengthChecker());
		}

		[Test]
		public void Naming()
		{
			RunTest("Naming", new PredictableNamingChecker());
		}

		[Test]
		public void Formatting()
		{
		     RunTest("Formatting", new IndentationChecker());
		}

		[Test]
		public void FormattingLambda()
		{
			RunTest("FormattingLambda", new IndentationChecker());
		}

		[Test]
		public void FormattingKR()
		{
			RunTest("FormattingKR", new IndentationChecker());
		}
	
		[Test]
		public void ArgumentsNumber()
		{
			RunTest("ArgumentsNumber", new ArgumentsNumberChecker());
		}
	
		[Test]
		public void ReturnBool()
		{
			RunTest("ReturnBool", new RedundantIfChecker());
		}

		[Test]
		public void WorstCodeEver()
		{
			RunTest("WorstCodeEver", StyleChecker.CreateBaseCheckers(Speller.Instance).ToArray());
		}

		private static void RunTest(string testName, params IChecker[] checkers)
		{
			TestCase testCase = TestCases.Get(testName);
			var styleChecker = new StyleChecker(checkers);
			List<string> actualIssues = styleChecker.Check(testCase.Source).Select(issue => issue.ToString()).ToList();
			foreach (string codeIssue in actualIssues)
				Console.WriteLine(codeIssue);
			CollectionAssert.AreEquivalent(testCase.CodeIssues, actualIssues);
		}

		[Test]
		[Explicit]
		public void RunChecker()
		{
//			Console.WriteLine(
//				new FormattingChecker().AnalayzeFile(
//					@"d:\work\stylist\packages\navigationroutes.mvc4.1.0.30130\Content\NavigationRoutes\NamedRoute.cs")
//					.FirstOrDefault());
//			new FormattingChecker().ReportErrorsToConsole(@"d:\work\stylist\NREf", showFiles: false, showErrors: false);
//			new FormattingChecker().ReportErrorsToConsole(@"d:\work\stylist\NREf", showFiles: true, showErrors: true);
			new ArgumentsNumberChecker().ReportErrorsToConsole(@"d:\work\stylist", showFiles: true, showErrors: true);
			//old scheme: 313 2936
			//new scheme: 352 3149
			//+ifelse and for: 113 657
			// ignore spaces mixing with tabs: 68 494
		}

	}
}
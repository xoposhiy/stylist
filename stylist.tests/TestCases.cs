using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist.tests
{
	public class TestCases
	{
		public static TestCase Get(string name)
		{
			return LoadTestCase("TestSources\\" + name + ".cs");
		}

		public static IEnumerable<TestCase> All()
		{
			return Directory.EnumerateFiles("TestSources").Select(LoadTestCase);
		}

		private static TestCase LoadTestCase(string file)
		{
			var source = File.ReadAllText(file);
			var ast = new CSharpParser().Parse(source);
			var expectedIssues = GetTrailingComments(ast);
			return new TestCase {CodeIssues = expectedIssues.ToArray(), Source = source};
		}

		private static IEnumerable<string> GetTrailingComments(SyntaxTree ast)
		{
			return ast.Descendants
				.Reverse()
				.Select(node => node as Comment)
				.TakeWhile(comment => comment != null)
				.Select(comment => comment.Content);
		}
	}
}
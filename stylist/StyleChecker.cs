using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class StyleChecker
	{
		private readonly BaseChecker[] checkers;

		public StyleChecker()
		:this(
			new NamingCaseChecker(), 
			new NamingLengthChecker(), 
			new NamingChecker(), 
			new MethodLengthChecker()
			)
		{
		}

		public StyleChecker(params BaseChecker[] checkers)
		{
			this.checkers = checkers;
		}

		public CodeStyleIssue[] Check(string source)
		{
			var ast = new CSharpParser().Parse(source);
			var codeIssues = new List<CodeStyleIssue>();
			foreach (var checker in checkers)
			{
				checker.Initialize(codeIssues);
				ast.AcceptVisitor(checker);
			}
			return codeIssues.ToArray();
		}
	}
}
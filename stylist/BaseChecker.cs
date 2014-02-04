using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class CodeIssues
	{
		public readonly List<CodeStyleIssue> Issues = new List<CodeStyleIssue>();

		public void Report(IChecker checker, string description, AstNode node)
		{
			Report(checker, description, new TextSpan(node));
		}

		public void Report(IChecker checker, string description, TextSpan span)
		{
			Issues.Add(new CodeStyleIssue(GetCheckerName(checker), description, span));
		}

		private static string GetCheckerName(IChecker checker)
		{
			var checkerName = checker.GetType().Name;
			if (checkerName.EndsWith("Checker"))
				return checkerName.Substring(0, checkerName.Length - "Checker".Length);
			return checkerName;
		}
	}

	public class BaseAstChecker : DepthFirstAstVisitor, IChecker
	{
		protected CodeIssues codeIssues;

		public void Initialize(CodeIssues result)
		{
			codeIssues = result;
		}
	}
}
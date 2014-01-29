using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class CodeIssues
	{
		public readonly List<CodeStyleIssue> Issues = new List<CodeStyleIssue>();

		public void Report(string issueId, string description, AstNode node)
		{
			Report(issueId, description, new TextSpan(node));
		}

		public void Report(string issueId, string description, TextSpan span)
		{
			Issues.Add(new CodeStyleIssue(issueId, description, span));
		}

		public void Check(bool condition, string issueId, string description, AstNode node)
		{
			if (!condition) Report(issueId, description, node);
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
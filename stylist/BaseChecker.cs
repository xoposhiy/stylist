using System.Collections.Generic;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class BaseChecker : DepthFirstAstVisitor
	{
		protected List<CodeStyleIssue> codeIssues;

		public void Initialize(List<CodeStyleIssue> result)
		{
			codeIssues = result;
		}

		protected void ReportIssue(string issueId, string description, AstNode node)
		{
			codeIssues.Add(new CodeStyleIssue(issueId, description, new TextSpan(node)));
		}
		
		protected void CheckIssue(bool condition, string issueId, string description, AstNode node)
		{
			if (!condition)
				codeIssues.Add(new CodeStyleIssue(issueId, description, new TextSpan(node)));
		}
	}
}
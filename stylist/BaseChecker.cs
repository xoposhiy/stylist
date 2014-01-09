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
	}
}
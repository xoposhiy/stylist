using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class MethodLengthChecker : BaseChecker
	{
		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			int count = methodDeclaration.Descendants.Count(node => node is Statement);
			if (count > 20)
				codeIssues.Add(new CodeStyleIssue("Complexity", "Too long method. Try to divide it into smaller parts.",
					new TextSpan(methodDeclaration.NameToken)));
			base.VisitMethodDeclaration(methodDeclaration);
		}
	}
}
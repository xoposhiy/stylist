using System;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;

namespace stylist.Checkers
{
	public class NamingChecker : BaseChecker
	{
		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			if (methodDeclaration.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
			{
				var type = methodDeclaration.ReturnType as PrimitiveType;
				if (type != null && type.KnownTypeCode == KnownTypeCode.Void)
					codeIssues.Add(new CodeStyleIssue("Naming.Verbs", "'Get' method without return value is confusing",
						new TextSpan(methodDeclaration.NameToken)));
			}
			if (methodDeclaration.Name.StartsWith("Set", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!methodDeclaration.Parameters.Any())
					codeIssues.Add(new CodeStyleIssue("Naming.Verbs", "'Set' method without arguments is confusing",
						new TextSpan(methodDeclaration.NameToken)));
			}
			base.VisitMethodDeclaration(methodDeclaration);
		}
	}
}
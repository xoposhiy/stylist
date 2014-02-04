using System;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;

namespace stylist.Checkers
{
	public class PredictableNamingChecker : BaseAstChecker
	{
		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			if (methodDeclaration.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
			{
				var type = methodDeclaration.ReturnType as PrimitiveType;
				if (type != null && type.KnownTypeCode == KnownTypeCode.Void)
					codeIssues.Report(this, "'Get' method without return value is confusing", methodDeclaration.NameToken);
			}
			if (methodDeclaration.Name.StartsWith("Set", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!methodDeclaration.Parameters.Any())
					codeIssues.Report(this, "'Set' method without arguments is confusing", methodDeclaration.NameToken);
			}
			base.VisitMethodDeclaration(methodDeclaration);
		}
	}
}
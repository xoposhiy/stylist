using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class ArgumentsNumberChecker : BaseAstChecker
	{
		public ArgumentsNumberChecker()
		{
			MaxArgumentsCount = 4;
		}

		public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
		{
			var parameterDeclarations = parameterDeclaration.PrevSiblings().OfType<ParameterDeclaration>();
			var parametersCount = parameterDeclarations.Count();
			if (parametersCount >= MaxArgumentsCount)
				codeIssues.Report(this, "Too many arguments", parameterDeclaration);
			base.VisitParameterDeclaration(parameterDeclaration);
		}
		public static string Url = "http://www.slideshare.net/xoposhiy/clean-ode/8";

		public int MaxArgumentsCount { get; set; }
	}
}
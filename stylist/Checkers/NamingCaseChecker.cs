using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class NamingCaseChecker : BaseNamingChecker
	{
		private static bool WrongNaming(bool shouldStartWithUpper, Identifier id)
		{
			return !string.IsNullOrEmpty(id.Name) && char.IsUpper(id.Name[0]) != shouldStartWithUpper;
		}

		private bool ShouldStartWithUpper(AstNode node)
		{
			return node is TypeParameterDeclaration
					|| node is TypeDeclaration
					|| node is EnumMemberDeclaration
					|| node is MethodDeclaration
					|| node is FieldDeclaration && !(node as FieldDeclaration).HasModifier(Modifiers.Private);
		}

		protected override void CheckName(Identifier identifier, AstNode node)
		{
			bool shouldStartWithUpper = ShouldStartWithUpper(node);
			if (WrongNaming(shouldStartWithUpper, identifier))
				codeIssues.Add(
					new CodeStyleIssue(
						"Naming.Case",
						string.Format("Use '{0}' naming here", shouldStartWithUpper ? "CamelCase" : "camelCase"),
						new TextSpan(identifier))
					);
		}
	}
}
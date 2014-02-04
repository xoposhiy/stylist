using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class NamingCaseChecker : BaseNamingChecker
	{
		private bool MustStartWithUpper(AstNode node)
		{
			return node is TypeParameterDeclaration
				|| node is TypeDeclaration
				|| node is EnumMemberDeclaration
				|| node is EntityDeclaration && (node as EntityDeclaration).HasModifier(Modifiers.Public);
		}

		private bool MustStartWithLower(AstNode node)
		{
			return node is VariableDeclarationStatement
				|| node is ParameterDeclaration
				|| node is FieldDeclaration && (node as FieldDeclaration).HasModifier(Modifiers.Private);
		}
		
		protected override void CheckName(Identifier identifier, AstNode node)
		{
			var name = identifier.Name;
			if (string.IsNullOrEmpty(name)) return;
			bool mustStartWithUpper = MustStartWithUpper(node);
			bool mustStartWithLower = MustStartWithLower(node);
			var isUpper = char.IsUpper(name[0]);
			var isLower = char.IsLower(name[0]);

			if (mustStartWithLower && !isLower)
				codeIssues.Report(this, "Use camelCaseNaming here", identifier);
			if (mustStartWithUpper && !isUpper)
				codeIssues.Report(this, "Use PascalCaseNaming here", identifier);
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class NamingLengthChecker : BaseChecker
	{
		private void AddIssues(IEnumerable<Identifier> ids, int minOkLength, int maxOkLength)
		{
			codeIssues.AddRange(
				ids
					.Select(id =>
						new CodeStyleIssue(
							"Naming.Length",
							id.Name.Length < minOkLength
								? "Too short name"
								: (id.Name.Length > maxOkLength ? "Too long name" : null),
							new TextSpan(id))
					)
					.Where(issue => issue.Description != null)
				);
		}

		public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
		{
			AddIssues(new[] {typeDeclaration.NameToken}, 3, 30);
			base.VisitTypeDeclaration(typeDeclaration);
		}

		public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
		{
			Statement block = parameterDeclaration.Parent.Block();
			if (block != null && !IsExceptionalName(parameterDeclaration.Name))
				AddIssues(new[] {parameterDeclaration.NameToken}, block.StatementsCount() > 2 ? 3 : 1, 20);
			base.VisitParameterDeclaration(parameterDeclaration);
		}

		private bool IsExceptionalName(string name)
		{
			return new[] {"x", "y", "z"}.Contains(name.ToLower());
		}

		public override void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
		{
			AddIssues(new[] {enumMemberDeclaration.NameToken}, 1, 30);
			base.VisitEnumMemberDeclaration(enumMemberDeclaration);
		}

		public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
		{
			AddIssues(fieldDeclaration.Variables.Where(v => !IsExceptionalName(v.Name)).Select(v => v.NameToken), 3, 30);
			base.VisitFieldDeclaration(fieldDeclaration);
		}

		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			AddIssues(new[] {methodDeclaration.NameToken}, 3, 30);
			base.VisitMethodDeclaration(methodDeclaration);
		}

		public override void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
		{
			AstNode parent = variableDeclarationStatement.Parent;
			var block = parent as BlockStatement;
			if (block != null)
				AddIssues(variableDeclarationStatement.Variables.Where(v => !IsExceptionalName(v.Name)).Select(v => v.NameToken), 3,
					20);
			else
			{
				int size = parent.Block().StatementsCount();
				AddIssues(variableDeclarationStatement.Variables.Where(v => !IsExceptionalName(v.Name)).Select(v => v.NameToken),
					size > 2 ? 3 : 1, 20);
			}
			base.VisitVariableDeclarationStatement(variableDeclarationStatement);
		}
	}
}
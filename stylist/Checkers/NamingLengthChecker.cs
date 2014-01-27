using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class NamingLengthChecker : BaseChecker
	{
		public IntRange TypeNameLength { get; set; }
		public IntRange ParameterNameLength { get; set; }
		public IntRange EnumMemberNameLength { get; set; }
		public IntRange FieldNameLength { get; set; }
		public IntRange MethodNameLength { get; set; }
		public IntRange VariableNameLength { get; set; }
		public string[] AllowedShortVariableNames { get; set; }

		public NamingLengthChecker()
		{
			TypeNameLength = new IntRange(3, 30);
			ParameterNameLength = new IntRange(3, 20);
			EnumMemberNameLength = new IntRange(1, 20);
			FieldNameLength = new IntRange(3, 30);
			MethodNameLength = new IntRange(3, 30);
			VariableNameLength = new IntRange(3, 30);
			AllowedShortVariableNames = new string[0];
		}

		private void AddIssues(IEnumerable<Identifier> ids, IntRange lengthConstraint)
		{
			codeIssues.AddRange(
				ids
					.Select(id =>
						new CodeStyleIssue(
							"Naming.Length",
							id.Name.Length < lengthConstraint.Min
								? "Too short name"
								: (id.Name.Length > lengthConstraint.Max ? "Too long name" : null),
							new TextSpan(id))
					)
					.Where(issue => issue.Description != null)
				);
		}

		public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
		{
			AddIssues(new[] {typeDeclaration.NameToken}, TypeNameLength);
			base.VisitTypeDeclaration(typeDeclaration);
		}

		public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
		{
			Statement block = parameterDeclaration.Parent.Block();
			if (block != null && !IsAllowedVariableName(parameterDeclaration.Name))
				AddIssues(new[] {parameterDeclaration.NameToken}, block.StatementsCount() > 2 ? ParameterNameLength : ParameterNameLength.WithMin(1));
			base.VisitParameterDeclaration(parameterDeclaration);
		}

		private bool IsAllowedVariableName(string name)
		{
			return AllowedShortVariableNames.Contains(name.ToLower());
		}

		public override void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
		{
			AddIssues(new[] {enumMemberDeclaration.NameToken}, EnumMemberNameLength);
			base.VisitEnumMemberDeclaration(enumMemberDeclaration);
		}

		public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
		{
			AddIssues(fieldDeclaration.Variables.Where(v => !IsAllowedVariableName(v.Name)).Select(v => v.NameToken), FieldNameLength);
			base.VisitFieldDeclaration(fieldDeclaration);
		}

		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			AddIssues(new[] {methodDeclaration.NameToken}, MethodNameLength);
			base.VisitMethodDeclaration(methodDeclaration);
		}

		public override void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
		{
			AstNode parent = variableDeclarationStatement.Parent;
			var block = parent as BlockStatement;
			if (block != null)
				AddIssues(variableDeclarationStatement.Variables.Where(v => !IsAllowedVariableName(v.Name)).Select(v => v.NameToken), VariableNameLength);
			else
			{
				int size = parent.Block().StatementsCount();
				AddIssues(variableDeclarationStatement.Variables.Where(v => !IsAllowedVariableName(v.Name)).Select(v => v.NameToken),
					size > 2 ? VariableNameLength : VariableNameLength.WithMin(1));
			}
			base.VisitVariableDeclarationStatement(variableDeclarationStatement);
		}
	}
}
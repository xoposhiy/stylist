using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class NameLengthChecker : BaseAstChecker
	{
		public NameLengthChecker()
		{
			TypeNameLength = new IntRange(3, 30);
			ParameterNameLength = new IntRange(3, 20);
			EnumMemberNameLength = new IntRange(1, 20);
			FieldNameLength = new IntRange(3, 30);
			MethodNameLength = new IntRange(3, 30);
			VariableNameLength = new IntRange(3, 30);
			AllowedShortVariableNames = new[]{"id", "x", "y", "z"};
		}

		public IntRange TypeNameLength { get; set; }
		public IntRange ParameterNameLength { get; set; }
		public IntRange EnumMemberNameLength { get; set; }
		public IntRange FieldNameLength { get; set; }
		public IntRange MethodNameLength { get; set; }
		public IntRange VariableNameLength { get; set; }
		public string[] AllowedShortVariableNames { get; set; }

		private void AddIssues(IEnumerable<Identifier> ids, IntRange lengthConstraint)
		{
			foreach (var id in ids)
			{
				if (string.IsNullOrWhiteSpace(id.Name)) continue;
				if (id.Name.Length < lengthConstraint.Min)
					codeIssues.Report(this, "Too short name", id);
				else if (id.Name.Length > lengthConstraint.Max)
					codeIssues.Report(this, "Too long name", id);
			}
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
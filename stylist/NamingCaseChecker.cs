using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;

namespace stylist
{
	public class NamingChecker : BaseChecker
	{
		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			if (methodDeclaration.Name.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
			{
				var type = methodDeclaration.ReturnType as PrimitiveType;
				if (type != null && type.KnownTypeCode == KnownTypeCode.Void)
					codeIssues.Add(new CodeStyleIssue("Naming.Verbs", "'Get' method without return value is confusing", new TextSpan(methodDeclaration.NameToken)));
			}
			if (methodDeclaration.Name.StartsWith("Set", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!methodDeclaration.Parameters.Any())
					codeIssues.Add(new CodeStyleIssue("Naming.Verbs", "'Set' method without arguments is confusing", new TextSpan(methodDeclaration.NameToken)));
			}
			base.VisitMethodDeclaration(methodDeclaration);
		}
	}

	public class NamingCaseChecker : BaseChecker
	{
		private void AddNamingIssues(IEnumerable<Identifier> ids, string nodeDescription, bool shouldStartWithUpper)
		{
			codeIssues.AddRange(
				ids.Where(id => WrongNaming(shouldStartWithUpper, id))
					.Select(id =>
						new CodeStyleIssue(
							"Naming.Case",
							string.Format("Use '{1}' naming style for {0}", nodeDescription, shouldStartWithUpper ? "CamelCase" : "camelCase"),
							new TextSpan(id.StartLocation, id.EndLocation))));
		}

		private static bool WrongNaming(bool shouldStartWithUpper, Identifier id)
		{
			return !string.IsNullOrEmpty(id.Name) && char.IsUpper(id.Name[0]) != shouldStartWithUpper;
		}

		public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
		{
			AddNamingIssues(new[] { typeDeclaration.NameToken }, "classes", shouldStartWithUpper: true);
			base.VisitTypeDeclaration(typeDeclaration);
		}

		public override void VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
		{
			AddNamingIssues(new[] { typeParameterDeclaration.NameToken }, "type parameters", shouldStartWithUpper: true);
			base.VisitTypeParameterDeclaration(typeParameterDeclaration);
		}

		public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
		{
			AddNamingIssues(new[] { parameterDeclaration.NameToken }, "arguments", shouldStartWithUpper: false);
			base.VisitParameterDeclaration(parameterDeclaration);
		}

		public override void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
		{
			AddNamingIssues(new[] { enumMemberDeclaration.NameToken }, "enum members", shouldStartWithUpper: true);
			base.VisitEnumMemberDeclaration(enumMemberDeclaration);
		}

		public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
		{
			AddNamingIssues(fieldDeclaration.Variables.Select(v => v.NameToken), "fields", shouldStartWithUpper: fieldDeclaration.HasModifier(Modifiers.Public));
			base.VisitFieldDeclaration(fieldDeclaration);
		}

		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			AddNamingIssues(new[] { methodDeclaration.NameToken }, "methods", shouldStartWithUpper: true);
			base.VisitMethodDeclaration(methodDeclaration);
		}

		public override void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
		{
			AddNamingIssues(variableDeclarationStatement.Variables.Select(v => v.NameToken), "variables", shouldStartWithUpper: false);
			base.VisitVariableDeclarationStatement(variableDeclarationStatement);
		}
	}
}
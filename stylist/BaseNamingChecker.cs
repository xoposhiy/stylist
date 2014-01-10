using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public abstract class BaseNamingChecker : BaseChecker
	{
		protected abstract void CheckName(Identifier identifier, AstNode node);

		public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
		{
			CheckName(typeDeclaration.NameToken, typeDeclaration);
			base.VisitTypeDeclaration(typeDeclaration);
		}

		public override void VisitTypeParameterDeclaration(TypeParameterDeclaration typeParameterDeclaration)
		{
			CheckName(typeParameterDeclaration.NameToken, typeParameterDeclaration);
			base.VisitTypeParameterDeclaration(typeParameterDeclaration);
		}

		public override void VisitParameterDeclaration(ParameterDeclaration parameterDeclaration)
		{
			CheckName(parameterDeclaration.NameToken, parameterDeclaration);
			base.VisitParameterDeclaration(parameterDeclaration);
		}

		public override void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
		{
			CheckName(enumMemberDeclaration.NameToken, enumMemberDeclaration);
			base.VisitEnumMemberDeclaration(enumMemberDeclaration);
		}

		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			CheckName(methodDeclaration.NameToken, methodDeclaration);
			base.VisitMethodDeclaration(methodDeclaration);
		}

		public override void VisitFieldDeclaration(FieldDeclaration fieldDeclaration)
		{
			foreach(var name in fieldDeclaration.Variables.Select(v => v.NameToken))
				CheckName(name, fieldDeclaration);
			base.VisitFieldDeclaration(fieldDeclaration);
		}
		
		public override void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement)
		{
			foreach (var name in variableDeclarationStatement.Variables.Select(v => v.NameToken))
				CheckName(name, variableDeclarationStatement);
			base.VisitVariableDeclarationStatement(variableDeclarationStatement);
		}
	}
}
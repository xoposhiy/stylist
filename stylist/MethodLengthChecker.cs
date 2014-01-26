using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class MethodLengthChecker : BaseChecker
	{
		public MethodLengthChecker()
		{
			MaxStatementsPerMethod = 20;
		}

		public int MaxStatementsPerMethod { get; set; }

		public override void VisitConstructorDeclaration(ConstructorDeclaration constructorDeclaration)
		{
			CheckMethodLength(constructorDeclaration);
			base.VisitConstructorDeclaration(constructorDeclaration);
		}

		public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
		{
			CheckMethodLength(propertyDeclaration.Getter, propertyDeclaration.NameToken);
			CheckMethodLength(propertyDeclaration.Setter, propertyDeclaration.NameToken);
			base.VisitPropertyDeclaration(propertyDeclaration);
		}

		public override void VisitOperatorDeclaration(OperatorDeclaration operatorDeclaration)
		{
			CheckMethodLength(operatorDeclaration);
			base.VisitOperatorDeclaration(operatorDeclaration);
		}

		public override void VisitDestructorDeclaration(DestructorDeclaration destructorDeclaration)
		{
			CheckMethodLength(destructorDeclaration);
			base.VisitDestructorDeclaration(destructorDeclaration);
		}

		public override void VisitMethodDeclaration(MethodDeclaration methodDeclaration)
		{
			CheckMethodLength(methodDeclaration);
			base.VisitMethodDeclaration(methodDeclaration);
		}

		private void CheckMethodLength(EntityDeclaration declaration, Identifier nameToken = null)
		{
			nameToken = nameToken ?? declaration.NameToken;
			int count = declaration.Descendants.Count(node => node is Statement);
			if (count > MaxStatementsPerMethod)
				codeIssues.Add(new CodeStyleIssue("Complexity", "Too long method. Try to divide it into smaller parts.",
					new TextSpan(nameToken)));
		}
	}
}
using System;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public static class AstNodeExtensions
	{
		public static V As<T, V>(this AstNode node, Func<T, V> convert) 
			where T : AstNode 
			where V : class
		{
			var t = node as T;
			return t == null ? null : convert(t);
		}

		public static Statement Block(this AstNode node)
		{
			return
				node.As((ForStatement f) => f.EmbeddedStatement)
				?? node.As((ForeachStatement s) => s.EmbeddedStatement)
				?? node.As((MethodDeclaration s) => s.Body)
				?? node.As((AnonymousMethodExpression s) => s.Body)
				?? node.As((ConstructorDeclaration s) => s.Body)
				?? node.As((DestructorDeclaration s) => s.Body);
		}

		public static int StatementsCount(this AstNode node)
		{
			return node.Descendants.Count(n => n is Statement);
		}
	}
}
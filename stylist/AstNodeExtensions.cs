using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public static class UsefulExtensions
	{
		public static string[] AsLines(this string text)
		{
			return Regex.Split(text, "\r\n|\r|\n");
		}

		public static TOutput Call<TInput, TOutput>(this TInput input, Func<TInput, TOutput> f)
			where TInput : class
			where TOutput : class
		{
			return input == null ? null : f(input);
		}
	}

	public static class AstNodeExtensions
	{
		public static V As<T, V>(this AstNode node, Func<T, V> convert)
			where T : AstNode
			where V : class
		{
			var t = node as T;
			return t == null ? null : convert(t);
		}

		public static IEnumerable<AstNode> PrevSiblings(this AstNode node)
		{
			while (node.PrevSibling != null)
				yield return node = node.PrevSibling;
		}

		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (var item in items)
				action(item);
		}

		public static IEnumerable<AstNode> NextSiblings(this AstNode node)
		{
			while (node.NextSibling != null)
				yield return node = node.NextSibling;
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
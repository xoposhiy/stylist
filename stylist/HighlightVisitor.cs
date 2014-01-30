using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class Highlight
	{
		public Highlight(TextSpan span, CodeSpanType type)
		{
			Span = span;
			Type = type;
		}

		public TextSpan Span;
		public CodeSpanType Type;
	}
	public class HighlightVisitor : DepthFirstAstVisitor
	{
		protected override void VisitChildren(AstNode node)
		{
			if (IsKeyword(node))
				AddHighlignting(node, CodeSpanType.Keyword);
			else if (node is Comment)
				AddHighlignting(node, CodeSpanType.Comment);
			else if (node is PrimitiveExpression && ((PrimitiveExpression)node).Value is string)
				AddHighlignting(node, CodeSpanType.String);
			base.VisitChildren(node);
		}

		public override void VisitSimpleType(SimpleType simpleType)
		{
			if (simpleType.Identifier == "var" && simpleType.TypeArguments.Count == 0)
				AddHighlignting(simpleType, CodeSpanType.Keyword);
			base.VisitSimpleType(simpleType);
		}

		public override void VisitPrimitiveType(PrimitiveType primitiveType)
		{
			AddHighlignting(primitiveType, CodeSpanType.Keyword);
			base.VisitPrimitiveType(primitiveType);
		}

		private void AddHighlignting(AstNode node, CodeSpanType type)
		{
			Highlights.AddRange(
				TextSpan.Split(node.StartLocation, node.EndLocation).Select(span => new Highlight(span, type)));
		}

		private static bool IsKeyword(AstNode node)
		{
			var text = node.GetText();
			return node is CSharpTokenNode && (CSharpOutputVisitor.IsKeyword(text, node) || text == "yield");
		}

		public List<Highlight> Highlights = new List<Highlight>();
	}
}
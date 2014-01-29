using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class FormattingChecker : BaseChecker
	{
		protected override void VisitChildren(AstNode node)
		{
			if (node is Statement) CheckFormatting(node);
			base.VisitChildren(node);
		}

		private void CheckFormatting(AstNode node)
		{
			if (node is BlockStatement && OnSameLine(node))
				CheckIssue((node as BlockStatement).Statements.Count <= 1, "Formatting", "Place each statement of the block on seprate line", node);
			if (!OnSameLine(node))
				CheckIdentation(node, parentIdentation: GetIdentation(node.Parent), identNode: !(node is BlockStatement));
		}

		private int GetIdentation(AstNode node)
		{
			int elseColumn;
			return IsElseIfClause(node, out elseColumn) ? elseColumn : node.StartLocation.Column;
		}

		private bool OnSameLine(AstNode node)
		{
			return node.StartLocation.Line == node.Parent.StartLocation.Line;
		}

		private void CheckIdentation(AstNode node, int parentIdentation, bool identNode)
		{
			var parent = node.Parent;
			if (node.StartLocation.Line != parent.StartLocation.Line)
			{
				var nodeColumn = node.StartLocation.Column;
				if (identNode && nodeColumn <= parentIdentation)
					codeIssues.Add(new CodeStyleIssue("Formatting", "Identation error", new TextSpan(node)));
				if (!identNode && nodeColumn != parentIdentation)
					codeIssues.Add(new CodeStyleIssue("Formatting", "Identation error", new TextSpan(node)));
			}
		}


		private bool IsElseIfClause(AstNode node, out int elseColumn)
		{
			elseColumn = 0;
			var ifStatement = node as IfElseStatement;
			if (ifStatement == null) return false;
			var parentIf = ifStatement.Parent as IfElseStatement;
			if (parentIf == null) return false;
			if (ifStatement == parentIf.FalseStatement && ifStatement.StartLocation.Line == parentIf.ElseToken.StartLocation.Line)
			{
				elseColumn = parentIf.ElseToken.StartLocation.Column;
				return true;
			}
			return false;
		}
	}
}

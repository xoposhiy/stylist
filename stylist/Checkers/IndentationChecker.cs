using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class IndentationChecker : BaseAstChecker
	{
		private List<AstNode> firstInLine;

		public override void VisitSyntaxTree(SyntaxTree syntaxTree)
		{
			firstInLine = new List<AstNode>();
			base.VisitSyntaxTree(syntaxTree);
		}

		protected override void VisitChildren(AstNode node)
		{
			UpdateFirstInLine(node);
			CheckBlockStatementFormatting(node as BlockStatement);
			CheckStatementFormatting(node as Statement);
			CheckEntityFormatting(node as EntityDeclaration);
			base.VisitChildren(node);
		}

		private void UpdateFirstInLine(AstNode node)
		{
			if (node is SyntaxTree) return;
			while (node.StartLocation.Line - 1 > firstInLine.Count)
				firstInLine.Add(null);
			if (node.StartLocation.Line - 1 == firstInLine.Count)
				firstInLine.Add(node);
		}

		private void CheckBlockStatementFormatting(BlockStatement block)
		{
			if (block == null) return;
			if (!firstInLine.Contains(block)) return;
			var parent = FindStatementIndentationParent(block);
			if (parent != null && parent.StartLocation.Column != block.StartLocation.Column)
				codeIssues.Report(this, "Do not indent blocks", block);
		}

		private AstNode FindStatementIndentationParent(Statement block)
		{
			return block.Ancestors
				.Where(firstInLine.Contains)
				.FirstOrDefault(p => p is Statement || p is EntityDeclaration);
		}

		private void CheckStatementFormatting(Statement statement)
		{
			if (statement == null || statement is BlockStatement) return;
			if (statement.Parent is ForStatement) return;
			if (IsElseIfStatement(statement, statement.Parent)) return;
			if (IsUsing(statement, statement.Parent)) return;
			var isFirstInLine = firstInLine.Contains(statement);
			var prevSiblingStatement = statement.PrevSiblings().FirstOrDefault(s => s is Statement);
			if (statement.Parent is BlockStatement && prevSiblingStatement != null)
			{
				if (!(isFirstInLine && SameIndentation(statement, prevSiblingStatement)))
					codeIssues.Report(this, "Statements in block should have same indentation", statement);
			}
			else if (isFirstInLine)
			{
				var parent = FindStatementIndentationParent(statement);
				if (parent != null && !Indented(statement, parent))
					codeIssues.Report(this, "Missing indentation", statement);
			}
			else if (statement.Parent is BlockStatement)
			{
				if (statement.NextSiblings().Any(s => s is Statement))
					codeIssues.Report(this, "Every statement in block should be placed on its own line", statement);
			}
		}

		private bool IsUsing(Statement statement, AstNode usingStatement)
		{
			var theUsing = usingStatement as UsingStatement;
			return theUsing != null && theUsing.ResourceAcquisition == statement && theUsing.UsingToken.StartLocation.Line == statement.StartLocation.Line;
		}

		private bool IsElseIfStatement(Statement ifStatement, AstNode elseStatement)
		{
			var parentIf = elseStatement as IfElseStatement;
			return ifStatement is IfElseStatement && parentIf != null &&
					parentIf.FalseStatement == ifStatement && parentIf.ElseToken.StartLocation.Line == ifStatement.StartLocation.Line;
		}

		private bool SameIndentation(AstNode node, AstNode parent)
		{
			return node.StartLocation.Column == parent.StartLocation.Column;
		}

		private bool Indented(AstNode node, AstNode parent)
		{
			return node.StartLocation.Column > parent.StartLocation.Column;
		}

		private void CheckEntityFormatting(EntityDeclaration entity)
		{
			if (entity == null) return;
			if (!(entity is Accessor) && !firstInLine.Contains(entity))
				codeIssues.Report(this, "Entity declaration should start on the separate line", entity);
			var prevSiblingEntity = entity.PrevSiblings().FirstOrDefault(s => s is EntityDeclaration);
			if (prevSiblingEntity != null && !(entity is Accessor) && !SameIndentation(entity, prevSiblingEntity))
				codeIssues.Report(this, "Sibling entities should have same indentation", entity);
			var parent = FindEntityIndentationParent(entity);
			if (parent != null && !Indented(entity, parent))
				codeIssues.Report(this, "Missing indentation", entity);
		}

		private AstNode FindEntityIndentationParent(EntityDeclaration entity)
		{
			return entity.Ancestors
				.Where(firstInLine.Contains)
				.FirstOrDefault(p => p is EntityDeclaration || p is NamespaceDeclaration);
		}
	}
}

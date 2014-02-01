using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class ReturnBoolChecker : BaseAstChecker
	{
		public override void VisitIfElseStatement(IfElseStatement ifElseStatement)
		{
			var trueReturnValue =
				(ifElseStatement.TrueStatement as ReturnStatement)
					.Call(r => r.Expression as PrimitiveExpression)
					.Call(e => e.Value);
			var falseReturnValue =
				(ifElseStatement.FalseStatement as ReturnStatement)
					.Call(r => r.Expression as PrimitiveExpression)
					.Call(e => e.Value)
				??
				(ifElseStatement.NextSibling as ReturnStatement)
					.Call(r => r.Expression as PrimitiveExpression)
					.Call(e => e.Value);
			if (trueReturnValue != null && falseReturnValue != null && trueReturnValue is bool)
				codeIssues.Report("CanBeSimplified", "Use return instead of if statement", ifElseStatement);
			base.VisitIfElseStatement(ifElseStatement);
		}
	}
}
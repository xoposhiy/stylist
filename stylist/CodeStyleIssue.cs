namespace stylist
{
	public class CodeStyleIssue
	{
		public CodeStyleIssue(string issueId, string description, TextSpan span)
		{
			IssueId = issueId;
			Description = description;
			Span = span;
		}

		public string Description;
		public string IssueId;
		public TextSpan Span;

		public override string ToString()
		{
			return string.Format("{0}: {2} (Line: {1})", IssueId, Span.Line+1, Description);
		}
	}
}
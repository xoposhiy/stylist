namespace stylist
{
	public class CodeStyleIssue
	{
		public CodeStyleIssue(string checkerName, string description, TextSpan span)
		{
			CheckerName = checkerName;
			Description = description;
			Span = span;
		}

		public string Description;
		public string CheckerName;
		public TextSpan Span;
		public string Fragment;

		public override string ToString()
		{
			return string.Format("{2} (Line: {1})", CheckerName, Span.Line+1, Description);
		}
	}
}
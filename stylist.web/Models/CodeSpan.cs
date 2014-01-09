namespace stylist.web.Models
{
	public class CodeSpan
	{
		public CodeSpan(string text, params CodeStyleIssue[] issues)
		{
			Text = text;
			Issues = issues;
		}

		public string Text;
		public CodeStyleIssue[] Issues;
	}
}
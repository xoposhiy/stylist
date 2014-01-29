namespace stylist.web.Models
{
	public class CodeSpan
	{
		public CodeSpan(CodeSpanType type, string text, params CodeStyleIssue[] issues)
		{
			Text = text;
			Issues = issues;
			Type = type;
		}

		public string Text;
		public CodeSpanType Type;

		public string TextWithVisibleWhitespaces
		{
			get { return Text.Replace(" ", "\u00B7").Replace("\t", "→   "); }
		}
		public CodeStyleIssue[] Issues;
	}

	public enum CodeSpanType
	{
		Code,
		Comment,
		String,
		Keyword,
		Error
	}
}
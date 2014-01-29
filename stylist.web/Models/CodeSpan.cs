﻿namespace stylist.web.Models
{
	public class CodeSpan
	{
		public CodeSpan(string text, params CodeStyleIssue[] issues)
		{
			Text = text;
			Issues = issues;
		}

		public string Text;

		public string TextWithVisibleWhitespaces
		{
			get { return Text.Replace(" ", "\u00B7").Replace("\t", "→   "); }
		}
		public CodeStyleIssue[] Issues;
	}
}
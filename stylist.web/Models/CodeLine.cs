using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stylist.web.Models
{
	public class CodeLine
	{
		public CodeLine(string line, IEnumerable<CodeStyleIssue> codeIssues)
		{
			Spans = SplitIntoSpans(line, codeIssues.ToArray())
				.Where(span => !string.IsNullOrEmpty(span.Text))
				.ToArray();
		}

		private IEnumerable<CodeSpan> SplitIntoSpans(string line, CodeStyleIssue[] codeIssues)
		{
			var chunk = new StringBuilder();
			var issues = new List<CodeStyleIssue>();
			for (var colIndex = 0; colIndex < line.Length; colIndex++)
			{
				var endedIssues = issues.Where(issue => issue.Span.LastColumn == colIndex).ToList();
				var newIssues = codeIssues.Where(issue => issue.Span.FirstColumn == colIndex).ToList();
				if (endedIssues.Any() || newIssues.Any())
				{
					yield return new CodeSpan(chunk.ToString(), issues.ToArray());
					chunk.Clear();
					issues = issues.Except(endedIssues).Union(newIssues).ToList();
				}
				chunk.Append(line[colIndex]);
			}
			yield return new CodeSpan(chunk.ToString(), issues.ToArray());
		}

		public CodeSpan[] Spans;
	}
}
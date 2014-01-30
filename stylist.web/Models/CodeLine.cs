using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace stylist.web.Models
{
	public class CodeLine
	{
		private class SpanInfo
		{
			public SpanInfo(CodeStyleIssue issue)
			{
				FirstColumn = issue.Span.FirstColumn;
				LastColumn = issue.Span.LastColumn;
				Issue = issue;
				Type = CodeSpanType.Error;
			}

			public SpanInfo(int firstColumn, int lastColumn, CodeSpanType type)
			{
				FirstColumn = firstColumn;
				LastColumn = lastColumn;
				Type = type;
				Issue = null;
			}

			public readonly int FirstColumn;
			public readonly int LastColumn;
			public readonly CodeStyleIssue Issue;
			public readonly CodeSpanType Type;
		}

		[JsonConstructor]
		public CodeLine(CodeSpan[] spans)
		{
			Spans = spans;
		}

		public CodeLine(string line, IEnumerable<CodeStyleIssue> codeIssues, IEnumerable<Highlight> highlights)
		{
			var ii = codeIssues.Select(i => new SpanInfo(i));
			var hh = highlights.Select(h => new SpanInfo(h.Span.FirstColumn, h.Span.LastColumn, h.Type));
			Spans = SplitIntoSpans(line, ii.Concat(hh).ToArray())
				.Where(span => !string.IsNullOrEmpty(span.Text))
				.ToArray();
		}

		private IEnumerable<CodeSpan> SplitIntoSpans(string line, SpanInfo[] spanInfos)
		{
			var chunk = new StringBuilder();
			var spans = new List<SpanInfo>();
			for (var colIndex = 0; colIndex < line.Length; colIndex++)
			{
				var endedIssues = spans.Where(issue => issue.LastColumn == colIndex).ToList();
				var newIssues = spanInfos.Where(issue => issue.FirstColumn == colIndex).ToList();
				if (endedIssues.Any() || newIssues.Any())
				{
					yield return CreateCodeSpan(spans, chunk);
					chunk.Clear();
					spans = spans.Except(endedIssues).Union(newIssues).ToList();
				}
				chunk.Append(line[colIndex]);
			}
			yield return CreateCodeSpan(spans, chunk);
		}

		private static CodeSpan CreateCodeSpan(List<SpanInfo> spans, StringBuilder chunk)
		{
			CodeSpan codeSpan;
			if (!spans.Any())
				codeSpan = new CodeSpan(CodeSpanType.Code, chunk.ToString());
			else
			{
				var issues = spans.Select(s => s.Issue).ToList();
				if (issues.Any(i => i != null))
					codeSpan = new CodeSpan(CodeSpanType.Error, chunk.ToString(), issues.ToArray());
				else
				{
					var spanInfo = spans.First();
					codeSpan = new CodeSpan(spanInfo.Type, chunk.ToString());
				}
			}
			return codeSpan;
		}


		public CodeSpan[] Spans;
	}
}
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stylist.web.Models
{
	public static class CodeLineHtmlHelper
	{
		public static HtmlString CodeLine(this HtmlHelper html, CodeLine line, int lineNo, string baseClass)
		{
			return new HtmlString(
				"<span class='src-line'><a name='" + lineNo + "'></a>" 
				+ string.Join("", line.Spans.Select(span => FormatSpan(span, baseClass)))
				+ "</span>\n");
		}

		private static string FormatSpan(CodeSpan span, string baseClass)
		{
			var code = new TagBuilder("code");
			code.AddCssClass(baseClass);
			var additionalClass = GetClassName(span.Type);
			if (additionalClass != null)
				code.AddCssClass(baseClass + "-" + additionalClass);
			var issue = span.Issues.FirstOrDefault();
			if (issue != null)
			{
				code.MergeAttribute("title", issue.IssueId + " Issue");
				code.MergeAttribute("data-content", issue.Description);
			}
			code.SetInnerText(span.Text);
			return code.ToString(TagRenderMode.Normal);
		}

		private static string GetClassName(CodeSpanType type)
		{
			if (type == CodeSpanType.Error) return "error";
			if (type == CodeSpanType.Comment) return "comment";
			if (type == CodeSpanType.String) return "string";
			if (type == CodeSpanType.Keyword) return "keyword";
			return null;
		}
	}
}
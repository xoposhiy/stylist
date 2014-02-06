using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using stylist.web.Models;

namespace stylist.web.Helpers
{
	public static class CodeHtmlHelper
	{
		public static HtmlString CodeBadGoodBlock(this HtmlHelper html, string badCode, string goodCode)
		{
			return html.Partial("BadGoodBlock", Tuple.Create(badCode, goodCode));
		}

		public static HtmlString CodeBlock(this HtmlHelper html, string code)
		{
			var pre = new TagBuilder("pre");
			pre.InnerHtml = string.Join("", code.Trim().AsLines().Select(WrapCodeLine));
			return new HtmlString(pre.ToString());
		}

		public static HtmlString CodeLine(this WebViewPage page, CodeLine line, int lineNo, string baseClass)
		{
			return new HtmlString(
				WrapCodeLine(
				"<a name='" + lineNo + "'></a>"
				+ string.Join("", line.Spans.Select(span => page.FormatSpan(span, baseClass)))
				));
		}

		private static string WrapCodeLine(string codeLine)
		{
			return "<span class='src-line'>" + codeLine + "</span>\n";
		}

		private static string FormatSpan(this WebViewPage page, CodeSpan span, string baseClass)
		{
			var code = new TagBuilder("code");
			code.AddCssClass(baseClass);
			var additionalClass = GetClassName(span);
			if (additionalClass != null)
				code.AddCssClass(additionalClass);
			var issue = span.Issues.FirstOrDefault();
			if (issue != null)
			{
				code.MergeAttribute("title", "Code Style Issue");
				var description = string.Format("{0}<p><a target='blank' href='{1}#{2}'>Объяснение</a></p>", issue.Description, page.Url.Action("Explanations", "Code"), issue.CheckerName);
				code.MergeAttribute("data-content", description);
			}
			code.SetInnerText(span.Text);
			return code.ToString(TagRenderMode.Normal);
		}

		public static string GetClassName(this CodeSpan span)
		{
			var type = span.Type;
			if (type == CodeSpanType.Error) return "error";
			if (type == CodeSpanType.Comment) return "comment";
			if (type == CodeSpanType.String) return "string";
			if (type == CodeSpanType.Keyword) return "keyword";
			return "";
		}
	}
}
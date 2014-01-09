using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ICSharpCode.NRefactory.CSharp;
using Mono.Cecil.Cil;
using stylist.web.Models;

namespace stylist.web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		[ValidateInput(false)]
		public ActionResult Check(string code)
		{
			CodeStyleIssue[] issues = new StyleChecker().Check(code);
			var lines =
				Regex.Split(code, "\r\n|\r|\n")
				.Select((line, index) => BuildCodeLine(line, index, issues)).ToArray();

			return View(new CodeIssuesModel {Lines = lines, Code=code});
		}

		private CodeLine BuildCodeLine(string line, int lineIndex, CodeStyleIssue[] issues)
		{
			return new CodeLine(line, issues.Where(issue => issue.Span.Line == lineIndex));
		}
	}
}
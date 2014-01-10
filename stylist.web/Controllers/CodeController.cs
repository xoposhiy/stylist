using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using stylist.web.Models;

namespace stylist.web.Controllers
{
	public class CodeController : Controller
	{
		private const string delimiter = "\r\n==code==\r\n";
		private static readonly MD5 md5 = MD5.Create();

		public ActionResult Index()
		{
			return View();
		}

		[ValidateInput(false)]
		public ActionResult Check(string code)
		{
			string id = CalculateHash(code);
			string filename = GetFilename(id);
			CodeStyleIssue[] issues = CodeStyleIssues(code);
			var dateTime = new[]{DateTime.Now.ToString()};
			string issuesText = string.Join("\r\n", dateTime.Concat(issues.Select(issue => issue.ToString())));
			System.IO.File.WriteAllText(filename, issuesText + delimiter + code);
			return RedirectToAction("Issues", new {id});
		}

		public ActionResult Issues(string id)
		{
			string filename = GetFilename(id);
			string code = System.IO.File.ReadAllText(filename).Split(new[] {delimiter}, 2, StringSplitOptions.None)[1];
			CodeStyleIssue[] issues = CodeStyleIssues(code);
			CodeLine[] lines =
				Regex.Split(code, "\r\n|\r|\n")
					.Select((line, index) => BuildCodeLine(line, index, issues)).ToArray();
			return View(new CodeIssuesModel {Lines = lines, Code = code});
		}

		private string GetFilename(string id)
		{
			string dataPath = HttpContext.Server.MapPath("~/App_Data/code");
			return Path.Combine(dataPath, id + ".cs");
		}

		private static CodeStyleIssue[] CodeStyleIssues(string code)
		{
			return new StyleChecker(Speller.Instance).Check(code);
		}

		private static string CalculateHash(string code)
		{
			return string.Join("", md5.ComputeHash(Encoding.Unicode.GetBytes(code)).Select(b => b.ToString("x2")));
		}

		private CodeLine BuildCodeLine(string line, int lineIndex, CodeStyleIssue[] issues)
		{
			return new CodeLine(line, issues.Where(issue => issue.Span.Line == lineIndex));
		}
	}
}
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ICSharpCode.NRefactory.CSharp;
using Newtonsoft.Json;
using stylist.web.Models;

namespace stylist.web.Controllers
{
	public class CodeController : Controller
	{
		private const string delimiter = "\r\n==code==\r\n";
		private static readonly MD5 md5 = MD5.Create();

		public ActionResult Index(string profile)
		{

			return View(new CodeModel { Profile = SafeProfile(profile) });
		}

		[ValidateInput(false)]
		public ActionResult Check(string code, string profile)
		{
			string uid = GetUid();
			string id = uid + "_" + CalculateHash(code);
			string filename = GetFilename(id);
			CheckerOption[] options = LoadCheckerOptions(SafeProfile(profile));
			CodeStyleIssue[] issues = CodeStyleIssues(code, options);
			var dateTime = new[] { DateTime.Now.ToString() };
			string issuesText = string.Join("\r\n", dateTime.Concat(issues.Select(issue => issue.ToString())));
			System.IO.File.WriteAllText(filename, issuesText + delimiter + code);
			return RedirectToAction("Issues", new { id, profile });
		}

		private string GetUid()
		{
			if (Request.Cookies["UID"] != null) return Request.Cookies["UID"].Value;
			var value = Guid.NewGuid().ToString("N");
			Response.Cookies.Add(new HttpCookie("UID")
			{
				Expires = DateTime.Now + TimeSpan.FromDays(1000),
				Value = value,
			});
			return value;
		}

		public ActionResult Issues(string id, string profile)
		{
			string filename = GetFilename(id);
			CheckerOption[] options = LoadCheckerOptions(SafeProfile(profile));
			string code = System.IO.File.ReadAllText(filename).Split(new[] { delimiter }, 2, StringSplitOptions.None)[1];
			CodeStyleIssue[] issues = CodeStyleIssues(code, options);
			Highlight[] highlights = HighlightCode(code);
			CodeLine[] lines = code.AsLines()
				.Select((line, index) => BuildCodeLine(line, index, issues, highlights)).ToArray();
			return View(new CodeIssuesModel { Lines = lines, Code = code, Profile = profile, CodeIssues = issues });
		}

		public ActionResult Explanations()
		{
			return View();
		}

		private static string SafeProfile(string profile)
		{
			return new string((profile ?? "").Where(char.IsLetter).ToArray());
		}

		private CheckerOption[] LoadCheckerOptions(string safeProfile)
		{
			if (safeProfile == null) return new CheckerOption[0];
			string profilePath = HttpContext.Server.MapPath("~/App_Data/profiles/" + safeProfile + ".js");
			if (!System.IO.File.Exists(profilePath)) return new CheckerOption[0];
			return JsonConvert.DeserializeObject<CheckerOption[]>(System.IO.File.ReadAllText(profilePath));
		}

		private Highlight[] HighlightCode(string code)
		{
			var ast = new CSharpParser().Parse(code);
			var highlightVisitor = new HighlightVisitor();
			ast.AcceptVisitor(highlightVisitor);
			return highlightVisitor.Highlights.ToArray();
		}

		private string GetFilename(string id)
		{
			string dataPath = HttpContext.Server.MapPath("~/App_Data/code");
			return Path.Combine(dataPath, id + ".cs");
		}

		private static CodeStyleIssue[] CodeStyleIssues(string code, CheckerOption[] options)
		{
			return new StyleChecker(Speller.Instance, options).Check(code);
		}

		private static string CalculateHash(string code)
		{
			return string.Join("", md5.ComputeHash(Encoding.Unicode.GetBytes(code)).Select(b => b.ToString("x2")));
		}

		private CodeLine BuildCodeLine(string line, int lineIndex, CodeStyleIssue[] issues, Highlight[] highlights)
		{
			return new CodeLine(
				line,
				issues.Where(issue => issue.Span.Line == lineIndex),
				highlights.Where(h => h.Span.Line == lineIndex));
		}
	}

}
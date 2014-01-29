using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHunspell;
using stylist.Checkers;

namespace stylist
{
	public class CheckerOption
	{
		public string Checker { get; set; }
		public JObject Options { get; set; }
	}

	public class StyleChecker
	{
		private readonly BaseChecker[] checkers;

		public BaseChecker[] Checkers { get { return checkers; } }

		public StyleChecker(Speller speller, CheckerOption[] options)
		{
			checkers = 
				CreateBaseCheckers(speller)
				.Select(ch => InitChecker(ch, options))
				.Where(ch => ch != null)
				.ToArray();
		}

		private BaseChecker InitChecker(BaseChecker ch, CheckerOption[] options)
		{
			CheckerOption option = options.FirstOrDefault(opt => opt.Checker + "Checker" == ch.GetType().Name);
			if (option == null) return ch;
			if (option.Options == null) return null;
			string json = JsonConvert.SerializeObject(option.Options);
			JsonConvert.PopulateObject(json, ch);
			return ch;
		}

		public StyleChecker(Speller speller)
			: this(CreateBaseCheckers(speller).ToArray())
		{
		}

		private static IEnumerable<BaseChecker> CreateBaseCheckers(Speller speller)
		{
			yield return new SpellChecker(speller);
			yield return new NamingCaseChecker();
			yield return new NamingLengthChecker();
			yield return new NamingChecker();
			yield return new MethodLengthChecker();
			yield return new FormattingChecker();
		}

		public StyleChecker(params BaseChecker[] checkers)
		{
			this.checkers = checkers;
		}

		public CodeStyleIssue[] Check(string source)
		{
			var ast = new CSharpParser().Parse(source);
			var codeIssues = new List<CodeStyleIssue>();
			foreach (var checker in checkers)
			{
				checker.Initialize(codeIssues);
				ast.AcceptVisitor(checker);
			}
			return codeIssues.ToArray();
		}
	}
}
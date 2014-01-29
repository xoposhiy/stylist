using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using Newtonsoft.Json;
using stylist.Checkers;

namespace stylist
{
	public class StyleChecker
	{
		private readonly IChecker[] checkers;

		public IChecker[] Checkers { get { return checkers; } }

		public StyleChecker(Speller speller, CheckerOption[] options)
		{
			checkers = 
				CreateBaseCheckers(speller)
				.Select(ch => InitChecker(ch, options))
				.Where(ch => ch != null)
				.ToArray();
		}

		private IChecker InitChecker(IChecker ch, CheckerOption[] options)
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

		private static IEnumerable<IChecker> CreateBaseCheckers(Speller speller)
		{
			yield return new SpellChecker(speller);
			yield return new NamingCaseChecker();
			yield return new NamingLengthChecker();
			yield return new NamingChecker();
			yield return new MethodLengthChecker();
			yield return new FormattingChecker();
			yield return new LineLengthChecker();
		}

		public StyleChecker(params IChecker[] checkers)
		{
			this.checkers = checkers;
		}

		public CodeStyleIssue[] Check(string source)
		{
			var ast = new CSharpParser().Parse(source);
			return Check(ast, source);
		}

		public CodeStyleIssue[] Check(SyntaxTree ast, string source)
		{
			
			var codeIssues = new CodeIssues();
			foreach (var checker in checkers.OfType<BaseAstChecker>())
			{
				checker.Initialize(codeIssues);
				ast.AcceptVisitor(checker);
			}
			foreach (var checker in checkers.OfType<BaseTextChecker>())
			{
				checker.Initialize(codeIssues);
				checker.Check(source);
			}
			return codeIssues.Issues.ToArray();
		}
	}
}
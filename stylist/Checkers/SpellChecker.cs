using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;

namespace stylist.Checkers
{
	public class SpellChecker : BaseNamingChecker
	{
		private readonly Speller speller;
		private readonly List<string> unknownWords = new List<string>();

		public SpellChecker(Speller speller)
		{
			this.speller = speller;
			AllowedWords = new string[0];
		}

		public string[] AllowedWords { get; set; }

		public IEnumerable<string> UnknownWords { get { return unknownWords; } }

		public string FindSpellingError(string camelCaseWords)
		{
			var words = SplitCamelCase(camelCaseWords).ToList();
			var wrongWords = words.Where(Incorrect).ToList();
			foreach (var wrongWord in wrongWords)
				unknownWords.Add(wrongWord);
			return wrongWords.Concat(words.Where(w => speller.Antiwords.Contains(w))).FirstOrDefault();
		}

		private bool Incorrect(string w)
		{
			return !speller.Hunspell.Spell(w) && !AllowedWords.Contains(w);
		}

		private IEnumerable<string> SplitCamelCase(string camelCaseWords)
		{
			var chunk = new StringBuilder();
			foreach (char ch in camelCaseWords)
			{
				if (char.IsUpper(ch) || !char.IsLetter(ch))
				{
					if (chunk.Length > 0)
						yield return chunk.ToString();
					chunk.Clear();
				}
				if (char.IsLetter(ch))
					chunk.Append(char.ToLower(ch));
			}
			if (chunk.Length > 0)
				yield return chunk.ToString();
		}

		protected override void CheckName(Identifier identifier, AstNode node)
		{
			string misspelledWord = FindSpellingError(identifier.Name);
			if (misspelledWord == null) return;
			codeIssues.Add(
				new CodeStyleIssue(
					"Naming.Spelling",
					string.Format("Spelling error in word '{0}'?", misspelledWord),
					new TextSpan(identifier)));
		}
	}
}
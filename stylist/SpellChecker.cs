using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using NHunspell;

namespace stylist
{
	public class SpellChecker : BaseNamingChecker
	{
		private readonly Hunspell speller;

		public SpellChecker(Hunspell speller)
		{
			this.speller = speller;
		}

		private string FindSpellingError(string camelCaseWords)
		{
			return SplitCamelCase(camelCaseWords).FirstOrDefault(word => !speller.Spell(word));
		}

		private IEnumerable<string> SplitCamelCase(string camelCaseWords)
		{
			var chunk = new StringBuilder();
			foreach (char ch in camelCaseWords)
			{
				if (char.IsUpper(ch) || !char.IsLetter(ch))
				{
					yield return chunk.ToString();
					chunk.Clear();
				}
				chunk.Append(ch);
			}
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
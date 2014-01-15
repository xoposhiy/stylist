using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace stylist.tests
{
	[TestFixture]
	public class BuildSpellerDictionary_Test
	{
		[TestFixtureSetUp]
		public void SetUpSpeller()
		{
			Speller.Initialize("spell");
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			Speller.DisposeInstance();
		}

		[Test]
		[Explicit]
		public void Test()
		{
			var spellChecker = new SpellChecker(Speller.Instance);
			var checker = new StyleChecker(spellChecker);
			foreach (var file in Directory.EnumerateFiles(@"c:\Users\xoposhiy\Documents\RefSrc\Source\.NET 4.5\4.5.50709.0\net\ndp\clr\src\BCL\System\", "*.cs", SearchOption.AllDirectories))
			{
				checker.Check(File.ReadAllText(file));
			}
			foreach (var w in spellChecker.UnknownWords.GroupBy(w => w.ToLower()).Select(g => new {w=g.Key, c=g.Count()}).OrderByDescending(g => g.c))
			{
				Console.WriteLine(w.c + "\t" + w.w);
			}
		}
	}
}

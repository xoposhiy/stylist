using System.Text.RegularExpressions;

namespace stylist.Checkers
{
	public class LineLengthChecker : BaseTextChecker
	{
		public LineLengthChecker()
		{
			MaxLineLength = 140;
		}

		public override void Check(string source)
		{
			var lines = Regex.Split(source, "\r\n|\r|\n");
			for (int i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Length > MaxLineLength)
					codeIssues.Report("LineLength", "Long lines are hard to read", new TextSpan(i, 0, line.Length-1));
			}
		}

		public int MaxLineLength { get; set; }

	}
}
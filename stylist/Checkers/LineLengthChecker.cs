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
			var lines = source.AsLines();
			for (int i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Length > MaxLineLength)
					codeIssues.Report(this, "Long lines are hard to read", new TextSpan(i, 0, line.Length-1));
			}
		}

		public int MaxLineLength { get; set; }

	}
}
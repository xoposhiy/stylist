using System.Collections.Generic;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public class TextSpan
	{
		public int Line, FirstColumn, LastColumn;

		public TextSpan(AstNode node) : this(node.StartLocation, node.EndLocation)
		{
		}

		public TextSpan(TextLocation start, TextLocation end)
			: this(start.Line-1, start.Column-1, end.Line == start.Line ? end.Column-1 : int.MaxValue)
		{
		}

		public static IEnumerable<TextSpan> Split(TextLocation start, TextLocation end)
		{
			if (end.Line == -1) end = new TextLocation(start.Line, int.MaxValue);
			for (int line = start.Line; line <= end.Line; line++)
			{
				var startCol = line == start.Line ? start.Column : 1;
				var endCol = line == end.Line ? end.Column : int.MaxValue;
				yield return new TextSpan(line - 1, startCol - 1, endCol - 1);
			}
		}

		public TextSpan(int line, int firstColumn, int lastColumn)
		{
			Line = line;
			FirstColumn = firstColumn;
			LastColumn = lastColumn;
		}

		public string ExtractBeforeSpan(string line)
		{
			return line.Substring(0, FirstColumn);
		}

		public string ExtractSpan(string line)
		{
			return line.Substring(FirstColumn, LastColumn - FirstColumn);
		}

		public string ExtractAfterSpan(string line)
		{
			return line.Substring(LastColumn, line.Length - LastColumn);
		}

		public override string ToString()
		{
			return string.Format("{0}:{1}—{2}", Line, FirstColumn, LastColumn);
		}
	}
}
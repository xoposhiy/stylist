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
			: this(start.Line-1, start.Column-1, end.Column-1)
		{
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
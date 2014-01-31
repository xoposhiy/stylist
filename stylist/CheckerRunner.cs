using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace stylist
{
	public static class CheckerExtensions
	{
		public static IEnumerable<CodeStyleIssue> AnalayzeText(this IChecker checker, string text)
		{
			text = text.Replace("    ", "\t");
			text = text.Replace(" \t", "\t");
			text = text.Replace("  \t", "\t");
			text = string.Join("\r\n", text.AsLines());
			var codeIssues = new CodeIssues();
			checker.Initialize(codeIssues);
			var astChecker = (checker as BaseAstChecker);
			if (astChecker != null)
				new CSharpParser().Parse(text).AcceptVisitor(astChecker);
			var textChecker = (checker as BaseTextChecker);
			if (textChecker != null)
				textChecker.Check(text);
			var lines = text.AsLines();
			foreach (var issue in codeIssues.Issues)
			{
				var lineIndex = issue.Span.Line;
				issue.Fragment = lines[lineIndex];
				if (lineIndex > 0) issue.Fragment = lines[lineIndex - 1] + Environment.NewLine + issue.Fragment;
				if (lineIndex < lines.Length-1) issue.Fragment += Environment.NewLine + lines[lineIndex + 1];
			}
			return codeIssues.Issues;
		}

		public static IEnumerable<CodeStyleIssue> AnalayzeFile(this IChecker checker, string filename)
		{
			return AnalayzeText(checker, File.ReadAllText(filename));
		}

		public static IEnumerable<Tuple<string, CodeStyleIssue[]>> AnalayzeFolder(this IChecker checker, string path)
		{
			return Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories)
				.Select(filePath => Tuple.Create(filePath, checker.AnalayzeFile(filePath).ToArray()));
		}

		public static void ReportErrorsToConsole(this IChecker checker, string path, bool showFiles, bool showErrors)
		{
			var fileCount = 0;
			var issuesCount = 0;
			var totalFiles = 0;
			foreach (var reportItem in checker.AnalayzeFolder(path))
			{
				totalFiles++;
				if (reportItem.Item2.Length > 0)
				{
					fileCount++;
					issuesCount += reportItem.Item2.Length;
					Console.WriteLine(fileCount + " " + issuesCount);
					if (showFiles)
					{
						Console.WriteLine("{0} {1}", reportItem.Item1, reportItem.Item2.Length);
						if (showErrors)
						{
							foreach (var issue in reportItem.Item2)
							{
								Console.WriteLine("\t{0}", issue);
								Console.WriteLine(issue.Fragment);
							}
						}
					}
				}
			}
			Console.WriteLine("Total files: {0}", totalFiles);
		}
	}
}

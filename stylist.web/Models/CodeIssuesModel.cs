using System.Collections;
using System.Collections.Generic;

namespace stylist.web.Models
{

	public class CodeModel
	{
		public string Code;
		public string Profile;
	}

	public class CodeIssuesModel : CodeModel
	{
		public CodeLine[] Lines;
		public CodeStyleIssue[] CodeIssues;
	}
}
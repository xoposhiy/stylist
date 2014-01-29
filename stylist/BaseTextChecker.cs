namespace stylist
{
	public abstract class BaseTextChecker : IChecker
	{
		protected CodeIssues codeIssues;

		public void Initialize(CodeIssues result)
		{
			codeIssues = result;
		}

		public abstract void Check(string source);
	}
}
using ICSharpCode.NRefactory;
using NUnit.Framework;

namespace stylist.tests
{
	[TestFixture]
	public class TextSpan_Test
	{
		[Test]
		public void Extract()
		{
			var span = new TextSpan(0, 0, 1);
			Assert.AreEqual("", span.ExtractBeforeSpan("h"));
			Assert.AreEqual("h", span.ExtractSpan("h"));
			Assert.AreEqual("", span.ExtractAfterSpan("h"));
		}

		[Test]
		public void Create()
		{
			var span = new TextSpan(new TextLocation(10, 1), new TextLocation(11, 1));
			Assert.AreEqual(9, span.Line);
			Assert.AreEqual(0, span.FirstColumn);
			Assert.AreEqual(int.MaxValue, span.LastColumn);
		}
	}
}
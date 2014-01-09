using NUnit.Framework;

namespace stylist.tests
{
	[TestFixture]
	public class TextSpan_Test
	{
		[Test]
		public void Test()
		{
			var span = new TextSpan(0, 0, 1);
			Assert.AreEqual("", span.ExtractBeforeSpan("h"));
			Assert.AreEqual("h", span.ExtractSpan("h"));
			Assert.AreEqual("", span.ExtractAfterSpan("h"));
		}
	}
}
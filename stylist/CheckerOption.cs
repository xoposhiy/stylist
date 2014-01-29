using Newtonsoft.Json.Linq;

namespace stylist
{
	public class CheckerOption
	{
		public string Checker { get; set; }
		public JObject Options { get; set; }
	}
}
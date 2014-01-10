using System.IO;
using NHunspell;

namespace stylist
{
	public class Speller
	{
		public static Hunspell Instance;

		public static void Initialize(string path)
		{
			Instance = new Hunspell(Path.Combine(path, "en_GB.aff"), Path.Combine(path, "en_GB.dic"));
		}

		public static void Dispose()
		{
			if (Instance != null)
				Instance.Dispose();
		}
	}
}
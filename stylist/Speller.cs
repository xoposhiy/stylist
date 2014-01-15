using System;
using System.IO;
using NHunspell;

namespace stylist
{
	public class Speller : IDisposable
	{
		public Speller(string path)
		{
			Hunspell = new Hunspell(Path.Combine(path, "en_GB.aff"), Path.Combine(path, "en_GB.dic"));
			Antiwords = File.ReadAllLines(Path.Combine(path, "antiwords.txt"));
		}

		public Hunspell Hunspell { get; private set; }
		public string[] Antiwords { get; private set; }

		public static Speller Instance;

		public static Speller Initialize(string path)
		{
			return Instance ?? (Instance = new Speller(path));
		}

		public static void DisposeInstance()
		{
			if (Instance != null)
				Instance.Dispose();
		}

		public void Dispose()
		{
			Hunspell.Dispose();
		}
	}
}
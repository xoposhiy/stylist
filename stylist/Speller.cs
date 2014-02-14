using System;
using System.IO;
using NHunspell;

namespace stylist
{
	public class Speller : IDisposable
	{
		private static string dictionaryPath;
		private static Speller instance;

		public Speller(string path)
		{
			Hunspell = new Hunspell(Path.Combine(path, "en_GB.aff"), Path.Combine(path, "en_GB.dic"));
			Antiwords = File.ReadAllLines(Path.Combine(path, "antiwords.txt"));
		}

		public Hunspell Hunspell { get; private set; }
		public string[] Antiwords { get; private set; }

		public static Speller Instance { get { return instance ?? (instance = new Speller(dictionaryPath)); } }

		public void Dispose()
		{
			Hunspell.Dispose();
		}

		public static Speller Initialize(string path)
		{
			dictionaryPath = path;
			return Instance;
		}

		public static void DisposeInstance()
		{
			if (Instance != null)
				Instance.Dispose();
		}
	}
}
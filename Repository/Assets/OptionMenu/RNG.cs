using System;
using System.Security.Cryptography;
using UnityEngine;
using Logger = Utilities.Logger;
using Random = System.Random;

namespace OptionMenu
{
	public static class RNG
	{
		private const string SeedPref = "Seed";
		private static readonly RNGCryptoServiceProvider Provider = new RNGCryptoServiceProvider();
		private static Random m_random;

		[RuntimeInitializeOnLoadMethod]
		public static void SetApplicationRuntimeSeed()
		{
			var newSeed = GetNewSeed();
			SetSeed(newSeed);
		}

		public static int Next(int min, int max)
		{
			return m_random.Next(min, max);
		}
		
		public static int Next(int val)
		{
			return m_random.Next(val);
		}

		public static int GetSeed() => PlayerPrefs.GetInt(SeedPref);

		public static int GetNewSeed() => SecureSeed();

		private static int SecureSeed()
		{
			var bytes = new byte[4];
			Provider.GetBytes(bytes);
			return BitConverter.ToInt32(bytes, 0);
		}

		public static void SetSeed(int newSeed)
		{
			PlayerPrefs.SetInt(SeedPref, newSeed);
			m_random = new Random(newSeed);
			Logger.Log("Seed", "Set new random runtime Seed : " + newSeed);
		}
	}
}
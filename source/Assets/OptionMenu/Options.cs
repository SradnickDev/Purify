using System.IO;
using Battle.General;
using TMPro;
using UnityEngine;
using Utilities;

#pragma warning disable 0649
namespace OptionMenu
{
	public class Options : MonoBehaviour
	{
		[SerializeField] private TMP_InputField m_inputField;

		private void Start()
		{
			SetSeedText();
		}

		public void SetSeedText()
		{
			if (m_inputField)
			{
				m_inputField.text = RNG.GetSeed().ToString();
			}
		}

		public void SetSeed()
		{
			if (m_inputField)
			{
				var enteredSeed = m_inputField.text;
				if (int.TryParse(enteredSeed, out var newSeed))
				{
					RNG.SetSeed(newSeed);
				}
			}
		}

		public void ResetSeed()
		{
			var newSeed = RNG.GetNewSeed();
			RNG.SetSeed(newSeed);
		}

		/// <summary>
		/// Delete all data at the PersistentPath, configs, savegame, deck.
		/// </summary>
		public static void ResetConfigData()
		{
			if (!Directory.Exists(Constants.PersistentPath)) return;
			var dir = new DirectoryInfo(Constants.PersistentPath);

			foreach (var file in dir.EnumerateFiles())
			{
				file.Delete();
			}
		}

		public static BattleConfig LoadConfigData()
			=> PersistentData.Load<BattleConfig>(Constants.BattleConfig) ?? new BattleConfig();

		public static void SaveConfigData(BattleConfig config)
			=> PersistentData.Save(config, Constants.BattleConfig);
	}
}
#pragma warning restore 0649
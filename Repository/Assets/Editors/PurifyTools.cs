using System.Collections.Generic;
using System.IO;
using Battle.General;
using Deck;
using UnityEditor;
using UnityEngine;
using Utilities;

#if UNITY_EDITOR
namespace Editors
{
	public class PurifyTools : Editor
	{
		[MenuItem("PurifyTools/Reset Save Data")]
		private static void ExecuteDeleteData()
		{
			DeleteData();
		}

		private static void DeleteData()
		{
			if (!Directory.Exists(Constants.PersistentPath)) return;

			var dir = new DirectoryInfo(Constants.PersistentPath);

			foreach (var file in dir.EnumerateFiles())
			{
				file.Delete();
			}

			Debug.LogWarning("Save data deleted!");
		}

		[MenuItem("PurifyTools/Create Starter Deck Json File")]
		private static void ExecuteStartDeckFile()
		{
			CreateStartDeckFile();
		}

		private static void CreateStartDeckFile()
		{
			var data = new DeckSaveData
			{
				Cards = new List<CardSaveData>()
				{
					new CardSaveData()
					{
						CardId = 11, Count = 1
					},
					new CardSaveData()
					{
						CardId = 12, Count = 1
					}
				}
			};

			var path = Application.dataPath + "/Resources/";
			PersistentJson.Save<DeckSaveData>(data, path, Constants.StarterDeckData);
			AssetDatabase.Refresh();
		}
	}
}
#endif
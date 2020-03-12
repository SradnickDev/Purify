using System.Collections.Generic;
using Cards.General;
using Utilities;

namespace Units.Enemy.General
{
	[System.Serializable]
	public class EncounterData
	{
		public string Name;
		public List<Enemy> Enemies;
	}
}
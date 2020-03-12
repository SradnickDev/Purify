using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.General
{
	//Data Container
	[CreateAssetMenu]
	public class EncounterCollection : ScriptableObject
	{
		public List<EncounterData> EncounterData;
	}
}
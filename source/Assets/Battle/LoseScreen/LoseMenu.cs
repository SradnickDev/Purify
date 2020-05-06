using OptionMenu;
using UnityEngine;

namespace Battle.LoseScreen
{
	public class LoseMenu : MonoBehaviour
	{
		public void Restart() => Options.ResetConfigData();
	}
}
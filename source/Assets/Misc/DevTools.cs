using Cards.General;
using LoadingMenu;
using OptionMenu;
using Units.Player.General;
using UnityEngine;
using Utilities;
using Zenject;

#pragma warning disable 0649

namespace Misc
{
	//cheats
	public class DevTools : MonoBehaviour
	{
		[Inject]
		private Player m_player;

		private void Update()
		{
			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha1))
			{
				m_player.Might.Current++;
			}

			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha2))
			{
				m_player.Perseverance.Current++;
			}

			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha3))
			{
				m_player.Defense.Current++;
			}

			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha4))
			{
				m_player.Health.Current += 10;
			}

			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha5))
			{
				m_player.Soul.Current += 10;
			}

			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha6))
			{
				m_player.Energy.Current++;
			}

			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha7))
			{
				var hiddenPool = DeckUtility.LoadHiddenPool();
				var data = hiddenPool.GetSingle(x => x.Id == 91);
				var hand = m_player.Hand;

				var newData = GeneralExtensions.DeepCopy(data);
				newData.Illustration = data.Illustration;
				newData.Icon = data.Icon;
				hand.Add(new CardInstance(newData, m_player));
			}

			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha8))
			{
				var hiddenPool = DeckUtility.LoadHiddenPool();
				var data = hiddenPool.GetSingle(x => x.Id == 92);
				var hand = m_player.Hand;

				var newData = GeneralExtensions.DeepCopy(data);
				newData.Illustration = data.Illustration;
				newData.Icon = data.Icon;
				hand.Add(new CardInstance(newData, m_player));
			}
			if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.Alpha9))
			{
				var config = Options.LoadConfigData();
				config.BattleCount++;
				config.Health = new Range(m_player.Health.Current, m_player.Health.Max);
				config.Soul = m_player.Soul.Current;
				Options.SaveConfigData(config);
				LoadingScreen.LoadSceneWithMap(3);
			}
		}
	}
}
#pragma warning restore 0649
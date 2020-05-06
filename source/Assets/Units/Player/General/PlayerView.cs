using Animateables;
using Battle.General;
using Misc.Events;
using Status;
using Status.General;
using TMPro;
using Units.General;
using UnityEngine;
using Zenject;

#pragma warning disable 0649
namespace Units.Player.General
{
	public class PlayerView : MonoBehaviour
	{
		[Header("Soul")]
		[SerializeField] private RadialBar m_soulBar;
		[SerializeField] private TextMeshProUGUI m_currentSoulValue;
		[SerializeField] private AnimateableScale m_animateableSoulbar;

		[Header("Health")]
		[SerializeField] private AnimateableScale m_animateableHealth;

		[SerializeField] private Bar m_healthBar;
		[SerializeField] private Color m_default = Color.red;
		[SerializeField] private Color m_withBock = Color.blue;

		[Header("Energy")]
		[SerializeField] private AnimateableScale m_animateableEnergy;

		[SerializeField] private TextMeshProUGUI m_energyDisplay;

		[Header("Block")]
		[SerializeField] private AnimateableScale m_animateableBlock;

		[SerializeField] private TextMeshProUGUI m_blockDisplay;

		[Inject]
		private Player m_player;

		private int m_previousHealth;
		private int m_previousSoul;
		private int m_previousBlock = -1;
		private int m_previousEnergy;
		private TriggeredAction m_attacked;

		private void Start()
		{
			m_currentSoulValue.text = m_player.Soul.Current.ToString();

			m_attacked = new TriggeredAction()
			{
				OnTriggered = OnPlayerAttacked,
				Requirement = new Attacked(m_player.name)
			};
			EventLog.Register(m_attacked);
		}

		private void OnPlayerAttacked()
		{
			ScreenShake.Instance.DoShake(1);
		}

		private void Update()
		{
			if (m_player == null) return;

			UpdateHealthBar();

			UpdateSoulBar();

			UpdateBlockDisplay();

			UpdateEnergyDisplay();
		}

		private void UpdateHealthBar()
		{
			if (m_healthBar)
			{
				if (m_player.Health.Current != m_previousHealth)
				{
					m_healthBar.SetValues(m_player.Health.Current, m_player.Health.Max);
					m_previousHealth = m_player.Health.Current;
					m_animateableHealth.Play();
				}
			}
		}

		private void UpdateSoulBar()
		{
			if (m_soulBar)
			{
				if (m_player.Soul.Current != m_previousSoul)
				{
					if (m_player.Soul.Current >= 0)
					{
						m_soulBar.TopFillAmount = m_player.Soul.Current / (float) m_player.Soul.Max;
					}
					else
					{
						m_soulBar.BottomFillAmount =
							m_player.Soul.Current / (float) m_player.Soul.Min;
					}

					m_currentSoulValue.text = m_player.Soul.Current.ToString();
					m_previousSoul = m_player.Soul.Current;
					m_animateableSoulbar.Play();
				}
			}
		}

		private void UpdateBlockDisplay()
		{
			if (m_blockDisplay)
			{
				if (m_player.Defense.Current != m_previousBlock)
				{
					m_blockDisplay.text = $"{m_player.Defense.Current}";
					m_previousBlock = m_player.Defense.Current;
					m_animateableBlock.gameObject.SetActive(m_player.Defense.Current > 0);
					if (m_animateableBlock.gameObject.activeInHierarchy)
					{
						m_animateableBlock.Play();
					}

					m_healthBar.SetColor(m_player.Defense.Current > 0 ? m_withBock : m_default);
				}
			}
		}

		private void UpdateEnergyDisplay()
		{
			if (m_energyDisplay)
			{
				if (m_player.Energy.Current != m_previousEnergy)
				{
					m_energyDisplay.text = $"{m_player.Energy.Current}";
					m_previousEnergy = m_player.Energy.Current;
					m_animateableEnergy.Play();
				}
			}
		}

		private void OnDisable()
		{
			EventLog.Deregister(m_attacked);
		}
	}
}
#pragma warning restore 0649
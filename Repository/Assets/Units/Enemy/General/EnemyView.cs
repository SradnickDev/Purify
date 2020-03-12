using Animateables;
using Battle.General;
using Misc.Events;
using SCT;
using Status;
using Status.General;
using TMPro;
using Units.General;
using UnityEngine;

#pragma warning disable 0649
namespace Units.Enemy.General
{
	[RequireComponent(typeof(Enemy))]
	public class EnemyView : MonoBehaviour
	{
		[Header("Health")]
		[SerializeField] private Bar m_healthBar = null;

		[SerializeField] private AnimateableScale m_animateableHealth;
		[SerializeField] private Color m_default = Color.red;
		[SerializeField] private Color m_withBock = Color.blue;

		[Header("Soul")]
		[SerializeField] private RadialBar m_soulBar = null;

		[SerializeField] private AnimateableScale m_animateableSoulbar;
		[SerializeField] private TextMeshProUGUI m_currentSoulValue;

		[Header("Block")]
		[SerializeField] private TextMeshProUGUI m_blockDisplay = null;

		[SerializeField] private AnimateableScale m_animateableBlock;

		[Header("Animation")]
		[SerializeField] private string m_hitAnimation = "Hitted";

		[SerializeField] private Animator m_animator = null;

		[SerializeField]
		private ObjectShake m_objectShake;

		private Enemy m_enemy;

		private int m_previousHealth;
		private int m_previousSoul;
		private int m_previousBlock = -1;

		private TriggeredAction m_blockedEvent;
		private TriggeredAction m_attacked;

		private void Start()
		{
			Setup();
		}

		private void Setup()
		{
			m_enemy = GetComponent<Enemy>();
			m_currentSoulValue.text = m_enemy.Soul.Current.ToString();

			m_objectShake.SetObject(this.transform);

			m_attacked = new TriggeredAction()
			{
				OnTriggered = OnAttacked,
				Requirement = new Attacked(m_enemy.name)
			};

			m_blockedEvent = new TriggeredAction()
			{
				OnTriggered = BlockedDamage,
				Requirement = new BlockedDamage(m_enemy.name)
			};

			EventLog.Register(m_blockedEvent, m_attacked);
		}

		private void BlockedDamage()
		{
			m_objectShake.DoShake(1);
		}

		private void OnAttacked()
		{
			TriggerAnimation(m_hitAnimation);
			var lastHit = m_enemy.LastHit;
			ScriptableTextDisplay.Instance.InitializeScriptableText(0, transform.position,
																	Mathf.Abs(lastHit.Item2)
																		 .ToString());
			m_objectShake.DoShake(0);
		}

		private void Update()
		{
			if (m_enemy == null) return;

			UpdateHealthBar();
			UpdateSoulBar();
			UpdateBlockDisplay();
		}

		public void TriggerAnimation(string type)
		{
			if (m_animator)
			{
				m_animator.SetTrigger(type);
			}
		}

		private void UpdateHealthBar()
		{
			if (m_healthBar)
			{
				if (m_enemy.Health.Current != m_previousHealth)
				{
					m_healthBar.SetValues(m_enemy.Health.Current, m_enemy.Health.Max);
					m_previousHealth = m_enemy.Health.Current;
					m_animateableHealth.Play();
				}
			}
		}

		private void UpdateSoulBar()
		{
			if (m_soulBar)
			{
				if (m_enemy.Soul.Current != m_previousSoul)
				{
					if (m_enemy.Soul.Current >= 0)
					{
						m_soulBar.TopFillAmount = m_enemy.Soul.Current / (float) m_enemy.Soul.Max;
					}
					else
					{
						m_soulBar.BottomFillAmount =
							m_enemy.Soul.Current / (float) m_enemy.Soul.Min;
					}

					m_previousSoul = m_enemy.Soul.Current;
					m_animateableSoulbar.Play();
				}

				m_currentSoulValue.text = m_enemy.Soul.Current.ToString();
			}
		}

		private void UpdateBlockDisplay()
		{
			if (m_blockDisplay)
			{
				if (m_enemy.Defense.Current != m_previousBlock)
				{
					m_blockDisplay.text = $"{m_enemy.Defense.Current}";
					m_previousBlock = m_enemy.Defense.Current;
					m_animateableBlock.gameObject.SetActive(m_enemy.Defense.Current > 0);
					if (m_animateableBlock.gameObject.activeInHierarchy)
					{
						m_animateableBlock.Play();
					}

					m_healthBar.SetColor(m_enemy.Defense.Current > 0 ? m_withBock : m_default);
				}
			}
		}

		private void OnDestroy()
		{
			EventLog.Deregister(m_blockedEvent, m_attacked);
		}
	}
}
#pragma warning restore 0649
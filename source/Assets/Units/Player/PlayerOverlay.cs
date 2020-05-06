using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Units.Player
{
	public class PlayerOverlay : MonoBehaviour
	{
		[SerializeField] private Range m_minMaxAlpha = new Range(0, 1);
		[SerializeField] private Image m_overlay;
		[SerializeField] private Color m_damage = Color.red;
		[SerializeField] private Color m_corruption = new Color(0.64f, 0.13f, 1f);
		[SerializeField] private Color m_defense = Color.cyan;
		[SerializeField] private float m_duration = 1;
		[SerializeField] private float m_fullDefenseAlpha = 0.3f;
		[SerializeField] private float m_brokenDefenseAlpha = 1;
		[SerializeField] private Ease m_ease;

		[Inject]
		private General.Player m_player;

		private int m_previousHealth;
		private int m_previousSoul;
		private int m_previousDefense;

		private void Start()
		{
			m_player.Health.CurrentChanged += OnHealthChanged;
			m_previousHealth = m_player.Health.Current;
			m_player.Soul.CurrentChanged += OnSoulChanged;
			m_previousSoul = m_player.Soul.Current;
			m_player.Defense.CurrentChanged += OnDefenseChanged;
			m_previousDefense = m_player.Defense.Current;
		}

		private void OnHealthChanged()
		{
			if (m_player.Health.Current < m_previousHealth)
			{
				var value = Mathf.Abs(1 - (m_player.Health.Current / (float) m_player.Health.Max));
				value = Mathf.Clamp(value, m_minMaxAlpha.Min, m_minMaxAlpha.Max);
				var color = m_damage;
				color.a = value;
				m_overlay.color = color;
				m_overlay
					.DOColor(new Color(color.r, color.g, color.g, 0), m_duration)
					.SetEase(m_ease);
			}

			m_previousHealth = m_player.Health.Current;
		}

		private void OnSoulChanged()
		{
			if (m_player.Soul.Current < 0 &&
				m_player.Soul.Current < m_previousSoul)
			{
				var value = m_player.Soul.Current / (float) m_player.Soul.Min;
				value = Mathf.Clamp(value, m_minMaxAlpha.Min, m_minMaxAlpha.Max);
				var color = m_corruption;
				color.a = value;
				m_overlay.color = color;
				m_overlay
					.DOColor(new Color(color.r, color.g, color.g, 0), m_duration)
					.SetEase(m_ease);
			}

			m_previousSoul = m_player.Soul.Current;
		}

		private void OnDefenseChanged()
		{
			if (m_player.Defense.Current < m_previousDefense)
			{
				var color = m_defense;
				color.a = m_brokenDefenseAlpha;
				m_overlay.color = color;
				m_overlay
					.DOColor(new Color(color.r, color.g, color.g, 0), m_duration)
					.SetEase(m_ease);
			}

			if (m_player.Defense.Current <= 0 && m_previousDefense != 0)
			{
				var color = m_defense;
				color.a = m_fullDefenseAlpha;
				m_overlay.color = color;
				m_overlay
					.DOColor(new Color(color.r, color.g, color.g, 0), m_duration)
					.SetEase(m_ease);
			}

			m_previousDefense = m_player.Defense.Current;
		}
	}
}
#pragma warning restore 0649
using System;
using System.Collections;
using Battle.General;
using Battle.Zones;
using Cards.Effects.General;
using Cards.General;
using SCT;
using Units.General;
using UnityEngine;
using UnityEngine.Events;
using VFX;

#pragma warning disable 0649
namespace Units.Enemy.General
{
	[Serializable]
	public class AnimationTrigger : UnityEvent<string> { }

	[RequireComponent(typeof(AudioSource))]
	public class Enemy : Unit
	{
		[SerializeField] private AudioSource m_audioSource;
		public AnimationTrigger AnimationTrigger;
		public AttackPattern Pattern;
		public ZonePosition ZonePosition;

		public Attack NextAttack
		{
			get
			{
				if (m_nextAttack == null)
				{
					m_nextAttack = Pattern.Attacks[m_nextAttackIndex];
				}

				return m_nextAttack;
			}
			set => m_nextAttack = value;
		}

		private Attack m_nextAttack;

		public bool IsAlive => Health.Current > 0 || Soul.Current < 0;

		private int m_nextAttackIndex = 0;

		public override void Start()
		{
			base.Start();
			Setup();
		}

		private void Setup()
		{
			Health.CurrentChanged += OnHealthUpdate;
			Soul.CurrentChanged += OnSoulUpdate;
		}

		private void OnSoulUpdate()
		{
			if (Soul.Current >= 0)
			{
				OnDeath();
			}
		}

		private void OnHealthUpdate()
		{
			if (Health.Current <= 0)
			{
				OnDeath();
			}
		}

		public IEnumerator Attack(WaitForSeconds waitForSeconds)
		{
			InvokeIntentionFeedback();

			yield return waitForSeconds;

			InvokeAnimation();
			InvokeSound();

			foreach (var effect in NextAttack.Effect)
			{
				effect.Use(null, this, TargetType.Single);
			}

			CardVFXHandler.Instance.Play(NextAttack.VFXIndex, this);
			SetNextAttack();
		}

		private void InvokeIntentionFeedback()
		{
			ScriptableTextDisplay.Instance.InitializeScriptableText(1, transform.position,
																	NextAttack.Name,
																	NextAttack.Icon);
		}

		private void InvokeSound()
		{
			if (NextAttack.AttackSound)
			{
				m_audioSource.PlayOneShot(NextAttack.AttackSound);
			}
		}

		private void InvokeAnimation()
		{
			AnimationTrigger?.Invoke(NextAttack.AnimationName);
		}

		private void SetNextAttack()
		{
			m_nextAttackIndex = (m_nextAttackIndex + 1) % Pattern.Attacks.Count;
			m_nextAttack = Pattern.Attacks[m_nextAttackIndex];
		}

		private void OnDeath()
		{
			Health.CurrentChanged -= OnHealthUpdate;
			Soul.CurrentChanged -= OnSoulUpdate;
			Destroy(gameObject);
		}
	}
}
#pragma warning restore 0649
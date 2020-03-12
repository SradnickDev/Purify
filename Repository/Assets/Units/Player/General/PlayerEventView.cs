using Misc.Events;
using Status;
using Status.General;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

#pragma warning disable 0649
namespace Units.Player.General
{
	[RequireComponent(typeof(AudioSource))]
	public class PlayerEventView : MonoBehaviour
	{
		public UnityEvent Attacked;
		public UnityEvent BlockedDamage;
		public UnityEvent Purified;
		public UnityEvent Corrupted;
		public UnityEvent Buffed;
		public UnityEvent Debuffed;

		[Inject] private Player m_player;
		private TriggeredAction m_attacked;
		private TriggeredAction m_blocked;
		private TriggeredAction m_purified;
		private TriggeredAction m_corrupted;
		private TriggeredAction m_buffed;
		private TriggeredAction m_debuffed;

		private void OnEnable()
		{
			SetupEvents();
		}

		private void SetupEvents()
		{
			m_attacked = new TriggeredAction(new Attacked(m_player.name), OnAttacked);
			m_blocked = new TriggeredAction(new BlockedDamage(m_player.name), OnBlockedDamage);
			m_purified = new TriggeredAction(new Purified(m_player.name), OnPurified);
			m_corrupted = new TriggeredAction(new Corrupted(m_player.name), OnCorrupted);
			m_buffed = new TriggeredAction(new Buffed(m_player.name), OnGainBuff);
			m_debuffed = new TriggeredAction(new Debuffed(m_player.name), OnGainDebuff);

			EventLog.Register(m_attacked, m_blocked, m_purified, m_corrupted, m_buffed, m_debuffed);
		}

		private void OnAttacked() => Attacked?.Invoke();

		private void OnBlockedDamage() => BlockedDamage?.Invoke();

		private void OnCorrupted() => Corrupted?.Invoke();

		private void OnPurified() => Purified?.Invoke();

		private void OnGainDebuff() => Debuffed?.Invoke();

		private void OnGainBuff() => Buffed?.Invoke();

		private void OnDisable()
		{
			EventLog.Deregister(m_attacked, m_blocked, m_purified, m_corrupted, m_buffed, m_debuffed);
		}
	}
}
#pragma warning restore 0649
using Units.Enemy.General;
using UnityEngine;
using Zenject;

namespace Cards.General
{
	public class CardController : MonoBehaviour
	{
		[Inject]
		private Hand m_hand = null;

		public bool CanPlayCard(CardInstance instance)
			=> m_hand.CanPlayCard(instance);

		public void Play(CardInstance instance, Enemy target)
		{
			m_hand.Play(instance, target);
		}
	}
}
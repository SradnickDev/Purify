using Battle.Zones;
using Cards.General;
using DG.Tweening;
using UnityEngine;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Cards
{
	public class BlessingZone : MonoBehaviour
	{
		[Header("Animation")]
		[SerializeField] private Ease m_ease = Ease.Linear;

		[SerializeField] private float m_durtaion = 0.25f;

		[Inject]
		private HandZone m_handZone;

		[Inject]
		private void Setup(BlessingPile blessingPile)
		{
			blessingPile.Added += Add;
		}

		private void Add(CardInstance card)
		{
			var targetCard = m_handZone.GetCard(card);

			targetCard.transform.DOMove(GeneralUtilities.ScreenCenter, m_durtaion)
					  .SetEase(m_ease);

			targetCard.transform.DOScale(0, m_durtaion)
					  .SetEase(m_ease)
					  .OnComplete(() => Destroy(targetCard.gameObject));
		}
	}
}
#pragma warning restore 0649
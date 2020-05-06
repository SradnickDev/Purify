using Animateables;
using Cards.General;
using Deck;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#pragma warning disable 0649
namespace Battle.Zones
{
	/// <summary>
	/// Visual part(View) of the Banish Pile.
	/// Handles animation, card overview, animation.
	/// </summary>
	[RequireComponent(typeof(AnimateableScale))]
	[RequireComponent(typeof(Button))]
	public class BanishPileZone : MonoBehaviour
	{
		[SerializeField] private CardCollectionViewOpener CardCollectionViewOpener;
		[SerializeField] private TextMeshProUGUI m_counter;
		[Header("Animation")]
		[SerializeField] private AnimateableScale m_animateableScale;

		[SerializeField] private float m_dissolveDuration = 1;
		[SerializeField] private Ease m_ease = Ease.Linear;
		[SerializeField] private float m_durtaion = 0.25f;

		[Inject]
		private HandZone m_handZone;

		private BanishPile m_banishPile;
		private Button m_button;

		[Inject]
		private void Setup(BanishPile banishPile)
		{
			banishPile.Added += Add;
			banishPile.Removed += Remove;
			banishPile.Changed += UpdateZone;
			m_banishPile = banishPile;

			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(OpenCardOverview);
		}

		private void Add(CardInstance card)
		{
			var targetCard = m_handZone.GetCard(card);

			targetCard.Dissolve(1);

			targetCard.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 0f), m_durtaion)
				.SetEase(m_ease)
				.OnPlay(() => targetCard.Dissolve(m_dissolveDuration))
				.OnComplete(() => Destroy(targetCard.gameObject));

			UpdateText();
		}

		private void Remove(CardInstance card) => UpdateText();

		private void UpdateZone(CardPile drawPile) => UpdateText();

		private void UpdateText()
		{
			m_counter.text = m_banishPile.Cards.Count.ToString();
			m_animateableScale.Play();
		}

		public void OpenCardOverview()
		{
			CardCollectionViewOpener.SetHeader("Banish Pile");
			CardCollectionViewOpener.Open
			(
				m_banishPile.Cards,
				null,
				true
			);
		}
	}
}
#pragma warning restore 0649
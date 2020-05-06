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
	/// Visual part(View) of the Discard Pile.
	/// Handles animation, card overview, animation.
	/// </summary>
	[RequireComponent(typeof(AnimateableScale))]
	[RequireComponent(typeof(Button))]
	public class DiscardPileZone : MonoBehaviour
	{
		[SerializeField] private CardCollectionViewOpener CardCollectionViewOpener;
		[SerializeField] private TextMeshProUGUI m_counter;

		[Header("Animation")]
		[SerializeField] private AnimateableScale m_animateableScale;

		[SerializeField] private Ease m_ease = Ease.Linear;
		[SerializeField] private float m_durtaion = 0.25f;
		[SerializeField] private float m_finalSize = 0;

		[Inject]
		private HandZone m_handZone;

		private DiscardPile m_discardPile;
		private Button m_button;

		[Inject]
		private void Setup(DiscardPile discardPile)
		{
			discardPile.Added += Add;
			discardPile.Removed += Remove;
			discardPile.Changed += UpdateZone;

			m_discardPile = discardPile;

			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(Display);
		}

		private void Add(CardInstance card)
		{
			var targetCard = m_handZone.GetCard(card);

			var dir = (transform.position - targetCard.transform.position).normalized;
			var rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			targetCard.transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, rotZ - 90), m_durtaion)
				.SetEase(m_ease);

			targetCard.transform.DOMove(transform.position, m_durtaion)
				.SetEase(m_ease);

			targetCard.transform.DOScale(m_finalSize, m_durtaion)
				.SetEase(m_ease)
				.OnComplete(() => Destroy(targetCard.gameObject));


			UpdateText();
		}

		private void UpdateZone(CardPile discardPile) => UpdateText();

		private void Remove(CardInstance obj) => UpdateText();

		private void UpdateText()
		{
			m_counter.text = m_discardPile.Cards.Count.ToString();
			m_animateableScale.Play();
		}

		public void Display()
		{
			CardCollectionViewOpener.SetHeader("Discard Pile");
			CardCollectionViewOpener.Open
			(
				m_discardPile.Cards,
				null,
				true
			);
		}
	}
}
#pragma warning restore 0649
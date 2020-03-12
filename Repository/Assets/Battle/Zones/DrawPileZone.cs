using Animateables;
using Cards.General;
using Deck;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

#pragma warning disable 0649
namespace Battle.Zones
{
	/// <summary>
	/// Visual part(View) of the Draw Pile.
	/// Handles animation, card overview, animation.
	/// </summary>
	[RequireComponent(typeof(AnimateableScale))]
	[RequireComponent(typeof(Button))]
	public class DrawPileZone : MonoBehaviour
	{
		[SerializeField] private CardCollectionViewOpener CardCollectionViewOpener;
		[SerializeField] private TextMeshProUGUI m_counter;
		[SerializeField] private AnimateableScale m_animateableScale;

		private DrawPile m_drawPile;
		private Button m_button;

		[Inject]
		private void Setup(DrawPile drawPile)
		{
			drawPile.Added += Add;
			drawPile.Removed += Remove;
			drawPile.Changed += UpdateZone;
			m_drawPile = drawPile;

			m_button = GetComponent<Button>();
			m_button.onClick.AddListener(Display);
		}

		private void Remove(CardInstance card) => UpdateText();

		private void UpdateZone(CardPile drawPile) => UpdateText();

		private void Add(CardInstance card) => UpdateText();

		private void UpdateText()
		{
			m_counter.text = m_drawPile.Cards.Count.ToString();
			m_animateableScale.Play();
		}

		public void Display()
		{
			CardCollectionViewOpener.SetHeader("Draw Pile");
			CardCollectionViewOpener.Open
			(
				m_drawPile.Cards,
				null,
				true
			);
		}
	}
}
#pragma warning restore 0649
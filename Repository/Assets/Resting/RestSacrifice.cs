using Deck;
using Units.Player.General;
using UnityEngine;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Resting
{
	public class RestSacrifice : RestMechanic
	{
		[SerializeField] private CardCollectionViewOpener m_cardViewOpener;

		/// <summary>
		/// Load and Visualize the current deck.
		/// A selected card will be deleted.
		/// </summary>
		public override void ApplyMechanic()
		{
			var deckData = DeckUtility.LoadSavedDeckData();
			var deck = DeckFactory.Build(deckData);

			m_cardViewOpener.Open(deck, view =>
			{
				deck.Remove(view.Instance);
				Destroy(view.gameObject);

				DeckUtility.SaveDeck(deck);

				RestMenu.UseRestPoints(Costs);
				if (!RestMenu.HasRestPoints(Costs))
				{
					m_cardViewOpener.Close();
				}
			}, true);
		}
	}
}
#pragma warning restore 0649
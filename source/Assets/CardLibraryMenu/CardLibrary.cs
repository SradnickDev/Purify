using Deck;
using UnityEngine;
using Utilities;

#pragma warning disable 0649
namespace CardLibraryMenu
{
	public class CardLibrary : MonoBehaviour
	{
		[SerializeField] private CardCollectionViewOpener m_viewOpener;

		void Start()
		{
			LoadLibrary();
		}

		private void LoadLibrary()
		{
			var pools = DeckUtility.LoadAllPools();
			var library = new CardPool();

			foreach (var pool in pools)
			{
				foreach (var card in pool.Cards)
				{
					library.Add(card);
				}
			}

			OpenCardOverview(library);
		}

		private void OpenCardOverview(CardPool library)
		{
			m_viewOpener.Open(library, null, false);
		}
	}
}
#pragma warning restore 0649
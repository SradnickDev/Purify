using System.Collections.Generic;
using Animateables;
using Battle.RewardMenu;
using Cards.General;
using Deck;
using DG.Tweening;
using LoadingMenu;
using OptionMenu;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

#pragma warning disable 0649

namespace StartSelectionmenu
{
	public class StarterCardSelection : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private RectTransform m_starterDeckPanel;

		[SerializeField] private CardCollectionViewOpener m_viewOpener;
		[SerializeField] private CardModel m_cardModel;
		[SerializeField] private List<Transform> m_emptySlots;
		[SerializeField] private AnimateableScale m_startButon;

		[Space]
		[SerializeField] private int m_maxSelections = 2;

		private int m_currentSelections = 0;
		private CardDeck m_selectedCards = new CardDeck();

		[Header("Animation")]
		[SerializeField] private float m_duration = 1;

		[SerializeField] private Ease m_ease = Ease.Linear;

		private void Start()
		{
			Options.ResetConfigData();

			SetStarterPool();
			SetSelectionPool();
		}

		/// <summary>
		/// Load default Starter Pool. Create Visual cards.
		/// </summary>
		private void SetStarterPool()
		{
			var starterPool = DeckFactory.Build(null);

			var ids = new List<int>();
			foreach (var card in starterPool.Cards)
			{
				if (ids.Contains(card.CardData.Id)) continue;

				var cardModel = CreateCardModel(card);
				Destroy(cardModel.GetComponent<HoverableCard>());
				cardModel.gameObject.AddComponent<SelectableCard>();
				ids.Add(card.CardData.Id);
				SetCardToContainer(cardModel);
			}
		}

		private CardModel CreateCardModel(CardInstance instance)
		{
			var retVal = Instantiate(m_cardModel);
			retVal.Setup(instance);
			return retVal;
		}

		/// <summary>
		/// Load Selection pool, calls CardCollectionViewOpener to visualize them.
		/// </summary>
		private void SetSelectionPool()
		{
			var selectionPool = DeckUtility.LoadSelectionStarterPool();
			m_viewOpener.Open(selectionPool, OnSelectionCardSelected, false);
		}

		/// <summary>
		/// Called if a Card is selected.
		/// Using DoTween to create smooth Animation
		/// </summary>
		/// <param name="cardModel"></param>
		private void OnSelectionCardSelected(CardModel cardModel)
		{
			if (m_currentSelections >= m_maxSelections)
			{
				return;
			}

			m_currentSelections++;

			if (m_currentSelections == m_maxSelections)
			{
				m_startButon.GetComponent<Button>().Select();
				m_startButon.Play();
			}

			m_selectedCards.Cards.Add(cardModel.Instance);
			cardModel.transform.SetParent(this.transform);
			cardModel.transform.SetAsLastSibling();

			cardModel.transform
					 .DOMove(m_emptySlots[m_currentSelections + 1].position,
							 m_duration)
					 .SetEase(m_ease)
					 .OnComplete(() =>
					 {
						 cardModel.transform.SetParent(m_starterDeckPanel, false);
						 cardModel.GetComponent<SelectableCard>().RemoveListener();
						 Destroy(cardModel.GetComponent<SelectableCard>());
						 cardModel.gameObject.AddComponent<SelectableCard>();
						 cardModel.transform.localScale = new Vector3(1, 1, 1);
					 });
			Clear();
		}

		private void Clear()
		{
			if (m_currentSelections >= m_maxSelections)
			{
				m_viewOpener.RemoveAllListeners();
			}
		}

		private void SetCardToContainer(Component cardModel)
		{
			cardModel.transform.SetParent(m_starterDeckPanel);
			cardModel.transform.localScale = new Vector3(1, 1, 1);
		}

		/// <summary>
		/// Called from Button in scene.
		/// </summary>
		public void StartMission()
		{
			CreateStarterDeck();
			LoadBattleScene();
		}

		/// <summary>
		/// Crate a starte deck from the default plus the selected cards.
		/// Save them to persistent data path.
		/// </summary>
		private void CreateStarterDeck()
		{
			//create a default deck
			var starterDeck = DeckFactory.Build(null);
			foreach (var selectedCard in m_selectedCards.Cards)
			{
				starterDeck.Cards.Add(selectedCard);
			}

			DeckUtility.SaveDeck(starterDeck);
		}

		private static void LoadBattleScene()
		{
			LoadingScreen.LoadSceneWithMap(3);
		}
	}
#pragma warning restore 0649
}
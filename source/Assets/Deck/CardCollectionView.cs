using System;
using System.Collections.Generic;
using Battle.RewardMenu;
using Cards.General;
using Misc.PopUp;
using UnityEngine;
using Utilities;

#pragma warning disable 0649
namespace Deck
{
	/// <summary>
	/// Visualize e.g a CardPool. Create GameObjects from card data and adds Action that can be triggered.
	/// </summary>
	public class CardCollectionView : MonoBehaviour
	{
		private event Action<CardModel> OnSelected;

		[SerializeField] private CardModel m_cardModel;
		[SerializeField] private Transform m_contentTransform;
		[SerializeField] private ConfirmationPopUp m_confirmationPopUp;

		public void AddListener(Action<CardModel> action)
		{
			OnSelected += action;
		}
		
		public void Open(CardPool pool, bool forceClose)
		{
			if (forceClose) OnOpen();
			var newPool = new List<CardData>(pool.Cards);
			newPool.Shuffle();
			foreach (var cardData in newPool)
			{
				CreateCard(new CardInstance(cardData));
			}
		}

		public void Open(CardDeck deck, bool forceClose)
		{
			if (forceClose) OnOpen();
			var newPool = new List<CardInstance>(deck.Cards);
			newPool.Shuffle();
			foreach (var card in newPool)
			{
				CreateCard(card);
			}
		}

		public void Open(IEnumerable<CardInstance> cards, bool forceClose)
		{
			if (forceClose) OnOpen();
			var newPool = new List<CardInstance>(cards);
			newPool.Shuffle();
			foreach (var card in newPool)
			{
				CreateCard(card);
			}
		}

		private void OnOpen()
		{
			if (gameObject.activeInHierarchy) Close();
			gameObject.SetActive(true);
		}

		private void CreateCard(CardInstance instance)
		{
			var newCardView = Instantiate(m_cardModel, m_contentTransform);

			Destroy(newCardView.GetComponent<HoverableCard>());

			newCardView.Setup(instance);

			var selection = newCardView.gameObject.AddComponent<SelectableCard>();
			selection.AddListener(() => OnCardSelected(newCardView));
		}

		private void OnCardSelected(CardModel view)
		{
			if (OnSelected != null)
			{
				m_confirmationPopUp.Open(() => { OnSelected?.Invoke(view); });
			}

			PopUpHandler.Instance.CloseAll();
		}

		public void Close()
		{
			OnSelected = null;

			for (var i = 0; i < m_contentTransform.childCount; i++)
			{
				Destroy(m_contentTransform.GetChild(i).gameObject);
			}

			gameObject.SetActive(false);
		}

		public void RemoveAllListeners()
		{
			OnSelected = null;
		}
	}
}
#pragma warning restore 0649
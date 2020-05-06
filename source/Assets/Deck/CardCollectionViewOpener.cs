using System;
using System.Collections.Generic;
using Cards.General;
using Misc.PopUp;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Deck
{
	/// <summary>
	/// Bridge between CardCollectionView and a simplified interface to create a card overview.
	/// Where each card can trigger specific Events.
	/// </summary>
	[RequireComponent(typeof(CardCollectionView))]
	public class CardCollectionViewOpener : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_header;
		[SerializeField] private CardCollectionView CardCollectionView = null;
		private bool m_isOpen = false;

		public void SetHeader(string name)
		{
			if (m_header)
			{
				m_header.text = name;
			}
		}
		//Note : if CardPool , CardDeck would a IEnumerable, it could be less boiler plate code.

		/// <summary>
		/// Create Overview of cards, where each card can trigger a given action.
		/// </summary>
		/// <param name="cards">Cards that will be created</param>
		/// <param name="action">Action that execute a card if selected</param>
		/// <param name="forceClose">Reset the View.</param>
		public void Open(IEnumerable<CardInstance> cards, Action<CardModel> action, bool forceClose)
		{
			if (OpenInternal(action))
			{
				CardCollectionView.Open(cards, forceClose);
			}
		}

		/// <summary>
		/// Create Overview of cards, where each card can trigger a given action.
		/// </summary>
		/// <param name="cards">Cards that will be created</param>
		/// <param name="action">Action that execute a card if selected</param>
		/// <param name="forceClose">Reset the View.</param>
		public void Open(CardPool pool, Action<CardModel> action, bool forceClose)
		{
			if (OpenInternal(action))
			{
				CardCollectionView.Open(pool, forceClose);
			}
		}

		/// <summary>
		/// Create Overview of cards, where each card can trigger a given action.
		/// </summary>
		/// <param name="cards">Cards that will be created</param>
		/// <param name="action">Action that execute a card if selected</param>
		/// <param name="forceClose">Reset the View.</param>
		public void Open(CardDeck pool, Action<CardModel> action, bool forceClose)
		{
			if (OpenInternal(action))
			{
				CardCollectionView.Open(pool, forceClose);
			}
		}

		private bool OpenInternal(Action<CardModel> action)
		{
			if (m_isOpen)
			{
				Close();
				return false;
			}

			CardCollectionView.AddListener(action);
			m_isOpen = true;
			return true;
		}

		public void RemoveAllListeners()
		{
			CardCollectionView.RemoveAllListeners();
			CardCollectionView.AddListener(null);
		}

		public void Close()
		{
			PopUpHandler.Instance.CloseAll();
			m_isOpen = false;
			CardCollectionView.Close();
		}
	}
}
#pragma warning restore 0649
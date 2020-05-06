using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Deck
{
	/// <summary>
	/// Used to wrap event and allow the player to accept or decline his decision. 
	/// </summary>
	public class ConfirmationPopUp : MonoBehaviour
	{
		[SerializeField] private Button m_accept;
		[SerializeField] private Button m_decline;

		public void Open(UnityAction onAccept)
		{
			gameObject.SetActive(true);
			m_accept.onClick.AddListener(onAccept);
			m_accept.onClick.AddListener(Close);
			m_decline.onClick.AddListener(Close);
		}

		private void Close()
		{
			m_accept.onClick.RemoveAllListeners();
			m_decline.onClick.RemoveAllListeners();
			gameObject.SetActive(false);
		}
	}
}
#pragma warning restore 0649
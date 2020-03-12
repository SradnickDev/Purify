using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.General
{
	[RequireComponent(typeof(CardModel))]
	public class CardView : MonoBehaviour
	{
		[SerializeField] private CardModel m_cardModel = null;
		[SerializeField] private TextMeshProUGUI m_energy = null;
		[SerializeField] private TextMeshProUGUI m_name = null;
		[SerializeField] private Image m_illustration = null;
		[SerializeField] private Image m_typeIcon = null;
		[SerializeField] private TextMeshProUGUI m_description = null;

		private void Update()
		{
			if (m_cardModel == null) return;
			UpdateView();
		}

		private void UpdateView()
		{
			if (m_cardModel.Instance == null) return;
			
			var cardInfo = m_cardModel.Instance.CardData;

			UpdateEnergy(cardInfo);
			UpdateName(cardInfo);
			UpdateIllustration(cardInfo);
			UpdateIcon(cardInfo);
			UpdateDescription();
		}

		private void UpdateName(CardData cardInfo)
		{
			var nameTxt = cardInfo.Name;
			if (m_name.text != nameTxt)
			{
				m_name.text = nameTxt;
			}
		}

		private void UpdateEnergy(CardData cardInfo)
		{
			var energyTxt = cardInfo.Energy.ToString();
			if (m_energy.text != energyTxt)
			{
				m_energy.text = energyTxt;
			}
		}

		private void UpdateIllustration(CardData cardInfo)
		{
			var illustration = cardInfo.Illustration;
			if (m_illustration.sprite != illustration)
			{
				m_illustration.sprite = illustration;
			}
		}

		private void UpdateIcon(CardData cardInfo)
		{
			var icon = cardInfo.Icon;
			if (m_typeIcon.sprite != icon)
			{
				m_typeIcon.sprite = icon;
			}
		}

		private void UpdateDescription()
		{
			var descriptionTxt = m_cardModel.Instance.Description(m_cardModel.Description);

			if (m_description.text != descriptionTxt)
			{
				m_description.text = descriptionTxt;
			}
		}
	}
}
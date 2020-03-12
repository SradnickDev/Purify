using System.Collections;
using System.Collections.Generic;
using Cards.General;
using Deck;
using DG.Tweening;
using Units.Player.General;
using UnityEngine;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace Battle.Zones
{
	[RequireComponent(typeof(AudioSource))]
	public class HandZone : MonoBehaviour
	{
		[SerializeField, Range(0f, 1f)] private float m_usedScreenWidth;
		[SerializeField, Range(0f, 1f)] private float m_usedAnchorHeight;

		[Header("Animation")]
		[SerializeField] private float m_duration = 0.5f;

		[SerializeField] private Ease m_ease = Ease.Linear;
		[SerializeField] private AudioClip m_drawSFX;
		[SerializeField] private AudioClip m_discardSFX;
		private AudioSource m_audioSource;

		[SerializeField]
		private Range m_angleRange = new Range(40, -40);

		[SerializeField]
		private CardModel CardModelPrefab;

		[Inject] private Player m_player;

		public List<CardModel> Cards { get; private set; } = new List<CardModel>();

		[Inject]
		private void Setup(Hand hand)
		{
			hand.Added += Add;
			hand.Removed += delegate(CardInstance instance) { StartCoroutine(Remove(instance)); };
			hand.Changed += SetHand;
			m_audioSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			RootCanvas.ResolutionChanged += DoRadialLayout;
		}

		private void SetHand(CardPile hand)
		{
			Clear();

			if (hand.Count == 0) return;

			foreach (var cardInstance in hand.GetCollection(info => true))
			{
				Add(cardInstance);
			}

			DoRadialLayout();
		}

		private void Add(CardInstance instance)
		{
			if (m_audioSource)
			{
				m_audioSource.PlayOneShot(m_drawSFX);
			}

			var newCard = Instantiate(CardModelPrefab, new Vector3(-500, 0), Quaternion.identity,
									  transform);
			newCard.Setup(instance);
			Cards.Add(newCard);
			DoRadialLayout();
		}

		private IEnumerator Remove(CardInstance instance)
		{
			if (m_audioSource)
			{
				m_audioSource.PlayOneShot(m_discardSFX);
			}

			var targetCard = GetCard(instance);
			yield return new WaitForSeconds(0.1f);
			Cards.Remove(targetCard);
			DoRadialLayout();
		}

		public CardModel GetCard(CardInstance instance)
		{
			var idx = Cards.FindIndex(x => x.Instance == instance);
			var targetCard = Cards[idx];
			return targetCard;
		}

		private void Clear()
		{
			foreach (var card in Cards)
			{
				Destroy(card.gameObject);
			}

			Cards = new List<CardModel>();
		}

		private void DoRadialLayout()
		{
			var halfWidth = Screen.width / 2f;
			var left = halfWidth - halfWidth * m_usedScreenWidth;
			var right = halfWidth + halfWidth * m_usedScreenWidth;
			var height = Screen.height;

			var leftStart = new Vector2(left, 0);
			var rightEnd = new Vector2(right, 0);
			var anchor = new Vector2(halfWidth, height * m_usedAnchorHeight);


			for (var i = 0; i < Cards.Count; i++)
			{
				var current = Cards[i];
				var percentage = (i + 1) / (float) (Cards.Count + 1);
				var angle = Mathf.Lerp(m_angleRange.Min, m_angleRange.Max, percentage);

				var pos = GeneralExtensions.CalculateQuadraticCurve(percentage,
																	leftStart,
																	anchor,
																	rightEnd);

				var hover = current.GetComponent<HoverableCard>();
				hover.IsActive = false;
				hover.TransformCache.Update(pos, Quaternion.Euler(0, 0, angle), Vector3.one, i);

				current.transform.DOMove(pos, m_duration)
					.SetEase(m_ease);

				current.transform.DORotateQuaternion(Quaternion.Euler(0, 0, angle), m_duration)
					.SetEase(m_ease)
					.OnComplete(() => hover.IsActive = true);
			}
		}

		private void OnDestroy()
		{
			RootCanvas.ResolutionChanged -= DoRadialLayout;
		}
	}
}
#pragma warning restore 0649
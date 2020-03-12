using Battle.General;
using Battle.Zones;
using Units.Enemy.General;
using Units.General;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

#pragma warning disable 0649
namespace Cards.General
{
	[RequireComponent(typeof(CardController))]
	public class CardHandling : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField, Range(0, 1f)] private float m_playHeight; 
		[HideInInspector]
		public Unit CurrentTarget;

		[SerializeField]
		private float m_targetAutoSelectTolerance = 10;

		[SerializeField]
		private CardController m_cardController;

		[Inject]
		private UILineRenderer m_lineRenderer;

		[Inject]
		private EnemyZone m_enemyZone;

		private CardModel m_selectedCardModel;
		private HoverableCard m_selectedHoverableCard;
		private bool m_inActivationHeight;
		private RectTransform m_rectTransform;
		private Enemy m_currentTarget;

		private void Start()
		{
			m_rectTransform = GetComponent<RectTransform>();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			SelectCard(eventData);
		}

		private void SelectCard(PointerEventData eventData)
		{
			if (eventData.pointerEnter == null) return;

			m_selectedCardModel = eventData.pointerEnter.GetComponent<CardModel>();
			if (m_selectedCardModel == null) return;

			if (!m_selectedCardModel.enabled)
			{
				m_selectedCardModel = null;
				m_selectedHoverableCard = null;
				return;
			}

			if (m_selectedCardModel != null &&
				!m_cardController.CanPlayCard(m_selectedCardModel.Instance))
			{
				m_selectedCardModel = null;
				m_selectedHoverableCard = null;
			}
			else
			{
				m_selectedHoverableCard = m_selectedCardModel.GetComponent<HoverableCard>();
				m_selectedCardModel.GetComponent<KeywordHandler>().DisableKeywords();
				if (m_selectedCardModel.Instance.CardData.TargetType != TargetType.Single)
				{
					m_selectedHoverableCard.IsActive = false;
					DragCard();
				}
				else
				{
					m_selectedHoverableCard.OnEnter();
					m_selectedHoverableCard.IsActive = false;
					for (var i = 0; i < m_enemyZone.transform.childCount; i++)
					{
						m_enemyZone.transform.GetChild(i).GetComponent<TargetHighlight>().Select();
					}
				}
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (m_selectedCardModel == null) return;
			if (m_selectedCardModel.Instance.CardData.TargetType == TargetType.Single)
			{
				m_selectedHoverableCard.IsActive = false;
				UpdateTargetSelection(eventData);
			}
			else
			{
				DragCard();
			}
		}

		private void DragCard()
		{
			if (m_selectedCardModel == null) return;

			m_selectedCardModel.transform.position = Input.mousePosition;

			m_inActivationHeight = Input.mousePosition.y >= Screen.height * m_playHeight;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (m_selectedCardModel == null) return;
			m_selectedCardModel.Instance.Target = null;

			//if card is not in activation height, use hover effect to reset
			if (!m_inActivationHeight &&
				m_selectedCardModel.Instance.CardData.TargetType != TargetType.Single)
			{
				m_selectedHoverableCard.IsActive = true;
				m_selectedHoverableCard.OnExit();
				ResetSelection();
				return;
			}

			if (m_selectedCardModel.Instance.CardData.TargetType == TargetType.Single)
			{
				for (var i = 0; i < m_enemyZone.transform.childCount; i++)
				{
					m_enemyZone.transform.GetChild(i).GetComponent<TargetHighlight>().Deselect();
				}

				var target = eventData.pointerEnter;
				var enemy = target != null ? target.GetComponent<Enemy>() : null;
				if (enemy != null)
				{
					PlaySelectedCard(enemy);
					m_lineRenderer.Visible(false);
					return;
				}

				m_lineRenderer.Visible(false);
				m_selectedHoverableCard.IsActive = true;
				m_selectedHoverableCard.OnExit();
				ResetSelection();
				return;
			}

			PlaySelectedCard(null);
			ResetSelection();
		}

		private void PlaySelectedCard(Enemy target)
		{
			CurrentTarget = target;
			m_selectedHoverableCard.IsActive = false;
			Destroy(m_selectedCardModel.GetComponent<HoverableCard>());
			m_cardController.Play(m_selectedCardModel.Instance, target);
			m_selectedCardModel.enabled = false;
		}

		private void ResetSelection()
		{
			m_inActivationHeight = false;
			m_selectedCardModel = null;
			m_selectedHoverableCard = null;
		}

		private void UpdateTargetSelection(PointerEventData eventData)
		{
			m_lineRenderer.Visible(true);
			var endPoint = Input.mousePosition;


			endPoint = SnapToEnemy(eventData, endPoint);
			var selectionRect = m_selectedCardModel.GetComponent<RectTransform>();

			m_lineRenderer.DrawLine
				(
				 m_selectedCardModel.transform.position,
				 new Vector2(selectionRect.position.x, selectionRect.position.y) +
				 new Vector2(0, selectionRect.sizeDelta.y / 2),
				 endPoint
				);
		}

		private Vector3 SnapToEnemy(PointerEventData eventData, Vector3 endPoint)
		{
			var targetObj = eventData.pointerEnter;
			Enemy enemy = null;
			if (targetObj)
			{
				enemy = targetObj.GetComponent<Enemy>();
			}

			if (enemy == null)
			{
				if (m_currentTarget != null)
				{
					m_currentTarget.GetComponent<TargetHighlight>().ResetFocus();
					m_currentTarget = null;
				}

				return endPoint;
			}


			if (enemy != null)
			{
				if (m_currentTarget != null && m_currentTarget != enemy)
				{
					m_currentTarget.GetComponent<TargetHighlight>().ResetFocus();
				}

				m_currentTarget = enemy;
				m_currentTarget.GetComponent<TargetHighlight>().Focus();

				m_selectedCardModel.Instance.Target = enemy;
				var distance = Vector2.Distance(Input.mousePosition, enemy.transform.position);
				if (distance <= m_targetAutoSelectTolerance)
				{
					endPoint = enemy.transform.position;
				}
			}

			return endPoint;
		}
	}
}
#pragma warning restore 0649
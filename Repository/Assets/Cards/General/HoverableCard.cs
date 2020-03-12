using DG.Tweening;
using UnityEngine;
using Utilities;

#pragma warning disable 0649
namespace Cards.General
{
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(KeywordHandler))]
	public class HoverableCard : MonoBehaviour, ISimplePointer
	{
		public bool IsActive { get; set; } = true;
		public TransformCache TransformCache { get; set; } = new TransformCache();

		[SerializeField] private AudioSource m_audioSource;
		[SerializeField] private KeywordHandler m_keywordHandler;
		[SerializeField] private float m_duration = 0.25f;
		[SerializeField] private float m_scaleFactor = 1;
		[SerializeField] private float m_yOffset;
		[SerializeField] private Ease EaseType = Ease.Linear;
		[SerializeField] private CardModel m_cardModel;

		private Sequence m_onEnter;
		private Sequence m_onExit;
		private RectTransform m_transform;
		private bool m_isPlaying = false;

		private void Start()
		{
			m_transform = GetComponent<RectTransform>();
			m_onEnter = DOTween.Sequence();
			m_onExit = DOTween.Sequence();
		}

		public void OnEnter()
		{
			if (m_isPlaying) return;

			m_onExit.Kill(true);

			CreateOnEnterSequence();

			m_transform.SetAsLastSibling();
			m_onEnter.Play();
			m_audioSource.PlayOneShot(m_audioSource.clip);
		}

		public void OnExit()
		{
			m_transform.SetSiblingIndex(TransformCache.ChildIndex);

			m_onEnter.Kill(true);

			CreateOnExitSequence();
			m_onExit.Play();
		}

		private Vector3 GetHoverPos()
		{
			var height = (m_transform.sizeDelta.y * RootCanvas.Scale.y * m_scaleFactor) / 2f;
			return new Vector3(m_transform.position.x, height + m_yOffset, 0);
		}

		private void CreateOnEnterSequence()
		{
			m_onEnter = DOTween.Sequence();


			m_onEnter.OnStart(() =>
			{
				m_transform.eulerAngles = Vector3.zero;
				m_transform.localScale = Vector3.one * m_scaleFactor;
				m_isPlaying = true;
			});

			m_onEnter.Join
			(
				m_transform.DOMove(GetHoverPos()
								   , m_duration)
					.SetEase(EaseType)
			);

			m_onEnter.OnComplete(() =>
			{
				m_keywordHandler.EnableKeywords();
				m_isPlaying = false;
			});

			m_onEnter.Pause();
		}

		private void CreateOnExitSequence()
		{
			m_onExit = DOTween.Sequence();
			m_onExit.OnStart(() =>
			{
				m_keywordHandler.DisableKeywords();
				m_isPlaying = true;
			});
			m_onExit.Join
			(
				m_transform.DOMove(
						TransformCache.Position
						, m_duration)
					.SetEase(EaseType)
			);
			m_onExit.Join
			(
				m_transform.DOScale(
						TransformCache.Scale
						, m_duration / 2)
					.SetEase(EaseType)
			);
			m_onExit.Join
			(
				m_transform.DORotate(
						TransformCache.Rot.eulerAngles
						, m_duration / 2)
					.SetEase(EaseType)
			);
			m_onExit.OnComplete(() => { m_isPlaying = false; });
			m_onExit.Pause();
		}
	}
}
#pragma warning restore 0649
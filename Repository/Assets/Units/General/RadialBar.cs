using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

#pragma warning disable 0649

namespace Units.General
{
	public class RadialBar : MonoBehaviour
	{
		[SerializeField] private bool m_setChechpoints = true;
		[SerializeField] private Transform m_pointer;
		[SerializeField] private Transform m_anchor;
		[SerializeField] private GameObject m_checkpoint;
		[SerializeField] private Range m_checkPointRange;
		[SerializeField, Range(0, 21)] private int m_checkpoints;
		[SerializeField] private float m_centerOffset = 3;

		[SerializeField] private Image m_fillTop;
		[SerializeField] private Image m_fillBottom;
		[Range(-1, 1)] [SerializeField] private float m_fillAmount = 0f;

		[Header("Animation")]
		[SerializeField] private Ease m_ease = Ease.Linear;

		[SerializeField] private float m_duration = 0.25f;

		public float BottomFillAmount
		{
			set
			{
				var val = Mathf.Clamp01(value);
				m_fillAmount = -val;
				UpdateFillAmount();
			}
		}

		public float TopFillAmount
		{
			set
			{
				var val = Mathf.Clamp01(value);
				m_fillAmount = val;
				UpdateFillAmount();
			}
		}

		public float FillAmount
		{
			get => m_fillAmount;
			set
			{
				m_fillAmount = value;
				UpdateFillAmount();
			}
		}

		private void Start()
		{
			if (m_setChechpoints)
			{
				ArrangeCheckpoints();
				UpdateCheckpoints();
			}
		}

	#if UNITY_EDITOR
		private void Update()
		{
			if (!Application.isPlaying)
			{
				ArrangeCheckpoints();
				UpdateCheckpoints();
				UpdateFillAmount();
			}
		}
	#endif
		private void ArrangeCheckpoints()
		{
			var diff = m_checkpoints - m_anchor.transform.childCount;
			if (diff > 0)
			{
				for (var i = 0; i < diff; i++)
				{
					var cp = Instantiate(m_checkpoint, m_anchor, false);
				}
			}
			else if (diff < 0)
			{
				diff = Mathf.Abs(diff);
				for (var i = diff - 1; i >= 0; i--)
				{
					var cp = m_anchor.transform.GetChild(i);
					DestroyImmediate(cp.gameObject, true);
				}
			}
		}

		private void UpdateCheckpoints()
		{
			for (var i = 0; i <= m_checkpoints - 1; i++)
			{
				var cp = m_anchor.transform.GetChild(i);
				;
				var percentage = (i) / (float) (m_checkpoints - 1);
				var angle = Mathf.Lerp(m_checkPointRange.Min, m_checkPointRange.Max, percentage);
				if (cp)
				{
					angle = float.IsNaN(angle) ? 0 : angle;
					cp.transform.rotation = Quaternion.Euler(0, 0, angle);
					cp.transform.localPosition = Vector3.zero;
					cp.transform.localPosition = -cp.transform.right * m_centerOffset;
				}
			}
		}

		private void UpdatePointer()
		{
			if (m_pointer)
			{
				var percentage = (m_fillAmount - -1) / (1f - -1f);
				var angle = Mathf.Lerp(m_checkPointRange.Max, m_checkPointRange.Min, percentage);


				if (!Application.isPlaying)
				{
					m_pointer.transform.rotation = Quaternion.Euler(0, 0, angle);
				}
				else
				{
					m_pointer.transform
							 .DORotate(new Vector3(0, 0, angle), m_duration)
							 .SetEase(m_ease);
				}
			}
		}

		private void UpdateFillAmount()
		{
			UpdateFillBarBottom();
			UpdateFillBarTop();
			UpdatePointer();
		}

		private void UpdateFillBarTop()
		{
			var amount = m_fillAmount / 1.0f;
			if (m_fillTop)
			{
				if (!Application.isPlaying)
				{
					m_fillTop.fillAmount = amount;
				}
				else
				{
					m_fillTop
						.DOFillAmount(amount, m_duration).SetEase(m_ease);
				}
			}
		}

		private void UpdateFillBarBottom()
		{
			var amount = m_fillAmount / -1.0f;
			if (m_fillBottom)
			{
				if (!Application.isPlaying)
				{
					m_fillBottom.fillAmount = amount;
				}
				else
				{
					m_fillBottom
						.DOFillAmount(amount, m_duration).SetEase(m_ease);
				}
			}
		}
	}
}
#pragma warning restore 0649
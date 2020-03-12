using System.Collections;
using UnityEngine;

#pragma warning disable 0649
namespace Animateables
{
	/// <summary>
	/// Base to Evaluate Animation Curves. Should be used to create quick and simple Animations.
	/// </summary>
	public abstract class Animateable : MonoBehaviour
	{
		[SerializeField]
		protected AnimationCurve AnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[SerializeField] private bool m_loop;
		private Coroutine m_coroutine;
		protected bool IsSetup = false;

		private void Awake()
		{
			if (!IsSetup)
			{
				Setup();
				IsSetup = true;
			}
		}

		/// <summary>
		/// Can be used to cache default Values.
		/// </summary>
		protected abstract void Setup();

		public virtual void Play()
		{
			if (!IsSetup)
			{
				Setup();
				IsSetup = true;
			}

			if (m_coroutine != null)
			{
				StopCoroutine(m_coroutine);
			}

			m_coroutine = StartCoroutine(Evaluate());
		}

		/// <summary>
		/// Can be used to reset to default Values.
		/// </summary>
		public abstract void OnReset();

		public void Stop()
		{
			StopAllCoroutines();
			OnReset();
		}

		public abstract void OnAnimate(float curveTime);

		private IEnumerator Evaluate()
		{
			var lastFrame = AnimationCurve[AnimationCurve.keys.Length - 1];
			var duration = lastFrame.time;

			var currentTime = 0.0f;

			while (currentTime <= duration)
			{
				var percentage = currentTime / duration;

				var curveTime = AnimationCurve.Evaluate(percentage);

				OnAnimate(curveTime);

				currentTime += Time.deltaTime;
				yield return null;
			}

			OnReset();

			m_coroutine = m_loop ? StartCoroutine(Evaluate()) : null;
		}
	}
}
#pragma warning restore 0649
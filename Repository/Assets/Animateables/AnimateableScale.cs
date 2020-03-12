using UnityEngine;

namespace Animateables
{
	public class AnimateableScale : Animateable
	{
		private Vector3 m_scale;

		protected override void Setup()
		{
			m_scale = transform.localScale;
		}

		public override void OnReset()
		{
			transform.localScale = m_scale;
		}

		public override void OnAnimate(float curveValue)
		{
			transform.localScale = m_scale * curveValue;
		}
	}
}
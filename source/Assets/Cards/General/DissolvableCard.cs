using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.General
{
	public static class DissolvableCard
	{
		private static readonly int m_dissolveAmount = Shader.PropertyToID("_DissolveAmount");
		private static IEnumerable<Image> m_components = null;
		private static Material m_dissolveMaterial;

		public static Material DissolveMaterial
		{
			get
			{
				if (m_dissolveMaterial == null)
				{
					m_dissolveMaterial = Resources.Load<Material>("Material/Dissolve");
				}

				return m_dissolveMaterial;
			}
		}

		private static void Setup(CardModel model)
		{
			m_components = GetAllImageComponents(model);

			foreach (var image in m_components)
			{
				image.material = DissolveMaterial;
			}
		}

		public static void Dissolve(this CardModel model, float duration)
		{
			model.GetComponent<CardPlayableEffect>().Reset();
			Setup(model);
			model.StartCoroutine(Animate(duration));
		}

		private static IEnumerable<Image> GetAllImageComponents(CardModel model)
		{
			var retVal = model.GetComponentsInChildren<Image>();
			return retVal;
		}

		private static IEnumerator Animate(float duration)
		{
			var timer = 0f;

			while (timer <= duration)
			{
				var percentage = timer / duration;

				DissolveAmount(percentage);

				timer += Time.deltaTime;
				yield return null;
			}
		}

		private static void DissolveAmount(float percentage)
		{
			foreach (var image in m_components)
			{
				image.material.SetFloat(m_dissolveAmount, percentage);
			}
		}
	}
}
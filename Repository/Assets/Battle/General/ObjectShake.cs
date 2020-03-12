using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Battle.General
{
	[System.Serializable]
	public class ObjectShake
	{
		private Transform m_transform;
		private Vector3 m_originPosition;
		private Vector3 m_originRotation;

		[System.Serializable]
		public class ShakeData
		{
			public string Name;
			public float Duration;
			public Vector2 Strength;
			public int Vibration;
			public float Randomness;
		}

		[SerializeField] private List<ShakeData> m_shakeDatas = new List<ShakeData>();

		/// <summary>
		/// Storing GameObject and that position/rotation, that will be used for shaking.
		/// </summary>
		/// <param name="transform"></param>
		public void SetObject(Transform transform)
		{
			m_transform = transform;
			m_originPosition = transform.localPosition;
			m_originRotation = transform.localEulerAngles;
		}

		/// <summary>
		/// Execute shake with data from given index.
		/// </summary>
		/// <param name="index">Index in Shake data list</param>
		public void DoShake(int index)
		{
			var data = m_shakeDatas[index];
			if (data != null)
			{
				ShakeInternal(data);
			}
		}

		/// <summary>
		/// Execute shake with data from given name.
		/// </summary>
		/// <param name="name">Shake data name</param>
		public void DoShake(string name)
		{
			var data = m_shakeDatas.Find(x => x.Name == name);
			if (data != null)
			{
				ShakeInternal(data);
			}
		}

		/// <summary>
		/// Using DoTween library to create a shake effect. 
		/// </summary>
		/// <param name="data"></param>
		private void ShakeInternal(ShakeData data)
		{
			m_transform.DOShakePosition(data.Duration,
										data.Strength,
										data.Vibration,
										data.Randomness,
										false,
										false)
					   .OnComplete(ResetTransform);
		}

		/// <summary>
		/// Reset to cached values.
		/// </summary>
		private void ResetTransform()
		{
			m_transform.localPosition = m_originPosition;
			m_transform.localEulerAngles = m_originRotation;
		}
	}
}
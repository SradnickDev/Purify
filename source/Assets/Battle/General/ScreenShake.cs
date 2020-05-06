using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#pragma warning disable 0649
namespace Battle.General
{
	/// <summary>
	/// Wraps ObjectShake, to create a screenshake effect.
	/// </summary>
	public class ScreenShake : MonoBehaviour
	{
		private static ScreenShake m_instance;

		public static ScreenShake Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = FindObjectOfType<ScreenShake>();
				}

				return m_instance;
			}
		}

		[SerializeField]
		private ObjectShake m_objectShake;

		private void Awake()
		{
			SetInstance();
		}

		public void DoShake(int index)
		{
			m_objectShake.DoShake(index);
		}

		private void SetInstance()
		{
			m_objectShake.SetObject(transform);
			m_instance = this;
		}
	}
#pragma warning restore 0649

#if UNITY_EDITOR
	[CustomEditor(typeof(ScreenShake))]
	public class ShakeInspector : Editor
	{
		private int m_dataIndex;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
			EditorGUILayout.Space();
			m_dataIndex = EditorGUILayout.IntField("Shake Index", m_dataIndex);

			if (GUILayout.Button("Shake"))
			{
				if (EditorApplication.isPlaying)
				{
					var screenShake = (ScreenShake) target;
					screenShake.DoShake(m_dataIndex);
				}
			}

			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
		}
	}
#endif
}
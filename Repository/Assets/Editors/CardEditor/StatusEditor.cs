using System;
using System.Collections.Generic;
using Status.General;
using UnityEditor;
using UnityEngine;
using Utilities;

#if UNITY_EDITOR
namespace Editors.CardEditor
{
	public class StatusEditor : EditorWindow
	{
		private const string ProjectRelativePath = "/Status/Resources/Status/";
		private const string ResourcesRelativePath = "Status/StatusList";

		private static StatusEditor m_window;
		private List<StatusData> m_targetList;
		private bool[] m_toggled;
		private Vector2 m_scrollPosition = new Vector2(0, 0);

		[MenuItem("PurifyTools/Status Editor %&e")]
		private static void ShowWindow()
		{
			m_window = GetWindow<StatusEditor>();
			m_window.titleContent = new GUIContent("Status Editor");
			m_window.Show();
		}

		private void OnGUI()
		{
			EditorGUIUtility.labelWidth = 120;
			Toolbar();

			if (m_targetList != null)
			{
				Body();
				ElementEditor.Footer(m_targetList);
			}
		}

		private void Body()
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
			GUILayout.Space(5);


			Array.Resize(ref m_toggled, m_targetList.Count);
			for (var i = 0; i < m_targetList.Count; i++)
			{
				var card = m_targetList[i];
				Status(card, i);
			}


			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void Status(StatusData statusData, int idx)
		{
			GUI.backgroundColor =
				idx % 2 == 0 ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.5f, 0.5f, 0.5f);
			GUI.backgroundColor = Color.white;
			GUILayout.Space(5);

			m_toggled[idx] =
				EditorGUILayout.Foldout(m_toggled[idx], $"Status : [{idx}] - {statusData.Name}");

			if (m_toggled[idx])
			{
				EditorGUILayout.BeginVertical(new GUIStyle("RegionBg"));
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(20);
				EditorGUILayout.BeginVertical();

				ElementEditor.Field(statusData, EditorStyles.helpBox, true, true, true);

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}

		private void Toolbar()
		{
			void CreateList()
			{
				PersistentJson.Save(new StatusDataList(),
									Application.dataPath + ProjectRelativePath,
									"StatusList");
				AssetDatabase.Refresh();
				Repaint();
			}

			void LoadList()
			{
				var list = Resources.Load<TextAsset>(ResourcesRelativePath);

				if (list)
				{
					m_targetList = PersistentJson.Create<StatusDataList>(list.text).Status;
				}
			}

			EditorToolBar.Draw(new FieldAction[]
			{
				new ButtonAction("Create", CreateList),
				new ButtonAction("Save", SaveToFile),
				new ButtonAction("Load", LoadList),
			});
		}

		private void SaveToFile()
		{
			if (m_targetList != null)
			{
				var t = new StatusDataList {Status = m_targetList};

				PersistentJson.Save(t, Application.dataPath + ProjectRelativePath,
									"StatusList");
				AssetDatabase.Refresh();
			}
		}
	}
}
#endif
using Editors.CardEditor;
using Units.Enemy.General;
using UnityEditor;
using UnityEngine;
using Utilities;

#if UNITY_EDITOR
namespace Editors
{
	public class EnemyAttackEditor : EditorWindow
	{
		private const string ResourceRelativePath = "AttackPattern/";
		private const string ProjectRelativePath = "/Units/Enemy/Resources/AttackPattern/";
		private const string WindowName = "Enemy AttackPattern Editor";

		private Enemy m_target;
		private AttackPattern m_pattern;
		private int m_deleteIdx;
		private Vector2 m_scrollPosition = new Vector2(0, 0);

		private InputAction m_inputAction = new InputAction("Index : ");

		/// <summary>
		/// To create an instance of an Editor Window.
		/// </summary>
		/// <param name="target">Selected Enemy/GameObject</param>
		public void Open(Enemy target)
		{
			m_target = target;

			LoadOrCreateFile(target);

			var window = GetWindow<EnemyAttackEditor>();
			window.titleContent = new GUIContent(WindowName);
			window.Show();
		}

		/// <summary>
		/// Load a json file for the selected Enemy or create one.
		/// </summary>
		/// <param name="target"></param>
		private void LoadOrCreateFile(Enemy target)
		{
			var jsonFiles = Resources.LoadAll<TextAsset>(ResourceRelativePath);
			var patternData = "";

			foreach (var asset in jsonFiles)
			{
				if (asset.name == target.gameObject.name)
				{
					patternData = asset.text;
				}
			}

			if (patternData == string.Empty)
			{
				CreateEmptyFile(target);
			}
			else
			{
				var obj = PersistentJson.Create<AttackPattern>(patternData);
				m_pattern = obj;
			}
		}

		private void CreateEmptyFile(Enemy target)
		{
			Debug.Log(target.gameObject.name);
			var tempData = new AttackPattern();
			PersistentJson.Save
				(
				 tempData,
				 Application.dataPath + ProjectRelativePath,
				 target.gameObject.name
				);
			m_pattern = tempData;
		}

		private void OnGUI()
		{
			if (m_pattern == null) return;

			Draw();
		}

		private void Draw()
		{
			Toolbar();

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
			GUILayout.Space(5);


			for (var i = 0; i < m_pattern.Attacks.Count; i++)
			{
				var attack = m_pattern.Attacks[i];
				EditorGUILayout.BeginVertical(new GUIStyle((GUIStyle) "RegionBg"));
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(20);
				EditorGUILayout.BeginVertical();
				foreach (var field in attack.GetType().GetFields())
				{
					EditorExtension.CreatePrimitiveFields(field, attack);
				}

				ElementEditor.FieldList(attack.Effect,
										$"[{i}] - {attack.Name}",
										false,
										false,
										true);
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
			DrawBottomBar();
		}

		/// <summary>
		/// Toolbar with save functionality.
		/// </summary>
		private void Toolbar()
		{
			EditorToolBar.Draw(new[]
			{
				new ButtonAction("Save", SaveToFile),
			});
		}

		/// <summary>
		/// Toolbar at the bottom of the window.
		/// Able to add,delete indices.
		/// </summary>
		private void DrawBottomBar()
		{
			void Remove()
			{
				if (int.TryParse(m_inputAction.Input, out var value))
				{
					if (m_pattern.Attacks.Count > 0 && value <= m_pattern.Attacks.Count)
					{
						m_pattern.Attacks.RemoveAt(value);
					}
				}
			}

			void Add()
			{
				m_pattern.Attacks.Add(new Attack());
			}

			EditorToolBar.Draw(new FieldAction[]
			{
				m_inputAction,
				new ButtonAction("Remove", Remove),
				new ButtonAction("Add", Add),
			});
		}

		private void SaveToFile()
		{
			PersistentJson.Save
				(
				 m_pattern,
				 Application.dataPath + ProjectRelativePath,
				 m_target.gameObject.name
				);
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// If a other Gameobject is selected, close this immediately.  
		/// </summary>
		private void OnSelectionChange()
		{
			Close();
		}
	}
}
#endif
using System;
using Cards.General;
using Deck;
using UnityEditor;
using UnityEngine;
using Utilities;
using Logger = Utilities.Logger;

#if UNITY_EDITOR
namespace Editors.CardEditor
{
	public class CardPoolEditor : EditorWindow
	{
		private const string WindowName = "Card Pool Editor";
		private const string ProjectRelativePath = "/Deck/Resources/CardPools/";
		private const string ResourcesRelativePath = "CardPools/";

		private static CardPoolEditor m_window;
		private CardPool m_target;
		private bool[] m_cardToggled;
		private bool[] m_effectToggled;
		private Vector2 m_scrollPosition = new Vector2(0, 0);

		private readonly InputAction m_inputAction = new InputAction("Index : ");
		private readonly InputAction m_nameAction = new InputAction("Name : ");

		[MenuItem("PurifyTools/Card Pool Editor %e")]
		private static void ShowWindow()
		{
			m_window = GetWindow<CardPoolEditor>();
			m_window.titleContent = new GUIContent(WindowName);
			m_window.Show();
		}

		private void OnGUI()
		{
			Toolbar();

			if (m_target == null) return;

			EditorGUIUtility.labelWidth = 120;
			PoolHeader();
			Body();
			DrawFooter();
		}

		/// <summary>
		/// Create and Draw Tool bar functionality.
		/// </summary>
		private void Toolbar()
		{
			void Create()
			{
				PersistentJson.Save(new CardPool() {Name = m_nameAction.Input}
								  , Application.dataPath + ProjectRelativePath
								  , m_nameAction.Input);
				AssetDatabase.Refresh();
			}

			void Load()
			{
				var menu = new GenericMenu();

				var pools = Resources.LoadAll<TextAsset>(ResourcesRelativePath);
				foreach (var pool in pools)
				{
					menu.AddItem(new GUIContent(pool.name), false
							   , () => m_target = PersistentJson.Create<CardPool>(pool.text));
				}

				if (pools.Length == 0)
				{
					menu.AddItem(new GUIContent("no pool available"), false, null);
				}

				menu.ShowAsContext();
			}

			EditorToolBar.Draw(new FieldAction[]
			{
				m_nameAction,
				new ButtonAction("Create", Create),
				new ButtonAction("Save", SaveTargetToFile),
				new ButtonAction("Load", Load)
			});
		}

		private void SaveTargetToFile()
		{
			if (m_target != null && m_target.Name != string.Empty)
			{
				ForceRefreshImmediate();
				Logger.Log(Color.green, m_target.Name, "saved");
				PersistentJson.Save(m_target,
									Application.dataPath + ProjectRelativePath,
									m_target.Name);
				AssetDatabase.Refresh();
			}
		}

		/// <summary>
		/// Force a refresh of all drawn data, loads specific json files and updates old with new Values.
		/// </summary>
		private void ForceRefreshImmediate()
		{
			for (var i = 0; i < m_cardToggled.Length; i++)
			{
				m_cardToggled[i] = true;
			}

			for (var i = 0; i < m_effectToggled.Length; i++)
			{
				m_effectToggled[i] = true;
			}

			Body();

			for (var i = 0; i < m_cardToggled.Length; i++)
			{
				m_cardToggled[i] = false;
			}

			for (var i = 0; i < m_effectToggled.Length; i++)
			{
				m_effectToggled[i] = false;
			}
		}

		private void PoolHeader()
		{
			CardPoolEditorStyles.BeginHeaderElement();

			m_target.Name = EditorGUILayout.TextField("Pool Name :", m_target.Name);
			m_target.Id = EditorGUILayout.IntField("ID :", m_target.Id);

			CardPoolEditorStyles.EndHeaderElement();
		}

		private void Body()
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
			GUILayout.Space(5);

			var cards = m_target.Cards;
			Array.Resize(ref m_cardToggled, cards.Count);
			Array.Resize(ref m_effectToggled, cards.Count);
			for (var i = 0; i < cards.Count; i++)
			{
				var card = cards[i];
				Card(card, i);
			}


			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}

		private void Card(CardData card, int idx)
		{
			GUILayout.Space(5);


			m_cardToggled[idx] =
				EditorGUILayout.Foldout(m_cardToggled[idx], $"Card : [{idx}] - {card.Name}");

			if (m_cardToggled[idx])
			{
				EditorGUILayout.BeginVertical(new GUIStyle((GUIStyle) "RegionBg"));
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(20);
				EditorGUILayout.BeginVertical();

				CardBody(card);

				CardEffects(card, idx);

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}

		private void CardEffects(CardData card, int idx)
		{
			EditorGUILayout.BeginHorizontal();
			m_effectToggled[idx] = EditorGUILayout.Foldout(m_effectToggled[idx], "Effects");
			GUILayout.Space(20);
			EditorGUILayout.BeginVertical();

			if (m_effectToggled[idx])
			{
				DrawEffectLists(card);
			}

			void DrawEffectLists(CardData targetCard)
			{
				ElementEditor.FieldList(targetCard.EarlyEffects,
										nameof(targetCard.EarlyEffects),
										false,
										false,
										true);

				ElementEditor.FieldList(targetCard.PlayCondition,
										nameof(targetCard.PlayCondition),
										false,
										false,
										true);

				ElementEditor.FieldList(targetCard.PlayEffect,
										nameof(targetCard.PlayEffect),
										false,
										false,
										true);

				ElementEditor.FieldList(targetCard.DiscardCondition,
										nameof(targetCard.DiscardCondition),
										false,
										false,
										true);
			}

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}

		private static void CardBody(CardData card)
		{
			foreach (var fieldInfo in card.GetType().GetFields())
			{
				if (fieldInfo.Name == nameof(card.Icon))
				{
					card.Icon = DeckUtility.GetIconWithType(card.Type);
				}

				EditorExtension.CreatePrimitiveFields(fieldInfo, card);
			}
		}

		private void DrawFooter()
		{
			void Remove()
			{
				if (int.TryParse(m_inputAction.Input, out var value))
				{
					if (m_target.Cards.Count > 0 && value <= m_target.Cards.Count)
					{
						m_target.Remove(value);
					}
				}
			}

			void Add()
			{
				m_target.Add(new CardData());
			}

			EditorToolBar.Draw(new FieldAction[]
			{
				m_inputAction,
				new ButtonAction("Remove", Remove),
				new ButtonAction("Add", Add),
			});
		}
	}
}
#endif
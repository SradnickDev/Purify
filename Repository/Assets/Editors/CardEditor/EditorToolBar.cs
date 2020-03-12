using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Editors.CardEditor
{
	/// <summary>
	/// Simple Toolbar, that can be extended with different actions.
	/// </summary>
	public static class EditorToolBar
	{
		/// <summary>
		/// Draw Toolbar with given actions.
		/// </summary>
		/// <param name="actions">Field Actions</param>
		public static void Draw(params FieldAction[] actions)
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar,
									  GUILayout.ExpandWidth(true),
									  GUILayout.MaxHeight(22));

			GUILayout.FlexibleSpace();


			foreach (var btn in actions)
			{
				switch (btn)
				{
					case ButtonAction buttonAction:
						if (GUILayout.Button(btn.Name, EditorStyles.toolbarButton,
											 GUILayout.MaxWidth(88)))
						{
							buttonAction.Action?.Invoke();
						}

						break;
					case InputAction inputAction:
						GUILayout.BeginHorizontal();

						GUILayout.Label(new GUIContent(inputAction.Name));
						inputAction.Input = GUILayout.TextField(inputAction.Input,
																EditorStyles.toolbarTextField,
																GUILayout.MaxWidth(88));

						GUILayout.EndHorizontal();
						break;
				}
			}


			GUILayout.EndHorizontal();
		}
	}
}
#endif
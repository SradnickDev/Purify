using Units.Enemy.General;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Editors
{
	[CustomEditor(typeof(Enemy))]
	public class EnemyAttackInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			DrawButton();
		}

		//Draw Button that opens a Editor for the selected Enemy.
		private void DrawButton()
		{
			if (GUILayout.Button("Open Attack Editor"))
			{
				var editor = CreateInstance<EnemyAttackEditor>();
				editor.Open((Enemy) target);
			}
		}
	}
}
#endif
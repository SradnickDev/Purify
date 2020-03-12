using UnityEditor;
using UnityEngine;

namespace Misc
{
	//To fix a unity bug that sets the scale of a Canvas to 0, if the canvas is the root object of  prefab
	[RequireComponent(typeof(Canvas))]
	public class CanvasInstantSize : MonoBehaviour
	{
	#if UNITY_EDITOR
		private void OnEnable()
		{
			PrefabUtility.prefabInstanceUpdated += PrefabInstanceUpdated;
		}

		private void OnDisable()
		{
			PrefabUtility.prefabInstanceUpdated -= PrefabInstanceUpdated;
		}

		private void OnValidate()
		{
			if (transform.localScale == Vector3.zero)
			{
				Debug.LogWarning($"{this} Scale was set to zero..");
				transform.localScale = new Vector3(1, 1, 1);
			}
		}

		private void PrefabInstanceUpdated(GameObject instance)
		{
			ResetScale(instance);
		}

		private void ResetScale(GameObject instance)
		{
			if (instance == this.gameObject)
			{
				transform.localScale = new Vector3(1, 1, 1);
			}
		}
	#endif
	}
}
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
	public class SimplePointerEvent : MonoBehaviour, ISimplePointer
	{
		public UnityEvent OnPointerEnter;
		public UnityEvent OnPointerExit;
		public bool IsActive { get; set; }
		public void OnEnter() => OnPointerEnter?.Invoke();

		public void OnExit() => OnPointerExit?.Invoke();
	}
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Resting
{
	/// <summary>
	/// Base for rest button. Provides simple Interface to create new Mechanics.
	/// </summary>
	[RequireComponent(typeof(Button))]
	public abstract class RestMechanic : MonoBehaviour
	{
		[Inject, HideInInspector]
		public RestMenu RestMenu;

		public TextMeshProUGUI CostsCounter;
		public int Costs = 0;

		protected Button Button;

		public virtual void Start()
		{
			Setup();
		}

		private void Setup()
		{
			Button = GetComponent<Button>();
			Button.onClick.AddListener(() =>
			{
				ApplyMechanic();
				RestMenu.Save();
			});

			if (!RestMenu)
			{
				Debug.LogError("Rest Mechanic is missing Rest Menu reference!", this);
			}

			if (CostsCounter)
			{
				CostsCounter.text = Costs.ToString();
			}
		}

		public abstract void ApplyMechanic();

		private void Update()
		{
			UpdateAvailableStatus();
		}

		protected virtual void UpdateAvailableStatus()
		{
			if (RestMenu)
			{
				Button.interactable = RestMenu.HasRestPoints(Costs);
			}
		}
	}
}
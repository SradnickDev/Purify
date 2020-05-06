using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OptionMenu
{
	public class GraphicSettings : MonoBehaviour
	{
		[SerializeField] private TMP_Dropdown ResolutionDropDown = null;
		[SerializeField] private Toggle FullscreenToggle = null;

		private void Start()
		{
			SetResolutionDropDownOptions();
			InitGraphicSettings();
		}

#region GraphicSettings

		private void InitGraphicSettings()
		{
			FullscreenToggle.isOn = Screen.fullScreen;

			ResolutionDropDown.onValueChanged.AddListener(OnResolutionDropDownChanged);
			FullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChange);
		}

		private void SetResolutionDropDownOptions()
		{
			ResolutionDropDown.ClearOptions();
			var resolution = Screen.resolutions;
			var matchingResolution = 0;
			var options = new List<string>();

			for (var i = 0; i < resolution.Length; i++)
			{
				var resInfo = resolution[i];
				var res = $"{resInfo.width} x {resInfo.height}  {resInfo.refreshRate} Mhz";

				if (!options.Contains(res))
				{
					options.Add(res);
				}

				if (resInfo.height == Screen.height &&
					resInfo.width == Screen.width)
				{
					matchingResolution = i;
				}
			}

			ResolutionDropDown.AddOptions(options);
			ResolutionDropDown.value = matchingResolution;
			ResolutionDropDown.onValueChanged.RemoveListener(OnResolutionDropDownChanged);
		}

		private void OnResolutionDropDownChanged(int index)
		{
			var resolution = Screen.resolutions;
			var targetResolution = resolution[index];

			Screen.SetResolution(targetResolution.width, targetResolution.height,
								 Screen.fullScreenMode,
								 targetResolution.refreshRate);
		}

		private void OnFullscreenToggleChange(bool value)
		{
			Screen.fullScreen = value;
		}

		private void OnDestroy()
		{
			FullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggleChange);
		}

#endregion
	}
}
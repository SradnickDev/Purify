using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace OptionMenu
{
	public class AudioSettings : MonoBehaviour
	{
		public const string MasterPref = "masterPref";
		public const string MusicPref = "musicPref";
		public const string SFXPref = "SFXPref";

		[SerializeField] private AudioMixer AudioMixer = null;
		[SerializeField] private Slider MasterSlider = null;
		[SerializeField] private Slider MusicSlider = null;
		[SerializeField] private Slider SFXSlider = null;

		private void Start()
		{
			InitSoundSettings();
		}

		private void OnEnable()
		{
			InitSoundSettings();
		}

		private void OnDisable()
		{
			MasterSlider.onValueChanged.RemoveListener(OnMasterSliderChanged);
			MusicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
			SFXSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
		}

#region AudioSettings
		
		private void InitSoundSettings()
		{
			LoadSettings();

			MasterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
			MusicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
			SFXSlider.onValueChanged.AddListener(OnSFXSliderChanged);
		}

		public void LoadSettings()
		{
			MasterSlider.value = PlayerPrefs.GetFloat(MasterPref, 0.75f);
			SFXSlider.value = PlayerPrefs.GetFloat(SFXPref, 0.75f);
			MusicSlider.value = PlayerPrefs.GetFloat(MusicPref, 0.75f);

			OnMasterSliderChanged(MasterSlider.value);
			OnMusicSliderChanged(MusicSlider.value);
			OnSFXSliderChanged(SFXSlider.value);
		}

		private void OnMasterSliderChanged(float value)
		{
			AudioMixer.SetFloat("Master", 20f * Mathf.Log10(value));
			PlayerPrefs.SetFloat(MasterPref, value);
		}

		private void OnMusicSliderChanged(float value)
		{
			AudioMixer.SetFloat("Music", 20f * Mathf.Log10(value));
			PlayerPrefs.SetFloat(MusicPref, value);
		}

		private void OnSFXSliderChanged(float value)
		{
			AudioMixer.SetFloat("SFX", 20f * Mathf.Log10(value));
			PlayerPrefs.SetFloat(SFXPref, value);
		}

		private void OnDestroy()
		{
			MasterSlider.onValueChanged.RemoveListener(OnMasterSliderChanged);
			MusicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
			SFXSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
		}

#endregion
	}
}
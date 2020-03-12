using UnityEngine;
using UnityEngine.Audio;
using AudioSettings = OptionMenu.AudioSettings;

#pragma warning disable 0649
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
	private static MusicPlayer m_instance;

	public static MusicPlayer Instance
	{
		get
		{
			if (m_instance == null)
			{
				var t = FindObjectOfType<MusicPlayer>();
				if (t == null)
				{
					t = Instantiate(Resources.Load<MusicPlayer>("MusicPlayer"));
				}

				m_instance = t;
			}

			return m_instance;
		}
	}

	[SerializeField] private AudioSource m_audioSource;
	[SerializeField] private AudioMixer AudioMixer = null;

	private void Awake()
	{
		m_audioSource = GetComponent<AudioSource>();
		LoadSettings();
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			m_instance = this;
		}

		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// Load and set Audio volumes. 
	/// </summary>
	private void LoadSettings()
	{
		var masterValue = PlayerPrefs.GetFloat(AudioSettings.MasterPref, 0.75f);
		var sfxValue = PlayerPrefs.GetFloat(AudioSettings.SFXPref, 0.75f);
		var musicValue = PlayerPrefs.GetFloat(AudioSettings.MusicPref, 0.75f);

		AudioMixer.SetFloat("Master", 20f * Mathf.Log10(masterValue));
		AudioMixer.SetFloat("Music", 20f * Mathf.Log10(sfxValue));
		AudioMixer.SetFloat("SFX", 20f * Mathf.Log10(musicValue));
	}

	public void Play(AudioClip clip)
	{
		if (m_audioSource.clip != null && m_audioSource.clip == clip) return;
		m_audioSource.clip = clip;
		m_audioSource.Play();
	}

	public void Stop()
	{
		m_audioSource.Stop();
	}
}
#pragma warning restore 0649
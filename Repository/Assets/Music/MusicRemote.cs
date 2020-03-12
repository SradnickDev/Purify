using UnityEngine;

#pragma warning disable 0649
public class MusicRemote : MonoBehaviour
{
	[SerializeField] private bool m_playOnAwake;
	[SerializeField] private AudioClip m_clip;

	private void Start()
	{
		if (m_playOnAwake)
		{
			Play();
		}
	}

	public void Play() => MusicPlayer.Instance.Play(m_clip);

	public void Stop() => MusicPlayer.Instance.Stop();
}
#pragma warning restore 0649
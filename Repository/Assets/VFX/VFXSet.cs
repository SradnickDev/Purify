using Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace VFX
{
	/// <summary>
	/// Combines custom Ui particle system and AudioSource.
	/// </summary>
	[RequireComponent(typeof(ParticleSystem))]
	[RequireComponent(typeof(UIParticleSystem))]
	[RequireComponent(typeof(AudioSource))]
	public class VFXSet : MonoBehaviour, IPoolable
	{
		private AudioSource m_source;
		private ParticleSystem m_particleSystem;

		public ParticleSystem ParticleSystem
		{
			get
			{
				if (m_particleSystem == null)
				{
					m_particleSystem = GetComponent<ParticleSystem>();
				}

				return m_particleSystem;
			}
		}

		public AudioSource Source
		{
			get
			{
				if (m_source == null)
				{
					m_source = GetComponent<AudioSource>();
				}

				return m_source;
			}
		}

		public bool Active() => ParticleSystem.isPlaying || Source.isPlaying;

		public void Activate()
		{
			ParticleSystem.Stop(true);
			ParticleSystem.Play(true);
			Source.Play();
		}

		public void Deactivate()
		{
			ParticleSystem.Stop(true);
			Source.Stop();
		}
	}

#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(typeof(VFXSet))]
	public class VFXSetInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("Play"))
			{
				var set = (VFXSet) target;
				set.Activate();
			}
		}
	}
#endif
}
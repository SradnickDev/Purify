using System.Collections.Generic;
using Battle.General;
using Cards.General;
using Misc;
using Units.Enemy.General;
using UnityEngine;
using Utilities;
using Zenject;

#pragma warning disable 0649
namespace VFX
{
	public class CardVFXHandler : MonoBehaviour
	{
		private static CardVFXHandler m_instance;

		public static CardVFXHandler Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = FindObjectOfType<CardVFXHandler>();
				}

				return m_instance;
			}
		}

		[SerializeField] private int m_initialPoolSize = 3;
		[SerializeField] private List<VFXSet> m_vfxSets = new List<VFXSet>();

		private Dictionary<int, ObjectPool<VFXSet>> m_pools =
			new Dictionary<int, ObjectPool<VFXSet>>();

		[Inject]
		private CardHandling m_cardHandling;

		private void Awake()
		{
			m_instance = this;
		}

		private void Start() => Initialize();

		/// <summary>
		/// FIll pool with VFX Sets.
		/// </summary>
		private void Initialize()
		{
			for (var index = 0; index < m_vfxSets.Count; index++)
			{
				var vfxSet = m_vfxSets[index];

				if (!m_pools.ContainsKey(index))
				{
					var pool = new ObjectPool<VFXSet>();
					pool.Initialize(m_initialPoolSize, vfxSet, this.transform);
					m_pools.Add(index, pool);
				}
			}
		}

		/// <summary>
		/// Activate an VFXSet found at index, positioning based on TargetType.
		/// </summary>
		/// <param name="idx">VFXSet index in Collection</param>
		/// <param name="targetType">Needs to be set properly, used for positioning.</param>
		public void Play(int idx, TargetType targetType)
		{
			if (idx > m_vfxSets.Count || idx == -1) return;

			switch (targetType)
			{
				case TargetType.Single:
					ActivateVFXSet(idx, m_cardHandling.CurrentTarget.transform.position);
					break;

				case TargetType.AllEnemies:
				case TargetType.AllUnits:
					foreach (var enemy in BattleInfo.Encounter)
					{
						ActivateVFXSet(idx, enemy.transform.position);
					}

					break;

				case TargetType.Self:
					ActivateVFXSet(idx, GeneralUtilities.ScreenCenter);
					break;
			}
		}

		/// <summary>
		/// Mainly used from Enemies, Simple activates a set at index.
		/// Positioned always on the Enemy.
		/// </summary>
		public void Play(int idx, Enemy enemy)
		{
			if (idx > m_vfxSets.Count || idx == -1) return;
			ActivateVFXSet(idx, enemy.transform.position);
		}

		/// <summary>
		/// Retrieves an set from a Pool and Activates it.
		/// </summary>
		/// <param name="idx">Set in Collection</param>
		/// <param name="position">Placed at Position</param>
		private void ActivateVFXSet(int idx, Vector3 position)
		{
			var vfxSet = m_pools[idx].Pop();
			vfxSet.transform.position = position;
			vfxSet.Activate();
		}

		/// <summary>
		/// Tracks if sets are don't playing and push them back to the pool.
		/// </summary>
		private void LateUpdate()
		{
			foreach (var pool in m_pools.Values)
			{
				pool.PushNonActiveObjects();
			}
		}
	}
}
#pragma warning restore 0649
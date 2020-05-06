using Units.Player.General;
using UnityEngine;
using Zenject;

#pragma warning disable 0649
namespace Battle
{
    public class EnergyView : MonoBehaviour
    {
        [SerializeField] private GameObject m_particleSystem;
        [Inject] private Player m_player;

        private void OnEnable()
        {
            m_player.Energy.CurrentChanged += OnEnergyChanged;
        }

        private void OnDisable()
        {
            m_player.Energy.CurrentChanged -= OnEnergyChanged;
        }

        private void OnEnergyChanged()
        {
            if (m_particleSystem == null) return;

            m_particleSystem.SetActive(m_player.Energy.Current > 0);
        }
    }
}
#pragma warning restore 0649
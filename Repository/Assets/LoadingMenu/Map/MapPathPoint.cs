using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace LoadingMenu.Map {
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(Animator))]
	public class MapPathPoint : MonoBehaviour
	{
		[SerializeField] private Image m_image;
		[SerializeField] private Animator m_animator;
		[SerializeField] private Sprite m_awaiting;
		[SerializeField] private Sprite m_current;
		[SerializeField] private Sprite m_visited;
		private readonly int Play = Animator.StringToHash("Play");

		private void Reset()
		{
			m_image = GetComponent<Image>();
			m_animator = GetComponent<Animator>();
		}

		public void Setup(bool visited)
		{
			if (visited)
			{
				m_image.sprite = m_visited;
			}
			else
			{
				m_image.sprite = m_awaiting;
			}
		}

		public void PlayAnimation()
		{
			m_image.sprite = m_current;
			m_animator.SetTrigger(Play);
		}
	}
}
#pragma warning restore 0649
using Battle.General;

namespace Battle.GameStates
{
	public abstract class GameState
	{
		public bool IsCompleted { get; protected set; }
		public int Turn { get; protected set; }

		private bool m_started;

		/// <summary>
		/// Handles method execution order.
		/// </summary>
		/// <returns>True if done otherwise false.</returns>
		public bool Execute()
		{
			if (!m_started)
			{
				Turn++;
				Start();
				m_started = true;
				IsCompleted = false;
				return false;
			}

			if (!IsCompleted)
			{
				Update();
				return false;
			}

			Finish();
			m_started = false;

			return true;
		}

		/// <summary>
		/// Similar to Unity's Start, called at first. Used to Setup things, called every time the state starts.
		/// </summary>
		protected abstract void Start();

		/// <summary>
		/// Similar to Unity's Update. Executed till round ends.
		/// </summary>
		protected abstract void Update();

		/// <summary>
		/// Called at the end of the state.
		/// </summary>
		protected abstract void Finish();
	}
}
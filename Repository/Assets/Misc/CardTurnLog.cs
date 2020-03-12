using System.Collections.Generic;
using Cards.General;
using Misc.Events;

namespace Misc
{
	/// <summary>
	/// Tracks played Cards.
	/// </summary>
	public static class CardTurnLog
	{
		private static Dictionary<CardType, int> m_log = new Dictionary<CardType, int>()
		{
			{CardType.Attack, 0},
			{CardType.Blessing, 0},
			{CardType.Defense, 0},
			{CardType.Prayer, 0}
		};

		public static int Count(CardType type)
		{
			m_log.TryGetValue(type, out var num);
			return num;
		}

		public static void Add(CardType type)
		{
			EventLog.Add(new PlayedCardType(type));
			if (m_log.ContainsKey(type))
			{
				m_log[type]++;
			}
			else
			{
				m_log.Add(type, 1);
			}
		}

		public static void Clear()
		{
			m_log = new Dictionary<CardType, int>()
			{
				{CardType.Attack, 0},
				{CardType.Blessing, 0},
				{CardType.Defense, 0},
				{CardType.Prayer, 0}
			};
		}
	}
}
using System.Collections.Generic;
using Misc.Events;

namespace Status.General
{
	public class CardEventComparer : IEqualityComparer<CardEvent>
	{
		public bool Equals(CardEvent x, CardEvent y)
		{
			return x?.GetType() == y?.GetType() && x?.GetHashCode() == y?.GetHashCode();
		}

		public int GetHashCode(CardEvent obj)
		{
			return obj.GetHashCode();
		}
	}
}
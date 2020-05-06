using Cards.General;

namespace Misc.Events
{
	public abstract class CardEvent
	{
		protected CardEvent() { }

		protected CardEvent(string key)
		{
			Key = key;
		}

		public string Key = "";
		public override int GetHashCode() => Key.GetHashCode();
	}

#region Unit

	public class Debuffed : CardEvent
	{
		public Debuffed() { }
		public Debuffed(string key) : base(key) { }
	}

	public class Buffed : CardEvent
	{
		public Buffed() { }
		public Buffed(string key) : base(key) { }
	}

	public class Damaged : CardEvent
	{
		public Damaged() { }
		public Damaged(string key) : base(key) { }
	}

	public class Attacked : CardEvent
	{
		public Attacked() { }
		public Attacked(string key) : base(key) { }
	}

	public class Healed : CardEvent
	{
		public Healed() { }
		public Healed(string key) : base(key) { }
	}

	public class GetDefense : CardEvent
	{
		public GetDefense() { }
		public GetDefense(string key) : base(key) { }
	}

	public class BlockedDamage : CardEvent
	{
		public BlockedDamage() { }
		public BlockedDamage(string key) : base(key) { }
	}

	public class Corrupted : CardEvent
	{
		public Corrupted() { }
		public Corrupted(string key) : base(key) { }
	}

	public class Purified : CardEvent
	{
		public Purified() { }
		public Purified(string key) : base(key) { }
	}

#endregion Unit

#region Cards

	public class PlayedCardType : CardEvent
	{
		public PlayedCardType(CardType type) : base(type.ToString()) { }
	}

	public class BanishCard : CardEvent
	{
		public BanishCard() { }
		public BanishCard(string key) : base(key) { }
	}

#endregion Cards

#region Battle

	public class TurnStart : CardEvent
	{
		public TurnStart() { }
		public TurnStart(string key) : base(key) { }
	}

	public class TurnEnd : CardEvent
	{
		public TurnEnd() { }
		public TurnEnd(string key) : base(key) { }
	}

	public class PlayerWon : CardEvent
	{
		public PlayerWon() { }
		public PlayerWon(string key) : base(key) { }
	}

	public class PlayerLost : CardEvent
	{
		public PlayerLost() { }
		public PlayerLost(string key) : base(key) { }
	}

#endregion Battle
}
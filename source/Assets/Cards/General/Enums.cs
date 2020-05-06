namespace Cards.General
{
	public enum CardType
	{
		Attack,
		Defense,
		Prayer,
		Blessing,
	}

	public enum TargetType
	{
		Self,
		Single,
		AllEnemies,
		AllUnits
	}

	public enum CustomTarget
	{
		Self,
		SpecifiedTarget
	}

	public enum StackType
	{
		Effect,
		Duration,
		Instances,
		Not
	}

	public enum TurnPhase
	{
		Restock,
		StatusPhase,
		PlayPhase,
		EndPhase
	}

	public enum BuffType
	{
		None,
		Buff,
		Debuff
	}

	public enum DiscardPileType
	{
		Default,
		BanishPile,
		None
	}

	public enum ZonePosition
	{
		Anywhere,
		Middle
	}
	
	public enum RectAnchor
	{
		Top,
		Bottom,
		Left,
		Right,
	}
}
using Cards.Effects.Condition;
using Cards.General;
using MessagePack;
using Units.General;
using Units.Player.General;

namespace Cards.Effects.General
{
	[Union(0, typeof(Retain))] [Union(1, typeof(Demonic))] [Union(2, typeof(Divine))]
	[MessagePackObject(true)]
	public abstract class ConditionEffect : IDescriptionValue
	{
		public ConditionEffect() { }

		public virtual bool HasReached(Player player)
		{
			return DoEffect(player);
		}

		protected abstract bool DoEffect(Player player);
		public abstract object Value(Unit @from, Unit target);
	}
}
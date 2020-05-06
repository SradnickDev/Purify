using System.Collections.Generic;
using System.Linq;
using Cards.Effects.General;
using MessagePack;
using UnityEngine;

namespace Cards.General
{
	/// <summary>
	/// Contains only data.
	/// </summary>
	[MessagePackObject(true)] [System.Serializable]
	public class CardData
	{
		public string Name;

		public int Id;

		public Sprite Illustration;

		public Sprite Icon;

		public int VFXIndex;

		public int Energy;

		[TextArea] public string Description;

		public Rarity Rarity = Rarity.Common;

		public CardType Type = CardType.Attack;

		public TargetType TargetType = TargetType.Single;

		//called when creating drawPile (RestockPhase)
		public List<Effect> EarlyEffects = new List<Effect>();

		//called right before the card is played / if card can be played
		public List<ConditionEffect> PlayCondition = new List<ConditionEffect>();

		//called on play
		public List<Effect> PlayEffect = new List<Effect>();

		//called right before card is discard / can be discarded
		public List<ConditionEffect> DiscardCondition = new List<ConditionEffect>();
	}
}
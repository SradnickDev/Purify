using System.Collections.Generic;
using System.Linq;
using Units.Enemy.General;
using Units.General;
using Units.Player.General;
using VFX;

namespace Cards.General
{
	[System.Serializable]
	public class CardInstance
	{
		public CardData CardData;
		public int LastPlayEffect { get; private set; }
		public Unit Owner { get; private set; }
		public Unit Target { get; set; }

		public CardInstance(CardData cardData)
		{
			CardData = cardData;
		}

		public CardInstance(CardData cardData, Unit owner)
		{
			CardData = cardData;
			Owner = owner;
		}

		public bool CanPlay(Player player)
		{
			var hasEnergy = player.Energy.Current >= CardData.Energy;

			if (CardData.PlayCondition.Any(conditionEffect => !conditionEffect.HasReached(player)))
			{
				return false;
			}

			return hasEnergy;
		}

		public void Play(Enemy enemy)
		{
			for (LastPlayEffect = 0; LastPlayEffect < CardData.PlayEffect.Count; LastPlayEffect++)
			{
				var effect = CardData.PlayEffect[LastPlayEffect];
				effect.Use(enemy, Owner, CardData.TargetType);
			}

			CardVFXHandler.Instance.Play(CardData.VFXIndex, CardData.TargetType);
		}

		public bool CanDiscard(Player player)
		{
			if (CardData.DiscardCondition.Count <= 0) return true;

			foreach (var condition in CardData.DiscardCondition)
			{
				if (!condition.HasReached(player))
					return false;
			}

			return true;
		}

		private string ParseValues(string description)
		{
			return DescriptionParser.Parse(description,
										   Owner,
										   Target,
										   new List<IDescriptionValue>(CardData.EarlyEffects),
										   new List<IDescriptionValue>(CardData.PlayCondition),
										   new List<IDescriptionValue>(CardData.PlayEffect),
										   new List<IDescriptionValue>(CardData.DiscardCondition)
										  );
		}

		public string Description(string description)
		{
			return ParseValues(description);
		}
	}
}
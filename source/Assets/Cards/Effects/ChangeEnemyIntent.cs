using Cards.Effects.General;
using MessagePack;
using Units.Enemy.General;
using Units.General;
using UnityEngine;

namespace Cards.Effects
{
	[MessagePackObject(true)]
	public class ChangeEnemyIntent : Effect
	{
		public string Name;
		public AudioClip AttackSound;
		public string Description;
		public Sprite Icon;
		public int IntentionPriorityIndex;
		public Effect Effect;

		public override void Execute(Unit target, Unit from)
		{
			if (target is Enemy enemy)
			{
				ApplyNewAttack(enemy);
			}
		}

		private void ApplyNewAttack(Enemy enemy)
		{
			var newAttack = new Attack
			{
				Name = Name,
				Icon = Icon,
				AttackSound = AttackSound,
				Description = Description,
				IntentionPriorityIndex = IntentionPriorityIndex
			};

			newAttack.Effect.Add(Effect);
			enemy.NextAttack = newAttack;
		}

		public override object Value(Unit @from, Unit target) => "";
	}
}
using System.Collections.Generic;
using System.Linq;
using Cards.Effects.General;
using Cards.General;
using MessagePack;
using Units.General;
using UnityEngine;

namespace Units.Enemy.General
{
	[MessagePackObject(true)]
	public class Attack : IDescriptionValue
	{
		public string Name;
		public string AnimationName = "Attack";
		public AudioClip AttackSound;
		public int VFXIndex;
		[TextArea] public string Description;
		public Sprite Icon;
		public List<Effect> Effect = new List<Effect>();
		public int IntentionPriorityIndex;

		public object Value(Unit from, Unit target)
		{
			return Effect[IntentionPriorityIndex].Value(from, target);
		}
	}
}
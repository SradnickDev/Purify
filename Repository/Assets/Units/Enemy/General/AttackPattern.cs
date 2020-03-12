using System.Collections.Generic;
using MessagePack;

namespace Units.Enemy.General
{
	[MessagePackObject(true)]
	public class AttackPattern
	{
		public List<Attack> Attacks = new List<Attack>();
	}
}
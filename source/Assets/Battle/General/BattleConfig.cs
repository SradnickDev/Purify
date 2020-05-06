using MessagePack;
using Utilities;

namespace Battle.General
{
	/// <summary>
	/// Data that will be saved.
	/// </summary>
	[MessagePackObject(true)]
	public class BattleConfig
	{
		[Key(0)]
		public int BattleCount = 0;

		[Key(1)]
		public Range Health;

		[Key(2)]
		public int Soul;

		[Key(3)]
		public int RestCount = 3;
	}
}
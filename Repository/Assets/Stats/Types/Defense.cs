using MessagePack;
using Stats.General;

namespace Stats.Types
{
	[System.Serializable][MessagePackObject(true)]
	public sealed class Defense : AbstractStat
	{
		public override int Clamp(int current, int min, int max)
		{
			return base.Clamp(current, -999, 999);
		}
	}
}
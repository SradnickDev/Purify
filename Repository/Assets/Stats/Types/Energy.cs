using MessagePack;
using Stats.General;

namespace Stats.Types
{
	[System.Serializable][MessagePackObject(true)]
	public class Energy : AbstractStat
	{
		public override int Clamp(int current, int min, int max)
		{
			return base.Clamp(current, -999, 999);
		}

		public void Refill()
		{
			Current = Max;
		}
	}
}
using MessagePack;
using Stats.General;

namespace Stats.Types
{
	[System.Serializable][MessagePackObject(true)]
	public sealed class Health : AbstractStat { }
}
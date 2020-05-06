using MessagePack;

namespace Utilities
{
	[System.Serializable, MessagePackObject(true)]
	public class Range
	{
		public int Min;
		public int Max;
		public Range() { }

		public Range(int min, int max)
		{
			Min = min;
			Max = max;
		}

		public override string ToString()
		{
			return $"{nameof(Min)}: {Min}, {nameof(Max)}: {Max}";
		}
	}
}
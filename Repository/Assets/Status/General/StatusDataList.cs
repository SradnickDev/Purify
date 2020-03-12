using System.Collections.Generic;
using MessagePack;

namespace Status.General {
	[MessagePackObject(true)]
	public class StatusDataList
	{
		public List<StatusData> Status = new List<StatusData>();
	}
}
using System.Collections.Generic;
using Status.Types;

namespace Status.General
{
	public class StatusComparer : IEqualityComparer<StatusBase>
	{
		public bool Equals(StatusBase x, StatusBase y)
		{
			return x?.StatusData.Name == y?.StatusData.Name;
		}

		public int GetHashCode(StatusBase obj)
		{
			return obj.StatusData.Name.GetHashCode();
		}
	}
}
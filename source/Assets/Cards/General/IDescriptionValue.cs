using Units.General;

namespace Cards.General
{
	public interface IDescriptionValue
	{
		object Value(Unit from, Unit target);
	}
}
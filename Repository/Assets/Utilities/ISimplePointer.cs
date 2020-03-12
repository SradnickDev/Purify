namespace Utilities
{
	public interface ISimplePointer
	{
		bool IsActive { get; set; }
		void OnEnter();
		void OnExit();
	}
}
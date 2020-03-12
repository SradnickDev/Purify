using System;

namespace Editors.CardEditor
{
	public class ButtonAction : FieldAction
	{
		public readonly Action Action;

		public ButtonAction(string name, Action action) : base(name)
		{
			Action = action;
		}
	}
}
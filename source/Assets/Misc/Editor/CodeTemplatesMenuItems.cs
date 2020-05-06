using UnityEditor;

namespace Misc.Editor
{
	public class CodeTemplatesMenuItems
	{
		private const string MenuItemPath = "Assets/Create/";

		private const int MenuItemPriority = 70;

		[MenuItem(MenuItemPath + "Card Effect Class", false, MenuItemPriority)]
		private static void CreateClass()
		{
			CodeTemplates.CreateFromTemplate(
											 "NewCardEffect.cs",
											 @"Assets/General/Editor/Template/CardEffectTemplate.txt");
		}

		[MenuItem(MenuItemPath + "Display Status Effect Class", false, MenuItemPriority)]
		private static void CreateDisplayStatus()
		{
			CodeTemplates.CreateFromTemplate(
											 "DisplayStatusEffect.cs",
											 @"Assets/General/Editor/Template/DisplayStatusEffectTemplate.txt");
		}

		[MenuItem(MenuItemPath + "Counter Status Effect Class", false, MenuItemPriority)]
		private static void CreateCounterStatus()
		{
			CodeTemplates.CreateFromTemplate(
											 "CounterStatusEffect.cs",
											 @"Assets/General/Editor/Template/CounterStatusEffectTemplate.txt");
		}

		[MenuItem(MenuItemPath + "Triggered Status Effect Class", false, MenuItemPriority)]
		private static void CreateTriggeredStatus()
		{
			CodeTemplates.CreateFromTemplate(
											 "TriggeredStatusEffect.cs",
											 @"Assets/General/Editor/Template/TriggeredStatusEffectTemplate.txt");
		}
	}
}
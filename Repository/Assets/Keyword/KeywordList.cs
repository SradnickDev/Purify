using System.Collections.Generic;
using UnityEngine;

namespace Keyword
{
	[CreateAssetMenu]
	public class KeywordList : ScriptableObject
	{
		[Drawer.ReadOnly] public string Tag = "[n] where n = index";
		public List<KeywordContainer> Keywords = new List<KeywordContainer>();

		public KeywordContainer GetKeywordByIndex(int index)
		{
			return Keywords.Count >= index ? Keywords[index] : null;
		}
	}
}
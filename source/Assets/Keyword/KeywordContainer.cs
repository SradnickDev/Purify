using System;

namespace Keyword
{
	/// <summary>
	/// Keyword data that would be parsed.
	/// </summary>
	[Serializable]
	public class KeywordContainer
	{
		[NonSerialized]
		public string Tag;

		public string Keyword;

		public string Description;

		public KeywordContainer()
		{
			Keyword = "";
			Description = "";
			Tag = "";
		}

		public KeywordContainer(KeywordContainer other)
		{
			Keyword = other.Keyword;
			Description = other.Description;
			Tag = other.Tag;
		}

		public override string ToString()
		{
			return $"{Keyword} : {Description}";
		}
	}
}
using System.Collections.Generic;
using UnityEngine;

namespace Keyword
{
	[System.Serializable]
	public class KeywordParser
	{
		public KeywordList KeywordList;
		public List<KeywordContainer> ParsedKeywords => m_parsedKeywords;
		private List<KeywordContainer> m_parsedKeywords;

		private const int MaxDepth = 3;

		public string Parse(string text)
		{
			m_parsedKeywords = new List<KeywordContainer>();
			var retVal = ParseKeywords(text);
			ParseNested();
			return retVal;
		}

		/// <summary>
		/// Iterate through each char of the text to find matching sign. Will be stored as a Keyword.
		/// </summary>
		/// <param name="text">Text that will be check.</param>
		/// <returns>new parsed Text</returns>
		private string ParseKeywords(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				Debug.LogError("String to parse it empty!");
				return "";
			}

			var content = "";
			for (var i = 0; i < text.Length; i++)
			{
				var c = text[i];
				if (c == '[')
				{
					for (var j = i; j < text.Length; j++)
					{
						c = text[j];
						content += c;
						if (c == ']')
						{
							i = j;

							TrimKeyword(content);
							content = "";
							break;
						}
					}
				}
			}

			return m_parsedKeywords.Count == 0
				? text
				: Replace(text);
		}

		/// <summary>
		/// Trim to a proper code that can later be replaced.
		/// </summary>
		/// <param name="content"></param>
		private void TrimKeyword(string content)
		{
			var value = content.Remove(0, 1);
			var index = int.Parse(value.Remove(value.Length - 1, 1));
			var matchingKeyword = KeywordList.GetKeywordByIndex(index);

			if (matchingKeyword == null)
			{
				Debug.LogError($"Keyword with tag <b>{content}</b> not found !");
				matchingKeyword = new KeywordContainer
				{
					Keyword = "Missing",
					Description = "Missing"
				};
			}

			var newKeyword = new KeywordContainer(matchingKeyword)
			{
				Tag = content
			};

			m_parsedKeywords.Add(newKeyword);
		}

		/// <summary>
		/// Replace code with Keyword from list.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private string Replace(string text)
		{
			var newDescription = text;
			foreach (var keyword in m_parsedKeywords)
			{
				newDescription = newDescription
					.Replace
						(
						 keyword.Tag, keyword.Keyword
						);
			}

			return newDescription;
		}

		/// <summary>
		/// Check for nested code.
		/// </summary>
		private void ParseNested()
		{
			var lastIdx = 0;
			for (var i = 0; i < MaxDepth; i++)
			{
				var secureCopy = new List<KeywordContainer>(m_parsedKeywords);
				for (var j = lastIdx; j < secureCopy.Count; j++)
				{
					var container = secureCopy[j];
					container.Description = ParseKeywords(container.Description);
				}

				lastIdx = secureCopy.Count;
			}
		}

		public void Reset()
		{
			m_parsedKeywords = new List<KeywordContainer>();
		}
	}
}
using System.Collections.Generic;
using Units.General;
using UnityEngine;

namespace Cards.General
{
	public static class DescriptionParser
	{
		private class ParserValue
		{
			public string Content;

			public int KeyIndex
			{
				get
				{
					var t = Content.Split(',');
					return int.Parse(t[0].Remove(0, 1));
				}
			}

			public int ValueIndex
			{
				get
				{
					var t = Content.Split(',');
					return int.Parse(t[1].Remove(t.Length - 1));
				}
			}

			public override string ToString()
			{
				return $"{KeyIndex} : {ValueIndex}";
			}
		}

		public static string Parse(string description,
								   Unit unit,
								   Unit target,
								   params List<IDescriptionValue>[] values)
		{
			if (string.IsNullOrEmpty(description)) return "";

			var parserValues = new List<ParserValue>();
			var content = "";
			for (var i = 0; i < description.Length; i++)
			{
				var c = description[i];
				if (c == '{')
				{
					var newValue = new ParserValue();

					for (var j = i; j < description.Length; j++)
					{
						c = description[j];
						content += c;
						if (c == '}')
						{
							i = j;
							newValue.Content = content;
							parserValues.Add(newValue);
							content = "";
							break;
						}
					}
				}
			}

			return parserValues.Count == 0
				? description
				: Replace(description, unit, target, parserValues, values);
		}

		private static string Replace(string description,
									  Unit unit,
									  Unit target,
									  IEnumerable<ParserValue> parserValues,
									  params List<IDescriptionValue>[] descriptionValues)
		{
			var newDescription = description;
			foreach (var descriptionValue in parserValues)
			{
				var value = "0";

				if (descriptionValues.Length >= descriptionValue.KeyIndex + 1 &&
					descriptionValues[descriptionValue.KeyIndex].Count >=
					descriptionValue.ValueIndex + 1)
				{
					value =
						descriptionValues[descriptionValue.KeyIndex][descriptionValue.ValueIndex]
							.Value(unit, target).ToString();
				}

				newDescription = newDescription
					.Replace
						(
						 descriptionValue.Content, value
						);
			}

			return newDescription;
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cards.General;
using MessagePack;
using OptionMenu;
using Stats.General;
using Stats.Types;
using Units.Enemy.General;
using Units.General;
using Units.Player.General;
using UnityEngine;
using Random = System.Random;

namespace Utilities
{
	public static class GeneralExtensions
	{
#region Transform

		public static void Reset(this Transform trf, TransformCache trfCache)
		{
			trfCache.Set(trf);
		}

		public static void Save(this Transform trf, TransformCache trfCache)
		{
			trfCache.Save(trf);
		}

		public static T Has<T>(this GameObject gameObject)
		{
			return gameObject.GetComponent<T>();
		}

		public static IEnumerable<T> HasComponents<T>(this GameObject gameObject)
		{
			foreach (var component in gameObject.GetComponents<T>())
			{
				yield return component;
			}
		}

#endregion

#region List

		public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
		{
			T temp = list[indexA];
			list[indexA] = list[indexB];
			list[indexB] = temp;
			return list;
		}

		public static void Shuffle<T>(this List<T> list)
		{
			var count = list.Count;
			for (var i = 0; i < count; i++)
			{
				var r = i + RNG.Next(count - i);
				var t = list[r];
				list[r] = list[i];
				list[i] = t;
			}
		}

		public static T RandomOne<T>(this List<T> list)
		{
			var count = list.Count;
			return list[RNG.Next(count)];
		}

#endregion

#region Math

		public static Vector3 CalculateQuadraticCurve(float t, Vector2 p0, Vector2 p1, Vector2 p2)
		{
			return CalculateQuadraticCurvePoint(t, p0, p1, p2);
		}

		private static Vector3 CalculateQuadraticCurvePoint(float t,
															Vector2 p0,
															Vector2 p1,
															Vector2 p2)
		{
			return (1.0f - t) * (1.0f - t) * p0 +
				   2.0f * (1.0f - t) * t *
				   p1 + t * t * p2;
		}

#endregion

#region Unit

		public static bool IsDead(this Player player)
		{
			return player.Health.Current <= 0 ||
				   player.Soul.Current <= player.Soul.Min;
		}

		public static bool OpponentsDied(this Encounter encounter)
		{
			return encounter.All(opponent => !opponent.IsAlive);
		}

		public static AbstractStat Stat(this Unit unit, Type type)
		{
			if (type == typeof(Defense))
			{
				return unit.Defense;
			}

			if (type == typeof(Health))
			{
				return unit.Health;
			}

			if (type == typeof(Might))
			{
				return unit.Might;
			}

			if (type == typeof(Perseverance))
			{
				return unit.Perseverance;
			}

			if (type == typeof(Soul))
			{
				return unit.Soul;
			}

			if (unit is Player player && type == typeof(Energy))
			{
				return player.Energy;
			}

			return null;
		}

#endregion

		public static string ParseStatusHeader(string header,
											   BuffType buffType,
											   Color buff,
											   Color debuff)
		{
			var color = buffType == BuffType.Buff ? buff : debuff;
			return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{header}</color>";
		}

		public static T DeepCopy<T>(T source)
		{
			if (MessagePackSerializer.DefaultResolver.GetFormatter<T>() == null)
			{
				throw new ArgumentException("This is not registered in default resolver.",
											nameof(source));
			}

			if (ReferenceEquals(source, null))
			{
				return default(T);
			}


			using (var stream = new MemoryStream())
			{
				MessagePackSerializer.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return MessagePackSerializer.Deserialize<T>(stream);
			}
		}
	}
}
using System.IO;
using UnityEngine;

namespace Utilities
{
	public static class Logger
	{
		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Log(string keyword, Object content)
		{
			var msg = $"<color=blue><b>{keyword}</b></color> : {content.ToString()}";
			Log(msg);
		}

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Log(string keyword, object content)
		{
			var msg = $"<color=blue><b>{keyword}</b></color> : {content.ToString()}";
			Log(msg);
		}

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Log(string keyword, string content)
		{
			var msg = $"<color=blue><b>{keyword}</b></color> : {content}";
			Log(msg);
		}

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Log(Color keyColor, string keyword, string content)
		{
			var colorCode = ColorUtility.ToHtmlStringRGB(keyColor);
			var msg = $"<color=#{colorCode}><b>{keyword}</b></color> : {content}";
			Log(msg);
		}

#region FileLog

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void LogF(string keyword, Object content)
		{
			var msg = $"<color=blue><b>{keyword}</b></color> : {content}";
			LogToFile($"{keyword} : {content}");
			Log(msg);
		}

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void LogF(string keyword, object content)
		{
			var msg = $"<color=blue><b>{keyword}</b></color> : {content}";
			LogToFile($"{keyword} : {content}");
			Log(msg);
		}

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void LogF(string keyword, string content)
		{
			var msg = $"<color=blue><b>{keyword}</b></color> : {content}";
			LogToFile($"{keyword} : {content}");
			Log(msg);
		}

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		private static void LogToFile(string msg)
		{
			File.AppendAllText(Application.dataPath + "/log.txt", msg + "\n");
		}

#endregion

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Log(object content)
		{
			Debug.Log(content);
		}

		[System.Diagnostics.Conditional("DEBUG"), System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void Log(string content)
		{
			Debug.Log(content);
		}
	}
}
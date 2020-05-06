using System.IO;
using System.Runtime.Serialization;
using MessagePack;
using UnityEngine;

namespace Utilities
{
	public static class PersistentJson
	{
		/// <summary>
		/// Converts a object to json and saves it to file.
		/// </summary>
		/// <param name="objToSave">Object to convert</param>
		/// <param name="path">Path to save</param>
		/// <param name="key">File name</param>
		/// <typeparam name="T">Object Type</typeparam>
		public static void Save<T>(T objToSave, string path, string key)
		{
			SaveAsJson(objToSave, path, key);
		}

		/// <summary>
		/// Load a file at the given path and convert its to a object.
		/// </summary>
		/// <param name="path">target path</param>
		/// <param name="key">file name</param>
		/// <typeparam name="T">target type</typeparam>
		/// <returns></returns>
		public static T Load<T>(string path, string key)
		{
			return LoadFromJson<T>(path, key);
		}

		/// <summary>
		/// Converts json to a Object
		/// </summary>
		/// <param name="content">Json content</param>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <returns></returns>
		public static T Create<T>(string content) where T : new()
		{
			var bytes = MessagePackSerializer.FromJson(content);
			var retVal = MessagePackSerializer.Deserialize<T>(bytes);
			return retVal;
		}

		private static void SaveAsJson<T>(T objToSave, string path, string key)
		{
			Directory.CreateDirectory(path);
			var fullPath = path + key + ".json";

			try
			{
				var bytes = MessagePackSerializer.Serialize(objToSave);
				var json = MessagePackSerializer.ToJson(bytes);

				using (var writer = new StreamWriter(fullPath))
				{
					writer.Write(json);
				}
			}
			catch (SerializationException exception)
			{
				Debug.Log("Save failed. Error: " + exception.Message);
			}
		}

		private static T LoadFromJson<T>(string path, string key)
		{
			Directory.CreateDirectory(path);
			var fullPath = path + key + ".json";
			var retVal = default(T);

			try
			{
				var content = "";
				using (var writer = new StreamReader(fullPath))
				{
					content = writer.ReadToEnd();
				}

				var bytes = MessagePackSerializer.FromJson(content);
				retVal = MessagePackSerializer.Deserialize<T>(bytes);
			}
			catch (SerializationException exception)
			{
				Debug.Log("Load failed. Error: " + exception.Message);
			}

			return retVal;
		}
	}
}
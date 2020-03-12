using System.IO;
using System.Runtime.Serialization;
using Battle.General;
using MessagePack;
using UnityEngine;

namespace Utilities
{
	public class PersistentData
	{
		private const string Format = ".dat";

		/// <summary>
		/// Serialize the object and save it to a file.
		/// </summary>
		/// <param name="objectToSave">Object to serialize and save.</param>
		/// <param name="key">As an identifier and the file name</param>
		/// <typeparam name="T">Type</typeparam>
		public static void Save<T>(T objectToSave, string key)
		{
			SaveToFile(objectToSave, key);
		}

		/// <summary>
		/// /// Serialize the object and save it to a file.
		/// </summary>
		/// <param name="objectToSave">Object to serialize and save.</param>
		/// <param name="key">As an identifier and the file name</param>
		public static void Save(Object objectToSave, string key)
		{
			SaveToFile(objectToSave, key);
		}

		/// <summary>
		/// Loads data from File and deserialize it to create a new Object.
		/// </summary>
		/// <param name="key">Key to identify the correct file.</param>
		/// <typeparam name="T">Type of object</typeparam>
		/// <returns></returns>
		public static T Load<T>(string key)
		{
			return LoadFromFile<T>(key);
		}

		private static void SaveToFile<T>(T objectToSave, string fileName)
		{
			Directory.CreateDirectory(Constants.PersistentPath);

			var fileStream = new FileStream(Constants.PersistentPath + fileName + Format, FileMode.Create);

			try
			{
				MessagePackSerializer.Serialize(fileStream, objectToSave);
			}
			catch (SerializationException exception)
			{
				Debug.LogError("Save failed. Error: " + exception.Message);
			}
			finally
			{
				fileStream.Close();
			}
		}

		private static T LoadFromFile<T>(string key)
		{
			if (!File.Exists(Constants.PersistentPath + key + Format) || !Directory.Exists(Constants.PersistentPath))
				return default(T);

			var fileStream = new FileStream(Constants.PersistentPath + key + Format, FileMode.Open);

			var returnValue = default(T);

			try
			{
				returnValue = MessagePackSerializer.Deserialize<T>(fileStream);
			}
			catch (SerializationException exception)
			{
				Debug.LogError("Load failed. Error: " + exception.Message);
			}
			finally
			{
				fileStream.Close();
			}

			return returnValue;
		}
	}
}
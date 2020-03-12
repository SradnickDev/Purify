using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Misc.Editor
{
	public class DoCreateCodeFile : EndNameEditAction
	{
		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			Object o = CodeTemplates.CreateScript(pathName, resourceFile);
			ProjectWindowUtil.ShowCreatedAsset(o);
		}
	}

	/// <summary>
	/// Editor class for creating code files from templates.
	/// </summary>
	public class CodeTemplates
	{
		private static readonly Texture2D ScriptIcon =
			(EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);

		internal static Object CreateScript(string pathName, string templatePath)
		{
			var newFilePath = Path.GetFullPath(pathName);
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
			var className = NormalizeClassName(fileNameWithoutExtension);

			if (File.Exists(templatePath))
			{
				string templateText;
				using (var sr = new StreamReader(templatePath))
				{
					templateText = sr.ReadToEnd();
				}

				templateText = templateText.Replace("##NAME##", className);

				UTF8Encoding encoding = new UTF8Encoding(true, false);

				using (var sw = new StreamWriter(newFilePath, false, encoding))
				{
					sw.Write(templateText);
				}

				AssetDatabase.ImportAsset(pathName);

				return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
			}
			else
			{
				Debug.LogError($"The template file was not found: {templatePath}");

				return null;
			}
		}

		private static string NormalizeClassName(string fileName)
		{
			return fileName.Replace(" ", string.Empty);
		}

		public static void CreateFromTemplate(string initialName, string templatePath)
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists
				(
				 0,
				 ScriptableObject.CreateInstance<DoCreateCodeFile>(),
				 initialName,
				 ScriptIcon,
				 templatePath
				);
		}
	}
}
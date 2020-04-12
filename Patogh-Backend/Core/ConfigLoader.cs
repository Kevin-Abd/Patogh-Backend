namespace PatoghBackend.Core
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.IO;
	using Jil;

	public static class ConfigLoader
	{
		public static readonly string ConfigPath = "./Config/";

		private const string Separator = "|#|";

		public static string GetPath(string folder)
		{
			var path = System.Reflection.Assembly.GetExecutingAssembly().Location;

			if (Directory.Exists(path.Substring(0, path.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase)) + $"\\{folder}\\"))
			{
				return path.Substring(0, path.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase)) + $"\\{folder}\\";
			}

			if (path.IndexOf("\\bin\\", StringComparison.OrdinalIgnoreCase) == -1)
			{
				throw new DirectoryNotFoundException($"{folder} folder:{path}");
			}

			return path.Substring(0, path.LastIndexOf("\\bin\\", StringComparison.OrdinalIgnoreCase)) + $"\\{folder}\\";
		}

		public static string GetConnectionString(string name)
		{
			return KeyValueFileGetValue("ConnectionString.txt", name);
		}

		public static ListDictionary KeyValueFileToListDictionary(string filename)
		{
			Check(filename);
			var configs = File.ReadAllLines(ConfigPath + filename);
			var result = new ListDictionary();
			foreach (var line in configs)
			{
				if (line.Length == 0)
				{
					continue;
				}

				var keyValue = line.Split(Separator);
				if (keyValue.Length != 2)
				{
					throw new Exception($"Invalid {filename} File,line: {line}");
				}

				result.Add(keyValue[0], keyValue[1]);
			}

			return result;
		}

		public static Dictionary<string, string> KeyValueFileToDictionary(string filename)
		{
			Check(filename);
			var configs = File.ReadAllLines(ConfigPath + filename);
			var result = new Dictionary<string, string>();
			foreach (var line in configs)
			{
				if (line.Length == 0)
				{
					continue;
				}

				var keyValue = line.Split(Separator);
				if (keyValue.Length != 2)
				{
					throw new Exception($"Invalid {filename} File,line: {line}");
				}

				result.Add(keyValue[0], keyValue[1]);
			}

			return result;
		}

		public static string KeyValueFileGetValue(string filename, string key)
		{
			Check(filename);
			var configs = File.ReadAllLines(ConfigPath + filename);
			foreach (var line in configs)
			{
				if (line.StartsWith(key + Separator, StringComparison.Ordinal))
				{
					return line.Substring(line.IndexOf(Separator, StringComparison.Ordinal) + 3);
				}
			}

			throw new Exception($"Invalid {filename} File,key: {key}");
		}

		private static void Check(string filename)
		{
			if (!File.Exists(ConfigPath + filename))
			{
				throw new FileNotFoundException(ConfigPath + filename);
			}
		}

		public static class Jil
		{
			private static readonly Options JilOptions = new Options(
			dateFormat: DateTimeFormat.ISO8601,
			prettyPrint: false,
			excludeNulls: false,
			jsonp: false,
			includeInherited: true,
			serializationNameFormat: SerializationNameFormat.CamelCase);

			public static string Serialize(object input)
			{
				return JSON.Serialize(input, JilOptions);
			}

			public static object DeSerialize(TextReader reader)
			{
				return JSON.Deserialize<object>(reader, JilOptions);
			}

			public static T DeSerialize<T>(TextReader reader)
			{
				return JSON.Deserialize<T>(reader, JilOptions);
			}

			public static T DeSerialize<T>(string path)
			{
				return JSON.Deserialize<T>(File.OpenText(path), JilOptions);
			}

			public static dynamic DeserializeDynamic(TextReader reader)
			{
				return JSON.DeserializeDynamic(reader, JilOptions);
			}
		}
	}
}
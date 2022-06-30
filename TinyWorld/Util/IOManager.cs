using System.IO;
using System.Text.Json;

namespace TinyWorld.Util {

	internal static class IOManager {

		public static async void Save (object obj, string name) {
			await using FileStream createStream = File.Create(name);
			await JsonSerializer.SerializeAsync(createStream, obj);
			await createStream.DisposeAsync();
		}

		public static T? Load<T> (string name) {
			if (!File.Exists(name)) {
				return default;
			}

			string file = File.ReadAllText(name);
			T? deserialized = JsonSerializer.Deserialize<T>(file);

			return deserialized;
		}

	}

}
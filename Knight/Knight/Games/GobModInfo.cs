using MZZT.FileFormats;
using System.Reflection;
using System.Text;

namespace MZZT.Knight.Games {
	public class GobModInfo {
		public static async Task<GobModInfo> GetModInfo(Stream stream) {
			JkGob gob = await JkGob.TryReadAsync(stream);
			if (gob == null) {
				return null;
			}

			JkGob.GobItemInformation file = gob.FindFile("patchinfo.txt");
			if (file.Offset == 0) {
				return null;
			}

			gob.MoveToStartOfFile(stream, file);
			byte[] data = new byte[file.Length];
			stream.Read(data, 0, file.Length);
			string patchinfo = Encoding.UTF8.GetString(data);
			data = null;

			GobModInfo ret = new();
			Type type = typeof(GobModInfo);

			foreach (KeyValuePair<string, string> line in patchinfo
				.Split('\n', '\r')
				.Where(x => !string.IsNullOrWhiteSpace(x) && x.Contains('='))
				.Select(x => {
					int index = x.IndexOf('=');
					return new KeyValuePair<string, string>(x[..index].Trim(),
						x[(index + 1)..].Trim());
				})) {

				PropertyInfo property = type.GetProperty(line.Key,
					BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
				if (property == null) {
					continue;
				}

				property.SetValue(ret, line.Value);
			}

			return ret;
		}

		private GobModInfo() { }

		public string Name { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public string Email { get; set; }
	}
}

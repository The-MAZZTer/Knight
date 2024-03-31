using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MZZT.FileFormats {
	public class Ini : File<Ini> {
		public override async Task LoadAsync(Stream stream) {
			await base.LoadAsync(stream);

			using StreamReader reader = new StreamReader(stream);
			string line;
			string currentHeader = null;

			while ((line = (await reader.ReadLineAsync())?.Trim()) != null) {
				if (string.IsNullOrEmpty(line) || line.StartsWith(";")) {
					continue;
				}

				if (line.StartsWith("[")) {
					if (!line.EndsWith("]")) {
						throw new FormatException();
					}

					currentHeader = line[1..^1];
					if (!this.Data.ContainsKey(currentHeader)) {
						this.Data[currentHeader] = new Dictionary<string, string>();
					}
					continue;
				}

				int index = line.IndexOf("=");
				if (index == -1) {
					throw new FormatException();
				}

				if (currentHeader == null) {
					throw new FormatException();
				}

				this.Data[currentHeader][line[..index]] = line[(index + 1)..];
			}
		}

		public Dictionary<string, Dictionary<string, string>> Data { get; } = new Dictionary<string, Dictionary<string, string>>();

		public string GetValue(string section, string key) => this.Data[section][key];

		public void SetValue(string section, string key, string value) {
			if (!this.Data.ContainsKey(section)) {
				this.Data[section] = new Dictionary<string, string>();
			}
			this.Data[section][key] = value;
		}

		public async Task Save(Stream stream, Encoding encoding = null) {
			using StreamWriter writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);
			foreach (string section in this.Data.Keys) {
				await writer.WriteLineAsync(string.Format("[{0}]", section));
				foreach (string key in this.Data[section].Keys) {
					await writer.WriteLineAsync(string.Format("{0}={1}", key, this.Data[section][key]));
				}
				await writer.WriteLineAsync();
			}
		}
	}
}

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MZZT.Settings {
	public abstract class Settings<T> where T : Settings<T>, new() {
		public static T Load(Stream stream, bool createDefaultOnException = false) {
			T ret;
			try {
				ret = JsonSerializer.Deserialize<T>(stream);
			} catch (Exception) {
				if (createDefaultOnException) {
					ret = new T();
				} else {
					throw;
				}
			}
			return ret;
		}

		public static T Load(string path, bool createDefaultOnException = false) {
			T ret;
			try {
				using FileStream stream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read);
				ret = Load(stream, createDefaultOnException);
			} catch (Exception) {
				if (createDefaultOnException) {
					ret = new T();
				} else {
					throw;
				}
			}
			ret.FilePath = path;
			return ret;
		}

		public void Save(Stream stream) {
			JsonSerializer.Serialize(stream, (T)this);
		}

		public void Save(string path = null) {
			path ??= this.FilePath;

			using FileStream stream = new(path, FileMode.Create, FileAccess.Write, FileShare.None);
			this.Save(stream);

			this.FilePath = path;
		}

		public static void SaveDefaults(string path) {
			using FileStream stream = new(path, FileMode.Create, FileAccess.Write, FileShare.None);
			JsonSerializer.Serialize(stream, new T());
		}

		[JsonIgnore, Browsable(false)]
		public string FilePath { get; private set; }
	}
}
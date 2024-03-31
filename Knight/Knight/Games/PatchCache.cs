using MZZT.Properties;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MZZT.Knight.Games {
	public class PatchCache {
		private static async Task<PatchCache> LoadDefaults() {
			PatchCache patchCache;
			using (MemoryStream stream = new(Resources.patches_defaults)) {
				patchCache = await JsonSerializer.DeserializeAsync<PatchCache>(stream);
			}
			foreach (string key in patchCache.Patches.Keys) {
				patchCache.Patches[key].FileName = key;
			}
			return patchCache;
		}

		public static async Task<PatchCache> Load() {
			if (!File.Exists(FilePath)) {
				PatchCache cache = await LoadDefaults();
				await cache.OnSave();
				return cache;
			}

			PatchCache patchCache;
			using (FileStream file = new(FilePath, FileMode.Open, FileAccess.Read,
				FileShare.Read)) {

				patchCache = await JsonSerializer.DeserializeAsync<PatchCache>(file);
			}

			foreach (string key in patchCache.Patches.Keys) {
				patchCache.Patches[key].FileName = key;
			}
			return patchCache;
		}

		public static string FilePath {
			get {
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
				return Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					versionInfo.CompanyName, versionInfo.ProductName, "patches.json");
			}
		}

		private async Task OnSave() {
			string path = FilePath;
			DirectoryInfo folder = Directory.GetParent(path);
			if (!folder.Exists) {
				folder.Create();
			}

			using (FileStream file = new(path, FileMode.Create, FileAccess.Write,
				FileShare.None)) {

				await JsonSerializer.SerializeAsync(file, this);
			}
			lock (this) {
				this.saveTask = null;
			}
		}

		private Task saveTask;
		public void Save() {
			lock (this) {
				if (this.saveTask != null) {
					this.saveTask.ContinueWith(x => this.Save());
					return;
				}
				this.saveTask = Task.Run(this.OnSave);
			}
		}

		public async Task Wait() {
			Task task;
			lock (this) {
				task = this.saveTask;
			}
			await task;
		}

		[JsonPropertyName("patches")]
		public Dictionary<string, PatchInfo> Patches {
			get; set;
		} = [];

		private static readonly SHA1 hasher = SHA1.Create();
		public static PatchInfo HashFile(string name, string filename) {
			using FileStream stream = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			PatchInfo patch = new() {
				Size = stream.Length,
				FileName = filename,
				Name = name
			};
			lock (hasher) {
				patch.Hash = hasher.ComputeHash(stream);
			}
			return patch;
		}

		public PatchInfo IdentifyFile(byte[] hash, long size) {
			lock (this) {
				return this.Patches.Values
					.FirstOrDefault(x => x.Size == size && x.Hash.SequenceEqual(hash));
			}
		}

		public PatchInfo IdentifyFile(string filename) {
			byte[] hash = HashFile(null, filename).Hash;
			long size = new FileInfo(filename).Length;
			return this.IdentifyFile(hash, size);
		}

		public void SetPatch(PatchInfo info) {
			lock (this) {
				this.Patches[info.FileName] = info;
			}
		}

		public bool HasPatch(string id) {
			lock (this) {
				return this.Patches.ContainsKey(id);
			}
		}
	}

	public class PatchInfo {
		[JsonPropertyName("name")]
		public string Name {
			get; set;
		} = null;

		[JsonIgnore]
		public string FileName {
			get; set;
		} = null;

		[JsonPropertyName("size")]
		public long Size {
			get; set;
		} = 0;

		[JsonPropertyName("hash")]
		public byte[] Hash {
			get; set;
		} = null;
	}
}

using MZZT.Properties;
using System.Collections;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MZZT.Knight.Games {
	public class ModCache {
		private static async Task<ModCache> LoadDefaults() {
			ModCache modCache;
			using (MemoryStream stream = new(Resources.mods_defaults)) {
				modCache = await JsonSerializer.DeserializeAsync<ModCache>(stream);
			}
			foreach (string key in modCache.DarkForcesMods.Keys) {
				modCache.DarkForcesMods[key].Id = key;
			}
			foreach (string key in modCache.JediKnightMods.Keys) {
				modCache.JediKnightMods[key].Id = key;
			}
			foreach (string key in modCache.MysteriesOfTheSithMods.Keys) {
				modCache.MysteriesOfTheSithMods[key].Id = key;
			}
			foreach (string key in modCache.JediOutcastMods.Keys) {
				modCache.JediOutcastMods[key].Id = key;
			}
			foreach (string key in modCache.JediAcademyMods.Keys) {
				modCache.JediAcademyMods[key].Id = key;
			}
			return modCache;
		}

		public static async Task<ModCache> Load() {
			if (!File.Exists(FilePath)) {
				ModCache cache = await LoadDefaults();
				await cache.OnSave();
				return cache;
			}

			ModCache modCache;
			using (FileStream file = new(FilePath, FileMode.Open, FileAccess.Read,
				FileShare.Read)) {

				modCache = await JsonSerializer.DeserializeAsync<ModCache>(file);
			}

			foreach (string key in modCache.DarkForcesMods.Keys) {
				modCache.DarkForcesMods[key].Id = key;
			}
			foreach (string key in modCache.JediKnightMods.Keys) {
				modCache.JediKnightMods[key].Id = key;
			}
			foreach (string key in modCache.MysteriesOfTheSithMods.Keys) {
				modCache.MysteriesOfTheSithMods[key].Id = key;
			}
			foreach (string key in modCache.JediOutcastMods.Keys) {
				modCache.JediOutcastMods[key].Id = key;
			}
			foreach (string key in modCache.JediAcademyMods.Keys) {
				modCache.JediAcademyMods[key].Id = key;
			}
			return modCache;
		}

		public static string FilePath {
			get {
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
				return Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					versionInfo.CompanyName, versionInfo.ProductName, "mods.json");
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
					this.saveTask = this.saveTask.ContinueWith(x => this.Save());
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

		[JsonPropertyName("darkForces")]
		public Dictionary<string, DarkForcesModInfo> DarkForcesMods { get; set; } = [];

		[JsonPropertyName("jediKnight")]
		public Dictionary<string, ModInfo> JediKnightMods { get; set; } = [];

		[JsonPropertyName("mysteriesOfTheSith")]
		public Dictionary<string, ModInfo> MysteriesOfTheSithMods { get; set; } = [];

		[JsonPropertyName("jediOutcast")]
		public Dictionary<string, Quake3ModInfo> JediOutcastMods { get; set; } = [];

		[JsonPropertyName("jediAcademy")]
		public Dictionary<string, Quake3ModInfo> JediAcademyMods { get; set; } = [];

		public IDictionary GetModInfos(SupportedGames game) {
			IDictionary dictionary = null;
			switch (game) {
				case SupportedGames.DarkForces:
					dictionary = this.DarkForcesMods;
					break;
				case SupportedGames.JediKnight:
					dictionary = this.JediKnightMods;
					break;
				case SupportedGames.MysteriesOfTheSith:
					dictionary = this.MysteriesOfTheSithMods;
					break;
				case SupportedGames.JediOutcast:
					dictionary = this.JediOutcastMods;
					break;
				case SupportedGames.JediAcademy:
					dictionary = this.JediAcademyMods;
					break;
			}
			return dictionary;
		}

		private string GetCaseInsensitiveModId(SupportedGames game, string modId) {
			IDictionary dictionary = this.GetModInfos(game);
			string lowerMod = modId.ToLower();
			return dictionary.Keys.Cast<string>().FirstOrDefault(x => x.Equals(lowerMod, StringComparison.CurrentCultureIgnoreCase));
		}

		public ModInfo GetModInfo(SupportedGames game, string modId) {
			IDictionary dictionary = this.GetModInfos(game);
			lock (this) {
				string key = this.GetCaseInsensitiveModId(game, modId);
				if (key == null) {
					return null;
				}
				return dictionary[key] as ModInfo;
			}
		}

		public void SetModInfo(SupportedGames game, ModInfo modInfo) {
			lock (this) {
				string key = this.GetCaseInsensitiveModId(game, modInfo.Id) ?? modInfo.Id;
				this.GetModInfos(game)[key] = modInfo;
			}
		}
	}

	public class ModInfo {
		[JsonIgnore]
		public string Id { get; set; } = null;

		[JsonPropertyName("name")]
		public string Name { get; set; } = null;
	}

	public class DarkForcesModInfo : ModInfo {
		[JsonPropertyName("lfd")]
		public string LfdFile { get; set; } = null;

		[JsonPropertyName("crawl")]
		public string CrawlFile { get; set; } = null;

		[JsonPropertyName("others")]
		public string[] OtherFiles { get; set; } = [];
	}

	public class Quake3ModInfo : ModInfo {
		[JsonPropertyName("singleplayer")]
		public bool SinglePlayer { get; set; } = false;
	}
}

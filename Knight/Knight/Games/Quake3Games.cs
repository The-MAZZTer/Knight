using MZZT.Knight.Forms;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using DProcess = System.Diagnostics.Process;

namespace MZZT.Knight.Games {
	public abstract class Quake3Game(SupportedGames game) : WindowsLecGame(game) {
		protected new Quake3Settings Settings => base.Settings as Quake3Settings;

		protected abstract string SinglePlayerExeFileName { get; }
		protected abstract string MultiplayerExeFileName { get; }

		public override bool IsValidGamePath(string folder) {
			if (string.IsNullOrWhiteSpace(folder)) {
				return false;
			}
			return File.Exists(Path.Combine(folder, "GameData", $"{this.SinglePlayerExeFileName}.exe"));
		}

		public override string CustomModPath {
			get => base.CustomModPath;
			set => throw new NotSupportedException($"Can't set ModPath for {this.Name}!");
		}
		public override string AutoModPath => Path.Combine(this.GamePath, "GameData");

		public override string CustomActiveModPath {
			get => base.CustomActiveModPath;
			set => throw new NotSupportedException($"Can't set ActiveModPath for {this.Name}!");
		}
		public override string AutoActiveModPath => this.AutoModPath;

		protected override async Task OnFindModsAsync(CancellationToken token) {
			if (!Directory.Exists(this.ModPath)) {
				return;
			}

			foreach (string file in Directory.EnumerateFiles(this.ModPath, "*.zip")) {
				if (token.IsCancellationRequested) {
					return;
				}

				string id = Path.GetFileNameWithoutExtension(file);
				if (this.Mods.ContainsKey(id)) {
					continue;
				}

				bool isMod = false;
				ZipArchive zip;
				try {
					zip = ZipFile.OpenRead(file);
				} catch (Exception ex) {
					Debug.WriteLine(ex);
					continue;
				}
				using (zip) {
					foreach (ZipArchiveEntry entry in zip.Entries) {
						string name = entry.Name.ToLower();
						if (name.EndsWith(".pk3") || name.EndsWith(".dll")) {
							isMod = true;
							break;
						}
					}
				}

				if (!isMod) {
					continue;
				}

				await this.OnModFoundAsync(new Quake3ZippedMod(this, id), token);
			}

			foreach (string dir in Directory.EnumerateDirectories(this.ModPath)) {
				if (token.IsCancellationRequested) {
					return;
				}
				await Task.Yield();

				string name = Path.GetFileName(dir);
				if (name.Equals("base", StringComparison.CurrentCultureIgnoreCase)) {
					continue;
				}

				if (this.Mods.ContainsKey(name)) {
					continue;
				}

				if (!Directory.EnumerateFiles(dir, "*.pk3").Any() &&
					!Directory.EnumerateFiles(dir, "*.dll").Any()) {

					continue;
				}

				await this.OnModFoundAsync(new Quake3Mod(this, name), token);
			}
		}

		public override bool AllowMultipleActiveMods => false;

		public override bool SupportsPatches => false;

		protected string GetArguments(string modId) {
			StringBuilder args = new();
			if (modId != null) {
				args.Append($"+set fs_game \"{modId}\" ");
			}

			if (this.Settings.Commands != null) {
				args.Append($"+{this.Settings.Commands} ");
			}

			return args.ToString().TrimEnd();
		}

		public override void Run(IEnumerable<Mod> mods) {
			string exe = Path.Combine(this.GamePath, "GameData", $"{this.SinglePlayerExeFileName}.exe");
			if (!File.Exists(exe)) {
				throw new FileNotFoundException($"Unable to locate {this.Name} Single Player!");
			}

			DProcess.Start(new ProcessStartInfo() {
				FileName = exe,
				WorkingDirectory = Path.GetDirectoryName(exe),
				Arguments = this.GetArguments(mods.FirstOrDefault()?.Id)
			});
		}

		public override bool HasSeparateMultiplayerBinary => true;

		public override void RunMultiplayer(IEnumerable<Mod> mods) {
			string exe = Path.Combine(this.GamePath, "GameData", $"{this.MultiplayerExeFileName}.exe");
			if (!File.Exists(exe)) {
				throw new FileNotFoundException($"Unable to locate {this.Name} Multiplayer!");
			}

			DProcess.Start(new ProcessStartInfo() {
				FileName = exe,
				WorkingDirectory = Path.GetDirectoryName(exe),
				Arguments = this.GetArguments(mods.FirstOrDefault()?.Id)
			});
		}

		public override GameOptions OptionsForm => new Quake3Options(this);
	}

	public class JediOutcast : Quake3Game {
		public JediOutcast() : base(SupportedGames.JediOutcast) { }

		protected override SteamHelper.AppIds AppId => SteamHelper.AppIds.JediOutcast;
		protected override string GameRegistryPath =>
			@"HKEY_LOCAL_MACHINE\SOFTWARE\LucasArts Entertainment Company LLC\Star Wars JK II Jedi Outcast\1.0";
		protected override string SteamGameFolder => "Jedi Outcast";

		protected override string SinglePlayerExeFileName => "jk2sp";
		protected override string MultiplayerExeFileName => "jk2mp";
	}

	public class JediAcademy : Quake3Game {
		public JediAcademy() : base(SupportedGames.JediAcademy) { }

		protected override SteamHelper.AppIds AppId => SteamHelper.AppIds.JediAcademy;
		protected override string GameRegistryPath =>
			@"HKEY_LOCAL_MACHINE\SOFTWARE\LucasArts\Star Wars Jedi Knight Jedi Academy\1.0";
		protected override string SteamGameFolder => "Jedi Academy";

		protected override string SinglePlayerExeFileName => "jasp";
		protected override string MultiplayerExeFileName => "jamp";
	}

	public abstract class Quake3ModBase(Game owner, string id) : Mod<Quake3ModInfo>(owner, id) {
		private static readonly Regex stripColors = new(@"\^((\^)|.)");
		protected async Task<string> GetName(Stream stream) {
			string name = null;
			using (StreamReader reader = new(stream)) {
				if (!reader.EndOfStream) {
					name = (await reader.ReadLineAsync()).Trim();
					name = stripColors.Replace(name, "$2");
				}
			}
			return name;
		}

		public override bool HasProperties => true;
		public override Form PropertiesForm => null; // new Quake3ModProperties(this);
	}

	public class Quake3Mod(Game owner, string id) : Quake3ModBase(owner, id) {
		private static readonly Regex stripColors = new(@"\^((\^)|.)");
		public override async Task RefreshCacheInfoAsync() {
			string description = Path.Combine(this.Game.ModPath, this.Id, "description.txt");
			string name = this.Id;
			if (File.Exists(description)) {
				using (FileStream stream = new(description, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					name = await this.GetName(stream);
				}

				if (string.IsNullOrWhiteSpace(name)) {
					name = this.Id;
				}
			}

			Program.ModCache.SetModInfo(this.Game.WhichGame, new Quake3ModInfo() {
				Id = this.Id,
				Name = name
			});
			this.OnCacheChanged();
		}

		public override bool Active => this.active;
		private bool active = false;
		protected override async Task OnActivate() {
			await Task.Yield();
			this.active = true;
		}
		protected override async Task OnDeactivate() {
			await Task.Yield();
			this.active = false;
		}
	}

	public class Quake3ZippedMod(Game owner, string id) : Quake3ModBase(owner, id) {
		private async Task RefreshZippedInfo() {
			string name = this.Id;
			using ZipArchive zip = ZipFile.OpenRead(Path.Combine(this.Game.ModPath, $"{this.Id}.zip"));
			ZipArchiveEntry entry = zip.Entries.FirstOrDefault(x => x.Name.Equals("description.txt", StringComparison.CurrentCultureIgnoreCase));
			if (entry != null) {
				Stream stream = null;
				try {
					stream = entry.Open();
				} catch {
				}
				if (stream != null) {
					using (stream) {
						name = await this.GetName(stream);
					}

					if (string.IsNullOrWhiteSpace(name)) {
						name = this.Id;
					}
				}
			}

			Program.ModCache.SetModInfo(this.Game.WhichGame, new Quake3ModInfo() {
				Id = this.Id,
				Name = name
			});
			this.OnCacheChanged();
		}

		public override async Task RefreshCacheInfoAsync() {
			if (this.Active) {
				await this.RefreshZippedInfo();
				return;
			}

			string description = Path.Combine(this.Game.ModPath, this.Id, "description.txt");
			if (!File.Exists(description)) {
				await this.RefreshZippedInfo();
				return;
			}

			string name;
			using (FileStream stream = new(description, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				name = await this.GetName(stream);
			}

			if (string.IsNullOrWhiteSpace(name)) {
				name = this.Id;
			}

			Program.ModCache.SetModInfo(this.Game.WhichGame, new Quake3ModInfo() {
				Id = this.Id,
				Name = name
			});
			this.OnCacheChanged();
		}

		protected override async Task OnActivate() {
			string zipFile = Path.Combine(this.Game.ModPath, $"{this.Id}.zip");
			if (!File.Exists(zipFile)) {
				throw new FileNotFoundException($"{zipFile} not found when trying to activate.");
			}

			string path = Path.Combine(this.Game.ActiveModPath, this.Id);
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}

			await Task.Run(() => ZipFile.ExtractToDirectory(zipFile, path));
		}

		protected override async Task OnDeactivate() {
			string path = Path.Combine(this.Game.ActiveModPath, this.Id);
			if (Directory.Exists(path)) {
				await Task.Run(() => Directory.Delete(path, true));
			}
		}

		public override bool Active => Directory.Exists(Path.Combine(this.Game.ActiveModPath, this.Id));
	}
}

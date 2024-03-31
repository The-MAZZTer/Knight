using MZZT.Extensions;
using MZZT.Knight.Forms;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using DProcess = System.Diagnostics.Process;

namespace MZZT.Knight.Games {
	public class DarkForces : Game {
		public DarkForces() : base(SupportedGames.DarkForces) { }
		protected new DarkForcesSettings Settings => base.Settings as DarkForcesSettings;

		public override string AutoGamePath {
			get {
				/*if (!SteamHelper.IsAppInstalled(SteamHelper.AppIds.DarkForces)) {
					return null;
				}*/

				string[] libraryFolders = SteamHelper.LibraryFolders;
				foreach (string library in libraryFolders) {
					string gamepath = Path.Combine(library, "steamapps", "common", "STAR WARS Dark Forces Remaster");
					if (this.IsValidGamePath(gamepath)) {
						this.Settings.DarkXlPath = gamepath;
						this.Settings.RunMode = DarkForcesSettings.RunModes.DarkXl;

						return gamepath;
					}
				}
				foreach (string library in libraryFolders) {
					string gamepath = Path.Combine(library, "steamapps", "common", "Dark Forces", "Game");
					if (this.IsValidGamePath(gamepath)) {
						return gamepath;
					}
				}

				return null;
			}
		}

		public override bool IsValidGamePath(string folder) {
			if (string.IsNullOrWhiteSpace(folder)) {
				return false;
			}
			return File.Exists(Path.Combine(folder, "DARK.GOB")) || File.Exists(Path.Combine(folder, "dark.gob"));
		}

		public override string AutoModPath => Path.Combine(this.GamePath, "Levels");

		public override string AutoActiveModPath {
			get {
				if (this.Settings.RunMode == DarkForcesSettings.RunModes.DarkXl) {
					return Path.GetDirectoryName(this.Settings.DarkXlPath);
				}

				return this.GamePath;
			}
		}

		private readonly string[] invalidMods = ["dark", "sounds", "sprites", "textures"];
		protected override async Task OnFindModsAsync(CancellationToken token) {
			string modPath = this.ModPath;
			if (!Directory.Exists(modPath)) {
				return;
			}

			if (File.Exists(Path.Combine(modPath, "dfrntend.exe"))) {
				await this.OnModFoundAsync(new DarkFrontend(this), token);
			}
			
			foreach (string folder in Directory.EnumerateDirectories(modPath)) {
				if (token.IsCancellationRequested) {
					return;
				}
				await Task.Yield();

				//string uid = Path.GetFileNameWithoutExtension(folder);
				string shortPath = PathExtensions.GetShortName(folder);
				string uid = Path.GetFileNameWithoutExtension(shortPath);
				if (!File.Exists(Path.Combine(shortPath, $"{uid}.gob")) ||
					this.Mods.ContainsKey(uid) ||
					invalidMods.Contains(uid.ToLower())) {

					continue;
				}

				await this.OnModFoundAsync(new DarkForcesMod(this, uid), token);
			}

			foreach (string file in Directory.EnumerateFiles(modPath, "*.gob")) {
				if (token.IsCancellationRequested) {
					return;
				}
				await Task.Yield();

				//string uid = Path.GetFileNameWithoutExtension(file);
				string shortPath = PathExtensions.GetShortName(file);
				string uid = Path.GetFileNameWithoutExtension(file);
				if (this.Mods.ContainsKey(uid) ||
					invalidMods.Contains(uid.ToLower())) {

					continue;
				}

				await this.OnModFoundAsync(new DarkForcesMessyMod(this, uid), token);
			}

			foreach (string file in Directory.EnumerateFiles(modPath, "*.zip")) {
				if (token.IsCancellationRequested) {
					return;
				}

				//string uid = Path.GetFileNameWithoutExtension(file);
				string shortPath = PathExtensions.GetShortName(file);
				string uid = Path.GetFileNameWithoutExtension(file);
				if (this.Mods.ContainsKey(uid) ||
					invalidMods.Contains(uid.ToLower())) {

					continue;
				}

				ZipArchive zip;
				try {
					zip = ZipFile.OpenRead(file);
				} catch (Exception ex) {
					Debug.WriteLine(ex);
					continue;
				}
				using (zip) {
					if (!zip.Entries.Any(x => x.Name.Equals($"{uid}.gob", StringComparison.CurrentCultureIgnoreCase))) {
						continue;
					}
				}

				await this.OnModFoundAsync(new DarkForcesZippedMod(this, uid), token);
			}
		}

		public override bool AllowMultipleActiveMods => false;

		public override bool SupportsPatches => false;

		public static char FirstCdDrive {
			get {
				foreach (DriveInfo driveInfo in DriveInfo.GetDrives()) {
					if (driveInfo.DriveType == DriveType.CDRom) {
						return driveInfo.Name[0];
					}
				}

				return (char)0;
			}
		}

		private string GetArguments(string modId = null) {
			StringBuilder args = new();
			if (modId != null) {
				args.Append($"-u{modId}.gob ");
			}
			if (this.Settings.AutoTest) {
				args.Append("-t ");
			}
			if (this.Settings.CdDrive != (char)0) {
				args.Append($"-x{this.Settings.CdDrive} ");
			}
			if (this.Settings.EnableScreenshots) {
				args.Append("-shots ");
			}
			if (this.Settings.LevelSelect != null) {
				args.Append($"-l{this.Settings.LevelSelect} ");
			}
			if (this.Settings.Log) {
				args.Append("-g ");
			}
			if (this.Settings.SkipCutscenes) {
				args.Append("-c ");
			}
			if (this.Settings.SkipFilesCheck) {
				args.Append("-f ");
			}
			if (this.Settings.SkipMemoryCheck) {
				args.Append("-m ");
			}
			return args.ToString().TrimEnd();
		}

		public static bool IsValidDarkXlPath(string path) {
			if (string.IsNullOrWhiteSpace(path)) {
				return false;
			}

			return File.Exists(path);
		}

		public static bool IsValidDosBoxPath(string path) {
			if (string.IsNullOrWhiteSpace(path)) {
				return false;
			}

			return File.Exists(path);
		}

		public override void Run(IEnumerable<Mod> mods) {
			Mod mod = mods.FirstOrDefault();
			if (mod is DarkFrontend) {
				this.RunDarkFrontend();
				return;
			}

			string dark = Path.Combine(this.GamePath, "dark.gob");
			if (!File.Exists(dark)) {
				//throw new FileNotFoundException("Unable to locate Dark Forces!");
				MessageBox.Show("Unable to locate Dark Forces!", "Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ProcessStartInfo startInfo = new();
			switch (this.Settings.RunMode) {
				case DarkForcesSettings.RunModes.Native:
					startInfo.FileName = Path.Combine(this.GamePath, "dark.exe");
					startInfo.Arguments = this.GetArguments(mod?.Id);
					break;
				case DarkForcesSettings.RunModes.DosBox:
					startInfo.FileName = this.Settings.DosBoxPath;
					startInfo.Arguments = $"-noconsole -c \"mount c: '{Path.GetDirectoryName(dark)}'\" ";
					if (!File.Exists(Path.Combine(GamePath, "cd.id"))) {
						startInfo.Arguments += $"-c \"mount d: '{FirstCdDrive}:\'\" ";
					}
					startInfo.Arguments += $"-c \"c:\" -c \"dark.exe {this.GetArguments(mod?.Id)}\" -c \"exit\"";
					break;
				case DarkForcesSettings.RunModes.DarkXl:
					startInfo.FileName = this.Settings.DarkXlPath;
					startInfo.Arguments = this.GetArguments(mod?.Id);
					break;
			}
			startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
			DProcess.Start(startInfo);
		}

		public void RunIMuse() {
			string imuse = Path.Combine(this.GamePath, "imuse.exe");
			if (!File.Exists(imuse)) {
				//throw new FileNotFoundException("Unable to locate Dark Forces iMuse tool!");
				MessageBox.Show("Unable to locate Dark Forces iMuse tool!", "Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ProcessStartInfo startInfo = new();
			if (this.Settings.RunMode == DarkForcesSettings.RunModes.Native ||
				!IsValidDosBoxPath(this.Settings.DosBoxPath)) {

				startInfo.FileName = imuse;
			} else {
				startInfo.FileName = this.Settings.DosBoxPath;
				startInfo.Arguments = $"-noconsole -c \"mount -u C:\" -c \"mount C: '{Path.GetDirectoryName(imuse)}'\" -c \"C:\" -c \"imuse.exe\" -c \"exit\"";
			}
			startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
			DProcess.Start(startInfo);
		}

		public void RunSetup() {
			string setup = Path.Combine(this.GamePath, "setup.exe");
			if (!File.Exists(setup)) {
				setup = Path.Combine(this.GamePath, "install.exe");
			}
			if (!File.Exists(setup)) {
				//throw new FileNotFoundException("Unable to locate Dark Forces Setup tool!");
				MessageBox.Show("Unable to locate Dark Forces Setup tool!", "Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ProcessStartInfo startInfo = new();
			if (this.Settings.RunMode == DarkForcesSettings.RunModes.Native ||
				!IsValidDosBoxPath(this.Settings.DosBoxPath)) {

				startInfo.FileName = setup;
			} else {
				startInfo.FileName = this.Settings.DosBoxPath;
				startInfo.Arguments = $"-noconsole -c \"mount -u C:\" -c \"mount c: '{Path.GetDirectoryName(setup)}'\" -c \"c:\" -c \"{Path.GetFileName(setup)}\" -c \"exit\"";
			}
			startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
			DProcess.Start(startInfo);
		}

		private void RunDarkFrontend() {
			string dfrntend = Path.Combine(this.ModPath, "dfrntend.exe");
			if (!File.Exists(dfrntend)) {
				//throw new FileNotFoundException("Unable to locate Dark Frontend!");
				MessageBox.Show("Unable to locate Dark Frontend!", "Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ProcessStartInfo startInfo = new();
			if (this.Settings.RunMode == DarkForcesSettings.RunModes.Native ||
				!IsValidDosBoxPath(this.Settings.DosBoxPath)) {

				startInfo.FileName = dfrntend;
			} else {
				startInfo.FileName = this.Settings.DosBoxPath;
				startInfo.Arguments = $"-noconsole -c \"mount c: '{this.GamePath}'\" ";
				if (!File.Exists(Path.Combine(this.GamePath, "cd.id"))) {
					startInfo.Arguments += $"-c \"mount d: '{FirstCdDrive}':\\\" ";
				}

				string dfrntendDir = Path.GetDirectoryName(dfrntend);
				if (dfrntendDir.StartsWith(this.GamePath, StringComparison.CurrentCultureIgnoreCase)) {
					int segments = dfrntendDir[this.GamePath.Length..]
						.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Length;
					string shortDir = PathExtensions.GetShortName(dfrntendDir);
					startInfo.Arguments += "-c \"c:\" ";
					string[] shorts = shortDir.Split(Path.DirectorySeparatorChar);
					string path = string.Join(Path.DirectorySeparatorChar.ToString(), shorts,
						shorts.Length - segments, segments);
					startInfo.Arguments += $"-c \"cd {path}\" ";
				} else {
					startInfo.Arguments += $"-c \"mount e: '{dfrntendDir}'\" -c \"e:\" ";
				}

				startInfo.Arguments += $"-c \"dfrntend.exe\" -c \"exit\"";
			}
			startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
			DProcess.Start(startInfo);
		}

		public override bool HasSeparateMultiplayerBinary => false;

		public override GameOptions OptionsForm => new DarkForcesOptions(this);
	}

	public class DarkFrontend(Game game) : Mod<DarkForcesModInfo>(game, "dfrntend") {
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

		public override int SortOrder => -1;

		public override async Task RefreshCacheInfoAsync() {
			Program.ModCache.SetModInfo(this.Game.WhichGame, new DarkForcesModInfo() {
				Id = this.Id,
				Name = "Dark Frontend"
			});
			this.OnCacheChanged();
			await Task.Yield();
		}
	}

	public abstract class DarkForcesModBase(Game game, string id) : Mod<DarkForcesModInfo>(game, id) {
		protected abstract string ModPath { get; }

		public override bool Active {
			get {
				bool samePath = this.Game.ActiveModPath == this.ModPath;
				if (samePath) {
					return this.active;
				}

				return File.Exists(Path.Combine(this.Game.ActiveModPath, $"{this.Id}.gob"));
			}
		}
		private bool active = false;

		protected async Task CopyToGameDir(string srcName, string destName = null) {
			string src = Path.Combine(this.ModPath, srcName);
			if (!File.Exists(src)) {
				return;
			}

			destName ??= Path.GetFileName(src);

			string dest = Path.Combine(this.Game.ActiveModPath, destName);
			if (src == dest) {
				return;
			}

			await Task.Run(() => File.Copy(src, dest, true));
		}

		protected override async Task OnActivate() {
			string path = this.ModPath;
			if (!Directory.Exists(path)) {
				throw new DirectoryNotFoundException($"{path} not found when trying to activate.");
			}

			string gob = Path.Combine(path, $"{this.Id}.gob");
			if (!File.Exists(gob)) {
				throw new FileNotFoundException($"{gob} not found when trying to activate.");
			}
			this.active = true;

			await this.CopyToGameDir($"{this.Id}.gob");

			if (this.Cache is not DarkForcesModInfo info) {
				return;
			}

			if (info.LfdFile != null) {
				await this.CopyToGameDir(info.LfdFile, "DFBRIEF.LFD");
			}
			if (info.CrawlFile != null) {
				await this.CopyToGameDir(info.LfdFile, "FTEXTCRA.LFD");
			}
			foreach (string other in info.OtherFiles) {
				await this.CopyToGameDir(other);
			}
		}

		protected override async Task OnDeactivate() {
			string path = this.Game.ActiveModPath;
			bool samePath = this.ModPath == path;
			this.active = false;
			if (!samePath) {
				string gob = Path.Combine(path, $"{this.Id}.gob");
				if (File.Exists(gob)) {
					await Task.Run(() => File.Delete(gob));
				}
			}

			if (this.Cache is not DarkForcesModInfo info) {
				return;
			}

			if (info.LfdFile != null && !info.LfdFile.Equals("DFBRIEF.LFD", StringComparison.CurrentCultureIgnoreCase)) {
				string file = Path.Combine(path, "DFBRIEF.LFD");
				if (File.Exists(file)) {
					await Task.Run(() => File.Delete(file));
				}
			}
			if (info.CrawlFile != null && !info.CrawlFile.Equals("FTEXTCRA.LFD", StringComparison.CurrentCultureIgnoreCase)) {
				string file = Path.Combine(path, "FTEXTCRA.LFD");
				if (File.Exists(file)) {
					await Task.Run(() => File.Delete(file));
				}
			}
			if (!samePath) {
				foreach (string file in info.OtherFiles.Select(x => Path.Combine(path, x))) {
					if (File.Exists(file)) {
						await Task.Run(() => File.Delete(file));
					}
				}
			}
		}

		protected static string[] UnusableExtensions => [
			".bat", ".txt", ".gob", ".ini", ".exe", ".zip", ".pcx", ".cfg", ".cd", ".msg", ".id",
			".srn", ".ico", ".ex", ".e", ".cmd", ".pif", ".lnk", ".url", ".doc", ".rtf"
		];

		protected virtual IEnumerable<string> UsableFiles {
			get {
				string path = this.ModPath;
				if (!Directory.Exists(path)) {
					return [];
				}

				return Directory.EnumerateFiles(path)
					.Select(x => Path.GetFileName(x))
					.Where(x => !UnusableExtensions.Contains(Path.GetExtension(x).ToLower()));
			}
		}

		public override bool HasProperties => true;
		public override Form PropertiesForm => new DarkForcesModOptions(this, this.UsableFiles);

		public override async Task RefreshCacheInfoAsync() {
			if (this.Cache != null) {
				return;
			}

			Program.ModCache.SetModInfo(this.Game.WhichGame, new DarkForcesModInfo() {
				Id = this.Id,
				Name = this.Id
			});
			this.OnCacheChanged();
			await Task.Yield();
		}
	}

	public class DarkForcesMod(Game game, string id) : DarkForcesModBase(game, id) {
		protected override string ModPath => Path.Combine(this.Game.ModPath, this.Id);
	}

	public class DarkForcesMessyMod(Game game, string id) : DarkForcesModBase(game, id) {
		protected override string ModPath => this.Game.ModPath;
	}

	public class DarkForcesZippedMod(Game game, string id) : DarkForcesModBase(game, id) {
		protected override string ModPath => Path.Combine(this.Game.ModPath, $"{this.Id}.zip");

		protected override async Task OnActivate() {
			string path = this.ModPath;
			if (!File.Exists(path)) {
				throw new FileNotFoundException($"{path} not found when trying to activate.");
			}

			Dictionary<string, string> files = new() {
				[$"{this.Id}.gob"] = $"{this.Id}.gob"
			};

			if (this.Cache is DarkForcesModInfo info) {
				if (info.LfdFile != null) {
					files[info.LfdFile] = "DFBRIEF.LFD";
				}
				if (info.CrawlFile != null) {
					files[info.CrawlFile] = "FTEXTCRA.LFD";
				}
				foreach (string other in info.OtherFiles) {
					files[other] = other;
				}
			}

			using ZipArchive zip = ZipFile.OpenRead(path);
			foreach (string src in files.Keys) {
				ZipArchiveEntry entry = zip.Entries.FirstOrDefault(x => src.Equals(x.Name, StringComparison.CurrentCultureIgnoreCase));
				if (entry == null) {
					if (src == $"{this.Id}.gob") {
						throw new FileNotFoundException($"{src} not found when trying to activate.");
					}

					continue;
				}

				string dest = Path.Combine(this.Game.ActiveModPath, files[src]);
				if (File.Exists(dest)) {
					await Task.Run(() => File.Delete(dest));
				}

				entry.ExtractToFile(dest);
			}
		}

		protected override IEnumerable<string> UsableFiles {
			get {
				string path = Path.Combine(this.Game.ModPath, $"{this.Id}.zip");
				if (!File.Exists(path)) {
					return [];
				}

				ZipArchive zip;
				try {
					zip = ZipFile.OpenRead(path);
				} catch (Exception ex) {
					Debug.WriteLine(ex);
					return [];
				}
				using (zip) {
					return zip.Entries
						.Select(x => x.Name)
						.Where(x => !UnusableExtensions.Contains(Path.GetExtension(x).ToLower()))
						.ToArray();
				}
			}
		}
	}
}

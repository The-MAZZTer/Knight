using MZZT.Knight.Forms;
using MZZT.Windows;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using DProcess = System.Diagnostics.Process;

namespace MZZT.Knight.Games {
	public abstract class SithGame(SupportedGames game) : WindowsLecGame(game) {
		protected new SithSettings Settings => base.Settings as SithSettings;

		protected string ExeFileName => this.FindExeFile(this.GamePath);
		protected abstract string[] PossibleExeFileNames { get; }
		protected string FindExeFile(string folder) {
			if (string.IsNullOrWhiteSpace(folder)) {
				return null;
			}
			return this.PossibleExeFileNames.FirstOrDefault(x => File.Exists(Path.Combine(folder, $"{x}.exe")));
		}
		public override bool IsValidGamePath(string folder) =>
			this.FindExeFile(folder) != null;

		public override string AutoModPath => Path.Combine(this.GamePath, "jkpatch");
		public override string AutoActiveModPath => Path.Combine(this.GamePath, "jkpatch", "active");

		public bool IsValidActiveModPath(string folder) {
			if (string.IsNullOrWhiteSpace(folder)) {
				return false;
			}
			string absolute = Path.GetFullPath(folder).TrimEnd(Path.DirectorySeparatorChar).ToLower();
			string mod = this.ModPath.TrimEnd(Path.DirectorySeparatorChar).ToLower();
			string game = this.GamePath.TrimEnd(Path.DirectorySeparatorChar).ToLower();

			if (absolute.StartsWith(game) || absolute == game || absolute == mod) {
				return false;
			}
			return !absolute[game.Length..].Contains(' ');
		}

		public abstract string ModExtension { get; }
		protected override async Task OnFindModsAsync(CancellationToken token) {
			string modPath = this.ModPath;
			if (Directory.Exists(modPath)) {
				foreach (string file in Directory.EnumerateFiles(modPath, $"*.{this.ModExtension}")) {
					if (token.IsCancellationRequested) {
						return;
					}
					await Task.Yield();

					string id = Path.GetFileNameWithoutExtension(file);
					if (this.Mods.ContainsKey(id)) {
						continue;
					}

					await this.OnModFoundAsync(new SithMod(this, id), token);
				}

				foreach (string file in Directory.EnumerateFiles(modPath, "*.zip")) {
					if (token.IsCancellationRequested) {
						return;
					}

					ZipArchive zip;
					try {
						zip = ZipFile.OpenRead(file);
					} catch (Exception ex) {
						Debug.WriteLine(ex);
						continue;
					}
					using (zip) {
						foreach (ZipArchiveEntry entry in zip.Entries) {
							if (token.IsCancellationRequested) {
								return;
							}
							await Task.Yield();

							if (!entry.Name.ToLower().EndsWith($".{this.ModExtension}")) {
								continue;
							}

							string id = Path.GetFileNameWithoutExtension(entry.Name);
							if (this.Mods.ContainsKey(id)) {
								continue;
							}

							await this.OnModFoundAsync(new SithZippedMod(this, id, file), token);
						}
					}
				}
			}

			modPath = this.ActiveModPath;
			if (Directory.Exists(modPath)) {
				foreach (string file in Directory.EnumerateFiles(modPath, $"*.{this.ModExtension}")) {
					if (token.IsCancellationRequested) {
						return;
					}
					await Task.Yield();

					string id = Path.GetFileNameWithoutExtension(file);
					if (this.Mods.ContainsKey(id)) {
						continue;
					}

					await this.OnModFoundAsync(new SithMod(this, id), token);
				}
			}
		}

		public override bool AllowMultipleActiveMods => true;

		public override bool SupportsPatches => true;
		public override string AutoPatchPath => Path.Combine(this.GamePath, "patches");

		private string GetUniquePatchName(string baseName) {
			if (!File.Exists(Path.Combine(this.PatchPath, $"{baseName}.exe"))) {
				return $"{baseName}.exe";
			}

			int i = 0;
			while (File.Exists(Path.Combine(this.PatchPath, $"{baseName}.{i}.exe"))) {
				i++;
			}
			return $"{baseName}.{i}.exe";
		}

		private async Task FindGamePathPatches(CancellationToken token) {
			string path = this.PatchPath;
			string activeFile = Path.Combine(this.GamePath, $"{this.ExeFileName}.exe");
			PatchInfo activeInfo = null;
			if (File.Exists(activeFile)) {
				await Task.Run(() => {
					activeInfo = Program.PatchCache.IdentifyFile(activeFile);
				});
			}

			bool foundActive = false;
			foreach (string exe in Directory.EnumerateFiles(path, "*.exe")) {
				if (token.IsCancellationRequested) {
					return;
				}

				if (Path.GetFileName(exe).Equals($"{this.ExeFileName}.exe", StringComparison.CurrentCultureIgnoreCase)) {
					continue;
				}

				PatchInfo info = null;
				await Task.Run(() => {
					info = Program.PatchCache.IdentifyFile(exe);
				});
				if (info == null || this.Patches.ContainsKey(Path.GetFileName(exe))) {
					continue;
				}

				bool active = activeInfo != null && activeInfo.Size == info.Size && activeInfo.Hash.SequenceEqual(info.Hash);
				foundActive = foundActive || active;

				this.OnPatchFound(new SithPatch(this, info, exe, exe, active), token);
			}

			if (token.IsCancellationRequested) {
				return;
			}

			if (!foundActive) {
				if (activeInfo != null) {
					string dest = Path.Combine(this.GamePath,
						this.GetUniquePatchName(Path.GetFileNameWithoutExtension(activeInfo.FileName)));
					await Task.Run(() => File.Copy(activeFile, dest));

					this.OnPatchFound(new SithPatch(this, activeInfo, dest, activeFile, true), token);
				} else if (File.Exists(activeFile)) {
					PatchInfo info = null;
					await Task.Run(() => {
						info = PatchCache.HashFile($"Unknown Patch ({this.ExeFileName}.exe)", activeFile);
					});
					info.FileName = this.GetUniquePatchName($"{this.ExeFileName}.unknown.patch");
					Program.PatchCache.SetPatch(info);
					string dest = Path.Combine(this.PatchPath, info.FileName);
					await Task.Run(() => File.Copy(activeFile, dest));

					this.OnPatchFound(new SithPatch(this, info, dest, activeFile, true), token);
				}
			}
		}

		protected override async Task OnFindPatchesAsync(CancellationToken token) {
			string path = this.PatchPath;
			if (!Directory.Exists(path)) {
				return;
			}

			string gamePath = this.GamePath;
			if (gamePath == path) {
				await this.FindGamePathPatches(token);
				return;
			}

			string activeFile = Path.Combine(this.GamePath, $"{this.ExeFileName}.exe");
			PatchInfo activeInfo = null;
			if (File.Exists(activeFile)) {
				await Task.Run(() => {
					activeInfo = Program.PatchCache.IdentifyFile(activeFile);
				});
			}

			bool foundActive = false;
			foreach (string exe in Directory.EnumerateFiles(path, "*.exe")) {
				if (token.IsCancellationRequested) {
					return;
				}

				PatchInfo info = null;
				PatchInfo identified = null;
				await Task.Run(() => {
					info = PatchCache.HashFile($"Unknown Patch ({Path.GetFileName(exe)})", exe); ;
					identified = Program.PatchCache.IdentifyFile(info.Hash, info.Size);
				});
				if (identified != null && this.Patches.ContainsKey(identified.FileName)) {
					continue;
				}
				if (identified == null) {
					identified = info;
					Program.PatchCache.SetPatch(identified);
				}

				bool active = activeInfo != null && activeInfo.Hash.SequenceEqual(identified.Hash) && activeInfo.Size == identified.Size;
				foundActive = foundActive || active;

				this.OnPatchFound(new SithPatch(this, identified, exe, activeFile, active), token);
			}

			if (token.IsCancellationRequested) {
				return;
			}

			if (!foundActive) {
				if (activeInfo != null) {
					string dest = Path.Combine(this.PatchPath,
						this.GetUniquePatchName(Path.GetFileNameWithoutExtension(activeInfo.FileName)));
					await Task.Run(() => File.Copy(activeFile, dest));

					this.OnPatchFound(new SithPatch(this, activeInfo, dest, activeFile, true), token);
				} else if (File.Exists(activeFile)) {
					PatchInfo info = null;
					await Task.Run(() => {
						info = PatchCache.HashFile($"Unknown Patch ({this.ExeFileName}.exe)", activeFile);
					});
					info.FileName = this.GetUniquePatchName($"{this.ExeFileName}.unknown.patch");
					Program.PatchCache.SetPatch(info);

					string dest = Path.Combine(this.PatchPath, info.FileName);
					await Task.Run(() => File.Copy(activeFile, dest));

					this.OnPatchFound(new SithPatch(this, info, dest, activeFile, true), token);
				}
			}
		}

		public override async Task<Patch> InstallPatch(string filepath, PatchInfo info) {
			string path = this.PatchPath;
			string filename = Path.GetFileName(filepath);
			if (path != Path.GetDirectoryName(filepath)) {
				filename = this.GetUniquePatchName(Path.GetFileNameWithoutExtension(info.FileName));
				await Task.Run(() => File.Copy(filepath, Path.Combine(this.PatchPath, filename)));
			}

			Program.PatchCache.SetPatch(info);

			SithPatch patch = new(this, info, Path.Combine(this.PatchPath, filename),
				Path.Combine(this.GamePath, $"{this.ExeFileName}.exe"), false);
			this.Patches[patch.FileName] = patch;
			return patch;
		}

		public virtual string GetArguments(bool mods = false) {
			StringBuilder args = new();
			if (mods) {
				string gamePath = this.GamePath;
				string activeModPath = this.ActiveModPath;
				if (activeModPath.StartsWith(gamePath, StringComparison.CurrentCultureIgnoreCase)) {
					int segments = activeModPath[GamePath.Length..]
						.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Length;
					string[] shorts = activeModPath.Split(Path.DirectorySeparatorChar);

					string path = string.Join(Path.DirectorySeparatorChar.ToString(), shorts,
						shorts.Length - segments, segments);

					args.Append($"-path {path} ");
				} else {
					MessageBox.Show(Application.OpenForms.Cast<Form>().FirstOrDefault(),
						"Invalid active mod path, mods will not be applied. Please set the active mod path in the game options to a subdirectory of the game directory without spaces.",
						"Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}

			if (this.Settings.AdvancedDisplayOptions) {
				args.Append("-displayconfig ");
			}

			if (this.Settings.Console) {
				args.Append("-devmode ");
			}

			if (this.Settings.ConsoleStats) {
				args.Append("-dispstats ");
			}

			if (this.Settings.Framerate) {
				args.Append("-framerate ");
			}

			switch (this.Settings.Log) {
				case SithSettings.LogTypes.File:
					args.Append("-debug log ");
					break;
				case SithSettings.LogTypes.WindowsConsole:
					args.Append("-debug con ");
					break;
			}

			if (!this.Settings.Hud) {
				args.Append("-nohud ");
			}

			if (this.Settings.Verbosity > -1) {
				args.Append($"-verbose {this.Settings.Verbosity} ");
			}

			if (this.Settings.WindowedGui) {
				args.Append("-windowgui ");
			}

			return args.ToString().TrimEnd();
		}

		public override void Run(IEnumerable<Mod> mods) {
			string jk;
			/*if (File.Exists(Path.Combine(this.GamePath, $"openjkdf2-64.exe"))) {
				jk = Path.Combine(this.GamePath, $"openjkdf2-64.exe");
			} else if (File.Exists(Path.Combine(this.GamePath, $"inject.exe"))) {
				jk = Path.Combine(this.GamePath, $"inject.exe");
			} else*/ if (File.Exists(Path.Combine(this.GamePath, $"{this.ExeFileName}.exe"))) {
				jk = Path.Combine(this.GamePath, $"{this.ExeFileName}.exe");
			} else {
				throw new FileNotFoundException($"Unable to locate {this.Name}!");
			}

			ProcessStartInfo startInfo = new() {
				FileName = jk,
				WorkingDirectory = Path.GetDirectoryName(jk),
				Arguments = this.GetArguments(mods.Any())
			};

			if (this.Settings.AllowMultipleInstances) {
				int num = DProcess.GetProcessesByName(this.ExeFileName).Length + 1;

				DProcess process = DProcess.Start(startInfo);
				process.WaitForInputIdle(5000);
				if (!process.MainWindowHandle.Equals(IntPtr.Zero)) {
					new Window(process.MainWindowHandle).Title = $"{this.Name} - Knighted (Instance #{num})";
				}
			} else {
				DProcess.Start(startInfo);
			}
		}

		public override bool HasSeparateMultiplayerBinary => false;
	}

	public class JediKnight : SithGame {
		public JediKnight() : base(SupportedGames.JediKnight) { }
		protected new JediKnightSettings Settings => base.Settings as JediKnightSettings;

		protected override string GameRegistryPath =>
			@"HKEY_LOCAL_MACHINE\SOFTWARE\LucasArts Entertainment Company\JediKnight\1.0";
		protected override SteamHelper.AppIds AppId => SteamHelper.AppIds.JediKnight;
		protected override string SteamGameFolder => "Star Wars Jedi Knight";
		protected override string[] PossibleExeFileNames => ["jk", "JediKnight"];

		public override string ModExtension => "gob";

		public override string GetArguments(bool mods = false) {
			StringBuilder args = new(base.GetArguments(mods));
			if (args.Length > 0) {
				args.Append(" ");
			}

			if (this.Settings.EnableJkupCogVerbs) {
				string jk = Path.Combine(this.GamePath, "jk.exe");
				if (File.Exists(jk)) {
					PatchInfo patch = Program.PatchCache.IdentifyFile(jk);
					if (patch?.Name.Contains("Unofficial Patch") ?? false) {
						args.Append("-z ");
					}
				}
			}

			return args.ToString().TrimEnd();
		}

		public override GameOptions OptionsForm => new JediKnightOptions(this);
	}

	public class MysteriesOfTheSith : SithGame {
		public MysteriesOfTheSith() : base(SupportedGames.MysteriesOfTheSith) { }
		protected new MysteriesOfTheSithSettings Settings => base.Settings as MysteriesOfTheSithSettings;

		protected override string GameRegistryPath =>
			@"HKEY_LOCAL_MACHINE\SOFTWARE\LucasArts Entertainment Company LLC\Mysteries of the Sith\v1.0";
		protected override SteamHelper.AppIds AppId => SteamHelper.AppIds.MysteriesOfTheSith;
		protected override string SteamGameFolder => "Jedi Knight Mysteries of the Sith";
		protected override string[] PossibleExeFileNames => ["jkm", "JediKnightM"];

		public override string ModExtension => "goo";

		public override string GetArguments(bool mods = false) {
			StringBuilder args = new(base.GetArguments(mods));
			if (args.Length > 0) {
				args.Append(" ");
			}

			if (this.Settings.CogLog) {
				args.Append("-coglog ");
			}

			if (this.Settings.Record) {
				args.Append("-record ");
			}

			if (this.Settings.ResLog) {
				args.Append("-fail ");
			}

			if (this.Settings.SpeedUp) {
				args.Append("-fixed ");
			}

			return args.ToString().TrimEnd();
		}

		public override GameOptions OptionsForm => new MysteriesOfTheSithOptions(this);
	}

	public abstract class SithModBase(Game game, string id) : Mod<ModInfo>(game, id) {
		protected new SithGame Game => base.Game as SithGame;

		protected async Task RefreshModInfo(string modPath) {
			string name = this.Id;
			string file = Path.Combine(modPath, $"{this.Id}.{this.Game.ModExtension}");
			if (File.Exists(file)) {
				using FileStream stream = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
				name = (await GobModInfo.GetModInfo(stream))?.Name;
				if (string.IsNullOrWhiteSpace(name)) {
					name = this.Id;
				}
			}

			Program.ModCache.SetModInfo(this.Game.WhichGame, new ModInfo() {
				Id = this.Id,
				Name = name
			});
			this.OnCacheChanged();
		}

		public override bool Active => File.Exists(Path.Combine(this.Game.ActiveModPath,
			$"{this.Id}.{this.Game.ModExtension}"));
	}

	public class SithMod(Game owner, string id) : SithModBase(owner, id) {
		public override async Task RefreshCacheInfoAsync() {
			string modPath = this.Active ? this.Game.ActiveModPath : this.Game.ModPath;
			await this.RefreshModInfo(modPath);
		}

		protected override async Task OnActivate() {
			string file = Path.Combine(this.Game.ModPath, $"{this.Id}.{this.Game.ModExtension}");
			if (!File.Exists(file)) {
				throw new FileNotFoundException($"{file} not found when trying to activate.");
			}

			if (!Directory.Exists(this.Game.ActiveModPath)) {
				Directory.CreateDirectory(this.Game.ActiveModPath);
			}

			await Task.Run(() => File.Move(file,
				Path.Combine(this.Game.ActiveModPath, $"{this.Id}.{this.Game.ModExtension}")));
		}

		protected override async Task OnDeactivate() {
			string file = Path.Combine(this.Game.ActiveModPath, $"{this.Id}.{this.Game.ModExtension}");
			if (!File.Exists(file)) {
				throw new FileNotFoundException($"{file} not found when trying to deactivate.");
			}

			await Task.Run(() => File.Move(file,
				Path.Combine(this.Game.ModPath, $"{this.Id}.{this.Game.ModExtension}")));
		}
	}

	public class SithZippedMod(Game owner, string id, string zipFile) : SithModBase(owner, id) {
		private readonly string zipFile = zipFile;

		private async Task RefreshZippedInfo() {
			string name = this.Id;
			using (ZipArchive zip = ZipFile.OpenRead(this.zipFile)) {
				ZipArchiveEntry entry = zip.Entries.FirstOrDefault(x => x.Name.Equals($"{this.Id}.{this.Game.ModExtension}", StringComparison.CurrentCultureIgnoreCase));
				if (entry != null) {
					Stream stream = null;
					try {
						stream = entry.Open();
					} catch {
					}
					if (stream != null) {
						using (stream) {
							name = (await GobModInfo.GetModInfo(stream))?.Name;
						}
						if (string.IsNullOrWhiteSpace(name)) {
							name = this.Id;
						}
					}
				}
			}

			Program.ModCache.SetModInfo(this.Game.WhichGame, new ModInfo() {
				Id = this.Id,
				Name = name
			});
			this.OnCacheChanged();
		}

		public override async Task RefreshCacheInfoAsync() {
			if (!this.Active) {
				await this.RefreshZippedInfo();
				return;
			}

			await this.RefreshModInfo(this.Game.ActiveModPath);
		}

		protected override async Task OnActivate() {
			if (!File.Exists(this.zipFile)) {
				throw new FileNotFoundException($"{this.zipFile} not found when trying to activate.");
			}

			if (!Directory.Exists(this.Game.ActiveModPath)) {
				Directory.CreateDirectory(this.Game.ActiveModPath);
			}

			using ZipArchive zip = ZipFile.OpenRead(this.zipFile);
			ZipArchiveEntry entry = zip.Entries.FirstOrDefault(x => string.Equals(x.Name, $"{this.Id}.{this.Game.ModExtension}", StringComparison.CurrentCultureIgnoreCase)) ?? throw new FileNotFoundException($"{this.Id}.{this.Game.ModExtension} not found when trying to activate.");
			string dest = Path.Combine(this.Game.ActiveModPath, $"{this.Id}.{this.Game.ModExtension}");
			if (File.Exists(dest)) {
				await Task.Run(() => File.Delete(dest));
			}

			await Task.Run(() => entry.ExtractToFile(dest));
		}

		protected override async Task OnDeactivate() {
			string file = Path.Combine(this.Game.ActiveModPath, $"{this.Id}.{this.Game.ModExtension}");
			if (File.Exists(file)) {
				await Task.Run(() => File.Delete(file));
			}
		}
	}

	public class SithPatch(Game owner, PatchInfo info, string source, string dest, bool active) : Patch(owner, info, source) {
		private readonly string dest = dest;

		public override bool Active => this.active;
		protected override async Task InternalDeactivate() {
			if (File.Exists(this.dest)) {
				if (!File.Exists(this.FileName)) {
					await Task.Run(() => File.Move(this.dest, this.FileName));
				} else {
					await Task.Run(() => File.Delete(this.dest));
				}
			}
			this.active = false;
		}

		protected override async Task InternalActivate() {
			await this.InternalDeactivate();
			await Task.Run(() => File.Copy(this.FileName, this.dest));
			this.active = true;
		}
		private bool active = active;
	}
}

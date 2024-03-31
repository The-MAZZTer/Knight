using Microsoft.Win32;
using MZZT.Knight.Forms;
using MZZT.Properties;

namespace MZZT.Knight.Games {
	public abstract class Game(SupportedGames game) {
		public SupportedGames WhichGame { get; private set; } = game;

		protected GameSettings Settings =>
			Program.Settings.GetGameSettings(this.WhichGame);

		public event EventHandler EnabledChanged;
		public bool Enabled {
			get => this.Settings.Enabled;
			set {
				if (value && !this.IsValidGamePath(this.GamePath)) {
					throw new InvalidOperationException("Can't enable a game when no path has been set!");
				}

				this.Settings.Enabled = value;
				this.EnabledChanged?.Invoke(this, new EventArgs());
			}
		}

		public string Name => Resources.ResourceManager.GetString($"{this.GetType().Name}_Name");
		public string MnemonicName => Resources.ResourceManager.GetString($"{this.GetType().Name}_MnemonicName");
		public Image SmallIcon => Resources.ResourceManager.GetObject($"{this.GetType().Name}_Icon16") as Image;
		public Image LargeIcon => Resources.ResourceManager.GetObject($"{this.GetType().Name}_Icon32") as Image;

		public abstract string AutoGamePath { get; }
		public event EventHandler GamePathChanged;
		public string CustomGamePath {
			get => this.IsValidGamePath(this.Settings.Path) ? this.Settings.Path : null;
			set {
				if (value != null && !this.IsValidGamePath(value)) {
					throw new DirectoryNotFoundException("Invalid game path.");
				}

				this.Settings.Path = value;
				this.GamePathChanged?.Invoke(this, new EventArgs());
			}
		}
		public string GamePath {
			get {
				string path = CustomGamePath ?? AutoGamePath;
				if (path == null) {
					return null;
				}
				return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), path);
			}
		}
		public abstract bool IsValidGamePath(string folder);

		public Dictionary<string, Mod> Mods { get; } = [];

		public abstract string AutoModPath { get; }
		public event EventHandler ModPathChanged;
		public virtual string CustomModPath {
			get => this.Settings.ModPath;
			set {
				if (string.IsNullOrEmpty(value) || value == this.AutoModPath) {
					value = null;
				}

				if (this.CustomModPath == value) {
					this.Settings.ModPath = value;
					return;
				}

				this.Settings.ModPath = value;
				this.ModPathChanged?.Invoke(this, new EventArgs());
			}
		}
		public string ModPath {
			get {
				string path = CustomModPath ?? AutoModPath;
				if (path == null) {
					return null;
				}
				return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), path);
			}
		}

		public abstract string AutoActiveModPath { get; }
		public virtual string CustomActiveModPath {
			get => this.Settings.ActiveModPath;
			set {
				if (string.IsNullOrEmpty(value) || value == this.AutoActiveModPath) {
					value = null;
				}

				if (this.CustomActiveModPath == value) {
					this.Settings.ActiveModPath = value;
					return;
				}

				this.Settings.ActiveModPath = value;
			}
		}
		public string ActiveModPath {
			get {
				string path = CustomActiveModPath ?? AutoActiveModPath;
				if (path == null) {
					return null;
				}
				return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), path);
			}
		}

		public event EventHandler<ModEventArgs> ModFound;
		protected async Task OnModFoundAsync(Mod mod, CancellationToken token) {
			if (token.IsCancellationRequested) {
				return;
			}

			this.Mods[mod.Id] = mod;

			if (mod.Cache == null) {
				await mod.RefreshCacheInfoAsync();
			}

			this.ModFound?.Invoke(this, new ModEventArgs(mod));
		}
		protected abstract Task OnFindModsAsync(CancellationToken cancellationToken);
		public event EventHandler BeginFindMods;
		public event EventHandler EndFindMods;
		public void FindMods() {
			if (this.findModsTask != null) {
				this.findModsCancelled.Cancel();
				this.findModsTask.GetAwaiter().GetResult();
			}
			this.Mods.Clear();
			this.BeginFindMods?.Invoke(this, new EventArgs());
			this.findModsTask = Task.Run(async () => await this.OnFindModsAsync(this.findModsCancelled.Token).ContinueWith(x => {
				this.findModsCancelled = new CancellationTokenSource();
				this.findModsTask = null;
				this.EndFindMods?.Invoke(this, new EventArgs());
			}));
		}
		public bool IsFindingMods => this.findModsTask != null;
		public void CancelFindMods() => this.findModsCancelled?.Cancel();
		private CancellationTokenSource findModsCancelled = new();
		private Task findModsTask;

		public abstract bool AllowMultipleActiveMods { get; }

		public abstract bool SupportsPatches { get; }
		public Dictionary<string, Patch> Patches { get; } = [];

		public virtual string AutoPatchPath => null;
		public event EventHandler PatchPathChanged;
		public virtual string CustomPatchPath {
			get => this.Settings.PatchPath;
			set {
				if (string.IsNullOrEmpty(value) || value == this.AutoPatchPath) {
					value = null;
				}

				if (this.CustomPatchPath == value) {
					this.Settings.PatchPath = value;
					return;
				}

				this.Settings.PatchPath = value;
				this.PatchPathChanged?.Invoke(this, new EventArgs());
			}
		}
		public string PatchPath {
			get {
				string path = CustomPatchPath ?? AutoPatchPath;
				if (path == null) {
					return null;
				}
				return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), path);
			}
		}

		public event EventHandler<PatchEventArgs> PatchFound;
		protected void OnPatchFound(Patch patch, CancellationToken token) {
			if (token.IsCancellationRequested) {
				return;
			}

			this.Patches[patch.FileName] = patch;
			this.PatchFound?.Invoke(this, new PatchEventArgs(patch));
		}
		protected virtual async Task OnFindPatchesAsync(CancellationToken cancellationToken) => await Task.Yield();
		public event EventHandler BeginFindPatches;
		public event EventHandler EndFindPatches;
		public void FindPatches() {
			if (this.findPatchesTask != null) {
				this.findPatchesCancelled.Cancel();
				this.findPatchesTask.GetAwaiter().GetResult();
			}
			this.Patches.Clear();
			this.BeginFindPatches?.Invoke(this, new EventArgs());
			this.findPatchesTask = Task.Run(async () => await this.OnFindPatchesAsync(this.findPatchesCancelled.Token).ContinueWith(x => {
				this.findPatchesCancelled = new CancellationTokenSource();
				this.findPatchesTask = null;
				this.EndFindPatches?.Invoke(this, new EventArgs());
			}));
		}
		public bool IsFindingPatches => this.findPatchesTask != null;
		public void CancelFindPatches() => this.findPatchesCancelled?.Cancel();
		private CancellationTokenSource findPatchesCancelled = new();
		private Task findPatchesTask;

		public virtual async Task<Patch> InstallPatch(string filename, PatchInfo patch) {
			await Task.Yield();
			return null;
		}

		public abstract void Run(IEnumerable<Mod> mods);

		public abstract bool HasSeparateMultiplayerBinary { get; }
		public virtual bool AreModsMultiplayerOnly(IEnumerable<Mod> mods) => false;

		public virtual void RunMultiplayer(IEnumerable<Mod> mods) => this.Run(mods);

		public virtual GameOptions OptionsForm => null;
	}

	public class ModEventArgs(Mod mod) : EventArgs() {
		public Mod Mod { get; } = mod;
	}

	public class PatchEventArgs(Patch patch) : EventArgs() {
		public Patch Patch { get; } = patch;
	}

	public abstract class WindowsLecGame(SupportedGames game) : Game(game) {
		protected abstract string GameRegistryPath { get; }
		private string AutoRegistryPath {
			get {
				return Registry.GetValue(this.GameRegistryPath, "Install Path", null)
					is string path && this.IsValidGamePath(path) ? path : null;
			}
		}

		protected abstract SteamHelper.AppIds AppId { get; }
		protected abstract string SteamGameFolder { get; }
		private string AutoSteamPath {
			get {
				/*if (!SteamHelper.IsAppInstalled(this.AppId)) {
					return null;
				}*/

				foreach (string library in SteamHelper.LibraryFolders) {
					string gamepath = Path.Combine(library, "steamapps", "common", this.SteamGameFolder);
					if (this.IsValidGamePath(gamepath)) {
						return gamepath;
					}
				}

				return null;
			}
		}

		public override string AutoGamePath => this.AutoRegistryPath ?? this.AutoSteamPath;
	}

	public abstract class Mod(Game game, string id) {
		public Game Game { get; private set; } = game;

		public abstract bool Active { get; }
		public async Task Activate() {
			if (this.Active) {
				return;
			}
			await this.OnActivate();
		}
		protected abstract Task OnActivate();
		public async Task Deactivate() {
			if (!this.Active) {
				return;
			}
			await this.OnDeactivate();
		}
		protected abstract Task OnDeactivate();

		public string Id { get; private set; } = id;

		public abstract string Name { get; set; }
		protected void OnNameChanged() =>
			this.NameChanged?.Invoke(this, new EventArgs());
		public event EventHandler NameChanged;

		public virtual int SortOrder { get; } = 0;

		public ModInfo Cache =>
			Program.ModCache.GetModInfo(this.Game.WhichGame, this.Id);

		public abstract Task RefreshCacheInfoAsync();
		public void RefreshCacheInfo() =>
			Task.Run(async () => await this.RefreshCacheInfoAsync());

		protected void OnCacheChanged() {
			this.CacheChanged?.Invoke(this, new EventArgs());
		}
		public event EventHandler CacheChanged;

		public virtual bool HasProperties => false;
		public virtual Form PropertiesForm => null;
	}

	public abstract class Mod<T>(Game owner, string id) : Mod(owner, id) where T : ModInfo, new() {
		public override string Name {
			get => this.Cache?.Name ?? this.Id;
			set {
				if (this.Name == value) {
					return;
				}

				if (this.Cache == null) {
					Program.ModCache.SetModInfo(this.Game.WhichGame, new T() {
						Id = this.Id
					});
				}
				this.Cache.Name = value;

				this.OnNameChanged();
			}
		}
	}

	public abstract class Patch(Game game, PatchInfo info, string source) {
		public Game Game { get; private set; } = game;

		public string FileName { get; } = source;
		public PatchInfo Cache { get; } = info;
		public string Name {
			get => this.Cache.Name;
			set {
				if (this.Cache.Name == value) {
					return;
				}

				this.Cache.Name = value;

				this.NameChanged?.Invoke(this, new EventArgs());
			}
		}
		public event EventHandler NameChanged;

		public virtual int SortOrder => 0;

		public abstract bool Active { get; }
		public async Task Activate() {
			if (this.Active) {
				return;
			}
			await this.InternalActivate();
		}
		protected abstract Task InternalActivate();
		public async Task Deactivate() {
			if (!this.Active) {
				return;
			}
			await this.InternalDeactivate();
		}
		protected abstract Task InternalDeactivate();

	}
}

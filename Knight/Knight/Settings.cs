using System.Text.Json.Serialization;

namespace MZZT.Knight {
	public class Settings : MZZT.Settings.Settings<Settings> {
		[JsonPropertyName("firstRun")]
		public bool FirstRun {
			get; set;
		} = true;

		[JsonPropertyName("formBounds")]
		public Rectangle FormBounds {
			get; set;
		} = new Rectangle(int.MinValue, int.MinValue, 300, 450);

		[JsonPropertyName("view")]
		public View View {
			get => this.view;
			set {
				if (this.view == value) {
					return;
				}

				this.view = value;
				this.ViewChanged?.Invoke(this, new EventArgs());
			}
		}
		private View view = View.Details;
		public event EventHandler ViewChanged;

		[JsonPropertyName("largeIconSize")]
		public Size LargeIconSize {
			get; set;
		} = new Size(91, 91);

		[JsonPropertyName("expandedGame")]
		public SupportedGames ExpandedGame {
			get; set;
		} = SupportedGames.None;

		[JsonPropertyName("simpleList")]
		public bool SimpleList {
			get; set;
		} = false;

		public enum RunActions : byte {
			None,
			Minimize,
			Close
		}

		[JsonPropertyName("runAction")]
		public RunActions RunAction {
			get; set;
		} = RunActions.None;

		[JsonPropertyName("darkForces")]
		public DarkForcesSettings DarkForces {
			get; set;
		} = new DarkForcesSettings();

		[JsonPropertyName("jediKnight")]
		public JediKnightSettings JediKnight {
			get; set;
		} = new JediKnightSettings();

		[JsonPropertyName("mysteriesOfTheSith")]
		public MysteriesOfTheSithSettings MysteriesOfTheSith {
			get; set;
		} = new MysteriesOfTheSithSettings();

		[JsonPropertyName("jediOutcast")]
		public Quake3Settings JediOutcast {
			get; set;
		} = new Quake3Settings();

		[JsonPropertyName("jediAcademy")]
		public Quake3Settings JediAcademy {
			get; set;
		} = new Quake3Settings();

		public GameSettings GetGameSettings(SupportedGames game) {
			return game switch {
				SupportedGames.DarkForces => this.DarkForces,
				SupportedGames.JediKnight => this.JediKnight,
				SupportedGames.MysteriesOfTheSith => this.MysteriesOfTheSith,
				SupportedGames.JediOutcast => this.JediOutcast,
				SupportedGames.JediAcademy => this.JediAcademy,
				_ => null,
			};
		}
	}

	public abstract class GameSettings {
		[JsonPropertyName("enabled")]
		public bool Enabled {
			get; set;
		} = false;

		[JsonPropertyName("path")]
		public string Path {
			get; set;
		} = null;

		[JsonPropertyName("modPath")]
		public string ModPath {
			get; set;
		} = null;

		[JsonPropertyName("activeModPath")]
		public string ActiveModPath {
			get; set;
		} = null;

		[JsonPropertyName("patchPath")]
		public string PatchPath {
			get; set;
		} = null;
	}

	public class DarkForcesSettings : GameSettings {
		[JsonPropertyName("autoTest")]
		public bool AutoTest {
			get; set;
		} = false;

		[JsonPropertyName("cdDrive")]
		public char CdDrive {
			get; set;
		} = (char)0;

		[JsonPropertyName("darkXlPath")]
		public string DarkXlPath {
			get; set;
		} = null;

		[JsonPropertyName("dosBoxPath")]
		public string DosBoxPath {
			get; set;
		} = null;

		[JsonPropertyName("enableScreenshots")]
		public bool EnableScreenshots {
			get; set;
		} = false;

		[JsonPropertyName("levelSelect")]
		public string LevelSelect {
			get; set;
		} = null;

		[JsonPropertyName("log")]
		public bool Log {
			get; set;
		} = false;

		public enum RunModes : byte {
			Native,
			DosBox,
			DarkXl
		}

		[JsonPropertyName("runMode")]
		public RunModes RunMode {
			get; set;
		} = RunModes.Native;

		[JsonPropertyName("skipCutscenes")]
		public bool SkipCutscenes {
			get; set;
		} = false;

		[JsonPropertyName("skipFilesCheck")]
		public bool SkipFilesCheck {
			get; set;
		} = false;

		[JsonPropertyName("skipMemoryCheck")]
		public bool SkipMemoryCheck {
			get; set;
		} = false;
	}

	public class SithSettings : GameSettings {
		[JsonPropertyName("advancedDisplayOptions")]
		public bool AdvancedDisplayOptions {
			get; set;
		} = true;

		[JsonPropertyName("allowMultipleInstances")]
		public bool AllowMultipleInstances {
			get; set;
		} = true;

		[JsonPropertyName("console")]
		public bool Console {
			get; set;
		} = false;

		[JsonPropertyName("consoleStats")]
		public bool ConsoleStats {
			get; set;
		} = false;

		[JsonPropertyName("framerate")]
		public bool Framerate {
			get; set;
		} = false;

		public enum LogTypes : byte {
			None,
			File,
			WindowsConsole
		}

		[JsonPropertyName("log")]
		public LogTypes Log {
			get; set;
		} = LogTypes.None;

		[JsonPropertyName("hud")]
		public bool Hud {
			get; set;
		} = true;

		[JsonPropertyName("verbosity")]
		public sbyte Verbosity {
			get; set;
		} = -1;

		[JsonPropertyName("windowedGui")]
		public bool WindowedGui {
			get; set;
		} = true;
	}

	public class JediKnightSettings : SithSettings {
		[JsonPropertyName("enableJkupCogVerts")]
		public bool EnableJkupCogVerbs {
			get; set;
		} = true;

	}

	public class MysteriesOfTheSithSettings : SithSettings {
		[JsonPropertyName("cogLog")]
		public bool CogLog {
			get; set;
		} = false;

		[JsonPropertyName("record")]
		public bool Record {
			get; set;
		} = false;

		[JsonPropertyName("resLog")]
		public bool ResLog {
			get; set;
		} = false;

		[JsonPropertyName("speedUp")]
		public bool SpeedUp {
			get; set;
		} = false;
	}

	public class Quake3Settings : GameSettings {
		[JsonPropertyName("commands")]
		public string Commands {
			get; set;
		} = null;
	}
}

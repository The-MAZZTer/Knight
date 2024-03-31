using MZZT.Drawing;
using MZZT.FileFormats;
using MZZT.Input;
using MZZT.Knight.Forms;
using MZZT.Knight.Games;
using MZZT.Properties;
using MZZT.Updates;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Process = MZZT.Diagnostics.Process;

[assembly: SupportedOSPlatform("windows")]
namespace MZZT.Knight {
	internal static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		internal static async Task Main() {
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();

			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);

			if (!ProgramArguments.InjectIntoStatic(typeof(Program))) {
				if (string.IsNullOrEmpty(ProgramArguments.ParseError)) {
					TaskDialog.ShowDialog(new() {
						Text = ProgramArguments.GetHelp(typeof(Program)),
						AllowCancel = true,
						Buttons = { TaskDialogButton.OK },
						Caption = "Help - Knight",
						DefaultButton = TaskDialogButton.OK,
						Icon = TaskDialogIcon.Information
					}, TaskDialogStartupLocation.CenterScreen);
				} else {
					TaskDialog.ShowDialog(new() {
						Text = ProgramArguments.ParseError + Environment.NewLine + Environment.NewLine + ProgramArguments.GetHelp(typeof(Program)),
						AllowCancel = true,
						Buttons = { TaskDialogButton.OK },
						Caption = "Error - Knight",
						DefaultButton = TaskDialogButton.OK,
						Icon = TaskDialogIcon.Error
					}, TaskDialogStartupLocation.CenterScreen);
				}
				return;
			}

			Process existing = Process.All
				.Where(x => {
					if (x == Process.Current) {
						return false;
					}
					try {
						if (x.ImageName != Process.Current.ImageName) {
							return false;
						}
					} catch (Win32Exception) {
						return false;
					}
					return true;
				}).FirstOrDefault();
			if (existing != null) {
				existing.MainWindow?.Activate();
				return;
			}

			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"The MAZZTer\Knight\options.json");
			try {
				Settings = Settings.Load(path);
			} catch (FormatException) {
				Settings = new Settings();
			} catch (IOException) {
				Settings = new Settings();
			}

			Games = new Dictionary<SupportedGames, Game>() {
				[SupportedGames.DarkForces] = new DarkForces(),
				[SupportedGames.JediKnight] = new JediKnight(),
				[SupportedGames.MysteriesOfTheSith] = new MysteriesOfTheSith(),
				[SupportedGames.JediOutcast] = new JediOutcast(),
				[SupportedGames.JediAcademy] = new JediAcademy()
			};

			if (Settings.FirstRun) {
				foreach (Game game in Games.Values) {
					if (game.GamePath != null) {
						game.Enabled = true;

						if (Settings.ExpandedGame == (SupportedGames)(-1)) {
							Settings.ExpandedGame = game.WhichGame;
						}
					}
				}

				Settings.FirstRun = false;
				Settings.Save(path);
			}

			ModCache = await ModCache.Load();
			PatchCache = await PatchCache.Load();

			using PrivateFontCollection fonts = new();
			byte[] rawFont = Resources.Material_Symbols_Filled;
			IntPtr block = Marshal.AllocHGlobal(rawFont.Length);
			try {
				Marshal.Copy(rawFont, 0, block, rawFont.Length);
				fonts.AddMemoryFont(block, rawFont.Length);
			} finally {
				Marshal.FreeHGlobal(block);
			}
			Glyphs = new(fonts.Families[0]);

			/*FontGlyphRenderer renderer = new(MaterialIcons);
			foreach (int i in new int[] { 16, 24, 32, 48, 64, 128, 256 }) {
				renderer.DrawBitmapGlyph(new Size(i, i), ["shield", "swords"], [Color.Orange, Color.Gold], i, GraphicsUnit.Pixel).Save($@"C:\Users\mzzt\OneDrive\Desktop\knight-{i}.png", ImageFormat.Png);
			}*/

			Thread thread = new(() => {
				Updater = new UpdateForm(new Uri(UPDATE_URL)) {
					Icon = Resources.Knight
				};

				Application.Run(new MainForm());
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}
		public static FontGlyphRenderer Glyphs { get; private set; }
		public static Settings Settings { get; private set; }
		public const string UPDATE_URL = "https://raw.githubusercontent.com/The-MAZZTer/Knight/main/update.json";
		public static UpdateForm Updater { get; private set; }
		public static Dictionary<SupportedGames, Game> Games { get; private set; }
		public static ModCache ModCache { get; private set; }
		public static PatchCache PatchCache { get; private set; }

		[ProgramSwitch("cdid", IgnoreOtherArgs = true)]
		public static async Task InstallCdId(bool state) {
			if (state) {
				await DarkForcesOptions.InstallCdId();
			} else {
				await DarkForcesOptions.RemoveCdId();
			}

			Environment.Exit(0);
		}

		[ProgramSwitch("directplay", IgnoreOtherArgs = true)]
		public static void SetDirectPlay(string args) {
			int index = args.IndexOf('=');
			string game = args[..index];
			string commandLine = args[(index + 1)..];

			DirectPlayOptions.SetDirectPlayCommandLine(game, commandLine);

			Environment.Exit(0);
		}

		public static void RunElevated(string arg, object value) {
			System.Diagnostics.Process.Start(new ProcessStartInfo(Application.ExecutablePath,
				$"--{arg}={value}") {

				UseShellExecute = true,
				Verb = "runas"
			});
		}

		[ProgramSwitch("masterini", IgnoreOtherArgs = true)]
		public static async Task ImportDarkFrontendMods(string path) {
			ModCache = await ModCache.Load();

			Ini ini = await Ini.ReadAsync(path);

			foreach (string section in ini.Data.Keys) {
				DarkForcesModInfo info = new() {
					Id = section
				};

				List<string> others = [];
				foreach (string key in ini.Data[section].Keys) {
					string value = ini.Data[section][key];
					if (string.IsNullOrWhiteSpace(value)) {
						continue;
					}

					switch (key) {
						case "GameName":
							info.Name = value;
							continue;
						case "LFDName":
							info.LfdFile = $"{value}.LFD";
							continue;
						case "CRAWLName":
							info.CrawlFile = $"{value}.LFD";
							continue;
					}
					if (key.StartsWith("Other")) {
						others.Add(value);
					}
				}
				info.OtherFiles = [.. others];
				ModCache.SetModInfo(SupportedGames.DarkForces, info);
			}

			ModCache.Save();
			await ModCache.Wait();

			Environment.Exit(0);
		}
	}

	public enum SupportedGames : sbyte {
		None = -1,
		DarkForces,
		JediKnight,
		MysteriesOfTheSith,
		JediOutcast,
		JediAcademy
	}
}

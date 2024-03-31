using MZZT.Input;
using MZZT.Knight.Games;

namespace MZZT.Knight.Forms {
	public partial class MysteriesOfTheSithOptions : GameOptions {
		public MysteriesOfTheSithOptions(MysteriesOfTheSith game) : base(game) {
			this.InitializeComponent();
			this.AddDialogButtons();

			this.BrowsePatchFolder.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "folder_managed", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.BrowseActiveModFolder.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "folder_managed", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.BrowseModFolder.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "folder_managed", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.DirectPlayButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "router", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
		}
		public new MysteriesOfTheSith Game => base.Game as MysteriesOfTheSith;
		public new MysteriesOfTheSithSettings Settings => base.Settings as MysteriesOfTheSithSettings;

		private string modPath;
		private string activeModPath;
		private string patchPath;

		protected override void Revert() {
			using (new UserInputBlocker()) {
				this.WindowedGuiCheckbox.Checked = this.Settings.WindowedGui;
				this.AdvancedDisplayOptionsCheckbox.Checked = this.Settings.AdvancedDisplayOptions;
				this.FramerateCheckbox.Checked = this.Settings.Framerate;
				this.PlayerStatusCheckbox.Checked = this.Settings.ConsoleStats;
				this.HudCheckbox.Checked = this.Settings.Hud;
				this.RecordCheckbox.Checked = this.Settings.Record;

				this.MultipleInstancesCheckbox.Checked = this.Settings.AllowMultipleInstances;
				this.LogNoRadio.Checked = this.Settings.Log == SithSettings.LogTypes.None;
				this.LogFileRadio.Checked = this.Settings.Log == SithSettings.LogTypes.File;
				this.LogConRadio.Checked = this.Settings.Log == SithSettings.LogTypes.WindowsConsole;

				this.VerboseCheckbox.Checked = this.Settings.Verbosity > -1;
				if (this.Settings.Verbosity > -1) {
					this.VerboseUpDown.Value = this.Settings.Verbosity;
				} else {
					this.VerboseUpDown.Value = 0;
				}

				this.LogCogCheckbox.Checked = this.Settings.CogLog;
				this.LogResourcesCheckbox.Checked = this.Settings.ResLog;

				this.ConsoleCheckbox.Checked = this.Settings.Console;
				this.SpeedUpCheckbox.Checked = this.Settings.SpeedUp;

				this.modPath = this.Game.ModPath;
				this.activeModPath = this.Game.ActiveModPath;
				this.patchPath = this.Game.PatchPath;
			}

			base.Revert();
		}

		protected override async Task Apply() {
			this.Settings.WindowedGui = this.WindowedGuiCheckbox.Checked;
			this.Settings.AdvancedDisplayOptions = this.AdvancedDisplayOptionsCheckbox.Checked;
			this.Settings.Framerate = this.FramerateCheckbox.Checked;
			this.Settings.ConsoleStats = this.PlayerStatusCheckbox.Checked;
			this.Settings.Hud = this.HudCheckbox.Checked;
			this.Settings.Record = this.RecordCheckbox.Checked;

			this.Settings.AllowMultipleInstances = this.MultipleInstancesCheckbox.Checked;
			if (this.LogNoRadio.Checked) {
				this.Settings.Log = SithSettings.LogTypes.None;
			} else if (this.LogFileRadio.Checked) {
				this.Settings.Log = SithSettings.LogTypes.File;
			} else if (this.LogConRadio.Checked) {
				this.Settings.Log = SithSettings.LogTypes.WindowsConsole;
			}

			if (this.VerboseCheckbox.Checked) {
				this.Settings.Verbosity = (sbyte)this.VerboseUpDown.Value;
			} else {
				this.Settings.Verbosity = -1;
			}
			this.Settings.CogLog = this.LogCogCheckbox.Checked;
			this.Settings.ResLog = this.LogResourcesCheckbox.Checked;

			this.Settings.Console = this.ConsoleCheckbox.Checked;
			this.Settings.SpeedUp = this.SpeedUpCheckbox.Checked;

			this.Game.CustomModPath = this.modPath;
			this.Game.CustomActiveModPath = this.activeModPath;
			this.Game.CustomPatchPath = this.patchPath;

			Program.Settings.Save();

			await base.Apply();
		}

		private void BrowseModFolder_Click(object sender, EventArgs e) {
			using FolderBrowserDialog dialog = new() {
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				Description = "Select the mods folder",
				InitialDirectory = this.modPath,
				ShowHiddenFiles = false,
				ShowNewFolderButton = false,
				ShowPinnedPlaces = true,
				UseDescriptionForTitle = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = this.modPath,
				EnsurePathExists = true,
				EnsureValidNames = true,
				InitialDirectory = null,
				IsFolderPicker = true,
				Multiselect = false,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = $"Select the mods folder"
			};*/

			if (dialog.ShowDialog(this) == DialogResult.OK) {
			//if (dialog.ShowDialog(this.Handle) == CommonFileDialogResult.Ok) {
				if (this.activeModPath.ToLower().TrimEnd(Path.DirectorySeparatorChar) !=
					dialog.SelectedPath.ToLower().TrimEnd(Path.DirectorySeparatorChar)) {
					/*if (this.activeModPath.ToLower().TrimEnd(Path.DirectorySeparatorChar) !=
						dialog.FileName.ToLower().TrimEnd(Path.DirectorySeparatorChar)) {*/

					this.modPath = dialog.SelectedPath;
					//this.modPath = dialog.FileName;
					this.EnableApply();
				} else {
					MessageBox.Show(this,
						"The mod path must be different from the nactive mod path. This change will not be saved.",
						"Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void BrowseActiveModFolder_Click(object sender, EventArgs e) {
			using FolderBrowserDialog dialog = new() {
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				Description = "Select the active mods folder",
				InitialDirectory = this.activeModPath,
				ShowHiddenFiles = false,
				ShowNewFolderButton = false,
				ShowPinnedPlaces = true,
				UseDescriptionForTitle = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = this.activeModPath,
				EnsurePathExists = true,
				EnsureValidNames = true,
				InitialDirectory = null,
				IsFolderPicker = true,
				Multiselect = false,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = $"Select the active mods folder"
			};*/

			if (dialog.ShowDialog(this) == DialogResult.OK) {
				//if (dialog.ShowDialog(this.Handle) == CommonFileDialogResult.Ok) {
				if (this.Game.IsValidActiveModPath(dialog.SelectedPath)) {
					//if (this.Game.IsValidActiveModPath(dialog.FileName)) {
					this.activeModPath = dialog.SelectedPath;
					//this.activeModPath = dialog.FileName;
					this.EnableApply();
				} else {
					MessageBox.Show(this,
						"The active mod path must be in a subfolder of the game path, must not be the inactive mod path, and the subpath name must not have spaces. This change will not be saved.",
						"Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void BrowsePatchFolder_Click(object sender, EventArgs e) {
			using FolderBrowserDialog dialog = new() {
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				Description = "Select the patch folder",
				InitialDirectory = this.patchPath,
				ShowHiddenFiles = false,
				ShowNewFolderButton = false,
				ShowPinnedPlaces = true,
				UseDescriptionForTitle = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = this.patchPath,
				EnsurePathExists = true,
				EnsureValidNames = true,
				InitialDirectory = null,
				IsFolderPicker = true,
				Multiselect = false,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = $"Select the patch folder"
			};*/

			if (dialog.ShowDialog(this) == DialogResult.OK) {
				//if (dialog.ShowDialog(this.Handle) == CommonFileDialogResult.Ok) {
				this.patchPath = dialog.SelectedPath;
				//this.patchPath = dialog.FileName;
				this.EnableApply();
			}
		}

		private async void DirectPlayButton_Click(object sender, EventArgs e) {
			if (this.ApplyEnabled) {
				await this.Apply();
			}

			new DirectPlayOptions(this.Game).ShowDialog(this);
		}

		private void VerboseCheckbox_CheckedChanged(object sender, EventArgs e) {
			this.VerboseUpDown.Enabled = (sender as CheckBox).Checked;

			if (!UserInputBlocker.IsUserInput) {
				return;
			}

			if (!this.VerboseCheckbox.Checked && this.LogConRadio.Checked) {
				this.LogConRadio.Checked = false;
				this.LogNoRadio.Checked = true;
			}

			this.EnableApply();
		}

		private void VerboseUpDown_ValueChanged(object sender, EventArgs e) {
			if (!UserInputBlocker.IsUserInput) {
				return;
			}

			if (this.VerboseUpDown.Value == 0 && this.LogConRadio.Checked) {
				this.LogConRadio.Checked = false;
				this.LogNoRadio.Checked = true;
			}

			this.EnableApply();
		}

		private void LogConRadio_CheckedChanged(object sender, EventArgs e) {
			if (!UserInputBlocker.IsUserInput) {
				return;
			}

			if (!this.LogConRadio.Checked) {
				return;
			}

			this.VerboseCheckbox.Checked = true;
			if (this.VerboseUpDown.Value < 1) {
				this.VerboseUpDown.Value = 1;
			}

			this.EnableApply();
		}

		protected void Changed(object sender, EventArgs e) =>
			this.EnableApply();

		private void LogMotSCheckbox_CheckedChanged(object sender, EventArgs e) {
			if (!UserInputBlocker.IsUserInput) {
				return;
			}

			if ((sender as CheckBox).Checked) {
				MessageBox.Show(this, "Warning: This option may slow the game significantly.",
					"Warning - Knight", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			this.EnableApply();
		}

		private void RecordCheckbox_CheckedChanged(object sender, EventArgs e) {
			if (!UserInputBlocker.IsUserInput) {
				return;
			}

			if ((sender as CheckBox).Checked) {
				MessageBox.Show(this,
					"Warning: This option will eat up lots of disk space very quickly at high resolutions.",
					"Warning - Knight", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			this.EnableApply();
		}
	}
}

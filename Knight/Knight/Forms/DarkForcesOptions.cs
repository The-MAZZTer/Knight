using MZZT.Input;
using MZZT.Knight.Games;

namespace MZZT.Knight.Forms {
	public partial class DarkForcesOptions : GameOptions {
		public DarkForcesOptions(DarkForces game) : base(game) {
			this.InitializeComponent();
			this.AddDialogButtons();

			this.IMuseButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "music_note", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.SetupButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "stadia_controller", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.BrowseMods.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "folder_managed", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.BrowseDarkXl.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "folder_managed", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.BrowseDosBox.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "folder_managed", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.NoCdCheckbox.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), ["shield", "security"], [Color.Blue, Color.Gold], 16, GraphicsUnit.Pixel);
		}
		public new DarkForces Game => base.Game as DarkForces;
		public new DarkForcesSettings Settings => base.Settings as DarkForcesSettings;

		private const string CdId = "Dark Forces Version 1.0 (Build 2)\r\n";

		public static async Task RemoveCdId() {
			string cdid = Path.Combine(Path.GetPathRoot(Application.ExecutablePath), "CD.ID");
			if (File.Exists(cdid)) {
				await Task.Run(() => File.Delete(cdid));
			}
		}

		public static async Task InstallCdId() {
			string cdid = Path.Combine(Path.GetPathRoot(Application.ExecutablePath), "CD.ID");
			using FileStream stream = new(cdid, FileMode.Create, FileAccess.Write, FileShare.None);
			using StreamWriter writer = new(stream);
			await writer.WriteAsync(CdId);
		}

		private void EnsureValidRunSelection() {
			if (this.RunNative.Checked) {
				return;
			}

			if (this.RunDosBox.Checked && DarkForces.IsValidDosBoxPath(this.dosBoxPath)) {
				this.RunNative.Checked = true;
			}

			if (this.RunDarkXl.Checked && DarkForces.IsValidDarkXlPath(this.darkXlPath)) {
				this.RunNative.Checked = true;
			}
		}
		private string dosBoxPath;
		private string darkXlPath;
		private string modPath;

		protected override void Revert() {
			using (new UserInputBlocker()) {
				this.RunNative.Checked = this.Settings.RunMode == DarkForcesSettings.RunModes.Native;
				this.RunDosBox.Checked = this.Settings.RunMode == DarkForcesSettings.RunModes.DosBox;
				this.dosBoxPath = this.Settings.DosBoxPath;
				this.RunDarkXl.Checked = this.Settings.RunMode == DarkForcesSettings.RunModes.DarkXl;
				this.darkXlPath = this.Settings.DarkXlPath;

				this.modPath = this.Game.ModPath;

				this.LogCheckbox.Checked = this.Settings.Log;
				this.SkipMemoryCheckbox.Checked = this.Settings.SkipMemoryCheck;
				this.SkipFilesCheckbox.Checked = this.Settings.SkipFilesCheck;
				this.AutoTestCheckbox.Checked = this.Settings.AutoTest;

				this.ScreenshotsCheckbox.Checked = this.Settings.EnableScreenshots;
				this.LevelSelectCheckbox.Checked = this.Settings.LevelSelect != null;
				if (this.Settings.LevelSelect != null) {
					this.LevelCombobox.Text = this.Settings.LevelSelect;
				} else {
					this.LevelCombobox.Text = "SECBASE";
				}
				this.SkipCutscenesCheckbox.Checked = this.Settings.SkipCutscenes;

				int cdIdCount = 0;
				cdIdCount += File.Exists(Path.Combine(this.Game.GamePath,
					"CD.ID")) ? 1 : 0;
				cdIdCount += File.Exists(Path.Combine(Path.GetPathRoot(Application.ExecutablePath),
					"CD.ID")) ? 1 : 0;
				this.NoCdCheckbox.CheckState = cdIdCount switch {
					0 => CheckState.Unchecked,
					1 => CheckState.Indeterminate,
					_ => CheckState.Checked
				};

				this.ForceCdCheckbox.Checked = this.Settings.CdDrive != '\0';
				if (this.ForceCdCheckbox.Checked) {
					this.CdUpDown.Text = this.Settings.CdDrive.ToString();
				} else {
					this.CdUpDown.Text = "D";
				}
			}

			base.Revert();
		}

		protected override async Task Apply() {
			if (this.RunNative.Checked) {
				this.Settings.RunMode = DarkForcesSettings.RunModes.Native;
			} else if (this.RunDosBox.Checked) {
				this.Settings.RunMode = DarkForcesSettings.RunModes.DosBox;
			} else if (this.RunDarkXl.Checked) {
				this.Settings.RunMode = DarkForcesSettings.RunModes.DarkXl;
			}

			if (DarkForces.IsValidDosBoxPath(this.dosBoxPath)) {
				this.Settings.DosBoxPath = this.dosBoxPath;
			}
			if (DarkForces.IsValidDarkXlPath(this.darkXlPath)) {
				this.Settings.DarkXlPath = this.darkXlPath;
			}

			this.Game.CustomModPath = this.modPath;

			this.Settings.Log = this.LogCheckbox.Checked;
			this.Settings.SkipMemoryCheck = this.SkipMemoryCheckbox.Checked;
			this.Settings.SkipFilesCheck = this.SkipFilesCheckbox.Checked;
			this.Settings.AutoTest = this.AutoTestCheckbox.Checked;

			this.Settings.EnableScreenshots = this.ScreenshotsCheckbox.Checked;
			if (this.LevelSelectCheckbox.Checked) {
				this.Settings.LevelSelect = this.LevelCombobox.Text;
			} else {
				this.Settings.LevelSelect = null;
			}
			this.Settings.SkipCutscenes = this.SkipCutscenesCheckbox.Checked;

			bool complete = true;
			if (this.NoCdCheckbox.CheckState == CheckState.Checked) {
				string file = Path.Combine(this.Game.GamePath, "CD.ID");
				if (!File.Exists(file)) {
					using FileStream stream = new(file,
						FileMode.Create, FileAccess.Write, FileShare.None);
					using StreamWriter writer = new(stream);
					await writer.WriteAsync(CdId);
				}

				using (FileStream stream = new(Path.Combine(this.Game.GamePath, "DRIVE.CD"),
					FileMode.Create, FileAccess.Write, FileShare.None)) {

					using StreamWriter writer = new(stream);
					if (this.RunNative.Checked || !DarkForces.IsValidDosBoxPath(this.dosBoxPath)) {
						await writer.WriteAsync(char.ToUpper(Application.ExecutablePath[0]));
					} else {
						await writer.WriteAsync('C');
					}
				}

				file = Path.Combine(Path.GetPathRoot(Application.ExecutablePath), "CD.ID");
				if (!File.Exists(file)) {
					try {
						await InstallCdId();
					} catch (UnauthorizedAccessException) {
						Program.RunElevated("cdid", true);
					} catch (Exception ex) {
						MessageBox.Show(this, $"Unable to change CD check setting: {ex}",
							"Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
						complete = false;
					}
				}
			} else if (this.NoCdCheckbox.CheckState == CheckState.Unchecked) {
				string file = Path.Combine(this.Game.GamePath, "CD.ID");
				if (File.Exists(file)) {
					await Task.Run(() => File.Delete(file));
				}

				char cd = 'D';
				if (this.RunNative.Checked || !DarkForces.IsValidDosBoxPath(this.dosBoxPath)) {
					char pending = DarkForces.FirstCdDrive;
					if (pending != '\0') {
						cd = pending;
					}
				}

				using (FileStream stream = new(Path.Combine(this.Game.GamePath, "DRIVE.CD"),
					FileMode.Create, FileAccess.Write, FileShare.None)) {

					using StreamWriter writer = new(stream);
					await writer.WriteAsync(cd);
				}

				file = Path.Combine(Path.GetPathRoot(Application.ExecutablePath), "CD.ID");
				if (File.Exists(file)) {
					try {
						await RemoveCdId();
					} catch (UnauthorizedAccessException) {
						Program.RunElevated("cdid", false);
					} catch (Exception ex) {
						MessageBox.Show(this, $"Unable to change CD check setting: {ex}",
							"CD Check Setting Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
						complete = false;
					}
				}
			}

			if (this.ForceCdCheckbox.Checked) {
				this.Settings.CdDrive = this.CdUpDown.Text[0];
			} else {
				this.Settings.CdDrive = '\0';
			}

			Program.Settings.Save();

			if (complete) {
				await base.Apply();
			}
		}

		private void BrowseDosBox_Click(object sender, EventArgs e) {
			using OpenFileDialog dialog = new() {
				AddExtension = true,
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				CheckFileExists = true,
				CheckPathExists = true,
				DefaultExt = "exe",
				DereferenceLinks = true,
				Filter = "DOSBox (dosbox*.exe)|dosbox*.exe|Applications (*.exe)|*.exe|All Files (*.*)|*.*",
				FilterIndex = 0,
				InitialDirectory = this.dosBoxPath,
				Multiselect = false,
				RestoreDirectory = true,
				ShowHelp = false,
				ShowHiddenFiles = false,
				ShowPinnedPlaces = true,
				ShowPreview = false,
				ShowReadOnly = false,
				SupportMultiDottedExtensions = true,
				Title = "Locate DOSBox",
				ValidateNames = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,				
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = this.dosBoxPath,
				EnsureFileExists = true,
				EnsurePathExists = true,
				EnsureValidNames = true,
				Filters = {
					new CommonFileDialogFilter("DOSBox (dosbox*.exe)", "dosbox*.exe"),
					new CommonFileDialogFilter("Applications (*.exe)", "*.exe"),
					new CommonFileDialogFilter("All Files (*.*)", "*.*")
				},
				InitialDirectory = null,
				IsFolderPicker = false,
				Multiselect = false,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = $"Locate DOSBox"
			};*/

			if (dialog.ShowDialog(this) == DialogResult.OK) {
			//if (dialog.ShowDialog(this.Handle) == CommonFileDialogResult.Ok) {
				if (DarkForces.IsValidDosBoxPath(dialog.FileName)) {
					this.dosBoxPath = dialog.FileName;
					this.EnableApply();
				} else {
					MessageBox.Show(this,
						"DOSBox was not found in that location. This change will not be saved.",
						"Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);

					this.EnsureValidRunSelection();
				}
			}
		}

		private void BrowseDarkXl_Click(object sender, EventArgs e) {
			using OpenFileDialog dialog = new() {
				AddExtension = true,
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				CheckFileExists = true,
				CheckPathExists = true,
				DefaultExt = "exe",
				DereferenceLinks = true,
				Filter = "Valid Applications|darkxl.exe;TheForceEngine.exe;khonsu_Shipping_Steam_x64.exe|Dark XL (darkxl.exe)|darkxl.exe|The Force Engine (TheForceEngine.exe)|TheForceEngine.exe|Remaster (khonsu_Shipping_Steam_x64.exe)|khonsu_Shipping_Steam_x64.exe|Applications (*.exe)|*.exe|All Files (*.*)|*.*",
				FilterIndex = 0,
				InitialDirectory = this.darkXlPath,
				Multiselect = false,
				RestoreDirectory = true,
				ShowHelp = false,
				ShowHiddenFiles = false,
				ShowPinnedPlaces = true,
				ShowPreview = false,
				ShowReadOnly = false,
				SupportMultiDottedExtensions = true,
				Title = "Locate Dark XL / The Force Engine / Remaster",
				ValidateNames = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = this.darkXlPath,
				EnsureFileExists = true,
				EnsurePathExists = true,
				EnsureValidNames = true,
				Filters = {
					new CommonFileDialogFilter("Valid Applications", "darkxl.exe;TheForceEngine.exe;khonsu_Shipping_Steam_x64.exe"),
					new CommonFileDialogFilter("Dark XL (darkxl.exe)", "darkxl.exe"),
					new CommonFileDialogFilter("The Force Engine (TheForceEngine.exe)", "TheForceEngine.exe"),
					new CommonFileDialogFilter("Remaster (khonsu_Shipping_Steam_x64.exe)", "khonsu_Shipping_Steam_x64.exe"),
					new CommonFileDialogFilter("Applications (*.exe)", "*.exe"),
					new CommonFileDialogFilter("All Files (*.*)", "*.*")
				},
				InitialDirectory = null,
				IsFolderPicker = false,
				Multiselect = false,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = $"Locate Dark XL / The Force Engine / Remaster"
			};*/

			if (dialog.ShowDialog(this) == DialogResult.OK) {
			// (dialog.ShowDialog(this.Handle) == CommonFileDialogResult.Ok) {
				if (DarkForces.IsValidDarkXlPath(dialog.FileName)) {
					this.darkXlPath = dialog.FileName;
					this.EnableApply();
				} else {
					MessageBox.Show(this,
						"Dark XL, The Force Engine, and the Remaster were not found in that location. This change will not be saved.",
						"Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);

					this.EnsureValidRunSelection();
				}
			}
		}

		private void BrowseMods_Click(object sender, EventArgs e) {
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
			}*/;

			if (dialog.ShowDialog(this) == DialogResult.OK) {
			//if (dialog.ShowDialog(this.Handle) == CommonFileDialogResult.Ok) {
				this.modPath = dialog.SelectedPath;
				//this.modPath = dialog.FileName;
				this.EnableApply();
			}
		}

		private async void IMuseButton_Click(object sender, EventArgs e) {
			if (this.ApplyEnabled) {
				await this.Apply();
			}

			this.Game.RunIMuse();
		}

		private async void SetupButton_Click(object sender, EventArgs e) {
			if (this.ApplyEnabled) {
				await this.Apply();
			}

			this.Game.RunSetup();
		}

		private void ForceCdCheckbox_CheckedChanged(object sender, EventArgs e) {
			this.CdUpDown.Enabled = (sender as CheckBox).Checked;
			this.EnableApply();
		}

		private void LevelSelectCheckbox_CheckedChanged(object sender, EventArgs e) {
			this.LevelCombobox.Enabled = (sender as CheckBox).Checked;
			this.EnableApply();
		}

		private void RunDosBox_CheckedChanged(object sender, EventArgs e) {
			if (!UserInputBlocker.IsUserInput) {
				return;
			}

			if ((sender as RadioButton).Checked && this.dosBoxPath == null) {
				this.BrowseDosBox.PerformClick();
			}

			this.EnableApply();
		}

		private void RunDarkXl_CheckedChanged(object sender, EventArgs e) {
			if (!UserInputBlocker.IsUserInput) {
				return;
			}

			if ((sender as RadioButton).Checked && this.darkXlPath == null) {
				this.BrowseDarkXl.PerformClick();
			}

			this.EnableApply();
		}

		private void Changed(object sender, EventArgs e) =>
			this.EnableApply();
	}
}

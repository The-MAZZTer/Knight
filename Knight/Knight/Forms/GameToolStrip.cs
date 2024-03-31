using MZZT.Knight.Games;

namespace MZZT.Knight.Forms {
	public class GameToolStrip : ToolStrip {
		public GameToolStrip(Game game) : base() {
			this.Game = game;

			this.Game.EnabledChanged += this.Game_EnabledChanged;
			this.Game.BeginFindMods += this.Game_BeginFindMods;
			this.Game.EndFindMods += this.Game_EndFindMods;
			this.Game.BeginFindPatches += this.Game_BeginFindPatches;
			this.Game.EndFindPatches += this.Game_EndFindPatches;
			this.Game.GamePathChanged += this.Game_GamePathChanged;

			this.Dock = DockStyle.Top;
			this.GripStyle = ToolStripGripStyle.Hidden;
			this.Padding = new Padding(3, 0, 3, 0);

			this.NameLabel.ActiveLinkColor = this.NameLabel.ForeColor;
			this.NameLabel.Enabled = false;
			this.NameLabel.Image = game.SmallIcon;
			this.NameLabel.IsLink = true;
			this.NameLabel.LinkBehavior = LinkBehavior.NeverUnderline;
			this.NameLabel.LinkColor = this.NameLabel.ForeColor;
			this.NameLabel.Overflow = ToolStripItemOverflow.Never;
			this.NameLabel.Text = game.Name;

			this.PlayButton.Alignment = ToolStripItemAlignment.Right;
			this.PlayButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.PlayButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "play_arrow", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.PlayButton.Visible = false;

			this.PlayMultiplayerButton.Alignment = ToolStripItemAlignment.Right;
			this.PlayMultiplayerButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.PlayMultiplayerButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "group", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.PlayMultiplayerButton.Visible = false;

			this.ApplyButton.Enabled = false;
			this.ApplyButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "done", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.ApplyButton.Overflow = ToolStripItemOverflow.Always;
			this.ApplyButton.Visible = false;

			this.OptionsButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "settings", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.OptionsButton.Overflow = ToolStripItemOverflow.Always;
			this.OptionsButton.Visible = false;

			this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.RefreshButton.Overflow = ToolStripItemOverflow.Always;
			this.RefreshButton.Visible = false;

			this.ImportPatchButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "extension", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.ImportPatchButton.Overflow = ToolStripItemOverflow.Always;
			this.ImportPatchButton.Visible = false;

			this.SetPathButton.Alignment = ToolStripItemAlignment.Right;
			this.SetPathButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.SetPathButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "folder_managed", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.SetPathButton.Visible = false;

			this.EnableButton.Alignment = ToolStripItemAlignment.Right;
			this.EnableButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.EnableButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "done", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.EnableButton.Visible = false;

			this.Items.AddRange(new ToolStripItem[] { this.NameLabel, this.PlayButton,
				this.PlayMultiplayerButton, this.ApplyButton, this.OptionsButton,
				this.RefreshButton, this.ImportPatchButton, this.SetPathButton,
				this.EnableButton });

			this.UpdateState();

			this.ApplyButton.Click += this.ApplyButton_Click;
			this.EnableButton.Click += this.EnableButton_Click;
			this.ImportPatchButton.Click += this.ImportPatchButton_Click;
			this.OptionsButton.Click += this.OptionsButton_Click;
			this.OverflowButton.DropDownOpening += this.OverflowButton_DropDownOpening;
			this.OverflowButton.DropDownClosed += this.OverflowButton_DropDownClosed;
			this.PlayButton.Click += this.PlayButton_Click;
			this.PlayMultiplayerButton.Click += this.PlayMultiplayerButton_Click;
			this.RefreshButton.Click += this.RefreshButton_Click;
			this.SetPathButton.Click += this.SetPathButton_Click;
		}

		public Game Game { get; }

		private readonly ToolStripLabel NameLabel = new("");
		private readonly ToolStripButton PlayButton =
			new("Play");
		private readonly ToolStripButton PlayMultiplayerButton =
			new("Play Multiplayer");
		private readonly ToolStripButton ApplyButton =
			new("Apply Mods Now");
		private readonly ToolStripButton OptionsButton =
			new("Game Options");
		private readonly ToolStripButton RefreshButton =
			new("Refresh");
		private readonly ToolStripButton ImportPatchButton =
			new("Import Patch");
		private readonly ToolStripButton SetPathButton =
			new("Locate Game");
		private readonly ToolStripButton EnableButton =
			new("Enable Game Support");

		private void UpdateState() {
			if (this.InvokeRequired) {
				this.Invoke(new Action(this.UpdateState));
				return;
			}

			string game = base.Text ?? "Game";

			this.SetPathButton.Text = $"Locate {game}";

			if (this.Game.Enabled) {
				this.EnableButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "close", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				this.EnableButton.Text = $"Disable {game} Support";
			} else {
				this.EnableButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "done", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				this.EnableButton.Text = $"Enable {game} Support";
			}

			if (string.IsNullOrEmpty(this.Game.GamePath)) {
				this.NameLabel.Enabled = false;
				this.PlayButton.Visible = false;
				this.PlayMultiplayerButton.Visible = false;
				this.ApplyButton.Visible = false;
				this.OptionsButton.Visible = false;
				this.RefreshButton.Visible = false;
				this.ImportPatchButton.Visible = false;

				this.SetPathButton.Alignment = ToolStripItemAlignment.Right;
				this.SetPathButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.SetPathButton.Overflow = ToolStripItemOverflow.AsNeeded;
				this.SetPathButton.Visible = true;

				this.EnableButton.Visible = false;
				return;
			}

			if (!this.Game.Enabled) {
				this.NameLabel.Enabled = false;
				this.PlayButton.Visible = false;
				this.PlayMultiplayerButton.Visible = false;
				this.ApplyButton.Visible = false;
				this.OptionsButton.Visible = false;
				this.RefreshButton.Visible = false;
				this.ImportPatchButton.Visible = false;

				this.SetPathButton.Alignment = ToolStripItemAlignment.Right;
				this.SetPathButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.SetPathButton.Overflow = ToolStripItemOverflow.AsNeeded;
				this.SetPathButton.Visible = true;

				this.EnableButton.Alignment = ToolStripItemAlignment.Right;
				this.EnableButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
				this.EnableButton.Overflow = ToolStripItemOverflow.AsNeeded;
				this.EnableButton.Visible = true;
				return;
			}

			this.NameLabel.Enabled = true;
			this.PlayButton.Visible = true;

			if (this.Game.HasSeparateMultiplayerBinary) {
				this.PlayMultiplayerButton.Visible = true;
				this.PlayButton.Text = "Play Single Player";
			} else {
				this.PlayMultiplayerButton.Visible = false;
				this.PlayButton.Text = "Play";
			}

			this.ApplyButton.Visible = true;
			this.OptionsButton.Visible = true;
			this.RefreshButton.Visible = true;

			if (this.Game.IsFindingMods || this.Game.IsFindingPatches) {
				this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "cancel", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				this.RefreshButton.Text = "Abort Refreshing";
			} else {
				this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				this.RefreshButton.Text = "Refresh";
			}

			this.ImportPatchButton.Visible = this.Game.SupportsPatches;

			this.SetPathButton.Alignment = ToolStripItemAlignment.Left;
			this.SetPathButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
			this.SetPathButton.Overflow = ToolStripItemOverflow.Always;
			this.SetPathButton.Visible = true;

			this.EnableButton.Alignment = ToolStripItemAlignment.Left;
			this.EnableButton.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
			this.EnableButton.Overflow = ToolStripItemOverflow.Always;
			this.EnableButton.Visible = true;
		}

		protected override void OnMouseUp(MouseEventArgs mea) {
			base.OnMouseUp(mea);

			if (!this.Game.Enabled) {
				return;
			}

			ToolStripItem item = this.GetItemAt(mea.Location);
			if (item != null && item != this.NameLabel) {
				return;
			}

			(this.Parent as CollapsibleGameListView).Expand();
		}

		private async void ApplyButton_Click(object sender, EventArgs e) =>
			await ModActivator.SetActivatedMods(this.Game,
				(this.Parent as CollapsibleGameListView).SelectedMods);

		private void EnableButton_Click(object sender, EventArgs e) {
			this.Game.Enabled = !this.Game.Enabled;
			this.UpdateState();

			if (this.Game.Enabled) {
				(this.Parent as CollapsibleGameListView).Expand();
			}
		}

		private async void ImportPatchButton_Click(object sender, EventArgs e) {
			using OpenFileDialog dialog = new() {
				AddExtension = true,
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				CheckFileExists = true,
				CheckPathExists = true,
				CustomPlaces = {
					this.Game.GamePath,
					this.Game.PatchPath,
					this.Game.ModPath,
					this.Game.ActiveModPath
				},
				DefaultExt = "exe",
				DereferenceLinks = true,
				Filter = "Applications (*.exe)|*.exe|All Files (*.*)|*.*",
				FilterIndex = 0,
				InitialDirectory = this.Game.GamePath,
				Multiselect = true,
				RestoreDirectory = true,
				ShowHelp = false,
				ShowHiddenFiles = false,
				ShowPinnedPlaces = true,
				ShowPreview = false,
				ShowReadOnly = false,
				SupportMultiDottedExtensions = true,
				Title = "Import Patches - Knight",
				ValidateNames = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = this.Game.GamePath,
				DefaultExtension = "exe",
				EnsureFileExists = true,
				EnsurePathExists = true,
				EnsureValidNames = true,
				DefaultFileName = "",
				Multiselect = true,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = "Import Patches - Knight"
			};
			dialog.AddPlace(this.Game.GamePath, FileDialogAddPlaceLocation.Top);
			dialog.AddPlace(this.Game.PatchPath, FileDialogAddPlaceLocation.Top);
			dialog.AddPlace(this.Game.ModPath, FileDialogAddPlaceLocation.Top);
			dialog.AddPlace(this.Game.ActiveModPath, FileDialogAddPlaceLocation.Top);
			dialog.Filters.Add(new CommonFileDialogFilter(
				"Applications (*.exe)", "*.exe"));
			dialog.Filters.Add(new CommonFileDialogFilter(
				"All Files (*.*)", "*.*"));*/

			if (dialog.ShowDialog(this.TopLevelControl) != DialogResult.OK) {
			//if (dialog.ShowDialog() != CommonFileDialogResult.Ok) {
				return;
			}

			bool duplicates = false;
			foreach (string file in dialog.FileNames) {
				PatchInfo info = PatchCache.HashFile("", file);
				PatchInfo identified = Program.PatchCache.IdentifyFile(info.Hash, info.Size);
				if (identified != null) {
					if (Program.PatchCache.HasPatch(identified.FileName)) {
						duplicates = true;
						continue;
					}
				} else {
					identified = new PatchInfo() {
						FileName = info.FileName,
						Hash = info.Hash,
						Name = "Unknown Imported Patch",
						Size = info.Size
					};
				}

				(this.Parent as CollapsibleGameListView)
					.AddPatch(await this.Game.InstallPatch(file, identified));
			}

			if (duplicates) {
				MessageBox.Show(this.FindForm(),
					"One or more of those patches is already installed and has been skipped.",
					"Already Installed - Knight", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void OptionsButton_Click(object sender, EventArgs e) =>
			this.Game.OptionsForm.ShowDialog(this.FindForm());

		private void OverflowButton_DropDownOpening(object sender, EventArgs e) {
			Size size = Size.Empty;
			ToolStripItem[] items = this.Items.Cast<ToolStripItem>().Where(x => x.IsOnOverflow).ToArray();
			foreach (ToolStripItem item in items) {
				item.AutoSize = false;
				item.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
				item.ImageAlign = ContentAlignment.MiddleLeft;
				item.TextAlign = ContentAlignment.MiddleLeft;

				Size itemSize = item.GetPreferredSize(Size.Empty);
				size.Width = Math.Max(size.Width, itemSize.Width);
				size.Height = Math.Max(size.Height, itemSize.Height);
			}

			foreach (ToolStripItem item in items) {
				item.Size = size;
			}
		}

		private void OverflowButton_DropDownClosed(object sender, EventArgs e) {
			foreach (ToolStripItem item in this.Items.Cast<ToolStripItem>().Where(x => x.IsOnOverflow)) {
				item.AutoSize = true;
				item.ImageAlign = ContentAlignment.MiddleCenter;
				item.TextAlign = ContentAlignment.MiddleCenter;

				if (item.Alignment == ToolStripItemAlignment.Right) {
					item.DisplayStyle = ToolStripItemDisplayStyle.Image;
				}
			}
		}

		private async void PlayButton_Click(object sender, EventArgs e) =>
			await ModActivator.PlaySingleplayer(this.Game,
				(this.Parent as CollapsibleGameListView).SelectedMods);

		private async void PlayMultiplayerButton_Click(object sender, EventArgs e) =>
			await ModActivator.PlayMultiplayer(this.Game,
				(this.Parent as CollapsibleGameListView).SelectedMods);

		private void RefreshButton_Click(object sender, EventArgs e) {
			if (!this.Game.IsFindingMods && !this.Game.IsFindingPatches) {
				this.Game.FindMods();
				if (this.Game.SupportsPatches) {
					this.Game.FindPatches();
				}

				(this.Parent as CollapsibleGameListView).Expand();
			} else {
				if (this.Game.IsFindingMods) {
					this.Game.CancelFindMods();
				}
				if (this.Game.IsFindingPatches) {
					this.Game.CancelFindPatches();
				}
			}
		}
		
		private void SetPathButton_Click(object sender, EventArgs e) {
			using FolderBrowserDialog dialog = new() {
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				Description = $"Locate {this.Game.Name}'s Directory",
				InitialDirectory = this.Game.GamePath ?? Path.GetDirectoryName(Application.ExecutablePath),
				ShowHiddenFiles = false,
				ShowNewFolderButton = false,
				ShowPinnedPlaces = true,
				UseDescriptionForTitle = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = this.Game.GamePath ?? Path.GetDirectoryName(Application.ExecutablePath),
				EnsurePathExists = true,
				EnsureValidNames = true,
				InitialDirectory = null,
				IsFolderPicker = true,
				Multiselect = false,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = $"Locate {this.Game.Name}'s Directory"
			};*/

			if (dialog.ShowDialog(this.FindForm()) != DialogResult.OK) {
			//if (dialog.ShowDialog(this.FindForm().Handle) != CommonFileDialogResult.Ok) {
				return;
			}

			if (!this.Game.IsValidGamePath(dialog.SelectedPath)) {
			//if (!this.Game.IsValidGamePath(dialog.FileName)) {
				MessageBox.Show(this, $"Can't find {this.Game.Name} in that folder.",
					"Game Path - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			this.Game.CustomGamePath = dialog.SelectedPath;
			//this.Game.CustomGamePath = dialog.FileName;

			(this.Parent as CollapsibleGameListView).Expand();

			if (this.Game.IsFindingMods || this.Game.IsFindingPatches) {
				if (this.Game.IsFindingMods) {
					this.Game.CancelFindMods();
				}
				if (this.Game.IsFindingPatches) {
					this.Game.CancelFindPatches();
				}
			}

			this.Game.FindMods();
			if (this.Game.SupportsPatches) {
				this.Game.FindPatches();
			}
		}

		private void Game_BeginFindMods(object sender, EventArgs e) =>
			this.UpdateState();
		private void Game_EndFindMods(object sender, EventArgs e) =>
			this.UpdateState();
		private void Game_BeginFindPatches(object sender, EventArgs e) =>
			this.UpdateState();
		private void Game_EndFindPatches(object sender, EventArgs e) =>
			this.UpdateState();

		private void Game_EnabledChanged(object sender, EventArgs e) =>
			this.UpdateState();
		private void Game_GamePathChanged(object sender, EventArgs e) =>
			this.UpdateState();

		public void OnApplyModsChanged(IEnumerable<Mod> mods) {
			this.ApplyButton.Enabled = ModActivator.GetModActivationChangesPending(this.Game, mods);
		}
	}
}

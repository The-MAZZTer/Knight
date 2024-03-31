using MZZT.Input;
using MZZT.Knight.Games;
using System.ComponentModel;
using static MZZT.WinApi.PInvoke.User32;

namespace MZZT.Knight.Forms {
	public class GameListView : ListView {
		public GameListView(Game game) : base() {
			this.Game = game;

			this.Game.BeginFindMods += this.Game_BeginFindMods;
			this.Game.ModFound += this.Game_ModFound;
			this.Game.EndFindMods += this.Game_EndFindMods;
			this.Game.BeginFindPatches += this.Game_BeginFindPatches;
			this.Game.PatchFound += this.Game_PatchFound;
			this.Game.EndFindPatches += this.Game_EndFindPatches;
			this.Game.EnabledChanged += this.Game_EnabledChanged;
			this.Game.ModPathChanged += this.Game_ModPathChanged;
			this.Game.PatchPathChanged += this.Game_PatchPathChanged;

			this.BeginUpdate();
			this.AutoArrange = true;
			this.BorderStyle = BorderStyle.None;
			this.CheckBoxes = game.AllowMultipleActiveMods;

			this.NameColumn.Text = "Name";
			this.Columns.Add(this.NameColumn);

			this.Dock = DockStyle.Fill;
			this.FullRowSelect = true;
			this.Groups.AddRange(new[] { this.PatchGroup, this.ModGroup });
			this.HeaderStyle = ColumnHeaderStyle.None;
			this.HideSelection = !game.AllowMultipleActiveMods;
			this.LabelEdit = true;
			this.LabelWrap = true;
			this.ListViewItemSorter = new ModAndPatchComparer();
			this.MultiSelect = game.AllowMultipleActiveMods;
			this.ShowGroups = game.SupportsPatches;
			this.ShowItemToolTips = true;
			this.Sorting = SortOrder.Ascending;
			this.Text = game.Name;
			this.TileSize = new Size(150, 48);
			this.UseCompatibleStateImageBehavior = false;
			this.View = Program.Settings.View;

			this.NoModsItem.ImageKey = this.DefaultImageKey;

			this.AddDefaultEntry();

			this.IconViewButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "grid_view", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.IconViewButton.ShowShortcutKeys = false;
			this.ListViewButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "view_list", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.ListViewButton.ShowShortcutKeys = false;
			this.ListViewMenu.Items.AddRange(new[] {
				this.IconViewButton, this.ListViewButton
			});

			this.RenameButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "text_select_start", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.RenameButton.ShowShortcutKeys = false;
			this.PropertiesButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "contact_edit", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.PropertiesButton.ShowShortcutKeys = false;
			this.ListViewItemMenu.Items.AddRange(new[] {
				this.RenameButton, this.PropertiesButton
			});

			this.ListViewMenu.Opening += this.ListViewMenu_Opening;
			this.IconViewButton.Click += this.IconViewButton_Click;
			this.ListViewButton.Click += this.ListViewButton_Click;

			this.RenameButton.Click += this.RenameButton_Click;
			this.PropertiesButton.Click += this.PropertiesButton_Click;
		}

		public Game Game { get; }

		private readonly ColumnHeader NameColumn = new();

		private readonly ListViewGroup ModGroup = new("Mods", "Mods");
		private readonly ListViewGroup PatchGroup = new("Patches", "Patches");

		private readonly ListViewItem NoModsItem = new("No Mods");

		private readonly ContextMenuStrip ListViewMenu = new();
		private readonly ToolStripMenuItem IconViewButton =
			new("&Icon View");
		private readonly ToolStripMenuItem ListViewButton =
			new("&List View");

		private readonly ContextMenuStrip ListViewItemMenu = new();
		private readonly ToolStripMenuItem RenameButton =
			new("&Rename");
		private readonly ToolStripMenuItem PropertiesButton =
			new("&Properties");

		public IEnumerable<Mod> ActivatedMods =>
			this.Items.Cast<ListViewItem>()
				.Where(x => x != null && x.Tag is Mod && (x.Checked || (!this.CheckBoxes && x.Selected)))
				.Select(x => x.Tag as Mod);

		public IEnumerable<Patch> ActivatedPatches =>
			this.Items.Cast<ListViewItem>()
				.Where(x => x != null && x.Tag is Patch && (x.Checked || (!this.CheckBoxes && x.Selected)))
				.Select(x => x.Tag as Patch);

		public string DefaultImageKey => $"|{this.Game.GetType().Name}";

		public override string Text {
			get => base.Text;
			set {
				base.Text = value;
				this.NoModsItem.Text = value;
			}
		}

		public void AddDefaultEntry() {
			using (new UserInputBlocker()) {
				this.NoModsItem.Checked = true;
				this.NoModsItem.Group = this.ModGroup;
				this.Items.Add(this.NoModsItem);

				if (this.Created && this.Height > 0) {
					this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				}
			}
		}

		protected override void OnCreateControl() {
			base.OnCreateControl();

			if (this.Created && this.Height > 0) {
				this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		protected override void OnAfterLabelEdit(LabelEditEventArgs e) {
			base.OnAfterLabelEdit(e);

			if (e.CancelEdit || string.IsNullOrWhiteSpace(e.Label)) {
				e.CancelEdit = true;
				return;
			}

			ListViewItem item = this.Items[e.Item];
			if (item.Tag == null) {
				e.CancelEdit = true;
				return;
			}

			if (item.Tag is Patch patch) {
				patch.Name = e.Label;
			} else if (item.Tag is Mod mod) {
				mod.Name = e.Label;
			}

			_ = this.WaitToResizeColumns();
		}

		private async Task WaitToResizeColumns() {
			await Task.Delay(1);

			if (this.Created && this.Height > 0) {
				this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		protected override void OnBeforeLabelEdit(LabelEditEventArgs e) {
			base.OnBeforeLabelEdit(e);

			if (this.Items[e.Item].Tag == null) {
				e.CancelEdit = true;
			}
		}

		protected override void OnHandleCreated(EventArgs e) {
			base.OnHandleCreated(e);

			Size size = Program.Settings.LargeIconSize;
			SendMessage(this.Handle, WM.LVM_SETICONSPACING, IntPtr.Zero,
				new IntPtr((size.Height * 0x10000) + (size.Width & 0xFFFF)));
		}

		protected override void OnItemActivate(EventArgs e) {
			base.OnItemActivate(e);

			if (this.CheckBoxes) {
				return;
			}

			if (this.SelectedIndices.Count == 0) {
				return;
			}

			ListViewItem item = this.SelectedItems.Cast<ListViewItem>().First();
			if (item.Tag is not Mod mod) {
				return;
			}

			_ = ModActivator.Play(this.Game, [mod]);
		}

		protected override void OnItemCheck(ItemCheckEventArgs ice) {
			if (!UserInputBlocker.IsUserInput) {
				base.OnItemCheck(ice);
				return;
			}

			using (new UserInputBlocker()) {
				ListViewItem selectedItem = this.Items[ice.Index];
				if (selectedItem.Tag == null) {
					ice.NewValue = CheckState.Checked;
					if (ice.CurrentValue == CheckState.Checked) {
						base.OnItemCheck(ice);
						return;
					}

					foreach (ListViewItem item in this.Items.Cast<ListViewItem>()
						.Where(x => x != selectedItem && x.Tag is not Patch)) {

						item.Checked = false;
					}
				} else if (selectedItem.Tag is Patch) {
					ice.NewValue = CheckState.Checked;
				} else if (ice.NewValue == CheckState.Checked) {
					this.NoModsItem.Checked = false;
				} else if (!this.Items.Cast<ListViewItem>()
					.Any(x => x != selectedItem && x != null && x.Tag is Mod && x.Checked)) {

					this.NoModsItem.Checked = true;
				}
			}

			base.OnItemCheck(ice);
		}

		protected override void OnItemChecked(ItemCheckedEventArgs e) {
			base.OnItemChecked(e);

			if (!this.CheckBoxes || !UserInputBlocker.IsUserInput) {
				return;
			}

			if (e.Item.Tag is Patch patch) {
				this.BeginUpdate();
				using (new UserInputBlocker()) {
					foreach (ListViewItem item in this.Items.Cast<ListViewItem>()
						.Where(x => x != e.Item && x.Tag is Patch)) {

						_ = (item.Tag as Patch).Deactivate();
						item.Checked = false;
					}
				}
				this.EndUpdate();

				_ = patch.Activate();
				return;
			}

			(this.Parent as CollapsibleGameListView).OnApplyModsChanged();
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);

			if ((e.Button & MouseButtons.Right) == 0) {
				return;
			}

			ListViewItem item = this.HitTest(e.Location).Item;
			if (item == null) {
				this.ListViewMenu.Show(this, e.Location);
				return;
			}

			if (item.Tag == null) {
				return;
			}

			this.PropertiesButton.Visible = (item.Tag as Mod)?.HasProperties ?? false;
			this.ListViewItemMenu.Tag = item;
			this.ListViewItemMenu.Show(this, e.Location);
		}

		protected override void OnSelectedIndexChanged(EventArgs e) {
			base.OnSelectedIndexChanged(e);

			if (this.CheckBoxes || !UserInputBlocker.IsUserInput) {
				return;
			}

			(this.Parent as CollapsibleGameListView).OnApplyModsChanged();
		}

		private void ListViewMenu_Opening(object sender, CancelEventArgs e) {
			this.IconViewButton.Checked = this.View == View.LargeIcon;
			this.ListViewButton.Checked = this.View == View.Details;
		}

		private void IconViewButton_Click(object sender, EventArgs e) {
			Program.Settings.View = View.LargeIcon;
			Program.Settings.Save();
		}

		private void ListViewButton_Click(object sender, EventArgs e) {
			Program.Settings.View = View.Details;
			Program.Settings.Save();
		}

		private void RenameButton_Click(object sender, EventArgs e) =>
			((sender as ToolStripMenuItem).Owner.Tag as ListViewItem).BeginEdit();

		private void PropertiesButton_Click(object sender, EventArgs e) =>
			(((sender as ToolStripMenuItem).Owner.Tag as ListViewItem).Tag as Mod)
			.PropertiesForm.ShowDialog(this.FindForm());

		private void Game_BeginFindMods(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_BeginFindMods(sender, e)));
				return;
			}

			this.BeginUpdate();
			using (new UserInputBlocker()) {
				foreach (ListViewItem item in this.ModGroup.Items.Cast<ListViewItem>().ToArray()) {
					if (item.Tag is Mod mod) {
						mod.CacheChanged -= this.Mod_CacheChanged;
						mod.NameChanged -= this.Mod_NameChanged;
					}

					this.Items.Remove(item);
				}
				this.AddDefaultEntry();
			}
			this.EndUpdate();
		}

		private void Game_ModFound(object sender, ModEventArgs e) {
			/*if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_ModFound(sender, e)));
				return;
			}

			this.BeginUpdate();
			using (new UserInputBlocker()) {
				ListViewItem item = new ListViewItem(e.Mod.Name) {
					Group = this.ModGroup,
					ImageKey = this.DefaultImageKey,
					Tag = e.Mod
				};
				if (this.Game.AllowMultipleActiveMods) {
					item.Checked = e.Mod.Active;
					if (item.Checked) {
						this.NoModsItem.Checked = false;
					}
				} else {
					item.Selected = e.Mod.Active;
				}
				this.Items.Add(item);
			}

			this.FixColumns();

			this.EndUpdate();

			e.Mod.CacheChanged += this.Mod_CacheChanged;
			e.Mod.NameChanged += this.Mod_NameChanged;*/
		}

		private void Game_EndFindMods(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_EndFindMods(sender, e)));
				return;
			}

			Game game = sender as Game;

			this.BeginUpdate();

			using (new UserInputBlocker()) {
				this.Items.AddRange(game.Mods.Values.Select(mod => {
					ListViewItem item = new(mod.Name) {
						Group = this.ModGroup,
						ImageKey = this.DefaultImageKey,
						Tag = mod
					};
					if (this.Game.AllowMultipleActiveMods) {
						item.Checked = mod.Active;
						if (item.Checked) {
							this.NoModsItem.Checked = false;
						}
					} else {
						item.Selected = mod.Active;
					}
					return item;
				}).ToArray());
			}

			this.FixColumns();

			this.EndUpdate();

			foreach (Mod mod in game.Mods.Values) {
				mod.CacheChanged += this.Mod_CacheChanged;
				mod.NameChanged += this.Mod_NameChanged;
			}
		}

		private void Game_BeginFindPatches(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_BeginFindPatches(sender, e)));
				return;
			}

			this.BeginUpdate();
			using (new UserInputBlocker()) {
				foreach (ListViewItem item in this.PatchGroup.Items.Cast<ListViewItem>().ToArray()) {
					this.Items.Remove(item);
				}
			}
			this.EndUpdate();
		}

		public void AddPatches(IEnumerable<Patch> patches) {
			this.BeginUpdate();

			using (new UserInputBlocker()) {
				this.Items.AddRange(patches.Select(patch => new ListViewItem(patch.Name) {
					Group = this.PatchGroup,
					ImageKey = this.DefaultImageKey,
					Tag = patch
				}).ToArray());
			}

			this.FixColumns();

			this.EndUpdate();
		}

		private void Game_PatchFound(object sender, PatchEventArgs e) {
			/*if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_PatchFound(sender, e)));
				return;
			}

			this.AddPatch(e.Patch);*/
		}


		private void Game_EndFindPatches(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_EndFindPatches(sender, e)));
				return;
			}

			Game game = sender as Game;
			this.AddPatches(game.Patches.Values);
		}

		private void Game_EnabledChanged(object sender, EventArgs e) {
			using (new UserInputBlocker()) {
				foreach (ListViewItem item in this.Items) {
					if (item.Tag is Mod mod) {
						mod.CacheChanged -= this.Mod_CacheChanged;
						mod.NameChanged -= this.Mod_NameChanged;
					}
				}

				this.Items.Clear();
			}
		}

		private void Mod_CacheChanged(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Mod_CacheChanged(sender, e)));
				return;
			}

			Mod mod = sender as Mod;

			ListViewItem item = this.Items.Cast<ListViewItem>().First(x => x.Tag == mod);
			if (item.Text == mod.Name) {
				return;
			}
			item.Text = mod.Name;

			this.FixColumns();

			this.Sort();
		}
		private void Mod_NameChanged(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Mod_NameChanged(sender, e)));
				return;
			}

			Mod mod = sender as Mod;

			ListViewItem item = this.Items.Cast<ListViewItem>().First(x => x.Tag == mod);
			if (item.Text == mod.Name) {
				return;
			}
			item.Text = mod.Name;

			this.FixColumns();

			this.Sort();
		}

		private void Game_ModPathChanged(object sender, EventArgs e) {
			if (this.Game.Enabled) {
				this.Game.CancelFindMods();
				this.Game.FindMods();
			}
		}
		private void Game_PatchPathChanged(object sender, EventArgs e) {
			if (this.Game.Enabled) {
				this.Game.CancelFindPatches();
				this.Game.FindPatches();
			}
		}

		public void FixColumns() {
			if (this.Created && this.Height > 0) {
				this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		protected override void OnVisibleChanged(EventArgs e) {
			base.OnVisibleChanged(e);

			if (this.Visible) {
				Program.Settings.ViewChanged += this.Settings_ViewChanged;
			} else {
				Program.Settings.ViewChanged -= this.Settings_ViewChanged;
			}
		}

		protected override void OnHandleDestroyed(EventArgs e) {
			base.OnHandleDestroyed(e);

			Program.Settings.ViewChanged -= this.Settings_ViewChanged;
		}

		private void Settings_ViewChanged(object sender, EventArgs e) {
			this.View = Program.Settings.View;
			this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}
	}

	public class ModAndPatchComparer : Comparer<ListViewItem> {
		public override int Compare(ListViewItem x, ListViewItem y) {
			if (x.Tag is Patch xPatch) {
				if (y.Tag is Patch yPatch) {
					if (xPatch.SortOrder != yPatch.SortOrder) {
						return xPatch.SortOrder - yPatch.SortOrder;
					}

					return x.Text.CompareTo(y.Text);
				} else {
					return -1;
				}
			} else if (y.Tag is Patch) {
				return 1;
			}

			if (x.Tag == null) {
				return -1;
			} else if (y.Tag == null) {
				return 1;
			}

			Mod xMod = x.Tag as Mod;
			Mod yMod = y.Tag as Mod;

			if (xMod.SortOrder != yMod.SortOrder) {
				return xMod.SortOrder - yMod.SortOrder;
			}

			return x.Text.CompareTo(y.Text);
		}
	}
}

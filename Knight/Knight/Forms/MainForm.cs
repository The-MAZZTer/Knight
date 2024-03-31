using MZZT.Forms;
using MZZT.Input;
using MZZT.Knight.Games;
using MZZT.Properties;
using System.Data;
using System.Diagnostics;

namespace MZZT.Knight.Forms {
	public partial class MainForm : Form {
		public MainForm() {
			this.InitializeComponent();

			this.Icon = Resources.Knight;
			Rectangle bounds = Program.Settings.FormBounds;
			if (bounds.X == int.MinValue && bounds.Y == int.MinValue) {
				this.StartPosition = FormStartPosition.WindowsDefaultLocation;
			} else {
				this.StartPosition = FormStartPosition.Manual;
				this.Location = bounds.Location;
			}
			this.ClientSize = bounds.Size;

			this.GameLists = Program.Games.Values
				.Select(x => {
					CollapsibleGameListView list = new(x, this.LargeImages, this.SmallImages);
					list.Collapsed += this.CollapsibleGameListView_Collapsed;
					list.Expanded += this.CollapsibleGameListView_Expanded;
					return list;
				})
				.ToArray();
			this.Controls.AddRange(this.GameLists);

			this.SimpleList.BeginUpdate();
			this.SimpleList.BringToFront();
			this.SimpleList.View = Program.Settings.View;
			this.SimpleList.ListViewItemSorter = new ModAndPatchComparer();

			if (Program.Settings.SimpleList) {
				this.SimpleList.Visible = true;
				if (this.SimpleList.Created && this.SimpleList.Visible) {
					this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				}
				this.SimpleList.EndUpdate();

				foreach (CollapsibleGameListView list in this.GameLists) {
					list.Visible = false;
				}

				this.PlayButton.Visible = true;
				this.PlayButton.Enabled = false;
			}

			int expanded = (int)Program.Settings.ExpandedGame;
			if (expanded > -1 && this.GameLists[expanded].Game.Enabled) {
				this.ExpandedGameList = this.GameLists[expanded];
				if (!Program.Settings.SimpleList) {
					this.ExpandedGameList.EndUpdate();
				}
			}

			this.PositionLists();

			this.SimpleList.Groups.AddRange(Program.Games.Values.Select(x => new ListViewGroup(x.Name, x.Name) {
				Tag = x
			}).ToArray());

			foreach (Game game in Program.Games.Values) {
				this.SmallImages.Images.Add($"|{game.GetType().Name}", game.SmallIcon);
				this.LargeImages.Images.Add($"|{game.GetType().Name}", game.LargeIcon);

				this.AddDefaultEntry(game);

				game.EnabledChanged += this.Game_EnabledChanged;
				game.BeginFindMods += this.Game_BeginFindMods;
				game.ModFound += this.Game_ModFound;
				game.EndFindMods += this.Game_EndFindMods;
				game.BeginFindPatches += this.Game_BeginFindPatches;
				game.EndFindPatches += this.Game_EndFindPatches;
				game.GamePathChanged += this.Game_GamePathChanged;
			}
			this.SimpleList.EndUpdate();

			this.GameEnabledMenuItems = Program.Games.Values.Select(x => {
				ToolStripMenuItem item = new($"&Enable {x.Name} Support", Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "done", SystemColors.MenuText, 16, GraphicsUnit.Pixel)) {
					ShowShortcutKeys = false
				};

				item.Click += this.GameEnabled_Click;

				return item;
			}).ToArray();

			this.GameOptionsMenuItems = Program.Games.Values.Select(x => {
				ToolStripMenuItem item = new("&Options", Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "settings", SystemColors.MenuText, 16, GraphicsUnit.Pixel)) {
					ShowShortcutKeys = false
				};

				item.Click += this.GameOptions_Click;

				return item;
			}).ToArray();

			this.GameRefreshMenuItems = Program.Games.Values.Select(x => {
				ToolStripMenuItem item = new("&Refresh", Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel)) {
					ShowShortcutKeys = false
				};

				item.Click += this.GameRefresh_Click;

				return item;
			}).ToArray();

			this.GamePathMenuItems = Program.Games.Values.Select(x => {
				ToolStripMenuItem item = new($"&Locate {x.Name}", Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "folder_managed", SystemColors.MenuText, 16, GraphicsUnit.Pixel)) {
					ShowShortcutKeys = false
				};

				item.Click += this.GamePath_Click;

				return item;
			}).ToArray();

			this.GameMenus = Program.Games.Values.Select((x, i) => {
				ToolStripMenuItem menu = new() {
					Image = x.SmallIcon,
					Tag = x,
					Text = x.MnemonicName,
					Visible = false
				};
				menu.DropDownItems.AddRange(new[] {
					this.GameOptionsMenuItems[i], this.GameRefreshMenuItems[i],
					this.GamePathMenuItems[i], this.GameEnabledMenuItems[i]
				});

				menu.DropDownOpening += this.GameMenu_DropDownOpening;

				return menu;
			}).ToArray();

			this.HelpToolbarButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "help", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.AboutButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "info", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.UpdateButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "globe", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.WebsiteButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "link", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.MassassiButton.Image = Resources.massassi;

			this.MassassiForumsButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "forum", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.MassassiIrcButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "chat", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.Df21Button.Image = Resources.site_df21;

			this.Df21ForumsButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "forum", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.Df21IrcButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "chat", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.OptionsButton.DropDownItems.AddRange(this.GameMenus);
			this.OptionsButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "settings", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.NoGroupingButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "list", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.GroupButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "table_rows", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.NothingOnRunButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "web_asset", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.MinimizeOnRunButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "minimize", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.QuitOnRunButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "close", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.PlayButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "play_arrow", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.PlaySingleplayerButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "play_arrow", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.PlayMultiplayerButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "group", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.IconViewButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "grid_view", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.ListViewButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "view_list", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.RenameButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "text_select_start", SystemColors.MenuText, 16, GraphicsUnit.Pixel);

			this.PropertiesButton.Image = Program.Glyphs.DrawBitmapGlyph(new(16, 16), "contact_edit", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
		}

		protected override void OnShown(EventArgs e) {
			base.OnShown(e);

			this.start = DateTime.Now;
			this.RefreshAll();
		}
		private DateTime start;

		private void RefreshAll() {
			IEnumerable<Game> list;
			if (Program.Settings.ExpandedGame > (SupportedGames)(-1) && this.ExpandedGameList != null) {
				list = new[] { this.ExpandedGameList.Game }.Concat(Program.Games.Values).Distinct();
			} else {
				list = Program.Games.Values;
			}

			foreach (Game game in list) {
				if (game.Enabled) {
					game.FindMods();
					if (game.SupportsPatches) {
						game.FindPatches();
					}
				}
			}
		}

		private readonly CollapsibleGameListView[] GameLists;
		private readonly ToolStripMenuItem[] GameMenus;
		private readonly ToolStripMenuItem[] GameEnabledMenuItems;
		private readonly ToolStripMenuItem[] GamePathMenuItems;
		private readonly ToolStripMenuItem[] GameRefreshMenuItems;
		private readonly ToolStripMenuItem[] GameOptionsMenuItems;

		protected override void OnVisibleChanged(EventArgs e) {
			base.OnVisibleChanged(e);

			if (this.Visible) {
				Program.Settings.ViewChanged += this.Settings_ViewChanged;
			} else {
				Program.Settings.ViewChanged -= this.Settings_ViewChanged;
			}
		}

		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);

			Program.Settings.ViewChanged -= this.Settings_ViewChanged;
		}

		private Rectangle[] CalculateListsBounds() {
			Rectangle[] bounds = new Rectangle[this.GameLists.Length];

			Rectangle available = this.ClientRectangle;
			available.Y += this.Toolbar.Height;
			available.Height -= this.Toolbar.Height;

			for (int i = 0; i < this.GameLists.Length &&
				this.GameLists[i] != this.ExpandedGameList; i++) {

				CollapsibleGameListView list = this.GameLists[i];
				list.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				bounds[i] = available;
				bounds[i].Height = list.CollapsedHeight;

				available.Y += bounds[i].Height;
				available.Height -= bounds[i].Height;
			}

			if (this.ExpandedGameList == null) {
				return bounds;
			}

			for (int i = this.GameLists.Length - 1;
				this.GameLists[i] != this.ExpandedGameList; i--) {

				CollapsibleGameListView list = this.GameLists[i];
				list.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				available.Height -= list.CollapsedHeight;
				bounds[i] = available;
				bounds[i].Y = available.Bottom;
				bounds[i].Height = list.CollapsedHeight;
			}

			this.ExpandedGameList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
				AnchorStyles.Left | AnchorStyles.Right;

			bounds[Array.IndexOf(this.GameLists, this.ExpandedGameList)] = available;
			return bounds;
		}

		private void PositionLists() {
			Rectangle[] bounds = this.CalculateListsBounds();
			for (int i = 0; i < bounds.Length; i++) {
				this.GameLists[i].Bounds = bounds[i];
			}
		}

		private CancellationTokenSource animateCancelled;
		private async Task Animate(double time) {
			this.animateCancelled?.Cancel();
			this.animateCancelled = new CancellationTokenSource();

			CancellationToken token = this.animateCancelled.Token;

			Rectangle[] startBounds = this.GameLists.Select(x => x.Bounds).ToArray();
			Rectangle[] endBounds = this.CalculateListsBounds();
			Size reference = this.ClientSize;
			DateTime startTime = DateTime.Now;
			bool fixedSize = false;

			double pos;
			do {
				await Task.Delay(10);
				if (token.IsCancellationRequested) {
					return;
				}
				if (this.ClientSize != reference) { // TODO Is this good enough?
					await this.Animate(time - (DateTime.Now - startTime).TotalMilliseconds);
					return;
				}

				pos = (double)(DateTime.Now - startTime).TotalMilliseconds / (double)time;
				if (this.SimpleList.Visible || pos > 1) {
					pos = 1;
				}

				for (int i = 0; i < this.GameLists.Length; i++) {
					this.GameLists[i].Bounds = new Rectangle(
						(int)(((endBounds[i].X - startBounds[i].X) * pos) + startBounds[i].X),
						(int)(((endBounds[i].Y - startBounds[i].Y) * pos) + startBounds[i].Y),
						(int)(((endBounds[i].Width - startBounds[i].Width) * pos) + startBounds[i].Width),
						(int)(((endBounds[i].Height - startBounds[i].Height) * pos) + startBounds[i].Height)
					);
				}

				if (!fixedSize) {
					this.ExpandedGameList?.FixColumns();
					fixedSize = true;
				}
			} while (pos < 1);
		}

		private async void CollapsibleGameListView_Expanded(object sender, EventArgs e) {
			if (this.ExpandedGameList == sender) {
				return;
			}
			this.ExpandedGameList = sender as CollapsibleGameListView;
			Program.Settings.ExpandedGame = (SupportedGames)Array.IndexOf(this.GameLists, this.ExpandedGameList);
			Program.Settings.Save();

			if (this.SimpleList.Visible) {
				this.PositionLists();
				this.ExpandedGameList.FixColumns();
			} else {
				await this.Animate(ANIMATION_TIME);
			}
		}
		private async void CollapsibleGameListView_Collapsed(object sender, EventArgs e) {
			if (this.ExpandedGameList != sender) {
				return;
			}
			this.ExpandedGameList = null;
			Program.Settings.ExpandedGame = (SupportedGames)(-1);
			Program.Settings.Save();

			if (this.SimpleList.Visible) {
				this.PositionLists();
			} else {
				await this.Animate(ANIMATION_TIME);
			}
		}
		private CollapsibleGameListView ExpandedGameList;
		private const int ANIMATION_TIME = 250;

		private void AddDefaultEntry(Game game) {
			ListViewItem item = new(game.Name, $"|{game.GetType().Name}",
				this.SimpleList.Groups[game.Name]);
			this.SimpleList.Items.Add(item);

			if (this.SimpleList.Created && this.SimpleList.Visible) {
				this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void Mod_CacheChanged(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Mod_CacheChanged(sender, e)));
				return;
			}

			Mod mod = sender as Mod;

			ListViewItem item = this.SimpleList.Items.Cast<ListViewItem>().First(x => x.Tag == mod);
			if (item.Text == mod.Name) {
				return;
			}
			item.Text = mod.Name;

			if (this.SimpleList.Created && this.SimpleList.Visible) {
				this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
			this.SimpleList.Sort();

			Program.ModCache.Save();
		}

		private void Mod_NameChanged(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Mod_NameChanged(sender, e)));
				return;
			}

			Mod mod = sender as Mod;

			ListViewItem item = this.SimpleList.Items.Cast<ListViewItem>().First(x => x.Tag == mod);
			if (item.Text == mod.Name) {
				return;
			}
			item.Text = mod.Name;

			if (this.SimpleList.Created && this.SimpleList.Visible) {
				this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
			this.SimpleList.Sort();

			Program.ModCache.Save();
		}

		private void ListViewMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
			this.IconViewButton.Checked = this.SimpleList.View == View.LargeIcon;
			this.ListViewButton.Checked = this.SimpleList.View == View.Details;
		}

		private void SimpleList_BeforeLabelEdit(object sender, LabelEditEventArgs e) {
			if (this.SimpleList.Items[e.Item].Tag == null) {
				e.CancelEdit = true;
			}
		}

		private async void SimpleList_AfterLabelEdit(object sender, LabelEditEventArgs e) {
			if (this.SimpleList.Items[e.Item].Tag == null) {
				e.CancelEdit = true;
			}

			if (e.CancelEdit || string.IsNullOrWhiteSpace(e.Label)) {
				return;
			}

			if (this.SimpleList.Items[e.Item].Tag is not Mod mod) {
				e.CancelEdit = true;
				return;
			}

			mod.Name = e.Label;

			await this.WaitToResizeColumns();
		}

		private async Task WaitToResizeColumns() {
			await Task.Delay(1);

			if (this.SimpleList.Created && this.SimpleList.Visible) {
				this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}

		private void SimpleList_ItemActivate(object sender, EventArgs e) {
			if (this.SimpleList.SelectedIndices.Count == 0) {
				return;
			}

			Game game = this.SimpleList.SelectedItems[0].Group.Tag as Game;

			_ = ModActivator.Play(game, this.SimpleList.SelectedItems[0].Tag is not Mod mod ? Array.Empty<Mod>() : [mod]);
		}

		private void SimpleList_MouseDown(object sender, MouseEventArgs e) {
			if ((e.Button & MouseButtons.Right) == 0) {
				return;
			}

			ListViewItem item = this.SimpleList.HitTest(e.Location).Item;
			if (item == null) {
				this.ListViewMenu.Show(this.SimpleList, e.Location);
				return;
			}

			if (item.Tag == null) {
				return;
			}

			this.PropertiesButton.Visible = (item.Tag as Mod).HasProperties;

			this.ListViewItemMenu.Tag = item;
			this.ListViewItemMenu.Show(this.SimpleList, e.Location);
		}

		private void SimpleList_SelectedIndexChanged(object sender, EventArgs e) {
			if (this.SimpleList.SelectedIndices.Count == 0) {
				this.PlaySingleplayerButton.Visible = false;
				this.PlayButton.Visible = true;
				this.PlayButton.Enabled = false;
			} else {
				this.PlayButton.Enabled = true;

				Game game = this.SimpleList.SelectedItems[0].Group.Tag as Game;
				this.PlaySingleplayerButton.Visible = game.HasSeparateMultiplayerBinary;
				this.PlayButton.Visible = !game.HasSeparateMultiplayerBinary;
			}
		}

		private void ClearGameMods(Game game) {
			this.SimpleList.BeginUpdate();

			ListViewGroup group = this.SimpleList.Groups[game.Name];
			foreach (ListViewItem item in group.Items.Cast<ListViewItem>().ToArray()) {
				if (item.Tag is Mod mod) {
					mod.CacheChanged -= this.Mod_CacheChanged;
					mod.NameChanged -= this.Mod_NameChanged;
				}
				this.SimpleList.Items.Remove(item);
			}

			this.SimpleList.EndUpdate();
		}

		private void Game_EnabledChanged(object sender, EventArgs e) {
			Game game = sender as Game;
			if (game.Enabled) {
				game.FindMods();
				if (game.SupportsPatches) {
					game.FindPatches();
				}
			} else {
				game.CancelFindMods();
				game.CancelFindPatches();

				this.ClearGameMods(game);
			}
		}

		private void Game_BeginFindMods(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_BeginFindMods(sender, e)));
				return;
			}

			Game game = sender as Game;
			ToolStripMenuItem refresh = this.GameRefreshMenuItems[(int)game.WhichGame];
			refresh.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "cancel", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			refresh.Text = "Abort &Refresh";

			this.ClearGameMods(game);
			this.AddDefaultEntry(game);

			this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "cancel", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.RefreshButton.Text = "Abort &Refresh";
		}

		private void Game_ModFound(object sender, ModEventArgs e) {
			/*if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_ModFound(sender, e)));
				return;
			}

			Game game = sender as Game;

			this.SimpleList.BeginUpdate();

			this.SimpleList.Items.Add(new ListViewItem(e.Mod.Name) {
				Group = this.SimpleList.Groups[game.Name],
				ImageKey = $"|{game.GetType().Name}",
				Tag = e.Mod
			});

			if (this.SimpleList.Created && this.SimpleList.Visible) {
				this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}

			this.SimpleList.EndUpdate();

			e.Mod.NameChanged += this.Mod_NameChanged;
			e.Mod.CacheChanged += this.Mod_CacheChanged;

			if (e.Mod.Cache != null) {
				return;
			}

			e.Mod.RefreshCacheInfo();*/
		}

		private void Game_EndFindMods(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_EndFindMods(sender, e)));
				return;
			}

			Game game = sender as Game;

			this.SimpleList.BeginUpdate();

			this.SimpleList.Items.AddRange(game.Mods.Values.Select(mod => new ListViewItem(mod.Name) {
				Group = this.SimpleList.Groups[game.Name],
				ImageKey = $"|{game.GetType().Name}",
				Tag = mod
			}).ToArray());

			if (this.SimpleList.Created && this.SimpleList.Visible) {
				this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}

			this.SimpleList.EndUpdate();

			foreach (Mod mod in game.Mods.Values) {
				mod.NameChanged += this.Mod_NameChanged;
				mod.CacheChanged += this.Mod_CacheChanged;

				/*if (mod.Cache == null) {
					mod.RefreshCacheInfo();
				}*/
			}

			if (!game.IsFindingMods && !game.IsFindingPatches) {
				ToolStripMenuItem refresh = this.GameRefreshMenuItems[(int)game.WhichGame];
				refresh.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				refresh.Text = "&Refresh";
			}

			if (!Program.Games.Values.Any(x => x.IsFindingMods) && !Program.Games.Values.Any(x => x.IsFindingPatches)) {
				this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				this.RefreshButton.Text = "&Refresh";

				Debug.WriteLine($"{game} Mods {DateTime.Now - start}");
			}

			Program.ModCache.Save();
		}

		private void Game_BeginFindPatches(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_BeginFindPatches(sender, e)));
				return;
			}

			Game game = sender as Game;
			ToolStripMenuItem refresh = this.GameRefreshMenuItems[(int)game.WhichGame];
			refresh.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "cancel", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			refresh.Text = "Abort &Refresh";

			this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "cancel", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.RefreshButton.Text = "Abort &Refresh";
		}

		private void Game_EndFindPatches(object sender, EventArgs e) {
			if (this.InvokeRequired) {
				this.Invoke(new Action(() => this.Game_EndFindPatches(sender, e)));
				return;
			}

			Game game = sender as Game;
			if (!game.IsFindingMods && !game.IsFindingPatches) {
				ToolStripMenuItem refresh = this.GameRefreshMenuItems[(int)game.WhichGame];
				refresh.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				refresh.Text = "&Refresh";
			}

			if (!Program.Games.Values.Any(x => x.IsFindingMods) && !Program.Games.Values.Any(x => x.IsFindingPatches)) {
				this.RefreshButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "refresh", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				this.RefreshButton.Text = "&Refresh";

				Debug.WriteLine($"{game} Patches {DateTime.Now - start}");
			}

			Program.PatchCache.Save();
		}

		private void PlayButton_Click(object sender, EventArgs e) {
			Game game = this.SimpleList.SelectedItems[0].Group.Tag as Game;


			_ = ModActivator.PlaySingleplayer(game,
				this.SimpleList.SelectedItems[0].Tag is not Mod mod ? Array.Empty<Mod>() : [mod]);
		}

		private void GameMenu_DropDownOpening(object sender, EventArgs e) {
			ToolStripMenuItem gameMenu = sender as ToolStripMenuItem;
			Game game = gameMenu.Tag as Game;
			int index = (int)game.WhichGame;

			if (game.GamePath == null) {
				this.GameOptionsMenuItems[index].Visible = false;
				this.GameRefreshMenuItems[index].Visible = false;
				this.GamePathMenuItems[index].Visible = true;
				this.GameEnabledMenuItems[index].Visible = false;
				return;
			}

			if (!game.Enabled) {
				this.GameOptionsMenuItems[index].Visible = false;
				this.GameRefreshMenuItems[index].Visible = false;
				this.GamePathMenuItems[index].Visible = true;
				this.GameEnabledMenuItems[index].Visible = false;

				this.GameEnabledMenuItems[index].Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "done", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
				this.GameEnabledMenuItems[index].Text = $"&Enable {game.Name} Support";
				return;
			}

			this.GameOptionsMenuItems[index].Visible = true;
			this.GameRefreshMenuItems[index].Visible = true;
			this.GamePathMenuItems[index].Visible = true;
			this.GameEnabledMenuItems[index].Visible = true;

			this.GameEnabledMenuItems[index].Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "close", SystemColors.MenuText, 16, GraphicsUnit.Pixel);
			this.GameEnabledMenuItems[index].Text = $"Disabl&e {game.Name} Support";
		}

		private void GameEnabled_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			Game game = item.OwnerItem.Tag as Game;
			game.Enabled = !game.Enabled;
		}

		private void GamePath_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			Game game = item.OwnerItem.Tag as Game;

			using FolderBrowserDialog dialog = new() {
				AddToRecent = false,
				AutoUpgradeEnabled = true,
				Description = $"Locate {game.Name}'s Directory",
				InitialDirectory = game.GamePath ?? Path.GetDirectoryName(Application.ExecutablePath),
				ShowHiddenFiles = false,
				ShowNewFolderButton = false,
				ShowPinnedPlaces = true,
				UseDescriptionForTitle = true
			};

			/*CommonOpenFileDialog dialog = new() {
				AddToMostRecentlyUsedList = false,
				AllowNonFileSystemItems = false,
				AllowPropertyEditing = true,
				DefaultDirectory = game.GamePath ?? Path.GetDirectoryName(Application.ExecutablePath),
				EnsurePathExists = true,
				EnsureValidNames = true,
				InitialDirectory = null,
				IsFolderPicker = true,
				Multiselect = false,
				NavigateToShortcut = true,
				RestoreDirectory = true,
				ShowHiddenItems = false,
				ShowPlacesList = true,
				Title = $"Locate {game.Name}'s Directory"
			};*/

			if (dialog.ShowDialog(this) != DialogResult.OK) {
			//if (dialog.ShowDialog(this.Handle) != CommonFileDialogResult.Ok) {
				return;
			}

			if (!game.IsValidGamePath(dialog.SelectedPath)) {
			//if (!game.IsValidGamePath(dialog.FileName)) {
				MessageBox.Show(this, $"Can't find {game.Name} in that folder.",
					"Game Path - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//game.CustomGamePath = dialog.FileName;
			game.CustomGamePath = dialog.SelectedPath;
		}

		private void GameOptions_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			Game game = item.OwnerItem.Tag as Game;
			game.OptionsForm.ShowDialog(this);
		}

		private void GameRefresh_Click(object sender, EventArgs e) {
			ToolStripMenuItem item = sender as ToolStripMenuItem;
			Game game = item.OwnerItem.Tag as Game;
			if (game.IsFindingMods) {
				game.CancelFindMods();
			} else {
				game.FindMods();
			}
		}

		private void IconViewButton_Click(object sender, EventArgs e) {
			Program.Settings.View = View.LargeIcon;
			Program.Settings.Save();
		}

		private void ListViewButton_Click(object sender, EventArgs e) {
			Program.Settings.View = View.Details;
			Program.Settings.Save();
		}

		private void PlayMultiplayerButton_Click(object sender, EventArgs e) {
			Game game = this.SimpleList.SelectedItems[0].Group.Tag as Game;


			_ = ModActivator.PlayMultiplayer(game,
				this.SimpleList.SelectedItems[0].Tag is not Mod mod ? Array.Empty<Mod>() : [mod]);
		}

		private void RefreshButton_Click(object sender, EventArgs e) {
			Game[] refreshing = Program.Games.Values.Where(x => x.IsFindingMods).ToArray();
			if (refreshing.Length > 0) {
				foreach (Game game in refreshing) {
					game.CancelFindMods();
				}
			} else {
				this.RefreshAll();
			}
		}

		private void RenameButton_Click(object sender, EventArgs e) {
			ListViewItem item = this.ListViewItemMenu.Tag as ListViewItem;
			item.BeginEdit();
		}

		private void PropertiesButton_Click(object sender, EventArgs e) {
			ListViewItem item = this.ListViewItemMenu.Tag as ListViewItem;
			(item.Tag as Mod).PropertiesForm.ShowDialog(this);
		}

		private void Settings_ViewChanged(object sender, EventArgs e) {
			this.SimpleList.View = Program.Settings.View;
			this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
		}

		protected override void OnResizeEnd(EventArgs e) {
			base.OnResizeEnd(e);

			Program.Settings.FormBounds = this.Bounds;
			Program.Settings.Save();
		}

		private void OptionsButton_DropDownOpening(object sender, EventArgs e) {
			this.NoGroupingButton.Checked = this.SimpleList.Visible;
			this.GroupButton.Checked = !this.SimpleList.Visible;

			this.QuitOnRunButton.Checked = Program.Settings.RunAction == Settings.RunActions.Close;
			this.MinimizeOnRunButton.Checked = Program.Settings.RunAction == Settings.RunActions.Minimize;
			this.NothingOnRunButton.Checked = Program.Settings.RunAction == Settings.RunActions.None;

			this.OptionsSeparator2.Visible = this.SimpleList.Visible;
			this.RefreshButton.Visible = this.SimpleList.Visible;
			foreach (ToolStripMenuItem item in this.GameMenus) {
				item.Visible = this.SimpleList.Visible;
			}
		}

		private void NoGroupingButton_Click(object sender, EventArgs e) {
			if (Program.Settings.SimpleList) {
				return;
			}

			using (new UserInputBlocker()) {
				this.SimpleList.Visible = true;
			}
			if (this.SimpleList.Created && this.SimpleList.Visible) {
				this.SimpleList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}

			foreach (CollapsibleGameListView list in this.GameLists) {
				list.Visible = false;
			}
			this.ExpandedGameList?.BeginUpdate();

			if (this.SimpleList.SelectedIndices.Count < 1) {
				this.PlayButton.Visible = true;
				this.PlayButton.Enabled = false;
			} else {
				Game game = this.SimpleList.SelectedItems[0].Group.Tag as Game;
				if (game.HasSeparateMultiplayerBinary) {
					this.PlaySingleplayerButton.Visible = true;
				} else {
					this.PlayButton.Visible = true;
					this.PlayButton.Enabled = false;
				}
			}

			this.SimpleList.View = Program.Settings.View;
			this.SimpleList.EndUpdate();

			Program.Settings.SimpleList = true;
			Program.Settings.Save();
		}

		private void GroupButton_Click(object sender, EventArgs e) {
			if (!Program.Settings.SimpleList) {
				return;
			}

			this.PositionLists();
			this.ExpandedGameList?.EndUpdate();
			using (new UserInputBlocker()) {
				foreach (CollapsibleGameListView list in this.GameLists) {
					list.Visible = true;
				}
			}

			this.ExpandedGameList.View = Program.Settings.View;

			this.ExpandedGameList?.FixColumns();
			this.SimpleList.Visible = false;

			this.PlayButton.Visible = false;
			this.PlaySingleplayerButton.Visible = false;

			this.SimpleList.BeginUpdate();

			Program.Settings.SimpleList = false;
			Program.Settings.Save();
		}

		private void NothingOnRunButton_Click(object sender, EventArgs e) {
			Program.Settings.RunAction = Settings.RunActions.None;
			Program.Settings.Save();
		}

		private void MinimizeOnRunButton_Click(object sender, EventArgs e) {
			Program.Settings.RunAction = Settings.RunActions.Minimize;
			Program.Settings.Save();
		}

		private void QuitOnRunButton_Click(object sender, EventArgs e) {
			Program.Settings.RunAction = Settings.RunActions.Close;
			Program.Settings.Save();
		}

		private void WebsiteButton_Click(object sender, EventArgs e) =>
			Process.Start(new ProcessStartInfo("https://github.com/The-MAZZTer/Knight") {
				UseShellExecute = true
			});

		private void MassassiButton_Click(object sender, EventArgs e) =>
			Process.Start(new ProcessStartInfo("http://www.massassi.net/") {
				UseShellExecute = true
			});

		private void MassassiForumsButton_Click(object sender, EventArgs e) =>
			Process.Start(new ProcessStartInfo("http://forums.massassi.net/") {
				UseShellExecute = true
			});

		private void MassassiIrcButton_Click(object sender, EventArgs e) {
			Process.Start(new ProcessStartInfo("https://discord.com/channels/480329790558044160/800417541654577204") {
				UseShellExecute = true
			});
		}

		private void Df21Button_Click(object sender, EventArgs e) {
			Process.Start(new ProcessStartInfo("http://www.df-21.net/") {
				UseShellExecute = true
			});
		}

		private void Df21ForumsButton_Click(object sender, EventArgs e) {
			Process.Start(new ProcessStartInfo("https://df-21.net/forum/") {
				UseShellExecute = true
			});
		}

		private void Df21IrcButton_Click(object sender, EventArgs e) {
			Process.Start(new ProcessStartInfo("https://discord.com/channels/799331202821521458/799331202821521461") {
				UseShellExecute = true
			});
		}

		private void UpdateButton_Click(object sender, EventArgs e) =>
			Program.Updater.Show(this);

		private AboutForm about;
		private void AboutButton_Click(object sender, EventArgs e) {
			if (this.about == null) {
				this.about = new AboutForm {
					Icon = this.Icon
				};
				this.about.BigIcon.Image = Resources.Knight.ToBitmap();
			}
			this.about.ShowDialog(this);
		}

		private void Game_GamePathChanged(object sender, EventArgs e) {
			Game game = sender as Game;

			if (!game.Enabled) {
				game.Enabled = true;
			} else if (game.ModPath == null || game.ActiveModPath == null) {
				this.RefreshAll();
			}
		}
	}
}

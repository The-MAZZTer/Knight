using MZZT.Knight.Games;

namespace MZZT.Knight.Forms {
	public class CollapsibleGameListView : Panel {
		public CollapsibleGameListView(Game game, ImageList largeImageList,
			ImageList smallImageList) : base() {

			this.Game = game;

			this.Game.EnabledChanged += this.Game_EnabledChanged;

			this.Toolbar = new GameToolStrip(game);
			this.ListView = new GameListView(game);

			this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.Height = this.Toolbar.Height;

			this.Controls.AddRange([this.Toolbar, this.ListView]);

			this.Toolbar.SendToBack();
			this.ListView.LargeImageList = largeImageList;
			this.ListView.SmallImageList = smallImageList;
		}

		public Game Game { get; }

		private readonly GameToolStrip Toolbar;
		private readonly GameListView ListView;

		public int CollapsedHeight => this.Toolbar.Height;

		public bool IsExpanded {
			get => this.Height <= this.CollapsedHeight;
			set {
				if (value) {
					this.Expand();
				} else {
					this.Collapse();
				}
			}
		}
		public event EventHandler Expanded;
		public void Expand() {
			this.Expanded?.Invoke(this, new EventArgs());
			this.ListView.EndUpdate();
		}
		public event EventHandler Collapsed;
		public void Collapse() {
			this.Collapsed?.Invoke(this, new EventArgs());
			this.ListView.BeginUpdate();
		}

		public View View {
			get => this.ListView.View;
			set => this.ListView.View = value;
		}

		public IEnumerable<Mod> SelectedMods {
			get {
				return this.ListView.Items
					.Cast<ListViewItem>()
					.Where(x => x.Tag is Mod && (x.Checked || (!this.ListView.CheckBoxes && x.Selected)))
					.Select(x => x.Tag as Mod);
			}
		}

		public void AddPatch(Patch patch) =>
			this.ListView.AddPatches([patch]);

		private void Game_EnabledChanged(object sender, EventArgs e) {
			if (!this.Game.Enabled) {
				this.Collapse();
			}
		}

		public void BeginUpdate() => this.ListView.BeginUpdate();
		public void EndUpdate() => this.ListView.EndUpdate();

		public void OnApplyModsChanged() =>
			this.Toolbar.OnApplyModsChanged(this.SelectedMods);

		public void FixColumns() => this.ListView.FixColumns();
	}
}

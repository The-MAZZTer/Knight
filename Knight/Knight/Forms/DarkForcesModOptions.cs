using MZZT.Input;
using MZZT.Knight.Games;
using System.Data;

namespace MZZT.Knight.Forms {
	public partial class DarkForcesModOptions : ModOptions {
		public DarkForcesModOptions(Mod mod, IEnumerable<string> files) : base(mod) {
			this.InitializeComponent();
			this.files = files.ToArray();
			this.AddDialogButtons();
		}
		private readonly string[] files;

		protected override void Revert() {
			using (new UserInputBlocker()) {
				this.Brief.BeginUpdate();
				this.Crawl.BeginUpdate();
				this.Others.BeginUpdate();

				this.Brief.Items.Clear();
				this.Brief.Items.Add("");
				this.Brief.Items.AddRange(this.files);

				this.Crawl.Items.Clear();
				this.Crawl.Items.Add("");
				this.Crawl.Items.AddRange(this.files);

				this.Others.Items.Clear();
				this.Others.Items.AddRange(this.files);

				ModInfo info = this.Mod.Cache;
				if (info == null || info is not DarkForcesModInfo dfInfo) {
					this.Brief.SelectedIndex = 0;
					this.Crawl.SelectedIndex = 0;
				} else {
					if (!string.IsNullOrWhiteSpace(dfInfo.LfdFile)) {
						int index = Array.IndexOf(this.files.Select(x => x.ToLower()).ToArray(),
							dfInfo.LfdFile.ToLower()) + 1;
						if (index < 0) {
							this.Brief.Items.Insert(1, dfInfo.LfdFile);
							this.Brief.SelectedIndex = 1;
						} else {
							this.Brief.SelectedIndex = index;
						}
					} else {
						this.Brief.SelectedIndex = 0;
					}

					if (!string.IsNullOrWhiteSpace(dfInfo.CrawlFile)) {
						int index = Array.IndexOf(this.files.Select(x => x.ToLower()).ToArray(),
							dfInfo.CrawlFile.ToLower()) + 1;
						if (index <= 0) {
							this.Crawl.Items.Insert(1, dfInfo.CrawlFile);
							this.Crawl.SelectedIndex = 1;
						} else {
							this.Crawl.SelectedIndex = index;
						}
					} else {
						this.Crawl.SelectedIndex = 0;
					}

					foreach (string other in dfInfo.OtherFiles) {
						int index = Array.IndexOf(this.files.Select(x => x.ToLower()).ToArray(),
							other.ToLower());
						if (index < 0) {
							this.Others.Items.Insert(this.Others.Items.Count - this.files.Length, other);
							this.Others.SetItemChecked(this.Others.Items.Count - this.files.Length - 1, true);
						} else {
							this.Others.SetItemChecked(index, true);
						}
					}
				}

				this.Brief.EndUpdate();
				this.Crawl.EndUpdate();
				this.Others.EndUpdate();
			}

			base.Revert();
		}

		protected override void Apply() {
			ModInfo info = this.Mod.Cache;
			if (info == null || info is not DarkForcesModInfo dfInfo) {
				dfInfo = new DarkForcesModInfo() {
					Id = this.Mod.Id
				};
			}

			string brief = this.Brief.SelectedItem as string;
			dfInfo.LfdFile = string.IsNullOrWhiteSpace(brief) ? null : brief;

			string crawl = this.Crawl.SelectedItem as string;
			dfInfo.CrawlFile = string.IsNullOrWhiteSpace(crawl) ? null : crawl;

			dfInfo.OtherFiles = this.Others.CheckedItems.Cast<string>().ToArray();

			Program.ModCache.SetModInfo(this.Mod.Game.WhichGame, dfInfo);

			base.Apply();
		}

		private void Changed(object sender, EventArgs e) =>
			this.EnableApply();

		private void Others_ItemCheck(object sender, ItemCheckEventArgs e) =>
			this.EnableApply();
	}
}

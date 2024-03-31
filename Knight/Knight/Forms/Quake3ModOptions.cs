using MZZT.Input;
using MZZT.Knight.Games;

namespace MZZT.Knight.Forms {
	public partial class Quake3ModOptions : ModOptions {
		public Quake3ModOptions(Mod mod) : base(mod) {
			this.InitializeComponent();
			this.AddDialogButtons();
		}

		protected override void Revert() {
			using (new UserInputBlocker()) {
				ModInfo info = this.Mod.Cache;
				if (info == null || info is not Quake3ModInfo q3Info) {
					this.SingleplayerCheckbox.Checked = false;
				} else {
					this.SingleplayerCheckbox.Checked = q3Info.SinglePlayer;
				}
			}

			base.Revert();
		}

		protected override void Apply() {
			ModInfo info = this.Mod.Cache;
			if (info == null || info is not Quake3ModInfo q3Info) {
				q3Info = new Quake3ModInfo() {
					Id = this.Mod.Id,
					Name = this.Mod.Name
				};
			}

			q3Info.SinglePlayer = this.SingleplayerCheckbox.Checked;

			Program.ModCache.SetModInfo(this.Mod.Game.WhichGame, q3Info);

			base.Apply();
		}

		private void SingleplayerCheckbox_CheckedChanged(object sender, EventArgs e) =>
			this.EnableApply();
	}
}

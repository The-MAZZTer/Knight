using MZZT.Input;
using MZZT.Knight.Games;
using System.Data;

namespace MZZT.Knight.Forms {
	public partial class Quake3Options : GameOptions {
		public Quake3Options(Quake3Game game) : base(game) {
			this.InitializeComponent();
			this.AddDialogButtons();
		}
		public new Quake3Game Game => base.Game as Quake3Game;
		public new Quake3Settings Settings => base.Settings as Quake3Settings;

		protected override void Revert() {
			using (new UserInputBlocker()) {
				string lines = this.Settings.Commands;
				if (lines == null) {
					this.Commands.Text = "";
				} else {
					this.Commands.Lines = lines.Split('+')
						.Select(x => x.Trim())
						.Where(x => !string.IsNullOrWhiteSpace(x))
						.ToArray();
				}
			}

			base.Revert();
		}

		protected override async Task Apply() {
			string lines = string.Join(" +", this.Commands.Lines
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => x.Trim()));
			if (string.IsNullOrWhiteSpace(lines)) {
				lines = null;
			}

			this.Settings.Commands = lines;

			await base.Apply();
		}

		private void Commands_TextChanged(object sender, EventArgs e) =>
			this.EnableApply();
	}
}

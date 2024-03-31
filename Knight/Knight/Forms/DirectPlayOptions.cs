using Microsoft.Win32;
using MZZT.Input;
using MZZT.Knight.Games;
using System.ComponentModel;
using System.Security;

namespace MZZT.Knight.Forms {
	public partial class DirectPlayOptions : Form {
		public DirectPlayOptions(SithGame game) : base() {
			this.InitializeComponent();

			this.Game = game;

			this.CloseButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "close", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.ResetButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), ["shield", "security"], [Color.Blue, Color.Gold], 16, GraphicsUnit.Pixel);
			this.ApplyButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), ["shield", "security"], [Color.Blue, Color.Gold], 16, GraphicsUnit.Pixel);
		}
		private SithGame Game { get; }

		public static void SetDirectPlayCommandLine(string game, string commandLine) {
			Registry.SetValue(
				$@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectPlay\Applications\{game} 1.0",
				"CommandLine", commandLine);
		}

		private void EnableApply() {
			if (UserInputBlocker.IsUserInput) {
				this.CloseButton.Text = "Cancel";
			}
		}

		private void Revert() {
			using (new UserInputBlocker()) {
				string commandLine = Registry.GetValue(
					$@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\DirectPlay\Applications\{this.Game.Name} 1.0",
					"CommandLine", null) as string;
				this.UseActiveMods.Checked = commandLine != null &&
					commandLine.Contains("-path", StringComparison.CurrentCultureIgnoreCase);
			}

			this.CloseButton.Text = "Close";
		}

		private void Apply(bool set) {
			string commandLine = "";
			if (set) {
				commandLine = this.Game.GetArguments(this.UseActiveMods.Checked);
			}

			bool complete = true;
			try {
				SetDirectPlayCommandLine(this.Game.Name, commandLine);
			} catch (SecurityException) {
				Program.RunElevated("directplay", $"\"{this.Game.Name}={commandLine}\"");
			} catch (UnauthorizedAccessException) {
				Program.RunElevated("directplay", $"\"{this.Game.Name}={commandLine}\"");
			} catch (Exception ex) {
				MessageBox.Show(this, $"Unable to change DirectPlay setting: {ex}",
					"Error - Knight", MessageBoxButtons.OK, MessageBoxIcon.Error);
				complete = false;
			}

			if (complete) {
				this.CloseButton.Text = "Close";
			}
		}

		private void ApplyButton_Click(object sender, EventArgs e) =>
			this.Apply(true);

		private void ResetButton_Click(object sender, EventArgs e) =>
			this.Apply(false);

		private void UseActiveMods_CheckedChanged(object sender, EventArgs e) =>
			this.EnableApply();

		protected override void OnVisibleChanged(EventArgs e) {
			if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) {
				base.OnVisibleChanged(e);
				return;
			}

			if (this.Visible) {
				this.Revert();
			}

			base.OnVisibleChanged(e);
		}
	}
}

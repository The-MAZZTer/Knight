using MZZT.Input;
using MZZT.Knight.Games;
using MZZT.WinApi.PInvoke;
using System.ComponentModel;

namespace MZZT.Knight.Forms {
	public class ModOptions : Form {
		public ModOptions() : base() { }

		public ModOptions(Mod mod) : base() {
			this.Mod = mod;

			IntPtr handle = (mod.Game.LargeIcon as Bitmap).GetHicon();
			try {
				this.Icon = Icon.FromHandle(handle).Clone() as Icon;
			} finally {
				User32.DestroyIcon(handle);
			}

			this.Text = $"{mod.Name} Options - Knight";
		}
		public Mod Mod { get; }

		private Button OkButton;
		private Button CloseButton;
		private Button ApplyButton;

		protected void AddDialogButtons() {
			this.OkButton = new Button();
			this.CloseButton = new Button();
			this.ApplyButton = new Button();
			this.SuspendLayout();
			// 
			// OkButton
			// 
			this.OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.OkButton.DialogResult = DialogResult.OK;
			this.OkButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "done", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.OkButton.ImageAlign = ContentAlignment.MiddleRight;
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new Size(75, 24);
			this.OkButton.Text = "OK";
			this.OkButton.TextAlign = ContentAlignment.MiddleLeft;
			this.OkButton.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += this.OkButton_Click;
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.CloseButton.DialogResult = DialogResult.Cancel;
			this.CloseButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "close", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.CloseButton.ImageAlign = ContentAlignment.MiddleRight;
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new Size(75, 24);
			this.CloseButton.Text = "Close";
			this.CloseButton.TextAlign = ContentAlignment.MiddleLeft;
			this.CloseButton.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// ApplyButton
			// 
			this.ApplyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.ApplyButton.Image = Program.Glyphs.DrawBitmapGlyph(new Size(16, 16), "save", SystemColors.ControlText, 16, GraphicsUnit.Pixel);
			this.ApplyButton.ImageAlign = ContentAlignment.MiddleRight;
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new Size(75, 24);
			this.ApplyButton.Text = "&Apply";
			this.ApplyButton.TextAlign = ContentAlignment.MiddleLeft;
			this.ApplyButton.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += this.ApplyButton_Click;
			// 
			// GameOptions
			// 
			this.AcceptButton = this.OkButton;
			this.CancelButton = this.CloseButton;
			this.ApplyButton.Location = new Point(this.ClientSize - new Size(10, 10) - this.ApplyButton.Size - new Size(this.ApplyButton.Margin.Right, this.ApplyButton.Margin.Bottom));
			this.CloseButton.Location = new Point(this.ApplyButton.Left - this.ApplyButton.Margin.Left - this.CloseButton.Margin.Right - this.CloseButton.Width, this.ApplyButton.Top);
			this.OkButton.Location = new Point(this.CloseButton.Left - this.CloseButton.Margin.Left - this.OkButton.Margin.Right - this.OkButton.Width, this.CloseButton.Top);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.ApplyButton);
			this.ResumeLayout(false);
		}

		private void OkButton_Click(object sender, EventArgs e) =>
			this.Apply();

		private void ApplyButton_Click(object sender, EventArgs e) =>
			this.Apply();

		protected void DisableApply() {
			this.OkButton.Enabled = false;
			this.ApplyButton.Enabled = false;
			this.CloseButton.Text = "Close";
		}

		protected void EnableApply() {
			if (UserInputBlocker.IsUserInput) {
				this.OkButton.Enabled = true;
				this.ApplyButton.Enabled = true;
				this.CloseButton.Text = "Cancel";
			}
		}

		protected bool ApplyEnabled => this.ApplyButton.Enabled;

		protected virtual void Revert() =>
			this.DisableApply();

		protected virtual void Apply() =>
			this.DisableApply();

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

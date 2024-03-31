namespace MZZT.Knight.Forms {
	partial class Quake3ModOptions {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.SingleplayerCheckbox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// SingleplayerCheckbox
			// 
			this.SingleplayerCheckbox.AutoSize = true;
			this.SingleplayerCheckbox.Location = new System.Drawing.Point(12, 12);
			this.SingleplayerCheckbox.Name = "SingleplayerCheckbox";
			this.SingleplayerCheckbox.Size = new System.Drawing.Size(357, 17);
			this.SingleplayerCheckbox.TabIndex = 1;
			this.SingleplayerCheckbox.Text = "When mod is double-clicked, run in single player instead of multiplayer.";
			this.SingleplayerCheckbox.UseVisualStyleBackColor = true;
			this.SingleplayerCheckbox.CheckedChanged += new System.EventHandler(this.SingleplayerCheckbox_CheckedChanged);
			// 
			// Quake3ModOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(381, 71);
			this.Controls.Add(this.SingleplayerCheckbox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Quake3ModOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Mod Options - Knight";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.CheckBox SingleplayerCheckbox;
	}
}
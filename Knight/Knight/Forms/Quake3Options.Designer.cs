namespace MZZT.Knight.Forms {
	partial class Quake3Options {
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
			System.Windows.Forms.Label CommandsLabel;
			this.Commands = new System.Windows.Forms.TextBox();
			CommandsLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// CommandsLabel
			// 
			CommandsLabel.AutoSize = true;
			CommandsLabel.Location = new System.Drawing.Point(12, 9);
			CommandsLabel.Name = "CommandsLabel";
			CommandsLabel.Size = new System.Drawing.Size(182, 13);
			CommandsLabel.TabIndex = 13;
			CommandsLabel.Text = "Console commands to run on startup:";
			// 
			// Commands
			// 
			this.Commands.AcceptsReturn = true;
			this.Commands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Commands.Location = new System.Drawing.Point(12, 25);
			this.Commands.Multiline = true;
			this.Commands.Name = "Commands";
			this.Commands.Size = new System.Drawing.Size(270, 156);
			this.Commands.TabIndex = 14;
			this.Commands.TextChanged += new System.EventHandler(this.Commands_TextChanged);
			// 
			// Quake3Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(294, 223);
			this.Controls.Add(this.Commands);
			this.Controls.Add(CommandsLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Quake3Options";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Jedi Outcast Options - Knight";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.TextBox Commands;
	}
}
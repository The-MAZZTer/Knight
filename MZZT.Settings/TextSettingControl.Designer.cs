namespace MZZT.Settings {
	partial class TextSettingControl {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.textbox = new System.Windows.Forms.TextBox();
			this.label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pathTextbox
			// 
			this.textbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textbox.Location = new System.Drawing.Point(0, 16);
			this.textbox.Name = "pathTextbox";
			this.textbox.Size = new System.Drawing.Size(400, 20);
			this.textbox.TabIndex = 1;
			this.textbox.TextChanged += new System.EventHandler(this.Textbox_TextChanged);
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(-3, 0);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(74, 13);
			this.label.TabIndex = 0;
			this.label.Text = "Setting Name:";
			// 
			// TextSettingControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.textbox);
			this.Controls.Add(this.label);
			this.Name = "TextSettingControl";
			this.Size = new System.Drawing.Size(400, 37);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label;
		private System.Windows.Forms.TextBox textbox;
	}
}

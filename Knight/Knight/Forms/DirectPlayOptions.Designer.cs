namespace MZZT.Knight.Forms {
	partial class DirectPlayOptions {
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
			System.Windows.Forms.Label Description;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectPlayOptions));
			this.ResetButton = new System.Windows.Forms.Button();
			this.ApplyButton = new System.Windows.Forms.Button();
			this.UseActiveMods = new System.Windows.Forms.CheckBox();
			this.CloseButton = new System.Windows.Forms.Button();
			Description = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Description
			// 
			Description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			Description.Location = new System.Drawing.Point(12, 9);
			Description.Name = "Description";
			Description.Size = new System.Drawing.Size(383, 107);
			Description.TabIndex = 5;
			Description.Text = resources.GetString("Description.Text");
			// 
			// ResetButton
			// 
			this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ResetButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ResetButton.Location = new System.Drawing.Point(12, 172);
			this.ResetButton.Name = "ResetButton";
			this.ResetButton.Size = new System.Drawing.Size(383, 24);
			this.ResetButton.TabIndex = 8;
			this.ResetButton.Text = "&Reset to default";
			this.ResetButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ResetButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.ResetButton.UseVisualStyleBackColor = true;
			this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
			// 
			// ApplyButton
			// 
			this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ApplyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ApplyButton.Location = new System.Drawing.Point(12, 142);
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new System.Drawing.Size(383, 24);
			this.ApplyButton.TabIndex = 7;
			this.ApplyButton.Text = "&Apply settings now";
			this.ApplyButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ApplyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
			// 
			// UseActiveMods
			// 
			this.UseActiveMods.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.UseActiveMods.AutoSize = true;
			this.UseActiveMods.Checked = true;
			this.UseActiveMods.CheckState = System.Windows.Forms.CheckState.Checked;
			this.UseActiveMods.Location = new System.Drawing.Point(12, 119);
			this.UseActiveMods.Name = "UseActiveMods";
			this.UseActiveMods.Size = new System.Drawing.Size(105, 17);
			this.UseActiveMods.TabIndex = 6;
			this.UseActiveMods.Text = "&Use active mods";
			this.UseActiveMods.UseVisualStyleBackColor = true;
			this.UseActiveMods.CheckedChanged += new System.EventHandler(this.UseActiveMods_CheckedChanged);
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CloseButton.Location = new System.Drawing.Point(320, 202);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 24);
			this.CloseButton.TabIndex = 9;
			this.CloseButton.Text = "Close";
			this.CloseButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.CloseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.CloseButton.UseVisualStyleBackColor = true;
			// 
			// DirectPlayOptions
			// 
			this.AcceptButton = this.CloseButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CloseButton;
			this.ClientSize = new System.Drawing.Size(407, 238);
			this.Controls.Add(this.ResetButton);
			this.Controls.Add(this.ApplyButton);
			this.Controls.Add(this.UseActiveMods);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(Description);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DirectPlayOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DirectPlay Lobby Support - Knight";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ResetButton;
		private System.Windows.Forms.Button ApplyButton;
		private System.Windows.Forms.CheckBox UseActiveMods;
		private System.Windows.Forms.Button CloseButton;
	}
}
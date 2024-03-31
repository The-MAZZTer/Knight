namespace MZZT.Knight.Forms {
	partial class DarkForcesModOptions {
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
			this.lOthers = new System.Windows.Forms.Label();
			this.lFTEXTCRA = new System.Windows.Forms.Label();
			this.lDFBRIEF = new System.Windows.Forms.Label();
			this.Others = new System.Windows.Forms.CheckedListBox();
			this.Crawl = new System.Windows.Forms.ComboBox();
			this.Brief = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// lOthers
			// 
			this.lOthers.AutoSize = true;
			this.lOthers.Location = new System.Drawing.Point(12, 68);
			this.lOthers.Name = "lOthers";
			this.lOthers.Size = new System.Drawing.Size(83, 13);
			this.lOthers.TabIndex = 13;
			this.lOthers.Text = "Other used files:";
			// 
			// lFTEXTCRA
			// 
			this.lFTEXTCRA.AutoSize = true;
			this.lFTEXTCRA.Location = new System.Drawing.Point(12, 42);
			this.lFTEXTCRA.Name = "lFTEXTCRA";
			this.lFTEXTCRA.Size = new System.Drawing.Size(82, 13);
			this.lFTEXTCRA.TabIndex = 11;
			this.lFTEXTCRA.Text = "Text crawl LFD:";
			// 
			// lDFBRIEF
			// 
			this.lDFBRIEF.AutoSize = true;
			this.lDFBRIEF.Location = new System.Drawing.Point(12, 15);
			this.lDFBRIEF.Name = "lDFBRIEF";
			this.lDFBRIEF.Size = new System.Drawing.Size(68, 13);
			this.lDFBRIEF.TabIndex = 9;
			this.lDFBRIEF.Text = "Briefing LFD:";
			// 
			// Others
			// 
			this.Others.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Others.CheckOnClick = true;
			this.Others.FormattingEnabled = true;
			this.Others.IntegralHeight = false;
			this.Others.Location = new System.Drawing.Point(101, 66);
			this.Others.Name = "Others";
			this.Others.Size = new System.Drawing.Size(181, 115);
			this.Others.Sorted = true;
			this.Others.TabIndex = 14;
			this.Others.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Others_ItemCheck);
			// 
			// Crawl
			// 
			this.Crawl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Crawl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Crawl.FormattingEnabled = true;
			this.Crawl.Location = new System.Drawing.Point(101, 39);
			this.Crawl.Name = "Crawl";
			this.Crawl.Size = new System.Drawing.Size(181, 21);
			this.Crawl.Sorted = true;
			this.Crawl.TabIndex = 12;
			this.Crawl.SelectedIndexChanged += new System.EventHandler(this.Changed);
			// 
			// Brief
			// 
			this.Brief.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Brief.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Brief.FormattingEnabled = true;
			this.Brief.Location = new System.Drawing.Point(101, 12);
			this.Brief.Name = "Brief";
			this.Brief.Size = new System.Drawing.Size(181, 21);
			this.Brief.Sorted = true;
			this.Brief.TabIndex = 10;
			this.Brief.SelectedIndexChanged += new System.EventHandler(this.Changed);
			// 
			// DarkForcesModOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(294, 223);
			this.Controls.Add(this.Others);
			this.Controls.Add(this.lOthers);
			this.Controls.Add(this.lFTEXTCRA);
			this.Controls.Add(this.lDFBRIEF);
			this.Controls.Add(this.Crawl);
			this.Controls.Add(this.Brief);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DarkForcesModOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Mod Options - Knight";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox Others;
		private System.Windows.Forms.ComboBox Crawl;
		private System.Windows.Forms.ComboBox Brief;
		private System.Windows.Forms.Label lOthers;
		private System.Windows.Forms.Label lFTEXTCRA;
		private System.Windows.Forms.Label lDFBRIEF;
	}
}
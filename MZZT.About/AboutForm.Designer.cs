namespace MZZT.Forms {
	partial class AboutForm {
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
			System.Windows.Forms.Label hr;
			System.Windows.Forms.Button ok;
			this.BigIcon = new System.Windows.Forms.PictureBox();
			this.name = new System.Windows.Forms.Label();
			this.version = new System.Windows.Forms.Label();
			hr = new System.Windows.Forms.Label();
			ok = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.BigIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// hr
			// 
			hr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			hr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			hr.Location = new System.Drawing.Point(15, 83);
			hr.Name = "hr";
			hr.Size = new System.Drawing.Size(426, 2);
			hr.TabIndex = 1;
			// 
			// ok
			// 
			ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			ok.Location = new System.Drawing.Point(375, 342);
			ok.Name = "ok";
			ok.Size = new System.Drawing.Size(75, 23);
			ok.TabIndex = 3;
			ok.Text = "OK";
			ok.UseVisualStyleBackColor = true;
			// 
			// BigIcon
			// 
			this.BigIcon.Location = new System.Drawing.Point(17, 9);
			this.BigIcon.Name = "BigIcon";
			this.BigIcon.Size = new System.Drawing.Size(64, 64);
			this.BigIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.BigIcon.TabIndex = 0;
			this.BigIcon.TabStop = false;
			// 
			// name
			// 
			this.name.AutoSize = true;
			this.name.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.name.Location = new System.Drawing.Point(80, 6);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(82, 65);
			this.name.TabIndex = 0;
			this.name.Text = "{0}";
			// 
			// version
			// 
			this.version.AutoSize = true;
			this.version.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.version.Location = new System.Drawing.Point(49, 101);
			this.version.Name = "version";
			this.version.Size = new System.Drawing.Size(81, 65);
			this.version.TabIndex = 2;
			this.version.Text = "{0}\r\nVersion {1} ({2})\r\n\r\nLibraries:\r\n{3}";
			// 
			// About
			// 
			this.AcceptButton = ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = ok;
			this.ClientSize = new System.Drawing.Size(458, 374);
			this.Controls.Add(ok);
			this.Controls.Add(this.version);
			this.Controls.Add(hr);
			this.Controls.Add(this.name);
			this.Controls.Add(this.BigIcon);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.Text = "About {0}";
			((System.ComponentModel.ISupportInitialize)(this.BigIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label version;
		private System.Windows.Forms.Label name;
		public System.Windows.Forms.PictureBox BigIcon;
	}
}
namespace MZZT.Updates {
	partial class UpdateForm {
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
			this.status = new System.Windows.Forms.Label();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.go = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// status
			// 
			this.status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.status.Location = new System.Drawing.Point(12, 41);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(279, 23);
			this.status.TabIndex = 0;
			this.status.Text = "Ready";
			this.status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// progress
			// 
			this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progress.Location = new System.Drawing.Point(12, 12);
			this.progress.Maximum = 10000;
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(360, 23);
			this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progress.TabIndex = 1;
			// 
			// go
			// 
			this.go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.go.Location = new System.Drawing.Point(297, 41);
			this.go.Name = "go";
			this.go.Size = new System.Drawing.Size(75, 23);
			this.go.TabIndex = 3;
			this.go.Text = "&Check";
			this.go.UseVisualStyleBackColor = true;
			this.go.Click += new System.EventHandler(this.Go_Click);
			// 
			// UpdateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 76);
			this.Controls.Add(this.go);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.status);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "UpdateForm";
			this.Text = "Check For Updates - {0}";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label status;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.Button go;
	}
}
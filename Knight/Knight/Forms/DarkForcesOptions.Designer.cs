namespace MZZT.Knight.Forms {
	partial class DarkForcesOptions {
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
			GroupBox ConfigureGroup;
			GroupBox DebugGroup;
			GroupBox CdGroup;
			GroupBox InGameGroup;
			GroupBox RunUsingGroup;
			this.IMuseButton = new Button();
			this.SetupButton = new Button();
			this.BrowseMods = new Button();
			this.AutoTestCheckbox = new CheckBox();
			this.LogCheckbox = new CheckBox();
			this.SkipMemoryCheckbox = new CheckBox();
			this.SkipFilesCheckbox = new CheckBox();
			this.CdUpDown = new DomainUpDown();
			this.NoCdCheckbox = new CheckBox();
			this.ForceCdCheckbox = new CheckBox();
			this.LevelCombobox = new ComboBox();
			this.LevelSelectCheckbox = new CheckBox();
			this.ScreenshotsCheckbox = new CheckBox();
			this.SkipCutscenesCheckbox = new CheckBox();
			this.BrowseDarkXl = new Button();
			this.BrowseDosBox = new Button();
			this.RunDarkXl = new RadioButton();
			this.RunDosBox = new RadioButton();
			this.RunNative = new RadioButton();
			ConfigureGroup = new GroupBox();
			DebugGroup = new GroupBox();
			CdGroup = new GroupBox();
			InGameGroup = new GroupBox();
			RunUsingGroup = new GroupBox();
			ConfigureGroup.SuspendLayout();
			DebugGroup.SuspendLayout();
			CdGroup.SuspendLayout();
			InGameGroup.SuspendLayout();
			RunUsingGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// ConfigureGroup
			// 
			ConfigureGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			ConfigureGroup.Controls.Add(this.IMuseButton);
			ConfigureGroup.Controls.Add(this.SetupButton);
			ConfigureGroup.Controls.Add(this.BrowseMods);
			ConfigureGroup.Location = new Point(13, 133);
			ConfigureGroup.Margin = new Padding(4, 3, 4, 3);
			ConfigureGroup.Name = "ConfigureGroup";
			ConfigureGroup.Padding = new Padding(4, 3, 4, 3);
			ConfigureGroup.Size = new Size(557, 56);
			ConfigureGroup.TabIndex = 10;
			ConfigureGroup.TabStop = false;
			ConfigureGroup.Text = "Configure";
			// 
			// IMuseButton
			// 
			this.IMuseButton.Anchor = AnchorStyles.Top;
			this.IMuseButton.ImageAlign = ContentAlignment.MiddleRight;
			this.IMuseButton.Location = new Point(191, 22);
			this.IMuseButton.Margin = new Padding(4, 3, 4, 3);
			this.IMuseButton.Name = "IMuseButton";
			this.IMuseButton.Size = new Size(175, 28);
			this.IMuseButton.TabIndex = 1;
			this.IMuseButton.Text = "&iMuse setup tool";
			this.IMuseButton.TextAlign = ContentAlignment.MiddleLeft;
			this.IMuseButton.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.IMuseButton.UseVisualStyleBackColor = true;
			this.IMuseButton.Click += this.IMuseButton_Click;
			// 
			// SetupButton
			// 
			this.SetupButton.ImageAlign = ContentAlignment.MiddleRight;
			this.SetupButton.Location = new Point(8, 22);
			this.SetupButton.Margin = new Padding(4, 3, 4, 3);
			this.SetupButton.Name = "SetupButton";
			this.SetupButton.Size = new Size(175, 28);
			this.SetupButton.TabIndex = 0;
			this.SetupButton.Text = "&Setup tool";
			this.SetupButton.TextAlign = ContentAlignment.MiddleLeft;
			this.SetupButton.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.SetupButton.UseVisualStyleBackColor = true;
			this.SetupButton.Click += this.SetupButton_Click;
			// 
			// BrowseMods
			// 
			this.BrowseMods.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.BrowseMods.ImageAlign = ContentAlignment.MiddleRight;
			this.BrowseMods.Location = new Point(374, 22);
			this.BrowseMods.Margin = new Padding(4, 3, 4, 3);
			this.BrowseMods.Name = "BrowseMods";
			this.BrowseMods.Size = new Size(175, 28);
			this.BrowseMods.TabIndex = 2;
			this.BrowseMods.Text = "Change &mods folder";
			this.BrowseMods.TextAlign = ContentAlignment.MiddleLeft;
			this.BrowseMods.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.BrowseMods.UseVisualStyleBackColor = true;
			this.BrowseMods.Click += this.BrowseMods_Click;
			// 
			// DebugGroup
			// 
			DebugGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			DebugGroup.Controls.Add(this.AutoTestCheckbox);
			DebugGroup.Controls.Add(this.LogCheckbox);
			DebugGroup.Controls.Add(this.SkipMemoryCheckbox);
			DebugGroup.Controls.Add(this.SkipFilesCheckbox);
			DebugGroup.Location = new Point(13, 393);
			DebugGroup.Margin = new Padding(4, 3, 4, 3);
			DebugGroup.Name = "DebugGroup";
			DebugGroup.Padding = new Padding(4, 3, 4, 3);
			DebugGroup.Size = new Size(557, 122);
			DebugGroup.TabIndex = 11;
			DebugGroup.TabStop = false;
			DebugGroup.Text = "Debug";
			// 
			// AutoTestCheckbox
			// 
			this.AutoTestCheckbox.AutoSize = true;
			this.AutoTestCheckbox.Location = new Point(8, 97);
			this.AutoTestCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.AutoTestCheckbox.Name = "AutoTestCheckbox";
			this.AutoTestCheckbox.Size = new Size(106, 19);
			this.AutoTestCheckbox.TabIndex = 3;
			this.AutoTestCheckbox.Text = "Auto &test levels";
			this.AutoTestCheckbox.UseVisualStyleBackColor = true;
			this.AutoTestCheckbox.CheckedChanged += this.Changed;
			// 
			// LogCheckbox
			// 
			this.LogCheckbox.AutoSize = true;
			this.LogCheckbox.Location = new Point(8, 22);
			this.LogCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.LogCheckbox.Name = "LogCheckbox";
			this.LogCheckbox.Size = new Size(102, 19);
			this.LogCheckbox.TabIndex = 0;
			this.LogCheckbox.Text = "&Log file access";
			this.LogCheckbox.UseVisualStyleBackColor = true;
			this.LogCheckbox.CheckedChanged += this.Changed;
			// 
			// SkipMemoryCheckbox
			// 
			this.SkipMemoryCheckbox.AutoSize = true;
			this.SkipMemoryCheckbox.Location = new Point(8, 47);
			this.SkipMemoryCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.SkipMemoryCheckbox.Name = "SkipMemoryCheckbox";
			this.SkipMemoryCheckbox.Size = new Size(130, 19);
			this.SkipMemoryCheckbox.TabIndex = 1;
			this.SkipMemoryCheckbox.Text = "Skip mem&ory check";
			this.SkipMemoryCheckbox.UseVisualStyleBackColor = true;
			this.SkipMemoryCheckbox.CheckedChanged += this.Changed;
			// 
			// SkipFilesCheckbox
			// 
			this.SkipFilesCheckbox.AutoSize = true;
			this.SkipFilesCheckbox.Location = new Point(8, 72);
			this.SkipFilesCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.SkipFilesCheckbox.Name = "SkipFilesCheckbox";
			this.SkipFilesCheckbox.Size = new Size(120, 19);
			this.SkipFilesCheckbox.TabIndex = 2;
			this.SkipFilesCheckbox.Text = "Skip &FILES= check";
			this.SkipFilesCheckbox.UseVisualStyleBackColor = true;
			this.SkipFilesCheckbox.CheckedChanged += this.Changed;
			this.SkipFilesCheckbox.ClientSizeChanged += this.Changed;
			// 
			// CdGroup
			// 
			CdGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			CdGroup.Controls.Add(this.CdUpDown);
			CdGroup.Controls.Add(this.NoCdCheckbox);
			CdGroup.Controls.Add(this.ForceCdCheckbox);
			CdGroup.Location = new Point(13, 302);
			CdGroup.Margin = new Padding(4, 3, 4, 3);
			CdGroup.Name = "CdGroup";
			CdGroup.Padding = new Padding(4, 3, 4, 3);
			CdGroup.Size = new Size(557, 85);
			CdGroup.TabIndex = 13;
			CdGroup.TabStop = false;
			CdGroup.Text = "CD";
			// 
			// CdUpDown
			// 
			this.CdUpDown.Enabled = false;
			this.CdUpDown.Items.Add("Z");
			this.CdUpDown.Items.Add("Y");
			this.CdUpDown.Items.Add("X");
			this.CdUpDown.Items.Add("W");
			this.CdUpDown.Items.Add("V");
			this.CdUpDown.Items.Add("U");
			this.CdUpDown.Items.Add("T");
			this.CdUpDown.Items.Add("S");
			this.CdUpDown.Items.Add("R");
			this.CdUpDown.Items.Add("Q");
			this.CdUpDown.Items.Add("P");
			this.CdUpDown.Items.Add("O");
			this.CdUpDown.Items.Add("N");
			this.CdUpDown.Items.Add("M");
			this.CdUpDown.Items.Add("L");
			this.CdUpDown.Items.Add("K");
			this.CdUpDown.Items.Add("J");
			this.CdUpDown.Items.Add("I");
			this.CdUpDown.Items.Add("H");
			this.CdUpDown.Items.Add("G");
			this.CdUpDown.Items.Add("F");
			this.CdUpDown.Items.Add("E");
			this.CdUpDown.Items.Add("D");
			this.CdUpDown.Items.Add("C");
			this.CdUpDown.Items.Add("B");
			this.CdUpDown.Items.Add("A");
			this.CdUpDown.Location = new Point(122, 56);
			this.CdUpDown.Margin = new Padding(4, 3, 4, 3);
			this.CdUpDown.Name = "CdUpDown";
			this.CdUpDown.Size = new Size(35, 23);
			this.CdUpDown.TabIndex = 2;
			this.CdUpDown.Text = "D";
			this.CdUpDown.TextChanged += this.Changed;
			// 
			// NoCdCheckbox
			// 
			this.NoCdCheckbox.ImageAlign = ContentAlignment.MiddleLeft;
			this.NoCdCheckbox.Location = new Point(8, 22);
			this.NoCdCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.NoCdCheckbox.Name = "NoCdCheckbox";
			this.NoCdCheckbox.Size = new Size(133, 28);
			this.NoCdCheckbox.TabIndex = 0;
			this.NoCdCheckbox.Text = "Skip &CD check";
			this.NoCdCheckbox.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.NoCdCheckbox.UseVisualStyleBackColor = true;
			this.NoCdCheckbox.CheckedChanged += this.Changed;
			// 
			// ForceCdCheckbox
			// 
			this.ForceCdCheckbox.AutoSize = true;
			this.ForceCdCheckbox.Location = new Point(8, 57);
			this.ForceCdCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.ForceCdCheckbox.Name = "ForceCdCheckbox";
			this.ForceCdCheckbox.Size = new Size(106, 19);
			this.ForceCdCheckbox.TabIndex = 1;
			this.ForceCdCheckbox.Text = "&Force CD drive:";
			this.ForceCdCheckbox.UseVisualStyleBackColor = true;
			this.ForceCdCheckbox.CheckedChanged += this.ForceCdCheckbox_CheckedChanged;
			// 
			// InGameGroup
			// 
			InGameGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			InGameGroup.Controls.Add(this.LevelCombobox);
			InGameGroup.Controls.Add(this.LevelSelectCheckbox);
			InGameGroup.Controls.Add(this.ScreenshotsCheckbox);
			InGameGroup.Controls.Add(this.SkipCutscenesCheckbox);
			InGameGroup.Location = new Point(13, 195);
			InGameGroup.Margin = new Padding(4, 3, 4, 3);
			InGameGroup.Name = "InGameGroup";
			InGameGroup.Padding = new Padding(4, 3, 4, 3);
			InGameGroup.Size = new Size(557, 101);
			InGameGroup.TabIndex = 12;
			InGameGroup.TabStop = false;
			InGameGroup.Text = "In-Game";
			// 
			// LevelCombobox
			// 
			this.LevelCombobox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.LevelCombobox.Enabled = false;
			this.LevelCombobox.FormattingEnabled = true;
			this.LevelCombobox.Items.AddRange(new object[] { "SECBASE", "TALAY", "SEWERS", "TESTBASE", "GROMAS", "DTENTION", "RAMSHED", "ROBOTICS", "NARSHADA", "JABSHIP", "IMPCITY", "FUELSTAT", "EXECUTOR", "ARC" });
			this.LevelCombobox.Location = new Point(105, 47);
			this.LevelCombobox.Margin = new Padding(4, 3, 4, 3);
			this.LevelCombobox.Name = "LevelCombobox";
			this.LevelCombobox.Size = new Size(444, 23);
			this.LevelCombobox.TabIndex = 2;
			this.LevelCombobox.Text = "SECBASE";
			this.LevelCombobox.TextChanged += this.Changed;
			// 
			// LevelSelectCheckbox
			// 
			this.LevelSelectCheckbox.AutoSize = true;
			this.LevelSelectCheckbox.Location = new Point(8, 49);
			this.LevelSelectCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.LevelSelectCheckbox.Name = "LevelSelectCheckbox";
			this.LevelSelectCheckbox.Size = new Size(89, 19);
			this.LevelSelectCheckbox.TabIndex = 1;
			this.LevelSelectCheckbox.Text = "L&evel select:";
			this.LevelSelectCheckbox.UseVisualStyleBackColor = true;
			this.LevelSelectCheckbox.CheckedChanged += this.LevelSelectCheckbox_CheckedChanged;
			// 
			// ScreenshotsCheckbox
			// 
			this.ScreenshotsCheckbox.AutoSize = true;
			this.ScreenshotsCheckbox.Checked = true;
			this.ScreenshotsCheckbox.CheckState = CheckState.Checked;
			this.ScreenshotsCheckbox.Location = new Point(8, 22);
			this.ScreenshotsCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.ScreenshotsCheckbox.Name = "ScreenshotsCheckbox";
			this.ScreenshotsCheckbox.Size = new Size(126, 19);
			this.ScreenshotsCheckbox.TabIndex = 0;
			this.ScreenshotsCheckbox.Text = "&Enable screenshots";
			this.ScreenshotsCheckbox.UseVisualStyleBackColor = true;
			this.ScreenshotsCheckbox.CheckedChanged += this.Changed;
			// 
			// SkipCutscenesCheckbox
			// 
			this.SkipCutscenesCheckbox.AutoSize = true;
			this.SkipCutscenesCheckbox.Location = new Point(8, 76);
			this.SkipCutscenesCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.SkipCutscenesCheckbox.Name = "SkipCutscenesCheckbox";
			this.SkipCutscenesCheckbox.Size = new Size(175, 19);
			this.SkipCutscenesCheckbox.TabIndex = 3;
			this.SkipCutscenesCheckbox.Text = "Skip cutscenes and &briefings";
			this.SkipCutscenesCheckbox.UseVisualStyleBackColor = true;
			this.SkipCutscenesCheckbox.CheckedChanged += this.Changed;
			// 
			// RunUsingGroup
			// 
			RunUsingGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			RunUsingGroup.Controls.Add(this.BrowseDarkXl);
			RunUsingGroup.Controls.Add(this.BrowseDosBox);
			RunUsingGroup.Controls.Add(this.RunDarkXl);
			RunUsingGroup.Controls.Add(this.RunDosBox);
			RunUsingGroup.Controls.Add(this.RunNative);
			RunUsingGroup.Location = new Point(13, 12);
			RunUsingGroup.Margin = new Padding(4, 3, 4, 3);
			RunUsingGroup.Name = "RunUsingGroup";
			RunUsingGroup.Padding = new Padding(4, 3, 4, 3);
			RunUsingGroup.Size = new Size(557, 115);
			RunUsingGroup.TabIndex = 9;
			RunUsingGroup.TabStop = false;
			RunUsingGroup.Text = "Run Using";
			// 
			// BrowseDarkXl
			// 
			this.BrowseDarkXl.ImageAlign = ContentAlignment.MiddleRight;
			this.BrowseDarkXl.Location = new Point(84, 81);
			this.BrowseDarkXl.Margin = new Padding(4, 3, 4, 3);
			this.BrowseDarkXl.Name = "BrowseDarkXl";
			this.BrowseDarkXl.Size = new Size(88, 28);
			this.BrowseDarkXl.TabIndex = 4;
			this.BrowseDarkXl.Text = "Browse";
			this.BrowseDarkXl.TextAlign = ContentAlignment.MiddleLeft;
			this.BrowseDarkXl.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.BrowseDarkXl.UseVisualStyleBackColor = true;
			this.BrowseDarkXl.Click += this.BrowseDarkXl_Click;
			// 
			// BrowseDosBox
			// 
			this.BrowseDosBox.ImageAlign = ContentAlignment.MiddleRight;
			this.BrowseDosBox.Location = new Point(84, 47);
			this.BrowseDosBox.Margin = new Padding(4, 3, 4, 3);
			this.BrowseDosBox.Name = "BrowseDosBox";
			this.BrowseDosBox.Size = new Size(88, 28);
			this.BrowseDosBox.TabIndex = 2;
			this.BrowseDosBox.Text = "Browse";
			this.BrowseDosBox.TextAlign = ContentAlignment.MiddleLeft;
			this.BrowseDosBox.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.BrowseDosBox.UseVisualStyleBackColor = true;
			this.BrowseDosBox.Click += this.BrowseDosBox_Click;
			// 
			// RunDarkXl
			// 
			this.RunDarkXl.AutoSize = true;
			this.RunDarkXl.CheckAlign = ContentAlignment.TopLeft;
			this.RunDarkXl.Location = new Point(8, 86);
			this.RunDarkXl.Margin = new Padding(4, 3, 4, 3);
			this.RunDarkXl.Name = "RunDarkXl";
			this.RunDarkXl.Size = new Size(74, 19);
			this.RunDarkXl.TabIndex = 3;
			this.RunDarkXl.Text = "&Remaster";
			this.RunDarkXl.UseVisualStyleBackColor = true;
			this.RunDarkXl.CheckedChanged += this.RunDarkXl_CheckedChanged;
			// 
			// RunDosBox
			// 
			this.RunDosBox.AutoSize = true;
			this.RunDosBox.Location = new Point(8, 52);
			this.RunDosBox.Margin = new Padding(4, 3, 4, 3);
			this.RunDosBox.Name = "RunDosBox";
			this.RunDosBox.Size = new Size(68, 19);
			this.RunDosBox.TabIndex = 1;
			this.RunDosBox.Text = "&DOSBox";
			this.RunDosBox.UseVisualStyleBackColor = true;
			this.RunDosBox.CheckedChanged += this.RunDosBox_CheckedChanged;
			// 
			// RunNative
			// 
			this.RunNative.AutoSize = true;
			this.RunNative.Checked = true;
			this.RunNative.Location = new Point(8, 22);
			this.RunNative.Margin = new Padding(4, 3, 4, 3);
			this.RunNative.Name = "RunNative";
			this.RunNative.Size = new Size(66, 19);
			this.RunNative.TabIndex = 0;
			this.RunNative.TabStop = true;
			this.RunNative.Text = "&NTVDM";
			this.RunNative.UseVisualStyleBackColor = true;
			this.RunNative.CheckedChanged += this.Changed;
			// 
			// DarkForcesOptions
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(583, 561);
			this.Controls.Add(ConfigureGroup);
			this.Controls.Add(DebugGroup);
			this.Controls.Add(CdGroup);
			this.Controls.Add(InGameGroup);
			this.Controls.Add(RunUsingGroup);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.Margin = new Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DarkForcesOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Dark Forces Options - Knight";
			ConfigureGroup.ResumeLayout(false);
			DebugGroup.ResumeLayout(false);
			DebugGroup.PerformLayout();
			CdGroup.ResumeLayout(false);
			CdGroup.PerformLayout();
			InGameGroup.ResumeLayout(false);
			InGameGroup.PerformLayout();
			RunUsingGroup.ResumeLayout(false);
			RunUsingGroup.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion
		private System.Windows.Forms.ComboBox LevelCombobox;
		private System.Windows.Forms.Button IMuseButton;
		private System.Windows.Forms.Button SetupButton;
		private System.Windows.Forms.Button BrowseMods;
		private System.Windows.Forms.CheckBox AutoTestCheckbox;
		private System.Windows.Forms.CheckBox LogCheckbox;
		private System.Windows.Forms.CheckBox SkipMemoryCheckbox;
		private System.Windows.Forms.CheckBox SkipFilesCheckbox;
		private System.Windows.Forms.DomainUpDown CdUpDown;
		private System.Windows.Forms.CheckBox NoCdCheckbox;
		private System.Windows.Forms.CheckBox ForceCdCheckbox;
		private System.Windows.Forms.CheckBox LevelSelectCheckbox;
		private System.Windows.Forms.CheckBox ScreenshotsCheckbox;
		private System.Windows.Forms.CheckBox SkipCutscenesCheckbox;
		private System.Windows.Forms.Button BrowseDarkXl;
		private System.Windows.Forms.Button BrowseDosBox;
		private System.Windows.Forms.RadioButton RunDarkXl;
		private System.Windows.Forms.RadioButton RunDosBox;
		private System.Windows.Forms.RadioButton RunNative;
	}
}
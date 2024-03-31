namespace MZZT.Knight.Forms {
	partial class JediKnightOptions {
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
			GroupBox ModsFoldersGroup;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JediKnightOptions));
			GroupBox SkipCdCheckGroup;
			GroupBox InGameGroup;
			GroupBox DisplayGroup;
			GroupBox DebugGroup;
			this.BrowsePatchFolder = new Button();
			this.BrowseActiveModFolder = new Button();
			this.BrowseModFolder = new Button();
			this.Cd2Radio = new RadioButton();
			this.Cd1Radio = new RadioButton();
			this.CdNoneRadio = new RadioButton();
			this.JkUpVerbsCheckbox = new CheckBox();
			this.ConsoleCheckbox = new CheckBox();
			this.PlayerStatusCheckbox = new CheckBox();
			this.FramerateCheckbox = new CheckBox();
			this.HudCheckbox = new CheckBox();
			this.WindowedGuiCheckbox = new CheckBox();
			this.AdvancedDisplayOptionsCheckbox = new CheckBox();
			this.VerboseUpDown = new NumericUpDown();
			this.VerboseCheckbox = new CheckBox();
			this.LogConRadio = new RadioButton();
			this.LogFileRadio = new RadioButton();
			this.LogNoRadio = new RadioButton();
			this.MultipleInstancesCheckbox = new CheckBox();
			this.DirectPlayButton = new Button();
			ModsFoldersGroup = new GroupBox();
			SkipCdCheckGroup = new GroupBox();
			InGameGroup = new GroupBox();
			DisplayGroup = new GroupBox();
			DebugGroup = new GroupBox();
			ModsFoldersGroup.SuspendLayout();
			SkipCdCheckGroup.SuspendLayout();
			InGameGroup.SuspendLayout();
			DisplayGroup.SuspendLayout();
			DebugGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.VerboseUpDown).BeginInit();
			this.SuspendLayout();
			// 
			// ModsFoldersGroup
			// 
			ModsFoldersGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			ModsFoldersGroup.Controls.Add(this.BrowsePatchFolder);
			ModsFoldersGroup.Controls.Add(this.BrowseActiveModFolder);
			ModsFoldersGroup.Controls.Add(this.BrowseModFolder);
			ModsFoldersGroup.Location = new Point(13, 12);
			ModsFoldersGroup.Margin = new Padding(4, 3, 4, 3);
			ModsFoldersGroup.Name = "ModsFoldersGroup";
			ModsFoldersGroup.Padding = new Padding(4, 3, 4, 3);
			ModsFoldersGroup.Size = new Size(557, 56);
			ModsFoldersGroup.TabIndex = 12;
			ModsFoldersGroup.TabStop = false;
			ModsFoldersGroup.Text = "Mod folders";
			// 
			// BrowsePatchFolder
			// 
			this.BrowsePatchFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.BrowsePatchFolder.ImageAlign = ContentAlignment.MiddleRight;
			this.BrowsePatchFolder.Location = new Point(374, 22);
			this.BrowsePatchFolder.Margin = new Padding(4, 3, 4, 3);
			this.BrowsePatchFolder.Name = "BrowsePatchFolder";
			this.BrowsePatchFolder.Size = new Size(175, 28);
			this.BrowsePatchFolder.TabIndex = 2;
			this.BrowsePatchFolder.Text = "Change &patch folder";
			this.BrowsePatchFolder.TextAlign = ContentAlignment.MiddleLeft;
			this.BrowsePatchFolder.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.BrowsePatchFolder.UseVisualStyleBackColor = true;
			this.BrowsePatchFolder.Click += this.BrowsePatchFolder_Click;
			// 
			// BrowseActiveModFolder
			// 
			this.BrowseActiveModFolder.Anchor = AnchorStyles.Top;
			this.BrowseActiveModFolder.ImageAlign = ContentAlignment.MiddleRight;
			this.BrowseActiveModFolder.Location = new Point(191, 22);
			this.BrowseActiveModFolder.Margin = new Padding(4, 3, 4, 3);
			this.BrowseActiveModFolder.Name = "BrowseActiveModFolder";
			this.BrowseActiveModFolder.Size = new Size(175, 28);
			this.BrowseActiveModFolder.TabIndex = 1;
			this.BrowseActiveModFolder.Text = "Change ac&tive folder";
			this.BrowseActiveModFolder.TextAlign = ContentAlignment.MiddleLeft;
			this.BrowseActiveModFolder.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.BrowseActiveModFolder.UseVisualStyleBackColor = true;
			this.BrowseActiveModFolder.Click += this.BrowseActiveModFolder_Click;
			// 
			// BrowseModFolder
			// 
			this.BrowseModFolder.ImageAlign = ContentAlignment.MiddleRight;
			this.BrowseModFolder.Location = new Point(8, 22);
			this.BrowseModFolder.Margin = new Padding(4, 3, 4, 3);
			this.BrowseModFolder.Name = "BrowseModFolder";
			this.BrowseModFolder.Size = new Size(175, 28);
			this.BrowseModFolder.TabIndex = 0;
			this.BrowseModFolder.Text = "Change &inactive folder";
			this.BrowseModFolder.TextAlign = ContentAlignment.MiddleLeft;
			this.BrowseModFolder.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.BrowseModFolder.UseVisualStyleBackColor = true;
			this.BrowseModFolder.Click += this.BrowseModFolder_Click;
			// 
			// SkipCdCheckGroup
			// 
			SkipCdCheckGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			SkipCdCheckGroup.Controls.Add(this.Cd2Radio);
			SkipCdCheckGroup.Controls.Add(this.Cd1Radio);
			SkipCdCheckGroup.Controls.Add(this.CdNoneRadio);
			SkipCdCheckGroup.Location = new Point(13, 305);
			SkipCdCheckGroup.Margin = new Padding(4, 3, 4, 3);
			SkipCdCheckGroup.Name = "SkipCdCheckGroup";
			SkipCdCheckGroup.Padding = new Padding(4, 3, 4, 3);
			SkipCdCheckGroup.Size = new Size(557, 97);
			SkipCdCheckGroup.TabIndex = 9;
			SkipCdCheckGroup.TabStop = false;
			SkipCdCheckGroup.Text = "Skip CD check";
			// 
			// Cd2Radio
			// 
			this.Cd2Radio.AutoSize = true;
			this.Cd2Radio.Location = new Point(8, 72);
			this.Cd2Radio.Margin = new Padding(4, 3, 4, 3);
			this.Cd2Radio.Name = "Cd2Radio";
			this.Cd2Radio.Size = new Size(90, 19);
			this.Cd2Radio.TabIndex = 2;
			this.Cd2Radio.Text = "Spoof Disc &2";
			this.Cd2Radio.UseVisualStyleBackColor = true;
			this.Cd2Radio.CheckedChanged += this.Changed;
			// 
			// Cd1Radio
			// 
			this.Cd1Radio.AutoSize = true;
			this.Cd1Radio.Location = new Point(8, 47);
			this.Cd1Radio.Margin = new Padding(4, 3, 4, 3);
			this.Cd1Radio.Name = "Cd1Radio";
			this.Cd1Radio.Size = new Size(90, 19);
			this.Cd1Radio.TabIndex = 1;
			this.Cd1Radio.Text = "Spoof Disc &1";
			this.Cd1Radio.UseVisualStyleBackColor = true;
			this.Cd1Radio.CheckedChanged += this.Changed;
			// 
			// CdNoneRadio
			// 
			this.CdNoneRadio.AutoSize = true;
			this.CdNoneRadio.Checked = true;
			this.CdNoneRadio.Location = new Point(8, 22);
			this.CdNoneRadio.Margin = new Padding(4, 3, 4, 3);
			this.CdNoneRadio.Name = "CdNoneRadio";
			this.CdNoneRadio.Size = new Size(70, 19);
			this.CdNoneRadio.TabIndex = 0;
			this.CdNoneRadio.TabStop = true;
			this.CdNoneRadio.Text = "&Disabled";
			this.CdNoneRadio.UseVisualStyleBackColor = true;
			this.CdNoneRadio.CheckedChanged += this.Changed;
			// 
			// InGameGroup
			// 
			InGameGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			InGameGroup.Controls.Add(this.JkUpVerbsCheckbox);
			InGameGroup.Controls.Add(this.ConsoleCheckbox);
			InGameGroup.Location = new Point(13, 227);
			InGameGroup.Margin = new Padding(4, 3, 4, 3);
			InGameGroup.Name = "InGameGroup";
			InGameGroup.Padding = new Padding(4, 3, 4, 3);
			InGameGroup.Size = new Size(557, 72);
			InGameGroup.TabIndex = 13;
			InGameGroup.TabStop = false;
			InGameGroup.Text = "In-Game";
			// 
			// JkUpVerbsCheckbox
			// 
			this.JkUpVerbsCheckbox.AutoSize = true;
			this.JkUpVerbsCheckbox.Checked = true;
			this.JkUpVerbsCheckbox.CheckState = CheckState.Checked;
			this.JkUpVerbsCheckbox.Location = new Point(8, 47);
			this.JkUpVerbsCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.JkUpVerbsCheckbox.Name = "JkUpVerbsCheckbox";
			this.JkUpVerbsCheckbox.Size = new Size(246, 19);
			this.JkUpVerbsCheckbox.TabIndex = 1;
			this.JkUpVerbsCheckbox.Text = "&Enable new cog verbs when running JKUP";
			this.JkUpVerbsCheckbox.UseVisualStyleBackColor = true;
			this.JkUpVerbsCheckbox.CheckedChanged += this.Changed;
			// 
			// ConsoleCheckbox
			// 
			this.ConsoleCheckbox.AutoSize = true;
			this.ConsoleCheckbox.Location = new Point(8, 22);
			this.ConsoleCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.ConsoleCheckbox.Name = "ConsoleCheckbox";
			this.ConsoleCheckbox.Size = new Size(152, 19);
			this.ConsoleCheckbox.TabIndex = 0;
			this.ConsoleCheckbox.Text = "&Console and level select";
			this.ConsoleCheckbox.UseVisualStyleBackColor = true;
			this.ConsoleCheckbox.CheckedChanged += this.Changed;
			// 
			// DisplayGroup
			// 
			DisplayGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			DisplayGroup.Controls.Add(this.PlayerStatusCheckbox);
			DisplayGroup.Controls.Add(this.FramerateCheckbox);
			DisplayGroup.Controls.Add(this.HudCheckbox);
			DisplayGroup.Controls.Add(this.WindowedGuiCheckbox);
			DisplayGroup.Controls.Add(this.AdvancedDisplayOptionsCheckbox);
			DisplayGroup.Location = new Point(13, 74);
			DisplayGroup.Margin = new Padding(4, 3, 4, 3);
			DisplayGroup.Name = "DisplayGroup";
			DisplayGroup.Padding = new Padding(4, 3, 4, 3);
			DisplayGroup.Size = new Size(557, 147);
			DisplayGroup.TabIndex = 10;
			DisplayGroup.TabStop = false;
			DisplayGroup.Text = "Display";
			// 
			// PlayerStatusCheckbox
			// 
			this.PlayerStatusCheckbox.AutoSize = true;
			this.PlayerStatusCheckbox.Location = new Point(8, 97);
			this.PlayerStatusCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.PlayerStatusCheckbox.Name = "PlayerStatusCheckbox";
			this.PlayerStatusCheckbox.Size = new Size(131, 19);
			this.PlayerStatusCheckbox.TabIndex = 3;
			this.PlayerStatusCheckbox.Text = "Player &status in chat";
			this.PlayerStatusCheckbox.UseVisualStyleBackColor = true;
			this.PlayerStatusCheckbox.CheckedChanged += this.Changed;
			// 
			// FramerateCheckbox
			// 
			this.FramerateCheckbox.AutoSize = true;
			this.FramerateCheckbox.Location = new Point(8, 72);
			this.FramerateCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.FramerateCheckbox.Name = "FramerateCheckbox";
			this.FramerateCheckbox.Size = new Size(157, 19);
			this.FramerateCheckbox.TabIndex = 2;
			this.FramerateCheckbox.Text = "Display &framerate in chat";
			this.FramerateCheckbox.UseVisualStyleBackColor = true;
			this.FramerateCheckbox.CheckedChanged += this.Changed;
			// 
			// HudCheckbox
			// 
			this.HudCheckbox.AutoSize = true;
			this.HudCheckbox.Checked = true;
			this.HudCheckbox.CheckState = CheckState.Checked;
			this.HudCheckbox.Location = new Point(8, 122);
			this.HudCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.HudCheckbox.Name = "HudCheckbox";
			this.HudCheckbox.Size = new Size(83, 19);
			this.HudCheckbox.TabIndex = 4;
			this.HudCheckbox.Text = "Show &HUD";
			this.HudCheckbox.UseVisualStyleBackColor = true;
			this.HudCheckbox.CheckedChanged += this.Changed;
			// 
			// WindowedGuiCheckbox
			// 
			this.WindowedGuiCheckbox.AutoSize = true;
			this.WindowedGuiCheckbox.Checked = true;
			this.WindowedGuiCheckbox.CheckState = CheckState.Checked;
			this.WindowedGuiCheckbox.Location = new Point(8, 22);
			this.WindowedGuiCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.WindowedGuiCheckbox.Name = "WindowedGuiCheckbox";
			this.WindowedGuiCheckbox.Size = new Size(193, 19);
			this.WindowedGuiCheckbox.TabIndex = 0;
			this.WindowedGuiCheckbox.Text = "&GUI/Cutscene Windowed Mode";
			this.WindowedGuiCheckbox.UseVisualStyleBackColor = true;
			this.WindowedGuiCheckbox.CheckedChanged += this.Changed;
			// 
			// AdvancedDisplayOptionsCheckbox
			// 
			this.AdvancedDisplayOptionsCheckbox.AutoSize = true;
			this.AdvancedDisplayOptionsCheckbox.Checked = true;
			this.AdvancedDisplayOptionsCheckbox.CheckState = CheckState.Checked;
			this.AdvancedDisplayOptionsCheckbox.Location = new Point(8, 47);
			this.AdvancedDisplayOptionsCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.AdvancedDisplayOptionsCheckbox.Name = "AdvancedDisplayOptionsCheckbox";
			this.AdvancedDisplayOptionsCheckbox.Size = new Size(165, 19);
			this.AdvancedDisplayOptionsCheckbox.TabIndex = 1;
			this.AdvancedDisplayOptionsCheckbox.Text = "Advanced Display &Options";
			this.AdvancedDisplayOptionsCheckbox.UseVisualStyleBackColor = true;
			this.AdvancedDisplayOptionsCheckbox.CheckedChanged += this.Changed;
			// 
			// DebugGroup
			// 
			DebugGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			DebugGroup.Controls.Add(this.VerboseUpDown);
			DebugGroup.Controls.Add(this.VerboseCheckbox);
			DebugGroup.Controls.Add(this.LogConRadio);
			DebugGroup.Controls.Add(this.LogFileRadio);
			DebugGroup.Controls.Add(this.LogNoRadio);
			DebugGroup.Controls.Add(this.MultipleInstancesCheckbox);
			DebugGroup.Location = new Point(13, 408);
			DebugGroup.Margin = new Padding(4, 3, 4, 3);
			DebugGroup.Name = "DebugGroup";
			DebugGroup.Padding = new Padding(4, 3, 4, 3);
			DebugGroup.Size = new Size(557, 158);
			DebugGroup.TabIndex = 11;
			DebugGroup.TabStop = false;
			DebugGroup.Text = "Debug";
			// 
			// VerboseUpDown
			// 
			this.VerboseUpDown.Enabled = false;
			this.VerboseUpDown.Location = new Point(121, 128);
			this.VerboseUpDown.Margin = new Padding(4, 3, 4, 3);
			this.VerboseUpDown.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
			this.VerboseUpDown.Name = "VerboseUpDown";
			this.VerboseUpDown.Size = new Size(35, 23);
			this.VerboseUpDown.TabIndex = 5;
			this.VerboseUpDown.ValueChanged += this.VerboseUpDown_ValueChanged;
			// 
			// VerboseCheckbox
			// 
			this.VerboseCheckbox.AutoSize = true;
			this.VerboseCheckbox.Location = new Point(7, 129);
			this.VerboseCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.VerboseCheckbox.Name = "VerboseCheckbox";
			this.VerboseCheckbox.Size = new Size(100, 19);
			this.VerboseCheckbox.TabIndex = 4;
			this.VerboseCheckbox.Text = "Log &verbosity:";
			this.VerboseCheckbox.UseVisualStyleBackColor = true;
			this.VerboseCheckbox.CheckedChanged += this.VerboseCheckbox_CheckedChanged;
			// 
			// LogConRadio
			// 
			this.LogConRadio.AutoSize = true;
			this.LogConRadio.Location = new Point(7, 102);
			this.LogConRadio.Margin = new Padding(4, 3, 4, 3);
			this.LogConRadio.Name = "LogConRadio";
			this.LogConRadio.Size = new Size(155, 19);
			this.LogConRadio.TabIndex = 3;
			this.LogConRadio.Text = "Log to &Windows console";
			this.LogConRadio.UseVisualStyleBackColor = true;
			this.LogConRadio.CheckedChanged += this.LogConRadio_CheckedChanged;
			// 
			// LogFileRadio
			// 
			this.LogFileRadio.AutoSize = true;
			this.LogFileRadio.Location = new Point(7, 75);
			this.LogFileRadio.Margin = new Padding(4, 3, 4, 3);
			this.LogFileRadio.Name = "LogFileRadio";
			this.LogFileRadio.Size = new Size(116, 19);
			this.LogFileRadio.TabIndex = 2;
			this.LogFileRadio.Text = "&Log to debug.log";
			this.LogFileRadio.UseVisualStyleBackColor = true;
			this.LogFileRadio.CheckedChanged += this.Changed;
			// 
			// LogNoRadio
			// 
			this.LogNoRadio.AutoSize = true;
			this.LogNoRadio.Checked = true;
			this.LogNoRadio.Location = new Point(7, 48);
			this.LogNoRadio.Margin = new Padding(4, 3, 4, 3);
			this.LogNoRadio.Name = "LogNoRadio";
			this.LogNoRadio.Size = new Size(85, 19);
			this.LogNoRadio.TabIndex = 1;
			this.LogNoRadio.TabStop = true;
			this.LogNoRadio.Text = "&No logging";
			this.LogNoRadio.UseVisualStyleBackColor = true;
			this.LogNoRadio.CheckedChanged += this.Changed;
			// 
			// MultipleInstancesCheckbox
			// 
			this.MultipleInstancesCheckbox.AutoSize = true;
			this.MultipleInstancesCheckbox.Checked = true;
			this.MultipleInstancesCheckbox.CheckState = CheckState.Checked;
			this.MultipleInstancesCheckbox.Location = new Point(7, 22);
			this.MultipleInstancesCheckbox.Margin = new Padding(4, 3, 4, 3);
			this.MultipleInstancesCheckbox.Name = "MultipleInstancesCheckbox";
			this.MultipleInstancesCheckbox.Size = new Size(155, 19);
			this.MultipleInstancesCheckbox.TabIndex = 0;
			this.MultipleInstancesCheckbox.Text = "Allow &Multiple Instances";
			this.MultipleInstancesCheckbox.UseVisualStyleBackColor = true;
			this.MultipleInstancesCheckbox.CheckedChanged += this.Changed;
			// 
			// DirectPlayButton
			// 
			this.DirectPlayButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.DirectPlayButton.ImageAlign = ContentAlignment.MiddleRight;
			this.DirectPlayButton.Location = new Point(13, 572);
			this.DirectPlayButton.Margin = new Padding(4, 3, 4, 3);
			this.DirectPlayButton.Name = "DirectPlayButton";
			this.DirectPlayButton.Size = new Size(175, 28);
			this.DirectPlayButton.TabIndex = 14;
			this.DirectPlayButton.Text = "DirectPla&y options";
			this.DirectPlayButton.TextAlign = ContentAlignment.MiddleLeft;
			this.DirectPlayButton.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.DirectPlayButton.UseVisualStyleBackColor = true;
			this.DirectPlayButton.Click += this.DirectPlayButton_Click;
			// 
			// JediKnightOptions
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(583, 612);
			this.Controls.Add(this.DirectPlayButton);
			this.Controls.Add(ModsFoldersGroup);
			this.Controls.Add(SkipCdCheckGroup);
			this.Controls.Add(InGameGroup);
			this.Controls.Add(DisplayGroup);
			this.Controls.Add(DebugGroup);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.Margin = new Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "JediKnightOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Jedi Knight Options - Knight";
			ModsFoldersGroup.ResumeLayout(false);
			SkipCdCheckGroup.ResumeLayout(false);
			SkipCdCheckGroup.PerformLayout();
			InGameGroup.ResumeLayout(false);
			InGameGroup.PerformLayout();
			DisplayGroup.ResumeLayout(false);
			DisplayGroup.PerformLayout();
			DebugGroup.ResumeLayout(false);
			DebugGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.VerboseUpDown).EndInit();
			this.ResumeLayout(false);
		}

		#endregion
		private System.Windows.Forms.Button DirectPlayButton;
		private System.Windows.Forms.Button BrowsePatchFolder;
		private System.Windows.Forms.Button BrowseActiveModFolder;
		private System.Windows.Forms.Button BrowseModFolder;
		private System.Windows.Forms.NumericUpDown VerboseUpDown;
		private System.Windows.Forms.CheckBox VerboseCheckbox;
		private System.Windows.Forms.RadioButton Cd2Radio;
		private System.Windows.Forms.RadioButton Cd1Radio;
		private System.Windows.Forms.RadioButton CdNoneRadio;
		private System.Windows.Forms.CheckBox JkUpVerbsCheckbox;
		private System.Windows.Forms.CheckBox ConsoleCheckbox;
		private System.Windows.Forms.CheckBox PlayerStatusCheckbox;
		private System.Windows.Forms.CheckBox FramerateCheckbox;
		private System.Windows.Forms.CheckBox HudCheckbox;
		private System.Windows.Forms.CheckBox WindowedGuiCheckbox;
		private System.Windows.Forms.CheckBox AdvancedDisplayOptionsCheckbox;
		private System.Windows.Forms.RadioButton LogConRadio;
		private System.Windows.Forms.RadioButton LogFileRadio;
		private System.Windows.Forms.RadioButton LogNoRadio;
		private System.Windows.Forms.CheckBox MultipleInstancesCheckbox;
	}
}
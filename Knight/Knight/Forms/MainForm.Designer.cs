namespace MZZT.Knight.Forms {
	partial class MainForm {
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
			this.components = new System.ComponentModel.Container();
			ToolStripSeparator HelpSeparator2;
			ToolStripSeparator HelpSeparator4;
			ToolStripSeparator OptionsSeparator1;
			ColumnHeader NameColumn;
			this.HelpToolbarButton = new ToolStripDropDownButton();
			this.AboutButton = new ToolStripMenuItem();
			this.UpdateButton = new ToolStripMenuItem();
			this.WebsiteButton = new ToolStripMenuItem();
			this.MassassiButton = new ToolStripMenuItem();
			this.MassassiForumsButton = new ToolStripMenuItem();
			this.MassassiIrcButton = new ToolStripMenuItem();
			this.Df21Button = new ToolStripMenuItem();
			this.Df21ForumsButton = new ToolStripMenuItem();
			this.Df21IrcButton = new ToolStripMenuItem();
			this.OptionsSeparator2 = new ToolStripSeparator();
			this.OptionsButton = new ToolStripDropDownButton();
			this.NoGroupingButton = new ToolStripMenuItem();
			this.GroupButton = new ToolStripMenuItem();
			this.NothingOnRunButton = new ToolStripMenuItem();
			this.MinimizeOnRunButton = new ToolStripMenuItem();
			this.QuitOnRunButton = new ToolStripMenuItem();
			this.RefreshButton = new ToolStripMenuItem();
			this.Toolbar = new ToolStrip();
			this.PlayButton = new ToolStripButton();
			this.PlaySingleplayerButton = new ToolStripSplitButton();
			this.PlayMultiplayerButton = new ToolStripMenuItem();
			this.SimpleList = new ListView();
			this.LargeImages = new ImageList(this.components);
			this.SmallImages = new ImageList(this.components);
			this.ListViewMenu = new ContextMenuStrip(this.components);
			this.IconViewButton = new ToolStripMenuItem();
			this.ListViewButton = new ToolStripMenuItem();
			this.ListViewItemMenu = new ContextMenuStrip(this.components);
			this.RenameButton = new ToolStripMenuItem();
			this.PropertiesButton = new ToolStripMenuItem();
			HelpSeparator2 = new ToolStripSeparator();
			HelpSeparator4 = new ToolStripSeparator();
			OptionsSeparator1 = new ToolStripSeparator();
			NameColumn = new ColumnHeader();
			this.Toolbar.SuspendLayout();
			this.ListViewMenu.SuspendLayout();
			this.ListViewItemMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// HelpSeparator2
			// 
			HelpSeparator2.Name = "HelpSeparator2";
			HelpSeparator2.Size = new Size(177, 6);
			// 
			// HelpSeparator4
			// 
			HelpSeparator4.Name = "HelpSeparator4";
			HelpSeparator4.Size = new Size(177, 6);
			// 
			// OptionsSeparator1
			// 
			OptionsSeparator1.Name = "OptionsSeparator1";
			OptionsSeparator1.Size = new Size(244, 6);
			// 
			// NameColumn
			// 
			NameColumn.Text = "Name";
			// 
			// HelpToolbarButton
			// 
			this.HelpToolbarButton.Alignment = ToolStripItemAlignment.Right;
			this.HelpToolbarButton.DropDownItems.AddRange(new ToolStripItem[] { this.AboutButton, this.UpdateButton, this.WebsiteButton, HelpSeparator2, this.MassassiButton, this.MassassiForumsButton, this.MassassiIrcButton, HelpSeparator4, this.Df21Button, this.Df21ForumsButton, this.Df21IrcButton });
			this.HelpToolbarButton.ImageTransparentColor = Color.Magenta;
			this.HelpToolbarButton.Name = "HelpToolbarButton";
			this.HelpToolbarButton.Size = new Size(45, 26);
			this.HelpToolbarButton.Text = "&Help";
			// 
			// AboutButton
			// 
			this.AboutButton.Name = "AboutButton";
			this.AboutButton.Size = new Size(180, 22);
			this.AboutButton.Text = "&About";
			this.AboutButton.Click += this.AboutButton_Click;
			// 
			// UpdateButton
			// 
			this.UpdateButton.Name = "UpdateButton";
			this.UpdateButton.Size = new Size(180, 22);
			this.UpdateButton.Text = "Check for &Updates";
			this.UpdateButton.Click += this.UpdateButton_Click;
			// 
			// WebsiteButton
			// 
			this.WebsiteButton.Name = "WebsiteButton";
			this.WebsiteButton.ShowShortcutKeys = false;
			this.WebsiteButton.Size = new Size(180, 22);
			this.WebsiteButton.Text = "&Website";
			this.WebsiteButton.Click += this.WebsiteButton_Click;
			// 
			// MassassiButton
			// 
			this.MassassiButton.Name = "MassassiButton";
			this.MassassiButton.ShowShortcutKeys = false;
			this.MassassiButton.Size = new Size(180, 22);
			this.MassassiButton.Text = "&The Massassi Temple";
			this.MassassiButton.Click += this.MassassiButton_Click;
			// 
			// MassassiForumsButton
			// 
			this.MassassiForumsButton.Name = "MassassiForumsButton";
			this.MassassiForumsButton.ShowShortcutKeys = false;
			this.MassassiForumsButton.Size = new Size(180, 22);
			this.MassassiForumsButton.Text = "Massassi &Forums";
			this.MassassiForumsButton.Click += this.MassassiForumsButton_Click;
			// 
			// MassassiIrcButton
			// 
			this.MassassiIrcButton.Name = "MassassiIrcButton";
			this.MassassiIrcButton.ShowShortcutKeys = false;
			this.MassassiIrcButton.Size = new Size(180, 22);
			this.MassassiIrcButton.Text = "Massassi &Chat";
			this.MassassiIrcButton.Click += this.MassassiIrcButton_Click;
			// 
			// Df21Button
			// 
			this.Df21Button.Name = "Df21Button";
			this.Df21Button.ShowShortcutKeys = false;
			this.Df21Button.Size = new Size(180, 22);
			this.Df21Button.Text = "&DF-21";
			this.Df21Button.Click += this.Df21Button_Click;
			// 
			// Df21ForumsButton
			// 
			this.Df21ForumsButton.Name = "Df21ForumsButton";
			this.Df21ForumsButton.ShowShortcutKeys = false;
			this.Df21ForumsButton.Size = new Size(180, 22);
			this.Df21ForumsButton.Text = "DF-21 F&orums";
			this.Df21ForumsButton.Click += this.Df21ForumsButton_Click;
			// 
			// Df21IrcButton
			// 
			this.Df21IrcButton.Name = "Df21IrcButton";
			this.Df21IrcButton.ShowShortcutKeys = false;
			this.Df21IrcButton.Size = new Size(180, 22);
			this.Df21IrcButton.Text = "DF-&21 Chat";
			this.Df21IrcButton.Click += this.Df21IrcButton_Click;
			// 
			// OptionsSeparator2
			// 
			this.OptionsSeparator2.Name = "OptionsSeparator2";
			this.OptionsSeparator2.Size = new Size(244, 6);
			// 
			// OptionsButton
			// 
			this.OptionsButton.Alignment = ToolStripItemAlignment.Right;
			this.OptionsButton.DropDownItems.AddRange(new ToolStripItem[] { this.NoGroupingButton, this.GroupButton, OptionsSeparator1, this.NothingOnRunButton, this.MinimizeOnRunButton, this.QuitOnRunButton, this.OptionsSeparator2, this.RefreshButton });
			this.OptionsButton.ImageTransparentColor = Color.Magenta;
			this.OptionsButton.Name = "OptionsButton";
			this.OptionsButton.Size = new Size(62, 26);
			this.OptionsButton.Text = "&Options";
			this.OptionsButton.DropDownOpening += this.OptionsButton_DropDownOpening;
			// 
			// NoGroupingButton
			// 
			this.NoGroupingButton.Name = "NoGroupingButton";
			this.NoGroupingButton.Size = new Size(247, 22);
			this.NoGroupingButton.Text = "&No Grouping";
			this.NoGroupingButton.Click += this.NoGroupingButton_Click;
			// 
			// GroupButton
			// 
			this.GroupButton.Checked = true;
			this.GroupButton.CheckState = CheckState.Checked;
			this.GroupButton.Name = "GroupButton";
			this.GroupButton.Size = new Size(247, 22);
			this.GroupButton.Text = "&Group By Game";
			this.GroupButton.Click += this.GroupButton_Click;
			// 
			// NothingOnRunButton
			// 
			this.NothingOnRunButton.Name = "NothingOnRunButton";
			this.NothingOnRunButton.Size = new Size(247, 22);
			this.NothingOnRunButton.Text = "&Do Nothing After Running Game";
			this.NothingOnRunButton.Click += this.NothingOnRunButton_Click;
			// 
			// MinimizeOnRunButton
			// 
			this.MinimizeOnRunButton.Name = "MinimizeOnRunButton";
			this.MinimizeOnRunButton.Size = new Size(247, 22);
			this.MinimizeOnRunButton.Text = "&Minimize When Running Game";
			this.MinimizeOnRunButton.Click += this.MinimizeOnRunButton_Click;
			// 
			// QuitOnRunButton
			// 
			this.QuitOnRunButton.Name = "QuitOnRunButton";
			this.QuitOnRunButton.Size = new Size(247, 22);
			this.QuitOnRunButton.Text = "&Qiot After Running Game";
			this.QuitOnRunButton.Click += this.QuitOnRunButton_Click;
			// 
			// RefreshButton
			// 
			this.RefreshButton.Name = "RefreshButton";
			this.RefreshButton.ShortcutKeys = Keys.F5;
			this.RefreshButton.Size = new Size(247, 22);
			this.RefreshButton.Text = "&Refresh";
			this.RefreshButton.Click += this.RefreshButton_Click;
			// 
			// Toolbar
			// 
			this.Toolbar.AutoSize = false;
			this.Toolbar.GripStyle = ToolStripGripStyle.Hidden;
			this.Toolbar.Items.AddRange(new ToolStripItem[] { this.PlayButton, this.OptionsButton, this.PlaySingleplayerButton, this.HelpToolbarButton });
			this.Toolbar.Location = new Point(0, 0);
			this.Toolbar.Name = "Toolbar";
			this.Toolbar.Size = new Size(341, 29);
			this.Toolbar.Stretch = true;
			this.Toolbar.TabIndex = 0;
			// 
			// PlayButton
			// 
			this.PlayButton.Enabled = false;
			this.PlayButton.ImageTransparentColor = Color.Magenta;
			this.PlayButton.Name = "PlayButton";
			this.PlayButton.Size = new Size(33, 26);
			this.PlayButton.Text = "&Play";
			this.PlayButton.Visible = false;
			this.PlayButton.Click += this.PlayButton_Click;
			// 
			// PlaySingleplayerButton
			// 
			this.PlaySingleplayerButton.DropDownItems.AddRange(new ToolStripItem[] { this.PlayMultiplayerButton });
			this.PlaySingleplayerButton.ImageTransparentColor = Color.Magenta;
			this.PlaySingleplayerButton.Name = "PlaySingleplayerButton";
			this.PlaySingleplayerButton.Size = new Size(115, 26);
			this.PlaySingleplayerButton.Text = "&Play Single Player";
			this.PlaySingleplayerButton.Visible = false;
			this.PlaySingleplayerButton.ButtonClick += this.PlayButton_Click;
			// 
			// PlayMultiplayerButton
			// 
			this.PlayMultiplayerButton.Name = "PlayMultiplayerButton";
			this.PlayMultiplayerButton.Size = new Size(159, 22);
			this.PlayMultiplayerButton.Text = "Play &Multiplayer";
			this.PlayMultiplayerButton.Click += this.PlayMultiplayerButton_Click;
			// 
			// SimpleList
			// 
			this.SimpleList.BorderStyle = BorderStyle.None;
			this.SimpleList.Columns.AddRange(new ColumnHeader[] { NameColumn });
			this.SimpleList.Dock = DockStyle.Fill;
			this.SimpleList.FullRowSelect = true;
			this.SimpleList.HeaderStyle = ColumnHeaderStyle.None;
			this.SimpleList.LabelEdit = true;
			this.SimpleList.LargeImageList = this.LargeImages;
			this.SimpleList.Location = new Point(0, 29);
			this.SimpleList.Margin = new Padding(4, 3, 4, 3);
			this.SimpleList.MultiSelect = false;
			this.SimpleList.Name = "SimpleList";
			this.SimpleList.ShowItemToolTips = true;
			this.SimpleList.Size = new Size(341, 454);
			this.SimpleList.SmallImageList = this.SmallImages;
			this.SimpleList.Sorting = SortOrder.Ascending;
			this.SimpleList.TabIndex = 2;
			this.SimpleList.UseCompatibleStateImageBehavior = false;
			this.SimpleList.View = View.Details;
			this.SimpleList.Visible = false;
			this.SimpleList.AfterLabelEdit += this.SimpleList_AfterLabelEdit;
			this.SimpleList.BeforeLabelEdit += this.SimpleList_BeforeLabelEdit;
			this.SimpleList.ItemActivate += this.SimpleList_ItemActivate;
			this.SimpleList.SelectedIndexChanged += this.SimpleList_SelectedIndexChanged;
			this.SimpleList.MouseDown += this.SimpleList_MouseDown;
			// 
			// LargeImages
			// 
			this.LargeImages.ColorDepth = ColorDepth.Depth32Bit;
			this.LargeImages.ImageSize = new Size(32, 32);
			this.LargeImages.TransparentColor = Color.Transparent;
			// 
			// SmallImages
			// 
			this.SmallImages.ColorDepth = ColorDepth.Depth32Bit;
			this.SmallImages.ImageSize = new Size(16, 16);
			this.SmallImages.TransparentColor = Color.Transparent;
			// 
			// ListViewMenu
			// 
			this.ListViewMenu.Items.AddRange(new ToolStripItem[] { this.IconViewButton, this.ListViewButton });
			this.ListViewMenu.Name = "cmsLV";
			this.ListViewMenu.Size = new Size(119, 48);
			this.ListViewMenu.Opening += this.ListViewMenu_Opening;
			// 
			// IconViewButton
			// 
			this.IconViewButton.Name = "IconViewButton";
			this.IconViewButton.ShowShortcutKeys = false;
			this.IconViewButton.Size = new Size(118, 22);
			this.IconViewButton.Text = "&Icon View";
			this.IconViewButton.Click += this.IconViewButton_Click;
			// 
			// ListViewButton
			// 
			this.ListViewButton.Checked = true;
			this.ListViewButton.CheckState = CheckState.Checked;
			this.ListViewButton.Name = "ListViewButton";
			this.ListViewButton.ShowShortcutKeys = false;
			this.ListViewButton.Size = new Size(118, 22);
			this.ListViewButton.Text = "&List View";
			this.ListViewButton.Click += this.ListViewButton_Click;
			// 
			// ListViewItemMenu
			// 
			this.ListViewItemMenu.Items.AddRange(new ToolStripItem[] { this.RenameButton, this.PropertiesButton });
			this.ListViewItemMenu.Name = "cmsLVI";
			this.ListViewItemMenu.Size = new Size(121, 48);
			// 
			// RenameButton
			// 
			this.RenameButton.Name = "RenameButton";
			this.RenameButton.ShowShortcutKeys = false;
			this.RenameButton.Size = new Size(120, 22);
			this.RenameButton.Text = "&Rename";
			this.RenameButton.Click += this.RenameButton_Click;
			// 
			// PropertiesButton
			// 
			this.PropertiesButton.Name = "PropertiesButton";
			this.PropertiesButton.ShowShortcutKeys = false;
			this.PropertiesButton.Size = new Size(120, 22);
			this.PropertiesButton.Text = "&Properties";
			this.PropertiesButton.Visible = false;
			this.PropertiesButton.Click += this.PropertiesButton_Click;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(341, 483);
			this.Controls.Add(this.SimpleList);
			this.Controls.Add(this.Toolbar);
			this.Margin = new Padding(4, 3, 4, 3);
			this.Name = "MainForm";
			this.Text = "Knight";
			this.Toolbar.ResumeLayout(false);
			this.Toolbar.PerformLayout();
			this.ListViewMenu.ResumeLayout(false);
			this.ListViewItemMenu.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.ToolStrip Toolbar;
		private System.Windows.Forms.ToolStripButton PlayButton;
		private System.Windows.Forms.ToolStripSplitButton PlaySingleplayerButton;
		private System.Windows.Forms.ToolStripMenuItem PlayMultiplayerButton;
		private System.Windows.Forms.ToolStripMenuItem RefreshButton;
		private System.Windows.Forms.ToolStripMenuItem NoGroupingButton;
		private System.Windows.Forms.ToolStripMenuItem GroupButton;
		private System.Windows.Forms.ToolStripMenuItem NothingOnRunButton;
		private System.Windows.Forms.ToolStripMenuItem MinimizeOnRunButton;
		private System.Windows.Forms.ToolStripMenuItem QuitOnRunButton;
		private System.Windows.Forms.ToolStripMenuItem UpdateButton;
		private System.Windows.Forms.ToolStripMenuItem WebsiteButton;
		private System.Windows.Forms.ToolStripMenuItem MassassiButton;
		private System.Windows.Forms.ToolStripMenuItem MassassiForumsButton;
		private System.Windows.Forms.ToolStripMenuItem MassassiIrcButton;
		private System.Windows.Forms.ToolStripMenuItem Df21Button;
		private System.Windows.Forms.ToolStripMenuItem Df21ForumsButton;
		private System.Windows.Forms.ToolStripMenuItem AboutButton;
		private System.Windows.Forms.ListView SimpleList;
		private System.Windows.Forms.ImageList SmallImages;
		private System.Windows.Forms.ImageList LargeImages;
		private System.Windows.Forms.ToolStripDropDownButton OptionsButton;
		private System.Windows.Forms.ContextMenuStrip ListViewMenu;
		private System.Windows.Forms.ToolStripMenuItem IconViewButton;
		private System.Windows.Forms.ToolStripMenuItem ListViewButton;
		private System.Windows.Forms.ContextMenuStrip ListViewItemMenu;
		private System.Windows.Forms.ToolStripMenuItem RenameButton;
		private System.Windows.Forms.ToolStripMenuItem PropertiesButton;
		private System.Windows.Forms.ToolStripSeparator OptionsSeparator2;
		private ToolStripDropDownButton HelpToolbarButton;
		private ToolStripMenuItem Df21IrcButton;
	}
}
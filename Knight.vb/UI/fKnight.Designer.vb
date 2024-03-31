Namespace MZZT.Knight
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fKnight
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim tssHelp1 As System.Windows.Forms.ToolStripSeparator
            Dim tssOptions1 As System.Windows.Forms.ToolStripSeparator
            Dim tssHelp2 As System.Windows.Forms.ToolStripSeparator
            Dim tssHelp3 As System.Windows.Forms.ToolStripSeparator
            Dim Name As System.Windows.Forms.ColumnHeader
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fKnight))
            Dim tssHelp4 As System.Windows.Forms.ToolStripSeparator
            Me.tsToolbar = New System.Windows.Forms.ToolStrip
            Me.tsbPlay = New System.Windows.Forms.ToolStripButton
            Me.tssbPlay = New System.Windows.Forms.ToolStripSplitButton
            Me.tsmiPlayMultiplayer = New System.Windows.Forms.ToolStripMenuItem
            Me.tsddbHelp = New System.Windows.Forms.ToolStripDropDownButton
            Me.tsmiWebsite = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiEmail = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiMassassi = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiMassassiForums = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiMassassiIRC = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiJKHub = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiJKHubChat = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiDF21 = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiDF21Forums = New System.Windows.Forms.ToolStripMenuItem
            Me.tsddbOptions = New System.Windows.Forms.ToolStripDropDownButton
            Me.tsmiSimple = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiTiered = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiNone = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiMinimize = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiClose = New System.Windows.Forms.ToolStripMenuItem
            Me.tssOptions2 = New System.Windows.Forms.ToolStripSeparator
            Me.tsmiRefresh = New System.Windows.Forms.ToolStripMenuItem
            Me.ilSmall = New System.Windows.Forms.ImageList(Me.components)
            Me.tAnim = New System.Windows.Forms.Timer(Me.components)
            Me.ilLarge = New System.Windows.Forms.ImageList(Me.components)
            Me.lvSimple = New System.Windows.Forms.ListView
            Me.fbdPath = New System.Windows.Forms.FolderBrowserDialog
            Me.cmsLV = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.tsmiIcons = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiList = New System.Windows.Forms.ToolStripMenuItem
            Me.cmsLVI = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.tsmiRename = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiProperties = New System.Windows.Forms.ToolStripMenuItem
            Me.tsmiUpdate = New System.Windows.Forms.ToolStripMenuItem
            tssHelp1 = New System.Windows.Forms.ToolStripSeparator
            tssOptions1 = New System.Windows.Forms.ToolStripSeparator
            tssHelp2 = New System.Windows.Forms.ToolStripSeparator
            tssHelp3 = New System.Windows.Forms.ToolStripSeparator
            Name = New System.Windows.Forms.ColumnHeader
            tssHelp4 = New System.Windows.Forms.ToolStripSeparator
            Me.tsToolbar.SuspendLayout()
            Me.cmsLV.SuspendLayout()
            Me.cmsLVI.SuspendLayout()
            Me.SuspendLayout()
            '
            'tssHelp1
            '
            tssHelp1.Name = "tssHelp1"
            tssHelp1.Size = New System.Drawing.Size(173, 6)
            '
            'tssOptions1
            '
            tssOptions1.Name = "tssOptions1"
            tssOptions1.Size = New System.Drawing.Size(223, 6)
            '
            'tssHelp2
            '
            tssHelp2.Name = "tssHelp2"
            tssHelp2.Size = New System.Drawing.Size(173, 6)
            '
            'tssHelp3
            '
            tssHelp3.Name = "tssHelp3"
            tssHelp3.Size = New System.Drawing.Size(173, 6)
            '
            'Name
            '
            Name.Text = "Name"
            '
            'tsToolbar
            '
            Me.tsToolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.tsToolbar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbPlay, Me.tssbPlay, Me.tsddbHelp, Me.tsddbOptions})
            Me.tsToolbar.Location = New System.Drawing.Point(0, 0)
            Me.tsToolbar.Name = "tsToolbar"
            Me.tsToolbar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
            Me.tsToolbar.Size = New System.Drawing.Size(292, 25)
            Me.tsToolbar.TabIndex = 0
            '
            'tsbPlay
            '
            Me.tsbPlay.Enabled = False
            Me.tsbPlay.Image = Global.My.Resources.Resources.silk_resultset_next
            Me.tsbPlay.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsbPlay.Name = "tsbPlay"
            Me.tsbPlay.Size = New System.Drawing.Size(47, 22)
            Me.tsbPlay.Text = "&Play"
            Me.tsbPlay.Visible = False
            '
            'tssbPlay
            '
            Me.tssbPlay.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiPlayMultiplayer})
            Me.tssbPlay.Image = Global.My.Resources.Resources.silk_resultset_next
            Me.tssbPlay.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tssbPlay.Name = "tssbPlay"
            Me.tssbPlay.Size = New System.Drawing.Size(127, 22)
            Me.tssbPlay.Text = "&Play Single Player"
            Me.tssbPlay.Visible = False
            '
            'tsmiPlayMultiplayer
            '
            Me.tsmiPlayMultiplayer.Image = Global.My.Resources.Resources.silk_group_go
            Me.tsmiPlayMultiplayer.Name = "tsmiPlayMultiplayer"
            Me.tsmiPlayMultiplayer.Size = New System.Drawing.Size(162, 22)
            Me.tsmiPlayMultiplayer.Text = "Play &Multiplayer"
            '
            'tsddbHelp
            '
            Me.tsddbHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.tsddbHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiUpdate, tssHelp1, Me.tsmiWebsite, Me.tsmiEmail, tssHelp2, Me.tsmiMassassi, Me.tsmiMassassiForums, Me.tsmiMassassiIRC, tssHelp3, Me.tsmiJKHub, Me.tsmiJKHubChat, tssHelp4, Me.tsmiDF21, Me.tsmiDF21Forums})
            Me.tsddbHelp.Image = CType(resources.GetObject("tsddbHelp.Image"), System.Drawing.Image)
            Me.tsddbHelp.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsddbHelp.Name = "tsddbHelp"
            Me.tsddbHelp.Size = New System.Drawing.Size(60, 22)
            Me.tsddbHelp.Text = "&Help"
            '
            'tsmiWebsite
            '
            Me.tsmiWebsite.Image = CType(resources.GetObject("tsmiWebsite.Image"), System.Drawing.Image)
            Me.tsmiWebsite.Name = "tsmiWebsite"
            Me.tsmiWebsite.ShowShortcutKeys = False
            Me.tsmiWebsite.Size = New System.Drawing.Size(176, 22)
            Me.tsmiWebsite.Text = "&mzzt.net"
            '
            'tsmiEmail
            '
            Me.tsmiEmail.Image = CType(resources.GetObject("tsmiEmail.Image"), System.Drawing.Image)
            Me.tsmiEmail.Name = "tsmiEmail"
            Me.tsmiEmail.ShowShortcutKeys = False
            Me.tsmiEmail.Size = New System.Drawing.Size(176, 22)
            Me.tsmiEmail.Text = "megazzt@&gmail.com"
            '
            'tsmiMassassi
            '
            Me.tsmiMassassi.Image = Global.My.Resources.Resources.site_massassi
            Me.tsmiMassassi.Name = "tsmiMassassi"
            Me.tsmiMassassi.ShowShortcutKeys = False
            Me.tsmiMassassi.Size = New System.Drawing.Size(176, 22)
            Me.tsmiMassassi.Text = "&The Massassi Temple"
            '
            'tsmiMassassiForums
            '
            Me.tsmiMassassiForums.Image = CType(resources.GetObject("tsmiMassassiForums.Image"), System.Drawing.Image)
            Me.tsmiMassassiForums.Name = "tsmiMassassiForums"
            Me.tsmiMassassiForums.ShowShortcutKeys = False
            Me.tsmiMassassiForums.Size = New System.Drawing.Size(176, 22)
            Me.tsmiMassassiForums.Text = "Massassi &Forums"
            '
            'tsmiMassassiIRC
            '
            Me.tsmiMassassiIRC.Image = CType(resources.GetObject("tsmiMassassiIRC.Image"), System.Drawing.Image)
            Me.tsmiMassassiIRC.Name = "tsmiMassassiIRC"
            Me.tsmiMassassiIRC.ShowShortcutKeys = False
            Me.tsmiMassassiIRC.Size = New System.Drawing.Size(176, 22)
            Me.tsmiMassassiIRC.Text = "Massassi &Chat"
            '
            'tsmiJKHub
            '
            Me.tsmiJKHub.Image = Global.My.Resources.Resources.site_jkhub
            Me.tsmiJKHub.Name = "tsmiJKHub"
            Me.tsmiJKHub.ShowShortcutKeys = False
            Me.tsmiJKHub.Size = New System.Drawing.Size(176, 22)
            Me.tsmiJKHub.Text = "&JK Hub"
            '
            'tsmiJKHubChat
            '
            Me.tsmiJKHubChat.Image = Global.My.Resources.Resources.silk_group
            Me.tsmiJKHubChat.Name = "tsmiJKHubChat"
            Me.tsmiJKHubChat.ShortcutKeys = System.Windows.Forms.Keys.F1
            Me.tsmiJKHubChat.Size = New System.Drawing.Size(176, 22)
            Me.tsmiJKHubChat.Text = "JK &Hub Chat"
            '
            'tsmiDF21
            '
            Me.tsmiDF21.Image = CType(resources.GetObject("tsmiDF21.Image"), System.Drawing.Image)
            Me.tsmiDF21.Name = "tsmiDF21"
            Me.tsmiDF21.ShowShortcutKeys = False
            Me.tsmiDF21.Size = New System.Drawing.Size(176, 22)
            Me.tsmiDF21.Text = "&DF-21"
            '
            'tsmiDF21Forums
            '
            Me.tsmiDF21Forums.Image = CType(resources.GetObject("tsmiDF21Forums.Image"), System.Drawing.Image)
            Me.tsmiDF21Forums.Name = "tsmiDF21Forums"
            Me.tsmiDF21Forums.ShowShortcutKeys = False
            Me.tsmiDF21Forums.Size = New System.Drawing.Size(176, 22)
            Me.tsmiDF21Forums.Text = "DF-21 F&orums"
            '
            'tsddbOptions
            '
            Me.tsddbOptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.tsddbOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiSimple, Me.tsmiTiered, tssOptions1, Me.tsmiNone, Me.tsmiMinimize, Me.tsmiClose, Me.tssOptions2, Me.tsmiRefresh})
            Me.tsddbOptions.Image = Global.My.Resources.Resources.silk_wrench
            Me.tsddbOptions.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.tsddbOptions.Name = "tsddbOptions"
            Me.tsddbOptions.Size = New System.Drawing.Size(78, 20)
            Me.tsddbOptions.Text = "&Options"
            '
            'tsmiSimple
            '
            Me.tsmiSimple.Image = Global.My.Resources.Resources.silk_application_view_list
            Me.tsmiSimple.Name = "tsmiSimple"
            Me.tsmiSimple.ShowShortcutKeys = False
            Me.tsmiSimple.Size = New System.Drawing.Size(226, 22)
            Me.tsmiSimple.Text = "&Simple list"
            '
            'tsmiTiered
            '
            Me.tsmiTiered.Checked = True
            Me.tsmiTiered.CheckState = System.Windows.Forms.CheckState.Checked
            Me.tsmiTiered.Image = Global.My.Resources.Resources.silk_application_split
            Me.tsmiTiered.Name = "tsmiTiered"
            Me.tsmiTiered.ShowShortcutKeys = False
            Me.tsmiTiered.Size = New System.Drawing.Size(226, 22)
            Me.tsmiTiered.Text = "&Tiered list"
            '
            'tsmiNone
            '
            Me.tsmiNone.Image = Global.My.Resources.Resources.silk_application
            Me.tsmiNone.Name = "tsmiNone"
            Me.tsmiNone.ShowShortcutKeys = False
            Me.tsmiNone.Size = New System.Drawing.Size(226, 22)
            Me.tsmiNone.Text = "Sta&y open when running game"
            '
            'tsmiMinimize
            '
            Me.tsmiMinimize.Image = Global.My.Resources.Resources.silk_application_put
            Me.tsmiMinimize.Name = "tsmiMinimize"
            Me.tsmiMinimize.ShowShortcutKeys = False
            Me.tsmiMinimize.Size = New System.Drawing.Size(226, 22)
            Me.tsmiMinimize.Text = "M&inimize when running game"
            '
            'tsmiClose
            '
            Me.tsmiClose.Checked = True
            Me.tsmiClose.CheckState = System.Windows.Forms.CheckState.Checked
            Me.tsmiClose.Image = Global.My.Resources.Resources.silk_door_in
            Me.tsmiClose.Name = "tsmiClose"
            Me.tsmiClose.ShowShortcutKeys = False
            Me.tsmiClose.Size = New System.Drawing.Size(226, 22)
            Me.tsmiClose.Text = "&Close when running game"
            '
            'tssOptions2
            '
            Me.tssOptions2.Name = "tssOptions2"
            Me.tssOptions2.Size = New System.Drawing.Size(223, 6)
            Me.tssOptions2.Visible = False
            '
            'tsmiRefresh
            '
            Me.tsmiRefresh.Image = Global.My.Resources.Resources.silk_arrow_refresh
            Me.tsmiRefresh.Name = "tsmiRefresh"
            Me.tsmiRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
            Me.tsmiRefresh.Size = New System.Drawing.Size(226, 22)
            Me.tsmiRefresh.Text = "&Refresh List"
            Me.tsmiRefresh.Visible = False
            '
            'ilSmall
            '
            Me.ilSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
            Me.ilSmall.ImageSize = New System.Drawing.Size(16, 16)
            Me.ilSmall.TransparentColor = System.Drawing.Color.Transparent
            '
            'tAnim
            '
            Me.tAnim.Interval = 10
            '
            'ilLarge
            '
            Me.ilLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
            Me.ilLarge.ImageSize = New System.Drawing.Size(32, 32)
            Me.ilLarge.TransparentColor = System.Drawing.Color.Transparent
            '
            'lvSimple
            '
            Me.lvSimple.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.lvSimple.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Name})
            Me.lvSimple.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lvSimple.FullRowSelect = True
            Me.lvSimple.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
            Me.lvSimple.HideSelection = False
            Me.lvSimple.LabelEdit = True
            Me.lvSimple.LargeImageList = Me.ilLarge
            Me.lvSimple.Location = New System.Drawing.Point(0, 25)
            Me.lvSimple.MultiSelect = False
            Me.lvSimple.Name = "lvSimple"
            Me.lvSimple.ShowItemToolTips = True
            Me.lvSimple.Size = New System.Drawing.Size(292, 396)
            Me.lvSimple.SmallImageList = Me.ilSmall
            Me.lvSimple.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvSimple.TabIndex = 1
            Me.lvSimple.UseCompatibleStateImageBehavior = False
            Me.lvSimple.View = System.Windows.Forms.View.Details
            Me.lvSimple.Visible = False
            '
            'fbdPath
            '
            Me.fbdPath.ShowNewFolderButton = False
            '
            'cmsLV
            '
            Me.cmsLV.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiIcons, Me.tsmiList})
            Me.cmsLV.Name = "cmsLV"
            Me.cmsLV.Size = New System.Drawing.Size(118, 48)
            '
            'tsmiIcons
            '
            Me.tsmiIcons.Image = Global.My.Resources.Resources.silk_application_view_icons
            Me.tsmiIcons.Name = "tsmiIcons"
            Me.tsmiIcons.ShowShortcutKeys = False
            Me.tsmiIcons.Size = New System.Drawing.Size(117, 22)
            Me.tsmiIcons.Text = "&Icon View"
            '
            'tsmiList
            '
            Me.tsmiList.Checked = True
            Me.tsmiList.CheckState = System.Windows.Forms.CheckState.Checked
            Me.tsmiList.Image = Global.My.Resources.Resources.silk_application_view_list
            Me.tsmiList.Name = "tsmiList"
            Me.tsmiList.ShowShortcutKeys = False
            Me.tsmiList.Size = New System.Drawing.Size(117, 22)
            Me.tsmiList.Text = "&List View"
            '
            'cmsLVI
            '
            Me.cmsLVI.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiRename, Me.tsmiProperties})
            Me.cmsLVI.Name = "cmsLVI"
            Me.cmsLVI.Size = New System.Drawing.Size(120, 48)
            '
            'tsmiRename
            '
            Me.tsmiRename.Image = Global.My.Resources.Resources.silk_textfield_rename
            Me.tsmiRename.Name = "tsmiRename"
            Me.tsmiRename.ShowShortcutKeys = False
            Me.tsmiRename.Size = New System.Drawing.Size(119, 22)
            Me.tsmiRename.Text = "&Rename"
            '
            'tsmiProperties
            '
            Me.tsmiProperties.Image = Global.My.Resources.Resources.silk_application_form_edit
            Me.tsmiProperties.Name = "tsmiProperties"
            Me.tsmiProperties.ShowShortcutKeys = False
            Me.tsmiProperties.Size = New System.Drawing.Size(119, 22)
            Me.tsmiProperties.Text = "&Properties"
            Me.tsmiProperties.Visible = False
            '
            'tsmiUpdate
            '
            Me.tsmiUpdate.Image = Global.My.Resources.Resources.silk_world
            Me.tsmiUpdate.Name = "tsmiUpdate"
            Me.tsmiUpdate.Size = New System.Drawing.Size(176, 22)
            Me.tsmiUpdate.Text = "&Check for Updates"
            '
            'tssHelp4
            '
            tssHelp4.Name = "tssHelp4"
            tssHelp4.Size = New System.Drawing.Size(173, 6)
            '
            'fKnight
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(292, 421)
            Me.Controls.Add(Me.lvSimple)
            Me.Controls.Add(Me.tsToolbar)
            Me.Name = "fKnight"
            Me.Text = "Knight"
            Me.tsToolbar.ResumeLayout(False)
            Me.tsToolbar.PerformLayout()
            Me.cmsLV.ResumeLayout(False)
            Me.cmsLVI.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents tsToolbar As System.Windows.Forms.ToolStrip
        Friend WithEvents tsddbHelp As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents tsmiMassassi As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiMassassiForums As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiMassassiIRC As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiDF21 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiDF21Forums As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiWebsite As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiEmail As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ilSmall As System.Windows.Forms.ImageList
        Friend WithEvents tAnim As System.Windows.Forms.Timer
        Friend WithEvents tsddbOptions As System.Windows.Forms.ToolStripDropDownButton
        Friend WithEvents tsmiMinimize As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiNone As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiClose As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ilLarge As System.Windows.Forms.ImageList
        Friend WithEvents tsmiSimple As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiTiered As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tssOptions2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents tsmiRefresh As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiJKHub As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiJKHubChat As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents lvSimple As System.Windows.Forms.ListView
        Friend WithEvents tssbPlay As System.Windows.Forms.ToolStripSplitButton
        Friend WithEvents tsmiPlayMultiplayer As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents fbdPath As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents tsbPlay As System.Windows.Forms.ToolStripButton
        Friend WithEvents cmsLV As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents cmsLVI As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents tsmiIcons As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiList As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiRename As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiProperties As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents tsmiUpdate As System.Windows.Forms.ToolStripMenuItem

    End Class
End Namespace
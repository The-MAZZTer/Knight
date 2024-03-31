Namespace MZZT.Knight.Games
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fDarkForcesOptions
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
            Dim gbRun As System.Windows.Forms.GroupBox
            Dim gbInGame As System.Windows.Forms.GroupBox
            Dim gbCD As System.Windows.Forms.GroupBox
            Dim gbDebug As System.Windows.Forms.GroupBox
            Dim ghConfigure As System.Windows.Forms.GroupBox
            Me.bBrowseDarkXL = New System.Windows.Forms.Button
            Me.bBrowseDOSBox = New System.Windows.Forms.Button
            Me.rbDarkXL = New System.Windows.Forms.RadioButton
            Me.rbDOSBox = New System.Windows.Forms.RadioButton
            Me.rbNative = New System.Windows.Forms.RadioButton
            Me.cbLevel = New System.Windows.Forms.ComboBox
            Me.cbLevelSelect = New System.Windows.Forms.CheckBox
            Me.cbScreenshots = New System.Windows.Forms.CheckBox
            Me.cbSkipCutscenes = New System.Windows.Forms.CheckBox
            Me.dudCD = New System.Windows.Forms.DomainUpDown
            Me.cbNoCD = New System.Windows.Forms.CheckBox
            Me.cbForceCD = New System.Windows.Forms.CheckBox
            Me.cbAutoTest = New System.Windows.Forms.CheckBox
            Me.cbLog = New System.Windows.Forms.CheckBox
            Me.cbSkipMemory = New System.Windows.Forms.CheckBox
            Me.cbSkipFiles = New System.Windows.Forms.CheckBox
            Me.bIMuse = New System.Windows.Forms.Button
            Me.bSetup = New System.Windows.Forms.Button
            Me.bBrowseMods = New System.Windows.Forms.Button
            Me.bApply = New System.Windows.Forms.Button
            Me.bCancel = New System.Windows.Forms.Button
            Me.bOK = New System.Windows.Forms.Button
            Me.fbdDOSBox = New System.Windows.Forms.FolderBrowserDialog
            Me.fbdDarkXL = New System.Windows.Forms.FolderBrowserDialog
            Me.fbdMods = New System.Windows.Forms.FolderBrowserDialog
            Me.ec = New MZZT.EventControl
            gbRun = New System.Windows.Forms.GroupBox
            gbInGame = New System.Windows.Forms.GroupBox
            gbCD = New System.Windows.Forms.GroupBox
            gbDebug = New System.Windows.Forms.GroupBox
            ghConfigure = New System.Windows.Forms.GroupBox
            gbRun.SuspendLayout()
            gbInGame.SuspendLayout()
            gbCD.SuspendLayout()
            gbDebug.SuspendLayout()
            ghConfigure.SuspendLayout()
            Me.SuspendLayout()
            '
            'gbRun
            '
            gbRun.Controls.Add(Me.bBrowseDarkXL)
            gbRun.Controls.Add(Me.bBrowseDOSBox)
            gbRun.Controls.Add(Me.rbDarkXL)
            gbRun.Controls.Add(Me.rbDOSBox)
            gbRun.Controls.Add(Me.rbNative)
            gbRun.Location = New System.Drawing.Point(12, 12)
            gbRun.Name = "gbRun"
            gbRun.Size = New System.Drawing.Size(159, 111)
            gbRun.TabIndex = 1
            gbRun.TabStop = False
            gbRun.Text = "Run Using"
            '
            'bBrowseDarkXL
            '
            Me.bBrowseDarkXL.Image = Global.My.Resources.Resources.silk_folder_explore
            Me.bBrowseDarkXL.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bBrowseDarkXL.Location = New System.Drawing.Point(78, 79)
            Me.bBrowseDarkXL.Name = "bBrowseDarkXL"
            Me.bBrowseDarkXL.Size = New System.Drawing.Size(75, 24)
            Me.bBrowseDarkXL.TabIndex = 4
            Me.bBrowseDarkXL.Text = "Browse"
            Me.bBrowseDarkXL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bBrowseDarkXL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bBrowseDarkXL.UseVisualStyleBackColor = True
            '
            'bBrowseDOSBox
            '
            Me.bBrowseDOSBox.Image = Global.My.Resources.Resources.silk_folder_explore
            Me.bBrowseDOSBox.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bBrowseDOSBox.Location = New System.Drawing.Point(78, 49)
            Me.bBrowseDOSBox.Name = "bBrowseDOSBox"
            Me.bBrowseDOSBox.Size = New System.Drawing.Size(75, 24)
            Me.bBrowseDOSBox.TabIndex = 2
            Me.bBrowseDOSBox.Text = "Browse"
            Me.bBrowseDOSBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bBrowseDOSBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bBrowseDOSBox.UseVisualStyleBackColor = True
            '
            'rbDarkXL
            '
            Me.rbDarkXL.AutoSize = True
            Me.rbDarkXL.Location = New System.Drawing.Point(6, 83)
            Me.rbDarkXL.Name = "rbDarkXL"
            Me.rbDarkXL.Size = New System.Drawing.Size(61, 17)
            Me.rbDarkXL.TabIndex = 3
            Me.rbDarkXL.Text = "Dark&XL"
            Me.rbDarkXL.UseVisualStyleBackColor = True
            '
            'rbDOSBox
            '
            Me.rbDOSBox.AutoSize = True
            Me.rbDOSBox.Location = New System.Drawing.Point(6, 53)
            Me.rbDOSBox.Name = "rbDOSBox"
            Me.rbDOSBox.Size = New System.Drawing.Size(66, 17)
            Me.rbDOSBox.TabIndex = 1
            Me.rbDOSBox.Text = "&DOSBox"
            Me.rbDOSBox.UseVisualStyleBackColor = True
            '
            'rbNative
            '
            Me.rbNative.AutoSize = True
            Me.rbNative.Checked = True
            Me.rbNative.Location = New System.Drawing.Point(6, 23)
            Me.rbNative.Name = "rbNative"
            Me.rbNative.Size = New System.Drawing.Size(64, 17)
            Me.rbNative.TabIndex = 0
            Me.rbNative.TabStop = True
            Me.rbNative.Text = "&NTVDM"
            Me.rbNative.UseVisualStyleBackColor = True
            '
            'gbInGame
            '
            gbInGame.Controls.Add(Me.cbLevel)
            gbInGame.Controls.Add(Me.cbLevelSelect)
            gbInGame.Controls.Add(Me.cbScreenshots)
            gbInGame.Controls.Add(Me.cbSkipCutscenes)
            gbInGame.Location = New System.Drawing.Point(12, 129)
            gbInGame.Name = "gbInGame"
            gbInGame.Size = New System.Drawing.Size(174, 115)
            gbInGame.TabIndex = 4
            gbInGame.TabStop = False
            gbInGame.Text = "In-Game"
            '
            'cbLevel
            '
            Me.cbLevel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cbLevel.Enabled = False
            Me.cbLevel.FormattingEnabled = True
            Me.cbLevel.Items.AddRange(New Object() {"SECBASE", "TALAY", "SEWERS", "TESTBASE", "GROMAS", "DTENTION", "RAMSHED", "ROBOTICS", "NARSHADA", "JABSHIP", "IMPCITY", "FUELSTAT", "EXECUTOR", "ARC"})
            Me.cbLevel.Location = New System.Drawing.Point(25, 65)
            Me.cbLevel.Name = "cbLevel"
            Me.cbLevel.Size = New System.Drawing.Size(143, 21)
            Me.cbLevel.TabIndex = 2
            Me.cbLevel.Text = "SECBASE"
            '
            'cbLevelSelect
            '
            Me.cbLevelSelect.AutoSize = True
            Me.cbLevelSelect.Location = New System.Drawing.Point(6, 42)
            Me.cbLevelSelect.Name = "cbLevelSelect"
            Me.cbLevelSelect.Size = New System.Drawing.Size(86, 17)
            Me.cbLevelSelect.TabIndex = 1
            Me.cbLevelSelect.Text = "L&evel select:"
            Me.cbLevelSelect.UseVisualStyleBackColor = True
            '
            'cbScreenshots
            '
            Me.cbScreenshots.AutoSize = True
            Me.cbScreenshots.Checked = True
            Me.cbScreenshots.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbScreenshots.Location = New System.Drawing.Point(6, 19)
            Me.cbScreenshots.Name = "cbScreenshots"
            Me.cbScreenshots.Size = New System.Drawing.Size(119, 17)
            Me.cbScreenshots.TabIndex = 0
            Me.cbScreenshots.Text = "&Enable screenshots"
            Me.cbScreenshots.UseVisualStyleBackColor = True
            '
            'cbSkipCutscenes
            '
            Me.cbSkipCutscenes.AutoSize = True
            Me.cbSkipCutscenes.Location = New System.Drawing.Point(6, 92)
            Me.cbSkipCutscenes.Name = "cbSkipCutscenes"
            Me.cbSkipCutscenes.Size = New System.Drawing.Size(162, 17)
            Me.cbSkipCutscenes.TabIndex = 3
            Me.cbSkipCutscenes.Text = "Skip cutscenes and &briefings"
            Me.cbSkipCutscenes.UseVisualStyleBackColor = True
            '
            'gbCD
            '
            gbCD.Controls.Add(Me.dudCD)
            gbCD.Controls.Add(Me.cbNoCD)
            gbCD.Controls.Add(Me.cbForceCD)
            gbCD.Location = New System.Drawing.Point(192, 129)
            gbCD.Name = "gbCD"
            gbCD.Size = New System.Drawing.Size(284, 75)
            gbCD.TabIndex = 5
            gbCD.TabStop = False
            gbCD.Text = "CD"
            '
            'dudCD
            '
            Me.dudCD.Enabled = False
            Me.dudCD.Items.Add("Z")
            Me.dudCD.Items.Add("Y")
            Me.dudCD.Items.Add("X")
            Me.dudCD.Items.Add("W")
            Me.dudCD.Items.Add("V")
            Me.dudCD.Items.Add("U")
            Me.dudCD.Items.Add("T")
            Me.dudCD.Items.Add("S")
            Me.dudCD.Items.Add("R")
            Me.dudCD.Items.Add("Q")
            Me.dudCD.Items.Add("P")
            Me.dudCD.Items.Add("O")
            Me.dudCD.Items.Add("N")
            Me.dudCD.Items.Add("M")
            Me.dudCD.Items.Add("L")
            Me.dudCD.Items.Add("K")
            Me.dudCD.Items.Add("J")
            Me.dudCD.Items.Add("I")
            Me.dudCD.Items.Add("H")
            Me.dudCD.Items.Add("G")
            Me.dudCD.Items.Add("F")
            Me.dudCD.Items.Add("E")
            Me.dudCD.Items.Add("D")
            Me.dudCD.Items.Add("C")
            Me.dudCD.Items.Add("B")
            Me.dudCD.Items.Add("A")
            Me.dudCD.Location = New System.Drawing.Point(112, 49)
            Me.dudCD.Name = "dudCD"
            Me.dudCD.Size = New System.Drawing.Size(30, 20)
            Me.dudCD.TabIndex = 2
            Me.dudCD.Text = "D"
            '
            'cbNoCD
            '
            Me.cbNoCD.Image = Global.My.Resources.Resources.silk_shield
            Me.cbNoCD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.cbNoCD.Location = New System.Drawing.Point(6, 19)
            Me.cbNoCD.Name = "cbNoCD"
            Me.cbNoCD.Size = New System.Drawing.Size(114, 24)
            Me.cbNoCD.TabIndex = 0
            Me.cbNoCD.Text = "Skip &CD check"
            Me.cbNoCD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.cbNoCD.UseVisualStyleBackColor = True
            '
            'cbForceCD
            '
            Me.cbForceCD.AutoSize = True
            Me.cbForceCD.Location = New System.Drawing.Point(6, 50)
            Me.cbForceCD.Name = "cbForceCD"
            Me.cbForceCD.Size = New System.Drawing.Size(100, 17)
            Me.cbForceCD.TabIndex = 1
            Me.cbForceCD.Text = "&Force CD drive:"
            Me.cbForceCD.UseVisualStyleBackColor = True
            '
            'gbDebug
            '
            gbDebug.Controls.Add(Me.cbAutoTest)
            gbDebug.Controls.Add(Me.cbLog)
            gbDebug.Controls.Add(Me.cbSkipMemory)
            gbDebug.Controls.Add(Me.cbSkipFiles)
            gbDebug.Location = New System.Drawing.Point(345, 12)
            gbDebug.Name = "gbDebug"
            gbDebug.Size = New System.Drawing.Size(131, 111)
            gbDebug.TabIndex = 3
            gbDebug.TabStop = False
            gbDebug.Text = "Debug"
            '
            'cbAutoTest
            '
            Me.cbAutoTest.AutoSize = True
            Me.cbAutoTest.Location = New System.Drawing.Point(6, 88)
            Me.cbAutoTest.Name = "cbAutoTest"
            Me.cbAutoTest.Size = New System.Drawing.Size(98, 17)
            Me.cbAutoTest.TabIndex = 3
            Me.cbAutoTest.Text = "Auto &test levels"
            Me.cbAutoTest.UseVisualStyleBackColor = True
            '
            'cbLog
            '
            Me.cbLog.AutoSize = True
            Me.cbLog.Location = New System.Drawing.Point(6, 19)
            Me.cbLog.Name = "cbLog"
            Me.cbLog.Size = New System.Drawing.Size(97, 17)
            Me.cbLog.TabIndex = 0
            Me.cbLog.Text = "&Log file access"
            Me.cbLog.UseVisualStyleBackColor = True
            '
            'cbSkipMemory
            '
            Me.cbSkipMemory.AutoSize = True
            Me.cbSkipMemory.Location = New System.Drawing.Point(6, 42)
            Me.cbSkipMemory.Name = "cbSkipMemory"
            Me.cbSkipMemory.Size = New System.Drawing.Size(119, 17)
            Me.cbSkipMemory.TabIndex = 1
            Me.cbSkipMemory.Text = "Skip mem&ory check"
            Me.cbSkipMemory.UseVisualStyleBackColor = True
            '
            'cbSkipFiles
            '
            Me.cbSkipFiles.AutoSize = True
            Me.cbSkipFiles.Location = New System.Drawing.Point(6, 65)
            Me.cbSkipFiles.Name = "cbSkipFiles"
            Me.cbSkipFiles.Size = New System.Drawing.Size(118, 17)
            Me.cbSkipFiles.TabIndex = 2
            Me.cbSkipFiles.Text = "Skip &FILES= check"
            Me.cbSkipFiles.UseVisualStyleBackColor = True
            '
            'ghConfigure
            '
            ghConfigure.Controls.Add(Me.bIMuse)
            ghConfigure.Controls.Add(Me.bSetup)
            ghConfigure.Controls.Add(Me.bBrowseMods)
            ghConfigure.Location = New System.Drawing.Point(177, 12)
            ghConfigure.Name = "ghConfigure"
            ghConfigure.Size = New System.Drawing.Size(162, 111)
            ghConfigure.TabIndex = 2
            ghConfigure.TabStop = False
            ghConfigure.Text = "Configure"
            '
            'bIMuse
            '
            Me.bIMuse.Image = Global.My.Resources.Resources.silk_music
            Me.bIMuse.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bIMuse.Location = New System.Drawing.Point(6, 49)
            Me.bIMuse.Name = "bIMuse"
            Me.bIMuse.Size = New System.Drawing.Size(150, 24)
            Me.bIMuse.TabIndex = 1
            Me.bIMuse.Text = "&iMuse setup tool"
            Me.bIMuse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bIMuse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bIMuse.UseVisualStyleBackColor = True
            '
            'bSetup
            '
            Me.bSetup.Image = Global.My.Resources.Resources.silk_controller
            Me.bSetup.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bSetup.Location = New System.Drawing.Point(6, 19)
            Me.bSetup.Name = "bSetup"
            Me.bSetup.Size = New System.Drawing.Size(150, 24)
            Me.bSetup.TabIndex = 0
            Me.bSetup.Text = "&Setup tool"
            Me.bSetup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bSetup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bSetup.UseVisualStyleBackColor = True
            '
            'bBrowseMods
            '
            Me.bBrowseMods.Image = Global.My.Resources.Resources.silk_folder_explore
            Me.bBrowseMods.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bBrowseMods.Location = New System.Drawing.Point(6, 79)
            Me.bBrowseMods.Name = "bBrowseMods"
            Me.bBrowseMods.Size = New System.Drawing.Size(150, 24)
            Me.bBrowseMods.TabIndex = 2
            Me.bBrowseMods.Text = "Change &mods folder"
            Me.bBrowseMods.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bBrowseMods.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bBrowseMods.UseVisualStyleBackColor = True
            '
            'bApply
            '
            Me.bApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bApply.Image = Global.My.Resources.Resources.silk_disk
            Me.bApply.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bApply.Location = New System.Drawing.Point(401, 220)
            Me.bApply.Name = "bApply"
            Me.bApply.Size = New System.Drawing.Size(75, 24)
            Me.bApply.TabIndex = 0
            Me.bApply.Text = "&Apply"
            Me.bApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bApply.UseVisualStyleBackColor = True
            '
            'bCancel
            '
            Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bCancel.Image = Global.My.Resources.Resources.silk_cross
            Me.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bCancel.Location = New System.Drawing.Point(320, 220)
            Me.bCancel.Name = "bCancel"
            Me.bCancel.Size = New System.Drawing.Size(75, 24)
            Me.bCancel.TabIndex = 7
            Me.bCancel.Text = "Close"
            Me.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bCancel.UseVisualStyleBackColor = True
            '
            'bOK
            '
            Me.bOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bOK.Image = Global.My.Resources.Resources.silk_tick
            Me.bOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bOK.Location = New System.Drawing.Point(239, 220)
            Me.bOK.Name = "bOK"
            Me.bOK.Size = New System.Drawing.Size(75, 24)
            Me.bOK.TabIndex = 6
            Me.bOK.Text = "OK"
            Me.bOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bOK.UseVisualStyleBackColor = True
            '
            'fbdDOSBox
            '
            Me.fbdDOSBox.Description = "Select the folder where DOSBox is installed."
            Me.fbdDOSBox.ShowNewFolderButton = False
            '
            'fbdDarkXL
            '
            Me.fbdDarkXL.Description = "Select the folder where DarkXL is installed."
            Me.fbdDarkXL.ShowNewFolderButton = False
            '
            'fbdMods
            '
            Me.fbdMods.Description = "Select the folder which contains all your mod folders and/or ZIPs."
            '
            'fDarkForcesOptions
            '
            Me.AcceptButton = Me.bOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.bCancel
            Me.ClientSize = New System.Drawing.Size(488, 256)
            Me.Controls.Add(Me.bOK)
            Me.Controls.Add(Me.bCancel)
            Me.Controls.Add(Me.bApply)
            Me.Controls.Add(ghConfigure)
            Me.Controls.Add(gbDebug)
            Me.Controls.Add(gbCD)
            Me.Controls.Add(gbInGame)
            Me.Controls.Add(gbRun)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "fDarkForcesOptions"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Dark Forces Options - Knight"
            gbRun.ResumeLayout(False)
            gbRun.PerformLayout()
            gbInGame.ResumeLayout(False)
            gbInGame.PerformLayout()
            gbCD.ResumeLayout(False)
            gbCD.PerformLayout()
            gbDebug.ResumeLayout(False)
            gbDebug.PerformLayout()
            ghConfigure.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents rbDarkXL As System.Windows.Forms.RadioButton
        Friend WithEvents rbDOSBox As System.Windows.Forms.RadioButton
        Friend WithEvents rbNative As System.Windows.Forms.RadioButton
        Friend WithEvents bBrowseDarkXL As System.Windows.Forms.Button
        Friend WithEvents bBrowseDOSBox As System.Windows.Forms.Button
        Friend WithEvents cbLevel As System.Windows.Forms.ComboBox
        Friend WithEvents cbLevelSelect As System.Windows.Forms.CheckBox
        Friend WithEvents cbScreenshots As System.Windows.Forms.CheckBox
        Friend WithEvents cbSkipCutscenes As System.Windows.Forms.CheckBox
        Friend WithEvents dudCD As System.Windows.Forms.DomainUpDown
        Friend WithEvents cbNoCD As System.Windows.Forms.CheckBox
        Friend WithEvents cbForceCD As System.Windows.Forms.CheckBox
        Friend WithEvents cbSkipFiles As System.Windows.Forms.CheckBox
        Friend WithEvents cbSkipMemory As System.Windows.Forms.CheckBox
        Friend WithEvents cbAutoTest As System.Windows.Forms.CheckBox
        Friend WithEvents cbLog As System.Windows.Forms.CheckBox
        Friend WithEvents bBrowseMods As System.Windows.Forms.Button
        Friend WithEvents bSetup As System.Windows.Forms.Button
        Friend WithEvents bIMuse As System.Windows.Forms.Button
        Friend WithEvents bApply As System.Windows.Forms.Button
        Friend WithEvents bCancel As System.Windows.Forms.Button
        Friend WithEvents bOK As System.Windows.Forms.Button
        Friend WithEvents ec As MZZT.EventControl
        Friend WithEvents fbdDOSBox As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents fbdDarkXL As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents fbdMods As System.Windows.Forms.FolderBrowserDialog
    End Class
End Namespace

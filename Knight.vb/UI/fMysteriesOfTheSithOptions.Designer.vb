Namespace MZZT.Knight.Games
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class fMysteriesOfTheSithOptions
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
            Dim gMods As System.Windows.Forms.GroupBox
            Dim gbDebug As System.Windows.Forms.GroupBox
            Dim gbDisplay As System.Windows.Forms.GroupBox
            Dim gbInGame As System.Windows.Forms.GroupBox
            Me.bActiveMods = New System.Windows.Forms.Button
            Me.bMods = New System.Windows.Forms.Button
            Me.nudVerbose = New System.Windows.Forms.NumericUpDown
            Me.cbVerbose = New System.Windows.Forms.CheckBox
            Me.cbLogResources = New System.Windows.Forms.CheckBox
            Me.cbLogCog = New System.Windows.Forms.CheckBox
            Me.rbLogCon = New System.Windows.Forms.RadioButton
            Me.rbLogFile = New System.Windows.Forms.RadioButton
            Me.rbLogNo = New System.Windows.Forms.RadioButton
            Me.cbMultipleInstances = New System.Windows.Forms.CheckBox
            Me.cbPlayerStatus = New System.Windows.Forms.CheckBox
            Me.cbFramerate = New System.Windows.Forms.CheckBox
            Me.cbRecord = New System.Windows.Forms.CheckBox
            Me.cbNoHUD = New System.Windows.Forms.CheckBox
            Me.cbWindowedGUI = New System.Windows.Forms.CheckBox
            Me.cbAdvancedDisplayOptions = New System.Windows.Forms.CheckBox
            Me.cbSpeedUp = New System.Windows.Forms.CheckBox
            Me.cbConsole = New System.Windows.Forms.CheckBox
            Me.bDirectPlay = New System.Windows.Forms.Button
            Me.bOK = New System.Windows.Forms.Button
            Me.fbdMods = New System.Windows.Forms.FolderBrowserDialog
            Me.ec = New MZZT.EventControl
            Me.bCancel = New System.Windows.Forms.Button
            Me.bApply = New System.Windows.Forms.Button
            Me.fbdActiveMods = New System.Windows.Forms.FolderBrowserDialog
            Me.fbdPatches = New System.Windows.Forms.FolderBrowserDialog
            Me.bPatches = New System.Windows.Forms.Button
            gMods = New System.Windows.Forms.GroupBox
            gbDebug = New System.Windows.Forms.GroupBox
            gbDisplay = New System.Windows.Forms.GroupBox
            gbInGame = New System.Windows.Forms.GroupBox
            gMods.SuspendLayout()
            gbDebug.SuspendLayout()
            CType(Me.nudVerbose, System.ComponentModel.ISupportInitialize).BeginInit()
            gbDisplay.SuspendLayout()
            gbInGame.SuspendLayout()
            Me.SuspendLayout()
            '
            'gMods
            '
            gMods.Controls.Add(Me.bPatches)
            gMods.Controls.Add(Me.bActiveMods)
            gMods.Controls.Add(Me.bMods)
            gMods.Location = New System.Drawing.Point(407, 86)
            gMods.Name = "gMods"
            gMods.Size = New System.Drawing.Size(162, 109)
            gMods.TabIndex = 3
            gMods.TabStop = False
            gMods.Text = "Mod folders"
            '
            'bActiveMods
            '
            Me.bActiveMods.Image = Global.My.Resources.Resources.silk_folder_explore
            Me.bActiveMods.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bActiveMods.Location = New System.Drawing.Point(6, 49)
            Me.bActiveMods.Name = "bActiveMods"
            Me.bActiveMods.Size = New System.Drawing.Size(150, 24)
            Me.bActiveMods.TabIndex = 1
            Me.bActiveMods.Text = "Change ac&tive folder"
            Me.bActiveMods.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bActiveMods.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bActiveMods.UseVisualStyleBackColor = True
            '
            'bMods
            '
            Me.bMods.Image = Global.My.Resources.Resources.silk_folder_explore
            Me.bMods.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bMods.Location = New System.Drawing.Point(6, 19)
            Me.bMods.Name = "bMods"
            Me.bMods.Size = New System.Drawing.Size(150, 24)
            Me.bMods.TabIndex = 0
            Me.bMods.Text = "Change &inactive folder"
            Me.bMods.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bMods.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bMods.UseVisualStyleBackColor = True
            '
            'gbDebug
            '
            gbDebug.Controls.Add(Me.nudVerbose)
            gbDebug.Controls.Add(Me.cbVerbose)
            gbDebug.Controls.Add(Me.cbLogResources)
            gbDebug.Controls.Add(Me.cbLogCog)
            gbDebug.Controls.Add(Me.rbLogCon)
            gbDebug.Controls.Add(Me.rbLogFile)
            gbDebug.Controls.Add(Me.rbLogNo)
            gbDebug.Controls.Add(Me.cbMultipleInstances)
            gbDebug.Location = New System.Drawing.Point(209, 12)
            gbDebug.Name = "gbDebug"
            gbDebug.Size = New System.Drawing.Size(192, 183)
            gbDebug.TabIndex = 1
            gbDebug.TabStop = False
            gbDebug.Text = "Debug"
            '
            'nudVerbose
            '
            Me.nudVerbose.Enabled = False
            Me.nudVerbose.Location = New System.Drawing.Point(104, 111)
            Me.nudVerbose.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
            Me.nudVerbose.Name = "nudVerbose"
            Me.nudVerbose.Size = New System.Drawing.Size(30, 20)
            Me.nudVerbose.TabIndex = 5
            '
            'cbVerbose
            '
            Me.cbVerbose.AutoSize = True
            Me.cbVerbose.Location = New System.Drawing.Point(6, 112)
            Me.cbVerbose.Name = "cbVerbose"
            Me.cbVerbose.Size = New System.Drawing.Size(92, 17)
            Me.cbVerbose.TabIndex = 4
            Me.cbVerbose.Text = "Log &verbosity:"
            Me.cbVerbose.UseVisualStyleBackColor = True
            '
            'cbLogResources
            '
            Me.cbLogResources.AutoSize = True
            Me.cbLogResources.Location = New System.Drawing.Point(6, 160)
            Me.cbLogResources.Name = "cbLogResources"
            Me.cbLogResources.Size = New System.Drawing.Size(180, 17)
            Me.cbLogResources.TabIndex = 7
            Me.cbLogResources.Text = "Log &resource files used to fail.log"
            Me.cbLogResources.UseVisualStyleBackColor = True
            '
            'cbLogCog
            '
            Me.cbLogCog.AutoSize = True
            Me.cbLogCog.Location = New System.Drawing.Point(6, 137)
            Me.cbLogCog.Name = "cbLogCog"
            Me.cbLogCog.Size = New System.Drawing.Size(165, 17)
            Me.cbLogCog.TabIndex = 6
            Me.cbLogCog.Text = "Log cog m&essages to cog.log"
            Me.cbLogCog.UseVisualStyleBackColor = True
            '
            'rbLogCon
            '
            Me.rbLogCon.AutoSize = True
            Me.rbLogCon.Location = New System.Drawing.Point(6, 88)
            Me.rbLogCon.Name = "rbLogCon"
            Me.rbLogCon.Size = New System.Drawing.Size(142, 17)
            Me.rbLogCon.TabIndex = 3
            Me.rbLogCon.Text = "Log to &Windows console"
            Me.rbLogCon.UseVisualStyleBackColor = True
            '
            'rbLogFile
            '
            Me.rbLogFile.AutoSize = True
            Me.rbLogFile.Location = New System.Drawing.Point(6, 65)
            Me.rbLogFile.Name = "rbLogFile"
            Me.rbLogFile.Size = New System.Drawing.Size(105, 17)
            Me.rbLogFile.TabIndex = 2
            Me.rbLogFile.Text = "&Log to debug.log"
            Me.rbLogFile.UseVisualStyleBackColor = True
            '
            'rbLogNo
            '
            Me.rbLogNo.AutoSize = True
            Me.rbLogNo.Checked = True
            Me.rbLogNo.Location = New System.Drawing.Point(6, 42)
            Me.rbLogNo.Name = "rbLogNo"
            Me.rbLogNo.Size = New System.Drawing.Size(76, 17)
            Me.rbLogNo.TabIndex = 1
            Me.rbLogNo.TabStop = True
            Me.rbLogNo.Text = "&No logging"
            Me.rbLogNo.UseVisualStyleBackColor = True
            '
            'cbMultipleInstances
            '
            Me.cbMultipleInstances.AutoSize = True
            Me.cbMultipleInstances.Checked = True
            Me.cbMultipleInstances.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbMultipleInstances.Location = New System.Drawing.Point(6, 19)
            Me.cbMultipleInstances.Name = "cbMultipleInstances"
            Me.cbMultipleInstances.Size = New System.Drawing.Size(139, 17)
            Me.cbMultipleInstances.TabIndex = 0
            Me.cbMultipleInstances.Text = "Allow &Multiple Instances"
            Me.cbMultipleInstances.UseVisualStyleBackColor = True
            '
            'gbDisplay
            '
            gbDisplay.Controls.Add(Me.cbPlayerStatus)
            gbDisplay.Controls.Add(Me.cbFramerate)
            gbDisplay.Controls.Add(Me.cbRecord)
            gbDisplay.Controls.Add(Me.cbNoHUD)
            gbDisplay.Controls.Add(Me.cbWindowedGUI)
            gbDisplay.Controls.Add(Me.cbAdvancedDisplayOptions)
            gbDisplay.Location = New System.Drawing.Point(12, 12)
            gbDisplay.Name = "gbDisplay"
            gbDisplay.Size = New System.Drawing.Size(191, 183)
            gbDisplay.TabIndex = 0
            gbDisplay.TabStop = False
            gbDisplay.Text = "Display"
            '
            'cbPlayerStatus
            '
            Me.cbPlayerStatus.AutoSize = True
            Me.cbPlayerStatus.Location = New System.Drawing.Point(6, 88)
            Me.cbPlayerStatus.Name = "cbPlayerStatus"
            Me.cbPlayerStatus.Size = New System.Drawing.Size(121, 17)
            Me.cbPlayerStatus.TabIndex = 3
            Me.cbPlayerStatus.Text = "Player &status in chat"
            Me.cbPlayerStatus.UseVisualStyleBackColor = True
            '
            'cbFramerate
            '
            Me.cbFramerate.AutoSize = True
            Me.cbFramerate.Location = New System.Drawing.Point(6, 65)
            Me.cbFramerate.Name = "cbFramerate"
            Me.cbFramerate.Size = New System.Drawing.Size(142, 17)
            Me.cbFramerate.TabIndex = 2
            Me.cbFramerate.Text = "Display &framerate in chat"
            Me.cbFramerate.UseVisualStyleBackColor = True
            '
            'cbRecord
            '
            Me.cbRecord.AutoSize = True
            Me.cbRecord.Location = New System.Drawing.Point(6, 134)
            Me.cbRecord.Name = "cbRecord"
            Me.cbRecord.Size = New System.Drawing.Size(157, 17)
            Me.cbRecord.TabIndex = 5
            Me.cbRecord.Text = "&Dump video to PCXs/BMPs"
            Me.cbRecord.UseVisualStyleBackColor = True
            '
            'cbNoHUD
            '
            Me.cbNoHUD.AutoSize = True
            Me.cbNoHUD.Location = New System.Drawing.Point(6, 111)
            Me.cbNoHUD.Name = "cbNoHUD"
            Me.cbNoHUD.Size = New System.Drawing.Size(67, 17)
            Me.cbNoHUD.TabIndex = 4
            Me.cbNoHUD.Text = "No &HUD"
            Me.cbNoHUD.UseVisualStyleBackColor = True
            '
            'cbWindowedGUI
            '
            Me.cbWindowedGUI.AutoSize = True
            Me.cbWindowedGUI.Checked = True
            Me.cbWindowedGUI.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbWindowedGUI.Location = New System.Drawing.Point(6, 19)
            Me.cbWindowedGUI.Name = "cbWindowedGUI"
            Me.cbWindowedGUI.Size = New System.Drawing.Size(179, 17)
            Me.cbWindowedGUI.TabIndex = 0
            Me.cbWindowedGUI.Text = "&GUI/Cutscene Windowed Mode"
            Me.cbWindowedGUI.UseVisualStyleBackColor = True
            '
            'cbAdvancedDisplayOptions
            '
            Me.cbAdvancedDisplayOptions.AutoSize = True
            Me.cbAdvancedDisplayOptions.Checked = True
            Me.cbAdvancedDisplayOptions.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbAdvancedDisplayOptions.Location = New System.Drawing.Point(6, 42)
            Me.cbAdvancedDisplayOptions.Name = "cbAdvancedDisplayOptions"
            Me.cbAdvancedDisplayOptions.Size = New System.Drawing.Size(151, 17)
            Me.cbAdvancedDisplayOptions.TabIndex = 1
            Me.cbAdvancedDisplayOptions.Text = "Advanced Display &Options"
            Me.cbAdvancedDisplayOptions.UseVisualStyleBackColor = True
            '
            'gbInGame
            '
            gbInGame.Controls.Add(Me.cbSpeedUp)
            gbInGame.Controls.Add(Me.cbConsole)
            gbInGame.Location = New System.Drawing.Point(407, 12)
            gbInGame.Name = "gbInGame"
            gbInGame.Size = New System.Drawing.Size(162, 68)
            gbInGame.TabIndex = 2
            gbInGame.TabStop = False
            gbInGame.Text = "In-Game"
            '
            'cbSpeedUp
            '
            Me.cbSpeedUp.AutoSize = True
            Me.cbSpeedUp.Location = New System.Drawing.Point(6, 42)
            Me.cbSpeedUp.Name = "cbSpeedUp"
            Me.cbSpeedUp.Size = New System.Drawing.Size(101, 17)
            Me.cbSpeedUp.TabIndex = 1
            Me.cbSpeedUp.Text = "Speed &up game"
            Me.cbSpeedUp.UseVisualStyleBackColor = True
            '
            'cbConsole
            '
            Me.cbConsole.AutoSize = True
            Me.cbConsole.Location = New System.Drawing.Point(6, 19)
            Me.cbConsole.Name = "cbConsole"
            Me.cbConsole.Size = New System.Drawing.Size(141, 17)
            Me.cbConsole.TabIndex = 0
            Me.cbConsole.Text = "&Console and level select"
            Me.cbConsole.UseVisualStyleBackColor = True
            '
            'bDirectPlay
            '
            Me.bDirectPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.bDirectPlay.Image = Global.My.Resources.Resources.silk_group
            Me.bDirectPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bDirectPlay.Location = New System.Drawing.Point(12, 201)
            Me.bDirectPlay.Name = "bDirectPlay"
            Me.bDirectPlay.Size = New System.Drawing.Size(150, 24)
            Me.bDirectPlay.TabIndex = 4
            Me.bDirectPlay.Text = "DirectPla&y options"
            Me.bDirectPlay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bDirectPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bDirectPlay.UseVisualStyleBackColor = True
            '
            'bOK
            '
            Me.bOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bOK.Image = Global.My.Resources.Resources.silk_tick
            Me.bOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bOK.Location = New System.Drawing.Point(332, 201)
            Me.bOK.Name = "bOK"
            Me.bOK.Size = New System.Drawing.Size(75, 24)
            Me.bOK.TabIndex = 5
            Me.bOK.Text = "OK"
            Me.bOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bOK.UseVisualStyleBackColor = True
            '
            'fbdMods
            '
            Me.fbdMods.Description = "Select the folder which contains all your GOOs and/or ZIPs."
            '
            'bCancel
            '
            Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bCancel.Image = Global.My.Resources.Resources.silk_cross
            Me.bCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bCancel.Location = New System.Drawing.Point(413, 201)
            Me.bCancel.Name = "bCancel"
            Me.bCancel.Size = New System.Drawing.Size(75, 24)
            Me.bCancel.TabIndex = 6
            Me.bCancel.Text = "Close"
            Me.bCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bCancel.UseVisualStyleBackColor = True
            '
            'bApply
            '
            Me.bApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bApply.Image = Global.My.Resources.Resources.silk_disk
            Me.bApply.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bApply.Location = New System.Drawing.Point(494, 201)
            Me.bApply.Name = "bApply"
            Me.bApply.Size = New System.Drawing.Size(75, 24)
            Me.bApply.TabIndex = 7
            Me.bApply.Text = "&Apply"
            Me.bApply.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bApply.UseVisualStyleBackColor = True
            '
            'fbdActiveMods
            '
            Me.fbdActiveMods.Description = "Select the folder where you want active mods to go."
            '
            'fbdPatches
            '
            Me.fbdPatches.Description = "Select the folder which contains all your patches."
            '
            'bPatches
            '
            Me.bPatches.Image = Global.My.Resources.Resources.silk_folder_explore
            Me.bPatches.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.bPatches.Location = New System.Drawing.Point(6, 79)
            Me.bPatches.Name = "bPatches"
            Me.bPatches.Size = New System.Drawing.Size(150, 24)
            Me.bPatches.TabIndex = 1
            Me.bPatches.Text = "Change &patch folder"
            Me.bPatches.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.bPatches.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
            Me.bPatches.UseVisualStyleBackColor = True
            '
            'fMysteriesOfTheSithOptions
            '
            Me.AcceptButton = Me.bOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.bCancel
            Me.ClientSize = New System.Drawing.Size(581, 237)
            Me.Controls.Add(gMods)
            Me.Controls.Add(gbDebug)
            Me.Controls.Add(Me.bDirectPlay)
            Me.Controls.Add(Me.bOK)
            Me.Controls.Add(Me.bCancel)
            Me.Controls.Add(Me.bApply)
            Me.Controls.Add(gbDisplay)
            Me.Controls.Add(gbInGame)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "fMysteriesOfTheSithOptions"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Mysteries of the Sith Options - Knight"
            gMods.ResumeLayout(False)
            gbDebug.ResumeLayout(False)
            gbDebug.PerformLayout()
            CType(Me.nudVerbose, System.ComponentModel.ISupportInitialize).EndInit()
            gbDisplay.ResumeLayout(False)
            gbDisplay.PerformLayout()
            gbInGame.ResumeLayout(False)
            gbInGame.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents bActiveMods As System.Windows.Forms.Button
        Friend WithEvents bMods As System.Windows.Forms.Button
        Friend WithEvents nudVerbose As System.Windows.Forms.NumericUpDown
        Friend WithEvents cbVerbose As System.Windows.Forms.CheckBox
        Friend WithEvents rbLogCon As System.Windows.Forms.RadioButton
        Friend WithEvents rbLogFile As System.Windows.Forms.RadioButton
        Friend WithEvents rbLogNo As System.Windows.Forms.RadioButton
        Friend WithEvents cbMultipleInstances As System.Windows.Forms.CheckBox
        Friend WithEvents bDirectPlay As System.Windows.Forms.Button
        Friend WithEvents bOK As System.Windows.Forms.Button
        Friend WithEvents fbdMods As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents ec As MZZT.EventControl
        Friend WithEvents bCancel As System.Windows.Forms.Button
        Friend WithEvents bApply As System.Windows.Forms.Button
        Friend WithEvents cbAdvancedDisplayOptions As System.Windows.Forms.CheckBox
        Friend WithEvents cbConsole As System.Windows.Forms.CheckBox
        Friend WithEvents cbPlayerStatus As System.Windows.Forms.CheckBox
        Friend WithEvents cbFramerate As System.Windows.Forms.CheckBox
        Friend WithEvents cbNoHUD As System.Windows.Forms.CheckBox
        Friend WithEvents cbWindowedGUI As System.Windows.Forms.CheckBox
        Friend WithEvents fbdActiveMods As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents cbSpeedUp As System.Windows.Forms.CheckBox
        Friend WithEvents cbRecord As System.Windows.Forms.CheckBox
        Friend WithEvents cbLogResources As System.Windows.Forms.CheckBox
        Friend WithEvents cbLogCog As System.Windows.Forms.CheckBox
        Friend WithEvents fbdPatches As System.Windows.Forms.FolderBrowserDialog
        Friend WithEvents bPatches As System.Windows.Forms.Button
    End Class
End Namespace
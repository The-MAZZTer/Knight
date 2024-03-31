Imports System.IO
Imports System.Text

Namespace MZZT.Knight.Games
    Public Class fMysteriesOfTheSithOptions
        Public Sub New(ByVal mots As MysteriesOfTheSith)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _mots = mots
            Icon = WinAPI.BitmapToIcon(mots.Image32)
            ec.Allow()
        End Sub
        Private _mots As MysteriesOfTheSith

        Private Sub ItemChanged()
            If ec.EventsAllowed Then
                bOK.Enabled = True
                bApply.Enabled = True
                bCancel.Text = "Cancel"
            End If
        End Sub

#Region "Load/Save Settings"
        Private Sub Apply()
            MyOptions.BeginUpdate()

            MyOptions.MysteriesOfTheSithWindowedGUI = cbWindowedGUI.Checked
            MyOptions.MysteriesOfTheSithAdvancedDisplayOptions = _
                cbAdvancedDisplayOptions.Checked
            MyOptions.MysteriesOfTheSithFramerate = cbFramerate.Checked
            MyOptions.MysteriesOfTheSithConsoleStats = cbPlayerStatus.Checked
            MyOptions.MysteriesOfTheSithNoHUD = cbNoHUD.Checked
            MyOptions.MysteriesOfTheSithRecord = cbRecord.Checked

            MyOptions.MysteriesOfTheSithAllowMultipleInstances = cbMultipleInstances. _
                Checked
            If rbLogNo.Checked Then
                MyOptions.MysteriesOfTheSithLog = Options.LogTypes.None
            ElseIf rbLogFile.Checked Then
                MyOptions.MysteriesOfTheSithLog = Options.LogTypes.File
            ElseIf rbLogCon.Checked Then
                MyOptions.MysteriesOfTheSithLog = Options.LogTypes.WindowsConsole
            End If

            If cbVerbose.Checked Then
                MyOptions.MysteriesOfTheSithVerbosity = nudVerbose.Value
            Else
                MyOptions.MysteriesOfTheSithVerbosity = -1
            End If
            MyOptions.MysteriesOfTheSithCogLog = cbLogCog.Checked
            MyOptions.MysteriesOfTheSithResLog = cbLogResources.Checked

            MyOptions.MysteriesOfTheSithConsole = cbConsole.Checked
            MyOptions.MysteriesOfTheSithSpeedUp = cbSpeedUp.Checked

            _mots.ModPath = fbdMods.SelectedPath
            _mots.ActiveModPath = fbdActiveMods.SelectedPath
            _mots.PatchPath = fbdPatches.SelectedPath

            MyOptions.EndUpdate()

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"
        End Sub

        Private Sub Revert()
            ec.Block()

            cbWindowedGUI.Checked = MyOptions.MysteriesOfTheSithWindowedGUI
            cbAdvancedDisplayOptions.Checked = MyOptions. _
                MysteriesOfTheSithAdvancedDisplayOptions
            cbFramerate.Checked = MyOptions.MysteriesOfTheSithFramerate
            cbPlayerStatus.Checked = MyOptions.MysteriesOfTheSithConsoleStats
            cbNoHUD.Checked = MyOptions.MysteriesOfTheSithNoHUD
            cbRecord.Checked = MyOptions.MysteriesOfTheSithRecord

            cbMultipleInstances.Checked = MyOptions. _
                MysteriesOfTheSithAllowMultipleInstances
            rbLogNo.Checked = MyOptions.MysteriesOfTheSithLog = Options.LogTypes.None
            rbLogFile.Checked = MyOptions.MysteriesOfTheSithLog = Options.LogTypes.File
            rbLogCon.Checked = MyOptions.MysteriesOfTheSithLog = Options.LogTypes. _
                WindowsConsole

            cbVerbose.Checked = MyOptions.MysteriesOfTheSithVerbosity > -1
            If cbVerbose.Checked Then
                nudVerbose.Value = MyOptions.MysteriesOfTheSithVerbosity
            Else
                nudVerbose.Value = 0
            End If

            cbLogCog.Checked = MyOptions.MysteriesOfTheSithCogLog
            cbLogResources.Checked = MyOptions.MysteriesOfTheSithResLog

            cbConsole.Checked = MyOptions.MysteriesOfTheSithConsole
            cbSpeedUp.Checked = MyOptions.MysteriesOfTheSithSpeedUp

            fbdMods.SelectedPath = _mots.GetModPath
            fbdActiveMods.SelectedPath = _mots.GetActiveModPath
            fbdPatches.SelectedPath = _mots.PatchPath

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"

            ec.Allow()
        End Sub
#End Region

#Region "Event Handlers"
        Private Sub bActiveMods_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles bActiveMods.Click

            Dim oldpath As String = fbdActiveMods.SelectedPath
            If fbdActiveMods.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If _mots.IsValidActiveModPath(fbdActiveMods.SelectedPath) Then
                    ItemChanged()
                Else
                    MsgBox("Invalid active mod path.  The active mod path must be " & _
                        "in a subfolder of the game path, must not be the inactive " & _
                        "mod path, and the folder names from the game path down " & _
                        "must not have any spaces.  Your change has been reverted.", _
                        MsgBoxStyle.Exclamation, "Error - Knight")
                    fbdActiveMods.SelectedPath = oldpath
                End If
            End If
        End Sub

        Private Sub bApply_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bApply.Click

            Apply()
        End Sub

        Private Sub bDirectPlay_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles bDirectPlay.Click

            If bApply.Enabled Then
                Apply()
            End If

            Dim f As New fDirectPlay(_mots)
            f.ShowDialog()
        End Sub

        Private Sub bMods_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles bMods.Click

            Dim oldpath As String = fbdMods.SelectedPath
            If fbdMods.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If fbdActiveMods.SelectedPath.ToLower.TrimEnd(Path. _
                    DirectorySeparatorChar) <> fbdMods.SelectedPath.ToLower. _
                    TrimEnd(Path.DirectorySeparatorChar) Then

                    ItemChanged()
                Else
                    MsgBox("The inactive mod path must be different from the " & _
                        "active mod path.  Your change has been reverted.", _
                        MsgBoxStyle.Exclamation, "Error - Knight")
                    fbdActiveMods.SelectedPath = oldpath
                End If
            End If
        End Sub

        Private Sub bOK_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bOK.Click

            Apply()
        End Sub

        Private Sub bPatches_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles bPatches.Click

            If fbdPatches.ShowDialog() = Windows.Forms.DialogResult.OK Then
                ItemChanged()
            End If
        End Sub

        Private Sub cbLogMotS_CheckedChanged(ByVal sender As System.Object, ByVal e _
            As System.EventArgs) Handles cbLogCog.CheckedChanged, cbLogResources. _
            CheckedChanged

            If Not ec.EventsAllowed Then
                Return
            End If

            If DirectCast(sender, CheckBox).Checked Then
                MsgBox("Warning: This option may slow the game significantly.", _
                    MsgBoxStyle.Exclamation, "Warning - Knight")
            End If
        End Sub

        Private Sub cbRecord_CheckedChanged(ByVal sender As System.Object, ByVal e _
            As System.EventArgs) Handles cbRecord.CheckedChanged

            If Not ec.EventsAllowed Then
                Return
            End If

            If DirectCast(sender, CheckBox).Checked Then
                MsgBox("Warning: This option will eat up lots of disk space very " & _
                    "quickly at high resolutions.  At 1280x1024 and 60 frames per " & _
                    "second this will consume 240mb per second!  However it will " & _
                    "also greatly slow down rendering at higher speeds as each " & _
                    "frame must be written to disk before the next can be " & _
                    "rendered.  Uncompressed BMPs are used for larger resolutions " & _
                    "while compressed PCXs are used for smaller ones such as 640x480.", _
                    MsgBoxStyle.Exclamation, "Warning - Knight")
            End If
        End Sub

        Private Sub cbVerbose_CheckedChanged(ByVal sender As System.Object, ByVal e _
            As System.EventArgs) Handles cbVerbose.CheckedChanged

            nudVerbose.Enabled = DirectCast(sender, CheckBox).Checked

            If Not ec.EventsAllowed Then
                Return
            End If

            If Not cbVerbose.Checked AndAlso rbLogCon.Checked Then
                rbLogCon.Checked = False
                rbLogNo.Checked = True
            End If
        End Sub

        Private Sub Changed(ByVal sender As Object, ByVal e As EventArgs) Handles _
            cbWindowedGUI.CheckedChanged, cbAdvancedDisplayOptions.CheckedChanged, _
            cbFramerate.CheckedChanged, cbPlayerStatus.CheckedChanged, cbNoHUD. _
            CheckedChanged, cbMultipleInstances.CheckedChanged, rbLogNo.CheckedChanged, _
            rbLogFile.CheckedChanged, rbLogCon.CheckedChanged, cbVerbose. _
            CheckedChanged, nudVerbose.ValueChanged, cbConsole.CheckedChanged, _
            cbRecord.CheckedChanged, cbLogResources.CheckedChanged, cbLogCog. _
            CheckedChanged, cbSpeedUp.CheckedChanged

            ItemChanged()
        End Sub

        Private Sub nudVerbose_ValueChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles nudVerbose.ValueChanged

            If Not ec.EventsAllowed Then
                Return
            End If

            If nudVerbose.Value = 0 AndAlso rbLogCon.Checked Then
                rbLogCon.Checked = False
                rbLogNo.Checked = True
            End If
        End Sub

        Private Sub rbLogCon_CheckedChanged(ByVal sender As System.Object, ByVal e _
            As System.EventArgs) Handles rbLogCon.CheckedChanged

            If Not ec.EventsAllowed Then
                Return
            End If

            If Not rbLogCon.Checked Then
                Return
            End If

            cbVerbose.Checked = True
            If nudVerbose.Value < 1 Then
                nudVerbose.Value = 1
            End If
        End Sub

        Public Overloads Function ShowDialog() As DialogResult
            Revert()
            Return MyBase.ShowDialog()
        End Function
#End Region
    End Class
End Namespace

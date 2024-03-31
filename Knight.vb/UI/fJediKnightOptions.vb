Imports System.IO
Imports System.Text

Namespace MZZT.Knight.Games
    Public Class fJediKnightOptions
        Private JK_CD1 As Byte() = New Byte() {&HC4, &H32, &H92, &H69}
        Private JK_CD2 As Byte() = New Byte() {&H84, &H33, &H92, &H69}

        Public Sub New(ByVal jk As JediKnight)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _jk = jk
            Icon = WinAPI.BitmapToIcon(jk.Image32)
            ec.Allow()
        End Sub
        Private _jk As JediKnight

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

            Dim fi As New FileInfo(Path.Combine(_jk.GamePath, "Resource\JK_.CD"))
            If rbCDNone.Checked Then
                If fi.Exists Then
                    fi.Delete()
                End If
            ElseIf rbCD1.Checked OrElse rbCD2.Checked Then
                Dim fs As FileStream = fi.Open(FileMode.Create, FileAccess.Write)
                Dim b As Byte()
                If rbCD1.Checked Then
                    b = JK_CD1
                Else
                    b = JK_CD2
                End If
                fs.Write(b, 0, b.Length)
                fs.Close()
                fs.Dispose()
            End If

            MyOptions.JediKnightWindowedGUI = cbWindowedGUI.Checked
            MyOptions.JediKnightAdvancedDisplayOptions = cbAdvancedDisplayOptions.Checked
            MyOptions.JediKnightFramerate = cbFramerate.Checked
            MyOptions.JediKnightConsoleStats = cbPlayerStatus.Checked
            MyOptions.JediKnightNoHUD = cbNoHUD.Checked

            MyOptions.JediKnightAllowMultipleInstances = cbMultipleInstances.Checked
            If rbLogNo.Checked Then
                MyOptions.JediKnightLog = Options.LogTypes.None
            ElseIf rbLogFile.Checked Then
                MyOptions.JediKnightLog = Options.LogTypes.File
            ElseIf rbLogCon.Checked Then
                MyOptions.JediKnightLog = Options.LogTypes.WindowsConsole
            End If

            If cbVerbose.Checked Then
                MyOptions.JediKnightVerbosity = nudVerbose.Value
            Else
                MyOptions.JediKnightVerbosity = -1
            End If

            _jk.ModPath = fbdMods.SelectedPath 
            _jk.ActiveModPath = fbdActiveMods.SelectedPath
            _jk.PatchPath = fbdPatches.SelectedPath

            MyOptions.JediKnightConsole = cbConsole.Checked
            MyOptions.JediKnightEnableJKUPCogVerbs = cbJKUPVerbs.Checked

            MyOptions.EndUpdate()

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"
        End Sub

        Private Sub Revert()
            ec.Block()

            Dim fi As New FileInfo(Path.Combine(_jk.GamePath, "Resource\JK_.CD"))
            rbCDNone.Checked = Not fi.Exists
            If fi.Exists AndAlso fi.Length = 4 Then
                Dim fs As FileStream = fi.Open(FileMode.Open, FileAccess.Read)
                Dim b(3) As Byte
                fs.Read(b, 0, b.Length)
                fs.Close()
                fs.Dispose()

                Dim iscd1 As Boolean = True
                Dim iscd2 As Boolean = True
                For i As Byte = 0 To b.Length - 1
                    If b(i) <> JK_CD1(i) Then
                        iscd1 = False
                    End If
                    If b(i) <> JK_CD2(i) Then
                        iscd2 = False
                    End If
                Next i

                rbCD1.Checked = iscd1
                rbCD2.Checked = iscd2
            Else
                rbCD1.Checked = False
                rbCD2.Checked = False
            End If

            cbWindowedGUI.Checked = MyOptions.JediKnightWindowedGUI
            cbAdvancedDisplayOptions.Checked = MyOptions.JediKnightAdvancedDisplayOptions
            cbFramerate.Checked = MyOptions.JediKnightFramerate
            cbPlayerStatus.Checked = MyOptions.JediKnightConsoleStats
            cbNoHUD.Checked = MyOptions.JediKnightNoHUD

            cbMultipleInstances.Checked = MyOptions.JediKnightAllowMultipleInstances
            rbLogNo.Checked = MyOptions.JediKnightLog = Options.LogTypes.None
            rbLogFile.Checked = MyOptions.JediKnightLog = Options.LogTypes.File
            rbLogCon.Checked = MyOptions.JediKnightLog = Options.LogTypes.WindowsConsole

            cbVerbose.Checked = MyOptions.JediKnightVerbosity > -1
            If cbVerbose.Checked Then
                nudVerbose.Value = MyOptions.JediKnightVerbosity
            Else
                nudVerbose.Value = 0
            End If

            fbdMods.SelectedPath = _jk.GetModPath
            fbdActiveMods.SelectedPath = _jk.GetActiveModPath
            fbdPatches.SelectedPath = _jk.PatchPath

            cbConsole.Checked = MyOptions.JediKnightConsole
            cbJKUPVerbs.Checked = MyOptions.JediKnightEnableJKUPCogVerbs

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
                If _jk.IsValidActiveModPath(fbdActiveMods.SelectedPath) Then
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

            Dim f As New fDirectPlay(_jk)
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
            rbCDNone.CheckedChanged, rbCD1.CheckedChanged, rbCD2.CheckedChanged, _
            cbWindowedGUI.CheckedChanged, cbAdvancedDisplayOptions.CheckedChanged, _
            cbFramerate.CheckedChanged, cbPlayerStatus.CheckedChanged, cbNoHUD. _
            CheckedChanged, cbMultipleInstances.CheckedChanged, rbLogNo.CheckedChanged, _
            rbLogFile.CheckedChanged, rbLogCon.CheckedChanged, cbVerbose. _
            CheckedChanged, nudVerbose.ValueChanged, cbConsole.CheckedChanged, _
            cbJKUPVerbs.CheckedChanged

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
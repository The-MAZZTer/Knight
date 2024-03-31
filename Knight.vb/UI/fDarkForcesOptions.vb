Imports System.IO
Imports System.Text

Namespace MZZT.Knight.Games
    Public Class fDarkForcesOptions
        Private Const CD_ID As String = "Dark Forces Version 1.0 (Build 2)" & Chr(&HD) _
            & Chr(&HA)

        Public Sub New(ByVal df As DarkForces)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _df = df
            Icon = WinAPI.BitmapToIcon(df.Image32)
            ec.Allow()
        End Sub
        Private _df As DarkForces

        Public Shared Sub DeleteCDID()
            Dim fi As New FileInfo(Application.ExecutablePath(0) & ":\CD.ID")
            If fi.Exists Then
                fi.Delete()
            End If
        End Sub

        Private Sub EnsureValidRadioSelection()
            If rbNative.Checked Then
                Return
            End If

            If rbDOSBox.Checked AndAlso DarkForces.IsValidDOSBoxPath(fbdDOSBox. _
                SelectedPath) Then

                rbNative.Checked = True
            End If

            If rbDarkXL.Checked AndAlso DarkForces.IsValidDarkXLPath(fbdDarkXL. _
                SelectedPath) Then

                rbNative.Checked = True
            End If
        End Sub

        Private Sub ItemChanged()
            If ec.EventsAllowed Then
                bOK.Enabled = True
                bApply.Enabled = True
                bCancel.Text = "Cancel"
            End If
        End Sub

		Public Shared Sub WriteCDID()
			Dim b As Byte() = Encoding.ASCII.GetBytes(CD_ID)
			Dim fs As New FileStream(Application.ExecutablePath(0) & ":\CD.ID",
								FileMode.Create, FileAccess.Write)
			fs.Write(b, 0, b.Length)
			fs.Close()
			fs.Dispose()
		End Sub

#Region "Load/Save Settings"
		Private Sub Apply()
            MyOptions.BeginUpdate()

            If rbNative.Checked Then
                MyOptions.DarkForcesRunMode = Options.DarkForcesRunModes.Native
            ElseIf rbDOSBox.Checked Then
                MyOptions.DarkForcesRunMode = Options.DarkForcesRunModes.DOSBox
            ElseIf rbDarkXL.Checked Then
                MyOptions.DarkForcesRunMode = Options.DarkForcesRunModes.DarkXL
            End If

            If DarkForces.IsValidDOSBoxPath(fbdDOSBox.SelectedPath) Then
                MyOptions.DarkForcesDOSBoxPath = fbdDOSBox.SelectedPath
            End If
            If DarkForces.IsValidDarkXLPath(fbdDarkXL.SelectedPath) Then
                MyOptions.DarkForcesDarkXLPath = fbdDarkXL.SelectedPath
            End If

            _df.ModPath = fbdMods.SelectedPath

            MyOptions.DarkForcesLog = cbLog.Checked
            MyOptions.DarkForcesSkipMemoryCheck = cbSkipMemory.Checked
            MyOptions.DarkForcesSkipFILESCheck = cbSkipFiles.Checked
            MyOptions.DarkForcesAutoTest = cbAutoTest.Checked

            MyOptions.DarkForcesEnableScreenshots = cbScreenshots.Checked
            If cbLevelSelect.Checked Then
                MyOptions.DarkForcesLevelSelect = cbLevel.Text
            Else
                MyOptions.DarkForcesLevelSelect = Nothing
            End If
            MyOptions.DarkForcesSkipCutscenes = cbSkipCutscenes.Checked

            Dim success As Boolean = True
            If cbNoCD.CheckState = CheckState.Checked Then
                Dim b As Byte() = Encoding.ASCII.GetBytes(CD_ID)

                Dim fs As FileStream
                Dim fi As New FileInfo(Path.Combine(_df.GamePath, "CD.ID"))
                If Not fi.Exists Then
                    fs = fi.Open(FileMode.Create, FileAccess.Write)
                    fs.Write(b, 0, b.Length)
                    fs.Close()
                    fs.Dispose()
                End If

                fs = New FileStream(Path.Combine(_df.GamePath, "DRIVE.CD"), FileMode. _
                    Create, FileAccess.Write)
                If rbNative.Checked OrElse Not DarkForces.IsValidDOSBoxPath(fbdDOSBox. _
                    SelectedPath) Then

                    fs.Write(Encoding.ASCII.GetBytes(Char.ToUpper(Application. _
                        ExecutablePath(0))), 0, 1)
                Else
                    fs.Write(Encoding.ASCII.GetBytes(New Char() {"C"c}), 0, 1)
                End If
                fs.Close()
                fs.Dispose()

                fi = New FileInfo(Application.ExecutablePath(0) & ":\CD.ID")
                If Not fi.Exists Then
                    Try
                        WriteCDID()
                    Catch ex As UnauthorizedAccessException
                        UAC(UACActions.InstallCDID)
                    Catch ex As Exception
                        MsgBox("Could not set ""Skip CD check"" setting: " & _
                            ex.Message, MsgBoxStyle.Exclamation, "Error - Knight")
                        success = False
                    End Try
                End If
            ElseIf cbNoCD.CheckState = CheckState.Unchecked Then
                Dim fi As New FileInfo(Path.Combine(_df.GamePath, "CD.ID"))
                If fi.Exists Then
                    fi.Delete()
                End If

                Dim cddrive As Char = "D"c
                If rbNative.Checked OrElse Not DarkForces.IsValidDOSBoxPath(fbdDOSBox. _
                    SelectedPath) Then

                    Dim c As Char = DarkForces.FindFirstCDDrive
                    If c <> Chr(0) Then
                        cddrive = c
                    End If
                End If

                Dim fs As New FileStream(Path.Combine(_df.GamePath, "DRIVE.CD"), _
                    FileMode.Create, FileAccess.Write)
                fs.Write(Encoding.ASCII.GetBytes(New Char() {cddrive}), 0, 1)
                fs.Close()
                fs.Dispose()

                Try
                    DeleteCDID()
                Catch ex As UnauthorizedAccessException
                    UAC(UACActions.RemoveCDID)
                Catch ex As Exception
                    MsgBox("Could not unset ""Skip CD check"" setting: " & ex.Message, _
                        MsgBoxStyle.Exclamation, "Error - Knight")
                    success = False
                End Try
            End If
            If cbForceCD.Checked Then
                MyOptions.DarkForcesCDDrive = dudCD.Text
            Else
                MyOptions.DarkForcesCDDrive = Chr(0)
            End If

            MyOptions.EndUpdate()

            If success Then
                bOK.Enabled = False
                bApply.Enabled = False
                bCancel.Text = "Close"
            End If
        End Sub

        Private Sub Revert()
            ec.Block()

            rbNative.Checked = MyOptions.DarkForcesRunMode = Options. _
                DarkForcesRunModes.Native
            rbDOSBox.Checked = MyOptions.DarkForcesRunMode = Options. _
                DarkForcesRunModes.DOSBox
            fbdDOSBox.SelectedPath = MyOptions.DarkForcesDOSBoxPath
            rbDarkXL.Checked = MyOptions.DarkForcesRunMode = Options. _
                DarkForcesRunModes.DarkXL
            fbdDarkXL.SelectedPath = MyOptions.DarkForcesDarkXLPath

            fbdMods.SelectedPath = _df.GetModPath

            cbLog.Checked = MyOptions.DarkForcesLog
            cbSkipMemory.Checked = MyOptions.DarkForcesSkipMemoryCheck
            cbSkipFiles.Checked = MyOptions.DarkForcesSkipFILESCheck
            cbAutoTest.Checked = MyOptions.DarkForcesAutoTest

            cbScreenshots.Checked = MyOptions.DarkForcesEnableScreenshots
            cbLevelSelect.Checked = MyOptions.DarkForcesLevelSelect IsNot Nothing
            If cbLevelSelect.Checked Then
                cbLevel.Text = MyOptions.DarkForcesLevelSelect
            Else
                cbLevel.Text = "SECBASE"
            End If
            cbSkipCutscenes.Checked = MyOptions.DarkForcesSkipCutscenes

            Dim cdid As Byte = 0
            If File.Exists(Path.Combine(_df.GamePath, "CD.ID")) Then
                cdid += 1
            End If
            If File.Exists(Application.ExecutablePath(0) & ":\CD.ID") Then
                cdid += 1
            End If
            cbNoCD.CheckState = cdid Xor 3

            cbForceCD.Checked = MyOptions.DarkForcesCDDrive <> Chr(0)
            If cbForceCD.Checked Then
                dudCD.Text = MyOptions.DarkForcesCDDrive
            Else
                dudCD.Text = "D"
            End If

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"

            ec.Allow()
        End Sub
#End Region

#Region "Event Handlers"
        Private Sub bApply_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bApply.Click

            Apply()
        End Sub

        Private Sub bBrowseDarkXL_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles bBrowseDarkXL.Click

            If fbdDarkXL.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If DarkForces.IsValidDarkXLPath(fbdDarkXL.SelectedPath) Then
                    ItemChanged()
                Else
                    MsgBox("DarkXL was not detected in that folder.  This change " & _
                        "will not be saved.", MsgBoxStyle.Exclamation, "Error - Knight")

                    EnsureValidRadioSelection()
                End If
            End If
        End Sub

        Private Sub bBrowseDOSBox_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles bBrowseDOSBox.Click

            If fbdDOSBox.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If DarkForces.IsValidDOSBoxPath(fbdDOSBox.SelectedPath) Then
                    ItemChanged()
                Else
                    MsgBox("DOSBox was not detected in that folder.  This change " & _
                        "will not be saved.", MsgBoxStyle.Exclamation, "Error - Knight")

                    EnsureValidRadioSelection()
                End If
            End If
        End Sub

        Private Sub bBrowseMods_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles bBrowseMods.Click

            If fbdMods.ShowDialog() = Windows.Forms.DialogResult.OK Then
                ItemChanged()
            End If
        End Sub

        Private Sub bIMuse_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bIMuse.Click

            If bApply.Enabled Then
                Apply()
            End If

            _df.RunIMuse()
        End Sub

        Private Sub bOK_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bOK.Click

            Apply()
        End Sub

        Private Sub bSetup_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bSetup.Click

            If bApply.Enabled Then
                Apply()
            End If

            _df.RunSetup()
        End Sub

        Private Sub cbForceCD_CheckedChanged(ByVal sender As System.Object, ByVal _
            e As System.EventArgs) Handles cbForceCD.CheckedChanged

            dudCD.Enabled = DirectCast(sender, CheckBox).Checked
        End Sub

        Private Sub cbLevelSelect_CheckedChanged(ByVal sender As System.Object, ByVal _
            e As System.EventArgs) Handles cbLevelSelect.CheckedChanged

            cbLevel.Enabled = DirectCast(sender, CheckBox).Checked
        End Sub

        Private Sub Changed(ByVal sender As Object, ByVal e As EventArgs) Handles _
            rbNative.CheckedChanged, rbDOSBox.CheckedChanged, rbDarkXL.CheckedChanged, _
            cbLog.CheckedChanged, cbSkipMemory.CheckedChanged, cbSkipFiles. _
            CheckedChanged, cbAutoTest.CheckedChanged, cbScreenshots.CheckedChanged, _
            cbLevelSelect.CheckedChanged, cbLevel.TextChanged, cbSkipCutscenes. _
            CheckedChanged, cbNoCD.CheckedChanged, cbForceCD.CheckedChanged, dudCD. _
            TextChanged

            ItemChanged()
        End Sub

        Private Sub rbDarkXL_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles rbDarkXL.Click

            If fbdDarkXL.SelectedPath Is Nothing Then
                bBrowseDarkXL.PerformClick()
            End If
        End Sub

        Private Sub rbDOSBox_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles rbDOSBox.Click

            If fbdDOSBox.SelectedPath Is Nothing Then
                bBrowseDOSBox.PerformClick()
            End If
        End Sub

        Public Overloads Function ShowDialog() As DialogResult
            Revert()
            Return MyBase.ShowDialog()
        End Function
#End Region
    End Class
End Namespace

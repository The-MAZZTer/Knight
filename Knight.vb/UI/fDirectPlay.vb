Imports Microsoft.Win32

Namespace MZZT.Knight.Games
    Public Class fDirectPlay
        Public Sub New(ByVal g As Game)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Icon = WinAPI.BitmapToIcon(My.Resources.silk_group)
            _g = g
        End Sub
        Private _g As Game

        Public Shared Sub SetDirectPlayCommandLine(ByVal game As String, ByVal _
            commandline As String)

            Dim rk As RegistryKey = Registry.LocalMachine.OpenSubKey( _
                "SOFTWARE\Microsoft\DirectPlay\Applications\" & game & " 1.0", True)
            rk.SetValue("CommandLine", commandline)
            rk.Close()
        End Sub

        Private Sub ItemChanged()
            If ec.EventsAllowed Then
                bCancel.Text = "Cancel"
            End If
        End Sub

#Region "Load/Save Settings"
        Private Sub Apply(ByVal buildArgs As Boolean)
            Dim args As String = ""
            If buildArgs Then
                If TypeOf _g Is MysteriesOfTheSith Then
                    args = DirectCast(_g, MysteriesOfTheSith).BuildArguments( _
                        cbMods.Checked)
                Else
                    args = DirectCast(_g, JediKnight).BuildArguments(cbMods.Checked)
                End If
            End If

            Dim rk As RegistryKey = Nothing
            Dim success As Boolean = True
            Try
                SetDirectPlayCommandLine(_g.Name, args)
            Catch ex As Security.SecurityException
                UAC(UACActions.SetDirectPlayCommandLine, _g.Name, args)
            Catch ex As Exception
                MsgBox("Could not set DirectPlay command line setting: " & ex.Message, _
                    MsgBoxStyle.Exclamation, "Error - Knight")
                success = False
            End Try

            If rk IsNot Nothing Then
                rk.SetValue("CommandLine", args)
                rk.Close()
            End If

            If success Then
                bCancel.Text = "Close"
            End If
        End Sub

        Private Sub Revert()
            ec.Block()

            Dim rk As RegistryKey = Registry.LocalMachine.OpenSubKey( _
                "SOFTWARE\Microsoft\DirectPlay\Applications\" & _g.Name & " 1.0", False)
            If rk Is Nothing Then
                cbMods.Checked = True
            Else
                Dim cl As String = rk.GetValue("CommandLine")
                cbMods.Checked = cl Is Nothing OrElse cl.Trim = "" OrElse cl.ToLower. _
                    Contains("-path")
                rk.Close()
            End If

            bCancel.Text = "Close"

            ec.Allow()
        End Sub
#End Region

#Region "Event Handlers"
        Private Sub bApply_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bApply.Click

            Apply(True)
        End Sub

        Private Sub bReset_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bReset.Click

            Apply(False)
        End Sub

        Private Sub Changed(ByVal sender As Object, ByVal e As EventArgs) Handles _
            cbMods.CheckedChanged

            ItemChanged()
        End Sub

        Public Overloads Function ShowDialog() As DialogResult
            Revert()
            Return MyBase.ShowDialog()
        End Function
#End Region

    End Class
End Namespace

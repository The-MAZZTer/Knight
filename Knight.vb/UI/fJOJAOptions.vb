Namespace MZZT.Knight.Games
    Public Class fJOJAOptions
        Public Sub New(ByVal g As Game)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _g = g
            Text = g.Name & " Options - Knight"
            Icon = WinAPI.BitmapToIcon(g.Image32)
            ec.Allow()
        End Sub
        Private _g As Game

        Private Sub ItemChanged()
            If ec.EventsAllowed Then
                bOK.Enabled = True
                bApply.Enabled = True
                bCancel.Text = "Cancel"
            End If
        End Sub

#Region "Load/Save Settings"
        Private Sub Apply()
            Dim lines As String = ""
            For Each line As String In tbCommands.Lines
                If line.Trim = "" Then
                    Continue For
                End If

                If lines.Length > 0 Then
                    lines &= " +"
                End If
                lines &= line.Trim
            Next line

            If lines = "" Then
                lines = Nothing
            End If

            If TypeOf _g Is JediOutcast Then
                MyOptions.JediOutcastCommands = lines
            ElseIf TypeOf _g Is JediAcademy Then
                MyOptions.JediAcademyCommands = lines
            End If

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"
        End Sub

        Private Sub Revert()
            ec.Block()

            Dim lines As String = Nothing
            If TypeOf _g Is JediOutcast Then
                lines = MyOptions.JediOutcastCommands
            ElseIf TypeOf _g Is JediAcademy Then
                lines = MyOptions.JediAcademyCommands
            End If

            If lines Is Nothing Then
                tbCommands.Text = ""
            Else
                Dim splitlines As New List(Of String)(lines.Split("+"))
                Dim i As Integer = 0
                While i < splitlines.Count
                    splitlines(i) = splitlines(i).Trim

                    If splitlines(i) = "" Then
                        splitlines.RemoveAt(i)
                    Else
                        i += 1
                    End If
                End While
                tbCommands.Lines = splitlines.ToArray
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

        Private Sub bOK_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bOK.Click

            Apply()
        End Sub

        Private Sub Changed(ByVal sender As Object, ByVal e As EventArgs) Handles _
            tbCommands.TextChanged

            ItemChanged()
        End Sub

        Public Overloads Function ShowDialog() As DialogResult
            Revert()
            Return MyBase.ShowDialog()
        End Function
#End Region
    End Class
End Namespace
Namespace MZZT.Knight.Games
    Public Class fJOJAModOptions
        Public Sub New(ByVal g As Game, ByVal m As Game.Modification)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _g = g
            _m = m
            Icon = WinAPI.BitmapToIcon(g.Image32)
            Text = m.Name & " Options - Knight"
            ec.Allow()
        End Sub
        Private _g As Game
        Private _m As Game.Modification

        Private Sub ItemChanged()
            If ec.EventsAllowed Then
                bOK.Enabled = True
                bApply.Enabled = True
                bCancel.Text = "Cancel"
            End If
        End Sub

#Region "Load/Save Settings"
        Private Sub Apply()
            MyModInfo.BeginUpdate()

            Dim m As ModInfoCache.Modification = MyModInfo(_g.Name, _m.UniqueID)
            If m Is Nothing OrElse Not TypeOf m Is Quake3ModInfo Then
                m = New Quake3ModInfo(_m.Name)
                m.UniqueID = _m.UniqueID
            End If
            Dim q As Quake3ModInfo = m

            q.SinglePlayer = cbSinglePlayer.Checked

            MyModInfo(_g.Name, _m.UniqueID) = m
            MyModInfo.EndUpdate()

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"
        End Sub

        Private Sub Revert()
            ec.Block()

            Dim m As ModInfoCache.Modification = MyModInfo(_g.Name, _m.UniqueID)
            If m Is Nothing OrElse Not TypeOf m Is Quake3ModInfo Then
                cbSinglePlayer.Checked = False
            Else
                Dim q As Quake3ModInfo = m
                cbSinglePlayer.Checked = q.SinglePlayer
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
            cbSinglePlayer.CheckedChanged

            ItemChanged()
        End Sub

        Public Overloads Function ShowDialog() As DialogResult
            Revert()
            Return MyBase.ShowDialog()
        End Function
#End Region
    End Class
End Namespace

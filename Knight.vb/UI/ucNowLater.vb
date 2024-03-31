Namespace MZZT
    Public Class ucNowLater
        Private Sub bNow_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bNow.Click

            RaiseEvent NowClicked(Me, e)
        End Sub

        Private Sub bLater_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bLater.Click

            RaiseEvent LaterClicked(Me, e)
        End Sub

        Public Event NowClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event LaterClicked(ByVal sender As Object, ByVal e As EventArgs)
    End Class
End Namespace
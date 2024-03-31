Namespace MZZT
    Public Class ucUpload
        Public Property Maximum() As Integer
            Get
                Return pbProgress.Maximum
            End Get
            Set(ByVal value As Integer)
                pbProgress.Maximum = value
            End Set
        End Property

        Public Property Value() As Integer
            Get
                Return pbProgress.Value
            End Get
            Set(ByVal value As Integer)
                pbProgress.Value = value
            End Set
        End Property

        Private Sub bCancel_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bCancel.Click

            RaiseEvent CancelClicked(Me, e)
        End Sub

        Public Event CancelClicked(ByVal sender As Object, ByVal e As EventArgs)
    End Class
End Namespace
Imports System.ComponentModel

Namespace MZZT.Knight
    Public Class fProgress
        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            DialogResult = Windows.Forms.DialogResult.Cancel
            Icon = My.Resources.hourglass
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()

            If _backgroundWorker IsNot Nothing Then
                RemoveHandler _backgroundWorker.ProgressChanged, AddressOf _
                    _backgroundWorker_ProgressChanged
                RemoveHandler _backgroundWorker.RunWorkerCompleted, AddressOf _
                    _backgroundWorker_RunWorkerCompleted
            End If
        End Sub

#Region "Event Handlers"
        Private Sub bCancel_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bCancel.Click

            Cancel()

            MyBase.Close()
        End Sub

        Private Sub Cancel()
            DialogResult = Windows.Forms.DialogResult.Cancel

            If _backgroundWorker IsNot Nothing AndAlso _backgroundWorker.IsBusy Then
                bCancel.Enabled = False
                _backgroundWorker.CancelAsync()
            End If
        End Sub

        Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms. _
            FormClosingEventArgs)

            MyBase.OnFormClosing(e)

            If e.CloseReason = CloseReason.UserClosing Then
                Cancel()
            End If
        End Sub

        Private Sub _backgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e _
            As System.ComponentModel.ProgressChangedEventArgs)

            If e.UserState IsNot Nothing Then
                lDesc.Text = e.UserState
            End If
            pbProgress.Value = e.ProgressPercentage
        End Sub

        Private Sub _backgroundWorker_RunWorkerCompleted(ByVal sender As Object, _
            ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)

            DialogResult = Windows.Forms.DialogResult.OK
            Close()
        End Sub
#End Region

#Region "Properties"
        Public Property BackgroundWorker() As BackgroundWorker
            Get
                Return _backgroundWorker
            End Get
            Set(ByVal value As BackgroundWorker)
                If _backgroundWorker IsNot Nothing Then
                    RemoveHandler _backgroundWorker.ProgressChanged, AddressOf _
                        _backgroundWorker_ProgressChanged
                    RemoveHandler _backgroundWorker.RunWorkerCompleted, AddressOf _
                        _backgroundWorker_RunWorkerCompleted
                End If

                _backgroundWorker = value

                If _backgroundWorker IsNot Nothing Then
                    AddHandler _backgroundWorker.ProgressChanged, AddressOf _
                        _backgroundWorker_ProgressChanged
                    AddHandler _backgroundWorker.RunWorkerCompleted, AddressOf _
                        _backgroundWorker_RunWorkerCompleted
                End If
            End Set
        End Property
        Private _backgroundWorker As BackgroundWorker = Nothing

        Public Property Maximum() As Integer
            Get
                Return pbProgress.Maximum
            End Get
            Set(ByVal value As Integer)
                pbProgress.Maximum = value
            End Set
        End Property
#End Region
    End Class
End Namespace

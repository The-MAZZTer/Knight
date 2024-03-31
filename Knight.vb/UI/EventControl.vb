Imports System.ComponentModel

Namespace MZZT
    <ToolboxItem(True)> _
    Public Class EventControl
        Inherits Component

        Private counter As Integer = 1

        Public Sub Block()
            counter += 1
        End Sub

        Public Sub Allow()
            counter -= 1

            If counter < 0 Then
                counter = 0

                Throw New Exception("Called EventControl.Allow() more times than EventControl.Block()!")
            End If
        End Sub

        Public Function EventsAllowed() As Boolean
            Return (counter = 0)
        End Function
    End Class
End Namespace
Imports System.IO
Imports Microsoft.Win32

Namespace MZZT.Knight.Games
    Public Class SteamHelper
        Private Sub New()
        End Sub

        Public Enum AppIDs As Integer
            JediAcademy = 6020
            JediOutcast = 6030
            JediKnight = 32380
            MysteriesOfTheSith = 32390
            DarkForces = 32400
        End Enum

        Public Shared Function IsAppInstalled(ByVal appid As AppIDs) As Boolean
            Dim rk As RegistryKey = Registry.CurrentUser.OpenSubKey( _
                "Software\Valve\Steam\Apps\" & CInt(appid).ToString, False)
            If rk Is Nothing Then
                Return False
            End If

            Dim installed As Integer = rk.GetValue("Installed", 0)
            rk.Close()

            Return installed > 0
        End Function

        Public Shared Function GetSteamPath() As String
            Dim rk As RegistryKey = Registry.CurrentUser.OpenSubKey( _
                "Software\Valve\Steam")
            Dim steampath As String = rk.GetValue("SteamPath", Nothing)
            rk.Close()
            If steampath IsNot Nothing Then
                Return steampath.Replace("/"c, "\"c)
            End If
            Return Nothing
        End Function
    End Class
End Namespace

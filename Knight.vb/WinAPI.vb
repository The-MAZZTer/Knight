Imports System.ComponentModel
Imports System.Text

Namespace MZZT
	Public Class WinAPI
		Private Sub New()
		End Sub

		Public Shared Function BitmapToIcon(ByVal b As Bitmap) As Icon
			Dim ip As IntPtr = b.GetHicon
			Dim i As Icon = Icon.FromHandle(ip)
			i = i.Clone
			DestroyIcon(ip)
			Return i
		End Function
		Private Declare Function DestroyIcon Lib "user32.dll" (ByVal handle As IntPtr) _
						As Boolean

		Public Shared Function GetShortPathName(ByVal path As String) As String
			Dim len As Integer = GetShortPathName(path, Nothing, 0)
			Dim spath As New StringBuilder(len)
			If GetShortPathName(path, spath, len) = 0 Then
				Throw New Win32Exception
			End If
			Return spath.ToString
		End Function
		Private Declare Auto Function GetShortPathName Lib "kernel32" (ByVal _
						lpszLongPath As String, ByVal lpszShortPath As StringBuilder, ByVal _
						cchBuffer As Integer) As Integer

		Public Shared Function ListView_SetIconSpacing(ByVal lv As ListView, ByVal s _
						As Size) As Size

			Dim i As Integer = SendMessage(lv.Handle, LVM.SETICONSPACING, 0,
								(s.Height * 65536) + (s.Width And 65535))

			Return New Size(i And 65535, i / 65536)
		End Function
		Private Declare Auto Function SendMessage Lib "user32" (ByVal hWnd As IntPtr,
						ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) _
						As Integer

		Public Enum WM As Integer
			WINDOWPOSCHANGED = &H47
		End Enum

		Private Enum LVM As Integer
			FIRST = &H1000
			SETICONSPACING = FIRST + 53
		End Enum

		Public Declare Function SetForegroundWindow Lib "user32.dll" (ByVal hWnd As _
						IntPtr) As Boolean

		Public Declare Auto Sub SetWindowText Lib "user32" (ByVal hWnd As IntPtr,
				ByVal lpString As String)
	End Class
End Namespace

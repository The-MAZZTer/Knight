Imports System.ComponentModel
Imports System.IO
Imports System.Threading

Namespace MZZT.Knight
	Public Module Program
		Public Const AnimationTime As Integer = 250

		Public Sub Main()
			AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf _
					AppDomain_UnhandledException

			Application.EnableVisualStyles()

			If WasRunWithSwitch("elevate") Then
				InnerUAC()
				Return
			End If

			If WasRunWithSwitch("masterini") Then
				ParseMasterIni()
				Return
			End If

			If Not IsSingleInstance() Then
				Return
			End If

			MyOptions = Options.Load

			MyUpdate = New Updater
			MyUpdate.UpdateProcessName = UpdateFilename
			MyUpdate.UpdateURL = New Uri(UpdateURL)
			MyUpdate.DoUpdate()

			MyGames = New Games.Game() {New Games.DarkForces, New Games.JediKnight,
					New Games.MysteriesOfTheSith, New Games.JediOutcast, New Games.
					JediAcademy}

			If MyOptions.FirstRun Then
				MyOptions.BeginUpdate()
				For i As Integer = 0 To MyGames.Length - 1
					MyGames(i).GamePath = MyGames(i).AutoGamePath

					If MyGames(i).GamePath IsNot Nothing Then
						MyGames(i).Enabled = True

						If MyOptions.ExpandedGame = -1 Then
							MyOptions.ExpandedGame = i
						End If
					End If
				Next i

				MyOptions.FirstRun = False
				MyOptions.EndUpdate()
			End If

			MyModInfo = Games.ModInfoCache.Load

			MyHashes = HashList.Load
			MyHashes.Save() ' Ensures up-to-date list written to disk

			MyForm = New fKnight

			AddHandler MyUpdate.AutoCheckFired, AddressOf MyUpdate_AutoCheckFired
			MyUpdate.StartAutoCheckTimer()

			Application.Run(MyForm)
		End Sub

		Private Function IsSingleInstance() As Boolean
			Dim p() As Process = Process.GetProcessesByName(IO.Path.
					GetFileNameWithoutExtension(Application.ExecutablePath))
			If p.Length > 1 Then
				MsgBox("Knight is already running!", MsgBoxStyle.Exclamation, "Knight")

				Dim hwnd As IntPtr
				If p(0).Id = Process.GetCurrentProcess.Id Then
					hwnd = p(1).MainWindowHandle
				Else
					hwnd = p(0).MainWindowHandle
				End If
				If Not hwnd.Equals(IntPtr.Zero) Then
					WinAPI.SetForegroundWindow(hwnd)
				End If
				Return False
			End If

			Return True
		End Function

#Region "Command Line Actions"
		Public Enum UACActions As Byte
			InstallCDID
			RemoveCDID
			SetDirectPlayCommandLine
		End Enum

		Private Sub InnerUAC()
			If WasRunWithSwitch(UACActions.InstallCDID.ToString) Then
				Try
					Games.fDarkForcesOptions.WriteCDID()
				Catch ex As Exception
					MsgBox("Could not set ""Skip CD check"" setting: " & ex.Message,
							MsgBoxStyle.Exclamation, "Error - Knight")
				End Try
			ElseIf WasRunWithSwitch(UACActions.RemoveCDID.ToString) Then
				Try
					Games.fDarkForcesOptions.DeleteCDID()
				Catch ex As Exception
					MsgBox("Could not unset ""Skip CD check"" setting: " & ex.Message,
							MsgBoxStyle.Exclamation, "Error - Knight")
				End Try
			ElseIf WasRunWithSwitch(UACActions.SetDirectPlayCommandLine.ToString) Then
				Dim argv As String() = Environment.GetCommandLineArgs
				Try
					Games.fDirectPlay.SetDirectPlayCommandLine(argv(3), argv(4))
				Catch ex As Exception
					MsgBox("Could not set DirectPlay command line setting: " &
							ex.Message, MsgBoxStyle.Exclamation, "Error - Knight")
				End Try
			End If
		End Sub

		Private Sub ParseMasterIni()
			Games.ModInfoCache.XMLPath = "..\..\Resources\modinfocache_defaults.xml"
			Games.ModInfoCache.PortableAppsXMLPath = Games.ModInfoCache.XMLPath

			MyModInfo = New Games.ModInfoCache()
			MyModInfo.BeginUpdate()

			Dim fs As New FileStream("master.ini", FileMode.Open, FileAccess.Read)
			Dim sr As New StreamReader(fs)

			Dim currentsection As String = Nothing
			Dim currentmod As Games.DarkForcesModInfo = Nothing

			Dim line As String
			While Not sr.EndOfStream
				line = sr.ReadLine()
				If line Is Nothing Then
					Continue While
				End If

				line = line.Trim
				Dim comment As Integer = line.IndexOf(";"c)
				If comment > -1 Then
					line = line.Substring(0, comment).TrimEnd
				End If

				If line = "" Then
					Continue While
				End If

				If line.StartsWith("[") Then
					If currentsection IsNot Nothing Then
						MyModInfo("Dark Forces", currentsection) = currentmod
					End If

					currentsection = line.Substring(1)
					Dim endsection As Integer = currentsection.IndexOf("]"c)
					If endsection > -1 Then
						currentsection = currentsection.Substring(0, endsection)
					End If

					currentmod = New Games.DarkForcesModInfo
					Continue While
				End If

				Dim equals As Integer = line.IndexOf("="c)
				If equals < 0 Then
					Continue While
				End If

				Dim name As String = line.Substring(0, equals).Trim.ToLower
				Dim value As String = line.Substring(equals + 1).Trim
				Select Case name
					Case "gamename"
						currentmod.Name = value
					Case "lfdname"
						If value <> "" Then
							currentmod.LFD = value & ".LFD"
						End If
					Case "crawlname"
						If value <> "" Then
							currentmod.Crawl = value & ".LFD"
						End If
					Case Else
						If Not name.ToLower.StartsWith("other") Then
							Continue While
						End If

						currentmod.Others.Add(value)
				End Select
			End While

			If currentsection IsNot Nothing Then
				MyModInfo("Dark Forces", currentsection) = currentmod
			End If

			sr.Close()
			fs.Close()
			sr.Dispose()
			fs.Dispose()
			MyModInfo.EndUpdate()
		End Sub

		Public Sub UAC(ByVal action As UACActions, ByVal ParamArray arguments As _
				String())

			Dim psi As New ProcessStartInfo(Application.ExecutablePath, "-elevate -" &
					action.ToString)

			For Each s As String In arguments
				psi.Arguments &= " """ & s & """"
			Next s

			psi.Verb = "runas"
			Process.Start(psi)
		End Sub

		Private Function WasRunWithSwitch(ByVal switch As String) As Boolean
			Dim argv As String() = Environment.GetCommandLineArgs
			For i As Integer = 1 To argv.Length - 1
				Dim s As String = argv(i)
				If Not s.StartsWith("/") AndAlso Not s.StartsWith("-") Then
					Continue For
				End If

				If s.StartsWith("--") Then
					s = s.Substring(2)
				Else
					s = s.Substring(1)
				End If

				If s.ToLower = switch.ToLower Then
					Return True
				End If
			Next i

			Return False
		End Function
#End Region

#Region "Updates"
		Private Const UpdateFilename As String = "knight_update"
		Private Const UpdateURL As String = "http://www.mzzt.net/update/knight.xml"

		Private Sub MyUpdate_AutoCheckFired(ByVal sender As Object, ByVal e As _
				CancelEventArgs)

			If Form.ActiveForm IsNot Nothing Then
				e.Cancel = True
				Return
			End If

			For Each f As Form In Application.OpenForms
				If f.Visible AndAlso Not TypeOf f Is fKnight AndAlso Not TypeOf f Is
						Notification Then

					e.Cancel = True
					Return
				End If
			Next f

			If Not e.Cancel Then
				RemoveHandler MyUpdate.AutoCheckFired, AddressOf MyUpdate_AutoCheckFired
			End If
		End Sub
#End Region

		Private Sub AppDomain_UnhandledException(ByVal sender As System.Object, ByVal _
				e As System.UnhandledExceptionEventArgs)

			If Process.GetCurrentProcess.MainModule.FileName.ToLower.EndsWith(
					".vshost.exe") Then

				Return
			End If

			If Not TypeOf e.ExceptionObject Is Exception Then
				Return
			End If

			Dim ex As Exception = e.ExceptionObject

			Dim message As String = "An unexpected condition has been reached in " &
					"this program and it is no longer able to continue running.  " &
					"Please report the following information to the author so that he " &
					"may fix the problem." & Environment.NewLine & Environment.NewLine &
					ex.GetType.FullName & ": " & ex.Message & Environment.NewLine & ex.
					StackTrace
			MsgBox(message, MsgBoxStyle.Critical, "Fatal Error - Knight")
			End
		End Sub

		Public MyForm As fKnight = Nothing
		Public MyGames() As Games.Game = Nothing
		Public MyModInfo As Games.ModInfoCache = Nothing
		Public MyOptions As Options = Nothing
		Public MyHashes As HashList = Nothing
		Public MyUpdate As Updater = Nothing
	End Module
End Namespace
Imports ICSharpCode.SharpZipLib.Zip
Imports Microsoft.Win32
Imports System.ComponentModel
Imports System.IO

Namespace MZZT.Knight.Games
	Public MustInherit Class Game
		Public MustInherit Class Modification
			Public Sub New(ByVal owner As Game)
				_owner = owner
			End Sub
			Protected _owner As Game

			Public MustOverride Function RefreshInfo() As ModInfoCache.Modification

			Public Function GetCachedInfo() As ModInfoCache.Modification
				Return MyModInfo(_owner.Name, UniqueID)
			End Function

			Public Overridable Sub ShowOptions()
			End Sub

#Region "Properties"
			Public MustOverride Property Active() As Boolean

			Public ReadOnly Property Name() As String
				Get
					Dim m As ModInfoCache.Modification = MyModInfo(_owner.Name,
												UniqueID)
					If m IsNot Nothing Then
						Return m.Name
					End If

					Return UniqueID
				End Get
			End Property

			Public Overridable ReadOnly Property HasOptions() As Boolean
				Get
					Return False
				End Get
			End Property

			Public Overridable ReadOnly Property SortOrder() As Integer
				Get
					Return 0
				End Get
			End Property

			Public MustOverride ReadOnly Property UniqueID() As String
#End Region
		End Class

		Public MustInherit Class Patch
			Public Sub New(ByVal owner As Game)
				_owner = owner
			End Sub
			Protected _owner As Game

#Region "Properties"
			Public MustOverride Property Active() As Boolean
			Public MustOverride ReadOnly Property FileName() As String
			Public MustOverride ReadOnly Property Name() As String

			Public Overridable ReadOnly Property SortOrder() As Integer
				Get
					Return 0
				End Get
			End Property

			Public MustOverride ReadOnly Property UniqueID() As String
#End Region
		End Class

		Public Sub AbortRefreshMods()
			If Not IsRefreshingMods() Then
				Return
			End If

			SyncLock _refreshMods
				_refreshMods.Abort()
			End SyncLock
			_refreshMods.Join()
			_refreshMods = Nothing

			RaiseEvent AbortModSearch(Me, New EventArgs)
		End Sub

		Public Sub AbortRefreshPatches()
			If Not IsRefreshingPatches() Then
				Return
			End If

			SyncLock _refreshPatches
				_refreshPatches.Abort()
			End SyncLock
			_refreshPatches.Join()
			_refreshPatches = Nothing

			RaiseEvent AbortPatchSearch(Me, New EventArgs)
		End Sub

		Protected Function AlreadyFoundMod(ByVal uid As String) As Boolean
			For Each m As Modification In _mods
				If m.UniqueID.ToLower = uid.ToLower Then
					Return True
				End If
			Next m

			Return False
		End Function

		Public Function AlreadyFoundPatch(ByVal uid As String) As Boolean
			For Each p As Patch In _patches
				If p.UniqueID.ToLower = uid.ToLower Then
					Return True
				End If
			Next p

			Return False
		End Function

		Public Function GetActiveModPath() As String
			Dim amp As String = ActiveModPath
			If amp Is Nothing Then
				Return AutoActiveModPath
			End If
			Return amp
		End Function

		Public Function GetModPath() As String
			Dim mp As String = ModPath
			If mp Is Nothing Then
				Return AutoModPath
			End If
			Return mp
		End Function

		Public Function GetPatchPath() As String
			Dim pp As String = PatchPath
			If pp Is Nothing Then
				Return AutoPatchPath
			End If
			Return pp
		End Function

		Public Overridable Function InstallPatch(ByVal filename As String, ByVal h _
						As HashList.Hash) As Patch

			Return Nothing
		End Function

		Public MustOverride Function IsValidGamePath(ByVal folder As String) As Boolean

		Public Sub RefreshMods()
			AbortRefreshMods()

			_mods.Clear()

			RaiseEvent BeginModSearch(Me, New EventArgs)

			_refreshMods = New Threading.Thread(AddressOf RefreshModsInternal)
			_refreshMods.IsBackground = True
			_refreshMods.Priority = Threading.ThreadPriority.BelowNormal
			_refreshMods.Start()
		End Sub
		Private _refreshMods As Threading.Thread = Nothing
		Protected MustOverride Sub RefreshModsInternal()

		Public Sub RefreshPatches()
			AbortRefreshPatches()

			_patches.Clear()

			RaiseEvent BeginPatchSearch(Me, New EventArgs)

			_refreshPatches = New Threading.Thread(AddressOf RefreshPatchesInternal)
			_refreshPatches.IsBackground = True
			_refreshPatches.Priority = Threading.ThreadPriority.BelowNormal
			_refreshPatches.Start()
		End Sub
		Private _refreshPatches As Threading.Thread = Nothing
		Protected Overridable Sub RefreshPatchesInternal()
			RaiseEvent EndPatchSearch(Me, New EventArgs)
		End Sub

		Protected Sub RaiseModFoundEvent(ByVal m As Modification)
			RaiseEvent ModFound(Me, New ModFoundEventArgs(m))
		End Sub

		Protected Sub RaiseEndModSearchEvent()
			RaiseEvent EndModSearch(Me, New EventArgs)
		End Sub

		Protected Sub RaisePatchFoundEvent(ByVal p As Patch)
			RaiseEvent PatchFound(Me, New PatchFoundEventArgs(p))
		End Sub

		Public MustOverride Sub Run()
		Public MustOverride Sub Run(ByVal mods As Modification())

		Public Overridable Sub RunMultiplayer()
		End Sub

		Public Overridable Sub RunMultiplayer(ByVal mods As Modification())
		End Sub

		Public Overridable Function ShoundRunMultiplayer(ByVal mods As Modification()) _
						As Boolean

			Return False
		End Function

		Public MustOverride Sub ShowOptions()

#Region "Properties"
		Public Overridable Property ActiveModPath() As String
			Get
				Return MyOptions(Me.GetType.Name & "ActiveModPath")
			End Get
			Set(ByVal value As String)
				Dim mp As String = GetActiveModPath()
				If value = "" Then
					value = Nothing
				End If
				If value Is Nothing Then
					value = AutoActiveModPath
				End If

				Dim valuedi As New DirectoryInfo(value)
				Dim currentdi As New DirectoryInfo(mp)
				Dim autodi As New DirectoryInfo(AutoActiveModPath)
				If valuedi.FullName.ToLower = currentdi.FullName.ToLower Then
					If valuedi.FullName.ToLower = autodi.FullName.ToLower Then
						MyOptions(Me.GetType.Name & "ActiveModPath") = Nothing
					End If

					Return
				End If

				If valuedi.FullName.ToLower = autodi.FullName.ToLower Then
					MyOptions(Me.GetType.Name & "ActiveModPath") = Nothing
				Else
					MyOptions(Me.GetType.Name & "ActiveModPath") = value
				End If

				RaiseEvent ActiveModPathChanged(Me, New EventArgs)
			End Set
		End Property

		Public MustOverride ReadOnly Property AllowMultipleActiveMods() As Boolean
		Public MustOverride ReadOnly Property AutoActiveModPath() As String
		Public MustOverride ReadOnly Property AutoGamePath() As String
		Public MustOverride ReadOnly Property AutoModPath() As String

		Public Overridable ReadOnly Property AutoPatchPath() As String
			Get
				Return Nothing
			End Get
		End Property

		Public Property Enabled() As Boolean
			Get
				If GamePath Is Nothing OrElse Not IsValidGamePath(GamePath) Then
					MyOptions(Me.GetType.Name & "Enabled") = False
					Return False
				End If

				Return MyOptions(Me.GetType.Name & "Enabled")
			End Get
			Set(ByVal value As Boolean)
				If GamePath Is Nothing OrElse Not IsValidGamePath(GamePath) AndAlso value Then
					Throw New InvalidOperationException(
												"Can't enable a game when no path has been set!")
				End If

				MyOptions(Me.GetType.Name & "Enabled") = value

				RaiseEvent EnabledChanged(Me, New EventArgs)
			End Set
		End Property

		Public Overridable Property GamePath() As String
			Get
				Dim path As String = MyOptions(Me.GetType.Name & "Path")

				If path Is Nothing OrElse Not IsValidGamePath(path) Then
					Return Nothing
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If value IsNot Nothing AndAlso Not IsValidGamePath(value) Then
					Throw New DirectoryNotFoundException("Invalid game path.")
				End If

				MyOptions(Me.GetType.Name & "Path") = value

				RaiseEvent GamePathChanged(Me, New EventArgs)
			End Set
		End Property

		Public MustOverride ReadOnly Property HasSeparateMultiplayerBinary() As Boolean

		Public ReadOnly Property IsRefreshingMods()
			Get
				Return _refreshMods IsNot Nothing AndAlso _refreshMods.IsAlive
			End Get
		End Property

		Public ReadOnly Property IsRefreshingPatches()
			Get
				Return _refreshPatches IsNot Nothing AndAlso _refreshPatches.IsAlive
			End Get
		End Property

		Public ReadOnly Property Mods()
			Get
				Return _mods
			End Get
		End Property
		Protected _mods As New List(Of Modification)

		Public Overridable Property ModPath() As String
			Get
				Return MyOptions(Me.GetType.Name & "ModPath")
			End Get
			Set(ByVal value As String)
				Dim mp As String = GetModPath()
				If value = "" Then
					value = Nothing
				End If
				If value Is Nothing Then
					value = AutoModPath
				End If

				Dim valuedi As New DirectoryInfo(value)
				Dim currentdi As New DirectoryInfo(mp)
				Dim autodi As New DirectoryInfo(AutoModPath)
				If valuedi.FullName.ToLower = currentdi.FullName.ToLower Then
					If valuedi.FullName.ToLower = autodi.FullName.ToLower Then
						MyOptions(Me.GetType.Name & "ModPath") = Nothing
					End If

					Return
				End If

				If valuedi.FullName.ToLower = autodi.FullName.ToLower Then
					MyOptions(Me.GetType.Name & "ModPath") = Nothing
				Else
					MyOptions(Me.GetType.Name & "ModPath") = value
				End If

				RaiseEvent ModPathChanged(Me, New EventArgs)
			End Set
		End Property

		Public MustOverride ReadOnly Property Image16() As Image
		Public MustOverride ReadOnly Property Image32() As Image
		Public MustOverride ReadOnly Property Name() As String

		Public ReadOnly Property Patches()
			Get
				Return _patches
			End Get
		End Property
		Protected _patches As New List(Of Patch)

		Public Overridable Property PatchPath() As String
			Get
				Return MyOptions(Me.GetType.Name & "PatchPath")
			End Get
			Set(ByVal value As String)
				Dim mp As String = GetPatchPath()
				If value = "" Then
					value = Nothing
				End If
				If value Is Nothing Then
					value = AutoModPath
				End If

				Dim valuedi As New DirectoryInfo(value)
				Dim currentdi As New DirectoryInfo(mp)
				Dim autodi As New DirectoryInfo(AutoModPath)
				If valuedi.FullName.ToLower = currentdi.FullName.ToLower Then
					If valuedi.FullName.ToLower = autodi.FullName.ToLower Then
						MyOptions(Me.GetType.Name & "PatchPath") = Nothing
					End If

					Return
				End If

				If valuedi.FullName.ToLower = autodi.FullName.ToLower Then
					MyOptions(Me.GetType.Name & "PatchPath") = Nothing
				Else
					MyOptions(Me.GetType.Name & "PatchPath") = value
				End If

				RaiseEvent PatchPathChanged(Me, New EventArgs)
			End Set
		End Property

		Public MustOverride ReadOnly Property SupportsPatches() As Boolean
#End Region

#Region "Events"
		Public Event ActiveModPathChanged(ByVal sender As Object, ByVal e As EventArgs)

		Public Event AbortModSearch(ByVal sender As Object, ByVal e As EventArgs)
		Public Event AbortPatchSearch(ByVal sender As Object, ByVal e As EventArgs)
		Public Event BeginModSearch(ByVal sender As Object, ByVal e As EventArgs)
		Public Event BeginPatchSearch(ByVal sender As Object, ByVal e As EventArgs)
		Public Event EndModSearch(ByVal sender As Object, ByVal e As EventArgs)
		Public Event EndPatchSearch(ByVal sender As Object, ByVal e As EventArgs)

		Public Event EnabledChanged(ByVal sender As Object, ByVal e As EventArgs)
		Public Event GamePathChanged(ByVal sender As Object, ByVal e As EventArgs)

		Public Class ModFoundEventArgs
			Inherits EventArgs

			Public Sub New(ByVal m As Modification)
				_modification = m
			End Sub

			Public ReadOnly Property Modification() As Modification
				Get
					Return _modification
				End Get
			End Property
			Private _modification As Modification
		End Class
		Public Event ModFound(ByVal sender As Object, ByVal e As ModFoundEventArgs)

		Public Event ModPathChanged(ByVal sender As Object, ByVal e As EventArgs)

		Public Class PatchFoundEventArgs
			Inherits EventArgs

			Public Sub New(ByVal p As Patch)
				_patch = p
			End Sub

			Public ReadOnly Property Patch() As Patch
				Get
					Return _patch
				End Get
			End Property
			Private _patch As Patch
		End Class
		Public Event PatchFound(ByVal sender As Object, ByVal e As PatchFoundEventArgs)
		Public Event PatchPathChanged(ByVal sender As Object, ByVal e As EventArgs)
#End Region
	End Class

#Region "Dark Forces"
	Public Class DarkForces
		Inherits Game

		Private Function BuildArguments(ByVal moduid As String) As String
			Dim args As String = ""
			If moduid IsNot Nothing Then
				args = "-u" & moduid & ".GOB "
			End If

			If MyOptions.DarkForcesAutoTest Then
				args &= "-t "
			End If

			If MyOptions.DarkForcesCDDrive <> Chr(0) Then
				args &= "-x" & MyOptions.DarkForcesCDDrive & " "
			End If

			If MyOptions.DarkForcesEnableScreenshots Then
				args &= "-shots "
			End If

			If MyOptions.DarkForcesLevelSelect IsNot Nothing Then
				args &= "-l" & MyOptions.DarkForcesLevelSelect & " "
			End If

			If MyOptions.DarkForcesLog Then
				args &= "-g "
			End If

			If MyOptions.DarkForcesSkipCutscenes Then
				args &= "-c "
			End If

			If MyOptions.DarkForcesSkipFILESCheck Then
				args &= "-f "
			End If

			If MyOptions.DarkForcesSkipMemoryCheck Then
				args &= "-m "
			End If

			Return args.TrimEnd
		End Function

		Public Shared Function FindFirstCDDrive() As Char
			For Each d As DriveInfo In DriveInfo.GetDrives()
				If d.DriveType = DriveType.CDRom Then
					Return d.Name(0)
				End If
			Next d

			Return Chr(0)
		End Function

		Public Shared Function IsValidDarkXLPath(ByVal folder As String) As Boolean
			If folder Is Nothing Then
				Return False
			End If

			Return File.Exists(Path.Combine(folder, "darkxl.exe"))
		End Function

		Public Shared Function IsValidDOSBoxPath(ByVal folder As String) As Boolean
			If folder Is Nothing Then
				Return False
			End If

			Return File.Exists(Path.Combine(folder, "dosbox.exe"))
		End Function

		Public Overrides Function IsValidGamePath(ByVal folder As String) As Boolean
			Dim fi As New FileInfo(Path.Combine(folder, "dark.exe"))
			Return fi.Exists
		End Function

		Protected Overrides Sub RefreshModsInternal()
			Dim mpath As New DirectoryInfo(GetModPath)
			If Not mpath.Exists Then
				RaiseEndModSearchEvent()
				Return
			End If

			Dim m As Modification = Nothing
			If File.Exists(Path.Combine(mpath.FullName, "dfrntend.exe")) Then
				SyncLock Threading.Thread.CurrentThread
					m = New DarkFrontend(Me)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			End If

			Dim invalidmods() As String = New String() {"dark", "sounds", "sprites",
								"textures"}

			For Each dir As DirectoryInfo In mpath.GetDirectories()
				If Not File.Exists(Path.Combine(dir.FullName, dir.Name & ".gob")) Then
					Continue For
				End If

				Dim uid As String = Nothing
				Try
					uid = WinAPI.GetShortPathName(dir.FullName)
				Catch ex As Win32Exception
					uid = dir.FullName
				End Try

				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				If Array.IndexOf(invalidmods, uid.ToLower) > -1 Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New DarkForcesMod(Me, Path.GetFileName(uid))
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next dir

			For Each gob As FileInfo In mpath.GetFiles("*.gob")
				Dim uid As String = Nothing
				Try
					uid = Path.GetFileNameWithoutExtension(WinAPI.GetShortPathName(gob.
												FullName))
				Catch ex As Win32Exception
					uid = Path.GetFileNameWithoutExtension(gob.Name)
				End Try

				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				If Array.IndexOf(invalidmods, uid.ToLower) > -1 Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New DarkForcesModMessy(Me, uid)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next gob

			For Each zip As FileInfo In mpath.GetFiles("*.zip")
				Dim uid As String = Nothing
				Try
					uid = Path.GetFileNameWithoutExtension(WinAPI.GetShortPathName(
												zip.FullName))
				Catch ex As Win32Exception
					uid = Path.GetFileNameWithoutExtension(zip.Name)
				End Try

				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				If Array.IndexOf(invalidmods, uid.ToLower) > -1 Then
					Continue For
				End If

				Dim z As New ZipFile(zip.FullName)
				If z.FindEntry(uid & ".gob", True) < 0 Then
					Continue For
				End If
				z.Close()

				SyncLock Threading.Thread.CurrentThread
					m = New DarkForcesModZipped(Me, uid)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next zip

			RaiseEndModSearchEvent()
		End Sub

		Public Overrides Sub Run()
			Dim fi As New FileInfo(Path.Combine(GamePath, "dark.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Dark Forces!", MsgBoxStyle.Exclamation,
										"Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			Select Case MyOptions.DarkForcesRunMode
				Case Options.DarkForcesRunModes.Native
					psi.FileName = fi.FullName
					psi.WorkingDirectory = fi.DirectoryName
					psi.Arguments = BuildArguments(Nothing)
				Case Options.DarkForcesRunModes.DOSBox
					psi.FileName = Path.Combine(MyOptions.DarkForcesDOSBoxPath,
												"dosbox.exe")
					psi.WorkingDirectory = MyOptions.DarkForcesDOSBoxPath
					psi.Arguments = "-noconsole "
					psi.Arguments &= "-c ""mount c: '" & fi.DirectoryName & "'"" "

					If Not File.Exists(Path.Combine(GamePath, "CD.ID")) Then
						psi.Arguments &= "-c ""mount d: '" & DarkForces.
														FindFirstCDDrive & ":\'"" "
					End If

					psi.Arguments &= "-c ""c:"" "
					psi.Arguments &= "-c """ & fi.Name & " " & BuildArguments(Nothing) _
												& """ "
					psi.Arguments &= "-c ""exit"""
				Case Options.DarkForcesRunModes.DarkXL
					psi.FileName = Path.Combine(MyOptions.DarkForcesDarkXLPath,
												"darkxl.exe")
					psi.WorkingDirectory = MyOptions.DarkForcesDarkXLPath
					psi.Arguments = BuildArguments(Nothing)
			End Select
			Process.Start(psi)
		End Sub

		Public Overrides Sub Run(ByVal mods() As Game.Modification)
			If TypeOf mods(0) Is DarkFrontend Then
				RunDarkFrontend()
				Return
			End If

			Dim fi As New FileInfo(Path.Combine(GamePath, "dark.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Dark Forces!", MsgBoxStyle.Exclamation,
										"Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			Select Case MyOptions.DarkForcesRunMode
				Case Options.DarkForcesRunModes.Native
					psi.FileName = fi.FullName
					psi.WorkingDirectory = fi.DirectoryName
					psi.Arguments = BuildArguments(mods(0).UniqueID)
				Case Options.DarkForcesRunModes.DOSBox
					psi.FileName = Path.Combine(MyOptions.DarkForcesDOSBoxPath,
												"dosbox.exe")
					psi.WorkingDirectory = MyOptions.DarkForcesDOSBoxPath
					psi.Arguments = "-noconsole "
					psi.Arguments &= "-c ""mount c: '" & fi.DirectoryName & "'"" "

					If Not File.Exists(Path.Combine(GamePath, "CD.ID")) Then
						psi.Arguments &= "-c ""mount d: '" & DarkForces.
														FindFirstCDDrive & ":\'"" "
					End If

					psi.Arguments &= "-c ""c:"" "
					psi.Arguments &= "-c """ & fi.Name & " " & BuildArguments(mods(0).
												UniqueID) & """ "
					psi.Arguments &= "-c ""exit"""
				Case Options.DarkForcesRunModes.DarkXL
					psi.FileName = Path.Combine(MyOptions.DarkForcesDarkXLPath,
												"darkxl.exe")
					psi.WorkingDirectory = MyOptions.DarkForcesDarkXLPath
					psi.Arguments = BuildArguments(mods(0).UniqueID)
			End Select
			Process.Start(psi)
		End Sub

		Private Sub RunDarkFrontend()
			Dim fi As New FileInfo(Path.Combine(GetModPath, "dfrntend.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Dark Frontend!", MsgBoxStyle.Exclamation,
										"Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			If MyOptions.DarkForcesRunMode = Options.DarkForcesRunModes.Native OrElse
								Not DarkForces.IsValidDOSBoxPath(MyOptions.DarkForcesDOSBoxPath) Then

				psi.FileName = fi.FullName
				psi.WorkingDirectory = fi.DirectoryName
			Else
				psi.FileName = Path.Combine(MyOptions.DarkForcesDOSBoxPath, "dosbox.exe")
				psi.WorkingDirectory = MyOptions.DarkForcesDOSBoxPath
				psi.Arguments = "-noconsole "

				Dim gp As String = Path.GetFullPath(GamePath)
				psi.Arguments &= "-c ""mount c: '" & gp & "'"" "

				If Not File.Exists(Path.Combine(GamePath, "CD.ID")) Then
					psi.Arguments &= "-c ""mount d: '" & DarkForces.
												FindFirstCDDrive & ":\'"" "
				End If

				If fi.DirectoryName.ToLower.IndexOf(gp.ToLower) = 0 Then
					Dim segments As Integer = fi.DirectoryName.Substring(gp.Length).
												Trim(Path.DirectorySeparatorChar).Split(Path.
												DirectorySeparatorChar).Length

					Dim spath As String = Nothing
					Try
						spath = WinAPI.GetShortPathName(fi.DirectoryName)
					Catch ex As Win32Exception
						spath = fi.DirectoryName
					End Try

					psi.Arguments &= "-c ""c:"" "

					Dim shorts As String() = spath.Split(Path.DirectorySeparatorChar)

					psi.Arguments &= "-c ""cd " & String.Join(Path.
												DirectorySeparatorChar, shorts, shorts.Length - segments,
												segments) & """ "
				Else
					psi.Arguments &= "-c ""mount e: '" & fi.DirectoryName & "'"" "
					psi.Arguments &= "-c ""e:"" "
				End If

				psi.Arguments &= "-c """ & fi.Name & """ "
				psi.Arguments &= "-c ""exit"""
			End If
			Process.Start(psi)
		End Sub

		Public Sub RunIMuse()
			Dim fi As New FileInfo(Path.Combine(GamePath, "imuse.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Dark Forces iMuse tool!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			If MyOptions.DarkForcesRunMode = Options.DarkForcesRunModes.Native OrElse
								Not DarkForces.IsValidDOSBoxPath(MyOptions.DarkForcesDOSBoxPath) Then

				psi.FileName = fi.FullName
				psi.WorkingDirectory = fi.DirectoryName
			Else
				psi.FileName = Path.Combine(MyOptions.DarkForcesDOSBoxPath, "dosbox.exe")
				psi.WorkingDirectory = MyOptions.DarkForcesDOSBoxPath
				psi.Arguments = "-noconsole "
				psi.Arguments &= "-c ""mount c: '" & fi.DirectoryName & "'"" "
				psi.Arguments &= "-c ""c:"" "
				psi.Arguments &= "-c """ & fi.Name & """ "
				psi.Arguments &= "-c ""exit"""
			End If
			Process.Start(psi)
		End Sub

		Public Sub RunSetup()
			Dim fi As New FileInfo(Path.Combine(GamePath, "setup.exe"))
			If Not fi.Exists Then
				fi = New FileInfo(Path.Combine(GamePath, "install.exe"))
			End If
			If Not fi.Exists Then
				MsgBox("Unable to locate Dark Forces Setup tool!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			If MyOptions.DarkForcesRunMode = Options.DarkForcesRunModes.Native OrElse
								Not DarkForces.IsValidDOSBoxPath(MyOptions.DarkForcesDOSBoxPath) Then

				psi.FileName = fi.FullName
				psi.WorkingDirectory = fi.DirectoryName
			Else
				psi.FileName = Path.Combine(MyOptions.DarkForcesDOSBoxPath, "dosbox.exe")
				psi.WorkingDirectory = MyOptions.DarkForcesDOSBoxPath
				psi.Arguments = "-noconsole "
				psi.Arguments &= "-c ""mount c: '" & fi.DirectoryName & "'"" "
				psi.Arguments &= "-c ""c:"" "
				psi.Arguments &= "-c """ & fi.Name & """ "
				psi.Arguments &= "-c ""exit"""
			End If
			Process.Start(psi)
		End Sub

		Public Overrides Sub ShowOptions()
			Dim f As New fDarkForcesOptions(Me)
			f.ShowDialog()
		End Sub

#Region "Properties"
		Public Overrides Property ActiveModPath() As String
			Get
				Return MyBase.ActiveModPath
			End Get
			Set(ByVal value As String)
				Throw New NotSupportedException(
										"Can't set ActiveModPath for DarkForces.")
			End Set
		End Property

		Public Overrides ReadOnly Property AllowMultipleActiveMods() As Boolean
			Get
				Return False
			End Get
		End Property

		Public Overrides ReadOnly Property AutoActiveModPath() As String
			Get
				Return GamePath
			End Get
		End Property

		Public Overrides ReadOnly Property AutoGamePath() As String
			Get
				If Not SteamHelper.IsAppInstalled(SteamHelper.AppIDs.DarkForces) Then
					Return Nothing
				End If

				Dim steampath As String = SteamHelper.GetSteamPath
				If steampath Is Nothing Then
					Return Nothing
				End If

				Dim gamepath As String = Path.Combine(steampath,
										"SteamApps\common\dark forces\Game")
				If IsValidGamePath(gamepath) Then
					Return gamepath
				End If

				Return Nothing
			End Get
		End Property

		Public Overrides ReadOnly Property AutoModPath() As String
			Get
				Return Path.Combine(GamePath, "Levels")
			End Get
		End Property

		Public Overrides ReadOnly Property HasSeparateMultiplayerBinary() As Boolean
			Get
				Return False
			End Get
		End Property

		Public Overrides ReadOnly Property Image16() As System.Drawing.Image
			Get
				Return My.Resources.game_df_16
			End Get
		End Property

		Public Overrides ReadOnly Property Image32() As System.Drawing.Image
			Get
				Return My.Resources.game_df_32
			End Get
		End Property

		Public Overrides ReadOnly Property Name() As String
			Get
				Return "Dark Forces"
			End Get
		End Property

		Public Overrides ReadOnly Property SupportsPatches() As Boolean
			Get
				Return False
			End Get
		End Property
#End Region
	End Class

	Public Class DarkFrontend
		Inherits Game.Modification

		Public Sub New(ByVal owner As DarkForces)
			MyBase.New(owner)
		End Sub

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			Return Nothing
		End Function

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return _active
			End Get
			Set(ByVal value As Boolean)
				If Active = value Then
					Return
				End If

				If value Then
					_active = True
				Else
					_active = False
				End If
			End Set
		End Property
		Private _active As Boolean = False

		Public Overrides ReadOnly Property SortOrder() As Integer
			Get
				Return -1
			End Get
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return "Dark Frontend"
			End Get
		End Property
#End Region
	End Class

	Public Class DarkForcesModMessy
		Inherits Game.Modification

		Public Sub New(ByVal owner As DarkForces, ByVal uid As String)
			MyBase.New(owner)
			_uid = uid
		End Sub

		Public Function GetUsableFiles() As String()
			Dim di As New DirectoryInfo(_owner.GetModPath)
			If Not di.Exists Then
				Return New String() {}
			End If

			Dim disallowedextensions As String() = New String() {"bat", "txt", "gob",
								"ini", "exe", "zip", "pcx", "cfg", "cd", "msg", "id", "srn", "ico",
								"ex", "e", "cmd", "pif", "lnk", "url", "doc", "rtf"}

			Dim files As New List(Of String)
			For Each fi As FileInfo In di.GetFiles()
				If Array.IndexOf(disallowedextensions, fi.Extension.ToLower.TrimStart(
										"."c)) > -1 Then

					Continue For
				End If

				files.Add(fi.Name)
			Next fi
			Return files.ToArray
		End Function

		Public Overrides Sub ShowOptions()
			Dim f As New fDarkForcesModOptions(_owner, Me)
			f.UsableFiles = GetUsableFiles()
			f.ShowDialog()
		End Sub

		Private Sub CopyToDFDir(ByVal srcname As String, ByVal destname As String)
			Dim src As New IO.FileInfo(Path.Combine(_owner.GetModPath, srcname))
			Dim dest As New IO.FileInfo(Path.Combine(_owner.GetActiveModPath, destname))

			If Not src.Exists Then
				Return
			End If

			If src.FullName.ToLower = dest.FullName.ToLower Then
				Return
			End If

			If dest.Exists Then
				dest.Delete()
			End If

			src.CopyTo(dest.FullName)
		End Sub

		Private Sub Activate()
			Dim gob As String = Path.Combine(_owner.GetModPath, _uid & ".GOB")
			If Not File.Exists(gob) Then
				Throw New FileNotFoundException(gob &
										" not found when trying to copy to Dark Forces directory.")
			End If

			CopyToDFDir(_uid & ".GOB", _uid & ".GOB")

			Dim m As ModInfoCache.Modification = MyModInfo(_owner.Name, _uid)
			If m Is Nothing Then
				Return
			End If

			If Not TypeOf m Is DarkForcesModInfo Then
				Return
			End If

			Dim dm As DarkForcesModInfo = m
			If dm.LFD IsNot Nothing Then
				CopyToDFDir(dm.LFD, "DFBRIEF.LFD")
			End If
			If dm.Crawl IsNot Nothing Then
				CopyToDFDir(dm.Crawl, "FTEXTCRA.LFD")
			End If
			For Each file As String In dm.Others
				CopyToDFDir(file, file)
			Next file
		End Sub

		Private Sub RemoveFromDFDir(ByVal name As String)
			Dim fi As New IO.FileInfo(Path.Combine(_owner.GetActiveModPath, name))
			If fi.Exists Then
				fi.Delete()
			End If
		End Sub

		Private Sub Deactivate()
			Dim samepath As Boolean = _owner.GetModPath.ToLower = _owner.
								GetActiveModPath.ToLower

			If Not samepath Then
				RemoveFromDFDir(_uid & ".GOB")
			End If

			Dim m As ModInfoCache.Modification = MyModInfo(_owner.Name, _uid)
			If m Is Nothing Then
				Return
			End If

			If Not TypeOf m Is DarkForcesModInfo Then
				Return
			End If

			Dim dm As DarkForcesModInfo = m
			If dm.LFD IsNot Nothing AndAlso dm.LFD.ToUpper <> "DFBRIEF.LFD" Then
				RemoveFromDFDir("DFBRIEF.LFD")
			End If
			If dm.Crawl IsNot Nothing AndAlso dm.LFD.ToUpper <> "FTEXTCRA.LFD" Then
				RemoveFromDFDir("FTEXTCRA.LFD")
			End If
			If Not samepath Then
				For Each file As String In dm.Others
					RemoveFromDFDir(file)
				Next file
			End If
		End Sub

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			Return Nothing
		End Function

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Dim samepath As Boolean = _owner.GetModPath.ToLower = _owner.
									 GetActiveModPath.ToLower

				If samepath Then
					Return _active
				End If

				Return File.Exists(Path.Combine(_owner.GetActiveModPath, UniqueID &
										".GOB"))
			End Get
			Set(ByVal value As Boolean)
				If Active = value Then
					Return
				End If

				If value Then
					Activate()
					_active = True
				Else
					Deactivate()
					_active = False
				End If
			End Set
		End Property
		Private _active As Boolean = False

		Public Overrides ReadOnly Property HasOptions() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _uid
			End Get
		End Property
		Private _uid As String
#End Region
	End Class

	Public Class DarkForcesMod
		Inherits Game.Modification

		Public Sub New(ByVal owner As DarkForces, ByVal uid As String)
			MyBase.New(owner)
			_uid = uid
		End Sub

		Public Function GetUsableFiles() As String()
			Dim di As New DirectoryInfo(Path.Combine(_owner.GetModPath, _uid))
			If Not di.Exists Then
				Return New String() {}
			End If

			Dim disallowedextensions As String() = New String() {"bat", "txt", "gob",
								"ini", "exe", "zip", "pcx", "cfg", "cd", "msg", "id", "srn", "ico",
								"ex", "e", "cmd", "pif", "lnk", "url", "doc", "rtf"}

			Dim files As New List(Of String)
			For Each fi As FileInfo In di.GetFiles()
				If Array.IndexOf(disallowedextensions, fi.Extension.ToLower.TrimStart(
										"."c)) > -1 Then

					Continue For
				End If

				files.Add(fi.Name)
			Next fi
			Return files.ToArray
		End Function

		Public Overrides Sub ShowOptions()
			Dim f As New fDarkForcesModOptions(_owner, Me)
			f.UsableFiles = GetUsableFiles()
			f.ShowDialog()
		End Sub

		Private Sub CopyToDFDir(ByVal srcname As String, ByVal destname As String)
			Dim src As New IO.FileInfo(Path.Combine(Path.Combine(_owner.GetModPath,
								_uid), srcname))
			Dim dest As New IO.FileInfo(Path.Combine(_owner.GetActiveModPath, destname))

			If Not src.Exists Then
				Return
			End If

			If dest.Exists Then
				dest.Delete()
			End If

			src.CopyTo(dest.FullName)
		End Sub

		Private Sub Activate()
			Dim di As New DirectoryInfo(Path.Combine(_owner.GetModPath, _uid))
			If Not di.Exists Then
				Throw New DirectoryNotFoundException(di.FullName &
										" not found when trying to activate.")
			End If

			Dim gob As String = Path.Combine(_owner.GetModPath, _uid & Path.
								DirectorySeparatorChar & _uid & ".GOB")
			If Not File.Exists(gob) Then
				Throw New FileNotFoundException(gob &
										" not found when trying to copy to Dark Forces directory.")
			End If

			CopyToDFDir(_uid & ".GOB", _uid & ".GOB")

			Dim m As ModInfoCache.Modification = MyModInfo(_owner.Name, _uid)
			If m Is Nothing Then
				Return
			End If

			If Not TypeOf m Is DarkForcesModInfo Then
				Return
			End If

			Dim dm As DarkForcesModInfo = m
			If dm.LFD IsNot Nothing Then
				CopyToDFDir(dm.LFD, "DFBRIEF.LFD")
			End If
			If dm.Crawl IsNot Nothing Then
				CopyToDFDir(dm.Crawl, "FTEXTCRA.LFD")
			End If
			For Each file As String In dm.Others
				CopyToDFDir(file, file)
			Next file
		End Sub

		Private Sub RemoveFromDFDir(ByVal name As String)
			Dim fi As New IO.FileInfo(Path.Combine(_owner.GetActiveModPath, name))
			If fi.Exists Then
				fi.Delete()
			End If
		End Sub

		Private Sub Deactivate()
			RemoveFromDFDir(_uid & ".GOB")

			Dim m As ModInfoCache.Modification = MyModInfo(_owner.Name, _uid)
			If m Is Nothing Then
				Return
			End If

			If Not TypeOf m Is DarkForcesModInfo Then
				Return
			End If

			Dim dm As DarkForcesModInfo = m
			If dm.LFD IsNot Nothing Then
				RemoveFromDFDir("DFBRIEF.LFD")
			End If
			If dm.Crawl IsNot Nothing Then
				RemoveFromDFDir("FTEXTCRA.LFD")
			End If
			For Each file As String In dm.Others
				RemoveFromDFDir(file)
			Next file
		End Sub

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			Return Nothing
		End Function

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return File.Exists(Path.Combine(_owner.GetActiveModPath, UniqueID &
										".GOB"))
			End Get
			Set(ByVal value As Boolean)
				If Active = value Then
					Return
				End If

				If value Then
					Activate()
				Else
					Deactivate()
				End If
			End Set
		End Property

		Public Overrides ReadOnly Property HasOptions() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _uid
			End Get
		End Property
		Private _uid As String
#End Region
	End Class

	Public Class DarkForcesModZipped
		Inherits Game.Modification

		Public Sub New(ByVal owner As DarkForces, ByVal uid As String)
			MyBase.New(owner)
			_uid = uid
		End Sub

		Public Function GetUsableFiles() As String()
			Dim fi As New FileInfo(Path.Combine(_owner.GetModPath, _uid & ".ZIP"))
			If Not fi.Exists Then
				Return New String() {}
			End If

			Dim disallowedextensions As String() = New String() {"bat", "txt", "gob",
								"ini", "exe", "zip", "pcx", "cfg", "cd", "msg", "id", "srn", "ico",
								"ex", "e", "cmd", "pif", "lnk", "url", "doc", "rtf"}

			Dim files As New List(Of String)

			Dim z As New ZipFile(fi.FullName)
			For Each ze As ZipEntry In z
				If Array.IndexOf(disallowedextensions, Path.GetExtension(ze.Name).
										ToLower.TrimStart("."c)) > -1 Then

					Continue For
				End If

				files.Add(fi.Name)
			Next ze
			z.Close()
			Return files.ToArray
		End Function

		Public Overrides Sub ShowOptions()
			Dim f As New fDarkForcesModOptions(_owner, Me)
			f.UsableFiles = GetUsableFiles()
			f.ShowDialog()
		End Sub

		Private Sub Activate()
			Dim fi As New FileInfo(Path.Combine(_owner.GetModPath, _uid & ".ZIP"))
			If Not fi.Exists Then
				Throw New FileNotFoundException(fi.FullName &
										" not found when trying to activate.")
			End If

			Dim files As New Dictionary(Of String, String)
			files.Add(_uid & ".GOB", _uid & ".GOB")

			Dim m As ModInfoCache.Modification = MyModInfo(_owner.Name, _uid)
			If m IsNot Nothing AndAlso TypeOf m Is DarkForcesModInfo Then
				Dim dm As DarkForcesModInfo = m
				If dm.LFD IsNot Nothing Then
					files.Add(dm.LFD, "DFBRIEF.LFD")
				End If
				If dm.Crawl IsNot Nothing Then
					files.Add(dm.Crawl, "FTEXTCRA.LFD")
				End If
				For Each s As String In dm.Others
					files.Add(s, s)
				Next s
			End If

			Dim z As New ZipFile(fi.FullName)
			Dim ze As ZipEntry
			Dim dest As FileInfo
			For Each file As KeyValuePair(Of String, String) In files
				ze = z.GetEntry(file.Key)
				If ze Is Nothing Then
					If file.Key <> _uid & ".GOB" Then
						Continue For
					End If

					Throw New FileNotFoundException(file.Key &
												" not found when trying to activate.")
				End If

				dest = New FileInfo(Path.Combine(_owner.GetActiveModPath, file.Value))
				If dest.Exists Then
					dest.Delete()
				End If

				Dim s As Stream = z.GetInputStream(ze)
				Dim fs As FileStream = dest.Open(FileMode.Create, FileAccess.Write)
				Dim read As Integer = 1
				Dim b(524287) As Byte
				While read > 0
					read = s.Read(b, 0, b.Length - 1)
					If read > 0 Then
						fs.Write(b, 0, read)
					End If
				End While
				fs.Close()
				fs.Dispose()
				s.Close()
				s.Dispose()
			Next file
			z.Close()
		End Sub

		Private Sub RemoveFromDFDir(ByVal name As String)
			Dim fi As New IO.FileInfo(Path.Combine(_owner.GetActiveModPath, name))
			If fi.Exists Then
				fi.Delete()
			End If
		End Sub

		Private Sub Deactivate()
			RemoveFromDFDir(_uid & ".GOB")

			Dim m As ModInfoCache.Modification = MyModInfo(_owner.Name, _uid)
			If m Is Nothing Then
				Return
			End If

			If Not TypeOf m Is DarkForcesModInfo Then
				Return
			End If

			Dim dm As DarkForcesModInfo = m
			If dm.LFD IsNot Nothing Then
				RemoveFromDFDir("DFBRIEF.LFD")
			End If
			If dm.Crawl IsNot Nothing Then
				RemoveFromDFDir("FTEXTCRA.LFD")
			End If
			For Each file As String In dm.Others
				RemoveFromDFDir(file)
			Next file
		End Sub

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			Return Nothing
		End Function

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return File.Exists(Path.Combine(_owner.GetActiveModPath, UniqueID &
										".GOB"))
			End Get
			Set(ByVal value As Boolean)
				If Active = value Then
					Return
				End If

				If value Then
					Activate()
				Else
					Deactivate()
				End If
			End Set
		End Property

		Public Overrides ReadOnly Property HasOptions() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _uid
			End Get
		End Property
		Private _uid As String
#End Region
	End Class
#End Region

#Region "Sith Engine"
	Public Class JediKnight
		Inherits Game

		Public Function BuildArguments(ByVal useMods As Boolean) As String
			Dim args As String = ""
			If useMods Then
				Dim gp As String = Path.GetFullPath(GamePath)
				Dim ap As String = Path.GetFullPath(GetActiveModPath)
				If ap.ToLower.IndexOf(gp.ToLower) = 0 Then
					Dim segments As Integer = ap.Substring(gp.Length).Trim(Path.
												DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Length
					Dim shorts As String() = ap.Split(Path.DirectorySeparatorChar)

					args = "-path " & String.Join(Path.DirectorySeparatorChar, shorts,
												shorts.Length - segments, segments) & " "
				Else
					MsgBox("Invalid active mod path, mods will not be applied.  " &
												"Please set the active mod path in the game options to a " &
												"subdirectory of the game directory without spaces.",
												MsgBoxStyle.Exclamation, "Error - Knight")
				End If
			End If

			If MyOptions.JediKnightAdvancedDisplayOptions Then
				args &= "-displayconfig "
			End If

			If MyOptions.JediKnightConsole Then
				args &= "-devmods "
			End If

			If MyOptions.JediKnightConsoleStats Then
				args &= "-dispstats "
			End If

			If MyOptions.JediKnightEnableJKUPCogVerbs Then
				Dim jk As String = Path.Combine(GamePath, "jk.exe")
				If File.Exists(jk) Then
					Dim h As HashList.Hash = MyHashes.IdentifyFile(jk)
					If h IsNot Nothing AndAlso h.Name.Contains("Unofficial Patch") _
												Then

						args &= "-z "
					End If
				End If
			End If

			If MyOptions.JediKnightFramerate Then
				args &= "-framerate "
			End If

			Select Case MyOptions.JediKnightLog
				Case Options.LogTypes.File
					args &= "-debug log "
				Case Options.LogTypes.WindowsConsole
					args &= "-debug con "
			End Select

			If MyOptions.JediKnightNoHUD Then
				args &= "-nohud "
			End If

			If MyOptions.JediKnightVerbosity > -1 Then
				args &= "-verbose " & MyOptions.JediKnightVerbosity.ToString & " "
			End If

			If MyOptions.JediKnightWindowedGUI Then
				args &= "-windowgui "
			End If

			Return args.TrimEnd
		End Function

		Private Function GetUniquePatchName(ByVal base As String) As String
			If Not File.Exists(Path.Combine(GetPatchPath, base & ".exe")) Then
				Return base & ".exe"
			End If

			Dim i As Integer = 0
			While File.Exists(Path.Combine(GetPatchPath, base & "." & i.ToString &
								".exe"))

				i += 1
			End While
			Return base & "." & i.ToString & ".exe"
		End Function

		Public Overrides Function InstallPatch(ByVal filename As String, ByVal h As _
						HashList.Hash) As Games.Game.Patch

			Dim gp As New DirectoryInfo(GetPatchPath)
			Dim patch As New FileInfo(filename)

			Dim finalname As String = Path.GetFileName(filename)
			If gp.FullName <> patch.DirectoryName Then
				finalname = GetUniquePatchName(Path.GetFileNameWithoutExtension(
										h.FileName))
				File.Copy(filename, Path.Combine(GetPatchPath, finalname))
			End If

			MyHashes(h.UniqueID) = h

			Dim p As New SithPatch(Me, h, Path.Combine(GetPatchPath, finalname), Path.
								Combine(GamePath, "jk.exe"), False)
			_patches.Add(p)
			Return p
		End Function

		Public Function IsValidActiveModPath(ByVal folder As String) As Boolean
			Dim full As String = Path.GetFullPath(folder).TrimEnd(Path.
								DirectorySeparatorChar).ToLower
			Dim fullmod As String = Path.GetFullPath(GetModPath).TrimEnd(Path.
								DirectorySeparatorChar).ToLower
			Dim fullgame As String = Path.GetFullPath(GamePath).TrimEnd(Path.
								DirectorySeparatorChar).ToLower

			If full.IndexOf(fullgame) <> 0 OrElse full = fullgame OrElse full =
								fullmod Then

				Return False
			End If
			Return Not full.Substring(fullgame.Length).Contains(" "c)
		End Function

		Public Overrides Function IsValidGamePath(ByVal folder As String) As Boolean
			Dim fi As New FileInfo(Path.Combine(folder, "jk.exe"))
			Return fi.Exists
		End Function

		Private Function ReadLECPath() As String
			Dim rk As RegistryKey = Registry.LocalMachine.OpenSubKey(
								"SOFTWARE\LucasArts Entertainment Company\JediKnight\1.0", False)
			If rk Is Nothing Then
				Return Nothing
			End If

			Dim installpath As String = rk.GetValue("Install Path", Nothing)
			rk.Close()
			If installpath Is Nothing Then
				Return Nothing
			End If

			If IsValidGamePath(installpath) Then
				Return installpath
			End If

			Return Nothing
		End Function

		Private Function ReadSteamPath() As String
			If Not SteamHelper.IsAppInstalled(SteamHelper.AppIDs.JediKnight) Then
				Return Nothing
			End If

			Dim steampath As String = SteamHelper.GetSteamPath
			If steampath Is Nothing Then
				Return Nothing
			End If

			Dim gamepath As String = Path.Combine(steampath,
								"SteamApps\common\star wars jedi knight")
			If IsValidGamePath(gamepath) Then
				Return gamepath
			End If

			Return Nothing
		End Function

		Private Sub RefreshActiveMods()
			Dim mpath As New DirectoryInfo(GetActiveModPath)
			If Not mpath.Exists Then
				Return
			End If

			Dim m As Modification = Nothing
			For Each gob As FileInfo In mpath.GetFiles("*.gob")
				Dim uid As String = Path.GetFileNameWithoutExtension(gob.Name)
				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New SithMod(Me, uid, False)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next gob
		End Sub

		Private Sub RefreshInactiveMods()
			Dim mpath As New DirectoryInfo(GetModPath)
			If Not mpath.Exists Then
				Return
			End If

			Dim m As Modification = Nothing
			For Each gob As FileInfo In mpath.GetFiles("*.gob")
				Dim uid As String = Path.GetFileNameWithoutExtension(gob.Name)
				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New SithMod(Me, uid, False)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next gob

			For Each zip As FileInfo In mpath.GetFiles("*.zip")
				Dim z As New ZipFile(zip.FullName)
				For Each ze As ZipEntry In z
					If Not ze.Name.ToLower.EndsWith(".gob") Then
						Continue For
					End If

					Dim uid As String = Path.GetFileNameWithoutExtension(ze.Name)
					If AlreadyFoundMod(uid) Then
						Continue For
					End If

					SyncLock Threading.Thread.CurrentThread
						m = New SithModZipped(Me, uid, zip.FullName, False)
						_mods.Add(m)
					End SyncLock
					RaiseModFoundEvent(m)
				Next ze
				z.Close()
			Next zip
		End Sub

		Protected Overrides Sub RefreshModsInternal()
			RefreshInactiveMods()
			RefreshActiveMods()
			RaiseEndModSearchEvent()
		End Sub

		Private Sub RefreshGamePathPatches()
			Dim mpath As New DirectoryInfo(GetPatchPath)

			Dim p As Patch = Nothing
			Dim jk As String = Path.Combine(GamePath, "jk.exe")
			Dim jkh As HashList.Hash = Nothing
			If File.Exists(jk) Then
				jkh = MyHashes.IdentifyFile(jk)
			End If

			Dim foundactive As Boolean = False
			For Each exe As FileInfo In mpath.GetFiles("*.exe")
				If exe.Name.ToLower = "jk.exe" Then
					Continue For
				End If

				Dim h As HashList.Hash = MyHashes.IdentifyFile(exe.FullName)
				If h Is Nothing OrElse AlreadyFoundPatch(h.UniqueID) Then
					Continue For
				End If

				Dim active As Boolean = jkh IsNot Nothing AndAlso jkh Is h
				If active Then
					foundactive = True
				End If
				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, h, exe.FullName, jk, active)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			Next exe

			If jkh IsNot Nothing AndAlso Not foundactive Then
				Dim dest As String = Path.Combine(GamePath, GetUniquePatchName(
										Path.GetFileNameWithoutExtension(jkh.FileName)))
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, jkh, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			ElseIf Not foundactive AndAlso File.Exists(jk) AndAlso jkh Is Nothing Then
				Dim h As HashList.Hash = MyHashes.HashFile("Unknown Patch (jk.exe)", jk)
				h.UniqueID = h.ValueString
				h.FileName = GetUniquePatchName("jk.unknown.patch")
				MyHashes(h.UniqueID) = h
				Dim dest As String = Path.Combine(GamePath, h.FileName)
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, h, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			End If
		End Sub

		Protected Overrides Sub RefreshPatchesInternal()
			Dim mpath As New DirectoryInfo(GetPatchPath)
			If Not mpath.Exists Then
				Return
			End If

			Dim gpath As New DirectoryInfo(GamePath)
			If mpath.FullName = gpath.FullName Then
				RefreshGamePathPatches()
				MyBase.RefreshPatchesInternal()
				Return
			End If

			Dim p As Patch = Nothing
			Dim jk As String = Path.Combine(GamePath, "jk.exe")
			Dim jkh As HashList.Hash = Nothing
			If File.Exists(jk) Then
				jkh = MyHashes.IdentifyFile(jk)
			End If

			Dim foundactive As Boolean = False
			For Each exe As FileInfo In mpath.GetFiles("*.exe")
				Dim h As HashList.Hash = MyHashes.HashFile("Unknown Patch (" & exe.Name _
										& ")", exe.FullName)
				Dim ih As HashList.Hash = MyHashes.IdentifyFile(h.DataHash, h.DataSize)
				If ih IsNot Nothing AndAlso AlreadyFoundPatch(ih.UniqueID) Then
					Continue For
				End If

				If ih Is Nothing Then
					ih = h
					ih.UniqueID = ih.ValueString
				End If

				Dim active As Boolean = jkh IsNot Nothing AndAlso jkh Is ih
				If active Then
					foundactive = True
				End If
				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, ih, exe.FullName, jk, active)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			Next exe

			If jkh IsNot Nothing AndAlso Not foundactive Then
				Dim dest As String = Path.Combine(GetPatchPath, GetUniquePatchName(
										Path.GetFileNameWithoutExtension(jkh.FileName)))
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, jkh, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			ElseIf Not foundactive AndAlso File.Exists(jk) AndAlso jkh Is Nothing Then
				Dim h As HashList.Hash = MyHashes.HashFile("Unknown Patch (jk.exe)", jk)
				h.UniqueID = h.ValueString
				h.FileName = GetUniquePatchName("jk.unknown.patch")
				MyHashes(h.UniqueID) = h
				Dim dest As String = Path.Combine(GetPatchPath, h.FileName)
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, h, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			End If

			MyBase.RefreshPatchesInternal()
		End Sub

		Public Overrides Sub Run()
			Dim fi As New FileInfo(Path.Combine(GamePath, "jk.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Knight!", MsgBoxStyle.Exclamation,
										"Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(False)

			If MyOptions.JediKnightAllowMultipleInstances Then
				Dim num As Integer = Process.GetProcessesByName("jk").Length + 1

				Dim p As Process = Process.Start(psi)
				p.WaitForInputIdle(5000)
				If Not p.MainWindowHandle.Equals(IntPtr.Zero) Then
					WinAPI.SetWindowText(p.MainWindowHandle, Name & " - Knighted (Instance #" & num.ToString & ")")
				End If
			Else
				Process.Start(psi)
			End If
		End Sub

		Public Overrides Sub Run(ByVal mods() As Game.Modification)
			Dim fi As New FileInfo(Path.Combine(GamePath, "jk.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Knight!", MsgBoxStyle.Exclamation,
										"Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(True)

			If MyOptions.JediKnightAllowMultipleInstances Then
				Dim num As Integer = Process.GetProcessesByName("jk").Length + 1

				Dim p As Process = Process.Start(psi)
				p.WaitForInputIdle(5000)
				If Not p.MainWindowHandle.Equals(IntPtr.Zero) Then
					WinAPI.SetWindowText(p.MainWindowHandle, Name & " - Knighted (Instance #" & num.ToString & ")")
				End If
			Else
				Process.Start(psi)
			End If
		End Sub

		Public Overrides Sub ShowOptions()
			Dim f As New fJediKnightOptions(Me)
			f.ShowDialog()
		End Sub

#Region "Properties"
		Public Overrides Property ActiveModPath() As String
			Get
				Dim path As String = MyBase.ActiveModPath

				If path IsNot Nothing AndAlso Not IsValidActiveModPath(path) Then
					Return Nothing
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If value IsNot Nothing AndAlso Not IsValidActiveModPath(value) Then
					Throw New InvalidDataException(
												"Active mod path must be a subdirectory of the game path " &
												"that does not contain spaces, and must be different from " &
												"the inactive mod path.")
				End If

				MyBase.ActiveModPath = value
			End Set
		End Property

		Public Overrides ReadOnly Property AllowMultipleActiveMods() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property AutoActiveModPath() As String
			Get
				Return Path.Combine(GamePath, "jkpatch\active")
			End Get
		End Property

		Public Overrides ReadOnly Property AutoGamePath() As String
			Get
				Dim path As String = ReadLECPath()
				If path IsNot Nothing Then
					Return path
				End If

				Return ReadSteamPath()
			End Get
		End Property

		Public Overrides ReadOnly Property AutoModPath() As String
			Get
				Return Path.Combine(GamePath, "jkpatch")
			End Get
		End Property

		Public Overrides ReadOnly Property AutoPatchPath() As String
			Get
				Return Path.Combine(GamePath, "patches")
			End Get
		End Property

		Public Overrides ReadOnly Property HasSeparateMultiplayerBinary() As Boolean
			Get
				Return False
			End Get
		End Property

		Public Overrides ReadOnly Property Image16() As System.Drawing.Image
			Get
				Return My.Resources.game_jk_16
			End Get
		End Property

		Public Overrides ReadOnly Property Image32() As System.Drawing.Image
			Get
				Return My.Resources.game_jk_32
			End Get
		End Property

		Public Overrides Property GamePath() As String
			Get
				Return MyBase.GamePath
			End Get
			Set(ByVal value As String)
				MyBase.GamePath = value

				If MyOptions(Me.GetType.Name & "ActiveModPath") IsNot Nothing _
										AndAlso Not IsValidActiveModPath(GetActiveModPath) Then

					ActiveModPath = Nothing
				End If
			End Set
		End Property

		Public Overrides ReadOnly Property Name() As String
			Get
				Return "Jedi Knight"
			End Get
		End Property

		Public Overrides ReadOnly Property SupportsPatches() As Boolean
			Get
				Return True
			End Get
		End Property
#End Region
	End Class

	Public Class MysteriesOfTheSith
		Inherits Game

		Public Function BuildArguments(ByVal useMods As Boolean) As String
			Dim args As String = ""
			If useMods Then
				Dim gp As String = Path.GetFullPath(GamePath)
				Dim ap As String = Path.GetFullPath(GetActiveModPath)
				If ap.ToLower.IndexOf(gp.ToLower) = 0 Then
					Dim segments As Integer = ap.Substring(gp.Length).Trim(Path.
												DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).Length
					Dim shorts As String() = ap.Split(Path.DirectorySeparatorChar)

					args = "-path " & String.Join(Path.DirectorySeparatorChar, shorts,
												shorts.Length - segments, segments) & " "
				Else
					MsgBox("Invalid active mod path, mods will not be applied.  " &
												"Please set the active mod path in the game options to a " &
												"subdirectory of the game directory without spaces.",
												MsgBoxStyle.Exclamation, "Error - Knight")
				End If
			End If

			If MyOptions.MysteriesOfTheSithAdvancedDisplayOptions Then
				args &= "-displayconfig "
			End If

			If MyOptions.MysteriesOfTheSithCogLog Then
				args &= "-coglog "
			End If

			If MyOptions.MysteriesOfTheSithConsole Then
				args &= "-devmods "
			End If

			If MyOptions.MysteriesOfTheSithConsoleStats Then
				args &= "-dispstats "
			End If

			If MyOptions.MysteriesOfTheSithFramerate Then
				args &= "-framerate "
			End If

			Select Case MyOptions.MysteriesOfTheSithLog
				Case Options.LogTypes.File
					args &= "-debug log "
				Case Options.LogTypes.WindowsConsole
					args &= "-debug con "
			End Select

			If MyOptions.MysteriesOfTheSithNoHUD Then
				args &= "-nohud "
			End If

			If MyOptions.MysteriesOfTheSithRecord Then
				args &= "-record "
			End If

			If MyOptions.MysteriesOfTheSithResLog Then
				args &= "-fail "
			End If

			If MyOptions.MysteriesOfTheSithSpeedUp Then
				args &= "-fixed "
			End If

			If MyOptions.MysteriesOfTheSithVerbosity > -1 Then
				args &= "-verbose " & MyOptions.JediKnightVerbosity.ToString & " "
			End If

			If MyOptions.MysteriesOfTheSithWindowedGUI Then
				args &= "-windowgui "
			End If

			Return args.TrimEnd
		End Function

		Private Function GetUniquePatchName(ByVal base As String) As String
			If Not File.Exists(Path.Combine(GetPatchPath, base & ".exe")) Then
				Return base & ".exe"
			End If

			Dim i As Integer = 0
			While File.Exists(Path.Combine(GetPatchPath, base & "." & i.ToString &
								".exe"))

				i += 1
			End While
			Return base & "." & i.ToString & ".exe"
		End Function

		Public Overrides Function InstallPatch(ByVal filename As String, ByVal h As _
						HashList.Hash) As Games.Game.Patch

			Dim gp As New DirectoryInfo(GetPatchPath)
			Dim patch As New FileInfo(filename)

			Dim finalname As String = Path.GetFileName(filename)
			If gp.FullName <> patch.DirectoryName Then
				finalname = GetUniquePatchName(Path.GetFileNameWithoutExtension(
										h.FileName))
				File.Copy(filename, Path.Combine(GetPatchPath, finalname))
			End If

			MyHashes(h.UniqueID) = h

			Dim p As New SithPatch(Me, h, Path.Combine(GetPatchPath, finalname), Path.
								Combine(GamePath, "jkm.exe"), False)
			_patches.Add(p)
			Return p
		End Function

		Public Function IsValidActiveModPath(ByVal folder As String) As Boolean
			Dim full As String = Path.GetFullPath(folder).TrimEnd(Path.
								DirectorySeparatorChar).ToLower
			Dim fullmod As String = Path.GetFullPath(GetModPath).TrimEnd(Path.
								DirectorySeparatorChar).ToLower
			Dim fullgame As String = Path.GetFullPath(GamePath).TrimEnd(Path.
								DirectorySeparatorChar).ToLower

			If full.IndexOf(fullgame) <> 0 OrElse full = fullgame OrElse full =
								fullmod Then

				Return False
			End If
			Return Not full.Substring(fullgame.Length).Contains(" "c)
		End Function

		Public Overrides Function IsValidGamePath(ByVal folder As String) As Boolean
			Dim fi As New FileInfo(Path.Combine(folder, "jkm.exe"))
			Return fi.Exists
		End Function

		Private Function ReadLECPath() As String
			Dim rk As RegistryKey = Registry.LocalMachine.OpenSubKey(
								"SOFTWARE\LucasArts Entertainment Company LLC\" &
								"Mysteries of the Sith\v1.0", False)
			If rk Is Nothing Then
				Return Nothing
			End If

			Dim installpath As String = rk.GetValue("Install Path", Nothing)
			rk.Close()
			If installpath Is Nothing Then
				Return Nothing
			End If

			If IsValidGamePath(installpath) Then
				Return installpath
			End If

			Return Nothing
		End Function

		Private Function ReadSteamPath() As String
			If Not SteamHelper.IsAppInstalled(SteamHelper.AppIDs.MysteriesOfTheSith) _
								Then

				Return Nothing
			End If

			Dim steampath As String = SteamHelper.GetSteamPath
			If steampath Is Nothing Then
				Return Nothing
			End If

			Dim gamepath As String = Path.Combine(steampath,
								"SteamApps\common\jedi knight mysteries of the sith")
			If IsValidGamePath(gamepath) Then
				Return gamepath
			End If

			Return Nothing
		End Function

		Private Sub RefreshActiveMods()
			Dim mpath As New DirectoryInfo(GetActiveModPath)
			If Not mpath.Exists Then
				Return
			End If

			Dim m As Modification = Nothing
			For Each gob As FileInfo In mpath.GetFiles("*.goo")
				Dim uid As String = Path.GetFileNameWithoutExtension(gob.Name)
				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New SithMod(Me, uid, True)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next gob
		End Sub

		Private Sub RefreshInactiveMods()
			Dim mpath As New DirectoryInfo(GetModPath)
			If Not mpath.Exists Then
				Return
			End If

			Dim m As Modification = Nothing
			For Each gob As FileInfo In mpath.GetFiles("*.goo")
				Dim uid As String = Path.GetFileNameWithoutExtension(gob.Name)
				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New SithMod(Me, uid, True)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next gob

			For Each zip As FileInfo In mpath.GetFiles("*.zip")
				Dim z As New ZipFile(zip.FullName)
				For Each ze As ZipEntry In z
					If Not ze.Name.ToLower.EndsWith(".goo") Then
						Continue For
					End If

					Dim uid As String = Path.GetFileNameWithoutExtension(ze.Name)
					If AlreadyFoundMod(uid) Then
						Continue For
					End If

					SyncLock Threading.Thread.CurrentThread
						m = New SithModZipped(Me, uid, zip.FullName, True)
						_mods.Add(m)
					End SyncLock
					RaiseModFoundEvent(m)
				Next ze
				z.Close()
			Next zip
		End Sub

		Protected Overrides Sub RefreshModsInternal()
			RefreshInactiveMods()
			RefreshActiveMods()
			RaiseEndModSearchEvent()
		End Sub

		Private Sub RefreshGamePathPatches()
			Dim mpath As New DirectoryInfo(GetPatchPath)

			Dim p As Patch = Nothing
			Dim jk As String = Path.Combine(GamePath, "jkm.exe")
			Dim jkh As HashList.Hash = Nothing
			If File.Exists(jk) Then
				jkh = MyHashes.IdentifyFile(jk)
			End If

			Dim foundactive As Boolean = False
			For Each exe As FileInfo In mpath.GetFiles("*.exe")
				If exe.Name.ToLower = "jkm.exe" Then
					Continue For
				End If

				Dim h As HashList.Hash = MyHashes.IdentifyFile(exe.FullName)
				If h Is Nothing OrElse AlreadyFoundPatch(h.UniqueID) Then
					Continue For
				End If

				Dim active As Boolean = jkh IsNot Nothing AndAlso jkh Is h
				If active Then
					foundactive = True
				End If
				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, h, exe.FullName, jk, active)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			Next exe

			If jkh IsNot Nothing AndAlso Not foundactive Then
				Dim dest As String = Path.Combine(GamePath, GetUniquePatchName(
										Path.GetFileNameWithoutExtension(jkh.FileName)))
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, jkh, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			ElseIf Not foundactive AndAlso File.Exists(jk) AndAlso jkh Is Nothing Then
				Dim h As HashList.Hash = MyHashes.HashFile("Unknown Patch (jkm.exe)", jk)
				h.UniqueID = h.ValueString
				h.FileName = GetUniquePatchName("jkm.unknown.patch")
				MyHashes(h.UniqueID) = h
				Dim dest As String = Path.Combine(GamePath, h.FileName)
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, h, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			End If
		End Sub

		Protected Overrides Sub RefreshPatchesInternal()
			Dim mpath As New DirectoryInfo(GetPatchPath)
			If Not mpath.Exists Then
				Return
			End If

			Dim gpath As New DirectoryInfo(GamePath)
			If mpath.FullName = gpath.FullName Then
				RefreshGamePathPatches()
				MyBase.RefreshPatchesInternal()
				Return
			End If

			Dim p As Patch = Nothing
			Dim jk As String = Path.Combine(GamePath, "jkm.exe")
			Dim jkh As HashList.Hash = Nothing
			If File.Exists(jk) Then
				jkh = MyHashes.IdentifyFile(jk)
			End If

			Dim foundactive As Boolean = False
			For Each exe As FileInfo In mpath.GetFiles("*.exe")
				Dim h As HashList.Hash = MyHashes.HashFile("Unknown Patch (" & exe.Name _
										& ")", exe.FullName)
				Dim ih As HashList.Hash = MyHashes.IdentifyFile(h.DataHash, h.DataSize)
				If ih IsNot Nothing AndAlso AlreadyFoundPatch(ih.UniqueID) Then
					Continue For
				End If

				If ih Is Nothing Then
					ih = h
					ih.UniqueID = ih.ValueString
					MyHashes(h.UniqueID) = h
				End If

				Dim active As Boolean = jkh IsNot Nothing AndAlso jkh Is ih
				If active Then
					foundactive = True
				End If
				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, ih, exe.FullName, jk, active)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			Next exe

			If jkh IsNot Nothing AndAlso Not foundactive Then
				Dim dest As String = Path.Combine(GetPatchPath, GetUniquePatchName(
										Path.GetFileNameWithoutExtension(jkh.FileName)))
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, jkh, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			ElseIf Not foundactive AndAlso File.Exists(jk) AndAlso jkh Is Nothing Then
				Dim h As HashList.Hash = MyHashes.HashFile("Unknown Patch (jkm.exe)", jk)
				h.UniqueID = h.ValueString
				h.FileName = GetUniquePatchName("jkm.unknown.patch")
				MyHashes(h.UniqueID) = h
				Dim dest As String = Path.Combine(GetPatchPath, h.FileName)
				File.Copy(jk, dest)

				SyncLock Threading.Thread.CurrentThread
					p = New SithPatch(Me, h, dest, jk, True)
					_patches.Add(p)
				End SyncLock
				RaisePatchFoundEvent(p)
			End If

			MyBase.RefreshPatchesInternal()
		End Sub

		Public Overrides Sub Run()
			Dim fi As New FileInfo(Path.Combine(GamePath, "jkm.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Mysteries of the Sith!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(False)

			If MyOptions.MysteriesOfTheSithAllowMultipleInstances Then
				Dim num As Integer = Process.GetProcessesByName("jkm").Length + 1

				Dim p As Process = Process.Start(psi)
				p.WaitForInputIdle(5000)
				If Not p.MainWindowHandle.Equals(IntPtr.Zero) Then
					WinAPI.SetWindowText(p.MainWindowHandle, Name & " - Knighted (Instance #" & num.ToString & ")")
				End If
			Else
				Process.Start(psi)
			End If
		End Sub

		Public Overrides Sub Run(ByVal mods() As Game.Modification)
			Dim fi As New FileInfo(Path.Combine(GamePath, "jkm.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Mysteries of the Sith!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(True)

			If MyOptions.MysteriesOfTheSithAllowMultipleInstances Then
				Dim num As Integer = Process.GetProcessesByName("jkm").Length + 1

				Dim p As Process = Process.Start(psi)
				p.WaitForInputIdle(5000)
				If Not p.MainWindowHandle.Equals(IntPtr.Zero) Then
					WinAPI.SetWindowText(p.MainWindowHandle, Name & " - Knighted (Instance #" & num.ToString & ")")
				End If
			Else
				Process.Start(psi)
			End If
		End Sub

		Public Overrides Sub ShowOptions()
			Dim f As New fMysteriesOfTheSithOptions(Me)
			f.ShowDialog()
		End Sub

#Region "Properties"
		Public Overrides Property ActiveModPath() As String
			Get
				Dim path As String = MyBase.ActiveModPath

				If path IsNot Nothing AndAlso Not IsValidActiveModPath(path) Then
					Return Nothing
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If value IsNot Nothing AndAlso Not IsValidActiveModPath(value) Then
					Throw New InvalidDataException(
												"Active mod path must be a subdirectory of the game path " &
												"that does not contain spaces, and must be different from " &
												"the inactive mod path.")
				End If

				MyBase.ActiveModPath = value
			End Set
		End Property

		Public Overrides ReadOnly Property AllowMultipleActiveMods() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property AutoActiveModPath() As String
			Get
				Return Path.Combine(GamePath, "jkpatch\active")
			End Get
		End Property

		Public Overrides ReadOnly Property AutoGamePath() As String
			Get
				Dim path As String = ReadLECPath()
				If path IsNot Nothing Then
					Return path
				End If

				Return ReadSteamPath()
			End Get
		End Property

		Public Overrides ReadOnly Property AutoModPath() As String
			Get
				Return Path.Combine(GamePath, "jkpatch")
			End Get
		End Property

		Public Overrides ReadOnly Property AutoPatchPath() As String
			Get
				Return Path.Combine(GamePath, "patches")
			End Get
		End Property

		Public Overrides Property GamePath() As String
			Get
				Return MyBase.GamePath
			End Get
			Set(ByVal value As String)
				MyBase.GamePath = value

				If MyOptions(Me.GetType.Name & "ActiveModPath") IsNot Nothing AndAlso
										Not IsValidActiveModPath(GetActiveModPath) Then

					ActiveModPath = Nothing
				End If
			End Set
		End Property

		Public Overrides ReadOnly Property HasSeparateMultiplayerBinary() As Boolean
			Get
				Return False
			End Get
		End Property

		Public Overrides ReadOnly Property Image16() As System.Drawing.Image
			Get
				Return My.Resources.game_mots_16
			End Get
		End Property

		Public Overrides ReadOnly Property Image32() As System.Drawing.Image
			Get
				Return My.Resources.game_mots_32
			End Get
		End Property

		Public Overrides ReadOnly Property Name() As String
			Get
				Return "Mysteries of the Sith"
			End Get
		End Property

		Public Overrides ReadOnly Property SupportsPatches() As Boolean
			Get
				Return True
			End Get
		End Property
#End Region
	End Class

	Public Class SithMod
		Inherits Game.Modification

		Public Sub New(ByVal owner As Game, ByVal uid As String, ByVal mots As Boolean)
			MyBase.New(owner)
			_uid = uid
			If mots Then
				_ext = "goo"
			End If
		End Sub
		Private _ext As String = "gob"

		Private Sub Activate()
			Dim fi As New FileInfo(Path.Combine(_owner.GetModPath, _uid & "." & _ext))
			If Not fi.Exists Then
				Throw New FileNotFoundException(fi.FullName &
										" not found when trying to activate.")
			End If

			If Not Directory.Exists(_owner.GetActiveModPath) Then
				Directory.CreateDirectory(_owner.GetActiveModPath)
			End If

			fi.MoveTo(Path.Combine(_owner.GetActiveModPath, _uid & "." & _ext))
		End Sub

		Private Sub Deactivate()
			Dim fi As New FileInfo(Path.Combine(_owner.GetActiveModPath, _uid & "." &
								_ext))
			If Not fi.Exists Then
				Throw New FileNotFoundException(fi.FullName &
										" not found when trying to deactivate.")
			End If

			fi.MoveTo(Path.Combine(_owner.GetModPath, _uid & "." & _ext))
		End Sub

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			Dim file As String
			If Active Then
				file = Path.Combine(_owner.GetActiveModPath, _uid & "." & _ext)
			Else
				file = Path.Combine(_owner.GetModPath, _uid & "." & _ext)
			End If
			Dim fi As New FileInfo(file)
			If Not fi.Exists Then
				Return Nothing
			End If

			Dim fs As FileStream = fi.Open(FileMode.Open, FileAccess.Read)
			Dim g As New Gob(fs)
			Dim pi As Gob.PatchInfo = g.GetModInfo()
			fs.Close()
			If pi.Name Is Nothing OrElse pi.Name = "" Then
				Return Nothing
			End If
			Return New ModInfoCache.Modification(pi.Name)
		End Function

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return File.Exists(Path.Combine(_owner.GetActiveModPath, UniqueID &
										"." & _ext))
			End Get
			Set(ByVal value As Boolean)
				If Active = value Then
					Return
				End If

				If value Then
					Activate()
				Else
					Deactivate()
				End If
			End Set
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _uid
			End Get
		End Property
		Private _uid As String
#End Region
	End Class

	Public Class SithModZipped
		Inherits Game.Modification

		Public Sub New(ByVal owner As Game, ByVal uid As String, ByVal zipname As _
						String, ByVal mots As Boolean)

			MyBase.New(owner)
			_uid = uid
			_zipname = zipname
			If mots Then
				_ext = "goo"
			End If
		End Sub
		Private _zipname As String
		Private _ext As String = "gob"

		Private Sub Activate()
			Dim fi As New FileInfo(Path.Combine(_owner.GetModPath, _zipname))
			If Not fi.Exists Then
				Throw New FileNotFoundException(fi.FullName &
										" not found when trying to activate.")
			End If

			If Not Directory.Exists(_owner.GetActiveModPath) Then
				Directory.CreateDirectory(_owner.GetActiveModPath)
			End If

			Dim z As New ZipFile(fi.FullName)
			Dim ze As ZipEntry = z.GetEntry(_uid & "." & _ext)
			If ze Is Nothing Then
				Throw New FileNotFoundException(_uid & "." & _ext &
										" not found when trying to activate.")
			End If

			Dim dest As New FileInfo(Path.Combine(_owner.GetActiveModPath, _uid & "." &
								_ext))
			If dest.Exists Then
				dest.Delete()
			End If

			Dim s As Stream = z.GetInputStream(ze)
			Dim fs As FileStream = dest.Open(FileMode.Create, FileAccess.Write)
			Dim read As Integer = 1
			Dim b(524287) As Byte
			While read > 0
				read = s.Read(b, 0, b.Length - 1)
				If read > 0 Then
					fs.Write(b, 0, read)
				End If
			End While
			fs.Close()
			fs.Dispose()
			s.Close()
			s.Dispose()
			z.Close()
		End Sub

		Private Sub Deactivate()
			Dim fi As New FileInfo(Path.Combine(_owner.GetActiveModPath, _uid & "." &
								_ext))
			If fi.Exists Then
				fi.Delete()
			End If
		End Sub

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			If Not Active Then
				Return RefreshZippedInfo()
			End If

			Dim fi As New FileInfo(Path.Combine(_owner.GetActiveModPath, _uid & "." &
								_ext))
			If Not fi.Exists Then
				Return RefreshZippedInfo()
			End If

			Dim fs As FileStream = fi.Open(FileMode.Open, FileAccess.Read)
			Dim g As New Gob(fs)
			Dim pi As Gob.PatchInfo = g.GetModInfo()
			fs.Close()
			If pi.Name Is Nothing OrElse pi.Name = "" Then
				Return Nothing
			End If
			Return New ModInfoCache.Modification(pi.Name)
		End Function

		Private Function RefreshZippedInfo() As ModInfoCache.Modification
			Dim z As New ZipFile(Path.Combine(_owner.GetModPath, _zipname))
			Dim index As Integer = z.FindEntry(_uid & "." & _ext, True)
			If index < 0 Then
				Return Nothing
			End If

			Dim s As Stream
			Try
				s = z.GetInputStream(z(index))
			Catch ex As Exception
				Return Nothing
			End Try

			Dim g As New Gob(s)
			Dim pi As Gob.PatchInfo = g.GetModInfo()
			s.Close()
			s.Dispose()
			z.Close()

			If pi.Name Is Nothing OrElse pi.Name = "" Then
				Return Nothing
			End If
			Return New ModInfoCache.Modification(pi.Name)
		End Function

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return File.Exists(Path.Combine(_owner.GetActiveModPath, UniqueID &
										".GOB"))
			End Get
			Set(ByVal value As Boolean)
				If Active = value Then
					Return
				End If

				If value Then
					Activate()
				Else
					Deactivate()
				End If
			End Set
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _uid
			End Get
		End Property
		Private _uid As String
#End Region
	End Class

	Public Class SithPatch
		Inherits Game.Patch

		Public Sub New(ByVal owner As Game, ByVal h As HashList.Hash, ByVal filename _
						As String, ByVal gameexe As String, ByVal active As Boolean)

			MyBase.New(owner)
			_h = h
			_filename = filename
			_gameexe = gameexe
			_active = active
		End Sub
		Private _gameexe As String

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return _active
			End Get
			Set(ByVal value As Boolean)
				If _active = value Then
					Return
				End If
				_active = value

				If File.Exists(_gameexe) Then
					If Not File.Exists(_filename) Then
						File.Move(_gameexe, _filename)
					Else
						File.Delete(_gameexe)
					End If
				End If

				If value Then
					File.Copy(_filename, _gameexe)
				End If
			End Set
		End Property
		Private _active As Boolean

		Public Overrides ReadOnly Property FileName() As String
			Get
				Return _filename
			End Get
		End Property
		Private _filename As String

		Public ReadOnly Property Hash() As HashList.Hash
			Get
				Return _h
			End Get
		End Property
		Private _h As HashList.Hash

		Public Overrides ReadOnly Property Name() As String
			Get
				Return _h.Name
			End Get
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _h.UniqueID
			End Get
		End Property
#End Region
	End Class
#End Region

#Region "Quake 3 Engine"
	Public Class JediOutcast
		Inherits Game

		Private Function BuildArguments(ByVal moduid As String) As String
			Dim args As String = ""
			If moduid IsNot Nothing Then
				args = "+set fs_game """ & moduid & """ "
			End If

			If MyOptions.JediOutcastCommands IsNot Nothing Then
				args &= "+" & MyOptions.JediOutcastCommands & " "
			End If

			Return args.TrimEnd
		End Function

		Public Overrides Function IsValidGamePath(ByVal folder As String) As Boolean
			Dim fi As New FileInfo(Path.Combine(folder, "GameData\jk2sp.exe"))
			Return fi.Exists
		End Function

		Private Function ReadLECPath() As String
			Dim rk As RegistryKey = Registry.LocalMachine.OpenSubKey(
								"SOFTWARE\LucasArts Entertainment Company LLC\" &
								"Star Wars JK II Jedi Outcast\1.0", False)
			If rk Is Nothing Then
				Return Nothing
			End If

			Dim installpath As String = rk.GetValue("InstallPath", Nothing)
			rk.Close()
			If installpath Is Nothing Then
				Return Nothing
			End If

			If IsValidGamePath(installpath) Then
				Return installpath
			End If

			Return Nothing
		End Function

		Private Function ReadSteamPath() As String
			If Not SteamHelper.IsAppInstalled(SteamHelper.AppIDs.JediOutcast) _
								Then

				Return Nothing
			End If

			Dim steampath As String = SteamHelper.GetSteamPath
			If steampath Is Nothing Then
				Return Nothing
			End If

			Dim gamepath As String = Path.Combine(steampath,
								"SteamApps\common\jedi outcast")
			If IsValidGamePath(gamepath) Then
				Return gamepath
			End If

			Return Nothing
		End Function

		Protected Overrides Sub RefreshModsInternal()
			Dim mpath As New DirectoryInfo(GetModPath)
			If Not mpath.Exists Then
				RaiseEndModSearchEvent()
				Return
			End If

			Dim m As Modification = Nothing
			For Each zip As FileInfo In mpath.GetFiles("*.zip")
				Dim uid As String = Path.GetFileNameWithoutExtension(zip.Name)
				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				Dim z As New ZipFile(zip.FullName)
				Dim ismod As Boolean = False
				For Each ze As ZipEntry In z
					If ze.Name.ToLower.EndsWith(".pk3") OrElse ze.Name.ToLower.
												EndsWith(".dll") Then

						ismod = True
						Exit For
					End If
				Next ze
				z.Close()

				If Not ismod Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New Quake3ModZipped(Me, uid)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next zip

			For Each dir As DirectoryInfo In mpath.GetDirectories()
				If dir.Name.ToLower = "base" Then
					Continue For
				End If

				If AlreadyFoundMod(dir.Name) Then
					Continue For
				End If

				If dir.GetFiles("*.pk3").Length < 1 AndAlso dir.GetFiles("*.dll") _
										.Length < 1 Then

					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New Quake3Mod(Me, dir.Name)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next dir

			RaiseEndModSearchEvent()
		End Sub

		Public Overrides Sub Run()
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jk2sp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Outcast Single Player!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(Nothing)
			Process.Start(psi)
		End Sub

		Public Overrides Sub Run(ByVal mods() As Game.Modification)
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jk2sp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Outcast Single Player!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(mods(0).UniqueID)
			Process.Start(psi)
		End Sub

		Public Overrides Sub RunMultiplayer()
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jk2mp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Outcast Multiplayer!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(Nothing)
			Process.Start(psi)
		End Sub

		Public Overrides Sub RunMultiplayer(ByVal mods() As Game.Modification)
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jk2mp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Outcast Multiplayer!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(mods(0).UniqueID)
			Process.Start(psi)
		End Sub

		Public Overrides Function ShoundRunMultiplayer(ByVal mods() As Game.
						Modification) As Boolean

			Dim m As ModInfoCache.Modification = MyModInfo(Me.Name, mods(0).UniqueID)
			If m Is Nothing OrElse Not TypeOf m Is Quake3ModInfo Then
				Return True
			End If

			Dim q As Quake3ModInfo = m
			Return Not q.SinglePlayer
		End Function

		Public Overrides Sub ShowOptions()
			Dim f As New fJOJAOptions(Me)
			f.ShowDialog()
		End Sub

#Region "Properties"
		Public Overrides Property ActiveModPath() As String
			Get
				Return MyBase.ActiveModPath
			End Get
			Set(ByVal value As String)
				Throw New NotSupportedException(
										"Can't set ActiveModPath on JediOutcast!")
			End Set
		End Property

		Public Overrides ReadOnly Property AllowMultipleActiveMods() As Boolean
			Get
				Return False
			End Get
		End Property

		Public Overrides ReadOnly Property AutoActiveModPath() As String
			Get
				Return Path.Combine(GamePath, "GameData")
			End Get
		End Property

		Public Overrides ReadOnly Property AutoGamePath() As String
			Get
				Dim path As String = ReadLECPath()
				If path IsNot Nothing Then
					Return path
				End If

				Return ReadSteamPath()
			End Get
		End Property

		Public Overrides ReadOnly Property AutoModPath() As String
			Get
				Return Path.Combine(GamePath, "GameData")
			End Get
		End Property

		Public Overrides ReadOnly Property HasSeparateMultiplayerBinary() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property Image16() As System.Drawing.Image
			Get
				Return My.Resources.game_jo_16
			End Get
		End Property

		Public Overrides ReadOnly Property Image32() As System.Drawing.Image
			Get
				Return My.Resources.game_jo_32
			End Get
		End Property

		Public Overrides Property ModPath() As String
			Get
				Return MyBase.ModPath
			End Get
			Set(ByVal value As String)
				Throw New NotSupportedException("Can't set ModPath on JediOutcast!")
			End Set
		End Property

		Public Overrides ReadOnly Property Name() As String
			Get
				Return "Jedi Outcast"
			End Get
		End Property

		Public Overrides ReadOnly Property SupportsPatches() As Boolean
			Get
				Return False
			End Get
		End Property
#End Region
	End Class

	Public Class JediAcademy
		Inherits Game

		Private Function BuildArguments(ByVal moduid As String) As String
			Dim args As String = ""
			If moduid IsNot Nothing Then
				args = "+set fs_game """ & moduid & """ "
			End If

			If MyOptions.JediAcademyCommands IsNot Nothing Then
				args &= "+" & MyOptions.JediAcademyCommands & " "
			End If

			Return args.TrimEnd
		End Function

		Public Overrides Function IsValidGamePath(ByVal folder As String) As Boolean
			Dim fi As New FileInfo(Path.Combine(folder, "GameData\jasp.exe"))
			Return fi.Exists
		End Function

		Private Function ReadLECPath() As String
			Dim rk As RegistryKey = Registry.LocalMachine.OpenSubKey(
								"SOFTWARE\LucasArts\Star Wars Jedi Knight Jedi Academy\1.0", False)
			If rk Is Nothing Then
				Return Nothing
			End If

			Dim installpath As String = rk.GetValue("InstallPath", Nothing)
			rk.Close()
			If installpath Is Nothing Then
				Return Nothing
			End If

			If IsValidGamePath(installpath) Then
				Return installpath
			End If

			Return Nothing
		End Function

		Private Function ReadSteamPath() As String
			If Not SteamHelper.IsAppInstalled(SteamHelper.AppIDs.JediAcademy) _
								Then

				Return Nothing
			End If

			Dim steampath As String = SteamHelper.GetSteamPath
			If steampath Is Nothing Then
				Return Nothing
			End If

			Dim gamepath As String = Path.Combine(steampath,
								"SteamApps\common\jedi academy")
			If IsValidGamePath(gamepath) Then
				Return gamepath
			End If

			Return Nothing
		End Function

		Protected Overrides Sub RefreshModsInternal()
			Dim mpath As New DirectoryInfo(GetModPath)
			If Not mpath.Exists Then
				RaiseEndModSearchEvent()
				Return
			End If

			Dim m As Modification = Nothing
			For Each zip As FileInfo In mpath.GetFiles("*.zip")
				Dim uid As String = Path.GetFileNameWithoutExtension(zip.Name)
				If AlreadyFoundMod(uid) Then
					Continue For
				End If

				Dim z As New ZipFile(zip.FullName)
				Dim ismod As Boolean = False
				For Each ze As ZipEntry In z
					If ze.Name.ToLower.EndsWith(".pk3") OrElse ze.Name.ToLower.
												EndsWith(".dll") Then

						ismod = True
						Exit For
					End If
				Next ze
				z.Close()

				If Not ismod Then
					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New Quake3ModZipped(Me, uid)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next zip

			For Each dir As DirectoryInfo In mpath.GetDirectories()
				If dir.Name.ToLower = "base" Then
					Continue For
				End If

				If AlreadyFoundMod(dir.Name) Then
					Continue For
				End If

				If dir.GetFiles("*.pk3").Length < 1 AndAlso dir.GetFiles("*.dll") _
										.Length < 1 Then

					Continue For
				End If

				SyncLock Threading.Thread.CurrentThread
					m = New Quake3Mod(Me, dir.Name)
					_mods.Add(m)
				End SyncLock
				RaiseModFoundEvent(m)
			Next dir

			RaiseEndModSearchEvent()
		End Sub

		Public Overrides Sub Run()
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jasp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Academy Single Player!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(Nothing)
			Process.Start(psi)
		End Sub

		Public Overrides Sub Run(ByVal mods() As Game.Modification)
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jasp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Academy Single Player!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(mods(0).UniqueID)
			Process.Start(psi)
		End Sub

		Public Overrides Sub RunMultiplayer()
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jamp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Academy Multiplayer!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(Nothing)
			Process.Start(psi)
		End Sub

		Public Overrides Sub RunMultiplayer(ByVal mods() As Game.Modification)
			Dim fi As New FileInfo(Path.Combine(GamePath, "GameData\jamp.exe"))
			If Not fi.Exists Then
				MsgBox("Unable to locate Jedi Academy Multiplayer!", MsgBoxStyle.
										Exclamation, "Error - Knight")
				Return
			End If

			Dim psi As New ProcessStartInfo()
			psi.FileName = fi.FullName
			psi.WorkingDirectory = fi.DirectoryName
			psi.Arguments = BuildArguments(mods(0).UniqueID)
			Process.Start(psi)
		End Sub

		Public Overrides Function ShoundRunMultiplayer(ByVal mods() As Game.
						Modification) As Boolean

			Dim m As ModInfoCache.Modification = MyModInfo(Me.Name, mods(0).UniqueID)
			If m Is Nothing OrElse Not TypeOf m Is Quake3ModInfo Then
				Return True
			End If

			Dim q As Quake3ModInfo = m
			Return Not q.SinglePlayer
		End Function

		Public Overrides Sub ShowOptions()
			Dim f As New fJOJAOptions(Me)
			f.ShowDialog()
		End Sub

#Region "Properties"
		Public Overrides Property ActiveModPath() As String
			Get
				Return MyBase.ActiveModPath
			End Get
			Set(ByVal value As String)
				Throw New NotSupportedException(
										"Can't set ActiveModPath on JediAcademy!")
			End Set
		End Property

		Public Overrides ReadOnly Property AllowMultipleActiveMods() As Boolean
			Get
				Return False
			End Get
		End Property

		Public Overrides ReadOnly Property AutoActiveModPath() As String
			Get
				Return Path.Combine(GamePath, "GameData")
			End Get
		End Property

		Public Overrides ReadOnly Property AutoGamePath() As String
			Get
				Dim path As String = ReadLECPath()
				If path IsNot Nothing Then
					Return path
				End If

				Return ReadSteamPath()
			End Get
		End Property

		Public Overrides ReadOnly Property AutoModPath() As String
			Get
				Return Path.Combine(GamePath, "GameData")
			End Get
		End Property

		Public Overrides ReadOnly Property HasSeparateMultiplayerBinary() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property Image16() As System.Drawing.Image
			Get
				Return My.Resources.game_ja_16
			End Get
		End Property

		Public Overrides ReadOnly Property Image32() As System.Drawing.Image
			Get
				Return My.Resources.game_ja_32
			End Get
		End Property

		Public Overrides Property ModPath() As String
			Get
				Return MyBase.ModPath
			End Get
			Set(ByVal value As String)
				Throw New NotSupportedException("Can't set ModPath on JediAcademy!")
			End Set
		End Property

		Public Overrides ReadOnly Property Name() As String
			Get
				Return "Jedi Academy"
			End Get
		End Property

		Public Overrides ReadOnly Property SupportsPatches() As Boolean
			Get
				Return False
			End Get
		End Property
#End Region
	End Class

	Public Class Quake3Mod
		Inherits Game.Modification

		Public Sub New(ByVal owner As Game, ByVal uid As String)
			MyBase.New(owner)
			_uid = uid
		End Sub

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			Dim fi As New FileInfo(Path.Combine(Path.Combine(_owner.GetModPath, _uid),
								"description.txt"))
			If Not fi.Exists Then
				Return Nothing
			End If

			Dim fs As FileStream = fi.Open(FileMode.Open, FileAccess.Read)
			Dim sr As New StreamReader(fs)
			Dim name As String = Nothing
			If Not sr.EndOfStream Then
				name = sr.ReadLine().Trim
				Dim pos As Integer = 0
				While pos < name.Length - 1
					If name(pos) = "^"c Then
						If name(pos + 1) = "^"c Then
							name = name.Substring(0, pos) & name.Substring(pos + 1)
							pos += 1
						Else
							name = name.Substring(0, pos) & name.Substring(pos + 2)
						End If
					Else
						pos += 1
					End If
				End While
			End If
			sr.Close()
			sr.Dispose()
			fs.Close()
			fs.Dispose()

			If name Is Nothing OrElse name = "" Then
				Return Nothing
			End If
			Return New ModInfoCache.Modification(name)
		End Function

		Public Overrides Sub ShowOptions()
			Dim f As New fJOJAModOptions(_owner, Me)
			f.ShowDialog()
		End Sub

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return _active
			End Get
			Set(ByVal value As Boolean)
				_active = value
			End Set
		End Property
		Private _active As Boolean = False

		Public Overrides ReadOnly Property HasOptions() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _uid
			End Get
		End Property
		Private _uid As String
#End Region
	End Class

	Public Class Quake3ModZipped
		Inherits Game.Modification

		Public Sub New(ByVal owner As Game, ByVal uid As String)
			MyBase.New(owner)
			_uid = uid
		End Sub

		Private Sub Activate()
			Dim fi As New FileInfo(Path.Combine(_owner.GetModPath, _uid & ".zip"))
			If Not fi.Exists Then
				Throw New FileNotFoundException(fi.FullName &
										" not found when trying to activate.")
			End If

			Dim di As New DirectoryInfo(Path.Combine(_owner.GetActiveModPath, _uid))
			If Not di.Exists Then
				di.Create()
			End If

			Dim fz As New FastZip()
			fz.ExtractZip(fi.FullName, di.FullName, FastZip.Overwrite.Always,
				Nothing, "", "", True)
		End Sub

		Private Sub Deactivate()
			Dim di As New DirectoryInfo(Path.Combine(_owner.GetActiveModPath, _uid))
			If di.Exists Then
				di.Delete(True)
			End If
		End Sub

		Private Function ParseDescriptionTxt(ByVal s As Stream) As String
			Dim sr As New StreamReader(s)
			Dim name As String = Nothing
			If Not sr.EndOfStream Then
				name = sr.ReadLine().Trim
				Dim pos As Integer = 0
				While pos < name.Length - 1
					If name(pos) = "^"c Then
						If name(pos + 1) = "^"c Then
							name = name.Substring(0, pos) & name.Substring(pos + 1)
							pos += 1
						Else
							name = name.Substring(0, pos) & name.Substring(pos + 2)
						End If
					Else
						pos += 1
					End If
				End While
			End If
			sr.Close()
			sr.Dispose()

			Return name
		End Function

		Public Overrides Function RefreshInfo() As ModInfoCache.Modification
			If Active Then
				Return RefreshZippedInfo()
			End If

			Dim fi As New FileInfo(Path.Combine(Path.Combine(_owner.GetModPath, _uid),
								"description.txt"))
			If Not fi.Exists Then
				Return RefreshZippedInfo()
			End If

			Dim fs As FileStream = fi.Open(FileMode.Open, FileAccess.Read)
			Dim name As String = ParseDescriptionTxt(fs)
			fs.Close()
			fs.Dispose()

			If name Is Nothing OrElse name = "" Then
				Return Nothing
			End If
			Return New ModInfoCache.Modification(name)
		End Function

		Private Function RefreshZippedInfo() As ModInfoCache.Modification
			Dim z As New ZipFile(Path.Combine(_owner.GetModPath, _uid & ".zip"))
			Dim index As Integer = z.FindEntry("description.txt", True)
			If index < 0 Then
				Return Nothing
			End If

			Dim s As Stream
			Try
				s = z.GetInputStream(z(index))
			Catch ex As Exception
				Return Nothing
			End Try

			Dim name As String = ParseDescriptionTxt(s)
			s.Close()
			s.Dispose()
			z.Close()

			If name Is Nothing OrElse name = "" Then
				Return Nothing
			End If
			Return New ModInfoCache.Modification(name)
		End Function

		Public Overrides Sub ShowOptions()
			Dim f As New fJOJAModOptions(_owner, Me)
			f.ShowDialog()
		End Sub

#Region "Properties"
		Public Overrides Property Active() As Boolean
			Get
				Return Directory.Exists(Path.Combine(_owner.GetActiveModPath, UniqueID))
			End Get
			Set(ByVal value As Boolean)
				If Active = value Then
					Return
				End If

				If value Then
					Activate()
				Else
					Deactivate()
				End If
			End Set
		End Property

		Public Overrides ReadOnly Property HasOptions() As Boolean
			Get
				Return True
			End Get
		End Property

		Public Overrides ReadOnly Property UniqueID() As String
			Get
				Return _uid
			End Get
		End Property
		Private _uid As String
#End Region
	End Class
#End Region
End Namespace
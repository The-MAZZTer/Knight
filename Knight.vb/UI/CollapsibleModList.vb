Imports System.ComponentModel

Namespace MZZT.Knight
    Public Class CollapsibleModList
        Inherits Panel

        Public Sub New()
            Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            Height = gh.Height

            Controls.AddRange(New Control() {gh, ml})
            gh.SendToBack()

            fbdPath.ShowNewFolderButton = False

            With ofdImportPatch
                .AddExtension = True
                .AutoUpgradeEnabled = True
                .CheckFileExists = True
                .CheckPathExists = True
                .DefaultExt = "exe"
                .DereferenceLinks = True
                .Filter = "Applications (*.exe)|*.exe|All Files (*.*)|*.*"
                .FilterIndex = 0
                .Multiselect = True
                .RestoreDirectory = True
                .ShowHelp = False
                .ShowReadOnly = False
                .SupportMultiDottedExtensions = True
                .Title = "Import A Patched EXE - Knight"
                .ValidateNames = True
            End With
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()

            If _game IsNot Nothing Then
                RemoveHandler _game.ActiveModPathChanged, AddressOf _game_ModPathChanged
                RemoveHandler _game.BeginModSearch, AddressOf _game_BeginModSearch
                RemoveHandler _game.BeginPatchSearch, AddressOf _game_BeginPatchSearch
                RemoveHandler _game.EndModSearch, AddressOf _game_EndModSearch
                RemoveHandler _game.EndPatchSearch, AddressOf _game_EndPatchSearch
                RemoveHandler _game.GamePathChanged, AddressOf _game_GamePathChanged
                RemoveHandler _game.ModFound, AddressOf _game_ModFound
                RemoveHandler _game.ModPathChanged, AddressOf _game_ModPathChanged
                RemoveHandler _game.PatchFound, AddressOf _game_PatchFound
            End If
        End Sub

        Private Sub AddPatch(ByVal p As Games.Game.Patch)
            ml.BeginUpdate()
            ml.ec.Block()
            Dim lvi As New ListViewItem(p.Name)
            With lvi
                .Checked = p.Active
                .Group = ml.Groups("Patches")
                .ImageKey = ml.DefaultImageKey
                .Tag = p
            End With
            ml.Items.Add(lvi)
            ml.ec.Allow()

            If ml.Created Then
                ml.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If
            ml.EndUpdate()
        End Sub

        Private Function BuildPatchChangeLists() As Games.Game.Patch()()
            Dim activate As New List(Of Games.Game.Patch)
            Dim deactivate As New List(Of Games.Game.Patch)
            Dim finalactivate As Games.Game.Patch() = ml.GetActivatedPatches()

            For Each p As Games.Game.Patch In _game.Patches
                Dim active As Boolean = p.Active
                If active AndAlso Array.IndexOf(finalactivate, p) > -1 Then
                    Continue For
                End If

                If Not active AndAlso Array.IndexOf(finalactivate, p) < 0 Then
                    Continue For
                End If

                If active Then
                    deactivate.Add(p)
                Else
                    activate.Add(p)
                End If
            Next p

            Return New Games.Game.Patch()() {activate.ToArray, deactivate.ToArray, _
                finalactivate}
        End Function

        Public Sub RefreshMods()
            _game.RefreshMods()
        End Sub

        Public Sub RefreshPatches()
            If _game.SupportsPatches Then
                _game.RefreshPatches()
            End If
        End Sub

        Public Sub Run(ByVal multi As Boolean, ByVal mods As Games.Game.Modification())
            If multi Then
                If mods.Length = 0 Then
                    _game.RunMultiplayer()
                Else
                    _game.RunMultiplayer(mods)
                End If
            Else
                If mods.Length = 0 Then
                    _game.Run()
                Else
                    _game.Run(mods)
                End If
            End If

            Select Case MyOptions.OnRunAction
                Case Options.OnRunActions.Minimize
                    DirectCast(TopLevelControl, Form).WindowState = FormWindowState. _
                        Minimized
                Case Options.OnRunActions.Close
                    Application.Exit()
            End Select
        End Sub

        Public Sub UpdateModInfo(ByVal game As Games.Game, ByVal m As Games.Game. _
            Modification, ByVal modinfo As Games.ModInfoCache.Modification)

            ml.UpdateModInfo(game, m, modinfo)
        End Sub

#Region "Mod Activation/Deactivation"
        Private Sub _activate_DoWork(ByVal sender As Object, ByVal e As System. _
            ComponentModel.DoWorkEventArgs)

            Dim bw As BackgroundWorker = sender
            Dim activate As Games.Game.Modification() = e.Argument(0)
            Dim deactivate As Games.Game.Modification() = e.Argument(1)

            For i As Integer = 0 To deactivate.Length - 1
                bw.ReportProgress(i, "Deactivating " & deactivate(i).Name & "...")

                deactivate(i).Active = False

                If bw.CancellationPending Then
                    e.Cancel = True
                    Return
                End If
            Next i

            For i As Integer = 0 To activate.Length - 1
                bw.ReportProgress(deactivate.Length + i, "Activating " & _
                    activate(i).Name & "...")

                activate(i).Active = True

                If bw.CancellationPending Then
                    e.Cancel = True
                    Return
                End If
            Next i

            If _runAfterwards Then
                bw.ReportProgress(deactivate.Length + activate.Length, "Starting " & _
                    _game.Name & "...")
            Else
                bw.ReportProgress(deactivate.Length + activate.Length, Nothing)
            End If
        End Sub
        Private _activate As BackgroundWorker = Nothing
        Private _progress As fProgress = Nothing
        Private _runAfterwards As Boolean = True
        Private _runMultiplayer As Boolean = False
        Private _runMods As Games.Game.Modification()

        Private Function BuildChangeLists() As Games.Game.Modification()()
            Dim activate As New List(Of Games.Game.Modification)
            Dim deactivate As New List(Of Games.Game.Modification)
            Dim finalactivate As Games.Game.Modification() = ml.GetActivatedMods()

            For Each m As Games.Game.Modification In _game.Mods
                Dim active As Boolean = m.Active
                If active AndAlso Array.IndexOf(finalactivate, m) > -1 Then
                    Continue For
                End If

                If Not active AndAlso Array.IndexOf(finalactivate, m) < 0 Then
                    Continue For
                End If

                If active Then
                    deactivate.Add(m)
                Else
                    activate.Add(m)
                End If
            Next m

            Return New Games.Game.Modification()() {activate.ToArray, _
                deactivate.ToArray, finalactivate}
        End Function

        Public Sub StartActivateModThread(ByVal activate As Games.Game. _
            Modification(), ByVal deactivate As Games.Game.Modification(), ByVal _
            runAfter As Boolean, ByVal runMultiplayer As Boolean, ByVal mods As Games. _
            Game.Modification())

            MyUpdate.IAmBusy()

            _runAfterwards = runAfter
            _runMultiplayer = runMultiplayer
            _runMods = mods

            _activate = New BackgroundWorker()
            _activate.WorkerReportsProgress = True
            _activate.WorkerSupportsCancellation = True
            AddHandler _activate.DoWork, AddressOf _activate_DoWork
            AddHandler _activate.RunWorkerCompleted, AddressOf _
                _activate_RunWorkerCompleted

            _progress = New fProgress
            _progress.Maximum = activate.Length + deactivate.Length
            _progress.BackgroundWorker = _activate

            _activate.RunWorkerAsync(New Games.Game.Modification()() {activate, _
                deactivate})

            activate = Nothing
            deactivate = Nothing

            _progress.ShowDialog()
            _activate = Nothing
            _progress = Nothing
        End Sub

        Private Sub _activate_RunWorkerCompleted(ByVal sender As Object, ByVal e As  _
            System.ComponentModel.RunWorkerCompletedEventArgs)

            RemoveHandler _activate.DoWork, AddressOf _activate_DoWork
            RemoveHandler _activate.RunWorkerCompleted, AddressOf _
                _activate_RunWorkerCompleted

            ml_ModActivatedChanged(Me, New EventArgs)

            If _runAfterwards Then
                Run(_runMultiplayer, _runMods)
            End If

            MyUpdate.IAmNotBusy()
        End Sub
#End Region

#Region "Event Handlers"
        Private Sub _game_AbortModSearch(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            gh.RefreshingMods = False
        End Sub

        Private Sub _game_AbortPatchSearch(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            gh.RefreshingPatches = False
        End Sub

        Private Sub _game_BeginModSearch(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            gh.RefreshingMods = True

            ml.BeginUpdate()
            ml.ec.Block()

            Dim remove(ml.lvgMods.Items.Count - 1) As ListViewItem
            ml.lvgMods.Items.CopyTo(remove, 0)
            For Each lvi As ListViewItem In remove
                ml.Items.Remove(lvi)
            Next lvi

            ml.AddDefaultEntry()
            ml.ec.Allow()
            ml.EndUpdate()
        End Sub

        Private Sub _game_BeginPatchSearch(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            gh.RefreshingPatches = True

            ml.BeginUpdate()
            ml.ec.Block()

            Dim remove(ml.lvgPatches.Items.Count - 1) As ListViewItem
            ml.lvgPatches.Items.CopyTo(remove, 0)
            For Each lvi As ListViewItem In remove
                ml.Items.Remove(lvi)
            Next lvi

            ml.ec.Allow()
            ml.EndUpdate()
        End Sub

        Private Sub _game_EnabledChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            gh.Enabled = _game.Enabled

            If _game.Enabled Then
                RefreshMods()
                RefreshPatches()
            Else
                _game.AbortRefreshMods()
                _game.AbortRefreshPatches()

                ml.ec.Block()
                ml.Items.Clear()
                ml.ec.Allow()

                RaiseEvent Collapse(Me, e)
            End If
        End Sub

        Private Sub _game_EndModSearch(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            If InvokeRequired Then
                Invoke(New MyModInfoBeginUpdateDelegate(AddressOf MyModInfo.BeginUpdate))

                Dim m As Games.ModInfoCache.Modification
                For Each _mod As Games.Game.Modification In _game.Mods
                    If MyModInfo(_game.Name, _mod.UniqueID) IsNot Nothing Then
                        Continue For
                    End If

                    SyncLock sender
                        m = _mod.RefreshInfo
                    End SyncLock

                    If m Is Nothing Then
                        If _game.GetType Is GetType(Games.DarkForces) Then
                            m = New Games.DarkForcesModInfo(_mod.UniqueID)
                        Else
                            m = New Games.ModInfoCache.Modification(_mod.UniqueID)
                        End If
                    End If

                    UpdateModInfo(_game, _mod, m)
                    RaiseEvent ModInfoRefreshed(Me, New ModInfoEventArgs(_game, _mod, m))
                Next _mod

                Invoke(New EventHandler(AddressOf _game_EndModSearch), sender, e)
                Return
            End If

            MyModInfo.EndUpdate()

            gh.RefreshingMods = False
        End Sub
        Private Delegate Sub MyModInfoBeginUpdateDelegate()

        Private Sub _game_EndPatchSearch(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            If InvokeRequired Then
                Invoke(New EventHandler(AddressOf _game_EndPatchSearch), sender, e)
                Return
            End If

            gh.RefreshingPatches = False
        End Sub

        Private Sub _game_GamePathChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            gh.GamePathDefined = True

            If Not _game.Enabled Then
                _game.Enabled = True
            ElseIf _game.ModPath Is Nothing OrElse _game.ActiveModPath Is Nothing Then
                RefreshMods()
                RefreshPatches()
            End If
        End Sub

        Private Sub _game_ModFound(ByVal sender As System.Object, ByVal e As Games. _
            Game.ModFoundEventArgs)

            If InvokeRequired Then
                Invoke(New EventHandler(AddressOf _game_ModFound), sender, e)
                Return
            End If

            ml.BeginUpdate()
            ml.ec.Block()
            Dim lvi As New ListViewItem(e.Modification.Name)
            With lvi
                If _game.AllowMultipleActiveMods Then
                    .Checked = e.Modification.Active

                    If .Checked Then
                        ml.lviNoMods.Checked = False
                    End If
                Else
                    .Selected = e.Modification.Active
                End If
                .Group = ml.Groups("Mods")
                .ImageKey = ml.DefaultImageKey
                .Tag = e.Modification
            End With
            ml.Items.Add(lvi)
            ml.ec.Allow()

            If ml.Created Then
                ml.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If
            ml.EndUpdate()
        End Sub

        Private Sub _game_PatchFound(ByVal sender As System.Object, ByVal e As Games. _
            Game.PatchFoundEventArgs)

            If InvokeRequired Then
                Invoke(New EventHandler(AddressOf _game_PatchFound), sender, e)
                Return
            End If

            AddPatch(e.Patch)
        End Sub

        Private Sub _game_ModPathChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            If _game.Enabled Then
                RefreshMods()
            End If
        End Sub

        Private Sub _game_PatchPathChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs)

            If _game.Enabled Then
                RefreshPatches()
            End If
        End Sub

        Private Sub gh_ApplyModsClicked(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.ApplyModsClicked

            Dim x As Games.Game.Modification()() = BuildChangeLists()
            Dim activate As Games.Game.Modification() = x(0)
            Dim deactivate As Games.Game.Modification() = x(1)

            Dim count As Integer = activate.Length + deactivate.Length
            If count = 0 Then
                Return
            End If

            StartActivateModThread(activate, deactivate, False, False, Nothing)
        End Sub

        Private Sub gh_EnabledClicked(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.EnabledClicked

            _game.Enabled = Not _game.Enabled
            If _game.Enabled Then
                RaiseEvent Expand(Me, e)
            End If
        End Sub

        Private Sub gh_ExpandRequested(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.ExpandRequested

            RaiseEvent Expand(Me, e)
        End Sub

        Private Sub gh_GamePathClicked(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.GamePathClicked

            fbdPath.SelectedPath = _game.GamePath

            If fbdPath.ShowDialog = DialogResult.Cancel Then
                Return
            End If

            If Not _game.IsValidGamePath(fbdPath.SelectedPath) Then
                MsgBox("This path does not appear to contain " & _game.Name & _
                    ".  Your change has been reverted.", MsgBoxStyle.Exclamation, _
                    "Game path not valid - Knight")
                Return
            End If

            _game.GamePath = fbdPath.SelectedPath
            RaiseEvent Expand(Me, e)
        End Sub

        Private Sub gh_ImportPatchClicked(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.ImportPatchClicked

            With ofdImportPatch
                .CustomPlaces.Clear()
                .CustomPlaces.Add(_game.GamePath)
                .CustomPlaces.Add(_game.GetPatchPath)
                .CustomPlaces.Add(_game.GetModPath)
                .CustomPlaces.Add(_game.GetActiveModPath)
                .FileName = ""
                .InitialDirectory = _game.GamePath

                If .ShowDialog <> DialogResult.OK Then
                    Return
                End If
            End With

            For Each file As String In ofdImportPatch.FileNames
                Dim suppressdialog As Boolean = False
                Dim filehash As HashList.Hash = MyHashes.HashFile("", file)
                Dim h As HashList.Hash = MyHashes.IdentifyFile(filehash.DataHash, _
                    filehash.DataSize)
                If h IsNot Nothing Then
                    If _game.AlreadyFoundPatch(h.UniqueID) Then
                        If Not suppressdialog Then
                            MsgBox("One or more of the patches you selected is " & _
                                "already installed.  They will be skipped.", _
                                MsgBoxStyle.Exclamation, "Error - Knight")
                            suppressdialog = True
                        End If

                        Continue For
                    End If
                Else
                    h = New HashList.Hash(filehash.ValueString)
                    h.Name = "Unknown Imported Patch"
                    h.FileName = filehash.FileName
                    h.DataHash = filehash.DataHash
                    h.DataSize = filehash.DataSize
                End If

                AddPatch(_game.InstallPatch(file, h))
            Next file
        End Sub

        Private Sub gh_OptionsClicked(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.OptionsClicked

            _game.ShowOptions()
        End Sub

        Private Sub gh_PlayClicked(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles gh.PlayClicked

            Dim x As Games.Game.Modification()() = BuildChangeLists()
            Dim activate As Games.Game.Modification() = x(0)
            Dim deactivate As Games.Game.Modification() = x(1)
            Dim finalactivate As Games.Game.Modification() = x(2)

            Dim count As Integer = activate.Length + deactivate.Length
            If finalactivate.Length = 0 OrElse count = 0 Then
                Run(False, finalactivate)
                Return
            End If

            StartActivateModThread(activate, deactivate, True, False, finalactivate)
        End Sub

        Private Sub gh_PlayMultiplayerClicked(ByVal sender As System.Object, ByVal e _
            As System.EventArgs) Handles gh.PlayMultiplayerClicked

            Dim x As Games.Game.Modification()() = BuildChangeLists()
            Dim activate As Games.Game.Modification() = x(0)
            Dim deactivate As Games.Game.Modification() = x(1)
            Dim finalactivate As Games.Game.Modification() = x(2)

            Dim count As Integer = activate.Length + deactivate.Length
            If finalactivate.Length = 0 OrElse count = 0 Then
                Run(True, finalactivate)
                Return
            End If

            StartActivateModThread(activate, deactivate, True, True, finalactivate)
        End Sub

        Private Sub gh_RefreshModsClicked(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.RefreshModsClicked

            If Not _game.IsRefreshingMods Then
                RefreshMods()
                RaiseEvent Expand(Me, e)
            Else
                _game.AbortRefreshMods()
            End If
        End Sub

        Private Sub gh_RefreshPatchesClicked(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles gh.RefreshPatchesClicked

            If Not _game.IsRefreshingPatches Then
                RefreshPatches()
                RaiseEvent Expand(Me, e)
            Else
                _game.AbortRefreshPatches()
            End If
        End Sub

        Private Sub ml_ModActivated(ByVal sender As System.Object, ByVal e As ModList. _
            ModEventArgs) Handles ml.ModActivated

            Dim x As Games.Game.Modification()() = BuildChangeLists()
            Dim activate As Games.Game.Modification() = x(0)
            Dim deactivate As Games.Game.Modification() = x(1)
            Dim finalactivate As Games.Game.Modification() = x(2)

            If finalactivate.Length = 0 OrElse Not _game. _
                HasSeparateMultiplayerBinary Then

                _runMultiplayer = False
            Else
                _runMultiplayer = _game.ShoundRunMultiplayer(finalactivate)
            End If

            Dim count As Integer = activate.Length + deactivate.Length
            If finalactivate.Length = 0 OrElse count = 0 Then
                Run(_runMultiplayer, finalactivate)
                Return
            End If

            StartActivateModThread(activate, deactivate, True, _runMultiplayer, _
                finalactivate)
        End Sub

        Private Sub ml_ModActivatedChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles ml.ModActivatedChanged

            Dim x As Games.Game.Modification()() = BuildChangeLists()
            gh.ApplyModEnabled = (x(0).Length > 0 OrElse x(1).Length > 0)
        End Sub

        Private Sub ml_ModNameChanged(ByVal sender As System.Object, ByVal e As  _
            ModList.ModNameChangedEventArgs) Handles ml.ModNameChanged

            Dim m As Games.ModInfoCache.Modification = MyModInfo(_game.Name, e. _
                Modification.UniqueID)
            If m Is Nothing Then
                MyModInfo(_game.Name, e.Modification.UniqueID) = New Games. _
                    ModInfoCache.Modification(e.Name)
            Else
                MyModInfo(_game.Name, e.Modification.UniqueID).Name = e.Name
            End If
            RaiseEvent ModInfoRefreshed(Me, New ModInfoEventArgs(_game, _
                e.Modification, m))
        End Sub

        Private Sub ml_PatchActivatedChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles ml.PatchActivatedChanged

            Dim x As Games.Game.Patch()() = BuildPatchChangeLists()
            Dim activate As Games.Game.Patch() = x(0)
            Dim deactivate As Games.Game.Patch() = x(1)
            For Each p As Games.Game.Patch In deactivate
                p.Active = False
            Next p
            For Each p As Games.Game.Patch In activate
                p.Active = True
            Next p
        End Sub

        Private Sub ml_PatchNameChanged(ByVal sender As Object, ByVal e As ModList. _
            PatchNameChangedEventArgs) Handles ml.PatchNameChanged

            DirectCast(e.Patch, Games.SithPatch).Hash.Name = e.Name
        End Sub

        Private Sub ml_ViewChanged(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles ml.ViewChanged

            RaiseEvent ViewChanged(Me, e)
        End Sub

        Private Sub ml_VisibleChanged(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles ml.VisibleChanged

            ml.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        End Sub
#End Region

#Region "Properties"
        Public ReadOnly Property CollapsedHeight() As Integer
            Get
                Return gh.Height
            End Get
        End Property

        Public Property Game() As Games.Game
            Get
                Return _game
            End Get
            Set(ByVal value As Games.Game)
                If _game IsNot Nothing Then
                    RemoveHandler _game.ActiveModPathChanged, AddressOf _
                        _game_ModPathChanged
                    RemoveHandler _game.AbortModSearch, AddressOf _game_AbortModSearch
                    RemoveHandler _game.AbortPatchSearch, AddressOf _
                        _game_AbortPatchSearch
                    RemoveHandler _game.BeginModSearch, AddressOf _game_BeginModSearch
                    RemoveHandler _game.BeginPatchSearch, AddressOf _
                        _game_BeginPatchSearch
                    RemoveHandler _game.EnabledChanged, AddressOf _game_EnabledChanged
                    RemoveHandler _game.EndModSearch, AddressOf _game_EndModSearch
                    RemoveHandler _game.EndPatchSearch, AddressOf _game_EndPatchSearch
                    RemoveHandler _game.GamePathChanged, AddressOf _game_GamePathChanged
                    RemoveHandler _game.ModFound, AddressOf _game_ModFound
                    RemoveHandler _game.ModPathChanged, AddressOf _game_ModPathChanged
                    RemoveHandler _game.PatchFound, AddressOf _game_PatchFound
                    RemoveHandler _game.PatchPathChanged, AddressOf _
                        _game_PatchPathChanged
                End If

                _game = value

                If _game Is Nothing Then
                    Return
                End If

                fbdPath.Description = "Locate " & _game.Name & "'s directory."

                gh.Enabled = _game.Enabled
                gh.GamePathDefined = _game.GamePath IsNot Nothing
                gh.Image = _game.Image16
                gh.ShowMultiplayerButton = _game.HasSeparateMultiplayerBinary
                gh.ShowPatchesButton = _game.SupportsPatches
                gh.Text = _game.Name

                ml.AllowMultipleMods = _game.AllowMultipleActiveMods
                ml.DefaultImageKey = "|" & _game.GetType.Name
                ml.ShowGroups = _game.SupportsPatches
                ml.Text = _game.Name

                AddHandler _game.ActiveModPathChanged, AddressOf _game_ModPathChanged
                AddHandler _game.AbortModSearch, AddressOf _game_AbortModSearch
                AddHandler _game.AbortPatchSearch, AddressOf _game_AbortPatchSearch
                AddHandler _game.BeginModSearch, AddressOf _game_BeginModSearch
                AddHandler _game.BeginPatchSearch, AddressOf _game_BeginPatchSearch
                AddHandler _game.EnabledChanged, AddressOf _game_EnabledChanged
                AddHandler _game.EndModSearch, AddressOf _game_EndModSearch
                AddHandler _game.EndPatchSearch, AddressOf _game_EndPatchSearch
                AddHandler _game.GamePathChanged, AddressOf _game_GamePathChanged
                AddHandler _game.ModFound, AddressOf _game_ModFound
                AddHandler _game.ModPathChanged, AddressOf _game_ModPathChanged
                AddHandler _game.PatchFound, AddressOf _game_PatchFound
                AddHandler _game.PatchPathChanged, AddressOf _game_PatchPathChanged
            End Set
        End Property
        Private _game As Games.Game = Nothing

        Public Property LargeImageList() As ImageList
            Get
                Return ml.LargeImageList
            End Get
            Set(ByVal value As ImageList)
                ml.LargeImageList = value
            End Set
        End Property

        Public Property SmallImageList() As ImageList
            Get
                Return ml.SmallImageList
            End Get
            Set(ByVal value As ImageList)
                ml.SmallImageList = value
            End Set
        End Property

        Public Property View() As View
            Get
                Return ml.View
            End Get
            Set(ByVal value As View)
                ml.View = value

                If ml.Created Then
                    ml.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                End If
            End Set
        End Property
#End Region

#Region "Events"
        Public Event Collapse(ByVal sender As Object, ByVal e As EventArgs)
        Public Event Expand(ByVal sender As Object, ByVal e As EventArgs)

        Public Class ModInfoEventArgs
            Inherits EventArgs

            Public Sub New(ByVal game As Games.Game, ByVal modification As Games.Game. _
                Modification, ByVal modinfo As Games.ModInfoCache.Modification)

                _game = game
                _modificiation = modification
                _modInfo = modinfo
            End Sub

            Public ReadOnly Property Game() As Games.Game
                Get
                    Return _game
                End Get
            End Property
            Private _game As Games.Game

            Public ReadOnly Property Modification() As Games.Game.Modification
                Get
                    Return _modificiation
                End Get
            End Property
            Private _modificiation As Games.Game.Modification

            Public ReadOnly Property ModInfo() As Games.ModInfoCache.Modification
                Get
                    Return _modInfo
                End Get
            End Property
            Private _modInfo As Games.ModInfoCache.Modification
        End Class
        Public Event ModInfoRefreshed(ByVal sender As Object, ByVal e As  _
            ModInfoEventArgs)

        Public Event ViewChanged(ByVal sender As Object, ByVal e As EventArgs)
#End Region

#Region "Controls"
        Private WithEvents gh As New GameHeader
        Private WithEvents ml As New ModList

        Private WithEvents ofdImportPatch As New OpenFileDialog()

        Private WithEvents fbdPath As New FolderBrowserDialog()
#End Region
    End Class
End Namespace
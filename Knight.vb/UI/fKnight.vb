Namespace MZZT.Knight
	Public Class fKnight
		Public Sub New()
			' This call is required by the Windows Form Designer.
			InitializeComponent()

			' Add any initialization after the InitializeComponent() call.
			Icon = My.Resources.knight
			StartPosition = MyOptions.Position
			If StartPosition = FormStartPosition.Manual Then
				Location = MyOptions.Location
			End If
			ClientSize = MyOptions.Size

			For i As Integer = 0 To cml.Length - 1
				cml(i) = New CollapsibleModList
				With cml(i)
					.Game = MyGames(i)
					.LargeImageList = ilLarge
					.SmallImageList = ilSmall

					AddHandler .Collapse, AddressOf cml_Collapse
					AddHandler .Expand, AddressOf cml_Expand
					AddHandler .ModInfoRefreshed, AddressOf cml_ModInfoRefreshed
					AddHandler .ViewChanged, AddressOf cml_ViewChanged
				End With
			Next i
			Controls.AddRange(cml)

			lvSimple.BringToFront()
			lvSimple.View = MyOptions.View

			If MyOptions.SimpleList Then
				lvSimple.Visible = True

				For i As Integer = 0 To cml.Length - 1
					cml(i).Visible = False
				Next i

				tsbPlay.Visible = True
				tsbPlay.Enabled = False
			End If

			Dim expandedindex As Integer = MyOptions.ExpandedGame
			If expandedindex > -1 AndAlso expandedindex < cml.Length AndAlso cml(expandedindex).Game.Enabled Then
				_expanded = cml(expandedindex)
			End If
			SetCMLRects()
		End Sub

		Protected Overrides Sub OnShown(ByVal e As System.EventArgs)
			lvSimple.ListViewItemSorter = New ModList.Sorter

			MyBase.OnShown(e)

			MyUpdate.ShowCompleteMessage()

			Dim mnemonics As Byte() = New Byte() {0, 0, 0, 5, 5}

			For i As Integer = 0 To MyGames.Length - 1
				lvSimple.BeginUpdate()
				With MyGames(i)
					AddHandler .AbortModSearch, AddressOf MyGames_AbortModSearch
					AddHandler .BeginModSearch, AddressOf MyGames_BeginModSearch
					AddHandler .EnabledChanged, AddressOf MyGames_EnabledChanged
					AddHandler .EndModSearch, AddressOf MyGames_EndModSearch
					AddHandler .ModFound, AddressOf MyGames_ModFound

					ilSmall.Images.Add("|" & .GetType.Name, .Image16)
					ilLarge.Images.Add("|" & .GetType.Name, .Image32)

					Dim lvg As New ListViewGroup(.Name, .Name)
					lvg.Tag = MyGames(i)
					lvSimple.Groups.Add(lvg)

					AddDefaultEntry(MyGames(i))
				End With
				lvSimple.EndUpdate()

				tsmiGame(i) = New ToolStripMenuItem
				With tsmiGame(i)
					.Image = MyGames(i).Image16
					.Tag = MyGames(i)
					.Text = MyGames(i).Name.Insert(mnemonics(i), "&")
					.Visible = False

					tsmiGameOptions(i) = New ToolStripMenuItem("Game &Options", My.
												Resources.silk_wrench)
					With tsmiGameOptions(i)
						.ShowShortcutKeys = False
						AddHandler .Click, AddressOf tsmiGameOptions_Click
					End With
					.DropDownItems.Add(tsmiGameOptions(i))

					tsmiGameRefresh(i) = New ToolStripMenuItem("&Refresh Mods", My.
												Resources.silk_arrow_refresh)
					With tsmiGameRefresh(i)
						.ShowShortcutKeys = False
						AddHandler .Click, AddressOf tsmiGameRefresh_Click
					End With
					.DropDownItems.Add(tsmiGameRefresh(i))

					tsmiGamePath(i) = New ToolStripMenuItem("&Locate " & MyGames(i).
												Name, My.Resources.silk_folder_explore)
					With tsmiGamePath(i)
						.ShowShortcutKeys = False
						AddHandler .Click, AddressOf tsmiGamePath_Click
					End With
					.DropDownItems.Add(tsmiGamePath(i))

					tsmiGameEnable(i) = New ToolStripMenuItem("&Enable " & MyGames(i).
												Name & " Support", My.Resources.silk_tick)
					With tsmiGameEnable(i)
						.ShowShortcutKeys = False
						AddHandler .Click, AddressOf tsmiGameEnable_Click
					End With
					.DropDownItems.Add(tsmiGameEnable(i))

					AddHandler .DropDownOpening, AddressOf tsmiGame_DropDownOpening
				End With

				tsddbOptions.DropDownItems.Add(tsmiGame(i))
			Next i

			For Each c As CollapsibleModList In cml
				If c.Game.Enabled Then
					c.RefreshMods()
					c.RefreshPatches()
				End If
			Next c
		End Sub

		Protected Overrides Sub Finalize()
			For i As Integer = 0 To cml.Length - 1
				With MyGames(i)
					RemoveHandler .AbortModSearch, AddressOf MyGames_AbortModSearch
					RemoveHandler .BeginModSearch, AddressOf MyGames_BeginModSearch
					RemoveHandler .EnabledChanged, AddressOf MyGames_EnabledChanged
					RemoveHandler .EndModSearch, AddressOf MyGames_EndModSearch
					RemoveHandler .ModFound, AddressOf MyGames_ModFound
				End With

				With cml(i)
					RemoveHandler .Collapse, AddressOf cml_Collapse
					RemoveHandler .Expand, AddressOf cml_Expand
					RemoveHandler .ModInfoRefreshed, AddressOf cml_ModInfoRefreshed
					RemoveHandler .ViewChanged, AddressOf cml_ViewChanged
				End With

				RemoveHandler tsmiGame(i).DropDownOpening, AddressOf _
										tsmiGame_DropDownOpening
				RemoveHandler tsmiGameOptions(i).Click, AddressOf tsmiGameOptions_Click
				RemoveHandler tsmiGameRefresh(i).Click, AddressOf tsmiGameRefresh_Click
				RemoveHandler tsmiGamePath(i).Click, AddressOf tsmiGamePath_Click
				RemoveHandler tsmiGameEnable(i).Click, AddressOf tsmiGameEnable_Click
			Next i

			MyBase.Finalize()
		End Sub

#Region "Animation"
		Private Sub Animate(ByVal endrects As Rectangle(), ByVal time As TimeSpan)
			For i As Integer = 0 To cml.Length - 1
				_startrects(i) = cml(i).Bounds
			Next i
			_endrects = endrects
			_starttime = Date.Now
			_time = time

			tAnim.Enabled = True
		End Sub

		Private Function CalculateCMLRects() As Rectangle()
			Dim r(cml.Length - 1) As Rectangle

			Dim availablearea As Rectangle = ClientRectangle
			availablearea.Y += tsToolbar.Height
			availablearea.Height -= tsToolbar.Height

			Dim expandindex As Integer = -1
			For i As Integer = 0 To cml.Length - 1
				If cml(i) Is _expanded Then
					expandindex = i
					Exit For
				End If

				cml(i).Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.
										Right
				r(i) = New Rectangle(availablearea.Location, New Size(availablearea.
										Width, cml(i).CollapsedHeight))

				availablearea.Y += cml(i).CollapsedHeight
				availablearea.Height -= cml(i).CollapsedHeight
			Next i

			If expandindex < 0 Then
				Return r
			End If

			For i As Integer = cml.Length - 1 To expandindex + 1 Step -1
				cml(i).Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or
										AnchorStyles.Right
				r(i) = New Rectangle(New Point(availablearea.X, availablearea.Bottom +
										1 - cml(i).CollapsedHeight), New Size(availablearea.Width, cml(i).
										CollapsedHeight))

				availablearea.Height -= cml(i).CollapsedHeight
			Next i

			_expanded.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or
								AnchorStyles.Left Or AnchorStyles.Right
			r(expandindex) = availablearea

			Return r
		End Function

		Private Sub SetCMLRects()
			Dim r() As Rectangle = CalculateCMLRects()
			For i As Integer = 0 To cml.Length - 1
				cml(i).Bounds = r(i)
			Next i
		End Sub

		Private Sub cml_Collapse(ByVal sender As Object, ByVal e As EventArgs)
			If _expanded IsNot sender Then
				Return
			End If
			_expanded = Nothing
			MyOptions.ExpandedGame = -1

			If lvSimple.Visible Then
				SetCMLRects()
			Else
				Animate(CalculateCMLRects(), New TimeSpan(0, 0, 0, 0, AnimationTime))
			End If
		End Sub

		Private Sub cml_Expand(ByVal sender As Object, ByVal e As EventArgs)
			_expanded = sender
			MyOptions.ExpandedGame = Array.IndexOf(cml, _expanded)

			If lvSimple.Visible Then
				SetCMLRects()
			Else
				Animate(CalculateCMLRects(), New TimeSpan(0, 0, 0, 0, AnimationTime))
			End If
		End Sub

		Private Sub tAnim_Tick(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tAnim.Tick

			Dim pos As Single = CSng((Date.Now - _starttime).Ticks) /
								_time.Ticks
			If lvSimple.Visible Then
				pos = 1
			End If

			If pos > 1 Then
				pos = 1
			End If

			Dim r As Rectangle
			For i As Integer = 0 To cml.Length - 1
				r.X = ((_endrects(i).X - _startrects(i).X) * pos) + _startrects(i).X
				r.Y = ((_endrects(i).Y - _startrects(i).Y) * pos) + _startrects(i).Y
				r.Width = ((_endrects(i).Width - _startrects(i).Width) * pos) +
										_startrects(i).Width
				r.Height = ((_endrects(i).Height - _startrects(i).Height) * pos) +
										_startrects(i).Height
				cml(i).Bounds = r
			Next i

			If pos = 1 Then
				tAnim.Enabled = False
			End If
		End Sub

		Private _endrects() As Rectangle = Nothing
		Private _expanded As CollapsibleModList = Nothing
		Private _startrects(4) As Rectangle
		Private _starttime As Date = Nothing
		Private _time As TimeSpan = Nothing
#End Region

#Region "Simple List"
		Private Sub AddDefaultEntry(ByVal g As Games.Game)
			Dim lviNoMods As New ListViewItem(g.Name, "|" & g.GetType.Name, lvSimple.
								Groups(g.Name))
			lvSimple.Items.Add(lviNoMods)

			If lvSimple.Created Then
				lvSimple.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
			End If
		End Sub

		Private Function BuildChangeLists() As Games.Game.Modification()()
			Dim activate As New List(Of Games.Game.Modification)
			Dim deactivate As New List(Of Games.Game.Modification)

			Dim game As Games.Game = lvSimple.SelectedItems(0).Group.Tag

			Dim finalactivate As Games.Game.Modification()
			If lvSimple.SelectedItems(0).Tag Is Nothing Then
				finalactivate = New Games.Game.Modification() {}
			Else
				finalactivate = New Games.Game.Modification() {lvSimple.
										SelectedItems(0).Tag}
			End If

			For Each m As Games.Game.Modification In game.Mods
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

			Return New Games.Game.Modification()() {activate.ToArray,
								deactivate.ToArray, finalactivate}
		End Function

		Private Sub cml_ModInfoRefreshed(ByVal sender As System.Object, ByVal e As _
						CollapsibleModList.ModInfoEventArgs)

			If InvokeRequired() Then
				Invoke(New EventHandler(AddressOf cml_ModInfoRefreshed), sender, e)
				Return
			End If

			For Each lvi As ListViewItem In lvSimple.Items
				If lvi.Tag Is e.Modification Then
					lvi.Text = e.Modification.Name

					If lvSimple.Created Then
						lvSimple.AutoResizeColumns(ColumnHeaderAutoResizeStyle.
														ColumnContent)
					End If
					lvSimple.Sort()

					Return
				End If
			Next lvi
		End Sub

		Private Sub cmdLV_Opening(ByVal sender As System.Object, ByVal e As System.
						ComponentModel.CancelEventArgs) Handles cmsLV.Opening

			tsmiIcons.Checked = lvSimple.View = Windows.Forms.View.LargeIcon
			tsmiList.Checked = lvSimple.View = Windows.Forms.View.Details
		End Sub

		Private Sub lvSimple_AfterLabelEdit(ByVal sender As System.Object, ByVal e As _
						System.Windows.Forms.LabelEditEventArgs) Handles lvSimple.AfterLabelEdit

			If lvSimple.Items(e.Item).Tag Is Nothing Then
				e.CancelEdit = True
			End If

			If e.CancelEdit OrElse e.Label = "" Then
				Return
			End If

			Dim m As Games.Game.Modification = lvSimple.Items(e.Item).Tag
			If m Is Nothing Then
				e.CancelEdit = True
				Return
			End If

			Dim game As Games.Game = lvSimple.Items(e.Item).Group.Tag
			Dim index As Integer = Array.IndexOf(MyGames, game)
			Dim modinfo As Games.ModInfoCache.Modification = MyModInfo(game.Name, m.
								UniqueID)

			MyModInfo.BeginUpdate()
			If modinfo Is Nothing Then
				modinfo = New Games.ModInfoCache.Modification(e.Label)
			Else
				modinfo.Name = e.Label
			End If

			cml(index).UpdateModInfo(game, m, modinfo)
			MyModInfo.EndUpdate()

			Dim t As New Timer
			t.Interval = 10
			AddHandler t.Tick, AddressOf t_Tick
			t.Start()
		End Sub

		Private Sub lvSimple_BeforeLabelEdit(ByVal sender As System.Object, ByVal e As _
						System.Windows.Forms.LabelEditEventArgs) Handles lvSimple.BeforeLabelEdit

			If lvSimple.Items(e.Item).Tag Is Nothing Then
				e.CancelEdit = True
			End If
		End Sub

		Private Sub lvSimple_ItemActivate(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles lvSimple.ItemActivate

			If lvSimple.SelectedIndices.Count = 0 Then
				Return
			End If

			Dim game As Games.Game = lvSimple.SelectedItems(0).Group.Tag
			Dim index As Integer = Array.IndexOf(MyGames, game)

			Dim x As Games.Game.Modification()() = BuildChangeLists()
			Dim activate As Games.Game.Modification() = x(0)
			Dim deactivate As Games.Game.Modification() = x(1)
			Dim finalactivate As Games.Game.Modification() = x(2)

			Dim multi As Boolean
			If finalactivate.Length = 0 OrElse Not game.HasSeparateMultiplayerBinary Then
				multi = False
			Else
				multi = game.ShoundRunMultiplayer(finalactivate)
			End If

			Dim count As Integer = activate.Length + deactivate.Length
			If finalactivate.Length = 0 OrElse count = 0 Then
				cml(index).Run(multi, finalactivate)
				Return
			End If

			cml(index).StartActivateModThread(activate, deactivate, True, multi,
								finalactivate)
		End Sub

		Private Sub lvSimple_MouseDown(ByVal sender As Object, ByVal e As System.
						Windows.Forms.MouseEventArgs) Handles lvSimple.MouseDown

			If (e.Button And Windows.Forms.MouseButtons.Right) = 0 Then
				Return
			End If

			Dim lvi As ListViewItem = lvSimple.HitTest(e.Location).Item
			If lvi Is Nothing Then
				cmsLV.Show(lvSimple, e.Location)
				Return
			End If

			If lvi.Tag Is Nothing Then
				Return
			End If

			tsmiProperties.Visible = DirectCast(lvi.Tag, Games.Game.Modification).
								HasOptions()

			cmsLVI.Tag = lvi
			cmsLVI.Show(lvSimple, e.Location)
		End Sub

		Private Sub lvSimple_SelectedIndexChanged(ByVal sender As System.Object, ByVal _
						e As System.EventArgs) Handles lvSimple.SelectedIndexChanged

			If lvSimple.SelectedIndices.Count = 0 Then
				tssbPlay.Visible = False
				tsbPlay.Visible = True
				tsbPlay.Enabled = False
			Else
				tsbPlay.Enabled = True

				Dim game As Games.Game = lvSimple.SelectedItems(0).Group.Tag
				tssbPlay.Visible = game.HasSeparateMultiplayerBinary
				tsbPlay.Visible = Not game.HasSeparateMultiplayerBinary
			End If
		End Sub

		Private Sub MyGames_AbortModSearch(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim game As Games.Game = sender
			Dim index As Integer = Array.IndexOf(MyGames, game)
			tsmiGameRefresh(index).Image = My.Resources.silk_arrow_refresh
			tsmiGameRefresh(index).Text = "&Refresh Mods"

			_numRefreshing -= 1
			If _numRefreshing <= 0 Then
				tsmiRefresh.Image = My.Resources.silk_arrow_refresh
				tsmiRefresh.Text = "&Refresh List"
			End If
		End Sub

		Private Sub MyGames_BeginModSearch(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim game As Games.Game = sender
			Dim index As Integer = Array.IndexOf(MyGames, game)
			tsmiGameRefresh(index).Image = My.Resources.silk_stop
			tsmiGameRefresh(index).Text = "Stop &Refreshing Mods"

			RemoveGameMods(game)
			AddDefaultEntry(game)

			_numRefreshing += 1
			tsmiRefresh.Image = My.Resources.silk_stop
			tsmiRefresh.Text = "Stop &Refreshing List"
		End Sub
		Private _numRefreshing As Byte = 0

		Private Sub MyGames_EnabledChanged(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim game As Games.Game = sender
			If Not game.Enabled Then
				RemoveGameMods(game)
			End If
		End Sub

		Private Sub MyGames_EndModSearch(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			If InvokeRequired Then
				Invoke(New EventHandler(AddressOf MyGames_EndModSearch), sender, e)
				Return
			End If

			Dim game As Games.Game = sender
			Dim index As Integer = Array.IndexOf(MyGames, game)
			tsmiGameRefresh(index).Image = My.Resources.silk_arrow_refresh
			tsmiGameRefresh(index).Text = "&Refresh Mods"

			_numRefreshing -= 1
			If _numRefreshing <= 0 Then
				tsmiRefresh.Image = My.Resources.silk_arrow_refresh
				tsmiRefresh.Text = "&Refresh List"
			End If
		End Sub

		Private Sub MyGames_ModFound(ByVal sender As System.Object, ByVal e As Games.
						Game.ModFoundEventArgs)

			If InvokeRequired Then
				Invoke(New EventHandler(AddressOf MyGames_ModFound), sender, e)
				Return
			End If

			Dim game As Games.Game = sender
			Dim index As Integer = Array.IndexOf(MyGames, game)

			lvSimple.BeginUpdate()
			Dim lvi As New ListViewItem(e.Modification.Name)
			With lvi
				.Group = lvSimple.Groups(game.Name)
				.ImageKey = "|" & game.GetType.Name
				.Tag = e.Modification
			End With
			lvSimple.Items.Add(lvi)

			If lvSimple.Created Then
				lvSimple.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
			End If
			lvSimple.EndUpdate()
		End Sub

		Private Sub RemoveGameMods(ByVal game As Games.Game)
			lvSimple.BeginUpdate()

			Dim lvg As ListViewGroup = lvSimple.Groups(game.Name)
			Dim remove(lvg.Items.Count - 1) As ListViewItem
			lvg.Items.CopyTo(remove, 0)
			For Each lvi As ListViewItem In remove
				lvSimple.Items.Remove(lvi)
			Next lvi

			lvSimple.EndUpdate()
		End Sub

		Private Sub tsbPlay_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsbPlay.Click, tssbPlay.ButtonClick

			Dim game As Games.Game = lvSimple.SelectedItems(0).Group.Tag
			Dim index As Integer = Array.IndexOf(MyGames, game)

			Dim x As Games.Game.Modification()() = BuildChangeLists()
			Dim activate As Games.Game.Modification() = x(0)
			Dim deactivate As Games.Game.Modification() = x(1)
			Dim finalactivate As Games.Game.Modification() = x(2)

			Dim count As Integer = activate.Length + deactivate.Length
			If finalactivate.Length = 0 OrElse count = 0 Then
				cml(index).Run(False, finalactivate)
				Return
			End If

			cml(index).StartActivateModThread(activate, deactivate, True, False,
								finalactivate)
		End Sub

		Private Sub tsmiGameEnable_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim tsmi As ToolStripMenuItem = sender
			Dim game As Games.Game = tsmi.OwnerItem.Tag
			game.Enabled = Not game.Enabled
		End Sub

		Private Sub tsmiGameOptions_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim tsmi As ToolStripMenuItem = sender
			Dim game As Games.Game = tsmi.OwnerItem.Tag
			game.ShowOptions()
		End Sub

		Private Sub tsmiGamePath_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim tsmi As ToolStripMenuItem = sender
			Dim game As Games.Game = tsmi.OwnerItem.Tag
			fbdPath.Description = "Locate " & game.Name & "'s directory."
			fbdPath.SelectedPath = game.GamePath

			If fbdPath.ShowDialog = DialogResult.Cancel Then
				Return
			End If

			If Not game.IsValidGamePath(fbdPath.SelectedPath) Then
				MsgBox("This path does not appear to contain " & game.Name &
										".  Your change has been reverted.", MsgBoxStyle.Exclamation,
										"Game path not valid - Knight")
				Return
			End If

			game.GamePath = fbdPath.SelectedPath
		End Sub

		Private Sub tsmiGameRefresh_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim tsmi As ToolStripMenuItem = sender
			Dim game As Games.Game = tsmi.OwnerItem.Tag
			If game.IsRefreshingMods Then
				game.AbortRefreshMods()
			Else
				game.RefreshMods()
			End If
		End Sub

		Private Sub tsmiGame_DropDownOpening(ByVal sender As System.Object, ByVal e As _
						System.EventArgs)

			Dim tsmi As ToolStripMenuItem = sender
			Dim index As Integer = Array.IndexOf(tsmiGame, tsmi)
			Dim game As Games.Game = tsmi.Tag

			If game.GamePath Is Nothing Then
				tsmiGameOptions(index).Visible = False
				tsmiGameRefresh(index).Visible = False
				tsmiGamePath(index).Visible = True
				tsmiGameEnable(index).Visible = False
				Return
			End If

			If Not game.Enabled Then
				tsmiGameOptions(index).Visible = False
				tsmiGameRefresh(index).Visible = False
				tsmiGamePath(index).Visible = True
				tsmiGameEnable(index).Visible = True

				tsmiGameEnable(index).Image = My.Resources.silk_tick
				tsmiGameEnable(index).Text = "&Enable " & game.Name & " Support"
				Return
			End If

			tsmiGameOptions(index).Visible = True
			tsmiGameRefresh(index).Visible = True
			tsmiGamePath(index).Visible = True
			tsmiGameEnable(index).Visible = True

			tsmiGameEnable(index).Image = My.Resources.silk_cross
			tsmiGameEnable(index).Text = "Disabl&e " & game.Name & " Support"
		End Sub

		Private Sub tsmiIcons_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiIcons.Click

			MyOptions.View = Windows.Forms.View.LargeIcon
			cml_ViewChanged(Me, e)
		End Sub

		Private Sub tsmiList_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiList.Click

			MyOptions.View = Windows.Forms.View.Details
			cml_ViewChanged(Me, e)
		End Sub

		Private Sub tsmiPlayMultiplayer_Click(ByVal sender As System.Object, ByVal e _
						As System.EventArgs) Handles tsmiPlayMultiplayer.Click

			Dim game As Games.Game = lvSimple.SelectedItems(0).Group.Tag
			Dim index As Integer = Array.IndexOf(MyGames, game)

			Dim x As Games.Game.Modification()() = BuildChangeLists()
			Dim activate As Games.Game.Modification() = x(0)
			Dim deactivate As Games.Game.Modification() = x(1)
			Dim finalactivate As Games.Game.Modification() = x(2)

			Dim count As Integer = activate.Length + deactivate.Length
			If finalactivate.Length = 0 OrElse count = 0 Then
				cml(index).Run(True, finalactivate)
				Return
			End If

			cml(index).StartActivateModThread(activate, deactivate, True, True,
								finalactivate)
		End Sub

		Private Sub tsmiRefresh_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiRefresh.Click

			Dim _refreshing As Boolean = False
			For Each g As Games.Game In MyGames
				If g.IsRefreshingMods Then
					_refreshing = True
					Exit For
				End If
			Next g

			If _refreshing Then
				For Each g As Games.Game In MyGames
					g.AbortRefreshMods()
				Next g
			Else
				For Each g As Games.Game In MyGames
					g.RefreshMods()
				Next g
			End If
		End Sub

		Private Sub tsmiRename_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiRename.Click

			Dim lvi As ListViewItem = cmsLVI.Tag
			lvi.BeginEdit()
		End Sub

		Private Sub tsmiProperties_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiProperties.Click

			Dim lvi As ListViewItem = cmsLVI.Tag
			DirectCast(lvi.Tag, Games.Game.Modification).ShowOptions()
		End Sub

		Private Sub t_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
			Dim t As Timer = sender
			t.Stop()
			RemoveHandler t.Tick, AddressOf t_Tick

			If lvSimple.Created Then
				lvSimple.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
			End If

			t.Dispose()
		End Sub
#End Region

#Region "Event Handlers"
		Private Sub cml_ViewChanged(ByVal sender As System.Object, ByVal e As System.
						EventArgs)

			For Each c As CollapsibleModList In cml
				c.View = MyOptions.View
			Next c

			lvSimple.View = MyOptions.View
		End Sub

		Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
			MyBase.OnResize(e)

			If Not tAnim.Enabled OrElse lvSimple.Visible Then
				Return
			End If

			For i As Integer = 0 To _endrects.Length - 1
				_endrects(i).Width = ClientSize.Width
				cml(i).Width = ClientSize.Width
				_startrects(i).Width = ClientSize.Width
			Next i

			If _expanded Is Nothing Then
				Return
			End If

			Dim deltaheight As Integer = ClientSize.Height - _endrects(4).Bottom + 1
			Dim index As Integer = Array.IndexOf(cml, _expanded)

			_endrects(index).Height += deltaheight
			_expanded.Height += deltaheight
			_startrects(index).Height += deltaheight

			For i As Integer = index + 1 To _endrects.Length - 1
				_endrects(i).Y += deltaheight
				cml(i).Top += deltaheight
				_startrects(i).Y += deltaheight
			Next i
		End Sub

		Protected Overrides Sub OnResizeEnd(ByVal e As System.EventArgs)
			MyBase.OnResizeEnd(e)

			MyOptions.Location = Location
			MyOptions.Size = ClientSize
		End Sub

		Private Sub tsddbHelp_DropDownOpening(ByVal sender As Object, ByVal e As _
						System.EventArgs) Handles tsddbHelp.DropDownOpening

			If MyUpdate.RestartForUpdate Then
				tsmiUpdate.Enabled = False
				tsmiUpdate.Text = "Restart to apply update"
			End If
		End Sub

		Private Sub tsddbOptions_DropDownOpening(ByVal sender As System.Object,
						ByVal e As System.EventArgs) Handles tsddbOptions.DropDownOpening

			tsmiSimple.Checked = lvSimple.Visible
			tsmiTiered.Checked = Not lvSimple.Visible

			tsmiClose.Checked = MyOptions.OnRunAction = Options.OnRunActions.Close
			tsmiMinimize.Checked = MyOptions.OnRunAction = Options.OnRunActions.Minimize
			tsmiNone.Checked = MyOptions.OnRunAction = Options.OnRunActions.None

			tssOptions2.Visible = lvSimple.Visible
			tsmiRefresh.Visible = lvSimple.Visible
			For i As Integer = 0 To MyGames.Length - 1
				tsmiGame(i).Visible = lvSimple.Visible
			Next i
		End Sub

		Private Sub tsmiClose_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiClose.Click

			MyOptions.OnRunAction = Options.OnRunActions.Close
		End Sub

		Private Sub tsmiDF21Forums_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiDF21Forums.Click

			Process.Start("http://www.df-21.net/phpbb/")
		End Sub

		Private Sub tsmiDF21_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiDF21.Click

			Process.Start("http://www.df-21.net/")
		End Sub

		Private Sub tsmiJKHubChat_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiJKHubChat.Click

			Dim rk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.
								ClassesRoot.OpenSubKey("irc", False)
			If rk IsNot Nothing Then
				rk.Close()
				Process.Start("irc://irc.synirc.net/jkhub/")
			Else
				Process.Start("http://www.jkhub.net/index.php?subsection=chat")
			End If
		End Sub

		Private Sub tsmiJKHub_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiJKHub.Click

			Process.Start("http://www.jkhub.net/")
		End Sub

		Private Sub tsmiEmail_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiEmail.Click

			Process.Start("mailto:megazzt@gmail.com")
		End Sub

		Private Sub tsmiMassassiForums_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiMassassiForums.Click

			Process.Start("http://forums.massassi.net/")
		End Sub

		Private Sub tsmiMassassiIRC_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiMassassiIRC.Click

			Dim rk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.
								ClassesRoot.OpenSubKey("irc", False)
			If rk IsNot Nothing Then
				rk.Close()
				Process.Start("irc://irc.synirc.net/massassi/")
			Else
				Process.Start("http://www.massassi.net/chat/chat.shtml")
			End If
		End Sub

		Private Sub tsmiMassassi_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiMassassi.Click

			Process.Start("http://www.massassi.net/")
		End Sub

		Private Sub tsmiMinimize_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiMinimize.Click

			MyOptions.OnRunAction = Options.OnRunActions.Minimize
		End Sub

		Private Sub tsmiNone_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiNone.Click

			MyOptions.OnRunAction = Options.OnRunActions.None
		End Sub

		Private Sub tsmiSimple_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiSimple.Click

			lvSimple.Visible = True
			If lvSimple.Created Then
				lvSimple.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
			End If

			For i As Integer = 0 To cml.Length - 1
				cml(i).Visible = False
			Next i

			If lvSimple.SelectedIndices.Count < 1 Then
				tsbPlay.Visible = True
				tsbPlay.Enabled = False
			Else
				Dim game As Games.Game = lvSimple.SelectedItems(0).Group.Tag
				If game.HasSeparateMultiplayerBinary Then
					tssbPlay.Visible = True
				Else
					tsbPlay.Visible = True
					tsbPlay.Enabled = True
				End If
			End If

			MyOptions.SimpleList = True
		End Sub

		Private Sub tsmiTiered_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiTiered.Click

			SetCMLRects()
			For i As Integer = 0 To cml.Length - 1
				cml(i).Visible = True
			Next i
			lvSimple.Visible = False

			tsbPlay.Visible = False
			tssbPlay.Visible = False

			MyOptions.SimpleList = False
		End Sub

		Private Sub tsmiUpdate_Click(ByVal sender As System.Object, ByVal e As System.
						EventArgs) Handles tsmiUpdate.Click

			If Not MyUpdate.RestartForUpdate Then
				MyUpdate.Update(True)
			End If
		End Sub

		Private Sub tsmiWebsite_Click(ByVal sender As System.Object, ByVal e As _
						System.EventArgs) Handles tsmiWebsite.Click

			Process.Start("http://www.mzzt.net/")
		End Sub
#End Region

#Region "Controls"
		Private cml(4) As CollapsibleModList
		Private tsmiGame(4) As ToolStripMenuItem
		Private tsmiGameOptions(4) As ToolStripMenuItem
		Private tsmiGameRefresh(4) As ToolStripMenuItem
		Private tsmiGamePath(4) As ToolStripMenuItem
		Private tsmiGameEnable(4) As ToolStripMenuItem
#End Region
	End Class
End Namespace

Namespace MZZT.Knight
    Public Class GameHeader
        Inherits ToolStrip

        Public Sub New()
            MyBase.New()

            GripStyle = ToolStripGripStyle.Hidden
            Dock = DockStyle.Top
            Padding = New Padding(3, 0, 3, 0)

            With tslName
                .ActiveLinkColor = .ForeColor
                .Enabled = False
                .IsLink = True
                .LinkBehavior = LinkBehavior.HoverUnderline
                .LinkColor = .ForeColor
                .Overflow = ToolStripItemOverflow.Never
            End With

            With tsbPlay
                .Alignment = ToolStripItemAlignment.Right
                .DisplayStyle = ToolStripItemDisplayStyle.Image
                .Visible = False
            End With

            With tsbPlayMultiplayer
                .Alignment = ToolStripItemAlignment.Right
                .DisplayStyle = ToolStripItemDisplayStyle.Image
                .Visible = False
            End With

            With tsbApplyMods
                .Enabled = False
                .Overflow = ToolStripItemOverflow.Always
                .Visible = False
            End With

            With tsbOptions
                .Overflow = ToolStripItemOverflow.Always
                .Visible = False
            End With

            With tsbRefresh
                .Overflow = ToolStripItemOverflow.Always
                .Visible = False
            End With

            With tsbRefreshPatches
                .Overflow = ToolStripItemOverflow.Always
                .Visible = False
            End With

            With tsbRefreshPatches
                .Overflow = ToolStripItemOverflow.Always
                .Visible = False
            End With

            With tsbImportPatch
                .Overflow = ToolStripItemOverflow.Always
                .Visible = False
            End With

            With tsbPath
                .Alignment = ToolStripItemAlignment.Right
                .DisplayStyle = ToolStripItemDisplayStyle.Image
                .Visible = False
            End With

            With tsbEnable
                .Alignment = ToolStripItemAlignment.Right
                .DisplayStyle = ToolStripItemDisplayStyle.Image
                .Visible = False
            End With

            Items.AddRange(New ToolStripItem() {tslName, tsbPlay, tsbPlayMultiplayer, _
                tsbApplyMods, tsbOptions, tsbRefresh, tsbRefreshPatches, _
                tsbImportPatch, tsbPath, tsbEnable})

            UpdateState()

            AddHandler OverflowButton.DropDownOpening, AddressOf _
                OverflowButton_DropDownOpening
            AddHandler OverflowButton.DropDownClosed, AddressOf _
                OverflowButton_DropDownClosed
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()

            RemoveHandler OverflowButton.DropDownOpening, AddressOf _
                OverflowButton_DropDownOpening
            RemoveHandler OverflowButton.DropDownClosed, AddressOf _
                OverflowButton_DropDownClosed
        End Sub

        Private Sub UpdateState()
            Dim game As String = MyBase.Text
            If game Is Nothing Then
                game = "Game"
            End If

            tsbPath.Text = "Locate " & game
            If _enabled Then
                tsbEnable.Image = My.Resources.silk_cross
                tsbEnable.Text = "Disable " & game & " Support"
            Else
                tsbEnable.Image = My.Resources.silk_tick
                tsbEnable.Text = "Enable " & game & " Support"
            End If

            If Not _gamePathDefined Then
                tslName.Enabled = False
                tsbPlay.Visible = False
                tsbPlayMultiplayer.Visible = False
                tsbApplyMods.Visible = False
                tsbOptions.Visible = False
                tsbRefresh.Visible = False
                tsbRefreshPatches.Visible = False
                tsbImportPatch.Visible = False

                With tsbPath
                    .Alignment = ToolStripItemAlignment.Right
                    .DisplayStyle = ToolStripItemDisplayStyle.Image
                    .Overflow = ToolStripItemOverflow.AsNeeded
                    .Visible = True
                End With

                tsbEnable.Visible = False
                Return
            End If

            If Not _enabled Then
                tslName.Enabled = False
                tsbPlay.Visible = False
                tsbPlayMultiplayer.Visible = False
                tsbApplyMods.Visible = False
                tsbOptions.Visible = False
                tsbRefresh.Visible = False
                tsbRefreshPatches.Visible = False
                tsbImportPatch.Visible = False

                With tsbPath
                    .Alignment = ToolStripItemAlignment.Right
                    .DisplayStyle = ToolStripItemDisplayStyle.Image
                    .Overflow = ToolStripItemOverflow.AsNeeded
                    .Visible = True
                End With

                With tsbEnable
                    .Alignment = ToolStripItemAlignment.Right
                    .DisplayStyle = ToolStripItemDisplayStyle.Image
                    .Overflow = ToolStripItemOverflow.AsNeeded
                    .Visible = True
                End With

                Return
            End If

            tslName.Enabled = True
            tsbPlay.Visible = True

            If _showMultiplayerButton Then
                tsbPlayMultiplayer.Visible = True
                tsbPlay.Text = "Play Single Player"
            Else
                tsbPlayMultiplayer.Visible = False
                tsbPlay.Text = "Play"
            End If

            tsbApplyMods.Visible = True
            tsbOptions.Visible = True
            tsbRefresh.Visible = True
            tsbRefreshPatches.Visible = _showPatchesButton

            If _refreshingMods Then
                tsbRefresh.Image = My.Resources.silk_stop
                If _showPatchesButton Then
                    tsbRefresh.Text = "Stop Refreshing Mods"
                Else
                    tsbRefresh.Text = "Stop Refreshing List"
                End If
            Else
                tsbRefresh.Image = My.Resources.silk_arrow_refresh
                If _showPatchesButton Then
                    tsbRefresh.Text = "Refresh Mods"
                Else
                    tsbRefresh.Text = "Refresh List"
                End If
            End If

            If _refreshingPatches Then
                tsbRefreshPatches.Image = My.Resources.silk_stop
                tsbRefreshPatches.Text = "Stop Refreshing Patches"
            Else
                tsbRefreshPatches.Image = My.Resources.silk_application_lightning
                tsbRefreshPatches.Text = "Refresh Patches"
            End If

            tsbImportPatch.Visible = _showPatchesButton

            With tsbPath
                .Alignment = ToolStripItemAlignment.Left
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                .Overflow = ToolStripItemOverflow.Always
                .Visible = True
            End With

            With tsbEnable
                .Alignment = ToolStripItemAlignment.Left
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                .Overflow = ToolStripItemOverflow.Always
                .Visible = True
            End With
        End Sub

#Region "Event Handlers"
        Protected Overrides Sub OnMouseUp(ByVal mea As System.Windows.Forms. _
            MouseEventArgs)

            MyBase.OnMouseUp(mea)

            If Not Enabled Then
                Return
            End If

            Dim tsi As ToolStripItem = GetItemAt(mea.Location)
            If tsi IsNot Nothing AndAlso tsi IsNot tslName Then
                Return
            End If

            RaiseEvent ExpandRequested(Me, New EventArgs)
        End Sub

        Private Sub OverflowButton_DropDownOpening(ByVal sender As Object, ByVal e As  _
            EventArgs)

            Dim s As Size = Size.Empty
            For Each tsi As ToolStripItem In Items
                If Not tsi.IsOnOverflow Then
                    Continue For
                End If

                tsi.AutoSize = False

                'Dim t As Type = GetType(ToolStripItem)
                'Dim b As Specialized.BitVector32 = t.GetField("state", Reflection. _
                '    BindingFlags.DeclaredOnly Or Reflection.BindingFlags.NonPublic Or _
                '    Reflection.BindingFlags.Instance).GetValue(tsi)
                'Dim v As Integer = t.GetField("stateVisible", Reflection.BindingFlags. _
                '    DeclaredOnly Or Reflection.BindingFlags.NonPublic Or Reflection. _
                '    BindingFlags.Static).GetValue(Nothing)
                'If b.Item(v) = 0 Then
                '    Continue For
                'End If

                tsi.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
                tsi.ImageAlign = ContentAlignment.MiddleLeft
                tsi.TextAlign = ContentAlignment.MiddleLeft

                Dim mysize As Size = tsi.GetPreferredSize(Nothing)
                s.Width = Math.Max(s.Width, mysize.Width)
                s.Height = Math.Max(s.Height, mysize.Height)
            Next tsi

            For Each tsi As ToolStripItem In Items
                If Not tsi.IsOnOverflow Then
                    Continue For
                End If

                tsi.Size = s
            Next tsi
        End Sub

        Private Sub OverflowButton_DropDownClosed(ByVal sender As Object, ByVal e As  _
            EventArgs)

            For Each tsi As ToolStripItem In Items
                If Not tsi.IsOnOverflow Then
                    Continue For
                End If

                tsi.AutoSize = True
                tsi.ImageAlign = ContentAlignment.MiddleCenter
                tsi.TextAlign = ContentAlignment.MiddleCenter

                If tsi.Alignment = ToolStripItemAlignment.Right Then
                    tsi.DisplayStyle = ToolStripItemDisplayStyle.Image
                End If
            Next tsi
        End Sub

        Private Sub tsbApplyMods_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles tsbApplyMods.Click

            RaiseEvent ApplyModsClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbEnable_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles tsbEnable.Click

            RaiseEvent EnabledClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbImportPatch_Click(ByVal sender As Object, ByVal e As  _
            EventArgs) Handles tsbImportPatch.Click

            RaiseEvent ImportPatchClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbOptions_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles tsbOptions.Click

            RaiseEvent OptionsClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbPath_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles tsbPath.Click

            RaiseEvent GamePathClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbPlay_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles tsbPlay.Click

            RaiseEvent PlayClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbPlayMultiplayer_Click(ByVal sender As Object, ByVal e As  _
            EventArgs) Handles tsbPlayMultiplayer.Click

            RaiseEvent PlayMultiplayerClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) _
            Handles tsbRefresh.Click

            RaiseEvent RefreshModsClicked(Me, New EventArgs)
        End Sub

        Private Sub tsbRefreshPatches_Click(ByVal sender As Object, ByVal e As  _
            EventArgs) Handles tsbRefreshPatches.Click

            RaiseEvent RefreshPatchesClicked(Me, New EventArgs)
        End Sub
#End Region

#Region "Properties"
        Public Property ApplyModEnabled() As Boolean
            Get
                Return tsbApplyMods.Enabled
            End Get
            Set(ByVal value As Boolean)
                tsbApplyMods.Enabled = value
            End Set
        End Property

        Public Overloads Property Enabled() As Boolean
            Get
                Return _enabled
            End Get
            Set(ByVal value As Boolean)
                _enabled = value
                UpdateState()
            End Set
        End Property
        Private _enabled As Boolean = False

        Public Property GamePathDefined() As Boolean
            Get
                Return _gamePathDefined
            End Get
            Set(ByVal value As Boolean)
                _gamePathDefined = value
                UpdateState()
            End Set
        End Property
        Private _gamePathDefined As Boolean = False

        Public Property Image() As Image
            Get
                Return tslName.Image
            End Get
            Set(ByVal value As Image)
                tslName.Image = value
            End Set
        End Property

        Public Property RefreshingMods() As Boolean
            Get
                Return _refreshingMods
            End Get
            Set(ByVal value As Boolean)
                If _refreshingMods = value Then
                    Return
                End If
                _refreshingMods = value
                UpdateState()
            End Set
        End Property
        Private _refreshingMods As Boolean = False

        Public Property RefreshingPatches() As Boolean
            Get
                Return _refreshingPatches
            End Get
            Set(ByVal value As Boolean)
                If _refreshingPatches = value Then
                    Return
                End If
                _refreshingPatches = value
                UpdateState()
            End Set
        End Property
        Private _refreshingPatches As Boolean = False

        Public Property ShowMultiplayerButton() As Boolean
            Get
                Return _showMultiplayerButton
            End Get
            Set(ByVal value As Boolean)
                _showMultiplayerButton = value
                UpdateState()
            End Set
        End Property
        Private _showMultiplayerButton As Boolean = False

        Public Property ShowPatchesButton() As Boolean
            Get
                Return _showPatchesButton
            End Get
            Set(ByVal value As Boolean)
                _showPatchesButton = value
                UpdateState()
            End Set
        End Property
        Private _showPatchesButton As Boolean = False

        Public Overrides Property Text() As String
            Get
                Return MyBase.Text
            End Get
            Set(ByVal value As String)
                MyBase.Text = value
                tslName.Text = value
                UpdateState()
            End Set
        End Property
#End Region

#Region "Events"
        Public Event ApplyModsClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event EnabledClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event ExpandRequested(ByVal sender As Object, ByVal e As EventArgs)
        Public Event GamePathClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event OptionsClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event PlayClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event PlayMultiplayerClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event RefreshModsClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event RefreshPatchesClicked(ByVal sender As Object, ByVal e As EventArgs)
        Public Event ImportPatchClicked(ByVal sender As Object, ByVal e As EventArgs)
#End Region

#Region "Controls"
        Private WithEvents tslName As New ToolStripLabel("")
        Private WithEvents tsbPlay As New ToolStripButton("Play", My.Resources. _
            silk_resultset_next)
        Private WithEvents tsbPlayMultiplayer As New ToolStripButton( _
            "Play Multiplayer", My.Resources.silk_group_go)
        Private WithEvents tsbApplyMods As New ToolStripButton( _
            "Apply Mod Changes Now", My.Resources.silk_cog)
        Private WithEvents tsbOptions As New ToolStripButton("Game Options", My. _
            Resources.silk_wrench)
        Private WithEvents tsbRefresh As New ToolStripButton("Refresh List", My. _
            Resources.silk_arrow_refresh)
        Private WithEvents tsbRefreshPatches As New ToolStripButton("Refresh Patches", _
            My.Resources.silk_application_lightning)
        Private WithEvents tsbImportPatch As New ToolStripButton("Import Patched EXE", _
            My.Resources.silk_application_add)
        Private WithEvents tsbPath As New ToolStripButton("Locate Game", My.Resources. _
            silk_folder_explore)
        Private WithEvents tsbEnable As New ToolStripButton("Enable Game Support", My. _
            Resources.silk_tick)
#End Region
    End Class
End Namespace
Namespace MZZT.Knight
    Public Class ModList
        Inherits ListView

        Public Class Sorter
            Implements IComparer

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
                Implements System.Collections.IComparer.Compare

                Dim lvx As ListViewItem = x
                Dim lvy As ListViewItem = y

                If lvx.Tag IsNot Nothing AndAlso TypeOf lvx.Tag Is Games.Game.Patch Then
                    If lvy.Tag IsNot Nothing AndAlso TypeOf lvy.Tag Is Games.Game. _
                        Patch Then

                        Dim px As Games.Game.Patch = lvx.Tag
                        Dim py As Games.Game.Patch = lvy.Tag

                        If px.SortOrder <> py.SortOrder Then
                            Return px.SortOrder - py.SortOrder
                        End If

                        Return lvx.Text.CompareTo(lvy.Text)
                    Else
                        Return -1
                    End If
                ElseIf lvy.Tag IsNot Nothing AndAlso TypeOf lvy.Tag Is Games.Game.Patch _
                    Then

                    Return 1
                End If

                If lvx.Tag Is Nothing Then
                    Return -1
                End If
                If lvy.Tag Is Nothing Then
                    Return 1
                End If

                Dim modx As Games.Game.Modification = lvx.Tag
                Dim mody As Games.Game.Modification = lvy.Tag

                If modx.SortOrder <> mody.SortOrder Then
                    Return modx.SortOrder - mody.SortOrder
                End If

                Return lvx.Text.CompareTo(lvy.Text)
            End Function
        End Class

        Public Sub New()
            MyBase.New()

            AutoArrange = True
            BorderStyle = Windows.Forms.BorderStyle.None

            lvcName.Text = "Name"
            Columns.Add(lvcName)

            Dock = DockStyle.Fill
            FullRowSelect = True
            Groups.AddRange(New ListViewGroup() {lvgPatches, lvgMods})
            HeaderStyle = ColumnHeaderStyle.None
            HideSelection = False
            LabelEdit = True
            LabelWrap = True
            ListViewItemSorter = New Sorter
            MultiSelect = False
            ShowGroups = False
            ShowItemToolTips = True
            Sorting = SortOrder.Ascending
            TileSize = New Size(150, 48)
            UseCompatibleStateImageBehavior = False
            View = MyOptions.View

            AddDefaultEntry()

            tsmiIcons.ShowShortcutKeys = False
            tsmiList.ShowShortcutKeys = False

            cmsLV.Items.AddRange(New ToolStripItem() {tsmiIcons, tsmiList})

            tsmiRename.ShowShortcutKeys = False
            tsmiProperties.ShowShortcutKeys = False

            cmsLVI.Items.AddRange(New ToolStripItem() {tsmiRename, tsmiProperties})

            ec.Allow()
        End Sub

        Public Sub AddDefaultEntry()
            ec.Block()

            lviNoMods.Checked = True
            lviNoMods.Group = lvgMods
            Items.Add(lviNoMods)

            If Created Then
                AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If

            ec.Allow()
        End Sub

        Public Function GetActivatedMods() As Games.Game.Modification()
            Dim l As New List(Of Games.Game.Modification)

            For Each lvi As ListViewItem In Items
                If lvi Is Nothing Then
                    Continue For
                End If

                If lvi.Tag Is Nothing Then
                    Continue For
                End If

                If TypeOf lvi.Tag Is Games.Game.Patch Then
                    Continue For
                End If

                If lvi.Checked OrElse (Not CheckBoxes AndAlso lvi.Selected) Then
                    l.Add(lvi.Tag)
                End If
            Next lvi

            Return l.ToArray
        End Function

        Public Function GetActivatedPatches() As Games.Game.Patch()
            Dim l As New List(Of Games.Game.Patch)

            For Each lvi As ListViewItem In Items
                If lvi.Tag Is Nothing Then
                    Continue For
                End If

                If TypeOf lvi.Tag Is Games.Game.Modification Then
                    Continue For
                End If

                If lvi.Checked OrElse (Not CheckBoxes AndAlso lvi.Selected) Then
                    l.Add(lvi.Tag)
                End If
            Next lvi

            Return l.ToArray
        End Function

        Public Sub UpdateModInfo(ByVal g As Games.Game, ByVal m As Games.Game. _
            Modification, ByVal minfo As Games.ModInfoCache.Modification)

            If InvokeRequired() Then
                Invoke(New UpdateModInfoDelegate(AddressOf UpdateModInfo), g, m, minfo)
                Return
            End If

            MyModInfo(g.Name, m.UniqueID) = minfo

            For Each lvi As ListViewItem In Items
                If lvi.Tag Is m Then
                    lvi.Text = m.Name

                    If Created Then
                        AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                    End If
                    Sort()

                    Return
                End If
            Next lvi
        End Sub
        Private Delegate Sub UpdateModInfoDelegate(ByVal g As Games.Game, ByVal m As  _
            Games.Game.Modification, ByVal minfo As Games.ModInfoCache.Modification)

#Region "Event Handlers"
        Private Sub cmdLV_Opening(ByVal sender As System.Object, ByVal e As System. _
            ComponentModel.CancelEventArgs) Handles cmsLV.Opening

            tsmiIcons.Checked = View = Windows.Forms.View.LargeIcon
            tsmiList.Checked = View = Windows.Forms.View.Details
        End Sub

        Protected Overrides Sub OnAfterLabelEdit(ByVal e As System.Windows.Forms. _
            LabelEditEventArgs)

            MyBase.OnAfterLabelEdit(e)

            If e.CancelEdit OrElse e.Label = "" Then
                Return
            End If

            Dim tag As Object = Items(e.Item).Tag
            If tag Is Nothing Then
                e.CancelEdit = True
                Return
            End If

            If TypeOf tag Is Games.Game.Patch Then
                RaiseEvent PatchNameChanged(Me, New PatchNameChangedEventArgs(tag, e. _
                    Label))
            Else
                Dim m As Games.Game.Modification = tag
                RaiseEvent ModNameChanged(Me, New ModNameChangedEventArgs(m, e.Label))
            End If

            Dim t As New Timer
            t.Interval = 10
            AddHandler t.Tick, AddressOf t_Tick
            t.Start()
        End Sub

        Protected Overrides Sub OnBeforeLabelEdit(ByVal e As System.Windows.Forms. _
            LabelEditEventArgs)

            MyBase.OnBeforeLabelEdit(e)

            If Items(e.Item).Tag Is Nothing Then
                e.CancelEdit = True
            End If
        End Sub

        Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
            MyBase.OnHandleCreated(e)

            WinAPI.ListView_SetIconSpacing(Me, MyOptions.BigIconsSize)
        End Sub

        Protected Overrides Sub OnItemActivate(ByVal e As System.EventArgs)
            MyBase.OnItemActivate(e)

            If CheckBoxes Then
                Return
            End If

            If SelectedIndices.Count = 0 Then
                Return
            End If

            If TypeOf SelectedItems(0).Tag Is Games.Game.Patch Then
                Return
            End If

            RaiseEvent ModActivated(Me, New ModEventArgs(SelectedItems(0).Tag))
        End Sub

        Protected Overrides Sub OnItemCheck(ByVal ice As System.Windows.Forms. _
            ItemCheckEventArgs)

            If Not ec.EventsAllowed Then
                MyBase.OnItemCheck(ice)
                Return
            End If

            ec.Block()

            Dim lvi As ListViewItem = Items(ice.Index)
            If lvi.Tag Is Nothing Then
                ice.NewValue = CheckState.Checked
                If ice.CurrentValue = CheckState.Checked Then
                    ec.Allow()
                    MyBase.OnItemCheck(ice)
                    Return
                End If

                For i As Integer = 0 To Items.Count - 1
                    If i = ice.Index Then
                        Continue For
                    End If

                    If TypeOf Items(i).Tag Is Games.Game.Patch Then
                        Continue For
                    End If

                    Items(i).Checked = False
                Next i
            ElseIf TypeOf lvi.Tag Is Games.Game.Patch Then
                ice.NewValue = CheckState.Checked
            ElseIf ice.NewValue = CheckState.Checked Then
                lviNoMods.Checked = False
            Else
                Dim anychecked As Boolean = False
                For i As Integer = 0 To Items.Count - 1
                    If i = ice.Index Then
                        Continue For
                    End If

                    If Items(i) Is Nothing Then
                        Continue For
                    End If

                    If Items(i).Tag Is Nothing Then
                        Continue For
                    End If

                    If TypeOf Items(i).Tag Is Games.Game.Patch Then
                        Continue For
                    End If

                    If Items(i).Checked Then
                        anychecked = True
                        Exit For
                    End If
                Next i

                If Not anychecked Then
                    lviNoMods.Checked = True
                End If
            End If

            ec.Allow()

            MyBase.OnItemCheck(ice)
        End Sub

        Protected Overrides Sub OnItemChecked(ByVal e As System.Windows.Forms. _
            ItemCheckedEventArgs)

            MyBase.OnItemChecked(e)

            If Not CheckBoxes OrElse Not ec.EventsAllowed Then
                Return
            End If

            If TypeOf e.Item.Tag Is Games.Game.Patch Then
                BeginUpdate()
                For Each lvi As ListViewItem In Items
                    If lvi Is e.Item Then
                        Continue For
                    End If

                    If lvi.Tag Is Nothing Then
                        Continue For
                    End If

                    If TypeOf lvi.Tag Is Games.Game.Modification Then
                        Continue For
                    End If

                    ec.Block()
                    lvi.Checked = False
                    ec.Allow()
                Next lvi
                EndUpdate()
                RaiseEvent PatchActivatedChanged(Me, New EventArgs)
                Return
            End If

            RaiseEvent ModActivatedChanged(Me, New EventArgs)
        End Sub

        Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms. _
            MouseEventArgs)

            MyBase.OnMouseDown(e)

            If (e.Button And Windows.Forms.MouseButtons.Right) = 0 Then
                Return
            End If

            Dim lvi As ListViewItem = HitTest(e.Location).Item
            If lvi Is Nothing Then
                cmsLV.Show(Me, e.Location)
                Return
            End If

            If lvi.Tag Is Nothing Then
                Return
            End If

            tsmiProperties.Visible = TypeOf lvi.Tag Is Games.Game.Modification _
                AndAlso DirectCast(lvi.Tag, Games.Game.Modification).HasOptions

            cmsLVI.Tag = lvi
            cmsLVI.Show(Me, e.Location)
        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
            MyBase.OnSelectedIndexChanged(e)

            If CheckBoxes OrElse Not ec.EventsAllowed Then
                Return
            End If

            RaiseEvent ModActivatedChanged(Me, New EventArgs)
        End Sub

        Private Sub t_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim t As Timer = sender
            t.Stop()
            RemoveHandler t.Tick, AddressOf t_Tick

            If Created Then
                AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If

            t.Dispose()
        End Sub

        Private Sub tsmiIcons_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles tsmiIcons.Click

            MyOptions.View = Windows.Forms.View.LargeIcon
            RaiseEvent ViewChanged(Me, New EventArgs)
        End Sub

        Private Sub tsmiList_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles tsmiList.Click

            MyOptions.View = Windows.Forms.View.Details
            RaiseEvent ViewChanged(Me, New EventArgs)
        End Sub

        Private Sub tsmiRename_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles tsmiRename.Click

            Dim lvi As ListViewItem = cmsLVI.Tag
            lvi.BeginEdit()
        End Sub

        Private Sub tsmiProperties_Click(ByVal sender As System.Object, ByVal e As  _
            System.EventArgs) Handles tsmiProperties.Click

            Dim lvi As ListViewItem = cmsLVI.Tag
            DirectCast(lvi.Tag, Games.Game.Modification).ShowOptions()
        End Sub
#End Region

#Region "Properties"
        Public Property AllowMultipleMods() As Boolean
            Get
                Return CheckBoxes
            End Get
            Set(ByVal value As Boolean)
                If CheckBoxes = value Then
                    Return
                End If

                CheckBoxes = value
                HideSelection = Not value
                MultiSelect = value
            End Set
        End Property

        Public Property DefaultImageKey() As String
            Get
                Return _defaultImageKey
            End Get
            Set(ByVal value As String)
                If _defaultImageKey = value Then
                    Return
                End If

                BeginUpdate()
                If lviNoMods.ImageKey = _defaultImageKey Then
                    lviNoMods.ImageKey = value
                End If

                For Each lvi As ListViewItem In Items
                    If lvi.ImageKey = _defaultImageKey Then
                        lvi.ImageKey = value
                    End If
                Next lvi
                EndUpdate()

                _defaultImageKey = value
            End Set
        End Property
        Private _defaultImageKey As String = Nothing

        Public Overrides Property Text() As String
            Get
                Return MyBase.Text
            End Get
            Set(ByVal value As String)
                MyBase.Text = value
                lviNoMods.Text = value
            End Set
        End Property
#End Region

#Region "Events"
        Public Class ModEventArgs
            Inherits EventArgs

            Public Sub New(ByVal m As Games.Game.Modification)
                _mod = m
            End Sub

            Public ReadOnly Property Modification() As Games.Game.Modification
                Get
                    Return _mod
                End Get
            End Property
            Private _mod As Games.Game.Modification
        End Class
        Public Event ModActivated(ByVal sender As Object, ByVal e As ModEventArgs)

        Public Event ModActivatedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Public Class ModNameChangedEventArgs
            Inherits EventArgs

            Public Sub New(ByVal m As Games.Game.Modification, ByVal name As String)
                _m = m
                _name = name
            End Sub

            Public ReadOnly Property Modification() As Games.Game.Modification
                Get
                    Return _m
                End Get
            End Property
            Private _m As Games.Game.Modification

            Public ReadOnly Property Name() As String
                Get
                    Return _name
                End Get
            End Property
            Private _name As String
        End Class
        Public Event ModNameChanged(ByVal sender As Object, ByVal e As  _
            ModNameChangedEventArgs)

        Public Event PatchActivatedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Public Class PatchNameChangedEventArgs
            Inherits EventArgs

            Public Sub New(ByVal p As Games.Game.Patch, ByVal name As String)
                _p = p
                _name = name
            End Sub

            Public ReadOnly Property Patch() As Games.Game.Patch
                Get
                    Return _p
                End Get
            End Property
            Private _p As Games.Game.Patch

            Public ReadOnly Property Name() As String
                Get
                    Return _name
                End Get
            End Property
            Private _name As String
        End Class
        Public Event PatchNameChanged(ByVal sender As Object, ByVal e As  _
            PatchNameChangedEventArgs)

        Public Event ViewChanged(ByVal sender As Object, ByVal e As EventArgs)
#End Region

#Region "Controls"
        Private WithEvents cmsLV As New ContextMenuStrip()
        Private WithEvents cmsLVI As New ContextMenuStrip()

        Friend ec As New EventControl

        Private lvcName As New ColumnHeader()

        Friend lvgMods As New ListViewGroup("Mods", "Mods")
        Friend lvgPatches As New ListViewGroup("Patches", "Patches")

        Friend lviNoMods As New ListViewItem("No Mods")

        Private WithEvents tsmiIcons As New ToolStripMenuItem("&Icon View", My. _
             Resources.silk_application_view_icons)
        Private WithEvents tsmiList As New ToolStripMenuItem("&List View", My. _
             Resources.silk_application_view_list)
        Private WithEvents tsmiRename As New ToolStripMenuItem("&Rename", My.Resources. _
            silk_textfield_rename)
        Private WithEvents tsmiProperties As New ToolStripMenuItem("&Properties", My. _
            Resources.silk_application_form_edit)
#End Region
    End Class
End Namespace

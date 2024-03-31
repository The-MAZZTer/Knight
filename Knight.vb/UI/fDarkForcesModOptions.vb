Namespace MZZT.Knight.Games
    Public Class fDarkForcesModOptions
        Public Sub New(ByVal df As DarkForces, ByVal m As Game.Modification)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _df = df
            _m = m
            Icon = WinAPI.BitmapToIcon(df.Image32)
            Text = m.Name & " Options - Knight"
            ec.Allow()
        End Sub
        Private _df As DarkForces
        Private _m As Game.Modification

        Private Sub ItemChanged()
            If ec.EventsAllowed Then
                bOK.Enabled = True
                bApply.Enabled = True
                bCancel.Text = "Cancel"
            End If
        End Sub

#Region "Load/Save Settings"
        Private Sub Apply()
            MyModInfo.BeginUpdate()

            Dim m As ModInfoCache.Modification = MyModInfo(_df.Name, _m.UniqueID)
            If m Is Nothing OrElse Not TypeOf m Is DarkForcesModInfo Then
                m = New DarkForcesModInfo(_m.Name)
                m.UniqueID = _m.UniqueID
            End If
            Dim dm As DarkForcesModInfo = m

            Dim lfd As String = cbDFBRIEF.SelectedItem
            Dim index As Integer = lfd.IndexOf("<")
            If index > -1 Then
                dm.LFD = Nothing
            Else
                dm.LFD = lfd
            End If

            Dim crawl As String = cbFTEXTCRA.SelectedItem
            index = crawl.IndexOf("<")
            If index > -1 Then
                dm.Crawl = Nothing
            Else
                dm.Crawl = crawl
            End If

            dm.Others.Clear()
            For Each other As String In clbOthers.CheckedItems
                dm.Others.Add(other)
            Next other

            MyModInfo(_df.Name, _m.UniqueID) = m
            MyModInfo.EndUpdate()

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"
        End Sub

        Private Sub Revert()
            ec.Block()

            With cbDFBRIEF
                .BeginUpdate()
                .Items.Clear()
                .Items.Add(" < None >")
                .Items.AddRange(_files)
            End With

            With cbFTEXTCRA
                .BeginUpdate()
                .Items.Clear()
                .Items.Add(" < None >")
                .Items.AddRange(_files)
            End With

            With clbOthers
                .BeginUpdate()
                .Items.Clear()
                .Items.AddRange(_files)
            End With

            Dim m As ModInfoCache.Modification = MyModInfo(_df.Name, _m.UniqueID)
            If m Is Nothing OrElse Not TypeOf m Is DarkForcesModInfo Then
                cbDFBRIEF.SelectedIndex = 0
                cbFTEXTCRA.SelectedIndex = 0
            Else
                Dim dm As DarkForcesModInfo = m
                If dm.LFD IsNot Nothing AndAlso dm.LFD <> "" Then
                    Dim found As Boolean = False
                    For i As Integer = 0 To cbDFBRIEF.Items.Count - 1
                        If DirectCast(cbDFBRIEF.Items(i), String).ToLower = _
                            dm.LFD.ToLower Then

                            found = True
                            cbDFBRIEF.SelectedIndex = i
                            Exit For
                        End If
                    Next i

                    If Not found Then
                        cbDFBRIEF.Items.Add(dm.LFD)
                        cbDFBRIEF.SelectedIndex = cbDFBRIEF.Items.Count - 1
                    End If
                Else
                    cbDFBRIEF.SelectedIndex = 0
                End If

                If dm.Crawl IsNot Nothing AndAlso dm.Crawl <> "" Then
                    Dim found As Boolean = False
                    For i As Integer = 0 To cbFTEXTCRA.Items.Count - 1
                        If DirectCast(cbFTEXTCRA.Items(i), String).ToLower = _
                            dm.Crawl.ToLower Then

                            found = True
                            cbFTEXTCRA.SelectedIndex = i
                            Exit For
                        End If
                    Next i

                    If Not found Then
                        cbFTEXTCRA.Items.Add(dm.Crawl)
                        cbFTEXTCRA.SelectedIndex = cbFTEXTCRA.Items.Count - 1
                    End If
                Else
                    cbFTEXTCRA.SelectedIndex = 0
                End If

                For Each other As String In dm.Others
                    Dim found As Boolean = False
                    For i As Integer = 0 To clbOthers.Items.Count - 1
                        If DirectCast(clbOthers.Items(i), String).ToLower = other. _
                            ToLower Then

                            found = True
                            clbOthers.SetItemChecked(i, True)
                            Exit For
                        End If
                    Next i

                    If Not found Then
                        clbOthers.Items.Add(other)
                        clbOthers.SetItemChecked(clbOthers.Items.Count - 1, True)
                    End If
                Next other
            End If

            cbDFBRIEF.EndUpdate()
            cbFTEXTCRA.EndUpdate()
            clbOthers.EndUpdate()

            bOK.Enabled = False
            bApply.Enabled = False
            bCancel.Text = "Close"

            ec.Allow()
        End Sub
#End Region

#Region "Properties"
        Public Property UsableFiles() As String()
            Get
                Return _files
            End Get
            Set(ByVal value As String())
                _files = value
            End Set
        End Property
        Private _files As String() = New String() {}
#End Region

#Region "Event Handlers"
        Private Sub bApply_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bApply.Click

            Apply()
        End Sub

        Private Sub bOK_Click(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles bOK.Click

            Apply()
        End Sub

        Private Sub Changed(ByVal sender As Object, ByVal e As EventArgs) Handles _
            cbDFBRIEF.SelectedIndexChanged, cbFTEXTCRA.SelectedIndexChanged, _
            clbOthers.ItemCheck

            ItemChanged()
        End Sub

        Public Overloads Function ShowDialog() As DialogResult
            Revert()
            Return MyBase.ShowDialog()
        End Function
#End Region
    End Class
End Namespace

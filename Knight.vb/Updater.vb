Imports System.ComponentModel
Imports System.IO
Imports System.Net

Namespace MZZT
    Public Class Updater
        Private Const PercentGranularity As Integer = 1000000000
        Private Const DownloadBlockSize As Integer = 524288

        Public Sub New()
            tUpdate.Interval = 15000
        End Sub

        Protected Overrides Sub Finalize()
            If n IsNot Nothing AndAlso Not n.IsDisposed Then
                n.Close()
            End If
        End Sub

        Public Sub Abort()
            bwUpdate.CancelAsync()
        End Sub

        Public Sub DoUpdate()
            Dim updatepath As String = Path.Combine(Path.GetDirectoryName( _
                Application.ExecutablePath), _updateProcessName & ".exe")
            If Not File.Exists(updatepath) Then
                Return
            End If

            Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(updatepath)
            Dim v As Version = _version
            If v Is Nothing Then
                v = New Version(FileVersionInfo.GetVersionInfo(Application. _
                    ExecutablePath).FileVersion)
            End If
            If New Version(fvi.FileVersion) > v Then
                Process.Start(updatepath)
                Application.Exit()
            Else
                Dim c As Integer = 1
                While c > 0
                    c = Process.GetProcessesByName(_updateProcessName).Length
                    If c > 0 Then
                        Threading.Thread.Sleep(500)
                    End If
                End While

                Try
                    File.Delete(updatepath)
                Catch
                End Try

                _complete = True
            End If
        End Sub
        Private _complete As Boolean = False

        Public Sub IAmBusy()
            _busy = True

            If Not _runAfterDownload Then
                Return
            End If

            If n Is Nothing OrElse n.IsDisposed Then
                Return
            End If

            If n.ExtraControl Is Nothing Then
                Return
            End If

            If Not TypeOf n.ExtraControl Is ucNowLater Then
                Return
            End If

            n.ExtraControl.Enabled = False
        End Sub
        Private _busy As Boolean = False

        Public Sub IAmNotBusy()
            _busy = False

            If Not _runAfterDownload Then
                Return
            End If

            If n Is Nothing OrElse n.IsDisposed Then
                Return
            End If

            If n.ExtraControl Is Nothing Then
                Return
            End If

            If Not TypeOf n.ExtraControl Is ucNowLater Then
                Return
            End If

            n.ExtraControl.Enabled = True
        End Sub

        Public Function IsUpdating()
            Return bwUpdate.IsBusy
        End Function

        Public Sub ShowCompleteMessage()
            If Not _complete Then
                Return
            End If

            Notify(ReportStates.UpdateComplete, PercentGranularity, _
                "Update has been applied successfully.")
        End Sub

        Private Sub CheckForUpdate(ByVal sender As BackgroundWorker, ByVal e As  _
            DoWorkEventArgs)

            If _restartForUpdate Then
                Throw New InvalidOperationException( _
                  "Can't update now, we need to restart first!")
            End If

            Dim report As New Report
            report.State = ReportStates.CheckStarting
            report.Message = Nothing
            sender.ReportProgress(0, report)

            Dim ux As UpdateXML
            Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application. _
                ExecutablePath)
            Try
                Dim req As HttpWebRequest = HttpWebRequest.Create(_updateURL)
                req.AllowAutoRedirect = True
                req.MaximumAutomaticRedirections = 1
                req.ReadWriteTimeout = 10000
                req.Timeout = 10000
                req.UserAgent = fvi.FileDescription & "/" & fvi.FileVersion.ToString

                Dim res As HttpWebResponse = req.GetResponse()
                Dim rs As Stream = res.GetResponseStream()

                If sender.CancellationPending Then
                    e.Cancel = True

                    rs.Close()
                    rs.Dispose()
                    res.Close()

                    report.State = ReportStates.CheckAborted
                    sender.ReportProgress(0, report)
                    Return
                End If

                report.State = ReportStates.CheckProgress

                Dim ms As New MemoryStream(res.ContentLength)
                Dim b(DownloadBlockSize - 1) As Byte
                Dim read As Integer = 1
                While read > 0
                    read = rs.Read(b, 0, b.Length)
                    If read > 0 Then
                        ms.Write(b, 0, read)

                        Threading.Thread.Sleep(1)
                    End If

                    If res.ContentLength > 0 Then
                        sender.ReportProgress(Math.Min(CDec(ms.Position) / res. _
                            ContentLength * PercentGranularity, PercentGranularity), _
                            report)
                    End If

                    If sender.CancellationPending Then
                        e.Cancel = True

                        rs.Close()
                        rs.Dispose()
                        res.Close()
                        ms.Close()
                        ms.Dispose()

                        report.State = ReportStates.CheckAborted
                        sender.ReportProgress(0, report)
                        Return
                    End If
                End While

                rs.Close()
                rs.Dispose()
                res.Close()

                ms.Seek(0, IO.SeekOrigin.Begin)
                ux = ParseUpdateXML(ms)
                ms.Close()
                ms.Dispose()
            Catch ex As Exception
                report.State = ReportStates.CheckError
                report.Message = ex.Message
                sender.ReportProgress(0, report)
                Return
            End Try

            If sender.CancellationPending Then
                e.Cancel = True

                report.State = ReportStates.CheckAborted
                sender.ReportProgress(0, report)
                Return
            End If

            If ux.Product Is Nothing OrElse ux.Version Is Nothing Then
                report.State = ReportStates.CheckError
                report.Message = "Unexpected update XML format."
                sender.ReportProgress(0, report)
                Return
            End If

            Dim product As String = _productName
            If product Is Nothing Then
                product = fvi.FileDescription
            End If

            If ux.Product.ToLower <> product.ToLower Then
                report.State = ReportStates.CheckError
                report.Message = "Update file is for different application!"
                sender.ReportProgress(0, report)
                Return
            End If

            Dim myversion As Version = _version
            If myversion Is Nothing Then
                myversion = New Version(fvi.FileVersion)
            End If

            If ux.Version = myversion Then
                report.State = ReportStates.CheckComplete
                report.Message = "You are up to date."
                sender.ReportProgress(0, report)
                Return
            End If

            If ux.Version < myversion Then
                report.State = ReportStates.CheckComplete
                report.Message = "You have a newer version (" & myversion.ToString & _
                    ") than what is publicly available (" & ux.Version.ToString & ")."
                sender.ReportProgress(0, report)
                Return
            End If

            If ux.Remote Is Nothing Then
                report.State = ReportStates.UpdateError
                report.Message = "Unexpected update XML format."
                sender.ReportProgress(0, report)
                Return
            End If

            Dim localdir As String = _localDirectory
            If localdir Is Nothing Then
                localdir = Path.GetDirectoryName(Application.ExecutablePath)
            End If

            ux.Local = Path.Combine(localdir, ux.Local)

            report.State = ReportStates.UpdateStarting
            report.Message = Nothing
            sender.ReportProgress(0, report)

            Try
                Dim req As HttpWebRequest = HttpWebRequest.Create(ux.Remote)
                req.AllowAutoRedirect = True
                req.MaximumAutomaticRedirections = 1
                req.ReadWriteTimeout = 10000
                req.Timeout = 10000
                req.UserAgent = fvi.FileDescription & "/" & fvi.FileVersion.ToString

                Dim res As HttpWebResponse = req.GetResponse()
                Dim rs As Stream = res.GetResponseStream()

                If sender.CancellationPending Then
                    e.Cancel = True

                    rs.Close()
                    rs.Dispose()
                    res.Close()

                    report.State = ReportStates.UpdateAborted
                    sender.ReportProgress(0, report)
                    Return
                End If

                If _renameSuffix IsNot Nothing AndAlso File.Exists(ux.Local) Then
                    Try
                        File.Move(ux.Local, ux.Local & _renameSuffix)
                    Catch
                    End Try
                End If

                report.State = ReportStates.UpdateProgress
                report.Message = ux.Remote.ToString

                Dim fs As New FileStream(ux.Local, IO.FileMode.Create, IO.FileAccess. _
                    Write)
                Dim b(DownloadBlockSize - 1) As Byte
                Dim read As Integer = 1
                While read > 0
                    read = rs.Read(b, 0, b.Length)
                    If read > 0 Then
                        fs.Write(b, 0, read)

                        Threading.Thread.Sleep(1)
                    End If

                    If res.ContentLength > 0 Then
                        sender.ReportProgress(Math.Min(CDec(fs.Position) / res. _
                            ContentLength * PercentGranularity, PercentGranularity), _
                            report)
                    End If

                    If sender.CancellationPending Then
                        e.Cancel = True

                        rs.Close()
                        rs.Dispose()
                        res.Close()
                        fs.Close()
                        fs.Dispose()
                        Try
                            File.Delete(ux.Local)
                        Catch
                        End Try

                        If _renameSuffix IsNot Nothing AndAlso File.Exists(ux.Local & _
                            _renameSuffix) Then

                            Try
                                File.Move(ux.Local & _renameSuffix, ux.Local)
                            Catch
                            End Try
                        End If

                        report.State = ReportStates.UpdateAborted
                        report.Message = Nothing
                        sender.ReportProgress(0, report)
                        Return
                    End If
                End While

                rs.Close()
                rs.Dispose()
                res.Close()

                fs.Close()
                fs.Dispose()
            Catch ex As Exception
                Try
                    File.Delete(ux.Local)
                Catch
                End Try

                If _renameSuffix IsNot Nothing AndAlso File.Exists(ux.Local & _
                    _renameSuffix) Then

                    Try
                        File.Move(ux.Local & _renameSuffix, ux.Local)
                    Catch
                    End Try
                End If

                report.State = ReportStates.UpdateError
                report.Message = ex.Message
                sender.ReportProgress(0, report)
                Return
            End Try

            If sender.CancellationPending Then
                e.Cancel = True

                Try
                    File.Delete(ux.Local)
                Catch
                End Try

                If _renameSuffix IsNot Nothing AndAlso File.Exists(ux.Local & _
                    _renameSuffix) Then

                    Try
                        File.Move(ux.Local & _renameSuffix, ux.Local)
                    Catch
                    End Try
                End If

                report.State = ReportStates.UpdateAborted
                report.Message = Nothing
                sender.ReportProgress(0, report)
                Return
            End If

            If _runAfterDownload Then
                _updateFile = ux.Local

                If _useBalloons Then
                    report.State = ReportStates.UpdateLater
                    report.Message = Nothing
                    sender.ReportProgress(PercentGranularity, report)
                    _restartForUpdate = True
                    Return
                End If

                report.State = ReportStates.UpdatePrompt
                report.Message = _updatePromptMessage2
            Else
                report.State = ReportStates.UpdateComplete
                report.Message = Nothing
            End If
            sender.ReportProgress(PercentGranularity, report)
        End Sub
        Private _updateFile As String = Nothing

        Private Sub Notify(ByVal state As ReportStates, ByVal progress As Integer, _
            ByVal text As String)

            If Not _useBalloons Then
                If n Is Nothing OrElse n.IsDisposed Then
                    n = New Notification
                    With n
                        .AutoClose = False
                        .ShowTime = -1
                        .TransitionTime = _notificationTransitionTime

                        If Not _notificationSystemTransition Then
                            .Fade = _notificationFade
                            .Slide = _notificationSlide
                            .SlideWhenOtherNotificationCloses = _notificationSlide
                        End If
                    End With
                ElseIf n.InvokeRequired Then
                    n.Invoke(New NotifyDelegate(AddressOf Notify), state, progress, _
                        text)
                    Return
                End If
            End If

            Dim title As String = Nothing
            Select Case state
                Case ReportStates.CheckStarting, ReportStates.CheckProgress
                    title = _checkingMessage
                Case ReportStates.CheckAborted
                    title = "Update check aborted"
                Case ReportStates.CheckError
                    title = _checkFailedMessage
                Case ReportStates.CheckComplete
                    title = _checkCompleteMessage
                Case ReportStates.UpdateStarting, ReportStates.UpdateProgress
                    title = _updateMessage
                Case ReportStates.UpdateAborted
                    title = "Update aborted"
                Case ReportStates.UpdateError
                    title = _updateFailedMessage
                Case ReportStates.UpdatePrompt
                    title = _updatePromptMessage
                Case ReportStates.UpdateInstalling
                    title = "Installing update..."
                Case ReportStates.UpdateLater
                    title = "Update deferred until program restart."
                Case ReportStates.UpdateComplete
                    title = _updateCompleteMessage
            End Select

            If _useBalloons Then
                Dim tti As ToolTipIcon = ToolTipIcon.Info
                Select Case state
                    Case ReportStates.CheckError, ReportStates.UpdateError
                        tti = ToolTipIcon.Error
                    Case ReportStates.CheckAborted, ReportStates.UpdateAborted
                        tti = ToolTipIcon.Warning
                    Case ReportStates.CheckProgress, ReportStates.UpdateProgress
                        title &= " (" & Math.Round(CDec(progress) / PercentGranularity _
                            * 100).ToString & "%)"
                End Select

                If text Is Nothing OrElse text = "" Then
                    text = " "
                End If

                _balloonIcon.ShowBalloonTip(_notificationShowTime, title, text, tti)
                Return
            End If

            If text Is Nothing OrElse text = "" Then
                n.Rtf = "{\rtf\ansi\deff{\b " & title & "}}"
            Else
                n.Rtf = "{\rtf\ansi\deff{\b " & title & "}\par " & text & "}"
            End If

            Select Case state
                Case ReportStates.CheckStarting, ReportStates.UpdateStarting
                    With n
                        Dim ucu As New ucUpload
                        .ExtraControl = ucu
                        AddHandler ucu.CancelClicked, AddressOf AbortUpdate

                        With ucu
                            .Maximum = PercentGranularity
                            .Value = 0
                        End With
                        .Image = ScaleBitmap(My.Resources.silk_world, 4)
                        .ShowTime = -1
                        .Show()
                    End With
                Case ReportStates.CheckAborted, ReportStates.UpdateAborted
                    If n.ExtraControl IsNot Nothing Then
                        If TypeOf n.ExtraControl Is ucUpload Then
                            Dim ucu As ucUpload = n.ExtraControl
                            RemoveHandler ucu.CancelClicked, AddressOf AbortUpdate
                        ElseIf TypeOf n.ExtraControl Is ucNowLater Then
                            Dim ucnl As ucNowLater = n.ExtraControl
                            RemoveHandler ucnl.NowClicked, AddressOf ucnl_NowClicked
                            RemoveHandler ucnl.LaterClicked, AddressOf ucnl_LaterClicked
                        End If
                    End If

                    With n
                        .Image = ScaleBitmap(My.Resources.silk_cross, 4)
                        .ExtraControl = Nothing
                        .Show()
                        .Hide(_notificationShowTime)
                    End With
                Case ReportStates.CheckError, ReportStates.UpdateError
                    If n.ExtraControl IsNot Nothing Then
                        If TypeOf n.ExtraControl Is ucUpload Then
                            Dim ucu As ucUpload = n.ExtraControl
                            RemoveHandler ucu.CancelClicked, AddressOf AbortUpdate
                        ElseIf TypeOf n.ExtraControl Is ucNowLater Then
                            Dim ucnl As ucNowLater = n.ExtraControl
                            RemoveHandler ucnl.NowClicked, AddressOf ucnl_NowClicked
                            RemoveHandler ucnl.LaterClicked, AddressOf ucnl_LaterClicked
                        End If
                    End If

                    With n
                        .Image = ScaleBitmap(My.Resources.silk_exclamation, 4)
                        .ExtraControl = Nothing
                        .Show()
                        .Hide(_notificationShowTime)
                    End With
                Case ReportStates.CheckProgress, ReportStates.UpdateProgress
                    With n
                        If .ExtraControl IsNot Nothing AndAlso TypeOf .ExtraControl _
                            Is ucUpload Then

                            Dim ucu As ucUpload = .ExtraControl
                            ucu.Value = progress
                        End If
                    End With
                Case ReportStates.UpdatePrompt
                    With n
                        Dim ucnl As New ucNowLater
                        ucnl.Enabled = Not _busy
                        AddHandler ucnl.NowClicked, AddressOf ucnl_NowClicked
                        AddHandler ucnl.LaterClicked, AddressOf ucnl_LaterClicked
                        .ExtraControl = ucnl

                        .Image = ScaleBitmap(My.Resources.silk_help, 4)
                        .ShowTime = -1
                        .Show()
                    End With
                Case ReportStates.CheckComplete, ReportStates.UpdateInstalling, _
                    ReportStates.UpdateComplete, ReportStates.UpdateLater

                    If n.ExtraControl IsNot Nothing Then
                        If TypeOf n.ExtraControl Is ucUpload Then
                            Dim ucu As ucUpload = n.ExtraControl
                            RemoveHandler ucu.CancelClicked, AddressOf AbortUpdate
                        ElseIf TypeOf n.ExtraControl Is ucNowLater Then
                            Dim ucnl As ucNowLater = n.ExtraControl
                            RemoveHandler ucnl.NowClicked, AddressOf ucnl_NowClicked
                            RemoveHandler ucnl.LaterClicked, AddressOf ucnl_LaterClicked
                        End If
                    End If

                    With n
                        If state = ReportStates.UpdateLater Then
                            .Image = ScaleBitmap(My.Resources.silk_clock, 4)
                        Else
                            .Image = ScaleBitmap(My.Resources.silk_tick, 4)
                        End If
                        .ExtraControl = Nothing
                        .Show()
                        .Hide(_notificationShowTime)
                    End With
            End Select
        End Sub
        Private Delegate Sub NotifyDelegate(ByVal state As ReportStates, ByVal _
            progress As Integer, ByVal text As String)
        Private n As Notification = Nothing

        Private Function ParseUpdateXML(ByVal s As IO.Stream) As UpdateXML
            Dim x As New Xml.XmlDocument
            x.Load(s)

            Dim ux As New UpdateXML
            ux.Product = Nothing
            ux.Version = Nothing
            ux.Remote = Nothing
            ux.Local = Nothing

            Dim update As Xml.XmlElement = Nothing
            For Each xn As Xml.XmlNode In x.ChildNodes
                If TypeOf xn Is Xml.XmlElement AndAlso xn.Name.ToLower = "update" Then
                    update = xn
                    Exit For
                End If
            Next xn

            If update Is Nothing Then
                Return ux
            End If

            Dim download As Xml.XmlElement = Nothing
            For Each xn As Xml.XmlNode In update.ChildNodes
                If Not TypeOf xn Is Xml.XmlElement Then
                    Continue For
                End If

                If xn.Name.ToLower = "product" Then
                    For Each xn2 As Xml.XmlNode In xn.ChildNodes
                        If TypeOf xn2 Is Xml.XmlText Then
                            ux.Product = xn2.Value
                            Exit For
                        End If
                    Next xn2
                ElseIf xn.Name.ToLower = "version" Then
                    For Each xn2 As Xml.XmlNode In xn.ChildNodes
                        If TypeOf xn2 Is Xml.XmlText Then
                            Try
                                ux.Version = New Version(xn2.Value)
                            Catch
                                Continue For
                            End Try
                            Exit For
                        End If
                    Next xn2
                ElseIf xn.Name.ToLower = "download" Then
                    download = xn
                End If
            Next xn

            If download Is Nothing Then
                Return ux
            End If

            For Each xn As Xml.XmlNode In download.ChildNodes
                If Not TypeOf xn Is Xml.XmlElement Then
                    Continue For
                End If

                If xn.Name.ToLower = "remote" Then
                    For Each xn2 As Xml.XmlNode In xn.ChildNodes
                        If TypeOf xn2 Is Xml.XmlText Then
                            If Not Uri.TryCreate(xn2.Value, UriKind.Absolute, _
                                ux.Remote) Then

                                Continue For
                            End If
                            Exit For
                        End If
                    Next xn2
                ElseIf xn.Name.ToLower = "local" Then
                    For Each xn2 As Xml.XmlNode In xn.ChildNodes
                        If TypeOf xn2 Is Xml.XmlText Then
                            ux.Local = xn2.Value
                            Exit For
                        End If
                    Next xn2
                End If
            Next xn

            If ux.Remote Is Nothing Then
                Return ux
            End If

            If ux.Local Is Nothing Then
                If _updateProcessName Is Nothing OrElse Not _runAfterDownload Then
                    Dim filename As String = ux.Remote.PathAndQuery
                    Dim delimit As Integer = filename.IndexOfAny("#?")
                    If delimit > -1 Then
                        filename = filename.Substring(0, delimit)
                    End If
                    delimit = filename.LastIndexOfAny("\/")
                    If delimit > -1 Then
                        filename = filename.Substring(delimit + 1)
                    End If
                    ux.Local = Uri.UnescapeDataString(filename)
                Else
                    ux.Local = _updateProcessName & ".exe"
                End If
            End If

            Return ux
        End Function

        Private Function ScaleBitmap(ByVal oldb As Bitmap, ByVal scale As Integer) As  _
            Bitmap

            If scale <= 1 Then
                Throw New ArgumentOutOfRangeException("scale", scale, _
                    "scale must be >= 2")
            End If

            Dim b As New Bitmap(oldb.Width * scale, oldb.Height * scale, Imaging. _
                PixelFormat.Format32bppArgb)
            Dim g As Graphics = Graphics.FromImage(b)

			Dim oldbd As Imaging.BitmapData = oldb.LockBits(New Rectangle(Point.Empty,
					oldb.Size), Imaging.ImageLockMode.ReadOnly, Imaging.PixelFormat.
					Format32bppArgb)
			Dim bd As Imaging.BitmapData = b.LockBits(New Rectangle(Point.Empty,
				b.Size), Imaging.ImageLockMode.WriteOnly, b.PixelFormat)

			For j As Integer = 0 To oldb.Height - 1
				For i As Integer = 0 To oldb.Width - 1
					Dim ip As New IntPtr(oldbd.Scan0.ToInt64 + (j * oldbd.Stride) + (i * 4))
					Dim c As Integer = Runtime.InteropServices.Marshal.ReadInt32(ip)

					For k As Integer = 0 To scale - 1
						ip = New IntPtr(bd.Scan0.ToInt64 + (((j * scale) + k) * bd.Stride) + (i * 4 * scale))
						For l As Integer = 0 To scale - 1
							Runtime.InteropServices.Marshal.WriteInt32(ip, l * 4, c)
						Next l
					Next k
				Next i
			Next j

			oldb.UnlockBits(oldbd)
            b.UnlockBits(bd)

            g.Dispose()
            oldb.Dispose()

            Return b
        End Function

        Public Sub StartAutoCheckTimer()
            tUpdate.Start()
        End Sub

        Public Sub Update(ByVal async As Boolean)
            tUpdate.Stop()

            With bwUpdate
                .WorkerReportsProgress = True
                .WorkerSupportsCancellation = True
                .RunWorkerAsync()
            End With

            If Not async Then
                While bwUpdate.IsBusy
                    Threading.Thread.Sleep(100)
                End While
            End If
        End Sub

        Public Property AutoCheckDelay() As Integer
            Get
                Return tUpdate.Interval
            End Get
            Set(ByVal value As Integer)
                tUpdate.Interval = value
            End Set
        End Property

        Public Property BalloonIcon() As NotifyIcon
            Get
                Return _balloonIcon
            End Get
            Set(ByVal value As NotifyIcon)
                _balloonIcon = value
            End Set
        End Property
        Private _balloonIcon As NotifyIcon = Nothing

        Public Property CheckCompleteMessage() As String
            Get
                Return _checkCompleteMessage
            End Get
            Set(ByVal value As String)
                _checkCompleteMessage = value
            End Set
        End Property
        Private _checkCompleteMessage As String = "Update check complete"

        Public Property CheckFailedMessage() As String
            Get
                Return _checkFailedMessage
            End Get
            Set(ByVal value As String)
                _checkFailedMessage = value
            End Set
        End Property
        Private _checkFailedMessage As String = "Update check failed"

        Public Property CheckingMessage() As String
            Get
                Return _checkingMessage
            End Get
            Set(ByVal value As String)
                _checkingMessage = value
            End Set
        End Property
        Private _checkingMessage As String = "Checking for update..."

        Public Property CurrentVersion() As Version
            Get
                Return _version
            End Get
            Set(ByVal value As Version)
                _version = value
            End Set
        End Property
        Private _version As Version = Nothing

        Public Property ExistingLocalFileRenameSuffix() As String
            Get
                Return _renameSuffix
            End Get
            Set(ByVal value As String)
                _renameSuffix = value
            End Set
        End Property
        Private _renameSuffix As String = Nothing

        Public Property LocalDirectory() As String
            Get
                Return _localDirectory
            End Get
            Set(ByVal value As String)
                _localDirectory = value
            End Set
        End Property
        Private _localDirectory As String = Nothing

        Public Property NotificationFade() As Boolean
            Get
                Return _notificationFade
            End Get
            Set(ByVal value As Boolean)
                _notificationFade = value

                If n IsNot Nothing AndAlso Not n.IsDisposed Then
                    n.Fade = value
                End If
            End Set
        End Property
        Private _notificationFade As Boolean = True

        Public Property NotificationShowTime() As Integer
            Get
                Return _notificationShowTime
            End Get
            Set(ByVal value As Integer)
                _notificationShowTime = value
            End Set
        End Property
        Private _notificationShowTime As Integer = 5000

        Public Property NotificationSlide() As Boolean
            Get
                Return _notificationSlide
            End Get
            Set(ByVal value As Boolean)
                _notificationSlide = value

                If n IsNot Nothing AndAlso Not n.IsDisposed Then
                    n.Slide = value
                    n.SlideWhenOtherNotificationCloses = value
                End If
            End Set
        End Property
        Private _notificationSlide As Boolean = True

        Public Property NotificationSystemTransition() As Boolean
            Get
                Return _notificationSystemTransition
            End Get
            Set(ByVal value As Boolean)
                _notificationSystemTransition = value

                If value Then
                    If n IsNot Nothing AndAlso Not n.IsDisposed Then
                        n.Fade = SystemInformation.UIEffectsEnabled AndAlso _
                            SystemInformation.IsMenuAnimationEnabled AndAlso _
                            SystemInformation.IsMenuFadeEnabled
                        n.Slide = SystemInformation.UIEffectsEnabled AndAlso _
                            SystemInformation.IsMenuAnimationEnabled AndAlso Not _
                            SystemInformation.IsMenuFadeEnabled
                        n.SlideWhenOtherNotificationCloses = SystemInformation. _
                            UIEffectsEnabled AndAlso SystemInformation. _
                            IsMenuAnimationEnabled AndAlso Not SystemInformation. _
                            IsMenuFadeEnabled
                    End If
                Else
                    If n IsNot Nothing AndAlso Not n.IsDisposed Then
                        n.Fade = _notificationFade
                        n.Slide = _notificationSlide
                        n.SlideWhenOtherNotificationCloses = _notificationSlide
                    End If
                End If
            End Set
        End Property
        Private _notificationSystemTransition As Boolean = True

        Public Property NotificationTransitionTime() As Integer
            Get
                Return _notificationTransitionTime
            End Get
            Set(ByVal value As Integer)
                _notificationTransitionTime = value
            End Set
        End Property
        Private _notificationTransitionTime As Integer = 500

        Public Property ProductName() As String
            Get
                Return _productName
            End Get
            Set(ByVal value As String)
                _productName = value
            End Set
        End Property
        Private _productName As String = Nothing

        Public ReadOnly Property RestartForUpdate() As Boolean
            Get
                Return _restartForUpdate
            End Get
        End Property
        Private _restartForUpdate As Boolean = False

        Public Property RunAfterDownload() As Boolean
            Get
                Return _runAfterDownload
            End Get
            Set(ByVal value As Boolean)
                _runAfterDownload = value
            End Set
        End Property
        Private _runAfterDownload As Boolean = True

        Public Property ShowNotifications() As Boolean
            Get
                Return _showNotifications
            End Get
            Set(ByVal value As Boolean)
                _showNotifications = value
            End Set
        End Property
        Private _showNotifications As Boolean = True

        Public Property UpdateCompleteMessage() As String
            Get
                Return _updateCompleteMessage
            End Get
            Set(ByVal value As String)
                _updateCompleteMessage = value
            End Set
        End Property
        Private _updateCompleteMessage As String = "Update complete"

        Public Property UpdateFailedMessage() As String
            Get
                Return _updateFailedMessage
            End Get
            Set(ByVal value As String)
                _updateFailedMessage = value
            End Set
        End Property
        Private _updateFailedMessage As String = "Update failed"

        Public Property UpdateMessage() As String
            Get
                Return _updateMessage
            End Get
            Set(ByVal value As String)
                _updateMessage = value
            End Set
        End Property
        Private _updateMessage As String = "Downloading update..."

        Public Property UpdateProcessName() As String
            Get
                Return _updateProcessName
            End Get
            Set(ByVal value As String)
                _updateProcessName = value
            End Set
        End Property
        Private _updateProcessName As String = Nothing

        Public Property UpdatePromptMessage() As String
            Get
                Return _updatePromptMessage
            End Get
            Set(ByVal value As String)
                _updatePromptMessage = value
            End Set
        End Property
        Private _updatePromptMessage As String = "Apply update now?"

        Public Property UpdatePromptMessage2() As String
            Get
                Return _updatePromptMessage2
            End Get
            Set(ByVal value As String)
                _updatePromptMessage2 = value
            End Set
        End Property
        Private _updatePromptMessage2 As String = _
            "Restart now, or restart the program yourself later."

        Public Property UpdateURL() As Uri
            Get
                Return _updateURL
            End Get
            Set(ByVal value As Uri)
                _updateURL = value
            End Set
        End Property
        Private _updateURL As Uri = Nothing

        Public Property UseBalloons() As Boolean
            Get
                Return _useBalloons
            End Get
            Set(ByVal value As Boolean)
                _useBalloons = value
            End Set
        End Property
        Private _useBalloons As Boolean = False

        Public Enum ReportStates
            CheckStarting
            CheckProgress
            CheckAborted
            CheckError
            CheckComplete
            UpdateStarting
            UpdateProgress
            UpdateAborted
            UpdateError
            UpdatePrompt
            UpdateInstalling
            UpdateLater
            UpdateComplete
            Complete
        End Enum

        Private Structure Report
            Public State As ReportStates
            Public Message As String
        End Structure

        Private Structure UpdateXML
            Public Product As String
            Public Version As Version
            Public Remote As Uri
            Public Local As String
        End Structure

#Region "Event Handlers"
        Private Sub AbortUpdate(ByVal sender As Object, ByVal e As EventArgs)
            Abort()
        End Sub

        Private Sub bwUpdate_DoWork(ByVal sender As Object, ByVal e As System. _
            ComponentModel.DoWorkEventArgs) Handles bwUpdate.DoWork

            CheckForUpdate(sender, e)
        End Sub

        Private Sub bwUpdate_ProgressChanged(ByVal sender As Object, ByVal e As  _
            System.ComponentModel.ProgressChangedEventArgs) Handles bwUpdate. _
            ProgressChanged

            Dim report As Report = e.UserState
            If _showNotifications Then
                Notify(report.State, e.ProgressPercentage, report.Message)
            End If
            RaiseEvent StatusChanged(Me, New StatusChangedEventArgs(report.State, _
                e.ProgressPercentage, report.Message))
        End Sub

        Private Sub bwUpdate_RunWorkerCompleted(ByVal sender As Object, ByVal e As  _
            System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwUpdate. _
            RunWorkerCompleted

            If e.Error Is Nothing Then
                RaiseEvent StatusChanged(Me, New StatusChangedEventArgs(ReportStates. _
                    Complete, -1, Nothing))
            Else
                RaiseEvent StatusChanged(Me, New StatusChangedEventArgs(ReportStates. _
                    Complete, -1, e.Error.Message))
            End If
        End Sub

        Private Sub ucnl_NowClicked(ByVal sender As Object, ByVal e As System.EventArgs)
            Notify(ReportStates.UpdateInstalling, PercentGranularity, Nothing)

            Process.Start(_updateFile)
            Application.Exit()
        End Sub

        Private Sub ucnl_LaterClicked(ByVal sender As Object, ByVal e As System. _
            EventArgs)

            Notify(ReportStates.UpdateLater, PercentGranularity, Nothing)
            _restartForUpdate = True
        End Sub

        Private Sub tUpdate_Tick(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles tUpdate.Tick

            Dim ea As New CancelEventArgs(False)
            RaiseEvent AutoCheckFired(Me, ea)

            If Not ea.Cancel Then
                Update(True)
            End If
        End Sub
#End Region

#Region "Events"
        Public Event AutoCheckFired(ByVal sender As Object, ByVal e As CancelEventArgs)
        Public Event CanApplyNow(ByVal sender As Object, ByVal e As CancelEventArgs)

        Public Class StatusChangedEventArgs
            Inherits EventArgs

            Public Sub New(ByVal state As ReportStates, ByVal progress As _
                Integer, ByVal text As String)

                _state = state
                _progress = progress
                _text = text
            End Sub

            Public ReadOnly Property Progress() As Integer
                Get
                    Return _progress
                End Get
            End Property
            Private _progress As Integer

            Public ReadOnly Property State() As ReportStates
                Get
                    Return _state
                End Get
            End Property
            Private _state As ReportStates

            Public ReadOnly Property Text() As String
                Get
                    Return _text
                End Get
            End Property
            Private _text As String
        End Class
        Public Event StatusChanged(ByVal sender As Object, ByVal e As  _
            StatusChangedEventArgs)
#End Region

        Private WithEvents tUpdate As New Timer
        Private WithEvents bwUpdate As New BackgroundWorker
    End Class
End Namespace
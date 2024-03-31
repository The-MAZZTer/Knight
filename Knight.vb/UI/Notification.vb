Namespace MZZT
    Public Class Notification
        Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            tAnim.Interval = AnimationGranularity
        End Sub

        'Form Overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents t As System.Windows.Forms.RichTextBox
        Friend WithEvents pic As System.Windows.Forms.PictureBox
        Friend WithEvents tShow As System.Windows.Forms.Timer
        Friend WithEvents tClose As System.Windows.Forms.Timer
        Friend WithEvents tAnim As System.Windows.Forms.Timer
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Me.pic = New System.Windows.Forms.PictureBox
            Me.t = New System.Windows.Forms.RichTextBox
            Me.tAnim = New System.Windows.Forms.Timer(Me.components)
            Me.tShow = New System.Windows.Forms.Timer(Me.components)
            Me.tClose = New System.Windows.Forms.Timer(Me.components)
            CType(Me.pic, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pic
            '
            Me.pic.Location = New System.Drawing.Point(12, 12)
            Me.pic.Name = "pic"
            Me.pic.Size = New System.Drawing.Size(48, 48)
            Me.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.pic.TabIndex = 0
            Me.pic.TabStop = False
            '
            't
            '
            Me.t.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.t.BackColor = System.Drawing.SystemColors.Control
            Me.t.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.t.Location = New System.Drawing.Point(66, 12)
            Me.t.Name = "t"
            Me.t.ReadOnly = True
            Me.t.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
            Me.t.Size = New System.Drawing.Size(262, 64)
            Me.t.TabIndex = 0
            Me.t.Text = ""
            '
            'tAnim
            '
            Me.tAnim.Interval = 10
            '
            'tShow
            '
            '
            'Notification
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 14)
            Me.ClientSize = New System.Drawing.Size(340, 88)
            Me.ControlBox = False
            Me.Controls.Add(Me.t)
            Me.Controls.Add(Me.pic)
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "Notification"
            Me.Opacity = 0
            Me.ShowInTaskbar = False
            Me.TopMost = True
            Me.TransparencyKey = System.Drawing.Color.Magenta
            CType(Me.pic, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private Const AnimationGranularity As Integer = 10

		Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.
						CreateParams

			Get
				Dim cp As CreateParams = MyBase.CreateParams
				cp.ExStyle = cp.ExStyle Or WS_EX.NOACTIVATE 'Or WS_EX.TRANSPARENT
				Return cp
			End Get
		End Property

#Region "Taskbar Support"
		Private Shared Function GetTaskbarRect() As Rectangle
            Dim taskbar As IntPtr = FindWindow("Shell_TrayWnd", Nothing)
            If taskbar = IntPtr.Zero Then
                Return Nothing
            End If

            Dim rect As RECT
            If GetWindowRect(taskbar, rect) = 0 Then
                Return Nothing
            End If

            Try
                Return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom)
            Catch
                Return Nothing
            End Try
        End Function

        Private Shared Function GetTaskbarInfo(ByVal taskbarrect As Rectangle) As  _
            TaskbarInfo

            Dim variance As Integer = Integer.MaxValue
            Dim tbi As New TaskbarInfo

            For Each s As Screen In Screen.AllScreens
                Dim v As Integer = Math.Abs(taskbarrect.Bottom - s.Bounds.Bottom) + _
                    Math.Abs(taskbarrect.Left - s.Bounds.Left) + _
                    Math.Abs(taskbarrect.Right - s.Bounds.Right)
                If v < variance Then
                    variance = v
                    tbi.Screen = s
                    tbi.Dock = DockStyle.Bottom
                    tbi.Size = s.Bounds.Bottom - taskbarrect.Top + 1
                End If

                v = Math.Abs(taskbarrect.Top - s.Bounds.Top) + _
                    Math.Abs(taskbarrect.Left - s.Bounds.Left) + _
                    Math.Abs(taskbarrect.Right - s.Bounds.Right)
                If v < variance Then
                    variance = v
                    tbi.Screen = s
                    tbi.Dock = DockStyle.Top
                    tbi.Size = taskbarrect.Bottom - s.Bounds.Top + 1
                End If

                v = Math.Abs(taskbarrect.Top - s.Bounds.Top) + _
                    Math.Abs(taskbarrect.Left - s.Bounds.Left) + _
                    Math.Abs(taskbarrect.Bottom - s.Bounds.Bottom)
                If v < variance Then
                    variance = v
                    tbi.Screen = s
                    tbi.Dock = DockStyle.Left
                    tbi.Size = taskbarrect.Right - s.Bounds.Left + 1
                End If

                v = Math.Abs(taskbarrect.Top - s.Bounds.Top) + _
                    Math.Abs(taskbarrect.Bottom - s.Bounds.Bottom) + _
                    Math.Abs(taskbarrect.Right - s.Bounds.Right)
                If v < variance Then
                    variance = v
                    tbi.Screen = s
                    tbi.Dock = DockStyle.Right
                    tbi.Size = s.Bounds.Right - taskbarrect.Left + 1
                End If
            Next s
            Return tbi
        End Function

        Private Structure TaskbarInfo
            Public Screen As Screen
            Public Dock As DockStyle
            Public Size As Integer
        End Structure
#End Region

#Region "Animation"
        Private Sub Animate(ByVal endpoint As Point, ByVal endsize As Size, _
            ByVal endopacity As Double, ByVal time As TimeSpan)

            _startpoint = Location
            _deltapoint = endpoint - _startpoint
            _startsize = Size
            _deltasize = endsize - _startsize
            _startopacity = Opacity
            _deltaopacity = endopacity - _startopacity
            _starttime = Date.Now
            _time = time

            tAnim.Enabled = True
        End Sub
        Private _startpoint As Point
        Private _deltapoint As Point
        Private _startsize As Size
        Private _deltasize As Size
        Private _startopacity As Double
        Private _deltaopacity As Double
        Private _starttime As Date
        Private _time As TimeSpan

        Private Sub tAnim_Tick(ByVal sender As System.Object, ByVal e As System. _
            EventArgs) Handles tAnim.Tick

            tAnim.Enabled = False

            Dim pos As Single = CSng((Date.Now - _starttime).Ticks) / _
                _time.Ticks
            If pos > 1 Then
                pos = 1
            End If

            Dim p As New Point
            p.X = (_deltapoint.X * pos) + _startpoint.X
            p.Y = (_deltapoint.Y * pos) + _startpoint.Y

            Dim s As New Size
            s.Width = (_deltasize.Width * pos) + _startsize.Width
            s.Height = (_deltasize.Height * pos) + _startsize.Height

            Dim o As Double = (_deltaopacity * pos) + _startopacity

            Location = p
            Size = s
            Opacity = o

            If pos < 1 Then
                tAnim.Enabled = True
            End If
        End Sub

        Public Overloads Sub Show()
            If _hidden Then
                tAnim.Enabled = False

                _instance = _totalinstances
                _totalinstances += 1

                Dim notifications As New ArrayList
                For Each f As Form In Application.OpenForms
                    If TypeOf f Is MZZT.Notification AndAlso f IsNot Me AndAlso _
                        DirectCast(f, MZZT.Notification).DisplayPoint <> Nothing Then

                        If DirectCast(f, Notification)._hidden Then
                            Continue For
                        End If

                        notifications.Add(f)
                    End If
                Next f
                Dim offsetdir As DockStyle = DockStyle.None

                Dim tbarea As Rectangle = GetTaskbarRect()
                If tbarea = Nothing Then
                    Dim bounds As Rectangle = Screen.PrimaryScreen.Bounds
                    offscreen = New Point(bounds.Right - Width, bounds.Bottom + 1)
                    _displayPoint = New Point(bounds.Right - Width, bounds.Bottom - _
                        Height)
                    offsetdir = DockStyle.Top
                Else
                    Dim tbi As TaskbarInfo = GetTaskbarInfo(tbarea)
                    Dim bounds As Rectangle = tbi.Screen.Bounds
                    Select Case tbi.Dock
                        Case DockStyle.Bottom
                            offscreen = New Point(bounds.Right - Width, bounds. _
                                Bottom + 1)
                            _displayPoint = New Point(bounds.Right - Width, tbarea.Top _
                                - Height)
                            offsetdir = DockStyle.Top
                        Case DockStyle.Top
                            offscreen = New Point(bounds.Right - Width, bounds.Top - _
                                Height)
                            _displayPoint = New Point(bounds.Right - Width, _
                                tbarea.Bottom + 1)
                            offsetdir = DockStyle.Bottom
                        Case DockStyle.Left
                            offscreen = New Point(bounds.Left - Width, bounds.Bottom _
                                - Height)
                            _displayPoint = New Point(tbarea.Right + 1, bounds.Bottom _
                                - Height)
                            offsetdir = DockStyle.Right
                        Case DockStyle.Right
                            offscreen = New Point(bounds.Right + 1, bounds.Bottom - _
                                Height)
                            _displayPoint = New Point(tbarea.Left - Width, _
                                bounds.Bottom - Height)
                            offsetdir = DockStyle.Left
                    End Select
                End If

                Dim largest As Notification = Nothing
                For Each n As Notification In notifications
                    If largest Is Nothing OrElse largest.Instance < n.Instance Then
                        largest = n
                    End If
                Next n

                If largest IsNot Nothing Then
                    Select Case offsetdir
                        Case DockStyle.Bottom
                            _displayPoint.Y = largest.DisplayPoint.Y + largest.Height
                        Case DockStyle.Left
                            _displayPoint.X = largest.DisplayPoint.X - Width
                        Case DockStyle.Right
                            _displayPoint.X = largest.DisplayPoint.X + largest.Width
                        Case DockStyle.Top
                            _displayPoint.Y = largest.DisplayPoint.Y - Height
                    End Select
                End If

                Location = IIf(_slide, offscreen, _displayPoint)
                Opacity = IIf(_fade, 0, 1)
                _hidden = False
            Else
                If Not _slide Then
                    Location = _displayPoint
                End If
                If Not _fade Then
                    Opacity = 1
                End If
            End If
            If _slide OrElse _fade Then
                Animate(_displayPoint, Size, 1, New TimeSpan(0, 0, 0, 0, _fadetime))
            End If

            If _showtime > 0 Then
                tShow.Interval = _showtime + IIf(_fade OrElse _slide, _fadetime, 0)
                tShow.Start()
            Else
                tShow.Enabled = False
            End If

            MyBase.Show()
        End Sub
        Private offscreen As Point
        Private _displayPoint As Point

        Public Property DisplayPoint() As Point
            Get
                Return _displayPoint
            End Get
            Set(ByVal value As Point)
                _displayPoint = value

                If _slide2 Then
                    Animate(_displayPoint, Size, 1, New TimeSpan(0, 0, 0, 0, _fadetime))
                Else
                    Location = _displayPoint
                End If
            End Set
        End Property

        Public Overloads Sub Hide(Optional ByVal delay As Integer = 0)
            tShow.Interval = delay
            tShow.Start()
        End Sub

        Private Sub tShow_Tick(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles tShow.Tick

            tShow.Enabled = False

            _hidden = True

            Dim p As Point = offscreen
            If Not _slide Then
                p = Location
            End If
            Dim o As Double = 0
            If Not _fade Then
                o = 1
            End If
            If _slide OrElse _fade Then
                Animate(p, Size, o, New TimeSpan(0, 0, 0, 0, _fadetime))
            Else
                Visible = False
            End If

            ShiftOtherNotifications(Me)

            If _autoclose Then
                tClose.Interval = _fadetime
                tClose.Start()
            End If
        End Sub
        Private _hidden As Boolean = True

        Private Sub tClose_Tick(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles tClose.Tick

            tClose.Enabled = False

            If _autoclose Then
                MyBase.Close()
            End If
        End Sub

        Public Overloads Sub Close()
            If InvokeRequired Then
                Invoke(New CloseDelegate(AddressOf MyBase.Close), New Object() {})
                Return
            End If

            If Opacity > 0 Then
                ShiftOtherNotifications(Me)
            End If

            MyBase.Close()
        End Sub
        Private Delegate Sub CloseDelegate()

        Private Shared Sub ShiftOtherNotifications(ByVal n As Notification)
            If Not n.SlideWhenOtherNotificationCloses Then
                Return
            End If

            Dim notifications As New ArrayList()
            For Each f As Form In Application.OpenForms
                If TypeOf f Is MZZT.Notification AndAlso f IsNot n AndAlso _
                    DirectCast(f, MZZT.Notification).DisplayPoint <> Nothing Then

                    If DirectCast(f, Notification)._hidden Then
                        Continue For
                    End If

                    notifications.Add(f)
                End If
            Next f

            Dim offsetdir As DockStyle = DockStyle.None
            Dim tbarea As Rectangle = GetTaskbarRect()
            If tbarea = Nothing Then
                Dim bounds As Rectangle = Screen.PrimaryScreen.Bounds
                offsetdir = DockStyle.Top
            Else
                Dim tbi As TaskbarInfo = GetTaskbarInfo(tbarea)
                Dim bounds As Rectangle = tbi.Screen.Bounds
                Select Case tbi.Dock
                    Case DockStyle.Bottom
                        offsetdir = DockStyle.Top
                    Case DockStyle.Top
                        offsetdir = DockStyle.Bottom
                    Case DockStyle.Left
                        offsetdir = DockStyle.Right
                    Case DockStyle.Right
                        offsetdir = DockStyle.Left
                End Select
            End If

            For Each no As Notification In notifications
                If no.Instance < n.Instance Then
                    Continue For
                End If

                Dim p As Point = no.DisplayPoint
                Select Case offsetdir
                    Case DockStyle.Bottom
                        p.Y -= n.Height
                    Case DockStyle.Top
                        p.Y += n.Height
                    Case DockStyle.Left
                        p.X += n.Width
                    Case DockStyle.Right
                        p.X -= n.Width
                End Select

                no.DisplayPoint = p
            Next no
        End Sub
#End Region

#Region "Properties"
        Public Property AutoClose() As Boolean
            Get
                Return _autoclose
            End Get
            Set(ByVal value As Boolean)
                _autoclose = value
            End Set
        End Property
        Private _autoclose As Boolean = True

        Private Function GetImage() As Image
            If pic.InvokeRequired Then
                Return pic.Invoke(New GetImageDelegate(AddressOf GetImage))
            End If

            Return pic.Image
        End Function
        Private Delegate Function GetImageDelegate() As Image

        Private Sub SetImage(ByVal value As Image)
            If pic.InvokeRequired Then
                pic.Invoke(New SetImageDelegate(AddressOf SetImage), value)
                Return
            End If

            pic.Image = value
        End Sub
        Private Delegate Sub SetImageDelegate(ByVal value As Image)

        Public Property Image() As Image
            Get
                Return GetImage()
            End Get
            Set(ByVal value As Image)
                SetImage(value)
            End Set
        End Property

        Private Function GetRtf() As String
            If t.InvokeRequired Then
                Return t.Invoke(New GetRtfDelegate(AddressOf GetRtf))
            End If

            Return t.Rtf
        End Function
        Private Delegate Function GetRtfDelegate() As String

        Private Sub SetRtf(ByVal value As String)
            If t.InvokeRequired Then
                t.Invoke(New SetRtfDelegate(AddressOf SetRtf), value)
                Return
            End If

            t.Rtf = ""
            t.Rtf = value
        End Sub
        Private Delegate Sub SetRtfDelegate(ByVal value As String)

        Public Property Rtf() As String
            Get
                Return GetRtf()
            End Get
            Set(ByVal value As String)
                SetRtf(value)
            End Set
        End Property

        Public Property ShowTime() As Integer
            Get
                Return _showtime
            End Get
            Set(ByVal value As Integer)
                _showtime = value
            End Set
        End Property
        Private _showtime As Integer = 5000

        Public Property TransitionTime() As Integer
            Get
                Return _fadetime
            End Get
            Set(ByVal value As Integer)
                _fadetime = value
            End Set
        End Property
        Private _fadetime As Integer = 500

        Public Property Slide() As Boolean
            Get
                Return _slide
            End Get
            Set(ByVal value As Boolean)
                _slide = value
            End Set
        End Property
        Private _slide As Boolean = SystemInformation.UIEffectsEnabled AndAlso _
            SystemInformation.IsMenuAnimationEnabled AndAlso Not SystemInformation. _
            IsMenuFadeEnabled

        Public Property SlideWhenOtherNotificationCloses() As Boolean
            Get
                Return _slide2
            End Get
            Set(ByVal value As Boolean)
                _slide2 = value
            End Set
        End Property
        Private _slide2 As Boolean = _slide

        Public Property Fade() As Boolean
            Get
                Return _fade
            End Get
            Set(ByVal value As Boolean)
                _fade = value
            End Set
        End Property
        Private _fade As Boolean = SystemInformation.UIEffectsEnabled AndAlso _
            SystemInformation.IsMenuAnimationEnabled AndAlso SystemInformation. _
            IsMenuFadeEnabled

        Private ReadOnly Property Instance() As UInteger
            Get
                Return _instance
            End Get
        End Property
        Private _instance As UInteger = 0
        Private Shared _totalinstances As UInteger = 0

        Public Property ExtraControl() As Control
            Get
                Return _extra
            End Get
            Set(ByVal value As Control)
                If _extra IsNot Nothing Then
                    Controls.Remove(_extra)
                    t.Height = ClientSize.Height - 24
                    t.Invalidate()
                End If

                _extra = value

                If _extra IsNot Nothing Then
                    Controls.Add(_extra)
                    _extra.Left = t.Left
                    _extra.Width = t.Width
                    _extra.Top = ClientSize.Height - 12 - _extra.Height
                    t.Height = _extra.Top - 12 - _extra.Margin.Top - t.Margin.Bottom
                    t.Invalidate()
                End If
            End Set
        End Property
        Private _extra As Control = Nothing
#End Region

#Region "Windows API"
        Private Enum WS_EX As Integer
            TRANSPARENT = &H20
            NOACTIVATE = &H8000000
        End Enum

        Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal _
            lpClassName As String, ByVal lpWindowName As String) As IntPtr
        Private Declare Function GetWindowRect Lib "user32" (ByVal hWnd As IntPtr, _
            ByRef lpRect As RECT) As Boolean

        <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind. _
            Sequential)> _
        Private Structure RECT
            Public Left As Integer
            Public Top As Integer
            Public Right As Integer
            Public Bottom As Integer
        End Structure
#End Region

        Private Sub t_LinkClicked(ByVal sender As System.Object, ByVal e As System. _
            Windows.Forms.LinkClickedEventArgs) Handles t.LinkClicked

            Process.Start(e.LinkText)
        End Sub
    End Class
End Namespace
Imports System.Xml.Serialization
Imports System.IO
Imports System.ComponentModel

Namespace MZZT.Knight.Games
    <XmlRoot("games", NameSpace:="mzzt:knight:modinfocache")> _
    Public Class ModInfoCache
        Public Class Game
            <EditorBrowsable(EditorBrowsableState.Never)> _
            Public Sub New()
            End Sub

            <EditorBrowsable(EditorBrowsableState.Never)> _
            Protected Friend Sub New(ByVal owner As ModInfoCache, ByVal name As _
                String)

                _owner = owner
                _name = name
            End Sub
            Protected Friend _owner As ModInfoCache

#Region "Properties"
            <XmlAttribute("name")> _
            Public Property Name() As String
                Get
                    Return _name
                End Get
                Set(ByVal value As String)
                    If _owner IsNot Nothing Then
                        SyncLock _owner
                            _name = value
                            _owner.Save()
                        End SyncLock
                    Else
                        _name = value
                    End If
                End Set
            End Property
            Private _name As String = Nothing

            <XmlIgnore(), EditorBrowsable(EditorBrowsableState.Never)> _
            Public ReadOnly Property Mods() As List(Of Modification)
                Get
                    Return _mods
                End Get
            End Property
            Protected Friend _mods As New List(Of Modification)

            <XmlElement("modtype"), EditorBrowsable(EditorBrowsableState.Never)> _
            Public Property ModSerializer() As Modification.Serializer
                Get
                    Return New Modification.Serializer(_mods)
                End Get
                Set(ByVal value As Modification.Serializer)
                    _mods = value.Mods

                    For Each m As Modification In _mods
                        m._owner = _owner
                    Next m
                End Set
            End Property
#End Region
        End Class

        <XmlRoot("mod")> _
        Public Class Modification
            <EditorBrowsable(EditorBrowsableState.Never)> _
            Public Class Serializer
                Implements IXmlSerializable

                Public Sub New()
                    _mods = New List(Of Modification)
                End Sub

                Public Sub New(ByVal l As List(Of Modification))
                    _mods = l
                End Sub

                Public ReadOnly Property Mods() As List(Of Modification)
                    Get
                        Return _mods
                    End Get
                End Property
                Private _mods As List(Of Modification)

                Public Function GetSchema() As System.Xml.Schema.XmlSchema _
                    Implements System.Xml.Serialization.IXmlSerializable.GetSchema

                    Return Nothing
                End Function

                Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements _
                    System.Xml.Serialization.IXmlSerializable.ReadXml

                    While reader.IsStartElement
                        Dim t As Type = Type.GetType(reader.GetAttribute("type"))
                        reader.ReadStartElement()
                        Dim x As New XmlSerializer(t)
                        _mods.Add(x.Deserialize(reader))
                        reader.ReadEndElement()
                    End While
                End Sub

                Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements _
                    System.Xml.Serialization.IXmlSerializable.WriteXml

                    For i As Integer = 0 To _mods.Count - 1
                        If i > 0 Then
                            writer.WriteStartElement("modtype")
                        End If

                        Dim t As Type = _mods(i).GetType
                        writer.WriteAttributeString("type", t.ToString)
                        Dim x As New XmlSerializer(t)
                        x.Serialize(writer, _mods(i))

                        If i < _mods.Count - 1 Then
                            writer.WriteEndElement()
                        End If
                    Next i
                End Sub
            End Class

            Public Sub New()
            End Sub

            Public Sub New(ByVal name As String)
                _name = name
            End Sub

            Protected Friend _owner As ModInfoCache

#Region "Properties"
            <XmlAttribute("uid")> _
            Public Property UniqueID() As String
                Get
                    Return _uid
                End Get
                Set(ByVal value As String)
                    If _owner IsNot Nothing Then
                        SyncLock _owner
                            _uid = value
                            _owner.Save()
                        End SyncLock
                    Else
                        _uid = value
                    End If
                End Set
            End Property
            Private _uid As String = Nothing

            <XmlAttribute("name")> _
            Public Property Name() As String
                Get
                    Return _name
                End Get
                Set(ByVal value As String)
                    If _owner IsNot Nothing Then
                        SyncLock _owner
                            _name = value
                            _owner.Save()
                        End SyncLock
                    Else
                        _name = value
                    End If
                End Set
            End Property
            Private _name As String = Nothing
#End Region
        End Class

        Public Sub BeginUpdate()
            _update += 1
        End Sub

        Public Sub EndUpdate()
            _update -= 1

            If _update = 0 AndAlso _write Then
                _write = False
                Save()
            End If

            If _update < 0 Then
                Throw New InvalidOperationException( _
                    "CachedModInfo.EndUpdate(): Used more times than BeginUpdate()!")
            End If
        End Sub
        Private _update As Integer = 0
        Private _write As Boolean = False

        Private Shared Function GetXMLPath() As String
            Dim fi As New FileInfo(New Uri(Reflection.Assembly. _
                GetExecutingAssembly.CodeBase).LocalPath)
            If Options.IsPortableAppsMode() Then
                Return Path.Combine(fi.Directory.FullName, _portableAppsXmlPath)
            Else
                Dim di As New DriveInfo(fi.FullName(0))
                If di.DriveType = DriveType.Removable Then
                    Return Path.Combine(fi.Directory.FullName, _removableXmlPath)
                Else
                    Return Path.Combine(fi.Directory.FullName, _xmlPath)
                End If
            End If
        End Function

        Public Shared Function Load() As ModInfoCache
            Dim sr As StreamReader
            Try
                sr = New StreamReader(GetXMLPath, False)
            Catch
                Return LoadDefaults()
            End Try

            Dim x As New XmlSerializer(GetType(ModInfoCache))
            Dim mic As ModInfoCache = Nothing
            Try
                mic = x.Deserialize(sr)
            Catch
                sr.Close()
                sr.Dispose()
                Return LoadDefaults()
            End Try

            sr.Close()
            sr.Dispose()

            For Each g As Game In mic._games
                g._owner = mic
                For Each m As Modification In g.Mods
                    m._owner = mic
                Next m
            Next g

            Return mic
        End Function

        Public Shared Function LoadDefaults() As ModInfoCache
            Dim mic As ModInfoCache = Nothing
            Dim res As String = My.Resources.ResourceManager.GetString( _
                "modinfocache_defaults")
            If res Is Nothing Then
                Return Nothing
            End If

            Dim sr As New StringReader(res)
            Dim x As New XmlSerializer(GetType(ModInfoCache))
            mic = x.Deserialize(sr)
            sr.Close()
            sr.Dispose()

            For Each g As Game In mic._games
                g._owner = mic
                For Each m As Modification In g.Mods
                    m._owner = mic
                Next m
            Next g

            Return mic
        End Function

        Public Sub Save()
            If _update > 0 Then
                _write = True
                Return
            End If

            Dim types As New List(Of Type)
            For Each g As Game In _games
                For Each m As Modification In g._mods
                    Dim t As Type = m.GetType
                    If types.Contains(t) Then
                        Continue For
                    End If
                    types.Add(t)
                Next m
            Next g

            Dim x As New XmlSerializer(Me.GetType, types.ToArray)
            Dim ms As New IO.MemoryStream()
            Dim sw As New StreamWriter(ms, System.Text.Encoding.UTF8)

            Try
                SyncLock Me
                    x.Serialize(sw, Me)
                End SyncLock
            Catch ex As Exception
                While ex.InnerException IsNot Nothing
                    ex = ex.InnerException
                End While

                MsgBox("A problem occurred saving cached mod info.  Changes have " & _
                    "been lost.  Further problems may occur in the program, it is " & _
                    "recommended you quit and restart." & Environment.NewLine & _
                    Environment.NewLine & "Technical information: " & ex.Message, _
                    MsgBoxStyle.Critical, "XML Serialization Error - Knight")
                sw.Close()
                sw.Dispose()
                ms.Close()
                ms.Dispose()
                Return
            End Try

            Dim fs As FileStream
            Try
                fs = New FileStream(GetXMLPath, FileMode.Create, FileAccess.Write)
            Catch
                sw.Close()
                sw.Dispose()
                ms.Close()
                ms.Dispose()
                Return
            End Try

            fs.Write(ms.ToArray, 0, ms.Length)
            fs.Close()
            fs.Dispose()
            sw.Close()
            sw.Dispose()
            ms.Close()
            ms.Dispose()
        End Sub

        Private Sub SetValue(ByVal game As String, ByVal uid As String, ByVal value _
            As Modification, ByVal save As Boolean)

            Dim selectedgame As Game = Nothing
            For Each g As Game In _games
                If g.Name.ToLower = game.ToLower Then
                    For i As Integer = 0 To g.Mods.Count - 1
                        If g.Mods(i).UniqueID.ToLower = uid.ToLower Then
                            SyncLock Me
                                g.Mods(i) = value
                                value.UniqueID = uid
                                value._owner = Me
                            End SyncLock

                            If save Then
                                Me.Save()
                            End If
                            Return
                        End If
                    Next i

                    selectedgame = g

                    Exit For
                End If
            Next g

            If selectedgame Is Nothing Then
                Dim g As New Game(Me, game)
                SyncLock Me
                    _games.Add(g)
                End SyncLock
                selectedgame = g
            End If

            SyncLock Me
                selectedgame.Mods.Add(value)
                value.UniqueID = uid
                value._owner = Me
            End SyncLock

            If save Then
                Me.Save()
            End If
        End Sub

#Region "Properties"
        <XmlIgnore()> _
        Default Public Property Item(ByVal game As String, ByVal uid As String) As  _
            Modification

            Get
                For Each g As Game In _games
                    If g.Name.ToLower = game.ToLower Then
                        For Each m As Modification In g.Mods
                            If m.UniqueID.ToLower = uid.ToLower Then
                                Return m
                            End If
                        Next m

                        Return Nothing
                    End If
                Next g

                Return Nothing
            End Get
            Set(ByVal value As Modification)
                SetValue(game, uid, value, True)
            End Set
        End Property

        <XmlElement("game"), EditorBrowsable(EditorBrowsableState.Never)> _
        Public ReadOnly Property Games() As List(Of Game)
            Get
                Return _games
            End Get
        End Property
        Private _games As New List(Of Game)

        Public Shared Property XMLPath() As String
            Get
                Return _xmlPath
            End Get
            Set(ByVal value As String)
                _xmlPath = value
            End Set
        End Property
        Private Shared _xmlPath As String = Path.Combine(Environment.GetFolderPath( _
            Environment.SpecialFolder.LocalApplicationData), _
            "The MAZZTer\Knight\modinfocache.xml")

        Public Shared Property RemovableXMLPath() As String
            Get
                Return _removableXmlPath
            End Get
            Set(ByVal value As String)
                _removableXmlPath = value
            End Set
        End Property
        Private Shared _removableXmlPath As String = "modinfocache.xml"

        Public Shared Property PortableAppsXMLPath() As String
            Get
                Return _portableAppsXmlPath
            End Get
            Set(ByVal value As String)
                _portableAppsXmlPath = value
            End Set
        End Property
        Private Shared _portableAppsXmlPath As String = "Data\settings\modinfocache.xml"
#End Region
    End Class

    <XmlRoot("mod")> _
    Public Class DarkForcesModInfo
        Inherits ModInfoCache.Modification

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal name As String)
            MyBase.New(name)
        End Sub

#Region "Properties"
        <XmlAttribute("lfd")> _
        Public Property LFD() As String
            Get
                Return _lfd
            End Get
            Set(ByVal value As String)
                If _owner IsNot Nothing Then
                    SyncLock _owner
                        _lfd = value
                        _owner.Save()
                    End SyncLock
                Else
                    _lfd = value
                End If
            End Set
        End Property
        Private _lfd As String = Nothing

        <XmlAttribute("crawl")> _
        Public Property Crawl() As String
            Get
                Return _crawl
            End Get
            Set(ByVal value As String)
                If _owner IsNot Nothing Then
                    SyncLock _owner
                        _crawl = value
                        _owner.Save()
                    End SyncLock
                Else
                    _crawl = value
                End If
            End Set
        End Property
        Private _crawl As String = Nothing

        <XmlElement("other")> _
        Public ReadOnly Property Others() As List(Of String)
            Get
                Return _others
            End Get
        End Property
        Private _others As New List(Of String)
#End Region
    End Class

    <XmlRoot("mod")> _
    Public Class Quake3ModInfo
        Inherits ModInfoCache.Modification

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal name As String)
            MyBase.New(name)
        End Sub

#Region "Properties"
        <XmlAttribute("singleplayer")> _
        Public Property SinglePlayer() As Boolean
            Get
                Return _singleplayer
            End Get
            Set(ByVal value As Boolean)
                If _owner IsNot Nothing Then
                    SyncLock _owner
                        _singleplayer = value
                        _owner.Save()
                    End SyncLock
                Else
                    _singleplayer = value
                End If
            End Set
        End Property
        Private _singleplayer As Boolean = False
#End Region
    End Class
End Namespace

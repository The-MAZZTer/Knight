Imports System.Xml.Serialization
Imports System.IO
Imports System.ComponentModel
Imports System.Security.Cryptography

Namespace MZZT.Knight
    <XmlRoot("hashes", namespace:="mzzt:hashlist")> _
    Public Class HashList
        Public Class Hash
            Public Sub New()
            End Sub

            Public Sub New(ByVal uid As String)
                _uid = uid
            End Sub

            Protected Friend _owner As Hashlist

#Region "Properties"
            <XmlIgnore()> _
            Public Property DataHash() As Byte()
                Get
                    Return _value
                End Get
                Set(ByVal value As Byte())
                    If _owner IsNot Nothing Then
                        SyncLock _owner
                            _value = value
                            _owner.Save()
                        End SyncLock
                    Else
                        _value = value
                    End If
                End Set
            End Property
            Private _value As Byte() = Nothing

            <XmlAttribute("datasize")> _
            Public Property DataSize() As UInteger
                Get
                    Return _size
                End Get
                Set(ByVal value As UInteger)
                    If _owner IsNot Nothing Then
                        SyncLock _owner
                            _size = value
                            _owner.Save()
                        End SyncLock
                    Else
                        _size = value
                    End If
                End Set
            End Property
            Private _size As UInteger = 0

            <XmlAttribute("filename")> _
            Public Property FileName() As String
                Get
                    Return _filename
                End Get
                Set(ByVal value As String)
                    If _owner IsNot Nothing Then
                        SyncLock _owner
                            _filename = value
                            _owner.Save()
                        End SyncLock
                    Else
                        _filename = value
                    End If
                End Set
            End Property
            Private _filename As String = Nothing

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

            <XmlAttribute("id")> _
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

            <XmlText(), EditorBrowsable(EditorBrowsableState.Never)> _
            Public Property ValueString() As String
                Get
                    Return Convert.ToBase64String(_value)
                End Get
                Set(ByVal value As String)
                    _value = Convert.FromBase64String(value)
                End Set
            End Property
#End Region
        End Class

        <EditorBrowsable(EditorBrowsableState.Never)> _
        Public Sub New()
        End Sub

        Public Sub New(ByVal a As HashAlgorithm)
            _algorithm = a
        End Sub

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
                    "HashList.EndUpdate(): Used more times than BeginUpdate()!")
            End If
        End Sub
        Private _update As Integer = 0
        Private _write As Boolean = False

        Public Function HashFile(ByVal name As String, ByVal filename As String) As Hash
            Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read)
            Dim h As New Hash()
            SyncLock _algorithm
                h.DataHash = _algorithm.ComputeHash(fs)
            End SyncLock
            h.DataSize = fs.Length
            h.FileName = Path.GetFileName(filename)
            h.Name = name
            fs.Close()
            fs.Dispose()
            Return h
        End Function

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

        Public Function IdentifyFile(ByVal b As Byte(), ByVal filesize As Integer) _
            As Hash

            SyncLock Me
                For Each h As Hash In _hashes
                    If h.DataSize <> filesize Then
                        Continue For
                    End If

                    If b.Length <> h.DataHash.Length Then
                        Continue For
                    End If

                    Dim match As Boolean = True
                    For i As Integer = 0 To b.Length - 1
                        If b(i) <> h.DataHash(i) Then
                            match = False
                            Exit For
                        End If
                    Next i

                    If match Then
                        Return h
                    End If
                Next h
            End SyncLock

            Return Nothing
        End Function

        Public Function IdentifyFile(ByVal filename As String) As Hash
            Dim filehash As Byte() = Nothing
            Dim fi As New FileInfo(filename)

            SyncLock Me
                For Each h As Hash In _hashes
                    If h.DataSize <> fi.Length Then
                        Continue For
                    End If

                    If filehash Is Nothing Then
                        filehash = HashFile(Nothing, filename).DataHash
                    End If

                    If filehash.Length <> h.DataHash.Length Then
                        Continue For
                    End If

                    Dim match As Boolean = True
                    For i As Integer = 0 To filehash.Length - 1
                        If filehash(i) <> h.DataHash(i) Then
                            match = False
                            Exit For
                        End If
                    Next i

                    If match Then
                        Return h
                    End If
                Next h
            End SyncLock

            Return Nothing
        End Function

        Public Shared Function Load() As HashList
            Dim dhl As HashList = LoadDefaults()

            Dim sr As StreamReader
            Dim x As New XmlSerializer(GetType(HashList))
            Try
                sr = New StreamReader(GetXMLPath, False)
            Catch
                Return dhl
            End Try

            Dim hl As HashList = Nothing
            Try
                hl = x.Deserialize(sr)
            Catch
                sr.Close()
                sr.Dispose()
                Return dhl
            End Try

            sr.Close()
            sr.Dispose()

            If dhl IsNot Nothing Then
                dhl.Merge(hl)
                Return dhl
            End If

            For Each h As Hash In hl._hashes
                h._owner = hl
            Next h

            Return hl
        End Function

        Public Shared Function LoadDefaults() As HashList
            Dim hl As HashList = Nothing
            Dim res As String = My.Resources.ResourceManager.GetString( _
                "hashlist_defaults")
            If res Is Nothing Then
                Return Nothing
            End If

            Dim sr As New StringReader(res)
            Dim x As New XmlSerializer(GetType(HashList))
            hl = x.Deserialize(sr)
            sr.Close()
            sr.Dispose()

            For Each h As Hash In hl._hashes
                h._owner = hl
            Next h

            Return hl
        End Function

        Private Sub Merge(ByVal hl As HashList)
            For Each h As Hash In hl._hashes
                SetValue(h.UniqueID, h, False)
            Next h
        End Sub

        Public Sub Save()
            If _update > 0 Then
                _write = True
                Return
            End If

            Dim x As New XmlSerializer(Me.GetType)
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

                MsgBox("A problem occurred saving the hash list.  Changes have " & _
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

        Private Sub SetValue(ByVal uid As String, ByVal value As Hash, ByVal save As _
            Boolean)

            For i As Integer = 0 To _hashes.Count - 1
                If _hashes(i).UniqueID.ToLower = uid.ToLower Then
                    SyncLock Me
                        _hashes(i) = value
                        _hashes(i).UniqueID = uid
                        _hashes(i)._owner = Me
                    End SyncLock

                    If save Then
                        Me.Save()
                    End If
                    Return
                End If
            Next i

            SyncLock Me
                _hashes.Add(value)
                value.UniqueID = uid
                value._owner = Me
            End SyncLock

            If save Then
                Me.Save()
            End If
        End Sub

#Region "Properties"
        <XmlIgnore()> _
        Public Property Algorithm() As HashAlgorithm
            Get
                Return _algorithm
            End Get
            Set(ByVal value As HashAlgorithm)
                _algorithm = value
            End Set
        End Property
        Private _algorithm As HashAlgorithm = Nothing

        <XmlAttribute("algorithm"), EditorBrowsable(EditorBrowsableState.Never)> _
        Public Property AlgorithmType() As String
            Get
                Return _algorithm.GetType().FullName
            End Get
            Set(ByVal value As String)
                Dim t As Type = Type.GetType(value, True, True)
                _algorithm = t.Assembly.CreateInstance(t.FullName, True)
            End Set
        End Property

        <XmlIgnore()> _
        Default Public Property Item(ByVal uid As String) As Hash
            Get
                For Each h As Hash In _hashes
                    If h.UniqueID.ToLower = uid.ToLower Then
                        Return h
                    End If
                Next h

                Return Nothing
            End Get
            Set(ByVal value As Hash)
                SetValue(uid, value, True)
            End Set
        End Property

        <XmlElement("hash"), EditorBrowsable(EditorBrowsableState.Never)> _
        Public ReadOnly Property Hashes() As List(Of Hash)
            Get
                Return _hashes
            End Get
        End Property
        Private _hashes As New List(Of Hash)

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
            "The MAZZTer\Knight\hashlist.xml")

        Public Shared Property RemovableXMLPath() As String
            Get
                Return _removableXmlPath
            End Get
            Set(ByVal value As String)
                _removableXmlPath = value
            End Set
        End Property
        Private Shared _removableXmlPath As String = "hashlist.xml"

        Public Shared Property PortableAppsXMLPath() As String
            Get
                Return _portableAppsXmlPath
            End Get
            Set(ByVal value As String)
                _portableAppsXmlPath = value
            End Set
        End Property
        Private Shared _portableAppsXmlPath As String = "Data\settings\hashlist.xml"
#End Region
    End Class
End Namespace

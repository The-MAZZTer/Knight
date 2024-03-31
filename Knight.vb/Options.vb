Imports System.Xml.Serialization
Imports System.IO
Imports System.ComponentModel

Namespace MZZT
		<XmlRoot("options", NameSpace:="mzzt:options")> _
		Public Class Options
				<EditorBrowsable(EditorBrowsableState.Never)> _
				Public Class [Option]
						<EditorBrowsable(EditorBrowsableState.Never)> _
						Public Sub New()
						End Sub

						<EditorBrowsable(EditorBrowsableState.Never)> _
						Protected Friend Sub New(ByVal owner As Options, ByVal name As String, _
								ByVal value As Object)

								_owner = owner
								_name = name
								_value = value
						End Sub
						Protected Friend _owner As Options

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

						<XmlElement("value")> _
						Public Property Value() As Object
								Get
										Return _value
								End Get
								Set(ByVal value As Object)
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
						Private _value As Object = Nothing
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
										"Options.EndUpdate(): Used more times than BeginUpdate()!")
						End If
				End Sub
				Private _update As Integer = 0
				Private _write As Boolean = False

				Private Shared Function GetXMLPath() As String
						Dim fi As New FileInfo(New Uri(Reflection.Assembly. _
								GetExecutingAssembly.CodeBase).LocalPath)
						If IsPortableAppsMode() Then
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

				Public Shared ReadOnly Property IsPortableAppsMode() As Boolean
						Get
								If _checkedportable Then
										Return _portable
								End If
								_checkedportable = True

								Dim fi As New FileInfo(New Uri(Reflection.Assembly. _
								GetExecutingAssembly.CodeBase).LocalPath)
								Dim di As DirectoryInfo = fi.Directory
								Dim portableapps As DirectoryInfo = di.Parent

								If portableapps Is Nothing Then
										_portable = False
										Return _portable
								End If

								_portable = portableapps.Name.ToLower = "portableapps"
								Return _portable
						End Get
				End Property
				Private Shared _portable As Boolean = False
				Private Shared _checkedportable As Boolean = False

				Public Shared Function Load() As Options
						Dim o As Options = LoadDefaults()

						Dim sr As StreamReader
						Dim x As New XmlSerializer(SerializationType)
						Try
								sr = New StreamReader(GetXMLPath, False)
						Catch
								Return o
						End Try

						Dim o2 As Options = Nothing
						Try
								o2 = x.Deserialize(sr)
						Catch
								sr.Close()
								sr.Dispose()
								Return o
						End Try

						sr.Close()
						sr.Dispose()

						If o IsNot Nothing Then
								o.Merge(o2)
								Return o
						End If

						For Each op As Options.Option In o2._options
								op._owner = o2
						Next op

						Return o2
				End Function

				Public Shared Function LoadDefaults() As Options
						Dim o As Options = Nothing
						Dim res As String = My.Resources.ResourceManager.GetString( _
								"options_defaults")
						If res Is Nothing Then
								Return Nothing
						End If

						Dim sr As New StringReader(res)
						Dim x As New XmlSerializer(SerializationType)
						o = x.Deserialize(sr)
						sr.Close()
						sr.Dispose()

						For Each op As Options.Option In o._options
								op._owner = o
						Next op

						Return o
				End Function
				Protected Friend Shared SerializationType As Type = GetType(Options)

				Private Sub Merge(ByVal os As Options)
						For Each o As [Option] In os._options
								SetValue(o.Name, o.Value, False)
						Next o
				End Sub

				Public Sub Save()
						If _update > 0 Then
								_write = True
								Return
						End If

						Dim types As New List(Of Type)
						For Each o As [Option] In _options
								If o.Value Is Nothing Then
										Continue For
								End If
								Dim t As Type = o.Value.GetType
								If types.Contains(t) Then
										Continue For
								End If
								types.Add(t)
						Next o

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

								MsgBox("A problem occurred saving settings.  Changes have been " & _
										"lost.  Further problems may occur in the program, it is " & _
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
				Dim fi As New FileInfo(GetXMLPath)
				fi.Directory.Create()
				fs = New FileStream(fi.FullName, FileMode.Create, FileAccess.Write)
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

				Private Sub SetValue(ByVal name As String, ByVal value As Object, ByVal save _
						As Boolean)

						For Each o As [Option] In _options
								If o.Name = name Then
										SyncLock Me
												o._owner = Nothing
												o.Value = value
												o._owner = Me
										End SyncLock

										If save Then
												Me.Save()
										End If

										Return
								End If
						Next o

						Dim newo As New [Option](Me, name, value)
						SyncLock Me
								_options.Add(newo)
						End SyncLock

						If save Then
								Me.Save()
						End If
				End Sub

#Region "Properties"
				<XmlIgnore()> _
				Default Public Property Item(ByVal name As String) As Object
						Get
								For Each o As [Option] In _options
										If o.Name = name Then
												Return o.Value
										End If
								Next o

								Return Nothing
						End Get
						Set(ByVal value As Object)
								SetValue(name, value, True)
						End Set
				End Property

				<XmlIgnore()> _
				Public ReadOnly Property ItemExists(ByVal name As String) As Boolean
						Get
								For Each o As [Option] In _options
										If o.Name = name Then
												Return True
										End If
								Next o

								Return False
						End Get
				End Property

				<XmlElement("option"), EditorBrowsable(EditorBrowsableState.Never)> _
				Public ReadOnly Property Options() As List(Of [Option])
						Get
								Return _options
						End Get
				End Property
				Private _options As New List(Of [Option])

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
						"The MAZZTer\Knight\options.xml")

				Public Shared Property RemovableXMLPath() As String
						Get
								Return _removableXmlPath
						End Get
						Set(ByVal value As String)
								_removableXmlPath = value
						End Set
				End Property
				Private Shared _removableXmlPath As String = "options.xml"

				Public Shared Property PortableAppsXMLPath() As String
						Get
								Return _portableAppsXmlPath
						End Get
						Set(ByVal value As String)
								_portableAppsXmlPath = value
						End Set
				End Property
				Private Shared _portableAppsXmlPath As String = "Data\settings\options.xml"
#End Region
		End Class
End Namespace

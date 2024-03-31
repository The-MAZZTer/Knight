Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Windows.Forms

Namespace MZZT.Knight
	<XmlRoot("options", Namespace:="mzzt:knight:options")>
	Public Class Options
		Inherits MZZT.Options

		Shared Sub New()
			SerializationType = GetType(Options)
		End Sub

		Private Function GetCurrentDrive() As String
			Return Application.ExecutablePath.Substring(0, 2)
		End Function

		Public Overloads Shared Function Load() As Options
			Dim o As MZZT.Options = MZZT.Options.Load()
			If TypeOf o Is Options Then
				Return o
			End If

			Return LoadDefaults()
		End Function

		Public Overloads Shared Function LoadDefaults() As Options
			' Overload to ensure Shared Sub New is called before base.LoadDefaults

			Return MZZT.Options.LoadDefaults
		End Function

#Region "Properties"
		<XmlIgnore()>
		Public Property BigIconsSize() As Size
			Get
				Dim name As String = MethodBase.GetCurrentMethod.Name.Substring(4)
				Return New Size(Item(name & "_Width"), Item(name & "_Height"))
			End Get
			Set(ByVal value As Size)
				Dim name As String = MethodBase.GetCurrentMethod.Name.Substring(4)

				BeginUpdate()
				Item(name & "_Width") = value.Width
				Item(name & "_Height") = value.Height
				EndUpdate()
			End Set
		End Property


#Region "Dark Forces Options"
		<XmlIgnore()>
		Public Property DarkForcesAutoTest() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesCDDrive() As Char
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Char)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesDarkXLPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesDOSBoxPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesEnabled() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesEnableScreenshots() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesLevelSelect() As String
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As String)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesLog() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesModPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesRunMode() As DarkForcesRunModes
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As DarkForcesRunModes)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = CByte(value)
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesSkipCutscenes() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesSkipFILESCheck() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property DarkForcesSkipMemoryCheck() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property
#End Region

		<XmlIgnore()>
		Public Property ExpandedGame() As Integer
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Integer)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property FirstRun() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

#Region "Jedi Academy Options"
		<XmlIgnore()>
		Public Property JediAcademyCommands() As String
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As String)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediAcademyEnabled() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediAcademyPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property
#End Region

#Region "Jedi Knight Options"
		<XmlIgnore()>
		Public Property JediKnightActiveModPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightAdvancedDisplayOptions() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightAllowMultipleInstances() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightConsole() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightConsoleStats() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightEnabled() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightEnableJKUPCogVerbs() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightFramerate() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightLog() As LogTypes
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As LogTypes)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = CByte(value)
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightModPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightNoHUD() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightPatchPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightVerbosity() As SByte
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As SByte)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediKnightWindowedGUI() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property
#End Region

#Region "Jedi Outcast Options"
		<XmlIgnore()>
		Public Property JediOutcastCommands() As String
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As String)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediOutcastEnabled() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property JediOutcastPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property
#End Region

		<XmlIgnore()>
		Public Property Location() As Point
			Get
				Dim name As String = MethodBase.GetCurrentMethod.Name.Substring(4)
				If Not ItemExists(name & "_X") OrElse Not ItemExists(name & "_Y") Then
					Return Nothing
				End If

				Return New Point(Item(name & "_X"), Item(name & "_Y"))
			End Get
			Set(ByVal value As Point)
				Dim name As String = MethodBase.GetCurrentMethod.Name.Substring(4)

				BeginUpdate()
				Item(name & "_X") = value.X
				Item(name & "_Y") = value.Y
				EndUpdate()
			End Set
		End Property

#Region "Mysteries of the Sith Options"
		<XmlIgnore()>
		Public Property MysteriesOfTheSithActiveModPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithAdvancedDisplayOptions() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithAllowMultipleInstances() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithCogLog() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithConsole() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithConsoleStats() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithEnabled() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithFramerate() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithLog() As LogTypes
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As LogTypes)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = CByte(value)
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithModPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithNoHUD() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithPatchPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithPath() As String
			Get
				Dim path As String = Item(MethodBase.GetCurrentMethod.Name.Substring(4))
				If IsPortableAppsMode AndAlso path IsNot Nothing AndAlso path.
						StartsWith("\"c) Then

					path = GetCurrentDrive() & path
				End If

				Return path
			End Get
			Set(ByVal value As String)
				If IsPortableAppsMode AndAlso value IsNot Nothing AndAlso value.
						Substring(0, 2).ToLower = GetCurrentDrive.ToLower Then

					value = value.Substring(2)
				End If

				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithRecord() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithResLog() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithSpeedUp() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithVerbosity() As SByte
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As SByte)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property MysteriesOfTheSithWindowedGUI() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property
#End Region

		<XmlIgnore()>
		Public Property OnRunAction() As OnRunActions
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As OnRunActions)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = CByte(value)
			End Set
		End Property

		<XmlIgnore()>
		Public Property Position() As FormStartPosition
			Get
				If Not ItemExists("Location_X") OrElse Not ItemExists("Location_Y") Then
					Return FormStartPosition.WindowsDefaultLocation
				End If

				Dim r As New Rectangle(Location, Size)
				Dim d As Rectangle = Screen.AllScreens(0).Bounds
				For i As Integer = 1 To Screen.AllScreens.Length - 1
					d = Rectangle.Union(d, Screen.AllScreens(i).Bounds)
				Next i
				If d.IntersectsWith(r) Then
					Return FormStartPosition.Manual
				End If

				Return FormStartPosition.WindowsDefaultLocation
			End Get
			Set(ByVal value As FormStartPosition)
				If Position = value Then
					Return
				End If

				Select Case value
					Case FormStartPosition.Manual
						Location = Point.Empty
						'Case FormStartPosition.WindowsDefaultBounds
					Case Else
						For i As Integer = 0 To Options.Count - 1
							If Options(i).Name.ToLower = "location_x" OrElse
									Options(i).Name.ToLower = "location_y" Then

								Options.RemoveAt(i)
							End If
						Next i
						Save()
						'Case Else
						'    Throw New NotSupportedException( _
						'        "That FormStartPosition is not supported")
				End Select
			End Set
		End Property

		<XmlIgnore()>
		Public Property SimpleList() As Boolean
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As Boolean)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = value
			End Set
		End Property

		<XmlIgnore()>
		Public Property Size() As Size
			Get
				Dim name As String = MethodBase.GetCurrentMethod.Name.Substring(4)
				Return New Size(Item(name & "_Width"), Item(name & "_Height"))
			End Get
			Set(ByVal value As Size)
				Dim name As String = MethodBase.GetCurrentMethod.Name.Substring(4)

				BeginUpdate()
				Item(name & "_Width") = value.Width
				Item(name & "_Height") = value.Height
				EndUpdate()
			End Set
		End Property

		<XmlIgnore()>
		Public Property View() As View
			Get
				Return Item(MethodBase.GetCurrentMethod.Name.Substring(4))
			End Get
			Set(ByVal value As View)
				Item(MethodBase.GetCurrentMethod.Name.Substring(4)) = CInt(value)
			End Set
		End Property
#End Region

#Region "Enumerations"
		Public Enum DarkForcesRunModes As Byte
			Native
			DOSBox
			DarkXL
		End Enum

		Public Enum LogTypes As Byte
			None
			File
			WindowsConsole
		End Enum

		Public Enum OnRunActions As Byte
			None
			Minimize
			Close
		End Enum
#End Region
	End Class
End Namespace
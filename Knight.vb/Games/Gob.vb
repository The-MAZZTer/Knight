Imports System.IO
Imports System.Text.Encoding

Namespace MZZT.Knight.Games
    Public Class Gob
        Public Sub New(ByVal s As IO.Stream)
            _stream = s
            If Not VerifyHeader() Then
                'Throw New InvalidDataException( _
                '    "Stream does not point to a GOB file header.")
                Return
            End If

            Dim ptrFirstFile As Integer = ReadInt32() - 4
            Dim ptrNumFiles As Integer = ReadInt32()
            Seek(ptrNumFiles)
            Dim numFiles As Integer = ReadInt32()
            Seek(ptrFirstFile)

            Dim name As Byte()
            For i As Integer = 0 To numFiles - 1
                Dim fi As New InternalFileInfo
                fi.Offset = ReadInt32()
                fi.Length = ReadInt32()
                name = ReadBytes(128)

                If name.Length < 128 Then
                    'Throw New EndOfStreamException("Unexpected end of stream.")
                    Return
                End If

                fi.Name = ASCII.GetString(name)
                fi.Name = fi.Name.Substring(0, fi.Name.IndexOf(Chr(0)))

                Files.Add(fi)
            Next i
        End Sub
        Private _stream As IO.Stream = Nothing
        Private _pos As Integer = 0

        Private Function VerifyHeader() As Boolean
            Dim header(3) As Byte
            Dim read As Integer = _stream.Read(header, 0, 4)
            _pos += read
            If read < 4 Then
                'Throw New EndOfStreamException("Unexpected end of stream.")
                Return False
            End If

            If ASCII.GetString(header) <> "GOB " Then
                Return False
            End If

            Return True
        End Function

        Private Function ReadInt32() As Integer
            Dim b(3) As Byte
            Dim read As Integer = _stream.Read(b, 0, 4)
            _pos += read
            If read < 4 Then
                Throw New EndOfStreamException("Unexpected end of GOB stream.")
            End If

            Return BitConverter.ToInt32(b, 0)
        End Function

        Private Function ReadBytes(ByVal len As Integer) As Byte()
            Dim b(len - 1) As Byte
            Dim read As Integer = _stream.Read(b, 0, len)
            _pos += read
            If read < len Then
                Dim newb(read - 1) As Byte
                Array.Copy(b, newb, read)
                b = newb
            End If

            Return b
        End Function

        Private Function Seek(ByVal dest As Integer) As Boolean
            Dim offset As Integer = dest - _pos
            If offset = 0 Then
                Return True
            End If

            If _stream.CanSeek Then
                Dim oldpos As Integer = _stream.Position
                Dim newpos As Integer = _stream.Seek(offset, SeekOrigin.Current)
                _pos += newpos - oldpos
                Return newpos = oldpos + offset
            End If

            If offset < 0 Then
                Throw New NotSupportedException( _
                    "Can't move backward in this stream!")
            End If

            Dim b(offset - 1) As Byte
            Dim read As Integer = _stream.Read(b, 0, offset)
            _pos += read
            Return read = offset
        End Function

        Private Structure InternalFileInfo
            Public Offset As Integer
            Public Length As Integer
            Public Name As String
        End Structure
        Private Files As New List(Of InternalFileInfo)

        Public Structure FileInfo
            Public Name As String
            Public Size As Integer
        End Structure

        Public Function GetFileNamesAndSizes() As FileInfo()
            Dim fi(Files.Count - 1) As FileInfo
            For i As Integer = 0 To fi.Length - 1
                fi(i) = New FileInfo
                fi(i).Name = Files(i).Name
                fi(i).Size = Files(i).Length
            Next i

            Return fi
        End Function

        Public Function GetFile(ByVal name As String) As MemoryStream
            Dim ifo As InternalFileInfo = Nothing
            For Each x As InternalFileInfo In Files
                If x.Name.ToLower = name.ToLower Then
                    ifo = x
                    Exit For
                End If
            Next x

            If ifo.Name Is Nothing Then
                'Throw New FileNotFoundException("That file is not in the GOB.")
                Return Nothing
            End If

            Seek(ifo.Offset)

            Dim ms As New MemoryStream(ifo.Length)
            Dim b(65536) As Byte
            Dim read As Integer = 1
            While read AndAlso ms.Position < ifo.Length
                read = _stream.Read(b, 0, Math.Min(b.Length, ifo.Length - ms.Position))
                _pos += read
                If read > 0 Then
                    ms.Write(b, 0, read)
                End If
            End While
            ms.Seek(0, SeekOrigin.Begin)
            Return ms
        End Function

        Public Structure PatchInfo
            Public Name As String
            Public Type As String
            Public Description As String
            Public Author As String
            Public Email As String
        End Structure

        Public Function GetModInfo() As PatchInfo
            Dim ms As MemoryStream = Nothing
            Dim pi As New PatchInfo
            Try
                ms = GetFile("patchinfo.txt")
            Catch
                Return pi
            End Try
            If ms Is Nothing Then
                Return pi
            End If

            Dim sr As New StreamReader(ms)
            While Not sr.EndOfStream
                Dim line As String = sr.ReadLine

                Dim equal As Integer = line.IndexOf("="c)
                If equal < 0 Then
                    Continue While
                End If

                Select Case line.Substring(0, equal).ToLower
                    Case "name"
                        pi.Name = line.Substring(equal + 1).Trim
                    Case "type"
                        pi.Type = line.Substring(equal + 1).Trim
                    Case "description"
                        pi.Description = line.Substring(equal + 1).Trim
                    Case "author"
                        pi.Author = line.Substring(equal + 1).Trim
                    Case "email"
                        pi.Email = line.Substring(equal + 1).Trim
                End Select
            End While
            sr.Close()
            sr.Dispose()
            ms.Close()
            ms.Dispose()
            Return pi
        End Function
    End Class
End Namespace

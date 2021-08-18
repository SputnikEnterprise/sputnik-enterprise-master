'------------------------------------
' File: FileInfoExtensionMethods.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Runtime.CompilerServices
Imports System.IO

Namespace FileHandling

    ''' <summary>
    ''' Extension methods for the System.IO.FileInfo class.
    ''' </summary>
    Public Module FileHandlingMethods

        ''' <summary>
        ''' FileInfo extension method.
        ''' Converts a file to a bytes array.
        ''' </summary>
        ''' <returns>File bytes or Nothing</returns>
        <Extension()>
        Public Function ToByteArray(ByVal fileInfo As FileInfo) As Byte()

            Dim data As Byte() = Nothing

            Try

                Dim numberOfBytes As Long = fileInfo.Length

                Dim fileStream As New FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read)

                Dim binaryReader As New BinaryReader(fileStream)

                data = binaryReader.ReadBytes(CInt(numberOfBytes))

                binaryReader.Close()
            Catch ex As Exception
                Throw New ApplicationException(String.Format("The file {0} could not be streamed to a byte array. Exception: {1}", fileInfo.FullName, ex.Message))
            End Try

            Return data

    End Function

    ''' <summary>
    ''' IsFileUsedByAnotherProcess checks if file is used by another process.
    ''' </summary>
    <Extension()>
    Public Function IsFileUsedByAnotherProcess(ByVal fileInfo As FileInfo) As Boolean

      Dim stream As FileStream = Nothing

      Try
        stream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)

      Catch ex As Exception
        Return True

      Finally
        If Not (stream Is Nothing) Then
          stream.Close()
        End If
      End Try

      Return False

    End Function


        ''' <summary>
        ''' DirectoryInfo extension method.
        ''' Deletes as folder.
        ''' </summary>
        ''' <param name="directoryInfo">The folder path.</param>
        ''' <returns>Boolean flag indicating success.</returns>
        <Extension()>
        Public Function DeleteFolder(ByVal directoryInfo As DirectoryInfo) As Boolean

            If String.IsNullOrEmpty(directoryInfo.FullName) Then
                Return False
            End If

            If directoryInfo.Exists Then
                Try
                    ' Delete the folder with all sub folders and files.
                    directoryInfo.Delete(True)

                    Return True
                Catch ex As Exception
                    Throw New ApplicationException(String.Format("The directory {0} could not be deleted. Exception: {1}", directoryInfo.FullName, ex.Message))
                End Try
            End If

            Return False

        End Function

    End Module

End Namespace



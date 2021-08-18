
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.IO

Public Module Win32Api
  'The FindExecutable function retrieves the name and handle to the executable (.EXE) 
  'file associated with the specified filename.
  'Parameters:
  'lpFile
  'Pointer to a null-terminated string specifying a filename. 
  'This can be a document or executable file.
  'lpDirectory
  'Pointer to a null-terminated string specifying the default directory. 
  'lpResult
  'Pointer to a buffer to receive the filename when the function returns. 
  'This filename is a null-terminated string specifying the executable file started 
  'when an "open" association is run on the file specified in the lpFile parameter.
  '
  'Return Values:
  'If the function succeeds, the return value is greater than 32.
  'If the function fails, the return value is less than or equal to 32. 
  'The following table lists the possible error values:
  '
  'Value			Meaning
  '0			The system is out of memory or resources.
  '31			There is no association for the specified file type.
  'ERROR_FILE_NOT_FOUND	The specified file was not found.
  'ERROR_PATH_NOT_FOUND	The specified path was not found.
  'ERROR_BAD_FORMAT	The .EXE file is invalid (non-Win32 .EXE or error in .EXE image).

  Public Declare Auto Function FindExecutable Lib "shell32.dll" _
   (ByVal lpFile As String, _
    ByVal lpDirectory As String, _
    ByVal lpResult As String) As Int32

  Public Const ERROR_FILE_NOT_FOUND As Int32 = 2
  Public Const ERROR_PATH_NOT_FOUND As Int32 = 3
  Public Const ERROR_BAD_FORMAT As Int32 = 11

  Public Declare Auto Function GetLongPathName Lib "kernel32.dll" _
   (ByVal lpszShortPath As String, _
   <MarshalAs(UnmanagedType.LPTStr)> ByVal lpszLongPath As StringBuilder, _
   <MarshalAs(UnmanagedType.U4)> ByVal cchBuffer As Integer) As Int32

End Module

Public Class ClassBrowserPath

  Private BrowserPath As String
  Private msg As String

  Public ReadOnly Property GetBrowserPath() As String
    Get
      Return BrowserPath
    End Get
  End Property

  Public ReadOnly Property ErrorMessage() As String
    Get
      Return msg
    End Get
  End Property

  Public Sub GetBrowserApplicationPath(ByVal Filename As String)
    'If (Path.GetExtension(Filename) = ".htm") Or (Path.GetExtension(Filename) = ".html") Then
    Dim ptFile As String = Filename
    Dim ptDirectory As String = String.Empty
    Const MAX_PATH As Int32 = 255
    Dim Buffer As New String(" "c, MAX_PATH)

    Dim Result As Int32 = FindExecutable(ptFile, ptDirectory, Buffer)
    Select Case Result
      Case Is > 32
        Dim LongPath As String = GetLongPath(Buffer)
        If LongPath <> "" Then
          BrowserPath = LongPath
        Else
          BrowserPath = Buffer
        End If
      Case 0
        msg = "The system is out of memory or resources."
      Case ERROR_FILE_NOT_FOUND
        msg = "The specified file was not found."
      Case ERROR_PATH_NOT_FOUND
        msg = "The specified path was not found."
      Case ERROR_BAD_FORMAT
        msg = "The .EXE file is invalid (non-Win32 .EXE or error in .EXE image)."
      Case 31
        msg = "There is no association for the specified file type"
    End Select
    'Else
    'MessageBox.Show("Necessary file format:  htm or html", "Info")
    'End If
  End Sub

  Private Function GetLongPath(ByVal ShortPath As String) As String
    Dim Buffer As New StringBuilder(256)
    Dim Size As Integer = Buffer.Capacity

    Dim Result As Integer = GetLongPathName(ShortPath, Buffer, Size)
    If Result <> 0 Then
      Return Buffer.ToString()
    Else
      Return ""
    End If
  End Function

End Class


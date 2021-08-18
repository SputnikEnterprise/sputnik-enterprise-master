
Imports System.IO


''' <summary>
''' FTP search view data.
''' </summary>
''' 
Public Class UpdateSettingData
	Public Property CommandID As Integer
	Public Property ProgCommand As String
	Public Property SourceFile As String
	Public Property DestinationFile As String
	Public Property Description As String
	Public Property CommandLine As String
	Public Property SourceFolder As String
	Public Property DestinationFolder As String

End Class

Public Class UpdateCommandData
	Public SourceFolder As DirectoryInfo
	Public DestFolder As DirectoryInfo
	Public SourceFile As FileInfo
End Class

''' <summary>
''' Diese Klasse speichert die Informationen für ein
''' zu kopierendes Verzeichnis der Kopierliste.
''' </summary>
''' <remarks></remarks>
Public Class CopyListEntry
	Public SourceFolder As DirectoryInfo
	Public SearchMask As String
	Public DestFolder As DirectoryInfo
	Public DestFileDescription As String = String.Empty
End Class

''' <summary>
''' Diese Klasse speichert die Informationen für eine 
''' zu kopierende Datei innerhalb eines zu kopierenden 
''' Verzeichnisses.
''' </summary>
''' <remarks></remarks>
Public Class CopyTaskItem
	Public SourceFile As FileInfo
	Public SourcePathPart As String
	Public DestPathPart As String
End Class

Public Class CustomerMDData
	Public Property CustomerID As String
	Public Property LocalIPAddress As String
	Public Property ExternalIPAddress As String
	Public Property LocalHostName As String
	Public Property LocalDomainName As String

End Class

Public Class StationData
	Public Property LocalIPAddress As String
	Public Property ExternalIPAddress As String
	Public Property LocalHostName As String
	Public Property LocalUserName As String
	Public Property LocalDomainName As String

End Class

Public Class FTPUpdateData

	Public Property UpdateID As Integer?
	Public Property UpdateFilename As String
	Public Property FileDestPath As String
	Public Property FileDestVersion As String
	Public Property UpdateFileDate As DateTime?
	Public Property UpdateFileTime As String
	Public Property UpdateFileSize As Long
	Public Property File_Guid As String

	Public Property FileContent As Byte()

	Public Enum FileDestEnum
		NET
		QUERY
		DOCUMENT
		TEMPLATE
		SYSTEM32
		SYSTEM64
		MODUL
	End Enum

	Public ReadOnly Property FileDestinationEnumValue As FileDestEnum
		Get
			If FileDestPath Is Nothing Then Return FileDestEnum.MODUL

			Select Case FileDestPath.ToUpper
				Case "Net\".ToUpper
					Return FileDestEnum.NET
				Case "Documents\".ToUpper
					Return FileDestEnum.DOCUMENT
				Case "Query\".ToUpper
					Return FileDestEnum.QUERY
				Case "Templates\".ToUpper
					Return FileDestEnum.TEMPLATE
				Case "System32\".ToUpper
					Return FileDestEnum.SYSTEM32
				Case "System64\".ToUpper
					Return FileDestEnum.SYSTEM64

				Case Else
					Return FileDestEnum.MODUL

			End Select
		End Get

	End Property

End Class


Public Class UpdateMDData

	Public Property ID As Integer?
	Public Property MDNr As Integer?
	Public Property MDName As String
	Public Property MDPath As String
	Public Property Deaktiviert As Boolean?
	Public Property DbName As String
	Public Property DbConnectionstr As String
	Public Property DbServerName As String
	Public Property Customer_id As String
	Public Property MDGroupNr As Integer?
	Public Property FileServerPath As String


	Public ReadOnly Property FileServerRootPath As String
		Get
			Return Directory.GetDirectoryRoot(MDPath)
		End Get
	End Property


End Class

Public Class EntryLOGData
	Public Property LogDate As DateTime?
	Public Property LogType As String
	Public Property Message As String

End Class


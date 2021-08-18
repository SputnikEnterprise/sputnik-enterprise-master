
Imports System.IO


Public Class CustomerMDData
	Public Property CustomerID As String
	Public Property LocalIPAddress As String
	Public Property ExternalIPAddress As String
	Public Property LocalHostName As String
	Public Property LocalDomainName As String

End Class


''' <summary>
''' FTP search view data.
''' </summary>
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
		SKIN
		MDYEAR
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
				Case "MDYEAR\".ToUpper
					Return FileDestEnum.MDYEAR
				Case "skin\".ToUpper
					Return FileDestEnum.SKIN


				Case Else
					Return FileDestEnum.MODUL

			End Select
		End Get

	End Property

End Class

Public Class ModuleFilesData

	Public Property UpdateID As Integer?
	Public Property UpdateFilename As String
	Public Property ModuleName As String
	Public Property FileDestVersion As String
	Public Property UpdateFileDate As DateTime?
	Public Property UpdateFileTime As String
	Public Property UpdateFileSize As Long
	Public Property File_Guid As String

	Public Property FileContent As Byte()


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



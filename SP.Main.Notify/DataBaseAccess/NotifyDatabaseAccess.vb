
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data

Public Class NotifyDatabaseAccess
	Inherits DatabaseAccessBase


#Region "Private Fields"


	Private m_ConnectionString As String


	''' <summary>
	''' Stores an explicit transaction.
	''' </summary>
	''' <remarks>An explicit transaction can be user across multiple database calls.</remarks>
	Private m_ExplicitTransaction As SqlTransaction

#End Region


#Region "Constructor"

	''' <summary>
	''' Constructor.
	''' </summary>
	''' <param name="connectionString">The connection string.</param>
	Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language, ByVal fileServerPath As String)
		MyBase.New(connectionString, translationLanguage, fileServerPath)
	End Sub

	''' <summary>
	''' Constructor.
	''' </summary>
	''' <param name="connectionString">The connection string.</param>
	''' <param name="translationLanguage">The translation language.</param>
	Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
		MyBase.New(connectionString, translationLanguage)
	End Sub

#End Region



#Region "Public Methods"


	''' <summary>
	''' Loads Mandanten data for Update.
	''' </summary>
	Public Function LoadUpdateMandantData() As IEnumerable(Of MasterMandantData)

		Dim result As List(Of MasterMandantData) = Nothing

		Dim sql As String

		sql = "SELECT ID ,"
		sql &= " MDNr ,"
		sql &= " MDName ,"
		sql &= " MDPath ,"
		sql &= " Deaktiviert ,"
		sql &= " DbName ,"
		sql &= " DbConnectionstr ,"
		sql &= " DbServerName ,"
		sql &= " Customer_id ,"
		sql &= " MDGroupNr ,"
		sql &= " FileServerPath "
		sql &= " FROM [Sputnik DbSelect].dbo.mandanten "
		'sql &= " Where "
		'sql &= " IsNull(Deaktiviert, 1) = 0 "

		sql &= " ORDER BY mdnr"

		Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MasterMandantData)

				While reader.Read

					Dim overviewData = New MasterMandantData

					overviewData.ID = SafeGetInteger(reader, "ID", 0)
					overviewData.MDNr = SafeGetInteger(reader, "MDNr", 0)
					overviewData.MDName = SafeGetString(reader, "MDName")
					overviewData.MDPath = SafeGetString(reader, "MDPath")
					overviewData.Deaktiviert = SafeGetBoolean(reader, "Deaktiviert", False)
					overviewData.DbName = SafeGetString(reader, "DbName")
          overviewData.DbConnectionstr = GetDbConnectionString(overviewData.MDNr, Path.GetFullPath(Path.Combine(overviewData.MDPath, "..\")))
          overviewData.DbServerName = SafeGetString(reader, "DbServerName")
					overviewData.Customer_id = SafeGetString(reader, "Customer_id")
					overviewData.MDGroupNr = SafeGetInteger(reader, "MDGroupNr", 0)
					overviewData.FileServerPath = SafeGetString(reader, "FileServerPath")

					If Not String.IsNullOrWhiteSpace(overviewData.DbConnectionstr) AndAlso File.Exists(Path.Combine(overviewData.FileServerPath, Now.Year, "programm.xml")) Then
						result.Add(overviewData)
					End If

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			CloseReader(reader)
		End Try

		Return result

	End Function

	''' <summary>
	''' Loads FTP-Update data.
	''' </summary>
	Public Function LoadUploadedData() As IEnumerable(Of FTPUpdateData)

		Dim result As List(Of FTPUpdateData) = Nothing

		Dim sql As String

		sql = "SELECT Distinct UpdateFileName, File_Guid, UpdateFileDate, UpdateFileTime, createdon "
		sql &= "FROM [Sputnik DbSelect].dbo.SpUpdateHistory Where File_Guid Is Not Null And IsNull(File_Guid, '') <> '' ORDER BY createdon desc"

		Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FTPUpdateData)

				While reader.Read

					Dim dto = New FTPUpdateData
					dto.UpdateID = Nothing
					dto.UpdateFilename = SafeGetString(reader, "UpdateFileName")
					dto.File_Guid = SafeGetString(reader, "File_Guid")
					dto.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)
					dto.UpdateFileTime = SafeGetString(reader, "UpdateFileTime")

					dto.FileContent = Nothing


					result.Add(dto)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			CloseReader(reader)
		End Try

		Return result

	End Function

	''' <summary>
	''' Loads FTP-Update data.
	''' </summary>
	Public Function AddDownloadedData(ByVal data As FTPUpdateData) As Boolean

		Dim result As Boolean = True

		Dim sql As String

		sql = "Insert Into "
		sql &= "[Sputnik DbSelect].dbo.SpUpdateHistory ("
		sql &= " UpdateFileName"
		sql &= ", UpdateFileVersion"
		sql &= ", File_Guid"
		sql &= ", UpdateFileDate"
		sql &= ", UpdateFileTime"
		sql &= ", UpdateFileSize"
		sql &= ", Result"
		sql &= ", createdon "
		sql &= " )"

		sql &= " Values ("

		sql &= " @UpdateFileName"
		sql &= ", @UpdateFileVersion"
		sql &= ", @File_Guid"
		sql &= ", @UpdateFileDate"
		sql &= ", @UpdateFileTime"
		sql &= ", @UpdateFileSize"
		sql &= ", 1"
		sql &= ", GetDate()"
		sql &= " )"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@UpdateFileName", ReplaceMissing(data.UpdateFilename, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("@UpdateFileVersion", ReplaceMissing(data.FileDestVersion, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("@File_Guid", ReplaceMissing(data.File_Guid, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("@UpdateFileDate", ReplaceMissing(data.UpdateFileDate, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("@UpdateFileSize", ReplaceMissing(data.UpdateFileSize, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("@UpdateFileTime", ReplaceMissing(data.UpdateFileTime, DBNull.Value)))

		result = ExecuteNonQuery(sql, listOfParams)


		Return result

	End Function


	''' <summary>
	''' Loads Mandanten data for Update.
	''' </summary>
	Public Function ExecuteAssignedSQLScript(ByVal scriptFile As String, ByVal connStr As String) As Boolean
		Dim result As Boolean = True

		Dim file As New FileInfo(scriptFile)
		Dim script As String = file.OpenText().ReadToEnd()
		Dim argValue As String

		result = result AndAlso OpenExplicitConnection(connStr)

		Dim dSource As String = m_ExcplicitConnection.DataSource
		Dim dbase As String = m_ExcplicitConnection.Database
		Dim tmpFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Sputnik\SputnikFTPUpdate")
		Dim resultFile As String = Path.Combine(tmpFolder, String.Format("Query-Log {0}.log", dbase))


		'1: Servername
		'2: DbName
		'3: filename
		'4: resultfilename

		'Syntax: Sqlcmd           [-U Anmelde-ID]        [-P Kennwort]
		'  [-S Server]            [-H Hostname]          [-E vertrauenswürdige Verbindung]
		'  [-N Verbindung verschlüsseln][-C Serverzertifikat vertrauen]
		'  [-d Datenbanknamen verwenden] [-l Anmeldetimeout]     [-t Abfragetimeout]
		'  [-h Header]            [-w Bildschirmbreite]  [-s Spaltentrennzeichen]
		'  [-a Paketgröße]        [-z neues Kennwort]    [-I Bezeichner in Anführungszeichen aktivieren]
		'  [-c Befehlsende]       [-L[c] Server auflisten [einfache Ausgabe]]
		'  [-q 'Befehlsz.abfr.']  [-Q 'Befehlszeilenabfrage' und beenden]
		'  [-m Fehlerebene]       [-V Schweregrad]       [-W nachfolgende Leerzeichen entfernen]
		'  [-u Unicode-Ausgabe]   [-r[0|1] Meldungen nach STDERR]
		'  [-i Eingabedatei]      [-o Ausgabedatei]      [-e Eingaben zurückgeben]
		'  [-f <codepage> | i:<codepage>[, o:<codepage>]] [-Z neues Kennwort und beenden]
		'  [-k[1|2] Steuerzeichen entfernen [ersetzen]]
		'  [-y variable Anzeigebreite]
		'  [-Y feste Anzeigebreite]
		'  [-p[1] Statistik ausgeben  [Spaltenformat]]
		'  [-R Clienteinstellung für Land/Region verwenden]
		'  [-b Batchabbruch bei Fehler]
		'  [-v Variable = 'Wert'...]  [-A dedizierte Administratorverbindung]
		'  [-X[1] Befehle, Startskript, Umgebungsvariablen deaktivieren [und beenden]]
		'  [-x Variablenersetzung deaktivieren]
		'  [-? Syntaxzusammenfassung anzeigen]
		' sqlcmd -S "Servername" -d "Sputnik DbName" -E -i "1_9_2016_setting.sql" -o "C:\Path\Query.csv"

		argValue = ""
		argValue &= " -S {0}{1}{0}"
		argValue &= " -d {0}{2}{0}"
		argValue &= " -E"
		argValue &= " -i {0}{3}{0}"
		argValue &= " -o {0}{4}{0}"

		argValue = String.Format(argValue, Chr(34), dSource, dbase, scriptFile, resultFile)
		Try

			Dim startInfo As New ProcessStartInfo("Sqlcmd.exe")
			startInfo.Arguments = argValue
			startInfo.UseShellExecute = False

			m_Logger.LogInfo(String.Format("running sql-query with following argumenst: {0}", argValue))

			Process.Start(startInfo)


		Catch ex As Exception
			result = False
			m_Logger.LogError(String.Format("error during running sql-query: {0} | {1}", argValue, ex.ToString()))

		End Try


		Return result

	End Function


#End Region


#Region "Helpers"

	'Private Function GetDbConnectionString(ByVal mandantNumber As Integer, ByVal mdPath As String) As String
	'	Dim result As String = String.Empty
	'	Dim GetFileServerXMLFullFilename = Path.Combine(Directory.GetDirectoryRoot(mdPath), "Bin\Programm.xml")

	'	Try

	'		Dim xDoc As XDocument = XDocument.Load(GetFileServerXMLFullFilename)
	'		Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("databaseconnections").Elements(String.Format("md_{0}", mandantNumber))
	'											 Select New With {.mdconnstr = GetSafeStringFromXElement(exportSetting.Element("connstr")),
	'												 .mddbname = GetSafeStringFromXElement(exportSetting.Element("dbname")),
	'												 .mddbserver = GetSafeStringFromXElement(exportSetting.Element("dbname"))
	'												 }).FirstOrDefault()

	'		result = FromBase64(ConfigQuery.mdconnstr)


	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString())
	'		Return result

	'	End Try


	'	Return result

	'End Function


	'Private Function GetSafeStringFromXElement(ByVal xelment As XElement)

	'	If xelment Is Nothing Then
	'		Return String.Empty
	'	Else

	'		Return xelment.Value
	'	End If

	'End Function

	'Private Function FromBase64(ByVal sText As String) As String
	'	' Base64-String zunächst in ByteArray konvertieren
	'	Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

	'	' ByteArray in String umwandeln
	'	Return System.Text.Encoding.Default.GetString(nBytes)
	'End Function


#End Region


End Class




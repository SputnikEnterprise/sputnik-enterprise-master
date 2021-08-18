
Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections.Generic

Imports NLog
Imports System.Linq

Module FuncLvg

	Private logger As Logger = LogManager.GetCurrentClassLogger()

	Private _ClsSystem As New ClsMain_Net

	Function LoadMDEntries(ByVal sRecIndex As Short, ByVal Conn As SqlConnection) As IEnumerable(Of ClsLogingMDData)
		Dim result As List(Of ClsLogingMDData) = Nothing
		Dim utility As New SPProgUtility.MainUtilities.Utilities
		Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sql As String = "[List Mandant Data For Selecting Mandant]"
		Dim strFullFilename As String = String.Empty
		Dim strMDPath As String = String.Empty
		Dim bIsCreatedNewYear As Boolean = True


		Dim bShowItem As Boolean = True
		Dim rMDrec = utility.OpenReader(Conn.ConnectionString, sql, Nothing, CommandType.StoredProcedure)

		result = New List(Of ClsLogingMDData)

		Try
			While rMDrec.Read()

				strMDPath = rMDrec("MDPath").ToString
				If Not strMDPath.EndsWith("\") Then strMDPath &= "\"
				strFullFilename = Path.Combine(strMDPath, Year(Today), "PROGRAMM.DAT")

				If Not System.IO.File.Exists(strFullFilename) Then
					strFullFilename = Path.Combine(strMDPath, Year(Today) - 1, "PROGRAMM.DAT")
					bIsCreatedNewYear = False
				End If

				If System.IO.File.Exists(strFullFilename) Then
					Try
						If Not bIsCreatedNewYear Then
							If Not System.IO.Directory.Exists(strMDPath & Year(Today) & "\LOGS") Then
								System.IO.Directory.CreateDirectory(strMDPath & Year(Today) & "\LOGS")
							End If
						End If
						bShowItem = True

					Catch ex As Exception
						logger.Error(String.Format("{0}.DirectoryCheck:{1}", strMethodeName, ex.ToString))
						bShowItem = False
					End Try


					If bShowItem Then
						Dim overviewData As New ClsLogingMDData

						overviewData.RecNr = rMDrec("ID").ToString

						overviewData.MDNr = rMDrec("MDNr").ToString
						overviewData.MDName = rMDrec("MDName").ToString
						overviewData.MDMainPath = rMDrec("MDPath").ToString
						overviewData.MDGuid = rMDrec("Customer_ID").ToString

						overviewData.MDGroupNr = rMDrec("MDGroupNr").ToString
						overviewData.MDDbServer = rMDrec("FileServerPath").ToString

						overviewData.RootDbServer = Conn.DataSource
						overviewData.RootDbName = Conn.Database
						overviewData.RootDbConn = Conn.ConnectionString

						overviewData.MDDbConn = ""
						overviewData.MDDbName = ""

						Dim tempFile = Path.Combine(overviewData.MDMainPath, "Profiles", Path.GetRandomFileName)
						Try
							File.Create(tempFile).Dispose()
							File.Delete(tempFile)
							logger.Info(String.Format("searching for security: {0}", tempFile))

							result.Add(overviewData)

						Catch ex As Exception
							logger.Warn(String.Format("directory access denied: {0}", overviewData.MDMainPath))
							logger.Warn(String.Format("denied write and delete security: {0}", tempFile))

						End Try

					End If

				End If

			End While

		Catch ex As Exception
			result = Nothing
			logger.Error(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

		Return result

	End Function

	Private Function GetDbConnectionString(ByVal mandantNumber As Integer, ByVal sputnikRootPath As String) As String
		Dim result As String = String.Empty
		Dim GetFileServerXMLFullFilename = Path.Combine(sputnikRootPath, "Bin\Programm.xml")

		Try
			logger.Info(String.Format("programm.xml is located on: {0} | mandantNumber: {1} | sputnikRootPath: {2}", GetFileServerXMLFullFilename, mandantNumber, sputnikRootPath))
			If Not File.Exists(GetFileServerXMLFullFilename) Then
				Throw New Exception(String.Format("setting file was not founded: {0}", GetFileServerXMLFullFilename))
			End If
			Dim xDoc As XDocument = XDocument.Load(GetFileServerXMLFullFilename)
			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("databaseconnections").Elements(String.Format("md_{0}", mandantNumber))
												 Select New With {.mdconnstr = GetSafeStringFromXElement(exportSetting.Element("connstr")),
													 .mddbname = GetSafeStringFromXElement(exportSetting.Element("dbname")),
													 .mddbserver = GetSafeStringFromXElement(exportSetting.Element("dbname"))
													 }).FirstOrDefault()

			result = FromBase64(ConfigQuery.mdconnstr)


		Catch ex As Exception
			logger.Error(String.Format("mandantNumber: {0} | sputnikRootPath: {1} | {2}", mandantNumber, sputnikRootPath, ex.ToString()))
			Return result

		End Try


		Return result

	End Function

	Function EncryptMyString(ByVal strData As String, ByVal strCryptKey As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

	Public Function Decrypt(ByVal sQueryString As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

	Public Function Encrypt(ByVal sInputVal As String) As String
		Dim value As String = ""

		' put hier your code for Decrypt / decrypting
		Return value
	End Function

	Private Function GetSafeStringFromXElement(ByVal xelment As XElement)

		If xelment Is Nothing Then
			Return String.Empty
		Else

			Return xelment.Value
		End If

	End Function

	Private Function FromBase64(ByVal sText As String) As String
		' Base64-String zunächst in ByteArray konvertieren
		Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

		' ByteArray in String umwandeln
		Return System.Text.Encoding.Default.GetString(nBytes)
	End Function



End Module
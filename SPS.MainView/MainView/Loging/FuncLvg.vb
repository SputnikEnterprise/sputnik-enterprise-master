
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.Infrastructure.UI.UtilityUI
Imports SP.Infrastructure.Settings

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.DXperience.Demos.TutorialControlBase
Imports System.Data.SqlClient

Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SPS.MainView.EmployeeSettings

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility


Module FuncLvg

	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Function LoadMDEntries(ByVal sRecIndex As Short, ByVal Conn As SqlConnection) As IEnumerable(Of ClsLogingMDData)
		Dim result As List(Of ClsLogingMDData) = Nothing

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = "[List Mandant Data For Selecting Mandant]"
		Dim strFullFilename As String = String.Empty
		Dim strMDPath As String = String.Empty
		Dim bIsCreatedNewYear As Boolean = True

		Dim cmd As SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim rMDrec As SqlDataReader = cmd.ExecuteReader
		Dim bShowItem As Boolean = True

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

				If Not String.IsNullOrWhiteSpace(strFullFilename) AndAlso System.IO.File.Exists(strFullFilename) Then
					Try
						If Not bIsCreatedNewYear Then
							If Not System.IO.Directory.Exists(strMDPath & Year(Today) & "\LOGS") Then
								System.IO.Directory.CreateDirectory(strMDPath & Year(Today) & "\LOGS")
							End If
							My.Computer.FileSystem.WriteAllText(strMDPath & Year(Today) & "\LOGS\log.txt",
																									Now & vbTab & "SPSEnterprise.EXE" & vbTab & My.User.Name & vbNewLine,
																									True)
						End If
						bShowItem = True

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.DirectoryCheck:{1}", strMethodeName, ex.Message))
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


						Dim tempFile = Path.Combine(overviewData.MDMainPath, Year(Now), Path.GetRandomFileName)
						Try
							File.Create(tempFile).Dispose()
							File.Delete(tempFile)
							m_Logger.LogDebug(String.Format("searching for security: {0}", tempFile))

							result.Add(overviewData)

						Catch ex As Exception
							m_Logger.LogWarning(String.Format("directory access denied: {0}", overviewData.MDMainPath))
							m_Logger.LogDebug(String.Format("denied write and delete security: {0}", tempFile))

						End Try

					End If

				End If

			End While

		Catch ex As Exception
			result = Nothing
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

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

End Module

Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading

Imports SP.Infrastructure.Logging
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten


Module Utilities

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsDivFunc As New ClsDivFunc

  Private Property bBodyAsHtml As Boolean
  Private Property strBez2Translate As String
  Private m_xml As New ClsXML


  Private m_Utility As New SPProgUtility.MainUtilities.Utilities
	Private m_mandant As New Mandant
	Private m_common As New CommonSetting
	Private m_path As New ClsProgPath
	Private m_UtilityUI As New SP.Infrastructure.UI.UtilityUI

	Public ReadOnly Property SPSMAHomeFolder As String
		Get
			Dim m_path As New ClsProgPath
			Return m_path.GetSpSMAHomeFolder
		End Get
	End Property

	Public ReadOnly Property SelectedCategorieNumber As List(Of String)
		Get
			Dim m_path As New ClsProgPath
			Dim m_md As New Mandant
			Dim selectedCategoryNumber = m_md.GetSelectedMDProfilValue(ClsDataDetail.ProgSettingData.SelectedMDNr, Now.Year, "Templates",
																	   "proposemaildocumentcategorynumber", String.Empty).Split(New Char() {";", ",", "#"}, StringSplitOptions.RemoveEmptyEntries).ToList

			Return selectedCategoryNumber
		End Get
	End Property


	Function LoadSenderEMailData(ByVal proposeNumber As Integer) As IEnumerable(Of SenderEMailData)
		Dim result As List(Of SenderEMailData) = Nothing

		Dim SQL As String

		SQL = "[Get All UserNumber For Sender In Propose]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("proposeNumber", m_Utility.ReplaceMissing(proposeNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, SQL, listOfParams, CommandType.StoredProcedure)

		Try

			result = New List(Of SenderEMailData)
			If ClsDataDetail.UserData.UsereMail <> ClsDataDetail.UserData.UserMDeMail Then
				result.Add(New SenderEMailData With {.UserEMail = ClsDataDetail.UserData.UserMDeMail})
			End If
			result.Add(New SenderEMailData With {.UserEMail = ClsDataDetail.UserData.UsereMail})

			If (Not reader Is Nothing AndAlso reader.Read()) Then
				Dim CustomerUserNumber As Integer = m_Utility.SafeGetInteger(reader, "KDUsNr", 0)
				Dim EmployeeUserNumber As Integer = m_Utility.SafeGetInteger(reader, "MAUsNr", 0)

				Dim customerUserData = GetSelectedMailData(CustomerUserNumber)
				Dim employeeUserData = GetSelectedMailData(EmployeeUserNumber)

				If Not customerUserData Is Nothing Then
					If Not customerUserData.MDEMail Is Nothing AndAlso customerUserData.UserEMail <> customerUserData.MDEMail Then
						result.Add(New SenderEMailData With {.UserEMail = customerUserData.MDEMail})
					End If
					result.Add(New SenderEMailData With {.UserEMail = customerUserData.UserEMail})

				End If
				If Not employeeUserData Is Nothing Then
					If Not employeeUserData.MDEMail Is Nothing AndAlso employeeUserData.UserEMail <> employeeUserData.MDEMail Then
						result.Add(New SenderEMailData With {.UserEMail = employeeUserData.MDEMail})
					End If
					result.Add(New SenderEMailData With {.UserEMail = employeeUserData.UserEMail})
				End If

			End If


		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally

		End Try

		Return result
	End Function

	Function GetSelectedMailData(ByVal userNumber As Integer) As UserData
		Dim result As UserData = Nothing

		Dim SQL As String = String.Empty

		SQL &= "[Get Selected Userdata]"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("USNr", m_Utility.ReplaceMissing(userNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, SQL, listOfParams, CommandType.StoredProcedure)

		Try

			result = New UserData
			If (Not reader Is Nothing AndAlso reader.Read()) Then

				Dim overviewData As New UserData

				overviewData.UserEMail = m_Utility.SafeGetString(reader, "USeMail")
				overviewData.MDEMail = m_Utility.SafeGetString(reader, "MDeMail")

				result = overviewData

			End If


		Catch ex As Exception
			result = Nothing
			m_Logger.LogError(ex.ToString())

		Finally

		End Try

		Return result
	End Function

	Function GetKandidatenDaten(ByVal employeeNumber As Integer) As List(Of String)
		Dim liResult As New List(Of String)
		Dim strQuery As String = String.Empty

		Try
			Dim sql = "Select Top 1 MA.eMail As MAeMail, MA.Nachname As MANachname, MA.Vorname As MAVorname "
			sql &= "From Mitarbeiter MA Where MA.MANr = @MANr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", m_Utility.ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			If (Not reader Is Nothing AndAlso reader.Read()) Then
				liResult.Add(String.Format("{0}#{1}#{2}", m_Utility.SafeGetString(reader, "MAeMail"), m_Utility.SafeGetString(reader, "MAVorname"), m_Utility.SafeGetString(reader, "MANachname")))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally

		End Try

		Return liResult
	End Function

	Function LoadEmployeeDocumentsForProposeAttachment(ByVal employeeNumber As Integer) As IEnumerable(Of EmployeeDocumentData)
		Dim result As List(Of EmployeeDocumentData) = Nothing
		Dim aDocFiles As New List(Of String)
		Dim strFilePath As String = SPSMAHomeFolder '  m_path.GetSpSMAHomeFolder
		Dim strFullFilename As String = String.Empty
		Dim selectedCategoryNumber = SelectedCategorieNumber
		Dim addToList As Boolean = True

		Try

			Dim sql = "[Get Employee Documents For Selecting in Propose EMal Attachment]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", m_Utility.ReplaceMissing(employeeNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			result = New List(Of EmployeeDocumentData)

			While reader.Read
				Dim data = New EmployeeDocumentData()

				data.WatchCategory = True

				data.RecGuid = m_Utility.SafeGetString(reader, "RecID")
				data.CategorieNumber = m_Utility.SafeGetInteger(reader, "Categorie_Nr", 0)
				data.Bezeichnung = m_Utility.SafeGetString(reader, "Bezeichnung")
				data.LLArt = m_Utility.SafeGetString(reader, "LLArt")
				data.EmployeeFirstname = m_Utility.SafeGetString(reader, "Vorname")
				data.EmployeeLastname = m_Utility.SafeGetString(reader, "Nachname")
				data.ScanExtension = m_Utility.SafeGetString(reader, "ScanExtension")
				data.DocContent = m_Utility.SafeGetByteArray(reader, "MyFile")
				data.Checked = False


				If data.WatchCategory AndAlso Not String.IsNullOrWhiteSpace(data.EmployeeFilename) Then result.Add(data)

			End While

		Catch ex As Exception
			Dim msg As String = ex.ToString
			Dim isTimeout As Boolean = False
			If msg.ToLower.Contains("Timeout".ToLower) Then
				isTimeout = True
				msg = String.Format("Es ist ein timeout in der Datenbank aufgetretten. Bitte versuchen Sie den Vorgangen zu einem späteren Zeitpunkt.{0}{1}", vbNewLine, msg)
			End If
			m_Logger.LogError(String.Format("{0}", msg))
			m_UtilityUI.ShowErrorDialog(msg)

			Return Nothing

		Finally

		End Try

		Return result
	End Function

	Function CreateFileFromTempTable(ByVal strRecID As String, ByVal strFileName As String) As String
		Dim aDocFiles As New List(Of String)
		Dim strResult As String = "Erfolgreich..."

		Try
			If File.Exists(strFileName) Then File.Delete(strFileName)

		Catch ex As Exception

		End Try
		Try
			Dim sql = "Select Top 1 Convert(nvarchar(70), RecID) RecID, MyFile, Bezeichnung, LLArt, ScanExtension, Convert(Int, MANr) MANr From _MADocuments_{0} Where RecID = @RecID"
			sql = String.Format(sql, ClsDataDetail.GetProposalMANr)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RecID", m_Utility.ReplaceMissing(strRecID, 0)))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			While reader.Read
				Try
					Dim BA As Byte()
					BA = CType(m_Utility.SafeGetByteArray(reader, "MyFile"), Byte())

					Dim ArraySize As New Integer
					ArraySize = BA.GetUpperBound(0)

					Dim fs As New FileStream(strFileName, FileMode.CreateNew)
					fs.Write(BA, 0, ArraySize + 1)
					fs.Close()
					fs.Dispose()

					aDocFiles.Add(String.Format("{0}#{1}", strFileName, m_Utility.SafeGetString(reader, "RecID")))

				Catch ex As Exception
					Dim strMessage As String = "Möglicherweise konnten nicht alle Kandidatendokumente übernommen werden."
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(m_xml.GetSafeTranslationValue(strMessage))
					strResult = String.Format("Fehler CreateFileFromTempTable_2: {0}", ex.Message)

				End Try
			End While

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			strResult = String.Format("Fehler CreateFileFromTempTable: {0}", ex.Message)

		Finally

		End Try

		Return strResult
	End Function

	Function GetKundenDaten(ByVal iKDNr As Integer, ByVal iZHDNr As Integer?) As List(Of String)
		Dim liResult As New List(Of String)

		Try
			Dim sql As String

			sql = "Select Top 1 KD.eMail As KDeMail, KD.Firma1 As Firma1, "
			sql &= "ZHD.Anrede As ZHDAnrede, ZHD.Anredeform As ZHDAnredeForm, "
			sql &= "ZHD.Nachname As ZHDNachname, ZHD.Vorname As ZHDVorname, "
			sql &= "ZHD.eMail As ZHDeMail "
			sql &= "From Kunden KD Left Join KD_Zustaendig ZHD On KD.KDNr = ZHD.KDNr "
			sql &= "Where KD.KDNr = @iKDNr And (@iZHDNr = 0 Or ZHD.RecNr = @iZHDNr)"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("iKDNr", m_Utility.ReplaceMissing(iKDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("iZHDNr", m_Utility.ReplaceMissing(iZHDNr, 0)))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			If (Not reader Is Nothing AndAlso reader.Read()) Then
				liResult.Add(String.Format("{0}#{1}", m_Utility.SafeGetString(reader, "KDeMail"), m_Utility.SafeGetString(reader, "Firma1")))

				If iZHDNr.HasValue Then
					liResult.Add(String.Format("{0}#{1} {2}", m_Utility.SafeGetString(reader, "ZHDeMail"),
																		 m_Utility.SafeGetString(reader, "ZHDVorname"),
																		 m_Utility.SafeGetString(reader, "ZHDNachname")))
				End If

			End If



		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Return liResult
	End Function

	Function ListMailTemplateName() As IEnumerable(Of MailTemplateData)
		Dim result As List(Of MailTemplateData) = Nothing

		Try
			Dim sql = "Select DocDb.Bezeichnung, DocDb.JobNr, DocDb.DocName From Dokprint DocDb Where DocDb.JobNr Like @JobNr Order By RecNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("JobNr", "Propose.Mail%"))

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			Dim i As Integer = 0
			result = New List(Of MailTemplateData)

			While reader.Read
				Dim data As New MailTemplateData
				data.ID = i + 1
				data.itemBez = m_Utility.SafeGetString(reader, "Bezeichnung")
				Dim tplFilename As String = m_Utility.SafeGetString(reader, "DocName")

				If File.Exists(Path.Combine(m_mandant.GetSelectedMDTemplatePath(ClsDataDetail.ProgSettingData.SelectedMDNr), tplFilename)) Then
					data.itemValue = tplFilename

					result.Add(data)
				End If

				i += 1
			End While

			If i = 0 Then
				Dim strMessage As String = "Keine Daten wurden gefunden. Bitte kontrollieren Sie Ihre Auswahl und versuchen Sie es erneuert."
				m_UtilityUI.ShowInfoDialog(m_xml.GetSafeTranslationValue(strMessage))

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("***Fehler (ListTemplateName): {0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Return result
	End Function



#Region "Funktionen zum Kontakt erfassen..."


	Function GetNewKontaktNr(ByVal lKDNr As Integer, ByVal lKDZNr As Integer?, ByVal iMANr As Integer) As Integer
		Dim lRecNr As Integer = 0
		Dim strTableName As String = String.Format("{0}", If(iMANr = 0, "KD_KontaktTotal", "MA_Kontakte"))

		Try
			Dim sql = String.Format("Select Top 1 IsNull(ID, 0) ID From {0} Order By ID Desc", strTableName)

			Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, sql, Nothing, CommandType.Text)


			If (Not reader Is Nothing AndAlso reader.Read()) Then
				lRecNr = m_Utility.SafeGetInteger(reader, "ID", 0)
			End If
			lRecNr += 1


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

		Return lRecNr
	End Function


#End Region


  Private Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
    target = value

    Return value
  End Function

End Module


Public Class EmployeeDocumentData

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Public Property RecGuid As String
	Public Property Bezeichnung As String
	Public Property LLArt As String
	Public Property ScanExtension As String
	Public Property EmployeeLastname As String
	Public Property EmployeeFirstname As String
	Public Property CategorieNumber As Integer?
	Public Property DocContent As Byte()
	Public Property WatchCategory As Boolean?
	Public Property Checked As Boolean?


	Public ReadOnly Property EmployeeFullname As String
		Get
			Return String.Format("{0} {1}", EmployeeFirstname, EmployeeLastname)
		End Get
	End Property

	Public ReadOnly Property EmployeeFilename As String
		Get
			'Return String.Format("{0} ({1:F0} KB)", Path.GetFileName(EmployeeDocumentFullFileName), (DocContent.Length / 1024))
			Return String.Format("{0}", Path.GetFileName(EmployeeDocumentFullFileName))
		End Get
	End Property

	Public ReadOnly Property EmployeeDocumentFullFileName As String
		Get
			Dim filename As String = String.Empty
			Dim strFile2Save As String = String.Empty
			Dim strFilePath As String = SPSMAHomeFolder
			Dim selectedCategoryNumber = SelectedCategorieNumber
			Dim tempLLArt = LLArt

			If String.IsNullOrWhiteSpace(tempLLArt) Then
				strFile2Save = Bezeichnung

			Else
				If tempLLArt.ToLower = "standard" Then
					tempLLArt = String.Empty
				Else
					tempLLArt &= "_"
				End If
				strFile2Save = If(String.IsNullOrEmpty(tempLLArt), "", tempLLArt) & String.Format("Dossier {0} {1}", EmployeeLastname, EmployeeFirstname)
			End If
			strFile2Save = strFile2Save.Replace(":", "").Replace("*", "").Replace("?", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace(vbTab, String.Empty)
			'strFile2Save = Regex.Replace(strFile2Save, "[^\w\\-]", "")
			If Not FilenameIsOK(strFile2Save) Then strFile2Save = Path.GetTempFileName

			If strFile2Save.Contains("\") Then strFile2Save = Path.GetFileNameWithoutExtension(strFile2Save)
			Dim strFullFilename = Path.Combine(strFilePath, String.Format("{0}.{1}", strFile2Save, ScanExtension))
			If Not WatchCategory.GetValueOrDefault(False) Then Return strFullFilename

			If String.IsNullOrWhiteSpace(tempLLArt) AndAlso selectedCategoryNumber.Count > 0 Then
				filename = strFullFilename
				For Each itm In selectedCategoryNumber
					If Val(itm) > 0 Then
						If CategorieNumber = Val(itm) Then
							filename = strFullFilename

							Exit For
						Else
							filename = String.Empty
						End If
					End If
				Next
			Else
				filename = strFullFilename

			End If


			Return filename

		End Get
	End Property

	Private Function FilenameIsOK(ByVal fileName As String) As Boolean
		Dim result As Boolean = True
		Dim file As String = String.Empty
		Dim directory As String = String.Empty
		'Dim cleanFilename As String = Regex.Replace(fileName, "[^\w\\-]", "") ' "[^A-Za-z0-9\-/]", "")

		Try
			file = Path.GetFileName(fileName)
			directory = Path.GetDirectoryName(fileName)

			result = Not (file.Intersect(Path.GetInvalidFileNameChars()).Any() OrElse directory.Intersect(Path.GetInvalidPathChars()).Any())

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0} | {1} >>> {2}", Directory, fileName, ex.ToString))
			result = False
		End Try

		Return result

	End Function

	Private Function IsValidFileNameOrPath(ByVal fileName As String, Optional ByVal allowPathDefinition As Boolean = False, Optional ByRef firstCharIndex As Integer = Nothing) As Boolean

		Dim file As String = String.Empty
		Dim directory As String = String.Empty

		If allowPathDefinition Then
			file = Path.GetFileName(fileName)
			directory = Path.GetDirectoryName(fileName)
		Else
			file = fileName
		End If

		If Not IsNothing(firstCharIndex) Then
			Dim f As IEnumerable(Of Char)
			f = file.Intersect(Path.GetInvalidFileNameChars())
			If f.Any Then
				firstCharIndex = Len(directory) + file.IndexOf(f.First)
				Return False
			End If

			f = directory.Intersect(Path.GetInvalidPathChars())
			If f.Any Then
				firstCharIndex = directory.IndexOf(f.First)
				Return False
			Else
				Return True
			End If
		Else
			Return Not (file.Intersect(Path.GetInvalidFileNameChars()).Any() OrElse directory.Intersect(Path.GetInvalidPathChars()).Any())
		End If


	End Function


End Class

Public Class SenderEMailData
	Public Property UserEMail As String

End Class

Public Class UserData
	Public Property UserEMail As String
	Public Property MDEMail As String

End Class

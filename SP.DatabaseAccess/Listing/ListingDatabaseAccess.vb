
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language

Namespace Listing

	''' <summary>
	''' Listing database access class.
	''' </summary>
	Public Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess

#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region



		Function LoadAssignedPrintJobData(ByVal mandantenNumber As Integer, ByVal language As String, ByVal jobnr As String) As PrintJobData Implements IListingDatabaseAccess.LoadAssignedPrintJobData
			Dim result As PrintJobData = Nothing
			Dim m_Mandant As New Mandant
			Dim docPath As String = m_Mandant.GetSelectedMDDocPath(mandantenNumber)

			Dim sql As String

			If language Is Nothing Then language = String.Empty
			Select Case True
				Case language.ToLower().TrimEnd() = "fr", language.ToLower().TrimEnd() = "f"
					language = "F"

				Case language.ToLower().TrimEnd() = "it", language.ToLower().TrimEnd() = "i"
					language = "I"

				Case language.ToLower().TrimEnd() = "en", language.ToLower().TrimEnd() = "e"
					language = "E"


				Case Else
					language = ""
			End Select

			sql = "Select JobNr"
			sql &= ",DocName"
			sql &= ",Bezeichnung"
			sql &= ",IsNull(ParamCheck, 0) ParamCheck"
			sql &= ",KonvertName"
			sql &= ",Convert(Int, ZoomProz) ZoomProz"
			sql &= ",Convert(Int, Anzahlkopien) Anzahlkopien "
			sql &= ",TempDocPath"
			sql &= ",ExportedFileName"

			sql &= " From DokPrint"
			sql &= " Where (DocName <> '' And DocName Is Not Null)"
			sql &= " And [JobNr] = @JobNr "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("JobNr", ReplaceMissing(jobnr, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				result = New PrintJobData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.JobNumber = SafeGetString(reader, "JobNr")
					Dim docName = IO.Path.Combine(docPath, language, SafeGetString(reader, "DocName"))
					If Not IO.File.Exists(docName) Then
						Dim newDocName = IO.Path.Combine(docPath, SafeGetString(reader, "DocName"))

						m_Logger.LogError(String.Format("Document not founded: {0} New Documentname: {1}", docName, newDocName))
						docName = newDocName

					End If

					result.LLDocName = docName
					result.LLDocLabel = SafeGetString(reader, "Bezeichnung")
					result.LLParamCheck = SafeGetBoolean(reader, "ParamCheck", False)
					result.LLKonvertName = SafeGetBoolean(reader, "KonvertName", False)
					result.LLZoomProz = SafeGetInteger(reader, "ZoomProz", 100)
					result.LLCopyCount = SafeGetInteger(reader, "Anzahlkopien", 1)
					result.LLExportedFilePath = SafeGetString(reader, "TempDocPath")
					result.LLExportedFileName = IO.Path.GetFileNameWithoutExtension(SafeGetString(reader, "ExportedFileName"))

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result


		End Function


		Function LoadUserAndMandantData(ByVal mandantenNumber As Integer, ByVal userFirstname As String, ByVal userLastname As String) As UserAndMandantPrintData Implements IListingDatabaseAccess.LoadUserAndMandantData
			Dim result As UserAndMandantPrintData = Nothing
			Dim m_md As New Mandant

			Dim sql As String

			sql = "[Get USData 4 Templates With USName]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("USNachname", ReplaceMissing(userLastname, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("USVorname", ReplaceMissing(userFirstname, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mandantenNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				result = New UserAndMandantPrintData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.USNr = SafeGetInteger(reader, "USNr", 0)
					result.USAnrede = SafeGetString(reader, "USAnrede")
					result.USeMail = SafeGetString(reader, "USeMail")
					result.USNachname = SafeGetString(reader, "USNachname")
					result.USVorname = SafeGetString(reader, "USVorname")
					result.USTelefon = SafeGetString(reader, "USTelefon")
					result.USTelefax = SafeGetString(reader, "USTelefax")
					result.USNatel = SafeGetString(reader, "USNatel")
					result.USTitel_1 = SafeGetString(reader, "USTitel_1")
					result.USTitel_2 = SafeGetString(reader, "USTitel_2")
					result.USAbteilung = SafeGetString(reader, "USAbteilung")
					result.USPostfach = SafeGetString(reader, "USPostfach")
					result.USStrasse = SafeGetString(reader, "USStrasse")
					result.USPLZ = SafeGetString(reader, "USPLZ")
					result.USLand = SafeGetString(reader, "USLand")
					result.USOrt = SafeGetString(reader, "USOrt")
					result.Exchange_USName = SafeGetString(reader, "EMail_UserName")
					result.Exchange_USPW = SafeGetString(reader, "EMail_UserPW")
					result.USMDname = SafeGetString(reader, "MDName")
					result.USMDname2 = SafeGetString(reader, "MDName2")
					result.USMDname3 = SafeGetString(reader, "MDName3")
					result.USMDPostfach = SafeGetString(reader, "MDPostfach")
					result.USMDStrasse = SafeGetString(reader, "MDStrasse")
					result.USMDPlz = SafeGetString(reader, "MDPLZ")
					result.USMDOrt = SafeGetString(reader, "MDOrt")
					result.USMDLand = SafeGetString(reader, "MDLand")
					result.USMDTelefon = SafeGetString(reader, "MDTelefon")
					result.USMDDTelefon = SafeGetString(reader, "MDDTelefon")
					result.USMDTelefax = SafeGetString(reader, "MDTelefax")
					result.USMDeMail = SafeGetString(reader, "MDeMail")
					result.USMDHomepage = SafeGetString(reader, "MDHomepage")
					result.UserPicture = SafeGetByteArray(reader, "USBild")
					result.UserSign = SafeGetByteArray(reader, "USSign")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result


		End Function

		Function LoadUserInformationData(ByVal mandantenNumber As Integer, ByVal userNumber As Integer) As UserAndMandantPrintData Implements IListingDatabaseAccess.LoadUserInformationData
			Dim result As UserAndMandantPrintData = Nothing
			Dim m_md As New Mandant

			Dim sql As String

			sql = "[Get USData 4 Templates With MDNumber And USNumber]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mandantenNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				result = New UserAndMandantPrintData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result.USNr = SafeGetInteger(reader, "USNr", 0)
					result.USAnrede = SafeGetString(reader, "USAnrede")
					result.USeMail = SafeGetString(reader, "USeMail")
					result.USNachname = SafeGetString(reader, "USNachname")
					result.USVorname = SafeGetString(reader, "USVorname")
					result.USTelefon = SafeGetString(reader, "USTelefon")
					result.USTelefax = SafeGetString(reader, "USTelefax")
					result.USNatel = SafeGetString(reader, "USNatel")
					result.USTitel_1 = SafeGetString(reader, "USTitel_1")
					result.USTitel_2 = SafeGetString(reader, "USTitel_2")
					result.USAbteilung = SafeGetString(reader, "USAbteilung")
					result.USPostfach = SafeGetString(reader, "USPostfach")
					result.USStrasse = SafeGetString(reader, "USStrasse")
					result.USPLZ = SafeGetString(reader, "USPLZ")
					result.USLand = SafeGetString(reader, "USLand")
					result.USOrt = SafeGetString(reader, "USOrt")
					result.Exchange_USName = SafeGetString(reader, "EMail_UserName")
					result.Exchange_USPW = SafeGetString(reader, "EMail_UserPW")
					result.USMDname = SafeGetString(reader, "MDName")
					result.USMDname2 = SafeGetString(reader, "MDName2")
					result.USMDname3 = SafeGetString(reader, "MDName3")
					result.USMDPostfach = SafeGetString(reader, "MDPostfach")
					result.USMDStrasse = SafeGetString(reader, "MDStrasse")
					result.USMDPlz = SafeGetString(reader, "MDPLZ")
					result.USMDOrt = SafeGetString(reader, "MDOrt")
					result.USMDLand = SafeGetString(reader, "MDLand")
					result.USMDTelefon = SafeGetString(reader, "MDTelefon")
					result.USMDDTelefon = SafeGetString(reader, "MDDTelefon")
					result.USMDTelefax = SafeGetString(reader, "MDTelefax")
					result.USMDeMail = SafeGetString(reader, "MDeMail")
					result.USMDHomepage = SafeGetString(reader, "MDHomepage")
					result.UserPicture = SafeGetByteArray(reader, "USBild")
					result.UserSign = SafeGetByteArray(reader, "USSign")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result


		End Function



		Function AddAssignedPrintedDocumentInForEmployee(ByVal data As Employee.DataObjects.MasterdataMng.EmployeePrintedDocProperty) As Boolean Implements IListingDatabaseAccess.AddAssignedPrintedDocumentInForEmployee

			Dim success = True

			Dim sql As String

			sql = "Insert Into MA_Printed_Docs ("
			sql &= " MANr"
			sql &= ", DocName"
			sql &= ", UserName"
			sql &= ", CreatedOn"
			sql &= ", ScanDoc "

			sql &= " ) Values ("

			sql &= " @MANr"
			sql &= ", @DocName"
			sql &= ", @UserName"
			sql &= ", GetDate()"
			sql &= ", @ScanDoc"
			sql &= " )"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			' Data of parameters
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(data.manr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocName", ReplaceMissing(data.docname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserName", ReplaceMissing(data.username, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanDoc", ReplaceMissing(data.scandoc, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Return success
		End Function


	End Class


End Namespace



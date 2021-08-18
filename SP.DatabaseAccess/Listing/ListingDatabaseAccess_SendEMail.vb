
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language



Namespace Listing



	Partial Class ListingDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IListingDatabaseAccess


		Function LoadTemplateDataToSendEmail(ByVal eMailType As String) As IEnumerable(Of EMailTemplateData) Implements IListingDatabaseAccess.LoadTemplateDataToSendEmail
			Dim result As List(Of EMailTemplateData) = Nothing

			Dim SQL As String

			SQL = "Select ID, Bezeichnung, JobNr, DocName From Dokprint"
			SQL &= " Where JobNr Like @eMailType "
			SQL &= " Order By Bezeichnung "

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("eMailType", eMailType))

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EMailTemplateData)

					While reader.Read()
						Dim overviewData As New EMailTemplateData

						overviewData.ID = SafeGetInteger(reader, "ID", 0)
						overviewData.DocumentLabel = SafeGetString(reader, "Bezeichnung")
						overviewData.DocumentName = SafeGetString(reader, "DocName")


						result.Add(overviewData)

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

		Function LoadCustomerDataToSendBulkEmail(ByVal userNumber As Integer, ByVal modulName As String) As IEnumerable(Of CustomerBulkEMailData) Implements IListingDatabaseAccess.LoadCustomerDataToSendBulkEmail
			Dim result As List(Of CustomerBulkEMailData) = Nothing

			Dim SQL As String

			SQL = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH; "

			If modulName = "" Then
				SQL &= "SELECT KDNr, Firma1, CONVERT(NVARCHAR(255), '') Nachname, CONVERT(NVARCHAR(255), '') Vorname, "
				SQL &= "CONVERT(NVARCHAR(50), '') Anrede, CONVERT(NVARCHAR(255), '') AnredeForm, "
				SQL &= "KDStrasse Strasse, KDPLZ PLZ, KDPostfach Postfach, KDOrt Ort , KDLand Land, "
				SQL &= "KDTelefon Telefon, "
				SQL &= "CONVERT(NVARCHAR(70), "
				SQL &= "( "
				SQL &= "CASE WHEN ISNULL(KD_Mail_Mailing, 0) = 0 THEN KDeMail "
				SQL &= "ELSE '' "
				SQL &= "End)) EMail, "
				SQL &= "ISNULL(KD_Mail_Mailing, 0) Mail_Mailing, "
				SQL &= "CONVERT(NVARCHAR(70), "
				SQL &= "( "
				SQL &= "CASE WHEN ISNULL(KD_Telefax_Mailing, 0) = 0 THEN KDTelefax "
				SQL &= "ELSE '' "
				SQL &= "End)) Telefax, "
				SQL &= "ISNULL(KD_Telefax_Mailing, 0) Telefax_Mailing, "
				SQL &= "CONVERT(NVARCHAR(255), '') Abteilung, "
				SQL &= "0 ZHDRecNr, "
				SQL &= "CONVERT(NVARCHAR(10), 'KD') ModulName "

				SQL &= "INTO #KD_Mailing "
				SQL &= String.Format("FROM dbo._Kundenliste_{0};", userNumber)


				SQL &= "INSERT INTO #KD_Mailing "
				SQL &= "( "
				SQL &= "KDNr, "
				SQL &= "Firma1, "
				SQL &= "Nachname, "
				SQL &= "Vorname, "
				SQL &= "Anrede, "
				SQL &= "AnredeForm, "
				SQL &= "Strasse, "
				SQL &= "PLZ, "
				SQL &= "Postfach, "
				SQL &= "Ort, "
				SQL &= "Land, "
				SQL &= "Telefon, "
				SQL &= "EMail, "
				SQL &= "Mail_Mailing, "
				SQL &= "Telefax, "
				SQL &= "Telefax_Mailing, "
				SQL &= "Abteilung, "
				SQL &= "ZHDRecNr, "
				SQL &= "ModulName "
				SQL &= ") "
				SQL &= "SELECT KDNr, Firma1, Nachname, Vorname, Anrede, AnredeForm, "
				SQL &= "ZHDStrasse, ZHDPLZ, ZHDPostfach, ZHDOrt, KDLand, "
				SQL &= "ZHDTelefon, "
				SQL &= "( "
				SQL &= "CASE WHEN ISNULL(ZHD_Mail_Mailing, 0) = 0 THEN ZHDeMail "
				SQL &= "ELSE '' "
				SQL &= "End) , "
				SQL &= "ISNULL(ZHD_Mail_Mailing, 0), "
				SQL &= "( "
				SQL &= "CASE WHEN ISNULL(ZHD_Telefax_Mailing, 0) = 0 THEN ZHDTelefax "
				SQL &= "ELSE '' "
				SQL &= "End), "
				SQL &= "ISNULL(ZHD_Telefax_Mailing, 0), "
				SQL &= "ZHDAbt, "
				SQL &= "ZHDRecNr, "
				SQL &= "'ZHD' "
				SQL &= String.Format("FROM dbo._Kundenliste_{0}; ", userNumber)

			ElseIf modulName = "KD" Then
				SQL &= LoadCustomerQueryStringToSendBulkEmail(userNumber, modulName)

			ElseIf modulName = "ZHD" Then
				SQL &= LoadCResponsibleQueryStringToSendBulkEmail(userNumber, modulName)

			End If

			SQL &= " Select * FROM dbo.#KD_Mailing "
			SQL &= "GROUP BY "
			SQL &= "KDNr, "
			SQL &= "Firma1, "
			SQL &= "Nachname, "
			SQL &= "Vorname, "
			SQL &= "Anrede, "
			SQL &= "AnredeForm, "
			SQL &= "Strasse, "
			SQL &= "PLZ, "
			SQL &= "Postfach, "
			SQL &= "Ort, "
			SQL &= "Land, "
			SQL &= "Telefon, "
			SQL &= "EMail, "
			SQL &= "Mail_Mailing, "
			SQL &= "Telefax, "
			SQL &= "Telefax_Mailing, "
			SQL &= "Abteilung, "
			SQL &= "ZHDRecNr, "
			SQL &= "ModulName "
			SQL &= "Order By Firma1, Nachname, Vorname"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(SQL, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerBulkEMailData)

					While reader.Read()
						Dim overviewData As New CustomerBulkEMailData

						overviewData.KDNr = SafeGetInteger(reader, "KDNr", 0)
						overviewData.ZHDRecNr = SafeGetInteger(reader, "ZHDRecNr", 0)
						overviewData.Firma1 = SafeGetString(reader, "Firma1")
						overviewData.NoMailing = SafeGetBoolean(reader, "Mail_Mailing", False)
						overviewData.ReceiverEMail = SafeGetString(reader, "EMail")

						overviewData.Anrede = SafeGetString(reader, "Anrede")
						overviewData.AnredeForm = SafeGetString(reader, "AnredeForm")
						overviewData.Nachname = SafeGetString(reader, "Nachname")
						overviewData.Vorname = SafeGetString(reader, "Vorname")

						overviewData.Strasse = SafeGetString(reader, "Strasse")
						overviewData.PLZ = SafeGetString(reader, "PLZ")
						overviewData.Postfach = SafeGetString(reader, "Postfach")
						overviewData.Ort = SafeGetString(reader, "Ort")
						overviewData.Land = SafeGetString(reader, "Land")
						overviewData.Telefax = SafeGetString(reader, "Telefax")
						overviewData.Telefax_Mailing = SafeGetBoolean(reader, "Telefax_Mailing", False)
						overviewData.Abteilung = SafeGetString(reader, "Abteilung")
						overviewData.ModulName = SafeGetString(reader, "ModulName")


						If Not String.IsNullOrWhiteSpace(overviewData.ReceiverEMail) Then
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


		Private Function LoadCustomerQueryStringToSendBulkEmail(ByVal userNumber As Integer, ByVal modulName As String) As String
			Dim SQL As String


			SQL = "SELECT KDNr, Firma1, CONVERT(NVARCHAR(255), '') Nachname, CONVERT(NVARCHAR(255), '') Vorname, "
			SQL &= "CONVERT(NVARCHAR(50), '') Anrede, CONVERT(NVARCHAR(255), '') AnredeForm, "
			SQL &= "KDStrasse Strasse, KDPLZ PLZ, KDPostfach Postfach, KDOrt Ort , KDLand Land, "
			SQL &= "KDTelefon Telefon, "
			SQL &= "CONVERT(NVARCHAR(70), "
			SQL &= "( "
			SQL &= "CASE WHEN ISNULL(KD_Mail_Mailing, 0) = 0 THEN KDeMail "
			SQL &= "ELSE '' "
			SQL &= "End)) EMail, "
			SQL &= "ISNULL(KD_Mail_Mailing, 0) Mail_Mailing, "
			SQL &= "CONVERT(NVARCHAR(70), "
			SQL &= "( "
			SQL &= "CASE WHEN ISNULL(KD_Telefax_Mailing, 0) = 0 THEN KDTelefax "
			SQL &= "ELSE '' "
			SQL &= "End)) Telefax, "
			SQL &= "ISNULL(KD_Telefax_Mailing, 0) Telefax_Mailing, "
			SQL &= "CONVERT(NVARCHAR(255), '') Abteilung, "
			SQL &= "0 ZHDRecNr, "
			SQL &= "CONVERT(NVARCHAR(10), 'KD') ModulName "

			SQL &= "INTO #KD_Mailing "
			SQL &= String.Format("FROM dbo._Kundenliste_{0};", userNumber)

			Return SQL

		End Function

		Private Function LoadCResponsibleQueryStringToSendBulkEmail(ByVal userNumber As Integer, ByVal modulName As String) As String
			Dim SQL As String

			SQL = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH; "

			SQL &= "SELECT KDNr, Firma1, Nachname, Vorname, Anrede, AnredeForm, "
			SQL &= "ZHDStrasse Strasse, ZHDPLZ PLZ, ZHDPostfach Postfach, ZHDOrt Ort, KDLand Land, "
			SQL &= "ZHDTelefon Telefon, "
			SQL &= "( "
			SQL &= "CASE WHEN ISNULL(ZHD_Mail_Mailing, 0) = 0 THEN ZHDeMail "
			SQL &= "ELSE '' "
			SQL &= "End) EMail, "
			SQL &= "ISNULL(ZHD_Mail_Mailing, 0) Mail_Mailing, "
			SQL &= "( "
			SQL &= "CASE WHEN ISNULL(ZHD_Telefax_Mailing, 0) = 0 THEN ZHDTelefax "
			SQL &= "ELSE '' "
			SQL &= "End) Telefax, "
			SQL &= "ISNULL(ZHD_Telefax_Mailing, 0) Telefax_Mailing, "
			SQL &= "ZHDAbt Abteilung, "
			SQL &= "ZHDRecNr, "
			SQL &= "CONVERT(NVARCHAR(10), 'ZHD') ModulName "

			SQL &= "INTO #KD_Mailing "
			SQL &= String.Format("FROM dbo._Kundenliste_{0};", userNumber)


			Return SQL
		End Function

	End Class


End Namespace

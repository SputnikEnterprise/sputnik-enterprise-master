Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Customer.DataObjects

Imports SPProgUtility.Mandanten

Namespace Customer

  ''' <summary>
  ''' Customer database access class.
  ''' </summary>
  Public Class CustomerDatabaseAccess
    Inherits DatabaseAccessBase
    Implements ICustomerDatabaseAccess

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
      MyBase.new(connectionString, translationLanguage)
    End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads customer master data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="usFiliale">The </param>
		''' <returns>Customer master data.</returns>
		Function LoadCustomerMasterData(ByVal customerNumber As Integer, ByVal usFiliale As String) As CustomerMasterData Implements ICustomerDatabaseAccess.LoadCustomerMasterData

			Dim customerMasterData As CustomerMasterData = Nothing

			Dim sql As String

			'sql = "SELECT KD.*, convert(tinyint, ISNull((Select Top 1 k.DV_DecisionID From KD_KreditInfo K Where K.DV_ArchivID <> '' And K.KDNr = KD.KDNr Order By CreatedOn Desc), 0)) As DecisionID, "
			'sql &= "ISNull((Select Top 1 k.Beschreibung From KD_KreditInfo K Where K.DV_ArchivID <> '' And K.KDNr = KD.KDNr Order By CreatedOn Desc), '')) As DecisionText,  "
			'sql &= "ISNull((Select Top 1 k.Beschreibung From KD_KreditInfo K Where K.DV_ArchivID <> '' And K.KDNr = KD.KDNr Order By CreatedOn Desc), '')) As DecisionText  "
			'sql &= "FROM Kunden KD "
			'sql &= "Where KD.KDNr = @customerNumber "
			'sql &= "And KD. KDFiliale Like '%" & usFiliale & "%' "

			sql = "[Load Selected Customer Data With Solvency]"

			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
			Dim customerFilialParameter As New SqlClient.SqlParameter("Filiale", ReplaceMissing("%%", String.Empty))
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(customerNumberParameter)
			listOfParams.Add(customerFilialParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If Not reader Is Nothing Then

					If reader.Read Then
						customerMasterData = New CustomerMasterData
						customerMasterData.CustomerMandantNumber = SafeGetInteger(reader, "MDNr", 0)
						customerMasterData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						customerMasterData.WOSGuid = SafeGetString(reader, "Transfered_Guid", String.Empty)

						' TODO: Anhand Int-Wert das Impage für btnSolvency auswählen...
						customerMasterData.SolvencyDecisionID = SafeGetInteger(reader, "DecisionID", 0)
						customerMasterData.SolvencyInfo = SafeGetString(reader, "SolvencyData")

						customerMasterData.Company1 = SafeGetString(reader, "Firma1")
						customerMasterData.Company2 = SafeGetString(reader, "Firma2")
						customerMasterData.Company3 = SafeGetString(reader, "Firma3")
						customerMasterData.Street = SafeGetString(reader, "Strasse")
						customerMasterData.CountryCode = SafeGetString(reader, "Land")
						customerMasterData.Postcode = SafeGetString(reader, "PLZ")
						customerMasterData.Latitude = SafeGetDouble(reader, "Latitude", 0)
						customerMasterData.Longitude = SafeGetDouble(reader, "Longitude", 0)

						customerMasterData.PostOfficeBox = SafeGetString(reader, "Postfach")
						customerMasterData.Location = SafeGetString(reader, "Ort")
						customerMasterData.Telephone = SafeGetString(reader, "Telefon")
						customerMasterData.Telefax = SafeGetString(reader, "Telefax")
						customerMasterData.Telefax_Mailing = SafeGetBoolean(reader, "KD_Telefax_Mailing", False)
						customerMasterData.EMail = SafeGetString(reader, "EMail")
						customerMasterData.Email_Mailing = SafeGetBoolean(reader, "KD_Mail_Mailing", False)
						customerMasterData.Hompage = SafeGetString(reader, "Homepage")
						customerMasterData.facebook = SafeGetString(reader, "facebook")
						customerMasterData.xing = SafeGetString(reader, "xing")
						customerMasterData.KST = SafeGetString(reader, "KST")
						customerMasterData.KDBusinessBranch = SafeGetString(reader, "KDFiliale")
						customerMasterData.FirstProperty = SafeGetDecimal(reader, "FProperty", Nothing)
						customerMasterData.Language = SafeGetString(reader, "Sprache")
						customerMasterData.HowContact = SafeGetString(reader, "HowKontakt")
						customerMasterData.CustomerState1 = SafeGetString(reader, "KDState1")
						customerMasterData.CustomerState2 = SafeGetString(reader, "KDState2")
						customerMasterData.NoUse = SafeGetBoolean(reader, "NoES", False)
						customerMasterData.NoUseComment = SafeGetString(reader, "NOESBez")

						customerMasterData.Comment = SafeGetString(reader, "Notice_Common")
						customerMasterData.Notice_Employment = SafeGetString(reader, "Notice_Employment")
						customerMasterData.Notice_Report = SafeGetString(reader, "Notice_Report")
						customerMasterData.Notice_Invoice = SafeGetString(reader, "Notice_Invoice")
						customerMasterData.Notice_Payment = SafeGetString(reader, "Notice_Payment")

						customerMasterData.SalaryPerMonth = SafeGetDecimal(reader, "GehaltPerMonth", Nothing)
						customerMasterData.SalaryPerHour = SafeGetDecimal(reader, "GehaltPerStd", Nothing)
						customerMasterData.Reserve1 = SafeGetString(reader, "KDRes1")
						customerMasterData.Reserve2 = SafeGetString(reader, "KDRes2")
						customerMasterData.Reserve3 = SafeGetString(reader, "KDRes3")
						customerMasterData.Reserve4 = SafeGetString(reader, "KDRes4")
						customerMasterData.CreditLimit1 = SafeGetDecimal(reader, "KreditLimite", 0)
						customerMasterData.CreditLimit2 = SafeGetDecimal(reader, "Kreditlimite_2", 0)
						customerMasterData.CreditLimitsFromDate = SafeGetDateTime(reader, "KreditlimiteAb", Nothing)
						customerMasterData.CreditLimitsToDate = SafeGetDateTime(reader, "KreditlimiteBis", Nothing)
						customerMasterData.OpenInvoiceAmount = SafeGetDecimal(reader, "OpenInvoiceAmount", 0)
						customerMasterData.ReferenceNumber = SafeGetString(reader, "KL_RefNr")
						customerMasterData.KD_UmsMin = SafeGetDecimal(reader, "KD_UmsMin", 0)
						customerMasterData.mwstpflicht = SafeGetBoolean(reader, "mwst", 1)
						customerMasterData.NumberOfCopies = SafeGetShort(reader, "AnzKopien", Nothing)
						customerMasterData.ValueAddedTaxNumber = SafeGetString(reader, "MwStNr", Nothing)
						customerMasterData.CreditWarning = SafeGetBoolean(reader, "KreditWarnung", False)
						customerMasterData.OPShipment = SafeGetString(reader, "OPVersand", "")
						customerMasterData.NotPrintReports = SafeGetBoolean(reader, "PrintNoRP", False)
						customerMasterData.TermsAndConditions_WOS = SafeGetString(reader, "AGB_WOS")
						customerMasterData.sendToWOS = SafeGetBoolean(reader, "Send2WOS", False)
						customerMasterData.OneInvoicePerMail = SafeGetBoolean(reader, "OneInvoicePerMail", False)
						customerMasterData.DoNotShowContractInWOS = SafeGetBoolean(reader, "DoNotShowContractInWOS", False)
						customerMasterData.CurrencyCode = SafeGetString(reader, "Currency")
						customerMasterData.BillTypeCode = SafeGetString(reader, "Faktura")
						customerMasterData.NumberOfEmployees = SafeGetString(reader, "MAAnzahl")
						customerMasterData.CanteenAvailable = SafeGetBoolean(reader, "Kantine", False)
						customerMasterData.TransportationOptions = SafeGetBoolean(reader, "Transport", False)
						customerMasterData.InvoiceOption = SafeGetString(reader, "FakturaOption")
						customerMasterData.ShowHoursInNormal = SafeGetBoolean(reader, "ShowHoursInNormal", False)

						customerMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						customerMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						customerMasterData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						customerMasterData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						customerMasterData.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")

						customerMasterData.CreatedUserNumber = SafeGetInteger(reader, "CreatedUserNumber", 0)
						customerMasterData.ChangedUserNumber = SafeGetInteger(reader, "ChangedUserNumber", 0)

					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				customerMasterData = Nothing
			Finally
				CloseReader(reader)
			End Try

			Return customerMasterData

		End Function

		''' <summary>
		'''  Loads customer responsible person master data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The record number.</param>
		''' <returns>Responsible person master data.</returns>
		Function LoadResponsiblePersonMasterData(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As ResponsiblePersonMasterData Implements ICustomerDatabaseAccess.LoadResponsiblePersonMasterData
      Dim responsiblePersonMasterData As ResponsiblePersonMasterData = Nothing

      Dim sql As String = String.Empty

			sql = sql & "SELECT Z.*, "
			sql = sql & String.Format("IsNull((SELECT TOP 1 Anrede_{0} FROM Anrede ANR WHERE ANR.Anrede = Z.Anrede), '') AS TranslatedAnrede, ", MapLanguageToShortLanguageCode(SelectedTranslationLanguage))
			sql = sql & String.Format("IsNull((SELECT TOP 1 BriefForm_{0} FROM Anrede ANR WHERE ANR.Anrede = Z.Anrede), '') AS TranslatedBriefFrom ", MapLanguageToShortLanguageCode(SelectedTranslationLanguage))
			sql = sql & "FROM KD_Zustaendig Z Where Z.KDNr = @customerNumber AND Z.RecNr = @recordNumber"

			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim recordNumberParameter As New SqlClient.SqlParameter("recordNumber", recordNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(recordNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            responsiblePersonMasterData = New ResponsiblePersonMasterData
            responsiblePersonMasterData.ID = SafeGetInteger(reader, "ID", 0)
            responsiblePersonMasterData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            responsiblePersonMasterData.Salutation = SafeGetString(reader, "Anrede")
            responsiblePersonMasterData.Lastname = SafeGetString(reader, "Nachname")
            responsiblePersonMasterData.Firstname = SafeGetString(reader, "Vorname")
            responsiblePersonMasterData.Department = SafeGetString(reader, "Abteilung")
            responsiblePersonMasterData.Position = SafeGetString(reader, "Position")
            responsiblePersonMasterData.Telephone = SafeGetString(reader, "Telefon")
            responsiblePersonMasterData.Telefax = SafeGetString(reader, "Telefax")
            responsiblePersonMasterData.MobilePhone = SafeGetString(reader, "Natel")
            responsiblePersonMasterData.Email = SafeGetString(reader, "eMail")
            responsiblePersonMasterData.Facebook = SafeGetString(reader, "facebook")
						responsiblePersonMasterData.LinkedIn = SafeGetString(reader, "linkedIn")
						responsiblePersonMasterData.Xing = SafeGetString(reader, "xing")

						responsiblePersonMasterData.Birthdate = SafeGetDateTime(reader, "Geb_Dat", Nothing)
            responsiblePersonMasterData.Interests = SafeGetString(reader, "Interessen")
            responsiblePersonMasterData.Company1 = SafeGetString(reader, "Firma1")
            responsiblePersonMasterData.PostOfficeBox = SafeGetString(reader, "Postfach")
            responsiblePersonMasterData.Street = SafeGetString(reader, "Strasse")
            responsiblePersonMasterData.Postcode = SafeGetString(reader, "PLZ")
            responsiblePersonMasterData.CountryCode = SafeGetString(reader, "Land")
            responsiblePersonMasterData.Location = SafeGetString(reader, "Ort")
            responsiblePersonMasterData.Advisor = SafeGetString(reader, "Berater")
            responsiblePersonMasterData.FirstContactDate = SafeGetDateTime(reader, "ErstKontakt", Nothing)
            responsiblePersonMasterData.LastContactDate = SafeGetDateTime(reader, "LetztKontakt", Nothing)
            responsiblePersonMasterData.State1 = SafeGetString(reader, "KDZState1")
            responsiblePersonMasterData.KDZComments = SafeGetString(reader, "KDZBemerkung")
            responsiblePersonMasterData.KDZHowKontakt = SafeGetString(reader, "KDZHowKontakt")
            responsiblePersonMasterData.State2 = SafeGetString(reader, "KDZState2")
            responsiblePersonMasterData.SalutationForm = SafeGetString(reader, "AnredeForm")
            responsiblePersonMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            responsiblePersonMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            responsiblePersonMasterData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            responsiblePersonMasterData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
            responsiblePersonMasterData.RecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
            responsiblePersonMasterData.Comments = SafeGetString(reader, "Bemerkung")
            responsiblePersonMasterData.Telefax_Mailing = SafeGetBoolean(reader, "ZHD_Telefax_Mailing", False)
            responsiblePersonMasterData.SMS_Mailing = SafeGetBoolean(reader, "ZHD_SMS_Mailing", False)
            responsiblePersonMasterData.Email_Mailing = SafeGetBoolean(reader, "ZHD_Mail_Mailing", False)
            responsiblePersonMasterData.TransferedGuid = SafeGetString(reader, "Transfered_Guid")
            responsiblePersonMasterData.TermsAndConditions_WOS = SafeGetString(reader, "AGB_WOS")

            responsiblePersonMasterData.TranslatedSalutation = SafeGetString(reader, "TranslatedAnrede")
            responsiblePersonMasterData.TranslatedSalutationForm = SafeGetString(reader, "TranslatedBriefFrom")

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        responsiblePersonMasterData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return responsiblePersonMasterData
    End Function

    ''' <summary>
    ''' Loads responsible person overview data for person management.
    ''' </summary>
    ''' <param name="customerNumber">The customer number</param>
    ''' <returns>List of responsible person overview data.</returns>
    Function LoadResponsiblePersonsOverviewDataForPersonManagement(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonOverviewDataForPersonManagement) Implements ICustomerDatabaseAccess.LoadResponsiblePersonsOverviewDataForPersonManagement

      Dim result As List(Of ResponsiblePersonOverviewDataForPersonManagement) = Nothing

      Dim sql As String

			sql = "Select ID, KDNr, RecNr, Nachname + ', ' + IsNull(Vorname, '') AS Name, Telefon, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom, KDZState1, KDZState2 "
			sql &= " FROM KD_Zustaendig Where KDNr = @customerNumber "
			sql &= " ORDER BY Nachname Asc, Vorname Asc"


      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If Not reader Is Nothing Then
          result = New List(Of ResponsiblePersonOverviewDataForPersonManagement)

          While reader.Read
            Dim overviewData As New ResponsiblePersonOverviewDataForPersonManagement
            overviewData.ID = SafeGetInteger(reader, "ID", 0)
            overviewData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            overviewData.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
            overviewData.Name = SafeGetString(reader, "Name")
            overviewData.Telephone = SafeGetString(reader, "Telefon")
            overviewData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            overviewData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            overviewData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            overviewData.ChangedFrom = SafeGetString(reader, "ChangedFrom")

						overviewData.ZState1 = SafeGetString(reader, "KDZState1")
						overviewData.ZState2 = SafeGetString(reader, "KDZState2")

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


		''' <summary>
		''' Lodas customer data.
		''' </summary>
		''' <returns>List of customer data.</returns>
		Function LoadCustomerData(Optional ByVal usFiliale As String = "") As IEnumerable(Of CustomerMasterData) Implements ICustomerDatabaseAccess.LoadCustomerData

			Dim result As List(Of CustomerMasterData) = Nothing

			Dim sql As String

			sql = "SELECT KDNr, Firma1, Strasse, PLZ, Ort FROM Kunden Where "
			sql &= "KDFiliale Like @Filiale "
			sql &= "ORDER BY Firma1 ASC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", "%" & ReplaceMissing(usFiliale, String.Empty) & "%"))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerMasterData)

					While reader.Read

						Dim customerData = New CustomerMasterData()
						customerData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						customerData.Company1 = SafeGetString(reader, "Firma1")
						customerData.Street = SafeGetString(reader, "Strasse")
						customerData.Postcode = SafeGetString(reader, "PLZ")
						customerData.Location = SafeGetString(reader, "Ort")

						result.Add(customerData)

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
		''' Loads responsible person data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Responsible person data.</returns>
		Public Function LoadResponsiblePersonData(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData) Implements ICustomerDatabaseAccess.LoadResponsiblePersonData

			Dim result As List(Of ResponsiblePersonData) = Nothing

			Dim sql As String
			Dim lang = MapLanguageToShortLanguageCode(SelectedTranslationLanguage)

			sql = "Select Top 500 ZHD.*, "
			sql = sql & String.Format("IsNull((SELECT TOP 1 Bez_{0} FROM Tab_ZHDState1 t WHERE t.Bezeichnung = ZHD.KDZState1), '') AS TranslatedState1, ", lang)
			sql = sql & String.Format("IsNull((SELECT TOP 1 Bez_{0} FROM Tab_ZHDState2 t WHERE t.Bezeichnung = ZHD.KDZState2), '') AS TranslatedState2, ", lang)
			sql = sql & String.Format("IsNull((SELECT TOP 1 Bez_{0} FROM Tab_ZHDKontakt t WHERE t.Bezeichnung = ZHD.KDZHowKontakt), '') AS TranslatedHowKontakt, ", lang)
			sql = sql & String.Format("IsNull((SELECT TOP 1 Anrede_{0} FROM Anrede ANR WHERE ANR.Anrede = ZHD.Anrede), '') AS TranslatedAnrede ", lang)

			sql = sql & "FROM KD_Zustaendig ZHD Where KDNr = @customerNumber "
			sql = sql & "Order By Nachname Asc, Vorname Asc"

			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(customerNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If Not reader Is Nothing Then
					result = New List(Of ResponsiblePersonData)
					' KDZState1, KDZState2, KDZHowKontakt
					While reader.Read
						Dim responsibleData As New ResponsiblePersonData
						responsibleData.ID = SafeGetInteger(reader, "ID", 0)
						responsibleData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
						responsibleData.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
						responsibleData.Position = SafeGetString(reader, "Position")
						responsibleData.Department = SafeGetString(reader, "Abteilung")
						responsibleData.Salutation = SafeGetString(reader, "Anrede")
						responsibleData.Firstname = SafeGetString(reader, "Vorname")
						responsibleData.Lastname = SafeGetString(reader, "Nachname")
						responsibleData.Telephone = SafeGetString(reader, "Telefon")
						responsibleData.Telefax = SafeGetString(reader, "Telefax")
						responsibleData.MobilePhone = SafeGetString(reader, "Natel")
						responsibleData.Email = SafeGetString(reader, "eMail")
						responsibleData.Xing = SafeGetString(reader, "Xing")
						responsibleData.Interests = SafeGetString(reader, "Interessen")
						responsibleData.Comments = SafeGetString(reader, "Bemerkung")

						responsibleData.TranslatedZState1 = SafeGetString(reader, "TranslatedState1")
						responsibleData.TranslatedZState2 = SafeGetString(reader, "TranslatedState2")
						responsibleData.TranslatedZHowKontakt = SafeGetString(reader, "TranslatedHowKontakt")

						responsibleData.ZState1 = SafeGetString(reader, "KDZState1")
						responsibleData.ZState2 = SafeGetString(reader, "KDZState2")
						responsibleData.BirthDate = SafeGetDateTime(reader, "Geb_Dat", Nothing)

						responsibleData.TranslatedSalutation = SafeGetString(reader, "TranslatedAnrede")

						responsibleData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						responsibleData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						responsibleData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						responsibleData.ChangedFrom = SafeGetString(reader, "ChangedFrom")

						result.Add(responsibleData)
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
		''' Loads responsible person data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Responsible person data.</returns>
		Public Function LoadResponsiblePersonDataActiv(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData) Implements ICustomerDatabaseAccess.LoadResponsiblePersonDataActiv

      Dim result As List(Of ResponsiblePersonData) = Nothing

      Dim sql As String

      sql = "Select Top 500 ZHD.*, "
      sql = sql & String.Format("IsNull((SELECT TOP 1 Anrede_{0} FROM Anrede ANR WHERE ANR.Anrede = ZHD.Anrede), '') AS TranslatedAnrede ", MapLanguageToShortLanguageCode(SelectedTranslationLanguage))
      sql = sql & "FROM KD_Zustaendig ZHD Where KDNr = @customerNumber "
			sql &= "And (ZHD.KDZState1 Not In ('nicht mehr aktiv', 'inaktiv', 'mehr aktiv') Or ZHD.KDZState1 Is Null) "
      sql = sql & "Order By Nachname Asc, Vorname Asc"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try
        If Not reader Is Nothing Then
          result = New List(Of ResponsiblePersonData)

          While reader.Read
            Dim responsibleData As New ResponsiblePersonData
            responsibleData.ID = SafeGetInteger(reader, "ID", 0)
            responsibleData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            responsibleData.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
            responsibleData.Position = SafeGetString(reader, "Position")
            responsibleData.Department = SafeGetString(reader, "Abteilung")
            responsibleData.Salutation = SafeGetString(reader, "Anrede")
            responsibleData.Firstname = SafeGetString(reader, "Vorname")
            responsibleData.Lastname = SafeGetString(reader, "Nachname")
            responsibleData.Telephone = SafeGetString(reader, "Telefon")
            responsibleData.Telefax = SafeGetString(reader, "Telefax")
            responsibleData.MobilePhone = SafeGetString(reader, "Natel")
            responsibleData.Email = SafeGetString(reader, "eMail")
            responsibleData.TranslatedSalutation = SafeGetString(reader, "TranslatedAnrede")

						responsibleData.ZState1 = SafeGetString(reader, "KDZState1")
						responsibleData.ZState2 = SafeGetString(reader, "KDZState2")

            result.Add(responsibleData)
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
    ''' Loads assigned customer business branches data (KD_Filialen)
    ''' </summary>
    ''' <returns>List of business branches data.</returns>
    Public Function LoadAssignedBusinessBranchsDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedBusinessBranchData) Implements ICustomerDatabaseAccess.LoadAssignedBusinessBranchsDataOfCustomer

      Dim result As List(Of CustomerAssignedBusinessBranchData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, Bezeichnung, MDNr FROM KD_Filiale WHERE KDNr = @customerNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedBusinessBranchData)

          While reader.Read()
            Dim customerBusinessBranchData As New CustomerAssignedBusinessBranchData
            customerBusinessBranchData.ID = SafeGetInteger(reader, "ID", 0)
            customerBusinessBranchData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            customerBusinessBranchData.Name = SafeGetString(reader, "Bezeichnung")
            customerBusinessBranchData.MDNr = SafeGetInteger(reader, "MDNr", 0)
            result.Add(customerBusinessBranchData)

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
    ''' Loads assigned customer profession data (KD_Berufe).
    ''' </summary>
    ''' <returns>List of profession data.</returns>
    Public Function LoadAssignedProfessionDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedProfessionData) Implements ICustomerDatabaseAccess.LoadAssignedProfessionDataOfCustomer

      Dim result As List(Of CustomerAssignedProfessionData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, BerufsCode, Bezeichnung, BerufCode FROM KD_Berufe WHERE KDNr = @customerNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedProfessionData)

          While reader.Read()
            Dim professionData As New CustomerAssignedProfessionData
            professionData.ID = SafeGetInteger(reader, "ID", 0)
            professionData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            professionData.ProfessionCodeString = SafeGetString(reader, "BerufsCode")
            professionData.Description = SafeGetString(reader, "Bezeichnung")
            professionData.ProfessionCodeInteger = SafeGetInteger(reader, "BerufCode", Nothing)
            result.Add(professionData)

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
    ''' Loads assigned responsible person profession data (KD_ZBerufe).
    ''' </summary>
    ''' <returns>List of profession data.</returns>
    Public Function LoadAssignedProfessionDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedProfessionData) Implements ICustomerDatabaseAccess.LoadAssignedProfessionDataOfResponsiblePerson

      Dim result As List(Of ResponsiblePersonAssignedProfessionData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, KDZNr, BerufsCode, Bezeichnung, BerufCode FROM KD_ZBerufe WHERE KDNr = @customerNumber AND KDZNr = @responsiblePersonRecNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecNumber", responsiblePersonRecordNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonAssignedProfessionData)

          While reader.Read()
            Dim professionData As New ResponsiblePersonAssignedProfessionData
            professionData.ID = SafeGetInteger(reader, "ID", 0)
            professionData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            professionData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", Nothing)
            professionData.ProfessionCodeString = SafeGetString(reader, "BerufsCode")
            professionData.Description = SafeGetString(reader, "Bezeichnung")
            professionData.ProfessionCodeInteger = SafeGetInteger(reader, "BerufCode", Nothing)
            result.Add(professionData)

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
    ''' Loads assigned customer sector data (KD_Branche).
    ''' </summary>
    ''' <returns>List of sector data.</returns>
    Public Function LoadAssignedSectorDataOfCustmer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedSectorData) Implements ICustomerDatabaseAccess.LoadAssignedSectorDataOfCustomer

      Dim result As List(Of CustomerAssignedSectorData) = Nothing

      Dim sql As String

			sql = "SELECT ID, KDNr, Bezeichnung, Result, BranchenCode FROM KD_Branche WHERE KDNr = @customerNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedSectorData)

          While reader.Read()
            Dim industryData As New CustomerAssignedSectorData
            industryData.ID = SafeGetInteger(reader, "ID", 0)
            industryData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            industryData.Description = SafeGetString(reader, "Bezeichnung")
            industryData.Result = SafeGetString(reader, "Result")
            industryData.SectorCode = SafeGetInteger(reader, "BranchenCode", Nothing)
            result.Add(industryData)

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
    ''' Loads assigned responsible person sector data (KD_ZBranche).
    ''' </summary>
    ''' <returns>List of sector data.</returns>
    Public Function LoadAssignedSectorDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedSectorData) Implements ICustomerDatabaseAccess.LoadAssignedSectorDataOfResponsiblePerson

      Dim result As List(Of ResponsiblePersonAssignedSectorData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, KDZNr, Bezeichnung, Result, BranchenCode FROM KD_ZBranche WHERE KDNr = @customerNumber AND KDZNr=@responsiblePersonRecordNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecordNumber", responsiblePersonRecordNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonAssignedSectorData)

          While reader.Read()
            Dim industryData As New ResponsiblePersonAssignedSectorData
            industryData.ID = SafeGetInteger(reader, "ID", 0)
            industryData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            industryData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", Nothing)
            industryData.Description = SafeGetString(reader, "Bezeichnung")
            industryData.Result = SafeGetString(reader, "Result")
            industryData.SectorCode = SafeGetInteger(reader, "BranchenCode", Nothing)
            result.Add(industryData)

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
    ''' Loads assigned customer employment type data (KD_Anstellung).
    ''' </summary>
    ''' <returns>List of employment type data.</returns>
    Function LoadAssignedEmploymentTypeDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedEmploymentTypeData) Implements ICustomerDatabaseAccess.LoadAssignedEmploymentTypeDataOfCustomer

      Dim result As List(Of CustomerAssignedEmploymentTypeData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, KDZuNr, Bezeichnung FROM KD_Anstellung WHERE KDNr = @customerNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedEmploymentTypeData)

          While reader.Read()
            Dim employmentTypeData As New CustomerAssignedEmploymentTypeData
            employmentTypeData.ID = SafeGetInteger(reader, "ID", 0)
            employmentTypeData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            employmentTypeData.CustomerNumber2 = SafeGetInteger(reader, "KDZuNr", Nothing)
            employmentTypeData.Description = SafeGetString(reader, "Bezeichnung")

            result.Add(employmentTypeData)

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
    ''' Loads assigned customer keyword data (KD_Stichwort).
    ''' </summary>
    ''' <returns>List of keyword data.</returns>
    Public Function LoadAssignedKeywordDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedKeywordData) Implements ICustomerDatabaseAccess.LoadAssignedKeywordDataOfCustomer
      Dim result As List(Of CustomerAssignedKeywordData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, Bezeichnung, Result FROM KD_Stichwort WHERE KDNr = @customerNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedKeywordData)

          While reader.Read()
            Dim keywordData As New CustomerAssignedKeywordData
            keywordData.ID = SafeGetInteger(reader, "ID", 0)
            keywordData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            keywordData.Description = SafeGetString(reader, "Bezeichnung", Nothing)
            keywordData.Result = SafeGetString(reader, "Bezeichnung", Nothing)

            result.Add(keywordData)

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
		''' Loads assigned customer invoice address data (KD_RE_Address).
		''' </summary>
		''' <returns>List of invoice address data.</returns>
		Public Function LoadAssignedInvoiceAddressDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedInvoiceAddressData) Implements ICustomerDatabaseAccess.LoadAssignedInvoiceAddressDataOfCustomer
      Dim result As List(Of CustomerAssignedInvoiceAddressData) = Nothing

      Dim sql As String

			sql = "SELECT * FROM dbo.KD_RE_Address WHERE KDNr = @customerNumber Order By REFirma ASC"

			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedInvoiceAddressData)

          While reader.Read()
            Dim invoiceAddressData As New CustomerAssignedInvoiceAddressData
            invoiceAddressData.ID = SafeGetInteger(reader, "ID", 0)
            invoiceAddressData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            invoiceAddressData.RecordNumber = SafeGetShort(reader, "RecNr", Nothing)
            invoiceAddressData.InvoiceCompany = SafeGetString(reader, "REFirma")
            invoiceAddressData.InvoiceCompany2 = SafeGetString(reader, "REFirma2")
            invoiceAddressData.InvoiceCompany3 = SafeGetString(reader, "REFirma3")
            invoiceAddressData.InvoiceForTheAttentionOf = SafeGetString(reader, "REZhd")
            invoiceAddressData.InvoicePostOfficeBox = SafeGetString(reader, "REPostfach")
            invoiceAddressData.InvoiceStreet = SafeGetString(reader, "REStrasse")
            invoiceAddressData.InvoicePostcode = SafeGetString(reader, "REPLZ")
            invoiceAddressData.InvoiceLocation = SafeGetString(reader, "REOrt")
						invoiceAddressData.InvoiceEMailAddress = SafeGetString(reader, "REeMail")
						invoiceAddressData.InvoiceSendAsZip = SafeGetBoolean(reader, "SendAsZip", False)
						invoiceAddressData.InvoiceCountryCode = SafeGetString(reader, "RELand")
						invoiceAddressData.KSTDescription = SafeGetString(reader, "KSTBez")
            invoiceAddressData.CurrencyCode = SafeGetString(reader, "Currency")
            invoiceAddressData.Active = SafeGetBoolean(reader, "ActiveRec", Nothing)
            invoiceAddressData.InvoiceDepartment = SafeGetString(reader, "REAbteilung")
            invoiceAddressData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            invoiceAddressData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            invoiceAddressData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            invoiceAddressData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
            invoiceAddressData.PaymentCondition = SafeGetString(reader, "ZahlKond")
            invoiceAddressData.ReminderCode = SafeGetString(reader, "MahnCode")

            result.Add(invoiceAddressData)

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
    ''' Loads an assigned customer invoice address data by record number (KD_RE_Address).
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="recordNumber">The record number.</param>
    ''' <returns>Invoice address data.</returns>
    Public Function LoadAssignedInvoiceAddressDataOfCustomerByRecordNumber(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As CustomerAssignedInvoiceAddressData Implements ICustomerDatabaseAccess.LoadAssignedInvoiceAddressDataOfCustomerByRecordNumber
      Dim result As CustomerAssignedInvoiceAddressData = Nothing

      Dim sql As String

			sql = "SELECT * FROM dbo.KD_RE_Address WHERE KDNr = @customerNumber AND RecNr = @recordNumber"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("recordNumber", recordNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing AndAlso reader.Read()) Then

          result = New CustomerAssignedInvoiceAddressData

          result.ID = SafeGetInteger(reader, "ID", 0)
          result.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
          result.RecordNumber = SafeGetShort(reader, "RecNr", Nothing)
          result.InvoiceCompany = SafeGetString(reader, "REFirma")
          result.InvoiceCompany2 = SafeGetString(reader, "REFirma2")
          result.InvoiceCompany3 = SafeGetString(reader, "REFirma3")
          result.InvoiceForTheAttentionOf = SafeGetString(reader, "REZhd")
          result.InvoicePostOfficeBox = SafeGetString(reader, "REPostfach")
          result.InvoiceStreet = SafeGetString(reader, "REStrasse")
          result.InvoicePostcode = SafeGetString(reader, "REPLZ")
          result.InvoiceLocation = SafeGetString(reader, "REOrt")
					result.InvoiceEMailAddress = SafeGetString(reader, "REeMail")
					result.InvoiceSendAsZip = SafeGetBoolean(reader, "SendAsZip", False)
					result.InvoiceCountryCode = SafeGetString(reader, "RELand")
					result.KSTDescription = SafeGetString(reader, "KSTBez")
          result.CurrencyCode = SafeGetString(reader, "Currency")
          result.Active = SafeGetBoolean(reader, "ActiveRec", Nothing)
          result.InvoiceDepartment = SafeGetString(reader, "REAbteilung")
          result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
          result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
          result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
          result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
          result.PaymentCondition = SafeGetString(reader, "ZahlKond")
          result.ReminderCode = SafeGetString(reader, "MahnCode")

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
    ''' Loads assigned customer KST data (KD_KST).
    ''' </summary>
    ''' <returns>List of KST data.</returns>
    Public Function LoadAssignedKSTsOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedKSTData) Implements ICustomerDatabaseAccess.LoadAssignedKSTsOfCustomer
      Dim result As List(Of CustomerAssignedKSTData) = Nothing

      Dim sql As String

      sql = "SELECT KST.*, r.REFirma, r.REFirma2, r.REAbteilung, r.REZhd, (r.REPLZ + ' ' + r.REOrt) AS REPLZOrt FROM KD_KST KST "
      sql &= "Left Join KD_RE_Address r on kst.kdnr = r.kdnr AND kst.ReAddressRecNr = r.RecNr "
      sql &= "WHERE kst.KDNr = @customerNumber "
      sql &= "Order By KST.RecNr ASC"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedKSTData)

          While reader.Read()
            Dim kstData As New CustomerAssignedKSTData
            kstData.ID = SafeGetInteger(reader, "ID", 0)
            kstData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            kstData.RecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
            kstData.InvoiceAddressRecordNumber = SafeGetInteger(reader, "ReAddressRecNr", Nothing)
            kstData.Description = SafeGetString(reader, "Bezeichnung")
            kstData.Result = SafeGetString(reader, "Result")
            kstData.EmploymentPostCode = SafeGetString(reader, "Ort_PLZ")
            kstData.BKPostCode = SafeGetString(reader, "PK_PLZ")
            kstData.Info1 = SafeGetString(reader, "Res_info_1")
            kstData.Info2 = SafeGetString(reader, "Res_info_2", )

            kstData.InvoiceAddressInfo = String.Format("{0}, {1}, {2}", SafeGetString(reader, "REFirma"),
                                                       SafeGetString(reader, "REAbteilung"),
                                                       SafeGetString(reader, "REPLZOrt"))

            result.Add(kstData)

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
    ''' Loads an assigned customer KST data by record number (KD_KST).
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="recordNumber">The record number of the KST.</param>
    ''' <returns>KST data or nothing in error case.</returns>
    Public Function LoadAssignedKSTsOfCustomerByRecordNumber(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As CustomerAssignedKSTData Implements ICustomerDatabaseAccess.LoadAssignedKSTsOfCustomerByRecordNumber
      Dim result As CustomerAssignedKSTData = Nothing

      Dim sql As String

      sql = "SELECT * FROM KD_KST WHERE KDNr = @customerNumber And RecNr = @recordNumber"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("recordNumber", recordNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing AndAlso reader.Read()) Then

          result = New CustomerAssignedKSTData

          result.ID = SafeGetInteger(reader, "ID", 0)
          result.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
          result.RecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
          result.InvoiceAddressRecordNumber = SafeGetInteger(reader, "ReAddressRecNr", Nothing)
          result.Description = SafeGetString(reader, "Bezeichnung")
          result.Result = SafeGetString(reader, "Result")
          result.EmploymentPostCode = SafeGetString(reader, "Ort_PLZ")
          result.BKPostCode = SafeGetString(reader, "PK_PLZ")
          result.Info1 = SafeGetString(reader, "Res_info_1")
          result.Info2 = SafeGetString(reader, "Res_info_2", )

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
    ''' Loads assigned customer email data (KD_Email).
    ''' </summary>
    ''' <returns>List of email data.</returns>
    Public Function LoadAssignedEmailsOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedEmailData) Implements ICustomerDatabaseAccess.LoadAssignedEmailsOfCustomer
      Dim result As List(Of CustomerAssignedEmailData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, Bezeichnung FROM KD_Email WHERE KDNr = @customerNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedEmailData)

          While reader.Read()
            Dim emailData As New CustomerAssignedEmailData
            emailData.ID = SafeGetInteger(reader, "ID", 0)
            emailData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            emailData.Email = SafeGetString(reader, "Bezeichnung")

            result.Add(emailData)

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
    ''' Loads assigned open customer debitor invoices data (RE).
    ''' </summary>
    ''' <returns>List of open debitor invoice data.</returns>
    Public Function LoadAssignedOpenDebitorInvoicesOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of CustomerAssignedInvoiceData) Implements ICustomerDatabaseAccess.LoadAssignedOpenDebitorInvoicesOfCustomer
      Dim result As List(Of CustomerAssignedInvoiceData) = Nothing

      Dim sql As String

			sql = "SELECT ID, RENr, FAK_Dat, Faellig, BetragInk, BetragEx, Bezahlt, "
			sql &= "(BetragInk - IsNull(Bezahlt, 0)) as OpenBetrag, "
			sql &= "(Select Top 1 zMD.MDName From [Sputnik DbSelect].Dbo.[Mandanten] zMD Where zMD.MDNr = RE.MDNr) As zFiliale "
			sql &= "FROM RE WHERE KDNr = @customerNumber "
			sql &= "AND Bezahlt <> BetragInk "
			sql &= "ORDER BY Fak_Dat Desc, RENr Desc"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedInvoiceData)

          While reader.Read()
            Dim customerInvoiceData As New CustomerAssignedInvoiceData
            customerInvoiceData.ID = SafeGetInteger(reader, "ID", 0)
            customerInvoiceData.InvoiceNumber = SafeGetInteger(reader, "RENr", Nothing)
            customerInvoiceData.InvoiceDate = SafeGetDateTime(reader, "FAK_Dat", Nothing)
            customerInvoiceData.DueDate = SafeGetDateTime(reader, "Faellig", Nothing)
						customerInvoiceData.AmountEx = SafeGetDecimal(reader, "BetragEx", Nothing)
						customerInvoiceData.AmountInk = SafeGetDecimal(reader, "BetragInk", Nothing)
						customerInvoiceData.AmountPayed = SafeGetDecimal(reader, "Bezahlt", 0)
            customerInvoiceData.OpenAmount = SafeGetDecimal(reader, "OpenBetrag", Nothing)
						customerInvoiceData.zFiliale = SafeGetString(reader, "zFiliale")

            result.Add(customerInvoiceData)

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
    ''' Loads assigned responsible person document data (KD_ZDoc).
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <param name="documentRecordNumber">The document record number.</param>
    ''' <param name="categoryNumber">The category number.</param>
    ''' <returns>List of responsible person document data.</returns>
    Public Function LoadAssignedResponsiblePersonDocumentData(ByVal customerNumber As Integer, Optional ByVal responsiblePersonRecordNumber As Integer? = Nothing, Optional documentRecordNumber As Integer? = Nothing, Optional categoryNumber As Integer? = Nothing) As IEnumerable(Of ResponsiblePersonAssignedDocumentData) Implements ICustomerDatabaseAccess.LoadAssignedResponsiblePersonDocumentData
      Dim result As List(Of ResponsiblePersonAssignedDocumentData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & "SELECT Doc.ID, Doc.KDNr, Doc.KDZNr, Doc.DocPath, Doc.RecNr, Doc.CreatedOn, Doc.CreatedFrom, "
      sql = sql & "Doc.ChangedOn, Doc.ChangedFrom, Doc.Bezeichnung, Doc.Beschreibung, Doc.ScanExtension, Doc.USNr, Doc.Categorie_Nr,  "
      sql = sql & String.Format("IsNull((SELECT TOP 1 Anrede_{0} FROM Anrede ANR WHERE ANR.Anrede = ZHD.Anrede), '') As TranslatedAnrede, ", MapLanguageToShortLanguageCode(SelectedTranslationLanguage))
      sql = sql & " IsNull(ZHD.nachname, '') As Nachname, IsNull(ZHD.vorname, '') As Vorname "
      sql = sql & "FROM KD_ZDoc Doc "
      sql = sql & "LEFT JOIN KD_Zustaendig ZHD ON Doc.KDZnr = ZHD.RecNr AND Doc.Kdnr = ZHD.KDNr  "
      sql = sql & "WHERE Doc.KDNr = @customerNumber AND (@responsiblePersonNumber IS NULL OR Doc.KDZNr = @responsiblePersonNumber) "
      sql = sql & "AND (@documentRecordNumber IS NULL OR Doc.RecNr = @documentRecordNumber) "
      sql = sql & "AND (@categoryNumber IS NULL OR Doc.Categorie_Nr = @categoryNumber) "
      sql = sql & "Order By CreatedOn DESC"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonNumber", ReplaceMissing(responsiblePersonRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("documentRecordNumber", ReplaceMissing(documentRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("categoryNumber", ReplaceMissing(categoryNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonAssignedDocumentData)

          While reader.Read()
            Dim documentData As New ResponsiblePersonAssignedDocumentData
            documentData.ID = SafeGetInteger(reader, "ID", 0)
            documentData.DocumentRecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
            documentData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            documentData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", Nothing)
            documentData.DocPath = SafeGetString(reader, "DocPath", Nothing)
            documentData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            documentData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            documentData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						documentData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
            documentData.Name = SafeGetString(reader, "Bezeichnung")
            documentData.Description = SafeGetString(reader, "Beschreibung")
            documentData.ScanExtension = SafeGetString(reader, "ScanExtension")
            documentData.FileFullPath = SafeGetString(reader, "DocPath")
            documentData.USNr = SafeGetInteger(reader, "USNr", Nothing)
            documentData.CategorieNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
            documentData.TranslatedSalutation = SafeGetString(reader, "TranslatedAnrede")
            documentData.Firstname = SafeGetString(reader, "Vorname")
            documentData.Lastname = SafeGetString(reader, "Nachname")

            result.Add(documentData)

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
    '''  Loads assigned responsible person document bytes data (KD_ZDoc).
    ''' </summary>
    ''' <param name="documentId">The document id.</param>
    ''' <returns>The bytes of the document.</returns>
    Function LoadAssignedResponsiblePersonDocumentBytesData(ByVal documentId As Integer) As Byte() Implements ICustomerDatabaseAccess.LoadAssignedResponsiblePersonDocumentBytesData
      Dim result As Byte() = Nothing

      Dim sql As String

      sql = "SELECT DocScan FROM KD_ZDoc WHERE ID = @id"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", documentId))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing AndAlso reader.Read()) Then

          result = SafeGetByteArray(reader, "DocScan")

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
    ''' Loads assigned customer credit info data (KD_KreditInfo).
    ''' </summary>
    ''' <returns>List of customer document data.</returns>
    Public Function LoadAssignedCreditInfosOfCustomer(ByVal customerNumber As Integer, ByVal recordNumber? As Integer, ByVal includeReportFileBytes As Boolean) As IEnumerable(Of CustomerAssignedCreditInfo) Implements ICustomerDatabaseAccess.LoadAssignedCreditInfosOfCustomer

      Dim result As List(Of CustomerAssignedCreditInfo) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, RecNr, AB_Date, Beschreibung, ActiveRec, Bis_Date, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom, DV_ArchivID, DV_DecisionID, DV_DecisionText," &
            "USNr, DV_QueryArt, DV_FoundedAddress, DV_FoundedAddressID, Cast((Case When DV_PDFFile Is Null Then 0 Else 1 End) as Bit) As PDF_Vorhanden "

      If includeReportFileBytes Then
        sql = sql & ", DV_PDFFile"
      End If

      sql = sql & " FROM KD_KreditInfo WHERE KDNr = @customerNumber AND (@recNr is NULL OR RecNr = @recNr) "
      sql = sql & " ORDER BY CreatedOn Desc"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("recNr", ReplaceMissing(recordNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerAssignedCreditInfo)

          While reader.Read()

            Dim creditInfo As New CustomerAssignedCreditInfo

            creditInfo.ID = SafeGetInteger(reader, "ID", 0)
            creditInfo.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            creditInfo.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
            creditInfo.FromDate = SafeGetDateTime(reader, "Ab_Date", Nothing)
            creditInfo.Description = SafeGetString(reader, "Beschreibung")
            creditInfo.ActiveRec = SafeGetBoolean(reader, "ActiveRec", Nothing)
            creditInfo.ToDate = SafeGetDateTime(reader, "Bis_Date", Nothing)
            creditInfo.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            creditInfo.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            creditInfo.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            creditInfo.ChangedFrom = SafeGetString(reader, "ChangedFrom")
            creditInfo.DV_ArchiveID = SafeGetString(reader, "DV_ArchivID")
            creditInfo.DV_DecisionID = SafeGetByte(reader, "DV_DecisionID", Nothing)
            creditInfo.DV_DecisionText = SafeGetString(reader, "DV_DecisionText")
            creditInfo.USNr = SafeGetInteger(reader, "USnr", Nothing)
            creditInfo.DV_QueryType = SafeGetByte(reader, "DV_QueryArt", Nothing)
            creditInfo.DV_FoundedAddress = SafeGetString(reader, "DV_FoundedAddress")
            creditInfo.DV_FoundedAddressID = SafeGetString(reader, "DV_FoundedAddressID")
            creditInfo.HasPDFFileFlag = SafeGetBoolean(reader, "PDF_Vorhanden", False)

            If includeReportFileBytes Then
              creditInfo.DV_PDFFile = SafeGetByteArray(reader, "DV_PDFFile")
            End If

            result.Add(creditInfo)

          End While
        End If

      Catch e As Exception
        result = Nothing
        m_Logger.LogError(e.ToString())

      Finally
        CloseReader(reader)
      End Try

      Return result


      Return result
    End Function

    ''' <summary>
    ''' Loads assigned communication data of a responsible person (KD_ZKomm).
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <returns>List of communication data.</returns>
    Public Function LoadAsssignedCommunicationDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedCommuncationData) Implements ICustomerDatabaseAccess.LoadAsssignedCommunicationDataOfResponsiblePerson
      Dim result As List(Of ResponsiblePersonAssignedCommuncationData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, Bezeichnung, KDZNr FROM KD_ZKomm WHERE KDNr = @customerNumber AND KDZnr = @responsiblePersonRecordNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecordNumber", responsiblePersonRecordNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonAssignedCommuncationData)

          While reader.Read()
            Dim communicationData As New ResponsiblePersonAssignedCommuncationData
            communicationData.ID = SafeGetInteger(reader, "ID", 0)
            communicationData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            communicationData.Description = SafeGetString(reader, "Bezeichnung")
            communicationData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", 0)

            result.Add(communicationData)

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
    ''' Loads assigned communication type of a responsible person (KD_ZKontaktArt).
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <returns>List of contact type data.</returns>
    Public Function LoadAssignedConcatTypeDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As IEnumerable(Of ResponsiblePersonAssignedContactTypeData) Implements ICustomerDatabaseAccess.LoadAssignedConcatTypeDataOfResponsiblePerson

      Dim result As List(Of ResponsiblePersonAssignedContactTypeData) = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, Bezeichnung, KDZNr FROM KD_ZKontaktArt WHERE KDNr = @customerNumber AND KDZnr = @responsiblePersonRecordNumber Order By Bezeichnung ASC"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecordNumber", responsiblePersonRecordNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonAssignedContactTypeData)

          While reader.Read()
            Dim communicationData As New ResponsiblePersonAssignedContactTypeData
            communicationData.ID = SafeGetInteger(reader, "ID", 0)
            communicationData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            communicationData.Description = SafeGetString(reader, "Bezeichnung")
            communicationData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", 0)

            result.Add(communicationData)

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
    ''' Loads assigned reserve date of a responsible person (KD_ZRes1 to KD_ZRes4).
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <returns>List of contact reserve type data.</returns>
    Function LoadAssignedReserveDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer, ByVal reserveType As ResponsiblePersonReserveDataType) As IEnumerable(Of ResponsiblePersonAssignedReserveData) Implements ICustomerDatabaseAccess.LoadAssignedReserveDataOfResponsiblePerson
      Dim result As List(Of ResponsiblePersonAssignedReserveData) = Nothing

      Dim sql As String

      sql = String.Format("SELECT ID, KDNr, KDZNr, Bezeichnung, Result FROM KD_ZRes{0} WHERE KDNr = @customerNumber AND KDZnr = @responsiblePersonRecordNumber Order By Bezeichnung ASC", CType(reserveType, Integer))

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecordNumber", responsiblePersonRecordNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonAssignedReserveData)

          While reader.Read()
            Dim reserveData As New ResponsiblePersonAssignedReserveData
            reserveData.ID = SafeGetInteger(reader, "ID", 0)
            reserveData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            reserveData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", 0)
            reserveData.Description = SafeGetString(reader, "Bezeichnung")
            reserveData.Result = SafeGetString(reader, "Result")

            result.Add(reserveData)

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
    ''' Loads responsible person assigned contact data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonNumber">The responsible person number.</param>
    ''' <param name="recordNumber">The contact data record number.</param>
    ''' <returns>Contact data of responible person.</returns>
    Function LoadAssignedContactDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonNumber As Integer?, ByVal recordNumber As Integer) As ResponsiblePersonAssignedContactData Implements ICustomerDatabaseAccess.LoadAssignedContactDataOfResponsiblePerson

      Dim result As ResponsiblePersonAssignedContactData = Nothing

      Dim sql As String

      sql = "SELECT KontaktTotal.*, MAKontakte.ID AS MAKontakte_ID, MAKontakte.MANr AS MAKontakte_MANr, MAKontakte.RecNr AS MAKontakte_RecNr  " &
            "FROM KD_KontaktTotal KontaktTotal LEFT JOIN MA_Kontakte MAKontakte ON KontaktTotal.MAKontaktRecID = MAKontakte.ID" +
           " Where KontaktTotal.KDNr = @customerNumber AND KontaktTotal.RecNr = @recordNumber AND " &
            "(@responsiblePersonNumber IS NULL OR KontaktTotal.KDZNr = @responsiblePersonNumber)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim responsiblePersonNumberParameter As New SqlClient.SqlParameter("responsiblePersonNumber", ReplaceMissing(responsiblePersonNumber, DBNull.Value))
      Dim recordNumberParameter As New SqlClient.SqlParameter("recordNumber", recordNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(responsiblePersonNumberParameter)
      listOfParams.Add(recordNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            result = New ResponsiblePersonAssignedContactData
            result.ID = SafeGetInteger(reader, "ID", 0)
            result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            result.ResponsiblePersonNumber = SafeGetInteger(reader, "KDZNr", 0)
            result.ContactDate = SafeGetDateTime(reader, "KontaktDate", Nothing)
            result.ContactsString = SafeGetString(reader, "Kontakte")
            result.Username = SafeGetString(reader, "UserName")
            result.RecordNumber = SafeGetInteger(reader, "Recnr", Nothing)
            result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
            result.ContactType1 = SafeGetString(reader, "KontaktType1")
            result.ContactType2 = SafeGetShort(reader, "KontaktType2", Nothing)
            result.ContactPeriodString = SafeGetString(reader, "KontaktDauer")
            result.ContactImportant = SafeGetBoolean(reader, "KontaktWichtig", False)
            result.ContactFinished = SafeGetBoolean(reader, "KontaktErledigt", False)
            result.MANr = SafeGetInteger(reader, "MANr", Nothing)
            result.ProposeNr = SafeGetInteger(reader, "Proposenr", Nothing)
            result.VacancyNumber = SafeGetInteger(reader, "VakNr", Nothing)
            result.OfNumber = SafeGetInteger(reader, "OfNr", Nothing)
            result.Mail_ID = SafeGetInteger(reader, "Mail_ID", Nothing)
            result.TaskRecNr = SafeGetInteger(reader, "TaskRecNr", Nothing)
            result.UsNr = SafeGetInteger(reader, "USNr", Nothing)
            result.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
            result.KontaktDocID = SafeGetInteger(reader, "KontaktDocID", Nothing)
            result.EmployeeContactRecID = SafeGetInteger(reader, "MAKontakte_ID", Nothing)
            result.EmployeeContactEmployeeNr = SafeGetInteger(reader, "MAKontakte_MANr", Nothing)
            result.EmplyoeeContactRecNr = SafeGetInteger(reader, "MAKontakte_RecNr", Nothing)

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        result = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return result

    End Function


    ''' <summary>
    ''' Loads sales volume data of customer.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>List of sales volume data or nothing in error case.</returns>
    Function LoadSalesVolumeDataOfCustomer(ByVal customerNumber As Integer) As IEnumerable(Of SalesVolumeData) Implements ICustomerDatabaseAccess.LoadSalesVolumeDataOfCustomer

      Dim result As List(Of SalesVolumeData) = Nothing

      Dim sql As String

      sql = "[dbo].[Get Umsatzdata 4 Selected KDNr in Lastyears]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of SalesVolumeData)

          While reader.Read()
            Dim salesVolumeData As New SalesVolumeData
            salesVolumeData.Category = SafeGetString(reader, "Art")
            salesVolumeData.Amount = SafeGetDecimal(reader, "Betrag", 0.0)
            salesVolumeData.Year = SafeGetInteger(reader, "Jahr", 0)

            result.Add(salesVolumeData)

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
    ''' Loads distinct document category data of responsible person.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person number.</param>
    ''' <returns>Distinct document category data.</returns>
    Public Function LoadDistinctDocumentCategorieDataOfResponsiblePerson(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?) As IEnumerable(Of CustomerDocumentCategoryData) Implements ICustomerDatabaseAccess.LoadDistinctDocumentCategorieDataOfResponsiblePerson

      Dim result As List(Of CustomerDocumentCategoryData) = Nothing

      Dim sql As String

      sql = "SELECT * FROM Tab_KDDocCategories TabCategories WHERE Categorie_Nr IN "
      sql = sql & "(SELECT DISTINCT Categorie_Nr FROM KD_ZDoc WHERE KDNr = @customerNumber AND (@responsiblePersonNumber IS NULL OR KDZNr = @responsiblePersonNumber))"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonNumber", ReplaceMissing(responsiblePersonRecordNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerDocumentCategoryData)

          While reader.Read()
            Dim categoryDataData As New CustomerDocumentCategoryData
            categoryDataData.ID = SafeGetInteger(reader, "ID", 0)
            categoryDataData.CategoryNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
						categoryDataData.DescriptionGerman = SafeGetString(reader, "Bez_D")
						categoryDataData.DescriptionFrench = SafeGetString(reader, "Bez_F")
						categoryDataData.DescriptionItalian = SafeGetString(reader, "Bez_I")

            result.Add(categoryDataData)

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
    ''' Loads the lastest customer solvency check credit info.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="includeReportFileBytes">Boolean flag indicating if the report bytes should also be loaded.</param>
    ''' <returns>Latest solvency credit info.</returns>
    Function LoadLatestCustomerSolvencyCheckCreditInfo(ByVal customerNumber As Integer, ByVal includeReportFileBytes As Boolean) As CustomerAssignedCreditInfo Implements ICustomerDatabaseAccess.LoadLatestCustomerSolvencyCheckCreditInfo

      Dim result As CustomerAssignedCreditInfo = Nothing

      Dim sql As String

      sql = "SELECT ID, KDNr, RecNr, AB_Date, Beschreibung, ActiveRec, Bis_Date, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom, DV_ArchivID, DV_DecisionID, DV_DecisionText," &
           "USNr, DV_QueryArt, DV_FoundedAddress, DV_FoundedAddressID, Cast((Case When DV_PDFFile Is Null Then 0 Else 1 End) as Bit) As PDF_Vorhanden "

      If includeReportFileBytes Then
        sql = sql & ", DV_PDFFile"
      End If

      sql = sql & " From KD_KreditInfo Where Not DV_ArchivID IS NULL AND DV_ArchivID <> '' And KDNr = @customerNumber Order By CreatedOn Desc"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing AndAlso reader.Read) Then

          result = New CustomerAssignedCreditInfo
          result.ID = SafeGetInteger(reader, "ID", 0)
          result.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
          result.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
          result.FromDate = SafeGetDateTime(reader, "Ab_Date", Nothing)
          result.Description = SafeGetString(reader, "Beschreibung")
          result.ActiveRec = SafeGetBoolean(reader, "ActiveRec", Nothing)
          result.ToDate = SafeGetDateTime(reader, "Bis_Date", Nothing)
          result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
          result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
          result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
          result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
          result.DV_ArchiveID = SafeGetString(reader, "DV_ArchivID")
          result.DV_DecisionID = SafeGetByte(reader, "DV_DecisionID", Nothing)
          result.DV_DecisionText = SafeGetString(reader, "DV_DecisionText")
          result.USNr = SafeGetInteger(reader, "USnr", Nothing)
          result.DV_QueryType = SafeGetByte(reader, "DV_QueryArt", Nothing)
          result.DV_FoundedAddress = SafeGetString(reader, "DV_FoundedAddress")
          result.DV_FoundedAddressID = SafeGetString(reader, "DV_FoundedAddressID")
          result.HasPDFFileFlag = SafeGetBoolean(reader, "PDF_Vorhanden", False)

          If includeReportFileBytes Then
            result.DV_PDFFile = SafeGetByteArray(reader, "DV_PDFFile")
          End If

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
    ''' Loads user data.
    ''' </summary>
    ''' <returns>List of user data.</returns>
    Public Function LoadUserData() As IEnumerable(Of UserData) Implements ICustomerDatabaseAccess.LoadUserData

      Dim result As List(Of UserData) = Nothing

      Dim sql As String

      sql = "SELECT USNr, Nachname, Vorname, KST FROM Benutzer WHERE Deaktiviert IS NULL OR Deaktiviert = 0 ORDER BY Nachname, Vorname"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of UserData)

          While reader.Read()
            Dim userData As New UserData
            userData.UsrNr = SafeGetInteger(reader, "USNr", 0)
            userData.FirstName = SafeGetString(reader, "Vorname")
            userData.LastName = SafeGetString(reader, "Nachname")
            userData.KST = SafeGetString(reader, "KST")
            result.Add(userData)

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
    ''' Loads customer contact info data (TAB_KDKontakt).
    ''' </summary>
    ''' <returns>List of customer contact info data.</returns>
    Public Function LoadCustomerContactInfoData() As IEnumerable(Of CustomerContactInfoData) Implements ICustomerDatabaseAccess.LoadCustomerContactInfoData

      Dim result As List(Of CustomerContactInfoData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Description FROM TAB_KDKontakt ORDER BY Description ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerContactInfoData)

          While reader.Read()
            Dim contactInfoData As New CustomerContactInfoData
            contactInfoData.ID = SafeGetInteger(reader, "ID", 0)
            contactInfoData.Description = SafeGetString(reader, "Description")

            result.Add(contactInfoData)

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
    ''' Loads customer communication data (Tab_KDKommunikation).
    ''' </summary>
    ''' <returns>List of customer communication data.</returns>
    Function LoadCustomerCommunicationData() As IEnumerable(Of CustomerCommunicationData) Implements ICustomerDatabaseAccess.LoadCustomerCommunicationData

      Dim result As List(Of CustomerCommunicationData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung FROM Tab_KDKommunikation ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerCommunicationData)

          While reader.Read()
            Dim communicationData As New CustomerCommunicationData
            communicationData.ID = SafeGetInteger(reader, "ID", 0)
            communicationData.Description = SafeGetString(reader, "Bezeichnung")

            result.Add(communicationData)

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
    ''' Loads customer communication  type data (Tab_KDKommArt).
    ''' </summary>
    ''' <returns>List of customer communication type data.</returns>
    Public Function LoadCustomerCommunicationTypeData() As IEnumerable(Of CustomerCommunicationTypeData) Implements ICustomerDatabaseAccess.LoadCustomerCommunicationTypeData

      Dim result As List(Of CustomerCommunicationTypeData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_KDKommArt ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerCommunicationTypeData)

          While reader.Read()
            Dim communicationType As New CustomerCommunicationTypeData
            communicationType.ID = SafeGetInteger(reader, "ID", 0)
            communicationType.Description = SafeGetString(reader, "Bezeichnung")
            communicationType.Result = SafeGetString(reader, "Result")

            result.Add(communicationType)

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
    ''' Loads responsible person contact info data (Tab_ZHDKontakt).
    ''' </summary>
    ''' <returns>List of responsible person contact info data.</returns>
    Function LoadResponsiblePersonContactInfoData() As IEnumerable(Of ResponsiblePersonContactInfo) Implements ICustomerDatabaseAccess.LoadResponsiblePersonContactInfoData

      Dim result As List(Of ResponsiblePersonContactInfo) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_ZHDKontakt ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonContactInfo)

          While reader.Read()
            Dim contactInfoData As New ResponsiblePersonContactInfo
            contactInfoData.ID = SafeGetInteger(reader, "ID", 0)
            contactInfoData.Description = SafeGetString(reader, "Bezeichnung")
            contactInfoData.Result = SafeGetString(reader, "Result")

            result.Add(contactInfoData)

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
    ''' Loads responsible person reserve data (Tab_ZHDRes1 to Tab_ZHDRes4).
    ''' </summary>
    ''' <returns>List of responsible person reserve data.</returns>
    Public Function LoadResponsiblePersonReserveData(ByVal reserveType As ResponsiblePersonReserveDataType) As IEnumerable(Of ResponsiblePersonReserveData) Implements ICustomerDatabaseAccess.LoadResponsiblePersonReserveData

      Dim result As List(Of ResponsiblePersonReserveData) = Nothing

      Dim sql As String

      sql = String.Format("SELECT ID, Bezeichnung, Result FROM Tab_ZHDRes{0} ORDER BY Bezeichnung ASC", CType(reserveType, Integer))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonReserveData)

          While reader.Read()
            Dim reserveData As New ResponsiblePersonReserveData
            reserveData.ID = SafeGetInteger(reader, "ID", 0)
            reserveData.Description = SafeGetString(reader, "Bezeichnung")
            reserveData.Result = SafeGetString(reader, "Result")

            result.Add(reserveData)

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
    ''' Loads customer state data1 (TAB_KDStat).
    ''' </summary>
    ''' <returns>List of customer state data (1).</returns>
    Public Function LoadCustomerStateData1() As IEnumerable(Of CustomerStateData) Implements ICustomerDatabaseAccess.LoadCustomerStateData1

      Dim result As List(Of CustomerStateData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Description FROM TAB_KDStat ORDER BY Description ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerStateData)

          While reader.Read()
            Dim customerStateData As New CustomerStateData
            customerStateData.ID = SafeGetInteger(reader, "ID", 0)
            customerStateData.Description = SafeGetString(reader, "Description")

            result.Add(customerStateData)

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
    ''' Loads customer state data2 (TAB_KDStat2).
    ''' </summary>
    ''' <returns>List of customer state data (2).</returns>
    Public Function LoadCustomerStateData2() As IEnumerable(Of CustomerStateData) Implements ICustomerDatabaseAccess.LoadCustomerStateData2

      Dim result As List(Of CustomerStateData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung FROM TAB_KDStat2 ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerStateData)

          While reader.Read()
            Dim customerStateData As New CustomerStateData
            customerStateData.ID = SafeGetInteger(reader, "ID", 0)
            customerStateData.Description = SafeGetString(reader, "Bezeichnung")

            result.Add(customerStateData)

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
    ''' Loads responsible person state data1 (Tab_ZHDState1)
    ''' </summary>
    ''' <returns>List of responsible person state data (1).</returns>
    Function LoadResponsiblePersonStateData1() As IEnumerable(Of ResponsiblePersonStateData) Implements ICustomerDatabaseAccess.LoadResponsiblePersonStateData1

      Dim result As List(Of ResponsiblePersonStateData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_ZHDState1 ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonStateData)

          While reader.Read()
            Dim responsiblePersonStateData As New ResponsiblePersonStateData
            responsiblePersonStateData.ID = SafeGetInteger(reader, "ID", 0)
            responsiblePersonStateData.Description = SafeGetString(reader, "Bezeichnung")
            responsiblePersonStateData.Result = SafeGetString(reader, "Result")

            result.Add(responsiblePersonStateData)

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
    ''' Loads responsible person state data2 (Tab_ZHDState2)
    ''' </summary>
    ''' <returns>List of responsible person state data (2).</returns>
    Function LoadResponsiblePersonStateData2() As IEnumerable(Of ResponsiblePersonStateData) Implements ICustomerDatabaseAccess.LoadResponsiblePersonStateData2

      Dim result As List(Of ResponsiblePersonStateData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_ZHDState2 ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ResponsiblePersonStateData)

          While reader.Read()
            Dim responsiblePersonStateData As New ResponsiblePersonStateData
            responsiblePersonStateData.ID = SafeGetInteger(reader, "ID", 0)
            responsiblePersonStateData.Description = SafeGetString(reader, "Bezeichnung")
            responsiblePersonStateData.Result = SafeGetString(reader, "Result")

            result.Add(responsiblePersonStateData)

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
    ''' Loads the first property data.
    ''' </summary>
    ''' <returns>List of first property data.</returns>
    Public Function LoadFirstPropertyData() As IEnumerable(Of FirstPropertyData) Implements ICustomerDatabaseAccess.LoadFirstPropertyData

      Dim result As List(Of FirstPropertyData) = Nothing

      Dim sql As String

      sql = "SELECT * FROM Tab_KDFProperty ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of FirstPropertyData)

          While reader.Read()
            Dim fPropertyData As New FirstPropertyData
            fPropertyData.FPropertyValue = SafeGetDecimal(reader, "ColorCode", Nothing)
            fPropertyData.Description = SafeGetString(reader, "Bezeichnung")

            result.Add(fPropertyData)

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
    ''' Loads the second property data.
    ''' </summary>
    ''' <returns>List of second property data.</returns>
    Public Function LoadSecondPropertyData() As IEnumerable(Of SecondPropertyData) Implements ICustomerDatabaseAccess.LoadSecondPropertyData

      Dim result As List(Of SecondPropertyData) = Nothing

      Dim sql As String

      sql = "SELECT * FROM Tab_KDSProperty ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of SecondPropertyData)

          While reader.Read()
            Dim sPropertyData As New SecondPropertyData
            sPropertyData.IconIndex = SafeGetShort(reader, "IconIndex", -1)
            sPropertyData.Description = SafeGetString(reader, "Bezeichnung")

            result.Add(sPropertyData)

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
    ''' Loads employment type data (TAB_KDAnstellung)
    ''' </summary>
    ''' <returns>List of employment type data.</returns>
    Public Function LoadEmploymentTypeData() As IEnumerable(Of EmploymentTypeData) Implements ICustomerDatabaseAccess.LoadEmploymentTypeData

      Dim result As List(Of EmploymentTypeData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung FROM Tab_KDAnstellung ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of EmploymentTypeData)

          While reader.Read()
            Dim employmentType As New EmploymentTypeData
            employmentType.ID = SafeGetInteger(reader, "ID", 0)
            employmentType.Description = SafeGetString(reader, "Bezeichnung")

            result.Add(employmentType)

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
    ''' Loads keyword data (TAB_Stichwort)
    ''' </summary>
    ''' <returns>List of keyword data.</returns>
    Public Function LoadKeywordData() As IEnumerable(Of KeywordData) Implements ICustomerDatabaseAccess.LoadKeywordData

      Dim result As List(Of KeywordData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_Stichwort ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of KeywordData)

          While reader.Read()
            Dim employmentType As New KeywordData
            employmentType.ID = SafeGetInteger(reader, "ID", 0)
            employmentType.Description = SafeGetString(reader, "Bezeichnung")
            employmentType.Result = SafeGetString(reader, "Result")

            result.Add(employmentType)

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
    ''' Loads OP shipment data (Tab_OPVersand)
    ''' </summary>
    ''' <returns>List of OP shipment data.</returns>
    Public Function LoadOPShipmentData() As IEnumerable(Of OPShipmentData) Implements ICustomerDatabaseAccess.LoadOPShipmentData
      Dim result As List(Of OPShipmentData) = Nothing

      Dim sql As String

      sql = "SELECT ID, GetFeld, Result, Bezeichnung FROM Tab_OPVersand ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of OPShipmentData)

          While reader.Read()
            Dim opShipmentData As New OPShipmentData
            opShipmentData.ID = SafeGetInteger(reader, "ID", 0)
            opShipmentData.GetField = SafeGetString(reader, "GetFeld")
            opShipmentData.Result = SafeGetString(reader, "Result")
            opShipmentData.Description = SafeGetString(reader, "Bezeichnung")

            result.Add(opShipmentData)

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
    ''' Loads invoice type data (Tab_Faktura)
    ''' </summary>
    ''' <returns>List of invoice type data.</returns>
    Public Function LoadInvoiceTypeData() As IEnumerable(Of InvoiceTypeData) Implements ICustomerDatabaseAccess.LoadInvoiceTypeData
      Dim result As List(Of InvoiceTypeData) = Nothing

      Dim sql As String
			'sql = "SELECT GetFeld, Bez_{0}Description FROM Tab_Faktura ORDER BY GetFeld ASC"

			sql = String.Format("SELECT GetFeld, Bez_{0} As [Description] FROM Tab_Faktura ORDER BY GetFeld ASC", MapLanguageToShortLanguageCode(SelectedTranslationLanguage))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of InvoiceTypeData)

          While reader.Read()
            Dim invoiceTypeData As New InvoiceTypeData
            invoiceTypeData.Code = SafeGetString(reader, "GetFeld")
            invoiceTypeData.Description = SafeGetString(reader, "Description")

            result.Add(invoiceTypeData)

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
    ''' Loads number of employees data (Tab_KDMAAnz)
    ''' </summary>
    ''' <returns>List of bill type data.</returns>
    Function LoadNumberOfEmployeesData() As IEnumerable(Of NumberOfEmployeesData) Implements ICustomerDatabaseAccess.LoadNumberOfEmployeesData
      Dim result As List(Of NumberOfEmployeesData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung FROM Tab_KDMAAnz ORDER BY ID ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of NumberOfEmployeesData)

          While reader.Read()
            Dim numberOfEmployees As New NumberOfEmployeesData
            numberOfEmployees.ID = SafeGetInteger(reader, "ID", 0)
            numberOfEmployees.NumberOfEmployees = SafeGetString(reader, "Bezeichnung")

            result.Add(numberOfEmployees)

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
    ''' Loads invoice options data (Tab_KDFOptions)
    ''' </summary>
    ''' <returns>List of invoice options data.</returns>
    Function LoadInvoiceOptionsData() As IEnumerable(Of InvoiceOptionData) Implements ICustomerDatabaseAccess.LoadInvoiceOptionsData

      Dim result As List(Of InvoiceOptionData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_KDFOptions ORDER BY ID ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of InvoiceOptionData)

          While reader.Read()
            Dim invoiceOptionData As New InvoiceOptionData
            invoiceOptionData.ID = SafeGetInteger(reader, "ID", 0)
            invoiceOptionData.Description = SafeGetString(reader, "Bezeichnung")
            invoiceOptionData.Result = SafeGetString(reader, "Result")

            result.Add(invoiceOptionData)

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
    ''' Loads payment condition data (Tab_ZahlKond).
    ''' </summary>
    ''' <returns>List of payment condition data.</returns>
    Function LoadPaymentConditionData() As IEnumerable(Of PaymentConditionData) Implements ICustomerDatabaseAccess.LoadPaymentConditionData

      Dim result As List(Of PaymentConditionData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Description, GetFeld FROM Tab_ZahlKond ORDER BY Description"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of PaymentConditionData)

          While reader.Read()
            Dim paymentCondition As New PaymentConditionData
            paymentCondition.ID = SafeGetInteger(reader, "ID", 0)
            paymentCondition.Description = SafeGetString(reader, "Description")
            paymentCondition.GetField = SafeGetString(reader, "GetFeld")

            result.Add(paymentCondition)

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
		''' Loads payment reminder code data (Tab_Mahncode).
		''' </summary>
		''' <returns>List of payment reminder code data.</returns>
		Function LoadPaymentReminderCodeData() As IEnumerable(Of PaymentReminderCodeData) Implements ICustomerDatabaseAccess.LoadPaymentReminderCodeData

			Dim result As List(Of PaymentReminderCodeData) = Nothing

			Dim sql As String

			sql = "SELECT ID, Mahn1, Mahn2, Mahn3, Mahn4, GetFeld FROM Tab_Mahncode ORDER BY GetFeld"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of PaymentReminderCodeData)

					While reader.Read()
						Dim reminderCode As New PaymentReminderCodeData
						reminderCode.ID = SafeGetInteger(reader, "ID", 0)
						reminderCode.Reminder1 = SafeGetString(reader, "Mahn1")
						reminderCode.Reminder2 = SafeGetString(reader, "Mahn2")
						reminderCode.Reminder3 = SafeGetString(reader, "Mahn3")
						reminderCode.Reminder4 = SafeGetString(reader, "Mahn4")
						reminderCode.GetField = SafeGetString(reader, "GetFeld")

						result.Add(reminderCode)

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
		''' Loads customer reserve data.
		''' </summary>
		''' <param name="reserveDataType">The customer reserve data type.</param>
		''' <returns>List of customer reserve data.</returns>
		Function LoadCustomerReserveData(ByVal reserveDataType As CustomerReserveDataType) As IEnumerable(Of CustomerReserveData) Implements ICustomerDatabaseAccess.LoadCustomerReserveData
      Dim result As List(Of CustomerReserveData) = Nothing

      Dim sql As String

      sql = String.Format("SELECT ID, Bezeichnung, Result FROM Tab_KDRes{0} ORDER BY Bezeichnung ASC", CType(reserveDataType, Integer))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerReserveData)

          While reader.Read()
            Dim reserveData As New CustomerReserveData
            reserveData.ID = SafeGetInteger(reader, "ID", 0)
            reserveData.Description = SafeGetString(reader, "Bezeichnung")
            reserveData.Result = SafeGetString(reader, "Result")

            result.Add(reserveData)

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
    ''' Loads customer document category data.
    ''' </summary>
    ''' <returns>List of customer category data.</returns>
    Public Function LoadCustomerDocumentCategoryData() As IEnumerable(Of CustomerDocumentCategoryData) Implements ICustomerDatabaseAccess.LoadCustomerDocumentCategoryData

      Dim result As List(Of CustomerDocumentCategoryData) = Nothing

      Dim sql As String

			sql = "SELECT ID, Categorie_Nr, Bez_D, Bez_F, Bez_I FROM Tab_KDDocCategories ORDER BY Categorie_Nr ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerDocumentCategoryData)

          While reader.Read()
            Dim categoryDataData As New CustomerDocumentCategoryData
            categoryDataData.ID = SafeGetInteger(reader, "ID", 0)
            categoryDataData.CategoryNumber = SafeGetInteger(reader, "Categorie_Nr", 0)
						categoryDataData.DescriptionGerman = SafeGetString(reader, "Bez_D")
						categoryDataData.DescriptionFrench = SafeGetString(reader, "Bez_F")
						categoryDataData.DescriptionItalian = SafeGetString(reader, "Bez_I")

            result.Add(categoryDataData)

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
    ''' Loads department data.
    ''' </summary>
    ''' <returns>List of department data.</returns>
    Public Function LoadDepartmentData() As IEnumerable(Of DepartmentData) Implements ICustomerDatabaseAccess.LoadDepartmentData

      Dim result As List(Of DepartmentData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_Abteilung ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of DepartmentData)

          While reader.Read

            Dim departmentData = New DepartmentData()
            departmentData.ID = SafeGetInteger(reader, "ID", 0)
            departmentData.Description = SafeGetString(reader, "Bezeichnung")
            departmentData.Result = SafeGetString(reader, "Result")

            result.Add(departmentData)

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
    ''' Loads position data.
    ''' </summary>
    ''' <returns>List of position data.</returns>
    Public Function LoadPositionData() As IEnumerable(Of PositionData) Implements ICustomerDatabaseAccess.LoadPositionData

      Dim result As List(Of PositionData) = Nothing

      Dim sql As String

      sql = "SELECT ID, Bezeichnung, Result FROM Tab_Position ORDER BY Bezeichnung ASC"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of PositionData)

          While reader.Read

            Dim positionData = New PositionData()
            positionData.ID = SafeGetInteger(reader, "ID", 0)
            positionData.Description = SafeGetString(reader, "Bezeichnung")
            positionData.Result = SafeGetString(reader, "Result")

            result.Add(positionData)

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
    ''' Loads employee data.
    ''' </summary>
    ''' <param name="maNr">The maNr (optional).</param>
    ''' <returns>List of employee data.</returns>
    Function LoadEmployeeData(Optional ByVal maNr As Integer? = Nothing) As IEnumerable(Of EmployeeData) Implements ICustomerDatabaseAccess.LoadEmployeeData

      Dim result As List(Of EmployeeData) = Nothing

      Dim sql As String

      sql = "SELECT ID, MANr, Nachname, Vorname, Strasse, PLZ, Ort, Land, Geschlecht FROM Mitarbeiter WHERE (@maNr IS NULL OR MANr = @maNr) ORDER BY Nachname, Vorname ASC"

      Dim maNrParameter As New SqlClient.SqlParameter("maNr", ReplaceMissing(maNr, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(maNrParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeData)

          While reader.Read

            Dim employeeData = New EmployeeData()
            employeeData.ID = SafeGetInteger(reader, "ID", 0)
            employeeData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            employeeData.Lastname = SafeGetString(reader, "Nachname")
            employeeData.Firstname = SafeGetString(reader, "Vorname")
            employeeData.Street = SafeGetString(reader, "Strasse")
            employeeData.Postcode = SafeGetString(reader, "PLZ")
            employeeData.Location = SafeGetString(reader, "Ort")
            employeeData.CountryCode = SafeGetString(reader, "Land")
            employeeData.Gender = SafeGetString(reader, "Geschlecht")

            result.Add(employeeData)

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
    ''' Loads vacancy data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonNumber">The responsible person number.</param>
    ''' <returns>List of vacancy data.</returns>
    Function LoadVacancyData(ByVal customerNumber As Integer, Optional ByVal responsiblePersonNumber As Integer? = Nothing) As IEnumerable(Of VacancyData) Implements ICustomerDatabaseAccess.LoadVacancyData
      Dim result As List(Of VacancyData) = Nothing

      Dim sql As String

			sql = "SELECT ID, KDNr, KDZHDNr, VakNr, Bezeichnung, CreatedOn, CreatedFrom, "
			sql &= "("
			sql &= "CASE  "
			sql &= " When isnumeric(V.VakState) = 1 then (Select Top 1 bez_d From tbl_base_VakState where RecValue = V.VakState) "
			sql &= " ELSE VakState "
			sql &= " End "
			sql &= ") as Vakstate "
			sql &= " FROM Vakanzen V WHERE KDNr = @customerNumber AND (@responsiblePersonRecordNumber IS NULL OR KDZHDNr = @responsiblePersonRecordNumber) ORDER BY CreatedOn DESC"

      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim responsiblePersonRecordNumberParameter As New SqlClient.SqlParameter("responsiblePersonRecordNumber", ReplaceMissing(responsiblePersonNumber, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(responsiblePersonRecordNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of VacancyData)

          While reader.Read

            Dim vacancyData = New VacancyData()
            vacancyData.ID = SafeGetInteger(reader, "ID", 0)
            vacancyData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            vacancyData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZHDNr", Nothing)
            vacancyData.VacancyNumber = SafeGetInteger(reader, "VakNr", Nothing)
            vacancyData.Description = SafeGetString(reader, "Bezeichnung")
						vacancyData.VakState = SafeGetString(reader, "VakState")

						vacancyData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						vacancyData.CreatedFrom = SafeGetString(reader, "CreatedFrom")

            result.Add(vacancyData)

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
    ''' Loads propose data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonNumber">The responsible person number.</param>
    ''' <returns>List of propose data.</returns>
    Public Function LoadProposeData(ByVal customerNumber As Integer, Optional ByVal responsiblePersonNumber As Integer? = Nothing) As IEnumerable(Of ProposeData) Implements ICustomerDatabaseAccess.LoadProposeData
      Dim result As List(Of ProposeData) = Nothing

      Dim sql As String

			sql = "SELECT P.ID, P.ProposeNr, P.MANr, P.KDNr, P.KDZHDNr, P.Bezeichnung, P.P_State, P.CreatedOn, P.CreatedFrom, MA.Nachname, MA.Vorname "
			sql &= "FROM Propose P Left Join Mitarbeiter MA On P.MANr = MA.MANr "
			sql &= "WHERE P.KDNr = @customerNumber AND (@responsiblePersonRecordNumber IS NULL OR P.KDZHDNr = @responsiblePersonRecordNumber) ORDER BY P.CreatedOn Desc"

			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
			Dim responsiblePersonRecordNumberParameter As New SqlClient.SqlParameter("responsiblePersonRecordNumber", ReplaceMissing(responsiblePersonNumber, DBNull.Value))
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(customerNumberParameter)
			listOfParams.Add(responsiblePersonRecordNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of ProposeData)

					While reader.Read

						Dim proposeData = New ProposeData()
						proposeData.ID = SafeGetInteger(reader, "ID", 0)
						proposeData.ProposeNumber = SafeGetInteger(reader, "ProposeNr", Nothing)
						proposeData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						proposeData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZHDNr", Nothing)
						proposeData.Description = SafeGetString(reader, "Bezeichnung")

						proposeData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						proposeData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						proposeData.P_State = SafeGetString(reader, "P_State")

						proposeData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						proposeData.EmployeeFirstname = SafeGetString(reader, "Vorname")
						proposeData.EmployeeLastname = SafeGetString(reader, "Nachname")

						result.Add(proposeData)

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
    ''' Loads ES (Einsatz) data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>List of ES data.</returns>
    Public Function LoadESData(ByVal customerNumber As Integer) As IEnumerable(Of ESData) Implements ICustomerDatabaseAccess.LoadESData
      Dim result As List(Of ESData) = Nothing

      Dim sql As String

			sql = "SELECT ES.ID, ES.ESNR, ES.KDNr, ES.MANr, ES.ES_Als, ES.ES_Ab, ES.ES_Ende, MA.Nachname, MA.Vorname "
			sql &= " FROM ES Left Join Mitarbeiter MA On ES.MANr = MA.MANr "
			sql &= " WHERE ES.KDNr = @customerNumber ORDER BY ES.ES_Ab Desc"

      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ESData)

          While reader.Read

            Dim esData = New ESData()
            esData.ID = SafeGetInteger(reader, "ID", 0)
            esData.ESNumber = SafeGetInteger(reader, "ESNR", Nothing)
            esData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            esData.ES_As = SafeGetString(reader, "ES_Als")
            esData.ES_FromDate = SafeGetDateTime(reader, "ES_Ab", Nothing)
            esData.ES_ToDate = SafeGetDateTime(reader, "ES_Ende", Nothing)

						esData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						esData.EmployeeFirstname = SafeGetString(reader, "Vorname")
						esData.EmployeeLastname = SafeGetString(reader, "Nachname")

            result.Add(esData)

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
    ''' Loads contact document data.
    ''' </summary>
    ''' <param name="contactId">The contact id.</param>
    ''' <param name="includeFileBytes">Boolean flag indicating if file bytes should be included.</param>
    ''' <returns>Document data.</returns>
    Public Function LoadContactDocumentData(ByVal contactId As Integer, ByVal includeFileBytes As Boolean) As ContactDoc Implements ICustomerDatabaseAccess.LoadContactDocumentData

      Dim contactDocData As ContactDoc = Nothing

      Dim sql As String

			sql = "SELECT ID, KontaktID, CreatedOn, CreatedFrom, FileExtension "

      If includeFileBytes Then
        sql = sql & ", DocScan "
      End If

      sql = sql & "FROM Kontakt_Doc Where ID = @id"

      ' Parameters
      Dim idParameter As New SqlClient.SqlParameter("id", contactId)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)
      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            contactDocData = New ContactDoc
            contactDocData.ID = SafeGetInteger(reader, "ID", 0)

            If includeFileBytes Then
              contactDocData.FileBytes = SafeGetByteArray(reader, "DocScan")
            End If
						contactDocData.FileExtension = SafeGetString(reader, "FileExtension")

            contactDocData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            contactDocData.CreatedFrom = SafeGetString(reader, "CreatedFrom")

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        contactDocData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return contactDocData

    End Function

    ''' <summary>
    ''' Loads existing customer data by search criteria.
    ''' </summary>
    ''' <param name="company">The company.</param>
    ''' <param name="street">The street.</param>
    ''' <param name="postcode">The postcode.</param>
    ''' <param name="location">The location.</param>
    ''' <param name="countryCode">The country code.</param>
    ''' <returns>List of existing customer data.</returns>
    Public Function LoadExistingCustomersBySearchCriteria(ByVal company As String, ByVal street As String, ByVal postcode As String, ByVal location As String, ByVal countryCode As String) As IEnumerable(Of ExistingCustomerSearchData) Implements ICustomerDatabaseAccess.LoadExistingCustomersBySearchCriteria

      Dim result As List(Of ExistingCustomerSearchData) = Nothing

      Dim sql As String

      sql = "[Get Search Existing Customer For New Customer]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@Firma1", ReplaceMissing(company, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("Strasse", ReplaceMissing(street, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(postcode, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(location, String.Empty)))
      listOfParams.Add(New SqlClient.SqlParameter("Land", ReplaceMissing(countryCode, String.Empty)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ExistingCustomerSearchData)

          While reader.Read()
            Dim searchData As New ExistingCustomerSearchData
            searchData.Company = SafeGetString(reader, "Firma1")
            searchData.Street = SafeGetString(reader, "Strasse")
            searchData.Postcode = SafeGetString(reader, "PLZ")
            searchData.Location = SafeGetString(reader, "Ort")
            searchData.CountryCode = SafeGetString(reader, "Land")

						searchData.customerKST = SafeGetString(reader, "KST")
						searchData.customerAdvisor = SafeGetString(reader, "Advisor")

            result.Add(searchData)

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
    ''' Loads customer contact total info by search criteria.
    ''' </summary>
    ''' <param name="customerNumber">The contact number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <returns>List of customer contact total data or nothing in error case.</returns>
    Public Function LoadCustomerContactTotalDataBySearchCriteria(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?,
                                                                 ByVal bHideTel As Boolean, ByVal bHideOffer As Boolean, ByVal bHideMail As Boolean,
                                                                 ByVal bHideSMS As Boolean, ByVal years As Integer()) As IEnumerable(Of CustomerContactOverviewData) Implements ICustomerDatabaseAccess.LoadCustomerContactOverviewlDataBySearchCriteria

      Dim result As List(Of CustomerContactOverviewData) = Nothing

      Dim sql As String

      sql = "SELECT KDK.ID, KDK.KDNr, KDK.KDZNr, KDK.RecNr, KDK.KontaktDate, KDK.Kontakte, KDK.KontaktDauer, KDK.KontaktWichtig, KDK.KontaktErledigt, KDK.KontaktDocID, "
      sql &= "KDK.CreatedFrom, " ', (Select Top 1 US.KST From Benutzer US Where KDK.CreatedFrom = US.Vorname + ' ' + US.Nachname) As KST  "
      sql &= "(z.Nachname + ', ' + IsNull(z.Vorname, '')) As ZFullname, "
      sql &= "(Select Top 1 min(KontaktDate) As MinKontaktDate FROM KD_KontaktTotal WHERE KDNr = @customerNumber AND "
      sql &= "(@responsiblePersonRecordNumber IS NULL OR KD_KontaktTotal.KDZNr = @responsiblePersonRecordNumber Or 0 = @responsiblePersonRecordNumber)) As MinKontaktDate, "
      sql &= "(Select Top 1 max(KontaktDate) As MaxKontaktDate FROM KD_KontaktTotal WHERE KDNr = @customerNumber AND "
      sql &= "(@responsiblePersonRecordNumber IS NULL OR KD_KontaktTotal.KDZNr = @responsiblePersonRecordNumber Or 0 = @responsiblePersonRecordNumber)) As MaxKontaktDate "

      sql &= "FROM KD_KontaktTotal KDK "
      sql &= "Left Join KD_Zustaendig z On KDK.KDZNr = z.RecNr And KDK.KDNr = z.KDNr "
      sql &= "WHERE KDK.KDNr = @customerNumber AND (@responsiblePersonRecordNumber IS NULL OR KDK.KDZNr = @responsiblePersonRecordNumber Or 0 = @responsiblePersonRecordNumber) "

      If Not bHideTel Then sql &= "And KDK.Kontakte Not Like '%telefoniert" & "%' "
			If Not bHideOffer Then sql &= "And (KDK.Kontakte Not Like '%Offertennummer: %' And KDK.Kontakte Not Like '%Wurde Offerte geschickt%' AND ISNULL(KDK.KontaktDauer, '') Not Like '% Offerte wurde %') "
      If Not bHideMail Then sql &= "And KDK.Kontakte Not Like '%Mail-Nachricht gesendet" & "%' "
      If Not bHideSMS Then sql &= "And KDK.Kontakte Not Like '%SMS-Nachricht gesendet" & "%' "

      If Not years Is Nothing AndAlso years.Count > 0 Then
        sql = sql & "And year(KDK.KontaktDate) IN ("
        sql = sql & String.Join(", ", years)
        sql = sql & ") "
      End If
      sql = sql & " ORDER BY KDK.KontaktDate DESC"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@customerNumber", ReplaceMissing(customerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@responsiblePersonRecordNumber", ReplaceMissing(responsiblePersonRecordNumber, DBNull.Value)))


      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerContactOverviewData)

          While reader.Read()
            Dim searchData As New CustomerContactOverviewData
            searchData.ID = SafeGetInteger(reader, "ID", 0)
            searchData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            searchData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", Nothing)
            searchData.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
            searchData.ContactDate = SafeGetDateTime(reader, "KontaktDate", Nothing)
            searchData.PersonOrSubject = String.Format("({0}) {1}", SafeGetString(reader, "ZFullname"), SafeGetString(reader, "KontaktDauer"))
            searchData.Description = SafeGetString(reader, "Kontakte")
            searchData.IsImportant = SafeGetBoolean(reader, "KontaktWichtig", False)
            searchData.IsCompleted = SafeGetBoolean(reader, "KontaktErledigt", False)
            searchData.Creator = SafeGetString(reader, "CreatedFrom")
            searchData.DocumentID = SafeGetInteger(reader, "KontaktDocID", Nothing)

            searchData.minContactDate = SafeGetDateTime(reader, "MinKontaktDate", Nothing)
            searchData.maxContactDate = SafeGetDateTime(reader, "MaxKontaktDate", Nothing)

            result.Add(searchData)

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
    ''' Loads customer contact total (KD_KontaktTotal) distinct years.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <returns>List of years or nothing in error case.</returns>
    Public Function LoadCustomerContactTotalDistinctYears(ByVal customerNumber As Integer, Optional ByVal responsiblePersonRecordNumber As Integer? = Nothing) As IEnumerable(Of Integer) Implements ICustomerDatabaseAccess.LoadCustomerContactTotalDistinctYears
      Dim result As List(Of Integer) = Nothing

      Dim sql As String

      sql = "SELECT ContactYear FROM (SELECT DISTINCT YEAR(KontaktDate) as ContactYear FROM KD_KontaktTotal WHERE KDNr = @customerNumber AND (@responsiblePersonRecordNumber IS NULL OR KDZNr = @responsiblePersonRecordNumber) AND KontaktDate IS NOT NULL UNION SELECT YEAR(GetDate()) as ContactYear) as ContactYears ORDER BY ContactYear desc"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecordNumber", ReplaceMissing(responsiblePersonRecordNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Integer)

          While reader.Read
            Dim year As Integer = SafeGetInteger(reader, "ContactYear", 0)
            result.Add(year)
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
    ''' Loads context menu data for print.
    ''' </summary>
    Public Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint) Implements ICustomerDatabaseAccess.LoadContextMenu4PrintData

      Dim result As List(Of ContextMenuForPrint) = Nothing

      Dim sql As String

			sql = "[Get List Of Documents For Print in Customer]"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ContextMenuForPrint)

          While reader.Read()
            Dim mnuItems As New ContextMenuForPrint
            mnuItems.MnuName = SafeGetString(reader, "jobNr", String.Empty)
            mnuItems.MnuCaption = SafeGetString(reader, "Bezeichnung", String.Empty)

            result.Add(mnuItems)

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
    ''' Loads context menu data for print (templates).
    ''' </summary>
    Public Function LoadContextMenu4PrintTemplatesData() As IEnumerable(Of ContextMenuForPrintTemplates) Implements ICustomerDatabaseAccess.LoadContextMenu4PrintTemplatesData

      Dim result As List(Of ContextMenuForPrintTemplates) = Nothing

      Dim sql As String

      sql = "Select IsNull(t.menuLabel, '') As menuLabel, IsNull(t.docfullname, '') As docfullname, IsNull(t.Makroname, '') As Makroname From Tab_TemplateMenu t Where t.ItemShowIn = 'KD' Order By t.RecNr"

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ContextMenuForPrintTemplates)

          While reader.Read()
            Dim mnuItems As New ContextMenuForPrintTemplates
            mnuItems.MnuDocPath = SafeGetString(reader, "docfullname", String.Empty)
            mnuItems.MnuDocMacro = SafeGetString(reader, "makroname", String.Empty)
            mnuItems.MnuCaption = SafeGetString(reader, "menulabel", String.Empty)

            result.Add(mnuItems)

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
    ''' Loads customer dependent contact data.
    ''' </summary>
    '''<param name="kdRecID">Custmer record id.</param>
    ''' <returns>List of customer dependent employee contact data.</returns>
    Public Function LoadCustomerDependentEmployeeContactData(ByVal kdRecID As Integer) As IEnumerable(Of CustomerDependentEmployeeContactData) Implements ICustomerDatabaseAccess.LoadCustomerDependentEmployeeContactData

      Dim result As List(Of CustomerDependentEmployeeContactData) = Nothing

      Dim sql As String = String.Empty

      sql = sql & "SELECT K.ID AS MAKontaktID, K.RecNr AS MAKontaktRecNr, M.MANr, M.Nachname, M.Vorname, M.Strasse, M.PLZ, M.Ort, M.Land, M.Geschlecht "
      sql = sql & "FROM MA_Kontakte  K "
      sql = sql & "INNER JOIN Mitarbeiter M ON K.MANr = M.MANr "
      sql = sql & "WHERE KDKontaktRecID = @kdRecID "

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("kdRecID", kdRecID))
      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerDependentEmployeeContactData)

          While reader.Read()
            Dim contactData As New CustomerDependentEmployeeContactData
            contactData.EmployeeContactID = SafeGetInteger(reader, "MAKontaktID", 0)
            contactData.EmployeeContactRecordNumber = SafeGetInteger(reader, "MAKontaktRecNr", 0)
            contactData.CustomerContactID = kdRecID
            contactData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
            contactData.LastName = SafeGetString(reader, "Nachname")
            contactData.FirstName = SafeGetString(reader, "Vorname")
            contactData.Street = SafeGetString(reader, "Strasse")
            contactData.Postcode = SafeGetString(reader, "PLZ")
            contactData.Location = SafeGetString(reader, "Ort")
            contactData.CountryCode = SafeGetString(reader, "Land")

            result.Add(contactData)

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
		''' Add new customer.
		''' </summary>
		Function AddNewCustomer(ByVal customer As NewCustomerInitData) As Boolean Implements ICustomerDatabaseAccess.AddNewCustomer
			Dim success = True

			Dim sql As String

			sql = "[Create New Customer]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", customer.CustomerMandantNumber))

			listOfParams.Add(New SqlClient.SqlParameter("company1", ReplaceMissing(customer.Company1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("street", ReplaceMissing(customer.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("countrycode", ReplaceMissing(customer.CountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("postcode", ReplaceMissing(customer.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("location", ReplaceMissing(customer.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("kst", ReplaceMissing(customer.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("currency", ReplaceMissing(customer.CurrencyCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OneInvoicePerMail", ReplaceMissing(customer.OneInvoicePerMail, False)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceremindercode", ReplaceMissing(customer.ReminderCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("conditionalcash", ReplaceMissing(customer.PaymentCondition, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoicetype", ReplaceMissing(customer.InvoiceOption, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customernotuse", ReplaceMissing(customer.NoUse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("warnbycreditlimitexceeded", ReplaceMissing(customer.CreditWarning, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("firstcreditlimitamount", ReplaceMissing(customer.CreditLimit1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("secondcreditlimitamount", ReplaceMissing(customer.CreditLimit2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumberOffset", ReplaceMissing(customer.CustomerNumberOffset, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("usFiliale", ReplaceMissing(customer.KDBusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(customer.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(customer.CreatedUserNumber, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@idNewCustomer", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing Then
				customer.CustomerNumber = CType(newIdParameter.Value, Integer)
			Else
				success = False
			End If


			Return success

		End Function


		'Dim MandantParameter As New SqlClient.SqlParameter("@MDNr", ReplaceMissing(iMDNr, DBNull.Value))
		'	Dim companyParameter As New SqlClient.SqlParameter("@company1", ReplaceMissing(company, DBNull.Value))
		'	Dim streetParameter As New SqlClient.SqlParameter("@street", ReplaceMissing(street, DBNull.Value))
		'	Dim countryParameter As New SqlClient.SqlParameter("@countrycode", ReplaceMissing(country, DBNull.Value))
		'	Dim plzParameter As New SqlClient.SqlParameter("@postcode", ReplaceMissing(plz, DBNull.Value))
		'	Dim locationParameter As New SqlClient.SqlParameter("@location", ReplaceMissing(location, DBNull.Value))
		'	Dim kstParameter As New SqlClient.SqlParameter("@kst", ReplaceMissing(Kst, DBNull.Value))

		'	Dim currencyParameter As New SqlClient.SqlParameter("@currency", ReplaceMissing(Currency, DBNull.Value))
		'	Dim invoiceremindercodeParameter As New SqlClient.SqlParameter("@invoiceremindercode", ReplaceMissing(invoiceremindercode, DBNull.Value))
		'	Dim conditionalcashParameter As New SqlClient.SqlParameter("@conditionalcash", ReplaceMissing(conditionalcash, DBNull.Value))
		'	Dim invoicetypeParameter As New SqlClient.SqlParameter("@invoicetype", ReplaceMissing(invoicetype, DBNull.Value))
		'	Dim customernotuseParameter As New SqlClient.SqlParameter("@customernotuse", ReplaceMissing(customernotuse, DBNull.Value))
		'	Dim warnbycreditlimitexceededParameter As New SqlClient.SqlParameter("@warnbycreditlimitexceeded", ReplaceMissing(warnbycreditlimitexceeded, DBNull.Value))

		'	Dim firstcreditlimitamountParameter As New SqlClient.SqlParameter("@firstcreditlimitamount", ReplaceMissing(firstcreditlimitamount, DBNull.Value))
		'	Dim secondcreditlimitamountParameter As New SqlClient.SqlParameter("@secondcreditlimitamount", ReplaceMissing(secondcreditlimitamount, DBNull.Value))




		'	Dim customerNumberOffsetParameter As New SqlClient.SqlParameter("@customerNumberOffset", ReplaceMissing(customerNumberOffset, DBNull.Value))
		'	Dim usFilialeParameter As New SqlClient.SqlParameter("@usFiliale", ReplaceMissing(usFiliale, DBNull.Value))
		'	Dim createdByParameter As New SqlClient.SqlParameter("@createdFrom", ReplaceMissing(createdFrom, DBNull.Value))
		'	Dim createdUserNumberParameter As New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(CreatedUserNumber, DBNull.Value))

		'	Dim listOfParams As New List(Of SqlClient.SqlParameter)

		'	listOfParams.Add(MandantParameter)
		'	listOfParams.Add(companyParameter)
		'	listOfParams.Add(streetParameter)
		'	listOfParams.Add(countryParameter)
		'	listOfParams.Add(plzParameter)
		'	listOfParams.Add(locationParameter)
		'	listOfParams.Add(kstParameter)

		'	listOfParams.Add(currencyParameter)
		'	listOfParams.Add(invoiceremindercodeParameter)
		'	listOfParams.Add(conditionalcashParameter)
		'	listOfParams.Add(invoicetypeParameter)
		'	listOfParams.Add(customernotuseParameter)
		'	listOfParams.Add(warnbycreditlimitexceededParameter)
		'	listOfParams.Add(firstcreditlimitamountParameter)
		'	listOfParams.Add(secondcreditlimitamountParameter)

		'	listOfParams.Add(customerNumberOffsetParameter)
		'	listOfParams.Add(usFilialeParameter)
		'	listOfParams.Add(createdByParameter)


		''' <summary>
		''' Adds a new customer credit info assignment to the database.
		''' </summary>
		''' <param name="customerCreditInfo">The customer credit info data.</param>
		''' <returns>Boolean flag indicting success.</returns>
		Function AddCustomerCreditInfoAssignment(ByVal customerCreditInfo As CustomerAssignedCreditInfo) As Boolean Implements ICustomerDatabaseAccess.AddCustomerCreditInfoAssignment

			Dim success = True

			Dim sql As String

			sql = "[Create New KD_KreditInfo]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerCreditInfo.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Ab_Date", ReplaceMissing(customerCreditInfo.FromDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Beschreibung", ReplaceMissing(customerCreditInfo.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ActiveRec", ReplaceMissing(customerCreditInfo.ActiveRec, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Bis_Date", ReplaceMissing(customerCreditInfo.ToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(customerCreditInfo.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(customerCreditInfo.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(customerCreditInfo.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(customerCreditInfo.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DV_ArchivID", ReplaceMissing(customerCreditInfo.DV_ArchiveID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DV_DecisionID", ReplaceMissing(customerCreditInfo.DV_DecisionID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DV_DecisionText", ReplaceMissing(customerCreditInfo.DV_DecisionText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@USNr", ReplaceMissing(customerCreditInfo.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DV_QueryArt", ReplaceMissing(customerCreditInfo.DV_QueryType, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DV_FoundedAddress", ReplaceMissing(customerCreditInfo.DV_FoundedAddress, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@DV_FoundedAddressID", ReplaceMissing(customerCreditInfo.DV_FoundedAddressID, DBNull.Value)))

			Dim fileBytesParameter = New SqlClient.SqlParameter("@DV_PDFFile", DbType.Binary, If(customerCreditInfo.DV_PDFFile Is Nothing, 0, customerCreditInfo.DV_PDFFile.Length))
			fileBytesParameter.Value = ReplaceMissing(customerCreditInfo.DV_PDFFile, DBNull.Value)
			listOfParams.Add(fileBytesParameter)


			Dim newIdParameter = New SqlClient.SqlParameter("@IdNewCustomerCreditInfo", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)


			Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
			recNrParameter.Direction = ParameterDirection.Output
			listOfParams.Add(recNrParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing AndAlso
		 Not recNrParameter.Value Is Nothing Then
				customerCreditInfo.ID = newIdParameter.Value
				customerCreditInfo.RecordNumber = recNrParameter.Value
			Else
				Return False
			End If

			Return success

		End Function

		''' <summary>
		''' Adds a new responsible person.
		''' </summary>
		''' <param name="responsiblePersonMasterData">The responsible person data.</param>
		''' <returns>Boolean flag indicting success.</returns>
		Public Function AddNewResponsiblePerson(ByVal responsiblePersonMasterData As ResponsiblePersonMasterData) As Boolean Implements ICustomerDatabaseAccess.AddNewResponsiblePerson

      Dim success = True

      Dim sql As String

      sql = "[Create New KD_Zustaendig]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(responsiblePersonMasterData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Anrede", ReplaceMissing(responsiblePersonMasterData.Salutation, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Nachname", ReplaceMissing(responsiblePersonMasterData.Lastname, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Vorname", ReplaceMissing(responsiblePersonMasterData.Firstname, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Abteilung", ReplaceMissing(responsiblePersonMasterData.Department, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Position", ReplaceMissing(responsiblePersonMasterData.Position, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Telefon", ReplaceMissing(responsiblePersonMasterData.Telephone, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Telefax", ReplaceMissing(responsiblePersonMasterData.Telefax, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Natel", ReplaceMissing(responsiblePersonMasterData.MobilePhone, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@eMail", ReplaceMissing(responsiblePersonMasterData.Email, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@facebook", ReplaceMissing(responsiblePersonMasterData.Facebook, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@linkedIn", ReplaceMissing(responsiblePersonMasterData.LinkedIn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@xing", ReplaceMissing(responsiblePersonMasterData.Xing, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("@Geb_Dat", ReplaceMissing(responsiblePersonMasterData.Birthdate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Interessen", ReplaceMissing(responsiblePersonMasterData.Interests, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Firma1", ReplaceMissing(responsiblePersonMasterData.Company1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Postfach", ReplaceMissing(responsiblePersonMasterData.PostOfficeBox, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Strasse", ReplaceMissing(responsiblePersonMasterData.Street, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PLZ", ReplaceMissing(responsiblePersonMasterData.Postcode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Land", ReplaceMissing(responsiblePersonMasterData.CountryCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ort", ReplaceMissing(responsiblePersonMasterData.Location, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Berater", ReplaceMissing(responsiblePersonMasterData.Advisor, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ErstKontakt", ReplaceMissing(responsiblePersonMasterData.FirstContactDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@LetztKontakt", ReplaceMissing(responsiblePersonMasterData.LastContactDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZState1", ReplaceMissing(responsiblePersonMasterData.State1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZBemerkung", ReplaceMissing(responsiblePersonMasterData.KDZComments, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZHowKontakt", ReplaceMissing(responsiblePersonMasterData.KDZHowKontakt, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZState2", ReplaceMissing(responsiblePersonMasterData.State2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@AnredeForm", ReplaceMissing(responsiblePersonMasterData.SalutationForm, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(responsiblePersonMasterData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(responsiblePersonMasterData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(responsiblePersonMasterData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(responsiblePersonMasterData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bemerkung", ReplaceMissing(responsiblePersonMasterData.Comments, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZHD_Telefax_Mailing", ReplaceMissing(responsiblePersonMasterData.Telefax_Mailing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZHD_SMS_Mailing", ReplaceMissing(responsiblePersonMasterData.SMS_Mailing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZHD_Mail_Mailing", ReplaceMissing(responsiblePersonMasterData.Email_Mailing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Transfered_Guid", ReplaceMissing(responsiblePersonMasterData.TransferedGuid, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@AGB_WOS", ReplaceMissing(responsiblePersonMasterData.TermsAndConditions_WOS, DBNull.Value)))

      Dim newIdParameter = New SqlClient.SqlParameter("@IDNewKDZuestaendig ", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
         Not recNrParameter.Value Is Nothing Then
        responsiblePersonMasterData.ID = newIdParameter.Value
        responsiblePersonMasterData.RecordNumber = recNrParameter.Value
      Else
        Return False
      End If

      Return True
    End Function


    ''' <summary>
    ''' Adds a customer bussiness branch assignment.
    ''' </summary>
    ''' <param name="customerBusinessBranch">The customer business branch object.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddCustomerBussinessBranchAssignment(ByVal customerBusinessBranch As CustomerAssignedBusinessBranchData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerBussinessBranchAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_Filiale (KDNr, Bezeichnung, MDNr) VALUES(@customerNumber, @Name, @mdNumber)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerBusinessBranch.CustomerNumber, DBNull.Value))
      Dim nameParameter As New SqlClient.SqlParameter("name", ReplaceMissing(customerBusinessBranch.Name, DBNull.Value))
      Dim mdNumberParameter As New SqlClient.SqlParameter("mdNumber", ReplaceMissing(customerBusinessBranch.MDNr, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(nameParameter)
      listOfParams.Add(mdNumberParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds a customer profession assignment.
    ''' </summary>
    ''' <param name="customerProfession">The customer profession object.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddCustomerProfessionAssignment(ByVal customerProfession As CustomerAssignedProfessionData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerProfessionAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_Berufe (KDNr, BerufsCode, Bezeichnung, BerufCode) VALUES(@customerNumber, @professionCodeString, @description, @professionCodeInteger)"
			sql = "[Add New Customer Profession]"

			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerProfession.CustomerNumber, DBNull.Value))
			'Dim professionCodeStringParameter As New SqlClient.SqlParameter("professionCodeString", ReplaceMissing(customerProfession.ProfessionCodeString, DBNull.Value))
			Dim descriptionStringParameter As New SqlClient.SqlParameter("bezeichnung", ReplaceMissing(customerProfession.Description, DBNull.Value))
			Dim professionCodeIntegerParameter As New SqlClient.SqlParameter("code", ReplaceMissing(customerProfession.ProfessionCodeInteger, DBNull.Value))
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(customerNumberParameter)
			'listOfParams.Add(professionCodeStringParameter)
			listOfParams.Add(descriptionStringParameter)
      listOfParams.Add(professionCodeIntegerParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

    End Function

    ''' <summary>
    ''' Adds a responsible person profession assignment.
    ''' </summary>
    ''' <param name="responsiblePersonProfession">The responsible person profession object.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function AddResponsiblePersonProfessionAssignment(ByVal responsiblePersonProfession As ResponsiblePersonAssignedProfessionData) As Boolean Implements ICustomerDatabaseAccess.AddResponsiblePersonProfessionAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_ZBerufe (KDNr, KDZNr, BerufsCode, Bezeichnung, BerufCode) VALUES(@customerNumber, @responsiblePersonRecordNumber, @professionCodeString, @description, @professionCodeInteger)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(responsiblePersonProfession.CustomerNumber, DBNull.Value))
      Dim responsiblePersonRecordNumberParameter As New SqlClient.SqlParameter("responsiblePersonRecordNumber", ReplaceMissing(responsiblePersonProfession.ResponsiblePersonRecordNumber, DBNull.Value))
      Dim professionCodeStringParameter As New SqlClient.SqlParameter("professionCodeString", ReplaceMissing(responsiblePersonProfession.ProfessionCodeString, DBNull.Value))
      Dim descriptionStringParameter As New SqlClient.SqlParameter("description", ReplaceMissing(responsiblePersonProfession.Description, DBNull.Value))
      Dim professionCodeIntegerParameter As New SqlClient.SqlParameter("professionCodeInteger", ReplaceMissing(responsiblePersonProfession.ProfessionCodeInteger, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(responsiblePersonRecordNumberParameter)
      listOfParams.Add(professionCodeStringParameter)
      listOfParams.Add(descriptionStringParameter)
      listOfParams.Add(professionCodeIntegerParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

		''' <summary>
		''' Adds a customer sector assignment.
		''' </summary>
		''' <param name="customerSector">The customer sector object.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function AddCustomerSectorAssignment(ByVal customerSector As CustomerAssignedSectorData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerSectorAssignment

			Dim success = True

			Dim sql As String

			sql = "INSERT INTO KD_Branche (KDNr, Bezeichnung, Result, BranchenCode) VALUES(@customerNumber, @description, @result, @sectorCode)"
			sql = "[Add New Customer Sector]"


			' Parameters
			Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerSector.CustomerNumber, DBNull.Value))
			Dim descriptionParameter As New SqlClient.SqlParameter("bezeichnung", ReplaceMissing(customerSector.Description, DBNull.Value))
			'Dim resultParameter As New SqlClient.SqlParameter("result", ReplaceMissing(customerSector.Result, DBNull.Value))
			Dim sectorCodeParameter As New SqlClient.SqlParameter("sectorCode", ReplaceMissing(customerSector.SectorCode, DBNull.Value))
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(customerNumberParameter)
			listOfParams.Add(descriptionParameter)
			'listOfParams.Add(resultParameter)
			listOfParams.Add(sectorCodeParameter)


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function


		''' <summary>
		''' Adds a responsible person sector assignment.
		''' </summary>
		''' <param name="responsiblePersonSector">The responsible person sector object.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function AddResponsiblePersonSectorAssignment(ByVal responsiblePersonSector As ResponsiblePersonAssignedSectorData) As Boolean Implements ICustomerDatabaseAccess.AddResponsiblePersonSectorAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_ZBranche (KDNr, KDZNr, Bezeichnung, Result, BranchenCode) VALUES(@customerNumber, @responsiblePersonRecordNumber,  @description, @result, @sectorCode)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(responsiblePersonSector.CustomerNumber, DBNull.Value))
      Dim responsiblePersonRecordNumberParameter As New SqlClient.SqlParameter("responsiblePersonRecordNumber", ReplaceMissing(responsiblePersonSector.ResponsiblePersonRecordNumber, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(responsiblePersonSector.Description, DBNull.Value))
      Dim resultParameter As New SqlClient.SqlParameter("result", ReplaceMissing(responsiblePersonSector.Result, DBNull.Value))
      Dim sectorCodeParameter As New SqlClient.SqlParameter("sectorCode", ReplaceMissing(responsiblePersonSector.SectorCode, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(responsiblePersonRecordNumberParameter)
      listOfParams.Add(descriptionParameter)
      listOfParams.Add(resultParameter)
      listOfParams.Add(sectorCodeParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds a customer employment type assignment.
    ''' </summary>
    ''' <param name="customerEmploymentType">The customer employment type object.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function AddCustomerEmploymentTypeAssignment(ByVal customerEmploymentType As CustomerAssignedEmploymentTypeData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerEmploymentTypeAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_Anstellung (KDNr, KDZUNr, Bezeichnung) VALUES(@customerNumber, @customerNumber2, @description)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerEmploymentType.CustomerNumber, DBNull.Value))
      Dim customerNumber2Parameter As New SqlClient.SqlParameter("customerNumber2", ReplaceMissing(customerEmploymentType.CustomerNumber2, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(customerEmploymentType.Description, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(customerNumber2Parameter)
      listOfParams.Add(descriptionParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds a customer employment type assignment.
    ''' </summary>
    ''' <param name="customerKeyword">The customer keyword object.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddCustomerKeywordAssignment(ByVal customerKeyword As CustomerAssignedKeywordData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerKeywordAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_Stichwort (KDNr, Bezeichnung, Result) VALUES(@customerNumber, @description, @result)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerKeyword.CustomerNumber, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(customerKeyword.Description, DBNull.Value))
      Dim resultParameter As New SqlClient.SqlParameter("result", ReplaceMissing(customerKeyword.Result, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(descriptionParameter)
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds a customer GAV group assignment.
    ''' </summary>
    ''' <param name="customerGAVGroup">The customer GAV group object.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddCustomerGAVGroupAssignment(ByVal customerGAVGroup As CustomerAssignedGAVGroupData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerGAVGroupAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_GAVGruppe (KDNr, Bezeichnung, Kanton, GAVNumber) VALUES(@customerNumber, @description, @canton, @gavNumber)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerGAVGroup.CustomerNumber, DBNull.Value))
      Dim descriptionParameter As New SqlClient.SqlParameter("description", ReplaceMissing(customerGAVGroup.Description, DBNull.Value))
      Dim cantonParameter As New SqlClient.SqlParameter("canton", ReplaceMissing(customerGAVGroup.Canton, DBNull.Value))
      Dim gavNumberParameter As New SqlClient.SqlParameter("gavNumber", ReplaceMissing(customerGAVGroup.GAVNUmber, DBNull.Value))

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(descriptionParameter)
      listOfParams.Add(cantonParameter)
      listOfParams.Add(gavNumberParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Adds a customer email assignment.
    ''' </summary>
    ''' <param name="customerEmailData">The customer email bject.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function AddCustomerEmailAssignment(ByVal customerEmailData As CustomerAssignedEmailData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerEmailAssignment
      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_Email (KDNr, Bezeichnung) VALUES(@customerNumber, @email)"

      ' Parameters
      Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerEmailData.CustomerNumber, DBNull.Value))
      Dim emailParameter As New SqlClient.SqlParameter("email", ReplaceMissing(customerEmailData.Email, DBNull.Value))

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)
      listOfParams.Add(emailParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success
    End Function

    ''' <summary>
    ''' Add customer invoice address assignent.
    ''' </summary>
    ''' <param name="invoiceAddress">The invoice address data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddCustomerInvoiceAddressAssignment(ByVal invoiceAddress As CustomerAssignedInvoiceAddressData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerInvoiceAddressAssignment
      Dim success = True

      Dim sql As String

      sql = "[Create New KD_RE_Address]"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(invoiceAddress.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REFirma", ReplaceMissing(invoiceAddress.InvoiceCompany, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REFrima2", ReplaceMissing(invoiceAddress.InvoiceCompany2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REFirma3", ReplaceMissing(invoiceAddress.InvoiceCompany3, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REZhd", ReplaceMissing(invoiceAddress.InvoiceForTheAttentionOf, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REPostfach", ReplaceMissing(invoiceAddress.InvoicePostOfficeBox, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REStrasse", ReplaceMissing(invoiceAddress.InvoiceStreet, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REPLZ", ReplaceMissing(invoiceAddress.InvoicePostcode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@REOrt", ReplaceMissing(invoiceAddress.InvoiceLocation, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", ReplaceMissing(invoiceAddress.InvoiceEMailAddress, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SendAsZip", ReplaceMissing(invoiceAddress.InvoiceSendAsZip, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RELand", ReplaceMissing(invoiceAddress.InvoiceCountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KSTBez", ReplaceMissing(invoiceAddress.KSTDescription, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(invoiceAddress.CurrencyCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ActiveRec", ReplaceMissing(invoiceAddress.Active, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ReAbteilung", ReplaceMissing(invoiceAddress.InvoiceDepartment, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(invoiceAddress.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(invoiceAddress.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(invoiceAddress.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(invoiceAddress.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ZahlKond", ReplaceMissing(invoiceAddress.PaymentCondition, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@MahnCode", ReplaceMissing(invoiceAddress.ReminderCode, DBNull.Value)))

      Dim newIdParameter = New SqlClient.SqlParameter("@NewREAddressID", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
          Not recNrParameter.Value Is Nothing Then
        invoiceAddress.ID = CType(newIdParameter.Value, Integer)
        invoiceAddress.RecordNumber = CType(recNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success
    End Function

    ''' <summary>
    ''' Adds new customer KST assignment.
    ''' </summary>
    ''' <param name="kst">The KST data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddCustomerKSTAssignment(ByVal kst As CustomerAssignedKSTData) As Boolean Implements ICustomerDatabaseAccess.AddCustomerKSTAssignment

      Dim success = True

      Dim sql As String

      sql = "[Create New KD_KST]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(kst.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ReAddressRecNr", ReplaceMissing(kst.InvoiceAddressRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bezeichnung", ReplaceMissing(kst.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Result", ReplaceMissing(kst.Result, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Ort_PLZ", ReplaceMissing(kst.EmploymentPostCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@PK_PLZ", ReplaceMissing(kst.BKPostCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Res_info_1", ReplaceMissing(kst.Info1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Res_info_2", ReplaceMissing(kst.Info2, DBNull.Value)))


      Dim newIdParameter = New SqlClient.SqlParameter("@NewKDKstID", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
          Not recNrParameter.Value Is Nothing Then
        kst.ID = CType(newIdParameter.Value, Integer)
        kst.RecordNumber = CType(recNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a responsible person communication data assignment.
    ''' </summary>
    ''' <param name="communicationData">The communication data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddResponsiblePersonCommunicationAssignment(ByVal communicationData As ResponsiblePersonAssignedCommuncationData) As Boolean Implements ICustomerDatabaseAccess.AddResponsiblePersonCommunicationAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_ZKomm (KDNr, Bezeichnung, KDZNr) VALUES(@customerNumber, @description, @responsiblePersonRecNumber); SELECT @@IDENTITY"

      ' Parameters


      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(communicationData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(communicationData.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecNumber", ReplaceMissing(communicationData.ResponsiblePersonRecordNumber, DBNull.Value)))

      Dim id = ExecuteScalar(sql, listOfParams)

      If Not id Is Nothing Then
        communicationData.ID = CType(id, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a responsible person contact type data assignment.
    ''' </summary>
    ''' <param name="contactTypeData">The contact type data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddResponsiblePersonContactTypeAssignment(ByVal contactTypeData As ResponsiblePersonAssignedContactTypeData) As Boolean Implements ICustomerDatabaseAccess.AddResponsiblePersonContactTypeAssignment

      Dim success = True

      Dim sql As String

      sql = "INSERT INTO KD_ZKontaktArt (KDNr, KDZNr, Bezeichnung, Result) VALUES(@customerNumber, @responsiblePersonRecNumber, @description, @result); SELECT @@IDENTITY"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(contactTypeData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecNumber", ReplaceMissing(contactTypeData.ResponsiblePersonRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(contactTypeData.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("result", ReplaceMissing(contactTypeData.Result, DBNull.Value)))

      Dim id = ExecuteScalar(sql, listOfParams)

      If Not id Is Nothing Then
        contactTypeData.ID = CType(id, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a responsible person reserve data assignment.
    ''' </summary>
    ''' <param name="reserveData">The reserve data.</param>
    ''' <param name="reserveType">The reserve type.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddResponsiblePersonReserveAssignment(ByVal reserveData As ResponsiblePersonAssignedReserveData, ByVal reserveType As ResponsiblePersonReserveDataType) As Boolean Implements ICustomerDatabaseAccess.AddResponsiblePersonReserveAssignment

      Dim success = True

      Dim sql As String

      sql = String.Format("INSERT INTO KD_ZRes{0} (KDNr, KDZNr, Bezeichnung, Result) VALUES(@customerNumber, @responsiblePersonRecNumber, @description, @result); SELECT @@IDENTITY", CType(reserveType, Integer))

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(reserveData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecNumber", ReplaceMissing(reserveData.ResponsiblePersonRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(reserveData.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("result", ReplaceMissing(reserveData.Result, DBNull.Value)))

      Dim id = ExecuteScalar(sql, listOfParams)

      If Not id Is Nothing Then
        reserveData.ID = CType(id, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a responsible person document data assignment.
    ''' </summary>
    ''' <param name="documentData">The document data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddResponsiblePersonDocumentAssignment(ByVal documentData As ResponsiblePersonAssignedDocumentData) As Boolean Implements ICustomerDatabaseAccess.AddResponsiblePersonDocumentAssignment

      Dim success = True

      Dim sql As String

      sql = "[Create New KD_ZDoc]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(documentData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@KDZnr", ReplaceMissing(documentData.ResponsiblePersonRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@DocPath", ReplaceMissing(documentData.DocPath, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Beschreibung", ReplaceMissing(documentData.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(documentData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(documentData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(documentData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(documentData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Bezeichnung", ReplaceMissing(documentData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@ScanExtension", ReplaceMissing(documentData.ScanExtension, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@UsNr", ReplaceMissing(documentData.USNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("@Categorie_Nr", ReplaceMissing(documentData.CategorieNumber, DBNull.Value)))

      Dim newIdParameter = New SqlClient.SqlParameter("@NewDocId", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
          Not recNrParameter.Value Is Nothing Then
        documentData.ID = CType(newIdParameter.Value, Integer)
        documentData.DocumentRecordNumber = CType(recNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function


    ''' <summary>
    ''' Adds a responsible person contact data assignment.
    ''' </summary>
    ''' <param name="contactData">The contact data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddResponsiblePersonContactAssignment(ByVal contactData As ResponsiblePersonAssignedContactData) As Boolean Implements ICustomerDatabaseAccess.AddResponsiblePersonContactAssignment

      Dim success = True

      Dim sql As String

			sql = "[Create New Customer Contact]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(contactData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KDZNr", ReplaceMissing(contactData.ResponsiblePersonNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontaktDate", ReplaceMissing(contactData.ContactDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Kontakte", ReplaceMissing(contactData.ContactsString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@UserName", ReplaceMissing(contactData.Username, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(contactData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(contactData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontaktType1", ReplaceMissing(contactData.ContactType1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontaktType2", ReplaceMissing(contactData.ContactType2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontaktDauer", ReplaceMissing(contactData.ContactPeriodString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontaktWichtig", ReplaceMissing(contactData.ContactImportant, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontaktErledigt", ReplaceMissing(contactData.ContactFinished, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MANr", ReplaceMissing(contactData.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(contactData.CreatedUserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ProposeNr", ReplaceMissing(contactData.ProposeNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@VakNr", ReplaceMissing(contactData.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@OfNr", ReplaceMissing(contactData.OfNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Mail_ID", ReplaceMissing(contactData.Mail_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@TaskRecNr", ReplaceMissing(contactData.TaskRecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@USNr", ReplaceMissing(contactData.UsNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ESNr", ReplaceMissing(contactData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KontaktDocID", ReplaceMissing(contactData.KontaktDocID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@EmployeeContactRecID", ReplaceMissing(contactData.EmployeeContactRecID, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewContactID", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not newIdParameter.Value Is Nothing AndAlso Not recNrParameter.Value Is Nothing Then
				contactData.ID = CType(newIdParameter.Value, Integer)
				contactData.RecordNumber = CType(recNrParameter.Value, Integer)
			Else
				success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Adds a contact document.
    ''' </summary>
    ''' <param name="contactDocument">The contact document.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddContactDocument(ByVal contactDocument As ContactDoc) As Boolean Implements ICustomerDatabaseAccess.AddContactDocument

      Dim success = True

      Dim sql As String

			sql = "INSERT INTO Kontakt_Doc (KontaktID, DocScan, FileExtension, CreatedOn, CreatedFrom, IsMA) " &
						" VALUES(@contactId, @fileBytes, @FileExtension, @createdOn, @createdFrom, 0); SELECT @@IDENTITY"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("contactId", DBNull.Value))
      listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(contactDocument.FileBytes, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileExtension", ReplaceMissing(contactDocument.FileExtension, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(contactDocument.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(contactDocument.CreatedFrom, DBNull.Value)))


      Dim id = ExecuteScalar(sql, listOfParams)

      If Not id Is Nothing Then
        contactDocument.ID = CType(id, Integer)
      Else
        success = False
      End If

      Return success

    End Function

		''' <summary>
		''' Updates customer master data.
		''' </summary>
		''' <param name="customerMasterData">The customer master data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateCustomerMasterData(ByVal customerMasterData As CustomerMasterData) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerMasterData

			Dim success = True

			Dim sql As String


			sql = "[Update Assigned Customer]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(customerMasterData.CustomerMandantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Company1", ReplaceMissing(customerMasterData.Company1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Company2", ReplaceMissing(customerMasterData.Company2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Company3", ReplaceMissing(customerMasterData.Company3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PostOfficeBox", ReplaceMissing(customerMasterData.PostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Street", ReplaceMissing(customerMasterData.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CountryCode", ReplaceMissing(customerMasterData.CountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Postcode", ReplaceMissing(customerMasterData.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Latitude", ReplaceMissing(customerMasterData.Latitude, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Longitude", ReplaceMissing(customerMasterData.Longitude, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Location", ReplaceMissing(customerMasterData.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Telephone", ReplaceMissing(customerMasterData.Telephone, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Telefax", ReplaceMissing(customerMasterData.Telefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Telefax_Mailing", ReplaceMissing(customerMasterData.Telefax_Mailing, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Homepage", ReplaceMissing(customerMasterData.Hompage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Email", ReplaceMissing(customerMasterData.EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("facebook", ReplaceMissing(customerMasterData.facebook, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("xing", ReplaceMissing(customerMasterData.xing, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KST", ReplaceMissing(customerMasterData.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FProperty", ReplaceMissing(customerMasterData.FirstProperty, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Mail_Mailing", ReplaceMissing(customerMasterData.Email_Mailing, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Language", ReplaceMissing(customerMasterData.Language, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("HowContact", ReplaceMissing(customerMasterData.HowContact, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState1", ReplaceMissing(customerMasterData.CustomerState1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerState2", ReplaceMissing(customerMasterData.CustomerState2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NoUse", ReplaceMissing(customerMasterData.NoUse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NoUseComment", ReplaceMissing(customerMasterData.NoUseComment, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Comment", ReplaceMissing(customerMasterData.Comment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Employment", ReplaceMissing(customerMasterData.Notice_Employment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Report", ReplaceMissing(customerMasterData.Notice_Report, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Invoice", ReplaceMissing(customerMasterData.Notice_Invoice, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notice_Payment", ReplaceMissing(customerMasterData.Notice_Payment, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("SalaryPerMonth", ReplaceMissing(customerMasterData.SalaryPerMonth, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SalaryPerHour", ReplaceMissing(customerMasterData.SalaryPerHour, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve1", ReplaceMissing(customerMasterData.Reserve1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve2", ReplaceMissing(customerMasterData.Reserve2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve3", ReplaceMissing(customerMasterData.Reserve3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve4", ReplaceMissing(customerMasterData.Reserve4, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreditLimit1", ReplaceMissing(customerMasterData.CreditLimit1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreditLimit2", ReplaceMissing(customerMasterData.CreditLimit2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreditLimitsFromDate", ReplaceMissing(customerMasterData.CreditLimitsFromDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreditLimitsToDate", ReplaceMissing(customerMasterData.CreditLimitsToDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ReferenceNumber", ReplaceMissing(customerMasterData.ReferenceNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_UmsMin", ReplaceMissing(customerMasterData.KD_UmsMin, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("NumberOfCopies", ReplaceMissing(customerMasterData.NumberOfCopies, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MwSt", ReplaceMissing(customerMasterData.mwstpflicht, 1)))
			listOfParams.Add(New SqlClient.SqlParameter("ValueAddedTaxNumber", ReplaceMissing(customerMasterData.ValueAddedTaxNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreditWarning", ReplaceMissing(customerMasterData.CreditWarning, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OPShipment", ReplaceMissing(customerMasterData.OPShipment, "")))
			listOfParams.Add(New SqlClient.SqlParameter("PrintNoRP", ReplaceMissing(customerMasterData.NotPrintReports, False)))

			listOfParams.Add(New SqlClient.SqlParameter("TermsAndConditions_WOS", ReplaceMissing(customerMasterData.TermsAndConditions_WOS, False)))
			listOfParams.Add(New SqlClient.SqlParameter("SendToWOS", ReplaceMissing(customerMasterData.sendToWOS, False)))
			listOfParams.Add(New SqlClient.SqlParameter("OneInvoicePerMail", ReplaceMissing(customerMasterData.OneInvoicePerMail, False)))
			listOfParams.Add(New SqlClient.SqlParameter("DoNotShowContractInWOS", ReplaceMissing(customerMasterData.DoNotShowContractInWOS, False)))

			listOfParams.Add(New SqlClient.SqlParameter("CurrencyCode", ReplaceMissing(customerMasterData.CurrencyCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BillTypeCode", ReplaceMissing(customerMasterData.BillTypeCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NumberOfEmployees", ReplaceMissing(customerMasterData.NumberOfEmployees, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CanteenAvailable", ReplaceMissing(customerMasterData.CanteenAvailable, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TransportOptions", ReplaceMissing(customerMasterData.TransportationOptions, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("InvoiceOption", ReplaceMissing(customerMasterData.InvoiceOption, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ShowHoursInNormal", ReplaceMissing(customerMasterData.ShowHoursInNormal, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedOn", ReplaceMissing(customerMasterData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(customerMasterData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(customerMasterData.ChangedUserNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerMasterData.CustomerNumber))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success

		End Function

		Function UpdateCustomerGeoData(ByVal customerMasterData As CustomerMasterData) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerGeoData
			Dim success = True

			Dim sql As String

			sql = "Update [dbo].[Kunden] Set "
			sql &= "[Latitude] = @Latitude"
			sql &= ",[Longitude] = @Longitude"

			sql &= " WHERE KDNr = @CustomerNumber "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", customerMasterData.CustomerNumber))
			listOfParams.Add(New SqlClient.SqlParameter("Latitude", ReplaceMissing(customerMasterData.Latitude, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Longitude", ReplaceMissing(customerMasterData.Longitude, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams)


			Return success

		End Function


		''' <summary>
		''' Updates responsible person master data.
		''' </summary>
		''' <param name="responsiblePeresonMasterData">The responsibler person master data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Function UpdateResponsiblePersonMasterData(ByVal responsiblePeresonMasterData As ResponsiblePersonMasterData) As Boolean Implements ICustomerDatabaseAccess.UpdateResponsiblePersonMasterData

      Dim success = True

      Dim sql As String

      sql = "UPDATE KD_Zustaendig SET "
      sql = sql & "Anrede = @salutation, "
      sql = sql & "Nachname = @lastname, "
      sql = sql & "Vorname = @firstname, "
      sql = sql & "Abteilung = @department, "
      sql = sql & "Position = @position, "
      sql = sql & "Telefon = @telephone, "
      sql = sql & "Telefax = @telefax, "
      sql = sql & "Natel = @mobilephone, "
      sql = sql & "eMail = @email, "
      sql = sql & "Facebook = @facebook, "
			sql = sql & "LinkedIn = @LinkedIn, "
			sql = sql & "Xing = @xing, "

			sql = sql & "Geb_Dat = @birthdate, "
      sql = sql & "Interessen = @interests, "
      sql = sql & "Firma1 = @company1, "
      sql = sql & "Postfach = @postofficebox, "
      sql = sql & "Strasse = @street, "
      sql = sql & "PLZ = @postcode, "
      sql = sql & "Land = @countrycode, "
      sql = sql & "Ort = @location, "
      sql = sql & "Berater = @advisor, "
      sql = sql & "ErstKontakt = @firstcontact, "
      sql = sql & "LetztKontakt = @lastcontact, "
      sql = sql & "KDZState1 = @state1, "
      sql = sql & "KDZBemerkung = @kdzbemerkung, "
      sql = sql & "KDZHowKontakt = @howcontact, "
      sql = sql & "KDZState2 = @state2, "
      sql = sql & "AnredeForm = @salutationform, "
      sql = sql & "CreatedOn = @createdon, "
      sql = sql & "CreatedFrom = @createdfrom, "
      sql = sql & "ChangedOn = @changedon, "
      sql = sql & "ChangedFrom = @changedfrom, "
      sql = sql & "Bemerkung = @comments, "
      sql = sql & "ZHD_Telefax_Mailing = @telefaxmailing, "
      sql = sql & "ZHD_SMS_Mailing = @smsmailing, "
      sql = sql & "ZHD_Mail_Mailing = @emailmailing, "
      sql = sql & "Transfered_Guid = @tranferedguid, "
      sql = sql & "AGB_WOS = @termsAndConditions "
      sql = sql & "WHERE KDNr = @customerNumber AND RecNr = @recordnumber"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("salutation", ReplaceMissing(responsiblePeresonMasterData.Salutation, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("lastname", ReplaceMissing(responsiblePeresonMasterData.Lastname, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("firstname", ReplaceMissing(responsiblePeresonMasterData.Firstname, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("department", ReplaceMissing(responsiblePeresonMasterData.Department, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("position", ReplaceMissing(responsiblePeresonMasterData.Position, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("telephone", ReplaceMissing(responsiblePeresonMasterData.Telephone, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("telefax", ReplaceMissing(responsiblePeresonMasterData.Telefax, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("mobilephone", ReplaceMissing(responsiblePeresonMasterData.MobilePhone, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("email", ReplaceMissing(responsiblePeresonMasterData.Email, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("facebook", ReplaceMissing(responsiblePeresonMasterData.Facebook, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("linkedIn", ReplaceMissing(responsiblePeresonMasterData.LinkedIn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("xing", ReplaceMissing(responsiblePeresonMasterData.Xing, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("birthdate", ReplaceMissing(responsiblePeresonMasterData.Birthdate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("interests", ReplaceMissing(responsiblePeresonMasterData.Interests, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("company1", ReplaceMissing(responsiblePeresonMasterData.Company1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("postofficebox", ReplaceMissing(responsiblePeresonMasterData.PostOfficeBox, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("street", ReplaceMissing(responsiblePeresonMasterData.Street, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("postcode", ReplaceMissing(responsiblePeresonMasterData.Postcode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("countrycode", ReplaceMissing(responsiblePeresonMasterData.CountryCode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("location", ReplaceMissing(responsiblePeresonMasterData.Location, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("advisor", ReplaceMissing(responsiblePeresonMasterData.Advisor, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("firstcontact", ReplaceMissing(responsiblePeresonMasterData.FirstContactDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("lastcontact", ReplaceMissing(responsiblePeresonMasterData.LastContactDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("state1", ReplaceMissing(responsiblePeresonMasterData.State1, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("kdzbemerkung", ReplaceMissing(responsiblePeresonMasterData.KDZComments, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("howcontact", ReplaceMissing(responsiblePeresonMasterData.KDZHowKontakt, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("state2", ReplaceMissing(responsiblePeresonMasterData.State2, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("salutationform", ReplaceMissing(responsiblePeresonMasterData.SalutationForm, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdon", ReplaceMissing(responsiblePeresonMasterData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdfrom", ReplaceMissing(responsiblePeresonMasterData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("changedon", ReplaceMissing(responsiblePeresonMasterData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("changedfrom", ReplaceMissing(responsiblePeresonMasterData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("comments", ReplaceMissing(responsiblePeresonMasterData.Comments, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("telefaxmailing", ReplaceMissing(responsiblePeresonMasterData.Telefax_Mailing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("smsmailing", ReplaceMissing(responsiblePeresonMasterData.SMS_Mailing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("emailmailing", ReplaceMissing(responsiblePeresonMasterData.Email_Mailing, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("tranferedguid", ReplaceMissing(responsiblePeresonMasterData.TransferedGuid, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("termsAndConditions", ReplaceMissing(responsiblePeresonMasterData.TermsAndConditions_WOS, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", responsiblePeresonMasterData.CustomerNumber))
      listOfParams.Add(New SqlClient.SqlParameter("recordnumber", responsiblePeresonMasterData.RecordNumber))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function


		''' <summary>
		''' Updates customer assigned invoice address data.
		''' </summary>
		''' <param name="invoiceAddress">The invoice address.</param>
		Function UpdateCustomerAssignedInvoiceAddress(ByVal invoiceAddress As CustomerAssignedInvoiceAddressData) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerAssignedInvoiceAddress
			Dim success = True

			Dim sql As String

			sql = "[UPDATE KD_RE_Address]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@ID_RE_Address", invoiceAddress.ID))

			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(invoiceAddress.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", ReplaceMissing(invoiceAddress.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REFirma", ReplaceMissing(invoiceAddress.InvoiceCompany, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REFirma2", ReplaceMissing(invoiceAddress.InvoiceCompany2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REFirma3", ReplaceMissing(invoiceAddress.InvoiceCompany3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REZhd", ReplaceMissing(invoiceAddress.InvoiceForTheAttentionOf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REPostfach", ReplaceMissing(invoiceAddress.InvoicePostOfficeBox, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REStrasse", ReplaceMissing(invoiceAddress.InvoiceStreet, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REPLZ", ReplaceMissing(invoiceAddress.InvoicePostcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@REOrt", ReplaceMissing(invoiceAddress.InvoiceLocation, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", ReplaceMissing(invoiceAddress.InvoiceEMailAddress, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SendAsZip", ReplaceMissing(invoiceAddress.InvoiceSendAsZip, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@RELand", ReplaceMissing(invoiceAddress.InvoiceCountryCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@KSTBez", ReplaceMissing(invoiceAddress.KSTDescription, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@Currency", ReplaceMissing(invoiceAddress.CurrencyCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ActiveRec", ReplaceMissing(invoiceAddress.Active, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ReAbteilung", ReplaceMissing(invoiceAddress.InvoiceDepartment, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedOn", ReplaceMissing(invoiceAddress.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@CreatedFrom", ReplaceMissing(invoiceAddress.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedOn", ReplaceMissing(invoiceAddress.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ChangedFrom", ReplaceMissing(invoiceAddress.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@ZahlKond", ReplaceMissing(invoiceAddress.PaymentCondition, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@MahnCode", ReplaceMissing(invoiceAddress.ReminderCode, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success
		End Function

		Function UpdateCustomerAssignedEMailDeliveryProperties(ByVal customerData As CustomerMasterData) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerAssignedEMailDeliveryProperties
			Dim success = True

			Dim sql As String

			sql = "[UPDATE Assigned Customer EMail Delivery Properties]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)


			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OneInvoicePerMail", ReplaceMissing(customerData.OneInvoicePerMail, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success
		End Function

		''' <summary>
		''' Updates customer assigned KST data.
		''' </summary>
		''' <param name="kstData">The KST data.</param>
		Function UpdateCustomerAssignedKST(ByVal kstData As CustomerAssignedKSTData) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerAssignedKST
			Dim success = True

			Dim sql As String

			sql = "UPDATE KD_KST SET "
			sql = sql & "RecNr = @recordNumber, "
			sql = sql & "KDNr = @customerNumber, "
			sql = sql & "ReAddressRecNr = @invoiceRecordNumber, "
			sql = sql & "Bezeichnung = @description, "
			sql = sql & "Result = @result, "
			sql = sql & "Ort_PLZ = @employmentPostcode, "
			sql = sql & "PK_PLZ = @pkPostcode, "
			sql = sql & "Res_info_1 = @info1, "
			sql = sql & "Res_info_2 = @info2 "

			sql = sql & "WHERE ID = @id "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recordNumber", ReplaceMissing(kstData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(kstData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("invoiceRecordNumber", ReplaceMissing(kstData.InvoiceAddressRecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(kstData.Description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("result", ReplaceMissing(kstData.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("employmentPostcode", ReplaceMissing(kstData.EmploymentPostCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("pkPostcode", ReplaceMissing(kstData.BKPostCode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("info1", ReplaceMissing(kstData.Info1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("info2", ReplaceMissing(kstData.Info2, DBNull.Value)))


			listOfParams.Add(New SqlClient.SqlParameter("id", kstData.ID))

			success = ExecuteNonQuery(sql, listOfParams)

			Return success
		End Function

		''' <summary>
		''' Updates responsible person assigned document data.
		''' </summary>
		''' <param name="documentData">The document data.</param>
		Public Function UpdateResponsiblePersonAssignedDocumentData(ByVal documentData As ResponsiblePersonAssignedDocumentData) As Boolean Implements ICustomerDatabaseAccess.UpdateResponsiblePersonAssignedDocumentData

      Dim success = True

      Dim sql As String

      sql = "UPDATE KD_ZDoc SET "
      sql = sql & "KDNr = @customerNumber, "
      sql = sql & "KDZNr = @responsiblePersonRecordNumber, "
      sql = sql & "DocPath = @docPath, "
      sql = sql & "Beschreibung = @description, "
      sql = sql & "RecNr = @documentRecordNumber, "
      sql = sql & "CreatedOn = @createdOn, "
      sql = sql & "CreatedFrom = @createdFrom, "
      sql = sql & "ChangedOn = @changedOn, "
      sql = sql & "ChangedFrom = @changedFrom, "
      sql = sql & "Bezeichnung = @name, "
      sql = sql & "ScanExtension = @scanExtension, "
      sql = sql & "UsNr = @usnr, "
      sql = sql & "Categorie_Nr = @categoryNumber "

      sql = sql & "WHERE ID = @id "

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(documentData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonRecordNumber", ReplaceMissing(documentData.ResponsiblePersonRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("docPath", ReplaceMissing(documentData.DocPath, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(documentData.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("documentRecordNumber", ReplaceMissing(documentData.DocumentRecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(documentData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(documentData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("changedOn", ReplaceMissing(documentData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("changedFrom", ReplaceMissing(documentData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("name", ReplaceMissing(documentData.Name, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("scanExtension", ReplaceMissing(documentData.ScanExtension, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", ReplaceMissing(documentData.USNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("categoryNumber", ReplaceMissing(documentData.CategorieNumber, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("id", documentData.ID))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Updates responsible person assigned document byte data.
    ''' </summary>
    ''' <param name="documentId">The document id.</param>
    ''' <param name="filebytes">The file bytes.</param>
    ''' <param name="fileExtension">The file extension.</param>
    ''' <returns>Boolean value indicating success.</returns>
    ''' <remarks></remarks>
    Public Function UpdateResponsiblePersonAssignedDocumentByteData(ByVal documentId As Integer, ByVal filebytes() As Byte, ByVal fileExtension As String) As Boolean Implements ICustomerDatabaseAccess.UpdateResponsiblePersonAssignedDocumentByteData

      Dim success = True

      Dim sql As String

      sql = "UPDATE KD_ZDoc SET "
      sql = sql & "DocScan = @fileBytes, "
      sql = sql & "ScanExtension = @scanExtension "

      sql = sql & "WHERE ID = @id "

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(filebytes, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("scanExtension", ReplaceMissing(fileExtension, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("id", documentId))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Updates customer assigned credit info data. 
    ''' </summary>
    ''' <param name="creditInfoData">The credit info data.</param>
    Public Function UpdateCustomerAssignedCreditInfoData(ByVal creditInfoData As CustomerAssignedCreditInfo) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerAssignedCreditInfoData

      Dim success = True

      Dim sql As String

      sql = "UPDATE KD_KreditInfo SET "
      sql = sql & "KDNr = @customerNumber, "
      sql = sql & "RecNr = @recNr, "
      sql = sql & "Ab_Date = @fromDate, "
      sql = sql & "Beschreibung = @description, "
      sql = sql & "ActiveRec = @activeRec, "
      sql = sql & "Bis_Date = @toDate, "
      sql = sql & "CreatedOn = @createdOn, "
      sql = sql & "CreatedFrom = @createdFrom, "
      sql = sql & "ChangedOn = @changedOn, "
      sql = sql & "ChangedFrom = @changedFrom, "
      sql = sql & "DV_ArchivID = @dv_archiveId, "
      sql = sql & "DV_DecisionID = @dv_decisionId, "
      sql = sql & "DV_DecisionText = @dv_decisinText, "
      sql = sql & "USNr = @usnr, "
      sql = sql & "DV_QueryArt = @dv_queryType, "
      sql = sql & "DV_FoundedAddress = @dv_foundedAddress, "
      sql = sql & "DV_FoundedAddressId = @dv_foundedAddressId "

      sql = sql & "WHERE ID = @id "

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(creditInfoData.CustomerNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("recNr", ReplaceMissing(creditInfoData.RecordNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("fromDate", ReplaceMissing(creditInfoData.FromDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("description", ReplaceMissing(creditInfoData.Description, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("activeRec", ReplaceMissing(creditInfoData.ActiveRec, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("toDate", ReplaceMissing(creditInfoData.ToDate, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(creditInfoData.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(creditInfoData.CreatedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("changedOn", ReplaceMissing(creditInfoData.ChangedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("changedFrom", ReplaceMissing(creditInfoData.ChangedFrom, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("dv_archiveId", ReplaceMissing(creditInfoData.DV_ArchiveID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("dv_decisionId", ReplaceMissing(creditInfoData.DV_DecisionID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("dv_decisinText", ReplaceMissing(creditInfoData.DV_DecisionText, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", ReplaceMissing(creditInfoData.USNr, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("dv_queryType", ReplaceMissing(creditInfoData.DV_QueryType, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("dv_foundedAddress", ReplaceMissing(creditInfoData.DV_FoundedAddress, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("dv_foundedAddressId", ReplaceMissing(creditInfoData.DV_PDFFile, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("id", creditInfoData.ID))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Updates customer assigned credit info byte data.
    ''' </summary>
    ''' <param name="creditInfoId">The credit info id.</param>
    ''' <param name="filebytes">The file bytes.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function UpdateCustomerAssignedCreditInfoByteData(ByVal creditInfoId As Integer, ByVal filebytes() As Byte) As Boolean Implements ICustomerDatabaseAccess.UpdateCustomerAssignedCreditInfoByteData

      Dim success = True

      Dim sql As String

      sql = "UPDATE KD_KreditInfo SET "
      sql = sql & "DV_PDFFile = @fileBytes "

      sql = sql & "WHERE ID = @id "

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(filebytes, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("id", creditInfoId))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success


    End Function

		''' <summary>
		''' Updates reponsible person assigned contact data.
		''' </summary>
		''' <param name="contactData">The contact data.</param>
		''' <returns>Boolean value indicating success.</returns>
		Function UpdateResponsiblePersonAssignedContactData(ByVal contactData As ResponsiblePersonAssignedContactData) As Boolean Implements ICustomerDatabaseAccess.UpdateResponsiblePersonAssignedContactData

			Dim success = True

			Dim sql As String


			'sql = "UPDATE KD_KontaktTotal SET "
			'sql = sql & "KDNr = @customerNumber, "
			'sql = sql & "KDZNr = @responsiblePersonNumber, "
			'sql = sql & "KontaktDate = @contactDate, "
			'sql = sql & "Kontakte = @contactsString, "
			'sql = sql & "UserName = @userName, "
			'sql = sql & "RecNr = @recordNumber, "
			'sql = sql & "CreatedOn = @createdOn, "
			'sql = sql & "CreatedFrom = @createdFrom, "
			'sql = sql & "ChangedOn = @changedOn, "
			'sql = sql & "ChangedFrom = @changedFrom, "
			'sql = sql & "ChangedUserNumber = @changedUserNumber, "
			'sql = sql & "KontaktType1 = @contactType1, "
			'sql = sql & "KontaktType2 = @contactType2, "
			'sql = sql & "KontaktDauer = @contactDurationString, "
			'sql = sql & "KontaktWichtig = @contactImportant, "
			'sql = sql & "KontaktErledigt = @contactFinished, "
			'sql = sql & "MANr = @maNr, "
			'sql = sql & "ProposeNr = @proposeNr, "
			'sql = sql & "VakNr = @vacanyNr, "
			'sql = sql & "OfNr = @ofNr, "
			'sql = sql & "Mail_ID = @mailId, "
			'sql = sql & "TaskRecNr = @taskRecNr, "
			'sql = sql & "USNr = @usnr, "
			'sql = sql & "EsNr = @esnr, "
			'sql = sql & "KontaktDocID = @kontaktDocID, "
			'sql = sql & "MAKontaktRecID = @EmployeeContactRecID "

			'sql = sql & "WHERE ID = @id "

			sql = "[Update Assigned Customer Contact]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(contactData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDZNr", ReplaceMissing(contactData.ResponsiblePersonNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("recordNumber", ReplaceMissing(contactData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDate", ReplaceMissing(contactData.ContactDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Kontakte", ReplaceMissing(contactData.ContactsString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserName", ReplaceMissing(contactData.Username, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktType1", ReplaceMissing(contactData.ContactType1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktType2", ReplaceMissing(contactData.ContactType2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDauer", ReplaceMissing(contactData.ContactPeriodString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktWichtig", ReplaceMissing(contactData.ContactImportant, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktErledigt", ReplaceMissing(contactData.ContactFinished, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(contactData.MANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(contactData.ProposeNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(contactData.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OfNr", ReplaceMissing(contactData.OfNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mail_ID", ReplaceMissing(contactData.Mail_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaskRecNr", ReplaceMissing(contactData.TaskRecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(contactData.UsNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(contactData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDocID", ReplaceMissing(contactData.KontaktDocID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeContactRecID", ReplaceMissing(contactData.EmployeeContactRecID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ChangedOn", ReplaceMissing(contactData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(contactData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(contactData.ChangedUserNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("id", contactData.ID))


			'listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(contactData.CustomerNumber, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("responsiblePersonNumber", ReplaceMissing(contactData.ResponsiblePersonNumber, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("contactDate", ReplaceMissing(contactData.ContactDate, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("contactsString", ReplaceMissing(contactData.ContactsString, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("userName", ReplaceMissing(contactData.Username, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("contactType1", ReplaceMissing(contactData.ContactType1, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("contactType2", ReplaceMissing(contactData.ContactType2, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("contactDurationString", ReplaceMissing(contactData.ContactPeriodString, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("contactImportant", ReplaceMissing(contactData.ContactImportant, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("contactFinished", ReplaceMissing(contactData.ContactFinished, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("maNr", ReplaceMissing(contactData.MANr, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("proposeNr", ReplaceMissing(contactData.ProposeNr, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("vacanyNr", ReplaceMissing(contactData.VacancyNumber, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("ofNr", ReplaceMissing(contactData.OfNumber, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("mailId", ReplaceMissing(contactData.Mail_ID, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("taskRecNr", ReplaceMissing(contactData.TaskRecNr, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("usnr", ReplaceMissing(contactData.UsNr, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("esnr", ReplaceMissing(contactData.ESNr, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("kontaktDocID", ReplaceMissing(contactData.KontaktDocID, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("maKontaktRecID", ReplaceMissing(contactData.EmployeeContactRecID, DBNull.Value)))

			'listOfParams.Add(New SqlClient.SqlParameter("recordNumber", ReplaceMissing(contactData.RecordNumber, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(contactData.CreatedOn, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(contactData.CreatedFrom, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("changedOn", ReplaceMissing(contactData.ChangedOn, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("changedFrom", ReplaceMissing(contactData.ChangedFrom, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("changedUserNumber", ReplaceMissing(contactData.ChangedUserNumber, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("id", contactData.ID))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

		End Function

		''' <summary>
		''' Updates contact document data.
		''' </summary>
		''' <param name="contactDocument">The contact document data.</param>
		''' <param name="ignoreFileBytes">Boolean flag indiciating if file bytes should be ignored.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateContactDocumentData(ByVal contactDocument As ContactDoc, ByVal ignoreFileBytes As Boolean) As Boolean Implements ICustomerDatabaseAccess.UpdateContactDocumentData

      Dim success = True

      Dim sql As String

      sql = "UPDATE Kontakt_Doc SET "
      sql = sql & "CreatedOn = @createdOn, "
			sql = sql & "CreatedFrom = @createdFrom, "
			sql = sql & "FileExtension = @FileExtension "

      If Not ignoreFileBytes Then
        sql = sql & ", DocScan = @fileBytes "
      End If

      sql = sql & "WHERE ID = @id"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(contactDocument.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(contactDocument.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileExtension", ReplaceMissing(contactDocument.FileExtension, DBNull.Value)))

      If Not ignoreFileBytes Then
        listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(contactDocument.FileBytes, DBNull.Value)))
      End If

      listOfParams.Add(New SqlClient.SqlParameter("id", contactDocument.ID))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function


		Public Function DeleteCustomerAddressAssignment(ByVal customernumber As Integer,
                                                    ByVal modul As String, ByVal username As String,
                                                    ByVal usnr As Integer) As DeleteCustomerAddressAssignmentResult Implements ICustomerDatabaseAccess.DeleteCustomerAddressAssignment

      Dim success = True

      Dim sql As String

      sql = "[Delete Selected Customer]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("kdnr", customernumber))
      listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
      listOfParams.Add(New SqlClient.SqlParameter("username", username))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

      Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteCustomerAddressAssignmentResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteCustomerAddressAssignmentResult)
        Catch
          resultEnum = DeleteCustomerAddressAssignmentResult.ErrorWhileDelete
        End Try
      Else
        resultEnum = DeleteCustomerAddressAssignmentResult.ErrorWhileDelete
      End If

      Return resultEnum

    End Function

    ''' <summary>
    ''' Deletes a customer business branch assignment.
    ''' </summary>
    ''' <param name="id">The database id of the business branch assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerBusinessBranchAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerBusinessBranchDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_Filiale WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a customer profession assignemnt.
    ''' </summary>
    ''' <param name="id">The database id of the profession assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerProfessionAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerProfessionDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_Berufe WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a responsible person profession assignemnt.
    ''' </summary>
    ''' <param name="id">The database id of the profession assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteResponsiblePersonProfessionDataAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteResponsiblePersonProfessionDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_ZBerufe WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a customer sector assignment.
    ''' </summary>
    ''' <param name="id">The database id of the sector assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerSectorAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerSectorDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_Branche WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a responsible person sector assignment.
    ''' </summary>
    ''' <param name="id">The database id of the sector assignment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function DeleteResponsiblePersonSectorDataAssigment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteResponsiblePersonSectorDataAssigment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_ZBranche WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success
    End Function

    ''' <summary>
    ''' Deletes a customer employment type assignment.
    ''' </summary>
    ''' <param name="id">The database id of the employment type assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerEmploymentTypeAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerEmploymentTypeDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_Anstellung WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a customer keyword assignment.
    ''' </summary>
    ''' <param name="id">The database id of the keyword assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerKeywordDataAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerKeywordDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_Stichwort WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success
    End Function

    ''' <summary>
    ''' Deletes a customer GAV group assignment.
    ''' </summary>
    ''' <param name="id">The database id of the GAV group assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerGAVGroupDataAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerGAVGroupDataAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_GAVGruppe WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a customer email assignment.
    ''' </summary>
    ''' <param name="id">The database id of the email assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerEmailAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerEmailAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_Email WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function


    ''' <summary>
    ''' Deletes a customer invoice address assignment.
    ''' </summary>
    ''' <param name="id">The database id of the responsible person.</param>
    ''' <param name="modul">The modul name the deletion is performed in.</param>
    ''' <param name="username">The username of the person which deletes the record.</param>
    ''' <param name="usnr">The USNr number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerInvoiceAddressAssignment(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As DeleteCustomerInvoiceAddressAssignmentResult Implements ICustomerDatabaseAccess.DeleteCustomerInvoiceAddressAssignment

      Dim success = True

      Dim sql As String

      sql = "[Delete KD_RE_Address]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
      listOfParams.Add(New SqlClient.SqlParameter("username", username))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

      Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteCustomerInvoiceAddressAssignmentResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteCustomerInvoiceAddressAssignmentResult)
        Catch
          resultEnum = DeleteCustomerInvoiceAddressAssignmentResult.ErrorWhileDelete
        End Try
      Else
        resultEnum = DeleteCustomerInvoiceAddressAssignmentResult.ErrorWhileDelete
      End If

      Return resultEnum

    End Function

    ''' <summary>
    ''' Deletes a customer KST assignment.
    ''' </summary>
    ''' <param name="id">The database id of KST assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerKSTAssignment(ByVal id As Integer) As DeleteCustomerKSTAssignmentResult Implements ICustomerDatabaseAccess.DeleteCustomerKSTAssignment

      Dim success = True

      Dim sql As String

      sql = "[Delete KD_KST]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))

      Dim resultParameter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
      resultParameter.Direction = ParameterDirection.Output
      listOfParams.Add(resultParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Dim resultEnum As DeleteCustomerKSTAssignmentResult

      If Not resultParameter.Value Is Nothing Then
        Try
          resultEnum = CType(resultParameter.Value, DeleteCustomerKSTAssignmentResult)
        Catch
          resultEnum = DeleteCustomerKSTAssignmentResult.ErrorWhileDelete
        End Try
      Else
        resultEnum = DeleteCustomerKSTAssignmentResult.ErrorWhileDelete
      End If

      Return resultEnum

    End Function

    ''' <summary>
    ''' Deletes a customer document assignment.
    ''' </summary>
    ''' <param name="id">The database id of the document assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerOrRespPersonDocumentAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerOrRespPersonDocumentAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM kd_ZDoc WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function


    ''' <summary>
    ''' Deletes a customer credit info assignment.
    ''' </summary>
    ''' <param name="id">The database id of the credit info assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteCustomerCreditInfoAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteCustomerCreditInfoAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_KreditInfo WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a responsible person communication data assignment.
    ''' </summary>
    ''' <param name="id">The database id of the communication data assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Function DeleteResponsiblePersonCommunicationAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteResponsiblePersonCommunicationAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_Zkomm WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a responsible person contact type assignment.
    ''' </summary>
    ''' <param name="id">The database id of the contact type assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteResponsiblePersonContactTypeAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteResponsiblePersonContactTypeAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_ZKontaktArt WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes responsible person reserve assignment.
    ''' </summary>
    ''' <param name="id">The database id of the reserve assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteResponsiblePersonReserveAssignment(ByVal id As Integer, ByVal reserveType As ResponsiblePersonReserveDataType) As Boolean Implements ICustomerDatabaseAccess.DeleteResponsiblePersonReserveAssignment

      Dim success = True

      Dim sql As String

      sql = String.Format("DELETE FROM KD_ZRes{0} WHERE ID = @id", CType(reserveType, Integer))

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function


    ''' <summary>
    ''' Deletes a responsible person contact assignment.
    ''' </summary>
    ''' <param name="id">The database id of the contact assigment.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteResponsiblePersonContactAssignment(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteResponsiblePersonContactAssignment

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM KD_KontaktTotal WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes responsible person.
    ''' </summary>
    ''' <param name="id">The database id of the responsible person.</param>
    ''' <param name="modul">The modul name the deletion is performed in.</param>
    ''' <param name="username">The username of the person which deletes the record.</param>
    ''' <param name="usnr">The USNr number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteResponsiblePerson(ByVal id As Integer, ByVal modul As String, ByVal username As String, ByVal usnr As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteResponsiblePerson

      Dim success = True

      Dim sql As String

      sql = "[Delete KD_Zustaendig]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("ID_KD_Zustaendig", id))
      listOfParams.Add(New SqlClient.SqlParameter("modul", modul))
      listOfParams.Add(New SqlClient.SqlParameter("username", username))
      listOfParams.Add(New SqlClient.SqlParameter("usnr", usnr))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a contact document.
    ''' </summary>
    ''' <param name="id">The id of the document.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteContactDocument(ByVal id As Integer) As Boolean Implements ICustomerDatabaseAccess.DeleteContactDocument
      Dim success = True

      Dim sql As String

      sql = "DELETE FROM Kontakt_Doc WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      listOfParams = New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      success = success AndAlso ExecuteNonQuery("UPDATE KD_KontaktTotal SET KontaktDocID = NULL WHERE KontaktDocID = @id", listOfParams, CommandType.Text, False)

      listOfParams = New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      success = success AndAlso ExecuteNonQuery("UPDATE MA_Kontakte SET KontaktDocID = NULL WHERE KontaktDocID = @id", listOfParams, CommandType.Text, False)

      Return success

    End Function

    ''' <summary>
    ''' Logs a solvency check usage.
    ''' </summary>
    ''' <param name="connectionString">The connection string.</param>
    ''' <param name="customerGuid">The customer guid.</param>
    ''' <param name="userGuid">The user guid.</param>
    ''' <param name="userName">The user name.</param>
    ''' <param name="solvencyCheckType">The solvency check type.</param>
    ''' <param name="serviceDate">The service date.</param>
    ''' <returns>Boolean flag indicating success.</returns>
		Public Function LogSolvencyUsage(ByVal connectionString As String, ByVal customerGuid As String, ByVal userGuid As String, ByVal userName As String, ByVal solvencyCheckType As String,
																		 ByVal jobID As String, ByVal serviceDate As DateTime) As Boolean Implements ICustomerDatabaseAccess.LogSolvencyUsage

			Dim success = True

			Dim sql As String = "INSERT INTO [Sputnik DbSelect].dbo.[tblCustomerPayableServices] ([Customer_Guid],[User_Guid],[ServiceName],[Servicedate],[CreatedOn],[CreatedFrom],[JobID]) " +
															 "VALUES (@Customer_Guid, @User_Guid, @ServiceName, @Servicedate, @CreatedOn, @CreatedFrom, @JobID)"

			' Parameters

			Dim dt As DateTime = DateTime.Now

			Try

				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlParameter("Customer_Guid", ReplaceMissing(customerGuid, DBNull.Value)))
				listOfParams.Add(New SqlParameter("User_Guid", ReplaceMissing(userGuid, DBNull.Value)))
				listOfParams.Add(New SqlParameter("ServiceName", ReplaceMissing(solvencyCheckType, DBNull.Value)))
				listOfParams.Add(New SqlParameter("Servicedate", ReplaceMissing(serviceDate, DBNull.Value)))
				listOfParams.Add(New SqlParameter("CreatedOn", ReplaceMissing(dt, DBNull.Value)))
				listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(userName, DBNull.Value)))
				listOfParams.Add(New SqlParameter("JobID", ReplaceMissing(jobID, DBNull.Value)))

				success = ExecuteNonQuery(connectionString, sql, listOfParams)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0} | {1} | {2}", connectionString, sql, ex.tostring))

			End Try

			Return success

		End Function

    ''' <summary>
    ''' Copies customer address to responsible persons.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>Boolean truth value indicating success.</returns>
    Public Function CopyCustomerAddressToResponsiblePersons(ByVal customerNumber As Integer) As Boolean Implements ICustomerDatabaseAccess.CopyCustomerAddressToResponsiblePersons

      Dim success = True

      Dim sql As String

      sql = "[Copy Customer Address To Responsible Person]"

      ' Parameters

      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      ' Data
      listOfParams.Add(New SqlClient.SqlParameter("@KDNr", customerNumber))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      Return success

    End Function



#Region "customer properties"



		''' <summary>
		''' Gets founded vacancies records for selected customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedVacanciesForCustomerMng(ByVal customerNumber As Integer?) As IEnumerable(Of CustomerVacanciesProperty) Implements ICustomerDatabaseAccess.LoadFoundedVacanciesForCustomerMng

			Dim success = True

			Dim result As List(Of CustomerVacanciesProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get VacanciesData 4 Selected Customer In MainView]"
			Else
				sql = "[Get VacanciesData 4 All Customer In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerVacanciesProperty)

					While reader.Read()
						Dim overviewData As New CustomerVacanciesProperty

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.vaknr = SafeGetInteger(reader, "vaknr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", 0)
						overviewData.kdzhdnr = SafeGetInteger(reader, "kdzhdnr", 0)

						overviewData.firma1 = SafeGetString(reader, "firma1")
						overviewData.bezeichnung = SafeGetString(reader, "bezeichnung")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")

						overviewData.kdzname = SafeGetString(reader, "kdzname")
						overviewData.advisor = SafeGetString(reader, "BeraterIn")

						overviewData.kdemail = SafeGetString(reader, "kdemail")
						overviewData.zemail = SafeGetString(reader, "zemail")

						overviewData.vakstate = SafeGetString(reader, "Vakstate")
						overviewData.vak_kanton = SafeGetString(reader, "Vak_kanton")

						overviewData.vaklink = SafeGetString(reader, "VakLink")

						overviewData.vakkontakt = SafeGetString(reader, "vakkontakt")
						overviewData.vacancygruppe = SafeGetString(reader, "vacancygruppe")

						overviewData.vacancyplz = SafeGetString(reader, "vacancyplz")
						overviewData.vacancyort = SafeGetString(reader, "vacancyort")

						overviewData.titelforsearch = SafeGetString(reader, "titelforsearch")
						overviewData.shortdescription = SafeGetString(reader, "shortdescription")

						overviewData.kdtelefon = SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = SafeGetString(reader, "kdtelefax")

						overviewData.ztelefon = SafeGetString(reader, "ztelefon")
						overviewData.ztelefax = SafeGetString(reader, "ztelefax")
						overviewData.znatel = SafeGetString(reader, "znatel")

						overviewData.ourisonline = SafeGetBoolean(reader, "ourisonline", Nothing)
						overviewData.jchisonline = SafeGetBoolean(reader, "jchisonline", Nothing)
						overviewData.ojisonline = SafeGetBoolean(reader, "ojisonline", Nothing)

						overviewData.jobchdate = SafeGetString(reader, "jobchdate")
						overviewData.ostjobchdate = SafeGetString(reader, "ostjobchdate")

						overviewData.zfiliale = SafeGetString(reader, "zfiliale")


						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded Propose records for selected customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedProposeForCustomerMng(ByVal customerNumber As Integer?) As IEnumerable(Of CustomerProposeProperty) Implements ICustomerDatabaseAccess.LoadFoundedProposeForCustomerMng

			Dim success = True

			Dim result As List(Of CustomerProposeProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get ProposeData 4 Selected KD In MainView]"
			Else
				sql = "[Get ProposeData 4 All KD In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerProposeProperty)

					While reader.Read()
						Dim overviewData As New CustomerProposeProperty

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.pnr = SafeGetInteger(reader, "ProposeNr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = SafeGetInteger(reader, "zhdnr", Nothing)
						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)

						overviewData.bezeichnung = SafeGetString(reader, "Bezeichnung")
						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.employeename = SafeGetString(reader, "maname")
						overviewData.zhdname = SafeGetString(reader, "zname")
						overviewData.p_art = SafeGetString(reader, "p_art")
						overviewData.p_state = SafeGetString(reader, "p_state")

						overviewData.advisor = SafeGetString(reader, "berater")
						overviewData.zfiliale = SafeGetString(reader, "zFiliale")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")


						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded Offer records for selected customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedOfferForCustomerMng(ByVal customerNumber As Integer?) As IEnumerable(Of CustomerOfferProperty) Implements ICustomerDatabaseAccess.LoadFoundedOfferForCustomerMng

			Dim success = True

			Dim result As List(Of CustomerOfferProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get OfferData 4 Selected Customer In MainView]"
			Else
				sql = "[Get OfferData 4 All Customer In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerOfferProperty)

					While reader.Read()
						Dim overviewData As New CustomerOfferProperty

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)

						overviewData.ofnr = SafeGetInteger(reader, "ofnr", 0)

						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.zhdnr = SafeGetInteger(reader, "kdzhdnr", Nothing)

						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)
						overviewData.employeename = SafeGetString(reader, "maname")
						overviewData.bezeichnung = SafeGetString(reader, "bezeichnung")

						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")
						overviewData.offerstate = SafeGetString(reader, "offerstate")

						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.customerstreet = SafeGetString(reader, "kdstrasse")
						overviewData.customeraddress = SafeGetString(reader, "kdadresse")
						overviewData.customeremail = SafeGetString(reader, "kdemail")
						overviewData.customertelefon = SafeGetString(reader, "kdTelefon")
						overviewData.customertelefax = SafeGetString(reader, "kdTelefax")

						overviewData.zname = SafeGetString(reader, "kdzname")
						overviewData.ztelefon = SafeGetString(reader, "zhdTelefon")
						overviewData.zmobile = SafeGetString(reader, "zhdNatel")
						overviewData.zemail = SafeGetString(reader, "zhdemail")

						overviewData.advisor = SafeGetString(reader, "Beraterin")

						overviewData.zfiliale = SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded ES records for selected customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedESForCustomerMng(ByVal customerNumber As Integer?) As IEnumerable(Of CustomerESProperty) Implements ICustomerDatabaseAccess.LoadFoundedESForCustomerMng

			Dim success = True

			Dim result As List(Of CustomerESProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get ESData 4 Selected KD In MainView]"
			Else
				sql = "[Get ESData 4 All KD In MainView]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerESProperty)

					While reader.Read()
						Dim overviewData As New CustomerESProperty

						overviewData.employeeMDNr = SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = SafeGetInteger(reader, "customermdnr", 0)

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.esnr = SafeGetInteger(reader, "esnr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", 0)
						overviewData.zhdnr = SafeGetInteger(reader, "kdzhdnr", 0)
						overviewData.manr = SafeGetInteger(reader, "manr", 0)

						overviewData.esals = SafeGetString(reader, "es_als")
						overviewData.periode = SafeGetString(reader, "periode")

						overviewData.customername = SafeGetString(reader, "Firma1")
						overviewData.employeename = SafeGetString(reader, "maname")

						overviewData.tarif = SafeGetDecimal(reader, "tarif", Nothing)
						overviewData.stundenlohn = SafeGetDecimal(reader, "stundenlohn", Nothing)
						overviewData.margemitbvg = SafeGetDecimal(reader, "MargeMitBVG", Nothing)
						overviewData.margeohnebvg = SafeGetDecimal(reader, "bruttomarge", Nothing)

						overviewData.CustomerTagesSpesen = SafeGetDecimal(reader, "KDTSpesen", Nothing)
						overviewData.EmployeeTagesSpesen = SafeGetDecimal(reader, "MATSpesen", Nothing)
						overviewData.EmployeeStundenSpesen = SafeGetDecimal(reader, "MAStdSpesen", Nothing)

						overviewData.actives = SafeGetBoolean(reader, "actives", False)

						overviewData.zfiliale = SafeGetString(reader, "zFiliale")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded reports records for selected customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedRPForCustomerMng(ByVal customerNumber As Integer?) As IEnumerable(Of CustomerReportsProperty) Implements ICustomerDatabaseAccess.LoadFoundedRPForCustomerMng

			Dim success = True

			Dim result As List(Of CustomerReportsProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get RPData 4 Selected KD In MainView]"
			Else
				sql = "[Get RPData 4 All KD In MainView]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerReportsProperty)

					While reader.Read()
						Dim overviewData As New CustomerReportsProperty

						overviewData.employeeMDNr = SafeGetInteger(reader, "employeemdnr", 0)
						overviewData.customerMDNr = SafeGetInteger(reader, "customermdnr", 0)

						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)
						overviewData.rpnr = SafeGetInteger(reader, "rpnr", Nothing)

						overviewData.manr = SafeGetInteger(reader, "manr", Nothing)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", Nothing)
						overviewData.esnr = SafeGetInteger(reader, "esnr", Nothing)
						overviewData.lonr = SafeGetInteger(reader, "lonr", Nothing)

						overviewData.monat = SafeGetInteger(reader, "monat", Nothing)
						overviewData.jahr = SafeGetInteger(reader, "jahr", Nothing)
						overviewData.rpdone = SafeGetBoolean(reader, "erfasst", Nothing)

						overviewData.employeename = SafeGetString(reader, "maname")
						overviewData.customername = SafeGetString(reader, "firma1")
						overviewData.rpgav_beruf = SafeGetString(reader, "RPGAV_Beruf")

						overviewData.periode = SafeGetString(reader, "periode")

						overviewData.zfiliale = SafeGetString(reader, "zFiliale")
						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function


		''' <summary>
		''' Gets founded Invoice records for selected customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedInvoiceForCustomerMng(ByVal customerNumber As Integer?, ByVal justOpenInvoices As Boolean?) As IEnumerable(Of CustomerInvoiceProperty) Implements ICustomerDatabaseAccess.LoadFoundedInvoiceForCustomerMng

			Dim success = True

			Dim result As List(Of CustomerInvoiceProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				If justOpenInvoices.hasvalue AndAlso justOpenInvoices Then
					sql = "[Get Open REData 4 Selected KD In MainView]"
				Else
					sql = "[Get REData 4 Selected KD In MainView]"
				End If

			Else
				sql = "[Get REData 4 All Kd In MainView]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerInvoiceProperty)

					While reader.Read()
						Dim overviewData As New CustomerInvoiceProperty

						overviewData.customerMDNr = SafeGetInteger(reader, "customerMDNr", 0)
						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)

						overviewData.renr = SafeGetInteger(reader, "renr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", 0)

						overviewData.firma1 = SafeGetString(reader, "firma1")

						overviewData.zhd = SafeGetString(reader, "zhd")
						overviewData.plzort = String.Format("{0} {1}", SafeGetString(reader, "plz"), SafeGetString(reader, "ort"))

						overviewData.fbmonth = SafeGetInteger(reader, "fakmonth", 0)

						overviewData.fakdate = SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = SafeGetString(reader, "einstufung")
						overviewData.branche = SafeGetString(reader, "branche")

						overviewData.betragink = SafeGetDecimal(reader, "betragink", 0)
						overviewData.betragex = SafeGetDecimal(reader, "betragex", 0)
						overviewData.bezahlt = SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.mwstproz = SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragmwst = SafeGetDecimal(reader, "betragmwst", 0)
						overviewData.betragopen = SafeGetDecimal(reader, "betragink", 0) - SafeGetDecimal(reader, "betragbezahlt", 0)
						overviewData.isopen = overviewData.betragopen <> 0

						overviewData.rekst1 = SafeGetString(reader, "rekst1")
						overviewData.rekst2 = SafeGetString(reader, "rekst2")
						overviewData.rekst = SafeGetString(reader, "rekst")

						overviewData.reart1 = SafeGetString(reader, "reart1")
						overviewData.reart2 = SafeGetString(reader, "reart2")
						overviewData.zahlkond = SafeGetString(reader, "zahlungskondition")

						overviewData.employeeadvisor = SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = SafeGetString(reader, "customeradvisor")

						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		''' <summary>
		''' Gets founded Payment records for selected customer.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function LoadFoundedPaymentForCustomerMng(ByVal customerNumber As Integer?) As IEnumerable(Of CustomerPaymentProperty) Implements ICustomerDatabaseAccess.LoadFoundedPaymentForCustomerMng

			Dim success = True

			Dim result As List(Of CustomerPaymentProperty) = Nothing

			Dim sql As String
			If customerNumber.HasValue Then
				sql = "[Get ZEData 4 Selected KD In MainView]"
			Else
				sql = "[Get ZEData 4 All KD In MainView]"
			End If

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@KDNr", ReplaceMissing(customerNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerPaymentProperty)

					While reader.Read()
						Dim overviewData As New CustomerPaymentProperty

						overviewData.customerMDNr = SafeGetInteger(reader, "customermdnr", 0)
						overviewData.mdnr = SafeGetInteger(reader, "mdnr", 0)

						overviewData.zenr = SafeGetInteger(reader, "zenr", 0)
						overviewData.renr = SafeGetInteger(reader, "renr", 0)
						overviewData.kdnr = SafeGetInteger(reader, "kdnr", 0)

						overviewData.firma1 = SafeGetString(reader, "firma1")
						overviewData.firma2 = SafeGetString(reader, "firma2")
						overviewData.firma3 = SafeGetString(reader, "firma3")
						overviewData.abteilung = SafeGetString(reader, "abteilung")

						overviewData.zhd = SafeGetString(reader, "zhd")
						overviewData.postfach = SafeGetString(reader, "postfach")
						overviewData.strasse = SafeGetString(reader, "strasse")
						overviewData.plz = SafeGetString(reader, "ort")
						overviewData.ort = SafeGetString(reader, "plz")
						overviewData.plzort = String.Format("{0} {1}", SafeGetString(reader, "plz"), SafeGetString(reader, "ort"))


						overviewData.valutadate = SafeGetDateTime(reader, "valutadate", Nothing)
						overviewData.buchungdate = SafeGetDateTime(reader, "buchungsdate", Nothing)
						overviewData.fakdate = SafeGetDateTime(reader, "fakdate", Nothing)
						overviewData.faelligdate = SafeGetDateTime(reader, "faelligdate", Nothing)

						overviewData.einstufung = SafeGetString(reader, "einstufung")
						overviewData.branche = SafeGetString(reader, "branche")

						overviewData.betragink = SafeGetDecimal(reader, "betragink", 0)
						overviewData.zebetrag = SafeGetDecimal(reader, "zebetrag", 0)

						overviewData.mwstproz = SafeGetDecimal(reader, "mwstproz", 0)
						overviewData.betragopen = SafeGetDecimal(reader, "betragink", 0) - SafeGetDecimal(reader, "betragbezahlt", 0)

						overviewData.rekst1 = SafeGetString(reader, "rekst1")
						overviewData.rekst2 = SafeGetString(reader, "rekst2")
						overviewData.rekst = SafeGetString(reader, "rekst")

						overviewData.reart1 = SafeGetString(reader, "reart1")
						overviewData.reart2 = SafeGetString(reader, "reart2")


						overviewData.kdtelefon = SafeGetString(reader, "kdtelefon")
						overviewData.kdtelefax = SafeGetString(reader, "kdtelefax")
						overviewData.kdemail = SafeGetString(reader, "kdemail")

						overviewData.employeeadvisor = SafeGetString(reader, "employeeadvisor")
						overviewData.customeradvisor = SafeGetString(reader, "customeradvisor")

						overviewData.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						overviewData.createdfrom = SafeGetString(reader, "createdfrom")
						overviewData.zfiliale = SafeGetString(reader, "zfiliale")

						result.Add(overviewData)

					End While

					reader.Close()

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function



#End Region



#Region "Propose"

		''' <summary>
		''' Loads customer contact total info by search criteria for Propose.
		''' </summary>
		''' <param name="customerNumber">The contact number.</param>
		''' <returns>List of customer contact total data or nothing in error case.</returns>
		Public Function LoadCustomerContactTotalDataForPropose(ByVal customerNumber As Integer, ByVal proposeNumber As Integer,
																																 ByVal bHideTel As Boolean, ByVal bHideOffer As Boolean, ByVal bHideMail As Boolean,
																																 ByVal bHideSMS As Boolean,
																																 ByVal years As Integer()) As IEnumerable(Of CustomerContactOverviewData) Implements ICustomerDatabaseAccess.LoadCustomerContactTotalDataForPropose

			Dim result As List(Of CustomerContactOverviewData) = Nothing

			Dim sql As String

			sql = "SELECT KDK.ID, KDK.KDNr, KDK.KDZNr, KDK.RecNr, KDK.KontaktDate, KDK.Kontakte, KDK.KontaktDauer, KDK.KontaktWichtig, KDK.KontaktErledigt, KDK.KontaktDocID, "
			sql &= "KDK.CreatedFrom, " ', (Select Top 1 US.KST From Benutzer US Where KDK.CreatedFrom = US.Vorname + ' ' + US.Nachname) As KST  "
			sql &= "(z.Nachname + ', ' + IsNull(z.Vorname, '')) As ZFullname, "
			sql &= "(Select Top 1 min(KontaktDate) As MinKontaktDate FROM KD_KontaktTotal WHERE KDNr = @customerNumber) As MinKontaktDate,  "
			'sql &= "AND (@responsiblePersonRecordNumber IS NULL OR KD_KontaktTotal.KDZNr = @responsiblePersonRecordNumber Or 0 = @responsiblePersonRecordNumber)) As MinKontaktDate, "
			sql &= "(Select Top 1 max(KontaktDate) As MaxKontaktDate FROM KD_KontaktTotal WHERE KDNr = @customerNumber) As MaxKontaktDate "
			'sql &= "AND (@responsiblePersonRecordNumber IS NULL OR KD_KontaktTotal.KDZNr = @responsiblePersonRecordNumber Or 0 = @responsiblePersonRecordNumber)) As MaxKontaktDate "

			sql &= "FROM KD_KontaktTotal KDK "
			sql &= "Left Join KD_Zustaendig z On KDK.KDZNr = z.RecNr And KDK.KDNr = z.KDNr "
			sql &= "WHERE KDK.KDNr = @customerNumber " 'AND (@responsiblePersonRecordNumber IS NULL OR KDK.KDZNr = @responsiblePersonRecordNumber Or 0 = @responsiblePersonRecordNumber) "
			sql &= "And KDK.ProposeNr = @proposeNumber "

			If Not bHideTel Then sql &= "And KDK.Kontakte Not Like '%telefoniert" & "%' "
			If Not bHideOffer Then sql &= "And KDK.Kontakte Not Like '%Offerte geschickt" & "%' "
			If Not bHideMail Then sql &= "And KDK.Kontakte Not Like '%Mail-Nachricht gesendet" & "%' "
			If Not bHideSMS Then sql &= "And KDK.Kontakte Not Like '%SMS-Nachricht gesendet" & "%' "

			If Not years Is Nothing AndAlso years.Count > 0 Then
				sql = sql & "And year(KDK.KontaktDate) IN ("
				sql = sql & String.Join(", ", years)
				sql = sql & ") "
			End If
			sql = sql & " ORDER BY KDK.KontaktDate DESC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@customerNumber", ReplaceMissing(customerNumber, DBNull.Value)))
			'listOfParams.Add(New SqlClient.SqlParameter("@responsiblePersonRecordNumber", ReplaceMissing(responsiblePersonRecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@proposeNumber", ReplaceMissing(proposeNumber, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of CustomerContactOverviewData)

					While reader.Read()
						Dim searchData As New CustomerContactOverviewData
						searchData.ID = SafeGetInteger(reader, "ID", 0)
						searchData.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
						searchData.ResponsiblePersonRecordNumber = SafeGetInteger(reader, "KDZNr", Nothing)
						searchData.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
						searchData.ContactDate = SafeGetDateTime(reader, "KontaktDate", Nothing)
						searchData.PersonOrSubject = String.Format("({0}) {1}", SafeGetString(reader, "ZFullname"), SafeGetString(reader, "KontaktDauer"))
						searchData.Description = SafeGetString(reader, "Kontakte")
						searchData.IsImportant = SafeGetBoolean(reader, "KontaktWichtig", False)
						searchData.IsCompleted = SafeGetBoolean(reader, "KontaktErledigt", False)
						searchData.Creator = SafeGetString(reader, "CreatedFrom")
						searchData.DocumentID = SafeGetInteger(reader, "KontaktDocID", Nothing)

						searchData.minContactDate = SafeGetDateTime(reader, "MinKontaktDate", Nothing)
						searchData.maxContactDate = SafeGetDateTime(reader, "MaxKontaktDate", Nothing)

						result.Add(searchData)

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

#End Region

#End Region




#Region "WOS Export"

		Function LoadCustomerDataForWOSExport(ByVal userNumber As Integer?, ByVal customerNumber As Integer?, ByVal cresponsibleNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal invoiceNumber As Integer?) As CustomerWOSData Implements ICustomerDatabaseAccess.LoadCustomerDataForWOSExport

			Dim success = True

			Dim result As CustomerWOSData = Nothing

			Dim sql As String
			sql = "[Get CustomerData For Transfer into WOS]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", ReplaceMissing(userNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDNr", ReplaceMissing(cresponsibleNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(employmentNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(eslohnNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New CustomerWOSData

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim overviewData As New CustomerWOSData

					overviewData.CustomerNumber = customerNumber
					overviewData.CresponsibleNumber = cresponsibleNumber
					overviewData.EmploymentNumber = employmentNumber
					overviewData.EmploymentLineNumber = eslohnNumber
					overviewData.ReportNumber = reportNumber
					overviewData.InvoiceNumber = invoiceNumber
					overviewData.AssignedDocumentGuid = SafeGetString(reader, "Doc_Guid")

					overviewData.KDTransferedGuid = SafeGetString(reader, "KD_Guid")
					overviewData.ZHDTransferedGuid = SafeGetString(reader, "ZHD_Guid")
					overviewData.ESDoc_Guid = SafeGetString(reader, "VerleihDoc_Guid")
					overviewData.RPDoc_Guid = SafeGetString(reader, "RPDoc_Guid")
					overviewData.REDoc_Guid = SafeGetString(reader, "REDoc_Guid")
					overviewData.CustomerMail = SafeGetString(reader, "MDeMail")
					overviewData.customername = SafeGetString(reader, "MDName")
					overviewData.CustomerStrasse = SafeGetString(reader, "MDStrasse")
					overviewData.CustomerOrt = SafeGetString(reader, "MDOrt")
					overviewData.CustomerPLZ = SafeGetString(reader, "MDPlz")
					overviewData.CustomerTelefon = SafeGetString(reader, "MDTelefon")
					overviewData.CustomerTelefax = SafeGetString(reader, "MDTelefax")
					overviewData.CustomerHomepage = SafeGetString(reader, "MDHomePage")

					overviewData.UserAnrede = SafeGetString(reader, "USAnrede")
					overviewData.UserVorname = SafeGetString(reader, "USVorname")
					overviewData.UserName = SafeGetString(reader, "USNachname")
					overviewData.UserTelefon = SafeGetString(reader, "USTelefon")
					overviewData.UserTelefax = SafeGetString(reader, "USTelefax")
					overviewData.UserMail = SafeGetString(reader, "USeMail")
					overviewData.LogedUserID = SafeGetString(reader, "LogedUser_Guid")

					overviewData.MDTelefon = SafeGetString(reader, "MDTelefon")
					overviewData.MD_DTelefon = SafeGetString(reader, "MDDTelefon")
					overviewData.MDTelefax = SafeGetString(reader, "MDTelefax")
					overviewData.MDMail = SafeGetString(reader, "MDeMail")

					overviewData.KD_Name = SafeGetString(reader, "KD_Name")
					overviewData.KD_Postfach = SafeGetString(reader, "KD_Postfach")
					overviewData.KD_Strasse = SafeGetString(reader, "KD_Strasse")
					overviewData.KD_PLZ = SafeGetString(reader, "KD_PLZ")
					overviewData.KD_Ort = SafeGetString(reader, "KD_Ort")
					overviewData.KD_Land = SafeGetString(reader, "KD_Land")
					overviewData.KD_Filiale = SafeGetString(reader, "KD_Filiale")
					overviewData.KD_Berater = SafeGetString(reader, "KD_Berater")
					overviewData.KD_Email = SafeGetString(reader, "KD_Email")
					overviewData.KD_AGB_Wos = SafeGetString(reader, "KD_AGB_Wos")
					overviewData.KD_Beruf = SafeGetString(reader, "KD_Beruf")
					overviewData.KD_Branche = SafeGetString(reader, "KD_Branche")
					overviewData.KD_Language = SafeGetString(reader, "KD_Language")
					overviewData.DoNotShowContractInWOS = SafeGetBoolean(reader, "DoNotShowContractInWOS", Nothing)

					overviewData.ZHD_Vorname = SafeGetString(reader, "ZHD_Vorname")
					overviewData.ZHD_Nachname = SafeGetString(reader, "ZHD_Nachname")
					overviewData.ZHD_EMail = SafeGetString(reader, "ZHD_EMail")
					overviewData.ZHDSex = SafeGetString(reader, "ZHDSex")
					overviewData.Zhd_BriefAnrede = SafeGetString(reader, "Zhd_BriefAnrede")
					overviewData.Zhd_Berater = SafeGetString(reader, "Zhd_Berater")
					overviewData.Zhd_Beruf = SafeGetString(reader, "Zhd_Beruf")
					overviewData.Zhd_Branche = SafeGetString(reader, "Zhd_Branche")
					overviewData.ZHD_AGB_Wos = SafeGetString(reader, "ZHD_AGB_Wos")
					overviewData.ZHD_GebDat = SafeGetDateTime(reader, "ZHD_GebDat", Nothing)

					overviewData.UserInitial = SafeGetString(reader, "User_Initial")
					overviewData.UserSex = SafeGetString(reader, "User_Sex")
					overviewData.UserFiliale = SafeGetString(reader, "User_Filiale")
					overviewData.UserSign = SafeGetByteArray(reader, "User_Sign")
					overviewData.UserPicture = SafeGetByteArray(reader, "User_Picture")

					result = overviewData

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.StackTrace)

			Finally
				CloseReader(reader)

			End Try

			Return result

		End Function

		Public Function LoadCustomerDataForCustomerWOSExport(ByVal userNumber As Integer?, ByVal customerNumber As Integer?, ByVal cresponsibleNumber As Integer?, ByVal employmentNumber As Integer?, ByVal eslohnNumber As Integer?, ByVal reportNumber As Integer?, ByVal invoiceNumber As Integer?) As DataTable Implements ICustomerDatabaseAccess.LoadCustomerDataForCustomerWOSExport

			Dim sql As String
			sql = "[Get CustomerData For Transfer into WOS]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", ReplaceMissing(userNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDNr", ReplaceMissing(cresponsibleNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(employmentNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(eslohnNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(reportNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(invoiceNumber, 0)))

			Dim dataTable = FillDataTable(sql, listOfParams, CommandType.StoredProcedure)


			Return dataTable

		End Function

		Public Function CacheCustomerWOSDataTemporary(ByVal customerData As CustomerWOSData) As Boolean Implements ICustomerDatabaseAccess.CacheCustomerWOSDataTemporary

			Dim success = True

			Dim sql As String

			sql = "Delete [Sputnik DbSelect].Dbo.Kunden_Doc_Online Where Doc_Guid = @Doc_Guid; "

			sql &= "Insert Into [Sputnik DbSelect].Dbo.Kunden_Doc_Online ("
			sql &= "KDNr"
			sql &= ",ZHDNr"
			sql &= ",ESNr"
			sql &= ",ESLohnNr"
			sql &= ",RPNr"
			sql &= ",RENr"
			sql &= ",LogedUser_ID"
			sql &= ",Customer_ID"
			sql &= ",KD_Name"
			sql &= ",KD_Filiale"
			sql &= ",KD_Kanton"
			sql &= ",KD_Ort"
			sql &= ",KD_eMail"
			sql &= ",KD_Beruf"
			sql &= ",KD_Branche"
			sql &= ",KD_Berater"
			sql &= ",KD_AGB_WOS"
			sql &= ",ZHD_Vorname"
			sql &= ",ZHD_Nachname"
			sql &= ",ZHDSex"
			sql &= ",ZHD_BriefAnrede"
			sql &= ",ZHD_eMail"
			sql &= ",ZHD_Beruf"
			sql &= ",ZHD_Branche"
			sql &= ",ZHD_GebDat"
			sql &= ",KD_Language"
			sql &= ",KD_Guid"
			sql &= ",ZHD_Guid"
			sql &= ",ZHD_Berater"
			sql &= ",ZHD_AGB_WOS"
			sql &= ",Doc_Guid"
			sql &= ",Doc_Art"
			sql &= ",Doc_Info"
			sql &= ",Transfered_User"
			sql &= ",Transfered_On"
			sql &= ",User_Nachname"
			sql &= ",User_Vorname"
			sql &= ",User_Telefon"
			sql &= ",User_Telefax"
			sql &= ",User_eMail"
			sql &= ",DocFilename"
			sql &= ",DocScan"

			sql &= " ) "
			sql &= " Values"
			sql &= " ("

			sql &= "@KDNr"
			sql &= ",@ZHDNr"
			sql &= ",@ESNr"
			sql &= ",@ESLohnNr"
			sql &= ",@RPNr"
			sql &= ",@RENr"
			sql &= ",@LogedUserID"
			sql &= ",@Customer_ID"
			sql &= ",@KD_Name"
			sql &= ",@KD_Filiale"
			sql &= ",@KD_Kanton"
			sql &= ",@KD_Ort"
			sql &= ",@KD_eMail"
			sql &= ",@KD_Beruf"
			sql &= ",@KD_Branche"
			sql &= ",@KD_Berater"
			sql &= ",@KD_AGB_WOS"
			sql &= ",@ZHD_Vorname"
			sql &= ",@ZHD_Nachname"
			sql &= ",@ZHDSex"
			sql &= ",@ZHD_BriefAnrede"
			sql &= ",@ZHD_eMail"
			sql &= ",@ZHD_Beruf"
			sql &= ",@ZHD_Branche"
			sql &= ",@ZHD_GebDat"
			sql &= ",@KD_Language"
			sql &= ",@KD_Guid"
			sql &= ",@ZHD_Guid"
			sql &= ",@ZHD_Berater"
			sql &= ",@ZHD_AGB_WOS"
			sql &= ",@Doc_Guid"
			sql &= ",@Doc_Art"
			sql &= ",@Doc_Info"
			sql &= ",@Transfered_User"
			sql &= ",GetDate()"
			sql &= ",@User_Nachname"
			sql &= ",@User_Vorname"
			sql &= ",@User_Telefon"
			sql &= ",@User_Telefax"
			sql &= ",@User_eMail"
			sql &= ",@DocFilename"
			sql &= ",@DocScan"

			sql &= ")"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDNr", ReplaceMissing(customerData.CresponsibleNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(customerData.EmploymentNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(customerData.EmploymentLineNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(customerData.ReportNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(customerData.InvoiceNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LogedUserID", ReplaceMissing(customerData.LogedUserID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerData.CustomerWOSID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Name", ReplaceMissing(customerData.KD_Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Filiale", ReplaceMissing(customerData.KD_Filiale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Kanton", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Ort", ReplaceMissing(customerData.KD_Ort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_eMail", ReplaceMissing(customerData.KD_Email, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Beruf", ReplaceMissing(customerData.KD_Beruf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Branche", ReplaceMissing(customerData.KD_Branche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Berater", ReplaceMissing(customerData.KD_Berater, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_AGB_WOS", ReplaceMissing(customerData.KD_AGB_Wos, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Vorname", ReplaceMissing(customerData.ZHD_Vorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Nachname", ReplaceMissing(customerData.ZHD_Nachname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDSex", ReplaceMissing(customerData.ZHDSex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_BriefAnrede", ReplaceMissing(customerData.Zhd_BriefAnrede, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_eMail", ReplaceMissing(customerData.ZHD_EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Beruf", ReplaceMissing(customerData.Zhd_Beruf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Branche", ReplaceMissing(customerData.Zhd_Branche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_GebDat", ReplaceMissing(customerData.ZHD_GebDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Language", "Deutsch"))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Guid", ReplaceMissing(customerData.KDTransferedGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Guid", ReplaceMissing(customerData.ZHDTransferedGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Berater", ReplaceMissing(customerData.Zhd_Berater, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_AGB_WOS", ReplaceMissing(customerData.ZHD_AGB_Wos, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Guid", ReplaceMissing(customerData.AssignedDocumentGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Art", ReplaceMissing(customerData.AssignedDocumentArtName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Info", ReplaceMissing(customerData.AssignedDocumentInfo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Transfered_User", ReplaceMissing(customerData.LogedUserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Nachname", ReplaceMissing(customerData.UserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Vorname", ReplaceMissing(customerData.UserVorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Telefon", ReplaceMissing(customerData.UserTelefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Telefax", ReplaceMissing(customerData.UserTelefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_eMail", ReplaceMissing(customerData.UserMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocFilename", ReplaceMissing(customerData.ScanDocName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(customerData.ScanDoc, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams)

			Return success

		End Function


#End Region



  End Class


End Namespace

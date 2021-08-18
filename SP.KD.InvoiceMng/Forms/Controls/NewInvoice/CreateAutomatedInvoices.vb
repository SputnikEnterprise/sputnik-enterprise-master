
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports System.ComponentModel
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.DateAndTimeCalculation


Namespace UI


	Public Class CreateAutomatedInvoices


#Region "private fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		Private m_dateUtility As DateAndTimeUtily

		''' <summary>
		''' The mandant.
		''' </summary>
		''' <remarks></remarks>
		Private m_Mandant As Mandant

		Private m_path As ClsProgPath

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()


		'Private m_Debitorenart As List(Of DebitorenAutomatedArt)
		'Private m_BankData As List(Of SP.DatabaseAccess.Invoice.DataObjects.BankData)
		Private m_MahnCodeData As List(Of DatabaseAccess.Customer.DataObjects.PaymentReminderCodeData)

		Private m_InvoiceDateAsEndOfReportDate As Boolean
		Private m_InvoiceDueDateFromCreatedDate As Boolean
		Private m_InvoiceNumberOffsetFromSettings As Integer
		Private m_MwStNr As String
		Private m_Currency As String
		Private m_MwStAnsatz As Decimal
		Private m_ReferenceNumbersTo10Setting As Boolean
		Private m_SelectedBank As BankData

		''' <summary>
		''' The posting accounts.
		''' </summary>
		Private m_PostingAccounts As New Dictionary(Of Integer, String)

#End Region


#Region "public properties"

		''' <summary>
		''' The invoice data access object.
		''' </summary>
		Public Property m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess

		''' <summary>
		''' The customer database access.
		''' </summary>
		Public Property m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		Public Property m_Debitorenart As List(Of DebitorenAutomatedArt)
		Public Property m_BankData As List(Of SP.DatabaseAccess.Invoice.DataObjects.BankData)
		Public Property IsSelected As Boolean
		Public Property SelectedBankNumber As Integer?

		Public Property m_MandantNumber As Integer


#End Region


#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Mandantendaten
			m_Mandant = New Mandant
			m_path = New ClsProgPath

			m_UtilityUI = New UtilityUI
			m_Utility = New Utility
			m_dateUtility = New DateAndTimeUtily


			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		End Sub

#End Region


#Region "Private Properties"

		''' <summary>
		''' Gets the currency setting.
		''' </summary>
		Private ReadOnly Property CurrencySetting As String
			Get

				Dim mdNumber = m_MandantNumber

				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

				Dim currencyvalue As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/currencyvalue", FORM_XML_MAIN_KEY))

				Return currencyvalue

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt number setting.
		''' </summary>
		Private ReadOnly Property MwStNrSetting(ByVal mdYear As Integer) As String
			Get
				Dim mdNumber = m_MandantNumber

				If mdYear = 0 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstNumber As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/mwstnr", FORM_XML_MAIN_KEY))

				Return mwstNumber

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt Ansatz setting.
		''' </summary>
		Private ReadOnly Property MwStAnsatz(ByVal mdYear As Integer) As Decimal
			Get

				Dim mdNumber = m_MandantNumber

				If mdYear = 0 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)

			End Get

		End Property

		''' <summary>
		''' Gets the reference number to 10 setting.
		''' </summary>
		Private ReadOnly Property ReferenceNumbersTo10Setting(ByVal mdYear As Integer) As Boolean
			Get

				Dim mdNumber = m_MandantNumber

				If mdYear = 0 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim ref10forfactoring As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/ref10forfactoring", FORM_XML_MAIN_KEY)), False)

				Return ref10forfactoring.HasValue AndAlso ref10forfactoring

			End Get

		End Property

		''' <summary>
		''' set fak_dat for old reports as end of report month setting.
		''' </summary>
		Private ReadOnly Property InvoiceDateAsEndOfMonth(ByVal mdYear As Integer) As Boolean
			Get

				Dim mdNumber = m_MandantNumber

				If mdYear = 0 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim setDateToEndofReportMonth As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/setfakdatetoendofreportmonth", FORM_XML_MAIN_KEY)), False)

				Return setDateToEndofReportMonth.HasValue AndAlso setDateToEndofReportMonth

			End Get

		End Property

		''' <summary>
		''' set duedate from now or fak_dat setting.
		''' </summary>
		Private ReadOnly Property InvoiceDueDateFromCreatedDate(ByVal mdYear As Integer) As Boolean
			Get

				Dim mdNumber = m_MandantNumber

				If mdYear = 0 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim setDueDateFromCreatedOn As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/calculateduedatefromcreatedon", FORM_XML_MAIN_KEY)), False)

				Return setDueDateFromCreatedOn.GetValueOrDefault(False)

			End Get

		End Property

#End Region


#Region "private methodes"

		''' <summary>
		''' Reads the invoice offset from the settings.
		''' </summary>
		''' <returns>Invoice offset or zero if it could not be read.</returns>
		Private Function ReadInvoiceOffsetFromSettings() As Integer

			Dim strQuery As String = "//StartNr/Fakturen"
			'Dim r = m_ClsProgSetting.GetUserProfileFile
			Dim invoiceNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
			Dim intVal As Integer

			If Integer.TryParse(invoiceNumberStartNumberSetting, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Function

		''' <summary>
		''' Loads the Posting Accounts from XML
		''' </summary>
		Private Sub LoadPostingAccounts(ByVal mdYear As Integer)

			Dim mdNumber = m_MandantNumber
			If mdYear = 0 Then mdYear = Now.Year

			m_PostingAccounts = New Dictionary(Of Integer, String)
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

			Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/BuchungsKonten", mdNumber)
			For i As Integer = 1 To 38
				Dim strValue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(
																												 m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear),
																												 String.Format("{0}/_{1}", FORM_XML_MAIN_KEY, i)), "0")
				m_PostingAccounts.Add(i, strValue)
			Next
		End Sub


#End Region


#Region "public methodes"

		Public Sub InitialData()

			m_InvoiceDateAsEndOfReportDate = InvoiceDateAsEndOfMonth(Now.Year)
			m_InvoiceDueDateFromCreatedDate = InvoiceDueDateFromCreatedDate(Now.Year)
			m_InvoiceNumberOffsetFromSettings = ReadInvoiceOffsetFromSettings()
			m_MwStNr = MwStNrSetting(Now.Year)
			m_Currency = CurrencySetting
			m_MwStAnsatz = MwStAnsatz(Now.Year)
			m_ReferenceNumbersTo10Setting = ReferenceNumbersTo10Setting(Now.Year)

			' loads fibukonten
			LoadPostingAccounts(Now.Year)
			If SelectedBankNumber.GetValueOrDefault(0) = 0 Then
				m_SelectedBank = (From b In m_BankData Where b.AsStandard = True).FirstOrDefault
			Else
				m_SelectedBank = (From b In m_BankData Where b.ID = SelectedBankNumber).FirstOrDefault
			End If
			If m_SelectedBank Is Nothing Then
				m_SelectedBank = (From b In m_BankData Order By b.RecNr).FirstOrDefault
				m_Logger.LogWarning("Keine Standard-Bankverbindung!")
				If m_SelectedBank Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Standard-Bankverbindung ist vorhanden! Bitte definieren Sie eine Standard-Bankverbindung in der Mandantenverwaltung."))
				End If
			End If

		End Sub


		''' <summary>
		''' invoicetype "R"
		''' </summary>
		Public Function BuildNewReportInvoiceData(ByVal rpData As ReportOverviewAutomatedInvoiceData, ByVal customerNumber As Integer) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData As ReportLineCreatingAutomatedInvoiceData
			rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithReportNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, rpData.RPNr)

			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, rpData.KstNr, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "E"
		''' </summary>
		Public Function BuildNewEmploymentInvoiceData(ByVal rpData As ReportOverviewAutomatedInvoiceData, ByVal customerNumber As Integer, ByVal closedMonth As Boolean) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date

			Dim rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithEmploymentNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, rpData.ESNr, rpData.ReportYear, rpData.ReportMonth, closedMonth)
			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, rpData.KstNr, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "K"
		''' </summary>
		Public Function BuildNewCustomerInvoiceData(ByVal rpData As ReportOverviewAutomatedInvoiceData, ByVal customerNumber As Integer, ByVal closedMonth As Boolean) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithCustomerNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, rpData.ReportYear, rpData.ReportMonth, closedMonth)
			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, rpData.KstNr, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "S"
		''' </summary>
		Public Function BuildNewCostCenterInvoiceData(ByVal rpData As ReportOverviewAutomatedInvoiceData, ByVal customerNumber As Integer, ByVal closedMonth As Boolean) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithCostcenterNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, rpData.RPNr, rpData.KstNr, rpData.ReportYear, rpData.ReportMonth, closedMonth)
			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, rpData.KstNr, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "E_W, K_W, R_W"
		''' </summary>
		Public Function BuildWeeklyInvoiceData(ByVal rpData As ReportOverviewAutomatedInvoiceData, ByVal customerNumber As Integer, ByVal closedMonth As Boolean) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithWeeklyEmploymentAndReportNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, rpData.RPNr, rpData.ESNr, rpData.ReportlineWeekFrom, rpData.ReportYear, rpData.ReportMonth, closedMonth)
			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, rpData.KstNr, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "S_W, SWR, SWR2"
		''' </summary>
		Public Function BuildWeeklyCostCenterInvoiceData(ByVal rpData As ReportOverviewAutomatedInvoiceData, ByVal customerNumber As Integer, ByVal closedMonth As Boolean) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithWeeklyCostCenterNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, rpData.RPNr, rpData.KstNr, rpData.ReportlineWeekFrom, rpData.ReportYear, rpData.ReportMonth, closedMonth)
			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, rpData.KstNr, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "RWSA, WSA"
		''' </summary>
		Public Function BuildWeeklyCostCenterAddressInvoiceData(ByVal rpData As ReportOverviewAutomatedInvoiceData, ByVal customerNumber As Integer) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithWeeklyCostCenterAddressNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, rpData.RPNr, rpData.ReportlineWeekFrom, rpData.KSTAddNr, rpData.ReportYear)
			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, rpData.KstNr, rpData.KSTAddNr)


			Return result

		End Function


#End Region



#Region "individuell selection"

		''' <summary>
		''' invoicetype "Report"
		''' </summary>
		Public Function BuildInvoiceForIndividuellSelectionWithReportNumberData(ByVal reportNumber As Integer, ByVal customerNumber As Integer) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData As ReportLineCreatingAutomatedInvoiceData
			rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithReportNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, reportNumber)

			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, Nothing, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "Einsatz"
		''' </summary>
		Public Function BuildInvoiceForIndividuellSelectionWithEmploymentNumberData(ByVal customerNumber As Integer?, ByVal employmentNumber As Integer, ByVal year As Integer) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData As ReportLineCreatingAutomatedInvoiceData
			rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithEmploymentNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, employmentNumber, year, Nothing, Nothing)

			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, Nothing, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "Kunden"
		''' </summary>
		Public Function BuildInvoiceForIndividuellSelectionWithCustomerNumberData(ByVal customerNumber As Integer, ByVal year As Integer) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData As ReportLineCreatingAutomatedInvoiceData
			rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithCustomerNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, year, Nothing, Nothing)

			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, Nothing, Nothing)


			Return result

		End Function

		''' <summary>
		''' invoicetype "RPKST"
		''' </summary>
		Public Function BuildInvoiceForIndividuellSelectionWithCostcenterNumberData(ByVal customerNumber As Integer, ByVal reportNumber As Integer, ByVal kstNumber As Integer, ByVal year As Integer) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date


			Dim rplineData As ReportLineCreatingAutomatedInvoiceData
			rplineData = m_InvoiceDatabaseAccess.LoadReportLineDataWithCostcenterNumberForCreatingAutomatedInvoices(m_MandantNumber, customerNumber, reportNumber, kstNumber, year, Nothing, Nothing)

			If rplineData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen konnten nicht geladen werden."))

				Return Nothing
			End If

			result = BuildInvoiceData(rplineData, kstNumber, Nothing)


			Return result

		End Function

#End Region


#Region "Debitoren logic"


		Private Function BuildInvoiceData(ByVal rplineData As ReportLineCreatingAutomatedInvoiceData, ByVal kstNumber As Integer?, ByVal addressNumber As Integer?) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date

			If m_SelectedBank Is Nothing Then
				Return Nothing
			End If
			Dim customerREAddresssData = m_InvoiceDatabaseAccess.LoadCustomerInvoiceDataForAutomatedInvoices(m_MandantNumber, rplineData.CustomerNumber, kstNumber, addressNumber)
			If customerREAddresssData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Adressdaten für Kundennummer: {0} konnten nicht geladen werden."), rplineData.CustomerNumber))

				Return Nothing
			End If

			Dim firstofRPDate = CDate(String.Format("01.{0}.{1}", rplineData.ReportMonth, rplineData.ReportYear))
			Dim endofRPDate = CDate(DateAdd(DateInterval.Month, 1, firstofRPDate.AddDays(-firstofRPDate.Day + 1))).AddDays(-1)

			m_InvoiceDateAsEndOfReportDate = InvoiceDateAsEndOfMonth(firstofRPDate.Year)
			m_InvoiceDueDateFromCreatedDate = InvoiceDueDateFromCreatedDate(firstofRPDate.Year)
			m_MwStNr = MwStNrSetting(firstofRPDate.Year)
			m_MwStAnsatz = MwStAnsatz(firstofRPDate.Year)
			m_ReferenceNumbersTo10Setting = ReferenceNumbersTo10Setting(firstofRPDate.Year)

			' loads fibukonten
			LoadPostingAccounts(firstofRPDate.Year)

			invoiceDate = If(m_InvoiceDateAsEndOfReportDate, m_dateUtility.MinDate(Now.Date, endofRPDate.Date), Now.Date)
			If invoiceDate.Year > rplineData.ReportYear Then invoiceDate = CDate(String.Format("31.12.{0}", rplineData.ReportYear))

			dueDate = CalculateDueDate(customerREAddresssData, invoiceDate)
			If dueDate < Now.Date Then dueDate = CalculateDueDate(customerREAddresssData, Now.Date)

			Dim invoiceArt As String = If(rplineData.BetragInkMwStTotal < 0, "G", "A")
			Select Case invoiceArt.ToUpper
				Case "A"
					FKSoll = m_PostingAccounts(1)
					FibuHaben0 = m_PostingAccounts(4)
					FibuHaben1 = m_PostingAccounts(2)

				Case "G"
					FKSoll = m_PostingAccounts(33)
					FibuHaben0 = m_PostingAccounts(34)
					FibuHaben1 = m_PostingAccounts(33)

			End Select
			If rplineData.MwSt = 0 Then
				FibuHaben1 = FibuHaben0
			Else
				FibuHaben0 = FibuHaben1
			End If

			success = success AndAlso CheckForConflictingMonthCloseRecordsInPeriod(m_MandantNumber, invoiceDate, invoiceDate)
			If Not success Then
				SplashScreenManager.CloseForm(False)

				Dim msg = "Der ausgewählte Monat ist abgeschlossen. Bitte öffnen Sie den Monat. {0}-{1}"
				msg = String.Format(m_Translate.GetSafeTranslationValue(msg), invoiceDate.Month, invoiceDate.Year)
				m_UtilityUI.ShowInfoDialog(msg, m_Translate.GetSafeTranslationValue("Rechnung erstellen"), MessageBoxIcon.Warning)

				Return Nothing
			End If

			result.MDNr = m_MandantNumber
			result.REKST1 = rplineData.RPKst1
			result.REKST2 = rplineData.RPKst2
			result.CreatedOn = DateTime.Now
			result.CreatedFrom = m_InitializationData.UserData.UserFullName
			result.Art = invoiceArt
			result.Art2 = "A"
			result.SKonto = 0
			result.Verlust = 0
			result.FSKonto = Nothing
			result.FVerlust = Nothing
			result.SPNr = 0
			result.VerNr = 0
			result.Storno = 0
			result.Gebucht = 0
			result.FBMonat = Nothing
			result.FBDat = Nothing
			result.FKSoll = FKSoll
			result.FKHaben0 = FibuHaben0
			result.FKHaben1 = FibuHaben1
			result.Result = Nothing
			result.ESRArt = 1
			result.MWSTNr = m_MwStNr
			result.PrintedDate = Nothing
			result.GebuchtAm = Nothing
			result.Ma3RepeatNr = 0
			result.EsEinstufung = rplineData.ES_Einstufung
			result.DTAName = Nothing
			result.DTAPLZOrt = Nothing
			result.DTAKonto = Nothing
			result.IBANDTA = Nothing
			result.IBANVG = Nothing
			result.EsrIBAN1 = m_SelectedBank.ESRIBAN1
			result.EsrIBAN2 = m_SelectedBank.ESRIBAN2
			result.EsrSwift = m_SelectedBank.Swift
			result.ProposeNr = Nothing
			result.MA0 = Nothing
			result.MA1 = Nothing
			result.MA2 = Nothing
			result.MA3 = Nothing
			result.MahnStopUntil = Nothing
			result.REDoc_Guid = String.Empty
			result.Transfered_User = String.Empty
			result.Transfered_On = String.Empty
			result.ZEBis0 = Nothing
			result.ZEBis1 = Nothing
			result.ZEBis2 = Nothing
			result.ZEBis3 = Nothing

			result.BetragOhne = rplineData.BetragOhneMwStTotal.GetValueOrDefault(0)
			result.BetragEx = m_Utility.SwissCommercialRound(rplineData.BetragInkMwStTotal.GetValueOrDefault(0))
			result.MWST1 = m_Utility.SwissCommercialRound(result.BetragEx * m_MwStAnsatz / 100)
			result.BetragInk = result.BetragOhne + result.BetragEx + result.MWST1

			Dim refNr As String = String.Empty
			Dim refFootNr As String = String.Empty


			' Update invoice data
			result.KdNr = rplineData.CustomerNumber
			result.KST = rplineData.RPKst

			result.ESRBankName = m_SelectedBank.BankName
			result.KontoNr = m_SelectedBank.KontoESR2

			result.Lp = CType(invoiceDate, Date).Month
			result.FakDat = CType(invoiceDate, Date)
			result.Currency = m_Currency
			result.Faellig = dueDate
			result.Mahncode = customerREAddresssData.MahnCode
			result.RName1 = customerREAddresssData.REFirma
			result.RName2 = customerREAddresssData.REFirma2
			result.RName3 = customerREAddresssData.REFirma3
			result.RZHD = customerREAddresssData.REZhd
			result.RAbteilung = customerREAddresssData.REAbteilung
			result.RPostfach = customerREAddresssData.REPostfach
			result.RStrasse = customerREAddresssData.REStrasse
			result.RLand = customerREAddresssData.RELand
			result.RPLZ = customerREAddresssData.REPLZ
			result.ROrt = customerREAddresssData.REOrt
			result.ReMail = customerREAddresssData.REeMail
			result.SendAsZip = customerREAddresssData.SendAsZip
			result.Zahlkond = customerREAddresssData.PaymentCondition
			result.RefNr = refNr
			result.RefFootNr = refFootNr
			result.ZEInfo = String.Empty
			result.ChangedOn = DateTime.Now
			result.ChangedFrom = m_InitializationData.UserData.UserFullName
			result.KDBranche = rplineData.KDBranche
			result.MWSTProz = If(result.MWST1 = 0, 0, m_MwStAnsatz)
			result.ESRBankID = m_SelectedBank.ID


			success = success AndAlso m_InvoiceDatabaseAccess.AddNewInvoice(result, m_InvoiceNumberOffsetFromSettings)
			success = success AndAlso result.ReNr.GetValueOrDefault(0) > 0
			success = success AndAlso m_InvoiceDatabaseAccess.UpdateInvoiceReferenceNumbers(result.ReNr, m_ReferenceNumbersTo10Setting)
			success = success AndAlso m_InvoiceDatabaseAccess.UpdateReportlineInoviceNumbers(result.ReNr, rplineData.RPLID)

			result.IsSelected = IsSelected


			Return result

		End Function

		''' <summary>
		''' Calculates the due date
		''' </summary>
		Private Function CalculateDueDate(ByVal addressData As CustomerReAddress, ByVal invoiceDate As Date) As Date
			Dim dueDate As Date = invoiceDate
			Dim calculateDate As Date = invoiceDate

			If m_InvoiceDueDateFromCreatedDate Then calculateDate = Now.Date

			If addressData Is Nothing OrElse String.IsNullOrWhiteSpace(addressData.MahnCode) Then
				Return dueDate
			End If

			If m_MahnCodeData Is Nothing Then
				m_MahnCodeData = m_CustomerDatabaseAccess.LoadPaymentReminderCodeData()
			End If

			Dim mahnCode = (From m In m_MahnCodeData Where m.GetField = addressData.MahnCode).FirstOrDefault
			If mahnCode Is Nothing OrElse mahnCode.GetField = "N" Then
				Return dueDate
			End If

			dueDate = calculateDate.AddDays(mahnCode.Reminder1)
			While (Weekday(dueDate) = vbSaturday OrElse Weekday(dueDate) = vbSunday)
				' next day after weekend
				dueDate = dueDate.AddDays(1)
			End While


			Return dueDate

		End Function


		''' <summary>
		''' Checks for conflicting MonthClose records in perdiod.
		''' </summary>
		''' <param name="startDate">The start date.</param>
		''' <param name="mdNumber">The mandant number.</param>
		''' <param name="endDate">The end date.</param>
		''' <returns>Boolean flag indicating if conflicting MonthClose records exist.</returns>
		Private Function CheckForConflictingMonthCloseRecordsInPeriod(ByVal mdNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean

			Dim isValid As Boolean = True

			Const RESULT_OK As Integer = 0
			Const RESULT_CONFLICT As Integer = 1

			Dim resultCode As Integer = 0
			Dim conflictedMonth = m_InvoiceDatabaseAccess.LoadConflictedMonthCloseRecordsInPeriod(mdNumber, startDate, endDate, resultCode)

			If conflictedMonth Is Nothing OrElse resultCode = -1 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler bei Konfliktprüfung"))
				Return False
			Else
				Select Case resultCode
					Case RESULT_OK
						' no conflicts
					Case RESULT_CONFLICT
						isValid = False


				End Select

			End If

			Return isValid
		End Function


#End Region



	End Class








End Namespace

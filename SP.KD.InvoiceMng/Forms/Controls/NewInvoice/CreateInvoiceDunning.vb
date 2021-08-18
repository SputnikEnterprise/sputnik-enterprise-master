
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


	Public Class CreateInvoiceDunning


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
		Private m_MahnCodeData As DatabaseAccess.Customer.DataObjects.PaymentReminderCodeData

		Private m_SelectedBank As BankData
		Private m_ReferenceNumbersTo10Setting As Boolean

		Private m_MwStNr As String
		Private m_Currency As String
		Private m_MwStAnsatz As Decimal

		Private m_MahnspesenabSetting As Integer
		Private m_MahnspesenchfSetting As Decimal
		Private m_VerzugszinsdaysafterSetting As Integer
		Private m_VerzugszinspercentSetting As Decimal


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

		Public Property DunningData As List(Of SP.DatabaseAccess.Customer.DataObjects.PaymentReminderCodeData)
		Public Property BankData As List(Of SP.DatabaseAccess.Invoice.DataObjects.BankData)
		Public Property IsSelected As Boolean

		Public Property MandantNumber As Integer
		Public Property DunningLevel As Integer
		Public Property InvoiceReminderCode As String
		Public Property ZEUntil As Date
		Public Property DunningDate As Date


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

				Dim mdNumber = MandantNumber

				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

				Dim currencyvalue As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/currencyvalue", FORM_XML_MAIN_KEY))

				Return currencyvalue

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt number setting.
		''' </summary>
		Private ReadOnly Property MwStNrSetting As String
			Get
				Dim mdNumber = MandantNumber

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstNumber As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mwstnr", FORM_XML_MAIN_KEY))

				Return mwstNumber

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt Ansatz setting.
		''' </summary>
		Private ReadOnly Property MwStAnsatz As Decimal
			Get

				Dim mdNumber = MandantNumber

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)

			End Get

		End Property

		''' <summary>
		''' Gets the reference number to 10 setting.
		''' </summary>
		Private ReadOnly Property ReferenceNumbersTo10Setting As Boolean
			Get

				Dim mdNumber = MandantNumber

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim ref10forfactoring As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/ref10forfactoring", FORM_XML_MAIN_KEY)), False)

				Return ref10forfactoring.HasValue AndAlso ref10forfactoring

			End Get

		End Property

		''' <summary>
		''' Gets the dunning from setting.
		''' </summary>
		Private ReadOnly Property MahnspesenabSetting As Integer
			Get
				Dim mdNumber = MandantNumber

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim dunningFrom As Integer = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mahnspesenab", FORM_XML_MAIN_KEY)))

				Return dunningFrom

			End Get

		End Property

		''' <summary>
		''' Gets the dunning amount setting.
		''' </summary>
		Private ReadOnly Property MahnspesenchfSetting As Decimal
			Get
				Dim mdNumber = MandantNumber

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim dunningAmount As Integer = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mahnspesenchf", FORM_XML_MAIN_KEY)))

				Return dunningAmount

			End Get

		End Property

		''' <summary>
		''' Gets the verzug after day setting.
		''' </summary>
		Private ReadOnly Property VerzugszinsdaysafterSetting As Integer
			Get
				Dim mdNumber = MandantNumber

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim dunningFrom As Integer = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/verzugszinsdaysafter", FORM_XML_MAIN_KEY)))

				Return dunningFrom

			End Get

		End Property

		''' <summary>
		''' Gets the verzug amount per day setting.
		''' </summary>
		Private ReadOnly Property VerzugszinspercentSetting As Decimal
			Get
				Dim mdNumber = MandantNumber

				Dim invoiceYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim verzugPercent As Integer = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/verzugszinspercent", FORM_XML_MAIN_KEY)))

				Return verzugPercent

			End Get

		End Property


#End Region


#Region "private methodes"


		''' <summary>
		''' Loads the Posting Accounts from XML
		''' </summary>
		Private Sub LoadPostingAccounts()

			Dim mdNumber = MandantNumber
			Dim invoiceYear = Now.Year

			m_PostingAccounts = New Dictionary(Of Integer, String)
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

			Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/BuchungsKonten", mdNumber)
			For i As Integer = 1 To 38
				Dim strValue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(
																												 m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear),
																												 String.Format("{0}/_{1}", FORM_XML_MAIN_KEY, i)), "0")
				m_PostingAccounts.Add(i, strValue)
			Next
		End Sub


#End Region


#Region "public methodes"

		Public Sub InitialData()

			m_MwStNr = MwStNrSetting
			m_Currency = CurrencySetting
			m_MwStAnsatz = MwStAnsatz
			m_ReferenceNumbersTo10Setting = ReferenceNumbersTo10Setting

			m_MahnspesenabSetting = MahnspesenabSetting
			m_MahnspesenchfSetting = MahnspesenchfSetting
			m_VerzugszinsdaysafterSetting = VerzugszinsdaysafterSetting
			m_VerzugszinspercentSetting = VerzugszinspercentSetting


			' loads fibukonten
			LoadPostingAccounts()
			'm_SelectedBank = (From b In BankData Where b.AsStandard = True).FirstOrDefault

		End Sub


		''' <summary>
		''' ma0 is null
		''' </summary>
		Public Function BuildKontoauszugData(ByVal invoiceData As SP.DatabaseAccess.Invoice.DataObjects.Invoice, ByVal zeUntil As Date, ByVal dunningDate As Date) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim invoiceDate As Date = Now.Date
			Dim dueDate As Date = Now.Date

			success = success AndAlso m_InvoiceDatabaseAccess.UpdateInvoiceDataForKontoauszug(MandantNumber, invoiceData.ReNr, zeUntil, dunningDate)
			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten für Mahnung konnten nicht aktuallisiert werden."))

				Return Nothing
			End If

			result = m_InvoiceDatabaseAccess.LoadInvoice(invoiceData.ReNr)
			result.IsSelected = IsSelected


			Return result

		End Function

		''' <summary>
		''' ma1 is null
		''' </summary>
		Public Function BuildFirstDunningData(ByVal invoiceData As SP.DatabaseAccess.Invoice.DataObjects.Invoice) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim dayCountOnVerzug As Integer = 0
			Dim verzugAmont As Decimal = 0


			m_MahnCodeData = (From b In DunningData Where b.GetField = invoiceData.Mahncode).FirstOrDefault
			If DateAdd("d", CInt(m_MahnCodeData.Reminder2), invoiceData.ZEBis0) >= ZEUntil Then Return Nothing

			If DateAdd("d", m_VerzugszinsdaysafterSetting, invoiceData.FakDat) < ZEUntil AndAlso m_VerzugszinsdaysafterSetting > 0 Then
				dayCountOnVerzug = DateDiff("d", DateAdd("d", m_VerzugszinsdaysafterSetting, invoiceData.FakDat), ZEUntil, FirstDayOfWeek.System, FirstWeekOfYear.System)
				verzugAmont = ((invoiceData.BetragEx + invoiceData.BetragOhne) * (m_VerzugszinspercentSetting / 100))
				verzugAmont /= 360
				verzugAmont *= dayCountOnVerzug
			End If

			success = success AndAlso m_InvoiceDatabaseAccess.UpdateInvoiceDataForFirstDunning(MandantNumber, invoiceData.ReNr, ZEUntil, DunningDate, m_MahnspesenabSetting, m_MahnspesenchfSetting, verzugAmont)
			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten für Mahnung konnten nicht aktuallisiert werden."))

				Return Nothing
			End If
			If m_MahnspesenabSetting = 1 AndAlso m_MahnspesenchfSetting + verzugAmont > 0 Then
				success = success AndAlso BuildFirstDunningAndArrearsData(invoiceData, verzugAmont, 1)
			End If

			result = m_InvoiceDatabaseAccess.LoadInvoice(invoiceData.ReNr)
			result.IsSelected = IsSelected


			Return result

		End Function

		''' <summary>
		''' ma2 is null
		''' </summary>
		Public Function BuildSecondDunningData(ByVal invoiceData As SP.DatabaseAccess.Invoice.DataObjects.Invoice) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim dayCountOnVerzug As Integer = 0
			Dim verzugAmont As Decimal = 0


			m_MahnCodeData = (From b In DunningData Where b.GetField = invoiceData.Mahncode).FirstOrDefault
			If DateAdd("d", CInt(m_MahnCodeData.Reminder3), invoiceData.ZEBis1) >= ZEUntil Then Return Nothing

			If DateAdd("d", m_VerzugszinsdaysafterSetting, invoiceData.FakDat) < ZEUntil AndAlso m_VerzugszinsdaysafterSetting > 0 Then
				dayCountOnVerzug = DateDiff("d", DateAdd("d", m_VerzugszinsdaysafterSetting, invoiceData.FakDat), ZEUntil, FirstDayOfWeek.System, FirstWeekOfYear.System)
				verzugAmont = ((invoiceData.BetragEx + invoiceData.BetragOhne) * (m_VerzugszinspercentSetting / 100))
				verzugAmont /= 360
				verzugAmont *= dayCountOnVerzug
			End If

			success = success AndAlso m_InvoiceDatabaseAccess.UpdateInvoiceDataForSecondDunning(MandantNumber, invoiceData.ReNr, ZEUntil, DunningDate, m_MahnspesenabSetting, m_MahnspesenchfSetting, verzugAmont)
			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten für Mahnung konnten nicht aktuallisiert werden."))

				Return Nothing
			End If
			If m_MahnspesenabSetting >= 1 AndAlso m_MahnspesenchfSetting + verzugAmont > 0 Then
				success = success AndAlso BuildFirstDunningAndArrearsData(invoiceData, verzugAmont, 2)
			End If

			result = m_InvoiceDatabaseAccess.LoadInvoice(invoiceData.ReNr)
			result.IsSelected = IsSelected


			Return result

		End Function

		''' <summary>
		''' ma3 is null
		''' </summary>
		Public Function BuildThirdDunningData(ByVal invoiceData As SP.DatabaseAccess.Invoice.DataObjects.Invoice) As SP.DatabaseAccess.Invoice.DataObjects.Invoice
			Dim success As Boolean = True
			Dim result = New SP.DatabaseAccess.Invoice.DataObjects.Invoice


			m_MahnCodeData = (From b In DunningData Where b.GetField = invoiceData.Mahncode).FirstOrDefault
			If DateAdd("d", CInt(m_MahnCodeData.Reminder4), invoiceData.ZEBis1) >= ZEUntil Then Return Nothing

			success = success AndAlso m_InvoiceDatabaseAccess.UpdateInvoiceDataForThirdDunning(MandantNumber, invoiceData.ReNr, ZEUntil, DunningDate)
			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten für Mahnung konnten nicht aktuallisiert werden."))

				Return Nothing
			End If

			result = m_InvoiceDatabaseAccess.LoadInvoice(invoiceData.ReNr)
			result.IsSelected = IsSelected


			Return result

		End Function

#End Region


#Region "Debitoren logic"


		Private Function BuildFirstDunningAndArrearsData(ByVal invoiceData As SP.DatabaseAccess.Invoice.DataObjects.Invoice, ByVal verzugAmont As Decimal?, ByVal spNumber As Integer?) As Boolean
			Dim success As Boolean = True
			Dim result = New DunningAndArrearsData
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0
			Dim dunningTotalAmount As Decimal = 0
			Dim ArrearsTotalAmount As Decimal = 0

			If m_MahnspesenabSetting = 0 Then m_MahnspesenchfSetting = 0
			If m_VerzugszinsdaysafterSetting = 0 Then verzugAmont = 0

			If invoiceData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))

				Return Nothing
			End If

			Dim invoiceArt As String = If(invoiceData.BetragInk > 0, "A", "G")
			Select Case invoiceArt.ToUpper
				Case "A"
					FKSoll = m_PostingAccounts(1)
					FibuHaben0 = m_PostingAccounts(4)
					FibuHaben1 = m_PostingAccounts(2)

			End Select
			If invoiceData.MWST1 = 0 Then
				FibuHaben1 = FibuHaben0
			Else
				FibuHaben0 = FibuHaben1
			End If


			If m_MahnspesenchfSetting > 0 Then
				result.MDNr = MandantNumber
				result.RENr = invoiceData.ReNr
				result.KDNr = invoiceData.KdNr
				result.SPNumber = spNumber
				result.MwStProz = invoiceData.MWSTProz

				result.MwStNr = invoiceData.MWSTNr
				result.ESRArt = invoiceData.ESRArt
				result.ESRID = invoiceData.ESRID
				result.ESRKonto = invoiceData.ESRKonto
				result.KontoNr = invoiceData.KontoNr
				result.SP_Text = m_Translate.GetSafeTranslationValue("Mahnspesen")

				result.FKSoll = FKSoll
				result.FKHaben0 = FibuHaben0
				result.FKHaben1 = FibuHaben1
				result.SPDate = ZEUntil

				result.DunningAmount = m_MahnspesenchfSetting * (spNumber / m_MahnspesenabSetting)

				result.BetragEx = invoiceData.BetragEx.GetValueOrDefault(0)
				result.BetragInk = invoiceData.BetragInk.GetValueOrDefault(0)
				dunningTotalAmount = m_Utility.SwissCommercialRound(result.DunningAmount * invoiceData.MWSTProz.GetValueOrDefault(0) / 100) + result.DunningAmount
				result.SP_BetragTotal = dunningTotalAmount
				result.SP_Bezahlt = 0

				success = success AndAlso m_InvoiceDatabaseAccess.AddNewDunning(MandantNumber, result)
			End If



			' insert Arrears data
			If verzugAmont.GetValueOrDefault(0) > 0 Then
				result.MDNr = MandantNumber
				result.RENr = invoiceData.ReNr
				result.KDNr = invoiceData.KdNr
				result.SPNumber = 1
				result.MwStProz = 0
				result.SP_Text = String.Format(m_Translate.GetSafeTranslationValue("Verzugszinsen {0:d} ({1:f0} Tage)"), ZEUntil.Date, m_VerzugszinsdaysafterSetting)
				result.MwStNr = invoiceData.MWSTNr
				result.ESRArt = invoiceData.ESRArt
				result.ESRID = invoiceData.ESRID
				result.ESRKonto = invoiceData.ESRKonto
				result.KontoNr = invoiceData.KontoNr

				result.FKSoll = FKSoll
				result.FKHaben0 = FibuHaben0
				result.FKHaben1 = FibuHaben1
				result.SPDate = ZEUntil

				result.DunningAmount = m_Utility.SwissCommercialRound(verzugAmont.GetValueOrDefault(0))

				result.BetragEx = invoiceData.BetragEx.GetValueOrDefault(0)
				result.BetragInk = invoiceData.BetragInk.GetValueOrDefault(0)
				ArrearsTotalAmount = m_Utility.SwissCommercialRound(result.DunningAmount)
				result.SP_BetragTotal = ArrearsTotalAmount
				result.SP_Bezahlt = 0

				success = success AndAlso m_InvoiceDatabaseAccess.AddNewDunning(MandantNumber, result)
			End If

			If success AndAlso dunningTotalAmount + ArrearsTotalAmount > 0 Then

				'	If (m_MahnspesenabSetting = 1 AndAlso m_MahnspesenchfSetting > 0) OrElse verzugAmont > 0 Then
				Dim newOPAmount As Decimal = invoiceData.BetragInk + dunningTotalAmount + ArrearsTotalAmount

				success = success AndAlso m_InvoiceDatabaseAccess.UpdateDunningAndArrearReferenceNumbers(m_InitializationData.MDData.MDNr, invoiceData.ReNr, invoiceData.KdNr, newOPAmount, m_ReferenceNumbersTo10Setting)
				'End If

			End If


			Return success

		End Function

		''' <summary>
		''' Calculates the due date
		''' </summary>
		Private Function CalculateDueDate(ByVal addressData As CustomerReAddress, ByVal invoiceDate As Date) As Date
			Dim dueDate As Date = invoiceDate

			If addressData Is Nothing OrElse String.IsNullOrWhiteSpace(addressData.MahnCode) Then
				Return dueDate
			End If

			If m_MahnCodeData Is Nothing Then
				m_MahnCodeData = m_CustomerDatabaseAccess.LoadPaymentReminderCodeData()
			End If

			'Dim mahnCode = (From m In m_MahnCodeData Where m.GetField = addressData.MahnCode).FirstOrDefault
			If m_MahnCodeData Is Nothing OrElse m_MahnCodeData.GetField = "N" Then
				Return dueDate
			End If

			dueDate = invoiceDate.AddDays(m_MahnCodeData.Reminder1)
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

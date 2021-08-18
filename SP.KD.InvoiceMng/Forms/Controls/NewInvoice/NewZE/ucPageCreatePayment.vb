
Imports SP.DatabaseAccess.Invoice
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Invoice.DataObjects

Namespace UI

	Public Class ucPageCreatePayment


#Region "Private Fields"

		''' <summary>
		''' The ESNr of the newly created ES.
		''' </summary>
		Private m_PaymentNumberOfNewlyCreatedPayment As Integer?

		''' <summary>
		''' The new invoice data.
		''' </summary>
		Private Property m_NewPaymentData As NewPaymentInitData

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

		''' <summary>
		''' The posting accounts.
		''' </summary>
		Private m_PostingAccounts As New Dictionary(Of Integer, String)

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			Try
				m_ProgPath = New ClsProgPath
				m_Mandant = New Mandant
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			'Captions
			Me.gpEigenschaftenZE.Text = m_Translate.GetSafeTranslationValue(Me.gpEigenschaftenZE.Text)
			Me.gpRechnungsdaten.Text = m_Translate.GetSafeTranslationValue(Me.gpRechnungsdaten.Text)

			'Labels
			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)

			Me.lblRechnung.Text = m_Translate.GetSafeTranslationValue(Me.lblRechnung.Text)
			Me.lblRechnungsbetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblRechnungsbetrag.Text)

			Me.lblSKonto.Text = m_Translate.GetSafeTranslationValue(Me.lblSKonto.Text)
			Me.lblZEBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblZEBetrag.Text)
			Me.lblValuta.Text = m_Translate.GetSafeTranslationValue(Me.lblValuta.Text)
			Me.lblBuchung.Text = m_Translate.GetSafeTranslationValue(Me.lblBuchung.Text)

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the RENr of the newly created Invoice.
		''' </summary>
		Public ReadOnly Property PaymentNumberOfNewlyCreatedPayment As Integer?
			Get
				Return m_PaymentNumberOfNewlyCreatedPayment
			End Get

		End Property

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets the currency setting.
		''' </summary>
		Public ReadOnly Property CurrencySetting As String
			Get

				Dim mdNumber = m_UCPaymentMediator.InitPaymentDataPage1.MandantData.MandantNumber
				Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

				Dim currencyvalue As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/currencyvalue", FORM_XML_MAIN_KEY))

				Return currencyvalue

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt number setting.
		''' </summary>
		Public ReadOnly Property MwStNrSetting As String
			Get
				Dim mdNumber = m_UCPaymentMediator.InitPaymentDataPage1.MandantData.MandantNumber
				Dim invoiceYear = m_UCPaymentMediator.InitPaymentDataPage1.VDate.Value.Date.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim mwstNumber As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mwstnr", FORM_XML_MAIN_KEY))

				Return mwstNumber

			End Get

		End Property

		''' <summary>
		''' Gets the MwSt Ansatz setting.
		''' </summary>
		Public ReadOnly Property MwStAnsatz As Decimal
			Get

				Dim isMWStPflichtig = m_UCPaymentMediator.InitPaymentDataPage1.InvoiceData.MWSTProz.GetValueOrDefault(0) <> 0

				If Not isMWStPflichtig Then
					Return 0
				End If

				Dim mdNumber = m_UCPaymentMediator.InitPaymentDataPage1.MandantData.MandantNumber
				Dim invoiceYear = m_UCPaymentMediator.InitPaymentDataPage1.VDate.Value.Date.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, invoiceYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)

			End Get

		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
			MyBase.InitWithConfigurationData(initializationClass, translationHelper)
		End Sub

		''' <summary>
		''' Activates the page.
		''' </summary>
		''' <returns>Boolean value indicating success.</returns>
		Public Overrides Function ActivatePage() As Boolean

			Dim success As Boolean = True
			Dim initDataPage1 = m_UCPaymentMediator.InitPaymentDataPage1

			lblMandantValue.Text = String.Format("{0}", initDataPage1.MandantData.MandantName1)
			lblInvoiceValue.Text = String.Format("{0}: {1}", initDataPage1.InvoiceData.ReNr, initDataPage1.InvoiceData.RName1)
			lblInvoiceAmountValue.Text = String.Format(m_Translate.GetSafeTranslationValue("{1:n2}{0}Offener Betrag: {2:n2}"),
																								 vbNewLine, initDataPage1.InvoiceData.BetragInk,
																								 (initDataPage1.InvoiceData.BetragInk.GetValueOrDefault(0) - initDataPage1.InvoiceData.Bezahlt.GetValueOrDefault(0)))

			lblSKontoValue.Text = String.Format("{0:F0}", initDataPage1.FKSOLL)
			lblPaymentAmountValue.Text = String.Format("{0:n2}", initDataPage1.Amount)

			If initDataPage1.VDate.HasValue Then
				lblValutaValue.Text = String.Format("{0:dd.MM.yyyy}", initDataPage1.VDate)
			Else
				lblValutaValue.Text = "-"
			End If
			If initDataPage1.BDate.HasValue Then
				lblBuchungValue.Text = String.Format("{0:dd.MM.yyyy}", initDataPage1.BDate)
			Else
				lblBuchungValue.Text = "-"
			End If


			m_IsFirstPageActivation = False

			Return success
		End Function

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_IsFirstPageActivation = True
			m_PostingAccounts = New Dictionary(Of Integer, String)
			m_NewPaymentData = Nothing
			m_PaymentNumberOfNewlyCreatedPayment = Nothing

			lblMandantValue.Text = String.Empty

			lblInvoiceValue.Text = String.Empty
			lblInvoiceAmountValue.Text = String.Empty

			lblSKontoValue.Text = String.Empty
			lblPaymentAmountValue.Text = String.Empty
			lblValutaValue.Text = String.Empty
			lblBuchungValue.Text = String.Empty

		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Reads the payment offset from the settings.
		''' </summary>
		''' <returns>payment offset or zero if it could not be read.</returns>
		Private Function ReadPaymentOffsetFromSettings() As Integer

			Dim strQuery As String = "//StartNr/Zahlungseingänge"
			Dim r = m_ClsProgSetting.GetUserProfileFile
			Dim paymentNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
			Dim intVal As Integer

			If Integer.TryParse(paymentNumberStartNumberSetting, intVal) Then
				Return intVal
			Else
				Return 0
			End If

		End Function

		''' <summary>
		''' Loads the Posting Accounts from XML
		''' </summary>
		Private Sub LoadPostingAccounts()

			Dim mdNumber = m_UCPaymentMediator.InitPaymentDataPage1.MandantData.MandantNumber
			Dim invoiceYear = m_UCPaymentMediator.InitPaymentDataPage1.VDate.Value.Date.Year

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

		''' <summary>
		''' Creates the invoice.
		''' </summary>
		Public Sub CreatePayment()

			MapUiToInvoiceData()

			Dim success = m_UCPaymentMediator.InvoiceDbAccess.AddNewPayment(m_NewPaymentData)

			m_PaymentNumberOfNewlyCreatedPayment = IIf(success, m_NewPaymentData.NewPaymentNr, Nothing)

		End Sub

		''' <summary>
		''' Maps the UI-Data to DTO
		''' </summary>
		Private Sub MapUiToInvoiceData()

			' new Invoice: initialization

			Dim initDataPage1 = m_UCPaymentMediator.InitPaymentDataPage1
			Dim paymentNumberOffsetFromSettings As Integer = ReadPaymentOffsetFromSettings()

			Dim debitorenart1 As String = initDataPage1.InvoiceData.Art
			Dim debitorenart2 As String = initDataPage1.InvoiceData.Art2

			m_NewPaymentData = New NewPaymentInitData
			'Dim mwStrNr As String = MwStNrSetting
			'Dim FKSoll As Integer = 0
			'Dim FibuHaben0 As Integer = 0
			'Dim FibuHaben1 As Integer = 0

			'Select Case debitorenart1.ToUpper
			'	Case "I"
			'		FKSoll = m_PostingAccounts(15)
			'		FibuHaben0 = m_PostingAccounts(5)
			'		FibuHaben1 = m_PostingAccounts(3)

			'	Case "F"
			'		FKSoll = m_PostingAccounts(16)
			'		FibuHaben0 = m_PostingAccounts(18)
			'		FibuHaben1 = m_PostingAccounts(17)

			'End Select

			'Select Case (debitorenart1 & debitorenart2).ToUpper
			'	Case "GA"
			'		FKSoll = m_PostingAccounts(33)
			'		FibuHaben0 = m_PostingAccounts(34)
			'		FibuHaben1 = m_PostingAccounts(33)

			'	Case "GI"
			'		FKSoll = m_PostingAccounts(35)
			'		FibuHaben0 = m_PostingAccounts(36)
			'		FibuHaben1 = m_PostingAccounts(35)

			'	Case "GF"
			'		FKSoll = m_PostingAccounts(37)
			'		FibuHaben0 = m_PostingAccounts(38)
			'		FibuHaben1 = m_PostingAccounts(37)

			'	Case "RA"
			'		FKSoll = m_PostingAccounts(23)
			'		FibuHaben0 = m_PostingAccounts(24)
			'		FibuHaben1 = m_PostingAccounts(23)

			'	Case "RI"
			'		FKSoll = m_PostingAccounts(27)
			'		FibuHaben0 = m_PostingAccounts(28)
			'		FibuHaben1 = m_PostingAccounts(27)

			'	Case "RF"
			'		FKSoll = m_PostingAccounts(31)
			'		FibuHaben0 = m_PostingAccounts(32)
			'		FibuHaben1 = m_PostingAccounts(31)

			'End Select
			'If MwStAnsatz = 0 Then
			'	FibuHaben1 = FibuHaben0
			'Else
			'	FibuHaben0 = FibuHaben1
			'End If


			With m_NewPaymentData
				.MDNr = initDataPage1.MandantData.MandantNumber
				.KDNR = initDataPage1.InvoiceData.KdNr
				.RENR = initDataPage1.InvoiceData.ReNr
				.FKSOLL = initDataPage1.FKSOLL
				.VDate = initDataPage1.VDate
				.BDate = initDataPage1.BDate
				.Amount = initDataPage1.Amount

				.Currency = CurrencySetting

				.CreatedFrom = m_InitializationData.UserData.UserFullName

				.PaymentNumberOffset = paymentNumberOffsetFromSettings

			End With

			' Update payment data
			With m_NewPaymentData
				.KDNR = initDataPage1.InvoiceData.KdNr
				.Currency = CurrencySetting

			End With

		End Sub

#End Region

	End Class

End Namespace

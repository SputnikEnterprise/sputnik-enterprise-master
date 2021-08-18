Imports SP.DatabaseAccess.Invoice
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten

Namespace UI

  Public Class ucPageCreateInvoice

#Region "Private Fields"

    ''' <summary>
    ''' The ESNr of the newly created ES.
    ''' </summary>
    Private m_ReNrOfNewlyCreatedInvoice As Integer?

    ''' <summary>
    ''' The new invoice data.
    ''' </summary>
    Private Property m_NewInvoiceData As DatabaseAccess.Invoice.DataObjects.Invoice

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
      Me.gpEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.gpEigenschaften.Text)
      Me.gpRechnungsdaten.Text = m_Translate.GetSafeTranslationValue(Me.gpRechnungsdaten.Text)

      'Labels
      Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
      Me.lblKst1.Text = m_Translate.GetSafeTranslationValue(Me.lblKst1.Text)
      Me.lblAdvisor.Text = m_Translate.GetSafeTranslationValue(Me.lblAdvisor.Text)
      Me.lblDebitorenart.Text = m_Translate.GetSafeTranslationValue(Me.lblDebitorenart.Text)
      Me.lblBankdaten.Text = m_Translate.GetSafeTranslationValue(Me.lblBankdaten.Text)
      Me.lblDatum.Text = m_Translate.GetSafeTranslationValue(Me.lblDatum.Text)
      Me.lblFirma.Text = m_Translate.GetSafeTranslationValue(Me.lblFirma.Text)
      Me.lblAddresse.Text = m_Translate.GetSafeTranslationValue(Me.lblAddresse.Text)
      Me.lblDueDate.Text = m_Translate.GetSafeTranslationValue(Me.lblDueDate.Text)

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the RENr of the newly created Invoice.
    ''' </summary>
    Public ReadOnly Property ReNrOfNewlyCreatedInvoice As Integer?
      Get
        Return m_ReNrOfNewlyCreatedInvoice
      End Get

    End Property

#End Region

#Region "Private Properties"

    ''' <summary>
    ''' Gets the currency setting.
    ''' </summary>
    Public ReadOnly Property CurrencySetting As String
      Get

        Dim mdNumber = m_UCMediator.InitDataPage1.MandantData.MandantNumber
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
        Dim mdNumber = m_UCMediator.InitDataPage1.MandantData.MandantNumber
        Dim invoiceYear = m_UCMediator.InitDataPage2.DateValue.Value.Date.Year
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

        Dim initDataPage2 = m_UCMediator.InitDataPage2
        Dim isMWStPflichtig = InitDataPage2.Customer.MWST.HasValue AndAlso InitDataPage2.Customer.MWST = True

        If Not isMWStPflichtig Then
          Return 0
        End If

        Dim mdNumber = m_UCMediator.InitDataPage1.MandantData.MandantNumber
        Dim invoiceYear = m_UCMediator.InitDataPage2.DateValue.Value.Date.Year
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

      ' Always load posting accounts, because they depent on the invoice date from the second page.
      LoadPostingAccounts()

      Dim initDataPage1 = m_UCMediator.InitDataPage1
      Dim initDataPage2 = m_UCMediator.InitDataPage2

      lblMandantValue.Text = String.Format("{0}", initDataPage1.MandantData.MandantName1)
      lblKostenstellenValue.Text = String.Format("{0}/{1}", If(initDataPage1.CostCenter1 Is Nothing, String.Empty, initDataPage1.CostCenter1.KSTName),
                                                            If(initDataPage1.CostCenter2 Is Nothing, String.Empty, initDataPage1.CostCenter2.KSTName))

      lblBeraterValue.Text = initDataPage1.CombinedAdvisorString

      lblDebitorenartValue.Text = initDataPage2.DebitorenArt.Display
      lblBankdatenValue.Text = initDataPage2.BankData.BankName

      If initDataPage2.DateValue.HasValue Then
        lblDatumValue.Text = String.Format("{0:dd.MM.yyyy}", initDataPage2.DateValue)
      Else
        lblDatumValue.Text = "-"
      End If

      lblFirmaValue.Text = initDataPage2.Customer.Firma1
      lblAdresseValue.Text = initDataPage2.CustomerReAddress.Address

      If initDataPage2.DueDate.HasValue Then
        lblFaelligValue.Text = String.Format("{0:dd.MM.yyyy}", initDataPage2.DueDate)
      Else
        lblFaelligValue.Text = "-"
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
      m_NewInvoiceData = Nothing
      m_ReNrOfNewlyCreatedInvoice = Nothing

      lblMandantValue.Text = String.Empty
      lblKostenstellenValue.Text = String.Empty
      lblBankdatenValue.Text = String.Empty
      lblDebitorenartValue.Text = String.Empty
      lblBankdatenValue.Text = String.Empty
      lblDatumValue.Text = String.Empty
      lblFirmaValue.Text = String.Empty
      lblAdresseValue.Text = String.Empty
      lblFaelligValue.Text = String.Empty

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Reads the invoice offset from the settings.
    ''' </summary>
    ''' <returns>Invoice offset or zero if it could not be read.</returns>
    Private Function ReadInvoiceOffsetFromSettings() As Integer

      Dim strQuery As String = "//StartNr/Fakturen"
      Dim r = m_ClsProgSetting.GetUserProfileFile
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
    Private Sub LoadPostingAccounts()

      Dim mdNumber = m_UCMediator.InitDataPage1.MandantData.MandantNumber
      Dim invoiceYear = m_UCMediator.InitDataPage2.DateValue.Value.Date.Year

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
    Public Sub CreateInvoice()

      Dim invoiceNumberOffsetFromSettings As Integer = ReadInvoiceOffsetFromSettings()

      MapUiToInvoiceData()

      Dim success = m_UCMediator.InvoiceDbAccess.AddNewInvoice(m_NewInvoiceData, invoiceNumberOffsetFromSettings)

      m_ReNrOfNewlyCreatedInvoice = IIf(success, m_NewInvoiceData.ReNr, Nothing)


    End Sub

    ''' <summary>
    ''' Maps the UI-Data to DTO
    ''' </summary>
    Private Sub MapUiToInvoiceData()

      ' new Invoice: initialization

      Dim initDataPage1 = m_UCMediator.InitDataPage1
      Dim initdataPage2 = m_UCMediator.InitDataPage2


      Dim debitorenart As String = (initdataPage2.DebitorenArt.Value).Trim
      Dim debitorenart1 As String = String.Empty
      Dim debitorenart2 As String = String.Empty
      If debitorenart.Length >= 1 Then
        debitorenart1 = debitorenart.Substring(0, 1)
        debitorenart2 = debitorenart1
      End If
      If debitorenart.Length >= 2 Then
        debitorenart2 = debitorenart.Substring(1, 1)
      End If
      m_NewInvoiceData = New DataObjects.Invoice

      Dim mwStrNr As String = MwStNrSetting
			Dim FKSoll As Integer = 0
			Dim FibuHaben0 As Integer = 0
			Dim FibuHaben1 As Integer = 0

			Select Case debitorenart1.ToUpper
				Case "I"
					FKSoll = m_PostingAccounts(15)
					FibuHaben0 = m_PostingAccounts(5)
					FibuHaben1 = m_PostingAccounts(3)

				Case "F"
					FKSoll = m_PostingAccounts(16)
					FibuHaben0 = m_PostingAccounts(18)
					FibuHaben1 = m_PostingAccounts(17)

			End Select

			Select Case (debitorenart1 & debitorenart2).ToUpper
				Case "GA"
					FKSoll = m_PostingAccounts(33)
					FibuHaben0 = m_PostingAccounts(34)
					FibuHaben1 = m_PostingAccounts(33)

				Case "GI"
					FKSoll = m_PostingAccounts(35)
					FibuHaben0 = m_PostingAccounts(36)
					FibuHaben1 = m_PostingAccounts(35)

				Case "GF"
					FKSoll = m_PostingAccounts(37)
					FibuHaben0 = m_PostingAccounts(38)
					FibuHaben1 = m_PostingAccounts(37)

				Case "RA"
					FKSoll = m_PostingAccounts(23)
					FibuHaben0 = m_PostingAccounts(24)
					FibuHaben1 = m_PostingAccounts(23)

				Case "RI"
					FKSoll = m_PostingAccounts(27)
					FibuHaben0 = m_PostingAccounts(28)
					FibuHaben1 = m_PostingAccounts(27)

				Case "RF"
					FKSoll = m_PostingAccounts(31)
					FibuHaben0 = m_PostingAccounts(32)
					FibuHaben1 = m_PostingAccounts(31)

			End Select
			If MwStAnsatz = 0 Then
				FibuHaben1 = FibuHaben0
			Else
				FibuHaben0 = FibuHaben1
			End If

			With m_NewInvoiceData
				.MDNr = initDataPage1.MandantData.MandantNumber
				.REKST1 = If(initDataPage1.CostCenter1 Is Nothing, String.Empty, initDataPage1.CostCenter1.KSTName)
				.REKST2 = If(initDataPage1.CostCenter2 Is Nothing, String.Empty, initDataPage1.CostCenter2.KSTName)
				.CreatedOn = DateTime.Now
				.CreatedFrom = m_InitializationData.UserData.UserFullName
				.Art = debitorenart1
				.Art2 = debitorenart2
				.SKonto = 0
				.Verlust = 0
				.FSKonto = Nothing
				.FVerlust = Nothing
				.SPNr = 0
				.VerNr = 0
				.Storno = 0
				.Gebucht = 0
				.FBMonat = Nothing
				.FBDat = Nothing
				.FKSoll = FKSoll
				.FKHaben0 = FibuHaben0
				.FKHaben1 = FibuHaben1
				.Result = Nothing
				.ESRArt = 1
				.MWSTNr = mwStrNr
				.PrintedDate = Nothing
				.GebuchtAm = Nothing
				.Ma3RepeatNr = 0
				.EsEinstufung = String.Empty
				.DTAName = Nothing
				.DTAPLZOrt = Nothing
				.DTAKonto = Nothing
				.IBANDTA = Nothing
				.IBANVG = Nothing
				.ESRBankID = initdataPage2.BankData.ID
				.EsrIBAN1 = initdataPage2.BankData.ESRIBAN1
				.EsrIBAN2 = initdataPage2.BankData.ESRIBAN2
				.EsrSwift = initdataPage2.BankData.Swift
				.ProposeNr = Nothing
				.MA0 = Nothing
				.MA1 = Nothing
				.MA2 = Nothing
				.MA3 = Nothing
				.MahnStopUntil = Nothing
				.REDoc_Guid = String.Empty
				.Transfered_User = String.Empty
				.Transfered_On = String.Empty
				.ZEBis0 = Nothing
				.ZEBis1 = Nothing
				.ZEBis2 = Nothing
				.ZEBis3 = Nothing
			End With

			Dim refNr As String = String.Empty
			Dim refFootNr As String = String.Empty

			Dim customerREAddresss = initdataPage2.CustomerReAddress

			' Update invoice data
			With m_NewInvoiceData
				.KdNr = initdataPage2.Customer.KDNr
				.KST = initDataPage1.CombinedAdvisorString
				.ESRBankName = initdataPage2.BankData.BankName
				.KontoNr = initdataPage2.BankData.KontoESR2
				.Lp = CType(initdataPage2.DateValue.Value.Date, Date).Month
				.FakDat = CType(initdataPage2.DateValue.Value.Date, Date)
				.Currency = CurrencySetting
				.Faellig = If(Not initdataPage2.DueDate.HasValue, Nothing, initdataPage2.DueDate.Value.Date)
				.Mahncode = customerREAddresss.MahnCode
				.RName1 = customerREAddresss.REFirma
				.RName2 = customerREAddresss.REFirma2
				.RName3 = customerREAddresss.REFirma3
				.RZHD = customerREAddresss.REZhd
				.RAbteilung = customerREAddresss.REAbteilung
				.RPostfach = customerREAddresss.REPostfach
				.RStrasse = customerREAddresss.REStrasse
				.RLand = customerREAddresss.RELand
				.RPLZ = customerREAddresss.REPLZ
				.ROrt = customerREAddresss.REOrt
				.ReMail = customerREAddresss.REeMail
				.SendAsZip = customerREAddresss.SendAsZip
				.Zahlkond = customerREAddresss.PaymentCondition
				.RefNr = refNr
				.RefFootNr = refFootNr
				.ZEInfo = String.Empty
				.ChangedOn = DateTime.Now
				.ChangedFrom = m_InitializationData.UserData.UserFullName
				.KDBranche = String.Empty
				.MWSTProz = MwStAnsatz
				.MA0 = Nothing
				.MA1 = Nothing
				.MA2 = Nothing
				.MA3 = Nothing
			End With

    End Sub

#End Region

  End Class

End Namespace

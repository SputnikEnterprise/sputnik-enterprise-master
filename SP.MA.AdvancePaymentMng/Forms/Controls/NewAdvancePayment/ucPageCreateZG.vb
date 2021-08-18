
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects
Imports SP.Infrastructure.Misc
Imports SP.DatabaseAccess.Employee.DataObjects.Salary

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath


Namespace UI

  Public Class ucPageCreateZG


#Region "Private Consts"
		Private Const LANR_CHECK As Integer = 8900
		Private Const LANR_BANK_TRANSFER As Integer = 8920
		Private Const LANR_BAR As Integer = 8930

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
#End Region


#Region "Private Fields"

    ''' <summary>
    ''' The ZGNr of the newly created ZG.
    ''' </summary>
    Private m_ZGNrOfNewlyCreatedAdvancePayment As Integer?

		Private m_PrintAdvancePayment As Boolean?
		Private m_OpenAdvancePaymentForm As Boolean?
		Private m_Mandant As Mandant

		''' <summary>
		''' Months.
		''' </summary>
    Private m_Months() As String

#End Region


#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			m_Mandant = New Mandant

		End Sub

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets the advance payment print 8900, 8930 setting.
		''' </summary>
		Private ReadOnly Property AdvancepaymentPrintAfterCreate8900AND8930 As Boolean
			Get

				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim result As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																																String.Format("{0}/advancepaymentprintaftercreate8900and8930", FORM_XML_MAIN_KEY)), False)

				Return result.HasValue AndAlso result
			End Get
		End Property

		''' <summary>
		''' Gets the advance payment print 8920 setting.
		''' </summary>
		Private ReadOnly Property AdvancepaymentPrintAfterCreate8920 As Boolean
			Get

				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim result As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																																String.Format("{0}/advancepaymentprintaftercreate8920", FORM_XML_MAIN_KEY)), False)

				Return result.HasValue AndAlso result
			End Get
		End Property

		''' <summary>
		''' Gets the advance payment open form setting.
		''' </summary>
		Private ReadOnly Property AdvancepaymentOpenformAfterCreate As Boolean
			Get

				Dim mandantNumber = m_UCMediator.SelectedCandidateAndAdvisorData.MandantData.MandantNumber
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim result As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																																																String.Format("{0}/advancepaymentopenformaftercreate", FORM_XML_MAIN_KEY)), False)

				Return result.HasValue AndAlso result
			End Get
		End Property

#End Region


#Region "Public Properties"

    ''' <summary>
    ''' Gets the ZGNr of the newly created ZG.
    ''' </summary>
    Public ReadOnly Property ZGNrOfNewlyCreatedAdvancePayment As Integer?
      Get
        Return m_ZGNrOfNewlyCreatedAdvancePayment
      End Get

    End Property

		''' <summary>
		''' Gets the ZGNr of the newly created ZG.
		''' </summary>
		Public ReadOnly Property PrintAdvancePayment As Boolean?
			Get
				Return m_PrintAdvancePayment
			End Get

		End Property

		''' <summary>
		''' Gets the ZGNr of the newly created ZG.
		''' </summary>
		Public ReadOnly Property OpenAdvancePaymentForm As Boolean?
			Get
				Return m_OpenAdvancePaymentForm
			End Get

		End Property

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Reads the ZG offset from the settings.
    ''' </summary>
    ''' <returns>ZG offset or zero if it could not be read.</returns>
    Private Function ReadZGOffsetFromSettings() As Integer

      Dim strQuery As String = "//StartNr/Vorschussverwaltung"
      Dim r = m_ClsProgSetting.GetUserProfileFile
      Dim zgNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
      Dim intVal As Integer

      If Integer.TryParse(zgNumberStartNumberSetting, intVal) Then
        Return intVal
      Else
        Return 0
      End If

    End Function


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

      If m_IsFirstPageActivation Then

        m_Months = New String() {m_Translate.GetSafeTranslationValue("Januar"), m_Translate.GetSafeTranslationValue("Februar"), m_Translate.GetSafeTranslationValue("März"),
                                 m_Translate.GetSafeTranslationValue("April"), m_Translate.GetSafeTranslationValue("Mai"), m_Translate.GetSafeTranslationValue("Juni"),
                                 m_Translate.GetSafeTranslationValue("Juli"), m_Translate.GetSafeTranslationValue("August"), m_Translate.GetSafeTranslationValue("September"),
                                 m_Translate.GetSafeTranslationValue("Oktober"), m_Translate.GetSafeTranslationValue("November"), m_Translate.GetSafeTranslationValue("Dezember")}

      End If

      Dim candidateAndAdvisorData = m_UCMediator.SelectedCandidateAndAdvisorData
			Dim paymentData = m_UCMediator.SelectedPaymentData

      lblMandantValue.Text = candidateAndAdvisorData.MandantData.MandantName1
      lblBeraterValue.Text = If(Not candidateAndAdvisorData.AdvisorData Is Nothing, candidateAndAdvisorData.AdvisorData.UserFullname, String.Empty)
      lblMitarbeiterValue.Text = String.Format("{0} {1}", candidateAndAdvisorData.EmployeeData.Lastname, candidateAndAdvisorData.EmployeeData.Firstname)

      lblJahrValue.Text = paymentData.Year
      lblMonatValue.Text = m_Months(paymentData.Month - 1)
      lblZahlumAmValue.Text = String.Format("{0:dd.MM.yyyy}", paymentData.PaymentAtDate.Date)
      lblZahlartValue.Text = paymentData.PaymentTypeText

      lblBetragValue.Text = String.Format("{0:N2} {1}", paymentData.AmountOfPayment, candidateAndAdvisorData.EmployeeLOSettingData.Currency)
      lblZahlungsgrundValue.Text = paymentData.PaymentReason
      chkGebAbzug.Checked = paymentData.GebAbzug

			lblWarningValue.Visible = paymentData.AmountExceeded
			lblWarning.Visible = paymentData.AmountExceeded

			If paymentData.PaymentType = LANR_BANK_TRANSFER Then
				chkPrintDocumentAfterCreate.Checked = AdvancepaymentPrintAfterCreate8920
			Else
				chkPrintDocumentAfterCreate.Checked = AdvancepaymentPrintAfterCreate8900AND8930
			End If
			chkOpenMainForm.Checked = AdvancepaymentOpenformAfterCreate

			m_IsFirstPageActivation = False

			Return success
		End Function

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_IsFirstPageActivation = True
      m_ZGNrOfNewlyCreatedAdvancePayment = Nothing

			m_PrintAdvancePayment = Nothing
			m_OpenAdvancePaymentForm = Nothing

      lblMandantValue.Text = String.Empty
      lblBeraterValue.Text = String.Empty
      lblMitarbeiterValue.Text = String.Empty

      lblJahrValue.Text = String.Empty
      lblMonatValue.Text = String.Empty
      lblZahlartValue.Text = String.Empty
      lblZahlartValue.Text = String.Empty
      lblBeraterValue.Text = String.Empty
      lblZahlungsgrundValue.Text = String.Empty
      chkGebAbzug.Checked = False
      chkGebAbzug.Properties.ReadOnly = True

      '  Reset drop downs and lists

      ErrorProvider.Clear()

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean

      ErrorProvider.Clear()

      Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

      Dim isValid As Boolean = True


      Return isValid

    End Function

#End Region

#Region "Translation"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      'Captions
      Me.gpEigenschaften.Text = m_Translate.GetSafeTranslationValue(Me.gpEigenschaften.Text)
      Me.grpAuszahlung.Text = m_Translate.GetSafeTranslationValue(Me.grpAuszahlung.Text)
			Me.grpAbschluss.Text = m_Translate.GetSafeTranslationValue(Me.grpAbschluss.Text)

      Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
      Me.lblBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblBerater.Text)
      Me.lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(Me.lblMitarbeiter.Text)

      Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
      Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
      Me.lblZahlungAm.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlungAm.Text)
      Me.lblZahlArt.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlArt.Text)
      Me.lblBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblBetrag.Text)
      Me.lblZahlungsgrund.Text = m_Translate.GetSafeTranslationValue(Me.lblZahlungsgrund.Text)
      Me.chkGebAbzug.Text = m_Translate.GetSafeTranslationValue(Me.chkGebAbzug.Text)

			Me.lblWarning.Text = m_Translate.GetSafeTranslationValue(Me.lblWarning.Text)
			Me.lblWarningValue.Text = m_Translate.GetSafeTranslationValue(Me.lblWarningValue.Text)

			Me.chkPrintDocumentAfterCreate.Text = m_Translate.GetSafeTranslationValue(Me.chkPrintDocumentAfterCreate.Text)
			Me.chkOpenMainForm.Text = m_Translate.GetSafeTranslationValue(Me.chkOpenMainForm.Text)

		End Sub

#End Region

#Region "Reset"


#Region "Create ZG"


    ''' <summary>
    ''' Creates the ZG.
    ''' </summary>
    Public Sub CreateZG()

      Dim dbAccess = m_UCMediator.AdvancePaymentDbAccess

      Dim candidateAndAdvisorData = m_UCMediator.SelectedCandidateAndAdvisorData
			Dim notice = m_UCMediator.SelectedCandidateAndAdvisorData.NoticeAdvancedpayment
			Dim bankData = m_UCMediator.SelectedPaymentData.BankData ' m_UCMediator.SelectedBankData
			Dim paymentData = m_UCMediator.SelectedPaymentData
      Dim numberToWordConverter As New NumberToWord()
      Dim dateNow = DateTime.Now


      ' Digits for 1, 10, 100, 1000... columns
      Dim amountInteger As Integer = Math.Floor(Math.Abs(paymentData.AmountOfPayment))
      Dim amountString As String = amountInteger.ToString
      Dim digits(amountString.Length - 1) As String

      For i As Integer = 0 To amountString.Length - 1
        digits(i) = amountString.Substring(i, 1)
      Next i
      Dim zgMasterData = New ZGMasterData

      zgMasterData.RPNR = If(Not PreselectionData Is Nothing, PreselectionData.RPNr, 0)
      zgMasterData.MANR = candidateAndAdvisorData.EmployeeData.EmployeeNumber
      zgMasterData.LANR = paymentData.PaymentType
      zgMasterData.LONR = 0
      zgMasterData.VGNR = 0
      zgMasterData.ZGGRUND = paymentData.PaymentReason
      zgMasterData.Betrag = paymentData.AmountOfPayment * (-1)
      zgMasterData.Anzahl = 1 '
      zgMasterData.Ansatz = 100
      zgMasterData.Basis = paymentData.AmountOfPayment * (-1)
      zgMasterData.Currency = candidateAndAdvisorData.EmployeeLOSettingData.Currency
      zgMasterData.LP = paymentData.Month
      zgMasterData.JAHR = paymentData.Year.ToString()
			zgMasterData.Aus_Dat = paymentData.PaymentAtDate.Date

			If Not bankData Is Nothing Then
				' Bank clearing number
				Dim bankClearingNumber As Integer = 0
				If Not String.IsNullOrWhiteSpace(bankData.DTABCNR) Then
					Integer.TryParse(bankData.DTABCNR, bankClearingNumber)
				End If
				zgMasterData.ClearingNr = bankClearingNumber
				zgMasterData.Bank = bankData.Bank
				zgMasterData.KontoNr = bankData.AccountNr
				zgMasterData.BankOrt = bankData.BankLocation
				zgMasterData.DTAAdr1 = bankData.DTAAdr1
				zgMasterData.DTAAdr2 = bankData.DTAAdr2
				zgMasterData.DTAAdr3 = bankData.DTAAdr3
				zgMasterData.DTAAdr4 = bankData.DTAAdr4

				zgMasterData.BnkAU = bankData.BankAU
				zgMasterData.IBANNr = bankData.IBANNr
				zgMasterData.Swift = bankData.Swift
				zgMasterData.BLZ = bankData.BLZ
			End If
			zgMasterData.N2Char = numberToWordConverter.ConvertNumberToGermanText(paymentData.AmountOfPayment)
			zgMasterData._1000000 = If(digits.Count > 6, numberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 7)), String.Empty)
			zgMasterData._100000 = If(digits.Count > 5, numberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 6)), String.Empty)
			zgMasterData._10000 = If(digits.Count > 4, numberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 5)), String.Empty)
			zgMasterData._1000 = If(digits.Count > 3, numberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 4)), String.Empty)
			zgMasterData._100 = If(digits.Count > 2, numberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 3)), String.Empty)
			zgMasterData._10 = If(digits.Count > 1, numberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 2)), String.Empty)
			zgMasterData._1 = If(digits.Count > 0, numberToWordConverter.ConvertNumberToGermanText(digits(digits.Length - 1)), String.Empty)
			zgMasterData.USName = m_InitializationData.UserData.UserFullName
			zgMasterData.Result = String.Empty
			zgMasterData.CheckNumber = String.Empty
			zgMasterData.GebAbzug = paymentData.GebAbzug
			zgMasterData.CreatedOn = dateNow
			zgMasterData.CreatedFrom = m_InitializationData.UserData.UserFullName
			zgMasterData.ChangedOn = dateNow
			zgMasterData.ChangedFrom = m_InitializationData.UserData.UserFullName
			zgMasterData.DTA_Dat = Nothing
			zgMasterData.DTADate = Nothing
			zgMasterData.Printed_Dat = Nothing
			zgMasterData.MDNr = candidateAndAdvisorData.MandantData.MandantNumber

			Dim success = dbAccess.AddNewZGData(zgMasterData, ReadZGOffsetFromSettings)

			m_ZGNrOfNewlyCreatedAdvancePayment = If(success, zgMasterData.ZGNr, Nothing)

			If success AndAlso Not String.IsNullOrWhiteSpace(notice) AndAlso Not notice.StartsWith(vbNewLine) Then
				Dim employeeNoticeData = m_UCMediator.EmployeeDbAccess.LoadEmployeeNoticesData(zgMasterData.MANR)

				If Not employeeNoticeData Is Nothing Then
					employeeNoticeData.Notice_AdvancedPayment = notice
					m_UCMediator.EmployeeDbAccess.UpdateEmployeeNoticesData(employeeNoticeData)
				End If

			End If

			m_PrintAdvancePayment = chkPrintDocumentAfterCreate.Checked
			m_OpenAdvancePaymentForm = chkOpenMainForm.Checked

		End Sub

#End Region

#End Region



	End Class

End Namespace

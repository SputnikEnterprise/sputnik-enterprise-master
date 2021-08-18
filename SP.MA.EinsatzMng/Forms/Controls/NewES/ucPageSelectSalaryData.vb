Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SPS.SalaryValueCalculation
Imports SP.MA.EinsatzMng.UI.ucExactSalaryValues
Imports SPS.SalaryValueCalculation.ESSalaryValueCalculation
Imports SP.DatabaseAccess.Customer
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports DevExpress.XtraEditors

Namespace UI

  Public Class ucPageSelectSalaryData

#Region "Private Consts"

    ' TODO: MWST müsste von XML kommen
    Private Const MWST_PERCENT As Decimal = 8D ' 8.0%
    Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
    Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

#Region "Private Fields"

		Private m_SelectedGAVStringData As GAVStringData
		Private m_EffectivGAVData As EffectiveGAVData
		Private m_MargeData As MargeStringData

		Private m_ESSalaryValueCalculator As ESSalaryValueCalculation
		Private m_ucExactSalaryValues As ucExactSalaryValues

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

		Private m_path As ClsProgPath

		Private m_DidUserConfirmOverrideOfTagespesen As Boolean = False

		Private m_AllowedTempDataPVL As Boolean

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      Try
        ' Mandantendaten
        m_Mandant = New Mandant
        m_path = New ClsProgPath
				m_AllowedTempDataPVL = True

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

      End Try

      m_ESSalaryValueCalculator = New ESSalaryValueCalculation
      m_ucExactSalaryValues = New ucExactSalaryValues

      popupContainerExactSalaryValues.Controls.Add(m_ucExactSalaryValues)

      m_MargeData = New MargeStringData

      AddHandler lueCustomerKST.ButtonClick, AddressOf OnDropDown_ButtonClick
      AddHandler dateEditSalaryDataFrom.ButtonClick, AddressOf OnDropDown_ButtonClick

    End Sub

#End Region

#Region "Private Properties"

		Private Property CountOfWarning As Integer

		Private ReadOnly Property GetDefaultMwStAnsatz(ByVal mdYear As Integer) As Decimal
			Get

				Dim mdNumber = m_InitializationData.MDData.MDNr

				If mdYear <= Now.Year - 10 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)

			End Get

		End Property

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected salary data.
		''' </summary>
		''' <returns>Salary data.</returns>
		Public ReadOnly Property SelectedESSalaryData As InitESSalaryData
			Get

				Dim data As New InitESSalaryData With {
			  .FarPflichtig = chkFarPflichtig.Checked,
			  .GrundLohn = txtGrundlohn.EditValue,
			  .FeiertagBasis = txtBasisFeiertag.EditValue,
			  .FeiertagAnsatz = txtAnsatzFeiertag.EditValue,
			  .FeiertagBetrag = txtBetragFeiertag.EditValue,
			  .FerienBasis = txtBasisFerien.EditValue,
			  .FerienAnsatz = txtAnsatzFerien.EditValue,
			  .FerienBetrag = txtBetragFerien.EditValue,
			  .Lohn13Basis = txtBasisLohn13.EditValue,
			  .Lohn13Ansatz = txtAnsatzLohn13.EditValue,
			  .Lohn13Betrag = txtBetragLohn13.EditValue,
			  .StundenLohn = txtStundenLohn.EditValue,
			  .Lohnspesen = txtLohnspesen.EditValue,
			  .TagespesenMA = txtTagesspesenMA.EditValue,
			  .Tarif = txtTarif.EditValue,
			  .TagesspesenKD = txtTagespesenKD.EditValue,
			  .MwStPflicht = chkMwStPflichtig.Checked,
			  .MwStBetrag = txtMwstBetrag.EditValue,
			  .CalcFeierWay = CalcFeierWay,
			  .CalcFerienWay = CalcFerienWay,
			  .CalcLohn13Way = CalcLohn13Way,
			  .CustomerKST = lueCustomerKST.EditValue,
			  .CustomerKSTBez = If(lueCustomerKST.EditValue Is Nothing, String.Empty, CType(lueCustomerKST.GetSelectedDataRow(), KSTViewData).Description),
			  .LOVon = dateEditSalaryDataFrom.EditValue,
			  .SelectedGAVStringData = m_SelectedGAVStringData,
			  .EffectiveGAVData = m_EffectivGAVData,
			  .MargeData = m_MargeData,
			  .DidUserConfirmOverrideOfTagespesen = m_DidUserConfirmOverrideOfTagespesen
			}

				Return data
			End Get
		End Property

		''' <summary>
		''' Gets boolean flag indicating if a GAV is selected.
		''' </summary>
		Public ReadOnly Property IsGAVSelected
      Get
        Return Not m_SelectedGAVStringData Is Nothing
      End Get
    End Property


    ''' <summary>
    ''' Gets the CalcFeierWay.
    ''' </summary>
    Public ReadOnly Property CalcFeierWay As Integer
      Get

        If Not m_SelectedGAVStringData Is Nothing Then
          Return m_SelectedGAVStringData.CalcFeier
        End If

        Return 0
      End Get
    End Property

    ''' <summary>
    ''' Gets the CalcFerienWay.
    ''' </summary>
    Public ReadOnly Property CalcFerienWay As Integer
      Get

        If Not m_SelectedGAVStringData Is Nothing Then
          Return m_SelectedGAVStringData.CalcFerien
        End If

        Dim mdnr = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber
        Dim escalcferienwayXML As Integer = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr),
                                                                       String.Format("{0}/escalcferienway", FORM_XML_MAIN_KEY)))

        Return escalcferienwayXML
      End Get
    End Property

    ''' <summary>
    ''' Gets the CalcLohn13Way.
    ''' </summary>
    Public ReadOnly Property CalcLohn13Way As Integer
      Get

        If Not m_SelectedGAVStringData Is Nothing Then
          Return m_SelectedGAVStringData.Calc13
        End If

        Dim mdnr = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber
        Dim escalc13lohnwayXml = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdnr), String.Format("{0}/escalc13lohnway", FORM_XML_MAIN_KEY)))

        Return escalc13lohnwayXml
      End Get
    End Property

    ''' <summary>
    ''' Gets or sets the ESNr.
    ''' </summary>
    Public Property ESNr As Integer?

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Inits the control with configuration information.
    ''' </summary>
    '''<param name="initializationClass">The initialization class.</param>
    '''<param name="translationHelper">The translation helper.</param>
    Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
      MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			m_ucExactSalaryValues.TranslateControls(translationHelper)
			lblKostenstelle.Text = m_Translate.GetSafeTranslationValue(lblKostenstelle.Text)
			lblSpesenWarning.Text = m_Translate.GetSafeTranslationValue(lblSpesenWarning.Text)

    End Sub

    ''' <summary>
    ''' Activates the page.
    ''' </summary>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function ActivatePage() As Boolean

      Dim success As Boolean = True

      If m_IsFirstPageActivation Then

        Dim customerData = m_UCMediator.SelectedCandidateAndCustomerData.CustomerData

        success = success AndAlso LoadDropDownData(customerData.CustomerNumber)
        success = success AndAlso PreselectData()

      End If

      If m_EffectivGAVData Is Nothing Then
        ' Creates a default effective GAV value object.
        UpdateEffectiveGAVValuesBasedOnGavStringStundenLohnAndFarPflichtig()
      Else
        CalcMarge()
      End If

			' must be resetet!
			CountOfWarning = 0

			m_SuppressUIEvents = False
      m_IsFirstPageActivation = False

      Return success
    End Function

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_IsFirstPageActivation = True
      m_SelectedGAVStringData = Nothing
      m_EffectivGAVData = Nothing
      m_MargeData = New MargeStringData

      ' Prevent UI events in this section
      Dim suppressUIState = m_SuppressUIEvents
      m_SuppressUIEvents = True

      chkFarPflichtig.Checked = False
      chkFarPflichtig.Properties.ReadOnly = True
      txtGAVVertrag.Text = "1 (-)"

      ' Grundlohn
      txtGrundlohn.EditValue = 0D
      txtGrundlohn.Properties.ReadOnly = False

      ' Feiertag
      txtBasisFeiertag.EditValue = 0D
      txtAnsatzFeiertag.EditValue = 0D
      txtBetragFeiertag.EditValue = 0D
      txtBasisFeiertag.Properties.ReadOnly = True
      txtAnsatzFeiertag.Properties.ReadOnly = False
      txtBetragFeiertag.Properties.ReadOnly = True

      ' Ferien
      txtBasisFerien.EditValue = 0D
      txtAnsatzFerien.EditValue = 0D
      txtBetragFerien.EditValue = 0D
      txtBasisFerien.Properties.ReadOnly = True
      txtAnsatzFerien.Properties.ReadOnly = False
      txtBetragFerien.Properties.ReadOnly = True

      ' 13. Lohn
      txtBasisLohn13.EditValue = 0D
      txtAnsatzLohn13.EditValue = 0D
      txtBetragLohn13.EditValue = 0D
      txtBasisLohn13.Properties.ReadOnly = True
      txtAnsatzLohn13.Properties.ReadOnly = False
      txtBetragLohn13.Properties.ReadOnly = True

      txtStundenLohn.EditValue = 0D
      txtLohnspesen.EditValue = 0D
      txtTagesspesenMA.EditValue = 0D

      txtTarif.EditValue = 0D
      txtTagespesenKD.EditValue = 0D
      chkMwStPflichtig.Checked = False
			chkMwStPflichtig.Visible = False
			txtMwstBetrag.EditValue = 0D

      lblMargeOhneBVGValue.Text = "0.0"
      lblMargeMitBVGValue.Text = "0.0"
      lblMargeOhneBVGProzValue.Text = "0.0"
      lblMargeMitBVGProzValue.Text = "0.0"

      btnUnlockTagespesen.Visible = False
      lblSpesenWarning.Visible = False

      dateEditSalaryDataFrom.EditValue = Nothing

      m_SuppressUIEvents = suppressUIState

			'Dim isAuthorizedForCode267 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 267, m_InitializationData.MDData.MDNr)
			txtGAVVertrag.Properties.Buttons(1).Visible = False 'isAuthorizedForCode267

			'  Reset drop downs and lists

			ResetCustomerKSTDropDown()

			' Clear errors
			DxErrorProvider1.ClearErrors()

		End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean

			DxErrorProvider1.ClearErrors()

			Dim bWarned As Boolean = False
			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorTextDateSalaryMustBeToday As String = m_Translate.GetSafeTranslationValue("Das Datum darf nicht vor oder nach Einsatzbeginn {0:d} liegen.")
			Dim errorTextDateSalaryMustBeBeforeEndDate As String = m_Translate.GetSafeTranslationValue("Das Datum darf nicht nach Einsatzende {0:d} liegen.")
			Dim isAuthorizedForCode261 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 261, m_InitializationData.MDData.MDNr)
			Dim errorMwStText As String = m_Translate.GetSafeTranslationValue("Achtung: Der Kunde ist MwSt.frei!")

			Dim isValid As Boolean = True

			isValid = isValid And SetDXErrorIfInvalid(lueCustomerKST, DxErrorProvider1, lueCustomerKST.EditValue Is Nothing, errorText)
			isValid = isValid And SetDXErrorIfInvalid(txtTarif, DxErrorProvider1, Not isAuthorizedForCode261 AndAlso (txtTarif.EditValue Is Nothing Or txtTarif.EditValue = 0), errorText)

			Dim ESBegin As Date? = m_UCMediator.SelectedESData.ESStartDate
			Dim ESEnd As Date? = m_UCMediator.SelectedESData.ESEndDate

			If ESNr Is Nothing Then
				isValid = isValid And SetDXErrorIfInvalid(dateEditSalaryDataFrom, DxErrorProvider1,
																								(dateEditSalaryDataFrom.EditValue Is Nothing OrElse Year(dateEditSalaryDataFrom.EditValue) <= 1900) Or (dateEditSalaryDataFrom.EditValue <> ESBegin),
																								String.Format(errorTextDateSalaryMustBeToday, ESBegin))
			Else
				isValid = isValid And SetDXErrorIfInvalid(dateEditSalaryDataFrom, DxErrorProvider1,
																								(dateEditSalaryDataFrom.EditValue Is Nothing OrElse Year(dateEditSalaryDataFrom.EditValue) <= 1900) Or (dateEditSalaryDataFrom.EditValue < ESBegin),
																								String.Format(errorTextDateSalaryMustBeToday, ESBegin))
				If Not ESEnd Is Nothing Then isValid = isValid And SetDXErrorIfInvalid(dateEditSalaryDataFrom, DxErrorProvider1,
																								(dateEditSalaryDataFrom.EditValue Is Nothing OrElse Year(dateEditSalaryDataFrom.EditValue) <= 1900) Or (dateEditSalaryDataFrom.EditValue > ESEnd),
																								String.Format(errorTextDateSalaryMustBeBeforeEndDate, ESEnd))
			End If

			If isValid And IsGAVSelected Then

        ' ---Check the BasisLohn---

        Dim gavBasisLohn As Decimal = m_SelectedGAVStringData.BasisLohn

				If Convert.ToDecimal(txtGrundlohn.Text) < Math.Round(gavBasisLohn, 2) Then
					isValid = False
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(String.Format("Der GAV Grundlohn von {0:n2} CHF ist unterschritten", gavBasisLohn)))
				End If

			End If

      ' Check if GAV must be selected.

      Dim mandantNumber As Integer = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber
      Dim mustGAVBeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
                                                  String.Format("{0}/gavselectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)

			isValid = isValid And SetDXErrorIfInvalid(txtGAVVertrag, DxErrorProvider1, (mustGAVBeSelected And (Not IsGAVSelected)), errorText)

			If (txtMwstBetrag.EditValue) = 0D AndAlso CountOfWarning = 0 Then
				isValid = isValid And SetDXWarningIfInvalid(txtMwstBetrag, DxErrorProvider1, True, errorMwStText)
				bWarned = True
			End If

			If isValid Then

        ' ---Check the marge---

        Dim mdNumber As Integer = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber
        Dim mdYear As Integer = Year(dateEditSalaryDataFrom.EditValue)
        Dim boundaryValues As MandantMargeBoundaryValues = m_UCMediator.ESDbAccess.LoadMargeBoundaryValuesForMandant(mdNumber, mdYear)

        If boundaryValues Is Nothing Then
          isValid = False
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Margen Grenzwerte konnten nicht geladen werden."))
        Else

          Const MARGE_BOUNDARY_DEFAULT As Integer = 7 ' CHF

          Dim isMargeBelowBoundary As Boolean = False
          Dim infoString As String = String.Empty

          If (boundaryValues.B_Marge > 0D) And (m_MargeData.MargeMitBVG < boundaryValues.B_Marge) Then
            isMargeBelowBoundary = True
            infoString = String.Format(m_Translate.GetSafeTranslationValue("Der Margengrenzwert von {0:0.00} CHF ist unterschritten."), boundaryValues.B_Marge)
          ElseIf (boundaryValues.B_Marge = 0D) And (boundaryValues.B_MargeP > 0) And (m_MargeData.MargeWithBVGInProz < boundaryValues.B_MargeP) Then
            isMargeBelowBoundary = True
            infoString = String.Format(m_Translate.GetSafeTranslationValue("Der Margengrenzwert von {0:0.00}% ist unterschritten."), boundaryValues.B_MargeP)
          ElseIf (boundaryValues.B_Marge = 0D) And (boundaryValues.B_MargeP = 0D) And (m_MargeData.MargeMitBVG < MARGE_BOUNDARY_DEFAULT) Then
            isMargeBelowBoundary = True
            infoString = String.Format(m_Translate.GetSafeTranslationValue("Der Margengrenzwert von {0:0.00} CHF ist unterschritten."), MARGE_BOUNDARY_DEFAULT)
          End If

          If isMargeBelowBoundary Then

						Dim isAuthorizedForCode262 = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 262, m_InitializationData.MDData.MDNr)
            If Not isAuthorizedForCode262 Then
              isValid = False
              m_UtilityUI.ShowInfoDialog(infoString)

            Else

              ' Marge is exceeded but user is allowed to continue. Ask the user if he wants to continue.
              If Not (m_UtilityUI.ShowYesNoDialog(infoString, m_Translate.GetSafeTranslationValue("Fortfahren?"))) Then
                isValid = False
              End If

            End If

          End If

        End If

      End If
			If bWarned Then CountOfWarning += 1

			Return isValid

    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.chkFarPflichtig.Text = m_Translate.GetSafeTranslationValue(Me.chkFarPflichtig.Text)
      Me.lblGAVVertrag.Text = m_Translate.GetSafeTranslationValue(Me.lblGAVVertrag.Text)
      Me.lblGrundlohn.Text = m_Translate.GetSafeTranslationValue(Me.lblGrundlohn.Text)
      Me.lblFeiertag.Text = m_Translate.GetSafeTranslationValue(Me.lblFeiertag.Text)
      Me.lblFerien.Text = m_Translate.GetSafeTranslationValue(Me.lblFerien.Text)
      Me.lblLohn13.Text = m_Translate.GetSafeTranslationValue(Me.lblLohn13.Text)
      Me.lblStundenlohn.Text = m_Translate.GetSafeTranslationValue(Me.lblStundenlohn.Text)
      Me.lblLohnspesen.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnspesen.Text)
      Me.lblTagespesen.Text = m_Translate.GetSafeTranslationValue(Me.lblTagespesen.Text, True)

      Me.lblTarif.Text = m_Translate.GetSafeTranslationValue(Me.lblTarif.Text)
      Me.lblTagesspesenKunde.Text = m_Translate.GetSafeTranslationValue(Me.lblTagesspesenKunde.Text, True)
			Me.lblMwStBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblMwStBetrag.Text)

      Me.lblMargeOhneBVG.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeOhneBVG.Text)
      Me.lblMargeMitBVG.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeMitBVG.Text)
      Me.lblMargeOhneBVGProz.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeOhneBVGProz.Text)
      Me.lblMargeMitBVGProz.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeMitBVGProz.Text)

      Me.lblSpesenWarning.Text = m_Translate.GetSafeTranslationValue(Me.lblSpesenWarning.Text)
      Me.lblLohndatenVon.Text = m_Translate.GetSafeTranslationValue(Me.lblLohndatenVon.Text)

    End Sub

    ''' <summary>
    ''' Resets the customer KST drop down.
    ''' </summary>
    Private Sub ResetCustomerKSTDropDown()

      lueCustomerKST.Properties.DropDownRows = 20

      lueCustomerKST.Properties.DisplayMember = "Description"
      lueCustomerKST.Properties.ValueMember = "RecordNumber"

      Dim columns = lueCustomerKST.Properties.Columns
      columns.Clear()
      columns.Add(New LookUpColumnInfo("Description", 0, String.Empty))

      lueCustomerKST.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueCustomerKST.Properties.SearchMode = SearchMode.AutoComplete
      lueCustomerKST.Properties.AutoSearchColumnIndex = 1

      lueCustomerKST.Properties.NullText = String.Empty
      lueCustomerKST.EditValue = Nothing

    End Sub


    ''' <summary>
    ''' Loads drop down data.
    ''' </summary>
    '''<param name="customerNumber">The customer number.</param>
    Private Function LoadDropDownData(ByVal customerNumber As Integer) As Boolean

      Dim success As Boolean = True
      success = success AndAlso LoadCustomerKSTDropdowndata(customerNumber)

      Return success
    End Function

    ''' <summary>
    ''' Loads customer KST drop down data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadCustomerKSTDropdowndata(ByVal customerNumber As Integer) As Boolean

      Dim assignedKSTs = m_UCMediator.CustomerDbAccess.LoadAssignedKSTsOfCustomer(customerNumber)

      If assignedKSTs Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kostenstellen konnten nicht geladen werden."))

        lueCustomerKST.Properties.DataSource = Nothing
        lueCustomerKST.Properties.ForceInitialize()

        Return False
      Else

        Dim listDataSource As BindingList(Of KSTViewData) = New BindingList(Of KSTViewData)

        ' Convert the data to view data.
        For Each kst In assignedKSTs

          Dim invoiceAddressViewData = New KSTViewData() With {
                  .Id = kst.ID,
                  .RecordNumber = kst.RecordNumber,
                  .Description = kst.Description
                  }

          listDataSource.Add(invoiceAddressViewData)
        Next

        lueCustomerKST.Properties.DataSource = listDataSource
        lueCustomerKST.Properties.ForceInitialize()

        Return True
      End If

    End Function

    ''' <summary>
    ''' Loads salary calculation percentage values form database.
    ''' </summary>
    ''' <param name="age">The age of the employee.</param>
    Private Function LoadSalaryCalculationPercentageValues(ByVal age As Integer) As Boolean

      Dim percentageValues As SalaryCalculationPercentageValues = m_UCMediator.ESDbAccess.LoadSalaryCalculationPercentageValues(age)

      If percentageValues Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(String.Format("Ferien, Feiertag und 13.Monatslohn-Prozentsätze für das Lebensalter von {0} Jahren sind nicht vorhanden!", age)))
			Else
        txtAnsatzFeiertag.EditValue = percentageValues.FeierProzenSatz
        txtAnsatzFerien.EditValue = percentageValues.FerProzentSatz
        txtAnsatzLohn13.EditValue = percentageValues.ProzenzSat13Lohn
      End If

      Return True
    End Function

    ''' <summary>
    ''' Handles leave event on stunden lohn.
    ''' </summary>
    Private Sub OnTxtStundenLohn_Leave(sender As System.Object, e As System.EventArgs) Handles txtStundenLohn.Leave
      CalcBaseSalaryAndThenValues()
      UpdateEffectiveGAVValuesBasedOnGavStringStundenLohnAndFarPflichtig()
    End Sub

    ''' <summary>
    ''' Handles leave event of Grundlohn, Basis and Ansatz textboxes.
    ''' </summary>
    Private Sub OnTxtGrundlohnOrBasisOrAnsatz_Leave(sender As System.Object, e As System.EventArgs) Handles txtGrundlohn.Leave, txtBasisFeiertag.Leave, txtBasisFerien.Leave, txtBasisLohn13.Leave, _
                                                                                                  txtAnsatzFeiertag.Leave, txtAnsatzFerien.Leave, txtAnsatzLohn13.Leave
      CalcValuesFromBaseSalary(True)
      CalcMarge()
    End Sub

    ''' <summary>
    ''' Handles click on select GAV button.
    ''' </summary>
    Private Sub OnTxtGAVVertrag_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtGAVVertrag.ButtonClick

      Dim selectedCandidateAndCustomer = m_UCMediator.SelectedCandidateAndCustomerData

      Dim customer = m_UCMediator.SelectedCandidateAndCustomerData.CustomerData
      Dim mdNr = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber

      Dim mdCanton = m_Mandant.GetMDData4SelectedMD(mdNr, DateTime.Now.Year).MDCanton
      Dim cantonFromDB = m_UCMediator.CommonDbAccess.LoadCantonByPostCode(customer.Postcode)

      Dim canton As String = If(Not String.IsNullOrEmpty(cantonFromDB), cantonFromDB, mdCanton)

			'If e.Button.Index = 0 Then
			If canton <> "FL" Then
					OpenGAVDataForm(ConstantValues.ModulName,
												selectedCandidateAndCustomer.EmployeeData.EmployeeNumber,
												selectedCandidateAndCustomer.CustomerData.CustomerNumber, canton, String.Empty)
				Else
					OpenIndividualGAVDataForm(selectedCandidateAndCustomer.EmployeeData.EmployeeNumber,
                                  selectedCandidateAndCustomer.CustomerData.CustomerNumber,
                                  canton)
      End If

    End Sub

    ''' <summary>
    ''' Handles leave event of txtTarif and txtTagesspesen.
    ''' </summary>
    Private Sub OnTxtTarifAndTagesspesenKD_Leave(sender As System.Object, e As System.EventArgs) Handles txtTarif.Leave,
                                                                                                                    txtTagespesenKD.Leave
      CalcMwstValue()
      CalcMarge()
    End Sub

    ''' <summary>
    ''' Handles change of Mwst pflichtig checkbox.
    ''' </summary>
    Private Sub OnChkMwStPflichtig_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkMwStPflichtig.CheckedChanged
      CalcMwstValue()
    End Sub

    ''' <summary>
    ''' Handles leave event of txtLohnspesen and txtTagesspesenMA
    ''' </summary>
    Private Sub OnTxtLohnspesenAndTagesspesenMA_Leave(sender As System.Object, e As System.EventArgs) Handles txtLohnspesen.Leave, txtTagesspesenMA.Leave
      CalcMarge()
    End Sub

    ''' <summary>
    ''' Handles click on open exact salary values popup button.
    ''' </summary>
    Private Sub OnBtnOpenExactSalaryValuesPopup_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenExactSalaryValuesPopup.Click

      Dim position = Cursor.Position

      Dim exactValues As New ExaclSalaryValues With {.GrundLohn = txtGrundlohn.EditValue,
                                                     .FeiertagBasis = txtBasisFeiertag.EditValue,
                                                     .FeiertagAnsatz = txtAnsatzFeiertag.EditValue,
                                                     .FeiertagBetrag = txtBetragFeiertag.EditValue,
                                                     .FerienBasis = txtBasisFerien.EditValue,
                                                     .FerienAnsatz = txtAnsatzFerien.EditValue,
                                                     .FerienBetrag = txtBetragFerien.EditValue,
                                                     .Lohn13Basis = txtBasisLohn13.EditValue,
                                                     .Lohn13Ansatz = txtAnsatzLohn13.EditValue,
                                                     .Lohn13Betrag = txtBetragLohn13.EditValue,
                                                     .StundenLohn = txtStundenLohn.EditValue,
                                                     .Tagespesen = txtTagesspesenMA.EditValue}


      m_ucExactSalaryValues.SetValues(exactValues)

      popupContainerExactSalaryValues.ShowPopup(position)
    End Sub

    ''' <summary>
    ''' Opens the GAV select from.
    ''' </summary>
    ''' <param name="strModulName">The module name.</param>
    ''' <param name="iMANr">The employee number.</param>
    ''' <param name="iKDNr">The customer number.</param>
    ''' <param name="strKanton">The canton.</param>
    ''' <param name="strOldGAVInfo">The strOldGavInfo.</param>
    Private Sub OpenGAVDataForm(ByVal strModulName As String, _
                           ByVal iMANr As Integer, _
                           ByVal iKDNr As Integer, _
                           ByVal strKanton As String, _
                           ByVal strOldGAVInfo As String)
			Dim result As String = String.Empty
			Dim pvlDatabaseName = String.Empty

			Try

				If m_AllowedTempDataPVL Then
					Dim frmPVL As New SPGAV.UI.frmTempDataPVL(m_InitializationData)

					frmPVL.EmployeeNumber = iMANr
					frmPVL.CustomerNumber = iKDNr
					frmPVL.CustomerCanton = strKanton
					frmPVL.EmploymentNumber = ESNr.GetValueOrDefault(0)
					frmPVL.ExistingGAVInfo = strOldGAVInfo
					frmPVL.Staging = Not (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

					Dim success = frmPVL.LoadData()
					If Not success Then Return

					frmPVL.BringToFront()
					frmPVL.ShowDialog()
					result = frmPVL.GetAssignedPVLData
					pvlDatabaseName = "XML-Query"

				Else
					Dim frmPVL_OLD As New SPGAV.frmGAV_PVL(m_InitializationData)

					frmPVL_OLD.EmployeeNumber = iMANr
					frmPVL_OLD.CustomerNumber = iKDNr
					frmPVL_OLD.EmploymentNumber = ESNr.GetValueOrDefault(0)
					frmPVL_OLD.CustomerCanton = strKanton
					frmPVL_OLD.ExistingGAVInfo = strOldGAVInfo

					frmPVL_OLD.LoadData()
					result = frmPVL_OLD.ShowDialog()
					pvlDatabaseName = frmPVL_OLD.GetAssignedPVLDatabase

				End If

				m_Logger.LogDebug(String.Format("GAVInfo_String: {0}", result))

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("GAV Werte konnten nicht bestimmt werden."))

			End Try

			If String.IsNullOrWhiteSpace(result) Then Return

			' Convert GAV data string to object.
			Dim paresedGAVStirngData As New GAVStringData()
			paresedGAVStirngData.FillFromString(result)
			paresedGAVStirngData.PVLDatabaseName = pvlDatabaseName

			m_SelectedGAVStringData = paresedGAVStirngData

			' Apply GAV values to UI
			ApplyGAVValuesToUI(paresedGAVStirngData)
			UpdateEffectiveGAVValuesBasedOnGavStringStundenLohnAndFarPflichtig()

		End Sub

		Private Sub OpenIndividualGAVDataForm(ByVal iMANr As Integer, ByVal iKDNr As Integer, ByVal strKanton As String)
			Dim strResult As String = String.Empty
			Dim m_CurrentESNr As Integer = 1
			Dim m_CurrentESLohnNr As Integer = 1

			Try

				Try
					Dim frmPVL As New SPGAV.frmGAV_FL(m_InitializationData)

					frmPVL.EmployeeNumber = iMANr
					frmPVL.CustomerNumber = iKDNr
					frmPVL.EmploymentNumber = ESNr.GetValueOrDefault(0)
					frmPVL.CustomerCanton = strKanton

					frmPVL.LoadData()
					strResult = frmPVL.ShowDialog()

				Catch ex As Exception
					m_Logger.LogError(ex.ToString())
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("GAV Werte konnten nicht bestimmt werden."))

				End Try


				If Not String.IsNullOrWhiteSpace(strResult) Then
					Dim paresedGAVStirngData As New GAVStringData()
					paresedGAVStirngData.FillFromString(strResult)
					m_SelectedGAVStringData = paresedGAVStirngData


					' Apply GAV values to UI
					ApplyGAVValuesToUI(paresedGAVStirngData)

					UpdateEffectiveGAVValuesBasedOnGavStringStundenLohnAndFarPflichtig()

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler zum Anzeigen der Zusatzlohndaten."))

			End Try

		End Sub

		''' <summary>
		'''  Applies GAV values to UI.
		''' </summary>
		''' <param name="gavData">The gav data.</param>
		Private Sub ApplyGAVValuesToUI(ByVal gavData As GAVStringData)

      txtGAVVertrag.Text = String.Format("{0}) {1}", gavData.GAVNr, gavData.Gruppe0)

      txtGrundlohn.EditValue = gavData.BasisLohn

      txtAnsatzFeiertag.EditValue = gavData.FeierProz * 100D
      txtAnsatzFerien.EditValue = gavData.FerienProz * 100D
      txtAnsatzLohn13.EditValue = gavData.Proz_Lohn13 * 100D

      Dim gavFeier = gavData.FeierBetrag
      Dim gavFerien = gavData.FerienBetrag
      Dim gavLohn13 = gavData.Betrag_Lohn13

      'txtBetragFeiertag.EditValue = gavData.FeierBetrag
      'txtBetragFerien.EditValue = gavData.FerienBetrag
      'txtBetragLohn13.EditValue = gavData.Betrag_Lohn13

      txtStundenLohn.EditValue = gavData.StdLohn

      CalcValuesFromBaseSalary(False)

    End Sub

    ''' <summary>
    ''' Calculates the base salaray.
    ''' </summary>
    Private Sub CalcBaseSalary()

      Dim grundLohn As Decimal = 0D

      If IsGAVSelected Then
        grundLohn = m_ESSalaryValueCalculator.CalculateGrundLohnWithGAV(txtStundenLohn.EditValue,
                                                                        txtAnsatzFeiertag.EditValue,
                                                                        txtAnsatzFerien.EditValue,
                                                                        txtAnsatzLohn13.EditValue,
                                                                        CalcFeierWay,
                                                                        CalcFerienWay,
                                                                        CalcLohn13Way)
      Else
        grundLohn = m_ESSalaryValueCalculator.CalculateGrundLohnWithoutGAV(txtStundenLohn.EditValue,
                                                                           txtAnsatzFeiertag.EditValue,
                                                                           txtAnsatzFerien.EditValue,
                                                                           txtAnsatzLohn13.EditValue,
                                                                           CalcFerienWay,
                                                                           CalcLohn13Way)

      End If

      txtGrundlohn.EditValue = grundLohn

    End Sub

    ''' <summary>
    ''' Calculates the values from base salary.
    ''' </summary>
    ''' <param name="setStundenLohn">Boolean flag indicating if the Stundenlohn should be upated.</param>
    Private Sub CalcValuesFromBaseSalary(ByVal setStundenLohn As Boolean)

      Dim result As CalcFerFeier13BasisResult = Nothing

      If IsGAVSelected Then
        result = m_ESSalaryValueCalculator.CalcFerFeier13BasisWithGAV(txtGrundlohn.EditValue,
                                                                      txtAnsatzFeiertag.EditValue,
                                                                      txtAnsatzFerien.EditValue,
                                                                      txtAnsatzLohn13.EditValue,
                                                                      CalcFeierWay,
                                                                      CalcFerienWay,
                                                                      CalcLohn13Way)
      Else
        result = m_ESSalaryValueCalculator.CalculateFerFeier13BasisWithoutGAV(txtGrundlohn.EditValue,
                                                                              txtAnsatzFeiertag.EditValue,
                                                                              txtAnsatzFerien.EditValue,
                                                                              txtAnsatzLohn13.EditValue,
                                                                              CalcFerienWay,
                                                                              CalcLohn13Way)
      End If


      txtBetragFeiertag.EditValue = result.FeiertagBetrag
      txtBetragFerien.EditValue = result.FerienBetrag
      txtBetragLohn13.EditValue = result.Lohn13Betrag

      txtBasisFeiertag.EditValue = result.FeiertagBasis
      txtBasisFerien.EditValue = result.FerienBasis
      txtBasisLohn13.EditValue = result.Lohn13Basis

      If setStundenLohn Then
        txtStundenLohn.EditValue = result.BetragSumme
      End If

    End Sub

    ''' <summary>
    ''' Calculates the base salary and then the values.
    ''' </summary>
    Private Sub CalcBaseSalaryAndThenValues()
      CalcBaseSalary()
      CalcValuesFromBaseSalary(False)
    End Sub

    ''' <summary>
    ''' Calculates the marge.
    ''' </summary>
    Private Sub CalcMarge()

      Dim esAb As DateTime?
      Dim mdNumber As Integer
      Dim employeeNubmer As Integer
      Dim customerNumber As Integer
      Dim suva As String = "A1"
      Dim esNumber As Integer

			If txtStundenLohn.EditValue > 0D OrElse txtTarif.EditValue > 0D Then

				Try
					esAb = m_UCMediator.SelectedESData.ESStartDate
				Catch ex As Exception
					m_UtilityUI.ShowErrorDialog("esAb" & vbNewLine & ex.ToString)

				End Try
				Try
					mdNumber = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber
				Catch ex As Exception
					m_UtilityUI.ShowErrorDialog("mdNumber" & vbNewLine & ex.ToString)

				End Try
				Try
					employeeNubmer = m_UCMediator.SelectedCandidateAndCustomerData.EmployeeData.EmployeeNumber
				Catch ex As Exception
					m_UtilityUI.ShowErrorDialog("employeeNubmer" & vbNewLine & ex.ToString)

				End Try
				Try
					customerNumber = m_UCMediator.SelectedCandidateAndCustomerData.CustomerData.CustomerNumber
				Catch ex As Exception
					m_UtilityUI.ShowErrorDialog("customerNumber" & vbNewLine & ex.ToString)

				End Try
				Try
					suva = m_UCMediator.SelectedESData.SUVA
				Catch ex As Exception
					m_UtilityUI.ShowErrorDialog("suva" & vbNewLine & ex.ToString)

				End Try
				Try
					esNumber = If(ESNr.HasValue, ESNr.Value, 1)
				Catch ex As Exception
					m_UtilityUI.ShowErrorDialog("esNumber" & vbNewLine & ex.ToString)

				End Try


				Try
					'  TODO Prüfen ob .SelectedESNr = 1 und .SelectedESLohnNr = 1 Seiteneffekte hat!
					Dim _settingCalcMArge As New SPS.ES.Utility.ClsESDataSetting With {.selectedMDNr = mdNumber, .SelectedYear = esAb.Value.Year,
																																		.SelectedMANr = employeeNubmer,
																																		.SelectedESNr = esNumber,
																																		.SelectedESLohnNr = 1,
																																		.SelectedKDNr = customerNumber,
																																		._dES_Ab = esAb,
																			 ._sbLohn = Convert.ToSingle(txtStundenLohn.EditValue),
																			 ._sMASSpesen = Convert.ToSingle(txtLohnspesen.EditValue),
																			 ._sMATSpesen = Convert.ToSingle(txtTagesspesenMA.EditValue),
																			 ._sKDTarif = Convert.ToSingle(txtTarif.EditValue),
																			 ._sKDTSpesen = Convert.ToSingle(txtTagespesenKD.EditValue),
																			 ._strSuva = suva,
																			 ._sFARProz = If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_FAG),
																			 ._sWAGProz = If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_WAG),
																			 ._sVAGProz = If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_VAG),
																			 ._sWAGBtr = If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_WAG_S),
																			 ._sVAGBtr = If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_VAG_S),
																			 ._strGAVInfo = If(m_EffectivGAVData Is Nothing, String.Empty, m_EffectivGAVData.GAVInfo_String),
																																		.GetMarge4NewES = True}
					Try
						Dim obj As New SPS.ES.Utility.CalculateESMarge.ClsESMarge(_settingCalcMArge)
						Dim strResult = obj.GetSelectedESMarge()

						m_MargeData = New MargeStringData
						m_MargeData.FillFromString(strResult)

					Catch ex As Exception
						m_Logger.LogError(ex.ToString)
						m_UtilityUI.ShowErrorDialog(ex.ToString)

					End Try

					lblMargeOhneBVGValue.Text = String.Format("{0:0.00}", Math.Round(m_MargeData.MargeOhneBVG, 4))
					lblMargeMitBVGValue.Text = String.Format("{0:0.00}", Math.Round(m_MargeData.MargeMitBVG, 4))
					lblMargeOhneBVGProzValue.Text = String.Format("{0:0.00}", Math.Round(m_MargeData.MargeOhneBVGInProz, 4))
					lblMargeMitBVGProzValue.Text = String.Format("{0:0.00}", Math.Round(m_MargeData.MargeWithBVGInProz, 4))


				Catch ex As Exception
					m_Logger.LogError(String.Format("._sbLohn = {0}, ._sMASSpesen = {1}, ._sMATSpesen = {2}, " &
																			 "._sKDTarif = {3}, " &
																			 "._sKDTSpesen = {4}, " &
																			 "._strSuva = {5}, " &
																			 "._sFARProz = {6}, " &
																			 "._sWAGProz = {7}, " &
																			 "._sVAGProz = {8}, " &
																			 "._sWAGBtr = {9}, " &
																			 "._sVAGBtr = {10}, " &
																			 "._strGAVInfo = {11}, ",
																			 Convert.ToSingle(txtStundenLohn.EditValue),
																												Convert.ToSingle(txtLohnspesen.EditValue),
																												Convert.ToSingle(txtTagesspesenMA.EditValue),
																												Convert.ToSingle(txtTarif.EditValue),
																												Convert.ToSingle(txtTagespesenKD.EditValue),
																												suva,
																												If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_FAG),
																												If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_WAG),
																												If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_VAG),
																												 If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_WAG_S),
																												 If(m_EffectivGAVData Is Nothing, 0, m_EffectivGAVData.GAV_VAG_S),
																												 If(m_EffectivGAVData Is Nothing, String.Empty, m_EffectivGAVData.GAVInfo_String)
																												 ))
					m_UtilityUI.ShowErrorDialog(String.Format("m_EffectivGAVData: {0} | {1}", m_EffectivGAVData Is Nothing, ex.ToString))

				End Try


			End If

    End Sub

    ''' <summary>
    ''' Calculates the MwSt value.
    ''' </summary>
    Private Sub CalcMwstValue()

      If Not chkMwStPflichtig.Checked Then
        txtMwstBetrag.EditValue = 0D
      Else
				'txtMwstBetrag.EditValue = txtTarif.EditValue * (MWST_PERCENT / 100D)
				Dim mdYear As Integer
				If dateEditSalaryDataFrom.EditValue Is Nothing Then mdYear = Now.Year Else mdYear = Year(dateEditSalaryDataFrom.EditValue)
				txtMwstBetrag.EditValue = txtTarif.EditValue * (GetDefaultMwStAnsatz(mdYear) / 100D)

			End If
    End Sub

    ''' <summary>
    ''' Updates effective gav values based on stundenlohn and far pflichtig.
    ''' </summary>
    Private Sub UpdateEffectiveGAVValuesBasedOnGavStringStundenLohnAndFarPflichtig()

      If Not m_UCMediator.SelectedCandidateAndCustomerData.MandantData Is Nothing Then

        Dim mdNumber As Integer = m_UCMediator.SelectedCandidateAndCustomerData.MandantData.MandantNumber

        Dim bNoPVL As Boolean = False

        Dim companyallowednopvl As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mdNumber),
                                                                                              String.Format("{0}/companyallowednopvl", FORM_XML_MAIN_KEY)), False)

        bNoPVL = If(companyallowednopvl.HasValue, companyallowednopvl, False)

        Dim esDataSupport As New ESCreateService(mdNumber, m_InitializationData)

        If Not m_SelectedGAVStringData Is Nothing Then
          chkFarPflichtig.Checked = (m_SelectedGAVStringData.FARAN +
                                     m_SelectedGAVStringData.FARAG +
                                     m_SelectedGAVStringData._FAG +
                                     m_SelectedGAVStringData._FAN) > 0
        Else
          chkFarPflichtig.Checked = False
        End If


				m_EffectivGAVData = esDataSupport.DetermineEffectiveGAVData(m_SelectedGAVStringData,
																																		txtGrundlohn.EditValue,
																																		chkFarPflichtig.Checked,
																																		mdNumber,
																																		bNoPVL)

      Else
        m_EffectivGAVData = Nothing
      End If

      ' Update marge because the marge depends on some GAV values.
      CalcMarge()
    End Sub

    ''' <summary>
    ''' Preselects data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function PreselectData() As Boolean

      Dim success As Boolean = True

      If Not PreselectionData Is Nothing AndAlso PreselectionData.CustomerKST.HasValue Then

        ' ---Customer KST---
        Dim preselectCustomerKSTSuccesful = False
        If PreselectionData.CustomerKST.HasValue AndAlso Not lueCustomerKST.Properties.DataSource Is Nothing Then

          Dim customerKSTDataList = CType(lueCustomerKST.Properties.DataSource, BindingList(Of KSTViewData))

          If customerKSTDataList.Any(Function(customerKST) customerKST.RecordNumber = PreselectionData.CustomerKST) Then
            lueCustomerKST.EditValue = PreselectionData.CustomerKST.Value
            preselectCustomerKSTSuccesful = True
          End If
        End If

        If Not preselectCustomerKSTSuccesful Then
          m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Kunden-Kostenstelle konnte nicht vorselektiert werden."))
        End If

      End If
      ' if just one record exists
      If lueCustomerKST.EditValue Is Nothing AndAlso Not lueCustomerKST.Properties.DataSource Is Nothing Then
        If lueCustomerKST.Properties.DataSource.count = 1 Then
          Dim customerKSTDataList = CType(lueCustomerKST.Properties.DataSource, BindingList(Of KSTViewData))

          If customerKSTDataList.Any(Function(customerKST) customerKST.RecordNumber = customerKSTDataList(0).RecordNumber) Then
            lueCustomerKST.EditValue = customerKSTDataList(0).RecordNumber
          End If
        End If

      End If


      ' Load initial Ansatz values
      Dim employeeBirthDate As DateTime? = m_UCMediator.SelectedCandidateAndCustomerData.EmployeeData.Birthdate

      If employeeBirthDate.HasValue Then
        Dim age As Integer = GetAge(employeeBirthDate)
        success = success AndAlso LoadSalaryCalculationPercentageValues(age)
      End If

      ' Mwst pflicht
      Dim customerData = m_UCMediator.SelectedCandidateAndCustomerData.CustomerData
      If customerData.mwstpflicht.HasValue Then
        chkMwStPflichtig.Checked = customerData.mwstpflicht.Value
      End If

      ' Salary from data
      Dim mustSalarydateBeAStoday As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr),
                                                                 String.Format("{0}/setsalarydatetotodayines", FORM_XML_MAIN_KEY)), True)

      If mustSalarydateBeAStoday Then
        dateEditSalaryDataFrom.EditValue = DateTime.Now.Date
      Else
        dateEditSalaryDataFrom.EditValue = Nothing
      End If

      ' Tagesspesen MA & KD
      If ESNr.HasValue Then
        ' The page is called for an existing ES. 
        ' In this case Tagespesen must not be entered by the user, they are set to match the existing ESLohn.
        ' This is done because if the Tagespesen value of different ESLohn records would not match, then this situation would lead to calculation errors in the report modul.

        Dim existingESLohnData = m_UCMediator.ESDbAccess.LoadESSalaryData(ESNr)

        If Not existingESLohnData Is Nothing Then

          ' Query for the active ESLohn data
          Dim activeSalaryData = existingESLohnData.Where(Function(data) data.AktivLODaten.HasValue AndAlso data.AktivLODaten).FirstOrDefault()


          If Not activeSalaryData Is Nothing Then

            txtTagesspesenMA.EditValue = activeSalaryData.MATSpesen
            txtTagespesenKD.EditValue = activeSalaryData.KDTSpesen

            txtTagesspesenMA.Properties.ReadOnly = True
            txtTagespesenKD.Properties.ReadOnly = True

            btnUnlockTagespesen.Visible = True
            lblSpesenWarning.Visible = True

          End If

        End If
			Else
				' it's new es.
				dateEditSalaryDataFrom.EditValue = UCMediator.SelectedESData.ESStartDate
			End If

      Return success
    End Function

    ''' <summary>
    ''' Gets the age in years.
    ''' </summary>
    ''' <param name="birthDate">The birthdate.</param>
    ''' <returns>Age in years.</returns>
    Private Function GetAge(ByVal birthDate As DateTime) As Integer

      ' Get year diff
      Dim years As Integer = DateTime.Now.Year - birthDate.Year

      birthDate = birthDate.AddYears(years)

      ' Subtract another year if its a day before the the birth day
      If (DateTime.Today.CompareTo(birthDate) < 0) Then
        years = years - 1
      End If

      Return years

    End Function

    ''' <summary>
    ''' Handles click on unloack tagespensen button.
    ''' </summary>
    Private Sub OnBtnUnlockTagespesen_Click(sender As Object, e As EventArgs) Handles btnUnlockTagespesen.Click
      m_DidUserConfirmOverrideOfTagespesen = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie die Tagesspesen übersteueren?") & vbCrLf & vbCrLf &
                                  m_Translate.GetSafeTranslationValue("Wenn Sie die Tagesspesen manuell anpassen, dann werden unbenutzte Lohndatensätze mit abweichenden Tagesspesen") & vbCrLf &
                                  m_Translate.GetSafeTranslationValue("beim Anlegen dieses Lohns automatisch entfernt."))

      If m_DidUserConfirmOverrideOfTagespesen Then

        txtTagesspesenMA.Properties.ReadOnly = False
        txtTagespesenKD.Properties.ReadOnly = False

      End If

    End Sub

    ''' <summary>
    ''' Handles drop down button clicks.
    ''' </summary>
    Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

      Const ID_OF_DELETE_BUTTON As Int32 = 1

      ' If delete button has been clicked reset the drop down.
      If e.Button.Index = ID_OF_DELETE_BUTTON Then
        If TypeOf sender Is BaseEdit Then
          If CType(sender, BaseEdit).Properties.ReadOnly Then
            ' nothing
          Else
            CType(sender, BaseEdit).EditValue = Nothing
          End If
        End If
      End If
    End Sub

#End Region



#Region "View helper classes"


    ''' <summary>
    '''  KST view data.
    ''' </summary>
    Class KSTViewData
      Public Property Id As Integer
      Public Property RecordNumber As Integer?
      Public Property Description As String

    End Class

#End Region

  End Class

End Namespace

Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Invoice
Imports DevExpress.XtraEditors
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.DatabaseAccess.Invoice.DataObjects

Namespace UI

	Public Class ucPageSelectMandantPayment

#Region "Private Constants"

		Public Const MANDANT_XML_SETTING_buch As String = "MD_{0}/Lohnbuchhaltung/guthaben/showguthabenpereaches"
		Public Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Private Fields"

		''' <summary>
		''' The mandant data.
		''' </summary>
		Private m_SelectedMandantData As SP.DatabaseAccess.Common.DataObjects.MandantData


		Private m_InvoiceData As DatabaseAccess.Invoice.DataObjects.Invoice

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			Try
				m_ProgPath = New ClsProgPath
				m_Mandant = New Mandant
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			AddHandler lueMandant.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler daeValutaDate.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler daeBookingDate.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueKonto.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueInvoice.ButtonClick, AddressOf OnDropDownButtonClick

		End Sub

#End Region


#Region "Public Properties"

		''' <summary>
		''' Gets the selected invoice data.
		''' </summary>
		Public ReadOnly Property SelecteData As InitPaymentDataPage1
			Get

				Dim data As New InitPaymentDataPage1 With {.MandantData = m_SelectedMandantData,
					.InvoiceData = m_InvoiceData,
					.Amount = txtAmount.EditValue,
					.VDate = daeValutaDate.EditValue,
					.BDate = daeBookingDate.EditValue,
					.FKSOLL = lueKonto.EditValue
				}

				Return data
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

			If m_IsFirstPageActivation Then
				success = success AndAlso LoadMandantDropDownData()
				' is done with preselection!
				'success = success AndAlso LoadInvoiceDropDownData()
				'success = success AndAlso LoadKontoDropDownData()


				PreselectData()

			End If

			m_IsFirstPageActivation = False

			Return success
		End Function

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_IsFirstPageActivation = True

			m_SelectedMandantData = Nothing
			m_InvoiceData = Nothing

			ResetInvoiceDetailData()

			'  Reset drop downs and lists
			ResetMandantDropDown()
			ResetInvoiceDropDown()
			ResetFibuKontenDropDown()


			ErrorProvider.Clear()

		End Sub


		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			Dim valid As Boolean = True
			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorInvalidAmountText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen gültigen Betrag ein. Der Betrag darf den offenen Rechnungsbetrag nicht übersteigen!")
			Dim errorInvalidDateText As String = m_Translate.GetSafeTranslationValue("Das ausgewählte Datum ist nicht gültig.")

			ErrorProvider.Clear()

			Try
				'mandatory fields
				valid = valid And SetErrorIfInvalid(lueMandant, ErrorProvider, lueMandant.EditValue Is Nothing, errorText)

				valid = valid And SetErrorIfInvalid(lueInvoice, ErrorProvider, lueInvoice.EditValue Is Nothing, errorText)
				valid = valid And SetErrorIfInvalid(lueKonto, ErrorProvider, lueKonto.EditValue Is Nothing, errorText)

				valid = valid And SetErrorIfInvalid(daeValutaDate, ErrorProvider, daeValutaDate.EditValue Is Nothing OrElse daeValutaDate.EditValue > daeBookingDate.EditValue, errorInvalidDateText)
				valid = valid And SetErrorIfInvalid(daeBookingDate, ErrorProvider, daeBookingDate.EditValue Is Nothing OrElse daeValutaDate.EditValue > daeBookingDate.EditValue, errorInvalidDateText)

				valid = valid And SetErrorIfInvalid(txtAmount, ErrorProvider, txtAmount.EditValue Is Nothing OrElse txtAmount.EditValue = 0 OrElse txtAmount.EditValue > m_InvoiceData.OpenAmount, errorInvalidAmountText)


			Catch ex As Exception
				valid = False
			End Try

			Return valid

		End Function

#End Region


#Region "private properties"

		Private ReadOnly Property RoundOpenAmount() As Boolean
			Get

				Dim value As Boolean = True
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim mandantNumber = lueMandant.EditValue
				Dim ivoiceData = SelectedInvoice()

				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mandantNumber)

				If ivoiceData Is Nothing Then
					value = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mandantNumber, m_InitializationData.MDData.MDYear), String.Format("{0}/roundopenamountforbooking", FORM_XML_MAIN_KEY)), False)

				Else
					value = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mandantNumber, Year(ivoiceData.FakDat)), String.Format("{0}/roundopenamountforbooking", FORM_XML_MAIN_KEY)), False)

				End If

				Return value
			End Get
		End Property

		Private ReadOnly Property LoadAutomatedKontoNumber() As Integer
			Get

				Dim value As Integer = 0
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
				Dim mandantNumber = lueMandant.EditValue
				Dim ivoiceData = SelectedInvoice()
				If ivoiceData Is Nothing Then Return value

				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/BuchungsKonten", mandantNumber)

				value = Val(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mandantNumber, Year(ivoiceData.FakDat)), String.Format("{0}/_7", FORM_XML_MAIN_KEY)))

				Return value
			End Get
		End Property

		Private ReadOnly Property SelectedInvoice As DataObjects.Invoice
			Get

				If lueInvoice.EditValue Is Nothing Then
					Return Nothing
				End If

				Dim invoiceListData = CType(lueInvoice.Properties.DataSource, List(Of DataObjects.Invoice))
				Dim data = invoiceListData.Where(Function(x) x.ReNr = lueInvoice.EditValue).FirstOrDefault


				Return data

			End Get
		End Property

#End Region



#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			'Captions
			gpEigenschaften.Text = m_Translate.GetSafeTranslationValue(gpEigenschaften.Text)

			'Labels
			lblMandant.Text = m_Translate.GetSafeTranslationValue(lblMandant.Text)

			grpZahlungseingang.Text = m_Translate.GetSafeTranslationValue(grpZahlungseingang.Text)
			lblInvoiceNumber.Text = m_Translate.GetSafeTranslationValue(lblInvoiceNumber.Text, True)
			lblValutaDate.Text = m_Translate.GetSafeTranslationValue(lblValutaDate.Text, True)
			lblBookingDate.Text = m_Translate.GetSafeTranslationValue(lblBookingDate.Text, True)

			lblKontoZE.Text = m_Translate.GetSafeTranslationValue(lblKontoZE.Text)
			lblBetragZE.Text = m_Translate.GetSafeTranslationValue(lblBetragZE.Text)

		End Sub

		''' <summary>
		''' Resets the Mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantNumber"

			lueMandant.Properties.Columns.Clear()
			lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})
			lueMandant.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the invoice drop down.
		''' </summary>
		Private Sub ResetInvoiceDropDown()

			lueInvoice.Properties.DisplayMember = "InvoiceViewData"
			lueInvoice.Properties.ValueMember = "ReNr"

			gvInvoice.OptionsView.ShowIndicator = False
			gvInvoice.OptionsView.ShowColumnHeaders = True
			gvInvoice.OptionsView.ShowFooter = False

			gvInvoice.OptionsView.ShowAutoFilterRow = True
			gvInvoice.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvInvoice.Columns.Clear()

			Dim columnReNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnReNr.Caption = m_Translate.GetSafeTranslationValue("Nr")
			columnReNr.Name = "ReNr"
			columnReNr.FieldName = "ReNr"
			columnReNr.Visible = True
			columnReNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvInvoice.Columns.Add(columnReNr)

			Dim columnRName1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRName1.Caption = m_Translate.GetSafeTranslationValue("Firma1")
			columnRName1.Name = "RName1"
			columnRName1.FieldName = "RName1"
			columnRName1.Visible = True
			columnRName1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvInvoice.Columns.Add(columnRName1)

			Dim columnCustomerPostcodeLocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomerPostcodeLocation.Caption = m_Translate.GetSafeTranslationValue("Adresse")
			columnCustomerPostcodeLocation.Name = "CustomerPostcodeLocation"
			columnCustomerPostcodeLocation.FieldName = "CustomerPostcodeLocation"
			columnCustomerPostcodeLocation.Visible = True
			columnCustomerPostcodeLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvInvoice.Columns.Add(columnCustomerPostcodeLocation)

			Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetragInk.Name = "BetragInk"
			columnBetragInk.FieldName = "BetragInk"
			columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
			columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetragInk.DisplayFormat.FormatString = "N2"
			columnBetragInk.Visible = True
			columnBetragInk.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvInvoice.Columns.Add(columnBetragInk)

			Dim columnOpenAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnOpenAmount.Caption = m_Translate.GetSafeTranslationValue("Offener Betrag")
			columnOpenAmount.Name = "OpenAmount"
			columnOpenAmount.FieldName = "OpenAmount"
			columnOpenAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnOpenAmount.AppearanceHeader.Options.UseTextOptions = True
			columnOpenAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnOpenAmount.DisplayFormat.FormatString = "N2"
			columnOpenAmount.Visible = True
			columnOpenAmount.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvInvoice.Columns.Add(columnOpenAmount)


			lueInvoice.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueInvoice.Properties.NullText = String.Empty
			lueInvoice.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the FKSoll drop down.
		''' </summary>
		Private Sub ResetFibuKontenDropDown()

			lueKonto.Properties.DisplayMember = "KontoViewData"
			lueKonto.Properties.ValueMember = "KontoNr"

			gvKonto.OptionsView.ShowIndicator = False
			gvKonto.OptionsView.ShowColumnHeaders = True
			gvKonto.OptionsView.ShowFooter = False

			gvKonto.OptionsView.ShowAutoFilterRow = True
			gvKonto.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvKonto.Columns.Clear()

			Dim columnKontoNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKontoNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnKontoNr.Name = "KontoNr"
			columnKontoNr.FieldName = "KontoNr"
			columnKontoNr.Visible = True
			columnKontoNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvKonto.Columns.Add(columnKontoNr)

			Dim columnbez_d As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbez_d.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnbez_d.Name = "TranslatedLabel"
			columnbez_d.FieldName = "TranslatedLabel"
			columnbez_d.Visible = True
			columnbez_d.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvKonto.Columns.Add(columnbez_d)


			lueKonto.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueKonto.Properties.NullText = String.Empty
			lueKonto.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets invoice detail data.
		''' </summary>
		Private Sub ResetInvoiceDetailData()

			daeValutaDate.EditValue = Now.Date
			daeBookingDate.EditValue = Now.Date

			lueKonto.EditValue = Nothing

			txtAmount.EditValue = 0D

		End Sub


		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantDropDownData() As Boolean
			Dim mandantData = m_UCPaymentMediator.CommonDbAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
			End If

			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return mandantData IsNot Nothing
		End Function

		''' <summary>
		''' Loads the invoice drop down data.
		''' </summary>
		Private Function LoadInvoiceDropDownData() As Boolean

			ErrorProvider.Clear()

			Dim invoiceData = m_UCPaymentMediator.InvoiceDbAccess.LoadInvoiceWithOpenAmount(lueMandant.EditValue)

			If invoiceData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdata konnten nicht geladen werden."))
				Return False
			End If

			lueInvoice.EditValue = Nothing
			Dim data As List(Of DataObjects.Invoice) = Nothing
			If RoundOpenAmount Then
				data = invoiceData.Where(Function(x) m_Utility.SwissCommercialRound(x.OpenAmount) <> 0).ToList()
			Else
				data = invoiceData.Where(Function(x) x.OpenAmount <> 0).ToList()

			End If
			lueInvoice.Properties.DataSource = data

			Return True

		End Function

		''' <summary>
		''' Loads the invoice drop down data.
		''' </summary>
		Private Function LoadKontoDropDownData() As Boolean

			Dim fibuData = m_UCPaymentMediator.TableDbAccess.LoadFIBUKontenData(m_InitializationData.UserData.UserLanguage)

			If fibuData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Buchungskonten konnten nicht geladen werden."))
				Return False
			End If

			lueKonto.EditValue = Nothing
			lueKonto.Properties.DataSource = fibuData

			Return True

		End Function


		''' <summary>
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			Dim hasPreselectionData As Boolean = Not (PreselectionPaymentData Is Nothing)

			If hasPreselectionData Then

				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

				' Mandant---
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

					If manantDataList.Any(Function(md) md.MandantNumber = PreselectionPaymentData.MDNr) Then

						' Mandant is required
						lueMandant.EditValue = PreselectionPaymentData.MDNr

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						m_SuppressUIEvents = supressUIEventState
						Return
					End If

				End If

				' invoices---
				If PreselectionPaymentData.InvoiceNumber.GetValueOrDefault > 0 AndAlso Not lueInvoice.Properties.DataSource Is Nothing Then

					Dim invoiceDataList = CType(lueInvoice.Properties.DataSource, List(Of DataObjects.Invoice))

					If invoiceDataList.Any(Function(x) x.ReNr = PreselectionPaymentData.InvoiceNumber) Then

						' Mandant is required
						lueInvoice.EditValue = PreselectionPaymentData.InvoiceNumber

						Dim kontoNumber = LoadAutomatedKontoNumber()
						lueKonto.EditValue = kontoNumber

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Rechnung konnte nicht vorselektiert werden. Möglicherweise wurde sie ausgeglichen."))
						m_SuppressUIEvents = supressUIEventState
						Return
					End If

				End If
				m_SuppressUIEvents = supressUIEventState

			Else
				' No preslection data -> use mandant form initialization object.

				' ---Mandant---
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

					If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

						' Mandant is required
						lueMandant.EditValue = m_InitializationData.MDData.MDNr

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						Return
					End If

				End If

			End If

		End Sub


#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles change of mandant.
		''' </summary>
		Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			If Not lueMandant.EditValue Is Nothing Then

				Dim mandantData = CType(lueMandant.GetSelectedDataRow(), MandantData)

				m_SelectedMandantData = mandantData
				m_UCPaymentMediator.HandleChangeOfMandant(m_SelectedMandantData.MandantNumber)

				LoadInvoiceDropDownData()
				LoadKontoDropDownData()

			Else
				m_SelectedMandantData = Nothing

				lueInvoice.EditValue = Nothing
				lueKonto.EditValue = Nothing

			End If

		End Sub

		''' <summary>
		''' Handles change of invoice.
		''' </summary>
		Private Sub OnLueInvoice_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueInvoice.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			ErrorProvider.Clear()
			If Not lueInvoice.EditValue Is Nothing Then

				Dim invoiceNumber As Integer = lueInvoice.EditValue

				Dim invoiceData = m_UCPaymentMediator.InvoiceDbAccess.LoadInvoice(invoiceNumber)

				If invoiceData Is Nothing Then

					ResetInvoiceDetailData()
					m_InvoiceData = Nothing

					If invoiceData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))
					End If

				Else
					m_InvoiceData = invoiceData

					DisplayInvoiceData()
				End If

			Else
				ResetInvoiceDetailData()
				m_InvoiceData = Nothing

			End If

			'm_UCPaymentMediator.HandleChangeMandantEmployeeOrCustomer()

		End Sub

		''' <summary>
		''' Displays invoice detail data.
		''' </summary>
		Private Function DisplayInvoiceData() As Boolean

			If m_InvoiceData Is Nothing Then Return False

			Dim kontoNumber = LoadAutomatedKontoNumber()
			lueKonto.EditValue = kontoNumber

			txtAmount.EditValue = m_InvoiceData.OpenAmount.GetValueOrDefault(0)

			Return True

		End Function


		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

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

	End Class

End Namespace

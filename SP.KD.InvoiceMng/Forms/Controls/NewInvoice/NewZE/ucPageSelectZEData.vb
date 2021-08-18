'Imports DevExpress.XtraEditors.Controls
'Imports SP.DatabaseAccess.Invoice
'Imports SP.Infrastructure.ucListSelectPopup
'Imports SP.Infrastructure.Messaging
'Imports SP.Infrastructure.Messaging.Messages
'Imports SP.KD.ReAdresse.UI
'Imports DevExpress.XtraEditors

'Namespace UI

'    Public Class ucPageSelectZEData

'#Region "Private Consts"

'        Private Const POPUP_DEFAULT_WIDTH As Integer = 420
'        Private Const POPUP_DEFAULT_HEIGHT As Integer = 325

'#End Region

'#Region "Private Fields"

'        ''' <summary>
'        ''' The customers popup data
'        ''' </summary>
'        Private m_CustomersData As List(Of DataObjects.Customer)
'        Private m_isCustomerPopupOpen As Boolean
'        Private m_SelectedCustomer As DataObjects.Customer
'        Private m_SelectedAddress As DataObjects.CustomerReAddress

'        Private m_Debitorenart As List(Of Debitorenart)
'        Private m_BankData As List(Of DataObjects.BankData)
'        Private m_AddressData As List(Of DataObjects.CustomerReAddress)
'        Private m_MahnCodeData As List(Of DatabaseAccess.Customer.DataObjects.PaymentReminderCodeData) = Nothing

'#End Region

'#Region "Constructor"

'        Public Sub New()

'            '    ' Dieser Aufruf ist für den Designer erforderlich.
'            '    InitializeComponent()

'            '    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

'            '    ' Button Click Handle
'            '    AddHandler lueDebitorenart.ButtonClick, AddressOf OnDropDownButtonClick
'            '    AddHandler lueBankdaten.ButtonClick, AddressOf OnDropDownButtonClick
'            '    AddHandler daeDatum.ButtonClick, AddressOf OnDropDownButtonClick
'            '    AddHandler lueAdresse.ButtonClick, AddressOf OnDropDownButtonClick
'            '    AddHandler daeDueDate.ButtonClick, AddressOf OnDropDownButtonClick

'            '    ' Register popup row click handlers
'            '    AddHandler ucCustomerPopup.RowClicked, AddressOf OnPopupCustomerRowClick
'            '    AddHandler ucCustomerPopup.PopupSizeChanged, AddressOf OnPopupCustomer_SizeChanged

'        End Sub

'#End Region

'#Region "Public Properties"

'        ''' <summary>
'        ''' Gets the selected candidate and customer data.
'        ''' </summary>
'        ''' <returns>Candidate and customer data.</returns>
'        Public ReadOnly Property SelecteData As InitDataPage2
'            Get

'                'Dim data As New InitDataPage2 With {
'                '            .DebitorenArt = (From d In m_Debitorenart Where d.Value = lueDebitorenart.EditValue).FirstOrDefault,
'                '            .BankData = (From b In m_BankData Where b.ID = lueBankdaten.EditValue).FirstOrDefault,
'                '            .DateValue = daeDatum.EditValue,
'                '            .Customer = m_SelectedCustomer,
'                '            .CustomerReAddress = m_SelectedAddress,
'                '            .DueDate = daeDueDate.EditValue
'                '        }

'                Dim data As New InitDataPage2
'                Return data
'            End Get
'        End Property

'#End Region

'#Region "Public Methods"

'        '''' <summary>
'        '''' Inits the control with configuration information.
'        '''' </summary>
'        ''''<param name="initializationClass">The initialization class.</param>
'        ''''<param name="translationHelper">The translation helper.</param>
'        'Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
'        '    MyBase.InitWithConfigurationData(initializationClass, translationHelper)
'        'End Sub

'        '''' <summary>
'        '''' Activates the page.
'        '''' </summary>
'        '''' <returns>Boolean value indicating success.</returns>
'        'Public Overrides Function ActivatePage() As Boolean

'        '    Dim success As Boolean = True

'        '    If m_IsFirstPageActivation Then
'        '        LoadDebitorenartDropDown()
'        '        success = success AndAlso LoadBankdatenDropDown(True)
'        '        success = success AndAlso LoadCustomerData()

'        '        PreselectData()

'        '    End If

'        '    m_IsFirstPageActivation = False

'        '    Return success
'        'End Function

'        '''' <summary>
'        '''' Resets the control.
'        '''' </summary>
'        'Public Overrides Sub Reset()

'        '    m_IsFirstPageActivation = True
'        '    HidePopups()

'        '    m_CustomersData = Nothing
'        '    m_SelectedCustomer = Nothing
'        '    m_SelectedAddress = Nothing
'        '    m_Debitorenart = Nothing
'        '    m_BankData = Nothing
'        '    m_AddressData = Nothing
'        '    m_MahnCodeData = Nothing

'        '    daeDatum.EditValue = Nothing
'        '    daeDueDate.EditValue = Nothing
'        '    cboCustomer.Text = String.Empty

'        '    '  Reset drop downs and lists

'        '    ResetAddressDropDown()
'        '    ResetDebitorenartDropDown()
'        '    ResetBankdatenDropDown()
'        '    ResetAddressDropDown()

'        '    ErrorProvider.Clear()
'        'End Sub


'        '''' <summary>
'        '''' Validated data.
'        '''' </summary>
'        'Public Overrides Function ValidateData() As Boolean

'        '    ErrorProvider.Clear()
'        '    Dim valid As Boolean = True
'        '    Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
'        '    Dim errorDueDate As String = m_Translate.GetSafeTranslationValue("Fällig-Datum muss nach dem Rechnungs-Datum liegen.")
'        '    Dim errorMonthCloseText As String = m_Translate.GetSafeTranslationValue("Der ausgewählte Monat ist bereits abgeschlossen.")
'        '    Try
'        '        'mandatory fields

'        '        valid = valid And SetErrorIfInvalid(lueDebitorenart, ErrorProvider, lueDebitorenart.EditValue Is Nothing, errorText)
'        '        valid = valid And SetErrorIfInvalid(lueBankdaten, ErrorProvider, lueBankdaten.EditValue Is Nothing, errorText)
'        '        valid = valid And SetErrorIfInvalid(daeDatum, ErrorProvider, daeDatum.EditValue Is Nothing, errorText)

'        '        ' Check for conflicting MonthClose records.
'        '        Dim IsMonthClosed = CheckForConflictingMonthCloseRecordsInPeriod(UCMediator.InitDataPage1.MandantData.MandantNumber, daeDatum.EditValue, daeDatum.EditValue)
'        '        valid = valid And SetErrorIfInvalid(daeDatum, ErrorProvider, Not IsMonthClosed, errorMonthCloseText)


'        '        valid = valid And SetErrorIfInvalid(daeDueDate, ErrorProvider, daeDueDate.EditValue Is Nothing, errorText)
'        '        valid = valid And SetErrorIfInvalid(daeDueDate, ErrorProvider, CType(daeDatum.EditValue, Date) > CType(daeDueDate.EditValue, Date), errorDueDate)
'        '        valid = valid And SetErrorIfInvalid(cboCustomer, ErrorProvider, m_SelectedCustomer Is Nothing, errorText)
'        '        valid = valid And SetErrorIfInvalid(lueAdresse, ErrorProvider, m_SelectedAddress Is Nothing, errorText)

'        '    Catch ex As Exception
'        '        valid = False
'        '    End Try

'        '    Return valid

'        'End Function

'#End Region

'#Region "Private Methods"

'        '''' <summary>
'        ''''  Translate controls.
'        '''' </summary>
'        'Protected Overrides Sub TranslateControls()

'        '    'Captions
'        '    gpRechnungsdaten.Text = m_Translate.GetSafeTranslationValue(gpRechnungsdaten.Text)

'        '    'Labels
'        '    lblDebitorenart.Text = m_Translate.GetSafeTranslationValue(lblDebitorenart.Text)
'        '    lblBankdaten.Text = m_Translate.GetSafeTranslationValue(lblBankdaten.Text)
'        '    lblDatum.Text = m_Translate.GetSafeTranslationValue(lblDatum.Text)
'        '    lblFirma.Text = m_Translate.GetSafeTranslationValue(lblFirma.Text)
'        '    lblAddresse.Text = m_Translate.GetSafeTranslationValue(lblAddresse.Text)
'        '    lblDueDate.Text = m_Translate.GetSafeTranslationValue(lblDueDate.Text)

'        'End Sub

'        '''' <summary>
'        '''' Resets the Address drop down.
'        '''' </summary>
'        'Private Sub ResetAddressDropDown()

'        '    lueAdresse.Properties.DisplayMember = "Address"
'        '    lueAdresse.Properties.ValueMember = "Id"

'        '    lueAdresse.Properties.Columns.Clear()
'        '    lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REFirma", 0))
'        '    lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REStrasse", 0))
'        '    lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REPLZ", 0))
'        '    lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REOrt", 0))

'        '    lueAdresse.Properties.DataSource = Nothing
'        '    lueAdresse.EditValue = Nothing
'        'End Sub

'        '''' <summary>
'        '''' Resets the Debitorenart drop down.
'        '''' </summary>
'        'Private Sub ResetDebitorenartDropDown()

'        '    lueDebitorenart.Properties.DisplayMember = "Label"
'        '    lueDebitorenart.Properties.ValueMember = "Value"

'        '    lueDebitorenart.Properties.Columns.Clear()
'        '    lueDebitorenart.Properties.Columns.Add(New LookUpColumnInfo("Value", 0))
'        '    lueDebitorenart.Properties.Columns.Add(New LookUpColumnInfo("Display", 0))

'        '    lueDebitorenart.EditValue = Nothing

'        'End Sub

'        '''' <summary>
'        '''' Resets the Bankdaten drop down.
'        '''' </summary>
'        'Private Sub ResetBankdatenDropDown()

'        '    lueBankdaten.Properties.DisplayMember = "BankName"
'        '    lueBankdaten.Properties.ValueMember = "ID"

'        '    lueBankdaten.Properties.Columns.Clear()
'        '    lueBankdaten.Properties.Columns.Add(New LookUpColumnInfo("KontoESR2", 0))
'        '    lueBankdaten.Properties.Columns.Add(New LookUpColumnInfo("BankName", 0))

'        '    lueBankdaten.EditValue = Nothing

'        'End Sub

'        '''' <summary>
'        '''' Loads the Debitorenart drop down data.
'        '''' </summary>
'        'Private Sub LoadDebitorenartDropDown()
'        '    m_Debitorenart = New List(Of Debitorenart) From {
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Individuelle Debitoren"), .Value = "I"},
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Festanstellung"), .Value = "F"},
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Gutschrift automatische Debitoren"), .Value = "GA"},
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Gutschrift individuelle Debitoren"), .Value = "GI"},
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Gutschrift Festanstellung"), .Value = "GF"},
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Rückvergütung automatische Debitoren"), .Value = "RA"},
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Rückvergütung individuelle Debitoren"), .Value = "RI"},
'        '        New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Rückvergütung Festanstellung"), .Value = "RF"}
'        '     }

'        '    lueDebitorenart.Properties.DataSource = m_Debitorenart

'        '    lueDebitorenart.Properties.ForceInitialize()

'        'End Sub

'        '''' <summary>
'        '''' Loads the Bankdaten drop down data.
'        '''' </summary>
'        'Private Function LoadBankdatenDropDown(ByVal setDefault As Boolean) As Boolean
'        '    ' Load data

'        '    Dim mandantNr = m_UCMediator.InitDataPage1.MandantData.MandantNumber

'        '    m_BankData = m_AddressData.InvoiceDbAccess.LoadBankData(mandantNr)

'        '    If (m_BankData Is Nothing) Then
'        '        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht geladen werden."))
'        '    End If

'        '    lueBankdaten.Properties.DataSource = m_BankData
'        '    lueBankdaten.Properties.ForceInitialize()

'        '    Dim standardBank = (From b In m_BankData Where b.AsStandard = True).FirstOrDefault
'        '    If standardBank Is Nothing Then
'        '        standardBank = (From b In m_BankData Order By b.RecNr).FirstOrDefault
'        '    End If

'        '    If setDefault AndAlso standardBank IsNot Nothing Then
'        '        lueBankdaten.EditValue = standardBank.ID
'        '    End If

'        '    Return m_BankData IsNot Nothing
'        'End Function

'        '''' <summary>
'        '''' Loads the customer data.
'        '''' </summary>
'        'Private Function LoadCustomerData() As Boolean

'        '    m_CustomersData = m_UCMediator.InvoiceDbAccess.LoadCustomerData()

'        '    Return m_CustomersData IsNot Nothing

'        'End Function

'        '''' <summary>
'        '''' Loads the ReAdress drop down data.
'        '''' </summary>
'        'Private Function LoadReAddressDropDown(ByVal setDefault As Boolean) As Boolean
'        '    If m_SelectedCustomer Is Nothing Then
'        '        m_AddressData = Nothing
'        '        m_SelectedAddress = Nothing
'        '        Return False
'        '    End If

'        '    m_AddressData = m_UCMediator.InvoiceDbAccess.LoadCustomerReAddressData(m_SelectedCustomer.KDNr)

'        '    If (m_AddressData Is Nothing) Then
'        '        Return False
'        '    End If

'        '    lueAdresse.Properties.DataSource = m_AddressData
'        '    lueAdresse.Properties.ForceInitialize()

'        '    'select active adress
'        '    Dim activeAdress = (From a In m_AddressData Where a.IsActive).FirstOrDefault
'        '    If activeAdress Is Nothing Then
'        '        activeAdress = (From a In m_AddressData Order By a.RecNr).FirstOrDefault
'        '    End If

'        '    If setDefault AndAlso activeAdress IsNot Nothing Then
'        '        lueAdresse.EditValue = activeAdress.Id
'        '    End If
'        '    Return m_AddressData IsNot Nothing
'        'End Function

'        '''' <summary>
'        '''' Hides the popups.
'        '''' </summary>
'        'Private Sub HidePopups()
'        '    ucCustomerPopup.HidePopup()
'        '    m_isCustomerPopupOpen = False
'        'End Sub


'        '''' <summary>
'        '''' Sets the selected Customer
'        '''' </summary>
'        'Private Sub SetCustomer(ByVal customerNumber As Integer)
'        '    m_SelectedCustomer = (From customer In m_CustomersData Where customer.KDNr = customerNumber).FirstOrDefault
'        '    If m_SelectedCustomer IsNot Nothing Then
'        '        cboCustomer.Text = String.Format("{0} - {1} - {2} {3}", m_SelectedCustomer.KDNr, m_SelectedCustomer.Firma1, m_SelectedCustomer.PLZ, m_SelectedCustomer.Ort)
'        '    End If

'        'End Sub

'        '''' <summary>
'        '''' Preselects data.
'        '''' </summary>
'        'Private Sub PreselectData()

'        '    Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

'        '    If hasPreselectionData AndAlso PreselectionData.RechnungsDatum.HasValue Then
'        '        daeDatum.EditValue = PreselectionData.RechnungsDatum
'        '    Else
'        '        daeDatum.EditValue = DateTime.Now.Date
'        '    End If

'        '    If hasPreselectionData AndAlso Not String.IsNullOrEmpty(PreselectionData.DebitorenArt) Then
'        '        lueDebitorenart.EditValue = PreselectionData.DebitorenArt
'        '    Else
'        '        lueDebitorenart.EditValue = "I"
'        '    End If

'        '    If hasPreselectionData AndAlso PreselectionData.CustomerNumber.HasValue Then

'        '        SetCustomer(PreselectionData.CustomerNumber)
'        '        LoadReAddressDropDown(True) 'False)

'        '        If PreselectionData.CustomerInvoiceAddressId.HasValue Then
'        '            lueAdresse.EditValue = PreselectionData.CustomerInvoiceAddressId.Value
'        '        End If

'        '    End If

'        'End Sub

'#End Region

'#Region "Event Handlers"

'        '''' <summary>
'        '''' Handles click on choose customer button.
'        '''' </summary>
'        'Private Sub OncboKundeButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cboCustomer.ButtonClick

'        '    If m_CustomersData Is Nothing Then
'        '        Return
'        '    End If

'        '    If e.Button.Index = 0 Then

'        '        If m_isCustomerPopupOpen Then
'        '            HidePopups()
'        '            Return
'        '        Else
'        '            m_isCustomerPopupOpen = True
'        '        End If


'        '        Dim customersPopupColumns As New List(Of PopupColumDefintion)
'        '        ' Column defintions for popups
'        '        customersPopupColumns.Add(New PopupColumDefintion With {.Name = "KDNr", .Translation = "KDNr", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Default})
'        '        customersPopupColumns.Add(New PopupColumDefintion With {.Name = "Firma1", .Translation = "Firma1", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})
'        '        customersPopupColumns.Add(New PopupColumDefintion With {.Name = "Strasse", .Translation = "Strasse", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})
'        '        customersPopupColumns.Add(New PopupColumDefintion With {.Name = "PLZ", .Translation = "PLZ", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})
'        '        customersPopupColumns.Add(New PopupColumDefintion With {.Name = "Ort", .Translation = "Ort", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})
'        '        customersPopupColumns.Add(New PopupColumDefintion With {.Name = "Land", .Translation = "Land", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})

'        '        ucCustomerPopup.InitPopup(m_CustomersData, customersPopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)

'        '        Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_CUSTOMER_SIZE)
'        '        Dim position = Cursor.Position
'        '        'Dim position As Point = cboCustomer.Parent.PointToScreen(cboCustomer.Location)
'        '        'position.Y += cboCustomer.Height

'        '        ucCustomerPopup.ShowPopup(position, popupSize)

'        '    Else

'        '        HidePopups()

'        '        If Not m_SelectedCustomer Is Nothing Then

'        '            Dim mdNumber = m_UCMediator.InitDataPage1.MandantData.MandantNumber
'        '            ' Send a request to open a customerMng form.
'        '            Dim hub = MessageService.Instance.Hub
'        '            Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, mdNumber, m_SelectedCustomer.KDNr)
'        '            hub.Publish(openCustomerMng)
'        '        End If

'        '    End If

'        'End Sub

'        '''' <summary>
'        '''' Handles poupup gav size change.
'        '''' </summary>
'        'Private Sub OnPopupCustomer_SizeChanged(ByVal sender As Object, ByVal newWidth As Integer, ByVal newHeight As Integer)
'        '    Try
'        '        Dim setting As String = String.Format("{0};{1}", newWidth, newHeight)
'        '        m_SettingsManager.WriteString(Settings.SettingKeys.SETTING_POPUP_CUSTOMER_SIZE, setting)
'        '        m_SettingsManager.SaveSettings()

'        '    Catch ex As Exception
'        '        m_Logger.LogError(ex.ToString())
'        '    End Try

'        'End Sub

'        '''' <summary>
'        '''' Handles click on a row on one of the popups.
'        '''' </summary>
'        'Private Sub OnPopupCustomerRowClick(ByVal sender As Object, ByVal clickedObject As Object)
'        '    Dim customerData As DataObjects.Customer = clickedObject

'        '    SetCustomer(customerData.KDNr)
'        '    LoadReAddressDropDown(True)

'        '    HidePopups()
'        'End Sub

'        '''' <summary>
'        '''' Handles the address changed event
'        '''' </summary>
'        'Private Sub OnlueAdresseEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueAdresse.EditValueChanged
'        '    If lueAdresse.EditValue Is Nothing Then
'        '        m_SelectedAddress = Nothing
'        '        Return
'        '    End If

'        '    m_SelectedAddress = (From a In m_AddressData Where a.Id = lueAdresse.EditValue).FirstOrDefault

'        '    CalculateDueDate()
'        'End Sub

'        '''' <summary>
'        '''' Handles click on address lookup edit button.
'        '''' </summary>
'        'Private Sub OnLueAdresseButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueAdresse.ButtonClick
'        '    If e.Button.Index = 2 Then

'        '        If Not m_SelectedAddress Is Nothing Then
'        '            Dim invoiceAddressesForm = New frmInvoiceAddress(m_InitializationData)
'        '            invoiceAddressesForm.Show()
'        '            invoiceAddressesForm.LoadCustomerInvoiceAddresses(m_SelectedAddress.KDNr, m_SelectedAddress.RecNr)
'        '            invoiceAddressesForm.BringToFront()
'        '        End If
'        '    End If
'        'End Sub

'        '''' <summary>
'        '''' Handles drop down button clicks.
'        '''' </summary>
'        'Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

'        '    Const ID_OF_DELETE_BUTTON As Int32 = 1

'        '    ' If delete button has been clicked reset the drop down.
'        '    If e.Button.Index = ID_OF_DELETE_BUTTON Then

'        '        If TypeOf sender Is BaseEdit Then
'        '            If CType(sender, BaseEdit).Properties.ReadOnly Then
'        '                ' nothing
'        '            Else
'        '                CType(sender, BaseEdit).EditValue = Nothing
'        '            End If
'        '        End If
'        '    End If
'        'End Sub

'#End Region

'#Region "Debitoren logic"

'        '''' <summary>
'        '''' Calculates the due date
'        '''' </summary>
'        'Private Sub CalculateDueDate()
'        '    If m_SelectedAddress Is Nothing OrElse String.IsNullOrWhiteSpace(m_SelectedAddress.MahnCode) OrElse daeDatum.EditValue Is Nothing Then
'        '        daeDueDate.EditValue = Nothing
'        '        Return
'        '    End If

'        '    If m_MahnCodeData Is Nothing Then
'        '        m_MahnCodeData = m_UCMediator.CustomerDbAccess.LoadPaymentReminderCodeData()
'        '    End If

'        '    Dim mahnCode = (From m In m_MahnCodeData Where m.GetField = m_SelectedAddress.MahnCode).FirstOrDefault
'        '    If mahnCode Is Nothing OrElse mahnCode.GetField = "N" Then
'        '        daeDueDate.EditValue = Nothing
'        '        Return
'        '    End If

'        '    Dim dueDate As Date = CType(daeDatum.EditValue, Date)
'        '    dueDate = dueDate.AddDays(mahnCode.Reminder1)
'        '    While (Weekday(dueDate) = vbSaturday OrElse Weekday(dueDate) = vbSunday)
'        '        ' next day after weekend
'        '        dueDate = dueDate.AddDays(1)
'        '    End While

'        '    daeDueDate.EditValue = dueDate
'        'End Sub


'        '''' <summary>
'        '''' Checks for conflicting MonthClose records in perdiod.
'        '''' </summary>
'        '''' <param name="startDate">The start date.</param>
'        '''' <param name="mdNumber">The mandant number.</param>
'        '''' <param name="endDate">The end date.</param>
'        '''' <returns>Boolean flag indicating if conflicting MonthClose records exist.</returns>
'        'Public Function CheckForConflictingMonthCloseRecordsInPeriod(ByVal mdNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean

'        '    Dim isValid As Boolean = True

'        '    Const RESULT_OK As Integer = 0
'        '    Const RESULT_CONFLICT As Integer = 1

'        '    Dim resultCode As Integer = 0
'        '    Dim conflictedMonth = m_UCMediator.InvoiceDbAccess.LoadConflictedMonthCloseRecordsInPeriod(mdNumber, startDate, endDate, resultCode)

'        '    If conflictedMonth Is Nothing OrElse resultCode = -1 Then
'        '        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler bei Konfliktprüfung"))
'        '        Return False
'        '    Else
'        '        Select Case resultCode
'        '            Case RESULT_OK
'        '                ' no conflicts
'        '            Case RESULT_CONFLICT
'        '                isValid = False


'        '        End Select

'        '    End If

'        '    Return isValid
'        'End Function


'#End Region


'        '    ''' <summary>
'        '    ''' Reads a popup size setting.
'        '    ''' </summary>
'        '    ''' <param name="settingKey">The settings key.</param>
'        '    ''' <returns>The size setting.</returns>
'        '    Private Function ReadPopupSizeSetting(ByRef settingKey As String) As Size

'        '        ' Load width/height setting
'        '        Dim popupSizeSetting As String = String.Empty
'        '        Dim popupSize As Size
'        '        popupSize.Width = POPUP_DEFAULT_WIDTH
'        '        popupSize.Height = POPUP_DEFAULT_HEIGHT

'        '        Try
'        '            popupSizeSetting = m_SettingsManager.ReadString(settingKey)

'        '            If Not String.IsNullOrEmpty(popupSizeSetting) Then
'        '                Dim arrSize As String() = popupSizeSetting.Split(CChar(";"))
'        '                popupSize.Width = arrSize(0)
'        '                popupSize.Height = arrSize(1)
'        '            End If
'        '        Catch ex As Exception
'        '            m_Logger.LogError(ex.ToString())
'        '        End Try

'        '        Return popupSize
'        '    End Function


'    End Class

'End Namespace

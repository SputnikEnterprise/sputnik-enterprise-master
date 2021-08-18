
'Imports System.Reflection.Assembly
'Imports DevExpress.LookAndFeel
'Imports DevExpress.XtraEditors
'Imports DevExpress.XtraEditors.Controls
'Imports DevExpress.XtraNavBar
'Imports SP.DatabaseAccess.Invoice
'Imports SP.Infrastructure
'Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.Settings
'Imports SP.Infrastructure.UI
'Imports SP.Infrastructure.ucListSelectPopup
'Imports SP.KD.InvoiceMng.Settings
'Imports SPProgUtility.CommonSettings
'Imports SPProgUtility.Mandanten
'Imports SPProgUtility.ProgPath
'Imports SPProgUtility.SPUserSec.ClsUserSec
'Imports SP.Infrastructure.Messaging
'Imports SP.Infrastructure.Messaging.Messages
'Imports SP.KD.ReAdresse.UI
'Imports SP.TodoMng
'Imports SPS.Listing.Print.Utility.InvoicePrint
'Imports DevExpress.Pdf
'Imports System.ComponentModel
'Imports SP.Internal.Automations.WOSUtility.DataObjects


'Namespace UI

'	''' <summary>
'	''' Invoice management.
'	''' </summary>
'	Public Class frmZE

'        '#Region "Private Consts"
'        '		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

'        '		Private Const POPUP_DEFAULT_WIDTH As Integer = 420
'        '		Private Const POPUP_DEFAULT_HEIGHT As Integer = 325

'        '#End Region

'        '#Region "Private Fields"

'        '		''' <summary>
'        '		''' The Initialization data.
'        '		''' </summary>
'        '		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

'        '		''' <summary>
'        '		''' The translation value helper.
'        '		''' </summary>
'        '		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

'        '		''' <summary>
'        '		''' The invoice data access object.
'        '		''' </summary>
'        '		Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess

'        '		''' <summary>
'        '		''' The common database access.
'        '		''' </summary>
'        '		Protected m_CommonDatabaseAccess As DatabaseAccess.Common.ICommonDatabaseAccess

'        '		''' <summary>
'        '		''' The data access object.
'        '		''' </summary>
'        '		Private m_CustomerDatabaseAccess As DatabaseAccess.Customer.ICustomerDatabaseAccess

'        '		''' <summary>
'        '		''' The settings manager.
'        '		''' </summary>
'        '		Private m_SettingsManager As ISettingsManager

'        '		''' <summary>
'        '		''' Contains the invoice number of the loaded invoice data.
'        '		''' </summary>
'        '		Private m_invoiceData As DataObjects.Invoice

'        '		''' <summary>
'        '		''' The invoice rows (Individual)
'        '		''' </summary>
'        '		Private m_invoiceRowsIndividual As List(Of DataObjects.InvoiceIndividual)

'        '		''' <summary>
'        '		''' The invoice rows (Automatisch)
'        '		''' </summary>
'        '		Private m_invoiceRowsRPL As List(Of DataObjects.InvoiceRPL)

'        '		''' <summary>
'        '		''' Contains the invoice number of the loaded invoice data.
'        '		''' </summary>
'        '		Private m_invoiceNumber As Integer?

'        '		''' <summary>
'        '		''' UI Utility functions.
'        '		''' </summary>
'        '		Private m_UtilityUI As UtilityUI

'        '		''' <summary>
'        '		''' Utility functions.
'        '		''' </summary>
'        '		Private m_Utility As Utility

'        '		''' <summary>
'        '		''' The logger.
'        '		''' </summary>
'        '		Private Shared m_Logger As ILogger = New Logger()

'        '		''' <summary>
'        '		''' Boolean flag indicating if initial data has been loaded.
'        '		''' </summary>
'        '		Private m_IsInitialDataLoaded As Boolean = False

'        '		''' <summary>
'        '		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
'        '		''' </summary>
'        '		Private m_SuppressUIEvents As Boolean = False

'        '		''' <summary>
'        '		''' The common settings.
'        '		''' </summary>
'        '		Private m_Common As CommonSetting

'        '		''' <summary>
'        '		''' The SPProgUtility object.
'        '		''' </summary>
'        '		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

'        '		Private m_md As Mandant
'        '		Private m_path As ClsProgPath

'        '		Private m_CostCenters As SP.DatabaseAccess.Common.DataObjects.CostCenters

'        '		Private Property m_PrintJobNr As String
'        '		Private Property m_SQL4Print As String
'        '		Private Property m_bPrintAsDesign As Boolean

'        '		Private m_SaveButton As NavBarItem
'        '		Private m_IsDataValid As Boolean = True

'        '		''' <summary>
'        '		''' WOS NavBar Item.
'        '		''' </summary>
'        '		Private m_Wos_P_Data As NavBarItem

'        '		''' <summary>
'        '		''' Boolan flag indicating if the form has been initialized.
'        '		''' </summary>
'        '		Private m_IsInitialized = False

'        '		''' <summary>
'        '		''' The customers popup data
'        '		''' </summary>
'        '		Private m_customersData As List(Of DataObjects.Customer)
'        '		Private m_selectedCustomer As DataObjects.Customer
'        '		Private m_addressData As List(Of DataObjects.CustomerReAddress)
'        '		Private m_selectedAddress As DataObjects.CustomerReAddress
'        '		Private m_mahnCodeData As List(Of DatabaseAccess.Customer.DataObjects.PaymentReminderCodeData) = Nothing
'        '		Private m_advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)
'        '		Private m_postingAccounts As New Dictionary(Of Integer, String)
'        '		Private m_InvoiceCaption As String

'        '		Private m_Guthaben_NavbarItem As NavBarItem
'        '		Private m_ChangeCstomerName_NavbarItem As NavBarItem
'        '		Private m_ChangeMahnstopDate_NavbarItem As NavBarItem

'        '		Private m_newInvoiceRow As DataObjects.IInvoiceRow

'        '		Private m_PropertyForm As frmInvoiceProperties

'        '#End Region

'        '#Region "Public Properties"

'        '		''' <summary>
'        '		''' Boolean flag indicating if invoice data is loaded.
'        '		''' </summary>
'        '		Public ReadOnly Property IsInvoiceDataLoaded As Boolean
'        '			Get
'        '				Return m_invoiceNumber.HasValue
'        '			End Get

'        '		End Property

'        '		''' <summary>
'        '		''' Gets or sets data valid flag.
'        '		''' </summary>
'        '		''' <returns>Data valid flag</returns>
'        '		Public Property IsDataValid As Boolean
'        '			Get
'        '				Return m_IsDataValid
'        '			End Get
'        '			Set(value As Boolean)

'        '				m_IsDataValid = value

'        '				If Not m_IsDataValid AndAlso Not m_SaveButton Is Nothing Then
'        '					m_SaveButton.Enabled = False
'        '				End If

'        '			End Set
'        '		End Property

'        '#End Region

'        '#Region "Private Properties"

'        '		''' <summary>
'        '		''' Gets the reference number to 10 setting.
'        '		''' </summary>
'        '		Private ReadOnly Property ReferenceNumbersTo10Setting As Boolean
'        '			Get

'        '				Dim ref10forfactoring As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_invoiceData.FakDat.Value.Year),
'        '																																														String.Format("MD_{0}/Debitoren/ref10forfactoring", m_InitializationData.MDData.MDNr)), False)

'        '				Return ref10forfactoring.HasValue AndAlso ref10forfactoring
'        '			End Get
'        '		End Property

'        '		'''' <summary>
'        '		'''' Gets the active invoice row.
'        '		'''' </summary>
'        '		'''' <returns>The active invoice row.</returns>
'        '		'''' <remarks>If the user enters a new row then the active row is not yet visible in the UI other whise the active row is the currently selected row.</remarks>
'        '		'Private ReadOnly Property ActiveInvoiceRow As DataObjects.IInvoiceRow
'        '		'	Get

'        '		'		If Not m_newInvoiceRow Is Nothing Then
'        '		'			Return m_newInvoiceRow
'        '		'		Else
'        '		'			Return SelectedInvoiceRow
'        '		'		End If

'        '		'	End Get
'        '		'End Property

'        '		'''' <summary>
'        '		'''' Get a boolean truth value indicating if an active invoic row is existing.
'        '		'''' </summary>
'        '		'''' <returns>Boolean truth value.</returns>
'        '		'Private ReadOnly Property HasActiveInvoiceRow As Boolean
'        '		'	Get
'        '		'		Dim hasActiveRow = Not m_newInvoiceRow Is Nothing OrElse gvInvoiceRows.SelectedRowsCount > 0

'        '		'		Return hasActiveRow
'        '		'	End Get
'        '		'End Property

'        '		'''' <summary>
'        '		'''' Gets the selected invoice row.
'        '		'''' </summary>
'        '		'''' <returns>The selected invoice row.</returns>
'        '		'Private ReadOnly Property SelectedInvoiceRow As DataObjects.IInvoiceRow
'        '		'	Get

'        '		'		If gvInvoiceRows.GetSelectedRows().Count > 0 Then
'        '		'			Return gvInvoiceRows.GetRow(gvInvoiceRows.GetSelectedRows(0))
'        '		'		Else
'        '		'			Return Nothing
'        '		'		End If

'        '		'	End Get
'        '		'End Property
'        '#End Region

'        '#Region "Constructor"

'        '		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

'        '			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
'        '			Try
'        '				' Mandantendaten
'        '				m_md = New Mandant
'        '				m_path = New ClsProgPath
'        '				m_Common = New CommonSetting
'        '				m_UtilityUI = New UtilityUI
'        '				m_Utility = New Utility

'        '				m_InitializationData = _setting
'        '				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

'        '			Catch ex As Exception
'        '				m_Logger.LogError(ex.ToString)

'        '			End Try

'        '			' Dieser Aufruf ist für den Designer erforderlich.
'        '			DevExpress.UserSkins.BonusSkins.Register()
'        '			DevExpress.Skins.SkinManager.EnableFormSkins()

'        '			m_SuppressUIEvents = True
'        '			InitializeComponent()
'        '			m_SuppressUIEvents = False

'        '			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

'        '			Dim connectionString As String = m_InitializationData.MDData.MDDbConn
'        '			m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
'        '			m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
'        '			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

'        '			DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor = DevComponents.DotNetBar.eToastGlowColor.Red
'        '			DevComponents.DotNetBar.ToastNotification.ToastFont = New System.Drawing.Font("tahoma", 8.25F, System.Drawing.FontStyle.Bold)
'        '			DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval = 2000

'        '			m_SettingsManager = New SettingsManager

'        '			' Translate controls.
'        '			'TranslateControls()
'        '			'm_InvoiceCaption = gpRechnungsdaten.Text

'        '			'' Button Click Handle
'        '			'AddHandler lueAdresse.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueBankdaten.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueAdvisor1.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueAdvisor2.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueInvoiceNumber.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueKst1.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueKst2.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueMandant.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueCurrency.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler daeDatum.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler daeDueDate.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler daeMahn1Mahnung.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler daeMahn2Mahnung.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler daeMahnIncasso.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler daeMahnKontoauszug.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler lueCountry.ButtonClick, AddressOf OnDropDownButtonClick
'        '			'AddHandler luePostcode.ButtonClick, AddressOf OnDropDownButtonClick

'        '			'Reset()

'        '			'CreateMyNavBar()

'        '		End Sub

'        '#End Region

'        '#Region "Public Methods"

'        '        ''' <summary>
'        '        ''' Show the data of an invoice.
'        '        ''' </summary>
'        '        ''' <param name="invoiceNumber">The invoice number.</param>
'        '        ''' <returns>Boolean flag indicating success.</returns>
'        '        Public Function LoadInvoiceData(ByVal invoiceNumber As Integer) As Boolean

'        '            If Not m_SaveButton Is Nothing Then
'        '                m_SaveButton.Enabled = True
'        '            End If

'        '            m_SuppressUIEvents = True

'        '            If Not m_IsInitialDataLoaded Then
'        '                LoadDropDownData()
'        '                m_IsInitialDataLoaded = True
'        '            End If

'        '            Dim success As Boolean = True

'        '            ' Load data
'        '            m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(invoiceNumber)

'        '            success = success And m_invoiceData IsNot Nothing
'        '            If success Then
'        '                LoadPostingAccounts()
'        '                MapInvoiceDataToUi()
'        '            End If

'        '            PrepareStatusAndNavigationBar(m_invoiceData)

'        '            m_invoiceNumber = IIf(success, m_invoiceData.ReNr, Nothing)

'        '            IsDataValid = success

'        '            ' Load data rows
'        '            If m_invoiceData.Art.ToUpper = "A" Then
'        '                LoadInvoiceRowsRPT()

'        '            Else
'        '                LoadInvoiceRowsInd()
'        '            End If

'        '            m_SuppressUIEvents = False

'        '            Return success
'        '        End Function

'        '        ''' <summary>
'        '        ''' Shows new invoice form.
'        '        ''' </summary>
'        '        Public Sub NewInvoice()
'        '            If m_InitializationData.MDData.ClosedMD = 1 Then Return

'        '            Dim frmNewInvoice As SP.KD.InvoiceMng.UI.frmNewInvoice = New SP.KD.InvoiceMng.UI.frmNewInvoice(m_InitializationData, Nothing)
'        '            frmNewInvoice.Show()
'        '            frmNewInvoice.BringToFront()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Saves invoice data.
'        '        ''' </summary>
'        '        Public Sub SaveInvoiceData()

'        '            If (ValidateData()) Then

'        '                Dim success As Boolean = False

'        '                If IsInvoiceDataLoaded Then

'        '                    ' Reload data 
'        '                    m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_invoiceData.ReNr)
'        '                    MapUiToInvoiceData()
'        '                    success = m_InvoiceDatabaseAccess.UpdateInvoice(m_invoiceData)

'        '                End If

'        '                Dim message As String = String.Empty

'        '                If (success) Then
'        '                    PrepareStatusAndNavigationBar(m_invoiceData)

'        '                    DevExpress.XtraEditors.XtraMessageBox.Show((m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")),
'        '                                                                                                                                                                                             m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxButtons.OK, MessageBoxIcon.Information)
'        '                Else
'        '                    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
'        '                End If

'        '            Else
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten können nicht gespeichert werden."))
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Deletes the invoice.
'        '        ''' </summary>
'        '        Public Sub DeleteInvoice()

'        '            Dim selectedEmployeeRPL = m_invoiceData.Id
'        '            Dim docGuid = m_invoiceData.REDoc_Guid
'        '            Dim customerNumber As Integer = m_invoiceData.KdNr
'        '            Dim strFileName As String = String.Empty
'        '            Dim msg As String

'        '            If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie Rechnung wirklich löschen?"),
'        '                                                                            m_Translate.GetSafeTranslationValue("Rechnung löschen")) = False) Then
'        '                Return
'        '            End If

'        '            Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.InvoicePrintData With {.frmhwnd = Me.Handle,
'        '                                                                                                                                                                                .PrintInvoiceAsCopy = False,
'        '                                                                                                                                                                                .ShowAsDesign = False,
'        '                                                                                                                                                                                .ExportPrintInFiles = True}
'        '            Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintInvoice(m_InitializationData)
'        '            _setting.InvoiceNumbers = New List(Of Integer)(New Integer() {m_invoiceNumber})
'        '            printUtil.PrintData = _setting
'        '            Dim exportResult = printUtil.PrintInvoice()
'        '            printUtil.Dispose()

'        '            If _setting.ExportedFiles.Count = 0 Then
'        '                msg = String.Format(m_Translate.GetSafeTranslationValue("Die Rechnungsvorlage {0} konnte nicht erstellt werden.{1}Möglicherweise hat die Rechnung keine Detailzeile!{1}{1}Soll die Rechnung trotzdem geöscht werden?"), m_invoiceNumber, vbNewLine)
'        '                m_Logger.LogWarning(msg)
'        '                If Not m_UtilityUI.ShowYesNoDialog(msg, "Rechnung ohne Detailzeile löschen?") Then Return

'        '            Else
'        '                strFileName = _setting.ExportedFiles(0)
'        '                If _setting.ExportedFiles.Count > 1 Then
'        '                    Dim pdfDocument As New PdfDocumentProcessor()
'        '                    pdfDocument.LoadDocument(strFileName)

'        '                    For i As Integer = 1 To _setting.ExportedFiles.Count - 1
'        '                        pdfDocument.AppendDocument(_setting.ExportedFiles(i))
'        '                        pdfDocument.SaveDocument(strFileName)
'        '                    Next
'        '                    pdfDocument.CloseDocument()
'        '                End If

'        '                If Not String.IsNullOrWhiteSpace(strFileName) AndAlso Not IO.File.Exists(strFileName) Then
'        '                    msg = String.Format("Die Datei für die Rechnungsvorlage {0} konnte nicht gefunden werden.", m_invoiceNumber)
'        '                    m_Logger.LogWarning(msg)
'        '                    m_UtilityUI.ShowErrorDialog(msg)

'        '                    Return
'        '                End If

'        '            End If

'        '            Dim result = New DeleteREResult
'        '            If Not String.IsNullOrWhiteSpace(strFileName) Then
'        '                Dim fileInfo As New IO.FileInfo(strFileName)
'        '                Dim fileByte() = m_Utility.LoadFileBytes(fileInfo.FullName)

'        '                result = m_InvoiceDatabaseAccess.DeleteInvoiceAndInsertInvoiceDocumentIntoDeleteDb(m_invoiceData.Id, ConstantValues.ModulName,
'        '                                                                                                                                                                     String.Format("{0}, {1}", m_InitializationData.UserData.UserLName, m_InitializationData.UserData.UserFName),
'        '                                                                                                                                                                     m_InitializationData.UserData.UserNr, If(fileByte.Length = 0, Nothing, fileByte))
'        '            Else
'        '                result = m_InvoiceDatabaseAccess.DeleteInvoice(m_invoiceData.Id, ConstantValues.ModulName,
'        '                                                                                                                 String.Format("{0}, {1}", m_InitializationData.UserData.UserLName, m_InitializationData.UserData.UserFName),
'        '                                                                                                                 m_InitializationData.UserData.UserNr)

'        '            End If

'        '            Select Case result
'        '                Case DeleteREResult.ResultCanNotDeleteBecauseMonthIsClosed
'        '                    m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnung kann nicht gelöscht werden, da der Monat bereits abgeschlossen ist."),
'        '                                                                                                                                                                             m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
'        '                Case DeleteREResult.ResultCanNotDeleteBecauseOfExistingZE
'        '                    m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnung kann nicht gelöscht werden, da bereits ein Zahlungseingang existiert."),
'        '                                                                                                                                     m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
'        '                Case DeleteREResult.ResultCanNotDeleteBecauseOfPartlyPayed
'        '                    m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnung kann nicht gelöscht werden, da bereits eine Teilzahlung existiert."),
'        '                                                                                                                                     m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
'        '                Case DeleteREResult.ResultDeleteError
'        '                    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Rechnung konnte nicht gelöscht werden."))
'        '                Case DeleteREResult.ResultDeleteOk

'        '                    Try

'        '                        If Not String.IsNullOrWhiteSpace(docGuid) Then

'        '                            Dim resultWOS As WOSSendResult = New WOSSendResult With {.Value = True}

'        '                            If m_invoiceData Is Nothing Then Return
'        '                            Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)

'        '                            Dim wosSetting = New WOSSendSetting

'        '                            wosSetting.InvoiceNumber = m_invoiceNumber
'        '                            wosSetting.CustomerNumber = customerNumber
'        '                            wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rechnung
'        '                            wosSetting.CustomerDocumentGuid = docGuid

'        '                            wos.WOSSetting = wosSetting
'        '                            resultWOS.Value = wos.DeleteTransferedCustomerDocument()
'        '                            If Not resultWOS.Value Then m_Logger.LogWarning(resultWOS.Message)

'        '                        End If


'        '                        'If System.IO.File.Exists(strFileName) Then
'        '                        '	Try
'        '                        '		oMyProg.TranslateProg4Net("DeleteSelectedrecNet", m_invoiceNumber, strFileName)
'        '                        '	Catch ex As Exception
'        '                        '		m_Logger.LogError(String.Format("DeleteSelectedrecNet: {0}", ex.tostring))
'        '                        '	End Try
'        '                        'Else
'        '                        '	m_Logger.LogWarning(String.Format("(DeleteSelectedrecNet): File not founded: {0}", strFileName))
'        '                        'End If

'        '                    Catch ex As Exception
'        '                        m_Logger.LogError(String.Format("Rechnung in WOS löschen: {0}", ex.ToString))
'        '                        m_UtilityUI.ShowErrorDialog(String.Format("{1}{0}Die Rechnung konnte in WOS nicht gelöscht werden!", vbNewLine, ex.ToString))

'        '                    End Try

'        '                    m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Rechnung wurde gelöscht."))
'        '                    Me.Close()
'        '            End Select

'        '        End Sub

'        '#End Region


'        '#Region "Private Methods"


'        '#Region "Reset Form"

'        '        ''' <summary>
'        '        ''' Resets the from.
'        '        ''' </summary>
'        '        Private Sub Reset()

'        '            SwitchReadOnlyFields(True)

'        '            m_SuppressUIEvents = True
'        '            m_invoiceNumber = Nothing
'        '            m_invoiceData = Nothing
'        '            m_invoiceRowsIndividual = Nothing
'        '            m_invoiceRowsRPL = Nothing
'        '            m_customersData = Nothing
'        '            m_selectedCustomer = Nothing
'        '            m_addressData = Nothing
'        '            m_selectedAddress = Nothing
'        '            m_newInvoiceRow = Nothing

'        '            'clear all input field

'        '            txtAbteilung.Text = String.Empty
'        '            txtBetragBezahlt.Text = String.Empty
'        '            txtBetragZE.Text = String.Empty
'        '            txtBetragOffen.Text = String.Empty
'        '            txtDebtLoss.Text = String.Empty
'        '            txtFirma2.Text = String.Empty
'        '            txtFirma3.Text = String.Empty
'        '            txtLocation.Text = String.Empty
'        '            txtReImbursement.Text = String.Empty
'        '            txtSkonto.Text = String.Empty
'        '            txtMwStPercen.Text = String.Empty
'        '            txtRowValueMwSt.Text = String.Empty
'        '            txtPostfach.Text = String.Empty
'        '            txtRowValueTotal.Text = String.Empty
'        '            txtStreet.Text = String.Empty
'        '            txtRowValueExcl.Text = String.Empty
'        '            txtZuHd.Text = String.Empty
'        '            txtRestBetragInProzent.Text = String.Empty
'        '            txtFibuHaben1.Text = String.Empty
'        '            txtRestBetrag.Text = String.Empty
'        '            btnShowPayments.Visible = False

'        '            lblMahnstopValue.Text = String.Empty
'        '            lblMahnstopValue.BackColor = Color.Empty
'        '            lblSelectedAdvisors.Text = String.Empty

'        '            daeValutaDate.EditValue = Nothing
'        '            daeDueDate.EditValue = Nothing
'        '            daeMahn1Mahnung.EditValue = Nothing
'        '            daeMahn2Mahnung.EditValue = Nothing
'        '            daeMahnIncasso.EditValue = Nothing
'        '            daeMahnKontoauszug.EditValue = Nothing

'        '            medZahlungsinformation.Text = String.Empty
'        '            medRowHead.Text = String.Empty
'        '            medRowDetail.Text = String.Empty

'        '            txtMwStPercen.Properties.ReadOnly = True

'        '            ' ---Reset drop downs, grids and lists---
'        '            ResetAddressDropDown()
'        '            ResetAdvisorDropDown()
'        '            ResetBankdatenDropDown()
'        '            ResetBranchDropDown()
'        '            ResetCountryDropDown()
'        '            ResetCurrencyDropDown()
'        '            ResetDebitorenartDropDown()
'        '            ResetKstDropDown()
'        '            ResetMandantDropDown()
'        '            ResetPaymentConditionDropDown()
'        '            ResetPostcodeDropDown()

'        '            LoadAdvisorDropDown()
'        '            LoadDebitorenartDropDown()
'        '            LoadPostcodeDropDownData()

'        '            ResetGridInvoiceRows()

'        '            m_SuppressUIEvents = False

'        '            errorProvider.Clear()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the invoice rows grid
'        '        ''' </summary>
'        '        Private Sub ResetGridInvoiceRows()

'        '            ' Reset the grid
'        '            gvInvoiceRows.OptionsView.ShowIndicator = False
'        '            gvInvoiceRows.OptionsSelection.EnableAppearanceFocusedCell = False

'        '            gvInvoiceRows.Columns.Clear()
'        '            gridInvoiceRows.DataSource = Nothing

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the Address drop down.
'        '        ''' </summary>
'        '        Private Sub ResetAddressDropDown()

'        '            lueAdresse.Properties.DisplayMember = "Address"
'        '            lueAdresse.Properties.ValueMember = "Id"

'        '            lueAdresse.Properties.Columns.Clear()
'        '            lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REFirma", 0))
'        '            lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REStrasse", 0))
'        '            lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REPLZ", 0))
'        '            lueAdresse.Properties.Columns.Add(New LookUpColumnInfo("REOrt", 0))

'        '            lueAdresse.Properties.DataSource = Nothing
'        '            lueAdresse.EditValue = Nothing
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the Mandant drop down.
'        '        ''' </summary>
'        '        Private Sub ResetMandantDropDown()

'        '            lueMandant.Properties.DisplayMember = "MandantName1"
'        '            lueMandant.Properties.ValueMember = "MandantNumber"

'        '            lueMandant.Properties.Columns.Clear()
'        '            lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
'        '                                                                                                                                                                     .Width = 100,
'        '                                                                                                                                                                     .Caption = m_Translate.GetSafeTranslationValue("Mandant")})
'        '            lueMandant.EditValue = Nothing
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the branch drop down.
'        '        ''' </summary>
'        '        Private Sub ResetBranchDropDown()
'        '            lueBranch.Properties.DisplayMember = "TranslatedBrancheText"
'        '            lueBranch.Properties.ValueMember = "Branche"

'        '            ' Reset the grid view
'        '            gvLueBranch.OptionsView.ShowIndicator = False

'        '            gvLueBranch.Columns.Clear()

'        '            Dim columnBranchText As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            columnBranchText.Caption = String.Empty
'        '            columnBranchText.Name = "TranslatedBrancheText"
'        '            columnBranchText.FieldName = "TranslatedBrancheText"
'        '            columnBranchText.Visible = True
'        '            gvLueBranch.Columns.Add(columnBranchText)

'        '            lueBranch.Properties.BestFitMode = BestFitMode.BestFitResizePopup
'        '            lueBranch.Properties.NullText = String.Empty
'        '            lueBranch.Properties.DataSource = Nothing
'        '            lueBranch.EditValue = Nothing
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the country drop down.
'        '        ''' </summary>
'        '        Private Sub ResetCountryDropDown()

'        '            lueCountry.Properties.DisplayMember = "Name"
'        '            lueCountry.Properties.ValueMember = "Code"

'        '            lueCountry.Properties.Columns.Clear()
'        '            lueCountry.Properties.Columns.Add(New LookUpColumnInfo("Name", 0))

'        '            lueCountry.Properties.NullText = String.Empty
'        '            lueCountry.EditValue = Nothing

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the postcode drop down.
'        '        ''' </summary>
'        '        Private Sub ResetPostcodeDropDown()

'        '            ' Invoice address post code data
'        '            luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
'        '            luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

'        '            luePostcode.Properties.DisplayMember = "Postcode"
'        '            luePostcode.Properties.ValueMember = "Postcode"

'        '            Dim columns = luePostcode.Properties.Columns
'        '            columns.Clear()
'        '            columns.Add(New LookUpColumnInfo("Postcode", 0))
'        '            columns.Add(New LookUpColumnInfo("Location", 0))

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the payment condition drop down.
'        '        ''' </summary>
'        '        Private Sub ResetPaymentConditionDropDown()

'        '            luePaymentConditions.Properties.DisplayMember = "GetField"
'        '            luePaymentConditions.Properties.ValueMember = "GetField"

'        '            Dim columns = luePaymentConditions.Properties.Columns
'        '            columns.Clear()
'        '            columns.Add(New LookUpColumnInfo("GetField", 0))

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the Bankdaten drop down.
'        '        ''' </summary>
'        '        Private Sub ResetBankdatenDropDown()

'        '            lueBankdaten.Properties.DisplayMember = "BankName"
'        '            lueBankdaten.Properties.ValueMember = "BankName"

'        '            lueBankdaten.Properties.Columns.Clear()
'        '            lueBankdaten.Properties.Columns.Add(New LookUpColumnInfo("KontoESR2", 0))
'        '            lueBankdaten.Properties.Columns.Add(New LookUpColumnInfo("BankName", 0))

'        '            lueBankdaten.EditValue = Nothing

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the Kst1 and Kst2 drop down.
'        '        ''' </summary>
'        '        Private Sub ResetKstDropDown()

'        '            'Kst1
'        '            lueKst1.Properties.DisplayMember = "KSTBezeichnung"
'        '            lueKst1.Properties.ValueMember = "KSTName"

'        '            lueKst1.Properties.Columns.Clear()
'        '            lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
'        '            lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

'        '            lueKst1.EditValue = Nothing

'        '            'Kst2
'        '            lueKst2.Properties.DisplayMember = "KSTBezeichnung"
'        '            lueKst2.Properties.ValueMember = "KSTName"

'        '            lueKst2.Properties.Columns.Clear()
'        '            lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
'        '            lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

'        '            lueKst2.EditValue = Nothing
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Resets the Advisor1 and Advisor2 drop down.
'        '        ''' </summary>
'        '        Private Sub ResetAdvisorDropDown()

'        '            'Advisor1
'        '            lueAdvisor1.Properties.DisplayMember = "UserFullname"
'        '            lueAdvisor1.Properties.ValueMember = "KST"

'        '            lueAdvisor1.Properties.Columns.Clear()
'        '            lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("KST", 0))
'        '            lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("UserFullname", 0))

'        '            lueAdvisor1.EditValue = Nothing

'        '            'Advisor2
'        '            lueAdvisor2.Properties.DisplayMember = "UserFullname"
'        '            lueAdvisor2.Properties.ValueMember = "KST"

'        '            lueAdvisor2.Properties.Columns.Clear()
'        '            lueAdvisor2.Properties.Columns.Add(New LookUpColumnInfo("KST", 0))
'        '            lueAdvisor2.Properties.Columns.Add(New LookUpColumnInfo("UserFullname", 0))

'        '            lueAdvisor2.EditValue = Nothing

'        '        End Sub


'        '        ''' <summary>
'        '        ''' Resets the Debitorenart drop down.
'        '        ''' </summary>
'        '        Private Sub ResetDebitorenartDropDown()

'        '            lueInvoiceNumber.Properties.DisplayMember = "Label"
'        '            lueInvoiceNumber.Properties.ValueMember = "Value"

'        '            lueInvoiceNumber.Properties.Columns.Clear()
'        '            lueInvoiceNumber.Properties.Columns.Add(New LookUpColumnInfo("Value", 0))
'        '            lueInvoiceNumber.Properties.Columns.Add(New LookUpColumnInfo("Display", 0))

'        '            lueInvoiceNumber.EditValue = Nothing

'        '        End Sub


'        '        ''' <summary>
'        '        ''' Resets the Currency drop down.
'        '        ''' </summary>
'        '        Private Sub ResetCurrencyDropDown()

'        '            lueCurrency.Properties.DisplayMember = "Code"
'        '            lueCurrency.Properties.ValueMember = "Code"

'        '            lueCurrency.Properties.Columns.Clear()
'        '            lueCurrency.Properties.Columns.Add(New LookUpColumnInfo("Code", 0))
'        '            lueCurrency.Properties.Columns.Add(New LookUpColumnInfo("Description", 0))

'        '            lueCurrency.EditValue = Nothing

'        '        End Sub

'        '#End Region

'        '#Region "Load master Data"

'        '        ''' <summary>
'        '        ''' Loads the DropDown Data
'        '        ''' </summary>
'        '        ''' <remarks></remarks>
'        '        Private Sub LoadDropDownData()
'        '            ' on customer selection: LoadBranchDropDown()
'        '            LoadCountryDropDownData()
'        '            LoadCurrencyDropDown()
'        '            ' in reset: LoadPostcodeDropDownData()
'        '            LoadPaymetConditionDropDownData()
'        '            LoadMandantDropDown()
'        '            LoadBankdatenDropDown()
'        '            LoadKst1DropDown()
'        '            ' in reset: LoadAdvisorDropDown()
'        '            ' in reset: LoadDebitorenartDropDown()
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the country drop down data.
'        '        ''' </summary>
'        '        Private Sub LoadCountryDropDownData()
'        '            Dim countryData = m_CommonDatabaseAccess.LoadCountryData()

'        '            If (countryData Is Nothing) Then
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten konnten nicht geladen werden."))
'        '            End If

'        '            lueCountry.Properties.DataSource = countryData
'        '            lueCountry.Properties.ForceInitialize()
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the currency drop down data.
'        '        ''' </summary>
'        '        Private Function LoadCurrencyDropDown() As Boolean
'        '            Dim currencyData = m_CommonDatabaseAccess.LoadCurrencyData()

'        '            If (currencyData Is Nothing) Then
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Währungsdaten konnten nicht geladen werden."))
'        '            End If

'        '            lueCurrency.Properties.DataSource = currencyData
'        '            lueCurrency.Properties.ForceInitialize()

'        '            Return Not currencyData Is Nothing
'        '        End Function

'        '        ''' <summary>
'        '        ''' Loads the postcode drop downdata.
'        '        ''' </summary>
'        '        Private Sub LoadPostcodeDropDownData()
'        '            Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

'        '            If (postcodeData Is Nothing) Then
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleizahldaten konnten nicht geladen werden."))
'        '            End If

'        '            luePostcode.Properties.DataSource = postcodeData
'        '            luePostcode.Properties.ForceInitialize()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the payment condition drop downdata.
'        '        ''' </summary>
'        '        Private Sub LoadPaymetConditionDropDownData()
'        '            Dim paymentConditionData = m_CustomerDatabaseAccess.LoadPaymentConditionData()

'        '            If (paymentConditionData Is Nothing) Then
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zahlungskonditionen konnten nicht geladen werden."))
'        '            End If

'        '            luePaymentConditions.Properties.DataSource = paymentConditionData
'        '            luePaymentConditions.Properties.ForceInitialize()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the Posting Accounts from XML
'        '        ''' </summary>
'        '        ''' <remarks></remarks>
'        '        Private Sub LoadPostingAccounts()

'        '            m_postingAccounts = New Dictionary(Of Integer, String)
'        '            Try

'        '                Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/BuchungsKonten", m_InitializationData.MDData.MDNr)
'        '                For i As Integer = 1 To 38
'        '                    Dim strValue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(m_md.GetDefaultMDNr, m_invoiceData.FakDat.Value.Year),
'        '                                                                                                                                                                    String.Format("{0}/_{1}", FORM_XML_MAIN_KEY, i)), "")
'        '                    m_postingAccounts.Add(i, strValue)
'        '                Next

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString)
'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the Mandant drop down data.
'        '        ''' </summary>
'        '        Private Function LoadMandantDropDown() As Boolean
'        '            Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

'        '            If (mandantData Is Nothing) Then
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
'        '            End If

'        '            lueMandant.Properties.DataSource = mandantData
'        '            lueMandant.Properties.ForceInitialize()

'        '            Return mandantData IsNot Nothing
'        '        End Function

'        '        ''' <summary>
'        '        ''' Loads the ReAdress drop down data.
'        '        ''' </summary>
'        '        Private Function LoadReAddressDropDown(ByVal setDefault As Boolean) As Boolean
'        '            If m_selectedCustomer Is Nothing Then
'        '                m_addressData = Nothing
'        '                m_selectedAddress = Nothing
'        '                Return False
'        '            End If

'        '            m_addressData = m_InvoiceDatabaseAccess.LoadCustomerReAddressData(m_selectedCustomer.KDNr)

'        '            If (m_addressData Is Nothing) Then
'        '                Return False
'        '                'm_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Adressdaten konnten nicht geladen werden."))
'        '            End If

'        '            lueAdresse.Properties.DataSource = m_addressData
'        '            lueAdresse.Properties.ForceInitialize()

'        '            'select active adress
'        '            Dim activeAdress = (From a In m_addressData Where a.IsActive).FirstOrDefault
'        '            If activeAdress Is Nothing Then
'        '                activeAdress = (From a In m_addressData Order By a.RecNr).FirstOrDefault
'        '            End If

'        '            If setDefault AndAlso activeAdress IsNot Nothing Then
'        '                lueAdresse.EditValue = activeAdress.Id
'        '            End If
'        '            Return m_addressData IsNot Nothing
'        '        End Function

'        '        ''' <summary>
'        '        ''' Loads the Bankdaten drop down data.
'        '        ''' </summary>
'        '        Private Function LoadBankdatenDropDown() As Boolean
'        '            ' Load data
'        '            If lueMandant.EditValue IsNot Nothing Then
'        '                Dim bankData As List(Of DataObjects.BankData) = m_InvoiceDatabaseAccess.LoadBankData(lueMandant.EditValue)

'        '                If (bankData Is Nothing) Then
'        '                    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht geladen werden."))
'        '                End If

'        '                lueBankdaten.Properties.DataSource = bankData
'        '                lueBankdaten.Properties.ForceInitialize()

'        '                Return bankData IsNot Nothing
'        '            End If
'        '            Return False
'        '        End Function

'        '        ''' <summary>
'        '        ''' Loads the Kst1 drop down data.
'        '        ''' </summary>
'        '        Private Sub LoadKst1DropDown()
'        '            ' Load data
'        '            m_CostCenters = m_CommonDatabaseAccess.LoadCostCenters()

'        '            ' Kst1
'        '            lueKst1.Properties.DataSource = m_CostCenters.CostCenter1
'        '            lueKst1.Properties.ForceInitialize()

'        '            ' Kst2
'        '            lueKst2.EditValue = Nothing
'        '            lueKst2.Properties.DataSource = Nothing
'        '            lueKst2.Properties.ForceInitialize()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the Kst2 drop down data.
'        '        ''' </summary>
'        '        Private Sub LoadKst2DropDown()

'        '            If (m_CostCenters Is Nothing) Then
'        '                Return
'        '            End If

'        '            Dim kst1Name = lueKst1.EditValue
'        '            Dim kst2Data = m_CostCenters.GetCostCenter2ForCostCenter1(kst1Name)

'        '            ' Kst2
'        '            lueKst2.EditValue = Nothing
'        '            lueKst2.Properties.DataSource = kst2Data
'        '            lueKst2.Properties.ForceInitialize()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the Advisor1 and Advisor2 drop down data.
'        '        ''' </summary>
'        '        Private Sub LoadAdvisorDropDown()
'        '            ' Load data
'        '            m_advisors = m_CommonDatabaseAccess.LoadAdvisorData()

'        '            ' Advisor1
'        '            lueAdvisor1.Properties.DataSource = m_advisors
'        '            lueAdvisor1.Properties.ForceInitialize()

'        '            ' Advisor2
'        '            lueAdvisor2.Properties.DataSource = m_advisors
'        '            lueAdvisor2.Properties.ForceInitialize()
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the Debitorenart drop down data.
'        '        ''' </summary>
'        '        Private Sub LoadDebitorenartDropDown()
'        '            Dim debitorenart = New List(Of Debitorenart) From {
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Individuelle Debitoren"), .Value = "I"},
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Festanstellung"), .Value = "F"},
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Gutschrift automatische Debitoren"), .Value = "GA"},
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Gutschrift individuelle Debitoren"), .Value = "GI"},
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Gutschrift Festanstellung"), .Value = "GF"},
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Rückvergütung automatische Debitoren"), .Value = "RA"},
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Rückvergütung individuelle Debitoren"), .Value = "RI"},
'        '                    New Debitorenart With {.Display = m_Translate.GetSafeTranslationValue("Rückvergütung Festanstellung"), .Value = "RF"}
'        '             }

'        '            lueInvoiceNumber.Properties.DataSource = debitorenart

'        '            lueInvoiceNumber.Properties.ForceInitialize()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the branch drop down data.
'        '        ''' </summary>
'        '        Private Function LoadBranchDropDownData() As Boolean
'        '            If m_selectedCustomer Is Nothing Then
'        '                Return True
'        '            End If
'        '            Dim customerNumber = m_selectedCustomer.KDNr
'        '            Dim allBranchData As List(Of DatabaseAccess.Common.DataObjects.BranchData) = Nothing

'        '            ' Load data
'        '            Try

'        '                allBranchData = m_CommonDatabaseAccess.LoadBranchData()
'        '                Dim customerAssignedBranchData = m_CustomerDatabaseAccess.LoadAssignedSectorDataOfCustomer(customerNumber)

'        '                Dim mergedBranchData As List(Of BranchViewData) = Nothing

'        '                If (allBranchData Is Nothing) Then
'        '                    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Branchen konnten nicht geladen werden."))
'        '                End If

'        '                If customerAssignedBranchData Is Nothing Then
'        '                    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kundenbranchen konnten nicht geladen werden."))
'        '                Else

'        '                    '--- This section arranges the branches so that the customer assigned ones will be at the top ---

'        '                    ' Create an empty list that will contain the merged branches of the customer and the ones from the master table
'        '                    mergedBranchData = New List(Of BranchViewData)

'        '                    For Each customerAssignedBranch In customerAssignedBranchData

'        '                        Dim customerBranchCode = customerAssignedBranch.SectorCode

'        '                        If customerBranchCode.HasValue AndAlso customerBranchCode <> 0 Then

'        '                            ' Try to find branch in master data
'        '                            Dim extistingBranch = allBranchData.Where(Function(branch) branch.Code = customerBranchCode).FirstOrDefault()

'        '                            If Not extistingBranch Is Nothing Then

'        '                                ' The branch could be found -> add to merged results..
'        '                                mergedBranchData.Add(New BranchViewData With {.Branche = extistingBranch.Branche,
'        '                                                                                                                                                                                                                        .TranslatedBrancheText = extistingBranch.TranslatedBrancheText,
'        '                                                                                                                                                                                                                        .IsAssignedToCustomer = True})

'        '                                ' Remove from all branch data. 
'        '                                allBranchData.Remove(extistingBranch)
'        '                            Else

'        '                                ' The branch could not be found. Most likely the sector code is set to 0 in the customer assigned branch data.
'        '                                ' The data is added manually. Translation is not taken into account!
'        '                                mergedBranchData.Add(New BranchViewData With {.Branche = customerAssignedBranch.Description,
'        '                                                                                                                                                                                                                        .TranslatedBrancheText = customerAssignedBranch.Description,
'        '                                                                                                                                                                                                                        .IsAssignedToCustomer = True})

'        '                            End If

'        '                        End If

'        '                    Next

'        '                    ' Add branches that are left in allBranchData
'        '                    For Each branch In allBranchData
'        '                        mergedBranchData.Add(New BranchViewData With {.Branche = branch.Branche,
'        '                                                                                                                                                                                                                .TranslatedBrancheText = branch.TranslatedBrancheText,
'        '                                                                                                                                                                                                                .IsAssignedToCustomer = False})
'        '                    Next
'        '                End If

'        '                lueBranch.Properties.DataSource = mergedBranchData

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString)

'        '            End Try

'        '            Return Not allBranchData Is Nothing
'        '        End Function

'        '#End Region

'        '#Region "Load, save Invoice Data"

'        '        ''' <summary>
'        '        ''' Validates the data on the form.
'        '        ''' </summary>
'        '        Private Function ValidateData() As Boolean

'        '            Dim valid As Boolean = True
'        '            Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
'        '            Dim errorDueDate As String = m_Translate.GetSafeTranslationValue("Fällig-Datum muss nach dem Rechnungs-Datum liegen.")
'        '            Try
'        '                'mandatory fields
'        '                valid = valid And SetErrorIfInvalid(lueMandant, errorProvider, lueMandant.EditValue Is Nothing, errorText)
'        '                valid = valid And SetErrorIfInvalid(lueAdvisor2, errorProvider, String.IsNullOrWhiteSpace(lblSelectedAdvisors.Text), errorText)

'        '                valid = valid And SetErrorIfInvalid(lueInvoiceNumber, errorProvider, lueInvoiceNumber.EditValue Is Nothing, errorText)
'        '                valid = valid And SetErrorIfInvalid(lueBankdaten, errorProvider, lueBankdaten.EditValue Is Nothing, errorText)
'        '                valid = valid And SetErrorIfInvalid(daeValutaDate, errorProvider, daeValutaDate.EditValue Is Nothing, errorText)
'        '                valid = valid And SetErrorIfInvalid(daeDueDate, errorProvider, daeDueDate.EditValue Is Nothing, errorText)
'        '                valid = valid And SetErrorIfInvalid(daeDueDate, errorProvider, CType(daeValutaDate.EditValue, Date) > CType(daeDueDate.EditValue, Date), errorDueDate)
'        '                valid = valid And SetErrorIfInvalid(cboCustomer, errorProvider, m_selectedCustomer Is Nothing, errorText)
'        '                valid = valid And SetErrorIfInvalid(lueAdresse, errorProvider, m_selectedAddress Is Nothing, errorText)

'        '                valid = valid And SetErrorIfInvalid(lueAdresse, errorProvider, m_InitializationData.MDData.ClosedMD = 1, errorText)

'        '            Catch ex As Exception
'        '                valid = False
'        '            End Try

'        '            Return valid

'        '        End Function

'        '        ''' <summary>
'        '        ''' Maps all data from DTO to UI
'        '        ''' </summary>
'        '        Private Sub MapInvoiceDataToUi()
'        '            gpRechnungsdaten.Text = String.Format(m_InvoiceCaption, m_invoiceData.ReNr)

'        '            'Eigenschaften und Merkmale
'        '            lueMandant.EditValue = m_invoiceData.MDNr
'        '            SelectKst1(m_invoiceData.REKST1)
'        '            lueKst2.EditValue = m_invoiceData.REKST2
'        '            ' split Advisors
'        '            Dim advisors = m_invoiceData.KST.Split({"/"c})
'        '            If advisors.Length = 1 Then
'        '                SelectAdvisor(lueAdvisor1, advisors(0))
'        '                SelectAdvisor(lueAdvisor2, advisors(0))
'        '            ElseIf advisors.Length = 2 Then
'        '                SelectAdvisor(lueAdvisor1, advisors(0))
'        '                SelectAdvisor(lueAdvisor2, advisors(1))
'        '            End If

'        '            'Kundendaten
'        '            Try
'        '                SetCustomer(m_invoiceData)
'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString)
'        '            End Try

'        '            lueBranch.EditValue = m_invoiceData.KDBranche

'        '            Try
'        '                LoadReAddressDropDown(False)

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString)
'        '            End Try

'        '            lueAdresse.EditValue = Nothing
'        '            m_selectedAddress = New DataObjects.CustomerReAddress With {
'        '                                    .Id = Nothing,
'        '                                    .KDNr = m_invoiceData.KdNr,
'        '                                    .REFirma = m_invoiceData.RName1,
'        '                                    .REFirma2 = m_invoiceData.RName2,
'        '                                    .REFirma3 = m_invoiceData.RName3,
'        '                                    .REStrasse = m_invoiceData.RStrasse,
'        '                                    .REPLZ = m_invoiceData.RPLZ,
'        '                                    .REOrt = m_invoiceData.ROrt,
'        '                                    .RELand = m_invoiceData.RLand,
'        '                                    .REAbteilung = m_invoiceData.RAbteilung,
'        '                                    .REZhd = m_invoiceData.RZHD,
'        '                                    .REPostfach = m_invoiceData.RPostfach,
'        '                                    .RecNr = Nothing,
'        '                                    .MahnCode = m_invoiceData.Mahncode
'        '            }

'        '            Try
'        '                DisplayAdress()

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString)

'        '            End Try

'        '            luePaymentConditions.EditValue = m_invoiceData.Zahlkond
'        '            lueCurrency.EditValue = m_invoiceData.Currency

'        '            txtMwStPercen.Text = m_invoiceData.MWSTProz

'        '            'Rechnungsdaten
'        '            Dim debitorenart As String = m_invoiceData.Art.ToUpper + m_invoiceData.Art2.ToUpper
'        '            If m_invoiceData.Art.ToUpper = "I" Or m_invoiceData.Art.ToUpper = "F" Then
'        '                debitorenart = m_invoiceData.Art.ToUpper
'        '            ElseIf m_invoiceData.Art.ToUpper = "A" Then
'        '                CType(lueInvoiceNumber.Properties.DataSource, List(Of Debitorenart)).Add(
'        '                        New Debitorenart With {.Display = "Automatische Debitoren", .Value = "A"}
'        '                )

'        '                debitorenart = m_invoiceData.Art.ToUpper
'        '            End If

'        '            lueInvoiceNumber.EditValue = debitorenart
'        '            lueBankdaten.EditValue = m_invoiceData.ESRBankName
'        '            daeValutaDate.EditValue = m_invoiceData.FakDat
'        '            daeDueDate.EditValue = m_invoiceData.Faellig

'        '            'MahnDaten
'        '            daeMahnKontoauszug.EditValue = m_invoiceData.MA0
'        '            daeMahn1Mahnung.EditValue = m_invoiceData.MA1
'        '            daeMahn2Mahnung.EditValue = m_invoiceData.MA2
'        '            daeMahnIncasso.EditValue = m_invoiceData.MA3

'        '            SetMahnStopValue(m_invoiceData.MahnStopUntil)

'        '            ' Beträge
'        '            txtSkonto.Text = m_invoiceData.BetragOhne
'        '            txtDebtLoss.Text = m_invoiceData.BetragEx
'        '            txtReImbursement.Text = m_invoiceData.MWST1
'        '            txtBetragZE.Text = m_invoiceData.BetragInk
'        '            txtBetragBezahlt.Text = m_invoiceData.Bezahlt
'        '            txtBetragOffen.Text = m_invoiceData.BetragInk - m_invoiceData.Bezahlt

'        '            txtRestBetrag.EditValue = m_invoiceData.FKSoll
'        '            txtRestBetragInProzent.EditValue = m_invoiceData.FKHaben0
'        '            txtFibuHaben1.EditValue = m_invoiceData.FKHaben1

'        '            btnShowPayments.Visible = (m_invoiceData.Art = "A" OrElse m_invoiceData.Art = "I" OrElse m_invoiceData.Art = "F") AndAlso m_invoiceData.Bezahlt > 0

'        '            medZahlungsinformation.Text = m_invoiceData.ZEInfo

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Maps the UI-Data to DTO
'        '        ''' </summary>
'        '        Private Sub MapUiToInvoiceData()

'        '            ' new Invoice: initialization
'        '            If Not IsInvoiceDataLoaded Then

'        '            End If

'        '            Dim refNr As String = String.Empty
'        '            Dim refFootNr As String = String.Empty

'        '            ' Update invoice data
'        '            With m_invoiceData
'        '                .KdNr = m_selectedCustomer.KDNr
'        '                .KST = lblSelectedAdvisors.Text
'        '                .ESRBankName = lueBankdaten.EditValue
'        '                .Lp = CType(daeValutaDate.EditValue, Date).Date.Month
'        '                .FakDat = CType(daeValutaDate.EditValue, Date).Date
'        '                .Currency = lueCurrency.EditValue
'        '                .Faellig = If(daeDueDate.EditValue Is Nothing, Nothing, CType(daeDueDate.EditValue, Date).Date)
'        '                .Mahncode = m_selectedAddress.MahnCode
'        '                .RName1 = m_selectedAddress.REFirma
'        '                .RName2 = txtFirma2.Text
'        '                .RName3 = txtFirma3.Text
'        '                .RZHD = txtZuHd.Text
'        '                .RAbteilung = txtAbteilung.Text
'        '                .RPostfach = txtPostfach.Text
'        '                .RStrasse = txtStreet.Text
'        '                .RLand = lueCountry.EditValue
'        '                .RPLZ = luePostcode.EditValue
'        '                .ROrt = txtLocation.Text
'        '                .Zahlkond = luePaymentConditions.EditValue
'        '                .RefNr = refNr
'        '                .RefFootNr = refFootNr
'        '                .ZEInfo = medZahlungsinformation.Text
'        '                .ChangedOn = DateTime.Now
'        '                .ChangedFrom = m_InitializationData.UserData.UserFullName
'        '                .KDBranche = lueBranch.EditValue
'        '                .MWSTProz = txtMwStPercen.Text
'        '                .MA0 = IIf(daeMahnKontoauszug.EditValue Is Nothing, Nothing, CType(daeMahnKontoauszug.EditValue, Date))
'        '                .MA1 = IIf(daeMahn1Mahnung.EditValue Is Nothing, Nothing, CType(daeMahn1Mahnung.EditValue, Date))
'        '                .MA2 = IIf(daeMahn2Mahnung.EditValue Is Nothing, Nothing, CType(daeMahn2Mahnung.EditValue, Date))
'        '                .MA3 = IIf(daeMahnIncasso.EditValue Is Nothing, Nothing, CType(daeMahnIncasso.EditValue, Date))
'        '            End With

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the invoice rows
'        '        ''' </summary>
'        '        Private Sub LoadInvoiceRowsRPT()
'        '            If m_invoiceData Is Nothing OrElse m_invoiceNumber Is Nothing Then
'        '                Return
'        '            End If

'        '            SetInvoiceRowUi()
'        '            Dim Sprache As String = m_selectedCustomer.Sprache
'        '            If String.IsNullOrWhiteSpace(Sprache) Then Sprache = "D"
'        '            m_invoiceRowsRPL = m_InvoiceDatabaseAccess.LoadInvoiceRPT(m_invoiceNumber, m_selectedCustomer.KDNr, Sprache)

'        '            If m_invoiceRowsRPL Is Nothing Then
'        '                Return
'        '            End If

'        '            Dim columnNumber As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            columnNumber.Caption = m_Translate.GetSafeTranslationValue("RPNr")
'        '            columnNumber.Name = "RPNr"
'        '            columnNumber.FieldName = "RPNr"
'        '            columnNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            columnNumber.Visible = True
'        '            columnNumber.Width = 50
'        '            gvInvoiceRows.Columns.Add(columnNumber)

'        '            Dim fromColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            fromColumn.Caption = m_Translate.GetSafeTranslationValue("Von")
'        '            fromColumn.Name = "Von"
'        '            fromColumn.FieldName = "VonDate"
'        '            fromColumn.Visible = True
'        '            gvInvoiceRows.Columns.Add(fromColumn)

'        '            Dim toColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            toColumn.Caption = m_Translate.GetSafeTranslationValue("Bis")
'        '            toColumn.Name = "Bis"
'        '            toColumn.FieldName = "BisDate"
'        '            toColumn.Visible = True
'        '            gvInvoiceRows.Columns.Add(toColumn)

'        '            Dim laOpColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            laOpColumn.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
'        '            laOpColumn.Name = "Bezeichnung"
'        '            laOpColumn.FieldName = "LAOpText"
'        '            laOpColumn.Visible = True
'        '            gvInvoiceRows.Columns.Add(laOpColumn)

'        '            Dim countColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            countColumn.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
'        '            countColumn.Name = "Anzahl"
'        '            countColumn.FieldName = "KAnzahl"
'        '            countColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            countColumn.Visible = True
'        '            gvInvoiceRows.Columns.Add(countColumn)

'        '            Dim basisColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            basisColumn.Caption = m_Translate.GetSafeTranslationValue("Basis")
'        '            basisColumn.Name = "Basis"
'        '            basisColumn.FieldName = "KBasis"
'        '            basisColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            basisColumn.AppearanceHeader.Options.UseTextOptions = True
'        '            basisColumn.Visible = True
'        '            basisColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
'        '            basisColumn.DisplayFormat.FormatString = "N"
'        '            gvInvoiceRows.Columns.Add(basisColumn)

'        '            Dim ansatzColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            ansatzColumn.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
'        '            ansatzColumn.Name = "Ansatz"
'        '            ansatzColumn.FieldName = "KAnsatz"
'        '            ansatzColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            ansatzColumn.AppearanceHeader.Options.UseTextOptions = True
'        '            ansatzColumn.Visible = True
'        '            ansatzColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
'        '            ansatzColumn.DisplayFormat.FormatString = "N"
'        '            gvInvoiceRows.Columns.Add(ansatzColumn)

'        '            Dim amountColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            amountColumn.Caption = m_Translate.GetSafeTranslationValue("Betrag")
'        '            amountColumn.Name = "Betrag"
'        '            amountColumn.FieldName = "KBetrag"
'        '            amountColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            amountColumn.AppearanceHeader.Options.UseTextOptions = True
'        '            amountColumn.Visible = True
'        '            amountColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
'        '            amountColumn.DisplayFormat.FormatString = "N"
'        '            gvInvoiceRows.Columns.Add(amountColumn)

'        '            Dim columnKSTName As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            columnKSTName.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
'        '            columnKSTName.Name = "kstname"
'        '            columnKSTName.FieldName = "kstname"
'        '            columnKSTName.Visible = True
'        '            gvInvoiceRows.Columns.Add(columnKSTName)

'        '            gridInvoiceRows.DataSource = m_invoiceRowsRPL

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads the invoice rows
'        '        ''' </summary>
'        '        Private Sub LoadInvoiceRowsInd()
'        '            If m_invoiceData Is Nothing OrElse m_invoiceNumber Is Nothing Then
'        '                Return
'        '            End If

'        '            SetInvoiceRowUi()

'        '            m_invoiceRowsIndividual = m_InvoiceDatabaseAccess.LoadInvoiceIndividual(m_invoiceNumber)

'        '            If m_invoiceRowsIndividual Is Nothing Then
'        '                Return
'        '            End If

'        '            Dim columnNumber As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            columnNumber.Caption = m_Translate.GetSafeTranslationValue("RecNr")
'        '            columnNumber.Name = "RecNr"
'        '            columnNumber.FieldName = "RecNr"
'        '            columnNumber.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            columnNumber.Visible = False
'        '            columnNumber.Width = 50
'        '            gvInvoiceRows.Columns.Add(columnNumber)

'        '            Dim reHeadColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            reHeadColumn.Caption = m_Translate.GetSafeTranslationValue("Kopfzeile")
'        '            reHeadColumn.Name = m_Translate.GetSafeTranslationValue("Kopfzeile")
'        '            reHeadColumn.FieldName = "ReHeadText"
'        '            reHeadColumn.Visible = True
'        '            gvInvoiceRows.Columns.Add(reHeadColumn)

'        '            Dim reTextColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            reTextColumn.Caption = m_Translate.GetSafeTranslationValue("Text")
'        '            reTextColumn.Name = m_Translate.GetSafeTranslationValue("Text")
'        '            reTextColumn.FieldName = "ReText"
'        '            reTextColumn.Visible = True
'        '            gvInvoiceRows.Columns.Add(reTextColumn)

'        '            Dim betragExColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            betragExColumn.Caption = m_Translate.GetSafeTranslationValue("Betrag exklusiv")
'        '            betragExColumn.Name = "Exklusiv"
'        '            betragExColumn.FieldName = "BetragEx"
'        '            betragExColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            betragExColumn.AppearanceHeader.Options.UseTextOptions = True
'        '            betragExColumn.Visible = True
'        '            betragExColumn.Width = 50
'        '            betragExColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
'        '            betragExColumn.DisplayFormat.FormatString = "N"
'        '            gvInvoiceRows.Columns.Add(betragExColumn)

'        '            Dim mwstColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            mwstColumn.Caption = m_Translate.GetSafeTranslationValue("MwSt")
'        '            mwstColumn.Name = m_Translate.GetSafeTranslationValue("MwSt")
'        '            mwstColumn.FieldName = "MWST1"
'        '            mwstColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            mwstColumn.AppearanceHeader.Options.UseTextOptions = True
'        '            mwstColumn.Visible = True
'        '            mwstColumn.Width = 50
'        '            mwstColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
'        '            mwstColumn.DisplayFormat.FormatString = "N"
'        '            gvInvoiceRows.Columns.Add(mwstColumn)

'        '            Dim betragTotalColumn As New DevExpress.XtraGrid.Columns.GridColumn()
'        '            betragTotalColumn.Caption = m_Translate.GetSafeTranslationValue("Total")
'        '            betragTotalColumn.Name = m_Translate.GetSafeTranslationValue("Total")
'        '            betragTotalColumn.FieldName = "BetragTotal"
'        '            betragTotalColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'        '            betragTotalColumn.AppearanceHeader.Options.UseTextOptions = True
'        '            betragTotalColumn.Visible = True
'        '            betragTotalColumn.Width = 50
'        '            betragTotalColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
'        '            betragTotalColumn.DisplayFormat.FormatString = "N"
'        '            gvInvoiceRows.Columns.Add(betragTotalColumn)

'        '            gridInvoiceRows.DataSource = m_invoiceRowsIndividual

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles double click on invoice row.
'        '        ''' </summary>
'        '        Private Sub OngvInvoiceRows_DoubleClick(sender As System.Object, e As System.EventArgs)
'        '            Dim selectedRows = gvInvoiceRows.GetSelectedRows()

'        '            If (selectedRows.Count > 0) Then

'        '                Dim rpNumber As Integer = 0

'        '                If m_invoiceData.Art.ToUpper() = "A" Then
'        '                    rpNumber = CType(SelectedInvoiceRow, DataObjects.InvoiceRPL).RPNr

'        '                    Dim hub = MessageService.Instance.Hub
'        '                    Dim openReportsMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, rpNumber)
'        '                    hub.Publish(openReportsMng)
'        '                End If

'        '                ' Clear possible non saved new row
'        '                m_newInvoiceRow = Nothing
'        '                SetInvoiceRowUi()
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Gets new Invoice values
'        '        ''' </summary>
'        '        Private Sub ReloadInvoiceValues()
'        '            Dim success As Boolean = m_InvoiceDatabaseAccess.ReloadInvoiceValues(m_invoiceData)

'        '            If success Then
'        '                ' Update UI
'        '                txtSkonto.Text = m_invoiceData.BetragOhne
'        '                txtDebtLoss.Text = m_invoiceData.BetragEx
'        '                txtReImbursement.Text = m_invoiceData.MWST1
'        '                txtBetragZE.Text = m_invoiceData.BetragInk
'        '                txtBetragBezahlt.Text = m_invoiceData.Bezahlt
'        '                txtBetragOffen.Text = m_invoiceData.BetragInk - m_invoiceData.Bezahlt
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' show payments for assigned invoice
'        '        ''' </summary>
'        '        ''' <param name="sender"></param>
'        '        ''' <param name="e"></param>
'        '        ''' <remarks></remarks>
'        '        Private Sub OnbtnShow_Payments(sender As Object, e As EventArgs)

'        '            If (Not IsInvoiceDataLoaded) Then
'        '                m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

'        '                Exit Sub
'        '            End If

'        '            If m_PropertyForm Is Nothing OrElse m_PropertyForm.IsDisposed Then
'        '                m_PropertyForm = New frmInvoiceProperties(m_InitializationData, m_Translate, m_invoiceNumber)
'        '            End If
'        '            m_PropertyForm.LoadFoundedInvoicePayrmentList(m_invoiceNumber)
'        '            m_PropertyForm.Show()
'        '            m_PropertyForm.BringToFront()

'        '        End Sub

'        '#End Region

'        '#Region "Event Handles"

'        '        ''' <summary>
'        '        ''' Handles form load event.
'        '        ''' </summary>
'        '        Private Sub OnFrmInvoices_Load(sender As Object, e As System.EventArgs) Handles Me.Load

'        '            Me.KeyPreview = True
'        '            Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
'        '            If strStyleName <> String.Empty Then
'        '                UserLookAndFeel.Default.SetSkinStyle(strStyleName)
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads form settings if form gets visible.
'        '        ''' </summary>
'        '        Private Sub OnFrmInvoices_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

'        '            If Visible Then
'        '                LoadFormSettings()
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles form closing event.
'        '        ''' </summary>
'        '        Private Sub OnFrmInvoices_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

'        '            CleanupAndHideForm()
'        '            e.Cancel = True

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Keypreview for Modul-version
'        '        ''' </summary>
'        '        Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
'        '            If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
'        '                Dim strRAssembly As String = ""
'        '                Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
'        '                For Each a In AppDomain.CurrentDomain.GetAssemblies()
'        '                    strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
'        '                Next
'        '                strMsg = String.Format(strMsg, vbNewLine,
'        '                                                                                                         GetExecutingAssembly().FullName,
'        '                                                                                                         GetExecutingAssembly().Location,
'        '                                                                                                         strRAssembly)
'        '                DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Clickevent for Navbar.
'        '        ''' </summary>
'        '        Private Sub OnnbMain_LinkClicked(ByVal sender As Object,
'        '                                                                                                                         ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs)
'        '            Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'        '            Dim bForDesign As Boolean = False
'        '            Try
'        '                Dim strLinkName As String = e.Link.ItemName
'        '                Dim strLinkCaption As String = e.Link.Caption

'        '                For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
'        '                    e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
'        '                Next
'        '                e.Link.Item.Appearance.ForeColor = Color.Orange

'        '                Select Case strLinkName.ToLower
'        '                    Case "New_Invoice".ToLower
'        '                        NewInvoice()
'        '                    Case "Save_Invoice_Data".ToLower
'        '                        SaveInvoiceData()
'        '                    Case "Delete_Invoice_Data".ToLower
'        '                        DeleteInvoice()
'        '                    Case "Close_Invoice_Form".ToLower
'        '                        CleanupAndHideForm()

'        '                    Case "Print_Invoice_Data".ToLower
'        '                        PrintSelectedInvoice()

'        '                    Case "wos_Invoice_Data".ToLower
'        '                        SendSelectedInvoiceToWOS()

'        '                    Case "CreateTODO".ToLower
'        '                        ShowTodo()
'        '                    Case "guausbuchen".ToLower
'        '                        BookoutBonus()
'        '                    Case "changeinvoicecustomer".ToLower()
'        '                        ChangeInvoiceCustomer()
'        '                    Case "changemahnstopdate"
'        '                        ChangeInvoiceMahnstopDate()
'        '                    Case Else
'        '                        ' Do nothing
'        '                End Select

'        '            Catch ex As Exception
'        '                m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
'        '                m_UtilityUI.ShowErrorDialog(ex.ToString)

'        '            Finally

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the mandant changed event
'        '        ''' </summary>
'        '        Private Sub OnlueMandantEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
'        '            LoadBankdatenDropDown()

'        '            If lueMandant.EditValue Is Nothing Then
'        '                If m_customersData IsNot Nothing Then
'        '                    m_customersData.Clear()
'        '                End If
'        '            Else
'        '                m_customersData = m_InvoiceDatabaseAccess.LoadCustomerData()
'        '            End If

'        '            'Kundendaten leeren
'        '            cboCustomer.EditValue = Nothing
'        '            ResetAddressDropDown()
'        '            m_selectedAddress = Nothing
'        '            DisplayAdress()

'        '            lueBranch.EditValue = Nothing
'        '            luePaymentConditions.EditValue = Nothing
'        '            lueCurrency.EditValue = Nothing
'        '            txtMwStPercen.Text = String.Empty

'        '            ' reset Fälligkeit
'        '            daeDueDate.EditValue = Nothing

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the advisor changed event
'        '        ''' </summary>
'        '        Private Sub OnlueAdvisorInEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueAdvisor1.EditValueChanged, lueAdvisor2.EditValueChanged
'        '            Dim advisor1 = lueAdvisor1.EditValue
'        '            Dim advisor2 = lueAdvisor2.EditValue
'        '            If String.IsNullOrWhiteSpace(advisor1) Then
'        '                lblSelectedAdvisors.Text = advisor2
'        '            ElseIf String.IsNullOrWhiteSpace(advisor2) Then
'        '                lblSelectedAdvisors.Text = advisor1
'        '            ElseIf advisor1 = advisor2 Then
'        '                lblSelectedAdvisors.Text = advisor1
'        '            Else
'        '                lblSelectedAdvisors.Text = advisor1 + "/" + advisor2
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles drop down button clicks.
'        '        ''' </summary>
'        '        Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

'        '            Const ID_OF_DELETE_BUTTON As Int32 = 1

'        '            ' If delete button has been clicked reset the drop down.
'        '            If e.Button.Index = ID_OF_DELETE_BUTTON Then

'        '                If TypeOf sender Is BaseEdit Then
'        '                    If CType(sender, BaseEdit).Properties.ReadOnly Then
'        '                        ' nothing
'        '                    Else
'        '                        CType(sender, BaseEdit).EditValue = Nothing
'        '                    End If
'        '                End If
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles click on choose customer button.
'        '        ''' </summary>
'        '        Private Sub OncboKundeButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

'        '            If m_customersData Is Nothing Then
'        '                Return
'        '            End If

'        '            If Not m_selectedCustomer Is Nothing Then
'        '                ' Send a request to open a customerMng form.
'        '                Dim hub = MessageService.Instance.Hub
'        '                Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_selectedCustomer.KDNr)
'        '                hub.Publish(openCustomerMng)
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles click on address lookup edit button.
'        '        ''' </summary>
'        '        Private Sub OnLueAdresseButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)
'        '            If e.Button.Index = 2 Then

'        '                If Not m_selectedAddress Is Nothing Then
'        '                    Dim invoiceAddressesForm = New frmInvoiceAddress(m_InitializationData)
'        '                    invoiceAddressesForm.Show()
'        '                    invoiceAddressesForm.LoadCustomerInvoiceAddresses(m_selectedAddress.KDNr, m_selectedAddress.RecNr)
'        '                    invoiceAddressesForm.BringToFront()
'        '                End If
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Sets the selected Customer
'        '        ''' </summary>
'        '        Private Sub SetCustomer(ByVal invoiceData As DataObjects.Invoice)
'        '            m_selectedCustomer = (From customer In m_customersData Where customer.KDNr = invoiceData.KdNr).FirstOrDefault
'        '            Try

'        '                If m_selectedCustomer IsNot Nothing Then
'        '                    SetCustomerText(m_selectedCustomer.KDNr, m_invoiceData.RName1, m_invoiceData.RPLZ, m_invoiceData.ROrt)
'        '                    lueCurrency.EditValue = m_selectedCustomer.Currency
'        '                    lueBranch.EditValue = Nothing

'        '                    Try
'        '                        LoadBranchDropDownData()
'        '                    Catch ex As Exception
'        '                        m_Logger.LogError(ex.ToString)

'        '                    End Try

'        '                End If

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString)

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Sets the mahnStop date value.
'        '        ''' </summary>
'        '        ''' <param name="mahnStopValue">The mahnstop date value.</param>
'        '        Private Sub SetMahnStopValue(ByVal mahnStopValue As DateTime?)

'        '            Dim text As String = String.Empty
'        '            Dim highlightBackColor As Color = Color.Empty
'        '            Dim highlightForeColor As Color = Color.Empty

'        '            Try

'        '                If mahnStopValue.HasValue Then
'        '                    text = String.Format("{0:dd.MM.yyyy}", mahnStopValue.Value)
'        '                    highlightBackColor = Color.Red
'        '                    highlightForeColor = Color.White
'        '                Else
'        '                    text = m_Translate.GetSafeTranslationValue("nicht gesetzt")
'        '                End If

'        '                lblMahnstopValue.Text = text
'        '                lblMahnstopValue.BackColor = highlightBackColor
'        '                lblMahnstopValue.ForeColor = highlightForeColor

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString)

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Sets the customer text.
'        '        ''' </summary>
'        '        Private Sub SetCustomerText(ByVal kdNr As Integer, ByVal rName1 As String, ByVal rplz As String, ByVal rort As String)
'        '            cboCustomer.Text = String.Format("{0} - {1} - {2} {3}", m_selectedCustomer.KDNr, m_invoiceData.RName1, m_invoiceData.RPLZ, m_invoiceData.ROrt)
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the address changed event
'        '        ''' </summary>
'        '        Private Sub OnlueAdresseEditValueChanged(sender As System.Object, e As System.EventArgs)
'        '            If lueAdresse.EditValue Is Nothing Then
'        '                m_selectedAddress = Nothing
'        '                DisplayAdress()
'        '                Return
'        '            End If

'        '            m_selectedAddress = (From a In m_addressData Where a.Id = lueAdresse.EditValue).FirstOrDefault

'        '            m_SuppressUIEvents = True
'        '            DisplayAdress()
'        '            m_SuppressUIEvents = False

'        '            CalculateDueDate()

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the date changed event
'        '        ''' </summary>
'        '        Private Sub OndaeDatumEditValueChanged(sender As System.Object, e As System.EventArgs) Handles daeValutaDate.EditValueChanged
'        '            CalculateDueDate()
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles new value event on postcode(plz) lookup edit.
'        '        ''' </summary>
'        '        Private Sub OnluePostcodeProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs)
'        '            If Not luePostcode.Properties.DataSource Is Nothing Then

'        '                Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of DatabaseAccess.Common.DataObjects.PostCodeData))

'        '                Dim newPostcode As New DatabaseAccess.Common.DataObjects.PostCodeData With {.Postcode = e.DisplayValue.ToString()}
'        '                listOfPostcode.Add(newPostcode)

'        '                e.Handled = True
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles change of postcode.
'        '        ''' </summary>
'        '        Private Sub OnluePostcodeEditValueChanged(sender As System.Object, e As System.EventArgs)
'        '            Dim postCodeData As DatabaseAccess.Common.DataObjects.PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), DatabaseAccess.Common.DataObjects.PostCodeData)

'        '            If m_SuppressUIEvents Then
'        '                Return
'        '            End If
'        '            If Not postCodeData Is Nothing Then
'        '                txtLocation.Text = postCodeData.Location
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the debitorenart ValueChanged Event
'        '        ''' sets defautl values for FibuHaben0 and FibuHaben1
'        '        ''' </summary>
'        '        Private Sub OnlueDebitorenartEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueInvoiceNumber.EditValueChanged
'        '            If lueInvoiceNumber.Properties.ReadOnly Then
'        '                Return
'        '            End If
'        '            Select Case lueInvoiceNumber.EditValue
'        '                Case "A"
'        '                    txtFibuHaben1.Text = m_postingAccounts(2)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(4)
'        '                Case "I"
'        '                    txtFibuHaben1.Text = m_postingAccounts(3)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(5)
'        '                Case "F"
'        '                    txtFibuHaben1.Text = m_postingAccounts(17)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(18)
'        '                Case "GA"
'        '                    txtFibuHaben1.Text = m_postingAccounts(33)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(34)
'        '                Case "GI"
'        '                    txtFibuHaben1.Text = m_postingAccounts(35)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(36)
'        '                Case "GF"
'        '                    txtFibuHaben1.Text = m_postingAccounts(37)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(38)
'        '                Case "RA"
'        '                    txtFibuHaben1.Text = m_postingAccounts(23)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(24)
'        '                Case "RI"
'        '                    txtFibuHaben1.Text = m_postingAccounts(27)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(28)
'        '                Case "RF"
'        '                    txtFibuHaben1.Text = m_postingAccounts(31)
'        '                    txtRestBetragInProzent.Text = m_postingAccounts(32)
'        '            End Select

'        '            If String.IsNullOrWhiteSpace(txtFibuHaben1.Text) OrElse String.IsNullOrWhiteSpace(txtRestBetragInProzent.Text) Then
'        '                m_UtilityUI.ShowErrorDialog("Achtung: Sie haben für Debitoren '" + lueInvoiceNumber.EditValue + "' keine Hauptkonto definiert. " +
'        '                                        "Bitte definieren Sie eine Hauptkonto in der Mandantenverwaltung." + vbLf +
'        '                                        "Ihre Daten können nicht gespeichert werden.")

'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' handles checkbox for accepting tab
'        '        ''' </summary>
'        '        Private Sub chkAllowedTab_EditValueChanged(sender As Object, e As EventArgs)

'        '            medRowHead.Properties.AcceptsTab = chkAllowedTab.Checked
'        '            medRowDetail.Properties.AcceptsTab = chkAllowedTab.Checked

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the invoice-row FocusedRowChanged Event
'        '        ''' </summary>
'        '        Private Sub OngvInvoiceRowsFocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
'        '            m_newInvoiceRow = Nothing
'        '            SetInvoiceRowUi()
'        '        End Sub

'        '        ''' <summary>
'        '        '''  Handles RowStyle event of lueBranch grid view.
'        '        ''' </summary>
'        '        Private Sub OnGvLueBranch_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs)

'        '            If e.RowHandle >= 0 Then

'        '                Dim rowData = CType(gvLueBranch.GetRow(e.RowHandle), BranchViewData)

'        '                If rowData.IsAssignedToCustomer Then
'        '                    e.Appearance.BackColor = Color.Yellow
'        '                    e.Appearance.BackColor2 = Color.Yellow
'        '                End If

'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the new row button click
'        '        ''' </summary>
'        '        Private Sub OnBtnRowNewClick(sender As System.Object, e As System.EventArgs) Handles btnRowNew.Click

'        '            If m_invoiceData Is Nothing Then
'        '                Return
'        '            End If

'        '            If m_invoiceData.Art.ToUpper <> "A" Then
'        '                Dim invoiceRowIndividual = New DataObjects.InvoiceIndividual With {
'        '                                .Id = Nothing,
'        '                                .RENr = m_invoiceData.ReNr,
'        '                                .KDNr = m_invoiceData.KdNr,
'        '                                .BetragTotal = 0,
'        '                                .MWST1 = 0,
'        '                                .BetragEx = 0,
'        '                                .Currency = Nothing,
'        '                                .Monat = Now.Month,
'        '                                .Jahr = Now.Year,
'        '                                .ReText = "",
'        '                                .RecNr = -1,
'        '                                .ReHeadText = ""
'        '                }

'        '                m_newInvoiceRow = invoiceRowIndividual
'        '                SetInvoiceRowUi()
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the row button save click
'        '        ''' </summary>
'        '        Private Sub OnBtnRowSaveClick(sender As System.Object, e As System.EventArgs) Handles btnRowSave.Click

'        '            If m_invoiceData Is Nothing Or Not HasActiveInvoiceRow Then
'        '                Return
'        '            End If

'        '            Dim success As Boolean = False

'        '            If m_invoiceData.Art.ToUpper = "A" Then
'        '                Dim activeRow As DataObjects.InvoiceRPL = CType(ActiveInvoiceRow, DataObjects.InvoiceRPL)
'        '                activeRow.RPText = medRowDetail.Text.Trim
'        '                success = m_InvoiceDatabaseAccess.UpdateInvoiceRPT(activeRow)
'        '            Else
'        '                Dim activeRow As DataObjects.InvoiceIndividual = CType(ActiveInvoiceRow, DataObjects.InvoiceIndividual)

'        '                activeRow.ReHeadText = medRowHead.Text.Trim
'        '                activeRow.ReText = medRowDetail.Text.Trim
'        '                activeRow.BetragEx = CType(txtRowValueExcl.Text, Decimal)
'        '                activeRow.MWST1 = m_Utility.SwissCommercialRound(activeRow.BetragEx * txtMwStPercen.EditValue / 100)
'        '                activeRow.BetragTotal = activeRow.BetragEx + activeRow.MWST1


'        '                If activeRow.RecNr = -1 Then
'        '                    success = m_InvoiceDatabaseAccess.AddNewInvoiceIndividual(activeRow, ReferenceNumbersTo10Setting)

'        '                    If success Then
'        '                        m_invoiceRowsIndividual.Add(activeRow)
'        '                        gridInvoiceRows.RefreshDataSource()
'        '                        FocusInvoiceIndividualRow(activeRow.Id)
'        '                        m_newInvoiceRow = Nothing
'        '                    End If

'        '                Else
'        '                    success = m_InvoiceDatabaseAccess.UpdateInvoiceIndividual(activeRow, ReferenceNumbersTo10Setting)
'        '                End If
'        '                ' reload invoice values
'        '                ReloadInvoiceValues()
'        '            End If

'        '            If (success) Then
'        '                gridInvoiceRows.RefreshDataSource()

'        '                'm_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Zeile wurde gespeichert."))
'        '            Else
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Zeile konnte nicht gespeichert werden."))
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles the row button delete click
'        '        ''' </summary>
'        '        Private Sub OngridInvoiceRows_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs)
'        '            'Private Sub OnBtnRowDeleteClick(sender As System.Object, e As System.EventArgs) Handles btnRowDelete.Click

'        '            If m_invoiceData Is Nothing Or gvInvoiceRows.SelectedRowsCount = 0 Then
'        '                Return
'        '            End If

'        '            If m_invoiceData.Art.ToUpper <> "A" Then

'        '                If (e.KeyCode = Keys.Delete) Then
'        '                    Dim selectedRow As DataObjects.InvoiceIndividual = CType(SelectedInvoiceRow, DataObjects.InvoiceIndividual)
'        '                    Dim success As Boolean = True
'        '                    If selectedRow.Id IsNot Nothing Then

'        '                        Dim result = m_InvoiceDatabaseAccess.DeleteInvoiceIndividual(selectedRow,
'        '                                                                                                                                                                                                                                ConstantValues.ModulName,
'        '                                                                                                                                                                                                                                String.Format("{0}, {1}", m_InitializationData.UserData.UserLName, m_InitializationData.UserData.UserFName),
'        '                                                                                                                                                                                                                                m_InitializationData.UserData.UserNr, ReferenceNumbersTo10Setting)

'        '                        Select Case result
'        '                            Case DeleteREIndResult.ResultCanNotDeleteBecauseMonthIsClosed
'        '                                m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnungszeile kann nicht gelöscht werden, da der Monat bereits abgeschlossen ist."),
'        '                                                                                                                                                                                         m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
'        '                            Case DeleteREIndResult.ResultCanNotDeleteBecauseOfExistingZE
'        '                                m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnungszeile kann nicht gelöscht werden, da bereits eine Zahlungseingang existiert."),
'        '                                                                                                                                                 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
'        '                            Case DeleteREIndResult.ResultCanNotDeleteBecauseOfPartlyPayed
'        '                                m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Rechnungszeile kann nicht gelöscht werden, da bereits eine Teilzahlung existiert."),
'        '                                                                                                                                                 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
'        '                            Case DeleteREIndResult.ResultDeleteError
'        '                                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungszeile konnte nicht gelöscht werden."))
'        '                            Case DeleteREIndResult.ResultDeleteOk

'        '                                ' remove row
'        '                                m_invoiceRowsIndividual.Remove(selectedRow)
'        '                                gridInvoiceRows.RefreshDataSource()
'        '                                SetInvoiceRowUi()

'        '                                ' reload invoice values
'        '                                ReloadInvoiceValues()

'        '                        End Select

'        '                    End If

'        '                End If

'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Handles an new row value text
'        '        ''' </summary>
'        '        Private Sub OnTxtRowValueExclEditValueChanged(sender As System.Object, e As System.EventArgs)
'        '            If Not txtRowValueExcl.Enabled Then
'        '                Return
'        '            End If
'        '            If m_invoiceData.Art.ToUpper = "A" Then
'        '                ' not implemented
'        '            Else


'        '                If (m_invoiceData.Art.ToUpper = "G" Or m_invoiceData.Art.ToUpper = "R") AndAlso CType(txtRowValueExcl.Text, Decimal) > 0 Then
'        '                    DevComponents.DotNetBar.ToastNotification.Close(Me)

'        '                    Dim msg As String = "Der Betrag wird ins Negativ umgewandelt..."
'        '                    DevComponents.DotNetBar.ToastNotification.Show(Me, m_Translate.GetSafeTranslationValue(msg),
'        '                                                                                                                 Nothing, DevComponents.DotNetBar.ToastNotification.DefaultTimeoutInterval,
'        '                                                                                                                 DevComponents.DotNetBar.ToastNotification.DefaultToastGlowColor,
'        '                                                                                                                 DevComponents.DotNetBar.eToastPosition.BottomRight)

'        '                    txtRowValueExcl.Text = CType(txtRowValueExcl.Text, Decimal) * -1
'        '                End If


'        '                Dim betragEx = CType(txtRowValueExcl.Text, Decimal)
'        '                Dim mwst1 = m_Utility.SwissCommercialRound(betragEx * txtMwStPercen.EditValue / 100)
'        '                Dim betragTotal = betragEx + mwst1

'        '                ' display 
'        '                txtRowValueMwSt.Text = mwst1
'        '                txtRowValueTotal.Text = betragTotal
'        '                '		gridInvoiceRows.RefreshDataSource()
'        '            End If

'        '        End Sub

'        '        Private Sub OntxtRowValueExclKeyUp(sender As Object, e As KeyEventArgs)
'        '            If e.KeyCode = Keys.Enter Then
'        '                'OnTxtRowValueExclEditValueChanged(sender, e)
'        '                btnRowNew.Focus()
'        '                OnBtnRowSaveClick(btnRowSave, e)
'        '            End If

'        '        End Sub


'        '#End Region

'        '#Region "Debitoren logic"

'        '        ''' <summary>
'        '        ''' Calculates the due date
'        '        ''' </summary>
'        '        Private Sub CalculateDueDate()
'        '            If m_selectedAddress Is Nothing OrElse String.IsNullOrWhiteSpace(m_selectedAddress.MahnCode) OrElse daeValutaDate.EditValue Is Nothing Then
'        '                daeDueDate.EditValue = Nothing
'        '                Return
'        '            End If

'        '            If m_mahnCodeData Is Nothing Then
'        '                m_mahnCodeData = m_CustomerDatabaseAccess.LoadPaymentReminderCodeData()
'        '            End If

'        '            Dim mahnCode = (From m In m_mahnCodeData Where m.GetField = m_selectedAddress.MahnCode).FirstOrDefault
'        '            If mahnCode Is Nothing OrElse mahnCode.GetField = "N" Then
'        '                daeDueDate.EditValue = Nothing
'        '                Return
'        '            End If

'        '            Dim dueDate As Date = CType(daeValutaDate.EditValue, Date)
'        '            dueDate = dueDate.AddDays(mahnCode.Reminder1)
'        '            While (Weekday(dueDate) = vbSaturday OrElse Weekday(dueDate) = vbSunday)
'        '                ' next day after weekend
'        '                dueDate = dueDate.AddDays(1)
'        '            End While

'        '            daeDueDate.EditValue = dueDate
'        '        End Sub

'        '#End Region

'        '#Region "Helper Methods"

'        '        ''' <summary>
'        '        '''  Trannslate controls.
'        '        ''' </summary>
'        '        Private Sub TranslateControls()
'        '            'Buttons
'        '            btnRowSave.Text = m_Translate.GetSafeTranslationValue(btnRowSave.Text)
'        '            btnRowNew.Text = m_Translate.GetSafeTranslationValue(btnRowNew.Text)

'        '            'Captions
'        '            gpBetraege.Text = m_Translate.GetSafeTranslationValue(gpBetraege.Text)
'        '            gpEigenschaften.Text = m_Translate.GetSafeTranslationValue(gpEigenschaften.Text)
'        '            gpKundendaten.Text = m_Translate.GetSafeTranslationValue(gpKundendaten.Text)
'        '            gpMahndaten.Text = m_Translate.GetSafeTranslationValue(gpMahndaten.Text)
'        '            gpRechnungsdaten.Text = m_Translate.GetSafeTranslationValue(gpRechnungsdaten.Text)

'        '            'Labels
'        '            lblMandant.Text = m_Translate.GetSafeTranslationValue(lblMandant.Text)
'        '            lblAbteilung.Text = m_Translate.GetSafeTranslationValue(lblAbteilung.Text)
'        '            lblAddresse.Text = m_Translate.GetSafeTranslationValue(lblAddresse.Text)
'        '            lblBankdaten.Text = m_Translate.GetSafeTranslationValue(lblBankdaten.Text)
'        '            lblAdvisor.Text = m_Translate.GetSafeTranslationValue(lblAdvisor.Text)
'        '            lblSelectedAdvisors.Text = m_Translate.GetSafeTranslationValue(lblSelectedAdvisors.Text)
'        '            lblBezahlt.Text = m_Translate.GetSafeTranslationValue(lblBezahlt.Text)
'        '            lblBranche.Text = m_Translate.GetSafeTranslationValue(lblBranche.Text)
'        '            lblDatum.Text = m_Translate.GetSafeTranslationValue(lblDatum.Text)
'        '            lblDebitorenart.Text = m_Translate.GetSafeTranslationValue(lblDebitorenart.Text)
'        '            lblRowDetail.Text = m_Translate.GetSafeTranslationValue(lblRowDetail.Text)
'        '            lblDebtLoss.Text = m_Translate.GetSafeTranslationValue(lblDebtLoss.Text)
'        '            lblDueDate.Text = m_Translate.GetSafeTranslationValue(lblDueDate.Text)
'        '            lblFibuHaben.Text = m_Translate.GetSafeTranslationValue(lblFibuHaben.Text)
'        '            lblRestBetrag.Text = m_Translate.GetSafeTranslationValue(lblRestBetrag.Text)
'        '            lblFirma.Text = m_Translate.GetSafeTranslationValue(lblFirma.Text)
'        '            lblIncasso.Text = m_Translate.GetSafeTranslationValue(lblIncasso.Text)
'        '            lblMahnstop.Text = m_Translate.GetSafeTranslationValue(lblMahnstop.Text)
'        '            lblKontoZE.Text = m_Translate.GetSafeTranslationValue(lblKontoZE.Text)
'        '            lblRowHead.Text = m_Translate.GetSafeTranslationValue(lblRowHead.Text)
'        '            lblKst1.Text = m_Translate.GetSafeTranslationValue(lblKst1.Text)
'        '            lblCountry.Text = m_Translate.GetSafeTranslationValue(lblCountry.Text)
'        '            lblMahnung1.Text = m_Translate.GetSafeTranslationValue(lblMahnung1.Text)
'        '            lblMahnung2.Text = m_Translate.GetSafeTranslationValue(lblMahnung2.Text)
'        '            lblReImbursement.Text = m_Translate.GetSafeTranslationValue(lblReImbursement.Text)
'        '            lblSkonto.Text = m_Translate.GetSafeTranslationValue(lblSkonto.Text)
'        '            lblMwStPercent.Text = m_Translate.GetSafeTranslationValue(lblMwStPercent.Text)
'        '            lblMwStValue.Text = m_Translate.GetSafeTranslationValue(lblMwStValue.Text)
'        '            lblOffen.Text = m_Translate.GetSafeTranslationValue(lblOffen.Text)
'        '            lblLocation.Text = m_Translate.GetSafeTranslationValue(lblLocation.Text)
'        '            lblPostcode.Text = m_Translate.GetSafeTranslationValue(lblPostcode.Text)
'        '            lblPostfach.Text = m_Translate.GetSafeTranslationValue(lblPostfach.Text)
'        '            lblStreet.Text = m_Translate.GetSafeTranslationValue(lblStreet.Text)
'        '            lblTotal.Text = m_Translate.GetSafeTranslationValue(lblTotal.Text)
'        '            lblWaehrung.Text = m_Translate.GetSafeTranslationValue(lblWaehrung.Text)
'        '            lblZahlungsinformation.Text = m_Translate.GetSafeTranslationValue(lblZahlungsinformation.Text)
'        '            lblZuHd.Text = m_Translate.GetSafeTranslationValue(lblZuHd.Text)
'        '            lblFirma2.Text = m_Translate.GetSafeTranslationValue(lblFirma2.Text)
'        '            lblFirma3.Text = m_Translate.GetSafeTranslationValue(lblFirma3.Text)
'        '            chkAllowedTab.Text = m_Translate.GetSafeTranslationValue(chkAllowedTab.Text)

'        '            bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
'        '            bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsiLblGeaendert.Caption)

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Creates Navigationbar
'        '        ''' TODO für NavBar: Mahnstop -> direkt in DB datum feld
'        '        ''' </summary>
'        '        Private Sub CreateMyNavBar()
'        '            Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'        '            Me.navMain.Items.Clear()
'        '            Try
'        '                navMain.PaintStyleName = "SkinExplorerBarView"

'        '                ' Create a Local group.
'        '                Dim groupDatei As NavBarGroup = New NavBarGroup(("Datei"))
'        '                groupDatei.Name = "gNavDatei"

'        '                Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
'        '                New_P.Name = "New_Invoice"
'        '                New_P.SmallImage = Me.ImageCollection1.Images(0)
'        '                New_P.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 401, m_InitializationData.MDData.MDNr)

'        '                m_SaveButton = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
'        '                m_SaveButton.Name = "Save_Invoice_Data"
'        '                m_SaveButton.SmallImage = Me.ImageCollection1.Images(1)
'        '                m_SaveButton.Enabled = IsDataValid

'        '                Dim Print_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
'        '                Print_P_Data.Name = "Print_Invoice_Data"
'        '                Print_P_Data.SmallImage = Me.ImageCollection1.Images(2)
'        '                Print_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 406, m_InitializationData.MDData.MDNr)

'        '                Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
'        '                Close_P_Data.Name = "Close_Invoice_Form"
'        '                Close_P_Data.SmallImage = Me.ImageCollection1.Images(3)

'        '                Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
'        '                groupDelete.Name = "gNavDelete"
'        '                groupDelete.Appearance.ForeColor = Color.Red

'        '                Dim Delete_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
'        '                Delete_P_Data.Name = "Delete_Invoice_Data"
'        '                Delete_P_Data.SmallImage = Me.ImageCollection1.Images(4)
'        '                Delete_P_Data.Appearance.ForeColor = Color.Red
'        '                Delete_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 403, m_InitializationData.MDData.MDNr)

'        '                Dim groupExtra As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
'        '                groupExtra.Name = "gNavExtra"
'        '                'Dim Property_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Abhängigkeiten anzeigen"))
'        '                'Property_P_Data.Name = "Invoice_properties"
'        '                'Property_P_Data.SmallImage = Me.ImageCollection1.Images(5)
'        '                'Property_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 217, m_InitializationData.MDData.MDNr)

'        '                m_Wos_P_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("Rechnung übermitteln"))
'        '                m_Wos_P_Data.Name = "wos_Invoice_Data"
'        '                m_Wos_P_Data.SmallImage = Me.ImageCollection1.Images(6)

'        '                Dim groupMoreModule As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras Verwaltung"))
'        '                groupExtra.Name = "gNavMoreModule"

'        '                Dim TODO_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("To-do erstellen"))
'        '                TODO_P_Data.Name = "CreateTODO"
'        '                TODO_P_Data.SmallImage = Me.ImageCollection1.Images(9)

'        '                m_Guthaben_NavbarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Gutschrift ausgleichen"))
'        '                m_Guthaben_NavbarItem.Name = "guausbuchen"
'        '                m_Guthaben_NavbarItem.SmallImage = Me.ImageCollection1.Images(5)
'        '                m_Guthaben_NavbarItem.Enabled = False

'        '                m_ChangeCstomerName_NavbarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Firmennamen ändern"))
'        '                m_ChangeCstomerName_NavbarItem.Name = "changeinvoicecustomer"
'        '                m_ChangeCstomerName_NavbarItem.SmallImage = Me.ImageCollection1.Images(10)
'        '                m_ChangeCstomerName_NavbarItem.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 484, m_InitializationData.MDData.MDNr)

'        '                m_ChangeMahnstopDate_NavbarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Mahnstopp"))
'        '                m_ChangeMahnstopDate_NavbarItem.Name = "changemahnstopdate"
'        '                m_ChangeMahnstopDate_NavbarItem.SmallImage = Me.ImageCollection1.Images(10)
'        '                m_ChangeMahnstopDate_NavbarItem.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 404, m_InitializationData.MDData.MDNr)

'        '                Try
'        '                    navMain.BeginUpdate()

'        '                    navMain.Groups.Add(groupDatei)
'        '                    If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 401, m_InitializationData.MDData.MDNr) Then
'        '                        groupDatei.ItemLinks.Add(New_P)
'        '                        groupDatei.ItemLinks.Add(m_SaveButton)
'        '                    End If

'        '                    If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 406, m_InitializationData.MDData.MDNr) Then groupDatei.ItemLinks.Add(Print_P_Data)
'        '                    groupDatei.ItemLinks.Add(Close_P_Data)
'        '                    groupDatei.Expanded = True

'        '                    navMain.Groups.Add(groupDelete)
'        '                    groupDelete.ItemLinks.Add(Delete_P_Data)
'        '                    groupDelete.Expanded = False

'        '                    navMain.Groups.Add(groupExtra)
'        '                    'groupExtra.ItemLinks.Add(Property_P_Data)
'        '                    If m_md.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year) Then groupExtra.ItemLinks.Add(m_Wos_P_Data)

'        '                    navMain.Groups.Add(groupMoreModule)
'        '                    groupMoreModule.ItemLinks.Add(TODO_P_Data)
'        '                    groupMoreModule.ItemLinks.Add(m_Guthaben_NavbarItem)
'        '                    groupMoreModule.ItemLinks.Add(m_ChangeCstomerName_NavbarItem)
'        '                    groupMoreModule.ItemLinks.Add(m_ChangeMahnstopDate_NavbarItem)

'        '                    groupExtra.Expanded = True
'        '                    groupMoreModule.Expanded = True

'        '                    If m_invoiceNumber.HasValue Then
'        '                        bsiCreated.Caption = String.Format("{0:f}, {1}", m_invoiceData.CreatedOn, m_invoiceData.CreatedFrom)
'        '                        bsiChanged.Caption = String.Format("{0:f}, {1}", m_invoiceData.ChangedOn, m_invoiceData.ChangedFrom)
'        '                    End If


'        '                    navMain.EndUpdate()

'        '                Catch ex As Exception
'        '                    m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.ToString))
'        '                    DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.ToString), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

'        '                End Try

'        '            Catch ex As Exception
'        '                m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
'        '                DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.ToString),
'        '                                                                                                                                                                                         "Menüleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Loads form settings.
'        '        ''' </summary>
'        '        Private Sub LoadFormSettings()

'        '            Try
'        '                Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
'        '                Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
'        '                Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
'        '                Dim setting_form_sccMainPos = m_SettingsManager.ReadInteger(SettingKeys.SETTING_SCC_MAINPOS)
'        '                Dim allowedTabinTextBox = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_ALLOWED_TABIN_INVOICE_TEXTBOXES)

'        '                If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
'        '                If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

'        '                If Not String.IsNullOrEmpty(setting_form_location) Then
'        '                    Dim aLoc As String() = setting_form_location.Split(CChar(";"))
'        '                    If Screen.AllScreens.Length = 1 Then
'        '                        If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
'        '                    End If
'        '                    Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
'        '                End If
'        '                If setting_form_sccMainPos > 0 Then Me.sccInvoiceMain.SplitterPosition = Math.Max(setting_form_sccMainPos, 10)

'        '                chkAllowedTab.Checked = allowedTabinTextBox

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString())

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Saves the form settings.
'        '        ''' </summary>
'        '        Private Sub SaveFromSettings()

'        '            ' Save form location, width and height in setttings
'        '            Try
'        '                If Not Me.WindowState = FormWindowState.Minimized Then
'        '                    m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
'        '                    m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
'        '                    m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
'        '                    m_SettingsManager.WriteBoolean(SettingKeys.SETTING_ALLOWED_TABIN_INVOICE_TEXTBOXES, chkAllowedTab.Checked)

'        '                    m_SettingsManager.WriteInteger(SettingKeys.SETTING_SCC_MAINPOS, Me.sccInvoiceMain.SplitterPosition)

'        '                    m_SettingsManager.SaveSettings()
'        '                End If

'        '            Catch ex As Exception
'        '                m_Logger.LogError(ex.ToString())

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Selects the Kst1.
'        '        ''' </summary>
'        '        ''' <param name="kst1">The kst1</param>
'        '        Private Sub SelectKst1(ByVal kst1 As String)

'        '            Dim suppressUIEventState = m_SuppressUIEvents
'        '            suppressUIEventState = True

'        '            lueKst1.EditValue = kst1
'        '            LoadKst2DropDown()

'        '            m_SuppressUIEvents = suppressUIEventState

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Selects an advisor and add missing advisor
'        '        ''' </summary>
'        '        Private Sub SelectAdvisor(lueAdvisor As LookUpEdit, advisorKST As String)
'        '            Dim advisor = (From a In m_advisors Where a.KST = advisorKST).FirstOrDefault
'        '            If advisor Is Nothing Then
'        '                'Add missing advisor
'        '                m_advisors.Add(New DatabaseAccess.Common.DataObjects.AdvisorData With {.KST = advisorKST})
'        '            End If
'        '            lueAdvisor.EditValue = advisorKST
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Sets the readOnly fields for new/edit
'        '        ''' </summary>
'        '        Private Sub SwitchReadOnlyFields(ByVal isReadOnly As Boolean)
'        '            lueMandant.Properties.ReadOnly = isReadOnly
'        '            lueKst1.Properties.ReadOnly = isReadOnly
'        '            lueKst2.Properties.ReadOnly = isReadOnly
'        '            lueAdvisor1.Properties.ReadOnly = isReadOnly
'        '            lueAdvisor2.Properties.ReadOnly = isReadOnly
'        '            lueInvoiceNumber.Properties.ReadOnly = isReadOnly

'        '            ' show groupInvoiceRows only in edit (=isReadOnly)
'        '            groupInvoiceRows.Visible = isReadOnly
'        '        End Sub

'        '        ''' <summary>
'        '        ''' Displays a selected adress
'        '        ''' </summary>
'        '        Private Sub DisplayAdress()
'        '            If m_selectedAddress Is Nothing Then
'        '                txtFirma2.Text = Nothing
'        '                txtFirma3.Text = Nothing
'        '                txtAbteilung.Text = Nothing
'        '                txtZuHd.Text = Nothing
'        '                txtPostfach.Text = Nothing
'        '                txtStreet.Text = Nothing
'        '                txtLocation.Text = Nothing
'        '                lueCountry.EditValue = Nothing
'        '                luePostcode.EditValue = Nothing
'        '            Else
'        '                txtFirma2.Text = m_selectedAddress.REFirma2
'        '                txtFirma3.Text = m_selectedAddress.REFirma3
'        '                txtAbteilung.Text = m_selectedAddress.REAbteilung
'        '                txtZuHd.Text = m_selectedAddress.REZhd
'        '                txtPostfach.Text = m_selectedAddress.REPostfach
'        '                txtStreet.Text = m_selectedAddress.REStrasse
'        '                txtLocation.Text = m_selectedAddress.REOrt
'        '                lueCountry.EditValue = m_selectedAddress.RELand
'        '                luePostcode.EditValue = m_selectedAddress.REPLZ
'        '            End If
'        '        End Sub

'        '        ''' <summary>
'        '        ''' UI-Config, depending on state and debitorenart
'        '        ''' </summary>
'        '        Private Sub SetInvoiceRowUi()

'        '            ' Payed
'        '            If m_invoiceData IsNot Nothing AndAlso m_invoiceData.IsPayed Then
'        '                'disable row fields
'        '                medRowHead.Enabled = False
'        '                medRowDetail.Enabled = False
'        '                txtRowValueTotal.Enabled = False
'        '                txtRowValueMwSt.Enabled = False
'        '                txtRowValueExcl.Enabled = False
'        '                ' Disable all Buttons
'        '                btnRowNew.Enabled = False
'        '                btnRowSave.Enabled = False

'        '                ' Hide all Buttons 
'        '                btnRowNew.Visible = False
'        '                btnRowSave.Visible = False

'        '                ' Nothing selected
'        '                If Not HasActiveInvoiceRow Then
'        '                    Return
'        '                End If

'        '                ' One row selected
'        '                If m_invoiceData.Art.ToUpper = "A" Then
'        '                    ' Set row Field content
'        '                    Dim selectedRow As DataObjects.InvoiceRPL = CType(ActiveInvoiceRow, DataObjects.InvoiceRPL)
'        '                    medRowDetail.Text = selectedRow.RPText
'        '                    txtRowValueExcl.Text = selectedRow.KBetrag
'        '                    txtRowValueMwSt.Text = selectedRow.KBetrag * selectedRow.MWST / 100
'        '                    txtRowValueTotal.Text = selectedRow.KBetrag * (100 + selectedRow.MWST) / 100
'        '                Else
'        '                    ' Set row Field content
'        '                    Dim selectedRow As DataObjects.InvoiceIndividual = CType(ActiveInvoiceRow, DataObjects.InvoiceIndividual)
'        '                    medRowHead.Text = selectedRow.ReHeadText
'        '                    medRowDetail.Text = selectedRow.ReText
'        '                    txtRowValueExcl.Text = selectedRow.BetragEx
'        '                    txtRowValueMwSt.Text = selectedRow.MWST1
'        '                    txtRowValueTotal.Text = selectedRow.BetragTotal
'        '                End If
'        '                Return
'        '            End If

'        '            ' Not Payed &
'        '            ' Nothing selected
'        '            If m_invoiceData Is Nothing Or Not HasActiveInvoiceRow Then
'        '                'clear row field
'        '                medRowHead.Text = String.Empty
'        '                medRowDetail.Text = String.Empty
'        '                txtRowValueTotal.Text = String.Empty
'        '                txtRowValueMwSt.Text = String.Empty
'        '                txtRowValueExcl.Text = String.Empty
'        '                'disable row fields
'        '                medRowHead.Enabled = False
'        '                medRowDetail.Enabled = False
'        '                txtRowValueTotal.Enabled = False
'        '                txtRowValueMwSt.Enabled = False
'        '                txtRowValueExcl.Enabled = False

'        '                If m_invoiceData Is Nothing Then
'        '                    Return
'        '                End If

'        '                If m_invoiceData.Art.ToUpper = "A" Then
'        '                    ' Automatische Debitoren
'        '                    ' Disable Buttons
'        '                    btnRowNew.Enabled = False
'        '                    btnRowSave.Enabled = False
'        '                    ' Show/Hide Buttons
'        '                    btnRowNew.Visible = False
'        '                    btnRowSave.Visible = True
'        '                    ' Hide Head
'        '                    lblRowHead.Visible = False
'        '                    medRowHead.Visible = False

'        '                Else
'        '                    ' Nicht automatische Debitoren
'        '                    ' Enable/Disable Buttons
'        '                    btnRowNew.Enabled = True
'        '                    btnRowSave.Enabled = False
'        '                    ' Show all Buttons
'        '                    btnRowNew.Visible = True
'        '                    btnRowSave.Visible = True

'        '                    ' Show Head
'        '                    lblRowHead.Visible = True
'        '                    medRowHead.Visible = True

'        '                End If
'        '                Return
'        '            End If

'        '            ' Not Payed &
'        '            ' One row selected
'        '            If m_invoiceData.Art.ToUpper = "A" Then
'        '                ' Set row Field content
'        '                Dim selectedRow As DataObjects.InvoiceRPL = CType(ActiveInvoiceRow, DataObjects.InvoiceRPL)
'        '                medRowDetail.Text = selectedRow.RPText
'        '                txtRowValueExcl.Text = selectedRow.KBetrag
'        '                txtRowValueMwSt.Text = selectedRow.KBetrag * selectedRow.MWST / 100
'        '                txtRowValueTotal.Text = selectedRow.KBetrag * (100 + selectedRow.MWST) / 100
'        '                If selectedRow.RPTId Is Nothing Then
'        '                    medRowDetail.Enabled = False
'        '                    btnRowSave.Enabled = False
'        '                Else
'        '                    ' Enable Fields
'        '                    medRowDetail.Enabled = True
'        '                    ' Enable Button
'        '                    btnRowSave.Enabled = True
'        '                End If

'        '                btnRowNew.Visible = False
'        '                btnRowSave.Visible = True
'        '            Else
'        '                ' Set row Field content
'        '                Dim selectedRow As DataObjects.InvoiceIndividual = CType(ActiveInvoiceRow, DataObjects.InvoiceIndividual)
'        '                medRowHead.Text = selectedRow.ReHeadText
'        '                medRowDetail.Text = selectedRow.ReText
'        '                txtRowValueExcl.Text = selectedRow.BetragEx
'        '                txtRowValueMwSt.Text = selectedRow.MWST1
'        '                txtRowValueTotal.Text = selectedRow.BetragTotal
'        '                ' Enable Fields
'        '                medRowHead.Enabled = True
'        '                medRowDetail.Enabled = True
'        '                txtRowValueExcl.Enabled = True
'        '                ' Enable Buttons
'        '                btnRowNew.Enabled = True
'        '                btnRowSave.Enabled = True

'        '                btnRowNew.Visible = True
'        '                btnRowSave.Visible = True
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Sets the valid state of a control.
'        '        ''' </summary>
'        '        ''' <param name="control">The control to validate.</param>
'        '        ''' <param name="errorProvider">The error providor.</param>
'        '        ''' <param name="invalid">Boolean flag if data is invalid.</param>
'        '        ''' <param name="errorText">The error text.</param>
'        '        ''' <returns>Valid flag</returns>
'        '        Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

'        '            If (invalid) Then
'        '                errorProvider.SetError(control, errorText)
'        '            Else
'        '                errorProvider.SetError(control, String.Empty)
'        '            End If

'        '            Return Not invalid

'        '        End Function

'        '        ''' <summary>
'        '        ''' Cleanup and close form.
'        '        ''' </summary>
'        '        Public Sub CleanupAndHideForm()

'        '            SaveFromSettings()

'        '            If Not m_PropertyForm Is Nothing AndAlso Not m_PropertyForm.IsDisposed Then

'        '                Try
'        '                    m_PropertyForm.Close()
'        '                    m_PropertyForm.Dispose()
'        '                Catch
'        '                    ' Do nothing
'        '                End Try
'        '            End If

'        '            Me.Hide()
'        '            Me.Reset() 'Clear all data.

'        '        End Sub

'        '        ''' <summary>
'        '        ''' print selected invoice
'        '        ''' </summary>
'        '        ''' <remarks></remarks>
'        '        Private Sub PrintSelectedInvoice()
'        '            Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
'        '            Dim printOpenAmount As Boolean = m_invoiceData.Art <> "G" AndAlso m_invoiceData.Art <> "R" AndAlso m_invoiceData.Bezahlt.GetValueOrDefault(0) > 0

'        '            If Not m_invoiceNumber.HasValue Then Exit Sub
'        '            If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 406, m_InitializationData.MDData.MDNr) Then Exit Sub
'        '            ShowDesign = ShowDesign AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 407, m_InitializationData.MDData.MDNr)

'        '            If printOpenAmount AndAlso m_invoiceData.BetragInk.GetValueOrDefault(0) > m_invoiceData.Bezahlt.GetValueOrDefault(0) Then
'        '                Dim msg = m_Translate.GetSafeTranslationValue("Möchten Sie die Rechnung mit offener Betrag drucken?")
'        '                printOpenAmount = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Rechnung drucken"), MessageBoxDefaultButton.Button2)
'        '            End If

'        '            Try
'        '                Dim invoiceNumbers As New List(Of Integer)

'        '                invoiceNumbers.Add(CType(m_invoiceNumber.GetValueOrDefault(0), Integer))
'        '                Dim _setting As New InvoicePrintData With {.frmhwnd = Me.Handle,
'        '                                                                                                     .InvoiceNumbers = invoiceNumbers,
'        '                                                                                                     .PrintInvoiceAsCopy = False,
'        '                                                                                                     .ShowAsDesign = ShowDesign,
'        '                                                                                                     .PrintOpenAmount = printOpenAmount}
'        '                Dim printUtil = New ClsPrintInvoice(m_InitializationData)
'        '                printUtil.PrintData = _setting
'        '                Dim result = printUtil.PrintInvoice()

'        '                printUtil.Dispose()


'        '            Catch ex As Exception
'        '                m_Logger.LogError(String.Format("{0}", ex.ToString))
'        '                m_UtilityUI.ShowErrorDialog(String.Format("{1}", ex.ToString))

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' send selected invoice to WOS
'        '        ''' </summary>
'        '        ''' <remarks></remarks>
'        '        Private Sub SendSelectedInvoiceToWOS()

'        '            If Not m_invoiceNumber.HasValue Then Exit Sub
'        '            If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 406, m_InitializationData.MDData.MDNr) Then Exit Sub

'        '            Try
'        '                Dim invoiceNumbers As New List(Of Integer)

'        '                invoiceNumbers.Add(CType(m_invoiceNumber.GetValueOrDefault(0), Integer))
'        '                Dim _setting As New InvoicePrintData With {.frmhwnd = Me.Handle,
'        '                                                                                                     .InvoiceNumbers = invoiceNumbers,
'        '                                                                                                     .PrintInvoiceAsCopy = False,
'        '                                                                                                     .ShowAsDesign = False,
'        '                                                                                                     .WOSSendValueEnum = InvoicePrintData.WOSValue.PrintOtherSendWOS}
'        '                Dim printUtil = New ClsPrintInvoice(m_InitializationData)
'        '                printUtil.PrintData = _setting
'        '                Dim result = printUtil.PrintInvoice()

'        '                printUtil.Dispose()

'        '                If result.Printresult Then
'        '                    Dim msg = m_Translate.GetSafeTranslationValue("Ihre Dokumente wurden erfolgreich übermitteilt.")

'        '                    m_UtilityUI.ShowInfoDialog(msg)

'        '                ElseIf result.Printresult = False Then
'        '                    m_UtilityUI.ShowErrorDialog(result.PrintresultMessage)

'        '                End If


'        '            Catch ex As Exception
'        '                m_Logger.LogError(String.Format("{0}", ex.ToString))
'        '                m_UtilityUI.ShowErrorDialog(String.Format("{1}", ex.ToString))

'        '            End Try

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Shows a todo From.
'        '        ''' </summary>
'        '        Private Sub ShowTodo()
'        '            Dim frmTodo As New frmTodo(m_InitializationData)
'        '            ' optional init new todo

'        '            If Not m_invoiceNumber.HasValue Then
'        '                Exit Sub
'        '            End If

'        '            If Not m_invoiceData Is Nothing Then

'        '                Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
'        '                Dim EmployeeNumber As Integer? = Nothing
'        '                Dim CustomerNumber As Integer? = m_invoiceData.KdNr
'        '                Dim ResponsiblePersonRecordNumber As Integer? = Nothing
'        '                Dim VacancyNumber As Integer? = Nothing
'        '                Dim ProposeNumber As Integer? = Nothing
'        '                Dim ESNumber As Integer? = Nothing
'        '                Dim RPNumber As Integer? = Nothing
'        '                Dim LMNumber As Integer? = Nothing
'        '                Dim RENumber As Integer? = m_invoiceNumber
'        '                Dim ZENumber As Integer? = Nothing
'        '                Dim Subject As String = String.Empty
'        '                Dim Body As String = ""

'        '                frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber,
'        '                                                                                                VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

'        '                frmTodo.Show()

'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Books a bonus out.
'        '        ''' </summary>
'        '        Private Sub BookoutBonus()

'        '            Dim errorText As String = m_Translate.GetSafeTranslationValue("Gutschrift konnte nicht ausgeglichen werden.")

'        '            ' First load invoice data again.
'        '            m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_invoiceData.ReNr)

'        '            If m_invoiceData Is Nothing Then
'        '                m_UtilityUI.ShowErrorDialog(errorText)
'        '                Return
'        '            End If

'        '            If (m_invoiceData.Bezahlt = m_invoiceData.BetragInk) Then
'        '                ' Gutschrift ist bereits schon ausgeglichen
'        '                m_invoiceData.Gebucht = 0
'        '                m_invoiceData.Bezahlt = 0
'        '                m_invoiceData.GebuchtAm = Nothing
'        '                lblRestBetragInProzent.Text = String.Empty

'        '            Else
'        '                ' Gutschrift muss ausgeglichen werden

'        '                Dim dateInputDialog As New DateInputDialog()
'        '                dateInputDialog.TitleText = m_Translate.GetSafeTranslationValue("Auf welches Datum möchten Sie die Gutschrift ausgleichen?")
'        '                dateInputDialog.TitleText = m_Translate.GetSafeTranslationValue("Datum Gutschriftausgleich")
'        '                dateInputDialog.StartPosition = FormStartPosition.CenterParent

'        '                dateInputDialog.ShowDialog()

'        '                If (Not dateInputDialog.DialogResult = DialogResult.OK) OrElse
'        '                        (Not dateInputDialog.SelectedDate.HasValue) Then
'        '                    Return
'        '                End If

'        '                Dim bookOutDate As Date = dateInputDialog.SelectedDate.Value.Date
'        '                If bookOutDate < m_invoiceData.FakDat Then
'        '                    m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Datum liegt vor dem Faktura-Datum! Der Gutschrift kann nicht ausgeglichen werden."))
'        '                    Return
'        '                End If
'        '                m_invoiceData.Gebucht = True
'        '                m_invoiceData.Bezahlt = m_invoiceData.BetragInk
'        '                m_invoiceData.GebuchtAm = bookOutDate
'        '                lblRestBetragInProzent.Text = String.Format(m_Translate.GetSafeTranslationValue("Ausgeglichen am: {0}"), m_invoiceData.GebuchtAm)

'        '            End If

'        '            Dim success = m_InvoiceDatabaseAccess.UpdateInvoice(m_invoiceData)

'        '            If Not success Then
'        '                m_UtilityUI.ShowErrorDialog(errorText)
'        '            Else
'        '                ReloadInvoiceValues()
'        '                SetInvoiceRowUi()
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Change invoice customer.
'        '        ''' </summary>
'        '        Private Sub ChangeInvoiceCustomer()

'        '            Dim errorText As String = m_Translate.GetSafeTranslationValue("Kundenname konnte nicht geändert werden.")

'        '            Dim singleLineTextInputDialog = New SingleLineTextInputDialog()
'        '            singleLineTextInputDialog.MessageText = m_Translate.GetSafeTranslationValue("Geben Sie bitte den neuen Kundennamen ein:")
'        '            singleLineTextInputDialog.TitleText = m_Translate.GetSafeTranslationValue("Kundennamen ändern")
'        '            singleLineTextInputDialog.StartPosition = FormStartPosition.CenterParent

'        '            singleLineTextInputDialog.ShowDialog()

'        '            If Not singleLineTextInputDialog.DialogResult = DialogResult.OK OrElse
'        '                    String.IsNullOrWhiteSpace(singleLineTextInputDialog.InputText) Then
'        '                Return
'        '            End If

'        '            ' First load invoice data again.
'        '            m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_invoiceData.ReNr)

'        '            If m_invoiceData Is Nothing Then
'        '                m_UtilityUI.ShowErrorDialog(errorText)
'        '                Return
'        '            End If

'        '            m_invoiceData.RName1 = singleLineTextInputDialog.InputText
'        '            m_selectedAddress.REFirma = m_invoiceData.RName1

'        '            Dim success = m_InvoiceDatabaseAccess.UpdateInvoice(m_invoiceData)

'        '            If Not success Then
'        '                m_UtilityUI.ShowErrorDialog(errorText)
'        '            Else
'        '                SetCustomerText(m_selectedCustomer.KDNr, m_invoiceData.RName1, m_invoiceData.RPLZ, m_invoiceData.ROrt)
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Changes the invoice mahnstop date.
'        '        ''' </summary>
'        '        Private Sub ChangeInvoiceMahnstopDate()

'        '            Dim errorText As String = m_Translate.GetSafeTranslationValue("Mahnstopp-Datum konnte nicht angepasst werden.")

'        '            ' First load invoice data again.
'        '            m_invoiceData = m_InvoiceDatabaseAccess.LoadInvoice(m_invoiceData.ReNr)

'        '            If m_invoiceData Is Nothing Then
'        '                m_UtilityUI.ShowErrorDialog(errorText)
'        '                Return
'        '            End If

'        '            Dim mahnStopDialog As New MahnstopDialog()
'        '            mahnStopDialog.TitleText = m_Translate.GetSafeTranslationValue("Bis zu welchem Datum soll der Mahnstopp für diese Rechnung gelten?") &
'        '                                                                    m_Translate.GetSafeTranslationValue("Zum Löschen einses Mahnstopps leer lassen.")
'        '            mahnStopDialog.TitleText = m_Translate.GetSafeTranslationValue("Datum für Mahnstopp")
'        '            mahnStopDialog.SelectedDate = m_invoiceData.MahnStopUntil
'        '            mahnStopDialog.StartPosition = FormStartPosition.CenterParent

'        '            mahnStopDialog.ShowDialog()

'        '            If (Not mahnStopDialog.DialogResult = DialogResult.OK) Then
'        '                Return
'        '            End If

'        '            Dim mahnStopDate As Date? = mahnStopDialog.SelectedDate
'        '            If mahnStopDate.HasValue AndAlso (mahnStopDate < m_invoiceData.FakDat) Then
'        '                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Datum liegt vor dem Faktura-Datum! Das Mahnstoppdatum kann nicht auf dieses Datum gesetzt werden."))
'        '                Return
'        '            End If

'        '            m_invoiceData.MahnStopUntil = mahnStopDate

'        '            Dim success = m_InvoiceDatabaseAccess.UpdateInvoice(m_invoiceData)

'        '            If Not success Then
'        '                m_UtilityUI.ShowErrorDialog(errorText)
'        '            Else
'        '                SetMahnStopValue(m_invoiceData.MahnStopUntil)
'        '            End If

'        '        End Sub

'        '        ''' <summary>
'        '        ''' Prepares status and navigation bar.
'        '        ''' </summary>
'        '        ''' <param name="invoice">The invoice object.</param>
'        '        Private Sub PrepareStatusAndNavigationBar(ByVal invoice As DataObjects.Invoice)

'        '            lblRestBetragInProzent.Text = String.Empty
'        '            If invoice Is Nothing Then
'        '                bsiCreated.Caption = String.Empty
'        '                bsiChanged.Caption = String.Empty
'        '                m_Wos_P_Data.Enabled = False

'        '                Return
'        '            End If

'        '            bsiCreated.Caption = String.Format(" {0:f}, {1}", invoice.CreatedOn, invoice.CreatedFrom)
'        '            bsiChanged.Caption = String.Format(" {0:f}, {1}", invoice.ChangedOn, invoice.ChangedFrom)

'        '            Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(invoice.KdNr, m_InitializationData.UserData.UserFiliale)

'        '            If Not customerMasterData Is Nothing Then
'        '                m_Wos_P_Data.Enabled = customerMasterData.WOSGuid <> String.Empty
'        '            Else
'        '                m_Wos_P_Data.Enabled = False
'        '            End If

'        '            m_ChangeCstomerName_NavbarItem.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 484, m_InitializationData.MDData.MDNr)
'        '            m_Guthaben_NavbarItem.Enabled = Not (m_invoiceData.Art = "A" OrElse m_invoiceData.Art = "I" OrElse m_invoiceData.Art = "F")
'        '            m_Guthaben_NavbarItem.Visible = Not (m_invoiceData.Art = "A" OrElse m_invoiceData.Art = "I" OrElse m_invoiceData.Art = "F")

'        '            If m_Guthaben_NavbarItem.Visible AndAlso m_Guthaben_NavbarItem.Enabled Then
'        '                If m_invoiceData.Bezahlt = m_invoiceData.BetragInk Then
'        '                    m_Guthaben_NavbarItem.Caption = m_Translate.GetSafeTranslationValue("Augleich löschen")
'        '                    lblRestBetragInProzent.Text = String.Format(m_Translate.GetSafeTranslationValue("Ausgeglichen am: {0}"), m_invoiceData.GebuchtAm)
'        '                Else
'        '                    m_Guthaben_NavbarItem.Caption = m_Translate.GetSafeTranslationValue("Gutschrift ausgleichen")
'        '                End If
'        '            End If


'        '        End Sub

'        '        ''' <summary>
'        '        ''' Focuses a invoice individual row.
'        '        ''' </summary>
'        '        ''' <param name="id">The individual row id.</param>
'        '        Private Sub FocusInvoiceIndividualRow(ByVal id As Integer)

'        '            If Not gridInvoiceRows.DataSource Is Nothing Then

'        '                Dim invoiceIndividualData = CType(gridInvoiceRows.DataSource, List(Of DataObjects.InvoiceIndividual))

'        '                Dim index = invoiceIndividualData.ToList().FindIndex(Function(data) data.Id = id)

'        '                Dim rowHandle = gvInvoiceRows.GetRowHandle(index)
'        '                gvInvoiceRows.FocusedRowHandle = rowHandle
'        '            End If

'        '        End Sub

'        '#End Region

'        '#End Region

'        '#Region "Helper Classes"

'        '        ''' <summary>
'        '        ''' Debitoren Art
'        '        ''' </summary>
'        '        Private Class Debitorenart
'        '            Public Property Display As String
'        '            Public Property Value As String
'        '            Public ReadOnly Property Label As String
'        '                Get
'        '                    Return Value + "  -  " + Display
'        '                End Get
'        '            End Property
'        '        End Class

'        '        ''' <summary>
'        '        ''' Branch view data.
'        '        ''' </summary>
'        '        Class BranchViewData
'        '            Public Property Branche As String
'        '            Public Property TranslatedBrancheText As String
'        '            Public Property IsAssignedToCustomer
'        '        End Class

'        '        Private Sub XtraScrollableControl2_Click(sender As Object, e As EventArgs) Handles XtraScrollableControl2.Click

'        '        End Sub

'        '#End Region



'    End Class

'End Namespace

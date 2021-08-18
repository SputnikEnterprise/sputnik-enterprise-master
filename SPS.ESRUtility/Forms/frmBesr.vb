
Imports System.IO

Imports SP.DatabaseAccess.ESRUtility.DataObjects
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports DevExpress.LookAndFeel
Imports SPS.ESRUtility.Settings
Imports System.Reflection.Assembly
Imports SP.DatabaseAccess.Common.DataObjects

Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.ESRUtility
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPS.Listing.Print.Utility
Imports SPS.ESRUtility.EsrRecord
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraBars.ToastNotifications


Namespace UI

	''' <summary>
	''' BESR From.
	''' </summary>
	''' <remarks></remarks>
	Public Class frmBesr


#Region "Private Consts"

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Public Shared Methods"

		''' <summary>
		''' Shows a new form instance.
		''' </summary>
		''' <param name="initData"></param>
		''' <param name="showModal"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ShowForm(ByVal initData As SP.Infrastructure.Initialization.InitializeClass, Optional ByVal showModal As Boolean = False) As frmBesr
			Dim form = New frmBesr(initData)
			form.LoadData(initData)
			If showModal Then
				form.ShowDialog()
			Else
				form.Show()
			End If
			Return form
		End Function

#End Region ' Public Shared Methods

#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The current connection string.
		''' </summary>
		Private m_CurrentConnectionString = String.Empty

		''' <summary>
		''' The common database access.
		''' </summary>
		Protected m_CommonDatabaseAccess As SP.DatabaseAccess.Common.ICommonDatabaseAccess

		''' <summary>
		''' The ESR utility data access object.
		''' </summary>
		Private m_ESRUtilityDatabaseAccess As SP.DatabaseAccess.ESRUtility.ESRUtilityDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Private m_SettingsManager As ISettingsManager

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if initial data has been loaded.
		''' </summary>
		Private m_IsInitialDataLoaded As Boolean = False

		''' <summary>
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		Private m_IsDataValid As Boolean = True

		''' <summary>
		''' The SPProgUtility object.
		''' </summary>
		Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Private m_mandant As Mandant
		Private m_path As ClsProgPath
		Private m_DebitorSetting As String

		Private m_mandantDataList As IEnumerable(Of MandantData)

		Private m_mandantYearDataList As IList(Of SP.DatabaseAccess.ESRUtility.DataObjects.MandantYearData)
		Private m_mandantYearDataSelected As SP.DatabaseAccess.ESRUtility.DataObjects.MandantYearData

		Private m_bankDataList As IList(Of SP.DatabaseAccess.ESRUtility.DataObjects.BankData)
		Private m_bankDataSelected As SP.DatabaseAccess.ESRUtility.DataObjects.BankData

		Private m_esrDataList As IList(Of EsrItem)
		Private m_Camt054Data As ESRRecord_Camt054
		Private m_CurrentCamt054Data As List(Of ESRData)
		Private m_esrDataSummary As EsrSummaryItem

		Private m_postingAccounts As New Dictionary(Of Integer, String)

		Private m_CurrentFileBytes As Byte()
		Private m_FirstpaymentNumber As Integer?
		Private m_DiskIdentity As String

		''' <summary>
		''' allowed to open lists designmodus?
		''' </summary>
		''' <remarks></remarks>
		Private m_AllowedDesign As Boolean
		Private m_AllowedMandantChange As Boolean

		Private m_AllowedPain001 As Boolean

		Private m_Unprocessed As Image
		Private m_InProcessing As Image
		Private m_Processed As Image
		Private m_Question As Image
		Private m_Failed As Image
		Private m_Lower As Image
		Private m_Higher As Image

#End Region  ' Private Fields


#Region "private properties"

		Private ReadOnly Property GetHwnd() As String
			Get
				Return Me.Handle
			End Get
		End Property


		''' <summary>
		''' Gets the reference number to 10 setting.
		''' </summary>
		Private ReadOnly Property ReferenceNumbersTo10Setting As Boolean
			Get

				Dim ref10forfactoring As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, lueYear.EditValue),
																																															String.Format("MD_{0}/Debitoren/ref10forfactoring", m_InitializationData.MDData.MDNr)), False)

				Return ref10forfactoring.HasValue AndAlso ref10forfactoring
			End Get
		End Property

		Private ReadOnly Property MandantAllowedPain001 As Boolean
			Get
				Dim result As Boolean = False
				result = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, lueYear.EditValue),
																																															String.Format("MD_{0}/Debitoren/mandantallowedwithcamt054", m_InitializationData.MDData.MDNr)), False)
				Return result

			End Get
		End Property

#End Region


#Region "Private Methods"

		''' <summary>
		''' Constructor (Private to force use of shared sub ShowForm).
		''' </summary>
		''' <param name="initData"></param>
		''' <remarks></remarks>
		Private Sub New(ByVal initData As SP.Infrastructure.Initialization.InitializeClass)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Try
				' Mandantendaten
				m_mandant = New Mandant
				m_path = New ClsProgPath

				m_InitializationData = initData
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(initData.TranslationData, initData.ProsonalizedData)
				m_UtilityUI = New UtilityUI
				m_Utility = New Utility

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_SuppressUIEvents = True
			InitializeComponent()
			m_SuppressUIEvents = False


			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Me.CreateDatabaseAccessObjects(m_InitializationData.MDData.MDNr)

			m_SettingsManager = New SettingsManager

			m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 506, m_InitializationData.MDData.MDNr)
			m_AllowedMandantChange = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)

			' Translate controls.
			TranslateControls()
			m_AllowedPain001 = MandantAllowedPain001

			Reset()

			m_Unprocessed = My.Resources.Unprocessed
			m_InProcessing = My.Resources.Processing
			m_Processed = My.Resources.Processed
			m_Question = My.Resources.question_16x16
			m_Failed = My.Resources.Failed
			m_Lower = My.Resources.lower
			m_Higher = My.Resources.higher

			LoadFormSettings()

			AddHandler gvChoosenEmployees.RowCellClick, AddressOf Ongv_RowCellClick

		End Sub

		''' <summary>
		''' Reads the payment offset from the settings.
		''' </summary>
		''' <returns>Invoice offset or zero if it could not be read.</returns>
		Private Function ReadPaymentOffsetFromSettings() As Integer

			Dim strQuery As String = "//StartNr/Zahlungseingänge"
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
		''' <remarks></remarks>
		Private Sub LoadPostingAccounts()

			m_postingAccounts = New Dictionary(Of Integer, String)
			Try

				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/BuchungsKonten", m_InitializationData.MDData.MDNr)
				For i As Integer = 1 To 38
					Dim strValue As String = m_Utility.ParseToString(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, lueYear.EditValue),
																																										String.Format("{0}/_{1}", FORM_XML_MAIN_KEY, i)), "")
					m_postingAccounts.Add(i, strValue)
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

		End Sub


#Region "Reset Form"

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()
			'  Reset grids, drop downs and lists, etc.
			ResetMandantDropDown()
			ResetYearDataDropDown()
			ResetBankDataDropDown()
			ResetEsrFileContentGrid()

			m_esrDataList = New List(Of EsrItem)
			grdChoosenEmployees.DataSource = m_esrDataList

			txtFilePath.Properties.TextEditStyle = TextEditStyles.DisableTextEditor

			Me.lueMandant.Visible = m_AllowedMandantChange
			Me.lblMandant.Visible = m_AllowedMandantChange

			bbiAction.Enabled = False
			bbiZEPrint.Enabled = False
			bbiESRPrint.Enabled = False
			chkUse7Digits.Visible = m_InitializationData.UserData.UserNr = 1

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

			lueMandant.Properties.ShowFooter = False
			lueMandant.Properties.DropDownRows = 10
			lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueMandant.Properties.SearchMode = SearchMode.AutoComplete
			lueMandant.Properties.AutoSearchColumnIndex = 0

			lueMandant.Properties.NullText = String.Empty
			lueMandant.EditValue = Nothing

		End Sub

		Private Sub ResetYearDataDropDown()
			lueYear.Properties.DisplayMember = "Jahr"
			lueYear.Properties.ValueMember = "Jahr"

			lueYear.Properties.Columns.Clear()
			lueYear.Properties.BestFitMode = BestFitMode.None
			lueYear.Properties.Columns.Add(New LookUpColumnInfo("Jahr"))

			lueYear.EditValue = Nothing
		End Sub

		Private Sub ResetBankDataDropDown()
			lueBankData.Properties.DisplayMember = "DisplayName"
			'lueBankData.Properties.ValueMember = "ID"

			lueBankData.Properties.Columns.Clear()
			lueBankData.Properties.BestFitMode = BestFitMode.BestFit
			'lueBankData.Properties.Columns.Add(New LookUpColumnInfo("KontoESR1", 0))
			lueBankData.Properties.Columns.Add(New LookUpColumnInfo("DisplayName", 0))

			lueBankData.EditValue = Nothing
		End Sub

		Private Sub ResetEsrFileContentGrid()
			Dim fieldName As String

			gvChoosenEmployees.OptionsView.ShowIndicator = False
			gvChoosenEmployees.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvChoosenEmployees.OptionsView.ShowFooter = True
			gvChoosenEmployees.OptionsBehavior.AutoUpdateTotalSummary = True
			gvChoosenEmployees.OptionsBehavior.SummariesIgnoreNullValues = True

			gvChoosenEmployees.Columns.Clear()


			fieldName = "amountDecision"
			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = "Resultat"
			docType.Name = fieldName
			docType.FieldName = fieldName
			docType.Visible = True
			docType.OptionsColumn.AllowEdit = False
			docType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			docType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			docType.AppearanceHeader.Options.UseTextOptions = True
			docType.AppearanceCell.Options.UseTextOptions = True
			docType.Width = 50
			docType.ColumnEdit = New RepositoryItemPictureEdit() With {.InitialImage = My.Resources.Processing,
				.NullText = " ",
				.SizeMode = PictureSizeMode.Zoom}
			docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
			gvChoosenEmployees.Columns.Add(docType)


			fieldName = "EsrInvoiceNo"
			Dim colEsrInvoiceNo As New DevExpress.XtraGrid.Columns.GridColumn()
			colEsrInvoiceNo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			colEsrInvoiceNo.Caption = m_Translate.GetSafeTranslationValue("Rechnungs-Nr.")
			colEsrInvoiceNo.Name = fieldName
			colEsrInvoiceNo.FieldName = fieldName
			colEsrInvoiceNo.Visible = True
			colEsrInvoiceNo.Width = 60
			colEsrInvoiceNo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			colEsrInvoiceNo.AppearanceHeader.Options.UseTextOptions = True
			gvChoosenEmployees.Columns.Add(colEsrInvoiceNo)

			fieldName = "EsrCustomerNo"
			Dim colEsrCustomerNo As New DevExpress.XtraGrid.Columns.GridColumn()
			colEsrCustomerNo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			colEsrCustomerNo.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
			colEsrCustomerNo.Name = fieldName
			colEsrCustomerNo.FieldName = fieldName
			colEsrCustomerNo.Visible = False
			colEsrCustomerNo.Width = 60
			colEsrCustomerNo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			colEsrCustomerNo.AppearanceHeader.Options.UseTextOptions = True
			gvChoosenEmployees.Columns.Add(colEsrCustomerNo)

			fieldName = "Customername"
			Dim colCustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			colCustomername.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			colCustomername.Caption = m_Translate.GetSafeTranslationValue("Firma")
			colCustomername.Name = "colCustomername"
			colCustomername.FieldName = fieldName
			colCustomername.Visible = True
			colCustomername.Width = 150
			gvChoosenEmployees.Columns.Add(colCustomername)

			fieldName = "EsrValutaDate"
			Dim colEsrValutaDate As New DevExpress.XtraGrid.Columns.GridColumn()
			colEsrValutaDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			colEsrValutaDate.Caption = m_Translate.GetSafeTranslationValue("Valuta")
			colEsrValutaDate.Name = "colEsrValutaDate"
			colEsrValutaDate.FieldName = fieldName
			colEsrValutaDate.Visible = True
			colEsrValutaDate.Width = 70
			gvChoosenEmployees.Columns.Add(colEsrValutaDate)

			fieldName = "EsrAmount"
			Dim colEsrAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			colEsrAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			colEsrAmount.Name = fieldName
			colEsrAmount.FieldName = fieldName
			colEsrAmount.Visible = True
			colEsrAmount.Width = 80
			colEsrAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			colEsrAmount.AppearanceHeader.Options.UseTextOptions = True
			colEsrAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			colEsrAmount.DisplayFormat.FormatString = "n2"
			colEsrAmount.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			colEsrAmount.SummaryItem.DisplayFormat = "{0:n2}"
			gvChoosenEmployees.Columns.Add(colEsrAmount)

			fieldName = "InvoiceAmountOpen"
			Dim colInvoiceAmountOpen As New DevExpress.XtraGrid.Columns.GridColumn()
			colInvoiceAmountOpen.Caption = m_Translate.GetSafeTranslationValue("Betrag offen")
			colInvoiceAmountOpen.Name = fieldName
			colInvoiceAmountOpen.FieldName = fieldName
			colInvoiceAmountOpen.Visible = True
			colInvoiceAmountOpen.Width = 80
			colInvoiceAmountOpen.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			colInvoiceAmountOpen.AppearanceHeader.Options.UseTextOptions = True
			colInvoiceAmountOpen.AppearanceCell.Options.UseTextOptions = True
			colInvoiceAmountOpen.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			colInvoiceAmountOpen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			colInvoiceAmountOpen.DisplayFormat.FormatString = "n2"
			colInvoiceAmountOpen.DisplayFormat.FormatString = "n2"
			colInvoiceAmountOpen.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
			colInvoiceAmountOpen.SummaryItem.DisplayFormat = "{0:n2}"
			gvChoosenEmployees.Columns.Add(colInvoiceAmountOpen)

			fieldName = "Status"
			Dim colStatus As New DevExpress.XtraGrid.Columns.GridColumn()
			colStatus.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			colStatus.Caption = m_Translate.GetSafeTranslationValue("Status")
			colStatus.Name = fieldName
			colStatus.FieldName = fieldName
			colStatus.Visible = True
			colStatus.Width = 160
			gvChoosenEmployees.Columns.Add(colStatus)

			fieldName = "EsrData"
			Dim colEsrData As New DevExpress.XtraGrid.Columns.GridColumn()
			colEsrData.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			colEsrData.Caption = m_Translate.GetSafeTranslationValue("ESR-Zeile")
			colEsrData.Name = fieldName
			colEsrData.FieldName = fieldName
			colEsrData.Visible = True
			colEsrData.Width = 120
			gvChoosenEmployees.Columns.Add(colEsrData)

			grdChoosenEmployees.DataSource = Nothing

		End Sub


#End Region  ' Reset Forms


#Region "Load Data"

		''' <summary>
		''' Loads the data.
		''' </summary>
		''' <returns>True wenn erfolgreich</returns>
		Private Function LoadData(ByVal initData As SP.Infrastructure.Initialization.InitializeClass) As Boolean
			Dim success As Boolean = True

			success = LoadMandantDropDownData()
			success = LoadYearDropDownData()

			Me.SetMandant(initData.MDData.MDNr)

			m_SuppressUIEvents = True
			chkUse7Digits.Checked = Not ReferenceNumbersTo10Setting
			m_SuppressUIEvents = False

			txtFilePath.EditValue = m_SettingsManager.ReadString(SettingKeys.SETTING_ESRFile_LOCATION)
			success = ValidateESRFile(txtFilePath.EditValue)

			Return success

		End Function

		Private Function LoadMandantDropDownData() As Boolean
			Dim success As Boolean = True

			m_mandantDataList = m_CommonDatabaseAccess.LoadCompaniesListData()
			If (m_mandantDataList Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
				success = False
			End If

			lueMandant.Properties.DataSource = m_mandantDataList
			lueMandant.Properties.DropDownRows = If(m_mandantDataList Is Nothing, 0, Math.Min(m_mandantDataList.Count, 20))
			lueMandant.Properties.ForceInitialize()

			Return success
		End Function

		Private Function LoadYearDropDownData() As Boolean
			Dim success As Boolean = True

			m_mandantYearDataList = m_ESRUtilityDatabaseAccess.LoadMandantYearData()
			success = Not m_mandantYearDataList Is Nothing

			lueYear.EditValue = Nothing
			lueYear.Properties.DataSource = m_mandantYearDataList

			lueYear.EditValue = Now.Year

			Return success

		End Function

		Private Function LoadBankDataDropDownData() As Boolean
			Dim success As Boolean = True

			If lueMandant.EditValue IsNot Nothing Then
				m_bankDataList = m_ESRUtilityDatabaseAccess.LoadBankData(lueMandant.EditValue)
				success = Not m_bankDataList Is Nothing
			End If

			lueBankData.EditValue = Nothing
			lueBankData.Properties.DataSource = m_bankDataList
			lueBankData.Properties.DropDownRows = If(m_bankDataList Is Nothing, 0, Math.Min(m_bankDataList.Count, 20))
			If m_bankDataList IsNot Nothing AndAlso m_bankDataList.Count > 0 Then
				lueBankData.EditValue = m_bankDataList(0)
			End If
			lueBankData.Properties.ForceInitialize()

			Return success
		End Function

		Private Function LoadEsrFileContentGridData() As Boolean
			Dim success As Boolean = True
			Dim continueWithReading As Boolean = True
			Dim esrFileParser = New EsrFileParser(m_ESRUtilityDatabaseAccess, m_InitializationData)
			Dim filename As String = txtFilePath.EditValue
			Dim fileESRKontoNumber As String = String.Empty
			Dim isMoreAcountNumber As Boolean = False

			bbiAction.Enabled = False
			grdChoosenEmployees.DataSource = Nothing

			esrFileParser.Use7Digit = chkUse7Digits.Checked
			esrFileParser.BankNumber = m_bankDataSelected.ID

			Try

				txtFilePath.EditValue = filename
				m_CurrentFileBytes = m_Utility.LoadFileBytes(filename)

				success = Not (m_CurrentFileBytes Is Nothing OrElse m_CurrentFileBytes.Length = 0)
				bbiESRPrint.Enabled = False
				bbiZEPrint.Enabled = False

				If success Then
					Dim saveddata = m_ESRUtilityDatabaseAccess.IsESRFileAlreadySaved(m_InitializationData.MDData.MDNr, New System.IO.FileInfo(filename), m_CurrentFileBytes)
					If Not saveddata Is Nothing Then
						Dim msg As String = "Die Datei {1} wurde bereits am {2} durch {3} eingelesen.{0}Bitte versuchen Sie eine andere Datei auszuwählen."
						msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine, filename, saveddata.createdon, saveddata.createdfrom)
						m_UtilityUI.ShowErrorDialog(msg)

						continueWithReading = False
					End If
				End If

				If success Then
					Dim esrFileData = New EsrFileData
					If chkCamt054.Checked Then
						m_Camt054Data = esrFileParser.ParseCamt054File(txtFilePath.Text)

						If m_Camt054Data Is Nothing OrElse m_Camt054Data.Ntfctn Is Nothing OrElse m_Camt054Data.Ntfctn.Ntry.Count = 0 Then
							m_Logger.LogError(String.Format("Die Datei konnte nicht gelesen werden: {0}", txtFilePath.Text))
							m_UtilityUI.ShowErrorDialog(String.Format("Die Datei konnte nicht gelesen werden: {0}", txtFilePath.Text))

							Return False
						End If
						Dim ntryData = From v In m_Camt054Data.Ntfctn.Ntry
						If ntryData Is Nothing Then Return Nothing
						m_CurrentCamt054Data = New List(Of ESRData)
						Try
							m_Logger.LogDebug(String.Format("Anzahl ntryData: {0}", ntryData.Count))

							For Each itm In ntryData

								Dim txDlt = From v In itm.NtryDtls.TxDtls
								m_Logger.LogDebug(String.Format("Anzahl txDlt: {0}", txDlt.Count))

								fileESRKontoNumber = itm.NtryRef
								isMoreAcountNumber = Not (fileESRKontoNumber.Contains(m_bankDataSelected.KontoESR1.Replace(" ", "")) OrElse fileESRKontoNumber.Contains(m_bankDataSelected.ESRIban1.Replace(" ", "")) OrElse fileESRKontoNumber.Contains(m_bankDataSelected.ESRCustomerID.Replace(" ", "")))

								For Each tx In txDlt
									Dim viewData As New ESRData
									Dim invoiceAmount As Double?
									Dim invoiceOpenAmount As Double?

									viewData.EsrCustomerNo = tx.RmtInf.CustomerNumber
									viewData.Customername = tx.RltdPtiesNm
									viewData.EsrInvoiceNo = tx.RmtInf.InvoiceNumber
									viewData.EsrValutaDate = itm.ValDt
									viewData.EsrAmount = tx.Amount
									viewData.EsrData = tx.RmtInf.Ref

									Dim invoicedata = LoadInoviceData(viewData.EsrInvoiceNo)
									If Not invoicedata Is Nothing Then
										invoiceAmount = invoicedata.BetragInk
										invoiceOpenAmount = invoicedata.BetragInk - invoicedata.Bezahlt

										viewData.InvoiceAmountOpen = invoiceOpenAmount
										viewData.Status = InvoiceStatus(viewData.EsrAmount, invoiceOpenAmount)
										If String.IsNullOrWhiteSpace(viewData.Customername) Then viewData.Customername = invoicedata.CustomerName
									Else
										m_Logger.LogDebug(String.Format("keine Rechnung wurde gefunden. ESRNumer: {0}", viewData.EsrInvoiceNo))
										viewData.Status = InvoiceStatus(Nothing, Nothing)
									End If



									m_CurrentCamt054Data.Add(viewData)
								Next

							Next

						Catch ex As Exception
							m_Logger.LogError(String.Format("Schleife der Liste: {0}", ex.ToString))

							success = False

						End Try
						m_Logger.LogDebug(String.Format("Anzahl ESR-Einträge: {0}", m_CurrentCamt054Data.Count))
						If isMoreAcountNumber Then
							Dim msg As String = "Die ausgewählte Datei kann nicht validiert werden.<br>Ich kann versuche trotzdem die offenen Rechnungen zu finden und diese buchen.<br><br>Datei-Kontonummer: {0}<br>Mandanten-Kontonummer: {1}"
							msg &= "<br><br>Soll ich den Vorgang fortsetzen?"
							msg = String.Format(m_Translate.GetSafeTranslationValue(msg), fileESRKontoNumber, m_bankDataSelected.KontoESR1)

							Dim msgResult = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Nicht bekannte Kontonummer"))
							If Not msgResult Then Return False

						ElseIf m_CurrentCamt054Data.Count = 0 Then
							m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Es wurden {0} Zahlungen gefunden, aber konnte keine entsprechende Rechnungsnummer wurde gefunden."), ntryData.Count))
						End If

						grdChoosenEmployees.DataSource = m_CurrentCamt054Data
						Dim sumAmount = m_CurrentCamt054Data.Sum(Function(x) x.InvoiceAmountOpen)
						bbiAction.Enabled = sumAmount > 0

						gvChoosenEmployees.Columns("EsrCustomerNo").Visible = False
						gvChoosenEmployees.Columns("Customername").Visible = True

					Else
						esrFileData = esrFileParser.ParseEsrFile(txtFilePath.Text)
						m_esrDataList = (From e In esrFileData.ListDetail
										 Select New EsrItem(m_ESRUtilityDatabaseAccess, e, m_InitializationData)
																 ).ToList()
						m_esrDataSummary = New EsrSummaryItem(esrFileData.SummaryRecord)
						grdChoosenEmployees.DataSource = m_esrDataList

						fileESRKontoNumber = esrFileData.List(0).KontoNo
						isMoreAcountNumber = Not (fileESRKontoNumber.Contains(m_bankDataSelected.KontoESR1.Replace(" ", "")) OrElse fileESRKontoNumber.Contains(m_bankDataSelected.ESRIban1.Replace(" ", "")) OrElse fileESRKontoNumber.Contains(m_bankDataSelected.ESRCustomerID.Replace(" ", "")))
						If isMoreAcountNumber Then
							Dim msg As String = "Die ausgewählte Datei kann nicht validiert werden.<br>Ich kann versuche trotzdem die offenen Rechnungen zu finden und diese buchen.<br><br>Datei-Kontonummer: {0}<br>Mandanten-Kontonummer: {1}"
							msg &= "<br><br>Soll ich den Vorgang fortsetzen?"
							msg = String.Format(m_Translate.GetSafeTranslationValue(msg), fileESRKontoNumber, m_bankDataSelected.KontoESR1)

							Dim msgResult = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Nicht bekannte Kontonummer"))
							If Not msgResult Then Return False
						End If

						gvChoosenEmployees.Columns("EsrCustomerNo").Visible = True
						gvChoosenEmployees.Columns("Customername").Visible = False
						If m_esrDataList Is Nothing OrElse m_esrDataList.Count = 0 Then ' AndAlso fileESRKontoNumber <> m_bankDataSelected.KontoESR1 Then
							m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Möglicherweise konnte die Datei nicht validiert werden.<br>Es wurden keine Zahlungen gefunden.")), m_Translate.GetSafeTranslationValue("Zahlung einlesen"))

							success = False
						End If

						bbiAction.Enabled = success
					End If

					bsiAllEntriesCount.Caption = gvChoosenEmployees.RowCount

				End If

				If Not continueWithReading Then
					bbiAction.Enabled = False
					success = False
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				success = False
			End Try


			Return success

		End Function


#End Region ' Load Data


#Region "Set Data"

		Private Sub SetMandant(mdNr As Integer)
			Dim mandantData As MandantData = m_mandantDataList.Where(Function(md) md.MandantNumber = mdNr).FirstOrDefault()
			lueMandant.EditValue = mandantData.MandantNumber
		End Sub

#End Region  ' Set Data

#Region "Event Handlers"

		Private Sub OngvChoosenEmployees_CustomDrawCell(sender As Object, e As Views.Base.RowCellCustomDrawEventArgs) Handles gvChoosenEmployees.CustomDrawCell

			If e.Column.Name.ToLower = "amountDecision".ToLower Then
				Dim state = CType(e.CellValue, EsrItem.PaymentProcessState)
				'Dim rec As Rectangle = New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1)

				Dim p As Point = New Point()
				Dim img As Image

				Select Case state
					Case PaymentProcessState.InProcessing
						img = m_InProcessing

					Case PaymentProcessState.Processed
						img = m_Processed

					Case PaymentProcessState.Question
						img = m_Question

					Case PaymentProcessState.Failed
						img = m_Failed

					Case PaymentProcessState.Lower
						img = m_Lower

					Case PaymentProcessState.Higher
						img = m_Higher

					Case Else
						img = m_Unprocessed

				End Select

				p.X = e.Bounds.Location.X + (e.Bounds.Width - img.Width) / 2
				p.Y = e.Bounds.Location.Y + (e.Bounds.Height - img.Height) / 2
				e.Graphics.DrawImage(img, p)

			End If

		End Sub

		'Private Sub OngvChoosenEmployees_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvChoosenEmployees.RowStyle

		'	If e.RowHandle >= 0 Then

		'		Dim rowData = CType(gvChoosenEmployees.GetRow(e.RowHandle), ESRData)
		'		If rowData Is Nothing Then Return

		'		If rowData.EsrAmount.GetValueOrDefault(0) <> rowData.InvoiceAmountOpen.GetValueOrDefault(0) Then
		'			e.Appearance.BackColor = Color.OrangeRed
		'			e.Appearance.BackColor2 = Color.OrangeRed
		'		End If

		'	End If

		'End Sub

		Private Sub OngvChoosenEmployees_RowCellStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gvChoosenEmployees.RowCellStyle

			If e.RowHandle >= 0 Then
				If Not e.Column.Name.ToLower = "Status".ToLower Then Return

				'Dim rowData = CType(gvChoosenEmployees.GetRow(e.RowHandle), EsrItem)
				'If rowData Is Nothing Then Return

				If e.CellValue<> "Zahlung korrekt" Then
					'If rowData.EsrAmount.GetValueOrDefault(0) <> rowData.InvoiceAmountOpen.GetValueOrDefault(0) Then
					e.Appearance.BackColor = Color.Orange
					e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
					e.Appearance.BackColor2 = Color.Orange
				End If

			End If

		End Sub

		Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvChoosenEmployees.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then

					If chkCamt054.Checked Then
						Dim viewData = CType(dataRow, ESRData)

						Select Case column.Name.ToLower
							Case "EsrCustomerNo".ToLower, "colCustomername".ToLower
								If viewData.EsrCustomerNo > 0 Then OpenSelectedCustomer(viewData.EsrCustomerNo)

							Case Else
								If viewData.EsrInvoiceNo > 0 Then OpenSelectedInvoice(viewData.EsrInvoiceNo)

						End Select

					Else
						Dim viewData = CType(dataRow, EsrItem)

						Select Case column.Name.ToLower
							Case "EsrCustomerNo".ToLower, "colCustomername".ToLower
								If viewData.EsrCustomerNo > 0 Then OpenSelectedCustomer(viewData.EsrCustomerNo)

							Case Else
								If viewData.EsrInvoiceNo > 0 Then OpenSelectedInvoice(viewData.EsrInvoiceNo)

						End Select

					End If

				End If

				End If

		End Sub

		Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs) Handles gvChoosenEmployees.CustomDrawEmptyForeground
			Dim s As String = "Keine Daten sind vorhanden"

			Try
				s = m_Translate.GetSafeTranslationValue(s)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
			Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
			e.Graphics.DrawString(s, font, Brushes.Black, r)

		End Sub


		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_mandant.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		End Sub

		''' <summary>
		''' Loads form settings if form gets visible.
		''' </summary>
		Private Sub OnForm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged
			If Visible Then
				'LoadFormSettings()
			End If
		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnForm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
			CleanupAndHideForm()
			e.Cancel = True
		End Sub

		''' <summary>
		''' Keypreview for Modul-version
		''' </summary>
		Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
			If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
				Dim frm As New frmLibraryInfo(m_InitializationData)
				frm.LoadAssemblyData()

				frm.Show()
				frm.BringToFront()
			End If
		End Sub


		''' <summary>
		''' Handles the form drag enter event.
		''' </summary>
		Private Sub OnForm_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
			Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

			If Not files Is Nothing AndAlso files.Count > 0 Then

				For Each file In files

					Dim fileInfo As New IO.FileInfo(file)

					If fileInfo.Extension.ToLower() = ".v11" OrElse fileInfo.Extension.ToLower() = ".esr" OrElse fileInfo.Extension.ToLower() = ".xml" Then
						' At least on pdf file must be in the collection.
						e.Effect = DragDropEffects.Copy
						Return
					End If

				Next

			End If

			e.Effect = DragDropEffects.None

		End Sub

		''' <summary>
		''' Handles the form dragdrop event.
		''' </summary>
		Private Sub OnADdLMDoc_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
			Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

			Dim comleteSuccess As Boolean = True

			If Not files Is Nothing AndAlso files.Count > 0 Then

				For Each file In files
					Dim fileInfo As New IO.FileInfo(file)

					If fileInfo.Extension.ToLower() = ".v11" OrElse fileInfo.Extension.ToLower() = ".esr" OrElse fileInfo.Extension.ToLower() = ".xml" Then
						Dim success = ValidateESRFile(fileInfo.FullName)
					End If

					Exit For
				Next
			End If

		End Sub

		Private Sub OnLueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

			If lueMandant.EditValue IsNot Nothing Then

				m_esrDataList = Nothing
				grdChoosenEmployees.DataSource = m_esrDataList

				If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
					Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

					Dim clsMandant = m_mandant.GetSelectedMDData(lueMandant.EditValue)
					Dim logedUserData = m_mandant.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
					Dim personalizedData = m_InitializationData.ProsonalizedData
					Dim translate = m_InitializationData.TranslationData

					m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

					m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 506, m_InitializationData.MDData.MDNr)

					'm_AllowedPain001 = MandantAllowedPain001
				End If
				'chkCamt054.Checked = m_AllowedPain001
				'chkCamt054.Enabled = m_AllowedPain001

				Me.CreateDatabaseAccessObjects(lueMandant.EditValue)
				Me.LoadBankDataDropDownData()
				If txtFilePath.Text.Length > 0 Then
					Dim success = LoadEsrFileContentGridData()
				End If
			End If

		End Sub

		Private Sub OnLueYear_EnabledChanged(sender As Object, e As EventArgs) Handles lueYear.EnabledChanged
			m_mandantYearDataSelected = TryCast(lueYear.EditValue, SP.DatabaseAccess.ESRUtility.DataObjects.MandantYearData)
		End Sub

		Private Sub OnLueBankData_EditValueChanged(sender As Object, e As EventArgs) Handles lueBankData.EditValueChanged
			m_bankDataSelected = TryCast(lueBankData.EditValue, SP.DatabaseAccess.ESRUtility.DataObjects.BankData)
		End Sub

		Private Sub OnChkUse7Digits_CheckedChanged(sender As Object, e As EventArgs) Handles chkUse7Digits.CheckedChanged

			If Not m_SuppressUIEvents Then
				If txtFilePath.Text.Length > 0 Then
					Dim success = LoadEsrFileContentGridData()
				End If
			End If

		End Sub

		Private Sub OnTxtFilePath_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles txtFilePath.ButtonClick
			Dim fileName = String.Empty

			If e.Button.Index = 0 Then
				If chkCamt054.Checked Then
					fileName = m_UtilityUI.ShowOpenFileDialog(Me, "camt.054 Dateien (*.XML)|*.xml")
				Else
					fileName = m_UtilityUI.ShowOpenFileDialog(Me, "ESR V11 Dateien (*.V11, *.ESR)|*.v11;*.esr")
				End If
			Else
				LoadEsrFileContentGridData()
			End If

			Dim success = ValidateESRFile(fileName)

		End Sub

		'Private Sub OngvChoosenEmployees_CustomSummaryCalculate(sender As Object, e As DevExpress.Data.CustomSummaryEventArgs) Handles gvChoosenEmployees.CustomSummaryCalculate
		'Dim item = CType(e.Item, DevExpress.XtraGrid.GridColumnSummaryItem)
		'Select Case e.SummaryProcess
		'	Case DevExpress.Data.CustomSummaryProcess.Finalize
		'		Select Case item.FieldName
		'			Case "EsrAmount"
		'				e.TotalValue = If(m_esrDataSummary Is Nothing, 0D, m_esrDataSummary.EsrAmount)
		'			Case "EsrData"
		'				e.TotalValue = If(m_esrDataSummary Is Nothing, String.Empty, m_esrDataSummary.EsrData)
		'		End Select
		'End Select
		'End Sub

		Private Sub OnbbiAction_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiAction.ItemClick
			Dim success As Boolean = True
			Dim amountDecision As String = "OK"
			If txtFilePath.EditValue Is Nothing Then Return

			m_DiskIdentity = String.Empty
			m_FirstpaymentNumber = 0

			' loads all accounts
			LoadPostingAccounts()

			If chkCamt054.Checked Then
				If m_CurrentCamt054Data Is Nothing OrElse m_CurrentCamt054Data.Count = 0 Then Return
				success = success AndAlso BuildCamt054File()

			Else
				If m_esrDataList Is Nothing OrElse m_esrDataList.Count = 0 Then Return
				Dim msg = m_Translate.GetSafeTranslationValue("Sie verwenden eine <b>alte Schnittstelle</b> zum Lesen der Zahlungen!<br>Diese Schnittstelle wird in der Zukunft nicht mehr unterstützut!<br><br>Bitte laden Sie die camt054-Datei von Ihrer Bank herunter.")
				m_UtilityUI.ShowOKDialog(msg, m_Translate.GetSafeTranslationValue("Alte Format"), MessageBoxIcon.Exclamation)

				success = success AndAlso BuildV11File()

			End If

			bbiAction.Enabled = False

		End Sub

		Private Function BuildCamt054File() As Boolean
			Dim success As Boolean = True
			Dim bookingCount As Integer = 0
			Dim bookingAmountSum As Decimal = 0D
			Dim fileinfo As System.IO.FileInfo
			Dim amountDecision As String = "OK"
			If txtFilePath.EditValue Is Nothing Then Return False
			Dim paymentNumberOffsetFromSettings As Integer = ReadPaymentOffsetFromSettings()

			' loads all accounts
			fileinfo = New System.IO.FileInfo(txtFilePath.Text)

			Dim esrDBRecord As New SP.DatabaseAccess.ESRUtility.DataObjects.EsrRecord

			For Each esrItem In m_CurrentCamt054Data
				Dim reData = LoadInoviceData(esrItem.EsrInvoiceNo)

				bookingCount += 1
				bookingAmountSum += esrItem.EsrAmount

				esrDBRecord.customerNumber = esrItem.EsrCustomerNo
				esrDBRecord.invoiceNumber = esrItem.EsrInvoiceNo
				esrDBRecord.valutadate = esrItem.EsrValutaDate

				esrDBRecord.amount = esrItem.EsrAmount
				esrDBRecord.bookingamountsum = bookingAmountSum

				esrDBRecord.data = esrItem.EsrData
				esrDBRecord.bookingcount = bookingCount
				esrDBRecord.amountDecision = "?"

				esrDBRecord.fileinfo = fileinfo
				m_DiskIdentity = esrDBRecord.dikey ' m_Camt054Data.GrpHdr.MsgId ' 
				esrDBRecord.createdfrom = m_InitializationData.UserData.UserFullName

				If reData Is Nothing Then
					' Rechnung ist nicht vorhanden -> keine Buchung
					esrDBRecord.amount = Nothing
					esrDBRecord.PayedAmount = esrItem.EsrAmount.GetValueOrDefault(0)
					m_CurrentCamt054Data(bookingCount - 1).amountDecision = PaymentProcessState.Failed

				Else
					' Buchung
					Dim amountOpen = reData.BetragInk - reData.Bezahlt
					Dim amountToBook = Math.Min(esrItem.EsrAmount.GetValueOrDefault(0), amountOpen)

					m_CurrentCamt054Data(bookingCount - 1).amountDecision = PaymentProcessState.Question
					esrItem.amountDecision = PaymentProcessState.Question

					If amountToBook > 0D Then
						esrDBRecord.PayedAmount = esrItem.EsrAmount.GetValueOrDefault(0)
						esrDBRecord.amount = amountToBook
						esrDBRecord.bkonto = Val(m_postingAccounts(7))
						esrDBRecord.fksoll = reData.FkSoll
						esrDBRecord.fak_date = reData.Fak_Dat
						esrDBRecord.iswithtax = If(reData.MWST1 > 0, False, True)
						esrDBRecord.currency = reData.Currency
						esrDBRecord.isinvoicefinished = (esrItem.EsrAmount.GetValueOrDefault(0) = amountOpen)

						If esrItem.EsrAmount.GetValueOrDefault(0) > amountOpen Then
							amountDecision = ">"
							esrItem.amountDecision = PaymentProcessState.Higher
							m_CurrentCamt054Data(bookingCount - 1).amountDecision = PaymentProcessState.Higher

						ElseIf esrItem.EsrAmount.GetValueOrDefault(0) < amountOpen Then
							amountDecision = "<"
							esrItem.amountDecision = PaymentProcessState.Lower
							m_CurrentCamt054Data(bookingCount - 1).amountDecision = PaymentProcessState.Lower

						ElseIf esrItem.EsrAmount.GetValueOrDefault(0) = amountOpen Then
							amountDecision = "OK"
							esrItem.amountDecision = PaymentProcessState.Processed
							m_CurrentCamt054Data(bookingCount - 1).amountDecision = PaymentProcessState.Processed

						End If
						esrDBRecord.amountDecision = amountDecision

						success = m_ESRUtilityDatabaseAccess.AddESRDataToPayment(lueMandant.EditValue, esrDBRecord, paymentNumberOffsetFromSettings)
						If success Then
							If m_FirstpaymentNumber = 0 Then m_FirstpaymentNumber = esrDBRecord.paymentNumber
						Else
							Dim msg = "Ihre Zahlung wurde nicht erfolgreich gebucht.{0}Rechnung-Nr.: {1}{0}Betrag:{2}{0}Kunden-Nr.: {3}"
							msg = String.Format(msg, vbNewLine, esrItem.EsrInvoiceNo, esrItem.EsrAmount.GetValueOrDefault(0), esrItem.EsrCustomerNo)
							m_UtilityUI.ShowErrorDialog(msg)

							'esrItem.amountDecision = PaymentProcessState.Processed
							m_CurrentCamt054Data(bookingCount).amountDecision = PaymentProcessState.Failed
							esrItem.amountDecision = PaymentProcessState.Failed

							Exit For
						End If

					Else
						esrDBRecord.amount = Nothing
						esrDBRecord.PayedAmount = esrItem.EsrAmount.GetValueOrDefault(0)
						esrDBRecord.fksoll = 0
						esrDBRecord.fak_date = Nothing
						esrItem.amountDecision = PaymentProcessState.Question
						m_CurrentCamt054Data(bookingCount - 1).amountDecision = PaymentProcessState.Question

					End If

				End If

				m_ESRUtilityDatabaseAccess.AddESRDataToESRTable(lueMandant.EditValue, esrDBRecord)

			Next
			grdChoosenEmployees.RefreshDataSource()
			If (bookingCount > 0) Then bbiESRPrint.Enabled = True

			If success AndAlso (bookingCount > 0) Then
				success = m_ESRUtilityDatabaseAccess.AddESRFileDataToDiskInfo(m_InitializationData.MDData.MDNr, esrDBRecord, m_CurrentFileBytes)


				Dim message As String = "Die Datei {0}<br>mit {1} Zahlung{2} und Gesamtbetrag von {3:n2} wurde erfolgreich eingelesen."
				message = String.Format(m_Translate.GetSafeTranslationValue(message),
																		fileinfo.FullName, bookingCount,
																		If(bookingCount > 1, "en", String.Empty), bookingAmountSum)
				m_UtilityUI.ShowInfoDialog(Me, message)

				Dim archivedirectory = Path.Combine(fileinfo.DirectoryName, "Archive")
				Dim archivefile = Path.Combine(archivedirectory, fileinfo.Name)

				System.IO.Directory.CreateDirectory(archivedirectory)

				Dim movingResult As Boolean = True
				Try
					If success AndAlso movingResult Then
						'Try
						If File.Exists(archivefile) Then
								archivefile = Path.Combine(archivedirectory, String.Format("{0}_{1}", Path.GetFileNameWithoutExtension(fileinfo.Name), Path.GetRandomFileName))
								archivefile = Path.ChangeExtension(archivefile, fileinfo.Extension)
							End If

						'Catch ex As Exception
						'	m_Logger.LogError(String.Format("error during deleting file: {0} >>> {1}", archivefile, ex.ToString))
						'	movingResult = False
						'End Try

						If movingResult Then fileinfo.MoveTo(archivefile)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("error during moving file: {0} to: {1} >>> {2}", fileinfo.FullName, archivefile, ex.ToString))
					movingResult = False
				End Try


				Dim messageMoving As String
				If movingResult Then
					messageMoving = "Ihre Datei ist in <br>{0}<br>archiviert."
				Else
					messageMoving = "Ihre Datei <b>konnte nicht</b> in <br>{0}<br>archiviert werden."
				End If

				messageMoving = String.Format(m_Translate.GetSafeTranslationValue(messageMoving), archivedirectory)
				m_UtilityUI.ShowOKDialog(Me, messageMoving, m_Translate.GetSafeTranslationValue("Archivierung"), If(movingResult, MessageBoxIcon.Information, MessageBoxIcon.Error))

				bbiZEPrint.Enabled = m_FirstpaymentNumber.GetValueOrDefault(0) > 0
				If m_FirstpaymentNumber.GetValueOrDefault(0) > 0 Then PrintESRList(txtFilePath.EditValue, Now, "6.4")

			Else
				m_UtilityUI.ShowErrorDialog("Es sind keine Buchungen erforderlich.")
			End If


			Return success

		End Function

		Private Function BuildV11File() As Boolean
			Dim success As Boolean = True
			Dim bookingCount As Integer = 0
			Dim bookingAmountSum As Decimal = 0D
			Dim fileinfo As System.IO.FileInfo
			Dim amountDecision As String = "OK"
			If txtFilePath.EditValue Is Nothing Then Return False
			Dim paymentNumberOffsetFromSettings As Integer = ReadPaymentOffsetFromSettings()

			' loads all accounts
			fileinfo = New System.IO.FileInfo(txtFilePath.Text)

			Dim esrDBRecord As New SP.DatabaseAccess.ESRUtility.DataObjects.EsrRecord

			If m_esrDataList Is Nothing Then Return False
			For Each esrItem As EsrItem In m_esrDataList
				Dim reData = esrItem.ReData
				Dim esrRecord = esrItem.EsrRecord

				bookingCount += 1
				bookingAmountSum += esrRecord.Amount

				esrDBRecord.customerNumber = esrItem.EsrCustomerNo
				esrDBRecord.invoiceNumber = esrItem.EsrInvoiceNo
				esrDBRecord.valutadate = esrItem.EsrValutaDate

				esrDBRecord.amount = esrRecord.Amount
				esrDBRecord.bookingamountsum = bookingAmountSum

				esrDBRecord.data = esrRecord.Data
				esrDBRecord.bookingcount = bookingCount
				esrDBRecord.amountDecision = "?"

				esrDBRecord.fileinfo = fileinfo
				m_DiskIdentity = esrDBRecord.dikey
				esrDBRecord.createdfrom = m_InitializationData.UserData.UserFullName

				If reData Is Nothing Then
					' Rechnung ist nicht vorhanden -> keine Buchung
					esrDBRecord.amount = Nothing
					esrDBRecord.PayedAmount = esrRecord.Amount
					m_esrDataList(bookingCount - 1).amountDecision = PaymentProcessState.Failed

				Else

					Select Case (esrRecord.BookingType)
						Case ESRUtility.EsrRecord.BookingTypeEnum.Storno
							amountDecision = "-"
						Case ESRUtility.EsrRecord.BookingTypeEnum.Correction
							amountDecision = "+"
						Case Else
							' Buchung
							Dim amountOpen = reData.BetragInk - reData.Bezahlt
							Dim amountToBook = Math.Min(esrRecord.Amount, amountOpen)

							m_esrDataList(bookingCount - 1).amountDecision = PaymentProcessState.Question
							If amountToBook > 0D Then
								esrDBRecord.amount = amountToBook
								esrDBRecord.PayedAmount = esrRecord.Amount
								esrDBRecord.bkonto = Val(m_postingAccounts(7))
								esrDBRecord.fksoll = reData.FkSoll
								esrDBRecord.fak_date = reData.Fak_Dat
								esrDBRecord.iswithtax = If(reData.MWST1 > 0, False, True)
								esrDBRecord.currency = reData.Currency
								esrDBRecord.isinvoicefinished = (esrRecord.Amount = amountOpen)

								If esrRecord.Amount > amountOpen Then
									amountDecision = ">"
									esrItem.amountDecision = PaymentProcessState.Higher
									m_esrDataList(bookingCount - 1).amountDecision = PaymentProcessState.Higher

								ElseIf esrRecord.Amount < amountOpen Then
									amountDecision = "<"
									esrItem.amountDecision = PaymentProcessState.Lower
									m_esrDataList(bookingCount - 1).amountDecision = PaymentProcessState.Lower

								ElseIf esrRecord.Amount = amountOpen Then
									amountDecision = "OK"
									esrItem.amountDecision = PaymentProcessState.Processed
									m_esrDataList(bookingCount - 1).amountDecision = PaymentProcessState.Processed

								End If
								esrDBRecord.amountDecision = amountDecision

#If Not DEBUG Then
								success = m_ESRUtilityDatabaseAccess.AddESRDataToPayment(lueMandant.EditValue, esrDBRecord, paymentNumberOffsetFromSettings)
#End If
								If success Then
									If m_FirstpaymentNumber = 0 Then m_FirstpaymentNumber = esrDBRecord.paymentNumber
								Else
									Dim msg = "Ihre Zahlung wurde nicht erfolgreich gebucht.{0}Rechnung-Nr.: {1}{0}Betrag:{2}{0}Kunden-Nr.: {3}"
									msg = String.Format(msg, vbNewLine, esrItem.EsrInvoiceNo, esrRecord.Amount, esrItem.EsrCustomerNo)
									m_UtilityUI.ShowErrorDialog(msg)

									esrItem.amountDecision = PaymentProcessState.Processed
									m_esrDataList(bookingCount).amountDecision = PaymentProcessState.Failed

									Exit For
								End If

							Else
								esrDBRecord.amount = Nothing
								esrDBRecord.PayedAmount = esrRecord.Amount
								esrDBRecord.fksoll = 0
								esrDBRecord.fak_date = Nothing
								esrItem.amountDecision = PaymentProcessState.Question
								'm_CurrentCamt054Data(bookingCount - 1).amountDecision = PaymentProcessState.Question

							End If

					End Select

				End If

#If Not DEBUG Then
				m_ESRUtilityDatabaseAccess.AddESRDataToESRTable(lueMandant.EditValue, esrDBRecord)
#End If

			Next
			grdChoosenEmployees.RefreshDataSource()

			If (bookingCount > 0) Then
				bbiESRPrint.Enabled = True
			End If

			If success AndAlso (bookingCount > 0) Then
#If Not DEBUG Then
				success = m_ESRUtilityDatabaseAccess.AddESRFileDataToDiskInfo(m_InitializationData.MDData.MDNr, esrDBRecord, m_CurrentFileBytes)
#End If

				Dim message As String = "Die Datei {0}<br>mit {1} Zahlung{2} und Gesamtbetrag von {3:n2} wurde erfolgreich eingelesen."
				message = String.Format(m_Translate.GetSafeTranslationValue(message),
																		fileinfo.FullName, bookingCount,
																		If(bookingCount > 1, "en", String.Empty), bookingAmountSum)
				m_UtilityUI.ShowInfoDialog(Me, message)

				Dim archivedirectory = Path.Combine(fileinfo.DirectoryName, "Archive") 'String.Format("{0}\Archive\{1}", fileinfo.DirectoryName)
				Dim archivefile = Path.Combine(archivedirectory, fileinfo.Name)

				System.IO.Directory.CreateDirectory(archivedirectory)

				Dim movingResult As Boolean = True
				Try
					If success AndAlso movingResult Then
						If File.Exists(archivefile) Then
							archivefile = Path.Combine(archivedirectory, String.Format("{0}_{1}", Path.GetFileNameWithoutExtension(fileinfo.Name), Path.GetRandomFileName))
							archivefile = Path.ChangeExtension(archivefile, fileinfo.Extension)
						End If

						If movingResult Then fileinfo.MoveTo(archivefile)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("error during moving file: {0} to: {1} >>> {2}", fileinfo.FullName, archivefile, ex.ToString))
					movingResult = False
				End Try


				Dim messageMoving As String
				If movingResult Then
					messageMoving = "Ihre Datei ist in <br>{0}<br>archiviert."
				Else
					messageMoving = "Ihre Datei <b>konnte nicht</b> in <br>{0}<br>archiviert werden."
				End If

				messageMoving = String.Format(m_Translate.GetSafeTranslationValue(messageMoving), archivedirectory)
				m_UtilityUI.ShowOKDialog(Me, messageMoving, m_Translate.GetSafeTranslationValue("Archivierung"), If(movingResult, MessageBoxIcon.Information, MessageBoxIcon.Error))

				bbiZEPrint.Enabled = m_FirstpaymentNumber.GetValueOrDefault(0) > 0
				If m_FirstpaymentNumber.GetValueOrDefault(0) > 0 Then PrintESRList(txtFilePath.EditValue, Now, "6.4")

			Else
				m_UtilityUI.ShowErrorDialog("Es sind keine Buchungen erforderlich.")
			End If


			Return success

		End Function

		Private Sub OnbbiESRPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiESRPrint.ItemClick
			PrintESRList(txtFilePath.EditValue, Now, "6.3")
		End Sub

		Private Sub OnbtnPrintAvailableGrid_Click(sender As Object, e As EventArgs) Handles btnPrintAvailableGrid.Click

			If gvChoosenEmployees.RowCount > 0 Then
				' Opens the Preview window. 
				grdChoosenEmployees.ShowPrintPreview()
			End If

		End Sub

		Private Sub OnbbiZEPrint_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiZEPrint.ItemClick
			PrintESRList(txtFilePath.EditValue, Now, "6.4")
		End Sub

		Private Function PrintESRList(ByVal ESRFilename As String, ByVal ESRFileDate As DateTime?, ByVal printJobKey As String) As Boolean
			Dim success As Boolean = True
			If m_FirstpaymentNumber.GetValueOrDefault(0) = 0 AndAlso String.IsNullOrWhiteSpace(m_DiskIdentity) Then Return False
			Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Try
				Dim bankIDNumber As Integer?
				If Not m_bankDataSelected Is Nothing Then bankIDNumber = m_bankDataSelected.ID

				Dim _Setting As New ClsLLESRPrintSetting With {.ShowAsDesgin = ShowDesign,
					.frmhwnd = GetHwnd,
					.firstPaymentNumber = New Integer() {m_FirstpaymentNumber.GetValueOrDefault(0)},
					.ESRfiledate = ESRFileDate,
					.ESRFileName = ESRFilename,
					.DiskIdentity = m_DiskIdentity,
					.ESRKontoNumber = m_bankDataSelected.KontoESR1,
					.MandantBankIDNumber = bankIDNumber,
					.ListFilterBez = New List(Of String)(New String() {String.Format("{0}", String.Empty)})
				}

				'Dim o2Open As New ZESearchListing.ClsPrintZESearchList(New ClsLLZESearchPrintSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr,
				'													   .SelectedMDYear = m_InitializationData.MDData.MDYear,
				'													   .DbConnString2Open = m_InitializationData.MDData.MDDbConn,
				'													   .JobNr2Print = printJobKey, .frmhwnd = GetHwnd})
				Dim printObj As New ClsLLESRPrint(m_InitializationData)
				printObj.PrintSetting = _Setting

				'(New ClsLLZESearchPrintSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr,
				'													   .SelectedMDYear = m_InitializationData.MDData.MDYear,
				'													   .DbConnString2Open = m_InitializationData.MDData.MDDbConn,
				'													   .JobNr2Print = printJobKey, .frmhwnd = GetHwnd})
				Dim printResult As New PrintResult With {.Printresult = True}

				If printJobKey = "6.3" Then
					printResult = printObj.PrintESRList()

				ElseIf printJobKey = "6.4" Then
					If m_FirstpaymentNumber.GetValueOrDefault(0) > 0 Then
						printResult = printObj.PrintESRPaymentList()

						If Not ShowDesign AndAlso printResult.Printresult Then
							_Setting.JobNr2Print = "6.3"
							printResult = printObj.PrintESRList()
						End If
					End If

				End If
				success = success AndAlso printResult.Printresult
				If Not success Then
					m_UtilityUI.ShowErrorDialog(printResult.PrintresultMessage)
				End If


			Catch ex As Exception
				Return False

			End Try

			Return success

		End Function


		Private Sub OnBtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
			Me.Close()
		End Sub

#End Region  ' Event Handlers

#Region "Helper Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Private Sub TranslateControls()

			Me.lblHeader.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader.Text)
			Me.lblHeaderDescription.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderDescription.Text)
			Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)

			Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
			Me.chkUse7Digits.Text = m_Translate.GetSafeTranslationValue(Me.chkUse7Digits.Text)
			Me.lblFilePath.Text = m_Translate.GetSafeTranslationValue(Me.lblFilePath.Text)
			Me.lblYear.Text = m_Translate.GetSafeTranslationValue(Me.lblYear.Text)
			Me.lblBankData.Text = m_Translate.GetSafeTranslationValue(Me.lblBankData.Text)
			Me.lblFileContent.Text = m_Translate.GetSafeTranslationValue(Me.lblFileContent.Text)

			Me.bbiAction.Caption = m_Translate.GetSafeTranslationValue(Me.bbiAction.Caption)
			Me.bbiESRPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiESRPrint.Caption)
			bbiZEPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiZEPrint.Caption)

		End Sub

		''' <summary>
		''' Creates database access objects by mandant number.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		Private Sub CreateDatabaseAccessObjects(mdNr As Integer?)
			If mdNr.HasValue Then
				Dim connectionString = m_mandant.GetSelectedMDData(mdNr.Value).MDDbConn
				If m_CurrentConnectionString <> connectionString Then
					m_ESRUtilityDatabaseAccess = New SP.DatabaseAccess.ESRUtility.ESRUtilityDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
					m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
					m_CurrentConnectionString = connectionString
				End If
			End If
		End Sub

		Private Function ValidateESRFile(ByVal filename) As Boolean
			Dim success As Boolean = Not String.IsNullOrWhiteSpace(filename)

			If success Then
				txtFilePath.EditValue = filename
				success = success AndAlso LoadEsrFileContentGridData()

				'bbiAction.Enabled = success
			End If

			Return success

		End Function

		''' <summary>
		''' Loads form settings.
		''' </summary>
		Private Sub LoadFormSettings()

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

				If Not String.IsNullOrEmpty(setting_form_location) Then
					Dim aLoc As String() = setting_form_location.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
				End If
				chkCamt054.Checked = m_SettingsManager.ReadBoolean(SettingKeys.SETTING_ESRFile_CAMT054)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Saves the form settings.
		''' </summary>
		Private Sub SaveFromSettings()

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)
					m_SettingsManager.WriteBoolean(SettingKeys.SETTING_ESRFile_CAMT054, chkCamt054.Checked)

					m_SettingsManager.WriteString(SettingKeys.SETTING_ESRFile_LOCATION, Me.txtFilePath.EditValue)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Sets the valid state of a control.
		''' </summary>
		''' <param name="control">The control to validate.</param>
		''' <param name="errorProvider">The error providor.</param>
		''' <param name="invalid">Boolean flag if data is invalid.</param>
		''' <param name="errorText">The error text.</param>
		''' <returns>Valid flag</returns>
		Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText)
			Else
				errorProvider.SetError(control, String.Empty)
			End If

			Return Not invalid

		End Function

		''' <summary>
		''' Cleanup and close form.
		''' </summary>
		Public Sub CleanupAndHideForm()

			SaveFromSettings()

			Me.Hide()
			Me.Reset() 'Clear all data.

		End Sub

		Private Function LoadInoviceData(ByVal invoiceNumber As Integer?) As ReData
			Dim data As ReData = Nothing

			If invoiceNumber.GetValueOrDefault(0) > 0 Then
				data = m_ESRUtilityDatabaseAccess.LoadReData(m_InitializationData.MDData.MDNr, Nothing, invoiceNumber)

			End If
			Return data

		End Function

		Private Function InvoiceStatus(ByVal esrAmount As Double?, ByVal invoiceOpenAmount As Double?) As String
			Dim msg As String = String.Empty

			If invoiceOpenAmount.GetValueOrDefault(0) = 0 AndAlso esrAmount.GetValueOrDefault(0) = 0 Then
				msg = m_Translate.GetSafeTranslationValue("Rechnung nicht vorhanden")
			Else
				Dim amountOpen = invoiceOpenAmount.GetValueOrDefault(0)
				If esrAmount > amountOpen Then
					msg = m_Translate.GetSafeTranslationValue("Zahlung zu hoch")
				ElseIf esrAmount < amountOpen Then
					msg = m_Translate.GetSafeTranslationValue("Zahlung zu tief")
				Else
					msg = m_Translate.GetSafeTranslationValue("Zahlung korrekt")
				End If
			End If
			Return msg

		End Function


#End Region  ' Helper Methods

#End Region  ' Private Methods



#Region "Private Helper Classes"

		Sub OpenSelectedCustomer(ByVal customerNumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customerNumber)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Sub OpenSelectedInvoice(ByVal invoiceNumber As Integer)

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, invoiceNumber)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Private Sub txtFilePath_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtFilePath.SelectedIndexChanged

		End Sub


#End Region 'Private Helper Classes



	End Class








	' Test...

	''' <summary>
	''' Item Klasse für ESR-Liste.
	''' </summary>
	''' <remarks></remarks>
	Public Class EsrItem


		Private m_esrUtilityDatabaseAccess As IESRUtilityDatabaseAccess
		Private m_esrRecord As EsrRecord
		Private m_reData As SP.DatabaseAccess.ESRUtility.DataObjects.ReData
		Private m_reDataDefined As Boolean

		Public Enum PaymentProcessState
			Unprocessed
			InProcessing
			Processed
			Question
			Failed
			Lower
			Higher
		End Enum
		Public Property amountDecision As PaymentProcessState



		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


		Public Sub New(ByRef esrUtilityDatabaseAccess As IESRUtilityDatabaseAccess, ByRef esrRecord As EsrRecord, ByVal initData As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = initData
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(initData.TranslationData, initData.ProsonalizedData)

			m_esrUtilityDatabaseAccess = esrUtilityDatabaseAccess
			m_esrRecord = esrRecord

			amountDecision = PaymentProcessState.Unprocessed

		End Sub

		Public ReadOnly Property EsrRecord As EsrRecord
			Get
				Return m_esrRecord
			End Get
		End Property

		Public ReadOnly Property ReData As SP.DatabaseAccess.ESRUtility.DataObjects.ReData
			Get
				If Not m_reDataDefined Then
					m_reData = m_esrUtilityDatabaseAccess.LoadReData(m_InitializationData.MDData.MDNr, m_esrRecord.CustomerNo, m_esrRecord.InvoiceNo)
					m_reDataDefined = True
				End If
				Return m_reData
			End Get
		End Property

		Public ReadOnly Property EsrInvoiceNo As Integer
			Get
				Return m_esrRecord.InvoiceNo
			End Get
		End Property

		Public ReadOnly Property EsrCustomerNo As Integer
			Get
				Return m_esrRecord.CustomerNo
			End Get
		End Property

		Public ReadOnly Property EsrAmount As Decimal
			Get
				Return m_esrRecord.Amount
			End Get
		End Property

		Public ReadOnly Property EsrValutaDate As Date?
			Get
				Return m_esrRecord.ValutaDate
			End Get
		End Property

		Public ReadOnly Property EsrData As String
			Get
				Return m_esrRecord.Data.Trim()
			End Get
		End Property

		Public ReadOnly Property InvoiceAmountOpen As String
			Get
				Dim reData = Me.ReData
				Return If(reData IsNot Nothing, (reData.BetragInk - reData.Bezahlt).ToString("n2"), String.Empty)
			End Get
		End Property

		Public ReadOnly Property InvoiceAmountOpenToolTip As String
			Get
				Dim reData = Me.ReData
				Dim msg As String = String.Empty
				If reData Is Nothing Then
					msg = m_Translate.GetSafeTranslationValue("Rechnung nicht vorhanden")
					Return msg
				Else
					msg = String.Format(m_Translate.GetSafeTranslationValue("Rechnungsbetrag: {1} {2:n2}{0}Bisher bezahlt: {1} {3:n2}"), vbNewLine, reData.Currency, reData.BetragInk, reData.Bezahlt)
					'& String.Format("Bisher bezahlt: {0} {1:n2}", reData.Currency, reData.Bezahlt)
				End If
				Return msg

			End Get
		End Property

		Public ReadOnly Property Status As String
			Get
				Dim reData = Me.ReData
				Dim msg As String = String.Empty
				If reData Is Nothing Then
					msg = m_Translate.GetSafeTranslationValue("Rechnung nicht vorhanden")
				Else
					Select Case (m_esrRecord.BookingType)
						Case ESRUtility.EsrRecord.BookingTypeEnum.Storno
							msg = m_Translate.GetSafeTranslationValue("Stornobuchung")
						Case ESRUtility.EsrRecord.BookingTypeEnum.Correction
							msg = m_Translate.GetSafeTranslationValue("Korrekturbuchung")
						Case Else
							Dim amountOpen = m_reData.BetragInk - m_reData.Bezahlt
							If m_esrRecord.Amount > amountOpen Then
								msg = m_Translate.GetSafeTranslationValue("Zahlung zu hoch")
							ElseIf m_esrRecord.Amount < amountOpen Then
								msg = m_Translate.GetSafeTranslationValue("Zahlung zu tief")
							Else
								msg = m_Translate.GetSafeTranslationValue("Zahlung korrekt")
							End If
					End Select
				End If
				Return msg

			End Get
		End Property


	End Class

	''' <summary>
	''' Item Klasse für ESR-Liste.
	''' </summary>
	''' <remarks></remarks>
	Public Class EsrSummaryItem

		Public Sub New(ByRef esrRecord As EsrRecord)
			m_esrRecord = esrRecord
		End Sub

		Public ReadOnly Property EsrRecord As EsrRecord
			Get
				Return m_esrRecord
			End Get
		End Property

		Public ReadOnly Property EsrAmount As Decimal
			Get
				Return m_esrRecord.Amount
			End Get
		End Property

		Public ReadOnly Property EsrData As String
			Get
				Return m_esrRecord.Data.Trim()
			End Get
		End Property

		Private m_esrRecord As EsrRecord

	End Class


End Namespace

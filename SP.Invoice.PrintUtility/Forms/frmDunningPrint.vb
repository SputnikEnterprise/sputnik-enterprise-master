

Imports System.Reflection.Assembly
Imports System.IO

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports SP.DatabaseAccess.Invoice


Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports DevExpress.XtraBars
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.Pdf
Imports SPProgUtility.CommonXmlUtility
Imports System.Threading



Public Class frmDunningPrint
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' The invoice data access object.
	''' </summary>
	Private m_DunningPrintDatabaseAccess As IInvoiceDatabaseAccess

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private bAllowedtowrite As Boolean

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	Private m_CustomerWOSID As String

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_CurrentConnectionString As String

	Private m_SearchData As PayrollSearchData

	Private m_SelectedPayroll As List(Of PayrollPrintData)

	Private m_SelectedWOSEnun As WOSSENDValue

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False

#End Region


#Region "private consts"

	Private Const MODULNAME_FOR_DELETE As String = "RE"
	Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"

#End Region

#Region "private property"

	Private Property WOSProperty4Search As WOSSearchValue


	Private ReadOnly Property CustomerWOSID() As String
		Get
			Dim value = m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_WOS_CUSTOMER_GUID, m_InitializationData.MDData.MDNr))

			Return value
		End Get
	End Property


#End Region


#Region "public property"
	''' <summary>
	''' Gets or sets the preselection data.
	''' </summary>
	Public Property PreselectionData As PreselectionDunningData

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		m_CurrentConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
		m_DunningPrintDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		m_CustomerWOSID = CustomerWOSID

		TranslateControls()
		Reset()

		AddHandler lueDunningDate.ButtonClick, AddressOf OnDropDownButtonClick

	End Sub

#End Region


	''' <summary>
	''' Gets the selected payroll.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedRecord As DunningPrintData
		Get
			Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim invoice = CType(gvRP.GetRow(selectedRows(0)), DunningPrintData)

					Return invoice
				End If

			End If

			Return Nothing
		End Get

	End Property


#Region "public methodes"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso LoadMandantenDropDown()
		lueMandant.EditValue = m_InitializationData.MDData.MDNr

		success = success AndAlso LoadDunningLevelDropDown()
		PreselectData()


		Return success

	End Function

	''' <summary>
	''' Preselects data.
	''' </summary>
	Private Sub PreselectData()

		Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

		If hasPreselectionData Then

			Dim supressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

			' ---Mandant---
			If Not lueMandant.Properties.DataSource Is Nothing Then

				Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

				If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then

					' Mandant is required
					lueMandant.EditValue = PreselectionData.MDNr

				Else
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
					m_SuppressUIEvents = supressUIEventState
					Return
				End If

			End If

			If Not PreselectionData.DunningLevel Is Nothing Then
				m_SuppressUIEvents = True
				lueDunningLevel.EditValue = PreselectionData.DunningLevel
				LoadDunningdateDropDown()

				If Not PreselectionData.DunningDate Is Nothing Then
					lueDunningDate.EditValue = PreselectionData.DunningDate
				End If
				SearchData()
			End If

			m_SuppressUIEvents = supressUIEventState
		Else
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


#Region "private mehthodes"

	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
		gpSuchKriterien.Text = m_Translate.GetSafeTranslationValue(gpSuchKriterien.Text)

		lblDetail.Text = m_Translate.GetSafeTranslationValue(lblDetail.Text)
		tgsSelection.Properties.OffText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OffText)
		tgsSelection.Properties.OnText = m_Translate.GetSafeTranslationValue(tgsSelection.Properties.OnText)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		lblMahnstufe.Text = m_Translate.GetSafeTranslationValue(lblMahnstufe.Text)
		lblMahnDatum.Text = m_Translate.GetSafeTranslationValue(lblMahnDatum.Text)
		chk_PrintInvoices.Text = m_Translate.GetSafeTranslationValue(chk_PrintInvoices.Text)

		Me.bsiPrintinfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiPrintinfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)

	End Sub


	Private Sub Reset()

		ResetMandantenDropDown()
		ResetInvoiceGrid()
		ResetDunningLevelDropDown()
		ResetDunningDateDropDown()

		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 551) Then Me.bbiDelete.Visibility = BarItemVisibility.Never
		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 554) Then
			Me.bbiPrint.Visibility = BarItemVisibility.Never
			Me.bbiExport.Visibility = BarItemVisibility.Never
		End If

		LockControls(False)

	End Sub

	Private Sub ResetMandantChanged()

		ResetInvoiceGrid()
		ResetDunningLevelDropDown()
		ResetDunningDateDropDown()

		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 551) Then Me.bbiDelete.Visibility = BarItemVisibility.Never
		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 554) Then
			Me.bbiPrint.Visibility = BarItemVisibility.Never
			Me.bbiExport.Visibility = BarItemVisibility.Never
		End If

		LockControls(False)

	End Sub

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the dunning level drop down.
	''' </summary>
	Private Sub ResetDunningLevelDropDown()

		lueDunningLevel.Properties.DisplayMember = "Label"
		lueDunningLevel.Properties.ValueMember = "Value"

		lueDunningLevel.Properties.Columns.Clear()
		'lueDunningLevel.Properties.Columns.Add(New LookUpColumnInfo("Value", 0))
		lueDunningLevel.Properties.Columns.Add(New LookUpColumnInfo("Display", 0))

		lueDunningLevel.EditValue = Nothing

	End Sub


	''' <summary>
	''' Resets the dunning date drop down.
	''' </summary>
	Private Sub ResetDunningDateDropDown()

		lueDunningDate.Properties.DisplayMember = "DunningDate"
		lueDunningDate.Properties.ValueMember = "DunningDate"

		lueDunningDate.Properties.Columns.Clear()
		lueDunningDate.Properties.Columns.Add(New LookUpColumnInfo("DunningDate", "", 0, DevExpress.Utils.FormatType.DateTime, "d", True, DevExpress.Utils.HorzAlignment.Default))
		lueDunningDate.Properties.Columns.Add(New LookUpColumnInfo("DunningCount", 0))

		lueDunningLevel.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets dunning grid.
	''' </summary>
	Private Sub ResetInvoiceGrid()

		gvPrint.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPrint.OptionsView.ShowIndicator = False
		gvPrint.OptionsBehavior.Editable = True
		gvPrint.OptionsView.ShowAutoFilterRow = True
		gvPrint.OptionsView.ColumnAutoWidth = True
		gvPrint.OptionsView.ShowFooter = True
		gvPrint.OptionsView.AllowHtmlDrawGroups = True

		gvPrint.Columns.Clear()


		Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
		columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnIsSelected.OptionsColumn.AllowEdit = True
		columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
		columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnIsSelected.Name = "IsSelected"
		columnIsSelected.FieldName = "IsSelected"
		columnIsSelected.Visible = True
		columnIsSelected.Width = 50
		gvPrint.Columns.Add(columnIsSelected)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKDNr.OptionsColumn.AllowEdit = False
		columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Width = 60
		columnKDNr.Visible = False
		gvPrint.Columns.Add(columnKDNr)

		Dim columnRName1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRName1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRName1.OptionsColumn.AllowEdit = False
		columnRName1.Caption = m_Translate.GetSafeTranslationValue("Rechnungsempfänger")
		columnRName1.Name = "RName1"
		columnRName1.FieldName = "RName1"
		columnRName1.Visible = True
		columnRName1.Width = 80
		gvPrint.Columns.Add(columnRName1)

		Dim columnRAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRAddress.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRAddress.OptionsColumn.AllowEdit = False
		columnRAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
		columnRAddress.Name = "RAddress"
		columnRAddress.FieldName = "RAddress"
		columnRAddress.Visible = True
		columnRAddress.Width = 100
		gvPrint.Columns.Add(columnRAddress)

		Dim columnCustomerLanguage As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerLanguage.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerLanguage.OptionsColumn.AllowEdit = False
		columnCustomerLanguage.Caption = m_Translate.GetSafeTranslationValue("Sprache")
		columnCustomerLanguage.Name = "CustomerLanguage"
		columnCustomerLanguage.FieldName = "CustomerLanguage"
		columnCustomerLanguage.Visible = False
		columnCustomerLanguage.Width = 50
		gvPrint.Columns.Add(columnCustomerLanguage)

		Dim columnBetragTotal As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBetragTotal.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBetragTotal.OptionsColumn.AllowEdit = False
		columnBetragTotal.Caption = m_Translate.GetSafeTranslationValue("Betrag ink. MwSt.")
		columnBetragTotal.Name = "BetragTotal"
		columnBetragTotal.FieldName = "BetragTotal"
		columnBetragTotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBetragTotal.AppearanceHeader.Options.UseTextOptions = True
		columnBetragTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBetragTotal.DisplayFormat.FormatString = "N2"
		columnBetragTotal.Width = 60
		columnBetragTotal.Visible = True
		columnBetragTotal.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnBetragTotal.SummaryItem.DisplayFormat = "{0:n2}"
		gvPrint.Columns.Add(columnBetragTotal)


		LockControls(False)
		grdPrint.DataSource = Nothing

	End Sub


	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Function LoadMandantenDropDown() As Boolean
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()


		Return Not (Data Is Nothing)

	End Function

	''' <summary>
	''' Loads the dunning level drop down data.
	''' </summary>
	Private Function LoadDunningLevelDropDown() As Boolean

		Dim data = New List(Of DunningLevel) From {
			New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("Kontoauszug"), .Value = 0},
			New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("1. Mahnstufe"), .Value = 1},
			New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("2. Mahnstufe"), .Value = 2},
			New DunningLevel With {.Display = m_Translate.GetSafeTranslationValue("3. Mahnstufe"), .Value = 3}
		 }

		lueDunningLevel.Properties.DataSource = data

		lueDunningLevel.Properties.ForceInitialize()

		Return Not data Is Nothing

	End Function

	''' <summary>
	''' Loads the Bankdaten drop down data.
	''' </summary>
	Private Function LoadDunningdateDropDown() As Boolean

		If lueDunningLevel.EditValue Is Nothing Then Return True
		Dim mandantNr = lueMandant.EditValue

		Dim data = m_DunningPrintDatabaseAccess.LoadDunningDateData(mandantNr, lueDunningLevel.EditValue)

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mahndatums konnten nicht geladen werden."))
		End If

		lueDunningDate.Properties.DataSource = data
		lueDunningDate.Properties.ForceInitialize()

		If Not data Is Nothing AndAlso data.Count > 0 Then lueDunningDate.EditValue = data(0).DunningDate


		Return data IsNot Nothing
	End Function


#End Region








	''' <summary>
	''' Handles change of mandant.
	''' </summary>
	Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueMandant.EditValue Is Nothing Then

			If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
				ResetMandantChanged()
				Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

				Dim clsMandant = m_mandant.GetSelectedMDData(lueMandant.EditValue)
				Dim logedUserData = m_mandant.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
				Dim personalizedData = m_InitializationData.ProsonalizedData
				Dim translate = m_InitializationData.TranslationData

				m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

				ChangeMandant(m_InitializationData.MDData.MDNr)
				lueDunningLevel.EditValue = 0

			End If

		End If

	End Sub


	''' <summary>
	''' Handles change of lueDunningLevel.
	''' </summary>
	Private Sub OnlueDunningLevel_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueDunningLevel.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueDunningLevel.EditValue Is Nothing Then
			ResetInvoiceGrid()
			LoadDunningdateDropDown()
			' not fire twice!
		Else
			LoadInvoicePrintList()

		End If

		SplashScreenManager.CloseForm(False)

	End Sub

	''' <summary>
	''' Handles change of lueDunningLevel.
	''' </summary>
	Private Sub OnlueDunningDate_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueDunningDate.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueDunningLevel.EditValue Is Nothing AndAlso Not lueDunningDate.EditValue Is Nothing Then
			LoadInvoicePrintList()

		End If
		SplashScreenManager.CloseForm(False)

	End Sub


#Region "Formhandle"

	Private Sub sbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbClose.Click
		Me.Dispose()
	End Sub

	Private Sub Onfrm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		SplashScreenManager.CloseForm(False)

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.printWithInvoices = chk_PrintInvoices.EditValue

			My.Settings.Save()
		End If

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub Onfrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			If My.Settings.iHeight > 0 Then Me.Height = My.Settings.iHeight
			If My.Settings.iWidth > 0 Then Me.Width = My.Settings.iWidth
			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If
			chk_PrintInvoices.EditValue = My.Settings.printWithInvoices

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.ToString))
		End Try

	End Sub

#End Region


	Private Function LoadInvoicePrintList() As Boolean

		SplashScreenManager.CloseForm(False)
		If lueDunningDate.EditValue Is Nothing OrElse lueDunningLevel.EditValue Is Nothing Then Return False
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim orderBy As OrderByValue
		orderBy = OrderByValue.OrderByInvoiceDate
		Dim wosValue As WOSSearchValue = WOSProperty4Search

		Dim listOfData = m_ListingDatabaseAccess.LoadDunningData(lueMandant.EditValue, lueDunningLevel.EditValue, lueDunningDate.EditValue, orderBy)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungen konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New DunningPrintData With {.MDNr = person.MDNr,
																											.KDNr = person.KDNr,
																											.CustomerLanguage = person.CustomerLanguage,
																											.RName1 = person.RName1,
																											.RStrasse = person.RStrasse,
																											.RPLZ = person.RPLZ,
																											.ROrt = person.ROrt,
																											.BetragTotal = person.BetragTotal,
																											.SPNr = person.SPNr,
																											.VerNr = person.VerNr,
																											.IsSelected = tgsSelection.EditValue
																										 }).ToList()

		Dim listDataSource As BindingList(Of DunningPrintData) = New BindingList(Of DunningPrintData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		bbiPrint.Enabled = tgsSelection.EditValue OrElse listDataSource.Count > 0
		bbiExport.Enabled = tgsSelection.EditValue OrElse listDataSource.Count > 0
		bbiDelete.Enabled = tgsSelection.EditValue OrElse listDataSource.Count > 0

		grdPrint.DataSource = listDataSource
		bsiPrintinfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Return Not listOfData Is Nothing

	End Function

	Private Sub OngvPrint_MasterRowGetRelationCount(ByVal sender As Object, ByVal e As MasterRowGetRelationCountEventArgs) Handles gvPrint.MasterRowGetRelationCount
		e.RelationCount = 1
	End Sub

	Private Sub OngvPrint_MasterRowGetRelationName(ByVal sender As Object, ByVal e As MasterRowGetRelationNameEventArgs) Handles gvPrint.MasterRowGetRelationName
		e.RelationName = "Gemahnte Rechnungen"
	End Sub

	Private Sub OngvPrint_MasterRowEmpty(ByVal sender As Object, ByVal e As MasterRowEmptyEventArgs) Handles gvPrint.MasterRowEmpty
		e.IsEmpty = False
	End Sub

	Private Sub OngvPrint_MasterRowGetChildList(ByVal sender As Object, ByVal e As MasterRowGetChildListEventArgs) Handles gvPrint.MasterRowGetChildList
		Dim dunningdata = SelectedRecord
		Dim listOfInvoiceData = m_ListingDatabaseAccess.LoadAssignedDunningDetailData(lueMandant.EditValue, dunningdata.KDNr, lueDunningLevel.EditValue, dunningdata.RName1, lueDunningDate.EditValue)

		Dim details As New BindingList(Of InvoiceDunningPrintViewData)
		e.ChildList = details
		Dim child As New InvoiceDunningPrintViewData
		For Each itm In listOfInvoiceData
			child = New InvoiceDunningPrintViewData

			child.ReNr = itm.ReNr
			child.BetragInk = itm.BetragInk
			child.FakDat = itm.FakDat
			child.Art = itm.Art
			child.KST = itm.KST

			details.Add(child)

		Next

	End Sub

	Private Sub gvPrint_MasterRowExpanded(sender As Object, e As CustomMasterRowEventArgs) Handles gvPrint.MasterRowExpanded
		Dim view As GridView = TryCast(TryCast(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)

		view.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		view.OptionsView.ShowIndicator = False
		view.OptionsBehavior.Editable = False
		view.OptionsView.ShowAutoFilterRow = True
		view.OptionsView.ColumnAutoWidth = False
		view.OptionsView.ShowFooter = True
		view.OptionsView.AllowHtmlDrawGroups = True

		view.Columns.Clear()

		Dim columnRENr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRENr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnRENr.OptionsColumn.AllowEdit = False
		columnRENr.Caption = m_Translate.GetSafeTranslationValue("Rechnung-Nr.")
		columnRENr.Name = "ReNr"
		columnRENr.FieldName = "ReNr"
		columnRENr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnRENr.AppearanceHeader.Options.UseTextOptions = True
		columnRENr.Width = 100
		columnRENr.Visible = True
		view.Columns.Add(columnRENr)

		Dim columnFakDat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFakDat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFakDat.OptionsColumn.AllowEdit = False
		columnFakDat.Caption = m_Translate.GetSafeTranslationValue("Faktura Datum")
		columnFakDat.Name = "FakDat"
		columnFakDat.FieldName = "FakDat"
		columnFakDat.Visible = True
		columnFakDat.Width = 150
		view.Columns.Add(columnFakDat)

		Dim columnBetragInk As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBetragInk.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBetragInk.OptionsColumn.AllowEdit = False
		columnBetragInk.Caption = m_Translate.GetSafeTranslationValue("Betrag ink. MwSt.")
		columnBetragInk.Name = "BetragInk"
		columnBetragInk.FieldName = "BetragInk"
		columnBetragInk.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBetragInk.AppearanceHeader.Options.UseTextOptions = True
		columnBetragInk.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBetragInk.DisplayFormat.FormatString = "N2"
		columnBetragInk.Width = 150
		columnBetragInk.Visible = True
		columnBetragInk.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnBetragInk.SummaryItem.DisplayFormat = "{0:n2}"
		view.Columns.Add(columnBetragInk)

		Dim columnArt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnArt.OptionsColumn.AllowEdit = False
		columnArt.Caption = m_Translate.GetSafeTranslationValue("Faktura Art")
		columnArt.Name = "Art"
		columnArt.FieldName = "Art"
		columnArt.Visible = False
		columnArt.Width = 50
		view.Columns.Add(columnArt)

		Dim columnKST As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKST.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnKST.OptionsColumn.AllowEdit = False
		columnKST.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
		columnKST.Name = "KST"
		columnKST.FieldName = "KST"
		columnKST.Visible = False
		columnKST.Width = 50
		view.Columns.Add(columnKST)


		Dim detailView As GridView = CType(CType(sender, GridView).GetDetailView(e.RowHandle, e.RelationIndex), GridView)
		RemoveHandler detailView.RowCellClick, AddressOf OngvDetail_RowCellClick
		If (Not (detailView) Is Nothing) Then
			'detailView.ParentView
			AddHandler detailView.RowCellClick, AddressOf OngvDetail_RowCellClick
		End If

	End Sub

	Sub OngvDetail_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim detailView As GridView = CType(sender, GridView)
			If (Not (detailView) Is Nothing) Then
				Dim dataRow = detailView.GetRow(e.RowHandle)
				Dim viewData = CType(dataRow, InvoiceDunningPrintViewData)
				If viewData.ReNr > 0 Then OpenSelectedInvoice(viewData.ReNr)
			End If
		End If

	End Sub


	Private Sub CreatePrintPopupMenu()

		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Mahnungen Drucken#PrintDunning"}
		Try

			bbiPrint.Manager = Me.BarManager1
			Dim allowedEmployeWOS As Boolean = m_mandant.AllowedExportCustomer2WOS(m_InitializationData.MDData.MDNr, Now.Year)
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				bshowMnu = myValue(0).ToString <> String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintDunning".ToUpper
					m_SelectedWOSEnun = WOSSENDValue.PrintWithoutSending

				Case "SendWOS_PrintRest".ToUpper
					m_SelectedWOSEnun = WOSSENDValue.PrintOtherSendWOS

				Case "SendAndPrint".ToUpper
					m_SelectedWOSEnun = WOSSENDValue.PrintAndSend

				Case Else
					Return

			End Select

			Dim listData = GetSelectedRName1()
			If listData.Count > 0 Then
				StartPrinting()

			Else
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(strMsg)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		SearchData()
	End Sub

	Private Sub OnbbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl
		If popupMenu Is Nothing Then Return

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))
	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
		Dim success As Boolean = True
		Dim msg As String = String.Empty

		Dim invoiceData = GetSelectedDunningPrintData()
		If invoiceData Is Nothing OrElse invoiceData.Count = 0 Then
			msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
			m_UtilityUI.ShowInfoDialog(msg)
			Return
		End If
		If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie die erstellten Rechnungen wirklich löschen?"),
															m_Translate.GetSafeTranslationValue("Rechnung löschen")) = False) Then
			Return
		End If
		For Each itm In invoiceData
			success = success AndAlso m_DunningPrintDatabaseAccess.DeleteCreatedDunning(lueMandant.EditValue, itm, lueDunningLevel.EditValue, lueDunningDate.EditValue)
		Next
		If Not success Then
			msg = m_Translate.GetSafeTranslationValue("Mahndaten konnten nicht zurück gesetzt werden!")
			m_Logger.LogError(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return
		Else
			msg = m_Translate.GetSafeTranslationValue("Mahndaten wurden erfolgreich zurück gesetzt.")

			m_UtilityUI.ShowInfoDialog(msg)

			SearchData()

		End If

	End Sub

	Private Sub LockControls(ByVal lock As Boolean)
		bbiPrint.Enabled = lock
		bbiDelete.Enabled = lock
		bbiExport.Enabled = lock
	End Sub

	Private Sub SearchData()
		Dim success As Boolean = LoadInvoicePrintList()

		If success Then CreatePrintPopupMenu()
		bbiPrint.Enabled = gvPrint.RowCount > 0
		bbiExport.Enabled = gvPrint.RowCount > 0
		bbiDelete.Enabled = gvPrint.RowCount > 0

		SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim invoiceNumbers = GetSelectedRName1()
			If invoiceNumbers.Count > 0 Then
				StartExporting()

			Else
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
				m_UtilityUI.ShowInfoDialog(strMsg)

			End If

		Catch ex As Exception
			m_Logger.LogInfo(String.Format("{0}:{1}", strMethodeName, ex.ToString))

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

	End Sub

	Sub StartPrinting()
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim msg As String

		Dim listData = GetSelectedDunningPrintData()
		If listData.Count = 0 Then
			msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt!")

			Return
		End If

		Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.PreselectedDunningPrintData With {.frmhwnd = Me.Handle,
																																																 .DunningDate = lueDunningDate.EditValue,
																																																 .DunningLevel = lueDunningLevel.EditValue,
																																																 .ShowAsDesign = ShowDesign,
																																																 .ExportPrintInFiles = False,
																																																 .PrintAssignedInvoices = chk_PrintInvoices.EditValue}

		Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintDunning(m_InitializationData)
		printUtil.PreselectionData = _setting
		printUtil.SelectedDunningData = listData
		Dim result = printUtil.PrintDunning()

		printUtil.Dispose()

		If Not ShowDesign AndAlso result.Printresult AndAlso Not m_SelectedWOSEnun = WOSSENDValue.PrintWithoutSending Then
			msg = m_Translate.GetSafeTranslationValue("Ihre Dokumente wurden erfolgreich übermitteilt.")

			m_UtilityUI.ShowInfoDialog(msg)

		ElseIf result.Printresult = False Then
			m_UtilityUI.ShowErrorDialog(result.PrintresultMessage)

		End If


	End Sub

	Sub StartExporting()
		Dim msg As String

		Dim listData = GetSelectedDunningPrintData()
		If listData.Count = 0 Then
			msg = m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt!")

			Return
		End If

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try

			Dim _setting As New SPS.Listing.Print.Utility.InvoicePrint.PreselectedDunningPrintData With {.frmhwnd = Me.Handle,
																																																	 .DunningDate = lueDunningDate.EditValue,
																																																	 .DunningLevel = lueDunningLevel.EditValue,
																																																	 .ShowAsDesign = False,
																																																	 .ExportPrintInFiles = True,
																																																	 .PrintAssignedInvoices = chk_PrintInvoices.EditValue}
			Dim printUtil = New SPS.Listing.Print.Utility.InvoicePrint.ClsPrintDunning(m_InitializationData)
			printUtil.PreselectionData = _setting
			printUtil.SelectedDunningData = listData
			Dim result = printUtil.PrintDunning()

			printUtil.Dispose()

			If result.Printresult AndAlso _setting.ExportedFiles.Count > 0 Then
				Dim filename As String = _setting.ExportedFiles(0).ToString
				Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in {0} gespeichert."), Path.Combine(_setting.ExportPath, filename))

				If _setting.ExportedFiles.Count > 1 Then
					Dim pdfDocument As New PdfDocumentProcessor()
					pdfDocument.LoadDocument(filename)

					For i As Integer = 1 To _setting.ExportedFiles.Count - 1
						pdfDocument.AppendDocument(_setting.ExportedFiles(i))
						pdfDocument.SaveDocument(filename)
					Next
					pdfDocument.CloseDocument()
				End If

				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowInfoDialog(strMsg)
				Process.Start(filename)

			Else
				Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.{0}{1}{0}{2}"),
																						 vbNewLine, _setting.ExportPath, result.PrintresultMessage)
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(strMsg)
			End If


		Catch ex As Exception
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Sub


	Private Function GetSelectedRName1() As List(Of String)

		Dim result As List(Of String)

		gvPrint.FocusedColumn = gvPrint.VisibleColumns(1)
		grdPrint.RefreshDataSource()
		Dim printList As BindingList(Of DunningPrintData) = grdPrint.DataSource
		Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

		result = New List(Of String)

		For Each receiver In sentList
			result.Add(receiver.RName1)
		Next


		Return result

	End Function

	Private Function GetSelectedDunningPrintData() As BindingList(Of DunningPrintData)

		Dim result As BindingList(Of DunningPrintData)

		gvPrint.FocusedColumn = gvPrint.VisibleColumns(1)
		grdPrint.RefreshDataSource()
		Dim printList As BindingList(Of DunningPrintData) = grdPrint.DataSource
		Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

		result = New BindingList(Of DunningPrintData)

		For Each receiver In sentList
			result.Add(receiver)
		Next


		Return result

	End Function

	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Sub OngvPrint_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvPrint.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvPrint.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, DunningPrintData)
				If viewData.KDNr > 0 Then OpenSelectedcustomer(viewData.KDNr)
			End If

		End If

	End Sub

	Private Sub tgsSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSelection.Toggled
		SelDeSelectItems(tgsSelection.EditValue)
	End Sub

	Private Sub SelDeSelectItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of DunningPrintData) = grdPrint.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.IsSelected = selectItem
			Next
		End If

		gvPrint.RefreshData()

	End Sub


#Region "Helpers"

	''' <summary>
	''' Changes the mandant nr.
	''' </summary>
	''' <param name="mdNr">The mandant number.</param>
	Public Sub ChangeMandant(ByVal mdNr As Integer?)

		If mdNr Is Nothing Then mdNr = m_InitializationData.MDData.MDNr
		Dim conStr = m_mandant.GetSelectedMDData(mdNr).MDDbConn

		If Not m_CurrentConnectionString = conStr Then

			m_CurrentConnectionString = conStr

			m_CurrentConnectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)
			m_DunningPrintDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)

			m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))

		End If

	End Sub

	Private Sub OpenSelectedcustomer(ByVal customerNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customerNumber)
			hub.Publish(openMng)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Sub OpenSelectedInvoice(ByVal invoiceNumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, invoiceNumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

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


	Private Class InvoiceDunningPrintViewData
		Public Property ReNr As Integer?
		Public Property Art As String
		Public Property KST As String
		Public Property FakDat As DateTime?
		Public Property BetragInk As Decimal?

	End Class


#End Region







End Class

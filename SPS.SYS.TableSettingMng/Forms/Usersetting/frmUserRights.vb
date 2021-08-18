
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Services

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraRichEdit

Public Class frmUserRights


#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As Common.ICommonDatabaseAccess

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_SuppressUIEvents As Boolean

	Private connectionString As String
	Private m_CurrenctUserData As UserData
	Private m_CurrentUserNumber As Integer

	Private Property m_Selectedmodul As SelectedModulKey

	Private errorProviderMangement As DXErrorProvider.DXErrorProvider

#End Region



#Region "private consts"

	Private Const MODUL_NAME_SETTING = "deletedreclist"

	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/deletedreclist/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/deletedreclist/{1}/keepfilter"

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting


		m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		connectionString = m_InitializationData.MDData.MDDbConn
		m_TablesettingDatabaseAccess = New SP.DatabaseAccess.TableSetting.TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_CommonDatabaseAccess = New Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		errorProviderMangement = New DXErrorProvider.DXErrorProvider


		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		' Translate controls.
		TranslateControls()

		Reset()

		LoadMandantDropDownData()
		LoadRightsDropDownData()

		'grpFilter.Visible = False

		' Creates the navigation bar.
		CreateMyNavBar()

		gvUserRights.OptionsBehavior.Editable = True

		'AddHandler gvTableContent.RowCellClick, AddressOf Ongv_RowCellClick
		'AddHandler gvTableContent.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground
		AddHandler lueMandant.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub


#End Region

#Region "private readonly properties"

	Private ReadOnly Property SelectedUserRightsViewData As UserRightData
		Get
			Dim grdView = TryCast(grdUserRights.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()
				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), UserRightData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

#End Region


#Region "public Methods"

	Public Property m_CurrentUserID As Integer
	Public Property m_CurrentMandantNumber As Integer
	Public Property UserDataList As IEnumerable(Of UserData)


	Public Function LoadUserRightData() As Boolean

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True

		lueMandant.EditValue = m_CurrentMandantNumber
		m_CurrenctUserData = m_TablesettingDatabaseAccess.LoadAssignedUserData(m_CurrentUserID)
		lblUserFullname.Text = String.Format(m_Translate.GetSafeTranslationValue("Benutzerrechte für {0}"), m_CurrenctUserData.UserFullname)
		m_CurrentUserNumber = m_CurrenctUserData.USNr

		Dim success = LoadUserRightsList()

		m_SuppressUIEvents = suppressUIEventsState

		Return success
	End Function

	Private Function LoadUserRightsList() As Boolean

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim data = m_TablesettingDatabaseAccess.LoadAssignedUserRightsData(m_CurrentUserNumber, m_CurrentMandantNumber, m_Selectedmodul.ToString)

			If (data Is Nothing) Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzerrechte konnten nicht geladen werden."))
				Return False
			End If

			Dim reportGridData = (From report In data
								  Select New UserRightData With
											   {.recid = report.recid,
												  .MDNr = report.MDNr,
												  .USNr = report.USNr,
												  .SecNr = report.SecNr,
												  .ModulName = report.ModulName,
												  .ChangedOn = report.ChangedOn,
												  .ChangedFrom = report.ChangedFrom,
												  .SecNrBez = report.SecNrBez,
												  .Autorized = report.Autorized
											   }).ToList()

			Dim listDataSource As BindingList(Of UserRightData) = New BindingList(Of UserRightData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdUserRights.DataSource = listDataSource
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvUserRights.RowCount)

			Return Not listDataSource Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Function


#End Region



#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		lblHeader.Text = m_Translate.GetSafeTranslationValue(lblHeader.Text)
		lblHeaderDescription.Text = m_Translate.GetSafeTranslationValue(lblHeaderDescription.Text)
		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)

		Me.lblTableCaption.Text = m_Translate.GetSafeTranslationValue(Me.lblTableCaption.Text)
		Me.grpFilter.Text = m_Translate.GetSafeTranslationValue(Me.grpFilter.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

	End Sub


#Region "Resets"

	Private Sub Reset()

		Dim suppressUIEventsState As Boolean = m_SuppressUIEvents
		m_SuppressUIEvents = True


		ResetMandantDropDown()
		ResetRightsDropDown()
		ResetGrid()

		m_SuppressUIEvents = suppressUIEventsState

		errorProviderMangement.ClearErrors()

	End Sub


	''' <summary>
	''' Resets the mandant drop down.
	''' </summary>
	Private Sub ResetMandantDropDown()

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False
		lueMandant.Properties.DropDownRows = 10

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the rights drop down data.
	''' </summary>
	Private Sub ResetRightsDropDown()

		lueRights.Properties.DisplayMember = "Bezeichnung"
		lueRights.Properties.ValueMember = "RightProc"

		Dim columns = lueRights.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Bezeichnung", 0))

		lueRights.Properties.ShowHeader = False
		lueRights.Properties.ShowFooter = False
		lueRights.Properties.DropDownRows = 10
		lueRights.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueRights.Properties.SearchMode = SearchMode.AutoComplete
		lueRights.Properties.AutoSearchColumnIndex = 0
		lueRights.Properties.NullText = String.Empty

		lueRights.EditValue = Nothing

	End Sub

	''' <summary>
	''' reset grid
	''' </summary>
	Private Sub ResetGrid()

		gvUserRights.FocusRectStyle = DrawFocusRectStyle.RowFocus

		Dim repoHTML = New RepositoryItemHypertextLabel
		repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
		repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
		repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
		repoHTML.ReadOnly = False
		repoHTML.Appearance.Options.UseTextOptions = True
		grdUserRights.RepositoryItems.Add(repoHTML)

		'Dim memoHTML = New RepositoryItemHypertextLabel
		'memoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
		'memoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
		'memoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		'memoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
		'memoHTML.ReadOnly = False
		'memoHTML.Appearance.Options.UseTextOptions = True
		'grdUserRights.RepositoryItems.Add(memoHTML)

		Dim richHtml As RepositoryItemRichTextEdit = New RepositoryItemRichTextEdit
		richHtml.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
		richHtml.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
		richHtml.OptionsExport.Html.DefaultCharacterPropertiesExportToCss = False
		richHtml.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.Default
		richHtml.DocumentFormat = DocumentFormat.Html
		grdUserRights.RepositoryItems.Add(richHtml)

		If m_InitializationData.UserData.UserNr = 1 Then
			gvUserRights.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True
			gvUserRights.OptionsView.NewItemRowPosition = NewItemRowPosition.Top
		End If
		gvUserRights.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvUserRights.OptionsView.ShowGroupPanel = False
		gvUserRights.OptionsView.ShowIndicator = False
		gvUserRights.OptionsView.ShowAutoFilterRow = True

		gvUserRights.Columns.Clear()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.OptionsColumn.AllowEdit = False
			columnrecid.Visible = False
			gvUserRights.Columns.Add(columnrecid)

			Dim columnUSNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnUSNr.Caption = m_Translate.GetSafeTranslationValue("USNr")
			columnUSNr.Name = "USNr"
			columnUSNr.FieldName = "USNr"
			columnUSNr.Visible = False
			columnUSNr.OptionsColumn.AllowEdit = m_InitializationData.UserData.UserNr = 1
			columnUSNr.BestFit()
			gvUserRights.Columns.Add(columnUSNr)

			Dim columnSecNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSecNr.Caption = m_Translate.GetSafeTranslationValue("SecNr")
			columnSecNr.Name = "SecNr"
			columnSecNr.FieldName = "SecNr"
			columnSecNr.Visible = True
			columnSecNr.OptionsColumn.AllowEdit = m_InitializationData.UserData.UserNr = 1
			columnSecNr.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			columnSecNr.AppearanceCell.Options.UseTextOptions = True
			columnSecNr.Width = 50
			gvUserRights.Columns.Add(columnSecNr)

			Dim columnModulName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnModulName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnModulName.Caption = m_Translate.GetSafeTranslationValue("ModulName")
			columnModulName.Name = "ModulName"
			columnModulName.FieldName = "ModulName"
			columnModulName.Visible = False
			columnModulName.OptionsColumn.AllowEdit = m_InitializationData.UserData.UserNr = 1
			columnModulName.AppearanceCell.Options.UseTextOptions = True
			columnModulName.BestFit()
			gvUserRights.Columns.Add(columnModulName)

			Dim columnSecNrBez As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSecNrBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSecNrBez.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnSecNrBez.Name = "SecNrBez"
			columnSecNrBez.FieldName = "SecNrBez"
			columnSecNrBez.Visible = True
			columnSecNrBez.ColumnEdit = richHtml
			columnSecNrBez.OptionsColumn.AllowEdit = m_InitializationData.UserData.UserNr = 1
			columnSecNrBez.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			columnSecNrBez.AppearanceCell.Options.UseTextOptions = True
			columnSecNrBez.Width = 200
			gvUserRights.Columns.Add(columnSecNrBez)

			Dim columnAutorized As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAutorized.Caption = m_Translate.GetSafeTranslationValue("Erlaubt?")
			columnAutorized.Name = "Autorized"
			columnAutorized.FieldName = "Autorized"
			columnAutorized.Visible = True
			columnAutorized.OptionsColumn.AllowEdit = True
			columnAutorized.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			columnAutorized.AppearanceCell.Options.UseTextOptions = True
			columnAutorized.Width = 50
			gvUserRights.Columns.Add(columnAutorized)

			Dim columnChangedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChangedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnChangedFrom.OptionsColumn.AllowEdit = False
			columnChangedFrom.Caption = m_Translate.GetSafeTranslationValue("Geändert durch")
			columnChangedFrom.Name = "ChangedFrom"
			columnChangedFrom.FieldName = "ChangedFrom"
			columnChangedFrom.Visible = True
			columnChangedFrom.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			columnChangedFrom.AppearanceCell.Options.UseTextOptions = True
			columnChangedFrom.Width = 80
			gvUserRights.Columns.Add(columnChangedFrom)

			Dim columnChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnChangedOn.OptionsColumn.AllowEdit = False
			columnChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnChangedOn.DisplayFormat.FormatString = "G"
			columnChangedOn.Caption = m_Translate.GetSafeTranslationValue("Geändert am")
			columnChangedOn.Name = "ChangedOn"
			columnChangedOn.FieldName = "ChangedOn"
			columnChangedOn.Visible = True
			columnChangedOn.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			columnChangedOn.AppearanceCell.Options.UseTextOptions = True
			columnChangedOn.Width = 60
			gvUserRights.Columns.Add(columnChangedOn)

			Dim columnMDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMDNr.Caption = m_Translate.GetSafeTranslationValue("MDNr")
			columnMDNr.Name = "MDNr"
			columnMDNr.FieldName = "MDNr"
			columnMDNr.Visible = False
			columnMDNr.OptionsColumn.AllowEdit = False
			columnMDNr.BestFit()
			gvUserRights.Columns.Add(columnMDNr)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdUserRights.DataSource = Nothing

	End Sub


#End Region


	''' <summary>
	''' Creates Navigationbar
	''' </summary>
	Private Sub CreateMyNavBar()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.navMain.Items.Clear()
		Try
			navMain.PaintStyleName = "SkinExplorerBarView"

			' Create a Local group.
			Dim groupModuls As NavBarGroup = New NavBarGroup(("Programmmodule"))
			groupModuls.Name = "gNavModuls"



			Dim nbiEmployee As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kandidatenverwaltung"))
			nbiEmployee.Name = "Show_Employee"
			Dim nbiCustomer As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kundenverwaltung"))
			nbiCustomer.Name = "Show_Customer"
			Dim nbiVacancy As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vakanzenverwaltung"))
			nbiVacancy.Name = "Show_Vacancy"
			Dim nbiPropose As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Offertenverwaltung"))
			nbiPropose.Name = "Show_Propose"

			Dim nbiEmployement As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Einsatzverwaltung"))
			nbiEmployement.Name = "Show_Employement"
			Dim nbiReports As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rapport und Vorschussverwaltung"))
			nbiReports.Name = "Show_Reports"

			Dim nbiInvoice As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Debitorenverwaltung"))
			nbiInvoice.Name = "Show_Invoice"
			Dim nbiReceipts As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Zahlungseingänge verwalten"))
			nbiReceipts.Name = "Show_Receipts"

			Dim nbiPayroll As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Lohnverwaltung"))
			nbiPayroll.Name = "Show_Payroll"

			Dim nbiListing As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Diverse Listen"))
			nbiListing.Name = "Show_Listing"

			Dim nbiSystem As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Systemverwaltung"))
			nbiSystem.Name = "Show_System"


			Try
				navMain.BeginUpdate()

				navMain.Groups.Add(groupModuls)

				groupModuls.ItemLinks.Add(nbiEmployee)
				groupModuls.ItemLinks.Add(nbiCustomer)
				groupModuls.ItemLinks.Add(nbiVacancy)
				groupModuls.ItemLinks.Add(nbiPropose)

				groupModuls.ItemLinks.Add(nbiEmployement)
				groupModuls.ItemLinks.Add(nbiReports)
				groupModuls.ItemLinks.Add(nbiInvoice)
				groupModuls.ItemLinks.Add(nbiReceipts)
				groupModuls.ItemLinks.Add(nbiPayroll)

				groupModuls.ItemLinks.Add(nbiListing)
				groupModuls.ItemLinks.Add(nbiSystem)


				groupModuls.Expanded = True


				navMain.EndUpdate()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.Message))

			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(String.Format("Fehler (navBarMain): {0}", ex.ToString))

		End Try

	End Sub

	''' <summary>
	''' Clickevent for Navbar.
	''' </summary>
	Private Sub OnnbMain_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bForDesign As Boolean = False
		Try
			'grpFilter.Visible = False
			'm_SuppressUIEvents = True

			Dim strLinkName As String = e.Link.ItemName
			Dim strLinkCaption As String = e.Link.Caption

			For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			e.Link.Item.Appearance.ForeColor = Color.Orange

			Select Case strLinkName.ToLower
				Case "Show_Employee".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYEE
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Customer".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_CUSTOMER
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Vacancy".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_VACANCY
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Propose".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_PROPOSE
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Employement".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_EMPLOYE
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Reports".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_REPORTS
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Invoice".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_INVOICE
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Receipts".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_RECEIPT
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Payroll".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_PAYROLL
					ResetGrid()
					LoadUserRightsList()

				Case "Show_Listing".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_LISTING
					ResetGrid()
					LoadUserRightsList()

				Case "Show_System".ToLower
					m_Selectedmodul = SelectedModulKey.MODUL_SYSTEM
					ResetGrid()
					LoadUserRightsList()


				Case Else
					grdUserRights.DataSource = Nothing


			End Select
			ChangeHeaderInfo()
			m_SuppressUIEvents = False

		Catch ex As Exception

		End Try


	End Sub


	Sub ChangeHeaderInfo()
		Dim modulCaption As String = String.Empty

		Select Case m_Selectedmodul.ToString
			Case SelectedModulKey.MODUL_EMPLOYEE.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Kandidatenverwaltung")

			Case SelectedModulKey.MODUL_CUSTOMER.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Kundenverwaltung")

			Case SelectedModulKey.MODUL_VACANCY.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Vakanzenverwaltung")

			Case SelectedModulKey.MODUL_PROPOSE.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Vorschlagverwaltung")

			Case SelectedModulKey.MODUL_EMPLOYE.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Einsatzverwaltung")

			Case SelectedModulKey.MODUL_REPORTS.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Rapport- und Vorschussverwaltung")

			Case SelectedModulKey.MODUL_INVOICE.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Debitorenverwaltung")

			Case SelectedModulKey.MODUL_RECEIPT.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Verwalten der Zahlungseingänge")

			Case SelectedModulKey.MODUL_PAYROLL.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Lohnbuchhaltung")

			Case SelectedModulKey.MODUL_LISTING.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Listen und Auswertungen")

			Case SelectedModulKey.MODUL_SYSTEM.ToString
				modulCaption = m_Translate.GetSafeTranslationValue("Rechte für Systemverwaltung")


			Case Else
				modulCaption = ""

		End Select
		lblTableCaption.Text = String.Format("<b>{0}</b>", modulCaption)

	End Sub





	''' <summary>
	''' Loads the mandant down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadMandantDropDownData() As Boolean
		Dim mandantData = m_CommonDatabaseAccess.LoadCompaniesListData()

		If (mandantData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
		End If

		lueMandant.Properties.DataSource = mandantData
		lueMandant.Properties.ForceInitialize()

		Return Not mandantData Is Nothing
	End Function

	''' <summary>
	''' Loads the rights group drop down data.
	''' </summary>
	Private Function LoadRightsDropDownData() As Boolean
		Dim data = m_TablesettingDatabaseAccess.LoadRightsData()

		If (data Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorlage für Benutzer-Rechte konnten nicht geladen werden."))
		End If

		lueRights.Properties.DataSource = data
		lueRights.Properties.ForceInitialize()

		Return Not data Is Nothing
	End Function

	Private Function CopyUserRightsforAllUser() As Boolean
		Dim success As Boolean = True
		Dim msg = m_Translate.GetSafeTranslationValue(String.Format("Möchten Sie wirklich die {0}-Rechte von {1} {2}, für alle anderen Benutzer ersetzen?",
																																m_Selectedmodul.ToString, m_CurrenctUserData.Vorname, m_CurrenctUserData.Nachname))
		If Not (m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Benutzerrechte ersetzen?"))) Then
			Return False
		End If
		gvUserRights.PostEditor()
		gvUserRights.UpdateCurrentRow()

		grdUserRights.RefreshDataSource()
		If grdUserRights.DataSource Is Nothing Then Return False

		Try

			Dim listDataSource As BindingList(Of UserRightData) = New BindingList(Of UserRightData)
			listDataSource = grdUserRights.DataSource

			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			For Each p In listDataSource
				p.ChangedFrom = m_InitializationData.UserData.UserFullName
				success = success AndAlso m_TablesettingDatabaseAccess.UpdateUserRightsForAllUserData(p)
			Next

		Catch ex As Exception
			success = False
			m_Logger.LogError(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)

		End Try


		success = success AndAlso LoadUserRightsList()


		Return success

	End Function

	''' <summary>
	''' Handles change of mandant number.
	''' </summary>
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As EventArgs) Handles lueMandant.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueMandant.EditValue Is Nothing Then
			m_CurrentMandantNumber = lueMandant.EditValue
			LoadUserRightsList()
		End If

	End Sub

	''' <summary>
	''' Handles change of user rights templates.
	''' </summary>
	Private Sub OnlueRights_EditValueChanged(sender As Object, e As EventArgs) Handles lueRights.EditValueChanged

		If m_SuppressUIEvents Then
			Return
		End If

		If Not lueRights.EditValue Is Nothing Then
			Dim data As RightsData = TryCast(lueRights.GetSelectedDataRow(), RightsData)

			If Not data Is Nothing Then
				SaveUserRightsData()
			End If

		End If

	End Sub

	Private Sub OngvTableContent_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gvUserRights.RowUpdated

		grdUserRights.FocusedView.CloseEditor()
		Dim success = UpdateRecord(e.Row)

		If success Then
			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
		Else
			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden.")
		End If

	End Sub

	Private Sub OnbbiSaveforAll_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSaveforAll.ItemClick
		Dim success = CopyUserRightsforAllUser()

		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
		End If

	End Sub


	''' <summary>
	''' save user rights with templates
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function SaveUserRightsData() As Boolean
		Dim success As Boolean = True
		Dim data = TryCast(lueRights.GetSelectedDataRow(), RightsData)

		If Not data Is Nothing Then

			Dim result = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Möchten Sie wirklich die Benutzerrechte komplett gemäss Vorlage überschreiben?"),
																				 m_Translate.GetSafeTranslationValue("Benutzerrechte übernehmen?"))
			If result Then
				success = m_TablesettingDatabaseAccess.SaveUSRightsWithSelectedTemplates(m_CurrentMandantNumber, m_CurrentUserNumber, data.RightProc)
			Else
				success = False
			End If

			If success Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Benutzerrechte wurden erfolgreich gespeichert."))

			Else
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Benutzerrechte konnten nicht gespeichert werden."))
				lueRights.EditValue = Nothing

			End If

		End If


		Return success

	End Function


	Private Function UpdateRecord(ByVal rowobject As Object) As Boolean
		Dim success As Boolean = True

		Dim SelectedData = CType(rowobject, UserRightData)
		If SelectedData.recid = 0 Then
			SelectedData.ChangedFrom = m_InitializationData.UserData.UserFullName
			SelectedData.MDNr = lueMandant.EditValue ' m_InitializationData.MDData.MDNr
			SelectedData.USNr = m_CurrenctUserData.USNr '.UserData.UserNr
			SelectedData.ModulName = m_Selectedmodul.ToString

			success = success AndAlso m_TablesettingDatabaseAccess.AddAssignedRightsForAllUsersData(SelectedData)

		Else

			SelectedData.ChangedFrom = m_InitializationData.UserData.UserFullName
			success = success AndAlso m_TablesettingDatabaseAccess.UpdateAssignedUserRightsData(SelectedData)

		End If
		success = success AndAlso LoadUserRightsList()


		Return success

	End Function

	Private Sub OnbtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
		Dim success As Boolean = True
		Dim msgResult = (DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich für <b>alle Benutzer</b> löschen?"),
																									m_Translate.GetSafeTranslationValue("Datensatz löschen"),
																									MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, DevExpress.Utils.DefaultBoolean.True))
		If msgResult = DialogResult.Cancel Then Return
		success = success AndAlso DeleteRecord(msgResult = DialogResult.Yes)
		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gelöscht."))
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gelöscht werden."))
		End If
		success = success AndAlso LoadUserRightsList()

	End Sub

	Private Function DeleteRecord(ByVal deleteAllUsers As Boolean) As Boolean
		Dim success As Boolean = True

		Dim SelectedData = SelectedUserRightsViewData
		success = success AndAlso m_TablesettingDatabaseAccess.DeleteAssignedUserRightsData(SelectedData, m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr, deleteAllUsers)


		Return success

	End Function

	Private Sub OngvUserRights_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gvUserRights.RowCellStyle

		If (e.RowHandle >= 0) Then
			Dim view As GridView = CType(sender, GridView)
			Dim data = CType(view.GetRow(e.RowHandle), UserRightData)

			If data.SecNr = 665 Or data.SecNr = 672 Then e.Appearance.BackColor = Color.Yellow
			If data.SecNr = 103 OrElse data.SecNr = 134 OrElse data.SecNr = 134 OrElse data.SecNr = 135 OrElse data.SecNr = 136 OrElse data.SecNr = 137 OrElse data.SecNr = 138 OrElse data.SecNr = 139 OrElse data.SecNr = 203 OrElse data.SecNr = 226 OrElse data.SecNr = 227 OrElse data.SecNr = 229 OrElse data.SecNr = 231 OrElse data.SecNr = 232 OrElse data.SecNr = 233 OrElse data.SecNr = 252 OrElse data.SecNr = 302 OrElse data.SecNr = 351 OrElse data.SecNr = 352 OrElse data.SecNr = 403 OrElse data.SecNr = 404 OrElse data.SecNr = 451 OrElse data.SecNr = 502 OrElse data.SecNr = 551 OrElse data.SecNr = 66 OrElse data.SecNr = 569 OrElse data.SecNr = 703 OrElse data.SecNr = 803 OrElse data.SecNr = 853 OrElse data.SecNr = 100102 Then
				e.Appearance.BackColor = Color.Peru

			End If

		End If

	End Sub

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_ADD_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_ADD_BUTTON Then
			If TypeOf sender Is LookUpEdit Then

				If m_CurrenctUserData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Benutzerrechte konnten nicht geladen werden."))

					Return
				End If
				Dim msg As String = m_Translate.GetSafeTranslationValue("Hiermit kopieren Sie die Rechte aus dem Hauptmandant {0} nach ausgewählter Mandant {1}.<br>Sie Sie sicher?")
				msg = String.Format(msg, m_CurrenctUserData.MDNr, lueMandant.EditValue)
				Dim msgResult = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Benutzerrechte kopieren"))
				If msgResult = False Then Return


				Dim success = m_TablesettingDatabaseAccess.CopyUserRightsFromMainMandantToAnotherSubMandant(m_CurrenctUserData.MDNr, lueMandant.EditValue, m_CurrentUserNumber)
				If success Then
					m_UtilityUI.ShowInfoDialog(String.Format(m_Translate.GetSafeTranslationValue("Die Bentuzerrechte wurden erfolgreich in Mandant {0} kopiert."), lueMandant.EditValue))

					m_CurrentMandantNumber = lueMandant.EditValue
					LoadUserRightsList()

				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Bentuzerrechte konnten nicht in Mandant {0} kopiert werden."), lueMandant.EditValue)

				End If

			End If
		End If

	End Sub

#End Region


#Region "Forms"

	Private Sub Onfrm_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		SaveFromSettings()
	End Sub

	''' <summary>
	''' Loads form settings if form gets visible.
	''' </summary>
	Private Sub OnFrm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

		If Visible Then
			LoadFormSettings()
		End If

	End Sub

	''' <summary>
	''' Loads form settings.
	''' </summary>
	Private Sub LoadFormSettings()

		Try
			Dim setting_form_height = My.Settings.ifrmHeightUserRights
			Dim setting_form_width = My.Settings.ifrmWidthUserRights
			Dim setting_form_location = My.Settings.frmLocationUserRights

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

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
				My.Settings.frmLocationUserrights = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidthUserrights = Me.Width
				My.Settings.ifrmHeightUserrights = Me.Height

				My.Settings.Save()

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	Private Sub OnbtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub


#End Region



#Region "Helpers"

	Private Class SelectedModulKey

		Private Key As String

		Public Shared ReadOnly MODUL_EMPLOYEE As SelectedModulKey = New SelectedModulKey("MA")
		Public Shared ReadOnly MODUL_CUSTOMER As SelectedModulKey = New SelectedModulKey("KD")
		Public Shared ReadOnly MODUL_VACANCY As SelectedModulKey = New SelectedModulKey("VAK")
		Public Shared ReadOnly MODUL_PROPOSE As SelectedModulKey = New SelectedModulKey("OFFERS")
		Public Shared ReadOnly MODUL_EMPLOYE As SelectedModulKey = New SelectedModulKey("ES")
		Public Shared ReadOnly MODUL_REPORTS As SelectedModulKey = New SelectedModulKey("RP")
		Public Shared ReadOnly MODUL_INVOICE As SelectedModulKey = New SelectedModulKey("OP")
		Public Shared ReadOnly MODUL_RECEIPT As SelectedModulKey = New SelectedModulKey("ZE")
		Public Shared ReadOnly MODUL_PAYROLL As SelectedModulKey = New SelectedModulKey("LO")
		Public Shared ReadOnly MODUL_LISTING As SelectedModulKey = New SelectedModulKey("LST")
		Public Shared ReadOnly MODUL_SYSTEM As SelectedModulKey = New SelectedModulKey("MAIN")

		Private Sub New(key As String)
			Me.Key = key
		End Sub

		Public Overrides Function ToString() As String
			Return Me.Key
		End Function

	End Class



#End Region



End Class
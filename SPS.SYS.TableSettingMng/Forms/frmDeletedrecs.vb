
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Deleted
Imports SP.DatabaseAccess.Deleted.DataObjects
Imports SP.DatabaseAccess.Deleted.DataObjects.DeletedData
Imports SP.DatabaseAccess.Deleted.DeletedDatabaseAccess

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen



''' <summary>
''' Shows founded Reports records..
''' </summary>
Public Class frmDeletedrecs

#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_DeletedDatabaseAccess As IDeletedDatabaseAccess

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

	Private connectionString As String

	Private m_GridSettingPath As String
	Private m_GVESSettingfilename As String

	Private m_xmlSettingRestoreSearchSetting As String
	Private m_xmlSettingSearchFilter As String

	Private Property Searchedmodul As Integer

	Private rdGroup As New RadioGroup

#End Region


#Region "private consts"

	Private Const MODUL_Employee As Integer = 1
	Private Const MODUL_Customer As Integer = 2
	Private Const MODUL_Vacancy As Integer = 3
	Private Const MODUL_Propose As Integer = 4
	Private Const MODUL_Offers As Integer = 5

	Private Const MODUL_ES As Integer = 6
	Private Const MODUL_RP As Integer = 7
	Private Const MODUL_ZG As Integer = 8

	Private Const MODUL_LO As Integer = 9

	Private Const MODUL_RE As Integer = 10
	Private Const MODUL_ZE As Integer = 11

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

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		Searchedmodul = MODUL_Employee

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		connectionString = m_InitializationData.MDData.MDDbConn
		m_DeletedDatabaseAccess = New SP.DatabaseAccess.Deleted.DeletedDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		Try
			m_GridSettingPath = String.Format("{0}DeleteMng\", m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVESSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdEmployeeProperty.Name, m_InitializationData.UserData.UserNr)

			m_xmlSettingRestoreSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)

		Catch ex As Exception

		End Try

		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Dim itemValues As Object() = New Object() {0, 1, 2}
		Dim itemDescriptions As String() = New String() {m_Translate.GetSafeTranslationValue("Alle Daten anzeigen"), m_Translate.GetSafeTranslationValue("Aktuelles Jahr"), m_Translate.GetSafeTranslationValue("Aktueller Monat")}
		Dim i As Integer = 0
		Do While i < itemValues.Length
			rdGroup.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop
		rdGroup.EditValue = If(Now.Day >= 10, 2, 1)

		' Translate controls.
		TranslateControls()


		' Creates the navigation bar.
		CreateMyNavBar()

		ResetDeletedGrid()
		LoadFoundedDeletedRecsList()

		AddHandler gvEmployeeProperty.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvEmployeeProperty.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler gvEmployeeProperty.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler gvEmployeeProperty.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		AddHandler rdGroup.Properties.EditValueChanged, AddressOf OnFilterOption_EditvalueChanged

	End Sub


#End Region


#Region "Private Properties"

	''' <summary>
	''' Gets the selected deleted data.
	''' </summary>
	''' <returns>The selected document or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedDeleteViewData As DeletedData
		Get
			Dim grdView = TryCast(grdEmployeeProperty.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim propose = CType(grdView.GetRow(selectedRows(0)), DeletedData)
					Return propose
				End If

			End If

			Return Nothing
		End Get

	End Property



#End Region


#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.rlblHeader.Text = m_Translate.GetSafeTranslationValue(Me.rlblHeader.Text)
		Me.bsiLblRecCount.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblRecCount.Caption)

	End Sub


	''' <summary>
	''' Creates Navigationbar
	''' </summary>
	Private Sub CreateMyNavBar()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.navMain.Items.Clear()
		Try
			navMain.PaintStyleName = "SkinExplorerBarView"
			navFilter.PaintStyleName = "SkinExplorerBarView"

			' Create a Local group.
			Dim groupModule As NavBarGroup = New NavBarGroup(("Module"))
			groupModule.Name = "gNavModule"

			Dim nbiEmployee As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kandidaten"))
			nbiEmployee.Name = "Show_Employee"

			Dim nbiCustomer As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kunden"))
			nbiCustomer.Name = "Show_Customer"

			Dim nbiVacancy As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vakanzen"))
			nbiVacancy.Name = "Show_Vacancy"

			Dim nbiPropose As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorschläge"))
			nbiPropose.Name = "Show_Propose"

			Dim nbiOffers As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Offerte"))
			nbiOffers.Name = "Show_Offers"

			Dim nbiES As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Einsätze"))
			nbiES.Name = "Show_ES"

			Dim nbiReports As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rapporte"))
			nbiReports.Name = "Show_Reports"

			Dim nbiAdvancePayment As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Vorschüsse"))
			nbiAdvancePayment.Name = "Show_AdvancePayment"

			Dim nbiPayroll As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Lohnabrechnungen"))
			nbiPayroll.Name = "Show_Payroll"

			Dim nbiInvoice As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Rechnungen"))
			nbiInvoice.Name = "Show_Invoice"

			Dim nbiPayment As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Zahlungseingänge"))
			nbiPayment.Name = "Show_Payment"

			Dim nbiGav As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kunden-GAV"))
			nbiGav.Name = "Show_gav"

			Dim nbiMABankmng As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Banken"))
			nbiMABankmng.Name = "Show_ma.bankmng"


			Try
				navMain.BeginUpdate()

				navMain.Groups.Add(groupModule)

				groupModule.ItemLinks.Add(nbiEmployee)
				groupModule.ItemLinks.Add(nbiCustomer)
				groupModule.ItemLinks.Add(nbiVacancy)
				groupModule.ItemLinks.Add(nbiPropose)
				groupModule.ItemLinks.Add(nbiOffers)

				groupModule.ItemLinks.Add(nbiES)
				groupModule.ItemLinks.Add(nbiReports)
				groupModule.ItemLinks.Add(nbiAdvancePayment)
				groupModule.ItemLinks.Add(nbiPayroll)

				groupModule.ItemLinks.Add(nbiInvoice)
				groupModule.ItemLinks.Add(nbiPayment)

				groupModule.ItemLinks.Add(nbiGav)
				groupModule.ItemLinks.Add(nbiMABankmng)

				groupModule.Expanded = True

				navMain.EndUpdate()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

			End Try

			Try

				navFilter.BeginUpdate()

				' Create a Local group.
				Dim groupFilter As NavBarGroup = New NavBarGroup(("Filter"))
				groupFilter.Name = "gNavFilter"

				navFilter.Groups.Add(groupFilter)

				rdGroup.Width = navFilter.Width - 10
				rdGroup.Properties.BorderStyle = BorderStyles.NoBorder

				groupFilter.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer
				groupFilter.ControlContainer.Controls.Add(rdGroup)
				groupFilter.GroupClientHeight = rdGroup.Height + 10
				navFilter.Width = rdGroup.Width + 10

				groupFilter.Expanded = True


				navFilter.EndUpdate()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message),
																								 "Menüleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)

		End Try

	End Sub

	''' <summary>
	''' Clickevent for Navbar.
	''' </summary>
	Private Sub OnnbMain_LinkClicked(ByVal sender As Object, _
															 ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bForDesign As Boolean = False
		Try
			Dim strLinkName As String = e.Link.ItemName
			Dim strLinkCaption As String = e.Link.Caption

			For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			e.Link.Item.Appearance.ForeColor = Color.Orange
			Me.rlblHeader.BackColor = Color.Transparent

			Select Case strLinkName.ToLower
				Case "show_employee".ToLower
					Searchedmodul = 1
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_customer".ToLower
					Searchedmodul = 2
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_vacancy".ToLower
					Searchedmodul = 3
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_propose".ToLower
					Searchedmodul = 4
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_offers".ToLower
					Searchedmodul = 5
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_es".ToLower
					Searchedmodul = 6
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_reports".ToLower
					Searchedmodul = 7
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_advancepayment".ToLower
					Searchedmodul = 8
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_payroll".ToLower
					Searchedmodul = 9
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_invoice".ToLower
					Searchedmodul = 10
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_payment".ToLower
					Searchedmodul = 11
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_gav".ToLower
					Searchedmodul = 12
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()

				Case "show_ma.bankmng".ToLower
					Searchedmodul = 13
					ResetDeletedGrid()
					LoadFoundedDeletedRecsList()


				Case Else
					' Do nothing
			End Select

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally

		End Try

	End Sub


	Private Sub OnFilterOption_EditvalueChanged(ByVal sender As Object, ByVal e As EventArgs)

		ResetDeletedGrid()
		LoadFoundedDeletedRecsList()

	End Sub

#Region "Reset grids"

	Sub ChangeHeaderInfo()
		Dim modulCaption As String = String.Empty

		Select Case Searchedmodul
			Case 1
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidaten")

			Case 2
				modulCaption = m_Translate.GetSafeTranslationValue("Kunden")

			Case 3
				modulCaption = m_Translate.GetSafeTranslationValue("Vakanzen")

			Case 4
				modulCaption = m_Translate.GetSafeTranslationValue("Vorschläge")

			Case 5
				modulCaption = m_Translate.GetSafeTranslationValue("Massen-Offerte")

			Case 6
				modulCaption = m_Translate.GetSafeTranslationValue("Einsätze")

			Case 7
				modulCaption = m_Translate.GetSafeTranslationValue("Rapporte")

			Case 8
				modulCaption = m_Translate.GetSafeTranslationValue("Vorschüsse")

			Case 9
				modulCaption = m_Translate.GetSafeTranslationValue("Lohnabrechnungen")

			Case 10
				modulCaption = m_Translate.GetSafeTranslationValue("Rechnungen")

			Case 11
				modulCaption = m_Translate.GetSafeTranslationValue("Zahlungseingänge")

			Case 12
				modulCaption = m_Translate.GetSafeTranslationValue("Kunden-GAV")

			Case 13
				modulCaption = m_Translate.GetSafeTranslationValue("Kandidatenbanken")

			Case Else


		End Select
		rlblHeader.Text = String.Format("<b><font size=""+6"">{0}</font></b>", modulCaption)

	End Sub

	Function GetDbModulName() As String
		Dim result As String = String.Empty

		Select Case Searchedmodul
			Case 1
				result = "MA"

			Case 2
				result = "KD"

			Case 3
				result = "Vak"

			Case 4
				result = "Propose"

			Case 5
				result = "offer"

			Case 6
				result = "ES"

			Case 7
				result = "RP"

			Case 8
				result = "ZG"

			Case 9
				result = "LO"

			Case 10
				result = "RE"

			Case 11
				result = "ZE"

			Case 12
				result = "GAV"

			Case 13
				result = "MA.BankMng"

			Case Else


		End Select

		Return result

	End Function

	''' <summary>
	''' reset Propose grid
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetDeletedGrid()

		gvEmployeeProperty.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvEmployeeProperty.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvEmployeeProperty.OptionsView.ShowGroupPanel = False
		gvEmployeeProperty.OptionsView.ShowIndicator = False
		gvEmployeeProperty.OptionsView.ShowAutoFilterRow = True

		gvEmployeeProperty.Columns.Clear()


		ChangeHeaderInfo()

		Try

			Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
			columnrecid.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columnrecid.Name = "recid"
			columnrecid.FieldName = "recid"
			columnrecid.Visible = False
			gvEmployeeProperty.Columns.Add(columnrecid)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCreatedFrom.Caption = m_Translate.GetSafeTranslationValue("Benutzer")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = True
			columnCreatedFrom.MaxWidth = 200
			gvEmployeeProperty.Columns.Add(columnCreatedFrom)

			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = True
			columnCreatedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			columnCreatedOn.MaxWidth = 150
			gvEmployeeProperty.Columns.Add(columnCreatedOn)

			Dim columndeletednumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columndeletednumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
			columndeletednumber.Name = "deletenumber"
			columndeletednumber.FieldName = "deletenumber"
			columndeletednumber.Visible = True
			columndeletednumber.MaxWidth = 100
			gvEmployeeProperty.Columns.Add(columndeletednumber)

			Dim columndeleteinfo As New DevExpress.XtraGrid.Columns.GridColumn()
			columndeleteinfo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columndeleteinfo.Caption = m_Translate.GetSafeTranslationValue("Info")
			columndeleteinfo.Name = "deleteinfo"
			columndeleteinfo.FieldName = "deleteinfo"
			columndeleteinfo.Visible = True
			columndeleteinfo.BestFit()
			gvEmployeeProperty.Columns.Add(columndeleteinfo)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdEmployeeProperty.DataSource = Nothing

	End Sub


#End Region



	Private Sub frmFoundedReports_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		SaveFromSettings()
	End Sub

	'Private Sub frmDeletedrecs_Load(sender As Object, e As EventArgs) Handles Me.Load
	'	Dim m_md As New Mandant

	'	Me.KeyPreview = True
	'	Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
	'	If strStyleName <> String.Empty Then
	'		UserLookAndFeel.Default.SetSkinStyle(strStyleName)
	'	End If

	'End Sub

	''' <summary>
	''' Loads form settings if form gets visible.
	''' </summary>
	Private Sub OnFrmFoundedReports_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

		If Visible Then
			LoadFormSettings()
		End If

	End Sub

	''' <summary>
	''' Loads form settings.
	''' </summary>
	Private Sub LoadFormSettings()

		Try
			Dim setting_form_height = My.Settings.ifrmHeight
			Dim setting_form_width = My.Settings.ifrmWidth
			Dim setting_form_location = My.Settings.frmLocation

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
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidth = Me.Width
				My.Settings.ifrmHeight = Me.Height

				My.Settings.Save()

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub



#Region "Open modules"

	Private Sub OpenSelectedDocument(ByVal recNr As Integer, ByVal bytes() As Byte)

		Dim selectedDocumentData = SelectedDeleteViewData

		Try

			If Not bytes Is Nothing Then

				Dim tempFileName = System.IO.Path.GetTempFileName()
				Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

				If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then

					m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub


#End Region

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvEmployeeProperty.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case Searchedmodul
					Case 9, 10
						HandleRowClickForDocument(column.Name, dataRow)


					Case Else
						Exit Sub

				End Select

			End If

		End If

	End Sub

	Sub HandleRowClickForDocument(ByVal column As String, ByVal dataRow As Object)

		If Not dataRow Is Nothing Then
			Dim viewData = CType(dataRow, DeletedData)

			If viewData.recid > 0 Then OpenSelectedDocument(viewData.recid, viewData.scandoc)

		End If

	End Sub

#End Region



#Region "Public Methods"

	Public Function LoadFoundedDeletedRecsList() As Boolean
		Dim modulname As String = GetDbModulName()

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try

			Dim listOfPropose = m_DeletedDatabaseAccess.LoadDeletedRecsForSelectedModules(modulname, If(rdGroup.EditValue = 1, True, False), If(rdGroup.EditValue = 2, True, False))

			If listOfPropose Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler in der {0}-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.", modulname))
				Return False
			End If

			Dim reportGridData = (From report In listOfPropose
			Select New DeletedData With
						 {.recid = report.recid,
							.deletedmodul = report.deletedmodul,
							.deletenumber = report.deletenumber,
							.deleteinfo = report.deleteinfo,
							.scandoc = report.scandoc,
							.createdfrom = report.createdfrom,
							.createdon = report.createdon
						 }).ToList()

			Dim listDataSource As BindingList(Of DeletedData) = New BindingList(Of DeletedData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdEmployeeProperty.DataSource = listDataSource
			bsiRecCount.Caption = listOfPropose.Count

			Return Not listOfPropose Is Nothing

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)
		End Try


	End Function


#End Region


#Region "GridSettings"


	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			If File.Exists(m_GVESSettingfilename) Then gvEmployeeProperty.RestoreLayoutFromXml(m_GVESSettingfilename)

			If restoreLayout AndAlso Not keepFilter Then gvEmployeeProperty.ActiveFilterCriteria = Nothing

		Catch ex As Exception

		End Try

	End Sub


	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvEmployeeProperty.SaveLayoutToXml(m_GVESSettingfilename)

	End Sub


	Private Sub OngvEmployeeProperty_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiRecCount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvEmployeeProperty.RowCount)

		OngvColumnPositionChanged(sender, New System.EventArgs)

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_Translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = m_Translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub


#End Region


End Class


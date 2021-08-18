
Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports SP.Infrastructure.UI

Imports DevExpress.XtraGrid.Views.Grid

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Views.Base
Imports SPS.MainView.DataBaseAccess
Imports System.ComponentModel
Imports System.IO
Imports SP.DatabaseAccess.ModulView
Imports SP.DatabaseAccess.ModulView.DataObjects

Public Class frmProposeDetails

	Protected Shared m_Logger As ILogger = New Logger()


	''' <summary>
	''' The modulview database access.
	''' </summary>
	Protected m_ModulViewDatabaseAccess As IModulViewDatabaseAccess

	Private m_griddata As GridData

	Private _ClsSetting As New ClsProposeSetting
	Private Property Modul2Open As String

	Private Property MetroForeColor As System.Drawing.Color
	Private Property MetroBorderColor As System.Drawing.Color

	Private m_translate As TranslateValues
	Private m_UitilityUI As UtilityUI

	Private m_GVESSettingfilename As String
	Private m_GVProposeSettingfilename As String
	Private m_GVContactSettingfilename As String
	Private m_GVZESettingfilename As String
	Private m_GVInvoiceSettingfilename As String
	Private m_GVReportSettingfilename As String

	Private m_GVESSettingfilenameWithCustomer As String
	Private m_GVProposeSettingfilenameWithCustomer As String
	Private m_GVContactSettingfilenameWithCustomer As String
	Private m_GVZESettingfilenameWithCustomer As String
	Private m_GVInvoiceSettingfilenameWithCustomer As String
	Private m_GVReportSettingfilenameWithCustomer As String


	Public Sub New(ByVal _setting As ClsProposeSetting, ByVal _m2Open As String)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me._ClsSetting = _setting
		Me.Modul2Open = _m2Open
		Me._ClsSetting.OpenDetailModul = _m2Open

		m_translate = New TranslateValues
		m_UitilityUI = New UtilityUI
		m_ModulViewDatabaseAccess = New ModulViewDatabaseAccess(ModulConstants.MDData.MDDbConn, ModulConstants.UserData.UserLanguage)

		Try
			Dim strModulName As String = Me.Modul2Open.ToLower

			m_GVESSettingfilename = String.Format("{0}Propose\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVProposeSettingfilename = String.Format("{0}Propose\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVContactSettingfilename = String.Format("{0}Propose\Details\{1}{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

			m_GVESSettingfilenameWithCustomer = String.Format("{0}Propose\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVProposeSettingfilenameWithCustomer = String.Format("{0}Propose\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)
			m_GVContactSettingfilenameWithCustomer = String.Format("{0}Propose\Details\{1}_WithCustomer{2}.xml", ModulConstants.GridSettingPath, strModulName, ModulConstants.UserData.UserNr)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try


		RemoveHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged
		Me.chkSelMA.Checked = Me._ClsSetting.Data4SelectedPropose
		AddHandler chkSelMA.CheckedChanged, AddressOf chkSelMA_CheckedChanged

		AddHandler Me.gvDetail.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvDetail.ColumnFilterChanged, AddressOf OnGVDetail_ColumnFilterChanged
		AddHandler Me.gvDetail.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
		AddHandler Me.gvDetail.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		AddHandler Me.gvDetail.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground


	End Sub

	Private Sub frmVakDetails_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.SETTING_PROPOSE_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.SETTING_PROPOSE_WIDTH = Me.Width
				My.Settings.SETTING_PROPOSE_HEIGHT = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub OnForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		OngvColumnPositionChanged(New Object, New System.EventArgs)
	End Sub

	Private Sub frmDetails_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTitle As String = String.Empty

		Try
			If My.Settings.SETTING_PROPOSE_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_VACANCIES_HEIGHT)
			If My.Settings.SETTING_PROPOSE_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_VACANCIES_WIDTH)
			If My.Settings.SETTING_PROPOSE_LOCATION <> String.Empty Then
				Dim aLoc As String() = My.Settings.SETTING_PROPOSE_LOCATION.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))

		End Try

		chkSelMA.Enabled = True
		Me.sccMain.Dock = DockStyle.Fill
		Me.bsiRecCount.Caption = TranslateText("Bereit")

		Me.gvDetail.OptionsView.ShowIndicator = False
		Select Case Me.Modul2Open.ToLower
			Case "ES".ToLower
				strTitle = "Anzeige der Einsätze"
				ResetESDetailGrid()
				LoadProposalESDetailList()

			Case "interview".ToLower
				strTitle = "Anzeige der Vorstellungstermine"
				ResetInterviewDetailGrid()
				LoadProposalInterviewDetailList()

			Case "contact".ToLower
				strTitle = "Anzeige der Kontakte"
				ResetContactDetailGrid()
				LoadProposalContactDetailList()
				chkSelMA.Enabled = False


			Case Else

		End Select
		Me.Text = String.Format(TranslateText(strTitle))
		Me.rlblDetailHeader.Text = String.Format("<b>{0}</b>", TranslateText(strTitle))
		Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

	End Sub

	Private Sub chkSelMA_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles chkSelMA.CheckedChanged

		grdDetailrec.BeginUpdate()
		Me.gvDetail.Columns.Clear()
		Me.grdDetailrec.DataSource = Nothing

		Me._ClsSetting.Data4SelectedPropose = Me.chkSelMA.Checked
		Select Case Me.Modul2Open.ToLower
			Case "ES".ToLower
				ResetESDetailGrid()
				LoadProposalESDetailList()

			Case "interview".ToLower
				ResetInterviewDetailGrid()
				LoadProposalInterviewDetailList()

			Case "contact".ToLower
				ResetContactDetailGrid()
				LoadProposalContactDetailList()

			Case Else

		End Select
		Me.grdDetailrec.EndUpdate()
		Me.bsiRecCount.Caption = String.Format(TranslateText("Anzahl Datensätze: {0}"), gvDetail.RowCount)

	End Sub

	Private Sub OnGVDetail_ColumnFilterChanged(sender As Object, e As System.EventArgs)

		Me.bsiRecCount.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvDetail.RowCount)
		OngvColumnPositionChanged(sender, e)

	End Sub



#Region "Reset und Load-Funktionen..."

	Sub ResetESDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = False

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "esnr"
			columnmodulname.FieldName = "esnr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columndestlmnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestlmnr.Caption = m_translate.GetSafeTranslationValue("Periode")
			columndestlmnr.Name = "periode"
			columndestlmnr.FieldName = "periode"
			columndestlmnr.Visible = True
			gvDetail.Columns.Add(columndestlmnr)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvDetail.Columns.Add(columnEmployeename)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_translate.GetSafeTranslationValue("ES-Als")
			columndestesnr.Name = "esals"
			columndestesnr.FieldName = "esals"
			columndestesnr.Visible = True
			gvDetail.Columns.Add(columndestesnr)

			Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlanr.Caption = m_translate.GetSafeTranslationValue("Stundenlohn")
			columnlanr.Name = "stundenlohn"
			columnlanr.FieldName = "stundenlohn"
			columnlanr.Visible = True
			columnlanr.BestFit()
			columnlanr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnlanr.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnlanr)

			Dim columnbezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbezeichnung.Caption = m_translate.GetSafeTranslationValue("Tarif")
			columnbezeichnung.Name = "tarif"
			columnbezeichnung.FieldName = "tarif"
			columnbezeichnung.Visible = True
			columnbezeichnung.BestFit()
			columnbezeichnung.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnbezeichnung.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnbezeichnung)

			Dim columnMargeOhneBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeOhneBVG.Caption = m_translate.GetSafeTranslationValue("Marge ohne BVG")
			columnMargeOhneBVG.Name = "margeohnebvg"
			columnMargeOhneBVG.FieldName = "margeohnebvg"
			columnMargeOhneBVG.Visible = True
			columnMargeOhneBVG.BestFit()
			columnMargeOhneBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnMargeOhneBVG.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnMargeOhneBVG)

			Dim columnMargeMitBVG As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMargeMitBVG.Caption = m_translate.GetSafeTranslationValue("Marge mit BVG")
			columnMargeMitBVG.Name = "margemitbvg"
			columnMargeMitBVG.FieldName = "margemitbvg"
			columnMargeMitBVG.Visible = True
			columnMargeMitBVG.BestFit()
			columnMargeMitBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnMargeMitBVG.DisplayFormat.FormatString = "N2"
			gvDetail.Columns.Add(columnMargeMitBVG)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadProposalESDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbProposalESDataForProperties(If(chkSelMA.Checked, Me._ClsSetting.SelectedProposeNr, Nothing))

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedProposalESDetailData With
			   {.esnr = person.esnr,
				.mdnr = person.mdnr,
				.employeeMDNr = person.employeeMDNr,
				.customerMDNr = person.customerMDNr,
				.periode = person.periode,
				.manr = person.manr,
				.kdnr = person.kdnr,
				.zhdnr = person.zhdnr,
				.employeename = person.employeename,
				.customername = person.customername,
				.esals = person.esals,
				.stundenlohn = person.stundenlohn,
				.tarif = person.tarif,
				.margeohnebvg = person.margeohnebvg,
				.margemitbvg = person.margemitbvg
			   }).ToList()

		Dim listDataSource As BindingList(Of FoundedProposalESDetailData) = New BindingList(Of FoundedProposalESDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdDetailrec.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Sub ResetInterviewDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = False

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "recid"
			columnmodulname.FieldName = "recid"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columndestlmnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestlmnr.Caption = m_translate.GetSafeTranslationValue("Datum")
			columndestlmnr.Name = "datum"
			columndestlmnr.FieldName = "datum"
			columndestlmnr.Visible = True
			gvDetail.Columns.Add(columndestlmnr)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnZHDname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDname.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDname.Name = "zhdname"
			columnZHDname.FieldName = "zhdname"
			columnZHDname.Visible = False
			gvDetail.Columns.Add(columnZHDname)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvDetail.Columns.Add(columnEmployeename)

			Dim columndestesnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columndestesnr.Caption = m_translate.GetSafeTranslationValue("Status")
			columndestesnr.Name = "jstate"
			columndestesnr.FieldName = "jstate"
			columndestesnr.Visible = True
			gvDetail.Columns.Add(columndestesnr)

			Dim columnlanr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnlanr.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnlanr.Name = "createdon"
			columnlanr.FieldName = "createdon"
			columnlanr.Visible = False
			columnlanr.BestFit()
			gvDetail.Columns.Add(columnlanr)

			Dim columnbezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnbezeichnung.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnbezeichnung.Name = "createdfrom"
			columnbezeichnung.FieldName = "createdfrom"
			columnbezeichnung.Visible = False
			columnbezeichnung.BestFit()
			gvDetail.Columns.Add(columnbezeichnung)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadProposalInterviewDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_DataAccess.GetDbProposalInterviewDataForProperties(If(chkSelMA.Checked, Me._ClsSetting.SelectedProposeNr, Nothing))

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedProposalInterviewDetailData With
			   {.recid = person.recid,
				.employeeMDNr = person.employeeMDNr,
				.customerMDNr = person.customerMDNr,
				.recnr = person.recnr,
				.kdnr = person.kdnr,
				.zhdnr = person.zhdnr,
				.employeenumber = person.employeenumber,
				.employeename = person.employeename,
				.customername = person.customername,
				.zhdname = person.zhdname,
				.datum = person.datum,
				.jstate = person.jstate,
				.createdon = person.createdon,
				.createdfrom = person.createdfrom
			   }).ToList()

		Dim listDataSource As BindingList(Of FoundedProposalInterviewDetailData) = New BindingList(Of FoundedProposalInterviewDetailData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdDetailrec.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function



#End Region


#Region "Contact Funktionen..."

	Sub ResetContactDetailGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = False

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Nummer")
			columnmodulname.Name = "contactnr"
			columnmodulname.FieldName = "contactnr"
			columnmodulname.Visible = False
			gvDetail.Columns.Add(columnmodulname)

			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "kdnr"
			columnKDNr.FieldName = "kdnr"
			columnKDNr.Visible = False
			gvDetail.Columns.Add(columnKDNr)

			Dim columnDatum As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDatum.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnDatum.Name = "datum"
			columnDatum.FieldName = "datum"
			columnDatum.Visible = True
			gvDetail.Columns.Add(columnDatum)

			Dim columnMonat As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonat.Caption = m_translate.GetSafeTranslationValue("Monat")
			columnMonat.Name = "monat"
			columnMonat.FieldName = "monat"
			columnMonat.Visible = False
			gvDetail.Columns.Add(columnMonat)

			Dim columnJahr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnJahr.Caption = m_translate.GetSafeTranslationValue("Jahr")
			columnJahr.Name = "jahr"
			columnJahr.FieldName = "jahr"
			columnJahr.Visible = False
			gvDetail.Columns.Add(columnJahr)

			Dim columncustomername As New DevExpress.XtraGrid.Columns.GridColumn()
			columncustomername.Caption = m_translate.GetSafeTranslationValue("Kunde")
			columncustomername.Name = "customername"
			columncustomername.FieldName = "customername"
			columncustomername.Visible = True
			gvDetail.Columns.Add(columncustomername)

			Dim columnZHDname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDname.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
			columnZHDname.Name = "zhdname"
			columnZHDname.FieldName = "zhdname"
			columnZHDname.Visible = False
			gvDetail.Columns.Add(columnZHDname)

			Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnEmployeename.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnEmployeename.Name = "employeename"
			columnEmployeename.FieldName = "employeename"
			columnEmployeename.Visible = True
			gvDetail.Columns.Add(columnEmployeename)

			Dim columnArt As New DevExpress.XtraGrid.Columns.GridColumn()
			columnArt.Caption = m_translate.GetSafeTranslationValue("Art")
			columnArt.Name = "art"
			columnArt.FieldName = "art"
			columnArt.Visible = True
			gvDetail.Columns.Add(columnArt)


			Dim columnCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedOn.Caption = m_translate.GetSafeTranslationValue("Erstellt am")
			columnCreatedOn.Name = "createdon"
			columnCreatedOn.FieldName = "createdon"
			columnCreatedOn.Visible = False
			columnCreatedOn.BestFit()
			gvDetail.Columns.Add(columnCreatedOn)

			Dim columnCreatedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCreatedFrom.Caption = m_translate.GetSafeTranslationValue("Erstellt durch")
			columnCreatedFrom.Name = "createdfrom"
			columnCreatedFrom.FieldName = "createdfrom"
			columnCreatedFrom.Visible = False
			columnCreatedFrom.BestFit()
			gvDetail.Columns.Add(columnCreatedFrom)

			RestoreGridLayoutFromXml()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub


	Public Function LoadProposalContactDetailList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim listOfEmployees = m_ModulViewDatabaseAccess.LoadAssignedProposeContactData(ModulConstants.MDData.MDNr, _ClsSetting.SelectedProposeNr, Nothing, Nothing)
		'Dim listOfEmployees = m_DataAccess.GetDbProposalContactDataForProperties(If(chkSelMA.Checked, Me._ClsSetting.SelectedProposeNr, Nothing), Nothing)

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New ModulViewProposeContactData With
			   {.contactnr = person.contactnr,
				.employeeMDNr = person.employeeMDNr,
				.customerMDNr = person.customerMDNr,
				.kdnr = person.kdnr,
				.zhdnr = person.zhdnr,
				.manr = person.manr,
				.monat = person.monat,
				.jahr = person.jahr,
				.employeename = person.employeename,
				.customername = person.customername,
				.zhdname = person.zhdname,
				.bezeichnung = person.bezeichnung,
				.beschreibung = person.beschreibung,
				.datum = person.datum,
				.art = person.art,
				.createdon = person.createdon,
				.createdfrom = person.createdfrom
			   }).ToList()

		Dim listDataSource As BindingList(Of ModulViewProposeContactData) = New BindingList(Of ModulViewProposeContactData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdDetailrec.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function




#End Region


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvDetail.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case Me.Modul2Open.ToLower
					Case "es"
						Dim viewData = CType(dataRow, FoundedProposalESDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.employeeMDNr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.customerMDNr, ModulConstants.UserData.UserNr)
								End If

							Case Else

								If viewData.esnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedESNr = viewData.esnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
								End If

						End Select


					Case "interview"
						Dim viewData = CType(dataRow, FoundedProposalInterviewDetailData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.employeenumber > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr, .SelectedMANr = viewData.employeenumber})
									_ClsKD.OpenSelectedEmployee(viewData.employeeMDNr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.customerMDNr, ModulConstants.UserData.UserNr)
								End If

							Case "zhdname"
								If viewData.zhdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr, .SelectedZHDNr = viewData.zhdnr})
									_ClsKD.OpenSelectedCPerson()
								End If

							Case Else
								If viewData.recid > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.InternviewRecNr = viewData.recnr, .SelectedMANr = viewData.employeenumber})
									_ClsKD.OpenSelectedEmployeeInterview()
								End If

						End Select


					Case "contact"
						Dim viewData = CType(dataRow, ModulViewProposeContactData)

						Select Case column.Name.ToLower
							Case "employeename"
								If viewData.manr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.employeeMDNr, .SelectedMANr = viewData.manr})
									_ClsKD.OpenSelectedEmployee(viewData.employeeMDNr, ModulConstants.UserData.UserNr)
								End If

							Case "customername"
								If viewData.kdnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.customerMDNr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomer(viewData.customerMDNr, ModulConstants.UserData.UserNr)
								End If

							Case Else
								If viewData.contactnr > 0 Then
									Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.ContactRecordNumber = viewData.contactnr, .SelectedKDNr = viewData.kdnr})
									_ClsKD.OpenSelectedCustomerContact()
								End If

						End Select

				End Select

			End If

		End If

	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = TranslateText(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub


#Region "GridSettings"


	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Select Case Me.Modul2Open
			Case "es".ToLower
				Try
					If Me.chkSelMA.Checked Then
						If File.Exists(m_GVESSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVESSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVESSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "interview".ToLower
				Try
					If Me.chkSelMA.Checked Then
						If File.Exists(m_GVProposeSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVProposeSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVProposeSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case "contact".ToLower
				Try
					If Me.chkSelMA.Checked Then
						If File.Exists(m_GVContactSettingfilenameWithCustomer) Then gvDetail.RestoreLayoutFromXml(m_GVContactSettingfilenameWithCustomer)
					Else
						If File.Exists(m_GVContactSettingfilename) Then gvDetail.RestoreLayoutFromXml(m_GVContactSettingfilename)
					End If

					If restoreLayout AndAlso Not keepFilter Then gvDetail.ActiveFilterCriteria = Nothing

				Catch ex As Exception

				End Try


			Case Else

				Exit Sub


		End Select


	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		If Me.Modul2Open = "es" Then
			If Me.chkSelMA.Checked Then
				gvDetail.SaveLayoutToXml(m_GVESSettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVESSettingfilename)

			End If

		ElseIf Me.Modul2Open = "interview" Then
			If Me.chkSelMA.Checked Then
				gvDetail.SaveLayoutToXml(m_GVProposeSettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVProposeSettingfilename)

			End If

		ElseIf Me.Modul2Open = "contact" Then

			If Me.chkSelMA.Checked Then
				gvDetail.SaveLayoutToXml(m_GVContactSettingfilenameWithCustomer)
			Else
				gvDetail.SaveLayoutToXml(m_GVContactSettingfilename)

			End If

		End If

	End Sub


#End Region


End Class
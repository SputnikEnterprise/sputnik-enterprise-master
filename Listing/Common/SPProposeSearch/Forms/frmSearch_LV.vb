
Option Strict Off

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading

Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel

Imports SPProgUtility.MainUtilities

Imports System.ComponentModel
Imports System.IO
Imports SP.KD.CustomerMng.UI
Imports SP.KD.CPersonMng.UI
Imports SP.MA.EmployeeMng.UI

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SP.Infrastructure.Logging

Public Class frmSearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc

	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_xml As New ClsXML
	Private m_md As Mandant
	Private m_utility As Utilities

	Public Property RecCount As Integer
	Private Property Sql2Open As String


	Private m_GridSettingPath As String
	Private UserGridSettingsXml As SettingsXml

	Private m_GVProposeSearchSettingfilename As String

	Private m_xmlSettingRestoreProposeSearchSetting As String
	Private m_xmlSettingProposeSearchFilter As String


#Region "Private Consts"

	Private Const MODUL_NAME_SETTING = "proposesearch"

	Private Const USER_XML_SETTING_SPUTNIK_PROPOSE_SEARCH_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/proposesearch/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_PROPOSE_SEARCH_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/proposesearch/{1}/keepfilter"

#End Region


#Region "Constructor"

	Public Sub New(ByVal strQuery As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities

		Me.pnlMain.Dock = DockStyle.Fill
		Me.Sql2Open = strQuery

		Try
			m_GridSettingPath = String.Format("{0}ProposeSearch\", m_md.GetGridSettingPath(ClsDataDetail.MDData.MDNr))
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVProposeSearchSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, ClsDataDetail.UserData.UserNr)

			m_xmlSettingRestoreProposeSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_PROPOSE_SEARCH_GRIDSETTING_RESTORE, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingProposeSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_PROPOSE_SEARCH_GRIDSETTING_FILTER, ClsDataDetail.UserData.UserNr, MODUL_NAME_SETTING)

			UserGridSettingsXml = New SettingsXml(m_md.GetAllUserGridSettingXMLFilename(ClsDataDetail.MDData.MDNr))

		Catch ex As Exception

		End Try

		ResetGridRPData()

	End Sub


#End Region


	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Function GetDbData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.PNr = CInt(m_utility.SafeGetInteger(reader, "ProposeNr", 0))
					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))
					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))
					overviewData.ZHDNr = CInt(m_utility.SafeGetInteger(reader, "kdzhdNr", 0))
					overviewData.VakNr = CInt(m_utility.SafeGetInteger(reader, "VakNr", 0))

					overviewData.bezeichnung = String.Format("{0}", m_utility.SafeGetString(reader, "bezeichnung"))

					overviewData.p_state = String.Format("{0}", m_utility.SafeGetString(reader, "p_state"))
					overviewData.p_art = String.Format("{0}", m_utility.SafeGetString(reader, "p_art"))
					overviewData.p_anstellung = String.Format("{0}", m_utility.SafeGetString(reader, "p_anstellung"))

					overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "MANachname"), m_utility.SafeGetString(reader, "MAVorname"))
					overviewData.ma_tel = String.Format("{0}", m_utility.SafeGetString(reader, "MATelefon"))
					overviewData.ma_natel = String.Format("{0}", m_utility.SafeGetString(reader, "MANatel"))

					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "Firma1"))
					overviewData.kd_tel = String.Format("{0}", m_utility.SafeGetString(reader, "kdTelefon"))

					overviewData.zhdname = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "KDZNachname"), m_utility.SafeGetString(reader, "KDZVorname"))
					overviewData.zhd_tel = String.Format("{0}", m_utility.SafeGetString(reader, "kdztelefon"))
					overviewData.zhd_natel = String.Format("{0}", m_utility.SafeGetString(reader, "kdzNatel"))

					overviewData.berater = String.Format("{0}", m_utility.SafeGetString(reader, "Berater"))

					overviewData.createdon = m_utility.SafeGetDateTime(reader, "createdon", Nothing)
					overviewData.createdfrom = m_utility.SafeGetString(reader, "CreatedFrom")

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedRPList() As Boolean

		Dim listOfEmployees = GetDbData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
																						 {.PNr = person.PNr,
																							.MANr = person.MANr,
																							.KDNr = person.KDNr,
																							.ZHDNr = person.ZHDNr,
																							 .VakNr = person.VakNr,
																						 .bezeichnung = person.bezeichnung,
																							.employeename = person.employeename,
																						 .ma_tel = person.ma_tel,
																						 .ma_natel = person.ma_natel,
																						 .customername = person.customername,
																						 .kd_tel = person.kd_tel,
																						 .zhdname = person.zhdname,
																						 .zhd_tel = person.zhd_tel,
																						 .zhd_natel = person.zhd_natel,
																						.p_state = person.p_state,
																						 .p_art = person.p_art,
																						 .p_anstellung = person.p_anstellung,
																						.berater = person.berater,
																							.createdon = person.createdon,
																							.createdfrom = person.createdfrom}).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function


	Private Sub ResetGridRPData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnProposeNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnProposeNumber.Caption = m_xml.GetSafeTranslationValue("Nummer")
		columnProposeNumber.Name = "PNr"
		columnProposeNumber.FieldName = "PNr"
		columnProposeNumber.Visible = False
		gvRP.Columns.Add(columnProposeNumber)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_xml.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = m_xml.GetSafeTranslationValue("Kunden-Nr.")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnZHDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDNr.Caption = m_xml.GetSafeTranslationValue("Zuständige Personen-Nr")
		columnZHDNr.Name = "ZHDNr"
		columnZHDNr.FieldName = "ZHDNr"
		columnZHDNr.Visible = False
		gvRP.Columns.Add(columnZHDNr)

		Dim columnVakNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVakNr.Caption = m_xml.GetSafeTranslationValue("Vakanzen-Nr")
		columnVakNr.Name = "VakNr"
		columnVakNr.FieldName = "VakNr"
		columnVakNr.Visible = False
		gvRP.Columns.Add(columnVakNr)

		Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBezeichnung.Caption = m_xml.GetSafeTranslationValue("Bezeichnung")
		columnBezeichnung.Name = "bezeichnung"
		columnBezeichnung.FieldName = "bezeichnung"
		columnBezeichnung.Visible = True
		gvRP.Columns.Add(columnBezeichnung)


		Dim columnStatus As New DevExpress.XtraGrid.Columns.GridColumn()
		columnStatus.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnStatus.Caption = m_xml.GetSafeTranslationValue("Status")
		columnStatus.Name = "p_state"
		columnStatus.FieldName = "p_state"
		columnStatus.Visible = True
		gvRP.Columns.Add(columnStatus)

		Dim columnanstellung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnanstellung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnanstellung.Caption = m_xml.GetSafeTranslationValue("Art der Anstellung")
		columnanstellung.Name = "p_anstellung"
		columnanstellung.FieldName = "p_anstellung"
		columnanstellung.Visible = True
		gvRP.Columns.Add(columnanstellung)

		Dim columnArt As New DevExpress.XtraGrid.Columns.GridColumn()
		columnArt.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnArt.Caption = m_xml.GetSafeTranslationValue("Art des Vorschlages")
		columnArt.Name = "p_art"
		columnArt.FieldName = "p_art"
		columnArt.Visible = True
		gvRP.Columns.Add(columnArt)

		Dim columnBerater As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBerater.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBerater.Caption = m_xml.GetSafeTranslationValue("Berater")
		columnBerater.Name = "berater"
		columnBerater.FieldName = "berater"
		columnBerater.Visible = True
		gvRP.Columns.Add(columnBerater)

		Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployee.Caption = m_xml.GetSafeTranslationValue("Kandidat")
		columnEmployee.Name = "employeename"
		columnEmployee.FieldName = "employeename"
		columnEmployee.Visible = True
		gvRP.Columns.Add(columnEmployee)

		Dim columnEmployeeTelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeTelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeTelefon.Caption = m_xml.GetSafeTranslationValue("Kandidaten Telefon")
		columnEmployeeTelefon.Name = "ma_tel"
		columnEmployeeTelefon.FieldName = "ma_tel"
		columnEmployeeTelefon.Visible = True
		gvRP.Columns.Add(columnEmployeeTelefon)

		Dim columnEmployeeNatel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeNatel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeNatel.Caption = m_xml.GetSafeTranslationValue("Kandidaten Mobile")
		columnEmployeeNatel.Name = "ma_natel"
		columnEmployeeNatel.FieldName = "ma_natel"
		columnEmployeeNatel.Visible = True
		gvRP.Columns.Add(columnEmployeeNatel)


		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = m_xml.GetSafeTranslationValue("Kunde")
		columnCustomer.Name = "customername"
		columnCustomer.FieldName = "customername"
		columnCustomer.Visible = True
		gvRP.Columns.Add(columnCustomer)

		Dim columnCustomerTelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerTelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerTelefon.Caption = m_xml.GetSafeTranslationValue("Kunden Telefon")
		columnCustomerTelefon.Name = "kd_tel"
		columnCustomerTelefon.FieldName = "kd_tel"
		columnCustomerTelefon.Visible = True
		gvRP.Columns.Add(columnCustomerTelefon)


		Dim columnZHDName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnZHDName.Caption = m_xml.GetSafeTranslationValue("Zuständige Person")
		columnZHDName.Name = "zhdname"
		columnZHDName.FieldName = "zhdname"
		columnZHDName.Visible = True
		gvRP.Columns.Add(columnZHDName)

		Dim columnZHDTelefon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDTelefon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnZHDTelefon.Caption = m_xml.GetSafeTranslationValue("ZHD Telefon")
		columnZHDTelefon.Name = "zhd_tel"
		columnZHDTelefon.FieldName = "zhd_tel"
		columnZHDTelefon.Visible = True
		gvRP.Columns.Add(columnZHDTelefon)

		Dim columnZHDNatel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZHDNatel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnZHDNatel.Caption = m_xml.GetSafeTranslationValue("ZHD Mobile")
		columnZHDNatel.Name = "zhd_natel"
		columnZHDNatel.FieldName = "zhd_natel"
		columnZHDNatel.Visible = True
		gvRP.Columns.Add(columnZHDNatel)

		Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedon.Caption = m_xml.GetSafeTranslationValue("Erstellt am")
		columncreatedon.Name = "createdon"
		columncreatedon.FieldName = "createdon"
		columncreatedon.Visible = False
		columncreatedon.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columncreatedon.DisplayFormat.FormatString = "G"
		gvRP.Columns.Add(columncreatedon)

		Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedfrom.Caption = m_xml.GetSafeTranslationValue("Erstellt durch")
		columncreatedfrom.Name = "createdfrom"
		columncreatedfrom.FieldName = "createdfrom"
		columncreatedfrom.Visible = False
		gvRP.Columns.Add(columncreatedfrom)

		RestoreGridLayoutFromXml()

		grdRP.DataSource = Nothing

	End Sub


#Region "Menüs..."

	Sub CallMailTo(ByVal sender As Object, ByVal e As EventArgs)
		MailTo(sender.ToString)
	End Sub

#End Region

#Region "Properties..."

	Dim _imouseX As Integer
	Property GetMouseP_X() As Integer
		Get
			Return _imouseX
		End Get
		Set(ByVal value As Integer)
			_imouseX = value
		End Set
	End Property

	Dim _imouseY As Integer
	Property GetMouseP_Y() As Integer
		Get
			Return _imouseY
		End Get
		Set(ByVal value As Integer)
			_imouseY = value
		End Set
	End Property

	Dim _iColIndex As Integer
	Property GetLV_ColIndex() As Integer
		Get
			Return _iColIndex
		End Get
		Set(ByVal value As Integer)
			_iColIndex = value
		End Set
	End Property

	Dim _iColSort As String
	Property GetLV_ColSort() As String
		Get
			Return _iColSort
		End Get
		Set(ByVal value As String)
			_iColSort = value
		End Set
	End Property

#End Region

#Region "Form Properties..."

	Private Sub frmSearchKD_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLVHeight = Me.Height
				My.Settings.ifrmLVWidth = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

#End Region


	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
			m_xml.GetChildChildBez(Me)
			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
			bbiPrintList.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub formonload(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()
		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, Me.Height)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			LoadFoundedRPList()

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format("Anzahl Datensätze: {0}", Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

			AddHandler Me.gvRP.RowCountChanged, AddressOf OngvMain_RowCountChanged
			AddHandler Me.gvRP.ColumnPositionChanged, AddressOf OngvColumnPositionChanged
			AddHandler Me.gvRP.ColumnWidthChanged, AddressOf OngvColumnPositionChanged

		Catch ex As Exception

		End Try

	End Sub


	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)
				Dim obj As New ThreadTesting.OpenFormsWithThreading()

				Select Case column.Name.ToLower
					Case "manr", "employeename"
						If viewData.MANr > 0 Then obj._OpenMAForm(viewData.MANr)
					Case "ma_tel"
						If viewData.ma_tel.Length > 0 Then StartCalling(viewData.ma_tel) ', viewData.MANr, 0, 0)
					Case "ma_natel"
						If viewData.ma_natel.Length > 0 Then StartCalling(viewData.ma_natel) ', viewData.MANr, 0, 0)

					Case "kdnr", "customername"
						If viewData.KDNr > 0 Then obj._OpenKDForm(viewData.KDNr)
					Case "kd_tel"
						If viewData.kd_tel.Length > 0 Then StartCalling(viewData.kd_tel) ', 0, viewData.KDNr, 0)

					Case "zhdnr", "zhdname"
						If viewData.ZHDNr > 0 Then obj._OpenKDZHD(viewData.KDNr, viewData.ZHDNr)
					Case "zhd_tel"
						If viewData.zhd_tel.Length > 0 Then StartCalling(viewData.zhd_tel) ', 0, viewData.KDNr, viewData.ZHDNr)
					Case "zhd_natel"
						If viewData.zhd_natel.Length > 0 Then StartCalling(viewData.zhd_natel) ', 0, viewData.KDNr, viewData.ZHDNr)


					Case "vaknr"
						If viewData.VakNr > 0 Then RunOpenVAKForm(viewData.VakNr)



					Case Else
						If viewData.PNr > 0 Then RunOpenProposeForm(viewData.PNr)

				End Select

			End If

		End If

	End Sub

	Private Sub StartCalling(ByVal number As String)

		Dim oMyProg As New SPSTapi.UI.frmCaller(ClsDataDetail.m_InitialData)

		oMyProg.LoadData(number)
		oMyProg.Show()
		oMyProg.BringToFront()

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub

	Private Sub OngvMain_RowCountChanged(sender As Object, e As EventArgs)

		Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
		Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.gvRP.RowCount)

	End Sub

	Private Sub OngvColumnPositionChanged(sender As Object, e As System.EventArgs)

		gvRP.SaveLayoutToXml(m_GVProposeSearchSettingfilename)

	End Sub

	Private Sub RestoreGridLayoutFromXml()
		Dim keepFilter = False
		Dim restoreLayout = True

		Try
			restoreLayout = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreProposeSearchSetting), True)
			keepFilter = m_utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingProposeSearchFilter), False)
		Catch ex As Exception

		End Try
		If File.Exists(m_GVProposeSearchSettingfilename) Then gvRP.RestoreLayoutFromXml(m_GVProposeSearchSettingfilename)

		If restoreLayout AndAlso Not keepFilter Then gvRP.ActiveFilterCriteria = Nothing

	End Sub


End Class


Namespace ThreadTesting


	Public Class OpenFormsWithThreading

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

		Private Property SelectedKDNr As Integer
		Private Property SelectedKDZHDNr As Integer

		Private Property SelectedVakNr As Integer
		Private Property SelectedProposeNr As Integer

		Private Property SelectedESNr As Integer
		Private Property SelectedRPNr As Integer
		Private Property SelectedOPNr As Integer
		Private Property SelectedZENr As Integer
		Private Property SelectedZGNr As Integer

		Private Property SelectedTelNr As String
		Private Property SQL2Open As String


		Sub _OpenKDForm(ByVal _iKDNr As Integer)
			Me.SelectedKDNr = _iKDNr

			Try
				Dim hub = MessageService.Instance.Hub
				Dim openMng As New OpenCustomerMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, _iKDNr)
				hub.Publish(openMng)

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Sub _OpenKDZHD(ByVal _iKDNr As Integer, ByVal _iKDZHDNr As Integer)
			Me.SelectedKDNr = _iKDNr
			Me.SelectedKDZHDNr = _iKDZHDNr

			Try
				Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))

				If (responsiblePersonsFrom.LoadResponsiblePersonData(_iKDNr, _iKDZHDNr)) Then
					responsiblePersonsFrom.Show()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

		End Sub

		Sub _OpenMAForm(ByVal _iMANr As Integer)

			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, _iMANr)
			hub.Publish(openMng)

		End Sub




		Public Sub New()
		End Sub

	End Class

End Namespace

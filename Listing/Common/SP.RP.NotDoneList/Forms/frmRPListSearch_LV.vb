
Imports DevExpress.LookAndFeel

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.ComponentModel
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SP.Infrastructure.Logging

Public Class frmRPListSearch_LV

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_utility As Utilities

	Public Property RecCount As Integer
	Private Property Sql2Open As String


#Region "Constructor..."

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal strQuery As String, ByVal LX As Integer, ByVal LY As Integer, ByVal lHeight As Integer)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		InitializeComponent()

		Me.pnlMain.Dock = DockStyle.Fill
		Me.Sql2Open = strQuery

		ResetGridRPData()

	End Sub

#End Region

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedRPData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedRPData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Function GetDbData4Show() As IEnumerable(Of FoundedRPData)
		Dim result As List(Of FoundedRPData) = Nothing
		m_utility = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitializationData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedRPData)

				While reader.Read()
					Dim overviewData As New FoundedRPData

					overviewData.RPNr = CInt(m_utility.SafeGetInteger(reader, "RPNr", 0))
					overviewData.MANr = CInt(m_utility.SafeGetInteger(reader, "MANr", 0))
					overviewData.KDNr = CInt(m_utility.SafeGetInteger(reader, "KDNr", 0))
					overviewData.ESNr = CInt(m_utility.SafeGetInteger(reader, "ESNr", 0))

					overviewData.employeename = String.Format("{0}, {1}", m_utility.SafeGetString(reader, "MANachname"), m_utility.SafeGetString(reader, "MAVorname"))
					overviewData.customername = String.Format("{0}", m_utility.SafeGetString(reader, "Firma1"))
					overviewData.rpperiode = String.Format("{0} - {1}", m_utility.SafeGetInteger(reader, "Monat", 0), m_utility.SafeGetInteger(reader, "Jahr", 0))
					overviewData.esperiode = String.Format("{0} - {1}", m_utility.SafeGetDateTime(reader, "ES_Ab", Nothing), m_utility.SafeGetDateTime(reader, "ES_Ende", Nothing))
					overviewData.es_als = String.Format("{0}", m_utility.SafeGetString(reader, "es_als"))
					overviewData.weeknumbers = String.Format("{0}", m_utility.SafeGetString(reader, "WeekNr"))

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

		'grdRP.DataSource = listOfEmployees


		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedRPData With
																						 {.RPNr = person.RPNr,
																							.MANr = person.MANr,
																							.KDNr = person.KDNr,
																							.ESNr = person.ESNr,
																							.employeename = person.employeename,
																							.customername = person.customername,
																						.es_als = person.es_als,
																						.weeknumbers = person.weeknumbers}).ToList()

		Dim listDataSource As BindingList(Of FoundedRPData) = New BindingList(Of FoundedRPData)

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

		Dim columnRPNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRPNumber.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRPNumber.Name = m_Translate.GetSafeTranslationValue("RPNr")
		columnRPNumber.FieldName = "RPNr"
		columnRPNumber.Visible = True
		gvRP.Columns.Add(columnRPNumber)

		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gvRP.Columns.Add(columnMANr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnESNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESNr.Caption = m_Translate.GetSafeTranslationValue("Einsatz-Nr")
		columnESNr.Name = "ESNr"
		columnESNr.FieldName = "ESNr"
		columnESNr.Visible = False
		gvRP.Columns.Add(columnESNr)

		Dim columnEmployee As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployee.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployee.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployee.Name = "employeename"
		columnEmployee.FieldName = "employeename"
		columnEmployee.Visible = True
		gvRP.Columns.Add(columnEmployee)

		Dim columnCustomer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomer.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomer.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCustomer.Name = "customername"
		columnCustomer.FieldName = "customername"
		columnCustomer.Visible = True
		gvRP.Columns.Add(columnCustomer)

		Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESAls.Caption = m_Translate.GetSafeTranslationValue("ES-Als")
		columnESAls.Name = "es_als"
		columnESAls.FieldName = "es_als"
		columnESAls.Visible = True
		gvRP.Columns.Add(columnESAls)

		Dim columnWeek As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWeek.Caption = m_Translate.GetSafeTranslationValue("Woche")
		columnWeek.Name = "weeknumbers"
		columnWeek.FieldName = "weeknumbers"
		columnWeek.Visible = True
		gvRP.Columns.Add(columnWeek)


		grdRP.DataSource = Nothing

	End Sub


#Region "Opens moduls..."

	Sub LoadSelectedRP(ByVal iRPNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenReportsMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, iRPNr)
		hub.Publish(openMng)

	End Sub

	Sub loadSelectedES(ByVal iESNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, iESNr)
		hub.Publish(openMng)

	End Sub

	Sub loadSelectedEmployee(ByVal iEmployeeNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, iEmployeeNr)
		hub.Publish(openMng)

	End Sub

	Sub loadSelectedCustomer(ByVal iCustomerNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, iCustomerNr)
		hub.Publish(openMng)

	End Sub

#End Region


	Private Sub frmOnDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

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

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			bbiPrintList.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmRPListSearch_LV_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim m_md As New Mandant

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
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
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

		Catch ex As Exception

		End Try

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedRPData)

				Select Case column.Name.ToLower
					Case "rpnr"
						If viewData.RPNr > 0 Then LoadSelectedRP(viewData.RPNr)

					Case "manr", "employeename"
						If viewData.MANr > 0 Then loadSelectedEmployee(viewData.MANr)

					Case "kdnr", "customername"
						If viewData.KDNr > 0 Then loadSelectedCustomer(viewData.KDNr)

					Case "esnr", "es_als", "esperiode"
						If viewData.ESNr > 0 Then loadSelectedES(viewData.ESNr)

					Case Else
						If viewData.RPNr > 0 Then LoadSelectedRP(viewData.RPNr)

				End Select

			End If

		End If

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub





End Class
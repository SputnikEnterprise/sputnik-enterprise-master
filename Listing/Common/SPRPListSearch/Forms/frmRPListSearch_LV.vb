
Imports DevExpress.LookAndFeel

Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports System.ComponentModel
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports SPRPListSearch.ClsDataDetail
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class frmRPListSearch_LV

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_CurrentConnectionString = String.Empty
	Private m_ListingDatabaseAccess As IListingDatabaseAccess


	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI



#Region "public properties"

	Public Property PrintYear As Integer
	Public Property FirstWeek As Integer
	Public Property LastWeek As Integer
	Public Property RecCount As Integer


#End Region


#Region "Constructor..."

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_UtilityUI = New UtilityUI
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_CurrentConnectionString = m_InitializationData.MDData.MDDbConn
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_CurrentConnectionString, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		Me.pnlMain.Dock = DockStyle.Fill

		TranslateControls()

		ResetGridRPData()

	End Sub

#End Region

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As RPPrintWeeklyData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), RPPrintWeeklyData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

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

		Dim columnEmployeeFullName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeFullName.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnEmployeeFullName.Name = "EmployeeFullName"
		columnEmployeeFullName.FieldName = "EmployeeFullName"
		columnEmployeeFullName.Visible = True
		gvRP.Columns.Add(columnEmployeeFullName)

		Dim columnEmployeePostcodeLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeePostcodeLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeePostcodeLocation.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Adresse")
		columnEmployeePostcodeLocation.Name = "EmployeePostcodeLocation"
		columnEmployeePostcodeLocation.FieldName = "EmployeePostcodeLocation"
		columnEmployeePostcodeLocation.Visible = False
		gvRP.Columns.Add(columnEmployeePostcodeLocation)

		Dim columnCustomerCompany As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerCompany.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerCompany.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnCustomerCompany.Name = "CustomerCompany"
		columnCustomerCompany.FieldName = "CustomerCompany"
		columnCustomerCompany.Visible = True
		gvRP.Columns.Add(columnCustomerCompany)

		Dim columnCustomerPostcodeLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerPostcodeLocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnCustomerPostcodeLocation.Caption = m_Translate.GetSafeTranslationValue("Kunden-Adresse")
		columnCustomerPostcodeLocation.Name = "CustomerPostcodeLocation"
		columnCustomerPostcodeLocation.FieldName = "CustomerPostcodeLocation"
		columnCustomerPostcodeLocation.Visible = False
		gvRP.Columns.Add(columnCustomerPostcodeLocation)

		Dim columnESAls As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESAls.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnESAls.Caption = m_Translate.GetSafeTranslationValue("ES-Als")
		columnESAls.Name = "ESAls"
		columnESAls.FieldName = "ESAls"
		columnESAls.Visible = True
		gvRP.Columns.Add(columnESAls)

		Dim columnMontag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMontag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMontag.Caption = m_Translate.GetSafeTranslationValue("Montag")
		columnMontag.Name = "Montag"
		columnMontag.FieldName = "Montag"
		columnMontag.Visible = True
		gvRP.Columns.Add(columnMontag)

		Dim columnDienstag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnCustomerCompany.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDienstag.Caption = m_Translate.GetSafeTranslationValue("Dienstag")
		columnDienstag.Name = "Dienstag"
		columnDienstag.FieldName = "Dienstag"
		columnDienstag.Visible = True
		gvRP.Columns.Add(columnDienstag)

		Dim columnMittwoch As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMittwoch.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnMittwoch.Caption = m_Translate.GetSafeTranslationValue("Mittwoch")
		columnMittwoch.Name = "Mittwoch"
		columnMittwoch.FieldName = "Mittwoch"
		columnMittwoch.Visible = True
		gvRP.Columns.Add(columnMittwoch)

		Dim columnDonnerstag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDonnerstag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDonnerstag.Caption = m_Translate.GetSafeTranslationValue("Donnerstag")
		columnDonnerstag.Name = "Donnerstag"
		columnDonnerstag.FieldName = "Donnerstag"
		columnDonnerstag.Visible = True
		gvRP.Columns.Add(columnDonnerstag)

		Dim columnFreitag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFreitag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFreitag.Caption = m_Translate.GetSafeTranslationValue("Freitag")
		columnFreitag.Name = "Freitag"
		columnFreitag.FieldName = "Freitag"
		columnFreitag.Visible = True
		gvRP.Columns.Add(columnFreitag)

		Dim columnSamstag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSamstag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSamstag.Caption = m_Translate.GetSafeTranslationValue("Samstag")
		columnSamstag.Name = "Samstag"
		columnSamstag.FieldName = "Samstag"
		columnSamstag.Visible = True
		gvRP.Columns.Add(columnSamstag)

		Dim columnSonntag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSonntag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnSonntag.Caption = m_Translate.GetSafeTranslationValue("Sonntag")
		columnSonntag.Name = "Sonntag"
		columnSonntag.FieldName = "Sonntag"
		columnSonntag.Visible = True
		gvRP.Columns.Add(columnSonntag)


		grdRP.DataSource = Nothing

	End Sub

	Public Function LoadReportWeeklyPrintData() As Boolean

		Dim listOfEmployees = m_ListingDatabaseAccess.LoadRPPrintWeeklyData(m_InitializationData.MDData.MDNr, PrintYear, FirstWeek, LastWeek, m_InitializationData.UserData.UserNr)
		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Fehler bei Auflisten der Rapport-Druck Daten.")

			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New RPPrintWeeklyData With
																				{.RPNr = person.RPNr,
																				.MANr = person.MANr,
																				.KDNr = person.KDNr,
																				.ESNr = person.ESNr,
																				.CustomerCompany = person.CustomerCompany,
																				.CustomerCountry = person.CustomerCountry,
																				.CustomerLocation = person.CustomerLocation,
																				.CustomerPostcode = person.CustomerPostcode,
																				.CustomerStreet = person.CustomerPostcode,
																				.Dienstag = person.Dienstag,
																				.Montag = person.Montag,
																				.Mittwoch = person.Mittwoch,
																				.Donnerstag = person.Donnerstag,
																				.Freitag = person.Freitag,
																				.Samstag = person.Samstag,
																				.Sonntag = person.Sonntag,
																				.EmployeeLastname = person.EmployeeLastname,
																				.EmployeeFirstname = person.EmployeeFirstname,
																				.EmployeeStreet = person.EmployeeStreet,
																				.EmployeeMobile = person.EmployeeMobile,
																				.EmployeeMobile_2 = person.EmployeeMobile_2,
																				.ESAls = person.ESAls,
																				.ESLGAVGruppe0 = person.ESLGAVGruppe0,
																				.ESLStdLohn = person.ESLStdLohn,
																				.ESLTarif = person.ESLTarif,
																				.ESLKDTSpesen = person.ESLKDTSpesen,
																				.ESLMAStdSpesen = person.ESLMAStdSpesen,
																				.ESLMATSpesen = person.ESLMATSpesen,
																				.ES_Ab = person.ES_Ab,
																				.ES_Ende = person.ES_Ende,
																				.Woche = person.Woche,
																				.Monat = person.Monat,
																				.PrintedDate = person.PrintedDate,
																				.PrintedWeeks = person.PrintedWeeks
		}).ToList()

		Dim listDataSource As BindingList(Of RPPrintWeeklyData) = New BindingList(Of RPPrintWeeklyData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		RecCount = gvRP.RowCount
		bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), RecCount)


		Return Not listOfEmployees Is Nothing
	End Function



#Region "Opens moduls..."

	Sub LoadSelectedRP(ByVal iRPNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenReportsMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, iRPNr)
		hub.Publish(openMng)

	End Sub

	Sub loadSelectedES(ByVal iESNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEinsatzMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, iESNr)
		hub.Publish(openMng)

	End Sub

	Sub loadSelectedEmployee(ByVal iEmployeeNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenEmployeeMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, iEmployeeNr)
		hub.Publish(openMng)

	End Sub

	Sub loadSelectedCustomer(ByVal iCustomerNr As Integer)

		Dim hub = MessageService.Instance.Hub
		Dim openMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, iCustomerNr)
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

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

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

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvRP.RowCellClick

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, RPPrintWeeklyData)

				Select Case column.Name.ToLower
					Case "rpnr"
						If viewData.RPNr > 0 Then LoadSelectedRP(viewData.RPNr)

					Case "manr", "EmployeeFullName".ToLower
						If viewData.MANr > 0 Then loadSelectedEmployee(viewData.MANr)

					Case "kdnr", "CustomerCompany".ToLower
						If viewData.KDNr > 0 Then loadSelectedCustomer(viewData.KDNr)

					Case "esnr", "ESAls".ToLower
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
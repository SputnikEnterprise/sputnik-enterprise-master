Imports SP.DatabaseAccess.Report.DataObjects
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports System.IO
Imports SP.Infrastructure
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.SPUserSec.ClsUserSec


Namespace UI
	Public Class ucReportDetailData

#Region "Private Constants"
		Private Const REPORTNUMBER_CAPTION_FORMAT = "{0}: {1}"

		Private MODUL_NAME_SETTING As String = "Report"
		Private Const USER_XML_SETTING_SPUTNIK_RP_ESSALARY_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/RP/eslohn/restorelayoutfromxml"
		Private Const USER_XML_SETTING_SPUTNIK_RP_ESSALARY_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/RP/eslohn/keepfilter"

#End Region


#Region "Private fields"

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant
		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		Private m_GVESSalarySettingfilename As String

		Private Property GridSettingPath As String

		Private UserGridSettingsXml As SettingsXml

		Private m_xmlSettingESSalaryFilter As String
		Private m_xmlSettingRestoreESSalarySetting As String

#End Region


#Region "Public Properties"

		''' <summary>
		''' Gets the selected ESSalary data.
		''' </summary>
		''' <returns>The selected ESSalaryData or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedESSalaryData As ESSalaryData
			Get
				Dim grdView = TryCast(grdSalaryData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim salaryData = CType(grdView.GetRow(selectedRows(0)), ESSalaryData)
						Return salaryData
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the first salary data in list.
		''' </summary>
		''' <returns>First  salary data in list or nothing.</returns>
		Public ReadOnly Property FirstESSalaryDataInList As ESSalaryData
			Get
				If gvSalaryData.RowCount > 0 Then

					Dim rowHandle = gvSalaryData.GetVisibleRowHandle(0)
					Return CType(gvSalaryData.GetRow(rowHandle), ESSalaryData)
				Else
					Return Nothing
				End If

			End Get
		End Property

		''' <summary>
		''' Gets the active es salary row
		''' </summary>
		Public ReadOnly Property ActiveESSalaryRow As ESSalaryData
			Get
				If gvSalaryData.RowCount > 0 Then

					For i = 0 To gvSalaryData.RowCount - 1

						Dim rowHandle = gvSalaryData.GetVisibleRowHandle(i)
						Dim salaryData = CType(gvSalaryData.GetRow(rowHandle), ESSalaryData)

						If salaryData.AktivLODaten.HasValue AndAlso salaryData.AktivLODaten.Value Then
							Return salaryData

						End If

					Next

					Return Nothing

				Else
					Return Nothing
				End If

			End Get
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			m_Mandant = New Mandant
			m_Utility = New Utility

			UserGridSettingsXml = New SettingsXml(m_Mandant.GetAllUserGridSettingXMLFilename(m_InitializationData.MDData.MDNr))

			AddHandler gvSalaryData.ColumnPositionChanged, AddressOf OngvESSalaryColumnPositionChanged

		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			Dim previousState = SetSuppressUIEventsState(True)

			Try
				Dim mSettingpath = String.Format("{0}Report\", m_Mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
				If Not Directory.Exists(mSettingpath) Then Directory.CreateDirectory(mSettingpath)

				m_GVESSalarySettingfilename = String.Format("{0}{1}{2}.xml", mSettingpath, gvSalaryData.Name, m_InitializationData.UserData.UserNr)

				m_xmlSettingRestoreESSalarySetting = String.Format(USER_XML_SETTING_SPUTNIK_RP_ESSALARY_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr)
				m_xmlSettingESSalaryFilter = String.Format(USER_XML_SETTING_SPUTNIK_RP_ESSALARY_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr)

			Catch ex As Exception

			End Try


			ResetReportDetails()
			ResetESSalaryGrid()

			SetSuppressUIEventsState(previousState)

		End Sub

		''' <summary>
		''' Loads data of the active report.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadDataOfActiveReport() As Boolean

			Dim activeReport = m_UCMediator.ActiveReportData

			If activeReport Is Nothing Then
				ResetReportDetails()
				Return True
			End If

			Dim success As Boolean = True

			SetSuppressUIEventsState(True)

			Dim rpDetailData = m_ReportDataAccess.LoadRPDetailData(activeReport.ReportData.RPNR)

			If rpDetailData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapport-Detaildaten konnten nicht geladen werden."))
				ResetReportDetails()
				Return False
			End If

			grpRPDetail.Text = String.Format(REPORTNUMBER_CAPTION_FORMAT, m_Translate.GetSafeTranslationValue("Rapportnummer"), activeReport.ReportData.RPNR)

			lblRPDetailMonthYearValue.Text = String.Format("{0} / {1}", rpDetailData.Month, rpDetailData.Year)
			hlnkEmployee.Text = rpDetailData.EmployeeNumber
			hlnkCustomer.Text = rpDetailData.CustomerNumber

			lblRPDetailChildCountValue.Text = If(rpDetailData.ChildsCount.HasValue, rpDetailData.ChildsCount, "0")
			hlnkES.Text = String.Format("{0}, {1}", rpDetailData.ESNr, rpDetailData.ESAls)
			lblRPDetailPeriodESFromValue.Text = String.Format("{0:dd.MM.yyy}", rpDetailData.ESFromDate)
			lblRPDetailPeriodESToValue.Text = If(rpDetailData.ESToDate.HasValue, rpDetailData.ESToDate.Value.ToString("dd.MM.yyyy"), "?")

			lblRPDetailPeriodRPFromValue.Text = String.Format("{0:dd.MM.yyy}", rpDetailData.RPFromDate)
			lblRPDetailPeriodRPToValue.Text = If(rpDetailData.RPToDate.HasValue, rpDetailData.RPToDate.Value.ToString("dd.MM.yyyy"), "?")

			lblLONrValue.Text = If(rpDetailData.LONr.HasValue, rpDetailData.LONr, String.Empty)

			' --- Salary data list ---

			success = success AndAlso LoadESSalaryData(activeReport.ReportData.ESNR)

			If success Then
				If Not ActiveESSalaryRow Is Nothing Then
					FocusESSalaryAndCheckIfSalaryDateRangeLiesInReportDateRange(ActiveESSalaryRow)
				Else
					FocusESSalaryAndCheckIfSalaryDateRangeLiesInReportDateRange(FirstESSalaryDataInList)
				End If
			End If

			SetSuppressUIEventsState(False)

			Return True
		End Function

#End Region

#Region "Private Methods"


		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			' Report detail data
			grpRPDetail.Text = String.Format(REPORTNUMBER_CAPTION_FORMAT, m_Translate.GetSafeTranslationValue("Rapportnummer"), "-")
			grpRPDetailSalaryData.Text = m_Translate.GetSafeTranslationValue(grpRPDetailSalaryData.Text)

			lblRPDetailMonthYear.Text = m_Translate.GetSafeTranslationValue(lblRPDetailMonthYear.Text)
			lblRPDetailMANr.Text = m_Translate.GetSafeTranslationValue(lblRPDetailMANr.Text)
			lblRPDetailKDNr.Text = m_Translate.GetSafeTranslationValue(lblRPDetailKDNr.Text)
			lblRPDetailChildCount.Text = m_Translate.GetSafeTranslationValue(lblRPDetailChildCount.Text)
			lblRPDetailESAls.Text = m_Translate.GetSafeTranslationValue(lblRPDetailESAls.Text)
			lblRPDetailPeriodES.Text = m_Translate.GetSafeTranslationValue(lblRPDetailPeriodES.Text)
			lblRPDetailPeriodRP.Text = m_Translate.GetSafeTranslationValue(lblRPDetailPeriodRP.Text)
			lblRPDetailLONr.Text = m_Translate.GetSafeTranslationValue(lblRPDetailLONr.Text)
			lblRPDetailLONr.Text = m_Translate.GetSafeTranslationValue(lblRPDetailLONr.Text)

		End Sub


#Region "Reset"

		''' <summary>
		''' Reset report details.
		''' </summary>
		Private Sub ResetReportDetails()

			grpRPDetail.Text = String.Format(REPORTNUMBER_CAPTION_FORMAT, m_Translate.GetSafeTranslationValue("Rapportnummer"), "-")
			lblRPDetailMonthYearValue.Text = String.Empty
			hlnkEmployee.Text = String.Empty
			hlnkCustomer.Text = String.Empty
			lblRPDetailChildCountValue.Text = String.Empty
			hlnkES.Text = String.Empty
			lblRPDetailPeriodESFromValue.Text = String.Empty
			lblRPDetailPeriodESToValue.Text = String.Empty
			lblRPDetailPeriodRPFromValue.Text = String.Empty
			lblRPDetailPeriodRPToValue.Text = String.Empty
			lblLONrValue.Text = String.Empty

		End Sub

		''' <summary>
		''' Resets the ES salary grid.
		''' </summary>
		Private Sub ResetESSalaryGrid()

			' Reset the grid
			gvSalaryData.OptionsView.ShowIndicator = False
			gvSalaryData.OptionsView.ColumnAutoWidth = False

			gvSalaryData.Columns.Clear()

			Dim columnGrundLohn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnGrundLohn.Caption = m_Translate.GetSafeTranslationValue("Lohn")
			columnGrundLohn.Name = "GrundLohn"
			columnGrundLohn.FieldName = "GrundLohn"
			columnGrundLohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnGrundLohn.AppearanceHeader.Options.UseTextOptions = True
			columnGrundLohn.Visible = True
			columnGrundLohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnGrundLohn.DisplayFormat.FormatString = "N2"
			gvSalaryData.Columns.Add(columnGrundLohn)

			Dim columnStundenLohn As New DevExpress.XtraGrid.Columns.GridColumn()
			columnStundenLohn.Caption = m_Translate.GetSafeTranslationValue("Std.-Lohn")
			columnStundenLohn.Name = "StundenLohn"
			columnStundenLohn.FieldName = "StundenLohn"
			columnStundenLohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnStundenLohn.AppearanceHeader.Options.UseTextOptions = True
			columnStundenLohn.Visible = False
			columnStundenLohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnStundenLohn.DisplayFormat.FormatString = "N2"
			gvSalaryData.Columns.Add(columnStundenLohn)

			Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTarif.Caption = m_Translate.GetSafeTranslationValue("Tarif")
			columnTarif.Name = "Tarif"
			columnTarif.FieldName = "Tarif"
			columnTarif.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnTarif.AppearanceHeader.Options.UseTextOptions = True
			columnTarif.Visible = True
			columnTarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnTarif.DisplayFormat.FormatString = "N2"
			gvSalaryData.Columns.Add(columnTarif)

			Dim columnLOVon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLOVon.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnLOVon.Name = "LOVon"
			columnLOVon.FieldName = "LOVon"
			columnLOVon.Visible = True
			gvSalaryData.Columns.Add(columnLOVon)

			grdSalaryData.DataSource = Nothing

			RestoreGridLayoutFromXml(gvSalaryData.Name.ToLower)

		End Sub

#End Region

#Region "Load Data"

		''' <summary>
		''' Loads ES salary data.
		''' </summary>
		''' <param name="esNr">The ES number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadESSalaryData(ByVal esNr As Integer) As Boolean

			Dim esSalaryDataList = m_ReportDataAccess.LoadESSalaryData(esNr)

			If esSalaryDataList Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht geladen werden."))
				Return False
			ElseIf esSalaryDataList.Count = 0 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zu dem Einsatz sind keine Lohndaten hinterlegt."))
				Return False
			End If

			Dim previousState = SetSuppressUIEventsState(True)
			grdSalaryData.DataSource = esSalaryDataList
			SetSuppressUIEventsState(previousState)

			Return True
		End Function


#End Region


#Region "GridSettings"

		Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
			Dim keepFilter = False
			Dim restoreLayout = True

			Select Case GridName.ToLower
				Case gvSalaryData.Name.ToLower
					Try
						keepFilter = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingESSalaryFilter), False)
						restoreLayout = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreESSalarySetting), True)

					Catch ex As Exception

					End Try

					If restoreLayout AndAlso File.Exists(m_GVESSalarySettingfilename) Then gvSalaryData.RestoreLayoutFromXml(m_GVESSalarySettingfilename)
					If restoreLayout AndAlso Not keepFilter Then gvSalaryData.ActiveFilterCriteria = Nothing


				Case Else

					Exit Sub


			End Select


		End Sub

		Private Sub OngvESSalaryColumnPositionChanged(sender As Object, e As System.EventArgs)

			gvSalaryData.SaveLayoutToXml(m_GVESSalarySettingfilename)

		End Sub


#End Region


#Region "Event Handlers"

		''' <summary>
		''' Handles change of focus on report overview grid.
		''' </summary>
		Private Sub OngvReportOverview_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvSalaryData.FocusedRowChanged

			If AreUIEventsSuppressed Then
				Return
			End If

			Dim rp = m_UCMediator.ActiveReportData.ReportData
			Dim rpStart = rp.Von
			Dim repEnd = rp.Bis


			Dim overlap = DoesESSalaryPeriodOverlapWithActiveReportPeriod(SelectedESSalaryData)

			If Not overlap Then
				Dim goon = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Der Gültigkeitszeitraum des gewählten Lohnes liegt ausserhalb des Rapportzeitraumes. ") & vbCrLf & vbCrLf &
																							 m_Translate.GetSafeTranslationValue("Möchten Sie trozdem fortfahren?"), m_Translate.GetSafeTranslationValue("Fortfahren?"), MessageBoxDefaultButton.Button2)

				If Not goon Then
					gvSalaryData.FocusedRowHandle = e.PrevFocusedRowHandle
					Return
				End If

			End If

			If Not DoesESSalarySocialBenefitsSameWithActiveReport(SelectedESSalaryData) Then
				ShowErrorESSalarySocialBenefitDataNotSameWithReportData()
			End If

			m_UCMediator.HandleESSalaryDataSelectionHasChange()

		End Sub

		''' <summary>
		''' Handles row style event of salary data grid.
		''' </summary>
		Private Sub OnGvSalaryData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvSalaryData.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim salaryData = CType(view.GetRow(e.RowHandle), ESSalaryData)

				If salaryData.AktivLODaten.HasValue AndAlso salaryData.AktivLODaten Then
					e.Appearance.Font = New Font(e.Appearance.Font, FontStyle.Bold)
				End If
			End If

		End Sub

		''' <summary>
		''' Handles click on employee management hyperlink.
		''' </summary>
		Private Sub OnHlnkEmployee_OpenLink(sender As System.Object, e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles hlnkEmployee.OpenLink

			Dim employeeNumber = m_UCMediator.ActiveReportData.EmployeeOfActiveReport.EmployeeNumber

			' Send a request to open a employeeMng form.
			Dim hub = MessageService.Instance.Hub
			Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, employeeNumber)
			hub.Publish(openEmployeeMng)

		End Sub

		''' <summary>
		''' Handles click on customer management hyperlink.
		''' </summary>
		Private Sub OnHlnkCustomer_OpenLink(sender As System.Object, e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles hlnkCustomer.OpenLink

			Dim customerNumber = m_UCMediator.ActiveReportData.CustomerOfActiveReport.CustomerNumber

			' Send a request to open a customerMng form.
			Dim hub = MessageService.Instance.Hub
			Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, customerNumber)
			hub.Publish(openCustomerMng)

		End Sub


		''' <summary>
		''' Handles click on ES management hyperlink.
		''' </summary>
		Private Sub OnHlnkES_OpenLink(sender As System.Object, e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles hlnkES.OpenLink

			Dim esNumber = m_UCMediator.ActiveReportData.ReportData.ESNR

			' Send a request to open a einsatzMng form.
			Dim hub = MessageService.Instance.Hub
			Dim openCustomerMng As New OpenEinsatzMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, esNumber)
			hub.Publish(openCustomerMng)
		End Sub

		''' <summary>
		''' Handles click on LO number link.
		''' </summary>
		Private Sub OnLblLONrValue_OpenLink(sender As Object, e As OpenLinkEventArgs) Handles lblLONrValue.OpenLink

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			If Not rpData Is Nothing AndAlso rpData.LONr > 0 Then
				Dim allowedUserNr As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 554, rpData.MDNr)

				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 551, rpData.MDNr) Then m_Logger.LogWarning("No rights...") : Return

				Try
					Dim _settring As New SP.LO.PrintUtility.ClsLOSetting With {.SelectedMDNr = rpData.MDNr,
																																		 .SelectedMANr = New List(Of Integer)(New Integer() {rpData.EmployeeNumber}),
																																		 .SelectedLONr = New List(Of Integer)(New Integer() {rpData.LONr}),
																																		 .SelectedMonth = New List(Of Integer)(New Integer() {0}),
																																		 .SelectedYear = New List(Of Integer)(New Integer() {0}),
																																		 .SearchAutomatic = True}

					Dim obj As New SP.LO.PrintUtility.frmLOPrint(m_InitializationData)
					obj.LOSetting = _settring

					obj.Show()
					obj.BringToFront()


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))

				End Try

			End If

		End Sub

#End Region

#Region "Helper methods"

		''' <summary>
		''' Focuses ES salary data.
		''' </summary>
		''' <param name="esLohnNr">The ES LohnNr.</param>
		Private Sub FocusESSSalaryData(ByVal esLohnNr As Integer)

			If Not grdSalaryData.DataSource Is Nothing Then

				Dim esSalaryData = CType(grdSalaryData.DataSource, List(Of ESSalaryData))

				Dim index = esSalaryData.ToList().FindIndex(Function(data) data.ESLohnNr = esLohnNr)

				Dim previousState = SetSuppressUIEventsState(True)
				Dim rowHandle = gvSalaryData.GetRowHandle(index)
				gvSalaryData.FocusedRowHandle = rowHandle
				previousState = SetSuppressUIEventsState(previousState)
			End If

		End Sub

		''' <summary>
		''' Checks if an ESSalary period overlaps with the active report period.
		''' </summary>
		Private Function DoesESSalaryPeriodOverlapWithActiveReportPeriod(ByVal esSalaryData As ESSalaryData) As Boolean

			Dim rp = m_UCMediator.ActiveReportData.ReportData
			Dim rpStart = rp.Von
			Dim repEnd = rp.Bis

			Dim loBis = If(esSalaryData.LOBis Is Nothing, DateTime.MaxValue, esSalaryData.LOBis)
			Dim overlap = esSalaryData.LOVon <= rp.Bis And rp.Von <= loBis


			Return overlap
		End Function

		''' <summary>
		''' Checks if an ESSalary FAR, Parifond data is not the same as RP data.
		''' </summary>
		Private Function DoesESSalarySocialBenefitsSameWithActiveReport(ByVal esSalaryData As ESSalaryData) As Boolean

			Dim rp = m_UCMediator.ActiveReportData.ReportData
			If rp Is Nothing Then Return False

			If String.IsNullOrWhiteSpace(esSalaryData.GAVInfo_String) Then Return True

			Dim originaGAvData As New GAVStringData()
			originaGAvData.FillFromString(esSalaryData.GAVInfo_String)

			If rp.RPGAV_FAG <> originaGAvData.FARAG OrElse rp.RPGAV_FAN <> originaGAvData.FARAN OrElse
				rp.RPGAV_VAG <> originaGAvData.VAG_Value OrElse rp.RPGAV_VAN <> originaGAvData.VAN_Value Then
				m_Logger.LogWarning(String.Format("rp.RPGAV_FAG: {0} >>> originaGAvData.FARAG: {1} | rp.RPGAV_FAN: {2} >>> originaGAvData.FARAN: {3} | rp.RPGAV_VAG: {4} >>> originaGAvData.VAG_Value: {5} | rp.RPGAV_VAN: {6} >>> originaGAvData.VAN_Value: {7}",
																					rp.RPGAV_FAG, originaGAvData.FARAG, rp.RPGAV_FAN, originaGAvData.FARAN, rp.RPGAV_VAG, originaGAvData.VAG_Value, rp.RPGAV_VAN, originaGAvData.VAN_Value))
				Return False
			End If


			Return True
		End Function

		''' <summary>
		''' Shows warning es salary outside report range.
		''' </summary>
		Private Sub ShowWarningESSalaryOutsideReportRange()
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Der Gültigkeitszeitraum des aktiven Lohnes liegt ausserhalb des Rapportzeitraumes!"))
		End Sub

		Private Sub ShowErrorESSalarySocialBenefitDataNotSameWithReportData()
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Achtung: Die Rapportdaten sind nicht mit Einsatz-Lohndaten identisch. Bitte löschen Sie den Rapport und erstellen Sie diesen neu."))
		End Sub

		''' <summary>
		''' Focuses an ESSalary and check if the date is range.
		''' </summary>
		Private Sub FocusESSalaryAndCheckIfSalaryDateRangeLiesInReportDateRange(ByVal esSalaryData As ESSalaryData)

			FocusESSSalaryData(esSalaryData.ESLohnNr)

			If Not DoesESSalaryPeriodOverlapWithActiveReportPeriod(esSalaryData) Then
				ShowWarningESSalaryOutsideReportRange()
			End If

			If Not DoesESSalarySocialBenefitsSameWithActiveReport(esSalaryData) Then
				ShowErrorESSalarySocialBenefitDataNotSameWithReportData()
			End If

		End Sub

#End Region

#End Region

	End Class

End Namespace

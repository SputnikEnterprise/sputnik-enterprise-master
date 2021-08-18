
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports System.ComponentModel
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.MonthlySalary



Public Class frmMAKiAU

#Region "private fields"


	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	Private m_employeeNumber As Integer
	Private m_recID As Integer
	Private m_employeeData As EmployeeMasterData

	''' <summary>
	''' LA List.
	''' </summary>
	Private m_LAList As IEnumerable(Of LAData)


#End Region



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_InitializationData = _setting

		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		m_MandantData = New Mandant

		m_UtilityUI = New UtilityUI

		m_InitializationData = _setting

		Reset()
		TranslateControls()

		AddHandler gvChildren.RowCellClick, AddressOf Ongv_RowCellClick

	End Sub

#End Region


#Region "reset"

	Private Sub Reset()

		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

		lueGender.EditValue = Nothing
		txtEmployeeLName.Text = String.Empty
		txtEmployeeFName.Text = String.Empty
		txtBemerkung.Text = String.Empty
		dateEditBirthday.EditValue = Nothing
		lblAge.Text = String.Empty

		lueLAData.EditValue = Nothing
		cboFirstMonth.EditValue = Nothing
		cboFirstYear.EditValue = Nothing
		cboLastMonth.EditValue = Nothing
		cboLastYear.EditValue = Nothing

		ResetGenderDropDown()
		ResetLADropDown()
		ResetMonthComboBoxes()
		ResetYearComboBoxes()
		ResetMonthlySalaryOverviewGrid()


	End Sub

	Private Sub PrepareForNew()

		lueGender.EditValue = Nothing
		txtEmployeeLName.Text = String.Empty
		txtEmployeeFName.Text = String.Empty
		txtBemerkung.Text = String.Empty
		dateEditBirthday.EditValue = Nothing
		lblAge.Text = String.Empty

		lueLAData.EditValue = Nothing
		cboFirstMonth.EditValue = Nothing
		cboFirstYear.EditValue = Nothing
		cboLastMonth.EditValue = Nothing
		cboLastYear.EditValue = Nothing

		If Not m_employeeData Is Nothing Then
			txtEmployeeLName.Text = m_employeeData.Lastname
		End If

		m_recID = Nothing

	End Sub

	Private Sub PresentDetailData(ByVal data As EmployeeChldData)

		If data Is Nothing Then Return

		m_recID = data.recID

		lueGender.EditValue = data.childsex
		txtEmployeeLName.Text = data.childLastname
		txtEmployeeFName.Text = data.childFirstname

		dateEditBirthday.EditValue = data.childGebDat
		RecalculateAge()

		lueLAData.EditValue = data.laNumber

		cboFirstMonth.EditValue = data.vonMonth
		cboFirstYear.EditValue = data.vonYear
		cboLastMonth.EditValue = data.bisMonth
		cboLastYear.EditValue = data.bisYear

		txtBemerkung.Text = data.bemerkung

		bsiCreated.Caption = String.Format("{0:f}, {1}", data.createdon, data.createdfrom)
		bsiChanged.Caption = String.Format("{0:f}{1} {2}", data.ChangedOn, If(data.ChangedOn Is Nothing, "", ","), data.ChangedFrom)

	End Sub

	''' <summary>
	''' Resets the gender drop down.
	''' </summary>
	Private Sub ResetGenderDropDown()

		lueGender.Properties.ShowHeader = False
		lueGender.Properties.ShowFooter = False
		lueGender.Properties.DropDownRows = 10

		lueGender.Properties.DisplayMember = "TranslatedGender"
		lueGender.Properties.ValueMember = "RecValue"

		Dim columns = lueGender.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("TranslatedGender", 0, ("Geschlecht")))

		lueGender.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueGender.Properties.SearchMode = SearchMode.AutoComplete
		lueGender.Properties.AutoSearchColumnIndex = 0

		lueGender.Properties.NullText = String.Empty
		lueGender.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the LA drop down data.
	''' </summary>
	Private Sub ResetLADropDown()

		lueLAData.Enabled = True

		lueLAData.Properties.DisplayMember = "DisplayText"
		lueLAData.Properties.ValueMember = "LANr"

		Dim columns = lueLAData.Properties.Columns
		columns.Clear()

		Dim laNrColumn As New LookUpColumnInfo("LANr", 0)
		laNrColumn.FormatString = "0.###"
		laNrColumn.FormatType = DevExpress.Utils.FormatType.Custom

		columns.Add(laNrColumn)
		columns.Add(New LookUpColumnInfo("LALoText", 0))

		lueLAData.Properties.ShowHeader = False
		lueLAData.Properties.ShowFooter = False
		lueLAData.Properties.DropDownRows = 10
		lueLAData.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueLAData.Properties.SearchMode = SearchMode.AutoComplete
		lueLAData.Properties.AutoSearchColumnIndex = 0

		lueLAData.Properties.NullText = String.Empty

		'Dim suppressUIEventsState = m_SuppressUIEvents
		'm_SuppressUIEvents = True
		lueLAData.EditValue = Nothing
		'm_SuppressUIEvents = suppressUIEventsState

	End Sub

	''' <summary>
	''' Resets the month combo boxes.
	''' </summary>
	Private Sub ResetMonthComboBoxes()

		'Dim suppressUIEventsState = m_SuppressUIEvents
		'm_SuppressUIEvents = True

		cboFirstMonth.Properties.Items.Clear()
		cboFirstMonth.Enabled = True

		cboLastMonth.Properties.Items.Clear()
		cboLastMonth.Enabled = True

		For i As Integer = 1 To 12
			cboFirstMonth.Properties.Items.Add(i)
			cboLastMonth.Properties.Items.Add(i)
		Next

		'm_SuppressUIEvents = suppressUIEventsState

	End Sub

	''' <summary>
	''' Resets the year combo boxes.
	''' </summary>
	Private Sub ResetYearComboBoxes()

		'Dim suppressUIEventsState = m_SuppressUIEvents
		'm_SuppressUIEvents = True

		cboFirstYear.Properties.Items.Clear()
		cboFirstYear.Enabled = True

		cboLastYear.Properties.Items.Clear()
		cboLastYear.Enabled = True

		For i As Integer = DateTime.Now.Year - 2 To DateTime.Now.Year + 10
			cboFirstYear.Properties.Items.Add(i)
			cboLastYear.Properties.Items.Add(i)
		Next

		'm_SuppressUIEvents = suppressUIEventsState

	End Sub

	''' <summary>
	''' Resets the monthly salary overview grid.
	''' </summary>
	Private Sub ResetMonthlySalaryOverviewGrid()

		' Reset the grid
		gvChildren.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvChildren.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvChildren.OptionsView.ShowGroupPanel = False
		gvChildren.OptionsView.ShowIndicator = False
		gvChildren.OptionsView.ShowAutoFilterRow = True

		gvChildren.Columns.Clear()

		Dim columnrecID As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecID.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecID.Name = "recID"
		columnrecID.FieldName = "recID"
		columnrecID.Visible = False
		gvChildren.Columns.Add(columnrecID)

		Dim columnRecNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRecNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnRecNr.Name = "RecNr"
		columnRecNr.FieldName = "RecNr"
		columnRecNr.Visible = False
		columnRecNr.Width = 235
		gvChildren.Columns.Add(columnRecNr)

		Dim columnChildFullname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnChildFullname.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnChildFullname.Name = "ChildFullname"
		columnChildFullname.FieldName = "ChildFullname"
		columnChildFullname.Visible = True
		columnChildFullname.Width = 70
		gvChildren.Columns.Add(columnChildFullname)

		Dim columnGenderLabel As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGenderLabel.Caption = m_Translate.GetSafeTranslationValue("Geschlecht")
		columnGenderLabel.Name = "GenderLabel"
		columnGenderLabel.FieldName = "GenderLabel"
		columnGenderLabel.Visible = True
		columnGenderLabel.Width = 70
		gvChildren.Columns.Add(columnGenderLabel)

		Dim columnchildGebDat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnchildGebDat.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
		columnchildGebDat.Name = "childGebDat"
		columnchildGebDat.FieldName = "childGebDat"
		columnchildGebDat.Visible = True
		columnchildGebDat.Width = 70
		gvChildren.Columns.Add(columnchildGebDat)

		Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBasis.Caption = m_Translate.GetSafeTranslationValue("Art der Zulage")
		columnBasis.Name = "ZulageArt"
		columnBasis.FieldName = "ZulageArt"
		columnBasis.Visible = True
		columnBasis.Width = 70
		gvChildren.Columns.Add(columnBasis)

		Dim columnValidFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnValidFrom.Caption = m_Translate.GetSafeTranslationValue("Von")
		columnValidFrom.Name = "ValidFrom"
		columnValidFrom.FieldName = "ValidFrom"
		columnValidFrom.Visible = True
		columnValidFrom.Width = 70
		gvChildren.Columns.Add(columnValidFrom)

		Dim columnValidTill As New DevExpress.XtraGrid.Columns.GridColumn()
		columnValidTill.Caption = m_Translate.GetSafeTranslationValue("Bis")
		columnValidTill.Name = "ValidTill"
		columnValidTill.FieldName = "ValidTill"
		columnValidTill.Visible = True
		columnValidTill.Width = 70
		gvChildren.Columns.Add(columnValidTill)

		grdChildren.DataSource = Nothing

	End Sub

	Sub TranslateControls()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

			grpAllgemein.Text = m_Translate.GetSafeTranslationValue(grpAllgemein.Text)
			grpZeitraum.Text = m_Translate.GetSafeTranslationValue(grpZeitraum.Text)

			Me.lblGeschlecht.Text = m_Translate.GetSafeTranslationValue(Me.lblGeschlecht.Text)
			Me.lblNachname.Text = m_Translate.GetSafeTranslationValue(Me.lblNachname.Text)
			Me.lblVorname.Text = m_Translate.GetSafeTranslationValue(Me.lblVorname.Text)

			Me.lblLohnart.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnart.Text)
			Me.lblVon.Text = m_Translate.GetSafeTranslationValue(Me.lblVon.Text)
			Me.lblBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBis.Text)

			Me.lblBemerkung.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung.Text)

			bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
			bsilblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsilblGeaendert.Caption)
			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)
			bbiNew.Caption = m_Translate.GetSafeTranslationValue(bbiNew.Caption)
			bbiDelete.Caption = m_Translate.GetSafeTranslationValue(bbiDelete.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	''' <summary>
	''' Loads LA data.
	''' </summary>
	''' <param name="year">The year.</param>
	''' <returns>Boolean value indicating success.</returns>
	Private Function LoadLAData(ByVal year As Integer) As Boolean

		m_LAList = m_EmployeeDatabaseAccess.LoadLAListForMonthlySalaryMng(year)

		'Dim suppressUIEventsState = m_SuppressUIEvents
		'm_SuppressUIEvents = True
		m_LAList = From m In m_LAList Where m.LANr >= 3600 And m.LANr <= 3901
		lueLAData.Properties.DataSource = m_LAList
		lueLAData.Properties.ForceInitialize()

		'm_LAList = m_LAList.Where(Function(data) data.LANr >= 3600D And data.LANr <= 3901D).FirstOrDefault()


		If Not lueLAData.EditValue Is Nothing AndAlso Not m_LAList.Any(Function(data) data.LANr = lueLAData.EditValue) Then
			lueLAData.EditValue = Nothing
		End If

		'm_SuppressUIEvents = suppressUIEventsState

		If m_LAList Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnarten konnten nicht geladen werden."))
		End If

		Return Not m_LAList Is Nothing

	End Function


	Private Sub CmdClose_Click(sender As Object, e As EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub


#End Region


	Private ReadOnly Property SelectedEmployeeChildData As EmployeeChldData
		Get
			Dim grdView = TryCast(grdChildren.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim result = CType(grdView.GetRow(selectedRows(0)), EmployeeChldData)
					Return result
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the selected LA.
	''' </summary>
	''' <returns>The selected LA or nothing.</returns>
	Public ReadOnly Property SelectedLA As LAData
		Get

			If lueLAData.EditValue Is Nothing Then
				Return Nothing
			End If

			Dim laData = m_LAList.Where(Function(data) data.LANr = lueLAData.EditValue).FirstOrDefault()
			Return laData
		End Get
	End Property

	Public Sub LoadData(ByVal employeeNumber As Integer)
		Dim success = True

		m_employeeNumber = employeeNumber

		success = success AndAlso LoadEmployeeMasterData(m_employeeNumber, True)
		success = success AndAlso LoadGenderDropDownData()
		success = success AndAlso LoadLAData(Now.Year)
		success = success AndAlso LoadEmployeeKiAuData(m_employeeNumber)

		If gvChildren.RowCount = 0 Then PrepareForNew()

		'Dim data = m_EmployeeDatabaseAccess.LoadEmployeeKiAuData(employeeNumber)
		'If data Is Nothing Then
		'	m_UtilityUI.ShowErrorDialog("Ihre Daten konnten nicht geladen werden. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
		'	Return
		'End If

	End Sub

	Private Function LoadEmployeeMasterData(ByVal employeeNumber As Integer, ByVal withFoto As Boolean) As Boolean

		m_employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_employeeNumber, withFoto)

		If m_employeeData Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Kandidatendaten konnten nicht geladen werden.")
		End If

		Return Not m_employeeData Is Nothing

	End Function

	''' <summary>
	''' Loads gender drop down data.
	''' </summary>
	'''<returns>Boolean flag indicating success.</returns>
	Private Function LoadGenderDropDownData() As Boolean
		Dim genderData = m_CommonDatabaseAccess.LoadGenderData()

		If (genderData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Geschlechtsdaten konnten nicht geladen werden."))
		End If

		lueGender.Properties.DataSource = genderData
		lueGender.Properties.ForceInitialize()

		Return Not genderData Is Nothing
	End Function

	Private Function LoadEmployeeKiAuData(ByVal employeeNumber As Integer) As Boolean
		Dim listingDataList As List(Of EmployeeChldData)

		Try
			listingDataList = m_EmployeeDatabaseAccess.LoadEmployeeKiAuData(employeeNumber)

			If (listingDataList Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))

				Return Nothing
			End If

			Dim reportGridData = (From report In listingDataList
								  Select New EmployeeChldData With
						 {.recID = report.recID,
							.RecNr = report.RecNr,
							.employeeNumber = report.employeeNumber,
							.childFirstname = report.childFirstname,
							.childLastname = report.childLastname,
							.childGebDat = report.childGebDat,
							.childsex = report.childsex,
							.laNumber = report.laNumber,
							.ZulageArt = report.ZulageArt,
							.vonMonth = report.vonMonth,
							.vonYear = report.vonYear,
							.bisMonth = report.bisMonth,
							.bisYear = report.bisYear,
							.bemerkung = report.bemerkung,
							.ChangedOn = report.ChangedOn,
							.ChangedFrom = report.ChangedFrom,
							.createdon = report.createdon,
							.createdfrom = report.createdfrom
						 }).ToList()

			Dim listDataSource As BindingList(Of EmployeeChldData) = New BindingList(Of EmployeeChldData)

			For Each p In reportGridData
				listDataSource.Add(p)
			Next

			grdChildren.DataSource = listDataSource

			Return Not listDataSource Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(ex.ToString))
			Return False

		End Try

	End Function


	''' <summary>
	''' Recalculates the age.
	''' </summary>
	Private Sub RecalculateAge()
		If dateEditBirthday.EditValue Is Nothing Then
			lblAge.Text = String.Empty
		Else
			lblAge.Text = GetAge(dateEditBirthday.EditValue)
		End If

	End Sub

	''' <summary>
	''' Gets the age in years.
	''' </summary>
	''' <param name="birthDate">The birthdate.</param>
	''' <returns>Age in years.</returns>
	Private Function GetAge(ByVal birthDate As DateTime)

		' Get year diff
		Dim years As Integer = DateTime.Now.Year - birthDate.Year

		birthDate = birthDate.AddYears(years)

		' Subtract another year if its a day before the the birth day
		If (DateTime.Today.CompareTo(birthDate) < 0) Then
			years = years - 1
		End If

		Return years

	End Function


	Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick

		Dim data = SelectedEmployeeChildData

		If data Is Nothing Then
			data = New EmployeeChldData
		End If
		data.recID = m_recID

		data.employeeNumber = m_employeeData.EmployeeNumber
		data.childsex = lueGender.EditValue
		data.childLastname = txtEmployeeLName.Text
		data.childFirstname = txtEmployeeFName.Text
		data.childGebDat = dateEditBirthday.EditValue

		data.laNumber = lueLAData.EditValue
		Dim la = SelectedLA
		If Not la Is Nothing Then
			data.ZulageArt = la.LALoText
		End If

		data.vonMonth = CType(cboFirstMonth.EditValue, Integer)
		data.vonYear = CType(cboFirstYear.EditValue, Integer)
		data.bisMonth = CType(cboLastMonth.EditValue, Integer)
		data.bisYear = CType(cboLastYear.EditValue, Integer)

		data.bemerkung = txtBemerkung.Text

		data.ChangedFrom = m_InitializationData.UserData.UserFullName
		data.createdfrom = m_InitializationData.UserData.UserFullName

		Dim success = True
		If data.recID.GetValueOrDefault(0) > 0 Then
			success = m_EmployeeDatabaseAccess.SaveEmployeeKiAuData(data)
		Else
			success = m_EmployeeDatabaseAccess.AddEmployeeKiAuData(data)
		End If

		Dim msg As String = String.Empty
		If Not success Then
			msg = "Die Daten konnten nicht gespeichert werden. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

		Else
			LoadEmployeeKiAuData(m_employeeNumber)
			m_recID = data.recID
			FocusGrid(data.recID)

			msg = "Die Daten wurden gespeichert."
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxIcon.Information)

		End If

		'End If

	End Sub

	''' <summary>
	''' Handles focus change of monthly salary row.
	''' </summary>
	Private Sub OnMonthlySalary_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvChildren.FocusedRowChanged

		Dim data = SelectedEmployeeChildData

		If Not data Is Nothing Then
			PresentDetailData(data)
		Else
			PrepareForNew()
		End If

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		Dim dataRow = gvChildren.GetRow(e.RowHandle)
		If Not dataRow Is Nothing Then
			Dim data = SelectedEmployeeChildData

			If Not data Is Nothing Then
				PresentDetailData(data)
			Else
				PrepareForNew()

			End If

		End If

	End Sub

	Private Sub OnbbiNew_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNew.ItemClick

		PrepareForNew()

	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

		If m_recID = 0 Then Return
		If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																							m_Translate.GetSafeTranslationValue("Datensatz löschen")) = True) Then

			Dim success = m_EmployeeDatabaseAccess.DeleteEmployeeKiAuData(m_recID)
			If Not success Then
				m_UtilityUI.ShowErrorDialog("Die Daten konnten nicht gelöscht werden.")
			Else
				LoadEmployeeKiAuData(m_employeeNumber)


				m_UtilityUI.ShowInfoDialog("Die Daten wurden erfolgreich gelöscht.")
				If gvChildren.RowCount = 0 Then
					PrepareForNew()

					Return
				End If

				FocusGrid(Math.Max(m_recID - 1, 1))
			End If

		End If


	End Sub

	''' <summary>
	''' Handles leave event of birthdate control.
	''' </summary>
	Private Sub OnDateEditBirthday_Leave(sender As System.Object, e As System.EventArgs) Handles dateEditBirthday.Leave
		RecalculateAge()
	End Sub

	''' <summary>
	''' Focus a monthly salary.
	''' </summary>
	''' <param name="recID">The ID number.</param>
	Private Sub FocusGrid(ByVal recID As Integer)

		Dim listDataSource As BindingList(Of EmployeeChldData) = grdChildren.DataSource

		If listDataSource Is Nothing Then
			Return
		End If

		Dim monthlySalaryViewData = listDataSource.Where(Function(data) data.recID = m_recID).FirstOrDefault()

		If Not monthlySalaryViewData Is Nothing Then
			Dim sourceIndex = listDataSource.IndexOf(monthlySalaryViewData)
			Dim rowHandle = gvChildren.GetRowHandle(sourceIndex)
			gvChildren.FocusedRowHandle = rowHandle
		End If

	End Sub


End Class

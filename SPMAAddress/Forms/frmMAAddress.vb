
Imports DevExpress.XtraEditors.Controls

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports DevExpress.LookAndFeel

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable

Public Class frmMAAddress
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
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess


	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' Boolean flag indicating if form is initializing.
	''' </summary>
	Protected m_SuppressUIEvents As Boolean = False


	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_CurrentAddressData As EmployeeSAddressData
	Private m_CurrentEmployeeNumber As Integer
	Private m_CurrentRecordID As Integer?

#End Region


#Region "Public Properties"
	Public Property EmployeeNumber As Integer

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(_setting.MDData.MDNr, _setting.UserData.UserNr, String.Empty)
		If Not String.IsNullOrWhiteSpace(strStyleName) Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Dim connectionString As String = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		AddHandler lueGender.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePostcode.ButtonClick, AddressOf OnDropDown_ButtonClick

		Reset()

		TranslateControls()

	End Sub

#End Region


	''' <summary>
	''' Gets the selected data.
	''' </summary>
	Public ReadOnly Property SelectedRecord As EmployeeSAddressData
		Get
			Dim gvRP = TryCast(grdPrint.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), EmployeeSAddressData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


#Region "public Methodes"

	Public Sub LoadData()
		Dim success As Boolean = True

		m_CurrentEmployeeNumber = EmployeeNumber
		success = success AndAlso LoadDropDownData()
		success = success AndAlso LoadAddressData()

	End Sub

#End Region


#Region "private methodes"

	Sub Reset()

		BlankFilelds()
		epError.ClearErrors()

		ResetGenderDropDown()
		ResetCountryDropDown()
		ResetPostcodeDropDown()
		ResetAddressGrid()

	End Sub


	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	Private Sub BlankFilelds()

		m_CurrentRecordID = Nothing
		txtLastname.EditValue = String.Empty
		txtFirstname.EditValue = String.Empty
		txtCOAdress.EditValue = String.Empty
		txtPostOfficeBox.EditValue = String.Empty
		txtStreet.EditValue = String.Empty
		lueCountry.EditValue = String.Empty
		luePostcode.EditValue = String.Empty
		txtLocation.EditValue = String.Empty

		Me.chkAGB.Checked = False
		Me.chkDivers.Checked = False
		Me.chkEmployment.Checked = False
		Me.chkNLA.Checked = False
		Me.chkPayroll.Checked = False
		Me.chkRepport.Checked = False
		Me.chkZV.Checked = False
		Me.chk_AktivAdress.Checked = False

		txt_Bemerkung.EditValue = String.Empty
		txt_Add_Res1.EditValue = String.Empty
		txt_Add_Res2.EditValue = String.Empty
		txt_Add_Res3.EditValue = String.Empty

		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

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
	''' Resets the country drop down.
	''' </summary>
	Private Sub ResetCountryDropDown()

		lueCountry.Properties.ShowHeader = False
		lueCountry.Properties.ShowFooter = False
		lueCountry.Properties.DropDownRows = 20
		lueCountry.Properties.DisplayMember = "Translated_Value"
		lueCountry.Properties.ValueMember = "Code"

		Dim columns = lueCountry.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Translated_Value", 0, m_Translate.GetSafeTranslationValue("Land")))

		lueCountry.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueCountry.Properties.SearchMode = SearchMode.AutoComplete
		lueCountry.Properties.AutoSearchColumnIndex = 0

		lueCountry.Properties.NullText = String.Empty
		lueCountry.EditValue = Nothing

	End Sub

	''' <summary>
	''' Resets the postcode drop down.
	''' </summary>
	Private Sub ResetPostcodeDropDown()

		luePostcode.Properties.SearchMode = SearchMode.OnlyInPopup
		luePostcode.Properties.TextEditStyle = TextEditStyles.Standard

		luePostcode.Properties.DisplayMember = "Postcode"
		luePostcode.Properties.ValueMember = "Postcode"

		Dim columns = luePostcode.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("Postcode", 0, m_Translate.GetSafeTranslationValue("PLZ")))
		columns.Add(New LookUpColumnInfo("Location", 0, m_Translate.GetSafeTranslationValue("Ort")))

		luePostcode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePostcode.Properties.SearchMode = SearchMode.AutoComplete
		luePostcode.Properties.AutoSearchColumnIndex = 1
		luePostcode.Properties.NullText = String.Empty
		luePostcode.EditValue = Nothing
	End Sub

	Private Sub ResetAddressGrid()

		gvPrint.OptionsView.ShowIndicator = False
		gvPrint.OptionsView.ShowAutoFilterRow = True
		gvPrint.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvPrint.OptionsView.ShowFooter = False
		gvPrint.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvPrint.Columns.Clear()

		Dim columnDivAddressFullName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDivAddressFullName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDivAddressFullName.OptionsColumn.AllowEdit = False
		columnDivAddressFullName.Caption = m_Translate.GetSafeTranslationValue("Name, Vorname")
		columnDivAddressFullName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnDivAddressFullName.Name = "DivAddressFullName"
		columnDivAddressFullName.FieldName = "DivAddressFullName"
		columnDivAddressFullName.Visible = True
		columnDivAddressFullName.Width = 200
		gvPrint.Columns.Add(columnDivAddressFullName)

		Dim columnForEmployment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnForEmployment.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnForEmployment.OptionsColumn.AllowEdit = False
		columnForEmployment.Caption = m_Translate.GetSafeTranslationValue("Einsatzvertrag")
		columnForEmployment.Name = "ForEmployment"
		columnForEmployment.FieldName = "ForEmployment"
		columnForEmployment.Width = 50
		columnForEmployment.Visible = True
		gvPrint.Columns.Add(columnForEmployment)

		Dim columnForReport As New DevExpress.XtraGrid.Columns.GridColumn()
		columnForReport.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnForReport.OptionsColumn.AllowEdit = False
		columnForReport.Caption = m_Translate.GetSafeTranslationValue("Rapport")
		columnForReport.Name = "ForReport"
		columnForReport.FieldName = "ForReport"
		columnForReport.Visible = True
		columnForReport.Width = 50
		gvPrint.Columns.Add(columnForReport)

		Dim columnForPayroll As New DevExpress.XtraGrid.Columns.GridColumn()
		columnForPayroll.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnForPayroll.OptionsColumn.AllowEdit = False
		columnForPayroll.Caption = m_Translate.GetSafeTranslationValue("Lohnabrechnung")
		columnForPayroll.Name = "ForPayroll"
		columnForPayroll.FieldName = "ForPayroll"
		columnForPayroll.Visible = True
		columnForPayroll.Width = 50
		gvPrint.Columns.Add(columnForPayroll)

		Dim columnForAGB As New DevExpress.XtraGrid.Columns.GridColumn()
		columnForAGB.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnForAGB.OptionsColumn.AllowEdit = False
		columnForAGB.Caption = m_Translate.GetSafeTranslationValue("AGB")
		columnForAGB.Name = "ForAGB"
		columnForAGB.FieldName = "ForAGB"
		columnForAGB.Visible = True
		columnForAGB.Width = 50
		gvPrint.Columns.Add(columnForAGB)

		Dim columnForZV As New DevExpress.XtraGrid.Columns.GridColumn()
		columnForZV.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnForZV.OptionsColumn.AllowEdit = False
		columnForZV.Caption = m_Translate.GetSafeTranslationValue("ZV")
		columnForZV.Name = "ForZV"
		columnForZV.FieldName = "ForZV"
		columnForZV.Visible = True
		columnForZV.Width = 50
		gvPrint.Columns.Add(columnForZV)

		Dim columnForNLA As New DevExpress.XtraGrid.Columns.GridColumn()
		columnForNLA.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnForNLA.OptionsColumn.AllowEdit = False
		columnForNLA.Caption = m_Translate.GetSafeTranslationValue("Lohnausweis")
		columnForNLA.Name = "ForNLA"
		columnForNLA.FieldName = "ForNLA"
		columnForNLA.Visible = True
		columnForNLA.Width = 50
		gvPrint.Columns.Add(columnForNLA)

		Dim columnForDivers As New DevExpress.XtraGrid.Columns.GridColumn()
		columnForDivers.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnForDivers.OptionsColumn.AllowEdit = False
		columnForDivers.Caption = m_Translate.GetSafeTranslationValue("Vorlagen")
		columnForDivers.DisplayFormat.FormatString = "G"
		columnForDivers.Name = "ForDivers"
		columnForDivers.FieldName = "ForDivers"
		columnForDivers.Visible = True
		columnForDivers.Width = 50
		gvPrint.Columns.Add(columnForDivers)

		Dim columnActiveRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnActiveRec.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnActiveRec.OptionsColumn.AllowEdit = False
		columnActiveRec.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
		columnActiveRec.Name = "ActiveRec"
		columnActiveRec.FieldName = "ActiveRec"
		columnActiveRec.Visible = True
		columnActiveRec.Width = 50
		gvPrint.Columns.Add(columnActiveRec)


		grdPrint.DataSource = Nothing

	End Sub


	Sub TranslateControls()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
			Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

			Me.grpAdresse.Text = m_Translate.GetSafeTranslationValue(Me.grpAdresse.Text)
			Me.lblNachname.Text = m_Translate.GetSafeTranslationValue(Me.lblNachname.Text)
			Me.lblVorname.Text = m_Translate.GetSafeTranslationValue(Me.lblVorname.Text)
			Me.lblWohntbei.Text = m_Translate.GetSafeTranslationValue(Me.lblWohntbei.Text)
			Me.lblPostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblPostfach.Text)
			Me.lblStrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblStrasse.Text)
			Me.lblOrt.Text = m_Translate.GetSafeTranslationValue(Me.lblOrt.Text)
			Me.chk_AktivAdress.Text = m_Translate.GetSafeTranslationValue(Me.chk_AktivAdress.Text)

			Me.grpArt.Text = m_Translate.GetSafeTranslationValue(Me.grpArt.Text)
			Me.lblBemerkung1.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung1.Text, True)
			Me.lblBemerkung2.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung2.Text, True)
			Me.lblBemerkung3.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung3.Text, True)
			Me.lblBemerkung4.Text = m_Translate.GetSafeTranslationValue(Me.lblBemerkung4.Text, True)

			Dim showLbl1 As Boolean = lblBemerkung1.Text <> "lblEmployeeSAdressBemerk1"
			Dim showLbl2 As Boolean = lblBemerkung2.Text <> "lblEmployeeSAdressBemerk2"
			Dim showLbl3 As Boolean = lblBemerkung3.Text <> "lblEmployeeSAdressBemerk3"
			Dim showLbl4 As Boolean = lblBemerkung4.Text <> "lblEmployeeSAdressBemerk4"
			If Not showLbl1 AndAlso Not showLbl2 AndAlso Not showLbl3 AndAlso Not showLbl4 Then grpSonstige.Visible = False

			Me.grpArt.Text = m_Translate.GetSafeTranslationValue(Me.grpArt.Text)
			Me.chkAGB.Text = m_Translate.GetSafeTranslationValue(Me.chkAGB.Text)
			Me.chkDivers.Text = m_Translate.GetSafeTranslationValue(Me.chkDivers.Text)
			Me.chkEmployment.Text = m_Translate.GetSafeTranslationValue(Me.chkEmployment.Text)
			Me.chkNLA.Text = m_Translate.GetSafeTranslationValue(Me.chkNLA.Text)
			Me.chkPayroll.Text = m_Translate.GetSafeTranslationValue(Me.chkPayroll.Text)
			Me.chkRepport.Text = m_Translate.GetSafeTranslationValue(Me.chkRepport.Text)
			Me.chkZV.Text = m_Translate.GetSafeTranslationValue(Me.chkZV.Text)

			Me.lblErfasstDatensatz.Text = m_Translate.GetSafeTranslationValue(Me.lblErfasstDatensatz.Text)

			Me.bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(Me.bsiLblErstellt.Caption)
			Me.bsilblGeaendert.Caption = m_Translate.GetSafeTranslationValue(Me.bsilblGeaendert.Caption)

			Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
			Me.bbiNew.Caption = m_Translate.GetSafeTranslationValue(Me.bbiNew.Caption)
			Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmMAAddress_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			My.Settings.frmLocation = String.Empty
			My.Settings.iLeft = Me.Top
			My.Settings.iTop = Me.Left
			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub


#Region "Funktionen zur Menüaufbau..."

	Private Sub OnbbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick

		epError.ClearErrors()
		Dim bSave As Boolean = ValidatenputData()
		If Not bSave Then Exit Sub

		If SaveRecordData() Then
			Dim savedID As Integer = m_CurrentAddressData.ID
			Dim result = LoadAddressData()

			FocusAddress(savedID)

		End If
		m_SuppressUIEvents = False

	End Sub

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	Private Sub Onbbinew_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNew.ItemClick

		BlankFilelds()
		LoadEmployeeMasterData()

		Me.lueGender.Focus()

	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

		If m_CurrentAddressData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))
			Return
		End If
		If (m_UtilityUi.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																									m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
			Return
		End If

		If DeleteRecordData() Then
			Dim result = LoadAddressData()

		End If
		m_SuppressUIEvents = False

	End Sub


#End Region



	''' <summary>
	''' Loads the drop down data.
	''' </summary>
	Private Function LoadDropDownData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso LoadGenderDropDownData()
		success = success AndAlso LoadCountryDropDownData()
		success = success AndAlso LoadPostcodeDropDownData()

		Return success
	End Function

	''' <summary>
	''' Loads gender drop down data.
	''' </summary>
	Private Function LoadGenderDropDownData() As Boolean
		Dim genderData = m_CommonDatabaseAccess.LoadGenderData()

		If (genderData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Geschlechtsdaten konnten nicht geladen werden."))
		End If

		lueGender.Properties.DataSource = genderData
		lueGender.Properties.ForceInitialize()

		Return Not genderData Is Nothing
	End Function

	''' <summary>
	''' Loads the country drop down data.
	''' </summary>
	Private Function LoadCountryDropDownData() As Boolean
		Dim result As Boolean = True
		Dim countryData As IEnumerable(Of CVLBaseTableViewData) = Nothing

		Try
			Dim baseTable = New SPSBaseTables(m_InitializationData)
			baseTable.BaseTableName = "Country"
			countryData = baseTable.PerformCVLBaseTablelistWebserviceCall()

			If (countryData Is Nothing) Then
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Länderdaten konnten nicht geladen werden."))
			End If

			lueCountry.Properties.DataSource = countryData
			lueCountry.Properties.ForceInitialize()

		Catch ex As Exception

		End Try

		'Dim countryData = m_CommonDatabaseAccess.LoadCountryData()
		Return Not countryData Is Nothing
	End Function

	''' <summary>
	''' Loads the postcode drop downdata.
	''' </summary>
	Private Function LoadPostcodeDropDownData() As Boolean
		Dim postcodeData = m_CommonDatabaseAccess.LoadPostcodeData()

		If (postcodeData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Postleizahldaten konnten nicht geladen werden."))
		End If

		luePostcode.Properties.DataSource = postcodeData
		luePostcode.Properties.ForceInitialize()

		Return Not postcodeData Is Nothing
	End Function

	Private Function LoadAddressData() As Boolean

		Dim success As Boolean = True

		Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeDivAddressData(m_CurrentEmployeeNumber)

		If (employeeMasterData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In employeeMasterData
										Select New EmployeeSAddressData With {.ActiveRec = person.ActiveRec,
																													.Add_Bemerkung = person.Add_Bemerkung,
																													.Add_Res1 = person.Add_Res1,
																													.Add_Res2 = person.Add_Res2,
																													.Add_Res3 = person.Add_Res3,
																													.ChangedFrom = person.ChangedFrom,
																													.ChangedOn = person.ChangedOn,
																													.Country = person.Country,
																													.Createdfrom = person.Createdfrom,
																													.Createdon = person.Createdon,
																													.employeeNumber = person.employeeNumber,
																													.Firstname = person.Firstname,
																													.ForAGB = person.ForAGB,
																													.ForDivers = person.ForDivers,
																													.ForEmployment = person.ForEmployment,
																													.ForNLA = person.ForNLA,
																													.ForPayroll = person.ForPayroll,
																													.ForReport = person.ForReport,
																													.ForZV = person.ForZV,
																													.Gender = person.Gender,
																													.ID = person.ID,
																													.RecNr = person.RecNr,
																													.Lastname = person.Lastname,
																													.Location = person.Location,
																													.Postcode = person.Postcode,
																													.PostOfficeBox = person.PostOfficeBox,
																													.StaysAt = person.StaysAt,
																													.Street = person.Street
																												 }).ToList()

		Dim listDataSource As BindingList(Of EmployeeSAddressData) = New BindingList(Of EmployeeSAddressData)

		Dim supressUIEventState = m_SuppressUIEvents
		m_SuppressUIEvents = True
		grdPrint.DataSource = Nothing
		m_SuppressUIEvents = supressUIEventState

		For Each p In gridData
			listDataSource.Add(p)
		Next
		grdPrint.DataSource = listDataSource

		Return Not listDataSource Is Nothing

	End Function

	Private Function LoadEmployeeMasterData() As Boolean

		Dim success As Boolean = True

		Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(EmployeeNumber, False)

		If (employeeMasterData Is Nothing) Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
			Return False
		End If

		' Address
		grpAdresse.Text = String.Format(m_Translate.GetSafeTranslationValue("Neuer Datensatz für Kandidat: {0}"), employeeMasterData.EmployeeNumber)
		lueGender.EditValue = employeeMasterData.Gender
		txtLastname.Text = employeeMasterData.Lastname
		txtFirstname.Text = employeeMasterData.Firstname
		txtCOAdress.Text = employeeMasterData.StaysAt
		txtPostOfficeBox.Text = employeeMasterData.PostOfficeBox
		txtStreet.Text = employeeMasterData.Street
		lueCountry.EditValue = employeeMasterData.Country

		' Add missing post code to drop down
		Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

		If Not String.IsNullOrEmpty(employeeMasterData.Postcode) AndAlso
			Not listOfPostcode.Any(Function(postcode) postcode.Postcode = employeeMasterData.Postcode) Then
			Dim newPostcode As New PostCodeData With {.Postcode = employeeMasterData.Postcode}
			listOfPostcode.Add(newPostcode)
		End If

		luePostcode.EditValue = employeeMasterData.Postcode

		txtLocation.Text = employeeMasterData.Location

		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

	End Function


#End Region

	''' <summary>
	''' Handles change of postcode.
	''' </summary>
	Private Sub OnLuePostcode_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles luePostcode.EditValueChanged

		Dim postCodeData As PostCodeData = TryCast(luePostcode.GetSelectedDataRow(), PostCodeData)

		If Not postCodeData Is Nothing Then
			txtLocation.Text = postCodeData.Location
		End If

	End Sub

	''' <summary>
	''' Handles new value event on postcode(plz) lookup edit.
	''' </summary>
	Private Sub OnLuePostcode_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles luePostcode.ProcessNewValue

		If Not luePostcode.Properties.DataSource Is Nothing Then

			Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

			Dim newPostcode As New PostCodeData With {.Postcode = e.DisplayValue.ToString()}
			listOfPostcode.Add(newPostcode)

			e.Handled = True
		End If
	End Sub


	''' <summary>
	''' Handles focus change of bank row.
	''' </summary>
	Private Sub OngvPrint_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvPrint.FocusedRowChanged

		If m_SuppressUIEvents Then
			Return
		End If

		m_CurrentAddressData = SelectedRecord
		If m_CurrentAddressData Is Nothing Then
			'm_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
			Return
		Else
			DisplayDetailAddressData()
		End If

	End Sub

	''' <summary>
	''' Handles focus click of row.
	''' </summary>
	Private Sub OngrdPrint_DoubleClick(sender As System.Object, e As System.EventArgs) Handles grdPrint.DoubleClick

		BlankFilelds()
		Dim data = SelectedRecord
		m_CurrentAddressData = data
		If data Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))
			Return
		End If
		DisplayDetailAddressData()

	End Sub

	Private Sub DisplayDetailAddressData()

		m_CurrentRecordID = m_CurrentAddressData.ID
		grpAdresse.Text = String.Format(m_Translate.GetSafeTranslationValue("Adress-Nummer: {0}"), m_CurrentAddressData.RecNr)
		lueGender.EditValue = m_CurrentAddressData.Gender
		txtLastname.EditValue = m_CurrentAddressData.Lastname
		txtFirstname.EditValue = m_CurrentAddressData.Firstname
		txtCOAdress.EditValue = m_CurrentAddressData.StaysAt
		txtPostOfficeBox.EditValue = m_CurrentAddressData.PostOfficeBox
		txtStreet.EditValue = m_CurrentAddressData.Street
		lueCountry.EditValue = m_CurrentAddressData.Country

		' Add missing post code to drop down
		Dim listOfPostcode = CType(luePostcode.Properties.DataSource, List(Of PostCodeData))

		If Not String.IsNullOrEmpty(m_CurrentAddressData.Postcode) AndAlso
			Not listOfPostcode.Any(Function(postcode) postcode.Postcode = m_CurrentAddressData.Postcode) Then
			Dim newPostcode As New PostCodeData With {.Postcode = m_CurrentAddressData.Postcode}
			listOfPostcode.Add(newPostcode)
		End If

		luePostcode.EditValue = m_CurrentAddressData.Postcode
		txtLocation.EditValue = m_CurrentAddressData.Location

		txt_Bemerkung.EditValue = m_CurrentAddressData.Add_Bemerkung
		txt_Add_Res1.EditValue = m_CurrentAddressData.Add_Res1
		txt_Add_Res2.EditValue = m_CurrentAddressData.Add_Res2
		txt_Add_Res3.EditValue = m_CurrentAddressData.Add_Res3

		chkEmployment.Checked = m_CurrentAddressData.ForEmployment
		chkRepport.Checked = m_CurrentAddressData.ForReport
		chkAGB.Checked = m_CurrentAddressData.ForAGB
		chkZV.Checked = m_CurrentAddressData.ForZV
		chkPayroll.Checked = m_CurrentAddressData.ForPayroll
		chkNLA.Checked = m_CurrentAddressData.ForNLA
		chkDivers.Checked = m_CurrentAddressData.ForDivers
		chk_AktivAdress.Checked = m_CurrentAddressData.ActiveRec

		bsiCreated.Caption = String.Format("{0:G}, {1}", m_CurrentAddressData.Createdon, m_CurrentAddressData.Createdfrom)
		bsiChanged.Caption = String.Format("{0:G}, {1}", m_CurrentAddressData.ChangedOn, m_CurrentAddressData.ChangedFrom)

	End Sub

	Private Function SaveRecordData() As Boolean

		Dim addressData As New EmployeeSAddressData
		Dim existingData = SelectedRecord

		addressData.ID = m_CurrentRecordID
		If Not m_CurrentRecordID Is Nothing Then
			If existingData Is Nothing Then
				addressData.RecNr = Nothing
			Else
				addressData.RecNr = existingData.RecNr
			End If
		End If

		addressData.ActiveRec = chk_AktivAdress.Checked
		addressData.ForAGB = chkAGB.Checked
		addressData.ForDivers = chkDivers.Checked
		addressData.ForEmployment = chkEmployment.Checked
		addressData.ForNLA = chkNLA.Checked
		addressData.ForPayroll = chkPayroll.Checked
		addressData.ForReport = chkRepport.Checked
		addressData.ForZV = chkZV.Checked

		addressData.Add_Bemerkung = txt_Bemerkung.EditValue
		addressData.Add_Res1 = txt_Add_Res1.EditValue
		addressData.Add_Res2 = txt_Add_Res2.EditValue
		addressData.Add_Res3 = txt_Add_Res3.EditValue

		addressData.Country = lueCountry.EditValue
		addressData.StaysAt = txtCOAdress.EditValue
		addressData.Firstname = txtFirstname.EditValue
		addressData.Lastname = txtLastname.EditValue
		addressData.Gender = lueGender.EditValue
		addressData.Location = txtLocation.EditValue
		addressData.Postcode = luePostcode.EditValue
		addressData.PostOfficeBox = txtPostOfficeBox.EditValue
		addressData.Street = txtStreet.EditValue

		Dim success = True
		If m_CurrentRecordID.HasValue Then
			addressData.ChangedFrom = m_InitializationData.UserData.UserFullName
			success = success AndAlso m_EmployeeDatabaseAccess.UpdateEmployeeDivAddressData(m_CurrentEmployeeNumber, addressData)
		Else
			addressData.Createdfrom = m_InitializationData.UserData.UserFullName
			success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeDivAddressData(m_CurrentEmployeeNumber, addressData)
		End If

		Dim msg As String
		If success Then
			msg = "Ihre Daten wurden erfolgreich aktualisiert."
			m_UtilityUi.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))
			m_CurrentAddressData = addressData
		Else
			msg = "Die Daten konnten nicht gespeichert werden."
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
		End If


		Return success

	End Function

	Private Function DeleteRecordData() As Boolean

		If m_CurrentAddressData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden ausgewählt."))
			Return False
		End If
		Dim success = m_EmployeeDatabaseAccess.DeleteEmployeeDivAddressData(m_CurrentAddressData.ID)

		Dim msg As String
		If success Then
			msg = "Die Daten wurde erfolgreich gelöscht."
			m_UtilityUi.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)

		Else
			msg = "Die Daten konnten nicht gelöscht werden."
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))
		End If


		Return success

	End Function

	''' <summary>
	''' Focuses a record.
	''' </summary>
	Private Sub FocusAddress(ByVal recID As Integer?)

		If grdPrint.DataSource Is Nothing Then Return

		Dim listDataSource As BindingList(Of EmployeeSAddressData) = grdPrint.DataSource
		Dim viewData = listDataSource.Where(Function(data) data.employeeNumber = m_CurrentEmployeeNumber AndAlso data.ID = recID).FirstOrDefault()
		'Dim index = listDataSource.ToList().FindIndex(Function(data) data.employeeNumber = m_CurrentEmployeeNumber AndAlso data.ID = recID)

		If Not viewData Is Nothing Then
			Dim sourceIndex = listDataSource.IndexOf(viewData)
			Dim rowHandle = gvPrint.GetRowHandle(sourceIndex)
			gvPrint.FocusedRowHandle = rowHandle


			'Dim rowHandle = gvPrint.GetRowHandle(index)
			'gvPrint.FocusedRowHandle = rowHandle


		End If

	End Sub

	''' <summary>
	''' Validates input data.
	''' </summary>
	Private Function ValidatenputData() As Boolean

		Dim missingFieldText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
		Dim missingFieldArt As String = m_Translate.GetSafeTranslationValue("Bitte definieren Sie eine Art.")

		Dim isValid As Boolean = True

		' may be is an organisation like "Gemeinde Gipf-Oberfrickfrick
		'isValid = isValid And SetErrorIfInvalid(lueGender, epError, lueGender.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueGender.EditValue), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(txtLastname, epError, txtLastname.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(txtLastname.EditValue), missingFieldText)
		'isValid = isValid And SetErrorIfInvalid(txtFirstname, epError, txtFirstname.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(txtFirstname.EditValue), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(luePostcode, epError, luePostcode.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(luePostcode.EditValue), missingFieldText)
		isValid = isValid And SetErrorIfInvalid(lueCountry, epError, lueCountry.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueCountry.EditValue), missingFieldText)

		If Not chkAGB.Checked AndAlso Not chkDivers.Checked AndAlso Not chkEmployment.Checked AndAlso Not chkNLA.Checked AndAlso Not chkPayroll.Checked AndAlso Not chkRepport.Checked AndAlso Not chkZV.Checked Then
			isValid = isValid And SetErrorIfInvalid(chkEmployment, epError, True, missingFieldArt)
		End If


		Return isValid
	End Function

	Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

		If (invalid) Then
			errorProvider.SetError(control, errorText)
		Else
			errorProvider.SetError(control, String.Empty)
		End If

		Return Not invalid

	End Function

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

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


End Class

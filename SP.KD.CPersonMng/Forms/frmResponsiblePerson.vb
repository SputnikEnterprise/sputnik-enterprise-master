
Imports System.Reflection.Assembly

Imports SPS.Listing.Print.Utility

Imports SP.KD.CPersonMng.UI
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Settings
Imports SP.KD.CPersonMng.Settings
Imports DevExpress.LookAndFeel

Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.TodoMng
Imports System.ComponentModel


Namespace UI

	Public Delegate Sub ResponsiblePersonDataSavedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer)
	Public Delegate Sub ResponsiblePersonDataDeletedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer)

	Public Class frmResponsiblePerson

#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' The active user control.
		''' </summary>
		Private m_ActiveUserControl As ucBaseControl

		''' <summary>
		''' List of tab controls.
		''' </summary>
		Private m_ListOfTabControls As New List(Of ucBaseControl)

		''' <summary>
		''' The data access object.
		''' </summary>
		Private m_DataAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The settings manager.
		''' </summary>
		Protected m_SettingsManager As ISettingsManager

		''' <summary>
		''' Contains the customer number of the loaded customer.
		''' </summary>
		Private m_CustomerNumber As Integer

		''' <summary>
		''' Contains the record number of the loaded responsible person.
		''' </summary>
		Protected m_CurrentRecordNumber As Integer?

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Protected m_UtilityUI As UtilityUI

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' Boolean flag indicating if form is initializing.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = True

		''' <summary>
		''' Boolan flag indicating if the form has been initialized.
		''' </summary>
		Private m_IsInitialized = False

		Private m_md As Mandant
		Private m_common As CommonSetting
		Private m_AllowedDesign As Boolean


#End Region

#Region "Events"

		Public Event ResponsiblePersonDataSaved As ResponsiblePersonDataSavedHandler
		Public Event ResponsiblePersonDataDeleted As ResponsiblePersonDataDeletedHandler

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
			Try
				' Mandantendaten
				m_md = New Mandant
				m_common = New CommonSetting
				m_InitializationData = _setting
				m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try
			m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 612, m_InitializationData.MDData.MDNr)

			Me.gvResponsiblePersons.OptionsView.ShowIndicator = False
			Me.gvResponsiblePersons.OptionsView.ShowAutoFilterRow = True

			' First page is active.
			m_ActiveUserControl = ucCommonData

			m_ListOfTabControls.Add(ucCommonData)
			m_ListOfTabControls.Add(ucDisposal)
			m_ListOfTabControls.Add(ucProfessionsAndSectors)
			m_ListOfTabControls.Add(ucContactData)
			m_ListOfTabControls.Add(ucDocumentManagement)

			' Init sub controls with configuration information
			For Each ctrl In m_ListOfTabControls
				ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
			Next


			m_DataAccess = New SP.DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_SettingsManager = New SettingsManager

			m_UtilityUI = New UtilityUI

			TranslateControls()


		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected responsible person overview data.
		''' </summary>
		''' <returns>The selected responsible person overview data or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedResponsiblePersonOverViewData As ResponsiblePersonOverviewData	'ResponsiblePersonOverviewDataForPersonManagement
			Get
				Dim grdView = TryCast(gridResponsiblePersons.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim overviewData = CType(grdView.GetRow(selectedRows(0)), ResponsiblePersonOverviewData) ' ResponsiblePersonOverviewDataForPersonManagement)
						Return overviewData
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Loads the data of a responsible person.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <param name="recordNumber">The record number of the responsible person. If the value is nothing then the mask will allow to insert a new person.</param>
		''' <returns>Boolean value indicating success.</returns>
		Public Function LoadResponsiblePersonData(ByVal customerNumber As Integer, ByVal recordNumber As Integer?) As Boolean

			If Not m_IsInitialized Then
				Reset()
				m_IsInitialized = True
			End If

			Dim success = True

			If (recordNumber.HasValue) Then

				success = success AndAlso m_ActiveUserControl.Activate(customerNumber, recordNumber)
				success = success AndAlso LoadResponsiblePersonOverviewData(customerNumber, recordNumber)

				m_CustomerNumber = IIf(success, customerNumber, 0)
				m_CurrentRecordNumber = IIf(success, recordNumber, Nothing)
			Else
				success = success AndAlso LoadResponsiblePersonOverviewData(customerNumber, recordNumber)
				m_CustomerNumber = IIf(success, customerNumber, 0)
				m_CurrentRecordNumber = Nothing
				success = success AndAlso PrepareToEnterNewResponsiblePerson()
			End If

			Return success

		End Function
#End Region

#Region "Private Methods"

		''' <summary>
		'''  trannslate controls
		''' </summary>
		''' <remarks></remarks>
		Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			xtabCommonData.Text = m_Translate.GetSafeTranslationValue(xtabCommonData.Text)
			xtabContacts.Text = m_Translate.GetSafeTranslationValue(xtabContacts.Text)
			xtabDispositionData.Text = m_Translate.GetSafeTranslationValue(xtabDispositionData.Text)
			xtabDocuments.Text = m_Translate.GetSafeTranslationValue(xtabDocuments.Text)
			xtabProfessionsAndSectorsData.Text = m_Translate.GetSafeTranslationValue(xtabProfessionsAndSectorsData.Text)

			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)
			bbiNew.Caption = m_Translate.GetSafeTranslationValue(bbiNew.Caption)
			bbiTODO.Caption = m_Translate.GetSafeTranslationValue(bbiTODO.Caption)
			bbiPrint.Caption = m_Translate.GetSafeTranslationValue(bbiPrint.Caption)
			bbiDelete.Caption = m_Translate.GetSafeTranslationValue(bbiDelete.Caption)

			grpZustaendigeperson.Text = m_Translate.GetSafeTranslationValue(grpZustaendigeperson.Text)

			lbLstlInfo.Text = m_Translate.GetSafeTranslationValue(lbLstlInfo.Text)
			bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
			bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsiLblGeaendert.Caption)

		End Sub

		''' <summary>
		''' Resets the from.
		''' </summary>
		Private Sub Reset()
			m_SuppressUIEvents = True
			m_CustomerNumber = 0
			m_CurrentRecordNumber = Nothing

			' Reset child panels
			For Each tabControl In m_ListOfTabControls
				tabControl.Reset()
			Next

			'' Activate first page
			'm_ActiveUserControl.Deactivate()
			'm_ActiveUserControl = ucCommonData
			'XtraTabControl1.SelectedTabPage = xtabCommonData

			' Show all tabs
			xtabCommonData.PageVisible = True
			xtabDispositionData.PageVisible = True
			xtabProfessionsAndSectorsData.PageVisible = True
			xtabContacts.PageVisible = True
			xtabDocuments.PageVisible = True

			ResetResponsiblePersonsOverviewGrid()

			bbiDelete.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 233)
			bbiPrint.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 602)

			m_SuppressUIEvents = False
		End Sub

		''' <summary>
		''' Resets the responsible persons overview grid.
		''' </summary>
		Private Sub ResetResponsiblePersonsOverviewGrid()

			gvResponsiblePersons.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvResponsiblePersons.OptionsView.ShowIndicator = False
			gvResponsiblePersons.OptionsBehavior.Editable = True
			gvResponsiblePersons.OptionsView.ShowAutoFilterRow = True
			gvResponsiblePersons.OptionsView.ColumnAutoWidth = True
			gvResponsiblePersons.OptionsView.ShowFooter = False
			gvResponsiblePersons.OptionsView.AllowHtmlDrawGroups = True

			' Reset the grid
			gvResponsiblePersons.Columns.Clear()


			Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsSelected.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsSelected.OptionsColumn.AllowEdit = True
			columnIsSelected.Caption = m_Translate.GetSafeTranslationValue(" ")
			columnIsSelected.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
			columnIsSelected.Name = "IsSelected"
			columnIsSelected.FieldName = "IsSelected"
			columnIsSelected.Visible = True
			columnIsSelected.Width = 10
			gvResponsiblePersons.Columns.Add(columnIsSelected)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnName.OptionsColumn.AllowEdit = False
			columnName.Name = "Name"
			columnName.FieldName = "Name"
			columnName.Visible = True
			gvResponsiblePersons.Columns.Add(columnName)

			Dim columnTelephone As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTelephone.Caption = m_Translate.GetSafeTranslationValue("Telefon")
			columnTelephone.OptionsColumn.AllowEdit = False
			columnTelephone.Name = "Telephone"
			columnTelephone.FieldName = "Telephone"
			columnTelephone.Visible = True
			gvResponsiblePersons.Columns.Add(columnTelephone)

			m_SuppressUIEvents = True
			gridResponsiblePersons.DataSource = Nothing
			m_SuppressUIEvents = False

		End Sub


		''' <summary>
		''' Loads responsible person overview data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadResponsiblePersonOverviewData(ByVal customerNumber As Integer, ByVal recordNumberToSelect As Integer?) As Boolean

			Dim overviewData = m_DataAccess.LoadResponsiblePersonsOverviewDataForPersonManagement(customerNumber)

			If (overviewData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zuständige Personen konnten nicht geladen werden."))
				Return False
			End If

			Dim gridData = (From person In overviewData
											Select New ResponsiblePersonOverviewData With {.ID = person.ID,
																																					.ChangedFrom = person.ChangedFrom,
																																					.ChangedOn = person.ChangedOn,
																																					.CreatedFrom = person.CreatedFrom,
																																					.CreatedOn = person.CreatedOn,
																																					.CustomerNumber = person.CustomerNumber,
																																					.Name = person.Name,
																																					.RecordNumber = person.RecordNumber,
																																					.Telephone = person.Telephone,
																																					.IsSelected = False,
																																		 .ZState1 = person.ZState1,
																																		 .ZState2 = person.ZState2
																																				 }).ToList()

			Dim listDataSource As BindingList(Of ResponsiblePersonOverviewData) = New BindingList(Of ResponsiblePersonOverviewData)
			For Each p In gridData
				listDataSource.Add(p)
			Next

			m_SuppressUIEvents = True
			gridResponsiblePersons.DataSource = listDataSource

			If Not recordNumberToSelect Is Nothing Then
				Dim index = listDataSource.ToList().FindIndex(Function(data) data.RecordNumber = recordNumberToSelect)
				gvResponsiblePersons.FocusedRowHandle = gvResponsiblePersons.GetRowHandle(index)
			ElseIf listDataSource.Count > 0 Then
				gvResponsiblePersons.FocusedRowHandle = gvResponsiblePersons.GetVisibleRowHandle(0)
			End If

			UpdateResponsiblePersonCreatedAndChangedTexts(SelectedResponsiblePersonOverViewData)

			m_SuppressUIEvents = False

			Return True
		End Function

		''' <summary>
		''' Upades the created and changed text.
		''' </summary>
		''' <param name="responsiblePersonOverView">Thre responsible person overview data.</param>
		Private Sub UpdateResponsiblePersonCreatedAndChangedTexts(ByVal responsiblePersonOverView As ResponsiblePersonOverviewData)	'ResponsiblePersonOverviewDataForPersonManagement)

			If Not responsiblePersonOverView Is Nothing Then
				bsiCreated.Caption = String.Format("{0:f}, {1}", responsiblePersonOverView.CreatedOn, responsiblePersonOverView.CreatedFrom)
				bsiChanged.Caption = String.Format("{0:f}, {1}", responsiblePersonOverView.ChangedOn, responsiblePersonOverView.ChangedFrom)
			Else
				bsiCreated.Caption = "-"
				bsiChanged.Caption = "-"
			End If

		End Sub

		''' <summary>
		''' Prepare to enter a new responsible person
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function PrepareToEnterNewResponsiblePerson() As Boolean
			m_SuppressUIEvents = True

			m_ActiveUserControl.Deactivate()
			m_ActiveUserControl = ucCommonData
			XtraTabControl1.SelectedTabPage = xtabCommonData

			xtabDispositionData.PageVisible = False
			xtabProfessionsAndSectorsData.PageVisible = False
			xtabContacts.PageVisible = False
			xtabDocuments.PageVisible = False

			m_CurrentRecordNumber = Nothing
			m_SuppressUIEvents = False

			Return ucCommonData.PrepareToEnterNewResponsiblePerson(m_CustomerNumber)

		End Function

		''' <summary>
		''' Saves responsible person data.
		''' </summary>
		Private Sub SaveResponsiblePersonData()

			If (ValidateData()) Then

				Dim responsiblePersonMasterData As ResponsiblePersonMasterData = Nothing

				Dim dt = DateTime.Now
				If Not m_CurrentRecordNumber.HasValue Then

					responsiblePersonMasterData = New ResponsiblePersonMasterData With {
							.CustomerNumber = m_CustomerNumber,
							.CreatedOn = dt,
							.CreatedFrom = m_common.GetLogedUserNameWithComma()}

					ucCommonData.MergeResponsiblePersonMasterData(responsiblePersonMasterData, True)
				Else

					responsiblePersonMasterData = m_DataAccess.LoadResponsiblePersonMasterData(m_CustomerNumber, m_CurrentRecordNumber)

					If responsiblePersonMasterData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
						Return
					End If

					ucCommonData.MergeResponsiblePersonMasterData(responsiblePersonMasterData)

				End If

				responsiblePersonMasterData.ChangedFrom = m_common.GetLogedUserNameWithComma()
				responsiblePersonMasterData.ChangedOn = dt

				' Update the responsible person data.
				Dim success As Boolean = True

				If responsiblePersonMasterData.ID = 0 Then
					success = m_DataAccess.AddNewResponsiblePerson(responsiblePersonMasterData)

					If success Then
						xtabDispositionData.PageVisible = True
						xtabProfessionsAndSectorsData.PageVisible = True
						xtabContacts.PageVisible = True
						xtabDocuments.PageVisible = True
					End If

				Else
					success = m_DataAccess.UpdateResponsiblePersonMasterData(responsiblePersonMasterData)
				End If

				Dim message As String = String.Empty

				m_CurrentRecordNumber = responsiblePersonMasterData.RecordNumber
				ucCommonData.Activate(m_CustomerNumber, m_CurrentRecordNumber)

				If (success) Then
					LoadResponsiblePersonOverviewData(responsiblePersonMasterData.CustomerNumber, responsiblePersonMasterData.RecordNumber)
					m_UtilityUI.ShowInfoDialog((m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")), m_Translate.GetSafeTranslationValue("Daten speichern"))

					RaiseEvent ResponsiblePersonDataSaved(Me, m_CustomerNumber, m_CurrentRecordNumber)

				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
				End If

			End If
		End Sub

		''' <summary>
		''' Deletes the selected responsible person.
		''' </summary>
		Private Sub DeleteSelectedResponsiblePerson()

			Dim selectedResponsiblePerson = SelectedResponsiblePersonOverViewData

			If selectedResponsiblePerson Is Nothing Then
				Return
			End If
			Dim responsibleName As String = String.Format("{0}", selectedResponsiblePerson.Name)
			If (m_UtilityUI.ShowYesNoDialog(String.Format(m_Translate.GetSafeTranslationValue("Möchten Sie die ausgewählte Person wirklich löschen?{0}{1}"), vbNewLine, responsibleName),
																			m_Translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then

				Dim success = m_DataAccess.DeleteResponsiblePerson(selectedResponsiblePerson.ID, ConstantValues.ModulName, m_common.GetLogedUserNameWithComma, m_common.GetLogedUserNr)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die zuständige Person konnte nicht gelöscht werden."))
				Else
					RaiseEvent ResponsiblePersonDataDeleted(Me, m_CustomerNumber, m_CurrentRecordNumber)
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Achtung: Die Kontakteinträge der Zuständigen Person wurden auf Hauptkunde übernommen."))
				End If

				LoadResponsiblePersonData(m_CustomerNumber, m_CurrentRecordNumber)
			End If
		End Sub

		''' <summary>
		''' Validates the data on the tabs.
		''' </summary>
		Public Function ValidateData() As Boolean

			Dim valid As Boolean = True
			For Each tabControl In m_ListOfTabControls

				If m_CustomerNumber = tabControl.CustomerNumber AndAlso
					 m_CurrentRecordNumber = tabControl.RecordNumber Then
					' Only validate tabs with the correct customer number and record number.
					valid = valid AndAlso tabControl.ValidateData()
				Else
					' Skip
				End If

			Next

			Return valid

		End Function

		''' <summary>
		''' Handles change of focues responsible person row.
		''' </summary>
		Private Sub OnGvResponsiblePersons_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvResponsiblePersons.FocusedRowChanged

			If (m_SuppressUIEvents) Then
				Return
			End If

			xtabDispositionData.PageVisible = True
			xtabProfessionsAndSectorsData.PageVisible = True
			xtabContacts.PageVisible = True
			xtabDocuments.PageVisible = True

			Dim selecteResponsiblePerson = SelectedResponsiblePersonOverViewData

			If Not selecteResponsiblePerson Is Nothing Then

				m_ActiveUserControl.Activate(selecteResponsiblePerson.CustomerNumber, selecteResponsiblePerson.RecordNumber)
				m_CurrentRecordNumber = selecteResponsiblePerson.RecordNumber

				UpdateResponsiblePersonCreatedAndChangedTexts(selecteResponsiblePerson)
			End If

		End Sub

		''' <summary>
		'''  Handles RowStyle event of gvZHDName grid view.
		''' </summary>
		Private Sub OngvResponsiblePersons_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvResponsiblePersons.RowStyle

			If e.RowHandle >= 0 Then

				Dim rowData = CType(gvResponsiblePersons.GetRow(e.RowHandle), ResponsiblePersonOverviewData)

				If Not rowData.IsZHDActiv.GetValueOrDefault(True) Then
					e.Appearance.BackColor = Color.LightGray
					e.Appearance.BackColor2 = Color.LightGray
				End If

			End If

		End Sub

		''' <summary>
		''' Handles keydown on reponsible persons grid.
		''' </summary>
		Private Sub OnGridReponsiblePersons_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gridResponsiblePersons.KeyDown

			If Not m_CurrentRecordNumber.HasValue Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then

				DeleteSelectedResponsiblePerson()

			End If
		End Sub

		''' <summary>
		''' Handles click on save button.
		''' </summary>
		Private Sub OnbbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
			SaveResponsiblePersonData()
		End Sub

		''' <summary>
		''' Handles click on delete responsible person button.
		''' </summary>
		Private Sub OnbbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
			DeleteSelectedResponsiblePerson()
		End Sub

		''' <summary>
		''' Handles click on new responsible person button.
		''' </summary>
		Private Sub OnbbiNew_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNew.ItemClick
			PrepareToEnterNewResponsiblePerson()
		End Sub

		''' <summary>
		''' Handles tab control selection changing.
		''' </summary>
		Private Sub OnxtraTabControl_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles XtraTabControl1.SelectedPageChanging

			If (m_SuppressUIEvents) Then
				Return
			End If

			Dim page = e.Page

			If Not (m_ActiveUserControl Is Nothing) Then
				m_ActiveUserControl.Deactivate()
			End If

			If (Object.ReferenceEquals(page, xtabCommonData)) Then
				ucCommonData.Activate(m_CustomerNumber, m_CurrentRecordNumber)
				m_ActiveUserControl = ucCommonData
			ElseIf (Object.ReferenceEquals(page, xtabDispositionData)) Then
				ucDisposal.Activate(m_CustomerNumber, m_CurrentRecordNumber)
				m_ActiveUserControl = ucDisposal
			ElseIf (Object.ReferenceEquals(page, xtabProfessionsAndSectorsData)) Then
				ucProfessionsAndSectors.Activate(m_CustomerNumber, m_CurrentRecordNumber)
				m_ActiveUserControl = ucProfessionsAndSectors
			ElseIf (Object.ReferenceEquals(page, xtabContacts)) Then
				ucContactData.Activate(m_CustomerNumber, m_CurrentRecordNumber)
				m_ActiveUserControl = ucContactData
			ElseIf (Object.ReferenceEquals(page, xtabDocuments)) Then
				ucDocumentManagement.Activate(m_CustomerNumber, m_CurrentRecordNumber)
				m_ActiveUserControl = ucDocumentManagement
			End If

		End Sub


		Private Sub OnbbiTODO_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiTODO.ItemClick

			If Not m_CurrentRecordNumber Is Nothing Then

				Dim frmTodo As New frmTodo(m_InitializationData)
				' optional init new todo
				Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
				Dim EmployeeNumber As Integer? = Nothing
				Dim CustomerNumber As Integer? = m_CustomerNumber
				Dim ResponsiblePersonRecordNumber As Integer? = m_CurrentRecordNumber
				Dim VacancyNumber As Integer? = Nothing
				Dim ProposeNumber As Integer? = Nothing
				Dim ESNumber As Integer? = Nothing
				Dim RPNumber As Integer? = Nothing
				Dim LMNumber As Integer? = Nothing
				Dim RENumber As Integer? = Nothing
				Dim ZENumber As Integer? = Nothing
				Dim Subject As String = String.Empty
				Dim Body As String = ""

				frmTodo.CustomerNumber = m_CustomerNumber
				frmTodo.ResponsiblePersonRecordNumber = m_CurrentRecordNumber
				frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber,
														VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

				frmTodo.Show()

			End If

		End Sub

		Private Sub OnbbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

			If Not m_CurrentRecordNumber Is Nothing Then

				Dim recordNumbers = New List(Of Integer)()

				Dim data = GetSelectedResponsibleData()
				For Each itm In data
					recordNumbers.Add(itm.RecordNumber)
				Next
				If recordNumbers.Count = 0 Then
					recordNumbers.Add(m_CurrentRecordNumber)
				End If


				Try
					Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
					Dim _settring As New ClsLLKDZHDSearchPrintSetting With {.DbConnString2Open = "",
																																	.ShowAsDesign = ShowDesign,
																																	.CustomerNumber = m_CustomerNumber,
																																	.CReocordNumbers = recordNumbers,
																																	.SelectedMDNr = m_InitializationData.MDData.MDNr,
																																	.LogedUSNr = m_InitializationData.UserData.UserNr,
																																	.JobNr2Print = "2.1.1"}

					Dim obj As New SPS.Listing.Print.Utility.KDZHDSearchListing.ClsPrintKDZHDSearchList(_settring)
					obj.PrintZHDListing(New List(Of String)({""}))


				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
					m_UtilityUI.ShowErrorDialog(ex.ToString)

				End Try

			End If

		End Sub

		Private Sub OnbbiOutlookContact_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiOutlookContact.ItemClick
			Dim objItem As Object
			Dim AddNewrec As Integer
			Dim strDesFoldername As String
			Dim headerLabel As String
			Dim iRecCount As Integer

			If m_CurrentRecordNumber Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden.")))

				Return
			End If

			Dim responsiblePersonMasterData = m_DataAccess.LoadResponsiblePersonMasterData(m_CustomerNumber, m_CurrentRecordNumber)
			If responsiblePersonMasterData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))

				Return
			End If

			Try

				Dim ol As Object = CreateObject("Outlook.Application")
				Dim olns As Object = ol.GetNamespace("MAPI")
				Dim objFolder As Object = SetContactFolder(ol, olns, "Sputnik Kunden")

				If objFolder Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Der Outlook-Kontakt Ordner konnte nicht ermittelt werden."))

					Return
				End If

				strDesFoldername = objFolder.Name

				headerLabel = String.Format("{0} {1}", responsiblePersonMasterData.Salutation, responsiblePersonMasterData.ResponsiblePersonFullname)

				' Existiert ein Dopplikat?
				For Each objItem In objFolder.Items
					If Val(UCase$(objItem.CustomerID)) = m_CurrentRecordNumber AndAlso UCase$(objItem.ReferredBy) = UCase$("KDZHD") Then

						If AddNewrec > 0 Then Exit For
						m_UtilityUI.ShowOKDialog(String.Format(m_Translate.GetSafeTranslationValue("Möglicherweise ist der Datensatz bereits vorhanden.{0}{1}: {2}"),
																									 vbNewLine, m_CurrentRecordNumber, headerLabel), m_Translate.GetSafeTranslationValue("Dopplikat"))

						Return
					End If
				Next objItem

				Dim objNewContact As Object = objFolder.Items.Add
				With objNewContact
					'.Gender = responsiblePersonMasterData.Salutation
					.LastName = responsiblePersonMasterData.Lastname
					.FirstName = responsiblePersonMasterData.Firstname
					.FullName = headerLabel
					.FileAs = String.Format("{0} ({1})", responsiblePersonMasterData.Company1, responsiblePersonMasterData.ResponsiblePersonFullname)

					.CompanyName = responsiblePersonMasterData.Company1
					.Department = responsiblePersonMasterData.Department
					.businessAddressPostOfficeBox = responsiblePersonMasterData.PostOfficeBox
					.businessAddressCity = responsiblePersonMasterData.Location
					.businessAddressStreet = responsiblePersonMasterData.Street
					.businessAddressPostalCode = responsiblePersonMasterData.Postcode
					.businessAddressCountry = responsiblePersonMasterData.CountryCode

					If responsiblePersonMasterData.Birthdate.HasValue Then .Birthday = responsiblePersonMasterData.Birthdate

					.MobileTelephoneNumber = responsiblePersonMasterData.MobilePhone
					.BusinessTelephoneNumber = responsiblePersonMasterData.Telephone
					.BusinessFaxNumber = responsiblePersonMasterData.Telefax
					.eMail1Address = responsiblePersonMasterData.Email

					.JobTitle = responsiblePersonMasterData.Position
					.Hobby = responsiblePersonMasterData.Interests

					.Categories = "Sputnik Enterprise"
					.CustomerID = m_CurrentRecordNumber
					.ReferredBy = "KDZHD"

					.Save
					'.Display(True)
					iRecCount = iRecCount + 1

				End With

				m_UtilityUI.ShowOKDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in Kontakt\{0} gespeichert."), strDesFoldername))

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich exportiert werden."), ex.ToString))

				Return
			End Try

		End Sub

		Private Function SetContactFolder(ByVal ol As Object, ByVal olns As Object, ByVal strFoldername As String) As Object
			Dim objFolder As Object = olns.GetDefaultFolder(10)

			Try
				Dim fldrs As Object = objFolder.Folders

				For Each folder In fldrs
					If folder.name = strFoldername Then
						objFolder = objFolder.Folders(strFoldername)

						Return objFolder
					End If
				Next
				objFolder.Folders.Add(strFoldername)
				objFolder = objFolder.Folders(strFoldername)

			Catch ex As Exception
				Try
					objFolder = objFolder.Folders(strFoldername)
				Catch e As Exception
					Return Nothing
				End Try


			End Try


			Return objFolder

		End Function

		Private Function GetSelectedResponsibleData() As List(Of ResponsiblePersonOverviewData)

			Dim result As New List(Of ResponsiblePersonOverviewData)

			gvResponsiblePersons.FocusedColumn = gvResponsiblePersons.VisibleColumns(1)
			gridResponsiblePersons.RefreshDataSource()
			Dim printList As BindingList(Of ResponsiblePersonOverviewData) = gridResponsiblePersons.DataSource
			If Not printList Is Nothing Then
				Dim sentList = (From r In printList Where r.IsSelected = True).ToList()

				result = New List(Of ResponsiblePersonOverviewData)

				For Each receiver In sentList
					result.Add(receiver)
				Next
			End If

			Return result

		End Function


#Region "Form handle"

		''' <summary>
		''' Handles form load event.
		''' </summary>
		Private Sub OnFrmResponsiblePerson_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
				Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
				Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
				Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTERPOSITION)
				Dim setting_form_detailsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_DETAILSPLITTERPOSITION)

				If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
				If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)
				If setting_form_mainsplitter > 0 Then Me.sccMain.SplitterPosition = Math.Max(Me.sccMain.SplitterPosition, setting_form_mainsplitter)

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

			m_SuppressUIEvents = False
		End Sub

		''' <summary>
		''' Handles the form disposed event.
		''' </summary>
		Private Sub OnFrmResponsiblePerson_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

			' Save form location, width and height in setttings
			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)

					m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTERPOSITION, Me.sccMain.SplitterPosition)

					m_SettingsManager.SaveSettings()
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Handles form closing event.
		''' </summary>
		Private Sub OnFrmResponsiblePerson_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
			' Cleanup child panels.
			For Each tabControl In m_ListOfTabControls
				tabControl.CleanUp()
			Next
		End Sub

#End Region


#End Region


#Region "Helpers"

		Public Class ResponsiblePersonOverviewData
			Public Property ID As Integer
			Public Property CustomerNumber As Integer
			Public Property RecordNumber As Integer
			Public Property Name As String
			Public Property Telephone As String
			Public Property CreatedOn As DateTime?
			Public Property CreatedFrom As String
			Public Property ChangedOn As DateTime?
			Public Property ChangedFrom As String
			Public Property IsSelected As Boolean

			Public Property ZState1 As String
			Public Property ZState2 As String

			Public ReadOnly Property IsZHDActiv As Boolean?
				Get
					Dim isZActiv As Boolean = True
					Dim state1 As String = If(String.IsNullOrWhiteSpace(ZState1), String.Empty, ZState1.ToLower)
					Dim state2 As String = If(String.IsNullOrWhiteSpace(ZState2), String.Empty, ZState2.ToLower)

					isZActiv = Not (state1.Contains("inaktiv") OrElse state1.Contains("mehr aktiv") OrElse state2.Contains("inaktiv") OrElse state2.Contains("mehr aktiv"))
					Return isZActiv
				End Get
			End Property

		End Class


#End Region

	End Class

End Namespace
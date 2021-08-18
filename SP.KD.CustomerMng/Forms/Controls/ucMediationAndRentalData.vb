Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Columns
Imports SP.Infrastructure.ucListSelectPopup
Imports SP.DatabaseAccess.Common.DataObjects

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.ComponentModel

Namespace UI

	''' <summary>
	''' Mediation and rental data (Vermittlung und Verleih)
	''' </summary>
	Public Class ucMediationAndRentalData

#Region "Private Consts"

		Private Const POPUP_DEFAULT_WIDTH As Integer = 300
		Private Const POPUP_DEFAULT_HEIGHT As Integer = 280

#End Region

#Region "Private Fields"

		''' <summary>
		''' The popup control container for GAV jobs.
		''' </summary>
		Private m_GAVPopupContainer As DevExpress.XtraBars.PopupControlContainer

		''' <summary>
		''' List of GAV group data.
		''' </summary>
		Private m_ListOfGAVGroupData As New List(Of GAVGroupData)

		''' <summary>
		''' Keyword popup data.
		''' </summary>
		Private m_KeywordPopupData As IEnumerable(Of KeywordData)

		''' <summary>
		''' Keyword popup column definitions.
		''' </summary>
		Private m_KeywordsPopupColumns As New List(Of PopupColumDefintion)

		''' <summary>
		''' GAV popup column definitions.
		''' </summary>
		Private m_GAVPopupColumns As New List(Of PopupColumDefintion)

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			lueEmploymentType.Properties.ShowHeader = False
			lueEmploymentType.Properties.ShowFooter = False
			lueEmploymentType.Properties.DropDownRows = 10

			AddHandler lueEmploymentType.ButtonClick, AddressOf OnDropDown_ButtonClick

			' Register popup row click handlers
			AddHandler ucKeywordsPopup.RowClicked, AddressOf OnPopupKeywords_RowClick
			AddHandler ucGAVPopup.RowClicked, AddressOf OnPopupGAV_RowClick

			' Register size changed handlers
			AddHandler ucKeywordsPopup.PopupSizeChanged, AddressOf OnPopupkeywords_SizeChanged
			AddHandler ucGAVPopup.PopupSizeChanged, AddressOf OnPopupGAV_SizeChanged

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)

			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			' Column defintions for popups
			m_KeywordsPopupColumns.Add(New PopupColumDefintion With {.Name = "Description", .Translation = "Name", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})
			m_GAVPopupColumns.Add(New PopupColumDefintion With {.Name = "Description", .Translation = "Name", .AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains})

		End Sub

		''' <summary>
		''' Activates the control.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function Activate(ByVal customerNumber As Integer?) As Boolean

			Dim success As Boolean = True

			If (Not IsIntialControlDataLoaded) Then
				success = success AndAlso LoadDropDownData()
				IsIntialControlDataLoaded = True
			End If

			HidePopups()

			If (customerNumber.HasValue) Then
				If (Not IsCustomerDataLoaded) Then
					success = success AndAlso LoadCustomerData(customerNumber)
				ElseIf Not customerNumber = m_CustomerNumber Then
					success = success AndAlso LoadCustomerData(customerNumber)
				End If
			Else
				Reset()
			End If

			Return success
		End Function

		''' <summary>
		''' Deactivates the control.
		''' </summary>
		Public Overrides Sub Deactivate()
			HidePopups()
		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_CustomerNumber = Nothing

			lstProfessions.DataSource = Nothing
			lstSector.DataSource = Nothing
			lstEmploymentType.DataSource = Nothing
			lstKeywords.DataSource = Nothing
			lstGAV.DataSource = Nothing

			ResetEmploymentTypeDropDown()
			HidePopups()

			btnAddGAV.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 220, m_InitializationData.MDData.MDNr)

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean
			' Do nothing
			Return True
		End Function

		''' <summary>
		''' Merges the custmer master data.
		''' </summary>
		''' <param name="customerMasterData">The customer master data object where the data gets filled into.</param>
		Public Overrides Sub MergeCustomerMasterData(ByVal customerMasterData As CustomerMasterData)
			' Do nothing
		End Sub

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()
			HidePopups()
		End Sub

#End Region


#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.grpberufe.Text = m_Translate.GetSafeTranslationValue(Me.grpberufe.Text)

			Me.grpbranchen.Text = m_Translate.GetSafeTranslationValue(Me.grpbranchen.Text)

			Me.grpgav.Text = m_Translate.GetSafeTranslationValue(Me.grpgav.Text)
			Me.grpanstellungsart.Text = m_Translate.GetSafeTranslationValue(Me.grpanstellungsart.Text)
			Me.grpstichwort.Text = m_Translate.GetSafeTranslationValue(Me.grpstichwort.Text)

		End Sub



		''' <summary>
		''' Loads the data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerData(ByVal customerNumber As Integer) As Boolean

			Dim success = True

			success = success AndAlso LoadCustomerMasterData(customerNumber)
			success = success AndAlso LoadAssignedProfessionDataOfCustomer(customerNumber)
			success = success AndAlso LoadAssignedSectorDataOfCustomer(customerNumber)
			success = success AndAlso LoadAssignedEmploymentTypeDataOfCustomer(customerNumber)
			success = success AndAlso LoadAssignedKeywordDataOfCustomer(customerNumber)
			success = success AndAlso LoadAssignedGAVGroupDataOfCustomer(customerNumber)

			m_CustomerNumber = IIf(success, customerNumber, Nothing)

			Return success

		End Function

		''' <summary>
		''' Loads the dropw down data.
		''' </summary>
		'''<returns>Boolean flag indicting success.</returns>
		Private Function LoadDropDownData() As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadEmploymentTypeDropDownData()

			Return success
		End Function

		''' <summary>
		''' Loads the employment type drop down data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmploymentTypeDropDownData() As Boolean
			Dim employmentTypeData = m_DataAccess.LoadEmploymentTypeData()

			If (employmentTypeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Anstellungsarten konnten nicht geladen werden.")
			End If

			lueEmploymentType.Properties.DataSource = employmentTypeData
			lueEmploymentType.Properties.ForceInitialize()

			Return Not employmentTypeData Is Nothing
		End Function

		''' <summary>
		''' Resets the country drop down.
		''' </summary>
		Private Sub ResetEmploymentTypeDropDown()

			lueEmploymentType.Properties.DisplayMember = "Description"
			lueEmploymentType.Properties.ValueMember = "ID"

			Dim columns = lueEmploymentType.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Description", 0))

			lueEmploymentType.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueEmploymentType.Properties.SearchMode = SearchMode.AutoComplete
			lueEmploymentType.Properties.AutoSearchColumnIndex = 0

			lueEmploymentType.Properties.NullText = String.Empty
			lueEmploymentType.EditValue = Nothing

		End Sub

		''' <summary>
		''' Loads customer master data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerMasterData(ByVal customerNumber As Integer) As Boolean

			Dim customerMasterData = m_DataAccess.LoadCustomerMasterData(customerNumber, m_ClsProgSetting.GetUSFiliale)

			If (customerMasterData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Stammdaten konnten nicht geladen werden.")
				Return False
			End If

			' No master data at the moment

			Return True

		End Function

		''' <summary>
		''' Loads assigned customer profession data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedProfessionDataOfCustomer(ByVal customerNumber As Integer) As Boolean

			Dim professionData = m_DataAccess.LoadAssignedProfessionDataOfCustomer(customerNumber)

			If (professionData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Zugeordnete Berufe konnten nicht geladen werden.")
				Return False
			End If

			lstProfessions.DisplayMember = "Description"
			lstProfessions.ValueMember = "ID"
			lstProfessions.DataSource = professionData

			Return True

		End Function

		''' <summary>
		''' Loads assigned the customer sector data (Branche).
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedSectorDataOfCustomer(ByVal customerNumber As Integer) As Boolean

			Dim sectorData = m_DataAccess.LoadAssignedSectorDataOfCustomer(customerNumber)

			If (sectorData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Zugeordnete Branchen konnten nicht geladen werden.")
				Return False
			End If

			lstSector.DisplayMember = "Description"
			lstSector.ValueMember = "ID"
			lstSector.DataSource = sectorData

			Return True

		End Function

		''' <summary>
		''' Loads assigned customer employment type data (Anstellungsart).
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedEmploymentTypeDataOfCustomer(ByVal customerNumber As Integer) As Boolean

			Dim employmentTypeData = m_DataAccess.LoadAssignedEmploymentTypeDataOfCustomer(customerNumber)

			If (employmentTypeData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Zugeordnete Anstellungsarten konnten nicht geladen werden.")
				Return False
			End If

			lstEmploymentType.DisplayMember = "Description"
			lstEmploymentType.ValueMember = "ID"
			lstEmploymentType.DataSource = employmentTypeData

			Return True

		End Function

		''' <summary>
		''' Loads assigned keyword data data (Stichworte).
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedKeywordDataOfCustomer(ByVal customerNumber As Integer) As Boolean

			Dim keywordData = m_DataAccess.LoadAssignedKeywordDataOfCustomer(customerNumber)

			If (keywordData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Zugeordnete Stichworte konnten nicht geladen werden.")
				Return False
			End If

			lstKeywords.DisplayMember = "Description"
			lstKeywords.ValueMember = "ID"
			lstKeywords.DataSource = keywordData

			Return True

		End Function

		''' <summary>
		''' Loads assigned GAV group data data (GAV Gruppe).
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadAssignedGAVGroupDataOfCustomer(ByVal customerNumber As Integer) As Boolean

			Dim gavGroupData = m_DataAccess.LoadAssignedGAVGroupDataOfCustomer(customerNumber)

			If (gavGroupData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog("Zugeordnete GAV Gruppen konnten nicht geladen werden.")
				Return False
			End If

			lstGAV.DisplayMember = "Description"
			lstGAV.ValueMember = "ID"
			lstGAV.DataSource = gavGroupData

			Return True

		End Function

		''' <summary>
		''' Handles click on add profession button.
		''' </summary>
		Private Sub OnBtnAddProfession_Click(sender As System.Object, e As System.EventArgs) Handles btnAddJobs.Click

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			HidePopups()

			Dim success = True

			' Show profession selection dialog.
			Dim obj As New SPQualicationUtility.frmQualification(m_InitializationData)
			obj.SelectMultirecords = True

			success = success AndAlso obj.LoadQualificationData("M")
			If Not success Then Return

			obj.ShowDialog()
			Dim selectedProfessionsString = obj.GetSelectedData
			If String.IsNullOrWhiteSpace(selectedProfessionsString) Then Return

			If Not selectedProfessionsString Is Nothing AndAlso selectedProfessionsString.Contains("#") Then

				Dim separatorChars() As Char = {"|", "#"}

				' Tokenize the result string.
				' Result string has the following format <ProfessionCode>#<ProfessionDescription>
				Dim tokens As String() = selectedProfessionsString.Split(separatorChars)

				' It must be an even number of tokens -> otherwhise something is wrong
				If tokens.Count Mod 2 = 0 Then

					' Load assinged professions.
					Dim assignedProfessionData = m_DataAccess.LoadAssignedProfessionDataOfCustomer(m_CustomerNumber)

					If (assignedProfessionData Is Nothing) Then
						success = False
					Else
						For i = 0 To tokens.Count() - 1 Step 2

							Dim professionCodeStr As String = tokens(i)
							Dim professionDescription As String = tokens(i + 1)
							Dim professionCodeInt As Integer = 0

							If Not Integer.TryParse(professionCodeStr, professionCodeInt) Then
								' Parsing of profession code was not possible.
								success = False
							ElseIf Not assignedProfessionData.Any(Function(data) Not data.ProfessionCodeInteger Is Nothing AndAlso data.ProfessionCodeInteger = professionCodeInt) Then

								' Add to database
								Dim professionToInsert = New CustomerAssignedProfessionData With {.CustomerNumber = m_CustomerNumber, .ProfessionCodeString = Nothing, .Description = professionDescription, .ProfessionCodeInteger = professionCodeInt}
								success = success AndAlso m_DataAccess.AddCustomerProfessionAssignment(professionToInsert)

							Else
								' Already assinged -> do nothing
							End If

						Next
					End If
				Else
					' Even number of tokens is invalid.
					success = False
				End If

			End If

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog("Ein oder mehrere Berufszuordnungen konnten nicht hinzugefügt werden.")
			End If

			LoadAssignedProfessionDataOfCustomer(m_CustomerNumber)

		End Sub

		''' <summary>
		''' Handles change of available employment type.
		''' </summary>
		Private Sub OnLueEmploymentType_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueEmploymentType.EditValueChanged

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			Dim success = True

			Dim selectedEmploymentTypeData As EmploymentTypeData = TryCast(lueEmploymentType.GetSelectedDataRow(), EmploymentTypeData)

			If (Not selectedEmploymentTypeData Is Nothing) Then

				' Load the already assigned employment types.
				Dim customerAssignedEmploymentTypes = m_DataAccess.LoadAssignedEmploymentTypeDataOfCustomer(m_CustomerNumber)

				' Check if the employment type is already assigned.
				If (customerAssignedEmploymentTypes Is Nothing) Then
					' Data could not be loaded
					success = False
				ElseIf Not customerAssignedEmploymentTypes.Any(Function(data) data.Description.ToLower().Trim() = selectedEmploymentTypeData.Description.ToLower().Trim()) Then

					' Add to database
					Dim employmentTypeAssignementToInsert = New CustomerAssignedEmploymentTypeData With {.CustomerNumber = m_CustomerNumber, .Description = selectedEmploymentTypeData.Description}

					success = m_DataAccess.AddCustomerEmploymentTypeAssignment(employmentTypeAssignementToInsert)
				Else
					' Already assinged -> do nothing
				End If

			Else
				success = False
			End If

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog("Anstellungsart konnte nicht hinzugefügt werden.")
			End If

			' Reload assigned employment types.
			LoadAssignedEmploymentTypeDataOfCustomer(m_CustomerNumber)

		End Sub

		''' <summary>
		''' Handles click on add sector button.
		''' </summary>
		Private Sub OnBtnAddSector_Click(sender As System.Object, e As System.EventArgs) Handles btnAddSector.Click

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			HidePopups()

			Dim success = True

			Dim separatorChars() As Char = {"|", "#"}

			' Show sector selection dialog.
			' Result string has the following format <SectorCode>#<SectorDescription>
			Dim obj As New SPQualicationUtility.frmBranches(m_InitializationData)
			obj.SelectMultirecords = True

			success = success AndAlso obj.LoadBranchesData()
			If Not success Then Return

			obj.ShowDialog()
			Dim selectedSectorsString = obj.GetSelectedData
			If String.IsNullOrWhiteSpace(selectedSectorsString) Then Return

			If Not selectedSectorsString Is Nothing AndAlso selectedSectorsString.Contains("#") Then

				Dim tokens As String() = selectedSectorsString.Split(separatorChars)

				' It must be an even number of tokens -> otherwhise something is wrong
				If tokens.Count Mod 2 = 0 Then

					' Load assinged sector
					Dim assignedSectorData = m_DataAccess.LoadAssignedSectorDataOfCustomer(m_CustomerNumber)

					If (assignedSectorData Is Nothing) Then
						success = False
					Else
						For i = 0 To tokens.Count() - 1 Step 2

							Dim sectorStr As String = tokens(i)
							Dim sectorDescription As String = tokens(i + 1)
							Dim sectorCodeInt As Integer = 0

							If Not Integer.TryParse(sectorStr, sectorCodeInt) Then
								' Parsing of profession code was not possible.
								success = False
							ElseIf Not assignedSectorData.Any(Function(data) Not data.SectorCode Is Nothing AndAlso data.SectorCode = sectorCodeInt) Then

								' Add to database
								Dim sectorToInsert = New CustomerAssignedSectorData With {.CustomerNumber = m_CustomerNumber, .Description = sectorDescription, .SectorCode = sectorCodeInt}
								success = success AndAlso m_DataAccess.AddCustomerSectorAssignment(sectorToInsert)
							Else
								' Already assinged -> do nothing
							End If

						Next
					End If
				Else
					' Even number of tokens is invalid.
					success = False
				End If

			End If

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog("Ein oder mehrere Branchenzuordnungen konnten nicht hinzugefügt werden.")
			End If

			LoadAssignedSectorDataOfCustomer(m_CustomerNumber)

		End Sub

		''' <summary>
		''' Handles click on add GAV group button.
		''' </summary>
		Private Sub OnBtnAddGAV_Click(sender As System.Object, e As System.EventArgs) Handles btnAddGAV.Click

			Dim cursorPos = Cursor.Position

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			HidePopups()
			Dim position = Cursor.Position

			' Parse the GAV jobs list.
			Dim canton = RetrieveCustomerCanton()
			LoadGAVGroups(canton)

			Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_GAV_SIZE)

			ucGAVPopup.InitPopup(m_ListOfGAVGroupData, m_GAVPopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
			ucGAVPopup.ShowPopup(position, popupSize)

		End Sub

		''' <summary>
		''' Parses GAV jobs.
		''' </summary>
		Private Sub LoadGAVGroups(ByVal canton As String)

			'Dim gav As New SPGAV.ClsMain_Net()
			'Dim gavJobsString = gav.ListPVLBerufe(m_CustomerNumber, canton)

			m_ListOfGAVGroupData.Clear()
			Dim customerData = m_DataAccess.LoadCustomerMasterData(m_CustomerNumber, m_ClsProgSetting.GetUSFiliale)
			If customerData Is Nothing Then Return
			Dim gavJobsString = New BindingList(Of SPGAV.PVLGAV.PVLGAVViewData)
			If canton = "FL" Then
				Dim gav As New SPGAV.PVLGAV.PVLGAVUtility(m_InitializationData)
				gavJobsString = gav.LoadFLPVLMetaData()

				For Each itm In gavJobsString
					Dim gavGroupData As New GAVGroupData With {.GAVNumber = itm.gav_number, .Description = itm.name_de, .BoolValue = True, .Canton = canton}
					m_ListOfGAVGroupData.Add(gavGroupData)
				Next

			Else
				Dim pvlData As New SPGAV.UI.frmTempDataPVL(m_InitializationData)
				Dim pvlContractData = pvlData.LoadPVLContractData

				For Each itm In pvlContractData
					Dim gavGroupData As New GAVGroupData With {.GAVNumber = itm.ContractNumber, .Description = itm.ContractName}
					m_ListOfGAVGroupData.Add(gavGroupData)
				Next

				'gavJobsString = gav.LoadPVLAssignedCantonMetaData(canton, customerData.Postcode, customerData.Language)

			End If
			Return


			'For Each itm In gavJobsString
			'	Dim gavNumber As Integer = itm.gav_number
			'	Dim gavDescription As String = itm.name_de
			'	Dim boolValue As Boolean = True

			'	If gavNumber > 0 AndAlso boolValue Then
			'		Dim gavGroupData As New GAVGroupData With {.GAVNumber = gavNumber, .Description = gavDescription, .BoolValue = boolValue, .Canton = canton}
			'		m_ListOfGAVGroupData.Add(gavGroupData)

			'	ElseIf canton = "FL" Then
			'		Dim gavGroupData As New GAVGroupData With {.GAVNumber = gavNumber, .Description = gavDescription, .BoolValue = boolValue, .Canton = canton}
			'		m_ListOfGAVGroupData.Add(gavGroupData)

			'	Else
			'		' GAV data ist not parseable.
			'		m_Logger.LogWarning(String.Format("{0} >>> {1}: {2} ist ungültig.", gavNumber, gavDescription, boolValue))
			'	End If

			'Next


		End Sub

		''' <summary>
		''' Handles click on add keyword button.
		''' </summary>
		Private Sub OnBtnAddKeywords_Click(sender As System.Object, e As System.EventArgs) Handles btnAddKeywords.Click

			Dim cursorPos = Cursor.Position

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			HidePopups()

			Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_KEYWORDS_SIZE)
			Dim position = Cursor.Position

			If (m_KeywordPopupData Is Nothing) Then
				m_KeywordPopupData = m_DataAccess.LoadKeywordData()
			End If

			ucKeywordsPopup.InitPopup(m_KeywordPopupData, m_KeywordsPopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
			ucKeywordsPopup.ShowPopup(position, popupSize)

		End Sub

		''' <summary>
		''' Handles click on a row on one of the popups.
		''' </summary>
		Private Sub OnPopupKeywords_RowClick(ByVal sender As Object, ByVal clickedObject As Object)
			Dim success = AddCustomerKeywordAssigment(clickedObject)

			HidePopups()

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog("Die Stichwortzuordnung konnte nicht gespeichert werden.")
			End If

		End Sub


		''' <summary>
		''' Handles popup gav row click.
		''' </summary>
		Private Sub OnPopupGAV_RowClick(ByVal sender As Object, ByVal clickedObject As Object)

			Dim gavGroupData As GAVGroupData = CType(clickedObject, GAVGroupData)

			Dim success = AddCustomerGAVGroupAssignment(gavGroupData)

			If (Not success) Then
				m_UtilityUI.ShowErrorDialog("Die GAV-Zuordnung konnte nicht gespeichert werden. Möglicherweise erkennt das System Ihre Auswahl als Duplikat.")
			End If

			' Ask if the data should also be added to sector (Branche) data.
			If success AndAlso
					m_UtilityUI.ShowYesNoDialog("Möchten Sie den Eintrag zusätzlich zu Branche / Gewerbe hinzufügen?") = True Then

				Dim assignedSectorData = m_DataAccess.LoadAssignedSectorDataOfCustomer(m_CustomerNumber)

				' Make sure the sector is not already assigned. Here we use the description text, because we do not have a sector code available.
				If Not assignedSectorData.Any(Function(data) Not data.Description Is Nothing AndAlso data.Description.ToLower().Trim() = gavGroupData.Description.ToLower().Trim()) Then

					' Add to database (In this case the SecorCode is written with  0).
					Dim sectorToInsert = New CustomerAssignedSectorData With {.CustomerNumber = m_CustomerNumber, .Description = gavGroupData.Description, .SectorCode = 0}
					success = m_DataAccess.AddCustomerSectorAssignment(sectorToInsert)

					If (Not success) Then
						m_UtilityUI.ShowErrorDialog("Die Eintrag konnte nicht zu Branche / Gewerbe hinzugefügt werden.")
					Else
						LoadAssignedSectorDataOfCustomer(m_CustomerNumber)
					End If
				End If

			End If

			HidePopups()

		End Sub

		''' <summary>
		''' Handles poupup keywords size change.
		''' </summary>
		Private Sub OnPopupkeywords_SizeChanged(ByVal sender As Object, ByVal newWidth As Integer, ByVal newHeight As Integer)
			Try
				Dim setting As String = String.Format("{0};{1}", newWidth, newHeight)
				m_SettingsManager.WriteString(Settings.SettingKeys.SETTING_POPUP_KEYWORDS_SIZE, setting)
				m_SettingsManager.SaveSettings()
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub

		''' <summary>
		''' Handles poupup gav size change.
		''' </summary>
		Private Sub OnPopupGAV_SizeChanged(ByVal sender As Object, ByVal newWidth As Integer, ByVal newHeight As Integer)
			Try
				Dim setting As String = String.Format("{0};{1}", newWidth, newHeight)
				m_SettingsManager.WriteString(Settings.SettingKeys.SETTING_POPUP_GAV_SIZE, setting)
				m_SettingsManager.SaveSettings()
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

		End Sub

		''' <summary>
		''' Handles keydown event on customer professions list.
		''' </summary>
		Private Sub OnLstProfessions_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstProfessions.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 226, m_InitializationData.MDData.MDNr) Then Exit Sub

				Dim selectedProfessionData As CustomerAssignedProfessionData = TryCast(lstProfessions.SelectedItem, CustomerAssignedProfessionData)

				If (Not selectedProfessionData Is Nothing) Then

					If Not m_DataAccess.DeleteCustomerProfessionDataAssignment(selectedProfessionData.ID) Then
						m_UtilityUI.ShowErrorDialog("Berufszuordnung konnte nicht gelöscht werden.")
					End If

					LoadAssignedProfessionDataOfCustomer(m_CustomerNumber)

				End If

			End If
		End Sub

		''' <summary>
		''' Handles keydown event on customer employment type list.
		''' </summary>
		Private Sub OnLstEmploymentType_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstEmploymentType.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then

				Dim employmentTypeData As CustomerAssignedEmploymentTypeData = TryCast(lstEmploymentType.SelectedItem, CustomerAssignedEmploymentTypeData)

				If (Not employmentTypeData Is Nothing) Then

					If Not m_DataAccess.DeleteCustomerEmploymentTypeDataAssignment(employmentTypeData.ID) Then
						m_UtilityUI.ShowErrorDialog("Anstellungsart konnte nicht gelöscht werden.")
					End If

					LoadAssignedEmploymentTypeDataOfCustomer(m_CustomerNumber)

				End If

			End If

		End Sub

		''' <summary>
		''' Handles keydown event on customer sector list.
		''' </summary>
		Private Sub OnLstSector_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstSector.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 227, m_InitializationData.MDData.MDNr) Then Exit Sub

				Dim selectedSectorData As CustomerAssignedSectorData = TryCast(lstSector.SelectedItem, CustomerAssignedSectorData)

				If (Not selectedSectorData Is Nothing) Then

					If Not m_DataAccess.DeleteCustomerSectorDataAssignment(selectedSectorData.ID) Then
						m_UtilityUI.ShowErrorDialog("Branchenzuordnung konnte nicht gelöscht werden.")
					End If

					LoadAssignedSectorDataOfCustomer(m_CustomerNumber)

				End If

			End If
		End Sub

		''' <summary>
		''' Handles keydown event on customer GAV group list.
		''' </summary>
		Private Sub OnLstGAV_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstGAV.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 221, m_InitializationData.MDData.MDNr) Then Exit Sub

				Dim selectedGAVGroupdData As CustomerAssignedGAVGroupData = TryCast(lstGAV.SelectedItem, CustomerAssignedGAVGroupData)

				If (Not selectedGAVGroupdData Is Nothing) Then

					If Not m_DataAccess.DeleteCustomerGAVGroupDataAssignment(selectedGAVGroupdData.ID) Then
						m_UtilityUI.ShowErrorDialog("GAV Gruppe konnte nicht gelöscht werden.")
					End If

					LoadAssignedGAVGroupDataOfCustomer(m_CustomerNumber)

				End If

			End If
		End Sub

		''' <summary>
		''' Handles keydown event on customer keywords list.
		''' </summary>
		Private Sub OnLstKeywords_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstKeywords.KeyDown

			If (Not IsCustomerDataLoaded) Then
				Return
			End If

			If (e.KeyCode = Keys.Delete) Then
				If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 228, m_InitializationData.MDData.MDNr) Then Exit Sub

				Dim selectedKeywordData As CustomerAssignedKeywordData = TryCast(lstKeywords.SelectedItem, CustomerAssignedKeywordData)

				If (Not selectedKeywordData Is Nothing) Then

					If Not m_DataAccess.DeleteCustomerKeywordDataAssignment(selectedKeywordData.ID) Then
						m_UtilityUI.ShowErrorDialog("Stichwort konnte nicht gelöscht werden.")
					End If

					LoadAssignedKeywordDataOfCustomer(m_CustomerNumber)

				End If

			End If
		End Sub

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_DELETE_BUTTON Then

				If TypeOf sender Is LookUpEdit Then
					Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
					lookupEdit.EditValue = Nothing
				ElseIf TypeOf sender Is ComboBoxEdit Then
					Dim comboboxEdit As ComboBoxEdit = CType(sender, ComboBoxEdit)
					comboboxEdit.EditValue = Nothing
				End If
			End If
		End Sub

		''' <summary>
		''' Adds a keyword assignment.
		''' </summary>
		''' <param name="keywordData">The keyword data object.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function AddCustomerKeywordAssigment(ByVal keywordData As KeywordData) As Boolean

			Dim succcess = True

			' Load assigned keyword data.
			Dim assignedKeywordData = m_DataAccess.LoadAssignedKeywordDataOfCustomer(m_CustomerNumber)

			If Not assignedKeywordData Is Nothing Then

				' Check if the new keyword is not already assigned
				If Not assignedKeywordData.Any(Function(data) data.Description.ToLower().Trim() = keywordData.Description.ToLower().Trim()) Then

					' Add to database.
					Dim keywordDataToAssign = New CustomerAssignedKeywordData With {.CustomerNumber = m_CustomerNumber, .Description = keywordData.Description}
					succcess = m_DataAccess.AddCustomerKeywordAssignment(keywordDataToAssign)
				End If
			Else
				succcess = False
			End If

			LoadAssignedKeywordDataOfCustomer(m_CustomerNumber)

			Return succcess

		End Function

		''' <summary>
		''' Adds a GAV group assignment.
		''' </summary>
		''' <param name="gavGroupData">The GAV data object.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function AddCustomerGAVGroupAssignment(ByVal gavGroupData As GAVGroupData) As Boolean

			Dim succcess = True

			' Load assigned GAV data.
			Dim assignedGAVData = m_DataAccess.LoadAssignedGAVGroupDataOfCustomer(m_CustomerNumber)

			If Not assignedGAVData Is Nothing Then

				' Check if the new GAV group is not already assigned
				If Not assignedGAVData.Any(Function(data) data.GAVNUmber = gavGroupData.GAVNumber) Then

					' Add to database.
					Dim gavData As New CustomerAssignedGAVGroupData With {.CustomerNumber = m_CustomerNumber, .Description = gavGroupData.Description, .GAVNUmber = gavGroupData.GAVNumber, .Canton = gavGroupData.Canton}
					succcess = m_DataAccess.AddCustomerGAVGroupAssignment(gavData)
				Else
					succcess = False
				End If
			Else
				succcess = False
			End If

			LoadAssignedGAVGroupDataOfCustomer(m_CustomerNumber)

			Return succcess

		End Function

		''' <summary>
		''' Retrievs the canton value from the customer.
		''' </summary>
		Private Function RetrieveCustomerCanton()
			Dim canton As String = String.Empty
			Dim frmCustomers = CType(ParentForm, frmCustomers)

			' Test if curently a postcode is selected in the UI.
			Dim selectedPostCode = frmCustomers.GetUISelectedPostCodeOfCustomer(m_CustomerNumber)

			If (selectedPostCode Is Nothing) Then

				' No postcode is seleded, maybe the data has not been loaded or its a new customer -> load customer data from database.
				Dim customerData = m_DataAccess.LoadCustomerMasterData(m_CustomerNumber, m_ClsProgSetting.GetUSFiliale)

				If Not (customerData Is Nothing AndAlso customerData.CountryCode.ToUpper().Trim = "CH") Then
					Dim cantonFromDB = m_CommonDatabaseAccess.LoadCantonByPostCode(customerData.Postcode)

					If Not String.IsNullOrEmpty(cantonFromDB) Then
						canton = cantonFromDB
					End If
				End If

			Else
				canton = CType(selectedPostCode, PostCodeData).Canton
			End If

			If String.IsNullOrEmpty(canton) Then

				canton = m_InitializationData.MDData.MDCanton

			End If


			Return canton
		End Function

		''' <summary>
		''' Hides the popups.
		''' </summary>
		Private Sub HidePopups()

			ucKeywordsPopup.HidePopup()
			ucGAVPopup.HidePopup()
		End Sub

		''' <summary>
		''' Reads a popup size setting.
		''' </summary>
		''' <param name="settingKey">The settings key.</param>
		''' <returns>The size setting.</returns>
		Private Function ReadPopupSizeSetting(ByRef settingKey As String) As Size

			' Load width/height setting
			Dim popupSizeSetting As String = String.Empty
			Dim popupSize As Size
			popupSize.Width = POPUP_DEFAULT_WIDTH
			popupSize.Height = POPUP_DEFAULT_HEIGHT

			Try
				popupSizeSetting = m_SettingsManager.ReadString(settingKey)

				If Not String.IsNullOrEmpty(popupSizeSetting) Then
					Dim arrSize As String() = popupSizeSetting.Split(CChar(";"))
					popupSize.Width = arrSize(0)
					popupSize.Height = arrSize(1)
				End If
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
			End Try

			Return popupSize
		End Function

#End Region

	End Class

End Namespace

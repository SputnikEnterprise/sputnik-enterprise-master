Imports System.ComponentModel
Imports DevExpress.XtraEditors.Repository
Imports System.IO
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports SP.KD.KontaktMng

Namespace UI

  ''' <summary>
  ''' Contact data.
  ''' </summary>
  Public Class ucContactData

#Region "Private Fields"

    Private m_CheckEditImportant As RepositoryItemCheckEdit
    Private m_CheckEditCompleted As RepositoryItemCheckEdit
    Private m_IsFilterActivated As Boolean = False

    ''' <summary>
    ''' Contact detail form.
    ''' </summary>
    Private m_ContactDetailForm As frmContacts
    Private m_ContactFilterSettingsForDetailForm As New ContactFilterSettings

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
      gridViewContactData.OptionsView.ShowIndicator = False

      ' Important symbol.
      m_CheckEditImportant = CType(gridContactData.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
      m_CheckEditImportant.PictureChecked = My.Resources.Important
      m_CheckEditImportant.PictureUnchecked = Nothing
      m_CheckEditImportant.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

      ' Completed symbol
      m_CheckEditCompleted = CType(gridContactData.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
      m_CheckEditCompleted.PictureChecked = My.Resources.Completed
      m_CheckEditCompleted.PictureUnchecked = Nothing
      m_CheckEditCompleted.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

      AddHandler chkTelephone.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
      AddHandler chkOffered.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
      AddHandler chkMailed.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
      AddHandler chkSMS.CheckedChanged, AddressOf OnFilterCheckBox_CheckedChanged
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="recordNumber">The record number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function Activate(ByVal customerNumber As Integer, ByVal recordNumber As Integer?) As Boolean

      Dim success = True

      If (recordNumber.HasValue) Then
        If (Not IsResponsiblePersonDataLoaded) Then
          LoadFilterSettings()
          success = success AndAlso LoadYearsListData(customerNumber, recordNumber)
          success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
        ElseIf Not customerNumber = m_CustomerNumber Or
              Not m_RecordNumber = recordNumber Then
          LoadFilterSettings()
          success = success AndAlso LoadYearsListData(customerNumber, recordNumber)
          success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
        Else
          m_IsFilterActivated = True
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
      m_IsFilterActivated = False
    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_CustomerNumber = Nothing
      m_RecordNumber = Nothing

      m_IsFilterActivated = False
      gridContactData.DataSource = Nothing

      chkTelephone.Checked = False
      chkMailed.Checked = False
      chkOffered.Checked = False
      chkSMS.Checked = False

      lstYears.DataSource = Nothing

      ' Reset the grid
      gridViewContactData.Columns.Clear()

      Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
      columnDate.Caption = m_translate.GetSafeTranslationValue("Datum")
      columnDate.Name = "ContactDate"
      columnDate.FieldName = "ContactDate"
      columnDate.Visible = True
      columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
      gridViewContactData.Columns.Add(columnDate)

      Dim personSubjectColumn As New DevExpress.XtraGrid.Columns.GridColumn()
      personSubjectColumn.Caption = m_translate.GetSafeTranslationValue("Person / Betreff")
      personSubjectColumn.Name = "Person_Subject"
      personSubjectColumn.FieldName = "Person_Subject"
      personSubjectColumn.Visible = True
      gridViewContactData.Columns.Add(personSubjectColumn)

      Dim descriptionColumn As New DevExpress.XtraGrid.Columns.GridColumn()
      descriptionColumn.Caption = m_translate.GetSafeTranslationValue("Beschreibung")
      descriptionColumn.Name = "Description"
      descriptionColumn.FieldName = "Description"
      descriptionColumn.Visible = True
      descriptionColumn.Width = 200
      gridViewContactData.Columns.Add(descriptionColumn)

      Dim importantColumn As New DevExpress.XtraGrid.Columns.GridColumn()
      importantColumn.Caption = m_translate.GetSafeTranslationValue("Wichtig")
      importantColumn.Name = "Important"
      importantColumn.FieldName = "Important"
      importantColumn.Visible = True
      importantColumn.ColumnEdit = m_CheckEditImportant
      gridViewContactData.Columns.Add(importantColumn)

      Dim completedColumn As New DevExpress.XtraGrid.Columns.GridColumn()
      completedColumn.Caption = m_translate.GetSafeTranslationValue("Erledigt")
      completedColumn.Name = "Completed"
      completedColumn.FieldName = "Completed"
      completedColumn.Visible = True
      completedColumn.ColumnEdit = m_CheckEditCompleted
      gridViewContactData.Columns.Add(completedColumn)

      Dim kstColumn As New DevExpress.XtraGrid.Columns.GridColumn()
      kstColumn.Caption = m_translate.GetSafeTranslationValue("Berater")
      kstColumn.Name = "KST"
      kstColumn.FieldName = "KST"
      kstColumn.Visible = True
      gridViewContactData.Columns.Add(kstColumn)

      Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
      docType.Caption = " "
      docType.Name = "docType"
      docType.FieldName = "docType"
      docType.Visible = True
      Dim picutureEdit As New RepositoryItemPictureEdit()
      picutureEdit.NullText = " "
      docType.ColumnEdit = picutureEdit
      docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
      docType.Width = 20
      gridViewContactData.Columns.Add(docType)

    End Sub

    ''' <summary>
    ''' Validated data.
    ''' </summary>
    Public Overrides Function ValidateData() As Boolean
      ' Do nothing
      Return True
    End Function

    ''' <summary>
    ''' Merges the responsible person master data.
    ''' </summary>
    ''' <param name="responsiblePersonMasterData">The responsible person master data object where the data gets filled into.</param>
    Public Overrides Sub MergeResponsiblePersonMasterData(ByVal responsiblePersonMasterData As ResponsiblePersonMasterData, Optional forceMerge As Boolean = False)
      ' Do nothing
    End Sub

    ''' <summary>
    ''' Cleanup control.
    ''' </summary>
    Public Overrides Sub CleanUp()

      If Not m_ContactDetailForm Is Nothing AndAlso
        Not m_ContactDetailForm.IsDisposed Then

        Try
          m_ContactDetailForm.Close()
          m_ContactDetailForm.Dispose()
        Catch
          ' Do nothing
        End Try
      End If

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.Text = m_translate.GetSafeTranslationValue(Me.Text)

      Me.grpKontakt.Text = m_translate.GetSafeTranslationValue(Me.grpKontakt.Text)
      Me.grpInfo.Text = m_translate.GetSafeTranslationValue(Me.grpInfo.Text)
      Me.grpausblenden.Text = m_translate.GetSafeTranslationValue(Me.grpausblenden.Text)
      Me.grpjahre.Text = m_translate.GetSafeTranslationValue(Me.grpjahre.Text)

      Me.chkMailed.Text = m_translate.GetSafeTranslationValue(Me.chkMailed.Text)
      Me.chkOffered.Text = m_translate.GetSafeTranslationValue(Me.chkOffered.Text)
      Me.chkSMS.Text = m_translate.GetSafeTranslationValue(Me.chkSMS.Text)
      Me.chkTelephone.Text = m_translate.GetSafeTranslationValue(Me.chkTelephone.Text)

      Me.lblAnzahl.Text = m_translate.GetSafeTranslationValue(Me.lblAnzahl.Text)
      Me.lblinfoneu.Text = m_translate.GetSafeTranslationValue(Me.lblinfoneu.Text)
      Me.lblinfoalt.Text = m_translate.GetSafeTranslationValue(Me.lblinfoalt.Text)

    End Sub

    ''' <summary>
    '''  Loads responsible person data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The record number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadResponsiblePersonData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As Boolean

      Dim success = True

      m_IsFilterActivated = True

      success = success AndAlso FilterContactData(customerNumber, responsiblePersonRecordNumber)


      m_CustomerNumber = IIf(success, customerNumber, 0)
      m_RecordNumber = IIf(success, responsiblePersonRecordNumber, Nothing)

      Return success
    End Function

    ''' <summary>
    ''' Loads the years drop down data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The record number.</param>
    '''<returns>Boolean flag indicating success.</returns>
    Private Function LoadYearsListData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?) As Boolean
      Dim yearsData = m_DataAccess.LoadCustomerContactTotalDistinctYears(customerNumber, responsiblePersonRecordNumber)

      If (yearsData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Jahre konnten nicht geladen werden."))
      End If

      Dim filterActive = m_IsFilterActivated

      ' Remember checked years
      Dim selectedYears As New List(Of Integer)
      If lstYears.Items.Count > 0 Then
        For Each item As CheckedListBoxItem In lstYears.CheckedItems
          selectedYears.Add(item.Value)
        Next
      End If

      ' Load years from database.
      lstYears.Items.Clear()
      For Each yearEntry In yearsData
        lstYears.Items.Add(yearEntry)
      Next

      ' Recheck previously selected years.
      If selectedYears.Count > 0 Then
        For Each iYear In selectedYears
          Dim index = lstYears.Items.IndexOf(iYear)

          If index > -1 Then
            lstYears.Items(index).CheckState = CheckState.Checked
          End If

        Next
      Else
        ' Slect current year.
        Dim index = lstYears.Items.IndexOf(Date.Now.Date.Year)
        lstYears.Items(index).CheckState = CheckState.Checked
      End If

      m_IsFilterActivated = filterActive
     
      Return Not yearsData Is Nothing
    End Function


    ''' <summary>
    ''' Loads contact data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The record number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function FilterContactData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer)

      If Not m_IsFilterActivated Then
        Return True
      End If

      ' Convert check years to a integer array.
      Dim selectedYears = lstYears.CheckedItems
      Dim filterYears As New List(Of Integer)

      For Each yar In selectedYears
        filterYears.Add(CType(yar, CheckedListBoxItem).Value)
      Next

      ' If no years are checked enter a impossible year (-> nothing will be found).
      If (filterYears.Count = 0) Then
        filterYears.Add(-1)
      End If

      Dim yearsArray = filterYears.ToArray()
      Dim contactData = m_DataAccess.LoadCustomerContactOverviewlDataBySearchCriteria(customerNumber,
                                                                                      responsiblePersonRecordNumber,
                                                                                      Not chkTelephone.Checked,
                                                                                      Not chkOffered.Checked,
                                                                                      Not chkMailed.Checked,
                                                                                      Not chkSMS.Checked,
                                                                                      yearsArray)

      If (contactData Is Nothing) Then

        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Kontaktdaten konnten nicht geladen werden."))

        Return False
      End If

      Dim listDataSource As BindingList(Of ContactViewData) = New BindingList(Of ContactViewData)

      ' Convert the data to view data.
      For Each p In contactData

        Dim cViewData = New ContactViewData() With {
            .ID = p.ID,
            .CustomerNumber = p.CustomerNumber,
            .ResponsiblePersonRecordNumber = p.ResponsiblePersonRecordNumber,
            .ContactRecorNumber = p.RecNr,
            .ContactDate = p.ContactDate,
            .minContactDate = p.minContactDate,
            .maxContactDate = p.maxContactDate,
            .Person_Subject = p.PersonOrSubject,
            .Description = p.Description,
            .Important = p.IsImportant,
            .Completed = p.IsCompleted,
            .KST = p.Creator}

        If p.DocumentID.HasValue Then
          cViewData.PDFImage = My.Resources.pdf
          cViewData.DocumentId = p.DocumentID
        End If

        listDataSource.Add(cViewData)
      Next

      gridContactData.DataSource = listDataSource

      ' Show number of results
      lblNumberOfEntriesValue.Text = contactData.Count
      If (contactData.Count > 0) Then
        lblFirstInfoValue.Text = IIf(contactData(0).minContactDate.HasValue, contactData(0).minContactDate.Value.ToShortDateString(), "-")
        lblLastInfoValue.Text = IIf(contactData(contactData.Count - 1).maxContactDate.HasValue, contactData(contactData.Count - 1).maxContactDate.Value.ToShortDateString(), "-")
      Else
        lblFirstInfoValue.Text = "-"
        lblLastInfoValue.Text = "-"
      End If

      SaveFilterSettings()

      Return True
    End Function

    ''' <summary>
    ''' Handles change of filter checkbox.
    ''' </summary>
    Private Sub OnFilterCheckBox_CheckedChanged(sender As System.Object, e As System.EventArgs)
      If IsResponsiblePersonDataLoaded Then
        FilterContactData(m_CustomerNumber, m_RecordNumber)
      End If
    End Sub

    ''' <summary>
    ''' Handles change of year checkbox.
    ''' </summary>
    Private Sub OnLstYears_ItemCheck(sender As System.Object, e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs) Handles lstYears.ItemCheck
      If IsResponsiblePersonDataLoaded Then
        FilterContactData(m_CustomerNumber, m_RecordNumber)
      End If
    End Sub

    ''' <summary>
    ''' Loads filter settings.
    ''' </summary>
    Private Sub LoadFilterSettings()
      Try
        chkTelephone.CheckState = IIf(m_SettingsManager.ReadBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_PHONE), CheckState.Checked, CheckState.Unchecked)
        chkMailed.CheckState = IIf(m_SettingsManager.ReadBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_MAIL), CheckState.Checked, CheckState.Unchecked)
        chkOffered.CheckState = IIf(m_SettingsManager.ReadBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_OFFERED), CheckState.Checked, CheckState.Unchecked)
        chkSMS.CheckState = IIf(m_SettingsManager.ReadBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_SMS), CheckState.Checked, CheckState.Unchecked)
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

    End Sub

    ''' <summary>
    ''' Saves filter settings.
    ''' </summary>
    Private Sub SaveFilterSettings()
      Try
        m_SettingsManager.WriteBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_PHONE, chkTelephone.CheckState = CheckState.Checked)
        m_SettingsManager.WriteBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_MAIL, chkMailed.CheckState = CheckState.Checked)
        m_SettingsManager.WriteBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_OFFERED, chkOffered.CheckState = CheckState.Checked)
        m_SettingsManager.WriteBoolean(Settings.SettingKeys.SETTING_CONTACT_SEARCH_EXCLUDE_SMS, chkSMS.CheckState = CheckState.Checked)
        m_SettingsManager.SaveSettings()
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

    End Sub

    ''' <summary>
    ''' Handles unbound column data event.
    ''' </summary>
    Private Sub OnGvDocuments_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gridViewContactData.CustomUnboundColumnData

      If e.Column.Name = "docType" Then
        If (e.IsGetData()) Then
          e.Value = CType(e.Row, ContactViewData).PDFImage
        End If
      End If
    End Sub

    ''' <summary>
    ''' Handles double click on contact.
    ''' </summary>
    Private Sub OnContact_DoubleClick(sender As System.Object, e As System.EventArgs) Handles gridViewContactData.DoubleClick
      Dim selectedRows = gridViewContactData.GetSelectedRows()

      If (selectedRows.Count > 0) Then
        Dim contactInfoData = CType(gridViewContactData.GetRow(selectedRows(0)), ContactViewData)
        ShowContatDetailForm(m_CustomerNumber, m_RecordNumber, contactInfoData.ContactRecorNumber)
      End If
    End Sub

    ''' <summary>
    ''' Handles click on add contact.
    ''' </summary>
    Private Sub OnBtnAddContactInfo_Click(sender As System.Object, e As System.EventArgs) Handles btnAddContact.Click
      If (IsResponsiblePersonDataLoaded) Then
        ShowContatDetailForm(m_CustomerNumber, m_RecordNumber, Nothing)
      End If
    End Sub

    ''' <summary>
    ''' Handles keydown on documents grid.
    ''' </summary>
    Private Sub OnGridDocuments_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gridContactData.KeyDown

      If Not IsResponsiblePersonDataLoaded Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then

        Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

        If Not (grdView Is Nothing) Then

          Dim selectedRows = grdView.GetSelectedRows()

          If (selectedRows.Count > 0) Then
            Dim contactData = CType(grdView.GetRow(selectedRows(0)), ContactViewData)

            If (m_UtilityUI.ShowYesNoDialog(m_translate.GetSafeTranslationValue("Möchten Sie die ausgewählen Daten wirklich löschen?"),
                                            m_translate.GetSafeTranslationValue("Daten entgültig löschen?"))) Then
              Dim success = m_DataAccess.DeleteResponsiblePersonContactAssignment(contactData.ID)

              If Not success Then
                m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Der Kontakt konnte nicht gelöscht werden."))
              Else
                If contactData.DocumentId.HasValue Then
                  success = success AndAlso m_DataAccess.DeleteContactDocument(contactData.DocumentId)

                  If Not success Then
                    m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Das angehängte Dokument des Kontaktes konnte nicht gelöscht werden."))
                  End If
                End If
              End If

              LoadYearsListData(m_CustomerNumber, m_RecordNumber)
              FilterContactData(m_CustomerNumber, m_RecordNumber)

            End If

          End If

        End If

      End If
    End Sub

    ''' <summary>
    ''' Shows the contact detail form.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person record number.</param>
    ''' <param name="conactRecordNumber">The contact record number to select.</param>
    Private Sub ShowContatDetailForm(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer, ByVal conactRecordNumber As Integer?)

      If m_ContactDetailForm Is Nothing OrElse m_ContactDetailForm.IsDisposed Then

        If Not m_ContactDetailForm Is Nothing Then
          'First cleanup handlers of old form before new form is created.
          RemoveHandler m_ContactDetailForm.FormClosed, AddressOf OnContactFormClosed
          RemoveHandler m_ContactDetailForm.ContactDataSaved, AddressOf OnContactInfoFormDocumentDataSaved
          RemoveHandler m_ContactDetailForm.ContactDataDeleted, AddressOf OnContactInfoFormDocumentDataDeleted
        End If

        m_ContactDetailForm = New frmContacts(m_InitializationData)
        AddHandler m_ContactDetailForm.FormClosed, AddressOf OnContactFormClosed
        AddHandler m_ContactDetailForm.ContactDataSaved, AddressOf OnContactInfoFormDocumentDataSaved
        AddHandler m_ContactDetailForm.ContactDataDeleted, AddressOf OnContactInfoFormDocumentDataDeleted
      End If

      m_ContactFilterSettingsForDetailForm.ExcluePhone = chkTelephone.Checked
      m_ContactFilterSettingsForDetailForm.ExclueMail = chkMailed.Checked
      m_ContactFilterSettingsForDetailForm.ExclueOffered = chkOffered.Checked
      m_ContactFilterSettingsForDetailForm.ExcludeSMS = chkSMS.Checked
      m_ContactFilterSettingsForDetailForm.ClearYears()

      Dim selectedYears = lstYears.CheckedItems

      For Each iYear In selectedYears
        m_ContactFilterSettingsForDetailForm.AddYear(CType(iYear, CheckedListBoxItem).Value)
      Next

      m_ContactDetailForm.Show()

      If conactRecordNumber.HasValue Then
        m_ContactDetailForm.LoadContactData(m_CustomerNumber, responsiblePersonRecordNumber, conactRecordNumber, m_ContactFilterSettingsForDetailForm)
      Else
        Dim initalData As New InitalDataForNewContact With {.StartDateTime = DateTime.Now}
        m_ContactDetailForm.ActivateNewContactDataMode(m_CustomerNumber, responsiblePersonRecordNumber, initalData, m_ContactFilterSettingsForDetailForm)
      End If

      m_ContactDetailForm.BringToFront()

    End Sub

    ''' <summary>
    ''' Handles close of contact form.
    ''' </summary>
    Private Sub OnContactFormClosed(sender As System.Object, e As System.EventArgs)
      LoadYearsListData(m_CustomerNumber, m_RecordNumber)
      FilterContactData(m_CustomerNumber, m_RecordNumber)

      Dim contatsForm = CType(sender, frmContacts)

      If contatsForm.CurrentContactRecordNumber.HasValue Then
        FocusContactInfo(m_CustomerNumber, m_RecordNumber, contatsForm.CurrentContactRecordNumber)
      End If

    End Sub

    ''' <summary>
    ''' Handles contact form data saved.
    ''' </summary>
    Private Sub OnContactInfoFormDocumentDataSaved(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)

      LoadYearsListData(m_CustomerNumber, m_RecordNumber)
      FilterContactData(m_CustomerNumber, m_RecordNumber)

      Dim contatsForm = CType(sender, frmContacts)

      FocusContactInfo(m_CustomerNumber, responsiblePersonRecordNumber, contactRecordNumber)

    End Sub

    ''' <summary>
    ''' Handles contact form data deleted saved.
    ''' </summary>
    Private Sub OnContactInfoFormDocumentDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)
      LoadYearsListData(m_CustomerNumber, m_RecordNumber)
      FilterContactData(m_CustomerNumber, m_RecordNumber)
    End Sub

    ''' <summary>
    ''' Focuses a contact info.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responiblePersonRecordNumber">The responsible person record number.</param>
    ''' <param name="contactRecordNumber">The contact record number.</param>
    Private Sub FocusContactInfo(ByVal customerNumber As Integer, ByVal responiblePersonRecordNumber As Integer, ByVal contactRecordNumber As Integer)

      Dim listDataSource As BindingList(Of ContactViewData) = gridContactData.DataSource

      Dim contactViewData = listDataSource.Where(Function(data) data.CustomerNumber = customerNumber AndAlso
                                                                data.ResponsiblePersonRecordNumber = responiblePersonRecordNumber AndAlso
                                                                data.ContactRecorNumber = contactRecordNumber).FirstOrDefault()

      If Not contactViewData Is Nothing Then
        Dim sourceIndex = listDataSource.IndexOf(contactViewData)
        Dim rowHandle = gridViewContactData.GetRowHandle(sourceIndex)
        gridViewContactData.FocusedRowHandle = rowHandle
      End If
    End Sub

#End Region

#Region "View helper classes"

    ''' <summary>
    '''  Contact view data.
    ''' </summary>
    Class ContactViewData
      Public Property ID As Integer
      Public Property CustomerNumber As Integer
      Public Property ResponsiblePersonRecordNumber As Integer
      Public Property ContactRecorNumber As Integer
      Public Property ContactDate As DateTime?
      Public Property minContactDate As DateTime?
      Public Property maxContactDate As DateTime?
      Public Property Person_Subject As String
      Public Property Description As String
      Public Property Important As Boolean?
      Public Property Completed As Boolean?
      Public Property KST As String
      Public Property PDFImage As Image
      Public Property DocumentId As Integer?
    End Class

#End Region

    Private Sub groupContactData_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles grpKontakt.Paint

    End Sub
  End Class
End Namespace
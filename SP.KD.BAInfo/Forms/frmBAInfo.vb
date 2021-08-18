
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.KD.BAInfo.Settings
Imports DevExpress.XtraEditors.Repository
Imports SPProgUtility.Mandanten

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SP.TodoMng


Public Class frmBAInfo

  Public Delegate Sub CreditInfoDataSavedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal creditInfoRecordNumber As Integer)
  Public Delegate Sub CreditInfoDataDeletedHandler(ByVal sender As Object, ByVal customerNumber As Integer, ByVal creditInfoRecordNumber As Integer)

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
	''' The data access object.
	''' </summary>
	Private m_DataAccess As ICustomerDatabaseAccess

  ''' <summary>
  ''' The settings manager.
  ''' </summary>
  Protected m_SettingsManager As ISettingsManager

  ''' <summary>
  ''' The SPProgUtility object.
  ''' </summary>
  Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  ''' <summary>
  ''' UI Utility functions.
  ''' </summary>
  Private m_UtilityUI As UtilityUI

  ''' <summary>
  ''' Utility functions.
  ''' </summary>
  Private m_Utility As Utility

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger = New Logger()

  ''' <summary>
  ''' Contains the customer number of the loaded customer.
  ''' </summary>
  Private m_CustomerNumber As Integer

  ''' <summary>
  ''' Record number of selected document.
  ''' </summary>
  Private m_CurrentCreditInfoRecordNumber As Integer?

  ''' <summary>
  ''' Current file bytes.
  ''' </summary>
  Private m_CurrentFileBytes As Byte()

  ''' <summary>
  ''' Boolean flag indicating if initial data has been loaded.
  ''' </summary>
  Private m_IsInitialDataLoaded As Boolean = False

  ''' <summary>
  ''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
  ''' </summary>
  Private m_SuppressUIEvents As Boolean = False

  ''' <summary>
  ''' Check edit for active symbol.
  ''' </summary>
  Private m_CheckEditActive As RepositoryItemCheckEdit

  Private m_md As Mandant

#End Region


#Region "Events"

  Public Event CreditInfoDataSaved As CreditInfoDataSavedHandler
  Public Event CreditInfoDataDeleted As CreditInfoDataDeletedHandler

#End Region

#Region "Public Properties"

  ''' <summary>
  ''' Gets the selected credit info view data.
  ''' </summary>
  ''' <returns>The selected credit info or nothing if none is selected.</returns>
  Public ReadOnly Property SelectedCreditInfoViewData As CreditInfoViewData
    Get
      Dim grdView = TryCast(gridBAInfo.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

      If Not (grdView Is Nothing) Then

        Dim selectedRows = grdView.GetSelectedRows()

        If (selectedRows.Count > 0) Then
          Dim document = CType(grdView.GetRow(selectedRows(0)), CreditInfoViewData)
          Return document
        End If

      End If

      Return Nothing
    End Get

  End Property

  ''' <summary>
  ''' Gets the first credit info in the list of credi infos.
  ''' </summary>
  ''' <returns>First credit info in list or nothing.</returns>
  Public ReadOnly Property FirstDocumentInListOfDocuments As CreditInfoViewData
    Get
      If gvBAInfo.RowCount > 0 Then

        Dim rowHandle = gvBAInfo.GetVisibleRowHandle(0)
        Return CType(gvBAInfo.GetRow(rowHandle), CreditInfoViewData)
      Else
        Return Nothing
      End If

    End Get
  End Property

#End Region

#Region "Constructor"

  Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

    ' Dieser Aufruf ist für den Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Try
      ' Mandantendaten
      m_md = New Mandant
      m_InitializationData = _setting
      m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    End Try

    gvBAInfo.OptionsView.ShowIndicator = False

    m_CheckEditActive = CType(gridBAInfo.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
    m_CheckEditActive.PictureChecked = My.Resources.Checked
    m_CheckEditActive.PictureUnchecked = Nothing
    m_CheckEditActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

    m_DataAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
    m_SettingsManager = New SettingsManager
    m_UtilityUI = New UtilityUI
    m_Utility = New Utility

  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Loads credit info data.
  ''' </summary>
  ''' <param name="customerNumber">The customer record number</param>
  ''' <param name="recordNumber">The optional record number.</param>
  ''' <returns>Boolean flag indicating success.</returns>
  Public Function LoadCreditInfoData(ByVal customerNumber As Integer, ByVal recordNumber As Integer?) As Boolean

    Dim success As Boolean = True

    If Not m_IsInitialDataLoaded Then
      'Load inital data here.
      m_IsInitialDataLoaded = True
    End If

    ' Reset the form
    Reset()

    m_CustomerNumber = customerNumber

    If recordNumber.HasValue Then

      ' Load the credit info list.
      success = success AndAlso LoadCustomerAssignedCreditInfoData(customerNumber)
      FocusCreditInfo(customerNumber, recordNumber)
      success = success AndAlso LoadCreditInfoDetailData(customerNumber, recordNumber)
    Else
      success = success AndAlso LoadCustomerAssignedCreditInfoData(customerNumber)
      PrepareForNew()
    End If

    If Not success Then
      m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Daten konnten nicht geladen werden."))
    End If

    Return True
  End Function

  ''' <summary>
  ''' Reload the credit infos.
  ''' </summary>
  Public Sub ReloadCreditInfos()

    Dim selectedCreditInfoBeforeReload = SelectedCreditInfoViewData

    ' Reload credit infos
    LoadCustomerAssignedCreditInfoData(m_CustomerNumber)

    ' Select the previously selected record
    If Not selectedCreditInfoBeforeReload Is Nothing Then
      FocusCreditInfo(m_CustomerNumber, selectedCreditInfoBeforeReload.RecNr)

    End If

  End Sub

#End Region

#Region "Private Methods"

  ''' <summary>
  '''  trannslate controls
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub TranslateControls()

    Me.Text = m_translate.GetSafeTranslationValue(Me.Text)

		Me.grpdetails.Text = m_Translate.GetSafeTranslationValue(Me.grpdetails.Text)
		Me.lblDatei.Text = m_Translate.GetSafeTranslationValue(Me.lblDatei.Text)
		Me.btnOpenReport.Text = m_Translate.GetSafeTranslationValue(Me.btnOpenReport.Text)
		Me.lblArt.Text = m_Translate.GetSafeTranslationValue(Me.lblArt.Text)
		Me.lblBeschreibung.Text = m_Translate.GetSafeTranslationValue(Me.lblBeschreibung.Text)

		Me.chkActive.Text = m_Translate.GetSafeTranslationValue(Me.chkActive.Text)
		Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)

		Me.btnNewCreditInfo.Text = m_Translate.GetSafeTranslationValue(Me.btnNewCreditInfo.Text)
		Me.btnDeleteCreditInfo.Text = m_Translate.GetSafeTranslationValue(Me.btnDeleteCreditInfo.Text)
		Me.btnCreateTODO.Text = m_Translate.GetSafeTranslationValue(Me.btnCreateTODO.Text)

		Me.lblErstellt.Text = m_Translate.GetSafeTranslationValue(Me.lblErstellt.Text)
		Me.lblgeaendert.Text = m_Translate.GetSafeTranslationValue(Me.lblgeaendert.Text)

	End Sub

	''' <summary>
	''' Resets the from.
	''' </summary>
	Private Sub Reset()

    m_CustomerNumber = 0
    m_CurrentCreditInfoRecordNumber = Nothing
    m_CurrentFileBytes = Nothing

    txtFilePath.Text = String.Empty

    lblArchiveNr.Text = String.Empty
    lblTypeOfQuery.Text = m_translate.GetSafeTranslationValue("Keine Angabe")

    txtDescription.Text = String.Empty
    'txtDescription.Properties.MaxLength = 255

    chkActive.Checked = False

    btnOpenReport.Enabled = False
    btnDeleteCreditInfo.Enabled = True
    btnSolvencyDecision.Visible = False
    txtFilePath.Enabled = True

    ' ---Reset drop downs, grids and lists---
    ResetCreditInfoGrid()

    translatecontrols()

    btnDeleteCreditInfo.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 231)

    ' Clear errors
    errorProviderCreditInfo.Clear()
  End Sub

  ''' <summary>
  ''' Resets the credit info grid.
  ''' </summary>
  Private Sub ResetCreditInfoGrid()

    ' Reset the grid
    gvBAInfo.Columns.Clear()

    Dim columnFromDate As New DevExpress.XtraGrid.Columns.GridColumn()
    columnFromDate.Caption = m_translate.GetSafeTranslationValue("Ab")
    columnFromDate.Name = "FromDate"
    columnFromDate.FieldName = "FromDate"
    columnFromDate.Visible = True
    gvBAInfo.Columns.Add(columnFromDate)

    Dim columnToDate As New DevExpress.XtraGrid.Columns.GridColumn()
    columnToDate.Caption = m_translate.GetSafeTranslationValue("Bis")
    columnToDate.Name = "ToDate"
    columnToDate.FieldName = "ToDate"
    columnToDate.Visible = False
    gvBAInfo.Columns.Add(columnToDate)

    Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
    columnDescription.Caption = m_translate.GetSafeTranslationValue("Beschreibung")
    columnDescription.Name = "Description"
    columnDescription.FieldName = "Description"
    columnDescription.Visible = True
    gvBAInfo.Columns.Add(columnDescription)

    Dim hasPDFFileColumn As New DevExpress.XtraGrid.Columns.GridColumn()
    hasPDFFileColumn.Caption = m_translate.GetSafeTranslationValue("Dokument angehängt")
    hasPDFFileColumn.Name = "HasPDFFile"
    hasPDFFileColumn.FieldName = "HasPDFFile"
    hasPDFFileColumn.Visible = True
    hasPDFFileColumn.ColumnEdit = m_CheckEditActive
    gvBAInfo.Columns.Add(hasPDFFileColumn)

    m_SuppressUIEvents = True
    gridBAInfo.DataSource = Nothing
    m_SuppressUIEvents = False
  End Sub

  ''' <summary>
  ''' Loads customer assigned credit info data.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <returns>Boolean flag indicating success</returns>
  ''' <remarks></remarks>
  Private Function LoadCustomerAssignedCreditInfoData(ByVal customerNumber As Integer) As Boolean

    Dim creditInfoData = m_DataAccess.LoadAssignedCreditInfosOfCustomer(customerNumber, Nothing, False)

    If (creditInfoData Is Nothing) Then

      m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Kreditinfo-Daten konnten nicht geladen werden."))

      Return False
    End If

    Dim listDataSource As BindingList(Of CreditInfoViewData) = New BindingList(Of CreditInfoViewData)

    ' Convert the data to view data.
    For Each creditInfo In creditInfoData

      Dim creditInfoViewData As New CreditInfoViewData With {
          .CustomerNumber = creditInfo.CustomerNumber,
          .RecNr = creditInfo.RecordNumber,
          .FromDate = creditInfo.FromDate,
          .ToDate = creditInfo.ToDate,
          .Description = creditInfo.Description,
          .HasPDFFile = creditInfo.HasPDFFileFlag
      }

      listDataSource.Add(creditInfoViewData)

    Next

    m_SuppressUIEvents = True
    gridBAInfo.DataSource = listDataSource
    m_SuppressUIEvents = False

    Return True
  End Function

  ''' <summary>
  ''' Handles focus change of credit info row.
  ''' </summary>
  Private Sub OnCreditInfo_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvBAInfo.FocusedRowChanged

    If m_SuppressUIEvents Then
      Return
    End If

    Dim selectedCreditInfo = SelectedCreditInfoViewData

    If Not selectedCreditInfo Is Nothing Then
      LoadCreditInfoDetailData(selectedCreditInfo.CustomerNumber, selectedCreditInfo.RecNr)
    End If

  End Sub

  ''' <summary>
  ''' Load credit info detail data.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <param name="creditInfoNumber">The credit info number.</param>
  ''' <returns>Boolean flag indicating success.</returns>
  Private Function LoadCreditInfoDetailData(ByVal customerNumber As Integer, ByVal creditInfoNumber As Integer) As Boolean

    ' Clear errors
    errorProviderCreditInfo.Clear()

    Dim creditInfoList = m_DataAccess.LoadAssignedCreditInfosOfCustomer(customerNumber, creditInfoNumber, False)

    If Not creditInfoList Is Nothing AndAlso creditInfoList.Count = 1 Then

      Dim creditInfoData = creditInfoList(0)

      lblArchiveNr.Text = If(String.IsNullOrEmpty(creditInfoData.DV_ArchiveID), String.Empty, String.Format(m_translate.GetSafeTranslationValue("Archiv-Nr. {0}"), creditInfoData.DV_ArchiveID))
      txtDescription.Text = creditInfoData.Description
      chkActive.Checked = creditInfoData.ActiveRec.HasValue AndAlso creditInfoData.ActiveRec.Value

      lblCreditInfoCreated.Text = String.Format("{0:f}, {1}", creditInfoData.CreatedOn, creditInfoData.CreatedFrom)
      lblCreditInfoChanged.Text = String.Format("{0:f}, {1}", creditInfoData.ChangedOn, creditInfoData.ChangedFrom)

      ' Query type
      Select Case creditInfoData.DV_QueryType
        Case BusinessSolvencyCheckType.QuickBusinessCheck
          lblTypeOfQuery.Text = m_translate.GetSafeTranslationValue("Quick Business Prüfung")
        Case BusinessSolvencyCheckType.BusinessCheck
          lblTypeOfQuery.Text = m_translate.GetSafeTranslationValue("Business Prüfung")
        Case Else
          lblTypeOfQuery.Text = m_translate.GetSafeTranslationValue("Keine Angabe")
      End Select

      ' Decision type
      If creditInfoData.DV_DecisionID.HasValue Then
        Try
          Dim decision = (CType(creditInfoData.DV_DecisionID, DecisionResult))
          btnSolvencyDecision.Visible = True

          Select Case decision
            Case DecisionResult.LightGreen
              btnSolvencyDecision.Image = My.Resources.bullet_green_small
            Case DecisionResult.Green
              btnSolvencyDecision.Image = My.Resources.bullet_green_small
            Case DecisionResult.YellowGreen
              btnSolvencyDecision.Image = My.Resources.bullet_green_small
            Case DecisionResult.Yellow
              btnSolvencyDecision.Image = My.Resources.bullet_yellow_small
            Case DecisionResult.Orange
              btnSolvencyDecision.Image = My.Resources.bullet_yellow_small
            Case DecisionResult.Red
              btnSolvencyDecision.Image = My.Resources.bullet_red_small
            Case DecisionResult.DarkRed
              btnSolvencyDecision.Image = My.Resources.bullet_red_small
            Case Else
              btnSolvencyDecision.Visible = False
              btnSolvencyDecision.Image = Nothing
          End Select
        Catch ex As Exception
          m_Logger.LogError(ex.ToString())
        End Try
      Else
        btnSolvencyDecision.Visible = False
      End If

      ' Enalbe/disable 
      btnOpenReport.Enabled = creditInfoData.HasPDFFileFlag
      btnDeleteCreditInfo.Enabled = String.IsNullOrEmpty(creditInfoData.DV_ArchiveID) And IsUserActionAllowed(m_InitializationData.UserData.UserNr, 231)

      txtFilePath.Enabled = String.IsNullOrEmpty(creditInfoData.DV_ArchiveID)

      m_CurrentCreditInfoRecordNumber = creditInfoData.RecordNumber

      Return True
    Else
      Return False
    End If

  End Function

  ''' <summary>
  ''' Focuses a credit info.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <param name="creditInfoRecordNumber">The credit info record number</param>
  Private Sub FocusCreditInfo(ByVal customerNumber As Integer, ByVal creditInfoRecordNumber As Integer)

    If Not gridBAInfo.DataSource Is Nothing Then

      Dim credInfoViewdata = CType(gvBAInfo.DataSource, BindingList(Of CreditInfoViewData))

      Dim index = credInfoViewdata.ToList().FindIndex(Function(data) data.CustomerNumber = customerNumber And data.RecNr = creditInfoRecordNumber)

      m_SuppressUIEvents = True
      Dim rowHandle = gvBAInfo.GetRowHandle(index)
      gvBAInfo.FocusedRowHandle = rowHandle
      m_SuppressUIEvents = False
    End If

  End Sub

  ''' <summary>
  ''' Handles click on new button.
  ''' </summary>
  Private Sub OnBtnNewCreditInfo_Click(sender As System.Object, e As System.EventArgs) Handles btnNewCreditInfo.Click
    PrepareForNew()
  End Sub

  ''' <summary>
  ''' Handles click on save button.
  ''' </summary>
  Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
    If ValidateCreditInfotInputData() Then

      Dim creditInfoData As CustomerAssignedCreditInfo = Nothing

      Dim dt = DateTime.Now
      If Not m_CurrentCreditInfoRecordNumber.HasValue Then
        creditInfoData = New CustomerAssignedCreditInfo With {.CustomerNumber = m_CustomerNumber,
                                                             .FromDate = dt,
                                                            .CreatedOn = dt,
                                                            .CreatedFrom = m_ClsProgSetting.GetUserName()}
      Else

        Dim creditInfoList = m_DataAccess.LoadAssignedCreditInfosOfCustomer(m_CustomerNumber, m_CurrentCreditInfoRecordNumber, False)

        If creditInfoList Is Nothing OrElse Not creditInfoList.Count = 1 Then
          m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
          Return
        End If

        creditInfoData = creditInfoList(0)
      End If

      creditInfoData.Description = txtDescription.Text
      creditInfoData.ActiveRec = chkActive.Checked
      creditInfoData.ChangedFrom = m_ClsProgSetting.GetUserName()
      creditInfoData.ChangedOn = dt
      creditInfoData.USNr = m_InitializationData.UserData.UserNr

      Dim success As Boolean = True

      ' Insert or update document
      If creditInfoData.ID = 0 Then
        success = m_DataAccess.AddCustomerCreditInfoAssignment(creditInfoData)
        m_CurrentCreditInfoRecordNumber = creditInfoData.RecordNumber
      Else
        success = m_DataAccess.UpdateCustomerAssignedCreditInfoData(creditInfoData)
      End If

      ' Check if the document bytes must also be saved.
      If Not (m_CurrentFileBytes Is Nothing) Then
        success = success AndAlso m_DataAccess.UpdateCustomerAssignedCreditInfoByteData(creditInfoData.ID, m_CurrentFileBytes)

        If (success) Then
          creditInfoData.HasPDFFileFlag = True
        End If

      End If
      m_CurrentFileBytes = Nothing

      txtFilePath.Text = String.Empty

      If Not success Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
      Else

        ' Update credit info dates.
        lblCreditInfoCreated.Text = String.Format("{0:f}, {1}", creditInfoData.CreatedOn, creditInfoData.CreatedFrom)
        lblCreditInfoChanged.Text = String.Format("{0:f}, {1}", creditInfoData.ChangedOn, creditInfoData.ChangedFrom)

        ' Load the credit info list data again.
        LoadCustomerAssignedCreditInfoData(m_CustomerNumber)
        FocusCreditInfo(m_CustomerNumber, creditInfoData.RecordNumber)

        btnOpenReport.Enabled = creditInfoData.HasPDFFileFlag

        RaiseEvent CreditInfoDataSaved(Me, m_CustomerNumber, creditInfoData.RecordNumber)

      End If

    End If
  End Sub

  ''' <summary>
  ''' Handles click on delete button.
  ''' </summary>
  Private Sub OnBtnDeleteCreditInfo_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteCreditInfo.Click

    If Not m_CurrentCreditInfoRecordNumber Is Nothing Then

      Dim recordToDeleteList = m_DataAccess.LoadAssignedCreditInfosOfCustomer(m_CustomerNumber, m_CurrentCreditInfoRecordNumber, False)

      If recordToDeleteList Is Nothing OrElse Not recordToDeleteList.Count = 1 Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Der Datensatz konnte nicht gelöscht werden."))
        Return
      End If

      Dim recordToDelete = recordToDeleteList(0)

      If (m_UtilityUI.ShowYesNoDialog(m_translate.GetSafeTranslationValue("Möchten Sie diesen Datensatz wirklich löschen?"), m_translate.GetSafeTranslationValue("Datensatz entgültig löschen?"))) Then
        Dim success = m_DataAccess.DeleteCustomerCreditInfoAssignment(recordToDelete.ID)

        If Not success Then
          m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Der Datensatz konnte nicht gelöscht werden."))
          Return
        End If


        ' Load the category list data again.

        LoadCustomerAssignedCreditInfoData(m_CustomerNumber)

        ' Load the detail data of the first credit info in the list.
        Dim firstCreditInfo = FirstDocumentInListOfDocuments

        If Not firstCreditInfo Is Nothing Then
          LoadCreditInfoDetailData(m_CustomerNumber, firstCreditInfo.RecNr)
        Else
          ' Keep current detail data.
        End If

        m_CurrentFileBytes = Nothing

        txtFilePath.Text = String.Empty

        RaiseEvent CreditInfoDataDeleted(Me, m_CustomerNumber, recordToDelete.RecordNumber)

      End If

    End If

  End Sub

  ''' <summary>
  ''' Prepare form for new credit info.
  ''' </summary>
  Private Sub PrepareForNew()

    m_CurrentCreditInfoRecordNumber = Nothing
    m_CurrentFileBytes = Nothing

    txtFilePath.Text = String.Empty
    txtFilePath.Enabled = True

    lblArchiveNr.Text = String.Empty
    lblTypeOfQuery.Text = m_translate.GetSafeTranslationValue("Keine Angabe")
    txtDescription.Text = String.Empty
    chkActive.Checked = False

    btnOpenReport.Enabled = False
    btnDeleteCreditInfo.Enabled = True
    btnSolvencyDecision.Visible = False

    lblCreditInfoCreated.Text = "-"
    lblCreditInfoChanged.Text = "-"

    ' Clear errors
    errorProviderCreditInfo.Clear()

  End Sub

  ''' <summary>
  ''' Validates credit info input data.
  ''' </summary>
  Private Function ValidateCreditInfotInputData() As Boolean

    Dim errorText As String = m_translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

    Dim isValid As Boolean = True

    isValid = isValid And SetErrorIfInvalid(txtDescription, errorProviderCreditInfo, String.IsNullOrEmpty(txtDescription.Text), errorText)

    Return isValid
  End Function

  ''' <summary>
  ''' Validates a control.
  ''' </summary>
  ''' <param name="control">The control to validate.</param>
  ''' <param name="errorProvider">The error providor.</param>
  ''' <param name="invalid">Boolean flag if data is invalid.</param>
  ''' <param name="errorText">The error text.</param>
  ''' <returns>Valid flag</returns>
  Private Function SetErrorIfInvalid(ByVal control As Control, ByVal errorProvider As ErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

    If (invalid) Then
      errorProvider.SetError(control, errorText)
    Else
      errorProvider.SetError(control, String.Empty)
    End If

    Return Not invalid

  End Function

  ''' <summary>
  ''' Handles click on file path button.
  ''' </summary>
  Private Sub OnTxtFilePath_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFilePath.ButtonClick

    If e.Button.Index = 0 Then

      With OpenFileDialog1
        .Filter = _
        "PDF-Dokumente (*.pdf)|*.pdf"
        .FilterIndex = 1
        .InitialDirectory = If(txtFilePath.Text = String.Empty, m_ClsProgSetting.GetUserHomePath, txtFilePath.Text)
        .Title = m_translate.GetSafeTranslationValue("PDF Dokument wählen")
        .FileName = String.Empty

        If .ShowDialog() = DialogResult.OK Then

          txtFilePath.Text = String.Empty
          m_CurrentFileBytes = m_Utility.LoadFileBytes(.FileName)

          If m_CurrentFileBytes Is Nothing Then
            m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Die Datei konnte nicht geöffnet werden."))
          Else

            txtFilePath.Text = .FileName
          End If

        End If
      End With

    End If

  End Sub

  ''' <summary>
  ''' Handles form load event.
  ''' </summary>
  Private Sub OnFrmBAInfo_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    Me.KeyPreview = True
    Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
    If strStyleName <> String.Empty Then
      UserLookAndFeel.Default.SetSkinStyle(strStyleName)
    End If

    Try
      Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
      Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
      Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
      Dim setting_form_mainsplitter = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_MAINSPLITTER)

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

  End Sub

  ''' <summary>
  ''' Handles the form disposed event.
  ''' </summary>
  Private Sub OnFrmBAInfo_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

    ' Save form location, width and height in setttings
    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)

        m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_MAINSPLITTER, Me.sccMain.SplitterPosition)

        m_SettingsManager.SaveSettings()
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())

    End Try

  End Sub

  ''' <summary>
  ''' Handles click on open report button.
  ''' </summary>
  Private Sub OnBtnOpenReport_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenReport.Click

    If m_CurrentCreditInfoRecordNumber.HasValue Then
      ShowReport(m_CustomerNumber, m_CurrentCreditInfoRecordNumber)
    End If

  End Sub

  ''' <summary>
  ''' Shows a credit info report.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <param name="recordNumber">The record number.</param>
  Private Sub ShowReport(ByVal customerNumber As Integer, ByVal recordNumber As Integer)

    Dim customerCreditInfoList = m_DataAccess.LoadAssignedCreditInfosOfCustomer(customerNumber, recordNumber, True)

    If Not customerCreditInfoList Is Nothing AndAlso
            customerCreditInfoList.Count = 1 AndAlso
         Not customerCreditInfoList(0).DV_PDFFile Is Nothing Then

      Dim bytes() = customerCreditInfoList(0).DV_PDFFile
      Dim tempFileName = System.IO.Path.GetTempFileName()
      Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

      If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then
        m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

      End If

    End If

  End Sub

  Private Sub OnbtnCreateTODO_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateTODO.Click

    Dim frmTodo As New frmTodo(m_InitializationData)
    ' optional init new todo
    Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
    Dim EmployeeNumber As Integer? = Nothing
    Dim CustomerNumber As Integer? = m_CustomerNumber
    Dim ResponsiblePersonRecordNumber As Integer? = Nothing
    Dim VacancyNumber As Integer? = Nothing
    Dim ProposeNumber As Integer? = Nothing
    Dim ESNumber As Integer? = Nothing
    Dim RPNumber As Integer? = Nothing
    Dim LMNumber As Integer? = Nothing
    Dim RENumber As Integer? = Nothing
    Dim ZENumber As Integer? = Nothing
    Dim Subject As String = String.Empty
    Dim Body As String = ""

		frmTodo.InitNewTodo(UserNumber, Subject, Body, EmployeeNumber, CustomerNumber, ResponsiblePersonRecordNumber,
												VacancyNumber, ProposeNumber, ESNumber, RPNumber, LMNumber, RENumber, ZENumber)

		frmTodo.Show()

  End Sub

#End Region

#Region "View helper classes"

  ''' <summary>
  ''' CreditInfo view data.
  ''' </summary>
  Class CreditInfoViewData
    Public Property CustomerNumber As Integer
    Public Property RecNr As Integer
    Public Property FromDate As DateTime?
    Public Property ToDate As DateTime?
    Public Property Description As String
    Public Property HasPDFFile As Boolean
  End Class

#End Region

  Private Sub btnCreateTODO_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateTODO.Click

  End Sub
End Class
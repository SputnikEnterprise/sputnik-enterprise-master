
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports DevExpress.XtraEditors
Imports SP.Infrastructure.ucListSelectPopup
Imports SP.Infrastructure

Namespace UI

  ''' <summary>
  ''' Disposal data.
  ''' </summary>
  Public Class ucDisposal
#Region "Private Consts"

    Private Const POPUP_DEFAULT_WIDTH As Integer = 300
    Private Const POPUP_DEFAULT_HEIGHT As Integer = 280

#End Region

#Region "Private Fields"

    ''' <summary>
    ''' Communication popup data.
    ''' </summary>
    Private m_CommunicationPopupData As IEnumerable(Of CustomerCommunicationData)

    ''' <summary>
    ''' Communication type popup data.
    ''' </summary>
    Private m_CommunicationTypePopupData As IEnumerable(Of CustomerCommunicationTypeData)

    ''' <summary>
    ''' Reserve popup data (array).
    ''' </summary>
    Private m_ReservePopupData() As IEnumerable(Of ResponsiblePersonReserveData)

    ''' <summary>
    ''' Communication popup column definitions.
    ''' </summary>
    Private m_CommunicationPopupColumns As New List(Of PopupColumDefintion)

    ''' <summary>
    ''' Communication type popup column definitions.
    ''' </summary>
    Private m_CommunicationTypePopupColumns As New List(Of PopupColumDefintion)

    ''' <summary>
    ''' Reserve popup column definitions (use for reserve data 1 to 4).
    ''' </summary>
    Private m_ReservePopupColumns As New List(Of PopupColumDefintion)

#End Region

#Region "Concturctor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Register popup row click handlers
      AddHandler ucComDataPopup.RowClicked, AddressOf OnPopupRowClicked
      AddHandler ucContactTypeDataPopup.RowClicked, AddressOf OnPopupRowClicked
      AddHandler ucReserve1Popup.RowClicked, AddressOf OnPopupRowClicked
      AddHandler ucReserve2Popup.RowClicked, AddressOf OnPopupRowClicked
      AddHandler ucReserve3Popup.RowClicked, AddressOf OnPopupRowClicked
      AddHandler ucReserve4Popup.RowClicked, AddressOf OnPopupRowClicked

      ' Register size changed handlers
      AddHandler ucComDataPopup.PopupSizeChanged, AddressOf OnPopupSizeChanged
      AddHandler ucContactTypeDataPopup.PopupSizeChanged, AddressOf OnPopupSizeChanged
      AddHandler ucReserve1Popup.PopupSizeChanged, AddressOf OnPopupSizeChanged
      AddHandler ucReserve2Popup.PopupSizeChanged, AddressOf OnPopupSizeChanged
      AddHandler ucReserve3Popup.PopupSizeChanged, AddressOf OnPopupSizeChanged
      AddHandler ucReserve4Popup.PopupSizeChanged, AddressOf OnPopupSizeChanged

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

      ' Create array to store reserve popup data 1 to 4.
      m_ReservePopupData = New IEnumerable(Of ResponsiblePersonReserveData)(3) {}

      ' Column defintions for popups
      m_CommunicationPopupColumns.Add(New PopupColumDefintion With {.Name = "Description", .Translation = "Name"})
      m_CommunicationTypePopupColumns.Add(New PopupColumDefintion With {.Name = "Description", .Translation = "Name"})
      m_ReservePopupColumns.Add(New PopupColumDefintion With {.Name = "Description", .Translation = "Name"})
    
    End Sub

    ''' <summary>
    ''' Activates the control.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="recordNumber">The record number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function Activate(ByVal customerNumber As Integer, ByVal recordNumber As Integer?) As Boolean
      m_SuppressUIEvents = True
      Dim success As Boolean = True

      HidePopups()

      If (recordNumber.HasValue) Then
        If (Not IsResponsiblePersonDataLoaded) Then
          success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
        ElseIf Not customerNumber = m_CustomerNumber Or
               Not m_RecordNumber = recordNumber Then
          success = success AndAlso LoadResponsiblePersonData(customerNumber, recordNumber)
        End If
      Else
        Reset()
      End If
      m_SuppressUIEvents = False

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

      HidePopups()

      m_CustomerNumber = 0
      m_RecordNumber = Nothing

      lstCommData.DataSource = Nothing
      lstContactType.DataSource = Nothing
      lstReserve1.DataSource = Nothing
      lstReserve2.DataSource = Nothing
      lstReserve3.DataSource = Nothing
      lstReserve4.DataSource = Nothing

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
      HidePopups()
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.grpKommunikation.Text = m_translate.GetSafeTranslationValue(Me.grpKommunikation.Text, True)
      Me.grpVersandart.Text = m_translate.GetSafeTranslationValue(Me.grpVersandart.Text, True)

      Me.grp1Reserve.Text = m_translate.GetSafeTranslationValue(Me.grp1Reserve.Text, True)
      Me.grp2Reserve.Text = m_translate.GetSafeTranslationValue(Me.grp2Reserve.Text, True)
      Me.grp3Reserve.Text = m_translate.GetSafeTranslationValue(Me.grp3Reserve.Text, True)
      Me.grp4Reserve.Text = m_translate.GetSafeTranslationValue(Me.grp4Reserve.Text, True)

    End Sub

    ''' <summary>
    ''' Loads communication popup data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadCommunicationPopupData() As Boolean

      m_CommunicationPopupData = m_DataAccess.LoadCustomerCommunicationData()

      If (m_CommunicationPopupData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Kommunikationsauswahldaten konnten nicht geladen werden."))
        Return False
      End If

      Return True
    End Function

    ''' <summary>
    ''' Loads communication type popup data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadCommunicationTypePopupData() As Boolean

      m_CommunicationTypePopupData = m_DataAccess.LoadCustomerCommunicationTypeData()

      If (m_CommunicationTypePopupData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Versandartauswahldaten konnten nicht geladen werden."))
        Return False
      End If

      Return True
    End Function

    ''' <summary>
    ''' Loads reserve popup data.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadReservePopupData(ByVal reserveType As ResponsiblePersonReserveDataType) As Boolean

      Dim reserveIntType = CType(reserveType, Integer)
      m_ReservePopupData(reserveIntType - 1) = m_DataAccess.LoadResponsiblePersonReserveData(reserveType)

      If (m_ReservePopupData(reserveIntType - 1) Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(String.Format(m_translate.GetSafeTranslationValue("Reservedaten {0} konnten nicht geladen werden."), reserveIntType))
        Return False
      End If

      Return True
    End Function

    ''' <summary>
    '''  Loads responsible person data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="recordNumber">The record number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadResponsiblePersonData(ByVal customerNumber As Integer, ByVal recordNumber As Integer) As Boolean

      Dim success As Boolean = True

      success = success AndAlso LoadAssignedCommunicationData(customerNumber, recordNumber)
      success = success AndAlso LoadAssignedContactTypeData(customerNumber, recordNumber)
      success = success AndAlso LoadAssignedReserveData(customerNumber, recordNumber, ResponsiblePersonReserveDataType.Reserve1)
      success = success AndAlso LoadAssignedReserveData(customerNumber, recordNumber, ResponsiblePersonReserveDataType.Reserve2)
      success = success AndAlso LoadAssignedReserveData(customerNumber, recordNumber, ResponsiblePersonReserveDataType.Reserve3)
      success = success AndAlso LoadAssignedReserveData(customerNumber, recordNumber, ResponsiblePersonReserveDataType.Reserve4)

      m_CustomerNumber = IIf(success, customerNumber, 0)
      m_RecordNumber = IIf(success, recordNumber, Nothing)

      Return success

    End Function

    ''' <summary>
    ''' Loads assigned communication data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadAssignedCommunicationData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As Boolean

      Dim communicationData = m_DataAccess.LoadAsssignedCommunicationDataOfResponsiblePerson(customerNumber, responsiblePersonRecordNumber)

      If (communicationData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Zugeordnete Kommunkationsdaten konnten nicht geladen werden."))
        Return False
      End If

      lstCommData.DisplayMember = "Description"
      lstCommData.ValueMember = "ID"
      lstCommData.DataSource = communicationData

      Return True

    End Function

    ''' <summary>
    ''' Loads assigned contact type data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadAssignedContactTypeData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer) As Boolean

      Dim contactTypeData = m_DataAccess.LoadAssignedConcatTypeDataOfResponsiblePerson(customerNumber, responsiblePersonRecordNumber)

      If (contactTypeData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Zugeordnete Versandartdaten konnten nicht geladen werden."))
        Return False
      End If

      lstContactType.DisplayMember = "Description"
      lstContactType.ValueMember = "ID"
      lstContactType.DataSource = contactTypeData

      Return True

    End Function

    ''' <summary>
    ''' Loads assigned reserve data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonRecordNumber">The responsible person number.</param>
    ''' <param name="reserveType">The reserve type.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function LoadAssignedReserveData(ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer, ByVal reserveType As ResponsiblePersonReserveDataType) As Boolean

      Dim reserveData = m_DataAccess.LoadAssignedReserveDataOfResponsiblePerson(customerNumber, responsiblePersonRecordNumber, reserveType)

      If (reserveData Is Nothing) Then
        m_UtilityUI.ShowErrorDialog(String.Format(m_translate.GetSafeTranslationValue("Zugeordnete Reserve {0} Daten konnten nicht geladen werden."),
                                                  CType(reserveType, Integer)))
        Return False
      End If

      Dim reserveList As ListBoxControl = Nothing

      Select Case reserveType
        Case ResponsiblePersonReserveDataType.Reserve1
          reserveList = lstReserve1
        Case ResponsiblePersonReserveDataType.Reserve2
          reserveList = lstReserve2
        Case ResponsiblePersonReserveDataType.Reserve3
          reserveList = lstReserve3
        Case ResponsiblePersonReserveDataType.Reserve4
          reserveList = lstReserve4
      End Select

      If (Not lstReserve1 Is Nothing) Then
        reserveList.DisplayMember = "Description"
        reserveList.ValueMember = "ID"
        reserveList.DataSource = reserveData
      End If

      Return True

    End Function

    ''' <summary>
    ''' Reads a popup size setting.
    ''' </summary>
    ''' <param name="settingKey">The settings key.</param>
    ''' <returns>The size setting.</returns>
    Public Function ReadPopupSizeSetting(ByRef settingKey As String) As Size

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

    ''' <summary>
    ''' Hides all popups.
    ''' </summary>
    Private Sub HidePopups()
      ucComDataPopup.HidePopup()
      ucContactTypeDataPopup.HidePopup()
      ucReserve1Popup.HidePopup()
      ucReserve2Popup.HidePopup()
      ucReserve3Popup.HidePopup()
      ucReserve4Popup.HidePopup()
    End Sub

    ''' <summary>
    ''' Handles click on button add communication data.
    ''' </summary>
    Private Sub OnBtnAddCommunicationData_Click(sender As System.Object, e As System.EventArgs) Handles btnAddCommunicationData.Click

      HidePopups()

      Dim position = Cursor.Position
      If m_CommunicationPopupData Is Nothing Then
        LoadCommunicationPopupData()
      End If

      If Not m_CommunicationPopupData Is Nothing Then

        Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_DISPOSITION_COMMUNICATION_SIZE)

        ' Show popup
        ucComDataPopup.InitPopup(m_CommunicationPopupData, m_CommunicationPopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
        ucComDataPopup.ShowPopup(position, popupSize)
      End If

    End Sub

    ''' <summary>
    ''' Handles click on button add contact type data.
    ''' </summary>
    Private Sub OnBtnAddContactType_Click(sender As System.Object, e As System.EventArgs) Handles btnAddContactType.Click

      HidePopups()
      Dim position = Cursor.Position

      If m_CommunicationTypePopupData Is Nothing Then
        LoadCommunicationTypePopupData()
      End If

      If Not m_CommunicationTypePopupData Is Nothing Then

        Dim popupSize = ReadPopupSizeSetting(Settings.SettingKeys.SETTING_POPUP_DISPOSITION_CONTACT_TYPE_SIZE)

        ucContactTypeDataPopup.InitPopup(m_CommunicationTypePopupData, m_CommunicationTypePopupColumns, False, False, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
        ucContactTypeDataPopup.ShowPopup(position, popupSize)
      End If

    End Sub

    ''' <summary>
    ''' Handles click on button add reserve data.
    ''' </summary>
    Private Sub OnBtnAddReserve_Click(sender As System.Object, e As System.EventArgs) Handles btnAddReserve1.Click, btnAddReserve2.Click,
      btnAddReserve3.Click, btnAddReserve4.Click

      HidePopups()

      Dim buttonSender = CType(sender, SimpleButton)
      Dim popupData As IEnumerable(Of ResponsiblePersonReserveData) = Nothing
      Dim index As Integer? = Nothing
      Dim reserveType As ResponsiblePersonReserveDataType? = Nothing
      Dim popup As ucListSelectPopup = Nothing
      Dim settingsKey As String = String.Empty

      If Object.ReferenceEquals(sender, btnAddReserve1) Then
        index = 0
        reserveType = ResponsiblePersonReserveDataType.Reserve1
        popup = ucReserve1Popup
        settingsKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE1_SIZE
      ElseIf Object.ReferenceEquals(sender, btnAddReserve2) Then
        index = 1
        reserveType = ResponsiblePersonReserveDataType.Reserve2
        popup = ucReserve2Popup
        settingsKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE2_SIZE
      ElseIf Object.ReferenceEquals(sender, btnAddReserve3) Then
        index = 2
        reserveType = ResponsiblePersonReserveDataType.Reserve3
        popup = ucReserve3Popup
        settingsKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE3_SIZE
      ElseIf Object.ReferenceEquals(sender, btnAddReserve4) Then
        index = 3
        reserveType = ResponsiblePersonReserveDataType.Reserve4
        popup = ucReserve4Popup
        settingsKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE4_SIZE
      End If

      If index.HasValue Then

        Dim position = Cursor.Position

        If m_ReservePopupData(index) Is Nothing Then
          LoadReservePopupData(reserveType)
        End If

        If Not m_ReservePopupData(index) Is Nothing Then

          Dim popupSize = ReadPopupSizeSetting(settingsKey)

          popup.InitPopup(m_ReservePopupData(index), m_ReservePopupColumns, False, True, DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never)
          popup.ShowPopup(position, popupSize)
        End If
      End If
    End Sub

    ''' <summary>
    ''' Handles click on a row on one of the popups.
    ''' </summary>
    Private Sub OnPopupRowClicked(ByVal sender As Object, ByVal clickedObject As Object)

      Dim success As Boolean = True

      If Object.ReferenceEquals(sender, ucComDataPopup) AndAlso
          TypeOf clickedObject Is CustomerCommunicationData Then
        success = AssignCommuncationDataToResponsiblePerson(CType(clickedObject, CustomerCommunicationData))
      ElseIf Object.ReferenceEquals(sender, ucContactTypeDataPopup) AndAlso
          TypeOf clickedObject Is CustomerCommunicationTypeData Then
        success = AssignContactTypeDataToResponsiblePerson(CType(clickedObject, CustomerCommunicationTypeData))
      ElseIf Object.ReferenceEquals(sender, ucReserve1Popup) AndAlso
          TypeOf clickedObject Is ResponsiblePersonReserveData Then
        success = AssignReserveDataToResponsiblePerson(CType(clickedObject, ResponsiblePersonReserveData), ResponsiblePersonReserveDataType.Reserve1)
      ElseIf Object.ReferenceEquals(sender, ucReserve2Popup) AndAlso
          TypeOf clickedObject Is ResponsiblePersonReserveData Then
        success = AssignReserveDataToResponsiblePerson(CType(clickedObject, ResponsiblePersonReserveData), ResponsiblePersonReserveDataType.Reserve2)
      ElseIf Object.ReferenceEquals(sender, ucReserve3Popup) AndAlso
          TypeOf clickedObject Is ResponsiblePersonReserveData Then
        success = AssignReserveDataToResponsiblePerson(CType(clickedObject, ResponsiblePersonReserveData), ResponsiblePersonReserveDataType.Reserve3)
      ElseIf Object.ReferenceEquals(sender, ucReserve4Popup) AndAlso
          TypeOf clickedObject Is ResponsiblePersonReserveData Then
        success = AssignReserveDataToResponsiblePerson(CType(clickedObject, ResponsiblePersonReserveData), ResponsiblePersonReserveDataType.Reserve4)
      End If

      HidePopups()

      If Not success Then
        m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Zuordnung konnte nicht duchgeführt werden."))
      End If

    End Sub

    ''' <summary>
    ''' Handles size changs of popups.
    ''' </summary>
    Private Sub OnPopupSizeChanged(ByVal sender As Object, ByVal newWidth As Integer, ByVal newHeight As Integer)
      Dim setting As String = String.Format("{0};{1}", newWidth, newHeight)
      Dim settingKey As String = String.Empty

      If Object.ReferenceEquals(sender, ucComDataPopup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_COMMUNICATION_SIZE
      ElseIf Object.ReferenceEquals(sender, ucContactTypeDataPopup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_CONTACT_TYPE_SIZE
      ElseIf Object.ReferenceEquals(sender, ucReserve1Popup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE1_SIZE
      ElseIf Object.ReferenceEquals(sender, ucReserve2Popup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE2_SIZE
      ElseIf Object.ReferenceEquals(sender, ucReserve3Popup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE3_SIZE
      ElseIf Object.ReferenceEquals(sender, ucReserve4Popup) Then
        settingKey = Settings.SettingKeys.SETTING_POPUP_DISPOSITION_RESERVE4_SIZE
      End If

      Try
        If Not String.IsNullOrEmpty(settingKey) Then
          m_SettingsManager.WriteString(settingKey, setting)
          m_SettingsManager.SaveSettings()
        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

    End Sub

    ''' <summary>
    ''' Assign communication data to a responsible person.
    ''' </summary>
    ''' <param name="communicationDataToAdd">The communication data to add.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function AssignCommuncationDataToResponsiblePerson(ByVal communicationDataToAdd As CustomerCommunicationData) As Boolean

      Dim succcess = True

      ' Load assigned communication data.
      Dim assignedCommunicationData = m_DataAccess.LoadAsssignedCommunicationDataOfResponsiblePerson(m_CustomerNumber, m_RecordNumber)

      If Not assignedCommunicationData Is Nothing Then

        ' Check if the new communication data is not already assigned
        If Not assignedCommunicationData.Any(Function(data) data.Description.ToLower().Trim() = communicationDataToAdd.Description.ToLower().Trim()) Then

          ' Add to database.
          Dim communicationDataToAssign = New ResponsiblePersonAssignedCommuncationData With {.CustomerNumber = m_CustomerNumber,
                                                                                              .ResponsiblePersonRecordNumber = m_RecordNumber,
                                                                                              .Description = communicationDataToAdd.Description}
          succcess = m_DataAccess.AddResponsiblePersonCommunicationAssignment(communicationDataToAssign)
        End If
      Else
        succcess = False
      End If

      LoadAssignedCommunicationData(m_CustomerNumber, m_RecordNumber)

      Return succcess

    End Function

    ''' <summary>
    ''' Assign contact type data to a responsible person.
    ''' </summary>
    ''' <param name="contactTypeToAdd">The contact type data to add.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function AssignContactTypeDataToResponsiblePerson(ByVal contactTypeToAdd As CustomerCommunicationTypeData) As Boolean

      Dim succcess = True

      ' Load assigned contact type data.
      Dim assignedContactTypeData = m_DataAccess.LoadAssignedConcatTypeDataOfResponsiblePerson(m_CustomerNumber, m_RecordNumber)

      If Not assignedContactTypeData Is Nothing Then

        ' Check if the new contact type data is not already assigned
        If Not assignedContactTypeData.Any(Function(data) data.Description.ToLower().Trim() = contactTypeToAdd.Description.ToLower().Trim()) Then

          ' Add to database.
          Dim contactTypeDataToAssign = New ResponsiblePersonAssignedContactTypeData With {.CustomerNumber = m_CustomerNumber,
                                                                                           .ResponsiblePersonRecordNumber = m_RecordNumber,
                                                                                           .Description = contactTypeToAdd.Description}
          succcess = m_DataAccess.AddResponsiblePersonContactTypeAssignment(contactTypeDataToAssign)
        End If
      Else
        succcess = False
      End If

      LoadAssignedContactTypeData(m_CustomerNumber, m_RecordNumber)

      Return succcess

    End Function


    ''' <summary>
    ''' Assign reserve data to a responsible person.
    ''' </summary>
    ''' <param name="reserveDataToAdd">The reserve data to add.</param>
    ''' <param name="reserveType">The reserve type.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Private Function AssignReserveDataToResponsiblePerson(ByVal reserveDataToAdd As ResponsiblePersonReserveData, ByRef reserveType As ResponsiblePersonReserveDataType) As Boolean

      Dim succcess = True

      ' Load assigned reserve data.
      Dim assignedReserveData = m_DataAccess.LoadAssignedReserveDataOfResponsiblePerson(m_CustomerNumber, m_RecordNumber, reserveType)

      If Not assignedReserveData Is Nothing Then

        ' Check if the new reserve data is not already assigned
        If Not assignedReserveData.Any(Function(data) data.Description.ToLower().Trim() = reserveDataToAdd.Description.ToLower().Trim()) Then

          ' Add to database.
          Dim reserveToAssign = New ResponsiblePersonAssignedReserveData With {.CustomerNumber = m_CustomerNumber,
                                                                               .ResponsiblePersonRecordNumber = m_RecordNumber,
                                                                               .Description = reserveDataToAdd.Description}
          succcess = m_DataAccess.AddResponsiblePersonReserveAssignment(reserveToAssign, reserveType)
        End If
      Else
        succcess = False
      End If

      LoadAssignedReserveData(m_CustomerNumber, m_RecordNumber, reserveType)

      Return succcess

    End Function

    ''' <summary>
    ''' Handles keydown event on communication data list.
    ''' </summary>
    Private Sub OnLstCommData_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstCommData.KeyDown

      If (Not IsResponsiblePersonDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then

        Dim selectedCommunicationData As ResponsiblePersonAssignedCommuncationData = TryCast(lstCommData.SelectedItem, ResponsiblePersonAssignedCommuncationData)

        If (Not selectedCommunicationData Is Nothing) Then

          If Not m_DataAccess.DeleteResponsiblePersonCommunicationAssignment(selectedCommunicationData.ID) Then
            m_UtilityUI.ShowErrorDialog("Kommunikation konnte nicht gelöscht werden.")
          End If

          LoadAssignedCommunicationData(m_CustomerNumber, m_RecordNumber)

        End If

      End If
    End Sub

    ''' <summary>
    ''' Handles keydown event on contact type list.
    ''' </summary>
    Private Sub OnLstContactType_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstContactType.KeyDown

      If (Not IsResponsiblePersonDataLoaded) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then

        Dim selectedContactTypeData As ResponsiblePersonAssignedContactTypeData = TryCast(lstContactType.SelectedItem, ResponsiblePersonAssignedContactTypeData)

        If (Not selectedContactTypeData Is Nothing) Then

          If Not m_DataAccess.DeleteResponsiblePersonContactTypeAssignment(selectedContactTypeData.ID) Then
            m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Versandart konnte nicht gelöscht werden."))
          End If

          LoadAssignedContactTypeData(m_CustomerNumber, m_RecordNumber)

        End If

      End If
    End Sub

    ''' <summary>
    ''' Handles keydown event on reserve list.
    ''' </summary>
    Private Sub OnLstReserve_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles lstReserve1.KeyDown,
      lstReserve2.KeyDown, lstReserve3.KeyDown, lstReserve4.KeyDown

      If (Not IsResponsiblePersonDataLoaded) Then
        Return
      End If

      Dim reserveType As ResponsiblePersonReserveDataType? = Nothing

      If Object.ReferenceEquals(sender, lstReserve1) Then
        reserveType = ResponsiblePersonReserveDataType.Reserve1
      ElseIf Object.ReferenceEquals(sender, lstReserve2) Then
        reserveType = ResponsiblePersonReserveDataType.Reserve2
      ElseIf Object.ReferenceEquals(sender, lstReserve3) Then
        reserveType = ResponsiblePersonReserveDataType.Reserve3
      ElseIf Object.ReferenceEquals(sender, lstReserve4) Then
        reserveType = ResponsiblePersonReserveDataType.Reserve4
      End If

      If (Not reserveType Is Nothing AndAlso e.KeyCode = Keys.Delete) Then

        Dim listBox = CType(sender, ListBoxControl)

        Dim selectedReserveData As ResponsiblePersonAssignedReserveData = TryCast(listBox.SelectedItem, ResponsiblePersonAssignedReserveData)

        If (Not selectedReserveData Is Nothing) Then

          If Not m_DataAccess.DeleteResponsiblePersonReserveAssignment(selectedReserveData.ID, reserveType) Then
            m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Reservedaten konnte nicht gelöscht werden."))
          End If

          LoadAssignedReserveData(m_CustomerNumber, m_RecordNumber, reserveType)

        End If

      End If
    End Sub

#End Region

  End Class
End Namespace

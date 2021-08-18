Imports System.Reflection.Assembly
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.ucListSelectPopup
Imports SP.MA.AdvancePaymentMng.Settings
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.DatabaseAccess.AdvancePaymentMng.DataObjects

Imports SPS.Listing.Print.Utility

Imports SP.TodoMng

Namespace UI

  ''' <summary>
  ''' Advance payments management.
  ''' </summary>
  Public Class frmAdvancePayments

#Region "Private Consts"
    Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

    Private Const POPUP_DEFAULT_WIDTH As Integer = 420
    Private Const POPUP_DEFAULT_HEIGHT As Integer = 325

    Private Const LANR_CHECK As Integer = 8900
    Private Const LANR_BANK_TRANSFER As Integer = 8920
    Private Const LANR_BAR As Integer = 8930

#End Region

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
    ''' The advancepayment data access object.
    ''' </summary>
    Private m_AdvancePaymentDatabaseAccess As IAdvancePaymentDatabaseAccess

    ''' <summary>
    ''' The common database access.
    ''' </summary>
    Protected m_CommonDatabaseAccess As DatabaseAccess.Common.ICommonDatabaseAccess

    ''' <summary>
    ''' The data access object.
    ''' </summary>
    Private m_CustomerDatabaseAccess As DatabaseAccess.Customer.ICustomerDatabaseAccess

    ''' <summary>
    ''' The settings manager.
    ''' </summary>
    Private m_SettingsManager As ISettingsManager

    ''' <summary>
    ''' Contains the advance payment data.
    ''' </summary>
    Private m_ZGData As DataObjects.ZGMasterData

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
    ''' Boolean flag indicating if initial data has been loaded.
    ''' </summary>
    Private m_IsInitialDataLoaded As Boolean = False

    ''' <summary>
    ''' List of user controls.
    ''' </summary>
    Private m_ListOfUserControls As New List(Of ucBaseControl)

    ''' <summary>
    ''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
    ''' </summary>
    Private m_SuppressUIEvents As Boolean = False

    ''' <summary>
    ''' Communication support between controls.
    ''' </summary>
    Protected m_UCMediator As UserControlFormMediator

    ''' <summary>
    ''' The active botoom tab page.
    ''' </summary>
    Private m_ActiveBottomTabPage As ucBaseControl

    ''' <summary>
    ''' The common settings.
    ''' </summary>
    Private m_Common As CommonSetting

    ''' <summary>
    ''' The SPProgUtility object.
    ''' </summary>
    Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

    Private m_md As Mandant
    Private m_path As ClsProgPath

    Private Property m_PrintJobNr As String
    Private Property m_SQL4Print As String
    Private Property m_bPrintAsDesign As Boolean

    Private m_SaveButton As NavBarItem
    Private m_IsDataValid As Boolean = True

		''' <summary>
		''' Print.
		''' </summary>
		Private m_Print_P_Data As NavBarItem

    ''' <summary>
    ''' Boolan flag indicating if the form has been initialized.
    ''' </summary>
    Private m_IsInitialized = False

    Dim m_Guthaben_NavbarItem As NavBarItem
    Dim m_ChangeCstomerName_NavbarItem As NavBarItem
    Dim m_ChangeMahnstopDate_NavbarItem As NavBarItem
		Dim m_AllowedDesign As Boolean


#End Region


#Region "Public Properties"

    ''' <summary>
    ''' Boolean flag indicating if advance payment data is loaded.
    ''' </summary>
    Public ReadOnly Property IsAdvancePaymentDataLoaded As Boolean
      Get
        Return m_ZGData IsNot Nothing
      End Get

    End Property

    ''' <summary>
    ''' Gets or sets data valid flag.
    ''' </summary>
    ''' <returns>Data valid flag</returns>
    Public Property IsDataValid As Boolean
      Get
        Return m_IsDataValid
      End Get
      Set(value As Boolean)

        m_IsDataValid = value

        If Not m_IsDataValid AndAlso Not m_SaveButton Is Nothing Then
          m_SaveButton.Enabled = False
        End If

      End Set
    End Property

    ''' <summary>
    ''' Gets the ZG data.
    ''' </summary>
    ''' <returns>The ZG data.</returns>
    Public ReadOnly Property ZGData As ZGMasterData
      Get
        Return m_ZGData
      End Get

    End Property

    ''' <summary>
    ''' Gets boolean flag indicating if the data is readonly.
    ''' </summary>
    Public ReadOnly Property IsDataReadonly
      Get

        If ZGData Is Nothing Then
          Return True
        End If

        Dim isReadonly = ZGData.IsMonthClosed OrElse
          (ZGData.VGNR.HasValue AndAlso ZGData.VGNR > 0) OrElse
          (ZGData.RPNR.HasValue AndAlso ZGData.RPNR > 0) OrElse
          (ZGData.LONR.HasValue AndAlso ZGData.LONR > 0)

        Return isReadonly

      End Get
    End Property
#End Region


#Region "Private Properties"

#End Region

#Region "Constructor"

    Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
      Try
        ' Mandantendaten
        m_md = New Mandant
        m_path = New ClsProgPath
        m_Common = New CommonSetting

        m_InitializationData = _setting
        m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

      Catch ex As Exception
        m_Logger.LogError(ex.ToString)

      End Try

      ' Dieser Aufruf ist für den Designer erforderlich.
      DevExpress.UserSkins.BonusSkins.Register()
      DevExpress.Skins.SkinManager.EnableFormSkins()

      m_SuppressUIEvents = True
      InitializeComponent()
      m_SuppressUIEvents = False

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

      m_ListOfUserControls.Add(ucMainContent)
      m_ListOfUserControls.Add(ucNegativeSalaryData)
      m_ListOfUserControls.Add(ucBankData)
      m_ListOfUserControls.Add(ucAdvancePaymentslist)

      ' Init sub controls with configuration information
      For Each ctrl In m_ListOfUserControls
        ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
      Next

      m_UCMediator = New UserControlFormMediator(Me,
                                                 ucMainContent,
                                                 ucNegativeSalaryData,
                                                 ucBankData,
                                                 ucAdvancePaymentslist)

      m_ActiveBottomTabPage = ucNegativeSalaryData

      Dim connectionString As String = m_InitializationData.MDData.MDDbConn
      m_AdvancePaymentDatabaseAccess = New DatabaseAccess.AdvancePaymentMng.AdvancePaymentDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
      m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
      m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			m_AllowedDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 355, m_InitializationData.MDData.MDNr)

      m_SettingsManager = New SettingsManager
      m_UtilityUI = New UtilityUI
      m_Utility = New Utility

      ' Translate controls.
      TranslateControls()

      Reset()

      CreateMyNavBar()

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Show the data of an advance payment.
    ''' </summary>
    ''' <param name="zgNumber">The zg number.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function LoadAdvancePaymentData(ByVal zgNumber As Integer) As Boolean

      If Not m_SaveButton Is Nothing Then
        m_SaveButton.Enabled = True
      End If

      m_SuppressUIEvents = True

      Dim success As Boolean = True

      ' Load data
      m_ZGData = m_AdvancePaymentDatabaseAccess.LoadZGMasterData(zgNumber)

      If (m_ZGData Is Nothing) Then
        Return False
      End If

      success = success AndAlso ucMainContent.LoadDataOfActiveAdvancePayment()
      success = success AndAlso m_ActiveBottomTabPage.Activate()

      For Each ctrl In m_ListOfUserControls
        ctrl.SetReadonlyStateOfControls(IsDataReadonly)
      Next

      PrepareStatusAndNavigationBar(m_ZGData)

      success = success And m_ZGData IsNot Nothing

      IsDataValid = success

      m_SuppressUIEvents = False

      Return success
    End Function

    ''' <summary>
    ''' Shows new advancepayment form.
    ''' </summary>
    Public Sub NewAdvancePayment()

      Dim frmNewAdvancePayment As SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment = New SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment(m_InitializationData, Nothing)
      frmNewAdvancePayment.Show()
      frmNewAdvancePayment.BringToFront()

    End Sub

    ''' <summary>
    ''' Saves advance payment data.
    ''' </summary>
    Public Sub SaveAdvancePaymentData()

      If (ValidateData()) Then

        Dim success As Boolean = False

        If IsAdvancePaymentDataLoaded Then

          ' Reload data 
          Dim zgFromDB = m_AdvancePaymentDatabaseAccess.LoadZGMasterData(m_ZGData.ZGNr)

          ' 2. Ask all controls to merge its data with the records just loaded.
          For Each control In m_ListOfUserControls
            control.MergeCustomerMasterData(zgFromDB)
          Next

          zgFromDB.ChangedOn = DateTime.Now
          zgFromDB.ChangedFrom = m_InitializationData.UserData.UserFullName

          success = m_AdvancePaymentDatabaseAccess.UpdateZGData(zgFromDB)

          If success Then
            m_ZGData = zgFromDB
          End If

          ' 2. Ask all the controls to save additional data.
          For Each control In m_ListOfUserControls
            control.SaveAdditionalData()
          Next

        End If

        Dim message As String = String.Empty

        If (success) Then
          PrepareStatusAndNavigationBar(m_ZGData)

          DevExpress.XtraEditors.XtraMessageBox.Show((m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")),
                                                                                               m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
          m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
        End If

      Else
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten können nicht gespeichert werden."))
      End If
    End Sub

    ''' <summary>
    ''' Deletes the advance payment data.
    ''' </summary>
    Public Sub DeleteAdvancePayment()

      If Not IsAdvancePaymentDataLoaded Then
        Return
      End If

      ' Check month closed
      If ZGData.IsMonthClosed Then
        m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Der Vorschuss kann nicht gelöscht werden, da der Monat bereits abgeschlossen wurde."))
        Return
      End If

      ' Check permission 351 (permission to delete ZG)
			If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 351, m_InitializationData.MDData.MDNr) Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Sie verfügen nicht über die notwendige Berechtigung."))
				Return
			End If

      If Not m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Sind Sie sicher, dass Sie den Vorschuss löschen möchten?"),
                                         m_Translate.GetSafeTranslationValue("Löschen eines Vorschusses")) Then
        Return
      End If

      ' Check for existing LO
      If ZGData.LONR > 0 Then
        m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der Vorschuss kann nicht gelöscht werden.") & vbCrLf &
                                m_Translate.GetSafeTranslationValue("Bitte löschen Sie die Lohnabrechnung."), m_Translate.GetSafeTranslationValue("Vorschuss löschen"), MessageBoxIcon.Warning)
        Return
      End If

      Dim isMoneyTransferedViaCheckOrBar = (ZGData.LANR.HasValue AndAlso (ZGData.LANR = LANR_CHECK Or ZGData.LANR = LANR_BAR) AndAlso ZGData.Printed_Dat.HasValue)
      Dim isMoneyTransferedViaBankTransfer = (ZGData.LANR.HasValue AndAlso (ZGData.LANR = LANR_BANK_TRANSFER) AndAlso ZGData.VGNR > 0)

      ' Check if money is already transfered via check, bar or bank transfer.
      If isMoneyTransferedViaCheckOrBar Or
         isMoneyTransferedViaBankTransfer Then

        ' Check permission 352 (permission to delete ZG if money is already transfered)
				If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 352, m_InitializationData.MDData.MDNr) Then

					If Not m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Die Auszahlung wurde bereits überwiesen / ausgedruckt.") & vbCrLf &
																						 m_Translate.GetSafeTranslationValue("Möchten Sie trozdem löschen?"), m_Translate.GetSafeTranslationValue("Löschen eines Vorschusses")) Then
						Return
					End If

				Else
					' No permission -> exit
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Auszahlung wurde bereits überwiesen / ausgedruckt."), m_Translate.GetSafeTranslationValue("Vorschuss löschen"), MessageBoxIcon.Warning)
					Return
				End If

      End If

      ' Delete
			'Dim result = True	' m_AdvancePaymentDatabaseAccess.DeleteZGData(ZGData.ID, ConstantValues.ModulName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)
			Dim result = m_AdvancePaymentDatabaseAccess.DeleteZGData(ZGData.ID, "ZG", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)

      Select Case result
        Case DeleteZGResult.ResultCanNotDeleteBecauseOfLO
          m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Der Vorschuss konnte nicht gelöscht werden, da er bereits mit einer Lohnabrechnung verbunden ist."))
        Case DeleteZGResult.ResultDeleteError
          m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gelöscht werden."))

        Case DeleteZGResult.ResultDeleteOk
          CleanupAndHideForm()

      End Select
    End Sub

		Private Function PrintAdvancePaymentData() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim ShowDesign As Boolean = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)

			Dim strResult As String = "success..."

			Try
				'0: Check
				'1: Quittung
				'2: Überweisung
				If (ZGData.LANR = LANR_CHECK Or ZGData.LANR = LANR_BAR) AndAlso ZGData.Printed_Dat.HasValue Then
					Dim msg As String = String.Format("Advancepayment: {0} was allready printed!!!", ZGData.ZGNr)
					m_Logger.LogWarning(msg)
					Return msg
				End If

				Dim docart As Integer = If(ZGData.LANR = LANR_CHECK, 0, If(ZGData.LANR = LANR_BANK_TRANSFER, 2, 1))

				Dim _settring As New ClsLLAdvancePaymentPrintSetting With {.ZGNr = ZGData.ZGNr,
																																	 .DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																	 .LogedUSNr = m_InitializationData.UserData.UserNr,
																																	 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																																	 .docart = docart,
																																	 .frmhwnd = Me.Handle,
																																	 .PerosonalizedData = m_InitializationData.ProsonalizedData,
																																	 .TranslationItems = m_InitializationData.TranslationData,
																																	 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", "")}),
																																	 .ShowAsDesign = ShowDesign}

				Dim obj As New AdvancePaymentData.ClsPrintAdvancePaymentData(_settring)
				strResult = obj.PrintAdvancePaymentDocument()

				If Not strResult.ToLower.Contains("error") Then m_ZGData = m_AdvancePaymentDatabaseAccess.LoadZGMasterData(ZGData.ZGNr)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:ZGNr: {1}:{2}", strMethodeName, ZGData.ZGNr, ex.Message))
				strResult = "Error: " & String.Format("{0}:ZGNr: {1}:{2}", strMethodeName, ZGData.ZGNr, ex.Message)

			End Try

			Return strResult

		End Function

    ''' <summary>
    ''' Shows a todo form.
    ''' </summary>
    Private Sub ShowTodo()
      Dim frmTodo As New frmTodo(m_InitializationData)
      ' optional init new todo

      If Not IsAdvancePaymentDataLoaded Then
        Return
      End If

      Dim UserNumber As Integer = m_InitializationData.UserData.UserNr
      Dim EmployeeNumber As Integer? = m_ZGData.MANR
      Dim CustomerNumber As Integer? = Nothing
      Dim ResponsiblePersonRecordNumber As Integer? = Nothing
      Dim VacancyNumber As Integer? = Nothing
      Dim ProposeNumber As Integer? = Nothing
      Dim ESNumber As Integer? = Nothing
			Dim RPNumber As Integer? = m_ZGData.RPNR
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

#Region "Private Methods"

#Region "Reset Form"

    ''' <summary>
    ''' Resets the from.
    ''' </summary>
    Private Sub Reset()

      Dim previousState = SetSuppressUIEventsState(True)

      m_ZGData = Nothing

      ' Reset all the child controls
      For Each ctrl In m_ListOfUserControls
        ctrl.Reset()
      Next

      bottomTabs.SelectedTabPageIndex = 0

      '  Reset grids, drop downs and lists, etc.

      SetSuppressUIEventsState(previousState)

    End Sub

#End Region

#Region "Load master Data"

    ''' <summary>
    ''' Loads the DropDown Data
    ''' </summary>
    Private Sub LoadDropDownData()

    End Sub

#End Region

#Region "Load, save advance payment Data"

    ''' <summary>
    ''' Validates the data on the form.
    ''' </summary>
    Private Function ValidateData() As Boolean

      Dim valid As Boolean = True
      For Each userControl In m_ListOfUserControls
        valid = valid AndAlso userControl.ValidateData()
      Next

      Return valid

    End Function

#End Region

#Region "Event Handles"

    ''' <summary>
    ''' Handles tab control selection changing
    ''' </summary>
    Private Sub OnxtraTabControl_SelectedPageChanging(sender As System.Object, e As DevExpress.XtraTab.TabPageChangingEventArgs) Handles bottomTabs.SelectedPageChanging

      If m_SuppressUIEvents Then
        Return
      End If

      Dim page = e.Page

      If Not (m_ActiveBottomTabPage Is Nothing) Then
        m_ActiveBottomTabPage.Deactivate()
      End If

      If (Object.ReferenceEquals(page, xtabNegativeSalary)) Then
        ucNegativeSalaryData.Activate()
        m_ActiveBottomTabPage = ucNegativeSalaryData
      ElseIf (Object.ReferenceEquals(page, xtabBank)) Then
        ucBankData.Activate()
        m_ActiveBottomTabPage = ucBankData
      ElseIf (Object.ReferenceEquals(page, xtabAdvancePaymentList)) Then
        ucAdvancePaymentslist.Activate()
        m_ActiveBottomTabPage = ucAdvancePaymentslist
      End If

    End Sub

    ''' <summary>
    ''' Handles form load event.
    ''' </summary>
    Private Sub OnFrmAdvancePayment_Load(sender As Object, e As System.EventArgs) Handles Me.Load

      Me.KeyPreview = True
      Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_md.GetDefaultUSNr, String.Empty)
      If strStyleName <> String.Empty Then
        UserLookAndFeel.Default.SetSkinStyle(strStyleName)
      End If
    End Sub

    ''' <summary>
    ''' Loads form settings if form gets visible.
    ''' </summary>
    Private Sub OnFrmAdvancePayment_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

      If Visible Then
        LoadFormSettings()
      End If

    End Sub

    ''' <summary>
    ''' Handles form closing event.
    ''' </summary>
    Private Sub OnFrmAdvancePayment_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

      CleanupAndHideForm()
      e.Cancel = True

    End Sub

    ''' <summary>
    ''' Keypreview for Modul-version
    ''' </summary>
    Private Sub OnForm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
      If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
        Dim strRAssembly As String = ""
        Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
        For Each a In AppDomain.CurrentDomain.GetAssemblies()
          strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
        Next
        strMsg = String.Format(strMsg, vbNewLine, _
                                                     GetExecutingAssembly().FullName, _
                                                     GetExecutingAssembly().Location, _
                                                     strRAssembly)
        DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
      End If
    End Sub

    ''' <summary>
    ''' Clickevent for Navbar.
    ''' </summary>
    Private Sub OnnbMain_LinkClicked(ByVal sender As Object, _
                                                             ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim bForDesign As Boolean = False
      Try
        Dim strLinkName As String = e.Link.ItemName
        Dim strLinkCaption As String = e.Link.Caption

        For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
          e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
        Next
        e.Link.Item.Appearance.ForeColor = Color.Orange

        Select Case strLinkName.ToLower
          Case "New_AdvancePayment".ToLower
            NewAdvancePayment()
          Case "Save_AdvancePayment_Data".ToLower
            SaveAdvancePaymentData()
					Case "Print_AdvancePayment_Data".ToLower
						PrintAdvancePaymentData()

					Case "Delete_AdvancePayment_Data".ToLower
						DeleteAdvancePayment()
          Case "Close_AdvancePayment_Form".ToLower
            CleanupAndHideForm()
          Case "CreateTODO".ToLower
            ShowTodo()
          Case Else
            ' Do nothing
        End Select

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        m_UtilityUI.ShowErrorDialog(ex.Message)

      Finally

      End Try

    End Sub

    ''' <summary>
    ''' Handles drop down button clicks.
    ''' </summary>
    Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

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

#End Region

#Region "Helper Methods"

    ''' <summary>
    '''  Trannslate controls.
    ''' </summary>
    Private Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.xtabNegativeSalary.Text = m_Translate.GetSafeTranslationValue(Me.xtabNegativeSalary.Text)
      Me.xtabBank.Text = m_Translate.GetSafeTranslationValue(Me.xtabBank.Text)
      Me.xtabAdvancePaymentList.Text = m_Translate.GetSafeTranslationValue(Me.xtabAdvancePaymentList.Text)

			bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
			bsiLblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsiLblGeaendert.Caption)
			bsiLblGedruckt.Caption = m_Translate.GetSafeTranslationValue(bsiLblGedruckt.Caption)

    End Sub

    ''' <summary>
    ''' Sets the suppress UI events state.
    ''' </summary>
    ''' <param name="shouldEventsBeSuppressed">Boolean flag indicating the  UI events should be suppressed.</param>
    ''' <returns>Previous state of suppress events.</returns>
    Public Function SetSuppressUIEventsState(ByVal shouldEventsBeSuppressed As Boolean)

      Dim orginalState = m_SuppressUIEvents
      m_SuppressUIEvents = shouldEventsBeSuppressed

      Return orginalState

    End Function

    ''' <summary>
    ''' Creates Navigationbar
    ''' </summary>
    Private Sub CreateMyNavBar()
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

      Me.navMain.Items.Clear()
      Try
        navMain.PaintStyleName = "SkinExplorerBarView"

        ' Create a Local group.
				Dim groupDatei As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Datei"))
        groupDatei.Name = "gNavDatei"

        Dim New_P As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neu"))
        New_P.Name = "New_AdvancePayment"
        New_P.SmallImage = Me.ImageCollection1.Images(0)
				New_P.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 353, m_InitializationData.MDData.MDNr)

        m_SaveButton = New NavBarItem(m_Translate.GetSafeTranslationValue("Daten sichern"))
        m_SaveButton.Name = "Save_AdvancePayment_Data"
        m_SaveButton.SmallImage = Me.ImageCollection1.Images(1)
        m_SaveButton.Enabled = IsDataValid

				m_Print_P_Data = New NavBarItem(m_Translate.GetSafeTranslationValue("Drucken"))
				m_Print_P_Data.Name = "Print_AdvancePayment_Data"
				m_Print_P_Data.SmallImage = Me.ImageCollection1.Images(2)
				m_Print_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 354, m_InitializationData.MDData.MDNr)

        Dim Close_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Schliessen"))
        Close_P_Data.Name = "Close_AdvancePayment_Form"
        Close_P_Data.SmallImage = Me.ImageCollection1.Images(3)

        Dim groupDelete As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Löschen"))
        groupDelete.Name = "gNavDelete"
        groupDelete.Appearance.ForeColor = Color.Red

        Dim Delete_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Löschen"))
        Delete_P_Data.Name = "Delete_AdvancePayment_Data"
        Delete_P_Data.SmallImage = Me.ImageCollection1.Images(4)
        Delete_P_Data.Appearance.ForeColor = Color.Red
				Delete_P_Data.Enabled = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 351, m_InitializationData.MDData.MDNr) Or IsUserActionAllowed(m_InitializationData.UserData.UserNr, 352, m_InitializationData.MDData.MDNr)

        Dim groupExtra As NavBarGroup = New NavBarGroup(m_Translate.GetSafeTranslationValue("Extras"))
        groupExtra.Name = "gNavExtra"

				Dim TODO_P_Data As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("To-do erstellen"))
        TODO_P_Data.Name = "CreateTODO"
        TODO_P_Data.SmallImage = Me.ImageCollection1.Images(9)

        Try
          navMain.BeginUpdate()

          navMain.Groups.Add(groupDatei)
					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 353, m_InitializationData.MDData.MDNr) Then
						groupDatei.ItemLinks.Add(New_P)
						groupDatei.ItemLinks.Add(m_SaveButton)
					End If

					If IsUserActionAllowed(m_InitializationData.UserData.UserNr, 354, m_InitializationData.MDData.MDNr) Then groupDatei.ItemLinks.Add(m_Print_P_Data)
          groupDatei.ItemLinks.Add(Close_P_Data)
          groupDatei.Expanded = True

          navMain.Groups.Add(groupDelete)
          groupDelete.ItemLinks.Add(Delete_P_Data)
          groupDelete.Expanded = False

					navMain.Groups.Add(groupExtra)
					groupExtra.ItemLinks.Add(TODO_P_Data)
          'groupExtra.ItemLinks.Add(Property_P_Data)

					'navMain.Groups.Add(groupMoreModule)
					'groupMoreModule.ItemLinks.Add(TODO_P_Data)

          groupExtra.Expanded = True

          If IsAdvancePaymentDataLoaded Then
						bsiCreated.Caption = String.Format(" {0:f}, {1}", m_ZGData.CreatedOn, m_ZGData.CreatedFrom)
						bsiChanged.Caption = String.Format(" {0:f}, {1}", m_ZGData.ChangedOn, m_ZGData.ChangedFrom)
						bsiPrinted.Caption = String.Format(" {0:f}", m_ZGData.Printed_Dat)
					End If

          navMain.EndUpdate()

        Catch ex As Exception
          m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
          DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message),
                                                                                             "Menüleiste", MessageBoxButtons.OK, MessageBoxIcon.Error)

      End Try

    End Sub

    ''' <summary>
    ''' Prepares status and navigation bar.
    ''' </summary>
    ''' <param name="zgData">The zgData object.</param>
    Private Sub PrepareStatusAndNavigationBar(ByVal zgData As ZGMasterData)

      If zgData Is Nothing Then
        bsiCreated.Caption = String.Empty
        bsiChanged.Caption = String.Empty
				bsiPrinted.Caption = String.Empty

        Return
			End If
			m_Print_P_Data.Enabled = Not ((zgData.LANR = LANR_CHECK Or zgData.LANR = LANR_BAR) And zgData.Printed_Dat.HasValue)

      bsiCreated.Caption = String.Format(" {0:f}, {1}", zgData.CreatedOn, zgData.CreatedFrom)
      bsiChanged.Caption = String.Format(" {0:f}, {1}", zgData.ChangedOn, zgData.ChangedFrom)
			bsiPrinted.Caption = String.Format(" {0:f}", zgData.Printed_Dat)

		End Sub


    ''' <summary>
    ''' Loads form settings.
    ''' </summary>
    Private Sub LoadFormSettings()

      Try
        Dim setting_form_height = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_HEIGHT)
        Dim setting_form_width = m_SettingsManager.ReadInteger(SettingKeys.SETTING_FORM_WIDTH)
        Dim setting_form_location = m_SettingsManager.ReadString(SettingKeys.SETTING_FORM_LOCATION)
        Dim setting_form_sccMainPos = m_SettingsManager.ReadInteger(SettingKeys.SETTING_SCC_MAINPOS)

        If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
        If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

        If Not String.IsNullOrEmpty(setting_form_location) Then
          Dim aLoc As String() = setting_form_location.Split(CChar(";"))
          If Screen.AllScreens.Length = 1 Then
            If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
          End If
          Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
        End If
        If setting_form_sccMainPos > 0 Then Me.sccAdvancePaymentMain.SplitterPosition = Math.Max(setting_form_sccMainPos, 10)


      Catch ex As Exception
        m_Logger.LogError(ex.ToString())

      End Try

    End Sub

    ''' <summary>
    ''' Saves the form settings.
    ''' </summary>
    Private Sub SaveFromSettings()

      ' Save form location, width and height in setttings
      Try
        If Not Me.WindowState = FormWindowState.Minimized Then
          m_SettingsManager.WriteString(SettingKeys.SETTING_FORM_LOCATION, String.Format("{0};{1}", Me.Location.X, Me.Location.Y))
          m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_WIDTH, Me.Width)
          m_SettingsManager.WriteInteger(SettingKeys.SETTING_FORM_HEIGHT, Me.Height)

          m_SettingsManager.WriteInteger(SettingKeys.SETTING_SCC_MAINPOS, Me.sccAdvancePaymentMain.SplitterPosition)

          m_SettingsManager.SaveSettings()
        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())

      End Try

    End Sub


    ''' <summary>
    ''' Sets the valid state of a control.
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
    ''' Cleanup and close form.
    ''' </summary>
    Public Sub CleanupAndHideForm()

      SaveFromSettings()

      Me.Hide()
      Me.Reset() 'Clear all data.

    End Sub

#End Region

#End Region


#Region "Helpers"


		''' <summary>
		''' Context menu data.
		''' </summary>
		Private Class ContextMenuForPrint
			Public Property MnuName As String
			Public Property MnuCaption As String
		End Class


#End Region


  End Class

End Namespace

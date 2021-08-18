
Imports System.ComponentModel
Imports System.Reflection
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.DesignerListBox
Imports DevExpress.XtraEditors.DXErrorProvider
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.UI
Imports SP.MA.KontaktMng.frmContacts
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports Traysoft.AddTapi


Namespace UI


	Public Class frmCaller
		Inherits DevExpress.XtraEditors.XtraForm

		Delegate Sub AddListItem(ByVal item As String)
		Private m_addToLogDelegate As AddListItem


#Region "private fields"

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
		''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
		''' </summary>
		Private m_SuppressUIEvents As Boolean = False

		Private m_CommonDatabaseAccess As ICommonDatabaseAccess
		Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess
		Private m_ESDatabaseAccess As IESDatabaseAccess

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_MandantData As Mandant
		Private m_utilitySP As Utilities

		Private m_TapiLines As IEnumerable(Of TapiLine)
		Private m_CurrentTapiLine As TapiLine
		Private m_CurrentCall As TapiCall

		Private m_TelephonyData As IEnumerable(Of CommonTelephonyData)
		Private m_CurrentEmployeeNumber As Integer?
		Private m_CurrentCustomerNumber As Integer?
		Private m_CurrentCResponsibleNumber As Integer?
		Private m_CurrentModulTyp As TelephonyRecordSource

		Private m_OfficeCode As Integer?
		Private m_ReplacePlusInToZero As Boolean?
		Private m_CreateAutoContact As Boolean?
		Private m_IsLineConected As Boolean


#End Region


#Region "constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_utilitySP = New Utilities
			m_UtilityUI = New UtilityUI


			InitializeComponent()

			Me.KeyPreview = True
			Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			' Subscribe to events from AddTapi
			AddHandler TapiApp.TapiError, AddressOf OnTapiError
			AddHandler TapiApp.LineAdded, AddressOf OnLineAdded
			AddHandler TapiApp.LineClosed, AddressOf OnLineClosed
			AddHandler TapiApp.LineRemoved, AddressOf OnLineRemoved
			AddHandler TapiApp.IncomingCall, AddressOf OnIncomingCall
			AddHandler TapiApp.OutgoingCall, AddressOf OnOutgoingCall
			AddHandler TapiApp.CallConnected, AddressOf OnCallConnected
			AddHandler TapiApp.CallDisconnected, AddressOf OnCallDisconnected

			Dim m_connectionString = m_InitializationData.MDData.MDDbConn
			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
			m_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

			TranslateControls()
			Reset()


			LoadTapiLines()
			If Not My.Settings.lineData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(My.Settings.lineData) Then
				lueTapiLines.EditValue = My.Settings.lineData
			Else
				Dim msg = m_Translate.GetSafeTranslationValue("Keine Leitung wurde gewählt.")
				SetDXErrorIfInvalid(lueTapiLines, ErrorProvider, (lueTapiLines.EditValue Is Nothing), msg)
			End If

			AddHandler gvAvailableData.RowCellClick, AddressOf OngvAvailableData_RowCellClick
			AddHandler gvContact.RowCellClick, AddressOf OngvContactData_RowCellClick

			m_addToLogDelegate = New AddListItem(AddressOf AddToLog)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			m_TelephonyData = m_CustomerDatabaseAccess.LoadCommonPhoneNumberData(String.Empty)

			If m_TelephonyData Is Nothing Then
				m_Logger.LogError(String.Format("no telephone data is founded"))
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."), "Telefonnummer-Daten")

				Return
			End If

			grdAvailableData.DataSource = m_TelephonyData
			grdAvailableData.ForceInitialize()
			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}", m_TelephonyData.Count)

			If String.IsNullOrWhiteSpace(My.Settings.AmtsZiffer) Then m_OfficeCode = Nothing Else m_OfficeCode = Val(My.Settings.AmtsZiffer)
			m_ReplacePlusInToZero = My.Settings.replacepluswithzero
			m_CreateAutoContact = My.Settings.createautocontact

			m_SuppressUIEvents = suppressUIEventsState

		End Sub

#End Region

#Region "Public Methodes"

		Public Sub LoadData(ByVal phoneNumber As String)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			phoneNumber = phoneNumber.Replace(" ", "")
			'm_TelephonyData = m_CustomerDatabaseAccess.LoadCommonPhoneNumberData(phoneNumber)

			'If m_TelephonyData Is Nothing Then
			'	m_Logger.LogError(String.Format("no telephone data is founded: {0}", phoneNumber))
			'	m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."), "Telefonnummer-Daten")

			'	Return
			'End If

			cbo_MyNumber.EditValue = phoneNumber
			'grdAvailableData.DataSource = m_TelephonyData
			'grdAvailableData.ForceInitialize()

			m_SuppressUIEvents = suppressUIEventsState
			bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), m_TelephonyData.Count)

			FocusAbailableData(phoneNumber)


		End Sub


#End Region


#Region "private properties"

		Private ReadOnly Property SelectedAvailableRowViewData As CommonTelephonyData
			Get
				Dim grdView = TryCast(grdAvailableData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(grdView.GetRow(selectedRows(0)), CommonTelephonyData)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

		Private ReadOnly Property SelectedContactRowViewData As ContactViewData
			Get
				Dim grdView = TryCast(grdContact.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(grdView.GetRow(selectedRows(0)), ContactViewData)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region


		Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			lblAvailableLines.Text = m_Translate.GetSafeTranslationValue(lblAvailableLines.Text)
			grpContact.Text = m_Translate.GetSafeTranslationValue(grpContact.Text)
			txtContactData.Properties.NullValuePrompt = m_Translate.GetSafeTranslationValue(txtContactData.Properties.NullValuePrompt)
			txtContactTitle.Properties.NullValuePrompt = m_Translate.GetSafeTranslationValue(txtContactTitle.Properties.NullValuePrompt)

			btnDial.Text = m_Translate.GetSafeTranslationValue(btnDial.Text)

			bbiSetting.Caption = m_Translate.GetSafeTranslationValue(bbiSetting.Caption)

		End Sub


#Region "reset"

		Private Sub Reset()

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			cbo_MyNumber.EditValue = String.Empty
			btnDial.ImageOptions.Image = My.Resources.phone3_16x16
			m_IsLineConected = False

			ResetLineDropDown()
			ResetAvailableGrid()
			txtContactTitle.Properties.NullValuePrompt = m_Translate.GetSafeTranslationValue("Kontakt Bezeichnung")
			txtContactData.Properties.NullValuePrompt = m_Translate.GetSafeTranslationValue("Kontakt Beschreibung")

			m_SuppressUIEvents = False

		End Sub

		Private Sub ResetLineDropDown()

			lueTapiLines.Properties.DisplayMember = "Name"
			lueTapiLines.Properties.ValueMember = "Name"

			Dim columns = lueTapiLines.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Name", 0, m_Translate.GetSafeTranslationValue("Name")))

			lueTapiLines.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueTapiLines.Properties.SearchMode = SearchMode.AutoComplete
			lueTapiLines.Properties.AutoSearchColumnIndex = 0

			lueTapiLines.Properties.NullText = String.Empty
			lueTapiLines.EditValue = Nothing

		End Sub

		Private Sub ResetAvailableGrid()

			' Create Columns
			gvAvailableData.OptionsView.ShowIndicator = False
			gvAvailableData.OptionsView.ShowAutoFilterRow = True
			gvAvailableData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			gvAvailableData.Appearance.SelectedRow.BackColor = Color.Orange
			gvAvailableData.Appearance.FocusedRow.BackColor = Color.Orange


			gvAvailableData.Columns.Clear()

			Dim columnFilename As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFilename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFilename.Caption = m_Translate.GetSafeTranslationValue("ModulSource")
			columnFilename.Name = "ModulSource"
			columnFilename.FieldName = "ModulSource"
			columnFilename.BestFit()
			columnFilename.Visible = False
			columnFilename.Width = 100
			gvAvailableData.Columns.Add(columnFilename)

			Dim columnFilelocation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFilelocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFilelocation.Caption = m_Translate.GetSafeTranslationValue("ZNumber")
			columnFilelocation.Name = "ZNumber"
			columnFilelocation.FieldName = "ZNumber"
			columnFilelocation.Visible = False
			columnFilelocation.Width = 300
			gvAvailableData.Columns.Add(columnFilelocation)

			Dim columnFileCreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFileCreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFileCreatedon.Caption = m_Translate.GetSafeTranslationValue("RecNumber")
			columnFileCreatedon.Name = "RecNumber"
			columnFileCreatedon.FieldName = "RecNumber"
			columnFileCreatedon.Visible = False
			columnFileCreatedon.Width = 30
			gvAvailableData.Columns.Add(columnFileCreatedon)

			Dim columnFileVersion As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFileVersion.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFileVersion.Caption = m_Translate.GetSafeTranslationValue("Name")
			columnFileVersion.Name = "FullName"
			columnFileVersion.FieldName = "FullName"
			columnFileVersion.Visible = True
			columnFileVersion.Width = 20
			gvAvailableData.Columns.Add(columnFileVersion)

			Dim columnCompany As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCompany.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCompany.Caption = m_Translate.GetSafeTranslationValue("Firma")
			columnCompany.Name = "Company"
			columnCompany.FieldName = "Company"
			columnCompany.Visible = True
			columnCompany.Width = 20
			gvAvailableData.Columns.Add(columnCompany)

			Dim columnTelephon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnTelephon.Caption = m_Translate.GetSafeTranslationValue("Telefon")
			columnTelephon.Name = "Telephon"
			columnTelephon.FieldName = "Telephon"
			columnTelephon.Visible = True
			columnTelephon.Width = 20
			gvAvailableData.Columns.Add(columnTelephon)


			grdAvailableData.DataSource = Nothing

		End Sub

		Sub ResetEmployeeContactDetailGrid()

			gvContact.OptionsView.ShowGroupPanel = False
			gvContact.OptionsView.ShowIndicator = False
			gvContact.OptionsView.ShowAutoFilterRow = True

			gvContact.Columns.Clear()

			Try

				Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
				columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
				columnDate.Name = "ContactDate"
				columnDate.FieldName = "ContactDate"
				columnDate.Visible = True
				columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
				gvContact.Columns.Add(columnDate)

				Dim personSubjectColumn As New DevExpress.XtraGrid.Columns.GridColumn()
				personSubjectColumn.Caption = m_Translate.GetSafeTranslationValue("Person / Betreff")
				personSubjectColumn.Name = "Person_Subject"
				personSubjectColumn.FieldName = "Person_Subject"
				personSubjectColumn.Visible = True
				gvContact.Columns.Add(personSubjectColumn)

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
				gvContact.Columns.Add(docType)

				grdContact.AllowDrop = True


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			grdContact.DataSource = Nothing

		End Sub

		Private Sub ResetCustomerContactDetailGrid()

			gvContact.OptionsView.ShowGroupPanel = False
			gvContact.OptionsView.ShowIndicator = False
			gvContact.OptionsView.ShowAutoFilterRow = True

			gvContact.Columns.Clear()

			Dim columnDate As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDate.Caption = m_Translate.GetSafeTranslationValue("Datum")
			columnDate.Name = "ContactDate"
			columnDate.FieldName = "ContactDate"
			columnDate.Visible = True
			columnDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
			gvContact.Columns.Add(columnDate)

			Dim personSubjectColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			personSubjectColumn.Caption = m_Translate.GetSafeTranslationValue("Person / Betreff")
			personSubjectColumn.Name = "Person_Subject"
			personSubjectColumn.FieldName = "Person_Subject"
			personSubjectColumn.Visible = True
			gvContact.Columns.Add(personSubjectColumn)

			Dim descriptionColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			descriptionColumn.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
			descriptionColumn.Name = "Description"
			descriptionColumn.FieldName = "Description"
			descriptionColumn.Visible = True
			descriptionColumn.Width = 200
			gvContact.Columns.Add(descriptionColumn)

			Dim kstColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			kstColumn.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
			kstColumn.Name = "Creator"
			kstColumn.FieldName = "Creator"
			kstColumn.Visible = True
			gvContact.Columns.Add(kstColumn)

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
			gvContact.Columns.Add(docType)


			grdContact.DataSource = Nothing

		End Sub


#End Region


#Region "notifyer"

		Sub PutFormInNotyfier()
			Dim mnu As New ContextMenu()
			Dim mnuitem As New MenuItem()

			Dim aAssembly As Assembly = System.Reflection.Assembly.GetExecutingAssembly
			Dim aDescAttr As AssemblyDescriptionAttribute = CType(AssemblyDescriptionAttribute.GetCustomAttribute(aAssembly, GetType(AssemblyDescriptionAttribute)), AssemblyDescriptionAttribute)
			Dim aTitleAttr As AssemblyTitleAttribute = CType(AssemblyTitleAttribute.GetCustomAttribute(aAssembly, GetType(AssemblyTitleAttribute)), AssemblyTitleAttribute)
			Dim aVersionAttr As AssemblyVersionAttribute = CType(AssemblyVersionAttribute.GetCustomAttribute(aAssembly, GetType(AssemblyVersionAttribute)), AssemblyVersionAttribute)

			mnu.MenuItems.Add("Öffnen", AddressOf frmShow)
			Dim strAssemlyInfo As String() = Split(aAssembly.FullName, ",")

			NotifyIcon1.Text = String.Format("{1}; {2}{0}{3}", vbNewLine, aTitleAttr.Title.ToString, strAssemlyInfo(1).ToString, getStrFromPtr(getVersion()))
			NotifyIcon1.ContextMenu = mnu
			NotifyIcon1.Icon = Me.Icon ' New Icon(Application.tartupPath & "\systray.ico")
			NotifyIcon1.Visible = True


		End Sub

		Sub CloseActiveForm()
			'If Not Me.pcc_Kontakt.Visible Then
			Me.NotifyIcon1.Dispose()
			Me.Close()
			Me.Dispose()
			'End If
		End Sub

		Private Sub frmShow(ByVal sender As Object, ByVal e As EventArgs)

			NotifyIcon1.Visible = False
			Me.WindowState = FormWindowState.Normal
			Me.Show()

		End Sub

		Private Sub NotifyIcon1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseUp
			Me.frmShow(sender, New EventArgs)
		End Sub


#End Region

		Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

			Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
			Me.Width = Math.Max(My.Settings.iWidth, Me.Width)
			If My.Settings.frmLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

			FocusAbailableData(cbo_MyNumber.EditValue.ToString.Replace(" ", ""))

		End Sub

		Private Sub Form1_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iLeft = Me.Top
				My.Settings.iTop = Me.Left

				If Not m_CurrentTapiLine Is Nothing Then
					My.Settings.lineData = m_CurrentTapiLine.Name
					If m_CurrentTapiLine.IsOpen Then m_CurrentTapiLine.Close()
				End If

				My.Settings.Save()
			End If

		End Sub

		Private Sub lueTapiLines_EditValueChanged(sender As Object, e As EventArgs) Handles lueTapiLines.EditValueChanged

			ErrorProvider.ClearErrors()

			Dim SelectedData = m_TapiLines.Where(Function(x) x.Name = lueTapiLines.EditValue).FirstOrDefault()
			m_CurrentTapiLine = SelectedData

			If Not m_CurrentTapiLine Is Nothing Then OpenTapiLine()

		End Sub

		Private Function LoadTapiLines() As Boolean
			Dim listDataSource As New BindingList(Of TapiLine)

			For Each objLine As TapiLine In TapiApp.Lines
				listDataSource.Add(objLine)
			Next
			If listDataSource Is Nothing Then
				m_Logger.LogError(String.Format("no lines could be founded!"))

				Return False
			End If

			m_TapiLines = listDataSource
			lueTapiLines.Properties.DataSource = m_TapiLines

		End Function

		Private Sub OnbtnDial_Click(sender As Object, e As EventArgs) Handles btnDial.Click
			Dim number As String = cbo_MyNumber.EditValue

			ErrorProvider.ClearErrors()
			If m_IsLineConected Then
				m_CurrentCall.Disconnect()
				'CloseTapiLine()
				btnDial.ImageOptions.Image = My.Resources.phone3_16x16

				Return
			End If

			If m_CurrentTapiLine Is Nothing Then
				Dim msg = m_Translate.GetSafeTranslationValue("Keine Leitung wurde gewählt.")
				m_Logger.LogWarning("no line is choosen.")
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Keine gültige TAPI-Leitung wurde gewählt!"), m_Translate.GetSafeTranslationValue("Anrufen"), MessageBoxIcon.Asterisk)

				SetDXErrorIfInvalid(lueTapiLines, ErrorProvider, (lueTapiLines.EditValue Is Nothing OrElse m_CurrentTapiLine Is Nothing), msg)

				Return
			End If

			If String.IsNullOrWhiteSpace(number) Then
				m_Logger.LogWarning(String.Format("Keine gültige Nummer wurde eingetragen."))
				m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Keine gültige Nummer wurde eingetragen."), m_Translate.GetSafeTranslationValue("Anrufen"), MessageBoxIcon.Asterisk)

				Return
			Else
				If number.Length > 5 Then
					number = String.Format("{0}{1}", m_OfficeCode, number)
					If m_ReplacePlusInToZero.GetValueOrDefault(False) Then number = number.Replace("++", "00").Replace("+", "00")

				End If
			End If

			Try
				'' Check if this line supports voice functions (playing wav files and speech)
				'If Not m_CurrentTapiLine.SupportsVoice Then
				'	MessageBox.Show("This line does not support audio playback/recording and cannot be used to play wav files.\n" +
				'													"If this is a voice modem, make sure you have installed TAPI-compatible voice driver for the modem.\n\n" +
				'													"Outgoing call will be placed in interactive mode and terminated 30 seconds after the call is connected.")
				'End If

				' Open selected line for outgoing calls if it wasn't already open
				If Not m_CurrentTapiLine.IsOpen Then
					m_CurrentTapiLine.Open(False, AddressOf CallHandler)
				End If
				m_CurrentTapiLine.SpeechVoice = Nothing 'comboBoxVoice.SelectedItem

				' Set no answer timeout to 30 seconds (approx. 5 rings)
				m_CurrentTapiLine.NoAnswerTimeout = 30

				' Dial entered number without the dialing rules
				m_CurrentCall = m_CurrentTapiLine.Dial(number, False)

				Dim msg = String.Format("Dialing {0} on line '{1}'", number, m_CurrentTapiLine.Name)
				AddToLog(msg)

			Catch exc As TapiException
				m_Logger.LogError(String.Format("TapiException: {0}", exc.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("TapiException: {0}", exc.ToString))

			Catch exc As Exception
				m_Logger.LogError(String.Format("Exception: {0}", exc.ToString))
				m_UtilityUI.ShowErrorDialog(String.Format("Exception: {0}", exc.ToString))

			End Try

		End Sub

		Private Sub cmdDrop_Click(sender As Object, e As EventArgs)
			CloseTapiLine()
		End Sub

		Private Sub btnRefreshData_Click(sender As Object, e As EventArgs) Handles btnRefreshData.Click

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			m_TelephonyData = m_CustomerDatabaseAccess.LoadCommonPhoneNumberData(String.Empty)

			If m_TelephonyData Is Nothing Then
				m_Logger.LogError(String.Format("no telephone data is founded"))
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."), "Telefonnummer-Daten")

				Return
			End If

			grdAvailableData.DataSource = m_TelephonyData
			grdAvailableData.ForceInitialize()

			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		Private Sub bbiSetting_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSetting.ItemClick
			Dim frm As New frmTelephonySetting(m_InitializationData)

			frm.ShowDialog()
			If String.IsNullOrWhiteSpace(My.Settings.AmtsZiffer) Then m_OfficeCode = Nothing Else m_OfficeCode = Val(My.Settings.AmtsZiffer)
			m_ReplacePlusInToZero = My.Settings.replacepluswithzero
			m_CreateAutoContact = My.Settings.createautocontact

		End Sub


#Region "Tapi event"

		Private Sub CallHandler(ByVal objCall As TapiCall)
			Try
				Dim msg = String.Format("Connected call to {0}.", objCall.CalledID)
				' Because it's another thread, we have to use Invoke to access UI controls
				listBoxLog.Invoke(m_addToLogDelegate, msg)

				' If the line doesn't support voice, wait 30 seconds then terminate the call
				If Not objCall.Line.SupportsVoice Then
					System.Threading.Thread.Sleep(30000)
					Return
				End If


				' TODO: is deactivated!
				'' Try to deliver a message, up to 5 times.
				'For retry As Integer = 1 To 5
				'	' Play a message:
				'	' "Hello! This is a message from Traysoft telephony demo."
				'	' "Please press 1 to listen to the message."
				'	objCall.Play("..\..\Wavs\hello.wav")
				'	objCall.Play("..\..\Wavs\press1.wav")
				'	' Wait for a digit (up to 10 seconds)
				'	Dim digit As String
				'	digit = objCall.WaitForDigit(10)
				'	If digit = "1" Then
				'		' Button 1 was pressed, speak using selected voice
				'		If Not objCall.Line.SpeechVoice Is Nothing Then
				'			msg = String.Format("You have selected {0} voice.",
				'											objCall.Line.SpeechVoice.Name)
				'		Else
				'			msg = "You have selected default computer voice."
				'		End If
				'		objCall.Speak(msg, True)
				'		' Wait until speaking is finished
				'		objCall.WaitUntilDone(10)
				'		msg = String.Format("Message delivered to {0}.", objCall.CalledID)
				'		' Because it's another thread, we have to use Invoke to access UI controls
				'		listBoxLog.Invoke(m_addToLogDelegate, msg)
				'		Exit For
				'	End If
				'Next

				'' Play goodbye message:
				'' "Thank you for using Traysoft telephony demo. Goodbye, and have a good day."
				'objCall.Play("..\..\Wavs\goodbye.wav", True)
				'' Wait until playing is finished
				'objCall.WaitUntilDone(10)

			Catch exc As TapiDisconnectException
				' This exception is thrown if the other party hung up before we called Disconnect().
				listBoxLog.Invoke(m_addToLogDelegate, "The called party hung up.")
			Catch exc As Exception
				Dim msg As String
				msg = String.Format("Exception in CallHandler! {0}", exc.Message)
				listBoxLog.Invoke(m_addToLogDelegate, msg)
			Finally
				' Disconnect the call to release the line
				'objCall.Disconnect()
			End Try
		End Sub

		Private Function OpenTapiLine() As Boolean
			Dim result As Boolean = True

			' Check if a line was selected
			Dim objLine As TapiLine
			If m_CurrentTapiLine Is Nothing OrElse m_CurrentTapiLine.IsOpen Then Return False

			objLine = m_CurrentTapiLine
			If objLine Is Nothing Then
				MessageBox.Show("Please select a line first.")
				Return False
			End If
			' Check if the line is already open
			If objLine.IsOpen Then
				AddToLog("Already monitoring line " + objLine.Name)
				Return False
			End If
			' Open selected line
			Try
				objLine.RingsToAnswer = 0
				objLine.Open(True, Nothing)
				AddToLog("Started monitoring line " + objLine.Name)
			Catch exc As TapiException
				AddToLog(String.Format("TapiException: {0}", exc.ToString))
				Trace.WriteLine(String.Format("TapiException: {0}", exc.ToString))
				'MessageBox.Show(exc.ToString, "TapiException!", MessageBoxButtons.OK, MessageBoxIcon.Error)
				result = False
			Catch exc As Exception
				AddToLog(String.Format("Exception: {0}", exc.ToString))
				Trace.WriteLine(String.Format("Exception: {0}", exc.ToString))
				'MessageBox.Show(exc.Message, "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error)
				result = False
			End Try


			Return result

		End Function

		Private Function CloseTapiLine() As Boolean
			Dim result As Boolean = True

			' Check if a line was selected
			m_IsLineConected = False

			Dim objLine As TapiLine
			If m_CurrentTapiLine Is Nothing OrElse Not m_CurrentTapiLine.IsOpen Then Return False

			objLine = m_CurrentTapiLine
			If objLine Is Nothing Then
				MessageBox.Show("Please select a line first.")
				Return False
			End If
			' Close selected line
			Try
				If objLine.IsOpen Then
					objLine.Close()
				End If
				AddToLog("Stopped monitoring line " + objLine.Name)
			Catch exc As TapiException
				MessageBox.Show(exc.Message, "TapiException!", MessageBoxButtons.OK, MessageBoxIcon.Error)
				result = False
			Catch exc As Exception
				MessageBox.Show(exc.Message, "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error)
				result = False
			End Try


			Return result

		End Function

		' This event handler is called when a new incoming call is received.
		Private Sub OnIncomingCall(ByVal sender As Object, ByVal args As TapiEventArgs)
			' Display message in the log
			Dim msg = String.Format("Incoming call from {0} {1} on line '{2}'.", args.Call.CallerIDName, args.Call.CallerID, args.Line.Name)
			AddToLog(msg)

			btnDial.ImageOptions.Image = My.Resources.delete_16x16
			FocusAbailableData(args.Call.CallerID.ToString.Replace(" ", ""))

		End Sub

		' This event handler is called when a new outgoing call is detected.
		Private Sub OnOutgoingCall(ByVal sender As Object, ByVal args As TapiEventArgs)
			' Display message in the log
			Dim msg = String.Format("Outgoing call to {0} on line '{1}'.", args.Call.CalledID, args.Line.Name)

			btnDial.ImageOptions.Image = My.Resources.delete_16x16
			AddToLog(msg)

		End Sub

		' This event handler is called when the call has been connected.
		Private Sub OnCallConnected(ByVal sender As Object, ByVal args As TapiEventArgs)
			Dim msg As String
			If args.Call.Direction = TapiCallDirection.Incoming Then
				msg = String.Format("Connected incoming call from {0} on line '{1}'", args.Call.CallerID, args.Line.Name)
			Else
				msg = String.Format("Connected outgoing call to {0} on line '{1}'", args.Call.CalledID, args.Line.Name)
			End If
			m_IsLineConected = True
			btnDial.ImageOptions.Image = My.Resources.delete_16x16

			txtContactTitle.Enabled = True
			txtContactData.Enabled = True
			btnAddNewContact.Enabled = True

			txtContactTitle.EditValue = m_Translate.GetSafeTranslationValue("Wurde telefoniert")
			txtContactData.EditValue = String.Format("{0}: {1}{2}wurde telefoniert.{2}", args.Call.CalledID, args.Call.CalledIDName, vbNewLine)
			txtContactData.Properties.NullValuePrompt = m_Translate.GetSafeTranslationValue("Geben Sie hier Ihre Kontakt-Notizen ein...")
			If m_CreateAutoContact.GetValueOrDefault(False) Then AddAutomaticContactForHistory()

			AddToLog(msg)

			If grpContact.Enabled Then txtContactData.Focus()

			Try
				m_Logger.LogDebug(String.Format("args.Line.Name: {0} >>> lueTapiLines.EditValue: {1}", args.Line.Name.ToString().ToUpper, lueTapiLines.EditValue.ToUpper))
				If args.Line.Name.ToString().ToUpper = lueTapiLines.EditValue.ToUpper Then
					AddLogToCallHistory(args.Line.Name, args.Call.CallerID, args.Call.CalledID, 0, args.Call.Direction)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
			End Try

		End Sub

		' This event handler is called when the call has been disconnected.
		Private Sub OnCallDisconnected(ByVal sender As Object, ByVal args As TapiEventArgs)

			Dim msg = String.Format("Disconnected the call on line '{0}', call duration {1}", args.Line.Name, args.Call.CallDuration.ToString("g"))

			m_IsLineConected = False
			btnDial.ImageOptions.Image = My.Resources.phone3_16x16
			AddToLog(msg)

		End Sub

		' This event handler is called when AddTapi error occurs asynchronously
		Private Sub OnTapiError(ByVal sender As Object, ByVal args As TapiErrorEventArgs)
			' Display error in the log
			Dim msg As String
			If Not args.Line Is Nothing Then
				msg = String.Format("TapiError event, line '{0}'. {1}", args.Line.Name, args.Message)
			Else
				msg = String.Format("TapiError event. {0}", args.Message)
			End If

			m_IsLineConected = False
			AddToLog(msg)
		End Sub

		' This event handler is called when new line has been added
		' while application is running, for example USB modem was connected.
		Private Sub OnLineAdded(ByVal sender As Object, ByVal args As TapiEventArgs)
			LoadTapiLines()
		End Sub

		' This event handler is called when the line was forcibly closed
		' by Windows because of hardware error or configuration change.
		Private Sub OnLineClosed(ByVal sender As Object, ByVal args As TapiEventArgs)
			' Display message in the log
			Dim msg = String.Format("LineClosed event, line '{0}' was forcibly closed by Windows.", args.Line.Name)
			AddToLog(msg)
			LoadTapiLines()
		End Sub

		' This event handler is called when new line has been removed
		' while application is running, for example USB modem was disconnected.
		Public Sub OnLineRemoved(ByVal sender As Object, ByVal args As TapiEventArgs)
			LoadTapiLines()
		End Sub

		Private Sub AddToLog(ByVal msg As String)
			Dim ind As Integer

			m_Logger.LogDebug(String.Format("AddTapi.Net message: {0}", msg))
			ind = listBoxLog.Items.Add(msg)

			' Ensure that the added item is visible
			listBoxLog.TopIndex = ind

		End Sub

		Private Sub OnlistBoxLog_MeasureItem(ByVal sender As System.Object, ByVal e As MeasureItemEventArgs) Handles listBoxLog.MeasureItem
			Dim lb As ListBoxControl = sender
			Dim itemValue As String = lb.Items(e.Index)
			If (itemValue.Contains(vbNewLine)) Then e.ItemHeight = e.ItemHeight * Math.Max(1, (itemValue.Split(vbNewLine).Length)) ' 2
		End Sub

		Private Sub cbo_MyNumber_KeyDown(sender As Object, e As KeyEventArgs) Handles cbo_MyNumber.KeyDown
			If e.KeyCode = Keys.Enter Then
				FocusAbailableData(cbo_MyNumber.EditValue.ToString.Replace(" ", ""))
			End If
		End Sub

		Private Sub cbo_MyNumber_TextChanged(sender As Object, e As EventArgs) Handles cbo_MyNumber.TextChanged

			btnDial.Enabled = Not String.IsNullOrWhiteSpace(cbo_MyNumber.EditValue.ToString)

			If m_SuppressUIEvents Then Return
			FocusAbailableData(cbo_MyNumber.EditValue.ToString.Replace(" ", ""))

		End Sub

		Private Sub OngvAvailableData_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvAvailableData.FocusedRowChanged

			If m_SuppressUIEvents Then
				Return
			End If

			Dim selectedData = SelectedAvailableRowViewData

			If Not selectedData Is Nothing Then
				ShowRecordDetails(selectedData)
			End If

		End Sub

		Sub OngvAvailableData_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim commonData = SelectedAvailableRowViewData
				If Not commonData Is Nothing Then
					grpContact.Enabled = True

					Dim suppressUIEventsState = m_SuppressUIEvents
					m_SuppressUIEvents = True

					If e.Column.Name.ToLower = "Telephon".ToLower Then
						cbo_MyNumber.EditValue = commonData.Telephon
					Else
						If Not m_CurrentModulTyp = TelephonyRecordSource.ResponsiblePerson Then OpenAssignedData()

					End If


					m_SuppressUIEvents = False

				End If
			End If

		End Sub

		'Private Sub OngvAvailableData_CustomDrawCell(sender As Object, e As Views.Base.RowCellCustomDrawEventArgs) Handles gvAvailableData.CustomDrawCell

		'	If e.Column.Name.ToLower = "amountDecision".ToLower Then
		'		Dim state = CType(e.CellValue, EsrItem.PaymentProcessState)
		'		'Dim rec As Rectangle = New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1)

		'		Dim p As Point = New Point()
		'		Dim img As Image

		'		Select Case state
		'			Case PaymentProcessState.InProcessing
		'				img = m_InProcessing

		'			Case PaymentProcessState.Processed
		'				img = m_Processed

		'			Case PaymentProcessState.Question
		'				img = m_Question

		'			Case PaymentProcessState.Failed
		'				img = m_Failed

		'			Case PaymentProcessState.Lower
		'				img = m_Lower

		'			Case PaymentProcessState.Higher
		'				img = m_Higher

		'			Case Else
		'				img = m_Unprocessed

		'		End Select

		'		p.X = e.Bounds.Location.X + (e.Bounds.Width - img.Width) / 2
		'		p.Y = e.Bounds.Location.Y + (e.Bounds.Height - img.Height) / 2
		'		e.Graphics.DrawImage(img, p)

		'	End If

		'End Sub

		Sub OngvContactData_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			Dim contactData = SelectedContactRowViewData

			txtContactTitle.EditValue = String.Empty
			txtContactData.EditValue = String.Empty
			txtContactTitle.Enabled = False
			txtContactData.Enabled = False
			btnAddNewContact.Enabled = False

			If (e.Clicks = 2) Then

				Dim column = e.Column
				If Not contactData Is Nothing Then

					Dim suppressUIEventsState = m_SuppressUIEvents
					m_SuppressUIEvents = True

					OpenAssignedConatact()

					m_SuppressUIEvents = False

				End If
			End If

		End Sub

		Private Sub ShowRecordDetails(ByVal commonData As CommonTelephonyData)

			m_CurrentEmployeeNumber = Nothing
			m_CurrentCustomerNumber = Nothing
			m_CurrentCustomerNumber = Nothing

			If Not commonData Is Nothing Then
				If commonData.ModulSource = TelephonyRecordSource.ResponsiblePerson Then
					m_CurrentCResponsibleNumber = commonData.ZNumber
					m_CurrentCustomerNumber = commonData.RecNumber
					m_CurrentModulTyp = TelephonyRecordSource.ResponsiblePerson

					ResetCustomerContactDetailGrid()
					LoadCustomerContactDetailList()

				ElseIf commonData.ModulSource = TelephonyRecordSource.Customer Then
					m_CurrentCustomerNumber = commonData.RecNumber
					m_CurrentModulTyp = TelephonyRecordSource.Customer

					ResetCustomerContactDetailGrid()
					LoadCustomerContactDetailList()

				Else
					m_CurrentEmployeeNumber = commonData.RecNumber
					m_CurrentModulTyp = TelephonyRecordSource.Employee

					ResetEmployeeContactDetailGrid()
					LoadEmployeeContactDetailList()

				End If

			End If

		End Sub

		Private Function LoadEmployeeContactDetailList() As Boolean
			Dim filterYears = New List(Of Integer) From {Now.Year - 1, Now.Year}
			Dim yearsArray = filterYears.ToArray()
			Dim contactData = m_EmployeeDatabaseAccess.LoadEmployeeContactOverviewDataBySearchCriteria(m_CurrentEmployeeNumber, True,
																																															 True,
																																															 True,
																																															 True,
																																															 yearsArray)

			If contactData Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Fehler in der Kontakt-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
				Return False
			End If

			Dim listDataSource As BindingList(Of ContactViewData) = New BindingList(Of ContactViewData)

			' Convert the data to view data.
			For Each p In contactData

				Dim cViewData = New ContactViewData() With {
							.ID = p.ID,
							.EmployeeNumber = p.EmployeeNumber,
							.ContactRecorNumber = p.RecNr,
							.ContactDate = p.ContactDate,
							.minContactDate = p.minContactDate,
							.maxContactDate = p.maxContactDate,
							.Person_Subject = p.PersonOrSubject,
							.Description = p.Description,
							.Important = p.IsImportant,
							.Completed = p.IsCompleted,
							.KDKontactRecID = p.KDKontactRecID,
							.CreatedFrom = p.CreatedFrom}

				listDataSource.Add(cViewData)
			Next
			grdContact.DataSource = listDataSource

			Return True
		End Function

		Private Function LoadCustomerContactDetailList() As Boolean
			Dim filterYears = New List(Of Integer) From {Now.Year - 1, Now.Year}
			Dim yearsArray = filterYears.ToArray()
			Dim contactData = m_CustomerDatabaseAccess.LoadCustomerContactOverviewlDataBySearchCriteria(m_CurrentCustomerNumber, Nothing, False, False, False, False, yearsArray)

			If contactData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in der Kontakt-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt."))
				Return False
			End If

			Dim listDataSource As BindingList(Of ContactViewData) = New BindingList(Of ContactViewData)

			' Convert the data to view data.
			For Each p In contactData

				Dim cViewData = New ContactViewData() With {
							.ID = p.ID,
							.CustomerNumber = p.CustomerNumber,
							.ContactRecorNumber = p.RecNr,
							.ContactDate = p.ContactDate,
							.minContactDate = p.minContactDate,
							.maxContactDate = p.maxContactDate,
							.Person_Subject = p.PersonOrSubject,
							.Description = p.Description,
							.Important = p.IsImportant,
							.Completed = p.IsCompleted,
							.Creator = p.Creator}

				listDataSource.Add(cViewData)
			Next
			grdContact.DataSource = listDataSource

			Return True
		End Function

		Private Sub btnAddNewESMALA_Click(sender As Object, e As EventArgs) Handles btnAddNewContact.Click

			' create new contact record
			AddAutomaticContactForHistory()

		End Sub

		Private Sub AddAutomaticContactForHistory()
			Dim success As Boolean = True
			Dim title As String = txtContactTitle.EditValue
			Dim description As String = txtContactData.EditValue
			Dim contactType As String = String.Empty

			If Not grpContact.Enabled Then Return

			Try
				contactType = "Telefonisch"
				If success AndAlso m_CurrentModulTyp = TelephonyRecordSource.Employee Then
					AddNewEmployeeContact(title, description, contactType, CType(Format(Now, "d"), Date), CType(Format(Now, "t"), DateTime), Nothing)

				ElseIf m_CurrentModulTyp <> TelephonyRecordSource.Employee Then
					AddNewCustomerContact(title, description, contactType, CType(Format(Now, "d"), Date), CType(Format(Now, "t"), DateTime), Nothing)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("contact could not be imsertetd! {0}", ex.ToString))
			End Try

		End Sub

		Private Sub AddNewEmployeeContact(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String)

			Dim currentContactRecordNumber As Integer = 0
			Dim currentDocumentID As Integer = 0
			Dim contactData As EmployeeContactData = Nothing
			'Dim fileContent = m_Utility.LoadFileBytes(attachedFile)
			Dim advisorFullname As String = m_InitializationData.UserData.UserFullName

			Try

				Dim dt = DateTime.Now
				contactData = New EmployeeContactData With {.EmployeeNumber = m_CurrentEmployeeNumber.GetValueOrDefault(0), .CreatedOn = dt, .CreatedFrom = advisorFullname}

				contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
				contactData.ContactType1 = If(String.IsNullOrWhiteSpace(contactType), 0, contactType)
				contactData.ContactPeriodString = title
				contactData.ContactsString = description
				contactData.ContactImportant = False
				contactData.ContactFinished = False
				contactData.VacancyNumber = Nothing
				contactData.ProposeNr = Nothing
				contactData.ESNr = Nothing
				contactData.CustomerNumber = Nothing

				contactData.ChangedFrom = advisorFullname
				contactData.ChangedOn = dt
				contactData.UsNr = m_InitializationData.UserData.UserNr

				Dim success As Boolean = True

				' Insert contact
				contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_EmployeeDatabaseAccess.AddEmployeeContact(contactData)

				If success Then
					currentContactRecordNumber = contactData.RecordNumber
					LoadEmployeeContactDetailList()

					If gvContact.RowCount > 0 Then FocusContactData(contactData.ID) 'currentContactRecordNumber)
				End If

				If Not success Then
					m_Logger.LogError("add new contact was not successfull!")
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("contact could not be imsertetd! {0} >>> EmployeeNumber: {1}", m_CurrentEmployeeNumber.GetValueOrDefault(0), ex.ToString))
			End Try

		End Sub

		Private Sub AddNewCustomerContact(ByVal title As String, ByVal description As String, ByVal contactType As String, ByVal contactDate As Date, ByVal contactTime As DateTime, ByVal attachedFile As String)
			Dim contactData As ResponsiblePersonAssignedContactData = Nothing
			Dim m_CurrentContactRecordNumber As Integer?
			Dim m_UtilityUI As New SP.Infrastructure.UI.UtilityUI

			Try
				Dim dt = DateTime.Now
				If Not m_CurrentContactRecordNumber.HasValue Then
					contactData = New ResponsiblePersonAssignedContactData With {
						.CustomerNumber = m_CurrentCustomerNumber.GetValueOrDefault(0),
						.ResponsiblePersonNumber = m_CurrentCResponsibleNumber.GetValueOrDefault(0),
						.CreatedOn = dt,
						.CreatedFrom = m_InitializationData.UserData.UserFullName}
				End If

				contactData.ContactDate = CombineDateAndTime(contactDate, contactTime)
				contactData.ContactType1 = contactType
				contactData.ContactPeriodString = title
				contactData.ContactsString = description
				contactData.ContactImportant = False
				contactData.ContactFinished = False

				contactData.ChangedFrom = m_InitializationData.UserData.UserFullName
				contactData.ChangedOn = dt
				contactData.UsNr = m_InitializationData.UserData.UserNr

				Dim success As Boolean = True

				' Insert or update contact
				contactData.CreatedUserNumber = m_InitializationData.UserData.UserNr
				success = success AndAlso m_CustomerDatabaseAccess.AddResponsiblePersonContactAssignment(contactData)
				If success Then
					LoadCustomerContactDetailList()
					Dim CurrentContactRecordNumber = contactData.RecordNumber

					If gvContact.RowCount > 0 Then FocusContactData(contactData.ID) 'CurrentContactRecordNumber)

				Else
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kontaktdaten konnten nicht gespeichert werden."))

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("contact could not be imsertetd! {0} >>> CustomerNumber: {1}", m_CurrentCustomerNumber.GetValueOrDefault(0), ex.ToString))
			End Try

		End Sub

		Private Sub OpenAssignedData()
			Dim contactData = SelectedContactRowViewData

			If m_CurrentModulTyp = TelephonyRecordSource.Employee AndAlso m_CurrentEmployeeNumber.GetValueOrDefault(0) > 0 Then
				Dim hub = MessageService.Instance.Hub
				Dim openEmployeeMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CurrentEmployeeNumber)
				hub.Publish(openEmployeeMng)

			ElseIf m_CurrentModulTyp = TelephonyRecordSource.Customer AndAlso m_CurrentCustomerNumber.GetValueOrDefault(0) > 0 Then
				Dim hub = MessageService.Instance.Hub
				Dim openCustomerMng As New OpenCustomerMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CurrentCustomerNumber)
				hub.Publish(openCustomerMng)

			End If

		End Sub

		Private Sub OpenAssignedConatact()
			Dim contactData = SelectedContactRowViewData

			If m_CurrentModulTyp = TelephonyRecordSource.Employee Then
				Dim m_ContactEmployeeDetailForm = New SP.MA.KontaktMng.frmContacts(m_InitializationData)
				Dim m_ContactEmployeeFilterSettingsForDetailForm = New SP.MA.KontaktMng.ContactFilterSettings

				m_ContactEmployeeFilterSettingsForDetailForm.AddYear(Now.Year)

				RemoveHandler m_ContactEmployeeDetailForm.ContactDataSaved, AddressOf OnEmployeeContactInfoFormDataSaved
				RemoveHandler m_ContactEmployeeDetailForm.ContactDataDeleted, AddressOf OnEmployeeContactInfoFormDataDeleted

				AddHandler m_ContactEmployeeDetailForm.ContactDataSaved, AddressOf OnEmployeeContactInfoFormDataSaved
				AddHandler m_ContactEmployeeDetailForm.ContactDataDeleted, AddressOf OnEmployeeContactInfoFormDataDeleted

				m_ContactEmployeeDetailForm.Show()
				m_ContactEmployeeDetailForm.LoadContactData(m_CurrentEmployeeNumber, contactData.ContactRecorNumber, Nothing)
				m_ContactEmployeeDetailForm.BringToFront()
			Else

				Dim m_ContactCustomerDetailForm = New SP.KD.KontaktMng.frmContacts(m_InitializationData)
				Dim m_ContactCustomerFilterSettingsForDetailForm = New SP.MA.KontaktMng.ContactFilterSettings
				m_ContactCustomerFilterSettingsForDetailForm.AddYear(Now.Year)

				RemoveHandler m_ContactCustomerDetailForm.ContactDataSaved, AddressOf OnCustomerContactInfoFormDataSaved
				RemoveHandler m_ContactCustomerDetailForm.ContactDataDeleted, AddressOf OnCustomerContactInfoFormDataDeleted

				AddHandler m_ContactCustomerDetailForm.ContactDataSaved, AddressOf OnCustomerContactInfoFormDataSaved
				AddHandler m_ContactCustomerDetailForm.ContactDataDeleted, AddressOf OnCustomerContactInfoFormDataDeleted

				m_ContactCustomerDetailForm.Show()
				m_ContactCustomerDetailForm.LoadContactData(m_CurrentCustomerNumber, m_CurrentCResponsibleNumber, contactData.ContactRecorNumber, Nothing)
				m_ContactCustomerDetailForm.BringToFront()

			End If

		End Sub

		Private Sub OnEmployeeContactInfoFormDataSaved(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)
			LoadEmployeeContactDetailList()
			Dim contatsForm = CType(sender, SP.MA.KontaktMng.frmContacts)

			FocusContactData(contactRecordNumber)

		End Sub

		Private Sub OnEmployeeContactInfoFormDataDeleted(ByVal sender As Object, ByVal employeeNumber As Integer, ByVal contactRecordNumber As Integer)
			LoadEmployeeContactDetailList()
		End Sub

		Private Sub OnCustomerContactInfoFormDataSaved(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)
			LoadCustomerContactDetailList()
			Dim contatsForm = CType(sender, SP.KD.KontaktMng.frmContacts)

			FocusContactData(contactRecordNumber)

		End Sub

		Private Sub OnCustomerContactInfoFormDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer, ByVal responsiblePersonRecordNumber As Integer?, ByVal contactRecordNumber As Integer)
			LoadCustomerContactDetailList()
		End Sub

		Private Sub FocusAbailableData(ByVal phoneNumber As String)

			If Not m_TelephonyData Is Nothing AndAlso m_TelephonyData.Count > 0 Then

				Dim index = m_TelephonyData.ToList().FindIndex(Function(data) data.Telephon.Contains(phoneNumber))
				If index < 0 Then
					m_CurrentEmployeeNumber = Nothing
					m_CurrentCustomerNumber = Nothing
					m_CurrentCResponsibleNumber = Nothing
					grpContact.Enabled = False

					Return
				Else
					grpContact.Enabled = True

				End If
				Dim rowHandle = gvAvailableData.GetRowHandle(index)
				gvAvailableData.FocusedRowHandle = rowHandle

				gvAvailableData.MakeRowVisible(rowHandle)

			End If

		End Sub

		Private Sub FocusContactData(ByVal recID As Integer)

			Try
				If Not grdContact.DataSource Is Nothing Then

					Dim commonViewData = CType(gvContact.DataSource, IEnumerable(Of ContactViewData))

					Dim index = commonViewData.ToList().FindIndex(Function(data) data.ID = recID)

					'm_SuppressUIEvents = True
					Dim rowHandle = gvAvailableData.GetRowHandle(index)
					gvContact.FocusedRowHandle = rowHandle
					'm_SuppressUIEvents = False

				End If

			Catch ex As Exception
				m_Logger.LogWarning(ex.ToString)
			End Try

		End Sub


#End Region

		Private Sub AddLogToCallHistory(ByVal lineName As String, ByVal outgoingNumber As String, ByVal incommingNumber As String, ByVal callDuration As String, ByVal callDirection As TapiCallDirection)

			Dim paramItems As String = String.Empty
			Dim m_Utilities As New Utilities
			Dim userTapiID As String = String.Empty

			Trace.WriteLine(String.Format("lineName: {0} | outgoingNumber: {1} | incommingNumber: {2} | callDuration: {3} | callDirection: {4}", lineName, outgoingNumber, incommingNumber, callDuration, callDirection))
			m_Logger.LogError(String.Format("lineName: {0} | outgoingNumber: {1} | incommingNumber: {2} | callDuration: {3} | callDirection: {4}", lineName, outgoingNumber, incommingNumber, callDuration, callDirection))
			Dim searcheCallData = New CallHistoryData With {.Advisor = m_InitializationData.UserData.UserFullName}
			searcheCallData.CallDuration = callDuration
			searcheCallData.USNr = m_InitializationData.UserData.UserNr
			searcheCallData.CallHandle = Nothing
			searcheCallData.UserTapiID = lineName
			searcheCallData.CallID = Nothing
			searcheCallData.CalledFrom = If(callDirection = TapiCallDirection.Outgoing, outgoingNumber, incommingNumber)
			searcheCallData.CalledTo = If(callDirection = TapiCallDirection.Outgoing, incommingNumber, outgoingNumber)
			searcheCallData.Incoming = callDirection = TapiCallDirection.Incoming
			searcheCallData.CustomerNumber = m_CurrentCustomerNumber
			searcheCallData.ResponslibePerson = m_CurrentCResponsibleNumber
			searcheCallData.EmployeeNumber = m_CurrentEmployeeNumber
			searcheCallData.ModeNr = If(callDirection = TapiCallDirection.Incoming, 1, 0)

			Try
				Dim success = m_CustomerDatabaseAccess.AddCallHistory(m_InitializationData.MDData.MDNr, searcheCallData)
				If Not success Then
					Dim msg = "Call-Ereignisse konnten nicht gespeichert werden."
					m_Logger.LogError(String.Format("msg: {0}", msg))

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		End Sub

		Private Function CombineDateAndTime(ByVal dateComponent As DateTime?, ByVal timeComponent As DateTime?) As DateTime?

			If Not dateComponent.HasValue Then
				Return Nothing
			End If

			If Not timeComponent.HasValue Then
				Return dateComponent.Value.Date
			End If

			Dim timeSpan As TimeSpan = timeComponent.Value - timeComponent.Value.Date
			Dim dateAndTime = dateComponent.Value.Date.Add(timeSpan)

			Return dateAndTime
		End Function

		Protected Function SetDXErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText, ErrorType.Critical)
			Else
				errorProvider.SetError(control, String.Empty)
			End If

			Return Not invalid

		End Function

		Protected Function SetDXWarningIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText, ErrorType.Warning)
			End If

			Return Not invalid

		End Function

#Region "View helper classes"

		''' <summary>
		'''  Contact view data.
		''' </summary>
		Class ContactViewData
			Public Property ID As Integer
			Public Property EmployeeNumber As Integer
			Public Property CustomerNumber As Integer
			Public Property ContactRecorNumber As Integer
			Public Property ContactDate As DateTime?
			Public Property minContactDate As DateTime?
			Public Property maxContactDate As DateTime?
			Public Property Person_Subject As String
			Public Property Description As String
			Public Property Important As Boolean?
			Public Property Completed As Boolean?
			Public Property CreatedFrom As String
			Public Property Creator As String
			Public Property PDFImage As Image
			Public Property DocumentId As Integer?
			Public Property KDKontactRecID As Integer?
		End Class



#End Region

	End Class


End Namespace

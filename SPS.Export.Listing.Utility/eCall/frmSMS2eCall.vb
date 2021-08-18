
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports System.Windows.Forms
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports DevExpress.XtraGrid
Imports DevExpress.XtraEditors.Controls
Imports SPSSendMail
Imports System.Drawing
Imports System.IO
Imports SPProgUtility.ProgPath
Imports System.Security.Cryptography
Imports System.Text
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SPProgUtility.CommonXmlUtility

Public Enum ReceiverType
	'Kandidat
	Employee = 1
	'Kunde
	Customer = 2
End Enum


Public Class frmSMS2eCall


#Region "Private Fields"
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_utility As Utilities
	Private m_md As Mandant
	Private m_translate As TranslateValues
	Private m_deleteHomeFolder As String

	Private m_templates As List(Of Template)
	Private m_actualTemplate As Template
	Private m_isTemplateShowing As Boolean

	Private m_sqlSelectReceiverList As String
	Private m_receiverType As ReceiverType

	Private m_random As Random = New Random

	Private m_contagtLogger As ContactLogger
	Private m_sendCount As Integer

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess


	Private WithEvents m_frmSMS2eCallWait As frmSMS2eCallWait

	Private m_connectionString As String

#End Region



#Region "Public Properties"

	Public Property RecCount As Integer
	Public Property QuickSearchData As BindingList(Of ExistingEmployeeSearchData)

#End Region


#Region "Constructor"

	Public Sub New(ByVal _Setting As InitializeClass, ByVal strQuery As String, ByVal receiverType As ReceiverType)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_md = New Mandant
		m_utility = New Utilities
		m_translate = New TranslateValues

		InitializeComponent()

		If _Setting.MDData Is Nothing Then
			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.PersonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		Else
			ModulConstants.MDData = _Setting.MDData
			ModulConstants.UserData = _Setting.UserData
			ModulConstants.PersonalizedData = _Setting.PersonalizedData
			ModulConstants.TranslationData = _Setting.TranslationData

		End If
		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ModulConstants.TranslationData, ModulConstants.PersonalizedData, ModulConstants.MDData, ModulConstants.UserData)

		m_connectionString = ModulConstants.MDData.MDDbConn
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, ModulConstants.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, ModulConstants.UserData.UserLanguage)

		Me.m_sqlSelectReceiverList = strQuery
		Me.m_receiverType = receiverType
		m_deleteHomeFolder = (New ClsProgPath).GetSpS2DeleteHomeFolder()

		Reset()


		AddHandler lueTemplates.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region


#Region "public Methodes"

	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		result = result AndAlso InitForm()
		btnSend.Enabled = IsMandantAllowedAsSender()

		Return result

	End Function

#End Region


#Region "Private Methods"

	Private Function IsMandantAllowedAsSender() As Boolean
		Dim result As Boolean = True
		Dim providerData = LoadPrivderData()

		If providerData Is Nothing OrElse String.IsNullOrWhiteSpace(providerData.UserName) Then Return False


		Return result

	End Function

	Private Function LoadPrivderData() As SP.Internal.Automations.ProviderViewData
		Dim providerObj As New SP.Internal.Automations.ProviderData(m_InitializationData)
		Dim result = providerObj.LoadProviderData(m_InitializationData.MDData.MDGuid, "eCall")

		If String.IsNullOrWhiteSpace(result.UserName) Then Return Nothing


		Return result

	End Function

	Private Sub TranslateControls()

		Try
			Me.Text = m_translate.GetSafeTranslationValue(Me.Text)
			Me.lblHeader1.Text = m_translate.GetSafeTranslationValue(Me.lblHeader1.Text)
			Me.lblHeader2.Text = m_translate.GetSafeTranslationValue(Me.lblHeader2.Text)

			tgsSelection.Properties.OffText = m_translate.GetSafeTranslationValue(tgsSelection.Properties.OffText)
			tgsSelection.Properties.OnText = m_translate.GetSafeTranslationValue(tgsSelection.Properties.OnText)

			Me.xtabVersanddata.Text = m_translate.GetSafeTranslationValue(Me.xtabVersanddata.Text)
			Me.xtabResultData.Text = m_translate.GetSafeTranslationValue(Me.xtabResultData.Text)
			Me.grpNachrichten.Text = m_translate.GetSafeTranslationValue(Me.grpNachrichten.Text)
			Me.lblVorlage.Text = m_translate.GetSafeTranslationValue(Me.lblVorlage.Text)
			Me.lblBeispiel.Text = m_translate.GetSafeTranslationValue(Me.lblBeispiel.Text)

			Me.btnClose.Text = m_translate.GetSafeTranslationValue(Me.btnClose.Text)
			Me.btnSend.Text = m_translate.GetSafeTranslationValue(Me.btnSend.Text)

			Me.lblTestNummer.Text = m_translate.GetSafeTranslationValue(Me.lblTestNummer.Text)
			Me.lblAntwortadresse.Text = m_translate.GetSafeTranslationValue(Me.lblAntwortadresse.Text)

			Me.bsiInfo.Caption = m_translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}", ex.ToString))
		End Try

	End Sub

	Private Sub Reset()
		m_actualTemplate = Nothing

		ResetReceiverGrid()
		ResetTemplateLookup()

	End Sub

	''' <summary>
	''' Initialized the gui - Grid and Lookups
	''' </summary>
	Private Function InitForm() As Boolean
		Dim success As Boolean = True

		m_actualTemplate = Nothing

		If QuickSearchData Is Nothing Then
			success = success AndAlso LoadReceiverData()
		Else
			success = success AndAlso LoadReciversFromObjectData()
		End If

		RecCount = gvMain.RowCount
		bsiInfo.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)

		success = success AndAlso LoadTemplateData()
		success = success AndAlso LoadUserDataForAnswer()

		Return success
	End Function

	Private Function LoadUserDataForAnswer() As Boolean

		Try
			Me.cboAnswerAddress.Properties.Items.Clear()

			If Not String.IsNullOrWhiteSpace(ModulConstants.UserData.UserMobile) Then
				Me.cboAnswerAddress.Properties.Items.Add(ModulConstants.UserData.UserMobile)
			End If

			If Not String.IsNullOrWhiteSpace(ModulConstants.UserData.UserTelefon) Then
				Me.cboAnswerAddress.Properties.Items.Add(ModulConstants.UserData.UserTelefon)
			End If

			If Not String.IsNullOrWhiteSpace(ModulConstants.UserData.UserMDTelefon) Then
				Me.cboAnswerAddress.Properties.Items.Add(ModulConstants.UserData.UserMDTelefon)
			End If
			If Not String.IsNullOrWhiteSpace(ModulConstants.UserData.UserMDDTelefon) Then
				Me.cboAnswerAddress.Properties.Items.Add(ModulConstants.UserData.UserMDDTelefon)
			End If


			Return True
		Catch ex As Exception
			Return False
		End Try

	End Function

	''' <summary>
	''' Resets the receiver grid layout.
	''' </summary>
	Private Sub ResetReceiverGrid()

		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True
		gvMain.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowFooter = False
		gvMain.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvMain.Columns.Clear()

		Dim columnSelectedRec As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSelectedRec.OptionsColumn.AllowEdit = True
		columnSelectedRec.Caption = m_translate.GetSafeTranslationValue("SMS")
		columnSelectedRec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
		columnSelectedRec.Name = "IsSelected"
		columnSelectedRec.FieldName = "IsSelected"
		columnSelectedRec.Visible = True
		columnSelectedRec.Width = 10
		gvMain.Columns.Add(columnSelectedRec)

		If Me.m_receiverType = Utility.ReceiverType.Employee Then

			Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMANr.OptionsColumn.AllowEdit = False
			columnMANr.Caption = m_translate.GetSafeTranslationValue("Kandidaten-Nr.")
			columnMANr.Name = "MANr"
			columnMANr.FieldName = "MANr"
			columnMANr.Visible = False
			gvMain.Columns.Add(columnMANr)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_translate.GetSafeTranslationValue("Kandidat")
			columnName.Name = "Name"
			columnName.FieldName = "Name"
			columnName.Visible = True
			gvMain.Columns.Add(columnName)

			Dim columnAdresse As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdresse.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdresse.OptionsColumn.AllowEdit = False
			columnAdresse.Caption = m_translate.GetSafeTranslationValue("Adresse")
			columnAdresse.Name = "Adresse"
			columnAdresse.FieldName = "Adresse"
			columnAdresse.Visible = True
			gvMain.Columns.Add(columnAdresse)

			Dim columnNatel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNatel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNatel.OptionsColumn.AllowEdit = False
			columnNatel.Caption = m_translate.GetSafeTranslationValue("Telefonnummer")
			columnNatel.Name = "Natel"
			columnNatel.FieldName = "Natel"
			columnNatel.Visible = True
			gvMain.Columns.Add(columnNatel)


		ElseIf Me.m_receiverType = Utility.ReceiverType.Customer Then
			Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKDNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnKDNr.OptionsColumn.AllowEdit = False
			columnKDNr.Caption = m_translate.GetSafeTranslationValue("Kunden-Nr.")
			columnKDNr.Name = "KDNr"
			columnKDNr.FieldName = "KDNr"
			columnKDNr.Visible = False
			gvMain.Columns.Add(columnKDNr)

			Dim columnZHDRecNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnZHDRecNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnZHDRecNr.OptionsColumn.AllowEdit = False
			columnZHDRecNr.Caption = m_translate.GetSafeTranslationValue("Zuständige Person-Nr.")
			columnZHDRecNr.Name = "ZHDRecNr"
			columnZHDRecNr.FieldName = "ZHDRecNr"
			columnZHDRecNr.Visible = False
			gvMain.Columns.Add(columnZHDRecNr)

			Dim columnFirma As New DevExpress.XtraGrid.Columns.GridColumn()
			columnFirma.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnFirma.OptionsColumn.AllowEdit = False
			columnFirma.Caption = m_translate.GetSafeTranslationValue("Firma")
			columnFirma.Name = "Firma"
			columnFirma.FieldName = "Firma"
			columnFirma.Visible = True
			gvMain.Columns.Add(columnFirma)

			Dim columnAdresse As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdresse.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAdresse.OptionsColumn.AllowEdit = False
			columnAdresse.Caption = m_translate.GetSafeTranslationValue("Adresse")
			columnAdresse.Name = "Adresse"
			columnAdresse.FieldName = "Adresse"
			columnAdresse.Visible = True
			gvMain.Columns.Add(columnAdresse)

			Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnName.OptionsColumn.AllowEdit = False
			columnName.Caption = m_translate.GetSafeTranslationValue("Zuständige Person")
			columnName.Name = "Name"
			columnName.FieldName = "Name"
			columnName.Visible = True
			gvMain.Columns.Add(columnName)

			Dim columnNatel As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNatel.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNatel.OptionsColumn.AllowEdit = False
			columnNatel.Caption = m_translate.GetSafeTranslationValue("Telefonnummer")
			columnNatel.Name = "Natel"
			columnNatel.FieldName = "Natel"
			columnNatel.Visible = True
			gvMain.Columns.Add(columnNatel)

		End If


		grdMain.DataSource = Nothing

	End Sub

	''' <summary>
	''' Initialized the template-lookup
	''' </summary>
	Private Sub ResetTemplateLookup()

		lueTemplates.Properties.DisplayMember = "BezValue"
		lueTemplates.Properties.ValueMember = "Id"

		Dim columns = lueTemplates.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("BezValue", 0))

		lueTemplates.Properties.ShowHeader = False
		lueTemplates.Properties.ShowFooter = False
		lueTemplates.Properties.DropDownRows = 10
		lueTemplates.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueTemplates.Properties.SearchMode = SearchMode.AutoComplete
		lueTemplates.Properties.AutoSearchColumnIndex = 0

		lueTemplates.Properties.NullText = String.Empty
		lueTemplates.EditValue = Nothing
		m_isTemplateShowing = False

	End Sub

	''' <summary>
	'''  Loads all available receivers 
	''' </summary>
	Private Function LoadReceiverData() As Boolean

		Dim bindingList As BindingList(Of ReceiverView) = New BindingList(Of ReceiverView)
		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Me.m_sqlSelectReceiverList, Nothing)
		m_Logger.LogInfo(String.Format("ReceiverList: {0}", m_sqlSelectReceiverList))

		Try
			If (reader IsNot Nothing) Then

				While reader.Read()

					Dim natel = m_utility.SafeGetString(reader, "Natel", "-")
					If natel.Length < 10 Then
						Continue While
					End If

					If Me.m_receiverType = Utility.ReceiverType.Employee Then
						bindingList.Add(New ReceiverView(New ReceiverViewData() With {
							.MANr = m_utility.SafeGetInteger(reader, "MANr", 0),
							.Vorname = m_utility.SafeGetString(reader, "Vorname"),
							.Nachname = m_utility.SafeGetString(reader, "Nachname"),
							.AnredeForm = m_utility.SafeGetString(reader, "Anredeform"),
							.Strasse = m_utility.SafeGetString(reader, "strasse"),
							.Land = m_utility.SafeGetString(reader, "Land"),
							.PLZ = m_utility.SafeGetString(reader, "PLZ"),
							.Ort = m_utility.SafeGetString(reader, "Ort"),
							.Natel = natel,
							.IsSelected = tgsSelection.EditValue
						}))

					ElseIf Me.m_receiverType = Utility.ReceiverType.Customer Then
						bindingList.Add(New ReceiverView(New ReceiverViewData() With {
							.KDNr = m_utility.SafeGetInteger(reader, "KDNr", 0),
							.ZHDRecNr = m_utility.SafeGetInteger(reader, "zhdrecnr", Nothing),
							.Firma = m_utility.SafeGetString(reader, "Firma1"),
							.Vorname = m_utility.SafeGetString(reader, "Vorname"),
							.Nachname = m_utility.SafeGetString(reader, "Nachname"),
							.AnredeForm = m_utility.SafeGetString(reader, "Anredeform"),
							.Strasse = m_utility.SafeGetString(reader, "strasse"),
							.Land = m_utility.SafeGetString(reader, "Land"),
							.PLZ = m_utility.SafeGetString(reader, "PLZ"),
							.Ort = m_utility.SafeGetString(reader, "Ort"),
							.Natel = natel,
							.IsSelected = tgsSelection.EditValue
						}))

					End If

				End While

			End If

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		' Binding
		grdMain.DataSource = bindingList

		Return grdMain IsNot Nothing
	End Function

	Private Function LoadReciversFromObjectData() As Boolean
		Dim result As Boolean = True
		Dim data = QuickSearchData.ToList()

		'data = data.OrderBy(Function(x) x.Lastname).ToList()
		If QuickSearchData Is Nothing OrElse QuickSearchData.Count = 0 Then Return False

		Dim bindingList As BindingList(Of ReceiverView) = New BindingList(Of ReceiverView)

		For Each item In QuickSearchData
			bindingList.Add(New ReceiverView(New ReceiverViewData() With {
							.MANr = item.EmployeeNumber,
							.Vorname = item.Firstname,
							.Nachname = item.Lastname,
							.AnredeForm = item.BriefAnrede,
							.Strasse = item.Street,
							.Land = item.CountryCode,
							.PLZ = item.Postcode,
							.Ort = item.Location,
							.Natel = item.MobilePhone,
							.IsSelected = True
						}))

		Next

		grdMain.DataSource = bindingList

		Return result
	End Function

	''' <summary>
	''' Loads all available sms-templates 
	''' </summary>
	Private Function LoadTemplateData() As Boolean
		m_templates = New List(Of Template)
		Dim Sql As String = "SELECT * from Tab_SMSTemplates Order By Bez_Value"
		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ModulConstants.MDData.MDDbConn, Sql, Nothing)

		Try
			If (reader IsNot Nothing) Then
				While reader.Read()
					Dim template As New Template(ModulConstants.UserData.UserLanguage)
					template.Id = m_utility.SafeGetInteger(reader, "ID", 0)
					template.BezValue = m_utility.SafeGetString(reader, "Bez_Value")
					template.Bez_D = m_utility.SafeGetString(reader, "Bez_D")
					template.Bez_I = m_utility.SafeGetString(reader, "Bez_I")
					template.Bez_F = m_utility.SafeGetString(reader, "Bez_F")
					template.Bez_E = m_utility.SafeGetString(reader, "Bez_E")
					m_templates.Add(template)
				End While
			End If

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		lueTemplates.Properties.DataSource = m_templates
		lueTemplates.Properties.ForceInitialize()

		Return (lueTemplates.Properties.DataSource IsNot Nothing)

	End Function

	''' <summary>
	''' Gets the selected Receiver.
	''' </summary>
	''' <returns>The selected receiver or nothing if none is selected.</returns>
	Private ReadOnly Property SelectedReceiver As ReceiverView
		Get
			'Dim gvMain = TryCast(grdMain.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			'If (gvMain IsNot Nothing) Then

			Dim selectedRows = gvMain.GetSelectedRows()

			If (selectedRows.Count > 0) Then
				Dim receiverView = CType(gvMain.GetRow(selectedRows(0)), ReceiverView)
				Return receiverView
			End If

			'End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the first Receiver.
	''' </summary>
	''' <returns>The selected receiver or nothing if none is selected.</returns>
	Private ReadOnly Property FirstReceiver As ReceiverView
		Get
			Dim receiverList As BindingList(Of ReceiverView) = grdMain.DataSource
			If receiverList.Count > 0 Then
				Return receiverList.Item(0)
			End If
			Return Nothing
		End Get

	End Property

	Private Sub SendSmsToSelectetReceivers()
		Dim smsList As List(Of ShortMessage) = New List(Of ShortMessage)
		Dim receiverList As BindingList(Of ReceiverView) = grdMain.DataSource

		For Each receiver In receiverList

			If receiver.IsSelected Then
				m_actualTemplate.EmployeeLanguage = LoadReceiverLanguage(receiver.MANr, receiver.KDNr)
				If lueTemplates.EditValue Is Nothing Then
					m_actualTemplate.Bez = meMessageText.EditValue
				End If

				smsList.Add(New ShortMessage With {
					.ReceiverId = receiver.Id,
					.Address = receiver.Natel,
					.Message = m_actualTemplate.GetMessage(receiver),
					.JobId = GenerateId(),
					.AnswerAddress = Me.cboAnswerAddress.Text
				})
			End If
		Next

		If smsList.Count > 0 Then
			m_sendCount = 0
			m_frmSMS2eCallWait = New frmSMS2eCallWait(smsList)
			m_frmSMS2eCallWait.StartPosition = FormStartPosition.Manual
			m_frmSMS2eCallWait.Location = New Point(Me.Location.X + (Me.Width - m_frmSMS2eCallWait.Width) / 2, Me.Location.Y + (Me.Height - m_frmSMS2eCallWait.Height) / 2)
			m_frmSMS2eCallWait.Show()
			Me.Enabled = False
		End If

	End Sub

	Private Sub SendSmsToTestNumer(ByVal address As String)
		Dim smsList As List(Of ShortMessage) = New List(Of ShortMessage)

		If m_actualTemplate Is Nothing Then m_actualTemplate = New Template(ModulConstants.UserData.UserLanguage) With {.Bez = meMessageText.EditValue}
		smsList.Add(New ShortMessage With {
			.ReceiverId = 0,
			.Address = address,
			.Message = m_actualTemplate.GetMessage(FirstReceiver),
			.JobId = GenerateId()
		})

		Dim question As String = String.Format(m_translate.GetSafeTranslationValue("Wollen Sie die Testnachricht als SMS versenden?{0}Ihre Nachricht lautet:{0}{0}{1}"),
																					 vbNewLine, m_actualTemplate.GetMessage(FirstReceiver))
		Dim dialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(question, m_translate.GetSafeTranslationValue("SMS-Nachricht senden"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
		If smsList.Count > 0 AndAlso (dialogResult = DialogResult.Yes) Then
			m_sendCount = 0
			m_frmSMS2eCallWait = New frmSMS2eCallWait(smsList)
			m_frmSMS2eCallWait.StartPosition = FormStartPosition.Manual
			m_frmSMS2eCallWait.Location = New Point(Me.Location.X + (Me.Width - m_frmSMS2eCallWait.Width) / 2, Me.Location.Y + (Me.Height - m_frmSMS2eCallWait.Height) / 2)
			m_frmSMS2eCallWait.Show()
			Me.Enabled = False
		End If
	End Sub

	Private Function GenerateId() As String
		Dim KeyLength As Integer = 45
		Dim a As String = "ABCDEFGHJKLMNOPQRSTUVWXYZ1234567890_-*@#?)/)(%+=¦"
		Dim chars() As Char = New Char((a.Length) - 1) {}
		chars = a.ToCharArray
		Dim data() As Byte = New Byte((KeyLength) - 1) {}
		Dim crypto As RNGCryptoServiceProvider = New RNGCryptoServiceProvider
		crypto.GetNonZeroBytes(data)
		Dim result As StringBuilder = New StringBuilder(KeyLength)
		For Each b As Byte In data
			result.Append(chars(b Mod (chars.Length - 1)))
		Next

		Return String.Format("{0}", result.ToString)
	End Function

	Private Function LoadReceiverLanguage(ByVal employeeNumber As Integer, ByVal customerNumber As Integer) As String
		Dim bezLang As String = "D"
		Dim employeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
		Dim customerData = m_CustomerDatabaseAccess.LoadCustomerMasterData(customerNumber, "%%")

		If employeeData Is Nothing AndAlso customerData Is Nothing Then
			m_Logger.LogError("md data could not be founded! " & ModulConstants.MDData.MDDbConn)
			Return bezLang
		End If
		If Not employeeData Is Nothing Then
			bezLang = employeeData.Language.ToString.ToUpper.Substring(0, 1)
		ElseIf Not customerData Is Nothing Then
			bezLang = customerData.Language.ToString.ToUpper.Substring(0, 1)
		End If


		Return bezLang

	End Function

	Private Sub ContactLog(ByVal message As ShortMessage, ByVal receiver As ReceiverView)
		If m_contagtLogger Is Nothing Then
			m_contagtLogger = New ContactLogger(New SPSSendMail.InitializeClass With {
				.MDData = ModulConstants.MDData,
				.ProsonalizedData = ModulConstants.PersonalizedData,
				.TranslationData = ModulConstants.TranslationData,
				.UserData = ModulConstants.UserData
			})
		End If

		Dim cType1 = "SMS Versand"
		Dim cType2 As Short = 0

		If m_receiverType = ReceiverType.Employee Then
			m_contagtLogger.NewEmployeeContact(receiver.MANr, message.Address, message.Message, cType1, cType2, DateTime.Now, False, True, DateTime.Now, ModulConstants.UserData.UserFullName)
		Else
			m_contagtLogger.NewResponsiblePersonContact(receiver.KDNr, message.Address, message.Message, receiver.ZHDRecNr, DateTime.Now, Nothing, Nothing,
																									DateTime.Now, ModulConstants.UserData.UserFullName, Nothing, Nothing, cType1, cType2, False, True, True)
		End If

	End Sub


#End Region

#Region "Event handler"

	Private Sub OnMessageSent(ByVal message As ShortMessage) Handles m_frmSMS2eCallWait.MessageSent
		If message.ReceiverId = 0 Then
			lstVersandResult.Items.Add("[success] Test SMS to  [" + message.Address + "]")
			Return
		End If
		Dim receiverList As BindingList(Of ReceiverView) = grdMain.DataSource
		Dim sentList = (From r In receiverList Where r.Id = message.ReceiverId).ToList()
		For Each receiver In sentList
			If message.ResponseCode = 0 Or message.ResponseCode = 11912 Or message.ResponseCode = 11913 Then
				lstVersandResult.Items.Add(String.Format("[success] SMS to  [{0}]  - {1}; {2}", receiver.Natel, receiver.Name, receiver.Adresse))
				receiver.IsSelected = False
				grdMain.RefreshDataSource()
				m_sendCount += 1
				bsiInfo.Caption = String.Format(m_translate.GetSafeTranslationValue("{0} Einträge wurden erfolgreich gesendet."), m_sendCount)
				ContactLog(message, receiver)
			Else
				lstVersandResult.Items.Add(String.Format("[failure][{3}] SMS to  [{0}]  - {1}; {2} : {4}", receiver.Natel, receiver.Name, receiver.Adresse, message.ResponseCode.ToString, message.ResponseText))
			End If
		Next
	End Sub

	Private Sub OnSendFinished(ByVal result As String) Handles m_frmSMS2eCallWait.SendFinished
		Me.Enabled = True
		DevExpress.XtraEditors.XtraMessageBox.Show(result, "Send SMS", MessageBoxButtons.OK, MessageBoxIcon.Information)
		If m_frmSMS2eCallWait IsNot Nothing Then
			m_frmSMS2eCallWait.Dispose()
		End If
		If lstVersandResult.Items.Count > 0 Then
			Me.XtraTabControl1.SelectedTabPage = Me.xtabResultData
			' write lstVersandResult to file
			Dim fileName As String = Path.Combine(m_deleteHomeFolder, DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_SMS.txt")
			Dim lines As List(Of String) = New List(Of String)
			lines.Add("-----------------------------------")
			lines.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
			lines.Add("SMS Vorlage: " + meMessageText.EditValue)
			lines.Add("-----------------------------------")
			For Each line In lstVersandResult.Items
				lines.Add(line.ToString())
			Next
			File.WriteAllLines(fileName, lines.ToArray())
		End If
	End Sub

	''' <summary>
	''' Loads and initializes the form
	''' </summary>
	Private Sub OnFrmLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ModulConstants.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.UserLookAndFeel {1}", strMethodeName, ex.Message))

		End Try

		TranslateControls()
		Try
			Me.Width = Math.Max(My.Settings.ifrmSMSWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmSMSHeight, Me.Height)

			If My.Settings.frm_SMS_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_SMS_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			'InitForm()


			'AddHandler gvMain.RowCellClick, AddressOf Ongv_RowCellClick

		Catch ex As Exception

		End Try

	End Sub

	''' <summary>
	''' Disposes thr from
	''' </summary>
	Private Sub OnFrmDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_SMS_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmSMSHeight = Me.Height
				My.Settings.ifrmSMSWidth = Me.Width

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	''' <summary>
	''' Translates and displays the selected template
	''' </summary>
	Private Sub OnReceiverFocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvMain.FocusedRowChanged
		If Not m_actualTemplate Is Nothing Then
			If Not SelectedReceiver Is Nothing Then
				m_actualTemplate.EmployeeLanguage = LoadReceiverLanguage(SelectedReceiver.MANr, SelectedReceiver.KDNr)
				m_isTemplateShowing = False
				meMessageTextExample.Text = m_actualTemplate.GetMessage(SelectedReceiver)
			Else
				meMessageTextExample.Text = m_actualTemplate.Bez
				m_isTemplateShowing = True
			End If
		End If
	End Sub

	''' <summary>
	''' Activates the selected template
	''' </summary>
	Private Sub OnTemplatesEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueTemplates.EditValueChanged
		Dim selectedId = lueTemplates.EditValue
		If selectedId Is Nothing Then
			If Not String.IsNullOrWhiteSpace(meMessageTextExample.EditValue) Then meMessageText.EditValue = meMessageTextExample.EditValue
			Return
		End If

		Dim template As Template = (From t In m_templates Where t.Id = selectedId).FirstOrDefault()

		If template IsNot Nothing Then
			m_actualTemplate = New Template(ModulConstants.UserData.UserLanguage) With {
				.Id = 0,
				.BezValue = template.BezValue,
				.Bez_D = template.Bez_D,
				.Bez_I = template.Bez_I,
				.Bez_F = template.Bez_F,
				.Bez_E = template.Bez_E
			 }
			meMessageText.EditValue = m_actualTemplate.Bez
			m_isTemplateShowing = True
		Else
			meMessageText.EditValue = String.Empty
		End If

		meMessageTextExample.EditValue = m_actualTemplate.GetMessage(SelectedReceiver)

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
			ElseIf TypeOf sender Is DateEdit Then
				Dim dateEdit As DateEdit = CType(sender, DateEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
	End Sub

	''' <summary>
	''' Displays the selected template
	''' </summary>
	Private Sub OnMessageTextEnter(sender As System.Object, e As System.EventArgs) Handles meMessageText.Enter
		If m_actualTemplate IsNot Nothing Then
			gvMain.ClearSelection()
			meMessageText.EditValue = m_actualTemplate.Bez
			m_isTemplateShowing = True
		Else
			'meMessageText.EditValue = String.Empty
		End If
	End Sub

	''' <summary>
	''' Updates the selected template
	''' </summary>
	Private Sub OnMessageTextEditValueChanged(sender As System.Object, e As System.EventArgs) Handles meMessageText.EditValueChanged
		If m_isTemplateShowing Then
			If m_actualTemplate IsNot Nothing Then
				m_actualTemplate.Bez = meMessageText.EditValue
				meMessageTextExample.Text = m_actualTemplate.GetMessage(SelectedReceiver)
			Else
				'meMessageText.EditValue = String.Empty
			End If
		End If
	End Sub

	Private Sub OntxtMobilePhone_ButtonClick(sender As System.Object, e As System.EventArgs) Handles txtMobilePhone.ButtonClick
		Dim testNumber = Me.txtMobilePhone.Text

		If Not String.IsNullOrEmpty(testNumber) Then
			SendSmsToTestNumer(testNumber)
		End If

	End Sub

	Private Sub OnButtonSendSmsClick(sender As System.Object, e As System.EventArgs) Handles btnSend.Click
		Dim bindingList As BindingList(Of ReceiverView) = grdMain.DataSource
		Dim countSMS = (From receiver In bindingList Where receiver.IsSelected = True).Count()

		If m_actualTemplate Is Nothing Then m_actualTemplate = New Template("D") With {.Bez = meMessageText.EditValue}

		If countSMS = 0 Then
			DevExpress.XtraEditors.XtraMessageBox.Show(m_translate.GetSafeTranslationValue("Bitte wählen Sie Empfänger aus."),
																								 m_translate.GetSafeTranslationValue("SMS-Nachricht senden"),
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		'If m_actualTemplate Is Nothing AndAlso meMessageText.Text.Length < 5 Then
		If meMessageText.EditValue.Length < 5 Then
			DevExpress.XtraEditors.XtraMessageBox.Show(m_translate.GetSafeTranslationValue("Bitte wählen Sie eine Vorlage aus oder tragen Sie Ihre Nachricht ein."),
																								 m_translate.GetSafeTranslationValue("SMS-Nachricht senden"),
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		If String.IsNullOrWhiteSpace(cboAnswerAddress.Text) OrElse cboAnswerAddress.Text.Contains("@") Then
			DevExpress.XtraEditors.XtraMessageBox.Show(m_translate.GetSafeTranslationValue("Bitte tragen Sie eine gültige (Mobile oder Festnetz)-Telefonnummer als Antwortadresse ein."),
																								 m_translate.GetSafeTranslationValue("SMS-Nachricht senden"),
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If

		Dim question As String = String.Format(m_translate.GetSafeTranslationValue("Wollen Sie {1} SMS versenden?{0}Ihre Nachricht lautet:{0}{0}{2}"),
																					 vbNewLine, countSMS, m_actualTemplate.GetMessage(bindingList(0)))

		Dim dialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(question, m_translate.GetSafeTranslationValue("SMS-Nachricht senden"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)
		If (dialogResult = DialogResult.Yes) Then
			m_Logger.LogInfo(String.Format("countSMS: {0}", countSMS))
			SendSmsToSelectetReceivers()
		End If

	End Sub

	Private Sub OnButtonCloseClick(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub

	Private Sub tgsSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSelection.Toggled
		SelDeSelectItems(tgsSelection.EditValue)
	End Sub

	Private Sub SelDeSelectItems(ByVal selectItem As Boolean)
		Dim data As BindingList(Of ReceiverView) = grdMain.DataSource

		If Not data Is Nothing Then
			For Each item In data
				item.IsSelected = selectItem
			Next
		End If

		gvMain.RefreshData()

	End Sub

#End Region

#Region "PrivateClasses"

	Private Class ReceiverView

		Public Property IsSelected As Boolean
			Get
				Return m_data.IsSelected
			End Get
			Set(value As Boolean)
				m_data.IsSelected = value
			End Set
		End Property

		Public ReadOnly Property Id As Integer
			Get
				If MANr = 0 Then
					Return KDNr
				Else
					Return MANr
				End If
			End Get
		End Property
		Public ReadOnly Property MANr As Integer
			Get
				Return m_data.MANr
			End Get
		End Property

		Public ReadOnly Property KDNr As Integer
			Get
				Return m_data.KDNr
			End Get
		End Property

		Public ReadOnly Property ZHDRecNr As Integer?
			Get
				Return m_data.ZHDRecNr
			End Get
		End Property

		Public ReadOnly Property Firma As String
			Get
				Return m_data.Firma
			End Get
		End Property

		Public ReadOnly Property AnredeForm As String
			Get
				Return m_data.AnredeForm
			End Get
		End Property

		Public ReadOnly Property Vorname As String
			Get
				Return m_data.Vorname
			End Get
		End Property

		Public ReadOnly Property Nachname As String
			Get
				Return m_data.Nachname
			End Get
		End Property

		Public ReadOnly Property Name As String
			Get
				Return m_data.Name
			End Get
		End Property

		Public ReadOnly Property Adresse As String
			Get
				Return m_data.Adresse
			End Get
		End Property

		Public ReadOnly Property Natel As String
			Get
				Return m_data.Natel
			End Get
		End Property

		Private m_data As ReceiverViewData

		Public Sub New(ByVal data As ReceiverViewData)
			m_data = data
		End Sub
	End Class

	Private Class ReceiverViewData
		Public Property IsSelected As Boolean
		Public Property MANr As Integer
		Public Property KDNr As Integer
		Public Property ZHDRecNr As Integer?

		Public Property Firma As String
		Public Property AnredeForm As String
		Public Property Vorname As String
		Public Property Nachname As String
		Public Property Strasse As String
		Public Property Land As String
		Public Property PLZ As String
		Public Property Ort As String
		Public Property Natel As String
		Public Property ReceiverLanguage As String

		Public ReadOnly Property Name As String
			Get
				Return Nachname & ", " & Vorname
			End Get
		End Property

		Public ReadOnly Property Adresse As String
			Get
				Return String.Format("{0}, {1}-{2} {3}", Strasse, Land, PLZ, Ort)
			End Get
		End Property

	End Class

	Private Class Template

		Public Property Id As Integer
		Public Property BezValue As String
		Public Property Bez_D As String
		Public Property Bez_I As String
		Public Property Bez_F As String
		Public Property Bez_E As String
		Public Property EmployeeLanguage As String

		Public Property Bez As String
			Get
				Dim bezLang As String
				Select Case EmployeeLanguage
					Case "D"
						bezLang = Bez_D
					Case "I"
						bezLang = Bez_I
					Case "F"
						bezLang = Bez_F
					Case "E"
						bezLang = Bez_E
					Case Else
						bezLang = Bez_D
				End Select

				Return bezLang
			End Get
			Set(value As String)
				Select Case EmployeeLanguage
					Case "D"
						Bez_D = value
					Case "I"
						Bez_I = value
					Case "F"
						Bez_F = value
					Case "E"
						Bez_E = value
					Case Else
						Bez_D = value
				End Select
			End Set
		End Property


		Public Sub New(ByVal lang As String)
			EmployeeLanguage = lang
		End Sub

		Public Function GetMessage(ByVal receiver As ReceiverView) As String
			Dim message = Bez

			' Search {#...}
			Dim regex As Regex = New Regex("\{#(\w+)\}", RegexOptions.Multiline)
			Dim matches As MatchCollection = regex.Matches(message)

			For Each match As Match In matches

				Dim pattern As String = match.Groups(0).Value
				Dim wildcard As String = match.Groups(1).Value

				Dim m = (From prop In GetType(ReceiverView).GetProperties()
						 Where prop.Name = wildcard).FirstOrDefault()
				If (m IsNot Nothing) Then
					message = message.Replace(pattern, m.GetValue(receiver, Nothing))
				End If

			Next

			' Search {@...}
			regex = New Regex("\{@(\w+)\}", RegexOptions.Multiline)
			matches = regex.Matches(message)

			For Each match As Match In matches
				Dim pattern As String = match.Groups(0).Value
				Dim wildcard As String = match.Groups(1).Value

				Select Case wildcard
					Case "MDName"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDName)
					Case "MDStrasse"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDStrasse)
					Case "MDTelefon"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDTelefon)
					Case "MDDTelefon"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDDTelefon)
					Case "MDEMail"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDeMail)
					Case "MDHomepage"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDHomepage)
					Case "MDOrt"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDOrt)
					Case "MDLand"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDLand)
					Case "MDPLZ"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDPLZ)
					Case "MDPostfach"
						message = message.Replace(pattern, ModulConstants.UserData.UserMDPostfach)
					Case "UserFullname"
						message = message.Replace(pattern, ModulConstants.UserData.UserFullName)
					Case "UserFTitel"
						message = message.Replace(pattern, ModulConstants.UserData.UserFTitel)
					Case "UserSTitel"
						message = message.Replace(pattern, ModulConstants.UserData.UserSTitel)
					Case "UserNatel", "UserMobile"
						message = message.Replace(pattern, ModulConstants.UserData.UserMobile)

				End Select

			Next

			Return message
		End Function

	End Class

#End Region


End Class
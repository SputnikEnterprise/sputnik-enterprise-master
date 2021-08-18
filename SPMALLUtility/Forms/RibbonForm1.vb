
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports System.IO

Imports System.Data.SqlClient
Imports System.Drawing.Imaging

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Services

Imports DevExpress.XtraRichEdit.Commands.Internal
Imports DevExpress.XtraRichEdit.Commands
Imports DevExpress.XtraBars.Alerter

Imports SPProgUtility.Mandanten
Imports DevExpress.XtraSplashScreen
Imports DevExpress.Pdf
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee

Imports SP.Internal.Automations
Imports SP.Internal.Automations.BaseTable
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports System.ComponentModel

Public Class RibbonForm1



#Region "private consts"

  Private Const LL_DBFIELD_NAME As String = "Reserve0"

#End Region


#Region "private fields"

  ''' <summary>
  ''' The Initialization data.
  ''' </summary>
  Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger = New Logger()

  ''' <summary>
  ''' The translation value helper.
  ''' </summary>
  Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

  ''' <summary>
  ''' The common database access.
  ''' </summary>
  Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
  Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

  Private m_Utility As SP.Infrastructure.Utility

  Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Private m_mandant As Mandant


  Private m_UtilityProg As SPProgUtility.MainUtilities.Utilities
  Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

  Private strExportFilename As String

  Private m_EmployeeMaskData As EmployeeMaskData
  Private m_TemplateName As String = "Standard"
	Private m_CurrentCVDocumentID As Integer?

	Private m_customerinfoinTextbox As Boolean

  Private m_employeePictureinTextbox As Boolean
  Private m_advisorPictureinTextbox As Boolean
  Private m_advisorSigninTextbox As Boolean
  Private m_resizeemployeePictureinTextbox As Boolean
  Private m_resizeadvisorPictureinTextbox As Boolean
  Private m_resizeadvisorSigninTextbox As Boolean

  Private m_coversheetdiplomascheckboxvalue As Boolean
  Private m_coversheetconditionscheckboxvalue As Boolean
  Private m_agbrentalcheckboxvalue As Boolean
  Private m_agbpermanentcheckboxvalue As Boolean

  Private m_AGBTemplateFilename As String
  Private m_AGBScanFilename As String
  Private m_agbinTextbox As Boolean
  Private m_resizeagbintextbox As Boolean

  Private direction As SearchDirection = SearchDirection.Forward
	Private m_TemplatePath As String
	Private m_CVTemplatePath As String
	Private m_TemplateFile As String
  Private strSearchValue As String = String.Empty

  Private m_MergePDFFilename As String = String.Empty
  Private f As AlertForm = Nothing

  Private m_MandantXMLFile As String
  Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

  Private m_LoadDataUtility As LoadData

  Private m_EmployeeMasterData As EmployeeMasterData


#End Region


#Region "private properties"

  Private ReadOnly Property GetLLExtentionValue() As String
    Get
      Return _ClsProgSetting.GetMDProfilValue("Sonstiges", "LLTemplateExtension", "DOC")
    End Get
  End Property

  Private ReadOnly Property GetLLDocFormat() As DevExpress.XtraRichEdit.DocumentFormat
    Get
      Dim strValue As String = GetLLExtentionValue()

      Select Case strValue.ToLower
        Case "doc"
          Return DevExpress.XtraRichEdit.DocumentFormat.Doc

        Case "odt"
          Return DevExpress.XtraRichEdit.DocumentFormat.OpenDocument

        Case "rtf"
          Return DevExpress.XtraRichEdit.DocumentFormat.Rtf

        Case "txt"
          Return DevExpress.XtraRichEdit.DocumentFormat.PlainText

        Case Else
          Return DevExpress.XtraRichEdit.DocumentFormat.Doc

      End Select

    End Get
  End Property

#End Region


#Region "public properties"

  Public Property EmployeeNumber As Integer
  Public Property CVTemplateLabel As String

#End Region


#Region "Constructor"

  Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    m_InitializationData = _setting
    m_UtilityUi = New UtilityUI
    m_Utility = New SP.Infrastructure.Utility
    m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
    m_mandant = New Mandant
    m_UtilityProg = New SPProgUtility.MainUtilities.Utilities

    m_LoadDataUtility = New LoadData(m_InitializationData)

    m_CommonDatabaseAccess = New CommonDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)
    m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)


    InitializeComponent()

    Try
      Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
      If strStyleName <> String.Empty Then
        UserLookAndFeel.Default.SetSkinStyle(strStyleName)
      End If

    Catch ex As Exception

    End Try

    rtfContent.Dock = DockStyle.Fill
    Me.iLayout.Visibility = If(_ClsProgSetting.GetLogedUSNr = 1,
                              DevExpress.XtraBars.BarItemVisibility.Always,
                              DevExpress.XtraBars.BarItemVisibility.Never)

    'ReplaceRichEditCommandFactoryService(Me.rtfContent)

    'Me.rtfContent.RemoveShortcutKey(Keys.Control, Keys.O)
    'rtfContent.RemoveShortcutKey(Keys.Control, Keys.N)
    'rtfContent.RemoveShortcutKey(Keys.Control, Keys.S)

    m_TemplatePath = m_mandant.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr)
		m_CVTemplatePath = m_mandant.GetSelectedMDTemplatePath(m_InitializationData.MDData.MDNr)


		Me.KeyPreview = True
		TranslateControls()

		ListTemplates()
    AddHandler beTemplate.EditValueChanged, AddressOf beScheme_EditValueChanged

  End Sub


#End Region


#Region "public methodes"

  Public Sub LoadCVTemplate()

		Dim templateData As List(Of LLTemplateData)
		templateData = LoadLLTemplateName("LLFields")
		If String.IsNullOrWhiteSpace(CVTemplateLabel) Then CVTemplateLabel = "Standard"

		m_TemplateName = CVTemplateLabel
    m_EmployeeMaskData = m_LoadDataUtility.LoadEmployeeData(EmployeeNumber)
    If Not LoadEmployeeMasterData() Then Return

    ReplaceRichEditCommandFactoryService(Me.rtfContent)
    Me.rtfContent.RemoveShortcutKey(Keys.Control, Keys.O)
    rtfContent.RemoveShortcutKey(Keys.Control, Keys.N)
    rtfContent.RemoveShortcutKey(Keys.Control, Keys.S)

    RemoveHandler beTemplate.EditValueChanged, AddressOf beScheme_EditValueChanged

		If templateData.Count > 0 Then

			If String.IsNullOrWhiteSpace(m_TemplateName) Then
				Me.beTemplate.EditValue = New myCombobox(m_Translate.GetSafeTranslationValue(templateData(0).Bezeichnung), templateData(0).FileName)

			Else

				Dim template = templateData.FirstOrDefault(Function(x) x.FileName = m_TemplateName)
				If Not template Is Nothing Then
					Me.beTemplate.EditValue = New myCombobox(m_Translate.GetSafeTranslationValue(template.Bezeichnung), template.FileName)
				End If
			End If

		End If

		AddHandler beTemplate.EditValueChanged, AddressOf beScheme_EditValueChanged

    InitialLLData()

  End Sub

#End Region

#Region "Private Methodes"

  Private Function LoadEmployeeMasterData() As Boolean

    m_EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(EmployeeNumber, True)

    If m_EmployeeMasterData Is Nothing Then
      m_UtilityUi.ShowErrorDialog("Kandidaten Daten konnten nicht geladen werden.")

      Return False
    End If


    Return Not (m_EmployeeMasterData Is Nothing)

  End Function

  Private Sub ReplaceRichEditCommandFactoryService(ByVal control As DevExpress.XtraRichEdit.RichEditControl)

		Try

			Dim service As IRichEditCommandFactoryService = control.GetService(Of IRichEditCommandFactoryService)()

			If service Is Nothing Then Return
			control.RemoveService(GetType(IRichEditCommandFactoryService))
			control.AddService(GetType(IRichEditCommandFactoryService), New CustomCommand.CustomRichEditCommandFactoryService(m_InitializationData, control, service, EmployeeNumber))

		Catch ex As Exception

		End Try

	End Sub

#Region "#substservice"


#End Region


#End Region




#Region "Hilfsfunktionen..."

  Sub GetMenuItem(ByVal sender As Object, ByVal e As EventArgs)
    Dim strMnuName As String = sender.name.ToString
    Dim strMnuLbl As String = sender.ToString
    Dim orgData = Clipboard.GetDataObject

    If Val(sender.text) > 0 Then GetMADoc(Val(sender.text))
    Clipboard.SetDataObject(orgData)

  End Sub

  Sub InitialLLData()

		Try
			strExportFilename = Path.Combine(_ClsProgSetting.GetSpSFiles2DeletePath, String.Format("Lebenslauf_{0}_{1}.{2}", m_TemplateName, EmployeeNumber, GetLLExtentionValue))

			ShowLLText(LL_DBFIELD_NAME)
			BuildLLFields4Docs(Me.rep_ScanDoc)

			ClsDataDetail.bContentChanged = False
			ClsDataDetail.GetSelectedTemplateName = m_TemplateName
			m_MergePDFFilename = Path.Combine(_ClsProgSetting.GetSpSFiles2DeletePath, String.Format("Lebenslauf_{0}.pdf", EmployeeNumber))

			Me.Text = String.Format(m_Translate.GetSafeTranslationValue("Lebenslauf für {0}{1} {2} {3}"), m_EmployeeMaskData.EmployeeAnrede, If(m_EmployeeMaskData.EmployeeAnrede = "Herr", "n", ""), m_EmployeeMaskData.MAVorname, m_EmployeeMaskData.MANachname)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

    End Try

  End Sub

  Sub BuildLLFields4Docs(ByVal cbo As DevExpress.XtraEditors.Repository.RepositoryItemComboBox)
    Dim LoUSKstBez As New List(Of String)
    Dim LoUSNameBez As New List(Of String)
    Dim Time_1 As Double = System.Environment.TickCount

    Try
      Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
      Dim i As Integer = 0

      cbo.Items.Clear()
			Dim sSql As String = "Select ID, isnull(Bezeichnung, '') Bezeichnung From MA_LLDoc Where MANr = @MANr And "
			sSql &= "ISNULL(ScanExtension, '') Not In ('.doc', 'doc', '.xls', 'xls', '') "
			sSql &= "AND ISNULL(Bezeichnung, '') <> '' "
			sSql &= "Order By Bezeichnung"
			Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MANr", EmployeeNumber)
      Dim rUSrec As SqlDataReader = cmd.ExecuteReader
      While rUSrec.Read
				cbo.Items.Add(String.Format("{0} - {1}", rUSrec("ID").ToString.Trim,
									m_Translate.GetSafeTranslationValue(m_UtilityProg.SafeGetString(rUSrec, "Bezeichnung"))))
				i += 1
      End While
      AddHandler cbo.SelectedValueChanged, AddressOf Me.GetMenuItem


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      m_UtilityUi.ShowErrorDialog(ex.ToString)

    End Try

  End Sub

  'Private Sub Check4DbRecExists(ByVal strLLName As String)
  '	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
  '	Dim sSql As String = "If Not Exists(Select ID From MA_Lebenslauf Where LL_Name = @RecName And MANr = @MANr) "
  '	sSql &= "Update MA_Lebenslauf Set LL_Name = @RecName Where MANr = @MANr And LL_Name Is Null"

  '	Conn.Open()

  '	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
  '	cmd.CommandType = Data.CommandType.Text
  '	Dim param As System.Data.SqlClient.SqlParameter

  '	param = cmd.Parameters.AddWithValue("@MANr", EmployeeNumber)
  '	param = cmd.Parameters.AddWithValue("@RecName", strLLName)

  '	cmd.ExecuteNonQuery()

  'End Sub


  Private Sub InsertAdditionalPage()

    Try
      ' Funktionen für die neue Seiten ohne Header...
      rtfContent.Document.AppendSection()

      rtfContent.Document.CaretPosition = rtfContent.Document.Range.End

      Dim eph As New EditPageHeaderCommand(rtfContent)
      Dim thfltp As New ToggleHeaderFooterLinkToPreviousCoreCommand(rtfContent)
      Dim cphf As New ClosePageHeaderFooterCommand(rtfContent)

      eph.Execute()
      thfltp.Execute()
      cphf.Execute()

      Dim doc As SubDocument = rtfContent.Document.Sections(1).BeginUpdateHeader()
      doc.Delete(doc.Range)
      rtfContent.Document.Sections(1).EndUpdateHeader(doc)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    End Try

  End Sub

  Sub ShowLLText(ByVal strFieldName As String)

    If String.IsNullOrWhiteSpace(m_TemplateName) Then
      m_Logger.LogError("no cv template was selected to show!")

      Dim msg As String = m_Translate.GetSafeTranslationValue("Achtung: Es wurde keine Vorlage ausgewählt.")
      m_UtilityUi.ShowErrorDialog(msg, m_Translate.GetSafeTranslationValue("Dokument anzeigen"), MessageBoxIcon.Error)

      Return
    End If

		Try
			bbiDeleteCV.Enabled = False
			m_CurrentCVDocumentID = Nothing
			Me.rtfContent.RtfText = String.Empty

			Dim data = m_EmployeeDatabaseAccess.LoadAssingedEmployeeCVData(EmployeeNumber, m_TemplateName, True)
			If data Is Nothing Then Return

			'Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

			'Dim sql As String
			'sql = String.Format("Select Top 1 ID, IsNull(_{0}, '') As _{0}, IsNull({0}, '') As {0}, DocumentContent ", strFieldName)
			'sql &= "From MA_Lebenslauf Where MANr = @MANr And (LL_Name = @TemplateName)"

			'Dim listOfParams As New List(Of SqlClient.SqlParameter)
			'listOfParams.Add(New SqlClient.SqlParameter("MANr", EmployeeNumber))
			'listOfParams.Add(New SqlClient.SqlParameter("TemplateName", m_TemplateName))
			'Dim reader As SqlClient.SqlDataReader = m_UtilityProg.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams)



			Dim content = data.DocumentContent ' m_UtilityProg.SafeGetByteArray(reader, "DocumentContent")
			Dim fieldname As String = data.ReserveRTFContent ' m_UtilityProg.SafeGetString(reader, "_" & strFieldName)

			If Not content Is Nothing Then
				Dim fileExtension = GetLLExtentionValue
				Dim tmpFilename As String = Path.GetTempFileName
				tmpFilename = Path.ChangeExtension(tmpFilename, fileExtension)

				Dim success As Boolean = m_Utility.WriteFileBytes(tmpFilename, content)
				If success Then
					Me.rtfContent.LoadDocument(tmpFilename, GetLLDocFormat)
					m_CurrentCVDocumentID = data.ID '  m_UtilityProg.SafeGetInteger(reader, "ID", 0)
					SetTemplateValues()
					Me.rtfContent.Modified = False
				End If

			Else
				If String.IsNullOrEmpty(fieldname) Then
					Me.rtfContent.Text = data.ReserveTextContent '  m_UtilityProg.SafeGetString(reader, strFieldName)
				Else
					Me.rtfContent.RtfText = fieldname
				End If

			End If
			bbiDeleteCV.Enabled = Not m_CurrentCVDocumentID Is Nothing


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(String.Format("(ShowLLText): {0}", ex.ToString))

		Finally

		End Try

  End Sub

#End Region

  Private Sub RibbonForm1_FormClosing(ByVal sender As Object,
                                      ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    pcAttachments.HidePopup()

    Try
      My.Settings.iHeight = Me.Height
      My.Settings.iWidth = Me.Width
      My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

      My.Settings.frmLayoutName = DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName
      My.Settings.Save()

    Catch ex As Exception

    End Try
    e.Cancel = ContinueChanging() = MsgBoxResult.Cancel

  End Sub

  Private Sub RibbonForm1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp

    If e.Control And e.KeyCode = Keys.S Then m_LoadDataUtility.SaveLLText("Reserve0", ClsDataDetail.GetSelectedTemplateName, EmployeeNumber, Me.rtfContent)

  End Sub

  Sub ListTemplates()

		Dim templateData As List(Of LLTemplateData)
		Dim listAsTemplate As Boolean = True

		templateData = LoadLLTemplateName("LLFields")

		For Each tpl In templateData
			Dim tplFilename As String = String.Format("LL_{0}_{1}.{2}", tpl.FileName, LL_DBFIELD_NAME, GetLLExtentionValue)

			'Dim templateFile = FindTemplateFile(tplFilename)
			Dim templateFile = FindExtendedTemplateFile("cvtemplate", tplFilename)
			listAsTemplate = Not String.IsNullOrWhiteSpace(templateFile)
			If listAsTemplate Then repTemplates.Items.Add(New myCombobox(m_Translate.GetSafeTranslationValue(tpl.Bezeichnung), m_Translate.GetSafeTranslationValue(tpl.FileName)))

		Next
		Me.repTemplates.Enabled = Not templateData Is Nothing AndAlso templateData.Count > 0

	End Sub

	Private Function FindTemplateFile(ByVal tplFilename As String) As String
		Dim result As Boolean = True

		Dim templateFile = Path.Combine(m_CVTemplatePath, tplFilename)
		If Not File.Exists(templateFile) Then
			result = False
			templateFile = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "CV Templates", tplFilename)

			If File.Exists(templateFile) Then
				result = True
				m_CVTemplatePath = Path.Combine(m_InitializationData.MDData.MDTemplatePath, "CV Templates")
			End If
		End If
		If Not result Then templateFile = String.Empty

		Return templateFile
	End Function

	Private Function FindExtendedTemplateFile(ByVal modulArt As String, ByVal tplFilename As String) As String
		Dim result As Boolean = True
		Dim additionalPath As String = "employee"
		Dim templatePath As String = m_InitializationData.MDData.MDTemplatePath
		Dim templateFile = Path.Combine(templatePath, tplFilename)

		Select Case modulArt.ToLower
			Case "employee"
				additionalPath = "employee"

			Case "propose"
				additionalPath = "propose"

			Case "cvtemplate"
				additionalPath = "CV Templates"

			Case Else
				Return String.Empty

		End Select

		If Not File.Exists(templateFile) Then
			result = False
			templateFile = Path.Combine(templatePath, additionalPath, tplFilename)

			If File.Exists(templateFile) Then
				result = True
				templatePath = Path.Combine(templatePath, additionalPath)
			End If

		End If
		m_CVTemplatePath = templatePath

		If Not result Then templateFile = String.Empty

		Return templateFile
	End Function

	Sub TranslateControls()

		Try
			Me.beTemplate.Caption = m_Translate.GetSafeTranslationValue(Me.beTemplate.Caption)
			Me.beTemplate.Description = m_Translate.GetSafeTranslationValue(Me.beTemplate.Description)
      Me.bgScannDocs.Text = m_Translate.GetSafeTranslationValue(Me.bgScannDocs.Text)
      Me.bgAbsatz.Text = m_Translate.GetSafeTranslationValue(Me.bgAbsatz.Text)
      Me.bgEdit.Text = m_Translate.GetSafeTranslationValue(Me.bgEdit.Text)
      Me.bgAbsatz.Text = m_Translate.GetSafeTranslationValue(Me.bgAbsatz.Text)
      Me.bgZwischenablage.Text = m_Translate.GetSafeTranslationValue(Me.bgZwischenablage.Text)
      Me.bgFont.Text = m_Translate.GetSafeTranslationValue(Me.bgFont.Text)

      Me.bgDocViewPage1.Text = m_Translate.GetSafeTranslationValue(Me.bgDocViewPage1.Text)
      Me.bgDocViewPage2.Text = m_Translate.GetSafeTranslationValue(Me.bgDocViewPage2.Text)
      Me.bgDocViewPage3.Text = m_Translate.GetSafeTranslationValue(Me.bgDocViewPage3.Text)
      Me.bgDoclayout.Text = m_Translate.GetSafeTranslationValue(Me.bgDoclayout.Text)

      Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)


    Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

  End Sub


  Private Sub RibbonForm1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try
      If My.Settings.iHeight > 0 Then Me.Height = My.Settings.iHeight
      If My.Settings.iWidth > 0 Then Me.Width = My.Settings.iWidth
      If My.Settings.frmLocation <> String.Empty Then
        Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
        If Screen.AllScreens.Length = 1 Then
          If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
        End If
        Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    End Try

  End Sub

  Private Sub beScheme_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles beTemplate.EditValueChanged
    Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
    Dim sSql As String = "If Not Exists(Select ID From MA_Lebenslauf Where LL_Name = @RecName And MANr = @MANr) "
    sSql &= "Insert Into MA_Lebenslauf (MANr, LL_Name, CreatedOn, CreatedFrom) Values (@MANr, @RecName, GetDate(), @UserName)"
    Conn.Open()

    SetTemplateName()

    ClsDataDetail.GetSelectedTemplateName = m_TemplateName
    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@MANr", EmployeeNumber)
      param = cmd.Parameters.AddWithValue("@RecName", m_TemplateName)
      param = cmd.Parameters.AddWithValue("@UserName", m_InitializationData.UserData.UserFullNameWithComma)

      cmd.ExecuteNonQuery()

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("(beTemplate): {0}", ex.Message),
                                         m_Translate.GetSafeTranslationValue("Lebenslaufarten anzeigen"),
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error)

    End Try

		Try
			m_CurrentCVDocumentID = Nothing
			If Not Me.beTemplate.EditValue Is Nothing Then
				InitialLLData()
			End If

		Catch ex As Exception
			InitialLLData()

    End Try


  End Sub

  Private Sub SetTemplateName()
    Try
      If (m_TemplateName Is Nothing AndAlso beTemplate Is Nothing) OrElse String.IsNullOrWhiteSpace(m_TemplateName) Then
        m_TemplateName = m_Translate.GetSafeTranslationValue("Standard")
      Else
        m_TemplateName = DirectCast(Me.beTemplate.EditValue, myCombobox)._Value
      End If

      'Catch ex As InvalidCastException
      '	m_Translate.GetSafeTranslationValue("Standard")
    Catch ex As Exception
      m_TemplateName = Me.beTemplate.EditValue.ToString
    End Try

  End Sub

  Private Sub beRunMacro_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles beRunMacro.ItemClick
    pcAttachments.HidePopup()

		If beTemplate.EditValue Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Vorlage wurde ausgewählt."))

			Return
		End If

		SetTemplateName()
    If ContinueChanging() = MsgBoxResult.Cancel Then Exit Sub
    ClsDataDetail.GetSelectedTemplateName = m_TemplateName

		Dim agbTemplateFilename As String = String.Empty
		Dim agbScanFilename As String = String.Empty

		m_TemplateFile = Path.Combine(m_CVTemplatePath, String.Format("LL_{0}_{1}.{2}", m_TemplateName, LL_DBFIELD_NAME, GetLLExtentionValue))
		Dim strNewTemplateFile As String = Path.Combine(_ClsProgSetting.GetSpSFiles2DeletePath, String.Format("LL_{0}_{1}.{2}", m_TemplateName, LL_DBFIELD_NAME, GetLLExtentionValue))

		m_customerinfoinTextbox = False
    m_employeePictureinTextbox = False
    m_advisorPictureinTextbox = False
    m_advisorSigninTextbox = False

    m_resizeemployeePictureinTextbox = False
    m_resizeadvisorPictureinTextbox = False
    m_resizeadvisorSigninTextbox = False

    m_coversheetdiplomascheckboxvalue = False
    m_coversheetconditionscheckboxvalue = False

    m_agbrentalcheckboxvalue = False
    m_agbpermanentcheckboxvalue = False

    m_agbinTextbox = False
    m_resizeagbintextbox = False

    If File.Exists(m_TemplateFile) Then
      File.Copy(m_TemplateFile, strNewTemplateFile, True)
      Me.rtfContent.LoadDocument(strNewTemplateFile, GetLLDocFormat)
      Me.rtfContent.Modified = True

      SetTemplateValues()
      InsertDataIntoDoc()

    Else
      m_UtilityUi.ShowErrorDialog(String.Format("Die Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_TemplateFile))
      m_Logger.LogError(String.Format("Die Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_TemplateFile))

    End If

  End Sub

  Private Sub SetTemplateValues()

		m_TemplateFile = Path.Combine(m_CVTemplatePath, String.Format("LL_{0}_{1}.{2}", m_TemplateName, LL_DBFIELD_NAME, GetLLExtentionValue))
		Dim strNewTemplateFile As String = Path.Combine(_ClsProgSetting.GetSpSFiles2DeletePath, String.Format("LL_{0}_{1}.{2}", m_TemplateName, LL_DBFIELD_NAME, GetLLExtentionValue))
    Dim agbTemplateFilename As String = String.Empty
    Dim agbScanFilename As String = String.Empty

    m_customerinfoinTextbox = False
    m_employeePictureinTextbox = False
    m_advisorPictureinTextbox = False
    m_advisorSigninTextbox = False

    m_resizeemployeePictureinTextbox = False
    m_resizeadvisorPictureinTextbox = False
    m_resizeadvisorSigninTextbox = False

    m_coversheetdiplomascheckboxvalue = False
    m_coversheetconditionscheckboxvalue = False

    m_agbrentalcheckboxvalue = False
    m_agbpermanentcheckboxvalue = False

    m_agbinTextbox = False
    m_resizeagbintextbox = False

    If File.Exists(m_TemplateFile) Then

      m_MandantXMLFile = Path.Combine(m_TemplatePath, Path.ChangeExtension(m_TemplateFile, "xml"))
      If Not System.IO.File.Exists(m_MandantXMLFile) Then
        m_Logger.LogWarning(String.Format("CV XML-File was not founded! {0}", m_MandantXMLFile))
      Else
        m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)

        m_customerinfoinTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/customerinfointextbox"), False)
        m_employeePictureinTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/employeepictureintextbox"), False)
        m_advisorPictureinTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/advisorpictureintextbox"), False)
        m_advisorSigninTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/advisorsignintextbox"), False)

        m_resizeemployeePictureinTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/resizeemployeepictureintextbox"), False)
        m_resizeadvisorPictureinTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/resizeadvisorpictureintextbox"), False)
        m_resizeadvisorSigninTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/resizeadvisorsignintextbox"), False)

        agbTemplateFilename = m_MandantSettingsXml.GetSettingByKey("CVSetting/agbtemplatefilename")
        agbScanFilename = m_MandantSettingsXml.GetSettingByKey("CVSetting/agbscanfilename")
        m_agbinTextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/agbintextbox"), False)
        m_resizeagbintextbox = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/resizeagbintextbox"), False)

        m_coversheetdiplomascheckboxvalue = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/coversheetdiplomascheckboxvalue"), False)
        m_coversheetconditionscheckboxvalue = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/coversheetconditionscheckboxvalue"), False)
        m_agbpermanentcheckboxvalue = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/agbpermanentcheckboxvalue"), False)
        m_agbrentalcheckboxvalue = m_Utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey("CVSetting/agbrentalcheckboxvalue"), False)

      End If

      chkWithFAGB.Checked = m_agbpermanentcheckboxvalue
      chkWithTAGB.Checked = m_agbrentalcheckboxvalue

      m_AGBTemplateFilename = Path.Combine(m_TemplatePath, agbTemplateFilename)
      m_AGBScanFilename = Path.Combine(m_TemplatePath, agbScanFilename)
    End If


  End Sub

  Function ContinueChanging() As MsgBoxResult
    Dim bResult As MsgBoxResult

    If ClsDataDetail.bContentChanged Then
      Dim strMsg As String = "Möchten Sie die an Dokument vorgenommenen Änderungen speichern?"
      bResult = m_UtilityUi.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxDefaultButton.Button2)
      If bResult = MsgBoxResult.Yes Then FileSaveItem1.PerformClick()

      Me.FileSaveAsItem1.Enabled = ClsDataDetail.bContentChanged
    End If

    Return bResult

  End Function

  Sub InsertDataIntoDoc()

    Me.rtfContent.Modified = True

    Try
      If Me.rtfContent.RtfText.Length > 0 Then

        Dim Time_1 As Double = System.Environment.TickCount

        Me.AlertControl1.ShowCloseButton = True
				Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form),
															m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
															m_Translate.GetSafeTranslationValue("Variable werden übersetzt..."))

				Try
          TranslateMyDocument()

        Catch ex As Exception
          m_UtilityUi.ShowErrorDialog(Me, ex.ToString)

        End Try

        ClsDataDetail.bContentChanged = True

        Me.FileSaveAsItem1.Enabled = ClsDataDetail.bContentChanged
        Me.FileSaveAsItem1.Enabled = True
        Me.FileSaveItem1.Enabled = True
        Me.FileSaveItem1.Refresh()

        PlaySound()

			End If


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    Finally
      Me.bsiInfo.Caption = "Bereit"
      If Not IsNothing(f) Then f.Close()

    End Try

  End Sub

	Private Sub OnbbiDeleteCV_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDeleteCV.ItemClick
		Dim success As Boolean = True

		Dim msgResult = m_UtilityUi.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Hiermit löschen Sie das Dokument endgültig. Sind Sie sicher?"))

		If m_CurrentCVDocumentID Is Nothing OrElse msgResult = False Then Return
		success = m_EmployeeDatabaseAccess.DeleteAssignedEmployeeCVDocument(m_CurrentCVDocumentID, m_InitializationData.UserData.UserNr, m_InitializationData.UserData.UserFullName)
		If success Then
			m_UtilityUi.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Das Dokument wurde erfolgreich gelöscht."))

			ClsDataDetail.bContentChanged = False
			Me.Close()

		Else
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument konnte nicht gelöscht werden."))

			Return
		End If


	End Sub

	Sub TranslateMyDocument()
    Dim bWithEmployeeBild As Boolean
    Dim bWithAdvisorSign As Boolean
    Dim bWithAdvisorPicture As Boolean
    Dim bWithAGB As Boolean

		Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form),
													m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
													m_Translate.GetSafeTranslationValue("Kandidaten-Variable werden übersetzt..."))
		If Not FillCVEmployeeFields() Then Return

		' Benutzervariable ausfüllen...
		Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form),
													m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
													m_Translate.GetSafeTranslationValue("Benutzer-Variable werden übersetzt..."))
		FillCVAdvisorFields()

    Dim employeePicturefilename As String = String.Empty
    Dim advisorPicturefilename As String = String.Empty
    Dim advisorSignfilename As String = String.Empty
    Dim agbfilename As String = String.Empty

    bWithEmployeeBild = Me.rtfContent.RtfText.ToLower.Contains("#MABild".ToLower)
    bWithAdvisorPicture = Me.rtfContent.RtfText.ToLower.Contains("#USBild".ToLower)
    bWithAdvisorSign = Me.rtfContent.RtfText.ToLower.Contains("#USUnterschrift".ToLower)
    bWithAGB = Me.rtfContent.RtfText.ToLower.Contains("#AGBJPG".ToLower)

    If bWithEmployeeBild Then employeePicturefilename = m_LoadDataUtility.LoadEmployeePicture(EmployeeNumber)
    If bWithAdvisorPicture Then advisorPicturefilename = m_LoadDataUtility.LoadAdvisorPicture(m_InitializationData.UserData.UserNr)
    If bWithAdvisorSign Then advisorSignfilename = m_LoadDataUtility.LoadAdvisorSign(m_InitializationData.UserData.UserNr)
    If bWithAGB Then agbfilename = Path.Combine(m_TemplatePath, "AGB.JPG")


    ' Foto vom Kandidat
    'Dim strFilename As String = If(bWithEmployeeBild, strBildFilename, String.Empty)
    Dim employeeIMG As Image = Nothing
    Dim advisorPictureIMG As Image = Nothing
    Dim advisorSignIMG As Image = Nothing
    Dim agbIMG As Image = Nothing
    Try
      If bWithEmployeeBild AndAlso File.Exists(employeePicturefilename) Then
				Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form), "Bitte warten Sie einen Augenblick:",
															"Kandidatenbild wird eingefügt...")

				employeeIMG = Image.FromFile(employeePicturefilename)
        strSearchValue = "#MABild"
        Dim searchResult As ISearchResult = Me.rtfContent.Document.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, rtfContent.Document.Range)
        rtfContent.Document.BeginUpdate()
        Do While searchResult.FindNext()
          searchResult.Replace(String.Empty)
          Me.rtfContent.Document.Images.Insert(searchResult.CurrentResult.Start, employeeIMG)
          'Me.rtfContent.Document.InsertImage(searchResult.CurrentResult.Start, img)
        Loop
        rtfContent.Document.EndUpdate()

      Else
        rtfContent.Document.ReplaceAll("#MABild", String.Empty, SearchOptions.None)
        'Me.rtfContent.RtfText = Me.rtfContent.RtfText.Replace("#MABild", String.Empty)

      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      m_UtilityUi.ShowErrorDialog(ex.ToString)

    End Try
    'Return

    Try
      ' Unterschrift vom Benutzer
      'strFilename = If(bWithAdvisorPicture, loadData.LoadAdvisorPicture(_ClsProgSetting.GetLogedUSNr), String.Empty)
      If bWithAdvisorSign AndAlso File.Exists(advisorPicturefilename) Then
				Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form), m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
															m_Translate.GetSafeTranslationValue("Benutzerbild wird eingefügt..."))

				advisorPictureIMG = Image.FromFile(advisorPicturefilename)
        strSearchValue = "#USBild"
        Dim searchResult As ISearchResult = rtfContent.Document.StartSearch(strSearchValue,
                                                                              SearchOptions.WholeWord,
                                                                              SearchDirection.Forward,
                                                                              rtfContent.Document.Range)
        rtfContent.Document.BeginUpdate()
        Do While searchResult.FindNext()
          searchResult.Replace(String.Empty)
          Me.rtfContent.Document.Images.Insert(searchResult.CurrentResult.Start, advisorPictureIMG)
          'rtfContent.Document.InsertImage(searchResult.CurrentResult.Start, img)
        Loop
        rtfContent.Document.EndUpdate()

      Else
        rtfContent.Document.ReplaceAll("#USBild", String.Empty, SearchOptions.None)
        'Me.rtfContent.RtfText = Me.rtfContent.RtfText.Replace("#USBild", String.Empty)

      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      m_UtilityUi.ShowErrorDialog(ex.ToString)

    End Try

    Try
      ' Unterschrift vom Benutzer
      'strFilename = If(bWithAdvisorSign, loadData.LoadAdvisorSign(_ClsProgSetting.GetLogedUSNr), String.Empty)
      If bWithAdvisorSign AndAlso File.Exists(advisorSignfilename) Then
				Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form), m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
															m_Translate.GetSafeTranslationValue("Benutzerunterschrift wird eingefügt..."))

				advisorSignIMG = Image.FromFile(advisorSignfilename)
        strSearchValue = "#USUnterschrift"
        Dim searchResult As ISearchResult = Me.rtfContent.Document.StartSearch(strSearchValue,
                                                                              SearchOptions.WholeWord,
                                                                              SearchDirection.Forward,
                                                                              rtfContent.Document.Range)
        rtfContent.Document.BeginUpdate()
        Do While searchResult.FindNext()
          searchResult.Replace(String.Empty)
          Me.rtfContent.Document.Images.Insert(searchResult.CurrentResult.Start, advisorSignIMG)
        Loop
        rtfContent.Document.EndUpdate()

      Else
        rtfContent.Document.ReplaceAll("#USUnterschrift", String.Empty, SearchOptions.None)
        'Me.rtfContent.RtfText = Me.rtfContent.RtfText.Replace("#USUnterschrift", String.Empty)

      End If


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      m_UtilityUi.ShowErrorDialog(ex.ToString)

    End Try

    Try
      ' AGBTemplate
      'strFilename = If(bWithAGB, m_TemplatePath & "AGB.JPG", String.Empty)
      If bWithAGB AndAlso File.Exists(agbfilename) Then
        agbIMG = Image.FromFile(agbfilename)
				Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form), m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
															m_Translate.GetSafeTranslationValue("AGB wird eingefügt..."))

				InsertAGBToEndOfFile_2()

      Else
        rtfContent.Document.ReplaceAll("#AGBJPG", String.Empty, SearchOptions.None)
        'Me.rtfContent.RtfText = Me.rtfContent.RtfText.Replace("#AGBJPG", String.Empty)

      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      m_UtilityUi.ShowErrorDialog(ex.ToString)

    End Try

    If m_employeePictureinTextbox OrElse m_advisorPictureinTextbox OrElse m_advisorSigninTextbox OrElse m_agbinTextbox Then SearchTextInSubDocument(rtfContent.Document, employeeIMG, advisorPictureIMG, advisorSignIMG, agbIMG)

    FillCVLWorkPhasesFields()
		FillCVLEducationDataFields()
		FillCVLLanguagesFields()

		Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form), m_Translate.GetSafeTranslationValue("Fertig") & ":",
											m_Translate.GetSafeTranslationValue("Das Dokument wurde erfolgreich übersetzt") & "...")

	End Sub

	Private Function FillCVEmployeeFields() As Boolean
		Dim result As EmployeeMaskData
		If m_EmployeeMaskData Is Nothing OrElse m_EmployeeMaskData.Geschlecht Is Nothing Then
			result = m_LoadDataUtility.LoadEmployeeData(EmployeeNumber)
		Else
			result = m_EmployeeMaskData
		End If

		If result Is Nothing OrElse result.EmployeeAge = 0 Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kandidaten-Daten wurdne nicht gefunden."))

			Return False
		End If
		'Return True
		Try

			Dim anrede = If(result.Geschlecht.ToUpper.StartsWith("W"), "Frau", "Herr")
			rtfContent.Document.BeginUpdate()

			rtfContent.Document.ReplaceAll("#MAName", result.MAName, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MANachname", result.MANachname, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAVorname", result.MAVorname, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAStrasse", result.MAStrasse, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAPLZ", result.MAPLZ, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAOrt", result.MAOrt, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MATelefon", result.MATelefon, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MANatel", result.MANatel, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAEMail", result.MAEMail, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAGebDat", result.MAGebDat, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAAlter", result.EmployeeAge, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAHBeruf", result.MAHBeruf, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAHeimatort", result.MAHeimatort, SearchOptions.None)

			rtfContent.Document.ReplaceAll("@employeeDerDie@", String.Format("{0}", If(anrede = "Frau", "die", "den")), SearchOptions.WholeWord)
			rtfContent.Document.ReplaceAll("@employeeIn@", String.Format("{0}", If(anrede = "Frau", "in", "")), SearchOptions.None)
			rtfContent.Document.ReplaceAll("#employeeGender", anrede, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAAnrede", anrede, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MANationalityBez", result.MANationalityLabel, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MANationality", result.MANationality, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#employeeNationalityLabel", result.MANationalityLabel, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#employeeNationality", result.MANationality, SearchOptions.None)

			rtfContent.Document.ReplaceAll("#MAMobility_Bez", result.MAMobility_Bez, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#MAFahrzeug", result.MAFahrzeug, SearchOptions.None)

			rtfContent.Document.ReplaceAll("#F_Schein_1_Bez", result.F_Schein_1_Bez, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#F_Schein_2_Bez", result.F_Schein_2_Bez, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#F_Schein_3_Bez", result.F_Schein_3_Bez, SearchOptions.None)

			rtfContent.Document.ReplaceAll("#F_Schein1", result.F_Schein1, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#F_Schein2", result.F_Schein2, SearchOptions.None)
			rtfContent.Document.ReplaceAll("#F_Schein3", result.F_Schein3, SearchOptions.None)

			Dim bWithKIAnz As Boolean = rtfContent.Document.RtfText.ToLower.Contains("#MAKiAnz".ToLower)
			If bWithKIAnz Then
				Dim liKIInfo As List(Of EmployeeChildMaskData) = Nothing
				liKIInfo = m_LoadDataUtility.LoadEmployeeChildData(EmployeeNumber)

				rtfContent.Document.ReplaceAll("#MAKiAnz", (liKIInfo.Count - 1) & " " & m_Translate.GetSafeTranslationValue("Kinder"), SearchOptions.None)
				Dim strJahrGang As String = ""
				For Each child In liKIInfo
					strJahrGang &= If(strJahrGang = String.Empty, "", ", ") & child.ChildBirthdayYear
				Next
				rtfContent.Document.ReplaceAll("#MAKiJahrgang", strJahrGang, SearchOptions.None)
			End If
			If rtfContent.RtfText.ToLower.Contains("#MAZivilstand".ToLower) Then
				If Not result.MAZivilstand Is Nothing AndAlso Not String.IsNullOrWhiteSpace(result.MAZivilstand) Then
					Dim civilLabel As String = result.MAZivilstand

					Dim civilData = m_CommonDatabaseAccess.LoadCivilStateData
					If Not civilData Is Nothing Then
						Dim assignedData = civilData.Where(Function(x) x.GetField = result.MAZivilstand).FirstOrDefault '  m_LoadDataUtility.GetZivilBez(result.MAZivilstand, "DE")
						If Not assignedData Is Nothing Then
							civilLabel = assignedData.TranslatedCivilState
						End If
						rtfContent.Document.ReplaceAll("#MAZivilstand", String.Format("{0}", civilLabel), SearchOptions.None)

					End If
				End If

			End If
			If rtfContent.RtfText.ToLower.Contains("#MABewillig".ToLower) Then
				If Not result.MABewillig Is Nothing AndAlso Not String.IsNullOrWhiteSpace(result.MABewillig) Then
					Dim bewBez = m_LoadDataUtility.GetBewBez(result.MABewillig)
					rtfContent.Document.ReplaceAll("#MABewillig", String.Format("({0}) {1}", result.MABewillig, bewBez), SearchOptions.None)
				End If
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try
		Me.rtfContent.Document.EndUpdate()


		Return Not result Is Nothing

	End Function

	Private Sub FillCVAdvisorFields()
    Dim result As AdvisorMaskData = m_LoadDataUtility.LoadAdvisorMaskData(m_InitializationData.UserData.UserNr)

    If result Is Nothing Then
      m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Berater-Daten wurdne nicht gefunden."))

      Return
    End If

    rtfContent.Document.BeginUpdate()

    rtfContent.Document.ReplaceAll("@advisor@", String.Format("{0}", result.Advisor), SearchOptions.None)
    rtfContent.Document.ReplaceAll("@advisorEMail@", String.Format("{0}", result.USEmail), SearchOptions.None)
    rtfContent.Document.ReplaceAll("@advisorTelefon@", String.Format("{0}", result.USTelefon), SearchOptions.None)

    rtfContent.Document.ReplaceAll("#USFName", result.USFName, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USLName", result.USLName, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USAnrede", result.USAnrede, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USTelefon", result.USTelefon, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USTelefax", result.USTelefax, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USEmail", result.USEmail, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#IhrBerater", m_Translate.GetSafeTranslationValue(result.YourAdvisor), SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USTitel_1", result.USTitel_1, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USTitel_2", result.USTitel_2, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USNatel", result.USNatel, SearchOptions.None)

    rtfContent.Document.ReplaceAll("#USMDName", result.USMDName, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDName2", result.USMDName2, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDName3", result.USMDName3, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDPostfach", result.USMDPostfach, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDStrasse", result.USMDStrasse, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDPLZ", result.USMDPLZ, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDOrt", result.USMDOrt, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDLand", result.USMDLand, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDTelefon", result.USMDTelefon, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDTelefax", result.USMDTelefax, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDEMail", result.USMDEMail, SearchOptions.None)
    rtfContent.Document.ReplaceAll("#USMDHomepage", result.USMDHomepage, SearchOptions.None)

    Me.rtfContent.Document.EndUpdate()

    If m_customerinfoinTextbox Then
      SearchCustomerInfoInSubDocument(rtfContent.Document, result)
    End If


  End Sub

  Private Sub FillCVLWorkPhasesFields()
    Dim result As BindingList(Of WorkPhaseLocalViewData) = New BindingList(Of WorkPhaseLocalViewData)
    Dim cvlWorkphase As New SPSBaseTables(m_InitializationData)

		If m_EmployeeMasterData.CVLProfileID.GetValueOrDefault(0) = 0 Then Return
		result = cvlWorkphase.LoadCVLWorkPhases(m_EmployeeMasterData.CVLProfileID, Nothing)
    If result Is Nothing OrElse result.Count = 0 Then Return

    rtfContent.Document.BeginUpdate()

    Try

      '@WP_Comments_{0}@
      '@WP_CompanyViewData_{0}@
      '@WP_CurrentViewData_{0}@
      '@WP_CustomCodesViewData_{0}@
      '@WP_DateFrom_{0}@
      '@WP_DateFromFuzzy_{0}@
      '@WP_DateFromToViewData_{0}@
      '@WP_DateTo_{0}@
      '@WP_DateToFuzzy_{0}@
      '@WP_Duration_{0}@
      '@WP_DurationViewData_{0}@
      '@WP_EmploymentViewData_{0}@
      '@WP_FunctionViewData_{0}@
      '@WP_IndustryViewData_{0}@
      '@WP_InternetResourcesViewData_{0}@
      '@WP_LocationViewData_{0}@
      '@WP_OperationAreasViewData_{0}@
      '@WP_PlainText_{0}@
      '@WP_PositionViewData_{0}@
      '@WP_ProjectViewData_{0}@
      '@WP_SkillViewData_{0}@
      '@WP_SoftSkillViewData_{0}@
      '@WP_TopicViewData_{0}@
      '@WP_WorkTimeViewData_{0}@

      Dim i As Integer = 0
      For Each phase In result

        rtfContent.Document.ReplaceAll(String.Format("@WP_Comments_{0}@", i), String.Format("{0}", phase.Comments), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_CompanyViewData_{0}@", i), String.Format("{0}", phase.CompanyViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_CurrentViewData_{0}@", i), String.Format("{0}", phase.CurrentViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_CustomCodesViewData_{0}@", i), String.Format("{0}", phase.CustomCodesViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_DateFrom_{0}@", i), String.Format("{0}", phase.DateFrom), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_DateFromFuzzy_{0}@", i), String.Format("{0}", phase.DateFromFuzzy), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_DateFromToViewData_{0}@", i), String.Format("{0}", phase.DateFromToViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_DateTo_{0}@", i), String.Format("{0}", phase.DateTo), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_DateToFuzzy_{0}@", i), String.Format("{0}", phase.DateToFuzzy), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_Duration_{0}@", i), String.Format("{0}", phase.Duration), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_DurationViewData_{0}@", i), String.Format("{0}", phase.DurationViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_EmploymentViewData_{0}@", i), String.Format("{0}", phase.EmploymentViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_FunctionViewData_{0}@", i), String.Format("{0}", phase.FunctionViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_IndustryViewData_{0}@", i), String.Format("{0}", phase.IndustryViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_InternetResourcesViewData_{0}@", i), String.Format("{0}", phase.InternetResourcesViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_LocationViewData_{0}@", i), String.Format("{0}", phase.LocationViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_OperationAreasViewData_{0}@", i), String.Format("{0}", phase.OperationAreasViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_PlainText_{0}@", i), String.Format("{0}", phase.PlainText), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_PositionViewData_{0}@", i), String.Format("{0}", phase.PositionViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_ProjectViewData_{0}@", i), String.Format("{0}", phase.ProjectViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_SkillViewData_{0}@", i), String.Format("{0}", phase.SkillViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_SoftSkillViewData_{0}@", i), String.Format("{0}", phase.SoftSkillViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_TopicViewData_{0}@", i), String.Format("{0}", phase.TopicViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@WP_WorkTimeViewData_{0}@", i), String.Format("{0}", phase.WorkTimeViewData), SearchOptions.None)


        i += 1
      Next

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

      m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in Aufbau von (WorkPhases) Lebenslauf Daten."))
      Return

    Finally
      Me.rtfContent.Document.EndUpdate()

    End Try


  End Sub

  Private Sub FillCVLEducationDataFields()
    Dim result As BindingList(Of EducationPhaseLocalViewData) = New BindingList(Of EducationPhaseLocalViewData)
    Dim cvlWorkphase As New SPSBaseTables(m_InitializationData)

		If m_EmployeeMasterData.CVLProfileID.GetValueOrDefault(0) = 0 Then Return
		result = cvlWorkphase.LoadCVLEducationData(m_EmployeeMasterData.CVLProfileID, Nothing)
		If result Is Nothing OrElse result.Count = 0 Then Return

    rtfContent.Document.BeginUpdate()

    Try

      '@Ed_Comments_{0}@
      '@Ed_Completed_{0}@
      '@Ed_CompletedViewData_{0}@
      '@Ed_Current_{0}@
      '@Ed_CurrentViewData_{0}@
      '@Ed_DateFrom_{0}@
      '@Ed_DateFromFuzzy_{0}@
      '@Ed_DateFromToViewData_{0}@
      '@Ed_DateTo_{0}@
      '@Ed_DateToFuzzy_{0}@
      '@Ed_Duration_{0}@
      '@Ed_DurationViewData_{0}@
      '@Ed_EducationTypeViewData_{0}@
      '@Ed_GraduationViewData_{0}@
      '@Ed_IsCedCodeLable_{0}@
      '@Ed_PlainText_{0}@
      '@Ed_SchoolnameViewData_{0}@

      '@Ed_Skills_{0}_{1}@
      '@Ed_SoftSkills_{0}_{1}@
      '@Ed_SoftSkills_{0}_{1}@
      '@Ed_Topic_{0}_{1}@

      Dim i As Integer = 0
      For Each phase In result

        rtfContent.Document.ReplaceAll(String.Format("@Ed_Comments_{0}@", i), String.Format("{0}", phase.Comments), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_Completed_{0}@", i), String.Format("{0}", phase.Completed), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_CompletedViewData_{0}@", i), String.Format("{0}", phase.CompletedViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_Current_{0}@", i), String.Format("{0}", phase.Current), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_CurrentViewData_{0}@", i), String.Format("{0}", phase.CurrentViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_DateFrom_{0}@", i), String.Format("{0}", phase.DateFrom), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_DateFromFuzzy_{0}@", i), String.Format("{0}", phase.DateFromFuzzy), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_DateFromToViewData_{0}@", i), String.Format("{0}", phase.DateFromToViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_DateTo_{0}@", i), String.Format("{0}", phase.DateTo), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_DateToFuzzy_{0}@", i), String.Format("{0}", phase.DateToFuzzy), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_Duration_{0}@", i), String.Format("{0}", phase.Duration), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_DurationViewData_{0}@", i), String.Format("{0}", phase.DurationViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_EducationTypeViewData_{0}@", i), String.Format("{0}", phase.EducationTypeViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_GraduationViewData_{0}@", i), String.Format("{0}", phase.GraduationViewData), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_IsCedCodeLable_{0}@", i), String.Format("{0}", phase.IsCedCodeLable), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_PlainText_{0}@", i), String.Format("{0}", phase.PlainText), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Ed_SchoolnameViewData_{0}@", i), String.Format("{0}", phase.SchoolnameViewData), SearchOptions.None)

        Dim j As Integer = 0
        For Each skill In phase.SoftSkills
          rtfContent.Document.ReplaceAll(String.Format("@Ed_Skills_{0}_{1}@", i, j), String.Format("{0}", skill.Name), SearchOptions.None)
          j += 1
        Next

        j = 0
        For Each softskills In phase.SoftSkills
          rtfContent.Document.ReplaceAll(String.Format("@Ed_SoftSkills_{0}_{1}@", i, j), String.Format("{0}", softskills.Name), SearchOptions.None)
          j += 1
        Next

        j = 0
        For Each topic In phase.Topic
          rtfContent.Document.ReplaceAll(String.Format("@Ed_Topic_{0}_{1}@", i, j), String.Format("{0}", topic.Lable), SearchOptions.None)
          j += 1
        Next

        i += 1
      Next


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

      m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in Aufbau von (Education) Lebenslauf Daten."))
      Return

    Finally
      Me.rtfContent.Document.EndUpdate()

    End Try

  End Sub


  Private Sub FillCVLLanguagesFields()
    Dim cvlWorkphase As New SPSBaseTables(m_InitializationData)


		If m_EmployeeMasterData.CVLProfileID.GetValueOrDefault(0) = 0 Then Return
		Dim success = cvlWorkphase.LoadCVLAdditionalInfos(m_EmployeeMasterData.CVLProfileID, Nothing)
		If Not success Then Return

    Dim additionalData As AdditionalInfoLocalViewData = New AdditionalInfoLocalViewData
    additionalData = cvlWorkphase.AdditionalInfoData
    Dim LanguageData = cvlWorkphase.AdditionalLanguageData

    If additionalData Is Nothing Then Return
    If LanguageData Is Nothing OrElse LanguageData.Count = 0 Then Return


    rtfContent.Document.BeginUpdate()

    Try

      '@Add_Additionals@
      '@Add_Competences@
      '@Add_DrivingLicenceViewData@
      '@Add_InternetResourcesViewData@
      '@Add_MilitaryServiceViewData@
      '@Add_UndatedIndustryViewData@
      '@Add_UndatedOperationAreViewData@
      '@Add_UndatedSkillViewData@

      '@Lang_Code_{0}@
      '@Lang_Name_{0}@

      rtfContent.Document.ReplaceAll("@Add_Additionals@", String.Format("{0}", additionalData.Additionals), SearchOptions.None)
      rtfContent.Document.ReplaceAll("@Add_Competences@", String.Format("{0}", additionalData.Competences), SearchOptions.None)
      rtfContent.Document.ReplaceAll("@Add_DrivingLicenceViewData@", String.Format("{0}", additionalData.DrivingLicenceViewData), SearchOptions.None)
      rtfContent.Document.ReplaceAll("@Add_InternetResourcesViewData@", String.Format("{0}", additionalData.InternetResourcesViewData), SearchOptions.None)
      rtfContent.Document.ReplaceAll("@Add_MilitaryServiceViewData@", String.Format("{0}", additionalData.MilitaryServiceViewData), SearchOptions.None)
      rtfContent.Document.ReplaceAll("@Add_UndatedIndustryViewData@", String.Format("{0}", additionalData.UndatedIndustryViewData), SearchOptions.None)
      rtfContent.Document.ReplaceAll("@Add_UndatedOperationAreViewData@", String.Format("{0}", additionalData.UndatedOperationAreViewData), SearchOptions.None)
      rtfContent.Document.ReplaceAll("@Add_UndatedSkillViewData@", String.Format("{0}", additionalData.UndatedSkillViewData), SearchOptions.None)

      Dim i As Integer = 0
      For Each lang In LanguageData
        rtfContent.Document.ReplaceAll(String.Format("@Lang_Code_{0}@", i), String.Format("{0}", lang.Code), SearchOptions.None)
        rtfContent.Document.ReplaceAll(String.Format("@Lang_Name_{0}@", i), String.Format("{0}", lang.Name), SearchOptions.None)
        i += 1
      Next


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

      m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler in Aufbau von (Additionalinfo / Language) Lebenslauf Daten."))
      Return

    Finally
      Me.rtfContent.Document.EndUpdate()

    End Try


  End Sub


#Region "Test Funktionen..."


  Private Sub SearchCustomerInfoInSubDocument(ByVal mydocument As Document, data As AdvisorMaskData)

    Dim searchResult As ISearchResult
    For Each shape As Shape In mydocument.Shapes

			If (shape.ShapeFormat.TextBox IsNot Nothing) Then

				Dim textBox = shape.ShapeFormat.TextBox
				Dim subDocument As SubDocument = textBox.Document
				subDocument.BeginUpdate()
				Dim strSearchValue As String = "#USMDName"

				strSearchValue = "@advisor@"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, String.Format("{0}", data.Advisor))
				Loop
				strSearchValue = "@advisorEMail@"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, String.Format("{0}", data.USEmail))
				Loop
				strSearchValue = "@advisorTelefon@"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, String.Format("{0}", data.USTelefon))
				Loop
				strSearchValue = "#USFName"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USFName)
				Loop
				strSearchValue = "#USLName"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USLName)
				Loop
				strSearchValue = "#USAnrede"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USAnrede)
				Loop
				strSearchValue = "#USTelefon"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USTelefon)
				Loop
				strSearchValue = "#USTelefax"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USTelefax)
				Loop
				strSearchValue = "#USEmail"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USEmail)
				Loop
				strSearchValue = "#IhrBerater"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, m_Translate.GetSafeTranslationValue(data.YourAdvisor))
				Loop
				strSearchValue = "#USTitel_1"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USTitel_1)
				Loop
				strSearchValue = "#USTitel_2"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USTitel_2)
				Loop
				strSearchValue = "#USNatel"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USNatel)
				Loop

				strSearchValue = "#USMDName"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDName)
				Loop
				strSearchValue = "#USMDName2"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDName2)
				Loop
				strSearchValue = "#USMDName3"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDName3)
				Loop
				strSearchValue = "#USMDPostfach"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDPostfach)
				Loop
				strSearchValue = "#USMDStrasse"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDStrasse)
				Loop

				strSearchValue = "#USMDPLZ"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDPLZ)
				Loop
				strSearchValue = "#USMDOrt"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDOrt)
				Loop
				strSearchValue = "#USMDLand"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDLand)
				Loop
				strSearchValue = "#USMDTelefon"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDTelefon)
				Loop
				strSearchValue = "#USMDTelefax"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDTelefax)
				Loop
				strSearchValue = "#USMDEMail"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDEMail)
				Loop
				strSearchValue = "#USMDHomepage"
				searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
				Do While searchResult.FindNext()
					searchResult.Replace(String.Empty)
					Dim myImage = subDocument.InsertText(searchResult.CurrentResult.Start, data.USMDHomepage)
				Loop


				subDocument.EndUpdate()

			End If
		Next

  End Sub

  Private Sub SearchTextInSubDocument(ByVal mydocument As Document, ByVal employeeIMG As Image, ByVal advisorPictureIMG As Image, ByVal advisorSignIMG As Image, ByVal agbIMG As Image)
    Dim searchResult As ISearchResult

    For Each shape As Shape In mydocument.Shapes
			If (shape.ShapeFormat.TextBox IsNot Nothing) Then
				Dim textBox = shape.ShapeFormat.TextBox
				Dim subDocument As SubDocument = textBox.Document

				If m_employeePictureinTextbox Then
					Dim strSearchValue As String = "#MABild"
					searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
					subDocument.BeginUpdate()
					Do While searchResult.FindNext()
						searchResult.Replace(String.Empty)
						If Not employeeIMG Is Nothing Then
							Dim myImage = subDocument.Images.Insert(searchResult.CurrentResult.Start, employeeIMG)
							If m_resizeemployeePictureinTextbox Then myImage.Size = New Size With {.Height = shape.Size.Height, .Width = shape.Size.Width}
						End If
					Loop
					subDocument.EndUpdate()
				End If


				If m_advisorPictureinTextbox Then
					strSearchValue = "#USBild"
					searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
					subDocument.BeginUpdate()
					Do While searchResult.FindNext()
						searchResult.Replace(String.Empty)
						If Not advisorPictureIMG Is Nothing Then
							Dim myImage = subDocument.Images.Insert(searchResult.CurrentResult.Start, advisorPictureIMG)
							If m_resizeadvisorPictureinTextbox Then myImage.Size = New Size With {.Height = shape.Size.Height, .Width = shape.Size.Width}
						End If
					Loop
					subDocument.EndUpdate()
				End If

				If m_advisorSigninTextbox AndAlso Not advisorSignIMG Is Nothing Then
					strSearchValue = "#USUnterschrift"
					searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
					subDocument.BeginUpdate()
					Do While searchResult.FindNext()
						searchResult.Replace(String.Empty)
						If Not advisorSignIMG Is Nothing Then
							Dim myImage = subDocument.Images.Insert(searchResult.CurrentResult.Start, advisorSignIMG)
							If m_resizeadvisorSigninTextbox Then myImage.Size = New Size With {.Height = shape.Size.Height, .Width = shape.Size.Width}
						End If
					Loop
					subDocument.EndUpdate()
				End If

				If m_agbinTextbox AndAlso Not agbIMG Is Nothing Then
					strSearchValue = "#AGBJPG"
					searchResult = subDocument.StartSearch(strSearchValue, SearchOptions.WholeWord, SearchDirection.Forward, subDocument.Range)
					subDocument.BeginUpdate()
					Do While searchResult.FindNext()
						searchResult.Replace(String.Empty)
						If Not agbIMG Is Nothing Then
							Dim myImage = subDocument.Images.Insert(searchResult.CurrentResult.Start, agbIMG)
							If m_resizeagbintextbox Then myImage.Size = New Size With {.Height = shape.Size.Height, .Width = shape.Size.Width}
						End If
					Loop
					subDocument.EndUpdate()
				End If


			End If
		Next

  End Sub

  Sub InsertAGBToEndOfFile_2()
    Dim rtfData_2 As New DevExpress.XtraRichEdit.RichEditControl
    Exit Sub

    rtfData_2.LoadDocument(m_AGBTemplateFilename, GetLLDocFormat)
    InsertAdditionalPage()

    Me.rtfContent.Document.InsertRtfText(Me.rtfContent.Document.CaretPosition, rtfData_2.RtfText)
    Dim par = rtfContent.Document.Paragraphs.Get(rtfContent.Document.CaretPosition)
    Dim pr As DevExpress.XtraRichEdit.API.Native.ParagraphProperties = rtfContent.Document.BeginUpdateParagraphs(par.Range)

    rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Page.PaperKind = System.Drawing.Printing.PaperKind.A4
    rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Margins.Top = -0.5
    rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Margins.Left = 1
    rtfContent.Document.EndUpdateParagraphs(pr)

    PlaySound()

  End Sub

  Function InsertTemplate4AGBFilesToEndOfFile(ByVal rtfControl As DevExpress.XtraRichEdit.RichEditControl) As DevExpress.XtraRichEdit.RichEditControl
    Dim strResult As DevExpress.XtraRichEdit.RichEditControl = rtfControl
    Dim rtfControl_Kopie As New DevExpress.XtraRichEdit.RichEditControl
    Dim rtfData_2 As New DevExpress.XtraRichEdit.RichEditControl

    rtfControl_Kopie = rtfControl
    rtfData_2.LoadDocument(m_AGBTemplateFilename, GetLLDocFormat)
    Try
      ' Funktionen für die neue Seiten ohne Header...
      rtfControl_Kopie.Document.AppendSection()
      rtfControl_Kopie.Document.CaretPosition = rtfControl_Kopie.Document.Range.End

      Dim eph As New EditPageHeaderCommand(rtfControl_Kopie)
      Dim thfltp As New ToggleHeaderFooterLinkToPreviousCoreCommand(rtfControl_Kopie)
      Dim cphf As New ClosePageHeaderFooterCommand(rtfControl_Kopie)

      eph.Execute()
      thfltp.Execute()
      cphf.Execute()

      Dim doc As SubDocument = rtfControl_Kopie.Document.Sections(1).BeginUpdateHeader()
      doc.Delete(doc.Range)
      rtfControl_Kopie.Document.Sections(1).EndUpdateHeader(doc)

    Catch ex As Exception

    End Try

    rtfControl_Kopie.Document.InsertRtfText(rtfControl_Kopie.Document.CaretPosition, rtfData_2.RtfText)
    Dim par As DevExpress.XtraRichEdit.API.Native.Paragraph = rtfControl_Kopie.Document.Paragraphs.Get(rtfControl_Kopie.Document.CaretPosition)
    Dim pr As DevExpress.XtraRichEdit.API.Native.ParagraphProperties = rtfControl_Kopie.Document.BeginUpdateParagraphs(par.Range)

    rtfControl_Kopie.Document.Sections(rtfControl_Kopie.Document.Sections.Count - 1).Page.PaperKind =
                              System.Drawing.Printing.PaperKind.A4
    rtfControl_Kopie.Document.Sections(rtfControl_Kopie.Document.Sections.Count - 1).Margins.Top = 0
    rtfControl_Kopie.Document.Sections(rtfControl_Kopie.Document.Sections.Count - 1).Margins.Left = 3
    rtfControl_Kopie.Document.EndUpdateParagraphs(pr)

    strResult = rtfControl_Kopie

    Return strResult
  End Function

  Sub InsertAGB2Document()
    Dim bWithAGB As Boolean

    bWithAGB = Me.rtfContent.RtfText.ToLower.Contains("#AGBJPG".ToLower)

    ' Unterschrift vom Benutzer
    Dim strFilename As String = m_AGBScanFilename ' m_TemplatePath & "AGB.JPG"

    Try

      If File.Exists(strFilename) Then
        InsertAdditionalPage()
				Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form), m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
															m_Translate.GetSafeTranslationValue("AGB wird eingefügt..."))

				Dim img As Image = Image.FromFile(strFilename)
        strSearchValue = "#AGBJPG"
        Dim searchResult As ISearchResult = rtfContent.Document.StartSearch(strSearchValue,
                                                                              SearchOptions.WholeWord,
                                                                              SearchDirection.Forward,
                                                                              rtfContent.Document.Range)
        rtfContent.Document.BeginUpdate()
        Do While searchResult.FindNext()
          searchResult.Replace(String.Empty)
          rtfContent.Document.Images.Insert(searchResult.CurrentResult.Start, img)
          Me.rtfContent.Document.CaretPosition = searchResult.CurrentResult.Start

          Me.rtfContent.Document.CaretPosition = searchResult.CurrentResult.Start

          Dim range As DocumentRange
          If direction = SearchDirection.Forward Then
            Dim startPos As Integer = Me.rtfContent.Document.Selection.End.ToInt()
            Dim length As Integer = Me.rtfContent.Document.Range.End.ToInt() - startPos
            range = Me.rtfContent.Document.CreateRange(startPos, length)

          Else
            Dim length As Integer = Me.rtfContent.Document.Selection.Start.ToInt()
            range = Me.rtfContent.Document.CreateRange(0, length)

          End If

        Loop
        rtfContent.Document.Images.Insert(rtfContent.Document.CaretPosition, img)

        Dim par As DevExpress.XtraRichEdit.API.Native.Paragraph =
        rtfContent.Document.Paragraphs.Get(rtfContent.Document.CaretPosition)
        Dim pr As DevExpress.XtraRichEdit.API.Native.ParagraphProperties = rtfContent.Document.BeginUpdateParagraphs(par.Range)

        rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Page.PaperKind = System.Drawing.Printing.PaperKind.A4
        rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Margins.Top = -0.5
        rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Margins.Left = 0
        rtfContent.Document.EndUpdateParagraphs(pr)

      Else
        rtfContent.Document.ReplaceAll("#AGBJPG", String.Empty, SearchOptions.None)
        'Me.rtfContent.RtfText = Me.rtfContent.RtfText.Replace("#AGBJPG", String.Empty)

      End If
      rtfContent.Document.EndUpdate()

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      m_UtilityUi.ShowErrorDialog(ex.ToString)

    End Try


  End Sub

  Sub InsertAGBToEndOfFile()
    Dim rtfData_1 As New DevExpress.XtraRichEdit.RichEditControl
    Dim rtfData_2 As New DevExpress.XtraRichEdit.RichEditControl
    Dim strSearchtext As String = "#AGBTemplate"

    rtfData_1.RtfText = Me.rtfContent.RtfText
    rtfData_2.LoadDocument(m_AGBTemplateFilename, GetLLDocFormat)
    Dim str1 As String = rtfData_1.RtfText
    Dim endPosition As DocumentPosition = rtfData_2.Document.CreatePosition(rtfData_2.Document.Range.End.ToInt())
    Dim searchResult As ISearchResult = rtfData_1.Document.StartSearch(strSearchtext,
                                                                      SearchOptions.WholeWord,
                                                                      SearchDirection.Forward,
                                                                      rtfData_1.Document.Range)
    If IsNothing(searchResult.CurrentResult) Then
      searchResult = rtfData_1.Document.StartSearch(strSearchtext, SearchOptions.WholeWord,
                                                    SearchDirection.Backward, rtfData_1.Document.Range)
    End If
    Do While searchResult.FindNext()
      searchResult.Replace(String.Empty)
      rtfData_1.Document.CaretPosition = searchResult.CurrentResult.Start

      Dim range As DocumentRange
      If direction = SearchDirection.Forward Then
        Dim startPos As Integer = rtfData_1.Document.Selection.End.ToInt()
        Dim length As Integer = rtfData_1.Document.Range.End.ToInt() - startPos
        range = rtfData_1.Document.CreateRange(startPos, length)

      Else
        Dim length As Integer = rtfData_1.Document.Selection.Start.ToInt()
        range = rtfData_1.Document.CreateRange(0, length)

      End If
      rtfData_1.Document.InsertRtfText(rtfData_1.Document.CreatePosition(range.Start.ToInt()), rtfData_2.RtfText)
      Me.rtfContent.RtfText = rtfData_1.RtfText

    Loop
    PlaySound()

  End Sub

  Function GetMADoc(ByVal lRecID As Long) As String
    Dim strResult As String = String.Empty
    Dim strGuidPath As String = Path.Combine(_ClsProgSetting.GetSpSFiles2DeletePath, System.Guid.NewGuid.ToString) & "\"
    Dim strFullFilename As String = String.Empty
    Dim obj As New SP.PDFO2S.ClsMain_Net


    If lRecID = 0 Then Return strResult
    If Not Directory.Exists(strGuidPath) Then Directory.CreateDirectory(strGuidPath)
    Try
      Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
      Conn.Open()

      Dim sSql As String = "Select [ID], DocScan, ScanExtension From MA_LLDoc Where MANr = @MANr "
      sSql &= "And ID = @RecID"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MANr", EmployeeNumber)
      param = cmd.Parameters.AddWithValue("@RecID", lRecID)
      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

      Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Die Dokumente werden eingelesen...")
      Me.bsiInfo.Refresh()
      InsertAdditionalPage()

      While rFoundedrec.Read
        Me.AlertControl1.AutoFormDelay = 1000
				Me.AlertControl1.Show(DirectCast(Me, System.Windows.Forms.Form),
															m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick:"),
															m_Translate.GetSafeTranslationValue("Kandidatendokument wird importiert"))
				Try
          Dim BA As Byte()
          BA = CType(rFoundedrec("DocScan"), Byte())

          Dim ArraySize As New Integer
          ArraySize = BA.GetUpperBound(0)
          strFullFilename = strGuidPath & rFoundedrec("ID").ToString &
                              If(rFoundedrec("ScanExtension").ToString.Contains("."),
                                 rFoundedrec("ScanExtension").ToString,
                                 "." & rFoundedrec("ScanExtension").ToString)

          If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
          Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
          fs.Write(BA, 0, ArraySize + 1)
          fs.Close()
          fs.Dispose()
          Dim aGrafikFiles As New List(Of String)
          If strFullFilename.ToLower.Contains(".pdf") Then
            Try
              aGrafikFiles = obj.ConvertPDFToGrafikAsArray(strFullFilename, strGuidPath & lRecID,
                                                          0, 0, ImageFormat.Jpeg)

            Catch ex As Exception
              m_Logger.LogError(ex.ToString)

            End Try
          End If

          Dim bExistsTags4ScanFiles As Boolean = False
          rtfContent.Document.BeginUpdate()

          For i As Integer = 0 To aGrafikFiles.Count - 1
            strFullFilename = aGrafikFiles(i)
            If strFullFilename.ToLower.Contains(".jpg") Or
                    strFullFilename.ToLower.Contains(".jpeg") Or
                    strFullFilename.ToLower.Contains(".bmp") Or
                    strFullFilename.ToLower.Contains(".png") Or
                    strFullFilename.ToLower.Contains(".emf") Or
                    strFullFilename.ToLower.Contains(".gif") Then

              Try
                ' Dokumente vom Kandidat
                Dim img As Image = Image.FromFile(strFullFilename)

                Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Dokumente {0} wird importiert..."), strFullFilename)
                'Me.siMeldung.Refresh()

                If i = 0 Then
                  strSearchValue = "#MAScannedFiles"
                  If Not bExistsTags4ScanFiles Then
                    Dim searchResult As ISearchResult = rtfContent.Document.StartSearch(strSearchValue,
                                                                                          SearchOptions.WholeWord,
                                                                                          SearchDirection.Forward,
                                                                                          rtfContent.Document.Range)


                    Do While searchResult.FindNext()
                      bExistsTags4ScanFiles = True
                      searchResult.Replace(String.Empty)
                      rtfContent.Document.Images.Insert(searchResult.CurrentResult.Start, img)
                      Me.rtfContent.Document.CaretPosition = searchResult.CurrentResult.Start
                    Loop
                    If Not bExistsTags4ScanFiles Then
                      rtfContent.Document.Images.Insert(rtfContent.Document.CaretPosition, img)
                      Dim par As DevExpress.XtraRichEdit.API.Native.Paragraph = rtfContent.Document.Paragraphs.Get(rtfContent.Document.CaretPosition)
                      Dim pr As DevExpress.XtraRichEdit.API.Native.ParagraphProperties = rtfContent.Document.BeginUpdateParagraphs(par.Range)

                      rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Page.PaperKind = System.Drawing.Printing.PaperKind.A4
                      'rtContent.Document.Sections(rtContent.Document.Sections.Count-1).Page.Landscape = True
                      rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Margins.Top = -0.5
                      rtfContent.Document.Sections(rtfContent.Document.Sections.Count - 1).Margins.Left = 0

                      rtfContent.Document.EndUpdateParagraphs(pr)
                    End If

                  End If

                Else
                  rtfContent.Document.Images.Insert(rtfContent.Document.CaretPosition, img)

                End If


              Catch ex As Exception
                m_Logger.LogError(ex.ToString)
                DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler ({0}): {1}", "GetMADoc_1", ex.Message),
                                                           m_Translate.GetSafeTranslationValue("Daten importieren"),
                                                           MessageBoxButtons.OK,
                                                           MessageBoxIcon.Error)

              End Try

            End If
          Next

        Catch ex As Exception
          m_Logger.LogError(ex.ToString)
          DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler ({0}): {1}", "GetMADoc_2", ex.Message),
                                                     m_Translate.GetSafeTranslationValue("Daten importieren"),
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error)
        End Try

        strResult &= If(strResult = String.Empty, "", ";") & strFullFilename
      End While

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler ({0}): {1}", "GetMADoc_0", ex.Message),
                                                 m_Translate.GetSafeTranslationValue("Daten importieren"),
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Error)

    Finally
      rtfContent.Document.EndUpdate()
      Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
      Me.siBar_0.Refresh()

      ClsDataDetail.bContentChanged = True

    End Try

    Return strResult
  End Function

#End Region


  Private Sub AlertControl1_BeforeFormShow(ByVal sender As Object,
                                           ByVal e As DevExpress.XtraBars.Alerter.AlertFormEventArgs) Handles AlertControl1.BeforeFormShow
    If Not IsNothing(f) Then
      f.Close()
    End If
    f = e.AlertForm
  End Sub

  Private Sub iPDF_ItemClick(ByVal sender As System.Object,
                             ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles iPdf.ItemClick
    Dim dlg As New SaveFileDialog
    dlg.Filter = "PDF-Dateien (*.PDF)|*.pdf"
    dlg.FilterIndex = 1
    dlg.InitialDirectory = _ClsProgSetting.GetSpSFiles2DeletePath

    dlg.FileName = String.Format("Lebenslauf_{0}.pdf", EmployeeNumber)
    If dlg.ShowDialog() = DialogResult.OK Then
      m_MergePDFFilename = dlg.FileName
    Else
      Exit Sub
    End If
    dlg.Dispose()

    Try
      If File.Exists(m_MergePDFFilename) Then File.Delete(m_MergePDFFilename)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      Dim strMsg As String = "Fehler beim Löschen der Datei. Möglicherweise ist die Datei in Verwendung."
      DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg) &
                                                 String.Format(" {0}{1}{2}", vbNewLine, m_MergePDFFilename, ex.ToString),
                                                               m_Translate.GetSafeTranslationValue("Datei löschen"),
                                                               MessageBoxButtons.OK,
                                                               MessageBoxIcon.Error)
      Exit Sub
    End Try

    Try
      Dim strResult As String = CreateLLMergeFile(m_MergePDFFilename)
      ClsDataDetail.strFilename4Print_Save = m_MergePDFFilename

      If strResult.ToLower.Contains("erfolg") Then
        Dim strMsg As String = m_Translate.GetSafeTranslationValue("Ihre Datei wurde erfolgreich erstellt.")
        DevExpress.XtraEditors.XtraMessageBox.Show(strMsg &
                String.Format(" {0}{1}",
                              vbNewLine, m_MergePDFFilename),
                              m_Translate.GetSafeTranslationValue("Datei anlegen"),
                              MessageBoxButtons.OK, MessageBoxIcon.Information)


      Else
        Dim strMsg As String = m_Translate.GetSafeTranslationValue("Ihre Datei konnte nicht erfolgreich erstellt werden.")
        DevExpress.XtraEditors.XtraMessageBox.Show(strMsg &
                String.Format(" {0}{1}",
                              vbNewLine, m_MergePDFFilename),
                              m_Translate.GetSafeTranslationValue("Datei anlegen"),
                              MessageBoxButtons.OK, MessageBoxIcon.Error)
      End If


    Catch ex As Exception
      Dim strMsg As String = m_Translate.GetSafeTranslationValue("Fehler beim Erstellen der Datei.")
      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg &
                                                 String.Format(" {0}{1}{2}",
                                                               vbNewLine, m_MergePDFFilename,
                                                               ex.ToString),
                                                               m_Translate.GetSafeTranslationValue("Datei anlegen"),
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error)
    End Try

  End Sub

  Function CreateLLMergeFile(ByVal strDestFile As String) As String

    If strDestFile = String.Empty Then Return "Leere Dateiname"

    ' Der Lebenslauf wird in PDF umgewandelt
    Dim strZDeckblatt As String = String.Empty
    Dim strAGBDeckblatt As String = String.Empty
    Dim strAGBTFile As String = String.Empty
    Dim strAGBFFile As String = String.Empty
    Dim strMAScanDoc As String = String.Empty
    Dim strLLFile As String = String.Empty
    Dim strResult As String = "erfolgreich..."

    Try
      Dim bExistDeckblatt4Zeugnis As Boolean = If(Me.clstMADoc.Items.Count = 0, False, Me.clstMADoc.Items(0).ToString.ToLower.Contains(m_Translate.GetSafeTranslationValue("zeugnis").ToLower))
      If bExistDeckblatt4Zeugnis Then
        strZDeckblatt = Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4ZeuginisDeckblatt)
        If CBool(Not Me.clstMADoc.Items(0).CheckState = CheckState.Checked) Then strZDeckblatt = String.Empty
      End If

      strAGBTFile = Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4AGBTemp)
      If CBool(Not Me.chkWithTAGB.Checked) Then strAGBTFile = String.Empty
      strAGBFFile = Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4AGBFest)
      If CBool(Not Me.chkWithFAGB.Checked) Then strAGBFFile = String.Empty

      Dim bExistDeckblatt4AGB As Boolean = If(Me.clstMADoc.Items.Count = 0, True, Me.clstMADoc.Items(Me.clstMADoc.Items.Count - 1).ToString.ToLower.Contains(m_Translate.GetSafeTranslationValue("Deckblatt").ToLower))
      If bExistDeckblatt4AGB Then
        strAGBDeckblatt = Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4AGBDeckblatt)
        If Me.clstMADoc.Items.Count <> 0 Then
          If CBool(Not Me.clstMADoc.Items(Me.clstMADoc.Items.Count - 1).CheckState = CheckState.Checked) Then strAGBDeckblatt = String.Empty
        End If
      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      Return String.Format("Fehler beim Erstellen der AGB. {0}{1}", vbNewLine, ex.ToString)

    End Try

    Try
      strMAScanDoc = m_LoadDataUtility.CreateMAScanDocFile(Me.clstMADoc, EmployeeNumber)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      Return String.Format("Fehler beim Erstellen der gescannten Dokumente. {0}{1}", vbNewLine, ex.ToString)

    End Try

    Try
      strLLFile = Path.Combine(_ClsProgSetting.GetSpSMATempPath, String.Format("LLTemp_{0}.pdf", EmployeeNumber))
      Me.rtfContent.ExportToPdf(strLLFile)

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      Return String.Format("Fehler beim Erstellen der Lebenslauf. {0}{1}", vbNewLine, ex.ToString)

    End Try

    Try
      Dim strFiles2Merge As New List(Of String)

      If File.Exists(strLLFile) Then strFiles2Merge.Add(strLLFile)
      If File.Exists(strZDeckblatt) Then strFiles2Merge.Add(strZDeckblatt)
      If File.Exists(strMAScanDoc) Then strFiles2Merge.Add(strMAScanDoc)
      If File.Exists(strAGBDeckblatt) Then strFiles2Merge.Add(strAGBDeckblatt)
      If File.Exists(strAGBTFile) Then strFiles2Merge.Add(strAGBTFile)
      If File.Exists(strAGBFFile) Then strFiles2Merge.Add(strAGBFFile)
      If strFiles2Merge.Count > 1 Then
        Dim obj As New SP.PDFO2S.ClsPDF4Net
        strResult = obj.Merg2PDFFiles(strDestFile, strFiles2Merge.ToArray)

      Else
        File.Copy(strLLFile, strDestFile)

      End If


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      Return String.Format("Fehler beim zusammenführen der einzelnen Dateien. {0}{1}", vbNewLine, ex.ToString)

    End Try

    Return strResult
  End Function

  Private Sub QuickPrintItem1_ItemClick(ByVal sender As System.Object,
                                        ByVal e As DevExpress.XtraBars.ItemClickEventArgs) _
                                        Handles QuickPrintItem1.ItemClick,
                                        PrintItem1.ItemClick

    Try
      If File.Exists(m_MergePDFFilename) Then File.Delete(m_MergePDFFilename)

    Catch ex As Exception
      Dim strMsg As String = "Fehler beim Löschen der Datei. Möglicherweise ist die Datei in Verwendung."
      DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg) &
                                                 String.Format("{0}{1}{2}",
                                                               vbNewLine, m_MergePDFFilename, ex.ToString),
                                                               m_Translate.GetSafeTranslationValue("Datei löschen"),
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error)
      Exit Sub
    End Try
    Dim m_pdfPrinterSettings As Printing.PrinterSettings
    m_pdfPrinterSettings = New Printing.PrinterSettings

    Try
      Dim strResult As String = CreateLLMergeFile(m_MergePDFFilename)
      ClsDataDetail.strFilename4Print_Save = m_MergePDFFilename

      ' jetzt muss hier gedruckt werden
      Dim dlgPrinter As New PrintDialog
      dlgPrinter.ShowNetwork = True

      If Not e.Item.Name.ToLower.Contains("quickprint") Then
        If dlgPrinter.ShowDialog() = DialogResult.OK Then
          m_pdfPrinterSettings = dlgPrinter.PrinterSettings
        Else
          Return

        End If
      End If
      SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
      SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihr Dokument wird gedruckt") & Space(20))
      SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Moment") & "...")

      Dim pdfDocument As New PdfDocumentProcessor()
      Dim pdfPrinterSettings As New PdfPrinterSettings(m_pdfPrinterSettings)
      pdfDocument.LoadDocument(m_MergePDFFilename)

      If Not e.Item.Name.ToLower.Contains("quickprint") Then
        pdfDocument.Print(pdfPrinterSettings)
      Else
        pdfDocument.Print(New PdfPrinterSettings)
      End If

      'Dim obj As New SP.PDFO2S.ClsMain_Net
      'strResult = obj.PrintSelectedPDFFile(m_MergePDFFilename, Not e.Item.Name.ToLower.Contains("quickprint"))

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    Finally
      SplashScreenManager.CloseForm(False)
    End Try

  End Sub

  Private Sub pcAttachments_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles pcAttachments.Popup
    Dim strResult As String = String.Empty

    If Me.clstMADoc.Items.Count > 0 Then Exit Sub
    Try


      Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
      Conn.Open()

			Dim sSql As String = "Select [ID], Bezeichnung From MA_LLDoc Where (ScanExtension = 'PDF' Or ScanExtension = '.PDF') And MANr = @MANr AND ISNull(Bezeichnung, '') <> ''"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MANr", EmployeeNumber)
      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

      Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Die Dokumente werden eingelesen...")
      Me.bsiInfo.Refresh()
      Me.clstMADoc.Items.Clear()

      If rFoundedrec.HasRows Then
        Dim strDeckblattZ As String = Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4ZeuginisDeckblatt)
        If File.Exists(strDeckblattZ) Then
          Me.clstMADoc.Items.Add(String.Format("{0}", m_Translate.GetSafeTranslationValue("Deckblatt Zeugnisse & Diplome anhängen")))
          Me.clstMADoc.Items(Me.clstMADoc.Items.Count - 1).CheckState = m_coversheetdiplomascheckboxvalue
        End If
      End If
      While rFoundedrec.Read
        Me.clstMADoc.Items.Add(m_Translate.GetSafeTranslationValue(String.Format("{0}", rFoundedrec("Bezeichnung").ToString)))
      End While

      Dim strDeckblattA As String = Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4AGBDeckblatt)
      Me.chkWithTAGB.Enabled = File.Exists(Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4AGBTemp))
      Me.chkWithFAGB.Enabled = File.Exists(Path.Combine(m_TemplatePath, _ClsProgSetting.GetFile4AGBFest))
      If Not chkWithTAGB.Enabled Then chkWithTAGB.Checked = False
      If Not chkWithFAGB.Enabled Then chkWithFAGB.Checked = False

      If Me.chkWithTAGB.Enabled OrElse Me.chkWithFAGB.Enabled Then
        If File.Exists(strDeckblattA) Then
          Me.clstMADoc.Items.Add(String.Format("{0}", m_Translate.GetSafeTranslationValue("Deckblatt AGB's anhängen")))
          Me.clstMADoc.Items(Me.clstMADoc.Items.Count - 1).CheckState = m_coversheetconditionscheckboxvalue
        End If
      End If


    Catch ex As Exception
      m_Logger.LogError(ex.ToString)

    Finally
      Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
    End Try

  End Sub

	Private Sub FileSaveItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles FileSaveItem1.ItemClick

    SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
    SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Daten werden gespeichert") & Space(20))
    SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Moment") & "...")

    'CreateNewLLRec() 

    SaveMyDoc()
		loadassignedCVDocument()


	End Sub

  Sub SaveMyDoc()

    Try
      If File.Exists(m_MergePDFFilename) Then File.Delete(m_MergePDFFilename)

    Catch ex As Exception
      Dim strMsg As String = "Fehler beim Löschen der Datei. Möglicherweise ist die Datei in Verwendung.{0}{1}{2}"
      m_UtilityUi.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, m_MergePDFFilename, ex.ToString))
      Return
    End Try

    Try
      Dim strResult As String = CreateLLMergeFile(m_MergePDFFilename)
      If strResult.ToLower.Contains("fehler") Then
        Dim strMsg As String = strResult
        DevExpress.XtraEditors.XtraMessageBox.Show(strMsg,
                                                     m_Translate.GetSafeTranslationValue("Datei speichern"),
                                                     MessageBoxButtons.OK,
                                                     MessageBoxIcon.Error)
        ClsDataDetail.strFilename4Print_Save = "Fehler..."
      Else
        ClsDataDetail.strFilename4Print_Save = m_MergePDFFilename

      End If
      ' Hier muss gespeichert werden...

    Catch ex As Exception
      m_Logger.LogError(ex.ToString)
      Dim strMsg As String = "Fehler beim Speichern der Daten. Möglicherweise ist die Datei in Verwendung.{0}{1}{2}"
      m_UtilityUi.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, m_MergePDFFilename, ex.ToString))

    End Try


  End Sub

	Private Sub LoadAssignedCVDocument()
		Dim data = m_EmployeeDatabaseAccess.LoadAssingedEmployeeCVData(EmployeeNumber, m_TemplateName, False)
		If data Is Nothing Then Return

		m_CurrentCVDocumentID = data.ID
		bbiDeleteCV.Enabled = Not m_CurrentCVDocumentID Is Nothing

	End Sub

	Private Sub rtContent_RtfTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtfContent.RtfTextChanged

		Me.rtfContent.Modified = True
		ClsDataDetail.bContentChanged = True
		Me.FileNewItem1.Enabled = True

	End Sub

	Private Sub beTemplate_ItemClick(ByVal sender As System.Object,
                                   ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles beTemplate.ItemClick

  End Sub

  Private Sub repTemplates_DrawItem(ByVal sender As Object,
                                    ByVal e As DevExpress.XtraEditors.ListBoxDrawItemEventArgs) Handles repTemplates.DrawItem

    If e.Item.ToString = "-" Then
      Dim r As Rectangle = e.Bounds
      r.Inflate(-1, -r.Height / 2 + 1)
      e.Graphics.FillRectangle(Brushes.Black, r)
      e.Handled = True
    End If

  End Sub

  Private Sub chkWithTAGB_CheckedChanged(ByVal sender As System.Object,
                                         ByVal e As System.EventArgs) Handles chkWithTAGB.CheckedChanged,
                                         chkWithFAGB.CheckedChanged

    If Not pcAttachments.Visible Then Return
    Dim bExistDeckblatt4AGB As Boolean = If(Me.clstMADoc.Items.Count = 0, False, Me.clstMADoc.Items(Me.clstMADoc.Items.Count - 1).ToString.ToLower.Contains(m_Translate.GetSafeTranslationValue("Deckblatt").ToLower))

    If chkWithTAGB.CheckState = CheckState.Checked OrElse chkWithFAGB.CheckState = CheckState.Checked Then
      If bExistDeckblatt4AGB Then Me.clstMADoc.Items(Me.clstMADoc.Items.Count - 1).CheckState = CheckState.Checked

    Else
      If bExistDeckblatt4AGB Then Me.clstMADoc.Items(Me.clstMADoc.Items.Count - 1).CheckState = CheckState.Unchecked

    End If

  End Sub

  Function LoadLLTemplateName(ByVal strFieldName As String) As List(Of LLTemplateData)
    Dim result As List(Of LLTemplateData) = Nothing
		Dim tplFilename As String
		Dim listAsTemplate As Boolean = True

		Try
			Dim sql As String
			sql = "Select * From tab_LLZusatzFields_Template Where DbFieldName = @DBFieldName"
			sql &= " Order By RecNr, Bezeichnung"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("DBFieldName", strFieldName))
			Dim reader As SqlClient.SqlDataReader = m_UtilityProg.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams)


			If (Not reader Is Nothing) Then

				result = New List(Of LLTemplateData)

				While reader.Read

					Dim data = New LLTemplateData()

					data.RecNr = m_UtilityProg.SafeGetInteger(reader, "RecNr", 0)
					data.DbFieldName = m_UtilityProg.SafeGetString(reader, "DbFieldName")
					data.Bezeichnung = m_UtilityProg.SafeGetString(reader, "Bezeichnung")
					data.FileName = m_UtilityProg.SafeGetString(reader, "FileName")

					tplFilename = String.Format("LL_{0}_{1}.{2}", data.FileName, LL_DBFIELD_NAME, GetLLExtentionValue)
					Dim templateFile = FindTemplateFile(tplFilename)
					listAsTemplate = Not String.IsNullOrWhiteSpace(templateFile)

					If listAsTemplate Then result.Add(data)


				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
      result = Nothing

    End Try

    Return result
  End Function


End Class


Public Class myCombobox
  Public _Capation As String
  Public _Value As String

  Public Sub New(ByVal strCaption As String, ByVal strValue As String)
    _Capation = strCaption
    _Value = strValue
  End Sub

  Public Overrides Function ToString() As String
    Return _Capation
  End Function
End Class



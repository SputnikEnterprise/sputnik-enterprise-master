
Imports System.IO
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraBars.Helpers

Imports DevExpress.XtraRichEdit.Services
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.MA.MassenVersand.ClsDataDetail

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Public Class frmMAMassenversand
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The common database access.
	''' </summary>
	Protected m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private m_SelectedLink As String = String.Empty
	Private m_SelectedLinkLabel As String = String.Empty
	Private oFontName = New Font("Calibri", 11, FontStyle.Regular)

	Private _ClsXML As New ClsXML

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_connectionString As String
	Private m_AssignedEmployeeData As EmployeeMasterData

#Region "public properties"

	Public Property EmployeeNumber As Integer


#End Region

#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		ClsDataDetail.m_InitialData = m_InitializationData
		ClsDataDetail.m_Translate = m_Translate

		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Me.rtfContent.Unit = DevExpress.Office.DocumentUnit.Centimeter ' DevExpress.XtraRichEdit.DocumentUnit.Centimeter
		Me.rtfContent.Document.Sections(0).Page.PaperKind = Printing.PaperKind.A4

		Me.rtfContent.RemoveShortcutKey(Keys.Control, Keys.O)
		rtfContent.RemoveShortcutKey(Keys.Control, Keys.N)
		rtfContent.RemoveShortcutKey(Keys.Control, Keys.S)

		ReplaceRichEditCommandFactoryService(Me.rtfContent)

		Reset()
		TranslateControls()

		AddHandler rtfContent.DocumentLoaded, AddressOf richEditControl1_DocumentLoaded

	End Sub

#End Region


#Region "public methodes"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		ClsDataDetail.GetMANumber = EmployeeNumber

		success = success AndAlso LoadEmployeeData()
		DisplayEmployeeData()

		Return success
	End Function

#End Region


#Region "reset"

	Private Sub Reset()

		bei_Template.Enabled = False
		BuildLLAbschnitt()

		Me.ChangeFontNameItem1.Caption = String.Empty
		Me.ChangeFontSizeItem1.Caption = String.Empty
		Me.siStatus.Caption = String.Empty
		Me.rtfContent.Text = String.Empty
		Me.rtfContent.Dock = DockStyle.Fill

		Try
			Dim iIndent As Integer = CInt(Val(_ClsProgSetting.GetMDProfilValue("Sonstiges", "LL_IndentSize", "20")))
			Dim ivalue As Integer = CInt(Val(_ClsProgSetting.GetMDProfilValue("Sonstiges", "FontSize", "11")))
			Dim strValue As String = _ClsProgSetting.GetMDProfilValue("Sonstiges", "FontName", "Calibri")

			oFontName = New Font(strValue, ivalue, FontStyle.Regular)
			Me.rtfContent.Font = oFontName
			Me.navBarControl.OptionsNavPane.ShowOverflowButton = False
			Me.navBarControl.OptionsNavPane.ShowOverflowPanel = False
			Me.navBarControl.OptionsNavPane.ShowExpandButton = False

		Catch ex As Exception

		End Try

		ResetTemplateDropDown()

	End Sub

	Private Sub ResetTemplateDropDown()

		rep_lueTemplate.DisplayMember = "Bezeichnung"
		rep_lueTemplate.ValueMember = "FileName"

		rep_lueTemplate.Columns.Clear()
		rep_lueTemplate.Columns.Add(New LookUpColumnInfo With {.FieldName = "Bezeichnung",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Vorlage")})

		rep_lueTemplate.ShowFooter = False
		rep_lueTemplate.DropDownRows = 10
		rep_lueTemplate.BestFitMode = BestFitMode.BestFitResizePopup
		rep_lueTemplate.SearchMode = SearchMode.AutoComplete
		rep_lueTemplate.AutoSearchColumnIndex = 0

		rep_lueTemplate.NullText = String.Empty
		rep_lueTemplate.DataSource = Nothing

		bei_Template.EditValue = Nothing

	End Sub

#End Region


	Private Function LoadEmployeeData() As Boolean
		Dim success As Boolean = True

		m_AssignedEmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(EmployeeNumber, False)
		If m_AssignedEmployeeData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mitarbeiterstammdaten konnten nicht geladen werden."))

			Return False
		End If

		Return success
	End Function

	Private Sub DisplayEmployeeData()

		Me.Text = String.Format("{0} - {1}", m_Translate.GetSafeTranslationValue("Zusätzliche Informationen "), m_AssignedEmployeeData.EmployeeFullname)

	End Sub

	Private Sub richEditControl1_DocumentLoaded(ByVal sender As Object, ByVal e As EventArgs)

		rtfContent.Font = CType(oFontName, System.Drawing.Font)
		rtfContent.Document.DefaultCharacterProperties.FontName = oFontName.Name
		rtfContent.Document.DefaultCharacterProperties.FontSize = oFontName.Size

	End Sub

	Private Sub ReplaceRichEditCommandFactoryService(ByVal control As DevExpress.XtraRichEdit.RichEditControl)
		Dim service As IRichEditCommandFactoryService = control.GetService(Of IRichEditCommandFactoryService)()

		If service Is Nothing Then Return
		control.RemoveService(GetType(IRichEditCommandFactoryService))
		control.AddService(GetType(IRichEditCommandFactoryService),
											 New CustomCommand.CustomRichEditCommandFactoryService(control, service))

	End Sub


	Function ContinueChanging(ByVal saveFile As Boolean) As MsgBoxResult
		Dim bResult As System.Windows.Forms.DialogResult ' MsgBoxResult

		If rtfContent.Modified Then
			Dim strMsg As String = "Möchten Sie die an Dokument vorgenommenen Änderungen speichern?"
			bResult = DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg),
																													 m_Translate.GetSafeTranslationValue("Daten speichern"),
																													 MessageBoxButtons.YesNoCancel,
																													 MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			If saveFile Then
				If bResult = MsgBoxResult.Yes Then SaveSelectedDbField(m_SelectedLink)
			End If
				Me.FileSaveAsItem1.Enabled = rtfContent.Modified
		Else
			Return MsgBoxResult.No
		End If

		Return bResult
	End Function

	Private Sub frmMAMassenversand_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Try
			If Me.WindowState <> FormWindowState.Minimized Then
				My.Settings.iHeight = Me.Height
				My.Settings.iWidth = Me.Width
				My.Settings.frmVersandLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

				My.Settings.frmLayoutName = DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName
				My.Settings.NavLayoutName = Me.navBarControl.PaintStyleName
				My.Settings.Save()
			End If

		Catch ex As Exception

		End Try
		Dim iResult As MsgBoxResult = ContinueChanging(True)

		'If iResult = MsgBoxResult.Cancel Then Me.pcc_Layout.Dispose()
		e.Cancel = iResult = MsgBoxResult.Cancel

	End Sub

	Private Sub frmMAMassenversand_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp

		If e.Control And e.KeyCode = Keys.S Then SaveSelectedDbField(m_SelectedLink)

	End Sub

	Sub TranslateControls()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			FileSaveItem1.Caption = m_Translate.GetSafeTranslationValue(FileSaveItem1.Caption)
			FindItem1.Caption = m_Translate.GetSafeTranslationValue(FindItem1.Caption)
			ReplaceItem1.Caption = m_Translate.GetSafeTranslationValue(ReplaceItem1.Caption)
			PasteItem1.Caption = m_Translate.GetSafeTranslationValue(PasteItem1.Caption)

			bei_Template.Caption = m_Translate.GetSafeTranslationValue(bei_Template.Caption)
			frpZwischenablage.Text = m_Translate.GetSafeTranslationValue(frpZwischenablage.Text)
			frpSchriftart.Text = m_Translate.GetSafeTranslationValue(frpSchriftart.Text)
			frpAbsatz.Text = m_Translate.GetSafeTranslationValue(frpAbsatz.Text)
			frpBearbeiten.Text = m_Translate.GetSafeTranslationValue(frpBearbeiten.Text)

			siStatus.Caption = m_Translate.GetSafeTranslationValue(siStatus.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub


	Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		Try
			If My.Settings.iHeight > 0 Then Me.Height = My.Settings.iHeight
			If My.Settings.iWidth > 0 Then Me.Width = My.Settings.iWidth
			If My.Settings.frmVersandLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmVersandLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub BuildLLAbschnitt()
		Dim liLLAbschnit As New List(Of String)

		Try
			liLLAbschnit = ListDBFieldsName()

		Catch ex As Exception

		End Try

		Dim groupLocal As DevExpress.XtraNavBar.NavBarGroup = New DevExpress.XtraNavBar.NavBarGroup(m_Translate.GetSafeTranslationValue("Abschnitte / Felder"))
		groupLocal.Name = "gNavMAZusatzFelder"

		Dim itemMA As New DevExpress.XtraNavBar.NavBarItem
		navBarControl.BeginUpdate()
		navBarControl.Groups.Add(groupLocal)

		Me.navBarControl.Items.Clear()
		Me.navBarControl.Refresh()
		For i As Integer = 0 To liLLAbschnit.Count - 1
			Dim strText As String() = liLLAbschnit(i).ToString.Split("#")

			Me.navBarControl.Groups(0).InsertItem(i).Item.Caption = m_Translate.GetSafeTranslationValue(Trim(strText(2)))
			Me.navBarControl.Groups(0).NavBar.Items(i).Name = Trim(strText(1))
		Next
		navBarControl.EndUpdate()

	End Sub

	Private Sub navBarControl_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navBarControl.LinkClicked

		Try
			If rtfContent.Modified Then
				Dim msgResult = ContinueChanging(False)
				If msgResult = MsgBoxResult.Cancel Then Return
				If msgResult = MsgBoxResult.Yes Then SaveSelectedDbField(m_SelectedLink)
			End If

			Me.siStatus.Caption = m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick...")
			Me.siStatus.Refresh()

			m_SelectedLink = e.Link.ItemName
			m_SelectedLinkLabel = e.Link.Caption
			For i As Integer = 0 To Me.navBarControl.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			e.Link.Item.Appearance.ForeColor = Color.Orange
			ListTemplates()
			DisplaySelectedDbField(m_SelectedLink)
			rtfContent.Modified = False

		Catch ex As Exception

		Finally
			Me.siStatus.Caption = String.Format("{0}", m_SelectedLinkLabel.ToString)

		End Try

	End Sub

	Private Sub iSave_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles FileSaveItem1.ItemClick

		Try
			Me.siStatus.Caption = m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick") & "..."

			If m_SelectedLink = String.Empty Then Exit Sub
			SaveSelectedDbField(m_SelectedLink)

		Catch ex As Exception

		Finally
			Me.siStatus.Caption = String.Format("{0}", m_SelectedLinkLabel.ToString)

		End Try

	End Sub

	Sub DisplaySelectedDbField(ByVal strFieldName As String)
		Dim strFieldValue As String = String.Empty

		If strFieldName <> String.Empty Then strFieldValue = GetDbFieldValue(strFieldName)
		If strFieldValue.ToLower.Contains("{\rtf1\") Then
			Me.rtfContent.RtfText = strFieldValue
		Else
			Me.rtfContent.Text = strFieldValue
		End If
		rtfContent.Modified = False

	End Sub

	Sub SaveSelectedDbField(ByVal strFieldName As String)
		Dim strFieldValue As String = Me.rtfContent.RtfText

		If strFieldName <> String.Empty Then strFieldValue = SaveDbFieldValue(strFieldName, Me.rtfContent.RtfText, Me.rtfContent.Text)
		rtfContent.Modified = False

	End Sub

	Sub ListTemplates()

		rep_lueTemplate.DataSource = Nothing
		bei_Template.EditValue = Nothing
		bei_Template.Enabled = False
		rtfContent.RtfText = Nothing

		Dim tplData = m_EmployeeDatabaseAccess.LoadLLZusatzFieldsTemplateData(m_SelectedLink)
		If tplData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Vorlage Daten konnten nicht geladen werden."))

			Return
		End If

		Dim listDataSource As BindingList(Of LLZusatzFieldsTemplateData) = New BindingList(Of LLZusatzFieldsTemplateData)
		For Each itm In tplData
			Dim listTemplate As Boolean = True
			Dim templateFilename As String = itm.FileName
			Dim templateFullfilename As String = FindExtendedTemplateFile("employee", templateFilename)

			listTemplate = Not String.IsNullOrWhiteSpace(templateFullfilename)

			If listTemplate Then
				listDataSource.Add(itm)
			Else

				m_Logger.LogWarning(String.Format("extended templates for {0} could not be founded! {1} >>> {2}", "employee", m_SelectedLink, templateFullfilename))
			End If

		Next

		rep_lueTemplate.DataSource = listDataSource
		bei_Template.Enabled = listDataSource.Count > 0

	End Sub

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
			End If
		End If
		If Not result Then templateFile = String.Empty

		Return templateFile
	End Function

	Private Sub rtfContent_RtfTextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtfContent.RtfTextChanged
		'Me.rtfContent.Modified = True
		'ClsDataDetail.bContentChanged = True
		'rtfContent.Modified = True
		'If String.IsNullOrWhiteSpace(m_SelectedLink) Then
		'	ClsDataDetail.bContentChanged = False
		'Else

		'	Dim strFieldValue = GetDbFieldValue(m_SelectedLink)
		'	If strFieldValue.ToLower.Contains("{\rtf1\") Then
		'		ClsDataDetail.bContentChanged = (rtfContent.RtfText = strFieldValue)
		'	Else
		'		ClsDataDetail.bContentChanged = (rtfContent.Text = strFieldValue)
		'	End If
		'End If

		Me.FileNewItem1.Enabled = True
	End Sub

	Private Sub rep_lueTemplate_CloseUp(sender As Object, e As CloseUpEventArgs) Handles rep_lueTemplate.CloseUp

		Try

			Dim data = CType(rep_lueTemplate.DataSource, BindingList(Of LLZusatzFieldsTemplateData))
			If data.Any(Function(tpl) tpl.FileName = e.Value) Then
				Dim tmpFilename As String = Path.Combine(m_InitializationData.MDData.MDTemplatePath, e.Value)
				If Not File.Exists(tmpFilename) Then
					m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Vorlage wurde nicht gefunden:{0}{1}"), vbNewLine, tmpFilename)
					rtfContent.RtfText = Nothing

					Return
				End If
				Dim test = rtfContent.LoadDocument(tmpFilename) ' Path.GetExtension(tmpFilename))
				'SetTemplateValues()
				rtfContent.Modified = True

			End If

		Catch ex As Exception

		End Try

	End Sub


	Private Class myCombobox
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


End Class




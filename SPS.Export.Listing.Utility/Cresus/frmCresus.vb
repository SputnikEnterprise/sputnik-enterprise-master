Imports System.Reflection.Assembly
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Imports System.IO
Imports System.Windows.Forms
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports System.Xml
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Public Class frmCresus


	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_xml As New ClsXML
	Private m_md As Mandant

	Private strConnString As String = String.Empty

	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath
	Private _docname As String

	Private Property _ExportSetting As New ClsCSVSettings
	Private Property SQL2Select() As String
	Private Property SQLFields() As String

	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI

	Public Property MetroForeColor As System.Drawing.Color
	Public Property MetroBorderColor As System.Drawing.Color


#Region "Fileexlorer..."

	Private Sub txtFilename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFilename.ButtonClick
		Dim t As Thread = New Thread(AddressOf ShowFolderBrowser)

		t.IsBackground = True
		t.Name = "FolderDialog"
		t.SetApartmentState(ApartmentState.STA)
		t.Start()

	End Sub

	Sub ShowFolderBrowser()
		Dim dialog As New FolderBrowserDialog()

		dialog.Description = m_xml.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei aus:")
		dialog.ShowNewFolderButton = True
		dialog.SelectedPath = If(Me.txtFilename.Text = String.Empty, _ClsProgSetting.GetSpSFiles2DeletePath, Path.GetDirectoryName(Me.txtFilename.Text))

		If dialog.ShowDialog() = DialogResult.OK Then
			Dim a As Action = Function() InlineAssignHelper(Me.txtFilename.Text, String.Format("{0}{1}Cre_Export.txt", dialog.SelectedPath,
																																												 If(dialog.SelectedPath.ToString.EndsWith("\"), "", "\")))
			Me.Invoke(a)
		End If

	End Sub

	Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
		target = value

		Return value
	End Function

#End Region


	Public Sub New(ByVal _Setting As ClsCSVSettings, ByVal _init As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _init

		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath
		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUI = New UtilityUI
		Me._ExportSetting = _Setting

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		'Dim modulName As String = _ExportSetting.ModulName.ToLower

		If _ExportSetting.ModulName.ToLower = "cresuslo" Then Me.txtFilename.Text = If(String.IsNullOrWhiteSpace(_Setting.ExportFileName), My.Settings.Filename4CresusLO, _Setting.ExportFileName)
		If _ExportSetting.ModulName.ToLower = "cresusop" Then Me.txtFilename.Text = If(String.IsNullOrWhiteSpace(_Setting.ExportFileName), My.Settings.Filename4CresusOP, _Setting.ExportFileName)
		If _ExportSetting.ModulName.ToLower = "cresusze" Then Me.txtFilename.Text = If(String.IsNullOrWhiteSpace(_Setting.ExportFileName), My.Settings.Filename4CresusZE, _Setting.ExportFileName)

		Try
			Dim xDoc As XDocument = XDocument.Load(_ClsProgSetting.GetMDData_XMLFile)
			Dim creConfigQuery = (From exportSetting In xDoc.Root.Elements("Interfaces")
														Select New With {
																.abafieldtrennzeichen = exportSetting.Element(String.Format("aba{0}fieldtrennzeichen", Me._ExportSetting.ModulName.Substring(3, 2))).Value,
																.abadarstellungszeichen = exportSetting.Element(String.Format("aba{0}darstellungszeichen", Me._ExportSetting.ModulName.Substring(3, 2))).Value,
																.abamwstcode = exportSetting.Element(String.Format("aba{0}mwstcode", Me._ExportSetting.ModulName.Substring(3, 2))).Value,
																.abagegenkonto = exportSetting.Element(String.Format("aba{0}gegenkonto", Me._ExportSetting.ModulName.Substring(3, 2))).Value,
																.abarefnr = exportSetting.Element(String.Format("aba{0}refnr", Me._ExportSetting.ModulName.Substring(3, 2))).Value
																	}).FirstOrDefault()

			'Me.cbo_Darstellungszeichen.Text = abaConfigQuery.abadarstellungszeichen
			'Me.cbo_Trennzeichen.Text = abaConfigQuery.abafieldtrennzeichen
			'Me.txtMwStCode.Text = abaConfigQuery.abamwstcode

			'Me.chkRef.Checked = m_Utility.ParseToBoolean(abaConfigQuery.abarefnr, False)
			'Me.chkGegenkostenart.Checked = m_Utility.ParseToBoolean(abaConfigQuery.abagegenkonto, False)

			TranslateControls()

		Catch ex As Exception
			'Me.cbo_Trennzeichen.Text = _Setting.FieldSeprator
			'Me.cbo_Darstellungszeichen.Text = _Setting.FieldIn
			'If Me.cbo_Trennzeichen.Text = String.Empty Then Me.cbo_Trennzeichen.Text = My.Settings.TrennzeichenAba

		End Try

	End Sub

	Private Sub frmCresus_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.frmCresusLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.iCresusWidth = Me.Width
			My.Settings.iCresusHeight = Me.Height
			'My.Settings.TrennzeichenCresus = Me.cbo_Trennzeichen.Text
			My.Settings.TrennzeichenCresus = Chr(9)

			If _ExportSetting.ModulName.ToLower = "cresuslo" Then My.Settings.Filename4CresusLO = Me.txtFilename.Text
			If _ExportSetting.ModulName.ToLower = "cresusop" Then My.Settings.Filename4CresusOP = Me.txtFilename.Text
			If _ExportSetting.ModulName.ToLower = "cresusze" Then My.Settings.Filename4CresusZE = Me.txtFilename.Text

			My.Settings.Save()
		End If

	End Sub

	Private Sub TranslateControls()

		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_xml.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.cmdOK.Text = m_xml.GetSafeTranslationValue(Me.cmdOK.Text)

		Me.lblHeader1.Text = m_xml.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_xml.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.lblDatei.Text = m_xml.GetSafeTranslationValue(Me.lblDatei.Text)

		'Me.lblFeldertrennen.Text = m_xml.GetSafeTranslationValue(Me.lblFeldertrennen.Text)
		'Me.lblFelderdarstellenin.Text = m_xml.GetSafeTranslationValue(Me.lblFelderdarstellenin.Text)
		'Me.lblMwstCode.Text = m_xml.GetSafeTranslationValue(Me.lblMwstCode.Text)
		'Me.chkGegenkostenart.Text = m_xml.GetSafeTranslationValue(Me.chkGegenkostenart.Text)
		'Me.chkRef.Text = m_xml.GetSafeTranslationValue(Me.chkRef.Text)

	End Sub

	Private Sub frmCresus_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsMainSetting.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				Me.Width = Math.Max(My.Settings.iCresusWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iCresusHeight, Me.Height)

				If My.Settings.frmCresusLocation <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmCresusLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				Else
					Me.Location = New System.Drawing.Point(0, 0)
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		Try
			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")
			'If String.IsNullOrWhiteSpace(Me.cbo_Darstellungszeichen.Text) Then Me.cbo_Darstellungszeichen.Text = Chr(34)
			'If String.IsNullOrWhiteSpace(Me.cbo_Trennzeichen.Text) Then Me.cbo_Trennzeichen.Text = ";"
			'Me.chkRef.Visible = _ExportSetting.ModulName.ToLower = "abalo"

			'Me.chkGegenkostenart.Visible = _ExportSetting.ModulName.ToLower <> "abalo"
			'Me.txtMwStCode.Visible = _ExportSetting.ModulName.ToLower <> "abalo"
			'Me.lblMwstCode.Visible = _ExportSetting.ModulName.ToLower <> "abalo"

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Felder füllen: {1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
		Dim strResult As String = String.Empty

		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bitte warten Sie einen Augenblick") & "..."
		If String.IsNullOrWhiteSpace(Me.txtFilename.Text) Then Me.txtFilename.Text = String.Format("{0}Cresus_Export.txt", _ClsProgSetting.GetSpSTempPath)

		_ExportSetting.ExportFileName = Me.txtFilename.Text
		'_ExportSetting.FieldIn = Me.cbo_Darstellungszeichen.Text
		'_ExportSetting.FieldSeprator = Me.cbo_Trennzeichen.Text
		'_ExportSetting.KDRefNrAsFKSoll = Me.chkRef.Checked
		'_ExportSetting.MitGegenKostenart = Me.chkGegenkostenart.Checked
		'_ExportSetting.MwStCode = Me.txtMwStCode.Text
		' Daten in XML schreiben
		SaveDataIntoXMLFile()

		If Me._ExportSetting.ModulName.ToLower = "cresuslo" Then
			Dim obj As New ExportCresus.ClsExportLOInCresus(_ExportSetting, m_InitializationData)
			strResult = obj.GetLOValue4Cresus()

		ElseIf Me._ExportSetting.ModulName.ToLower = "cresusop" Then
			Dim obj As New ExportCresus.ClsExportOPInCresus(_ExportSetting)
			strResult = obj.GetOPValue4Cresus()

		ElseIf Me._ExportSetting.ModulName.ToLower = "cresusze" Then
			Dim obj As New ExportCresus.ClsExportZEInCresus(_ExportSetting, m_InitializationData)
			strResult = obj.GetZEValue4Cresus()

		End If

		If File.Exists(strResult) Then
			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Die Datei wurde erfolgreich erstellt.")

		Else
			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(String.Format("Fehler: {0}", strResult))

		End If

	End Sub

	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	'Private Sub cbo_Trennzeichen_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Trennzeichen.QueryPopUp

	'	If Me.cbo_Trennzeichen.Properties.Items.Count > 0 Then
	'		Me.cbo_Trennzeichen.Properties.Items.Add(",")
	'		Me.cbo_Trennzeichen.Properties.Items.Add(";")
	'		Me.cbo_Trennzeichen.Properties.Items.Add("#")
	'	End If

	'End Sub

	'Private Sub cbo_Darstellungszeichen_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Darstellungszeichen.QueryPopUp

	'	If Me.cbo_Darstellungszeichen.Properties.Items.Count > 0 Then
	'		Me.cbo_Darstellungszeichen.Properties.Items.Add("'")
	'		Me.cbo_Darstellungszeichen.Properties.Items.Add(Chr(34))
	'	End If

	'End Sub

	''' <summary>
	''' Exportinformationen in XML Datei schreiben
	''' </summary>
	''' <remarks></remarks>
	Sub SaveDataIntoXMLFile()
		Dim strXMLFile As String = _ClsProgSetting.GetMDData_XMLFile
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		xDoc.Load(strXMLFile)

		xNode = xDoc.SelectSingleNode("*//Interfaces")
		If xNode Is Nothing Then
			xNode = xDoc.CreateNode(XmlNodeType.Element, "Interfaces", "")
			xDoc.DocumentElement.AppendChild(xNode)
		End If
		If xNode IsNot Nothing Then
			If TypeOf xNode Is XmlElement Then
				xElmntFamily = CType(xNode, XmlElement)
			End If
			Dim strNode As String = String.Format("cresus{0}fieldtrennzeichen", Me._ExportSetting.ModulName.Substring(3, 2))
			If xElmntFamily.SelectSingleNode(strNode) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNode))
			InsertTextNode(xDoc, xElmntFamily, strNode, Convert.ToChar(Keys.Tab))

			strNode = String.Format("cresus{0}darstellungszeichen", Me._ExportSetting.ModulName.Substring(3, 2))
			If xElmntFamily.SelectSingleNode(strNode) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNode))
			InsertTextNode(xDoc, xElmntFamily, strNode, String.Empty)

			strNode = String.Format("cresus{0}refnr", Me._ExportSetting.ModulName.Substring(3, 2))
			If xElmntFamily.SelectSingleNode(strNode) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNode))
			InsertTextNode(xDoc, xElmntFamily, strNode, False)

			strNode = String.Format("cresus{0}gegenkonto", Me._ExportSetting.ModulName.Substring(3, 2))
			If xElmntFamily.SelectSingleNode(strNode) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNode))
			InsertTextNode(xDoc, xElmntFamily, strNode, False)

			strNode = String.Format("cresus{0}mwstcode", Me._ExportSetting.ModulName.Substring(3, 2))
			If xElmntFamily.SelectSingleNode(strNode) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strNode))
			InsertTextNode(xDoc, xElmntFamily, strNode, String.Empty)

		End If
		xDoc.Save(strXMLFile)

	End Sub

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode,
														ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function

End Class
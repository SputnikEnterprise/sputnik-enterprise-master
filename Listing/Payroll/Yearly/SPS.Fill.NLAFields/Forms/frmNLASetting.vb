
Imports System.IO
Imports DevExpress.XtraEditors.Controls
Imports System.Windows.Forms
Imports System.Xml
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Public Class frmNLASetting

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private strMainKey As String = "//Lohnausweis_NLA"

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_InitialData As SP.Infrastructure.Initialization.InitializeClass
	Private Property NLATemplate() As String

	Private m_mandant As Mandant
	''' <summary>
	''' The cls prog path.
	''' </summary>
	Private m_path As ClsProgPath

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitialData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_mandant = New Mandant
		m_path = New ClsProgPath

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		translatecontrols()
		DialogResult = False

	End Sub


#End Region


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		xtabTextfelder.Text = m_Translate.GetSafeTranslationValue(xtabTextfelder.Text)
		xtabVorlage.Text = m_Translate.GetSafeTranslationValue(xtabVorlage.Text)
		grpTextFelder.Text = m_Translate.GetSafeTranslationValue(grpTextFelder.Text)
		grp2.Text = m_Translate.GetSafeTranslationValue(grp2.Text)

		Me.lbl2_3.Text = m_Translate.GetSafeTranslationValue(Me.lbl2_3.Text)
		Me.lbl3.Text = m_Translate.GetSafeTranslationValue(Me.lbl3.Text)
		Me.lbl4.Text = m_Translate.GetSafeTranslationValue(Me.lbl4.Text)
		Me.lbl7.Text = m_Translate.GetSafeTranslationValue(Me.lbl7.Text)

		grp13.Text = m_Translate.GetSafeTranslationValue(grp13.Text)
		Me.lbl13_1_2.Text = m_Translate.GetSafeTranslationValue(Me.lbl13_1_2.Text)
		Me.lbl13_2_3.Text = m_Translate.GetSafeTranslationValue(Me.lbl13_2_3.Text)
		Me.lbl14.Text = m_Translate.GetSafeTranslationValue(Me.lbl14.Text)
		Me.lbl15.Text = m_Translate.GetSafeTranslationValue(Me.lbl15.Text)

		grpDruckvorlage.Text = m_Translate.GetSafeTranslationValue(grpDruckvorlage.Text)
		Me.chkDisableUserName.Text = m_Translate.GetSafeTranslationValue(Me.chkDisableUserName.Text)
		Me.chkEnableEditing.Text = m_Translate.GetSafeTranslationValue(Me.chkEnableEditing.Text)
		Me.chkOpenpdffile.Text = m_Translate.GetSafeTranslationValue(Me.chkOpenpdffile.Text)
		Me.chkNewVersion2021.Text = m_Translate.GetSafeTranslationValue(Me.chkNewVersion2021.Text)

		Me.btnSaveTextValues.Text = m_Translate.GetSafeTranslationValue(Me.btnSaveTextValues.Text)

	End Sub

	Private Sub frmNLASetting_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

		Dim itemValues As Object() = New Object() {0, 1}
		Dim itemDescriptions As String() = New String() {m_Translate.GetSafeTranslationValue("Adresse rechts"), m_Translate.GetSafeTranslationValue("Adresse links")}
		Dim i As Integer = 0
		Do While i < itemValues.Length
			rgOrientation.Properties.Items.Add(New RadioGroupItem(itemValues(i), itemDescriptions(i)))
			i += 1
		Loop

		DisplaySettingValue()

	End Sub

	Sub DisplaySettingValue()

		Dim strXMLFile As String = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitialData.MDData.MDNr)
		Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Lohnausweis_NLA"
		Dim sValue As String = String.Empty
		Dim strKeyName As String = "NLA_2_3".ToLower
		Dim strQuery As String = String.Format("{0}/{1}", strMainKey, strKeyName)

		Try

			'Me.txt_2_3.Text = _ClsProgSetting.GetXMLValueByQuery(strXMLFile, strQuery, String.Empty)
			Me.txt_2_3.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_3_0".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_3.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_4_0".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_4.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_7_0".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_7.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_13_1_2".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_13_1_2.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_13_2_3".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_13_2_3.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_Nebenleistung_1".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_14_1.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_Nebenleistung_2".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_14_2.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_Bemerkung_1".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_15_1.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "NLA_Bemerkung_2".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Me.txt_15_2.Text = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			strKeyName = "Orientation".ToLower
			'strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			Dim strValue As String = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))

			If strValue = String.Empty Then
			ElseIf strValue = "1" Then
				rgOrientation.SelectedIndex = 1
			ElseIf strValue = "0" Then
				rgOrientation.SelectedIndex = 0
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


		Try
			strKeyName = "DisableUserName".ToLower
			Me.chkDisableUserName.Checked = If(m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName)) = "1", 1, 0)

			strKeyName = "EnableEditing".ToLower
			Me.chkEnableEditing.Checked = If(m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName)) = "1", 1, 0)

			strKeyName = "opennlapdffile".ToLower
			Me.chkOpenpdffile.Checked = If(m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName)) <> "true", False, True)
			strKeyName = "version2021".ToLower
			Me.chkNewVersion2021.Checked = If(m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName)) <> "true", False, True)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	Sub SaveNLATextValueSetting()
		Dim strXMLFile As String = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitialData.MDData.MDNr)
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing
		xDoc.Load(strXMLFile)
		Dim sKey As String = String.Empty

		xNode = xDoc.SelectSingleNode("*//Lohnausweis_NLA")
		If xNode Is Nothing Then
			xNode = xDoc.CreateNode(XmlNodeType.Element, "Lohnausweis_NLA", "")
			xDoc.DocumentElement.AppendChild(xNode)
		End If
		If xNode IsNot Nothing Then
			If TypeOf xNode Is XmlElement Then
				xElmntFamily = CType(xNode, XmlElement)
			End If
			' txt_2_3
			sKey = "NLA_2_3".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_2_3.Text)
			' txt_3
			sKey = "NLA_3_0".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_3.Text)
			' txt_4
			sKey = "NLA_4_0".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_4.Text)

			' txt_7
			sKey = "NLA_7_0".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_7.Text)
			' txt_13_1_2
			sKey = "NLA_13_1_2".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_13_1_2.Text)
			' txt_13_2_3
			sKey = "NLA_13_2_3".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_13_2_3.Text)

			' txt_14_1
			sKey = "NLA_Nebenleistung_1".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_14_1.Text)

			' txt_14_2
			sKey = "NLA_Nebenleistung_2".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_14_2.Text)

			' txt_15_1
			sKey = "NLA_Bemerkung_1".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_15_1.Text)
			' txt_15_2
			sKey = "NLA_Bemerkung_2".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, Me.txt_15_2.Text)

			sKey = "Orientation".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, If(Me.rgOrientation.SelectedIndex = 0, 0, If(Me.rgOrientation.SelectedIndex = 1, 1, 2)))

			sKey = "DisableUserName".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, If(Me.chkDisableUserName.Checked, 1, 0))

			sKey = "enableediting".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, If(Me.chkEnableEditing.Checked, 1, 0))

			sKey = "opennlapdffile".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, If(Me.chkOpenpdffile.Checked, "true", "false"))

			sKey = "version2021".ToLower
			If xElmntFamily.SelectSingleNode(sKey) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(sKey))
			InsertTextNode(xDoc, xElmntFamily, sKey, If(chkNewVersion2021.Checked, "true", "false"))

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

	Sub SaveNLAOrientationSetting()
		Dim strAdresse As String = "http://downloads.domain.com/sps_downloads/prog/forms/"
		Dim strFilename As String = Me.NLATemplate
		Dim bDownladnewFile As Boolean = True
		Dim sValue As Short = 2

		If sValue = 0 Then
			strFilename = String.Format("nla_r{0}.pdf", If(chkNewVersion2021.EditValue, "_2021", ""))

		ElseIf sValue = 1 Then
			strFilename = String.Format("nla_l{0}.pdf", If(chkNewVersion2021.EditValue, "_2021", ""))

		Else
			strFilename = String.Format("nla_r{0}.pdf", If(chkNewVersion2021.EditValue, "_2021", ""))

		End If
		If bDownladnewFile Then
			Try
				If Not File.Exists(String.Format("{0}{1}", _ClsProgSetting.GetMDDocPath, strFilename)) Then
					My.Computer.Network.DownloadFile(String.Format("{0}{1}", strAdresse, strFilename), String.Format("{0}{1}", _ClsProgSetting.GetMDDocPath, strFilename))
				Else
					m_Logger.LogWarning(String.Format("file exists: {0}", String.Format("{0}{1}", _ClsProgSetting.GetMDDocPath, strFilename)))
				End If

			Catch ex As Exception
				Dim strMsg As String = "Die Datei {0} konnte nicht heruntergeladen werden. Bitte  kontaktieren Sie Ihren Systemadministrator."

				strMsg = m_Translate.GetSafeTranslationValue(strMsg)
				m_UtilityUI.ShowErrorDialog(String.Format(strMsg, strFilename))

			End Try
		End If

	End Sub

	Private Sub btnSaveTextValues_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveTextValues.Click

		SaveNLATextValueSetting()
		SaveNLAOrientationSetting()

		Me.Close()

	End Sub

	Private Sub rgOrientation_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles rgOrientation.SelectedIndexChanged

		Me.NLATemplate = String.Format("nla_{0}.pdf", If(sender.SelectedIndex = 0, "r", If(sender.SelectedIndex = 1, "l", 2)))

	End Sub

	Private Sub frmNLASetting_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.Escape Then
			Me.Dispose()
		End If
	End Sub

End Class

Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Imports SP.Infrastructure.Logging
Imports System.IO
Imports DevExpress.XtraEditors.Controls
Imports DevComponents.DotNetBar
Imports System.Drawing
Imports DevComponents.DotNetBar.Metro.ColorTables

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports System.Data.SqlClient
Imports System.Xml

Public Class frmRPCPrintSetting

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML

  Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private _RPCSetting As New ClsRPCSetting
	Private strGuid As String = "SP.RPContent.PrintUtility"



	'Private ReadOnly Property PrintCusotmerDataOnReportsContentTemplate() As Boolean
	'	Get
	'		Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
	'		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

	'		Dim UserXMLFileName = m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
	'		Dim value As Boolean? = StrToBool(m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/printcustomerdataonreporttemplate", FORM_XML_MAIN_KEY)))
	'		If Not value.HasValue Then value = True


	'		Return value
	'	End Get

	'End Property


	Sub TranslateControls()

		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
		Me.btnSaveTextValues.Text = m_xml.GetSafeTranslationValue(Me.btnSaveTextValues.Text)

		Me.grpEinstellung.Text = m_xml.GetSafeTranslationValue(Me.grpEinstellung.Text)

		Me.lblExportDateipfad.Text = m_xml.GetSafeTranslationValue(Me.lblExportDateipfad.Text)
		Me.lblExportDateiname.Text = m_xml.GetSafeTranslationValue(Me.lblExportDateiname.Text)
		Me.lblDateizusammen.Text = m_xml.GetSafeTranslationValue(Me.lblDateizusammen.Text)

		Me.lblAnmerkung1.Text = m_xml.GetSafeTranslationValue(Me.lblAnmerkung1.Text)
		Me.lblAnmerkung2.Text = m_xml.GetSafeTranslationValue(Me.lblAnmerkung2.Text)

	End Sub


	Private Sub frmLOPrintSetting_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sValue As String = String.Empty
		Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
		Dim strKeyName As String = String.Empty ' "ExportPfad".ToLower
		Dim strQuery As String = String.Empty ' Format(strMainKey, Chr(34), strGuid, strKeyName)

		TranslateControls()


		Try
      m_xml.GetChildChildBez(Me)

    Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formtranslation.{1}", strMethodeName, ex.Message))

		End Try
    Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
    Me.KeyPreview = True


		strKeyName = "printcustomerdataonreporttemplate".ToLower
		strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		sValue = "true"
		chkPrintCustomerData.Checked = ParseToBoolean(_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue), True)

		sValue = String.Empty
		strKeyName = "ExportPfad".ToLower
		strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
		sValue = strBez
		Me.cbo_ExportPfad.Text = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
		strBez = String.Empty

    strKeyName = "ExportFilename".ToLower
    strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
    sValue = strBez
    Me.txt_ExportFile.Text = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)

    strBez = String.Empty
    strKeyName = "ExportFinalFileFilename".ToLower
    strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
    sValue = strBez
    Me.txt_ExportFinalFile.Text = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)

  End Sub

  Private Sub frmLOPrintSetting_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    If e.KeyCode = Keys.Escape Then
      Me.Dispose()
    End If
  End Sub

  Private Sub btnSaveTextValues_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveTextValues.Click
    Dim strXMLFile As String = _ClsProgSetting.GetUserProfileFile
    Dim xDoc As XmlDocument = New XmlDocument()
    Dim xNode As XmlNode
    Dim xElmntFamily As XmlElement = Nothing
    Dim strMainKey As String = "//ExportSetting[@Name='{0}']"
    xDoc.Load(strXMLFile)


    ' Metro Design anpassen...
    xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
    If xNode Is Nothing Then
      Dim newNode As Xml.XmlElement = xDoc.CreateElement("ExportSetting")

      newNode.SetAttribute("Name", strGuid)
      xDoc.DocumentElement.AppendChild(newNode)
      xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
    End If
    If xNode IsNot Nothing Then
      If TypeOf xNode Is XmlElement Then
        xElmntFamily = CType(xNode, XmlElement)
      End If
      Dim strKeyName As String = String.Empty

			strKeyName = "printcustomerdataonreporttemplate".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, IIf(chkPrintCustomerData.Checked, "true", "false"))

			strKeyName = "ExportPfad".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.cbo_ExportPfad.Text))

      strKeyName = "ExportFilename".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.txt_ExportFile.Text))

      strKeyName = "ExportFinalFileFilename".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.txt_ExportFinalFile.Text))

    End If
    xDoc.Save(strXMLFile)

    Me.Dispose()

  End Sub

  Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode, _
                            ByVal strTag As String, ByVal strText As String) As XmlElement
    Dim xNodeTemp As XmlNode

    xNodeTemp = xDoc.CreateElement(strTag)
    xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
    xNode.AppendChild(xNodeTemp)

    Return CType(xNodeTemp, XmlElement)
  End Function

  Private Sub cbo_ExportPfad_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cbo_ExportPfad.ButtonClick
    Dim dialog As New FolderBrowserDialog()

    dialog.Description = m_xml.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei aus:")
    dialog.ShowNewFolderButton = True
    dialog.SelectedPath = If(Me.cbo_ExportPfad.Text = String.Empty, _ClsProgSetting.GetSpSFiles2DeletePath, _
                             Path.GetDirectoryName(Me.cbo_ExportPfad.Text))
    If dialog.ShowDialog() = DialogResult.OK Then
      Me.cbo_ExportPfad.Text = String.Format("{0}\", dialog.SelectedPath)
    End If

  End Sub


#Region "helpers"

	Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function


#End Region
End Class
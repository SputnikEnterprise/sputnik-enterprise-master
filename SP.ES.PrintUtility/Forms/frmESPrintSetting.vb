
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings

Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Imports System.IO
Imports DevExpress.XtraEditors.Controls
Imports DevComponents.DotNetBar
Imports System.Drawing
Imports DevComponents.DotNetBar.Metro.ColorTables

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports System.Data.SqlClient
Imports System.Xml
Imports SP.Infrastructure.Logging

Public Class frmESPrintSetting

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Dim m_xml As New ClsXML

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting

	Private _ESSetting As New ClsESSetting

	Private Property PrintJobNr4ESVertrag As String
	Private Property PrintJobNr4Verleih As String


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant
		m_utility = New Utilities

	End Sub

#End Region


	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount

		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
		Me.btnSaveTextValues.Text = m_xml.GetSafeTranslationValue(Me.btnSaveTextValues.Text)

		Me.grpEinstellung.Text = m_xml.GetSafeTranslationValue(Me.grpEinstellung.Text)

		Me.grpAnzahl.Text = m_xml.GetSafeTranslationValue(Me.grpAnzahl.Text)
		Me.lblEinsatzvertrag.Text = m_xml.GetSafeTranslationValue(Me.lblEinsatzvertrag.Text)
		Me.lblVerleihvertrag.Text = m_xml.GetSafeTranslationValue(Me.lblVerleihvertrag.Text)

		Me.grpUnterzeichner.Text = m_xml.GetSafeTranslationValue(Me.grpUnterzeichner.Text)
		Me.chkUnterzeichner.Text = m_xml.GetSafeTranslationValue(Me.chkUnterzeichner.Text)
		Me.lblAktiviert.Text = m_xml.GetSafeTranslationValue(Me.lblAktiviert.Text)

		Me.grpExport.Text = m_xml.GetSafeTranslationValue(Me.grpExport.Text)
		Me.lblDateipfad.Text = m_xml.GetSafeTranslationValue(Me.lblDateipfad.Text)
		Me.lblExportEinsatzvertrag.Text = m_xml.GetSafeTranslationValue(Me.lblExportEinsatzvertrag.Text)
		Me.lblExportVerleih.Text = m_xml.GetSafeTranslationValue(Me.lblExportVerleih.Text)
		Me.lblDateizusammenfuegen.Text = m_xml.GetSafeTranslationValue(Me.lblDateizusammenfuegen.Text)
		Me.lblESVertragDatei.Text = m_xml.GetSafeTranslationValue(Me.lblESVertragDatei.Text)
		Me.lblVerleihvertragDatei.Text = m_xml.GetSafeTranslationValue(Me.lblVerleihvertragDatei.Text)

		Me.lblAnmerkung1.Text = m_xml.GetSafeTranslationValue(Me.lblAnmerkung1.Text)
		Me.lblAnmerkung2.Text = m_xml.GetSafeTranslationValue(Me.lblAnmerkung2.Text)

	End Sub

	Private Sub frmLOPrintSetting_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sValue As String = String.Empty
		Dim strGuid As String = "SP.ES.PrintUtility"
		Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
		Dim strKeyName As String = "ExportPfad".ToLower
		Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		Dim strBez As String = String.Empty

		StartTranslation()
		Try

			strBez = m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr), strQuery, String.Empty)
			'm_md.GetLogedUSProfilValue(ClsDataDetail.MDData.MDNr, strKeyName, ClsDataDetail.UserData.UserNr)
			'_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			sValue = strBez
		Catch ex As Exception

		End Try

		Try
			m_xml.GetChildChildBez(Me)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formtranslation.{1}", strMethodeName, ex.Message))

		End Try
		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
		Me.PrintJobNr4ESVertrag = "4.3"
		Me.PrintJobNr4Verleih = "4.2"
		Me.KeyPreview = True
		StyleManager.MetroColorGeneratorParameters = New MetroColorGeneratorParameters(_ESSetting.MetroForeColor, _
																																									 _ESSetting.MetroBorderColor)
		DisplayFoundedDocData()

		Me.cbo_ExportPfad.Text = m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr), strQuery, String.Empty)
		'_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)

		strBez = String.Empty
		strKeyName = "ESUnterzeichner_ESVertrag".ToLower
		strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		sValue = strBez
		Me.chkUnterzeichner.Checked = If(m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr), strQuery, 0) = 1, True, False)
		'Me.chkUnterzeichner.Checked = If(_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue) = 1, True, False)

		strBez = String.Empty
		strKeyName = "ExportFilename_ESVertrag".ToLower
		strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		sValue = strBez
		Me.txt_ExportFileESVertrag.Text = m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr), strQuery, String.Empty)
		'_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)

		strBez = String.Empty
		strKeyName = "ExportFilename_Verleih".ToLower
		strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		sValue = strBez
		Me.txt_ExportFileVerleihVertrag.Text = m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr), strQuery, String.Empty)
		'_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)

		strBez = String.Empty
		strKeyName = "ExportFinalFileFilename_ESVertrag".ToLower
		strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		sValue = strBez
		Me.txt_ExportFinalFileESVertrag.Text = m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr), strQuery, String.Empty)
		'_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)

		strBez = String.Empty
		strKeyName = "ExportFinalFileFilename_Verleih".ToLower
		strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
		sValue = strBez
		Me.txt_ExportFinalFileVerleih.Text = m_utility.GetXMLValueByQueryWithFilename(m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr), strQuery, String.Empty)
		'_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)

	End Sub

	Private Sub frmLOPrintSetting_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.Escape Then
			Me.Dispose()
		End If
	End Sub

	Sub DisplayFoundedDocData()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim _ClsDb As New ClsDbFunc

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			Dim strQuery As String = "Select Top 2 JobNr, Anzahlkopien From DokPrint Where JobNr In (@JobNr4ESVertrag, @JobNr4Verleih) Order By JobNr"
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@JobNr4ESVertrag", Me.PrintJobNr4ESVertrag)
			param = cmd.Parameters.AddWithValue("@JobNr4Verleih", Me.PrintJobNr4Verleih)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader
			While rAdressrec.Read
				If rAdressrec("JobNr") = Me.PrintJobNr4ESVertrag Then
					Me.txt_AnzKopienESVertrag.Value = Math.Max(CInt(rAdressrec("Anzahlkopien")), 1)

				Else
					Me.txt_AnzKopienVerleih.Value = Math.Max(CInt(rAdressrec("Anzahlkopien")), 1)

				End If

			End While
			If Me.txt_AnzKopienESVertrag.Value = 0 Then Me.txt_AnzKopienESVertrag.Value = 1
			If Me.txt_AnzKopienVerleih.Value = 0 Then Me.txt_AnzKopienVerleih.Value = 1

		Catch e As Exception
			If Me.txt_AnzKopienESVertrag.Value = 0 Then Me.txt_AnzKopienESVertrag.Value = 1
			If Me.txt_AnzKopienVerleih.Value = 0 Then Me.txt_AnzKopienVerleih.Value = 1
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.Message, MsgBoxStyle.Critical, strMethodeName)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub SaveFoundedDocData()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			Dim strQuery As String = "Update DokPrint Set Anzahlkopien = @AnzKopienESVertrag Where JobNr = @JobNrESVertrag "
			strQuery &= "Update DokPrint Set Anzahlkopien = @AnzKopienVerleih Where JobNr = @JobNrVerleih"
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@AnzKopienESVertrag", Me.txt_AnzKopienESVertrag.Value)
			param = cmd.Parameters.AddWithValue("@AnzKopienVerleih", Me.txt_AnzKopienVerleih.Value)
			param = cmd.Parameters.AddWithValue("@JobNrESVertrag", Me.PrintJobNr4ESVertrag)
			param = cmd.Parameters.AddWithValue("@JobNrVerleih", Me.PrintJobNr4Verleih)

			cmd.ExecuteNonQuery()

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.Message, MsgBoxStyle.Critical, strMethodeName)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Private Sub btnSaveTextValues_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveTextValues.Click
		Dim sAnzKopien As Short = CShort(Me.txt_AnzKopienESVertrag.Value)

		SaveFoundedDocData()

		Dim strXMLFile As String = m_md.GetSelectedMDUserProfileXMLFilename(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr)
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing
		Dim strMainKey As String = "//ExportSetting[@Name='{0}']"
		Dim strGuid As String = "SP.ES.PrintUtility"
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

			strKeyName = "ESUnterzeichner_ESVertrag".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, If(Me.chkUnterzeichner.Checked, 1, 0))

			strKeyName = "ExportPfad".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.cbo_ExportPfad.Text))

			strKeyName = "ExportFilename_ESVertrag".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.txt_ExportFileESVertrag.Text))

			strKeyName = "ExportFilename_Verleih".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.txt_ExportFileVerleihVertrag.Text))

			strKeyName = "ExportFinalFileFilename_ESVertrag".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.txt_ExportFinalFileESVertrag.Text))

			strKeyName = "ExportFinalFileFilename_Verleih".ToLower
			If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
						xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
			InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.txt_ExportFinalFileVerleih.Text))

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
		dialog.SelectedPath = If(Me.cbo_ExportPfad.Text = String.Empty, m_utility.GetSpS2DeleteHomeFolder, Path.GetDirectoryName(Me.cbo_ExportPfad.Text))
		If dialog.ShowDialog() = DialogResult.OK Then
			Me.cbo_ExportPfad.Text = String.Format("{0}\", dialog.SelectedPath)
		End If

	End Sub



End Class
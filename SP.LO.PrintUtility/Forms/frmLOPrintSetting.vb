
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Imports SP.Infrastructure.Logging
Imports System.IO
Imports DevExpress.XtraEditors.Controls
Imports DevComponents.DotNetBar
Imports System.Drawing
Imports DevComponents.DotNetBar.Metro.ColorTables

Imports System.Xml

Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.LO.PrintUtility.ClsDataDetail
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.Infrastructure.UI

Public Class frmLOPrintSetting


#Region "private consts"

	Private Const USER_XML_SETTING_SPUTNIK_EXPORT_SETTING As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
	Private Const USER_MODUL_GUID As String = "SP.LO.PrintUtility"

#End Region


#Region "private fields"


	''' <summary>
	''' The logger.
	''' </summary>
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
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess

	Private m_path As New ClsProgPath
	Private m_md As New Mandant

	Private _LOSetting As New ClsLOSetting

	Private Property PrintJobNr As String
	Private m_connectionString As String
	Private Property m_UserXMLProfileFile As String
	Private m_DocumentData As MandantDocumentData

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_UtilityUI = New UtilityUI

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()


		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_connectionString = m_InitializationData.MDData.MDDbConn

		m_UserXMLProfileFile = m_md.GetSelectedMDUserProfileXMLFilename(m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr)
		m_TablesettingDatabaseAccess = New TablesDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		TranslateControls()

	End Sub

#End Region


#Region "Public methods"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		success = success AndAlso LoadDocumentData()
		success = success AndAlso LoadXMLData()


		Return success
	End Function

#End Region

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		xtabMain.Text = m_Translate.GetSafeTranslationValue(xtabMain.Text)
		Me.btnSaveTextValues.Text = m_Translate.GetSafeTranslationValue(Me.btnSaveTextValues.Text)

		grpEinstellungen.Text = m_Translate.GetSafeTranslationValue(grpEinstellungen.Text)
		grpDruckLohnabrechnung.Text = m_Translate.GetSafeTranslationValue(grpDruckLohnabrechnung.Text)

		lblAnzahlkopien.Text = m_Translate.GetSafeTranslationValue(lblAnzahlkopien.Text)
		lblfuerallevorlagen.Text = m_Translate.GetSafeTranslationValue(lblfuerallevorlagen.Text)

		grpExport.Text = m_Translate.GetSafeTranslationValue(grpExport.Text)
		lblExportpfad.Text = m_Translate.GetSafeTranslationValue(lblExportpfad.Text)
		lblExportDateiname.Text = m_Translate.GetSafeTranslationValue(lblExportDateiname.Text)

		lblDateienzusammenfassung.Text = m_Translate.GetSafeTranslationValue(lblDateienzusammenfassung.Text)

		lblAnmerkung1.Text = m_Translate.GetSafeTranslationValue(lblAnmerkung1.Text)
		lblAnmerkung2.Text = m_Translate.GetSafeTranslationValue(lblAnmerkung2.Text)

	End Sub


	Private Function LoadDocumentData() As Boolean
		m_DocumentData = m_TablesettingDatabaseAccess.LoadMandantAssignedDocumentData("9.1")

		If (m_DocumentData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokumentendaten konnten nicht geladen werden."))
			Return False
		End If
		Me.txt_AnzKopien.Value = Math.Max(m_DocumentData.Anzahlkopien.GetValueOrDefault(0), 1)


		Return Not m_DocumentData Is Nothing
	End Function

	Private Function LoadXMLData() As Boolean
		Dim success As Boolean = True
		Dim sValue As String = String.Empty
		Dim strKeyName As String = "ExportPfad".ToLower
		Dim strQuery As String = String.Format(USER_XML_SETTING_SPUTNIK_EXPORT_SETTING, Chr(34), USER_MODUL_GUID, strKeyName)

		Try

			Dim strbez = m_path.GetXMLNodeValue(m_UserXMLProfileFile, strQuery)
			sValue = strbez

			Me.cbo_ExportPfad.Text = m_path.GetXMLNodeValue(m_UserXMLProfileFile, strQuery)

			strKeyName = "ExportFilename".ToLower
			strQuery = String.Format(USER_XML_SETTING_SPUTNIK_EXPORT_SETTING, Chr(34), USER_MODUL_GUID, strKeyName)
			sValue = String.Empty
			Me.txt_ExportFile.Text = m_path.GetXMLNodeValue(m_UserXMLProfileFile, strQuery)

			strKeyName = "ExportFinalFileFilename".ToLower
			strQuery = String.Format(USER_XML_SETTING_SPUTNIK_EXPORT_SETTING, Chr(34), USER_MODUL_GUID, strKeyName)
			sValue = String.Empty
			Me.txt_ExportFinalFile.Text = m_path.GetXMLNodeValue(m_UserXMLProfileFile, strQuery)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			success = False

		End Try


		Return success
	End Function

	Private Sub frmLOPrintSetting_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

		Me.KeyPreview = True
		StyleManager.MetroColorGeneratorParameters = New MetroColorGeneratorParameters(_LOSetting.MetroForeColor, _LOSetting.MetroBorderColor)

	End Sub

	Private Sub frmLOPrintSetting_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.Escape Then
			Me.Dispose()
		End If
	End Sub

	'Sub DisplayFoundedDocData(ByVal strJobNr As String)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim i As Integer = 0
	'	Dim _ClsDb As New ClsDbFunc

	'	Try
	'		Conn.Open()
	'		Dim cmd As System.Data.SqlClient.SqlCommand
	'		Dim strQuery As String = "Select Top 1 * From DokPrint Where JobNr = @JobNr"
	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@JobNr", strJobNr)

	'		Dim rAdressrec As SqlDataReader = cmd.ExecuteReader
	'		While rAdressrec.Read
	'			Me.txt_AnzKopien.Value = Math.Max(CInt(rAdressrec("Anzahlkopien")), 1)

	'		End While
	'		If Me.txt_AnzKopien.Value = 0 Then Me.txt_AnzKopien.Value = 1

	'	Catch e As Exception
	'		If Me.txt_AnzKopien.Value = 0 Then Me.txt_AnzKopien.Value = 1
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
	'		MsgBox(e.Message, MsgBoxStyle.Critical, strMethodeName)

	'	Finally
	'		Conn.Close()
	'		Conn.Dispose()

	'	End Try

	'End Sub

	Private Sub btnSaveTextValues_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveTextValues.Click
		Dim success As Boolean = True

		success = success AndAlso SaveFoundedDocData()
		success = success AndAlso SaveXMLData()

		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden."))
		End If
		Me.Dispose()

	End Sub

	Private Function SaveFoundedDocData() As Boolean
		Dim success As Boolean = True

		If m_DocumentData Is Nothing Then Return False

		Dim data = m_DocumentData
		data.Anzahlkopien = Math.Max(Val(txt_AnzKopien.EditValue), 1)

		success = success AndAlso m_TablesettingDatabaseAccess.UpdateAssignedMandantDocumentData(data)

		Return success
	End Function

	Private Function SaveXMLData() As Boolean
		Dim success As Boolean = True

		Dim strXMLFile As String = m_UserXMLProfileFile
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing
		Dim strMainKey As String = "//ExportSetting[@Name='{0}']"
		xDoc.Load(strXMLFile)

		Try
			xNode = xDoc.SelectSingleNode(String.Format(strMainKey, USER_MODUL_GUID))
			If xNode Is Nothing Then
				Dim newNode As Xml.XmlElement = xDoc.CreateElement("ExportSetting")

				newNode.SetAttribute("Name", USER_MODUL_GUID)
				xDoc.DocumentElement.AppendChild(newNode)
				xNode = xDoc.SelectSingleNode(String.Format(strMainKey, USER_MODUL_GUID))
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				Dim strKeyName As String = String.Empty

				strKeyName = "ExportPfad".ToLower
				If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
				InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.cbo_ExportPfad.Text))

				strKeyName = "ExportFilename".ToLower
				If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
				If Not txt_ExportFile.Text.Contains("{0}") Then txt_ExportFile.EditValue = String.Empty
				InsertTextNode(xDoc, xElmntFamily, strKeyName, Me.txt_ExportFile.Text)

				strKeyName = "ExportFinalFileFilename".ToLower
				If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
				If Not txt_ExportFinalFile.Text.Contains("{0}") Then txt_ExportFinalFile.EditValue = String.Empty
				InsertTextNode(xDoc, xElmntFamily, strKeyName, (Me.txt_ExportFinalFile.Text))

			End If
			xDoc.Save(strXMLFile)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
			success = False

		End Try

		Return success
	End Function
	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode,
														ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function

	Private Sub cbo_ExportPfad_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cbo_ExportPfad.ButtonClick
		Dim dialog As New FolderBrowserDialog()

		dialog.Description = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei aus:")
		dialog.ShowNewFolderButton = True
		dialog.SelectedPath = If(Me.cbo_ExportPfad.Text = String.Empty, m_path.GetSpS2DeleteHomeFolder,
														 Path.GetDirectoryName(Me.cbo_ExportPfad.Text))
		If dialog.ShowDialog() = DialogResult.OK Then
			Me.cbo_ExportPfad.Text = String.Format("{0}\", dialog.SelectedPath)
		End If

	End Sub




End Class


Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
'Imports SP.MA.EmployeeMng.UI
Imports System.Threading


Module FuncOpenProg


#Region "Private Fields"

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
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' translate values
	''' </summary>
	''' <remarks></remarks>
	Private m_translate As TranslateValues
	Private m_md As Mandant

	Private _ClsFunc As New ClsDivFunc
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg


#End Region

	Private Property SelectedMANr As Integer


	Function GetMenuItems4Export() As List(Of String)
		Dim sql As String = String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr", _
																							ClsDataDetail.GetAppGuidValue)
		Dim liResult As New List(Of String)

		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMnurec As SqlDataReader = cmd.ExecuteReader

			While rMnurec.Read

				liResult.Add(String.Format("{0}#{1}#{2}", m_translate.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString),
																		 m_translate.GetSafeTranslationValue(rMnurec("MnuName").ToString),
																		 m_translate.GetSafeTranslationValue(rMnurec("Docname").ToString)))

			End While


		Catch e As Exception
			m_Logger.LogError(e.Message)
			m_UtilityUI.ShowErrorDialog(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult

	End Function


#Region "Funktionen für Exportieren..."

	Private Function ShowMyFileDlg(ByVal strFile2Search As String) As String
		Dim strFullFileName As String = String.Empty
		Dim strFilePath As String = String.Empty
		Dim myStream As Stream = Nothing
		Dim openFileDialog1 As New OpenFileDialog()

		openFileDialog1.Title = strFile2Search
		openFileDialog1.InitialDirectory = strFile2Search
		openFileDialog1.Filter = "EXE-Dateien (*.exe)|*.exe|Alle Dateien (*.*)|*.*"
		openFileDialog1.FilterIndex = 1
		openFileDialog1.RestoreDirectory = True

		If openFileDialog1.ShowDialog() = DialogResult.OK Then
			Try

				myStream = openFileDialog1.OpenFile()
				If (myStream IsNot Nothing) Then
					strFullFileName = openFileDialog1.FileName()

					' Insert code to read the stream here.
				End If

			Catch Ex As Exception
				MessageBox.Show("Kann keine Daten lesen: " & Ex.Message)
			Finally
				' Check this again, since we need to make sure we didn't throw an exception on open.
				If (myStream IsNot Nothing) Then
					myStream.Close()
				End If
			End Try
		End If

		Return strFullFileName
	End Function

	Sub RunSMSProg(ByVal strQuery As String)

		' Umstellung von der neuen SQL-Query wieder zur alten Version.
		strQuery = strQuery.Replace("MANachname", "Nachname").Replace("MAVorname", "Vorname")

		Dim strProgPath As String
		Dim strSMSProgName As String = "Sputnik Suite SMS.EXE"
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", _
							 "SQLQuery", strQuery)

		Dim strSMSFile As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg")
		If strSMSFile = String.Empty Then
			strProgPath = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath")
			strProgPath = _ClsReg.AddDirSep(strProgPath) & "Binn\"

			If strSMSFile = String.Empty Then strSMSFile = strProgPath & strSMSProgName
		End If

		If Not File.Exists(strSMSFile) Then
			MsgBox("Folgende Datei wurde nicht gefunden. Bitte wählen Sie das Programm aus." & vbLf & _
					(strSMSFile), MsgBoxStyle.Critical, "Programm wurde nicht gefunden")

			strSMSFile = ShowMyFileDlg(strSMSFile)
			If strSMSFile <> String.Empty Then
				_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg", strSMSFile)
				Process.Start(strSMSFile)
			End If

		Else
			Process.Start(strSMSFile)

		End If

	End Sub


#End Region



	Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Dim bResult As Boolean = False

		' alle geöffneten Forms durchlaufen
		For Each oForm As Form In Application.OpenForms
			If oForm.Name.ToLower = sName.ToLower Then
				If bDisposeForm Then oForm.Dispose() : Exit For
				bResult = True : Exit For
			End If
		Next

		Return (bResult)
	End Function

	Function ExtraRights(ByVal lModulNr As Integer) As Boolean
		Dim bAllowed As Boolean
		Dim strModulCode As String

		' 10200        ' Fremdrechnung
		' 10201        ' Rapportinhalt
		' 10202        ' Export nach Abacus
		' 10206        ' Export nach Sesam
		strModulCode = _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile, "ExtraModuls", CStr(lModulNr))
		If InStr(1, strModulCode, "+" & lModulNr & "+") > 0 Then bAllowed = True

		ExtraRights = bAllowed

	End Function


	Sub MailTo(ByVal An As String, Optional ByVal Betreff As String = "")
		System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject={1}", An, Betreff))
	End Sub


End Module

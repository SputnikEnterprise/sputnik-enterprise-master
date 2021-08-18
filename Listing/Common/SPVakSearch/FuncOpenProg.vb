
Option Strict Off

Imports SPProgUtility.SPUserSec.ClsUserSec

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SP.KD.CustomerMng.UI
Imports SP.KD.CPersonMng.UI
Imports SP.Infrastructure.Logging

Module FuncOpenProg

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_xml As New ClsXML



#Region "Funktionen für Exportieren..."

	Sub RunBewModul(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSBewUtility.ClsMain")
			oMyProg.OpenKDFieldsform(strTempSQL)

		Catch e As Exception

		End Try

	End Sub


	''' <summary>
	''' Vakanzenverwaltung öffnen
	''' </summary>
	''' <param name="iVakNr"></param>
	''' <remarks></remarks>
	Sub RunOpenVAKForm(ByVal iVakNr As Integer, ByVal kdNr As Integer, ByVal zhdNr As Integer)

		Try
			'If Not IsUserActionAllowed(ClsDataDetail.UserData.UserNr, 701) Then m_Logger.LogWarning("No rights...") : Exit Sub

			Try
				Dim init = CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr)
				Dim frmVacancy = New SPKD.Vakanz.frmVakanzen(init)
				Dim setting = New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = iVakNr, .SelectedKDNr = kdNr, .SelectedZHDNr = zhdNr}

				frmVacancy.VacancySetting = setting
				If Not frmVacancy.LoadData Then Return

				frmVacancy.Show()
				frmVacancy.BringToFront()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try


			'Dim obj As New SPKD.Vakanz.ClsMain_Net(New SPKD.Vakanz.ClsVakSetting With {.SelectedVakNr = iVakNr},
			'																			 New SPKD.Vakanz.ClsSetting With {.SelectedMDNr = ClsDataDetail.MDData.MDNr,
			'																																				.LogedUSNr = ClsDataDetail.UserData.UserNr})
			'obj.ShowfrmVakanzen()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	''' <summary>
	''' Kandidatenverwaltung öffnen
	''' </summary>
	''' <param name="iMANr"></param>
	''' <remarks></remarks>
	Sub RunOpenMAForm(ByVal iMANr As Integer)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		Try
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MANr", iMANr.ToString)

			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("KandidatUtility.ClsMain", iMANr.ToString)

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenMAForm")

		End Try

	End Sub


	''' <summary>
	''' Kundenverwaltung öffnen
	''' </summary>
	''' <param name="iKDNr"></param>
	''' <remarks></remarks>
	Sub RunOpenKDform(ByVal iKDNr As Integer)

		Try
			Dim frm_Kunden As New frmCustomers(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))
			If iKDNr > 0 Then
				frm_Kunden.LoadCustomerData(iKDNr)
			Else
				frm_Kunden.LoadCustomerData(Nothing)

			End If
			frm_Kunden.Show()

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	''' <summary>
	''' Zuständige Person - Verwaltung öffnen
	''' </summary>
	''' <param name="iKDZHDNr"></param>
	''' <remarks></remarks>
	Sub RunOpenKDZHDform(ByVal iKDNr As Integer, ByVal iKDZHDNr As Integer)

		Try
			Dim responsiblePersonsFrom = New frmResponsiblePerson(CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr))

			If (responsiblePersonsFrom.LoadResponsiblePersonData(iKDNr, iKDZHDNr)) Then
				responsiblePersonsFrom.Show()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

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
				MessageBox.Show(String.Format(m_xml.GetSafeTranslationValue("Kann keine Daten lesen: {0}"), Ex.Message))
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
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections",
							 "SQLQuery", strQuery)

		Dim strSMSFile As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg")
		If strSMSFile = String.Empty Then
			strProgPath = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath")
			strProgPath = _ClsReg.AddDirSep(strProgPath) & "Binn\"

			If strSMSFile = String.Empty Then strSMSFile = strProgPath & strSMSProgName
		End If

		If Not File.Exists(strSMSFile) Then
			MsgBox(m_xml.GetSafeTranslationValue("Folgende Datei wurde nicht gefunden. Bitte wählen Sie das Programm aus.") & vbLf &
					(strSMSFile), MsgBoxStyle.Critical, m_xml.GetSafeTranslationValue("Programm wurde nicht gefunden"))

			strSMSFile = ShowMyFileDlg(strSMSFile)
			If strSMSFile <> String.Empty Then
				_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg", strSMSFile)
				Process.Start(strSMSFile)
			End If

		Else
			Process.Start(strSMSFile)

		End If

	End Sub

	Sub RunMailModul(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSMailUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSMailUtility.ClsMain", strTempSQL)

		Catch e As Exception

		End Try

	End Sub

	Sub ExportDataToOutlook(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSCommUtil.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			If MsgBox(m_xml.GetSafeTranslationValue("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?"),
								MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Daten exportieren") = MsgBoxResult.Yes Then
				oMyProg = CreateObject("SPSModulsView.ClsMain")
				oMyProg.ExportDataToOutlook(strTempSQL, "KD")
			End If

		Catch e As Exception

		End Try

	End Sub

	Sub RunKommaModul(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "KD")

		Catch e As Exception

		End Try

	End Sub

	Sub RunTobitFaxModul(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "MA", "1")

		Catch e As Exception

		End Try

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


	Sub MailTo(ByVal An As String, Optional ByVal Betreff As String = "")
		System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject={1}", An, Betreff))
	End Sub


End Module

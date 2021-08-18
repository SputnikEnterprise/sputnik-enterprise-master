
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports SP.Infrastructure.Logging
Imports SP.KD.CustomerMng.UI
Imports SP.KD.InvoiceMng.UI
Imports SPProgUtility.SPUserSec.ClsUserSec


Module FuncOpenProg

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsFunc As New ClsDivFunc
	Dim _ClsReg As New SPProgUtility.ClsDivReg


	Function GetMenuItems4Export() As List(Of String)
		Dim sql As String = String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr",
																							ClsDataDetail.GetAppGuidValue)
		Dim i As Integer = 0
		Dim bIsAllowed As Boolean = False
		Dim liResult As New List(Of String)

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMnurec As SqlDataReader = cmd.ExecuteReader

			While rMnurec.Read
				i += 1
				Trace.WriteLine(rMnurec("MnuName").ToString)

				' Berechtigung für bestimmte Einträge ohne ExtraRechte ausser Kraft setzen
				If rMnurec("MnuName").ToString.ToUpper.Contains("Comatic".ToUpper) Then
					bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
				ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("Abacus".ToUpper) Then
					bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)

				ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("Sesam".ToUpper) Then
					' Zur Zeit schalten wir es ab...
					bIsAllowed = False ' IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
				ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("cresus".ToUpper) Then
					bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)

				ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("SWIFAC".ToUpper) Then
					bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
				ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("KMUFactoring".ToUpper) Then
					bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)
				ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("CSOPListe".ToUpper) Then
					bIsAllowed = IsModulLicenceOK("CSOPList".ToLower)
				ElseIf rMnurec("MnuName").ToString.ToUpper = "CRF".ToUpper Then
					bIsAllowed = IsModulLicenceOK("CSOPList".ToLower)

				Else
					bIsAllowed = True ' Standardmässig darf man alles sehen

				End If

				If bIsAllowed Then
					liResult.Add(String.Format("{0}#{1}#{2}", ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString),
																		 ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("MnuName").ToString),
																		 ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("Docname").ToString)))
				End If

				'End If







				'liResult.Add(String.Format("{0}#{1}#{2}", ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString),
				'														 ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("MnuName").ToString),
				'														 ClsDataDetail.m_Translate.GetSafeTranslationValue(rMnurec("Docname").ToString)))

			End While


		Catch e As Exception
			MsgBox(e.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult

	End Function


	Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, ByVal dBetrag_inklMwSt As Double, ByVal dBetrag_MwStFrei As Double)
		Dim i As Integer = 0

		Try
			tsbMenu.DropDownItems.Clear()
			tsbMenu.DropDown.SuspendLayout()

			Dim mnu As ToolStripMenuItem

			mnu = New ToolStripMenuItem()
			mnu.Text = String.Format(ClsDataDetail.m_Translate.GetSafeTranslationValue("Total Beträge (inkl MwSt): {0}"),
															 Format(dBetrag_inklMwSt, "n"))
			tsbMenu.DropDownItems.Add(mnu)

			mnu = New ToolStripMenuItem()
			mnu.Text = String.Format(ClsDataDetail.m_Translate.GetSafeTranslationValue("Total Beträge (MwSt-frei): {0}"),
															 Format(dBetrag_MwStFrei, "n"))
			tsbMenu.DropDownItems.Add(mnu)

			tsbMenu.DropDown.ResumeLayout()
			tsbMenu.ShowDropDown()

		Catch e As Exception
			MsgBox(Err.GetException.ToString)

		Finally

		End Try

	End Sub


#Region "Funktionen für Exportieren..."

	Sub RunOpenZEForm(ByVal iZENr As Integer)
		Dim Init = CreateInitialData(ClsDataDetail.m_InitialData.MDData.MDNr, ClsDataDetail.m_InitialData.UserData.UserNr)

		Dim frmPaymentMng = New SP.KD.InvoiceMng.UI.frmZEedit(Init)
		frmPaymentMng.CurrentPaymentNumber = CInt(iZENr)
		If frmPaymentMng.LoadData() Then
			frmPaymentMng.Show()
			frmPaymentMng.BringToFront()
		End If


		'Dim oMyProg As Object
		'Dim strTranslationProgName As String = String.Empty

		''strTranslationProgName = _ClsSystem.GetPersonalFolder() & "SPTranslationProg" & _ClsSystem.GetLogedUSNr()
		'_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "ZEUtility.ClsMain")
		'_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iZENr.ToString)

		'Try
		'	_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "ZENr", iZENr.ToString)

		'	oMyProg = CreateObject("SPSModulsView.ClsMain")
		'	oMyProg.TranslateProg4Net("ZEUtility.ClsMain", iZENr.ToString, 2)

		'Catch e As Exception
		'	MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenZEForm")

		'End Try

	End Sub

	Sub RunOpenOPForm(ByVal iOPNr As Integer)
		Try
			Dim frm_Invoice As New frmInvoices(CreateInitialData(ClsDataDetail.m_InitialData.MDData.MDNr, ClsDataDetail.m_InitialData.UserData.UserNr))
			If frm_Invoice.LoadInvoiceData(iOPNr) Then
				frm_Invoice.Show()
				frm_Invoice.BringToFront()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try



		'Dim oMyProg As Object
		'Dim strTranslationProgName As String = String.Empty

		''strTranslationProgName = _ClsSystem.GetPersonalFolder() & "SPTranslationProg" & _ClsSystem.GetLogedUSNr()
		'_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "OPUtility.ClsMain")
		'_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iOPNr.ToString)

		'Try
		'	_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "RENr", iOPNr.ToString)

		'	oMyProg = CreateObject("SPSModulsView.ClsMain")
		'	oMyProg.TranslateProg4Net("OPUtility.ClsMain", iOPNr.ToString, 2)

		'Catch e As Exception
		'	MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenREForm")

		'End Try

	End Sub

	Sub RunOpenKDForm(ByVal iKDNr As Integer)

		Try
			Dim frm_Kunden As New frmCustomers(CreateInitialData(ClsDataDetail.m_InitialData.MDData.MDNr, ClsDataDetail.m_InitialData.UserData.UserNr))
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

	Sub ExportDataToAbacus(ByVal strTempSQL As String)
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																				.ModulName = "abaze".ToLower,
																																				.SQL2Open = strTempSQL}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			obj.ShowAbacusForm()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Datenexport in ABACUS")

		End Try
	End Sub


	Sub ExportDataToCresus(ByVal strTempSQL As String)
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																				.ModulName = "cresusze".ToLower,
																																				.SQL2Open = strTempSQL}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			obj.ShowCresusForm()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Datenexport in CRESUS")

		End Try
	End Sub


	Sub ExportDataToSimba(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		'strTranslationProgName = _ClsSystem.GetPersonalFolder() & "SPTranslationProg" & _ClsSystem.GetLogedUSNr()
		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try

			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "Simba")

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "ExportDataToSimba")

		End Try

	End Sub



	Sub RunTapi_KDZhd(ByVal strNumber As String, ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
		Dim strTranslationProgName As String = String.Empty
		Dim iTest As Integer = 0

		'strTranslationProgName = _ClsSystem.GetPersonalFolder() & "SPTranslationProg" & _ClsSystem.GetLogedUSNr()
		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSKDZHD.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDZhdNr.ToString)

		Try


		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunTapi_KDZhd")

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
				MessageBox.Show(ClsDataDetail.m_Translate.GetSafeTranslationValue("Kann keine Daten lesen:") & " " & Ex.Message)
			Finally
				' Check this again, since we need to make sure we didn't throw an exception on open.
				If (myStream IsNot Nothing) Then
					myStream.Close()
				End If
			End Try
		End If

		Return strFullFileName
	End Function

	Sub RunSMSProg()
		Dim strProgPath As String
		Dim strSMSProgName As String = "Sputnik Suite SMS.EXE"

		Dim strSMSFile As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg")
		If strSMSFile = String.Empty Then
			strProgPath = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath")
			strProgPath = _ClsReg.AddDirSep(strProgPath) & "Binn\"

			If strSMSFile = String.Empty Then strSMSFile = strProgPath & strSMSProgName
		End If

		If Not File.Exists(strSMSFile) Then
			MsgBox("Folgende Datei wurde nicht gefunden. Bitte wählen Sie das Programm aus." & vbLf &
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

	Sub RunKommaModul(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		'strTranslationProgName = _ClsSystem.GetPersonalFolder() & "SPTranslationProg" & _ClsSystem.GetLogedUSNr()
		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "OP")

		Catch e As Exception

		End Try

	End Sub



#End Region


	Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Dim bResult As Boolean = False

		' alle geöffneten Forms durchlauden
		For Each oForm As Form In Application.OpenForms
			If oForm.Name.ToLower = sName.ToLower Then
				If bDisposeForm Then oForm.Dispose() : Exit For
				bResult = True : Exit For
			End If
		Next

		Return (bResult)
	End Function



End Module

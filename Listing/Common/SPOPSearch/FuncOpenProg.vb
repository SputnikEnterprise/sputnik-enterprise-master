
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten

Imports SPOPSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Module FuncOpenProg

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_utility As New Utilities
	Private m_md As New Mandant

	Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)

		Dim i As Integer = 0
		' Zuerst auf neue Tabelle suchen
		Dim strSqlQuery As String =
		String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr",
																							ClsDataDetail.GetAppGuidValue)

		' Falls noch nicht vorhanden, so auf die alte Tabelle zurückgreifen.
		Dim strSqlQueryOld As String = "Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From OPExportDb Where ModulName = 'OP' "
		strSqlQueryOld += "Order By RecNr"

		Dim bIsAllowed As Boolean = False

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMnurec As SqlDataReader = cmd.ExecuteReader

			If Not rMnurec.HasRows Then ' Falls keine Zeilen gefunden, so auf die alte Tabelle zurückgreifen.
				rMnurec.Close()
				cmd.CommandText = strSqlQueryOld
				rMnurec = cmd.ExecuteReader()
			End If

			tsbMenu.DropDownItems.Clear()
			tsbMenu.DropDown.SuspendLayout()

			Dim mnu As ToolStripMenuItem
			While rMnurec.Read
				i += 1
				If rMnurec("Bezeichnung").ToString = "-" Then
					Dim sep As New ToolStripSeparator()
					tsbMenu.DropDownItems.Add(sep)

				Else

					' Berechtigung für bestimmte Einträge ohne ExtraRechte ausser Kraft setzen
					If rMnurec("MnuName").ToString.ToUpper.Contains("Comatic".ToUpper) Then
						bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)

					ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("Abacus".ToUpper) Then
						bIsAllowed = IsModulLicenceOK(rMnurec("MnuName").ToString.ToLower)

					ElseIf rMnurec("MnuName").ToString.ToUpper.Contains("Sesam".ToUpper) Then
						' Zur Zeit schalten wir es ab...
						bIsAllowed = False

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
					If rMnurec("Bezeichnung").ToString.ToLower.Contains("Creditreform".ToLower) Then
						' Achtung: rMnurec("MnuName").ToString.ToUpper = "CRF" !!!
						If ClsDataDetail.SelectedListArt.Contains("4") Then bIsAllowed = True Else bIsAllowed = False
					End If

					If bIsAllowed Then
						mnu = New ToolStripMenuItem()

						mnu.Text = m_Translate.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString)
						If Not IsDBNull(rMnurec("ToolTip")) Then
							mnu.ToolTipText = m_Translate.GetSafeTranslationValue(rMnurec("ToolTip").ToString)
						End If
						If Not IsDBNull(rMnurec("MnuName").ToString) Then
							mnu.Name = rMnurec("MnuName").ToString
						End If
						If Not IsDBNull(rMnurec("Docname")) Then
							mnu.Tag = rMnurec("Docname").ToString
						End If
						tsbMenu.DropDownItems.Add(mnu)
					End If

				End If

			End While
			tsbMenu.DropDown.ResumeLayout()
			tsbMenu.ShowDropDown()


		Catch e As Exception
			MsgBox(Err.GetException.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function GetMenuItems4Export() As List(Of String)
		Dim i As Integer = 0
		Dim strSqlQuery As String =
		String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr",
																							ClsDataDetail.GetAppGuidValue)
		Dim liResult As New List(Of String)

		' Falls noch nicht vorhanden, so auf die alte Tabelle zurückgreifen.
		Dim strSqlQueryOld As String = "Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From OPExportDb Where ModulName = 'OP' "
		strSqlQueryOld += "Order By RecNr"

		Dim bIsAllowed As Boolean = False

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMnurec As SqlDataReader = cmd.ExecuteReader

			If Not rMnurec.HasRows Then ' Falls keine Zeilen gefunden, so auf die alte Tabelle zurückgreifen.
				rMnurec.Close()
				cmd.CommandText = strSqlQueryOld
				rMnurec = cmd.ExecuteReader()
			End If

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
				If rMnurec("Bezeichnung").ToString.ToLower.Contains("Creditreform".ToLower) Then
					' Achtung: rMnurec("MnuName").ToString.ToUpper = "CRF" !!!
					If ClsDataDetail.SelectedListArt.Contains("3") Or ClsDataDetail.SelectedListArt.Contains("4") Then bIsAllowed = True Else bIsAllowed = False
				End If

				If bIsAllowed Then
					liResult.Add(String.Format("{0}#{1}#{2}", m_Translate.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString),
																		 m_Translate.GetSafeTranslationValue(rMnurec("MnuName").ToString),
																		 m_Translate.GetSafeTranslationValue(rMnurec("Docname").ToString)))
				End If

				'End If

			End While

		Catch e As Exception
			MsgBox(Err.GetException.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult
	End Function



	Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, ByVal dBetrag_1 As Double, _
												ByVal dBetrag_2 As Double, ByVal dBetrag_3 As Double, ByVal dBetrag_4 As Double)
		Dim i As Integer = 0

		Try
			tsbMenu.DropDownItems.Clear()
			tsbMenu.DropDown.SuspendLayout()

			Dim mnu As ToolStripMenuItem

			mnu = New ToolStripMenuItem()
			mnu.Text = String.Format(m_Translate.GetSafeTranslationValue("Betrag ohne MwSt.: {0}"), _
															 Format(dBetrag_2, "n"))
			tsbMenu.DropDownItems.Add(mnu)

			mnu = New ToolStripMenuItem()
			mnu.Text = String.Format(m_Translate.GetSafeTranslationValue("Betrag exkl. MwSt.: {0}"), _
															 Format(dBetrag_3, "n"))
			tsbMenu.DropDownItems.Add(mnu)

			mnu = New ToolStripMenuItem()
			mnu.Text = String.Format(m_Translate.GetSafeTranslationValue("Betrag MwSt.: {0}"), _
															 Format(dBetrag_4, "n"))
			tsbMenu.DropDownItems.Add(mnu)

			Dim sep As New ToolStripSeparator()
			tsbMenu.DropDownItems.Add(sep)

			mnu = New ToolStripMenuItem()
			mnu.Text = String.Format(m_Translate.GetSafeTranslationValue("Betrag inkl. MwSt.: {0}"), _
															 Format(dBetrag_1, "n"))
			tsbMenu.DropDownItems.Add(mnu)

			If Not IsNothing(ClsDataDetail.GetTotalOpenBetrag4Date) And ClsDataDetail.GetTotalOpenBetrag4Date <> 0 Then
				mnu = New ToolStripMenuItem()
				mnu.Text = String.Format(m_Translate.GetSafeTranslationValue("Offener Betrag inkl. MwSt.: {0}"), _
																 Format(ClsDataDetail.GetTotalOpenBetrag4Date, "n"))
				tsbMenu.DropDownItems.Add(mnu)
			End If

			tsbMenu.DropDown.ResumeLayout()
			tsbMenu.ShowDropDown()

		Catch e As Exception
			MsgBox(Err.GetException.ToString)

		Finally

		End Try

	End Sub


#Region "Funktionen für Exportieren..."

	'Sub RunOpenOPForm(ByVal invoiceNumber As Integer)

	'	Try
	'		Dim frm_Invoice As New frmInvoices(CreateInitialData(m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr))
	'		If frm_Invoice.LoadInvoiceData(invoiceNumber) Then
	'			frm_Invoice.Show()
	'			frm_Invoice.BringToFront()
	'		End If

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString())
	'	End Try


	'	'Dim oMyProg As Object
	'	'Dim strTranslationProgName As String = String.Empty

	'	'_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "OPUtility.ClsMain")
	'	'_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iOPNr.ToString)

	'	'Try
	'	'	_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "RENr", iOPNr.ToString)

	'	'	oMyProg = CreateObject("SPSModulsView.ClsMain")
	'	'	oMyProg.TranslateProg4Net("OPUtility.ClsMain", iOPNr.ToString, 2)

	'	'Catch e As Exception
	'	'	MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenOPForm")

	'	'End Try

	'End Sub

	'Sub RunOpenKDForm(ByVal iKDNr As Integer)

	'	Try
	'		Dim hub = MessageService.Instance.Hub
	'		Dim openMng As New OpenCustomerMngRequest(Me, m_InitialData.UserData.UserNr, m_InitialData.MDData.MDNr, iKDNr)
	'		hub.Publish(openMng)


	'		'Dim frm_Kunden As New frmCustomers(CreateInitialData(m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr))
	'		'If iKDNr > 0 Then
	'		'	frm_Kunden.LoadCustomerData(iKDNr)
	'		'Else
	'		'	frm_Kunden.LoadCustomerData(Nothing)

	'		'End If
	'		'frm_Kunden.Show()

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try


	'End Sub

	Sub ExportDataToAbacus(ByVal strTempSQL As String)
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitialData.MDData.MDDbConn, _
																																				.ModulName = "AbaOP".ToLower, _
																																				.SQL2Open = strTempSQL,
																																				.SelectedMDNr = m_InitialData.MDData.MDNr,
																																				.LogedUSNr = m_InitialData.UserData.UserNr,
																																				.SelectedMDYear = m_InitialData.MDData.MDYear}

		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_setting)

		Try
			obj.ShowAbacusForm()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "Datenexport in ABACUS")

		End Try

	End Sub


	Sub ExportDataToCresus(ByVal strTempSQL As String)
		Dim _setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitialData.MDData.MDDbConn,
																																				.ModulName = "CresusOP".ToLower,
																																				.SQL2Open = strTempSQL,
																																				.SelectedMDNr = m_InitialData.MDData.MDNr,
																																				.LogedUSNr = m_InitialData.UserData.UserNr,
																																				.SelectedMDYear = m_InitialData.MDData.MDYear}

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

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try

			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "Simba")

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "ExportDataToSimba")

		End Try

	End Sub

	Sub RunSWIFACExport(Optional ByVal docname As String = "")

		Try
			Dim frmSource As New frmSWIFAC(docname)

			frmSource.Show()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunSWIFACExport")

		End Try

	End Sub

	Sub RunFactoringExport(Optional ByVal docname As String = "")

		Try
			Dim frmSource As New frmFactoring(docname)

			frmSource.Show()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunFactoringExport")

		End Try

	End Sub

	Sub RunCRFExport(ByVal strDocname As String, ByVal iMahnNr As Integer, ByVal dMahnDate As Date)

		Try
			Dim frmSource As New frmCreditreform(strDocname, iMahnNr, dMahnDate)

			frmSource.Show()

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunCRFExport")

		End Try

	End Sub

	Sub StartExportOPListModul(ByVal sql As String)
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitialData.MDData.MDDbConn, _
																																					 .SQL2Open = sql, _
																																					 .ModulName = "OPSearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromOPListing(sql)

	End Sub

	Sub RunTapi_KDZhd(ByVal strNumber As String, ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
		Dim strTranslationProgName As String = String.Empty
		Dim iTest As Integer = 0


		Try
			_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSKDZHD.ClsMain")
			_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDZhdNr.ToString)


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
			If MsgBox("Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?", _
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
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "OP")

		Catch e As Exception

		End Try

	End Sub

	Sub RunXMLModul(ByVal strTempSQL As String)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strTranslationProgName As String = String.Empty

		Dim cmd As System.Data.SqlClient.SqlCommand
		cmd = New System.Data.SqlClient.SqlCommand(strTempSQL & " FOR XML AUTO", Conn)

		Try
			Conn.Open()

			Dim Xml_Reader As System.Xml.XmlReader

			Xml_Reader = cmd.ExecuteXmlReader()
			Dim sb As New System.Text.StringBuilder
			sb.Append("<xml>")
			Xml_Reader.Read()
			Do
				Dim node As String = Xml_Reader.ReadOuterXml()
				If node.Length = 0 Then Exit Do
				sb.Append(node)
			Loop
			sb.Append("</xml>")

			Xml_Reader.Close()

			Dim objDateiMacher As StreamWriter
			objDateiMacher = New StreamWriter(m_utility.GetMyDocumentsPathWithBackSlash & "OPList.XML")
			objDateiMacher.Write(sb.ToString)
			objDateiMacher.Close()
			objDateiMacher.Dispose()

			MessageBox.Show("Die Datei " & m_utility.GetMyDocumentsPathWithBackSlash & "OPList.XML" & " wurde erfolgreich erstelle.", _
											"Export in XML", MessageBoxButtons.OK, MessageBoxIcon.Information)


		Catch e As Exception

		End Try

	End Sub

	Sub RunXLSModul4CS(ByVal strTempSQL As String, ByVal strListCreatedOn As String, ByVal strListPer As String)
		Dim strTranslationProgName As String = m_utility.GetSpSREHomeFolder & "RunXLSModul4CS"

		Try
			File.Delete(strTranslationProgName)

		Catch ex As Exception

		End Try
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "UserNr", m_InitialData.UserData.UserNr)
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "OPErstelltam", strListCreatedOn)
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "OPListeper", strListPer)
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			Dim strTemplatename As String = m_md.GetSelectedMDTemplatePath(m_InitialData.MDData.MDNr) & "Muster_Offene_Posten.XLS"
			Dim strKopieFile As String = m_utility.GetSpSREHomeFolder & "Muster_Offene_Posten.XLS"
			File.Copy(strTemplatename, strKopieFile, True)
			Process.Start(strKopieFile)


		Catch e As Exception
			MessageBox.Show(String.Format("Fehler: {0}", e.Message), _
											"RunXLSModul4CS: Dateiexport für CS", MessageBoxButtons.OK, MessageBoxIcon.Error)

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

	'Function ExtraRights(ByVal lModulNr As Integer) As Boolean
	'  Dim bAllowed As Boolean
	'  Dim strModulCode As String

	'  ' 10200        ' Fremdrechnung
	'  ' 10201        ' Rapportinhalt
	'  ' 10202        ' Export nach Abacus
	'  ' 10206        ' Export nach Sesam
	'  ' 10211        ' Export nach Comatic
	'  strModulCode = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "ExtraModuls", CStr(lModulNr))
	'  If InStr(1, strModulCode, "+" & lModulNr & "+") > 0 Then bAllowed = True

	'  ExtraRights = bAllowed

	'  ' TODO: ExtraRights gibt immer True zurück. 08.12.2010 ar
	'  ExtraRights = True

	'End Function


End Module

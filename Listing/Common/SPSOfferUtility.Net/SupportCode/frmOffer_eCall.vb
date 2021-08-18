

'Imports System.Data.SqlClient
'Imports DevExpress.XtraGrid.Columns

'Partial Class OfferMessages


'#Region "private Consts"

'	Private Const EXIT_ON_ERRORSTATUS As String = "6000 # 6002 # 6005 # 6014 # 6016 # 6017"
'	Private Const CONTINUE_ON_RESPONSECODE As String = "0 # 11912"

'#End Region


'#Region "Private Fields"

'	''' <summary>
'	''' The message list
'	''' </summary>
'	''' <remarks></remarks>
'	Private m_faxCollextion As FaxCollection


'#End Region


'#Region "private properties"

'	Private Property FaxNumber2Send As String
'	Private Property KDNr2Send As Integer
'	Private Property ZHDNr2Send As Integer

'#End Region

'	Sub FillMyDataGrid()
'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'		Dim strValue As String = String.Empty
'		Dim dt As New DataTable
'		Dim i As Integer = 0

'		Try
'			dt = GetKDDbData4SendFax()

'		Catch ex As Exception
'			Dim strMsg As String = String.Format("Error: Fehler bei Zuweisung der Datenbank.{0}{1}", vbNewLine, ex.Message)
'			m_Logger.LogError(String.Format("{0}.Datenbank öffnen. {1}", strMethodeName, ex.Message))

'			DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Datenbank zuweisen"), MessageBoxButtons.OK, MessageBoxIcon.Error)
'			Return
'		End Try

'		Try
'			Dim Columnnames As String()
'			Dim ColumnCaption As String()

'			Columnnames = {"KDNr".ToLower, "ZHDRecNr".ToLower, "Firma1".ToLower, "Anrede".ToLower, "Nachname".ToLower, "Vorname".ToLower,
'										 "KDTelefax".ToLower, "KD_Telefax_Mailing".ToLower, "ZHDTelefax".ToLower, "ZHD_Telefax_Mailing".ToLower,
'										 "übernehmen?".ToLower}

'			ColumnCaption = {"Kunden-Nr.", "ZHD-Nr.", "Firma", "Anrede", "Nachname", "Vorname",
'												 "Telefax (KD)", "Mailing (KD)", "Telefax (ZHD)", "Mailing (ZHD)",
'												 "übernehmen?"}

'			dt.Columns.Add("übernehmen?", GetType(Boolean))
'			gvContent4InsertIntoSPDb.Columns.Clear()
'			grdContent4ImportIntoSPDb.DataSource = Nothing
'			grdContent4ImportIntoSPDb.DataSource = dt

'			For Each col As GridColumn In Me.gvContent4InsertIntoSPDb.Columns
'				Trace.WriteLine(col.FieldName)
'				col.MinWidth = 0
'				Dim strColName As String = col.FieldName.ToLower
'				Try
'					Dim bVisible As Boolean = Array.IndexOf(Columnnames, strColName) >= 0
'					If bVisible Then
'						col.Visible = True
'						col.Caption = m_Translate.GetSafeTranslationValue(ColumnCaption(Array.IndexOf(Columnnames, strColName)))
'						col.OptionsColumn.AllowEdit = strColName = "übernehmen?"
'						If strColName = "übernehmen?" Then
'							col.AppearanceCell.BackColor = Color.Beige
'						End If

'					Else
'						col.Visible = False

'					End If

'				Catch ex As Exception
'					m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'					Trace.WriteLine(ex.Message)
'					col.Visible = False

'				End Try
'				i += 1
'			Next col

'		Catch ex As Exception
'			Dim strMsg As String = String.Format("Error: Fehler bei definition der Datenbankspalten.{0}{1}", vbNewLine, ex.Message)
'			m_Logger.LogError(String.Format("{0}.Spalten anordnen. {1}", strMethodeName, ex.Message))

'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Spalten auflisten"), MessageBoxIcon.Error)
'			Return
'		End Try

'		i = 0
'		Dim bDeleterec As Boolean
'		Dim liSelectedFaxNumber As New List(Of String)
'		Try
'			Dim strMsg As String = m_Translate.GetSafeTranslationValue("Ihre Daten werden importiert. Bitte warten Sie einen Augenblick...")
'			Me.bsiInfo.Caption = strMsg

'			For Each myrow As DataRow In dt.Rows
'				bDeleterec = False
'				bDeleterec = CStr(myrow.Item("KDTelefax").ToString) & CStr(myrow.Item("ZHDTelefax").ToString) = String.Empty
'				If Not bDeleterec Then myrow("übernehmen?") = tgsFaxRecipientSelection.EditValue ' True
'				If bDeleterec Then myrow.Delete() Else i += 1

'			Next
'			Dim iRowCount As Integer = i
'			bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), iRowCount)

'		Catch ex As Exception
'			Dim strMsg As String = String.Format("Error: Fehler bei öffnen der Datenbank.{0}{1}", vbNewLine, ex.Message)
'			m_Logger.LogError(String.Format("{0}.Daten auflisten. {1}", strMethodeName, ex.Message))

'			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Daten auflisten"), MessageBoxIcon.Error)
'			Return
'		End Try

'	End Sub

'	Function GetKDDbData4SendFax() As DataTable
'		Dim ds As New DataSet
'		Dim dt As New DataTable
'		Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)

'		GetQery4ShowInFaxGrid()

'		Dim strQuery As String = Me.Sql2Open4Grid
'		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
'		cmd.CommandType = CommandType.Text

'		Dim objAdapter As New SqlDataAdapter
'		objAdapter.SelectCommand = cmd
'		objAdapter.Fill(ds, "KDData")

'		Return ds.Tables(0)
'	End Function

'	Sub GetQery4ShowInFaxGrid()
'		Dim strBeginTrySql As String = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH"
'		Dim strTestSql As String = String.Format("{0} SELECT * {1} FROM _Kundenliste_{2} ",
'															 strBeginTrySql,
'															 "Into #KD_Mailing",
'															 m_InitializationData.UserData.UserNr)
'		strTestSql &= "SELECT KDNr, ZHDRecNr, Firma1, "
'		strTestSql &= "(Case KD_Telefax_Mailing When 1 Then '' Else KDTelefax End ) As KDTelefax, "
'		strTestSql &= "KD_Telefax_Mailing, "

'		strTestSql &= "Vorname, Nachname, "
'		strTestSql &= "(Case ZHD_Telefax_Mailing When 1 Then '' Else ZHDTelefax End ) As ZHDTelefax, "
'		strTestSql &= "ZHD_Telefax_Mailing "

'		strTestSql &= "From #KD_Mailing "
'		strTestSql &= "Where KDTelefax + ZHDTelefax <> '' "
'		strTestSql &= "Group By KDNr, ZHDRecNr, Firma1, KDTelefax, ZHDTelefax, KD_Telefax_Mailing, ZHD_Telefax_Mailing, Vorname, Nachname "
'		strTestSql &= "Order By Firma1, Vorname, Nachname"

'		Me.Sql2Open4Grid = strTestSql

'	End Sub

'	Private Sub OnbtnSendTelefax_Click(sender As System.Object, e As System.EventArgs) Handles btnSendTelefax.Click
'		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'		'Dim strResult As String = String.Empty
'		'Dim strFaxValue As String = "Success"
'		'Dim i As Integer = 0

'		'Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick") & "..."

'		'Dim bAllowed2Send As Boolean = True
'		'Dim liSelectedFaxNumber As New List(Of String)
'		'Dim strKDFax As String = String.Empty
'		'Dim strZHDFax As String = String.Empty
'		'Dim strFinalFax As String = String.Empty
'		'Dim strFinalKDZHDNr As String = String.Empty
'		'Dim data = SelectedTemplateRecord


'		'lstVersandResult.Items.Clear()

'		'Try
'		'	If lueOffer.EditValue Is Nothing OrElse lueOffer.EditValue = 0 Then Throw New Exception(m_Translate.GetSafeTranslationValue("Sie haben keine Offerte ausgewählt."))
'		'	If data Is Nothing OrElse String.IsNullOrWhiteSpace(data.JobNr) Then Throw New Exception(m_Translate.GetSafeTranslationValue("Sie haben keine Vorlage ausgewählt."))

'		'	Dim strMsg As String = m_Translate.GetSafeTranslationValue("Ihre Daten werden importiert. Bitte warten Sie einen Augenblick...")
'		'	Me.bsiInfo.Caption = strMsg

'		'	grdContent4ImportIntoSPDb.RefreshDataSource()
'		'	Dim dt As DataTable = CType(grdContent4ImportIntoSPDb.DataSource, DataTable)

'		'	' In der Offerte sind Kandidaten vorhanden
'		'	Dim existsEmployee As Boolean?  ' = ExistDocFile()
'		'	existsEmployee = m_ProposeDatabaseAccess.CheckIfEmployeeInOfferExists(m_InitializationData.MDData.MDNr, lueOffer.EditValue) ' ExistDocFile()
'		'	m_Logger.LogDebug(String.Format("Offertennummer: {0} | bExistDocFile: {1}", lueOffer.EditValue, existsEmployee.GetValueOrDefault(False)))
'		'	If existsEmployee Is Nothing Then
'		'		m_UtilityUI.ShowErrorDialog("Ihre Kandidaten Daten konnten nicht geladen werden.")

'		'		Return
'		'	End If




'		'	Dim storedFiles = StoreDataToFs(lueOffer.EditValue)


'		'	Try

'		'		For Each myrow As DataRow In dt.Rows
'		'			strFinalFax = String.Empty
'		'			strFinalKDZHDNr = String.Empty
'		'			If myrow.RowState = DataRowState.Deleted Then

'		'			Else
'		'				If CBool(myrow("übernehmen?")) Then
'		'					bAllowed2Send = True
'		'					strKDFax = myrow("KDTelefax").ToString
'		'					strZHDFax = myrow("ZHDTelefax").ToString
'		'					If strKDFax.Contains("055 616 21 67") Then
'		'						Trace.WriteLine("")
'		'					End If

'		'					If bAllowed2Send Then

'		'						If Not Me.chkSendKD.Checked Then strKDFax = String.Empty
'		'						If Not Me.chkSendZhd.Checked Then strZHDFax = String.Empty

'		'						If strKDFax = strZHDFax Then
'		'							strZHDFax = String.Empty
'		'							Trace.WriteLine("OneCheck")
'		'						End If
'		'						strFinalFax = String.Format("{0}|{1}", strKDFax, strZHDFax)
'		'						strFinalKDZHDNr = String.Format("{0}|{1}", myrow.Item("KDNr"), myrow.Item("ZHDRecNr"))

'		'						If strKDFax + strZHDFax = "" Then
'		'							strFaxValue = m_Translate.GetSafeTranslationValue("Leere Empfänger!")
'		'							bAllowed2Send = False

'		'						ElseIf Not liSelectedFaxNumber.Contains(strFinalFax) Then
'		'							liSelectedFaxNumber.Add(strFinalFax)

'		'						Else
'		'							strFaxValue = m_Translate.GetSafeTranslationValue("Bereits versendet!")
'		'							bAllowed2Send = False

'		'						End If

'		'					End If

'		'					If bAllowed2Send AndAlso Trim(strFinalFax).Length >= 10 Then
'		'						Dim bResult = bAllowed2Send
'		'						If existsEmployee Then
'		'							bResult = bResult AndAlso IsPDFTemplateCreated(CInt(myrow.Item("KDNr")), CInt(myrow.Item("ZHDRecNr")))
'		'						End If

'		'						For Each filename In storedFiles
'		'							If Not String.IsNullOrWhiteSpace(filename) Then
'		'								m_ExportedFilename &= If(String.IsNullOrWhiteSpace(m_ExportedFilename), "", ";") & filename
'		'							End If
'		'						Next

'		'						m_Logger.LogInfo(String.Format("{0}.Fax-Daten: {1} | Fax-Dokument: {2}", strMethodeName, strFinalFax, m_ExportedFilename))
'		'						If bResult Then strFaxValue = SendFinaldata2eCall(strFinalFax, strFinalKDZHDNr) Else strFaxValue = String.Empty
'		'						i += 1

'		'						Me.lstVersandResult.Items.Add(String.Format("{0} - {1}: {2}", i, strFinalFax, strFaxValue))
'		'						myrow("übernehmen?") = False
'		'						If strFaxValue.ToLower.Contains("error") Then
'		'							Me.lstVersandResult.Items.Add(String.Format("{0} - {1}: Der Vorgang wird abgebrochen!", i, strFinalFax))
'		'							Exit For
'		'						End If

'		'					Else
'		'						Me.lstVersandResult.Items.Add(String.Format("{0} - {1}: {2}", i, strFinalFax, strFaxValue))

'		'					End If

'		'				End If
'		'			End If
'		'		Next
'		'	Catch ex As Exception
'		'		strMsg = String.Format("Error: Fehler bei Zuweisung der Datenbank.{0}{1}", vbNewLine, ex.Message)
'		'		m_Logger.LogError(String.Format("{0}.Daten auflisten. {1}", strMethodeName, ex.Message))
'		'		Me.lstVersandResult.Items.Add(String.Format("{0}", ex.Message))

'		'		DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Datenbank zuweisen"), MessageBoxButtons.OK, MessageBoxIcon.Error)

'		'		Return
'		'	End Try

'		'	Dim iRowCount As Integer = i
'		'	Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Einträge wurden erfolgreich gesendet."), iRowCount)

'		'Catch ex As Exception
'		'	Dim strMsg As String = String.Format("Error: Fehler bei Zuweisung der Datenbank.{0}{1}", vbNewLine, ex.Message)
'		'	m_Logger.LogError(String.Format("{0}.Daten auflisten. {1}", strMethodeName, ex.Message))
'		'	Me.lstVersandResult.Items.Add(String.Format("{0}", ex.Message))

'		'	DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Datenbank zuweisen"), MessageBoxButtons.OK, MessageBoxIcon.Error)
'		'	Return

'		'Finally
'		'	If Me.lstVersandResult.Items.Count > 0 Then Me.XtraTabControl1.SelectedTabPage = Me.xtabResultData
'		'	Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

'		'End Try

'	End Sub

'	Function SendFinaldata2eCall(ByVal FaxNumber As String, ByVal KD_ZHD_Nr As String) As String
'		'Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'		'Dim strValue As String = "Success"
'		'Dim strFValue As String = ""
'		'Dim strLValue As String = ""
'		'Dim strFaxNumber As String() = FaxNumber.Split(CChar("|"))
'		'Dim aKDZHDNr As String() = KD_ZHD_Nr.Split(CChar("|"))
'		'Dim counter As Integer = 0
'		'Dim result As String = String.Format("Es wurden {0} Faxe gesendet.", counter)

'		'Dim isFirstMessage As Boolean = True
'		'Dim faxService As New SPSSendMail.ClsFaxStart2(New SPSSendMail.InitializeClass With {.MDData = ClsOfDetails.m_InitialData.MDData,
'		'											   .ProsonalizedData = ClsOfDetails.m_InitialData.ProsonalizedData,
'		'											   .TranslationData = ClsOfDetails.m_InitialData.TranslationData,
'		'											   .UserData = ClsOfDetails.m_InitialData.UserData})
'		'm_faxCollextion = New FaxCollection
'		'For Each number In strFaxNumber

'		'	If number.Length > 5 Then
'		'		Dim KDNr As Integer = CInt(Val(aKDZHDNr(0)))
'		'		Dim ZHDNr As Integer = CInt(Val(aKDZHDNr(1)))
'		'		Dim addReciever = False

'		'		For Each filename In m_ExportedFilename.Split(";")
'		'			If filename <> String.Empty Then
'		'				faxService.AddAttachment(filename)
'		'				addReciever = True
'		'			End If
'		'		Next
'		'		If addReciever Then
'		'			m_faxCollextion.AddReveiver(number, KDNr, ZHDNr2Send)
'		'		End If

'		'	End If

'		'Next

'		'If m_faxCollextion.Receivers.Count = 0 Then result = String.Format("Es wurden {0} Faxe gesendet. Keine Dokumente sind vorhanden!", counter)
'		'For Each faxMessage In m_faxCollextion.Receivers

'		'	Dim status = faxService.SendFax(faxMessage)

'		'	If isFirstMessage Then
'		'		' Wait for SendState 41, 42
'		'		'101 JobGruppe erfolgreich an Gateway übergeben
'		'		'201 Begonnen mit dem Konvertieren
'		'		'202 Einzelnes File konvertiert
'		'		'301 Begonnen mit dem Zusammenfügen der Dokumente
'		'		'401 Begonnen mit dem Senden
'		'		'402 Einzelner Job abgeschlossen
'		'		'501 Gesamte JobGroup abgeschlossen
'		'		' 41 Fax Meldung erfolgreich übermittelt.
'		'		' 42 Versand mit Fehler beendet.
'		'		While faxMessage.SendState <> 41 And faxMessage.SendState <> 42

'		'			' exit on error while sending first fax
'		'			Dim ResponseCodeOk = New Long() {-1, 0, 11912}
'		'			If Not ResponseCodeOk.Contains(faxMessage.ResponseCode) Then
'		'				' Break all Messages and LogPaymentService
'		'				result = String.Format("Fehler: [{0}] {1}", faxMessage.ResponseCode.ToString(), faxMessage.ResponseText)
'		'				faxService.LogPaymentService(faxMessage.JobId, faxMessage.PointsUsed)
'		'				Exit For
'		'			End If

'		'			Thread.Sleep(2000)
'		'			status = faxService.GetState(faxMessage)
'		'			faxMessage.UpdateStatus(status)
'		'		End While

'		'		faxService.LogPaymentService(faxMessage.JobId, faxMessage.PointsUsed)

'		'		If faxMessage.SendState = 42 Then
'		'			If EXIT_ON_ERRORSTATUS.IndexOf(faxMessage.ErrorState) > -1 Then
'		'				' Break all Messages
'		'				result = String.Format("Fehler: [{0}] ", faxMessage.ErrorState)
'		'				Exit For
'		'			End If
'		'		End If
'		'		isFirstMessage = False
'		'	Else
'		'		faxService.LogPaymentService(faxMessage.JobId, faxMessage.PointsUsed)
'		'	End If

'		'	'clone message
'		'	Dim cMessage As FaxReceiver = faxMessage.Clone()
'		'	If CONTINUE_ON_RESPONSECODE.IndexOf(faxMessage.ResponseCode) > -1 Then
'		'		counter += 1
'		'		result = String.Format("Es wurden {0} Faxe gesendet.", counter)
'		'	Else
'		'		result += String.Format("Fehler: [{0}]{1} ", faxMessage.ResponseCode, faxMessage.ResponseText)
'		'		Exit For
'		'	End If

'		'Next
'		'Trace.WriteLine(String.Format("{0} | {1}", FaxNumber, KD_ZHD_Nr))


'		'Return result

'	End Function



'#Region "tests"

'	'Function SendData2Fax() As String
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	Dim strResult As String = "Success"

'	'	Dim oMailObject As New SPSSendMail.ClsMain_Net
'	'	Dim iKDNr As Integer = 0
'	'	Dim iKDZNr As Integer = 0
'	'	Dim strSearchQuery As String = If(m_SendAsStaging, m_GetSearchQuery, ClsOfDetails.GetOrgProgQuery)
'	'	Dim strToField As String = String.Empty
'	'	Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
'	'	Dim cmd As SqlCommand = New SqlCommand(strSearchQuery, Conn)
'	'	Dim bResult As Boolean = True
'	'	Dim strOldReciver As String = ";"
'	'	Dim strFaxField_1 As String = "KDTelefax"
'	'	Dim strFaxField_2 As String = "ZHDTelefax"
'	'	Dim bGoWithSendig As String = AllowedToSend()
'	'	Dim strFaxToDomain As String = GetFaxDomain().Trim
'	'	Dim strMessageGuid As String = Guid.NewGuid().ToString
'	'	Dim bSendAIneternFax As Boolean
'	'	Dim bSendMail2Fax As Boolean
'	'	Dim cv As ComboValue

'	'	Try
'	'		ClsOfDetails.GetMessageGuid = strMessageGuid
'	'		If bGoWithSendig.ToLower.Contains("error") Then Throw New Exception(bGoWithSendig)

'	'	Catch ex As Exception
'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'	'		Dim strMsg As String = ex.Message

'	'		strResult = String.Format("Error: {0}", strMsg)
'	'		Return strResult
'	'	End Try

'	'	' In der Offerte sind Kandidaten vorhanden
'	'	Dim bExistDocFile As Boolean = ExistDocFile()
'	'	Dim storedFiles = StoreDataToFs(lueOffer.EditValue)

'	'	m_LOGFileName = String.Format("{0}{1}.{2}", _ClsProgSetting.GetSpSFiles2DeletePath, ClsOfDetails.GetMessageGuid, "tmp")
'	'	_ClsLog.WriteTempLogFile(String.Format("***Programmstart: {0}", Now.ToString), m_LOGFileName)

'	'	'cv = DirectCast(Me.CboMailTo.SelectedItem, ComboValue)

'	'	Try
'	'		Conn.Open()
'	'		strToField = String.Empty ' cv.ComboValue.ToUpper  ' Me.CboMailTo.Text

'	'		Dim rOFFKDrec As SqlDataReader = cmd.ExecuteReader                  ' Offertendatenbank
'	'		Dim bFirstCall As Boolean = True
'	'		Dim strOfferBez As String = GetOfferSubject(lueOffer.EditValue)
'	'		Dim i As Integer = 0
'	'		Dim bIsAllreadySent As Boolean = False

'	'		While rOFFKDrec.Read
'	'			i += 1

'	'			iKDNr = CInt(rOFFKDrec("KDNr").ToString)
'	'			If Not IsDBNull(rOFFKDrec("ZHDRecNr")) Then
'	'				iKDZNr = CInt(rOFFKDrec("ZHDRecNr").ToString)
'	'			Else
'	'				iKDZNr = 0
'	'			End If

'	'			'If strToField.ToUpper <> "kdzhd".ToUpper Then
'	'			'	If Not String.IsNullOrEmpty(rOFFKDrec(strToField).ToString) Then
'	'			'		Try
'	'			'			If strToField.ToUpper.Contains("KDTelefax".ToUpper) Then
'	'			'				If CBool(rOFFKDrec("KD_Telefax_Mailing").ToString) Then bResult = False
'	'			'			ElseIf strToField.ToUpper.Contains("ZHDTelefax".ToUpper) And iKDZNr > 0 Then
'	'			'				If CBool(rOFFKDrec("ZHD_Telefax_Mailing").ToString) Then bResult = False
'	'			'			Else
'	'			'				bResult = False
'	'			'			End If
'	'			'			If Not m_SendAsStaging AndAlso bResult Then bIsAllreadySent = IsMyMessageAlreadySent(rOFFKDrec(strToField).ToString, strOfferBez, "",
'	'			'																																			 iKDNr, strMessageGuid, Me.m_SendAsStaging)
'	'			'			If Not bResult Then
'	'			'				Dim strMsg As String = "Achtung: Die Fax-Nachricht darf nicht versendet werden!!!"
'	'			'				m_Logger.LogWarning(String.Format("{0}.{1}", strMethodeName, strMsg))
'	'			'				_ClsLog.WriteTempLogFile(String.Format("***{0} ==>> {1}",
'	'			'																 m_Translate.GetSafeTranslationValue(strMsg), rOFFKDrec(strToField).ToString), m_LOGFileName)
'	'			'				m_Logger.LogInfo(String.Format("{0}.Message was not allowed to send.", strMethodeName))

'	'			'			ElseIf Not strOldReciver.Replace(" ", "").ToLower.Contains(String.Format(";{0};", rOFFKDrec(strToField).ToString.Replace(" ", ""))) AndAlso
'	'			'																						Not bIsAllreadySent Then
'	'			'				ClsOfDetails.strLLFaxNumber = rOFFKDrec(strToField).ToString
'	'			'				ClsOfDetails.strLLFaxRecp = String.Format("{0}: {1} {2} {3}", rOFFKDrec("Firma1").ToString,
'	'			'																									rOFFKDrec("Anrede").ToString,
'	'			'																									rOFFKDrec("Vorname").ToString,
'	'			'																									rOFFKDrec("Nachname").ToString)

'	'			'				If strFaxToDomain = String.Empty Then

'	'			'				Else
'	'			'					bSendAIneternFax = False
'	'			'					If bExistDocFile Then
'	'			'						bResult = IsPDFTemplateCreated(iKDNr, iKDZNr)
'	'			'						m_Logger.LogInfo(String.Format("{0}.WithDomain: Erfolgreicher Versand: {1}", strMethodeName, rOFFKDrec(strToField).ToString))
'	'			'					End If

'	'			'				End If
'	'			'				bFirstCall = False
'	'			'				Me.KDNr2Send = iKDNr
'	'			'				Me.ZHDNr2Send = iKDZNr
'	'			'				Me.FaxNumber2Send = rOFFKDrec(strToField).ToString

'	'			'				If Not bSendAIneternFax Then bSendMail2Fax = True
'	'			'				strOldReciver += String.Format("{0};", rOFFKDrec(strToField).ToString)

'	'			'			Else
'	'			'				bResult = False
'	'			'				Dim strMsg As String = "Achtung: Wurde bereits versendet!!!"
'	'			'				m_Logger.LogWarning(String.Format("{0}.{1}", strMethodeName, strMsg))
'	'			'				_ClsLog.WriteTempLogFile(String.Format("***{0} ==>> {1}", m_Translate.GetSafeTranslationValue(strMsg), rOFFKDrec(strToField).ToString), m_LOGFileName)

'	'			'				m_Logger.LogInfo(String.Format("{0}.Dopplicated message was founded.", strMethodeName))
'	'			'			End If

'	'			'		Catch ex_0 As Exception
'	'			'			bResult = False
'	'			'			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex_0.Message))
'	'			'			_ClsLog.WriteTempLogFile(String.Format("***SendData2Fax_1: {0}", ex_0.Message), m_LOGFileName)

'	'			'			m_Logger.LogInfo(String.Format("{0}.Versand. {1}", strMethodeName, ex_0.Message))

'	'			'		End Try

'	'			'	End If
'	'			'	If bResult AndAlso bSendMail2Fax Then
'	'			'		For Each filename In storedFiles
'	'			'			If Not String.IsNullOrWhiteSpace(filename) Then
'	'			'				m_ExportedFilename &= If(String.IsNullOrWhiteSpace(m_ExportedFilename), "", ";") & filename
'	'			'			End If
'	'			'		Next

'	'			'		Dim strMsg As String = SendFinaldata2eCall(Me.FaxNumber2Send, String.Format("{0}|{1}", iKDNr, iKDZNr))
'	'			'		_ClsLog.WriteTempLogFile(String.Format("Versand: {0} ==>> {1}", Me.FaxNumber2Send, m_Translate.GetSafeTranslationValue(strMsg)), m_LOGFileName)

'	'			'	End If

'	'			'Else
'	'			Try
'	'					Dim aMailFields As New List(Of String)
'	'					Dim strMailAddress As String = String.Empty

'	'					If chkSendKD.Checked AndAlso iKDNr > 0 Then aMailFields.Add(CStr("KDTelefax"))
'	'					If chkSendZhd.Checked AndAlso iKDZNr > 0 Then aMailFields.Add(CStr("ZHDTelefax"))

'	'					For Each strMailAddress In aMailFields
'	'						If strMailAddress.ToUpper.Contains("KDTelefax".ToUpper) Then
'	'							If Not CBool(rOFFKDrec("KD_Telefax_Mailing").ToString) Then
'	'								bResult = Not String.IsNullOrEmpty(rOFFKDrec(strMailAddress).ToString)

'	'							Else
'	'								bResult = False

'	'							End If

'	'						ElseIf strMailAddress.ToUpper.Contains("ZHDTelefax".ToUpper) And iKDZNr > 0 Then
'	'							If Not CBool(rOFFKDrec("ZHD_Telefax_Mailing").ToString) Then
'	'								bResult = Not String.IsNullOrEmpty(rOFFKDrec(strMailAddress).ToString)

'	'							Else
'	'								bResult = False

'	'							End If

'	'						End If
'	'						If Not m_SendAsStaging AndAlso bResult Then bIsAllreadySent = IsMyMessageAlreadySent(rOFFKDrec(strMailAddress).ToString, strOfferBez, "", iKDNr, strMessageGuid, Me.m_SendAsStaging)

'	'						If Not bResult Then
'	'							Dim strMsg As String = "Achtung: Die Fax-Nachricht darf nicht versendet werden!!!"
'	'							m_Logger.LogWarning(String.Format("{0}.{1}", strMethodeName, strMsg))
'	'							_ClsLog.WriteTempLogFile(String.Format("***{0} ==>> {1}",
'	'																			 m_Translate.GetSafeTranslationValue(strMsg), rOFFKDrec(strMailAddress).ToString), m_LOGFileName)
'	'							m_Logger.LogInfo(String.Format("{0}.Message was not allowed to send.", strMethodeName))

'	'						ElseIf Not strOldReciver.Replace(" ", "").ToLower.Contains(String.Format(";{0};", rOFFKDrec(strMailAddress).ToString.Replace(" ", ""))) AndAlso
'	'																									Not bIsAllreadySent Then
'	'							ClsOfDetails.strLLFaxNumber = rOFFKDrec(strMailAddress).ToString
'	'							ClsOfDetails.strLLFaxRecp = rOFFKDrec("Firma1").ToString & ": " &
'	'																					rOFFKDrec("Anrede").ToString & " " &
'	'																					rOFFKDrec("Vorname").ToString & " " &
'	'																					rOFFKDrec("Nachname").ToString

'	'							If strFaxToDomain = String.Empty Then

'	'						Else
'	'								bSendAIneternFax = False
'	'								If bExistDocFile Then
'	'									bResult = IsPDFTemplateCreated(iKDNr, iKDZNr)
'	'									m_Logger.LogInfo(String.Format("{0}.WithDomain: Erfolgreicher Versand: {1}", strMethodeName, rOFFKDrec(strMailAddress).ToString))
'	'								End If

'	'						End If
'	'							bFirstCall = False
'	'							Me.KDNr2Send = iKDNr
'	'							Me.ZHDNr2Send = iKDZNr
'	'						Me.FaxNumber2Send = rOFFKDrec(strMailAddress).ToString

'	'						If bResult And Not bSendAIneternFax Then bSendMail2Fax = True

'	'					Else
'	'							bResult = False
'	'							Dim strMsg As String = "Achtung: Wurde bereits versendet!!!"
'	'							m_Logger.LogWarning(String.Format("{0}.{1}", strMethodeName, strMsg))
'	'						_ClsLog.WriteTempLogFile(String.Format("***{0} ==>> {1}", m_Translate.GetSafeTranslationValue(strMsg), rOFFKDrec(strMailAddress).ToString), m_LOGFileName)

'	'					End If

'	'						If bResult AndAlso bSendMail2Fax Then
'	'							For Each filename In storedFiles
'	'								If Not String.IsNullOrWhiteSpace(filename) Then
'	'									m_ExportedFilename &= If(String.IsNullOrWhiteSpace(m_ExportedFilename), "", ";") & filename
'	'								End If
'	'							Next

'	'							Dim strMsg As String = SendFinaldata2eCall(Me.FaxNumber2Send, String.Format("{0}|{1}", iKDNr, iKDZNr))
'	'							_ClsLog.WriteTempLogFile(String.Format("Versand: {0} ==>> {1}",
'	'																											 Me.FaxNumber2Send, m_Translate.GetSafeTranslationValue(strMsg)), m_LOGFileName)
'	'						End If
'	'					Next

'	'				Catch ex_1 As Exception
'	'					m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex_1.Message))
'	'					_ClsLog.WriteTempLogFile(String.Format("***SendData2Fax_2: {0}", ex_1.Message), m_LOGFileName)
'	'					m_Logger.LogInfo(String.Format("{0}.Versand. {1}", strMethodeName, ex_1.Message))

'	'				End Try

'	'			'End If

'	'			bResult = True
'	'		End While
'	'		_ClsLog.WriteTempLogFile(String.Format("***Programmende: {0}", Now.ToString), m_LOGFileName)

'	'		rOFFKDrec.Close()

'	'	Catch ex As Exception
'	'		Dim strMsg As String = ex.Message
'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'	'		_ClsLog.WriteTempLogFile(String.Format("***{0}: {1}", strMethodeName, ex.Message), m_LOGFileName)

'	'		strResult = String.Format("Error: {0}", strMsg)
'	'		Return strResult

'	'	Finally
'	'		oMailObject.OpenMailingList(2, strMessageGuid)
'	'		oMailObject = Nothing

'	'		cmd.Dispose()
'	'		Conn.Close()

'	'	End Try

'	'	Return strResult
'	'End Function

'#End Region


'End Class

Public Class FaxCollection

	'Public Property Filename2Send As String

	'Public Property Receivers As List(Of FaxReceiver)

	'Public Sub New()
	'	Receivers = New List(Of FaxReceiver)()
	'End Sub

	'Public Function AddReveiver(ByVal Address As String, ByVal KDNr As String, ByVal ZHDNr As String) As String
	'	If String.IsNullOrEmpty(Address) Then
	'		Return String.Empty
	'	End If

	'	Dim jobId = GenerateId()
	'	If String.IsNullOrWhiteSpace(jobId) Then jobId = Guid.NewGuid.ToString
	'	Dim strFaxToValue As String = Address
	'	If Not strFaxToValue.StartsWith("00") And Not strFaxToValue.StartsWith("+") Then
	'		If strFaxToValue.StartsWith("0") Then strFaxToValue = strFaxToValue.Remove(0, 1)
	'		strFaxToValue = "0041" & strFaxToValue
	'	End If
	'	strFaxToValue = strFaxToValue.Replace(" ", "").Replace("-", "").Replace("/", "")
	'	Address = strFaxToValue

	'	Dim duplicate = (From r In Receivers Where r.Address = Address).FirstOrDefault()

	'	If duplicate Is Nothing Then
	'		Receivers.Add(New FaxReceiver With {
	'									 .Address = Address,
	'									 .KDNr = KDNr,
	'									 .ZHDNr = ZHDNr,
	'									 .jobId = jobId
	'									 })
	'		Return jobId
	'	End If
	'	Return String.Empty
	'End Function

	Private m_random As Random = New Random

	Private Function GenerateId() As String
		Dim symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890"
		Return New String((From s In Enumerable.Repeat(symbols, 20) Select symbols(m_random.Next(symbols.Length))).ToArray())
	End Function


End Class

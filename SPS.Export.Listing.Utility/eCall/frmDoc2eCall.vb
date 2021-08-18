
Imports System.Reflection.Assembly
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Imports System.IO
Imports System.Windows.Forms
Imports System.Threading
Imports System.Data.SqlClient

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.Data.Filtering.Helpers
Imports System.Drawing
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPSSendMail

Imports SPProgUtility.ProgPath
Imports System.Security.Cryptography
Imports System.Text


Public Class frmDoc2eCall

#Region "Private Members"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_xml As New ClsXML
	Private m_md As Mandant
	Private m_deleteHomeFolder As String

	Private strConnString As String = String.Empty

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _docname As String

	Private Property _ExportSetting As New ClsCSVSettings
	Private Property SQL2Select() As String
	Private Property SQLFields() As String

	Public Property MetroForeColor As System.Drawing.Color
	Public Property MetroBorderColor As System.Drawing.Color

	Private Property MessageGuid As String
	Private WaitWindow As DevExpress.Utils.WaitDialogForm

	Private WithEvents m_frmDoc2eCallWait As frmDoc2eCallWait
	Private m_sendCount As Integer
	Private m_contagtLogger As ContactLogger
	Private m_UtilityUI As New UtilityUI

#End Region

#Region "Constructor"

	Public Sub New(ByVal _Setting As ClsCSVSettings)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant

		Try

			If ModulConstants.MDData.MDNr = 0 Or ModulConstants.MDData Is Nothing Then
				ModulConstants.MDData = ModulConstants.SelectedMDData(0)
				ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

				ModulConstants.PersonalizedData = ModulConstants.PersonalizedValues
				ModulConstants.TranslationData = ModulConstants.TranslationValues

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ModulConstants.TranslationData, ModulConstants.PersonalizedData)


		m_deleteHomeFolder = (New ClsProgPath).GetSpS2DeleteHomeFolder()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me._ExportSetting = _Setting
		Me.txtFilename.Text = String.Empty ' If(String.IsNullOrWhiteSpace(_Setting.ExportFileName), My.Settings.Filename4Doc2eCall, _Setting.ExportFileName)

	End Sub

#End Region

#Region "Fileexplorer..."

	Private Sub txtFilename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFilename.ButtonClick

		If e.Button.Index = 0 Then
			Dim t As Thread = New Thread(AddressOf ShowFolderBrowser)

			t.IsBackground = True
			t.Name = "FolderDialog"
			t.SetApartmentState(ApartmentState.STA)
			t.Start()

		Else
			If File.Exists(Me.txtFilename.Text) Then Process.Start(Me.txtFilename.Text)

		End If

	End Sub

	Sub ShowFolderBrowser()
		Dim dialog As New OpenFileDialog()

		dialog.InitialDirectory = If(Me.txtFilename.Text = String.Empty, _ClsProgSetting.GetSpSFiles2DeletePath, Path.GetDirectoryName(Me.txtFilename.Text))
		dialog.Filter = "txt Dateien (*.txt)|*.txt|Word-Dokument (*.Doc*)|*.Doc*|Rich-Text (*.RTF)|*.RTF|Excel-Dokument (*.XLS*)|*.XLS*|PDF-Dokument (*.PDF)|*.PDF"
		dialog.FilterIndex = 5
		dialog.RestoreDirectory = True


		'    dialog. = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei aus:")


		If dialog.ShowDialog() = DialogResult.OK Then
			Dim a As Action = Function() InlineAssignHelper(Me.txtFilename.Text, String.Format("{0}", dialog.FileName))
			Me.Invoke(a)
		End If

	End Sub

	Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
		target = value

		Return value
	End Function

#End Region

#Region "Public  Methods"

	Public Function ContainsMoreString(ByVal str As String, ByVal ParamArray values As String()) As Boolean

		For Each value As String In values
			If str = value Then
				Return True
			End If
		Next

		Return False
	End Function

#End Region

#Region "Private Methods"

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		Me.cmdLoad.Text = m_Translate.GetSafeTranslationValue(Me.cmdLoad.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.lblDatei.Text = m_Translate.GetSafeTranslationValue(Me.lblDatei.Text)

		Me.xtabVersanddata.Text = m_Translate.GetSafeTranslationValue(Me.xtabVersanddata.Text)
		Me.xtabResultData.Text = m_Translate.GetSafeTranslationValue(Me.xtabResultData.Text)

		Me.chkSendKD.Text = m_Translate.GetSafeTranslationValue(Me.chkSendKD.Text)
		Me.chkSendOne.Text = m_Translate.GetSafeTranslationValue(Me.chkSendOne.Text)
		Me.chkSendZhd.Text = m_Translate.GetSafeTranslationValue(Me.chkSendZhd.Text)

	End Sub

	'<Obsolete("This method is deprecated.")> _
	'Private Function SendFinaldata2eCall(ByVal FaxNumber As String, ByVal KD_ZHD_Nr As String, ByVal bWait4Response As Boolean) As String
	'  Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'  Dim strValue As String = "Success"
	'  Dim strFValue As String = ""
	'  Dim strLValue As String = ""
	'  Dim strFaxNumber As String() = FaxNumber.Split(CChar("|"))
	'  Dim aKDZHDNr As String() = KD_ZHD_Nr.Split(CChar("|"))

	'  Dim _setting As New SPSSendMail.ClsFaxSetting
	'  For i As Integer = 0 To strFaxNumber.Length - 1
	'    Dim strFinalFaxNumber As String = strFaxNumber(i)
	'    Dim KDNr As Integer = CInt(Val(aKDZHDNr(0)))
	'    Dim ZHDNr As Integer = CInt(Val(aKDZHDNr(1)))

	'    If strFinalFaxNumber.Length > 5 Then
	'      'strFinalFaxNumber = "062 865 01 69"
	'      Dim strFaxToValue As String = strFinalFaxNumber
	'      If Not strFaxToValue.StartsWith("00") And Not strFaxToValue.StartsWith("+") Then
	'        If strFaxToValue.StartsWith("0") Then strFaxToValue = strFaxToValue.Remove(0, 1)
	'        strFaxToValue = "0041" & strFaxToValue
	'      End If
	'      strFaxToValue = strFaxToValue.Replace(" ", "").Replace("-", "").Replace("/", "")
	'      strFinalFaxNumber = strFaxToValue

	'      _setting = New SPSSendMail.ClsFaxSetting With {.DbConnString2Open = Me._ExportSetting.DbConnString2Open,
	'                                                          .Filename2Send = Me.txtFilename.Text,
	'                                                          .SQL2Open = String.Empty,
	'                                                     .VersandGuid = Me.MessageGuid,
	'                                                          .KDNr = CInt(KDNr),
	'                                                          .ZHDNr = If(i = 0, 0, CInt(ZHDNr)),
	'                                                          .Number2Send = strFinalFaxNumber,
	'                                                     .Wait4Result = bWait4Response}
	'      Dim obj As New SPSSendMail.ClsFaxStart(_setting)
	'      strValue = obj.SendFax2eCall()    ' Fax-Nachricht wird geschickt...

	'      If Not strValue.ToLower.Contains("error") Then
	'        If bWait4Response Then
	'          Dim strMsg As String = "Der Versand wird überprüft.{0}Bitte warten Sie..."
	'          strMsg = String.Format(strMsg, vbNewLine)
	'          WaitWindow = New DevExpress.Utils.WaitDialogForm(m_Translate.GetSafeTranslationValue(strMsg),
	'                                                           m_Translate.GetSafeTranslationValue("Der Vorgang kann einige Minuten dauern!"), New Size(400, 100))

	'          Try
	'            WaitWindow.Show()
	'            strValue = obj.GeteCallState()
	'            If strValue.ToLower.Contains("error") Then
	'              m_Logger.LogError(String.Format("{0}.", strValue))
	'              strValue = String.Format("{0}", strValue)
	'            End If

	'          Catch ex As Exception
	'            m_Logger.LogInfo(String.Format("{0}. Fehler beim Warten auf Nachrichtenstatus. {1}", strMethodeName, ex.Message))
	'          Finally
	'            WaitWindow.Close()
	'          End Try

	'        End If
	'        _ClsProgSetting.GetMDData_XMLFile()
	'        Try
	'          CreateLogToKDKontaktDb(KDNr, ZHDNr, "MassenFax")
	'        Catch ex As Exception
	'          m_Logger.LogError(String.Format("{0}.Kunden-Kontakteintrag hinzufügen. {1}", strMethodeName, ex.Message))

	'        End Try

	'      Else
	'        m_Logger.LogError(String.Format("{0}", strValue))

	'      End If
	'      If i = 0 Then strFValue = strValue Else strLValue = strValue
	'    End If

	'  Next
	'  strValue = String.Format("{0} | {1}", strFValue, strLValue)
	'  Trace.WriteLine(String.Format("{0} | {1}", FaxNumber, KD_ZHD_Nr))

	'  Return strValue
	'End Function

	Private Function GetKDDbData4SendFax() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
		'Dim strQuery As String = Me._ExportSetting.SQL2Open

		Dim strBeginTrySql As String = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH"
		Dim strTestSql As String = String.Format("{0} SELECT * {1} FROM _Kundenliste_{2} ", _
															 strBeginTrySql, _
															 "Into #KD_Mailing", _
																ModulConstants.UserData.UserNr)
		strTestSql &= "SELECT Firma1 As [Kunde], (IsNull(Vorname,'') + ', ' + IsNull(Nachname, '')) [Zuständige Person], "
		strTestSql &= "KDNr, ZHDRecNr, (Case KD_Telefax_Mailing "
		strTestSql &= "When 1 Then '' "
		strTestSql &= "Else KDTelefax "
		strTestSql &= "End ) As "
		strTestSql &= "KDTelefax, "
		strTestSql &= "KD_Telefax_Mailing, "

		strTestSql &= "	(Case ZHD_Telefax_Mailing "
		strTestSql &= "When 1 Then '' "
		strTestSql &= "Else ZHDTelefax "
		strTestSql &= "End ) As 	"
		strTestSql &= "ZHDTelefax, "
		strTestSql &= "ZHD_Telefax_Mailing "

		'strTestSql &= "(IsNull(Vorname,'') + ', ' + IsNull(Nachname, '')) ZHDName "
		strTestSql &= "From #KD_Mailing Where KDTelefax + ZHDTelefax <> '' "
		strTestSql &= "Group By Firma1, Vorname, Nachname, KDNr, ZHDRecNr, KDTelefax, KD_Telefax_Mailing, ZHDTelefax , ZHD_Telefax_Mailing "
		'strTestSql &= "Group By KDNr, ZHDRecNr, KDTelefax, KD_Telefax_Mailing, ZHDTelefax , ZHD_Telefax_Mailing, Vorname, Nachname "
		strTestSql &= "Order By KDNr, Nachname, Vorname"

		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strTestSql, Conn)
		cmd.CommandType = CommandType.Text

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "KDData")

		Return ds.Tables(0)
	End Function

	Private Sub FillMyDataGrid()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strValue As String = String.Empty
		Dim dt As New DataTable
		Dim i As Integer = 0

		Try
			dt = GetKDDbData4SendFax()

		Catch ex As Exception
			Dim strMsg As String = String.Format("Error: Fehler bei Zuweisung der Datenbank.{0}{1}", vbNewLine, ex.Message)
			m_Logger.LogError(String.Format("{0}.Datenbank öffnen. {1}", strMethodeName, ex.Message))

			DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Datenbank zuweisen"), MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return
		End Try

		Try
			Dim Columnnames As String() = {"Kunde".ToLower, "Zuständige Person".ToLower, "KDNr".ToLower, "ZHDRecNr".ToLower,
																		 "KDTelefax".ToLower, "KD_Telefax_Mailing".ToLower,
																	"ZHDTelefax".ToLower, "ZHD_Telefax_Mailing".ToLower, "übernehmen?".ToLower}
			Dim ColumnCaption As String() = {"Kunde", "Zuständige Person", "Kunden-Nr.", "ZHD-Nr.", "Telefax (KD)", "Mailing (KD)",
																			 "Telefax (ZHD)", "Mailing (ZHD)",
																			 "Übernehmen?"}
			dt.Columns.Add("übernehmen?", GetType(Boolean))
			dt.Columns.Add("JobIdKD", GetType(String))
			dt.Columns.Add("JobIdZHD", GetType(String))
			grdContent4ImportIntoSPDb.DataSource = dt

			For Each col As GridColumn In Me.gvContent4InsertIntoSPDb.Columns
				Trace.WriteLine(col.FieldName)
				col.MinWidth = 0
				Dim strColName As String = col.FieldName.ToLower
				Try
					Dim bVisible As Boolean = Array.IndexOf(Columnnames, strColName) >= 0 And (strColName <> "kdnr" And strColName <> "zhdrecnr")
					If bVisible Then
						col.Visible = True
						col.Caption = m_Translate.GetSafeTranslationValue(ColumnCaption(Array.IndexOf(Columnnames, strColName))) ' col.GetCaption)
						col.OptionsColumn.AllowEdit = strColName = "übernehmen?"
						If strColName = "übernehmen?" Then
							col.AppearanceCell.BackColor = Color.Beige
						End If

					Else
						col.Visible = False

					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
					Trace.WriteLine(ex.Message)
					col.Visible = False

				End Try
				i += 1
			Next col

		Catch ex As Exception
			Dim strMsg As String = String.Format("Error: Fehler bei definition der Datenbankspalten.{0}{1}", vbNewLine, ex.Message)
			m_Logger.LogError(String.Format("{0}.Spalten anordnen. {1}", strMethodeName, ex.Message))

			DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Spalten auflisten"), MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return
		End Try

		i = 0
		Dim bDeleterec As Boolean
		Dim liSelectedFaxNumber As New List(Of String)
		Try
			Dim strMsg As String = m_Translate.GetSafeTranslationValue("Ihre Daten werden importiert. Bitte warten Sie einen Augenblick...")
			Me.bsiInfo.Caption = strMsg

			For Each myrow As DataRow In dt.Rows
				bDeleterec = False
				Dim kdFaxNumber As String = myrow.Item("KDTelefax").ToString
				Dim zhdFaxNumber As String = myrow.Item("ZHDTelefax").ToString
				If kdFaxNumber.Length <= 5 OrElse CBool(myrow.Item("KD_Telefax_Mailing")) Then kdFaxNumber = String.Empty
				If zhdFaxNumber.Length <= 5 OrElse CBool(myrow.Item("ZHD_Telefax_Mailing")) Then zhdFaxNumber = String.Empty


				If String.IsNullOrWhiteSpace(String.Join("", kdFaxNumber, zhdFaxNumber)) Then
					bDeleterec = True

				Else
					Dim str2Search As String = String.Format("{0}|{1}", kdFaxNumber, zhdFaxNumber).Replace(" ", "")
					If Not liSelectedFaxNumber.Contains(str2Search) Then
						liSelectedFaxNumber.Add(str2Search)

					Else
						bDeleterec = True
					End If

				End If

				If Not bDeleterec Then myrow("übernehmen?") = True
				If bDeleterec Then myrow.Delete() Else i += 1
			Next
			dt.AcceptChanges()
			Dim iRowCount As Integer = i
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Einträge wurden gefunden."), iRowCount)

		Catch ex As Exception
			Dim strMsg As String = String.Format("Error: Fehler bei öffnen der Datenbank.{0}{1}", vbNewLine, ex.Message)
			m_Logger.LogError(String.Format("{0}.Daten auflisten. {1}", strMethodeName, ex.Message))

			DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg), m_Translate.GetSafeTranslationValue("Daten auflisten"), MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return
		End Try

	End Sub

	Private Sub ContactLog(ByVal message As FaxReceiver, ByVal faxFile As String)
		If m_contagtLogger Is Nothing Then
			m_contagtLogger = New ContactLogger(New SPSSendMail.InitializeClass With {
				.MDData = ModulConstants.MDData,
				.ProsonalizedData = ModulConstants.PersonalizedData,
				.TranslationData = ModulConstants.TranslationData,
				.UserData = ModulConstants.UserData
			})
		End If

		Dim cType1 = "MassenFax"
		Dim cType2 As Short = 2

		m_contagtLogger.NewResponsiblePersonContact(message.KDNr, message.Address, faxFile, message.ZHDNr, DateTime.Now, Nothing, Nothing, DateTime.Now, Nothing, Nothing, Nothing, cType1, cType2, False, True, False)

	End Sub

#End Region

#Region "Event Handler"

	Private Sub OnMessageSent(ByVal faxMessage As FaxReceiver) Handles m_frmDoc2eCallWait.FaxSent

		Dim dataTable As DataTable = grdContent4ImportIntoSPDb.DataSource
		Dim sentList
		If faxMessage.ZHDNr = 0 Then
			sentList = (From r As DataRow In dataTable.Rows Where r.Item("JobIdKD").ToString() = faxMessage.JobId)
		Else
			sentList = (From r As DataRow In dataTable.Rows Where r.Item("JobIdZHD").ToString() = faxMessage.JobId)
		End If

		For Each row In sentList
			If faxMessage.ResponseCode = 0 Or faxMessage.ResponseCode = 11912 Or faxMessage.ResponseCode = 11913 Then
				lstVersandResult.Items.Add(String.Format("[success] Fax to [{0}]", faxMessage.Address))
				row("übernehmen?") = False
				grdContent4ImportIntoSPDb.RefreshDataSource()
				m_sendCount += 1
				bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Einträge wurden erfolgreich gesendet."), m_sendCount)
				ContactLog(faxMessage, Me.txtFilename.Text)
			Else
				Me.lstVersandResult.Items.Add(String.Format("[failure][{2}] SMS to  [{0}] - {1}", faxMessage.Address, faxMessage.ResponseCode.ToString, faxMessage.ResponseText))
			End If
		Next
	End Sub

	Private Sub OnSendFinished(ByVal result As String) Handles m_frmDoc2eCallWait.SendFinished
		Me.PanelControl1.Enabled = True
		Me.CmdClose.Enabled = True

		DevExpress.XtraEditors.XtraMessageBox.Show(result, "Send Fax", MessageBoxButtons.OK, MessageBoxIcon.Information)
		If m_frmDoc2eCallWait IsNot Nothing Then
			m_frmDoc2eCallWait.Dispose()
		End If
		If lstVersandResult.Items.Count > 0 Then
			Me.XtraTabControl1.SelectedTabPage = Me.xtabResultData
			' write lstVersandResult to file
			Dim fileName As String = Path.Combine(m_deleteHomeFolder, DateTime.Now.ToString("yyyyMMdd-HHmmss") + "_FAX.txt")
			Dim lines As List(Of String) = New List(Of String)
			lines.Add("-----------------------------------")
			lines.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
			lines.Add("Fax file: " + Me.txtFilename.Text)
			lines.Add("-----------------------------------")
			For Each line In lstVersandResult.Items
				lines.Add(line.ToString())
			Next
			File.WriteAllLines(fileName, lines.ToArray())
		End If
	End Sub


	Private Sub cmdLoad_Click(sender As System.Object, e As System.EventArgs) Handles cmdLoad.Click
		FillMyDataGrid()
	End Sub

	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub OnSendButtonClick(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim strFaxValue As String = "Success"
		Dim i As Integer = 0
		Dim fi As FileInfo

		Me.lstVersandResult.Items.Clear()
		Try
			If String.IsNullOrWhiteSpace(Me.txtFilename.Text) Or Not File.Exists(Me.txtFilename.Text) Then
				Dim strMsg As String = String.Format("Error: Die Datei wurde nicht gefunden.")
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, strMsg))
				Throw New Exception(strMsg)
				'Exit Sub
			Else
				fi = New FileInfo(Me.txtFilename.Text)
				If fi.Extension.Substring(1, fi.Extension.Length - 1).ToLower <> "pdf" Then
					Dim strMsg As String = String.Format("Sie versuchen eine Datei in {1}-Format zu senden.{0}Wir empfehlen, Ihre Datei in PDF-Format zu konvertieren und zu senden.",
																							 vbNewLine, fi.Extension.Substring(1, fi.Extension.Length - 1).ToUpper)
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(strMsg))

					Throw New Exception("Error: " & m_Translate.GetSafeTranslationValue("Datei Format nicht empfohlen!"))
				End If
			End If
		Catch ex As Exception
			Me.lstVersandResult.Items.Add(String.Format("{0}", ex.Message))
			Exit Sub
		Finally
			If Me.lstVersandResult.Items.Count > 0 Then Me.XtraTabControl1.SelectedTabPage = Me.xtabResultData
		End Try

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bitte warten Sie einen Augenblick") & "..."
		_ExportSetting.ExportFileName = Me.txtFilename.Text
		Me.MessageGuid = GetRandom(1111, 100001) ' Guid.NewGuid.ToString

		Dim bAllowed2Send As Boolean = True
		Dim liSelectedFaxNumber As New List(Of String)
		Dim strKDFax As String = String.Empty
		Dim strZHDFax As String = String.Empty
		Dim strFinalFax As String = String.Empty
		Dim strFinalKDZHDNr As String = String.Empty
		Me.lstVersandResult.Items.Clear()

		Try
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Ihre Daten werden importiert. Bitte warten Sie einen Augenblick") & "..."

			grdContent4ImportIntoSPDb.RefreshDataSource()
			Dim dt As DataTable = grdContent4ImportIntoSPDb.DataSource
			Me.SuspendLayout()
			Try

				Dim faxCollection As New FaxCollection With {.Filename2Send = Me.txtFilename.Text}

				For Each myrow As DataRow In dt.Rows
					strFinalFax = String.Empty
					strFinalKDZHDNr = String.Empty
					If myrow.RowState = DataRowState.Deleted Then

					Else
						If myrow("übernehmen?") Then
							bAllowed2Send = True
							strKDFax = myrow("KDTelefax").ToString
							strZHDFax = myrow("ZHDTelefax").ToString
							If Not Me.chkSendKD.Checked OrElse strKDFax.Length <= 5 OrElse CBool(myrow.Item("KD_Telefax_Mailing")) Then strKDFax = String.Empty
							If Not Me.chkSendZhd.Checked OrElse strZHDFax.Length <= 5 OrElse CBool(myrow.Item("ZHD_Telefax_Mailing")) Then strZHDFax = String.Empty

							'If (String.IsNullOrWhiteSpace(myrow.Item("KDTelefax").ToString) Or CBool(myrow.Item("KD_Telefax_Mailing"))) Then
							'If (String.IsNullOrWhiteSpace(strKDFax) Or CBool(myrow.Item("KD_Telefax_Mailing"))) Then
							'	strKDFax = String.Empty
							'	If (String.IsNullOrWhiteSpace(strZHDFax) Or CBool(myrow.Item("ZHD_Telefax_Mailing"))) Then
							'		strZHDFax = String.Empty
							'		bAllowed2Send = False
							'	End If

							'ElseIf CBool(myrow.Item("KD_Telefax_Mailing")) And CBool(myrow.Item("ZHD_Telefax_Mailing")) Then
							'	bAllowed2Send = False

							'End If

							bAllowed2Send = Not String.IsNullOrWhiteSpace(String.Join("", strKDFax, strZHDFax))
							If bAllowed2Send Then
								'If CBool(myrow.Item("KD_Telefax_Mailing")) Then strKDFax = String.Empty
								'If CBool(myrow.Item("ZHD_Telefax_Mailing")) Then strZHDFax = String.Empty

								'If Not Me.chkSendKD.Checked Then strKDFax = String.Empty
								'If Not Me.chkSendZhd.Checked Then strZHDFax = String.Empty

								If strKDFax = strZHDFax Then strZHDFax = String.Empty

								If liSelectedFaxNumber.Contains(strKDFax) Then strKDFax = String.Empty
								If liSelectedFaxNumber.Contains(strZHDFax) Then strZHDFax = String.Empty

								If Not String.IsNullOrWhiteSpace(strKDFax) Then liSelectedFaxNumber.Add(strKDFax)
								If Not String.IsNullOrWhiteSpace(strZHDFax) Then liSelectedFaxNumber.Add(strZHDFax)

								strFinalFax = String.Format("{0}|{1}", strKDFax, strZHDFax)
								strFinalKDZHDNr = String.Format("{0}|{1}", myrow.Item("KDNr"), myrow.Item("ZHDRecNr"))
								If Trim(strFinalFax).Length < 10 Then bAllowed2Send = False

							End If

							If bAllowed2Send Then
								Dim jobId As String
								jobId = faxCollection.AddReveiver(strKDFax, myrow.Item("KDNr"), 0)
								If Not String.IsNullOrEmpty(jobId) Then
									myrow.Item("JobIdKD") = jobId
								End If
								jobId = faxCollection.AddReveiver(strZHDFax, myrow.Item("KDNr"), myrow.Item("ZHDRecNr"))
								If Not String.IsNullOrEmpty(jobId) Then
									myrow.Item("JobIdZHD") = jobId
								End If
							End If

						End If

					End If
				Next

				Try
					Dim msg As String = String.Empty
					If faxCollection.Receivers.Count = 0 Then
						msg = m_Translate.GetSafeTranslationValue("Sie haben keine Aufträge zum Versand!")
						m_UtilityUI.ShowOKDialog(msg, m_Translate.GetSafeTranslationValue("Versand starten"), MessageBoxIcon.Warning)

						Return

					End If

					For Each itm In faxCollection.Receivers
						Trace.WriteLine(itm.Address)
					Next

					msg = m_Translate.GetSafeTranslationValue("Hiermit starten Sie den Fax-Versand. Es werden {0} Faxe versendet. Möchten Sie wirklich mit Versand beginnen?")
					msg = String.Format(msg, faxCollection.Receivers.Count)
					Dim rReult = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Fax-Versand"), MessageBoxDefaultButton.Button1)

					If Not rReult Then
						m_Logger.LogWarning(String.Format("{0}. Versandvorgang wurde abgebrochen.", strMethodeName))

						Return
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

					Return

				End Try

				If faxCollection.Receivers.Count > 0 Then
					m_sendCount = 0
					m_frmDoc2eCallWait = New frmDoc2eCallWait(faxCollection)
					m_frmDoc2eCallWait.StartPosition = FormStartPosition.Manual
					m_frmDoc2eCallWait.Location = New Point(Me.Location.X + (Me.Width - m_frmDoc2eCallWait.Width) / 2, Me.Location.Y + (Me.Height - m_frmDoc2eCallWait.Height) / 2)
					m_frmDoc2eCallWait.Show()

					Me.PanelControl1.Enabled = False
					Me.CmdClose.Enabled = False
				End If

			Catch ex As Exception
				Dim strMsg = String.Format("Error: Fehler bei Zuweisung der Datenbank.{0}{1}", vbNewLine, ex.ToString)
				m_Logger.LogError(String.Format("{0}.Daten auflisten. {1}", strMethodeName, ex.Message))
				Me.lstVersandResult.Items.Add(String.Format("{0}", strMsg))

				m_UtilityUI.ShowOKDialog(strMsg)
				Return
			End Try

		Catch ex As Exception
			Dim strMsg As String = String.Format("Error: Fehler bei Zuweisung der Datenbank.{0}{1}", vbNewLine, ex.ToString)
			m_Logger.LogError(String.Format("{0}.Daten auflisten. {1}", strMethodeName, ex.ToString))
			Me.lstVersandResult.Items.Add(String.Format("{0}", strMsg))

			m_UtilityUI.ShowOKDialog(strMsg)

			Return
		Finally
			Me.ResumeLayout()
		End Try

	End Sub

	Private Sub frmDoc2eCall_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.frmDoc2eCallLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.iDoc2eCallWidth = Me.Width
			My.Settings.iDoc2eCallHeight = Me.Height

			My.Settings.sendeCall2KD = Me.chkSendKD.Checked
			My.Settings.sendeCall2Zhd = Me.chkSendZhd.Checked
			My.Settings.sendeCallOne = Me.chkSendOne.Checked

			My.Settings.Filename4Doc2eCall = Me.txtFilename.Text

			My.Settings.Save()
		End If

	End Sub

	Private Sub frmDoc2eCall_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsMainSetting.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				Me.Width = Math.Max(My.Settings.iDoc2eCallWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iDoc2eCallHeight, Me.Height)

				If My.Settings.frmDoc2eCallLocation <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmDoc2eCallLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		Try
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
			Try
				Me.chkSendKD.Checked = CBool(My.Settings.sendeCall2KD)
				Me.chkSendZhd.Checked = CBool(My.Settings.sendeCall2Zhd)
				Me.chkSendOne.Checked = CBool(My.Settings.sendeCallOne)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setzen von CheckBoxen: {1}", strMethodeName, ex.Message))

				Me.chkSendKD.Checked = False
				Me.chkSendZhd.Checked = False
				Me.chkSendOne.Checked = False

			End Try

			Try
				FillMyDataGrid()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Füllen von Daten in Grid: {1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Felder füllen: {1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And ModulConstants.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine, _
														 GetExecutingAssembly().FullName, _
														 GetExecutingAssembly().Location, _
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

#End Region

#Region "Private Classes"

	Public Class FaxCollection

		Public Property Filename2Send As String

		Public Property Receivers As List(Of FaxReceiver)

		Public Sub New()
			Receivers = New List(Of FaxReceiver)()
		End Sub

		Public Function AddReveiver(ByVal Address As String, ByVal KDNr As String, ByVal ZHDNr As String) As String
			If String.IsNullOrEmpty(Address) Then
				Return String.Empty
			End If

			Dim jobId = GenerateId()
			Dim strFaxToValue As String = Address
			If Not strFaxToValue.StartsWith("00") And Not strFaxToValue.StartsWith("+") Then
				If strFaxToValue.StartsWith("0") Then strFaxToValue = strFaxToValue.Remove(0, 1)
				strFaxToValue = "0041" & strFaxToValue
			End If
			strFaxToValue = strFaxToValue.Replace(" ", "").Replace("-", "").Replace("/", "")
			Address = strFaxToValue

			Dim duplicate = (From r In Receivers Where r.Address = Address).FirstOrDefault()

			If duplicate Is Nothing Then
				Receivers.Add(New FaxReceiver With {
											 .Address = Address,
											 .KDNr = KDNr,
											 .ZHDNr = ZHDNr,
											 .JobId = jobId
											 })
				Return jobId
			End If
			Return String.Empty
		End Function

		Private m_random As Random = New Random

		Private Function GenerateId() As String
			Dim KeyLength As Integer = 45
			Dim a As String = "ABCDEFGHJKLMNOPQRSTUVWXYZ1234567890_-*@#?)/)(%+=¦"
			Dim chars() As Char = New Char((a.Length) - 1) {}
			chars = a.ToCharArray
			Dim data() As Byte = New Byte((KeyLength) - 1) {}
			Dim crypto As RNGCryptoServiceProvider = New RNGCryptoServiceProvider
			crypto.GetNonZeroBytes(data)
			Dim result As StringBuilder = New StringBuilder(KeyLength)
			For Each b As Byte In data
				result.Append(chars(b Mod (chars.Length - 1)))
			Next

			Return String.Format("{0}", result.ToString)
		End Function

	End Class

#End Region


End Class

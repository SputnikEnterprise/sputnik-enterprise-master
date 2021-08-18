
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.StringBuilder
Imports System.Collections.Generic
Imports DevExpress.XtraBars.Alerter
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.LookAndFeel

Imports SPProgUtility.Mandanten

Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging


Public Class frmSendOffer
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private Fields"

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

	Private m_md As Mandant
	Private m_UtilityUI As UtilityUI

	Private SendSuccess As Boolean
	Private _ClsProgsetting As SPProgUtility.ClsProgSettingPath
	Private m_SmtpServer As String

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		_ClsProgsetting = New SPProgUtility.ClsProgSettingPath
		m_md = New Mandant
		m_UtilityUI = New UtilityUI

		m_SmtpServer = _ClsProgsetting.GetSmtpServer()

		TranslateControls()


	End Sub


#End Region


	Private Sub LoadTemplateDropDown()

		cboTemplate.Properties.Items.Clear()
		LoadMailTemplateData(Me.cboTemplate, False)

		If cboTemplate.Properties.Items.Count > 0 Then
			cboTemplate.EditValue = cboTemplate.Properties.Items(0)
		Else
			cboTemplate.EditValue = Nothing
		End If

	End Sub

	Private Sub LoadSenderDropDown()
		Dim strMailFrom As String = m_InitializationData.UserData.UserMDeMail

		cboFromField.Properties.Items.Clear()

		' 1 - Benutzer eMail-Adresse
		' 2 - System eMail-Adresse
		Try
			strMailFrom = m_InitializationData.UserData.UsereMail
			If Not String.IsNullOrWhiteSpace(strMailFrom) Then cboFromField.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("1 - Benutzer eMail-Adresse ({0})"),
						strMailFrom), strMailFrom))

			If m_InitializationData.UserData.UsereMail <> m_InitializationData.UserData.UserMDeMail Then
				strMailFrom = m_InitializationData.UserData.UserMDeMail
				If Not String.IsNullOrWhiteSpace(strMailFrom) Then cboFromField.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("2 - System eMail-Adresse ({0})"),
							strMailFrom), strMailFrom))
			End If

			cboFromField.EditValue = cboFromField.Properties.Items.Item(0)

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Private Sub LoadRecipientDropDown()

		cboToField.Properties.Items.Clear()
		Try
			Dim Customer = LoadCustomerData(ClsDataDetail.GetKDNr, ClsDataDetail.GetZHDNr)

			If Customer Is Nothing Then
				m_UtilityUI.ShowErrorDialog("Die Kundenndaten konnten nicht geladen werden.")
				Return
			End If

			cboToField.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("1 - Kunden-Mailadresse ({0})"), Customer.CustomerEMail), "KDeMail"))
			If Not String.IsNullOrWhiteSpace(Customer.cResponsibleEMail) AndAlso Customer.CustomerEMail <> Customer.cResponsibleEMail Then
				If ClsDataDetail.GetZHDNr > 0 Then cboToField.Properties.Items.Add(New ComboBoxItem(String.Format(m_Translate.GetSafeTranslationValue("2 - Zuständige Person-Mailadresse({0})"),
							Customer.cResponsibleEMail), "ZHDeMail"))
			End If

			cboToField.EditValue = cboToField.Properties.Items.Item(0)


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally

		End Try

	End Sub


	Private Sub OnbbiSend_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSend.ItemClick

		ClsDataDetail.CheckForMailSent = False
		bbiWait.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		SendSuccess = False

		Try

			If Not AllowedToSend(False) Then Return
			Try
				BackgroundWorker1.WorkerSupportsCancellation = True
				BackgroundWorker1.WorkerReportsProgress = True
				BackgroundWorker1.RunWorkerAsync()		' Multithreading starten

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			bbiWait.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		End Try

	End Sub

	Function AllowedToSend(ByVal bAsTest As Boolean) As Boolean
		Dim result As Boolean = True

		Try
			If Not Me.chkOffblatt.Checked Then ClsDataDetail.GetAttachmentFile(0) = ""
			ClsDataDetail.SendWithPDoks = Me.chkOffPBlatt.Checked
			ClsDataDetail.SendWithMADoks = Me.chkOffMABlatt.Checked
			If bAsTest Then
				ClsDataDetail.GeteMailFieldToSend = "#UsereMail"
			Else
				If Me.cboToField.Text <> "" Then
					ClsDataDetail.GeteMailFieldToSend = DirectCast(Me.cboToField.SelectedItem, ComboBoxItem).Value
				Else
					ClsDataDetail.GeteMailFieldToSend = "KDeMail"
				End If
			End If

			If Me.cboFromField.Text <> "" Then
				ClsDataDetail.GeteMailFrom = DirectCast(Me.cboFromField.SelectedItem, ComboBoxItem).Value
			Else
				ClsDataDetail.GeteMailFrom = _ClsProgsetting.GetUserMail
				If ClsDataDetail.GeteMailFrom = String.Empty Then
					ClsDataDetail.GeteMailFrom = GeteMailFrom()
				End If
			End If

			If Me.cboTemplate.Text <> "" Then
				ClsDataDetail.GetMailTemplateFilename = GetTemplateFileFromDocDb(DirectCast(Me.cboTemplate.SelectedItem,  _
																																 ComboBoxItem).Value).Item(0)
			Else
				ClsDataDetail.GetMailTemplateFilename = String.Format("{0}{1}", _
																															_ClsProgsetting.GetMDTemplatePath, _
																															FileIO.FileSystem.GetName(ClsDataDetail.GetMailTemplateFilename))

			End If

			Try
				If ClsDataDetail.GeteMailFieldToSend = String.Empty Then
					Throw New NullReferenceException(m_Translate.GetSafeTranslationValue("E-Mail Empfänger ist fehlerhaft."))

				ElseIf ClsDataDetail.GeteMailFrom = String.Empty Then
					Throw New NullReferenceException(m_Translate.GetSafeTranslationValue("E-Mail Absender ist fehlerhaft."))

				ElseIf ClsDataDetail.GetMailTemplateFilename = String.Empty Then
					Throw New NullReferenceException(m_Translate.GetSafeTranslationValue("Die E-Mail-Vorlage wurde nicht gefunden."))

				End If

			Catch ex As NullReferenceException
				Dim strMsg As String = String.Format("Sie haben fehlerhafte Daten erfasst.{0}{1}{0}Das Programm wird beendet.", _
																						 vbNewLine, ex.Message)
				m_UtilityUI.ShowErrorDialog(strMsg)
				Return False

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)
				Return False
			End Try

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			Return False
		End Try

		Return result
	End Function


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Me.bbiSend.Enabled = False
		Me.bbiWait.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

		Try
			SendSuccess = OpenConnection(False)


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Try

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			m_UtilityUI.ShowErrorDialog("Fehler in Ihrer Anwendung." & vbNewLine & e.Error.ToString)
		Else
			If e.Cancelled = True Then
				Me.bbiSend.Enabled = True
				m_UtilityUI.ShowErrorDialog("Aktion abgebrochen!")

			Else
				BackgroundWorker1.CancelAsync()
				Me.bbiWait.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

			End If

			Me.bbiSend.Enabled = True
			System.Media.SystemSounds.Asterisk.Play()

			Dim msg As String = m_Translate.GetSafeTranslationValue(If(SendSuccess, "Ihre Nachricht wurde erfolgreich gesendet.", "Ihre Nachricht konnte nicht erfolgreich gesendet werden!"))
			If SendSuccess Then
				m_UtilityUI.ShowInfoDialog(msg)
			Else
				m_UtilityUI.ShowErrorDialog(msg)
			End If


		End If

	End Sub

#End Region


	Private Sub frmSendOffer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

		My.Settings.bWithOffblatt = If(Me.chkOffblatt.Checked, True, False)
		My.Settings.bWithOffPBlatt = If(Me.chkOffPBlatt.Checked, True, False)
		My.Settings.bWithOffMABlatt = If(Me.chkOffMABlatt.Checked, True, False)

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.frmOffMailLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.iOffMailWidth = Me.Width
			My.Settings.iOffMailHeight = Me.Height
		End If
		My.Settings.Save()

	End Sub

	Private Sub TranslateControls()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.grpAnhaenge.Text = m_Translate.GetSafeTranslationValue(Me.grpAnhaenge.Text)
			Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
			Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
			Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)
			Me.bbiSend.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSend.Caption)
			Me.bbiSendAsTest.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSendAsTest.Caption)
			Me.chkOffblatt.Text = m_Translate.GetSafeTranslationValue(Me.chkOffblatt.Text)
			Me.chkOffMABlatt.Text = m_Translate.GetSafeTranslationValue(Me.chkOffMABlatt.Text)
			Me.chkOffPBlatt.Text = m_Translate.GetSafeTranslationValue(Me.chkOffPBlatt.Text)

			Me.lblEmpfaenger.Text = m_Translate.GetSafeTranslationValue(Me.lblEmpfaenger.Text)

			Me.lblAbsender.Text = m_Translate.GetSafeTranslationValue(Me.lblAbsender.Text)
			Me.lblVorlage.Text = m_Translate.GetSafeTranslationValue(Me.lblVorlage.Text)

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


	End Sub

	Private Sub frmSendOffer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		bbiWait.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		Try
			m_Logger.LogInfo(String.Format("{0}. FormStyling", strMethodeName))
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(0, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.ToString))

		End Try

		Try
			m_Logger.LogInfo(String.Format("{0}. 1. FormPosition", strMethodeName))
			If My.Settings.frmOffMailLocation <> String.Empty Then
				m_Logger.LogInfo(String.Format("{0}. 2. FormPosition", strMethodeName))
				Me.Width = Math.Max(My.Settings.iOffMailWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iOffMailHeight, Me.Height)
				Dim aLoc As String() = My.Settings.frmOffMailLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))

		End Try

		Try

			m_Logger.LogDebug(String.Format("{0}. ClsDataDetail.GetMailTemplateFilename: {1}", strMethodeName, ClsDataDetail.GetMailTemplateFilename))

			m_Logger.LogDebug(String.Format("{0}. 2. Endphase", strMethodeName))

			LoadSenderDropDown()
			LoadRecipientDropDown()
			LoadTemplateDropDown()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Function GetTemplateFileFromDocDb(ByVal JobNr As String) As List(Of String)
		Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim liResult As New List(Of String)

		If JobNr = String.Empty Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keine Vorlage ausgewählt. Bitte wählen Sie aus der Liste eine Vorlage aus."))
			Return liResult
		End If
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()

			param = cmd.Parameters.AddWithValue("@JobNr", JobNr)
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader					' Dokumentendatenbank
			rDocrec.Read()
			If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
				liResult.Add(_ClsProgsetting.GetMDTemplatePath & rDocrec("DocName").ToString)
				liResult.Add(rDocrec("Bezeichnung").ToString)
				If IsDBNull(rDocrec("ParamCheck")) Or (rDocrec("ParamCheck").ToString = String.Empty) Then
					liResult.Add("0")
				Else
					liResult.Add(CStr(IIf(CBool(rDocrec("ParamCheck")), "1", "0")))
				End If

				If IsDBNull(rDocrec("KonvertName")) Or (rDocrec("KonvertName").ToString = String.Empty) Then
					liResult.Add("0")
				Else
					liResult.Add(CStr(IIf(CBool(rDocrec("KonvertName")), "1", "0")))
				End If

				If IsDBNull(rDocrec("ZoomProz")) Or (rDocrec("ZoomProz").ToString = String.Empty) Then
					liResult.Add("100")
				Else
					liResult.Add(CStr(IIf(CInt(rDocrec("ZoomProz")) = 0, "150", CInt(rDocrec("ZoomProz")))))
				End If

				If IsDBNull(rDocrec("Anzahlkopien")) Or (rDocrec("Anzahlkopien").ToString = String.Empty) Then
					liResult.Add("1")
				Else
					liResult.Add(CStr(IIf(CInt(rDocrec("Anzahlkopien")) = 0, "1", CInt(rDocrec("Anzahlkopien")))))
				End If

				If IsDBNull(rDocrec("TempDocPath")) Or (rDocrec("TempDocPath").ToString = String.Empty) Then
					liResult.Add(_ClsReg.AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.Personal)))
				Else
					liResult.Add(_ClsReg.AddDirSep(rDocrec("TempDocPath").ToString))
				End If

				If IsDBNull(rDocrec("ExportedFileName")) Or (rDocrec("ExportedFileName").ToString = String.Empty) Then
					liResult.Add(Path.GetFileNameWithoutExtension(rDocrec("DocName").ToString))
				Else
					liResult.Add(Path.GetFileNameWithoutExtension(rDocrec("ExportedFileName").ToString))
				End If

			End If
			rDocrec.Close()


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(Err.ToString)
			liResult.Clear()

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Return liResult
	End Function

	Private Sub OnbtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Me.Dispose()
	End Sub

	Private Sub OnbbiSendAsTest_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSendAsTest.ItemClick
		Dim SendTest As Boolean = True

		ClsDataDetail.CheckForMailSent = False
		Me.bbiWait.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		SendSuccess = False

		Try
			If Not AllowedToSend(SendTest) Then Return

			Try
				SendSuccess = OpenConnection(SendTest)

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try
			System.Media.SystemSounds.Asterisk.Play()

			Dim msg As String = m_Translate.GetSafeTranslationValue(If(SendSuccess, "Ihre Nachricht wurde erfolgreich gesendet.", "Ihre Nachricht konnte nicht erfolgreich gesendet werden!"))

			If SendSuccess Then
				m_UtilityUI.ShowInfoDialog(msg)
			Else
				m_UtilityUI.ShowErrorDialog(msg)
			End If


		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			Me.bbiWait.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		End Try

	End Sub


End Class

''' <summary>
''' Klasse für die ComboBox, um Text und Wert zu haben.
''' Das Item wird mit den Parameter Text für die Anzeige und
''' Value für den Wert zur ComboBox hinzugefügt.
''' </summary>
''' <remarks></remarks>
Class ComboBoxItem

	Public Text As String
	Public Value As String

	Public Sub New(ByVal text As String, ByVal val As String)
		Me.Text = text
		Me.Value = val
	End Sub

	Public Overrides Function ToString() As String
		Return Text
	End Function

End Class

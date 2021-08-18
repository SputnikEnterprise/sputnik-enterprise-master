Imports System.Windows.Forms

Namespace UI

  ''' <summary>
  ''' UI utility functions.
  ''' </summary>
  Public Class UtilityUI

		Private m_MessageBody As String
		Private m_MessageCaption As String
		Private m_Icon As MessageBoxIcon
		Private m_Owner As System.Windows.Forms.IWin32Window
		Private m_DelayTime As Integer?
		Private m_DefaultButton As MessageBoxDefaultButton

		''' <summary>
		''' Shows an error message.
		''' </summary>
		''' <param name="text">The text to show.</param>
		Public Sub ShowErrorDialog(ByVal text As String)
			If String.IsNullOrWhiteSpace(text) Then Return

			m_Icon = MessageBoxIcon.Error
			m_MessageBody = text
			m_MessageCaption = "Fehler"

			ShowMessageBoxWithArguments()

		End Sub

		''' <summary>
		''' Shows an error message.
		''' </summary>
		''' <param name="owner">The owner.</param>
		''' <param name="text">The text to show.</param>
		Public Sub ShowErrorDialog(ByVal owner As System.Windows.Forms.IWin32Window, ByVal text As String)
			If String.IsNullOrWhiteSpace(text) Then Return
			m_Icon = MessageBoxIcon.Error
			m_Owner = owner

			m_MessageBody = text
			m_MessageCaption = "Fehler"

			ShowMessageBoxWithArguments()

		End Sub

		Public Sub ShowErrorDialog(ByVal text As String, Optional ByVal caption As String = "Fehler", Optional icon As MessageBoxIcon = MessageBoxIcon.Error)
			If String.IsNullOrWhiteSpace(text) Then Return

			m_Icon = icon
			m_MessageBody = text
			m_MessageCaption = caption

			ShowMessageBoxWithArguments()

			'DevExpress.XtraEditors.XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, icon)
		End Sub

		Public Sub ShowErrorDialog(ByVal text As String, ByVal delayTime As Integer?)
			If String.IsNullOrWhiteSpace(text) Then Return
			Dim caption As String = "Fehler"
			m_Icon = MessageBoxIcon.Error
			m_DelayTime = delayTime

			m_MessageBody = text
			m_MessageCaption = caption

			ShowMessageBoxWithArguments()

		End Sub


#Region "showing infodialog"

		''' <summary>
		''' Shows an info message.
		''' </summary>
		''' <param name="text">The text to show.</param>
		Public Sub ShowInfoDialog(ByVal text As String)
			If String.IsNullOrWhiteSpace(text) Then Return
			Dim caption As String = "Information"
			m_Icon = MessageBoxIcon.Information

			m_MessageBody = text
			m_MessageCaption = caption

			ShowMessageBoxWithArguments()

		End Sub

		''' <summary>
		''' Shows an info message.
		''' </summary>
		''' <param name="owner">The owner.</param>
		''' <param name="text">The text to show.</param>
		Public Sub ShowInfoDialog(ByVal owner As System.Windows.Forms.IWin32Window, ByVal text As String)
			If String.IsNullOrWhiteSpace(text) Then Return
			Dim caption As String = "<b>Information</b>"

			m_Icon = MessageBoxIcon.Information
			m_Owner = owner

			m_MessageBody = text
			m_MessageCaption = String.Format("{0}", caption)

			ShowMessageBoxWithArguments()

		End Sub

		Public Sub ShowInfoDialog(ByVal text As String, ByVal caption As String, ByVal delayTime As Integer?)
			If String.IsNullOrWhiteSpace(text) Then Return

			m_Icon = MessageBoxIcon.Information
			m_DelayTime = delayTime
			m_MessageBody = text
			m_MessageCaption = caption

			ShowMessageBoxWithArguments()

		End Sub

		Public Sub ShowInfoDialog(ByVal text As String, Optional ByVal caption As String = "Information", Optional icon As MessageBoxIcon = MessageBoxIcon.Information)
			If String.IsNullOrWhiteSpace(text) Then Return

			m_Icon = icon
			m_MessageBody = text
			m_MessageCaption = caption

			ShowMessageBoxWithArguments()

		End Sub

#End Region


#Region "showing yes or no dialog"

		Public Function ShowYesNoDialog(ByVal text As String) As Boolean
			Dim result As DialogResult
			If String.IsNullOrWhiteSpace(text) Then Return False
			Dim caption As String = "Information"

			m_Icon = MessageBoxIcon.Question
			m_DefaultButton = MessageBoxDefaultButton.Button1
			m_MessageBody = text
			m_MessageCaption = caption

			result = ShowYesNOMessageBoxWithArguments()

			'Dim result As DialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(text, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
			Return result = DialogResult.Yes
		End Function

		''' <summary>
		''' Shows a yes/no dialog
		''' </summary>
		''' <param name="text">The text to show.</param>
		''' <param name="caption">Optional caption.</param>
		''' <returns>Boolean flag indicating if user pressed yes button.</returns>
		Public Function ShowYesNoDialog(ByVal text As String, ByVal caption As String) As Boolean
			Dim result As DialogResult
			If String.IsNullOrWhiteSpace(text) Then Return False

			m_Icon = MessageBoxIcon.Question
			m_DefaultButton = MessageBoxDefaultButton.Button1
			m_MessageBody = text
			m_MessageCaption = caption

			result = ShowYesNOMessageBoxWithArguments()

			'Dim result As DialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
			Return result = DialogResult.Yes
		End Function

		Public Function ShowYesNoDialog(ByVal text As String, ByVal caption As String, ByVal defaultButton As MessageBoxDefaultButton, Optional ByVal icon As MessageBoxIcon = MessageBoxIcon.Question) As Boolean
			Dim result As DialogResult
			If String.IsNullOrWhiteSpace(text) Then Return False

			m_Icon = icon
			m_DefaultButton = defaultButton
			m_MessageBody = text
			m_MessageCaption = caption

			result = ShowYesNOMessageBoxWithArguments()

			'Dim result As DialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(text, caption, MessageBoxButtons.YesNo, icon, defaultButton)
			Return result = DialogResult.Yes
		End Function

		Public Function ShowYesNoDialog(ByVal text As String, ByVal caption As String, ByVal defaultButton As MessageBoxDefaultButton) As Boolean
			Dim result As DialogResult
			If String.IsNullOrWhiteSpace(text) Then Return False

			m_Icon = MessageBoxIcon.Question
			m_DefaultButton = defaultButton
			m_MessageBody = text
			m_MessageCaption = caption

			result = ShowYesNOMessageBoxWithArguments()

			'Dim result As DialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, defaultButton)
			Return result = DialogResult.Yes
		End Function

		''' <summary>
		''' Shows a yes/no dialog
		''' </summary>
		''' <param name="owner">The owner.</param>
		''' <param name="text">The text to show.</param>
		''' <param name="caption">Optional caption.</param>
		''' <returns>Boolean flag indicating if user pressed yes button.</returns>
		Public Function ShowYesNoDialog(ByVal owner As System.Windows.Forms.IWin32Window, ByVal text As String, Optional ByVal caption As String = "Information") As Boolean
			Dim result As DialogResult
			If String.IsNullOrWhiteSpace(text) Then Return False

			m_Icon = MessageBoxIcon.Question
			m_DefaultButton = MessageBoxDefaultButton.Button1
			m_Owner = owner
			m_MessageBody = text
			m_MessageCaption = caption

			result = ShowYesNOMessageBoxWithArguments()

			'Dim result As DialogResult = DevExpress.XtraEditors.XtraMessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			Return result = DialogResult.Yes
    End Function

#End Region

#Region "showing ok dialog"

		''' <summary>
		''' Shows an OK dialog.
		''' </summary>
		''' <param name="text">The text to show.</param>
		''' <param name="caption">Optional caption.</param>
		''' <param name="icon">Optional icon.</param>
		Public Sub ShowOKDialog(ByVal text As String, Optional ByVal caption As String = "Information", Optional icon As MessageBoxIcon = MessageBoxIcon.Information)
			If String.IsNullOrWhiteSpace(text) Then Return

			'm_MessageBody = text
			'm_MessageCaption = caption
			'DevExpress.XtraEditors.XtraMessageBox.Show(text, caption, MessageBoxButtons.OK, icon)

			m_Icon = icon
			m_MessageBody = text
			m_MessageCaption = caption

			ShowMessageBoxWithArguments()

		End Sub


		''' <summary>
		''' Shows an OK dialog.
		''' </summary>
		''' <param name="owner">The owner.</param>
		''' <param name="text">The text to show.</param>
		''' <param name="caption">Optional caption.</param>
		''' <param name="icon">Optional icon.</param>
		Public Sub ShowOKDialog(ByVal owner As System.Windows.Forms.IWin32Window, ByVal text As String, Optional ByVal caption As String = "Information", Optional icon As MessageBoxIcon = MessageBoxIcon.Information)
			If String.IsNullOrWhiteSpace(text) Then Return

			'm_MessageBody = text
			'm_MessageCaption = caption
			'DevExpress.XtraEditors.XtraMessageBox.Show(owner, text, caption, MessageBoxButtons.OK, icon)



			m_Icon = icon
			m_MessageBody = text
			m_MessageCaption = caption
			m_Owner = owner

			ShowMessageBoxWithArguments()






		End Sub

#End Region


		Private Sub ShowMessageBoxWithArguments()

			Dim args As New DevExpress.XtraEditors.XtraMessageBoxArgs()

			If m_DelayTime.HasValue Then
				args.AutoCloseOptions.Delay = m_DelayTime
				args.AutoCloseOptions.ShowTimerOnDefaultButton = True
				args.DefaultButtonIndex = 0
			End If

			If Not m_Owner Is Nothing Then args.Owner = m_Owner

			args.Caption = String.Format("<b>{0}</b>", m_MessageCaption)
			args.Text = m_MessageBody
			args.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
			args.Buttons = New DialogResult() {DialogResult.OK} ', DialogResult.Cancel}

			Select Case m_Icon
				Case MessageBoxIcon.Error
					args.Icon = System.Drawing.SystemIcons.Error
				Case MessageBoxIcon.Asterisk
					args.Icon = System.Drawing.SystemIcons.Asterisk
				Case MessageBoxIcon.Exclamation
					args.Icon = System.Drawing.SystemIcons.Exclamation
				Case MessageBoxIcon.Hand
					args.Icon = System.Drawing.SystemIcons.Hand
				Case MessageBoxIcon.Question
					args.Icon = System.Drawing.SystemIcons.Question
				Case MessageBoxIcon.Stop
					args.Icon = System.Drawing.SystemIcons.Shield
				Case MessageBoxIcon.Warning
					args.Icon = System.Drawing.SystemIcons.Warning
				Case Else
					args.Icon = System.Drawing.SystemIcons.Information

			End Select

			DevExpress.XtraEditors.XtraMessageBox.Show(args)

		End Sub

		Private Function ShowYesNOMessageBoxWithArguments() As DialogResult
			Dim result As DialogResult
			Dim args As New DevExpress.XtraEditors.XtraMessageBoxArgs()

			If m_DelayTime.HasValue Then
				args.AutoCloseOptions.Delay = m_DelayTime
				args.AutoCloseOptions.ShowTimerOnDefaultButton = True
			End If

			If Not m_Owner Is Nothing Then args.Owner = m_Owner

			args.DefaultButtonIndex = m_DefaultButton
			args.Caption = String.Format("<b>{0}</b>", m_MessageCaption)
			args.Text = m_MessageBody
			args.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
			args.Buttons = New DialogResult() {DialogResult.Yes, DialogResult.No}

			Select Case m_Icon
				Case MessageBoxIcon.Error
					args.Icon = System.Drawing.SystemIcons.Error
				Case MessageBoxIcon.Asterisk
					args.Icon = System.Drawing.SystemIcons.Asterisk
				Case MessageBoxIcon.Exclamation
					args.Icon = System.Drawing.SystemIcons.Exclamation
				Case MessageBoxIcon.Hand
					args.Icon = System.Drawing.SystemIcons.Hand
				Case MessageBoxIcon.Question
					args.Icon = System.Drawing.SystemIcons.Question
				Case MessageBoxIcon.Stop
					args.Icon = System.Drawing.SystemIcons.Shield
				Case MessageBoxIcon.Warning
					args.Icon = System.Drawing.SystemIcons.Warning
				Case Else
					args.Icon = System.Drawing.SystemIcons.Information

			End Select

			result = DevExpress.XtraEditors.XtraMessageBox.Show(args)

			Return result
		End Function


		''' <summary>
		''' Öffnet einen Dialog zum Öffnen einer einzelnen Datei.
		''' </summary>
		''' <returns>Dateiname mit Pfad oder Nothing bei Abbruch.</returns>
		''' <remarks></remarks>
		Public Function ShowOpenFileDialog() As String
			Return Me.ShowOpenFileDialog(Nothing)
		End Function

		''' <summary>
		''' Öffnet einen Dialog zum Öffnen einer einzelnen Datei.
		''' </summary>
		''' <param name="owner"></param>
		''' <param name="filter"></param>
		''' <returns>Dateiname mit Pfad oder Nothing bei Abbruch.</returns>
		''' <remarks></remarks>
		Public Function ShowOpenFileDialog(ByVal owner As System.Windows.Forms.IWin32Window, Optional ByVal filter As String = Nothing) As String
      Dim openFileDialog = New OpenFileDialog()
      openFileDialog.Multiselect = False
      openFileDialog.Filter = filter
      openFileDialog.FilterIndex = 0
      Dim result = openFileDialog.ShowDialog(owner)
      Select Case result
        Case DialogResult.OK
          Return openFileDialog.FileName
        Case Else
          Return Nothing
      End Select
    End Function

    ''' <summary>
    ''' Opens an email.
    ''' </summary>
    ''' <param name="emailAddress">The email address.</param>
    ''' <param name="subject">The subject.</param>
    ''' <param name="body">The body.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function OpenEmail(ByVal emailAddress As String, Optional ByVal subject As String = "", Optional ByVal body As String = "") As Boolean

      Dim success As Boolean = True
      Dim sParams As String
      sParams = emailAddress
      If LCase(Strings.Left(sParams, 7)) <> "mailto:" Then
        sParams = "mailto:" & sParams
      End If

      If subject <> "" Then
        sParams = sParams & "?subject=" & subject
      End If

      If body <> "" Then
        sParams = sParams & IIf(subject = "", "?", "&")
        sParams = sParams & "body=" & body
      End If

      Try
        System.Diagnostics.Process.Start(sParams)

      Catch
        success = False
      End Try

      Return success

    End Function

    ''' <summary>
    ''' Opens an url in the default browser.
    ''' </summary>
    ''' <param name="url">The url to open.</param>
    Public Sub OpenURL(url As String)
      Try
        System.Diagnostics.Process.Start(url)
      Catch
        ' Do nothing.
      End Try

    End Sub

		Public Function OpenSMSMessage(ByVal number As String, ByVal iManNr As Integer,
								   ByVal iCustomerNumber As Integer,
								   ByVal responiblePersonNumber As Integer,
								   ByVal hwnNr As Integer,
								   ByVal moduleNumber As Integer,
								   ByVal recid As Integer) As Integer

			Dim oMyProg As Object
			Dim strTranslationProgName As String = String.Empty


			Try
				If responiblePersonNumber > 0 Then
					oMyProg = CreateObject("SPSModulsView.ClsMain")
					oMyProg.TranslateProg4Net("SPSCommUtil.ClsMain", "zhd", number, iCustomerNumber, responiblePersonNumber)

				ElseIf responiblePersonNumber > 0 Then
					oMyProg = CreateObject("SPSModulsView.ClsMain")
					oMyProg.TranslateProg4Net("SPSCommUtil.ClsMain", "MA", number, iManNr)

				End If


			Catch e As Exception

			End Try

			Return 0

		End Function

	End Class


End Namespace

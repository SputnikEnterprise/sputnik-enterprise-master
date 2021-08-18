
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel

Public Class frmMDLogin
  Inherits XtraForm

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsSystem As New ClsMain_Net
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private _ClsSetting As New ClsMDData



#Region "Constructor"

	Public Sub New(ByVal _setting As ClsMDData)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		'MyBase.New()
		'Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()

		Me._ClsSetting = _setting


	End Sub

#End Region


  Private Sub frmMDLogin_Load(ByVal eventSender As System.Object, _
                              ByVal eventArgs As System.EventArgs) Handles MyBase.Load

    Me.Top = frmSelectMD.Top + frmSelectMD.Height + 10
    Me.Left = frmSelectMD.Left

    Try
      Me.Text = GetSafeTranslationValue(Me.Text)
      Me.lblHeader1.Text = GetSafeTranslationValue(Me.lblHeader1.Text)
      Me.lblHeader2.Text = GetSafeTranslationValue(Me.lblHeader2.Text)
      Me.lblPassword.Text = GetSafeTranslationValue(Me.lblPassword.Text)
      Me.CmdXPOK.Text = GetSafeTranslationValue(Me.CmdXPOK.Text)
      Me.CmdXPCancel.Text = GetSafeTranslationValue(Me.CmdXPCancel.Text)

    Catch ex As Exception

    End Try

    Try
      Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
      Dim strStyleName As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, String.Empty)
      If strStyleName <> String.Empty Then
        UserLookAndFeel.Default.SetSkinStyle(strStyleName)
      End If

    Catch ex As Exception

    End Try

  End Sub

  Private Sub frmMDLogin_KeyPress(ByVal eventSender As System.Object, _
                                  ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    If KeyAscii = System.Windows.Forms.Keys.Escape Then
      KeyAscii = 0

      CmdXPCancel_Click(CmdXPCancel, New System.EventArgs())
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then eventArgs.Handled = True

  End Sub

	'Private Sub frmMDLogin_FormClosed(ByVal eventSender As System.Object, _
	'                                  ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

	'  frmSelectMD.LvgMDData.Enabled = True

	'End Sub

	Private Sub txtPassword_KeyPress(ByVal sender As Object, _
                                   ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
    Dim KeyAscii As Short = Asc(e.KeyChar)

    If KeyAscii = System.Windows.Forms.Keys.Escape Then
      KeyAscii = 0
      CmdXPCancel_Click(Me.CmdXPCancel, New System.EventArgs())

    ElseIf KeyAscii = System.Windows.Forms.Keys.Return Then
      KeyAscii = 0
      CmdXPOK_Click(CmdXPOK, New System.EventArgs())

    End If

  End Sub

  Private Sub CmdXPOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdXPOK.Click
    Dim strEnteredName As String = ""
    Dim strEnteredPass As String = ""

    If Me.txtPassword.Text = "" Then Me.Close() : Exit Sub
    strEnteredPass = EncryptMyString(Me.txtPassword.Text & strExtraPass, strEncryptionKey)
    If strEnteredPass <> _ClsData.MDPw4 Then
      MsgBox(GetSafeTranslationValue("Geben Sie das Kennwort erneut ein." & _
             "Beachten Sie die Gross-/Kleinschreibung."), _
             MsgBoxStyle.Critical, GetSafeTranslationValue("Falsches Kennwort"))
      Me.txtPassword.Text = ""
      Me.txtPassword.Focus()

    Else
      Dim frmlogin As New frmLogin(Me._ClsSetting)
      frmLogin.Show()
      'frmLogin.txtUserName.Focus()

      Me.Close()
			'frmSelectMD.LvgMDData.Enabled = False

		End If

  End Sub

  Private Sub CmdXPCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdXPCancel.Click
    Me.Close()
  End Sub





End Class
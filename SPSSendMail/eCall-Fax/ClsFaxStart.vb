


<Obsolete("This class is deprecated.")> _
Public Class ClsFaxStart


  'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Dim _FaxSetting As New ClsFaxSetting

	'Function SendFax2eCall() As String
	'  Dim strValue As String = "Success"
	'  Dim _set As New Fax2eCall.ClsSendFaxOverSmtp(Me._FaxSetting)

	'  strValue = _set.SendFaxWithSMTP()
	'  Return strValue
	'End Function

  Function GeteCallState() As String
    Dim strValue As String = "Success"
    Dim _set As New Fax2eCall.ClseCallState(Me._FaxSetting)

    strValue = _set.GeteCallNotification(Me._FaxSetting.Number2Send)
    Return strValue
  End Function

  Public Sub New(ByVal _Setting As ClsFaxSetting)

    Me._FaxSetting = _Setting

  End Sub

End Class

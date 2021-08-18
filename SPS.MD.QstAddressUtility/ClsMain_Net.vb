

Public Class ClsMain_Net
  Public Shared frmTest As frmQSTAddress


	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name


#Region "Startfunktionen..."

	Sub ShowfrmQstAddress()
    Dim _clsEventlog As New SPProgUtility.ClsEventLog
    _clsEventlog.WriteMainLog(strMethodeName)

    frmTest = New frmQSTAddress(ClsDataDetail.ProgSettingData)
    frmTest.Show()

  End Sub


  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

  Public Sub New(ByVal _setting As ClsSetting)

    ClsDataDetail.ProgSettingData = _setting
    If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
      ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
      ClsDataDetail.TranslationData = ClsDataDetail.Translation
    Else
      ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems

    End If

    Application.EnableVisualStyles()

  End Sub

#End Region

End Class

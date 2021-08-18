
Imports SPProgUtility.Mandanten
Imports SPProgUtility


Public Class ClsMain_Net

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

  Public Sub New(ByVal _setting As ClsSetting)

    ClsDataDetail.ProgSettingData = _setting

    ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
    ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, 0)
    If _setting.SelectedMDNr = 0 Then ClsDataDetail.ProgSettingData.SelectedMDNr = ClsDataDetail.MDData.MDNr
    ClsDataDetail.GetSelectedMDConnstring = ClsDataDetail.MDData.MDDbConn

    If _setting.LogedUSNr = 0 Then ClsDataDetail.ProgSettingData.LogedUSNr = ClsDataDetail.UserData.UserNr

    If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
      ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
      ClsDataDetail.TranslationData = ClsDataDetail.Translation
    Else
      ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
    End If

    Application.EnableVisualStyles()

  End Sub

  Sub ShowfrmCallHistory()

    Dim frmTest As frmCallHistory
    frmTest = New frmCallHistory
    frmTest.Show()

  End Sub

End Class

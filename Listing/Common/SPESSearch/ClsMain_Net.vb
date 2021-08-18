
Imports System.IO.File

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
		ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData)

		Application.EnableVisualStyles()

  End Sub

  Sub ShowfrmESSearch()
    Dim _clsEventlog As New SPProgUtility.ClsEventLog
    _clsEventlog.WriteMainLog("ShowfrmESSearch")

    Dim frmTest As frmESSearch
    frmTest = New frmESSearch
    frmTest.Show()
    'test.Show()

  End Sub

End Class


Imports System.IO.File
Imports SPProgUtility.Mandanten


Public Class ClsMain_Net
  Public Shared frmTest As frmKDSearch

#Region "Startfunktionen..."

  Sub ShowfrmKDSearch()
    Dim _ClsLog As New SPProgUtility.ClsEventLog

    frmTest = New frmKDSearch
    frmTest.Show()

  End Sub


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

#End Region

End Class

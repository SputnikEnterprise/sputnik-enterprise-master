
'Imports System.IO.File

'Public Class ClsMain_Net

'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _clsEventlog As New SPProgUtility.ClsEventLog



'#Region "Startfunktionen..."

'  Sub ShowfrmZESearch()
'    Dim frmTest As frmZESearch
'    _clsEventlog.WriteMainLog("ShowfrmZESearch")

'    frmTest = New frmZESearch
'    frmTest.Show()

'  End Sub

'  Protected Overrides Sub Finalize()
'    MyBase.Finalize()
'  End Sub

'  Public Sub New(ByVal _setting As ClsSetting)

'    ClsDataDetail.ProgSettingData = _setting

'		ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(ClsDataDetail.m_InitialData.MDData.MDNr)
'		ClsDataDetail.UserData = ClsDataDetail.LogededUSData(ClsDataDetail.m_InitialData.MDData.MDNr, ClsDataDetail.ProgSettingData.LogedUSNr)

'		If _setting.SelectedMDNr = 0 Then ClsDataDetail.m_InitialData.MDData.MDNr = ClsDataDetail.m_InitialData.MDData.MDNr
'		ClsDataDetail.m_InitialData.MDData.MDDbConn = ClsDataDetail.MDData.MDDbConn

'    If _setting.LogedUSNr = 0 Then ClsDataDetail.ProgSettingData.LogedUSNr = ClsDataDetail.m_InitialData.UserData.UserNr

'    If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
'      ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
'      ClsDataDetail.TranslationData = ClsDataDetail.Translation
'    Else
'      ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
'    End If

'    Application.EnableVisualStyles()

'  End Sub

'#End Region

'End Class

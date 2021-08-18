''Naas added on 27.08.2018 
'Imports System.IO.File
'Imports SPProgUtility.ClsProgSettingPath
'Imports SPS.Listing.Print.Utility.ClsMainSetting
'Imports SPS.Listing.Print.Utility.MainUtilities.Utilities



'Namespace PrintWeeklyMonthlyReportList

'  Public Class ClsPrintWeeklyMonthlyReportList

'    Private _ClsSetting As New ClsLLWeeklyMonthlyReportPrintSetting

'    Public Property PrintData As ClsLLWeeklyMonthlyReportPrintSetting

'    ''' <summary>
'    ''' Funktion for the Print of the Weekly and Monthly Reportlist
'    ''' </summary>
'    Function PrintWeeklyMonthlyReportList(ByVal PrintYear As Integer, ByVal FirstWeek As Integer, ByVal LastWeek As Integer) As String
'      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'      Dim strResult As String = String.Empty

'			Dim _clsRPPrint As New ClsLLWeeklyMonthlyReportPrint(m_InitialData, _ClsSetting, PrintYear, FirstWeek, LastWeek)
'			If _ClsSetting.bAsDesign Then
'        _clsRPPrint.ShowInDesign()
'      Else
'        strResult = _clsRPPrint.ShowInPrint(True)
'      End If

'      _clsRPPrint.Dispose()

'      Return strResult

'    End Function

'    Public Sub New(ByVal _Setting As ClsLLWeeklyMonthlyReportPrintSetting)

'      _ClsSetting = _Setting
'      PrintData = _Setting

'      Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
'      m_InitialData = m_init
'      m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

'      ClsMainSetting.MDData = m_init.MDData
'      ClsMainSetting.UserData = m_init.UserData
'      ClsMainSetting.TranslationData = m_InitialData.TranslationData
'      ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

'      ClsMainSetting.ProgSettingData = New ClsSetting
'      Me._ClsSetting.USSignFileName = GetUSSign(Me._ClsSetting.DbConnString2Open)

'    End Sub

'    Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

'      m_InitialData = _setting
'      m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

'      ClsMainSetting.MDData = m_InitialData.MDData
'      ClsMainSetting.UserData = m_InitialData.UserData
'      ClsMainSetting.TranslationData = m_InitialData.TranslationData
'      ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

'      ClsMainSetting.ProgSettingData = New ClsSetting
'      Me._ClsSetting.USSignFileName = GetUSSign(Me._ClsSetting.DbConnString2Open)

'    End Sub

'    Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

'      Dim m_md As New SPProgUtility.Mandanten.Mandant
'      Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
'      Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
'      Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

'      Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
'      Dim translate = clsTransalation.GetTranslationInObject

'      Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

'    End Function

'  End Class

'End Namespace

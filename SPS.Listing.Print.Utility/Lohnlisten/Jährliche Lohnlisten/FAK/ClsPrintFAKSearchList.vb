
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities



Namespace FAKSearchListing

  Public Class ClsPrintFAKSearchList

		Private _ClsSetting As New ClsLLFAKSearchPrintSetting

		Public Property PrintData As ClsLLFAKSearchPrintSetting


    Function PrintFAKSearchList(ByVal bAsDesign As Boolean, ByVal strSortbez As String, ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

      Dim _clsFAKSearchSetting As ClsLLFAKSearchPrintSetting = _ClsSetting
      _clsFAKSearchSetting.ListSortBez = strSortbez
      _clsFAKSearchSetting.ListFilterBez = _Filterbez

      Dim _clsFAKPrint As New ClsLLFAKSearchPrint(_clsFAKSearchSetting)
      If bAsDesign Then
        _clsFAKPrint.ShowInDesign()
      Else
        strResult = _clsFAKPrint.ShowInPrint(True)
      End If
			_clsFAKPrint.Dispose()

      Return strResult
    End Function

    Public Sub New(ByVal _Setting As ClsLLFAKSearchPrintSetting)

			_ClsSetting = _Setting
			PrintData = _Setting

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			'ClsMainSetting.MDData = ClsMainSetting.SelectedMDData(_Setting.SelectedMDNr)
			'ClsMainSetting.UserData = ClsMainSetting.LogededUSData(_Setting.SelectedMDNr, _Setting.LogedUSNr)

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)

    End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)

		End Sub

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

  End Class

End Namespace

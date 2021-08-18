
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace KDSearchListing

  Public Class ClsPrintKDSearchList

		Private _ClsSetting As New ClsLLKDSearchPrintSetting
		Public Property PrintData As ClsLLKDSearchPrintSetting


    Function PrintVakStammBlatt(ByVal strSortbez As String, ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

			_ClsSetting.ListSortBez = strSortbez
      _ClsSetting.ListFilterBez = _Filterbez
      Dim _clsVakPrint As New ClsLLKDSearchPrint(_ClsSetting)
      _clsVakPrint.ShowInDesign()

			_clsVakPrint.Dispose()

      Return strResult
    End Function

    Function PrintKDSearchList_1(ByVal bAsDesign As Boolean, ByVal strSortbez As String, ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

      _ClsSetting.ListSortBez = strSortbez
      _ClsSetting.ListFilterBez = _Filterbez
      Dim _clsVakPrint As New ClsLLKDSearchPrint(_ClsSetting)
      If bAsDesign Then
        _clsVakPrint.ShowInDesign()
      Else
        strResult = _clsVakPrint.ShowInPrint(True)
      End If
      _clsVakPrint.Dispose()

      Return strResult
    End Function

    Public Sub New(ByVal _Setting As ClsLLKDSearchPrintSetting)

      _ClsSetting = _Setting
			PrintData = _Setting

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_init.MDData
			ClsMainSetting.UserData = m_init.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			'ClsMainSetting.MDData = ClsMainSetting.SelectedMDData(_ClsSetting.SelectedMDNr)
			'   ClsMainSetting.UserData = ClsMainSetting.LogededUSData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)

			'   ClsMainSetting.TranslationData = _ClsSetting.TranslationData
			'   ClsMainSetting.PerosonalizedData = _ClsSetting.PerosonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(Me._ClsSetting.DbConnString2Open)
    End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = GetUSSign(Me._ClsSetting.DbConnString2Open)

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

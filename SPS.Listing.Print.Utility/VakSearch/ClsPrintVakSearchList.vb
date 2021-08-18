
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace VakSearchListing

  Public Class ClsPrintVakSearchList


    Private _ClsSetting As New ClsLLVakSearchPrintSetting

		Public Property PrintData As ClsLLVakSearchPrintSetting


    Function PrintVakStammBlatt(ByVal strSortbez As String, ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

			_ClsSetting.ListSortBez = strSortbez
      _ClsSetting.ListFilterBez = _Filterbez
      _ClsSetting.IsJobAsListing = False
      Dim _clsVakPrint As New ClsLLVakSearchPrint(_ClsSetting)

      _clsVakPrint.ShowInDesign()

			_clsVakPrint.Dispose()

      Return strResult
    End Function

    Function PrintVakTpl_1(ByVal bAsDesign As Boolean, ByVal ivakNr As Integer) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

			_ClsSetting.VakNr2Print = ivakNr
      _ClsSetting.IsJobAsListing = False
      Dim _clsvakPrint As New ClsLLVakSearchPrint(_ClsSetting)
      If bAsDesign Then
        _clsvakPrint.ShowInDesign()
      Else
        strResult = _clsvakPrint.ShowInPrint(True)
      End If

			_clsvakPrint.Dispose()

      Return strResult
    End Function

    Function PrintVakSearchList_1(ByVal bAsDesign As Boolean, ByVal strSortbez As String, ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

			_ClsSetting.ListSortBez = strSortbez
      _ClsSetting.ListFilterBez = _Filterbez
      _ClsSetting.IsJobAsListing = True
      Dim _clsvakPrint As New ClsLLVakSearchPrint(_ClsSetting)
      If bAsDesign Then
        _clsvakPrint.ShowInDesign()
      Else
        strResult = _clsvakPrint.ShowInPrint(True)
      End If

			_clsvakPrint.Dispose()


      Return strResult
    End Function

    Public Sub New(ByVal _Setting As ClsLLVakSearchPrintSetting)

			_ClsSetting = _Setting
			PrintData = _Setting

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			If _ClsSetting.DbConnString2Open = String.Empty Then _ClsSetting.DbConnString2Open = m_InitialData.MDData.MDDbConn
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

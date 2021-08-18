
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace ProposeSearchListing

  Public Class ClsPrintProposeSearchList

		Private _ClsSetting As New ClsLLProposeSearchPrintSetting

		Public Property PrintData As ClsLLProposeSearchPrintSetting


    Function PrintProposeTpl_1(ByVal _bAsDesign As Boolean, ByVal _strSortbez As String, _
                             ByVal _iPNr As Integer, ByVal _bIsAsListing As Boolean, ByVal _bExportDoc As Boolean) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

      Me._ClsSetting.ListSortBez = _strSortbez
      Me._ClsSetting.IsJobAsListing = _bIsAsListing
      Me._ClsSetting.ProposeNr2Print = _iPNr

      Dim _clsProposePrint As New ClsLLProposeSearchPrint(Me._ClsSetting)
      If _bAsDesign Then
        _clsProposePrint.ShowInDesign()
      Else
        If _bExportDoc Then
          strResult = _clsProposePrint.ExportLLDoc()

        Else
          strResult = _clsProposePrint.ShowInPrint(True)

        End If

      End If

      _clsProposePrint.Dispose()


      Return strResult
    End Function

    Function PrintProposeSearchList_1(ByVal bAsDesign As Boolean, ByVal strSortbez As String, _
                                 ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

      Me._ClsSetting.ListFilterBez = _Filterbez
      Me._ClsSetting.IsJobAsListing = True
      Me._ClsSetting.ListSortBez = strSortbez
      Dim _clsProposePrint As New ClsLLProposeSearchPrint(Me._ClsSetting)
      If bAsDesign Then
        _clsProposePrint.ShowInDesign()
      Else
        strResult = _clsProposePrint.ShowInPrint(True)
      End If

      _clsProposePrint.Dispose()

      Return strResult
    End Function


    Public Sub New(ByVal _Setting As ClsLLProposeSearchPrintSetting)

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

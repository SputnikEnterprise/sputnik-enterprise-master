
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace OPSearchListing

  Public Class ClsPrintOPSearchList

		Private _ClsSetting As New ClsLLOPSearchPrintSetting

		Public Property PrintData As ClsLLOPSearchPrintSetting


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLOPSearchPrintSetting)

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

#End Region

    Function PrintOPStammBlatt(ByVal strSortbez As String, ByVal _Filterbez As List(Of String)) As String
			Return "not exist..."
    End Function

		Function PrintOPSearchList_1() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			Dim _clsOPPrint As New ClsLLOPSearchPrint(Me._ClsSetting)
			If Me._ClsSetting.ShowAsDesign Then
				_clsOPPrint.ShowInDesign()
			Else
				strResult = _clsOPPrint.ShowInPrint(True)
			End If

			_clsOPPrint.Dispose()

			Return strResult
		End Function


#Region "Drucken von diverse Exportlisten innerhalb der Debitorenliste..."

    Function PrintOPExportList_Comatic(ByVal bAsDesign As Boolean, ByVal strSortbez As String, _
                             ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

      _ClsSetting.ListSortBez = strSortbez
      _ClsSetting.ListFilterBez = _Filterbez
      Dim _clsOPPrint As New ClsLLOPSearchPrint(_ClsSetting)
      If bAsDesign Then
        _clsOPPrint.ShowInDesign()
      Else
        strResult = _clsOPPrint.ShowInPrint(True)
      End If

			_clsOPPrint.Dispose()


      Return strResult
    End Function

    Function PrintOPExportList_Factoring(ByVal bAsDesign As Boolean, ByVal strSortbez As String, _
                         ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

			_ClsSetting.ListSortBez = strSortbez
      _ClsSetting.ListFilterBez = _Filterbez
      Dim _clsOPPrint As New ClsLLOPSearchPrint(_ClsSetting)
      If bAsDesign Then
        _clsOPPrint.ShowInDesign()
      Else
        strResult = _clsOPPrint.ShowInPrint(True)
      End If

			_clsOPPrint.Dispose()

      Return strResult
    End Function

    Function PrintOPExportList_Creditreform(ByVal bAsDesign As Boolean, ByVal strSortbez As String, _
                         ByVal _Filterbez As List(Of String)) As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

			_ClsSetting.ListSortBez = strSortbez
      _ClsSetting.ListFilterBez = _Filterbez
      Dim _clsOPPrint As New ClsLLOPSearchPrint(_ClsSetting)
      If bAsDesign Then
        _clsOPPrint.ShowInDesign()
      Else
        strResult = _clsOPPrint.ShowInPrint(True)
      End If

			_clsOPPrint.Dispose()

      Return strResult
    End Function

#End Region



  End Class



End Namespace

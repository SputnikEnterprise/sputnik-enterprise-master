
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SP.Infrastructure.Logging
Imports SPS.Listing.Print.Utility.ClsMainSetting


Namespace PVLSearchListing

  Public Class ClsPrintPVLSearchList


		Private _ClsSetting As New ClsLLPVLSearchPrintSetting

		Public Property PrintData As ClsLLPVLSearchPrintSetting


		Function PrintPVLSearchList() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			Dim _clsPVLPrint As New ClsLLPVLSearchPrint(_ClsSetting)
			If _ClsSetting.ShowAsDesign Then
				_clsPVLPrint.ShowInDesign()
			Else
				strResult = _clsPVLPrint.ShowInPrint(True)
			End If
			_clsPVLPrint.dispose()

			Return strResult
		End Function

    Public Sub New(ByVal _Setting As ClsLLPVLSearchPrintSetting)

			_ClsSetting = _Setting
			PrintData = _Setting

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(m_InitialData.MDData.MDDbConn)

		End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(m_InitialData.MDData.MDDbConn)

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


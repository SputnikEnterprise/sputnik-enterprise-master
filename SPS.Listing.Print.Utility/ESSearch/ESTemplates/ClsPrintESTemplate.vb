
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting


Namespace ESTemplate

  Public Class ClsPrintESTemplate

		Private _ClsSetting As New ClsLLESTemplateSetting


#Region "constructor"

		Public Sub New(ByVal _Setting As ClsLLESTemplateSetting)

			_ClsSetting = _Setting
			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting

			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(Me._ClsSetting.DbConnString2Open)
		End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _Set As ClsLLESTemplateSetting)

			_ClsSetting = _Set
			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(Me._ClsSetting.DbConnString2Open)

		End Sub

#End Region


		Function PrintESTemplate() As String
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim strResult As String = String.Empty

      Dim _clsESPrint As New ClsLLESTemplatePrint(_ClsSetting)
      If _ClsSetting.bAsDesign Then
        _clsESPrint.ShowInDesign()
      Else
        strResult = _clsESPrint.ShowInPrint(True)
      End If

      _clsESPrint.Dispose()
      Return strResult
    End Function


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

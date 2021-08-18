
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports DevExpress.XtraSplashScreen
Imports SPS.Listing.Print.Utility.ClsMainSetting


Namespace BLJSearchListing

	Public Class ClsPrintBLJSearchList

		Private _ClsSetting As New ClsLLBLJSearchPrintSetting


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLBLJSearchPrintSetting)

			_ClsSetting = _Setting
			ClsMainSetting.ProgSettingData = New ClsSetting
			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)

			ClsMainSetting.MDData = m_init.MDData
			ClsMainSetting.UserData = m_init.UserData

			ClsMainSetting.TranslationData = m_init.TranslationData	' _ClsSetting.TranslationData
			ClsMainSetting.PerosonalizedData = m_init.ProsonalizedData '  _ClsSetting.PerosonalizedData
			If _ClsSetting.DbConnString2Open = String.Empty Then _ClsSetting.DbConnString2Open = ClsMainSetting.MDData.MDDbConn
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsMainSetting.TranslationData, ClsMainSetting.PerosonalizedData)


			'ClsMainSetting.ProgSettingData = New ClsSetting
			'ClsMainSetting.MDData = ClsMainSetting.SelectedMDData(_ClsSetting.SelectedMDNr)
			'ClsMainSetting.UserData = ClsMainSetting.LogededUSData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)

			'ClsMainSetting.TranslationData = _ClsSetting.TranslationData
			'ClsMainSetting.PerosonalizedData = _ClsSetting.PerosonalizedData
			'If _ClsSetting.DbConnString2Open = String.Empty Then _ClsSetting.DbConnString2Open = ClsMainSetting.MDData.MDDbConn


			SplashScreenManager.ShowForm(GetType(WaitForm1), True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(ClsMainSetting.MDData.MDDbConn)

		End Sub

#End Region


		Public Function PrintBLJListing() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			Dim _clsMAPrint As New ClsLLBLJSearchPrint(_ClsSetting)
			If _ClsSetting.ShowAsDesign Then _clsMAPrint.ShowInDesign() Else _clsMAPrint.ShowInPrint(True)
			_clsMAPrint.Dispose()

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




Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace UVGSearchListing

  Public Class ClsPrintUVGSearchList

		Private _ClsSetting As New ClsLLUVGSearchPrintSetting

		Public Property PrintData As ClsLLUVGSearchPrintSetting

		Function PrintUVGSearchList() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty


			If _ClsSetting.JobNr2Print = "All" Then _ClsSetting.JobNr2Print = "9.7"
			Dim _clsuvgPrint As New ClsLLUVGSearchPrint(_ClsSetting)
			If _ClsSetting.ShowAsDesign Then
				_clsuvgPrint.ShowInDesign()
			Else
				strResult = _clsuvgPrint.ShowInPrint(True)
				_clsuvgPrint.Dispose()
				If Not strResult.ToLower.Contains("error") AndAlso _ClsSetting.PrintUVGWithSuvaRekap Then
					_ClsSetting.JobNr2Print = "9.5"
					_ClsSetting.SQL2Open = _ClsSetting.SQL2Open.Replace("SELECT ", "SELECT TOP 1 ")

					strResult = PrintUVGRekapSearchList()
				End If
			End If
			_clsuvgPrint.Dispose()


			Return strResult
		End Function

		Function PrintUVGRekapSearchList() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			Dim _clsLOANRekapPrint As New ClsLLUVGSearchPrint(_ClsSetting)
			If _ClsSetting.ShowAsDesign Then
				_clsLOANRekapPrint.ShowInDesign()
			Else
				strResult = _clsLOANRekapPrint.ShowInPrint(True)
			End If
			_clsLOANRekapPrint.Dispose()


			Return strResult
		End Function

		Public Sub New(ByVal _Setting As ClsLLUVGSearchPrintSetting)

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


Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace ZGSearchListing

  Public Class ClsPrintZGSearchList

		Private _ClsSetting As New ClsLLZGSearchPrintSetting
		Public Property PrintData As ClsLLZGSearchPrintSetting


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLZGSearchPrintSetting)

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


		Function PrintZGSearchList_1(ByVal bAsDesign As Boolean, ByVal strSortbez As String, ByVal _bShowBetragAsPositiv As Boolean, _
																 ByVal _Filterbez As List(Of String)) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			_ClsSetting.ListSortBez = strSortbez
			_ClsSetting.ListFilterBez = _Filterbez
			_ClsSetting.ShowBetragAsPositiv = _bShowBetragAsPositiv
			Dim _clsZGPrint As New ClsLLZGSearchPrint(_ClsSetting)
			If bAsDesign Then
				_clsZGPrint.ShowInDesign()
			Else
				strResult = _clsZGPrint.ShowInPrint(True)
			End If

			_clsZGPrint.Dispose()


			Return strResult

		End Function


#Region "DTA-Listen"

		Function PrintDTAList(ByVal _setting As ClsLLDTAPrintSetting) As Boolean
			Dim success As Boolean = True

			Try

				Dim _clsZGPrint As New ClsLLDTAPrint(_setting)
				If _setting.ShowAsDesgin Then
					_clsZGPrint.ShowInDesign()
				Else
					_clsZGPrint.ShowInPrint(True)
				End If

				_clsZGPrint.Dispose()

			Catch ex As Exception
				Return False

			End Try


			Return success

		End Function

#End Region


  End Class


End Namespace

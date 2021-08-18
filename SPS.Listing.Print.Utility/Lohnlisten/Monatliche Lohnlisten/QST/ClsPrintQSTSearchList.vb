

Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace QSTSearchListing

  Public Class ClsPrintQSTSearchList

		Private _ClsSetting As New ClsLLQSTSearchPrintSetting

		Public Property PrintData As ClsLLQSTSearchPrintSetting


#Region "Constructor"

		'Public Sub New(ByVal _Setting As ClsLLQSTSearchPrintSetting)

		'	_ClsSetting = _Setting
		'	PrintData = _Setting

		'	Dim m_init = CreateInitialData(PrintData.SelectedMDNr, PrintData.LogedUSNr)
		'	m_InitialData = m_init
		'	m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

		'	ClsMainSetting.MDData = m_InitialData.MDData
		'	ClsMainSetting.UserData = m_InitialData.UserData
		'	ClsMainSetting.TranslationData = m_InitialData.TranslationData
		'	ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

		'	If PrintData.DbConnString2Open = String.Empty Then PrintData.DbConnString2Open = m_InitialData.MDData.MDDbConn
		'	ClsMainSetting.ProgSettingData = New ClsSetting
		'	Me.PrintData.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)

		'End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting

		End Sub


#End Region


		Public Function PrintQSTSearchList() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			PrintData.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)
			Dim _clsPrint As New ClsLLQSTSearchPrint(m_InitialData, PrintData)
			If PrintData.ShowAsDesgin Then
				_clsPrint.ShowInDesign()
			Else
				strResult = _clsPrint.ShowInPrint(PrintData.ShowPrintBox)
			End If
			_clsPrint.Dispose()

			Return strResult
		End Function




#Region "Helpers"

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


	End Class

End Namespace


Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace AdvancePaymentData

	Public Class ClsPrintAdvancePaymentData


		Private _ClsSetting As New ClsLLAdvancePaymentPrintSetting

		Public Property PrintData As ClsLLAdvancePaymentPrintSetting


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLAdvancePaymentPrintSetting)

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


#End Region


		Function PrintAdvancePaymentDocument() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			'0: Check
			'1: Quittung
			'2: Überweisung
			If _ClsSetting.docart = 0 Then
				_ClsSetting.JobNr2Print = "5.3"

			ElseIf _ClsSetting.docart = 1 Then
				_ClsSetting.JobNr2Print = "5.2"

			Else
				_ClsSetting.JobNr2Print = "5.4"

			End If

			Dim _clsLohnKontiSearchSetting As ClsLLAdvancePaymentPrintSetting = _ClsSetting

			Dim _clsPrint As New ClsLLAdvancePaymentPrint(_ClsSetting)
			If _clsLohnKontiSearchSetting.ShowAsDesign Then
				_clsPrint.ShowInDesign()
			Else
				strResult = _clsPrint.ShowInPrint(True)
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



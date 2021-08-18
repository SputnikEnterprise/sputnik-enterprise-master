
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath
Imports SP.Infrastructure.Logging
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities


Namespace ESSearchListing

  Public Class ClsPrintESSearchList

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()
		Private _ClsSetting As New ClsLLESSearchPrintSetting

		Public Property PrintData As ClsLLESSearchPrintSetting


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLESSearchPrintSetting)

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


#End Region
		Function PrintESSearchList_1(ByVal bAsDesign As Boolean, ByVal strSortbez As String, _
																 ByVal _Filterbez As List(Of String), _
																 ByVal _bShowStatistik As Boolean, _
																 ByVal _strTblName As String, _
																 ByVal _strTblKSTStatistikName As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			Dim _clsESPrint As New ClsLLESSearchPrint(PrintData)
			If PrintData.bAsDesign Then
				_clsESPrint.ShowInDesign()
			Else
				strResult = _clsESPrint.ShowInPrint(True)
			End If

			_clsESPrint.Dispose()


			Return strResult
		End Function

		Function PrintESGAVStatistiken() As PrintResult

			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			m_Logger.LogInfo("Wurde gestartet")

			Dim _clsESPrint As New ClsLLESSearchPrint(PrintData)
			If PrintData.bAsDesign Then
				_clsESPrint.LoadDesingerForGAVStatistiken()
			Else
				result = _clsESPrint.PrintGAVStatistiken()
			End If

			_clsESPrint.Dispose()


			Return result

		End Function

		Function PrintESEmployeesATStatistiken() As PrintResult

			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			m_Logger.LogInfo("Wurde gestartet")

			Dim _clsESPrint As New ClsLLESSearchPrint(PrintData)
			If PrintData.bAsDesign Then
				_clsESPrint.LoadDesingerForEmployeesATStatistiken()
			Else
				result = _clsESPrint.PrintEmployeesATStatistiken()
			End If

			_clsESPrint.Dispose()


			Return result

		End Function

		Function PrintESTemplate() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))

			Dim _clsESSearchSetting As New ClsLLESSearchPrintSetting With {.JobNr2Print = _ClsSetting.JobNr2Print,
																																		 .SQL2Open = _ClsSetting.SQL2Open,
																																		 .DbConnString2Open = _ClsSetting.DbConnString2Open, _
																																		 .ListSortBez = String.Empty, _
																																		 .ESNr2Print = _ClsSetting.ESNr2Print}
			Dim _clsESPrint As New ClsLLESSearchPrint(_clsESSearchSetting)
			If _ClsSetting.bAsDesign Then
				_clsESPrint.ShowInDesign()
			Else
				strResult = _clsESPrint.ShowInPrint(True)
			End If

			_clsESPrint.Dispose()
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


Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath


Namespace OfferSearchListing


	Public Class ClsPrintOfferSearchList

		Private _ClsSetting As New ClsLLOfferSearchPrintSetting


#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLOfferSearchPrintSetting)

			_ClsSetting = _Setting

			ClsMainSetting.ProgSettingData = New ClsSetting
			ClsMainSetting.MDData = _ClsSetting.m_initData.MDData
			ClsMainSetting.UserData = _ClsSetting.m_initData.UserData

			ClsMainSetting.TranslationData = _ClsSetting.m_initData.TranslationData
			ClsMainSetting.PerosonalizedData = _ClsSetting.m_initData.ProsonalizedData


			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(_ClsSetting.m_initData.MDData.MDDbConn)

		End Sub

#End Region


		Function PrintOfferTemplate() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			Dim _clsPrint As New ClsLLOfferSearchPrint(_ClsSetting)
			If _ClsSetting.ShowAsDesgin Then
				_clsPrint.ShowInDesign()

			ElseIf _ClsSetting.ShowAsExport Then
				strResult = _clsPrint.ExportLLDoc

			Else
				strResult = _clsPrint.ShowInPrint(True)

			End If

			_clsPrint.Dispose()


			Return strResult

		End Function

	End Class


End Namespace



Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath


Namespace MFakSearchListing

	Public Class ClsPrintMFakSearchList

		Private _ClsSetting As SP.Infrastructure.Initialization.InitializeClass


		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			_ClsSetting = _setting

		End Sub

		Function PrintMFakListing(ByVal _printSetting As ClsLLMFakSetting) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty


			Dim _clsMAPrint As New ClsLLMFakSearchPrint(_ClsSetting, _printSetting)
			If _printSetting.ShowAsDesign Then _clsMAPrint.ShowInDesign() Else _clsMAPrint.ShowInPrint(True)
			_clsMAPrint.Dispose()

			Return strResult

		End Function


	End Class

End Namespace


Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath



Namespace RPSearchListing

	Public Class ClsPrintRPSearchList

		Private _ClsSetting As SP.Infrastructure.Initialization.InitializeClass


		Function PrintRPSearchList(ByVal _printSetting As ClsLLRPSearchPrintSetting) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty

			_printSetting.USSignFileName = MainUtilities.GetUSSign(_ClsSetting.MDData.MDDbConn, _ClsSetting.UserData.UserNr)
			Dim _clsPrint As New ClsLLRPSearchPrint(_ClsSetting, _printSetting)
			If _printSetting.ShowAsDesign Then
				_clsPrint.ShowInDesign()
			Else
				strResult = _clsPrint.ShowInPrint(True)
			End If

			Return strResult
		End Function


#Region "Drucken von diverse Exportlisten innerhalb der Debitorenliste..."


#End Region

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			_ClsSetting = _setting

		End Sub

	End Class


End Namespace

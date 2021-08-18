
Imports System.Data.SqlClient

Public Class ClsMAUtility



  Private _PControlSetting As New ClsParifondSetting
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


  Function CreateData4Kandidaten() As String
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strResult As String = "Success..."

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = _PControlSetting.DbConnString2Open,
																				   .JobNr2Print = "1.0",
																				  .ShowAsDesign = False,
																				  .frmhwnd = 0,
																				  .liMANr2Print = _PControlSetting.liMANr2Print.Distinct().ToList()}
		Dim obj As New SPS.Listing.Print.Utility.MAStammblatt.ClsPrintMAStammblatt(_Setting)
    strResult = obj.ExportMAStammBlatt()

    Return strResult
  End Function

  Public Sub New(ByVal _MASetting As ClsParifondSetting)
    Me._PControlSetting = _MASetting
  End Sub

End Class


Imports System.Data.SqlClient

Public Class ClsKDUtility


  Private _PControlSetting As New ClsParifondSetting
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


  Function CreateData4Kunden() As String
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strResult As String = "Success..."

    Dim _Setting As New SPS.Listing.Print.Utility.ClsLLKDSearchPrintSetting With {.DbConnString2Open = _PControlSetting.DbConnString2Open, _
                                                                                   .JobNr2Print = "2.0", _
                                                                                  .ShowAsDesign = False, _
                                                                                  .liKDNr2Print = _PControlSetting.liKDNr2Print.Distinct().ToList()}
    Dim obj As New SPS.Listing.Print.Utility.KDStammblatt.ClsPrintKDStammblatt(_Setting)
    strResult = obj.ExportKDStammBlatt()

    Return strResult
  End Function

  Public Sub New(ByVal _KDSetting As ClsParifondSetting)
    Me._PControlSetting = _KDSetting
  End Sub

End Class

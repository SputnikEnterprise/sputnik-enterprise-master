
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Module Utilities

  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


  Function TranslateMyText(ByVal strBez As String) As String
    Return _ClsProgSetting.TranslateText(strBez)
  End Function

End Module


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

  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Dim _clsLog As New SPProgUtility.ClsEventLog


#Region "Funktionen zur Übersetzung..."

  Function TranslateMyText(ByVal strBez As String) As String
    Dim strOrgText As String = strBez
    Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)

    If _ClsProgSetting.GetLogedUSNr = 1 Then
      _clsLog.WriteTempLogFile(String.Format("Progbez: {0}{1}{0} Translatedbez: {0}{2}{0}", _
                                  Chr(34), strBez, strTranslatedText), _
                                _ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
    End If

    Return strTranslatedText
  End Function

  Function TranslateMyText(ByVal strFuncName As String, _
                           ByVal strOrgControlBez As String, _
                           ByVal strBez As String) As String
    Dim strOrgText As String = strBez
    Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)

    If _ClsProgSetting.GetLogedUSNr = 1 Then
      _clsLog.WriteTempLogFile(String.Format("{1}: Progbez: {0}{2}{0} Namedbez: {0}{3}{0}, Translatedbez: {0}{4}{0}", _
                                  Chr(34), strFuncName, strOrgControlBez, strBez, strTranslatedText), _
                                _ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
    End If

    Return strTranslatedText
  End Function

#End Region

End Module

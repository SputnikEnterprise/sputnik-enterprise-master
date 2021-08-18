
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Module FuncOpenProg

  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Sub RunOpenOffForm(ByVal iOffNr As Integer)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "OfferUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iOffNr.ToString)

    Try
      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "OfNr", iOffNr.ToString)

      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("OfferUtility.ClsMain", iOffNr.ToString)

    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenOffForm")

    End Try

  End Sub

End Module

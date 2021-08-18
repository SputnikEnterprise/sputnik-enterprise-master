
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
Imports DevExpress.XtraRichEdit
Imports DevExpress.Utils

Imports System.Globalization

Module Utilities

  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Dim _clsLog As New SPProgUtility.ClsEventLog

  Function GetXMLValueByQuery(ByVal strFilename As String, _
                            ByVal strQuery As String, _
                            ByVal strValuebyNull As String) As String
    Dim bResult As String = String.Empty
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strFilename, strQuery)

    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

  Public Function CalendarWeek(ByVal nWeek As Integer, _
  ByVal nYear As Integer) As Date

    ' Wochentag des 4. Januar des Jahres ermitteln
    Dim dStart As New Date(nYear, 1, 4)
    Dim nDay As Integer = (dStart.DayOfWeek + 6) Mod 7 + 1

    ' Beginn der 1. KW des Jahres
    Dim dFirst As Date = dStart.AddDays(1 - nDay)

    ' Gesuchte KW ermitteln
    Return dFirst.AddDays((nWeek - 1) * 7)
  End Function

  Public Function DateToWeek(ByVal dDate As Date) As Integer
    ' Startdatum der ersten Kalenderwoche des Jahres und Folgejahres berechnen
    Dim dThisYear As Date = CalendarWeek(1, dDate.Year)
    Dim dNextYear As Date = CalendarWeek(1, dDate.Year + 1)

    'Dim cui As New CultureInfo(CultureInfo.CurrentCulture.Name)
    'Dim itest As Integer = (cui.Calendar.GetWeekOfYear(dDate, cui.DateTimeFormat.CalendarWeekRule, cui.DateTimeFormat.FirstDayOfWeek))

    'Dim kalenderwoche As Integer = (dDate.DayOfYear / 7) + 1
    'If kalenderwoche = 53 Then
    '  kalenderwoche = 1
    'End If



    ' Prüfen, ob Datum zur ersten Woche des Folgejahres gehört
    If dDate >= dNextYear Then
      ' Rückgabe: KW 1 des Folgejahres
      'Return dDate.Year + 1 & "01"
      Return 1
    ElseIf dDate < dThisYear Then
      ' Falls das Datum noch zu einer KW aus dem letzten Jahr zählt
      'Return dDate.Year - 1 & DatePart(DateInterval.WeekOfYear, New Date(dDate.Year - 1, 12, 28), _
      '                                 FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
      Return DatePart(DateInterval.WeekOfYear, New Date(dDate.Year - 1, 12, 28), _
                                      FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
    Else
      ' KW = Differenz zum ersten Tag der ersten Woche
      'Return dDate.Year & Format$(dDate.Subtract(dThisYear).Days \ 7 + 1, "00")
      Return CInt(Format$(dDate.Subtract(dThisYear).Days \ 7 + 1, "00"))
    End If
  End Function

End Module


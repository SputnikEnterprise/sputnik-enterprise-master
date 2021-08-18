'------------------------------------
' File: DateUtils.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Date utility class.
''' </summary>
Public Class DateUtils

#Region "Public Shared Functions"

    ''' <summary>
    ''' Date of Monday. 
    ''' example: format(CalendarWeek(53,2004),"d") = 27.12.2004
    ''' </summary>
    ''' <param name="nWeek">The calendar week.</param>
    ''' <param name="nYear">The year.</param>
    ''' <returns>Date of monday of calendar week and year.</returns>
    ''' <remarks></remarks>
    Public Shared Function DateOfMonday(ByVal nWeek As Integer, ByVal nYear As Integer) As Date

        ' Check week.
        If (nWeek < 0 Or nWeek > 53) Then
            Throw New Exception(String.Format("Invalid week ({0}).", nWeek))
        End If

        ' Check year.
        If (nYear < 0) Then
            Throw New Exception(String.Format("Invalid year ({0}).", nYear))
        End If

        ' Determine week day of 4th january of year 
        If nWeek = 0 Then Return CDate("01.01.1900")
        Dim dStart As New Date(nYear, 1, 4)
        Dim nDay As Integer = (dStart.DayOfWeek + 6) Mod 7 + 1

        ' Start of first calendar week of year
        Dim dFirst As Date = dStart.AddDays(1 - nDay)

        ' Find calendar week
        Return dFirst.AddDays((nWeek - 1) * 7)
    End Function

#End Region

End Class

Imports System.Globalization

Namespace DateAndTimeCalculation

  Public Class DateAndTimeUtily

#Region "Public Enums"

    ''' <summary>
    ''' Definiert die Art der Rundung des Dezimalwertes in Stunden.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum HoursRoundKind
      ''' <summary>
      ''' Rundung auf ganze Minuten (1 Minute = 0.01666666~h).
      ''' </summary>
      ''' <remarks></remarks>
      Minutes = 1
      ''' <summary>
      ''' Rundung auf 0.01 Stunden (1 Minute = 0.02h, 2 Minuten = 0.03h, 3 Minuten = 0.05h, ...)
      ''' </summary>
      ''' <remarks></remarks>
      HundredthsOfHour = 2
    End Enum

#End Region

#Region "Public Shared Methods"

    ''' <summary>
    ''' Rundet den Stunden-Dezimalwert <paramref>hours</paramref> in der Rundungsart <paramref>roundKind</paramref>.
    ''' </summary>
    ''' <param name="hours"></param>
    ''' <param name="roundKind"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RoundHourValue(ByVal hours As Decimal, roundKind As HoursRoundKind) As Decimal
      Select Case roundKind
        Case HoursRoundKind.Minutes
          Return Math.Round(hours * 60) / 60
        Case HoursRoundKind.HundredthsOfHour
          Return Math.Round(hours, 2)
        Case Else
          Return hours
      End Select
    End Function

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Gets the minimum of two dates.
    ''' </summary>
    ''' <param name="date1">The first date.</param>
    ''' <param name="date2">The second date.</param>
    ''' <returns>The minimum date.</returns>
    Public Function MinDate(ByVal date1, ByVal date2)
      Dim minDateValue = If(date1 < date2, date1, date2)
      Return minDateValue
    End Function

    ''' <summary>
    ''' Gets the maximum of two dates.
    ''' </summary>
    ''' <param name="date1">The first date.</param>
    ''' <param name="date2">The second date.</param>
    ''' <returns>The maximum date.</returns>
    Public Function MaxDate(ByVal date1, ByVal date2)
      Dim maxDateValue = If(date1 > date2, date1, date2)
      Return maxDateValue
    End Function

    ''' <summary>
    ''' Limits a date value to  a date range.
    ''' </summary>
    ''' <param name="value">The value to check.</param>
    ''' <param name="minDateValue">The min date.</param>
    ''' <param name="maxDateValue">The max date.</param>
    ''' <returns>The date value limited to the date range.</returns>
    Public Function LimitToRange(ByVal value As DateTime, ByVal minDateValue As DateTime?, ByVal maxDateValue As DateTime?)

      Dim min = If(minDateValue.HasValue, minDateValue, DateTime.MinValue)
      Dim max = If(maxDateValue.HasValue, maxDateValue, DateTime.MaxValue)

      Dim limitedValue = MaxDate(MinDate(value, maxDateValue), minDateValue)

      Return limitedValue

    End Function

    ''' <summary>
    ''' Gets the first day of a month.
    ''' </summary>
    ''' <param name="year">The year.</param>
    ''' <param name="month">The month.</param>
    ''' <returns>The first day of the month.</returns>
    Public Function GetFirstDayOfMonth(ByVal year As Integer, ByVal month As Integer) As DateTime
      Dim firstDate As Date = New DateTime(year, month, 1)
      Return firstDate
    End Function

    ''' <summary>
    ''' Gets the last day of a month.
    ''' </summary>
    ''' <param name="year">The year.</param>
    ''' <param name="month">The month.</param>
    ''' <returns>The last day of the month.</returns>
    Public Function GetLastDayOfMonth(ByVal year As Integer, ByVal month As Integer) As DateTime
      Dim firstDate As Date = GetFirstDayOfMonth(year, month)
      Dim lastDate As Date = firstDate.AddMonths(1).AddDays(-1)
      Return lastDate
    End Function

    ''' <summary>
    ''' Gets the frist day of a week.
    ''' </summary>
    ''' <param name="year">The year.</param>
    ''' <param name="weekNumber">The week number.</param>
    ''' <returns>The first day of a week.</returns>
    ''' <remarks>Weeks range from 1 to 53.</remarks>
    Public Function GetFirstDayOfWeek(ByVal year As Integer, ByVal weekNumber As Integer) As DateTime
      Dim januar4 As New DateTime(year, 1, 4)
      Dim weekdayjah4 As Integer = GetDayOfWeekForFirstDayOfWeekCalculation(januar4)
      Dim dateoffirstWeek As DateTime = januar4.AddDays(1 - weekdayjah4)

      Return dateoffirstWeek.AddDays((weekNumber - 1) * 7)
    End Function

    ''' <summary>
    ''' Gets date of sunday in a given week
    ''' </summary>
    ''' <param name="year"></param>
    ''' <param name="month"></param>
    ''' <param name="weekNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLastDayOfWeek(ByVal year As Integer, ByVal month As Integer, ByVal weekNumber As Integer) As DateTime

      Dim lastDayofWeek = GetFirstDayOfWeek(year, weekNumber).AddDays(6)
      Dim DayofWeek As Integer = Math.Min((lastDayofWeek).Day, Date.DaysInMonth(year, month))

      Return CDate(DayofWeek & "." & lastDayofWeek.Month & "." & lastDayofWeek.Year)
    End Function

    Public Function GetMondayAndSundayOfWeek(ByVal year As Integer, ByVal month As Integer, ByVal weeknumber As Integer) As List(Of Date)
      Dim result As New List(Of Date)

      result.Add(GetFirstDayOfWeek(year, weeknumber))
      result.Add(GetLastDayOfWeek(year, month, weeknumber))

      Return result

    End Function

    ''' <summary>
    ''' Gets calendar weeks between dates.
    ''' </summary>
    ''' <param name="fromDate">The from date.</param>
    ''' <param name="toDate">The to date</param>
    ''' <returns>Array list of calendar weeks.</returns>
    Public Function GetCalendarWeeksBetweenDates(ByVal fromDate As DateTime, ByVal toDate As DateTime) As Integer()

      If fromDate > toDate Then
        Return Nothing
      End If
      Dim s = FirstWeekOfYear.System

      Dim iFWeek As Integer = DatePart(DateInterval.WeekOfYear, fromDate, _
                                FirstDayOfWeek.System, FirstWeekOfYear.System)
      Dim iLWeek As Integer = DatePart(DateInterval.WeekOfYear, toDate, _
                                      FirstDayOfWeek.System, FirstWeekOfYear.System)

			' Safety check
			Dim weeks As Integer()
      If iFWeek > 50 And iLWeek = 1 Then
        iLWeek = 53
			ElseIf iFWeek = 52 AndAlso iLWeek < iFWeek AndAlso iLWeek <= 5 Then
				weeks = New Integer() {52, 1, 2, 3, 4, 5}.ToArray
				Return weeks

			ElseIf iFWeek = 53 AndAlso iLWeek < iFWeek Then
				weeks = New Integer() {53, 1, 2, 3, 4}.ToArray
				Return weeks
			End If

			weeks = Enumerable.Range(iFWeek, iLWeek - iFWeek + 1).ToArray()


      Return weeks
    End Function

#End Region

#Region "Private Functions"

    ''' <summary>
    ''' Support function.
    ''' </summary>
    ''' <param name="myDate">The dayte.</param>
    Private Function GetDayOfWeekForFirstDayOfWeekCalculation(ByVal myDate As DateTime) As Integer
      Return (myDate.DayOfWeek + 6) Mod 7 + 1
    End Function

#End Region

  End Class

End Namespace

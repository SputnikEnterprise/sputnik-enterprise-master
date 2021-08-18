Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Report.DataObjects.RPAbsenceDaysData

Public Class TimeTable

#Region "Private Consts"

  Private Const MAX_HOURS As Decimal = 24D
  Private Const MIN_HOURS As Decimal = 0D

#End Region ' Private Consts

#Region "Private Fields"

  Private m_DayHourLookup As Dictionary(Of DateTime, TimeTableDayData)

#End Region ' Private Fields

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New()
    m_DayHourLookup = New Dictionary(Of DateTime, TimeTableDayData)
  End Sub

#End Region ' Constructor

#Region "Public Methods"

  ''' <summary>
  ''' Clears the time table data.
  ''' </summary>
  Public Sub Clear()
    m_DayHourLookup.Clear()
  End Sub

  ''' <summary>
  ''' Adds day hour data.
  ''' </summary>
  ''' <param name="dateValue">The date value.</param>
  ''' <param name="dayHourData">The day hour data.</param>
  ''' <returns>Boolean flag indicating if data could be added.</returns>
  Public Function AddDayHourData(ByVal dateValue As DateTime, ByVal dayHourData As TimeTableDayData) As Boolean

    If Not m_DayHourLookup.ContainsKey(dateValue.Date) Then
      m_DayHourLookup.Add(dateValue.Date, dayHourData)

      Return True
    End If

    Return False
  End Function

  ''' <summary>
  ''' Clears all working hours.
  ''' </summary>
  ''' <remarks>Sets them to nothing.</remarks>
  Public Sub ClearAllWorkingHours()

    For Each entry In m_DayHourLookup.Values
      entry.WorkHours = Nothing
    Next

  End Sub

  ''' <summary>
  ''' Clears all absence codes.
  ''' </summary>
  Public Sub ClearAllAbsenceCodes()

    For Each entry In m_DayHourLookup.Values
      entry.FehlzeitCode = Nothing
    Next

  End Sub

  ''' <summary>
  ''' Sets the working hours of a day.
  ''' </summary>
  ''' <param name="day">The day date.</param>
  ''' <param name="workingHours">The working hours.</param>
  Public Sub SetWorkingHoursOfDay(ByVal day As DateTime, ByVal workingHours As Decimal?)

    Dim timeTableDayData = GetDayDataByDate(day)

    If Not timeTableDayData Is Nothing Then
      timeTableDayData.WorkHours = workingHours
    End If

  End Sub

  ''' <summary>
  ''' Adds working hours of a day.
  ''' </summary>
  ''' <param name="day">The day.</param>
  ''' <param name="workingHoursToAdd">The working hours to add.</param>
  Public Sub AddToWorkingHoursOfDay(ByVal day As DateTime, ByVal workingHoursToAdd As Decimal?)

    Dim timeTableDayData = GetDayDataByDate(day)

    If Not timeTableDayData Is Nothing Then

      If timeTableDayData.WorkHours.HasValue Then

        If workingHoursToAdd.HasValue Then
          timeTableDayData.WorkHours = timeTableDayData.WorkHours + workingHoursToAdd.Value
        End If

      Else
        timeTableDayData.WorkHours = workingHoursToAdd
      End If

    End If

  End Sub

  ''' <summary>
  ''' Sets the working hours of many days.
  ''' </summary>
  ''' <param name="workingHoursOfManyDays">The working hours of many days.</param>
  Public Sub SetWorkinHoursOfManyDays(ByVal workingHoursOfManyDays As DateAndHourData())

    For Each entry In workingHoursOfManyDays
      SetWorkingHoursOfDay(entry.DayDate, entry.WorkingHours)
    Next

  End Sub

  ''' <summary>
  ''' Adds workings hours of many days.
  ''' </summary>
  ''' <param name="workingHoursOfManyDays">The working hours of many days.</param>
  Public Sub AddWorkinHoursOfManyDays(ByVal workingHoursOfManyDays As DateAndHourData())

    For Each entry In workingHoursOfManyDays
      AddToWorkingHoursOfDay(entry.DayDate, entry.WorkingHours)
    Next

  End Sub

  ''' <summary>
  ''' Sets the absence code of a day.
  ''' </summary>
  ''' <param name="day">The day date.</param>
  ''' <param name="absenceCode">The absence code.</param>
  Public Sub SetAbsenceCodeOfDay(ByVal day As DateTime, ByVal absenceCode As String)

    Dim timeTableDayData = GetDayDataByDate(day)

    If Not timeTableDayData Is Nothing Then
      timeTableDayData.FehlzeitCode = absenceCode
    End If

  End Sub

  ''' <summary>
  ''' Sets the absence code of many days.
  ''' </summary>
  ''' <param name="absenceCodesOfManyDays">The absence codes of many days.</param>
  Public Sub SetAbsenceCodeOfManyDays(ByVal absenceCodesOfManyDays As DateAndAbsenceCodeData())

    For Each entry In absenceCodesOfManyDays

      SetAbsenceCodeOfDay(entry.DayDate, entry.AbsenceCode)

    Next

  End Sub

  ''' <summary>
  ''' Gets day hour data by a date value.
  ''' </summary>
  ''' <param name="dateValue">The date value.</param>
  ''' <returns>Day hour data or nothing in error case.</returns>
  Private Function GetDayDataByDate(ByVal dateValue As DateTime) As TimeTableDayData

    If m_DayHourLookup.Keys.Contains(dateValue.Date) Then
      Return m_DayHourLookup(dateValue.Date)
    End If

    Return Nothing
  End Function

  ''' <summary>
  ''' Gets the time table data.
  ''' </summary>
  ''' <returns>List of day hour data sorted by DayDate.</returns>
  Public Function GetTimeTableData() As IEnumerable(Of TimeTableDayData)

    Dim values() As TimeTableDayData
    ReDim values(Math.Max(m_DayHourLookup.Values.Count - 1, 0))

    m_DayHourLookup.Values.CopyTo(values, 0)

    values = values.OrderBy(Function(data) data.DayDate).ToArray()

    Return values

  End Function

  ''' <summary>
  ''' Gets the working hours.
  ''' </summary>
  ''' <returns>List of working hours.</returns>
  Public Function GetDateAndHourData() As IEnumerable(Of DateAndHourData)

    Dim timeTableDayData = GetTimeTableData()
    Dim dayHourData = (From d In timeTableDayData
                        Select New DateAndHourData With {
                            .DayDate = d.DayDate,
                            .WorkingHours = d.WorkHours}).ToArray()


    Return dayHourData
  End Function

  ''' <summary>
  ''' Gets the sums of the hour data.
  ''' </summary>
  ''' <returns>Sum of hour data.</returns>
  Public Function SumHourData() As Decimal
    Dim sum As Decimal = 0D
    Dim workingHours = GetDateAndHourData()

    For Each hoursData In workingHours
      If hoursData.WorkingHours.HasValue Then
        sum = sum + hoursData.WorkingHours.Value
      End If
    Next
    Return sum
  End Function


  ''' <summary>
  ''' Gets the absence codes day data.
  ''' </summary>
  ''' <returns>List of absence code data of days.</returns>
  Public Function GetDateAndAbsenceCodeData() As IEnumerable(Of DateAndAbsenceCodeData)

    Dim rawData = GetTimeTableData()
    Dim absenceCodeData = (From d In rawData
                        Select New DateAndAbsenceCodeData With {
                            .DayDate = d.DayDate,
                            .AbsenceCode = d.FehlzeitCode}).ToArray()


    Return absenceCodeData
  End Function

  ''' <summary>
  ''' Gets regular working hours and flexible time.
  ''' </summary>
  ''' <param name="maximalWorkingHoursPerWorkDay">The maximal working hours per work day.</param>
  ''' <returns>Working hours and flexible time.</returns>
  Public Function GetRegularWorkingHoursAndFlexibleTime(ByVal maximalWorkingHoursPerWorkDay As Decimal) As WorkingHoursAndFlexibleTime

    Dim workingHours = GetDateAndHourData()

    Dim limitedDayHoursList As New List(Of DateAndHourData)

    Dim flexibleTime As Decimal = 0D
    Dim sumRegularWorkingHours As Decimal = 0D

    For Each hoursData In workingHours

      Dim regularWorkingHoursDay As Decimal? = Nothing

      If hoursData.WorkingHours.HasValue Then

        Select Case hoursData.DayDate.DayOfWeek
          Case DayOfWeek.Saturday, DayOfWeek.Sunday

            ' No regular working hours on weekends
            regularWorkingHoursDay = 0D
            flexibleTime = flexibleTime + hoursData.WorkingHours.Value
          Case Else
            ' Normal work day
            If hoursData.WorkingHours.Value > maximalWorkingHoursPerWorkDay Then
              regularWorkingHoursDay = maximalWorkingHoursPerWorkDay
              flexibleTime = flexibleTime + (hoursData.WorkingHours.Value - maximalWorkingHoursPerWorkDay)
            Else
              regularWorkingHoursDay = hoursData.WorkingHours.Value
            End If

        End Select

        sumRegularWorkingHours = sumRegularWorkingHours + regularWorkingHoursDay

      End If

      Dim limitedDayHoursDAta = New DateAndHourData With {.DayDate = hoursData.DayDate, .WorkingHours = regularWorkingHoursDay}

      limitedDayHoursList.Add(limitedDayHoursDAta)
    Next

    Dim result As New WorkingHoursAndFlexibleTime With {.WorkingHoursWithoutFlexTime = limitedDayHoursList,
                                                        .SumRegularWorkingHours = sumRegularWorkingHours,
                                                        .SumFlexibleTime = flexibleTime}

    Return result
  End Function

  ''' <summary>
  ''' Replaces empty hours.
  ''' </summary>
  ''' <param name="value">The replacement value.</param>
  Public Sub ReplaceEmptyHours(ByVal value As Decimal)

    value = Math.Max(MIN_HOURS, Math.Min(MAX_HOURS, value))

    Dim timeTableDayData = GetTimeTableData()

    For Each dayData In timeTableDayData

      If (Not dayData.WorkHours.HasValue OrElse
        dayData.WorkHours = 0) AndAlso
        String.IsNullOrEmpty(dayData.FehlzeitCode) Then

        dayData.WorkHours = value

      End If
    Next

  End Sub

  ''' <summary>
  ''' Replaces empty absence codes.
  ''' </summary>
  ''' <param name="absenceCode">The replacment value.</param>
  Public Sub ReplaceEmptyAbsenceCodes(ByVal absenceCode As String)

    Dim timeTableDayData = GetTimeTableData()

    For Each dayData In timeTableDayData

      If (Not dayData.WorkHours.HasValue OrElse
        dayData.WorkHours = 0) AndAlso
        String.IsNullOrEmpty(dayData.FehlzeitCode) Then

        dayData.FehlzeitCode = absenceCode

      End If
    Next

  End Sub

  ''' <summary>
  ''' Replaces empty hours evenly.
  ''' </summary>
  ''' <param name="totalHoursValueToDistribute">The total hours value to distribute.</param>
  Public Sub ReplaceEmptyHoursEvenly(ByVal totalHoursValueToDistribute As Decimal)

    Dim timeTableDayData = GetTimeTableData()
    Dim numberOfEmptyDays As Integer = 0
    For Each dayData In timeTableDayData

      If (Not dayData.WorkHours.HasValue OrElse
        dayData.WorkHours = 0) AndAlso
        String.IsNullOrEmpty(dayData.FehlzeitCode) Then

        numberOfEmptyDays = numberOfEmptyDays + 1

      End If
    Next

    If numberOfEmptyDays > 0 Then
      Dim hoursPerDay As Decimal = totalHoursValueToDistribute / numberOfEmptyDays

      ReplaceEmptyHours(hoursPerDay)
    End If
  End Sub

  ''' <summary>
  ''' Sets the display format of work time
  ''' </summary>
  ''' <param name="showMinutes"></param>
  ''' <remarks></remarks>
  Public Sub SetWorkTimeFormat(showMinutes As Boolean)
    Dim timeTableDayData = GetTimeTableData()
    For Each dayData In timeTableDayData
      dayData.ShowMinutes = showMinutes
    Next
  End Sub

#End Region ' Public Methods

#Region "Helper Methods"

  ''' <summary>
  ''' Converts decimal to normal time.
  ''' </summary>
  ''' <param name="decimalTime">The decimal time.</param>
  ''' <returns>Normal time.</returns>
  <Obsolete("Es wird immer die Dezimalzeit belassen und nur die Darstellung angepasst")>
  Private Function DecimalToNormalTime(decimalTime As Decimal) As Decimal
    Dim hours As Integer
    Dim minutes As Double

    hours = Int(decimalTime)
    minutes = Format(decimalTime - hours, "n5")
    minutes = Math.Round(minutes * 100 * 0.6, 0) / 100

    Dim result As Decimal = hours + minutes
    Dim x As Integer = 10 ^ 2
    result = Fix(result * x) / x

    Return result
  End Function

  ''' <summary>
  ''' Converts normal to decimal time.
  ''' </summary>
  ''' <param name="normalTime">The normal time.</param>
  ''' <returns>Decimal time.</returns>
  <Obsolete("Es wird immer die Dezimalzeit belassen und nur die Darstellung angepasst")>
  Public Shared Function NormalToDecimalTime(ByVal normalTime As Decimal, Optional ByVal round As Boolean = True) As Decimal
    Dim hours As Integer
    Dim minutes As Decimal

    hours = Int(normalTime)

    If round Then
      minutes = Format((normalTime - hours), "n5")
      minutes = Math.Round(minutes * 100 / 60, 5)
    Else
      minutes = normalTime - hours
      minutes = (minutes * 100 / 60)
    End If

    Dim result As Decimal = hours + minutes

    Dim x As Integer = 10 ^ 2
    result = Fix(result * x) / x

    Return result
  End Function

#End Region ' Helper Methods

#Region "Helper Classes"

  Public Class WorkingHoursAndFlexibleTime
    Public Property WorkingHoursWithoutFlexTime As IEnumerable(Of DateAndHourData)
    Public Property SumRegularWorkingHours As Decimal
    Public Property SumFlexibleTime As Decimal
  End Class

#End Region ' Helper Classes

End Class


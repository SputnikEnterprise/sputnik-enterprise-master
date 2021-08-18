Namespace Report.DataObjects

  ''' <summary>
  '''  Used fro RPL_MA_Day and RPL_KD_Day data
  ''' </summary>
  Public Class RPLDayData
    Public Property ID As Integer
    Public Property RPNr As Integer?
    Public Property RPLNr As Integer?
    Public Property EmployeeNumber As Integer?
    Public Property CustomerNumber As Integer?
    Public Property ESNr As Integer?
    Public Property Monat As Byte?
    Public Property Jahr As String
    Public Property Tag1 As Decimal?
    Public Property Tag2 As Decimal?
    Public Property Tag3 As Decimal?
    Public Property Tag4 As Decimal?
    Public Property Tag5 As Decimal?
    Public Property Tag6 As Decimal?
    Public Property Tag7 As Decimal?
    Public Property Tag8 As Decimal?
    Public Property Tag9 As Decimal?
    Public Property Tag10 As Decimal?
    Public Property Tag11 As Decimal?
    Public Property Tag12 As Decimal?
    Public Property Tag13 As Decimal?
    Public Property Tag14 As Decimal?
    Public Property Tag15 As Decimal?
    Public Property Tag16 As Decimal?
    Public Property Tag17 As Decimal?
    Public Property Tag18 As Decimal?
    Public Property Tag19 As Decimal?
    Public Property Tag20 As Decimal?
    Public Property Tag21 As Decimal?
    Public Property Tag22 As Decimal?
    Public Property Tag23 As Decimal?
    Public Property Tag24 As Decimal?
    Public Property Tag25 As Decimal?
    Public Property Tag26 As Decimal?
    Public Property Tag27 As Decimal?
    Public Property Tag28 As Decimal?
    Public Property Tag29 As Decimal?
    Public Property Tag30 As Decimal?
    Public Property Tag31 As Decimal?
    Public Property KumulativStd As Decimal?
    Public Property KstNr As Integer?
    Public Property KstBez As String
    Public Property ESLohnNr As Integer?

		Public Property isdecimal As Boolean?

    Public Property Type As RPLType

    ''' <summary>
    ''' Gets the working hours of a day.
    ''' </summary>
    ''' <param name="day">The day.</param>
    ''' <returns>The working hours done at the day.</returns>
    Public Function GetWorkingHoursOfDay(ByVal day As Day) As Decimal?

      Dim columnName As String = String.Format("Tag{0}", Convert.ToInt32(day))
      Dim propertyRef = Me.GetType().GetProperty(columnName)

      Dim dayValue As Decimal? = propertyRef.GetValue(Me, Nothing)

      Return dayValue

    End Function


    ''' <summary>
    ''' Gets the working hours of all days in the month.
    ''' </summary>
    ''' <returns>List of date and working hoursdata.</returns>
    Public Function GetWorkingHoursOfAllDaysInMonth() As IEnumerable(Of DateAndHourData)

      Dim listOfDateAndHourData As New List(Of DateAndHourData)

      ' Check if year and month are set.
      Dim year As Integer = 0
      If Not Integer.TryParse(Jahr, year) Or
         Not Monat.HasValue Then
        Return Nothing
      End If

      Dim daysInMonth = DateTime.DaysInMonth(year, Monat)

      For i As Integer = 1 To daysInMonth

        Dim day As Day = i
        Dim dayHourValue = GetWorkingHoursOfDay(day)

				Dim dayDate = New DateTime(year, Monat, i)

        Dim dayHourData As New DateAndHourData() With {.DayDate = dayDate,
                                                       .WorkingHours = dayHourValue}
        listOfDateAndHourData.Add(dayHourData)

			Next

      Return listOfDateAndHourData
    End Function

    ''' <summary>
    ''' Applies a list of working hours.
    ''' </summary>
    ''' <param name="workingHours">The list of working hours.</param>
    Public Sub ApplyWorkingHours(ByVal workingHours As IEnumerable(Of DateAndHourData))

      ClearAllWorkingHoursOfMonth()

      For Each workingHoursData In workingHours

        Dim day As Day = workingHoursData.DayDate.Day

        Dim columnName As String = String.Format("Tag{0}", Convert.ToInt32(day))
        Dim propertyRef = Me.GetType().GetProperty(columnName)

        propertyRef.SetValue(Me, workingHoursData.WorkingHours, Nothing)

      Next

    End Sub

    ''' <summary>
    ''' Clear all working hours of month.
    ''' </summary>
    Public Sub ClearAllWorkingHoursOfMonth()

      For i = 1 To 31

        Dim day As Day = i

        Dim columnName As String = String.Format("Tag{0}", Convert.ToInt32(day))
        Dim propertyRef = Me.GetType().GetProperty(columnName)

        propertyRef.SetValue(Me, Nothing, Nothing)

      Next

    End Sub

  End Class


  ''' <summary>
  ''' Date and wokring hour data.
  ''' </summary>
  Public Class DateAndHourData

    Public Property DayDate As DateTime
    Public Property WorkingHours As Decimal?

  End Class

End Namespace

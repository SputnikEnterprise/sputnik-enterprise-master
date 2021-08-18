Namespace Report.DataObjects

  Public Class RPLDayHoursTotal

    Public Property RPNr As Integer
    Public Property Tag1 As Decimal
    Public Property Tag2 As Decimal
    Public Property Tag3 As Decimal
    Public Property Tag4 As Decimal
    Public Property Tag5 As Decimal
    Public Property Tag6 As Decimal
    Public Property Tag7 As Decimal
    Public Property Tag8 As Decimal
    Public Property Tag9 As Decimal
    Public Property Tag10 As Decimal
    Public Property Tag11 As Decimal
    Public Property Tag12 As Decimal
    Public Property Tag13 As Decimal
    Public Property Tag14 As Decimal
    Public Property Tag15 As Decimal
    Public Property Tag16 As Decimal
    Public Property Tag17 As Decimal
    Public Property Tag18 As Decimal
    Public Property Tag19 As Decimal
    Public Property Tag20 As Decimal
    Public Property Tag21 As Decimal
    Public Property Tag22 As Decimal
    Public Property Tag23 As Decimal
    Public Property Tag24 As Decimal
    Public Property Tag25 As Decimal
    Public Property Tag26 As Decimal
    Public Property Tag27 As Decimal
    Public Property Tag28 As Decimal
    Public Property Tag29 As Decimal
    Public Property Tag30 As Decimal
    Public Property Tag31 As Decimal
		Public Property isdecimal As Boolean

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
    ''' Gets the number of days that reach a specified working time.
    ''' </summary>
    ''' <param name="workTimeToReach">The working time to reach.</param>
    ''' <returns>Number of days that reach the specified working time.</returns>
    Public Function GetNumberOfDaysThatReachASpecifiedWorktime(ByVal workTimeToReach As Decimal) As Integer
      Dim numberOfDaysThatReachTheWorkingTime As Integer = 0

      For i As Integer = 1 To 31

        Dim day As Day = i
        Dim dayHourValue = GetWorkingHoursOfDay(day)

        If dayHourValue.HasValue AndAlso dayHourValue.Value >= workTimeToReach Then
          numberOfDaysThatReachTheWorkingTime = numberOfDaysThatReachTheWorkingTime + 1
        End If
      Next

      Return numberOfDaysThatReachTheWorkingTime
    End Function



	End Class

End Namespace

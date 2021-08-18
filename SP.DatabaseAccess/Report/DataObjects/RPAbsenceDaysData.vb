Namespace Report.DataObjects

  ''' <summary>
  ''' RP_Fehltage
  ''' </summary>
  Public Class RPAbsenceDaysData
    Public Property ID As Integer
    Public Property RPNr As Integer?
		Public Property RPMonat As Integer?
		Public Property RPJahr As Integer?
    Public Property Fehltag1 As String
    Public Property Fehltag2 As String
    Public Property Fehltag3 As String
    Public Property Fehltag4 As String
    Public Property Fehltag5 As String
    Public Property Fehltag6 As String
    Public Property Fehltag7 As String
    Public Property Fehltag8 As String
    Public Property Fehltag9 As String
    Public Property Fehltag10 As String
    Public Property Fehltag11 As String
    Public Property Fehltag12 As String
    Public Property Fehltag13 As String
    Public Property Fehltag14 As String
    Public Property Fehltag15 As String
    Public Property Fehltag16 As String
    Public Property Fehltag17 As String
    Public Property Fehltag18 As String
    Public Property Fehltag19 As String
    Public Property Fehltag20 As String
    Public Property Fehltag21 As String
    Public Property Fehltag22 As String
    Public Property Fehltag23 As String
    Public Property Fehltag24 As String
    Public Property Fehltag25 As String
    Public Property Fehltag26 As String
    Public Property Fehltag27 As String
    Public Property Fehltag28 As String
    Public Property Fehltag29 As String
    Public Property Fehltag30 As String
    Public Property Fehltag31 As String
    Public Property RPNr2 As Integer?

    ''' <summary>
    ''' Gets absence day code of a day.
    ''' </summary>
    ''' <param name="day">The day.</param>
    ''' <returns>Absence code.</returns>
    Public Function GetAbsenceDayCodeOfDay(ByVal day As Day) As String

      Dim columnName As String = String.Format("Fehltag{0}", Convert.ToInt32(day))
      Dim propertyRef = Me.GetType().GetProperty(columnName)

      Dim absenceValue As String = propertyRef.GetValue(Me, Nothing)

      Return absenceValue

    End Function

    ''' <summary>
    ''' Gets the absence day codes of all days in the month.
    ''' </summary>
    ''' <returns>List of date and absence code data.</returns>
    Public Function GetAbsenceDayCodesOfAllDaysInMonth() As IEnumerable(Of DateAndAbsenceCodeData)

      Dim listOfDateAndAbsenceCodeData As New List(Of DateAndAbsenceCodeData)

      ' Check if year and month are set.
      Dim year As Integer = 0
      If Not Integer.TryParse(RPJahr, year) Or
         Not RPMonat.HasValue Then

        Return listOfDateAndAbsenceCodeData
      End If

      Dim daysInMonth = DateTime.DaysInMonth(year, RPMonat)

      For i As Integer = 1 To daysInMonth

        Dim day As Day = i
        Dim absenceValue = GetAbsenceDayCodeOfDay(day)
        Dim dayDate = New DateTime(year, RPMonat, i)

        If absenceValue Is Nothing Then
          ' If the day is saturday or sunday set initial fehlzeitCode to 'S'
          Select Case dayDate.DayOfWeek
            Case DayOfWeek.Saturday, DayOfWeek.Sunday
              absenceValue = "S"
            Case Else
              absenceValue = String.Empty
          End Select
        End If

        Dim dayHourData As New DateAndAbsenceCodeData() With {.DayDate = dayDate,
                                                              .AbsenceCode = absenceValue}

        listOfDateAndAbsenceCodeData.Add(dayHourData)

      Next

      Return listOfDateAndAbsenceCodeData

    End Function

    ''' <summary>
    ''' Applies absence code data.
    ''' </summary>
    ''' <param name="absenceCodeData">The absence code data.</param>
    Public Sub ApplyAbsenceCodeData(ByVal absenceCodeData As IEnumerable(Of DateAndAbsenceCodeData))

      For Each absenceCodeDayData In absenceCodeData

        Dim day As Day = absenceCodeDayData.DayDate.Day

        Dim columnName As String = String.Format("Fehltag{0}", Convert.ToInt32(day))
        Dim propertyRef = Me.GetType().GetProperty(columnName)

        propertyRef.SetValue(Me, absenceCodeDayData.AbsenceCode, Nothing)

      Next

    End Sub

    ''' <summary>
    ''' Inits absence day data.
    ''' </summary>
    ''' <param name="rpYear">They year.</param>
    ''' <param name="rpMonth">The month.</param>
		Public Sub InitAbsenceDayData(ByVal rpYear As Integer, ByVal rpMonth As Integer)

			Dim initData As New List(Of DateAndAbsenceCodeData)

			Dim daysInMonth = DateTime.DaysInMonth(rpYear, rpMonth)

			For i As Integer = 1 To daysInMonth

				Dim dayDate = New DateTime(rpYear, rpMonth, i)

				Dim absenceValue = GetInitalAbsenceCodeValueOfDate(dayDate)

				Dim absenceCodeData = New DateAndAbsenceCodeData With {
															.DayDate = dayDate,
															.AbsenceCode = absenceValue}

				initData.Add(absenceCodeData)

			Next

			ApplyAbsenceCodeData(initData)

		End Sub

    ''' <summary>
    ''' Inits absence day data before date.
    ''' </summary>
    ''' <param name="rpYear">The year.</param>
    ''' <param name="rpMonth">The month.</param>
    ''' <param name="dateDate">The date.</param>
    Public Sub InitAbsenceDayDataBeforeDate(ByVal rpYear As Integer, ByVal rpMonth As Byte, ByVal dateDate As DateTime)

      Dim initData As New List(Of DateAndAbsenceCodeData)

      Dim endDay = dateDate.Day - 1

      For i As Integer = 1 To endDay

        Dim dayDate = New DateTime(rpYear, rpMonth, i)

        Dim absenceValue = GetInitalAbsenceCodeValueOfDate(dayDate)

        Dim absenceCodeData = New DateAndAbsenceCodeData With {
                              .DayDate = dayDate,
                              .AbsenceCode = absenceValue}

        initData.Add(absenceCodeData)

      Next

      ApplyAbsenceCodeData(initData)
    End Sub

    ''' <summary>
    ''' Inits absence day data after date.
    ''' </summary>
    ''' <param name="rpYear">The year.</param>
    ''' <param name="rpMonth">The month.</param>
    ''' <param name="dateDate">The date.</param>
    Public Sub InitAbsenceDayDataAfterDate(ByVal rpYear As Integer, ByVal rpMonth As Byte, ByVal dateDate As DateTime)

      Dim initData As New List(Of DateAndAbsenceCodeData)

      Dim daysInMonth = DateTime.DaysInMonth(rpYear, rpMonth)

      Dim startDay = dateDate.Day + 1

      For i As Integer = startDay To daysInMonth

        Dim dayDate = New DateTime(rpYear, rpMonth, i)

        Dim absenceValue = GetInitalAbsenceCodeValueOfDate(dayDate)

        Dim absenceCodeData = New DateAndAbsenceCodeData With {
                              .DayDate = dayDate,
                              .AbsenceCode = absenceValue}

        initData.Add(absenceCodeData)

      Next

      ApplyAbsenceCodeData(initData)
    End Sub


    ''' <summary>
    ''' Gets the initial absence code value of a date.
    ''' </summary>
    ''' <param name="dayDate">Te date.</param>
    ''' <returns>Initial absence code value.</returns>
    Private Function GetInitalAbsenceCodeValueOfDate(ByVal dayDate As DateTime)
      Dim absenceValue = String.Empty

      Select Case dayDate.DayOfWeek
        Case DayOfWeek.Saturday, DayOfWeek.Sunday
          absenceValue = "S"
        Case Else
          absenceValue = String.Empty
      End Select

      Return absenceValue

    End Function

    ''' <summary>
    ''' Date and absence code data.
    ''' </summary>
    Public Class DateAndAbsenceCodeData

      Public Property DayDate As DateTime
      Public Property AbsenceCode As String

    End Class

  End Class

End Namespace

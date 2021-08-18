'------------------------------------
' File: DateUtilsTest.vb
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports System.Globalization

<TestClass()>
Public Class DateUtilsTest

    <TestMethod()>
    Public Sub DateOfMonday_DifferentCalendarWeekAndYearCombinationsAreTested_CorrectDateOfMondayIsReturned()

        ' ----------Arrange----------
        Dim dateOfMonday1 As Date = Nothing
        Dim dateOfmonday2 As Date = Nothing
        Dim dateOfmonday3 As Date = Nothing
        Dim dateOfmonday4 As Date = Nothing

        ' ----------Act----------
        dateOfMonday1 = DateUtils.DateOfMonday(1, 2012)
        dateOfmonday2 = DateUtils.DateOfMonday(52, 2012)
        dateOfmonday3 = DateUtils.DateOfMonday(34, 2011)
        dateOfmonday4 = DateUtils.DateOfMonday(25, 2013)

        ' ----------Assert---------- 
        Assert.IsTrue(dateOfMonday1.Day = 2)
        Assert.IsTrue(dateOfMonday1.Month = 1)
        Assert.IsTrue(dateOfMonday1.Year = 2012)

        Assert.IsTrue(dateOfmonday2.Day = 24)
        Assert.IsTrue(dateOfmonday2.Month = 12)
        Assert.IsTrue(dateOfmonday2.Year = 2012)

        Assert.IsTrue(dateOfmonday3.Day = 22)
        Assert.IsTrue(dateOfmonday3.Month = 8)
        Assert.IsTrue(dateOfmonday3.Year = 2011)

        Assert.IsTrue(dateOfmonday4.Day = 17)
        Assert.IsTrue(dateOfmonday4.Month = 6)
        Assert.IsTrue(dateOfmonday4.Year = 2013)

    End Sub

    <TestMethod()>
    Public Sub DateOfMonday_IterativeRangeCheck_CorrectDateOfMondayIsReturned()

        ' ----------Arrange----------
        Dim dateOfMonday As Date = Nothing


        ' ----------Act----------

        For week As Integer = 1 To 53

            dateOfMonday = DateUtils.DateOfMonday(week, 2012)

            Assert.IsTrue(dateOfMonday.DayOfWeek = 1)

            Dim ciCurr As CultureInfo = New CultureInfo("de-CH")
            Dim weekNum As Integer = ciCurr.Calendar.GetWeekOfYear(dateOfMonday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)

            ' ----------Assert---------- 
            Assert.IsTrue(weekNum = week)

        Next

    End Sub


End Class

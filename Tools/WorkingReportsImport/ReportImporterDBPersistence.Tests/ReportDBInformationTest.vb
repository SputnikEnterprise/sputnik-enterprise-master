'------------------------------------
' File: ReportDBInformationTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

<TestClass()>
Public Class ReportDBInformationTest

    <TestMethod()>
    Public Sub IsValid_ValidDataMatrixCodeValue_IsValidReturnsTrue()

        ' ----------Arrange----------

        Dim reportImportDBInformation As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation.DataMatrixCodeValue = "12345678_12_2012"

        ' ----------Assert----------
        Assert.IsTrue(reportImportDBInformation.IsDataMatrixCodeValid)
        Assert.IsTrue(reportImportDBInformation.DataMatrixCodeValue = "12345678_12_2012")
        Assert.IsTrue(reportImportDBInformation.ReportNumber = 12345678)
        Assert.IsTrue(reportImportDBInformation.CalendarWeek = 12)
        Assert.IsTrue(reportImportDBInformation.Year = 2012)

    End Sub

    <TestMethod()>
    Public Sub IsValid_DataMatrixCodeValueIsEmtpy_IsValidReturnsFalse()

        ' ----------Arrange----------

        Dim reportImportDBInformation As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation.DataMatrixCodeValue = ""

        ' ----------Assert----------
        Assert.IsFalse(reportImportDBInformation.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation.ReportNumber)
        Assert.IsNull(reportImportDBInformation.CalendarWeek)
        Assert.IsNull(reportImportDBInformation.Year)

    End Sub

    <TestMethod()>
    Public Sub IsValid_DataMatrixCodeValueIsNothing_IsValidReturnsFalse()

        ' ----------Arrange----------

        Dim reportImportDBInformation As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation.DataMatrixCodeValue = Nothing

        ' ----------Assert----------
        Assert.IsFalse(reportImportDBInformation.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation.ReportNumber)
        Assert.IsNull(reportImportDBInformation.CalendarWeek)
        Assert.IsNull(reportImportDBInformation.Year)

    End Sub

    <TestMethod()>
    Public Sub IsValid_DataMatrixCodeValueHasToManyGroups_IsValidReturnsFalse()

        ' ----------Arrange----------

        Dim reportImportDBInformation1 As New ReportDBInformation
        Dim reportImportDBInformation2 As New ReportDBInformation
        Dim reportImportDBInformation3 As New ReportDBInformation
        Dim reportImportDBInformation4 As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation1.DataMatrixCodeValue = "12345678_12_2012_00"
        reportImportDBInformation3.DataMatrixCodeValue = "12345678_12_2012_00_01"
        reportImportDBInformation1.DataMatrixCodeValue = "ab_12345678_12_2012_00_01"
        reportImportDBInformation1.DataMatrixCodeValue = "0_12345678_12_2012_00_01"

        ' ----------Assert----------
        Assert.IsFalse(reportImportDBInformation1.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation1.ReportNumber)
        Assert.IsNull(reportImportDBInformation1.CalendarWeek)
        Assert.IsNull(reportImportDBInformation1.Year)

        Assert.IsFalse(reportImportDBInformation2.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation2.ReportNumber)
        Assert.IsNull(reportImportDBInformation2.CalendarWeek)
        Assert.IsNull(reportImportDBInformation2.Year)

        Assert.IsFalse(reportImportDBInformation3.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation3.ReportNumber)
        Assert.IsNull(reportImportDBInformation3.CalendarWeek)
        Assert.IsNull(reportImportDBInformation3.Year)

        Assert.IsFalse(reportImportDBInformation4.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation4.ReportNumber)
        Assert.IsNull(reportImportDBInformation4.CalendarWeek)
        Assert.IsNull(reportImportDBInformation4.Year)

    End Sub

    <TestMethod()>
    Public Sub IsValid_DataMatrixCodeValueHasToFewGroups_IsValidReturnsFalse()

        ' ----------Arrange----------

        Dim reportImportDBInformation1 As New ReportDBInformation
        Dim reportImportDBInformation2 As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation1.DataMatrixCodeValue = "12345678"
        reportImportDBInformation2.DataMatrixCodeValue = "12345678_12"

        ' ----------Assert----------
        Assert.IsFalse(reportImportDBInformation1.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation1.ReportNumber)
        Assert.IsNull(reportImportDBInformation1.CalendarWeek)
        Assert.IsNull(reportImportDBInformation1.Year)

        Assert.IsFalse(reportImportDBInformation2.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation2.ReportNumber)
        Assert.IsNull(reportImportDBInformation2.CalendarWeek)
        Assert.IsNull(reportImportDBInformation2.Year)

    End Sub

    <TestMethod()>
    Public Sub IsValid_DataMatrixCodeValueWithInvalidReportNumber_IsValidReturnsFalse()

        ' ----------Arrange----------

        Dim reportImportDBInformation1 As New ReportDBInformation
        Dim reportImportDBInformation2 As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation1.DataMatrixCodeValue = "123x5678_12_201a"
        reportImportDBInformation2.DataMatrixCodeValue = " 12345678_12_201a"

        ' ----------Assert----------
        Assert.IsFalse(reportImportDBInformation1.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation1.ReportNumber)
        Assert.IsNull(reportImportDBInformation1.CalendarWeek)
        Assert.IsNull(reportImportDBInformation1.Year)

        Assert.IsFalse(reportImportDBInformation2.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation2.ReportNumber)
        Assert.IsNull(reportImportDBInformation2.CalendarWeek)
        Assert.IsNull(reportImportDBInformation2.Year)

    End Sub

    <TestMethod()>
    Public Sub IsValid_DataMatrixCodeValueWithInvalidCalendarWeek_IsValidReturnsFalse()

        ' ----------Arrange----------

        Dim reportImportDBInformation1 As New ReportDBInformation
        Dim reportImportDBInformation2 As New ReportDBInformation
        Dim reportImportDBInformation3 As New ReportDBInformation
        Dim reportImportDBInformation4 As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation1.DataMatrixCodeValue = "123x5678_12b_2012"
        reportImportDBInformation2.DataMatrixCodeValue = "12345678_x_2012"
        reportImportDBInformation3.DataMatrixCodeValue = "12345678_100_2012"
        reportImportDBInformation4.DataMatrixCodeValue = "12345678_2x_2012"

        ' ----------Assert----------
        Assert.IsFalse(reportImportDBInformation1.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation1.ReportNumber)
        Assert.IsNull(reportImportDBInformation1.CalendarWeek)
        Assert.IsNull(reportImportDBInformation1.Year)

        Assert.IsFalse(reportImportDBInformation2.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation2.ReportNumber)
        Assert.IsNull(reportImportDBInformation2.CalendarWeek)
        Assert.IsNull(reportImportDBInformation2.Year)

        Assert.IsFalse(reportImportDBInformation3.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation3.ReportNumber)
        Assert.IsNull(reportImportDBInformation3.CalendarWeek)
        Assert.IsNull(reportImportDBInformation3.Year)

        Assert.IsFalse(reportImportDBInformation4.IsDataMatrixCodeValid)
        Assert.IsNull(reportImportDBInformation4.ReportNumber)
        Assert.IsNull(reportImportDBInformation4.CalendarWeek)
        Assert.IsNull(reportImportDBInformation4.Year)

    End Sub

    <TestMethod()>
    Public Sub IsValid_DataMatrixCodeValueWithInvalidYear_IsValidReturnsFalse()

        ' ----------Arrange----------

        Dim reportImportDBInformation1 As New ReportDBInformation
        Dim reportImportDBInformation2 As New ReportDBInformation
        Dim reportImportDBInformation3 As New ReportDBInformation
        Dim reportImportDBInformation4 As New ReportDBInformation
        Dim reportImportDBInformation5 As New ReportDBInformation
        Dim reportImportDBInformation6 As New ReportDBInformation
        Dim reportImportDBInformation7 As New ReportDBInformation

        ' ----------Act----------
        reportImportDBInformation1.DataMatrixCodeValue = "12345678_12_201a"
        reportImportDBInformation2.DataMatrixCodeValue = "12345678_12_2012x"
        reportImportDBInformation3.DataMatrixCodeValue = "12345678_12_1"
        reportImportDBInformation4.DataMatrixCodeValue = "12345678_12_12"
        reportImportDBInformation5.DataMatrixCodeValue = "12345678_12_123"
        reportImportDBInformation6.DataMatrixCodeValue = "12345678_12_12345"
        reportImportDBInformation7.DataMatrixCodeValue = "12345678_12_2012 "

        ' ----------Assert----------
        Assert.IsFalse(reportImportDBInformation1.IsDataMatrixCodeValid)
        Assert.IsFalse(reportImportDBInformation2.IsDataMatrixCodeValid)
        Assert.IsFalse(reportImportDBInformation3.IsDataMatrixCodeValid)
        Assert.IsFalse(reportImportDBInformation4.IsDataMatrixCodeValid)
        Assert.IsFalse(reportImportDBInformation5.IsDataMatrixCodeValid)
        Assert.IsFalse(reportImportDBInformation6.IsDataMatrixCodeValid)
        Assert.IsFalse(reportImportDBInformation7.IsDataMatrixCodeValid)

    End Sub

End Class

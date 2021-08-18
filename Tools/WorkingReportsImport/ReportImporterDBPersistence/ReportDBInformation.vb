'------------------------------------
' File: ReportDBInformation.vb
' Date:  16.11.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text.RegularExpressions
Imports System.IO
Imports ReportImporterCommon.Logging


''' <summary>
''' Report information that is persisted in the database.
''' </summary>
Public Class ReportDBInformation
	Private Shared m_logger As ILogger = New Logger()

#Region "Private Const Fields"
	''' <summary>
	''' Regular expression pattern of DataMatrix value.
	''' </summary>
	Private Const DATAMATRIX_VALUE_PATTERN_0 As String = "^(?<RecordNo>\d+)_(?<Week>\d{1,2})_(?<Year>\d{4})$"
	Private Const DATAMATRIX_VALUE_PATTERN As String = "^(?<RecordNo>\d+)_(?<Week>\d{1,2})_(?<Year>\d{4})_(?<Month>\d{1,2})$"
	Private Const DATAMATRIX_VALUE_PATTERN_2 As String = "^(?<RecordNo>\d+)_(?<Week>\d{1,2})_(?<Year>\d{4})_(?<Month>\d{1,2})_(?<firstRPDay>\d{1,2})_(?<lastRPDay>\d{1,2})$"

	Private Const DATAMATRIX_VALUE_PATTERN_EMPLOYEE As String = "^MA_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"
	Private Const DATAMATRIX_VALUE_PATTERN_CUSTOMER As String = "^KD_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"
	Private Const DATAMATRIX_VALUE_PATTERN_EMPLOYMENT As String = "^employment_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"
	Private Const DATAMATRIX_VALUE_PATTERN_REPORT As String = "^RP_(?<RecordNo>\d+)_(?<Year>\d+)_(?<Month>\d+)_(?<Week>\d+)_(?<FirstRPDay>\d+)_(?<LastRPDay>\d+)$"
	Private Const DATAMATRIX_VALUE_PATTERN_REPORTLINE As String = "^RP_(?<RecordNo>\d+)_(?<Year>\d+)_(?<Month>\d+)_(?<Week>\d+)_(?<FirstRPDay>\d+)_(?<LastRPDay>\d+)_(?<RPLID>\d+)$"
	Private Const DATAMATRIX_VALUE_PATTERN_INVOICE As String = "^invoice_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"
	Private Const DATAMATRIX_VALUE_PATTERN_PAYROLL As String = "^payroll_(?<RecordNo>\d+)_(?<DocCategorieID>\d+)$"


	Private Const FILE_VALUE_PATTERN_EMPLOYEE As String = "^employee_(?<DocCategorieID>\d+)_(?<BusinessBranchID>\d+)_(?<Timestamp>\d+)"
	Private Const FILE_VALUE_PATTERN_CUSTOMER As String = "^customer_(?<DocCategorieID>\d+)_(?<BusinessBranchID>\d+)_(?<Timestamp>\d+)"


#End Region

#Region "Private Fields"

	''' <summary>
	''' The extracted value from the DataMatrix.
	''' </summary>
	Private dataMatrixValue As String

	''' <summary>
	''' The report number value.
	''' </summary>
	Private reportNumberValue As Integer?

	''' <summary>
	''' The calendar week value.
	''' </summary>
	Private calendarWeekValue As Integer?

	''' <summary>
	''' The year value.
	''' </summary>
	Private yearValue As Integer?
	Private monthValue As Integer?
	Private FirstRPDayValue As Integer?
	Private LastRPDayValue As Integer?
	Private rplineIDValue As Integer?

	''' <summary>
	''' Boolean flag indicating if the DataMatrix code is valid.
	''' </summary>
	Private isDataMatrixCodeValidValue As Boolean = False

#End Region

#Region "Public Properties"

	''' <summary>
	''' Gets or sets the MDGuid value.
	''' </summary>
	Public Property MDGuid As String

	''' <summary>
	''' Gets or sets the pdf full path.
	''' </summary>
	Public Property PDFFullPath As String

	''' <summary>
	''' Gets or sets the employee number.
	''' </summary>
	Public Property ModulRecordNumber As Integer?


	''' <summary>
	''' Gets or sets the document categorie number.
	''' </summary>
	Public Property DocumentCategoryNumber As Integer?

	''' <summary>
	''' Gets or sets the document BusinessBranch number.
	''' </summary>
	Public Property BusinessBranch As Integer?

	Public Property Pages As Integer?

	''' <summary>
	''' Gets or sets the employe number.
	''' </summary>
	Public Property EmployeNumber As Integer?

	''' <summary>
	''' Gets or sets the payroll number.
	''' </summary>
	Public Property PayrollNumber As Integer?

	''' <summary>
	''' Gets or sets the if is for webservice useing.
	''' </summary>
	Public Property WorkingForWebService As Boolean?

	''' <summary>
	''' Gets or sets the original file guid.
	''' </summary>
	Public Property OriginalFileGuid As String

	Public Property ScanModulType As ScanTypes

	Public Enum ScanTypes
		employee
		customer
		employment
		reports
		invoice
		payroll
	End Enum

	''' <summary>
	''' Gets or sets the DataMatrix code value.
	''' </summary>
	Public Property DataMatrixCodeValue As String

		Get
			Return Me.dataMatrixValue
		End Get

		Set(value As String)
			Dim isNewWebServerReport As Boolean = False
			Dim match As Match
			Dim matchFileName As Match

			ScanModulType = ScanTypes.reports
			Me.isDataMatrixCodeValidValue = True

			Dim pdfFileInfo As FileInfo = New FileInfo(PDFFullPath)
			Dim pdffilename = pdfFileInfo.Name.ToLower
			If Not (pdffilename.StartsWith("employee_".ToLower) OrElse pdffilename.StartsWith("customer_".ToLower)) Then
				DocumentCategoryNumber = 0
			End If
			BusinessBranch = 0
			matchFileName = Nothing

			Me.dataMatrixValue = IIf(String.IsNullOrEmpty(value), String.Empty, value)
			m_logger.LogInfo(String.Format("Anzahl Split in Datamatrix-Code: {0} | {1}", value, value.Split(CChar("_")).Count))

			If dataMatrixValue.StartsWith("MA_") OrElse dataMatrixValue.StartsWith("employee_") Then
				match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_EMPLOYEE)
				matchFileName = Regex.Match(pdffilename, FILE_VALUE_PATTERN_EMPLOYEE)
				ScanModulType = ScanTypes.employee

			ElseIf dataMatrixValue.StartsWith("KD_") OrElse dataMatrixValue.StartsWith("customer_") Then
				match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_CUSTOMER)
				matchFileName = Regex.Match(pdffilename, FILE_VALUE_PATTERN_CUSTOMER)
				ScanModulType = ScanTypes.customer

			ElseIf dataMatrixValue.StartsWith("employment_") Then
				match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_EMPLOYMENT)
				ScanModulType = ScanTypes.employment

			ElseIf dataMatrixValue.StartsWith("invoice_") Then
				match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_INVOICE)
				ScanModulType = ScanTypes.invoice

			ElseIf dataMatrixValue.StartsWith("payroll_") Then
				match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_PAYROLL)
				ScanModulType = ScanTypes.payroll

			ElseIf dataMatrixValue.StartsWith("RP_") OrElse dataMatrixValue.StartsWith("report_") Then
				If dataMatrixValue.Split("_"c).Count > 7 Then
					match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_REPORTLINE)
				Else
					match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_REPORT)
				End If
				ScanModulType = ScanTypes.reports
				isNewWebServerReport = True


			Else

				ScanModulType = ScanTypes.reports
				Dim splitNumber As Integer = Me.dataMatrixValue.Split(CChar("_")).Count
				Select Case splitNumber
					Case 4
						m_logger.LogInfo(String.Format("Neue Art der RegEx: {0}", Me.dataMatrixValue))
						match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN)

					Case 6
						m_logger.LogInfo(String.Format("RegEx with fRPDay and lRPDay: {0}", Me.dataMatrixValue))
						match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_2)

					Case Else
						m_logger.LogInfo(String.Format("Alte Art der RegEx: {0}", Me.dataMatrixValue))
						match = Regex.Match(Me.dataMatrixValue, DATAMATRIX_VALUE_PATTERN_0)

				End Select
			End If


			If (match.Success) Then
				'If Not ScanModulType = ScanTypes.reports Then
				ModulRecordNumber = Convert.ToInt32(match.Groups("RecordNo").Value)
				'End If

				Select Case ScanModulType
					Case ScanTypes.employee
						If matchFileName.Success Then
							DocumentCategoryNumber = Convert.ToInt32(matchFileName.Groups("DocCategorieID").Value)
							BusinessBranch = Convert.ToInt32(matchFileName.Groups("BusinessBranchID").Value)
						Else
							DocumentCategoryNumber = Convert.ToInt32(match.Groups("DocCategorieID").Value)
						End If

					Case ScanTypes.customer
						If matchFileName.Success Then
							DocumentCategoryNumber = Convert.ToInt32(matchFileName.Groups("DocCategorieID").Value)
							BusinessBranch = Convert.ToInt32(matchFileName.Groups("BusinessBranchID").Value)
						Else
							DocumentCategoryNumber = Convert.ToInt32(match.Groups("DocCategorieID").Value)
						End If

					Case ScanTypes.employment
						DocumentCategoryNumber = Convert.ToInt32(match.Groups("DocCategorieID").Value)

					Case ScanTypes.invoice
						DocumentCategoryNumber = Convert.ToInt32(match.Groups("DocCategorieID").Value)

					Case ScanTypes.payroll
						DocumentCategoryNumber = Convert.ToInt32(match.Groups("DocCategorieID").Value)

					Case ScanTypes.reports
						yearValue = Convert.ToInt32(match.Groups("Year").Value)
						calendarWeekValue = Convert.ToInt32(match.Groups("Week").Value)

						If isNewWebServerReport Then
							monthValue = Convert.ToInt32(match.Groups("Month").Value)
							FirstRPDayValue = Convert.ToInt32(match.Groups("FirstRPDay").Value)
							LastRPDayValue = Convert.ToInt32(match.Groups("LastRPDay").Value)
							If match.Groups("RPLID").Success Then
								rplineIDValue = Convert.ToInt32(match.Groups("RPLID").Value)
							End If

						Else
							Try
								m_logger.LogInfo(String.Format("Month wird kontrolliert: {0}", Me.dataMatrixValue))
								Me.monthValue = Convert.ToInt32(match.Groups("Month").Value)

							Catch ex As Exception
								' wird andhand Monday errechnet...
								Me.monthValue = GetFirstDayOfWeek(Me.yearValue, Me.calendarWeekValue).Month
								m_logger.LogError(String.Format("Month hat Fehler und neu lautet: {0} -> {1}", Me.monthValue, Me.dataMatrixValue))

							End Try

							' fRPDay...
							Try
								Me.FirstRPDayValue = Convert.ToInt32(match.Groups("firstRPDay").Value)
								m_logger.LogInfo(String.Format("FirstRPDay wird kontrolliert: {0} >>> {1}", Me.dataMatrixValue, Me.FirstRPDayValue))

							Catch ex As Exception
								' wird andhand Monday errechnet...
								If GetFirstDayOfWeek(Me.yearValue, Me.calendarWeekValue).Month < Me.monthValue Then
									Me.FirstRPDayValue = 1
								Else
									Me.FirstRPDayValue = GetFirstDayOfWeek(Me.yearValue, Me.calendarWeekValue).Day
								End If
								m_logger.LogError(String.Format("FirstRPDay hat Fehler und neu lautet: {0} >>> {1}", Me.dataMatrixValue, Me.FirstRPDayValue))

							End Try

							' lRPDay...
							Try
								Me.LastRPDayValue = Convert.ToInt32(match.Groups("lastRPDay").Value)
								m_logger.LogInfo(String.Format("LastRPDay wird kontrolliert: {0} >>> {1}", Me.dataMatrixValue, Me.LastRPDayValue))

							Catch ex As Exception
								' wird andhand Monday errechnet...
								Me.LastRPDayValue = Math.Min((GetFirstDayOfWeek(Me.yearValue, Me.calendarWeekValue).AddDays(6)).Day, Date.DaysInMonth(Me.yearValue, Me.monthValue))
								m_logger.LogError(String.Format("LastRPDay hat Fehler und neu lautet: {0} >>> {1}", Me.dataMatrixValue, Me.LastRPDayValue))

							End Try

						End If

					Case Else
						Me.isDataMatrixCodeValidValue = False

				End Select

			Else
				Me.isDataMatrixCodeValidValue = False
			End If

		End Set

	End Property

	Public Function GetLastOccurenceOfDay(ByVal value As DateTime, ByVal dayOfWeek As DayOfWeek) As DateTime
		Dim tempDate As DateTime = value.AddDays(-1)
		While tempDate.DayOfWeek <> dayOfWeek
			tempDate = tempDate.AddDays(-1)
		End While

		Return tempDate
	End Function

	Public Function GetFirstDayOfWeek(ByVal year As Integer, ByVal weekNumber As Integer) As DateTime
		Dim januar4 As New DateTime(year, 1, 4)
		Dim weekdayjah4 As Integer = GetDayOfWeek(januar4)
		Dim dateoffirstWeek As DateTime = januar4.AddDays(1 - weekdayjah4)

		Return dateoffirstWeek.AddDays((weekNumber - 1) * 7)
	End Function

	Function GetDayOfWeek(ByVal myDate As DateTime) As Integer
		Return (myDate.DayOfWeek + 6) Mod 7 + 1
	End Function



	Private Function GetFirstDayOfMonth(ByVal dtDate As DateTime) As DateTime
		Dim dtFrom As DateTime = dtDate
		dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1))

		Return dtFrom
	End Function

	Private Function GetLastDayOfMonth(ByVal dtDate As DateTime) As DateTime
		Dim dtTo As New DateTime(dtDate.Year, dtDate.Month, 1)

		dtTo = dtTo.AddMonths(1)
		dtTo = dtTo.AddDays(-(dtTo.Day))

		Return dtTo
	End Function



#End Region

#Region "Public Readonly Properties"

	''' <summary>
	''' Gets the report number value.
	''' </summary>
	Public ReadOnly Property ReportNumber As Integer?
		Get
			Return Me.reportNumberValue
		End Get
	End Property

	''' <summary>
	''' Gets the calendarweek value.
	''' </summary>
	Public ReadOnly Property CalendarWeek As Integer?
		Get
			Return Me.calendarWeekValue
		End Get
	End Property

	''' <summary>
	''' Gets the year value.
	''' </summary>
	Public ReadOnly Property Year As Integer?
		Get
			Return Me.yearValue
		End Get
	End Property

	Public ReadOnly Property Month As Integer?
		Get
			Return Me.monthValue
		End Get
	End Property

	Public ReadOnly Property FirstRPDay As Integer?
		Get
			Return Me.FirstRPDayValue
		End Get
	End Property

	Public ReadOnly Property LastRPDay As Integer?
		Get
			Return Me.LastRPDayValue
		End Get
	End Property

	Public ReadOnly Property RepportLineID As Integer?
		Get
			Return Me.rplineIDValue.GetValueOrDefault(0)
		End Get
	End Property

	''' <summary>
	'''  Returns if the DataMatrix code is valid.
	''' </summary>
	Public ReadOnly Property IsDataMatrixCodeValid As Boolean
		Get
			Return Me.isDataMatrixCodeValidValue
		End Get
	End Property

#End Region

End Class


Public Class NotificationData
	Public Property Customer_ID As String
	Public Property NotifyArt As Integer?
	Public Property NotifyHeader As String
	Public Property NotifyComments As String
	Public Property CreatedFrom As String

End Class
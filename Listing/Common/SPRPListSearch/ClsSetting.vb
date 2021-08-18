Imports SPProgUtility

Public Class ClsSetting

  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class MandantenData

  Public Property MDNr As Integer
  Public Property MDName As String
  Public Property MDGuid As String
  Public Property MDConnStr As String
  Public Property MultiMD As Short

End Class

Public Class EmployeeData

	Public Property EmployeeNumber As Integer?
	Public Property LastName As String
	Public Property Firstname As String
	Public Property Postcode As String
	Public Property Location As String

	Public ReadOnly Property LastnameFirstname As String
		Get
			Return String.Format("{0}, {1}", LastName, Firstname)
		End Get
	End Property

	Public ReadOnly Property PostcodeAndLocation As String
		Get
			Return String.Format("{0} {1}", Postcode, Location)
		End Get
	End Property

End Class

Public Class CustomerData

	Public Property CustomerNumber As Integer
	Public Property Company1 As String
	Public Property Street As String
	Public Property Postcode As String
	Public Property Location As String

	Public ReadOnly Property PostcodeAndLocation As String
		Get

			Return String.Format("{0} {1}", Postcode, Location)

		End Get
	End Property

End Class

Public Class EmploymentData

	Public Property EmploymentNumber As Integer
	Public Property EmployeeNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property Company1 As String
	Public Property EmployeeFirstname As String
	Public Property EmployeeLasstname As String
	Public Property EmploymentAs As String
	Public Property EmploymentFrom As DateTime?
	Public Property EmploymentTo As DateTime?

	Public ReadOnly Property EmployeeFullname As String
		Get

			Return String.Format("{1}, {0}", EmployeeFirstname, EmployeeLasstname)

		End Get
	End Property

	Public ReadOnly Property EmploymentFromTO As String
		Get

			Return String.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy} ", EmploymentFrom, EmploymentTo)

		End Get
	End Property

	Public ReadOnly Property EmploymentViewData As String
		Get

			Return String.Format("({0}): {1} | {2:dd.MM.yyyy} - {3:dd.MM.yyyy} ", EmploymentNumber, EmployeeFullname, EmploymentFrom, EmploymentTo)

		End Get
	End Property

End Class

Public Class ReportData

	Public Property ReportNumber As Integer
	Public Property EmploymentNumber As Integer
	Public Property EmployeeNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property Company1 As String
	Public Property EmployeeFirstname As String
	Public Property EmployeeLasstname As String
	Public Property EmploymentAs As String
	Public Property ReportFrom As DateTime?
	Public Property ReportTo As DateTime?

	Public ReadOnly Property EmployeeFullname As String
		Get

			Return String.Format("{1}, {0}", EmployeeFirstname, EmployeeLasstname)

		End Get
	End Property

	Public ReadOnly Property ReportFromTO As String
		Get

			Return String.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy} ", ReportFrom, ReportTo)

		End Get
	End Property

	Public ReadOnly Property ReportViewData As String
		Get

			Return String.Format("({0}): {1} | {2:dd.MM.yyyy} - {3:dd.MM.yyyy} ", ReportNumber, EmployeeFullname, ReportFrom, ReportTo)

		End Get
	End Property

End Class


Public Class SortData

	Public Property BezValue As String

End Class


Public Class FoundedRPData

  Public Property RPNr As Integer
  Public Property MANr As Integer
  Public Property KDNr As Integer
  Public Property ESNr As Integer

  Public Property employeename As String
  Public Property customername As String

  Public Property monthyear As String
  Public Property esperiode As String
  Public Property es_als As String
  Public Property rpperiode As String
  Public Property weeknumbers As String


End Class


Public Class SearchCriteria

  Public Property mandantenname As String
  Public Property listname As String
  Public Property EmployeeNumber As Integer?
  Public Property CustomerNumber As Integer?
	Public Property Employmentnumber As Integer?
	Public Property Reportnumber As Integer?
	Public Property FromDate As Date
	Public Property ToDate As Date
	Public Property FromMonth As Integer?
	Public Property FromYear As Integer?
	Public Property FirstWeek As Integer
	Public Property LastWeek As Integer
	Public Property ReportWeeks As String

	Public Property kst1 As String
	Public Property Kst1Label As String
  Public Property kst2 As String
  Public Property Kst2Label As String
  Public Property filiale As String
  Public Property Berater As String

  Public Property SortIn As String


End Class


''' <summary>
''' RP Data structur to create RPPrint Record.
''' </summary>
Public Class RPDataForRPPrint
  Public Property RPNr As Integer?
  Public Property ESNr As Integer?
  Public Property MANr As Integer?
  Public Property KDNr As Integer?
  Public Property Monat As Byte?
  Public Property Jahr As String
  Public Property Von As DateTime?
  Public Property Bis As DateTime?
  Public Property Erfasst As Boolean?
  Public Property PrintedWeeks As String
  Public Property PrintedDate As String
  Public Property MDNr As Integer?
  Public Property IsMonthClosed As Boolean

End Class


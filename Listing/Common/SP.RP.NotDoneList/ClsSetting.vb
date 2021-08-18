
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
	Public Property FromMonth As Integer?
	Public Property FromYear As Integer?
	Public Property kst1 As String
	Public Property Kst1Label As String
	Public Property kst2 As String
	Public Property Kst2Label As String
	Public Property filiale As String
	Public Property Berater As String

	Public Property SortIn As String


End Class


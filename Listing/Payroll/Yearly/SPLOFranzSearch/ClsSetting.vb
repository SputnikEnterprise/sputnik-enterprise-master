
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

Public Class ComboboxValue
	Public Property ComboValue As String
	Public Property ComboCaption As String

End Class


Public Class SearchCriteria

	Public Property mandantenname As String
	Public Property listname As String
	Public Property MANrList As String

	Public Property FirstYear As Integer
	Public Property filiale As String
	Public Property EmployeeCanton As String
	Public Property EmployeeTaxCanton As String

	Public Property sqlsearchstring As String

End Class


Public Class FoundedData

	' Jahr, MANr, Nachname, Vorname, BLohn, magebdat, mageschlecht, eintritt, austritt
	Public Property manr As Integer?

	Public Property employeeLastname As String
	Public Property employeeFirstname As String

	Public Property blohn As Decimal?
	Public Property gebdat As DateTime?
	Public Property geschlecht As String

	Public Property eintritt As DateTime?
	Public Property austritt As DateTime?
	Public Property EmployeeCanton As String
	Public Property EmployeeTaxCanton As String

	Public ReadOnly Property employeename As String
		Get
			Return String.Format("{0}, {1}", employeeLastname, employeeFirstname)
		End Get
	End Property


End Class

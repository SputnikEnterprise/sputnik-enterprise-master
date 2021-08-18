
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


''' <summary>
''' KD-KST data.
''' </summary>
Public Class KSTData
	Public Property kstnr As Integer?
	Public Property kstbez As String
	Public Property esnr As Integer?
	Public Property rpnr As Integer?
	Public Property kdnr As Integer?
	Public Property totalstd As Decimal?

End Class


Public Class SearchCriteria

	Public Property mandantenname As String
	Public Property listname As String
	Public Property KDNrList As String
	Public Property ESNrList As String
	Public Property MANrList As String

	Public Property FirstYear As Integer
	Public Property FirstMonth As Integer
	Public Property LastMonth As Integer

	Public Property beruf As String


	Public Property sqlsearchstring As String

End Class


Public Class FoundedData

	' MANr, Nachname, Vorname, Gebdat, AHV_Nr, Ablp, BisLP, anzahlstd, Lohnsumme 
	Public Property MDNr As Integer?

	Public Property manr As Integer?

	Public Property employeeLastname As String
	Public Property employeeFirstname As String

	Public Property ablp As Integer?
	Public Property bislp As Integer?


	Public Property anzahlstd As Decimal?
	Public Property lohnsumme As Decimal?

	Public Property ahvnr As String

	Public Property gebdat As DateTime?

	Public ReadOnly Property abbislp As String
		Get
			Return String.Format("{0:00}-{1:00}", ablp, bislp)
		End Get
	End Property

	Public ReadOnly Property employeename As String
		Get
			Return String.Format("{0}, {1}", employeeLastname, employeeFirstname)
		End Get
	End Property


End Class

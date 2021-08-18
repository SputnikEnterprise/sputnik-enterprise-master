
Imports SPProgUtility

Public Class ClsSetting

	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class EachPayrollAGAnteilData

	Public Property AHVAnteil As Decimal
	Public Property AHVAmountEachEmployee As Decimal
	Public Property AGAnteil As Decimal
	Public Property AGAmountEachEmployee As Decimal
	Public Property AGBVGAmountEachEmployee As Decimal

End Class

Public Class SearchCriteria

	Public Property mandantenname As String
	Public Property listname As String
	Public Property sortvalue As String
	Public Property FirstMonth As Integer
	Public Property LastMonth As Integer
	Public Property FirstYear As Integer
	Public Property LastYear As Integer
	Public Property kst As String
	Public Property filiale As String
	Public Property esbranche As String
	Public Property kanton As String
	Public Property customercity As String
	Public Property employeenationality As String
	Public Property employeecountry As String


	Public Property employeeNumber As Integer?
	Public Property customerNumber As Integer?
	Public Property esNumber As Integer?

End Class


'Public Class MandantenData

'	Public Property MDNr As Integer
'	Public Property MDName As String
'	Public Property MDGuid As String
'	Public Property MDConnStr As String
'	Public Property MultiMD As Short

'End Class


Public Class FoundedData

	Public Property MDNr As Integer?
	Public Property monat As Integer?
	Public Property jahr As Integer?

	Public Property EmployeeNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property EmploymentNumber As Integer?

	Public Property EmployeeFirstName As String
	Public Property EmployeeLastName As String
	Public Property CustomerName As String

	Public Property ID As Integer?
	Public Property kst3_1 As String
	Public Property kst3bez As String
	Public Property USFiliale As String

	Public Property _tempumsatz As Decimal?
	Public Property _indumsatz As Decimal?
	Public Property _festumsatz As Decimal?


	Public Property bruttolohn As Decimal?
	Public Property ahvlohn As Decimal?
	Public Property agbetrag As Decimal?
	Public Property fremdleistung As Decimal?

	Public Property lohnaufwand_1 As Decimal?
	Public Property lohnaufwand_2 As Decimal?
	Public Property _marge As Decimal?

	Public Property _bgtemp As Decimal?
	Public Property _bgind As Decimal?
	Public Property _bgfest As Decimal?


	Public ReadOnly Property EmployeeFullName
		Get
			Return (String.Format("{0}, {1}", EmployeeLastName, EmployeeFirstName))
		End Get
	End Property


End Class



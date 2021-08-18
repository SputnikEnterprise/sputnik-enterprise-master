
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


Public Class SearchCriteria

	Public Property MDNr As Integer
	Public Property MandanteNname As String
	Public Property listname As String
	Public Property FromMonth As Integer?
	Public Property FromYear As Integer

	Public Property EmployeeNumberList As String
	'Public Property filiale As String

	'Public Property ShowG500 As Boolean?
	'Public Property ShowG600 As Boolean?
	'Public Property ShowG700 As Boolean?

	'Public Property ShowG529 As Boolean?
	'Public Property ShowG629 As Boolean?
	'Public Property ShowG729 As Boolean?

	'Public Property ShowGDar As Boolean?
	'Public Property ShowGGTime As Boolean?

End Class


'Public Class FoundedData

'	Public Property MANr As Integer?

'	Public Property employeelastname As String
'	Public Property employeefirstname As String
'	Public Property employeefullname As String

'	Public Property lanr As Decimal?
'	Public Property laname As String
'	Public Property januar As Decimal?
'	Public Property februar As Decimal?

'	Public Property march As Decimal?
'	Public Property april As Decimal?
'	Public Property mai As Decimal?

'	Public Property juni As Decimal?
'	Public Property juli As Decimal?
'	Public Property august As Decimal?

'	Public Property september As Decimal?
'	Public Property oktober As Decimal?
'	Public Property november As Decimal?
'	Public Property dezember As Decimal?
'	Public Property kumulativ As Decimal?

'	Public Property gebdat As DateTime?
'	Public Property esbegin As DateTime?
'	Public Property esende As DateTime?

'	Public Property ahvnew As String

'End Class

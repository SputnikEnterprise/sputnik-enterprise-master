
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

	Public Property mandantenname As String
	Public Property listname As String
	Public Property manr As String
	Public Property bismonat As String
	Public Property bisjahr As String
	Public Property filiale As String

	Public Property ShowG500 As Boolean?
	Public Property ShowG600 As Boolean?
	Public Property ShowG700 As Boolean?

	Public Property ShowG529 As Boolean?
	Public Property ShowG629 As Boolean?
	Public Property ShowG729 As Boolean?

	Public Property ShowGDar As Boolean?
	Public Property ShowGGTime As Boolean?
	Public Property ToleranceLimit As Decimal?
	Public Property ExistsTolerantAmount As Boolean

End Class


Public Class FoundedData

	Public Property MANr As Integer

	Public Property employeelastname As String
	Public Property employeefirstname As String
	Public Property employeefullname As String

	Public Property g500 As Decimal?
	Public Property g600 As Decimal?
	Public Property g700 As Decimal?

	Public Property g529 As Decimal?
	Public Property g629 As Decimal?
	Public Property g729 As Decimal?

	Public Property gdar As Decimal?
	Public Property ggtotal As Decimal?
	Public Property ggtime As Decimal?

End Class

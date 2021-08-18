
Imports SPProgUtility

Public Class ClsSetting

	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class SearchCriteria

	Public Property sortvalue As Integer
	Public Property mandantenname As String
	Public Property listname As String
	Public Property MANr As String
	Public Property FromEmployee As String
	Public Property ToEmployee As String

	Public Property jahr As Integer

	Public Property employeeCanton As String
	Public Property employeeCountry As String
	Public Property employeePermission As String


End Class


Public Class FoundedData

	Public Property IsSelected As Boolean

	Public Property MANr As Integer?

	Public Property employeelastname As String
	Public Property employeefirstname As String

	Public Property employeemastrasse As String
	Public Property employeemaplz As String
	Public Property employeemaort As String
	Public Property employeemaland As String
	Public Property employeemaco As String
	Public Property employeeahv_nr As String
	Public Property employeeahv_nr_new As String
	Public Property employeesend2wos As Boolean?
	Public Property employeegeschlecht As String
	Public Property employeemapostfach As String
	Public Property employeelajahr As Integer?


	Public Property Z_1_0 As Decimal?
	Public Property Z_2_1 As Decimal?
	Public Property Z_2_2 As Decimal?
	Public Property Z_2_3 As Decimal?
	Public Property Z_3_0 As Decimal?

	Public Property Z_4_0 As Decimal?
	Public Property Z_5_0 As Decimal?

	Public Property Z_6_0 As Decimal?
	Public Property Z_7_0 As Decimal?
	Public Property Z_8_0 As Decimal?
	Public Property Z_9_0 As Decimal?

	Public Property Z_10_1 As Decimal?
	Public Property Z_10_2 As Decimal?
	Public Property Z_11_0 As Decimal?
	Public Property Z_12_0 As Decimal?


	Public Property Z_13_1_1 As Decimal?
	Public Property Z_13_1_2 As Decimal?
	Public Property Z_13_2_1 As Decimal?
	Public Property Z_13_2_2 As Decimal?
	Public Property Z_13_2_3 As Decimal?
	Public Property Z_13_3_0 As Decimal?


	Public Property NLA_LoAusweis As Boolean?
	Public Property NLA_Befoerderung As Boolean?
	Public Property NLA_Kantine As Boolean?


	Public Property NLA_2_3 As String
	Public Property NLA_3_0 As String
	Public Property NLA_4_0 As String
	Public Property NLA_7_0 As String


	Public Property NLA_Spesen_NotShow As Boolean?
	Public Property NLA_13_1_2 As String
	Public Property NLA_13_2_3 As String


	Public Property NLA_Nebenleistung_1 As String
	Public Property NLA_Nebenleistung_2 As String
	Public Property NLA_Bemerkung_1 As String
	Public Property NLA_Bemerkung_2 As String
	Public Property Grund As String


	Public Property ES_Ab1 As String
	Public Property ES_Bis1 As String


	Public ReadOnly Property LastnameFirstname As String
		Get
			Return String.Format("{0}, {1}", employeelastname, employeefirstname)
		End Get
	End Property

	Public ReadOnly Property PostcodeCity As String
		Get
			Return String.Format("{0} {1}", employeemaplz, employeemaort)
		End Get
	End Property

End Class

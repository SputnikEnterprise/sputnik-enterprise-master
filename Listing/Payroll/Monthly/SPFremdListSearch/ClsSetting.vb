
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

	Public Property mandantenname As String
	Public Property listname As String
	Public Property manr As String
	Public Property vonmonat As String
	Public Property bismonat As String
	Public Property vonjahr As String
	Public Property bisjahr As String
	Public Property lohnarten As String
	Public Property kst As String
	Public Property filiale As String
	Public Property esbranche As String

	Public Property ShowBruttolohn As Integer?
	Public Property ShowSUVABasis As Integer?
	Public Property ShowAHVBasis As Integer?

End Class

Public Class FoundedData
	' LONR, LANR, MANR, Nachname, Vorname, LP, Jahr, KST, 
	' M_BTR, Bruttopflichtig, Ahvpflichtig, NBUVpflichtig, GAV_Beruf, GAV_Kanton,	ESBranche, ESEinstufung, Filiale1, Filiale2, MonatVon, MonatBis, JahrVon, JahrBis

	Public Property lanr As Decimal?
	Public Property lalotext As String
	Public Property betrag As Decimal?


End Class

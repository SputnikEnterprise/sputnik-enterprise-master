
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
	Public Property Filiale As String
	Public Property TeilungFiliale As Boolean?
	Public Property Db1Data As Boolean?

End Class



Public Class FoundedLOANData

	Public Property MANr As Integer?
	Public Property lanr As Decimal?
	Public Property lalotext As String

	Public Property vonmonat As Integer?
	Public Property bismonat As Integer?
	Public Property vonjahr As Integer?
	Public Property bisjahr As Integer?
	Public Property lonr As Integer?

	Public Property jahr As Integer?
	Public Property monat As Integer?

	Public Property gav_beruf As String
	Public Property gav_kanton As String
	Public Property gav_gruppe1 As String

	Public Property ahv_nr As String
	Public Property ahv_nr_new As String
	Public Property gebdat As Date?

	Public Property employeename As String
	Public Property employeestreet As String
	Public Property employeepostcode As String
	Public Property employeepostcodecity As String
	Public Property employeecity As String
	Public Property employeecountry As String

	Public Property m_anz As Decimal?
	Public Property m_bas As Decimal?
	Public Property m_ans As Decimal?
	Public Property m_btr As Decimal?


End Class


Public Class FoundedLORekapData

	'lanr, jahr, lp, betrag, kumulativ, hkonto, skonto, bezeichnung, usnr 

	Public Property lanr As Decimal?
	Public Property lalotext As String

	Public Property jahr As Integer?
	Public Property monat As Integer?


	Public Property hkonto As Integer?
	Public Property skonto As Integer?

	Public Property betrag As Decimal?
	Public Property kumulativ As Decimal?

	Public Property userNumber As Integer?

End Class


Public Class FoundedLOKTGData

	' LOL.LONr, LOL.MANR, LOL.Jahr, LOL.LP, LOL.LANR, LA.LALOText, LOL.M_Bas, LOL.M_Anz, LOL.M_Ans, LOL.M_Btr, LOL.GAV_Kanton, LOL.GAV_Beruf, LOL.GAV_Gruppe1, Mitarbeiter.Nachname, 
	' Mitarbeiter.Vorname, Mitarbeiter.AHV_Nr, Mitarbeiter.AHV_Nr_New, Mitarbeiter.Land As MALand, Mitarbeiter.PLZ, Mitarbeiter.Ort, Mitarbeiter.Strasse, Mitarbeiter.GebDat, 
	' Mitarbeiter.KST As MAKST, 2014 As VonJahr, 1 As VonMonat, 2014 As BisJahr, 1 As BisMonat, LOL.GAV_Kanton, Mitarbeiter.PLZ, Mitarbeiter.Ort, tblBVG.ESBegin, tblBVG.BVGBEgin 

	Public Property MANr As Integer?
	Public Property lanr As Decimal?
	Public Property lalotext As String

	Public Property vonmonat As Integer?
	Public Property bismonat As Integer?
	Public Property vonjahr As Integer?
	Public Property bisjahr As Integer?
	Public Property lonr As Integer?

	Public Property jahr As Integer?
	Public Property monat As Integer?

	Public Property gav_beruf As String
	Public Property gav_kanton As String
	Public Property gav_gruppe1 As String

	Public Property ahv_nr As String
	Public Property ahv_nr_new As String
	Public Property gebdat As Date?

	Public Property employeename As String
	Public Property employeestreet As String
	Public Property employeepostcode As String
	Public Property employeepostcodecity As String
	Public Property employeecity As String
	Public Property employeecountry As String

	Public Property m_anz As Decimal?
	Public Property m_bas As Decimal?
	Public Property m_ans As Decimal?
	Public Property m_btr As Decimal?

	Public Property esbegin As String
	Public Property bvgbegin As String

End Class

'Public Class Form1

'  Dim MyGuid As String

'  Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
'    Dim sQuery As String = "Select Kunden.KDNr, Kunden.FProperty, Kunden.Firma1, Kunden.PLZ As KDPLZ, Kunden.Ort As KDOrt, Kunden.Land As KDLand, Kunden.Telefon As KDTelefon, Kunden.Telefax As KDTelefax, Kunden.Strasse As KDStrasse, Kunden.KDState1, Kunden.KDState2, Kunden.HowKontakt As KDKontakt, Kunden.eMail As KDeMail, Kunden.Currency As KDCurrency, Kunden.Bemerkung As KDBemerkung, Kunden.Kreditlimite As KDKreditlimite, Kunden.KreditlimiteAb As KDKreditLimiteAb, Kunden.KreditlimiteBis As KDKreditLimiteBis, Kunden.Faktura As KDFaktura, Kunden.ZahlKond As KDZahlKond, Kunden.MahnCode As KDMahnCode, Kunden.MwStNr As KDMwStNr, Kunden.MwSt As KDMwSt, Kunden.KD_Telefax_Mailing, Kunden.KD_Mail_Mailing, KD_Zustaendig.RecNr As ZHDRecNr, KD_Zustaendig.Anrede, KD_Zustaendig.Nachname, KD_Zustaendig.Vorname, KD_Zustaendig.AnredeForm, KD_Zustaendig.Telefon As ZHDTelefon, KD_Zustaendig.Telefax As ZHDTelefax, KD_Zustaendig.Natel As ZHDNatel, KD_Zustaendig.eMail As ZHDeMail, KD_Zustaendig.ZHD_Telefax_Mailing, KD_Zustaendig.ZHD_SMS_Mailing, KD_Zustaendig.ZHD_Mail_Mailing, KD_Zustaendig.Postfach As ZHDPostfach, KD_Zustaendig.Strasse As ZHDStrasse, KD_Zustaendig.PLZ As ZHDPLZ, KD_Zustaendig.Ort As ZHDOrt, KD_Zustaendig.Abteilung As ZHDAbt, KD_Zustaendig.Position As ZHDPos, KD_Zustaendig.Interessen As ZHDInteressen, KD_Zustaendig.Bemerkung As ZHDBemerkung, KD_Zustaendig.Geb_Dat As ZHDGebdat, KD_Zustaendig.KDZHowKontakt, KD_Zustaendig.KDZState1, KD_Zustaendig.KDZState2, KD_Zustaendig.Berater As ZHDBerater , convert(datetime, '01.01.1900') As ZHDKontaktDate From Kunden Left Join KD_Zustaendig On Kunden.KDNr = KD_Zustaendig.KDNr  Where  Kunden.KDNr in (3,6) Order by  Kunden.Firma1"

'    sQuery = "SELECT * FROM _Kundenliste_1 ORDER BY  KDTelefax /*BEGIN TRY DROP TABLE _Kundenliste_1 END TRY BEGIN CATCH END CATCH Select KD.KDNr, KD.FProperty, KD.Firma1, KD.Firma2 As KDFirma2, KD.Firma3 As KDFirma3, KD.PLZ As KDPLZ, KD.Ort As KDOrt, KD.Postfach As KDPostfach, KD.Land As KDLand, KD.Telefon As KDTelefon, KD.Telefax As KDTelefax, KD.Strasse As KDStrasse, KD.KDState1, KD.KDState2, KD.HowKontakt As KDKontakt, KD.eMail As KDeMail, KD.Currency As KDCurrency, KD.Bemerkung As KDBemerkung, KD.Kreditlimite As KDKreditlimite, KD.KreditlimiteAb As KDKreditLimiteAb, KD.KreditlimiteBis As KDKreditLimiteBis, KD.Kreditlimite_2 As KDKreditlimite_2, KD.KL_RefNr As KL_RefNr, KD.KreditWarnung, KD.KDFiliale As KDAllFiliale, KD.Faktura As KDFaktura, KD.ZahlKond As KDZahlKond, KD.MahnCode As KDMahnCode, KD.MwStNr As KDMwStNr, KD.MwSt As KDMwSt, KD.KD_Telefax_Mailing, KD.KD_Mail_Mailing, KD_Zustaendig.RecNr As ZHDRecNr, KD_Zustaendig.Anrede, KD_Zustaendig.Nachname, KD_Zustaendig.Vorname, KD_Zustaendig.AnredeForm, KD_Zustaendig.Telefon As ZHDTelefon, KD_Zustaendig.Telefax As ZHDTelefax, KD_Zustaendig.Natel As ZHDNatel, KD_Zustaendig.eMail As ZHDeMail, KD_Zustaendig.ZHD_Telefax_Mailing, KD_Zustaendig.ZHD_SMS_Mailing, KD_Zustaendig.ZHD_Mail_Mailing, KD_Zustaendig.Postfach As ZHDPostfach, KD_Zustaendig.Strasse As ZHDStrasse, KD_Zustaendig.PLZ As ZHDPLZ, KD_Zustaendig.Ort As ZHDOrt, KD_Zustaendig.Abteilung As ZHDAbt, KD_Zustaendig.Position As ZHDPos, KD_Zustaendig.Interessen As ZHDInteressen, KD_Zustaendig.Bemerkung As ZHDBemerkung, KD_Zustaendig.Geb_Dat As ZHDGebdat, KD_Zustaendig.KDZHowKontakt, KD_Zustaendig.KDZState1, KD_Zustaendig.KDZState2, KD_Zustaendig.Berater As ZHDBerater, convert(datetime, '01.01.1900') As ZHDKontaktDate, Convert(nvarchar(10), Convert(DateTime, '01.01.1900'),104) As ZHDKontaktDateString, Convert(nvarchar(5),Convert(DateTime, '01.01.1900 00:00:00',104),108) as ZHDKontaktTimeString, '' As ZHDKontaktRecNr, '' As ZHDKontaktType1, '' As ZHDKontaktDauer, '' As ZHDKontakte, '' As ZHDKontakteFrom, (   CASE    WHEN ( SELECT top 1 ESNR     FROM ES     WHERE     ES.KDNR = KD.KDNR And     ES.NoListing = 0 And     (ES.ES_Ende >= convert(nvarchar(10), GETDATE() ,104) Or ES.ES_Ende Is Null) And      ES.ES_Ab <= convert(nvarchar(10), GETDATE() ,104)) > 0 THEN '****** '    WHEN KD.NoES = 1 THEN '******* '	WHEN (	SELECT TOP 1 KDKredi.KDNR		FROM Kunden KDKredi		WHERE KDKredi.KDNR IN (		SELECT RE.KDNR FROM RE GROUP BY RE.KDNR 		HAVING		RE.KDNR = KD.KDNR And		(KD.Kreditlimite > 0 And		IsNull(Sum(RE.BetragInk-RE.Bezahlt),0) > KD.Kreditlimite))		) > 0 THEN '******** '    ELSE  ''   END) As '0'INTO _Kundenliste_1 From Kunden KD Left Join KD_Zustaendig On KD.KDNr = KD_Zustaendig.KDNr  Where  KD.KDNr = 3 Order by  KD.Firma1, KD.Ort, KD.Kreditlimite, KD.Telefax*/"

'    Me.TextBox1.Text = sQuery
'    MyGuid = System.Guid.NewGuid().ToString()

'  End Sub

'  Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
'    'Dim oMailObject As New SPSSendMail.ClsMain_Net
'    ''Dim spOffertest As New SPSOfferUtility_Net.ClsMain_Net
'    ''Dim sQuery As String = Me.TextBox1.Text

'    ''spOffertest.ShowMainForm(sQuery)

'    'oMailObject.StartWithFaxing(29, 3, 0, True, "KDTelefax", MyGuid, "C:\Users\Fardin Asghari.SPUTNIK\Documents\TempFile_1.taf") ';C:\Users\Fardin Asghari.SPUTNIK\Documents\TempFile.taf")

'  End Sub

'  Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
'    Dim oMailObject As New SPSOfferUtility_Net.ClsMain_Net
'    oMailObject.PrintLLDocToFile(96, 8123, 3, 24, True, False, "15.1.1")
'    'oMailObject.StartWithMailing(29, 3, 0, True, "kdEmail", MyGuid, "") ' "C:\Users\Fardin Asghari.SPUTNIK\Documents\TempFile_1.taf;C:\Users\Fardin Asghari.SPUTNIK\Documents\TempFile.taf")

'  End Sub

'  Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
'    'Dim oMailObject As New SPSTapi.ClsMain_Net

'    'oMailObject.ShowfrmTapi("0628650163", 0, 3, 0, 0)

'  End Sub

'  Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
'    'Dim oMailObject As New SPKDSearch.ClsMain_Net()

'    'oMailObject.ShowfrmKDSearch()

'  End Sub

'  Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
'    Dim oMailObject As New SPSOfferUtility_Net.ClsMain_Net()

'    oMailObject.ShowMainForm(Me.TextBox1.Text)

'  End Sub

'End Class

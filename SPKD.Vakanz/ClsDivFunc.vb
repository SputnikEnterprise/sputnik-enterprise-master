
Imports System.Text.RegularExpressions

Partial Class frmVakanzen



#Region "Übersetzen der Variable..."


	Private Function ParseTemplateFile(ByVal TextToTranslate As String) As String
		Dim ParsedFile As String = TextToTranslate
		Dim pattern As String = String.Empty

		Const REGEX_USMDName As String = "\{(?i)TMPL_VAR name=\'USMDName\'\}"
		Const REGEX_USMDName2 As String = "\{(?i)TMPL_VAR name=\'USMDName2\'\}"
		Const REGEX_USMDPostfach As String = "\{(?i)TMPL_VAR name=\'USMDPostfach\'\}"
		Const REGEX_USMDStrasse As String = "\{(?i)TMPL_VAR name=\'USMDStrasse\'\}"
		Const REGEX_USMDOrt As String = "\{(?i)TMPL_VAR name=\'USMDort\'\}"
		Const REGEX_USMDPlz As String = "\{(?i)TMPL_VAR name=\'USMDPlz\'\}"
		Const REGEX_USMDLand As String = "\{(?i)TMPL_VAR name=\'USMDLand\'\}"
		Const REGEX_USMDTelefon As String = "\{(?i)TMPL_VAR name=\'USMDTelefon\'\}"
		Const REGEX_USMDDTelefon As String = "\{(?i)TMPL_VAR name=\'USMDDTelefon\'\}"
		Const REGEX_USMDTelefax As String = "\{(?i)TMPL_VAR name=\'USMDTelefax\'\}"
		Const REGEX_USMDeMail As String = "\{(?i)TMPL_VAR name=\'USMDeMail\'\}"
		Const REGEX_USMDHomepage As String = "\{(?i)TMPL_VAR name=\'USMDHomepage\'\}"

		Const REGEX_USAnrede As String = "\{(?i)TMPL_VAR name=\'USAnrede\'\}"
		Const REGEX_USNachname As String = "\{(?i)TMPL_VAR name=\'USNachname\'\}"
		Const REGEX_USVorname As String = "\{(?i)TMPL_VAR name=\'USVorname\'\}"
		Const REGEX_USPostfach As String = "\{(?i)TMPL_VAR name=\'USPostfach\'\}"
		Const REGEX_USStrasse As String = "\{(?i)TMPL_VAR name=\'USStrasse\'\}"
		Const REGEX_USPLZ As String = "\{(?i)TMPL_VAR name=\'USPLZ\'\}"
		Const REGEX_USOrt As String = "\{(?i)TMPL_VAR name=\'USOrt\'\}"
		Const REGEX_USLand As String = "\{(?i)TMPL_VAR name=\'USLand\'\}"
		Const REGEX_USTelefon As String = "\{(?i)TMPL_VAR name=\'USTelefon\'\}"
		Const REGEX_USNatel As String = "\{(?i)TMPL_VAR name=\'USNatel\'\}"
		Const REGEX_USTelefax As String = "\{(?i)TMPL_VAR name=\'USTelefax\'\}"
		Const REGEX_USeMail As String = "\{(?i)TMPL_VAR name=\'USeMail\'\}"
		Const REGEX_USAbteilung As String = "\{(?i)TMPL_VAR name=\'USAbteilung\'\}"
		Const REGEX_USTitel_1 As String = "\{(?i)TMPL_VAR name=\'USTitel_1\'\}"
		Const REGEX_USTitel_2 As String = "\{(?i)TMPL_VAR name=\'USTitel_2\'\}"


		Try
			'// search templatevars
			'pattern = REGEX_USNachname
			Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(pattern)

			Dim userData = m_CommonDatabaseAccess.LoadAdvisorDataforGivenNumber(m_InitializationData.MDData.MDNr, m_InitializationChangedData.UserData.UserNr)
			If userData Is Nothing Then
				m_Logger.LogError("user could not be founded!")

				Return String.Empty
			End If

			' Benutzerdaten ...............................................................................................
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USNachname, userData.Lastname)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USVorname, userData.Firstname)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USAnrede, userData.Salutation)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTelefon, userData.UserMDTelefon)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USNatel, userData.UserMobile)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTelefax, userData.UserMDTelefax)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USeMail, userData.UserMDeMail)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USPostfach, userData.UserMDPostfach)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USStrasse, userData.UserMDStrasse)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USPLZ, userData.UserMDPLZ)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USOrt, userData.UserMDOrt)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USLand, userData.UserMDLand)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USAbteilung, userData.UserFTitel)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDName, userData.UserMDName)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDName2, userData.UserMDName2)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDPostfach, userData.UserMDPostfach)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDStrasse, userData.UserMDStrasse)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDOrt, userData.UserMDOrt)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDPlz, userData.UserMDPLZ)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDLand, userData.UserMDLand)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDTelefon, userData.UserMDTelefon)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDDTelefon, String.Format("{0}", userData.UserMDDTelefon))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDTelefax, userData.UserMDTelefax)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDeMail, userData.UserMDeMail)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDHomepage, userData.UserMDHomepage)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTitel_1, userData.UserFTitel)
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTitel_2, userData.UserSTitel)


			'ParsedFile = SetSyntax(ParsedFile)


		Catch ex As Exception
			'      ParsedFile = String.Empty
			'      MsgBox("Feher: " & ex.Message.Trim & vbCrLf & ParsedFile & vbCrLf & pattern & vbCrLf & line)

		End Try

		Return ParsedFile
	End Function


#End Region


End Class

Public Class ClsDbFunc


	Private m_md As Mandant
	Private m_utility As Utilities


#Region "Funktionen für Speichern der Daten..."

	Public Property GetVakNr() As Integer
	Public Property GetVak_Bezeichnung() As String
	Public Property GetVak_KST() As String
	Public Property GetVak_Filiale() As String
	Public Property GetVak_Kontakt() As String
	Public Property GetVak_State() As String

	Public Property GetVak_Slogan() As String
	Public Property GetVak_Gruppe() As String
	Public Property GetVak_SBN As String

	Public Property GetVak_ZHDNachName() As String
	Public Property GetVak_ZHDVorName() As String

	Public Property GetVak_ExistLink() As Boolean
	Public Property GetVak_KDJobLink() As String
	Public Property GetVak_MAZivil() As String
	Public Property GetVak_MALohn() As String
	Public Property GetVak_Jobtime() As String

	Public Property MDNr() As String
	Public Property GetVak_CustomerGuid() As String
	Public Property GetVak_JobPLZ() As String
	Public Property GetVak_JobOrt() As String

	Public Property GetVak_MAFSchein() As String
	Public Property GetVak_Bemerkung() As String
	Public Property GetVak_KDBeschreibung() As String
	Public Property GetVak_KDBietet() As String
	Public Property GetVak_SBeschreibung() As String
	Public Property GetVak_Reserve1() As String
	Public Property GetVak_Taetigkeit() As String
	Public Property GetVak_Anforderung() As String
	Public Property GetVak_Reserve2() As String
	Public Property GetVak_Reserve3() As String
	Public Property GetVak_Ausbildung() As String
	Public Property GetVak_Weiterbildung() As String
	Public Property GetVak_SKennt() As String

	Public Property GetVak_EDVKennt() As String
	Public Property GetVak_Transfered_User() As String
	Public Property GetVak_Transfered_On() As String
	Public Property GetVak_Transfered_Guid() As String

	Public Property GetKDNr As Integer?
	Public Property GetKDZHDNr As Integer?

#End Region




	Public Sub New()

		m_md = New Mandant
		m_utility = New Utilities

	End Sub


#Region "Funktionen zur Suche nach Daten..."

	'Function GetLocalSQLString(ByVal bSelectAsNew As Boolean) As String
	'	Dim sql As String

	'	sql = "SELECT TOP 1"
	'	sql &= "V.ID ,"
	'	sql &= "V.VakNr ,"
	'	sql &= "V.Berater ,"
	'	sql &= "V.Filiale ,"
	'	sql &= "Convert(Int, V.VakKontakt) VakKontakt ,"
	'	sql &= "Convert(Int, V.VakState) VakState ,"
	'	sql &= "V.SBNNumber,"
	'	sql &= "V.SBNPublicationState,"
	'	sql &= "V.Bezeichnung ,"
	'	sql &= "V.Slogan ,"
	'	sql &= "V.Gruppe ,"
	'	sql &= "V.KDNr ,"
	'	sql &= "V.KDZHDNr ,"
	'	sql &= "V.ExistLink ,"
	'	sql &= "V.VakLink ,"
	'	sql &= "V.Beginn ,"
	'	sql &= "V.JobProzent ,"
	'	sql &= "V.Anstellung ,"
	'	sql &= "V.Dauer ,"
	'	sql &= "V.MAAge ,"
	'	sql &= "V.MASex ,"
	'	sql &= "V.MAZivil ,"
	'	sql &= "V.MALohn ,"
	'	sql &= "V.Jobtime ,"
	'	sql &= "V.JobOrt ,"
	'	sql &= "V.MAFSchein ,"
	'	sql &= "V.MAAuto ,"
	'	sql &= "V.MANationality ,"
	'	sql &= "V.IEExport ,"
	'	sql &= "V.KDBeschreibung ,"
	'	sql &= "V.KDBietet ,"
	'	sql &= "V.SBeschreibung ,"
	'	sql &= "V.Reserve1 ,"
	'	sql &= "V.Taetigkeit ,"
	'	sql &= "V.Anforderung ,"
	'	sql &= "V.Reserve2 ,"
	'	sql &= "V.Reserve3 ,"
	'	sql &= "V.Ausbildung ,"
	'	sql &= "V.Weiterbildung ,"
	'	sql &= "V.SKennt ,"
	'	sql &= "V.EDVKennt ,"
	'	sql &= "V.CreatedOn ,"
	'	sql &= "V.CreatedFrom ,"
	'	sql &= "V.ChangedOn ,"
	'	sql &= "V.ChangedFrom ,"
	'	sql &= "V.Result ,"
	'	sql &= "V.Vak_Region ,"
	'	sql &= "V.Transfered_User ,"
	'	sql &= "V.Transfered_On ,"
	'	sql &= "V.Transfered_Guid ,"
	'	sql &= "V.Vak_Kanton ,"
	'	sql &= "V.Customer_Guid ,"
	'	sql &= "V.Bemerkung ,"
	'	sql &= "V.MDNr ,"
	'	sql &= "V.JobPLZ ,"
	'	sql &= "V.UserKontakt ,"
	'	sql &= "V.UserEMail ,"
	'	sql &= "V.TitelForSearch ,"
	'	sql &= "V.ShortDescription ,"
	'	sql &= "KD.Firma1 ,"
	'	sql &= "KDz.Nachname AS KDzNachname ,"
	'	sql &= "KDz.Vorname As KDzVorname ,"
	'	sql &= "US.Vorname AS USVorname ,"
	'	sql &= "US.Nachname As USNachname ,"
	'	sql &= "ISNULL(JCH.IsOnline, 0) AS ISOnline"
	'	sql &= " FROM    Vakanzen V"
	'	sql &= " LEFT JOIN Kunden KD ON V.KDNr = KD.KDNr"
	'	sql &= " LEFT JOIN KD_Zustaendig KDz On V.KDZHDNr = KDz.RecNr"
	'	sql &= " AND KD.KDNr = KDz.KDNr"
	'	sql &= " LEFT JOIN Benutzer US On V.Berater = US.KST"
	'	sql &= " LEFT JOIN Vak_JobCHData JCH On V.VakNr = JCH.VakNr"

	'	If Not bSelectAsNew Then sql &= " Where V.VakNr = @iVakNr"

	'	Return sql
	'End Function

	'Function GetLocalSQL4JobCHString() As String
	'	Dim sSql As String = String.Empty

	'	sSql = "If Not Exists(Select ID From Vak_JobCHData Where VakNr = @iVakNr) "
	'	sSql &= "Insert Into Vak_JobCHData (VakNr, USNr, Vak_Sprache, Organisation_ID, Organisation_SubID, Layout_ID, Logo_ID) "
	'	sSql &= "Values (@iVakNr, @USNr, 'de', "
	'	sSql &= "IsNull((Select Top 1 Organisation_ID FROM tblJobAccount Where Organisation_SubID = (IsNull((Select Top 1 JCH_SubID FROM Benutzer Where USNr = @USNr), 0)) And Customer_Guid = @Customer_Guid ), 0), "
	'	sSql &= "IsNull((Select Top 1 JCH_SubID FROM Benutzer Where USNr = @USNr), 0), "
	'	sSql &= "IsNull((Select Top 1 JCH_LayoutID FROM Benutzer Where USNr = @USNr), 0), IsNull((Select Top 1 JCH_LogoID FROM Benutzer Where USNr = @USNr), 0)) "

	'	sSql &= "Select Top 1 V.VakNr, V.UserKontakt, V.SBNNumber, V.UserEMail, V.TitelForSearch, V.ShortDescription, "
	'	sSql &= "V.Beginn, V.JobProzent, V.Anstellung, V.Dauer, V.MASex, V.Vak_Kanton, V.MAAge, V.IEExport, "
	'	sSql &= "KD.Firma1, "
	'	sSql &= "KDz.Nachname As KDzNachname, KDz.Vorname As KDzVorname, "
	'	sSql &= "US.Vorname As USVorname, US.Nachname As USNachname, "
	'	sSql &= "JBra.Bez_Value As BranchenValue, JBra.Bezeichnung As BranchenBez, "
	'	sSql &= "IsNull(JCH.Position_Value, '') As Position_Value, IsNull(JCH.Position, '') As Position, "

	'	sSql &= "JCH.Organisation_ID, JCH.Organisation_SubID, JCH.Inserat_ID, "
	'	sSql &= "JCH.Our_URL, JCH.Direkt_URL, JCH.Branche, JCH.Vak_Sprache, "
	'	sSql &= "JCH.Layout_ID, JCH.Logo_ID, JCH.Bewerben_URL, JCH.Angebot_Value, "
	'	sSql &= "JCH.Xing_Poster_URL, JCH.Xing_Company_Profile_URL, JCH.Xing_Company_Is_Poc, "
	'	sSql &= "JCH.StartDate, JCH.EndDate, JCH.IsOnline, "
	'	sSql &= "JCH.CreatedOn, JCH.CreatedFrom, JCH.ChangedOn, JCH.ChangedFrom, "

	'	'sSql &= "tj.Organisation_ID As USJCHOrganisation_ID, "
	'	'sSql &= "tj.[Organisation_SubID] As USJCHOrganisation_SubID, "

	'	sSql &= "US.JCH_SubID As USJCHSub_ID, US.JCH_LayoutID As USJCHLayout_ID, US.JCH_LogoID As USJCHLogo_ID, "

	'	'sSql &= "USJCH.Angebot_Value As USJCHAngebot_Value, "
	'	sSql &= "USJCH.Our_URL As USJCHOur_URL, USJCH.DaysToAdd, "
	'	sSql &= "USJCH.Direkt_URL As USJCHDirekt_URL, "
	'	sSql &= "USJCH.Xing_Poster_URL As USJCHXing_Poster_URL, "
	'	sSql &= "USJCH.Xing_Company_Profile_URL As USJCHXing_Company_Profile_URL, "
	'	sSql &= "USJCH.Xing_Company_Is_Poc As USJCHXing_Company_Is_Poc "

	'	sSql &= "From Vakanzen V "
	'	sSql &= "Left Join Kunden KD On V.KDNr = KD.KDNr "
	'	sSql &= "Left Join KD_Zustaendig KDz On V.KDZhdNr = KDz.RecNr And KD.KDNr = KDz.KDNr "
	'	sSql &= "Left Join Benutzer US On V.Berater = US.KST "
	'	sSql &= "Left Join Vak_JobCHBranchenData JBra On V.Vaknr = JBra.VakNr "
	'	sSql &= "Left Join Vak_JobCHData JCH On V.VakNr = JCH.VakNr "
	'	sSql &= "Left Join US_JobPlattforms USJCH On USJCH.Customer_Guid = @Customer_Guid "
	'	'sSql &= "Left Join tblJobAccount tj On tj.Customer_Guid = @Customer_Guid And tj.Organisation_ID = usjch.Organisation_ID "

	'	sSql &= "Left Join Vak_OstJobData OJ On V.VakNr = OJ.VakNr "


	'	sSql &= "Where "
	'	'sSql &= "USJCH.Jobplattform_Art = 1 And "
	'	sSql &= "V.VakNr = @iVakNr "

	'	Return sSql
	'End Function

	'Function GetSQLString4Print(ByVal iProposeNr As Integer) As String
	'	Dim sSql As String = "[Get Propose Data 4 Print]"

	'	Return sSql
	'End Function

	'Function GetKDZHDSQLString(ByVal iKDNr As Integer, ByVal iZHDNr As Integer) As String
	'	Dim sSql As String = String.Empty

	'	If iZHDNr = 0 Then
	'		sSql = "Select Top 1 KD.eMail As KDeMail, KD.Firma1 As Firma1 "
	'		sSql &= "From Kunden KD Where KD.KDNr = @iKDNr"

	'	Else
	'		sSql = "Select Top 1 KD.eMail As KDeMail, KD.Firma1 As Firma1, "
	'		sSql &= "ZHD.Anrede As ZHDAnrede, ZHD.Anredeform As ZHDAnredeForm, "
	'		sSql &= "ZHD.Nachname As ZHDNachname, ZHD.Vorname As ZHDVorname, "
	'		sSql &= "ZHD.eMail As ZHDeMail "
	'		sSql &= "From Kunden KD Left Join KD_Zustaendig ZHD On KD.KDNr = ZHD.KDNr "
	'		sSql &= "Where KD.KDNr = @iKDNr And ZHD.RecNr = @iZHDNr"

	'	End If

	'	Return sSql
	'End Function

	'Function GetLstItems(ByVal lst As ListBox) As String
	'	Dim strBerufItems As String = String.Empty

	'	For i As Integer = 0 To lst.Items.Count - 1
	'		strBerufItems += lst.Items(i).ToString & "#@"
	'	Next

	'	Return Left(strBerufItems, Len(strBerufItems) - 2)
	'End Function

#End Region




End Class

Imports System.Text.RegularExpressions
Imports System.Data.SqlClient
Imports System.IO
Imports SPProgUtility.MainUtilities.Utilities

Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.UI
Imports SPS.Propose.SendMail.Utility.ClsDataDetail



Public Class ClsDivFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private m_xml As New ClsXML

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUI As UtilityUI
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


#Region "Constructor"

	Public Sub New()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData)

		m_utility = New SPProgUtility.MainUtilities.Utilities
		m_md = New Mandant
		m_UtilityUI = New UtilityUI

	End Sub

#End Region


#Region "Private Properties"

	ReadOnly Property GetSQLString4KontaktDb(ByVal b4KD As Boolean) As String
		Get
			Dim strSQL As String = String.Empty
			If b4KD Then
				strSQL = "Insert Into KD_KontaktTotal (KDNr, KDZNr, KontaktDate,Kontakte, RecNr, "
				strSQL &= "KontaktType1, KontaktType2, "
				strSQL &= "KontaktDauer, KontaktWichtig, KontaktErledigt, MANr, ProposeNr, VakNr, OfNr, Mail_ID, "
				strSQL &= "CreatedOn, CreatedFrom) Values "

				strSQL &= "(@KDNr, @KDZNr, @KontaktDate, @Kontakte, @RecNr, "
				strSQL &= "@KontaktType1, @KontaktType2, "
				strSQL &= "@KontaktDauer, @KontaktWichtig, @KontaktErledigt, @MANr, @ProposeNr, @VakNr, @OfNr, @Mail_ID, "
				strSQL &= "@Createdon, @CreatedFrom)"

			Else
				strSQL = "Insert Into MA_Kontakte (MANr, KontaktDate,Kontakte, RecNr, "
				strSQL &= "KontaktType1, KontaktType2, "
				strSQL &= "KontaktDauer, KontaktWichtig, KontaktErledigt, ProposeNr, VakNr, OfNr, Mail_ID, "
				strSQL &= "CreatedOn, CreatedFrom) Values "

				strSQL &= "(@MANr, @KontaktDate, @Kontakte, @RecNr, "
				strSQL &= "@KontaktType1, @KontaktType2, "
				strSQL &= "@KontaktDauer, @KontaktWichtig, @KontaktErledigt, @ProposeNr, @VakNr, @OfNr, @Mail_ID, "
				strSQL &= "@Createdon, @CreatedFrom)"

			End If

			Return strSQL
		End Get
	End Property


#End Region



	Public Function TranslateMailSubject(ByVal proposeNumber As Integer) As String
		Dim result As String = String.Empty

		Dim SQL As String = String.Empty
		SQL = "Select Top 1 P.Bezeichnung, P.MANr, P.KDNr, P.KDZHDNr, P.VakNr, "
		SQL &= "MA.Vorname, MA.Nachname, MA.Geschlecht, MAK.AnredeForm "
		SQL &= "From Propose P "
		SQL &= "Left Join Mitarbeiter MA On P.MANr = MA.MANr "
		SQL &= "LEFT JOIN dbo.MAKontakt_Komm MAK ON MAK.MANr = P.MANr "
		SQL &= "Where P.ProposeNr = @proposeNumber"

		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("proposeNumber", m_Utility.ReplaceMissing(proposeNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(ClsDataDetail.MDData.MDDbConn, SQL, listOfParams, CommandType.Text)

		Try
			' TODO: Subject 
			If (Not reader Is Nothing AndAlso reader.Read()) Then
				Dim gender = m_Translate.GetSafeTranslationValue(m_Utility.SafeGetString(reader, "AnredeForm"))
				Dim employeeLastname = m_Utility.SafeGetString(reader, "Nachname")
				Dim employeeFirstname = m_Utility.SafeGetString(reader, "Vorname")

				MAAnredeform = gender
				MANachname = employeeLastname
				MAVorname = employeeFirstname
				PBez = m_Utility.SafeGetString(reader, "Bezeichnung")

				Dim strMAName As String = String.Format("{0} {1} {2}", gender, employeeFirstname, employeeLastname)
				Dim subject As String = m_md.GetSelectedMDProfilValue(MDData.MDNr, Now.Year, "Templates", "proposemailsubject", String.Empty)

				If subject = String.Empty Then
					subject = m_Translate.GetSafeTranslationValue("Bewerbungsunterlagen von") & " {#P_MAAnredeForm} {#P_MANachname} {#P_MAVorname}"
				End If

				Dim regex As Regex = New Regex("\{#(\w+)\}", RegexOptions.Multiline)
				Dim matches As MatchCollection = Regex.Matches(subject)

				For Each match As Match In matches
					Dim pattern As String = match.Groups(0).Value
					Dim wildcard As String = match.Groups(1).Value

					Select Case wildcard.ToUpper
						Case "P_MAVorname".ToUpper
							subject = subject.Replace(pattern, MAVorname)
						Case "P_MANachname".ToUpper
							subject = subject.Replace(pattern, MANachname)
						Case "P_MAAnredeForm".ToUpper
							subject = subject.Replace(pattern, MAAnredeform)
						Case "P_Bez".ToUpper
							subject = subject.Replace(pattern, PBez)

					End Select

				Next
				If subject.Length > 78 Then
					Dim msg As String = "Die Text-Länge für Betreffzeile ist zu lang. Manche Mail-Empfänger Systeme können Ihre Nachricht nicht richtig anzeigen. Bitte reduzieren Sie die Länge auf 78 Zeichen."
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg))
				End If
				result = subject

			End If


		Catch ex As Exception
			result = Nothing
			m_Logger.LogError(ex.ToString())

		Finally

		End Try

		Return result

	End Function


#Region "Properties Vorschlagdaten..."


	Private Property PKDNr() As Integer
	Private Property PKDzNr() As Integer
	Private Property PMANr() As Integer
	Private Property PBez() As String
	Private Property PArbBegin() As String
	Private Property PESAls() As String
	Private Property PKDTarif() As String
	Private Property PBerater1() As String
	Private Property PBerater2() As String
	Private Property PBemerkung() As String
	Private Property PSpesen() As String
	Private Property PZusatz1() As String
	Private Property PZusatz2() As String
	Private Property PZusatz3() As String
	Private Property PZusatz4() As String
	Private Property PZusatz5() As String

	Private Property PCreatedOn() As Date
	Private Property PCreatedFrom() As String
	Private Property CustomerTransferGuid As String
	Private Property ZHDTransferGuid As String

#End Region


#Region "Properties US-MD Daten..."

	Public Property USMDname() As String
	Public Property USMDname2() As String
	Public Property USMDname3() As String
	Public Property USMDPostfach() As String
	Public Property USMDStrasse() As String
	Public Property USMDOrt() As String
	Public Property USMDPlz() As String
	Public Property USMDLand() As String
	Public Property USMDTelefon() As String
	Public Property USMDDTelefon As String
	Public Property USMDTelefax() As String
	Public Property USMDeMail() As String
	Public Property USMDHomepage() As String

#End Region


#Region "Properties US Private Daten..."

	Public Property USeMail() As String
	Public Property USHomepage() As String
	Public Property USNatel() As String
	Public Property USTelefon() As String
	Public Property USTelefax() As String
	Public Property USVorname() As String
	Public Property USAnrede() As String
	Public Property USNachname() As String
	Public Property USTitel_1() As String
	Public Property USTitel_2() As String
	Public Property USAbteilung() As String
	Public Property USStrasse() As String
	Public Property USPostfach() As String
	Public Property USPLZ() As String
	Public Property USOrt() As String
	Public Property USLand() As String
	Public Property USAbteil() As String

#End Region


#Region "Properties Kundendaten"

	Public Property Off_Firma1() As String
	Public Property KdzAnredeform() As String
	Public Property KdzAnrede() As String
	Public Property KdzNachname() As String
	Public Property KdzVorname() As String
	Public Property KdEmail() As String
	Public Property KdZEmail() As String

#End Region


#Region "Properties Kandidaten Massenversandfelder..."

	Public Property MARes1() As String
	Public Property MARes2() As String
	Public Property MARes3() As String
	Public Property MARes4() As String
	Public Property MARes5() As String
	Public Property MARes6() As String
	Public Property MARes7() As String
	Public Property MARes8() As String
	Public Property MARes9() As String
	Public Property MARes10() As String
	Public Property MARes11() As String
	Public Property MARes12() As String
	Public Property MARes13() As String
	Public Property MARes14() As String
	Public Property MARes15() As String
	Public Property MAFSchein As String
	Public Property MAAuto As String

#End Region


#Region "Properties Kandidatendaten..."

	Public Property MANachname() As String
	Public Property MAVorname() As String
	Public Property MAMail() As String
	Public Property MAGeschlecht() As String
	Public Property MAAnredeform() As String
	Public Property MABriefAnredeform As String
	Public Property MAGebDat() As String
	Public Property MAOrt() As String
	Public Property MAAlter() As String
	Public Property MABeruf() As String

	Public Property MANationality() As String
	Public Property MANationalityBez() As String
	Public Property MAStrasse() As String
	Public Property MAPLZ() As String
	Public Property MAGebort() As String
	Public Property MAZivilBez() As String
	Public Property MAZivil() As String

#End Region


#Region "Properties Sonstigedaten..."

	Public Property BodyAsHtml() As Boolean
	Public Property MailBetreff() As String
	Public Property Exchange_USName() As String
	Public Property Exchange_USPW() As String

	Public Property strUSSignFilename() As String

#End Region


	Function TranslateSeletedTemplate(ByVal rtfControl As DevExpress.XtraRichEdit.RichEditControl) As String
		Dim sSql As String = String.Empty
		Dim Conn As SqlConnection
		Dim strResult As String = String.Empty

		' Kundendaten suchen...
		sSql = "[Get All ProposeData 4 eMail From Propse]"
		Conn = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()
			Dim SQLDocCmd As SqlCommand = New SqlCommand(sSql, Conn)
			SQLDocCmd.CommandType = CommandType.StoredProcedure
			Dim param As SqlParameter
			param = SQLDocCmd.Parameters.AddWithValue("@PNr", ClsDataDetail.GetProposalNr)
			SQLDocCmd.Connection = Conn
			Dim rFoundedrec As SqlDataReader = SQLDocCmd.ExecuteReader

			While rFoundedrec.Read

				PMANr = m_utility.SafeGetInteger(rFoundedrec, "MANr", 0)
				PKDNr = m_utility.SafeGetInteger(rFoundedrec, "KDNr", 0)
				PKDzNr = m_utility.SafeGetInteger(rFoundedrec, "KDZHDNr", 0)

				Off_Firma1 = m_Utility.SafeGetString(rFoundedrec, "Firma1")
				KdzAnredeform = m_Utility.SafeGetString(rFoundedrec, "KDzAnredeForm")
				KdzNachname = m_utility.SafeGetString(rFoundedrec, "KDzNachname")
				KdzVorname = m_utility.SafeGetString(rFoundedrec, "KDzVorname", "")

				' Kandidatendaten
				MANachname = m_utility.SafeGetString(rFoundedrec, "MANachname", "")
				MAVorname = m_utility.SafeGetString(rFoundedrec, "MAVorname", "")
				MAGeschlecht = m_utility.SafeGetString(rFoundedrec, "MAGeschlecht", "")
				MAAnredeform = m_Utility.SafeGetString(rFoundedrec, "MAAnredeForm", "")
				MABriefAnredeform = m_Utility.SafeGetString(rFoundedrec, "MABriefAnredeForm", "")

				MAGebDat = m_Utility.SafeGetString(rFoundedrec, "MAGebDat", "")
				MAOrt = m_utility.SafeGetString(rFoundedrec, "MAOrt", "")
				MAAlter = m_utility.SafeGetString(rFoundedrec, "MAAlter", "")
				MABeruf = m_utility.SafeGetString(rFoundedrec, "MABeruf", "")

				MANationality = m_utility.SafeGetString(rFoundedrec, "MANationality")
				MANationalityBez = m_utility.SafeGetString(rFoundedrec, "MANationalityBez")
				MAStrasse = m_utility.SafeGetString(rFoundedrec, "MAStrasse")
				MAPLZ = m_utility.SafeGetString(rFoundedrec, "MAPLZ")
				MAGebort = m_utility.SafeGetString(rFoundedrec, "MAGebort")
				MAZivilBez = m_utility.SafeGetString(rFoundedrec, "MAZivilBez")
				MAZivil = m_utility.SafeGetString(rFoundedrec, "MAZivil")


				MARes1 = m_utility.SafeGetString(rFoundedrec, "LLRes1", "")
				MARes2 = m_utility.SafeGetString(rFoundedrec, "LLRes2", "")
				MARes3 = m_utility.SafeGetString(rFoundedrec, "LLRes3", "")
				MARes4 = m_utility.SafeGetString(rFoundedrec, "LLRes4", "")
				MARes5 = m_utility.SafeGetString(rFoundedrec, "LLRes5", "")
				MARes6 = m_utility.SafeGetString(rFoundedrec, "LLRes6", "")
				MARes7 = m_utility.SafeGetString(rFoundedrec, "LLRes7", "")
				MARes8 = m_utility.SafeGetString(rFoundedrec, "LLRes8", "")
				MARes9 = m_utility.SafeGetString(rFoundedrec, "LLRes9", "")
				MARes10 = m_utility.SafeGetString(rFoundedrec, "LLRes10", "")
				MARes11 = m_utility.SafeGetString(rFoundedrec, "LLRes11", "")
				MARes12 = m_utility.SafeGetString(rFoundedrec, "LLRes12", "")
				MARes13 = m_utility.SafeGetString(rFoundedrec, "LLRes13", "")
				MARes14 = m_utility.SafeGetString(rFoundedrec, "LLRes14", "")
				MARes15 = m_utility.SafeGetString(rFoundedrec, "LLRes15", "")

				' mobilität
				MAFSchein = m_Utility.SafeGetString(rFoundedrec, "MAFSchein", "")
				MAAuto = m_Utility.SafeGetString(rFoundedrec, "MAFahrzeug", "")

				' Vorschlagdaten
				PBez = m_utility.SafeGetString(rFoundedrec, "Bezeichnung", "")
				PArbBegin = m_utility.SafeGetString(rFoundedrec, "P_Arbbegin", "")
				PESAls = m_utility.SafeGetString(rFoundedrec, "MA_ESAls", "")
				PKDTarif = m_utility.SafeGetString(rFoundedrec, "KD_Tarif", "")
				Dim aValue As String() = m_utility.SafeGetString(rFoundedrec, "Berater", "").ToString.Split(CChar("/"))
				PBerater1 = aValue(0).ToString
				PBerater2 = If(aValue.Length > 1, aValue(1).ToString, String.Empty)

				PZusatz1 = m_utility.SafeGetString(rFoundedrec, "_P_Zusatz1", "")
				PZusatz2 = m_utility.SafeGetString(rFoundedrec, "_P_Zusatz2", "")
				PZusatz3 = m_utility.SafeGetString(rFoundedrec, "_P_Zusatz3", "")
				PZusatz4 = m_utility.SafeGetString(rFoundedrec, "_P_Zusatz4", "")
				PZusatz5 = m_utility.SafeGetString(rFoundedrec, "_P_Zusatz5", "")

				PCreatedOn = m_utility.SafeGetDateTime(rFoundedrec, "createdon", Nothing)
				PCreatedFrom = m_utility.SafeGetString(rFoundedrec, "CreatedFrom")
				CustomerTransferGuid = m_Utility.SafeGetString(rFoundedrec, "KD_Transfered_Guid")
				ZHDTransferGuid = m_Utility.SafeGetString(rFoundedrec, "KDz_Transfered_Guid")

				' Mandantendaten
				USMDname = m_utility.SafeGetString(rFoundedrec, "USPMDName1", "")
				USMDname2 = m_utility.SafeGetString(rFoundedrec, "USPMDName2", "")
				USMDname3 = m_utility.SafeGetString(rFoundedrec, "USPMDName3", "")
				USMDPostfach = m_utility.SafeGetString(rFoundedrec, "USPPostfach", "")
				USMDStrasse = m_utility.SafeGetString(rFoundedrec, "USPStrasse", "")
				USMDLand = m_utility.SafeGetString(rFoundedrec, "USPLand", "")
				USMDPlz = m_utility.SafeGetString(rFoundedrec, "USPPLZ", "")
				USMDOrt = m_utility.SafeGetString(rFoundedrec, "USPOrt", "")
				USMDTelefon = m_utility.SafeGetString(rFoundedrec, "USPTelefon", "")
				USMDdTelefon = m_Utility.SafeGetString(rFoundedrec, "USPDTelefon", "")
				USMDTelefax = m_Utility.SafeGetString(rFoundedrec, "USPTelefax", "")
				USMDeMail = m_utility.SafeGetString(rFoundedrec, "USPeMail", "")
				USMDHomepage = m_utility.SafeGetString(rFoundedrec, "USPHomepage", "")

				' Private Daten
				USNachname = m_utility.SafeGetString(rFoundedrec, "USPPNachname", "")
				USVorname = m_utility.SafeGetString(rFoundedrec, "USPPVorname", "")
				USTitel_1 = m_utility.SafeGetString(rFoundedrec, "USPPTitel_1", "")
				USTitel_2 = m_utility.SafeGetString(rFoundedrec, "USPPTitel_2", "")
				USAbteilung = m_utility.SafeGetString(rFoundedrec, "USPPAbteilung", "")

				USPostfach = m_utility.SafeGetString(rFoundedrec, "USPPPostfach", "")
				USStrasse = m_utility.SafeGetString(rFoundedrec, "USPPStrasse", "")
				USLand = m_utility.SafeGetString(rFoundedrec, "USPPLand", "")
				USPLZ = m_utility.SafeGetString(rFoundedrec, "USPPPLZ", "")
				USOrt = m_utility.SafeGetString(rFoundedrec, "USPPOrt", "")
				USNatel = m_utility.SafeGetString(rFoundedrec, "USPPNatel", "")
				USTelefon = m_utility.SafeGetString(rFoundedrec, "USPPTelefon", "")
				USTelefax = m_utility.SafeGetString(rFoundedrec, "USPPTelefax", "")
				USeMail = m_utility.SafeGetString(rFoundedrec, "USPPeMail", "")
				USHomepage = m_utility.SafeGetString(rFoundedrec, "USPPHomepage", "")

			End While
			ParseTemplateFile(rtfControl)
			strResult = rtfControl.RtfText


		Catch ex As Exception
			MsgBox(String.Format("***Fehler (TranslateSeletedTemplate): {0}", ex.Message))

		Finally
			Conn.Close()

		End Try

		Return strResult
	End Function


#Region "Übersetzen der Variable..."


	Function ParseTemplateFile(ByVal FullFileName As DevExpress.XtraRichEdit.RichEditControl) As String
		Dim ParsedFile As String = FullFileName.RtfText + vbCrLf
		Dim pattern As String = String.Empty

		Const REGEX_MailBetreff As String = "\{(?i)TMPL_VAR name=\'MailBetreff\'\}"

		Const REGEX_KDFirma1 As String = "\{(?i)TMPL_VAR name=\'KDFirma1\'\}"
		Const REGEX_KDzANREDEFORM As String = "\{(?i)TMPL_VAR name=\'KDzAnredeform\'\}"
		Const REGEX_GANZE_ANREDE As String = "\{(?i)TMPL_VAR name=\'KDzFullAnredeform\'\}"
		Const REGEX_KDzANREDE As String = "\{(?i)TMPL_VAR name=\'KDzAnrede\'\}"
		Const REGEX_KDzVorname As String = "\{(?i)TMPL_VAR name=\'KDzVorname\'\}"
		Const REGEX_KDzNachname As String = "\{(?i)TMPL_VAR name=\'KDzNachname\'\}"

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

		Const REGEX_PNr As String = "\{(?i)TMPL_VAR name=\'P_Nr\'\}"
		Const REGEX_PMANr As String = "\{(?i)TMPL_VAR name=\'P_MANr\'\}"
		Const REGEX_PKDNr As String = "\{(?i)TMPL_VAR name=\'P_KDNr\'\}"
		Const REGEX_PKDZNr As String = "\{(?i)TMPL_VAR name=\'P_KDzNr\'\}"

		Const REGEX_PBez As String = "\{(?i)TMPL_VAR name=\'P_Bez\'\}"
		Const REGEX_PArbbegin As String = "\{(?i)TMPL_VAR name=\'P_ArbBegin\'\}"
		Const REGEX_PESAls As String = "\{(?i)TMPL_VAR name=\'P_ESAls\'\}"
		Const REGEX_PKDTarif As String = "\{(?i)TMPL_VAR name=\'P_KD_Tarif\'\}"
		Const REGEX_PBerater1 As String = "\{(?i)TMPL_VAR name=\'P_Berater1\'\}"
		Const REGEX_PBerater2 As String = "\{(?i)TMPL_VAR name=\'P_Berater2\'\}"

		Const REGEX_PZusatz1 As String = "\{(?i)TMPL_VAR name=\'P_Zusatz1\'\}"
		Const REGEX_PZusatz2 As String = "\{(?i)TMPL_VAR name=\'P_Zusatz2\'\}"
		Const REGEX_PZusatz3 As String = "\{(?i)TMPL_VAR name=\'P_Zusatz3\'\}"
		Const REGEX_PZusatz4 As String = "\{(?i)TMPL_VAR name=\'P_Zusatz4\'\}"
		Const REGEX_PZusatz5 As String = "\{(?i)TMPL_VAR name=\'P_Zusatz5\'\}"
		Const REGEX_PBemerkung As String = "\{(?i)TMPL_VAR name=\'P_Bemerkung\'\}"
		Const REGEX_PSpesen As String = "\{(?i)TMPL_VAR name=\'P_Spesen\'\}"

		Const REGEX_MARes1 As String = "\{(?i)TMPL_VAR name=\'MA_Res1\'\}"
		Const REGEX_MARes2 As String = "\{(?i)TMPL_VAR name=\'MA_Res2\'\}"
		Const REGEX_MARes3 As String = "\{(?i)TMPL_VAR name=\'MA_Res3\'\}"
		Const REGEX_MARes4 As String = "\{(?i)TMPL_VAR name=\'MA_Res4\'\}"
		Const REGEX_MARes5 As String = "\{(?i)TMPL_VAR name=\'MA_Res5\'\}"
		Const REGEX_MARes6 As String = "\{(?i)TMPL_VAR name=\'MA_Res6\'\}"
		Const REGEX_MARes7 As String = "\{(?i)TMPL_VAR name=\'MA_Res7\'\}"
		Const REGEX_MARes8 As String = "\{(?i)TMPL_VAR name=\'MA_Res8\'\}"
		Const REGEX_MARes9 As String = "\{(?i)TMPL_VAR name=\'MA_Res9\'\}"
		Const REGEX_MARes10 As String = "\{(?i)TMPL_VAR name=\'MA_Res10\'\}"
		Const REGEX_MARes11 As String = "\{(?i)TMPL_VAR name=\'MA_Res11\'\}"
		Const REGEX_MARes12 As String = "\{(?i)TMPL_VAR name=\'MA_Res12\'\}"
		Const REGEX_MARes13 As String = "\{(?i)TMPL_VAR name=\'MA_Res13\'\}"
		Const REGEX_MARes14 As String = "\{(?i)TMPL_VAR name=\'MA_Res14\'\}"
		Const REGEX_MARes15 As String = "\{(?i)TMPL_VAR name=\'MA_Res15\'\}"

		Const REGEX_MAFSchein As String = "\{(?i)TMPL_VAR name=\'MA_FSchein\'\}"
		Const REGEX_MAAuto As String = "\{(?i)TMPL_VAR name=\'MA_Auto\'\}"

		Const REGEX_MA_Nachname As String = "\{(?i)TMPL_VAR name=\'P_MANachname\'\}"
		Const REGEX_MA_Vorname As String = "\{(?i)TMPL_VAR name=\'P_MAVorname\'\}"

		Const REGEX_MA_Geschlecht As String = "\{(?i)TMPL_VAR name=\'P_MAGeschlecht\'\}"
		Const REGEX_MA_AnredeForm As String = "\{(?i)TMPL_VAR name=\'P_MAAnredeForm\'\}"
		Const REGEX_MA_BriefAnredeForm As String = "\{(?i)TMPL_VAR name=\'P_MABriefAnredeForm\'\}"
		Const REGEX_MA_Anrede As String = "\{(?i)TMPL_VAR name=\'P_MAAnrede\'\}"
		Const REGEX_MA_GebDat As String = "\{(?i)TMPL_VAR name=\'P_MAGebDat\'\}"
		Const REGEX_MA_Ort As String = "\{(?i)TMPL_VAR name=\'P_MAOrt\'\}"
		Const REGEX_MA_Alter As String = "\{(?i)TMPL_VAR name=\'P_MAAlter\'\}"
		Const REGEX_MA_Beruf As String = "\{(?i)TMPL_VAR name=\'P_MABeruf\'\}"

		Const REGEX_MA_Nationality As String = "\{(?i)TMPL_VAR name=\'P_MANationality\'\}"
		Const REGEX_MA_NationalityBez As String = "\{(?i)TMPL_VAR name=\'P_MANationalityBez\'\}"
		Const REGEX_MA_Strasse As String = "\{(?i)TMPL_VAR name=\'P_MAStrasse\'\}"
		Const REGEX_MA_PLZ As String = "\{(?i)TMPL_VAR name=\'P_MAPLZ\'\}"
		Const REGEX_MA_Gebort As String = "\{(?i)TMPL_VAR name=\'P_MAGebort\'\}"
		Const REGEX_MA_ZivilBez As String = "\{(?i)TMPL_VAR name=\'P_MAZivilBez\'\}"
		Const REGEX_MA_Zivil As String = "\{(?i)TMPL_VAR name=\'P_MAZivil\'\}"

		Const REGEX_PCreatedOn As String = "\{(?i)TMPL_VAR name=\'P_CreatedOn\'\}"
		Const REGEX_PCreatedFrom As String = "\{(?i)TMPL_VAR name=\'P_CreatedFrom\'\}"
		Const REGEX_ProposeAttachmentLink As String = "\{(?i)TMPL_VAR name=\'ProposeattachmentLink\'\}"

		Dim tplWithZHDData As Boolean = False
		Try
			'// search templatevars
			pattern = REGEX_KDzANREDEFORM
			Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(pattern)

			'// replace vars
			Dim myRegEx_0 As Regex = New Regex(REGEX_MailBetreff)
			Dim result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MailBetreff)
			End While

			myRegEx_0 = New Regex(REGEX_KDFirma1)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(Off_Firma1)
			End While

			myRegEx_0 = New Regex(REGEX_KDzANREDEFORM)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				tplWithZHDData = True
				If PKDzNr > 0 Then result_0.Replace(_KdzAnredeform)
			End While

			myRegEx_0 = New Regex(REGEX_KDzANREDE)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				tplWithZHDData = True
				If PKDzNr > 0 Then result_0.Replace(_KdzAnrede)
			End While

			myRegEx_0 = New Regex(REGEX_KDzNachname)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				tplWithZHDData = True
				If PKDzNr > 0 Then result_0.Replace(KdzNachname)
			End While

			myRegEx_0 = New Regex(REGEX_KDzVorname)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				tplWithZHDData = True
				result_0.Replace(KdzVorname)
			End While

			myRegEx_0 = New Regex(REGEX_GANZE_ANREDE)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			If result_0.FindNext() Then
				tplWithZHDData = True
				Dim strValue As String = String.Format("{0} {1}", _KdzAnredeform, KdzNachname)
				If strValue.Contains(m_xml.GetSafeTranslationValue("liebe")) Then
					strValue = String.Format("{0} {1}", _KdzAnredeform, KdzVorname)

				ElseIf strValue.Contains(KdzNachname) Or strValue.Contains(KdzVorname) Then
					strValue = String.Format("{0}", KdzAnredeform)

				End If
				If PKDzNr > 0 Then result_0.Replace(strValue)
			End If


			' Kandidatendaten ...............................................................................................
			myRegEx_0 = New Regex(REGEX_MA_Nachname)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MANachname)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Vorname)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MAVorname)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Geschlecht)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MAGeschlecht)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Anrede)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(m_xml.GetSafeTranslationValue(If(_MAGeschlecht = "M", "Herr", "Frau")))
			End While

			myRegEx_0 = New Regex(REGEX_MA_AnredeForm)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MAAnredeform)
			End While

			myRegEx_0 = New Regex(REGEX_MA_BriefAnredeForm)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MABriefAnredeform)
			End While

			myRegEx_0 = New Regex(REGEX_MA_GebDat)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MAGebDat)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Ort)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MAOrt)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Alter)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MAAlter)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Beruf)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_MABeruf)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Nationality)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MANationality)
			End While

			myRegEx_0 = New Regex(REGEX_MA_NationalityBez)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MANationalityBez)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Strasse)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MAStrasse)
			End While

			myRegEx_0 = New Regex(REGEX_MA_PLZ)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MAPLZ)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Gebort)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MAGebort)
			End While

			myRegEx_0 = New Regex(REGEX_MA_ZivilBez)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MAZivilBez)
			End While

			myRegEx_0 = New Regex(REGEX_MA_Zivil)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(MAZivil)
			End While


			' Massenversandfelder in der Kandidatenverwaltung ................................................................
			myRegEx_0 = New Regex(REGEX_MARes1)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes1)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes1)
			End While
			myRegEx_0 = New Regex(REGEX_MARes2)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes2)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes2)
			End While
			myRegEx_0 = New Regex(REGEX_MARes3)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes3)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes3)
			End While
			myRegEx_0 = New Regex(REGEX_MARes4)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes4)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes4)
			End While
			myRegEx_0 = New Regex(REGEX_MARes5)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes5)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes5)
			End While
			myRegEx_0 = New Regex(REGEX_MARes6)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes6)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes6)
			End While
			myRegEx_0 = New Regex(REGEX_MARes7)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes7)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes7)
			End While
			myRegEx_0 = New Regex(REGEX_MARes8)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes8)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes8)
			End While
			myRegEx_0 = New Regex(REGEX_MARes9)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes9)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes9)
			End While
			myRegEx_0 = New Regex(REGEX_MARes10)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes10)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes10)
			End While
			myRegEx_0 = New Regex(REGEX_MARes11)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes11)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes11)
			End While
			myRegEx_0 = New Regex(REGEX_MARes12)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes12)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes12)
			End While
			myRegEx_0 = New Regex(REGEX_MARes13)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes13)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes13)
			End While
			myRegEx_0 = New Regex(REGEX_MARes14)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_MARes14)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes14)
			End While
			myRegEx_0 = New Regex(REGEX_MARes15)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _MARes15)
			End While


			' Vorschlagdaten ...............................................................................................
			myRegEx_0 = New Regex(REGEX_PNr)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(ClsDataDetail.GetProposalNr)
			End While
			myRegEx_0 = New Regex(REGEX_PKDNr)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(ClsDataDetail.GetProposalKDNr)
			End While
			myRegEx_0 = New Regex(REGEX_PKDZNr)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				If ClsDataDetail.GetProposalZHDNr.HasValue Then
					result_0.Replace(ClsDataDetail.GetProposalZHDNr)
				Else
					result_0.Replace("???")
				End If

			End While
			myRegEx_0 = New Regex(REGEX_PMANr)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(ClsDataDetail.GetProposalMANr)
			End While

			myRegEx_0 = New Regex(REGEX_PBez)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PBez)
			End While

			myRegEx_0 = New Regex(REGEX_PArbbegin)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PArbBegin)
			End While

			myRegEx_0 = New Regex(REGEX_PESAls)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PESAls)
			End While

			myRegEx_0 = New Regex(REGEX_PKDTarif)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PKDTarif)
			End While
			myRegEx_0 = New Regex(REGEX_PBerater1)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PBerater1)
			End While
			myRegEx_0 = New Regex(REGEX_PBerater2)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PBerater2)
			End While
			myRegEx_0 = New Regex(REGEX_PBemerkung)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PBemerkung)
			End While
			myRegEx_0 = New Regex(REGEX_PSpesen)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PSpesen)
			End While


			myRegEx_0 = New Regex(REGEX_PZusatz1)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_PZusatz1)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _PZusatz1)
			End While
			myRegEx_0 = New Regex(REGEX_PZusatz2)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_PZusatz2)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _PZusatz2)
			End While

			myRegEx_0 = New Regex(REGEX_PZusatz3)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_PZusatz3)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _PZusatz3)
			End While

			myRegEx_0 = New Regex(REGEX_PZusatz4)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _PZusatz4)
			End While

			myRegEx_0 = New Regex(REGEX_PZusatz5)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Empty) '_PZusatz5)
				FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _PZusatz5)
			End While

			myRegEx_0 = New Regex(REGEX_MAFSchein)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(If(String.IsNullOrWhiteSpace(MAFSchein) OrElse MAFSchein.Trim = "-", "Nein", "Ja"))
				'FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, If(String.IsNullOrWhiteSpace(MAFSchein) OrElse MAFSchein.Trim = "-", "Nein", "Ja"))
			End While
			myRegEx_0 = New Regex(REGEX_MAAuto)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(If(String.IsNullOrWhiteSpace(MAAuto) OrElse MAAuto = "N", "Nein", "Ja"))
				'FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, If(String.IsNullOrWhiteSpace(MAAuto) OrElse MAAuto = "N", "Nein", "Ja"))
			End While


			' Benutzerdaten ...............................................................................................
			myRegEx_0 = New Regex(REGEX_USAnrede)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USAnrede)
			End While

			myRegEx_0 = New Regex(REGEX_USNachname)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USNachname)
			End While

			myRegEx_0 = New Regex(REGEX_USVorname)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USVorname)
			End While

			myRegEx_0 = New Regex(REGEX_USTelefon)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USTelefon)
			End While

			myRegEx_0 = New Regex(REGEX_USNatel)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(USNatel)
			End While

			myRegEx_0 = New Regex(REGEX_USTelefax)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USTelefax)
			End While

			myRegEx_0 = New Regex(REGEX_USeMail)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USeMail)
			End While

			myRegEx_0 = New Regex(REGEX_USPostfach)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USPostfach)
			End While

			myRegEx_0 = New Regex(REGEX_USStrasse)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USStrasse)
			End While

			myRegEx_0 = New Text.RegularExpressions.Regex(REGEX_USPLZ)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USPLZ)
			End While

			myRegEx_0 = New Regex(REGEX_USOrt)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USOrt)
			End While

			myRegEx_0 = New Regex(REGEX_USLand)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USLand)
			End While

			myRegEx_0 = New Regex(REGEX_USAbteilung)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USAbteil)
			End While

			myRegEx_0 = New Regex(REGEX_USMDName)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDname)
			End While

			myRegEx_0 = New Regex(REGEX_USMDName2)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDname2)
			End While

			myRegEx_0 = New Regex(REGEX_USMDPostfach)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDPostfach)
			End While

			myRegEx_0 = New Regex(REGEX_USMDStrasse)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDStrasse)
			End While

			myRegEx_0 = New Regex(REGEX_USMDOrt)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDOrt)
			End While

			myRegEx_0 = New Regex(REGEX_USMDPlz)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDPlz)
			End While

			myRegEx_0 = New Regex(REGEX_USMDLand)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDLand)
			End While

			myRegEx_0 = New Regex(REGEX_USMDTelefon)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDTelefon)
			End While

			myRegEx_0 = New Regex(REGEX_USMDDTelefon)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDDTelefon)
			End While

			myRegEx_0 = New Regex(REGEX_USMDTelefax)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDTelefax)
			End While

			myRegEx_0 = New Regex(REGEX_USMDeMail)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDeMail)
			End While

			myRegEx_0 = New Regex(REGEX_USMDHomepage)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USMDHomepage)
			End While

			myRegEx_0 = New Regex(REGEX_USTitel_1)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USTitel_1)
			End While

			myRegEx_0 = New Regex(REGEX_USTitel_2)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_USTitel_2)
			End While

			myRegEx_0 = New Regex(REGEX_PCreatedOn)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Format("{0:d}", _PCreatedOn))
				'FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _Pcreatedon)
			End While

			myRegEx_0 = New Regex(REGEX_PCreatedFrom)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(_PCreatedFrom)
				'FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _Pcreatedfrom)
			End While

			myRegEx_0 = New Regex(REGEX_ProposeAttachmentLink)
			result_0 = FullFileName.Document.StartSearch(myRegEx_0)
			While result_0.FindNext()
				result_0.Replace(String.Format("{0}", ZHDTransferGuid))
			End While

			If tplWithZHDData AndAlso PKDzNr = 0 Then
				m_UtilityUI.ShowOKDialog(String.Format(m_Translate.GetSafeTranslationValue("Achtung: Sie haben keine zuständige Person ausgewählt. Möglicherweise werden gewisse Formeln nicht korrekt übersetzt.{0}Bitte kontrollieren Sie sorgfälltig Ihre Vorlage auf Inhalte."), vbNewLine),
										 m_Translate.GetSafeTranslationValue("Fehlende zuständige Person"), System.Windows.Forms.MessageBoxIcon.Warning)

			End If


			If ClsDataDetail.bBodyAsHtml Then ParsedFile = SetSyntax(ParsedFile)


		Catch ex As Exception
			'      ParsedFile = String.Empty
			'      MsgBox("Feher: " & ex.Message.Trim & vbCrLf & ParsedFile & vbCrLf & pattern & vbCrLf & line)

		End Try

		Return ParsedFile
	End Function

	Function SetSyntax(ByVal str1 As String) As String
		Console.WriteLine("Str1/1: " & str1)

		str1 = Replace(str1, vbCrLf, "<br />")
		str1 = Replace(str1, vbNewLine, "<br />")
		str1 = Replace(str1, "ä", "&auml;")
		str1 = Replace(str1, "ö", "&ouml;")
		str1 = Replace(str1, "ü", "&uuml;")
		str1 = Replace(str1, "Ä", "&Auml;")
		str1 = Replace(str1, "Ö", "&Ouml;")
		str1 = Replace(str1, "Ü", "&Uuml;")
		str1 = Replace(str1, "ß", "&szlig;")
		str1 = Replace(str1, "§", "&sect;")

		str1 = Replace(str1, "€", "&euro;")

		'str1 = Replace(str1, Chr(228), "&auml;")      ' ä
		str1 = Replace(str1, Chr(252), "&uuml;")			' ü
		str1 = Replace(str1, Chr(129), "&uuml;")			' ü

		SetSyntax = str1

		Console.WriteLine("Str1/2: " & str1)

	End Function



#End Region

	Sub GetUSData()	'ByVal myRegx As ClsDivFunc)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strUSKst As String = String.Empty
		Dim strUSNachname As String = String.Empty
		Dim iUSNr As Integer = 0
		Dim Conn As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Conn.Open()

		Dim sSql As String = "[Get USData 4 Templates]"
		Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
		cmd.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@USNr", _ClsProgSetting.GetLogedUSNr)

		Dim rTemprec As SqlDataReader = cmd.ExecuteReader					 ' Benutzerdatenbank

		Try
			rTemprec.Read()
			Me.USAnrede = rTemprec("USAnrede").ToString
			Me.USeMail = rTemprec("USeMail").ToString
			Me.USNachname = rTemprec("USNachname").ToString
			Me.USVorname = rTemprec("USVorname").ToString
			Me.USTelefon = rTemprec("USTelefon").ToString
			Me.USTelefax = rTemprec("USTelefax").ToString

			Me.USTitel_1 = rTemprec("USTitel_1").ToString
			Me.USTitel_2 = rTemprec("USTitel_2").ToString

			Me.USAbteilung = rTemprec("USAbteilung").ToString
			Me.USPostfach = rTemprec("USPostfach").ToString
			Me.USStrasse = rTemprec("USStrasse").ToString
			Me.USPLZ = rTemprec("USPLZ").ToString
			Me.USLand = rTemprec("USLand").ToString
			Me.USOrt = rTemprec("USOrt").ToString

			Me.Exchange_USName = rTemprec("EMail_UserName").ToString
			Me.Exchange_USPW = rTemprec("EMail_UserPW").ToString
			m_Logger.LogInfo(String.Format("{0}.{1} | {2}", strMethodeName, Me.Exchange_USName, Me.Exchange_USPW))

			Me.USMDname = rTemprec("MDName").ToString
			Me.USMDname2 = rTemprec("MDName2").ToString
			Me.USMDname3 = rTemprec("MDName3").ToString
			Me.USMDPostfach = rTemprec("MDPostfach").ToString
			Me.USMDStrasse = rTemprec("MDStrasse").ToString
			Me.USMDPlz = rTemprec("MDPLZ").ToString
			Me.USMDOrt = rTemprec("MDOrt").ToString
			Me.USMDLand = rTemprec("MDLand").ToString

			Me.USMDTelefon = rTemprec("MDTelefon").ToString
			Me.USMDDTelefon = rTemprec("MDDTelefon").ToString
			Me.USMDTelefax = rTemprec("MDTelefax").ToString
			Me.USMDeMail = rTemprec("MDeMail").ToString
			Me.USMDHomepage = rTemprec("MDHomepage").ToString

			Me.strUSSignFilename = GetUSSign(_ClsProgSetting.GetLogedUSNr)

			rTemprec.Close()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			rTemprec.Close()
			Conn.Close()

		End Try

	End Sub

	Function GetUSSign(ByVal lUSNr As Integer) As String
		Dim Conn As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte()
		Dim sUSSql As String = "Select USSign, USNr From Benutzer US Where "
		sUSSql &= String.Format("USNr = {0} And USSign Is Not Null", lUSNr)

		Dim i As Integer = 0

		Conn.Open()
		Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

		Try

			strFullFilename = String.Format("{0}Bild_{1}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
																			 System.Guid.NewGuid.ToString())

			Try
				Try
					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
					If BA Is Nothing Then Return String.Empty

				Catch ex As Exception
					Return String.Empty

				End Try

				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()
				ClsDataDetail.strUSSignFilename = strFullFilename

				i += 1


			Catch ex As Exception
				'_ClsLog.WriteToEventLog(String.Format("***GetUSSign: {0}", ex.Message))
				MsgBox(String.Format(m_xml.GetSafeTranslationValue("Fehler: {0}"), ex.Message), MsgBoxStyle.Critical, "GetUSSign")


			End Try


		Catch ex As Exception
			'_ClsLog.WriteToEventLog(String.Format("***GetUSSign: {0}", ex.Message))

		End Try

		Return strFullFilename
	End Function



End Class


Imports System.Text.RegularExpressions
Imports System.IO

Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SP.DatabaseAccess.Propose.DataObjects
Imports SP.DatabaseAccess.Listing.DataObjects


Public Class TemplateFieldParser


#Region "private consts"

	Const REGEX_Off_Nr As String = "\{(?i)TMPL_VAR name=\'OffNr\'\}"
	Const REGEX_MailBetreff As String = "\{(?i)TMPL_VAR name=\'MailBetreff\'\}"
	Const REGEX_KDFirma1 As String = "\{(?i)TMPL_VAR name=\'KDFirma1\'\}"
	Const REGEX_KDEMail As String = "\{(?i)TMPL_VAR name=\'KDEMail\'\}"

	Const REGEX_ANREDE As String = "\{(?i)TMPL_VAR name=\'Anredeform\'\}"
	Const REGEX_ANREDE_0 As String = "\{(?i)TMPL_VAR name=\'KDzAnrede\'\}"
	Const REGEX_KDzVorname As String = "\{(?i)TMPL_VAR name=\'KDzVorname\'\}"
	Const REGEX_KDzNachname As String = "\{(?i)TMPL_VAR name=\'KDzNachname\'\}"
	Const REGEX_KDzEMail As String = "\{(?i)TMPL_VAR name=\'KDzEMail\'\}"

	Const REGEX_KD_Guid As String = "\{(?i)TMPL_VAR name=\'KD_Guid\'\}"
	Const REGEX_KD_DocLink As String = "\{(?i)TMPL_VAR name=\'KDDocLink\'\}"
	Const REGEX_ZHD_Guid As String = "\{(?i)TMPL_VAR name=\'ZHd_Guid\'\}"
	Const REGEX_ZHD_DocLink As String = "\{(?i)TMPL_VAR name=\'ZHDDocLink\'\}"


	Const REGEX_Off_USNr As String = "\{(?i)TMPL_VAR name=\'Off_USNr\'\}"
	Const REGEX_USAnrede As String = "\{(?i)TMPL_VAR name=\'USAnrede\'\}"
	Const REGEX_USNachname As String = "\{(?i)TMPL_VAR name=\'USNachname\'\}"
	Const REGEX_USVorname As String = "\{(?i)TMPL_VAR name=\'USVorname\'\}"
	Const REGEX_USTelefon As String = "\{(?i)TMPL_VAR name=\'USTelefon\'\}"
	Const REGEX_USTelefax As String = "\{(?i)TMPL_VAR name=\'USTelefax\'\}"
	Const REGEX_USNatel As String = "\{(?i)TMPL_VAR name=\'USNatel\'\}"
	Const REGEX_USeMail As String = "\{(?i)TMPL_VAR name=\'USeMail\'\}"

	Const REGEX_USMDName As String = "\{(?i)TMPL_VAR name=\'USMDName\'\}"
	Const REGEX_USMDName2 As String = "\{(?i)TMPL_VAR name=\'USMDName2\'\}"
	Const REGEX_USMDName3 As String = "\{(?i)TMPL_VAR name=\'USMDName3\'\}"
	Const REGEX_USMDPostfach As String = "\{(?i)TMPL_VAR name=\'USMDPostfach\'\}"
	Const REGEX_USMDStrasse As String = "\{(?i)TMPL_VAR name=\'USMDStrasse\'\}"
	Const REGEX_USMDOrt As String = "\{(?i)TMPL_VAR name=\'USMDort\'\}"
	Const REGEX_USMDPlz As String = "\{(?i)TMPL_VAR name=\'USMDPlz\'\}"
	Const REGEX_USMDLand As String = "\{(?i)TMPL_VAR name=\'USMDLand\'\}"

	Const REGEX_USMDDTelefon As String = "\{(?i)TMPL_VAR name=\'USMDDTelefon\'\}"
	Const REGEX_USMDTelefon As String = "\{(?i)TMPL_VAR name=\'USMDTelefon\'\}"
	Const REGEX_USMDTelefax As String = "\{(?i)TMPL_VAR name=\'USMDTelefax\'\}"
	Const REGEX_USMDeMail As String = "\{(?i)TMPL_VAR name=\'USMDeMail\'\}"
	Const REGEX_USMDHomepage As String = "\{(?i)TMPL_VAR name=\'USMDHomepage\'\}"

	Const REGEX_USTitel_1 As String = "\{(?i)TMPL_VAR name=\'USTitel_1\'\}"
	Const REGEX_USTitel_2 As String = "\{(?i)TMPL_VAR name=\'USTitel_2\'\}"

	Const REGEX_USPostfach As String = "\{(?i)TMPL_VAR name=\'USPostfach\'\}"
	Const REGEX_USStrasse As String = "\{(?i)TMPL_VAR name=\'USStrasse\'\}"

	Const REGEX_USPLZ As String = "\{(?i)TMPL_VAR name=\'USPLZ\'\}"
	Const REGEX_USOrt As String = "\{(?i)TMPL_VAR name=\'USOrt\'\}"
	Const REGEX_USLand As String = "\{(?i)TMPL_VAR name=\'USLand\'\}"
	Const REGEX_USAbteilung As String = "\{(?i)TMPL_VAR name=\'USAbteilung\'\}"

	Const REGEX_OffRes1 As String = "\{(?i)TMPL_VAR name=\'OfferRes1\'\}"
	Const REGEX_OffRes2 As String = "\{(?i)TMPL_VAR name=\'OfferRes2\'\}"
	Const REGEX_OffRes3 As String = "\{(?i)TMPL_VAR name=\'OfferRes3\'\}"
	Const REGEX_OffRes4 As String = "\{(?i)TMPL_VAR name=\'OfferRes4\'\}"
	Const REGEX_OffRes5 As String = "\{(?i)TMPL_VAR name=\'OfferRes5\'\}"
	Const REGEX_OffRes6 As String = "\{(?i)TMPL_VAR name=\'OfferSchlusstext\'\}"
	Const REGEX_OffRes8 As String = "\{(?i)TMPL_VAR name=\'OfferNachricht\'\}"

	Const REGEX_OffBez As String = "\{(?i)TMPL_VAR name=\'OfferBez\'\}"
	Const REGEX_OffSlogan As String = "\{(?i)TMPL_VAR name=\'OfferWerbetext\'\}"
	Const REGEX_OffGruppe As String = "\{(?i)TMPL_VAR name=\'OfferGruppe\'\}"
	Const REGEX_OffKontakt As String = "\{(?i)TMPL_VAR name=\'OfferKontakt\'\}"

	Const REGEX_Off_KDNr As String = "\{(?i)TMPL_VAR name=\'OFF_KDNr\'\}"
	Const REGEX_Off_KDZNr As String = "\{(?i)TMPL_VAR name=\'OFF_KDZNr\'\}"

	Const REGEX_MANr As String = "\{(?i)TMPL_VAR name=\'MANr\'\}"
	Const REGEX_MA_Nachname As String = "\{(?i)TMPL_VAR name=\'MANachname\'\}"
	Const REGEX_MA_Vorname As String = "\{(?i)TMPL_VAR name=\'MAVorname\'\}"
	Const REGEX_MA_BriefAnrede As String = "\{(?i)TMPL_VAR name=\'MABriefAnrede\'\}"
	Const REGEX_MA_Anrede As String = "\{(?i)TMPL_VAR name=\'MAAnrede\'\}"
	Const REGEX_MA_ForAnrede As String = "\{(?i)TMPL_VAR name=\'MAForAnrede\'\}"
	Const REGEX_MA_Owner_Guid As String = "\{(?i)TMPL_VAR name=\'MAOwner_Guid\'\}"
	Const REGEX_MA_DocLink As String = "\{(?i)TMPL_VAR name=\'MADocLink\'\}"

#End Region


#Region "Private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private _ClsReg As New SPProgUtility.ClsDivReg
	'private _ClsSystem As New ClsMain_Net
	Private _ClsLog As New SPProgUtility.ClsEventLog
	Private _ClsApp As New ClsAssInfo
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUI As UtilityUI

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting

		m_Utility = New SPProgUtility.MainUtilities.Utilities
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

	End Sub

#End Region


#Region "Public Properties"

	Public Property EMailLanguageTemplateFilename As String
	Public Property EMailSubject As String
	Public Property DestinationLanguage As String

	Public Property CurrentOfferData As OffersMasterData
	Public Property CurrentUserData As UserAndMandantPrintData

#End Region

	Function ParseTemplateFile(ByVal FullFileName As String) As String

		Dim ParsedFile As String = String.Empty
		Dim line As String = String.Empty
		Dim pattern As String = String.Empty
		Dim bLocAsHtml As Boolean = True  ' BodyAsHtml

		If String.IsNullOrWhiteSpace(FullFileName) Then
			FullFileName = EMailLanguageTemplateFilename
			m_Logger.LogDebug(String.Format("Template filename: {0}", FullFileName))

			If Not File.Exists(FullFileName) Then
				Dim msg As String = String.Format("Die Vorlage für EMail-Versand wurde nicht gefunden. Die Daten werden leer gesendet.{0}{1}", vbNewLine, FullFileName)

				m_UtilityUI.ShowErrorDialog(msg)

				Return String.Empty
			Else

				Dim lines = IO.File.ReadAllLines(FullFileName, System.Text.Encoding.Default)
				For Each line In lines
					ParsedFile &= line + vbNewLine
				Next
			End If

		Else
			bLocAsHtml = False
			ParsedFile = FullFileName + vbCrLf

		End If

		Try

			'// search templatevars
			pattern = REGEX_ANREDE
			Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(pattern)

			'// replace vars
			ParsedFile = Regex.Replace(ParsedFile, REGEX_Off_Nr, CurrentOfferData.OfNumber.ToString)
			ParsedFile = regex.Replace(ParsedFile, REGEX_MailBetreff, m_Utility.ReplaceMissing(EMailSubject, String.Empty))

			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffRes1, String.Format("{0}", CurrentOfferData.OF_Res1))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffRes2, String.Format("{0}", CurrentOfferData.OF_Res2))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffRes3, String.Format("{0}", CurrentOfferData.OF_Res3))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffRes4, String.Format("{0}", CurrentOfferData.OF_Res4))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffRes5, String.Format("{0}", CurrentOfferData.OF_Res5))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffRes6, String.Format("{0}", CurrentOfferData.OF_Res6))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffRes8, String.Format("{0}", CurrentOfferData.OF_Res8))

			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffBez, String.Format("{0}", CurrentOfferData.OFLabel))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffGruppe, String.Format("{0}", CurrentOfferData.OF_Group))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffSlogan, String.Format("{0}", CurrentOfferData.OF_Slogan))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_OffKontakt, String.Format("{0}", CurrentOfferData.OF_Kontakt))

			ParsedFile = Regex.Replace(ParsedFile, REGEX_Off_KDNr, String.Format("{0}", CurrentOfferData.CustomerNumber))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_Off_KDZNr, String.Format("{0}", CurrentOfferData.CResponsibleNumber))


			ParsedFile = regex.Replace(ParsedFile, REGEX_KDFirma1, m_Utility.ReplaceMissing(CurrentOfferData.CustomerCompany, String.Empty))
			ParsedFile = regex.Replace(ParsedFile, REGEX_KDEMail, m_Utility.ReplaceMissing(CurrentOfferData.CustomerEMail, String.Empty))
			If String.IsNullOrWhiteSpace(CurrentOfferData.CResponsibleLetterSalutation) Then
				ParsedFile = Regex.Replace(ParsedFile, REGEX_ANREDE, m_Translate.GetSafeTranslationValueInOtherLanguage("Sehr geehrte Damen und Herren", DestinationLanguage))
			Else
				ParsedFile = Regex.Replace(ParsedFile, REGEX_ANREDE, m_Translate.GetSafeTranslationValueInOtherLanguage(CurrentOfferData.CResponsibleLetterSalutation, DestinationLanguage))
			End If

			ParsedFile = regex.Replace(ParsedFile, REGEX_ANREDE_0, m_Utility.ReplaceMissing(CurrentOfferData.CResponsibleSalution, String.Empty))
			ParsedFile = regex.Replace(ParsedFile, REGEX_KDzNachname, m_Utility.ReplaceMissing(CurrentOfferData.CResponsibleLastname, String.Empty))
			ParsedFile = regex.Replace(ParsedFile, REGEX_KDzVorname, m_Utility.ReplaceMissing(CurrentOfferData.CResponsibleFirstname, String.Empty))
			ParsedFile = regex.Replace(ParsedFile, REGEX_KDzEMail, m_Utility.ReplaceMissing(CurrentOfferData.CResponsibleEMail, String.Empty))


			ParsedFile = Regex.Replace(ParsedFile, REGEX_Off_USNr, String.Format("{0}", CurrentUserData.USNr))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USAnrede, String.Format("{0}", CurrentUserData.USAnrede))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USNachname, String.Format("{0}", CurrentUserData.USNachname))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USVorname, String.Format("{0}", CurrentUserData.USVorname))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTelefon, String.Format("{0}", CurrentUserData.USTelefon))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTelefax, String.Format("{0}", CurrentUserData.USTelefax))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USNatel, String.Format("{0}", CurrentUserData.USNatel))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USeMail, String.Format("{0}", CurrentUserData.USeMail))


			ParsedFile = Regex.Replace(ParsedFile, REGEX_USPostfach, String.Format("{0}", CurrentUserData.USPostfach))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USStrasse, String.Format("{0}", CurrentUserData.USStrasse))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USPLZ, String.Format("{0}", CurrentUserData.USPLZ))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USOrt, String.Format("{0}", CurrentUserData.USOrt))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USLand, String.Format("{0}", CurrentUserData.USLand))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USAbteilung, String.Format("{0}", CurrentUserData.USAbteilung))


			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDName, String.Format("{0}", CurrentUserData.USMDname))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDName2, String.Format("{0}", CurrentUserData.USMDname2))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDName3, String.Format("{0}", CurrentUserData.USMDname3))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDPostfach, String.Format("{0}", CurrentUserData.USMDPostfach))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDStrasse, String.Format("{0}", CurrentUserData.USMDStrasse))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDOrt, String.Format("{0}", CurrentUserData.USMDOrt))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDPlz, String.Format("{0}", CurrentUserData.USMDPlz))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDLand, String.Format("{0}", CurrentUserData.USMDLand))


			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDDTelefon, String.Format("{0}", CurrentUserData.USMDDTelefon))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDTelefon, String.Format("{0}", CurrentUserData.USMDTelefon))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDTelefax, String.Format("{0}", CurrentUserData.USMDTelefax))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDeMail, String.Format("{0}", CurrentUserData.USMDeMail))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USMDHomepage, String.Format("{0}", CurrentUserData.USMDHomepage))

			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTitel_1, String.Format("{0}", CurrentUserData.USTitel_1))
			ParsedFile = Regex.Replace(ParsedFile, REGEX_USTitel_2, String.Format("{0}", CurrentUserData.USTitel_2))


			m_Logger.LogDebug(String.Format("finishing parse template file: bLocAsHtml: {0}", bLocAsHtml))
			If bLocAsHtml Then ParsedFile = SetSyntax(ParsedFile)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			'      ParsedFile = String.Empty
			'      MsgBox("Feher: " & ex.Message.Trim & vbCrLf & ParsedFile & vbCrLf & pattern & vbCrLf & line)

		End Try

		Return ParsedFile
	End Function

	Private Function SetSyntax(ByVal str1 As String) As String
		'Console.WriteLine("Str1/1: " & str1)

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
		str1 = Replace(str1, Chr(252), "&uuml;")      ' ü
		str1 = Replace(str1, Chr(129), "&uuml;")      ' ü

		Return str1
	End Function


End Class

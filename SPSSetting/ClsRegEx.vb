
Imports System.Text.RegularExpressions
Imports System.IO

Public Class ClsRegEx

  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath ' ClsMain_Net
  Dim _ClsLog As New SPProgUtility.ClsEventLog
  Dim _ClsApp As New ClsAssInfo


#Region "Properties"
  '// OfferRes3 (= OfferRes3 des Offertes)
  Dim _Firma1 As String = String.Empty
  Public Property Off_Firma1() As String
    Get
      Return _Firma1
    End Get
    Set(ByVal value As String)
      _Firma1 = value
    End Set
  End Property

  '// Guid für Kunden-Mail
  Dim _KD_Guid As String = String.Empty
  Public Property KD_Guid() As String
    Get
      Return _KD_Guid
    End Get
    Set(ByVal value As String)
      _KD_Guid = value
    End Set
  End Property

  '// Link für Kunden-Mail
  Dim _KDDocLink As String = String.Empty
  Public Property KDDocLink() As String
    Get
      Return _KDDocLink
    End Get
    Set(ByVal value As String)
      _KDDocLink = value
    End Set
  End Property

  '// Guid für ZHD-Mail
  Dim _ZHD_Guid As String = String.Empty
  Public Property ZHD_Guid() As String
    Get
      Return _ZHD_Guid
    End Get
    Set(ByVal value As String)
      _ZHD_Guid = value
    End Set
  End Property

  '// Link für ZHD-Mail
  Dim _ZHDDocLink As String = String.Empty
  Public Property ZHDDocLink() As String
    Get
      Return _ZHDDocLink
    End Get
    Set(ByVal value As String)
      _ZHDDocLink = value
    End Set
  End Property

  '// Anredeform (= Anredeform von ZHD)
  Dim _KDzAnredeform As String = String.Empty
  Public Property KdzAnredeform() As String
    Get
      Return _KDzAnredeform
    End Get
    Set(ByVal value As String)
      _KDzAnredeform = value
    End Set
  End Property

  '// Anrede (= Anrede von ZHD)
  Dim _KDzAnrede As String = String.Empty
  Public Property KdzAnrede() As String
    Get
      Return _KDzAnrede
    End Get
    Set(ByVal value As String)
      _KDzAnrede = value
    End Set
  End Property

  '// ZhdNachname (= Nachname von ZHD)
  Dim _ZhdNachname As String = String.Empty
  Public Property KdzNachname() As String
    Get
      Return _ZhdNachname
    End Get
    Set(ByVal value As String)
      _ZhdNachname = value
    End Set
  End Property

  '// ZhdVorname (= Vorname von ZHD)
  Dim _ZhdVorname As String = String.Empty
  Public Property KdzVorname() As String
    Get
      Return _ZhdVorname
    End Get
    Set(ByVal value As String)
      _ZhdVorname = value
    End Set
  End Property

  '// Kundenemail (= KDeMail des Personalvermittlers)
  Dim _KdEmail As String = String.Empty
  Public Property KdEmail() As String
    Get
      Return _KdEmail
    End Get
    Set(ByVal value As String)
      _KdEmail = value
    End Set
  End Property

  '// Kundenemail (= Kunde des Personalvermittlers)
  Dim _KdZEmail As String = String.Empty
  Public Property KdZEmail() As String
    Get
      Return _KdZEmail
    End Get
    Set(ByVal value As String)
      _KdZEmail = value
    End Set
  End Property

  '// USNr (= USNr des Kostenstell)
  Dim _Off_USNr As Integer = 0
  Public Property Off_USNr() As Integer
    Get
      Return _Off_USNr
    End Get
    Set(ByVal value As Integer)
      _Off_USNr = value
    End Set
  End Property
  '// USeMail (= eMail des Personalvermittlers)
  Dim _USeMail As String = String.Empty
  Public Property USeMail() As String
    Get
      Return _USeMail
    End Get
    Set(ByVal value As String)
      _USeMail = value
    End Set
  End Property

  '// USTelefon (= USTelefon des Personalvermittlers)
  Dim _USTelefon As String = String.Empty
  Public Property USTelefon() As String
    Get
      Return _USTelefon
    End Get
    Set(ByVal value As String)
      _USTelefon = value
    End Set
  End Property

  '// USTelefax (= USTelefax des Personalvermittlers)
  Dim _USTelefax As String = String.Empty
  Public Property USTelefax() As String
    Get
      Return _USTelefax
    End Get
    Set(ByVal value As String)
      _USTelefax = value
    End Set
  End Property

  '// USVorname (= USVorname des Personalvermittlers)
  Dim _USVorname As String = String.Empty
  Public Property USVorname() As String
    Get
      Return _USVorname
    End Get
    Set(ByVal value As String)
      _USVorname = value
    End Set
  End Property

  '// USAnrede (= USAnrede des Personalvermittlers)
  Dim _USAnrede As String = String.Empty
  Public Property USAnrede() As String
    Get
      Return _USAnrede
    End Get
    Set(ByVal value As String)
      _USAnrede = value
    End Set
  End Property

  '// USNachname (= USNachname des Personalvermittlers)
  Dim _USNachname As String = String.Empty
  Public Property USNachname() As String
    Get
      Return _USNachname
    End Get
    Set(ByVal value As String)
      _USNachname = value
    End Set
  End Property

  '// USTitel_1 (= USTitel_1 des Personalvermittlers)
  Dim _USTitel_1 As String = String.Empty
  Public Property USTitel_1() As String
    Get
      Return _USTitel_1
    End Get
    Set(ByVal value As String)
      _USTitel_1 = value
    End Set
  End Property

  '// USTitel_2 (= USTitel_2 des Personalvermittlers)
  Dim _USTitel_2 As String = String.Empty
  Public Property USTitel_2() As String
    Get
      Return _USTitel_2
    End Get
    Set(ByVal value As String)
      _USTitel_2 = value
    End Set
  End Property

  '// USStrass (= USStrasse des Personalvermittlers)
  Dim _USStrasse As String = String.Empty
  Public Property USStrasse() As String
    Get
      Return _USStrasse
    End Get
    Set(ByVal value As String)
      _USStrasse = value
    End Set
  End Property

  '// USPst (= USPostfach des Personalvermittlers)
  Dim _USPostfach As String = String.Empty
  Public Property USPostfach() As String
    Get
      Return _USpostfach
    End Get
    Set(ByVal value As String)
      _USPostfach = value
    End Set
  End Property

  '// USPLZ (= USPLZ des Personalvermittlers)
  Dim _USPLZ As String = String.Empty
  Public Property USPLZ() As String
    Get
      Return _USPLZ
    End Get
    Set(ByVal value As String)
      _USPLZ = value
    End Set
  End Property

  '// USOrt (= USOrt des Personalvermittlers)
  Dim _USOrt As String = String.Empty
  Public Property USOrt() As String
    Get
      Return _USOrt
    End Get
    Set(ByVal value As String)
      _USOrt = value
    End Set
  End Property

  '// USLand (= USLand des Personalvermittlers)
  Dim _USLand As String = String.Empty
  Public Property USLand() As String
    Get
      Return _USLand
    End Get
    Set(ByVal value As String)
      _USLand = value
    End Set
  End Property

  '// USAbteil (= USAbteil des Personalvermittlers)
  Dim _USAbteil As String = String.Empty
  Public Property USAbteil() As String
    Get
      Return _USAbteil
    End Get
    Set(ByVal value As String)
      _USAbteil = value
    End Set
  End Property



  '// USMDName (= MDName des Personalvermittlers)
  Dim _USMDname As String = String.Empty
  Public Property USMDname() As String
    Get
      Return _USMDname
    End Get
    Set(ByVal value As String)
      _USMDname = value
    End Set
  End Property

  '// USMDName2 (= MDName2 des Personalvermittlers)
  Dim _USMDname2 As String = String.Empty
  Public Property USMDname2() As String
    Get
      Return _USMDname2
    End Get
    Set(ByVal value As String)
      _USMDname2 = value
    End Set
  End Property

  '// USMDPostfach (= MDPostfach des Personalvermittlers)
  Dim _USMDPostfach As String = String.Empty
  Public Property USMDPostfach() As String
    Get
      Return _USMDPostfach
    End Get
    Set(ByVal value As String)
      _USMDPostfach = value
    End Set
  End Property

  '// USMDStrasse (= MDstrasse des Personalvermittlers)
  Dim _USMDStrasse As String = String.Empty
  Public Property USMDStrasse() As String
    Get
      Return _USMDStrasse
    End Get
    Set(ByVal value As String)
      _USMDStrasse = value
    End Set
  End Property

  '// USMDOrt (= MDOrt des Personalvermittlers)
  Dim _USMDOrt As String = String.Empty
  Public Property USMDOrt() As String
    Get
      Return _USMDOrt
    End Get
    Set(ByVal value As String)
      _USMDOrt = value
    End Set
  End Property

  '// USMDPLZ (= MDPLZ des Personalvermittlers)
  Dim _USMDPlz As String = String.Empty
  Public Property USMDPlz() As String
    Get
      Return _USMDPlz
    End Get
    Set(ByVal value As String)
      _USMDPlz = value
    End Set
  End Property

  '// USMDLand (= MDLand des Personalvermittlers)
  Dim _USMDLand As String = String.Empty
  Public Property USMDLand() As String
    Get
      Return _USMDLand
    End Get
    Set(ByVal value As String)
      _USMDLand = value
    End Set
  End Property

  '// USMDTelefon (= MDTelefon des Personalvermittlers)
  Dim _USMDTelefon As String = String.Empty
  Public Property USMDTelefon() As String
    Get
      Return _USMDTelefon
    End Get
    Set(ByVal value As String)
      _USMDTelefon = value
    End Set
  End Property

  '// USMDTelefax (= MDTelefax des Personalvermittlers)
  Dim _USMDTelefax As String = String.Empty
  Public Property USMDTelefax() As String
    Get
      Return _USMDTelefax
    End Get
    Set(ByVal value As String)
      _USMDTelefax = value
    End Set
  End Property


  '// USMDeMail (= MDeMail des Personalvermittlers)
  Dim _USMDeMail As String = String.Empty
  Public Property USMDeMail() As String
    Get
      Return _USMDeMail
    End Get
    Set(ByVal value As String)
      _USMDeMail = value
    End Set
  End Property

  '// USMDHomepage (= MDHomepage des Personalvermittlers)
  Dim _USMDHomepage As String = String.Empty
  Public Property USMDHomepage() As String
    Get
      Return _USMDHomepage
    End Get
    Set(ByVal value As String)
      _USMDHomepage = value
    End Set
  End Property

  '// Body in Html (= Body in Html des Personalvermittlers)
  Dim _bBodyAsHtml As Boolean
  Public Property BodyAsHtml() As Boolean
    Get
      Return _bBodyAsHtml
    End Get
    Set(ByVal value As Boolean)
      _bBodyAsHtml = value
    End Set
  End Property

  '// Betreff (= MailBetreff des Offertes)
  Dim _MailBetreff As String = String.Empty
  Public Property MailBetreff() As String
    Get
      Return _MailBetreff
    End Get
    Set(ByVal value As String)
      _MailBetreff = value
    End Set
  End Property

  '// Schlusstext (= Schlusstext des Offertes)
  Dim _OfferSchluss As String = String.Empty
  Public Property OfferSchluss() As String
    Get
      Return _OfferSchluss
    End Get
    Set(ByVal value As String)
      _OfferSchluss = value
    End Set
  End Property

  '// OfferNachricht (= OfferNachricht des Offertes)
  Dim _OfferNachricht As String = String.Empty
  Public Property OfferNachricht() As String
    Get
      Return _OfferNachricht
    End Get
    Set(ByVal value As String)
      _OfferNachricht = value
    End Set
  End Property

  '// OfferRes1 (= OfferRes1 des Offertes)
  Dim _OfferRes1 As String = String.Empty
  Public Property OfferRes1() As String
    Get
      Return _OfferRes1
    End Get
    Set(ByVal value As String)
      _OfferRes1 = value
    End Set
  End Property

  '// OfferRes2 (= OfferRes2 des Offertes)
  Dim _OfferRes2 As String = String.Empty
  Public Property OfferRes2() As String
    Get
      Return _OfferRes2
    End Get
    Set(ByVal value As String)
      _OfferRes2 = value
    End Set
  End Property

  '// OfferRes3 (= OfferRes3 des Offertes)
  Dim _OfferRes3 As String = String.Empty
  Public Property OfferRes3() As String
    Get
      Return _OfferRes3
    End Get
    Set(ByVal value As String)
      _OfferRes3 = value
    End Set
  End Property

  '// OfferRes4 (= OfferRes4 des Offertes)
  Dim _OfferRes4 As String = String.Empty
  Public Property OfferRes4() As String
    Get
      Return _OfferRes4
    End Get
    Set(ByVal value As String)
      _OfferRes4 = value
    End Set
  End Property

  '// OfferRes5 (= OfferRes5 des Offertes)
  Dim _OfferRes5 As String = String.Empty
  Public Property OfferRes5() As String
    Get
      Return _OfferRes5
    End Get
    Set(ByVal value As String)
      _OfferRes5 = value
    End Set
  End Property

  '// OfferWerbe (= OfferWerbe des Offertes)
  Dim _OfferWerbe As String = String.Empty
  Public Property OfferWerbe() As String
    Get
      Return _OfferWerbe
    End Get
    Set(ByVal value As String)
      _OfferWerbe = value
    End Set
  End Property

  '// OfferGruppe (= OfferGruppe des Offertes)
  Dim _OfferGruppe As String = String.Empty
  Public Property OfferGruppe() As String
    Get
      Return _OfferGruppe
    End Get
    Set(ByVal value As String)
      _OfferGruppe = value
    End Set
  End Property

  '// OfferKontakt (= OfferKontakt des Offertes)
  Dim _OfferKontakt As String = String.Empty
  Public Property OfferKontakt() As String
    Get
      Return _OfferKontakt
    End Get
    Set(ByVal value As String)
      _OfferKontakt = value
    End Set
  End Property

  '// OfferBez (= OfferBez des Offertes)
  Dim _OfferBez As String = String.Empty
  Public Property OfferBez() As String
    Get
      Return _OfferBez
    End Get
    Set(ByVal value As String)
      _OfferBez = value
    End Set
  End Property

  '// OffKDNr (= KDNr des Offertes)
  Dim _Off_KDNr As Integer = 0
  Public Property Off_KDNr() As Integer
    Get
      Return _Off_KDNr
    End Get
    Set(ByVal value As Integer)
      _Off_KDNr = value
    End Set
  End Property

  '// Off_KDZNr (= Off_KDZNr des ZHD)
  Dim _Off_KDZNr As Integer = 0
  Public Property Off_KDZNr() As Integer
    Get
      Return _Off_KDZNr
    End Get
    Set(ByVal value As Integer)
      _Off_KDZNr = value
    End Set
  End Property

  '// Nachname des Kandidaten
  Dim _MANachname As String = String.Empty
  Public Property MANachname() As String
    Get
      Return _MANachname
    End Get
    Set(ByVal value As String)
      _MANachname = value
    End Set
  End Property

  '// Vorname des Kandidaten
  Dim _MAVorname As String = String.Empty
  Public Property MAVorname() As String
    Get
      Return _MAVorname
    End Get
    Set(ByVal value As String)
      _MAVorname = value
    End Set
  End Property

  '// Mail des Kandidaten
  Dim _MAMail As String = String.Empty
  Public Property MAMail() As String
    Get
      Return _MAMail
    End Get
    Set(ByVal value As String)
      _MAMail = value
    End Set
  End Property

  '// Anredeart des Kandidaten
  Dim _MABriefAnrede As String = String.Empty
  Public Property MABriefAnrede() As String
    Get
      Return _MABriefAnrede
    End Get
    Set(ByVal value As String)
      _MABriefAnrede = value
    End Set
  End Property

  '// Anrede des Kandidaten
  Dim _MAAnrede As String = String.Empty
  Public Property MAAnrede() As String
    Get
      Return _MAAnrede
    End Get
    Set(ByVal value As String)
      _MAAnrede = value
    End Set
  End Property

  '// Link für Kandidatendoc-Mail
  Dim _MADocLink As String = String.Empty
  Public Property MADocLink() As String
    Get
      Return _MADocLink
    End Get
    Set(ByVal value As String)
      _MADocLink = value
    End Set
  End Property

  '// Link für Kandidatendoc-Mail
  Dim _MAOwner_Guid As String = String.Empty
  Public Property MAOwner_Guid() As String
    Get
      Return _MAOwner_Guid
    End Get
    Set(ByVal value As String)
      _MAOwner_Guid = value
    End Set
  End Property

  '// Exchange UserName
  Dim _Exchange_USName As String = String.Empty
  Public Property Exchange_USName() As String
    Get
      Return _Exchange_USName
    End Get
    Set(ByVal value As String)
      _Exchange_USName = value
    End Set
  End Property

  '// Exchange UserPW
  Dim _Exchange_USPW As String = String.Empty
  Public Property Exchange_USPW() As String
    Get
      Return _Exchange_USPW
    End Get
    Set(ByVal value As String)
      _Exchange_USPW = value
    End Set
  End Property

#End Region

  Function ParseTemplateFile(ByVal strMyText As String, ByVal FullFileName As String) As String

    If strMyText & FullFileName = String.Empty Then Return strMyText
    Const REGEX_MailBetreff As String = "\{(?i)TMPL_VAR name=\'MailBetreff\'\}"
    Const REGEX_KDFirma1 As String = "\{(?i)TMPL_VAR name=\'KDFirma1\'\}"

    Const REGEX_ANREDE As String = "\{(?i)TMPL_VAR name=\'Anredeform\'\}"
    Const REGEX_ANREDE_0 As String = "\{(?i)TMPL_VAR name=\'KDzAnrede\'\}"
    Const REGEX_KDzVorname As String = "\{(?i)TMPL_VAR name=\'KDzVorname\'\}"
    Const REGEX_KDzNachname As String = "\{(?i)TMPL_VAR name=\'KDzNachname\'\}"

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
    Const REGEX_USeMail As String = "\{(?i)TMPL_VAR name=\'USeMail\'\}"

    Const REGEX_USMDName As String = "\{(?i)TMPL_VAR name=\'USMDName\'\}"
    Const REGEX_USMDName2 As String = "\{(?i)TMPL_VAR name=\'USMDName2\'\}"
    Const REGEX_USMDPostfach As String = "\{(?i)TMPL_VAR name=\'USMDPostfach\'\}"
    Const REGEX_USMDStrasse As String = "\{(?i)TMPL_VAR name=\'USMDStrasse\'\}"
    Const REGEX_USMDOrt As String = "\{(?i)TMPL_VAR name=\'USMDort\'\}"
    Const REGEX_USMDPlz As String = "\{(?i)TMPL_VAR name=\'USMDPlz\'\}"
    Const REGEX_USMDLand As String = "\{(?i)TMPL_VAR name=\'USMDLand\'\}"

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

    Const REGEX_MA_Nachname As String = "\{(?i)TMPL_VAR name=\'MANachname\'\}"
    Const REGEX_MA_Vorname As String = "\{(?i)TMPL_VAR name=\'MAVorname\'\}"
    Const REGEX_MA_BriefAnrede As String = "\{(?i)TMPL_VAR name=\'MABriefAnrede\'\}"
    Const REGEX_MA_Anrede As String = "\{(?i)TMPL_VAR name=\'MAAnrede\'\}"
    Const REGEX_MA_ForAnrede As String = "\{(?i)TMPL_VAR name=\'MAForAnrede\'\}"
    Const REGEX_MA_Owner_Guid As String = "\{(?i)TMPL_VAR name=\'MAOwner_Guid\'\}"
    Const REGEX_MA_DocLink As String = "\{(?i)TMPL_VAR name=\'MADocLink\'\}"

    Dim ParsedFile As String = String.Empty
    Dim line As String = String.Empty
    Dim pattern As String = String.Empty
    Dim bLocAsHtml As Boolean = BodyAsHtml

    If FullFileName <> String.Empty Then
      If Not File.Exists(FullFileName) Then
        MsgBox(String.Format("Die Vorlage für eMail-Versand konnte ich nicht finden." & vbCrLf & _
               "Die Daten werden leer gesendet.{0}{1}", vbNewLine, FullFileName), _
               MsgBoxStyle.Information, "Vorlage für Mailversand")

        Return ""

      Else
        With My.Computer.FileSystem.OpenTextFileReader(FullFileName, System.Text.Encoding.Default)
          Do
            line = .ReadLine()
            ParsedFile += line + vbNewLine
          Loop Until line Is Nothing

          .Close()
        End With

      End If

    Else
      bLocAsHtml = False
      ParsedFile = strMyText + vbCrLf

    End If

    Try

      '// search templatevars
      pattern = REGEX_ANREDE
      Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(pattern)

			' replace vars
			ParsedFile = regex.Replace(ParsedFile, REGEX_MailBetreff, _MailBetreff)

      'ParsedFile = regex.Replace(ParsedFile, REGEX_MANr, ClsDataDetail.GetMANr)
      ParsedFile = regex.Replace(ParsedFile, REGEX_MA_Nachname, _MANachname)
      ParsedFile = regex.Replace(ParsedFile, REGEX_MA_Vorname, _MAVorname)
      ParsedFile = regex.Replace(ParsedFile, REGEX_MA_BriefAnrede, _MABriefAnrede)
      ParsedFile = regex.Replace(ParsedFile, REGEX_MA_Anrede, _MAAnrede)
      ParsedFile = regex.Replace(ParsedFile, REGEX_MA_ForAnrede, If(_MAAnrede = "Herr", "Herrn", "Frau"))
      ParsedFile = regex.Replace(ParsedFile, REGEX_MA_Owner_Guid, _MAOwner_Guid)
      ParsedFile = regex.Replace(ParsedFile, REGEX_MA_DocLink, _MADocLink)

      ParsedFile = regex.Replace(ParsedFile, REGEX_KDFirma1, _Firma1)
      ParsedFile = regex.Replace(ParsedFile, REGEX_ANREDE, _KDzAnredeform)
      ParsedFile = regex.Replace(ParsedFile, REGEX_ANREDE_0, _KDzAnrede)
      ParsedFile = regex.Replace(ParsedFile, REGEX_KDzNachname, _ZhdNachname)
      ParsedFile = regex.Replace(ParsedFile, REGEX_KDzVorname, _ZhdVorname)

      ParsedFile = regex.Replace(ParsedFile, REGEX_KD_Guid, _KD_Guid)
      ParsedFile = regex.Replace(ParsedFile, REGEX_KD_DocLink, _KDDocLink)

      ParsedFile = regex.Replace(ParsedFile, REGEX_ZHD_Guid, _ZHD_Guid)
      ParsedFile = regex.Replace(ParsedFile, REGEX_ZHD_DocLink, _ZHDDocLink)


      ParsedFile = regex.Replace(ParsedFile, REGEX_Off_USNr, CStr(_Off_USNr))
      ParsedFile = regex.Replace(ParsedFile, REGEX_USAnrede, _USAnrede)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USNachname, _USNachname)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USVorname, _USVorname)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USTelefon, _USTelefon)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USTelefax, _USTelefax)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USeMail, _USeMail)

      ParsedFile = regex.Replace(ParsedFile, REGEX_USPostfach, _USPostfach)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USStrasse, _USStrasse)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USPLZ, _USPLZ)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USOrt, _USOrt)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USLand, _USLand)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USAbteilung, _USAbteil)

      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDName, _USMDname)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDName2, _USMDname2)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDPostfach, _USMDPostfach)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDStrasse, _USMDStrasse)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDOrt, _USMDOrt)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDPlz, _USMDPlz)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDLand, _USMDLand)

      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDTelefon, _USMDTelefon)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDTelefax, _USMDTelefax)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDeMail, _USMDeMail)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USMDHomepage, _USMDHomepage)

      ParsedFile = regex.Replace(ParsedFile, REGEX_USTitel_1, _USTitel_1)
      ParsedFile = regex.Replace(ParsedFile, REGEX_USTitel_2, _USTitel_2)

      ParsedFile = regex.Replace(ParsedFile, REGEX_OffRes1, _OfferRes1)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffRes2, _OfferRes2)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffRes3, _OfferRes3)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffRes4, _OfferRes4)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffRes5, _OfferRes5)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffRes6, _OfferSchluss)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffRes8, _OfferNachricht)

      ParsedFile = regex.Replace(ParsedFile, REGEX_OffBez, _OfferBez)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffGruppe, _OfferGruppe)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffSlogan, _OfferWerbe)
      ParsedFile = regex.Replace(ParsedFile, REGEX_OffKontakt, _OfferKontakt)

      ParsedFile = regex.Replace(ParsedFile, REGEX_Off_KDNr, CStr(_Off_KDNr))
      ParsedFile = regex.Replace(ParsedFile, REGEX_Off_KDZNr, CStr(_Off_KDZNr))

      If _bBodyAsHtml And bLocAsHtml Then ParsedFile = SetSyntax(ParsedFile)

    Catch ex As Exception
      '      ParsedFile = String.Empty
      '      MsgBox("Feher: " & ex.Message.Trim & vbCrLf & ParsedFile & vbCrLf & pattern & vbCrLf & line)

    End Try

    Return ParsedFile
  End Function

  Private Function SetSyntax(ByVal str1 As String) As String
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
    str1 = Replace(str1, Chr(252), "&uuml;")      ' ü
    str1 = Replace(str1, Chr(129), "&uuml;")      ' ü

    SetSyntax = str1

    Console.WriteLine("Str1/2: " & str1)

  End Function

End Class

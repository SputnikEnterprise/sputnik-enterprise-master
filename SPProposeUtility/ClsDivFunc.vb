
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging

Imports SPProposeUtility.ClsDataDetail


Public Class ClsDivFunc


#Region "Diverses"

  '// Get4What._strModul4What
  Dim _strModul4What As String
  Public Property Get4What() As String
    Get
      Return _strModul4What
    End Get
    Set(ByVal value As String)
      _strModul4What = value
    End Set
  End Property

  '// Query.GetSearchQuery
  Dim _strQuery As String
  Public Property GetSearchQuery() As String
    Get
      Return _strQuery
    End Get
    Set(ByVal value As String)
      _strQuery = value
    End Set
  End Property

  '// LargerLV
  Dim _bLargerLV As Boolean
  Public Property GetLargerLV() As Boolean
    Get
      Return _bLargerLV
    End Get
    Set(ByVal value As Boolean)
      _bLargerLV = value
    End Set
  End Property


#End Region


#Region "Funktionen für LvClick in der Suchmaske..."

  '// KDNr
  Dim _strKDNr As String
  Public Property GetKDNr() As String
    Get
      Return _strKDNr
    End Get
    Set(ByVal value As String)
      _strKDNr = value
    End Set
  End Property

  '// Firmenname
  Dim _strKDName As String
  Public Property GetKDName() As String
    Get
      Return _strKDName
    End Get
    Set(ByVal value As String)
      _strKDName = value
    End Set
  End Property

  '// KDZhd
  Dim _iKDZhdNr As Integer
  Public Property GetKDZhdNr() As Integer
    Get
      Return _iKDZhdNr
    End Get
    Set(ByVal value As Integer)
      _iKDZhdNr = value
    End Set
  End Property

  '// Query.GetSearchQuery
  Dim _strTelNr As String
  Public Property GetTelNr() As String
    Get
      Return _strTelNr
    End Get
    Set(ByVal value As String)
      _strTelNr = value
    End Set
  End Property

#End Region


#Region "LL_Properties"
  '// Print.LLDocName
  Dim _LLDocName As String
  Public Property LLDocName() As String
    Get
      Return _LLDocName
    End Get
    Set(ByVal value As String)
      _LLDocName = value
    End Set
  End Property

  '// Print.LLDocLabel
  Dim _LLDocLabel As String
  Public Property LLDocLabel() As String
    Get
      Return _LLDocLabel
    End Get
    Set(ByVal value As String)
      _LLDocLabel = value
    End Set
  End Property

  '// Print.LLFontDesent
  Dim _LLFontDesent As Integer
  Public Property LLFontDesent() As Integer
    Get
      Return _LLFontDesent
    End Get
    Set(ByVal value As Integer)
      _LLFontDesent = value
    End Set
  End Property

  '// Print.LLIncPrv
  Dim _LLIncPrv As Integer
  Public Property LLIncPrv() As Integer
    Get
      Return _LLIncPrv
    End Get
    Set(ByVal value As Integer)
      _LLIncPrv = value
    End Set
  End Property

  '// Print.LLParamCheck
  Dim _LLParamCheck As Integer
  Public Property LLParamCheck() As Integer
    Get
      Return _LLParamCheck
    End Get
    Set(ByVal value As Integer)
      _LLParamCheck = value
    End Set
  End Property

  '// Print.LLKonvertName
  Dim _LLKonvertName As Integer
  Public Property LLKonvertName() As Integer
    Get
      Return _LLKonvertName
    End Get
    Set(ByVal value As Integer)
      _LLKonvertName = value
    End Set
  End Property

  '// Print.LLZoomProz
  Dim _LLZoomProz As Integer
  Public Property LLZoomProz() As Integer
    Get
      Return _LLZoomProz
    End Get
    Set(ByVal value As Integer)
      _LLZoomProz = value
    End Set
  End Property

  '// Print.LLCopyCount
  Dim _LLCopyCount As Integer
  Public Property LLCopyCount() As Integer
    Get
      Return _LLCopyCount
    End Get
    Set(ByVal value As Integer)
      _LLCopyCount = value
    End Set
  End Property

  '// Print.LLExportedFilePath
  Dim _LLExportedFilePath As String
  Public Property LLExportedFilePath() As String
    Get
      Return _LLExportedFilePath
    End Get
    Set(ByVal value As String)
      _LLExportedFilePath = value
    End Set
  End Property

  '// Print.LLExportedFileName
  Dim _LLExportedFileName As String
  Public Property LLExportedFileName() As String
    Get
      Return _LLExportedFileName
    End Get
    Set(ByVal value As String)
      _LLExportedFileName = value
    End Set
  End Property

  '// Print.LLExportfilter
  Dim _LLExportfilter As String
  Public Property LLExportfilter() As String
    Get
      Return _LLExportfilter
    End Get
    Set(ByVal value As String)
      _LLExportfilter = value
    End Set
  End Property

  '// Print.LLExporterName
  Dim _LLExporterName As String
  Public Property LLExporterName() As String
    Get
      Return _LLExporterName
    End Get
    Set(ByVal value As String)
      _LLExporterName = value
    End Set
  End Property

  '// Print.LLExporterFileName
  Dim _LLExporterFileName As String
  Public Property LLExporterFileName() As String
    Get
      Return _LLExporterFileName
    End Get
    Set(ByVal value As String)
      _LLExporterFileName = value
    End Set
  End Property

#End Region


#Region "Properties US Private Daten..."

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

  '// USHomepage (= Homepage des Personalvermittlers)
  Dim _USHomepage As String = String.Empty
  Public Property USHomepage() As String
    Get
      Return _USHomepage
    End Get
    Set(ByVal value As String)
      _USHomepage = value
    End Set
  End Property

  '// USNatel (= USNatel des Personalvermittlers)
  Dim _USNatel As String = String.Empty
  Public Property USNatel() As String
    Get
      Return _USNatel
    End Get
    Set(ByVal value As String)
      _USNatel = value
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

  '// USAbteilung (= USAbteilung des Personalvermittlers)
  Dim _USAbteilung As String = String.Empty
  Public Property USAbteilung() As String
    Get
      Return _USAbteilung
    End Get
    Set(ByVal value As String)
      _USAbteilung = value
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
      Return _USPostfach
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

#End Region


#Region "Properties US-MD Daten..."

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

  '// MDName2 (= MDName2 des Personalvermittlers)
  Dim _USMDname2 As String = String.Empty
  Public Property USMDname2() As String
    Get
      Return _USMDname2
    End Get
    Set(ByVal value As String)
      _USMDname2 = value
    End Set
  End Property

  '// MDName3 (= MDName3 des Personalvermittlers)
  Dim _USMDname3 As String = String.Empty
  Public Property USMDname3() As String
    Get
      Return _USMDname3
    End Get
    Set(ByVal value As String)
      _USMDname3 = value
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

#End Region


#Region "Properties Kundendaten"
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

#End Region


#Region "Properties Vorschlagdaten..."

  Dim _PKDNr As Integer = 0
  Public Property PKDNr() As Integer
    Get
      Return _PKDNr
    End Get
    Set(ByVal value As Integer)
      _PKDNr = value
    End Set
  End Property

  Dim _PKDzNr As Integer = 0
  Public Property PKDzNr() As Integer
    Get
      Return _PKDzNr
    End Get
    Set(ByVal value As Integer)
      _PKDzNr = value
    End Set
  End Property

  Dim _PMANr As Integer = 0
  Public Property PMANr() As Integer
    Get
      Return _PMANr
    End Get
    Set(ByVal value As Integer)
      _PMANr = value
    End Set
  End Property

  Dim _PBez As String = String.Empty
  Public Property PBez() As String
    Get
      Return _PBez
    End Get
    Set(ByVal value As String)
      _PBez = value
    End Set
  End Property

  Dim _PArbbegin As String = String.Empty
  Public Property PArbBegin() As String
    Get
      Return _PArbbegin
    End Get
    Set(ByVal value As String)
      _PArbbegin = value
    End Set
  End Property

  Dim _PESAls As String = String.Empty
  Public Property PESAls() As String
    Get
      Return _PESAls
    End Get
    Set(ByVal value As String)
      _PESAls = value
    End Set
  End Property

  Dim _PKDTarif As String = String.Empty
  Public Property PKDTarif() As String
    Get
      Return _PKDTarif
    End Get
    Set(ByVal value As String)
      _PKDTarif = value
    End Set
  End Property

  Dim _PBerater1 As String = String.Empty
  Public Property PBerater1() As String
    Get
      Return _PBerater1
    End Get
    Set(ByVal value As String)
      _PBerater1 = value
    End Set
  End Property

  Dim _PBerater2 As String = String.Empty
  Public Property PBerater2() As String
    Get
      Return _PBerater2
    End Get
    Set(ByVal value As String)
      _PBerater2 = value
    End Set
  End Property

  Dim _PBemerkung As String = String.Empty
  Public Property PBemerkung() As String
    Get
      Return _PBemerkung
    End Get
    Set(ByVal value As String)
      _PBemerkung = value
    End Set
  End Property

  Dim _PSpesen As String = String.Empty
  Public Property PSpesen() As String
    Get
      Return _PSpesen
    End Get
    Set(ByVal value As String)
      _PSpesen = value
    End Set
  End Property


  Dim _PZusatz1 As String = String.Empty
  Public Property PZusatz1() As String
    Get
      Return _PZusatz1
    End Get
    Set(ByVal value As String)
      _PZusatz1 = value
    End Set
  End Property

  Dim _PZusatz2 As String = String.Empty
  Public Property PZusatz2() As String
    Get
      Return _PZusatz2
    End Get
    Set(ByVal value As String)
      _PZusatz2 = value
    End Set
  End Property

  Dim _PZusatz3 As String = String.Empty
  Public Property PZusatz3() As String
    Get
      Return _PZusatz3
    End Get
    Set(ByVal value As String)
      _PZusatz3 = value
    End Set
  End Property

  Dim _PZusatz4 As String = String.Empty
  Public Property PZusatz4() As String
    Get
      Return _PZusatz4
    End Get
    Set(ByVal value As String)
      _PZusatz4 = value
    End Set
  End Property

  Dim _PZusatz5 As String = String.Empty
  Public Property PZusatz5() As String
    Get
      Return _PZusatz5
    End Get
    Set(ByVal value As String)
      _PZusatz5 = value
    End Set
  End Property

#End Region


#Region "Properties Kandidaten Massenversandfelder..."

  '// Versandfeld
  Dim _MARes1 As String = String.Empty
  Public Property MARes1() As String
    Get
      Return _MARes1
    End Get
    Set(ByVal value As String)
      _MARes1 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes2 As String = String.Empty
  Public Property MARes2() As String
    Get
      Return _MARes2
    End Get
    Set(ByVal value As String)
      _MARes2 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes3 As String = String.Empty
  Public Property MARes3() As String
    Get
      Return _MARes3
    End Get
    Set(ByVal value As String)
      _MARes3 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes4 As String = String.Empty
  Public Property MARes4() As String
    Get
      Return _MARes4
    End Get
    Set(ByVal value As String)
      _MARes4 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes5 As String = String.Empty
  Public Property MARes5() As String
    Get
      Return _MARes5
    End Get
    Set(ByVal value As String)
      _MARes5 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes6 As String = String.Empty
  Public Property MARes6() As String
    Get
      Return _MARes6
    End Get
    Set(ByVal value As String)
      _MARes6 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes7 As String = String.Empty
  Public Property MARes7() As String
    Get
      Return _MARes7
    End Get
    Set(ByVal value As String)
      _MARes7 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes8 As String = String.Empty
  Public Property MARes8() As String
    Get
      Return _MARes8
    End Get
    Set(ByVal value As String)
      _MARes8 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes9 As String = String.Empty
  Public Property MARes9() As String
    Get
      Return _MARes9
    End Get
    Set(ByVal value As String)
      _MARes9 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes10 As String = String.Empty
  Public Property MARes10() As String
    Get
      Return _MARes10
    End Get
    Set(ByVal value As String)
      _MARes10 = value
    End Set
  End Property

  '// Versandfeld
  Dim _MARes11 As String = String.Empty
  Public Property MARes11() As String
    Get
      Return _MARes11
    End Get
    Set(ByVal value As String)
      _MARes11 = value
    End Set
  End Property
  '// Versandfeld
  Dim _MARes12 As String = String.Empty
  Public Property MARes12() As String
    Get
      Return _MARes12
    End Get
    Set(ByVal value As String)
      _MARes12 = value
    End Set
  End Property
  '// Versandfeld
  Dim _MARes13 As String = String.Empty
  Public Property MARes13() As String
    Get
      Return _MARes13
    End Get
    Set(ByVal value As String)
      _MARes13 = value
    End Set
  End Property
  '// Versandfeld
  Dim _MARes14 As String = String.Empty
  Public Property MARes14() As String
    Get
      Return _MARes14
    End Get
    Set(ByVal value As String)
      _MARes14 = value
    End Set
  End Property
  '// Versandfeld
  Dim _MARes15 As String = String.Empty
  Public Property MARes15() As String
    Get
      Return _MARes15
    End Get
    Set(ByVal value As String)
      _MARes15 = value
    End Set
  End Property

#End Region


#Region "Properties Kandidatendaten..."

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

  '// Anrede des Kandidaten
  Dim _MAGeschlecht As String = String.Empty
  Public Property MAGeschlecht() As String
    Get
      Return _MAGeschlecht
    End Get
    Set(ByVal value As String)
      _MAGeschlecht = value
    End Set
  End Property

  '// Kandidaten Geburtsdatum
  Dim _MAGebDat As String = String.Empty
  Public Property MAGebDat() As String
    Get
      Return _MAGebDat
    End Get
    Set(ByVal value As String)
      _MAGebDat = value
    End Set
  End Property

  '// Alter von Kandidaten
  Dim _MAAlter As String = String.Empty
  Public Property MAAlter() As String
    Get
      Return _MAAlter
    End Get
    Set(ByVal value As String)
      _MAAlter = value
    End Set
  End Property

  Dim _MABeruf As String = String.Empty
  Public Property MABeruf() As String
    Get
      Return _MABeruf
    End Get
    Set(ByVal value As String)
      _MABeruf = value
    End Set
  End Property

#End Region


#Region "Properties Sonstigedaten..."

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


  Function ParseTemplateFile(ByVal FullFileName As DevExpress.XtraRichEdit.RichEditControl) As String
    Dim ParsedFile As String = FullFileName.RtfText + vbCrLf
    Dim pattern As String = String.Empty
    Dim bLocAsHtml As Boolean = True

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

    Const REGEX_MA_Nachname As String = "\{(?i)TMPL_VAR name=\'P_MANachname\'\}"
    Const REGEX_MA_Vorname As String = "\{(?i)TMPL_VAR name=\'P_MAVorname\'\}"
    Const REGEX_MA_Geschlecht As String = "\{(?i)TMPL_VAR name=\'P_MAGeschlecht\'\}"
    Const REGEX_MA_Anrede As String = "\{(?i)TMPL_VAR name=\'P_MAAnrede\'\}"
    Const REGEX_MA_GebDat As String = "\{(?i)TMPL_VAR name=\'P_MAGebDat\'\}"
    Const REGEX_MA_Alter As String = "\{(?i)TMPL_VAR name=\'P_MAAlter\'\}"
    Const REGEX_MA_Beruf As String = "\{(?i)TMPL_VAR name=\'P_MABeruf\'\}"

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
        result_0.Replace(_Firma1)
      End While

      myRegEx_0 = New Regex(REGEX_KDzANREDEFORM)
      result_0 = FullFileName.Document.StartSearch(myRegEx_0)
      While result_0.FindNext()
        result_0.Replace(_KDzAnredeform)
      End While

      myRegEx_0 = New Regex(REGEX_KDzANREDE)
      result_0 = FullFileName.Document.StartSearch(myRegEx_0)
      While result_0.FindNext()
        result_0.Replace(_KDzAnrede)
      End While

      myRegEx_0 = New Regex(REGEX_KDzNachname)
      result_0 = FullFileName.Document.StartSearch(myRegEx_0)
      While result_0.FindNext()
        result_0.Replace(_ZhdNachname)
      End While

      myRegEx_0 = New Regex(REGEX_KDzVorname)
      result_0 = FullFileName.Document.StartSearch(myRegEx_0)
      While result_0.FindNext()
        result_0.Replace(_ZhdVorname)
      End While

      myRegEx_0 = New Regex(REGEX_GANZE_ANREDE)
      result_0 = FullFileName.Document.StartSearch(myRegEx_0)
      If result_0.FindNext() Then
        Dim strValue As String = String.Format("{0} {1}", _KDzAnredeform, _ZhdNachname)
				If strValue.Contains(m_Translate.GetSafeTranslationValue("liebe")) Then
					strValue = String.Format("{0} {1}", _KDzAnredeform, _ZhdVorname)

				ElseIf strValue.Contains(_ZhdNachname) Or strValue.Contains(_ZhdVorname) Then
					strValue = String.Format("{0}", _KDzAnredeform)

				End If
				result_0.Replace(strValue)
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
				result_0.Replace(m_Translate.GetSafeTranslationValue(If(_MAGeschlecht = "M", "Herr", "Frau")))
			End While

      myRegEx_0 = New Regex(REGEX_MA_GebDat)
      result_0 = FullFileName.Document.StartSearch(myRegEx_0)
      While result_0.FindNext()
        result_0.Replace(_MAGebDat)
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
        result_0.Replace(String.Empty) '_MARes15)
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
        result_0.Replace(ClsDataDetail.GetProposalZHDNr)
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
        result_0.Replace(_PArbbegin)
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
        result_0.Replace(String.Empty) 'CType(_PZusatz4, RichTextBox))
        FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _PZusatz4)
      End While

      myRegEx_0 = New Regex(REGEX_PZusatz5)
      result_0 = FullFileName.Document.StartSearch(myRegEx_0)
      While result_0.FindNext()
        result_0.Replace(String.Empty) '_PZusatz5)
        FullFileName.Document.InsertRtfText(result_0.CurrentResult.Start, _PZusatz5)
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

      myRegEx_0 = New Regex(REGEX_USPLZ)
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





      If _bBodyAsHtml And bLocAsHtml Then ParsedFile = SetSyntax(ParsedFile)

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
    str1 = Replace(str1, Chr(252), "&uuml;")      ' ü
    str1 = Replace(str1, Chr(129), "&uuml;")      ' ü

    SetSyntax = str1

    Console.WriteLine("Str1/2: " & str1)

  End Function

End Class

Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


  Private m_md As Mandant
  Private m_utility As Utilities



  ''' <summary>
  ''' listet eine Auflistung der Mandantendaten
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function LoadMandantenData() As IEnumerable(Of MandantenData)
    Dim m_utility As New Utilities
    Dim result As List(Of MandantenData) = Nothing
    m_md = New Mandant

    Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim recData As New MandantenData

					recData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					recData.MDName = m_utility.SafeGetString(reader, "MDName")
					recData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					recData.MDConnStr = m_md.GetSelectedMDData(recData.MDNr).MDDbConn

					result.Add(recData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function


#Region "Funktionen für Speichern der Daten..."

	'// RecNr
	Public Property GetRecNr() As Integer

	Public Property GetMDNr As Integer?
	'// MANr
	Public Property GetMANr() As Integer?

	'// KDNr
	Public Property GetKDNr() As Integer?

	'// KDZHDNr
	Public Property GetKDZHDNr() As Integer?

	'// VakNr
	Public Property GetVakNr() As Integer?
	Public Property ApplicationNumber As Integer?

	'// Bezeichnung
	Public Property GetBezeichnung() As String

	'// KST
	Public Property GetP_KST_1() As String

	'// Berater 
	Public Property GetBerater_1() As String

	'// KST
	Public Property GetP_KST_2() As String

	'// Berater 
	Public Property GetBerater_2() As String

	'// Status
	Public Property GetP_State() As String

	'// Art
	Public Property GetP_Art() As String

	'// Tarif
	Public Property GetKD_Tarif() As String

	'// Arbeitszeit
	Public Property GetP_ArbZeit() As String

	'// Spesen
	Public Property GetP_Spesen() As String

	'// Anstellung
	Public Property GetAnstellung() As String

	'// ArbBegin
	Public Property GetArbBegin() As String

	'// ArbBegin Per Datum
	Public Property GetArbbegin_Date() As Date?

	'// Anstellung als
	Public Property GetAb_AnstellungAls() As String

	'// Antritt per
	Public Property GetAb_AntrittPer() As String

	'// LohnBasis
	Public Property GetAb_LohnBasis() As Double?

	'// LohnAnzahl
	Public Property GetAb_LohnAnzahl() As Double?

	'// LohnBetrag
	Public Property GetAb_LohnBetrag() As Double?

	'// HBasis
	Public Property GetAb_HBasis() As Double?

	'// HAnsatz
	Public Property GetAb_HAnsatz() As Double?

	'// HBetrag
	Public Property GetAb_HBetrag() As Double?

	'// Verrechnung Per
	Public Property GetAb_RePer() As String

	'// Verrechnung Per Datum
	Public Property GetAb_RePer_Date() As Date?

	'// Abschluss Bemerkung
	Public Property GetAb_Bemerkung() As String

#End Region

	Function GetJobNr4Print(ByVal sListArt As Short) As String
		Dim strModul2print As String = String.Empty

		strModul2print = "18.0"

		Return strModul2print
	End Function


#Region "Datensatz bearbeiten..."

	Function GetSQLString(ByVal iRecNr As Integer) As String
		Dim strSQL As String = String.Empty

		If iRecNr <> 0 Then
			strSQL = "Update Propose Set Bezeichnung = @Bez, VakNr = @VakNr, ApplicationNumber = @ApplicationNumber,"
			strSQL &= "P_State = @P_State, P_Art = @P_Art, "
			strSQL &= "KD_Tarif = @KD_Tarif, P_ArbZeit = @P_ArbZeit, P_Spesen = @P_Spesen, "
			strSQL &= "P_Anstellung = @P_Anstellung, P_ArbBegin = @P_ArbBegin, P_ArbBegin_Date = @P_ArbBegin_Date, "

			If ClsDataDetail.bAllowedToChange Then
				strSQL &= "KST = @KST, MA_Kst = @MA_Kst, KD_Kst = @KD_Kst, "
				strSQL &= "Berater = @Berater, MANr = @MANr, KDNr = @KDNr, KDZHDNr = @KDZHDNr, "
			End If

			strSQL &= "Ab_AnstellungAls = @Ab_AnstellungAls "
			strSQL &= ", Ab_AntrittPer = @Ab_AntrittPer "
			strSQL &= ", Ab_LohnBas = @Ab_LohnBasis "
			strSQL &= ", Ab_LohnAnz = @Ab_LohnAnzahl "
			strSQL &= ", Ab_LohnBetrag = @Ab_LohnBetrag "
			strSQL &= ", Ab_HBas = @Ab_HBasis "
			strSQL &= ", Ab_HAns = @Ab_HAnsatz"
			strSQL &= ", Ab_HBetrag = @Ab_HBetrag "
			strSQL &= ", Ab_RePer = @Ab_RePer "
			strSQL &= ", Ab_RePer_Date = @Ab_RePer_Date "
			strSQL &= ", Ab_Bemerkung = @Ab_Bemerkung "
			strSQL &= ", MDNr = @MDNr, "

			strSQL &= "ChangedOn = @ChangedOn, ChangedFrom = @ChangedFrom "
			strSQL &= "Where ProposeNr = @RecNr "

		Else
			strSQL = "Insert Into Propose (ProposeNr, MANr, KDNr, KDZHDNr, VakNr, ApplicationNumber, "
			strSQL &= "KST, MA_Kst, KD_Kst, Berater, Bezeichnung, P_State, "
			strSQL &= "P_Art, KD_Tarif, P_ArbZeit, P_Spesen, P_Anstellung, P_ArbBegin, P_ArbBegin_Date, "

			strSQL &= "Ab_AnstellungAls, Ab_AntrittPer, "
			strSQL &= "Ab_LohnBas, Ab_LohnAnz, Ab_LohnBetrag, "
			strSQL &= "Ab_HBas, Ab_HAns, Ab_HBetrag, "
			strSQL &= "Ab_RePer, Ab_RePer_Date, Ab_Bemerkung, MDNr, "

			strSQL &= "CreatedOn, CreatedFrom) Values ("

			strSQL &= "@RecNr, @MANr, @KDNr, @KDZHDNr, @VakNr, @ApplicationNumber, "
			strSQL &= "@KST, @MA_Kst, @KD_Kst, @Berater, @Bezeichnung, @P_State, "
			strSQL &= "@P_Art, @KD_Tarif, @P_ArbZeit, @P_Spesen, @P_Anstellung, @P_ArbBegin, @P_ArbBegin_Date, "

			strSQL &= "@Ab_AnstellungAls, @Ab_AntrittPer, "
			strSQL &= "@Ab_LohnBasis, @Ab_LohnAnzahl, @Ab_LohnBetrag, "
			strSQL &= "@Ab_HBasis, @Ab_HAnsatz, @Ab_HBetrag, "
			strSQL &= "@Ab_RePer, @Ab_RePer_Date, @Ab_Bemerkung, @MDNr, "

			strSQL &= "@CreatedOn, @CreatedFrom)"

		End If

		ClsDataDetail.SQLQuery = strSQL

		Return strSQL
	End Function

	Function SaveDataToProposeDb(ByVal iRecNr As Integer) As Boolean
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strQuery As String = GetSQLString(iRecNr)
		Dim iNewRecNr As Integer = 0
		Dim bAsNewrec As Boolean = iRecNr = 0
		Dim bResult As Boolean

		If Not bAsNewrec Then
			iNewRecNr = iRecNr

		Else
			iNewRecNr = GetNewRecNr()

		End If

		Dim i As Integer = 0
		Dim _ClsDb As New ClsDbFunc
		Me.GetRecNr = iNewRecNr

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			If Not bAsNewrec Then
				param = cmd.Parameters.AddWithValue("@Bez", ReplaceMissing(Me.GetBezeichnung, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@VakNr", ReplaceMissing(Me.GetVakNr, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@ApplicationNumber", ReplaceMissing(ApplicationNumber, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@P_State", Me.GetP_State)
				param = cmd.Parameters.AddWithValue("@P_Art", Me.GetP_Art)
				param = cmd.Parameters.AddWithValue("@KD_Tarif", Me.GetKD_Tarif)
				param = cmd.Parameters.AddWithValue("@P_ArbZeit", Me.GetP_ArbZeit)
				param = cmd.Parameters.AddWithValue("@P_Spesen", Me.GetP_Spesen)

				param = cmd.Parameters.AddWithValue("@P_Anstellung", Me.GetAnstellung)
				param = cmd.Parameters.AddWithValue("@P_ArbBegin", Me.GetArbBegin)
				param = cmd.Parameters.AddWithValue("@P_ArbBegin_Date", ReplaceMissing(Me.GetArbbegin_Date, DBNull.Value))

				If ClsDataDetail.bAllowedToChange Then
					param = cmd.Parameters.AddWithValue("@KST", String.Format("{0}{1}{2}", Me.GetP_KST_1, _
																																		If(Me.GetP_KST_1.ToLower = Me.GetP_KST_2.ToLower, "", "/"), _
																																		If(Me.GetP_KST_1.ToLower = Me.GetP_KST_2.ToLower, "", _
																																			 Me.GetP_KST_2)))

					param = cmd.Parameters.AddWithValue("@MA_KST", String.Format("{0}", Me.GetP_KST_1))
					param = cmd.Parameters.AddWithValue("@KD_KST", String.Format("{0}", Me.GetP_KST_2))
					param = cmd.Parameters.AddWithValue("@Berater", String.Format("{0}{1}{2}", Me.GetBerater_1, _
																																		If(Me.GetBerater_1.ToLower = Me.GetBerater_2.ToLower, _
																																			 "", "/"), _
																																		If(Me.GetBerater_1.ToLower = Me.GetBerater_2.ToLower, "", _
																																			 Me.GetBerater_2)))
					param = cmd.Parameters.AddWithValue("@MANr", ReplaceMissing(Me.GetMANr, DBNull.Value))
					param = cmd.Parameters.AddWithValue("@KDNr", ReplaceMissing(Me.GetKDNr, DBNull.Value))
					param = cmd.Parameters.AddWithValue("@KDZHDNr", ReplaceMissing(Me.GetKDZHDNr, DBNull.Value))
				End If

				param = cmd.Parameters.AddWithValue("@Ab_AnstellungAls", Me.GetAb_AnstellungAls)
				param = cmd.Parameters.AddWithValue("@Ab_AntrittPer", Me.GetAb_AntrittPer)

				param = cmd.Parameters.AddWithValue("@Ab_LohnBasis", Me.GetAb_LohnBasis)
				param = cmd.Parameters.AddWithValue("@Ab_LohnAnzahl", Me.GetAb_LohnAnzahl)
				param = cmd.Parameters.AddWithValue("@Ab_LohnBetrag", Me.GetAb_LohnBetrag)
				param = cmd.Parameters.AddWithValue("@Ab_HBasis", Me.GetAb_HBasis)
				param = cmd.Parameters.AddWithValue("@Ab_HAnsatz", Me.GetAb_HAnsatz)
				param = cmd.Parameters.AddWithValue("@Ab_HBetrag", Me.GetAb_HBetrag)

				param = cmd.Parameters.AddWithValue("@Ab_RePer", ReplaceMissing(Me.GetAb_RePer, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@Ab_RePer_Date", ReplaceMissing(Me.GetAb_RePer_Date, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@Ab_Bemerkung", ReplaceMissing(Me.GetAb_Bemerkung, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@MDNr", ReplaceMissing(GetMDNr, m_InitialData.MDData.MDNr))

				param = cmd.Parameters.AddWithValue("@ChangedOn", Format(Now, "g"))
				param = cmd.Parameters.AddWithValue("@ChangedFrom", String.Format("{0}", m_InitialData.UserData.UserFullName))

				param = cmd.Parameters.AddWithValue("@RecNr", Me.GetRecNr)

			Else
				param = cmd.Parameters.AddWithValue("@RecNr", ReplaceMissing(Me.GetRecNr, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@MANr", ReplaceMissing(Me.GetMANr, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@KDNr", ReplaceMissing(Me.GetKDNr, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@KDZHDNr", ReplaceMissing(Me.GetKDZHDNr, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@VakNr", ReplaceMissing(Me.GetVakNr, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@ApplicationNumber", ReplaceMissing(ApplicationNumber, DBNull.Value))

				param = cmd.Parameters.AddWithValue("@KST", String.Format("{0}{1}{2}", Me.GetP_KST_1, _
																													If(Me.GetP_KST_1.ToLower = Me.GetP_KST_2.ToLower, "", "/"), _
																													If(Me.GetP_KST_1.ToLower = Me.GetP_KST_2.ToLower, "", _
																														 Me.GetP_KST_2)))
				param = cmd.Parameters.AddWithValue("@MA_KST", String.Format("{0}", Me.GetP_KST_1))
				param = cmd.Parameters.AddWithValue("@KD_KST", String.Format("{0}", Me.GetP_KST_2))
				param = cmd.Parameters.AddWithValue("@Berater", String.Format("{0}{1}{2}", Me.GetBerater_1, _
																																	If(Me.GetBerater_1.ToLower = Me.GetBerater_2.ToLower, _
																																		 "", "/"), _
																																	If(Me.GetBerater_1.ToLower = Me.GetBerater_2.ToLower, "", _
																																		 Me.GetBerater_2)))

				param = cmd.Parameters.AddWithValue("@Bezeichnung", Me.GetBezeichnung)
				param = cmd.Parameters.AddWithValue("@P_State", Me.GetP_State)
				param = cmd.Parameters.AddWithValue("@P_Art", Me.GetP_Art)
				param = cmd.Parameters.AddWithValue("@KD_Tarif", Me.GetKD_Tarif)
				param = cmd.Parameters.AddWithValue("@P_ArbZeit", Me.GetP_ArbZeit)
				param = cmd.Parameters.AddWithValue("@P_Spesen", Me.GetP_Spesen)

				param = cmd.Parameters.AddWithValue("@P_Anstellung", Me.GetAnstellung)
				param = cmd.Parameters.AddWithValue("@P_ArbBegin", Me.GetArbBegin)
				param = cmd.Parameters.AddWithValue("@P_ArbBegin_Date", ReplaceMissing(Me.GetArbbegin_Date, DBNull.Value))

				param = cmd.Parameters.AddWithValue("@Ab_AnstellungAls", Me.GetAb_AnstellungAls)
				param = cmd.Parameters.AddWithValue("@Ab_AntrittPer", Me.GetAb_AntrittPer)

				param = cmd.Parameters.AddWithValue("@Ab_LohnBasis", Me.GetAb_LohnBasis)
				param = cmd.Parameters.AddWithValue("@Ab_LohnAnzahl", Me.GetAb_LohnAnzahl)
				param = cmd.Parameters.AddWithValue("@Ab_LohnBetrag", Me.GetAb_LohnBetrag)
				param = cmd.Parameters.AddWithValue("@Ab_HBasis", Me.GetAb_HBasis)
				param = cmd.Parameters.AddWithValue("@Ab_HAnsatz", Me.GetAb_HAnsatz)
				param = cmd.Parameters.AddWithValue("@Ab_HBetrag", Me.GetAb_HBetrag)

				param = cmd.Parameters.AddWithValue("@Ab_RePer", ReplaceMissing(Me.GetAb_RePer, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@Ab_RePer_Date", ReplaceMissing(Me.GetAb_RePer_Date, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@Ab_Bemerkung", ReplaceMissing(Me.GetAb_Bemerkung, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@MDNr", ReplaceMissing(GetMDNr, m_InitialData.MDData.MDNr))

				param = cmd.Parameters.AddWithValue("@CreatedOn", Format(Now, "g"))
				param = cmd.Parameters.AddWithValue("@CreatedFrom", String.Format("{0}", m_InitialData.UserData.UserFullName))

			End If

			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()
			bResult = True

		Catch ex As SqlException
			MsgBox(ex.Message, MsgBoxStyle.Critical, "SaveDataToProposeDb_1")
			Me.GetRecNr = If(bAsNewrec, 0, iNewRecNr)
			bResult = False

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "SaveDataToProposeDb_0")
			Me.GetRecNr = If(bAsNewrec, 0, iNewRecNr)
			bResult = False

		End Try

		Return bResult
	End Function

	Function GetSQLString4KontaktDb(ByVal b4KD As Boolean) As String
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
			strSQL = "Insert Into MA_Kontakte (MANr, KDNr, KontaktDate,Kontakte, RecNr, "
			strSQL &= "KontaktType1, KontaktType2, "
			strSQL &= "KontaktDauer, KontaktWichtig, KontaktErledigt, ProposeNr, VakNr, OfNr, Mail_ID, "
			strSQL &= "CreatedOn, CreatedFrom) Values "

			strSQL &= "(@MANr, @KDNr, @KontaktDate, @Kontakte, @RecNr, "
			strSQL &= "@KontaktType1, @KontaktType2, "
			strSQL &= "@KontaktDauer, @KontaktWichtig, @KontaktErledigt, @ProposeNr, @VakNr, @OfNr, @Mail_ID, "
			strSQL &= "@Createdon, @CreatedFrom)"

		End If

		Return strSQL
	End Function

	'Function SaveDataToKontaktDb(ByVal b4KD As Boolean) As Boolean
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim strQuery As String = GetSQLString4KontaktDb(b4KD)
	'	Dim iNewRecNr As Integer = 0
	'	Dim bAsNewrec As Boolean = True
	'	Dim bResult As Boolean
	'	Dim i As Integer = 0
	'	Dim _ClsDb As New ClsDbFunc

	'	iNewRecNr = GetNewKontaktRecNr(b4KD)

	'	Try
	'		Conn.Open()
	'		Dim cmd As System.Data.SqlClient.SqlCommand
	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
	'		Dim param As System.Data.SqlClient.SqlParameter

	'		If b4KD Then
	'			param = cmd.Parameters.AddWithValue("@KDNr", Me.GetKDNr)
	'			param = cmd.Parameters.AddWithValue("@KDZNr", ReplaceMissing(Me.GetKDZHDNr, DBNull.Value))

	'			param = cmd.Parameters.AddWithValue("@KontaktDate", Format(Now, "g"))
	'			param = cmd.Parameters.AddWithValue("@Kontakte", m_Translate.GetSafeTranslationValue("Vorschlagstatus wurde geändert."))
	'			param = cmd.Parameters.AddWithValue("@RecNr", iNewRecNr)

	'			param = cmd.Parameters.AddWithValue("@KontaktType1", "Information")
	'			param = cmd.Parameters.AddWithValue("@KontaktType2", "1")
	'			param = cmd.Parameters.AddWithValue("@KontaktDauer", m_Translate.GetSafeTranslationValue("Änderung der Status"))
	'			param = cmd.Parameters.AddWithValue("@KontaktWichtig", "1")
	'			param = cmd.Parameters.AddWithValue("@KontaktErledigt", "0")

	'			param = cmd.Parameters.AddWithValue("@MANr", Me.GetMANr)
	'			param = cmd.Parameters.AddWithValue("@ProposeNr", Me.GetRecNr)
	'			param = cmd.Parameters.AddWithValue("@VakNr", ReplaceMissing(Me.GetVakNr, DBNull.Value))
	'			param = cmd.Parameters.AddWithValue("@OfNr", 0)
	'			param = cmd.Parameters.AddWithValue("@Mail_ID", 0)

	'			param = cmd.Parameters.AddWithValue("@CreatedOn", Format(Now, "g"))
	'			param = cmd.Parameters.AddWithValue("@CreatedFrom", String.Format("{0}", m_InitialData.UserData.UserFullName))

	'		Else
	'			param = cmd.Parameters.AddWithValue("@MANr", Me.GetMANr)
	'			param = cmd.Parameters.AddWithValue("@KDNr", Me.GetKDNr)

	'			param = cmd.Parameters.AddWithValue("@KontaktDate", Format(Now, "g"))
	'			param = cmd.Parameters.AddWithValue("@Kontakte", m_Translate.GetSafeTranslationValue("Vorschlagstatus wurde geändert."))
	'			param = cmd.Parameters.AddWithValue("@RecNr", iNewRecNr)

	'			param = cmd.Parameters.AddWithValue("@KontaktType1", "Information")
	'			param = cmd.Parameters.AddWithValue("@KontaktType2", "1")
	'			param = cmd.Parameters.AddWithValue("@KontaktDauer", m_Translate.GetSafeTranslationValue("Änderung der Status"))
	'			param = cmd.Parameters.AddWithValue("@KontaktWichtig", "1")
	'			param = cmd.Parameters.AddWithValue("@KontaktErledigt", "0")

	'			param = cmd.Parameters.AddWithValue("@ProposeNr", Me.GetRecNr)
	'			param = cmd.Parameters.AddWithValue("@VakNr", ReplaceMissing(Me.GetVakNr, DBNull.Value))
	'			param = cmd.Parameters.AddWithValue("@OfNr", 0)
	'			param = cmd.Parameters.AddWithValue("@Mail_ID", 0)

	'			param = cmd.Parameters.AddWithValue("@CreatedOn", Format(Now, "g"))
	'			param = cmd.Parameters.AddWithValue("@CreatedFrom", String.Format("{0}", m_InitialData.UserData.UserFullName))

	'		End If


	'		cmd.ExecuteNonQuery()
	'		cmd.Parameters.Clear()
	'		bResult = True

	'	Catch ex As Exception
	'		MsgBox(ex.Message, MsgBoxStyle.Critical, "SaveDataToProposeDb_0")
	'		Me.GetRecNr = iNewRecNr
	'		bResult = False

	'	End Try


	'	Return bResult
	'End Function

	Function GetNewRecNr() As Integer
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iResult As Integer = 1
		Dim strQuery As String = "Select Top 1 ProposeNr From Propose Order By ProposeNr Desc"
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand
		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
		Dim rKDrec As SqlDataReader = cmd.ExecuteReader					 ' Datenbank


		Try
			While rKDrec.Read
				iResult += CInt(rKDrec("ProposeNr").ToString)
			End While
			If Not rKDrec.HasRows Then iResult = 1

		Catch ex As Exception


		End Try


		Return iResult
	End Function

	'Function GetNewKontaktRecNr(ByVal b4KD As Boolean) As Integer
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim iResult As Integer = 1
	'	Dim strQuery As String = String.Format("Select Top 1 RecNr From {0} Order By RecNr Desc", _
	'																				If(b4KD, "KD_KontaktTotal", "MA_Kontakte"))
	'	Conn.Open()

	'	Dim cmd As System.Data.SqlClient.SqlCommand
	'	cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
	'	Dim rKDrec As SqlDataReader = cmd.ExecuteReader					 ' Datenbank


	'	Try
	'		While rKDrec.Read
	'			iResult += CInt(rKDrec("RecNr").ToString)
	'		End While
	'		If Not rKDrec.HasRows Then iResult = 1

	'	Catch ex As Exception


	'	End Try


	'	Return iResult
	'End Function

	Sub DeleteSelectedRec(ByVal iRecNr As Integer)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Dim strQuery As String = "[Delete Selected Proposerec]"

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@PNumber", iRecNr)
			param = cmd.Parameters.AddWithValue("@USInfo", String.Format("{0}", m_InitialData.UserData.UserFullName))

			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()


		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "DeleteSelectedRec_0")

		End Try


	End Sub

#End Region

#Region "Funktionen zur Suche nach Daten..."

	Function GetLocalSQLString(ByVal iRecNr As Integer, ByVal bSelectAsNew As Boolean) As String
		Dim sSql As String = String.Empty

		sSql = "Select Top 1 P.*, MA.Nachname As MANachname, MA.Vorname As MAVorname, KD.Firma1, "
		sSql &= "KDz.Nachname As KDzNachname, KDz.Vorname As KDzVorname, Vak.Bezeichnung As VakBez From Propose P "
		sSql &= "Left Join Mitarbeiter MA On P.MANr = MA.MANr "
		sSql &= "Left Join Kunden KD On P.KDNr = KD.KDNr "
		sSql &= "Left Join KD_Zustaendig KDz On P.KDZhdNr = KDz.RecNr And KD.KDNr = KDz.KDNr "
		sSql &= "Left Join Vakanzen Vak On P.VakNr = Vak.VakNr "

		If Not bSelectAsNew Then sSql &= "Where P.ProposeNr = @iRecNr "
		sSql &= "Order By P.ProposeNr"

		Return sSql
	End Function

	Function GetSQLString4Print(ByVal iProposeNr As Integer) As String
    Dim sSql As String = "[Get Propose Data 4 Print]"
    Return sSql
  End Function

  Function GetVorstellungSQLString(ByVal iRecNr As Integer) As String
    Dim sSql As String = String.Empty

    sSql = "Select Top 20 MAJt.* From MA_JobTermin MAJt "
    sSql &= "Where MAJt.ProposeNr = @iRecNr "
    sSql &= "Order By MAJt.TerminDate Desc"

    Return sSql
  End Function

  Function GetMAKontaktSQLString(ByVal iRecNr As Integer) As String
    Dim sSql As String = String.Empty

    sSql = "Select Top 20 MAK.* From MA_Kontakte MAK "
    sSql &= "Where MAK.ProposeNr = @iRecNr "
    sSql &= "Order By MAK.KontaktDate Desc"

    Return sSql
  End Function

  Function GetKDKontaktSQLString(ByVal iRecNr As Integer) As String
    Dim sSql As String = String.Empty

    sSql = "Select Top 20 KDK.* From KD_KontaktTotal KDK "
    sSql &= "Where KDK.ProposeNr = @iRecNr "
    sSql &= "Order By KDK.KontaktDate Desc"

    Return sSql
  End Function

  Function GetESSQLString(ByVal iRecNr As Integer) As String
    Dim sSql As String = String.Empty

    sSql = "Select Top 1 RE.RENr, ES.ESNr from RE Left Join ES on RE.ProposeNr = ES.ProposeNr "
    sSql &= "Where RE.ProposeNr = @iRecNr"

    Return sSql
  End Function

  Function GetUserSQLString() As String
    Dim sSql As String = String.Empty

    sSql = "[Get All User eMail 4 Propose]"

    Return sSql
  End Function

  Function GetMASQLString(ByVal iMANr As Integer) As String
    Dim sSql As String = String.Empty

    sSql = "Select Top 1 MA.eMail As MAeMail, MA.Nachname As MANachname, MA.Vorname As MAVorname "
    sSql &= "From Mitarbeiter MA Where MA.MANr = @iMANr"

    Return sSql
  End Function

	Function GetFileInfoFromTempTable() As String
		Dim sSql As String = String.Empty

		sSql = "Select Top 1 * From _MADocuments_" & ClsDataDetail.GetProposalMANr & " Where RecID = @RecID"

		Return sSql
	End Function

	Function GetKDZHDSQLString(ByVal iKDNr As Integer, ByVal iZHDNr As Integer) As String
    Dim sSql As String = String.Empty

    If iZHDNr = 0 Then
      sSql = "Select Top 1 KD.eMail As KDeMail, KD.Firma1 As Firma1 "
      sSql &= "From Kunden KD Where KD.KDNr = @iKDNr"

    Else
      sSql = "Select Top 1 KD.eMail As KDeMail, KD.Firma1 As Firma1, "
      sSql &= "ZHD.Anrede As ZHDAnrede, ZHD.Anredeform As ZHDAnredeForm, "
      sSql &= "ZHD.Nachname As ZHDNachname, ZHD.Vorname As ZHDVorname, "
      sSql &= "ZHD.eMail As ZHDeMail "
      sSql &= "From Kunden KD Left Join KD_Zustaendig ZHD On KD.KDNr = ZHD.KDNr "
      sSql &= "Where KD.KDNr = @iKDNr And ZHD.RecNr = @iZHDNr"

    End If

    Return sSql
  End Function

  Function GetLstItems(ByVal lst As ListBox) As String
    Dim strBerufItems As String = String.Empty

    For i As Integer = 0 To lst.Items.Count - 1
      strBerufItems += lst.Items(i).ToString & "#@"
    Next

    Return Left(strBerufItems, Len(strBerufItems) - 2)
  End Function

#End Region


End Class


Public Class ClsLLFunc

#Region "Properties"

  Public Property LLDocName() As String
  Public Property LLDocLabel() As String
  Public Property LLFontDesent() As Integer
  Public Property LLIncPrv() As Integer
  Public Property LLParamCheck() As Integer
  Public Property LLKonvertName() As Integer
  Public Property LLZoomProz() As Integer
  Public Property LLCopyCount() As Integer
  Public Property LLExportedFilePath() As String
  Public Property LLExportedFileName() As String
  Public Property LLExportfilter() As String
  Public Property LLExporterName() As String
  Public Property LLExporterFileName() As String
  Public Property LLPrintInDiffColor() As Boolean
  Public Property ListBez() As String

#End Region


#Region "UserData..."

  Public Property iSelectedUSNr() As Integer
  Public Property USAnrede() As String
  Public Property USVorname() As String
  Public Property USNachname() As String

  Public Property USPostfach() As String
  Public Property USStrasse() As String
  Public Property USPLZ() As String
  Public Property USLand() As String
  Public Property USOrt() As String

  Public Property Exchange_USName() As String
  Public Property Exchange_USPW() As String
  Public Property strUSSignFilename() As String


  Public Property USTitel_1() As String
  Public Property USTitel_2() As String
  Public Property USAbteilung() As String
  Public Property USeMail() As String
  Public Property USTelefon() As String
  Public Property USTelefax() As String
	Public Property USNatel() As String

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


  Public Property GetTestSearchQuery() As String
  Public Property SelectedMAsBerufe() As String


	Public Property bvgmaximallohnjahr As Decimal?
	Public Property bvgkoordinationlohnjahr As Decimal?
	Public Property bvgkoordinationlohnstd As Decimal?
	Public Property bvgminmallohnjahr As Decimal?
	Public Property bvgstd As Decimal?
	Public Property bvgwoche As Integer?


#End Region

End Class

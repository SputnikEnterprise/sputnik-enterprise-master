Imports SP.DatabaseAccess.ESRUtility.DataObjects

Namespace ESRUtility

  ''' <summary>
  ''' Interface for ESRUtility database access.
  ''' </summary>
  Public Interface IESRUtilityDatabaseAccess

		''' <summary>
		''' Lädt die Bankdaten für einen Mandanten.
		''' </summary>
		''' <param name="mdNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
    Function LoadBankData(ByVal mdNr As Integer) As IList(Of DataObjects.BankData)

    ''' <summary>
    ''' Lädt die jährlichen Mandantdaten.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function LoadMandantYearData() As IList(Of DataObjects.MandantYearData)

		''' <summary>
		''' Lädt RE Daten für eine Rechnungsnummer.
		''' </summary>
		''' <param name="mdNr">Mandant Nr.</param>
		''' <param name="kdNr">Kunden Nr.</param>
		''' <param name="reNr">Rechnungs Nr.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function LoadReData(ByVal mdNr As Integer, ByVal kdNr As Integer?, ByVal reNr As Integer?) As ReData

		Function AddESRDataToPayment(mdNr As Integer, data As EsrRecord, ByVal zeNumberOffset As Integer) As Boolean
		Function AddESRDataToESRTable(mdNr As Integer, data As EsrRecord) As Boolean
		Function AddESRFileDataToDiskInfo(mdNr As Integer, data As EsrRecord, ByVal filebytes() As Byte) As Boolean

		Function IsESRFileAlreadySaved(mdNr As Integer, fileinfo As IO.FileInfo, filebytes As Byte()) As SavedESRData

	End Interface

End Namespace
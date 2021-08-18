Imports SP.DatabaseAccess.DTAUtility.DataObjects

Namespace DTAUtility

  ''' <summary>
  ''' Interface for DTAUtility database access.
  ''' </summary>
  Public Interface IDTAUtilityDatabaseAccess

		Function LoadBankData(mdNr As Integer) As List(Of DataObjects.BankData)

    ''' <summary>
    ''' Lädt DTA Auftragsdaten.
    ''' </summary>
    ''' <param name="dtaJobType">Auftragstyp.</param>
    ''' <param name="mdNr">Mandant Nr.</param>
    ''' <returns>Liste oder Nothing im Fehlerfall.</returns>
    ''' <remarks></remarks>
    Function LoadJobNumberData(ByVal dtaJobType As DTAUtilityDatabaseAccess.DtaJobTypeEnum, ByVal mdNr As Integer) As List(Of JobNumberData)

    ''' <summary>
    ''' Lädt Zahlungsdaten zum Erstellen von DTA Dateien.
    ''' </summary>
    ''' <param name="zgType">Zahlungstyp.</param>
    ''' <param name="mdNr">Mandant Nr.</param>
    ''' <returns>Liste oder Nothing im Fehlerfall.</returns>
    ''' <remarks></remarks>
    Function LoadZGADataForDTAFileCreation(ByVal zgType As DTAUtilityDatabaseAccess.ZgTypeEnum, ByVal mdNr As Integer) As List(Of ZgData)

    ''' <summary>
    ''' Lädt LOL Daten zum Erstellen von DTA Dateien.
    ''' </summary>
    ''' <param name="mdNr">Mandant Nr.</param>
    ''' <returns>Liste oder Nothing im Fehlerfall.</returns>
    Function LoadLolDataForDtaFileCreation(ByVal mdNr As Integer) As List(Of LolDataForDtaFileCreation)
    Function LoadMandantAndBankDataForDTAFileCreation(ByVal mdNr As Integer, ByVal year As Integer, ByVal bankRecNr As Integer) As MandantAndBankDataForDTAFileCreation
    Function LoadIndZGData(ByVal mdNr As Integer, ByVal InlandBank As Integer, ByVal zgNumbers As Integer()) As IEnumerable(Of ZgData)
    Function GetNewDtaNr(ByVal offset As Integer) As Integer?
		Function UpdateZGrecForDTAFile(ByVal zgNr As Integer, ByVal dtaNr As Integer?, ByVal dtadate As DateTime) As Boolean

		Function UpdateLOLrecForDTAFile(ByVal recID As Integer, ByVal dtaNr As Integer?, ByVal dtadate As DateTime) As Boolean

		Function LoadZGADataForDTAList(ByVal mdNr As Integer, ByVal dtaNumber As Integer()) As List(Of DtaDataForListing)

		''' <summary>
		''' Setzt einen ZG Auftrag als gelöscht.
		''' </summary>
		''' <param name="vgNoArray"></param>
		''' <returns></returns>
		''' <remarks></remarks>
    Function SetZgOrderDeleted(ByVal vgNoArray As Integer()) As Boolean
    ''' <summary>
    ''' Setzt einen Kreditor Auftrag als gelöscht.
    ''' </summary>
    ''' <param name="vgNoArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function SetLolOrderDeleted(ByVal vgNoArray As Integer()) As Boolean

  End Interface

End Namespace
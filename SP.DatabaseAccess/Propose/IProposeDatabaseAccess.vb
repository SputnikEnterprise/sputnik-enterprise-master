

Imports SP.DatabaseAccess.Propose.DataObjects

Namespace Propose

	''' <summary>
	''' Interface for propose database access.
	''' </summary>
	Public Interface IProposeDatabaseAccess

		Function LoadProposeMasterData(ByVal proposeNumber As Integer) As ProposeMasterData
		Function DuplicateProposeData(ByVal mdNumber As Integer, ByVal oldProposeNumber As Integer, ByVal proposeMasterData As ProposeMasterData) As Boolean

		Function DeleteProposeData(ByVal proposeNumber As Integer, ByVal userNumber As Integer) As Boolean
		Function LoadContextMenu4PrintData() As IEnumerable(Of ContextMenuForPrint)

		Function CheckIfEmployeeInOfferExists(ByVal mdNr As Integer, ByVal ofNr As Integer) As Boolean?
		Function LoadAssingedOfferData(ByVal mdNr As Integer, ByVal ofNr As Integer, ByVal customerNumber As Integer?, ByVal cResponsiblepersonNumber As Integer?) As OffersMasterData
		Function LoadOffersDocumentsForEMailAttachment(ByVal offerNumber As Integer) As IEnumerable(Of OffersDocumentData)
		Function IsAssignedMessageAlreadySent(ByVal mdGuid As String, ByVal iKDNr As Integer, ByVal streMailTo As String, ByVal strSubject As String) As EMailJob.DataObjects.EMailData

		Function AddNewEntryForSentMessage(ByVal contactData As SentMailContactData) As Boolean
		Function LoadSentMessageAttachmentBytesData(ByVal mailId As Integer) As Byte()

	End Interface

End Namespace
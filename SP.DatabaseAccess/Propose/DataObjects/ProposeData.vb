
Namespace Propose.DataObjects

	Public Class ProposeMasterData

		Public Property ID As Integer?
		Public Property ProposeNumberOffset As Integer?
		Public Property ProposeNr As Integer?
		Public Property MANr As Integer?
		Public Property KDNr As Integer?
		Public Property KDZHDNr As Integer?
		Public Property VakNr As Integer?
		Public Property ApplicationNumber As Integer?
		Public Property Employee_UserNumber As Integer?
		Public Property Customer_UserNumber As Integer?
		Public Property KST As String
		Public Property Berater As String
		Public Property KD_Kst As String
		Public Property MA_Kst As String
		Public Property Bezeichnung As String
		Public Property P_State As String
		Public Property P_Art As String
		Public Property KD_Tarif As String
		Public Property MA_ESAls As String
		Public Property P_Anstellung As String
		Public Property P_ArbBegin As String
		Public Property P_Zusatz1 As String
		Public Property P_Zusatz2 As String
		Public Property P_Zusatz3 As String
		Public Property P_Zusatz4 As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property Ab_AnstellungAls As String
		Public Property Ab_AntrittPer As String
		Public Property Ab_LohnBas As Decimal?
		Public Property Ab_LohnAnz As Integer?
		Public Property Ab_LohnBetrag As Decimal?
		Public Property Ab_HBas As Decimal?
		Public Property Ab_HAns As Decimal?
		Public Property Ab_HBetrag As Decimal?
		Public Property Ab_REPer As String
		Public Property Ab_Bemerkung As String
		Public Property Ab_RePer_Date As DateTime?
		Public Property P_Zusatz5 As String
		Public Property P_ArbZeit As String
		Public Property P_Spesen As String
		Public Property P_ArbBegin_Date As DateTime?
		Public Property _P_Zusatz1_Html As String
		Public Property _P_Zusatz2_Html As String
		Public Property _P_Zusatz3_Html As String
		Public Property _P_Zusatz4_Html As String
		Public Property _P_Zusatz5_Html As String
		Public Property Doc_Guid As String
		Public Property MDNr As Integer?

	End Class


	Public Class OffersMasterData

		Public Property ID As Integer?
		Public Property OfNumber As Integer?
		Public Property OF_Res1 As String
		Public Property OF_Res2 As String
		Public Property OF_Res3 As String
		Public Property OF_Res4 As String
		Public Property OF_Res5 As String
		Public Property OF_Res6 As String
		Public Property OF_Res7 As String
		Public Property OF_Res8 As String
		Public Property OF_Slogan As String
		Public Property OF_Group As String
		Public Property OF_Kontakt As String
		Public Property OFLabel As String

		Public Property CustomerNumber As Integer?
		Public Property CustomerCompany As String
		Public Property CustomerEMail As String
		Public Property CustomerLanguage As String
		Public Property CustomerNotMailing As Boolean?

		Public Property CResponsibleNumber As Integer?
		Public Property CResponsibleSalution As String
		Public Property CResponsibleLetterSalutation As String
		Public Property CResponsibleLastname As String
		Public Property CResponsibleFirstname As String
		Public Property CResponsibleEMail As String
		Public Property CResponsibleNotMailing As Boolean?


	End Class

	Public Class OffersDocumentData

		Public Property ID As Integer?
		Public Property Bezeichnung As String
		Public Property DocArt As OffersDocumenttype
		Public Property ScanExtension As String
		Public Property EmployeeLastname As String
		Public Property EmployeeFirstname As String
		Public Property Content As Byte()

	End Class

	Public Class SentMailContactData
		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property RecNr As Integer?
		Public Property CreatedUserNumber As Integer?
		Public Property OfNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property CResponsibleNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property EMailTo As String
		Public Property EMailFrom As String
		Public Property Subject As String
		Public Property Body As String
		Public Property SMTPServer As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property MessageGuid As String
		Public Property AttachmentFileName As String
		Public Property Content As Byte()


	End Class

	Public Enum OffersDocumenttype
		PresentationDoc
		EmployeeDoc
	End Enum

	Public Enum DeleteProposeResult

		Deleted = 2
		ErrorWhileDelete = 4

	End Enum

	Public Class ContextMenuForPrint
		Public Property MnuName As String
		Public Property MnuCaption As String
	End Class

	''' <summary>
	''' Printmenu for office-templates
	''' </summary>
	''' <remarks></remarks>
	Public Class ContextMenuForPrintTemplates
		Public Property MnuDocPath As String
		Public Property MnuDocMacro As String
		Public Property MnuCaption As String
	End Class





End Namespace

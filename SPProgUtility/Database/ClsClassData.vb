
Imports System.IO
Imports DevExpress.XtraGrid.Columns

Public Class ScanDbData

	Public ScanDbConn As String
	Public ScanDbName As String
	Public ScanDbServer As String

End Class

Public Class ClsDbSelectData

	Public MDDbConn As String
	Public MDDbName As String
	Public MDDbServer As String

End Class

Public Class ClsUserData

	Public UserNr As Integer
	Public UserGuid As String

	Public UserLoginname As String
	Public UserLoginPassword As String

	Public UserSalutation As String

	Public UserLName As String
	Public UserFName As String

	Public UserKST As String
	Public UserKST_1 As String
	Public UserKST_2 As String

	Public UserBusinessBranch As String
	Public UserFiliale As String
	Public UserFTitel As String
	Public UserSTitel As String

	Public UserTelefon As String
	Public UserTelefax As String
	Public UserMobile As String
	Public UsereMail As String
	Public UserLanguage As String

	Public UserMDTelefon As String
	Public UserMDDTelefon As String
	Public UserMDTelefax As String
	Public UserMDeMail As String
	Public UserMDGuid As String

	Public UserMDName As String
	Public UserMDName2 As String
	Public UserMDName3 As String
	Public UserMDPostfach As String
	Public UserMDStrasse As String
	Public UserMDPLZ As String
	Public UserMDOrt As String
	Public UserMDCanton As String
	Public UserMDLand As String
	Public UserMDHomepage As String

	Public UserFullNameWithComma As String
	Public UserFullName As String


	Public ReadOnly Property SPPath As String

		Get
			Dim docPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

			Return Path.Combine(docPath, "username")
		End Get

	End Property

	Public ReadOnly Property SPTempPath As String

		Get
			Return Path.Combine(SPPath, "Temp")
		End Get

	End Property

	Public ReadOnly Property spAllowedPath As String

		Get
			Return Path.Combine(SPPath, "Allowed2Delete")
		End Get

	End Property

	Public ReadOnly Property spAllowedPicturePath As String

		Get
			Return Path.Combine(SPPath, "Allowed2Delete", "Bilder")
		End Get

	End Property

	Public ReadOnly Property spPrinterPath As String

		Get
			Return Path.Combine(SPPath, "Printerfiles")
		End Get

	End Property

	Public ReadOnly Property spTempEmployeePath As String

		Get
			Return Path.Combine(SPPath, "Temp", "Kandidat")
		End Get

	End Property

	Public ReadOnly Property spTempCustomerPath As String

		Get
			Return Path.Combine(SPPath, "Temp", "Kunde")
		End Get

	End Property

	Public ReadOnly Property spTempEmplymentPath As String

		Get
			Return Path.Combine(SPPath, "Temp", "ES")
		End Get

	End Property

	Public ReadOnly Property spTempOfferPath As String

		Get
			Return Path.Combine(SPPath, "Temp", "Offer")
		End Get

	End Property

	Public ReadOnly Property spTempRepportPath As String

		Get
			Return Path.Combine(SPPath, "Temp", "RP")
		End Get

	End Property

	Public ReadOnly Property spTempInvoicePath As String

		Get
			Return Path.Combine(SPPath, "Temp", "RE")
		End Get

	End Property

	Public ReadOnly Property spTempPayrollPath As String

		Get
			Return Path.Combine(SPPath, "Temp", "LO")
		End Get

	End Property

	Public ReadOnly Property spTempNLAPath As String

		Get
			Return Path.Combine(SPPath, "Temp", "NLA")
		End Get

	End Property


End Class



Public Class ClsMDData

	Private Const MANDANT_DOCUMENTS_PATH = "Documents"
	Private Const MANDANT_USERPROFILE_PATH = "Profiles"
	Private Const MANDANT_TEMPLATES_PATH = "Templates"
	Private Const MANDANT_SKIN_PATH = "Templates\Skins"

	Public MDNr As Integer
	Public MDGroupNr As Integer
	Public MDYear As Integer

	Public MDName As String
	Public MDGuid As String
	Public MultiMD As Short
	Public ClosedMD As Short

	Public MDMainPath As String
	Public MDDbConn As String
	Public MDDbName As String
	Public MDDbServer As String
	Public WebserviceDomain As String

	Public MDName_2 As String
	Public MDName_3 As String
	Public MDStreet As String
	Public MDPostcode As String
	Public MDCountry As String
	Public MDCity As String
	Public MDCanton As String

	Public MDTelefon As String
	Public MDTelefax As String
	Public MDeMail As String
	Public MDHomepage As String

	Public MandantColorName As String
	Public AutoFilterConditionDate As AutoFilterCondition
	Public AutoFilterConditionNumber As AutoFilterCondition

	Public MDRootPath As String

	Public ReadOnly Property MDIniPath() As String
		Get
			Return String.Format("{0}", IO.Path.Combine(MDRootPath, MDYear))
		End Get
	End Property

	Public ReadOnly Property MandantCurrentXMLFileName() As String
		Get
			Return String.Format("{0}", IO.Path.Combine(MDRootPath, MDYear, "Programm.XML"))
		End Get
	End Property

	Public ReadOnly Property MDDocumentPath() As String
		Get
			Return String.Format("{0}", IO.Path.Combine(MDRootPath, MANDANT_DOCUMENTS_PATH))
		End Get
	End Property

	Public ReadOnly Property MDUserProfilesPath() As String
		Get
			Return String.Format("{0}", IO.Path.Combine(MDRootPath, MANDANT_USERPROFILE_PATH))
		End Get
	End Property

	Public ReadOnly Property MDTemplatePath() As String
		Get
			Return String.Format("{0}", IO.Path.Combine(MDRootPath, MANDANT_TEMPLATES_PATH))
		End Get
	End Property

	Public ReadOnly Property MDFormXMLFileName() As String
		Get
			Return String.Format("{0}", IO.Path.Combine(MDRootPath, MANDANT_SKIN_PATH, "FormData.XML"))
		End Get
	End Property


End Class

Public Class ClsMailingData

	Public SMTPServer As String
	Public SMTPPort As Integer
	Public EnableSSL As Boolean

	Public FaxServer As String
	Public FaxExtension As String
	Public FaxForwarder As String
	Public FaxDavidServer As String

End Class

Public Class LicenseData

	Public sesam As String
	Public cresus As String
	Public abacus As String

	Public swifac As String
	Public comatic As String
	Public kmufactoring As String
	Public csoplist As String
	Public parifond As String

	Public dvrefnr As String
	Public dvusername As String
	Public dvuserpw As String
	Public dvurl As String

	Public ScanDropIN As Boolean?
	Public CVDropIN As Boolean?
	Public PMSearch As Boolean?

End Class

Public Class TapiData

	Public TapiSerialnumber As String
	Public TapiLicenseKey As String

End Class

Public Class O2SolutionData

	Public PDFVWSerialnumber As String
	Public PDFSerialnumber As String

End Class


Public Class WOSData

	Public WOSEmployeeGuid As String
	Public WOSCustomerGuid As String
	Public WOSVacancyGuid As String

End Class


Public Class TemplateData

	Public Path4ZeugnisDeckblatt As String
	Public Path4AGBDeckblatt As String
	Public Path4AGBFest As String
	Public Path4AGBTemp As String


End Class

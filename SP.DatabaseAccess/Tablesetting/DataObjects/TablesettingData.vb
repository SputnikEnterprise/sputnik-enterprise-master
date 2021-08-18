
Namespace TableSetting.DataObjects



	Public Class LAStammData

		Public Property Selected As Boolean
		Public Property LANr As Decimal?
		Public Property LAText As String
		Public Property LALoText As String
		Public Property LAOpText As String
		Public Property Bedingung As String
		Public Property RunFuncBefore As String
		Public Property Verwendung As String
		Public Property GroupKey As Decimal?
		Public Property Vorzeichen As String
		Public Property Rundung As Integer
		Public Property TypeAnzahl As Short
		Public Property MAAnzVar As String
		Public Property FixAnzahl As Decimal
		Public Property Sum0Anzahl As Short
		Public Property Sum1Anzahl As Short
		Public Property PrintAnzahl As Boolean
		Public Property TypeBasis As Short
		Public Property MABasVar As String
		Public Property FixBasis As Decimal
		Public Property Sum0Basis As Short
		Public Property Sum1Basis As Short
		Public Property Sum2Basis As Short
		Public Property PrintBasis As Boolean
		Public Property TypeAnsatz As Short
		Public Property MAAnsVar As String
		Public Property FixAnsatz As Decimal
		Public Property SumAnsatz As Short
		Public Property PrintAnsatz As Boolean
		Public Property Sum0Betrag As Short
		Public Property Sum1Betrag As Short
		Public Property Sum2Betrag As Short
		Public Property Sum3Betrag As Short
		Public Property PrintBetrag As Boolean?
		Public Property PrintLA As Boolean?
		Public Property BruttoPflichtig As Boolean?
		Public Property AHVPflichtig As Boolean?
		Public Property ALVPflichtig As Boolean?
		Public Property NBUVPflichtig As Boolean?
		Public Property UVPflichtig As Boolean?
		Public Property BVGPflichtig As Boolean?
		Public Property KKPflichtig As Boolean?
		Public Property QSTPflichtig As Boolean?
		Public Property MWSTPflichtig As Boolean?
		Public Property Reserve1 As Boolean?
		Public Property Reserve2 As Boolean?
		Public Property Reserve3 As Boolean?
		Public Property Reserve4 As Boolean?
		Public Property Reserve5 As Boolean?
		Public Property FerienInklusiv As Boolean?
		Public Property FeierInklusiv As Boolean?
		Public Property _13Inklusiv As Boolean?
		Public Property ByNullCreate As Boolean?
		Public Property KDAnzahl As String
		Public Property KDBasis As String
		Public Property KDAnsatz As String
		Public Property Leerzeile As String
		Public Property SKonto As Integer
		Public Property HKonto As Integer
		Public Property VorzeichenLAW As String
		Public Property BruttoLAWPflichtig As Boolean?
		Public Property Kumulativ As Boolean
		Public Property LAWFeld As String
		Public Property ES_Pflichtig As Boolean?
		Public Property DuppInKD As Boolean?
		Public Property Result As String
		Public Property LAJahr As Integer
		Public Property nolisting As Boolean?
		Public Property ShowInZG As Boolean?
		Public Property KumulativMonth As Boolean?
		Public Property TagesSpesen As Boolean?
		Public Property StdSpesen As Boolean?
		Public Property KumLANr As Integer
		Public Property recid As Integer
		Public Property LADeactivated As Boolean?
		Public Property AGLA As Boolean?
		Public Property ProTag As Boolean?
		Public Property LOBeleg1 As String
		Public Property LOBeleg2 As String
		Public Property GleitTime As Boolean?
		Public Property AllowedMore_Anz As Boolean?
		Public Property AllowedMore_Bas As Boolean?
		Public Property AllowedMore_Ans As Boolean?
		Public Property AllowedMore_Btr As Boolean?
		Public Property Vorzeichen_2 As String
		Public Property WarningByZero As Boolean?
		Public Property LAWFeld_0 As String
		Public Property SeeKanton As Boolean?
		Public Property ARGB_Verdienst_Unterkunft As Boolean?
		Public Property ARGB_Verdienst_Mahlzeit As Boolean?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property NotForZV As Boolean?
		Public Property Vorzeichen_3 As String
		Public Property CalcFer13BasAsStd As Boolean?
		Public Property FuncBeforePrint As String
		Public Property DB1_Bruttopflichtig As Boolean?
		Public Property Db1_AHVpflichtig As Boolean?
		Public Property MoreProz4Fer As Boolean?
		Public Property MoreProz4Feier As Boolean?
		Public Property MoreProz413 As Boolean?
		Public Property MoreProz4FerAmount As Decimal?
		Public Property MoreProz4FeierAmount As Decimal?
		Public Property MoreProz413Amount As Decimal?

		Public Property MDNr As Integer
		Public Property USNr As Integer

		Public Property LSE_Field As String

	End Class



	Public Class LATranslationData

		Public Property recid As Integer
		Public Property LANr As Decimal?
		Public Property LAText As String
		Public Property Name_I As String
		Public Property Name_F As String
		Public Property Name_E As String

		Public Property Name_OP_I As String
		Public Property Name_OP_F As String
		Public Property Name_OP_E As String

		Public Property Name_LO_I As String
		Public Property Name_LO_F As String
		Public Property Name_LO_E As String

		Public ReadOnly Property LANumber As Decimal
			Get
				Return LANr
			End Get
		End Property

		Public ReadOnly Property LAName As String
			Get
				Return LAText
			End Get
		End Property

	End Class



	Public Class UserData

		Public Property USNr As Integer
		Public Property US_Name As String
		Public Property Nachname As String
		Public Property Vorname As String
		Public Property Anrede As String
		Public Property PW As String
		Public Property Postfach As String
		Public Property Strasse As String
		Public Property PLZ As String
		Public Property Ort As String
		Public Property Land As String
		Public Property Telefon As String
		Public Property Telefax As String
		Public Property Natel As String
		Public Property eMail As String
		Public Property Homepage As String
		Public Property Abteilung As String
		Public Property GebDat As Date?
		Public Property Sprache As String
		Public Property KST As String
		Public Property SecLevel As Integer?
		Public Property Logged As Boolean?
		Public Property Deaktiviert As Boolean?
		Public Property Result As String
		Public Property USKst1 As String
		Public Property USKst2 As String
		Public Property PlanerDb As String
		Public Property AktivUntil As DateTime?
		Public Property recid As Integer
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property USBild As Byte()
		Public Property USSign As Byte()
		Public Property USFiliale As String
		Public Property USLanguage As String
		Public Property USTitel_1 As String
		Public Property USTitel_2 As String
		Public Property Transfered_Guid As String
		Public Property EMail_UserName As String
		Public Property EMail_UserPW As String
		Public Property USRightsTemplate As String
		Public Property MDNr As Integer
		Public Property jch_layoutID As Integer?
		Public Property jch_logoID As Integer
		Public Property OstJob_ID As String
		Public Property ostjob_Kontingent As Integer?
		Public Property JCH_SubID As Integer?
		Public Property AsCostCenter As Boolean?
		Public Property LogonMorePlaces As Boolean?

		Public ReadOnly Property UserFullname
			Get
				Return String.Format("{0}, {1}", Nachname, Vorname)
			End Get
		End Property

	End Class


	''' <summary>
	''' Result of ZG deletion.
	''' </summary>
	Public Enum DeleteUserResult
		ResultDeleteOk = 1
		ResultDeleteError = 20
	End Enum

	Public Enum DeleteUserRightsResult
		ResultDeleteOk = 1
		ResultDeleteError = 20
	End Enum

	Public Class UserAddressData

		Public Property USNr As Integer
		Public Property MD_Name1 As String
		Public Property MD_Name2 As String
		Public Property MD_Name3 As String
		Public Property MD_Postfach As String
		Public Property MD_Strasse As String
		Public Property MD_PLZ As String
		Public Property MD_Ort As String
		Public Property MD_Land As String
		Public Property MD_Telefon As String
		Public Property MD_DTelefon As String
		Public Property MD_Telefax As String
		Public Property MD_eMail As String
		Public Property MD_Homepage As String
		Public Property MD_Bewilligung As String
		Public Property recid As Integer
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedOn As DateTime?

	End Class



	Public Class RightsData

		Public Property recid As Integer
		Public Property Bezeichnung As String
		Public Property RightProc As String

	End Class


	Public Class UserRightData

		Public Property recid As Integer
		Public Property USNr As Integer
		Public Property MDNr As Integer
		Public Property SecNr As Integer
		Public Property Autorized As Boolean
		Public Property SecNrBez As String
		Public Property ModulName As String
		Public Property UserRightsChecked As Boolean?
		Public Property AllowedToSeeAllData As Boolean?
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

	End Class



	Public Class DocumentData

		Public Property recid As Integer
		Public Property UDR_ID As Integer?
		Public Property USNr As Integer?
		Public Property MDNr As Integer?
		Public Property JobNr As String
		Public Property Bezeichnung As String
		Public Property DocName As String
		Public Property AllowedToExport As Boolean?
		Public Property LogActivity As Boolean?
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

	End Class


	Public Class MandantDocumentData
		Public Property ID As Integer
		Public Property ModulNr As Integer?
		Public Property RecNr As Integer?
		Public Property jobNr As String
		Public Property Bezeichnung As String
		Public Property DocName As String
		Public Property ExportedFileName As String
		Public Property DokNameToShow As String
		Public Property WindowsTitle As String
		Public Property LeftMargin As Integer?
		Public Property RightMargin As Integer?
		Public Property TopMargin As Integer?
		Public Property BottomMargin As Integer?
		Public Property Anzahlkopien As Integer?
		Public Property Meldung0 As String
		Public Property TempDocPath As String
		Public Property Meldung1 As String
		Public Property ZoomProz As Integer?
		Public Property ParamCheck As Boolean?
		Public Property KonvertName As Boolean?
		Public Property FontDesent As String
		Public Property IncPrv As Boolean?
		Public Property PrintInDiffColor As Boolean?
		Public Property InsertFileToDb As Boolean?

		Public ReadOnly Property LabelViewData() As String
			Get
				Return String.Format("{0} - {1}", jobNr, Bezeichnung)
			End Get
		End Property
	End Class


	Public Class lmvKTGData

		Public Property mdyear As Integer
		Public Property recid As Integer
		Public Property recnr As Integer
		Public Property mdnr As Integer
		Public Property gavnumber As Integer
		Public Property berufbez As String

		Public Property KK_AN_WA_Proz As Decimal?
		Public Property KK_AN_WZ_Proz As Decimal?
		Public Property KK_AN_MA_Proz As Decimal?
		Public Property KK_AN_MZ_Proz As Decimal?
		Public Property KK_AG_WA_Proz As Decimal?
		Public Property KK_AG_WZ_Proz As Decimal?
		Public Property KK_AG_MA_Proz As Decimal?
		Public Property KK_AG_MZ_Proz As Decimal?
		Public Property KK_AN_WA_Btr As Decimal?
		Public Property KK_AN_WZ_Btr As Decimal?
		Public Property KK_AN_MA_Btr As Decimal?
		Public Property KK_AN_MZ_Btr As Decimal?
		Public Property KK_AG_WA_Btr As Decimal?
		Public Property KK_AG_WZ_Btr As Decimal?
		Public Property KK_AG_MA_Btr As Decimal?
		Public Property KK_AG_MZ_Btr As Decimal?

		Public Property KK_AN_WA_Proz_72 As Decimal?
		Public Property KK_AN_WZ_Proz_72 As Decimal?
		Public Property KK_AN_MA_Proz_72 As Decimal?
		Public Property KK_AN_MZ_Proz_72 As Decimal?
		Public Property KK_AG_WA_Proz_72 As Decimal?
		Public Property KK_AG_WZ_Proz_72 As Decimal?
		Public Property KK_AG_MA_Proz_72 As Decimal?
		Public Property KK_AG_MZ_Proz_72 As Decimal?
		Public Property KK_AN_WA_Btr_72 As Decimal?
		Public Property KK_AN_WZ_Btr_72 As Decimal?
		Public Property KK_AN_MA_Btr_72 As Decimal?
		Public Property KK_AN_MZ_Btr_72 As Decimal?
		Public Property KK_AG_WA_Btr_72 As Decimal?
		Public Property KK_AG_WZ_Btr_72 As Decimal?
		Public Property KK_AG_MA_Btr_72 As Decimal?
		Public Property KK_AG_MZ_Btr_72 As Decimal?

		Public Property createdon As DateTime?
		Public Property createdfrom As String
		Public Property changedon As DateTime?
		Public Property changedfrom As String

		Public ReadOnly Property KK_ANAG_WZ_Proz
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_WZ_Proz, KK_AG_WZ_Proz)
			End Get
		End Property

		Public ReadOnly Property KK_ANAG_WA_Proz
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_WA_Proz, KK_AG_WA_Proz)
			End Get
		End Property

		Public ReadOnly Property KK_ANAG_MZ_Proz
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_MZ_Proz, KK_AG_MZ_Proz)
			End Get
		End Property

		Public ReadOnly Property KK_ANAG_MA_Proz
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_MA_Proz, KK_AG_MA_Proz)
			End Get
		End Property




		Public ReadOnly Property KK_ANAG_WZ_Proz_72
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_WZ_Proz_72, KK_AG_WZ_Proz_72)
			End Get
		End Property

		Public ReadOnly Property KK_ANAG_WA_Proz_72
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_WA_Proz_72, KK_AG_WA_Proz_72)
			End Get
		End Property

		Public ReadOnly Property KK_ANAG_MZ_Proz_72
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_MZ_Proz_72, KK_AG_MZ_Proz_72)
			End Get
		End Property

		Public ReadOnly Property KK_ANAG_MA_Proz_72
			Get
				Return String.Format("{0:n5} - {0:n5}", KK_AN_MA_Proz_72, KK_AG_MA_Proz_72)
			End Get
		End Property


	End Class


	Public Class lmvTSpesenData

		Public Property mdyear As Integer
		Public Property recid As Integer
		Public Property recnr As Integer
		Public Property mdnr As Integer
		Public Property gavnumber As Integer
		Public Property berufbez As String

		Public Property TSpesen As Decimal?
		Public Property TWochenstunden As Decimal?

		Public Property createdon As DateTime?
		Public Property createdfrom As String
		Public Property changedon As DateTime?
		Public Property changedfrom As String


	End Class



	Public Class ChildEducationData

		Public Property mdyear As Integer
		Public Property recid As Integer
		Public Property recnr As Integer
		Public Property mdnr As Integer

		Public Property fak_kanton As String
		Public Property ki1_fakmax As Decimal?
		Public Property ki2_fakmax As Decimal?
		Public Property ki1_std As Decimal?

		Public Property ki2_std As Decimal?
		Public Property ki1_day As Decimal?

		Public Property ki2_day As Decimal?
		Public Property ki1_month As Decimal?
		Public Property ki2_month As Decimal?
		Public Property changekiin As String
		Public Property au1_std As Decimal?
		Public Property au2_std As Decimal?
		Public Property au1_day As Decimal?
		Public Property au2_day As Decimal?
		Public Property au1_month As Decimal?
		Public Property changeauin As String

		Public Property createdon As DateTime?
		Public Property createdfrom As String

		Public Property changedon As DateTime?
		Public Property changedfrom As String

		Public Property fak_name As String
		Public Property fak_zhd As String
		Public Property fak_postfach As String
		Public Property fak_strasse As String
		Public Property fak_plzort As String
		Public Property fak_mnr As String
		Public Property fak_knr As String
		Public Property changeauin_2 As String
		Public Property changekiin_2 As String
		Public Property yminlohn As Decimal?

		Public Property bemerkung_1 As String
		Public Property bemerkung_2 As String
		Public Property bemerkung_3 As String
		Public Property bemerkung_4 As String
		Public Property AtEndBeginES As Boolean?
		Public Property SeeAHVLohnForYear As Boolean?

	End Class



	Public Class MandantData


		Public Property Jahr As Integer
		Public Property MD_Name1 As String
		Public Property MD_Name2 As String
		Public Property MD_Name3 As String
		Public Property MDFullFileName As String
		Public Property MDAuflistung As String
		Public Property dbname As String
		Public Property Customer_ID As String
		Public Property Postfach As String
		Public Property Strasse As String
		Public Property PLZ As String
		Public Property Ort As String
		Public Property MD_Kanton As String
		Public Property Land As String
		Public Property Telefon As String
		Public Property Telefax As String
		Public Property Homepage As String
		Public Property eMail As String
		Public Property Passwort4 As String

		Public Property Suva_HL As Decimal?
		Public Property ALV1_HL As Decimal?
		Public Property ALV2_HL As Decimal?
		Public Property RentAlter_M As Short?
		Public Property RentAlter_W As Short?
		Public Property BVG_Koordination_Jahr As Decimal?
		Public Property BVG_Std As Integer?
		Public Property BVG_Max_Jahr As Decimal?
		Public Property BVG_Min_Jahr As Decimal?
		Public Property BVG_Aus1Woche As Decimal?
		Public Property BVG_Aus2Woche As Decimal?
		Public Property BVG_List As String
		Public Property BVG_List_Grouped As String

		Public Property MindestAlter As Short?

		Public Property RentFrei_Monat As Decimal?
		Public Property RentFrei_Jahr As Decimal?
		Public Property NBUV_WStd As Decimal?
		Public Property AHV_AN As Decimal?
		Public Property AHV_2_AN As Decimal?
		Public Property ALV1_HL_ As Decimal?
		Public Property ALV2_HL_ As Decimal?
		Public Property ALV_AN As Decimal?
		Public Property ALV2_An As Decimal?
		Public Property SUVA_HL_ As Decimal?
		Public Property NBUV_M As Decimal?
		Public Property NBUV_M_Z As Decimal?
		Public Property NBUV_W As Decimal?
		Public Property NBUV_W_Z As Decimal?
		Public Property KK_An_MA As Decimal?
		Public Property KK_An_MZ As Decimal?
		Public Property KK_An_WA As Decimal?
		Public Property KK_An_WZ As Decimal?
		Public Property AHV_AG As Decimal?
		Public Property AHV_2_AG As Decimal?
		Public Property ALV_AG As Decimal?
		Public Property ALV2_AG As Decimal?
		Public Property Suva_A As Decimal?
		Public Property Suva_Z As Decimal?
		Public Property UVGZ_A As Decimal?
		Public Property UVGZ_B As Decimal?
		Public Property UVGZ2_A As Decimal?
		Public Property UVGZ2_B As Decimal?

		Public Property KK_AG_MA As Decimal?
		Public Property KK_AG_MZ As Decimal?
		Public Property KK_AG_WA As Decimal?
		Public Property KK_AG_WZ As Decimal?
		Public Property Fak_Proz As Decimal?


		Public Property b_marge As Decimal?
		Public Property b_margep As Decimal?
		Public Property x_marge As Decimal?
		Public Property ag_tar_proz As Decimal?
		Public Property n_zhlg As Decimal?
		Public Property q_zhlg As Decimal?
		Public Property ma_kl As Decimal?

	End Class

	Public Class EmployeeContactData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class EmployeeStateData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class AssessmentData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class CommunicationTypeData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class DrivingLicenceData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class VehicleData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class CarReserveData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Enum ContactReserveType
		Reserve1 = 1
		Reserve2 = 2
		Reserve3 = 3
		Reserve4 = 4
	End Enum


	Public Class ContactReserveData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class



	Public Class DeadlineData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class WorkPensumData
		Public Property recid As Integer
		Public Property bez_value As String

	End Class




	Public Class EmployeeEmployementTypeData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class EmployeeInteriviewStateData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class PrintTemplatesData

		'select ID, RecNr, SecNr, DocNr, DocFullName, MakroName, MenuLabel, ItemShowIn, CreatedOn, CreatedFrom, ChangedOn, ChangedFrom From Tab_TemplateMenu
		Public Property recid As Integer
		Public Property recnr As Integer
		Public Property secnr As Integer
		Public Property docnr As String
		Public Property docfullname As String
		Public Property makroname As String
		Public Property menulabel As String
		Public Property itemshowin As String
		Public Property createdon As DateTime?
		Public Property createdfrom As String
		Public Property changedon As DateTime?
		Public Property changedfrom As String

	End Class

	Public Class ExportTemplatesData

		'ID, RecNr, Modulname, Bezeichnung, DocName, Tooltip, MnuName
		Public Property recid As Integer
		Public Property recnr As Integer
		Public Property ModulName As String

		Public Property Bezeichnung As String
		Public Property DocName As String
		Public Property Tooltip As String
		Public Property MnuName As String

		Public Enum ModulEnum

			Unknown
			EmployeeListing
			CustomerListing
			EmploymentListing

			InvoiceListing
			ReceiptListing

			TelefonListing
			NotFinishedReportListing
			ZEListing
			AdvancedPaymentListing
			VacancyListing
			ProposeListing
			CustomerSalesListing

			PayrollQSTListing
			PayrollFakListing
			PayrollFibuListing
			PayrollLOANListing

		End Enum


		Public ReadOnly Property ProgramName(ByVal modul As ModulEnum) As String
			Get
				Select Case modul
					Case ModulEnum.EmployeeListing
						Return "779ECA32-679C-43f5-825D-CE3DD439FA1A"

					Case ModulEnum.CustomerListing
						Return "4c2db8b0-0521-4862-a640-d895e02100f9"

					Case ModulEnum.EmploymentListing
						Return "E7AC7BF2-F177-461b-AE4C-723856928E72"

					Case ModulEnum.InvoiceListing
						Return "15be882f-5eb5-4f83-ad94-5721af563ca9"

					Case ModulEnum.ReceiptListing
						Return "BAFCF91A-CE90-4b8f-896C-E3731A90D253"

					Case ModulEnum.TelefonListing
						Return "11762B24-36AE-4fd8-9D66-D31A86040095"

					Case ModulEnum.NotFinishedReportListing
						Return "9cce2b1b-8fcc-4965-9839-420736d7149c"
					Case ModulEnum.AdvancedPaymentListing
						Return "b0ff184d-9750-4aec-b79a-930163b36ab2"
					Case ModulEnum.VacancyListing
						Return "33A7A875-90D6-4c87-AB69-8249FE251093"
					Case ModulEnum.ProposeListing
						Return "7836EB38-A0C5-4b90-941F-B803C741BA02"
					Case ModulEnum.CustomerSalesListing
						Return "34e02b48-59a7-4c5d-8b13-6bba72c98f33"

					Case ModulEnum.PayrollQSTListing
						Return "23EC6013-BBB0-45ff-B72B-5940FA738A6B"
					Case ModulEnum.PayrollFakListing
						Return "335c767b-2873-4c64-b53f-e338c054f836"
					Case ModulEnum.PayrollFibuListing
						Return "942ECD22-2128-4962-8C59-2F0447BE6F6A"
					Case ModulEnum.PayrollLOANListing
						Return "F7447635-8C97-441a-A1C0-7D3F86245396"

					Case Else
						Return String.Empty

				End Select

			End Get
		End Property

		Public ReadOnly Property TranslatedModul() As ModulEnum
			Get
				Select Case ModulName
					Case "779ECA32-679C-43f5-825D-CE3DD439FA1A"
						Return ModulEnum.EmployeeListing

					Case "4c2db8b0-0521-4862-a640-d895e02100f9"
						Return ModulEnum.CustomerListing

					Case "E7AC7BF2-F177-461b-AE4C-723856928E72"
						Return ModulEnum.EmploymentListing

					Case "15be882f-5eb5-4f83-ad94-5721af563ca9"
						Return ModulEnum.InvoiceListing

					Case "BAFCF91A-CE90-4b8f-896C-E3731A90D253"
						Return ModulEnum.ReceiptListing

					Case "11762B24-36AE-4fd8-9D66-D31A86040095"
						Return ModulEnum.TelefonListing

					Case "9cce2b1b-8fcc-4965-9839-420736d7149c"
						Return ModulEnum.NotFinishedReportListing
					Case "b0ff184d-9750-4aec-b79a-930163b36ab2"
						Return ModulEnum.AdvancedPaymentListing
					Case "33A7A875-90D6-4c87-AB69-8249FE251093"
						Return ModulEnum.VacancyListing
					Case "7836EB38-A0C5-4b90-941F-B803C741BA02"
						Return ModulEnum.ProposeListing
					Case "34e02b48-59a7-4c5d-8b13-6bba72c98f33"
						Return ModulEnum.CustomerSalesListing

					Case "23EC6013-BBB0-45ff-B72B-5940FA738A6B"
						Return ModulEnum.PayrollQSTListing
					Case "335c767b-2873-4c64-b53f-e338c054f836"
						Return ModulEnum.PayrollFakListing
					Case "942ECD22-2128-4962-8C59-2F0447BE6F6A"
						Return ModulEnum.PayrollFibuListing
					Case "F7447635-8C97-441a-A1C0-7D3F86245396"
						Return ModulEnum.PayrollLOANListing

					Case Else
						Return ModulEnum.Unknown

				End Select

			End Get
		End Property


	End Class


	Public Class JobLanguageData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class JobData
		Public Property recid As Integer
		Public Property code As Integer?
		Public Property beruf As String

		Public Property beruf_d_m As String
		Public Property beruf_d_w As String
		Public Property beruf_i_m As String
		Public Property beruf_i_w As String
		Public Property beruf_f_m As String
		Public Property beruf_f_w As String
		Public Property beruf_e_m As String
		Public Property beruf_e_w As String

		Public Property fach_d As String
		Public Property fach_i As String
		Public Property fach_f As String

		Public Property createdon As DateTime
		Public Property createdfrom As String

	End Class


	Public Class SectorData
		Public Property recid As Integer
		Public Property code As Integer?
		Public Property branche As String

		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

		Public Property createdon As DateTime
		Public Property createdfrom As String

	End Class


	Public Class AvilableBusinessBranchData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property Code_1 As String

	End Class



	Public Class CurrencyData
		Public Property recId As Integer
		Public Property code As String
		Public Property description As String

	End Class


	Public Class CostCenter1Data
		Public Property recId As Integer
		Public Property kstname As Integer
		Public Property kstbezeichnung As String

	End Class


	Public Class CostCenter2Data

		Public Property recId As Integer
		Public Property kstname As String
		Public Property kstbezeichnung As String
		Public Property kstname1 As Integer

	End Class



	Public Class LanguageData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class SalutationData

		Public Property recId As Integer
		Public Property salutation As String
		Public Property letterform As String

		Public Property salutation_d As String
		Public Property salutation_i As String
		Public Property salutation_f As String
		Public Property salutation_e As String

		Public Property letterform_d As String
		Public Property letterform_i As String
		Public Property letterform_f As String
		Public Property letterform_e As String


	End Class



	Public Class TermsAndConditionsData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class SMSTemplateData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class AbsenceData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property Description As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class BVGData

		Public Property recId As Integer
		Public Property MDNr As Integer
		Public Property Alter As Integer
		Public Property ProzentSatz As Decimal?
		Public Property ProzJahr As Integer

	End Class

	Public Class FF13LohnData

		Public Property recId As Integer
		Public Property Jahrgang As Integer
		Public Property FerProzentSatz As Decimal?
		Public Property FeierProzentSatz As Decimal?
		Public Property Prozent13Satz As Decimal?

	End Class



	Public Class QstInfoData

		Public Property recId As Integer
		Public Property SKanton As String
		Public Property MonthStd As Integer?
		Public Property StdDown As Boolean?
		Public Property StdDownAtEndBegin As Boolean?
		Public Property StdUp As Boolean?
		Public Property DESameAsCH As Boolean?
		Public Property JustAtEndBegin As Boolean?
		Public Property CalendarDay As Boolean?
		Public Property WithFLeistung As Boolean?
		Public Property HandleAsAutomation As Boolean?

	End Class


	Public Class CountryData

		Public Property recid As Integer
		Public Property code As String
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class FIBUData

		Public Property recid As Integer
		Public Property KontoNr As Integer
		Public Property KontoName As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String
		Public Property TranslatedLabel As String

		Public ReadOnly Property KontoViewData As String
			Get
				Return (String.Format("{0:F0}: {1}", KontoNr, KontoName))
			End Get
		End Property

	End Class


	''' <summary>
	''' Customer Data
	''' </summary>
	Public Class CustomerPropertyData

		Public Property recid As Integer
		Public Property bez_value As Decimal?
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class CustomerContactData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class CustomerStateData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class CustomerEmployementTypeData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class CustomerStichwortData
		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class




	Public Class VacancyContactData

		Public Property ID As Integer
		Public Property recvalue As Integer?
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class VacancyStateData

		Public Property ID As Integer
		Public Property recvalue As Integer?
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class VacancyGroupData

		Public Property ID As Integer
		Public Property Bez_Value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class VacancySubGroupData

		Public Property ID As Integer
		Public Property MainGroup As String
		Public Property SubGroup As String
		Public Property Bez_DE As String
		Public Property Bez_IT As String
		Public Property Bez_FR As String
		Public Property Bez_EN As String
		Public Property TranslatedValue As String

	End Class


	''' <summary>
	''' Offer
	''' </summary>
	Public Class OfferContactData

		Public Property ID As Integer
		Public Property Description As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	Public Class OfferStateData

		Public Property ID As Integer
		Public Property Description As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class OfferGroupData

		Public Property ID As Integer
		Public Property Description As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class


	''' <summary>
	''' propose data
	''' </summary>
	Public Class ProposeStateData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class ProposeEmployementTypeData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class ProposeArtData

		Public Property recid As Integer
		Public Property bez_value As String
		Public Property bez_d As String
		Public Property bez_i As String
		Public Property bez_f As String
		Public Property bez_e As String

	End Class

	Public Class MDBankData

		Public Property ID As Integer?
		Public Property MDNr As Integer?
		Public Property Jahr As Integer?
		Public Property RecNr As Integer?
		Public Property ModulArt As BankModulEnum?
		Public Property MD_ID As String
		Public Property KontoESR1 As String
		Public Property KontoESR2 As String
		Public Property DTAClnr As String
		Public Property KontoDTA As String
		Public Property KontoVG As String
		Public Property BankName As String
		Public Property BankClnr As String
		Public Property BankAdresse As String
		Public Property Swift As String
		Public Property ESRIBAN1 As String
		Public Property ESRIBAN2 As String
		Public Property ESRIBAN3 As String
		Public Property DTAIBAN As String
		Public Property VGIBAN As String
		Public Property DTAAdr1 As String
		Public Property DTAAdr2 As String
		Public Property DTAAdr3 As String
		Public Property DTAAdr4 As String
		Public Property AsStandard As Boolean?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property RecBez As String
		Public Property USNr As Integer?

		Public ReadOnly Property DisplayName As String
			Get
				Dim name = If(Not String.IsNullOrWhiteSpace(BankName), BankName, RecBez)

				Return name
			End Get
		End Property

		Public ReadOnly Property BankCountry As String
			Get
				Dim name = String.Empty
				If Not String.IsNullOrWhiteSpace(DTAIBAN) Then name = DTAIBAN.Substring(0, 2) Else name = ""

				Return name
			End Get
		End Property

	End Class

	Public Enum BankModulEnum
		DTADATA
		ESRDATA
	End Enum


End Namespace



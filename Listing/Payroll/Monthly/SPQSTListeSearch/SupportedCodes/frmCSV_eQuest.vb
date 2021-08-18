
Imports System.IO

Imports DevExpress.Export
Imports DevExpress.Export.Xl
Imports DevExpress.Printing.ExportHelpers
Imports DevExpress.Office.Crypto
Imports System.Globalization
Imports System.ComponentModel
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraBars.Navigation

Partial Class frmCSV


#Region "private consts"

	Private Const CSV_SEPRATOR As String = ";"

#End Region

	Private m_CompanyTaxCommunityCode As Integer

	Private Function ResetForeQuest() As Boolean
		Dim result As Boolean = True

		Try
			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			result = result AndAlso LoadCommunityOverWebService()
			If Not result Then Return False

			m_CompanyTaxCommunityCode = 0
			If m_CommunityData Is Nothing OrElse m_CommunityData.Count = 0 Then Return False
			Dim communityData = m_CommunityData.Where(Function(x) x.Translated_Value = m_InitializationData.MDData.MDCity).FirstOrDefault
			If Not communityData Is Nothing Then
				m_CompanyTaxCommunityCode = communityData.BFSNumber
			End If

			Select Case CallerModulName

				Case CallerModulNum.EQUEST
					ResetGridValideQuestData()
					ResetGridInvalideQuestData()
					txt_eQuestFilename.EditValue = My.Settings.Filename4eQuest
					tpeQuestData.SelectedPage = If(gveQuestInvalidData.RowCount > 0, tnpInvalidData, tnpValidData)

					LoadImportDataForeQuest()


				Case Else
					Return False

			End Select
			txt_eQuestFilename.Visible = True

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)

		End Try


		Return result

	End Function

	Private Sub ResetGridValideQuestData()

		gveQuestValidData.OptionsView.ShowIndicator = False
		gveQuestValidData.OptionsView.ShowAutoFilterRow = True
		gveQuestValidData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gveQuestValidData.OptionsView.ShowFooter = True

		gveQuestValidData.Columns.Clear()


		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MaNr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gveQuestValidData.Columns.Add(columnMANr)

		Dim columnAbrechnungsstelle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAbrechnungsstelle.Caption = m_Translate.GetSafeTranslationValue("Abrechnungsstelle")
		columnAbrechnungsstelle.Name = "Abrechnungsstelle"
		columnAbrechnungsstelle.FieldName = "Abrechnungsstelle"
		columnAbrechnungsstelle.Visible = False
		gveQuestValidData.Columns.Add(columnAbrechnungsstelle)

		Dim columnSocialInsuranceNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSocialInsuranceNumber.Caption = m_Translate.GetSafeTranslationValue("AHVN13")
		columnSocialInsuranceNumber.Name = "SocialInsuranceNumber"
		columnSocialInsuranceNumber.FieldName = "SocialInsuranceNumber"
		columnSocialInsuranceNumber.Visible = False
		gveQuestValidData.Columns.Add(columnSocialInsuranceNumber)

		Dim columnGeschlecht As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGeschlecht.Caption = m_Translate.GetSafeTranslationValue("Geschlecht")
		columnGeschlecht.Name = "Geschlecht"
		columnGeschlecht.FieldName = "Geschlecht"
		columnGeschlecht.Visible = False
		gveQuestValidData.Columns.Add(columnGeschlecht)

		Dim columngebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columngebdat.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columngebdat.Name = "Employeename"
		columngebdat.FieldName = "Employeename"
		columngebdat.Visible = True
		gveQuestValidData.Columns.Add(columngebdat)

		Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
		columnEmployeename.Name = "BirthDay"
		columnEmployeename.FieldName = "BirthDay"
		columnEmployeename.DisplayFormat.FormatString = "g"
		columnEmployeename.Visible = False
		columnEmployeename.BestFit()
		gveQuestValidData.Columns.Add(columnEmployeename)

		Dim columnNationality As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNationality.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnNationality.Caption = m_Translate.GetSafeTranslationValue("Nationalität")
		columnNationality.Name = "Nationality"
		columnNationality.FieldName = "Nationality"
		columnNationality.Visible = False
		columnNationality.BestFit()
		gveQuestValidData.Columns.Add(columnNationality)

		Dim columnEmployeeStreetName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeStreetName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeStreetName.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnEmployeeStreetName.Name = "EmployeeStreetName"
		columnEmployeeStreetName.FieldName = "EmployeeStreetName"
		columnEmployeeStreetName.Visible = False
		columnEmployeeStreetName.BestFit()
		gveQuestValidData.Columns.Add(columnEmployeeStreetName)

		Dim columnEmployeeHouseNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeHouseNumber.Caption = m_Translate.GetSafeTranslationValue("Hausnummer")
		columnEmployeeHouseNumber.Name = "EmployeeHouseNumber"
		columnEmployeeHouseNumber.FieldName = "EmployeeHouseNumber"
		columnEmployeeHouseNumber.Visible = False
		columnEmployeeHouseNumber.BestFit()
		gveQuestValidData.Columns.Add(columnEmployeeHouseNumber)

		Dim columnPostOfficeBox As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostOfficeBox.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnPostOfficeBox.Caption = m_Translate.GetSafeTranslationValue("Postfach")
		columnPostOfficeBox.Name = "PostOfficeBox"
		columnPostOfficeBox.FieldName = "PostOfficeBox"
		columnPostOfficeBox.Visible = False
		columnPostOfficeBox.BestFit()
		gveQuestValidData.Columns.Add(columnPostOfficeBox)

		Dim columnEmployeeAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
		columnEmployeeAddress.Name = "EmployeeAddress"
		columnEmployeeAddress.FieldName = "EmployeeAddress"
		columnEmployeeAddress.BestFit()
		columnEmployeeAddress.Visible = True
		gveQuestValidData.Columns.Add(columnEmployeeAddress)

		Dim columnEmployeecity As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeecity.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnEmployeecity.Name = "Employeecity"
		columnEmployeecity.FieldName = "Employeecity"
		columnEmployeecity.BestFit()
		columnEmployeecity.Visible = False
		gveQuestValidData.Columns.Add(columnEmployeecity)

		Dim columnEmployeecountry As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeecountry.Caption = m_Translate.GetSafeTranslationValue("Wohnsitzstaat")
		columnEmployeecountry.Name = "Employeecountry"
		columnEmployeecountry.FieldName = "Employeecountry"
		columnEmployeecountry.Visible = False
		columnEmployeecountry.BestFit()
		gveQuestValidData.Columns.Add(columnEmployeecountry)

		Dim columnTaxCommunityCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTaxCommunityCode.Caption = m_Translate.GetSafeTranslationValue("Gemeinde Nr.")
		columnTaxCommunityCode.Name = "TaxCommunityCode"
		columnTaxCommunityCode.FieldName = "TaxCommunityCode"
		columnTaxCommunityCode.Visible = False
		columnTaxCommunityCode.BestFit()
		gveQuestValidData.Columns.Add(columnTaxCommunityCode)

		Dim columnEmploymentLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentLocation.Caption = m_Translate.GetSafeTranslationValue("Arbeitsort")
		columnEmploymentLocation.Name = "EmploymentLocation"
		columnEmploymentLocation.FieldName = "EmploymentLocation"
		columnEmploymentLocation.Visible = False
		columnEmploymentLocation.BestFit()
		gveQuestValidData.Columns.Add(columnEmploymentLocation)

		Dim columnEmploymentPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentPostcode.Caption = m_Translate.GetSafeTranslationValue("Arbeitort PLZ")
		columnEmploymentPostcode.Name = "EmploymentPostcode"
		columnEmploymentPostcode.FieldName = "EmploymentPostcode"
		columnEmploymentPostcode.Visible = False
		columnEmploymentPostcode.BestFit()
		gveQuestValidData.Columns.Add(columnEmploymentPostcode)

		Dim columnGemeindeNummer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGemeindeNummer.Caption = m_Translate.GetSafeTranslationValue("Arbeitort Gemeinde Nr.")
		columnGemeindeNummer.Name = "GemeindeNummer"
		columnGemeindeNummer.FieldName = "GemeindeNummer"
		columnGemeindeNummer.Visible = False
		columnGemeindeNummer.BestFit()
		gveQuestValidData.Columns.Add(columnGemeindeNummer)

		Dim columnEmploymentTypeViewData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentTypeViewData.Caption = m_Translate.GetSafeTranslationValue("Beschäftigungsart")
		columnEmploymentTypeViewData.Name = "EmploymentTypeViewData"
		columnEmploymentTypeViewData.FieldName = "EmploymentTypeViewData"
		columnEmploymentTypeViewData.Visible = False
		columnEmploymentTypeViewData.BestFit()
		gveQuestValidData.Columns.Add(columnEmploymentTypeViewData)

		Dim columnESAb As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESAb.Caption = m_Translate.GetSafeTranslationValue("Eintrittsdatum")
		columnESAb.Name = "ESAb"
		columnESAb.FieldName = "ESAb"
		columnESAb.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnESAb.DisplayFormat.FormatString = "g"
		columnESAb.Visible = False
		columnESAb.BestFit()
		gveQuestValidData.Columns.Add(columnESAb)

		Dim columnMutation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMutation.Caption = m_Translate.GetSafeTranslationValue("Mutationsdatum")
		columnMutation.Name = "Mutation"
		columnMutation.FieldName = "Mutation"
		columnMutation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnMutation.DisplayFormat.FormatString = "g"
		columnMutation.Visible = False
		columnMutation.BestFit()
		gveQuestValidData.Columns.Add(columnMutation)


		Dim columnESEnde As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESEnde.Caption = m_Translate.GetSafeTranslationValue("Austrittsdatum")
		columnESEnde.Name = "ESEnde"
		columnESEnde.FieldName = "ESEnde"
		columnESEnde.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnESEnde.DisplayFormat.FormatString = "g"
		columnESEnde.Visible = False
		gveQuestValidData.Columns.Add(columnESEnde)

		Dim columnDismissalreason As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDismissalreason.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDismissalreason.Caption = m_Translate.GetSafeTranslationValue("Austrittsgrund")
		columnDismissalreason.Name = "Dismissalreason"
		columnDismissalreason.FieldName = "Dismissalreason"
		columnDismissalreason.BestFit()
		columnDismissalreason.Visible = False
		gveQuestValidData.Columns.Add(columnDismissalreason)

		Dim columnBruttolohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBruttolohn.Caption = m_Translate.GetSafeTranslationValue("Bruttolohn")
		columnBruttolohn.Name = "Bruttolohn"
		columnBruttolohn.FieldName = "Bruttolohn"
		columnBruttolohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBruttolohn.AppearanceHeader.Options.UseTextOptions = True
		columnBruttolohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBruttolohn.DisplayFormat.FormatString = "N2"
		columnBruttolohn.SummaryItem.DisplayFormat = "{0:n2}"
		columnBruttolohn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnBruttolohn.SummaryItem.Tag = "Sumbruttolohn"
		columnBruttolohn.MaxWidth = 100
		columnBruttolohn.Visible = True
		gveQuestValidData.Columns.Add(columnBruttolohn)


		Dim columnWorkingHoursWeek As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkingHoursWeek.Caption = m_Translate.GetSafeTranslationValue("Arbeitsstunden pro Woche")
		columnWorkingHoursWeek.Name = "WorkingHoursWeek"
		columnWorkingHoursWeek.FieldName = "WorkingHoursWeek"
		columnWorkingHoursWeek.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnWorkingHoursWeek.DisplayFormat.FormatString = "N2"
		columnWorkingHoursWeek.Visible = False
		gveQuestValidData.Columns.Add(columnWorkingHoursWeek)

		Dim columnWorkingPensum As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkingPensum.Caption = m_Translate.GetSafeTranslationValue("Arbeitspensum")
		columnWorkingPensum.Name = "WorkingPensum"
		columnWorkingPensum.FieldName = "WorkingPensum"
		columnWorkingPensum.Visible = False
		gveQuestValidData.Columns.Add(columnWorkingPensum)

		Dim columnVacationDaysCount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVacationDaysCount.Caption = m_Translate.GetSafeTranslationValue("Ferienanspruch")
		columnVacationDaysCount.Name = "VacationDaysCount"
		columnVacationDaysCount.FieldName = "VacationDaysCount"
		columnVacationDaysCount.DisplayFormat.FormatString = "F0"
		columnVacationDaysCount.Visible = False
		gveQuestValidData.Columns.Add(columnVacationDaysCount)

		Dim columnTaxCivilStatusCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTaxCivilStatusCode.Caption = m_Translate.GetSafeTranslationValue("Zivilstand")
		columnTaxCivilStatusCode.Name = "TaxCivilStatusCode"
		columnTaxCivilStatusCode.FieldName = "TaxCivilStatusCode"
		columnTaxCivilStatusCode.Visible = False
		gveQuestValidData.Columns.Add(columnTaxCivilStatusCode)

		Dim columnNumberOfChildren As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNumberOfChildren.Caption = m_Translate.GetSafeTranslationValue("Anzahl Kinder")
		columnNumberOfChildren.Name = "NumberOfChildren"
		columnNumberOfChildren.FieldName = "NumberOfChildren"
		columnNumberOfChildren.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnNumberOfChildren.DisplayFormat.FormatString = "F0"
		columnNumberOfChildren.Visible = False
		gveQuestValidData.Columns.Add(columnNumberOfChildren)

		Dim columnTaxChurchCodeViewData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTaxChurchCodeViewData.Caption = m_Translate.GetSafeTranslationValue("Kirchensteuerpflicht")
		columnTaxChurchCodeViewData.Name = "TaxChurchCodeViewData"
		columnTaxChurchCodeViewData.FieldName = "TaxChurchCodeViewData"
		columnTaxChurchCodeViewData.Visible = False
		gveQuestValidData.Columns.Add(columnTaxChurchCodeViewData)

		Dim columnInEmployment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnInEmployment.Caption = m_Translate.GetSafeTranslationValue("Partner erwerbstätig")
		columnInEmployment.Name = "InEmployment"
		columnInEmployment.FieldName = "InEmployment"
		columnInEmployment.Visible = False
		gveQuestValidData.Columns.Add(columnInEmployment)

		Dim columnPartnerFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerFirstname.Caption = m_Translate.GetSafeTranslationValue("Partner Vorname")
		columnPartnerFirstname.Name = "PartnerFirstname"
		columnPartnerFirstname.FieldName = "PartnerFirstname"
		columnPartnerFirstname.Visible = False
		gveQuestValidData.Columns.Add(columnPartnerFirstname)

		Dim columnPartnerLastName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerLastName.Caption = m_Translate.GetSafeTranslationValue("Partner Nachname")
		columnPartnerLastName.Name = "PartnerLastName"
		columnPartnerLastName.FieldName = "PartnerLastName"
		columnPartnerLastName.Visible = False
		gveQuestValidData.Columns.Add(columnPartnerLastName)

		Dim columnpartnerStreetName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnpartnerStreetName.Caption = m_Translate.GetSafeTranslationValue("Partner Strasse")
		columnpartnerStreetName.Name = "partnerStreetName"
		columnpartnerStreetName.FieldName = "partnerStreetName"
		columnpartnerStreetName.Visible = False
		gveQuestValidData.Columns.Add(columnpartnerStreetName)

		Dim columnPartnerHouseNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerHouseNumber.Caption = m_Translate.GetSafeTranslationValue("Partner Hausnummer")
		columnPartnerHouseNumber.Name = "PartnerHouseNumber"
		columnPartnerHouseNumber.FieldName = "PartnerHouseNumber"
		columnPartnerHouseNumber.Visible = False
		gveQuestValidData.Columns.Add(columnPartnerHouseNumber)

		Dim columnPartnerPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerPostcode.Caption = m_Translate.GetSafeTranslationValue("Partner PLZ")
		columnPartnerPostcode.Name = "PartnerPostcode"
		columnPartnerPostcode.FieldName = "PartnerPostcode"
		columnPartnerPostcode.Visible = False
		gveQuestValidData.Columns.Add(columnPartnerPostcode)

		Dim columnPartnerCity As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerCity.Caption = m_Translate.GetSafeTranslationValue("Partner Stadt")
		columnPartnerCity.Name = "PartnerCity"
		columnPartnerCity.FieldName = "PartnerCity"
		columnPartnerCity.Visible = False
		gveQuestValidData.Columns.Add(columnPartnerCity)

		Dim columnPartnerCountry As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerCountry.Caption = m_Translate.GetSafeTranslationValue("Partner Wohnsitzstaat")
		columnPartnerCountry.Name = "PartnerCountry"
		columnPartnerCountry.FieldName = "PartnerCountry"
		columnPartnerCountry.Visible = False
		gveQuestValidData.Columns.Add(columnPartnerCountry)



		grdeQuestValidData.DataSource = Nothing

	End Sub

	Private Sub ResetGridInvalideQuestData()

		gveQuestInvalidData.OptionsView.ShowIndicator = False
		gveQuestInvalidData.OptionsView.ShowAutoFilterRow = True
		gveQuestInvalidData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gveQuestInvalidData.OptionsView.ShowFooter = True

		gveQuestInvalidData.Columns.Clear()


		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MaNr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		gveQuestInvalidData.Columns.Add(columnMANr)

		Dim columnAbrechnungsstelle As New DevExpress.XtraGrid.Columns.GridColumn()
		columnAbrechnungsstelle.Caption = m_Translate.GetSafeTranslationValue("Abrechnungsstelle")
		columnAbrechnungsstelle.Name = "Abrechnungsstelle"
		columnAbrechnungsstelle.FieldName = "Abrechnungsstelle"
		columnAbrechnungsstelle.Visible = False
		gveQuestInvalidData.Columns.Add(columnAbrechnungsstelle)

		Dim columnSocialInsuranceNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnSocialInsuranceNumber.Caption = m_Translate.GetSafeTranslationValue("AHVN13")
		columnSocialInsuranceNumber.Name = "SocialInsuranceNumber"
		columnSocialInsuranceNumber.FieldName = "SocialInsuranceNumber"
		columnSocialInsuranceNumber.Visible = False
		gveQuestInvalidData.Columns.Add(columnSocialInsuranceNumber)

		Dim columnGeschlecht As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGeschlecht.Caption = m_Translate.GetSafeTranslationValue("Geschlecht")
		columnGeschlecht.Name = "Geschlecht"
		columnGeschlecht.FieldName = "Geschlecht"
		columnGeschlecht.Visible = False
		gveQuestInvalidData.Columns.Add(columnGeschlecht)

		Dim columngebdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columngebdat.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columngebdat.Name = "Employeename"
		columngebdat.FieldName = "Employeename"
		columngebdat.Visible = True
		gveQuestInvalidData.Columns.Add(columngebdat)

		Dim columnEmployeename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeename.Caption = m_Translate.GetSafeTranslationValue("Geburtsdatum")
		columnEmployeename.Name = "BirthDay"
		columnEmployeename.FieldName = "BirthDay"
		columnEmployeename.DisplayFormat.FormatString = "g"
		columnEmployeename.Visible = False
		columnEmployeename.BestFit()
		gveQuestInvalidData.Columns.Add(columnEmployeename)

		Dim columnNationality As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNationality.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnNationality.Caption = m_Translate.GetSafeTranslationValue("Nationalität")
		columnNationality.Name = "Nationality"
		columnNationality.FieldName = "Nationality"
		columnNationality.Visible = False
		columnNationality.BestFit()
		gveQuestInvalidData.Columns.Add(columnNationality)

		Dim columnEmployeeStreetName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeStreetName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnEmployeeStreetName.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnEmployeeStreetName.Name = "EmployeeStreetName"
		columnEmployeeStreetName.FieldName = "EmployeeStreetName"
		columnEmployeeStreetName.Visible = False
		columnEmployeeStreetName.BestFit()
		gveQuestInvalidData.Columns.Add(columnEmployeeStreetName)

		Dim columnEmployeeHouseNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeHouseNumber.Caption = m_Translate.GetSafeTranslationValue("Hausnummer")
		columnEmployeeHouseNumber.Name = "EmployeeHouseNumber"
		columnEmployeeHouseNumber.FieldName = "EmployeeHouseNumber"
		columnEmployeeHouseNumber.Visible = False
		columnEmployeeHouseNumber.BestFit()
		gveQuestInvalidData.Columns.Add(columnEmployeeHouseNumber)

		Dim columnPostOfficeBox As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostOfficeBox.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnPostOfficeBox.Caption = m_Translate.GetSafeTranslationValue("Postfach")
		columnPostOfficeBox.Name = "PostOfficeBox"
		columnPostOfficeBox.FieldName = "PostOfficeBox"
		columnPostOfficeBox.Visible = False
		columnPostOfficeBox.BestFit()
		gveQuestInvalidData.Columns.Add(columnPostOfficeBox)

		Dim columnEmployeeAddress As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeeAddress.Caption = m_Translate.GetSafeTranslationValue("Adresse")
		columnEmployeeAddress.Name = "EmployeeAddress"
		columnEmployeeAddress.FieldName = "EmployeeAddress"
		columnEmployeeAddress.BestFit()
		columnEmployeeAddress.Visible = True
		gveQuestInvalidData.Columns.Add(columnEmployeeAddress)

		Dim columnEmployeecity As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeecity.Caption = m_Translate.GetSafeTranslationValue("Ort")
		columnEmployeecity.Name = "Employeecity"
		columnEmployeecity.FieldName = "Employeecity"
		columnEmployeecity.BestFit()
		columnEmployeecity.Visible = False
		gveQuestInvalidData.Columns.Add(columnEmployeecity)

		Dim columnEmployeecountry As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmployeecountry.Caption = m_Translate.GetSafeTranslationValue("Wohnsitzstaat")
		columnEmployeecountry.Name = "Employeecountry"
		columnEmployeecountry.FieldName = "Employeecountry"
		columnEmployeecountry.Visible = False
		columnEmployeecountry.BestFit()
		gveQuestInvalidData.Columns.Add(columnEmployeecountry)

		Dim columnTaxCommunityCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTaxCommunityCode.Caption = m_Translate.GetSafeTranslationValue("Gemeinde Nr.")
		columnTaxCommunityCode.Name = "TaxCommunityCode"
		columnTaxCommunityCode.FieldName = "TaxCommunityCode"
		columnTaxCommunityCode.Visible = False
		columnTaxCommunityCode.BestFit()
		gveQuestInvalidData.Columns.Add(columnTaxCommunityCode)

		Dim columnEmploymentLocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentLocation.Caption = m_Translate.GetSafeTranslationValue("Arbeitsort")
		columnEmploymentLocation.Name = "EmploymentLocation"
		columnEmploymentLocation.FieldName = "EmploymentLocation"
		columnEmploymentLocation.Visible = False
		columnEmploymentLocation.BestFit()
		gveQuestInvalidData.Columns.Add(columnEmploymentLocation)

		Dim columnEmploymentPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentPostcode.Caption = m_Translate.GetSafeTranslationValue("Arbeitort PLZ")
		columnEmploymentPostcode.Name = "EmploymentPostcode"
		columnEmploymentPostcode.FieldName = "EmploymentPostcode"
		columnEmploymentPostcode.Visible = False
		columnEmploymentPostcode.BestFit()
		gveQuestInvalidData.Columns.Add(columnEmploymentPostcode)

		Dim columnGemeindeNummer As New DevExpress.XtraGrid.Columns.GridColumn()
		columnGemeindeNummer.Caption = m_Translate.GetSafeTranslationValue("Arbeitort Gemeinde Nr.")
		columnGemeindeNummer.Name = "GemeindeNummer"
		columnGemeindeNummer.FieldName = "GemeindeNummer"
		columnGemeindeNummer.Visible = False
		columnGemeindeNummer.BestFit()
		gveQuestInvalidData.Columns.Add(columnGemeindeNummer)

		Dim columnEmploymentTypeViewData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnEmploymentTypeViewData.Caption = m_Translate.GetSafeTranslationValue("Beschäftigungsart")
		columnEmploymentTypeViewData.Name = "EmploymentTypeViewData"
		columnEmploymentTypeViewData.FieldName = "EmploymentTypeViewData"
		columnEmploymentTypeViewData.Visible = False
		columnEmploymentTypeViewData.BestFit()
		gveQuestInvalidData.Columns.Add(columnEmploymentTypeViewData)

		Dim columnESAb As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESAb.Caption = m_Translate.GetSafeTranslationValue("Eintrittsdatum")
		columnESAb.Name = "ESAb"
		columnESAb.FieldName = "ESAb"
		columnESAb.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnESAb.DisplayFormat.FormatString = "G"
		columnESAb.Visible = False
		columnESAb.BestFit()
		gveQuestInvalidData.Columns.Add(columnESAb)

		Dim columnMutation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMutation.Caption = m_Translate.GetSafeTranslationValue("Mutationsdatum")
		columnMutation.Name = "Mutation"
		columnMutation.FieldName = "Mutation"
		columnMutation.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnMutation.DisplayFormat.FormatString = "g"
		columnMutation.Visible = False
		columnMutation.BestFit()
		gveQuestInvalidData.Columns.Add(columnMutation)


		Dim columnESEnde As New DevExpress.XtraGrid.Columns.GridColumn()
		columnESEnde.Caption = m_Translate.GetSafeTranslationValue("Austrittsdatum")
		columnESEnde.Name = "ESEnde"
		columnESEnde.FieldName = "ESEnde"
		columnESEnde.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnESEnde.DisplayFormat.FormatString = "g"
		columnESEnde.Visible = False
		gveQuestInvalidData.Columns.Add(columnESEnde)

		Dim columnDismissalreason As New DevExpress.XtraGrid.Columns.GridColumn()
		columnDismissalreason.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnDismissalreason.Caption = m_Translate.GetSafeTranslationValue("Austrittsgrund")
		columnDismissalreason.Name = "Dismissalreason"
		columnDismissalreason.FieldName = "Dismissalreason"
		columnDismissalreason.BestFit()
		columnDismissalreason.Visible = False
		gveQuestInvalidData.Columns.Add(columnDismissalreason)

		Dim columnBruttolohn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBruttolohn.Caption = m_Translate.GetSafeTranslationValue("Bruttolohn")
		columnBruttolohn.Name = "Bruttolohn"
		columnBruttolohn.FieldName = "Bruttolohn"
		columnBruttolohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		columnBruttolohn.AppearanceHeader.Options.UseTextOptions = True
		columnBruttolohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnBruttolohn.DisplayFormat.FormatString = "N2"
		columnBruttolohn.SummaryItem.DisplayFormat = "{0:n2}"
		columnBruttolohn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
		columnBruttolohn.SummaryItem.Tag = "Sumbruttolohn"
		columnBruttolohn.MaxWidth = 100
		columnBruttolohn.Visible = True
		gveQuestInvalidData.Columns.Add(columnBruttolohn)


		Dim columnWorkingHoursWeek As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkingHoursWeek.Caption = m_Translate.GetSafeTranslationValue("Arbeitsstunden pro Woche")
		columnWorkingHoursWeek.Name = "WorkingHoursWeek"
		columnWorkingHoursWeek.FieldName = "WorkingHoursWeek"
		columnWorkingHoursWeek.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnWorkingHoursWeek.DisplayFormat.FormatString = "N2"
		columnWorkingHoursWeek.Visible = False
		gveQuestInvalidData.Columns.Add(columnWorkingHoursWeek)

		Dim columnWorkingPensum As New DevExpress.XtraGrid.Columns.GridColumn()
		columnWorkingPensum.Caption = m_Translate.GetSafeTranslationValue("Arbeitspensum")
		columnWorkingPensum.Name = "WorkingPensum"
		columnWorkingPensum.FieldName = "WorkingPensum"
		columnWorkingPensum.Visible = False
		gveQuestInvalidData.Columns.Add(columnWorkingPensum)

		Dim columnVacationDaysCount As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVacationDaysCount.Caption = m_Translate.GetSafeTranslationValue("Ferienanspruch")
		columnVacationDaysCount.Name = "VacationDaysCount"
		columnVacationDaysCount.FieldName = "VacationDaysCount"
		columnVacationDaysCount.DisplayFormat.FormatString = "F0"
		columnVacationDaysCount.Visible = False
		gveQuestInvalidData.Columns.Add(columnVacationDaysCount)

		Dim columnTaxCivilStatusCode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTaxCivilStatusCode.Caption = m_Translate.GetSafeTranslationValue("Zivilstand")
		columnTaxCivilStatusCode.Name = "TaxCivilStatusCode"
		columnTaxCivilStatusCode.FieldName = "TaxCivilStatusCode"
		columnTaxCivilStatusCode.Visible = False
		gveQuestInvalidData.Columns.Add(columnTaxCivilStatusCode)

		Dim columnNumberOfChildren As New DevExpress.XtraGrid.Columns.GridColumn()
		columnNumberOfChildren.Caption = m_Translate.GetSafeTranslationValue("Anzahl Kinder")
		columnNumberOfChildren.Name = "NumberOfChildren"
		columnNumberOfChildren.FieldName = "NumberOfChildren"
		columnNumberOfChildren.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnNumberOfChildren.DisplayFormat.FormatString = "F0"
		columnNumberOfChildren.Visible = False
		gveQuestInvalidData.Columns.Add(columnNumberOfChildren)

		Dim columnTaxChurchCodeViewData As New DevExpress.XtraGrid.Columns.GridColumn()
		columnTaxChurchCodeViewData.Caption = m_Translate.GetSafeTranslationValue("Kirchensteuerpflicht")
		columnTaxChurchCodeViewData.Name = "TaxChurchCodeViewData"
		columnTaxChurchCodeViewData.FieldName = "TaxChurchCodeViewData"
		columnTaxChurchCodeViewData.Visible = False
		gveQuestInvalidData.Columns.Add(columnTaxChurchCodeViewData)

		Dim columnInEmployment As New DevExpress.XtraGrid.Columns.GridColumn()
		columnInEmployment.Caption = m_Translate.GetSafeTranslationValue("Partner erwerbstätig")
		columnInEmployment.Name = "InEmployment"
		columnInEmployment.FieldName = "InEmployment"
		columnInEmployment.Visible = False
		gveQuestInvalidData.Columns.Add(columnInEmployment)

		Dim columnPartnerFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerFirstname.Caption = m_Translate.GetSafeTranslationValue("Partner Vorname")
		columnPartnerFirstname.Name = "PartnerFirstname"
		columnPartnerFirstname.FieldName = "PartnerFirstname"
		columnPartnerFirstname.Visible = False
		gveQuestInvalidData.Columns.Add(columnPartnerFirstname)

		Dim columnPartnerLastName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerLastName.Caption = m_Translate.GetSafeTranslationValue("Partner Nachname")
		columnPartnerLastName.Name = "PartnerLastName"
		columnPartnerLastName.FieldName = "PartnerLastName"
		columnPartnerLastName.Visible = False
		gveQuestInvalidData.Columns.Add(columnPartnerLastName)

		Dim columnpartnerStreetName As New DevExpress.XtraGrid.Columns.GridColumn()
		columnpartnerStreetName.Caption = m_Translate.GetSafeTranslationValue("Partner Strasse")
		columnpartnerStreetName.Name = "partnerStreetName"
		columnpartnerStreetName.FieldName = "partnerStreetName"
		columnpartnerStreetName.Visible = False
		gveQuestInvalidData.Columns.Add(columnpartnerStreetName)

		Dim columnPartnerHouseNumber As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerHouseNumber.Caption = m_Translate.GetSafeTranslationValue("Partner Hausnummer")
		columnPartnerHouseNumber.Name = "PartnerHouseNumber"
		columnPartnerHouseNumber.FieldName = "PartnerHouseNumber"
		columnPartnerHouseNumber.Visible = False
		gveQuestInvalidData.Columns.Add(columnPartnerHouseNumber)

		Dim columnPartnerPostcode As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerPostcode.Caption = m_Translate.GetSafeTranslationValue("Partner PLZ")
		columnPartnerPostcode.Name = "PartnerPostcode"
		columnPartnerPostcode.FieldName = "PartnerPostcode"
		columnPartnerPostcode.Visible = False
		gveQuestInvalidData.Columns.Add(columnPartnerPostcode)

		Dim columnPartnerCity As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerCity.Caption = m_Translate.GetSafeTranslationValue("Partner Stadt")
		columnPartnerCity.Name = "PartnerCity"
		columnPartnerCity.FieldName = "PartnerCity"
		columnPartnerCity.Visible = False
		gveQuestInvalidData.Columns.Add(columnPartnerCity)

		Dim columnPartnerCountry As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPartnerCountry.Caption = m_Translate.GetSafeTranslationValue("Partner Wohnsitzstaat")
		columnPartnerCountry.Name = "PartnerCountry"
		columnPartnerCountry.FieldName = "PartnerCountry"
		columnPartnerCountry.Visible = False
		gveQuestInvalidData.Columns.Add(columnPartnerCountry)

		Dim columnErrorReason As New DevExpress.XtraGrid.Columns.GridColumn()
		columnErrorReason.Caption = m_Translate.GetSafeTranslationValue("Fehler")
		columnErrorReason.Name = "ErrorReason"
		columnErrorReason.FieldName = "ErrorReason"
		columnErrorReason.Visible = True
		gveQuestInvalidData.Columns.Add(columnErrorReason)


		grdeQuestInvalidData.DataSource = Nothing

	End Sub

	Private Function LoadDataForeQuest() As Boolean
		Dim success As Boolean = True
		Dim eQuestPath As String = txt_eQuestFilename.EditValue

		If String.IsNullOrWhiteSpace(eQuestPath) OrElse Not Directory.Exists(Path.GetDirectoryName(eQuestPath)) Then eQuestPath = m_InitializationData.UserData.spAllowedPath
		txt_eQuestFilename.EditValue = eQuestPath

		If LoadedQSTData Is Nothing OrElse LoadedQSTData.Count = 0 Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

			Return False
		End If

		Try
			Dim result As Boolean
			Dim extendedResult As ClsExtendedResult = Nothing
			result = ExportForeQuest()


			My.Settings.Filename4eQuest = txt_eQuestFilename.EditValue
			My.Settings.Save()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			SplashScreenManager.CloseForm(False)
			m_UtilityUi.ShowErrorDialog(String.Format("{0}", ex.ToString))

			Return False

		End Try


		Return success

	End Function

	Function ExportForeQuest() As Boolean
		Dim success As Boolean = True

		Try
			SplashScreenManager.CloseForm(False)
			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Vorlage wird übersetzt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			success = success AndAlso CreateNewCSVImportDataForeQuest()
			success = success AndAlso CreateNewCSVAbrechnungForeQuest()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)

			If success Then
				m_UtilityUi.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich exportiert."))
				If gveQuestInvalidData.RowCount > 0 Then
					m_UtilityUi.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("<b>Achtung:</b><br>Es sind {0} Kandidaten welche nicht übermittelt werden können!"), gveQuestInvalidData.RowCount))
				End If

				Process.Start(txt_eQuestFilename.EditValue)

			Else
				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ihre Daten kontten nicht exportiert werden!"))

			End If


		End Try

		Return success
	End Function

	Private Function LoadImportDataForeQuest() As Boolean
		Dim success As Boolean = True

		Dim validData = New BindingList(Of EQuerstViewData)
		Dim inValidData = New BindingList(Of EQuerstViewData)

		Try

			For Each itm In LoadedQSTData
				Dim data = New EQuerstViewData
				Dim isValid As Boolean = True
				Dim employeeStreetName As String = String.Empty
				Dim employeeHouseNumber As String = String.Empty

				success = success AndAlso LoadEmployeeData(itm.MANr)
				If Not success Then Continue For

				Dim employeeTaxCommunityCode As Integer = itm.TaxCommunityCode.GetValueOrDefault(0)
				If employeeTaxCommunityCode = 0 Then
					Dim communityData = m_CommunityData.Where(Function(x) x.Translated_Value = m_EmployeeMasterData.Location).FirstOrDefault
					If Not communityData Is Nothing Then
						employeeTaxCommunityCode = communityData.BFSNumber
						itm.TaxCommunityCode = employeeTaxCommunityCode
					End If
				End If

				Dim employmentTaxCommunityCode As Integer = 0
				If itm.employeecountry <> "CH" Then
					itm.EmploymentCommunityCode = m_CompanyTaxCommunityCode
					itm.EmploymentLocation = m_InitializationData.MDData.MDCity
					itm.EmploymentPostcode = m_InitializationData.MDData.MDPostcode
					employmentTaxCommunityCode = m_CompanyTaxCommunityCode
				End If


				If Not String.IsNullOrWhiteSpace(m_EmployeeMasterData.Street) Then
					Dim adressString = m_EmployeeMasterData.Street
					Dim i As Integer = 0
					Dim streetData = adressString.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)

					employeeHouseNumber = streetData(streetData.Length - 1)
					If Val(employeeHouseNumber) = 0 Then employeeHouseNumber = streetData(0)
					For Each strItm In streetData
						If i < streetData.Length - 1 Then
							employeeStreetName = String.Format("{0}{1}{2}", employeeStreetName, If(String.IsNullOrWhiteSpace(employeeStreetName), "", " "), strItm)
						End If
						i += 1
					Next

				End If

				Dim partnerStreetName As String = String.Empty
				Dim partnerHouseNumber As String = String.Empty

				If Not String.IsNullOrWhiteSpace(itm.PartnerStreet) Then
					Dim adressString = itm.PartnerStreet
					Dim i As Integer = 0
					Dim streetData = adressString.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)

					partnerHouseNumber = streetData(streetData.Length - 1)
					For Each strItm In streetData
						If i < streetData.Length - 1 Then
							partnerStreetName = String.Format("{0}{1}{2}", partnerStreetName, If(String.IsNullOrWhiteSpace(partnerStreetName), "", " "), strItm)
						End If
						i += 1
					Next

				End If
				Try
					Dim dismissalreason As String = String.Empty


					' 5.	Der Austrittsgründe sind vordefiniert: WITHDRAWAL_COMPANY, NATURALIZATION, SETTLED_C, TEMPORARY, CANTON_CHANGE, OTHERS 
					itm.Dismissalreason = dismissalreason

					data.MDNr = m_InitializationData.MDData.MDNr
					data.EmployeeNumber = itm.MANr
					data.Abrechnungsstelle = m_AccountNumber
					data.SocialInsuranceNumber = itm.SocialInsuranceNumber
					data.Geschlecht = If(itm.geschlecht = "M", "M", "F")
					data.Employeename = m_EmployeeMasterData.EmployeeFullname
					data.BirthDay = itm.gebdat
					data.Nationality = m_EmployeeMasterData.Nationality


					data.EmployeeStreetName = employeeStreetName
					data.EmployeeHouseNumber = employeeHouseNumber
					data.PostOfficeBox = m_EmployeeMasterData.PostOfficeBox
					data.Employeepostcode = itm.employeepostcode
					data.Employeecity = itm.employeecity
					data.Employeecountry = itm.employeecountry
					data.TaxCommunityCode = employeeTaxCommunityCode
					data.EmploymentLocation = itm.EmploymentLocation
					data.EmploymentPostcode = itm.EmploymentPostcode
					data.GemeindeNummer = employmentTaxCommunityCode
					data.EmploymentTypeViewData = itm.EmploymentTypeViewData
					data.ESAb = itm.esab
					data.Mutation = Nothing
					data.ESEnde = itm.esende
					data.Dismissalreason = dismissalreason
					data.Bruttolohn = itm.Bruttolohn
					data.WorkingHoursWeek = itm.WorkingHoursWeek
					data.WorkingPensum = itm.WorkingPensum
					data.VacationDaysCount = itm.VacationDaysCount
					data.TaxCivilStatusCode = itm.TaxCivilStatusCode
					data.NumberOfChildren = itm.NumberOfChildren
					data.TaxChurchCodeViewData = itm.TaxChurchCodeViewData
					data.InEmployment = itm.InEmployment
					data.PartnerFirstname = itm.PartnerFirstname
					data.PartnerLastName = itm.PartnerLastName
					data.PartnerStreetName = partnerStreetName
					data.PartnerHouseNumber = partnerHouseNumber
					data.PartnerPostcode = itm.PartnerPostcode
					data.PartnerCity = itm.PartnerCity
					data.PartnerCountry = itm.PartnerCountry

				Catch ex As Exception
					m_Logger.LogError(ex.ToString)

				End Try

				If itm.PermissionCode = "G" Then
					data.TaxCommunityCode = Nothing
					data.EmploymentPostcode = m_InitializationData.MDData.MDPostcode
					data.GemeindeNummer = m_CompanyTaxCommunityCode
				End If

				If ((itm.TaxCivilStatusCode = "MARRIED" OrElse itm.TaxCivilStatusCode = "REGISTERED_PARTNERSHIP") AndAlso itm.EmployeePartnerRecID.GetValueOrDefault(0) = 0) Then
					data.ErrorReason = String.Format("Zivilstand: {0} >>> PartnerID: {1}", itm.TaxCivilStatusCode, itm.EmployeePartnerRecID)
					m_Logger.LogWarning(String.Format("import is not valid! {0} >>> TaxCivilStatusCode: {1} ", m_EmployeeMasterData.EmployeeFullname, itm.TaxCivilStatusCode))

					isValid = False
				End If
				If employeeTaxCommunityCode = 0 AndAlso employmentTaxCommunityCode = 0 Then
					data.ErrorReason = String.Format("Gemeinde Nummer: {0}", employeeTaxCommunityCode)
					m_Logger.LogWarning(String.Format("import is not valid! {0} >>> taxCommunityCode: {1} ", m_EmployeeMasterData.EmployeeFullname, employeeTaxCommunityCode))

					isValid = False
				End If


				If Not isValid Then
					inValidData.Add(data)
				Else
					validData.Add(data)
				End If

			Next

			grdeQuestValidData.DataSource = validData
			grdeQuestValidData.ForceInitialize()

			grdeQuestInvalidData.DataSource = inValidData
			grdeQuestInvalidData.ForceInitialize()

			tnpInvalidData.Caption = String.Format(m_Translate.GetSafeTranslationValue("Ungültige Daten (Daten werden NICHT übermittelt): {0}"), inValidData.Count)

			Dim pane = (TryCast(tpeQuestData, INavigationPane)).ButtonsPanel.Buttons(3).Properties.Appearance
			'pane.Font = New Font(pane.Font, FontStyle.Underline)
			pane.ForeColor = If(inValidData.Count > 0, Color.Red, Color.Black)
			pane.Image = If(inValidData.Count > 0, My.Resources.warning_16x16, Nothing)
			pane.Options.UseFont = True
			pane.Options.UseImage = True


			tnpInvalidData.Enabled = inValidData.Count > 0
			bbiExport.Enabled = validData.Count > 0



		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return False

		Finally

		End Try


		Return success
	End Function

	Private Function CreateNewCSVImportDataForeQuest() As Boolean
		Dim success As Boolean = True

		Try

			Dim eQuestPath As String = txt_eQuestFilename.EditValue
			Dim eQuestFilename As String

			eQuestFilename = Path.Combine(eQuestPath, "eQuestListe_Importdaten.CSV")

			Dim Abrechnungsstelle As String = m_AccountNumber

			Dim headerListToImport = New List(Of String) From {"Abrechnungsstelle", "AHVN13", "Geschlecht",
							"Vorname", "Nachname", "Geburtsdatum", "Nationalität", "Aufenthaltskategorie",
							"Strasse", "Hausnummer", "Postfach", "PLZ", "Ort", "Wohnsitzstaat", "Gemeinde Nr.", "Arbeitsort", "Arbeitort PLZ", "Arbeitort Gemeinde Nr.",
							"Beschäftigungsart", "Eintrittsdatum", "Mutationsdatum", "Austrittsdatum", "Austrittsgrund", "Bruttolohn", "Arbeitsstunden pro Woche", "Arbeitspensum",
							"Ferienanspruch", "Zivilstand", "Anzahl Kinder", "Kirchensteuerpflicht",
							"Partner erwerbstätig", "Partner Vorname", "Partner Nachname", "Partner Strasse", "Partner Hausnummer", "Partner PLZ", "Partner Stadt", "Partner Wohnsitzstaat"}

			Dim contentRows = New List(Of String) From {String.Join(CSV_SEPRATOR, headerListToImport)}

			For Each itm In LoadedQSTData
				success = success AndAlso LoadEmployeeData(itm.MANr)

				If itm.PermissionCode = "G" Then
					itm.TaxCommunityCode = Nothing
					itm.EmploymentPostcode = m_InitializationData.MDData.MDPostcode
					itm.EmploymentCommunityCode = m_CompanyTaxCommunityCode
				End If

				If ((itm.TaxCivilStatusCode = "MARRIED" OrElse itm.TaxCivilStatusCode = "REGISTERED_PARTNERSHIP") AndAlso itm.EmployeePartnerRecID.GetValueOrDefault(0) = 0) Then
					m_Logger.LogWarning(String.Format("import is not valid! {0} >>> TaxCivilStatusCode: {1} ", m_EmployeeMasterData.EmployeeFullname, itm.TaxCivilStatusCode))

					success = False
				End If
				If itm.TaxCommunityCode.GetValueOrDefault(0) = 0 AndAlso itm.EmploymentCommunityCode = 0 Then
					m_Logger.LogWarning(String.Format("import is not valid! {0} >>> taxCommunityCode: {1} | EmploymentCommunityCode: {2} ", m_EmployeeMasterData.EmployeeFullname, itm.TaxCommunityCode.GetValueOrDefault(0), itm.EmploymentCommunityCode))

					success = False
				End If


				If Not success Then Continue For

				Dim employeeStreetName As String = String.Empty
				Dim employeeHouseNumber As String = String.Empty

				If Not String.IsNullOrWhiteSpace(m_EmployeeMasterData.Street) Then
					Dim adressString = m_EmployeeMasterData.Street
					Dim i As Integer = 0
					Dim streetData = adressString.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)

					employeeHouseNumber = streetData(streetData.Length - 1)
					If Val(employeeHouseNumber) = 0 Then employeeHouseNumber = streetData(0)
					For Each strItm In streetData
						If i < streetData.Length - 1 Then
							employeeStreetName = String.Format("{0}{1}{2}", employeeStreetName, If(String.IsNullOrWhiteSpace(employeeStreetName), "", " "), strItm)
						End If
						i += 1
					Next

				End If

				Dim partnerStreetName As String = String.Empty
				Dim partnerHouseNumber As String = String.Empty

				If Not String.IsNullOrWhiteSpace(itm.PartnerStreet) Then
					Dim adressString = itm.PartnerStreet
					Dim i As Integer = 0
					Dim streetData = adressString.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)

					partnerHouseNumber = streetData(streetData.Length - 1)
					For Each strItm In streetData
						If i < streetData.Length - 1 Then
							partnerStreetName = String.Format("{0}{1}{2}", partnerStreetName, If(String.IsNullOrWhiteSpace(partnerStreetName), "", " "), strItm)
						End If
						i += 1
					Next
				End If

				' If(itm.esende Is Nothing, String.Empty, Format("{0:dd.MM.yyyy", Convert.ToDateTime(itm.esende)))
				Dim row = New List(Of String) From {Abrechnungsstelle,
					itm.SocialInsuranceNumber,
					If(itm.geschlecht = "M", "M", "F"),
					m_EmployeeMasterData.Firstname,
					m_EmployeeMasterData.Lastname,
					String.Format("{0:dd.MM.yyyy}", Convert.ToDateTime(itm.gebdat)),
					m_EmployeeMasterData.Nationality,
					itm.TypeofStayViewData,
					employeeStreetName,
					employeeHouseNumber,
					m_EmployeeMasterData.PostOfficeBox,
					itm.employeepostcode,
					itm.employeecity,
					itm.employeecountry,
					If(itm.employeecountry <> "CH", String.Empty, itm.TaxCommunityCode),
					If(itm.employeecountry = "CH", String.Empty, itm.EmploymentLocation),
					If(itm.employeecountry = "CH", String.Empty, itm.EmploymentPostcode),
					If(itm.employeecountry = "CH", String.Empty, itm.EmploymentCommunityCode),
					itm.EmploymentTypeViewData,
					If(itm.esab Is Nothing, String.Empty, Format("{0:dd.MM.yyyy", Convert.ToDateTime(itm.esab))),
					String.Empty,
					String.Empty,
					itm.Dismissalreason,
					itm.Bruttolohn,
					itm.WorkingHoursWeek,
					itm.WorkingPensum,
					itm.VacationDaysCount,
					itm.TaxCivilStatusCode,
					itm.NumberOfChildren,
					itm.TaxChurchCodeViewData}

				If Not (itm.TaxCivilStatusCode = "MARRIED" OrElse itm.TaxCivilStatusCode = "REGISTERED_PARTNERSHIP") Then
					row.Add(String.Empty)
					row.Add(String.Empty)
					row.Add(String.Empty)
					row.Add(String.Empty)
					row.Add(String.Empty)
					row.Add(String.Empty)
					row.Add(String.Empty)
					row.Add(String.Empty)

				Else
					row.Add(If(itm.InEmployment, "yes", "no"))
					row.Add(itm.PartnerFirstname)
					row.Add(itm.PartnerLastName)
					row.Add(partnerStreetName)
					row.Add(partnerHouseNumber)
					row.Add(itm.PartnerPostcode)
					row.Add(itm.PartnerCity)
					row.Add(itm.PartnerCountry)

				End If


				contentRows.Add(String.Join(CSV_SEPRATOR, row))
			Next

			IO.File.WriteAllLines(eQuestFilename, contentRows.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			SplashScreenManager.CloseForm(False)

			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return False

		Finally

		End Try


		Return success
	End Function

	Private Function CreateNewCSVAbrechnungForeQuest() As Boolean
		Dim success As Boolean = True

		Try
			Dim eQuestPath As String = txt_eQuestFilename.EditValue
			Dim eQuestFilename As String

			eQuestFilename = Path.Combine(eQuestPath, "eQuestListe_Abrechnung_Import.CSV")

			Dim Abrechnungsstelle As String = m_AccountNumber

			Dim headerListToImport = New List(Of String) From {"Abrechnungsstelle",
							"AHVN13",
							"Jahr",
							"Monat",
							"Bruttolohn",
							"Tage",
							"Satzbestimmend"}

			Dim contentRows = New List(Of String) From {String.Join(CSV_SEPRATOR, headerListToImport)}

			For Each itm In LoadedQSTData
				success = success AndAlso LoadEmployeeData(itm.MANr)
				If Not success Then Continue For

				Dim row = New List(Of String) From {Abrechnungsstelle,
											  itm.SocialInsuranceNumber,
											  itm.jahr,
											  itm.monat,
											  itm.Bruttolohn,
											  itm.workeddays,
											  itm.qstbasis}


				contentRows.Add(String.Join(CSV_SEPRATOR, row))
			Next

			IO.File.WriteAllLines(eQuestFilename, contentRows.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			SplashScreenManager.CloseForm(False)

			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return False

		Finally

		End Try


		Return success
	End Function

	Private Function LoadCommunityOverWebService() As Boolean
		Dim language = m_InitializationData.UserData.UserLanguage

		m_CommunityData = m_BaseTableUtil.PerformCommunityDataOverWebService(String.Empty, language)
		If m_CommunityData Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gemeinde Daten konnten nicht geladen werden."))

			Return False
		End If

		Return Not m_CommunityData Is Nothing
	End Function


	Private Function CreateNewXLSImportDataForeQuest() As Boolean
		Dim success As Boolean = True

		Try

			Dim eQuestPath As String = txt_eQuestFilename.EditValue
			Dim eQuestFilename As String

			eQuestFilename = Path.Combine(eQuestPath, "eQuestListe_Importdaten.xlsx")

			' Ensure that the data-aware export mode is enabled.
			ExportSettings.DefaultExportType = ExportType.DataAware

			Dim Abrechnungsstelle As String = m_AccountNumber
			Dim table As IXlTable
			Using stream As New FileStream(eQuestFilename, FileMode.Create)

				Dim documentFormat As XlDocumentFormat = XlDocumentFormat.Xlsx
				Dim exporter As IXlExporter = XlExport.CreateExporter(documentFormat)

				' Create a new document.
				Using document As IXlDocument = exporter.CreateDocument(stream)

					' Specify the document culture.
					document.Options.Culture = CultureInfo.CurrentCulture

					' Create a new worksheet under the specified name. 
					Using sheet As IXlSheet = document.CreateSheet()
						sheet.Name = String.Format("Importdaten", CInt(Math.Ceiling(Rnd() * Integer.MaxValue)) + 1)

						Dim columnNames() As String = {"Abrechnungsstelle", "AHVN13", "Geschlecht",
							"Vorname", "Nachname", "Geburtsdatum", "Nationalität", "Aufenthaltskategorie",
							"Strasse", "Hausnummer", "Postfach", "PLZ", "Ort", "Wohnsitzstaat", "Gemeinde Nr.", "Arbeitsort", "Arbeitort PLZ", "Arbeitort Gemeinde Nr.",
							"Beschäftigungsart", "Eintrittsdatum", "Mutationsdatum", "Austrittsdatum", "Austrittsgrund", "Bruttolohn", "Arbeitsstunden pro Woche", "Arbeitspensum",
							"Ferienanspruch", "Zivilstand", "Anzahl Kinder", "Kirchensteuerpflicht",
							"Partner erwerbstätig", "Partner Vorname", "Partner Nachname", "Partner Strasse", "Partner Hausnummer", "Partner PLZ", "Partner Stadt", "Partner Wohnsitzstaat"}


						' set header
						Using row As IXlRow = sheet.CreateRow()
							table = row.BeginTable(columnNames, True)
						End Using


						For Each itm In LoadedQSTData
							success = success AndAlso LoadEmployeeData(itm.MANr)
							If Not success Then Continue For

							Using row As IXlRow = sheet.CreateRow()
								Dim employeeStreetName As String = String.Empty
								Dim employeeHouseNumber As String = String.Empty

								If Not String.IsNullOrWhiteSpace(m_EmployeeMasterData.Street) Then
									Dim adressString = m_EmployeeMasterData.Street
									Dim i As Integer = 0
									Dim streetData = adressString.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)

									employeeHouseNumber = streetData(streetData.Length - 1)
									If Val(employeeHouseNumber) = 0 Then employeeHouseNumber = streetData(0)
									For Each strItm In streetData
										If i < streetData.Length - 1 Then
											employeeStreetName = String.Format("{0}{1}{2}", employeeStreetName, If(String.IsNullOrWhiteSpace(employeeStreetName), "", " "), strItm)
										End If
										i += 1
									Next

								End If

								Dim partnerStreetName As String = String.Empty
								Dim partnerHouseNumber As String = String.Empty

								If Not String.IsNullOrWhiteSpace(itm.PartnerStreet) Then
									Dim adressString = itm.PartnerStreet
									Dim i As Integer = 0
									Dim streetData = adressString.Split(New String() {" "}, StringSplitOptions.RemoveEmptyEntries)

									partnerHouseNumber = streetData(streetData.Length - 1)
									For Each strItm In streetData
										If i < streetData.Length - 1 Then
											partnerStreetName = String.Format("{0}{1}{2}", partnerStreetName, If(String.IsNullOrWhiteSpace(partnerStreetName), "", " "), strItm)
										End If
										i += 1
									Next

								End If

								row.BulkCells(New Object() {Abrechnungsstelle, itm.SocialInsuranceNumber, If(itm.geschlecht = "M", "M", "F"),
							m_EmployeeMasterData.Firstname, m_EmployeeMasterData.Lastname, itm.gebdat, m_EmployeeMasterData.Nationality, itm.TypeofStayViewData,
							employeeStreetName, employeeHouseNumber, m_EmployeeMasterData.PostOfficeBox, itm.employeepostcode, itm.employeecity, itm.employeecountry,
							If(itm.TaxCommunityCode.HasValue AndAlso itm.TaxCommunityCode.GetValueOrDefault(0) > 0, itm.TaxCommunityCode, String.Empty), itm.EmploymentLocation, itm.EmploymentPostcode, String.Empty,
							itm.EmploymentTypeViewData, If(itm.esab Is Nothing, String.Empty, Format("{0:dd.MM.yyyy", Convert.ToDateTime(itm.esab))),
							String.Empty,
							If(itm.esende Is Nothing, String.Empty, Format("{0:dd.MM.yyyy", Convert.ToDateTime(itm.esende))),
							itm.Dismissalreason,
							itm.Bruttolohn,
							itm.WorkingHoursWeek,
							itm.WorkingPensum,
							itm.VacationDaysCount,
							itm.TaxCivilStatusCode,
							itm.NumberOfChildren,
							itm.TaxChurchCodeViewData,
							If(itm.InEmployment, "yes", "no"), itm.PartnerFirstname, itm.PartnerLastName, partnerStreetName, partnerHouseNumber, itm.PartnerPostcode, itm.PartnerCity, itm.PartnerCountry}, Nothing)

							End Using

						Next


					End Using
				End Using

			End Using


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			SplashScreenManager.CloseForm(False)

			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return False

		Finally

		End Try


		Return success
	End Function

	Private Function CreateNewAbrechnungForeQuest() As Boolean
		Dim success As Boolean = True

		Try
			Dim eQuestPath As String = txt_eQuestFilename.EditValue
			Dim eQuestFilename As String

			eQuestFilename = Path.Combine(eQuestPath, "eQuestListe_Abrechnung_Import.xlsx")

			' Ensure that the data-aware export mode is enabled.
			ExportSettings.DefaultExportType = ExportType.DataAware

			Dim Abrechnungsstelle As String = m_AccountNumber
			Dim table As IXlTable
			Using stream As New FileStream(eQuestFilename, FileMode.Create)

				Dim documentFormat As XlDocumentFormat = XlDocumentFormat.Xlsx
				Dim exporter As IXlExporter = XlExport.CreateExporter(documentFormat)

				' Create a new document.
				Using document As IXlDocument = exporter.CreateDocument(stream)

					' Specify the document culture.
					document.Options.Culture = CultureInfo.CurrentCulture

					' Create a new worksheet under the specified name. 
					Using sheet As IXlSheet = document.CreateSheet()
						sheet.Name = String.Format("Abrechnung_Import", CInt(Math.Ceiling(Rnd() * Integer.MaxValue)) + 1)

						Dim columnNames() As String = {"Abrechnungsstelle",
							"AHVN13",
							"Jahr",
							"Monat",
							"Bruttolohn",
							"Tage",
							"Satzbestimmend"}


						' set header
						Using row As IXlRow = sheet.CreateRow()
							table = row.BeginTable(columnNames, True)
						End Using


						For Each itm In LoadedQSTData
							success = success AndAlso LoadEmployeeData(itm.MANr)
							If Not success Then Continue For

							Using row As IXlRow = sheet.CreateRow()

								row.BulkCells(New Object() {Abrechnungsstelle,
											  itm.SocialInsuranceNumber,
											  itm.jahr,
											  itm.monat,
											  itm.Bruttolohn,
											  itm.workeddays,
											  itm.qstbasis}, Nothing)

							End Using

						Next


					End Using
				End Using

			End Using


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			SplashScreenManager.CloseForm(False)

			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return False

		Finally

		End Try


		Return success
	End Function

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then
			Dim obj As New ThreadTesting.OpenFormsWithThreading()

			Dim column = e.Column
			Dim dataRow = sender.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, EQuerstViewData)

				Select Case column.Name.ToLower

					Case Else
						If CheckIfRunning("SPS.MainView") Then
							If viewData.EmployeeNumber.HasValue Then obj.OpenSelectedEmployee(viewData.EmployeeNumber)
						End If

				End Select

			End If

		End If

	End Sub

	Private Class EQuerstViewData

		Public Property MDNr As Integer?
		Public Property EmployeeNumber As Integer?

		Public Property Abrechnungsstelle As String
		Public Property SocialInsuranceNumber As String
		Public Property Geschlecht As String
		Public Property Employeename As String
		Public Property BirthDay As DateTime?
		Public Property Nationality As String
		Public Property EmployeeStreetName As String
		Public Property EmployeeHouseNumber As String
		Public Property PostOfficeBox As String
		Public Property Employeepostcode As String
		Public Property Employeecity As String
		Public Property Employeecountry As String
		Public Property TaxCommunityCode As Integer?
		Public Property EmploymentLocation As String
		Public Property EmploymentPostcode As String
		Public Property GemeindeNummer As Integer?
		Public Property EmploymentTypeViewData As String
		Public Property ESAb As DateTime?
		Public Property Mutation As DateTime?
		Public Property ESEnde As DateTime?
		Public Property Dismissalreason As String
		Public Property Bruttolohn As Decimal?
		Public Property WorkingHoursWeek As Decimal?
		Public Property WorkingPensum As String
		Public Property VacationDaysCount As Decimal?
		Public Property TaxCivilStatusCode As String
		Public Property NumberOfChildren As Integer?
		Public Property TaxChurchCodeViewData As String
		Public Property InEmployment As Boolean?
		Public Property PartnerFirstname As String
		Public Property PartnerLastName As String
		Public Property PartnerStreetName As String
		Public Property PartnerHouseNumber As String
		Public Property PartnerPostcode As String
		Public Property PartnerCity As String
		Public Property PartnerCountry As String
		Public Property ErrorReason As String

		Public ReadOnly Property TypeofStayViewData As String
			Get
				Return (If(Employeecountry <> "CH", "WEEKLY", "CH"))
			End Get
		End Property

		Public ReadOnly Property EmployeeAddress As String
			Get
				Return (String.Format("{0} {1}, {2}-{3} {4}", EmployeeStreetName, EmployeeHouseNumber, Employeecountry, Employeepostcode, Employeecity))
			End Get
		End Property

	End Class


End Class

Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings

Namespace UI

  Public Class ucPageCreateEmployee

#Region "Public Methods"

    ''' <summary>
    ''' Inits the control with configuration information.
    ''' </summary>
    '''<param name="initializationClass">The initialization class.</param>
    '''<param name="translationHelper">The translation helper.</param>
    Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
      MyBase.InitWithConfigurationData(initializationClass, translationHelper)
    End Sub

    ''' <summary>
    ''' Activates the page.
    ''' </summary>
    ''' <returns>Boolean value indicating success.</returns>
    Public Overrides Function ActivatePage() As Boolean

      Const MISSING_ENTRY_INDICATOR As String = "-"

      Dim success As Boolean = True

      Dim mandantAndAvisorData = m_UCMediator.SelectedMandantAndAdvisorData
      Dim employeeBasicData = m_UCMediator.SelectedBasiscData
      Dim additionaldata1 = m_UCMediator.SelectedAdditionalData1
      Dim additionaldata2 = m_UCMediator.selectedAdditionalData2

      ' ---Mandant and advisor data---
      lblMandantValue.Text = mandantAndAvisorData.MandantData.MandantName1
      lblBeraterValue.Text = If(mandantAndAvisorData.AdvisorData Is Nothing, String.Empty, mandantAndAvisorData.AdvisorData.UserFullname)

      ' ---Kandidat---
      lblNameValue.Text = String.Format("{0} {1}", employeeBasicData.Firstname, employeeBasicData.Lastname)
      lblAdresseValue.Text = String.Format("{0}, {1}-{2} {3}", employeeBasicData.Street, employeeBasicData.CountryCode, employeeBasicData.PostCode, employeeBasicData.Location)
      lblGeschlechtValue.Text = employeeBasicData.Gender
			lblNationalitaetValue.Text = employeeBasicData.Nationality
			lblZivilstandValue.Text = employeeBasicData.CivilState
      lblGeburtsdatumValue.Text = String.Format("{0:dd.MM.yyyy}", employeeBasicData.Birthdate)
      lblSpracheValue.Text = employeeBasicData.Language

      ' ---Additional data 1---
      lblStatus1Value.Text = ReplaceMissing(additionaldata1.KStat1, MISSING_ENTRY_INDICATOR)
      lblStatus2Value.Text = ReplaceMissing(additionaldata1.KStat2, MISSING_ENTRY_INDICATOR)
      lblKontaktValue.Text = ReplaceMissing(additionaldata1.KontaktHow, MISSING_ENTRY_INDICATOR)
      lblQualifikationValue.Text = If(Not String.IsNullOrEmpty(additionaldata1.Profession),
                                      String.Format("{0}-{1}", additionaldata1.ProfessionCode, additionaldata1.Profession),
                                      MISSING_ENTRY_INDICATOR)
      lblHerkunftslandQualifikationValue.Text = ReplaceMissing(additionaldata1.QLand, MISSING_ENTRY_INDICATOR)
      chkDStellen.Checked = additionaldata1.DStellen
      chkESLock.Checked = additionaldata1.NoES

      ' ---Additional data 2---
      lblBewilligungValue.Text = additionaldata2.Permission
      lblBewilligungBisValue.Text = If(additionaldata2.PermissionToDate.HasValue, String.Format("{0:dd.MM.yyyy}", additionaldata2.PermissionToDate), MISSING_ENTRY_INDICATOR)
      lblHeimatortValue.Text = ReplaceMissing(additionaldata2.BirthPlace, MISSING_ENTRY_INDICATOR)
      lblSCantonValue.Text = ReplaceMissing(additionaldata2.S_Canton, MISSING_ENTRY_INDICATOR)
      chkResidence.Checked = additionaldata2.Residence
      lblAnsQSTToValue.Text = If(additionaldata2.ANS_QST_Bis.HasValue, String.Format("{0:dd.MM.yyyy}", additionaldata2.ANS_QST_Bis), MISSING_ENTRY_INDICATOR)
      lblQSTCodeValue.Text = ReplaceMissing(additionaldata2.Q_Steuer, MISSING_ENTRY_INDICATOR)
      lblChurchTaxValue.Text = ReplaceMissing(additionaldata2.ChurchTax, MISSING_ENTRY_INDICATOR)
      lblChildCountValue.Text = If(additionaldata2.ChildsCount.HasValue, additionaldata2.ChildsCount, 0)
      lblQSTCommunityValue.Text = ReplaceMissing(additionaldata2.QSTCommunity, MISSING_ENTRY_INDICATOR)

      m_IsFirstPageActivation = False

      Return success
    End Function

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_IsFirstPageActivation = True

      lblMandantValue.Text = String.Empty
      lblBeraterValue.Text = String.Empty

      lblNameValue.Text = String.Empty
      lblAdresseValue.Text = String.Empty
      lblGeschlechtValue.Text = String.Empty
      lblNationalitaetValue.Text = String.Empty
      lblZivilstandValue.Text = String.Empty
      lblGeburtsdatumValue.Text = String.Empty
      lblSpracheValue.Text = String.Empty

      lblStatus1Value.Text = String.Empty
      lblStatus2Value.Text = String.Empty
      lblKontaktValue.Text = String.Empty
      lblQualifikationValue.Text = String.Empty
      lblHerkunftslandQualifikationValue.Text = String.Empty

      chkDStellen.Checked = False
      chkDStellen.Properties.ReadOnly = True

      chkESLock.Checked = False
      chkESLock.Properties.ReadOnly = True

      lblSCantonValue.Text = String.Empty
      chkResidence.Checked = False
      chkResidence.Properties.ReadOnly = True
      lblAnsQSTToValue.Text = String.Empty
      lblQSTCodeValue.Text = String.Empty
      lblChurchTaxValue.Text = String.Empty
      lblChildCountValue.Text = String.Empty
      lblQSTCommunityValue.Text = String.Empty

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.lblMandantUndBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblMandantUndBerater.Text)
      Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
      Me.lblBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblBerater.Text)

			Me.lblEmployee.Text = m_Translate.GetSafeTranslationValue(Me.lblEmployee.Text)
			Me.lblName.Text = m_Translate.GetSafeTranslationValue(Me.lblName.Text)
      Me.lblAdresse.Text = m_Translate.GetSafeTranslationValue(Me.lblAdresse.Text)
      Me.lblGeschlecht.Text = m_Translate.GetSafeTranslationValue(Me.lblGeschlecht.Text)
      Me.lblNationalitaet.Text = m_Translate.GetSafeTranslationValue(Me.lblNationalitaet.Text)
      Me.lblZivilstand.Text = m_Translate.GetSafeTranslationValue(Me.lblZivilstand.Text)
      Me.lblGeburtsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblGeburtsdatum.Text)
      Me.lblSprache.Text = m_Translate.GetSafeTranslationValue(Me.lblSprache.Text)

			Me.lblBewilligungHeader.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligungHeader.Text)
			Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)
			Me.lblBewilligungBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligungBis.Text)

      Me.lblEigenschaftenUndMerkmale.Text = m_Translate.GetSafeTranslationValue(Me.lblEigenschaftenUndMerkmale.Text)
			Me.lblState1.Text = m_Translate.GetSafeTranslationValue(Me.lblState1.Text, True)
			Me.lblState2.Text = m_Translate.GetSafeTranslationValue(Me.lblState2.Text, True)
			Me.lblKontakt.Text = m_Translate.GetSafeTranslationValue(Me.lblKontakt.Text, True)
      Me.lblQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblQualifikation.Text)
      Me.lblHerkunftslandQualifikation.Text = m_Translate.GetSafeTranslationValue(Me.lblHerkunftslandQualifikation.Text)
      Me.chkDStellen.Text = m_Translate.GetSafeTranslationValue(Me.chkDStellen.Text)
      Me.chkESLock.Text = m_Translate.GetSafeTranslationValue(Me.chkESLock.Text)

      Me.lblQuellensteuer.Text = m_Translate.GetSafeTranslationValue(Me.lblQuellensteuer.Text)
      Me.lblSCanton.Text = m_Translate.GetSafeTranslationValue(Me.lblSCanton.Text)
      Me.chkResidence.Text = m_Translate.GetSafeTranslationValue(Me.chkResidence.Text)
      Me.lblAnsQSTBis.Text = m_Translate.GetSafeTranslationValue(Me.lblAnsQSTBis.Text)
      Me.lblQSTCode.Text = m_Translate.GetSafeTranslationValue(Me.lblQSTCode.Text)
      Me.lblChurchTax.Text = m_Translate.GetSafeTranslationValue(Me.lblChurchTax.Text)
      Me.lblChildCount.Text = m_Translate.GetSafeTranslationValue(Me.lblChildCount.Text)
      Me.lblHeimatort.Text = m_Translate.GetSafeTranslationValue(Me.lblHeimatort.Text)
			Me.lblQSTCommunity.Text = m_Translate.GetSafeTranslationValue(Me.lblQSTCommunity.Text)

    End Sub

    ''' <summary>
    ''' Replace missing string with other string.
    ''' </summary>
    ''' <param name="str">The string to check.</param>
    ''' <param name="replacement">The replacement string.</param>
    ''' <returns>The original string or the replacement string in case of empty original string.</returns>
    Private Function ReplaceMissing(ByVal str As String, ByVal replacement As String)

      If String.IsNullOrEmpty(str) Then
        Return replacement
      End If

      Return str

    End Function

#End Region

  End Class

End Namespace

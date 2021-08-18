Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SPProgUtility.CommonSettings
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.MA.EinsatzMng.UI.ucPageSelectSalaryData
Namespace UI

  Public Class ucPageCreateES

#Region "Private Fields"

    ''' <summary>
    ''' The ESNr of the newly created ES.
    ''' </summary>
    Private m_ESNrOfNewlyCreatedES As Integer?

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.lblEmployeeAndCustomer.Text = m_Translate.GetSafeTranslationValue(Me.lblEmployeeAndCustomer.Text)
      Me.lblMandant.Text = m_Translate.GetSafeTranslationValue(Me.lblMandant.Text)
      Me.lblMitarbeiter.Text = m_Translate.GetSafeTranslationValue(Me.lblMitarbeiter.Text)
      Me.lblFirma.Text = m_Translate.GetSafeTranslationValue(Me.lblFirma.Text)
      Me.lblZHD.Text = m_Translate.GetSafeTranslationValue(Me.lblZHD.Text)

      Me.lblEinsatz.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatz.Text)
      Me.lblESAls.Text = m_Translate.GetSafeTranslationValue(Me.lblESAls.Text)
      Me.lblEinsatzBeginnt.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatzBeginnt.Text)
      Me.lblEndetAm.Text = m_Translate.GetSafeTranslationValue(Me.lblEndetAm.Text)

			Me.lblGueltigab.Text = m_Translate.GetSafeTranslationValue(Me.lblGueltigab.Text)
      Me.chkFarPflichtig.Text = m_Translate.GetSafeTranslationValue(Me.chkFarPflichtig.Text)
      Me.lblGAVVertrag.Text = m_Translate.GetSafeTranslationValue(Me.lblGAVVertrag.Text)
      Me.lblGrundlohn.Text = m_Translate.GetSafeTranslationValue(Me.lblGrundlohn.Text)
      Me.lblFeiertag.Text = m_Translate.GetSafeTranslationValue(Me.lblFeiertag.Text)
      Me.lblFerien.Text = m_Translate.GetSafeTranslationValue(Me.lblFerien.Text)
      Me.lblLohn13.Text = m_Translate.GetSafeTranslationValue(Me.lblLohn13.Text)
      Me.lblStundenlohn.Text = m_Translate.GetSafeTranslationValue(Me.lblStundenlohn.Text)
      Me.lblLohnspesen.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnspesen.Text)
      Me.lblTagespesen.Text = m_Translate.GetSafeTranslationValue(Me.lblTagespesen.Text)

      Me.lblTarif.Text = m_Translate.GetSafeTranslationValue(Me.lblTarif.Text)
      Me.lblTagesspesenKunde.Text = m_Translate.GetSafeTranslationValue(Me.lblTagesspesenKunde.Text)
      Me.lblMwStPflichtig.Text = m_Translate.GetSafeTranslationValue(Me.lblMwStPflichtig.Text)
      Me.lblMwStBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblMwStBetrag.Text)

      Me.lblMargeOhneBVG.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeOhneBVG.Text)
      Me.lblMargeMitBVG.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeMitBVG.Text)
      Me.lblMargeOhneBVGProz.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeOhneBVGProz.Text)
      Me.lblMargeMitBVGProz.Text = m_Translate.GetSafeTranslationValue(Me.lblMargeMitBVGProz.Text)

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the ESNr of the newly created ES.
    ''' </summary>
    Public ReadOnly Property ESNrOfNewlyCreatedES As Integer?
      Get
        Return m_ESNrOfNewlyCreatedES
      End Get

    End Property

#End Region

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

      Dim success As Boolean = True

      Dim candidateAndCustomerData = m_UCMediator.SelectedCandidateAndCustomerData
      Dim esData = m_UCMediator.SelectedESData
      Dim salaryData = m_UCMediator.SelectedSalaryData

      ' ---Mandant, candidate and customer data---
      lblMandantValue.Text = String.Format("{0}", candidateAndCustomerData.MandantData.MandantName1)
      lblEmployeeValue.Text = String.Format("{0} {1}", candidateAndCustomerData.EmployeeData.Lastname, candidateAndCustomerData.EmployeeData.Firstname)
      lblCustomerValue.Text = String.Format("{0}", candidateAndCustomerData.CustomerData.Company1)

      If Not candidateAndCustomerData.ResponsiblePersondata Is Nothing Then
        lblZHDValue.Text = String.Format("{0} {1} {2}", candidateAndCustomerData.ResponsiblePersondata.TranslatedSalutation,
                                                      candidateAndCustomerData.ResponsiblePersondata.Lastname,
                                                       candidateAndCustomerData.ResponsiblePersondata.Firstname)
      Else
        lblZHDValue.Text = "-"
      End If

      ' ---ES data---
      lblESAlsValue.Text = esData.ESAls

      lblBeginAtValue.Text = String.Format("{0:dd.MM.yyyy}", esData.ESStartDate)

      If esData.ESEndDate.HasValue Then
        lblEndsAtValue.Text = String.Format("{0:dd.MM.yyyy}", esData.ESEndDate)
      Else
        lblEndsAtValue.Text = "-"
      End If

      ' ---Salary data---

      lblLohndatenVon.Text = String.Format("{0}: {1:dd.MM.yyyy}", m_Translate.GetSafeTranslationValue("Von"), salaryData.LOVon)

      If salaryData.IsGAVSelected Then
        lblGAVVertragValue.Text = String.Format("{0}) {1}", salaryData.SelectedGAVStringData.GAVNr, salaryData.SelectedGAVStringData.Gruppe0)
      Else
        lblGAVVertragValue.Text = "1 (-)"
      End If

      chkFarPflichtig.Checked = salaryData.FarPflichtig

      txtGrundlohn.EditValue = salaryData.GrundLohn

      txtBasisFeiertag.EditValue = salaryData.FeiertagBasis
      txtAnsatzFeiertag.EditValue = salaryData.FeiertagAnsatz
      txtBetragFeiertag.EditValue = salaryData.FeiertagBetrag

      txtBasisFerien.EditValue = salaryData.FerienBasis
      txtAnsatzFerien.EditValue = salaryData.FerienAnsatz
      txtBetragFerien.EditValue = salaryData.FerienBetrag

      txtBasisLohn13.EditValue = salaryData.Lohn13Basis
      txtAnsatzLohn13.EditValue = salaryData.Lohn13Ansatz
      txtBetragLohn13.EditValue = salaryData.Lohn13Betrag

      txtStundenLohn.EditValue = salaryData.StundenLohn
      txtLohnspesen.EditValue = salaryData.Lohnspesen
      txtTagesspesenMA.EditValue = salaryData.TagespesenMA

      txtTarif.EditValue = salaryData.Tarif
      txtTagespesenKD.EditValue = salaryData.TagesspesenKD
      chkMwStPflichtig.Checked = salaryData.MwStPflicht
      txtMwstBetrag.EditValue = salaryData.MwStBetrag

      ' --- Marge ---
      lblMargeOhneBVGValue.Text = String.Format("{0:0.00}", salaryData.MargeData.MargeOhneBVG)
      lblMargeMitBVGValue.Text = String.Format("{0:0.00}", salaryData.MargeData.MargeMitBVG)
      lblMargeOhneBVGProzValue.Text = String.Format("{0:0.00}", salaryData.MargeData.MargeOhneBVGInProz)
      lblMargeMitBVGProzValue.Text = String.Format("{0:0.00}", salaryData.MargeData.MargeWithBVGInProz)

      m_IsFirstPageActivation = False

      Return success
    End Function

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_IsFirstPageActivation = True

      lblMandantValue.Text = String.Empty
      lblEmployeeValue.Text = String.Empty
      lblCustomerValue.Text = String.Empty
      lblZHDValue.Text = String.Empty

      lblESAlsValue.Text = String.Empty
      lblBeginAtValue.Text = String.Empty
      lblEndsAtValue.Text = String.Empty

      lblLohndatenVon.Text = String.Empty

      chkFarPflichtig.Checked = False
      lblGAVVertragValue.Text = "1 (-)"

      ' Grundlohn
      txtGrundlohn.EditValue = 0D
      txtGrundlohn.Properties.ReadOnly = False

      ' Feiertag
      txtBasisFeiertag.EditValue = 0D
      txtAnsatzFeiertag.EditValue = 0D
      txtBetragFeiertag.EditValue = 0D

      ' Ferien
      txtBasisFerien.EditValue = 0D
      txtAnsatzFerien.EditValue = 0D
      txtBetragFerien.EditValue = 0D

      ' 13. Lohn
      txtBasisLohn13.EditValue = 0D
      txtAnsatzLohn13.EditValue = 0D
      txtBetragLohn13.EditValue = 0D

      txtStundenLohn.EditValue = 0D
      txtLohnspesen.EditValue = 0D
      txtTagesspesenMA.EditValue = 0D

      txtTarif.EditValue = 0D
      txtTagespesenKD.EditValue = 0D
      chkMwStPflichtig.Checked = False
      txtMwstBetrag.EditValue = 0D

      lblMargeOhneBVGValue.Text = "0.0"
      lblMargeMitBVGValue.Text = "0.0"
      lblMargeOhneBVGProzValue.Text = "0.0"
      lblMargeMitBVGProzValue.Text = "0.0"

    End Sub

		''' <summary>
		''' Creates an ES.
		''' </summary>
		Public Function CreateES() As Integer?

			Dim candidateAndCustomerData = m_UCMediator.SelectedCandidateAndCustomerData
			Dim esData = m_UCMediator.SelectedESData
			Dim salaryData = m_UCMediator.SelectedSalaryData
			Dim employeeNotice = candidateAndCustomerData.EmployeeNoticeEmployement
			Dim customerNotice = candidateAndCustomerData.CustomerNoticeEmployement

			Dim esDataSupport As New ESCreateService(candidateAndCustomerData.MandantData.MandantNumber, m_InitializationData)

			Dim newESNr As Integer? = esDataSupport.CreateES(candidateAndCustomerData, esData, salaryData)

			m_ESNrOfNewlyCreatedES = newESNr
			If newESNr.GetValueOrDefault(0) > 0 Then
				If Not employeeNotice.StartsWith(vbNewLine) Then
					Dim employeeNoticeData = m_UCMediator.EmployeeDbAccess.LoadEmployeeNoticesData(candidateAndCustomerData.EmployeeData.EmployeeNumber)

					If Not employeeNoticeData Is Nothing Then
						employeeNoticeData.Notice_Employment = employeeNotice
						m_UCMediator.EmployeeDbAccess.UpdateEmployeeNoticesData(employeeNoticeData)
					End If

				End If
				If Not customerNotice.StartsWith(vbNewLine) Then
					Dim customerNoticeData = m_UCMediator.CustomerDbAccess.LoadCustomerNoticesData(candidateAndCustomerData.CustomerData.CustomerNumber)
					If Not customerNoticeData Is Nothing Then
						customerNoticeData.Notice_Employment = customerNotice
						m_UCMediator.CustomerDbAccess.UpdateCustomerNoticesData(customerNoticeData)
					End If

				End If

			End If


				Return newESNr

		End Function

#End Region

	End Class

End Namespace
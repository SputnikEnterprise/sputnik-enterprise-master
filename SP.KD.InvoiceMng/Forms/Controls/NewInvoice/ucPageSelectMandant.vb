Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Invoice
Imports DevExpress.XtraEditors
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Namespace UI

  Public Class ucPageSelectMandant

#Region "Private Constants"

    Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Private Fields"

    ''' <summary>
    ''' The mandant data.
    ''' </summary>
    Private m_SelectedMandantData As SP.DatabaseAccess.Common.DataObjects.MandantData

		Private m_CostCenters As SP.DatabaseAccess.Common.DataObjects.CostCenters

		Private m_Advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_ProgPath As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

#End Region

#Region "Constructor"

		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			Try
				m_ProgPath = New ClsProgPath
				m_Mandant = New Mandant
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			AddHandler lueMandant.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueKst1.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueKst2.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueAdvisor1.ButtonClick, AddressOf OnDropDownButtonClick
			AddHandler lueAdvisor2.ButtonClick, AddressOf OnDropDownButtonClick
		End Sub

#End Region


#Region "Public Properties"

		''' <summary>
		''' Gets the selected candidate and customer data.
		''' </summary>
		''' <returns>Candidate and customer data.</returns>
		Public ReadOnly Property SelecteData As InitDataPage1
			Get

				Dim data As New InitDataPage1 With {
					.MandantData = m_SelectedMandantData,
					.CostCenter1 = (From kst In m_CostCenters.CostCenter1 Where kst.KSTName = lueKst1.EditValue).FirstOrDefault,
					.CostCenter2 = (From kst In m_CostCenters.CostCenter2 Where kst.KSTName = lueKst2.EditValue).FirstOrDefault,
					.Advisor1 = (From a In m_Advisors Where a.KST = lueAdvisor1.EditValue).FirstOrDefault,
					.Advisor2 = (From a In m_Advisors Where a.KST = lueAdvisor2.EditValue).FirstOrDefault,
					.CombinedAdvisorString = lblSelectedAdvisors.Text
				}

				Return data
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

			If m_IsFirstPageActivation Then
				success = success AndAlso LoadMandantDropDownData()

				PreselectData()

			End If

			m_IsFirstPageActivation = False

			Return success
		End Function

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			m_IsFirstPageActivation = True

			m_SelectedMandantData = Nothing
			m_CostCenters = Nothing
			m_Advisors = Nothing

			lblSelectedAdvisors.Text = String.Empty

			'  Reset drop downs and lists

			ResetMandantDropDown()
			ResetKstDropDown()
			ResetAdvisorDropDown()

			ErrorProvider.Clear()

		End Sub


		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean


			Dim valid As Boolean = True
			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorDueDate As String = m_Translate.GetSafeTranslationValue("Fällig-Datum muss nach dem Rechnungs-Datum liegen.")
			Try
				'mandatory fields
				valid = valid And SetErrorIfInvalid(lueMandant, ErrorProvider, lueMandant.EditValue Is Nothing, errorText)
				valid = valid And SetErrorIfInvalid(lueAdvisor2, ErrorProvider, String.IsNullOrWhiteSpace(lblSelectedAdvisors.Text), errorText)
			Catch ex As Exception
				valid = False
			End Try

			Return valid

		End Function

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			'Captions
			gpEigenschaften.Text = m_Translate.GetSafeTranslationValue(gpEigenschaften.Text)

			'Labels
			lblMandant.Text = m_Translate.GetSafeTranslationValue(lblMandant.Text)

			lblAdvisor.Text = m_Translate.GetSafeTranslationValue(lblAdvisor.Text, True)
			lblKst1.Text = m_Translate.GetSafeTranslationValue(lblKst1.Text, True)
			lblKST2.Text = m_Translate.GetSafeTranslationValue(lblKST2.Text, True)

			Me.lblBeraterMA.Text = m_Translate.GetSafeTranslationValue(Me.lblBeraterMA.Text)
			Me.lblBeraterKD.Text = m_Translate.GetSafeTranslationValue(Me.lblBeraterKD.Text)

		End Sub

		''' <summary>
		''' Resets the Mandant drop down.
		''' </summary>
		Private Sub ResetMandantDropDown()

			lueMandant.Properties.DisplayMember = "MandantName1"
			lueMandant.Properties.ValueMember = "MandantNumber"

			lueMandant.Properties.Columns.Clear()
			lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																						 .Width = 100,
																						 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})
			lueMandant.EditValue = Nothing
		End Sub


		''' <summary>
		''' Resets the Kst1 and Kst2 drop down.
		''' </summary>
		Private Sub ResetKstDropDown()

			'Kst1
			lueKst1.Properties.DisplayMember = "KSTBezeichnung"
			lueKst1.Properties.ValueMember = "KSTName"

			lueKst1.Properties.Columns.Clear()
			lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
			lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

			lueKst1.EditValue = Nothing

			'Kst2
			lueKst2.Properties.DisplayMember = "KSTBezeichnung"
			lueKst2.Properties.ValueMember = "KSTName"

			lueKst2.Properties.Columns.Clear()
			lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
			lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

			lueKst2.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the Advisor1 and Advisor2 drop down.
		''' </summary>
		Private Sub ResetAdvisorDropDown()

			'Advisor1
			lueAdvisor1.Properties.DisplayMember = "UserFullname"		' "KST"
			lueAdvisor1.Properties.ValueMember = "KST"

			lueAdvisor1.Properties.Columns.Clear()
			lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("KST", 0))
			lueAdvisor1.Properties.Columns.Add(New LookUpColumnInfo("UserFullname", 0))

			lueAdvisor1.EditValue = Nothing

			'Advisor2
			lueAdvisor2.Properties.DisplayMember = "UserFullname"
			lueAdvisor2.Properties.ValueMember = "KST"

			lueAdvisor2.Properties.Columns.Clear()
			lueAdvisor2.Properties.Columns.Add(New LookUpColumnInfo("KST", 0))
			lueAdvisor2.Properties.Columns.Add(New LookUpColumnInfo("UserFullname", 0))

			lueAdvisor2.EditValue = Nothing

		End Sub

		''' <summary>
		''' Loads the mandant drop down data.
		''' </summary>
		Private Function LoadMandantDropDownData() As Boolean
			Dim mandantData = m_UCMediator.CommonDbAccess.LoadCompaniesListData()

			If (mandantData Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandantendaten konnten nicht geladen werden."))
			End If

			lueMandant.Properties.DataSource = mandantData
			lueMandant.Properties.ForceInitialize()

			Return mandantData IsNot Nothing
		End Function

		''' <summary>
		''' Loads the Kst1 drop down data.
		''' </summary>
		Private Sub LoadKst1DropDown()
			' Load data
			m_CostCenters = m_UCMediator.CommonDbAccess.LoadCostCenters()

			' Kst1
			lueKst1.EditValue = Nothing
			lueKst1.Properties.DataSource = m_CostCenters.CostCenter1
			lueKst1.Properties.ForceInitialize()

			' Kst2
			lueKst2.EditValue = Nothing
			lueKst2.Properties.DataSource = Nothing
			lueKst2.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the Kst2 drop down data.
		''' </summary>
		Private Sub LoadKst2DropDown()

			If (m_CostCenters Is Nothing) Then
				Return
			End If

			Dim kst1Name = lueKst1.EditValue
			Dim kst2Data = m_CostCenters.GetCostCenter2ForCostCenter1(kst1Name)

			' Kst2
			lueKst2.EditValue = Nothing
			lueKst2.Properties.DataSource = kst2Data
			lueKst2.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the Advisor1 and Advisor2 drop down data.
		''' </summary>
		Private Sub LoadAdvisorDropDown()
			' Load data
			m_Advisors = m_UCMediator.CommonDbAccess.LoadActivatedAdvisorData()

			' Advisor1
			lueAdvisor1.EditValue = Nothing
			lueAdvisor1.Properties.DataSource = m_Advisors
			lueAdvisor1.Properties.ForceInitialize()

			' Advisor2
			lueAdvisor2.EditValue = Nothing
			lueAdvisor2.Properties.DataSource = m_Advisors
			lueAdvisor2.Properties.ForceInitialize()
		End Sub

		''' <summary>
		''' Loads the Kst1 and Kst2 values for given User.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdvisorKst12Data() As Boolean

			Dim m_selectedAdvisor = lueAdvisor1.EditValue
			If m_selectedAdvisor Is Nothing Then
				lueKst1.EditValue = Nothing
				lueKst2.EditValue = Nothing
				lueKst1.Properties.DataSource = Nothing
				lueKst2.Properties.DataSource = Nothing

				Return False
			End If

      Dim givenAdvisors = m_UCMediator.CommonDbAccess.LoadAdvisorDataforGivenAdvisor(m_selectedAdvisor)

      If givenAdvisors Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die gesuchten Beraterdaten konnten nicht geladen werden."))
      End If

      LoadKst1DropDown()
      LoadKst2DropDown()

      lueKst1.EditValue = givenAdvisors.KST1
      lueKst2.EditValue = givenAdvisors.KST2

      Return Not givenAdvisors Is Nothing
		End Function

		''' <summary>
		''' Preselects data.
		''' </summary>
		Private Sub PreselectData()

			Dim hasPreselectionData As Boolean = Not (PreselectionData Is Nothing)

			If hasPreselectionData Then

				Dim supressUIEventState = m_SuppressUIEvents
				m_SuppressUIEvents = False ' Make sure UI event are fired so that the lookup data is loaded correctly.

				' ---Mandant---
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

					If manantDataList.Any(Function(md) md.MandantNumber = PreselectionData.MDNr) Then

						' Mandant is required
						lueMandant.EditValue = PreselectionData.MDNr

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						m_SuppressUIEvents = supressUIEventState
						Return
					End If

				End If

				m_SuppressUIEvents = supressUIEventState
			Else
				' No preslection data -> use mandant form initialization object.

				' ---Mandant---
				If Not lueMandant.Properties.DataSource Is Nothing Then

					Dim manantDataList = CType(lueMandant.Properties.DataSource, List(Of MandantData))

					If manantDataList.Any(Function(md) md.MandantNumber = m_InitializationData.MDData.MDNr) Then

						' Mandant is required
						lueMandant.EditValue = m_InitializationData.MDData.MDNr

					Else
						m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Mandant konnte nicht vorselektiert werden."))
						Return
					End If

				End If

			End If

			Dim preselectedMDNr As Integer = lueMandant.EditValue
			Dim selectadvisorkst As Boolean? = m_Utility.ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(preselectedMDNr),
																																					 String.Format("{0}/selectadvisorkst", FORM_XML_MAIN_KEY)), False)
			Try
				' Advisor MA
				If (hasPreselectionData AndAlso Not String.IsNullOrWhiteSpace(PreselectionData.BeraterMA)) Then
					SelectAdvisor(lueAdvisor1, PreselectionData.BeraterMA)
				ElseIf selectadvisorkst Then
					SelectAdvisor(lueAdvisor1, m_InitializationData.UserData.UserKST)
				End If
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

			Try
				' Advisor KD
				If hasPreselectionData AndAlso Not String.IsNullOrWhiteSpace(PreselectionData.BeraterKD) Then
					SelectAdvisor(lueAdvisor2, PreselectionData.BeraterKD)
				ElseIf selectadvisorkst Then
					SelectAdvisor(lueAdvisor2, m_InitializationData.UserData.UserKST)
				End If
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

			' Kostenstellen
			Try
				If lueAdvisor1.EditValue = lueAdvisor2.EditValue And lueAdvisor1.EditValue = m_InitializationData.UserData.UserKST Then
					SelectKst1(m_InitializationData.UserData.UserKST_1)
					lueKst2.EditValue = (m_InitializationData.UserData.UserKST_2)

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			End Try

		End Sub

		''' <summary>
		''' Selects the Kst1.
		''' </summary>
		''' <param name="kst1">The kst1</param>
		Private Sub SelectKst1(ByVal kst1 As String)

			Dim suppressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			lueKst1.EditValue = kst1
			LoadKst2DropDown()

			m_SuppressUIEvents = suppressUIEventState

		End Sub

		''' <summary>
		''' Selects an advisor and add missing advisor
		''' </summary>
		Private Sub SelectAdvisor(lueAdvisor As LookUpEdit, advisorKST As String)
			Dim advisor = (From a In m_Advisors Where a.KST = advisorKST).FirstOrDefault
			If advisor Is Nothing Then
				'Add missing advisor
				m_Advisors.Add(New DatabaseAccess.Common.DataObjects.AdvisorData With {.KST = advisorKST})
			End If
			lueAdvisor.EditValue = advisorKST
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles change of mandant.
		''' </summary>
		Private Sub OnLueMandant_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

			If Not lueMandant.EditValue Is Nothing Then

				Dim mandantData = CType(lueMandant.GetSelectedDataRow(), MandantData)

				m_SelectedMandantData = mandantData
				m_UCMediator.HandleChangeOfMandant(m_SelectedMandantData.MandantNumber)

				LoadKst1DropDown()
				LoadAdvisorDropDown()

			Else
				m_SelectedMandantData = Nothing

				lueKst1.EditValue = Nothing
				lueKst2.EditValue = Nothing
				lueAdvisor1.EditValue = Nothing
				lueAdvisor2.EditValue = Nothing

				lueKst1.Properties.DataSource = Nothing
				lueKst2.Properties.DataSource = Nothing
				lueAdvisor1.Properties.DataSource = Nothing
				lueAdvisor2.Properties.DataSource = Nothing

			End If

		End Sub

		''' <summary>
		''' Handles change of KST1.
		''' </summary>
		Private Sub OnLueKst1_EditValueChanged(sender As Object, e As EventArgs) Handles lueKst1.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			LoadKst2DropDown()
		End Sub

		''' <summary>
		''' Handles the advisor changed event
		''' </summary>
		Private Sub OnlueAdvisorInEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueAdvisor1.EditValueChanged, lueAdvisor2.EditValueChanged
			Dim advisor1 = lueAdvisor1.EditValue
			Dim advisor2 = lueAdvisor2.EditValue

			LoadAdvisorKst12Data()

			If String.IsNullOrWhiteSpace(advisor1) Then
				lblSelectedAdvisors.Text = advisor2
			ElseIf String.IsNullOrWhiteSpace(advisor2) Then
				lblSelectedAdvisors.Text = advisor1
			ElseIf advisor1 = advisor2 Then
				lblSelectedAdvisors.Text = advisor1
			Else
				lblSelectedAdvisors.Text = advisor1 + "/" + advisor2
			End If

		End Sub

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_DELETE_BUTTON Then

				If TypeOf sender Is BaseEdit Then
					If CType(sender, BaseEdit).Properties.ReadOnly Then
						' nothing
					Else
						CType(sender, BaseEdit).EditValue = Nothing
					End If
				End If
			End If
		End Sub

#End Region

  End Class

End Namespace

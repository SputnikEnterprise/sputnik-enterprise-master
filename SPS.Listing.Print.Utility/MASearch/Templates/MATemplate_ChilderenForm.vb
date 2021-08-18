
Imports System.IO
Imports DevExpress.Pdf

Partial Class ClsLLMATemplatesPrint

	Private m_ShowAdvisorData As Boolean
	Private m_EnableEditingTemplateData As Boolean
	Private m_DoneNLAFiles As List(Of String)
	Private m_CurrentDoneNLAFile As String

	Public Function FillEUEFTAForm(ByVal employeeNumber As Integer, ByVal customerNumber As Integer?, ByVal employmentNumber As Integer) As PrintResult
		Dim result As PrintResult = New PrintResult With {.Printresult = True}

		m_CurrentEmployeeNumber = employeeNumber
		result.Printresult = result.Printresult AndAlso CreatedPDFEuEfTaForm()


		Return result

	End Function


	Public Function FillKiAuForm(ByVal employeeNumber As Integer, ByVal customerNumber As Integer?, ByVal employmentNumber As Integer) As PrintResult
		Dim result As PrintResult = New PrintResult With {.Printresult = True}

		m_CurrentEmployeeNumber = employeeNumber
		result.Printresult = result.Printresult AndAlso CreatedPDFKiAuForm()


		Return result

	End Function

	Public Function LoadAllPDFFields(ByVal tplFilename As String) As String
		Dim result As String = String.Empty

		result = ListAllPDFFields(tplFilename)

		Return result

	End Function

	Private Function ListAllPDFFields(ByVal pdfFilename As String) As String
		Dim result As String = String.Empty

		Using documentProcessor As New PdfDocumentProcessor()
			documentProcessor.LoadDocument(pdfFilename)

			' Get names of interactive form fields.
			Dim formData As PdfFormData = documentProcessor.GetFormData()
			Dim names As IList(Of String) = formData.GetFieldNames()

			' Show the field names in the rich text box.
			Dim strings(names.Count - 1) As String
			names.CopyTo(strings, 0)

			Dim fieldInfos As New List(Of String)
			For Each itm In strings
				fieldInfos.Add(String.Format("{0} >>> {1}", itm, formData(itm).Value))
			Next
			result = String.Join("|", fieldInfos.ToList())

		End Using

		Return result

	End Function

	Private Function CreatedPDFEuEfTaForm() As Boolean
		Dim result As Boolean = True
		Dim formData As DevExpress.Pdf.PdfFormData = Nothing

		m_CurrentDoneNLAFile = String.Empty

		result = result AndAlso LoadData(m_CurrentEmployeeNumber, Nothing, Nothing)
		If Not result Then Return False

		Dim canton As String = m_EmployeeData.MA_Canton
		If String.IsNullOrWhiteSpace(canton) Then canton = m_InitializationData.MDData.MDCanton

		Dim employeeLanguage = m_EmployeeData.Language
		Select Case employeeLanguage.ToLower().TrimEnd()
			Case "italienisch", "it", "i"
				employeeLanguage = "IT"
			Case "französisch", "fr", "f"
				employeeLanguage = "FR"
			Case "englisch", "en", "e"
				employeeLanguage = "EN"

			Case Else
				employeeLanguage = ""
		End Select


		Dim tplFilename = Path.Combine(m_InitializationData.MDData.MDTemplatePath, employeeLanguage, "EU_EFTA Forms", String.Format("A1 Formular {0}.pdf", canton))
		If Not File.Exists(tplFilename) Then
			tplFilename = Path.Combine(Path.GetPathRoot(m_InitializationData.MDData.MDTemplatePath), "Templates", employeeLanguage, "EU_EFTA Forms", String.Format("A1 Formular {0}.pdf", canton))
			If Not File.Exists(tplFilename) Then
				m_Logger.LogWarning(String.Format("template was not founded!", tplFilename))
				m_UtilityUi.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage wurde nicht gefunden! {0}"), tplFilename))

				Return False
			End If

		End If

		PrintSetting.TemplateName = tplFilename
		ListAllPDFFields(PrintSetting.TemplateName)

		Using documentProcessor As New PdfDocumentProcessor()

			documentProcessor.LoadDocument(tplFilename)
			' Obtain interactive form data from a document.
			formData = documentProcessor.GetFormData()

			Dim TempFilename As String = Path.GetTempFileName
			TempFilename = Path.ChangeExtension(TempFilename, "PDF")

			Try
				' Specify values for FirstName, LastName and Gender form fields.

				formData("Familienname(n)").Value = m_EmployeeData.Lastname
				formData("Vorname(n)").Value = m_EmployeeData.Firstname
				formData("Geburtsort").Value = m_EmployeeData.BirthPlace
				formData("Geburtsdatum_Tag").Value = String.Format("{0}", Day(m_EmployeeData.Birthdate))
				formData("Geburtsdatum_Monat").Value = String.Format("{0}", Month(m_EmployeeData.Birthdate))
				formData("Geburtsdatum_Jahr").Value = String.Format("{0}", Year(m_EmployeeData.Birthdate))

				formData("Geburtsland").Value = m_EmployeeData.Nationality
				formData("Adresse").Value = String.Format("{0}, {1} {2}", m_EmployeeData.Street, m_EmployeeData.Postcode, m_EmployeeData.Location)
				formData("Staatsangehörigkeit").Value = m_EmployeeData.Nationality
				If m_EmployeeData.Gender = "W" Then formData("weiblich").Value = "On"
				If m_EmployeeData.Gender = "M" Then formData("männlich").Value = "On"
				If m_EmployeeData.CivilStatus = "L" Then formData("ledig").Value = "On"
				If m_EmployeeData.CivilStatus = "V" Then formData("verheiratet").Value = "On"
				If m_EmployeeData.CivilStatus = "W" Then formData("verwitwet").Value = "On"
				If m_EmployeeData.CivilStatus = "G" Then formData("geschieden seit wann").Value = "On"


			Catch ex As Exception
				m_Logger.LogError(String.Format("error during filling form. {0}", ex.ToString))

				Return False
			End Try

			If result Then
				documentProcessor.SaveDocument(TempFilename)
				m_Logger.LogWarning(String.Format("for employee: {0} form is not saved as readonly! EnableEditingTemplateData: {1}", m_EmployeeData.EmployeeNumber, m_EnableEditingTemplateData))
				Process.Start(TempFilename)

			Else
				m_Logger.LogWarning(String.Format("for employee: {0} form could not be successfully filled!", m_EmployeeData.EmployeeNumber))
			End If

		End Using


		Return result
	End Function

	Private Function CreatedPDFKiAuForm() As Boolean
		Dim result As Boolean = True
		Dim formData As DevExpress.Pdf.PdfFormData = Nothing

		m_CurrentDoneNLAFile = String.Empty

		result = result AndAlso LoadData(m_CurrentEmployeeNumber, Nothing, Nothing)
		If Not result Then Return False

		Dim canton As String = m_EmployeeData.MA_Canton
		If String.IsNullOrWhiteSpace(canton) Then canton = m_InitializationData.MDData.MDCanton

		Dim employeeLanguage = m_EmployeeData.Language
		Select Case employeeLanguage.ToLower().TrimEnd()
			Case "italienisch", "it", "i"
				employeeLanguage = "IT"
			Case "französisch", "fr", "f"
				employeeLanguage = "FR"
			Case "englisch", "en", "e"
				employeeLanguage = "EN"

			Case Else
				employeeLanguage = ""
		End Select


		Dim tplFilename = Path.Combine(m_InitializationData.MDData.MDTemplatePath, employeeLanguage, "Kinderzulagen", String.Format("{0}", New FileInfo(PrintSetting.TemplateName).Name))
		If Not File.Exists(tplFilename) Then
			tplFilename = Path.Combine(Path.GetPathRoot(m_InitializationData.MDData.MDTemplatePath), "Templates", employeeLanguage, "Kinderzulagen", String.Format("{0}", New FileInfo(PrintSetting.TemplateName).Name))
			If Not File.Exists(tplFilename) Then
				m_Logger.LogWarning(String.Format("template was not founded!", tplFilename))
				m_UtilityUi.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Ihre Vorlage wurde nicht gefunden! {0}"), tplFilename))

				Return False
			End If

		End If

		PrintSetting.TemplateName = tplFilename
		ListAllPDFFields(PrintSetting.TemplateName)

		Using documentProcessor As New PdfDocumentProcessor()

			documentProcessor.LoadDocument(tplFilename)
			' Obtain interactive form data from a document.
			formData = documentProcessor.GetFormData()

			Dim TempFilename As String = Path.GetTempFileName
			TempFilename = Path.ChangeExtension(TempFilename, "PDF")

			Try
				Dim AHVMitgliedNr As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/AHVMitgliedNr", m_AHVSetting))

				formData("AbrNr_5").Value = String.Format("{0}", AHVMitgliedNr)
				formData("Name ArbeitgeberArbeitgeberin").Value = m_InitializationData.MDData.MDName
				formData("Adresse-AG").Value = m_InitializationData.MDData.MDStreet
				formData("PLZ Ort-AG").Value = String.Format("{0} {1}", m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity)


				formData("Geburtsdatum_Tag").Value = String.Format("{0}", Day(m_EmployeeData.Birthdate))
				formData("Geburtsdatum_Monat").Value = String.Format("{0}", Month(m_EmployeeData.Birthdate))
				formData("Geburtsdatum_Jahr").Value = String.Format("{0}", Year(m_EmployeeData.Birthdate))

				formData("Geburtsland").Value = m_EmployeeData.Nationality
				formData("Adresse").Value = String.Format("{0}, {1} {2}", m_EmployeeData.Street, m_EmployeeData.Postcode, m_EmployeeData.Location)
				formData("Staatsangehörigkeit").Value = m_EmployeeData.Nationality
				If m_EmployeeData.Gender = "W" Then formData("weiblich").Value = "On"
				If m_EmployeeData.Gender = "M" Then formData("männlich").Value = "On"
				If m_EmployeeData.CivilStatus = "L" Then formData("ledig").Value = "On"
				If m_EmployeeData.CivilStatus = "V" Then formData("verheiratet").Value = "On"
				If m_EmployeeData.CivilStatus = "W" Then formData("verwitwet").Value = "On"
				If m_EmployeeData.CivilStatus = "G" Then formData("geschieden seit wann").Value = "On"


			Catch ex As Exception
				m_Logger.LogError(String.Format("error during filling form. {0}", ex.ToString))

				Return False
			End Try

			If result Then
				documentProcessor.SaveDocument(TempFilename)
				m_Logger.LogWarning(String.Format("for employee: {0} form is not saved as readonly! EnableEditingTemplateData: {1}", m_EmployeeData.EmployeeNumber, m_EnableEditingTemplateData))
				Process.Start(TempFilename)

			Else
				m_Logger.LogWarning(String.Format("for employee: {0} form could not be successfully filled!", m_EmployeeData.EmployeeNumber))
			End If

		End Using


		Return result
	End Function

End Class

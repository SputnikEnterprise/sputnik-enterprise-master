

Imports SP.Infrastructure.Utility
Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure

Namespace MATemplates

	Public Class ClsPrintMATemplates


#Region "private consts"

		Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
		Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_SUVA_SETTING As String = "MD_{0}/SUVA-Daten"
		Private Const MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING As String = "MD_{0}/Fak-Daten"

#End Region


		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private m_mandant As Mandant

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

		Private m_MandantXMLFile As String
		Private m_MandantFormXMLFileName As String
		Private m_MandantSetting As String
		Private m_SonstigesSetting As String
		Private m_SuvaSetting As String
		Private m_AHVSetting As String
		Private m_FAKSetting As String


#Region "Constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)
			m_InitializationData = _setting

			m_mandant = New Mandant

			m_UtilityUI = New UtilityUI

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)

			m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
			m_FAKSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_FAK_SETTING, m_InitializationData.MDData.MDNr)

		End Sub

#End Region

		Public Function PrintMATemplateForWOS(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Dim _clsMAPrint As New ClsLLMATemplatesPrint(m_InitializationData)
			_clsMAPrint.PrintSetting = _printSetting

			result.Printresult = _clsMAPrint.FillEmployeeDataIntoPDU1PDF()

			_clsMAPrint.Dispose()


			Return result

		End Function


		Public Function PrintMATemplatePDU1PDF(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Dim _clsMAPrint As New ClsLLMATemplatesPrint(m_InitializationData)
			_clsMAPrint.PrintSetting = _printSetting

			result.Printresult = _clsMAPrint.FillEmployeeDataIntoPDU1PDF()

			_clsMAPrint.Dispose()


			Return result

		End Function

		Public Function PrintTGQuest110PDF(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Dim _clsMAPrint As New ClsLLMATemplatesPrint(m_InitializationData)
			_clsMAPrint.PrintSetting = _printSetting

			result.Printresult = _clsMAPrint.FillPDFQSTFormForTGWithIText()

			_clsMAPrint.Dispose()


			Return result

		End Function

		Public Function PrintEU_EFTAFormularPDF(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Dim _clsMAPrint As New ClsLLMATemplatesPrint(m_InitializationData)
			_clsMAPrint.PrintSetting = _printSetting

			result = _clsMAPrint.FillEUEFTAForm(_printSetting.EmployeeNumbers2Print(0), Nothing, Nothing)

			_clsMAPrint.Dispose()


			Return result

		End Function

		Public Function PrintChilderFormularPDF(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Dim _clsMAPrint As New ClsLLMATemplatesPrint(m_InitializationData)
			_clsMAPrint.PrintSetting = _printSetting

			result = _clsMAPrint.FillKiAuForm(_printSetting.EmployeeNumbers2Print(0), Nothing, Nothing)

			_clsMAPrint.Dispose()


			Return result

		End Function

		Public Function AddContentToPDF(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}

			Dim _clsMAPrint As New ClsLLMATemplatesPrint(m_InitializationData)
			_clsMAPrint.PrintSetting = _printSetting

			result.Printresult = _clsMAPrint.PutImageToPDF(_printSetting.TemplateName)

			_clsMAPrint.Dispose()


			Return result

		End Function

		Public Function SplitPDFDocument(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim pdffiles As New List(Of String)

			Try
				Dim test As New PdfManipulation2
				pdffiles = test.SplitPdfByPages(_printSetting.TemplateName, 1)

			Catch ex As Exception

			End Try


			Return result

		End Function

		Public Function GetPDFFieldData(ByVal _printSetting As ClsLLMATemplateSetting) As PrintResult
			Dim result As PrintResult = New PrintResult With {.Printresult = True}
			Dim pdffiles As New Dictionary(Of String, String)

			Try
				Dim test As New PdfManipulation2
				_printSetting.TemplateName = "C:\Users\username\Documents\KIZU Anmeldung work24 neu.pdf"
				pdffiles = test.GetAcroFieldsData(_printSetting.TemplateName)

				Trace.WriteLine(String.Join(",", pdffiles.ToArray))

			Catch ex As Exception

			End Try


			Return result

		End Function

		Public Function LoadPDFFieldDataWithDX(ByVal tplFilename As String) As String

			Dim result As String = String.Empty
			Dim _clsMAPrint As New ClsLLMATemplatesPrint(m_InitializationData)

			result = _clsMAPrint.LoadAllPDFFields(tplFilename)

			_clsMAPrint.Dispose()


			Return result

		End Function



	End Class

End Namespace


Imports iTextSharp.text.pdf
Imports System.IO
Imports SP.Infrastructure.Logging
Imports Logger = SP.Infrastructure.Logging.Logger


Namespace PDFUtilities

	Public Class Utilities

		Private m_Logger As ILogger = New Logger()

		Public Function MergePdfFiles(ByVal pdfFiles() As String, ByVal outputPath As String) As Boolean
			Dim result As Boolean = False
			Dim pdfCount As Integer = 0     'total input pdf file count
			Dim f As Integer = 0    'pointer to current input pdf file
			Dim fileName As String
			Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
			Dim pageCount As Integer = 0
			Dim document As iTextSharp.text.Document = Nothing    'the output pdf document
			Dim writer As PdfWriter = Nothing
			Dim cb As PdfContentByte = Nothing

			Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
			Dim rotation As Integer = 0

			If pdfFiles.Count = 0 Then
				m_Logger.LogError(String.Format("list of files is empty."))

				Return False
			End If
			If String.IsNullOrWhiteSpace(outputPath) Then
				m_Logger.LogError(String.Format("no destination is defined."))

				Return False
			End If

			Try
				pdfCount = pdfFiles.Length
				If pdfCount > 1 Then
					'Open the 1st item in the array PDFFiles
					fileName = pdfFiles(f)
					If Not File.Exists(fileName) Then
						m_Logger.LogError(String.Format("file not founded. {0}", fileName))

						Return False
					End If

					reader = New iTextSharp.text.pdf.PdfReader(fileName)
					'Get page count
					pageCount = reader.NumberOfPages

					document = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(1), 0.5, 0.5, 0.5, 0.5)

					writer = PdfWriter.GetInstance(document, New FileStream(outputPath, FileMode.OpenOrCreate))

					With document
						.Open()
					End With
					'Instantiate a PdfContentByte object
					cb = writer.DirectContent

					'Now loop thru the input pdfs
					While f < pdfCount
						'Declare a page counter variable
						Dim i As Integer = 0
						'Loop thru the current input pdf's pages starting at page 1
						While i < pageCount
							i += 1
							'Get the input page size
							document.SetPageSize(reader.GetPageSizeWithRotation(i))

							'Create a new page on the output document
							document.NewPage()
							'If it is the 1st page, we add bookmarks to the page
							'Now we get the imported page
							page = writer.GetImportedPage(reader, i)
							'Read the imported page's rotation
							rotation = reader.GetPageRotation(i)

							'Then add the imported page to the PdfContentByte object as a template based on the page's rotation
							If rotation = 90 Then
								cb.AddTemplate(page, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(i).Height)

							ElseIf rotation = 270 Then
								cb.AddTemplate(page, 0, 1.0F, -1.0F, 0, reader.GetPageSizeWithRotation(i).Width + 60, -30)

							Else
								cb.AddTemplate(page, 1.0F, 0, 0, 1.0F, 0, 0)
							End If

						End While

						'Increment f and read the next input pdf file
						f += 1
						If f < pdfCount Then
							fileName = pdfFiles(f)
							reader = New iTextSharp.text.pdf.PdfReader(fileName)
							pageCount = reader.NumberOfPages
						End If

					End While

					'When all done, we close the document so that the pdfwriter object can write it to the output file
					document.Close()
					reader.Close()

					result = True

				End If


			Catch ex As Exception

				Return False
			End Try

			Return result
		End Function

		Public Function LoadPDFFieldsData(ByVal pdfFilename As String) As List(Of String)
			Dim result As New List(Of String)
			Dim pdfTemplate As String = pdfFilename

			If Not File.Exists(pdfFilename) Then
				m_Logger.LogError(String.Format("file not founded. {0}", pdfFilename))

				Return Nothing
			End If

			Dim readerPDF As New PdfReader(PdfTemplate)
			Dim PDFfld As Object

			For Each PDFfld In readerPDF.AcroFields.Fields
				result.Add(PDFfld.key.ToString())
			Next

			Return result
		End Function

		Public Function GetPlainTextFromPDF(ByVal pdfFileName As String) As String
			If Not File.Exists(pdfFileName) Then
				m_Logger.LogError(String.Format("file not founded. {0}", pdfFileName))

				Return String.Empty
			End If
			Dim oReader As New PdfReader(pdfFileName)
			Dim sOut As String = String.Empty

			For i = 1 To oReader.NumberOfPages
				Dim its As New parser.SimpleTextExtractionStrategy

				sOut &= parser.PdfTextExtractor.GetTextFromPage(oReader, i, its)
			Next

			Return sOut
		End Function


	End Class

End Namespace

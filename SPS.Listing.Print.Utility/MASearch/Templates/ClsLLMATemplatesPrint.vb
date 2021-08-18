
Imports SP.Infrastructure.Logging

Imports System.IO

Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Customer

Imports SPProgUtility.Mandanten

Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.Internal.Automations.BaseTable

Public Class ClsLLMATemplatesPrint
	Implements IDisposable
	Protected disposed As Boolean = False


#Region "Private consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"

#End Region



#Region "private fields"

	Private Shared m_Logger As ILogger = New Logger()

	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_EmploymentDatabaseAccess As IESDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The Listing data access object.
	''' </summary>
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private m_connectionString As String

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility


	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_CurrentEmployeeNumber As Integer
	Private m_CurrentCustomerNumber As Integer
	Private m_CurrentEmploymentNumber As Integer

	Private m_EmployeeData As EmployeeMasterData
	Private m_CustomerData As CustomerMasterData
	Private m_EmploymentData As ESMasterData
	Private m_EmployeeEmploymentData As IEnumerable(Of EmployeeESProperty)
	Private m_AHVSetting As String
	Private m_PrintJobData As PrintJobData
	Private m_EmployeeLanguage As String


#End Region


#Region "public properties"

	Public Property PrintSetting As ClsLLMATemplateSetting

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_UtilityUi = New SP.Infrastructure.UI.UtilityUI
		m_MandantData = New Mandant
		m_path = New SPProgUtility.ProgPath.ClsProgPath
		m_Utility = New SP.Infrastructure.Utility

		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmploymentDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)

		m_BaseTableUtil = New SPSBaseTables(m_InitializationData)
		m_PermissionData = m_BaseTableUtil.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_BaseTableUtil.BaseTableName = "Country"
		m_CountryData = m_BaseTableUtil.PerformCVLBaseTablelistWebserviceCall()

	End Sub


	Protected Overridable Overloads Sub Dispose(
		 ByVal disposing As Boolean)
		If Not Me.disposed Then
			If disposing Then

			End If
			' Add code here to release the unmanaged resource.
			LL.Dispose()
			LL.Core.Dispose()
			' Note that this is not thread safe.
		End If
		Me.disposed = True
	End Sub

#Region " IDisposable Support "
	' Do not change or add Overridable to these methods.
	' Put cleanup code in Dispose(ByVal disposing As Boolean).
	Public Overloads Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub
	Protected Overrides Sub Finalize()
		Dispose(False)
		MyBase.Finalize()
	End Sub

#End Region


#End Region


#Region "private properties"

	ReadOnly Property GetPDFVW_O2SSerial() As String
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Return _ClsProgSetting.GetPDFVW_O2SSerial  ' "yourlicencekey"
		End Get
	End Property

	ReadOnly Property GetPDF_O2SSerial() As String
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Return _ClsProgSetting.GetPDF_O2SSerial ' "yourlicencekey"
		End Get
	End Property

#End Region


	Function FillEmployeeDataIntoPDU1PDF_() As Boolean

		Return True
	End Function

	Public Function FillEmployeeDataIntoPDU1PDF() As Boolean
		Dim success As Boolean = True

		Dim filename = PrintSetting.TemplateName
		If Not File.Exists(filename) Then Return False

		If PrintSetting.EmployeeNumbers2Print Is Nothing OrElse PrintSetting.EmployeeNumbers2Print.Count = 0 Then
			m_Logger.LogError("Keine Kandidatennummer wurde übergeben!")

			Return False
		End If

		For Each Nr In PrintSetting.EmployeeNumbers2Print
			m_CurrentEmployeeNumber = Nr
		Next
		If Not PrintSetting.EmploymentNumbers2Print Is Nothing AndAlso PrintSetting.EmploymentNumbers2Print.Count > 0 Then
			For Each Nr In PrintSetting.EmploymentNumbers2Print
				m_CurrentEmploymentNumber = Nr
			Next
		End If

		If Not PrintSetting.CustomerNumbers2Print Is Nothing AndAlso PrintSetting.CustomerNumbers2Print.Count > 0 Then
			For Each Nr In PrintSetting.CustomerNumbers2Print
				m_CurrentCustomerNumber = Nr
			Next
		End If

		Try
			success = success AndAlso LoadData(m_CurrentEmployeeNumber, Nothing, Nothing)
			If Not success Then Return False

			Dim employeeAddressData = m_EmployeeDatabaseAccess.LoadEmployeeDivAddressData(m_CurrentEmployeeNumber)
			Dim addressData = employeeAddressData.Where(Function(data) data.ActiveRec = True And (data.ForDivers = True Or data.ForEmployment = True)).FirstOrDefault()

			Dim pdfReader As PdfReader = New PdfReader(filename)

			Dim TempFilename As String = Path.GetTempFileName()
			TempFilename = Path.ChangeExtension(TempFilename, "PDF")

			Dim pdfStamp = New PdfStamper(pdfReader, New System.IO.FileStream(TempFilename, System.IO.FileMode.Create))
			Dim fields As AcroFields = pdfStamp.AcroFields

			For Each fi In pdfStamp.AcroFields.Fields
				Trace.WriteLine(fi.Key)
			Next

			pdfStamp.AcroFields.SetField("Name Vorname", String.Format("{0}", m_EmployeeData.EmployeeFullnameWithComma))
			pdfStamp.AcroFields.SetField("Sozialversicherungsnummer", String.Format("{0}", m_EmployeeData.AHV_Nr_New))
			pdfStamp.AcroFields.SetField("Geburtsdatum", String.Format("{0:d}", m_EmployeeData.Birthdate))
			pdfStamp.AcroFields.SetField("Staatsangehörigkeiten", String.Format("{0}", m_EmployeeData.Nationality))
			pdfStamp.AcroFields.SetField("Geburtsort Ort Land", String.Format("{0}, {1}", m_EmployeeData.BirthPlace, m_EmployeeData.Country))
			pdfStamp.AcroFields.SetField("Geschlecht", If(m_EmployeeData.Gender = "M", "Männlich", "Weiblich"))

			If m_EmployeeData.Country <> "CH" Then
				pdfStamp.AcroFields.SetField("Adresse im Ausland Strasse Nummer", String.Format("{0}", m_EmployeeData.Street))
				pdfStamp.AcroFields.SetField("PLZ Ort Land", String.Format("{0}", m_EmployeeData.EmployeeAddress))

				If Not addressData Is Nothing Then
					pdfStamp.AcroFields.SetField("Adresse in der Schweiz Strasse Nummer", String.Format("{0}", addressData.Street))
					pdfStamp.AcroFields.SetField("PLZ Ort", String.Format("{0}", addressData.EmployeeAddress))
				End If

			Else
				pdfStamp.AcroFields.SetField("Adresse in der Schweiz Strasse Nummer", String.Format("{0}", m_EmployeeData.Street))
				pdfStamp.AcroFields.SetField("PLZ Ort", String.Format("{0}", m_EmployeeData.EmployeeAddress))
			End If

			Dim i As Integer = 0
			For Each employment In m_EmployeeEmploymentData
				Dim customerData = m_CustomerDatabaseAccess.LoadCustomerMasterData(employment.kdnr, m_InitializationData.UserData.UserFiliale)

				Dim strDocFieldName As String = String.Format("Zeitraum von bis.0.{0}", i)
				pdfStamp.AcroFields.SetField(strDocFieldName, String.Format("{0}", employment.periode))

				strDocFieldName = String.Format("Art der Tätigkeit bzw Erläuterung.0.{0}", i)
				pdfStamp.AcroFields.SetField(strDocFieldName, String.Format("{0}", employment.esals))

				strDocFieldName = String.Format("Art Nr.0.{0}", i)
				pdfStamp.AcroFields.SetField(strDocFieldName, String.Format("{0}", 1))

				strDocFieldName = String.Format("Name und Adresse des Arbeitgebersder Arbeitgeberin der Firma oder der zahlenden Stelle.0.{0}", i)
				pdfStamp.AcroFields.SetField(strDocFieldName, String.Format("{0}, {1} {2} {3}", m_InitializationData.MDData.MDName, m_InitializationData.MDData.MDStreet,
																																		m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity))

				i += 1
				If i > 6 Then Exit For
			Next

			pdfStamp.AcroFields.SetField("Ort und Datum", String.Format("{0} {1:d}", m_InitializationData.MDData.MDCity, Now.Date))

			' make it as normal file withou possibility for fill new form file
			'pdfStamp.FormFlattening = True

			pdfStamp.Close()
			pdfReader.Close()

			If File.Exists(TempFilename) Then Process.Start(TempFilename)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

			success = False
		End Try


		Return success

	End Function

	Public Function FillPDFQSTFormForTGWithIText() As Boolean
		Dim employeeNumber As Integer = 0
		Dim employmentNumber As Integer = 0
		Dim customerNumber As Integer = 0
		Dim success As Boolean = True

		Dim filename = PrintSetting.TemplateName
		If Not File.Exists(filename) Then Return False


		If PrintSetting.EmployeeNumbers2Print Is Nothing OrElse PrintSetting.EmployeeNumbers2Print.Count = 0 Then
			m_Logger.LogError("Keine Kandidatennummer wurde übergeben!")

			Return False
		End If

		For Each Nr In PrintSetting.EmployeeNumbers2Print
			employeeNumber = Nr
		Next
		If Not PrintSetting.EmploymentNumbers2Print Is Nothing AndAlso PrintSetting.EmploymentNumbers2Print.Count > 0 Then
			For Each Nr In PrintSetting.EmploymentNumbers2Print
				m_CurrentEmploymentNumber = Nr
			Next
		End If

		If Not PrintSetting.CustomerNumbers2Print Is Nothing AndAlso PrintSetting.CustomerNumbers2Print.Count > 0 Then
			For Each Nr In PrintSetting.CustomerNumbers2Print
				customerNumber = Nr
			Next
		End If

		Try
			success = success AndAlso LoadData(employeeNumber, Nothing, m_CurrentEmploymentNumber)

			Dim civilData = m_CommonDatabaseAccess.LoadCivilStateData()
			Dim civilCode = civilData.Where(Function(data) data.GetField = m_EmployeeData.CivilStatus).FirstOrDefault()


			Dim pdfReader As PdfReader = New PdfReader(filename)

			Dim TempFilename As String = Path.GetTempFileName()
			TempFilename = Path.ChangeExtension(TempFilename, "PDF")



			Dim pdfStamp = New PdfStamper(pdfReader, New System.IO.FileStream(TempFilename, System.IO.FileMode.Create))

			Dim fields As AcroFields = pdfStamp.AcroFields

			pdfStamp.AcroFields.SetField("Textfield0", String.Format("{0}", m_InitializationData.MDData.MDName))
			pdfStamp.AcroFields.SetField("Textfield1", String.Format("{0}", m_InitializationData.MDData.MDName_2))
			pdfStamp.AcroFields.SetField("Textfield2", String.Format("{0}", m_InitializationData.MDData.MDStreet))
			pdfStamp.AcroFields.SetField("Textfield3", String.Format("{0} {1}", m_InitializationData.MDData.MDPostcode, m_InitializationData.MDData.MDCity))
			pdfStamp.AcroFields.SetField("Textfield4", String.Format("{0}", m_InitializationData.UserData.UserFullName))
			pdfStamp.AcroFields.SetField("Textfield5", String.Format("{0}", m_InitializationData.UserData.UserMDTelefon))
			pdfStamp.AcroFields.SetField("Textfield6", String.Format("{0}", m_InitializationData.UserData.UserMDeMail))
			pdfStamp.AcroFields.SetField("ELM", "1")

			pdfStamp.AcroFields.SetField("Geschlecht", If(m_EmployeeData.Gender = "M", "On", "on"))
			pdfStamp.AcroFields.SetField("Textfield7", String.Format("{0}", m_EmployeeData.Lastname))
			pdfStamp.AcroFields.SetField("Textfield13", String.Format("{0}", m_EmployeeData.Firstname))
			pdfStamp.AcroFields.SetField("7560", String.Format("{0}", m_EmployeeData.AHV_Nr_New))
			pdfStamp.AcroFields.SetField("Textfield8", String.Format("{0}", civilCode.TranslatedCivilState))
			pdfStamp.AcroFields.SetField("Textfield14", String.Format("{0:d}", m_EmployeeData.Birthdate))
			pdfStamp.AcroFields.SetField("Textfield9", String.Format("{0}", m_EmployeeData.Nationality))
			pdfStamp.AcroFields.SetField("Textfield10", String.Format("{0}", m_EmployeeData.Street))
			pdfStamp.AcroFields.SetField("Textfield11", String.Format("{0}", m_EmployeeData.Profession))
			pdfStamp.AcroFields.SetField("Textfield12", String.Format("{0:d}", m_EmploymentData.ES_Ab))
			pdfStamp.AcroFields.SetField("Textfield16", String.Format("{0}", m_EmployeeData.Permission))
			pdfStamp.AcroFields.SetField("Textfield17", String.Format("{0} {1}", m_EmployeeData.Postcode, m_EmployeeData.Location))
			pdfStamp.AcroFields.SetField("Textfield18", String.Format("{0}", m_EmploymentData.Arbort))

			pdfStamp.AcroFields.SetField("37410109a", String.Format("{0} {1:d}", m_InitializationData.MDData.MDCity, Now.Date))

			pdfStamp.Close()
			pdfReader.Close()


			If File.Exists(TempFilename) Then Process.Start(TempFilename)


		Catch ex As Exception
			Return String.Format("{0}", ex.ToString)

			success = False
		End Try


		Return success

	End Function

	Public Function PutImageToPDF(ByVal pdfFile As String) As Boolean
		Dim result As Boolean = True

		Dim imageFile As String = "C:\Users\username\Pictures\Unterschrift.jpg"
		Dim outputPDFFile As String = "C:\Users\username\Documents\result.pdf"

		Dim inputPdfStream As Stream = New FileStream(pdfFile, FileMode.Open, FileAccess.Read, FileShare.Read)
		Dim inputImageStream As Stream = New FileStream(imageFile, FileMode.Open, FileAccess.Read, FileShare.Read)
		Dim outputPdfStream As Stream = New FileStream(outputPDFFile, FileMode.Create, FileAccess.Write, FileShare.None)

		Dim reader = New PdfReader(inputPdfStream)
		Dim stamper = New PdfStamper(reader, outputPdfStream)
		Dim pdfContentByte = stamper.GetOverContent(1)
		Dim image = GetDataMatrixBarcode("mein Test") 'As Image = Image.GetInstance(inputImageStream)



		image.SetAbsolutePosition(100, 100)
		pdfContentByte.AddImage(image)

		stamper.Close()


		Return result

	End Function

	Public Function SplitPDFDocument(ByVal pdfFile As String) As Boolean
		Dim result As Boolean = True

		Dim numOfPage = 1        '< 1 page per output pdf
		Dim baseName As String = "Splitted-"   '< the base file name for output pdf's.
		SplitPdfByPages(pdfFile, numOfPage, baseName)


		Return result

	End Function


#Region "private methodes"

	Private Function LoadData(ByVal employeeNumber As Integer, ByVal customerNumber As Integer?, ByVal employmentNumber As Integer?) As Boolean
		Dim result As Boolean = True

		m_EmployeeData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
		Dim employeeAddressData = m_EmployeeDatabaseAccess.LoadEmployeeDivAddressData(employeeNumber)
		Dim addressData = employeeAddressData.Where(Function(data) data.ActiveRec = True And (data.ForDivers = True Or data.ForEmployment = True)).FirstOrDefault()
		m_EmployeeEmploymentData = m_EmployeeDatabaseAccess.LoadFoundedESForEmployeeMng(employeeNumber)

		If employmentNumber.GetValueOrDefault(0) > 0 Then m_EmploymentData = m_EmploymentDatabaseAccess.LoadESMasterData(employmentNumber)

		Dim civilData = m_CommonDatabaseAccess.LoadCivilStateData()
		Dim civilCode = civilData.Where(Function(data) data.GetField = m_EmployeeData.CivilStatus).FirstOrDefault()

		If customerNumber.GetValueOrDefault(0) > 0 Then m_CustomerData = m_CustomerDatabaseAccess.LoadCustomerMasterData(customerNumber, m_InitializationData.UserData.UserFiliale)

		If m_EmployeeData Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Keine Kandidatendaten wurden gefunden.")
			Return False
		End If

		Return result
	End Function

	Private Function GetDataMatrixBarcode(ByVal message As String) As iTextSharp.text.Image

		Dim barcode As BarcodeDatamatrix = New BarcodeDatamatrix()
		Dim barcodeDimensions() As Integer = New Integer() {10, 12, 14, 16, 18, 20, 22, 24, 26, 32, 36, 40, 44, 48, 52, 64, 72, 80, 88, 96, 104, 120, 132, 144}
		Dim returnResult As Integer

		barcode.Options = BarcodeDatamatrix.DM_AUTO

		For generateCount As Integer = 0 To barcodeDimensions.Length - 1
			barcode.Width = barcodeDimensions(generateCount)
			barcode.Height = barcodeDimensions(generateCount)
			returnResult = barcode.Generate(message)
			If returnResult = BarcodeDatamatrix.DM_NO_ERROR Then
				Return barcode.CreateImage
			End If
		Next

		Throw New Exception("Error generating datamatrix barcode for text '" & message & "'")

	End Function

	Private Sub SplitPdfByPages(ByVal sourcePdf As String, ByVal numOfPages As Integer, ByVal baseNameOutPdf As String)
		Dim raf As iTextSharp.text.pdf.RandomAccessFileOrArray = Nothing
		Dim reader As iTextSharp.text.pdf.PdfReader = Nothing
		Dim doc As iTextSharp.text.Document = Nothing
		Dim pdfCpy As iTextSharp.text.pdf.PdfCopy = Nothing
		Dim page As iTextSharp.text.pdf.PdfImportedPage = Nothing
		Dim pageCount As Integer = 0

		Try
			raf = New iTextSharp.text.pdf.RandomAccessFileOrArray(sourcePdf)
			reader = New iTextSharp.text.pdf.PdfReader(raf, Nothing)
			pageCount = reader.NumberOfPages
			If pageCount < numOfPages Then
				Throw New ArgumentException("Not enough pages in source pdf to split")
			Else
				Dim ext As String = IO.Path.GetExtension(baseNameOutPdf)
				Dim outfile As String = String.Empty
				Dim n As Integer = CInt(Math.Ceiling(pageCount / numOfPages))
				Dim currentPage As Integer = 1
				For i As Integer = 1 To n
					outfile = baseNameOutPdf.Replace(ext, "_" & i & ext)
					doc = New iTextSharp.text.Document(reader.GetPageSizeWithRotation(currentPage))
					pdfCpy = New iTextSharp.text.pdf.PdfCopy(doc, New IO.FileStream(outfile, IO.FileMode.Create))
					doc.Open()
					If i < n Then
						For j As Integer = 1 To numOfPages
							page = pdfCpy.GetImportedPage(reader, currentPage)
							pdfCpy.AddPage(page)
							currentPage += 1
						Next j
					Else
						For j As Integer = currentPage To pageCount
							page = pdfCpy.GetImportedPage(reader, j)
							pdfCpy.AddPage(page)
						Next j
					End If
					doc.Close()
				Next
			End If
			reader.Close()
		Catch ex As Exception
			Throw ex
		End Try

	End Sub


#End Region




#Region "test methodes"

	Private Function PutBarcode_1(ByVal pdfFile As String) As Boolean
		Dim result As Boolean = True

		Dim Batch As String = "12345"

		'Define a new PDF Doc
		Dim doc As New Document(New iTextSharp.text.Rectangle(350, 400), 5, 5, 1, 1)  ''the size of the rectangle width x height

		Try
			Dim writer As PdfWriter = PdfWriter.GetInstance(doc, New FileStream(pdfFile, FileMode.Append))
			doc.Open()

			Dim dt As New DataTable()
			dt.Columns.Add("Batch")
			Dim row As DataRow = dt.NewRow()
			row("Batch") = Batch.ToString    '& i.ToString()
			dt.Rows.Add(row)

			Dim img1 As System.Drawing.Image = Nothing
			For i As Integer = 0 To dt.Rows.Count - 1
				If i <> 0 Then
					doc.NewPage()
				End If

				Dim cb As iTextSharp.text.pdf.PdfContentByte = writer.DirectContent
				Dim bc As iTextSharp.text.pdf.Barcode128 = New Barcode128()
				bc.TextAlignment = Element.ALIGN_LEFT
				bc.Code = dt.Rows(i)("Batch").ToString()
				bc.StartStopText = False
				bc.CodeType = iTextSharp.text.pdf.Barcode128.EAN13
				bc.Extended = True

				Dim img As iTextSharp.text.Image = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.BLACK)

				cb.SetTextMatrix(100.5F, 320.0F)
				img.ScaleToFit(240, 600)
				img.SetAbsolutePosition(5.5F, 320)
				cb.AddImage(img)
			Next i

			doc.Close()

		Catch ex As Exception
			doc.Close()
			MsgBox(ex.ToString)
		End Try


		Return result

	End Function

	Private Function PutBarcode_2(ByVal pdfFile As String) As Boolean
		Dim result As Boolean = True

		Dim myDoc As New Document(iTextSharp.text.PageSize.LETTER, 10, 10, 50, 50)

		Try

			Dim imageFile As String = "C:\Users\username\Pictures\Unterschrift.jpg"

			Dim writer As PdfWriter = PdfWriter.GetInstance(myDoc, New FileStream(pdfFile, FileMode.Append))

			myDoc.Open()

			Dim para As New Paragraph("Let's write some text before inserting image.")

			Dim myImage As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(imageFile)

			myImage.ScaleToFit(300.0F, 250.0F)

			myImage.SpacingBefore = 50.0F

			myImage.SpacingAfter = 10.0F

			myImage.Alignment = Element.ALIGN_CENTER

			myDoc.Add(para)

			myDoc.Add(myImage)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False

		End Try

		myDoc.Close()


		Return result

	End Function


#End Region




End Class

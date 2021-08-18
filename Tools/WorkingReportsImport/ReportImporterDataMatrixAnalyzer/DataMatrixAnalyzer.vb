'------------------------------------
' File: DataMatrixAnalyzer.vb
' Date: 22.08.2012
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO
Imports System.Drawing
Imports ReportImporterCommon.Logging
Imports O2S.Components.PDF4NET.PDFFile
Imports SoftekBarcodeNet.BarcodeReader

Imports System.Reflection
Imports ReportImporterCommon

''' <summary>
''' Performs DataMatrix barcode analysis of pdf files.
''' </summary>
Public Class DataMatrixAnalyzer


#Region "private Constans"

	Private Const TEMPORARY_FOLDER_SPLITTED_PDFS As String = "SPLITTED_PDFS"
	Private Const LIBRARY_LICENSEKEY As String = "yourserialnumber"
	Private Const RUNTIME_LIBRARY_FOLDER As String = ".\Libs"
	Private Const SOFTTEK_SETTING_FILE As String = "softtekSettings.xml"
	Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
	Private Const RUNTIME_MANDANT_CONFIG_FOLDER As String = "Config\Mandants"

	''' <summary>
	'''  Lizenznummer für Version 4.4.1 PDF4Net
	''' </summary>
	Private Const PDF4NET_SERIALNUMBER As String = "yourserialnumber"

#End Region

#Region "Private Fields"

	''' <summary>
	''' The logger object.
	''' </summary>
	Private Shared m_logger As ILogger = New Logger()

	Private m_mandantGuid As String
	Private m_CommonConfigFolder As String
	Private m_MandantConfigFolder As String


#End Region

#Region "Constructor"

	Public Sub New(ByVal mandantFolder As String, ByVal mandantGuid As String)

		m_mandantGuid = mandantGuid
		m_CommonConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_COMMON_CONFIG_FOLDER)
		m_MandantConfigFolder = Path.Combine(Environment.CurrentDirectory, RUNTIME_MANDANT_CONFIG_FOLDER, m_mandantGuid)

	End Sub

#End Region

#Region "Public Properties"

	Public Property SettingFileValue As ProgramSettings
	Public Property SplitPDFFileWithBarCode As Boolean

#End Region

#Region "Public Methods"

	''' <summary>
	''' Analyzes a PDF file in order to find a DataMatix code with automatic threshold detection method.
	''' </summary>
	''' <param name="pdfFileName">The PDF file full path.</param>
	''' <returns>The result of the DataMatrix analysis.</returns>
	Public Function ReadDataMatrixCode(ByVal pdfFileName As String) As DataMatrixResult

		Dim dataMatrixResult As DataMatrixResult = New DataMatrixResult(pdfFileName)

		If Not String.IsNullOrWhiteSpace(SettingFileValue.SoftekSettingFolder) AndAlso Directory.Exists(SettingFileValue.SoftekSettingFolder) Then
			m_CommonConfigFolder = SettingFileValue.SoftekSettingFolder
			m_MandantConfigFolder = Path.Combine(SettingFileValue.SoftekSettingFolder, "Mandants", m_mandantGuid)
		End If

		Dim barcodesInfo As List(Of DataMatrixInfo) = ReadDataMatrixCodesFromPDF(pdfFileName)

		For Each info In barcodesInfo
			dataMatrixResult.AddDataMatrixInfo(info)
		Next

		dataMatrixResult.CouldPDFFileBeAnalyzed = dataMatrixResult.DataMatrixInfos.Count > 0 ' True
		Return dataMatrixResult

	End Function

#End Region

#Region "Private Functions"

	Private Function ReadDataMatrixCodesFromPDF(ByVal pdfFile As String) As List(Of DataMatrixInfo)

		Dim nDataMatrixCodes As Integer
		Dim foundDataMatrixCodes As New List(Of DataMatrixInfo)

		Dim settingFile As String
		settingFile = Path.Combine(m_MandantConfigFolder, SOFTTEK_SETTING_FILE)
		If Not File.Exists(settingFile) Then
			settingFile = Path.Combine(m_CommonConfigFolder, SOFTTEK_SETTING_FILE)
		End If

#If DEBUG Then
		settingFile = "C:\Temp\XML\EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A\softtekSettings.xml"
#End If

		If Not File.Exists(settingFile) Then
			m_logger.LogError(String.Format("setting file could not be found!:{0}MDGuid: {1}{0}settingFile: {2}", vbNewLine, m_mandantGuid, settingFile))
		End If
		m_logger.LogInfo(String.Format("setting file will be read for customer_id: {1}{0}settingFile: {2}", vbNewLine, m_mandantGuid, settingFile))

		Dim directoryInfo As DirectoryInfo = New DirectoryInfo(pdfFile)
		Dim Splitfolder = Path.GetDirectoryName(pdfFile)
		Splitfolder = Path.GetDirectoryName(Splitfolder)
		Splitfolder = Path.Combine(Splitfolder, TEMPORARY_FOLDER_SPLITTED_PDFS)

		Dim pdfparentName As String = Path.GetFileNameWithoutExtension(pdfFile)
		pdfparentName = Path.GetFileNameWithoutExtension(pdfFile)

		Dim splitString As String = String.Empty
		If SplitPDFFileWithBarCode Then
			splitString = String.Format("{0}\{1} _%d_%s.PDF", Splitfolder, pdfparentName)
		End If
		m_logger.LogInfo(String.Format("file will be{1}splited:{0}", pdfparentName, If(SplitPDFFileWithBarCode, " ", " NOT ")))

		Dim barcodeReader As New SoftekBarcodeNet.BarcodeReader
		barcodeReader.LicenseKey = LIBRARY_LICENSEKEY

		Dim settingExists = barcodeReader.LoadXMLSettings(settingFile)
		If Not settingExists Then
			m_logger.LogError(String.Format("setting file could not be loaded!:{0}MDGuid: {1}{0}settingFile: {2}", vbNewLine, m_mandantGuid, settingFile))
		End If

		barcodeReader.TifSplitPath = splitString

		Try

			' background working: makes some problems with employee and customer files!
			barcodeReader.ScanBarCodeInBackground(pdfFile)
			Dim waitDuration As Integer = Math.Max(SettingFileValue.SoftekWaitDuration.GetValueOrDefault(0), 120000)
			m_logger.LogInfo(String.Format("datamatrixcode in file will be read for {0} minutes({1} ms): {2} | Settingfile: {3} >>> {4}", waitDuration / 60000, waitDuration, pdfFile, settingFile, If(settingExists, "could be loaded", "could NOT be loaded!!!")))
			barcodeReader.ScanBarCodeWait(waitDuration)
			Dim n = barcodeReader.GetScanExitCode()

			Dim count As Integer = 0
			While (barcodeReader.ScanBarCodeWait(0) = 1)
				count = barcodeReader.GetBarCodeCount()
				Trace.WriteLine(String.Format("codecount: {0} >>> still working {1}%...", count, barcodeReader.GetProgress()))

				Exit While
			End While

			n = barcodeReader.GetScanExitCode()
			nDataMatrixCodes = barcodeReader.GetBarCodeCount()

			If nDataMatrixCodes = 0 Then
				m_logger.LogWarning(String.Format("barcodeReader.ScanBarCode in file: {0} with no barcode!", pdfFile))
			ElseIf nDataMatrixCodes < 0 Then
				m_logger.LogWarning(String.Format("barcodeReader.ScanBarCode in file: {0} could not be analyzed! Error code: {1}", pdfFile, nDataMatrixCodes))
			End If
			m_logger.LogInfo(String.Format("in file {0} founded {1} datamatrix codes.", pdfFile, nDataMatrixCodes))

			For i = 1 To nDataMatrixCodes
				Dim dataMatrixCode = barcodeReader.GetBarString(i) ' 1-based index to barcode to be queried.

				If dataMatrixCode.ToString.ToLower.Split(CChar("_")).Count < 3 Then
					m_logger.LogWarning(String.Format("{0} -th datamatrix code with {1} is NOT valid!", i, dataMatrixCode))

					Continue For
				Else
					m_logger.LogInfo(String.Format("{0} -th datamatrix code with {1} is valid!", i, dataMatrixCode))

				End If

				Dim dataMatrixInfo As DataMatrixInfo = New DataMatrixInfo With {.DataMatrixValue = dataMatrixCode}
				foundDataMatrixCodes.Add(dataMatrixInfo)
			Next i


		Catch ex As Exception
			m_logger.LogError(String.Format("getting code from file: {0} was NOT successfull!{1}{2}", pdfFile, vbNewLine, ex.ToString))


		End Try


		Return foundDataMatrixCodes

	End Function


#End Region

End Class

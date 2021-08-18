
Imports SP.DatabaseAccess.Listing
Imports SPProgUtility


Public Class PrintResult

	Public Property Printresult As Boolean
	Public Property PrintresultMessage As String
	Public Property WOSresult As Boolean?
	Public Property ExportFilename As String
	Public Property JobResultInvoiceData As List(Of PrintedInvoiceData)
	Public Property JobResultPayrollData As List(Of PrintedPayrollData)

End Class

Public Class PrintedInvoiceData
	Public Property InvoiceNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property Companyname As String
	Public Property REEMail As String
	Public Property SendAsZip As Boolean?
	Public Property OneInvoicePerMail As Boolean?
	Public Property InvoiceDate As Date?
	Public Property ExportedFileName As String
	Public Property MandantName As String
	Public Property MandantLocation As String

End Class

Public Class PrintedPayrollData
	Public Property PayrollNumber As Integer?
	Public Property EmployeeNumber As Integer?
	Public Property Lastname As String
	Public Property Firstname As String
	Public Property EmployeeEMail As String
	Public Property SendAsZip As Boolean?
	Public Property PayrollDate As Date?
	Public Property ExportedFileName As String
	Public Property MandantName As String
	Public Property MandantLocation As String

End Class

Public Class ClsLLLOSearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
	Public Property SelectedMDYear As Integer
	Public Property SelectedMDGuid As String
	Public Property LogedUSNr As Integer

	Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
	Public TranslationData As Dictionary(Of String, ClsTranslationData)

	Public Property DbConnString2Open As String
	Public Property JobNr2Print As String
	Public Property SQL2Open As String
	Public Property ExportFilename As String
	Public Property ExportFinalFilename As String
	Public Property ExportPath As String
	Public Property USSignFileName As String

	Public Property AnzahlCopies As Short

	Public Property ListOfExportedFiles As New List(Of ExportPayrollData)
	Public Property liMANr2Print As New List(Of Integer)
	Public Property liLONr2Print As New List(Of Integer)
	Public Property liLOSend2WOS As New List(Of Boolean)
	Public Property WOSSendValueEnum As WOSSENDValue
	Public Property SortValue As Integer

	Public Property LiMALang As New List(Of String)

	Public Property SelectedMonth As Byte
	Public Property SelectedYear As Short
	Public Property SelectedMANr2Print As Integer
	Public Property SelectedLONr2Print As Integer
	Public Property SelectedMALang As String
	Public Property LoCreatedOn As DateTime
	Public Property LoCreatedFrom As String
	Public Property NumberOfCopy As Integer?

	Public Property Is4Export As Boolean              ' Exportiert nur in PDF
	Public Property SendData2WOS As Boolean           ' Sendet einzelne Daten zum WOS
	Public Property SendAndPrintData2WOS As Boolean   ' Sendet und Druckt die Daten zum WOS

	Public ReadOnly Property GetExportPfad() As String
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sValue As String = _ClsProgSetting.GetSpSLOTempPath
			Dim strGuid As String = "SP.LO.PrintUtility"
			Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			Dim strKeyName As String = "ExportPfad".ToLower
			Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			sValue = strBez

			Return sValue
		End Get

	End Property

	Public ReadOnly Property GetExportFinalFilename() As String
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sValue As String = "FinalFile.PDF"
			Dim strGuid As String = "SP.LO.PrintUtility"
			Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			Dim strKeyName As String = "ExportFinalFileFilename".ToLower
			Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			sValue = strBez

			Return sValue
		End Get

	End Property

	Public ReadOnly Property GetExportFilename() As String
		Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sValue As String = "LO_{0}_{1}.PDF"
			Dim strGuid As String = "SP.LO.PrintUtility"
			Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			Dim strKeyName As String = "ExportFilename".ToLower
			Dim strQuery As String = String.Format(strMainKey, Chr(34), strGuid, strKeyName)

			Dim strBez As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue)
			sValue = strBez
			If Not sValue.Contains("_{0}_{1}") Then sValue = "LO_{0}_{1}.PDF"

			Return sValue
		End Get

	End Property


End Class




Public Class ExportPayrollData
	Public Property ID As Integer
	Public Property LONr As Integer
	Public Property PayrollYear As Integer
	Public Property PayrollMonth As Integer
	Public Property MANr As Integer
	Public Property ExportFilename As String
	Public Property FileContent As Byte()


End Class
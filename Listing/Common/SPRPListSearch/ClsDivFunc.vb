
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.XtraEditors.Controls

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPRPListSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class ClsDivFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()


#Region "Diverses"

	'// Get4What._strModul4What
	Dim _strModul4What As String
	Public Property Get4What() As String
		Get
			Return _strModul4What
		End Get
		Set(ByVal value As String)
			_strModul4What = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strQuery As String
	Public Property GetSearchQuery() As String
		Get
			Return _strQuery
		End Get
		Set(ByVal value As String)
			_strQuery = value
		End Set
	End Property

#End Region


#Region "LL_Properties"
	'// Print.LLDocName
	Dim _LLDocName As String
	Public Property LLDocName() As String
		Get
			Return _LLDocName
		End Get
		Set(ByVal value As String)
			_LLDocName = value
		End Set
	End Property

	'// Print.LLDocLabel
	Dim _LLDocLabel As String
	Public Property LLDocLabel() As String
		Get
			Return _LLDocLabel
		End Get
		Set(ByVal value As String)
			_LLDocLabel = value
		End Set
	End Property

	'// Print.LLFontDesent
	Dim _LLFontDesent As Integer
	Public Property LLFontDesent() As Integer
		Get
			Return _LLFontDesent
		End Get
		Set(ByVal value As Integer)
			_LLFontDesent = value
		End Set
	End Property

	'// Print.LLIncPrv
	Dim _LLIncPrv As Integer
	Public Property LLIncPrv() As Integer
		Get
			Return _LLIncPrv
		End Get
		Set(ByVal value As Integer)
			_LLIncPrv = value
		End Set
	End Property

	'// Print.LLParamCheck
	Dim _LLParamCheck As Integer
	Public Property LLParamCheck() As Integer
		Get
			Return _LLParamCheck
		End Get
		Set(ByVal value As Integer)
			_LLParamCheck = value
		End Set
	End Property

	'// Print.LLKonvertName
	Dim _LLKonvertName As Integer
	Public Property LLKonvertName() As Integer
		Get
			Return _LLKonvertName
		End Get
		Set(ByVal value As Integer)
			_LLKonvertName = value
		End Set
	End Property

	'// Print.LLZoomProz
	Dim _LLZoomProz As Integer
	Public Property LLZoomProz() As Integer
		Get
			Return _LLZoomProz
		End Get
		Set(ByVal value As Integer)
			_LLZoomProz = value
		End Set
	End Property

	'// Print.LLCopyCount
	Dim _LLCopyCount As Integer
	Public Property LLCopyCount() As Integer
		Get
			Return _LLCopyCount
		End Get
		Set(ByVal value As Integer)
			_LLCopyCount = value
		End Set
	End Property

	'// Print.LLExportedFilePath
	Dim _LLExportedFilePath As String
	Public Property LLExportedFilePath() As String
		Get
			Return _LLExportedFilePath
		End Get
		Set(ByVal value As String)
			_LLExportedFilePath = value
		End Set
	End Property

	'// Print.LLExportedFileName
	Dim _LLExportedFileName As String
	Public Property LLExportedFileName() As String
		Get
			Return _LLExportedFileName
		End Get
		Set(ByVal value As String)
			_LLExportedFileName = value
		End Set
	End Property

	'// Print.LLExportfilter
	Dim _LLExportfilter As String
	Public Property LLExportfilter() As String
		Get
			Return _LLExportfilter
		End Get
		Set(ByVal value As String)
			_LLExportfilter = value
		End Set
	End Property

	'// Print.LLExporterName
	Dim _LLExporterName As String
	Public Property LLExporterName() As String
		Get
			Return _LLExporterName
		End Get
		Set(ByVal value As String)
			_LLExporterName = value
		End Set
	End Property

	'// Print.LLExporterFileName
	Dim _LLExporterFileName As String
	Public Property LLExporterFileName() As String
		Get
			Return _LLExporterFileName
		End Get
		Set(ByVal value As String)
			_LLExporterFileName = value
		End Set
	End Property

#End Region

#Region "US Setting"

	'// USeMail (= eMail des Personalvermittlers)
	Dim _USeMail As String
	Public Property USeMail() As String
		Get
			Return _USeMail
		End Get
		Set(ByVal value As String)
			_USeMail = value
		End Set
	End Property

	'// USTelefon (= USTelefon des Personalvermittlers)
	Dim _USTelefon As String
	Public Property USTelefon() As String
		Get
			Return _USTelefon
		End Get
		Set(ByVal value As String)
			_USTelefon = value
		End Set
	End Property

	'// USTelefax (= USTelefax des Personalvermittlers)
	Dim _USTelefax As String
	Public Property USTelefax() As String
		Get
			Return _USTelefax
		End Get
		Set(ByVal value As String)
			_USTelefax = value
		End Set
	End Property

	'// USVorname (= USVorname des Personalvermittlers)
	Dim _USVorname As String
	Public Property USVorname() As String
		Get
			Return _USVorname
		End Get
		Set(ByVal value As String)
			_USVorname = value
		End Set
	End Property

	'// USAnrede (= USAnrede des Personalvermittlers)
	Dim _USAnrede As String
	Public Property USAnrede() As String
		Get
			Return _USAnrede
		End Get
		Set(ByVal value As String)
			_USAnrede = value
		End Set
	End Property

	'// USNachname (= USNachname des Personalvermittlers)
	Dim _USNachname As String
	Public Property USNachname() As String
		Get
			Return _USNachname
		End Get
		Set(ByVal value As String)
			_USNachname = value
		End Set
	End Property

	'// USMDName (= MDName des Personalvermittlers)
	Dim _USMDname As String
	Public Property USMDname() As String
		Get
			Return _USMDname
		End Get
		Set(ByVal value As String)
			_USMDname = value
		End Set
	End Property

	'// MDName2 (= MDName2 des Personalvermittlers)
	Dim _USMDname2 As String
	Public Property USMDname2() As String
		Get
			Return _USMDname2
		End Get
		Set(ByVal value As String)
			_USMDname2 = value
		End Set
	End Property

	'// MDName3 (= MDName3 des Personalvermittlers)
	Dim _USMDname3 As String
	Public Property USMDname3() As String
		Get
			Return _USMDname3
		End Get
		Set(ByVal value As String)
			_USMDname3 = value
		End Set
	End Property

	'// USMDPostfach (= MDPostfach des Personalvermittlers)
	Dim _USMDPostfach As String
	Public Property USMDPostfach() As String
		Get
			Return _USMDPostfach
		End Get
		Set(ByVal value As String)
			_USMDPostfach = value
		End Set
	End Property

	'// USMDStrasse (= MDstrasse des Personalvermittlers)
	Dim _USMDStrasse As String
	Public Property USMDStrasse() As String
		Get
			Return _USMDStrasse
		End Get
		Set(ByVal value As String)
			_USMDStrasse = value
		End Set
	End Property

	'// USMDOrt (= MDOrt des Personalvermittlers)
	Dim _USMDOrt As String
	Public Property USMDOrt() As String
		Get
			Return _USMDOrt
		End Get
		Set(ByVal value As String)
			_USMDOrt = value
		End Set
	End Property

	'// USMDPLZ (= MDPLZ des Personalvermittlers)
	Dim _USMDPlz As String
	Public Property USMDPlz() As String
		Get
			Return _USMDPlz
		End Get
		Set(ByVal value As String)
			_USMDPlz = value
		End Set
	End Property

	'// USMDLand (= MDLand des Personalvermittlers)
	Dim _USMDLand As String
	Public Property USMDLand() As String
		Get
			Return _USMDLand
		End Get
		Set(ByVal value As String)
			_USMDLand = value
		End Set
	End Property

	'// USMDTelefon (= MDTelefon des Personalvermittlers)
	Dim _USMDTelefon As String
	Public Property USMDTelefon() As String
		Get
			Return _USMDTelefon
		End Get
		Set(ByVal value As String)
			_USMDTelefon = value
		End Set
	End Property

	'// USMDTelefax (= MDTelefax des Personalvermittlers)
	Dim _USMDTelefax As String
	Public Property USMDTelefax() As String
		Get
			Return _USMDTelefax
		End Get
		Set(ByVal value As String)
			_USMDTelefax = value
		End Set
	End Property

	'// USMDeMail (= MDeMail des Personalvermittlers)
	Dim _USMDeMail As String
	Public Property USMDeMail() As String
		Get
			Return _USMDeMail
		End Get
		Set(ByVal value As String)
			_USMDeMail = value
		End Set
	End Property

	'// USMDHomepage (= MDHomepage des Personalvermittlers)
	Dim _USMDHomepage As String
	Public Property USMDHomepage() As String
		Get
			Return _USMDHomepage
		End Get
		Set(ByVal value As String)
			_USMDHomepage = value
		End Set
	End Property

#End Region


End Class



Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsDataSaver As DbDataSaver

	Private m_md As Mandant
	Private m_utility As Utilities


#Region "Constructor"

	Public Sub New()

		m_utility = New Utilities
		m_md = New Mandant
		_ClsDataSaver = New DbDataSaver

	End Sub


#End Region

	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim result As List(Of MandantenData) = Nothing

		Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim recData As New MandantenData

					recData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					recData.MDName = m_utility.SafeGetString(reader, "MDName")
					recData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					recData.MDConnStr = m_md.GetSelectedMDData(recData.MDNr).MDDbConn

					result.Add(recData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function

	''' <summary>
	''' loads employee data
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadEmployeeData(ByVal firstMonth As Integer?, ByVal firstYear As Integer?) As IEnumerable(Of EmployeeData)
		Dim result As List(Of EmployeeData) = Nothing

		Dim sql As String

		sql = "SELECT MA.MANr, MA.Nachname, MA.Vorname, MA.PLZ, MA.Ort "
		sql &= "FROM Mitarbeiter MA "
		sql &= "Left Join RP On RP.MANr = MA.MANr "
		sql &= "Left Join ES On ES.MANr = MA.MANr "
		sql &= "Where RP.MDNr = @MDNr "
		sql &= "And RP.Monat = @firstMonth "
		sql &= "And RP.Jahr = @firstYear "
		sql &= "AND (ISNULL(ES.PrintNoRP, 0) = 0) "

		sql &= "Group By MA.MANr, MA.Nachname, MA.Vorname, MA.PLZ, MA.Ort "
		sql &= "ORDER BY MA.Nachname, MA.Vorname"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)

		listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("firstMonth", m_utility.ReplaceMissing(firstMonth, Now.Month)))
		listOfParams.Add(New SqlClient.SqlParameter("firstYear", m_utility.ReplaceMissing(firstYear, Now.Year)))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of EmployeeData)

				While reader.Read

					Dim employeeData = New EmployeeData()
					employeeData.EmployeeNumber = m_utility.SafeGetInteger(reader, "MANr", 0)
					employeeData.LastName = m_utility.SafeGetString(reader, "Nachname")
					employeeData.Firstname = m_utility.SafeGetString(reader, "Vorname")
					employeeData.Postcode = m_utility.SafeGetString(reader, "PLZ")
					employeeData.Location = m_utility.SafeGetString(reader, "Ort")

					result.Add(employeeData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function

	''' <summary>
	''' loads customer data
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadCustomerData(ByVal firstMonth As Integer?, ByVal firstYear As Integer?, ByVal employeeNumber As Integer?) As IEnumerable(Of CustomerData)
		Dim result As List(Of CustomerData) = Nothing

		Dim sql As String

		sql = "SELECT KD.KDNr, KD.Firma1, KD.Strasse, KD.PLZ, KD.Ort "
		sql &= "FROM Kunden KD "
		sql &= "Left Join RP On RP.KDNr = KD.KDNr "
		sql &= "Left Join ES On ES.KDNr = KD.KDNr And ES.ESNr = RP.ESNr "
		sql &= "Where RP.MDNr = @MDNr "
		sql &= "And RP.Monat = @firstMonth "
		sql &= "And RP.Jahr = @firstYear "
		'sql &= "And (@employeeNumber = 0 Or RP.MANr = @employeeNumber) "
		sql &= "AND (ISNULL(ES.PrintNoRP, 0) = 0) "

		sql &= "Group By KD.KDNr, KD.Firma1, KD.Strasse, KD.PLZ, KD.Ort "
		sql &= "ORDER BY KD.Firma1"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)

		listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("firstMonth", m_utility.ReplaceMissing(firstMonth, Now.Month)))
		listOfParams.Add(New SqlClient.SqlParameter("firstYear", m_utility.ReplaceMissing(firstYear, Now.Year)))
		'listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", m_utility.ReplaceMissing(employeeNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of CustomerData)

				While reader.Read

					Dim customerData = New CustomerData()
					customerData.CustomerNumber = m_utility.SafeGetInteger(reader, "KDNr", 0)
					customerData.Company1 = m_utility.SafeGetString(reader, "Firma1")
					customerData.Street = m_utility.SafeGetString(reader, "Strasse")
					customerData.Postcode = m_utility.SafeGetString(reader, "PLZ")
					customerData.Location = m_utility.SafeGetString(reader, "Ort")

					result.Add(customerData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function

	Function LoadEmploymentData(ByVal firstMonth As Integer?, ByVal firstYear As Integer?, ByVal employeeNumber As Integer?, ByVal customerNumber As Integer?) As IEnumerable(Of employmentData)
		Dim result As List(Of employmentData) = Nothing

		Dim sql As String

		sql = "SELECT RP.ESNr, RP.MANr, RP.KDNr, KD.Firma1, MA.Vorname, MA.Nachname, ES.ES_Ab, ES.ES_Ende, ES.ES_Als "
		sql &= "FROM RP "
		sql &= "LEFT Join dbo.ES On ES.ESNr = RP.ESNr AND ES.MDNr = @MDNr "
		sql &= "LEFT Join dbo.Mitarbeiter MA On MA.MANr = RP.MANr "
		sql &= "LEFT Join dbo.Kunden KD On KD.KDNr = RP.KDNr "
		sql &= "WHERE RP.MDNr = @MDNr "
		sql &= "AND (ISNULL(@firstMonth, 0) = 0 OR RP.Monat = @firstMonth) "
		sql &= "AND (ISNULL(@firstYear, 0) = 0 OR Convert(Int, RP.Jahr) = @firstYear) "
		sql &= "AND (ISNULL(@employeeNumber, 0) = 0 Or RP.MANr = @employeeNumber) "
		sql &= "AND (ISNULL(@customerNumber, 0) = 0 Or RP.KDNr = @customerNumber) "
		sql &= "AND (ISNULL(ES.PrintNoRP, 0) = 0) "

		sql &= "GROUP By RP.ESNr, RP.MANr, RP.KDNr, KD.Firma1, MA.Vorname, MA.Nachname, ES.ES_Ab, ES.ES_Ende, ES.ES_Als "
		sql &= "ORDER BY MA.Nachname, MA.Vorname, ES.ES_Ab"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)

		listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("firstMonth", m_utility.ReplaceMissing(firstMonth, Now.Month)))
		listOfParams.Add(New SqlClient.SqlParameter("firstYear", m_utility.ReplaceMissing(firstYear, Now.Year)))
		listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", m_utility.ReplaceMissing(employeeNumber, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("customerNumber", m_utility.ReplaceMissing(customerNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of employmentData)

				While reader.Read

					Dim customerData = New employmentData()
					customerData.EmploymentNumber = m_utility.SafeGetInteger(reader, "ESNr", 0)
					customerData.EmployeeNumber = m_utility.SafeGetInteger(reader, "MANr", 0)
					customerData.CustomerNumber = m_utility.SafeGetInteger(reader, "KDNr", 0)
					customerData.Company1 = m_utility.SafeGetString(reader, "Firma1")
					customerData.EmployeeFirstname = m_utility.SafeGetString(reader, "Vorname")
					customerData.EmployeeLasstname = m_utility.SafeGetString(reader, "Nachname")

					customerData.EmploymentAs = m_utility.SafeGetString(reader, "ES_Als")
					customerData.EmploymentFrom = m_utility.SafeGetDateTime(reader, "ES_Ab", Nothing)
					customerData.EmploymentTo = m_utility.SafeGetDateTime(reader, "ES_Ende", Nothing)

					result.Add(customerData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function

	Function LoadReportData(ByVal firstMonth As Integer?, ByVal firstYear As Integer?, ByVal employeeNumber As Integer?, ByVal customerNumber As Integer?, ByVal employmentNumber As Integer?) As IEnumerable(Of ReportData)
		Dim result As List(Of ReportData) = Nothing

		Dim sql As String

		sql = "SELECT RP.RPNR, RP.KDNr, RP.MANr, RP.ESNr, KD.Firma1, MA.Vorname, MA.Nachname, RP.Von, RP.Bis, ES.ES_Als "
		sql &= "FROM RP "
		sql &= "LEFT Join dbo.ES On ES.ESNr = RP.ESNr AND ES.MDNr = @MDNr "
		sql &= "LEFT Join dbo.Mitarbeiter MA On MA.MANr = RP.MANr "
		sql &= "LEFT Join dbo.Kunden KD On KD.KDNr = RP.KDNr "
		sql &= "WHERE RP.MDNr = @MDNr "
		sql &= "AND (ISNULL(@firstMonth, 0) = 0 OR RP.Monat = @firstMonth) "
		sql &= "AND (ISNULL(@firstYear, 0) = 0 OR RP.Jahr = @firstYear) "
		sql &= "AND (ISNULL(@employeeNumber, 0) = 0 Or RP.MANr = @employeeNumber) "
		sql &= "AND (ISNULL(@customerNumber, 0) = 0 Or RP.KDNr = @customerNumber) "
		sql &= "AND (ISNULL(@employmentNumber, 0) = 0 Or RP.ESNr = @employmentNumber) "
		sql &= "AND (ISNULL(ES.PrintNoRP, 0) = 0) "

		sql &= "GROUP By RP.RPNR, RP.KDNr, RP.MANr, RP.ESNr, KD.Firma1, MA.Vorname, MA.Nachname, RP.Von, RP.Bis, ES.ES_Als "
		sql &= "ORDER BY MA.Nachname, MA.Vorname, RP.Von"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)

		listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("firstMonth", m_utility.ReplaceMissing(firstMonth, Now.Month)))
		listOfParams.Add(New SqlClient.SqlParameter("firstYear", m_utility.ReplaceMissing(firstYear, Now.Year)))
		listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", m_utility.ReplaceMissing(employeeNumber, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("customerNumber", m_utility.ReplaceMissing(customerNumber, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("employmentNumber", m_utility.ReplaceMissing(employmentNumber, 0)))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of ReportData)

				While reader.Read

					Dim customerData = New ReportData()
					customerData.ReportNumber = m_utility.SafeGetInteger(reader, "RPNr", 0)
					customerData.EmploymentNumber = m_utility.SafeGetInteger(reader, "ESNr", 0)
					customerData.EmployeeNumber = m_utility.SafeGetInteger(reader, "MANr", 0)
					customerData.CustomerNumber = m_utility.SafeGetInteger(reader, "KDNr", 0)

					customerData.Company1 = m_utility.SafeGetString(reader, "Firma1")
					customerData.EmployeeFirstname = m_utility.SafeGetString(reader, "Vorname")
					customerData.EmployeeLasstname = m_utility.SafeGetString(reader, "Nachname")

					customerData.EmploymentAs = m_utility.SafeGetString(reader, "ES_Als")
					customerData.ReportFrom = m_utility.SafeGetDateTime(reader, "Von", Nothing)
					customerData.ReportTo = m_utility.SafeGetDateTime(reader, "Bis", Nothing)

					result.Add(customerData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function


#Region "Funktionen zur Suche nach Daten..."

	Function GetStartSQLString() As String
		Dim sSql As String

		sSql = "Select RP.RPNr, RP.Monat, RP.Jahr, RP.Von, RP.Bis "
		sSql &= "From dbo.RP "
		sSql &= "Left Join dbo.ES On RP.ESNr = ES.ESNr "

		Return sSql
	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String, ByVal searchData As SearchCriteria) As String
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery
		Dim strFieldName As String = String.Empty
		Dim m_common As New CommonSetting

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = m_InitialData.UserData.UserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)

		Dim strName As String()
		Dim strMyName As String = String.Empty

		' Monat -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.Monat"
		If searchData.FromMonth > 0 Then
			sZusatzBez = searchData.FromMonth
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Monat wie ({0}){1}"), sZusatzBez, vbLf)

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
		End If

		' Jahr -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.Jahr"
		If searchData.FromYear > 0 Then
			sZusatzBez = searchData.FromYear
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Jahr wie ({0}){1}"), sZusatzBez, vbLf)

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
		End If

		' employee -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.MANr"
		If searchData.EmployeeNumber.GetValueOrDefault(0) > 0 Then
			sZusatzBez = searchData.EmployeeNumber
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Kaniddatennummer wie ({0}){1}"), sZusatzBez, vbLf)

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
		End If

		' customer -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.KDNr"
		If searchData.CustomerNumber.GetValueOrDefault(0) > 0 Then
			sZusatzBez = searchData.CustomerNumber
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Kundennummer wie ({0}){1}"), sZusatzBez, vbLf)

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
		End If

		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.ESNr"
		If searchData.Employmentnumber.GetValueOrDefault(0) > 0 Then
			sZusatzBez = searchData.Employmentnumber
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Einsatznummer wie ({0}){1}"), sZusatzBez, vbLf)

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
		End If

		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.RPNr"
		If searchData.reportnumber.GetValueOrDefault(0) > 0 Then
			sZusatzBez = searchData.reportnumber
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Rapportnummer wie ({0}){1}"), sZusatzBez, vbLf)

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
		End If

		' 1. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.RPKst1"
		If Not String.IsNullOrWhiteSpace(searchData.kst1) Then
			sZusatzBez = searchData.kst1
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("{0} wie {1}{2}"), searchData.Kst1Label, sZusatzBez, vbLf)
			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
			'End If

		End If

		' 2. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.RPKst2"
		If Not String.IsNullOrWhiteSpace(searchData.kst2) Then
			sZusatzBez = searchData.kst1
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("{0} wie {1}{2}"), searchData.Kst1Label, sZusatzBez, vbLf)
			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then
					sZusatzBez = Replace(sZusatzBez, ",", "','")

				End If
				sSql += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
			'End If

		End If

		' Berater -------------------------------------------------------------------------------------------------------
		'Dim cv As ComboValue

		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		If Not String.IsNullOrWhiteSpace(searchData.Berater) Then
			Dim KstsSql As String = String.Empty
			Dim i As Integer = 0
			For Each itm In searchData.Berater.Split(",")
				Dim strKst As String = itm.Trim.Replace("'", "").Replace("*", "%")

				KstsSql += String.Format("{0}(RP.RPKst  = '{1}' Or ", If(i > 0, " OR ", String.Empty), strKst)
				KstsSql += String.Format("RP.RPKst  Like '{0}/%' Or ", strKst)
				KstsSql += String.Format("RP.RPKst  Like '%/{0}')", strKst)


				i += 1
			Next
			FilterBez += String.Format("Rapporte mit Berater wie ({0}){1}", searchData.Berater.Replace("#", ","), vbLf)

			sSql += String.Format("{0} ( {1} ) ", strAndString, KstsSql)
		End If


		' Filiale -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "RP.RPKst"
		If Not String.IsNullOrWhiteSpace(searchData.filiale) Then
			sZusatzBez = searchData.filiale.Trim
			FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Filiale wie ({0}){1}"), sZusatzBez, vbLf)

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				sZusatzBez = GetFilialKstData(sZusatzBez)
				If sZusatzBez.Length > 1 Then sZusatzBez = sZusatzBez.Replace("'", "")
				Dim aKST As String() = sZusatzBez.Split(CChar(","))
				Dim strKstQuery As String = String.Empty
				For i As Integer = 0 To aKST.Length - 1
					strKstQuery += String.Format("{0} (RP.RPKst  = '{1}' Or ", If(i > 0, "Or", ""), aKST(i))
					strKstQuery += String.Format("RP.RPKst  Like '{0}/%' Or ", aKST(i))
					strKstQuery += String.Format("RP.RPKst  Like '%/{0}') ", aKST(i))
				Next
				If strKstQuery.Length > 2 Then
					sSql &= String.Format("{0}({1})", strAndString, strKstQuery)
				End If

			End If

		End If

		' Filialen Teilung...
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		If strUSFiliale <> "" Then
			strFieldName = "RP.RPKst"
			If UCase(strUSFiliale) <> String.Empty Then
				sZusatzBez = strUSFiliale
				FilterBez += String.Format(m_Translate.GetSafeTranslationValue("Filiale wie ({0}){1}"), sZusatzBez, vbLf)

				sZusatzBez = GetFilialKstData(sZusatzBez)
				sZusatzBez = Replace(sZusatzBez, "'", "")
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				sSql += strAndString & " (" & sZusatzBez & ")"

			End If
		End If

    'Naas entfernt   --  on 27.08.2018 
    '  ' Nicht Erfasste Rapporte
    '  strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
    '  sSql += String.Format("{0}RP.Erfasst <> 1 ", strAndString)
    ' Nur wenn Flag Keine Rapportdrucken deaktiviert ist
    'Naas Add 'ES.PrintNoRP' -- 05.09.2018
    strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
    sSql += String.Format("{0}ES.PrintNoRP <> 1 ", strAndString)


    ' Mandantennummer -------------------------------------------------------------------------------------------------------
    If m_InitialData.MDData.MultiMD = 1 Then
      strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
      sSql += String.Format("{0}RP.MDNr = {1}", strAndString, m_InitialData.MDData.MDNr)
    End If

    ClsDataDetail.GetFilterBez = FilterBez

		m_Logger.LogInfo(String.Format("RPListSearchWhereQuery: {0}", sSql))

		Return sSql
	End Function

	Function GetStartSQLString_2() As String
		Dim sSql As String = String.Empty

		sSql = "Select RP.RPNr, RP.MANr, RP.ESNr, RP.KDNr, RP.Monat, RP.Jahr, RP.Result, RP.PrintedWeeks, "
    sSql += "RP.Von, RP.Bis, RP.MDNr, MA.Nachname As MANachname, MA.Vorname As MAVorname, "
    'Naas Add 'ES.PrintNoRP' -- 05.09.2018
    sSql += "KD.Firma1, KD.Ort As KSOrt, ES.ES_Ab, ES.ES_Ende, ES.ES_Als, ES.PrintNoRP, "
    sSql += "RPDayDb.Day1, RPDayDb.Day2, RPDayDb.Day3, RPDayDb.Day4, RPDayDb.Day5, "
		sSql += "RPDayDb.Day6, RPDayDb.Day7, RPDayDb.Day8, RPDayDb.Day9, RPDayDb.Day10, "
		sSql += "RPDayDb.Day11, RPDayDb.Day12, RPDayDb.Day13, RPDayDb.Day14, RPDayDb.Day15, "
		sSql += "RPDayDb.Day16, RPDayDb.Day17, RPDayDb.Day18, RPDayDb.Day19, RPDayDb.Day20,  "
		sSql += "RPDayDb.Day21, RPDayDb.Day22, RPDayDb.Day23, RPDayDb.Day24, RPDayDb.Day25, "
		sSql += "RPDayDb.Day26, RPDayDb.Day27, RPDayDb.Day28, RPDayDb.Day29, RPDayDb.Day30, "
		sSql += "RPDayDb.Day31, RPDayDb.WeekNr "
		sSql += "From RPDayDb Left Join RP On RP.RPNr = RPDayDb.RPNr	"
		sSql += "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
		sSql += "Left Join Kunden KD On RP.KDNr = KD.KDNr "
		sSql += "Left Join ES On RP.ESNr = ES.ESNr Where RPDayDb.USNr = "

		sSql += CStr(m_InitialData.UserData.UserNr)

		Return sSql
	End Function

	Function GetSortString(ByVal searchData As SearchCriteria) As String
		Dim strSort As String = " Order by "
		Dim strSortBez As String = String.Empty
		'Dim strName As String()
		Dim strMyName As String = String.Empty

		'0 - Rapportnummer
		'1 - Rapportdatum
		'2 - Kandidatennummer
		'3 - Kandidatenname
		'4 - Kundennummer
		'5 - Kundenname

		'With frmTest

		strMyName = "MA.Nachname ASC, MA.Vorname ASC, RP.RPNr ASC"
		strSortBez = "Kandidatenname"

		'strName = Regex.Split(searchData.SortIn.Trim, ",")
		'strMyName = String.Empty
		'For i As Integer = 0 To strName.Length - 1

		'	Select Case CInt(Val(Left(strName(i).ToString.Trim, 1)))
		'		Case 0      ' Nach Rapportnummer
		'			strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "RP.RPNr"
		'			strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Rapportnummer"

		'		Case 1      ' Von / Bis
		'			strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "RP.Von ASC, RP.Bis ASC"
		'			strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Rapportdatum"

		'		Case 2      ' Kandidatennummer
		'			strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "RP.MANr"
		'			strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatennummer"

		'		Case 3      ' Kandidatenname
		'			strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "MA.Nachname ASC, MA.Vorname ASC"
		'			strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname"

		'		Case 4      ' Kundennummer
		'			strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "KD.KDNr ASC"
		'			strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundennummer"

		'		Case 5      ' Kundenname
		'			strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "KD.Firma1 ASC"
		'			strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kundenname"


		'		Case Else      ' Auszahlungdatum (Absteigend)
		'			strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & "MA.Nachname ASC, MA.Vorname ASC"
		'			strSortBez += CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname"


		'	End Select

		'Next
		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		'End With

		Return strSort

	End Function

	Sub GetJobNr4Print(ByVal frmSource As frmRPListSearch)
		Dim strModul2print As String = String.Empty

		With frmSource
			strModul2print = "10.4"  ' Druck für Rapport

		End With
		ClsDataDetail.GetModulToPrint = strModul2print

	End Sub


#End Region

#Region "Sonstige Funktionen für Datenbank..."


	Function BuildRPDayDb(ByVal strQuery As String) As Boolean
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim sSql As String = "Delete RPDayDb Where USNr = @USNr"
		Dim i As Integer = 0
		Dim strRPMonthYear As String = String.Empty
		Dim iFWeek As Integer = 0
		Dim iLWeek As Integer = 0

		Try
			Dim Time_1 As Double = System.Environment.TickCount
			Conn.Open()


			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@USNr", m_InitialData.UserData.UserNr)
			cmd.ExecuteNonQuery()     ' Datenbank geleert...

			cmd.Dispose()
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim rRPrec As SqlDataReader = cmd.ExecuteReader
			Trace.Listeners.Clear()

			If rRPrec.HasRows() Then
				While rRPrec.Read
					' Fehlcodes und Tages-Stunden in Var's speichern...
					GetRPDayRec(CInt(rRPrec("RPNr").ToString))

					' Datensatz hinzufügen...
					GetInsertString(CInt(rRPrec("RPNr").ToString),
													CDate(rRPrec("Von").ToString),
													CDate(rRPrec("Bis").ToString))

				End While
			End If

			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für BuildRPDayDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


		Catch e As Exception
			MsgBox(e.Message)
			Return False

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return True
	End Function

	Sub GetInsertString(ByVal iRPNr As Integer, ByVal dRPVon As Date, ByVal dRPBis As Date)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iNewWeek As Integer = 0
		Dim iOldWeek As Integer = 0
		Dim StrWeekNr As String = String.Empty
		Dim iFWeek As Integer = DatePart(DateInterval.WeekOfYear, dRPVon,
																		FirstDayOfWeek.System, FirstWeekOfYear.System)
		Dim iLWeek As Integer = DatePart(DateInterval.WeekOfYear, dRPBis,
																		FirstDayOfWeek.System, FirstWeekOfYear.System)
		Dim i As Integer = 0

		Dim strResult As String = "Insert Into RPDayDb (RPNr, USNr, Day1, Day2, Day3, Day4, Day5, Day6, "
		strResult += "Day7, Day8, Day9, Day10, Day11, Day12, "
		strResult += "Day13, Day14, Day15, Day16, Day17, Day18, Day19, Day20, "
		strResult += "Day21, Day22, Day23, Day24, Day25, Day26, Day27, Day28, Day29, "
		strResult += "Day30, Day31, WeekNr) Values (@RPNr, @USNr, @Day1, @Day2, @Day3, @Day4, @Day5, @Day6, @Day7, "
		strResult += "@Day8, @Day9, @Day10, @Day11, @Day12, @Day13, @Day14, @Day15, @Day16, @Day17, @Day18, "
		strResult += "@Day19, @Day20, @Day21, @Day22, @Day23, @Day24, @Day25, @Day26, @Day27, @Day28, @Day29, "
		strResult += "@Day30, @Day31, @WeekNr) "


		Try
			Dim Time_1 As Double = System.Environment.TickCount
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strResult, Conn)

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)
			param = cmd.Parameters.AddWithValue("@USNr", CInt(m_InitialData.UserData.UserNr))

			For i = 1 To 31
				If _ClsDataSaver.GetRPFehlCode(0) = "1" Then

					If DatePart(DateInterval.Day, dRPVon,
																		FirstDayOfWeek.System, FirstWeekOfYear.System) <= i And DatePart(DateInterval.Day, dRPBis,
																		FirstDayOfWeek.System, FirstWeekOfYear.System) >= i Then
						If _ClsDataSaver.GetRPFehlCode(i) = "" Then
							If CDec(_ClsDataSaver.GetRPDays(i)) <> 0 Then
								'Trace.WriteLine("@Day" & CStr(i) & vbTab & _ClsDataSaver.GetRPDays(i))
								param = cmd.Parameters.AddWithValue("@Day" & CStr(i), CDec(_ClsDataSaver.GetRPDays(i)))
							Else
								'Trace.WriteLine("@Day" & CStr(i) & vbTab & "-1")
								param = cmd.Parameters.AddWithValue("@Day" & CStr(i), CDbl("-1"))

								iNewWeek = DatePart(DateInterval.WeekOfYear, CDate(i & "." & CStr(Month(dRPVon)) & "." & Str(Year(dRPVon))),
																		FirstDayOfWeek.System, FirstWeekOfYear.System)
								If iNewWeek = iOldWeek Then
									'          nichts tun!!!
								Else
									StrWeekNr += CStr(IIf(StrWeekNr = "", "", " / ")) & CStr(iNewWeek)
									iOldWeek = iNewWeek
								End If

							End If
						Else
							'Trace.WriteLine("@Day" & CStr(i) & vbTab & "-2")
							param = cmd.Parameters.AddWithValue("@Day" & CStr(i), CDbl("-2"))
						End If
					Else
						'Trace.WriteLine("@Day" & CStr(i) & vbTab & "-10")
						param = cmd.Parameters.AddWithValue("@Day" & CStr(i), CDbl("-10"))
					End If

				Else
					'Trace.WriteLine("@Day" & CStr(i) & vbTab & "-1")
					param = cmd.Parameters.AddWithValue("@Day" & CStr(i), CDbl("-1"))
				End If
			Next
			StrWeekNr = CStr(IIf(_ClsDataSaver.GetRPFehlCode(0) = "0", CStr(iFWeek) & " bis " & CStr(iLWeek), StrWeekNr))
			param = cmd.Parameters.AddWithValue("@WeekNr", StrWeekNr)
			'For i = 0 To cmd.Parameters.Count - 1
			'  Trace.WriteLine(cmd.Parameters(i).ParameterName & vbTab & ": " & CStr(cmd.Parameters(i).Value))
			'Next

			cmd.ExecuteNonQuery()     ' Datensatz hinzufügen...
			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für GetInsertString: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


		Catch e As Exception
			MsgBox(i & ": " & e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	''' <summary>
	''' fügt die Array vom DbDataSaver zu
	''' _ClsDataSaver.GetRPDays(0) = -1  => Datensätze sind vorhanden
	''' _ClsDataSaver.GetRPDays(0) = -2  => KEINEN Datensatz gefunden
	''' 
	''' _ClsDataSaver.GetRPFehlCode(0) = "1"  => Datensätze sind vorhanden
	''' _ClsDataSaver.GetRPFehlCode(0) = "0"  => KEINEN Datensatz gefunden
	''' </summary>
	''' <param name="iRPNr">Die Rapportnummer</param>
	''' <remarks></remarks>
	Sub GetRPDayRec(ByVal iRPNr As Integer)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim sSqlStd As String = "[Get RPL TotalStdDay For Print In List]"
		Dim sSqlFehlCode As String = "[Get RP FehlCode For Print In List]"
		Dim i As Integer = 0

		Try

			Dim Time_1 As Double = System.Environment.TickCount
			Conn.Open()
			_ClsDataSaver = New DbDataSaver

			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(sSqlStd, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)
			Dim rRPDayrec As SqlDataReader = cmd.ExecuteReader ' Datenbank für Tage...

			If rRPDayrec.HasRows() Then
				_ClsDataSaver.GetRPDays(0) = -1
				rRPDayrec.Read()
				Try
					For i = 1 To 31
						_ClsDataSaver.GetRPDays(i) = CDec(IIf(IsDBNull(rRPDayrec("Day" & CStr(i))), 0, rRPDayrec("Day" & CStr(i)).ToString))
					Next

				Catch ex As Exception
					MsgBox(i & ": " & ex.Message)
				End Try

			Else
				_ClsDataSaver.GetRPDays(0) = -2

			End If
			rRPDayrec.Close()
			cmd.Dispose()

			cmd = New System.Data.SqlClient.SqlCommand(sSqlFehlCode, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)
			rRPDayrec = cmd.ExecuteReader ' Datenbank für Tage...

			If rRPDayrec.HasRows() Then
				_ClsDataSaver.GetRPFehlCode(0) = "1"
				rRPDayrec.Read()
				For i = 1 To 31
					_ClsDataSaver.GetRPFehlCode(i) = CStr(IIf(IsDBNull(rRPDayrec("Fehltag" & CStr(i))), "", rRPDayrec("Fehltag" & CStr(i)).ToString))
				Next

			Else
				_ClsDataSaver.GetRPFehlCode(0) = "0"

			End If

			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für GetRPDayRec: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


		Catch e As Exception
			MsgBox(i & ": " & e.Message)


		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub


#End Region


	Function GetLstItems(ByVal lst As ListBox) As String
		Dim strBerufItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strBerufItems += lst.Items(i).ToString & ","
		Next

		Return Left(strBerufItems, Len(strBerufItems) - 1)
	End Function


End Class


Class DbDataSaver

	Public Sub New()

		For i As Integer = 0 To 31
			Me.GetRPDays(i) = 0
			Me.GetRPFehlCode(i) = String.Empty
		Next

	End Sub

	'// RPDays
	Dim _Days(32) As Decimal
	Property GetRPDays() As Decimal()
		Get
			Return _Days
		End Get
		Set(ByVal value As Decimal())
			_Days = value
		End Set
	End Property

	'// RPFehlCode
	Dim _DayFehlCode(32) As String
	Property GetRPFehlCode() As String()
		Get
			Return _DayFehlCode
		End Get
		Set(ByVal value As String())
			_DayFehlCode = value
		End Set
	End Property

	'// Day1
	Dim _Day1 As String
	Property GetDay1() As String
		Get
			Return _Day1
		End Get
		Set(ByVal value As String)
			_Day1 = value
		End Set
	End Property

	'// Day2
	Dim _Day2 As String
	Property GetDay2() As String
		Get
			Return _Day2
		End Get
		Set(ByVal value As String)
			_Day2 = value
		End Set
	End Property

	'// Day3
	Dim _Day3 As String
	Property GetDay3() As String
		Get
			Return _Day3
		End Get
		Set(ByVal value As String)
			_Day3 = value
		End Set
	End Property

	'// Day4
	Dim _Day4 As String
	Property GetDay4() As String
		Get
			Return _Day4
		End Get
		Set(ByVal value As String)
			_Day4 = value
		End Set
	End Property

	'// Day5
	Dim _Day5 As String
	Property GetDay5() As String
		Get
			Return _Day5
		End Get
		Set(ByVal value As String)
			_Day5 = value
		End Set
	End Property

	'// Day6
	Dim _Day6 As String
	Property GetDay6() As String
		Get
			Return _Day6
		End Get
		Set(ByVal value As String)
			_Day6 = value
		End Set
	End Property

	'// Day7
	Dim _Day7 As String
	Property GetDay7() As String
		Get
			Return _Day7
		End Get
		Set(ByVal value As String)
			_Day7 = value
		End Set
	End Property

	'// Day8
	Dim _Day8 As String
	Property GetDay8() As String
		Get
			Return _Day8
		End Get
		Set(ByVal value As String)
			_Day8 = value
		End Set
	End Property

	'// Day9
	Dim _Day9 As String
	Property GetDay9() As String
		Get
			Return _Day9
		End Get
		Set(ByVal value As String)
			_Day9 = value
		End Set
	End Property

	'// Day10
	Dim _Day10 As String
	Property GetDay10() As String
		Get
			Return _Day10
		End Get
		Set(ByVal value As String)
			_Day10 = value
		End Set
	End Property

	'// Day11
	Dim _Day11 As String
	Property GetDay11() As String
		Get
			Return _Day11
		End Get
		Set(ByVal value As String)
			_Day11 = value
		End Set
	End Property

	'// Day12
	Dim _Day12 As String
	Property GetDay12() As String
		Get
			Return _Day12
		End Get
		Set(ByVal value As String)
			_Day12 = value
		End Set
	End Property

	'// Day13
	Dim _Day13 As String
	Property GetDay13() As String
		Get
			Return _Day13
		End Get
		Set(ByVal value As String)
			_Day13 = value
		End Set
	End Property

	'// Day14
	Dim _Day14 As String
	Property GetDay14() As String
		Get
			Return _Day14
		End Get
		Set(ByVal value As String)
			_Day14 = value
		End Set
	End Property

	'// Day15
	Dim _Day15 As String
	Property GetDay15() As String
		Get
			Return _Day15
		End Get
		Set(ByVal value As String)
			_Day15 = value
		End Set
	End Property

	'// Day16
	Dim _Day16 As String
	Property GetDay16() As String
		Get
			Return _Day16
		End Get
		Set(ByVal value As String)
			_Day16 = value
		End Set
	End Property

	'// Day17
	Dim _Day17 As String
	Property GetDay17() As String
		Get
			Return _Day17
		End Get
		Set(ByVal value As String)
			_Day17 = value
		End Set
	End Property

	'// Day18
	Dim _Day18 As String
	Property GetDay18() As String
		Get
			Return _Day18
		End Get
		Set(ByVal value As String)
			_Day18 = value
		End Set
	End Property

	'// Day19
	Dim _Day19 As String
	Property GetDay19() As String
		Get
			Return _Day19
		End Get
		Set(ByVal value As String)
			_Day19 = value
		End Set
	End Property

	'// Day20
	Dim _Day20 As String
	Property GetDay20() As String
		Get
			Return _Day20
		End Get
		Set(ByVal value As String)
			_Day20 = value
		End Set
	End Property

	'// Day21
	Dim _Day21 As String
	Property GetDay21() As String
		Get
			Return _Day21
		End Get
		Set(ByVal value As String)
			_Day21 = value
		End Set
	End Property

	'// Day22
	Dim _Day22 As String
	Property GetDay22() As String
		Get
			Return _Day22
		End Get
		Set(ByVal value As String)
			_Day22 = value
		End Set
	End Property

	'// Day23
	Dim _Day23 As String
	Property GetDay23() As String
		Get
			Return _Day23
		End Get
		Set(ByVal value As String)
			_Day23 = value
		End Set
	End Property

	'// Day24
	Dim _Day24 As String
	Property GetDay24() As String
		Get
			Return _Day24
		End Get
		Set(ByVal value As String)
			_Day24 = value
		End Set
	End Property

	'// Day25
	Dim _Day25 As String
	Property GetDay25() As String
		Get
			Return _Day25
		End Get
		Set(ByVal value As String)
			_Day25 = value
		End Set
	End Property

	'// Day26
	Dim _Day26 As String
	Property GetDay26() As String
		Get
			Return _Day26
		End Get
		Set(ByVal value As String)
			_Day26 = value
		End Set
	End Property

	'// Day27
	Dim _Day27 As String
	Property GetDay27() As String
		Get
			Return _Day27
		End Get
		Set(ByVal value As String)
			_Day27 = value
		End Set
	End Property

	'// Day28
	Dim _Day28 As String
	Property GetDay28() As String
		Get
			Return _Day28
		End Get
		Set(ByVal value As String)
			_Day28 = value
		End Set
	End Property

	'// Day29
	Dim _Day29 As String
	Property GetDay29() As String
		Get
			Return _Day29
		End Get
		Set(ByVal value As String)
			_Day29 = value
		End Set
	End Property

	'// Day30
	Dim _Day30 As String
	Property GetDay30() As String
		Get
			Return _Day30
		End Get
		Set(ByVal value As String)
			_Day30 = value
		End Set
	End Property

	'// Day31
	Dim _Day31 As String
	Property GetDay31() As String
		Get
			Return _Day31
		End Get
		Set(ByVal value As String)
			_Day31 = value
		End Set
	End Property


End Class

Imports System.IO
Imports System.Data.SqlClient
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects


Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.DateAndTimeCalculation

Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.SalaryValueCalculation

Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Imports System.Windows.Forms
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports SPS.MA.Lohn.Utility.ModulConstants
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ES
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.PayrollMng.DataObjects

Public Class PayrollUtility


#Region "private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_ESDatabaseAccess As IESDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_SalaryValueDatabaseAccess As ISalaryValueCalculationDatabaseAccess
	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_connectionString As String

	''' <summary>
	''' The invoice data access object.
	''' </summary>
	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility
	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	''' <summary>
	''' Settings xml.
	''' </summary>
	Private m_MandantSettingsXml As SettingsXml
	Private m_SonstigesSetting As String
	Private m_AHVSetting As String

#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
	Private Const MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING As String = "MD_{0}/AHV-Daten"

#End Region


#Region "Constructor"

	Public Sub New(ByVal mdNr As Integer)

		m_InitializationData = CreateInitialData(mdNr, 0)

		m_mandant = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_path = New SPProgUtility.ProgPath.ClsProgPath

		m_connectionString = m_InitializationData.MDData.MDDbConn
		'm_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		'm_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		'm_ESDatabaseAccess = New ESDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		'm_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_SalaryValueDatabaseAccess = New SalaryValueCalculationDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_PayrollDatabaseAccess = New PayrollDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
		m_AHVSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_AHV_SETTING, m_InitializationData.MDData.MDNr)
		m_MandantSettingsXml = New SettingsXml(m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))


	End Sub

#End Region


#Region "public methodes"

	Public Function LoadMandantPercentages(ByVal dMAGebDat As Date, ByVal strGeschlecht As String, ByVal iYear As Integer) As MandantAnsatzData
		Dim result As MandantAnsatzData = Nothing
		result = m_PayrollDatabaseAccess.LoadMandantAnsatzData(m_InitializationData.MDData.MDNr, m_InitializationData.MDData.MDYear)

		If result Is Nothing Then
			m_Logger.LogError("Die Mandanten-Prozentsätze können nicht geladen werden!")
		End If


		Return result

	End Function

	Public Function LoadBVGProcentage(ByVal dMAGebDat As Date, ByVal strGeschlecht As String) As Decimal

		Dim cResult As Decimal
		Dim ProzentSatz As Decimal?
		Dim iAlter As Integer

		iAlter = AgeYear(Int(Now.Year), dMAGebDat)

		If strGeschlecht = "M" Then
			ProzentSatz = m_PayrollDatabaseAccess.LoadBVGAnsMForPayroll(iAlter, m_InitializationData.MDData.MDYear, m_InitializationData.MDData.MDNr)
		Else
			ProzentSatz = m_PayrollDatabaseAccess.LoadBVGAnsWForPayroll(iAlter, m_InitializationData.MDData.MDYear, m_InitializationData.MDData.MDNr)
		End If

		If Not ProzentSatz.HasValue Then
			m_Logger.LogError(String.Format("BVG-Prozentsatz wurde nicht gefunden: Alter: {0}", iAlter))

			cResult = 0
		Else
			cResult = ProzentSatz.Value

		End If


		Return cResult

	End Function


	Public Function LoadAssignedEmployeeFerienData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal
		Dim result As Decimal? = 0
		result = m_SalaryValueDatabaseAccess.LoadFerienGuthaben(m_InitializationData.MDData.MDNr, employeeNumber, esNumber)

		If result Is Nothing Then
			m_Logger.LogError("Ferienguthaben können nicht geladen werden!")
		End If


		Return result

	End Function

	Public Function LoadAssignedEmployeeFeierTagData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal
		Dim result As Decimal? = 0
		result = m_SalaryValueDatabaseAccess.LoadFeierTagGuthaben(m_InitializationData.MDData.MDNr, employeeNumber, esNumber)

		If result Is Nothing Then
			m_Logger.LogError("Feiertag-Guthaben können nicht geladen werden!")
		End If


		Return result

	End Function

	Public Function LoadAssignedEmployee13LohnData(ByVal mdNr As Integer, ByVal employeeNumber As Integer, ByVal esNumber As Integer) As Decimal
		Dim result As Decimal? = 0
		result = m_SalaryValueDatabaseAccess.Load13LohnGuthaben(m_InitializationData.MDData.MDNr, employeeNumber, esNumber)

		If result Is Nothing Then
			m_Logger.LogError("13. Monatslohn-Guthaben können nicht geladen werden!")
		End If


		Return result

	End Function

	Public Function LoadAssignedEmployeeDarlehenData(ByVal mdNr As Integer, ByVal employeeNumber As Integer) As Decimal
		Dim result As Decimal? = 0
		result = m_SalaryValueDatabaseAccess.LoadDarlehenGuthaben(m_InitializationData.MDData.MDNr, employeeNumber)

		If result Is Nothing Then
			m_Logger.LogError("Darlehen-Guthaben können nicht geladen werden!")
		End If


		Return result

	End Function


#End Region


	Private Function AgeYear(ByVal iSelYear As Integer, ByVal dGebdat As Date) As Integer
		Return iSelYear - Year(dGebdat)
	End Function


End Class



Namespace SPSLohnUtility

	''' <summary>
	''' Funktionen zur Berechnung der Lohnarten von individuelle Rückstellungen (529.10 - 559.10)
	''' </summary>
	''' <remarks></remarks>
	Public Class ClsGuthabenIndividuell

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()


		Public Shared Sub InitializeObject()

			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		End Sub


		''' <summary>
		''' Guthaben von Feiertag wenn die Rückstellungen manuell erfasst wurden (LANr: 529.10 - 559.10)...
		''' </summary>
		''' <param name="iMANr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetFeierGuthabenIndividuell(ByVal iMANr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Feiertag Guthaben Pro MA 4 Manuelle Lohnarten]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return dBasis
		End Function

		' Guthaben von Feiertag Jahresübergreifend aber für Lohnabrechnungen...
		Public Shared Function GetLOFeierGuthabenIndividuell(ByVal iMANr As Integer,
																												 ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Feiertag Guthaben For Lo Pro MA 4 Manuelle Lohnarten]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function GetFerGuthabenIndividuell(ByVal iMANr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Ferien Guthaben Pro MA 4 Manuelle Lohnarten]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function GetLOFerGuthabenIndividuell(ByVal iMANr As Integer,
																											 ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject()
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Ferien Guthaben For Lo Pro MA 4 Manuelle Lohnarten]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function Get13GuthabenIndividuell(ByVal iMANr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject()
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get 13Lohn Guthaben Pro MA 4 Manuelle Lohnarten]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function GetLO13GuthabenIndividuell(ByVal iMANr As Integer,
																											ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject()
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get 13Lohn Guthaben For Lo Pro MA 4 Manuelle Lohnarten]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function


	End Class


	''' <summary>
	''' Funktionen zur Ermittlung des Guthaben pro Lohnabrechnung für den Druck
	''' </summary>
	''' <remarks></remarks>
	Public Class ClsGuthabenProLohnabrechnung

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()



		Public Shared Sub InitializeObject(ByVal mandantNumber As Integer?)

			ModulConstants.MDData = ModulConstants.SelectedMDData(If(mandantNumber.HasValue AndAlso mandantNumber > 0, mandantNumber, 0))
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		End Sub


		Public Shared Function GetFeierGuthabenProLO(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(mandantNumber)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get Feiertag Guthaben For LO With Netto 1 And 2 With Mandant]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		' Guthaben von Feiertag Jahresübergreifend aber für Lohnabrechnungen...
		Public Shared Function GetFeierGuthabenProLO(ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(Nothing)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get Feiertag Guthaben For LO With Netto 1 And 2]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function GetFerGuthabenProLO(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(mandantNumber)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get Ferientag Guthaben For LO With Netto 1 And 2 With Mandant]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function GetFerGuthabenProLO(ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(Nothing)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get Ferientag Guthaben For LO With Netto 1 And 2]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function


		Public Shared Function Get13LohnGuthabenProLO(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(mandantNumber)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get 13Lohn Guthaben For LO With Netto 1 And 2 With Mandant]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function Get13LohnGuthabenProLO(ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(Nothing)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get 13Lohn Guthaben For LO With Netto 1 And 2]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function


		Public Shared Function GetDarlehenGuthabenProLO(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(mandantNumber)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get Darlehen Guthaben For Lo With Mandant]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Auszahlung  + Rückstellung (ist negativ) = Das Guthaben von Darlehen
					dBasis = dBezahlt + dRueckstellung
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function GetDarlehenGuthabenProLO(ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(Nothing)
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get Darlehen Guthaben For Lo]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Auszahlung  + Rückstellung (ist negativ) = Das Guthaben von Darlehen
					dBasis = dBezahlt + dRueckstellung
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function GetNightGStdProLO(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer,
											ByRef cGutStd As Single, ByRef cGutBetrag As Single) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject(mandantNumber)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellungStd As Decimal = 0
			Dim dRueckstellungBetrag As Decimal = 0
			Dim dBezahltStd As Decimal = 0
			Dim dBezahltBetrag As Decimal = 0

			Dim dRestStd As Decimal = 0
			Dim dRestBetrag As Decimal = 0

			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get NightStd For MA In LO With Mandant]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@strYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellungStd = CDec(rFoundedrec("BackedStd"))
					dRueckstellungBetrag = CDec(rFoundedrec("BackedBetrag"))
					dBezahltStd = CDec(rFoundedrec("PayedStd"))
					dBezahltBetrag = CDec(rFoundedrec("PayedBetrag"))
					rFoundedrec.Close()

					sSql = "Select m_Btr, m_Anz, LP, Jahr From LOL Where LANr = 290 "
					sSql &= "And m_Anz <> 0 And Jahr > 2007 And MANr = @MANr And MDNr = @MDNr "
					sSql &= "Order By LP ASC, Jahr ASC"
					cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
					param = New System.Data.SqlClient.SqlParameter
					param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
					param = cmd.Parameters.AddWithValue("@MANr", iMANr)
					cmd.CommandType = Data.CommandType.Text
					Dim rLOLrec As SqlDataReader = cmd.ExecuteReader

					With rLOLrec
						Do While .Read
							Select Case Val(!LP)
								Case Is > sMonth
									If Val(!Jahr) < iYear Then
										dBezahltStd = dBezahltStd + Val(!m_Anz)
										dBezahltBetrag = dBezahltBetrag + Val(!m_Btr)
									End If

								Case Else
									If Val(!Jahr) <= iYear Then
										dBezahltStd = dBezahltStd + Val(!m_Anz)
										dBezahltBetrag = dBezahltBetrag + Val(!m_Btr)
									End If

							End Select
						Loop
					End With
					If dRueckstellungStd = 0 Or dRueckstellungBetrag = 0 Then Return dBasis

					dRestStd = dRueckstellungStd - dBezahltStd
					dRestBetrag = dRueckstellungBetrag - dBezahltBetrag
					If dRestStd = 0 And dRestBetrag = 0 Then Return dBasis

					' Rückstellung - Auszahlung = Das Guthaben von Gleitzeit
					cGutStd = dRestStd
					cGutBetrag = dRestBetrag
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis

		End Function

		Public Shared Function GetNightGStdProLO(ByVal iMANr As Integer, ByVal sMonth As Short, ByVal iYear As Integer,
													ByRef cGutStd As Single, ByRef cGutBetrag As Single) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject(Nothing)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellungStd As Decimal = 0
			Dim dRueckstellungBetrag As Decimal = 0
			Dim dBezahltStd As Decimal = 0
			Dim dBezahltBetrag As Decimal = 0

			Dim dRestStd As Decimal = 0
			Dim dRestBetrag As Decimal = 0

			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get NightStd For MA In LO]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@iMonth", sMonth)
				param = cmd.Parameters.AddWithValue("@strYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellungStd = CDec(rFoundedrec("BackedStd"))
					dRueckstellungBetrag = CDec(rFoundedrec("BackedBetrag"))
					dBezahltStd = CDec(rFoundedrec("PayedStd"))
					dBezahltBetrag = CDec(rFoundedrec("PayedBetrag"))
					rFoundedrec.Close()

					sSql = "Select m_Btr, m_Anz, LP, Jahr From LOL Where LANr = 290 "
					sSql &= "And m_Anz <> 0 And Jahr > 2007 And MANr = @MANr "
					sSql &= "Order By LP ASC, Jahr ASC"
					cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
					param = New System.Data.SqlClient.SqlParameter
					param = cmd.Parameters.AddWithValue("@MANr", iMANr)
					cmd.CommandType = Data.CommandType.Text
					Dim rLOLrec As SqlDataReader = cmd.ExecuteReader

					With rLOLrec
						Do While .Read
							Select Case Val(!LP)
								Case Is > sMonth
									If Val(!Jahr) < iYear Then
										dBezahltStd = dBezahltStd + Val(!m_Anz)
										dBezahltBetrag = dBezahltBetrag + Val(!m_Btr)
									End If

								Case Else
									If Val(!Jahr) <= iYear Then
										dBezahltStd = dBezahltStd + Val(!m_Anz)
										dBezahltBetrag = dBezahltBetrag + Val(!m_Btr)
									End If

							End Select
						Loop
					End With
					If dRueckstellungStd = 0 Or dRueckstellungBetrag = 0 Then Return dBasis

					dRestStd = dRueckstellungStd - dBezahltStd
					dRestBetrag = dRueckstellungBetrag - dBezahltBetrag
					If dRestStd = 0 And dRestBetrag = 0 Then Return dBasis

					' Rückstellung - Auszahlung = Das Guthaben von Gleitzeit
					cGutStd = dRestStd
					cGutBetrag = dRestBetrag
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis

		End Function


	End Class


	''' <summary>
	''' Funktionen zur Berechnung des Guthaben des Kandidaten...
	''' </summary>
	''' <remarks></remarks>
	Public Class ClsGuthaben

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()



		Public Shared Sub InitializeObject(ByVal mandantNumber As Integer?)

			ModulConstants.MDData = ModulConstants.SelectedMDData(If(mandantNumber.HasValue AndAlso mandantNumber > 0, mandantNumber, 0))
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		End Sub


		Public Shared Function GetFeierGuthaben(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal iESNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Feiertag Guthaben With Netto 1 And 2]"
			InitializeObject(mandantNumber)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		''' <summary>
		''' Guthaben von Feiertag Jahresübergreifend...
		''' Wenn lESNr > 0 dann kommt pro Einsatz!!!
		''' </summary>
		''' <param name="iMANr"></param>
		''' <param name="iESNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetFeierGuthaben(ByVal iMANr As Integer, ByVal iESNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Feiertag Guthaben With Netto 1 And 2]"
			InitializeObject(Nothing)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				param = cmd.Parameters.AddWithValue("@MDNr", ModulConstants.MDData.MDNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function


		Public Shared Function GetFerGuthaben(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal iESNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject(mandantNumber)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Ferientag Guthaben With Netto 1 And 2]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return dBasis
		End Function

		Public Shared Function GetFerGuthaben(ByVal iMANr As Integer, ByVal iESNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			InitializeObject(Nothing)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Ferientag Guthaben With Netto 1 And 2]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				param = cmd.Parameters.AddWithValue("@MDNr", ModulConstants.MDData.MDNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return dBasis
		End Function


		Public Shared Function Get13Guthaben(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal iESNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(mandantNumber)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get 13Lohn Guthaben With Netto 1 And 2]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		Public Shared Function Get13Guthaben(ByVal iMANr As Integer, ByVal iESNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(Nothing)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get 13Lohn Guthaben With Netto 1 And 2]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				param = cmd.Parameters.AddWithValue("@MDNr", ModulConstants.MDData.MDNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Rückstellung - Auszahlung = Das Guthaben von Ferien
					dBasis = (-1 * dRueckstellung) - dBezahlt
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function


		Public Shared Function GetDarlehenGuthaben(ByVal iMANr As Integer, ByVal iMDNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As Decimal = 0
			InitializeObject(iMDNr)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = "[Get Darlehen Guthaben]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@MDNr", iMDNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellung = CDec(rFoundedrec("BackedBetrag"))
					dBezahlt = CDec(rFoundedrec("PayedBetrag"))

					' Auszahlung  + Rückstellung (ist negativ) = Das Guthaben von Darlehen
					dBasis = dBezahlt + dRueckstellung
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function



		Public Shared Function GetAnzRPGStd_ES(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal iESNr As Integer, ByRef cGutStd As Decimal, ByRef cGutBetrag As Decimal) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellungStd As Decimal = 0
			Dim dRueckstellungBetrag As Decimal = 0
			Dim dBezahltStd As Decimal = 0
			Dim dBezahltBetrag As Decimal = 0
			InitializeObject(mandantNumber)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRestStd As Decimal = 0
			Dim dRestBetrag As Decimal = 0

			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get GleitStd For RP ES With Mandant]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellungStd = CDec(rFoundedrec("BackedStd"))
					dRueckstellungBetrag = CDec(rFoundedrec("BackedBetrag"))
					dBezahltStd = CDec(rFoundedrec("PayedStd"))
					dBezahltBetrag = CDec(rFoundedrec("PayedBetrag"))

					If dRueckstellungStd = 0 Or dRueckstellungBetrag = 0 Then Return dBasis

					dRestStd = dRueckstellungStd - dBezahltStd
					dRestBetrag = dRueckstellungBetrag - dBezahltBetrag
					If dRestStd = 0 And dRestBetrag = 0 Then Return dBasis

					' Rückstellung - Auszahlung = Das Guthaben von Gleitzeit
					cGutStd = dRestStd
					cGutBetrag = dRestBetrag
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis

		End Function

		Public Shared Function GetAnzRPGStd_ES(ByVal iMANr As Integer, ByVal iESNr As Integer, ByRef cGutStd As Decimal, ByRef cGutBetrag As Decimal) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellungStd As Decimal = 0
			Dim dRueckstellungBetrag As Decimal = 0
			Dim dBezahltStd As Decimal = 0
			Dim dBezahltBetrag As Decimal = 0
			InitializeObject(Nothing)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRestStd As Decimal = 0
			Dim dRestBetrag As Decimal = 0

			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get GleitStd For RP ES]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellungStd = CDec(rFoundedrec("BackedStd"))
					dRueckstellungBetrag = CDec(rFoundedrec("BackedBetrag"))
					dBezahltStd = CDec(rFoundedrec("PayedStd"))
					dBezahltBetrag = CDec(rFoundedrec("PayedBetrag"))

					If dRueckstellungStd = 0 Or dRueckstellungBetrag = 0 Then Return dBasis

					dRestStd = dRueckstellungStd - dBezahltStd
					dRestBetrag = dRueckstellungBetrag - dBezahltBetrag
					If dRestStd = 0 And dRestBetrag = 0 Then Return dBasis

					' Rückstellung - Auszahlung = Das Guthaben von Gleitzeit
					cGutStd = dRestStd
					cGutBetrag = dRestBetrag
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis

		End Function

		Public Shared Function Get_AnzRPNightStd(ByVal mandantNumber As Integer, ByVal iMANr As Integer, ByVal iESNr As Integer, ByRef cGutStd As Decimal, ByRef cGutBetrag As Decimal) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellungStd As Decimal = 0
			Dim dRueckstellungBetrag As Decimal = 0
			Dim dBezahltStd As Decimal = 0
			Dim dBezahltBetrag As Decimal = 0
			InitializeObject(mandantNumber)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRestStd As Decimal = 0
			Dim dRestBetrag As Decimal = 0

			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get NightStd For RP ES With Mandant]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", mandantNumber)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellungStd = CDec(rFoundedrec("BackedStd"))
					dRueckstellungBetrag = CDec(rFoundedrec("BackedBetrag"))
					dBezahltStd = CDec(rFoundedrec("PayedStd"))
					dBezahltBetrag = CDec(rFoundedrec("PayedBetrag"))

					If dRueckstellungStd = 0 Or dRueckstellungBetrag = 0 Then Return dBasis

					dRestStd = dRueckstellungStd - dBezahltStd
					dRestBetrag = dRueckstellungBetrag - dBezahltBetrag
					If dRestStd = 0 And dRestBetrag = 0 Then Return dBasis

					' Rückstellung - Auszahlung = Das Guthaben von Gleitzeit
					cGutStd = dRestStd
					cGutBetrag = dRestBetrag
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis

		End Function

		Public Shared Function Get_AnzRPNightStd(ByVal iMANr As Integer, ByVal iESNr As Integer,
															 ByRef cGutStd As Decimal, ByRef cGutBetrag As Decimal) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellungStd As Decimal = 0
			Dim dRueckstellungBetrag As Decimal = 0
			Dim dBezahltStd As Decimal = 0
			Dim dBezahltBetrag As Decimal = 0
			InitializeObject(Nothing)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim dRestStd As Decimal = 0
			Dim dRestBetrag As Decimal = 0

			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get NightStd For RP ES]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellungStd = CDec(rFoundedrec("BackedStd"))
					dRueckstellungBetrag = CDec(rFoundedrec("BackedBetrag"))
					dBezahltStd = CDec(rFoundedrec("PayedStd"))
					dBezahltBetrag = CDec(rFoundedrec("PayedBetrag"))

					If dRueckstellungStd = 0 OrElse dRueckstellungBetrag = 0 Then Return dBasis

					dRestStd = dRueckstellungStd - dBezahltStd
					dRestBetrag = dRueckstellungBetrag - dBezahltBetrag
					If dRestStd = 0 And dRestBetrag = 0 Then Return dBasis

					' Rückstellung - Auszahlung = Das Guthaben von Gleitzeit
					cGutStd = dRestStd
					cGutBetrag = dRestBetrag
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis

		End Function


		Public Shared Function GetAnzNightStd(ByVal iMANr As Integer, ByRef cGutStd As Decimal, ByRef cGutBetrag As Decimal, ByVal iMDNr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellungStd As Decimal = 0
			Dim dRueckstellungBetrag As Decimal = 0
			Dim dBezahltStd As Decimal = 0
			Dim dBezahltBetrag As Decimal = 0
			InitializeObject(iMDNr)

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Dim dRestStd As Decimal = 0
			Dim dRestBetrag As Decimal = 0

			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get NightStd For MA]"


			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@MDNr", iMDNr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dRueckstellungStd = CDec(rFoundedrec("BackedStd"))
					dRueckstellungBetrag = CDec(rFoundedrec("BackedBetrag"))
					dBezahltStd = CDec(rFoundedrec("PayedStd"))
					dBezahltBetrag = CDec(rFoundedrec("PayedBetrag"))

					'If dRueckstellungStd = 0 Or dRueckstellungBetrag = 0 Then Return dBasis

					dRestStd = dRueckstellungStd - dBezahltStd
					dRestBetrag = dRueckstellungBetrag - dBezahltBetrag
					If dRestStd = 0 And dRestBetrag = 0 Then Return dBasis

					' Rückstellung - Auszahlung = Das Guthaben von Gleitzeit
					cGutStd = dRestStd
					cGutBetrag = dRestBetrag
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis

		End Function



	End Class


	Public Class ClsLohn

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()


#Region "Constructor"

		Public Sub New()

			InitializeObject()

		End Sub

#End Region

		Public Shared Sub InitializeObject()

			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		End Sub



#Region "Funktionen für die Berechnung der Rückstellungen in Lohnabrechnung..."

		<Obsolete("This method is deprecated.")>
		Public Shared Function GetFeierBackNettoBasis(ByVal iLONr As Integer, ByVal iMANr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dAnteilSozial As Decimal = 0
			Dim dBetrag As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Data 4 FeierBetrag in Netto]"
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@LONr", iLONr)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dAnteilSozial = CDec(rFoundedrec("SozialAnteil"))
					dBetrag = CDec(rFoundedrec("FeierBasis"))
				End If

				If dBetrag <> 0 AndAlso dAnteilSozial <> 0 Then
					dBasis = dBetrag * (dAnteilSozial / 100)
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		<Obsolete("This method is deprecated.")>
		Public Shared Function GetFerienBackNettoBasis(ByVal iLONr As Integer, ByVal iMANr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dAnteilSozial As Decimal = 0
			Dim dBetrag As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Data 4 FerienBetrag in Netto]"
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@LONr", iLONr)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dAnteilSozial = CDec(rFoundedrec("SozialAnteil"))
					dBetrag = CDec(rFoundedrec("FerienBasis"))
				End If

				If dBetrag <> 0 AndAlso dAnteilSozial <> 0 Then
					dBasis = dBetrag * (dAnteilSozial / 100)
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function

		<Obsolete("This method is deprecated.")>
		Public Shared Function Get13LohnBackNettoBasis(ByVal iLONr As Integer, ByVal iMANr As Integer) As Decimal
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dAnteilSozial As Decimal = 0
			Dim dBetrag As Decimal = 0
			Dim dBasis As Decimal = 0
			Dim sSql As String = "[Get Data 4 13LohnBetrag in Netto]"
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@LONr", iLONr)
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dAnteilSozial = CDec(rFoundedrec("SozialAnteil"))
					dBetrag = CDec(rFoundedrec("13LohnBasis"))
				End If

				If dBetrag <> 0 AndAlso dAnteilSozial <> 0 Then
					dBasis = dBetrag * (dAnteilSozial / 100)
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dBasis
		End Function


#End Region


	End Class


	Public Class ClsLOFunktionality

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()
		Private m_md As New Mandant

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility
		Private m_utilitySP As SPProgUtility.MainUtilities.Utilities



#Region "Constructor"


		Public Sub New()

			m_md = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_utilitySP = New SPProgUtility.MainUtilities.Utilities

			InitializeObject()

		End Sub


		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_md = New Mandant
			m_md = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_utilitySP = New SPProgUtility.MainUtilities.Utilities

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

			InitializeObject()

		End Sub


#End Region

		Public Shared Sub InitializeObject()

			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		End Sub

		Public Shared Function GetMAAlter(ByVal iYear As Integer, ByVal Gebdat As Date) As Short
			Return iYear - Year(Gebdat)
		End Function


		Public Shared Function IsMARentner(ByVal iMANr As Integer, ByVal iYear As Integer) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim bResult As Boolean = False
			Dim RentAlter As Integer
			Dim MAAlter As Short
			Dim sSql As String = String.Empty
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			sSql = "[Get Data For Calculating Renter]"

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@Jahr", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					Dim MAGebDat As Date = Format(rFoundedrec("GebDat"), "d")

					RentAlter = IIf(rFoundedrec("Geschlecht") = "M", rFoundedrec("RentAlter_M"), rFoundedrec("RentAlter_w"))
					MAAlter = GetMAAlter(iYear, rFoundedrec("GebDat"))
					If MAAlter = RentAlter Then
						If Month(MAGebDat) >= Now.Month Then
							MAAlter = MAAlter - 1
						End If
					End If

					If MAAlter >= RentAlter Or rFoundedrec("AHVCode") = 2 Then bResult = True
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				bResult = False

			Finally


			End Try

			Return bResult
		End Function


		Public Shared Function GetBVGAns(ByVal dMAGebDat As Date, ByVal strGeschlecht As String, ByVal iYear As Integer) As Single
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim iAlter As Decimal = iYear - Year(dMAGebDat)
			Dim dAnsatz As Decimal = 0
			Dim sSql As String = String.Format("[Get BVG Ansatz 4 {0}]", strGeschlecht)
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@Alter", iAlter)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dAnsatz = CDec(rFoundedrec("Prozentsatz"))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dAnsatz

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try

			Return dAnsatz
		End Function


		Public Shared Function GetSelectedLAInfo(ByVal sLANr As Single, ByVal b4MA As Boolean, ByVal strlang As String, ByVal iYear As Integer) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strValue As String = CStr(sLANr)
			Dim sSql As String = "[Get Info 4 Selected LANr]"
			InitializeObject()

			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@mLANr", sLANr)
				param = cmd.Parameters.AddWithValue("@b4MA", b4MA)
				param = cmd.Parameters.AddWithValue("@MDYear", iYear)
				param = cmd.Parameters.AddWithValue("@Lang", strlang)

				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  '
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					strValue = rFoundedrec("LAName")
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return strValue

			Finally
				Conn.Close()
				Conn.Dispose()


			End Try


			Return strValue
		End Function


	End Class




End Namespace



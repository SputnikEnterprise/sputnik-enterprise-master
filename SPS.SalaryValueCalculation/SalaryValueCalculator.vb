Imports SP.DatabaseAccess.SalaryValueCalculation
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenIndividuell
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenProLohnabrechnung
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben
Imports SP.DatabaseAccess.Listing
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

''' <summary>
''' Calculates salary values.
''' </summary>
Public Class SalaryValueCalculator

#Region "Private Fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The mandant number.
	''' </summary>
	Private m_MDNr As Integer

	Private m_DBAccess As ISalaryValueCalculationDatabaseAccess


#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal setting As SP.Infrastructure.Initialization.InitializeClass)
		'ByVal mdNr As Integer,
		'		 ByVal dbConnectionString As String,
		'		 ByVal userLangauge As String)
		m_InitializationData = setting
		m_UtilityUI = New UtilityUI

		m_MDNr = m_InitializationData.MDData.MDNr
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_DBAccess = New SalaryValueCalculationDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

	End Sub

#End Region

	''' <summary>
	''' Calculates the basis (Basis) value.
	''' </summary>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Calculation result or nothing if calculation has no result.</returns>
	Public Function CalculateBasisValue(ByVal parameters As SalaryCalculationParameters) As SalaryCalculationResult
		Dim result As SalaryCalculationResult = New SalaryCalculationResult

		Dim employeeData = m_DBAccess.LoadEmployeeData(parameters.MANr)

		If employeeData Is Nothing Then
			Throw New Exception(String.Format("Could not read Mitarbeiter data. MANr={0}", parameters.MANr))
		End If

		Dim laData = m_DBAccess.LoadLAData(parameters.LANr, parameters.Year)

		If laData Is Nothing Then
			Throw New Exception(String.Format("Could not read LA data. LANr={0}", parameters.LANr))
		End If

		Dim fixBasis As Decimal? = laData.FixBasis

		If Not fixBasis.HasValue Then
			Throw New Exception(String.Format("FixBasis must have a value. LANr={0}", parameters.LANr))
		End If

		If laData.SeeKanton Then

			Dim fixBasisColumnInMDKiAu As String = laData.MABasVar

			If String.IsNullOrEmpty(fixBasisColumnInMDKiAu) Then
				Throw New Exception("MABasVar must not be null or empty if SeeKanton is true.")
			End If

			If Not String.IsNullOrEmpty(employeeData.CivilStatus) AndAlso
			  employeeData.CivilStatus.Trim().ToUpper = "A" AndAlso
			  Not fixBasisColumnInMDKiAu.ToLower().Contains("month") Then

				fixBasisColumnInMDKiAu = fixBasisColumnInMDKiAu.Replace("1", "2")

			End If

			Dim canton As String = parameters.Canton
			If String.IsNullOrEmpty(canton) Then
				canton = employeeData.Canton
			End If

			Dim md_KiAuData = m_DBAccess.LoadFixBasisFromMDKiAu(fixBasisColumnInMDKiAu, canton, parameters.Year)

			If md_KiAuData Is Nothing Then
				Throw New Exception(String.Format("FixBasis could not be loaded from MD_KiAu data for the following parameters (Column={0}, Canton={1} , Year={2})", fixBasisColumnInMDKiAu, canton, parameters.Year))
			End If

			fixBasis = md_KiAuData.FixBasis

		End If

		If Not laData.TypeBasis.HasValue Then
			Throw New Exception(String.Format("LA record does not have a TypeBasis value. (LANr={0})", laData.LANR))
		End If

		Select Case laData.TypeBasis.Value

			Case 1
				result.Basis = fixBasis
			Case 2
				' Fixer Ansatz
				result.Basis = fixBasis
				result.IsBasisReadonly = True
			Case 3
				'Systemkonstanten
				result.Basis = fixBasis
			Case 4
		'Summenvariable (nothing to do at the moment)
			Case 30
				' Funktionen
				Dim maBasVar = laData.MABasVar

				If String.IsNullOrEmpty(maBasVar) Then
					Throw New Exception("MABasVar must not be null or empty.")
				End If

				If maBasVar.Contains(")") Then
					maBasVar = maBasVar.Substring(0, maBasVar.IndexOf(")"))
				End If
				Dim functionNr As Decimal = Convert.ToDecimal(maBasVar)
				Dim value As Decimal? = EvaluateBasValue(functionNr, parameters, result)

				If Not value = Decimal.MaxValue Then
					result.Basis = value
				End If

		End Select

		Return result

	End Function

	''' <summary>
	''' Calculates the count (Anzahl) value.
	''' </summary>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Calculation result or nothing if calculation has no result.</returns>
	Public Function CalculateCount(ByVal parameters As SalaryCalculationParameters) As SalaryCalculationResult

		Dim result As SalaryCalculationResult = New SalaryCalculationResult

		Dim laData = m_DBAccess.LoadLAData(parameters.LANr, parameters.Year)

		If laData Is Nothing Then
			Throw New Exception(String.Format("Could not read LA data. LANr={0}", parameters.LANr))
		End If

		If Not laData.TypeAnzahl.HasValue Then
			Throw New Exception(String.Format("LA record does not have a TypeAnzahl value. (LANr={0})", laData.LANR))
		End If

		Select Case laData.TypeAnzahl
			Case 1
				' Fixer Anzahl
				result.Anzahl = laData.FixAnzahl
			Case 2
				' Fixer Anzahl
				result.Anzahl = laData.FixAnzahl
				result.IsAnzahlReadonly = True

			Case 3
				' Systemkonstanten
				result.Anzahl = laData.FixAnzahl
			Case 4
				' Summenvariable (nothing to do at the moment)
		End Select

		Return result
	End Function

	''' <summary>
	''' Calculates the Ansatz value.
	''' </summary>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Calculation result or nothing if calculation has no result.</returns>
	Public Function CalculateAnsatz(ByVal parameters As SalaryCalculationParameters) As SalaryCalculationResult

		Dim result As SalaryCalculationResult = New SalaryCalculationResult

		Dim laData = m_DBAccess.LoadLAData(parameters.LANr, parameters.Year)

		If laData Is Nothing Then
			Throw New Exception(String.Format("Could not read LA data. LANr={0}", parameters.LANr))
		End If

		If Not laData.TypeAnsatz.HasValue Then
			Throw New Exception(String.Format("LA record does not have a TypeAnsatz value. (LANr={0})", laData.LANR))
		End If

		Select Case laData.TypeAnsatz
			Case 1
				' Fixer Ansatz
				result.Ansatz = laData.FixAnsatz
			Case 2
				' Fixer Ansatz
				result.Ansatz = laData.FixAnsatz
				result.IsAnsatzReadonly = True
			Case 3
				' Systemkonstanten
				result.Ansatz = laData.FixAnsatz
			Case 30
				' Funktionen
				Dim maAnsVar = laData.MAAnsVar.Trim()

				If String.IsNullOrEmpty(maAnsVar) Then
					Throw New Exception("maAnsVar must not be null or empty.")
				End If

				If maAnsVar.Contains(")") Then
					maAnsVar = maAnsVar.Substring(0, maAnsVar.IndexOf(")"))
				End If
				Dim functionNr As Decimal = Convert.ToDecimal(maAnsVar)
				Dim value As Decimal? = EvaluateBasValue(functionNr, parameters, result)

				If Not value = Decimal.MaxValue Then
					result.Ansatz = value
				End If


		End Select

		Return result

	End Function

	''' <summary>
	''' Evaluates a bas value.
	''' </summary>
	''' <param name="functionNumber">The function number.</param>
	''' <param name="parameters">The parameters.</param>
	''' <param name="calculationResult">The calculation result reference.</param>
	''' <returns>Decimal value. In two cases (FunctionNr 21 and 22) the result values are stored in the calculationResult parameter)</returns>
	Private Function EvaluateBasValue(ByVal functionNumber As Decimal, ByVal parameters As SalaryCalculationParameters, ByVal calculationResult As SalaryCalculationResult) As Decimal?
		Dim esNr As Integer = 0
		Dim cRestStd As Decimal = 0
		Dim cRestBetrag As Decimal = 0

		If functionNumber = 0 Then
			Return 0
		End If

		Dim result As Decimal? = Nothing

		Select Case functionNumber
			Case 10, 10.1, 10.11, 10.2, 10.21, 17, 18, 19, 20

				Dim employeeData = m_DBAccess.LoadEmployeeData(parameters.MANr)

				If employeeData Is Nothing Then
					Throw New Exception(String.Format("Could not read Mitarbeiter data. MANr={0}", parameters.MANr))
				End If

				If Not employeeData.Birthdate.HasValue Then
					Throw New Exception(String.Format("Mitarbeiter GebDat is null. MANr={0}", parameters.MANr))
				End If

				Dim payrollUtility = New SPS.MA.Lohn.Utility.PayrollUtility(m_MDNr)
				If functionNumber = 10 Then
					result = payrollUtility.LoadBVGProcentage(employeeData.Birthdate.Value.Date, employeeData.Gender)

				Else
					Dim data = payrollUtility.LoadMandantPercentages(employeeData.Birthdate.Value.Date, employeeData.Gender, parameters.Year)

					If functionNumber = 10.1 Then
						result = data.UVGZ_A

					ElseIf functionNumber = 10.11 Then
						result = data.UVGZ_B

					ElseIf functionNumber = 10.2 Then
						result = data.UVGZ2_A

					ElseIf functionNumber = 10.21 Then
						result = data.UVGZ2_B

					ElseIf functionNumber = 17 Then
						esNr = If(parameters.OptionCalculateGuthabenWithouthES, 0, parameters.ESNr)
						result = payrollUtility.LoadAssignedEmployeeFeierTagData(m_MDNr, parameters.MANr, esNr)
					ElseIf functionNumber = 18 Then
						esNr = If(parameters.OptionCalculateGuthabenWithouthES, 0, parameters.ESNr)
						result = payrollUtility.LoadAssignedEmployeeFerienData(m_MDNr, parameters.MANr, esNr)
					ElseIf functionNumber = 19 Then
						esNr = If(parameters.OptionCalculateGuthabenWithouthES, 0, parameters.ESNr)
						result = payrollUtility.LoadAssignedEmployee13LohnData(m_MDNr, parameters.MANr, esNr)
					ElseIf functionNumber = 20 Then
						result = payrollUtility.LoadAssignedEmployeeDarlehenData(m_MDNr, parameters.MANr)

					End If
				End If


			'Case 17
			'	esNr = If(parameters.OptionCalculateGuthabenWithouthES, 0, parameters.ESNr)
	 '     result = GetFeierGuthaben(parameters.MANr, esNr)
		'  Case 18
				'esNr = If(parameters.OptionCalculateGuthabenWithouthES, 0, parameters.ESNr)
				'result = GetFerGuthaben(parameters.MANr, esNr)
			'Case 19
	 '     esNr = If(parameters.OptionCalculateGuthabenWithouthES, 0, parameters.ESNr)
	 '     result = Get13Guthaben(parameters.MANr, esNr)
			Case 17.1
				result = GetLOFeierGuthabenIndividuell(parameters.MANr, parameters.Month, parameters.Year)
			Case 18.1
				result = GetFerGuthabenIndividuell(parameters.MANr)
			Case 19.1
				result = Get13GuthabenIndividuell(parameters.MANr)
			Case 20
				result = GetDarlehenGuthaben(parameters.MANr, m_MDNr)
			Case 21
				'Auszahlung(Gleitzeit)
				GetAnzRPGStd_ES(parameters.MANr, parameters.ESNr, cRestStd, cRestBetrag)

				calculationResult.Anzahl = cRestStd
				calculationResult.Ansatz = 100.0

				If cRestStd = 0 Then
					calculationResult.Basis = 0
				Else
					calculationResult.Basis = cRestBetrag / cRestStd
				End If

				result = Decimal.MaxValue
			Case 22
				' Auszahlung Nachtzeitzulage
				Get_AnzRPNightStd(parameters.MANr, parameters.ESNr, cRestStd, cRestBetrag)

				calculationResult.Anzahl = cRestStd
				calculationResult.Ansatz = 100.0

				If cRestStd = 0 Then
					calculationResult.Basis = 0
				Else
					calculationResult.Basis = cRestBetrag / cRestStd
				End If

				result = Decimal.MaxValue
			Case Else
				' Do nothing

		End Select

		Return result
	End Function

End Class

''' <summary>
''' Salary calculation parameters.
''' </summary>
Public Class SalaryCalculationParameters


#Region "Public Properties"

	Public Property MANr As Integer
	Public Property LANr As Decimal
	Public Property ESNr As Integer
	Public Property Canton As String
	Public Property Month As Short
	Public Property Year As Integer

	Public Property OptionCalculateGuthabenWithouthES As Boolean

#End Region

End Class

''' <summary>
''' Salary calculation result.
''' </summary>
Public Class SalaryCalculationResult
	Public Property Anzahl As Decimal?
	Public Property Basis As Decimal?
	Public Property Ansatz As Decimal?

	Public Property IsAnzahlReadonly As Boolean?
	Public Property IsBasisReadonly As Boolean?
	Public Property IsAnsatzReadonly As Boolean?

End Class


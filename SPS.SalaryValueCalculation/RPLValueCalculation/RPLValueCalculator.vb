Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenIndividuell
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenProLohnabrechnung
Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben
Imports SP.DatabaseAccess.SalaryValueCalculation


Namespace RPLValueCalculation

	Public Class RPLValueCalculator



#Region "Private Fields"

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Protected m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The data access object.
		''' </summary>
		Private m_SalaryValueDataAccess As ISalaryValueCalculationDatabaseAccess

#End Region


#Region "Constructor"

		Public Sub New(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = initializationClass
			m_SalaryValueDataAccess = New SalaryValueCalculationDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		End Sub


#End Region


		''' <summary>
		''' Calculates salary type (LA) values for employee.
		''' </summary>
		''' <param name="params">The input parameters.</param>
		''' <returns>The calculation result.</returns>
		Public Function CalculateSalaryTypeValuesForEmployee(ByVal params As RPLSalaryTypeValuesCalcParams) As RPLSalaryTypeValuesCalcuResult

			Dim result As RPLSalaryTypeValuesCalcuResult = New RPLSalaryTypeValuesCalcuResult

			' Anzahl
			result.Anzahl = 0D

			If params.LAParams.TypeAnsatz = 2 OrElse params.LAParams.TypeAnsatz = 3 Then
				result.Ansatz = params.LAParams.FixAnsatz
			Else
				result.Ansatz = params.LAParams.FixAnsatz
			End If

			' Basis
			Select Case params.LAParams.TypeBasis
				Case 30 ' Funktionen

					Dim maBasVar = params.LAParams.MABasVar

					If String.IsNullOrEmpty(maBasVar) Then
						Throw New Exception("MABasVar must not be null or empty.")
					End If

					If maBasVar.Contains(")") Then
						maBasVar = maBasVar.Substring(0, maBasVar.IndexOf(")"))
					End If
					Dim functionNr As Decimal = Convert.ToDecimal(maBasVar)
					Dim value As Decimal? = EvaluateBasValue(functionNr, params, result)

					If Not value = Decimal.MaxValue Then
						result.Basis = value
					End If

				Case Else
					If params.LAParams.MABasVar.Trim() = "8000" Then
						result.Basis = params.ESParams.Grundlohn

					ElseIf params.LAParams.MABasVar.Trim() = "8001" Then
						' Grundlohn + 13. Lohn
						result.Basis = params.ESParams.Grundlohn + (params.ESParams.Grundlohn.GetValueOrDefault(0) * params.ESParams.Lohn13Proz / 100)

					ElseIf params.LAParams.MABasVar.Trim() = "8002" Then
						' Grundlohn + Ferien + 13. Lohn
						Dim basisFerien As Decimal = params.ESParams.Grundlohn.GetValueOrDefault(0)
						Dim amountFerien As Decimal = (basisFerien * params.ESParams.LohnFerienProz / 100)

						Dim basis13Lohn As Decimal = params.ESParams.Grundlohn.GetValueOrDefault(0) + amountFerien
						Dim amount13Lohn As Decimal = (basis13Lohn * params.ESParams.Lohn13Proz / 100)


						result.Basis = params.ESParams.Grundlohn + amountFerien + amount13Lohn

					ElseIf params.LAParams.MABasVar.Trim() = "8003" Then
						' std-Lohn
						result.Basis = params.ESParams.Stundenlohn

					Else
							result.Basis = 0D
					End If

			End Select

			Return result
		End Function

		''' <summary>
		''' Calculates salary type (LA) values for customer.
		''' </summary>
		''' <param name="params">The input parameters.</param>
		''' <returns>The calculation result.</returns>
		Public Function CalculateSalaryTypeValuesForCustomer(ByVal params As RPLSalaryTypeValuesCalcParams) As RPLSalaryTypeValuesCalcuResult

			Dim result As RPLSalaryTypeValuesCalcuResult = New RPLSalaryTypeValuesCalcuResult

			' Ansatz
			If params.LAParams.TypeAnsatz = 2 Or
			 params.LAParams.TypeAnsatz = 3 Then
				result.Ansatz = params.LAParams.FixAnsatz
			Else
				result.Ansatz = params.LAParams.FixAnsatz
			End If

			If params.LAParams.KDBasis = "8000" Then
				result.Basis = params.ESParams.Tarif
			ElseIf params.LAParams.KDBasis = "8001" Then
				result.Basis = params.ESParams.Tarif
			ElseIf params.LAParams.KDBasis = "8002" Then
				result.Basis = params.ESParams.Tarif
			ElseIf params.LAParams.KDBasis = "8003" Then
				result.Basis = params.ESParams.Tarif

			Else
				result.Basis = 0D
			End If

			If params.LAParams.MWSTPflichtig = True Then

				'result.MwSt = If(params.ESParams.MWStBetrag.HasValue AndAlso params.ESParams.MWStBetrag > 0, params.DefaultMWStProz, 0)
				result.MwSt = If(params.ESParams.ESLohnIsMwStPflichtig, params.DefaultMWStProz, 0)

			End If

			Return result
		End Function

		''' <summary>
		''' Calculates RPL additional fee values.
		''' </summary>
		''' <param name="params">The input parmeters.</param>
		''' <returns>The result.</returns>
		Public Function CalculateRPLAdditionalFeeValues(ByVal params As RPLAdditionalFeesValuesCalcParams) As RPLAdditionalFeesValuesCalcResult

			Dim result As RPLAdditionalFeesValuesCalcResult = New RPLAdditionalFeesValuesCalcResult


			' Determine Ansätze
			If params.LAParams.FeierInklusiv Then
				result.FeiertagAnsatz = params.ESParams.FeierProz
			Else
				result.FeiertagAnsatz = 0D
			End If

			If params.LAParams.FerienInklusiv Then
				result.FerienAnsatz = params.ESParams.FerienProz
			Else
				result.FerienAnsatz = 0D
			End If

			If params.LAParams.Inklusiv13 Then
				result.Lohn13Ansatz = params.ESParams.Lohn13Proz
			Else
				result.Lohn13Ansatz = 0D
			End If


			Dim esSalaryValueCalculation = New ESSalaryValueCalculation

			Dim calcFerFeier13BasisResult As SalaryValueCalculation.ESSalaryValueCalculation.CalcFerFeier13BasisResult
			If params.ESParams.HasGAVData Then
				calcFerFeier13BasisResult = esSalaryValueCalculation.CalcFerFeier13BasisWithGAV(params.GrundLohnForBaseValueCalculation,
																		result.FeiertagAnsatz,
																		result.FerienAnsatz,
																		result.Lohn13Ansatz,
																		params.ESParams.LOFeiertagWay,
																		params.ESParams.FerienWay,
																		params.ESParams.LO13Way)
			Else
				calcFerFeier13BasisResult = esSalaryValueCalculation.CalculateFerFeier13BasisWithoutGAV(params.GrundLohnForBaseValueCalculation,
																				result.FeiertagAnsatz,
																				result.FerienAnsatz,
																				result.Lohn13Ansatz,
																				params.ESParams.FerienWay,
																				params.ESParams.LO13Way)
			End If


			result.CalcFerFeier13BasisResult = calcFerFeier13BasisResult

			Return result
		End Function


		''' <summary>
		''' Evaluates a bas value.
		''' </summary>
		''' <param name="funtionNumber">The function number.</param>
		''' <param name="parameters">The parameters.</param>
		''' <param name="calculationResult">The calculation result reference.</param>
		''' <returns>Decimal value. In two cases (FunctionNr 21 and 22) the result values are stored in the calculationResult parameter)</returns>
		Private Function EvaluateBasValue(ByVal funtionNumber As Decimal, ByVal parameters As RPLSalaryTypeValuesCalcParams, ByVal calculationResult As RPLSalaryTypeValuesCalcuResult) As Decimal?
			Dim esNr As Integer = 0
			Dim cRestStd As Decimal = 0
			Dim cRestBetrag As Decimal = 0

			If funtionNumber = 0 Then
				Return 0
			End If

			Dim result As Decimal? = Nothing

			Select Case funtionNumber

				Case 21
					'Auszahlung(Gleitzeit)
					GetAnzRPGStd_ES(parameters.MANr, parameters.ESParams.ESNr, cRestStd, cRestBetrag)

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
					Get_AnzRPNightStd(parameters.MANr, parameters.ESParams.ESNr, cRestStd, cRestBetrag)

					calculationResult.Anzahl = cRestStd
					calculationResult.Ansatz = parameters.LAParams.FixAnsatz ' 100.0

					If cRestStd = 0 Then
						calculationResult.Basis = 0
					Else
						calculationResult.Basis = cRestBetrag * calculationResult.Ansatz / cRestStd
					End If

					result = Decimal.MaxValue


				Case 22.01
					' Auszahlung Nachtzeitzulage
					Dim nightData = m_SalaryValueDataAccess.LoadAmountOfNightInReport(m_InitializationData.MDData.MDNr, parameters.MANr, parameters.ESParams.ESNr)

					If nightData Is Nothing OrElse nightData.BackedHours.GetValueOrDefault(0) = 0 OrElse nightData.BackedHours.GetValueOrDefault(0) = 0 Then Return 0

					Dim restHours As Decimal = nightData.BackedHours - nightData.PayedHours
					Dim restAmount As Decimal = nightData.BackedAmount - nightData.PayedAmount
					If restHours = 0 And restAmount = 0 Then Return 0

					calculationResult.Anzahl = restHours
					calculationResult.Ansatz = parameters.LAParams.FixAnsatz ' 100.0

					If restHours = 0 Then
						calculationResult.Basis = 0
					Else
						calculationResult.Basis = restAmount * calculationResult.Ansatz / restHours
					End If


				Case Else
					' Do nothing

			End Select

			Return result
		End Function


	End Class



End Namespace
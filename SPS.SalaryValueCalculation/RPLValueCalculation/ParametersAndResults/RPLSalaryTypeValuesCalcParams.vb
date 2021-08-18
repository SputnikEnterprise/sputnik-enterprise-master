Namespace RPLValueCalculation

	''' <summary>
	''' RPL salary type values calculation parmeters.
	''' </summary>
	Public Class RPLSalaryTypeValuesCalcParams

#Region "Constructor"

		Public Sub New(ByVal maNr As Integer,
					   ByVal defaultMwStProz As Decimal,
					   ByVal esParameters As RPLSalaryTypeValuesCalcESParams,
					   ByVal laParameters As RPLSalaryTypeValuesCalcLAParams)

			Me.MANr = maNr
			Me.DefaultMWStProz = defaultMwStProz

			Me.ESParams = esParameters
			Me.LAParams = laParameters

		End Sub

#End Region

#Region "Public Properties"

		Public Property MANr As Integer
		Public Property DefaultMWStProz As Decimal

		Public Property ESParams As RPLSalaryTypeValuesCalcESParams
		Public Property LAParams As RPLSalaryTypeValuesCalcLAParams

#End Region

#Region "Helper Classes"

		''' <summary>
		''' The ES parameters
		''' </summary>
		Class RPLSalaryTypeValuesCalcESParams

			Public Sub New(ByVal esNr As Integer, ByVal tarif As Decimal?, ByVal grundlohn As Decimal?, ByVal mwstBetrag As Decimal?)

				Me.ESNr = esNr
				Me.Tarif = tarif
				Me.Grundlohn = grundlohn
				Me.MWStBetrag = mwstBetrag
			End Sub

			Public Property ESNr As Integer?
			Public Property Tarif As Decimal?
			Public Property Grundlohn As Decimal?
			Public Property Stundenlohn As Decimal?
			Public Property MWStBetrag As Decimal?
			Public Property LohnFeierProz As Decimal?
			Public Property LohnFerienProz As Decimal?
			Public Property Lohn13Proz As Decimal?
			Public Property ESLohnIsMwStPflichtig As Boolean?

		End Class

		''' <summary>
		''' The LA parameters.
		''' </summary>
		Class RPLSalaryTypeValuesCalcLAParams

			Public Sub New(ByVal laNr As Decimal?,
						 ByVal typeAnsatz As Short, ByVal typeBasis As Short, ByVal fixAnsatz As Decimal,
						 ByVal maBasVar As String, ByVal kdBasis As String, ByVal mwstPflcihtig As Boolean)

				Me.LANr = laNr
				Me.TypeAnsatz = typeAnsatz
				Me.TypeBasis = typeBasis
				Me.FixAnsatz = fixAnsatz
				Me.MABasVar = maBasVar
				Me.KDBasis = kdBasis
				Me.MWSTPflichtig = mwstPflcihtig

			End Sub

			Public Property LANr As Decimal
			Public Property TypeAnsatz As Short
			Public Property TypeBasis As Short
			Public Property FixAnsatz As Decimal
			Public Property MABasVar As String
			Public Property KDBasis As String
			Public Property MWSTPflichtig As Boolean

		End Class

#End Region

	End Class

End Namespace
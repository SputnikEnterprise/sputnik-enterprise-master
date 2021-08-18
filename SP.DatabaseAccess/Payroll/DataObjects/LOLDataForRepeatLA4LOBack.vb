Namespace PayrollMng.DataObjects

  Public Class LOLDataFoRepeatLA4LOBack

		Public Property ID As Integer?
		Public Property MDNr As Integer?
		Public Property MANr As Integer?
		Public Property LONr As Integer?
		Public Property LP As Integer?
		Public Property Jahr As Integer?
		Public Property LANr As Decimal?

		Public Property LOLKst1 As String
		Public Property LOLKst2 As String
    Public Property Kst As String
    Public Property m_Anz As Decimal?
    Public Property m_Bas As Decimal?
    Public Property m_Ans As Decimal?
    Public Property m_Btr As Decimal?
    Public Property Suva As String
    Public Property KW As Integer?
    Public Property KW2 As Short?
    Public Property DestRPNr As Integer?
    Public Property DestESNr As Integer?
		Public Property DestKDNr As Integer?
		Public Property RPText As String
		Public Property ModulName As String

		Public Property GAVNr As Integer?
		Public Property GAV_Kanton As String
    Public Property GAV_Beruf As String
    Public Property GAV_Gruppe1 As String
    Public Property GAV_Gruppe2 As String
    Public Property GAV_Gruppe3 As String
    Public Property GAV_Text As String
    Public Property ESEinstufung As String
    Public Property ESBranche As String
		Public Property DateOfLO As DateTime?


		Public ReadOnly Property Periode As String
			Get
				Return String.Format("{0} - {1}", LP, Jahr)
			End Get
		End Property

	End Class

	''' <summary>
	''' Result of customer address assignment (Kunde) deletion.
	''' </summary>
	''' <remarks></remarks>
	Public Enum DeleteLOLForCorrectionAssignmentResult
		ResultDeleteOk = 1
		ResultCanNotDeleteBecauseMonthIsClosed = 2

		ResultDeleteError = 5
	End Enum

End Namespace

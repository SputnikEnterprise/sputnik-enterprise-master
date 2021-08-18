Namespace Report.DataObjects

  ''' <summary>
  ''' RP Data.
  ''' </summary>
  Public Class RPMasterData

    Public Property ID As Integer
    Public Property RPNR As Integer?
    Public Property ESNR As Integer?
    Public Property EmployeeNumber As Integer?
    Public Property CustomerNumber As Integer?
		Public Property ResponsiblePersonNumber As Integer?
		Public Property LONr As Integer?
		Public Property Currency As String
    Public Property SUVA As String
    Public Property Monat As Byte?
    Public Property Jahr As String
    Public Property Von As DateTime?
    Public Property Bis As DateTime?
    Public Property Erfasst As Boolean?
    Public Property Result As String
    Public Property RPKST As String
    Public Property RPKST1 As String
    Public Property RPKST2 As String
    Public Property PrintedWeeks As String
    Public Property PrintedDate As String
    Public Property Farpflicht As Boolean?
    Public Property BVGStd As Decimal?
    Public Property CreatedFrom As String
    Public Property CreatedOn As DateTime?
    Public Property BVGCode As Short?
    Public Property RPGAV_FAG As Decimal?
    Public Property RPGAV_FAN As Decimal?
    Public Property RPGAV_WAG As Decimal?
    Public Property RPGAV_WAN As Decimal?
    Public Property RPGAV_VAG As Decimal?
    Public Property RPGAV_VAN As Decimal?
    Public Property RPGAV_Nr As Integer?
    Public Property RPGAV_Kanton As String
    Public Property RPGAV_Beruf As String
    Public Property RPGAV_Gruppe1 As String
    Public Property RPGAV_Gruppe2 As String
    Public Property RPGAV_Gruppe3 As String
    Public Property RPGAV_Text As String
    Public Property RPGAV_StdWeek As Decimal?
    Public Property RPGAV_StdMonth As Decimal?
    Public Property RPGAV_StdYear As Decimal?
    Public Property RPGAV_FAG_M As Decimal?
    Public Property RPGAV_FAN_M As Decimal?
    Public Property RPGAV_VAG_M As Decimal?
    Public Property RPGAV_VAN_M As Decimal?
    Public Property RPGAV_WAG_M As Decimal?
    Public Property RPGAV_WAN_M As Decimal?
    Public Property RPGAV_FAG_S As Decimal?
    Public Property RPGAV_FAN_S As Decimal?
    Public Property RPGAV_VAG_S As Decimal?
    Public Property RPGAV_VAN_S As Decimal?
    Public Property RPGAV_WAG_S As Decimal?
    Public Property RPGAV_WAN_S As Decimal?
    Public Property RPGAV_FAG_J As Decimal?
    Public Property RPGAV_FAN_J As Decimal?
    Public Property RPGAV_VAG_J As Decimal?
    Public Property RPGAV_VAN_J As Decimal?
    Public Property RPGAV_WAG_J As Decimal?
    Public Property RPGAV_WAN_J As Decimal?
    Public Property ES_Einstufung As String
    Public Property KDBranche As String
    Public Property ProposeNr As Integer?
    Public Property RPDoc_Guid As String
    Public Property MDNr As Integer?

    Public Property IsMonthClosed As Boolean

    Public ReadOnly Property GAVMaximalWorkHoursPerDay As Decimal?
      Get

        If RPGAV_Nr > 0 Then
          Dim stdWorkingHours As Decimal = RPGAV_StdWeek / 5.0
          Return stdWorkingHours
        End If

        Return Nothing
      End Get
    End Property

		Public ReadOnly Property MonthInteger As Integer
			Get
				Return Val(Monat.GetValueOrDefault(0))
			End Get
		End Property

		Public ReadOnly Property YearInteger As Integer
			Get
				If Jahr Is Nothing Then
					Return 0
				Else
					Return Val(Jahr)
				End If

			End Get
		End Property

	End Class

End Namespace

Namespace PayrollMng.DataObjects

  Public Class EmployeeDataForPayroll

    Public Property MANr As Integer
    Public Property Nachname As String
    Public Property Vorname As String
    Public Property Q_Steuer As String
    Public Property Kinder As Short?
    Public Property S_Kanton As String
    Public Property Kirchensteuer As String
    Public Property Bewillig As String
    Public Property Zahlart As String
    Public Property NoLO As Boolean?
    Public Property FerienBack As Boolean?
    Public Property FeierBack As Boolean?
    Public Property Lohn13Back As Boolean?
    Public Property MAGleitzeit As Boolean?
    Public Property InZV As Boolean?
    Public Property Is_Current_LO_Existing As Boolean
    Public Property Is_NonComplete_RP_Existing
    Public Property Is_PreviousMonth_ES_With_No_LO_Existing
    Public Property EmployeeLOProcessState As EmployeeLOProcessState
    Public Property LONr As Integer?

    Public ReadOnly Property Nachname_Vorname As String
      Get
        Return String.Format("{0}, {1}", Nachname, Vorname)
      End Get
    End Property

    Public ReadOnly Property SteuerInfo As String
      Get
        Return String.Format("{0}-{1}-{2}-{3}", S_Kanton, Q_Steuer, Kinder, Kirchensteuer)
      End Get
    End Property

    Public ReadOnly Property IsEmployeeInvalidForPayroll As Boolean

			Get
				Return (NoLO.GetValueOrDefault(0) Or Is_Current_LO_Existing Or
							 Is_NonComplete_RP_Existing Or
							 Is_PreviousMonth_ES_With_No_LO_Existing)	' AndAlso Not Is_Current_LO_Existing

				'Return NoLO.GetValueOrDefault(0) Or Is_Current_LO_Existing Or
				'			 Is_NonComplete_RP_Existing Or
				'			 Is_PreviousMonth_ES_With_No_LO_Existing

			End Get

    End Property

  End Class

  Public Enum EmployeeLOProcessState
    Unprocessed
    InProcessing
    Processed
    Failed
  End Enum

End Namespace
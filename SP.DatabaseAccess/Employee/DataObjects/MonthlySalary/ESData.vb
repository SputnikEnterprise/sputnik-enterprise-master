Namespace Employee.DataObjects.MonthlySalary

  ''' <summary>
  ''' ES data.
  ''' </summary>
  Public Class ESData

    Public Property ESNr As Integer?
    Public Property Company1 As String
    Public Property ES_From As DateTime?
    Public Property ES_To As DateTime?
    Public Property ESKst1 As String
    Public Property ESKst2 As String
    Public Property ESKst As String
    Public Property CustomerNumber As Integer?
		Public Property ESLohnNr As Integer?

    Public Property GAVNr As Integer?
    Public Property GAVKanton As String
    Public Property GAVGruppe0 As String
    Public Property GAVGruppe1 As String
    Public Property GAVGruppe2 As String
    Public Property GAVGruppe3 As String
    Public Property GAVBezeichnung As String
    Public Property Einstufung As String
    Public Property ESBranche As String
    Public Property Suva As String
    Public Property GAV_FAG As Decimal?
    Public Property GAV_FAN As Decimal?
    Public Property GAV_WAG As Decimal?
    Public Property GAV_WAN As Decimal?
    Public Property GAV_VAG As Decimal?
    Public Property GAV_VAN As Decimal?
    Public Property GAV_FAG_S As Decimal?
    Public Property GAV_FAN_S As Decimal?
    Public Property GAV_WAG_S As Decimal?
    Public Property GAV_WAN_S As Decimal?
    Public Property GAV_VAG_S As Decimal?
    Public Property GAV_VAN_S As Decimal?
    Public Property GAV_FAG_M As Decimal?
    Public Property GAV_FAN_M As Decimal?
    Public Property GAV_WAG_M As Decimal?
    Public Property GAV_WAN_M As Decimal?
    Public Property GAV_VAG_M As Decimal?
    Public Property GAV_VAN_M As Decimal?
    Public Property GAV_FAG_J As Decimal?
    Public Property GAV_FAN_J As Decimal?
    Public Property GAV_WAG_J As Decimal?
    Public Property GAV_WAN_J As Decimal?
    Public Property GAV_VAG_J As Decimal?
    Public Property GAV_VAN_J As Decimal?

    Public ReadOnly Property DisplayText As String
      Get
        Return String.Format("{0} - {1}", ESNr, Company1)
      End Get
    End Property

  End Class

End Namespace

Namespace ES.DataObjects.ESMng

  Public Class ESSalaryData

    Public Property ID As Integer
		Public Property ESLohnNr As Integer?
    Public Property ESNr As Integer?
    Public Property EmployeeNumber As Integer?
    Public Property CustomerNumber As Integer?
		Public Property KSTNr As Integer?
    Public Property KSTBez As String
    Public Property GavText As String
    Public Property GrundLohn As Decimal?
    Public Property StundenLohn As Decimal?
    Public Property FerBasis As Decimal?
    Public Property Ferien As Decimal?
    Public Property FerienProz As Decimal?
    Public Property Feier As Decimal?
    Public Property FeierProz As Decimal?
    Public Property Basis13 As Decimal?
    Public Property Lohn13 As Decimal?
    Public Property Lohn13Proz As Decimal?
    Public Property Tarif As Decimal?
    Public Property MAStdSpesen As Decimal?
    Public Property MATSpesen As Decimal?
    Public Property KDTSpesen As Decimal?
    Public Property MATotal As Decimal?
    Public Property KDTotal As Decimal?
    Public Property MWSTBetrag As Decimal?
    Public Property BruttoMarge As Decimal?
    Public Property LOVon As DateTime?
    Public Property LOBis As DateTime?
    Public Property Result As String
    Public Property AktivLODaten As Boolean?
    Public Property MargeMitBVG As Decimal?
    Public Property GAVNr As Integer?
    Public Property GAVKanton As String
    Public Property GAVGruppe0 As String
    Public Property GAVGruppe1 As String
    Public Property GAVGruppe2 As String
    Public Property GAVGruppe3 As String
    Public Property GAVBezeichnung As String
    Public Property GAV_FAG As Decimal?
    Public Property GAV_FAN As Decimal?
    Public Property GAV_WAG As Decimal?
    Public Property GAV_WAN As Decimal?
    Public Property GAV_VAG As Decimal?
    Public Property GAV_VAN As Decimal?
    Public Property GAV_StdWeek As Decimal?
    Public Property GAV_StdMonth As Decimal?
    Public Property GAV_StdYear As Decimal?
    Public Property GAVStdLohn As Decimal?
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
    Public Property FerienWay As Short?
    Public Property LO13Way As Short?
    Public Property GAV_FAG_J As Decimal?
    Public Property GAV_FAN_J As Decimal?
    Public Property GAV_WAG_J As Decimal?
    Public Property GAV_WAN_J As Decimal?
    Public Property GAV_VAG_J As Decimal?
    Public Property GAV_VAN_J As Decimal?
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property VerleihDoc_Guid As String
    Public Property ESDoc_Guid As String
    Public Property Transfered_User As String
    Public Property Transfered_On As DateTime?
    Public Property IsPVL As Byte?
    Public Property FeierBasis As Decimal?
    Public Property GAVInfo_String As String
    Public Property LOFeiertagWay As Byte
    Public Property GavDate As DateTime?
    Public Property MargenInfo_String As String
		Public Property PVLDatabaseName As String
		Public Property FARPflichtig As Boolean?

		Public ReadOnly Property Erfasst As String
      Get
        Return String.Format("{0:dd.MM.yyyy HH:ss} /{1}", CreatedOn, CreatedFrom)
      End Get
    End Property

    Public ReadOnly Property Geandert As String
      Get
        Return String.Format("{0:dd.MM.yyyy HH:ss} /{1}", ChangedOn, ChangedFrom)
      End Get
    End Property

    Public ReadOnly Property FormatString1 As String
      Get
        Return String.Format("{0:dd.MM.yyyy}-G:{1:n2}-Std:{2:n2}-T:{3:n2}", LOVon, GrundLohn, StundenLohn, Tarif)
      End Get
    End Property

    Public Property MargeOhneBVGInProzent As Decimal
   

    Public Property MargeMitBVGInProzent As Decimal


  End Class


End Namespace

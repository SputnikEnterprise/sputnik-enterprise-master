Namespace ES.DataObjects.ESMng

  Public Class ESMasterData

    Public Property ID As Integer
    Public Property ESNR As Integer?
    Public Property EmployeeNumber As Integer?
    Public Property CustomerNumber As Integer?
    Public Property KSTBez As String
    Public Property ESKst As String
    Public Property Arbzeit As String
    Public Property Arbort As String
    Public Property Melden As String
    Public Property ES_Als As String
    Public Property ES_Ab As DateTime?
    Public Property ES_Uhr As String
    Public Property ES_Ende As DateTime?
    Public Property Ende As String
    Public Property GAVText As String
    Public Property Bemerk_MA As String
    Public Property Bemerk_KD As String
    Public Property Bemerk_RE As String
    Public Property Bemerk_Lo As String
    Public Property Bemerk_P As String

    Public Property dismissalon As DateTime?
    Public Property dismissalfor As DateTime?
    Public Property dismissalkind As String
    Public Property dismissalreason As String
    Public Property dismissalwho As String

    Public Property RP_Art As String
    Public Property LeistungsDoc As String
    Public Property MWST As String
    Public Property SUVA As String
    Public Property Currency As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String
    Public Property CreatedKST As String
    Public Property ChangedOn As DateTime?
    Public Property ChangedFrom As String
    Public Property ChangedKST As String
    Public Property Result As String
    Public Property KDZustaendig As String
    Public Property ESKST1 As String
    Public Property ESKST2 As String
    Public Property ESUnterzeichner As String
    Public Property VerleihBacked As Boolean?
    Public Property Bemerk_1 As String
    Public Property Bemerk_2 As String
    Public Property Bemerk_3 As String
    Public Property Print_KD As Boolean?
    Public Property Print_MA As Boolean?
    Public Property FarPflichtig As Boolean?
    Public Property ESVerBacked As Boolean?
    Public Property NoListing As Boolean?
    Public Property BVGCode As Short?
    Public Property Einstufung As String
    Public Property ESBranche As String
    Public Property GoesLonger As String
    Public Property ProposeNr As Integer?
    Public Property VakNr As Integer?
    Public Property PNr As Integer?
    Public Property KDZHDNr As Integer?
    Public Property MDNr As Integer?
    Public Property PrintNoRP As Boolean?

  End Class

	Public Class EmploymentDependentData

		Public Property LONr As Integer?
		Public Property RENr As Integer?
		Public Property LANr As Decimal?
		Public Property VerleihDoc_Guid As String
		Public Property ESDoc_Guid As String
		Public Property RPDoc_Guid As String


	End Class


End Namespace
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Employee
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.Infrastructure.Misc
Imports SP.DatabaseAccess

Partial Public Class EmployeePayroll

  Private m_EmployeeData As EmployeeMasterData
  Private m_EmployeeLOSetting As EmployeeLOSettingsData

  Private RentFrei_Monat_ans As Decimal
  Private AHV_AN_ans As Decimal
  Private AHV_2_AN_ans As Decimal
  Private ALV1_HL__ans As Decimal
  Private ALV2_HL__ans As Decimal
  Private ALV_AN_ans As Decimal
  Private ALV2_An_ans As Decimal

  Private SUVA_HL__ans As Decimal
  Private NBUV_M_ans As Decimal
  Private NBUV_M_Z_ans As Decimal
  Private NBUV_W_ans As Decimal
  Private NBUV_W_Z_ans As Decimal

  Private KK_An_MA_ans As Decimal
  Private KK_An_MZ_ans As Decimal
  Private KK_An_WA_ans As Decimal
  Private KK_An_WZ_ans As Decimal
  Private AHV_AG_ans As Decimal
  Private AHV_2_AG_ans As Decimal
  Private ALV_AG_ans As Decimal
  Private ALV2_AG_ans As Decimal

  Private Suva_A_ans As Decimal
  Private Suva_Z_ans As Decimal
	Private UVGZ_A_ans As Decimal
	Private UVGZ_B_ans As Decimal
	Private UVGZ2_A_ans As Decimal
	Private UVGZ2_B_ans As Decimal

	Private KK_AG_MA_ans As Decimal
  Private KK_AG_MZ_ans As Decimal
  Private KK_AG_WA_ans As Decimal
  Private KK_AG_WZ_ans As Decimal
  Private Fak_Proz_ans As Decimal

  Private CheckGebuehrZG_ As Decimal
  Private CheckGebuehrAuszahlung_ As Decimal
  Private UeberweisungGebuehrZG_ As Decimal
  Private BarGebuehrZG_ As Decimal
  Private UeberweisungGebuehrZGAusland_ As Decimal
  Private UeberweisungGebuehrZGAuslandLO_ As Decimal

  Public Property cBetragKIZulage As Decimal
  Public Property cBetragAuZulage As Decimal

  Private bIsKTGAsZ As Boolean

  Private Const Anhang1GAV = "(365001, 380004, 390001, 425002, 425003, 425004, 425005, 425007, " & _
                              "425008, 425010, 425011, 445001, 565006, 705012, 750015, 903001, 903003, " & _
                              "903004, 903005, 903007, 903008, 903009, 903015, 903016, 905003, 905004, " & _
                              "905009, 905010, 905011, 909001, 909003, 909005, 942501, )"

  Private bIs60KTGDay As Boolean     ' if true then 60 Days; if false then 720 Versicherung
  Private bIs60KTGDayOK As Boolean     ' if true then 60 Days; if false then 720 Versicherung
  Public Property iESLP4QST As Integer
  Public Property strOriginData As String

  Private AllRPLOrec As IEnumerable(Of EmployeeRPLDataForLOCreation)
  Private AllLMLOrec As IEnumerable(Of EmployeeLMDataForLOCreation)
  Private AllZGLOrec As IEnumerable(Of EmployeeZGDataForPayroll)

  Private MDNr As Integer
  Private LPYear As Integer          ' Das aktuelle Jahr
  Private LPMonth As Integer      ' Der aktuelle Monat

	Private ESLPTage As Integer          ' Gearbeitete Tage im aktuellen Monat
	Private ESYearTage As Integer           ' Gearbeitete Tage bis und mit aktuellem Monat
	Private DONotShowAgainQSTForm As Boolean?

	Private m_Calculatebvgwithesdays As Boolean
	Private m_BVGInterval As String
	Private m_BVGAfter As Integer
	Private m_iESBreakWeek As Integer



	Public Property LONewNr As Integer            ' Nummer der Lohnabrechnung

  Private strQstGemeinde As String

  Private Kostenstelle1 As String       ' 1. Kostenstelle des Mitarbeiters
  Private Kostenstelle2 As String       ' 2. Kostenstelle des Mitarbeiters
  Private Kostenstelle3 As String       ' 3. Kostenstelle des Mitarbeiters
  Public Property QSTTarif As String           ' Bezeichnung für Quellensteuertarife
  Private LpDate As Date

  Private IsRentner As Boolean
  Private IsToYoung As Boolean

  Private U(50) As Decimal            ' Variable für Unterstellung
  Private S(70) As Decimal            ' Summen Variable
  Private G(70) As Decimal            ' Summen für GAV

  Private Div(10) As Decimal          ' Variable für Bearbeitung

  ' die Variable für GAV's
  Private cFAGBasis As Decimal
  Private cFANBasis As Decimal
  Private cWAGBasis As Decimal
  Private cWANBasis As Decimal
  Private cVAGBasis As Decimal
  Private cVANBasis As Decimal

  Private cGAVKTGAGBasis As Decimal
  Private cGAVKTGANBasis As Decimal
  'Private cGAVKTGAGBasis60 As Currency
  'Private cGAVKTGANBasis60 As Currency

  Private aGAVKanton(26) As String
	Private aGAVPVLNumber(50) As Integer
	Private aGAVBerufe(50) As String
	Private aGGruppe1(26) As String

	Private aFeiertag(50) As Object
  Private aFerien(50) As Object
  Private a13Lohn(50) As Object

  Private aGAVFAG(50) As Decimal
  Private aGAVFAN(50) As Decimal
  Private aGAVWAG(50) As Decimal
  Private aGAVWAN(50) As Decimal
  Private aGAVVAG(50) As Decimal
  Private aGAVVAN(50) As Decimal

  Private aGAVKKAG(50) As Decimal
  Private aGAVKKAN(50) As Decimal
  'Private aGAVKKAG60(50) As Currency
  'Private aGAVKKAN60(50) As Currency

  Private aGComplettStd As New GAVDataPerNumberCantonAndGroup
  Private aGComplettFAG As New GAVDataPerNumberAndCanton
  Private aGComplettFAN As New GAVDataPerNumberAndCanton
  Private aGComplettWAG As New GAVDataPerNumberCantonAndGroup
  Private aGComplettWAN As New GAVDataPerNumberCantonAndGroup
  Private aGComplettVAG As New GAVDataPerNumberCantonAndGroup
  Private aGComplettVAN As New GAVDataPerNumberCantonAndGroup

  Private aGComplettWAGStd As New GAVDataPerNumberCantonAndGroup
  Private aGComplettWANStd As New GAVDataPerNumberCantonAndGroup
  Private aGComplettVAGStd As New GAVDataPerNumberCantonAndGroup
  Private aGComplettVANStd As New GAVDataPerNumberCantonAndGroup

  Private aGComplettWAGM As New GAVDataPerNumberCantonAndGroup
  Private aGComplettWANM As New GAVDataPerNumberCantonAndGroup
  Private aGComplettVAGM As New GAVDataPerNumberCantonAndGroup
  Private aGComplettVANM As New GAVDataPerNumberCantonAndGroup

  Private aGComplettGleitBetrag As New GAVDataPerNumberCantonAndGroup
  Private aGComplettGleitStd As New GAVDataPerNumberCantonAndGroup

  Private aGComplettKKAG As New GAVDataPerNumber
  Private aGComplettKKAN As New GAVDataPerNumber

  Private aGAVStdAnzGleit As New GleizeitDataPerGAVNumber

  Private TempSuvaCode As String
	Private m_IsBackBrutto As Boolean

	Private LOLAnzahl As Decimal
  Private LOLBasis As Decimal
  Private LOLAnsatz As Decimal
  Private LOLBetrag As Decimal

  Public Property strESData As String
  Public Property strQSTKanton As String

  Private dFoundedBVGVon As Date?                 ' BVG-Beginn
  Private dFoundedBVGBis As Date?                 ' BVG-Ende

  Private currRplData As EmployeeRPLDataForLOCreation
  Private currLMData As EmployeeLMDataForLOCreation
  Private currZGData As EmployeeZGDataForPayroll

  Private m_RPRecordsToUpdate As New List(Of Integer)
  Private m_ZGRecordsToUpdate As New List(Of Integer)

  Private SetEmployeeLOBackSetting As Boolean

  Private BVGBeginForLO As DateTime?
  Private BVGEndForLO As DateTime?
	Private m_BVGDateData As String

End Class

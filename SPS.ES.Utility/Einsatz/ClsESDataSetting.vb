
Public Class ClsESDataSetting

  Public Property DbConnString2Open As String

  Public Property SelectedYear As Integer
  Public Property SelectedMonth As Short

  Public Property SelectedESNr As Integer
  Public Property SelectedESLohnNr As Integer
	Public Property selectedMDNr As Integer
	Public Property SelectedMANr As Integer
	Public Property SelectedKDNr As Integer

	Public Property SelectedESVertragGuid As String
  Public Property SelectedVerleihGuid As String

  Public Property ShowMsgBox As Boolean

  ' Variable für Margenberechnung
  Public Property ShowMargeWithBVG As Boolean
  Public Property aDebugMargenCalculation As New Dictionary(Of String, String)

  ' wenn der Einsatz neu ist...
  Public Property GetMarge4NewES As Boolean
  Public Property _dES_Ab As Date

  Public Property _sbLohn As Single
  Public Property _sMASSpesen As Single
  Public Property _sMATSpesen As Single
  Public Property _sKDTarif As Single
  Public Property _sKDTSpesen As Single

  Public Property _sFARProz As Single
  Public Property _sWAGProz As Single
  Public Property _sVAGProz As Single
  Public Property _sWAGBtr As Single
  Public Property _sVAGBtr As Single

  Public Property _strSuva As String
  Public Property _strGAVInfo As String

End Class

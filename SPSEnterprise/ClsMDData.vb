
Public Class ClsMDData

  Public RootDbServer As String
  Public RootDbName As String
  Public RootDbConn As String

  Public MDNr As Integer
  Public MDGroupNr As Integer
  Public MDYear As Integer

  Public MDName As String
  Public MDGuid As String

  Public MDMainPath As String
  Public MDDbServer As String
  Public MDDbName As String
  Public MDDbConn As String

  Public ReadOnly Property SQLDateFormat As String
    Get
      Dim _ClsReg As New ClsDivReg

      Dim strFormat As String = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", "DBServer")
      If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"

      Return strFormat
    End Get
  End Property

End Class



Public Class ClsLogingMDData

	Public Property RootDbServer As String
	Public Property RootDbName As String
	Public Property RootDbConn As String

	Public Property RecNr As Integer
	Public Property MDNr As Integer
	Public Property MDGroupNr As Integer
	Public Property MDYear As Integer

	Public Property MDName As String
	Public Property MDGuid As String

	Public Property MDMainPath As String
	Public Property MDDbServer As String
	Public Property MDDbName As String
	Public Property MDDbConn As String

	Public ReadOnly Property SQLDateFormat As String
		Get
			Dim _ClsReg As New ClsDivReg

			Dim strFormat As String = _ClsReg.GetINIString(SrvSettingFullFileName, "Customer", "DBServer")
			If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"

			Return strFormat
		End Get
	End Property

End Class


Public Enum LoginResult

	loginok
	loginfailed
	tryfailed

End Enum


Public Class DeleteData
	Public Property PayrollNumber As Integer
	Public Property ExportedFilename As String

End Class


Public Class ClsLODataSetting

  Public Property DbConnString2Open As String
	Public Property MDNr As Integer

  Public Property SelectedYear As Integer
  Public Property SelectedMonth As Short

  Public Property SelectedLONr As Integer
  Public Property SelectedMANr As Integer
  Public Property SelectedZGNr As Integer
  Public Property SelectedLMNr As Integer

  Public Property SelectedLOGuid As String
  Public Property SelectedMAGuid As String

  Public Property ShowMsgBox As Boolean
  Public Property liNotDeletedZGNr As New List(Of Integer)

End Class


Public Class FoundedAdvancePaymentData

	Public Property recid As Integer
	Public Property zgnr As Integer
	Public Property MANr As Integer

	Public Property employeelastname As String
	Public Property employeefirstname As String
	Public Property ZGGrund As String

	Public Property lp As Integer
	Public Property jahr As Integer
	Public Property Aus_Dat As DateTime?

	Public Property betrag As Decimal


	Public ReadOnly Property LastnameFirstname As String
		Get
			Return String.Format("{0}, {1}", employeelastname, employeefirstname)
		End Get
	End Property

	Public ReadOnly Property Zeitraum As String
		Get
			Return String.Format("{0:00} - {1}", lp, jahr)
		End Get
	End Property

End Class



Imports System.IO.File

Public Class ClsMain_Net
  Public Shared frmTest As frmCalculator
  Dim _ClsReg As New SPProgUtility.ClsDivReg



#Region "Interne Funktionen"



	Function GetUserID(ByVal strIDNr As String) As String
    Dim strResult As String = "7BDD25A4FBB1A9AA5FA5DD9BC8BB6546EFF712FAE3B8C01E20241B03EC9697F04E2DB80838B3F33C"

		Return strResult
  End Function



#End Region


#Region "Startfunktionen..."

	Sub ShowfrmTarifCalculator()

    frmTest = New frmCalculator
    frmTest.Show()

  End Sub


  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

  Public Sub New()

    Application.EnableVisualStyles()

  End Sub

#End Region

End Class

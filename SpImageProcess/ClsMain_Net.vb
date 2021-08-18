
Imports System.IO.File

Public Class ClsMain_Net

  Public Shared frmTest As frmMAPhoto
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsEventLog As New SPProgUtility.ClsEventLog


#Region "Interne Funktionen"

  Function GetAppGuidValue() As String
    Return "83B4376F-D6FB-4060-AAF9-0D4D076FE2A5"
  End Function


#End Region


#Region "Startfunktionen..."

  Function ShowfrmMAPhoto(ByVal iMANr As Integer, ByVal strFileName As String) As String
    _ClsEventLog.WriteMainLog("ShowfrmMAPhoto")

    'If strFileName = String.Empty Then strFileName = _ClsSystem.StoreSelectedMAPhoto2FS(iMANr)

    'ClsDataDetail.strNewFileName = strFileName
    frmTest = New frmMAPhoto(iMANr)
    frmTest.ShowDialog()

    Return ClsDataDetail.strNewFileName
  End Function


  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

  Public Sub New()

    Application.EnableVisualStyles()

  End Sub

#End Region

End Class

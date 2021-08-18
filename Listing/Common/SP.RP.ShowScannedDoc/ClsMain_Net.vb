

Public Class ClsMain_Net
  Implements IDisposable
  Protected disposed As Boolean = False


#Region "Startfunktionen..."

  Sub ShowfrmScanOneRP(ByVal iMANr As Integer, ByVal iKDNr As Integer, ByVal iESNr As Integer, _
                       ByVal iRPNr As Integer, ByVal iRPLNr As Integer, ByVal sKW As Short)

		'Dim frmTest = New frmRPDocScan(iMANr, iKDNr, iESNr, iRPNr, iRPLNr, sKW)
		'  frmTest.Show()

  End Sub

	'Function getMainform() As System.Windows.Forms.Control
	''Dim frmTest = New frmRPDocScan()

	''  Return frmTest.xtabControl
	'End Function

	Sub ShowfrmScanMoreRP()

		'Dim frmTest = New frmRPDocScan
		'  frmTest.Show()

  End Sub

  Public Sub New()
    Application.EnableVisualStyles()
  End Sub

#End Region


  Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
    If Not Me.disposed Then
      If disposing Then
        'frmTest.Dispose()
      End If
      ' Add code here to release the unmanaged resource.

      ' Note that this is not thread safe.
    End If
    Me.disposed = True
  End Sub

#Region " IDisposable Support "
  ' Do not change or add Overridable to these methods.
  ' Put cleanup code in Dispose(ByVal disposing As Boolean).
  Public Overloads Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Protected Overrides Sub Finalize()
    Dispose(False)
    MyBase.Finalize()
  End Sub
#End Region

End Class

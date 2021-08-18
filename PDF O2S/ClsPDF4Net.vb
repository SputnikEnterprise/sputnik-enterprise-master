
Imports O2S.Components.PDF4NET.PDFFile

Public Class ClsPDF4Net
  Implements IDisposable
  Protected disposed As Boolean = False


  Public ReadOnly Property GetPDF4Net_O2SSerial() As String
    Get
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Return _ClsProgSetting.GetPDF_O2SSerial	' "yourlicencekey"
    End Get
  End Property

  ''' <summary>
  ''' fügt die 2 PDF-Dateien zu einer Datei
  ''' </summary>
  ''' <param name="strFinalFile">die endgültige Datei</param>
  ''' <param name="aPDFFilename">die Dateien</param>
  ''' <returns>das Resultat </returns>
  ''' <remarks></remarks>
  Function Merg2PDFFiles(ByVal strFinalFile As String, ByVal aPDFFilename() As String) As String
    Dim strResult As String = "Erfolgreich"

    Try
      PDFFile.SerialNumber = Me.GetPDF4Net_O2SSerial
      PDFFile.MergeFiles(strFinalFile, aPDFFilename, True)

    Catch ex As Exception
			strResult = String.Format("Fehler: {0}", ex.ToString)

    End Try


    Return strResult
  End Function

  Function SplitSeletedPDFFile(ByVal strSource As String, ByVal strDest As String) As String
    Dim strResult As String = "Erfolgreich"

    Try
      'Dim strFile As O2S.Components.PDFRender4NET.PDFFile = O2S.Components.PDFRender4NET.PDFFile.Open(strSource)
      Dim strFile As O2S.Components.PDFRender4NET.PDFFile = O2S.Components.PDFRender4NET.PDFFile.Open(strSource)
      strFile.SerialNumber = Me.GetPDF4Net_O2SSerial

      O2S.Components.PDF4NET.PDFFile.PDFFile.SplitFile(strSource, strDest)
      strFile.Dispose()

    Catch ex As Exception
			strResult = String.Format("Fehler: {0}", ex.ToString)

    Finally

    End Try

    Return strResult
  End Function

  Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
    If Not Me.disposed Then
      If disposing Then

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

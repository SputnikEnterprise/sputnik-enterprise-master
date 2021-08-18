
Imports System
Imports System.Drawing
Imports System.Text
Imports System.IO

Imports System.Security.Cryptography
Imports System.Drawing.Printing
Imports System.Drawing.Imaging
Imports O2S.Components.PDFRender4NET
Imports O2S.Components.PDFRender4NET.Printing

Imports DevExpress.DXperience.Demos.TutorialControlBase
Imports DevExpress.XtraEditors
Imports DevExpress.Utils

Imports SP.Infrastructure.Logging

Public Class ClsMain_Net
  Implements IDisposable
  Protected disposed As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Private WaitWindow As DevExpress.Utils.WaitDialogForm

  ReadOnly Property GetPDFVW_O2SSerial() As String
    Get
      Return _ClsProgSetting.GetPDFVW_O2SSerial '"yourlicencekey"
    End Get
  End Property

  ''' <summary>
  ''' Konvertiert eine PDF-Datei in einer Grafikdatei.
  ''' </summary>
  ''' <param name="strSourceFullFileName"></param>
  ''' <param name="strDestFullFileName"></param>
  ''' <param name="iDestFormat"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function ConvertPDFToGrafik(ByVal strSourceFullFileName As String, _
                                     ByVal strDestFullFileName As String, _
                                     ByVal iDestFormat As Integer) As String
    Dim strDestFile As String = strDestFullFileName

    If File.Exists(strSourceFullFileName) Then
      Try
        Dim strFile As O2S.Components.PDFRender4NET.PDFFile = O2S.Components.PDFRender4NET.PDFFile.Open(strSourceFullFileName)
        strFile.SerialNumber = Me.GetPDFVW_O2SSerial
        Dim iPagecount As Integer = 0 ' If(strFile.PageCount - 1 > 5, 5, strFile.PageCount - 1)

        Try

          For i As Integer = 0 To iPagecount 'strFile.PageCount - 1
            Dim pageImage As Bitmap = strFile.GetPageImage(i, 96)
            strDestFullFileName = String.Format("{0}", strDestFullFileName)

            If iDestFormat = 0 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Bmp)
            ElseIf iDestFormat = 1 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Emf)
            ElseIf iDestFormat = 2 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Exif)
            ElseIf iDestFormat = 3 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Gif)
            ElseIf iDestFormat = 4 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Icon)

            ElseIf iDestFormat = 5 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Jpeg)
            ElseIf iDestFormat = 6 Then
              pageImage.Save(strDestFullFileName, ImageFormat.MemoryBmp)
            ElseIf iDestFormat = 7 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Png)
            ElseIf iDestFormat = 8 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Tiff)
            ElseIf iDestFormat = 9 Then
              pageImage.Save(strDestFullFileName, ImageFormat.Wmf)

            End If
          Next i


        Catch ex As Exception
          strDestFile = String.Format("Fehler (ConvertPDFToGrafik): {0}", ex.Message)

        Finally
          strFile.Dispose()

        End Try

      Catch ex As Exception
        strDestFile = String.Format("Fehler (ConvertPDFToGrafik): {0}", ex.Message)

      End Try

    End If

    Return strDestFile
  End Function

  ''' <summary>
  ''' Konvertiert eine PDF-Datei in einer Grafik-Datei um.
  ''' </summary>
  ''' <param name="strSourceFullFileName"></param> die PDF-Datei
  ''' <param name="strDestFullFileName"></param> Ohne Erweiterung!!!
  ''' <param name="iFPage"></param> Startseite x
  ''' <param name="iLPage"></param> letzte Seite x
  ''' <param name="imgEx"></param> Format der Export-Datei
  ''' <returns></returns> wird in einem Array ausgegeben
  ''' <remarks></remarks>
  Public Function ConvertPDFToGrafikAsArray(ByVal strSourceFullFileName As String, _
                                            ByVal strDestFullFileName As String, _
                                            ByVal iFPage As Integer, _
                                            ByVal iLPage As Integer, _
                                            ByVal imgEx As ImageFormat) As List(Of String)
    Dim strDestFile As New List(Of String)
    Dim strOrgDes As String = strDestFullFileName

    If File.Exists(strSourceFullFileName) Then
      Try
        Dim strExtension As String = "JPG"
        Dim strFile As O2S.Components.PDFRender4NET.PDFFile = O2S.Components.PDFRender4NET.PDFFile.Open(strSourceFullFileName)

        strFile.SerialNumber = Me.GetPDFVW_O2SSerial
        If iLPage > strFile.PageCount - 1 Or iLPage = 0 Then iLPage = strFile.PageCount - 1

        Try

          If imgEx Is ImageFormat.Bmp Then
            strExtension = "bmp"
          ElseIf imgEx Is ImageFormat.Emf Then
            strExtension = "emf"
          ElseIf imgEx Is ImageFormat.Exif Then
            strExtension = "exif"
          ElseIf imgEx Is ImageFormat.Gif Then
            strExtension = "gif"
          ElseIf imgEx Is ImageFormat.Icon Then
            strExtension = "ico"
          ElseIf imgEx Is ImageFormat.Jpeg Then
            strExtension = "jpg"
          ElseIf imgEx Is ImageFormat.MemoryBmp Then
            strExtension = "bmp"
          ElseIf imgEx Is ImageFormat.Png Then
            strExtension = "png"
          ElseIf imgEx Is ImageFormat.Tiff Then
            strExtension = "tiff"
          ElseIf imgEx Is ImageFormat.Wmf Then
            strExtension = "wmf"
          End If

          For i As Integer = iFPage To iLPage
            Dim pageImage As Bitmap = strFile.GetPageImage(i, 96)

            strDestFullFileName = String.Format("{0}_{1}.{2}", strOrgDes, i, strExtension)
            pageImage.Save(strDestFullFileName, imgEx) ' ImageFormat.Bmp)

            strDestFile.Add(strDestFullFileName)
          Next i
          'strDestFullFileName = "C:\Path\Test As Tiff_2"
          'strFile.GetPageImagesAsMultipageTiff(strDestFullFileName, 96)



        Catch ex As Exception
          strDestFile.Add(String.Format("Fehler (ConvertPDFToGrafik_1): {0}", ex.Message))

        Finally
          strFile.Dispose()

        End Try

      Catch ex As Exception
        strDestFile.Add(String.Format("Fehler (ConvertPDFToGrafik_0): {0}", ex.Message))

      End Try

    End If

    Return strDestFile
  End Function

  Function PrintSelectedPDFFile(ByVal strFilename As String, ByVal bShowPrintDialog As Boolean) As String
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim strResult As String = "Erfolgreich"
    Dim bPrint As Boolean = True
    Dim strMsg As String = _ClsProgSetting.TranslateText("Ihr Dokument wird geöffnet...")
    WaitWindow = New DevExpress.Utils.WaitDialogForm(strMsg, _
                                                     _ClsProgSetting.TranslateText("Bitte warten Sie einen Augenblick..."))

    Try

      Dim file As PDFFile = PDFFile.Open(strFilename)
      file.SerialNumber = Me.GetPDFVW_O2SSerial
      ' Create a default printer settings to print on the default printer.
      Dim settings As New PrinterSettings()
      Dim pdfPrintSettings As New PDFPrintSettings(settings)
      pdfPrintSettings.PageScaling = PageScaling.None ' .FitToPrinterMargins
      WaitWindow.Show()

      Try
        If bShowPrintDialog Then
          Dim dlg As New PrintDialog
          If dlg.ShowDialog() = DialogResult.OK Then
            settings.PrinterName = dlg.PrinterSettings.PrinterName
            settings.DefaultPageSettings.PaperSource = dlg.PrinterSettings.DefaultPageSettings.PaperSource
            settings.DefaultPageSettings.Color = dlg.PrinterSettings.DefaultPageSettings.Color
          Else
            bPrint = False
          End If
        End If
        WaitWindow.Close()

        strMsg = _ClsProgSetting.TranslateText("Ihr Dokument wird gedruckt...")
        WaitWindow = New DevExpress.Utils.WaitDialogForm(strMsg, _ClsProgSetting.TranslateText("Bitte warten Sie einen Augenblick..."))
        WaitWindow.Show()
        If bPrint Then file.Print(pdfPrintSettings)

      Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Print{1}", strMethodeName, ex.ToString))

      End Try
      file.Dispose()


    Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
      strResult = String.Format("Fehler: {0}", ex.Message)

    Finally
      WaitWindow.Close()

    End Try


    Return strResult
  End Function

  Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
    If Not Me.disposed Then
      If disposing Then
        WaitWindow.Dispose()
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

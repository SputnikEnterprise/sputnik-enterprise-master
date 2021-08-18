Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraPrinting.BarCode
Imports System.Drawing.Printing

''' <summary>
''' Utility Klasse zum Drucken von Barcodes.
''' <author>egle</author>
''' </summary>
''' <remarks></remarks>
Public Class BarcodeUtility

#Region "Public Shared Methods"

  ''' <summary>
  ''' Zeigt einen Barcode im Vorschaufenster.
  ''' </summary>
  ''' <param name="barcodeText"></param>
  ''' <remarks></remarks>
  Public Shared Sub ShowBarcodePreviewDialog(barcodeText As String)
    Dim barcodeReport As New BarcodeReport()
    barcodeReport.ShowPreviewDialog(barcodeText)
  End Sub

  ''' <summary>
  ''' Druckt einen Barcode aus.
  ''' </summary>
  ''' <param name="barcodeText"></param>
  ''' <param name="printerName">Druckername oder Standarddrucker falls nicht übergeben</param>
  ''' <returns>True wenn erfolgreich</returns>
  ''' <remarks></remarks>
  Public Shared Function PrintBarcode(barcodeText As String, Optional ByVal printerName As String = Nothing) As Boolean
		Dim success As Boolean = True
		Dim barcodeReport As New BarcodeReport()

		success = barcodeReport.Print(barcodeText, printerName)

		Return success

  End Function

#End Region ' Public Shared Methods


#Region "Nested Classes"

  ''' <summary>
  ''' Hilfsklasse zum Erstellen eines Barcode Reports.
  ''' Vorläufig wird fix ein DateMatrix Barcode erstellt.
  ''' <author>egle</author>
  ''' </summary>
  ''' <remarks></remarks>
  Private Class BarcodeReport

    ''' <summary>
    ''' Konstruktor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
			m_report = New XtraReport()

			m_report.ShowPrintMarginsWarning = False
			m_report.PaperKind = System.Drawing.Printing.PaperKind.Custom
			m_report.ReportUnit = ReportUnit.TenthsOfAMillimeter

			m_report.PageSize = New System.Drawing.Size With {.Height = 230, .Width = 230}
			m_report.Margins = New System.Drawing.Printing.Margins With {.Bottom = 0, .Left = 0, .Right = 0, .Top = 0}
			m_report.PaperName = "23mm x 23mm"

			AddHandler m_report.BeforePrint, AddressOf m_report_BeforePrint

    End Sub

    ''' <summary>
    ''' Zeigt den Barcode Report im Preview Fenster.
    ''' </summary>
    ''' <param name="barcodeText"></param>
    ''' <remarks></remarks>
    Public Sub ShowPreviewDialog(ByVal barcodeText As String)
      m_barcodeText = barcodeText
			Try
				Dim rpt As New ReportPrintTool(m_report)
				rpt.ShowPreviewDialog()


				'm_report.ShowPreviewDialog()
			Catch ex As Exception
			End Try
    End Sub

    ''' <summary>
    ''' Druckt den Barcode Report aus.
    ''' </summary>
    ''' <param name="barcodeText"></param>
    ''' <param name="printerName">Druckername oder Standarddrucker falls nicht übergeben</param>
    ''' <returns>True wenn erfolgreich</returns>
    ''' <remarks></remarks>
    Public Function Print(ByVal barcodeText As String, Optional ByVal printerName As String = Nothing) As Boolean
      m_barcodeText = barcodeText
      Dim success As Boolean
      Try
        If String.IsNullOrEmpty(printerName) Then
          m_report.Print()
        Else
          m_report.Print(printerName)
        End If
        success = True
      Catch ex As Exception
        success = False
      End Try
      Return success
    End Function

		Private Sub m_report_BeforePrint()
			' Barcode erstellen
			barCode = New DevExpress.XtraReports.UI.XRBarCode()
			barCode = CreateDataMatrixBarcode(m_barcodeText)

			barCode.ShowText = False
			Dim banddetail = New DetailBand()
			m_report.Bands.Add(banddetail)
			banddetail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {barCode})

			barCode.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
			barCode.SizeF = New Drawing.SizeF(230, 230)
			barCode.AutoModule = False


			' Detail Band erstellen und Barcode hizufügen.
			'CType(sender, XtraReport).Bands.Add(New DetailBand())
			'CType(sender, XtraReport).Bands(BandKind.Detail).Controls.Add(barCode)

		End Sub

    ''' <summary>
    ''' Erstellt einen ECC200 Data Matrix Barcode.
    ''' </summary>
    ''' <param name="BarCodeText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function CreateDataMatrixBarcode(ByVal barCodeText As String) As XRBarCode
      ' Barcode Control erstellen
			Dim barcode As New XRBarCode()

      ' Barcode Typ DataMatrix setzen
      barcode.Symbology = New DataMatrixGenerator()

      ' Barcode Haupt-Eigenschaften setzen
      barcode.Text = barCodeText

			' Adjust the properties specific to the bar code type.
			CType(barcode.Symbology, DataMatrixGenerator).CompactionMode = DataMatrixCompactionMode.ASCII
			CType(barcode.Symbology, DataMatrixGenerator).MatrixSize = DataMatrixSize.Matrix22x22

      Return barcode
    End Function

    Private m_report As XtraReport
    Private m_barcodeText As String
		Private barCode As DevExpress.XtraReports.UI.XRBarCode

  End Class

#End Region ' Nested Classes

End Class

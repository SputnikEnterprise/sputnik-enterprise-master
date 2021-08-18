
Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrinting.Preview
Imports DevExpress.XtraReports.UI



Public Class frmReportViewer


#Region "Private Fields"

	Private m_dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing)

	Private filePath As String = "C:\Path\XtraReport.repx"


#End Region	' Private Fields


	Public Sub New(ByVal dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing))
		InitializeComponent()

		m_dtaDataList = dtaDataList

	End Sub

	Public Sub LoadReport(ByVal reportName As String)

		'Dim report As XtraReport = XtraReport.FromFile(filePath, False)

		'' Showing the report's preview.
		'report.ShowPreview()

		'Return

		'Dim ps As New PrintingSystem()

		'' Load the document from a file.
		'ps.LoadDocument(filePath)

		'' Create an instance of the preview dialog.
		'Dim preview As New PrintPreviewFormEx()

		'' Load the report document into it.
		'preview.PrintingSystem = ps

		'' Show the preview dialog.
		'preview.ShowDialog()

	End Sub

End Class
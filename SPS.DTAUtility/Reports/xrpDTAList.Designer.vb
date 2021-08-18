<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class xrpDTAList
    Inherits DevExpress.XtraReports.UI.XtraReport

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Dim ShapeBrace1 As DevExpress.XtraPrinting.Shape.ShapeBrace = New DevExpress.XtraPrinting.Shape.ShapeBrace()
		Dim ShapeBrace2 As DevExpress.XtraPrinting.Shape.ShapeBrace = New DevExpress.XtraPrinting.Shape.ShapeBrace()
		Me.PageFooter = New DevExpress.XtraReports.UI.PageFooterBand()
		Me.xrLine2 = New DevExpress.XtraReports.UI.XRLine()
		Me.xrPageInfo1 = New DevExpress.XtraReports.UI.XRPageInfo()
		Me.GroupHeader1 = New DevExpress.XtraReports.UI.GroupHeaderBand()
		Me.xrTable1 = New DevExpress.XtraReports.UI.XRTable()
		Me.xrTableRow1 = New DevExpress.XtraReports.UI.XRTableRow()
		Me.xrTableCell1 = New DevExpress.XtraReports.UI.XRTableCell()
		Me.xrTableCell2 = New DevExpress.XtraReports.UI.XRTableCell()
		Me.LblBCNr = New DevExpress.XtraReports.UI.XRTableCell()
		Me.lblBank = New DevExpress.XtraReports.UI.XRTableCell()
		Me.xrLine1 = New DevExpress.XtraReports.UI.XRLine()
		Me.OddStyle = New DevExpress.XtraReports.UI.XRControlStyle()
		Me.TableHeader = New DevExpress.XtraReports.UI.XRControlStyle()
		Me.EvenStyle = New DevExpress.XtraReports.UI.XRControlStyle()
		Me.Detail = New DevExpress.XtraReports.UI.DetailBand()
		Me.FooterLine = New DevExpress.XtraReports.UI.XRControlStyle()
		Me.Header = New DevExpress.XtraReports.UI.XRControlStyle()
		Me.Lines = New DevExpress.XtraReports.UI.XRControlStyle()
		Me.GroupFooter1 = New DevExpress.XtraReports.UI.GroupFooterBand()
		Me.xrLine3 = New DevExpress.XtraReports.UI.XRLine()
		Me.BottomMargin = New DevExpress.XtraReports.UI.BottomMarginBand()
		Me.xrShape1 = New DevExpress.XtraReports.UI.XRShape()
		Me.TopMargin = New DevExpress.XtraReports.UI.TopMarginBand()
		Me.xrShape2 = New DevExpress.XtraReports.UI.XRShape()
		Me.xrLabel1 = New DevExpress.XtraReports.UI.XRLabel()
		Me.XrTable2 = New DevExpress.XtraReports.UI.XRTable()
		Me.XrTableRow2 = New DevExpress.XtraReports.UI.XRTableRow()
		Me.zgnr = New DevExpress.XtraReports.UI.XRTableCell()
		Me.madata = New DevExpress.XtraReports.UI.XRTableCell()
		Me.bcnr = New DevExpress.XtraReports.UI.XRTableCell()
		Me.Bank = New DevExpress.XtraReports.UI.XRTableCell()
		Me.ibannr = New DevExpress.XtraReports.UI.XRTableCell()
		Me.zggrund = New DevExpress.XtraReports.UI.XRTableCell()
		Me.monat = New DevExpress.XtraReports.UI.XRTableCell()
		Me.betrag = New DevExpress.XtraReports.UI.XRTableCell()
		CType(Me.xrTable1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.XrTable2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
		'
		'PageFooter
		'
		Me.PageFooter.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLine2, Me.xrPageInfo1})
		Me.PageFooter.HeightF = 100.0!
		Me.PageFooter.Name = "PageFooter"
		'
		'xrLine2
		'
		Me.xrLine2.LocationFloat = New DevExpress.Utils.PointFloat(0.00003178914!, 64.58334!)
		Me.xrLine2.Name = "xrLine2"
		Me.xrLine2.SizeF = New System.Drawing.SizeF(649.9999!, 2.0!)
		Me.xrLine2.StyleName = "FooterLine"
		'
		'xrPageInfo1
		'
		Me.xrPageInfo1.Format = "Page {0} of {1}"
		Me.xrPageInfo1.LocationFloat = New DevExpress.Utils.PointFloat(584.3959!, 76.99998!)
		Me.xrPageInfo1.Name = "xrPageInfo1"
		Me.xrPageInfo1.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
		Me.xrPageInfo1.SizeF = New System.Drawing.SizeF(65.60406!, 23.0!)
		Me.xrPageInfo1.StyleName = "EvenStyle"
		'
		'GroupHeader1
		'
		Me.GroupHeader1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrTable1, Me.xrLine1})
		Me.GroupHeader1.HeightF = 60.74999!
		Me.GroupHeader1.KeepTogether = True
		Me.GroupHeader1.Name = "GroupHeader1"
		Me.GroupHeader1.RepeatEveryPage = True
		'
		'xrTable1
		'
		Me.xrTable1.BackColor = System.Drawing.Color.White
		Me.xrTable1.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold)
		Me.xrTable1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(108, Byte), Integer), CType(CType(59, Byte), Integer))
		Me.xrTable1.LocationFloat = New DevExpress.Utils.PointFloat(0.0!, 33.74999!)
		Me.xrTable1.Name = "xrTable1"
		Me.xrTable1.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.xrTableRow1})
		Me.xrTable1.SizeF = New System.Drawing.SizeF(494.7915!, 25.0!)
		Me.xrTable1.StyleName = "TableHeader"
		Me.xrTable1.StylePriority.UseFont = False
		'
		'xrTableRow1
		'
		Me.xrTableRow1.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.xrTableCell1, Me.xrTableCell2, Me.LblBCNr, Me.lblBank})
		Me.xrTableRow1.Name = "xrTableRow1"
		Me.xrTableRow1.Weight = 1.0R
		'
		'xrTableCell1
		'
		Me.xrTableCell1.Name = "xrTableCell1"
		Me.xrTableCell1.Text = "ZG-Nr."
		Me.xrTableCell1.Weight = 0.49923086015248452R
		'
		'xrTableCell2
		'
		Me.xrTableCell2.Name = "xrTableCell2"
		Me.xrTableCell2.Text = "Nach-/Vorname"
		Me.xrTableCell2.Weight = 0.9984616398888968R
		'
		'LblBCNr
		'
		Me.LblBCNr.Name = "LblBCNr"
		Me.LblBCNr.Text = "BCNr"
		Me.LblBCNr.Weight = 0.32171618042278682R
		'
		'lblBank
		'
		Me.lblBank.Name = "lblBank"
		Me.lblBank.Text = "Bank"
		Me.lblBank.Weight = 0.46073167054920361R
		'
		'xrLine1
		'
		Me.xrLine1.LineWidth = 2
		Me.xrLine1.LocationFloat = New DevExpress.Utils.PointFloat(0.0!, 58.74999!)
		Me.xrLine1.Name = "xrLine1"
		Me.xrLine1.SizeF = New System.Drawing.SizeF(649.9999!, 2.0!)
		Me.xrLine1.StyleName = "Lines"
		'
		'OddStyle
		'
		Me.OddStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(216, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(242, Byte), Integer))
		Me.OddStyle.Font = New System.Drawing.Font("Calibri", 9.75!)
		Me.OddStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(13, Byte), Integer), CType(CType(13, Byte), Integer))
		Me.OddStyle.Name = "OddStyle"
		Me.OddStyle.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		Me.OddStyle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
		'
		'TableHeader
		'
		Me.TableHeader.BackColor = System.Drawing.Color.White
		Me.TableHeader.Font = New System.Drawing.Font("Californian FB", 18.0!, System.Drawing.FontStyle.Bold)
		Me.TableHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(107, Byte), Integer), CType(CType(115, Byte), Integer))
		Me.TableHeader.Name = "TableHeader"
		Me.TableHeader.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		Me.TableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
		'
		'EvenStyle
		'
		Me.EvenStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.EvenStyle.Font = New System.Drawing.Font("Calibri", 9.75!)
		Me.EvenStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(13, Byte), Integer), CType(CType(13, Byte), Integer))
		Me.EvenStyle.Name = "EvenStyle"
		Me.EvenStyle.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		Me.EvenStyle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
		'
		'Detail
		'
		Me.Detail.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.XrTable2})
		Me.Detail.HeightF = 64.58334!
		Me.Detail.Name = "Detail"
		Me.Detail.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		Me.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
		'
		'FooterLine
		'
		Me.FooterLine.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(24, Byte), Integer))
		Me.FooterLine.Name = "FooterLine"
		Me.FooterLine.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		'
		'Header
		'
		Me.Header.Font = New System.Drawing.Font("Poor Richard", 48.0!)
		Me.Header.ForeColor = System.Drawing.Color.FromArgb(CType(CType(153, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(191, Byte), Integer))
		Me.Header.Name = "Header"
		Me.Header.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		Me.Header.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
		'
		'Lines
		'
		Me.Lines.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(24, Byte), Integer))
		Me.Lines.Name = "Lines"
		Me.Lines.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		'
		'GroupFooter1
		'
		Me.GroupFooter1.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrLine3})
		Me.GroupFooter1.HeightF = 2.0!
		Me.GroupFooter1.Name = "GroupFooter1"
		'
		'xrLine3
		'
		Me.xrLine3.LineWidth = 2
		Me.xrLine3.LocationFloat = New DevExpress.Utils.PointFloat(0.0001271566!, 0.0!)
		Me.xrLine3.Name = "xrLine3"
		Me.xrLine3.SizeF = New System.Drawing.SizeF(649.9999!, 2.0!)
		Me.xrLine3.StyleName = "Lines"
		'
		'BottomMargin
		'
		Me.BottomMargin.HeightF = 0.0!
		Me.BottomMargin.Name = "BottomMargin"
		Me.BottomMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		Me.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
		'
		'xrShape1
		'
		Me.xrShape1.ForeColor = System.Drawing.Color.Black
		Me.xrShape1.LocationFloat = New DevExpress.Utils.PointFloat(0.00003178914!, 10.00001!)
		Me.xrShape1.Name = "xrShape1"
		Me.xrShape1.Shape = ShapeBrace1
		Me.xrShape1.SizeF = New System.Drawing.SizeF(29.16667!, 76.12502!)
		Me.xrShape1.Stretch = True
		Me.xrShape1.StyleName = "Header"
		'
		'TopMargin
		'
		Me.TopMargin.Controls.AddRange(New DevExpress.XtraReports.UI.XRControl() {Me.xrShape1, Me.xrShape2, Me.xrLabel1})
		Me.TopMargin.HeightF = 86.12503!
		Me.TopMargin.Name = "TopMargin"
		Me.TopMargin.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0!)
		Me.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft
		'
		'xrShape2
		'
		Me.xrShape2.Angle = 180
		Me.xrShape2.LocationFloat = New DevExpress.Utils.PointFloat(621.875!, 10.00001!)
		Me.xrShape2.Name = "xrShape2"
		Me.xrShape2.Shape = ShapeBrace2
		Me.xrShape2.SizeF = New System.Drawing.SizeF(28.125!, 76.12501!)
		Me.xrShape2.Stretch = True
		Me.xrShape2.StyleName = "Header"
		'
		'xrLabel1
		'
		Me.xrLabel1.LocationFloat = New DevExpress.Utils.PointFloat(29.1667!, 10.00001!)
		Me.xrLabel1.Name = "xrLabel1"
		Me.xrLabel1.Padding = New DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100.0!)
		Me.xrLabel1.SizeF = New System.Drawing.SizeF(592.7083!, 76.12502!)
		Me.xrLabel1.StyleName = "Header"
		Me.xrLabel1.Text = "Customers"
		'
		'XrTable2
		'
		Me.XrTable2.BookmarkParent = Me.zgnr
		Me.XrTable2.LocationFloat = New DevExpress.Utils.PointFloat(29.16668!, 35.41667!)
		Me.XrTable2.Name = "XrTable2"
		Me.XrTable2.Rows.AddRange(New DevExpress.XtraReports.UI.XRTableRow() {Me.XrTableRow2})
		Me.XrTable2.SizeF = New System.Drawing.SizeF(745.8333!, 16.66666!)
		'
		'XrTableRow2
		'
		Me.XrTableRow2.Cells.AddRange(New DevExpress.XtraReports.UI.XRTableCell() {Me.zgnr, Me.madata, Me.bcnr, Me.Bank, Me.ibannr, Me.zggrund, Me.monat, Me.betrag})
		Me.XrTableRow2.Name = "XrTableRow2"
		Me.XrTableRow2.Weight = 1.0R
		'
		'zgnr
		'
		Me.zgnr.Name = "zgnr"
		Me.zgnr.Text = "zgnr"
		Me.zgnr.Weight = 0.4050633529469938R
		'
		'madata
		'
		Me.madata.Name = "madata"
		Me.madata.Text = "madata"
		Me.madata.Weight = 0.86075937005537984R
		'
		'bcnr
		'
		Me.bcnr.Name = "bcnr"
		Me.bcnr.Text = "bc"
		Me.bcnr.Weight = 0.32278477922270576R
		'
		'Bank
		'
		Me.Bank.Name = "Bank"
		Me.Bank.Text = "Bank"
		Me.Bank.Weight = 0.63303819101068026R
		'
		'ibannr
		'
		Me.ibannr.Name = "ibannr"
		Me.ibannr.Text = "ibannr"
		Me.ibannr.Weight = 0.73563281868077535R
		'
		'zggrund
		'
		Me.zggrund.Name = "zggrund"
		Me.zggrund.Text = "zggrund"
		Me.zggrund.Weight = 0.71123414631131321R
		'
		'monat
		'
		Me.monat.Name = "monat"
		Me.monat.Text = "monat"
		Me.monat.Weight = 0.64042710702630523R
		'
		'betrag
		'
		Me.betrag.Name = "betrag"
		Me.betrag.Text = "betrag"
		Me.betrag.Weight = 0.22270568075059335R
		'
		'xrpDTAList
		'
		Me.Bands.AddRange(New DevExpress.XtraReports.UI.Band() {Me.Detail, Me.TopMargin, Me.BottomMargin, Me.PageFooter, Me.GroupHeader1, Me.GroupFooter1})
		Me.Margins = New System.Drawing.Printing.Margins(35, 30, 86, 0)
		Me.StyleSheet.AddRange(New DevExpress.XtraReports.UI.XRControlStyle() {Me.Header, Me.TableHeader, Me.OddStyle, Me.EvenStyle, Me.Lines, Me.FooterLine})
		Me.Version = "14.1"
		CType(Me.xrTable1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.XrTable2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

	End Sub
	Friend WithEvents PageFooter As DevExpress.XtraReports.UI.PageFooterBand
	Friend WithEvents xrLine2 As DevExpress.XtraReports.UI.XRLine
	Friend WithEvents xrPageInfo1 As DevExpress.XtraReports.UI.XRPageInfo
	Friend WithEvents GroupHeader1 As DevExpress.XtraReports.UI.GroupHeaderBand
	Friend WithEvents xrTable1 As DevExpress.XtraReports.UI.XRTable
	Friend WithEvents xrTableRow1 As DevExpress.XtraReports.UI.XRTableRow
	Friend WithEvents xrTableCell1 As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents xrTableCell2 As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents LblBCNr As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents lblBank As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents xrLine1 As DevExpress.XtraReports.UI.XRLine
	Friend WithEvents OddStyle As DevExpress.XtraReports.UI.XRControlStyle
	Friend WithEvents TableHeader As DevExpress.XtraReports.UI.XRControlStyle
	Friend WithEvents EvenStyle As DevExpress.XtraReports.UI.XRControlStyle
	Friend WithEvents Detail As DevExpress.XtraReports.UI.DetailBand
	Friend WithEvents FooterLine As DevExpress.XtraReports.UI.XRControlStyle
	Friend WithEvents Header As DevExpress.XtraReports.UI.XRControlStyle
	Friend WithEvents Lines As DevExpress.XtraReports.UI.XRControlStyle
	Friend WithEvents GroupFooter1 As DevExpress.XtraReports.UI.GroupFooterBand
	Friend WithEvents xrLine3 As DevExpress.XtraReports.UI.XRLine
	Friend WithEvents BottomMargin As DevExpress.XtraReports.UI.BottomMarginBand
	Friend WithEvents xrShape1 As DevExpress.XtraReports.UI.XRShape
	Friend WithEvents TopMargin As DevExpress.XtraReports.UI.TopMarginBand
	Friend WithEvents xrShape2 As DevExpress.XtraReports.UI.XRShape
	Friend WithEvents xrLabel1 As DevExpress.XtraReports.UI.XRLabel
	Friend WithEvents XrTable2 As DevExpress.XtraReports.UI.XRTable
	Friend WithEvents zgnr As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents XrTableRow2 As DevExpress.XtraReports.UI.XRTableRow
	Friend WithEvents madata As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents bcnr As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents Bank As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents ibannr As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents zggrund As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents monat As DevExpress.XtraReports.UI.XRTableCell
	Friend WithEvents betrag As DevExpress.XtraReports.UI.XRTableCell
End Class

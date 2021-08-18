Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucReportDetailData
    Inherits ucBaseControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
      MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
			Me.grpRPDetail = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lblLONrValue = New DevExpress.XtraEditors.HyperLinkEdit()
			Me.hlnkES = New DevExpress.XtraEditors.HyperLinkEdit()
			Me.hlnkCustomer = New DevExpress.XtraEditors.HyperLinkEdit()
			Me.hlnkEmployee = New DevExpress.XtraEditors.HyperLinkEdit()
			Me.lblRPDetailPeriodRPToValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailPeriodESToValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailChildCount = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailKDNr = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailMANr = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailMonthYear = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailESAls = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailPeriodES = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailLONr = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailMonthYearValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailChildCountValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailPeriodESFromValue = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailPeriodRP = New DevExpress.XtraEditors.LabelControl()
			Me.lblRPDetailPeriodRPFromValue = New DevExpress.XtraEditors.LabelControl()
			Me.grpRPDetailSalaryData = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.grdSalaryData = New DevExpress.XtraGrid.GridControl()
			Me.gvSalaryData = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grpRPDetail.SuspendLayout()
			CType(Me.lblLONrValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.hlnkES.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.hlnkCustomer.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.hlnkEmployee.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpRPDetailSalaryData.SuspendLayout()
			CType(Me.grdSalaryData, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvSalaryData, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grpRPDetail
			'
			Me.grpRPDetail.BackColor = System.Drawing.Color.Transparent
			Me.grpRPDetail.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpRPDetail.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpRPDetail.Controls.Add(Me.lblLONrValue)
			Me.grpRPDetail.Controls.Add(Me.hlnkES)
			Me.grpRPDetail.Controls.Add(Me.hlnkCustomer)
			Me.grpRPDetail.Controls.Add(Me.hlnkEmployee)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailPeriodRPToValue)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailPeriodESToValue)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailChildCount)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailKDNr)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailMANr)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailMonthYear)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailESAls)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailPeriodES)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailLONr)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailMonthYearValue)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailChildCountValue)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailPeriodESFromValue)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailPeriodRP)
			Me.grpRPDetail.Controls.Add(Me.lblRPDetailPeriodRPFromValue)
			Me.grpRPDetail.Dock = System.Windows.Forms.DockStyle.Top
			Me.grpRPDetail.Location = New System.Drawing.Point(0, 0)
			Me.grpRPDetail.Name = "grpRPDetail"
			Me.grpRPDetail.Size = New System.Drawing.Size(323, 245)
			'
			'
			'
			Me.grpRPDetail.Style.BackColorGradientAngle = 90
			Me.grpRPDetail.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetail.Style.BorderBottomWidth = 1
			Me.grpRPDetail.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpRPDetail.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetail.Style.BorderLeftWidth = 1
			Me.grpRPDetail.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetail.Style.BorderRightWidth = 1
			Me.grpRPDetail.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetail.Style.BorderTopWidth = 1
			Me.grpRPDetail.Style.CornerDiameter = 4
			Me.grpRPDetail.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpRPDetail.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpRPDetail.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpRPDetail.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpRPDetail.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpRPDetail.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpRPDetail.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpRPDetail.TabIndex = 212
			Me.grpRPDetail.Text = "Rapportnummer: {0}"
			'
			'lblLONrValue
			'
			Me.lblLONrValue.EditValue = "LO-Nr"
			Me.lblLONrValue.Location = New System.Drawing.Point(103, 189)
			Me.lblLONrValue.Name = "lblLONrValue"
			Me.lblLONrValue.Properties.AllowFocused = False
			Me.lblLONrValue.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.lblLONrValue.Properties.Appearance.Options.UseBackColor = True
			Me.lblLONrValue.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.lblLONrValue.Size = New System.Drawing.Size(159, 18)
			Me.lblLONrValue.TabIndex = 299
			'
			'hlnkES
			'
			Me.hlnkES.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.hlnkES.EditValue = "ES"
			Me.hlnkES.Location = New System.Drawing.Point(103, 91)
			Me.hlnkES.Name = "hlnkES"
			Me.hlnkES.Properties.AllowFocused = False
			Me.hlnkES.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.hlnkES.Properties.Appearance.Options.UseBackColor = True
			Me.hlnkES.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.hlnkES.Size = New System.Drawing.Size(207, 18)
			Me.hlnkES.TabIndex = 241
			'
			'hlnkCustomer
			'
			Me.hlnkCustomer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.hlnkCustomer.EditValue = "Customer"
			Me.hlnkCustomer.Location = New System.Drawing.Point(103, 50)
			Me.hlnkCustomer.Name = "hlnkCustomer"
			Me.hlnkCustomer.Properties.AllowFocused = False
			Me.hlnkCustomer.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.hlnkCustomer.Properties.Appearance.Options.UseBackColor = True
			Me.hlnkCustomer.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.hlnkCustomer.Size = New System.Drawing.Size(207, 18)
			Me.hlnkCustomer.TabIndex = 240
			'
			'hlnkEmployee
			'
			Me.hlnkEmployee.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.hlnkEmployee.EditValue = "Employee"
			Me.hlnkEmployee.Location = New System.Drawing.Point(103, 31)
			Me.hlnkEmployee.Name = "hlnkEmployee"
			Me.hlnkEmployee.Properties.AllowFocused = False
			Me.hlnkEmployee.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
			Me.hlnkEmployee.Properties.Appearance.Options.UseBackColor = True
			Me.hlnkEmployee.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.hlnkEmployee.Size = New System.Drawing.Size(207, 18)
			Me.hlnkEmployee.TabIndex = 239
			'
			'lblRPDetailPeriodRPToValue
			'
			Me.lblRPDetailPeriodRPToValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblRPDetailPeriodRPToValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailPeriodRPToValue.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.lblRPDetailPeriodRPToValue.Location = New System.Drawing.Point(103, 170)
			Me.lblRPDetailPeriodRPToValue.Name = "lblRPDetailPeriodRPToValue"
			Me.lblRPDetailPeriodRPToValue.Size = New System.Drawing.Size(207, 13)
			Me.lblRPDetailPeriodRPToValue.TabIndex = 215
			Me.lblRPDetailPeriodRPToValue.Text = "{0}"
			'
			'lblRPDetailPeriodESToValue
			'
			Me.lblRPDetailPeriodESToValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblRPDetailPeriodESToValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailPeriodESToValue.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.lblRPDetailPeriodESToValue.Location = New System.Drawing.Point(103, 133)
			Me.lblRPDetailPeriodESToValue.Name = "lblRPDetailPeriodESToValue"
			Me.lblRPDetailPeriodESToValue.Size = New System.Drawing.Size(207, 13)
			Me.lblRPDetailPeriodESToValue.TabIndex = 214
			Me.lblRPDetailPeriodESToValue.Text = "{0}"
			'
			'lblRPDetailChildCount
			'
			Me.lblRPDetailChildCount.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailChildCount.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailChildCount.Location = New System.Drawing.Point(15, 75)
			Me.lblRPDetailChildCount.Name = "lblRPDetailChildCount"
			Me.lblRPDetailChildCount.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailChildCount.TabIndex = 43
			Me.lblRPDetailChildCount.Text = "Anzahl Kinder"
			'
			'lblRPDetailKDNr
			'
			Me.lblRPDetailKDNr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailKDNr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailKDNr.Location = New System.Drawing.Point(15, 53)
			Me.lblRPDetailKDNr.Name = "lblRPDetailKDNr"
			Me.lblRPDetailKDNr.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailKDNr.TabIndex = 42
			Me.lblRPDetailKDNr.Text = "KD-Nr"
			'
			'lblRPDetailMANr
			'
			Me.lblRPDetailMANr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailMANr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailMANr.Location = New System.Drawing.Point(15, 34)
			Me.lblRPDetailMANr.Name = "lblRPDetailMANr"
			Me.lblRPDetailMANr.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailMANr.TabIndex = 41
			Me.lblRPDetailMANr.Text = "MA-Nr"
			'
			'lblRPDetailMonthYear
			'
			Me.lblRPDetailMonthYear.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailMonthYear.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailMonthYear.Location = New System.Drawing.Point(15, 15)
			Me.lblRPDetailMonthYear.Name = "lblRPDetailMonthYear"
			Me.lblRPDetailMonthYear.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailMonthYear.TabIndex = 5
			Me.lblRPDetailMonthYear.Text = "Monat / Jahr"
			'
			'lblRPDetailESAls
			'
			Me.lblRPDetailESAls.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailESAls.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailESAls.Location = New System.Drawing.Point(15, 94)
			Me.lblRPDetailESAls.Name = "lblRPDetailESAls"
			Me.lblRPDetailESAls.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailESAls.TabIndex = 48
			Me.lblRPDetailESAls.Text = "Einsatz"
			'
			'lblRPDetailPeriodES
			'
			Me.lblRPDetailPeriodES.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailPeriodES.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailPeriodES.Location = New System.Drawing.Point(15, 114)
			Me.lblRPDetailPeriodES.Name = "lblRPDetailPeriodES"
			Me.lblRPDetailPeriodES.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailPeriodES.TabIndex = 52
			Me.lblRPDetailPeriodES.Text = "ES-Periode"
			'
			'lblRPDetailLONr
			'
			Me.lblRPDetailLONr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailLONr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailLONr.Location = New System.Drawing.Point(15, 191)
			Me.lblRPDetailLONr.Name = "lblRPDetailLONr"
			Me.lblRPDetailLONr.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailLONr.TabIndex = 54
			Me.lblRPDetailLONr.Text = "LO-Nr"
			'
			'lblRPDetailMonthYearValue
			'
			Me.lblRPDetailMonthYearValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblRPDetailMonthYearValue.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
			Me.lblRPDetailMonthYearValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailMonthYearValue.Location = New System.Drawing.Point(103, 15)
			Me.lblRPDetailMonthYearValue.Name = "lblRPDetailMonthYearValue"
			Me.lblRPDetailMonthYearValue.Size = New System.Drawing.Size(207, 13)
			Me.lblRPDetailMonthYearValue.TabIndex = 55
			Me.lblRPDetailMonthYearValue.Text = "Month/Year Value"
			'
			'lblRPDetailChildCountValue
			'
			Me.lblRPDetailChildCountValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblRPDetailChildCountValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailChildCountValue.Location = New System.Drawing.Point(103, 75)
			Me.lblRPDetailChildCountValue.Name = "lblRPDetailChildCountValue"
			Me.lblRPDetailChildCountValue.Size = New System.Drawing.Size(207, 13)
			Me.lblRPDetailChildCountValue.TabIndex = 58
			Me.lblRPDetailChildCountValue.Text = "ChildCountValue"
			'
			'lblRPDetailPeriodESFromValue
			'
			Me.lblRPDetailPeriodESFromValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblRPDetailPeriodESFromValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailPeriodESFromValue.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.lblRPDetailPeriodESFromValue.Location = New System.Drawing.Point(103, 114)
			Me.lblRPDetailPeriodESFromValue.Name = "lblRPDetailPeriodESFromValue"
			Me.lblRPDetailPeriodESFromValue.Size = New System.Drawing.Size(207, 13)
			Me.lblRPDetailPeriodESFromValue.TabIndex = 61
			Me.lblRPDetailPeriodESFromValue.Text = "{0}"
			'
			'lblRPDetailPeriodRP
			'
			Me.lblRPDetailPeriodRP.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblRPDetailPeriodRP.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailPeriodRP.Location = New System.Drawing.Point(15, 153)
			Me.lblRPDetailPeriodRP.Name = "lblRPDetailPeriodRP"
			Me.lblRPDetailPeriodRP.Size = New System.Drawing.Size(82, 13)
			Me.lblRPDetailPeriodRP.TabIndex = 63
			Me.lblRPDetailPeriodRP.Text = "RP-Periode"
			'
			'lblRPDetailPeriodRPFromValue
			'
			Me.lblRPDetailPeriodRPFromValue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblRPDetailPeriodRPFromValue.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblRPDetailPeriodRPFromValue.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			Me.lblRPDetailPeriodRPFromValue.Location = New System.Drawing.Point(103, 153)
			Me.lblRPDetailPeriodRPFromValue.Name = "lblRPDetailPeriodRPFromValue"
			Me.lblRPDetailPeriodRPFromValue.Size = New System.Drawing.Size(207, 13)
			Me.lblRPDetailPeriodRPFromValue.TabIndex = 64
			Me.lblRPDetailPeriodRPFromValue.Text = "{0}"
			'
			'grpRPDetailSalaryData
			'
			Me.grpRPDetailSalaryData.BackColor = System.Drawing.Color.Transparent
			Me.grpRPDetailSalaryData.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpRPDetailSalaryData.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpRPDetailSalaryData.Controls.Add(Me.grdSalaryData)
			Me.grpRPDetailSalaryData.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.grpRPDetailSalaryData.Location = New System.Drawing.Point(0, 257)
			Me.grpRPDetailSalaryData.Name = "grpRPDetailSalaryData"
			Me.grpRPDetailSalaryData.Size = New System.Drawing.Size(323, 117)
			'
			'
			'
			Me.grpRPDetailSalaryData.Style.BackColorGradientAngle = 90
			Me.grpRPDetailSalaryData.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetailSalaryData.Style.BorderBottomWidth = 1
			Me.grpRPDetailSalaryData.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpRPDetailSalaryData.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetailSalaryData.Style.BorderLeftWidth = 1
			Me.grpRPDetailSalaryData.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetailSalaryData.Style.BorderRightWidth = 1
			Me.grpRPDetailSalaryData.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpRPDetailSalaryData.Style.BorderTopWidth = 1
			Me.grpRPDetailSalaryData.Style.CornerDiameter = 4
			Me.grpRPDetailSalaryData.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpRPDetailSalaryData.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpRPDetailSalaryData.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpRPDetailSalaryData.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpRPDetailSalaryData.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpRPDetailSalaryData.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpRPDetailSalaryData.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpRPDetailSalaryData.TabIndex = 213
			Me.grpRPDetailSalaryData.Text = "Lohndaten"
			'
			'grdSalaryData
			'
			Me.grdSalaryData.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grdSalaryData.Location = New System.Drawing.Point(0, 0)
			Me.grdSalaryData.MainView = Me.gvSalaryData
			Me.grdSalaryData.Name = "grdSalaryData"
			Me.grdSalaryData.Size = New System.Drawing.Size(317, 95)
			Me.grdSalaryData.TabIndex = 4
			Me.grdSalaryData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvSalaryData})
			'
			'gvSalaryData
			'
			Me.gvSalaryData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvSalaryData.GridControl = Me.grdSalaryData
			Me.gvSalaryData.Name = "gvSalaryData"
			Me.gvSalaryData.OptionsBehavior.Editable = False
			Me.gvSalaryData.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvSalaryData.OptionsView.ShowGroupPanel = False
			'
			'ucReportDetailData
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpRPDetail)
			Me.Controls.Add(Me.grpRPDetailSalaryData)
			Me.Name = "ucReportDetailData"
			Me.Size = New System.Drawing.Size(323, 374)
			Me.grpRPDetail.ResumeLayout(False)
			CType(Me.lblLONrValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.hlnkES.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.hlnkCustomer.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.hlnkEmployee.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpRPDetailSalaryData.ResumeLayout(False)
			CType(Me.grdSalaryData, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvSalaryData, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents grpRPDetail As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents grpRPDetailSalaryData As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents grdSalaryData As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvSalaryData As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblRPDetailPeriodRPFromValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailPeriodRP As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblRPDetailPeriodESFromValue As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblRPDetailChildCountValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailMonthYearValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailLONr As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailPeriodES As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailESAls As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailMonthYear As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailMANr As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailKDNr As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailChildCount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailPeriodESToValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRPDetailPeriodRPToValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents hlnkEmployee As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents hlnkCustomer As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents hlnkES As DevExpress.XtraEditors.HyperLinkEdit
		Friend WithEvents lblLONrValue As DevExpress.XtraEditors.HyperLinkEdit
	End Class

End Namespace

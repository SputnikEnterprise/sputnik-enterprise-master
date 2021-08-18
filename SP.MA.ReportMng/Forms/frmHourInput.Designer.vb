<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHourInput
  Inherits DevExpress.XtraEditors.XtraForm

  'Form overrides dispose to clean up the component list.
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHourInput))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.grdTimeTable = New DevExpress.XtraGrid.GridControl()
		Me.gvTimetable = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.txtReplaceHoursThrough = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtReplaceHoursEvenly = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.lblReplaceHours = New DevExpress.XtraEditors.LabelControl()
		Me.lblReplaceHourSumEvenly = New DevExpress.XtraEditors.LabelControl()
		Me.lblReplaceAbsenceCodes = New DevExpress.XtraEditors.LabelControl()
		Me.lueAbsenceCodeDataReplaceTrough = New DevExpress.XtraEditors.LookUpEdit()
		Me.chkAsNormalHours = New DevExpress.XtraEditors.CheckEdit()
		Me.pnlTopControls = New DevExpress.XtraEditors.PanelControl()
		Me.chkAutoPrinBarcode = New DevExpress.XtraEditors.CheckEdit()
		Me.lblMaximalWorkingHoursPerDay = New DevExpress.XtraEditors.LabelControl()
		CType(Me.grdTimeTable, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvTimetable, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtReplaceHoursThrough.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtReplaceHoursEvenly.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueAbsenceCodeDataReplaceTrough.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkAsNormalHours.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlTopControls, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlTopControls.SuspendLayout()
		CType(Me.chkAutoPrinBarcode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'grdTimeTable
		'
		Me.grdTimeTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdTimeTable.Location = New System.Drawing.Point(0, 172)
		Me.grdTimeTable.MainView = Me.gvTimetable
		Me.grdTimeTable.Name = "grdTimeTable"
		Me.grdTimeTable.Size = New System.Drawing.Size(736, 332)
		Me.grdTimeTable.TabIndex = 5
		Me.grdTimeTable.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvTimetable})
		'
		'gvTimetable
		'
		Me.gvTimetable.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvTimetable.GridControl = Me.grdTimeTable
		Me.gvTimetable.Name = "gvTimetable"
		Me.gvTimetable.OptionsBehavior.Editable = False
		Me.gvTimetable.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvTimetable.OptionsView.ShowGroupPanel = False
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
		Me.btnSave.Location = New System.Drawing.Point(596, 102)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(105, 28)
		Me.btnSave.TabIndex = 6
		Me.btnSave.Text = "Speichern"
		'
		'txtReplaceHoursThrough
		'
		Me.txtReplaceHoursThrough.Location = New System.Drawing.Point(229, 21)
		Me.txtReplaceHoursThrough.Name = "txtReplaceHoursThrough"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Green
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.txtReplaceHoursThrough.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, CType(resources.GetObject("txtReplaceHoursThrough.Properties.Buttons"), System.Drawing.Image), New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
		Me.txtReplaceHoursThrough.Properties.DisplayFormat.FormatString = "N2"
		Me.txtReplaceHoursThrough.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		Me.txtReplaceHoursThrough.Properties.EditFormat.FormatString = "N2"
		Me.txtReplaceHoursThrough.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		Me.txtReplaceHoursThrough.Size = New System.Drawing.Size(94, 20)
		Me.txtReplaceHoursThrough.TabIndex = 2
		'
		'txtReplaceHoursEvenly
		'
		Me.txtReplaceHoursEvenly.Location = New System.Drawing.Point(229, 46)
		Me.txtReplaceHoursEvenly.Name = "txtReplaceHoursEvenly"
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Green
		SerializableAppearanceObject2.Options.UseForeColor = True
		Me.txtReplaceHoursEvenly.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, CType(resources.GetObject("txtReplaceHoursEvenly.Properties.Buttons"), System.Drawing.Image), New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
		Me.txtReplaceHoursEvenly.Size = New System.Drawing.Size(94, 20)
		Me.txtReplaceHoursEvenly.TabIndex = 3
		'
		'lblReplaceHours
		'
		Me.lblReplaceHours.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblReplaceHours.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblReplaceHours.Location = New System.Drawing.Point(35, 24)
		Me.lblReplaceHours.Name = "lblReplaceHours"
		Me.lblReplaceHours.Size = New System.Drawing.Size(188, 13)
		Me.lblReplaceHours.TabIndex = 299
		Me.lblReplaceHours.Text = "Stunden ersetzen durch"
		'
		'lblReplaceHourSumEvenly
		'
		Me.lblReplaceHourSumEvenly.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblReplaceHourSumEvenly.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblReplaceHourSumEvenly.Location = New System.Drawing.Point(35, 49)
		Me.lblReplaceHourSumEvenly.Name = "lblReplaceHourSumEvenly"
		Me.lblReplaceHourSumEvenly.Size = New System.Drawing.Size(188, 13)
		Me.lblReplaceHourSumEvenly.TabIndex = 300
		Me.lblReplaceHourSumEvenly.Text = "Stundensumme verteilen auf Tage"
		'
		'lblReplaceAbsenceCodes
		'
		Me.lblReplaceAbsenceCodes.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblReplaceAbsenceCodes.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblReplaceAbsenceCodes.Location = New System.Drawing.Point(35, 75)
		Me.lblReplaceAbsenceCodes.Name = "lblReplaceAbsenceCodes"
		Me.lblReplaceAbsenceCodes.Size = New System.Drawing.Size(188, 13)
		Me.lblReplaceAbsenceCodes.TabIndex = 302
		Me.lblReplaceAbsenceCodes.Text = "Fehltage ersetzen durch"
		'
		'lueAbsenceCodeDataReplaceTrough
		'
		Me.lueAbsenceCodeDataReplaceTrough.Location = New System.Drawing.Point(229, 72)
		Me.lueAbsenceCodeDataReplaceTrough.Name = "lueAbsenceCodeDataReplaceTrough"
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Green
		SerializableAppearanceObject3.Options.UseForeColor = True
		Me.lueAbsenceCodeDataReplaceTrough.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
		Me.lueAbsenceCodeDataReplaceTrough.Properties.ShowFooter = False
		Me.lueAbsenceCodeDataReplaceTrough.Size = New System.Drawing.Size(94, 20)
		Me.lueAbsenceCodeDataReplaceTrough.TabIndex = 4
		'
		'chkAsNormalHours
		'
		Me.chkAsNormalHours.Location = New System.Drawing.Point(68, 111)
		Me.chkAsNormalHours.Name = "chkAsNormalHours"
		Me.chkAsNormalHours.Properties.AllowFocused = False
		Me.chkAsNormalHours.Properties.Appearance.Options.UseTextOptions = True
		Me.chkAsNormalHours.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkAsNormalHours.Properties.Caption = "Als Normalstunden erfassen"
		Me.chkAsNormalHours.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkAsNormalHours.Size = New System.Drawing.Size(255, 19)
		Me.chkAsNormalHours.TabIndex = 1
		'
		'pnlTopControls
		'
		Me.pnlTopControls.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.pnlTopControls.Controls.Add(Me.chkAutoPrinBarcode)
		Me.pnlTopControls.Controls.Add(Me.lblMaximalWorkingHoursPerDay)
		Me.pnlTopControls.Controls.Add(Me.chkAsNormalHours)
		Me.pnlTopControls.Controls.Add(Me.lueAbsenceCodeDataReplaceTrough)
		Me.pnlTopControls.Controls.Add(Me.lblReplaceHours)
		Me.pnlTopControls.Controls.Add(Me.lblReplaceAbsenceCodes)
		Me.pnlTopControls.Controls.Add(Me.btnSave)
		Me.pnlTopControls.Controls.Add(Me.lblReplaceHourSumEvenly)
		Me.pnlTopControls.Controls.Add(Me.txtReplaceHoursThrough)
		Me.pnlTopControls.Controls.Add(Me.txtReplaceHoursEvenly)
		Me.pnlTopControls.Location = New System.Drawing.Point(0, -1)
		Me.pnlTopControls.Name = "pnlTopControls"
		Me.pnlTopControls.Size = New System.Drawing.Size(736, 167)
		Me.pnlTopControls.TabIndex = 305
		'
		'chkAutoPrinBarcode
		'
		Me.chkAutoPrinBarcode.Location = New System.Drawing.Point(56, 136)
		Me.chkAutoPrinBarcode.Name = "chkAutoPrinBarcode"
		Me.chkAutoPrinBarcode.Properties.AllowFocused = False
		Me.chkAutoPrinBarcode.Properties.Appearance.Options.UseTextOptions = True
		Me.chkAutoPrinBarcode.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkAutoPrinBarcode.Properties.Caption = "Matrixcode beim Speichern drucken"
		Me.chkAutoPrinBarcode.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkAutoPrinBarcode.Size = New System.Drawing.Size(267, 19)
		Me.chkAutoPrinBarcode.TabIndex = 304
		'
		'lblMaximalWorkingHoursPerDay
		'
		Me.lblMaximalWorkingHoursPerDay.Appearance.ForeColor = System.Drawing.Color.Red
		Me.lblMaximalWorkingHoursPerDay.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblMaximalWorkingHoursPerDay.Location = New System.Drawing.Point(329, 24)
		Me.lblMaximalWorkingHoursPerDay.Name = "lblMaximalWorkingHoursPerDay"
		Me.lblMaximalWorkingHoursPerDay.Size = New System.Drawing.Size(267, 13)
		Me.lblMaximalWorkingHoursPerDay.TabIndex = 303
		Me.lblMaximalWorkingHoursPerDay.Text = "#"
		'
		'frmHourInput
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(733, 506)
		Me.Controls.Add(Me.pnlTopControls)
		Me.Controls.Add(Me.grdTimeTable)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.Name = "frmHourInput"
		Me.Text = "Stundenplan"
		CType(Me.grdTimeTable, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvTimetable, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtReplaceHoursThrough.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtReplaceHoursEvenly.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueAbsenceCodeDataReplaceTrough.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkAsNormalHours.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlTopControls, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlTopControls.ResumeLayout(False)
		CType(Me.chkAutoPrinBarcode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
  Friend WithEvents grdTimeTable As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvTimetable As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
  Friend WithEvents txtReplaceHoursThrough As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents txtReplaceHoursEvenly As DevExpress.XtraEditors.ComboBoxEdit
  Friend WithEvents lblReplaceHours As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblReplaceHourSumEvenly As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblReplaceAbsenceCodes As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lueAbsenceCodeDataReplaceTrough As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents chkAsNormalHours As DevExpress.XtraEditors.CheckEdit
  Friend WithEvents pnlTopControls As DevExpress.XtraEditors.PanelControl
  Friend WithEvents lblMaximalWorkingHoursPerDay As DevExpress.XtraEditors.LabelControl
  Friend WithEvents chkAutoPrinBarcode As DevExpress.XtraEditors.CheckEdit
End Class

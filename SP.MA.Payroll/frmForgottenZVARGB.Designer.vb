Namespace UI

	<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
	Partial Class frmForgottenZVARGB
		Inherits DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm

		'Form overrides dispose to clean up the component list.
		<System.Diagnostics.DebuggerNonUserCode()>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			Try
				If disposing AndAlso components IsNot Nothing Then
					components.Dispose()
				End If
			Finally
				MyBase.Dispose(disposing)
			End Try
		End Sub

		'Required by the Windows Form Designer
		Private components As System.ComponentModel.IContainer

		'NOTE: The following procedure is required by the Windows Form Designer
		'It can be modified using the Windows Form Designer.  
		'Do not modify it using the code editor.
		<System.Diagnostics.DebuggerStepThrough()>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmForgottenZVARGB))
			Me.FluentDesignFormContainer1 = New DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer()
			Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
			Me.lblHeaderInfo = New DevExpress.XtraEditors.LabelControl()
			Me.grdZVARGBEmployees = New DevExpress.XtraGrid.GridControl()
			Me.gvZVARGBEmployees = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.AccordionControl1 = New DevExpress.XtraBars.Navigation.AccordionControl()
			Me.AccordionContentContainer1 = New DevExpress.XtraBars.Navigation.AccordionContentContainer()
			Me.pnlFilter = New DevExpress.XtraEditors.PanelControl()
			Me.SeparatorControl2 = New DevExpress.XtraEditors.SeparatorControl()
			Me.SeparatorControl1 = New DevExpress.XtraEditors.SeparatorControl()
			Me.btnOpenZVForm = New DevExpress.XtraEditors.SimpleButton()
			Me.btnOpenAGForm = New DevExpress.XtraEditors.SimpleButton()
			Me.lueMandant = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblMonat = New DevExpress.XtraEditors.LabelControl()
			Me.lblMandant = New DevExpress.XtraEditors.LabelControl()
			Me.lueMonth = New DevExpress.XtraEditors.LookUpEdit()
			Me.lueYear = New DevExpress.XtraEditors.LookUpEdit()
			Me.lblJahr = New DevExpress.XtraEditors.LabelControl()
			Me.AccordionControlElement1 = New DevExpress.XtraBars.Navigation.AccordionControlElement()
			Me.AccordionControlElement2 = New DevExpress.XtraBars.Navigation.AccordionControlElement()
			Me.FluentDesignFormControl1 = New DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl()
			Me.FluentFormDefaultManager1 = New DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(Me.components)
			Me.TextEdit1 = New DevExpress.XtraEditors.TextEdit()
			Me.FluentDesignFormContainer1.SuspendLayout()
			CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.PanelControl2.SuspendLayout()
			CType(Me.grdZVARGBEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvZVARGBEmployees, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.AccordionControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.AccordionControl1.SuspendLayout()
			Me.AccordionContentContainer1.SuspendLayout()
			CType(Me.pnlFilter, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.pnlFilter.SuspendLayout()
			CType(Me.SeparatorControl2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.FluentDesignFormControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.FluentFormDefaultManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'FluentDesignFormContainer1
			'
			Me.FluentDesignFormContainer1.Controls.Add(Me.PanelControl2)
			Me.FluentDesignFormContainer1.Controls.Add(Me.grdZVARGBEmployees)
			Me.FluentDesignFormContainer1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.FluentDesignFormContainer1.Location = New System.Drawing.Point(289, 31)
			Me.FluentDesignFormContainer1.Margin = New System.Windows.Forms.Padding(2)
			Me.FluentDesignFormContainer1.Name = "FluentDesignFormContainer1"
			Me.FluentDesignFormContainer1.Padding = New System.Windows.Forms.Padding(5)
			Me.FluentDesignFormContainer1.Size = New System.Drawing.Size(608, 655)
			Me.FluentDesignFormContainer1.TabIndex = 0
			'
			'PanelControl2
			'
			Me.PanelControl2.Controls.Add(Me.lblHeaderInfo)
			Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Top
			Me.PanelControl2.Location = New System.Drawing.Point(5, 5)
			Me.PanelControl2.Name = "PanelControl2"
			Me.PanelControl2.Size = New System.Drawing.Size(598, 109)
			Me.PanelControl2.TabIndex = 291
			'
			'lblHeaderInfo
			'
			Me.lblHeaderInfo.AllowHtmlString = True
			Me.lblHeaderInfo.Appearance.Options.UseTextOptions = True
			Me.lblHeaderInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			Me.lblHeaderInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			Me.lblHeaderInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblHeaderInfo.LineColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
			Me.lblHeaderInfo.LineLocation = DevExpress.XtraEditors.LineLocation.Left
			Me.lblHeaderInfo.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Vertical
			Me.lblHeaderInfo.LineVisible = True
			Me.lblHeaderInfo.Location = New System.Drawing.Point(13, 12)
			Me.lblHeaderInfo.Name = "lblHeaderInfo"
			Me.lblHeaderInfo.Size = New System.Drawing.Size(534, 78)
			Me.lblHeaderInfo.TabIndex = 293
			Me.lblHeaderInfo.Text = resources.GetString("lblHeaderInfo.Text")
			'
			'grdZVARGBEmployees
			'
			Me.grdZVARGBEmployees.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdZVARGBEmployees.Location = New System.Drawing.Point(5, 120)
			Me.grdZVARGBEmployees.MainView = Me.gvZVARGBEmployees
			Me.grdZVARGBEmployees.Name = "grdZVARGBEmployees"
			Me.grdZVARGBEmployees.Size = New System.Drawing.Size(598, 528)
			Me.grdZVARGBEmployees.TabIndex = 290
			Me.grdZVARGBEmployees.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvZVARGBEmployees})
			'
			'gvZVARGBEmployees
			'
			Me.gvZVARGBEmployees.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvZVARGBEmployees.GridControl = Me.grdZVARGBEmployees
			Me.gvZVARGBEmployees.Name = "gvZVARGBEmployees"
			Me.gvZVARGBEmployees.OptionsBehavior.Editable = False
			Me.gvZVARGBEmployees.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvZVARGBEmployees.OptionsView.ShowGroupPanel = False
			'
			'AccordionControl1
			'
			Me.AccordionControl1.Controls.Add(Me.AccordionContentContainer1)
			Me.AccordionControl1.Dock = System.Windows.Forms.DockStyle.Left
			Me.AccordionControl1.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {Me.AccordionControlElement1})
			Me.AccordionControl1.ExpandElementMode = DevExpress.XtraBars.Navigation.ExpandElementMode.[Single]
			Me.AccordionControl1.ExpandGroupOnHeaderClick = False
			Me.AccordionControl1.ExpandItemOnHeaderClick = False
			Me.AccordionControl1.Location = New System.Drawing.Point(0, 31)
			Me.AccordionControl1.Margin = New System.Windows.Forms.Padding(2)
			Me.AccordionControl1.Name = "AccordionControl1"
			Me.AccordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch
			Me.AccordionControl1.ShowGroupExpandButtons = False
			Me.AccordionControl1.ShowItemExpandButtons = False
			Me.AccordionControl1.Size = New System.Drawing.Size(289, 655)
			Me.AccordionControl1.TabIndex = 1
			Me.AccordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu
			'
			'AccordionContentContainer1
			'
			Me.AccordionContentContainer1.Controls.Add(Me.pnlFilter)
			Me.AccordionContentContainer1.Name = "AccordionContentContainer1"
			Me.AccordionContentContainer1.Padding = New System.Windows.Forms.Padding(5)
			Me.AccordionContentContainer1.Size = New System.Drawing.Size(270, 347)
			Me.AccordionContentContainer1.TabIndex = 2
			'
			'pnlFilter
			'
			Me.pnlFilter.Controls.Add(Me.SeparatorControl2)
			Me.pnlFilter.Controls.Add(Me.SeparatorControl1)
			Me.pnlFilter.Controls.Add(Me.btnOpenZVForm)
			Me.pnlFilter.Controls.Add(Me.btnOpenAGForm)
			Me.pnlFilter.Controls.Add(Me.lueMandant)
			Me.pnlFilter.Controls.Add(Me.lblMonat)
			Me.pnlFilter.Controls.Add(Me.lblMandant)
			Me.pnlFilter.Controls.Add(Me.lueMonth)
			Me.pnlFilter.Controls.Add(Me.lueYear)
			Me.pnlFilter.Controls.Add(Me.lblJahr)
			Me.pnlFilter.Dock = System.Windows.Forms.DockStyle.Fill
			Me.pnlFilter.Location = New System.Drawing.Point(5, 5)
			Me.pnlFilter.Name = "pnlFilter"
			Me.pnlFilter.Size = New System.Drawing.Size(260, 337)
			Me.pnlFilter.TabIndex = 3
			'
			'SeparatorControl2
			'
			Me.SeparatorControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.SeparatorControl2.Location = New System.Drawing.Point(5, 58)
			Me.SeparatorControl2.Name = "SeparatorControl2"
			Me.SeparatorControl2.Size = New System.Drawing.Size(250, 18)
			Me.SeparatorControl2.TabIndex = 296
			'
			'SeparatorControl1
			'
			Me.SeparatorControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.SeparatorControl1.Location = New System.Drawing.Point(5, 199)
			Me.SeparatorControl1.Name = "SeparatorControl1"
			Me.SeparatorControl1.Size = New System.Drawing.Size(250, 18)
			Me.SeparatorControl1.TabIndex = 295
			'
			'btnOpenZVForm
			'
			Me.btnOpenZVForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnOpenZVForm.Location = New System.Drawing.Point(105, 223)
			Me.btnOpenZVForm.Name = "btnOpenZVForm"
			Me.btnOpenZVForm.Size = New System.Drawing.Size(140, 31)
			Me.btnOpenZVForm.TabIndex = 294
			Me.btnOpenZVForm.Text = "ZV.-Formular öffnen"
			'
			'btnOpenAGForm
			'
			Me.btnOpenAGForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnOpenAGForm.Location = New System.Drawing.Point(105, 260)
			Me.btnOpenAGForm.Name = "btnOpenAGForm"
			Me.btnOpenAGForm.Size = New System.Drawing.Size(140, 30)
			Me.btnOpenAGForm.TabIndex = 293
			Me.btnOpenAGForm.Text = "ARGB.-Formular öffnen"
			'
			'lueMandant
			'
			Me.lueMandant.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lueMandant.Location = New System.Drawing.Point(81, 26)
			Me.lueMandant.Name = "lueMandant"
			Me.lueMandant.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup
			Me.lueMandant.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueMandant.Properties.DropDownRows = 10
			Me.lueMandant.Properties.NullText = ""
			Me.lueMandant.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete
			Me.lueMandant.Properties.ShowFooter = False
			Me.lueMandant.Properties.ShowHeader = False
			Me.lueMandant.Size = New System.Drawing.Size(164, 20)
			Me.lueMandant.TabIndex = 287
			'
			'lblMonat
			'
			Me.lblMonat.Appearance.Options.UseTextOptions = True
			Me.lblMonat.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMonat.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMonat.Location = New System.Drawing.Point(5, 120)
			Me.lblMonat.Name = "lblMonat"
			Me.lblMonat.Size = New System.Drawing.Size(70, 13)
			Me.lblMonat.TabIndex = 292
			Me.lblMonat.Text = "Monat"
			'
			'lblMandant
			'
			Me.lblMandant.Appearance.Options.UseTextOptions = True
			Me.lblMandant.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMandant.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMandant.Location = New System.Drawing.Point(5, 30)
			Me.lblMandant.Name = "lblMandant"
			Me.lblMandant.Size = New System.Drawing.Size(70, 13)
			Me.lblMandant.TabIndex = 288
			Me.lblMandant.Text = "Mandant"
			'
			'lueMonth
			'
			Me.lueMonth.Location = New System.Drawing.Point(81, 116)
			Me.lueMonth.Name = "lueMonth"
			Me.lueMonth.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueMonth.Properties.ShowFooter = False
			Me.lueMonth.Size = New System.Drawing.Size(104, 20)
			Me.lueMonth.TabIndex = 290
			'
			'lueYear
			'
			Me.lueYear.Location = New System.Drawing.Point(81, 90)
			Me.lueYear.Name = "lueYear"
			Me.lueYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lueYear.Properties.ShowFooter = False
			Me.lueYear.Size = New System.Drawing.Size(104, 20)
			Me.lueYear.TabIndex = 289
			'
			'lblJahr
			'
			Me.lblJahr.Appearance.Options.UseTextOptions = True
			Me.lblJahr.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblJahr.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblJahr.Location = New System.Drawing.Point(5, 94)
			Me.lblJahr.Name = "lblJahr"
			Me.lblJahr.Size = New System.Drawing.Size(70, 13)
			Me.lblJahr.TabIndex = 291
			Me.lblJahr.Text = "Jahr"
			'
			'AccordionControlElement1
			'
			Me.AccordionControlElement1.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {Me.AccordionControlElement2})
			Me.AccordionControlElement1.Expanded = True
			Me.AccordionControlElement1.HeaderTemplate.AddRange(New DevExpress.XtraBars.Navigation.HeaderElementInfo() {New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Text), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Image, DevExpress.XtraBars.Navigation.HeaderElementAlignment.Right), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.HeaderControl, DevExpress.XtraBars.Navigation.HeaderElementAlignment.Left), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.ContextButtons, DevExpress.XtraBars.Navigation.HeaderElementAlignment.Left)})
			Me.AccordionControlElement1.HeaderVisible = False
			Me.AccordionControlElement1.ImageOptions.Image = Global.SP.MA.PayrollMng.My.Resources.Resources.quickfilter_16x16
			Me.AccordionControlElement1.Name = "AccordionControlElement1"
			Me.AccordionControlElement1.VisibleInFooter = False
			'
			'AccordionControlElement2
			'
			Me.AccordionControlElement2.ContentContainer = Me.AccordionContentContainer1
			Me.AccordionControlElement2.Expanded = True
			Me.AccordionControlElement2.HeaderTemplate.AddRange(New DevExpress.XtraBars.Navigation.HeaderElementInfo() {New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Image), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.HeaderControl), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.ContextButtons), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Text, DevExpress.XtraBars.Navigation.HeaderElementAlignment.Right)})
			Me.AccordionControlElement2.HeaderVisible = False
			Me.AccordionControlElement2.Name = "AccordionControlElement2"
			Me.AccordionControlElement2.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
			'
			'FluentDesignFormControl1
			'
			Me.FluentDesignFormControl1.FluentDesignForm = Me
			Me.FluentDesignFormControl1.Location = New System.Drawing.Point(0, 0)
			Me.FluentDesignFormControl1.Manager = Me.FluentFormDefaultManager1
			Me.FluentDesignFormControl1.Margin = New System.Windows.Forms.Padding(2)
			Me.FluentDesignFormControl1.Name = "FluentDesignFormControl1"
			Me.FluentDesignFormControl1.Size = New System.Drawing.Size(897, 31)
			Me.FluentDesignFormControl1.TabIndex = 2
			Me.FluentDesignFormControl1.TabStop = False
			'
			'FluentFormDefaultManager1
			'
			Me.FluentFormDefaultManager1.DockingEnabled = False
			Me.FluentFormDefaultManager1.Form = Me
			'
			'TextEdit1
			'
			Me.TextEdit1.Location = New System.Drawing.Point(147, 90)
			Me.TextEdit1.MenuManager = Me.FluentFormDefaultManager1
			Me.TextEdit1.Name = "TextEdit1"
			Me.TextEdit1.Size = New System.Drawing.Size(100, 20)
			Me.TextEdit1.TabIndex = 2
			'
			'frmForgottenZVARGB
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(897, 686)
			Me.ControlContainer = Me.FluentDesignFormContainer1
			Me.Controls.Add(Me.FluentDesignFormContainer1)
			Me.Controls.Add(Me.AccordionControl1)
			Me.Controls.Add(Me.TextEdit1)
			Me.Controls.Add(Me.FluentDesignFormControl1)
			Me.FluentDesignFormControl = Me.FluentDesignFormControl1
			Me.Margin = New System.Windows.Forms.Padding(2)
			Me.MinimumSize = New System.Drawing.Size(899, 687)
			Me.Name = "frmForgottenZVARGB"
			Me.NavigationControl = Me.AccordionControl1
			Me.Text = "Fehlende Zwischenverdienst und Arbeitgeberbescheinigung"
			Me.FluentDesignFormContainer1.ResumeLayout(False)
			CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
			Me.PanelControl2.ResumeLayout(False)
			CType(Me.grdZVARGBEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvZVARGBEmployees, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.AccordionControl1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.AccordionControl1.ResumeLayout(False)
			Me.AccordionContentContainer1.ResumeLayout(False)
			CType(Me.pnlFilter, System.ComponentModel.ISupportInitialize).EndInit()
			Me.pnlFilter.ResumeLayout(False)
			CType(Me.SeparatorControl2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.SeparatorControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueMandant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueMonth.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.FluentDesignFormControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.FluentFormDefaultManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.TextEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents FluentDesignFormContainer1 As DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer
		Friend WithEvents AccordionControl1 As DevExpress.XtraBars.Navigation.AccordionControl
		Friend WithEvents AccordionControlElement1 As DevExpress.XtraBars.Navigation.AccordionControlElement
		Friend WithEvents FluentDesignFormControl1 As DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl
		Friend WithEvents FluentFormDefaultManager1 As DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager
		Friend WithEvents grdZVARGBEmployees As DevExpress.XtraGrid.GridControl
		Friend WithEvents gvZVARGBEmployees As DevExpress.XtraGrid.Views.Grid.GridView
		Friend WithEvents pnlFilter As DevExpress.XtraEditors.PanelControl
		Friend WithEvents AccordionContentContainer1 As DevExpress.XtraBars.Navigation.AccordionContentContainer
		Friend WithEvents AccordionControlElement2 As DevExpress.XtraBars.Navigation.AccordionControlElement
		Friend WithEvents TextEdit1 As DevExpress.XtraEditors.TextEdit
		Friend WithEvents lueMandant As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lblMonat As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lblMandant As DevExpress.XtraEditors.LabelControl
		Friend WithEvents lueMonth As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lueYear As DevExpress.XtraEditors.LookUpEdit
		Friend WithEvents lblJahr As DevExpress.XtraEditors.LabelControl
		Friend WithEvents btnOpenZVForm As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents btnOpenAGForm As DevExpress.XtraEditors.SimpleButton
		Friend WithEvents SeparatorControl2 As DevExpress.XtraEditors.SeparatorControl
		Friend WithEvents SeparatorControl1 As DevExpress.XtraEditors.SeparatorControl
		Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
		Friend WithEvents lblHeaderInfo As DevExpress.XtraEditors.LabelControl
	End Class

End Namespace

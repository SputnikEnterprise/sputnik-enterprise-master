Namespace UI


  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucPageSelectBank
    Inherits ucWizardPageBaseControl

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
			Me.components = New System.ComponentModel.Container()
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Me.Label1 = New System.Windows.Forms.Label()
			Me.grpCandidateData = New DevComponents.DotNetBar.Controls.GroupPanel()
			Me.lueBank = New DevExpress.XtraEditors.GridLookUpEdit()
			Me.gvBank = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grdBankDetail = New DevExpress.XtraGrid.GridControl()
			Me.gvBankDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.lblMitarbeiter = New DevExpress.XtraEditors.LabelControl()
			Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
			Me.grpCandidateData.SuspendLayout()
			CType(Me.lueBank.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvBank, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdBankDetail, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvBankDetail, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'Label1
			'
			Me.Label1.AutoSize = True
			Me.Label1.Location = New System.Drawing.Point(222, 98)
			Me.Label1.Name = "Label1"
			Me.Label1.Size = New System.Drawing.Size(37, 13)
			Me.Label1.TabIndex = 1
			Me.Label1.Text = "Page2"
			'
			'grpCandidateData
			'
			Me.grpCandidateData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grpCandidateData.BackColor = System.Drawing.Color.Transparent
			Me.grpCandidateData.CanvasColor = System.Drawing.SystemColors.Control
			Me.grpCandidateData.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Metro
			Me.grpCandidateData.Controls.Add(Me.lueBank)
			Me.grpCandidateData.Controls.Add(Me.grdBankDetail)
			Me.grpCandidateData.Controls.Add(Me.lblMitarbeiter)
			Me.grpCandidateData.Location = New System.Drawing.Point(5, 3)
			Me.grpCandidateData.Name = "grpCandidateData"
			Me.grpCandidateData.Size = New System.Drawing.Size(510, 247)
			'
			'
			'
			Me.grpCandidateData.Style.BackColorGradientAngle = 90
			Me.grpCandidateData.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderBottomWidth = 1
			Me.grpCandidateData.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
			Me.grpCandidateData.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderLeftWidth = 1
			Me.grpCandidateData.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderRightWidth = 1
			Me.grpCandidateData.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
			Me.grpCandidateData.Style.BorderTopWidth = 1
			Me.grpCandidateData.Style.CornerDiameter = 4
			Me.grpCandidateData.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
			Me.grpCandidateData.Style.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpCandidateData.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
			Me.grpCandidateData.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
			Me.grpCandidateData.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
			'
			'
			'
			Me.grpCandidateData.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
			'
			'
			'
			Me.grpCandidateData.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
			Me.grpCandidateData.TabIndex = 6
			Me.grpCandidateData.Text = "Bankdaten"
			'
			'lueBank
			'
			Me.lueBank.Location = New System.Drawing.Point(100, 6)
			Me.lueBank.Name = "lueBank"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			Me.lueBank.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Global.SP.MA.AdvancePaymentMng.My.Resources.Resources.Open, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
			Me.lueBank.Properties.View = Me.gvBank
			Me.lueBank.Size = New System.Drawing.Size(319, 21)
			Me.lueBank.TabIndex = 234
			'
			'gvBank
			'
			Me.gvBank.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvBank.Name = "gvBank"
			Me.gvBank.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvBank.OptionsView.ShowGroupPanel = False
			'
			'grdBankDetail
			'
			Me.grdBankDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdBankDetail.Location = New System.Drawing.Point(100, 33)
			Me.grdBankDetail.MainView = Me.gvBankDetail
			Me.grdBankDetail.Name = "grdBankDetail"
			Me.grdBankDetail.Size = New System.Drawing.Size(378, 173)
			Me.grdBankDetail.TabIndex = 233
			Me.grdBankDetail.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBankDetail})
			'
			'gvBankDetail
			'
			Me.gvBankDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvBankDetail.GridControl = Me.grdBankDetail
			Me.gvBankDetail.Name = "gvBankDetail"
			Me.gvBankDetail.OptionsBehavior.Editable = False
			Me.gvBankDetail.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvBankDetail.OptionsView.ShowGroupPanel = False
			'
			'lblMitarbeiter
			'
			Me.lblMitarbeiter.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblMitarbeiter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblMitarbeiter.Location = New System.Drawing.Point(2, 10)
			Me.lblMitarbeiter.Name = "lblMitarbeiter"
			Me.lblMitarbeiter.Size = New System.Drawing.Size(92, 13)
			Me.lblMitarbeiter.TabIndex = 232
			Me.lblMitarbeiter.Text = "Bankdaten"
			'
			'ErrorProvider
			'
			Me.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
			Me.ErrorProvider.ContainerControl = Me
			'
			'ucPageSelectBank
			'
			Me.Appearance.BackColor = System.Drawing.Color.White
			Me.Appearance.Options.UseBackColor = True
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpCandidateData)
			Me.Controls.Add(Me.Label1)
			Me.Name = "ucPageSelectBank"
			Me.Size = New System.Drawing.Size(523, 261)
			Me.grpCandidateData.ResumeLayout(False)
			CType(Me.lueBank.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvBank, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdBankDetail, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvBankDetail, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)
			Me.PerformLayout()

		End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblMitarbeiter As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grdBankDetail As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvBankDetail As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents lueBank As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents gvBank As DevExpress.XtraGrid.Views.Grid.GridView
    Private WithEvents grpCandidateData As DevComponents.DotNetBar.Controls.GroupPanel

  End Class


End Namespace

Namespace UI

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucBankData
    Inherits ucBaseControlBottomTab

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
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucBankData))
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Me.grdBank = New DevExpress.XtraGrid.GridControl()
			Me.gvBankDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.grpBank = New DevExpress.XtraEditors.GroupControl()
			Me.btnShowBank = New DevExpress.XtraEditors.SimpleButton()
			Me.btnAddBank = New DevExpress.XtraEditors.SimpleButton()
			Me.pnlContents = New System.Windows.Forms.Panel()
			Me.lblBankdaten = New DevExpress.XtraEditors.LabelControl()
			Me.lueBank = New DevExpress.XtraEditors.GridLookUpEdit()
			Me.gvBank = New DevExpress.XtraGrid.Views.Grid.GridView()
			CType(Me.grdBank, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvBankDetail, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grpBank, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.grpBank.SuspendLayout()
			Me.pnlContents.SuspendLayout()
			CType(Me.lueBank.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvBank, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			'
			'grdBank
			'
			Me.grdBank.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
							Or System.Windows.Forms.AnchorStyles.Left) _
							Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdBank.Location = New System.Drawing.Point(94, 45)
			Me.grdBank.MainView = Me.gvBankDetail
			Me.grdBank.Name = "grdBank"
			Me.grdBank.Size = New System.Drawing.Size(699, 178)
			Me.grdBank.TabIndex = 1
			Me.grdBank.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvBankDetail})
			'
			'gvBankDetail
			'
			Me.gvBankDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvBankDetail.GridControl = Me.grdBank
			Me.gvBankDetail.Name = "gvBankDetail"
			Me.gvBankDetail.OptionsBehavior.Editable = False
			Me.gvBankDetail.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvBankDetail.OptionsView.ShowGroupPanel = False
			'
			'grpBank
			'
			Me.grpBank.Appearance.BackColor = System.Drawing.Color.White
			Me.grpBank.Appearance.BackColor2 = System.Drawing.Color.White
			Me.grpBank.Appearance.BorderColor = System.Drawing.Color.White
			Me.grpBank.Appearance.Options.UseBackColor = True
			Me.grpBank.Appearance.Options.UseBorderColor = True
			Me.grpBank.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
			Me.grpBank.AppearanceCaption.Options.UseFont = True
			Me.grpBank.Controls.Add(Me.btnShowBank)
			Me.grpBank.Controls.Add(Me.btnAddBank)
			Me.grpBank.Controls.Add(Me.pnlContents)
			Me.grpBank.Dock = System.Windows.Forms.DockStyle.Fill
			Me.grpBank.Location = New System.Drawing.Point(0, 0)
			Me.grpBank.Name = "grpBank"
			Me.grpBank.Size = New System.Drawing.Size(824, 262)
			Me.grpBank.TabIndex = 272
			Me.grpBank.Text = "Bankverbindung"
			'
			'btnShowBank
			'
			Me.btnShowBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnShowBank.Image = CType(resources.GetObject("btnShowBank.Image"), System.Drawing.Image)
			Me.btnShowBank.Location = New System.Drawing.Point(759, 3)
			Me.btnShowBank.Name = "btnShowBank"
			Me.btnShowBank.Size = New System.Drawing.Size(27, 15)
			Me.btnShowBank.TabIndex = 238
			'
			'btnAddBank
			'
			Me.btnAddBank.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnAddBank.Image = CType(resources.GetObject("btnAddBank.Image"), System.Drawing.Image)
			Me.btnAddBank.Location = New System.Drawing.Point(792, 3)
			Me.btnAddBank.Name = "btnAddBank"
			Me.btnAddBank.Size = New System.Drawing.Size(27, 15)
			Me.btnAddBank.TabIndex = 4
			'
			'pnlContents
			'
			Me.pnlContents.Controls.Add(Me.grdBank)
			Me.pnlContents.Controls.Add(Me.lblBankdaten)
			Me.pnlContents.Controls.Add(Me.lueBank)
			Me.pnlContents.Dock = System.Windows.Forms.DockStyle.Fill
			Me.pnlContents.Location = New System.Drawing.Point(2, 21)
			Me.pnlContents.Name = "pnlContents"
			Me.pnlContents.Size = New System.Drawing.Size(820, 239)
			Me.pnlContents.TabIndex = 237
			'
			'lblBankdaten
			'
			Me.lblBankdaten.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblBankdaten.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblBankdaten.Location = New System.Drawing.Point(18, 22)
			Me.lblBankdaten.Name = "lblBankdaten"
			Me.lblBankdaten.Size = New System.Drawing.Size(70, 13)
			Me.lblBankdaten.TabIndex = 235
			Me.lblBankdaten.Text = "Bankdaten"
			'
			'lueBank
			'
			Me.lueBank.Location = New System.Drawing.Point(94, 18)
			Me.lueBank.Name = "lueBank"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			Me.lueBank.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Global.SP.MA.AdvancePaymentMng.My.Resources.Resources.Open, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
			Me.lueBank.Properties.View = Me.gvBank
			Me.lueBank.Size = New System.Drawing.Size(319, 21)
			Me.lueBank.TabIndex = 236
			'
			'gvBank
			'
			Me.gvBank.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvBank.Name = "gvBank"
			Me.gvBank.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvBank.OptionsView.ShowGroupPanel = False
			'
			'ucBankData
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.grpBank)
			Me.Name = "ucBankData"
			Me.Size = New System.Drawing.Size(824, 262)
			CType(Me.grdBank, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvBankDetail, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grpBank, System.ComponentModel.ISupportInitialize).EndInit()
			Me.grpBank.ResumeLayout(False)
			Me.pnlContents.ResumeLayout(False)
			CType(Me.lueBank.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvBank, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
    Friend WithEvents grdBank As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvBankDetail As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents grpBank As DevExpress.XtraEditors.GroupControl
    Friend WithEvents btnAddBank As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lueBank As DevExpress.XtraEditors.GridLookUpEdit
    Friend WithEvents gvBank As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblBankdaten As DevExpress.XtraEditors.LabelControl
		Friend WithEvents pnlContents As System.Windows.Forms.Panel
		Friend WithEvents btnShowBank As DevExpress.XtraEditors.SimpleButton

  End Class

End Namespace

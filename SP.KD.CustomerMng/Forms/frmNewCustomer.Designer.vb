
Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmNewCustomer
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
			Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
			Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim EditorButtonImageOptions2 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
			Dim SerializableAppearanceObject5 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject6 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject7 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim SerializableAppearanceObject8 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
			Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNewCustomer))
			Me.txtCompany1 = New DevExpress.XtraEditors.TextEdit()
			Me.lblFirma = New DevExpress.XtraEditors.LabelControl()
			Me.luePostcode = New DevExpress.XtraEditors.LookUpEdit()
			Me.lueCountry = New DevExpress.XtraEditors.LookUpEdit()
			Me.txtLocation = New DevExpress.XtraEditors.TextEdit()
			Me.lblort = New DevExpress.XtraEditors.LabelControl()
			Me.txtStreet = New DevExpress.XtraEditors.TextEdit()
			Me.lblStrasse = New DevExpress.XtraEditors.LabelControl()
			Me.lblland = New DevExpress.XtraEditors.LabelControl()
			Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
			Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
			Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider()
			Me.lblplz = New DevExpress.XtraEditors.LabelControl()
			Me.grdExistingCustomers = New DevExpress.XtraGrid.GridControl()
			Me.gvExistingCustomers = New DevExpress.XtraGrid.Views.Grid.GridView()
			Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
			CType(Me.txtCompany1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.luePostcode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.lueCountry.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtLocation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.txtStreet.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.grdExistingCustomers, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.gvExistingCustomers, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.sccMain.SuspendLayout()
			Me.SuspendLayout()
			'
			'txtCompany1
			'
			Me.txtCompany1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.txtCompany1.Location = New System.Drawing.Point(108, 13)
			Me.txtCompany1.Name = "txtCompany1"
			Me.txtCompany1.Size = New System.Drawing.Size(350, 20)
			Me.txtCompany1.TabIndex = 1
			'
			'lblFirma
			'
			Me.lblFirma.Appearance.Options.UseTextOptions = True
			Me.lblFirma.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblFirma.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblFirma.Location = New System.Drawing.Point(5, 17)
			Me.lblFirma.Name = "lblFirma"
			Me.lblFirma.Size = New System.Drawing.Size(97, 13)
			Me.lblFirma.TabIndex = 2
			Me.lblFirma.Text = "Firma"
			'
			'luePostcode
			'
			Me.luePostcode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.luePostcode.Location = New System.Drawing.Point(355, 65)
			Me.luePostcode.Name = "luePostcode"
			SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject1.Options.UseForeColor = True
			SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject2.Options.UseForeColor = True
			SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject3.Options.UseForeColor = True
			SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject4.Options.UseForeColor = True
			Me.luePostcode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
			Me.luePostcode.Properties.ShowFooter = False
			Me.luePostcode.Size = New System.Drawing.Size(103, 20)
			Me.luePostcode.TabIndex = 4
			'
			'lueCountry
			'
			Me.lueCountry.Location = New System.Drawing.Point(108, 65)
			Me.lueCountry.Name = "lueCountry"
			SerializableAppearanceObject5.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject5.Options.UseForeColor = True
			SerializableAppearanceObject6.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject6.Options.UseForeColor = True
			SerializableAppearanceObject7.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject7.Options.UseForeColor = True
			SerializableAppearanceObject8.ForeColor = System.Drawing.Color.Red
			SerializableAppearanceObject8.Options.UseForeColor = True
			Me.lueCountry.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions2, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject5, SerializableAppearanceObject6, SerializableAppearanceObject7, SerializableAppearanceObject8, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
			Me.lueCountry.Properties.ShowFooter = False
			Me.lueCountry.Size = New System.Drawing.Size(115, 20)
			Me.lueCountry.TabIndex = 3
			'
			'txtLocation
			'
			Me.txtLocation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.txtLocation.Location = New System.Drawing.Point(108, 91)
			Me.txtLocation.Name = "txtLocation"
			Me.txtLocation.Size = New System.Drawing.Size(350, 20)
			Me.txtLocation.TabIndex = 5
			'
			'lblort
			'
			Me.lblort.Appearance.Options.UseTextOptions = True
			Me.lblort.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblort.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblort.Location = New System.Drawing.Point(5, 95)
			Me.lblort.Name = "lblort"
			Me.lblort.Size = New System.Drawing.Size(97, 13)
			Me.lblort.TabIndex = 210
			Me.lblort.Text = "Ort"
			'
			'txtStreet
			'
			Me.txtStreet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.txtStreet.Location = New System.Drawing.Point(108, 39)
			Me.txtStreet.Name = "txtStreet"
			Me.txtStreet.Size = New System.Drawing.Size(350, 20)
			Me.txtStreet.TabIndex = 2
			'
			'lblStrasse
			'
			Me.lblStrasse.Appearance.Options.UseTextOptions = True
			Me.lblStrasse.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblStrasse.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblStrasse.Location = New System.Drawing.Point(5, 43)
			Me.lblStrasse.Name = "lblStrasse"
			Me.lblStrasse.Size = New System.Drawing.Size(97, 13)
			Me.lblStrasse.TabIndex = 208
			Me.lblStrasse.Text = "Strasse"
			'
			'lblland
			'
			Me.lblland.Appearance.Options.UseTextOptions = True
			Me.lblland.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblland.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblland.Location = New System.Drawing.Point(5, 69)
			Me.lblland.Name = "lblland"
			Me.lblland.Size = New System.Drawing.Size(97, 13)
			Me.lblland.TabIndex = 212
			Me.lblland.Text = "Land"
			'
			'btnSave
			'
			Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnSave.Location = New System.Drawing.Point(506, 16)
			Me.btnSave.Name = "btnSave"
			Me.btnSave.Size = New System.Drawing.Size(86, 24)
			Me.btnSave.TabIndex = 6
			Me.btnSave.Text = "Speichern"
			'
			'btnClose
			'
			Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.btnClose.Location = New System.Drawing.Point(506, 89)
			Me.btnClose.Name = "btnClose"
			Me.btnClose.Size = New System.Drawing.Size(86, 24)
			Me.btnClose.TabIndex = 7
			Me.btnClose.Text = "Abbrechen"
			Me.btnClose.Visible = False
			'
			'ErrorProvider1
			'
			Me.ErrorProvider1.BlinkRate = 0
			Me.ErrorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
			Me.ErrorProvider1.ContainerControl = Me
			'
			'lblplz
			'
			Me.lblplz.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.lblplz.Appearance.Options.UseTextOptions = True
			Me.lblplz.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			Me.lblplz.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
			Me.lblplz.Location = New System.Drawing.Point(229, 69)
			Me.lblplz.Name = "lblplz"
			Me.lblplz.Size = New System.Drawing.Size(120, 13)
			Me.lblplz.TabIndex = 217
			Me.lblplz.Text = "PLZ"
			'
			'grdExistingCustomers
			'
			Me.grdExistingCustomers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.grdExistingCustomers.Location = New System.Drawing.Point(5, 5)
			Me.grdExistingCustomers.MainView = Me.gvExistingCustomers
			Me.grdExistingCustomers.Name = "grdExistingCustomers"
			Me.grdExistingCustomers.Size = New System.Drawing.Size(605, 330)
			Me.grdExistingCustomers.TabIndex = 218
			Me.grdExistingCustomers.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvExistingCustomers})
			'
			'gvExistingCustomers
			'
			Me.gvExistingCustomers.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
			Me.gvExistingCustomers.GridControl = Me.grdExistingCustomers
			Me.gvExistingCustomers.Name = "gvExistingCustomers"
			Me.gvExistingCustomers.OptionsBehavior.Editable = False
			Me.gvExistingCustomers.OptionsSelection.EnableAppearanceFocusedCell = False
			Me.gvExistingCustomers.OptionsView.ShowGroupPanel = False
			'
			'sccMain
			'
			Me.sccMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
			Me.sccMain.Horizontal = False
			Me.sccMain.IsSplitterFixed = True
			Me.sccMain.Location = New System.Drawing.Point(5, 5)
			Me.sccMain.Name = "sccMain"
			Me.sccMain.Panel1.Controls.Add(Me.txtCompany1)
			Me.sccMain.Panel1.Controls.Add(Me.lblplz)
			Me.sccMain.Panel1.Controls.Add(Me.lblFirma)
			Me.sccMain.Panel1.Controls.Add(Me.btnClose)
			Me.sccMain.Panel1.Controls.Add(Me.lblland)
			Me.sccMain.Panel1.Controls.Add(Me.btnSave)
			Me.sccMain.Panel1.Controls.Add(Me.lblStrasse)
			Me.sccMain.Panel1.Controls.Add(Me.luePostcode)
			Me.sccMain.Panel1.Controls.Add(Me.txtStreet)
			Me.sccMain.Panel1.Controls.Add(Me.lueCountry)
			Me.sccMain.Panel1.Controls.Add(Me.lblort)
			Me.sccMain.Panel1.Controls.Add(Me.txtLocation)
			Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
			Me.sccMain.Panel1.Text = "Panel1"
			Me.sccMain.Panel2.Controls.Add(Me.grdExistingCustomers)
			Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
			Me.sccMain.Panel2.Text = "Panel2"
			Me.sccMain.Size = New System.Drawing.Size(615, 478)
			Me.sccMain.SplitterPosition = 133
			Me.sccMain.TabIndex = 219
			Me.sccMain.Text = "SplitContainerControl1"
			'
			'frmNewCustomer
			'
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(625, 488)
			Me.Controls.Add(Me.sccMain)
			Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
			Me.MinimumSize = New System.Drawing.Size(635, 520)
			Me.Name = "frmNewCustomer"
			Me.Padding = New System.Windows.Forms.Padding(5)
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
			Me.Text = "Neuer Kunde"
			CType(Me.txtCompany1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.luePostcode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.lueCountry.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtLocation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.txtStreet.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.grdExistingCustomers, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.gvExistingCustomers, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
			Me.sccMain.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub
		Friend WithEvents txtCompany1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblFirma As DevExpress.XtraEditors.LabelControl
    Friend WithEvents luePostcode As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lueCountry As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents txtLocation As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblort As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtStreet As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblStrasse As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblland As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents lblplz As DevExpress.XtraEditors.LabelControl
    Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
        Friend WithEvents grdExistingCustomers As DevExpress.XtraGrid.GridControl
        Friend WithEvents gvExistingCustomers As DevExpress.XtraGrid.Views.Grid.GridView
    End Class

End Namespace

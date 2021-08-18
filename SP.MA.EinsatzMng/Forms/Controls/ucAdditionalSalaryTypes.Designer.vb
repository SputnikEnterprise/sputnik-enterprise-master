Namespace UI


  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class ucAdditionalSalaryTypes
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
      Me.components = New System.ComponentModel.Container()
      Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucAdditionalSalaryTypes))
      Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
      Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
      Me.lblMitarbeiter = New DevExpress.XtraEditors.LabelControl()
      Me.grdEmployeeSalaryTypeData = New DevExpress.XtraGrid.GridControl()
      Me.gvEmployeeSalaryTypeData = New DevExpress.XtraGrid.Views.Grid.GridView()
      Me.lblKunde = New DevExpress.XtraEditors.LabelControl()
      Me.grdCustomerSalaryTypeData = New DevExpress.XtraGrid.GridControl()
      Me.gvCustomerSalaryTypeData = New DevExpress.XtraGrid.Views.Grid.GridView()
      Me.lblLohnart = New DevExpress.XtraEditors.LabelControl()
      Me.lblBasis = New DevExpress.XtraEditors.LabelControl()
      Me.lueLAData = New DevExpress.XtraEditors.LookUpEdit()
      Me.lblProzent = New DevExpress.XtraEditors.LabelControl()
      Me.lblAnsatz = New DevExpress.XtraEditors.LabelControl()
      Me.lblBetrag = New DevExpress.XtraEditors.LabelControl()
      Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
      Me.txtBasis = New DevExpress.XtraEditors.TextEdit()
      Me.txtAnsatz = New DevExpress.XtraEditors.TextEdit()
      Me.txtBetrag = New DevExpress.XtraEditors.TextEdit()
      Me.btnAddNewESMALA = New DevExpress.XtraEditors.SimpleButton()
      Me.btnAddNewESKDLA = New DevExpress.XtraEditors.SimpleButton()
      Me.lblInfo = New DevExpress.XtraEditors.LabelControl()
      Me.errorProviderLAData = New System.Windows.Forms.ErrorProvider(Me.components)
      Me.lblDayMonthStd = New DevExpress.XtraEditors.LabelControl()
      Me.lueDayMonthStd = New DevExpress.XtraEditors.LookUpEdit()
      Me.lblESLohn = New DevExpress.XtraEditors.LabelControl()
      Me.lueESSalary = New DevExpress.XtraEditors.LookUpEdit()
      CType(Me.grdEmployeeSalaryTypeData, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvEmployeeSalaryTypeData, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.grdCustomerSalaryTypeData, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.gvCustomerSalaryTypeData, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.lueLAData.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.txtBasis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.txtAnsatz.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.txtBetrag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.errorProviderLAData, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.lueDayMonthStd.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      CType(Me.lueESSalary.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'lblMitarbeiter
      '
      Me.lblMitarbeiter.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblMitarbeiter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblMitarbeiter.Location = New System.Drawing.Point(20, 19)
      Me.lblMitarbeiter.Name = "lblMitarbeiter"
      Me.lblMitarbeiter.Size = New System.Drawing.Size(78, 13)
      Me.lblMitarbeiter.TabIndex = 233
      Me.lblMitarbeiter.Text = "Kandidat"
      '
      'grdEmployeeSalaryTypeData
      '
      Me.grdEmployeeSalaryTypeData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.grdEmployeeSalaryTypeData.Location = New System.Drawing.Point(104, 19)
      Me.grdEmployeeSalaryTypeData.MainView = Me.gvEmployeeSalaryTypeData
      Me.grdEmployeeSalaryTypeData.Name = "grdEmployeeSalaryTypeData"
      Me.grdEmployeeSalaryTypeData.Size = New System.Drawing.Size(553, 87)
      Me.grdEmployeeSalaryTypeData.TabIndex = 234
      Me.grdEmployeeSalaryTypeData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvEmployeeSalaryTypeData})
      '
      'gvEmployeeSalaryTypeData
      '
      Me.gvEmployeeSalaryTypeData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvEmployeeSalaryTypeData.GridControl = Me.grdEmployeeSalaryTypeData
      Me.gvEmployeeSalaryTypeData.Name = "gvEmployeeSalaryTypeData"
      Me.gvEmployeeSalaryTypeData.OptionsBehavior.Editable = False
      Me.gvEmployeeSalaryTypeData.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvEmployeeSalaryTypeData.OptionsView.ShowGroupPanel = False
      '
      'lblKunde
      '
      Me.lblKunde.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblKunde.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblKunde.Location = New System.Drawing.Point(20, 112)
      Me.lblKunde.Name = "lblKunde"
      Me.lblKunde.Size = New System.Drawing.Size(78, 13)
      Me.lblKunde.TabIndex = 235
      Me.lblKunde.Text = "Kunde"
      '
      'grdCustomerSalaryTypeData
      '
      Me.grdCustomerSalaryTypeData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.grdCustomerSalaryTypeData.Location = New System.Drawing.Point(104, 112)
      Me.grdCustomerSalaryTypeData.MainView = Me.gvCustomerSalaryTypeData
      Me.grdCustomerSalaryTypeData.Name = "grdCustomerSalaryTypeData"
      Me.grdCustomerSalaryTypeData.Size = New System.Drawing.Size(553, 87)
      Me.grdCustomerSalaryTypeData.TabIndex = 236
      Me.grdCustomerSalaryTypeData.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvCustomerSalaryTypeData})
      '
      'gvCustomerSalaryTypeData
      '
      Me.gvCustomerSalaryTypeData.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      Me.gvCustomerSalaryTypeData.GridControl = Me.grdCustomerSalaryTypeData
      Me.gvCustomerSalaryTypeData.Name = "gvCustomerSalaryTypeData"
      Me.gvCustomerSalaryTypeData.OptionsBehavior.Editable = False
      Me.gvCustomerSalaryTypeData.OptionsSelection.EnableAppearanceFocusedCell = False
      Me.gvCustomerSalaryTypeData.OptionsView.ShowGroupPanel = False
      '
      'lblLohnart
      '
      Me.lblLohnart.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblLohnart.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblLohnart.Location = New System.Drawing.Point(20, 246)
      Me.lblLohnart.Name = "lblLohnart"
      Me.lblLohnart.Size = New System.Drawing.Size(78, 13)
      Me.lblLohnart.TabIndex = 237
      Me.lblLohnart.Text = "Lohnart"
      '
      'lblBasis
      '
      Me.lblBasis.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblBasis.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblBasis.Location = New System.Drawing.Point(20, 297)
      Me.lblBasis.Name = "lblBasis"
      Me.lblBasis.Size = New System.Drawing.Size(78, 13)
      Me.lblBasis.TabIndex = 238
      Me.lblBasis.Text = "Basis"
      '
      'lueLAData
      '
      Me.lueLAData.Location = New System.Drawing.Point(104, 242)
      Me.lueLAData.Name = "lueLAData"
      SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
      SerializableAppearanceObject1.Options.UseForeColor = True
      Me.lueLAData.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
      Me.lueLAData.Properties.ShowFooter = False
      Me.lueLAData.Size = New System.Drawing.Size(424, 20)
      Me.lueLAData.TabIndex = 244
      '
      'lblProzent
      '
      Me.lblProzent.Location = New System.Drawing.Point(190, 324)
      Me.lblProzent.Name = "lblProzent"
      Me.lblProzent.Size = New System.Drawing.Size(11, 13)
      Me.lblProzent.TabIndex = 253
      Me.lblProzent.Text = "%"
      '
      'lblAnsatz
      '
      Me.lblAnsatz.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblAnsatz.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblAnsatz.Location = New System.Drawing.Point(20, 324)
      Me.lblAnsatz.Name = "lblAnsatz"
      Me.lblAnsatz.Size = New System.Drawing.Size(78, 13)
      Me.lblAnsatz.TabIndex = 254
      Me.lblAnsatz.Text = "Ansatz"
      '
      'lblBetrag
      '
      Me.lblBetrag.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblBetrag.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblBetrag.Location = New System.Drawing.Point(220, 324)
      Me.lblBetrag.Name = "lblBetrag"
      Me.lblBetrag.Size = New System.Drawing.Size(69, 13)
      Me.lblBetrag.TabIndex = 256
      Me.lblBetrag.Text = "Betrag"
      '
      'btnSave
      '
      Me.btnSave.Location = New System.Drawing.Point(439, 317)
      Me.btnSave.Name = "btnSave"
      Me.btnSave.Size = New System.Drawing.Size(89, 23)
      Me.btnSave.TabIndex = 258
      Me.btnSave.Text = "Speichern"
      '
      'txtBasis
      '
      Me.txtBasis.Location = New System.Drawing.Point(105, 294)
      Me.txtBasis.Name = "txtBasis"
      Me.txtBasis.Properties.Appearance.Options.UseTextOptions = True
      Me.txtBasis.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.txtBasis.Properties.Mask.EditMask = "n2"
      Me.txtBasis.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
      Me.txtBasis.Properties.Mask.UseMaskAsDisplayFormat = True
      Me.txtBasis.Size = New System.Drawing.Size(80, 20)
      Me.txtBasis.TabIndex = 259
      '
      'txtAnsatz
      '
      Me.txtAnsatz.Location = New System.Drawing.Point(104, 320)
      Me.txtAnsatz.Name = "txtAnsatz"
      Me.txtAnsatz.Properties.Appearance.Options.UseTextOptions = True
      Me.txtAnsatz.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.txtAnsatz.Properties.Mask.EditMask = "n2"
      Me.txtAnsatz.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
      Me.txtAnsatz.Properties.Mask.UseMaskAsDisplayFormat = True
      Me.txtAnsatz.Size = New System.Drawing.Size(80, 20)
      Me.txtAnsatz.TabIndex = 260
      '
      'txtBetrag
      '
      Me.txtBetrag.Location = New System.Drawing.Point(295, 320)
      Me.txtBetrag.Name = "txtBetrag"
      Me.txtBetrag.Properties.Appearance.Options.UseTextOptions = True
      Me.txtBetrag.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.txtBetrag.Properties.Mask.EditMask = "n2"
      Me.txtBetrag.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
      Me.txtBetrag.Properties.Mask.UseMaskAsDisplayFormat = True
      Me.txtBetrag.Size = New System.Drawing.Size(121, 20)
      Me.txtBetrag.TabIndex = 261
      '
      'btnAddNewESMALA
      '
      Me.btnAddNewESMALA.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddNewESMALA.Image = CType(resources.GetObject("btnAddNewESMALA.Image"), System.Drawing.Image)
      Me.btnAddNewESMALA.Location = New System.Drawing.Point(664, 19)
      Me.btnAddNewESMALA.Name = "btnAddNewESMALA"
      Me.btnAddNewESMALA.Size = New System.Drawing.Size(27, 15)
      Me.btnAddNewESMALA.TabIndex = 272
      '
      'btnAddNewESKDLA
      '
      Me.btnAddNewESKDLA.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.btnAddNewESKDLA.Image = CType(resources.GetObject("btnAddNewESKDLA.Image"), System.Drawing.Image)
      Me.btnAddNewESKDLA.Location = New System.Drawing.Point(663, 112)
      Me.btnAddNewESKDLA.Name = "btnAddNewESKDLA"
      Me.btnAddNewESKDLA.Size = New System.Drawing.Size(27, 15)
      Me.btnAddNewESKDLA.TabIndex = 273
      '
      'lblInfo
      '
      Me.lblInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblInfo.Location = New System.Drawing.Point(319, 272)
      Me.lblInfo.Name = "lblInfo"
      Me.lblInfo.Size = New System.Drawing.Size(207, 13)
      Me.lblInfo.TabIndex = 274
      '
      'errorProviderLAData
      '
      Me.errorProviderLAData.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
      Me.errorProviderLAData.ContainerControl = Me
      '
      'lblDayMonthStd
      '
      Me.lblDayMonthStd.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblDayMonthStd.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblDayMonthStd.Location = New System.Drawing.Point(20, 271)
      Me.lblDayMonthStd.Name = "lblDayMonthStd"
      Me.lblDayMonthStd.Size = New System.Drawing.Size(78, 13)
      Me.lblDayMonthStd.TabIndex = 275
      Me.lblDayMonthStd.Text = "Pro"
      '
      'lueDayMonthStd
      '
      Me.lueDayMonthStd.Location = New System.Drawing.Point(104, 268)
      Me.lueDayMonthStd.Name = "lueDayMonthStd"
      SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
      SerializableAppearanceObject3.Options.UseForeColor = True
      Me.lueDayMonthStd.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
      Me.lueDayMonthStd.Properties.ShowFooter = False
      Me.lueDayMonthStd.Size = New System.Drawing.Size(424, 20)
      Me.lueDayMonthStd.TabIndex = 276
      '
      'lblESLohn
      '
      Me.lblESLohn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      Me.lblESLohn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
      Me.lblESLohn.Location = New System.Drawing.Point(20, 219)
      Me.lblESLohn.Name = "lblESLohn"
      Me.lblESLohn.Size = New System.Drawing.Size(78, 13)
      Me.lblESLohn.TabIndex = 277
      Me.lblESLohn.Text = "Einsatzlohn"
      '
      'lueESSalary
      '
      Me.lueESSalary.Location = New System.Drawing.Point(104, 216)
      Me.lueESSalary.Name = "lueESSalary"
      SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
      SerializableAppearanceObject2.Options.UseForeColor = True
      Me.lueESSalary.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
      Me.lueESSalary.Properties.ShowFooter = False
      Me.lueESSalary.Size = New System.Drawing.Size(424, 20)
      Me.lueESSalary.TabIndex = 278
      '
      'ucAdditionalSalaryTypes
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.Controls.Add(Me.lueESSalary)
      Me.Controls.Add(Me.lblESLohn)
      Me.Controls.Add(Me.lueDayMonthStd)
      Me.Controls.Add(Me.lblDayMonthStd)
      Me.Controls.Add(Me.lblInfo)
      Me.Controls.Add(Me.btnAddNewESKDLA)
      Me.Controls.Add(Me.btnAddNewESMALA)
      Me.Controls.Add(Me.txtBetrag)
      Me.Controls.Add(Me.txtAnsatz)
      Me.Controls.Add(Me.txtBasis)
      Me.Controls.Add(Me.btnSave)
      Me.Controls.Add(Me.lblBetrag)
      Me.Controls.Add(Me.lblAnsatz)
      Me.Controls.Add(Me.lblProzent)
      Me.Controls.Add(Me.lueLAData)
      Me.Controls.Add(Me.lblBasis)
      Me.Controls.Add(Me.lblLohnart)
      Me.Controls.Add(Me.grdCustomerSalaryTypeData)
      Me.Controls.Add(Me.lblKunde)
      Me.Controls.Add(Me.grdEmployeeSalaryTypeData)
      Me.Controls.Add(Me.lblMitarbeiter)
      Me.Name = "ucAdditionalSalaryTypes"
      Me.Size = New System.Drawing.Size(721, 358)
      CType(Me.grdEmployeeSalaryTypeData, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvEmployeeSalaryTypeData, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.grdCustomerSalaryTypeData, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.gvCustomerSalaryTypeData, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.lueLAData.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.txtBasis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.txtAnsatz.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.txtBetrag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.errorProviderLAData, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.lueDayMonthStd.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      CType(Me.lueESSalary.Properties, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

    End Sub
    Friend WithEvents lblMitarbeiter As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grdEmployeeSalaryTypeData As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvEmployeeSalaryTypeData As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblKunde As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grdCustomerSalaryTypeData As DevExpress.XtraGrid.GridControl
    Friend WithEvents gvCustomerSalaryTypeData As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblLohnart As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBasis As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueLAData As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblProzent As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAnsatz As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblBetrag As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtBasis As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtAnsatz As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtBetrag As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnAddNewESMALA As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnAddNewESKDLA As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents errorProviderLAData As System.Windows.Forms.ErrorProvider
    Friend WithEvents lueDayMonthStd As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblDayMonthStd As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblESLohn As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueESSalary As DevExpress.XtraEditors.LookUpEdit

  End Class

End Namespace

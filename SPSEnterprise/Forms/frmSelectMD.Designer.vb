Partial Class frmSelectMD
	'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> 

	Private components As System.ComponentModel.IContainer = Nothing

#Region "Vom Windows Form-Designer generierter Code "
	'<System.Diagnostics.DebuggerNonUserCode()> 
	'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
	'<System.Diagnostics.DebuggerNonUserCode()>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing AndAlso (components IsNot Nothing) Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Wird vom Windows Form-Designer benötigt.
	'Private components As System.ComponentModel.IContainer
	Public WithEvents lblHeader1 As System.Windows.Forms.Label
	Public WithEvents lblHeader2 As System.Windows.Forms.Label
	Public WithEvents Picture1 As DevExpress.XtraEditors.PanelControl
	Public WithEvents lblMandanten As System.Windows.Forms.Label
	'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
	'Das Verändern mit dem Windows Form-Designer ist nicht möglich.
	'Das Bearbeiten mit dem Code-Editor ist nicht möglich.

	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectMD))
		Me.Picture1 = New DevExpress.XtraEditors.PanelControl()
		Me.lblIsUpdateOK = New System.Windows.Forms.Label()
		Me.CmdXPCancel = New DevExpress.XtraEditors.SimpleButton()
		Me.lblHeader1 = New System.Windows.Forms.Label()
		Me.lblHeader2 = New System.Windows.Forms.Label()
		Me.lblMandanten = New System.Windows.Forms.Label()
		Me.lblMandantNameValue = New System.Windows.Forms.Label()
		Me.CmdXPOK = New DevExpress.XtraEditors.SimpleButton()
		Me.grdMDList = New DevExpress.XtraGrid.GridControl()
		Me.gvMDList = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.Picture1.SuspendLayout()
		CType(Me.grdMDList, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvMDList, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'Picture1
		'
		Me.Picture1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.Picture1.Appearance.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Picture1.Appearance.Options.UseBackColor = True
		Me.Picture1.Appearance.Options.UseForeColor = True
		Me.Picture1.Controls.Add(Me.lblIsUpdateOK)
		Me.Picture1.Controls.Add(Me.CmdXPCancel)
		Me.Picture1.Controls.Add(Me.lblHeader1)
		Me.Picture1.Controls.Add(Me.lblHeader2)
		Me.Picture1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Picture1.Dock = System.Windows.Forms.DockStyle.Top
		Me.Picture1.Location = New System.Drawing.Point(0, 0)
		Me.Picture1.Name = "Picture1"
		Me.Picture1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Picture1.Size = New System.Drawing.Size(533, 63)
		Me.Picture1.TabIndex = 7
		'
		'lblIsUpdateOK
		'
		Me.lblIsUpdateOK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblIsUpdateOK.BackColor = System.Drawing.Color.Transparent
		Me.lblIsUpdateOK.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblIsUpdateOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblIsUpdateOK.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lblIsUpdateOK.Location = New System.Drawing.Point(27, 42)
		Me.lblIsUpdateOK.Name = "lblIsUpdateOK"
		Me.lblIsUpdateOK.Size = New System.Drawing.Size(298, 14)
		Me.lblIsUpdateOK.TabIndex = 13
		Me.lblIsUpdateOK.Text = "Kein automatische Update!"
		'
		'CmdXPCancel
		'
		Me.CmdXPCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdXPCancel.Location = New System.Drawing.Point(421, 12)
		Me.CmdXPCancel.Name = "CmdXPCancel"
		Me.CmdXPCancel.Size = New System.Drawing.Size(100, 23)
		Me.CmdXPCancel.TabIndex = 0
		Me.CmdXPCancel.Text = "Schliessen"
		'
		'lblHeader1
		'
		Me.lblHeader1.AutoSize = True
		Me.lblHeader1.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader1.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblHeader1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader1.ForeColor = System.Drawing.Color.Black
		Me.lblHeader1.Location = New System.Drawing.Point(14, 12)
		Me.lblHeader1.Name = "lblHeader1"
		Me.lblHeader1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblHeader1.Size = New System.Drawing.Size(162, 13)
		Me.lblHeader1.TabIndex = 9
		Me.lblHeader1.Text = "Auswahl der Mandanten"
		'
		'lblHeader2
		'
		Me.lblHeader2.AutoSize = True
		Me.lblHeader2.BackColor = System.Drawing.Color.Transparent
		Me.lblHeader2.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblHeader2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblHeader2.ForeColor = System.Drawing.Color.Black
		Me.lblHeader2.Location = New System.Drawing.Point(27, 29)
		Me.lblHeader2.Name = "lblHeader2"
		Me.lblHeader2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblHeader2.Size = New System.Drawing.Size(228, 13)
		Me.lblHeader2.TabIndex = 8
		Me.lblHeader2.Text = "Bitte wählen Sie einen Mandanten aus."
		'
		'lblMandanten
		'
		Me.lblMandanten.AutoSize = True
		Me.lblMandanten.BackColor = System.Drawing.Color.Transparent
		Me.lblMandanten.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMandanten.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMandanten.Location = New System.Drawing.Point(14, 81)
		Me.lblMandanten.Name = "lblMandanten"
		Me.lblMandanten.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMandanten.Size = New System.Drawing.Size(61, 13)
		Me.lblMandanten.TabIndex = 3
		Me.lblMandanten.Text = "Mandanten"
		'
		'lblMandantNameValue
		'
		Me.lblMandantNameValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblMandantNameValue.BackColor = System.Drawing.Color.Transparent
		Me.lblMandantNameValue.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMandantNameValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblMandantNameValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.lblMandantNameValue.Location = New System.Drawing.Point(86, 80)
		Me.lblMandantNameValue.Name = "lblMandantNameValue"
		Me.lblMandantNameValue.Size = New System.Drawing.Size(312, 14)
		Me.lblMandantNameValue.TabIndex = 12
		Me.lblMandantNameValue.Text = "Keine Daten vorhanden..."
		Me.lblMandantNameValue.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'CmdXPOK
		'
		Me.CmdXPOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.CmdXPOK.Location = New System.Drawing.Point(421, 100)
		Me.CmdXPOK.Name = "CmdXPOK"
		Me.CmdXPOK.Size = New System.Drawing.Size(100, 23)
		Me.CmdXPOK.TabIndex = 0
		Me.CmdXPOK.Text = "Auswählen"
		'
		'grdMDList
		'
		Me.grdMDList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.grdMDList.Location = New System.Drawing.Point(14, 100)
		Me.grdMDList.MainView = Me.gvMDList
		Me.grdMDList.Name = "grdMDList"
		Me.grdMDList.Size = New System.Drawing.Size(384, 348)
		Me.grdMDList.TabIndex = 13
		Me.grdMDList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvMDList})
		'
		'gvMDList
		'
		Me.gvMDList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvMDList.GridControl = Me.grdMDList
		Me.gvMDList.Name = "gvMDList"
		Me.gvMDList.OptionsBehavior.Editable = False
		Me.gvMDList.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvMDList.OptionsView.ShowAutoFilterRow = True
		Me.gvMDList.OptionsView.ShowGroupPanel = False
		'
		'frmSelectMD
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(533, 460)
		Me.ControlBox = False
		Me.Controls.Add(Me.grdMDList)
		Me.Controls.Add(Me.CmdXPOK)
		Me.Controls.Add(Me.Picture1)
		Me.Controls.Add(Me.lblMandantNameValue)
		Me.Controls.Add(Me.lblMandanten)
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Location = New System.Drawing.Point(119, 217)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(549, 413)
		Me.Name = "frmSelectMD"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
		Me.Tag = "1"
		Me.Text = "Sputnik Enterprise Suite"
		CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.Picture1.ResumeLayout(False)
		Me.Picture1.PerformLayout()
		CType(Me.grdMDList, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvMDList, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Public WithEvents lblMandantNameValue As System.Windows.Forms.Label
	Friend WithEvents CmdXPCancel As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
	Friend WithEvents CmdXPOK As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
	Public WithEvents lblIsUpdateOK As Label
	Friend WithEvents grdMDList As DevExpress.XtraGrid.GridControl
	Friend WithEvents gvMDList As DevExpress.XtraGrid.Views.Grid.GridView
#End Region
End Class
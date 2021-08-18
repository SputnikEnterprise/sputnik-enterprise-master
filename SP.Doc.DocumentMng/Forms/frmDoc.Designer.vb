<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDoc
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDoc))
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.SplitContainerControl1 = New DevExpress.XtraEditors.SplitContainerControl()
		Me.lstCategorie = New DevExpress.XtraEditors.ListBoxControl()
		Me.gridDocuments = New DevExpress.XtraGrid.GridControl()
		Me.gvDocuments = New DevExpress.XtraGrid.Views.Grid.GridView()
		Me.grpdetails = New DevExpress.XtraEditors.GroupControl()
		Me.btnDeleteDocument = New DevExpress.XtraEditors.SimpleButton()
		Me.lblDocumentChanged = New DevExpress.XtraEditors.LabelControl()
		Me.lblDocumentCreated = New DevExpress.XtraEditors.LabelControl()
		Me.lblgaendert = New DevExpress.XtraEditors.LabelControl()
		Me.lblerstellt = New DevExpress.XtraEditors.LabelControl()
		Me.btnNewDocument = New DevExpress.XtraEditors.SimpleButton()
		Me.btnSave = New DevExpress.XtraEditors.SimpleButton()
		Me.lueCategory = New DevExpress.XtraEditors.LookUpEdit()
		Me.lblkategorie = New DevExpress.XtraEditors.LabelControl()
		Me.lblbeschreibung = New DevExpress.XtraEditors.LabelControl()
		Me.txtDescription = New DevExpress.XtraEditors.MemoEdit()
		Me.lbldatei = New DevExpress.XtraEditors.LabelControl()
		Me.lblzhd = New DevExpress.XtraEditors.LabelControl()
		Me.txtTitle = New DevExpress.XtraEditors.TextEdit()
		Me.lblbezeichnung = New DevExpress.XtraEditors.LabelControl()
		Me.txtFilePath = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.errorProviderDocumentMangement = New System.Windows.Forms.ErrorProvider()
		Me.lueZHDName = New DevExpress.XtraEditors.GridLookUpEdit()
		Me.gvZHDName = New DevExpress.XtraGrid.Views.Grid.GridView()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SplitContainerControl1.SuspendLayout()
		CType(Me.lstCategorie, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.grpdetails, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpdetails.SuspendLayout()
		CType(Me.lueCategory.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtTitle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtFilePath.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.errorProviderDocumentMangement, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lueZHDName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gvZHDName, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'sccMain
		'
		Me.sccMain.AllowDrop = True
		Me.sccMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.sccMain.Horizontal = False
		Me.sccMain.Location = New System.Drawing.Point(5, 5)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.SplitContainerControl1)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.AutoScroll = True
		Me.sccMain.Panel2.Controls.Add(Me.grpdetails)
		Me.sccMain.Panel2.MinSize = 300
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(753, 600)
		Me.sccMain.SplitterPosition = 295
		Me.sccMain.TabIndex = 0
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'SplitContainerControl1
		'
		Me.SplitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.SplitContainerControl1.Location = New System.Drawing.Point(0, 0)
		Me.SplitContainerControl1.Name = "SplitContainerControl1"
		Me.SplitContainerControl1.Padding = New System.Windows.Forms.Padding(5)
		Me.SplitContainerControl1.Panel1.Controls.Add(Me.lstCategorie)
		Me.SplitContainerControl1.Panel1.Text = "Panel1"
		Me.SplitContainerControl1.Panel2.Controls.Add(Me.gridDocuments)
		Me.SplitContainerControl1.Panel2.Text = "Panel2"
		Me.SplitContainerControl1.Size = New System.Drawing.Size(753, 295)
		Me.SplitContainerControl1.SplitterPosition = 209
		Me.SplitContainerControl1.TabIndex = 0
		Me.SplitContainerControl1.Text = "SplitContainerControl1"
		'
		'lstCategorie
		'
		Me.lstCategorie.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lstCategorie.Location = New System.Drawing.Point(0, 0)
		Me.lstCategorie.Name = "lstCategorie"
		Me.lstCategorie.Size = New System.Drawing.Size(209, 285)
		Me.lstCategorie.TabIndex = 0
		'
		'gridDocuments
		'
		Me.gridDocuments.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gridDocuments.Location = New System.Drawing.Point(0, 0)
		Me.gridDocuments.MainView = Me.gvDocuments
		Me.gridDocuments.Name = "gridDocuments"
		Me.gridDocuments.Size = New System.Drawing.Size(529, 285)
		Me.gridDocuments.TabIndex = 1
		Me.gridDocuments.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gvDocuments})
		'
		'gvDocuments
		'
		Me.gvDocuments.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvDocuments.GridControl = Me.gridDocuments
		Me.gvDocuments.Name = "gvDocuments"
		Me.gvDocuments.OptionsBehavior.Editable = False
		Me.gvDocuments.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvDocuments.OptionsView.ShowGroupPanel = False
		'
		'grpdetails
		'
		Me.grpdetails.AllowDrop = True
		Me.grpdetails.Controls.Add(Me.btnDeleteDocument)
		Me.grpdetails.Controls.Add(Me.lblDocumentChanged)
		Me.grpdetails.Controls.Add(Me.lblDocumentCreated)
		Me.grpdetails.Controls.Add(Me.lblgaendert)
		Me.grpdetails.Controls.Add(Me.lblerstellt)
		Me.grpdetails.Controls.Add(Me.btnNewDocument)
		Me.grpdetails.Controls.Add(Me.btnSave)
		Me.grpdetails.Controls.Add(Me.lueCategory)
		Me.grpdetails.Controls.Add(Me.lblkategorie)
		Me.grpdetails.Controls.Add(Me.lblbeschreibung)
		Me.grpdetails.Controls.Add(Me.txtDescription)
		Me.grpdetails.Controls.Add(Me.lbldatei)
		Me.grpdetails.Controls.Add(Me.lblzhd)
		Me.grpdetails.Controls.Add(Me.txtTitle)
		Me.grpdetails.Controls.Add(Me.lblbezeichnung)
		Me.grpdetails.Controls.Add(Me.txtFilePath)
		Me.grpdetails.Controls.Add(Me.lueZHDName)
		Me.grpdetails.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpdetails.Location = New System.Drawing.Point(5, 5)
		Me.grpdetails.Name = "grpdetails"
		Me.grpdetails.Size = New System.Drawing.Size(743, 290)
		Me.grpdetails.TabIndex = 2
		Me.grpdetails.Text = "Details"
		'
		'btnDeleteDocument
		'
		Me.btnDeleteDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnDeleteDocument.Image = CType(resources.GetObject("btnDeleteDocument.Image"), System.Drawing.Image)
		Me.btnDeleteDocument.Location = New System.Drawing.Point(614, 166)
		Me.btnDeleteDocument.Name = "btnDeleteDocument"
		Me.btnDeleteDocument.Size = New System.Drawing.Size(105, 28)
		Me.btnDeleteDocument.TabIndex = 8
		Me.btnDeleteDocument.Text = "Löschen"
		'
		'lblDocumentChanged
		'
		Me.lblDocumentChanged.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblDocumentChanged.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDocumentChanged.Location = New System.Drawing.Point(141, 263)
		Me.lblDocumentChanged.Name = "lblDocumentChanged"
		Me.lblDocumentChanged.Size = New System.Drawing.Size(467, 13)
		Me.lblDocumentChanged.TabIndex = 32
		Me.lblDocumentChanged.Text = "Geändert:"
		'
		'lblDocumentCreated
		'
		Me.lblDocumentCreated.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.lblDocumentCreated.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblDocumentCreated.Location = New System.Drawing.Point(141, 244)
		Me.lblDocumentCreated.Name = "lblDocumentCreated"
		Me.lblDocumentCreated.Size = New System.Drawing.Size(467, 13)
		Me.lblDocumentCreated.TabIndex = 31
		Me.lblDocumentCreated.Text = "Erstellt:"
		'
		'lblgaendert
		'
		Me.lblgaendert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblgaendert.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblgaendert.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblgaendert.Location = New System.Drawing.Point(16, 263)
		Me.lblgaendert.Name = "lblgaendert"
		Me.lblgaendert.Size = New System.Drawing.Size(119, 13)
		Me.lblgaendert.TabIndex = 30
		Me.lblgaendert.Text = "Geändert"
		'
		'lblerstellt
		'
		Me.lblerstellt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lblerstellt.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblerstellt.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblerstellt.Location = New System.Drawing.Point(16, 244)
		Me.lblerstellt.Name = "lblerstellt"
		Me.lblerstellt.Size = New System.Drawing.Size(119, 13)
		Me.lblerstellt.TabIndex = 29
		Me.lblerstellt.Text = "Erstellt"
		'
		'btnNewDocument
		'
		Me.btnNewDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnNewDocument.Image = CType(resources.GetObject("btnNewDocument.Image"), System.Drawing.Image)
		Me.btnNewDocument.Location = New System.Drawing.Point(614, 129)
		Me.btnNewDocument.Name = "btnNewDocument"
		Me.btnNewDocument.Size = New System.Drawing.Size(105, 28)
		Me.btnNewDocument.TabIndex = 7
		Me.btnNewDocument.Text = "Neu"
		'
		'btnSave
		'
		Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
		Me.btnSave.Location = New System.Drawing.Point(614, 92)
		Me.btnSave.Name = "btnSave"
		Me.btnSave.Size = New System.Drawing.Size(105, 28)
		Me.btnSave.TabIndex = 6
		Me.btnSave.Text = "Speichern"
		'
		'lueCategory
		'
		Me.lueCategory.Location = New System.Drawing.Point(141, 69)
		Me.lueCategory.Name = "lueCategory"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		Me.lueCategory.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, "", Nothing, Nothing, True)})
		Me.lueCategory.Properties.ShowFooter = False
		Me.lueCategory.Size = New System.Drawing.Size(182, 20)
		Me.lueCategory.TabIndex = 2
		'
		'lblkategorie
		'
		Me.lblkategorie.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblkategorie.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblkategorie.Location = New System.Drawing.Point(16, 73)
		Me.lblkategorie.Name = "lblkategorie"
		Me.lblkategorie.Size = New System.Drawing.Size(119, 13)
		Me.lblkategorie.TabIndex = 9
		Me.lblkategorie.Text = "Kategorie"
		'
		'lblbeschreibung
		'
		Me.lblbeschreibung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblbeschreibung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblbeschreibung.Location = New System.Drawing.Point(16, 121)
		Me.lblbeschreibung.Name = "lblbeschreibung"
		Me.lblbeschreibung.Size = New System.Drawing.Size(119, 13)
		Me.lblbeschreibung.TabIndex = 7
		Me.lblbeschreibung.Text = "Beschreibung"
		'
		'txtDescription
		'
		Me.txtDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtDescription.Location = New System.Drawing.Point(141, 121)
		Me.txtDescription.Name = "txtDescription"
		Me.txtDescription.Size = New System.Drawing.Size(449, 75)
		Me.txtDescription.TabIndex = 4
		'
		'lbldatei
		'
		Me.lbldatei.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.lbldatei.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lbldatei.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lbldatei.Location = New System.Drawing.Point(16, 206)
		Me.lbldatei.Name = "lbldatei"
		Me.lbldatei.Size = New System.Drawing.Size(119, 13)
		Me.lbldatei.TabIndex = 5
		Me.lbldatei.Text = "Datei"
		'
		'lblzhd
		'
		Me.lblzhd.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblzhd.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblzhd.Location = New System.Drawing.Point(16, 47)
		Me.lblzhd.Name = "lblzhd"
		Me.lblzhd.Size = New System.Drawing.Size(119, 13)
		Me.lblzhd.TabIndex = 3
		Me.lblzhd.Text = "Zuständige Person"
		'
		'txtTitle
		'
		Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtTitle.Location = New System.Drawing.Point(141, 95)
		Me.txtTitle.Name = "txtTitle"
		Me.txtTitle.Size = New System.Drawing.Size(449, 20)
		Me.txtTitle.TabIndex = 3
		'
		'lblbezeichnung
		'
		Me.lblbezeichnung.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.lblbezeichnung.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
		Me.lblbezeichnung.Location = New System.Drawing.Point(16, 99)
		Me.lblbezeichnung.Name = "lblbezeichnung"
		Me.lblbezeichnung.Size = New System.Drawing.Size(119, 13)
		Me.lblbezeichnung.TabIndex = 0
		Me.lblbezeichnung.Text = "Bezeichnung"
		'
		'txtFilePath
		'
		Me.txtFilePath.AllowDrop = True
		Me.txtFilePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtFilePath.Location = New System.Drawing.Point(141, 202)
		Me.txtFilePath.Name = "txtFilePath"
		Me.txtFilePath.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Global.SP.KD.DocumentMng.My.Resources.Resources.pdf, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject2, "", Nothing, Nothing, True)})
		Me.txtFilePath.Size = New System.Drawing.Size(449, 21)
		Me.txtFilePath.TabIndex = 5
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.FileName = "OpenFileDialog1"
		'
		'errorProviderDocumentMangement
		'
		Me.errorProviderDocumentMangement.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
		Me.errorProviderDocumentMangement.ContainerControl = Me
		'
		'lueZHDName
		'
		Me.lueZHDName.Location = New System.Drawing.Point(141, 43)
		Me.lueZHDName.Name = "lueZHDName"
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		Me.lueZHDName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, DevExpress.XtraEditors.ImageLocation.MiddleCenter, Nothing, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject3, "", Nothing, Nothing, True)})
		Me.lueZHDName.Properties.ShowFooter = False
		Me.lueZHDName.Properties.View = Me.gvZHDName
		Me.lueZHDName.Size = New System.Drawing.Size(182, 20)
		Me.lueZHDName.TabIndex = 1
		'
		'gvZHDName
		'
		Me.gvZHDName.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
		Me.gvZHDName.Name = "gvZHDName"
		Me.gvZHDName.OptionsSelection.EnableAppearanceFocusedCell = False
		Me.gvZHDName.OptionsView.ShowGroupPanel = False
		'
		'frmDoc
		'
		Me.AllowDrop = True
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(763, 610)
		Me.Controls.Add(Me.sccMain)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(695, 598)
		Me.Name = "frmDoc"
		Me.Padding = New System.Windows.Forms.Padding(5)
		Me.Text = "Dokumentverwaltung"
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.SplitContainerControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SplitContainerControl1.ResumeLayout(False)
		CType(Me.lstCategorie, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gridDocuments, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvDocuments, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.grpdetails, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpdetails.ResumeLayout(False)
		CType(Me.lueCategory.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtTitle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtFilePath.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.errorProviderDocumentMangement, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lueZHDName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gvZHDName, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
  Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents SplitContainerControl1 As DevExpress.XtraEditors.SplitContainerControl
  Friend WithEvents lstCategorie As DevExpress.XtraEditors.ListBoxControl
  Friend WithEvents gridDocuments As DevExpress.XtraGrid.GridControl
  Friend WithEvents gvDocuments As DevExpress.XtraGrid.Views.Grid.GridView
  Friend WithEvents grpdetails As DevExpress.XtraEditors.GroupControl
  Friend WithEvents lblbeschreibung As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txtDescription As DevExpress.XtraEditors.MemoEdit
  Friend WithEvents lbldatei As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblzhd As DevExpress.XtraEditors.LabelControl
  Friend WithEvents txtTitle As DevExpress.XtraEditors.TextEdit
  Friend WithEvents lblbezeichnung As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lueCategory As DevExpress.XtraEditors.LookUpEdit
  Friend WithEvents lblkategorie As DevExpress.XtraEditors.LabelControl
	Friend WithEvents lblDocumentChanged As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblDocumentCreated As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblgaendert As DevExpress.XtraEditors.LabelControl
  Friend WithEvents lblerstellt As DevExpress.XtraEditors.LabelControl
  Friend WithEvents btnNewDocument As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents errorProviderDocumentMangement As System.Windows.Forms.ErrorProvider
  Friend WithEvents btnDeleteDocument As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txtFilePath As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents lueZHDName As DevExpress.XtraEditors.GridLookUpEdit
	Friend WithEvents gvZHDName As DevExpress.XtraGrid.Views.Grid.GridView
End Class

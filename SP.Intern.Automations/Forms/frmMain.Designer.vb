<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
	Inherits DevExpress.XtraEditors.XtraForm

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()>
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
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
		Me.btnJobsCHXML = New DevExpress.XtraEditors.SimpleButton()
		Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar4 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiReccount = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.BarStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
		Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
		Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton()
		Me.SimpleButton5 = New DevExpress.XtraEditors.SimpleButton()
		Me.btnRefreshEmployeeGeoData = New DevExpress.XtraEditors.SimpleButton()
		Me.btnRefreshCustomerGeoData = New DevExpress.XtraEditors.SimpleButton()
		Me.btnRefreshEmployeeCountryCodeData = New DevExpress.XtraEditors.SimpleButton()
		Me.btnRefreshCustomerCountryCodeData = New DevExpress.XtraEditors.SimpleButton()
		Me.btnUpdateEmployeeTaxCoummunity = New DevExpress.XtraEditors.SimpleButton()
		Me.btnUpdateWOSMySetting = New DevExpress.XtraEditors.SimpleButton()
		Me.btnReadPDFFields = New DevExpress.XtraEditors.SimpleButton()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'SimpleButton1
		'
		Me.SimpleButton1.Location = New System.Drawing.Point(547, 28)
		Me.SimpleButton1.Name = "SimpleButton1"
		Me.SimpleButton1.Size = New System.Drawing.Size(209, 66)
		Me.SimpleButton1.TabIndex = 0
		Me.SimpleButton1.Text = "Payment mit JobID abgleichen"
		'
		'btnJobsCHXML
		'
		Me.btnJobsCHXML.Location = New System.Drawing.Point(801, 28)
		Me.btnJobsCHXML.Name = "btnJobsCHXML"
		Me.btnJobsCHXML.Size = New System.Drawing.Size(209, 66)
		Me.btnJobsCHXML.TabIndex = 1
		Me.btnJobsCHXML.Text = "Jobs.ch XML-Datei neu anlegen"
		'
		'SimpleButton3
		'
		Me.SimpleButton3.Location = New System.Drawing.Point(801, 105)
		Me.SimpleButton3.Name = "SimpleButton3"
		Me.SimpleButton3.Size = New System.Drawing.Size(209, 66)
		Me.SimpleButton3.TabIndex = 2
		Me.SimpleButton3.Text = "Ostjob.ch XML-Datei neu anlegen"
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar4})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo, Me.BarStaticItem1, Me.bsiReccount})
		Me.BarManager1.MaxItemId = 11
		Me.BarManager1.StatusBar = Me.Bar4
		'
		'Bar4
		'
		Me.Bar4.BarName = "Statusleiste"
		Me.Bar4.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar4.DockCol = 0
		Me.Bar4.DockRow = 0
		Me.Bar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiReccount)})
		Me.Bar4.OptionsBar.AllowQuickCustomization = False
		Me.Bar4.OptionsBar.DrawDragBorder = False
		Me.Bar4.OptionsBar.UseWholeRow = True
		Me.Bar4.Text = "Statusleiste"
		'
		'bsiInfo
		'
		Me.bsiInfo.Caption = "Bereit"
		Me.bsiInfo.Id = 0
		Me.bsiInfo.Name = "bsiInfo"
		'
		'bsiReccount
		'
		Me.bsiReccount.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
		Me.bsiReccount.Caption = "Reccount"
		Me.bsiReccount.Id = 10
		Me.bsiReccount.Name = "bsiReccount"
		Me.bsiReccount.Size = New System.Drawing.Size(32, 0)
		Me.bsiReccount.Width = 32
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(1027, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 375)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1027, 22)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 375)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1027, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 375)
		'
		'BarStaticItem1
		'
		Me.BarStaticItem1.Caption = " "
		Me.BarStaticItem1.Id = 5
		Me.BarStaticItem1.Name = "BarStaticItem1"
		'
		'SimpleButton2
		'
		Me.SimpleButton2.Location = New System.Drawing.Point(547, 105)
		Me.SimpleButton2.Name = "SimpleButton2"
		Me.SimpleButton2.Size = New System.Drawing.Size(209, 66)
		Me.SimpleButton2.TabIndex = 7
		Me.SimpleButton2.Text = "IBAN-Version abfragen"
		'
		'SimpleButton4
		'
		Me.SimpleButton4.Location = New System.Drawing.Point(801, 185)
		Me.SimpleButton4.Name = "SimpleButton4"
		Me.SimpleButton4.Size = New System.Drawing.Size(209, 66)
		Me.SimpleButton4.TabIndex = 8
		Me.SimpleButton4.Text = "eCall-Daten"
		'
		'SimpleButton5
		'
		Me.SimpleButton5.Location = New System.Drawing.Point(547, 185)
		Me.SimpleButton5.Name = "SimpleButton5"
		Me.SimpleButton5.Size = New System.Drawing.Size(209, 66)
		Me.SimpleButton5.TabIndex = 13
		Me.SimpleButton5.Text = "QST-Tabelle einlesen"
		'
		'btnRefreshEmployeeGeoData
		'
		Me.btnRefreshEmployeeGeoData.Location = New System.Drawing.Point(28, 108)
		Me.btnRefreshEmployeeGeoData.Name = "btnRefreshEmployeeGeoData"
		Me.btnRefreshEmployeeGeoData.Size = New System.Drawing.Size(209, 66)
		Me.btnRefreshEmployeeGeoData.TabIndex = 18
		Me.btnRefreshEmployeeGeoData.Text = "Kandidaten-Geodaten aktualisieren"
		'
		'btnRefreshCustomerGeoData
		'
		Me.btnRefreshCustomerGeoData.Location = New System.Drawing.Point(272, 108)
		Me.btnRefreshCustomerGeoData.Name = "btnRefreshCustomerGeoData"
		Me.btnRefreshCustomerGeoData.Size = New System.Drawing.Size(209, 66)
		Me.btnRefreshCustomerGeoData.TabIndex = 19
		Me.btnRefreshCustomerGeoData.Text = "Kunden-Geodaten aktualisieren"
		'
		'btnRefreshEmployeeCountryCodeData
		'
		Me.btnRefreshEmployeeCountryCodeData.Location = New System.Drawing.Point(28, 28)
		Me.btnRefreshEmployeeCountryCodeData.Name = "btnRefreshEmployeeCountryCodeData"
		Me.btnRefreshEmployeeCountryCodeData.Size = New System.Drawing.Size(209, 66)
		Me.btnRefreshEmployeeCountryCodeData.TabIndex = 24
		Me.btnRefreshEmployeeCountryCodeData.Text = "Kandidaten Ländertabelle aktualisieren"
		'
		'btnRefreshCustomerCountryCodeData
		'
		Me.btnRefreshCustomerCountryCodeData.Location = New System.Drawing.Point(272, 28)
		Me.btnRefreshCustomerCountryCodeData.Name = "btnRefreshCustomerCountryCodeData"
		Me.btnRefreshCustomerCountryCodeData.Size = New System.Drawing.Size(209, 66)
		Me.btnRefreshCustomerCountryCodeData.TabIndex = 29
		Me.btnRefreshCustomerCountryCodeData.Text = "Kunden Ländertabelle aktualisieren"
		'
		'btnUpdateEmployeeTaxCoummunity
		'
		Me.btnUpdateEmployeeTaxCoummunity.Location = New System.Drawing.Point(28, 206)
		Me.btnUpdateEmployeeTaxCoummunity.Name = "btnUpdateEmployeeTaxCoummunity"
		Me.btnUpdateEmployeeTaxCoummunity.Size = New System.Drawing.Size(209, 66)
		Me.btnUpdateEmployeeTaxCoummunity.TabIndex = 34
		Me.btnUpdateEmployeeTaxCoummunity.Text = "Kandidaten QST-Gemeinde updaten"
		'
		'btnUpdateWOSMySetting
		'
		Me.btnUpdateWOSMySetting.Location = New System.Drawing.Point(547, 267)
		Me.btnUpdateWOSMySetting.Name = "btnUpdateWOSMySetting"
		Me.btnUpdateWOSMySetting.Size = New System.Drawing.Size(209, 66)
		Me.btnUpdateWOSMySetting.TabIndex = 39
		Me.btnUpdateWOSMySetting.Text = "WOS-MySetting Updaten"
		'
		'btnReadPDFFields
		'
		Me.btnReadPDFFields.Location = New System.Drawing.Point(801, 267)
		Me.btnReadPDFFields.Name = "btnReadPDFFields"
		Me.btnReadPDFFields.Size = New System.Drawing.Size(209, 66)
		Me.btnReadPDFFields.TabIndex = 44
		Me.btnReadPDFFields.Text = "Read PDF-Fields"
		'
		'frmMain
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1027, 397)
		Me.Controls.Add(Me.btnReadPDFFields)
		Me.Controls.Add(Me.btnUpdateWOSMySetting)
		Me.Controls.Add(Me.btnUpdateEmployeeTaxCoummunity)
		Me.Controls.Add(Me.btnRefreshCustomerCountryCodeData)
		Me.Controls.Add(Me.btnRefreshEmployeeCountryCodeData)
		Me.Controls.Add(Me.btnRefreshCustomerGeoData)
		Me.Controls.Add(Me.btnRefreshEmployeeGeoData)
		Me.Controls.Add(Me.SimpleButton5)
		Me.Controls.Add(Me.SimpleButton4)
		Me.Controls.Add(Me.SimpleButton2)
		Me.Controls.Add(Me.SimpleButton3)
		Me.Controls.Add(Me.btnJobsCHXML)
		Me.Controls.Add(Me.SimpleButton1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.Name = "frmMain"
		Me.Text = "Various system settings"
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnJobsCHXML As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents Bar4 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiReccount As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents BarStaticItem1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents SimpleButton5 As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnRefreshCustomerGeoData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnRefreshEmployeeGeoData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnRefreshEmployeeCountryCodeData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnRefreshCustomerCountryCodeData As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnUpdateEmployeeTaxCoummunity As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnUpdateWOSMySetting As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents btnReadPDFFields As DevExpress.XtraEditors.SimpleButton
End Class

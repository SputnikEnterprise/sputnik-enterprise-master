' Developer Express Code Central Example:
' How to refresh the Field List in the End-User Designer
' 
' This example demonstrates how to programmatically update the Field List in the
' End-User Designer. This may be required when a data source is created and bound
' to a report at runtime, and isn't present in the designer host of the End-User
' Designer. For instance, this situation occurs when a data source is represented
' by a list (e.g. ArrayList), as demonstrated in the sample below.
' To accomplish
' this task, we've introduced the FieldListDockPanel.UpdateDataSource method,
' which should be called after assigning a data source to a report.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E1584

Imports Microsoft.VisualBasic
Imports System
Namespace FieldList_UpdateDataSource
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(Form1))
			Me.xrDesignBarManager1 = New DevExpress.XtraReports.UserDesigner.XRDesignBarManager(Me.components)
			Me.designBar1 = New DevExpress.XtraReports.UserDesigner.DesignBar()
			Me.barSubItem1 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem31 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem39 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem32 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem33 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem40 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem41 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem2 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem37 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem38 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem34 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem35 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem36 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem42 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem43 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem3 = New DevExpress.XtraBars.BarSubItem()
			Me.barReportTabButtonsListItem1 = New DevExpress.XtraReports.UserDesigner.BarReportTabButtonsListItem()
			Me.barSubItem4 = New DevExpress.XtraBars.BarSubItem()
			Me.xrBarToolbarsListItem1 = New DevExpress.XtraReports.UserDesigner.XRBarToolbarsListItem()
			Me.barSubItem5 = New DevExpress.XtraBars.BarSubItem()
			Me.barDockPanelsListItem1 = New DevExpress.XtraReports.UserDesigner.BarDockPanelsListItem()
			Me.barSubItem6 = New DevExpress.XtraBars.BarSubItem()
			Me.commandColorBarItem1 = New DevExpress.XtraReports.UserDesigner.CommandColorBarItem()
			Me.commandColorBarItem2 = New DevExpress.XtraReports.UserDesigner.CommandColorBarItem()
			Me.barSubItem7 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem1 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem2 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem3 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem8 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem4 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem5 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem6 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem7 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem9 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem9 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem10 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem11 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem12 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem13 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem14 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem8 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem10 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem15 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem16 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem17 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem18 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem11 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem19 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem20 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem21 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem22 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem12 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem23 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem24 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem25 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem26 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem13 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem27 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem28 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem14 = New DevExpress.XtraBars.BarSubItem()
			Me.commandBarItem29 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.commandBarItem30 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barSubItem15 = New DevExpress.XtraBars.BarSubItem()
			Me.designBar2 = New DevExpress.XtraReports.UserDesigner.DesignBar()
			Me.designBar3 = New DevExpress.XtraReports.UserDesigner.DesignBar()
			Me.barEditItem1 = New DevExpress.XtraBars.BarEditItem()
			Me.recentlyUsedItemsComboBox1 = New DevExpress.XtraReports.UserDesigner.RecentlyUsedItemsComboBox()
			Me.barEditItem2 = New DevExpress.XtraBars.BarEditItem()
			Me.designRepositoryItemComboBox1 = New DevExpress.XtraReports.UserDesigner.DesignRepositoryItemComboBox()
			Me.designBar4 = New DevExpress.XtraReports.UserDesigner.DesignBar()
			Me.designBar5 = New DevExpress.XtraReports.UserDesigner.DesignBar()
			Me.barStaticItem1 = New DevExpress.XtraBars.BarStaticItem()
			Me.designBar6 = New DevExpress.XtraReports.UserDesigner.DesignBar()
			Me.commandBarItem44 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.xrZoomBarEditItem1 = New DevExpress.XtraReports.UserDesigner.XRZoomBarEditItem()
			Me.designRepositoryItemComboBox2 = New DevExpress.XtraReports.UserDesigner.DesignRepositoryItemComboBox()
			Me.commandBarItem45 = New DevExpress.XtraReports.UserDesigner.CommandBarItem()
			Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
			Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
			Me.xrDesignDockManager1 = New DevExpress.XtraReports.UserDesigner.XRDesignDockManager(Me.components)
			Me.panelContainer1 = New DevExpress.XtraBars.Docking.DockPanel()
			Me.panelContainer2 = New DevExpress.XtraBars.Docking.DockPanel()
			Me.fieldListDockPanel1 = New DevExpress.XtraReports.UserDesigner.FieldListDockPanel()
			Me.fieldListDockPanel1_Container = New DevExpress.XtraReports.UserDesigner.DesignControlContainer()
			Me.xrDesignPanel1 = New DevExpress.XtraReports.UserDesigner.XRDesignPanel()
			Me.reportExplorerDockPanel1 = New DevExpress.XtraReports.UserDesigner.ReportExplorerDockPanel()
			Me.reportExplorerDockPanel1_Container = New DevExpress.XtraReports.UserDesigner.DesignControlContainer()
			Me.propertyGridDockPanel1 = New DevExpress.XtraReports.UserDesigner.PropertyGridDockPanel()
			Me.propertyGridDockPanel1_Container = New DevExpress.XtraReports.UserDesigner.DesignControlContainer()
			Me.toolBoxDockPanel1 = New DevExpress.XtraReports.UserDesigner.ToolBoxDockPanel()
			Me.toolBoxDockPanel1_Container = New DevExpress.XtraReports.UserDesigner.DesignControlContainer()
			Me.groupAndSortDockPanel1 = New DevExpress.XtraReports.UserDesigner.GroupAndSortDockPanel()
			Me.groupAndSortDockPanel1_Container = New DevExpress.XtraReports.UserDesigner.DesignControlContainer()
			Me.barButtonItem1 = New DevExpress.XtraBars.BarButtonItem()
			CType(Me.xrDesignBarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.recentlyUsedItemsComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.designRepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.designRepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.xrDesignDockManager1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.panelContainer1.SuspendLayout()
			Me.panelContainer2.SuspendLayout()
			Me.fieldListDockPanel1.SuspendLayout()
			CType(Me.xrDesignPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.reportExplorerDockPanel1.SuspendLayout()
			Me.propertyGridDockPanel1.SuspendLayout()
			Me.toolBoxDockPanel1.SuspendLayout()
			Me.groupAndSortDockPanel1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' xrDesignBarManager1
			' 
			Me.xrDesignBarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() { Me.designBar1, Me.designBar2, Me.designBar3, Me.designBar4, Me.designBar5, Me.designBar6})
			Me.xrDesignBarManager1.DockControls.Add(Me.barDockControlTop)
			Me.xrDesignBarManager1.DockControls.Add(Me.barDockControlBottom)
			Me.xrDesignBarManager1.DockControls.Add(Me.barDockControlLeft)
			Me.xrDesignBarManager1.DockControls.Add(Me.barDockControlRight)
			Me.xrDesignBarManager1.DockManager = Me.xrDesignDockManager1
			Me.xrDesignBarManager1.FontNameBox = Me.recentlyUsedItemsComboBox1
			Me.xrDesignBarManager1.FontNameEdit = Me.barEditItem1
			Me.xrDesignBarManager1.FontSizeBox = Me.designRepositoryItemComboBox1
			Me.xrDesignBarManager1.FontSizeEdit = Me.barEditItem2
			Me.xrDesignBarManager1.Form = Me
			Me.xrDesignBarManager1.FormattingToolbar = Me.designBar3
			Me.xrDesignBarManager1.HintStaticItem = Me.barStaticItem1
			Me.xrDesignBarManager1.ImageStream = (CType(resources.GetObject("xrDesignBarManager1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer))
			Me.xrDesignBarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() { Me.barEditItem1, Me.barEditItem2, Me.commandBarItem1, Me.commandBarItem2, Me.commandBarItem3, Me.commandColorBarItem1, Me.commandColorBarItem2, Me.commandBarItem4, Me.commandBarItem5, Me.commandBarItem6, Me.commandBarItem7, Me.commandBarItem8, Me.commandBarItem9, Me.commandBarItem10, Me.commandBarItem11, Me.commandBarItem12, Me.commandBarItem13, Me.commandBarItem14, Me.commandBarItem15, Me.commandBarItem16, Me.commandBarItem17, Me.commandBarItem18, Me.commandBarItem19, Me.commandBarItem20, Me.commandBarItem21, Me.commandBarItem22, Me.commandBarItem23, Me.commandBarItem24, Me.commandBarItem25, Me.commandBarItem26, Me.commandBarItem27, Me.commandBarItem28, Me.commandBarItem29, Me.commandBarItem30, Me.commandBarItem31, Me.commandBarItem32, Me.commandBarItem33, Me.commandBarItem34, Me.commandBarItem35, Me.commandBarItem36, Me.commandBarItem37, Me.commandBarItem38, Me.barStaticItem1, Me.barSubItem1, Me.barSubItem2, Me.barSubItem3, Me.barReportTabButtonsListItem1, Me.barSubItem4, Me.xrBarToolbarsListItem1, Me.barSubItem5, Me.barDockPanelsListItem1, Me.barSubItem6, Me.barSubItem7, Me.barSubItem8, Me.barSubItem9, Me.barSubItem10, Me.barSubItem11, Me.barSubItem12, Me.barSubItem13, Me.barSubItem14, Me.commandBarItem39, Me.commandBarItem40, Me.commandBarItem41, Me.commandBarItem42, Me.commandBarItem43, Me.commandBarItem44, Me.xrZoomBarEditItem1, Me.commandBarItem45, Me.barSubItem15, Me.barButtonItem1})
			Me.xrDesignBarManager1.LayoutToolbar = Me.designBar4
			Me.xrDesignBarManager1.MainMenu = Me.designBar1
			Me.xrDesignBarManager1.MaxItemId = 72
			Me.xrDesignBarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() { Me.recentlyUsedItemsComboBox1, Me.designRepositoryItemComboBox1, Me.designRepositoryItemComboBox2})
			Me.xrDesignBarManager1.StatusBar = Me.designBar5
			Me.xrDesignBarManager1.Toolbar = Me.designBar2
			Me.xrDesignBarManager1.XRDesignPanel = Me.xrDesignPanel1
			Me.xrDesignBarManager1.ZoomItem = Me.xrZoomBarEditItem1
			' 
			' designBar1
			' 
			Me.designBar1.BarName = "Main Menu"
			Me.designBar1.DockCol = 0
			Me.designBar1.DockRow = 0
			Me.designBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
			Me.designBar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem15)})
			Me.designBar1.OptionsBar.MultiLine = True
			Me.designBar1.OptionsBar.UseWholeRow = True
			Me.designBar1.Text = "Main Menu"
			' 
			' barSubItem1
			' 
			Me.barSubItem1.Caption = "&File"
			Me.barSubItem1.Id = 43
			Me.barSubItem1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem31), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem39), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem32), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem33, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem40), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem41, True)})
			Me.barSubItem1.Name = "barSubItem1"
			' 
			' commandBarItem31
			' 
			Me.commandBarItem31.Caption = "&New"
			Me.commandBarItem31.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport
			Me.commandBarItem31.Hint = "Create a new blank report"
			Me.commandBarItem31.Id = 34
			Me.commandBarItem31.ImageIndex = 9
			Me.commandBarItem31.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N))
			Me.commandBarItem31.Name = "commandBarItem31"
			' 
			' commandBarItem39
			' 
			Me.commandBarItem39.Caption = "New with &Wizard..."
			Me.commandBarItem39.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.NewReportWizard
			Me.commandBarItem39.Hint = "Create a new report using the Wizard"
			Me.commandBarItem39.Id = 60
			Me.commandBarItem39.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W))
			Me.commandBarItem39.Name = "commandBarItem39"
			' 
			' commandBarItem32
			' 
			Me.commandBarItem32.Caption = "&Open..."
			Me.commandBarItem32.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.OpenFile
			Me.commandBarItem32.Hint = "Open a report"
			Me.commandBarItem32.Id = 35
			Me.commandBarItem32.ImageIndex = 10
			Me.commandBarItem32.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O))
			Me.commandBarItem32.Name = "commandBarItem32"
			' 
			' commandBarItem33
			' 
			Me.commandBarItem33.Caption = "&Save"
			Me.commandBarItem33.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFile
			Me.commandBarItem33.Enabled = False
			Me.commandBarItem33.Hint = "Save a report"
			Me.commandBarItem33.Id = 36
			Me.commandBarItem33.ImageIndex = 11
			Me.commandBarItem33.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S))
			Me.commandBarItem33.Name = "commandBarItem33"
			' 
			' commandBarItem40
			' 
			Me.commandBarItem40.Caption = "Save &As..."
			Me.commandBarItem40.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFileAs
			Me.commandBarItem40.Enabled = False
			Me.commandBarItem40.Hint = "Save a report with a new name"
			Me.commandBarItem40.Id = 61
			Me.commandBarItem40.Name = "commandBarItem40"
			' 
			' commandBarItem41
			' 
			Me.commandBarItem41.Caption = "E&xit"
			Me.commandBarItem41.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Exit
			Me.commandBarItem41.Hint = "Close the designer"
			Me.commandBarItem41.Id = 62
			Me.commandBarItem41.Name = "commandBarItem41"
			' 
			' barSubItem2
			' 
			Me.barSubItem2.Caption = "&Edit"
			Me.barSubItem2.Id = 44
			Me.barSubItem2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem37, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem38), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem34, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem35), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem36), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem42), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem43, True)})
			Me.barSubItem2.Name = "barSubItem2"
			' 
			' commandBarItem37
			' 
			Me.commandBarItem37.Caption = "&Undo"
			Me.commandBarItem37.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Undo
			Me.commandBarItem37.Enabled = False
			Me.commandBarItem37.Hint = "Undo the last operation"
			Me.commandBarItem37.Id = 40
			Me.commandBarItem37.ImageIndex = 15
			Me.commandBarItem37.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z))
			Me.commandBarItem37.Name = "commandBarItem37"
			' 
			' commandBarItem38
			' 
			Me.commandBarItem38.Caption = "&Redo"
			Me.commandBarItem38.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Redo
			Me.commandBarItem38.Enabled = False
			Me.commandBarItem38.Hint = "Redo the last operation"
			Me.commandBarItem38.Id = 41
			Me.commandBarItem38.ImageIndex = 16
			Me.commandBarItem38.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y))
			Me.commandBarItem38.Name = "commandBarItem38"
			' 
			' commandBarItem34
			' 
			Me.commandBarItem34.Caption = "Cu&t"
			Me.commandBarItem34.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Cut
			Me.commandBarItem34.Enabled = False
			Me.commandBarItem34.Hint = "Delete the control and copy it to the clipboard"
			Me.commandBarItem34.Id = 37
			Me.commandBarItem34.ImageIndex = 12
			Me.commandBarItem34.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X))
			Me.commandBarItem34.Name = "commandBarItem34"
			' 
			' commandBarItem35
			' 
			Me.commandBarItem35.Caption = "&Copy"
			Me.commandBarItem35.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Copy
			Me.commandBarItem35.Enabled = False
			Me.commandBarItem35.Hint = "Copy the control to the clipboard"
			Me.commandBarItem35.Id = 38
			Me.commandBarItem35.ImageIndex = 13
			Me.commandBarItem35.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C))
			Me.commandBarItem35.Name = "commandBarItem35"
			' 
			' commandBarItem36
			' 
			Me.commandBarItem36.Caption = "&Paste"
			Me.commandBarItem36.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Paste
			Me.commandBarItem36.Enabled = False
			Me.commandBarItem36.Hint = "Add the control from the clipboard"
			Me.commandBarItem36.Id = 39
			Me.commandBarItem36.ImageIndex = 14
			Me.commandBarItem36.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V))
			Me.commandBarItem36.Name = "commandBarItem36"
			' 
			' commandBarItem42
			' 
			Me.commandBarItem42.Caption = "&Delete"
			Me.commandBarItem42.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.Delete
			Me.commandBarItem42.Enabled = False
			Me.commandBarItem42.Hint = "Delete the control"
			Me.commandBarItem42.Id = 63
			Me.commandBarItem42.Name = "commandBarItem42"
			' 
			' commandBarItem43
			' 
			Me.commandBarItem43.Caption = "Select &All"
			Me.commandBarItem43.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SelectAll
			Me.commandBarItem43.Enabled = False
			Me.commandBarItem43.Hint = "Select all the controls in the document"
			Me.commandBarItem43.Id = 64
			Me.commandBarItem43.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A))
			Me.commandBarItem43.Name = "commandBarItem43"
			' 
			' barSubItem3
			' 
			Me.barSubItem3.Caption = "&View"
			Me.barSubItem3.Id = 45
			Me.barSubItem3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.barReportTabButtonsListItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem4, True), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem5, True)})
			Me.barSubItem3.Name = "barSubItem3"
			' 
			' barReportTabButtonsListItem1
			' 
			Me.barReportTabButtonsListItem1.Caption = "Tab Buttons"
			Me.barReportTabButtonsListItem1.Id = 46
			Me.barReportTabButtonsListItem1.Name = "barReportTabButtonsListItem1"
			' 
			' barSubItem4
			' 
			Me.barSubItem4.Caption = "&Toolbars"
			Me.barSubItem4.Id = 47
			Me.barSubItem4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.xrBarToolbarsListItem1)})
			Me.barSubItem4.Name = "barSubItem4"
			' 
			' xrBarToolbarsListItem1
			' 
			Me.xrBarToolbarsListItem1.Caption = "&Toolbars"
			Me.xrBarToolbarsListItem1.Id = 48
			Me.xrBarToolbarsListItem1.Name = "xrBarToolbarsListItem1"
			' 
			' barSubItem5
			' 
			Me.barSubItem5.Caption = "&Windows"
			Me.barSubItem5.Id = 49
			Me.barSubItem5.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.barDockPanelsListItem1)})
			Me.barSubItem5.Name = "barSubItem5"
			' 
			' barDockPanelsListItem1
			' 
			Me.barDockPanelsListItem1.Caption = "&Windows"
			Me.barDockPanelsListItem1.Id = 50
			Me.barDockPanelsListItem1.Name = "barDockPanelsListItem1"
			Me.barDockPanelsListItem1.ShowCustomizationItem = False
			Me.barDockPanelsListItem1.ShowDockPanels = True
			Me.barDockPanelsListItem1.ShowToolbars = False
			' 
			' barSubItem6
			' 
			Me.barSubItem6.Caption = "Fo&rmat"
			Me.barSubItem6.Id = 51
			Me.barSubItem6.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandColorBarItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.commandColorBarItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem7, True), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem8), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem9, True), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem10), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem11, True), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem12), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem13, True), New DevExpress.XtraBars.LinkPersistInfo(Me.barSubItem14, True)})
			Me.barSubItem6.Name = "barSubItem6"
			' 
			' commandColorBarItem1
			' 
			Me.commandColorBarItem1.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.commandColorBarItem1.Caption = "For&eground Color"
			Me.commandColorBarItem1.CloseSubMenuOnClick = False
			Me.commandColorBarItem1.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.ForeColor
			Me.commandColorBarItem1.Enabled = False
			Me.commandColorBarItem1.Glyph = (CType(resources.GetObject("commandColorBarItem1.Glyph"), System.Drawing.Image))
			Me.commandColorBarItem1.Hint = "Set the foreground color of the control"
			Me.commandColorBarItem1.Id = 5
			Me.commandColorBarItem1.Name = "commandColorBarItem1"
			' 
			' commandColorBarItem2
			' 
			Me.commandColorBarItem2.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.commandColorBarItem2.Caption = "Bac&kground Color"
			Me.commandColorBarItem2.CloseSubMenuOnClick = False
			Me.commandColorBarItem2.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.BackColor
			Me.commandColorBarItem2.Enabled = False
			Me.commandColorBarItem2.Glyph = (CType(resources.GetObject("commandColorBarItem2.Glyph"), System.Drawing.Image))
			Me.commandColorBarItem2.Hint = "Set the background color of the control"
			Me.commandColorBarItem2.Id = 6
			Me.commandColorBarItem2.Name = "commandColorBarItem2"
			' 
			' barSubItem7
			' 
			Me.barSubItem7.Caption = "&Font"
			Me.barSubItem7.Id = 52
			Me.barSubItem7.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem1, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem3)})
			Me.barSubItem7.Name = "barSubItem7"
			' 
			' commandBarItem1
			' 
			Me.commandBarItem1.Caption = "&Bold"
			Me.commandBarItem1.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.FontBold
			Me.commandBarItem1.Enabled = False
			Me.commandBarItem1.Hint = "Make the font bold"
			Me.commandBarItem1.Id = 2
			Me.commandBarItem1.ImageIndex = 0
			Me.commandBarItem1.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B))
			Me.commandBarItem1.Name = "commandBarItem1"
			' 
			' commandBarItem2
			' 
			Me.commandBarItem2.Caption = "&Italic"
			Me.commandBarItem2.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.FontItalic
			Me.commandBarItem2.Enabled = False
			Me.commandBarItem2.Hint = "Make the font italic"
			Me.commandBarItem2.Id = 3
			Me.commandBarItem2.ImageIndex = 1
			Me.commandBarItem2.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I))
			Me.commandBarItem2.Name = "commandBarItem2"
			' 
			' commandBarItem3
			' 
			Me.commandBarItem3.Caption = "&Underline"
			Me.commandBarItem3.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.FontUnderline
			Me.commandBarItem3.Enabled = False
			Me.commandBarItem3.Hint = "Underline the font"
			Me.commandBarItem3.Id = 4
			Me.commandBarItem3.ImageIndex = 2
			Me.commandBarItem3.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.U))
			Me.commandBarItem3.Name = "commandBarItem3"
			' 
			' barSubItem8
			' 
			Me.barSubItem8.Caption = "&Justify"
			Me.barSubItem8.Id = 53
			Me.barSubItem8.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem4, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem7)})
			Me.barSubItem8.Name = "barSubItem8"
			' 
			' commandBarItem4
			' 
			Me.commandBarItem4.Caption = "&Left"
			Me.commandBarItem4.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyLeft
			Me.commandBarItem4.Enabled = False
			Me.commandBarItem4.Hint = "Align the control's text to the left"
			Me.commandBarItem4.Id = 7
			Me.commandBarItem4.ImageIndex = 5
			Me.commandBarItem4.Name = "commandBarItem4"
			' 
			' commandBarItem5
			' 
			Me.commandBarItem5.Caption = "&Center"
			Me.commandBarItem5.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyCenter
			Me.commandBarItem5.Enabled = False
			Me.commandBarItem5.Hint = "Align the control's text to the center"
			Me.commandBarItem5.Id = 8
			Me.commandBarItem5.ImageIndex = 6
			Me.commandBarItem5.Name = "commandBarItem5"
			' 
			' commandBarItem6
			' 
			Me.commandBarItem6.Caption = "&Rights"
			Me.commandBarItem6.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyRight
			Me.commandBarItem6.Enabled = False
			Me.commandBarItem6.Hint = "Align the control's text to the right"
			Me.commandBarItem6.Id = 9
			Me.commandBarItem6.ImageIndex = 7
			Me.commandBarItem6.Name = "commandBarItem6"
			' 
			' commandBarItem7
			' 
			Me.commandBarItem7.Caption = "&Justify"
			Me.commandBarItem7.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.JustifyJustify
			Me.commandBarItem7.Enabled = False
			Me.commandBarItem7.Hint = "Justify the control's text"
			Me.commandBarItem7.Id = 10
			Me.commandBarItem7.ImageIndex = 8
			Me.commandBarItem7.Name = "commandBarItem7"
			' 
			' barSubItem9
			' 
			Me.barSubItem9.Caption = "&Align"
			Me.barSubItem9.Id = 54
			Me.barSubItem9.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem9, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem10), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem11), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem12, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem13), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem14), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem8, True)})
			Me.barSubItem9.Name = "barSubItem9"
			' 
			' commandBarItem9
			' 
			Me.commandBarItem9.Caption = "&Lefts"
			Me.commandBarItem9.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignLeft
			Me.commandBarItem9.Enabled = False
			Me.commandBarItem9.Hint = "Left align the selected controls"
			Me.commandBarItem9.Id = 12
			Me.commandBarItem9.ImageIndex = 18
			Me.commandBarItem9.Name = "commandBarItem9"
			' 
			' commandBarItem10
			' 
			Me.commandBarItem10.Caption = "&Centers"
			Me.commandBarItem10.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignVerticalCenters
			Me.commandBarItem10.Enabled = False
			Me.commandBarItem10.Hint = "Align the centers of the selected controls vertically"
			Me.commandBarItem10.Id = 13
			Me.commandBarItem10.ImageIndex = 19
			Me.commandBarItem10.Name = "commandBarItem10"
			' 
			' commandBarItem11
			' 
			Me.commandBarItem11.Caption = "&Rights"
			Me.commandBarItem11.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignRight
			Me.commandBarItem11.Enabled = False
			Me.commandBarItem11.Hint = "Right align the selected controls"
			Me.commandBarItem11.Id = 14
			Me.commandBarItem11.ImageIndex = 20
			Me.commandBarItem11.Name = "commandBarItem11"
			' 
			' commandBarItem12
			' 
			Me.commandBarItem12.Caption = "&Tops"
			Me.commandBarItem12.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignTop
			Me.commandBarItem12.Enabled = False
			Me.commandBarItem12.Hint = "Align the tops of the selected controls"
			Me.commandBarItem12.Id = 15
			Me.commandBarItem12.ImageIndex = 21
			Me.commandBarItem12.Name = "commandBarItem12"
			' 
			' commandBarItem13
			' 
			Me.commandBarItem13.Caption = "&Middles"
			Me.commandBarItem13.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignHorizontalCenters
			Me.commandBarItem13.Enabled = False
			Me.commandBarItem13.Hint = "Align the centers of the selected controls horizontally"
			Me.commandBarItem13.Id = 16
			Me.commandBarItem13.ImageIndex = 22
			Me.commandBarItem13.Name = "commandBarItem13"
			' 
			' commandBarItem14
			' 
			Me.commandBarItem14.Caption = "&Bottoms"
			Me.commandBarItem14.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignBottom
			Me.commandBarItem14.Enabled = False
			Me.commandBarItem14.Hint = "Align the bottoms of the selected controls"
			Me.commandBarItem14.Id = 17
			Me.commandBarItem14.ImageIndex = 23
			Me.commandBarItem14.Name = "commandBarItem14"
			' 
			' commandBarItem8
			' 
			Me.commandBarItem8.Caption = "to &Grid"
			Me.commandBarItem8.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.AlignToGrid
			Me.commandBarItem8.Enabled = False
			Me.commandBarItem8.Hint = "Align the positions of the selected controls to the grid"
			Me.commandBarItem8.Id = 11
			Me.commandBarItem8.ImageIndex = 17
			Me.commandBarItem8.Name = "commandBarItem8"
			' 
			' barSubItem10
			' 
			Me.barSubItem10.Caption = "&Make Same Size"
			Me.barSubItem10.Id = 55
			Me.barSubItem10.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem15, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem16), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem17), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem18)})
			Me.barSubItem10.Name = "barSubItem10"
			' 
			' commandBarItem15
			' 
			Me.commandBarItem15.Caption = "&Width"
			Me.commandBarItem15.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToControlWidth
			Me.commandBarItem15.Enabled = False
			Me.commandBarItem15.Hint = "Make the selected controls have the same width"
			Me.commandBarItem15.Id = 18
			Me.commandBarItem15.ImageIndex = 24
			Me.commandBarItem15.Name = "commandBarItem15"
			' 
			' commandBarItem16
			' 
			Me.commandBarItem16.Caption = "Size to Gri&d"
			Me.commandBarItem16.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToGrid
			Me.commandBarItem16.Enabled = False
			Me.commandBarItem16.Hint = "Size the selected controls to the grid"
			Me.commandBarItem16.Id = 19
			Me.commandBarItem16.ImageIndex = 25
			Me.commandBarItem16.Name = "commandBarItem16"
			' 
			' commandBarItem17
			' 
			Me.commandBarItem17.Caption = "&Height"
			Me.commandBarItem17.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToControlHeight
			Me.commandBarItem17.Enabled = False
			Me.commandBarItem17.Hint = "Make the selected controls have the same height"
			Me.commandBarItem17.Id = 20
			Me.commandBarItem17.ImageIndex = 26
			Me.commandBarItem17.Name = "commandBarItem17"
			' 
			' commandBarItem18
			' 
			Me.commandBarItem18.Caption = "&Both"
			Me.commandBarItem18.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SizeToControl
			Me.commandBarItem18.Enabled = False
			Me.commandBarItem18.Hint = "Make the selected controls the same size"
			Me.commandBarItem18.Id = 21
			Me.commandBarItem18.ImageIndex = 27
			Me.commandBarItem18.Name = "commandBarItem18"
			' 
			' barSubItem11
			' 
			Me.barSubItem11.Caption = "&Horizontal Spacing"
			Me.barSubItem11.Id = 56
			Me.barSubItem11.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem19, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem20), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem21), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem22)})
			Me.barSubItem11.Name = "barSubItem11"
			' 
			' commandBarItem19
			' 
			Me.commandBarItem19.Caption = "Make &Equal"
			Me.commandBarItem19.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceMakeEqual
			Me.commandBarItem19.Enabled = False
			Me.commandBarItem19.Hint = "Make the spacing between the selected controls equal"
			Me.commandBarItem19.Id = 22
			Me.commandBarItem19.ImageIndex = 28
			Me.commandBarItem19.Name = "commandBarItem19"
			' 
			' commandBarItem20
			' 
			Me.commandBarItem20.Caption = "&Increase"
			Me.commandBarItem20.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceIncrease
			Me.commandBarItem20.Enabled = False
			Me.commandBarItem20.Hint = "Increase the spacing between the selected controls"
			Me.commandBarItem20.Id = 23
			Me.commandBarItem20.ImageIndex = 29
			Me.commandBarItem20.Name = "commandBarItem20"
			' 
			' commandBarItem21
			' 
			Me.commandBarItem21.Caption = "&Decrease"
			Me.commandBarItem21.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceDecrease
			Me.commandBarItem21.Enabled = False
			Me.commandBarItem21.Hint = "Decrease the spacing between the selected controls"
			Me.commandBarItem21.Id = 24
			Me.commandBarItem21.ImageIndex = 30
			Me.commandBarItem21.Name = "commandBarItem21"
			' 
			' commandBarItem22
			' 
			Me.commandBarItem22.Caption = "&Remove"
			Me.commandBarItem22.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.HorizSpaceConcatenate
			Me.commandBarItem22.Enabled = False
			Me.commandBarItem22.Hint = "Remove the spacing between the selected controls"
			Me.commandBarItem22.Id = 25
			Me.commandBarItem22.ImageIndex = 31
			Me.commandBarItem22.Name = "commandBarItem22"
			' 
			' barSubItem12
			' 
			Me.barSubItem12.Caption = "&Vertical Spacing"
			Me.barSubItem12.Id = 57
			Me.barSubItem12.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem23, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem24), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem25), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem26)})
			Me.barSubItem12.Name = "barSubItem12"
			' 
			' commandBarItem23
			' 
			Me.commandBarItem23.Caption = "Make &Equal"
			Me.commandBarItem23.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceMakeEqual
			Me.commandBarItem23.Enabled = False
			Me.commandBarItem23.Hint = "Make the spacing between the selected controls equal"
			Me.commandBarItem23.Id = 26
			Me.commandBarItem23.ImageIndex = 32
			Me.commandBarItem23.Name = "commandBarItem23"
			' 
			' commandBarItem24
			' 
			Me.commandBarItem24.Caption = "&Increase"
			Me.commandBarItem24.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceIncrease
			Me.commandBarItem24.Enabled = False
			Me.commandBarItem24.Hint = "Increase the spacing between the selected controls"
			Me.commandBarItem24.Id = 27
			Me.commandBarItem24.ImageIndex = 33
			Me.commandBarItem24.Name = "commandBarItem24"
			' 
			' commandBarItem25
			' 
			Me.commandBarItem25.Caption = "&Decrease"
			Me.commandBarItem25.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceDecrease
			Me.commandBarItem25.Enabled = False
			Me.commandBarItem25.Hint = "Decrease the spacing between the selected controls"
			Me.commandBarItem25.Id = 28
			Me.commandBarItem25.ImageIndex = 34
			Me.commandBarItem25.Name = "commandBarItem25"
			' 
			' commandBarItem26
			' 
			Me.commandBarItem26.Caption = "&Remove"
			Me.commandBarItem26.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.VertSpaceConcatenate
			Me.commandBarItem26.Enabled = False
			Me.commandBarItem26.Hint = "Remove the spacing between the selected controls"
			Me.commandBarItem26.Id = 29
			Me.commandBarItem26.ImageIndex = 35
			Me.commandBarItem26.Name = "commandBarItem26"
			' 
			' barSubItem13
			' 
			Me.barSubItem13.Caption = "&Center in Form"
			Me.barSubItem13.Id = 58
			Me.barSubItem13.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem27, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem28)})
			Me.barSubItem13.Name = "barSubItem13"
			' 
			' commandBarItem27
			' 
			Me.commandBarItem27.Caption = "&Horizontally"
			Me.commandBarItem27.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.CenterHorizontally
			Me.commandBarItem27.Enabled = False
			Me.commandBarItem27.Hint = "Horizontally center the selected controls within a band"
			Me.commandBarItem27.Id = 30
			Me.commandBarItem27.ImageIndex = 36
			Me.commandBarItem27.Name = "commandBarItem27"
			' 
			' commandBarItem28
			' 
			Me.commandBarItem28.Caption = "&Vertically"
			Me.commandBarItem28.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.CenterVertically
			Me.commandBarItem28.Enabled = False
			Me.commandBarItem28.Hint = "Vertically center the selected controls within a band"
			Me.commandBarItem28.Id = 31
			Me.commandBarItem28.ImageIndex = 37
			Me.commandBarItem28.Name = "commandBarItem28"
			' 
			' barSubItem14
			' 
			Me.barSubItem14.Caption = "&Order"
			Me.barSubItem14.Id = 59
			Me.barSubItem14.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem29, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem30)})
			Me.barSubItem14.Name = "barSubItem14"
			' 
			' commandBarItem29
			' 
			Me.commandBarItem29.Caption = "&Bring to Front"
			Me.commandBarItem29.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.BringToFront
			Me.commandBarItem29.Enabled = False
			Me.commandBarItem29.Hint = "Bring the selected controls to the front"
			Me.commandBarItem29.Id = 32
			Me.commandBarItem29.ImageIndex = 38
			Me.commandBarItem29.Name = "commandBarItem29"
			' 
			' commandBarItem30
			' 
			Me.commandBarItem30.Caption = "&Send to Back"
			Me.commandBarItem30.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.SendToBack
			Me.commandBarItem30.Enabled = False
			Me.commandBarItem30.Hint = "Move the selected controls to the back"
			Me.commandBarItem30.Id = 33
			Me.commandBarItem30.ImageIndex = 39
			Me.commandBarItem30.Name = "commandBarItem30"
			' 
			' barSubItem15
			' 
			Me.barSubItem15.Caption = "Custom Action"
			Me.barSubItem15.Id = 68
			Me.barSubItem15.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.barButtonItem1)})
			Me.barSubItem15.Name = "barSubItem15"
			' 
			' designBar2
			' 
			Me.designBar2.BarName = "Toolbar"
			Me.designBar2.DockCol = 0
			Me.designBar2.DockRow = 1
			Me.designBar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
			Me.designBar2.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem31), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem32), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem33), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem34, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem35), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem36), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem37, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem38)})
			Me.designBar2.Text = "Toolbar"
			' 
			' designBar3
			' 
			Me.designBar3.BarName = "Formatting Toolbar"
			Me.designBar3.DockCol = 1
			Me.designBar3.DockRow = 1
			Me.designBar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
			Me.designBar3.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.barEditItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.barEditItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem3), New DevExpress.XtraBars.LinkPersistInfo(Me.commandColorBarItem1, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandColorBarItem2), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem4, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem5), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem6), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem7)})
			Me.designBar3.Text = "Formatting Toolbar"
			' 
			' barEditItem1
			' 
			Me.barEditItem1.Caption = "Font Name"
			Me.barEditItem1.Edit = Me.recentlyUsedItemsComboBox1
			Me.barEditItem1.Hint = "Font Name"
			Me.barEditItem1.Id = 0
			Me.barEditItem1.Name = "barEditItem1"
			Me.barEditItem1.Width = 120
			' 
			' recentlyUsedItemsComboBox1
			' 
			Me.recentlyUsedItemsComboBox1.AutoHeight = False
			Me.recentlyUsedItemsComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() { New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.recentlyUsedItemsComboBox1.DropDownRows = 12
			Me.recentlyUsedItemsComboBox1.Name = "recentlyUsedItemsComboBox1"
			' 
			' barEditItem2
			' 
			Me.barEditItem2.Caption = "Font Size"
			Me.barEditItem2.Edit = Me.designRepositoryItemComboBox1
			Me.barEditItem2.Hint = "Font Size"
			Me.barEditItem2.Id = 1
			Me.barEditItem2.Name = "barEditItem2"
			Me.barEditItem2.Width = 55
			' 
			' designRepositoryItemComboBox1
			' 
			Me.designRepositoryItemComboBox1.AutoHeight = False
			Me.designRepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() { New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.designRepositoryItemComboBox1.Name = "designRepositoryItemComboBox1"
			' 
			' designBar4
			' 
			Me.designBar4.BarName = "Layout Toolbar"
			Me.designBar4.DockCol = 0
			Me.designBar4.DockRow = 2
			Me.designBar4.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
			Me.designBar4.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem8), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem9, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem10), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem11), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem12, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem13), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem14), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem15, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem16), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem17), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem18), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem19, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem20), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem21), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem22), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem23, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem24), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem25), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem26), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem27, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem28), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem29, True), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem30)})
			Me.designBar4.Text = "Layout Toolbar"
			' 
			' designBar5
			' 
			Me.designBar5.BarName = "Status Bar"
			Me.designBar5.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
			Me.designBar5.DockCol = 0
			Me.designBar5.DockRow = 0
			Me.designBar5.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
			Me.designBar5.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.barStaticItem1)})
			Me.designBar5.OptionsBar.AllowQuickCustomization = False
			Me.designBar5.OptionsBar.DrawDragBorder = False
			Me.designBar5.OptionsBar.UseWholeRow = True
			Me.designBar5.Text = "Status Bar"
			' 
			' barStaticItem1
			' 
			Me.barStaticItem1.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
			Me.barStaticItem1.Id = 42
			Me.barStaticItem1.Name = "barStaticItem1"
			Me.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near
			Me.barStaticItem1.Width = 32
			' 
			' designBar6
			' 
			Me.designBar6.BarName = "Zoom Toolbar"
			Me.designBar6.DockCol = 1
			Me.designBar6.DockRow = 2
			Me.designBar6.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
			Me.designBar6.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() { New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem44), New DevExpress.XtraBars.LinkPersistInfo(Me.xrZoomBarEditItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.commandBarItem45)})
			Me.designBar6.Text = "Zoom Toolbar"
			' 
			' commandBarItem44
			' 
			Me.commandBarItem44.Caption = "Zoom Out"
			Me.commandBarItem44.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.ZoomOut
			Me.commandBarItem44.Enabled = False
			Me.commandBarItem44.Hint = "Zoom out the design surface"
			Me.commandBarItem44.Id = 65
			Me.commandBarItem44.ImageIndex = 40
			Me.commandBarItem44.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Subtract))
			Me.commandBarItem44.Name = "commandBarItem44"
			' 
			' xrZoomBarEditItem1
			' 
			Me.xrZoomBarEditItem1.Caption = "Zoom"
			Me.xrZoomBarEditItem1.Edit = Me.designRepositoryItemComboBox2
			Me.xrZoomBarEditItem1.Enabled = False
			Me.xrZoomBarEditItem1.Hint = "Select or input the zoom factor"
			Me.xrZoomBarEditItem1.Id = 66
			Me.xrZoomBarEditItem1.Name = "xrZoomBarEditItem1"
			Me.xrZoomBarEditItem1.Width = 70
			' 
			' designRepositoryItemComboBox2
			' 
			Me.designRepositoryItemComboBox2.AutoComplete = False
			Me.designRepositoryItemComboBox2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() { New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.designRepositoryItemComboBox2.Name = "designRepositoryItemComboBox2"
			' 
			' commandBarItem45
			' 
			Me.commandBarItem45.Caption = "Zoom In"
			Me.commandBarItem45.Command = DevExpress.XtraReports.UserDesigner.ReportCommand.ZoomIn
			Me.commandBarItem45.Enabled = False
			Me.commandBarItem45.Hint = "Zoom in the design surface"
			Me.commandBarItem45.Id = 67
			Me.commandBarItem45.ImageIndex = 41
			Me.commandBarItem45.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Add))
			Me.commandBarItem45.Name = "commandBarItem45"
			' 
			' barDockControlTop
			' 
			Me.barDockControlTop.CausesValidation = False
			Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
			Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
			Me.barDockControlTop.Size = New System.Drawing.Size(1008, 84)
			' 
			' barDockControlBottom
			' 
			Me.barDockControlBottom.CausesValidation = False
			Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
			Me.barDockControlBottom.Location = New System.Drawing.Point(0, 707)
			Me.barDockControlBottom.Size = New System.Drawing.Size(1008, 25)
			' 
			' barDockControlLeft
			' 
			Me.barDockControlLeft.CausesValidation = False
			Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
			Me.barDockControlLeft.Location = New System.Drawing.Point(0, 84)
			Me.barDockControlLeft.Size = New System.Drawing.Size(0, 623)
			' 
			' barDockControlRight
			' 
			Me.barDockControlRight.CausesValidation = False
			Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
			Me.barDockControlRight.Location = New System.Drawing.Point(1008, 84)
			Me.barDockControlRight.Size = New System.Drawing.Size(0, 623)
			' 
			' xrDesignDockManager1
			' 
			Me.xrDesignDockManager1.Form = Me
			Me.xrDesignDockManager1.ImageStream = (CType(resources.GetObject("xrDesignDockManager1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer))
			Me.xrDesignDockManager1.MenuManager = Me.xrDesignBarManager1
			Me.xrDesignDockManager1.RootPanels.AddRange(New DevExpress.XtraBars.Docking.DockPanel() { Me.panelContainer1, Me.toolBoxDockPanel1, Me.groupAndSortDockPanel1})
			Me.xrDesignDockManager1.TopZIndexControls.AddRange(New String() { "DevExpress.XtraBars.BarDockControl", "DevExpress.XtraBars.StandaloneBarDockControl", "System.Windows.Forms.StatusBar", "DevExpress.XtraBars.Ribbon.RibbonStatusBar", "DevExpress.XtraBars.Ribbon.RibbonControl"})
			Me.xrDesignDockManager1.XRDesignPanel = Me.xrDesignPanel1
			' 
			' panelContainer1
			' 
			Me.panelContainer1.Controls.Add(Me.panelContainer2)
			Me.panelContainer1.Controls.Add(Me.propertyGridDockPanel1)
			Me.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right
			Me.panelContainer1.ID = New System.Guid("48686ecb-fae8-4327-89f7-18680136a04d")
			Me.panelContainer1.Location = New System.Drawing.Point(758, 84)
			Me.panelContainer1.Name = "panelContainer1"
			Me.panelContainer1.OriginalSize = New System.Drawing.Size(250, 463)
			Me.panelContainer1.Size = New System.Drawing.Size(250, 623)
			Me.panelContainer1.Text = "panelContainer1"
			' 
			' panelContainer2
			' 
			Me.panelContainer2.ActiveChild = Me.fieldListDockPanel1
			Me.panelContainer2.Controls.Add(Me.fieldListDockPanel1)
			Me.panelContainer2.Controls.Add(Me.reportExplorerDockPanel1)
			Me.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill
			Me.panelContainer2.ID = New System.Guid("d1a51387-0c37-4e5d-8084-af462e4e2d9b")
			Me.panelContainer2.Location = New System.Drawing.Point(0, 0)
			Me.panelContainer2.Name = "panelContainer2"
			Me.panelContainer2.OriginalSize = New System.Drawing.Size(250, 314)
			Me.panelContainer2.Size = New System.Drawing.Size(250, 311)
			Me.panelContainer2.Tabbed = True
			Me.panelContainer2.Text = "panelContainer2"
			' 
			' fieldListDockPanel1
			' 
			Me.fieldListDockPanel1.Controls.Add(Me.fieldListDockPanel1_Container)
			Me.fieldListDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill
			Me.fieldListDockPanel1.ID = New System.Guid("faf69838-a93f-4114-83e8-d0d09cc5ce95")
			Me.fieldListDockPanel1.ImageIndex = 0
			Me.fieldListDockPanel1.Location = New System.Drawing.Point(4, 23)
			Me.fieldListDockPanel1.Name = "fieldListDockPanel1"
			Me.fieldListDockPanel1.OriginalSize = New System.Drawing.Size(244, 263)
			Me.fieldListDockPanel1.Size = New System.Drawing.Size(242, 256)
			Me.fieldListDockPanel1.Text = "Field List"
			Me.fieldListDockPanel1.XRDesignPanel = Me.xrDesignPanel1
			' 
			' fieldListDockPanel1_Container
			' 
			Me.fieldListDockPanel1_Container.Location = New System.Drawing.Point(0, 0)
			Me.fieldListDockPanel1_Container.Name = "fieldListDockPanel1_Container"
			Me.fieldListDockPanel1_Container.Size = New System.Drawing.Size(242, 256)
			Me.fieldListDockPanel1_Container.TabIndex = 0
			' 
			' xrDesignPanel1
			' 
			Me.xrDesignPanel1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.xrDesignPanel1.Location = New System.Drawing.Point(165, 84)
			Me.xrDesignPanel1.Name = "xrDesignPanel1"
			Me.xrDesignPanel1.Padding = New System.Windows.Forms.Padding(1)
			Me.xrDesignPanel1.Size = New System.Drawing.Size(593, 463)
			Me.xrDesignPanel1.TabIndex = 5
			' 
			' reportExplorerDockPanel1
			' 
			Me.reportExplorerDockPanel1.Controls.Add(Me.reportExplorerDockPanel1_Container)
			Me.reportExplorerDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill
			Me.reportExplorerDockPanel1.ID = New System.Guid("fb3ec6cc-3b9b-4b9c-91cf-cff78c1edbf1")
			Me.reportExplorerDockPanel1.ImageIndex = 2
			Me.reportExplorerDockPanel1.Location = New System.Drawing.Point(4, 23)
			Me.reportExplorerDockPanel1.Name = "reportExplorerDockPanel1"
			Me.reportExplorerDockPanel1.OriginalSize = New System.Drawing.Size(244, 263)
			Me.reportExplorerDockPanel1.Size = New System.Drawing.Size(242, 256)
			Me.reportExplorerDockPanel1.Text = "Report Explorer"
			Me.reportExplorerDockPanel1.XRDesignPanel = Me.xrDesignPanel1
			' 
			' reportExplorerDockPanel1_Container
			' 
			Me.reportExplorerDockPanel1_Container.Location = New System.Drawing.Point(0, 0)
			Me.reportExplorerDockPanel1_Container.Name = "reportExplorerDockPanel1_Container"
			Me.reportExplorerDockPanel1_Container.Size = New System.Drawing.Size(242, 256)
			Me.reportExplorerDockPanel1_Container.TabIndex = 0
			' 
			' propertyGridDockPanel1
			' 
			Me.propertyGridDockPanel1.Controls.Add(Me.propertyGridDockPanel1_Container)
			Me.propertyGridDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill
			Me.propertyGridDockPanel1.ID = New System.Guid("b38d12c3-cd06-4dec-b93d-63a0088e495a")
			Me.propertyGridDockPanel1.ImageIndex = 1
			Me.propertyGridDockPanel1.Location = New System.Drawing.Point(0, 311)
			Me.propertyGridDockPanel1.Name = "propertyGridDockPanel1"
			Me.propertyGridDockPanel1.OriginalSize = New System.Drawing.Size(250, 315)
			Me.propertyGridDockPanel1.Size = New System.Drawing.Size(250, 312)
			Me.propertyGridDockPanel1.Text = "Property Grid"
			Me.propertyGridDockPanel1.XRDesignPanel = Me.xrDesignPanel1
			' 
			' propertyGridDockPanel1_Container
			' 
			Me.propertyGridDockPanel1_Container.Location = New System.Drawing.Point(4, 23)
			Me.propertyGridDockPanel1_Container.Name = "propertyGridDockPanel1_Container"
			Me.propertyGridDockPanel1_Container.Size = New System.Drawing.Size(242, 285)
			Me.propertyGridDockPanel1_Container.TabIndex = 0
			' 
			' toolBoxDockPanel1
			' 
			Me.toolBoxDockPanel1.Controls.Add(Me.toolBoxDockPanel1_Container)
			Me.toolBoxDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left
			Me.toolBoxDockPanel1.ID = New System.Guid("161a5a1a-d9b9-4f06-9ac4-d0c3e507c54f")
			Me.toolBoxDockPanel1.ImageIndex = 3
			Me.toolBoxDockPanel1.Location = New System.Drawing.Point(0, 84)
			Me.toolBoxDockPanel1.Name = "toolBoxDockPanel1"
			Me.toolBoxDockPanel1.OriginalSize = New System.Drawing.Size(165, 623)
			Me.toolBoxDockPanel1.Size = New System.Drawing.Size(165, 623)
			Me.toolBoxDockPanel1.Text = "Tool Box"
			Me.toolBoxDockPanel1.XRDesignPanel = Me.xrDesignPanel1
			' 
			' toolBoxDockPanel1_Container
			' 
			Me.toolBoxDockPanel1_Container.Location = New System.Drawing.Point(4, 23)
			Me.toolBoxDockPanel1_Container.Name = "toolBoxDockPanel1_Container"
			Me.toolBoxDockPanel1_Container.Size = New System.Drawing.Size(157, 596)
			Me.toolBoxDockPanel1_Container.TabIndex = 0
			' 
			' groupAndSortDockPanel1
			' 
			Me.groupAndSortDockPanel1.Controls.Add(Me.groupAndSortDockPanel1_Container)
			Me.groupAndSortDockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom
			Me.groupAndSortDockPanel1.ID = New System.Guid("4bab159e-c495-4d67-87dc-f4e895da443e")
			Me.groupAndSortDockPanel1.ImageIndex = 4
			Me.groupAndSortDockPanel1.Location = New System.Drawing.Point(165, 547)
			Me.groupAndSortDockPanel1.Name = "groupAndSortDockPanel1"
			Me.groupAndSortDockPanel1.OriginalSize = New System.Drawing.Size(843, 160)
			Me.groupAndSortDockPanel1.Size = New System.Drawing.Size(593, 160)
			Me.groupAndSortDockPanel1.Text = "Group and Sort"
			Me.groupAndSortDockPanel1.XRDesignPanel = Me.xrDesignPanel1
			' 
			' groupAndSortDockPanel1_Container
			' 
			Me.groupAndSortDockPanel1_Container.Location = New System.Drawing.Point(4, 23)
			Me.groupAndSortDockPanel1_Container.Name = "groupAndSortDockPanel1_Container"
			Me.groupAndSortDockPanel1_Container.Size = New System.Drawing.Size(585, 133)
			Me.groupAndSortDockPanel1_Container.TabIndex = 0
			' 
			' barButtonItem1
			' 
			Me.barButtonItem1.Caption = "Bind to a DataSource"
			Me.barButtonItem1.Id = 71
			Me.barButtonItem1.Name = "barButtonItem1"
'			Me.barButtonItem1.ItemClick += New DevExpress.XtraBars.ItemClickEventHandler(Me.commandBarItem46_ItemClick);
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(1008, 732)
			Me.Controls.Add(Me.xrDesignPanel1)
			Me.Controls.Add(Me.groupAndSortDockPanel1)
			Me.Controls.Add(Me.toolBoxDockPanel1)
			Me.Controls.Add(Me.panelContainer1)
			Me.Controls.Add(Me.barDockControlLeft)
			Me.Controls.Add(Me.barDockControlRight)
			Me.Controls.Add(Me.barDockControlBottom)
			Me.Controls.Add(Me.barDockControlTop)
			Me.Name = "Form1"
			Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
			Me.Text = "Form1"
'			Me.Load += New System.EventHandler(Me.Form1_Load);
			CType(Me.xrDesignBarManager1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.recentlyUsedItemsComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.designRepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.designRepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.xrDesignDockManager1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.panelContainer1.ResumeLayout(False)
			Me.panelContainer2.ResumeLayout(False)
			Me.fieldListDockPanel1.ResumeLayout(False)
			CType(Me.xrDesignPanel1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.reportExplorerDockPanel1.ResumeLayout(False)
			Me.propertyGridDockPanel1.ResumeLayout(False)
			Me.toolBoxDockPanel1.ResumeLayout(False)
			Me.groupAndSortDockPanel1.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private xrDesignBarManager1 As DevExpress.XtraReports.UserDesigner.XRDesignBarManager
		Private designBar1 As DevExpress.XtraReports.UserDesigner.DesignBar
		Private barSubItem1 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem31 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem39 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem32 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem33 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem40 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem41 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem2 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem37 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem38 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem34 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem35 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem36 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem42 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem43 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem3 As DevExpress.XtraBars.BarSubItem
		Private barReportTabButtonsListItem1 As DevExpress.XtraReports.UserDesigner.BarReportTabButtonsListItem
		Private barSubItem4 As DevExpress.XtraBars.BarSubItem
		Private xrBarToolbarsListItem1 As DevExpress.XtraReports.UserDesigner.XRBarToolbarsListItem
		Private barSubItem5 As DevExpress.XtraBars.BarSubItem
		Private barDockPanelsListItem1 As DevExpress.XtraReports.UserDesigner.BarDockPanelsListItem
		Private barSubItem6 As DevExpress.XtraBars.BarSubItem
		Private commandColorBarItem1 As DevExpress.XtraReports.UserDesigner.CommandColorBarItem
		Private commandColorBarItem2 As DevExpress.XtraReports.UserDesigner.CommandColorBarItem
		Private barSubItem7 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem1 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem2 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem3 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem8 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem4 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem5 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem6 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem7 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem9 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem9 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem10 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem11 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem12 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem13 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem14 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem8 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem10 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem15 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem16 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem17 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem18 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem11 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem19 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem20 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem21 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem22 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem12 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem23 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem24 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem25 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem26 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem13 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem27 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem28 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barSubItem14 As DevExpress.XtraBars.BarSubItem
		Private commandBarItem29 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private commandBarItem30 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private designBar2 As DevExpress.XtraReports.UserDesigner.DesignBar
		Private designBar3 As DevExpress.XtraReports.UserDesigner.DesignBar
		Private barEditItem1 As DevExpress.XtraBars.BarEditItem
		Private recentlyUsedItemsComboBox1 As DevExpress.XtraReports.UserDesigner.RecentlyUsedItemsComboBox
		Private barEditItem2 As DevExpress.XtraBars.BarEditItem
		Private designRepositoryItemComboBox1 As DevExpress.XtraReports.UserDesigner.DesignRepositoryItemComboBox
		Private designBar4 As DevExpress.XtraReports.UserDesigner.DesignBar
		Private designBar5 As DevExpress.XtraReports.UserDesigner.DesignBar
		Private barStaticItem1 As DevExpress.XtraBars.BarStaticItem
		Private designBar6 As DevExpress.XtraReports.UserDesigner.DesignBar
		Private commandBarItem44 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private xrZoomBarEditItem1 As DevExpress.XtraReports.UserDesigner.XRZoomBarEditItem
		Private designRepositoryItemComboBox2 As DevExpress.XtraReports.UserDesigner.DesignRepositoryItemComboBox
		Private commandBarItem45 As DevExpress.XtraReports.UserDesigner.CommandBarItem
		Private barDockControlTop As DevExpress.XtraBars.BarDockControl
		Private barDockControlBottom As DevExpress.XtraBars.BarDockControl
		Private barDockControlLeft As DevExpress.XtraBars.BarDockControl
		Private barDockControlRight As DevExpress.XtraBars.BarDockControl
		Private xrDesignPanel1 As DevExpress.XtraReports.UserDesigner.XRDesignPanel
		Private barSubItem15 As DevExpress.XtraBars.BarSubItem
		Private xrDesignDockManager1 As DevExpress.XtraReports.UserDesigner.XRDesignDockManager
		Private groupAndSortDockPanel1 As DevExpress.XtraReports.UserDesigner.GroupAndSortDockPanel
		Private groupAndSortDockPanel1_Container As DevExpress.XtraReports.UserDesigner.DesignControlContainer
		Private toolBoxDockPanel1 As DevExpress.XtraReports.UserDesigner.ToolBoxDockPanel
		Private toolBoxDockPanel1_Container As DevExpress.XtraReports.UserDesigner.DesignControlContainer
		Private panelContainer1 As DevExpress.XtraBars.Docking.DockPanel
		Private panelContainer2 As DevExpress.XtraBars.Docking.DockPanel
		Private reportExplorerDockPanel1 As DevExpress.XtraReports.UserDesigner.ReportExplorerDockPanel
		Private reportExplorerDockPanel1_Container As DevExpress.XtraReports.UserDesigner.DesignControlContainer
		Private fieldListDockPanel1 As DevExpress.XtraReports.UserDesigner.FieldListDockPanel
		Private fieldListDockPanel1_Container As DevExpress.XtraReports.UserDesigner.DesignControlContainer
		Private propertyGridDockPanel1 As DevExpress.XtraReports.UserDesigner.PropertyGridDockPanel
		Private propertyGridDockPanel1_Container As DevExpress.XtraReports.UserDesigner.DesignControlContainer
		Private WithEvents barButtonItem1 As DevExpress.XtraBars.BarButtonItem
	End Class
End Namespace


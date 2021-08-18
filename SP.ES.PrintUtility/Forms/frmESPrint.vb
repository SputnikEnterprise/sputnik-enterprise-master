
Option Strict Off

Imports System.Reflection.Assembly
Imports System.IO
Imports DevComponents.DotNetBar.Metro.ColorTables

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports SPS.Listing.Print.Utility
Imports SPS.Listing.Print.Utility.ESVertrag
Imports SPS.Listing.Print.Utility.ESVerleih

Imports SPS.ES.Utility.SPSESUtility.ClsESFunktionality
Imports SPS.ES.Utility.SPRPSUtility.ClsRPFunktionality

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Threading
Imports DevExpress.XtraBars
Imports DevComponents.DotNetBar
Imports System.ComponentModel
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging


Public Class frmESPrint
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ESSetting As New ClsESSetting
	Private _ClsFunc As New ClsDivFunc
	Private m_xml As New ClsXML

	Private bAllowedtowrite As Boolean

	Private PrintListingThread As Thread
	Private ExportListingThread As Thread
	Private DeleteListingThread As Thread

	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private Property SelectedYear2Print As Integer

	Private Property PrintJobNr As String
	Private Property SQL4Print As String

	Private Property bPrintAsDesign As Boolean
	Private Property bPrintAsExport As Boolean
	Private Property bSendPrintJob2WOS As Short
	Private Property bSend_And_PrintJob2WOS As Short
	Private Property WOSProperty4Search As Short

	Private Property SelectedESNr As New List(Of Integer)
	Private Property SelectedMANr As New List(Of Integer)
	Private Property SelectedKDNr As New List(Of Integer)
	Private Property SelectedKDZHDNr As New List(Of Integer)
	Private Property SelectedData2WOS As New List(Of Boolean)
	Private Property SelectedMALang As New List(Of String)
	Private Property SelectedKDLang As New List(Of String)

	Private Property SelectedMAData2WOS As New List(Of Boolean)
	Private Property SelectedKDData2WOS As New List(Of Boolean)

	Private Property ResultOFPrintingESVertragRec As String
	Private Property ResultOFPrintingESVerleihRec As String

	Private Property ResultOFExportingESVertragRec As String
	Private Property ResultOFExportingESVerleihRec As String

	Private Property ResultOFDeletingESRec As String

	Private WithEvents mESVertragWorker As BackgroundWorker
	Private WithEvents mESVertragWOSWorker As BackgroundWorker
	Private WithEvents mESVerleihWorker As BackgroundWorker
	Private WithEvents mESVerleihWOSWorker As BackgroundWorker

  Private m_md As Mandant

  Private WithEvents mESDeleteWorker As BackgroundWorker


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsESSetting)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant

		Me._ESSetting = _setting

	End Sub

#End Region


	Private Sub sbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbClose.Click
		Me.Dispose()
	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount

		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)
		Me.lblHeader1.Text = m_xml.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_xml.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.sbClose.Text = m_xml.GetSafeTranslationValue(Me.sbClose.Text)

		Me.bbiSearch.Caption = m_xml.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiPrint.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_xml.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiSetting.Caption = m_xml.GetSafeTranslationValue(Me.bbiSetting.Caption)
		Me.bbiDelete.Caption = m_xml.GetSafeTranslationValue(Me.bbiDelete.Caption)

		Me.grpSuchkriterien.Text = m_xml.GetSafeTranslationValue(Me.grpSuchkriterien.Text)
		Me.lblDetails.Text = m_xml.GetSafeTranslationValue(Me.lblDetails.Text)
		Me.lblJahr.Text = m_xml.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_xml.GetSafeTranslationValue(Me.lblMonat.Text)
		Me.lblKanton.Text = m_xml.GetSafeTranslationValue(Me.lblKanton.Text)
		Me.lblPVLBeruf.Text = m_xml.GetSafeTranslationValue(Me.lblPVLBeruf.Text)
		Me.lblESNr.Text = m_xml.GetSafeTranslationValue(Me.lblESNr.Text)
		Me.lblWOS.Text = m_xml.GetSafeTranslationValue(Me.lblWOS.Text)
		Me.chkESVertrag.Text = m_xml.GetSafeTranslationValue(Me.chkESVertrag.Text)
		Me.chkVerleihvertrag.Text = m_xml.GetSafeTranslationValue(Me.chkVerleihvertrag.Text)
		Me.btnWOSProperty.Text = m_xml.GetSafeTranslationValue(Me.btnWOSProperty.Text)

	End Sub

	Private Sub frmESPrint_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.Save()
		End If

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmESPrint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
			StyleManager.MetroColorGeneratorParameters = New MetroColorGeneratorParameters(_ESSetting.MetroForeColor, _
																																				 _ESSetting.MetroBorderColor)

			Try
				If My.Settings.iHeight > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.iHeight)
				If My.Settings.iWidth > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.iWidth)
				If My.Settings.frmLocation <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		Try
			' UpdateMethod() enthält den Code, der ein Windows-Steuerelement modifiziert.
			Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
			' UpdateMethod() wird nun auf dem Benutzeroberflächen-Thread aufgerufen.
			Me.Invoke(UpdateDelegate)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try
		If Not m_md.AllowedExportEmployee2WOS(ClsDataDetail.MDData.MDNr, Now.Year) Then
			Me.btnWOSProperty.Visible = False
			Me.btnWOSProperty.Text = String.Empty
			Me.lblWOS.Visible = False

		Else
			CreateWOSPopup()
			Me.btnWOSProperty.Text = m_xml.GetSafeTranslationValue("Keine Suchkriterien")

		End If
		Me.WOSProperty4Search = 2

		Dim strESNr As String = String.Empty
		For i As Integer = 0 To _ESSetting.SelectedESNr.Count - 1
			strESNr &= If(Not String.IsNullOrWhiteSpace(strESNr), ",", "") & _ESSetting.SelectedESNr(i)
		Next
		Me.cbo_ESNr.Text = strESNr


		Me.cbo_Month.Text = If(_ESSetting.SelectedMonth(0) = 0, Now.Month, _ESSetting.SelectedMonth(0))
		Me.cbo_Year.Text = If(_ESSetting.SelectedMonth(0) = 0, Now.Year, _ESSetting.SelectedYear(0))

		Me.LblTimeValue.Visible = CBool(CInt(ClsDataDetail.UserData.UserNr) = 1)
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

		If _ESSetting.SearchAutomatic Then
			Dim b As DevExpress.XtraBars.ItemClickEventArgs = Nothing
			bbiSearch_ItemClick(bbiSearch, DirectCast(b, DevExpress.XtraBars.ItemClickEventArgs))
		End If

		Try
			' Löschen
			If Not IsUserActionAllowed(0, 252) Then Me.bbiDelete.Visibility = BarItemVisibility.Never

			' allgemeines Drucken und Exportieren
			If Not IsUserActionAllowed(0, 253) And Not IsUserActionAllowed(0, 255) Then
				Me.bbiPrint.Visibility = BarItemVisibility.Never
				Me.bbiExport.Visibility = BarItemVisibility.Never
			End If
			If ClsDataDetail.UserData.UserNr <> 1 And (Not IsUserAllowed4DocExport("4.2") Or Not IsUserAllowed4DocExport("4.3")) Then Me.bbiExport.Visibility = BarItemVisibility.Never

			' Einsatzvertrag drucken
			If Not IsUserActionAllowed(0, 253) Then
				Me.chkESVertrag.Checked = False
				Me.chkESVertrag.Enabled = False
			End If
			' Verleihvertrag drucken
			If Not IsUserActionAllowed(0, 255) Then
				Me.chkVerleihvertrag.Checked = False
				Me.chkVerleihvertrag.Enabled = False
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Benutzerrechte:{1}", strMethodeName, ex.Message))
			' Alles sperren...
			Me.bbiDelete.Visibility = BarItemVisibility.Never
			Me.bbiPrint.Visibility = BarItemVisibility.Never
			Me.bbiExport.Visibility = BarItemVisibility.Never
			Me.chkESVertrag.Checked = False
			Me.chkESVertrag.Enabled = False
			Me.chkVerleihvertrag.Checked = False
			Me.chkVerleihvertrag.Enabled = False

		End Try

	End Sub

	Private Sub form_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And ClsDataDetail.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			'For i As Integer = 0 To GetExecutingAssembly.GetReferencedAssemblies.Count - 1
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase) ' GetExecutingAssembly.GetReferencedAssemblies(i).FullName)
			Next

			strMsg = String.Format(strMsg, vbNewLine, _
														 GetExecutingAssembly().FullName, _
														 GetExecutingAssembly().Location, _
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
			'GetAssemyliesInfo()
		End If
	End Sub

#Region "Funktionen für Reset der Controls..."

	Sub BlankFields()
		ResetAllTabEntries()

		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

	End Sub


	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetAllTabEntries()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			For Each ctrls As Control In Me.Controls
				ResetControl(ctrls)
			Next

		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			'_ClsErrException.MessageBoxShowError(clsdatadetail.userdata.usernr, "ResetControl", ex)
		End Try

	End Sub

	Private Sub ResetControl(ByVal con As Control)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			' Rekursiver Aufruf
			If con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else
				' Sonst Control zurücksetzen
				If TypeOf (con) Is TextBox Then
					Dim tb As TextBox = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is System.Windows.Forms.ComboBox Or TypeOf (con) Is ComboBoxEdit Or TypeOf (con) Is CheckedComboBoxEdit Then
					Dim cbo As System.Windows.Forms.ComboBox = con
					cbo.Text = String.Empty
					cbo.SelectedIndex = -1

				ElseIf TypeOf (con) Is ListBox Then
					Dim lst As ListBox = con
					lst.Items.Clear()

				End If
			End If

		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub


#End Region


	Sub FillFieldsWithDefaults()

		BlankFields()
		Me.cbo_Month.Text = If(Now.Day < 15, Now.Month - 1, Now.Month)
		Me.cbo_Year.Text = If(Now.Day < 15 And Now.Month = 1, Now.Year - 1, Now.Year)

	End Sub



	Private Sub cbo_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cbo_ESNr.ButtonClick
		Dim frmTest As New frmSearchRec(Me.cbo_PVLBez.Text, _
																		Me.cbo_PVLKanton.Text, "Einsatz-Nr.", _
																		CInt(Val(Me.cbo_Year.Text)), CInt(Val(Me.cbo_Month.Text)))

		ClsDataDetail.strButtonValue = Me.cbo_ESNr.Text

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iMyValue(_ClsFunc.GetSelektion)
		Me.cbo_ESNr.Text = If(m = Nothing, String.Empty, CStr(m.ToString))
		frmTest.Dispose()

	End Sub

	Private Sub cbo_PVLNr_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_PVLBez.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		_ESSetting.SelectedYear.Clear()
		_ESSetting.SelectedMonth.Clear()
		_ESSetting.SelectedWOSProperty = Me.WOSProperty4Search

		If CInt(Val(Me.cbo_Year.Text)) <> 0 Then _ESSetting.SelectedYear.Add(CInt(Val(Me.cbo_Year.Text)))
		If CInt(Val(Me.cbo_Month.Text)) <> 0 Then _ESSetting.SelectedMonth.Add(CInt(Val(Me.cbo_Month.Text)))
		_ESSetting.SelectedKanton = Me.cbo_PVLKanton.Text

		Try

			Try
				Me.Cursor = Cursors.WaitCursor

				ListPVLBez(cbo_PVLBez, _ESSetting)


			Catch ex As Exception	' Manager
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Private Sub cbo_PVLKanton_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_PVLKanton.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		_ESSetting.SelectedYear.Clear()
		_ESSetting.SelectedMonth.Clear()
		_ESSetting.SelectedWOSProperty = Me.WOSProperty4Search

		_ESSetting.SelectedYear.Add(CInt(Val(Me.cbo_Year.Text)))
		_ESSetting.SelectedMonth.Add(CInt(Val(Me.cbo_Month.Text)))
		_ESSetting.SelectedPVLBez = Me.cbo_PVLBez.Text

		Try

			Try
				Me.Cursor = Cursors.WaitCursor

				ListPVLKanton(cbo_PVLKanton, _ESSetting)


			Catch ex As Exception	' Manager
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub


	Private Sub cbo_Month_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Month.KeyPress, cbo_Year.KeyPress, _
		cbo_PVLKanton.KeyPress

		Select Case Asc(e.KeyChar)
			Case 48 To 57, 8, 32
				' Zahlen, Backspace und Space zulassen

			Case Else
				' alle anderen Eingaben unterdrücken
				e.Handled = True
		End Select

	End Sub

	Private Sub cbo_Year_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Year.QueryPopUp
		ListLOYear(sender)
	End Sub

	Private Sub cbo_Month_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Month.QueryPopUp
		ListLOMonth(sender, Me.cbo_Year.Text)
	End Sub


#Region "Listview Funktionen..."

	Private Sub ListViewEx1_ColumnWidthChanged(sender As Object, e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvDetails.ColumnWidthChanged
		Dim strColInfo As String = String.Empty
		Dim strColAlign As String = String.Empty

		For i As Integer = 0 To lvDetails.Columns.Count - 1
			If lvDetails.Columns.Item(i).TextAlign = HorizontalAlignment.Center Then
				strColAlign = "2"

			ElseIf lvDetails.Columns.Item(i).TextAlign = HorizontalAlignment.Right Then
				strColAlign = "1"
			Else
				strColAlign = "0"
			End If

			strColInfo &= CStr(IIf(strColInfo = String.Empty, "", ";")) & (lvDetails.Columns.Item(i).Width) & "-" & strColAlign
		Next
		My.Settings.LV_ColumnWidth = strColInfo
		My.Settings.Save()

	End Sub

	Private Sub ListViewEx1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lvDetails.SelectedIndexChanged
		Dim bSelected As Boolean = False

		For i As Integer = 0 To lvDetails.Items.Count - 1
			If lvDetails.Items(i).Selected = True Then
				'Me.LblRecID.Text = Me.LvFoundedrecs.Items(i).SubItems(0).Text
				'Me.LblRecNr.Text = Me.LvFoundedrecs.Items(i).SubItems(1).Text

				'DisplayFoundedData(Me, Me.LblRecID.Text)
				bSelected = True
				Exit For
			End If

		Next

		Me.bbiDelete.Enabled = bSelected
		Me.bbiPrint.Enabled = bSelected
		Me.bbiExport.Enabled = bSelected

	End Sub

#End Region


#Region "WOS-Popup..."

	Sub CreateWOSPopup()
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Nur WOS-Einsätze#1", _
																					 "Keine WOS-Einsätze#0", _
																					 "Keine Suchkriterien#2"}

		Me.btnWOSProperty.DropDownControl = popupMenu
		For i As Integer = 0 To liMnu.Count - 1
			Dim myValue As String() = liMnu(i).Split(CChar("#"))

			If myValue(0).ToString <> String.Empty Then
				popupMenu.Manager = BarManager1

				Dim itm As New DevExpress.XtraBars.BarButtonItem

				itm.Caption = m_xml.GetSafeTranslationValue(myValue(0).ToString)
				itm.Name = myValue(1).ToString

				popupMenu.AddItem(itm)
				AddHandler itm.ItemClick, AddressOf GetWOSPopupMnu

			End If

		Next

		'Throw New System.NotImplementedException()
	End Sub

	Public Sub GetWOSPopupMnu(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Me.btnWOSProperty.Text = e.Item.Caption
		Me.WOSProperty4Search = CShort(e.Item.Name)
	End Sub

#End Region


	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Einsatzdokumente Drucken#PrintES", _
																					 "Drucken ohne Übermittlung#PrintES", _
																					 "WOS -> übermitteln / restliche -> Drucken#SendWOS_PrintRest", _
																					 "Drucken mit Übermittlung#SendAndPrint", _
																					 "Entwurfsansicht#PrintDesign"}
		Try

			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			If m_md.AllowedExportEmployee2WOS(ClsDataDetail.MDData.MDNr, Now.Year) Then
				liMnu.RemoveAt(0)

			Else
				liMnu.RemoveRange(1, liMnu.Count - 2)

			End If

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			'Me.bbiPrint.Visibility = BarItemVisibility.Always
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then bshowMnu = IsUserActionAllowed(0, 254) Else bshowMnu = myValue(0).ToString <> String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_xml.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_xml.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False
		Me.bPrintAsExport = False
		Me.bSendPrintJob2WOS = False
		Me.bSend_And_PrintJob2WOS = False

		SelectedESNr.Clear()
		SelectedMANr.Clear()
		SelectedKDNr.Clear()
		SelectedKDZHDNr.Clear()
		SelectedMAData2WOS.Clear()
		SelectedKDData2WOS.Clear()
		SelectedMALang.Clear()
		SelectedKDLang.Clear()
		For Each item As ListViewItem In lvDetails.SelectedItems()
			SelectedESNr.Add(Val(item.SubItems(1).Text))
			SelectedMANr.Add(Val(item.SubItems(2).Text))
			SelectedKDNr.Add(Val(item.SubItems(3).Text))
			SelectedKDZHDNr.Add(Val(item.SubItems(4).Text))

			SelectedMAData2WOS.Add(If(e.Item.Name.ToUpper = "PrintES".ToUpper, False, CBool(item.SubItems(5).Text)))
			SelectedKDData2WOS.Add(If(e.Item.Name.ToUpper = "PrintES".ToUpper, False, CBool(item.SubItems(6).Text)))

			SelectedMALang.Add(item.SubItems(7).Text)
			SelectedKDLang.Add(item.SubItems(8).Text)
		Next

		Select Case e.Item.Name.ToUpper
			Case "PrintES".ToUpper

			Case "SendWOS_PrintRest".ToUpper
				Me.bSendPrintJob2WOS = True

			Case "SendAndPrint".ToUpper
				Me.bSend_And_PrintJob2WOS = True

			Case "printdesign".ToUpper
				Me.bPrintAsDesign = True


			Case Else
				Exit Sub

		End Select

		If SelectedESNr.Count > 0 Then
			' Testing
			If Me.chkVerleihvertrag.Checked Then

				StartPrintingWithESVerleihVertrag()
				'StartSendigVerleih2WOS()

				'mESVerleihWorker = New BackgroundWorker
				'mESVerleihWorker.WorkerReportsProgress = True
				'AddHandler mESVerleihWorker.DoWork, AddressOf StartPrintingWithESVerleihVertrag
				'AddHandler mESVerleihWorker.RunWorkerCompleted, AddressOf StartPrintingWithESVerleihVertragCompleted

				'mESVerleihWorker.RunWorkerAsync()


				'mESVerleihWOSWorker = New BackgroundWorker
				'mESVerleihWOSWorker.WorkerReportsProgress = True
				'AddHandler mESVerleihWOSWorker.DoWork, AddressOf StartSendigVerleih2WOS
				'AddHandler mESVerleihWOSWorker.RunWorkerCompleted, AddressOf StartSendigVerleih2WOSCompleted

				'mESVerleihWOSWorker.RunWorkerAsync()

			End If
			If Me.chkESVertrag.Checked Then
				StartPrintingWithESVertrag()
				'StartSendigESVertrag2WOS()

				'mESVertragWorker = New BackgroundWorker
				'mESVertragWorker.WorkerReportsProgress = True
				'AddHandler mESVertragWorker.DoWork, AddressOf StartPrintingWithESVertrag
				'AddHandler mESVertragWorker.RunWorkerCompleted, AddressOf StartPrintingWithESVertragCompleted

				'mESVertragWorker.RunWorkerAsync()
			End If

			' End of Testing

		Else
			Dim strMsg As String = m_xml.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _
																								 m_xml.GetSafeTranslationValue("Einsatzverträge verwalten"), _
																								MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)

		End If

	End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim liESnr As String() = Me.cbo_ESNr.Text.Split(CChar(","))
		Dim aYear As String() = Me.cbo_Year.Text.Split(CChar(","))
		Dim aMonth As String() = Me.cbo_Month.Text.Split(CChar(","))

		_ESSetting.SelectedYear.Clear()
		_ESSetting.SelectedMonth.Clear()
		_ESSetting.SelectedWOSProperty = Me.WOSProperty4Search
		_ESSetting.SelectedESNr.Clear()

		For i As Integer = 0 To aYear.Length - 1
			If CInt(Val(aYear(i))) <> 0 Then
				_ESSetting.SelectedYear.Add(CInt(Val(aYear(i))))
			End If
		Next
		For i As Integer = 0 To aMonth.Length - 1
			If CInt(Val(aMonth(i))) <> 0 Then
				_ESSetting.SelectedMonth.Add(CInt(Val(aMonth(i))))
			End If
		Next

		_ESSetting.SelectedPVLBez = Me.cbo_PVLBez.Text
		_ESSetting.SelectedKanton = Me.cbo_PVLKanton.Text

		For i As Integer = 0 To liESnr.Count - 1
			_ESSetting.SelectedESNr.Add(CInt(Val(liESnr(i))))
		Next

		_ESSetting.PrintESVertrag = Me.chkESVertrag.Checked
		_ESSetting.PrintVerleihvertrag = Me.chkVerleihvertrag.Checked

		ListFoundedrec(Me.lvDetails, _ESSetting)

		If Me.lvDetails.Items.Count > 1 Then
			Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden gefunden."), Me.lvDetails.Items.Count)

		Else
			Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensatz wurde gefunden."), Me.lvDetails.Items.Count)
		End If

		CreatePrintPopupMenu()

		Me.bbiPrint.Enabled = Me.lvDetails.SelectedItems.Count > 0
		Me.bbiExport.Enabled = Me.lvDetails.SelectedItems.Count > 0
		Me.bbiDelete.Enabled = Me.lvDetails.SelectedItems.Count > 0

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Me.SQL4Print = String.Empty
		Me.PrintJobNr = "9.1"

		Me.bPrintAsDesign = False
		Me.bPrintAsExport = False
		Me.bSendPrintJob2WOS = False
		Me.bSend_And_PrintJob2WOS = False

		SelectedESNr.Clear()
		SelectedMANr.Clear()
		SelectedKDNr.Clear()
		SelectedKDZHDNr.Clear()
		SelectedMAData2WOS.Clear()
		SelectedKDData2WOS.Clear()
		SelectedMALang.Clear()
		SelectedKDLang.Clear()
		For Each item As ListViewItem In lvDetails.SelectedItems()
			SelectedESNr.Add(Val(item.SubItems(1).Text))
			SelectedMANr.Add(Val(item.SubItems(2).Text))
			SelectedKDNr.Add(Val(item.SubItems(3).Text))
			SelectedKDZHDNr.Add(Val(item.SubItems(4).Text))

			SelectedMAData2WOS.Add(False)	'If(e.Item.Name.ToUpper = "PrintES".ToUpper, False, CBool(item.SubItems(4).Text)))
			SelectedKDData2WOS.Add(False)	'If(e.Item.Name.ToUpper = "PrintES".ToUpper, False, CBool(item.SubItems(5).Text)))

			SelectedMALang.Add(item.SubItems(7).Text)
			SelectedKDLang.Add(item.SubItems(8).Text)
		Next

		Me.bPrintAsDesign = False
		Me.bPrintAsExport = True
		Me.bSendPrintJob2WOS = False
		Me.bSend_And_PrintJob2WOS = False

		If SelectedESNr.Count > 0 Then
			' Testing

			If Me.chkESVertrag.Checked Then
				mESVertragWorker = New BackgroundWorker
				mESVertragWorker.WorkerReportsProgress = True
				mESVertragWorker.WorkerSupportsCancellation = True
				AddHandler mESVertragWorker.DoWork, AddressOf StartExportingWithESVertrag
				AddHandler mESVertragWorker.RunWorkerCompleted, AddressOf StartExportingWithESVertragCompleted

				mESVertragWorker.RunWorkerAsync()
			End If
			If Me.chkVerleihvertrag.Checked Then
				mESVerleihWorker = New BackgroundWorker
				mESVerleihWorker.WorkerReportsProgress = True
				mESVerleihWorker.WorkerSupportsCancellation = True
				AddHandler mESVerleihWorker.DoWork, AddressOf StartExportingWithESVerleihVertrag
				AddHandler mESVerleihWorker.RunWorkerCompleted, AddressOf StartExportingWithESVerleihVertragCompleted

				mESVerleihWorker.RunWorkerAsync()
			End If
			' End of Testing

		Else
			Dim strMsg As String = m_xml.GetSafeTranslationValue("Sie haben keine Daten ausgewählt.")
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _
																								 m_xml.GetSafeTranslationValue("Einsatzverträge verwalten"), _
																								MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)

		End If

	End Sub

#Region "Einsatzvertrag..."

	Sub StartPrintingWithESVertrag()
		Dim _Setting As New ClsLLESVertragSetting

		_Setting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																							 .SQL2Open = Me.SQL4Print, _
																							 .JobNr2Print = "4.3", _
																							 .IsPrintAsVerleih = False, _
																							 .Is4Export = Me.bPrintAsExport, _
																							 .liESNr2Print = Me.SelectedESNr, _
																							 .liMANr2Print = Me.SelectedMANr, _
																							 .liKDNr2Print = Me.SelectedKDNr, _
																							 .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
																							 .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
																							 .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
																							 .SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS, _
																							 .liESSend2WOS = Me.SelectedData2WOS, _
																							 .LiMALang = Me.SelectedMALang, _
																							 .LiKDLang = Me.SelectedKDLang, _
																							 .SelectedESNr2Print = Val(Me.cbo_ESNr.Text), _
																							 .SelectedMANr2Print = 0,
																							 .SelectedMDNr = ClsDataDetail.MDData.MDNr,
																							 .LogedUSNr = ClsDataDetail.UserData.UserNr,
																							 .PerosonalizedData = ClsDataDetail.ProsonalizedData, .TranslationData = ClsDataDetail.TranslationData}
		Dim obj As New ClsPrintESVertrag(_Setting)
		obj.PrintESVertrag(Me.bPrintAsDesign)

	End Sub

	Sub StartPrintingWithESVertragCompleted()


	End Sub

	Sub StartExportingWithESVertrag()
		Dim _Setting As New ClsLLESVertragSetting

		_Setting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																																						 .SQL2Open = Me.SQL4Print, _
																																						 .JobNr2Print = "4.3", _
																							 .IsPrintAsVerleih = False, _
																																						 .Is4Export = Me.bPrintAsExport, _
																																						 .liESNr2Print = Me.SelectedESNr, _
																																						 .liMANr2Print = Me.SelectedMANr, _
																																						 .liKDNr2Print = Me.SelectedKDNr, _
																																						 .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
																																						 .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
																																						 .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
																																						 .SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS, _
																																						 .liESSend2WOS = Me.SelectedData2WOS, _
																																						 .LiMALang = Me.SelectedMALang, _
																																						 .LiKDLang = Me.SelectedKDLang, _
																																						 .SelectedESNr2Print = Val(Me.cbo_ESNr.Text), _
																																						 .SelectedMANr2Print = 0,
																							 .SelectedMDNr = ClsDataDetail.MDData.MDNr,
																							 .LogedUSNr = ClsDataDetail.UserData.UserNr,
																							 .PerosonalizedData = ClsDataDetail.ProsonalizedData, .TranslationData = ClsDataDetail.TranslationData}
		Dim obj As New ClsPrintESVertrag(_Setting)
		Dim strResult As String = obj.ExportESVertrag()
		Me.ResultOFExportingESVertragRec = strResult

	End Sub

	Sub StartExportingWithESVertragCompleted()

		If File.Exists(ResultOFExportingESVertragRec) Then
			Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in {0} gespeichert."), ResultOFExportingESVertragRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)
			Process.Start(ResultOFExportingESVertragRec)

		Else
			Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.{0}{1}"), _
																					 vbNewLine, ResultOFExportingESVertragRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If

	End Sub


	'Sub StartSendigESVertrag2WOS()
	'  Dim _Setting As New ClsLLESVertragSetting

	'  _Setting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
	'                                             .SQL2Open = Me.SQL4Print, _
	'                                             .JobNr2Print = "4.3", _
	'                                             .IsPrintAsVerleih = False, _
	'                                             .Is4Export = Me.bPrintAsExport, _
	'                                             .liESNr2Print = Me.SelectedESNr, _
	'                                             .liMANr2Print = Me.SelectedMANr, _
	'                                             .liKDNr2Print = Me.SelectedKDNr, _
	'                                             .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
	'                                             .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
	'                                             .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
	'                                             .SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS, _
	'                                             .liESSend2WOS = Me.SelectedData2WOS, _
	'                                             .LiMALang = Me.SelectedMALang, _
	'                                             .LiKDLang = Me.SelectedKDLang, _
	'                                             .SelectedESNr2Print = Val(Me.cbo_ESNr.Text), _
	'                                             .SelectedMANr2Print = 0}
	'  Dim obj As New ClsPrintESVertrag(_Setting)
	'  obj.SendESVertrag2WOS()

	'End Sub

	Sub StartSendigESVertrag2WOSCompleted()


	End Sub


#End Region


#Region "Verleihvertrag..."

	Sub StartPrintingWithESVerleihVertrag()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLESVertragSetting

		_Setting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																																						 .SQL2Open = Me.SQL4Print, _
																																						 .JobNr2Print = "4.2", _
																																				 .IsPrintAsVerleih = True, _
																																						 .Is4Export = Me.bPrintAsExport, _
																																						 .liESNr2Print = Me.SelectedESNr, _
																																						 .liMANr2Print = Me.SelectedMANr, _
																																						 .liKDNr2Print = Me.SelectedKDNr, _
																																						 .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
																																						 .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
																																						 .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
																																						 .SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS, _
																																						 .liESSend2WOS = Me.SelectedData2WOS, _
																																						 .LiMALang = Me.SelectedMALang, _
																																						 .LiKDLang = Me.SelectedKDLang, _
																																						 .SelectedESNr2Print = Val(Me.cbo_ESNr.Text), _
																																						 .SelectedMANr2Print = 0,
																							 .SelectedMDNr = ClsDataDetail.MDData.MDNr,
																							 .LogedUSNr = ClsDataDetail.UserData.UserNr,
																							 .PerosonalizedData = ClsDataDetail.ProsonalizedData, .TranslationData = ClsDataDetail.TranslationData}
		Dim obj As New ClsPrintESVerleihvertrag(_Setting)
		obj.PrintESVerleih(Me.bPrintAsDesign)

	End Sub

	'Sub StartPrintingWithESVerleihVertragCompleted()

	'  If Me.chkESVertrag.Checked Then
	'    mESVertragWorker = New BackgroundWorker
	'    mESVertragWorker.WorkerReportsProgress = True
	'    AddHandler mESVertragWorker.DoWork, AddressOf StartPrintingWithESVertrag
	'    AddHandler mESVertragWorker.RunWorkerCompleted, AddressOf StartPrintingWithESVertragCompleted

	'    mESVertragWorker.RunWorkerAsync()

	'    ' WOS Senden...
	'    mESVertragWOSWorker = New BackgroundWorker
	'    mESVertragWOSWorker.WorkerReportsProgress = True
	'    AddHandler mESVertragWOSWorker.DoWork, AddressOf StartSendigESVertrag2WOS
	'    AddHandler mESVertragWOSWorker.RunWorkerCompleted, AddressOf StartSendigESVertrag2WOSCompleted

	'    mESVertragWOSWorker.RunWorkerAsync()

	'  End If

	'End Sub

	Sub StartSendigVerleih2WOS()
		'Dim _Setting As New SPS.Listing.Print.Utility.ClsLLESVertragSetting

		'_Setting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
		'                                                                         .SQL2Open = Me.SQL4Print, _
		'                                                                         .JobNr2Print = "4.2", _
		'                                                                     .IsPrintAsVerleih = True, _
		'                                                                         .Is4Export = Me.bPrintAsExport, _
		'                                                                         .liESNr2Print = Me.SelectedESNr, _
		'                                                                         .liMANr2Print = Me.SelectedMANr, _
		'                                                                         .liKDNr2Print = Me.SelectedKDNr, _
		'                                                                         .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
		'                                                                         .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
		'                                                                         .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
		'                                                                         .SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS, _
		'                                                                         .liESSend2WOS = Me.SelectedData2WOS, _
		'                                                                         .LiMALang = Me.SelectedMALang, _
		'                                                                         .LiKDLang = Me.SelectedKDLang, _
		'                                                                         .SelectedESNr2Print = Val(Me.cbo_ESNr.Text), _
		'                                                                         .SelectedMANr2Print = 0}
		'Dim obj As New ClsPrintESVerleihvertrag(_Setting)
		'obj.SendESVerleih2WOS()

	End Sub

	Sub StartSendigVerleih2WOSCompleted()

	End Sub


	Sub StartExportingWithESVerleihVertrag()
		Dim _Setting As New ClsLLESVertragSetting

		_Setting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																																						 .SQL2Open = Me.SQL4Print, _
																																						 .JobNr2Print = "4.2", _
																							 .IsPrintAsVerleih = True, _
																																						 .Is4Export = Me.bPrintAsExport, _
																																						 .liESNr2Print = Me.SelectedESNr, _
																																						 .liMANr2Print = Me.SelectedMANr, _
																																						 .liKDNr2Print = Me.SelectedKDNr, _
																																						 .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
																																						 .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
																																						 .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
																																						 .SendAndPrintData2WOS = Me.bSend_And_PrintJob2WOS, _
																																						 .liESSend2WOS = Me.SelectedData2WOS, _
																																						 .LiMALang = Me.SelectedMALang, _
																																						 .LiKDLang = Me.SelectedKDLang, _
																																						 .SelectedESNr2Print = Val(Me.cbo_ESNr.Text), _
																																						 .SelectedMANr2Print = 0,
																							 .SelectedMDNr = ClsDataDetail.MDData.MDNr,
																							 .LogedUSNr = ClsDataDetail.UserData.UserNr,
																							 .PerosonalizedData = ClsDataDetail.ProsonalizedData, .TranslationData = ClsDataDetail.TranslationData}

		Dim obj As New ClsPrintESVerleihvertrag(_Setting)
		Dim strResult As String = obj.ExportESVerleihvertrag()
		Me.ResultOFExportingESVerleihRec = strResult

	End Sub

	Sub StartExportingWithESVerleihVertragCompleted()

		If File.Exists(ResultOFExportingESVerleihRec) Then
			Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in {0} gespeichert."), ResultOFExportingESVerleihRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)
			Process.Start(ResultOFExportingESVerleihRec)

		Else
			Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.{0}{1}"), _
																					 vbNewLine, ResultOFExportingESVerleihRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If

	End Sub


#End Region


	Private Sub bbiSetting_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSetting.ItemClick
		Dim frm As New frmESPrintSetting

		'For i As Integer = 1 To 10000
		'  frm.Opacity = i / 10
		'Next
		frm.Top = (Me.Top + Me.Height) - frm.Height - 50
		frm.Left = (Me.Left + Me.Width) - frm.Width - 50

		frm.Show()
		'ShowForm(frm)

	End Sub

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub bbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
		Dim bAddtoList As Boolean = False
		Dim strClosedLP As String = String.Empty
		Dim sMonth As Short = 0
		Dim iYear As Short = 0

		Dim strMsg As String = m_xml.GetSafeTranslationValue("Der Datensatz wird gelöscht. Möchten Sie wirklich diesen Datensatz löschen?")
		If DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Datensatz löschen?"), _
																									MessageBoxButtons.YesNo, _
																									MessageBoxIcon.Question, _
																									MessageBoxDefaultButton.Button2) = DialogResult.No Then
			Exit Sub
		End If

		SelectedESNr.Clear()
		For Each item As ListViewItem In lvDetails.SelectedItems()
			'Dim aZeitraum As String() = item.SubItems(5).Text.Split(CChar("/"))
			'If aZeitraum.Length > 0 Then
			'  sMonth = CShort(Val(aZeitraum(0).Trim))
			'  iYear = CInt(Val(aZeitraum(1).Trim))
			'  strClosedLP = IsMonthClosed(sMonth, iYear).ToString.Trim

			'  bAddtoList = strClosedLP.Length = 0
			'  If strClosedLP.Length > 0 Then
			'    strClosedLP &= If(strClosedLP.Length > 0, "", vbNewLine) & strClosedLP
			'  End If

			'End If
			SelectedESNr.Add(Val(item.SubItems(1).Text))
			'If bAddtoList Then SelectedESNr.Add(Val(item.SubItems(1).Text))
		Next
		'If strClosedLP.Length > 0 Then
		'  strMsg = String.Format(m_xml.GetSafeTranslationValue("Folgende Monate sind bereits abgeschlossen:{0}{1}"), vbNewLine, strClosedLP)
		'  DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _
		'                                             m_xml.GetSafeTranslationValue("Einsätze löschen"), _
		'                                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
		'End If

		If SelectedESNr.Count > 0 Then
			'DeleteListingThread = New Thread(AddressOf StartDeleteingSelected)
			'DeleteListingThread.Name = "DeleteLOListing"
			'DeleteListingThread.SetApartmentState(ApartmentState.STA)
			'DeleteListingThread.Start()

			' Testing...
			mESDeleteWorker = New BackgroundWorker
			mESDeleteWorker.WorkerReportsProgress = True
			AddHandler mESDeleteWorker.DoWork, AddressOf StartDeleteingSelectedES
			AddHandler mESDeleteWorker.RunWorkerCompleted, AddressOf StartDeleteingSelectedESCompleted

			mESDeleteWorker.RunWorkerAsync()

			' Testing Ende...

		Else
			strMsg = String.Format(m_xml.GetSafeTranslationValue("Sie haben keine Daten ausgewählt."), vbNewLine, strClosedLP)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _
																								 m_xml.GetSafeTranslationValue("Einsätze löschen"), _
																								MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)

		End If

	End Sub

	Sub StartDeleteingSelectedES()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim liESNr As New List(Of Integer)

		Try
			ResultOFDeletingESRec = DeleteSelectedES(Me.SelectedESNr, False) 'New List(Of Integer)(New Integer() {CInt(Me.txtLONr.Text)}), false)

		Catch ex As Exception
			m_Logger.LogInfo(String.Format("{0}:{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub StartDeleteingSelectedESCompleted()

		If Not ResultOFDeletingESRec.ToLower.Contains("error") Then
			Dim strMsg As String = m_xml.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gelöscht.")

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten löschen"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)

			Dim b As DevExpress.XtraBars.ItemClickEventArgs = Nothing
			bbiSearch_ItemClick(bbiSearch, DirectCast(b, DevExpress.XtraBars.ItemClickEventArgs))

			' Hauptübersicht aktuallisieren...
			RunLVUpdate()

		Else
			Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gelöscht werden.{0}{1}"), _
																					 vbNewLine, _
																					 ResultOFExportingESVerleihRec)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten löschen"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

		End If

	End Sub



	Private Sub cbo_ESNr_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_ESNr.SelectedIndexChanged

	End Sub
End Class


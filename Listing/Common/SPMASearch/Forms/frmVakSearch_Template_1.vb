
Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Threading

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging

Public Class frmVakSearch_Template_1
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private m_md As Mandant
	Private m_utilities As Utilities
	Private m_xml As New ClsXML

	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmMASearch_LV

	Private PrintListingThread As Thread
	Private Property ShortSQLQuery As String
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant
		m_utilities = New Utilities

	End Sub

#End Region



	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Try
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Nach Daten wird gesucht...")
			Me.txt_IndSQLQuery.Text = String.Empty
			Me.ShortSQLQuery = String.Empty

			FormIsLoaded("frmMASearch_LV", True)
			If ClsDataDetail.GetLVSortBez <> String.Empty Then Me.CboSort.Text = ClsDataDetail.GetLVSortBez

			' Die Query-String aufbauen...
			GetMyQueryString()

			Stopwatch.Reset()
			Stopwatch.Start()

			' Daten auflisten...
			If Not FormIsLoaded("frmMASearch_LV", True) Then
				frmMyLV = New frmMASearch_LV(Me.txt_IndSQLQuery.Text, False)

				frmMyLV.Show()
				Me.Select()
			End If

			Me.LblTimeValue.Text = String.Format(m_xml.GetSafeTranslationValue("Datenauflistung für {0} Einträge: in {1} ms"),
																					 frmMyLV.RecCount,
																					 Stopwatch.ElapsedMilliseconds().ToString)

			Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																				 frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																							frmMyLV.RecCount)

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				CreatePrintPopupMenu()
				CreateExportPopupMenu()

				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			MessageBox.Show(ex.Message, "btnSearch_Click")

		Finally

		End Try

	End Sub

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty

		Dim _ClsDb As New ClsDbFunc(ClsDataDetail.GetVakNr, Me.Chk_HBerufe.Checked,
																Me.Chk_SBerufe.Checked, Me.Chk_Branche.Checked,
																Me.Chk_Geschlecht.Checked, Me.Chk_InES.Checked,
																Me.Cbo_MAKontakt.Text, Me.Cbo_MAStatus1.Text)
		If Me.txt_IndSQLQuery.Text = String.Empty Then

			sSql1Query = _ClsDb.GetStartQuery_Template_1()        ' 1. String
			sSql2Query = _ClsDb.GetQuerySQLString_Template_1(sSql1Query)       ' Where Klausel

			If Trim(sSql2Query) <> String.Empty Then
				sSql1Query += " Where "
			End If
			strSort = _ClsDb.GetSortString_Template_1(Me.CboSort.Text.Trim)          ' Sort Klausel

			Me.txt_IndSQLQuery.Text = sSql1Query + sSql2Query '+ strSort
			Me.ShortSQLQuery = String.Format("Select * From _MAVakTemplate_{0} {1}",
																						 ClsDataDetail.UserData.UserNr, strSort)
			Me.txt_IndSQLQuery.Text = String.Format(Me.txt_IndSQLQuery.Text & " " & ShortSQLQuery,
																						 ClsDataDetail.UserData.UserNr, strSort)

			If strLastSortBez = String.Empty Then strLastSortBez = strSort

		End If

		Return True
	End Function


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 611)
		Dim liMnu As New List(Of String) From {"Kandidatenliste#mnumalistedrucken",
																					 If(bShowDesign, "Entwurfanssicht Kandidatenliste#mnudesignmaliste", "")}
		If Not IsUserActionAllowed(0, 601) Then Exit Sub
		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				bshowMnu = myValue(0).ToString <> String.Empty

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_xml.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_xml.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower.Contains("mnudesignmaliste".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False

		Select Case e.Item.Name.ToUpper
			Case "mnumalistedrucken".ToUpper
				GetMAData4Print(False, False, "1.3")

			Case "mnudesignmaliste".ToUpper
				GetMAData4Print(True, False, "1.3")


			Case Else
				Exit Sub

		End Select

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiExport.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As List(Of String) = GetMenuItems4Export()

		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()
			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			'Me.bbiExport.Visibility = BarItemVisibility.Always
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_xml.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_xml.GetSafeTranslationValue(myValue(1).ToString)
					itm.AccessibleName = myValue(2).ToString
					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.ShortSQLQuery

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				Dim ExportThread As New Thread(AddressOf StartExportModul)
				ExportThread.Name = "ExportMAListing"
				ExportThread.Start()


			Case UCase("Contact")
				Call ExportDataToOutlook(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

			Case UCase("MAIL")
				Call RunMailModul(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

			Case UCase("Multi-Db")
				Call RunBewModul(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

			Case UCase("FAX")
				Call RunTobitFaxModul(Me.ShortSQLQuery) ' Me.txt_SQLQuery.Text)

				'Case UCase("SMS")
				'  Dim sSql As String = "Select MANr, MANachname As Nachname, MAVorname As Vorname, ( "
				'  sSql &= "Case MA_SMS_Mailing "
				'  sSql &= "When 0 Then MANatel Else '' End) As _MANatel "
				'  sSql &= "From _MATemplate_{0} Where (MANatel <> '' And MANatel Is Not Null ) And MA_SMS_Mailing <> 1 "
				'  sSql &= "Order by MANachname"
				'  sSql = String.Format(sSql, ClsDataDetail.UserData.UserNr)

				'  Call RunSMSProg(sSql) ' Me.txt_SQLQuery.Text)


		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																			 .SQL2Open = Me.ShortSQLQuery,
																																			 .ModulName = "MASearch"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		'Dim _clsExport As New SPS.Export.Listing.Utility.ClsExportStart(Me.ShortSQLQuery, "MASearch")
		obj.ExportCSVFromMASearchListing(Me.ShortSQLQuery)
	End Sub





	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmMASearch_LV", True)
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()

		Me.CboSort.Text = strText   ' Letzte Sortierung wieder herstellen
		Me.txt_IndSQLQuery.Text = m_xml.GetSafeTranslationValue("Wurde geleert...")

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub
	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each ctrls In Me.Controls
			ResetControl(CType(ctrls, Control))
		Next
	End Sub


	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion mit rekursivem Aufruf.</remarks>
	Private Sub ResetControl(ByVal con As Control)

		If con.Name = "CboSort" Then Exit Sub
		If TypeOf (con) Is TextBox Then
			Dim tb As TextBox = CType(con, TextBox)
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
			Dim tb As DevExpress.XtraEditors.TextEdit = CType(con, DevExpress.XtraEditors.TextEdit)
			tb.Text = String.Empty
		ElseIf TypeOf (con) Is TextBox Then
			Dim tb As DevExpress.XtraEditors.TextEdit = CType(con, DevExpress.XtraEditors.TextEdit)
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = CType(con, DevExpress.XtraEditors.ComboBoxEdit)
			cbo.Properties.Items.Clear()
			cbo.Text = String.Empty
		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(con, DevExpress.XtraEditors.CheckedComboBoxEdit)
			cbo.Properties.Items.Clear()
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
			Dim de As DevExpress.XtraEditors.DateEdit = CType(con, DevExpress.XtraEditors.DateEdit)
			de.Text = String.Empty
			de.EditValue = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
			Dim de As DevExpress.XtraEditors.CheckEdit = CType(con, DevExpress.XtraEditors.CheckEdit)
			de.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is CheckBox Then
			Dim cbo As CheckBox = CType(con, CheckBox)
			cbo.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is ComboBox Then
			Dim cbo As ComboBox = CType(con, ComboBox)
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl Then
			Dim lst As DevExpress.XtraEditors.ListBoxControl = CType(con, DevExpress.XtraEditors.ListBoxControl)
			lst.Items.Clear()

		ElseIf con.HasChildren Then
			For Each conChild As Control In con.Controls
				ResetControl(conChild)
			Next
		End If

		If TypeOf (con) Is DevExpress.XtraTab.XtraTabPage Then
			Dim tabPg As DevExpress.XtraTab.XtraTabPage = CType(con, DevExpress.XtraTab.XtraTabPage)
			tabPg.Text = tabPg.Text.Replace("*", "")
		End If

		If TypeOf (con) Is TabPage Then
			Dim tabPg As TabPage = CType(con, TabPage)
			tabPg.Text = tabPg.Text.Replace("*", "")
		End If

	End Sub

#Region "Form Funktionen..."

	Sub StartTranslation()
		Dim m_xml As New ClsXML

		m_xml.GetChildChildBez(Me)
		Dim Time_1 As Double = System.Environment.TickCount
		Dim Time_2 As Double = System.Environment.TickCount

		If ClsDataDetail.UserData.UserNr = 1 Then
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Trace.WriteLine("1. Verbrauchte Zeit: " & ((Time_2 - Time_1) / 1000) & " s.")
		End If

	End Sub

	Private Sub frmVakSearch_Template_1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strSort As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																									 Me.Name & "_0").ToString
		If strSort = String.Empty Or Not strSort.Contains(" - ") Then strSort = "1 - Kandidatennname"
		Me.CboSort.Text = strSort
		Dim iChkValue As Boolean = If(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																													Me.Name & "_1").ToString) = 0, False, True)
		Me.Chk_HBerufe.Checked = iChkValue
		iChkValue = If(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																													Me.Name & "_2").ToString) = 0, False, True)
		Me.Chk_SBerufe.Checked = iChkValue

		iChkValue = If(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																													Me.Name & "_3").ToString) = 0, False, True)
		Me.Chk_Branche.Checked = iChkValue

		iChkValue = If(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																											Me.Name & "_4").ToString) = 0, False, True)
		Me.Chk_Geschlecht.Checked = iChkValue

		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth_2, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight_2, Me.Height)
			If My.Settings.frm_Location_2 <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location_2.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception

		End Try

		Me.txt_IndSQLQuery.Visible = ClsDataDetail.UserData.UserNr = 1
		Me.LibVakNr_1.TabStop = False

	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And ClsDataDetail.ProgSettingData.LogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Close()
	End Sub

	Private Sub frmMASearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmMASearch_LV", True)

		Try
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																													Me.Name & "_1", CStr(If(Me.Chk_HBerufe.Checked, 1, 0)))
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																													Me.Name & "_2", CStr(If(Me.Chk_SBerufe.Checked, 1, 0)))
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																													Me.Name & "_3", CStr(If(Me.Chk_Branche.Checked, 1, 0)))
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields",
																													Me.Name & "_4", CStr(If(Me.Chk_Geschlecht.Checked, 1, 0)))

			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					My.Settings.frm_Location_2 = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
					My.Settings.ifrmHeight_2 = Me.Height
					My.Settings.ifrmWidth_2 = Me.Width

					My.Settings.Save()
				End If

				PrintListingThread.Abort()

			Catch ex As Exception
				' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
			End Try

			PrintListingThread.Abort()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmMASearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmMASearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

#End Region

	Private Sub CboSort_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(CboSort)
	End Sub

	Private Sub CboSort_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryCloseUp
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_0", Me.CboSort.Text)
		ClsDataDetail.GetLVSortBez = String.Empty
	End Sub



#Region "Diverse Funktionen..."

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	''' <param name="bForExport">ob die Liste für Export ist.</param>
	''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	''' <remarks></remarks>
	Sub GetMAData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iMANr As Integer = 0
		Dim bResult As Boolean = True

		Dim sSql As String = Me.ShortSQLQuery ' Me.txt_IndSQLQuery.Text
		If sSql = String.Empty Then
			MsgBox("Keine Suche wurde gestartet!.", MsgBoxStyle.Exclamation, "GetMAData4Print")
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		If bForDesign Then sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ")
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

		Try
			Conn.Open()

			Dim rMArec As SqlDataReader = cmd.ExecuteReader                  ' Offertendatenbank
			Try
				If Not rMArec.HasRows Then
					cmd.Dispose()
					rMArec.Close()

					MessageBox.Show("Ich konnte leider Keine Daten finden.", "GetMAData", MessageBoxButtons.OK, MessageBoxIcon.Information)
				End If

			Catch ex As Exception
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetMAData")

			End Try
			rMArec.Read()
			If rMArec.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo

				PrintListingThread = New Thread(AddressOf StartPrinting)
				PrintListingThread.Name = "PrintingMAVakListing"
				PrintListingThread.SetApartmentState(ApartmentState.STA)
				PrintListingThread.Start()

				'Dim obj As New SPS.Listing.Print.Utility.MASearchListing.ClsPrintMASearchList(sSql, strJobInfo)
				'obj.PrintMASearchList_1(bForDesign, ClsDataDetail.GetSortBez, _
				'                        New List(Of String)(New String() {ClsDataDetail.GetFilterBez, _
				'                                                          ClsDataDetail.GetFilterBez2, _
				'                                                          ClsDataDetail.GetFilterBez3, _
				'                                                          ClsDataDetail.GetFilterBez4}))

			End If
			rMArec.Close()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetMAData4Print")

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try
	End Sub

	Sub StartPrinting()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																							 .SQL2Open = Me.SQL4Print,
																																							 .JobNr2Print = Me.PrintJobNr}
		Dim obj As New SPS.Listing.Print.Utility.MASearchListing.ClsPrintMASearchList(_Setting)
		obj.PrintMASearchList_1(Me.bPrintAsDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub


#End Region

	Private Sub LibVakNr_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LibVakNr_1.Click
		Dim frmTest As New frmMASearch

		Try
			FormIsLoaded("frmMASearch_LV", True)
			frmTest.Show()
			Me.Dispose()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Cbo_MAKontakt_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MAKontakt.QueryPopUp
		If Me.Cbo_MAKontakt.Properties.Items.Count = 0 Then ListMAKontakt(Cbo_MAKontakt)
	End Sub

	Private Sub Cbo_MAState_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MAStatus1.QueryPopUp
		If Me.Cbo_MAStatus1.Properties.Items.Count = 0 Then ListMAStatus1(Cbo_MAStatus1)
	End Sub



End Class
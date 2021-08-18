
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports System.Threading
Imports DevExpress.XtraEditors.Controls

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging

Public Class frmProposeSearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML
	Private _ClsFunc As New ClsDivFunc

	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_md As Mandant

	Private iInterval As Integer = 10
	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmSearch_LV

	Private pcc As New DevExpress.XtraBars.PopupControlContainer
	Private Property ShortSQLQuery As String
	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private Property TranslatedPage As New List(Of Boolean)


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		ResetMandantenDropDown()
		LoadMandantenDropDown()

	End Sub

#End Region

#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName",
																					 .Width = 100,
																					 .Caption = "Mandant"})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim _ClsFunc As New ClsDbFunc
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

		'    Return Not Data Is Nothing
	End Sub

	' Mandantendaten...
	Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If Not SelectedData Is Nothing Then
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.m_InitialData.UserData.UserNr)

		Else
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.m_InitialData.UserData.UserNr)

		End If
		ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)

		Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		'Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub


#End Region



#Region "Dropdown Funktionen Allgmeine"

	Private Sub CboSort_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(CboSort)
	End Sub

	Private Sub Cbo_Bez_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Bez.QueryPopUp
		If Me.Cbo_Bez.Properties.Items.Count = 0 Then ListBezeichnung(Cbo_Bez)
	End Sub

	Private Sub Cbo_State_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_State.QueryPopUp
		If Me.Cbo_State.Properties.Items.Count = 0 Then ListPStatus(Me.Cbo_State)
	End Sub

	Private Sub Cbo_Art_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Art.QueryPopUp
		If Me.Cbo_Art.Properties.Items.Count = 0 Then ListPArt(Me.Cbo_Art)
	End Sub

	Private Sub Cbo_Anstellung_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Anstellung.QueryPopUp
		If Me.Cbo_Anstellung.Properties.Items.Count = 0 Then ListPAnstellung(Me.Cbo_Anstellung)
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp

		If Me.Cbo_KST.SelectedIndex > 0 Then
			Dim item As ComboValue = CType(Me.Cbo_KST.SelectedItem, ComboValue)
			ListUSFilialen(Me.Cbo_Filiale, item.ComboValue)
		Else
			ListUSFilialen(Me.Cbo_Filiale)
		End If

	End Sub

	Private Sub Cbo_KST_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_KST.QueryPopUp
		FillFoundedKstBez(Me.Cbo_KST, Me.Cbo_Filiale.Text) ' ListPBerater(Me.Cbo_KST)
	End Sub

	Private Sub Cbo_Arbbegin_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Arbbegin.QueryPopUp
		If Me.Cbo_Arbbegin.Properties.Items.Count = 0 Then ListPArbbegin(Me.Cbo_Arbbegin)
	End Sub

	Private Sub Cbo_Lohn_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Lohn.QueryPopUp
		If Me.Cbo_Lohn.Properties.Items.Count = 0 Then ListPLohn(Me.Cbo_Lohn)
	End Sub

	Private Sub Cbo_Tarif_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Tarif.QueryPopUp
		If Me.Cbo_Tarif.Properties.Items.Count = 0 Then ListPTarif(Me.Cbo_Tarif)
	End Sub

#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmVAKSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmSearch_LV", True)

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				My.Settings.b_Show_1_ProposeSearch = Me.p_1.Height > 0
				My.Settings.b_Show_2_ProposeSearch = Me.p_2.Height > 0
				My.Settings.SortBez = Me.CboSort.Text

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmVAKSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		For i As Integer = 0 To Me.xtabProposeSearch.TabPages.Count - 1
			Me.TranslatedPage.Add(False)
		Next

		m_xml.GetChildChildBez(Me.xtabAllgemein)
		m_xml.GetChildChildBez(Me.PanelControl1)

		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)

		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_xml.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_xml.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrint.Caption)

		For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.xtabProposeSearch.TabPages
			tbp.Text = m_xml.GetSafeTranslationValue(tbp.Text)
		Next
		TranslatedPage(0) = True

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmProposeSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetDefaultSortValues()
		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)
		SetUserFilialFieldsValue()

		Me.bbiPrint.Enabled = False

		Me.LblTimeValue.Visible = CBool(CInt(ClsDataDetail.UserData.UserNr.ToString) = 1)
		ClsDataDetail.IsFirstTapiCall = True

		Me.xtabProposeSearch.SelectedTabPage = Me.xtabAllgemein
		Me.XtraTabControl1.SelectedTabPage = Me.xtabBegin

		' Tab-Reiter "SQL-Abfrage" und "Erweiterte Abfrage" anzeigen, nur wenn User = 1 (Admin) angemeldet ist.
		If ClsDataDetail.UserData.UserNr <> 1 Then
			Me.xtabProposeSearch.TabPages.Remove(Me.xtabSQLAbfrage)
			Me.xtabProposeSearch.TabPages.Remove(Me.xtabErweitert)
		End If

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				If strSort = String.Empty Then strSort = String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Firmenname"))
				ListSort(Me.CboSort)
				Dim aSortlist As String() = strSort.Split(CChar(","))

				For Each bez As String In aSortlist
					CboSort.Properties.Items(CInt(Val(bez))).CheckState = CheckState.Checked
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.Message))

			End Try

			Try
				Me.lueMandant.EditValue = ClsDataDetail.ProgSettingData.SelectedMDNr
				Dim showMDSelection As Boolean = IsUserActionAllowed(ClsDataDetail.ProgSettingData.LogedUSNr, 642, ClsDataDetail.ProgSettingData.SelectedMDNr)
				Me.lueMandant.Visible = showMDSelection
				Me.lblMDName.Visible = showMDSelection

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
				Me.lueMandant.Visible = False
				Me.lblMDName.Visible = False
			End Try

			Me.p_1.Height = 0
			Me.p_2.Height = 0

			If Not My.Settings.b_Show_1_ProposeSearch Then Me.p_1.Height = 0 Else LblHeader_1_Click(New Object, New System.EventArgs)
			If Not My.Settings.b_Show_2_ProposeSearch Then Me.p_2.Height = 0 Else LblHeader_2_Click(New Object, New System.EventArgs)

			Dim strQuery As String = m_xml.GetColorValue()
			If strQuery.Contains(";") Then
				Dim aColor As String() = strQuery.Split(";")
				Me.LblHeader_1.ForeColor = Color.FromArgb(aColor(0), aColor(1), aColor(2))
				Me.LblHeader_2.ForeColor = Color.FromArgb(aColor(0), aColor(1), aColor(2))

			Else
				Me.LblHeader_1.ForeColor = Color.FromName(strQuery)
				Me.LblHeader_2.ForeColor = Color.FromName(strQuery)

			End If
			RefreshAllPanels()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub SetUserFilialFieldsValue()

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(0, 672) Then
			Me.Cbo_Filiale.Enabled = False
			Dim strUSTitle As String = String.Format("{0}|{1}", ClsDataDetail.UserData.UserFTitel, ClsDataDetail.UserData.UserSTitel)

			Me.Cbo_KST.Enabled = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")
			Me.Cbo_Filiale.Text = ClsDataDetail.UserData.UserFiliale
			FillFoundedKstBez(Me.Cbo_KST, ClsDataDetail.UserData.UserFiliale)

			For i As Integer = 0 To Me.Cbo_KST.Properties.Items.Count - 1
				Dim item As ComboValue = DirectCast(Me.Cbo_KST.Properties.Items(i), ComboValue)

				If item.ComboValue.ToUpper = ClsDataDetail.m_InitialData.UserData.UserKST.ToUpper Then
					Me.Cbo_KST.SelectedIndex = i
					Exit For
				End If
			Next
		End If

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


	Private Sub xtabProposeSearch_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabProposeSearch.SelectedPageChanged

		If Not TranslatedPage(xtabProposeSearch.SelectedTabPageIndex) Then
			m_xml.GetChildChildBez(Me.xtabProposeSearch.SelectedTabPage)
			TranslatedPage(xtabProposeSearch.SelectedTabPageIndex) = True
		End If

	End Sub


	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	''' <param name="bForExport">ob die Liste für Export ist.</param>
	''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	''' <remarks></remarks>
	Sub GetVAKData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iESNr As Integer = 0
		Dim bResult As Boolean = True
		Dim storedProc As String = ""

		Dim sSql As String = Me.txt_SQLQuery.Text
		If sSql = String.Empty Then
			MsgBox("Keine Suche wurde gestartet!.", MsgBoxStyle.Exclamation, "GetESData4Print")
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		If bForDesign Then sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ")

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

		Try
			Conn.Open()

			Dim rFoundedrec4Print As SqlDataReader = cmd.ExecuteReader                  ' 
			Try
				If Not rFoundedrec4Print.HasRows Then
					cmd.Dispose()
					rFoundedrec4Print.Close()

					MessageBox.Show("Ich konnte leider Keine Daten finden.", "GetVAKData", MessageBoxButtons.OK, MessageBoxIcon.Information)
				End If

			Catch ex As Exception
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetVAKData")

			End Try
			rFoundedrec4Print.Read()
			If rFoundedrec4Print.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo

				PrintListingThread = New Thread(AddressOf StartPrinting)
				PrintListingThread.Name = "PrintingProposeListing"
				PrintListingThread.SetApartmentState(ApartmentState.STA)
				PrintListingThread.Start()

			End If


			rFoundedrec4Print.Close()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetVAKData4Print")

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try
	End Sub

	Sub StartPrinting()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLProposeSearchPrintSetting With {.SelectedMDNr = lueMandant.EditValue, .DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																							 .SQL2Open = Me.SQL4Print,
																																							 .JobNr2Print = Me.PrintJobNr}
		Dim obj As New SPS.Listing.Print.Utility.ProposeSearchListing.ClsPrintProposeSearchList(_Setting)
		obj.PrintProposeSearchList_1(Me.bPrintAsDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))

	End Sub


#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)

		Try
			Me.txt_SQLQuery.Text = String.Empty
			If Not Me.txt_PNr_2.Visible Then Me.txt_PNr_2.Text = Me.txt_PNr_1.Text
			If Not Me.txt_MANr_2.Visible Then Me.txt_MANr_2.Text = Me.txt_MANr_1.Text
			If Not Me.txt_KDNr_2.Visible Then Me.txt_KDNr_2.Text = Me.txt_KDNr_1.Text
			If Not Me.txt_VakNr_2.Visible Then Me.txt_VakNr_2.Text = Me.txt_VakNr_1.Text

			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			FormIsLoaded("frmSearch_LV", True)
			If ClsDataDetail.GetLVSortBez <> String.Empty Then Me.CboSort.Text = ClsDataDetail.GetLVSortBez

			' Die Query-String aufbauen...
			GetMyQueryString()

			Stopwatch.Reset()
			Stopwatch.Start()

			' Daten auflisten...
			If Not FormIsLoaded("frmSearch_LV", True) Then
				frmMyLV = New frmSearch_LV(Me.txt_SQLQuery.Text) ' , Me.Location.X, Me.Location.Y, Me.Height)
				frmMyLV.Show()
				Me.Select()
			End If

			Me.LblTimeValue.Text = String.Format(m_xml.GetSafeTranslationValue("Datenauflistung für {0} Einträge: in {1} ms"),
																					 frmMyLV.RecCount,
																					 Stopwatch.ElapsedMilliseconds().ToString)
			Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet"),
																				 frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet"),
																							frmMyLV.RecCount)

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.gvRP.RowCount > 0 Then
				Me.bbiPrint.Enabled = True
				'Me.bbiExport.Enabled = True
				CreatePrintPopupMenu()
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "btnSearch_Click")

		Finally

		End Try

	End Sub

	Function GetMyQueryString() As Boolean
		Dim sSqlQuerySelect As String = String.Empty
		Dim sSqlQueryWhere As String = String.Empty
		Dim sSqlQuerySort As String = String.Empty

		Dim _ClsDb As New ClsDbFunc
		If Me.txt_IndSQLQuery.Text = String.Empty Then

			' Selektion 
			sSqlQuerySelect = _ClsDb.GetStartSQLString()


			' Where Klausel
			sSqlQueryWhere = _ClsDb.GetQuerySQLString(sSqlQuerySelect, Me)

			If Trim(sSqlQueryWhere) <> String.Empty Then
				sSqlQuerySelect += " Where "
			End If

			' Sort Klausel
			sSqlQuerySort = _ClsDb.GetSortString(Me)

			' Query zusammenstellen
			Me.txt_SQLQuery.Text = sSqlQuerySelect + sSqlQueryWhere + sSqlQuerySort
			If strLastSortBez = String.Empty Then strLastSortBez = sSqlQuerySort

		Else

			Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
		End If

		Return True
	End Function


#Region "Contextmenü für Print und Export..."

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 805)
		Dim liMnu As New List(Of String) From {If(IsUserActionAllowed(0, 804), "Liste drucken#mnuListPrint", ""),
																					 If(bShowDesign, "Entwurfansicht#PrintDesign", "")}

		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			'Me.bbiPrint.Visibility = BarItemVisibility.Always
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				bshowMnu = myValue(0).ToString <> String.Empty

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_xml.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_xml.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
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

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		Select Case e.Item.Name.ToUpper
			Case "mnuListPrint".ToUpper
				GetVAKData4Print(False, False, ClsDataDetail.GetModulToPrint)

			Case "PrintDesign".ToUpper
				GetVAKData4Print(True, False, ClsDataDetail.GetModulToPrint)


			Case Else
				Exit Sub

		End Select

	End Sub


#End Region






	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmSearch_LV", True)
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()

		Me.CboSort.Text = strText   ' Letzte Sortierung wieder herstellen
		Me.txt_SQLQuery.Text = m_xml.GetSafeTranslationValue("Wurde geleert...")

		SetUserFilialFieldsValue()

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False

	End Sub
	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabProposeSearch").Controls
			For Each ctrls In tabPg.Controls
				ResetControl(ctrls)
			Next
		Next
	End Sub
	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		If TypeOf (con) Is TextBox Then
			Dim tb As TextBox = con
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
			cbo.Properties.Items.Clear()
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
			Dim de As DevExpress.XtraEditors.DateEdit = con
			de.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
			Dim de As DevExpress.XtraEditors.CheckEdit = con
			de.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is CheckBox Then
			Dim cbo As CheckBox = con
			cbo.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is ComboBox Then
			Dim cbo As ComboBox = con
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is ListBox Then
			Dim lst As ListBox = con
			lst.Items.Clear()

		ElseIf con.HasChildren Then
			For Each conChild As Control In con.Controls
				ResetControl(conChild)
			Next
		End If

		If TypeOf (con) Is DevExpress.XtraTab.XtraTabPage Then
			Dim tabPg As DevExpress.XtraTab.XtraTabPage = con
			tabPg.Text = tabPg.Text.Replace("*", "")
		End If

	End Sub


#End Region

#Region "Sonstige Funktionen..."

	Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

		Try
			If lv.Items.Count > 0 Then
				Dim lvi As ListViewItem = lv.SelectedItems(0)    '.Item(0)
				If lvi.Selected Then
					Return lvi.Index
				Else
					Return -1
				End If
			End If

		Catch ex As Exception

		End Try

	End Function

#End Region

#Region "KeyDown für Lst und Textfelder..."



#End Region

	Private Sub txt_PNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_PNr_1.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_PNr_1.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_PNr_1.Text))
		ClsDataDetail.strButtonValue = "Propose"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_PNr_1.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_PNr_1.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub

	Private Sub txt_PNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_PNr_2.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_PNr_2.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_PNr_2.Text))
		ClsDataDetail.strButtonValue = "Propose"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_PNr_2.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_PNr_2.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub


	Private Sub txt_MANr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr_1.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_MANr_1.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_MANr_1.Text))
		ClsDataDetail.strButtonValue = "MA"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_MANr_1.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_MANr_1.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub

	Private Sub txt_MANr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr_2.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_MANr_2.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_MANr_2.Text))
		ClsDataDetail.strButtonValue = "MA"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_MANr_2.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_MANr_2.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub


	Private Sub txt_KDNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_KDNr_1.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_KDNr_1.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_KDNr_1.Text))
		ClsDataDetail.strButtonValue = "KD"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_KDNr_1.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_KDNr_1.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub

	Private Sub txt_KDNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_KDNr_2.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_KDNr_2.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_KDNr_1.Text))
		ClsDataDetail.strButtonValue = "KD"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_KDNr_1.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_KDNr_2.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub


	Private Sub txt_VakNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_VakNr_1.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_VakNr_1.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_VakNr_1.Text))
		ClsDataDetail.strButtonValue = "VAK"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_VakNr_1.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_VakNr_1.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub

	Private Sub txt_VakNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_VakNr_2.ButtonClick
		Dim frmTest As New frmSearchRec

		ClsDataDetail.GetSelectedNumbers = Me.txt_VakNr_2.Text

		_ClsFunc.Get4What = CStr(Val(Me.txt_VakNr_2.Text))
		ClsDataDetail.strButtonValue = "VAK"
		ClsDataDetail.Get4What = CStr(Val(Me.txt_VakNr_2.Text))

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iValue(_ClsFunc.Get4What)
		Me.txt_VakNr_2.Text = CStr(ClsDataDetail.GetSelectedNumbers.Replace("#@", ",")) ' CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()

	End Sub




	''' <summary>
	''' KeyPressEvent der Controls auffangen und verarbeiten. (Enter --> Tab)
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles CboSort.KeyPress,
															txt_PNr_1.KeyPress, txt_PNr_2.KeyPress,
															txt_MANr_1.KeyPress, txt_MANr_2.KeyPress,
															txt_KDNr_1.KeyPress, txt_KDNr_2.KeyPress,
															txt_VakNr_1.KeyPress, txt_VakNr_2.KeyPress,
															Cbo_Bez.KeyPress, Cbo_Filiale.KeyPress, Cbo_KST.KeyPress,
															Cbo_Arbbegin.KeyPress, Cbo_Lohn.KeyPress, Cbo_Tarif.KeyPress,
															Cbo_State.KeyPress, Cbo_Art.KeyPress, Cbo_Anstellung.KeyPress

		Try
			If e.KeyChar = Chr(13) Then ' Enter
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Private Sub CboSort_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryCloseUp
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_0", Me.CboSort.Text)
		ClsDataDetail.GetLVSortBez = String.Empty
	End Sub

	Private Sub CboSort_Layout(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles CboSort.Layout
		' TODO: Inhalt aus XML-Datei holen
	End Sub

	Private Sub frmVAKSearch_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.txt_VakNr_1.Focus()
	End Sub

	Private Sub frmVAKSearch_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Me.Focus()
	End Sub





#Region "LblHeader_..."

	Private Sub LblHeader_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblHeader_1.Click
		Dim bIstOpen As Boolean
		bIstOpen = Me.p_1.Height > 0

		If Not bIstOpen Then
			For iPHeight As Integer = Me.p_1.Height To Me.txt_VakNr_1.Top + Me.txt_VakNr_1.Height Step 5
				p_1.Height = iPHeight
				p_1.Refresh()
				RefreshAllPanels()
				'        Threading.Thread.Sleep(iInterval)
				System.Windows.Forms.Application.DoEvents()
			Next
			Me.p_1.Height = Me.txt_VakNr_1.Top + Me.txt_VakNr_1.Height
			Me.txt_PNr_1.Focus()

		Else
			For iPHeight As Integer = Me.p_1.Height To Me.txt_VakNr_1.Top + Me.txt_VakNr_1.Height Step -5
				p_1.Height = iPHeight
				p_1.Refresh()
				Threading.Thread.Sleep(iInterval)
				System.Windows.Forms.Application.DoEvents()
			Next
			Me.p_1.Height = 0

			Me.CboSort.Focus()
		End If
		RefreshAllPanels()

	End Sub

	Private Sub LblHeader_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblHeader_2.Click
		Dim bIstOpen As Boolean
		bIstOpen = Me.p_2.Height > 0

		If Not bIstOpen Then
			For iPHeight As Integer = Me.p_2.Height To Me.Cbo_KST.Top + Me.Cbo_KST.Height Step 5
				p_2.Height = iPHeight
				p_2.Refresh()
				RefreshAllPanels()
				'       Threading.Thread.Sleep(iInterval)
				System.Windows.Forms.Application.DoEvents()
			Next
			Me.p_2.Height = Me.Cbo_KST.Top + Me.Cbo_KST.Height
			Me.Cbo_Bez.Focus()

		Else
			For iPHeight As Integer = Me.p_2.Height To Me.Cbo_KST.Top + Me.Cbo_KST.Height Step -5
				p_2.Height = iPHeight
				p_2.Refresh()
				Threading.Thread.Sleep(iInterval)
				System.Windows.Forms.Application.DoEvents()
			Next
			Me.p_2.Height = 0

			Me.CboSort.Focus()
		End If
		RefreshAllPanels()

	End Sub

#End Region

	Sub RefreshAllPanels()

		Me.xtabAllgemein.Refresh()

		Me.p_1.Top = Me.LblHeader_1.Top + 20

		Me.LblHeader_2.Top = Me.p_1.Top + Me.p_1.Height + 30
		Me.p_2.Top = Me.LblHeader_2.Top + 20 ' Me.p_1.Top + Me.p_1.Height + 30

	End Sub

	Private Sub LblSetting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		My.Settings.LV_1_Size_ProposeSearch = String.Empty
	End Sub

	Private Sub btnPrint_DropDownOpened(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_xml.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub




#Region "Switschboxen..."

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txt_PNr_2.Visible = Me.SwitchButton1.Value
		Me.txt_PNr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txt_MANr_2.Visible = Me.SwitchButton2.Value
		Me.txt_MANr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton3.ValueChanged
		Me.txt_KDNr_2.Visible = Me.SwitchButton3.Value
		Me.txt_KDNr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton4_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton4.ValueChanged
		Me.txt_VakNr_2.Visible = Me.SwitchButton4.Value
		Me.txt_VakNr_2.Text = String.Empty
	End Sub


#End Region



	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function



End Class



Module StringExtensions

	<Extension()>
	Public Sub KommasEntfernen(ByRef text As String)
		Do While (text.Contains(",,"))
			text = text.Replace(",,", ",")
		Loop
		If text.StartsWith(",") Then
			text = text.Remove(0, 1)
		End If
		If text.EndsWith(",") Then
			text = text.Remove(text.Length - 1, 1)
		End If
	End Sub

End Module
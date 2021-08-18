
Option Strict Off

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports DevExpress.XtraEditors.Controls

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Base
Imports System.Threading
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports System.Xml
Imports SP.Infrastructure.Logging

Public Class frmVakSearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant

	Private m_xml As New ClsXML
	Private _ClsFunc As New ClsDivFunc

	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmVakSearch_LV

	Private pcc As New DevExpress.XtraBars.PopupControlContainer
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

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant

		ResetMandantenDropDown()
		LoadMandantenDropDownData()

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
		columns.Add(New LookUpColumnInfo("MDName", 100, "Mandant"))

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function LoadMandantenDropDownData() As Boolean
		Dim _ClsFunc As New ClsDbFunc
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

		Return Not Data Is Nothing
	End Function

	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If Not SelectedData Is Nothing Then
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName)
			ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)

		Else
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)

		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub


#End Region




#Region "Dropdown Funktionen Allgmeine"
	' Sortierung
	Private Sub CboSort_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(Me.CboSort)
	End Sub
	' Antritt per
	Private Sub Cbo_VakAntrittPer_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakAntrittPer.QueryPopUp
		ListVakAntrittPer(Me.Cbo_VakAntrittPer)
	End Sub
	' Beschäftigung
	Private Sub Cbo_VakBeschaeftigung_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakBeschaeftigung.QueryPopUp
		ListVakBeschaeftigung(Me.Cbo_VakBeschaeftigung)
	End Sub
	' Anstellungsart
	Private Sub Cbo_VakAnstellungsart_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakAnstellungsart.QueryPopUp
		LoadAnstellungData(Me.Cbo_VakAnstellungsart)
	End Sub
	' Dauer
	Private Sub Cbo_VakDauer_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakDauer.QueryPopUp
		ListVakDauer(Me.Cbo_VakDauer)
	End Sub
	' Alter
	Private Sub Cbo_VakAlter_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakAlter.QueryPopUp
		LoadAgeData(Me.Cbo_VakAlter)
	End Sub
	' Geschlecht
	Private Sub Cbo_VakGeschlecht_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakGeschlecht.QueryPopUp
		ListVakGeschlecht(Me.Cbo_VakGeschlecht)
	End Sub
	'Zivilstand
	Private Sub Cbo_VakZivilstand_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakZivilstand.QueryPopUp
		ListVakZivilstand(Me.Cbo_VakZivilstand)
	End Sub
	' Lohn
	Private Sub Cbo_VakLohn_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakLohn.QueryPopUp
		ListVakLohn(Me.Cbo_VakLohn)
	End Sub
	' Arbeitszeit
	Private Sub Cbo_VakArbeitszeit_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakArbeitszeit.QueryPopUp
		ListVakArbeitszeit(Me.Cbo_VakArbeitszeit)
	End Sub
	' Arbeitsort
	Private Sub Cbo_VakArbeitsort_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakArbeitsort.QueryPopUp
		ListVakArbeitsort(Me.Cbo_VakArbeitsort)
	End Sub

	' Gruppe
	Private Sub Cbo_VakGruppe_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakGruppe.QueryPopUp
		ListVakGruppe(Me.Cbo_VakGruppe)
	End Sub
	' Kontakt
	Private Sub Cbo_VakKontakt_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakKontakt.QueryPopUp
		LoadVakKontaktData(Me.Cbo_VakKontakt)
	End Sub
	' Status
	Private Sub Cbo_VakStatus_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakStatus.QueryPopUp
		LoadVakStateData(Me.Cbo_VakStatus)
	End Sub
	' Geschäftsstelle
	Private Sub Cbo_VakGeschaeftsstelle_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakGeschaeftsstelle.QueryPopUp
		ListVakGeschaeftsstelle(Me.Cbo_VakGeschaeftsstelle)
	End Sub
	' Berater
	Private Sub Cbo_VakBerater_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakBerater.QueryPopUp
		ListVakBerater(Me.Cbo_VakBerater)
	End Sub

	' Im Web veröffentlicht
	Private Sub Cbo_VakImWeb_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VakImWeb.QueryPopUp, cbo_JobsCH.QueryPopUp, cbo_Ostjob.QueryPopUp
		Dim cbo = CType(sender, DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		ListVakImWeb(cbo)

	End Sub

	Private Sub OnCbo_AVAM_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_AVAM.QueryPopUp
		Dim cbo = CType(sender, DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		ListVakAVAM(cbo)

	End Sub

	Private Sub OnCbo_AVAMState_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_AVAMState.QueryPopUp
		Dim cbo = CType(sender, DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		ListVakAVAMState(cbo)

	End Sub

#End Region


#Region "Click Event Sonstige"

	Private Sub LibVakBranchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibVakBranchen.Click
		Dim frmTest As New frmSearchRec("Branchen")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.GetSelectedValue()
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Me.Lst_VakBranchen.Items.Contains(strEintrag(i)) Then
					Me.Lst_VakBranchen.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

	Private Sub LibVakBezeichnung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibVakBezeichnung.Click
		Dim frmTest As New frmSearchRec("Bezeichnung")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.GetSelectedValue()
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Me.Lst_VakBezeichnung.Items.Contains(strEintrag(i)) Then
					Me.Lst_VakBezeichnung.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub

	Private Sub LibVakMSprachen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LibVakMSprachen.Click
		Dim frmTest As New frmSearchRec("Sprachen")
		Dim i As Integer = 0

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.GetSelectedValue()
		If m.ToString <> String.Empty Then
			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			For i = 0 To strEintrag.Count - 1
				If Not Me.Lst_VakMSprachen.Items.Contains(strEintrag(i)) Then
					Me.Lst_VakMSprachen.Items.Add(strEintrag(i))
				End If
			Next
		End If

		frmTest.Dispose()
	End Sub


#End Region


#Region "Allgemeine Funktionen"

	' 0 - Nicht aktiviert // 1 - Aktiviert
	Private Sub FillCboAktiviertNichtAktviert(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboAktiviertNichtAktiviert(cbo)
		End If
	End Sub
	' 0 - Nicht vollständig // 1 - Vollständig
	Private Sub FillCboVollstaendigNichtVoll(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboVollstaendigNichtVoll(cbo)
		End If
	End Sub

	' Ja - Nein
	Private Sub FillJaNein(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListJaNein(cbo)
		End If
	End Sub

	' Checkbox-Hintergrund ändern beim Fokus
	Private Sub Checkbox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is CheckBox Then
			Dim chk As CheckBox = DirectCast(sender, CheckBox)
			chk.BackColor = Color.Gray
		End If
	End Sub

	' Checkbox-Hintergrund wieder transparent beim verlassen des Fokus
	Private Sub Checkbox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is CheckBox Then
			Dim chk As CheckBox = DirectCast(sender, CheckBox)
			chk.BackColor = Color.Transparent
		End If
	End Sub




#End Region

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmVAKSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmVAKSearch_LV", True)

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				My.Settings.SortBez = Me.CboSort.Text

				My.Settings.Save()
			End If

			PrintListingThread.Abort()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmVAKSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmVakSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		For i As Integer = 0 To Me.xtabVakSearch.TabPages.Count - 1
			Me.TranslatedPage.Add(False)
		Next

		m_xml.GetChildChildBez(Me.xtabAllgemein)
		m_xml.GetChildChildBez(Me.GroupBox1)
		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)

		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_xml.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_xml.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_xml.GetSafeTranslationValue(Me.bbiExport.Caption)

		For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.xtabVakSearch.TabPages
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
	Private Sub frmVAKSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount

		SetDefaultSortValues()
		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception

		End Try

		Me.bbiExport.Enabled = False
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		Me.LblTimeValue.Visible = CBool(ClsDataDetail.UserData.UserNr = 1)
		ClsDataDetail.IsFirstTapiCall = True

		Me.xtabVakSearch.SelectedTabPage = Me.xtabAllgemein
		If ClsDataDetail.UserData.UserNr <> 1 Then
			Me.xtabVakSearch.TabPages.Remove(Me.xtabSQLAbfrage)
			Me.xtabVakSearch.TabPages.Remove(Me.xtabErweitert)
		End If

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				If strSort = String.Empty Then strSort = String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Firma"))
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

			' Tab-Stops von LinkButtons müssen programmatisch deaktiviert werden
			Me.LibVakBranchen.TabStop = False
			Me.LibVakBezeichnung.TabStop = False
			Me.LibVakMSprachen.TabStop = False

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub xtabVakSearch_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabVakSearch.SelectedPageChanged

		If Not TranslatedPage(xtabVakSearch.SelectedTabPageIndex) Then
			m_xml.GetChildChildBez(Me.xtabVakSearch.SelectedTabPage)
			TranslatedPage(xtabVakSearch.SelectedTabPageIndex) = True
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
			MsgBox(m_xml.GetSafeTranslationValue("Keine Suche wurde gestartet!."), MsgBoxStyle.Exclamation, "GetESData4Print")
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		If bForDesign Then sSql = Replace(UCase(sSql), "SELECT V.ID", "SELECT TOP 10 V.ID")

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

		Try
			Conn.Open()

			Dim rVAKrec As SqlDataReader = cmd.ExecuteReader                  ' 
			Try
				If Not rVAKrec.HasRows Then
					cmd.Dispose()
					rVAKrec.Close()

					MessageBox.Show(m_xml.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."),
													"GetVAKData", MessageBoxButtons.OK, MessageBoxIcon.Information)
				End If

			Catch ex As Exception
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetVAKData")

			End Try

			rVAKrec.Read()
			If rVAKrec.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo

				PrintListingThread = New Thread(AddressOf StartPrinting)
				PrintListingThread.Name = "PrintingVakListing"
				PrintListingThread.SetApartmentState(ApartmentState.STA)
				PrintListingThread.Start()

			End If
			rVAKrec.Close()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetVAKData4Print")

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try
	End Sub

	Sub StartPrinting()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLVakSearchPrintSetting With {.SelectedMDNr = lueMandant.EditValue, .DbConnString2Open = ClsDataDetail.MDData.MDDbConn,
																																									 .SQL2Open = Me.SQL4Print,
																																									 .JobNr2Print = Me.PrintJobNr}
		Dim obj As New SPS.Listing.Print.Utility.VakSearchListing.ClsPrintVakSearchList(_Setting)
		obj.PrintVakSearchList_1(Me.bPrintAsDesign, ClsDataDetail.GetSortBez,
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

		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		Try
			Me.txt_SQLQuery.Text = String.Empty
			If Not Me.txtVakNr_2.Visible Then Me.txtVakNr_2.Text = Me.txtVakNr_1.Text
			If Not Me.txtVakKDNr_2.Visible Then Me.txtVakKDNr_2.Text = Me.txtVakKDNr_1.Text

			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded("frmVakSearch_LV", True)
			If ClsDataDetail.GetLVSortBez <> String.Empty Then Me.CboSort.Text = ClsDataDetail.GetLVSortBez

			' Die Query-String aufbauen...
			GetMyQueryString()

			Stopwatch.Reset()
			Stopwatch.Start()

			' Daten auflisten...
			If Not FormIsLoaded("frmVakSearch_LV", True) Then
				frmMyLV = New frmVakSearch_LV(Me.txt_SQLQuery.Text)
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

			' Buttons Drucken und Export aktivieren/deaktivieren
			If frmMyLV.RecCount > 0 Then
				CreatePrintPopupMenu()

				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

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
			sSqlQuerySelect = _ClsDb.GetStartSQLString(Me)


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


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 705)
		Dim liMnu As New List(Of String) From {"Liste drucken#mnuPrint",
																					 If(bShowDesign, "Entwurfanssicht#mnuDesign", "")}
		If Not IsUserActionAllowed(0, 704) Then Exit Sub
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

					If myValue(1).ToString.ToLower.Contains("mnuDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim _clsdb As New ClsDbFunc

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		Select Case e.Item.Name.ToUpper
			Case "mnuPrint".ToUpper
				_clsdb.GetJobNr4Print()
				GetVAKData4Print(False, False, ClsDataDetail.GetModulToPrint())


			Case "mnuDesign".ToUpper
				_clsdb.GetJobNr4Print()
				GetVAKData4Print(True, False, ClsDataDetail.GetModulToPrint())


			Case Else
				Exit Sub

		End Select

	End Sub





	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmVakSearch_LV", True)
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()

		Me.CboSort.Text = strText   ' Letzte Sortierung wieder herstellen
		Me.txt_SQLQuery.Text = m_xml.GetSafeTranslationValue("Wurde geleert" & "...")

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub
	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabVakSearch").Controls
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
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try

			If con.Name.ToLower = "cbo_eskst1" Then
				Trace.WriteLine(String.Format("{0}: {1} | {2}", con.Name, con.GetType, con.Text))

			Else
				Trace.WriteLine(String.Format("{0}: {1} | {2}", con.Name, con.GetType, con.Text))
			End If

			If con.Enabled Then
				Trace.WriteLine(con.Name)
				If TypeOf (con) Is TextBox Then
					Dim tb As TextBox = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
					Dim de As DevExpress.XtraEditors.DateEdit = con
					de.EditValue = Nothing

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.MemoEdit Then
					Dim tb As DevExpress.XtraEditors.MemoEdit = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
					Dim tb As DevExpress.XtraEditors.TextEdit = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is System.Windows.Forms.ComboBox Then
					Dim cbo As System.Windows.Forms.ComboBox = con
					cbo.Text = String.Empty
					cbo.SelectedIndex = -1

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
					Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
					cbo.Text = String.Empty
					cbo.Properties.Items.Clear()

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
					Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = con
					cbo.Text = String.Empty
					cbo.Properties.Items.Clear()

				ElseIf TypeOf (con) Is CheckBox Then
					Dim cbo As CheckBox = con
					cbo.CheckState = CheckState.Unchecked

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
					Dim cbo As DevExpress.XtraEditors.CheckEdit = con
					cbo.CheckState = CheckState.Unchecked

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl Then
					Dim lst As DevExpress.XtraEditors.ListBoxControl = con
					lst.Items.Clear()

				ElseIf TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl Then
					Dim lst As DevExpress.XtraEditors.ListBoxControl = con
					lst.Items.Clear()

				Else
					For Each childCon As Control In con.Controls
						ResetControl(childCon)
					Next

				End If
			End If

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

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

	Private Sub txtVakNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtVakNr_1.ButtonClick
		Dim frmTest As New frmSearchRec("VakNr")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()

		frmTest.MdiParent = Me.MdiParent

		m = frmTest.GetSelectedValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtVakNr_1.Text = m

	End Sub

	Private Sub txtVakNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtVakNr_2.ButtonClick
		Dim frmTest As New frmSearchRec("VakNr")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()

		frmTest.MdiParent = Me.MdiParent

		m = frmTest.GetSelectedValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtVakNr_2.Text = m

	End Sub

	Private Sub txtVakKDNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtVakKDNr_1.ButtonClick
		Dim frmTest As New frmSearchRec("KDNr")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()

		frmTest.MdiParent = Me.MdiParent

		m = frmTest.GetSelectedValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtVakKDNr_1.Text = m
	End Sub

	Private Sub txtVakKDNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtVakKDNr_2.ButtonClick
		Dim frmTest As New frmSearchRec("KDNr")
		Dim strModulName As String = String.Empty

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()

		frmTest.MdiParent = Me.MdiParent

		m = frmTest.GetSelectedValue()
		m = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
		Me.txtVakKDNr_2.Text = m

	End Sub






	''' <summary>
	''' KeyPressEvent der Controls auffangen und verarbeiten. (Enter --> Tab)
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtVakKDNr_2.KeyPress, txtVakKDNr_1.KeyPress, txtVakNr_1.KeyPress, txtVakNr_2.KeyPress, Cbo_VakDauer.KeyPress, CboSort.KeyPress, Cbo_VakBeschaeftigung.KeyPress, Cbo_VakAntrittPer.KeyPress, Cbo_VakAnstellungsart.KeyPress, Cbo_VakAlter.KeyPress, Cbo_VakGeschlecht.KeyPress, Cbo_VakZivilstand.KeyPress, Cbo_VakLohn.KeyPress, Cbo_VakArbeitszeit.KeyPress, Cbo_VakKontakt.KeyPress, Cbo_VakStatus.KeyPress, Cbo_VakGeschaeftsstelle.KeyPress, Cbo_VakBerater.KeyPress, Lst_VakMSprachen.KeyPress, Lst_VakBranchen.KeyPress, Lst_VakBezeichnung.KeyPress         ' System.EventArgs)

		Try
			If e.KeyChar = Chr(13) Then ' Enter
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	''' <summary>
	''' Allgemeiner Delete-Event für die Listbox und Textbox und
	''' allgmeiner KeyEvent für F4.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyUpEvent(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtVakNr_1.KeyUp, txtVakKDNr_2.KeyUp, txtVakKDNr_1.KeyUp, txtVakNr_2.KeyUp, Cbo_VakArbeitsort.KeyUp, Lst_VakMSprachen.KeyUp, Lst_VakBranchen.KeyUp, Lst_VakBezeichnung.KeyUp
		' LISTBOX

		If TypeOf (sender) Is DevExpress.XtraEditors.ListBoxControl Then

			Dim _lst As DevExpress.XtraEditors.ListBoxControl = DirectCast(sender, DevExpress.XtraEditors.ListBoxControl)
			If e.KeyCode = Keys.Delete And _lst.SelectedIndex > -1 Then
				For i As Integer = 0 To _lst.SelectedIndices.Count - 1
					' Wenn der erste selektierte Inidices gelöscht wird,
					' rückt der nächste automatisch nach bis keine mehr
					' vorhanden sind. Darum immer 0. 19.08.2009 A.Ragusa
					_lst.Items.RemoveAt(_lst.SelectedIndices(0))
				Next

			ElseIf e.KeyCode = Keys.F4 Then

				Select Case _lst.Tag
					Case LibVakBranchen.Name
						LibVakBranchen_Click(sender, New System.EventArgs)
					Case LibVakBezeichnung.Name
						LibVakBezeichnung_Click(sender, New System.EventArgs)
					Case LibVakMSprachen.Name
						LibVakMSprachen_Click(sender, New System.EventArgs)
				End Select

			End If
			' TEXTBOX

		ElseIf TypeOf (sender) Is DevExpress.XtraEditors.TextEdit Then

			Dim _txt As DevExpress.XtraEditors.TextEdit = DirectCast(sender, DevExpress.XtraEditors.TextEdit)

			If e.KeyCode = Keys.F4 Then
				Select Case _txt.Tag
					Case txtVakNr_1.Name
						Me.txtVakNr_1_ButtonClick(sender, New System.EventArgs)
					Case txtVakNr_2.Name
						Me.txtVakNr_2_ButtonClick(sender, New System.EventArgs)

					Case txtVakKDNr_1.Name
						Me.txtVakKDNr_1_ButtonClick(sender, New System.EventArgs)
					Case txtVakKDNr_2.Name
						Me.txtVakKDNr_2_ButtonClick(sender, New System.EventArgs)

				End Select

			End If
		End If

	End Sub


	Private Sub frmVAKSearch_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.txtVakNr_1.Focus()
	End Sub

	Private Sub frmVAKSearch_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Me.Focus()
	End Sub

	Private Sub btnPrint_DropDownOpening(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_xml.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub

	Private Sub btnExport_DropDownOpening(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_xml.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub



	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtVakNr_2.Visible = Me.SwitchButton1.Value
		Me.txtVakNr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txtVakKDNr_2.Visible = Me.SwitchButton2.Value
		Me.txtVakKDNr_2.Text = String.Empty
	End Sub


	Private Sub LibVakMSprachen_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LibVakMSprachen.LinkClicked

	End Sub

	Private Sub Cbo_VakImWeb_DropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_VakImWeb.QueryPopUp, cbo_Ostjob.QueryPopUp, cbo_JobsCH.QueryPopUp, Cbo_AVAM.QueryPopUp

	End Sub
End Class


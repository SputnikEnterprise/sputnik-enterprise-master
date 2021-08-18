
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.LookAndFeel

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Public Class frmZESearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility
	Private m_Utility_SP As SPProgUtility.MainUtilities.Utilities

	''' <summary>
	''' The mandant.
	''' </summary>
	''' <remarks></remarks>
	Private m_Mandant As Mandant

	Private m_path As ClsProgPath
	Private _ClsFunc As New ClsDivFunc
	Private strValueSeprator As String = "#@"
	Private Stopwatch As Stopwatch = New Stopwatch()
	Private Delegate Function AsyncDelegate(ByVal i As Integer) As Integer

	Public Shared frmMyLV As frmZESearch_LV

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private Property TranslatedPage As New List(Of Boolean)


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		ClsDataDetail.m_InitialData = m_InitializationData
		ClsDataDetail.m_Translate = m_Translate

		m_UtilityUI = New UtilityUI
		m_Utility = New Utility
		m_Utility_SP = New SPProgUtility.MainUtilities.Utilities

		' Mandantendaten
		m_Mandant = New Mandant
		m_path = New ClsProgPath


		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_Mandant = New Mandant

		ResetMandantenDropDown()
		LoadMandantenDropDown()

	End Sub

#End Region


	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property


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
		Dim _ClsFunc As New ClsDbFunc(m_InitializationData)
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

		'    Return Not Data Is Nothing
	End Sub

	' Mandantendaten...
	Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)

			m_InitializationData = ChangeMandantData
			ClsDataDetail.m_InitialData = m_InitializationData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

		'Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		'If Not SelectedData Is Nothing Then
		'	'Me.DbConnstr = SelectedData.MDConnStr
		'	ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
		'	m_InitializationData.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, m_InitializationData.UserData.UserLName, m_InitializationData.UserData.UserFName)

		'Else
		'	ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
		'	m_InitializationData.UserData = ClsDataDetail.LogededUSData(0, m_InitializationData.UserData.UserNr)

		'	'Me.DbConnstr = ClsDataDetail.SelectedMDData(0).MDDbConn

		'End If

		'Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		'Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		'Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		'Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub


#End Region




#Region "Dropdown Funktionen 1. Seite..."
	Private Sub CboSort_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(Me.CboSort)
	End Sub

	Private Sub Cbo_Berater_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		If Me.Cbo_Berater.Properties.Items.Count = 0 Then ListBerater(Me.Cbo_Berater)
	End Sub

	Private Sub Cbo_KST1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST1.QueryPopUp
		If Me.Cbo_KST1.Properties.Items.Count = 0 Then ListREKst1(Me.Cbo_KST1, Me.Cbo_Filiale.Text)
	End Sub

	Private Sub Cbo_KST2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST2.QueryPopUp
		If Me.Cbo_KST2.Properties.Items.Count = 0 Then ListREKst2(Me.Cbo_KST2)
	End Sub

	Private Sub Cbo_BuKonto_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_BuKonto.QueryPopUp
		ListBuKonto(Me.Cbo_BuKonto, Me.Cbo_Filiale.Text)
	End Sub

	Private Sub Cbo_MwSt_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MwSt.QueryPopUp
		If Me.Cbo_MwSt.Properties.Items.Count = 0 Then
			ListMwSt(Me.Cbo_MwSt)
			'Me.Cbo_MwSt.Items.Add("")
			'Me.Cbo_MwSt.Items.Add(New ComboBoxItem("frei", "0"))
			'Me.Cbo_MwSt.Items.Add(New ComboBoxItem("pflichtig", "1"))
		End If
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		If Me.Cbo_Filiale.Properties.Items.Count = 0 Then ListKDFiliale(Me.Cbo_Filiale)
	End Sub

	Private Sub Cbo_REArt_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_REArt.QueryPopUp
		If Me.Cbo_REArt.Properties.Items.Count = 0 Then ListREArt(Me.Cbo_REArt)
	End Sub


#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmZESearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmZESearch_LV", True)
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				My.Settings.SortBez = Me.CboSort.Text

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub StartTranslation()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader2.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabErweitert.Text = m_Translate.GetSafeTranslationValue(Me.xtabErweitert.Text)
		Me.xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblsortierung.Text = m_Translate.GetSafeTranslationValue(Me.lblsortierung.Text)

		Me.lblNummer.Text = m_Translate.GetSafeTranslationValue(Me.lblNummer.Text)
		Me.lblRENr.Text = m_Translate.GetSafeTranslationValue(Me.lblRENr.Text)
		Me.lblkundennr.Text = m_Translate.GetSafeTranslationValue(Me.lblkundennr.Text)
		Me.lblValutadatum.Text = m_Translate.GetSafeTranslationValue(Me.lblValutadatum.Text)
		Me.lblBuchungsdatum.Text = m_Translate.GetSafeTranslationValue(Me.lblBuchungsdatum.Text)

		Me.lblbuchungsbetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblbuchungsbetrag.Text)
		Me.lblBuchungskonto.Text = m_Translate.GetSafeTranslationValue(Me.lblBuchungskonto.Text)
		Me.lblMwSt.Text = m_Translate.GetSafeTranslationValue(Me.lblMwSt.Text)

		Me.lblBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblBerater.Text)
		Me.lblfiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblfiliale.Text)

		Me.lblAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblAbfrage.Text)
		Me.lblderzeitigeabfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblderzeitigeabfrage.Text)

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmZESearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetDefaultSortValues()

		Dim Time_1 As Double = System.Environment.TickCount
		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

		End Try

		Try
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
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

			' Berechtigung für den Export
			ClsDbFunc.SetJobNr4Print(Me)
			If Not IsUserAllowed4DocExport(ClsDataDetail.GetModulToPrint) Then
				Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
			End If
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			Me.LblTimeValue.Visible = CBool(CInt(m_InitializationData.UserData.UserNr) = 1)

			ClsDataDetail.IsFirstTapiCall = True

			Me.xtabZESearch.SelectedTabPage = Me.xtabAllgemein

			' Folgende Controls ausblenden, falls nicht Benutzer 1 (Administrator) eingeloggt
			If m_InitializationData.UserData.UserNr <> 1 Then
				Me.xtabZESearch.TabPages.Remove(Me.xtabSQLAbfrage)
				Me.xtabZESearch.TabPages.Remove(Me.xtabErweitert)
			End If


		Catch ex As Exception
			MessageBox.Show(ex.Message, "frmOPSearch_Load", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				If strSort = String.Empty Then strSort = String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsempfänger"))
				ListSort(Me.CboSort)
				Dim aSortlist As String() = strSort.Split(CChar(","))

				For Each bez As String In aSortlist
					CboSort.Properties.Items(CInt(Val(bez))).CheckState = CheckState.Checked
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.Message))

			End Try

			Try
				Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
				m_Logger.LogDebug(String.Format("LogedUSNr: {0} >>> SelectedMDNr: {1}", m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr))
				Dim showMDSelection As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)
				Me.lueMandant.Visible = showMDSelection
				Me.lblMDName.Visible = showMDSelection

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
				Me.lueMandant.Visible = False
				Me.lblMDName.Visible = False
			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.Message))

		End Try

	End Sub


	'''' <summary>
	'''' Daten fürs Drucken bereit stellen.
	'''' </summary>
	'''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	'''' <param name="bForExport">ob die Liste für Export ist.</param>
	'''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	'''' <remarks></remarks>
	'Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'	Dim iKDNr As Integer = 0
	'	Dim iKDZNr As Integer = 0
	'	Dim bResult As Boolean = True
	'	Dim bWithKD As Boolean = True

	'	Dim sSql As String = Me.txt_SQLQuery.Text
	'	If sSql = String.Empty Then
	'		MsgBox(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!."), MsgBoxStyle.Exclamation, "GetData4Print")
	'		Exit Sub
	'	End If

	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'	Dim cmd As System.Data.SqlClient.SqlCommand

	'	Try
	'		Conn.Open()
	'		'Daten sind bereits auf der Datenbank

	'		sSql = String.Format("{0}", Me.txt_SQLQuery.Text) ' ClsDataDetail.SPTabNamenZEListe)
	'		' Sort-Klausel wieder hinzufügen
	'		Dim _clsDB As New ClsDbFunc(m_InitializationData)
	'		'sSql += _clsDB.GetSortString(Me)

	'		If bForDesign Then sSql = Replace(sSql, "SELECT ", "SELECT TOP 10 ")

	'		cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

	'		Dim rFoundedRec2Print As SqlDataReader = cmd.ExecuteReader
	'		Try
	'			If Not rFoundedRec2Print.HasRows Then
	'				cmd.Dispose()
	'				rFoundedRec2Print.Close()

	'				MessageBox.Show(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."),
	'												"GetData4Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'				Exit Sub
	'			End If

	'		Catch ex As NoNullAllowedException

	'		Catch ex As Exception
	'			MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetData4Print")

	'		End Try

	'		rFoundedRec2Print.Read()
	'		If rFoundedRec2Print.HasRows Then
	'			Me.SQL4Print = sSql
	'			Me.bPrintAsDesign = bForDesign
	'			Me.PrintJobNr = strJobInfo

	'			StartPrinting()
	'			'PrintListingThread = New Thread(AddressOf StartPrinting)
	'			'PrintListingThread.Name = "PrintingVakListing"
	'			'PrintListingThread.SetApartmentState(ApartmentState.STA)
	'			'PrintListingThread.Start()

	'		End If
	'		rFoundedRec2Print.Close()

	'	Catch ex As Exception
	'		MsgBox(ex.Message, MsgBoxStyle.Critical, "GetData4Print")

	'	Finally
	'		Conn.Close()

	'	End Try

	'End Sub


#Region "Funktionen zur Menüaufbau..."

	Sub ResetClsValue()
		Try
			ClsDataDetail.strKDData = String.Empty
			ClsDataDetail.strButtonValue = String.Empty
			ClsDataDetail.Get4What = String.Empty
			ClsDataDetail.GetSortBez = String.Empty
			ClsDataDetail.GetFilterBez = String.Empty
			ClsDataDetail.GetFilterBez2 = String.Empty
			ClsDataDetail.GetFilterBez3 = String.Empty
			ClsDataDetail.GetFilterBez4 = String.Empty
			ClsDataDetail.GetFilterBezArray.Clear()

			ClsDataDetail.ListBez = String.Empty

			ClsDataDetail.GetTotalBetrag = 0
			ClsDataDetail.GetTotalOpenBetrag4Date = 0
			ClsDataDetail.LLDocName = String.Empty
		Catch ex As Exception
			MessageBox.Show(ex.Message, "ResetClsValue", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try


	End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)
		Dim strArtQuery As String = String.Empty

		ResetClsValue()

		'    BackgroundWorker1.WorkerSupportsCancellation = True

		Try
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

			_ClsFunc.GetRENr = 0
			_ClsFunc.GetKDNr = 0
			Me.txt_SQLQuery.Text = String.Empty
			Me.LblTimeValue.Text = String.Empty
			If Not Me.txtZENr2.Visible Then Me.txtZENr2.Text = Me.txtZENr1.Text
			If Not Me.txtRENr2.Visible Then Me.txtRENr2.Text = Me.txtRENr1.Text
			If Not Me.txtKDNr2.Visible Then Me.txtKDNr2.Text = Me.txtKDNr1.Text

			'BackgroundWorker1.RunWorkerAsync(5)    ' für Multithreading

			FormIsLoaded("frmZESearch_LV", True)

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			' Eingabe-Kontrolle
			If Not Kontrolle() Then
				Return
			End If

			' Die Query-String aufbauen...
			GetMyQueryString()

			Stopwatch.Reset()
			Stopwatch.Start()

			' Daten auflisten...
			If Not FormIsLoaded("frmZESearch_LV", True) Then
				frmMyLV = New frmZESearch_LV(Me.txt_SQLQuery.Text)

				frmMyLV.Show()
				Me.Select()
			End If

			' Die Buttons Drucken und Export aktivieren/deaktivieren

			If Me.deBuchungDate_2.Text = "31.12.3099" Then
				Me.deBuchungDate_2.Text = String.Empty
				Me.deBuchungDate_2.ForeColor = Color.Black
			End If

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				'CreatePrintPopupMenu()
				CreateExportPopupMenu()

				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

			Else
				Me.bbiPrint.Enabled = False
				Me.bbiExport.Enabled = False

			End If

			Me.LblTimeValue.Text = String.Format(m_Translate.GetSafeTranslationValue("Datenauflistung für {0} Einträge: in {1} ms"),
																					 frmMyLV.RecCount,
																					 Stopwatch.ElapsedMilliseconds().ToString)
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																				 frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																							frmMyLV.RecCount)

		Catch ex As Exception
			MessageBox.Show(ex.Message, "btnSearch_Click")

		Finally

		End Try

	End Sub



	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

		Dim ShowDesign As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 617, m_InitializationData.MDData.MDNr)
		ShowDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso ShowDesign

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False
		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint

		Me.SQL4Print = txt_SQLQuery.Text
		Me.bPrintAsDesign = ShowDesign
		Me.PrintJobNr = strModultoPrint

		StartPrinting()

	End Sub

	Sub StartPrinting()
		Dim ShouldDivideAmount As Boolean = Not String.IsNullOrWhiteSpace(Cbo_Berater.EditValue)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLZESearchPrintSetting With {.SelectedMDNr = lueMandant.EditValue,
			.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
			.frmhwnd = GetHwnd,
			.SQL2Open = Me.SQL4Print,
			.JobNr2Print = Me.PrintJobNr,
			.CalculateVerfallTagWithCreatedOn = chkVerfalltageFromCreatedOn.Checked,
			.ShouldDivideAmount = ShouldDivideAmount}

		Dim obj As New SPS.Listing.Print.Utility.ZESearchListing.ClsPrintZESearchList(_Setting)
		ClsDataDetail.GetFilterBez = String.Empty
		ClsDataDetail.GetFilterBez2 = String.Empty
		ClsDataDetail.GetFilterBez3 = String.Empty
		ClsDataDetail.GetFilterBez4 = String.Empty
		For i As Integer = 0 To ClsDataDetail.GetFilterBezArray.Count - 1
			If i <= 5 Then
				ClsDataDetail.GetFilterBez &= ClsDataDetail.GetFilterBezArray(i).ToString & vbNewLine
			Else
				ClsDataDetail.GetFilterBez2 &= ClsDataDetail.GetFilterBezArray(i).ToString & vbNewLine

			End If
		Next
		obj.PrintZESearchList_1(Me.bPrintAsDesign, ClsDataDetail.GetSortBez, ClsDataDetail.GetTotalOpenBetrag4Date,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl

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
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
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
		Dim modul = m_Mandant.ModulLicenseKeys(m_InitializationData.MDData.MDNr, Now.Year, String.Empty)
		Dim sql As String = txt_SQLQuery.EditValue

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("Abacus")
				Call ExportDataToAbacus(Me.txt_SQLQuery.Text)

			Case UCase("Cresus")
				Call ExportDataToCresus(Me.txt_SQLQuery.Text)

			Case UCase("Simba")
				Call ExportDataToSimba(Me.txt_SQLQuery.Text)

			Case UCase("TXT"), UCase("CSV")
				Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn, .SQL2Open = txt_SQLQuery.Text, .ModulName = "ZESearch"}
				Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
				obj.ExportCSVFromZEForKDMailing(sql)

				'Call RunKommaModul(Me.txt_SQLQuery.Text)

			Case UCase("Swifac")
				Call ExportDataForSWIFAC(Me.txt_SQLQuery.Text)

			Case UCase("Comatic")
				Dim frmTest As New frmComatic(m_InitializationData)
				frmTest.strTempSQL = Me.txt_SQLQuery.Text

				frmTest.Show()
				frmTest.BringToFront()

		End Select

	End Sub


	Function Kontrolle() As Boolean
		Dim check As Boolean = True
		Dim meldung As String = ""

		If Not check Then
			MessageBox.Show(meldung, "Keine Suche möglich", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
		Return check
	End Function

	Function GetMyQueryString() As Boolean

		Try
			Dim sSql As String = String.Empty
			Dim strSort As String = String.Empty
			Dim strAndString As String = String.Empty
			ClsDataDetail.GetFilterBezArray.Clear()
			Dim cv As ComboValue

			Dim _ClsDb As New ClsDbFunc(m_InitializationData)
			If Me.txt_IndSQLQuery.Text = String.Empty Then

				' Die Tabellennamen für das Speichern der Tabellen auf der DB
				ClsDataDetail.SPTabNamenZEListe = String.Format("[_ZEListe_{0}]", m_InitializationData.UserData.UserGuid)

				' Start-Query ========================================================================================================
				' ====================================================================================================================
				sSql = String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", ClsDataDetail.SPTabNamenZEListe)
				sSql += "SELECT ZE.*, RE.R_Name1, RE.R_Name2, RE.R_Name3, RE.R_ZHD, RE.R_Abteilung, "
				sSql += "RE.R_Postfach, RE.R_Strasse, RE.R_PLZ, RE.R_Ort, RE.R_Land, RE.FKHaben0 As REFKHaben0, RE.FKHaben1 As REFKHaben1, "
				sSql += "RE.Zahlkond, RE.Faellig, RE.MwStProz, "
				sSql += "RE.KST, RE.REKst1, RE.REKst2, RE.Art as REArt, RE.CreatedOn AS InvoiceCreatedOn, "
				'' employeeadvisor
				sSql += "ISNULL(( SELECT TOP 1 " &
													"(Nachname + ', '+Vorname) AS USName " &
				"FROM Benutzer " &
									 "WHERE  USNR = ( SELECT TOP 1 " &
				"USNR " &
				"FROM Benutzer " &
																	 "WHERE  KST LIKE SUBSTRING(RE.Kst, 0, " &
																														 "CHARINDEX('/', " &
																																"RE.Kst)) " &
																					"OR KST LIKE RE.Kst " &
																 ") " &
								 "), '') AS employeeadvisor , "

				'' customeradvisor
				sSql += "ISNULL(( SELECT TOP 1 " &
												 "(Nachname + ', '+Vorname) AS USName " &
			 "FROM Benutzer " &
									 "WHERE  USNR = ( SELECT TOP 1 " &
				"USNR " &
				"FROM Benutzer " &
																	 "WHERE  KST LIKE SUBSTRING(RE.Kst, " &
																														 "CHARINDEX('/', " &
																																"RE.Kst) + 1, " &
																														 "LEN(RE.Kst)) " &
																					"OR KST LIKE RE.Kst " &
																 ") " &
								 "), '') AS customeradvisor, "



				sSql += "Kunden.KL_RefNr As Kredit_RefNr, Kunden.KreditLimite "
				sSql += String.Format(" INTO {0} ", ClsDataDetail.SPTabNamenZEListe)
				sSql += "From ZE "
				sSql += "Left Join RE On RE.RENr = ZE.RENr "
				sSql += "Left Join Kunden On ZE.KDNr = Kunden.KDNr "
				sSql += "Where ZE.ZENr > 0 "

				' Mandantennummer -------------------------------------------------------------------------------------------------------
				If m_InitializationData.MDData.MultiMD = 1 Then
					sSql += String.Format("And RE.MDNr = {0} ", m_InitializationData.MDData.MDNr)
				End If

				' 1. Seite === Allgemeine ============================================================================================
				' ====================================================================================================================
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString

				' Zahlungs-Nr --------------------------------------------------------------------------------------------------
				Dim zenr1 As String = Me.txtZENr1.Text.Replace("'", "").Replace("*", "%").Trim
				Dim zenr2 As String = Me.txtZENr2.Text.Replace("'", "").Replace("*", "%").Trim

				If zenr1 = "" And zenr2 = "" Then
					'do nothing

				ElseIf zenr1.Contains("%") Then
					' Suche ZENr mit Sonderzeichen
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}Zahlungseingänge mit ZENr ({1}){2}"),
																														strAndString, zenr1, vbLf))
					sSql += String.Format("{0}ZE.ZENr Like '{1}'", strAndString, zenr1)

				ElseIf InStr(zenr1, ",") > 0 Then
					' Suche ZENr mit Komma getrennt
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit ZENr ({0}){1}"),
																														zenr1, vbLf))
					sSql += String.Format("{0}ZE.ZENr In (", strAndString)
					For Each esnr In zenr1.Split(CChar(","))
						sSql += String.Format("'{0}',", esnr)
					Next
					sSql = sSql.Substring(0, sSql.Length - 1)
					sSql += ")"

				ElseIf zenr1 = zenr2 Then
					' Suche genau eine ZENr
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit ZENr = {0}{1}"),
																														zenr1, vbLf))
					sSql += String.Format("{0}ZE.ZENr = {1}", strAndString, zenr1)

				ElseIf zenr1 <> "" And zenr2 = "" Then
					' Suche ab ZENr1 
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge ab ZENr = {0}{1}"),
																														zenr1, vbLf))
					sSql += String.Format("{0}ZE.ZENr >= {1}", strAndString, zenr1)

				ElseIf zenr1 = "" And zenr2 <> "" Then
					' Suche bis ZENr2
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge bis ZENr = {0}{1}"),
																														zenr2, vbLf))
					sSql += String.Format("{0}ZE.ZENr <= {1}", strAndString, zenr2)

				Else
					' Suche zwischen erste und zweite ZENr
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge zwischen ZENr {0} und {1}{2}"),
																														zenr1, zenr2, vbLf))
					sSql += String.Format("{0}ZE.ZENr Between '{1}' And '{2}'", strAndString, zenr1, zenr2)
				End If

				' Rechnungs-Nr --------------------------------------------------------------------------------------------------
				Dim renr1 As String = Me.txtRENr1.Text.Replace("'", "").Replace("*", "%").Trim
				Dim renr2 As String = Me.txtRENr2.Text.Replace("'", "").Replace("*", "%").Trim

				If renr1 = "" And renr2 = "" Then
					'do nothing

				ElseIf renr1.Contains("%") Then
					' Suche RENr mit Sonderzeichen
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}Zahlungseingänge mit RENr ({1}){2}"),
																														strAndString, renr1, vbLf))
					sSql += String.Format("{0}RE.RENr Like '{1}'", strAndString, renr1)

				ElseIf InStr(renr1, ",") > 0 Then
					' Suche RENr mit Komma getrennt
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit RENr ({0}){1}"),
																														renr1, vbLf))
					sSql += String.Format("{0}RE.RENr In (", strAndString)
					For Each renr In renr1.Split(CChar(","))
						sSql += String.Format("'{0}',", renr)
					Next
					sSql = sSql.Substring(0, sSql.Length - 1)
					sSql += ")"

				ElseIf renr1 = renr2 Then
					' Suche genau eine RENr
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit RENr = {0}{1}"),
																														renr1, vbLf))
					sSql += String.Format("{0}RE.RENr = {1}", strAndString, renr1)

				ElseIf renr1 <> "" And renr2 = "" Then
					' Suche ab RENr1 
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge ab RENr = {0}{1}"),
																														renr1, vbLf))
					sSql += String.Format("{0}RE.RENr >= {1}", strAndString, renr1)

				ElseIf renr1 = "" And renr2 <> "" Then
					' Suche bis RENr2
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge bis RENr = {0}{1}"),
																														renr2, vbLf))
					sSql += String.Format("{0}RE.RENr <= {1}", strAndString, renr2)

				Else
					' Suche zwischen erste und zweite RENr
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge zwischen RENr {0} und {1}{2}"),
																														renr1, renr2, vbLf))
					sSql += String.Format("{0}RE.RENr Between '{1}' And '{2}'", strAndString, renr1, renr2)
				End If

				' Kunden-Nr --------------------------------------------------------------------------------------------------
				Dim kdnr1 As String = Me.txtKDNr1.Text.Replace("'", "").Replace("*", "%").Trim
				Dim kdnr2 As String = Me.txtKDNr2.Text.Replace("'", "").Replace("*", "%").Trim

				If kdnr1 = "" And kdnr2 = "" Then
					'do nothing

				ElseIf kdnr1.Contains("%") Then
					' Suche KDNr mit Sonderzeichen
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}Zahlungseingänge mit KDNr ({1}){2}"),
																														strAndString, kdnr1, vbLf))
					sSql += String.Format("{0}ZE.KDNr Like '{1}'", strAndString, kdnr1)

				ElseIf InStr(kdnr1, ",") > 0 Then
					' Suche KDNr mit Komma getrennt
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit KDNr ({0}){1}"),
																														kdnr1, vbLf))
					sSql += String.Format("{0}ZE.KDNr In (", strAndString)
					For Each kdnr In kdnr1.Split(CChar(","))
						sSql += String.Format("'{0}',", kdnr)
					Next
					sSql = sSql.Substring(0, sSql.Length - 1)
					sSql += ")"

				ElseIf kdnr1 = kdnr2 Then
					' Suche genau eine KDNr
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit KDNr = {0}{1}"),
																														kdnr1, vbLf))
					sSql += String.Format("{0}ZE.KDNr = {1}", strAndString, kdnr1)

				ElseIf kdnr1 <> "" And kdnr2 = "" Then
					' Suche ab KDNr1 
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge ab KDNr = {0}{1}"),
																														kdnr1, vbLf))
					sSql += String.Format("{0}ZE.KDNr >= {1}", strAndString, kdnr1)

				ElseIf kdnr1 = "" And kdnr2 <> "" Then
					' Suche bis KDNr2
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge bis KDNr = {0}{1}"),
																														kdnr2, vbLf))
					sSql += String.Format("{0}ZE.KDNr <= {1}", strAndString, kdnr2)

				Else
					' Suche zwischen erste und zweite KDNr
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge zwischen KDNr {0} und {1}{2}"),
																														kdnr1, kdnr2, vbLf))
					sSql += String.Format("{0}ZE.KDNr Between '{1}' And '{2}'", strAndString, kdnr1, kdnr2)
				End If

				' Valutadatum ab/bis -----------------------------------------------------------------------------------------------
				If Me.deValutaDate_1.Text <> "" Or Me.deValutaDate_2.Text <> "" Then
					strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
					Dim valutaDtab As String = ""
					Dim valutaDtbis As String = ""
					If IsDate(Me.deValutaDate_1.Text) Then
						valutaDtab = Date.Parse(Me.deValutaDate_1.Text).ToString("d")
					End If
					If IsDate(Me.deValutaDate_2.Text) Then
						valutaDtbis = Date.Parse(Me.deValutaDate_2.Text).ToString("d")
					End If

					' Suche zwischen zwei Datum
					If valutaDtab.Length > 0 And valutaDtbis.Length > 0 Then
						sSql += String.Format("{0}ZE.V_Date Between '{1} 00:00' And '{2} 23:59'",
																	strAndString, valutaDtab, valutaDtbis)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit Valutadatum zwischen {0} und {1}{2}"), valutaDtab, valutaDtbis, vbLf))
						' Suche ab erstes Datum
					ElseIf valutaDtab.Length > 0 Then
						sSql += String.Format("{0}ZE.V_Date >= '{1}'", strAndString, valutaDtab)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge ab Valutadatum {0}{1}"),
																															valutaDtab, vbLf))
						' Suche bis zweites Datum
					ElseIf valutaDtbis.Length > 0 Then
						sSql += String.Format("{0}ZE.V_Date <= '{1} 23:59'", strAndString, valutaDtbis)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge bis Valutadatum {0}{1}"),
																															valutaDtbis, vbLf))
					End If
				End If

				' Buchungsdatum ab/bis -----------------------------------------------------------------------------------------------
				If Me.deBuchungDate_1.Text <> "" Or Me.deBuchungDate_2.Text <> "" Then
					strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
					Dim buchngDtab As String = ""
					Dim buchungDtbis As String = ""
					If IsDate(Me.deBuchungDate_1.Text) Then
						buchngDtab = Date.Parse(Me.deBuchungDate_1.Text).ToString("d")
					End If
					If IsDate(Me.deBuchungDate_2.Text) Then
						buchungDtbis = Date.Parse(Me.deBuchungDate_2.Text).ToString("d")
					End If

					' Suche zwischen zwei Datum
					If buchngDtab.Length > 0 And buchungDtbis.Length > 0 Then
						sSql += String.Format("{0}ZE.B_Date Between '{1} 00:00' And '{2} 23:59'",
																	strAndString, buchngDtab, buchungDtbis)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit Buchungsdatum zwischen {0} und {1}{2}"), buchngDtab, buchungDtbis, vbLf))
						' Suche ab erstes Datum
					ElseIf buchngDtab.Length > 0 Then
						sSql += String.Format("{0}ZE.B_Date >= '{1}'", strAndString, buchngDtab)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge ab Buchungsdatum {0}{1}"),
																															buchngDtab, vbLf))
						' Suche bis zweites Datum
					ElseIf buchungDtbis.Length > 0 Then
						sSql += String.Format("{0}ZE.B_Date <= '{1} 23:59'", strAndString, buchungDtbis)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge bis Buchungsdatum {0}{1}"),
																															buchungDtbis, vbLf))
					End If
				End If

				' Rechnungsbetrag --------------------------------------------------------------------------------------------------
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim betr1 As String = Me.txtBUBetrag1.Text.Replace("'", "").Replace("*", "%").Trim
				Dim betr2 As String = Me.txtBUBetrag2.Text.Replace("'", "").Replace("*", "%").Trim

				If betr1 = "" And betr2 = "" Then
					'do nothing

				ElseIf betr1 = betr2 Then
					' Suche genau ein Betrag
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit Rechnungsbetrag = {0}{1}"), betr1, vbLf))
					sSql += String.Format("{0}ZE.Betrag = {1}", strAndString, betr1)

				ElseIf betr1 <> "" And betr2 = "" Then
					' Suche ab Betrag 1 
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge ab Rechnungsbetrag = {0}{1}"), betr1, vbLf))
					sSql += String.Format("{0}ZE.Betrag >= {1}", strAndString, betr1)

				ElseIf betr1 = "" And betr2 <> "" Then
					' Suche bis Betrag 2
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge bis Rechnungsbetrag = {0}{1}"), betr2, vbLf))
					sSql += String.Format("{0}ZE.Betrag <= {1}", strAndString, betr2)

				Else
					' Suche zwischen ersten und zweiten Betrag
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit Rechnungsbetrag zwischen {0} und {1}{2}"),
																							betr1, betr2, vbLf))
					sSql += String.Format("{0}ZE.Betrag Between '{1}' And '{2}'", strAndString, betr1, betr2)
				End If

			Else
				Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
			End If

			' Buchungskonto --------------------------------------------------------------------------------------------------
			If Me.Cbo_BuKonto.SelectedIndex > -1 And Me.Cbo_BuKonto.Text <> String.Empty Then
				cv = DirectCast(Me.Cbo_BuKonto.SelectedItem, ComboValue)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim buKonto As String = cv.ComboValue.Trim ' Me.Cbo_BuKonto.ToItem.Value
				ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Zahlungseingänge mit Buchungskonto {0}{1}"), buKonto, vbLf))
				sSql += String.Format("{0}ZE.FKSoll = {1}", strAndString, buKonto)
			End If

			' MwSt.-pflichtig/-frei ------------------------------------------------------------------------------------------
			If Me.Cbo_MwSt.SelectedIndex > -1 And Me.Cbo_MwSt.Text <> String.Empty Then
				cv = DirectCast(Me.Cbo_MwSt.SelectedItem, ComboValue)
				strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
				Dim mwstPflicht As Boolean = cv.ComboValue.Trim = 1 ' Me.Cbo_MwSt.ToItem.Value = 1
				ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Mit MwSt.-{0} Zahlungseingänge"), Me.Cbo_MwSt.Text))
				If mwstPflicht Then
					sSql += String.Format("{0}RE.MwStProz > 0", strAndString)
				ElseIf cv.ComboValue = 0 Then ' Me.Cbo_MwSt.ToItem.Value = 0 Then
					sSql += String.Format("{0}RE.MwStProz = 0", strAndString)
				Else
					sSql += String.Format("{0}RE.MwStProz = {1}", strAndString, cv.ComboValue) ' Me.Cbo_MwSt.ToItem.Value)
				End If
			End If

			' 1. KST -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(Me.Cbo_KST1.Text) <> String.Empty Then
				Dim kst1 = Me.Cbo_KST1.Text.Trim

				If Not Me.ChkKst1Exakt.Checked Then
					sSql += String.Format("{0}RE.REKst1 Like '%{1}%' ", strAndString, kst1)
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Mit 1.KST wie {0}{1}"), Me.Cbo_KST1.Text, vbLf))

				Else
					Dim strName As String() = Regex.Split(kst1.Trim, ",")
					Dim strMyName As String = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
					Next
					If strName.Length > 0 Then kst1 = strMyName

					If kst1.Contains("Leere Felder") Then
						sSql += String.Format("{0}(RE.REKst1 = '' Or RE.REKst1 Is Null)", strAndString)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Mit leeren 1.KST-Feld{0}"), vbLf))
					Else
						If InStr(kst1, ",") > 0 Then kst1 = Replace(kst1, ",", "','")
						sSql += String.Format("{0}RE.REKst1 In ('{1}')", strAndString, kst1)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Mit 1.KST exakt für ('{0}'){1}"), kst1, vbLf))
					End If
				End If

			End If

			' 2. KST -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(Me.Cbo_KST2.Text) <> String.Empty Then
				Dim kst2 = Me.Cbo_KST2.Text.Trim

				If Not Me.ChkKst2Exakt.Checked Then
					sSql += String.Format("{0}RE.REKst2 Like '%{1}%' ", strAndString, kst2)
					ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Mit 2.KST wie {0}{1}"), Me.Cbo_KST2.Text, vbLf))
				Else
					Dim strName As String() = Regex.Split(kst2.Trim, ",")
					Dim strMyName As String = String.Empty
					For i As Integer = 0 To strName.Length - 1
						strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
					Next
					If strName.Length > 0 Then kst2 = strMyName

					If kst2.Contains("Leere Felder") Then
						sSql += String.Format("{0}(RE.REKst2 = '' Or RE.REKst2 Is Null)", strAndString)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Mit leeren 2.KST-Feld{0}"), vbLf))
					Else
						If InStr(kst2, ",") > 0 Then kst2 = Replace(kst2, ",", "','")
						sSql += String.Format("{0}RE.REKst2 In ('{1}')", strAndString, kst2)
						ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Mit 2.KST exakt für ('{0}'){1}"), kst2, vbLf))
					End If
				End If

			End If

			' Berater -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(Me.Cbo_Berater.Text) <> String.Empty Then
				Dim strFieldName As String = "RE.Kst"
				Dim sZusatzBez As String = String.Empty
				Dim strName As String()
				Dim strMyName As String = String.Empty
				ClsDataDetail.GetFilterBezArray.Add(String.Format("Berater wie ({0}){1}", Me.Cbo_Berater.Text, vbLf))

				Dim strKst As String = String.Empty
				If Not String.IsNullOrWhiteSpace(Me.Cbo_Berater.Text) Then
					Dim aUSData As String() = Me.Cbo_Berater.Text.Split(CChar("("))
					sZusatzBez = aUSData(1).Replace(")", "").ToUpper
				End If
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i) & "'"
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '" & strName(i) & "/%'"
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%/" & strName(i) & "'"
				Next

				If strName.Length > 0 Then sZusatzBez = strMyName
				sSql += strAndString & " (" & sZusatzBez & ")"

			End If




			' Filiale -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(Me.Cbo_Filiale.Text) <> String.Empty Then
				Dim filiale As String = Me.Cbo_Filiale.Text.Trim
				'FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf
				ClsDataDetail.GetFilterBezArray.Add(String.Format("Filiale wie ({0}){1}", filiale, vbLf))

				filiale = GetFilialKstData(filiale)
				filiale = Replace(filiale, "'", "")
				Dim strName As String() = Regex.Split(filiale.Trim, ",")
				Dim strMyName As String = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += String.Format("{0}RE.Kst = '{1}'", IIf(strMyName.Length > 0, " Or ", ""), strName(i))
					strMyName += String.Format("{0}RE.Kst Like '{1}/%'", IIf(strMyName.Length > 0, " Or ", ""), strName(i))
					strMyName += String.Format("{0}RE.Kst Like '%/{1}'", IIf(strMyName.Length > 0, " Or ", ""), strName(i))
				Next
				If strName.Length > 0 Then filiale = strMyName
				If InStr(filiale, ",") > 0 Then filiale = Replace(filiale, ",", ",")
				sSql += strAndString & " (" & filiale & ")"
			End If

			' Rechnungsart -------------------------------------------------------------------------------------------------------
			strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
			If UCase(Me.Cbo_REArt.Text) <> String.Empty Then
				Dim reart As String = Me.Cbo_REArt.Text

				' Bezeichnung ausschreiben
				Dim codeAusgeschrieben As String = ""
				For Each fakturaArt In reart.Split(CChar(","))
					Select Case fakturaArt.Trim
						Case "A"
							codeAusgeschrieben += String.Format("(A) Automatische ")
						Case "F"
							codeAusgeschrieben += String.Format("(F) Festanstellung ")
						Case "G"
							codeAusgeschrieben += String.Format("(G) Gutschriften ")
						Case "I"
							codeAusgeschrieben += String.Format("(I) Individuelle ")
					End Select

				Next
				ClsDataDetail.GetFilterBezArray.Add(String.Format(m_Translate.GetSafeTranslationValue("Faktura-Art: {0}{1}"), codeAusgeschrieben, vbLf))

				Dim strName As String() = Regex.Split(reart.Trim, ",")
				Dim strMyName As String = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then reart = strMyName
				If InStr(reart, ",") > 0 Then reart = Replace(reart, ",", "','")

				sSql += String.Format("{0}RE.Art In ('{1}')", strAndString, reart)
			End If

			m_Logger.LogInfo(String.Format("ZESearchQuery: {0}", sSql))


			' Order-By-Klausel ===================================================================================================
			' ====================================================================================================================
			Dim strSortQuery As String = _ClsDb.GetSortString(Me)
			If String.IsNullOrWhiteSpace(Cbo_Berater.EditValue) Then
				sSql = String.Format("{0} SELECT * FROM {1} {2}", sSql, ClsDataDetail.SPTabNamenZEListe, strSortQuery)
			Else
				sSql = String.Format("{0} SELECT * FROM {1} {2};  Update {1} Set Betrag = (Betrag / 2) WHERE CHARINDEX('/', KST) > 0;", sSql, ClsDataDetail.SPTabNamenZEListe, strSortQuery)
			End If

			' Query durchführen und auf DB speichern =============================================================================
			' ====================================================================================================================
			Dim conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Dim cmd As SqlCommand = New SqlCommand(sSql, conn)
			Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)

			If da.Fill(ClsDataDetail.DtZE) > 0 Then
				' Man kann hier eine Meldung einbauen.
			End If

			' Die Query für das Holen der Daten ==================================================================================
			' ====================================================================================================================

			Me.txt_SQLQuery.Text = String.Format("SELECT * FROM {0} {1}", ClsDataDetail.SPTabNamenZEListe, strSortQuery)



		Catch ex As Exception
			MessageBox.Show(ex.Message, "GetMyQueryString", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		Return True
	End Function

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiclear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		'Dim cControl As Control
		'Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmZESearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()

		FillDefaultValues()

		'Me.CboSort.Text = strText
		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabZESearch").Controls
				For Each ctrls In tabPg.Controls
					ResetControl(ctrls)
				Next
			Next
		Catch ex As Exception
			MessageBox.Show(ex.Message, "ResetAllTabEntries", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub
	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion mit rekursivem Aufruf.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Try
			If con.Name.ToLower = "cbosort" Then Exit Sub
			'Trace.WriteLine(con.Name.ToLower)

			If TypeOf (con) Is TextBox Then
				Dim tb As TextBox = con
				tb.Text = String.Empty

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = con
				tb.Text = String.Empty

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
				' Bewirkt, dass alle Items neu geladen werden müssen
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
		Catch ex As Exception
			MessageBox.Show(ex.Message, "ResetControl", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	''' <summary>
	''' Default-Werte 
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()
		Me.ChkKst1Exakt.Checked = True
		Me.ChkKst2Exakt.Checked = True
		Me.ChkBeraterExakt.Checked = True
	End Sub

	Sub ExportDataForSWIFAC(ByVal docname As String)
		Dim lst As New List(Of String)
		Dim zeile As String = ""
		Dim belegDatum As String = ""
		Dim belegTyp As String = ""
		Dim belegNr As String = ""
		Dim debitorenNr As String = ""
		Dim währung As String = ""
		Dim betrag As String = ""
		Dim beschreibung As String = ""
		Dim fälligkeitsDatum As String = ""
		Dim skontoDatum As String = ""
		Dim skonto As String = ""
		Dim ausgleichMitBelegart As String = ""
		Dim ausgleichMitBelegNr As String = ""

		Dim conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim cmd As SqlCommand = New SqlCommand(String.Format("SELECT * FROM {0}", ClsDataDetail.SPTabNamenZEListe), conn)
		Dim dt As DataTable = New DataTable("Debitorenliste")
		Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
		da.Fill(dt)
		If dt.Rows.Count > 0 Then
			For Each row As DataRow In dt.Rows
				belegDatum = DateTime.Parse(row("FAK_DAT").ToString).ToShortDateString
				If row("ART").ToString = "G" Then
					belegTyp = "Gutschrift"
				Else
					belegTyp = "Rechnung"
				End If
				belegNr = row("RENR").ToString
				debitorenNr = row("KDNR").ToString
				währung = row("Currency").ToString
				betrag = String.Format("{0:0.00}", Decimal.Parse(row("BetragInk").ToString))
				beschreibung = "" ' Bleibt leer
				fälligkeitsDatum = DateTime.Parse(row("Faellig").ToString).ToShortDateString
				skontoDatum = "" ' Bleibt leer
				skonto = "" ' Bleibt leer
				ausgleichMitBelegart = "" ' Bleibt leer
				ausgleichMitBelegNr = "" ' Bleibt leer
				zeile = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}" _
																						, belegDatum _
																						, belegTyp _
																						, belegNr _
																						, debitorenNr _
																						, währung _
																						, betrag _
																						, beschreibung _
																						, fälligkeitsDatum _
																						, skontoDatum _
																						, skonto _
																						, ausgleichMitBelegart _
																						, ausgleichMitBelegNr
																						)
				lst.Add(zeile)
			Next
			ExportDataToFile(lst, docname)

		End If
	End Sub

	''' <summary>
	''' Ein File-Dialogfenster wird geöffnet, um die Datei zu speichern.
	''' </summary>
	''' <param name="listOfStrings"></param>
	''' <param name="docname"></param>
	''' <param name="defaultExtension"></param>
	''' <remarks></remarks>
	Sub ExportDataToFile(ByVal listOfStrings As List(Of String),
											 Optional ByVal docname As String = "", Optional ByVal defaultExtension As String = "txt")
		Dim saveDial As SaveFileDialog = New SaveFileDialog()
		Dim result As DialogResult
		saveDial.FileName = docname
		saveDial.DefaultExt = "csv"
		result = saveDial.ShowDialog()

		If result = DialogResult.OK Then
			Try
				IO.File.WriteAllLines(saveDial.FileName, listOfStrings.ToArray, System.Text.Encoding.Default)
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Die Datei {0} wurde erfolgreich gespeichert."),
																			saveDial.FileName), "Daten gespeichert",
												MessageBoxButtons.OK, MessageBoxIcon.Information)
			Catch ex As Exception
				MessageBox.Show(ex.Message, m_Translate.GetSafeTranslationValue("Speicherung nicht durchgeführt"),
												MessageBoxButtons.OK, MessageBoxIcon.Error)
			End Try
		End If

	End Sub


#End Region


#Region "KeyEvents für Lst und Textfelder..."

	Private Sub txtKDNr_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDNr1.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDNr1_ButtonClick(sender, New System.EventArgs)

			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtKDNr_2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDNr2.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDNr1_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtOPNr_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRENr1.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtRENr1_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtOPNr_2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRENr2.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtRENr2_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtOPNr_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Private Sub txt_OpenBetrag_1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

		Try
			sender.text = Format(Val(sender.text.ToString), "0.00")

		Catch ex As Exception
			sender.text = "0.00"

		End Try

	End Sub

	Private Sub txt_FakDat_1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

		Try
			sender.text = Format(CDate(sender.text.ToString), "d")

		Catch ex As Exception
			sender.text = String.Empty

		End Try

	End Sub

	''' <summary>
	''' Übergebene Controls mit dem Klick-Event verbinden
	''' </summary>
	''' <param name="Ctrls"></param>
	''' <remarks></remarks>
	Private Sub InitClickHandler(ByVal ParamArray Ctrls() As Control)

		For Each Ctrl As Control In Ctrls
			AddHandler Ctrl.KeyPress, AddressOf KeyPressEvent
			'      AddHandler Ctrl.Click, AddressOf ClickEvents
		Next

	End Sub

	Private Sub txtZENr1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtZENr1.ButtonClick
		Dim frmTest As New frmSearchRec("ZENr")

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.GetSelektion)
		Me.txtZENr1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtZENr2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtZENr2.ButtonClick
		Dim frmTest As New frmSearchRec("ZENr")

		_ClsFunc.Get4What = "ZENr"
		ClsDataDetail.strButtonValue = "ZE"
		ClsDataDetail.Get4What = "ZENR"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.Get4What)
		Me.txtZENr2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtKDNr1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDNr1.ButtonClick
		Dim frmTest As New frmSearchRec("KDNr")
		Dim strModulName As String = String.Empty

		_ClsFunc.Get4What = "KDNr"
		ClsDataDetail.strButtonValue = "KD"
		ClsDataDetail.Get4What = "KDNr"

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iESValue(_ClsFunc.Get4What)
		Me.txtKDNr1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtKDNr2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDNr2.ButtonClick
		Dim frmTest As New frmSearchRec("KDNr")
		Dim strModulName As String = String.Empty

		_ClsFunc.Get4What = "KDNr"
		ClsDataDetail.strButtonValue = "KD"
		ClsDataDetail.Get4What = "KDNr"

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iESValue(_ClsFunc.Get4What)
		Me.txtKDNr2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtRENr1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtRENr1.ButtonClick
		Dim frmTest As New frmSearchRec("RENr")

		_ClsFunc.Get4What = "RENr"
		ClsDataDetail.strButtonValue = "RE"
		ClsDataDetail.Get4What = "RENR"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iESValue(_ClsFunc.Get4What)
		Me.txtRENr1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub txtRENr2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtRENr2.ButtonClick
		Dim frmTest As New frmSearchRec("RENr")
		Dim strModulName As String = String.Empty

		_ClsFunc.Get4What = "RENr"
		ClsDataDetail.strButtonValue = "RE"
		ClsDataDetail.Get4What = "RENR"

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iESValue(_ClsFunc.Get4What)
		Me.txtRENr2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub





	''' <summary>
	''' Klick-Event der Controls auffangen und verarbeiten
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtBUBetrag2.KeyPress, txtBUBetrag1.KeyPress, txtRENr2.KeyPress, txtRENr1.KeyPress, txtKDNr2.KeyPress, txtKDNr1.KeyPress, Cbo_KST1.KeyPress, Cbo_REArt.KeyPress, Cbo_KST2.KeyPress, Cbo_Filiale.KeyPress, Cbo_Berater.KeyPress, CboSort.KeyPress, txtZENr2.KeyPress, txtZENr1.KeyPress, Cbo_MwSt.KeyPress, Cbo_BuKonto.KeyPress ' System.EventArgs)
		'   ToDo  Auswertung und Klick-Aktion ausführen
		'If sender Is TextBox1 Then

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		'End If
	End Sub

#End Region

	Private Sub frmOPSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmZESearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub


#Region "Sonstige Funktionen..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		Stopwatch.Reset()
		Stopwatch.Start()

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		e.Result = GetMyQueryString() ' FillrecData2Array(Me.LvFoundedrecs, Me.txt_SQLQuery.Text)
		If bw.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(e.Error.Message)
		Else
			If e.Cancelled = True Then
				MessageBox.Show("Aktion abgebrochen!")

			Else
				BackgroundWorker1.CancelAsync()
				'        MessageBox.Show(e.Result.ToString())

			End If
		End If

	End Sub


#End Region

	Private Sub frmOPSearch_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
		Me.txtZENr1.Focus()
	End Sub


	Private Sub btnPrint_DropDownOpened(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub

	Private Sub btnExport_DropDownOpening(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub



	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtZENr2.Visible = Me.SwitchButton1.Value
		Me.txtZENr2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txtRENr2.Visible = Me.SwitchButton2.Value
		Me.txtRENr2.Text = String.Empty
	End Sub

	Private Sub SwitchButton3_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton3.ValueChanged
		Me.txtKDNr2.Visible = Me.SwitchButton3.Value
		Me.txtKDNr2.Text = String.Empty
	End Sub

	Private Sub txtZENr1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtZENr1.SelectedIndexChanged

	End Sub
End Class

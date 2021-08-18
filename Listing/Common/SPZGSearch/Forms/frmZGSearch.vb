
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports DevExpress.XtraEditors.Controls
Imports System.Threading
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Public Class frmZGSearch
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

	Private _ClsFunc As New ClsDivFunc

	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmZGSearch_LV

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean


#Region "Constructor..."

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
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, m_InitializationData.UserData.UserNr)

			m_InitializationData = ChangeMandantData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub


#End Region


#Region "Lb clicks 1. Seite..."

	Private Sub txtZGNr1onButtonclick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtZGNr_1.ButtonClick
		Dim frmTest As New frmSearchRec

		_ClsFunc.Get4What = "ZGNR"
		ClsDataDetail.strButtonValue = "ZG"
		ClsDataDetail.Get4What = "ZGNR"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue(_ClsFunc.Get4What)
		Me.txtZGNr_1.Text = CStr(m.ToString)
		frmTest.Dispose()

	End Sub

	Private Sub txtZGNr2onButtonclick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtZGNr_2.ButtonClick
		Dim frmTest As New frmSearchRec
		Dim strModulName As String = String.Empty

		_ClsFunc.Get4What = "ZGNR"
		ClsDataDetail.strButtonValue = "ZG"
		ClsDataDetail.Get4What = "ZGNR"

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iKDValue(_ClsFunc.Get4What)
		Me.txtZGNr_2.Text = CStr(m.ToString)
		frmTest.Dispose()

	End Sub

	Private Sub txtMANr1onButtonclick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMANr_1.ButtonClick
		Dim frmTest As New frmSearchRec
		Dim strModulName As String = String.Empty

		_ClsFunc.Get4What = "MANr"
		ClsDataDetail.strButtonValue = "MA"
		ClsDataDetail.Get4What = "MANr"

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iKDValue(_ClsFunc.Get4What)
		Me.txtMANr_1.Text = CStr(m.ToString)
		frmTest.Dispose()

	End Sub

	Private Sub txtMANr2onButtonclick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMANr_2.ButtonClick
		Dim frmTest As New frmSearchRec
		Dim strModulName As String = String.Empty

		_ClsFunc.Get4What = "MANr"
		ClsDataDetail.strButtonValue = "MA"
		ClsDataDetail.Get4What = "MANr"

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iKDValue(_ClsFunc.Get4What)
		Me.txtMANr_2.Text = CStr(m.ToString)
		frmTest.Dispose()

	End Sub

#End Region


#Region "Dropdown Funktionen 1. Seite..."

	Private Sub CboSort_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(Me.CboSort)
	End Sub


	Private Sub Cbo_Berater_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		If Me.Cbo_Berater.Properties.Items.Count = 0 Then ListBerater(Me.Cbo_Berater)
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		If Me.Cbo_Filiale.Properties.Items.Count = 0 Then ListZGFiliale(Me.Cbo_Filiale)
	End Sub

	Private Sub Cbo_Month_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Month.QueryPopUp
		If Me.Cbo_Month.Properties.Items.Count = 0 Then ListZGMonth(Cbo_Month)
	End Sub

	Private Sub Cbo_Year_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Year.QueryPopUp
		If Me.Cbo_Year.Properties.Items.Count = 0 Then ListZGYear(Cbo_Year)
	End Sub

	Private Sub Cbo_LANr_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_LANr.QueryPopUp
		ListZGLANr(Cbo_LANr)
	End Sub

	Private Sub Cbo_LO_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_LO.QueryPopUp
		If Me.Cbo_LO.Properties.Items.Count = 0 Then ListForActivate(Me.Cbo_LO)
	End Sub

	Private Sub Cbo_Paryed_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Paryed.QueryPopUp
		If Me.Cbo_Paryed.Properties.Items.Count = 0 Then ListForActivate(Me.Cbo_Paryed)
	End Sub

	Private Sub Cbo_Currency_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Currency.QueryPopUp
		If Me.Cbo_Currency.Properties.Items.Count = 0 Then ListZGCurrency(Me.Cbo_Currency)
	End Sub

#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmOnDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmZGSearch_LV", True)

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iWidth = Me.Width
				My.Settings.iHeight = Me.Height

				My.Settings.SortBez = Me.CboSort.Text

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
			Me.xtabErweitert.Text = m_Translate.GetSafeTranslationValue(Me.xtabErweitert.Text)
			Me.xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)

			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
			Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
			Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
			Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

			Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
			Me.lblheader2.Text = m_Translate.GetSafeTranslationValue(Me.lblheader2.Text)

			Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)

			Me.lblSortierung.Text = m_Translate.GetSafeTranslationValue(Me.lblSortierung.Text)
			Me.lblNummer.Text = m_Translate.GetSafeTranslationValue(Me.lblNummer.Text)
			Me.lblkandidat.Text = m_Translate.GetSafeTranslationValue(Me.lblkandidat.Text)
			Me.lblverguetung.Text = m_Translate.GetSafeTranslationValue(Me.lblverguetung.Text)
			Me.lblmonat.Text = m_Translate.GetSafeTranslationValue(Me.lblmonat.Text)
			Me.lbljahr.Text = m_Translate.GetSafeTranslationValue(Me.lbljahr.Text)
			Me.lblzahlart.Text = m_Translate.GetSafeTranslationValue(Me.lblzahlart.Text)
			Me.lblwaehrung.Text = m_Translate.GetSafeTranslationValue(Me.lblwaehrung.Text)
			Me.lblberater.Text = m_Translate.GetSafeTranslationValue(Me.lblberater.Text)
			Me.lblfiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblfiliale.Text)

			Me.lblueberwiesen.Text = m_Translate.GetSafeTranslationValue(Me.lblueberwiesen.Text)
			Me.lbllohnabrechnung.Text = m_Translate.GetSafeTranslationValue(Me.lbllohnabrechnung.Text)

			Me.lblzahlungam.Text = m_Translate.GetSafeTranslationValue(Me.lblzahlungam.Text)
			Me.lbldruckam.Text = m_Translate.GetSafeTranslationValue(Me.lbldruckam.Text)

			Me.lblAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblAbfrage.Text)
			Me.lblderzeitigeabfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblderzeitigeabfrage.Text)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

		cbo.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Auszahlungsnummer")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kandidatennummer")))
		cbo.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Kandidatenname")))

		cbo.Properties.Items.Add(String.Format("3 - {0}", m_Translate.GetSafeTranslationValue("Auszahlungdatum (Aufsteigend)")))
		cbo.Properties.Items.Add(String.Format("4 - {0}", m_Translate.GetSafeTranslationValue("Auszahlungdatum (Absteigend)")))

		cbo.Properties.Items.Add(String.Format("5 - {0}", m_Translate.GetSafeTranslationValue("Betrag (Aufsteigend)")))
		cbo.Properties.Items.Add(String.Format("6 - {0}", m_Translate.GetSafeTranslationValue("Betrag (Absteigend)")))

		cbo.Properties.DropDownRows = 7
	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmZGSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetDefaultSortValues()

		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Me.KeyPreview = True
		Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		Try
			If My.Settings.frmLocation <> String.Empty Then
				Me.Width = Math.Max(My.Settings.iWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		If m_InitializationData.UserData.UserNr <> 1 Then
			Me.xtabZGSearch.TabPages.Remove(Me.xtabSQLAbfrage)
			Me.xtabZGSearch.TabPages.Remove(Me.xtabErweitert)
		End If

		Me.bbiPrint.Enabled = False
		Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		Me.LblTimeValue.Visible = CBool(CInt(m_InitializationData.UserData.UserNr) = 1)
		Me.Cbo_Year.Properties.Items.Clear()

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				If strSort = String.Empty Then strSort = String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kandidatennname"))
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
				Dim showMDSelection As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)
				Me.lueMandant.Visible = showMDSelection
				Me.lblMDName.Visible = showMDSelection

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
				Me.lueMandant.Visible = False
				Me.lblMDName.Visible = False
			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True

		Dim sSql As String = Me.txt_SQLQuery.Text
		If sSql = String.Empty Then
			MsgBox(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!."), MsgBoxStyle.Exclamation, "GetData4Print")
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		If bForDesign Then sSql = Replace(sSql, "Select ZG.", "Select Top 10 ZG.")
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

		Try
			Conn.Open()

			Dim rFoundedrec4Print As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			Try
				If Not rFoundedrec4Print.HasRows Then
					cmd.Dispose()
					rFoundedrec4Print.Close()

					MessageBox.Show(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."),
													"GetData4Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
				End If

			Catch ex As Exception
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetData4Print_2")

			End Try

			rFoundedrec4Print.Read()
			If rFoundedrec4Print.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo

				PrintListingThread = New Thread(AddressOf StartPrinting)
				PrintListingThread.Name = "PrintingZGListing"
				PrintListingThread.SetApartmentState(ApartmentState.STA)
				PrintListingThread.Start()

			End If
			rFoundedrec4Print.Close()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetData4Print_3")

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

	End Sub

	Sub StartPrinting()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLZGSearchPrintSetting With {.SelectedMDNr = lueMandant.EditValue, .DbConnString2Open = m_InitializationData.MDData.MDDbConn, .SQL2Open = Me.SQL4Print, .JobNr2Print = Me.PrintJobNr}
		Dim obj As New SPS.Listing.Print.Utility.ZGSearchListing.ClsPrintZGSearchList(_Setting)
		obj.PrintZGSearchList_1(Me.bPrintAsDesign, ClsDataDetail.GetSortBez, _ClsFunc.GetBetragSign,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub

#Region "Funktionen zur Menüaufbau..."

	Private Sub mnuDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

		GetData4Print(True, False, ClsDataDetail.GetModulToPrint())

	End Sub

	Private Sub mnuZGListPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

		GetData4Print(False, False, ClsDataDetail.GetModulToPrint())

	End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)

		Try
			_ClsFunc.GetZGNr = 0
			_ClsFunc.GetMANr = 0
			ClsDataDetail.GetTotalBetrag = 0
			Me.txt_SQLQuery.Text = String.Empty
			Me.LblTimeValue.Text = String.Empty
			If Not Me.txtZGNr_2.Visible Then Me.txtZGNr_2.Text = Me.txtZGNr_1.Text
			If Not Me.txtMANr_2.Visible Then Me.txtMANr_2.Text = Me.txtMANr_1.Text

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")
			GetMyQueryString()

			Stopwatch.Reset()
			Stopwatch.Start()

			' Daten auflisten...
			If Not FormIsLoaded("frmZGSearch_LV", True) Then
				frmMyLV = New frmZGSearch_LV(Me.txt_SQLQuery.Text, Me.Location.X, Me.Location.Y, Me.Height)

				frmMyLV.Show()
				Me.Select()
			End If

			Me.bbiPrint.Enabled = frmMyLV.RecCount > 0
			Me.bbiExport.Enabled = frmMyLV.RecCount > 0
			If frmMyLV.RecCount > 0 Then
				CreatePrintPopupMenu()
				CreateExportPopupMenu()
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

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty
		Dim _ClsDb As New ClsDbFunc

		If Me.txt_IndSQLQuery.Text = String.Empty Then
			sSql1Query = _ClsDb.GetStartSQLString()    ' 1. String
			sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)   ' Where Klausel

			If Trim(sSql2Query) <> String.Empty Then
				sSql1Query += " Where "
			End If
			strSort = _ClsDb.GetSortString(Me)      ' Sort Klausel

			Me.txt_SQLQuery.Text = sSql1Query + sSql2Query + strSort
			If strLastSortBez = String.Empty Then strLastSortBez = strSort

		Else

			Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
		End If
		_ClsDb.GetJobNr4Print(Me)

		Return True
	End Function

#Region "Reset Controls..."

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmZGSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()

		Me.CboSort.Text = strText
		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabZGSearch").Controls
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
			'Die Sortierung darf nicht zurückgesetzt werden
			If cbo.Name = "CboSort" Then Exit Sub
			' Alle Felder auf Unchecked setzen
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

	End Sub

#End Region

#End Region

#Region "KeyDown für Lst und Textfelder..."

	Private Sub txtMANr_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMANr_1.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtMANr1onButtonclick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtMANr_2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMANr_2.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtMANr2onButtonclick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtZGNr_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtZGNr_1.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtZGNr1onButtonclick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtZGNr_2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtZGNr_2.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtZGNr2onButtonclick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub



#End Region


	Private Sub txtZGNr_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtZGNr_1.KeyPress, txtZGNr_2.KeyPress, txtMANr_1.KeyPress, txtMANr_2.KeyPress, txtVGNr_1.KeyPress, Cbo_Filiale.KeyPress,
		Cbo_Month.KeyPress, Cbo_LANr.KeyPress, Cbo_Currency.KeyPress, Cbo_Year.KeyPress, Cbo_Berater.KeyPress

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

	''' <summary>
	''' Klick-Event der Controls auffangen und verarbeiten
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) ' System.EventArgs)
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

	End Sub

	Private Sub frmZGSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmZGSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Try

			Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

			If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu

		Try
			Dim bShowPrint As Boolean = IsUserActionAllowed(0, 605)
			Dim bShowDesign As Boolean = IsUserActionAllowed(0, 615)
			Dim liMnu As New List(Of String) From {If(bShowPrint, "Liste drucken#mnuListPrint", ""),
																						 If(bShowDesign, "Liste bearbeiten#PrintDesign", "")}
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
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

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

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		Select Case e.Item.Name.ToUpper
			Case "mnuListPrint".ToUpper
				Me.bPrintAsDesign = False
				GetData4Print(False, False, ClsDataDetail.GetModulToPrint())

			Case "PrintDesign".ToUpper
				Me.bPrintAsDesign = True
				GetData4Print(True, False, ClsDataDetail.GetModulToPrint())


			Case Else
				Exit Sub

		End Select

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
			bbiExport.Manager = Me.BarManager1
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

		Select Case e.Item.Name.ToUpper

			Case UCase("TXT")
				Call RunKommaModul(Me.txt_SQLQuery.Text)


			Case Else
				Exit Sub

		End Select

	End Sub


	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtZGNr_2.Visible = Me.SwitchButton1.Value
		Me.txtZGNr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txtMANr_2.Visible = Me.SwitchButton2.Value
		Me.txtMANr_2.Text = String.Empty
	End Sub




	Private Sub txtZGNr_1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles txtZGNr_1.SelectedIndexChanged

	End Sub

End Class


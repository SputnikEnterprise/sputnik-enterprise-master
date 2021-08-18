
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevComponents.DotNetBar
Imports System.Threading

Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings

Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Public Class frmKDUmsatz
	Inherits DevExpress.XtraEditors.XtraForm


#Region "Private Consts"

	Private Const M_SEPRATOR As String = "#@"

#End Region

#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private _ClsFunc As New ClsDivFunc

	Private m_common As CommonSetting
	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI


	Public Shared frmMyLV As frmKDUmsatz_LV
	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private Property Filter_VonMonth_1 As String
	Private Property Filter_BisMonth_1 As String
	Private Property Filter_VonMonth_2 As String
	Private Property Filter_BisMonth_2 As String
	Private Property Filter_VonYear_1 As String
	Private Property Filter_VonYear_2 As String

	Private Property TranslatedPage As New List(Of Boolean)

	Private m_SearchCriteria As New SearchCriteria

	Enum SelectedListArt

		Grouped

		Detail

	End Enum

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_mandant = New Mandant
		m_common = New CommonSetting


		m_InitializationData = _setting

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)




		ResetMandantenDropDown()
		LoadMandantenDropDown()

	End Sub

#End Region


#Region "private property"

	Private Property FinalSQLQuery As String

	Private ReadOnly Property ListArt As SelectedListArt
		Get
			If Me.CboLstArt.Text.StartsWith("0") Then
				Return SelectedListArt.Detail
			Else
				Return SelectedListArt.Grouped
			End If
		End Get
	End Property


#End Region


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

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
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData) ' MandantenData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(CInt(lueMandant.EditValue), ClsDataDetail.m_InitialData.UserData.UserNr)

			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub


#End Region


	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property




#Region "Dropdown Funktionen 1. Seite..."

	Private Sub CboSort_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then LoadListSort(Me.CboSort)
	End Sub

	Private Sub CboLstArt_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboLstArt.QueryPopUp
		If Me.CboLstArt.Properties.Items.Count = 0 Then LoadListArt(Me.CboLstArt)
	End Sub

	Private Sub Cbo_KDKanton_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDKanton.QueryPopUp
		If Me.Cbo_KDKanton.Properties.Items.Count = 0 Then LoadListKDKanton(Me.Cbo_KDKanton)
	End Sub

	Private Sub Cbo_VMonth_1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_VMonth_1.QueryPopUp
		If Me.Cbo_VMonth_1.Properties.Items.Count = 0 Then ListListMonth(Cbo_VMonth_1)
	End Sub

	Private Sub Cbo_BMonth_1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_BMonth_1.QueryPopUp
		If Me.Cbo_BMonth_1.Properties.Items.Count = 0 Then ListListMonth(Cbo_BMonth_1)
	End Sub

	Private Sub Cbo_VYear_1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_VYear_1.QueryPopUp
		If Me.Cbo_VYear_1.Properties.Items.Count = 0 Then LoadListYear(Cbo_VYear_1)
	End Sub

	Private Sub Cbo_VMonth_2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_VMonth_2.QueryPopUp
		If Me.Cbo_VMonth_2.Properties.Items.Count = 0 Then ListListMonth(Cbo_VMonth_2)
	End Sub

	Private Sub Cbo_BMonth_2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_BMonth_2.QueryPopUp
		If Me.Cbo_BMonth_2.Properties.Items.Count = 0 Then ListListMonth(Cbo_BMonth_2)
	End Sub

	Private Sub Cbo_VYear_2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_VYear_2.QueryPopUp
		If Me.Cbo_VYear_2.Properties.Items.Count = 0 Then LoadListYear(Cbo_VYear_2)
	End Sub

	Private Sub Cbo_REKst1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_REKst1.QueryPopUp
		If Me.Cbo_REKst1.Properties.Items.Count = 0 Then LoadListOPKst(Me.Cbo_REKst1, 1)
	End Sub

	Private Sub Cbo_REKst2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_REKst2.QueryPopUp
		If Me.Cbo_REKst2.Properties.Items.Count = 0 Then LoadListOPKst(Me.Cbo_REKst2, 2)
	End Sub

	Private Sub Cbo_REKst3_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_REKst3.QueryPopUp
		LoadListBerater(Me.Cbo_REKst3)
	End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		LoadListKDFiliale(Me.Cbo_Filiale)
	End Sub

#End Region



	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

#Region "Form Aktionen..."

	Private Sub frmKDUmsatz_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmKDUmsatz_LV", True)

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iWidth = Me.Width
				My.Settings.iHeight = Me.Height

				My.Settings.SortBez = Me.CboSort.Text
				My.Settings.Listart = Me.CboLstArt.Text

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		Try
			For i As Integer = 0 To Me.xtabMain.TabPages.Count - 1
				Me.TranslatedPage.Add(False)
			Next
		Catch ex As Exception
			m_Logger.LogError(String.Format("2=>{0}.{1}", strMethodeName, ex.Message))
		End Try

		Try
			lbFakDateInfo.Text = m_Translate.GetSafeTranslationValue(lbFakDateInfo.Text)

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
			Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
			Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
			Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try
		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Try
			For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.xtabMain.TabPages
				tbp.Text = m_Translate.GetSafeTranslationValue(tbp.Text)
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("0=>{0}.{1}", strMethodeName, ex.Message))

		End Try
		Me.TranslatedPage(0) = True

	End Sub

	Private Sub SPKDUmsatz_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		SetDefaultSortValues()

		Try
			Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
			Me.Invoke(UpdateDelegate)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
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

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try
		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Dim showMDSelection As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 642, m_InitializationData.MDData.MDNr)
			Me.lueMandant.Visible = showMDSelection
			Me.lblMDName.Visible = showMDSelection

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

		Me.bbiExport.Enabled = False
		Me.bbiPrint.Enabled = False

		If m_common.GetLogedUserNr <> 1 Then
			Me.xtabMain.TabPages.Remove(Me.xtabSQLAbfrage)
			Me.xtabMain.TabPages.Remove(Me.xtabErweitert)
		End If

		Me.LblTimeValue.Visible = CBool(CInt(m_common.GetLogedUserNr.ToString) = 1)

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				Me.CboSort.Text = IIf(strSort = String.Empty, "1 - " & m_Translate.GetSafeTranslationValue("Kundenname"), strSort)
				LoadListSort(Me.CboSort)
				Dim aSortlist As String() = strSort.Split(CChar(","))

				For Each bez As String In aSortlist
					CboSort.Properties.Items(CInt(Val(bez))).CheckState = CheckState.Checked
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.Message))

			End Try

			Dim strListart As String = My.Settings.Listart
			Me.CboLstArt.Text = If(strListart = String.Empty, "1 - " & m_Translate.GetSafeTranslationValue("Liste gruppiert"), strListart)
			'Me.CboLstArt.Text = strListart

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub SPKDUmsatz_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmKDUmsatz_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If

	End Sub

	Private Sub frmESSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_common.GetLogedUserNr = 1 Then
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

#End Region


	Sub GetKDData4Print(ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True

		Dim sSql As String = FinalSQLQuery ' Me.txt_SQLQuery.Text
		If sSql = String.Empty Then
			DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"),
																								 m_Translate.GetSafeTranslationValue("Drucken"), MessageBoxButtons.OK, MessageBoxIcon.Error)
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

		Try
			Conn.Open()

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader
			Try
				If Not rKDrec.HasRows Then
					cmd.Dispose()
					rKDrec.Close()
					DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden!"),
																					 m_Translate.GetSafeTranslationValue("Drucken"), MessageBoxButtons.OK, MessageBoxIcon.Error)
					Exit Sub
				End If

				rKDrec.Read()
				Me.SQL4Print = sSql
				Me.PrintJobNr = strJobInfo

				Me.Filter_VonMonth_1 = Me.Cbo_VMonth_1.EditValue
				Me.Filter_BisMonth_1 = Me.Cbo_BMonth_1.EditValue
				Me.Filter_VonMonth_2 = Me.Cbo_VMonth_2.EditValue
				Me.Filter_BisMonth_2 = Me.Cbo_BMonth_2.EditValue
				Me.Filter_VonYear_1 = Me.Cbo_VYear_1.EditValue
				Me.Filter_VonYear_2 = Me.Cbo_VYear_2.EditValue
				StartPrinting()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Datensuche_2")

			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MsgBox(ex.Message, MsgBoxStyle.Critical, "Datensuche_3")

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

	End Sub

	Sub StartPrinting()
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 620)
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso bShowDesign

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLKDUmsatzSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																												.SQL2Open = Me.SQL4Print,
																																												.frmhwnd = GetHwnd,
																																												.JobNr2Print = Me.PrintJobNr,
																																												.ShowAsDesign = ShowDesign,
																																												.ListSortBez = ClsDataDetail.GetSortBez,
																																												.ListFilterBez = New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																																																																					 ClsDataDetail.GetFilterBez2,
																																																																					 ClsDataDetail.GetFilterBez3,
																																																																					 ClsDataDetail.GetFilterBez4}),
																																												.Filter_VonMonth_1 = Me.Filter_VonMonth_1,
																																												.Filter_BisMonth_1 = Me.Filter_BisMonth_1,
																																												.Filter_VonMonth_2 = Me.Filter_VonMonth_2,
																																												.Filter_BisMonth_2 = Me.Filter_BisMonth_2,
																																												.Filter_VonYear_1 = Me.Filter_VonYear_1,
																																												.Filter_VonYear_2 = Me.Filter_VonYear_2,
																																												.SelectedMDNr = m_InitializationData.MDData.MDNr,
																																												.LogedUSNr = ClsDataDetail.m_InitialData.UserData.UserNr}

		Dim obj As New SPS.Listing.Print.Utility.KDUmsatzSearchListing.ClsPrintKDUmsatzSearchList(_Setting)
		obj.PrintKDUmsatzList()

	End Sub


#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		FinalSQLQuery = String.Empty

		Try
			If Not Me.txtKDNr_2.Visible Then Me.txtKDNr_2.Text = Me.txtKDNr_1.Text

			Me.LblTimeValue.Text = String.Empty
			Me.txt_SQLQuery.Text = String.Empty
			Me.txt_1_Query.Text = String.Empty

			ClsDataDetail.strAllKDNr = String.Empty
			FormIsLoaded("frmKDUmsatz_LV", True)

			If Me.Cbo_VMonth_1.Text.Trim & Me.Cbo_BMonth_1.Text.Trim & Me.Cbo_VYear_1.Text.Trim = String.Empty And
					Me.Cbo_VMonth_2.Text.Trim & Me.Cbo_BMonth_2.Text.Trim & Me.Cbo_VYear_2.Text.Trim <> String.Empty Then
				DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Sie versuchen eine Vergleichsliste zu erstellen. Bitte füllen Sie auch die linken Felder aus."),
																									 m_Translate.GetSafeTranslationValue("Kundenumsatzliste"),
																									 MessageBoxButtons.OK, MessageBoxIcon.Hand)
				Me.Cbo_VMonth_1.Focus()

				Exit Sub
			End If

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			' Daten suchen...
			GetMyQueryString()

			' Daten auflisten...
			If Not FormIsLoaded("frmKDUmsatz_LV", True) Then
				frmMyLV = New frmKDUmsatz_LV(Me.txt_SQLQuery.Text, Val(Me.Cbo_VMonth_2.Text + Me.Cbo_BMonth_2.Text + Me.Cbo_VYear_2.Text + Me.Cbo_VYear_22.Text) > 0,
																		 Me.CboLstArt.Text.Contains("0 -"))

				frmMyLV.Show()
				Me.Select()
			End If

			Me.bbiPrint.Enabled = frmMyLV.RecCount > 0
			Me.bbiExport.Enabled = frmMyLV.RecCount > 0
			If frmMyLV.RecCount > 0 Then
				CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If


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
		Dim sql As String = String.Empty

		Dim _ClsDb As New ClsDbFunc

		If String.IsNullOrWhiteSpace(Me.txt_IndSQLQuery.Text) Then

			' ist die Liste mit Detials...
			If ListArt = SelectedListArt.Detail Then
				ClsDataDetail.ListAsGrouped = False
				sql = _ClsDb.GetStartEinzelnSQLString(Me)       ' 1. String
				strSort = _ClsDb.GetSortString(True, Me)       ' Sort Klausel
				sql &= ";"

			ElseIf ListArt = SelectedListArt.Grouped Then
				' die Liste wird Gruppiert...
				ClsDataDetail.ListAsGrouped = True
				ClsDataDetail.strAllKDNr = String.Empty

				DeleteAllRecinUmsatzDb()

				sql = _ClsDb.GetStartGroupedSQLString(Me, False)          ' 1. String
				'Me.txt_1_Query.Text = sSql1Query
				GetGroupKDData(sql, False)

				If (Me.Cbo_VMonth_2.Text & Me.Cbo_BMonth_2.Text & Me.Cbo_VYear_2.Text).Trim <> String.Empty Then
					sql = _ClsDb.GetStartGroupedSQLString(Me, True)        ' 1. String
					GetGroupKDData(sql, True)
				End If

				strSort = _ClsDb.GetSortString(False, Me)      ' Sort Klausel
				sql = _ClsDb.GetKDGroupString4Listing() & ";"

			End If

			sql &= String.Format(" Select * From _KDUmsatz_{0} KDUms {1}", m_InitializationData.UserData.UserNr, strSort)

			FinalSQLQuery = String.Format(" Select * From _KDUmsatz_{0} KDUms {1}", m_InitializationData.UserData.UserNr, strSort)

		Else
			ClsDataDetail.ListAsGrouped = False
			sql = Me.txt_IndSQLQuery.Text

			FinalSQLQuery = sql

		End If

		Me.txt_SQLQuery.Text = sql

		_ClsDb.GetJobNr4Print(Me)

		Return True
	End Function


#Region "Rest von Controls..."

	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		Dim strText As String = Me.CboSort.Text
		Dim strLstArt As String = Me.CboLstArt.Text
		FinalSQLQuery = String.Empty

		FormIsLoaded("frmKDUmsatz_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")
		ResetAllTabEntries()

		Me.CboSort.Text = strText
		Me.CboLstArt.Text = strLstArt

	End Sub

	Private Sub ResetAllTabEntries()
		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabMain").Controls
			For Each ctrls In tabPg.Controls
				ResetControl(ctrls)
			Next
		Next
	End Sub

	Private Sub ResetControl(ByVal con As Control)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try

			If con.Name.ToLower = "cbosort" Or con.Name.ToLower = "cbolstart" Then
				'Trace.WriteLine(String.Format("{0}: {1} | {2}", con.Name, con.GetType, con.Text))
				Exit Sub
			Else
				'Trace.WriteLine(String.Format("{0}: {1} | {2}", con.Name, con.GetType, con.Text))
			End If

			If con.Enabled Then
				Trace.WriteLine(con.Name)
				If TypeOf (con) Is TextBox Then
					Dim tb As TextBox = con
					tb.Text = String.Empty

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

				ElseIf TypeOf (con) Is ListBox Then
					Dim lst As ListBox = con
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

#End Region

#Region "KeyDown für Lst und Textfelder..."


	Private Sub txtKDNr_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDNr_1.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDNr_1_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtKDNr_2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDNr_2.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDNr_2_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtKDNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDNr_1.ButtonClick
		If e.Button.Index = 0 Then
			Dim frmTest As New frmSearchRec(m_InitializationData, "Customer")
			Dim strModulName As String = String.Empty

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iKDValue()
			Me.txtKDNr_1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(m_Seprator, ",")))
			frmTest.Dispose()
		End If

	End Sub

	Private Sub txtKDNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDNr_2.ButtonClick

		If e.Button.Index = 0 Then
			Dim frmTest As New frmSearchRec(m_InitializationData, "Customer")
			Dim strModulName As String = String.Empty

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iKDValue()
			Me.txtKDNr_2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(m_Seprator, ",")))
			frmTest.Dispose()
		End If

	End Sub


#End Region


	Private Sub txtKDNr_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtKDNr_1.KeyPress, txtKDNr_2.KeyPress, txtKDPLZ_1.KeyPress, txtKDOrt_1.KeyPress, txtKDLand_1.KeyPress, Cbo_KDKanton.KeyPress, Cbo_VMonth_1.KeyPress, Cbo_BMonth_1.KeyPress, Cbo_VYear_1.KeyPress, Cbo_VMonth_2.KeyPress, Cbo_BMonth_2.KeyPress, Cbo_VYear_2.KeyPress, Cbo_REKst1.KeyPress, Cbo_REKst2.KeyPress, Cbo_REKst3.KeyPress
		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub
	Private Sub CboLstArt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboLstArt.SelectedIndexChanged

		If Val(Me.CboLstArt.Text) = 0 Then

			Me.Cbo_VMonth_2.Text = String.Empty
			Me.Cbo_BMonth_2.Text = String.Empty
			Me.Cbo_VYear_2.Text = String.Empty

			Me.GroupBox2.Enabled = False
		Else
			Me.GroupBox2.Enabled = True

		End If

	End Sub


#Region "SwitchBoxen..."

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtKDNr_2.Visible = Me.SwitchButton1.Value
		Me.txtKDNr_2.Text = String.Empty
	End Sub


#End Region





	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		GetKDData4Print(False, ClsDataDetail.GetModulToPrint())

		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		'popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Liste drucken#mnuListPrint", _
																					 "Entwurfsansicht#PrintDesign"}
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
				Dim captionLbl = myValue(0).ToString
				bshowMnu = Not String.IsNullOrWhiteSpace(captionLbl)

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					If captionLbl.StartsWith("-") Then captionLbl = captionLbl.Substring(1, captionLbl.Length - 1)
					captionLbl = m_Translate.GetSafeTranslationValue(captionLbl)
					itm.Caption = captionLbl

					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
					If myValue(0).ToString.ToLower.StartsWith("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Me.SQL4Print = String.Empty

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		Select Case e.Item.Name.ToUpper
			Case "mnuListPrint".ToUpper
				Me.bPrintAsDesign = False
				GetKDData4Print(False, ClsDataDetail.GetModulToPrint())

			Case "PrintDesign".ToUpper
				Me.bPrintAsDesign = True
				GetKDData4Print(False, ClsDataDetail.GetModulToPrint())


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
		Dim sql As String = String.Empty

		Select Case e.Item.Name.ToUpper

			Case UCase("KD_CSV")
				sql &= "Select DISTINCT(E.KDNr), KD.Firma1, KD.Postfach, "
				sql &= "(Case KD.KD_Mail_Mailing "
				sql &= "When 0 Then KD.eMail Else '' End) As KDeMail, "

				sql &= "KD.Strasse AS KDStrasse, KD.Land AS KDLand, KD.Plz AS KDPLZ, KD.Ort AS KDOrt, "

				If ListArt = SelectedListArt.Grouped Then
					sql &= "E.fBetragInk, E.sBetragInk "
				Else
					sql &= "SUM(E.fBetragInk) As fBetragInk, SUM(E.sBetragInk) As sBetragInk "
				End If

				sql &= "From _KDUmsatz_{0} E "
				sql &= "LEFT JOIN dbo.Kunden KD ON E.KDnr = KD.KDnr "

				If ListArt = SelectedListArt.Detail Then
					sql &= "GROUP BY E.KDNr, KD.Firma1, KD.Postfach, KD.KD_Mail_Mailing, KD.eMail, KD.Strasse, KD.Land, KD.Plz, KD.Ort "
				End If

				sql &= "Order by KD.Firma1"

				sql = String.Format(sql, ClsDataDetail.UserData.UserNr)

				StartExportKDUmsatzForAdressModul(sql)

			Case UCase("CSV")
				sql &= FinalSQLQuery

				sql = String.Format(sql, ClsDataDetail.UserData.UserNr)

				StartExportKDUmsatzModul(sql)



			Case Else
				Exit Sub

		End Select


	End Sub

	Sub StartExportKDUmsatzForAdressModul(ByVal sql As String)
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																																					 .SQL2Open = sql, _
																																					 .ModulName = "KDUmsatzGrouped4Adress"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromKDUmsatzForKDMailing(sql)

	End Sub

	Sub StartExportKDUmsatzModul(ByVal sql As String)
		Dim selModulName As String = String.Format("KDUmsatz_{0}_Liste", If(ListArt = SelectedListArt.Detail, "Detail", "Grouped"))
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																																					 .SQL2Open = sql, _
																																					 .ModulName = selModulName}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromKDUmsatzListing(sql)

	End Sub


End Class

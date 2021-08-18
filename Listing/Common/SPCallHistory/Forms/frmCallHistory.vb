
Option Strict Off

Imports System.Reflection.Assembly
Imports DevExpress.XtraEditors.Controls
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports System.Threading

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging

Public Class frmCallHistory
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Protected m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Protected m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private _ClsFunc As New ClsDivFunc

	Private m_xml As New ClsXML
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private m_md As Mandant

	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmCallHistory_LV

	Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI


#Region "Private properties"

	Private Property PrintJobNr As String
	Private Property SQL4Print As String

	Private ReadOnly Property GetJobNr4Print() As String
		Get
			If Me.XtraTabControl1.SelectedTabPage Is xtabAllgemein Then
				Return "17.0"   ' Call-Historyliste
			Else
				If Me.sbKontaktDb.Value Then
					Return "2.1.4"   ' Kunden-Kontaktliste

				Else
					Return "1.1.2"  ' Kandidaten-Kontaktliste

				End If

			End If

		End Get
	End Property

#End Region


#Region "Constructor..."

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_md = New Mandant
		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

		InitializeComponent()

		Me.XtraTabControl1.TabPages.Remove(xtabAllgemein)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Reset()

	End Sub

#End Region

	Private ReadOnly Property GetHwnd() As String
		Get
			Return CStr(Me.Handle)
		End Get
	End Property



	Private Sub Reset()

		ResetMandantenDropDown()

	End Sub

#Region "Lookup Edit Reset und Load..."


	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.Items.Clear()

		Dim _ClsFunc As New ClsDbFunc
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		lueMandant.Properties.DataSource = Data

	End Sub

	' Mandantendaten...
	Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

		Dim bSetToDefault As Boolean = False

		If String.IsNullOrWhiteSpace(Me.lueMandant.Properties.GetCheckedItems) Then Return

		If ClsDataDetail.MDData.MultiMD = 0 AndAlso Me.lueMandant.EditValue.ToString.Contains(",") Then
			m_UtilityUI.ShowInfoDialog("Es kann nur aus einer Mandant gesucht werden. Ich wähle den Hauptmandant.")

			bSetToDefault = True

		End If
		If Me.lueMandant.EditValue.ToString.Contains(",") Then bSetToDefault = True

		If Not bSetToDefault Then
			Dim SelectedData = lueMandant.Properties.GetItems.GetCheckedValues(0)

			If Not SelectedData Is Nothing Then
				ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
				ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserNr)

				bSetToDefault = False

			Else
				bSetToDefault = True

			End If

		Else
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)

		End If
		ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		m_InitializationData = ClsDataDetail.m_InitialData





		'Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)
		'If Not SelectedData Is Nothing Then
		'	ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
		'	ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName)

		'Else
		'	ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
		'	ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)

		'End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub


#End Region



#Region "Dropdown Funktionen Allgmeine"

	Private Sub CboSort_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		ListSort(CboSort)
	End Sub

	Private Sub CboContactSort_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboContactSort.QueryPopUp

		If Me.sbKontaktDb.Value Then
			ListCustomerContactSort(cboContactSort)
		Else
			ListEmployeeContactSort(cboContactSort)
		End If

	End Sub

	Private Sub Cbo_LstArt_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_LstArt.QueryPopUp
		ListArt(Cbo_LstArt)
	End Sub

	Private Sub Cbo_Berater_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		ListBerater(Me.Cbo_Berater)
	End Sub

	Private Sub Cbo_MA_Kst_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MA_KST.QueryPopUp

		If Me.sbKontaktDb.Value Then
			ListCustomerCreatedFrom(Me.Cbo_MA_KST)
		Else
			ListEmployeeCreatedFrom(Me.Cbo_MA_KST)
		End If

	End Sub

	Private Sub Cbo_MA_Art_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MA_Art.QueryPopUp
		ListKontaktArt(Me.Cbo_MA_Art)
	End Sub


#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmCallHistory_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Me.Timer1.Enabled = False
		FormIsLoaded("frmCallHistory_LV", True)
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidth = Me.Width
				My.Settings.ifrmHeight = Me.Height

				My.Settings.SortBez1 = Me.CboSort.Text
				My.Settings.SortBez2 = Me.cboContactSort.Text
				My.Settings.ListArt = Me.Cbo_LstArt.Text

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmCallHistory_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmCallHistory_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Private Sub frmCallHistory_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Me.Timer1.Enabled = False
	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		Try
			Me.Text = m_xml.GetSafeTranslationValue(Me.Text)

			Me.CmdClose.Text = m_xml.GetSafeTranslationValue(Me.CmdClose.Text)
			Me.lblHeader1.Text = m_xml.GetSafeTranslationValue(Me.lblHeader1.Text)
			Me.lblHeader2.Text = m_xml.GetSafeTranslationValue(Me.lblHeader2.Text)

			Me.sbKontaktDb.OnText = m_xml.GetSafeTranslationValue(Me.sbKontaktDb.OnText)
			Me.sbKontaktDb.OffText = m_xml.GetSafeTranslationValue(Me.sbKontaktDb.OffText)

			Me.xtabAllgemein.Text = m_xml.GetSafeTranslationValue(Me.xtabAllgemein.Text)
			Me.xtabKontakt.Text = m_xml.GetSafeTranslationValue(Me.xtabKontakt.Text)
			Me.xtabErweitert.Text = m_xml.GetSafeTranslationValue(Me.xtabErweitert.Text)
			Me.xtabSQLQuery.Text = m_xml.GetSafeTranslationValue(Me.xtabSQLQuery.Text)

			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue(Me.bsiInfo.Caption)
			Me.bbiSearch.Caption = m_xml.GetSafeTranslationValue(Me.bbiSearch.Caption)
			Me.bbiClear.Caption = m_xml.GetSafeTranslationValue(Me.bbiClear.Caption)
			Me.bbiPrint.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrint.Caption)
			Me.bbiExport.Caption = m_xml.GetSafeTranslationValue(Me.bbiExport.Caption)

			Me.lblMDName.Text = m_xml.GetSafeTranslationValue(Me.lblMDName.Text)
			Me.lblSort1.Text = m_xml.GetSafeTranslationValue(Me.lblSort1.Text)
			Me.lbllistart.Text = m_xml.GetSafeTranslationValue(Me.lbllistart.Text)
			Me.lblBerater.Text = m_xml.GetSafeTranslationValue(Me.lblBerater.Text)
			Me.lblperiode.Text = m_xml.GetSafeTranslationValue(Me.lblperiode.Text)
			Me.lblDatumzwischen.Text = m_xml.GetSafeTranslationValue(Me.lblDatumzwischen.Text)

			Me.lblSort2.Text = m_xml.GetSafeTranslationValue(Me.lblSort2.Text)
			Me.lblkontaktart.Text = m_xml.GetSafeTranslationValue(Me.lblkontaktart.Text)
			Me.lblberater2.Text = m_xml.GetSafeTranslationValue(Me.lblberater2.Text)
			Me.lblperiode2.Text = m_xml.GetSafeTranslationValue(Me.lblperiode2.Text)
			Me.lbldatumzwischen2.Text = m_xml.GetSafeTranslationValue(Me.lbldatumzwischen2.Text)
			Me.lblbezeichnung.Text = m_xml.GetSafeTranslationValue(Me.lblbezeichnung.Text)

			Me.lblAbfrage.Text = m_xml.GetSafeTranslationValue(Me.lblAbfrage.Text)
			Me.lblderzeitigeabfrage.Text = m_xml.GetSafeTranslationValue(Me.lblderzeitigeabfrage.Text)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.ToString))
		End Try

	End Sub

	Private Sub frmCallHistory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		SetDefaultSortValues()
		Try
			Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
			Me.Invoke(UpdateDelegate)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.ToString))

		End Try
		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, ClsDataDetail.ProgSettingData.LogedUSNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				If My.Settings.frmLocation <> String.Empty Then
					Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
					Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
					Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))

					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.ToString))

		End Try

		If ClsDataDetail.UserData.UserNr <> 1 Then
			Me.XtraTabControl1.TabPages.Remove(Me.xtabTODO)
			Me.XtraTabControl1.TabPages.Remove(Me.xtabErweitert)
			Me.XtraTabControl1.TabPages.Remove(Me.xtabSQLQuery)
		End If
		Me.LblTimeValue.Visible = CBool(CInt(ClsDataDetail.UserData.UserNr) = 1)

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim strSort As String = My.Settings.SortBez1
			If strSort = String.Empty Then strSort = String.Format("0 - {0}", m_xml.GetSafeTranslationValue("BeraterIn"))
			Me.CboSort.Text = strSort

			' Listenart
			Dim strListArt As String = My.Settings.ListArt
			If strListArt = String.Empty Then strListArt = String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Alle Anrufe"))
			Me.Cbo_LstArt.Text = strListArt

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim strSort As String = My.Settings.SortBez2
			If strSort = String.Empty Then strSort = String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Firmenname"))
			Me.cboContactSort.Text = strSort

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.ToString))

		End Try

		Try
			Me.lueMandant.SetEditValue(ClsDataDetail.ProgSettingData.SelectedMDNr)
			Dim showMDSelection As Boolean = IsUserActionAllowed(ClsDataDetail.ProgSettingData.LogedUSNr, 642, ClsDataDetail.ProgSettingData.SelectedMDNr)
			Me.lueMandant.Visible = showMDSelection
			Me.lblMDName.Visible = showMDSelection

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.ToString))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

		SetBeraterValues()

	End Sub

	Sub SetBeraterValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_common As New CommonSetting

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(ClsDataDetail.UserData.UserNr, 672, ClsDataDetail.MDData.MDNr) Then
			Try
				Me.Cbo_Berater.Enabled = False
				Me.Cbo_MA_KST.Enabled = False

				Dim strUSTitle As String = String.Format("{0}|{1}", ClsDataDetail.UserData.UserFTitel, ClsDataDetail.UserData.UserSTitel)
				Dim IsLeiter As Boolean = True
				IsLeiter = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")

				Me.Cbo_Berater.Enabled = IsLeiter
				Me.Cbo_MA_KST.Enabled = IsLeiter

				Dim usName = String.Format("{0} {1}", ClsDataDetail.UserData.UserFName, ClsDataDetail.UserData.UserLName)
				Cbo_Berater.Properties.Items.Clear()

				Cbo_Berater.Properties.Items.Add(New ComboValue(usName, usName))
				Me.Cbo_Berater.SelectedIndex = 0
				Me.Cbo_Berater.Refresh()

				Cbo_MA_KST.Properties.Items.Clear()
				Cbo_MA_KST.Properties.Items.Add(New ComboValue(usName, usName))
				Me.Cbo_MA_KST.SelectedIndex = 0
				Me.Cbo_MA_KST.Refresh()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.BenutzerTitel auflisten: {1}", strMethodeName, ex.ToString))
			End Try

		End If

	End Sub

	Private Sub frmESSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And ClsDataDetail.UserData.UserNr = 1 Then
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

	'''' <summary>
	'''' Daten fürs Drucken bereit stellen.
	'''' </summary>
	'''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	'''' <param name="bForExport">ob die Liste für Export ist.</param>
	'''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	'''' <remarks></remarks>
	'Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'	Dim iESNr As Integer = 0
	'	Dim bResult As Boolean = True
	'	Dim storedProc As String = String.Empty

	'	Dim sSql As String = Me.txt_SQLQuery.Text
	'	If sSql = String.Empty Then
	'		MsgBox("Keine Suche wurde gestartet!.", MsgBoxStyle.Exclamation, "GetData4Print")
	'		Exit Sub
	'	End If

	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

	'	If bForDesign Then sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ")

	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

	'	Try
	'		Conn.Open()

	'		Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader                  ' 
	'		Try
	'			If Not rFoundedrec.HasRows Then
	'				cmd.Dispose()
	'				rFoundedrec.Close()

	'				MessageBox.Show("Ich konnte leider Keine Daten finden.", "GetData4Print",
	'												MessageBoxButtons.OK, MessageBoxIcon.Information)
	'			End If

	'		Catch ex As Exception
	'			MsgBox(ex.ToString.ToString, MsgBoxStyle.Critical, "GetData4Print")

	'		End Try
	'		rFoundedrec.Read()
	'		If rFoundedrec.HasRows Then
	'			Me.SQL4Print = sSql
	'			Me.PrintJobNr = strJobInfo

	'			StartPrinting()

	'		End If
	'		rFoundedrec.Close()

	'	Catch ex As Exception
	'		MsgBox(ex.ToString, MsgBoxStyle.Critical, "GetData4Print")

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try
	'End Sub


#Region "Funktionen zur Menüaufbau..."


	Private Sub OnbbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		'Dim strModulNr As String = "0"
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)

		Try
			Me.txt_SQLQuery.Text = String.Empty

			Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Nach Daten wird gesucht") & "..."

			FormIsLoaded("frmCallHistory_LV", True)
			If ClsDataDetail.GetLVSortBez <> String.Empty Then Me.CboSort.Text = ClsDataDetail.GetLVSortBez

			' Die Query-String aufbauen...
			GetMyQueryString()

			Me.Timer1.Enabled = True
			Me.Timer1.Interval = 100

			Stopwatch.Reset()
			Stopwatch.Start()

			' Daten auflisten...
			If Not FormIsLoaded("frmCallHistory_LV", True) Then
				' Callhistory
				'strModulNr = Me.XtraTabControl1.SelectedTabPageIndex + If(Me.sbKontaktDb.Value, 1, 0)

				If Me.XtraTabControl1.SelectedTabPage Is Me.xtabAllgemein Then
					frmMyLV = New frmCallHistory_LV(Me.txt_SQLQuery.Text, frmCallHistory_LV.ListArt.CallDb)


				ElseIf Me.XtraTabControl1.SelectedTabPage Is Me.xtabTODO Then
					' to-do Liste
					If Me.sbKontaktDb.Value Then
						frmMyLV = New frmCallHistory_LV(Me.txt_SQLQuery.Text, frmCallHistory_LV.ListArt.customer)
					Else
						frmMyLV = New frmCallHistory_LV(Me.txt_SQLQuery.Text, frmCallHistory_LV.ListArt.employee)
					End If

				Else 'If Me.XtraTabControl1.SelectedTabPage Is Me.xtabKontakt Then
					' Kontaktliste
					If Me.sbKontaktDb.Value Then
						frmMyLV = New frmCallHistory_LV(Me.txt_SQLQuery.Text, frmCallHistory_LV.ListArt.customer)
					Else
						frmMyLV = New frmCallHistory_LV(Me.txt_SQLQuery.Text, frmCallHistory_LV.ListArt.employee)
					End If
				End If

				frmMyLV.Visible = False
				frmMyLV.Show()
				frmMyLV.Visible = True
				Me.Select()
			End If

			Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet"),
																				 frmMyLV.RecCount.ToString)
			frmMyLV.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet"),
																							frmMyLV.RecCount.ToString)

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				'CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If


		Catch ex As Exception
			MessageBox.Show(ex.ToString, "bbiSearch_Click")

		Finally
			Me.Timer1.Enabled = False
		End Try

	End Sub

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim sSqlSelect As String = String.Empty
		Dim strKontaktTable As String = String.Empty

		Dim _ClsDb As New ClsDbFunc
		If Me.txt_IndSQLQuery.Text = String.Empty Then


			Dim result = lueMandant.Properties.GetItems.GetCheckedValues()
			For Each itm In result
				If CInt(Val(itm.ToString)) > 0 Then _ClsDb.mandantNumber.Add(CInt(itm.ToString))
			Next

			' Callhistory
			If Me.XtraTabControl1.SelectedTabPage Is xtabAllgemein Then
				sSql1Query = _ClsDb.GetStartSQLString()
				sSql2Query = _ClsDb.GetQuerySQLString(Me)
				If Trim(sSql2Query) <> String.Empty Then sSql1Query &= " Where "

				strSort = _ClsDb.GetSortString(Me)
				sSqlSelect = " Select * From ##CallDb "

				' KD-Kontakte
			Else 'If Me.XtraTabControl1.SelectedTabPage Is xtabKontakt Then
				If Me.sbKontaktDb.Value Then
					sSql1Query = _ClsDb.GetStartSQLString4KD_Kontakt()
					sSql2Query = _ClsDb.GetQuerySQLString4KD_Kontakt(Me)
					If Trim(sSql2Query) <> String.Empty Then sSql1Query += " Where "

					strSort = _ClsDb.GetSortString4KD_Kontakt(Me)
					sSqlSelect = " Select * From ##KDKontaktDb "

					' Kandidaten-Kontakte
				Else
					sSql1Query = _ClsDb.GetStartSQLString4MA_Kontakt()
					sSql2Query = _ClsDb.GetQuerySQLString4MA_Kontakt(Me)
					If Trim(sSql2Query) <> String.Empty Then sSql1Query += " Where "

					strSort = _ClsDb.GetSortString4MA_Kontakt(Me)
					sSqlSelect = " Select * From ##MAKontaktDb "

				End If

			End If

			Me.txt_SQLQuery.Text = sSql1Query + sSql2Query + sSqlSelect + strSort
			strLastSortBez = strSort

		Else

			Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
		End If

		Return True
	End Function

	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmCallHistory_LV", True)
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

		deCallAt_1.Enabled = True
		deCallAt_2.Enabled = True
		deMAKontakt_1.Enabled = True
		deMAKontakt_2.Enabled = True

		ResetAllTabEntries()

		Me.CboSort.Text = strText   ' Letzte Sortierung wieder herstellen
		Me.txt_SQLQuery.Text = m_xml.GetSafeTranslationValue("Wurde geleert") & "..."

		SetBeraterValues()

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.XtraTabControl1.Name).Controls
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

		If con.Name.ToLower.Contains("cbosort") OrElse con.Name.ToLower.Contains("luemandant") OrElse con.Name.ToString.ToLower.Contains("cbo_lstart") Then Exit Sub

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

			ElseIf TypeOf (con) Is System.Windows.Forms.ComboBox Then
				Dim cbo As System.Windows.Forms.ComboBox = con
				cbo.Text = String.Empty
				cbo.SelectedIndex = -1

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
				cbo.Properties.Items.Clear()
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = con
				cbo.Properties.Items.Clear()
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is CheckBox Then
				Dim cbo As CheckBox = con
				cbo.CheckState = CheckState.Unchecked

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckEdit = con
				cbo.CheckState = CheckState.Unchecked

			ElseIf TypeOf (con) Is ListBox Then
				Dim lst As ListBox = con
				lst.Items.Clear()

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = con
				tb.Text = String.Empty


			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl Then
				Dim lst As DevExpress.XtraEditors.ListBoxControl = con
				lst.Items.Clear()

			Else
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next

			End If
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


	''' <summary>
	''' Klick-Event der Controls auffangen und verarbeiten
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles Cbo_Berater.KeyPress, Cbo_ESAktivImSelektion.KeyPress, CboSort.KeyPress, Cbo_LstArt.KeyPress

		Try
			If e.KeyChar = Chr(13) Then ' Enter
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.ToString, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Private Sub CboSort_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryCloseUp
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_0", Me.CboSort.Text)
		ClsDataDetail.GetLVSortBez = String.Empty
	End Sub

	Private Sub Cbo_LstArt_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_LstArt.QueryCloseUp
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_1", Me.Cbo_LstArt.Text)
	End Sub

#Region "Perioden..."

	Private Sub Cbo_ESAktivImSelektion_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESAktivImSelektion.QueryPopUp

		If Me.Cbo_ESAktivImSelektion.Properties.Items.Count = 0 Then
			' Berechnung der Kalenderwoche (Achtung: In der letzte Woche des Jahres wird die Zahl nicht immer korrekt zurückgegeben.)
			Dim kw As Integer = DatePart(DateInterval.WeekOfYear, Date.Now, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
			' Berechnung des Monats
			Dim mon As Integer = DatePart(DateInterval.Month, Date.Now)
			' Berechnung des Jahres
			Dim jahr As Integer = DatePart(DateInterval.Year, Date.Now)

			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue("", ""))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Letzte Woche / KW {0}"), kw - 1), "LW"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Diese Woche / KW {0}"), kw), "DW"))

			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Letzten Monat ({0}{1})"), If(mon = 1, 12, mon - 1), If(mon = 1, " / " & jahr - 1, "")), "LM"))

			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Diesen Monat ({0})"), mon), "DM"))

			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Letztes Jahr ({0})"), jahr - 1), "LJ"))
			Me.Cbo_ESAktivImSelektion.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Dieses Jahr ({0})"), jahr), "DJ"))
		End If

	End Sub

	Private Sub Cbo_ESAktivImSelektion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ESAktivImSelektion.SelectedIndexChanged
		Dim selectedItem As String = Me.Cbo_ESAktivImSelektion.Text '.ToItem.Value_1
		Dim dayOfWeek As Integer = Integer.Parse(Date.Now.DayOfWeek.ToString("D"))
		Dim month As Integer = Date.Now.Month
		Dim year As Integer = Date.Now.Year

		' Eingabe von ein neuenm Datum sperren

		' Die Woche fängt mit Sonntag=0 an, somit muss eine Woche abgezogen werden,
		' falls am Sonntag die Abfrage gestartet wird.
		If dayOfWeek = 0 Then
			dayOfWeek = 7
		End If
		Dim cv As ComboValue = DirectCast(sender.selecteditem, ComboValue)
		If cv Is Nothing Then Exit Sub
		Me.deCallAt_1.Enabled = False
		Me.deCallAt_2.Enabled = False

		Select Case cv.ComboValue

			Case "LW" ' Letzte Woche
				' Ob er am Montag oder Sonntag die Daten der letzten Woche abfragt,
				' so das Wochentag berücksichtigen und abziehen
				Me.deCallAt_1.Text = Date.Now.AddDays(-6 - dayOfWeek).Date  ', "dd.MM.yyyy")
				Me.deCallAt_2.EditValue = Date.Now.AddDays(0 - dayOfWeek).Date

			Case "DW" ' Diese Woche
				Me.deCallAt_1.Text = Date.Now.AddDays(1 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deCallAt_2.Text = Date.Now.AddDays(7 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "NW" ' Nächste Woche
				Me.deCallAt_1.Text = Date.Now.AddDays(8 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deCallAt_2.Text = Date.Now.AddDays(14 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "LM" ' Letzter Monat
				If month = 1 Then
					month = 12
					year -= 1
				Else
					month -= 1
				End If
				Me.deCallAt_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deCallAt_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "DM" ' Diesen Monat
				Me.deCallAt_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deCallAt_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "NM" ' Nächsten Monat
				month += 1
				Me.deCallAt_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deCallAt_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "LJ" ' Letztes Jahr
				year -= 1
				Me.deCallAt_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deCallAt_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "DJ" ' Dieses Jahr
				Me.deCallAt_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deCallAt_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "NJ" ' Nächstes Jahr
				year += 1
				Me.deCallAt_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deCallAt_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case Else
				deCallAt_1.EditValue = Nothing
				deCallAt_2.EditValue = Nothing

				Me.deCallAt_1.Enabled = True
				Me.deCallAt_2.Enabled = True

		End Select

	End Sub

	Private Sub Cbo_MA_DateTpl_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MA_DateTpl.QueryPopUp
		If Me.Cbo_MA_DateTpl.Properties.Items.Count = 0 Then
			' Berechnung der Kalenderwoche (Achtung: In der letzte Woche des Jahres wird die Zahl nicht immer korrekt zurückgegeben.)
			Dim kw As Integer = DatePart(DateInterval.WeekOfYear, Date.Now, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
			' Berechnung des Monats
			Dim mon As Integer = DatePart(DateInterval.Month, Date.Now)
			' Berechnung des Jahres
			Dim jahr As Integer = DatePart(DateInterval.Year, Date.Now)

			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue("", ""))
			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Letzte Woche / KW {0}"), kw - 1), "LW"))
			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Diese Woche / KW {0}"), kw), "DW"))
			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Nächste Woche / KW {0}"), kw + 1), "NW"))

			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Letzten Monat ({0}{1})"), If(mon = 1, 12, mon - 1), If(mon = 1, " / " & jahr - 1, "")), "LM"))
			'Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Letzten Monat ({0})"), mon - 1), "LM"))

			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Diesen Monat ({0})"), mon), "DM"))
			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Nächsten Monat ({0})"), mon + 1), "NM"))
			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Letztes Jahr ({0})"), jahr - 1), "LJ"))
			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Dieses Jahr ({0})"), jahr), "DJ"))
			Me.Cbo_MA_DateTpl.Properties.Items.Add(New ComboValue(String.Format(m_xml.GetSafeTranslationValue("Nächstes Jahr ({0})"), jahr + 1), "NJ"))
		End If

	End Sub

	Private Sub Cbo_MA_DateTpl_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MA_DateTpl.SelectedIndexChanged
		Dim selectedItem As String = Cbo_MA_DateTpl.Text
		Dim dayOfWeek As Integer = Integer.Parse(Date.Now.DayOfWeek.ToString("D"))
		Dim month As Integer = Date.Now.Month
		Dim year As Integer = Date.Now.Year

		' Eingabe von ein neuenm Datum sperren

		' Die Woche fängt mit Sonntag=0 an, somit muss eine Woche abgezogen werden,
		' falls am Sonntag die Abfrage gestartet wird.
		If dayOfWeek = 0 Then
			dayOfWeek = 7
		End If
		Dim cv As ComboValue = DirectCast(sender.selecteditem, ComboValue)
		If cv Is Nothing Then Exit Sub
		Me.deMAKontakt_1.Enabled = False
		Me.deMAKontakt_2.Enabled = False

		Select Case cv.ComboValue
			Case "LW" ' Letzte Woche
				' Ob er am Montag oder Sonntag die Daten der letzten Woche abfragt,
				' so das Wochentag berücksichtigen und abziehen
				Me.deMAKontakt_1.Text = Date.Now.AddDays(-6 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deMAKontakt_2.Text = Date.Now.AddDays(0 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "DW" ' Diese Woche
				Me.deMAKontakt_1.Text = Date.Now.AddDays(1 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deMAKontakt_2.Text = Date.Now.AddDays(7 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "NW" ' Nächste Woche
				Me.deMAKontakt_1.Text = Date.Now.AddDays(8 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deMAKontakt_2.Text = Date.Now.AddDays(14 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "LM" ' Letzter Monat
				If month = 1 Then
					month = 12
					year -= 1
				Else
					month -= 1
				End If
				Me.deMAKontakt_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deMAKontakt_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "DM" ' Diesen Monat
				Me.deMAKontakt_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deMAKontakt_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "NM" ' Nächsten Monat
				month += 1
				Me.deMAKontakt_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deMAKontakt_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "LJ" ' Letztes Jahr
				year -= 1
				Me.deMAKontakt_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deMAKontakt_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "DJ" ' Dieses Jahr
				Me.deMAKontakt_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deMAKontakt_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "NJ" ' Nächstes Jahr
				year += 1
				Me.deMAKontakt_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deMAKontakt_2.Text = Date.Parse(String.Format("31.12.{0}", year))


			Case Else
				Me.deMAKontakt_1.EditValue = Nothing
				deMAKontakt_2.EditValue = Nothing
				Me.deMAKontakt_1.Enabled = True
				Me.deMAKontakt_2.Enabled = True

		End Select

	End Sub


#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl
		'popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

		StartPrinting()

	End Sub

	Sub StartPrinting()
		Dim showDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(ClsDataDetail.UserData.UserNr, 611, ClsDataDetail.MDData.MDNr)

		Me.SQL4Print = txt_SQLQuery.Text
		Me.PrintJobNr = GetJobNr4Print
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLCallHistoryPrintSetting With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																										 .frmhwnd = Me.Handle,
																																										 .ShowAsDesign = showDesign,
																																										 .SQL2Open = Me.SQL4Print,
																																										 .JobNr2Print = Me.PrintJobNr,
																																										 .ListSortBez = ClsDataDetail.GetSortBez,
																																										 .ListFilterBez = New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																																																																				ClsDataDetail.GetFilterBez2,
																																																																				ClsDataDetail.GetFilterBez3,
																																																																				ClsDataDetail.GetFilterBez4}),
																																										 .SelectedMDNr = ClsDataDetail.ProgSettingData.SelectedMDNr,
																																										 .LogedUSNr = ClsDataDetail.ProgSettingData.LogedUSNr}
		Dim obj As New SPS.Listing.Print.Utility.CallHistoryListing.ClsPrintCallHistoryList(_Setting)
		obj.PrintCallHistoryList(showDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub


	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Liste drucken#mnuListPrint"}
	'	If Not IsUserActionAllowed(0, 601) Then Exit Sub
	'	Try
	'		bbiPrint.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiPrint.ActAsDropDown = False
	'		Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiPrint.DropDownEnabled = True
	'		Me.bbiPrint.DropDownControl = popupMenu
	'		Me.bbiPrint.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))
	'			bshowMnu = myValue(0).ToString <> String.Empty

	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				itm.Caption = m_xml.GetSafeTranslationValue(myValue(0).ToString)
	'				itm.Name = m_xml.GetSafeTranslationValue(myValue(1).ToString)

	'				popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
	'			End If

	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

	'	End Try
	'End Sub

	'Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

	'	Me.SQL4Print = String.Empty
	'	Me.bPrintAsDesign = False

	'	Select Case e.Item.Name.ToUpper
	'		Case "mnuListPrint".ToUpper
	'			GetData4Print(False, False, Me.GetJobNr4Print)


	'		Case Else
	'			Exit Sub

	'	End Select

	'End Sub

#Region "bbExport"

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
		Dim query As String = Me.txt_SQLQuery.Text

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				StartExportESModul()


		End Select

	End Sub

	Sub StartExportESModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn,
																																					 .SQL2Open = Me.txt_SQLQuery.Text,
																																					 .ModulName = "CallHistory"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromCallhistoryListing(Me.txt_SQLQuery.Text)

	End Sub


#End Region



	Private Sub sbKontaktDb_ValueChanged(sender As System.Object, e As System.EventArgs) Handles sbKontaktDb.ValueChanged
		Dim iLastsortkey As Integer = Me.cboContactSort.SelectedIndex

		If Me.cboContactSort.Text.Contains("0 - ") Then

			If Me.sbKontaktDb.Value Then
				Me.cboContactSort.Text = String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Firmenname + Datum aufsteigend"))

			Else
				Me.cboContactSort.Text = String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Kandidatenname + Datum aufsteigend"))

			End If

		End If

	End Sub

	Private Sub Cbo_Berater_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Berater.SelectedIndexChanged

	End Sub
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
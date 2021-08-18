
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Runtime.CompilerServices
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports SPS.Listing.Print.Utility

Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports DevExpress.LookAndFeel
Imports SPLOAGSearch.ClsDataDetail
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Public Class frmLOAGSearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_ConnectionString As String

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private strValueSeprator As String = "#@"

	Dim _ClsFunc As New ClsDivFunc

	Dim strLastSortBez As String
	Dim Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmListeSearch_LV
	Public Const frmMyLVName As String = "frmListeSearch_LV"
	Dim _resetLohnarten As Boolean = False ' Lohnarten neu laden oder nicht

	Dim PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private Property TranslatedPage As New List(Of Boolean)

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI


	Private Property ModulName As String
	Private m_SearchCriteria As New SearchCriteria

	Enum Arrow As Integer
		Left
		Right
	End Enum


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()
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

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, ClsDataDetail.m_InitialData.UserData.UserNr)
			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub


#End Region


#Region "Dropdown Funktionen Allgemein"

	' Jahr
	'Private Sub ListLOJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'  Try
	'    If DirectCast(sender, myCbo).Items.Count = 0 Then ListLOJahr(sender)

	'  Catch ex As Exception ' Manager
	'    m_UtilityUi.ShowErrorDialog("ListLOJahr_DropDown", ex)
	'  End Try

	'End Sub

#End Region


#Region "Allgemeine Funktionen"

	' Monate 1 bis 12
	Private Sub Monate1bis12(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboMonate1Bis12(cbo)
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


	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmLOAGSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded(frmMyLVName, True)
		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			' Panels
			My.Settings.pnl_Nummerfeld_Height = Me.pnl_Nummerfelder.Height
			My.Settings.pnl_Berufe_Height = Me.pnl_Berufe.Height

			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmLOAGSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		Try
			If FormIsLoaded(frmMyLVName, False) Then
				frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
				frmMyLV.TopMost = True
				frmMyLV.TopMost = False
			End If
		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	Sub StartTranslation()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)
		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)

		lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)
		lblArtderListe.Text = m_Translate.GetSafeTranslationValue(lblArtderListe.Text)
		lblPeriode.Text = m_Translate.GetSafeTranslationValue(lblPeriode.Text)
		lblJahrVon.Text = m_Translate.GetSafeTranslationValue(lblJahrVon.Text)
		lblMonatVon.Text = m_Translate.GetSafeTranslationValue(lblMonatVon.Text)

		ChkNullBetrag.Text = m_Translate.GetSafeTranslationValue(ChkNullBetrag.Text)

		lblKandidatenname.Text = m_Translate.GetSafeTranslationValue(lblKandidatenname.Text)

		lblBeruf.Text = m_Translate.GetSafeTranslationValue(lblBeruf.Text)
		lbl1Kategorie.Text = m_Translate.GetSafeTranslationValue(lbl1Kategorie.Text)

		For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.XtraTabControl1.TabPages
			tbp.Text = m_Translate.GetSafeTranslationValue(tbp.Text)
		Next

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmLohnartenSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		SetInitialFields()

		Try
			If m_InitializationData.UserData.UserNr <> 1 Then Me.XtraTabPage2.PageVisible = False

			If m_InitializationData.UserData.UserNr = 1 Then
				Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
			Else
				If (IsUserAllowed4DocExport("11.0") Or IsUserAllowed4DocExport("11.3")) Then Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
			End If

			Me.LblTimeValue.Visible = m_InitializationData.UserData.UserNr = 1
			ClsDataDetail.IsFirstTapiCall = True

			'Me.mnuDesignLOANRekap.Visible = False

			' Panels
			If My.Settings.pnl_Nummerfeld_Height > 0 Then
				Me.pnl_Nummerfelder.Height = My.Settings.pnl_Nummerfeld_Height
			Else
				SetPanelHeight(Me.pnl_Nummerfelder)
			End If

			If My.Settings.pnl_Berufe_Height > 0 Then
				Me.pnl_Berufe.Height = My.Settings.pnl_Berufe_Height
			Else
				SetPanelHeight(Me.pnl_Berufe)
			End If
			RefreshAllPanels()

			FillDefaultValues()
			Me.Text = String.Format(m_Translate.GetSafeTranslationValue("Monatliche Lohnlisten: {0}"), Me.Cbo_ArtderListe.Text)

			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		End Try


	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
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
				m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Me.lueMandant.Visible = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

		Catch ex As Exception
			m_Logger.LogError(String.Format("Mandantenauswahl anzeigen: {0}", ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

	End Sub

	Sub SetPanelHeight(ByRef Panel As Panel)
		Try
			Panel.Height = ClsDataDetail.GetPanelHeight(Panel)
		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	''' <summary>
	''' Öffnet oder schliesst ein Panel auf die maximale bzw. minimale Höhe.
	''' </summary>
	''' <param name="pnl">Das Panel, das geöffnet oder geschlossen werden soll.</param>
	''' <param name="open">Soll das Panel geöffnet werden oder geschlossen. True = Panel öffnen.</param>
	''' <param name="stepInterval">In welchen Schritten das Panel geöffnet bzw. geschlossen werden soll.</param>
	''' <param name="minHeight">Bis auf welcher Höche das Panel geschlossen werden soll.</param>
	''' <remarks></remarks>
	Sub TogglePanel(ByRef pnl As Panel, Optional ByVal open As Boolean = True, Optional ByVal stepInterval As Integer = 5,
									Optional ByVal minHeight As Integer = 14)
		Try
			Dim maxHeight As Integer = ClsDataDetail.GetPanelHeight(pnl)

			If Not open Then
				stepInterval *= -1
				maxHeight = minHeight
				minHeight = ClsDataDetail.GetPanelHeight(pnl)
			End If

			For tempHeight As Integer = minHeight To maxHeight Step stepInterval
				pnl.Height = tempHeight
				pnl.Refresh()
				RefreshAllPanels()
				System.Windows.Forms.Application.DoEvents()
			Next
			pnl.Height = maxHeight
		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			' ART DER LISTE
			If Me.Cbo_ArtderListe.Properties.Items.Count = 0 Then
				ListCboArtderListe(Me.Cbo_ArtderListe)
			End If
			If ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Arbeitgeberlohnarten Then
				Me.Cbo_ArtderListe.SelectedIndex = 0
			Else
				Me.Cbo_ArtderListe.SelectedIndex = 1
			End If

			' DATUM ---------------------------------
			FillDefaultDates()

			' LOHNARTEN -----------------------------
			'If Me.Cbo_Lohnart.Items.Count = 0 Then
			'  ListLohnarten(Me.Cbo_Lohnart, Me.Cbo_JahrVon.Text, Me.Cbo_JahrVon.Text, Me.Cbo_JahrVon.Text, _
			'                Me.Cbo_MonatVon.Text, Me.Cbo_Beruf.Text, Me.Cbo_1Kategorie.Text, Me.txt_MANr.Text)
			'  ' Andernfalls gibt es eine Verzögerung während dem ersten Wechsel der List-Art
			'  Me.Cbo_Lohnart.CheckAll()
			'End If
		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try


	End Sub

	Private Sub FillDefaultDates()
		Try
			' DATUM ---------------------------------
			' Dropdown von Monat und Jahr müssen vorbelegt sein.
			If Me.Cbo_MonatVon.Properties.Items.Count = 0 Then
				ListCboMonate1Bis12(Me.Cbo_MonatVon)
				ListCboMonate1Bis12(Me.Cbo_MonatBis)
				ListLOJahr(Me.Cbo_JahrVon)
				ListLOJahr(Me.Cbo_JahrBis)
			End If
			' Bis am 10. des Monats wird der Vormonat selektiert. Ab 11. den aktuellen Monat
			Dim datum As Date = Date.Now
			If datum.Day < 11 Then
				datum = datum.AddMonths(-1)
			End If
			Me.Cbo_MonatVon.Text = datum.Month.ToString
			Me.Cbo_MonatBis.Text = ""
			Me.Cbo_JahrBis.Text = ""
			Me.Cbo_JahrVon.Text = datum.Year
			Me.Cbo_MonatVon.Text = datum.Month

		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try


	End Sub

	''' <summary>
	''' Die Positionen der Panels müssen untereinander sich befinden.
	''' </summary>
	''' <remarks></remarks>
	Sub RefreshAllPanels()

		Me.pnl_Berufe.Top = Me.pnl_Nummerfelder.Top + Me.pnl_Nummerfelder.Height + 20

	End Sub

	'''' <summary>
	'''' Daten fürs Drucken bereit stellen.
	'''' </summary>
	'''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	'''' <param name="bForExport">ob die Liste für Export ist.</param>
	'''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	'''' <remarks></remarks>
	'Sub Get4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'	Dim iESNr As Integer = 0
	'	Dim bResult As Boolean = True
	'	Dim storedProc As String = ""

	'	Dim sSql As String = Me.txt_SQLQuery.Text
	'	If sSql = String.Empty Then
	'		MsgBox(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"), MsgBoxStyle.Exclamation, "Get4Print")
	'		Exit Sub
	'	End If

	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
	'	Dim sqlSelectText As String = ""
	'	If ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Arbeitgeberlohnarten Then
	'		sqlSelectText = String.Format("SELECT * FROM {0} ORDER BY Nachname ASC, Vorname ASC, Jahr ASC, LP ASC, LANR ASC" _
	'																					, ClsDataDetail.LLTablename)
	'	Else
	'		sqlSelectText = String.Format("SELECT * FROM {0} ORDER BY Jahr ASC, LP ASC, LANR ASC", ClsDataDetail.LLTablename)
	'	End If
	'	Dim cmd As New SqlCommand(sqlSelectText, Conn)

	'	Try
	'		Conn.Open()

	'		If bForDesign Then
	'			sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ", 1, 1)
	'		Else
	'			' Für die Fortschrittsanzeige im LL
	'			ClsDataDetail.AnzMax = 0
	'			Dim countReader As SqlDataReader = cmd.ExecuteReader
	'			While countReader.Read
	'				ClsDataDetail.AnzMax += 1
	'			End While
	'			countReader.Close()
	'		End If

	'		Dim rec As SqlDataReader = cmd.ExecuteReader									' 
	'		Try
	'			If Not rec.HasRows Then
	'				cmd.Dispose()
	'				rec.Close()

	'				MessageBox.Show(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."), _
	'												"Get4Print", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'			End If

	'		Catch ex As Exception
	'			MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Get4Print")

	'		End Try
	'		rec.Read()
	'		If rec.HasRows Then
	'			Me.SQL4Print = sSql
	'			Me.bPrintAsDesign = bForDesign
	'			Me.PrintJobNr = strJobInfo

	'			PrintListingThread = New Thread(AddressOf StartPrinting)
	'			PrintListingThread.Name = "PrintingLOAGListing"
	'			PrintListingThread.SetApartmentState(ApartmentState.STA)
	'			PrintListingThread.Start()

	'		End If
	'		rec.Close()

	'	Catch ex As Exception	' Manager
	'		m_UtilityUi.ShowErrorDialog(ex.ToString)

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try
	'End Sub

	Sub StartPrinting()
		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)
		Dim strFilter As String = String.Empty

		Me.PrintJobNr = GetJobNr()
		If ModulName = "loag" Then
			Me.SQL4Print = Me.txt_SQLQuery.Text
		ElseIf ModulName = "loagrekap" Then
			Me.SQL4Print = String.Format("Select * From {0} Order By LANr", ClsDataDetail.LLTablename)
		End If

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine
		strFilter &= If(m_SearchCriteria.lohnarten.Length > 0, String.Format("Lohnarten: {0}", m_SearchCriteria.lohnarten, vbNewLine), String.Empty)

		strFilter &= If(m_SearchCriteria.vonmonat.Length > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.vonmonat, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.vonjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.vonjahr, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.bismonat.Length > 0, String.Format(" - {0}", m_SearchCriteria.bismonat), String.Empty)
		strFilter &= If(m_SearchCriteria.bisjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.bisjahr), String.Empty)

		strFilter &= If(m_SearchCriteria.manr.Length > 0, String.Format("{1}Kandidaten: {0}", m_SearchCriteria.manr, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.gavberuf.Length > 0, String.Format("{1}GAV-Beruf: {0}", m_SearchCriteria.gavberuf, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.gav1kategorie.Length > 0, String.Format("{1}GAV Kategorie: {0}", m_SearchCriteria.gav1kategorie, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.deletenull, String.Format("{0}Nullbeträge ausblenden? Ja", vbNewLine), String.Empty)


		Dim _Setting As New ClsLLLOAGSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																													.SelectedMDNr = m_InitializationData.MDData.MDNr,
																													.SQL2Open = Me.SQL4Print,
																													.JobNr2Print = Me.PrintJobNr,
																													.ListBez2Print = m_SearchCriteria.listname,
																													.ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)
																																																						})}
		Dim obj As New LOAGSearchListing.ClsPrintLOAGSearchList(_Setting)

		obj.PrintLOAGSearchList(Me.bPrintAsDesign, ClsDataDetail.GetSortBez, Nothing)


	End Sub


#Region "Funktionen zur Menüaufbau..."
	'Private Sub mnuListeDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

	'	Get4Print(False, False, GetJobNr)	' Arbeitgeberlohnarten, Arbeitgeberlohnarten gruppiert oder AG-LANR-Rekapitulation

	'End Sub

	'Private Sub mnuDesignListeLOAG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	Get4Print(True, False, GetJobNr) ' Arbeitgeberlohnarten (Design)
	'End Sub

	Function GetJobNr() As String
		Dim jobnr As String = If(Me.Cbo_ArtderListe.SelectedIndex = 0, "11.2", "11.1")

		' Wenn die Lister der Arbeitgeberlohnarten über meherere Monate, so die gruppierte Liste
		If jobnr = "11.2" Then 'And ((Me.Cbo_JahrVon.Text <> Me.Cbo_JahrBis.Text And _
			'Me.Cbo_JahrBis.Text.Length > 0 And Me.Cbo_JahrBis.Visible) Or _
			'(Me.Cbo_MonatVon.Text <> Me.Cbo_MonatBis.Text And _
			'Me.Cbo_MonatBis.Text.Length > 0) And Me.Cbo_JahrBis.Visible or Me.Cbo_MonatBis.Visible) Then
			If Me.Cbo_JahrBis.Visible Or Me.Cbo_MonatBis.Visible Then jobnr += ".G"
		End If

		Return jobnr
	End Function

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)
		Me.Cursor = Cursors.WaitCursor

		Try
			If Me.Cbo_MonatVon.Text = String.Empty Then Me.Cbo_MonatVon.Text = Month(Now)
			If Me.Cbo_JahrVon.Text = String.Empty Then Me.Cbo_JahrVon.Text = Year(Now)

			m_SearchCriteria = GetSearchKrieteria()

			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQLQuery.Text = String.Empty
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded(frmMyLVName, True)

			' Die Query-String aufbauen...
			GetMyQueryString()


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Fehler aufgetreten") & "..."

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Try
			Dim meldung As String = ""
			Dim monatBis As String = Me.Cbo_MonatVon.Text
			Dim jahrBis As String = Me.Cbo_JahrVon.Text

			If ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Arbeitgeberlohnarten Then
				If String.IsNullOrWhiteSpace(m_SearchCriteria.lohnarten) Then
					meldung += String.Format(m_Translate.GetSafeTranslationValue("Die Liste der Mitarbeiterlohnarten benötigt mindestens eine Lohnart.{0}"), vbNewLine)
				End If
			End If

			If Me.Cbo_MonatBis.Text.Length > 0 And Me.Cbo_MonatBis.Visible Then
				monatBis = Me.Cbo_MonatBis.Text
			End If
			If Me.Cbo_JahrBis.Text.Length > 0 And Me.Cbo_JahrBis.Visible Then
				jahrBis = Me.Cbo_JahrBis.Text
			End If

			'Dim dtVon As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", Me.Cbo_MonatVon.Text, Me.Cbo_JahrVon.Text))
			'Dim dtBis As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", monatBis, jahrBis))
			'If DateDiff(DateInterval.Day, dtVon, dtBis) < 0 Then
			'	meldung += String.Format(m_Translate.GetSafeTranslationValue("Das Datum Von kann nicht kleiner als Datum Bis sein.{0}"), vbLf)
			'End If

			If Me.txt_MANr.Text.Trim.Length > 0 Then
				For Each manr As String In Me.txt_MANr.Text.Split(CChar(","))
					If Not IsNumeric(manr) Then
						meldung += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), manr, vbLf)
					ElseIf CInt(manr).ToString <> manr Then
						meldung += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist ungültig.{1}"), manr, vbLf)
					End If
				Next
			End If

			If meldung.Length > 0 Then
				m_UtilityUi.ShowErrorDialog(String.Format("{1}:{0}{2}", vbNewLine, m_Translate.GetSafeTranslationValue("Kriterienauswahl unvollständig"), meldung))

				Return False
			End If

		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

		Return True
	End Function


	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		Dim bisjahr As String = Cbo_JahrBis.Text
		Dim bismonat As String = Cbo_MonatBis.Text
		Dim loarten As String = String.Empty

		If Cbo_MonatBis.Visible Then
			If String.IsNullOrEmpty(Cbo_MonatBis.Text) Then bismonat = 12
		Else
			bismonat = Cbo_MonatVon.Text

		End If
		If Cbo_JahrBis.Visible Then
			If String.IsNullOrEmpty(Cbo_JahrBis.Text) Then bisjahr = Now.Year
		Else
			bisjahr = Cbo_JahrVon.Text

		End If

		loarten = Cbo_Lohnart.Text

		result.mandantenname = lueMandant.Text
		result.listname = Cbo_ArtderListe.Text

		result.manr = txt_MANr.Text
		result.vonjahr = Cbo_JahrVon.Text
		result.bisjahr = bisjahr
		result.vonmonat = Cbo_MonatVon.Text
		result.bismonat = bismonat
		result.lohnarten = loarten
		result.deletenull = ChkNullBetrag.Checked
		result.gavberuf = cbo_Beruf.Text
		result.gav1kategorie = Cbo_1Kategorie.Text


		Return result

	End Function


	Function GetMyQueryString() As Boolean

		Try
			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)
			Return False

		End Try

	End Function

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClearFields_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClearFields.ItemClick

		FormIsLoaded("frmListeSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()

		FillDefaultValues()

		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()

		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.XtraTabControl1.Name).Controls
				For Each con As Control In tabPg.Controls
					ResetControl(con)
				Next
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))
		End Try

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Try
			If con.Name.ToLower = Cbo_ArtderListe.Name.ToLower Or con.Name.ToLower = lueMandant.Name.ToLower Then Exit Sub

			If con.Name.ToLower = Cbo_Lohnart.Name.ToLower Then
				Trace.WriteLine(con.Name)
			End If

			' Rekursiver Aufruf
			' Sonst Control zurücksetzen
			If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = con
				tb.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = con
				cbo.EditValue = Nothing

			ElseIf con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

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


	'Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

	'	GetMenuItems4Export(Me.btnExport)

	'End Sub

	'Private Sub btnExport_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)

	'	btnExport.DropDown.Close()

	'	Select Case UCase(e.ClickedItem.Name)

	'		Case UCase("XLS")


	'		Case UCase("Contact")
	'			Call ExportDataToOutlook(Me.txt_SQL_2.Text)

	'		Case UCase("MAIL")
	'			Call RunMailModul(Me.txt_SQL_2.Text)

	'		Case UCase("FAX")
	'			Call RunTobitFaxModul(Me.txt_SQL_2.Text)

	'		Case UCase("SMS")
	'			Call RunSMSProg(Me.txt_SQL_2.Text)

	'	End Select
	'	'MsgBox("test: " & sender.ToString & " ; " & "Name: " & e.ClickedItem.Name & " ;" & e.ClickedItem.Text)

	'End Sub

	'Private Sub Jahr_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
	'  CheckJahr(sender)
	'End Sub

	'Private Sub CheckJahr(ByVal sender As Object)
	'  Try
	'    Dim jahr As String = DirectCast(sender, ComboBox).Text
	'    For Each item As ComboBoxItem In DirectCast(sender, myCbo).Items
	'      If item.Value = jahr Then
	'        Return
	'      End If
	'    Next
	'    DirectCast(sender, ComboBox).Text = Date.Now.Year.ToString
	'  Catch ex As Exception ' Manager
	'    m_UtilityUi.ShowErrorDialog("CheckJahr", ex)
	'  End Try

	'End Sub

	'Private Sub Monat_Leave(ByVal sender As Object, ByVal e As System.EventArgs)
	'  CheckMonat(sender)
	'End Sub

	'Private Sub CheckMonat(ByVal sender As Object)
	'  Try
	'    Dim monat As String = DirectCast(sender, ComboBox).Text
	'    For Each item As ComboBoxItem In DirectCast(sender, myCbo).Items
	'      If item.Value = monat Then
	'        Return
	'      End If
	'    Next
	'    DirectCast(sender, ComboBox).Text = Date.Now.Month.ToString
	'  Catch ex As Exception ' Manager
	'    m_UtilityUi.ShowErrorDialog("CheckMonat", ex)
	'  End Try

	'End Sub

	''' <summary>
	''' KeyPressEvent der Controls auffangen und verarbeiten. (Enter --> Tab)
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles ChkNullBetrag.KeyPress

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
	Private Sub KeyUpEvent(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
		Try
			If TypeOf (sender) Is ListBox Then
				' LISTBOX
				Dim _lst As ListBox = DirectCast(sender, ListBox)
				If e.KeyCode = Keys.Delete And _lst.SelectedIndex > -1 Then
					For i As Integer = 0 To _lst.SelectedIndices.Count - 1
						' Wenn der erste selektierte Inidices gelöscht wird,
						' rückt der nächste automatisch nach bis keine mehr
						' vorhanden sind. Darum immer 0. 19.08.2009 A.Ragusa
						_lst.Items.RemoveAt(_lst.SelectedIndices(0))
					Next
				ElseIf e.KeyCode = Keys.F4 Then
					Select Case _lst.Tag

					End Select
				End If
				' TEXTBOX
			ElseIf TypeOf (sender) Is TextBox Then
				Dim _txt As TextBox = DirectCast(sender, TextBox)
				If e.KeyCode = Keys.F4 Then
					Select Case _txt.Tag

					End Select
				End If
			End If
		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	Private Sub frmLOAGSearch_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.Cbo_MonatVon.Focus()
	End Sub

	Private Sub cbo_Beruf_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Beruf.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Cursor = Cursors.WaitCursor
			Dim lanr As String = ""
			Dim ala As String() = Me.Cbo_Lohnart.Text.Split(",")
			For Each la As String In ala
				Dim alanr As String = Val(la.Trim)
				lanr += If(lanr.Length = 0, "", ",") & alanr
			Next

			ListBeruf(sender, Me.Cbo_JahrVon.Text, Me.Cbo_JahrBis.Text, Me.Cbo_MonatVon.Text,
								Me.Cbo_MonatBis.Text, Me.Cbo_1Kategorie.Text, Me.txt_MANr.Text, lanr)
			Me.Cursor = Cursors.Default

		Catch ex As Exception ' Manager
			Me.Cursor = Cursors.Default
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub Cbo_1Kategorie_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_1Kategorie.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			Me.Cursor = Cursors.WaitCursor
			Dim lanr As String = ""
			Dim ala As String() = Me.Cbo_Lohnart.Text.Split(",")
			For Each la As String In ala
				Dim alanr As String = Val(la.Trim)
				lanr += If(lanr.Length = 0, "", ",") & alanr
			Next

			List1Kategorie(sender, Me.Cbo_JahrVon.Text, Me.Cbo_JahrBis.Text, Me.Cbo_MonatVon.Text,
										 Me.Cbo_MonatBis.Text, Me.cbo_Beruf.Text, Me.txt_MANr.Text, lanr)
			Me.Cursor = Cursors.Default

		Catch ex As Exception ' Manager
			Me.Cursor = Cursors.Default
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub Reset_BerufKategorie(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Try
			Me.Cursor = Cursors.WaitCursor
			If Me.cbo_Beruf.Enabled Then
				Me.cbo_Beruf.SelectedIndex = -1
				Me.Cbo_1Kategorie.SelectedIndex = -1
			End If
			Me.Cursor = Cursors.Default
		Catch ex As Exception ' Manager
			Me.Cursor = Cursors.Default
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	''' <summary>
	''' Selektionsfenster für die erste Mitarbeiter-Nummer öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr.ButtonClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim frmTest As New frmSearchRec("MA-Nr.")

		Dim m As String

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iListeValue(ClsDataDetail.strLOAGData)
		Me.txt_MANr.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()


	End Sub

	Private Sub Cbo_ArtderListe_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Cbo_ArtderListe.SelectedIndexChanged
		Try
			Cursor.Current = Cursors.WaitCursor
			Me.Cbo_Periode.Text = String.Empty
			Me.SwitchButton1.Value = False
			Me.SwitchButton2.Value = False

			If Me.Cbo_ArtderListe.SelectedIndex > -1 Then
				' Liste der Arbeitgeberlohnarten
				If Me.Cbo_ArtderListe.SelectedIndex = 0 Then ' DirectCast(Me.Cbo_ArtderListe.SelectedItem, ComboBoxItem).Value = "11.2" Then
					ClsDataDetail.LLTablename = String.Format("_LOAG_{0}", m_InitializationData.UserData.UserNr)
					ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Arbeitgeberlohnarten
					SwitchButton1.Visible = True
					SwitchButton2.Visible = True
					Me.lblLohnarten.Visible = True
					Me.Cbo_Lohnart.Visible = True
					Me.lblPeriode.Visible = True
					Me.Cbo_Periode.Visible = True
					Me.lblBeruf.Visible = True
					Me.cbo_Beruf.Visible = True
					Me.lbl1Kategorie.Visible = True
					Me.Cbo_1Kategorie.Visible = True
					Me.lbl_BerufeHeader.Visible = True

					Me.ModulName = "loag"

					' Arbeitgeberlohnartenrekapitulation
				ElseIf Me.Cbo_ArtderListe.SelectedIndex = 1 Then ' DirectCast(Me.Cbo_ArtderListe.SelectedItem, ComboBoxItem).Value = "11.1" Then
					ClsDataDetail.LLTablename = String.Format("_LORekap_{0}", m_InitializationData.UserData.UserNr)
					ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Lohnartenrekapitulation
					SwitchButton1.Visible = False
					SwitchButton2.Visible = False
					Me.Cbo_Lohnart.Properties.Items.Clear()
					Me.Cbo_MonatBis.Visible = False
					Me.Cbo_JahrBis.Visible = False
					Me.lblLohnarten.Visible = False
					Me.Cbo_Lohnart.Visible = False
					Me.lblPeriode.Visible = False
					Me.Cbo_Periode.Visible = False
					Me.lblBeruf.Visible = False
					Me.cbo_Beruf.Visible = False
					Me.lbl1Kategorie.Visible = False
					Me.Cbo_1Kategorie.Visible = False
					Me.lbl_BerufeHeader.Visible = False

					Me.ModulName = "loagrekap"

				End If
			End If

			Cursor.Current = Cursors.Default
			Me.Text = String.Format(m_Translate.GetSafeTranslationValue("Monatliche Lohnlisten: {0}"), Me.Cbo_ArtderListe.Text)

		Catch ex As Exception
			Cursor.Current = Cursors.Default
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	Private Sub LabelHeader_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbl_NummerfelderHeader.Click, lbl_BerufeHeader.Click
		Try
			_resetLohnarten = True
			Dim bIstOpen As Boolean
			Dim pnl As Panel = New Panel
			Dim lblHeader As Label = DirectCast(sender, Label)

			Select Case lblHeader.Name
				Case Me.lbl_NummerfelderHeader.Name
					pnl = Me.pnl_Nummerfelder
				Case Me.lbl_BerufeHeader.Name
					pnl = Me.pnl_Berufe
			End Select

			' Ist der Panel geöffnet oder geschlossen?
			bIstOpen = pnl.Height > lblHeader.Height

			If bIstOpen Then
				TogglePanel(pnl, False)
				Me.Cbo_MonatVon.Focus()
			Else
				TogglePanel(pnl)
				' Jedes Controls im Panel wird untersucht, um zu bestimmen wer den Focus erhält
				For Each con As Control In pnl.Controls
					' Wenn es ein Eingabe-Feld (Textbox, ComboBox) so Focus setzen
					If (TypeOf con Is ComboBox Or TypeOf con Is TextBox) Then
						con.Focus()
						Exit For
					End If
				Next
			End If

			RefreshAllPanels()
		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	Private Sub Cbo_Lohnart_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Lohnart.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			Me.Cursor = Cursors.WaitCursor
			Dim beruf As String = ""
			Dim gruppe1 As String = ""
			If ClsDataDetail.PanelExpanded(Me.pnl_Berufe) Then
				beruf = Me.cbo_Beruf.Text
				gruppe1 = Me.Cbo_1Kategorie.Text
			End If
			Me.Cbo_Lohnart.Text = ""
			Me.Cbo_Lohnart.CheckAll()
			ListLohnarten(Me.Cbo_Lohnart, Me.Cbo_JahrVon.Text, Me.Cbo_JahrBis.Text, Me.Cbo_MonatVon.Text,
											Me.Cbo_MonatBis.Text, beruf, gruppe1, Me.txt_MANr.Text)
			Me.Cursor = Cursors.Default

		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	Private Sub resetLohnarten(ByVal sender As System.Object, ByVal e As System.EventArgs)

		_resetLohnarten = True
	End Sub


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object,
																			 ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim i As Integer = 0
		Dim cForeColor As New System.Drawing.Color

		Me.bbiSearch.Enabled = False

		Try
			Dim sSqlQuerySelect As String = String.Empty

			Dim _ClsDb As New ClsDbFunc
			Me.txt_SQLQuery.Text = _ClsDb.GetQuerySQLString(Me)


		Catch ex As Exception ' Manager
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally
			Me.bbiSearch.Enabled = True
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Try

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object,
																								ByVal e As System.ComponentModel.ProgressChangedEventArgs) _
																								Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object,
																									 ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
																									 Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Fehler in Ihrer Anwendung.{0}{1}"), vbNewLine, e.Error.Message))
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				m_UtilityUi.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Aktion abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()

				Me.bbiSearch.Enabled = True

				' Daten auflisten...
				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(Me.txt_SQLQuery.Text, Me.ModulName)
					frmMyLV.Show()
					Me.Select()
				End If

				Me.LblTimeValue.Text = String.Format(m_Translate.GetSafeTranslationValue("Datenauflistung für {0} Einträge: in {1} ms"),
																						 frmMyLV.RecCount,
																						 Stopwatch.ElapsedMilliseconds().ToString)

				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."),
																					 frmMyLV.RecCount)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."),
																								frmMyLV.RecCount)

				Me.ResumeLayout()

			End If

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				'CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

			'ClsDataDetail.LLSelektionText = ""
			'ClsDataDetail.GetFilterBez += ClsDataDetail.LLSelektionText

			System.Media.SystemSounds.Asterisk.Play()

		End If

	End Sub

#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Liste drucken#PrintList", _
	'																				 "Entwurfsansicht#PrintDesign"}
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

	'			If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then bshowMnu = IsUserActionAllowed(0, 560) Else bshowMnu = myValue(0).ToString <> String.Empty
	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
	'				itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

	'				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetMenuItem
	'			End If

	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub

	'Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'	Me.PrintJobNr = GetJobNr()
	'	If ModulName = "loag" Then
	'		Me.SQL4Print = Me.txt_SQLQuery.Text

	'	ElseIf ModulName = "loagrekap" Then
	'		Me.SQL4Print = String.Format("Select * From {0} Order By LANr", ClsDataDetail.LLTablename)

	'	End If

	'	Try
	'		Select Case e.Item.Name.ToUpper
	'			Case "PrintList".ToUpper
	'				Me.bPrintAsDesign = False

	'			Case "printdesign".ToUpper
	'				Me.bPrintAsDesign = True

	'			Case Else
	'				Exit Sub

	'		End Select
	'		PrintListingThread = New Thread(AddressOf StartPrinting)
	'		PrintListingThread.Name = "PrintingList"
	'		PrintListingThread.SetApartmentState(ApartmentState.STA)
	'		PrintListingThread.Start()

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Daten in CSV- / TXT exportieren...#CSV"}
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
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = myValue(1).ToString

					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.txt_SQLQuery.Text

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				Dim ExportThread As New Thread(AddressOf StartExportModul)
				ExportThread.Name = "LOAGTOCSV"
				ExportThread.Start()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = Me.txt_SQLQuery.Text,
																																			 .ModulName = "LOAGTOCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		If Cbo_ArtderListe.SelectedIndex = 0 Then
			_Setting.ModulName = "LOAGToCSV"
			obj.ExportCSVFromLOANListing(Me.txt_SQLQuery.Text)

		ElseIf Cbo_ArtderListe.SelectedIndex = 1 Then
			_Setting.ModulName = "LOAGRekapToCSV"
			obj.ExportCSVFromLORekapListing(Me.txt_SQLQuery.Text)

		End If

	End Sub



#Region "Funktionen für SwitchButton..."

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_MonatBis.Visible = Me.SwitchButton1.Value
		Me.Cbo_MonatBis.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.Cbo_JahrBis.Visible = Me.SwitchButton2.Value
		Me.Cbo_JahrBis.Text = String.Empty
	End Sub

#End Region




#Region "Funktionen für Periode..."

	Private Sub Cbo_Periode_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Periode.QueryPopUp
		Dim datum As Date = Date.Now
		Me.Cbo_Periode.Properties.Items.Clear()

		'Me.Cbo_Periode.Properties.Items.Add(String.Empty)
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0:00}/{1})"), datum.Month, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0:00}/{1})"),
																												datum.AddMonths(-1).Month, datum.AddMonths(-1).Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 1, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 2, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 3, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 4, datum.Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letztes Jahr ({0})"),
																												datum.AddYears(-1).Year))

	End Sub

	Private Sub Cbo_Periode_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Cbo_Periode.SelectedIndexChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim dauer As Integer = 0
			Dim datum As Date = Date.Now
			Dim index1 As Integer = datum.Month - 1
			Dim bSetSelectedDate As Boolean = True
			Me.Cbo_MonatBis.Visible = False
			Me.Cbo_JahrBis.Visible = False
			Me.SwitchButton1.Value = False
			Me.SwitchButton2.Value = False

			Select Case Me.Cbo_Periode.SelectedIndex
				'Case 0
				'  bSetSelectedDate = False

				Case 2 '"1Q"
					index1 = 0
					dauer = 2
				Case 3 ' "2Q"
					index1 = 3
					dauer = 2
				Case 4 ' "3Q"
					index1 = 6
					dauer = 2
				Case 5 ' "4Q"
					index1 = 9
					dauer = 2
					'Case "1H"
					'  dauer = 5
					'Case "2H"
					'  index1 = 6
					'  dauer = 5
				Case 0 ' "DM"
					index1 = datum.Month - 1
					dauer = 0
				Case 1 ' "LM"
					datum = datum.AddMonths(-1)
					index1 = datum.Month - 1
					dauer = 0
				Case 6 ' "LJ"
					index1 = 0
					dauer = 11
					datum = datum.AddYears(-1)
			End Select

			If bSetSelectedDate Then
				Me.SwitchButton1.Value = True
				Me.SwitchButton2.Value = True
				Me.Cbo_JahrVon.Text = datum.Year.ToString
				Me.Cbo_JahrBis.Text = datum.Year.ToString
				Me.Cbo_MonatVon.Text = index1 + 1
				Me.Cbo_MonatBis.Text = index1 + 1 + dauer
				Me.Cbo_MonatBis.Visible = True
				Me.Cbo_JahrBis.Visible = True

			Else
				FillDefaultDates()
			End If


		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

#End Region

#Region "Funktionen für Monat und Jahr-Auswahl..."

	Private Sub Cbo_Monatvon_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_MonatVon.QueryPopUp, Cbo_MonatBis.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			If DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit).Properties.Items.Count = 0 Then ListCboMonate1Bis12(sender)
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub Cbo_JahrVon_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_JahrVon.QueryPopUp, Cbo_JahrBis.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			If DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit).Properties.Items.Count = 0 Then ListLOJahr(sender)
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub


#End Region

	Private Sub cbo_Beruf_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_Beruf.SelectedIndexChanged
		_resetLohnarten = True
	End Sub

	Private Sub Cbo_1Kategorie_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Cbo_1Kategorie.SelectedIndexChanged
		_resetLohnarten = True
	End Sub

	Private Sub txt_MANr_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles txt_MANr.EditValueChanged
		_resetLohnarten = True
	End Sub

End Class


Class ComboBoxItem
	Public Text As String
	Public Value As String
	Public Sub New(ByVal text As String, ByVal val As String)
		Me.Text = text
		Me.Value = val
	End Sub

	Public Overrides Function ToString() As String
		Return Text
	End Function
End Class


Module StringExtensions

	<Extension()> _
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

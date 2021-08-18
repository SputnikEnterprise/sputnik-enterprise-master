
Imports System.Reflection.Assembly

Imports System.Data.SqlClient
Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPS.Listing.Print.Utility
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Public Class frmFremdListSearch
  Inherits DevExpress.XtraEditors.XtraForm

#Region "private fields"

	Protected Shared m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_ConnectionString As String

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	Private _ClsFunc As New ClsDivFunc
	Private strValueSeprator As String = "#@"



	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()

  Public Shared frmMyLV As frmListeSearch_LV 
	Public Const frmMyLVName As String = "frmListeSearch_LV"


  Dim PrintListingThread As Thread
  Private Property PrintJobNr As String
  Private Property SQL4Print As String
  Private Property bPrintAsDesign As Boolean

  Private Property TranslatedPage As New List(Of Boolean)

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI


	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private m_SearchCriteria As New SearchCriteria

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()
		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


		ResetMandantenDropDown()
		LoadMandantenDropDown()

		PrintJobNr = GetJobNr

	End Sub

#End Region

	Private ReadOnly Property GetHwnd() As String
		Get
			Return CStr(Me.Handle)
		End Get
	End Property

	Private ReadOnly Property GetJobNr() As String
		Get
			Return "11.9"

		End Get
	End Property


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


	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmFremdListSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded(frmMyLVName, True)

		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			' Panels
			My.Settings.pnl_Nummerfeld_Height = Me.pnl_Nummerfelder.Height

			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmFremdListSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		Try
			If FormIsLoaded(frmMyLVName, False) Then
				frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
				frmMyLV.TopMost = True
				frmMyLV.TopMost = False
			End If
		Catch ex As Exception	' Manager
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	Sub StartTranslation()
		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblMANr.Text = m_Translate.GetSafeTranslationValue(Me.lblMANr.Text)

		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
		Me.LblFilial.Text = m_Translate.GetSafeTranslationValue(Me.LblFilial.Text)

		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiClearFields.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClearFields.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

		For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.XtraTabControl1.TabPages
			tbp.Text = m_Translate.GetSafeTranslationValue(tbp.Text)
		Next

	End Sub

	''' <summary>
	''' Beim Starten der Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmFremdListSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount

		SetInitialFields()

		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		If m_InitializationData.UserData.UserNr = 1 Then
			Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		Else
			If IsUserAllowed4DocExport(PrintJobNr) Then Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		End If

		Me.xtabSQLAbfrage.PageVisible = m_InitializationData.UserData.UserNr = 1

		Try
			FillDefaultValues()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Form_Load: {0}", ex))
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
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = "0"
					End If
					Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
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
		Catch ex As Exception	' Manager
			m_Logger.LogError(ex.ToString)
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
	Sub TogglePanel(ByRef pnl As Panel, Optional ByVal open As Boolean = True, Optional ByVal stepInterval As Integer = 5, _
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
				System.Windows.Forms.Application.DoEvents()
			Next
			pnl.Height = maxHeight

		Catch ex As Exception	' Manager
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()

		' DATUM ---------------------------------
		FillDefaultDates()

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
			' NEU: Nur Von-Monat und Von-Jahr vorbelegen. Bis-Monat und Bis-Jahr bleibt leer.
			Me.Cbo_MonatVon.Text = datum.Month.ToString
			Me.Cbo_MonatBis.Text = ""
			Me.Cbo_JahrBis.Text = ""
			Me.Cbo_JahrVon.Text = datum.Year.ToString
			'Me.Cbo_JahrBis.SelectedIndex = Me.Cbo_JahrBis.FindString(datum.Year.ToString)
			Me.Cbo_MonatVon.Text = datum.Month.ToString
			'Me.Cbo_MonatBis.SelectedIndex = Me.Cbo_MonatBis.FindString(datum.Month.ToString)

		Catch ex As Exception	' Manager
			m_Logger.LogError(ex.ToString)
		End Try


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

	'	Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'	Dim cmd As New SqlCommand(String.Format("SELECT * FROM {0} Order By LANr", ClsDataDetail.LLTablename), Conn)

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

	'		Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader									' 
	'		Try
	'			If Not rFoundedrec.HasRows Then
	'				cmd.Dispose()
	'				rFoundedrec.Close()

	'				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."))
	'			End If

	'		Catch ex As Exception
	'			MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Get4Print")

	'		End Try
	'		rFoundedrec.Read()
	'		If rFoundedrec.HasRows Then
	'			Me.SQL4Print = sSql
	'			Me.bPrintAsDesign = bForDesign
	'			Me.PrintJobNr = strJobInfo

	'			PrintListingThread = New Thread(AddressOf StartPrinting)
	'			PrintListingThread.Name = "PrintingFremdleistungListing"
	'			PrintListingThread.SetApartmentState(ApartmentState.STA)
	'			PrintListingThread.Start()

	'		End If

	'		rFoundedrec.Close()

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()

	'	End Try
	'End Sub


#Region "Funktionen zur Menüaufbau..."

	'Private Sub mnuListeDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	Try
	'		Get4Print(False, False, "11.9")	' Liste der Fremdleistungen
	'	Catch ex As Exception	' Manager
	'		m_Logger.LogError(ex.ToString)
	'	End Try

	'End Sub

	'Private Sub mnuDesignListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	Try
	'		Get4Print(True, False, "11.9") ' Liste der Fremdleistungen (Design)
	'	Catch ex As Exception	' Manager
	'		m_Logger.LogError(ex.ToString)
	'	End Try

	'End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Try
			If Me.Cbo_MonatVon.Text = String.Empty Then Me.Cbo_MonatVon.Text = CStr(Month(Now))
			If Me.Cbo_JahrVon.Text = String.Empty Then Me.Cbo_JahrVon.Text = CStr(Year(Now))

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Exit Sub
			Me.txt_SQLQuery.Text = String.Empty

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded(frmMyLVName, True)

			' Die Query-String aufbauen...
			GetMyQueryString()


		Catch ex As Exception	' Manager
			m_Logger.LogError(ex.ToString)

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		Dim bisjahr As String = Cbo_JahrBis.Text
		Dim bismonat As String = Cbo_MonatBis.Text
		Dim loarten As String = String.Empty

		If Cbo_MonatBis.Visible Then
			If String.IsNullOrEmpty(Cbo_MonatBis.Text) Then bismonat = CStr(12)
		Else
			bismonat = Cbo_MonatVon.Text

		End If
		If Cbo_JahrBis.Visible Then
			If String.IsNullOrEmpty(Cbo_JahrBis.Text) Then bisjahr = CStr(Now.Year)
		Else
			bisjahr = Cbo_JahrVon.Text

		End If
		result.listname = m_Translate.GetSafeTranslationValue("Aufstellung über Fremdleistung")
		result.mandantenname = lueMandant.Text

		result.manr = txt_MANr.Text
		result.vonjahr = Cbo_JahrVon.Text
		result.bisjahr = bisjahr
		result.vonmonat = Cbo_MonatVon.Text
		result.bismonat = bismonat

		Return result

	End Function

	Function Kontrolle() As Boolean
		Try
			Dim msg As String = ""
			Dim monatBis As String = Me.Cbo_MonatVon.Text
			Dim jahrBis As String = Me.Cbo_JahrVon.Text

			If Me.Cbo_MonatBis.Text.Length > 0 And Me.Cbo_MonatBis.Visible Then
				monatBis = Me.Cbo_MonatBis.Text
			End If
			If Me.Cbo_JahrBis.Text.Length > 0 And Me.Cbo_JahrBis.Visible Then
				jahrBis = Me.Cbo_JahrBis.Text
			End If

			Dim dtVon As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", Me.Cbo_MonatVon.Text, Me.Cbo_JahrVon.Text))
			Dim dtBis As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", monatBis, jahrBis))
			If DateDiff(DateInterval.Day, dtVon, dtBis) < 0 Then
				msg += String.Format(m_Translate.GetSafeTranslationValue("Das Datum Von kann nicht kleiner als Datum Bis sein.{0}"), vbLf)
			End If

			If Me.txt_MANr.Text.Trim.Length > 0 Then
				For Each manr As String In Me.txt_MANr.Text.Split(CChar(","))
					If Not IsNumeric(manr) Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), manr, vbLf)
					ElseIf CInt(manr).ToString <> manr Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist ungültig.{1}"), manr, vbLf)
					End If
				Next
			End If

			If msg.Length > 0 Then
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), _
																			vbLf, msg), _
																		m_Translate.GetSafeTranslationValue("Keine Suche möglich"), _
																		MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return False
			End If
		Catch ex As Exception	' Manager
			m_Logger.LogError(ex.ToString)
		End Try

		Return True
	End Function

	Function GetMyQueryString() As Boolean

		Try
			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()		' Multithreading starten

		Catch ex As Exception	' Manager
			m_Logger.LogError(ex.ToString)
		End Try

		Return True
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
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Try
			If con.Name.ToLower = lueMandant.Name.ToLower Then Exit Sub

			' Rekursiver Aufruf
			' Sonst Control zurücksetzen
			If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = CType(con, DevExpress.XtraEditors.TextEdit)
				tb.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = CType(con, DevExpress.XtraEditors.ComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(con, DevExpress.XtraEditors.CheckedComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

#End Region

#Region "Sonstige Funktionen..."

	Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

		Try
			If lv.Items.Count > 0 Then
				Dim lvi As ListViewItem = lv.SelectedItems(0)		 '.Item(0)
				If lvi.Selected Then
					Return lvi.Index
				Else
					Return -1
				End If
			End If

		Catch ex As Exception
			' Keine Fehlermeldung
		End Try

	End Function

#End Region

	'Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	Try
	'		GetMenuItems4Export(Me.btnExport)

	'	Catch ex As Exception	' Manager
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'End Sub

	'Private Sub btnExport_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)

	'	Try
	'		btnExport.DropDown.Close()

	'		Select Case UCase(e.ClickedItem.Name)

	'			Case UCase("XLS")


	'			Case UCase("Contact")
	'				Call ExportDataToOutlook(Me.txt_SQLQuery.Text)

	'			Case UCase("MAIL")
	'				Call RunMailModul(Me.txt_SQLQuery.Text)

	'			Case UCase("FAX")
	'				Call RunTobitFaxModul(Me.txt_SQLQuery.Text)

	'			Case UCase("SMS")
	'				Call RunSMSProg(Me.txt_SQLQuery.Text)

	'		End Select
	'		'MsgBox("test: " & sender.ToString & " ; " & "Name: " & e.ClickedItem.Name & " ;" & e.ClickedItem.Text)
	'	Catch ex As Exception	' Manager
	'		MessageBoxShowError("btnExport_DropDownItemClicked", ex)
	'	End Try


	'End Sub

	Private Sub frmQSTListeSearch_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.Cbo_MonatVon.Focus()
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

		m = frmTest.iListeValue()
		Me.txt_MANR.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()

	End Sub

	Private Sub Cbo_Periode_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Periode.QueryPopUp
		Dim datum As Date = Date.Now
		Me.Cbo_Periode.Properties.Items.Clear()

		'Me.Cbo_Periode.Properties.Items.Add(String.Empty)
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0:00}/{1})"), datum.Month, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0:00}/{1})"), _
																												datum.AddMonths(-1).Month, datum.AddMonths(-1).Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 1, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 2, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 3, datum.Year))
		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}. Quartal ({1})"), 4, datum.Year))

		Me.Cbo_Periode.Properties.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("Letztes Jahr ({0})"), _
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
				Me.Cbo_MonatVon.Text = CStr(index1 + 1)
				Me.Cbo_MonatBis.Text = CStr(index1 + 1 + dauer)
				Me.Cbo_MonatBis.Visible = True
				Me.Cbo_JahrBis.Visible = True

			Else
				FillDefaultDates()
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub Cbo_Filiale_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_Filiale.QueryPopUp
		ListEmployeeFiliale(Me.Cbo_Filiale)
	End Sub



#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, _
																			 ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim i As Integer = 0
		Dim cForeColor As New System.Drawing.Color

		Me.bbiSearch.Enabled = False

		Try
			Dim sSqlQuerySelect As String = String.Empty

			Dim _ClsDb As New ClsDbFunc
			_ClsDb.GetQuerySQLString(Me)
			Me.txt_SQLQuery.Text = String.Format("SELECT * FROM {0} Order By LANr", ClsDataDetail.LLTablename)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			e.Result = True
			If bw.CancellationPending Then e.Cancel = True

		End Try

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, _
																									 ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
																									 Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Fehler in Ihrer Anwendung.{0}{1}"), vbNewLine, e.Error.Message))
		Else
			If e.Cancelled = True Then
				Me.bbiSearch.Enabled = True
				MessageBox.Show(m_Translate.GetSafeTranslationValue("Aktion abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()

				' Daten auflisten...
				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(Me.txt_SQLQuery.Text, String.Empty)
					frmMyLV.Show()
					Me.Select()
				End If

				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), _
																					 frmMyLV.RecCount)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), _
																								frmMyLV.RecCount)

				Me.ResumeLayout()

			End If

			' Die Buttons Drucken und Export aktivieren
			Me.bbiSearch.Enabled = True
			If frmMyLV.RecCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				'CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

		End If

	End Sub

#End Region



	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

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
	'	Me.SQL4Print = Me.txt_SQLQuery.Text

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
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

	'	End Try

	'End Sub

	Sub StartPrinting()

		Dim strFilter As String = String.Empty

		bPrintAsDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 561, lueMandant.EditValue) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Me.PrintJobNr = GetJobNr()
		Me.SQL4Print = Me.txt_SQLQuery.Text

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine

		strFilter &= If(m_SearchCriteria.vonmonat.Length > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.vonmonat, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.vonjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.vonjahr, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.bismonat.Length > 0, String.Format(" - {0}", m_SearchCriteria.bismonat), String.Empty)
		strFilter &= If(m_SearchCriteria.bisjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.bisjahr), String.Empty)

		strFilter &= If(String.IsNullOrWhiteSpace(m_SearchCriteria.filiale), "", String.Format("{1}Filiale: {0}", m_SearchCriteria.filiale.ToString, vbNewLine))
		strFilter &= If(m_SearchCriteria.manr.Length > 0, String.Format("{1}Kandidaten: {0}", m_SearchCriteria.manr, vbNewLine), String.Empty)

		Dim _Setting As New ClsLLLOFremdSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																													.SelectedMDNr = m_InitializationData.MDData.MDNr,
																														 .SQL2Open = Me.SQL4Print,
																														 .JobNr2Print = Me.PrintJobNr,
																														 .ListBez2Print = m_SearchCriteria.listname,
																														 .frmhwnd = GetHwnd,
																														 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)
																																																							 })}
		Dim obj As New LOFremdSearchListing.ClsPrintLOFremdSearchList(_Setting)

		obj.PrintLOFremdSearchList(Me.bPrintAsDesign)

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiExport.DropDownControl, DevExpress.XtraBars.PopupMenu)

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
				ExportThread.Name = "LOFremdLeistungTOCSV"
				ExportThread.Start()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn, _
																																			 .SQL2Open = Me.txt_SQLQuery.Text, _
																																			 .ModulName = "LOFremdLeistungTOCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		_Setting.ModulName = "LOFremdleistungToCSV"
		obj.ExportCSVFromLOFremdleistungListing(Me.txt_SQLQuery.Text)

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



	Private Sub frmFremdListSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For i As Integer = 0 To GetExecutingAssembly.GetReferencedAssemblies.Count - 1
				strRAssembly &= String.Format("-->> {1}{0}", vbNewLine, GetExecutingAssembly.GetReferencedAssemblies(i).FullName)
			Next
			strMsg = String.Format(strMsg, vbNewLine, _
														 GetExecutingAssembly().FullName, _
														 GetExecutingAssembly().Location, _
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub



#Region "Helpers"

	' Monate 1 bis 12
	Private Sub ListCboMonate1Bis12(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		For i As Integer = 1 To 12
			cbo.Properties.Items.Add(i.ToString)
		Next
	End Sub


	Sub ListEmployeeFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Bezeichnung"

		Dim strSqlQuery As String = "[GetMAFiliale]"


		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("Leere Felder")
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)

				i += 1
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "Select LO.Jahr From LO "
		strSqlQuery += "Group By LO.Jahr Order By LO.Jahr DESC"

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader									'

			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				strEntry = rFOPrec("Jahr").ToString
				cbo.Properties.Items.Add(strEntry)
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region


End Class


'Module StringExtensions

'  <Extension()> _
'  Public Sub KommasEntfernen(ByRef text As String)
'    Do While (text.Contains(",,"))
'      text = text.Replace(",,", ",")
'    Loop
'    If text.StartsWith(",") Then
'      text = text.Remove(0, 1)
'    End If
'    If text.EndsWith(",") Then
'      text = text.Remove(text.Length - 1, 1)
'    End If
'  End Sub

'End Module

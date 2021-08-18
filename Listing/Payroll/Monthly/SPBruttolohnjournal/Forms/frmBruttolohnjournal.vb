
Option Strict Off

Imports System.Data.SqlClient
Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPS.Listing.Print.Utility
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.UI
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Public Class frmBruttolohnjournal
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

	Private _ClsFunc As New ClsDivFunc


	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()
	Public Shared frmMyLV As frmListeSearch_LV

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean
	Private Property ModulName As String

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()
		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		ResetMandantenDropDown()
		LoadMandantenDropDown()

	End Sub

#End Region


#Region "private properties"

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
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
	Private Sub ListLOJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_JahrVon.QueryPopUp, Cbo_JahrBis.QueryPopUp
		If DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit).Properties.Items.Count = 0 Then ListLOJahr(sender)
	End Sub

#End Region


#Region "Allgemeine Funktionen"

	' 0 - Nicht aktiviert // 1 - Aktiviert
	Private Sub FillCboAktiviertNichtAktviert(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboAktiviertNichtAktiviert(cbo)
		End If
	End Sub

	Private Sub FillCboVollstaendigNichtVoll(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboVollstaendigNichtVoll(cbo)
		End If
	End Sub

	' Ja - Nein
	Private Sub FillJaNein(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListJaNein(cbo)
		End If
	End Sub

	' Monate 1 bis 12
	Private Sub Monate1bis12(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MonatVon.QueryPopUp, Cbo_MonatBis.QueryPopUp
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboMonate1Bis12(cbo)
		End If
	End Sub

	Private Sub Checkbox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.CheckEdit Then
			Dim chk As CheckBox = DirectCast(sender, CheckBox)
			chk.BackColor = Color.Gray
		End If
	End Sub

	Private Sub Checkbox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.CheckEdit Then
			Dim chk = DirectCast(sender, DevExpress.XtraEditors.CheckEdit)
			chk.BackColor = Color.Transparent
		End If
	End Sub


#End Region

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmQSTListeSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmListeSearch_LV", True)
		Try
			My.Settings.frm_Location = String.Empty
			My.Settings.ifrmHeight = Me.Top
			My.Settings.ifrmWidth = Me.Left

			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmQSTListeSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmListeSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
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
		Me.lblLohnarten.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnarten.Text)
		Me.LblFilial.Text = m_Translate.GetSafeTranslationValue(Me.LblFilial.Text)

		Me.lblESBranche.Text = m_Translate.GetSafeTranslationValue(lblESBranche.Text)

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
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


		SetInitialFields()

		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Me.xtabSQLQuery.PageVisible = m_InitializationData.UserData.UserNr = 1

		If m_InitializationData.UserData.UserNr = 1 Then
			Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		Else
			If IsUserAllowed4DocExport("11.4") Then Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		End If

		' Berechtigung Fililale/Kostenstelle wählen
		If Not IsUserActionAllowed(m_InitializationData.UserData.UserNr, 672) Then
			Me.Cbo_FilialeDebitoren.Enabled = False
			Me.Cbo_Berater.Enabled = False
			'Dim _ClsUSData As New ClsUserSec
			Dim strUSTitle As String = m_InitializationData.UserData.UserFTitel

			Me.Cbo_Berater.Enabled = strUSTitle.ToLower.Contains("leiter") Or strUSTitle.ToLower.Contains("führer")
			Me.Cbo_FilialeDebitoren.Text = If(Me.Cbo_Berater.Enabled, m_InitializationData.UserData.UserFiliale, String.Empty)
			ListBerater(Me.Cbo_Berater, "")

			Dim main As ClsMain_Net = New ClsMain_Net
			For i As Integer = 1 To Me.Cbo_Berater.Properties.Items.Count - 1
				Dim item As ComboBoxItem = DirectCast(Me.Cbo_Berater.Properties.Items(i), ComboBoxItem)
				If item.Text.StartsWith(String.Format("{0}", m_InitializationData.UserData.UserFullNameWithComma)) Then
					Me.Cbo_Berater.SelectedIndex = i
				End If
			Next
		End If

		Me.LblTimeValue.Visible = CBool(m_InitializationData.UserData.UserNr = 1)

		' Link-Button Tab-Stops deaktvieren
		Me.lblMANr.TabStop = False
		Me.lblESBranche.TabStop = False

		FillDefaultValues()

	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try

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

	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()

		ListCboMonate1Bis12(Me.Cbo_MonatVon)
		ListCboMonate1Bis12(Me.Cbo_MonatBis)
		ListLOJahr(Me.Cbo_JahrVon)
		ListLOJahr(Me.Cbo_JahrBis)

		Dim datum As Date = Date.Now
		If datum.Day < 15 Then
			datum = datum.AddMonths(-1)
		End If
		Me.Cbo_JahrVon.EditValue = datum.Year
		Me.Cbo_JahrBis.EditValue = datum.Year
		Me.Cbo_MonatVon.EditValue = datum.Month
		Me.Cbo_MonatBis.EditValue = datum.Month

		ListLohnarten(Me.Cbo_Lohnart)

	End Sub

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	''' <param name="bForExport">ob die Liste für Export ist.</param>
	''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	''' <remarks></remarks>
	Sub GetBruttolohnjournal4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iESNr As Integer = 0
		Dim bResult As Boolean = True
		Dim storedProc As String = ""

		Dim sSql As String = Me.txt_SQLQuery.Text
		If sSql = String.Empty Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"))
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim cmd As New SqlCommand("[Create New Table For BruttolohnjournalListe]", Conn)

		Try
			Conn.Open()
			If strJobInfo = "11.4" Then
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.AddWithValue("@tblNameSource", ClsDataDetail.TablenameSource)
				cmd.Parameters.AddWithValue("@tblNameTarget", ClsDataDetail.TablenameBLJListe)

				cmd.ExecuteNonQuery()

				'Daten sind auf der Datenbank
				sSql = String.Format("SELECT * FROM {0}", ClsDataDetail.TablenameBLJListe)

			End If

			If bForDesign Then sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ", 1, 1)

			Dim rQSTListerec As SqlDataReader = cmd.ExecuteReader                  ' 
			Try
				If Not rQSTListerec.HasRows Then
					cmd.Dispose()
					rQSTListerec.Close()

					m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."))
				End If

			Catch ex As Exception
				m_UtilityUi.ShowErrorDialog(ex.ToString)

			End Try

			If rQSTListerec.HasRows Then
				'Me.SQL4Print = sSql
				'Me.PrintJobNr = strJobInfo
				StartPrinting()
				'PrintListingThread = New Thread(AddressOf StartPrinting)
				'PrintListingThread.Name = "PrintingListing"
				'PrintListingThread.SetApartmentState(ApartmentState.STA)
				'PrintListingThread.Start()

			End If
			rQSTListerec.Close()

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.Message)

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try
	End Sub


	Sub StartPrinting()
		Dim strFilter As String = String.Empty

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine
		strFilter &= If(m_SearchCriteria.lohnarten.Length > 0, String.Format("Lohnarten: {0}", m_SearchCriteria.lohnarten, vbNewLine), String.Empty)

		strFilter &= If(m_SearchCriteria.vonmonat.Length > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.vonmonat, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.vonjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.vonjahr, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.bismonat.Length > 0, String.Format(" - {0}", m_SearchCriteria.bismonat), String.Empty)
		strFilter &= If(m_SearchCriteria.bisjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.bisjahr), String.Empty)

		strFilter &= If(m_SearchCriteria.manr.Length > 0, String.Format("{1}Kandidaten: {0}", m_SearchCriteria.manr, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.kst.Length > 0, String.Format("{1}Berater: {0}", m_SearchCriteria.kst, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.filiale.Length > 0, String.Format("{1}Filiale: {0}", m_SearchCriteria.filiale, vbNewLine), String.Empty)

		Dim _Setting As New ClsLLBLJSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
			.SelectedMDNr = m_InitializationData.MDData.MDNr,
																												 .frmhwnd = GetHwnd,
																												 .ShowAsDesign = Me.bPrintAsDesign,
																												 .SQL2Open = Me.SQL4Print,
																												 .JobNr2Print = Me.PrintJobNr,
																												 .ShowAHVBasis = m_SearchCriteria.ShowAHVBasis,
																												 .ShowBruttolohn = m_SearchCriteria.ShowBruttolohn,
																												 .ShowSUVABasis = m_SearchCriteria.ShowSUVABasis,
																												 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)
																																																					 }
																												)}

		Dim obj As New BLJSearchListing.ClsPrintBLJSearchList(_Setting)
		obj.PrintBLJListing()

	End Sub


#Region "Funktionen zur Menüaufbau..."
	'Private Sub mnuBLJDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	' Daten auf der Datenbank vorbereiten
	'	GetBruttolohnjournal4Print(False, False, "11.4") ' Bruttolohnjournal

	'End Sub

	'Private Sub mnuDesignQSTListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	' Daten auf der Datenbank vorbereiten
	'	GetBruttolohnjournal4Print(True, False, "11.4")	' Bruttolohnjournal (Design)

	'End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Me.Cursor = Cursors.WaitCursor
		Try

			m_SearchCriteria = GetSearchKrieteria()
			ClsDataDetail.SelectionCriteria = m_SearchCriteria

			If Not (Kontrolle()) Then
				Exit Sub
			End If

			Me.txt_SQLQuery.Text = String.Empty

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded("frmQSTListeSearch_LV", True)

			' Die Query-String aufbauen...
			GetMyQueryString()

			Stopwatch.Reset()
			Stopwatch.Start()
			' Daten auflisten...
			If Not FormIsLoaded("frmListeSearch_LV", True) Then
				frmMyLV = New frmListeSearch_LV("[Create New Table For Bruttolohnjournal With Mandant]", m_SearchCriteria)

				frmMyLV.Show()
				Me.Select()
			End If


			Me.LblTimeValue.Text = String.Format(m_Translate.GetSafeTranslationValue("Datenauflistung für {0} Einträge: in {1} ms"), frmMyLV.RecCount,
																					 Stopwatch.ElapsedMilliseconds().ToString)

			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."),
																				 frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."),
																							frmMyLV.RecCount)

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				'CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

			' Selektion-Text für den LL
			ClsDataDetail.LLSelektionText = ""

		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.Message)

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Dim meldung As String = ""

		If lueMandant.EditValue Is Nothing Then meldung &= m_Translate.GetSafeTranslationValue("- Der Mandant muss ausgewählt werden.")
		If String.IsNullOrWhiteSpace(m_SearchCriteria.lohnarten) Then meldung &= m_Translate.GetSafeTranslationValue("- Die Lohnarten müssen ausgewählt werden.")

		If meldung.Length > 0 Then
			m_UtilityUi.ShowErrorDialog(String.Format("{1}:{0}{2}", vbNewLine, m_Translate.GetSafeTranslationValue("Kriterienauswahl unvollständig"), meldung))

			Return False
		End If

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

		If Cbo_Lohnart.Text.Length > 0 Then

			result.ShowBruttolohn = 0
			result.ShowSUVABasis = 0
			result.ShowAHVBasis = 0
			For Each item As String In Cbo_Lohnart.Text.Split(CChar(","))
				Select Case item.ToUpper.Trim
					Case "Brutto".ToUpper
						result.ShowBruttolohn = 1
					Case "SUVA-Basis".ToUpper
						result.ShowSUVABasis = 1
					Case "AHV-Basis".ToUpper
						result.ShowAHVBasis = 1
				End Select
			Next
		End If
		loarten = Cbo_Lohnart.Text

		result.mandantenname = lueMandant.Text

		result.manr = txt_MANR.Text
		result.vonjahr = Cbo_JahrVon.Text
		result.bisjahr = bisjahr
		result.vonmonat = Cbo_MonatVon.Text
		result.bismonat = bismonat
		result.lohnarten = loarten
		result.esbranche = txt_ESGewerbe.Text
		result.filiale = Cbo_FilialeDebitoren.Text
		If Not Cbo_Berater.EditValue Is Nothing Then
			result.kst = DirectCast(Cbo_Berater.SelectedItem, ComboBoxItem).Value
		Else
			result.kst = String.Empty
		End If

		Return result

	End Function

	Function GetMyQueryString() As Boolean
		Dim sSqlQuerySelect As String = String.Empty

		Dim _ClsDb As New ClsDbFunc

		ClsDataDetail.TablenameBLJListe = String.Format("_BruttolohnjournalListe_{0}", m_InitializationData.UserData.UserNr)
		ClsDataDetail.TablenameSource = String.Format("_Bruttolohnjournal_{0}", m_InitializationData.UserData.UserNr)


		Me.txt_SQLQuery.Text = String.Format("EXEC [Create New Table For Bruttolohnjournal With Mandant] {0}, '{1}', {2}, {3}, {4}, {5}, '{6}', '{7}', {8}, '{9}'",
																				 m_InitializationData.MDData.MDNr,
																				 m_SearchCriteria.manr,
																				 m_SearchCriteria.vonmonat,
																				 m_SearchCriteria.bismonat,
																				 m_SearchCriteria.vonjahr,
																				 m_SearchCriteria.bisjahr,
																				 m_SearchCriteria.kst,
																				 m_SearchCriteria.filiale,
																				 m_InitializationData.UserData.UserNr,
																				 m_SearchCriteria.esbranche)

		Return True
	End Function


#End Region


#Region "Clear Fields"

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
			If con.Name.ToLower = CboSort.Name.ToLower Or con.Name.ToLower = lueMandant.Name.ToLower Then Exit Sub

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

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		End Try

	End Sub

#End Region




	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		bPrintAsDesign = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 561, lueMandant.EditValue) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Me.SQL4Print = String.Format("SELECT * FROM {0} Order By Nachname, Vorname", ClsDataDetail.TablenameBLJListe)
		Me.PrintJobNr = "11.4"

		GetBruttolohnjournal4Print(Me.bPrintAsDesign, False, "11.4")

		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Liste drucken#PrintList",
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

	'	Me.SQL4Print = String.Format("SELECT * FROM {0} Order By Nachname, Vorname", ClsDataDetail.TablenameBLJListe)
	'	Me.PrintJobNr = "11.4"

	'	Me.bPrintAsDesign = False

	'	Try
	'		Select Case e.Item.Name.ToUpper
	'			Case "PrintList".ToUpper
	'				Me.bPrintAsDesign = False
	'				GetBruttolohnjournal4Print(Me.bPrintAsDesign, False, "11.4")

	'			Case "printdesign".ToUpper
	'				Me.bPrintAsDesign = True
	'				GetBruttolohnjournal4Print(Me.bPrintAsDesign, False, "11.4")


	'			Case Else
	'				Exit Sub

	'		End Select

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


		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Dim cmd As New SqlCommand("[Create New Table For BruttolohnjournalListe]", Conn)

			Conn.Open()
			cmd.CommandType = CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@tblNameSource", ClsDataDetail.TablenameSource)
			cmd.Parameters.AddWithValue("@tblNameTarget", ClsDataDetail.TablenameBLJListe)

			cmd.ExecuteNonQuery()


		Catch ex As Exception

		End Try

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				Dim ExportThread As New Thread(AddressOf StartExportModul)
				ExportThread.Name = "ExportLOBLJistToCSV"
				ExportThread.Start()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = String.Format("SELECT * FROM {0} Order By Nachname, Vorname", ClsDataDetail.TablenameBLJListe)
																																			 }
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		_Setting.ModulName = "LOBLJToCSV"
		obj.ExportCSVFromBLJListing(_Setting.SQL2Open)

	End Sub



	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_MonatBis.Visible = Me.SwitchButton1.Value
		Me.Cbo_MonatBis.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.Cbo_JahrBis.Visible = Me.SwitchButton2.Value
		Me.Cbo_JahrBis.Text = String.Empty
	End Sub




	Private Sub Onfom_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.Cbo_JahrVon.Focus()
	End Sub


	Private Sub ListBerater_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		ListBerater(sender, Me.Cbo_FilialeDebitoren.Text)
	End Sub

	Private Sub Cbo_ListUSFilalen_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_FilialeDebitoren.QueryPopUp
		Dim kst As String = String.Empty

		If Not Cbo_Berater.EditValue Is Nothing Then
			kst = DirectCast(Cbo_Berater.SelectedItem, ComboBoxItem).Value
		Else
			kst = String.Empty
		End If

		ListUSFilialen(Me.Cbo_FilialeDebitoren, Me.Cbo_MonatVon.Text, Me.Cbo_MonatBis.Text, Me.Cbo_JahrVon.Text, Me.Cbo_JahrBis.Text, kst)

	End Sub

	Private Sub Reset_BeraterFiliale(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MonatVon.SelectedIndexChanged,
								Cbo_MonatBis.SelectedIndexChanged, Cbo_JahrVon.SelectedIndexChanged, Cbo_JahrBis.SelectedIndexChanged
		If Me.Cbo_Berater.Enabled Then
			Me.Cbo_Berater.SelectedIndex = -1
			Me.Cbo_FilialeDebitoren.SelectedIndex = -1
		End If
	End Sub

	Private Sub Cbo_Lohnart_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Lohnart.QueryPopUp

		If Cbo_Lohnart.Properties.Items.Count = 0 Then ListLohnarten(Cbo_Lohnart)

	End Sub


	''' <summary>
	''' Selektionsfenster für die erste Mitarbeiter-Nummer öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OntxtMANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANR.ButtonClick
		m_SearchCriteria.vonjahr = Cbo_JahrVon.Text
		m_SearchCriteria.vonmonat = Cbo_MonatVon.Text

		Dim frmTest As New frmSearchRec(m_SearchCriteria, "madata")

		_ClsFunc.Get4What = "MANR"
		ClsDataDetail.strButtonValue = "BLJ"
		ClsDataDetail.Get4What = "MANR"

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		Dim m As String
		m = frmTest.iListValue()
		Me.txt_MANR.Text = CStr(m.ToString.Replace("#@", ","))

		frmTest.Dispose()
	End Sub

	Private Sub OntxtESGewerbe_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_ESGewerbe.ButtonClick
		m_SearchCriteria.vonjahr = Cbo_JahrVon.Text
		m_SearchCriteria.vonmonat = Cbo_MonatVon.Text

		Dim frmTest As New frmSearchRec(m_SearchCriteria, "BranchenData")

		_ClsFunc.Get4What = "ESGewerbe"
		ClsDataDetail.strButtonValue = "ESGewerbe"
		ClsDataDetail.Get4What = "ESGewerbe"

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iListValue()
		Me.txt_ESGewerbe.Text = CStr(m.ToString.Replace("#@", ","))
		frmTest.Dispose()
	End Sub


End Class

''' <summary>
''' Klasse für die ComboBox, um Text und Wert zu haben.
''' Das Item wird mit den Parameter Text für die Anzeige und
''' Value für den Wert zur ComboBox hinzugefügt.
''' </summary>
''' <remarks></remarks>
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

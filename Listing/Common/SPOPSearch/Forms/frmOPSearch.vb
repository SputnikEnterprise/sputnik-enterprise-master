
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten


Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel
Imports SPOPSearch.ClsDataDetail
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Common
Imports System.Globalization
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.UI
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class frmOPSearch
	Inherits DevExpress.XtraEditors.XtraForm

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

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private _ClsFunc As New ClsDivFunc
	Private m_Seprator As String = "#@"

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()

	Private m_md As Mandant

	Public Shared frmMyLV As frmOPSearch_LV

	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean
	Private Property bGetCreatedOnInsteadFakDate As Boolean
	Private Property TranslatedPage As New List(Of Boolean)



#Region "Constructor..."

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_md = New Mandant
		m_UtilityUI = New UtilityUI

		m_InitializationData = ClsDataDetail.m_InitialData
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		Reset()

		LoadMandantenDropDown()
		LoadPaymetReminderDropDownData()
		LoadDebitorenartDropDown()

		AddHandler luePaymentReminderCode.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler lueDebitorenart.ButtonClick, AddressOf OnDropDown_ButtonClick

	End Sub

#End Region

#Region "Lookup Edit Reset und Load..."

	Private Sub Reset()

		ResetMandantenDropDown()
		ResetPaymentReminderCodeDropDown()
		ResetDebitorenartDropDown()

	End Sub


	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		Dim columns = lueMandant.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo With {.FieldName = "MDName", .Width = 100, .Caption = "Mandant"})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	Private Sub ResetPaymentReminderCodeDropDown()

		luePaymentReminderCode.Properties.DisplayMember = "PaymentReminderDataString"
		luePaymentReminderCode.Properties.ValueMember = "PaymentReminderCode"

		Dim columns = luePaymentReminderCode.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("PaymentReminderDataString", 0))

		luePaymentReminderCode.Properties.ShowFooter = False
		luePaymentReminderCode.Properties.DropDownRows = 10
		luePaymentReminderCode.Properties.ShowHeader = False
		luePaymentReminderCode.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		luePaymentReminderCode.Properties.SearchMode = SearchMode.AutoComplete
		luePaymentReminderCode.Properties.AutoSearchColumnIndex = 0
		luePaymentReminderCode.Properties.NullText = String.Empty
		luePaymentReminderCode.EditValue = Nothing

	End Sub

	Private Sub ResetDebitorenartDropDown()

		lueDebitorenart.Properties.DisplayMember = "ArtLabel"
		lueDebitorenart.Properties.ValueMember = "Art"

		lueDebitorenart.Properties.Columns.Clear()
		lueDebitorenart.Properties.Columns.Add(New LookUpColumnInfo("Art", 0))
		lueDebitorenart.Properties.Columns.Add(New LookUpColumnInfo("ArtLabel", 0))

		lueDebitorenart.EditValue = Nothing

	End Sub

	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			End If
		End If
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
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, ClsDataDetail.m_InitialData.UserData.UserNr)
			m_InitializationData = ChangeMandantData

			m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_CustomerDatabaseAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (m_InitializationData.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (m_InitializationData.MDData Is Nothing)

	End Sub


#End Region


#Region "Dropdown Funktionen 1. Seite..."

	Private Sub Cbo_ESEinstufung_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ESEinstufung.QueryPopUp
		If Me.Cbo_ESEinstufung.Properties.Items.Count = 0 Then ListESEinstufung(Me.Cbo_ESEinstufung)
	End Sub

	Private Sub Cbo_ESBranche_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ESBranche.QueryPopUp
		If Me.cbo_ESBranche.Properties.Items.Count = 0 Then ListREBranche(Me.cbo_ESBranche)
	End Sub

	Private Sub cbo_ESRBank_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_ESRBank.QueryPopUp
		ListESRBankname(Me.cbo_ESRBank)
	End Sub

	Private Sub Cbo_Berater_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Berater.QueryPopUp
		ListBerater(Me.Cbo_Berater)
	End Sub

	Private Sub Cbo_KST1_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST1.QueryPopUp
		If Me.Cbo_KST1.Properties.Items.Count = 0 Then ListREKst1(Me.Cbo_KST1)
	End Sub

	Private Sub Cbo_KST2_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KST2.QueryPopUp
		If Me.Cbo_KST2.Properties.Items.Count = 0 Then ListREKst2(Me.Cbo_KST2)
	End Sub

	'Private Sub Cbo_MahnCode_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Mahncode.QueryPopUp
	'	If Me.Cbo_Mahncode.Properties.Items.Count = 0 Then ListREMahnCode(Me.Cbo_Mahncode)
	'End Sub

	'Private Sub Cbo_OPArt_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_OPArt.QueryPopUp
	'	If Me.Cbo_OPArt.Properties.Items.Count = 0 Then ListREArt(Me.Cbo_OPArt)

	'	If Me.Cbo_ListingArt.Text.StartsWith("3") Then
	'		If Me.Cbo_OPArt.Text.ToUpper = "G" Then Me.Cbo_OPArt.Text = ""
	'		If Me.Cbo_OPArt.Text.ToUpper = "R" Then Me.Cbo_OPArt.Text = ""

	'	Else
	'		If Me.Cbo_OPArt.Text.ToUpper <> "G" Or Me.Cbo_OPArt.Text.ToUpper <> "R" Then
	'			ListREArt(Me.Cbo_OPArt)
	'		End If

	'	End If

	'End Sub

	Private Sub Cbo_Filiale_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Filiale.QueryPopUp
		If Me.Cbo_Filiale.Properties.Items.Count = 0 Then ListKDFiliale(Me.Cbo_Filiale)
	End Sub

	Private Sub Cbo_Periode_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_FakPeriode.QueryPopUp, Cbo_ErstellPeriode.QueryPopUp

		If sender.Properties.Items.Count = 0 Then
			' Berechnung der Kalenderwoche (Achtung: In der letzte Woche des Jahres wird die Zahl nicht immer korrekt zurückgegeben.)
			Dim kw As Integer = DatePart(DateInterval.WeekOfYear, Date.Now, FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
			' Berechnung des Monats
			Dim mon As Integer = DatePart(DateInterval.Month, Date.Now)
			' Berechnung des Jahres
			Dim jahr As Integer = DatePart(DateInterval.Year, Date.Now)

			sender.Properties.Items.Add(New ComboValue("", ""))
			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Letzte Woche / KW {0}"), kw - 1), "LW"))
			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Diese Woche / KW {0}"), kw), "DW"))
			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Nächste Woche / KW {0}"), kw + 1), "NW"))

			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0}{1})"), If(mon = 1, 12, mon - 1), If(mon = 1, " / " & jahr - 1, "")), "LM"))
			'sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Letzten Monat ({0})"), mon - 1), "LM"))

			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Diesen Monat ({0})"), mon), "DM"))
			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Nächsten Monat ({0})"), mon + 1), "NM"))
			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Letztes Jahr ({0})"), jahr - 1), "LJ"))
			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Dieses Jahr ({0})"), jahr), "DJ"))
			sender.Properties.Items.Add(New ComboValue(String.Format(m_Translate.GetSafeTranslationValue("Nächstes Jahr ({0})"), jahr + 1), "NJ"))
		End If
		sender.properties.DropDownRows = 15

	End Sub

#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmOPSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
		FormIsLoaded("frmOPSearch_LV", True)

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iWidth = Me.Width
				My.Settings.iHeight = Me.Height

				My.Settings.SortBez = Me.CboSort.Text
				My.Settings.Listart = Me.Cbo_ListingArt.Text

				My.Settings.Setting_hideOPInfo = Me.chkHideOPInfoLine.Checked
				My.Settings.Setting_hideRefNr = Me.chkHideRefNrLine.Checked
				My.Settings.Setting_hideKreditInfo = Me.chkHideKreditInfoLine.Checked

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_InitializationData.UserData.UserNr = 1 Then

			Dim frm As New frmLibraryInfo
			frm.LoadAssemblyData()

			frm.Show()
			frm.BringToFront()

		End If
	End Sub




	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblheader1.Text = m_Translate.GetSafeTranslationValue(Me.lblheader1.Text)
		Me.lblheader2.Text = m_Translate.GetSafeTranslationValue(Me.lblheader2.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSonstiges.Text = m_Translate.GetSafeTranslationValue(Me.xtabSonstiges.Text)
		Me.xtabErweitert.Text = m_Translate.GetSafeTranslationValue(Me.xtabErweitert.Text)
		Me.xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)
		Me.xtabAdmin.Text = m_Translate.GetSafeTranslationValue(Me.xtabAdmin.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblsortierung.Text = m_Translate.GetSafeTranslationValue(Me.lblsortierung.Text)
		Me.lbllistenart.Text = m_Translate.GetSafeTranslationValue(Me.lbllistenart.Text)
		Me.lblnummer.Text = m_Translate.GetSafeTranslationValue(Me.lblnummer.Text)
		Me.lblkundennr.Text = m_Translate.GetSafeTranslationValue(Me.lblkundennr.Text)
		Me.lblverfalltage.Text = m_Translate.GetSafeTranslationValue(Me.lblverfalltage.Text)
		Me.chkValutaFromCreatedOn.Text = m_Translate.GetSafeTranslationValue(Me.chkValutaFromCreatedOn.Text)

		Me.lbl1kst.Text = m_Translate.GetSafeTranslationValue(Me.lbl1kst.Text)
		Me.lbl2kst.Text = m_Translate.GetSafeTranslationValue(Me.lbl2kst.Text)
		Me.lblberater.Text = m_Translate.GetSafeTranslationValue(Me.lblberater.Text)
		Me.lblfiliale.Text = m_Translate.GetSafeTranslationValue(Me.lblfiliale.Text)
		Me.lblmahncode.Text = m_Translate.GetSafeTranslationValue(Me.lblmahncode.Text)
		Me.lblart.Text = m_Translate.GetSafeTranslationValue(Me.lblart.Text)
		Me.lblmahnstufe.Text = m_Translate.GetSafeTranslationValue(Me.lblmahnstufe.Text)
		Me.lblmahndatum.Text = m_Translate.GetSafeTranslationValue(Me.lblmahndatum.Text)

		Me.grpInvoiceDate.Text = m_Translate.GetSafeTranslationValue(Me.grpInvoiceDate.Text)
		lblStichtag.Text = m_Translate.GetSafeTranslationValue(lblStichtag.Text)
		lblInvoiceDate.Text = m_Translate.GetSafeTranslationValue(lblInvoiceDate.Text)
		Me.lblfakturaperiode.Text = m_Translate.GetSafeTranslationValue(Me.lblfakturaperiode.Text)
		lblCreateDate.Text = m_Translate.GetSafeTranslationValue(lblCreateDate.Text)
		Me.lblfakturazwischen.Text = m_Translate.GetSafeTranslationValue(Me.lblfakturazwischen.Text)

		Me.grpKostenteilung.Text = m_Translate.GetSafeTranslationValue(Me.grpKostenteilung.Text)
		Me.lblErstellperiode.Text = m_Translate.GetSafeTranslationValue(Me.lblErstellperiode.Text)
		Me.lblerstellungzwischen.Text = m_Translate.GetSafeTranslationValue(Me.lblerstellungzwischen.Text)

		Me.grpMahnArt.Text = m_Translate.GetSafeTranslationValue(Me.grpMahnArt.Text)
		Me.grpEinstufung.Text = m_Translate.GetSafeTranslationValue(Me.grpEinstufung.Text)
		Me.lbleinstufung.Text = m_Translate.GetSafeTranslationValue(Me.lbleinstufung.Text)
		Me.lblBranche.Text = m_Translate.GetSafeTranslationValue(Me.lblBranche.Text)

		Me.grpESRAngaben.Text = m_Translate.GetSafeTranslationValue(Me.grpESRAngaben.Text)
		Me.lblESBank.Text = m_Translate.GetSafeTranslationValue(Me.lblESBank.Text)

		Me.lblOffenerbetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblOffenerbetrag.Text)
		Me.chkKDKreditlimiteUeberschritten.Text = m_Translate.GetSafeTranslationValue(Me.chkKDKreditlimiteUeberschritten.Text)

		Me.lblAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblAbfrage.Text)
		Me.lblderzeitigeabfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblderzeitigeabfrage.Text)
		Me.lbladmin.Text = m_Translate.GetSafeTranslationValue(Me.lbladmin.Text)

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmOPSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		SetDefaultSortValues()

		Try

			Dim UpdateDelegate As New MethodInvoker(AddressOf TranslateControls)
			Me.Invoke(UpdateDelegate)
			Try
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
					m_Logger.LogError(ex.ToString)

				End Try

				Me.KeyPreview = True
				Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
				If strStyleName <> String.Empty Then
					UserLookAndFeel.Default.SetSkinStyle(strStyleName)
				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			If _ClsProgSetting.GetLogedUSNr <> 1 Then
				Me.xtabOPSearch.TabPages.Remove(Me.xtabSQLAbfrage)
				Me.xtabOPSearch.TabPages.Remove(Me.xtabErweitert)
				Me.xtabOPSearch.TabPages.Remove(Me.xtabAdmin)

			End If

			Me.bbiExport.Visibility = If(IsUserAllowed4DocExport("7.0") Or _ClsProgSetting.GetLogedUSNr = 1,
																	 DevExpress.XtraBars.BarItemVisibility.Always, DevExpress.XtraBars.BarItemVisibility.Never)
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False

		Catch ex As Exception
			DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "frmOPSearch_Load", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		Me.xtabOPSearch.SelectedTabPage = Me.xtabAllgemein

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

			Dim strListart As String = My.Settings.Listart
			If String.IsNullOrWhiteSpace(strListart) Then strListart = String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Offene/Teil-Offene Rechnungen"))
			Me.Cbo_ListingArt.Text = strListart

			Me.chkHideOPInfoLine.Checked = My.Settings.Setting_hideOPInfo
			Me.chkHideRefNrLine.Checked = My.Settings.Setting_hideRefNr
			Me.chkHideKreditInfoLine.Checked = My.Settings.Setting_hideKreditInfo

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

			EnableControls()

			Try
				Me.cbo_Mahnstufe.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Kontoauszug")))
				Me.cbo_Mahnstufe.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("1. Mahnung")))
				Me.cbo_Mahnstufe.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("2. Mahnung")))
				Me.cbo_Mahnstufe.Properties.Items.Add(String.Format("3 - {0}", m_Translate.GetSafeTranslationValue("Inkasso")))

			Catch ex As Exception

			End Try
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.Message))

		End Try

	End Sub


	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	Sub GetData4Print(ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True

		Dim sSql As String = Me.txt_SQLQuery.Text
		If sSql = String.Empty Then
			MsgBox("Keine Suche wurde gestartet!.", MsgBoxStyle.Exclamation, "GetData4Print")
			Exit Sub
		End If

		' Faktura-Art Gutschriften-Filterbezeichnung
		SetFilterBezForGutschriften(strJobInfo)


		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand

		Try
			Conn.Open()
			'Daten sind bereits auf der Datenbank
			Select Case strJobInfo
				Case "7.10"
					' Fälligkeiten offener Rechnungen.
					Dim cmdDBLFA As SqlCommand = New SqlCommand("[Create New Table For Debitorenliste Faelligkeit Offene Rechnungen]", Conn)
					cmdDBLFA.Parameters.AddWithValue("@tblNameSource", ClsDataDetail.SPTabNamenDBL)
					cmdDBLFA.Parameters.AddWithValue("@tblNameTarget", ClsDataDetail.SPTabNamenDBLFälligkeiten)
					cmdDBLFA.Parameters.AddWithValue("@tblFaelligFieldName", If(Me.chkValutaFromCreatedOn.Checked, "CreatedOn", "Faellig"))

					cmdDBLFA.CommandType = CommandType.StoredProcedure
					cmdDBLFA.ExecuteNonQuery()
					sSql = String.Format("SELECT * FROM {0} ORDER BY R_Name1", ClsDataDetail.SPTabNamenDBLFälligkeiten) ' 04.02.2010

				Case "7.11"
					' Statistik bezahlter Rechnungen.
					Dim cmdDBLStat As SqlCommand = New SqlCommand("[Create New Table For Debitorenliste BezRechnStat]", Conn)
					cmdDBLStat.Parameters.AddWithValue("@tblNameSource", ClsDataDetail.SPTabNamenDBL)
					cmdDBLStat.Parameters.AddWithValue("@tblNameTarget", ClsDataDetail.SPTabNamenDBLStat)
					cmdDBLStat.CommandType = CommandType.StoredProcedure
					cmdDBLStat.ExecuteNonQuery()

					sSql = String.Format("SELECT * FROM {0}", ClsDataDetail.SPTabNamenDBLStat) ' 12.02.2010

				Case "7.12"
					' Liste offener Rechnung nach Fakturadatum.
					' Daten vorbereiten. D.h. Tabelle auf DB erstellen. 18.02.2010
					Dim cmdDBLFA As SqlCommand = New SqlCommand("[Create New Table For Debitorenliste nach Fakturadatum]", Conn)
					cmdDBLFA.Parameters.AddWithValue("@tblNameSource", ClsDataDetail.SPTabNamenDBL)
					cmdDBLFA.Parameters.AddWithValue("@tblNameTarget", ClsDataDetail.SPTabNamenDBLnachFakturadatum)
					cmdDBLFA.Parameters.AddWithValue("@tblFaelligFieldName", If(Me.chkValutaFromCreatedOn.Checked, "CreatedOn", "Fak_Dat"))

					cmdDBLFA.CommandType = CommandType.StoredProcedure
					cmdDBLFA.ExecuteNonQuery()

					sSql = String.Format("SELECT * FROM {0}", ClsDataDetail.SPTabNamenDBLnachFakturadatum) ' 18.02.2010

				Case "7.14"
					' Verfallkalender...
					' Daten vorbereiten. D.h. Tabelle auf DB erstellen. 20.05.2010
					Dim cmdDBLVK As SqlCommand = New SqlCommand("Create New Table For Debitorenliste Verfallkalender", Conn)
					cmdDBLVK.Parameters.AddWithValue("@tblNameSource", ClsDataDetail.SPTabNamenDBL)
					cmdDBLVK.Parameters.AddWithValue("@tblNameTarget", ClsDataDetail.SPTabNamenDBLVK)
					cmdDBLVK.Parameters.AddWithValue("@tblFaelligFieldName", If(Me.chkValutaFromCreatedOn.Checked, "CreatedOn", "Faellig"))

					cmdDBLVK.CommandType = CommandType.StoredProcedure
					cmdDBLVK.ExecuteNonQuery()

					sSql = String.Format("SELECT * FROM {0} Order By R_Name1, R_Ort", ClsDataDetail.SPTabNamenDBLVK)  ' 20.05.2010


				Case Else
					sSql = String.Format("SELECT * FROM {0} RE", ClsDataDetail.SPTabNamenDBL) ' 04.02.2010
					' Sort-Klausel wieder hinzufügen
					Dim _clsDB As New ClsDbFunc
					sSql += GetSortString()

			End Select
			cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

			Dim rFoundedRec2Print As SqlDataReader = cmd.ExecuteReader
			Try
				If Not rFoundedRec2Print.HasRows Then
					cmd.Dispose()
					rFoundedRec2Print.Close()

					MessageBox.Show(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."),
													"GetData4Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
					Exit Sub
				End If

			Catch ex As NoNullAllowedException

			Catch ex As Exception
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetData4Print")

			End Try

			rFoundedRec2Print.Read()
			ClsDataDetail.GetFilterBez = String.Empty
			ClsDataDetail.GetFilterBez2 = String.Empty
			ClsDataDetail.GetFilterBez3 = String.Empty
			ClsDataDetail.GetFilterBez4 = String.Empty
			If rFoundedRec2Print.HasRows Then
				For i As Integer = 0 To ClsDataDetail.GetFilterBezArray.Count - 1
					ClsDataDetail.GetFilterBez &= ClsDataDetail.GetFilterBezArray(i).ToString.Replace(vbLf, String.Empty) & vbNewLine
				Next

				Me.SQL4Print = sSql
				Me.PrintJobNr = strJobInfo

				StartPrinting()

			End If
			rFoundedRec2Print.Close()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetData4Print")

		Finally
			Conn.Close()

		End Try

	End Sub

	Sub StartPrinting()
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 616)
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso bShowDesign

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLOPSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																																									.SQL2Open = Me.SQL4Print,
																																									.JobNr2Print = Me.PrintJobNr,
																																									.DocBez = ClsDataDetail.ListBez,
																																									.ListSortBez = ClsDataDetail.GetSortBez,
																																									.ListFilterBez = New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																																																																		 ClsDataDetail.GetFilterBez2,
																																																																		 ClsDataDetail.GetFilterBez3,
																																																																		 ClsDataDetail.GetFilterBez4}),
																																									.ShowAsDesign = ShowDesign,
																																									.TotalOpenBetrag4Date = ClsDataDetail.GetTotalOpenBetrag4Date,
																																									.FirstDate = ClsDataDetail.GetFDate4OP,
																																									.LastDate = ClsDataDetail.GetSDate4OP,
																																									.bShowFaelligInColumn = False,
																																									.bShow15DayAsFirst = False,
																																									.bGetCreatedOnInsteadFakDate = Me.bGetCreatedOnInsteadFakDate,
																																									.SelectedMDNr = m_InitializationData.MDData.MDNr,
																																									.LogedUSNr = m_InitializationData.UserData.UserNr,
																																									.HideOPInfoLine = chkHideOPInfoLine.Checked,
																																									.HideRefNrLine = chkHideRefNrLine.Checked,
																																									.HideKreditInfoLine = chkHideKreditInfoLine.Checked
																																									}
		Dim obj As New SPS.Listing.Print.Utility.OPSearchListing.ClsPrintOPSearchList(_Setting)
		obj.PrintOPSearchList_1()

	End Sub

#Region "Funktionen zur Menüaufbau..."

	Private Sub mnuDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		GetData4Print(False, strModultoPrint) ' Debitorenlisten

	End Sub


	Private Sub SetFilterBezForGutschriften(ByVal JobNr As String)

		' Die Faktura-Art-Filterbezeichnung wird nach dem Suchen festgelegt. Kann nicht in der Array direkt hinzugefügt werden,
		' da mehrere unterschiedliche Listen mit unterschiedliche Einschränkungen gedruckt werden.
		' Es muss diese auch zuerst entfernt werden, falls vorhanden, da nicht klar ist, welche Liste vorhin gedruckt wurde.
		ClsDataDetail.GetFilterBezArray.Remove(ClsDataDetail.FakturaArtFilterBez)
		ClsDataDetail.FakturaArtFilterBez = ""

		' Nur Filter-Bezeichnungen hinzufügen, wenn keine bestimmte Faktura-Art angegeben wurde.
		If lueDebitorenart.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueDebitorenart.EditValue) Then
			Select Case JobNr
				Case "7.0", "7.9", "7.13"
					ClsDataDetail.FakturaArtFilterBez = String.Format(m_Translate.GetSafeTranslationValue("inkl. aller Gutschriften{0}"), vbLf)
				Case "7.5"
					ClsDataDetail.FakturaArtFilterBez = String.Format(m_Translate.GetSafeTranslationValue("inkl. offener Gutschriften{0}"), vbLf)
				Case "7.6"
					ClsDataDetail.FakturaArtFilterBez = String.Format(m_Translate.GetSafeTranslationValue("inkl. ausgeglichener Gutschriften{0}"), vbLf)
					ClsDataDetail.FakturaArtFilterBez += String.Format(m_Translate.GetSafeTranslationValue("Teilzahlungen nicht berücksichtigt{0}"), vbLf)
				Case "7.10", "7.12", "7.14"
					If ClsDataDetail.SelectedListArt <> "3" Then
						ClsDataDetail.FakturaArtFilterBez = String.Format(m_Translate.GetSafeTranslationValue("inkl. offener Gutschriften{0}"), vbLf)
					End If
			End Select
			If ClsDataDetail.FakturaArtFilterBez <> "" Then
				ClsDataDetail.GetFilterBezArray.Add(ClsDataDetail.FakturaArtFilterBez)
			End If

		End If

	End Sub

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
			ClsDataDetail.GetFDate4OP = String.Empty
			ClsDataDetail.GetSDate4OP = String.Empty
			ClsDataDetail.GetModulToPrint = String.Empty
			ClsDataDetail.GetstrOPNr4Date = String.Empty
			ClsDataDetail.GetstrKDNr4Date_2 = String.Empty

			ClsDataDetail.GetSortValue_2 = String.Empty
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
		Try
			_ClsFunc.GetOPNr = 0
			_ClsFunc.GetKDNr = 0
			Me.txt_SQLQuery.Text = String.Empty
			If Not Me.txtOPNr_2.Visible Then Me.txtOPNr_2.Text = Me.txtOPNr_1.Text
			If Not Me.txtKDNr_2.Visible Then Me.txtKDNr_2.Text = Me.txtKDNr_1.Text

			FormIsLoaded("frmOPSearch_LV", True)
			' Hinterlegte Sortierung anzeigen
			If ClsDataDetail.GetLVSortBez <> String.Empty Then Me.CboSort.Text = ClsDataDetail.GetLVSortBez
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			' Soll Fakturadatum oder Erstellungsdatum der Rechnung berücksichtigt werden...
			bGetCreatedOnInsteadFakDate = Me.chkValutaFromCreatedOn.Checked

			' Eingabe-Kontrolle
			If Not Kontrolle() Then Return

			' Die Query-String aufbauen...
			GetMyQueryString()

			' List-Art- und OP-Art-Selektion vormerken
			ClsDataDetail.SelectedListArt = Me.Cbo_ListingArt.Text.Substring(0, 1)
			ClsDataDetail.SelectedOPArt = lueDebitorenart.EditValue
			' Die zu druckenden Listen freischalten oder deaktivieren
			'EnableListToPrint()

			Stopwatch.Reset()
			Stopwatch.Start()

			' Daten auflisten...
			If Not FormIsLoaded("frmOPSearch_LV", True) Then
				frmMyLV = New frmOPSearch_LV(Me.txt_SQLQuery.Text)

				frmMyLV.Show()
				Me.Select()
			End If

			' Nach dem Datenauflisten müssen noch die Kreditinfo auf der Debitorenliste ergänzt werden. 21.05.2010
			'InsertKreditInfoToDebitorenListe()

			' Die Buttons Drucken und Export aktivieren/deaktivieren
			Me.bbiPrint.Enabled = False
			Me.bbiExport.Enabled = False
			ClsDataDetail.RecsFound = False

			If frmMyLV.RecCount > 0 Then
				ClsDataDetail.RecsFound = True
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True
				CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

			' Drucken-Kontextmenu aktualisieren
			'SetPrintContextMenuText()
			If Me.deFakDate_2.Text = "31.12.3099" Then
				Me.deFakDate_2.Text = String.Empty
				Me.deFakDate_2.ForeColor = Color.Black
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

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Debitorenliste#mnuOPListPrint",
																					 "-Fälligkeiten offener Rechnungen#mnuOPListPrintFaelligkeit",
																					 "Liste offener Rechnungen nach Fakturadatum#mnuOPListPrintNachFakturadatum",
																					 "Statistik bezahlter Rechnungen#mnuOPListPrintStatBezRechn",
																					 "Verfallkalender#mnuOPListPrintVerfallkalender"}
		If Not IsUserActionAllowed(0, 606) Then Exit Sub
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
			Case "mnuOPListPrint".ToUpper
				GetData4Print(False, strModultoPrint) ' Design Debitorenlisten

			'Case "PrintDesign".ToUpper
			'	GetData4Print(False, strModultoPrint)	' Design Debitorenlisten

			Case "mnuOPListPrintFaelligkeit".ToUpper
				GetData4Print(False, "7.10") ' Fälligkeiten offener Rechnungen

			'Case "mnuDesignOPListFaelligkeit".ToUpper
			'	GetData4Print(False, "7.10") ' Design Fälligkeiten offener Rechnungen

			Case "mnuOPListPrintStatBezRechn".ToUpper
				GetData4Print(False, "7.11") ' Statistik von bezahlten Rechnungen

			'Case "mnuDesignOPListStatBezRechn".ToUpper
			'	GetData4Print(False, "7.11") ' Design Statistik von bezahlten Rechnungen

			Case "mnuOPListPrintNachFakturadatum".ToUpper
				GetData4Print(False, "7.12") ' Liste offener Posten nach Fakturadatum

			'Case "mnuDesignOPListNachFakturadatum".ToUpper
			'	GetData4Print(False, "7.12") ' Design Liste offener Posten nach Fakturadatum

			Case "mnuOPListPrintVerfallkalender".ToUpper
				GetData4Print(False, "7.14") ' Verfallkalender

				'Case "mnuDesignOPListVerfallkalender".ToUpper
				'	GetData4Print(False, "7.14") ' Design Verfallkalender

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

		Select Case e.Item.Name.ToUpper
			Case UCase("Abacus")
				ExportDataToAbacus(Me.txtAdminQuery.Text)

			Case UCase("Simba")
				Call ExportDataToSimba(Me.txt_SQLQuery.Text)

			Case UCase("Cresus")
				Call ExportDataToCresus(Me.txt_SQLQuery.Text)

			Case "TXT", "CSV"
				StartExportOPListModul(Me.txt_SQLQuery.Text)

			Case UCase("CSOPListe")
				Call RunXLSModul4CS(Me.txt_SQLQuery.Text, Now.Date, Me.deFakDate_1.Text)

			Case UCase("XML")
				Call RunXMLModul(Me.txt_SQLQuery.Text)


			Case UCase("SWIFAC")
				Call RunSWIFACExport(e.Item.AccessibleName.ToString)

			Case UCase("Comatic")
				Dim frmTest As New frmComatic(Me.txt_SQLQuery.Text)
				frmTest.ShowDialog()

			Case UCase("KMUFACTORING")
				Call RunFactoringExport(e.Item.AccessibleName.ToString)

			Case UCase("CRF"), UCase("CSOPListe")
				If Trim(Me.deMahnDate.Text).Length + Trim(Me.cbo_Mahnstufe.Text).Length = 0 Then
					If Me.deMahnDate.Text.Length = 0 Then
						DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Achtung: Sie haben kein Mahndatum ausgewählt. Der Vorgang wird beendet."),
									 m_Translate.GetSafeTranslationValue("Daten für Creditreform"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

					ElseIf Me.cbo_Mahnstufe.Text.Length = 0 Then
						DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Achtung: Sie haben keine Mahnstufe ausgewählt. Der Vorgang wird beendet."),
									 m_Translate.GetSafeTranslationValue("Daten für Creditreform"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

					End If
					Exit Sub

				End If
				Call RunCRFExport(e.Item.Name.ToString,
													If(Me.cbo_Mahnstufe.Text.Length > 0,
														 CInt(Val(Me.cbo_Mahnstufe.Text.Substring(0, 1))), 4),
														 If(Me.deMahnDate.Text.Length = 0, "1.1.1900", Me.deMahnDate.Text))
			Case Else
				Exit Sub

		End Select

	End Sub

	Function Kontrolle() As Boolean
		Dim check As Boolean = True
		Dim meldung As String = ""
		Dim culture As CultureInfo = New CultureInfo("de-DE")

		If String.IsNullOrWhiteSpace(deStichtag.Text) Then
			deStichtag.EditValue = Nothing
		Else
			deStichtag.EditValue = Convert.ToDateTime(deStichtag.Text, culture)
		End If

		If Val(Cbo_ListingArt.EditValue) = 0 OrElse Val(Cbo_ListingArt.EditValue) = 3 OrElse Val(Cbo_ListingArt.EditValue) = 5 OrElse Val(Cbo_ListingArt.EditValue) = 6 Then
			deStichtag.EditValue = Nothing

		ElseIf Val(Cbo_ListingArt.EditValue) = 1 OrElse Val(Cbo_ListingArt.EditValue) = 2 Then
			Dim invoiceDeadlineDate As Date?
			If Not deStichtag.EditValue Is Nothing AndAlso IsDate(deStichtag.EditValue) Then
				invoiceDeadlineDate = DateTime.ParseExact(deStichtag.EditValue, "dd.MM.yyyy", Nothing)
			Else
				invoiceDeadlineDate = Now.Date
			End If

			deStichtag.EditValue = invoiceDeadlineDate
		End If


		If Not check Then
			MessageBox.Show(meldung, m_Translate.GetSafeTranslationValue("Keine Suche möglich"),
											MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
		Return check
	End Function

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		'Dim cControl As Control
		'Dim strText As String = Me.CboSort.Text

		FormIsLoaded("frmOPSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()
		EnableControls()


		'deFakDate_1.Enabled = True
		'deFakDate_2.Enabled = True

		'deCreated_1.Enabled = True
		'deCreated_2.Enabled = True

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
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabOPSearch").Controls
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
			If con.Name.ToLower = "CboSort".ToLower Or con.Name.ToLower = "Cbo_ListingArt".ToLower Or con.Name.ToLower = "luemandant".ToLower Then Exit Sub

			If TypeOf (con) Is TextBox Then
				Dim tb As TextBox = con
				tb.Text = String.Empty

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
				cbo.Properties.Items.Clear()
				cbo.Text = String.Empty

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.LookUpEdit Then
				Dim de As DevExpress.XtraEditors.LookUpEdit = con
				de.EditValue = Nothing

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

	'''' <summary>
	'''' Default-Werte 
	'''' </summary>
	'''' <remarks></remarks>
	'Sub FillDefaultValues()
	'	'Me.CheckBox1.Checked = True
	'	'Me.CheckBox2.Checked = True
	'	'Me.CheckBox3.Checked = True
	'	'Me.txt_OpenBetrag_1.Text = "0.00"
	'	'Me.txt_OpenBetrag_2.Text = "0.00"
	'End Sub

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
		Dim cmd As SqlCommand = New SqlCommand(String.Format("SELECT * FROM {0}", ClsDataDetail.SPTabNamenDBL), conn)
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
											 Optional ByVal docname As String = "",
											 Optional ByVal defaultExtension As String = "txt")
		Dim saveDial As SaveFileDialog = New SaveFileDialog()
		Dim result As DialogResult
		saveDial.FileName = docname
		saveDial.DefaultExt = "csv"
		result = saveDial.ShowDialog()

		If result = DialogResult.OK Then
			Try
				IO.File.WriteAllLines(saveDial.FileName, listOfStrings.ToArray, System.Text.Encoding.Default)
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Die Datei {0} wurde erfolgreich gespeichert."),
																			saveDial.FileName), m_Translate.GetSafeTranslationValue("Daten gespeichert"),
																			MessageBoxButtons.OK, MessageBoxIcon.Information)
			Catch ex As Exception
				MessageBox.Show(ex.Message, m_Translate.GetSafeTranslationValue("Speicherung nicht durchgeführt"),
												MessageBoxButtons.OK, MessageBoxIcon.Error)
			End Try
		End If

	End Sub


#End Region


#Region "KeyEvents für Lst und Textfelder..."

	Private Sub txtOPNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtOPNr_1.ButtonClick

		If e.Button.Index = 0 Then
			Dim frmTest As New frmSearchRec("Invoice")

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iKDValue()
			Me.txtOPNr_1.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(m_Seprator, ",")))
			frmTest.Dispose()
		End If

	End Sub

	Private Sub txtOPNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtOPNr_2.ButtonClick

		If e.Button.Index = 0 Then
			Dim frmTest As New frmSearchRec("Invoice")
			Dim strModulName As String = String.Empty

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iKDValue()
			Me.txtOPNr_2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(m_Seprator, ",")))
			frmTest.Dispose()
		End If

	End Sub

	Private Sub txtKDNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDNr_1.ButtonClick

		If e.Button.Index = 0 Then
			Dim frmTest As New frmSearchRec("Customer")
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
			Dim frmTest As New frmSearchRec("Customer")
			Dim strModulName As String = String.Empty

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iKDValue()
			Me.txtKDNr_2.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(m_Seprator, ",")))
			frmTest.Dispose()
		End If

	End Sub


	Private Sub txt_OpenBetrag_1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_OpenBetrag_1.LostFocus, txt_OpenBetrag_2.LostFocus

		Try
			sender.text = Format(Val(sender.text.ToString), "0.00")

		Catch ex As Exception
			sender.text = "0.00"

		End Try

	End Sub

	Private Sub txt_FakDat_1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

		Try
			sender.text = Format(CDate(sender.text.ToString), "dd.MM.yyyy")

		Catch ex As Exception
			sender.text = String.Empty

		End Try

	End Sub



#End Region

	Private Sub Cbo_ListingArt_QueryCloseUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_ListingArt.QueryCloseUp
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_1", Me.Cbo_ListingArt.Text)

		EnableControls()
		ClsDataDetail.SelectedListArt = Me.Cbo_ListingArt.Text.Substring(0, 1)

	End Sub

	Private Sub EnableControls()

		If Me.Cbo_ListingArt.Text.StartsWith("3") Then ' AndAlso (Me.lueDebitorenart.EditValue.Contains("G") OrElse lueDebitorenart.EditValue.Contains("R")) Then
			Me.lueDebitorenart.EditValue = Nothing
		End If


		'' Drucken- und Export-Button nur für gesuchte Liste freigeben, falls überhaupt Datensätze vorhanden sind.
		'If ClsDataDetail.RecsFound Then
		'	Me.bbiPrint.Enabled = ClsDataDetail.SelectedListArt = Me.Cbo_ListingArt.Text.Substring(0, 1)
		'	Me.bbiExport.Enabled = Me.bbiPrint.Enabled
		'End If

		' Betrag Von/Bis
		Me.lblOffenerbetrag.Enabled = Cbo_ListingArt.Text.Contains("1 -")
		Me.txt_OpenBetrag_1.Enabled = Cbo_ListingArt.Text.Contains("1 -")
		Me.txt_OpenBetrag_2.Enabled = Cbo_ListingArt.Text.Contains("1 -")
		Me.txt_OpenBetrag_1.Text = If(Not Cbo_ListingArt.Text.Contains("1 -"), "0.00", Me.txt_OpenBetrag_1.Text)
		Me.txt_OpenBetrag_2.Text = If(Not Cbo_ListingArt.Text.Contains("1 -"), "0.00", Me.txt_OpenBetrag_2.Text)


		' Erstellungsdatum Von/Bis
		grpInvoiceDate.Enabled = Not (Val(Cbo_ListingArt.Text) = 4)

		deStichtag.Enabled = Not (Val(Cbo_ListingArt.Text) = 0 OrElse Val(Cbo_ListingArt.Text) = 3 OrElse Val(Cbo_ListingArt.Text) = 5 OrElse Val(Cbo_ListingArt.Text) = 6)
		If (Val(Cbo_ListingArt.Text) = 0 OrElse Val(Cbo_ListingArt.Text) = 3 OrElse Val(Cbo_ListingArt.Text) = 5 OrElse Val(Cbo_ListingArt.Text) = 6) Then
			deStichtag.EditValue = Nothing
		End If
		If Cbo_ListingArt.Text.Contains("4 -") Then
			lueDebitorenart.EditValue = Nothing
			deStichtag.EditValue = Now.Date

			deFakDate_1.EditValue = If(Cbo_ListingArt.Text.Contains("4 -"), Nothing, Me.deFakDate_1.Text)
			deFakDate_2.EditValue = If(Cbo_ListingArt.Text.Contains("4 -"), Nothing, Me.deFakDate_2.Text)

			deCreated_1.EditValue = If(Cbo_ListingArt.Text.Contains("4 -"), Nothing, Me.deCreated_1.Text)
			deCreated_2.EditValue = If(Cbo_ListingArt.Text.Contains("4 -"), Nothing, Me.deCreated_2.Text)

		End If

		deFakDate_1.Enabled = String.IsNullOrWhiteSpace(Cbo_FakPeriode.Text)
		deFakDate_2.Enabled = String.IsNullOrWhiteSpace(Cbo_FakPeriode.Text)
		deCreated_1.Enabled = String.IsNullOrWhiteSpace(Cbo_ErstellPeriode.Text)
		deCreated_2.Enabled = String.IsNullOrWhiteSpace(Cbo_ErstellPeriode.Text)

	End Sub

	Private Sub Cbo_ListingArt_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Cbo_ListingArt.QueryPopUp
		If Me.Cbo_ListingArt.Properties.Items.Count = 0 Then ListArt(Cbo_ListingArt)
	End Sub

	Private Sub frmOPSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmOPSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub


#Region "Sonstige Funktionen..."

	'Private Sub LookupCompleted(ByVal sender As Object, ByVal e As WeatherLookupCompletedEventArgs)

	'  ' Das Aktualisierungsobjekt erstellen.
	'  Dim Updater As New ControlTextUpdater(txt_SQLQuery)
	'  ' Eine thread-sichere Aktualisierung durchführen.
	'  Updater.AddText(e.CityName & " ist: " & e.Temperature.ToString() & vbNewLine)

	'  ' Das Task-Objekt löschen.
	'  SyncLock TaskThreads
	'    'For i As Integer = 1 To TaskThreads.Count
	'    '  TaskThreads.Remove(i)
	'    Dim Hauptthread As Threading.Thread = Threading.Thread.CurrentThread
	'    TaskThreads.Remove(Hauptthread.ToString)
	'    '        TaskThreads.Remove(e.TaskGuid.ToString())
	'    '      Next
	'  End SyncLock

	'End Sub

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object,
																			 ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		Stopwatch.Reset()
		Stopwatch.Start()

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		e.Result = GetMyQueryString() ' FillrecData2Array(Me.LvFoundedrecs, Me.txt_SQLQuery.Text)
		If bw.CancellationPending Then e.Cancel = True

	End Sub

	Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object,
																								ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
		Trace.WriteLine(e.ToString)
	End Sub

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object,
																									 ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(e.Error.Message)
		Else
			If e.Cancelled = True Then
				MessageBox.Show(m_Translate.GetSafeTranslationValue("Aktion abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()
				'        MessageBox.Show(e.Result.ToString())

			End If
		End If

	End Sub

	'Private Sub EnableListToPrint()

	'  ' Statistik bezahlter Rechnungen bei List-Art 1 - Offene/Teil-Offene Rechnungen de- bzw. aktivieren
	'  If ClsDataDetail.SelectedListArt = "1" Then
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintStatBezRechn").Enabled = False
	'  Else
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintStatBezRechn").Enabled = True
	'  End If

	'  ' Listen der offenen Rechnungen und Verfallkalender bei List-Art 2 - Gebuchte (bezahlte) Rechnungen de- bzw. aktivieren
	'  If ClsDataDetail.SelectedListArt = "2" Then
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintFaelligkeit").Enabled = False
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintNachFakturadatum").Enabled = False
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintVerfallkalender").Enabled = False
	'  Else
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintFaelligkeit").Enabled = True
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintNachFakturadatum").Enabled = True
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintVerfallkalender").Enabled = True
	'  End If

	'  ' Bei mehrere Faktura-Arten alle Statistiken deaktivieren
	'  If Cbo_OPArt.Text.Split(CChar(",")).Count > 1 Then
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintStatBezRechn").Enabled = False
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintFaelligkeit").Enabled = False
	'    DirectCast(Me.StatusStrip1.Items("btnPrint"),  _
	'      ToolStripDropDownButton).DropDownItems("mnuOPListPrintNachFakturadatum").Enabled = False
	'  End If

	'End Sub

#End Region

	Private Sub frmOPSearch_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
		Me.txtOPNr_1.Focus()
	End Sub


	Private Sub Cbo_FakPeriode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_FakPeriode.SelectedIndexChanged
		'    SetPeriodDate(DirectCast(sender, myCbo), Me.deFakDate_1, Me.deFakDate_2)
		Dim selectedItem As String = Me.Cbo_FakPeriode.Text '.ToItem.Value_1
		Dim dayOfWeek As Integer = Integer.Parse(Date.Now.DayOfWeek.ToString("D"))
		Dim month As Integer = Date.Now.Month
		Dim year As Integer = Date.Now.Year

		' Eingabe von ein neuenm Datum sperren
		Me.deFakDate_1.Enabled = False
		Me.deFakDate_2.Enabled = False

		' Die Woche fängt mit Sonntag=0 an, somit muss eine Woche abgezogen werden,
		' falls am Sonntag die Abfrage gestartet wird.
		If dayOfWeek = 0 Then
			dayOfWeek = 7
		End If
		Dim cv As ComboValue = DirectCast(sender.selecteditem, ComboValue)
		If cv Is Nothing Then Exit Sub
		Select Case cv.ComboValue
			Case "LW" ' Letzte Woche
				' Ob er am Montag oder Sonntag die Daten der letzten Woche abfragt,
				' so das Wochentag berücksichtigen und abziehen
				Me.deFakDate_1.Text = Date.Now.AddDays(-6 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deFakDate_2.Text = Date.Now.AddDays(0 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "DW" ' Diese Woche
				Me.deFakDate_1.Text = Date.Now.AddDays(1 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deFakDate_2.Text = Date.Now.AddDays(7 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "NW" ' Nächste Woche
				Me.deFakDate_1.Text = Date.Now.AddDays(8 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deFakDate_2.Text = Date.Now.AddDays(14 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "LM" ' Letzter Monat
				If month = 1 Then
					month = 12
					year -= 1
				Else
					month -= 1
				End If
				Me.deFakDate_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deFakDate_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "DM" ' Diesen Monat
				Me.deFakDate_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deFakDate_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "NM" ' Nächsten Monat
				month += 1
				Me.deFakDate_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deFakDate_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "LJ" ' Letztes Jahr
				year -= 1
				Me.deFakDate_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deFakDate_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "DJ" ' Dieses Jahr
				Me.deFakDate_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deFakDate_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "NJ" ' Nächstes Jahr
				year += 1
				Me.deFakDate_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deFakDate_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case Else
				Me.deFakDate_1.Enabled = True
				Me.deFakDate_2.Enabled = True

		End Select


	End Sub

	Private Sub Cbo_ErstellPeriode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ErstellPeriode.SelectedIndexChanged
		'SetPeriodDate(DirectCast(sender, myCbo), Me.deCreated_1, Me.deCreated_2)
		Dim selectedItem As String = Me.Cbo_ErstellPeriode.Text '.ToItem.Value_1
		Dim dayOfWeek As Integer = Integer.Parse(Date.Now.DayOfWeek.ToString("D"))
		Dim month As Integer = Date.Now.Month
		Dim year As Integer = Date.Now.Year

		' Eingabe von ein neuenm Datum sperren
		Me.deCreated_1.Enabled = False
		Me.deCreated_2.Enabled = False

		' Die Woche fängt mit Sonntag=0 an, somit muss eine Woche abgezogen werden,
		' falls am Sonntag die Abfrage gestartet wird.
		If dayOfWeek = 0 Then
			dayOfWeek = 7
		End If
		Dim cv As ComboValue = DirectCast(sender.selecteditem, ComboValue)
		If cv Is Nothing Then Exit Sub
		Select Case cv.ComboValue
			Case "LW" ' Letzte Woche
				' Ob er am Montag oder Sonntag die Daten der letzten Woche abfragt,
				' so das Wochentag berücksichtigen und abziehen
				Me.deCreated_1.Text = Date.Now.AddDays(-6 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deCreated_2.Text = Date.Now.AddDays(0 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "DW" ' Diese Woche
				Me.deCreated_1.Text = Date.Now.AddDays(1 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deCreated_2.Text = Date.Now.AddDays(7 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "NW" ' Nächste Woche
				Me.deCreated_1.Text = Date.Now.AddDays(8 - dayOfWeek).ToString("dd.MM.yyyy")
				Me.deCreated_2.Text = Date.Now.AddDays(14 - dayOfWeek).ToString("dd.MM.yyyy")
			Case "LM" ' Letzter Monat
				If month = 1 Then
					month = 12
					year -= 1
				Else
					month -= 1
				End If
				Me.deCreated_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deCreated_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "DM" ' Diesen Monat
				Me.deCreated_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deCreated_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "NM" ' Nächsten Monat
				month += 1
				Me.deCreated_1.Text = Date.Parse(String.Format("01.{0}.{1}", month, year))
				Me.deCreated_2.Text = Date.Parse(String.Format("{0}.{1}.{2}",
										Date.DaysInMonth(year, month), month, year))
			Case "LJ" ' Letztes Jahr
				year -= 1
				Me.deCreated_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deCreated_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "DJ" ' Dieses Jahr
				Me.deCreated_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deCreated_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case "NJ" ' Nächstes Jahr
				year += 1
				Me.deCreated_1.Text = Date.Parse(String.Format("01.01.{0}", year))
				Me.deCreated_2.Text = Date.Parse(String.Format("31.12.{0}", year))
			Case Else
				Me.deCreated_1.Enabled = True
				Me.deCreated_2.Enabled = True

		End Select

	End Sub


	Private Sub LblSetting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

		My.Settings.LV_1_Size = ""
		My.Settings.Rec_KDNr_Size = ""
		My.Settings.Rec_OPNr_Size = ""

		My.Settings.Save()

	End Sub

	Private Sub btnPrint_DropDownOpening(sender As Object, e As System.EventArgs)
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


#Region "Wisch Buttons..."

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtOPNr_2.Visible = Me.SwitchButton1.Value
		Me.txtOPNr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txtKDNr_2.Visible = Me.SwitchButton2.Value
		Me.txtKDNr_2.Text = String.Empty
	End Sub

	Private Sub CboSort_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(CboSort)
	End Sub

	Private Sub CboSort_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboSort.QueryCloseUp
		_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\SearchFields", Me.Name & "_0", Me.CboSort.Text)
		ClsDataDetail.GetLVSortBez = String.Empty
	End Sub

	Private Sub Cbo_MahnCode_DropDown(sender As Object, e As System.ComponentModel.CancelEventArgs)

	End Sub

	Private Sub Cbo_OPArt_DropDown(sender As Object, e As System.ComponentModel.CancelEventArgs)

	End Sub

#End Region

	Private Sub LoadPaymetReminderDropDownData()
		Dim paymentReminderCodeData = m_ListingDatabaseAccess.LoadPaymentReminderCodeInInvoicesData()

		If (paymentReminderCodeData Is Nothing) Then
			m_Logger.LogError("paymentreminder could not be loaded.")
			m_UtilityUI.ShowErrorDialog("Mahnungscodes konnten nicht geladen werden.")
		End If

		Dim listOfPaymentReminderCodeViewData = Nothing
		If Not paymentReminderCodeData Is Nothing Then

			listOfPaymentReminderCodeViewData = New List(Of PaymentReminderViewData)
			For Each paymentReminderCode In paymentReminderCodeData

				Dim paymentReminderCodeViewData = New PaymentReminderViewData With {
							.PaymentReminderCode = paymentReminderCode.GetField,
							.PaymentReminderDataString = String.Format("{0} ({1})-{2}-{3}-{4}",
																							paymentReminderCode.GetField,
																							paymentReminderCode.Reminder1,
																							paymentReminderCode.Reminder2,
																							paymentReminderCode.Reminder3,
																							paymentReminderCode.Reminder4)
							}

				listOfPaymentReminderCodeViewData.Add(paymentReminderCodeViewData)

			Next

		End If

		luePaymentReminderCode.Properties.DataSource = listOfPaymentReminderCodeViewData
		luePaymentReminderCode.Properties.ForceInitialize()

	End Sub

	Private Sub LoadDebitorenartDropDown()
		Dim listOfPaymentReminderCodeViewData = Nothing
		Dim paymentReminderCodeData = m_ListingDatabaseAccess.LoadInvoiceArtCodeInInvoicesData()

		If (paymentReminderCodeData Is Nothing) Then
			m_Logger.LogError("invoice art could not be loaded.")
			m_UtilityUI.ShowErrorDialog("Debitorenart Daten konnten nicht geladen werden.")
		End If

		listOfPaymentReminderCodeViewData = New List(Of InvoiceArtData)
		For Each paymentReminderCode In paymentReminderCodeData

			Dim paymentReminderCodeViewData = New InvoiceArtData With {.Art = paymentReminderCode.Art, .ArtLabel = m_Translate.GetSafeTranslationValue(paymentReminderCode.ArtLabel)}

			listOfPaymentReminderCodeViewData.Add(paymentReminderCodeViewData)

		Next


		lueDebitorenart.Properties.DataSource = listOfPaymentReminderCodeViewData
		lueDebitorenart.Properties.ForceInitialize()

	End Sub



	Private Class PaymentReminderViewData
		Public Property PaymentReminderCode As String
		Public Property PaymentReminderDataString As String
	End Class

End Class

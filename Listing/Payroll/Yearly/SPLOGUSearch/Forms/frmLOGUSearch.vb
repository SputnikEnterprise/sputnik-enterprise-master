
'Imports System.Reflection.Assembly

'Imports System.Data.SqlClient
'Imports System.Text.RegularExpressions
'Imports System.Runtime.CompilerServices
'Imports System.Drawing

Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.SPUserSec.ClsUserSec
'Imports SPS.Listing.Print.Utility
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.Utility

'Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath

Public Class frmLOGUSearch


#Region "private Const"

	Private Const STRINGSEPRATER As String = "#@"
	Private Const MANDANT_XML_SETTING_SPUTNIK_PAYROLL_SETTING As String = "MD_{0}/Lohnbuchhaltung"
	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Private fields"


	Private m_Logger As ILogger = New Logger()
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_mandant As Mandant
	Protected m_Utility As SPProgUtility.MainUtilities.Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI
	Private m_ProgPath As ClsProgPath


	Private Shared frmMyLV As frmListeSearch_LV
	Private Const frmMyLVName As String = "frmListeSearch_LV"
	Private m_MandantFormXMLFile As String
	'Private m_ExistsTolerantAmount As Boolean

	Private m_SearchCriteria As New SearchCriteria

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean


#End Region



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_mandant = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
		m_ProgPath = New ClsProgPath

		m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
		If Not System.IO.File.Exists(m_MandantFormXMLFile) Then
			m_UtilityUi.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantFormXMLFile))
		End If

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		ResetMandantenDropDown()
		TranslateControls()

		LoadMandantenDropDown()
		SetInitialFields()

		AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

	End Sub


#End Region


#Region "private properties"

	Private ReadOnly Property GetHwnd() As String
		Get
			Return Me.Handle
		End Get
	End Property

	Private ReadOnly Property GetJobNr() As String
		Get
			Return "11.4.2"

		End Get
	End Property

	'Private ReadOnly Property ExistsTolerantAmount() As Boolean
	'	Get

	'		Dim minAmountfeiertaginpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountfeiertaginpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
	'		Dim minAmountFerienInpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountFerienInpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
	'		Dim minAmount13LohnInpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmount13LohnInpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
	'		Dim minAmountDarleheninpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountDarleheninpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
	'		Dim minAmountGleitStdinpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountGleitStdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)
	'		Dim minAmountNightStdinpayslip = ParseToDec(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/minAmountNightStdinpayslip", FORM_XML_DEFAULTVALUES_KEY)), 0)

	'		Return minAmountfeiertaginpayslip + minAmountFerienInpayslip + minAmount13LohnInpayslip + minAmountDarleheninpayslip + minAmountGleitStdinpayslip + minAmountNightStdinpayslip + Val(txtTolerancelimit.EditValue) > 0
	'	End Get

	'End Property

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
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(CInt(lueMandant.EditValue), ClsDataDetail.m_InitialData.UserData.UserNr)

			m_InitializationData = ClsDataDetail.m_InitialData
			ShowHideOldLANrControl()

		Else
			' do nothing
		End If

		m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
		If Not System.IO.File.Exists(m_MandantFormXMLFile) Then
			m_UtilityUi.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantFormXMLFile))
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = False ' Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiExport.Enabled = False ' Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub


#End Region


#Region "Allgemeine Funktionen"

	' Monate 1 bis 12
	Private Sub Monate1bis12(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbo_MonatBis.QueryPopUp
		If TypeOf (sender) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cbo.Properties.Items.Count = 0 Then ListCboMonate1Bis12(cbo)
		End If
	End Sub

#End Region

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmLOGUSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			FormIsLoaded(frmMyLVName, True)

			If Not Me.WindowState = FormWindowState.Minimized Then

				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				' Checkboxes
				My.Settings.chk_Feiertag_Checked = Me.ChkFeiertag.Checked
				My.Settings.chk_Ferien_Checked = Me.ChkFerien.Checked
				My.Settings.chk_13Lohn_Checked = Me.Chk13Lohn.Checked
				My.Settings.chk_Darlehen_Checked = Me.ChkDarlehen.Checked
				My.Settings.chk_Gleitstunden_Checked = Me.ChkGleitstunden.Checked
				My.Settings.chk_FeiertagVJ_Checked = Me.ChkFeiertagVJ.Checked
				My.Settings.chk_FerienVJ_Checked = Me.ChkFerienVJ.Checked
				My.Settings.chk_13LohnVJ_Checked = Me.Chk13LohnVJ.Checked

				My.Settings.Tolerancelimit = Val(txtTolerancelimit.EditValue)

				My.Settings.Save()

			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub OnForm_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		Try
			If FormIsLoaded(frmMyLVName, False) Then
				frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
				frmMyLV.TopMost = True
				frmMyLV.TopMost = False
			End If
		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	''' <summary>
	'''  trannslate controls
	''' </summary>
	''' <remarks></remarks>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSQL.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQL.Text)
		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.grpGuthaben.Text = m_Translate.GetSafeTranslationValue(Me.grpGuthaben.Text)
		Me.ChkFeiertag.Text = m_Translate.GetSafeTranslationValue(Me.ChkFeiertag.Text)
		Me.ChkFerien.Text = m_Translate.GetSafeTranslationValue(Me.ChkFerien.Text)
		Me.Chk13Lohn.Text = m_Translate.GetSafeTranslationValue(Me.Chk13Lohn.Text)

		Me.grpGuthabenvorjahr.Text = m_Translate.GetSafeTranslationValue(Me.grpGuthabenvorjahr.Text)
		Me.ChkFeiertagVJ.Text = m_Translate.GetSafeTranslationValue(Me.ChkFeiertagVJ.Text)
		Me.ChkFerienVJ.Text = m_Translate.GetSafeTranslationValue(Me.ChkFerienVJ.Text)
		Me.Chk13LohnVJ.Text = m_Translate.GetSafeTranslationValue(Me.Chk13LohnVJ.Text)

		Me.grpDarlehen.Text = m_Translate.GetSafeTranslationValue(Me.grpDarlehen.Text)
		Me.ChkDarlehen.Text = m_Translate.GetSafeTranslationValue(Me.ChkDarlehen.Text)
		Me.ChkGleitstunden.Text = m_Translate.GetSafeTranslationValue(Me.ChkGleitstunden.Text)

		Me.lblToleranzgrenze.Text = m_Translate.GetSafeTranslationValue(lblToleranzgrenze.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)


	End Sub

	''' <summary>
	''' Beim Starten der Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmLOGUSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		Try
			Me.ChkFeiertag.Checked = My.Settings.chk_Feiertag_Checked
			Me.ChkFerien.Checked = My.Settings.chk_Ferien_Checked
			Me.Chk13Lohn.Checked = My.Settings.chk_13Lohn_Checked

			Me.ChkFeiertagVJ.Checked = My.Settings.chk_FeiertagVJ_Checked
			Me.ChkFerienVJ.Checked = My.Settings.chk_FerienVJ_Checked
			Me.Chk13LohnVJ.Checked = My.Settings.chk_13LohnVJ_Checked

			Me.ChkDarlehen.Checked = My.Settings.chk_Darlehen_Checked
			Me.ChkGleitstunden.Checked = My.Settings.chk_Gleitstunden_Checked

			Me.txtTolerancelimit.EditValue = Val(My.Settings.Tolerancelimit)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try


	End Sub

	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
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
			Dim visibleMDSelection = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)
			Me.lueMandant.Visible = visibleMDSelection
			Me.lblMDName.Visible = visibleMDSelection

			FillDefaultValues()

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

		Try
			' Monat bis
			If Me.cbo_MonatBis.Properties.Items.Count = 0 Then ListCboMonate1Bis12(Me.cbo_MonatBis)
			' Bis am 10. des Monats wird der Vormonat selektiert. Ab 11. den aktuellen Monat
			Dim datum As Date = Date.Now
			If datum.Day < 11 Then datum = datum.AddMonths(-1)
			Me.cbo_MonatBis.EditValue = datum.Month

			' Jahr bis
			ListLOJahr(Me.cbo_JahrBis)
			Me.cbo_JahrBis.EditValue = datum.Year


		Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

	Private Sub Oncbo_JahrBis_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_JahrBis.SelectedValueChanged
		ShowHideOldLANrControl()
	End Sub

	Private Sub Oncbo_MonatBiss_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_MonatBis.SelectedValueChanged
		ShowHideOldLANrControl()
	End Sub

	Private Sub ShowHideOldLANrControl()
		Dim _ClsFunc As New ClsDbFunc
		Dim allowedtoShow = _ClsFunc.ExistsLastYearLAInLOL(m_InitializationData.MDData.MDNr, cbo_JahrBis.EditValue, cbo_MonatBis.EditValue)

		ChkFerienVJ.Checked = allowedtoShow
		ChkFeiertagVJ.Checked = allowedtoShow
		Chk13LohnVJ.Checked = allowedtoShow
		grpGuthabenvorjahr.Enabled = allowedtoShow

	End Sub

	Sub StartPrinting()
		Dim strFilter As String = String.Empty

		bPrintAsDesign = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 560)
		PrintJobNr = GetJobNr()
		SQL4Print = Me.txt_SQLQuery.Text

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine

		strFilter &= If(m_SearchCriteria.bismonat.Length > 0, String.Format("Monat bis {0}", m_SearchCriteria.bismonat), String.Empty)
		strFilter &= If(m_SearchCriteria.bisjahr.Length > 0, String.Format("Jahr bis {0}", m_SearchCriteria.bisjahr), String.Empty)

		strFilter &= If(m_SearchCriteria.manr.Length > 0, String.Format("{1}Kandidaten: {0}", m_SearchCriteria.manr, vbNewLine), String.Empty)

		strFilter &= If(m_SearchCriteria.ShowG500.HasValue AndAlso m_SearchCriteria.ShowG500, String.Format("Feiertag-Guthaben{0}", vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.ShowG600.HasValue AndAlso m_SearchCriteria.ShowG600, String.Format("Ferien-Guthaben{0}", vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.ShowG700.HasValue AndAlso m_SearchCriteria.ShowG700, String.Format("13. Lohn-Guthaben{0}", vbNewLine), String.Empty)

		strFilter &= If(m_SearchCriteria.ShowG529.HasValue AndAlso m_SearchCriteria.ShowG529, String.Format("Feiertag-Guthaben (Vorjahr){0}", vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.ShowG629.HasValue AndAlso m_SearchCriteria.ShowG629, String.Format("Ferien-Guthaben (Vorjahr){0}", vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.ShowG729.HasValue AndAlso m_SearchCriteria.ShowG729, String.Format("13. Lohn-Guthaben (Vorjahr){0}", vbNewLine), String.Empty)

		strFilter &= If(m_SearchCriteria.ShowGDar.HasValue AndAlso m_SearchCriteria.ShowGDar, String.Format("Darlehen{0}", vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.ShowGGTime.HasValue AndAlso m_SearchCriteria.ShowGGTime, String.Format("Gleitzeit{0}", vbNewLine), String.Empty)

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLGUSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
			.SelectedMDNr = m_InitializationData.MDData.MDNr,
																																									.SQL2Open = Me.SQL4Print,
																																									.JobNr2Print = Me.PrintJobNr,
																																									.ListBez2Print = m_SearchCriteria.listname,
																																									.frmhwnd = GetHwnd,
																																									.ShowAsDesign = Me.bPrintAsDesign,
																																									.ShowG500 = m_SearchCriteria.ShowG500,
																																									.ShowG600 = m_SearchCriteria.ShowG600,
																																									.ShowG700 = m_SearchCriteria.ShowG700,
																																									.ShowG529 = m_SearchCriteria.ShowG529,
																																									.ShowG629 = m_SearchCriteria.ShowG629,
																																									.ShowG729 = m_SearchCriteria.ShowG729,
																																									.ShowGGTime = m_SearchCriteria.ShowGGTime,
																																									.ShowGDar = m_SearchCriteria.ShowGDar,
																																									.ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)
																																																																		})}

		Dim obj As New SPS.Listing.Print.Utility.GUSearchListing.ClsPrintGUSearchList(m_InitializationData)
		obj.PrintData = _Setting

		obj.PrintGUListing()


	End Sub


#Region "Funktionen zur Menüaufbau..."


	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Try
			If Me.cbo_MonatBis.Text = String.Empty Then Me.cbo_MonatBis.Text = CStr(Month(Now))
			If Me.cbo_JahrBis.Text = String.Empty Then Me.cbo_JahrBis.Text = CStr(Year(Now))

			m_SearchCriteria = GetSearchKrieteria()
			If Not (Kontrolle()) Then Return
			Me.txt_SQLQuery.Text = String.Empty

			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded(frmMyLVName, True)

			' Die Query-String aufbauen...
			GetMyQueryString()


		Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		Dim bisjahr As String = cbo_JahrBis.Text
		Dim bismonat As String = cbo_MonatBis.Text
		Dim loarten As String = String.Empty

		If Me.cbo_MonatBis.Text = String.Empty Then Me.cbo_MonatBis.Text = CStr(Month(Now))
		If Me.cbo_JahrBis.Text = String.Empty Then Me.cbo_JahrBis.Text = CStr(Year(Now))
		bismonat = cbo_MonatBis.Text
		bisjahr = cbo_JahrBis.Text

		result.listname = m_Translate.GetSafeTranslationValue("Aufstellung über Kandidatenguthaben")
		result.mandantenname = lueMandant.Text

		result.manr = txt_MANr.Text
		result.bisjahr = bisjahr
		result.bismonat = bismonat

		result.ShowG500 = ChkFeiertag.Checked
		result.ShowG600 = ChkFerien.Checked
		result.ShowG700 = Chk13Lohn.Checked

		result.ShowG529 = ChkFeiertagVJ.Checked
		result.ShowG629 = ChkFerienVJ.Checked
		result.ShowG729 = Chk13LohnVJ.Checked

		result.ShowGDar = ChkDarlehen.Checked
		result.ShowGGTime = ChkGleitstunden.Checked

		result.ToleranceLimit = Val(txtTolerancelimit.EditValue)
		'result.ExistsTolerantAmount = ExistsTolerantAmount

		Return result

	End Function

	Function Kontrolle() As Boolean
		Try
			Dim msg As String = ""

			If Me.txt_MANr.Text.Trim.Length > 0 Then
				For Each manr As String In Me.txt_MANr.Text.Split(CChar(","))
					If Not IsNumeric(manr) Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), manr, vbLf)
					ElseIf CInt(manr).ToString <> manr Then
						msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist ungültig.{1}"), manr, vbLf)
					End If
				Next
			End If
			If Not ChkFeiertag.Checked AndAlso Not ChkFerien.Checked AndAlso Not Chk13Lohn.Checked _
				AndAlso Not ChkFeiertagVJ.Checked AndAlso Not ChkFerienVJ.Checked AndAlso Not Chk13LohnVJ.Checked _
				AndAlso Not ChkDarlehen.Checked AndAlso Not ChkGleitstunden.Checked Then
				msg += String.Format(m_Translate.GetSafeTranslationValue("Sie haben keine Spalte zur Auflistung ausgewählt!"), vbLf)
			End If

			If msg.Length > 0 Then
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien unvollständig:{0}{1}"), vbLf, msg),
												m_Translate.GetSafeTranslationValue("Keine Suche möglich"), MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return False
			End If
		Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)
		End Try

		' Da ein threadübergreifender Zugriff nicht erlaubt ist, müssen Parameter übertragen werden
		ClsDataDetail.Param.MonatBis = Me.cbo_MonatBis.Text
		ClsDataDetail.Param.JahrBis = Me.cbo_JahrBis.Text
		ClsDataDetail.Param.MANR = Me.txt_MANr.Text

		Return True
	End Function

	Function GetMyQueryString() As Boolean

		Try
			BackgroundWorker1.WorkerSupportsCancellation = True
			BackgroundWorker1.WorkerReportsProgress = True
			BackgroundWorker1.RunWorkerAsync()    ' Multithreading starten

		Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)
		End Try

		Return True

	End Function


#Region "Multitreading..."

	Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

		CheckForIllegalCrossThreadCalls = False
		Dim bw As System.ComponentModel.BackgroundWorker = DirectCast(sender, System.ComponentModel.BackgroundWorker)

		Dim i As Integer = 0
		Dim cForeColor As New System.Drawing.Color

		Me.bbiSearch.Enabled = False

		Try
			Dim sSqlQuerySelect As String = String.Empty

			Dim _ClsDb As New ClsDbFunc()

			Me.txt_SQLQuery.Text = _ClsDb.GetQuerySQLString(m_SearchCriteria)
			Me.txt_SQLQuery.Text = String.Format("SELECT * FROM {0} Order By MANachname, MAVorname", ClsDataDetail.LLTablename)


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

	Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object,
																									 ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
																									 Handles BackgroundWorker1.RunWorkerCompleted

		If (e.Error IsNot Nothing) Then
			MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Fehler in Ihrer Anwendung.{0}{1}"), vbNewLine, e.Error.Message))
		Else
			If e.Cancelled = True Then
				MessageBox.Show(m_Translate.GetSafeTranslationValue("Aktion abgebrochen!"))

			Else
				BackgroundWorker1.CancelAsync()

				' Daten auflisten...
				If Not FormIsLoaded(frmMyLVName, True) Then
					frmMyLV = New frmListeSearch_LV(Me.txt_SQLQuery.Text, String.Empty)
					frmMyLV.Show()
					Me.Select()
				End If

				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", frmMyLV.RecCount)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...", frmMyLV.RecCount)

				Me.ResumeLayout()

			End If

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				'CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

		End If
		Me.bbiSearch.Enabled = True

	End Sub

#End Region



	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
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
				StartExportModul()
				'Dim ExportThread As New Thread(AddressOf StartExportModul)
				'ExportThread.Name = "LOGUTOCSV"
				'ExportThread.Start()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = Me.txt_SQLQuery.Text,
																																			 .ModulName = "LOGUTOCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		_Setting.ModulName = "LOGUToCSV"
		obj.ExportCSVFromLOGUListing(Me.txt_SQLQuery.Text)

	End Sub


#End Region


#Region "zum Leeren..."

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		FormIsLoaded("frmListeSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()
		FillDefaultValues()

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiSearch.Enabled = True
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		' Checkbox defaults

		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item(Me.xtabMain.Name).Controls
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
	''' <remarks>Funktion ruft sich rekursiv auf.</remarks>
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

	''' <summary>
	''' Selektionsfenster für die erste Mitarbeiter-Nummer öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr.ButtonClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim frmTest As New frmSearchRec(m_InitializationData)
		frmTest.Field2Select = "MANr"
		frmTest.CurrentYear = Val(cbo_JahrBis.EditValue)
		frmTest.CurrentMonth = Val(cbo_MonatBis.EditValue)

		Dim m As String

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iListeValue()
		Me.txt_MANr.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(STRINGSEPRATER, ",")))
		frmTest.Dispose()

	End Sub

	Private Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function


End Class

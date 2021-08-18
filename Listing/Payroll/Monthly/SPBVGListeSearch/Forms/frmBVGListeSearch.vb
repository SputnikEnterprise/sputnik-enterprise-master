
Option Strict Off

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.Infrastructure.UI

Imports System.Reflection.Assembly
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPS.Listing.Print.Utility

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraSplashScreen
Imports System.Threading
Imports DevExpress.XtraEditors.Controls
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPBVGListeSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmBVGListeSearch

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

	Public Shared frmMyLV As frmBVGListeSearch_LV

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private m_md As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria


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

		Try
			Me.Chk_BVGListeNullBetrag.Checked = CBool(My.Settings.CheckedBVGListeNullBetrag)
			Me.Chk_BVGListeNurErstenES.Checked = CBool(My.Settings.CheckedBVGListeNurErstenES)

		Catch ex As Exception
			Me.Chk_BVGListeNullBetrag.Checked = True
			Me.Chk_BVGListeNurErstenES.Checked = True

		End Try

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
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(lueMandant.EditValue, ClsDataDetail.m_InitialData.UserData.UserNr)
			m_InitializationData = ChangeMandantData

			' Lohnarten wieder lesen, es kann sein, dass in dem neu gewählten Mandanten die alten Lohnarten nicht vorhanden sind!!!
			'Dim jahrvon As Integer? = Val(ReplaceMissing(Cbo_VonJahr.EditValue, Now.Year))
			'Dim jahrbis As Integer? = Val(ReplaceMissing(Cbo_BisJahr.EditValue, jahrvon))
			'Dim monatvon As Integer? = Val(ReplaceMissing(Cbo_MonatVon.EditValue, Now.Month))
			'Dim monatbis As Integer? = Val(ReplaceMissing(Cbo_MonatBis.EditValue, monatvon))

			'ListBVGListLohnarten(Me.Cbo_Lohnart, jahrvon, jahrbis, monatvon, monatbis)
			LoadLAList()
			If Cbo_Lohnart.Properties.Items.Count > 0 Then
				Cbo_Lohnart.EditValue = Cbo_Lohnart.Properties.Items(0)
			End If

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



#Region "Dropdown Funktionen Allgemein"

	Private Sub Cbo_QSTListePeriode_QueryPopUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_Periode.QueryPopUp
		Dim datum As Date = Date.Now
		Me.Cbo_Periode.Properties.Items.Clear()

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

	Private Sub OnCbo_VonJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_VonJahr.QueryPopUp
		ListBVGListeJahr(Me.Cbo_VonJahr)
	End Sub

	Private Sub OnCbo_BisJahr_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_BisJahr.QueryPopUp
		ListBVGListeJahr(Me.Cbo_BisJahr)
	End Sub

	Private Sub OnCbo_VonMonat_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MonatVon.QueryPopUp
		ListBVGListeMonth(Me.Cbo_MonatVon, Cbo_VonJahr.EditValue)
	End Sub

	Private Sub OnCbo_BisMonat_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_MonatBis.QueryPopUp
		ListBVGListeMonth(Me.Cbo_MonatBis, Cbo_VonJahr.EditValue)
	End Sub


#End Region


#Region "Allgemeine Funktionen"

	Private Sub Checkbox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)
		If TypeOf (sender) Is DevExpress.XtraEditors.CheckEdit Then
			Dim chk = DirectCast(sender, DevExpress.XtraEditors.CheckEdit)
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

	Private Sub frmBVGListeSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmBVGListeSearch_LV", True)
		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			My.Settings.CheckedBVGListeNurErstenES = Me.Chk_BVGListeNurErstenES.Checked
			My.Settings.CheckedBVGListeNullBetrag = Me.Chk_BVGListeNullBetrag.Checked

			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmBVGListeSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmBVGListeSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblMANr.Text = m_Translate.GetSafeTranslationValue(Me.lblMANr.Text)

		Me.lblPeriode.Text = m_Translate.GetSafeTranslationValue(Me.lblPeriode.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_Translate.GetSafeTranslationValue(Me.lblMonat.Text)
		Me.lblLohnart.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnart.Text)

		Me.Chk_BVGListeNullBetrag.Text = m_Translate.GetSafeTranslationValue(Me.Chk_BVGListeNullBetrag.Text)
		Me.Chk_BVGListeNurErstenES.Text = m_Translate.GetSafeTranslationValue(Me.Chk_BVGListeNurErstenES.Text)

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
	Private Sub frmBVGListeSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

		SetInitialFields()

		Dim UpdateDelegate As New MethodInvoker(AddressOf TranslateControls)
		Me.Invoke(UpdateDelegate)

		Me.xtabSQLQuery.PageVisible = m_InitializationData.UserData.UserNr = 1

		If m_InitializationData.UserData.UserNr = 1 Then
			Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		Else
			If IsUserAllowed4DocExport("12.7") Then Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
		End If


		Me.xtabSQLQuery.PageVisible = m_InitializationData.UserData.UserNr = 1


		FillDefaultValues()

		'AddHandler Me.Cbo_VonJahr.EditValueChanged, AddressOf OnCbo_MonthAndYearEditValueChanged
		'AddHandler Me.Cbo_BisJahr.EditValueChanged, AddressOf OnCbo_MonthAndYearEditValueChanged
		'AddHandler Me.Cbo_MonatVon.EditValueChanged, AddressOf OnCbo_MonthAndYearEditValueChanged
		'AddHandler Me.Cbo_MonatBis.EditValueChanged, AddressOf OnCbo_MonthAndYearEditValueChanged

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

	Private Sub frmESSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And ClsDataDetail.m_InitialData.UserData.UserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			m_UtilityUi.ShowInfoDialog(strMsg)
		End If

	End Sub


	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()

		' Bis am 14. des Monats wird der Vormonat selektiert. Ab 15. den aktuellen Monat
		FillDefaultDates()

		If My.Settings.CheckedBVGListeNurErstenES Then
			Me.Chk_BVGListeNurErstenES.Checked = True
		Else
			Me.Chk_BVGListeNurErstenES.Checked = False
		End If
		If My.Settings.CheckedBVGListeNullBetrag Then
			Me.Chk_BVGListeNullBetrag.Checked = True
		Else
			Me.Chk_BVGListeNurErstenES.Checked = False
		End If

		Me.bbiExport.Enabled = False
		Me.bbiPrint.Enabled = False

	End Sub

	Private Sub FillDefaultDates()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim datum As Date = Date.Now
			If datum.Day < 11 Then
				datum = datum.AddMonths(-1)
			End If
			Me.Cbo_VonJahr.EditValue = datum.Year
			Me.Cbo_BisJahr.EditValue = Nothing
			Me.Cbo_MonatVon.EditValue = datum.Month
			Me.Cbo_MonatBis.EditValue = Nothing

			Try
				'Dim jahrvon As Integer? = Val(ReplaceMissing(Cbo_VonJahr.EditValue, Now.Year))
				'Dim jahrbis As Integer? = Val(ReplaceMissing(Cbo_BisJahr.EditValue, jahrvon))
				'Dim monatvon As Integer? = Val(ReplaceMissing(Cbo_MonatVon.EditValue, Now.Month))
				'Dim monatbis As Integer? = Val(ReplaceMissing(Cbo_MonatBis.EditValue, monatvon))

				'ListBVGListLohnarten(Me.Cbo_Lohnart, jahrvon, jahrbis, monatvon, monatbis)
				LoadLAList()
				Cbo_Lohnart.EditValue = Cbo_Lohnart.Properties.Items(0)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try


	End Sub

	' ''' <summary>
	' ''' Daten fürs Drucken bereit stellen.
	' ''' </summary>
	' ''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	' ''' <param name="bForExport">ob die Liste für Export ist.</param>
	' ''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	' ''' <remarks></remarks>
	'Sub GetBVGListeData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
	'	Dim iESNr As Integer = 0
	'	Dim bResult As Boolean = True
	'	Dim storedProc As String = ""

	'	Dim sSql As String = Me.txt_SQLQuery.Text
	'	If sSql = String.Empty Then
	'		MsgBox(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"), MsgBoxStyle.Exclamation, "GetESData4Print")
	'		Exit Sub
	'	End If

	'	' Bei Formularanpassung nur die ersten 10 Datensätze holen
	'	If bForDesign Then sSql = Replace(UCase(sSql), "SELECT ", "SELECT TOP 10 ", 1, 1)

	'	'Daten sind auf der Datenbank
	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)

	'	Try
	'		Conn.Open()

	'		Dim rBVGListerec As SqlDataReader = cmd.ExecuteReader									 ' 

	'		Try
	'			If Not rBVGListerec.HasRows Then
	'				cmd.Dispose()
	'				rBVGListerec.Close()

	'				MessageBox.Show(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."), _
	'												"GetBVGListeData", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'			End If

	'		Catch ex As Exception
	'			MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetBVGListeData")

	'		End Try

	'		While rBVGListerec.Read

	'			'If LL.Core.Handle <> 0 Then LL.Core.LlJobClose()
	'			'LL.Core.LlJobOpen(2)

	'			If bForExport Then
	'				'            ExportLLDoc(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex),CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
	'				'          bResult = ExportLLDocWithStorage(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex), CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
	'			Else
	'				If bForDesign Then
	'					ShowInDesign(LL, strJobInfo, rBVGListerec)

	'				Else
	'					bResult = ShowInPrint(LL, strJobInfo, rBVGListerec)

	'				End If
	'			End If

	'			If Not bResult Or bForDesign Then Exit While
	'		End While
	'		rBVGListerec.Close()

	'	Catch ex As Exception
	'		MsgBox(ex.Message, MsgBoxStyle.Critical, "GetBVGListeData4Print")

	'	Finally
	'		cmd.Dispose()
	'		Conn.Close()
	'	End Try
	'End Sub


#Region "Funktionen zur Menüaufbau..."

	'Private Sub mnuBVGListeDrucken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	If Me.Cbo_MonatVon.Text <> Me.Cbo_MonatBis.Text Then
	'		GetBVGListeData4Print(False, False, "12.7.G")	'BVG-Liste gruppiert
	'	Else
	'		GetBVGListeData4Print(False, False, "12.7")	'BVG-Liste
	'	End If
	'End Sub
	'Private Sub mnuDesignBVGListe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	If Me.Cbo_MonatVon.Text <> Me.Cbo_MonatBis.Text Then
	'		GetBVGListeData4Print(True, False, "12.7.G") 'BVG-Liste gruppiert (Design)
	'	Else
	'		GetBVGListeData4Print(True, False, "12.7") 'BVG-Liste (Design)
	'	End If
	'End Sub

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)
		Me.Cursor = Cursors.WaitCursor

		Try
			m_SearchCriteria = GetSearchKrieteria()

			If Not (Kontrolle()) Then
				Exit Sub
			End If

			Me.txt_SQLQuery.Text = String.Empty

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded("frmBVGListeSearch_LV", True)

			' Die Query-String aufbauen...
			GetMyQueryString()

			' Daten auflisten...
			If Not String.IsNullOrWhiteSpace(Me.txt_SQLQuery.Text) Then
				If Not FormIsLoaded("frmBVGListeSearch_LV", True) Then
					frmMyLV = New frmBVGListeSearch_LV(txt_SQLQuery.Text, m_SearchCriteria)
					frmMyLV.Show()
					Me.Select()
				End If

				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."),
																					 frmMyLV.RecCount)
				frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."),
																								frmMyLV.RecCount)
			End If

			' Die Buttons Drucken und Export aktivieren
			If frmMyLV.RecCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

				'CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

			ClsDataDetail.LLSelektionText = ""
			ClsDataDetail.GetFilterBez += ClsDataDetail.LLSelektionText


		Catch ex As Exception
			m_UtilityUi.ShowErrorDialog(ex.ToString)
			Me.bbiSearch.Enabled = True

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Function Kontrolle() As Boolean
		Dim meldung As String = ""

		If lueMandant.EditValue Is Nothing Then meldung &= m_Translate.GetSafeTranslationValue("- Der Mandant muss ausgewählt werden.") & vbNewLine

		If Cbo_VonJahr.EditValue Is Nothing Then meldung &= m_Translate.GetSafeTranslationValue("- Der Jahr muss ausgewählt werden.") & vbNewLine
		If Cbo_MonatVon.EditValue Is Nothing Then meldung &= m_Translate.GetSafeTranslationValue("- Der Monat muss ausgewählt werden.") & vbNewLine
		If String.IsNullOrWhiteSpace(m_SearchCriteria.lohnarten) Then meldung &= m_Translate.GetSafeTranslationValue("- Die Lohnarten müssen ausgewählt werden.")


		If meldung.Length > 0 Then
			m_UtilityUi.ShowErrorDialog(String.Format("{1}:{0}{2}", vbNewLine, m_Translate.GetSafeTranslationValue("Kriterienauswahl unvollständig"), meldung))

			Return False
		End If

		Return True
	End Function

	Function GetSearchKrieteria() As SearchCriteria

		Dim result As New SearchCriteria

		Dim bisjahr As String = Cbo_BisJahr.EditValue
		Dim bismonat As String = Cbo_MonatBis.EditValue
		Dim loarten As String = String.Empty

		If Cbo_MonatBis.Visible Then
			If String.IsNullOrEmpty(Cbo_MonatBis.EditValue) Then bismonat = 12
		Else
			bismonat = Cbo_MonatVon.EditValue

		End If
		If Cbo_BisJahr.Visible Then
			If String.IsNullOrEmpty(Cbo_BisJahr.EditValue) Then bisjahr = Now.Year
		Else
			bisjahr = Cbo_VonJahr.Text

		End If

		If Cbo_Lohnart.Text.Contains("All") Then
			ListBVGListLohnarten(Me.Cbo_Lohnart, Val(Cbo_VonJahr.EditValue), Val(bisjahr), Val(Cbo_MonatVon.EditValue), Val(bismonat))
			Cbo_Lohnart.EditValue = If(Cbo_Lohnart.Properties.Items.Count = 0, Nothing, Cbo_Lohnart.Properties.Items(0))
		End If

		If Cbo_Lohnart.Text.Contains(",") Then
			loarten = Cbo_Lohnart.Text
		Else
			If Not TryCast(Cbo_Lohnart.SelectedItem, ComboValue) Is Nothing Then
				Dim cv As ComboValue
				cv = DirectCast(Cbo_Lohnart.SelectedItem, ComboValue)
				If Not cv Is Nothing Then
					loarten = cv.ComboValue
				End If

			Else
				For Each lonr As String In Cbo_Lohnart.Text.Split(CChar(","))
					loarten += String.Format("{0},", lonr.Trim)
				Next
				loarten = loarten.Remove(loarten.Length - 1, 1) ' Letzten Komma entfernen

			End If
		End If

		Me.PrintJobNr = "12.7"
		If Me.Cbo_MonatVon.EditValue <> Me.Cbo_MonatBis.EditValue And Me.Cbo_MonatBis.Visible Then Me.PrintJobNr &= ".G"

		result.jobnrforprint = Me.PrintJobNr
		result.mandantenname = lueMandant.EditValue
		result.manr = txt_MANR.Text
		result.vonjahr = Cbo_VonJahr.EditValue
		result.bisjahr = bisjahr
		result.vonmonat = Cbo_MonatVon.EditValue
		result.bismonat = bismonat
		result.lohnarten = loarten
		result.getfirstes = Chk_BVGListeNurErstenES.Checked
		result.deletenull = Chk_BVGListeNullBetrag.Checked

		Return result

	End Function

	Function GetMyQueryString() As Boolean
		Dim sSqlQuerySelect As String = String.Empty
		Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)

		Try

			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			Me.txt_SQLQuery.Text = _ClsDb.GetStartSQLString()

		Catch ex As Exception

		Finally
			SplashScreenManager.CloseForm(False)

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

		FormIsLoaded("frmBVGListeSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		Cbo_Lohnart.EditValue = Nothing
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
			If con.Name.ToLower = "CboSort".ToLower Or con.Name.ToLower = "luemandant".ToLower Then Exit Sub

			' Rekursiver Aufruf
			If con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else
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

				End If
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.Message))

		End Try

	End Sub

#End Region


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		StartPrinting()
		'GetMenuItem(Nothing, Nothing)

		'Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		'If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	'Private Sub CreatePrintPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Liste drucken#PrintList"}	'																					 "Entwurfsansicht#PrintDesign"}
	'	Try
	'		bbiPrint.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiPrint.ActAsDropDown = False
	'		Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiPrint.DropDownEnabled = True
	'		Me.bbiPrint.DropDownControl = popupMenu
	'		'Me.bbiPrint.Visibility = BarItemVisibility.Always
	'		Me.bbiPrint.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))

	'			'If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then bshowMnu = IsUserActionAllowed(0, 560) Else bshowMnu = myValue(0).ToString <> String.Empty
	'			bshowMnu = myValue(0).ToString <> String.Empty
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

	'	Me.SQL4Print = String.Empty
	'	Me.PrintJobNr = "12.7"
	'	If Me.Cbo_MonatVon.EditValue <> Me.Cbo_MonatBis.EditValue And Me.Cbo_MonatBis.Visible Then Me.PrintJobNr &= ".G"

	'	Try
	'		Try
	'			If ClsDataDetail.QSTListeDataTable.Rows.Count = 0 Then
	'				m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."))
	'			End If

	'		Catch ex As Exception
	'			DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message.ToString, strMethodeName, MessageBoxButtons.OK, MessageBoxIcon.Error)

	'		End Try
	'		Me.SQL4Print = Me.txt_SQLQuery.Text

	'	Catch ex As Exception
	'		DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message.ToString, strMethodeName, MessageBoxButtons.OK, MessageBoxIcon.Error)

	'	Finally

	'	End Try
	'	Me.bPrintAsDesign = False

	'	Try
	'		'Select Case e.Item.Name.ToUpper
	'		'	Case "PrintList".ToUpper
	'		'		Me.bPrintAsDesign = False

	'		'		'Case "printdesign".ToUpper
	'		'		'	Me.bPrintAsDesign = True

	'		'	Case Else
	'		'		Exit Sub

	'		'End Select
	'		StartPrinting()


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub

	''' <summary>
	''' starts printing list
	''' </summary>
	''' <remarks></remarks>
	Sub StartPrinting()
		bPrintAsDesign = IsUserActionAllowed(m_InitialData.UserData.UserNr, 560) AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		Dim strFilter As String = String.Empty

		Me.SQL4Print = Me.txt_SQLQuery.Text
		Me.PrintJobNr = "12.7"
		If Me.Cbo_MonatVon.EditValue <> Me.Cbo_MonatBis.EditValue And Me.Cbo_MonatBis.Visible Then Me.PrintJobNr &= ".G"

		strFilter &= String.Format("Mandant: {0}", m_SearchCriteria.mandantenname) & vbNewLine
		strFilter &= If(m_SearchCriteria.lohnarten.Length > 0, String.Format("Lohnarten: {0}", m_SearchCriteria.lohnarten, vbNewLine), String.Empty)

		strFilter &= If(m_SearchCriteria.vonmonat.Length > 0, String.Format("{1}Zeitraum: {0}", m_SearchCriteria.vonmonat, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.vonjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.vonjahr, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.bismonat.Length > 0, String.Format(" - {0}", m_SearchCriteria.bismonat), String.Empty)
		strFilter &= If(m_SearchCriteria.bisjahr.Length > 0, String.Format(" / {0}", m_SearchCriteria.bisjahr), String.Empty)

		strFilter &= If(m_SearchCriteria.manr.Length > 0, String.Format("{1}Kandidaten: {0}", m_SearchCriteria.manr, vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.getfirstes, String.Format("{0}1. Einsatz im Jahr? Ja", vbNewLine), String.Empty)
		strFilter &= If(m_SearchCriteria.deletenull, String.Format("{0}0-Beträge ausblenden? Ja", vbNewLine), String.Empty)


		Dim _Setting As New ClsLLBVGSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																												 .SelectedMDNr = m_InitializationData.MDData.MDNr,
																												 .LogedUSNr = m_InitializationData.UserData.UserNr,
																												 .SelectedMDYear = m_SearchCriteria.vonjahr,
																												 .SQL2Open = Me.SQL4Print,
																												 .JobNr2Print = Me.PrintJobNr,
																												 .ShowAsDesign = Me.bPrintAsDesign,
																												 .LLSelektionText = String.Empty,
																												 .frmhwnd = GetHwnd,
																												 .ListFilterBez = New List(Of String)(New String() {String.Format("{0}", strFilter)
																																																					 })
																												}

		Dim obj As New BVGSearchListing.ClsPrintBVGSearchList(_Setting)
		obj.PrintBVGListing()

	End Sub


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
				'Dim ExportThread As New Thread(AddressOf StartExportModul)
				'ExportThread.Name = "ExportBVGToCSV"
				'ExportThread.Start()
				StartExportModul()

		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																			 .SQL2Open = Me.txt_SQLQuery.Text,
																																			 .ModulName = "BVGTOCSV"}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		obj.ExportCSVFromBVGListing(Me.txt_SQLQuery.Text)

	End Sub


	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANR.ButtonClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim frmTest As New frmSearchRec("MA-Nr.")

		Dim m As String

		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iListeValue(ClsDataDetail.strBVGListeData)
		Me.txt_MANR.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace(strValueSeprator, ",")))
		frmTest.Dispose()


	End Sub

	Private Sub OnCboLohnart_DropDown(sender As Object, e As System.EventArgs) Handles Cbo_Lohnart.QueryPopUp
		LoadLAList()
	End Sub

	Private Sub LoadLAList()

		Dim jahrvon As Integer? = Val(ReplaceMissing(Cbo_VonJahr.EditValue, Now.Year))
		Dim jahrbis As Integer? = Val(ReplaceMissing(Cbo_BisJahr.EditValue, jahrvon))
		Dim monatvon As Integer? = Val(ReplaceMissing(Cbo_MonatVon.EditValue, Now.Month))
		Dim monatbis As Integer? = Val(ReplaceMissing(Cbo_MonatBis.EditValue, monatvon))

		ListBVGListLohnarten(Me.Cbo_Lohnart, jahrvon, jahrbis, monatvon, monatbis)

	End Sub

	Private Sub Cbo_Periode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbo_Periode.SelectedIndexChanged
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim dauer As Integer = 0
			Dim datum As Date = Date.Now
			Dim index1 As Integer = datum.Month - 1
			Dim bSetSelectedDate As Boolean = True
			Me.Cbo_MonatBis.Visible = False
			Me.SwitchButton1.Value = False

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

				Me.Cbo_VonJahr.EditValue = datum.Year
				Me.Cbo_BisJahr.EditValue = datum.Year

				Me.Cbo_MonatVon.EditValue = index1 + 1
				Me.Cbo_MonatBis.EditValue = index1 + 1 + dauer
				Me.Cbo_MonatBis.Visible = True

			Else
				FillDefaultDates()

			End If
			Cbo_Lohnart.EditValue = Nothing
			LoadLAList()


		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.Cbo_MonatBis.Visible = Me.SwitchButton1.Value
		Me.Cbo_MonatBis.EditValue = Nothing
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.Cbo_BisJahr.Visible = Me.SwitchButton2.Value
		Me.Cbo_BisJahr.EditValue = Nothing
	End Sub

End Class

